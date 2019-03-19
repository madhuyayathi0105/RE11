using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using BalAccess;

using System.Data.SqlClient;

using Gios.Pdf;
using System.IO;


public partial class AnonymousDepartmentwiseReport : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ReuasableMethods rs = new ReuasableMethods();
    Boolean cellflag = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        if (!IsPostBack)
        {
            bindclg();
            bindBatch();
            BindDegree();
            binddept();
            bindsem();
            bindsec();
            bindstaff();
            bindfeedback();
            rdb_deptwise.Checked = true;
            txtfeedbackmulti.Visible = false;
            Panel5.Visible = false;
            cbmul.Visible = false;
            rdbanonyomous.Checked = true;
            rdbloginbased.Checked = false;
        }
    }

    #region Filter Events

    protected void cbl_clgname_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_clgname, cbl_clgname, txtclgname, "College");
        BindDegree();
        binddept();
        bindfeedback();
        bindsem();
        bindsec();
        bindstaff();
    }

    protected void cb_clgname_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_clgname, cbl_clgname, txtclgname, "College", "--Select--");
        binddept();
        bindfeedback();
        bindsec();
        bindstaff();
    }

    protected void cbl_deptname_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_deptname, cbl_deptname, txtdeptname, "Department");
        bindstaff();
        bindfeedback();
        bindsem();
        bindsec();
    }

    protected void cb_deptname_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_deptname, cbl_deptname, txtdeptname, "Department", "--Select--");
        bindstaff();
        bindfeedback();
        bindsem();
        bindsec();
    }

    protected void ddlformate6_deptname_selectedindex(object sender, EventArgs e)
    {
        bindstaff();
        bindfeedback();
        bindsem();
    }

    protected void ddl_feedback_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (cbIndividual.Checked == true)
        //{
        //    bindstaffsubject();
        //}
        //else
        //{
        bindstaff();
        bindsubject();
        // }

        //bindsubjectformate6();
    }

    protected void cbl_staffname_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_staffname, cbl_staffname, txtstaffname, "Staff");
        //if (cbIndividual.Checked == true)
        //{
        //    bindstaffsubject();
        //}
        //bindsubjectformate6();
    }

    protected void cb_staffname_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_staffname, cbl_staffname, txtstaffname, "Staff", "--Select--");
        //bindstaffsubject();
        //bindsubjectformate6();
    }

    protected void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, "Batch");
    }

    protected void cb_batch_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
    }

    protected void cb_sem_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
    }

    protected void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, "Semester");
    }

    public void cb_sec_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_sec.Text = "--Select--";
            if (cb_sec.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = true;
                }
                txt_sec.Text = "Semester(" + (cbl_sec.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = false;
                }
                txt_sec.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    public void cbl_sec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_sec.Text = "--Select--";
            cb_sec.Checked = false;
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_sec.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sec.Items.Count)
                {
                    cb_sec.Checked = true;
                }
                txt_sec.Text = "Section(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst)
    {
        try
        {
            int sel = 0;
            int count = 0;
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
            else
            {
                txt.Text = "--Select--";
            }
        }
        catch { }
    }

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }

    #endregion

    #region Bind Methods

    protected void bindsem()
    {
        string branch = rs.GetSelectedItemsValueAsString(cbl_deptname);
        string collegecode = rs.GetSelectedItemsValueAsString(cbl_clgname);
        cbl_sem.Items.Clear();
        txt_sem.Text = "--Select--";
        if (!string.IsNullOrEmpty(collegecode))
        {
            //string query = " select distinct  MAX( ndurations)as ndurations from ndegree where Degree_code in('" + branch + "') union select distinct  MAX(duration) as ndurations  from degree where Degree_Code in('" + branch + "') ";
            //ds = d2.select_method_wo_parameter(query, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    cbl_sem.Items.Clear();
            //    string sem = Convert.ToString(ds.Tables[0].Rows[0]["ndurations"]);
            //    for (int j = 1; j <= Convert.ToInt32(sem); j++)
            //    {
            //        cbl_sem.Items.Add(new System.Web.UI.WebControls.ListItem(j.ToString(), j.ToString()));
            //        cbl_sem.Items[j - 1].Selected = true;
            //        cb_sem.Checked = true;
            //    }
            //    txt_sem.Text = "Semester(" + sem + ")";
            //}
            string max = d2.GetFunction("select  distinct MAX(duration) from degree where college_code='" + collegecode + "'  ");
            if (Convert.ToInt32(max) > 0)
            {
                cbl_sem.Items.Clear();
                for (int row = 0; row < Convert.ToInt32(max); row++)
                {
                    cbl_sem.Items.Add(new System.Web.UI.WebControls.ListItem((row + 1).ToString(), (row + 1).ToString()));
                    cbl_sem.Items[row].Selected = true;
                    cb_sem.Checked = true;
                }
                txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
            }
        }
    }

    public void bindsec()
    {
        try
        {
            cbl_sec.Items.Clear();
            txt_sec.Text = "---Select---";
            cb_sec.Checked = false;
            string batch = rs.GetSelectedItemsValueAsString(cbl_batch);
            string branchcode1 = rs.GetSelectedItemsValueAsString(cbl_deptname);
            if (batch != "" && branchcode1 != "")
            {
                //ds = d2.BindSectionDetail(batch, branchcode1);
                ds = d2.select_method_wo_parameter("select distinct sections from registration where batch_year in('" + batch + "')  and sections<>'-1' and ltrim(sections)<>'' and sections is not null and delflag=0 and exam_flag<>'Debar' and CC=0 order by Sections", "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sec.DataSource = ds;
                    cbl_sec.DataTextField = "sections";
                    cbl_sec.DataValueField = "sections";
                    cbl_sec.DataBind();
                    if (cbl_sec.Items.Count > 0)
                    {
                        cbl_sec.Items.Add(new ListItem("Empty", " "));
                        for (int row = 0; row < cbl_sec.Items.Count; row++)
                        {
                            cbl_sec.Items[row].Selected = true;
                            cb_sec.Checked = true;
                        }
                        txt_sec.Text = "Section(" + cbl_sec.Items.Count + ")";

                    }
                }
                else
                {
                    cbl_sec.Items.Add(new ListItem("Empty", " "));
                    for (int row = 0; row < cbl_sec.Items.Count; row++)
                    {
                        cbl_sec.Items[row].Selected = true;
                        cb_sec.Checked = true;
                    }
                    txt_sec.Text = "Section(" + cbl_sec.Items.Count + ")";
                }
            }
            else
            {
                cbl_sec.Items.Add(new ListItem("Empty", " "));
                for (int row = 0; row < cbl_sec.Items.Count; row++)
                {
                    cbl_sec.Items[row].Selected = true;
                    cb_sec.Checked = true;
                }
                txt_sec.Text = "Section(" + cbl_sec.Items.Count + ")";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Feedback Report");
        }
    }

    protected void binddept()
    {
        try
        {
            if (rdb_classwise.Checked == true)
            {

                cbl_deptname.Items.Clear();
                string college_cd = rs.GetSelectedItemsValueAsString(cbl_clgname);
                string course_id = rs.GetSelectedItemsValueAsString(cbl_degree);
                string query = "";
                if (course_id != "" && college_cd != "")
                {
                    ds.Clear();
                    query = " select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + course_id + "') and degree.college_code in ('" + college_cd + "')";
                    ds = d2.select_method_wo_parameter(query, "Text");

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cbl_deptname.DataSource = ds;
                        cbl_deptname.DataTextField = "dept_name";
                        cbl_deptname.DataValueField = "degree_code";
                        cbl_deptname.DataBind();
                        if (cbl_deptname.Items.Count > 0)
                        {
                            cbl_deptname.Items[0].Selected = true;

                            txtdeptname.Text = "Department(1)";
                        }
                    }
                }
                else
                {
                    cb_deptname.Checked = false;
                    txtdeptname.Text = "--Select--";
                }
            }
            else
            {
                ds.Clear();
                cbl_deptname.Items.Clear();
                string college_cd = rs.GetSelectedItemsValueAsString(cbl_clgname);
                //string query = " select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.college_code in ('" + college_cd + "')";
                string query = " select Dept_Code,Dept_Name from Department where college_code in ('" + college_cd + "') order by Dept_Name";
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //ddlformate6_deptname.DataSource = ds;
                    //ddlformate6_deptname.DataTextField = "Dept_Name";
                    //ddlformate6_deptname.DataValueField = "Dept_Code";
                    //ddlformate6_deptname.DataBind();
                    cbl_deptname.DataSource = ds;
                    cbl_deptname.DataTextField = "Dept_Name";
                    cbl_deptname.DataValueField = "Dept_Code";
                    cbl_deptname.DataBind();
                    if (cbl_deptname.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_deptname.Items.Count; row++)
                        {
                            cbl_deptname.Items[row].Selected = true;
                        }
                        cb_deptname.Checked = true;
                        txtdeptname.Text = "Department(" + cbl_deptname.Items.Count + ")";
                    }
                }
                else
                {
                    txtdeptname.Text = "--Select--";
                    //ddlformate6_deptname.Items.Add(new System.Web.UI.WebControls.ListItem("Select", "0"));
                }
            }
        }
        catch { }
    }

    protected void bindfeedback()
    {
        try
        {
            ddl_feedback.Items.Clear();
            cblfeedbackmulti.Items.Clear();
            collegecode = "";
            collegecode = rs.GetSelectedItemsValueAsString(cbl_clgname);
            string DeptCode = rs.GetSelectedItemsValueAsString(cbl_deptname);
            //string degreecode = Convert.ToString(ddlformate6_deptname.SelectedItem.Value);
            string batchyear = rs.GetSelectedItemsValueAsString(cbl_batch);
            ds.Clear();
            string q1 = ""; string empty = "";
            if (DeptCode.Trim() != "")
            {
                // q1 = " select d.Degree_Code from Degree d,Department dt where d.Dept_Code =dt.Dept_Code and d.Dept_Code in('" + DeptCode + "')";
                q1 = " select d.Degree_Code from Degree d,Department dt where d.Dept_Code =dt.Dept_Code and d.Degree_Code in('" + DeptCode + "')";
                ds = d2.select_method_wo_parameter(q1, "text");
                empty = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (empty == "")
                        {
                            empty = Convert.ToString(ds.Tables[0].Rows[i]["Degree_Code"]);
                        }
                        else
                        {
                            empty = empty + "','" + Convert.ToString(ds.Tables[0].Rows[i]["Degree_Code"]);
                        }
                    }
                }
            }
            q1 = "";
            if (empty.Trim() == "")
            {
                q1 = "select distinct FeedBackName  from CO_FeedBackMaster where CollegeCode in('" + collegecode + "')  and FeedBackType='1' and Batch_Year in('" + batchyear + "') ";
            }
            else
            {
                q1 = "select distinct FeedBackName  from CO_FeedBackMaster where CollegeCode in('" + collegecode + "') and DegreeCode in('" + empty + "') and FeedBackType='1' and Batch_Year in('" + batchyear + "') ";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (cbmul.Checked == false)
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    ds.Clear();
                    q1 = " select distinct FeedBackName  from CO_FeedBackMaster where CollegeCode in('" + collegecode + "') ";
                    ds = d2.select_method_wo_parameter(q1, "Text");
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_feedback.DataSource = ds;
                    ddl_feedback.DataTextField = "FeedBackName";
                    ddl_feedback.DataValueField = "FeedBackName";
                    ddl_feedback.DataBind();
                    ddl_feedback.Items.Insert(0, "--Select--");
                }
                else
                {
                    ddl_feedback.Items.Insert(0, "--Select--");
                }
            }
            if (cbmul.Checked == true)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblfeedbackmulti.DataSource = ds;
                    cblfeedbackmulti.DataTextField = "FeedBackName";
                    cblfeedbackmulti.DataValueField = "FeedBackName";
                    cblfeedbackmulti.DataBind();
                }
                if (cblfeedbackmulti.Items.Count > 0)
                {
                    for (int row = 0; row < cblfeedbackmulti.Items.Count; row++)
                    {
                        cblfeedbackmulti.Items[row].Selected = true;
                        cbfeedbackmulti.Checked = true;
                    }
                    txtfeedbackmulti.Text = "FeedBack(" + cblfeedbackmulti.Items.Count + ")";
                }
                else
                {
                    txtfeedbackmulti.Text = "--Select--";
                }

            }
        }
        catch { }
    }

    protected void bindstaff()
    {
        try
        {
            ds.Clear();
            cbl_staffname.Items.Clear();
            //if (ddlformate6_deptname.SelectedItem.Text.Trim() != "0")
            //{
            //    degreecode = Convert.ToString(ddlformate6_deptname.SelectedItem.Value);
            //}
            string degreecode = rs.GetSelectedItemsValueAsString(cbl_deptname);
            string query = " select s.staff_code,s.staff_name,sa.appl_id from staff_appl_master sa,staffmaster s,stafftrans t where sa.appl_no =s.appl_no and s.staff_code =t.staff_code and t.latestrec =1 and s.resign =0 and s.settled =0 and t.dept_code in ('" + degreecode + "') order by s.staff_name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_staffname.DataSource = ds;
                cbl_staffname.DataTextField = "staff_name";
                cbl_staffname.DataValueField = "appl_id";
                cbl_staffname.DataBind();
                if (cbl_staffname.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_staffname.Items.Count; row++)
                    {
                        cbl_staffname.Items[row].Selected = true;
                    }
                    cb_staffname.Checked = true;
                    txtstaffname.Text = "Staff(" + cbl_staffname.Items.Count + ")";
                }
            }
            else
            {
                txtstaffname.Text = "--Select--";
            }
        }
        catch { }
    }

    public void bindclg()
    {
        try
        {
            ds.Clear();
            cbl_clgname.Items.Clear();
            string clgname = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(clgname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_clgname.DataSource = ds;
                cbl_clgname.DataTextField = "collname";
                cbl_clgname.DataValueField = "college_code";
                cbl_clgname.DataBind();
            }
            BindDegree();
            bindfeedback();
            binddept();
            if (cbl_clgname.Items.Count > 0)
            {
                for (int row = 0; row < cbl_clgname.Items.Count; row++)
                {
                    cbl_clgname.Items[row].Selected = true;
                    cb_clgname.Checked = true;
                }
                txtclgname.Text = "College(" + cbl_clgname.Items.Count + ")";
            }
            else
            {
                txtclgname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }

    void bindBatch()
    {
        ds = d2.BindBatch();
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_batch.DataSource = ds;
            cbl_batch.DataTextField = "batch_year";
            cbl_batch.DataValueField = "batch_year";
            cbl_batch.DataBind();
        }
        if (cbl_batch.Items.Count > 0)
        {
            for (int row = 0; row < cbl_batch.Items.Count; row++)
            {
                cbl_batch.Items[row].Selected = true;
                cb_batch.Checked = true;
            }
            txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
        }
        else
        {
            txt_batch.Text = "--Select--";
        }
    }

    #endregion

    #region Button Events

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdbanonyomous.Checked == true)
            {
                #region DepartmentWise

                if (rdb_deptwise.Checked == true)
                {
                    FpSpread1.Sheets[0].AutoPostBack = true;

                    lbl_error.Visible = false;
                    Printcontrol1.Visible = false;
                    //string degreecode = rs.GetSelectedItemsValueAsString(cbl_deptname);
                    //string sem = rs.GetSelectedItemsValueAsString(cbl_sem);
                    //string batchyear = rs.GetSelectedItemsValueAsString(cbl_batch);
                    //string clgcode = rs.GetSelectedItemsValueAsString(cbl_clgname);
                    //string StaffAppID = rs.GetSelectedItemsValueAsString(cbl_staffname);
                    string degreecode = rs.GetSelectedItemsValue(cbl_deptname);
                    string sem = rs.GetSelectedItemsValue(cbl_sem);
                    string batchyear = rs.GetSelectedItemsValue(cbl_batch);
                    string clgcode = rs.GetSelectedItemsValue(cbl_clgname);
                    string StaffAppID = rs.GetSelectedItemsValue(cbl_staffname);
                    // string degree = rs.GetSelectedItemsValue(cbl_degree);
                    string sec = string.Empty;
                    for (int i = 0; i < cbl_sec.Items.Count; i++)
                    {
                        if (cbl_sec.Items[i].Selected == true)
                        {
                            if (string.IsNullOrEmpty(sec))
                                sec = cbl_sec.Items[i].Value.ToString();
                            else
                                sec = sec + "," + cbl_sec.Items[i].Value.ToString() + "";
                        }
                    }
                    if (!string.IsNullOrEmpty(degreecode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batchyear))
                    {
                        if (ddl_feedback.SelectedItem.Text.Trim() != "--Select--")
                        {
                            string type = "1";
                            string fbpk = " select FeedBackMasterPK,ISNULL(issubjecttype,0)issubjecttype from CO_FeedBackMaster where FeedBackName ='" + ddl_feedback.SelectedItem.Value + "'";// and DegreeCode in ('" + degreecode + "') and semester in ('" + sem + "') and Batch_Year in('" + batchyear + "') and section in ('" + sec + "')";
                            DataSet dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                            string feedbakpk = string.Empty;
                            string issubjecttype = string.Empty;
                            if (dsfb.Tables.Count > 0)
                            {
                                if (dsfb.Tables[0].Rows.Count > 0)
                                {
                                    issubjecttype = Convert.ToString(dsfb.Tables[0].Rows[0]["issubjecttype"]);
                                    for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                                    {
                                        if (string.IsNullOrEmpty(feedbakpk))
                                            feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                        else
                                            feedbakpk = feedbakpk + "," + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                                    }
                                }
                            }
                            rs.Fpreadheaderbindmethod("S No-50/Department-200/StaffCode-100/StaffName-150/Subject Code-150/Subject Name-250/SubjectType-100", FpSpread1, "true");

                            string selqry = " select count( distinct S.FeedbackUnicode)Strength,SUM(M.Point)Points,(convert(varchar(10), f.Batch_Year)+'-'+co.Course_Name+'-'+ dt.dept_acronym+'-'+convert(varchar(10), f.Semester)+'-'+f.Section ) as department,c.subject_code, s.StaffApplNo,sm.staff_code +' - '+staff_name as staff ,f.FeedBackMasterPK,Batch_Year , f.semester,f.DegreeCode,f.Section,SubjectNo,c.subject_name,sm.staff_code,staff_name,c.acronym from CO_FeedBackMaster F,CO_StudFeedBack S,staff_appl_master sa,staffmaster sm ,subject c,Department dt,course co,Degree d  ,CO_MarkMaster M where M.MarkMasterPK =S.MarkMasterPK and  d.Degree_Code =f.degreecode and dt.Dept_Code =d.Dept_Code and co.Course_Id =d.Course_Id and c.subject_no=s.SubjectNo and sa.appl_no=sm.appl_no  and sa.appl_id=s.StaffApplNo and s.FeedBackMasterFK =f.FeedBackMasterPK  and f.Batch_Year in('" + batchyear + "') and f.semester in ('" + sem + "')  and isnull(f.Section,'') in ('" + sec + "') and f.InclueCommon='1' and s.FeedbackUnicode<>'' and f.FeedBackMasterPK in ('" + feedbakpk + "') and s.StaffApplNo in('" + StaffAppID + "') group by staff_code,staff_name, f.FeedBackMasterPK, StaffApplNo,Batch_Year, f.semester,f.DegreeCode ,f.Section,subject_name, SubjectNo,subject_code,Course_Name,dept_acronym,c.acronym order by sm.staff_name ";// and   f.degreecode in('" + degreecode + "')
                            selqry += " SELECT distinct Question,QuestionMasterPK,HeaderCode FROM CO_FeedBackMaster B,CO_QuestionMaster Q ,CO_FeedBackQuestions FB WHERE  b.FeedBackMasterPK =fb.FeedBackMasterFK and q.QuestionMasterPK =fb.QuestionMasterFK and  InclueCommon='1' and FeedBackType = '" + type + "' and B.FeedBackName='" + Convert.ToString(ddl_feedback.SelectedItem.Text) + "'  and B.CollegeCode in ('" + clgcode + "') order by HeaderCode";
                            //  and   b.degreecode in ('" + degreecode + "') //26.12.17 barath added
                            selqry += " SELECT StaffApplNo,sum(M.Point) as points,QuestionMasterfK,SubjectNo,isnull(b.Section,'')Section FROM CO_StudFeedBack F,CO_FeedBackMaster B,CO_MarkMaster M where F.FeedBackMasterFK = B.FeedBackMasterPK AND F.MarkMasterPK = M.MarkMasterPK AND  b.InclueCommon='1' and FeedBackType = '" + type + "' and B.FeedBackMasterpK in ('" + feedbakpk + "') and B.CollegeCode in ('" + clgcode + "') and b.Batch_Year in('" + batchyear + "') and b.semester in ('" + sem + "')  and isnull(b.Section,'') in ('" + sec + "') group by StaffApplNo,QuestionMasterfK,SubjectNo,isnull(b.Section,'')";
                            //selqry += " SELECT StaffApplNo,sum(M.Point) as points,QuestionMasterfK,SubjectNo FROM CO_StudFeedBack F,CO_FeedBackMaster B,CO_MarkMaster M,CO_FeedbackUniCode fu WHERE fu.FeedbackUnicode=f.FeedbackUnicode and fu.FeedbackMasterFK=f.FeedBackMasterFK and F.FeedBackMasterFK = B.FeedBackMasterPK AND F.MarkMasterPK = M.MarkMasterPK AND  b.InclueCommon='1' and FeedBackType = '" + type + "' and B.FeedBackName ='" + Convert.ToString(ddl_feedback.SelectedItem.Text) + "' and B.CollegeCode in ('" + clgcode + "') and b.Batch_Year in('" + batchyear + "') and b.semester in ('" + sem + "')  and isnull(b.Section,'') in ('" + sec + "') group by StaffApplNo,QuestionMasterfK,SubjectNo,isnull(b.Section,'')";//,isnull(b.Section,'') 26.12.17
                            selqry += " select count(App_No)studentcount,degree_code,sections,college_code from Registration where  college_code in('" + clgcode + "') and isnull(Sections,'') in('" + sec + "') and cc=0 and delflag=0 and exam_flag<>'Debar' group by degree_code,college_code,sections ";//degree_code in('" + degreecode + "') and
                            selqry += " select COUNT( distinct QuestionMasterFK)question_count,isnull(SubjectType,'')SubjectType from CO_FeedBackQuestions where FeedBackMasterFK in ('" + feedbakpk + "') group by isnull(SubjectType,'')";
                            // ds = d2.select_method_wo_parameter(selqry, "Text");

                            Hashtable hat = new Hashtable();
                            hat.Add("@CollegeCode", clgcode);
                            hat.Add("@batchyear", batchyear);
                            hat.Add("@Degreecode", degreecode);
                            hat.Add("@semester", sem);
                            hat.Add("@section", sec);
                            hat.Add("@FeedbackName", Convert.ToString(ddl_feedback.SelectedItem.Text));
                            hat.Add("@FeedbackMasterFK", feedbakpk);
                            hat.Add("@StaffAppNo", StaffAppID);
                            hat.Add("@FeedbackType", type);
                            ds = d2.select_method("AnonymousDepartmentwiseReport", hat, "sp");
                            //string question_count = d2.GetFunction("select COUNT( distinct QuestionMasterFK)question_count from CO_FeedBackQuestions where FeedBackMasterFK in ('" + feedbakpk + "')");
                            double question_count = 0;
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[4].Rows.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(ds.Tables[4].Compute("sum(question_count)", "")), out question_count);
                                }
                                string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                    {
                                        FpSpread1.Sheets[0].ColumnCount++;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[1].Rows[i]["QuestionMasterPK"]);
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 300;
                                    }
                                    FpSpread1.Sheets[0].ColumnCount++;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Student Total";
                                    FpSpread1.Sheets[0].ColumnCount++;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Maximum Total";
                                    FpSpread1.Sheets[0].ColumnCount++;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Percentage";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Bold = true;
                                }
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    int k = 0; string staffname = ""; int s = 1;
                                    FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                                    cb.AutoPostBack = true;
                                    FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                                    cb1.AutoPostBack = false;
                                    //FpSpread1.Sheets[0].RowCount++;
                                    double staffavg = 0; bool staffinvdiavg = false; double sumavgpoint = 0; int staffrowcount = 0;
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        staffinvdiavg = false;
                                        FpSpread1.Sheets[0].RowCount++;
                                        if (staffname.Trim() == "")
                                        { k++; }
                                        else if (staffname == ds.Tables[0].Rows[i]["staff_name"].ToString())
                                        { k++; staffrowcount++; }
                                        else
                                        {
                                            k = 1; s++;
                                            FpSpread1.Sheets[0].RowCount++;
                                            //staffavg = (staffavg / Convert.ToDouble(staffrowcount+1));
                                            staffavg = ((staffavg / (Convert.ToDouble(staffrowcount + 1) * 100)) * 100);
                                            double.TryParse(Convert.ToString(Math.Round(staffavg, 2)), out sumavgpoint);
                                            staffinvdiavg = true;
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(s);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["FeedBackMasterPK"]);


                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["department"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]);
                                        staffname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                                        string staff_codeName = ds.Tables[0].Rows[i]["staff"].ToString();
                                        string staff_Code = string.Empty;
                                        string staff_Name = string.Empty;

                                        if (staff_codeName.Contains("-"))
                                        {
                                            string[] splitval = staff_codeName.Split('-');
                                            staff_Code = Convert.ToString(splitval[0]);
                                            staff_Name = Convert.ToString(splitval[1]);

                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = staff_Code;//k.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = staff_Name;//k.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["subject_code"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["Subject_Name"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["acronym"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;


                                        double gtotal = 0; double mtotal = 0; double avgper = 0;
                                        string filterquery = string.Empty;
                                        string section = Convert.ToString(ds.Tables[0].Rows[i]["Section"]);
                                        filterquery = "degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["DegreeCode"]) + "' ";
                                        if (section.Trim() != "")
                                        {
                                            filterquery = filterquery + " and Sections='" + section + "'";
                                        }
                                        ds.Tables[3].DefaultView.RowFilter = "" + filterquery + "";
                                        DataView dvnew = ds.Tables[3].DefaultView;
                                        string totalstudnent = "";
                                        if (dvnew.Count > 0)
                                        {
                                            totalstudnent = Convert.ToString(dvnew[0]["studentcount"]);
                                        }
                                        if (totalstudnent.Trim() == "")
                                            totalstudnent = "0";
                                        //double maximun = Convert.ToDouble(question_count) * Convert.ToDouble(sum_total) * Convert.ToDouble(totalstudnent);
                                        Double attendstrength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                        double maximun = Convert.ToDouble(sum_total) * Convert.ToDouble(attendstrength);
                                        double QuestionAttendcount = 0;
                                        for (int j = 8; j <= FpSpread1.Columns.Count - 3; j++)
                                        {
                                            string questionmasterPK = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, j - 1].Tag);
                                            ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";//and FeedbackUnicode='" + ds.Tables[0].Rows[i]["FeedbackUnicode"] + "'
                                            DataView dv = ds.Tables[2].DefaultView;
                                            if (dv.Count > 0)
                                            {
                                                QuestionAttendcount++;
                                                string point1 = Convert.ToString(dv[0]["points"]);
                                                if (string.IsNullOrEmpty(point1.Trim()) || point1.Trim() == "-")
                                                    point1 = "0";
                                                double questavgpoint = Convert.ToDouble(point1) / maximun * Convert.ToDouble(sum_total);// 100;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(Math.Round(questavgpoint, 2)); //Convert.ToString(dv[0]["points"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                gtotal += Convert.ToDouble((Math.Round(questavgpoint, 2)));
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = "-";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                            }
                                            FpSpread1.Columns[j - 1].Locked = true;
                                            FpSpread1.Columns[4].Locked = true;
                                        }
                                        //Double strength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                        //double calfbcal = Convert.ToDouble(totalstudnent) * Convert.ToDouble(question_count) * Convert.ToDouble(sum_total);
                                        //double fbavg = (gtotal / calfbcal) * 100;
                                        //double avg = Convert.ToDouble(Math.Round(fbavg, 2));
                                        //string studentcount = "";
                                        //if (Convert.ToString(ds.Tables[0].Rows[i]["Strength"]).Trim() != "")
                                        //{
                                        //    studentcount = Convert.ToString(ds.Tables[0].Rows[i]["Strength"]);
                                        //}
                                        //else
                                        //{
                                        //    studentcount = "-";
                                        //}
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].CellType = new FarPoint.Web.Spread.TextCellType();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Text = Convert.ToString(String.Format("{0:0.00}", Math.Round(gtotal, 2)));
                                        if (issubjecttype == "1" || issubjecttype.ToUpper() == "TRUE")
                                        {
                                            question_count = QuestionAttendcount;
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = Convert.ToString(Math.Round((question_count * Convert.ToDouble(sum_total)), 2));
                                        double avg = (Math.Round(Math.Round(gtotal, 2) / Math.Round((question_count * Convert.ToDouble(sum_total)), 2) * 100, 2));
                                        //barath 31.07.17 *100 added
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(avg, 2));
                                        staffavg += avg;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(avg);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Bold = true;
                                        if (staffinvdiavg == true)
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(sumavgpoint, 2));

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].ForeColor = System.Drawing.Color.BlueViolet;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                            sumavgpoint = 0;
                                            staffrowcount = 0; staffavg = 0; staffavg += avg;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Text = "Average";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].ForeColor = System.Drawing.Color.BlueViolet;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                    FpSpread1.Sheets[0].RowCount++;
                                    staffavg = ((staffavg / (Convert.ToDouble(staffrowcount + 1) * 100)) * 100);
                                    double.TryParse(Convert.ToString(Math.Round(staffavg, 2)), out sumavgpoint);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = Convert.ToString(String.Format("{0:0.00}", sumavgpoint));
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].ForeColor = System.Drawing.Color.BlueViolet;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = "Average";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].ForeColor = System.Drawing.Color.BlueViolet;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Columns[FpSpread1.Columns.Count - 1].Locked = true;
                                    FpSpread1.Columns[FpSpread1.Columns.Count - 2].Locked = true;
                                    FpSpread1.Columns[FpSpread1.Columns.Count - 3].Locked = true;
                                    FpSpread1.Columns[0].Locked = true;
                                    FpSpread1.Columns[1].Locked = true;
                                    FpSpread1.Columns[2].Locked = true;
                                    FpSpread1.Columns[3].Locked = true;
                                    FpSpread1.Columns[4].Locked = true;
                                    FpSpread1.Columns[5].Locked = true;
                                    FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    FpSpread1.Height = 500;
                                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                    SpreadDiv.Visible = true;
                                }
                                else
                                {
                                    lbl_error.Visible = true;
                                    lbl_error.Text = "No Records Found";
                                    SpreadDiv.Visible = false;
                                }
                            }
                            else
                            {
                                lbl_error.Visible = true;
                                lbl_error.Text = "No Records Found";
                                SpreadDiv.Visible = false;
                            }
                        }
                        else
                        {
                            SpreadDiv.Visible = false;
                            lbl_error.Visible = true;
                            lbl_error.Text = "Please Select Feedback Name";
                        }
                    }
                    else
                    {
                        SpreadDiv.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "Please select all fields";
                    }


                }

                #endregion

                #region ClassWise
                if (cbmul.Checked == false)
                {
                    if (Rdbques.Checked == true)
                    {

                        if (rdb_classwise.Checked == true)//delsi1903
                        {
                            #region ClassWise With RoundOff

                            if (cb_WithOutRoundOff.Checked == false)
                            {
                                lbl_error.Visible = false;
                                Printcontrol1.Visible = false;
                                string degreecode = rs.GetSelectedItemsValue(cbl_deptname);
                                string sem = rs.GetSelectedItemsValue(cbl_sem);
                                string batchyear = rs.GetSelectedItemsValue(cbl_batch);
                                string clgcode = rs.GetSelectedItemsValue(cbl_clgname);
                                //   string StaffAppID = rs.GetSelectedItemsValue(cbl_staffname);
                                //   string degree = rs.GetSelectedItemsValue(cbl_degree);
                                string subjectcode = rs.GetSelectedItemsValue(Cbl_Subject);

                                string sec = string.Empty;
                                for (int i = 0; i < cbl_sec.Items.Count; i++)
                                {
                                    if (cbl_sec.Items[i].Selected == true)
                                    {
                                        if (string.IsNullOrEmpty(sec))
                                            sec = cbl_sec.Items[i].Value.ToString();
                                        else
                                            sec = sec + "," + cbl_sec.Items[i].Value.ToString() + "";
                                    }
                                }
                                if (!string.IsNullOrEmpty(degreecode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batchyear))
                                {
                                    if (ddl_feedback.SelectedItem.Text.Trim() != "--Select--")
                                    {
                                        string type = "1";
                                        string fbpk = " select FeedBackMasterPK,ISNULL(issubjecttype,0)issubjecttype from CO_FeedBackMaster where FeedBackName ='" + ddl_feedback.SelectedItem.Value + "'";
                                        DataSet dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                                        string feedbakpk = string.Empty;
                                        string issubjecttype = string.Empty;
                                        if (dsfb.Tables.Count > 0)
                                        {
                                            if (dsfb.Tables[0].Rows.Count > 0)
                                            {
                                                issubjecttype = Convert.ToString(dsfb.Tables[0].Rows[0]["issubjecttype"]);
                                                for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                                                {
                                                    if (string.IsNullOrEmpty(feedbakpk))
                                                        feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                                    else
                                                        feedbakpk = feedbakpk + "," + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                                                }
                                            }
                                        }
                                        //rs.Fpreadheaderbindmethod("S No-50/Department-200/StaffCode & StaffName-250/Subject Code-150/Subject Name-250/SubjectType-100", FpSpread1, "false");

                                        //Modified by saranya on 20/08/2018
                                        rs.Fpreadheaderbindmethod("S No-50/Department-200/StaffCode-100/StaffName-200/Subject Code-150/Subject Name-250/SubjectType-100", FpSpread1, "false");
                                        Hashtable hat = new Hashtable();
                                        hat.Add("@CollegeCode", clgcode);
                                        hat.Add("@batchyear", batchyear);
                                        hat.Add("@Degreecode", degreecode);
                                        hat.Add("@semester", sem);
                                        hat.Add("@section", sec);
                                        hat.Add("@FeedbackName", Convert.ToString(ddl_feedback.SelectedItem.Text));
                                        hat.Add("@FeedbackMasterFK", feedbakpk);
                                        //   hat.Add("@StaffAppNo", StaffAppID);
                                        hat.Add("@FeedbackType", type);
                                        hat.Add("@subjectno", subjectcode);
                                        ds = d2.select_method("[AnonymousReportClassWise]", hat, "sp");
                                        //string question_count = d2.GetFunction("select COUNT( distinct QuestionMasterFK)question_count from CO_FeedBackQuestions where FeedBackMasterFK in ('" + feedbakpk + "')");
                                        double question_count = 0;
                                        if (ds.Tables.Count > 0)
                                        {
                                            if (ds.Tables[4].Rows.Count > 0)
                                            {
                                                double.TryParse(Convert.ToString(ds.Tables[4].Compute("sum(question_count)", "")), out question_count);
                                            }
                                            string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                            if (ds.Tables[1].Rows.Count > 0)
                                            {
                                                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                                {
                                                    FpSpread1.Sheets[0].ColumnCount++;
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[1].Rows[i]["QuestionMasterPK"]);
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                    FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 300;
                                                }
                                                //========Modified By saranya on 27/08/2018=======//
                                                FpSpread1.Sheets[0].ColumnCount++;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "No.Of Students";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                FpSpread1.Sheets[0].ColumnCount++;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Feedback Percentage";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                //==============================================//

                                                //FpSpread1.Sheets[0].ColumnCount++;
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Maximum Total";
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Name = "Book Antiqua";
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Bold = true;
                                            }
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                int k = 0; string staffname = ""; int s = 1;
                                                FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                                                cb.AutoPostBack = true;
                                                FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                                                cb1.AutoPostBack = false;
                                                //FpSpread1.Sheets[0].RowCount++;
                                                double staffavg = 0; bool staffinvdiavg = false; double sumavgpoint = 0; int staffrowcount = 0;
                                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                                {
                                                    staffinvdiavg = false;
                                                    FpSpread1.Sheets[0].RowCount++;
                                                    if (staffname.Trim() == "")
                                                    {
                                                        k++;
                                                    }
                                                    else if (staffname == ds.Tables[0].Rows[i]["staff_name"].ToString())
                                                    {
                                                        k++; staffrowcount++;
                                                    }
                                                    else
                                                    {
                                                        k = 1; s++;
                                                        //FpSpread1.Sheets[0].RowCount++;
                                                        //staffavg = (staffavg / Convert.ToDouble(staffrowcount+1));
                                                        staffavg = ((staffavg / (Convert.ToDouble(staffrowcount + 1) * 100)) * 100);
                                                        double.TryParse(Convert.ToString(Math.Round(staffavg, 2)), out sumavgpoint);
                                                        staffinvdiavg = true;
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(s);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["department"].ToString();
                                                    staffname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                                                    //Changed by saranya on 20/8/2018
                                                    string staff = Convert.ToString(ds.Tables[0].Rows[i]["staff"]);
                                                    string[] staffSplit = staff.Split('-');

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = staffSplit[0];//k.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = staffSplit[1];//k.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                                    ////////////////////////////////////

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["subject_code"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["Subject_Name"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["acronym"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = ds.Tables[0].Rows[i]["section"].ToString();
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Bold = true;

                                                    double gtotal = 0; double mtotal = 0; double avgper = 0;
                                                    string filterquery = string.Empty;
                                                    string section = Convert.ToString(ds.Tables[0].Rows[i]["Section"]);
                                                    filterquery = "degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["DegreeCode"]) + "' ";
                                                    if (section.Trim() != "")
                                                    {
                                                        filterquery = filterquery + " and Sections='" + section + "'";
                                                    }
                                                    ds.Tables[3].DefaultView.RowFilter = "" + filterquery + "";
                                                    DataView dvnew = ds.Tables[3].DefaultView;
                                                    string totalstudnent = "";
                                                    if (dvnew.Count > 0)
                                                    {
                                                        totalstudnent = Convert.ToString(dvnew[0]["studentcount"]);
                                                    }
                                                    if (totalstudnent.Trim() == "")
                                                        totalstudnent = "0";
                                                    //double maximun = Convert.ToDouble(question_count) * Convert.ToDouble(sum_total) * Convert.ToDouble(totalstudnent);
                                                    Double attendstrength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                    double maximun = Convert.ToDouble(sum_total) * Convert.ToDouble(attendstrength);
                                                    double QuestionAttendcount = 0;
                                                    for (int j = 8; j <= FpSpread1.Columns.Count - 2; j++)//modified by saranya on 27Aug2018 FpSpread1.Columns.Count changed to FpSpread1.Columns.Count - 2
                                                    {
                                                        string questionmasterPK = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, j - 1].Tag);
                                                        ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";//and FeedbackUnicode='" + ds.Tables[0].Rows[i]["FeedbackUnicode"] + "'
                                                        DataView dv = ds.Tables[2].DefaultView;
                                                        if (dv.Count > 0)
                                                        {
                                                            QuestionAttendcount++;
                                                            string point1 = Convert.ToString(dv[0]["points"]);
                                                            if (string.IsNullOrEmpty(point1.Trim()) || point1.Trim() == "-")
                                                                point1 = "0";
                                                            double questavgpoint = Convert.ToDouble(point1) / maximun * Convert.ToDouble(sum_total);// 100;
                                                            questavgpoint = Math.Round(questavgpoint, 0, MidpointRounding.AwayFromZero);
                                                            //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(Math.Round(questavgpoint, 2)); //Convert.ToString(dv[0]["points"]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(questavgpoint);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                            gtotal += Convert.ToDouble((Math.Round(questavgpoint, 2)));
                                                        }
                                                        else
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = "-";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                        }
                                                        FpSpread1.Columns[j - 1].Locked = true;
                                                        FpSpread1.Columns[4].Locked = true;
                                                    }
                                                    //Modified by saranya on 27Aug2018
                                                    Double strength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                    double calfbcal = Convert.ToDouble(attendstrength) * Convert.ToDouble(question_count) * Convert.ToDouble(sum_total);
                                                    double fbavg = (gtotal / calfbcal) * 100;
                                                    double avg = Convert.ToDouble(Math.Round(fbavg, 2));
                                                    string studentcount = "";
                                                    if (Convert.ToString(ds.Tables[0].Rows[i]["Strength"]).Trim() != "")
                                                    {
                                                        studentcount = Convert.ToString(ds.Tables[0].Rows[i]["Strength"]);
                                                    }
                                                    else
                                                    {
                                                        studentcount = "-";
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].CellType = new FarPoint.Web.Spread.TextCellType();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = Convert.ToString(studentcount);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";

                                                    double averag = (Math.Round(Math.Round(gtotal, 2) / Math.Round((question_count * Convert.ToDouble(sum_total)), 2) * 100, 2));
                                                    //barath 31.07.17 *100 added
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(averag, 2));
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                    //==========================================================================//

                                                    //if (issubjecttype == "1" || issubjecttype.ToUpper() == "TRUE")
                                                    //{
                                                    //    question_count = QuestionAttendcount;
                                                    //}
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = Convert.ToString(Math.Round((question_count * Convert.ToDouble(sum_total)), 2));
                                                    //double avg = (Math.Round(Math.Round(gtotal, 2) / Math.Round((question_count * Convert.ToDouble(sum_total)), 2) * 100, 2));
                                                    ////barath 31.07.17 *100 added
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(avg, 2));
                                                    //staffavg += avg;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].HorizontalAlign = HorizontalAlign.Right;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Name = "Book Antiqua";
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Bold = true;
                                                    //if (staffinvdiavg == true)
                                                    //{
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(sumavgpoint, 2));

                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].ForeColor = System.Drawing.Color.BlueViolet;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                    //    sumavgpoint = 0;
                                                    //    staffrowcount = 0; staffavg = 0; staffavg += avg;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Text = "Average";
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].ForeColor = System.Drawing.Color.BlueViolet;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                                                    //}
                                                }
                                                //FpSpread1.Sheets[0].RowCount++;
                                                //staffavg = ((staffavg / (Convert.ToDouble(staffrowcount + 1) * 100)) * 100);
                                                //double.TryParse(Convert.ToString(Math.Round(staffavg, 2)), out sumavgpoint);
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = Convert.ToString(String.Format("{0:0.00}", sumavgpoint));
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].ForeColor = System.Drawing.Color.BlueViolet;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].HorizontalAlign = HorizontalAlign.Right;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Name = "Book Antiqua";
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Bold = true;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = "Average";
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].ForeColor = System.Drawing.Color.BlueViolet;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                                                //FpSpread1.Columns[FpSpread1.Columns.Count - 1].Locked = true;
                                                //FpSpread1.Columns[FpSpread1.Columns.Count - 2].Locked = true;
                                                //FpSpread1.Columns[FpSpread1.Columns.Count - 3].Locked = true;
                                                //FpSpread1.Columns[0].Locked = true;
                                                //FpSpread1.Columns[1].Locked = true;
                                                //FpSpread1.Columns[2].Locked = true;
                                                //FpSpread1.Columns[3].Locked = true;
                                                //FpSpread1.Columns[4].Locked = true;
                                                //FpSpread1.Columns[5].Locked = true;
                                                FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                                                // FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                                                //FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                                                //FpSpread1.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                                FpSpread1.Height = 500;
                                                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                                SpreadDiv.Visible = true;
                                            }
                                            else
                                            {
                                                lbl_error.Visible = true;
                                                lbl_error.Text = "No Records Found";
                                                SpreadDiv.Visible = false;
                                            }
                                        }
                                        else
                                        {
                                            lbl_error.Visible = true;
                                            lbl_error.Text = "No Records Found";
                                            SpreadDiv.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        SpreadDiv.Visible = false;
                                        lbl_error.Visible = true;
                                        lbl_error.Text = "Please Select Feedback Name";
                                    }
                                }
                                else
                                {
                                    SpreadDiv.Visible = false;
                                    lbl_error.Visible = true;
                                    lbl_error.Text = "Please select all fields";
                                }
                            }

                            #endregion

                            #region Added By Saranya On 28/08/2018 For ClassWise Without RoundOff

                            if (cb_WithOutRoundOff.Checked == true)
                            {
                                lbl_error.Visible = false;
                                Printcontrol1.Visible = false;
                                string degreecode = rs.GetSelectedItemsValue(cbl_deptname);
                                string sem = rs.GetSelectedItemsValue(cbl_sem);
                                string batchyear = rs.GetSelectedItemsValue(cbl_batch);
                                string clgcode = rs.GetSelectedItemsValue(cbl_clgname);
                                string subjectcode = rs.GetSelectedItemsValue(Cbl_Subject);
                                string sec = string.Empty;
                                for (int i = 0; i < cbl_sec.Items.Count; i++)
                                {
                                    if (cbl_sec.Items[i].Selected == true)
                                    {
                                        if (string.IsNullOrEmpty(sec))
                                            sec = cbl_sec.Items[i].Value.ToString();
                                        else
                                            sec = sec + "," + cbl_sec.Items[i].Value.ToString() + "";
                                    }
                                }
                                if (!string.IsNullOrEmpty(degreecode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batchyear))
                                {
                                    if (ddl_feedback.SelectedItem.Text.Trim() != "--Select--")
                                    {
                                        string type = "1";
                                        string fbpk = " select FeedBackMasterPK,ISNULL(issubjecttype,0)issubjecttype from CO_FeedBackMaster where FeedBackName ='" + ddl_feedback.SelectedItem.Value + "'";
                                        DataSet dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                                        string feedbakpk = string.Empty;
                                        string issubjecttype = string.Empty;
                                        if (dsfb.Tables.Count > 0)
                                        {
                                            if (dsfb.Tables[0].Rows.Count > 0)
                                            {
                                                issubjecttype = Convert.ToString(dsfb.Tables[0].Rows[0]["issubjecttype"]);
                                                for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                                                {
                                                    if (string.IsNullOrEmpty(feedbakpk))
                                                        feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                                    else
                                                        feedbakpk = feedbakpk + "," + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                                                }
                                            }
                                        }
                                        rs.Fpreadheaderbindmethod("S No-50/Department-200/StaffCode-100/StaffName-200/Subject Code-150/Subject Name-250/SubjectType-100", FpSpread1, "false");
                                        Hashtable hat = new Hashtable();
                                        hat.Add("@CollegeCode", clgcode);
                                        hat.Add("@batchyear", batchyear);
                                        hat.Add("@Degreecode", degreecode);
                                        hat.Add("@semester", sem);
                                        hat.Add("@section", sec);
                                        hat.Add("@FeedbackName", Convert.ToString(ddl_feedback.SelectedItem.Text));
                                        hat.Add("@FeedbackMasterFK", feedbakpk);
                                        hat.Add("@FeedbackType", type);
                                        hat.Add("@subjectno", subjectcode);
                                        ds = d2.select_method("[AnonymousReportClassWise]", hat, "sp");

                                        double question_count = 0;
                                        if (ds.Tables.Count > 0)
                                        {
                                            if (ds.Tables[4].Rows.Count > 0)
                                            {
                                                double.TryParse(Convert.ToString(ds.Tables[4].Compute("sum(question_count)", "")), out question_count);
                                            }
                                            string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                            if (ds.Tables[1].Rows.Count > 0)
                                            {
                                                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                                {
                                                    FpSpread1.Sheets[0].ColumnCount++;
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[1].Rows[i]["QuestionMasterPK"]);
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                    FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 300;
                                                }
                                                FpSpread1.Sheets[0].ColumnCount++;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "No.Of Students";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                FpSpread1.Sheets[0].ColumnCount++;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Feedback Percentage";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                            }
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                int k = 0; string staffname = ""; int s = 1;
                                                FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                                                cb.AutoPostBack = true;
                                                FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                                                cb1.AutoPostBack = false;
                                                double staffavg = 0; bool staffinvdiavg = false; double sumavgpoint = 0; int staffrowcount = 0;
                                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                                {
                                                    staffinvdiavg = false;
                                                    FpSpread1.Sheets[0].RowCount++;
                                                    if (staffname.Trim() == "")
                                                    {
                                                        k++;
                                                    }
                                                    else if (staffname == ds.Tables[0].Rows[i]["staff_name"].ToString())
                                                    {
                                                        k++; staffrowcount++;
                                                    }
                                                    else
                                                    {
                                                        k = 1; s++;
                                                        //FpSpread1.Sheets[0].RowCount++;                                          
                                                        staffavg = ((staffavg / (Convert.ToDouble(staffrowcount + 1) * 100)) * 100);
                                                        double.TryParse(Convert.ToString(Math.Round(staffavg, 2)), out sumavgpoint);
                                                        staffinvdiavg = true;
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(s);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["department"].ToString();
                                                    staffname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                                    string staff = Convert.ToString(ds.Tables[0].Rows[i]["staff"]);
                                                    string[] staffSplit = staff.Split('-');

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = staffSplit[0];//k.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = staffSplit[1];//k.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["subject_code"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["Subject_Name"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["acronym"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;

                                                    double gtotal = 0; double mtotal = 0; double avgper = 0;
                                                    string filterquery = string.Empty;
                                                    string section = Convert.ToString(ds.Tables[0].Rows[i]["Section"]);
                                                    filterquery = "degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["DegreeCode"]) + "' ";
                                                    if (section.Trim() != "")
                                                    {
                                                        filterquery = filterquery + " and Sections='" + section + "'";
                                                    }
                                                    ds.Tables[3].DefaultView.RowFilter = "" + filterquery + "";
                                                    DataView dvnew = ds.Tables[3].DefaultView;
                                                    string totalstudnent = "";
                                                    if (dvnew.Count > 0)
                                                    {
                                                        totalstudnent = Convert.ToString(dvnew[0]["studentcount"]);
                                                    }
                                                    if (totalstudnent.Trim() == "")
                                                        totalstudnent = "0";

                                                    Double attendstrength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                    double maximun = Convert.ToDouble(sum_total) * Convert.ToDouble(attendstrength);
                                                    double QuestionAttendcount = 0;
                                                    for (int j = 8; j <= FpSpread1.Columns.Count - 2; j++)
                                                    {
                                                        string questionmasterPK = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, j - 1].Tag);
                                                        ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";
                                                        DataView dv = ds.Tables[2].DefaultView;
                                                        if (dv.Count > 0)
                                                        {
                                                            QuestionAttendcount++;
                                                            string point1 = Convert.ToString(dv[0]["points"]);
                                                            if (string.IsNullOrEmpty(point1.Trim()) || point1.Trim() == "-")
                                                                point1 = "0";
                                                            double questavgpoint = Convert.ToDouble(point1) / maximun * Convert.ToDouble(sum_total);// 100;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(Math.Round(questavgpoint, 2));
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                            gtotal += Convert.ToDouble((Math.Round(questavgpoint, 2)));
                                                        }
                                                        else
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = "-";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                        }
                                                        FpSpread1.Columns[j - 1].Locked = true;
                                                        FpSpread1.Columns[4].Locked = true;
                                                    }
                                                    Double strength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                    double calfbcal = Convert.ToDouble(attendstrength) * Convert.ToDouble(question_count) * Convert.ToDouble(sum_total);
                                                    double fbavg = (gtotal / calfbcal) * 100;
                                                    double avg = Convert.ToDouble(Math.Round(fbavg, 2));
                                                    string studentcount = "";
                                                    if (Convert.ToString(ds.Tables[0].Rows[i]["Strength"]).Trim() != "")
                                                    {
                                                        studentcount = Convert.ToString(ds.Tables[0].Rows[i]["Strength"]);
                                                    }
                                                    else
                                                    {
                                                        studentcount = "-";
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].CellType = new FarPoint.Web.Spread.TextCellType();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = Convert.ToString(studentcount);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";

                                                    double averag = (Math.Round(Math.Round(gtotal, 2) / Math.Round((question_count * Convert.ToDouble(sum_total)), 2) * 100, 2));
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(averag, 2));
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                }
                                                FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                                FpSpread1.Height = 500;
                                                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                                SpreadDiv.Visible = true;
                                            }
                                            else
                                            {
                                                lbl_error.Visible = true;
                                                lbl_error.Text = "No Records Found";
                                                SpreadDiv.Visible = false;
                                            }
                                        }
                                        else
                                        {
                                            lbl_error.Visible = true;
                                            lbl_error.Text = "No Records Found";
                                            SpreadDiv.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        SpreadDiv.Visible = false;
                                        lbl_error.Visible = true;
                                        lbl_error.Text = "Please Select Feedback Name";
                                    }
                                }
                                else
                                {
                                    SpreadDiv.Visible = false;
                                    lbl_error.Visible = true;
                                    lbl_error.Text = "Please select all fields";
                                }
                            }
                            #endregion
                        }
                    }
                    else if (Rdbquesacr.Checked == true)//delsi2510
                    {

                        if (rdb_classwise.Checked == true)//delsi1903
                        {
                            #region ClassWise With RoundOff

                            if (cb_WithOutRoundOff.Checked == false)
                            {
                                lbl_error.Visible = false;
                                Printcontrol1.Visible = false;
                                string degreecode = rs.GetSelectedItemsValue(cbl_deptname);
                                string sem = rs.GetSelectedItemsValue(cbl_sem);
                                string batchyear = rs.GetSelectedItemsValue(cbl_batch);
                                string clgcode = rs.GetSelectedItemsValue(cbl_clgname);
                                //   string StaffAppID = rs.GetSelectedItemsValue(cbl_staffname);
                                //   string degree = rs.GetSelectedItemsValue(cbl_degree);
                                string subjectcode = rs.GetSelectedItemsValue(Cbl_Subject);

                                string sec = string.Empty;
                                for (int i = 0; i < cbl_sec.Items.Count; i++)
                                {
                                    if (cbl_sec.Items[i].Selected == true)
                                    {
                                        if (string.IsNullOrEmpty(sec))
                                            sec = cbl_sec.Items[i].Value.ToString();
                                        else
                                            sec = sec + "," + cbl_sec.Items[i].Value.ToString() + "";
                                    }
                                }
                                if (!string.IsNullOrEmpty(degreecode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batchyear))
                                {
                                    if (ddl_feedback.SelectedItem.Text.Trim() != "--Select--")
                                    {
                                        string type = "1";
                                        string fbpk = " select FeedBackMasterPK,ISNULL(issubjecttype,0)issubjecttype from CO_FeedBackMaster where FeedBackName ='" + ddl_feedback.SelectedItem.Value + "'";
                                        DataSet dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                                        string feedbakpk = string.Empty;
                                        string issubjecttype = string.Empty;
                                        if (dsfb.Tables.Count > 0)
                                        {
                                            if (dsfb.Tables[0].Rows.Count > 0)
                                            {
                                                issubjecttype = Convert.ToString(dsfb.Tables[0].Rows[0]["issubjecttype"]);
                                                for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                                                {
                                                    if (string.IsNullOrEmpty(feedbakpk))
                                                        feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                                    else
                                                        feedbakpk = feedbakpk + "," + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                                                }
                                            }
                                        }
                                        //rs.Fpreadheaderbindmethod("S No-50/Department-200/StaffCode & StaffName-250/Subject Code-150/Subject Name-250/SubjectType-100", FpSpread1, "false");

                                        //Modified by saranya on 20/08/2018
                                        rs.Fpreadheaderbindmethod("S No-50/Department-200/StaffCode-100/StaffName-200/Subject Code-150/Subject Name-250/SubjectType-100", FpSpread1, "false");
                                        Hashtable hat = new Hashtable();
                                        hat.Add("@CollegeCode", clgcode);
                                        hat.Add("@batchyear", batchyear);
                                        hat.Add("@Degreecode", degreecode);
                                        hat.Add("@semester", sem);
                                        hat.Add("@section", sec);
                                        hat.Add("@FeedbackName", Convert.ToString(ddl_feedback.SelectedItem.Text));
                                        hat.Add("@FeedbackMasterFK", feedbakpk);
                                        //   hat.Add("@StaffAppNo", StaffAppID);
                                        hat.Add("@FeedbackType", type);
                                        hat.Add("@subjectno", subjectcode);
                                        ds = d2.select_method("[AnonymousReportClassWise]", hat, "sp");
                                        //string question_count = d2.GetFunction("select COUNT( distinct QuestionMasterFK)question_count from CO_FeedBackQuestions where FeedBackMasterFK in ('" + feedbakpk + "')");
                                        double question_count = 0;
                                        if (ds.Tables.Count > 0)
                                        {
                                            if (ds.Tables[4].Rows.Count > 0)
                                            {
                                                double.TryParse(Convert.ToString(ds.Tables[4].Compute("sum(question_count)", "")), out question_count);
                                            }
                                            string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                            if (ds.Tables[1].Rows.Count > 0)
                                            {
                                                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                                {
                                                    FpSpread1.Sheets[0].ColumnCount++;
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[1].Rows[i]["questionacr"]);
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[1].Rows[i]["QuestionMasterPK"]);
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                    FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 100;
                                                }
                                                //========Modified By saranya on 27/08/2018=======//
                                                FpSpread1.Sheets[0].ColumnCount++;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "No.Of Students";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                FpSpread1.Sheets[0].ColumnCount++;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Feedback Percentage";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                //==============================================//

                                                //FpSpread1.Sheets[0].ColumnCount++;
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Maximum Total";
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Name = "Book Antiqua";
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Bold = true;
                                            }
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                int k = 0; string staffname = ""; int s = 1;
                                                FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                                                cb.AutoPostBack = true;
                                                FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                                                cb1.AutoPostBack = false;
                                                //FpSpread1.Sheets[0].RowCount++;
                                                double staffavg = 0; bool staffinvdiavg = false; double sumavgpoint = 0; int staffrowcount = 0;
                                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                                {
                                                    staffinvdiavg = false;
                                                    FpSpread1.Sheets[0].RowCount++;
                                                    if (staffname.Trim() == "")
                                                    {
                                                        k++;
                                                    }
                                                    else if (staffname == ds.Tables[0].Rows[i]["staff_name"].ToString())
                                                    {
                                                        k++; staffrowcount++;
                                                    }
                                                    else
                                                    {
                                                        k = 1; s++;
                                                        //FpSpread1.Sheets[0].RowCount++;
                                                        //staffavg = (staffavg / Convert.ToDouble(staffrowcount+1));
                                                        staffavg = ((staffavg / (Convert.ToDouble(staffrowcount + 1) * 100)) * 100);
                                                        double.TryParse(Convert.ToString(Math.Round(staffavg, 2)), out sumavgpoint);
                                                        staffinvdiavg = true;
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(s);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["department"].ToString();
                                                    staffname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                                                    //Changed by saranya on 20/8/2018
                                                    string staff = Convert.ToString(ds.Tables[0].Rows[i]["staff"]);
                                                    string[] staffSplit = staff.Split('-');

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = staffSplit[0];//k.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = staffSplit[1];//k.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                                    ////////////////////////////////////

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["subject_code"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["Subject_Name"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["acronym"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = ds.Tables[0].Rows[i]["section"].ToString();
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Bold = true;

                                                    double gtotal = 0; double mtotal = 0; double avgper = 0;
                                                    string filterquery = string.Empty;
                                                    string section = Convert.ToString(ds.Tables[0].Rows[i]["Section"]);
                                                    filterquery = "degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["DegreeCode"]) + "' ";
                                                    if (section.Trim() != "")
                                                    {
                                                        filterquery = filterquery + " and Sections='" + section + "'";
                                                    }
                                                    ds.Tables[3].DefaultView.RowFilter = "" + filterquery + "";
                                                    DataView dvnew = ds.Tables[3].DefaultView;
                                                    string totalstudnent = "";
                                                    if (dvnew.Count > 0)
                                                    {
                                                        totalstudnent = Convert.ToString(dvnew[0]["studentcount"]);
                                                    }
                                                    if (totalstudnent.Trim() == "")
                                                        totalstudnent = "0";
                                                    //double maximun = Convert.ToDouble(question_count) * Convert.ToDouble(sum_total) * Convert.ToDouble(totalstudnent);
                                                    Double attendstrength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                    double maximun = Convert.ToDouble(sum_total) * Convert.ToDouble(attendstrength);
                                                    double QuestionAttendcount = 0;
                                                    for (int j = 8; j <= FpSpread1.Columns.Count - 2; j++)//modified by saranya on 27Aug2018 FpSpread1.Columns.Count changed to FpSpread1.Columns.Count - 2
                                                    {
                                                        string questionmasterPK = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, j - 1].Tag);
                                                        ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";//and FeedbackUnicode='" + ds.Tables[0].Rows[i]["FeedbackUnicode"] + "'
                                                        DataView dv = ds.Tables[2].DefaultView;
                                                        if (dv.Count > 0)
                                                        {
                                                            QuestionAttendcount++;
                                                            string point1 = Convert.ToString(dv[0]["points"]);
                                                            if (string.IsNullOrEmpty(point1.Trim()) || point1.Trim() == "-")
                                                                point1 = "0";
                                                            double questavgpoint = Convert.ToDouble(point1) / maximun * Convert.ToDouble(sum_total);// 100;
                                                            questavgpoint = Math.Round(questavgpoint, 0, MidpointRounding.AwayFromZero);
                                                            //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(Math.Round(questavgpoint, 2)); //Convert.ToString(dv[0]["points"]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(questavgpoint);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                            gtotal += Convert.ToDouble((Math.Round(questavgpoint, 2)));
                                                        }
                                                        else
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = "-";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                        }
                                                        FpSpread1.Columns[j - 1].Locked = true;
                                                        FpSpread1.Columns[4].Locked = true;
                                                    }
                                                    //Modified by saranya on 27Aug2018
                                                    Double strength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                    double calfbcal = Convert.ToDouble(attendstrength) * Convert.ToDouble(question_count) * Convert.ToDouble(sum_total);
                                                    double fbavg = (gtotal / calfbcal) * 100;
                                                    double avg = Convert.ToDouble(Math.Round(fbavg, 2));
                                                    string studentcount = "";
                                                    if (Convert.ToString(ds.Tables[0].Rows[i]["Strength"]).Trim() != "")
                                                    {
                                                        studentcount = Convert.ToString(ds.Tables[0].Rows[i]["Strength"]);
                                                    }
                                                    else
                                                    {
                                                        studentcount = "-";
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].CellType = new FarPoint.Web.Spread.TextCellType();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = Convert.ToString(studentcount);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";

                                                    double averag = (Math.Round(Math.Round(gtotal, 2) / Math.Round((question_count * Convert.ToDouble(sum_total)), 2) * 100, 2));
                                                    //barath 31.07.17 *100 added
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(averag, 2));
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                    //==========================================================================//

                                                    //if (issubjecttype == "1" || issubjecttype.ToUpper() == "TRUE")
                                                    //{
                                                    //    question_count = QuestionAttendcount;
                                                    //}
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = Convert.ToString(Math.Round((question_count * Convert.ToDouble(sum_total)), 2));
                                                    //double avg = (Math.Round(Math.Round(gtotal, 2) / Math.Round((question_count * Convert.ToDouble(sum_total)), 2) * 100, 2));
                                                    ////barath 31.07.17 *100 added
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(avg, 2));
                                                    //staffavg += avg;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].HorizontalAlign = HorizontalAlign.Right;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Name = "Book Antiqua";
                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Bold = true;
                                                    //if (staffinvdiavg == true)
                                                    //{
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(sumavgpoint, 2));

                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].ForeColor = System.Drawing.Color.BlueViolet;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                    //    sumavgpoint = 0;
                                                    //    staffrowcount = 0; staffavg = 0; staffavg += avg;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Text = "Average";
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].ForeColor = System.Drawing.Color.BlueViolet;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                                                    //}
                                                }
                                                //FpSpread1.Sheets[0].RowCount++;
                                                //staffavg = ((staffavg / (Convert.ToDouble(staffrowcount + 1) * 100)) * 100);
                                                //double.TryParse(Convert.ToString(Math.Round(staffavg, 2)), out sumavgpoint);
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = Convert.ToString(String.Format("{0:0.00}", sumavgpoint));
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].ForeColor = System.Drawing.Color.BlueViolet;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].HorizontalAlign = HorizontalAlign.Right;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Name = "Book Antiqua";
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Bold = true;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = "Average";
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].ForeColor = System.Drawing.Color.BlueViolet;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                                                //FpSpread1.Columns[FpSpread1.Columns.Count - 1].Locked = true;
                                                //FpSpread1.Columns[FpSpread1.Columns.Count - 2].Locked = true;
                                                //FpSpread1.Columns[FpSpread1.Columns.Count - 3].Locked = true;
                                                //FpSpread1.Columns[0].Locked = true;
                                                //FpSpread1.Columns[1].Locked = true;
                                                //FpSpread1.Columns[2].Locked = true;
                                                //FpSpread1.Columns[3].Locked = true;
                                                //FpSpread1.Columns[4].Locked = true;
                                                //FpSpread1.Columns[5].Locked = true;
                                                FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                                                // FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                                                //FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                                                //FpSpread1.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                                                string selqry = "";
                                                DataSet dsnew = new DataSet();

                                                string college_cd = string.Empty;
                                                if (cbl_clgname.Items.Count > 0)
                                                {
                                                    for (int i = 0; i < cbl_clgname.Items.Count; i++)
                                                    {
                                                        if (cbl_clgname.Items[i].Selected == true)
                                                        {
                                                            if (college_cd == "")
                                                            {
                                                                college_cd = "" + cbl_clgname.Items[i].Value.ToString() + "";
                                                            }
                                                            else
                                                            {
                                                                college_cd = college_cd + "','" + Convert.ToString(cbl_clgname.Items[i].Value);
                                                            }
                                                        }
                                                    }
                                                }
                                                string Batch_Year = "";
                                                for (int i = 0; i < cbl_batch.Items.Count; i++)
                                                {
                                                    if (cbl_batch.Items[i].Selected == true)
                                                    {
                                                        if (Batch_Year == "")
                                                        {
                                                            Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                                                        }
                                                        else
                                                        {
                                                            Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                                                        }
                                                    }
                                                }
                                                string degree_code = "";
                                                for (int i = 0; i < cbl_deptname.Items.Count; i++)
                                                {
                                                    if (cbl_deptname.Items[i].Selected == true)
                                                    {
                                                        if (degree_code == "")
                                                        {
                                                            degree_code = "" + cbl_deptname.Items[i].Value.ToString() + "";
                                                        }
                                                        else
                                                        {
                                                            degree_code = degree_code + "','" + cbl_deptname.Items[i].Value.ToString() + "";
                                                        }
                                                    }
                                                }
                                                string sections = "";
                                                for (int i = 0; i < cbl_sec.Items.Count; i++)
                                                {
                                                    if (cbl_sec.Items[i].Selected == true)
                                                    {
                                                        if (sections == "")
                                                        {
                                                            sections = "" + cbl_sec.Items[i].Value.ToString() + "";
                                                        }
                                                        else
                                                        {
                                                            sections = sections + "','" + cbl_sec.Items[i].Value.ToString() + "";
                                                        }
                                                        if (cbl_sec.Items[i].Value == "Empty")
                                                        {
                                                            sections = "";
                                                        }
                                                    }
                                                }
                                                if (sections.Trim() != "")
                                                {
                                                    sections = sections + "','";
                                                }
                                                string semester = "";
                                                for (int i = 0; i < cbl_sem.Items.Count; i++)
                                                {
                                                    if (cbl_sem.Items[i].Selected == true)
                                                    {
                                                        if (semester == "")
                                                        {
                                                            semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                                                        }
                                                        else
                                                        {
                                                            semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                                                        }
                                                    }
                                                }




                                                selqry = " select COUNT( distinct  s.FeedbackUnicode) as Strength,f.FeedBackMasterPK,Batch_Year ,f.semester,f.DegreeCode,f.Section from CO_FeedBackMaster F,CO_StudFeedBack S where s.FeedBackMasterFK =f.FeedBackMasterPK  and f.degreecode in ('" + degree_code + "') and f.Batch_Year in('" + Batch_Year + "') and f.semester in ('" + semester + "') and f.FeedBackName='" + Convert.ToString(ddl_feedback.SelectedItem.Text) + "'  and f.InclueCommon='1' and s.FeedbackUnicode<>''";
                                                if (sections != "")
                                                {
                                                    selqry = selqry + " and f.Section in ('" + sections + "')  ";
                                                }
                                                selqry = selqry + " group by f.FeedBackMasterPK  ,Batch_Year ,f.semester,f.DegreeCode ,f.Section";
                                                selqry = selqry + " SELECT Course_Name+'-'+Dept_Name Degree,Current_semester,R.degree_code,Sections,Batch_Year,COUNT(*)as TotStrengh FROM Registration R,Degree G,Course C,Department D WHERE R.degree_code = G.Degree_Code AND G.Course_Id = C.Course_Id AND G.college_code = C.college_code AND g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' and r.degree_code in ('" + degree_code + "') and r.Batch_Year in('" + Batch_Year + "') ";//and 
                                                if (sections != "")
                                                {
                                                    selqry = selqry + " and r.Sections in ('" + sections + "') GROUP BY Course_Name,R.degree_code, Dept_Name,Current_semester,Sections, Batch_Year ORDER BY R.degree_code,Current_Semester,Sections , Batch_Year,Course_Name,Dept_Name ";
                                                }
                                                else
                                                {
                                                    selqry = selqry + " GROUP BY Course_Name, R.degree_code, Dept_Name,Current_semester,Sections,Batch_Year ORDER BY R.degree_code,Current_Semester,Sections , Batch_Year,Course_Name,Dept_Name";
                                                }
                                                selqry = selqry + "   SELECT Course_Name+'-'+Dept_Name Degree,semester,f.DegreeCode,Section,Batch_Year FROM Degree G,Course C,Department D,CO_FeedBackMaster F,CO_StudFeedBack S WHERE f.FeedBackMasterPK=s.FeedBackMasterFK and f.DegreeCode = G.Degree_Code AND G.Course_Id = C.Course_Id AND G.college_code = C.college_code AND g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND  f.degreecode in ('" + degree_code + "') and f.Batch_Year in('" + Batch_Year + "') and f.Semester in ('" + semester + "') and f.FeedBackName='" + Convert.ToString(ddl_feedback.SelectedItem.Text) + "'  and f.InclueCommon='1' and s.FeedbackUnicode<>''";
                                                if (sections != "")
                                                {
                                                    selqry = selqry + " and f.Section in ('" + sections + "')   GROUP BY Course_Name,f.degreecode, Dept_Name,semester,Section, Batch_Year ORDER BY f.degreecode,Semester,Section , Batch_Year,Course_Name,Dept_Name ";
                                                }
                                                else
                                                {
                                                    selqry = selqry + "   GROUP BY Course_Name,f.degreecode, Dept_Name,semester,Section, Batch_Year ORDER BY f.degreecode,Semester,Section , Batch_Year,Course_Name,Dept_Name ";
                                                }


                                                dsnew = d2.select_method_wo_parameter(selqry, "text");
                                                DataView dvnews = new DataView();
                                                DataView totalview = new DataView();
                                                int overallstrength = 0;
                                                int attended = 0;
                                                if (dsnew.Tables.Count > 0)
                                                {
                                                    if (dsnew.Tables[0].Rows.Count > 0 && dsnew.Tables[1].Rows.Count > 0 && dsnew.Tables[2].Rows.Count > 0)
                                                    {

                                                        for (int i = 0; i < dsnew.Tables[2].Rows.Count; i++)
                                                        {
                                                            string sectons = Convert.ToString(dsnew.Tables[2].Rows[i]["Section"]);
                                                            string degrecode = Convert.ToString(dsnew.Tables[2].Rows[i]["degreecode"]);
                                                            string getsem = Convert.ToString(dsnew.Tables[2].Rows[i]["semester"]);
                                                            string totalfind = " degree_code='" + degrecode + "'  and  Batch_Year='" + dsnew.Tables[2].Rows[i]["Batch_Year"].ToString() + "'";
                                                            if (sectons.Trim() != "")
                                                            {
                                                                totalfind = totalfind + " and Sections='" + sectons + "'";
                                                            }
                                                            dsnew.Tables[1].DefaultView.RowFilter = "" + totalfind + "";
                                                            totalview = dsnew.Tables[1].DefaultView;
                                                            int total = 0;
                                                            if (totalview.Count > 0)
                                                            {

                                                                string totas = totalview[0]["TotStrengh"].ToString();
                                                                if (totas.Trim() == "")
                                                                {
                                                                    totas = "0";
                                                                }
                                                                total = Convert.ToInt32(totas);
                                                                overallstrength = overallstrength + total;
                                                            }
                                                            string filterquery = "";

                                                            int attand = 0;
                                                            filterquery = "degreecode='" + degrecode + "'  and  semester='" + getsem + "' ";
                                                            if (sectons.Trim() != "")
                                                            {
                                                                filterquery = filterquery + " and Section='" + sectons + "'";
                                                            }
                                                            dsnew.Tables[0].DefaultView.RowFilter = "" + filterquery + "";
                                                            dvnews = dsnew.Tables[0].DefaultView;
                                                            if (dvnews.Count > 0)
                                                            {

                                                                attand = Convert.ToInt32(dvnews[0]["Strength"]);
                                                            }
                                                            attended = attended + attand;
                                                        }

                                                    }
                                                }



                                                int colcount = FpSpread1.Sheets[0].ColumnCount;
                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, colcount);


                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Students Strength:" + Convert.ToString(overallstrength);
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, colcount);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].RowCount++;

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "No.Of Students Feedback Obtained:" + Convert.ToString(attended);
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, colcount);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, colcount);


                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Columns[0].Locked = true;
                                                FpSpread1.Columns[1].Locked = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "QUESTION USED FOR THE ASSESMENT PROCESS";
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, colcount);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                for (int j = 0; j < ds.Tables[1].Rows.Count; j++)//delsi2610
                                                {
                                                    FpSpread1.Sheets[0].RowCount++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[1].Rows[j]["questionacr"]) + ".";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[1].Rows[j]["Question"]) + "?";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, colcount);

                                                }
                                                FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                                FpSpread1.Height = 500;
                                                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                                SpreadDiv.Visible = true;
                                            }
                                            else
                                            {
                                                lbl_error.Visible = true;
                                                lbl_error.Text = "No Records Found";
                                                SpreadDiv.Visible = false;
                                            }
                                        }
                                        else
                                        {
                                            lbl_error.Visible = true;
                                            lbl_error.Text = "No Records Found";
                                            SpreadDiv.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        SpreadDiv.Visible = false;
                                        lbl_error.Visible = true;
                                        lbl_error.Text = "Please Select Feedback Name";
                                    }
                                }
                                else
                                {
                                    SpreadDiv.Visible = false;
                                    lbl_error.Visible = true;
                                    lbl_error.Text = "Please select all fields";
                                }
                            }

                            #endregion

                            #region Added By Saranya On 28/08/2018 For ClassWise Without RoundOff

                            if (cb_WithOutRoundOff.Checked == true)
                            {
                                lbl_error.Visible = false;
                                Printcontrol1.Visible = false;
                                string degreecode = rs.GetSelectedItemsValue(cbl_deptname);
                                string sem = rs.GetSelectedItemsValue(cbl_sem);
                                string batchyear = rs.GetSelectedItemsValue(cbl_batch);
                                string clgcode = rs.GetSelectedItemsValue(cbl_clgname);
                                string subjectcode = rs.GetSelectedItemsValue(Cbl_Subject);
                                string sec = string.Empty;
                                for (int i = 0; i < cbl_sec.Items.Count; i++)
                                {
                                    if (cbl_sec.Items[i].Selected == true)
                                    {
                                        if (string.IsNullOrEmpty(sec))
                                            sec = cbl_sec.Items[i].Value.ToString();
                                        else
                                            sec = sec + "," + cbl_sec.Items[i].Value.ToString() + "";
                                    }
                                }
                                if (!string.IsNullOrEmpty(degreecode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batchyear))
                                {
                                    if (ddl_feedback.SelectedItem.Text.Trim() != "--Select--")
                                    {
                                        string type = "1";
                                        string fbpk = " select FeedBackMasterPK,ISNULL(issubjecttype,0)issubjecttype from CO_FeedBackMaster where FeedBackName ='" + ddl_feedback.SelectedItem.Value + "'";
                                        DataSet dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                                        string feedbakpk = string.Empty;
                                        string issubjecttype = string.Empty;
                                        if (dsfb.Tables.Count > 0)
                                        {
                                            if (dsfb.Tables[0].Rows.Count > 0)
                                            {
                                                issubjecttype = Convert.ToString(dsfb.Tables[0].Rows[0]["issubjecttype"]);
                                                for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                                                {
                                                    if (string.IsNullOrEmpty(feedbakpk))
                                                        feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                                    else
                                                        feedbakpk = feedbakpk + "," + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                                                }
                                            }
                                        }
                                        rs.Fpreadheaderbindmethod("S No-50/Department-200/StaffCode-100/StaffName-200/Subject Code-150/Subject Name-250/SubjectType-100", FpSpread1, "false");
                                        Hashtable hat = new Hashtable();
                                        hat.Add("@CollegeCode", clgcode);
                                        hat.Add("@batchyear", batchyear);
                                        hat.Add("@Degreecode", degreecode);
                                        hat.Add("@semester", sem);
                                        hat.Add("@section", sec);
                                        hat.Add("@FeedbackName", Convert.ToString(ddl_feedback.SelectedItem.Text));
                                        hat.Add("@FeedbackMasterFK", feedbakpk);
                                        hat.Add("@FeedbackType", type);
                                        hat.Add("@subjectno", subjectcode);
                                        ds = d2.select_method("[AnonymousReportClassWise]", hat, "sp");

                                        double question_count = 0;
                                        if (ds.Tables.Count > 0)
                                        {
                                            if (ds.Tables[4].Rows.Count > 0)
                                            {
                                                double.TryParse(Convert.ToString(ds.Tables[4].Compute("sum(question_count)", "")), out question_count);
                                            }
                                            string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                            if (ds.Tables[1].Rows.Count > 0)
                                            {
                                                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                                {
                                                    FpSpread1.Sheets[0].ColumnCount++;
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[1].Rows[i]["questionacr"]);
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[1].Rows[i]["QuestionMasterPK"]);
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                    FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 100;
                                                }
                                                FpSpread1.Sheets[0].ColumnCount++;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "No.Of Students";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                FpSpread1.Sheets[0].ColumnCount++;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Feedback Percentage";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                            }
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                int k = 0; string staffname = ""; int s = 1;
                                                FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                                                cb.AutoPostBack = true;
                                                FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                                                cb1.AutoPostBack = false;
                                                double staffavg = 0; bool staffinvdiavg = false; double sumavgpoint = 0; int staffrowcount = 0;
                                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                                {
                                                    staffinvdiavg = false;
                                                    FpSpread1.Sheets[0].RowCount++;
                                                    if (staffname.Trim() == "")
                                                    {
                                                        k++;
                                                    }
                                                    else if (staffname == ds.Tables[0].Rows[i]["staff_name"].ToString())
                                                    {
                                                        k++; staffrowcount++;
                                                    }
                                                    else
                                                    {
                                                        k = 1; s++;
                                                        //FpSpread1.Sheets[0].RowCount++;                                          
                                                        staffavg = ((staffavg / (Convert.ToDouble(staffrowcount + 1) * 100)) * 100);
                                                        double.TryParse(Convert.ToString(Math.Round(staffavg, 2)), out sumavgpoint);
                                                        staffinvdiavg = true;
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(s);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["department"].ToString();
                                                    staffname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                                    string staff = Convert.ToString(ds.Tables[0].Rows[i]["staff"]);
                                                    string[] staffSplit = staff.Split('-');

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = staffSplit[0];//k.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = staffSplit[1];//k.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["subject_code"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["Subject_Name"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["acronym"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;

                                                    double gtotal = 0; double mtotal = 0; double avgper = 0;
                                                    string filterquery = string.Empty;
                                                    string section = Convert.ToString(ds.Tables[0].Rows[i]["Section"]);
                                                    filterquery = "degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["DegreeCode"]) + "' ";
                                                    if (section.Trim() != "")
                                                    {
                                                        filterquery = filterquery + " and Sections='" + section + "'";
                                                    }
                                                    ds.Tables[3].DefaultView.RowFilter = "" + filterquery + "";
                                                    DataView dvnew = ds.Tables[3].DefaultView;
                                                    string totalstudnent = "";
                                                    if (dvnew.Count > 0)
                                                    {
                                                        totalstudnent = Convert.ToString(dvnew[0]["studentcount"]);
                                                    }
                                                    if (totalstudnent.Trim() == "")
                                                        totalstudnent = "0";

                                                    Double attendstrength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                    double maximun = Convert.ToDouble(sum_total) * Convert.ToDouble(attendstrength);
                                                    double QuestionAttendcount = 0;
                                                    for (int j = 8; j <= FpSpread1.Columns.Count - 2; j++)
                                                    {
                                                        string questionmasterPK = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, j - 1].Tag);
                                                        ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";
                                                        DataView dv = ds.Tables[2].DefaultView;
                                                        if (dv.Count > 0)
                                                        {
                                                            QuestionAttendcount++;
                                                            string point1 = Convert.ToString(dv[0]["points"]);
                                                            if (string.IsNullOrEmpty(point1.Trim()) || point1.Trim() == "-")
                                                                point1 = "0";
                                                            double questavgpoint = Convert.ToDouble(point1) / maximun * Convert.ToDouble(sum_total);// 100;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(Math.Round(questavgpoint, 2));
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                            gtotal += Convert.ToDouble((Math.Round(questavgpoint, 2)));
                                                        }
                                                        else
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = "-";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                        }
                                                        FpSpread1.Columns[j - 1].Locked = true;
                                                        FpSpread1.Columns[4].Locked = true;
                                                    }
                                                    Double strength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                    double calfbcal = Convert.ToDouble(attendstrength) * Convert.ToDouble(question_count) * Convert.ToDouble(sum_total);
                                                    double fbavg = (gtotal / calfbcal) * 100;
                                                    double avg = Convert.ToDouble(Math.Round(fbavg, 2));
                                                    string studentcount = "";
                                                    if (Convert.ToString(ds.Tables[0].Rows[i]["Strength"]).Trim() != "")
                                                    {
                                                        studentcount = Convert.ToString(ds.Tables[0].Rows[i]["Strength"]);
                                                    }
                                                    else
                                                    {
                                                        studentcount = "-";
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].CellType = new FarPoint.Web.Spread.TextCellType();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = Convert.ToString(studentcount);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";

                                                    double averag = (Math.Round(Math.Round(gtotal, 2) / Math.Round((question_count * Convert.ToDouble(sum_total)), 2) * 100, 2));
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(averag, 2));
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                }
                                                FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                                                FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                                                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                                                string selqry = "";
                                                DataSet dsnew = new DataSet();

                                                string college_cd = string.Empty;
                                                if (cbl_clgname.Items.Count > 0)
                                                {
                                                    for (int i = 0; i < cbl_clgname.Items.Count; i++)
                                                    {
                                                        if (cbl_clgname.Items[i].Selected == true)
                                                        {
                                                            if (college_cd == "")
                                                            {
                                                                college_cd = "" + cbl_clgname.Items[i].Value.ToString() + "";
                                                            }
                                                            else
                                                            {
                                                                college_cd = college_cd + "','" + Convert.ToString(cbl_clgname.Items[i].Value);
                                                            }
                                                        }
                                                    }
                                                }
                                                string Batch_Year = "";
                                                for (int i = 0; i < cbl_batch.Items.Count; i++)
                                                {
                                                    if (cbl_batch.Items[i].Selected == true)
                                                    {
                                                        if (Batch_Year == "")
                                                        {
                                                            Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                                                        }
                                                        else
                                                        {
                                                            Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                                                        }
                                                    }
                                                }
                                                string degree_code = "";
                                                for (int i = 0; i < cbl_deptname.Items.Count; i++)
                                                {
                                                    if (cbl_deptname.Items[i].Selected == true)
                                                    {
                                                        if (degree_code == "")
                                                        {
                                                            degree_code = "" + cbl_deptname.Items[i].Value.ToString() + "";
                                                        }
                                                        else
                                                        {
                                                            degree_code = degree_code + "','" + cbl_deptname.Items[i].Value.ToString() + "";
                                                        }
                                                    }
                                                }
                                                string sections = "";
                                                for (int i = 0; i < cbl_sec.Items.Count; i++)
                                                {
                                                    if (cbl_sec.Items[i].Selected == true)
                                                    {
                                                        if (sections == "")
                                                        {
                                                            sections = "" + cbl_sec.Items[i].Value.ToString() + "";
                                                        }
                                                        else
                                                        {
                                                            sections = sections + "','" + cbl_sec.Items[i].Value.ToString() + "";
                                                        }
                                                        if (cbl_sec.Items[i].Value == "Empty")
                                                        {
                                                            sections = "";
                                                        }
                                                    }
                                                }
                                                if (sections.Trim() != "")
                                                {
                                                    sections = sections + "','";
                                                }
                                                string semester = "";
                                                for (int i = 0; i < cbl_sem.Items.Count; i++)
                                                {
                                                    if (cbl_sem.Items[i].Selected == true)
                                                    {
                                                        if (semester == "")
                                                        {
                                                            semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                                                        }
                                                        else
                                                        {
                                                            semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                                                        }
                                                    }
                                                }




                                                selqry = " select COUNT( distinct  s.FeedbackUnicode) as Strength,f.FeedBackMasterPK,Batch_Year ,f.semester,f.DegreeCode,f.Section from CO_FeedBackMaster F,CO_StudFeedBack S where s.FeedBackMasterFK =f.FeedBackMasterPK  and f.degreecode in ('" + degree_code + "') and f.Batch_Year in('" + Batch_Year + "') and f.semester in ('" + semester + "') and f.FeedBackName='" + Convert.ToString(ddl_feedback.SelectedItem.Text) + "'  and f.InclueCommon='1' and s.FeedbackUnicode<>''";
                                                if (sections != "")
                                                {
                                                    selqry = selqry + " and f.Section in ('" + sections + "')  ";
                                                }
                                                selqry = selqry + " group by f.FeedBackMasterPK  ,Batch_Year ,f.semester,f.DegreeCode ,f.Section";
                                                selqry = selqry + " SELECT Course_Name+'-'+Dept_Name Degree,Current_semester,R.degree_code,Sections,Batch_Year,COUNT(*)as TotStrengh FROM Registration R,Degree G,Course C,Department D WHERE R.degree_code = G.Degree_Code AND G.Course_Id = C.Course_Id AND G.college_code = C.college_code AND g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' and r.degree_code in ('" + degree_code + "') and r.Batch_Year in('" + Batch_Year + "') ";//and 
                                                if (sections != "")
                                                {
                                                    selqry = selqry + " and r.Sections in ('" + sections + "') GROUP BY Course_Name,R.degree_code, Dept_Name,Current_semester,Sections, Batch_Year ORDER BY R.degree_code,Current_Semester,Sections , Batch_Year,Course_Name,Dept_Name ";
                                                }
                                                else
                                                {
                                                    selqry = selqry + " GROUP BY Course_Name, R.degree_code, Dept_Name,Current_semester,Sections,Batch_Year ORDER BY R.degree_code,Current_Semester,Sections , Batch_Year,Course_Name,Dept_Name";
                                                }
                                                selqry = selqry + "   SELECT Course_Name+'-'+Dept_Name Degree,semester,f.DegreeCode,Section,Batch_Year FROM Degree G,Course C,Department D,CO_FeedBackMaster F,CO_StudFeedBack S WHERE f.FeedBackMasterPK=s.FeedBackMasterFK and f.DegreeCode = G.Degree_Code AND G.Course_Id = C.Course_Id AND G.college_code = C.college_code AND g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND  f.degreecode in ('" + degree_code + "') and f.Batch_Year in('" + Batch_Year + "') and f.Semester in ('" + semester + "') and f.FeedBackName='" + Convert.ToString(ddl_feedback.SelectedItem.Text) + "'  and f.InclueCommon='1' and s.FeedbackUnicode<>''";
                                                if (sections != "")
                                                {
                                                    selqry = selqry + " and f.Section in ('" + sections + "')   GROUP BY Course_Name,f.degreecode, Dept_Name,semester,Section, Batch_Year ORDER BY f.degreecode,Semester,Section , Batch_Year,Course_Name,Dept_Name ";
                                                }
                                                else
                                                {
                                                    selqry = selqry + "   GROUP BY Course_Name,f.degreecode, Dept_Name,semester,Section, Batch_Year ORDER BY f.degreecode,Semester,Section , Batch_Year,Course_Name,Dept_Name ";
                                                }


                                                dsnew = d2.select_method_wo_parameter(selqry, "text");
                                                DataView dvnews = new DataView();
                                                DataView totalview = new DataView();
                                                int overallstrength = 0;
                                                int attended = 0;
                                                if (dsnew.Tables.Count > 0)
                                                {
                                                    if (dsnew.Tables[0].Rows.Count > 0 && dsnew.Tables[1].Rows.Count > 0 && dsnew.Tables[2].Rows.Count > 0)
                                                    {

                                                        for (int i = 0; i < dsnew.Tables[2].Rows.Count; i++)
                                                        {
                                                            string sectons = Convert.ToString(dsnew.Tables[2].Rows[i]["Section"]);
                                                            string degrecode = Convert.ToString(dsnew.Tables[2].Rows[i]["degreecode"]);
                                                            string getsem = Convert.ToString(dsnew.Tables[2].Rows[i]["semester"]);
                                                            string totalfind = " degree_code='" + degrecode + "'  and  Batch_Year='" + dsnew.Tables[2].Rows[i]["Batch_Year"].ToString() + "'";
                                                            if (sectons.Trim() != "")
                                                            {
                                                                totalfind = totalfind + " and Sections='" + sectons + "'";
                                                            }
                                                            dsnew.Tables[1].DefaultView.RowFilter = "" + totalfind + "";
                                                            totalview = dsnew.Tables[1].DefaultView;
                                                            int total = 0;
                                                            if (totalview.Count > 0)
                                                            {

                                                                string totas = totalview[0]["TotStrengh"].ToString();
                                                                if (totas.Trim() == "")
                                                                {
                                                                    totas = "0";
                                                                }
                                                                total = Convert.ToInt32(totas);
                                                                overallstrength = overallstrength + total;
                                                            }
                                                            string filterquery = "";

                                                            int attand = 0;
                                                            filterquery = "degreecode='" + degrecode + "'  and  semester='" + getsem + "' ";
                                                            if (sectons.Trim() != "")
                                                            {
                                                                filterquery = filterquery + " and Section='" + sectons + "'";
                                                            }
                                                            dsnew.Tables[0].DefaultView.RowFilter = "" + filterquery + "";
                                                            dvnews = dsnew.Tables[0].DefaultView;
                                                            if (dvnews.Count > 0)
                                                            {

                                                                attand = Convert.ToInt32(dvnews[0]["Strength"]);
                                                            }
                                                            attended = attended + attand;
                                                        }

                                                    }
                                                }



                                                int colcount = FpSpread1.Sheets[0].ColumnCount;
                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, colcount);


                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Students Strength:" + Convert.ToString(overallstrength);
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, colcount);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].RowCount++;

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "No.Of Students Feedback Obtained:" + Convert.ToString(attended);
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, colcount);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, colcount);


                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Columns[0].Locked = true;
                                                FpSpread1.Columns[1].Locked = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "QUESTION USED FOR THE ASSESMENT PROCESS";
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, colcount);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                for (int j = 0; j < ds.Tables[1].Rows.Count; j++)//delsi2610
                                                {
                                                    FpSpread1.Sheets[0].RowCount++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[1].Rows[j]["questionacr"]) + ".";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[1].Rows[j]["Question"]) + "?";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, colcount);

                                                }


                                                FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                                FpSpread1.Height = 500;
                                                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                                SpreadDiv.Visible = true;
                                            }
                                            else
                                            {
                                                lbl_error.Visible = true;
                                                lbl_error.Text = "No Records Found";
                                                SpreadDiv.Visible = false;
                                            }
                                        }
                                        else
                                        {
                                            lbl_error.Visible = true;
                                            lbl_error.Text = "No Records Found";
                                            SpreadDiv.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        SpreadDiv.Visible = false;
                                        lbl_error.Visible = true;
                                        lbl_error.Text = "Please Select Feedback Name";
                                    }
                                }
                                else
                                {
                                    SpreadDiv.Visible = false;
                                    lbl_error.Visible = true;
                                    lbl_error.Text = "Please select all fields";
                                }
                            }
                            #endregion
                        }
                    }

                }
                else if (cbmul.Checked == true)//delsi3013
                {

                    if (Rdbques.Checked == true)
                    {

                        if (rdb_classwise.Checked == true)//delsi1903
                        {
                            #region ClassWise With RoundOff

                            if (cb_WithOutRoundOff.Checked == false)
                            {
                                bool checkheader = false;
                                int overallcolcount = 0;
                                int colcountheader = 0;
                                int tagrowcount = 0;
                                int getcountrow = 0;
                                bool checkgreatercol = false;
                                string feebackname = string.Empty;

                                if (cblfeedbackmulti.Items.Count > 0)//delsi2910
                                {
                                    string feedbak = string.Empty;
                                    for (int fb = 0; fb < cblfeedbackmulti.Items.Count; fb++)
                                    {
                                        if (cblfeedbackmulti.Items[fb].Selected == true)
                                        {
                                            feebackname = Convert.ToString(cblfeedbackmulti.Items[fb].Value);
                                            lbl_error.Visible = false;
                                            Printcontrol1.Visible = false;
                                            string degreecode = rs.GetSelectedItemsValue(cbl_deptname);
                                            string sem = rs.GetSelectedItemsValue(cbl_sem);
                                            string batchyear = rs.GetSelectedItemsValue(cbl_batch);
                                            string clgcode = rs.GetSelectedItemsValue(cbl_clgname);
                                            //   string StaffAppID = rs.GetSelectedItemsValue(cbl_staffname);
                                            //   string degree = rs.GetSelectedItemsValue(cbl_degree);
                                            string subjectcode = rs.GetSelectedItemsValue(Cbl_Subject);

                                            string sec = string.Empty;
                                            for (int i = 0; i < cbl_sec.Items.Count; i++)
                                            {
                                                if (cbl_sec.Items[i].Selected == true)
                                                {
                                                    if (string.IsNullOrEmpty(sec))
                                                        sec = cbl_sec.Items[i].Value.ToString();
                                                    else
                                                        sec = sec + "," + cbl_sec.Items[i].Value.ToString() + "";
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(degreecode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batchyear))
                                            {
                                                string type = "1";
                                                string fbpk = " select FeedBackMasterPK,ISNULL(issubjecttype,0)issubjecttype from CO_FeedBackMaster where FeedBackName ='" + feebackname + "'";
                                                DataSet dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                                                string feedbakpk = string.Empty;
                                                string issubjecttype = string.Empty;
                                                if (dsfb.Tables.Count > 0)
                                                {
                                                    if (dsfb.Tables[0].Rows.Count > 0)
                                                    {
                                                        issubjecttype = Convert.ToString(dsfb.Tables[0].Rows[0]["issubjecttype"]);
                                                        for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                                                        {
                                                            if (string.IsNullOrEmpty(feedbakpk))
                                                                feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                                            else
                                                                feedbakpk = feedbakpk + "," + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                                                        }
                                                    }
                                                }
                                                //rs.Fpreadheaderbindmethod("S No-50/Department-200/StaffCode & StaffName-250/Subject Code-150/Subject Name-250/SubjectType-100", FpSpread1, "false");

                                                //Modified by saranya on 20/08/2018
                                                if (checkheader == false)
                                                {
                                                    rs.Fpreadheaderbindmethod("S No-50/Department-200/StaffCode-100/StaffName-200/Subject Code-150/Subject Name-250/SubjectType-100", FpSpread1, "false");
                                                }
                                                else if (checkheader == true)
                                                {
                                                    FpSpread1.Sheets[0].RowCount++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "S.No";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Columns[0].Width = 50;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.White;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Department";
                                                    FpSpread1.Columns[1].Width = 200;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Columns[2].Width = 200;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].ForeColor = Color.White;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "StaffCode";
                                                    FpSpread1.Columns[3].Width = 200;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.White;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = "StaffName";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].ForeColor = Color.White;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "Subject Code";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                                                    FpSpread1.Columns[4].Width = 200;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "Subject Name";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = Color.White;
                                                    FpSpread1.Columns[5].Width = 200;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "SubjectType";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.White;
                                                    FpSpread1.Columns[6].Width = 200;


                                                }
                                                Hashtable hat = new Hashtable();
                                                hat.Add("@CollegeCode", clgcode);
                                                hat.Add("@batchyear", batchyear);
                                                hat.Add("@Degreecode", degreecode);
                                                hat.Add("@semester", sem);
                                                hat.Add("@section", sec);
                                                hat.Add("@FeedbackName", Convert.ToString(feebackname));
                                                hat.Add("@FeedbackMasterFK", feedbakpk);
                                                //   hat.Add("@StaffAppNo", StaffAppID);
                                                hat.Add("@FeedbackType", type);
                                                hat.Add("@subjectno", subjectcode);
                                                ds = d2.select_method("[AnonymousReportClassWise]", hat, "sp");
                                                //string question_count = d2.GetFunction("select COUNT( distinct QuestionMasterFK)question_count from CO_FeedBackQuestions where FeedBackMasterFK in ('" + feedbakpk + "')");
                                                double question_count = 0;
                                                if (ds.Tables.Count > 0)
                                                {
                                                    if (ds.Tables[4].Rows.Count > 0)
                                                    {
                                                        double.TryParse(Convert.ToString(ds.Tables[4].Compute("sum(question_count)", "")), out question_count);
                                                    }
                                                    string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                                    if (checkheader == false)
                                                    {
                                                        if (ds.Tables[1].Rows.Count > 0)
                                                        {
                                                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                                            {
                                                                colcountheader = ds.Tables[1].Rows.Count;
                                                                FpSpread1.Sheets[0].ColumnCount++;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[1].Rows[i]["QuestionMasterPK"]);
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 300;
                                                            }
                                                            //========Modified By saranya on 27/08/2018=======//
                                                            FpSpread1.Sheets[0].ColumnCount++;
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "No.Of Students";
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                            FpSpread1.Sheets[0].ColumnCount++;
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Feedback Percentage";
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                            //==============================================//

                                                            //FpSpread1.Sheets[0].ColumnCount++;
                                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Maximum Total";
                                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Name = "Book Antiqua";
                                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Bold = true;
                                                        }
                                                    }
                                                    else if (checkheader == true)
                                                    {
                                                        if (ds.Tables[1].Rows.Count > 0)
                                                        {
                                                            overallcolcount = 6;
                                                            int totinc = 0;
                                                            if (colcountheader < ds.Tables[1].Rows.Count)
                                                            {
                                                                int getcount = ds.Tables[1].Rows.Count;
                                                                totinc = getcount - colcountheader;
                                                                for (int val = 0; val < totinc; val++)
                                                                {
                                                                    FpSpread1.Sheets[0].ColumnCount++;
                                                                    checkgreatercol = false;
                                                                }
                                                            }
                                                            if (colcountheader > ds.Tables[1].Rows.Count)
                                                            {
                                                                checkgreatercol = true;

                                                            }

                                                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                                            {
                                                                overallcolcount++;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Text = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Tag = Convert.ToString(ds.Tables[1].Rows[i]["QuestionMasterPK"]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Font.Name = "Book Antiqua";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].ForeColor = Color.White;
                                                                tagrowcount = FpSpread1.Sheets[0].RowCount;
                                                                FpSpread1.Columns[overallcolcount].Width = 200;


                                                            }
                                                            for (int col = 0; col < FpSpread1.Columns.Count; col++)
                                                            {
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");

                                                            }
                                                            overallcolcount++;

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = "No.Of Students";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].ForeColor = Color.White;
                                                            overallcolcount++;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Feedback Percentage";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;

                                                        }
                                                    }


                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        int k = 0; string staffname = ""; int s = 1;
                                                        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                                                        cb.AutoPostBack = true;
                                                        FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                                                        cb1.AutoPostBack = false;
                                                        //FpSpread1.Sheets[0].RowCount++;
                                                        double staffavg = 0; bool staffinvdiavg = false; double sumavgpoint = 0; int staffrowcount = 0;
                                                        FpSpread1.Sheets[0].RowCount++;
                                                        int colcnt = FpSpread1.Sheets[0].ColumnCount;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(cblfeedbackmulti.Items[fb].Value);//delsis29
                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, colcnt);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.MistyRose;

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;

                                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                                        {
                                                            staffinvdiavg = false;
                                                            FpSpread1.Sheets[0].RowCount++;
                                                            if (staffname.Trim() == "")
                                                            {
                                                                k++;
                                                            }
                                                            else if (staffname == ds.Tables[0].Rows[i]["staff_name"].ToString())
                                                            {
                                                                k++; staffrowcount++;
                                                            }
                                                            else
                                                            {
                                                                k = 1; s++;
                                                                //FpSpread1.Sheets[0].RowCount++;
                                                                //staffavg = (staffavg / Convert.ToDouble(staffrowcount+1));
                                                                staffavg = ((staffavg / (Convert.ToDouble(staffrowcount + 1) * 100)) * 100);
                                                                double.TryParse(Convert.ToString(Math.Round(staffavg, 2)), out sumavgpoint);
                                                                staffinvdiavg = true;
                                                            }
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(s);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["department"].ToString();
                                                            staffname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                                                            //Changed by saranya on 20/8/2018
                                                            string staff = Convert.ToString(ds.Tables[0].Rows[i]["staff"]);
                                                            string[] staffSplit = staff.Split('-');

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = staffSplit[0];//k.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = staffSplit[1];//k.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                                            ////////////////////////////////////

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["subject_code"].ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["Subject_Name"].ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["acronym"].ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = ds.Tables[0].Rows[i]["section"].ToString();
                                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Bold = true;

                                                            double gtotal = 0; double mtotal = 0; double avgper = 0;
                                                            string filterquery = string.Empty;
                                                            string section = Convert.ToString(ds.Tables[0].Rows[i]["Section"]);
                                                            filterquery = "degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["DegreeCode"]) + "' ";
                                                            if (section.Trim() != "")
                                                            {
                                                                filterquery = filterquery + " and Sections='" + section + "'";
                                                            }
                                                            ds.Tables[3].DefaultView.RowFilter = "" + filterquery + "";
                                                            DataView dvnew = ds.Tables[3].DefaultView;
                                                            string totalstudnent = "";
                                                            if (dvnew.Count > 0)
                                                            {
                                                                totalstudnent = Convert.ToString(dvnew[0]["studentcount"]);
                                                            }
                                                            if (totalstudnent.Trim() == "")
                                                                totalstudnent = "0";
                                                            //double maximun = Convert.ToDouble(question_count) * Convert.ToDouble(sum_total) * Convert.ToDouble(totalstudnent);
                                                            Double attendstrength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                            double maximun = Convert.ToDouble(sum_total) * Convert.ToDouble(attendstrength);
                                                            double QuestionAttendcount = 0;
                                                            if (checkgreatercol == false)
                                                            {

                                                                for (int j = 8; j <= FpSpread1.Columns.Count - 2; j++)//modified by saranya on 27Aug2018 FpSpread1.Columns.Count changed to FpSpread1.Columns.Count - 2
                                                                {
                                                                    string questionmasterPK = string.Empty;
                                                                    DataView dv = new DataView();
                                                                    if (checkheader == false)
                                                                    {
                                                                        questionmasterPK = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, j - 1].Tag);
                                                                        ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";//and FeedbackUnicode='" + ds.Tables[0].Rows[i]["FeedbackUnicode"] + "'

                                                                    }

                                                                    else if (checkheader == true)
                                                                    {

                                                                        questionmasterPK = Convert.ToString(FpSpread1.Cells[tagrowcount - 1, j - 1].Tag);
                                                                        if (questionmasterPK != "")
                                                                        {

                                                                            ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";
                                                                        }

                                                                    }

                                                                    dv = ds.Tables[2].DefaultView;
                                                                    if (dv.Count > 0)
                                                                    {
                                                                        QuestionAttendcount++;
                                                                        string point1 = Convert.ToString(dv[0]["points"]);
                                                                        if (string.IsNullOrEmpty(point1.Trim()) || point1.Trim() == "-")
                                                                            point1 = "0";
                                                                        double questavgpoint = Convert.ToDouble(point1) / maximun * Convert.ToDouble(sum_total);// 100;
                                                                        questavgpoint = Math.Round(questavgpoint, 0, MidpointRounding.AwayFromZero);
                                                                        //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(Math.Round(questavgpoint, 2)); //Convert.ToString(dv[0]["points"]);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(questavgpoint);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                                        gtotal += Convert.ToDouble((Math.Round(questavgpoint, 2)));
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = "-";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                                    }
                                                                    FpSpread1.Columns[j - 1].Locked = true;
                                                                    FpSpread1.Columns[4].Locked = true;
                                                                }
                                                            }
                                                            if (checkgreatercol == true)
                                                            {
                                                                for (int j = 8; j <= overallcolcount - 1; j++)//modified by saranya on 27Aug2018 FpSpread1.Columns.Count changed to FpSpread1.Columns.Count - 2
                                                                {
                                                                    string questionmasterPK = string.Empty;
                                                                    DataView dv = new DataView();
                                                                    if (checkheader == false)
                                                                    {
                                                                        questionmasterPK = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, j - 1].Tag);
                                                                        ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";//and FeedbackUnicode='" + ds.Tables[0].Rows[i]["FeedbackUnicode"] + "'

                                                                    }

                                                                    else if (checkheader == true)
                                                                    {

                                                                        questionmasterPK = Convert.ToString(FpSpread1.Cells[tagrowcount - 1, j - 1].Tag);
                                                                        if (questionmasterPK != "")
                                                                        {

                                                                            ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";
                                                                        }

                                                                    }

                                                                    dv = ds.Tables[2].DefaultView;
                                                                    if (dv.Count > 0)
                                                                    {
                                                                        QuestionAttendcount++;
                                                                        string point1 = Convert.ToString(dv[0]["points"]);
                                                                        if (string.IsNullOrEmpty(point1.Trim()) || point1.Trim() == "-")
                                                                            point1 = "0";
                                                                        double questavgpoint = Convert.ToDouble(point1) / maximun * Convert.ToDouble(sum_total);// 100;
                                                                        questavgpoint = Math.Round(questavgpoint, 0, MidpointRounding.AwayFromZero);
                                                                        //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(Math.Round(questavgpoint, 2)); //Convert.ToString(dv[0]["points"]);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(questavgpoint);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                                        gtotal += Convert.ToDouble((Math.Round(questavgpoint, 2)));
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = "-";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                                    }
                                                                    FpSpread1.Columns[j - 1].Locked = true;
                                                                    FpSpread1.Columns[4].Locked = true;
                                                                }

                                                            }

                                                            if (checkgreatercol == false)
                                                            {
                                                                //Modified by saranya on 27Aug2018
                                                                Double strength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                                double calfbcal = Convert.ToDouble(attendstrength) * Convert.ToDouble(question_count) * Convert.ToDouble(sum_total);
                                                                double fbavg = (gtotal / calfbcal) * 100;
                                                                double avg = Convert.ToDouble(Math.Round(fbavg, 2));
                                                                string studentcount = "";
                                                                if (Convert.ToString(ds.Tables[0].Rows[i]["Strength"]).Trim() != "")
                                                                {
                                                                    studentcount = Convert.ToString(ds.Tables[0].Rows[i]["Strength"]);
                                                                }
                                                                else
                                                                {
                                                                    studentcount = "-";
                                                                }
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].CellType = new FarPoint.Web.Spread.TextCellType();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = Convert.ToString(studentcount);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";

                                                                double averag = (Math.Round(Math.Round(gtotal, 2) / Math.Round((question_count * Convert.ToDouble(sum_total)), 2) * 100, 2));
                                                                //barath 31.07.17 *100 added
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(averag, 2));
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                            }
                                                            if (checkgreatercol == true)
                                                            {
                                                                Double strength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                                double calfbcal = Convert.ToDouble(attendstrength) * Convert.ToDouble(question_count) * Convert.ToDouble(sum_total);
                                                                double fbavg = (gtotal / calfbcal) * 100;
                                                                double avg = Convert.ToDouble(Math.Round(fbavg, 2));
                                                                string studentcount = "";
                                                                if (Convert.ToString(ds.Tables[0].Rows[i]["Strength"]).Trim() != "")
                                                                {
                                                                    studentcount = Convert.ToString(ds.Tables[0].Rows[i]["Strength"]);
                                                                }
                                                                else
                                                                {
                                                                    studentcount = "-";
                                                                }

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].CellType = new FarPoint.Web.Spread.TextCellType();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(studentcount);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";

                                                                double averag = (Math.Round(Math.Round(gtotal, 2) / Math.Round((question_count * Convert.ToDouble(sum_total)), 2) * 100, 2));
                                                                //barath 31.07.17 *100 added
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = String.Format("{0:0.00}", Math.Round(averag, 2));
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";

                                                            }

                                                        }

                                                    }
                                                    else
                                                    {
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1]
                                                        FpSpread1.Sheets[0].RowCount--;

                                                        if (feedbak == "")
                                                        {
                                                            feedbak = Convert.ToString(cblfeedbackmulti.Items[fb].Value);
                                                        }
                                                        else
                                                        {
                                                            feedbak = feedbak + "," + Convert.ToString(cblfeedbackmulti.Items[fb].Value);
                                                        }

                                                        lbl_error.Visible = true;
                                                        lbl_error.Text = "No Records Found for" + "-" + feedbak;
                                                        SpreadDiv.Visible = false;
                                                    }
                                                }
                                                else
                                                {
                                                    lbl_error.Visible = true;
                                                    lbl_error.Text = "No Records Found";
                                                    SpreadDiv.Visible = false;
                                                }

                                            }
                                            else
                                            {
                                                SpreadDiv.Visible = false;
                                                lbl_error.Visible = true;
                                                lbl_error.Text = "Please select all fields";
                                            }
                                            checkheader = true;
                                        }

                                    }

                                    FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;

                                    FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    FpSpread1.Height = 500;
                                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                    SpreadDiv.Visible = true;
                                }
                            }

                            #endregion

                            #region Added By Saranya On 28/08/2018 For ClassWise Without RoundOff

                            if (cb_WithOutRoundOff.Checked == true)
                            {
                                bool checkheader = false;
                                int overallcolcount = 0;
                                int colcountheader = 0;
                                int tagrowcount = 0;
                                int getcountrow = 0;
                                bool checkgreatercol = false;
                                string feebackname = string.Empty;
                                if (cblfeedbackmulti.Items.Count > 0)//delsi2910
                                {
                                    string feedbak = string.Empty;
                                    for (int fb = 0; fb < cblfeedbackmulti.Items.Count; fb++)
                                    {
                                        if (cblfeedbackmulti.Items[fb].Selected == true)
                                        {
                                            feebackname = Convert.ToString(cblfeedbackmulti.Items[fb].Value);
                                            lbl_error.Visible = false;
                                            Printcontrol1.Visible = false;
                                            string degreecode = rs.GetSelectedItemsValue(cbl_deptname);
                                            string sem = rs.GetSelectedItemsValue(cbl_sem);
                                            string batchyear = rs.GetSelectedItemsValue(cbl_batch);
                                            string clgcode = rs.GetSelectedItemsValue(cbl_clgname);
                                            string subjectcode = rs.GetSelectedItemsValue(Cbl_Subject);
                                            string sec = string.Empty;
                                            for (int i = 0; i < cbl_sec.Items.Count; i++)
                                            {
                                                if (cbl_sec.Items[i].Selected == true)
                                                {
                                                    if (string.IsNullOrEmpty(sec))
                                                        sec = cbl_sec.Items[i].Value.ToString();
                                                    else
                                                        sec = sec + "," + cbl_sec.Items[i].Value.ToString() + "";
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(degreecode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batchyear))
                                            {

                                                string type = "1";
                                                string fbpk = " select FeedBackMasterPK,ISNULL(issubjecttype,0)issubjecttype from CO_FeedBackMaster where FeedBackName ='" + feebackname + "'";
                                                DataSet dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                                                string feedbakpk = string.Empty;
                                                string issubjecttype = string.Empty;
                                                if (dsfb.Tables.Count > 0)
                                                {
                                                    if (dsfb.Tables[0].Rows.Count > 0)
                                                    {
                                                        issubjecttype = Convert.ToString(dsfb.Tables[0].Rows[0]["issubjecttype"]);
                                                        for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                                                        {
                                                            if (string.IsNullOrEmpty(feedbakpk))
                                                                feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                                            else
                                                                feedbakpk = feedbakpk + "," + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                                                        }
                                                    }
                                                }
                                                if (checkheader == false)
                                                {
                                                    rs.Fpreadheaderbindmethod("S No-50/Department-200/StaffCode-100/StaffName-200/Subject Code-150/Subject Name-250/SubjectType-100", FpSpread1, "false");
                                                }
                                                else if (checkheader == true)
                                                {
                                                    FpSpread1.Sheets[0].RowCount++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "S.No";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Columns[0].Width = 50;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.White;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Department";
                                                    FpSpread1.Columns[1].Width = 200;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Columns[2].Width = 200;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].ForeColor = Color.White;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "StaffCode";
                                                    FpSpread1.Columns[3].Width = 200;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.White;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = "StaffName";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].ForeColor = Color.White;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "Subject Code";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                                                    FpSpread1.Columns[4].Width = 200;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "Subject Name";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = Color.White;
                                                    FpSpread1.Columns[5].Width = 200;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "SubjectType";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.White;
                                                    FpSpread1.Columns[6].Width = 200;


                                                }
                                                Hashtable hat = new Hashtable();
                                                hat.Add("@CollegeCode", clgcode);
                                                hat.Add("@batchyear", batchyear);
                                                hat.Add("@Degreecode", degreecode);
                                                hat.Add("@semester", sem);
                                                hat.Add("@section", sec);
                                                hat.Add("@FeedbackName", Convert.ToString(feebackname));
                                                hat.Add("@FeedbackMasterFK", feedbakpk);
                                                hat.Add("@FeedbackType", type);
                                                hat.Add("@subjectno", subjectcode);
                                                ds = d2.select_method("[AnonymousReportClassWise]", hat, "sp");

                                                double question_count = 0;
                                                if (ds.Tables.Count > 0)
                                                {
                                                    if (ds.Tables[4].Rows.Count > 0)
                                                    {
                                                        double.TryParse(Convert.ToString(ds.Tables[4].Compute("sum(question_count)", "")), out question_count);
                                                    }
                                                    string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                                    if (checkheader == false)
                                                    {
                                                        if (ds.Tables[1].Rows.Count > 0)
                                                        {
                                                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                                            {
                                                                colcountheader = ds.Tables[1].Rows.Count;
                                                                FpSpread1.Sheets[0].ColumnCount++;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[1].Rows[i]["QuestionMasterPK"]);
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 300;
                                                            }
                                                            FpSpread1.Sheets[0].ColumnCount++;
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "No.Of Students";
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                            FpSpread1.Sheets[0].ColumnCount++;
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Feedback Percentage";
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                        }
                                                    }

                                                    else if (checkheader == true)
                                                    {
                                                        if (ds.Tables[1].Rows.Count > 0)
                                                        {
                                                            overallcolcount = 6;
                                                            int totinc = 0;
                                                            if (colcountheader < ds.Tables[1].Rows.Count)
                                                            {
                                                                int getcount = ds.Tables[1].Rows.Count;
                                                                totinc = getcount - colcountheader;
                                                                for (int val = 0; val < totinc; val++)
                                                                {
                                                                    FpSpread1.Sheets[0].ColumnCount++;
                                                                    checkgreatercol = false;
                                                                }
                                                            }
                                                            if (colcountheader > ds.Tables[1].Rows.Count)
                                                            {
                                                                checkgreatercol = true;

                                                            }

                                                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                                            {
                                                                overallcolcount++;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Text = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Tag = Convert.ToString(ds.Tables[1].Rows[i]["QuestionMasterPK"]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Font.Name = "Book Antiqua";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].ForeColor = Color.White;
                                                                tagrowcount = FpSpread1.Sheets[0].RowCount;
                                                                FpSpread1.Columns[overallcolcount].Width = 200;


                                                            }

                                                            overallcolcount++;
                                                            for (int col = 0; col < FpSpread1.Columns.Count; col++)
                                                            {
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                            }

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = "No.Of Students";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].ForeColor = Color.White;
                                                            overallcolcount++;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Feedback Percentage";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;

                                                        }
                                                    }

                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        int k = 0; string staffname = ""; int s = 1;
                                                        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                                                        cb.AutoPostBack = true;
                                                        FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                                                        cb1.AutoPostBack = false;
                                                        double staffavg = 0; bool staffinvdiavg = false; double sumavgpoint = 0; int staffrowcount = 0;

                                                        FpSpread1.Sheets[0].RowCount++;
                                                        int colcnt = FpSpread1.Sheets[0].ColumnCount;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(cblfeedbackmulti.Items[fb].Value);//delsis29
                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, colcnt);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.MistyRose;

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;

                                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                                        {
                                                            staffinvdiavg = false;
                                                            FpSpread1.Sheets[0].RowCount++;
                                                            if (staffname.Trim() == "")
                                                            {
                                                                k++;
                                                            }
                                                            else if (staffname == ds.Tables[0].Rows[i]["staff_name"].ToString())
                                                            {
                                                                k++; staffrowcount++;
                                                            }
                                                            else
                                                            {
                                                                k = 1; s++;
                                                                //FpSpread1.Sheets[0].RowCount++;                                          
                                                                staffavg = ((staffavg / (Convert.ToDouble(staffrowcount + 1) * 100)) * 100);
                                                                double.TryParse(Convert.ToString(Math.Round(staffavg, 2)), out sumavgpoint);
                                                                staffinvdiavg = true;
                                                            }
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(s);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["department"].ToString();
                                                            staffname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                                            string staff = Convert.ToString(ds.Tables[0].Rows[i]["staff"]);
                                                            string[] staffSplit = staff.Split('-');

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = staffSplit[0];//k.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = staffSplit[1];//k.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["subject_code"].ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["Subject_Name"].ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["acronym"].ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;

                                                            double gtotal = 0; double mtotal = 0; double avgper = 0;
                                                            string filterquery = string.Empty;
                                                            string section = Convert.ToString(ds.Tables[0].Rows[i]["Section"]);
                                                            filterquery = "degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["DegreeCode"]) + "' ";
                                                            if (section.Trim() != "")
                                                            {
                                                                filterquery = filterquery + " and Sections='" + section + "'";
                                                            }
                                                            ds.Tables[3].DefaultView.RowFilter = "" + filterquery + "";
                                                            DataView dvnew = ds.Tables[3].DefaultView;
                                                            string totalstudnent = "";
                                                            if (dvnew.Count > 0)
                                                            {
                                                                totalstudnent = Convert.ToString(dvnew[0]["studentcount"]);
                                                            }
                                                            if (totalstudnent.Trim() == "")
                                                                totalstudnent = "0";

                                                            Double attendstrength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                            double maximun = Convert.ToDouble(sum_total) * Convert.ToDouble(attendstrength);
                                                            double QuestionAttendcount = 0;
                                                            if (checkgreatercol == false)
                                                            {
                                                                for (int j = 8; j <= FpSpread1.Columns.Count - 2; j++)
                                                                {
                                                                    string questionmasterPK = string.Empty;
                                                                    DataView dv = new DataView();
                                                                    if (checkheader == false)
                                                                    {
                                                                        questionmasterPK = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, j - 1].Tag);
                                                                        ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";
                                                                    }
                                                                    else if (checkheader == true)
                                                                    {

                                                                        questionmasterPK = Convert.ToString(FpSpread1.Cells[tagrowcount - 1, j - 1].Tag);
                                                                        if (questionmasterPK != "")
                                                                        {

                                                                            ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";
                                                                        }

                                                                    }
                                                                    dv = ds.Tables[2].DefaultView;
                                                                    if (dv.Count > 0)
                                                                    {
                                                                        QuestionAttendcount++;
                                                                        string point1 = Convert.ToString(dv[0]["points"]);
                                                                        if (string.IsNullOrEmpty(point1.Trim()) || point1.Trim() == "-")
                                                                            point1 = "0";
                                                                        double questavgpoint = Convert.ToDouble(point1) / maximun * Convert.ToDouble(sum_total);// 100;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(Math.Round(questavgpoint, 2));
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                                        gtotal += Convert.ToDouble((Math.Round(questavgpoint, 2)));
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = "-";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                                    }
                                                                    FpSpread1.Columns[j - 1].Locked = true;
                                                                    FpSpread1.Columns[4].Locked = true;
                                                                }
                                                            }

                                                            if (checkgreatercol == true)
                                                            {
                                                                for (int j = 8; j <= overallcolcount - 1; j++)//modified by saranya on 27Aug2018 FpSpread1.Columns.Count changed to FpSpread1.Columns.Count - 2
                                                                {
                                                                    string questionmasterPK = string.Empty;
                                                                    DataView dv = new DataView();
                                                                    if (checkheader == false)
                                                                    {
                                                                        questionmasterPK = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, j - 1].Tag);
                                                                        ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";//and FeedbackUnicode='" + ds.Tables[0].Rows[i]["FeedbackUnicode"] + "'

                                                                    }

                                                                    else if (checkheader == true)
                                                                    {

                                                                        questionmasterPK = Convert.ToString(FpSpread1.Cells[tagrowcount - 1, j - 1].Tag);
                                                                        if (questionmasterPK != "")
                                                                        {

                                                                            ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";
                                                                        }

                                                                    }

                                                                    dv = ds.Tables[2].DefaultView;
                                                                    if (dv.Count > 0)
                                                                    {
                                                                        QuestionAttendcount++;
                                                                        string point1 = Convert.ToString(dv[0]["points"]);
                                                                        if (string.IsNullOrEmpty(point1.Trim()) || point1.Trim() == "-")
                                                                            point1 = "0";
                                                                        double questavgpoint = Convert.ToDouble(point1) / maximun * Convert.ToDouble(sum_total);// 100;
                                                                        questavgpoint = Math.Round(questavgpoint, 0, MidpointRounding.AwayFromZero);
                                                                        //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(Math.Round(questavgpoint, 2)); //Convert.ToString(dv[0]["points"]);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(questavgpoint);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                                        gtotal += Convert.ToDouble((Math.Round(questavgpoint, 2)));
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = "-";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                                    }
                                                                    FpSpread1.Columns[j - 1].Locked = true;
                                                                    FpSpread1.Columns[4].Locked = true;
                                                                }

                                                            }
                                                            if (checkgreatercol == false)
                                                            {
                                                                Double strength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                                double calfbcal = Convert.ToDouble(attendstrength) * Convert.ToDouble(question_count) * Convert.ToDouble(sum_total);
                                                                double fbavg = (gtotal / calfbcal) * 100;
                                                                double avg = Convert.ToDouble(Math.Round(fbavg, 2));
                                                                string studentcount = "";
                                                                if (Convert.ToString(ds.Tables[0].Rows[i]["Strength"]).Trim() != "")
                                                                {
                                                                    studentcount = Convert.ToString(ds.Tables[0].Rows[i]["Strength"]);
                                                                }
                                                                else
                                                                {
                                                                    studentcount = "-";
                                                                }
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].CellType = new FarPoint.Web.Spread.TextCellType();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = Convert.ToString(studentcount);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";

                                                                double averag = (Math.Round(Math.Round(gtotal, 2) / Math.Round((question_count * Convert.ToDouble(sum_total)), 2) * 100, 2));
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(averag, 2));
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                            }

                                                            if (checkgreatercol == true)
                                                            {
                                                                Double strength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                                double calfbcal = Convert.ToDouble(attendstrength) * Convert.ToDouble(question_count) * Convert.ToDouble(sum_total);
                                                                double fbavg = (gtotal / calfbcal) * 100;
                                                                double avg = Convert.ToDouble(Math.Round(fbavg, 2));
                                                                string studentcount = "";
                                                                if (Convert.ToString(ds.Tables[0].Rows[i]["Strength"]).Trim() != "")
                                                                {
                                                                    studentcount = Convert.ToString(ds.Tables[0].Rows[i]["Strength"]);
                                                                }
                                                                else
                                                                {
                                                                    studentcount = "-";
                                                                }

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].CellType = new FarPoint.Web.Spread.TextCellType();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(studentcount);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";

                                                                double averag = (Math.Round(Math.Round(gtotal, 2) / Math.Round((question_count * Convert.ToDouble(sum_total)), 2) * 100, 2));
                                                                //barath 31.07.17 *100 added
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = String.Format("{0:0.00}", Math.Round(averag, 2));
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";

                                                            }

                                                        }

                                                    }
                                                    else
                                                    {
                                                        FpSpread1.Sheets[0].RowCount--;

                                                        if (feedbak == "")
                                                        {
                                                            feedbak = Convert.ToString(cblfeedbackmulti.Items[fb].Value);
                                                        }
                                                        else
                                                        {
                                                            feedbak = feedbak + "," + Convert.ToString(cblfeedbackmulti.Items[fb].Value);
                                                        }

                                                        lbl_error.Visible = true;
                                                        lbl_error.Text = "No Records Found for" + "-" + feedbak;
                                                        SpreadDiv.Visible = false;

                                                    }
                                                }
                                                else
                                                {
                                                    lbl_error.Visible = true;
                                                    lbl_error.Text = "No Records Found";
                                                    SpreadDiv.Visible = false;
                                                }

                                            }
                                            else
                                            {
                                                SpreadDiv.Visible = false;
                                                lbl_error.Visible = true;
                                                lbl_error.Text = "Please select all fields";
                                            }
                                            checkheader = true;
                                        }
                                    }

                                    FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    FpSpread1.Height = 500;
                                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                    SpreadDiv.Visible = true;
                                }
                            }
                            #endregion
                        }
                    }
                    else if (Rdbquesacr.Checked == true)//delsi2510
                    {

                        if (rdb_classwise.Checked == true)//delsi1903
                        {
                            #region ClassWise With RoundOff

                            if (cb_WithOutRoundOff.Checked == false)
                            {
                                bool checkheader = false;
                                int overallcolcount = 0;
                                int colcountheader = 0;
                                int tagrowcount = 0;
                                int getcountrow = 0;
                                bool checkgreatercol = false;
                                string feebackname = string.Empty;
                                if (cblfeedbackmulti.Items.Count > 0)//delsi2910
                                {
                                    string feedbak = string.Empty;
                                    for (int fb = 0; fb < cblfeedbackmulti.Items.Count; fb++)
                                    {
                                        if (cblfeedbackmulti.Items[fb].Selected == true)
                                        {
                                            feebackname = Convert.ToString(cblfeedbackmulti.Items[fb].Value);
                                            lbl_error.Visible = false;
                                            Printcontrol1.Visible = false;
                                            string degreecode = rs.GetSelectedItemsValue(cbl_deptname);
                                            string sem = rs.GetSelectedItemsValue(cbl_sem);
                                            string batchyear = rs.GetSelectedItemsValue(cbl_batch);
                                            string clgcode = rs.GetSelectedItemsValue(cbl_clgname);
                                            //   string StaffAppID = rs.GetSelectedItemsValue(cbl_staffname);
                                            //   string degree = rs.GetSelectedItemsValue(cbl_degree);
                                            string subjectcode = rs.GetSelectedItemsValue(Cbl_Subject);

                                            string sec = string.Empty;
                                            for (int i = 0; i < cbl_sec.Items.Count; i++)
                                            {
                                                if (cbl_sec.Items[i].Selected == true)
                                                {
                                                    if (string.IsNullOrEmpty(sec))
                                                        sec = cbl_sec.Items[i].Value.ToString();
                                                    else
                                                        sec = sec + "," + cbl_sec.Items[i].Value.ToString() + "";
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(degreecode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batchyear))
                                            {

                                                string type = "1";
                                                string fbpk = " select FeedBackMasterPK,ISNULL(issubjecttype,0)issubjecttype from CO_FeedBackMaster where FeedBackName ='" + feebackname + "'";
                                                DataSet dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                                                string feedbakpk = string.Empty;
                                                string issubjecttype = string.Empty;
                                                if (dsfb.Tables.Count > 0)
                                                {
                                                    if (dsfb.Tables[0].Rows.Count > 0)
                                                    {
                                                        issubjecttype = Convert.ToString(dsfb.Tables[0].Rows[0]["issubjecttype"]);
                                                        for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                                                        {
                                                            if (string.IsNullOrEmpty(feedbakpk))
                                                                feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                                            else
                                                                feedbakpk = feedbakpk + "," + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                                                        }
                                                    }
                                                }
                                                //rs.Fpreadheaderbindmethod("S No-50/Department-200/StaffCode & StaffName-250/Subject Code-150/Subject Name-250/SubjectType-100", FpSpread1, "false");

                                                //Modified by saranya on 20/08/2018
                                                if (checkheader == false)
                                                {
                                                    rs.Fpreadheaderbindmethod("S No-50/Department-200/StaffCode-100/StaffName-200/Subject Code-150/Subject Name-250/SubjectType-100", FpSpread1, "false");
                                                }
                                                else if (checkheader == true)
                                                {
                                                    FpSpread1.Sheets[0].RowCount++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "S.No";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Columns[0].Width = 50;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.White;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Department";
                                                    FpSpread1.Columns[1].Width = 200;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Columns[2].Width = 200;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].ForeColor = Color.White;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "StaffCode";
                                                    FpSpread1.Columns[3].Width = 200;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.White;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = "StaffName";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].ForeColor = Color.White;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "Subject Code";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                                                    FpSpread1.Columns[4].Width = 200;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "Subject Name";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = Color.White;
                                                    FpSpread1.Columns[5].Width = 200;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "SubjectType";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.White;
                                                    FpSpread1.Columns[6].Width = 200;


                                                }
                                                Hashtable hat = new Hashtable();
                                                hat.Add("@CollegeCode", clgcode);
                                                hat.Add("@batchyear", batchyear);
                                                hat.Add("@Degreecode", degreecode);
                                                hat.Add("@semester", sem);
                                                hat.Add("@section", sec);
                                                hat.Add("@FeedbackName", Convert.ToString(feebackname));
                                                hat.Add("@FeedbackMasterFK", feedbakpk);
                                                //   hat.Add("@StaffAppNo", StaffAppID);
                                                hat.Add("@FeedbackType", type);
                                                hat.Add("@subjectno", subjectcode);
                                                ds = d2.select_method("[AnonymousReportClassWise]", hat, "sp");
                                                //string question_count = d2.GetFunction("select COUNT( distinct QuestionMasterFK)question_count from CO_FeedBackQuestions where FeedBackMasterFK in ('" + feedbakpk + "')");
                                                double question_count = 0;
                                                if (ds.Tables.Count > 0)
                                                {
                                                    if (ds.Tables[4].Rows.Count > 0)
                                                    {
                                                        double.TryParse(Convert.ToString(ds.Tables[4].Compute("sum(question_count)", "")), out question_count);
                                                    }
                                                    string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                                    if (checkheader == false)
                                                    {
                                                        if (ds.Tables[1].Rows.Count > 0)
                                                        {
                                                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                                            {
                                                                colcountheader = ds.Tables[1].Rows.Count;
                                                                FpSpread1.Sheets[0].ColumnCount++;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[1].Rows[i]["questionacr"]);
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[1].Rows[i]["QuestionMasterPK"]);
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 100;
                                                            }
                                                            //========Modified By saranya on 27/08/2018=======//
                                                            FpSpread1.Sheets[0].ColumnCount++;
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "No.Of Students";
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                            FpSpread1.Sheets[0].ColumnCount++;
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Feedback Percentage";
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                            //==============================================//

                                                            //FpSpread1.Sheets[0].ColumnCount++;
                                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Maximum Total";
                                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Name = "Book Antiqua";
                                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Bold = true;
                                                        }
                                                    }
                                                    else if (checkheader == true)
                                                    {
                                                        if (ds.Tables[1].Rows.Count > 0)
                                                        {
                                                            overallcolcount = 6;
                                                            int totinc = 0;
                                                            if (colcountheader < ds.Tables[1].Rows.Count)
                                                            {
                                                                int getcount = ds.Tables[1].Rows.Count;
                                                                totinc = getcount - colcountheader;
                                                                for (int val = 0; val < totinc; val++)
                                                                {
                                                                    FpSpread1.Sheets[0].ColumnCount++;
                                                                    checkgreatercol = false;
                                                                }
                                                            }
                                                            if (colcountheader > ds.Tables[1].Rows.Count)
                                                            {
                                                                checkgreatercol = true;

                                                            }

                                                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                                            {
                                                                overallcolcount++;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Text = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Tag = Convert.ToString(ds.Tables[1].Rows[i]["QuestionMasterPK"]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Font.Name = "Book Antiqua";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].ForeColor = Color.White;
                                                                tagrowcount = FpSpread1.Sheets[0].RowCount;
                                                                FpSpread1.Columns[overallcolcount].Width = 200;


                                                            }

                                                            for (int col = 0; col < FpSpread1.Columns.Count; col++)
                                                            {
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                            }
                                                            overallcolcount++;

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = "No.Of Students";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].ForeColor = Color.White;
                                                            overallcolcount++;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Feedback Percentage";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;

                                                        }
                                                    }


                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        int k = 0; string staffname = ""; int s = 1;
                                                        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                                                        cb.AutoPostBack = true;
                                                        FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                                                        cb1.AutoPostBack = false;
                                                        //FpSpread1.Sheets[0].RowCount++;
                                                        double staffavg = 0; bool staffinvdiavg = false; double sumavgpoint = 0; int staffrowcount = 0;
                                                        FpSpread1.Sheets[0].RowCount++;
                                                        int colcnt = FpSpread1.Sheets[0].ColumnCount;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(cblfeedbackmulti.Items[fb].Value);//delsis29
                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, colcnt);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.MistyRose;

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                                        {
                                                            staffinvdiavg = false;
                                                            FpSpread1.Sheets[0].RowCount++;
                                                            if (staffname.Trim() == "")
                                                            {
                                                                k++;
                                                            }
                                                            else if (staffname == ds.Tables[0].Rows[i]["staff_name"].ToString())
                                                            {
                                                                k++; staffrowcount++;
                                                            }
                                                            else
                                                            {
                                                                k = 1; s++;
                                                                //FpSpread1.Sheets[0].RowCount++;
                                                                //staffavg = (staffavg / Convert.ToDouble(staffrowcount+1));
                                                                staffavg = ((staffavg / (Convert.ToDouble(staffrowcount + 1) * 100)) * 100);
                                                                double.TryParse(Convert.ToString(Math.Round(staffavg, 2)), out sumavgpoint);
                                                                staffinvdiavg = true;
                                                            }
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(s);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["department"].ToString();
                                                            staffname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                                                            //Changed by saranya on 20/8/2018
                                                            string staff = Convert.ToString(ds.Tables[0].Rows[i]["staff"]);
                                                            string[] staffSplit = staff.Split('-');

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = staffSplit[0];//k.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = staffSplit[1];//k.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                                            ////////////////////////////////////

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["subject_code"].ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["Subject_Name"].ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["acronym"].ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = ds.Tables[0].Rows[i]["section"].ToString();
                                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Bold = true;

                                                            double gtotal = 0; double mtotal = 0; double avgper = 0;
                                                            string filterquery = string.Empty;
                                                            string section = Convert.ToString(ds.Tables[0].Rows[i]["Section"]);
                                                            filterquery = "degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["DegreeCode"]) + "' ";
                                                            if (section.Trim() != "")
                                                            {
                                                                filterquery = filterquery + " and Sections='" + section + "'";
                                                            }
                                                            ds.Tables[3].DefaultView.RowFilter = "" + filterquery + "";
                                                            DataView dvnew = ds.Tables[3].DefaultView;
                                                            string totalstudnent = "";
                                                            if (dvnew.Count > 0)
                                                            {
                                                                totalstudnent = Convert.ToString(dvnew[0]["studentcount"]);
                                                            }
                                                            if (totalstudnent.Trim() == "")
                                                                totalstudnent = "0";
                                                            //double maximun = Convert.ToDouble(question_count) * Convert.ToDouble(sum_total) * Convert.ToDouble(totalstudnent);
                                                            Double attendstrength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                            double maximun = Convert.ToDouble(sum_total) * Convert.ToDouble(attendstrength);
                                                            double QuestionAttendcount = 0;
                                                            if (checkgreatercol == false)
                                                            {
                                                                for (int j = 8; j <= FpSpread1.Columns.Count - 2; j++)//modified by saranya on 27Aug2018 FpSpread1.Columns.Count changed to FpSpread1.Columns.Count - 2
                                                                {
                                                                    string questionmasterPK = string.Empty;
                                                                    DataView dv = new DataView();
                                                                    if (checkheader == false)
                                                                    {
                                                                        questionmasterPK = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, j - 1].Tag);
                                                                        ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";//and FeedbackUnicode='" + ds.Tables[0].Rows[i]["FeedbackUnicode"] + "'
                                                                    }
                                                                    else if (checkheader == true)
                                                                    {

                                                                        questionmasterPK = Convert.ToString(FpSpread1.Cells[tagrowcount - 1, j - 1].Tag);
                                                                        if (questionmasterPK != "")
                                                                        {

                                                                            ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";
                                                                        }

                                                                    }
                                                                    dv = ds.Tables[2].DefaultView;
                                                                    if (dv.Count > 0)
                                                                    {
                                                                        QuestionAttendcount++;
                                                                        string point1 = Convert.ToString(dv[0]["points"]);
                                                                        if (string.IsNullOrEmpty(point1.Trim()) || point1.Trim() == "-")
                                                                            point1 = "0";
                                                                        double questavgpoint = Convert.ToDouble(point1) / maximun * Convert.ToDouble(sum_total);// 100;
                                                                        questavgpoint = Math.Round(questavgpoint, 0, MidpointRounding.AwayFromZero);
                                                                        //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(Math.Round(questavgpoint, 2)); //Convert.ToString(dv[0]["points"]);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(questavgpoint);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                                        gtotal += Convert.ToDouble((Math.Round(questavgpoint, 2)));
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = "-";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                                    }
                                                                    FpSpread1.Columns[j - 1].Locked = true;
                                                                    FpSpread1.Columns[4].Locked = true;
                                                                }
                                                            }
                                                            if (checkgreatercol == true)
                                                            {
                                                                for (int j = 8; j <= overallcolcount - 1; j++)//modified by saranya on 27Aug2018 FpSpread1.Columns.Count changed to FpSpread1.Columns.Count - 2
                                                                {
                                                                    string questionmasterPK = string.Empty;
                                                                    DataView dv = new DataView();
                                                                    if (checkheader == false)
                                                                    {
                                                                        questionmasterPK = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, j - 1].Tag);
                                                                        ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";//and FeedbackUnicode='" + ds.Tables[0].Rows[i]["FeedbackUnicode"] + "'

                                                                    }

                                                                    else if (checkheader == true)
                                                                    {

                                                                        questionmasterPK = Convert.ToString(FpSpread1.Cells[tagrowcount - 1, j - 1].Tag);
                                                                        if (questionmasterPK != "")
                                                                        {

                                                                            ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";
                                                                        }

                                                                    }

                                                                    dv = ds.Tables[2].DefaultView;
                                                                    if (dv.Count > 0)
                                                                    {
                                                                        QuestionAttendcount++;
                                                                        string point1 = Convert.ToString(dv[0]["points"]);
                                                                        if (string.IsNullOrEmpty(point1.Trim()) || point1.Trim() == "-")
                                                                            point1 = "0";
                                                                        double questavgpoint = Convert.ToDouble(point1) / maximun * Convert.ToDouble(sum_total);// 100;
                                                                        questavgpoint = Math.Round(questavgpoint, 0, MidpointRounding.AwayFromZero);
                                                                        //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(Math.Round(questavgpoint, 2)); //Convert.ToString(dv[0]["points"]);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(questavgpoint);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                                        gtotal += Convert.ToDouble((Math.Round(questavgpoint, 2)));
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = "-";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                                    }
                                                                    FpSpread1.Columns[j - 1].Locked = true;
                                                                    FpSpread1.Columns[4].Locked = true;
                                                                }

                                                            }

                                                            if (checkgreatercol == false)
                                                            {
                                                                //Modified by saranya on 27Aug2018
                                                                Double strength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                                double calfbcal = Convert.ToDouble(attendstrength) * Convert.ToDouble(question_count) * Convert.ToDouble(sum_total);
                                                                double fbavg = (gtotal / calfbcal) * 100;
                                                                double avg = Convert.ToDouble(Math.Round(fbavg, 2));
                                                                string studentcount = "";
                                                                if (Convert.ToString(ds.Tables[0].Rows[i]["Strength"]).Trim() != "")
                                                                {
                                                                    studentcount = Convert.ToString(ds.Tables[0].Rows[i]["Strength"]);
                                                                }
                                                                else
                                                                {
                                                                    studentcount = "-";
                                                                }
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].CellType = new FarPoint.Web.Spread.TextCellType();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = Convert.ToString(studentcount);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";

                                                                double averag = (Math.Round(Math.Round(gtotal, 2) / Math.Round((question_count * Convert.ToDouble(sum_total)), 2) * 100, 2));
                                                                //barath 31.07.17 *100 added
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(averag, 2));
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";

                                                                //==========================================================================//

                                                            }

                                                            if (checkgreatercol == true)
                                                            {
                                                                Double strength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                                double calfbcal = Convert.ToDouble(attendstrength) * Convert.ToDouble(question_count) * Convert.ToDouble(sum_total);
                                                                double fbavg = (gtotal / calfbcal) * 100;
                                                                double avg = Convert.ToDouble(Math.Round(fbavg, 2));
                                                                string studentcount = "";
                                                                if (Convert.ToString(ds.Tables[0].Rows[i]["Strength"]).Trim() != "")
                                                                {
                                                                    studentcount = Convert.ToString(ds.Tables[0].Rows[i]["Strength"]);
                                                                }
                                                                else
                                                                {
                                                                    studentcount = "-";
                                                                }

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].CellType = new FarPoint.Web.Spread.TextCellType();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(studentcount);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";

                                                                double averag = (Math.Round(Math.Round(gtotal, 2) / Math.Round((question_count * Convert.ToDouble(sum_total)), 2) * 100, 2));
                                                                //barath 31.07.17 *100 added
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = String.Format("{0:0.00}", Math.Round(averag, 2));
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";

                                                            }
                                                        }

                                                        FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                                                        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                                                        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                                                        FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;

                                                        FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                                                        FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                                                        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                                                        string selqry = "";
                                                        DataSet dsnew = new DataSet();

                                                        string college_cd = string.Empty;
                                                        if (cbl_clgname.Items.Count > 0)
                                                        {
                                                            for (int i = 0; i < cbl_clgname.Items.Count; i++)
                                                            {
                                                                if (cbl_clgname.Items[i].Selected == true)
                                                                {
                                                                    if (college_cd == "")
                                                                    {
                                                                        college_cd = "" + cbl_clgname.Items[i].Value.ToString() + "";
                                                                    }
                                                                    else
                                                                    {
                                                                        college_cd = college_cd + "','" + Convert.ToString(cbl_clgname.Items[i].Value);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        string Batch_Year = "";
                                                        for (int i = 0; i < cbl_batch.Items.Count; i++)
                                                        {
                                                            if (cbl_batch.Items[i].Selected == true)
                                                            {
                                                                if (Batch_Year == "")
                                                                {
                                                                    Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                                                                }
                                                                else
                                                                {
                                                                    Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                                                                }
                                                            }
                                                        }
                                                        string degree_code = "";
                                                        for (int i = 0; i < cbl_deptname.Items.Count; i++)
                                                        {
                                                            if (cbl_deptname.Items[i].Selected == true)
                                                            {
                                                                if (degree_code == "")
                                                                {
                                                                    degree_code = "" + cbl_deptname.Items[i].Value.ToString() + "";
                                                                }
                                                                else
                                                                {
                                                                    degree_code = degree_code + "','" + cbl_deptname.Items[i].Value.ToString() + "";
                                                                }
                                                            }
                                                        }
                                                        string sections = "";
                                                        for (int i = 0; i < cbl_sec.Items.Count; i++)
                                                        {
                                                            if (cbl_sec.Items[i].Selected == true)
                                                            {
                                                                if (sections == "")
                                                                {
                                                                    sections = "" + cbl_sec.Items[i].Value.ToString() + "";
                                                                }
                                                                else
                                                                {
                                                                    sections = sections + "','" + cbl_sec.Items[i].Value.ToString() + "";
                                                                }
                                                                if (cbl_sec.Items[i].Value == "Empty")
                                                                {
                                                                    sections = "";
                                                                }
                                                            }
                                                        }
                                                        if (sections.Trim() != "")
                                                        {
                                                            sections = sections + "','";
                                                        }
                                                        string semester = "";
                                                        for (int i = 0; i < cbl_sem.Items.Count; i++)
                                                        {
                                                            if (cbl_sem.Items[i].Selected == true)
                                                            {
                                                                if (semester == "")
                                                                {
                                                                    semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                                                                }
                                                                else
                                                                {
                                                                    semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                                                                }
                                                            }
                                                        }


                                                        selqry = " select COUNT( distinct  s.FeedbackUnicode) as Strength,f.FeedBackMasterPK,Batch_Year ,f.semester,f.DegreeCode,f.Section from CO_FeedBackMaster F,CO_StudFeedBack S where s.FeedBackMasterFK =f.FeedBackMasterPK  and f.degreecode in ('" + degree_code + "') and f.Batch_Year in('" + Batch_Year + "') and f.semester in ('" + semester + "') and f.FeedBackName='" + Convert.ToString(feebackname) + "'  and f.InclueCommon='1' and s.FeedbackUnicode<>''";
                                                        if (sections != "")
                                                        {
                                                            selqry = selqry + " and f.Section in ('" + sections + "')  ";
                                                        }
                                                        selqry = selqry + " group by f.FeedBackMasterPK  ,Batch_Year ,f.semester,f.DegreeCode ,f.Section";
                                                        selqry = selqry + " SELECT Course_Name+'-'+Dept_Name Degree,Current_semester,R.degree_code,Sections,Batch_Year,COUNT(*)as TotStrengh FROM Registration R,Degree G,Course C,Department D WHERE R.degree_code = G.Degree_Code AND G.Course_Id = C.Course_Id AND G.college_code = C.college_code AND g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' and r.degree_code in ('" + degree_code + "') and r.Batch_Year in('" + Batch_Year + "') ";//and 
                                                        if (sections != "")
                                                        {
                                                            selqry = selqry + " and r.Sections in ('" + sections + "') GROUP BY Course_Name,R.degree_code, Dept_Name,Current_semester,Sections, Batch_Year ORDER BY R.degree_code,Current_Semester,Sections , Batch_Year,Course_Name,Dept_Name ";
                                                        }
                                                        else
                                                        {
                                                            selqry = selqry + " GROUP BY Course_Name, R.degree_code, Dept_Name,Current_semester,Sections,Batch_Year ORDER BY R.degree_code,Current_Semester,Sections , Batch_Year,Course_Name,Dept_Name";
                                                        }
                                                        selqry = selqry + "   SELECT Course_Name+'-'+Dept_Name Degree,semester,f.DegreeCode,Section,Batch_Year FROM Degree G,Course C,Department D,CO_FeedBackMaster F,CO_StudFeedBack S WHERE f.FeedBackMasterPK=s.FeedBackMasterFK and f.DegreeCode = G.Degree_Code AND G.Course_Id = C.Course_Id AND G.college_code = C.college_code AND g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND  f.degreecode in ('" + degree_code + "') and f.Batch_Year in('" + Batch_Year + "') and f.Semester in ('" + semester + "') and f.FeedBackName='" + Convert.ToString(feebackname) + "'  and f.InclueCommon='1' and s.FeedbackUnicode<>''";
                                                        if (sections != "")
                                                        {
                                                            selqry = selqry + " and f.Section in ('" + sections + "')   GROUP BY Course_Name,f.degreecode, Dept_Name,semester,Section, Batch_Year ORDER BY f.degreecode,Semester,Section , Batch_Year,Course_Name,Dept_Name ";
                                                        }
                                                        else
                                                        {
                                                            selqry = selqry + "   GROUP BY Course_Name,f.degreecode, Dept_Name,semester,Section, Batch_Year ORDER BY f.degreecode,Semester,Section , Batch_Year,Course_Name,Dept_Name ";
                                                        }


                                                        dsnew = d2.select_method_wo_parameter(selqry, "text");
                                                        DataView dvnews = new DataView();
                                                        DataView totalview = new DataView();
                                                        int overallstrength = 0;
                                                        int attended = 0;
                                                        if (dsnew.Tables.Count > 0)
                                                        {
                                                            if (dsnew.Tables[0].Rows.Count > 0 && dsnew.Tables[1].Rows.Count > 0 && dsnew.Tables[2].Rows.Count > 0)
                                                            {

                                                                for (int i = 0; i < dsnew.Tables[2].Rows.Count; i++)
                                                                {
                                                                    string sectons = Convert.ToString(dsnew.Tables[2].Rows[i]["Section"]);
                                                                    string degrecode = Convert.ToString(dsnew.Tables[2].Rows[i]["degreecode"]);
                                                                    string getsem = Convert.ToString(dsnew.Tables[2].Rows[i]["semester"]);
                                                                    string totalfind = " degree_code='" + degrecode + "'  and  Batch_Year='" + dsnew.Tables[2].Rows[i]["Batch_Year"].ToString() + "'";
                                                                    if (sectons.Trim() != "")
                                                                    {
                                                                        totalfind = totalfind + " and Sections='" + sectons + "'";
                                                                    }
                                                                    dsnew.Tables[1].DefaultView.RowFilter = "" + totalfind + "";
                                                                    totalview = dsnew.Tables[1].DefaultView;
                                                                    int total = 0;
                                                                    if (totalview.Count > 0)
                                                                    {

                                                                        string totas = totalview[0]["TotStrengh"].ToString();
                                                                        if (totas.Trim() == "")
                                                                        {
                                                                            totas = "0";
                                                                        }
                                                                        total = Convert.ToInt32(totas);
                                                                        overallstrength = overallstrength + total;
                                                                    }
                                                                    string filterquery = "";

                                                                    int attand = 0;
                                                                    filterquery = "degreecode='" + degrecode + "'  and  semester='" + getsem + "' ";
                                                                    if (sectons.Trim() != "")
                                                                    {
                                                                        filterquery = filterquery + " and Section='" + sectons + "'";
                                                                    }
                                                                    dsnew.Tables[0].DefaultView.RowFilter = "" + filterquery + "";
                                                                    dvnews = dsnew.Tables[0].DefaultView;
                                                                    if (dvnews.Count > 0)
                                                                    {

                                                                        attand = Convert.ToInt32(dvnews[0]["Strength"]);
                                                                    }
                                                                    attended = attended + attand;
                                                                }

                                                            }
                                                        }



                                                        int colcount = FpSpread1.Sheets[0].ColumnCount;
                                                        FpSpread1.Sheets[0].RowCount++;
                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, colcount);


                                                        FpSpread1.Sheets[0].RowCount++;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Students Strength:" + Convert.ToString(overallstrength);
                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, colcount);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].RowCount++;

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "No.Of Students Feedback Obtained:" + Convert.ToString(attended);
                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, colcount);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                                        FpSpread1.Sheets[0].RowCount++;
                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, colcount);


                                                        FpSpread1.Sheets[0].RowCount++;
                                                        FpSpread1.Columns[0].Locked = true;
                                                        FpSpread1.Columns[1].Locked = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "QUESTION USED FOR THE ASSESMENT PROCESS";
                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, colcount);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)//delsi2610
                                                        {
                                                            FpSpread1.Sheets[0].RowCount++;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[1].Rows[j]["questionacr"]) + ".";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[1].Rows[j]["Question"]) + "?";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, colcount);

                                                        }

                                                    }
                                                    else
                                                    {
                                                        FpSpread1.Sheets[0].RowCount--;

                                                        if (feedbak == "")
                                                        {
                                                            feedbak = Convert.ToString(cblfeedbackmulti.Items[fb].Value);
                                                        }
                                                        else
                                                        {
                                                            feedbak = feedbak + "," + Convert.ToString(cblfeedbackmulti.Items[fb].Value);
                                                        }

                                                        lbl_error.Visible = true;
                                                        lbl_error.Text = "No Records Found for" + "-" + feedbak;
                                                        SpreadDiv.Visible = false;


                                                    }
                                                }
                                                else
                                                {
                                                    lbl_error.Visible = true;
                                                    lbl_error.Text = "No Records Found";
                                                    SpreadDiv.Visible = false;
                                                }


                                            }
                                            else
                                            {
                                                SpreadDiv.Visible = false;
                                                lbl_error.Visible = true;
                                                lbl_error.Text = "Please select all fields";
                                            }
                                            checkheader = true;
                                        }
                                    }
                                    FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    FpSpread1.Height = 500;
                                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                    SpreadDiv.Visible = true;
                                }
                            }

                            #endregion

                            #region Added By Saranya On 28/08/2018 For ClassWise Without RoundOff

                            if (cb_WithOutRoundOff.Checked == true)
                            {
                                bool checkheader = false;
                                int overallcolcount = 0;
                                int colcountheader = 0;
                                int tagrowcount = 0;
                                int getcountrow = 0;
                                bool checkgreatercol = false;
                                string feebackname = string.Empty;
                                if (cblfeedbackmulti.Items.Count > 0)//delsi2910
                                {
                                    string feedbak = string.Empty;
                                    for (int fb = 0; fb < cblfeedbackmulti.Items.Count; fb++)
                                    {
                                        if (cblfeedbackmulti.Items[fb].Selected == true)
                                        {
                                            feebackname = Convert.ToString(cblfeedbackmulti.Items[fb].Value);
                                            lbl_error.Visible = false;
                                            Printcontrol1.Visible = false;
                                            string degreecode = rs.GetSelectedItemsValue(cbl_deptname);
                                            string sem = rs.GetSelectedItemsValue(cbl_sem);
                                            string batchyear = rs.GetSelectedItemsValue(cbl_batch);
                                            string clgcode = rs.GetSelectedItemsValue(cbl_clgname);
                                            string subjectcode = rs.GetSelectedItemsValue(Cbl_Subject);
                                            string sec = string.Empty;
                                            for (int i = 0; i < cbl_sec.Items.Count; i++)
                                            {
                                                if (cbl_sec.Items[i].Selected == true)
                                                {
                                                    if (string.IsNullOrEmpty(sec))
                                                        sec = cbl_sec.Items[i].Value.ToString();
                                                    else
                                                        sec = sec + "," + cbl_sec.Items[i].Value.ToString() + "";
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(degreecode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batchyear))
                                            {

                                                string type = "1";
                                                string fbpk = " select FeedBackMasterPK,ISNULL(issubjecttype,0)issubjecttype from CO_FeedBackMaster where FeedBackName ='" + feebackname + "'";
                                                DataSet dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                                                string feedbakpk = string.Empty;
                                                string issubjecttype = string.Empty;
                                                if (dsfb.Tables.Count > 0)
                                                {
                                                    if (dsfb.Tables[0].Rows.Count > 0)
                                                    {
                                                        issubjecttype = Convert.ToString(dsfb.Tables[0].Rows[0]["issubjecttype"]);
                                                        for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                                                        {
                                                            if (string.IsNullOrEmpty(feedbakpk))
                                                                feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                                            else
                                                                feedbakpk = feedbakpk + "," + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                                                        }
                                                    }
                                                }
                                                if (checkheader == false)
                                                {
                                                    rs.Fpreadheaderbindmethod("S No-50/Department-200/StaffCode-100/StaffName-200/Subject Code-150/Subject Name-250/SubjectType-100", FpSpread1, "false");
                                                }
                                                else if (checkheader == true)
                                                {
                                                    FpSpread1.Sheets[0].RowCount++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "S.No";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Columns[0].Width = 50;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.White;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Department";
                                                    FpSpread1.Columns[1].Width = 200;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Columns[2].Width = 200;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].ForeColor = Color.White;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "StaffCode";
                                                    FpSpread1.Columns[3].Width = 200;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.White;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = "StaffName";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].ForeColor = Color.White;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "Subject Code";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = Color.White;
                                                    FpSpread1.Columns[4].Width = 200;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "Subject Name";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = Color.White;
                                                    FpSpread1.Columns[5].Width = 200;

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "SubjectType";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.White;
                                                    FpSpread1.Columns[6].Width = 200;


                                                }

                                                Hashtable hat = new Hashtable();
                                                hat.Add("@CollegeCode", clgcode);
                                                hat.Add("@batchyear", batchyear);
                                                hat.Add("@Degreecode", degreecode);
                                                hat.Add("@semester", sem);
                                                hat.Add("@section", sec);
                                                hat.Add("@FeedbackName", Convert.ToString(feebackname));
                                                hat.Add("@FeedbackMasterFK", feedbakpk);
                                                hat.Add("@FeedbackType", type);
                                                hat.Add("@subjectno", subjectcode);
                                                ds = d2.select_method("[AnonymousReportClassWise]", hat, "sp");

                                                double question_count = 0;
                                                if (ds.Tables.Count > 0)
                                                {
                                                    if (ds.Tables[4].Rows.Count > 0)
                                                    {
                                                        double.TryParse(Convert.ToString(ds.Tables[4].Compute("sum(question_count)", "")), out question_count);
                                                    }
                                                    string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                                    if (checkheader == false)
                                                    {
                                                        if (ds.Tables[1].Rows.Count > 0)
                                                        {
                                                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                                            {
                                                                colcountheader = ds.Tables[1].Rows.Count;
                                                                FpSpread1.Sheets[0].ColumnCount++;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[1].Rows[i]["questionacr"]);
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[1].Rows[i]["QuestionMasterPK"]);
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 100;
                                                            }
                                                            FpSpread1.Sheets[0].ColumnCount++;
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "No.Of Students";
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                            FpSpread1.Sheets[0].ColumnCount++;
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Feedback Percentage";
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                        }
                                                    }
                                                    else if (checkheader == true)
                                                    {
                                                        if (ds.Tables[1].Rows.Count > 0)
                                                        {
                                                            overallcolcount = 6;
                                                            int totinc = 0;
                                                            if (colcountheader < ds.Tables[1].Rows.Count)
                                                            {
                                                                int getcount = ds.Tables[1].Rows.Count;
                                                                totinc = getcount - colcountheader;
                                                                for (int val = 0; val < totinc; val++)
                                                                {
                                                                    FpSpread1.Sheets[0].ColumnCount++;
                                                                    checkgreatercol = false;
                                                                }
                                                            }
                                                            if (colcountheader > ds.Tables[1].Rows.Count)
                                                            {
                                                                checkgreatercol = true;

                                                            }

                                                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                                            {
                                                                overallcolcount++;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Text = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Tag = Convert.ToString(ds.Tables[1].Rows[i]["QuestionMasterPK"]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Font.Name = "Book Antiqua";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, overallcolcount].ForeColor = Color.White;
                                                                tagrowcount = FpSpread1.Sheets[0].RowCount;
                                                                FpSpread1.Columns[overallcolcount].Width = 200;


                                                            }
                                                            for (int col = 0; col < FpSpread1.Columns.Count; col++)
                                                            {
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                            }
                                                            overallcolcount++;

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = "No.Of Students";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].ForeColor = Color.White;
                                                            overallcolcount++;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Feedback Percentage";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.White;

                                                        }
                                                    }



                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        int k = 0; string staffname = ""; int s = 1;
                                                        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                                                        cb.AutoPostBack = true;
                                                        FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                                                        cb1.AutoPostBack = false;
                                                        double staffavg = 0; bool staffinvdiavg = false; double sumavgpoint = 0; int staffrowcount = 0;
                                                        FpSpread1.Sheets[0].RowCount++;
                                                        int colcnt = FpSpread1.Sheets[0].ColumnCount;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(cblfeedbackmulti.Items[fb].Value);//delsis29
                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, colcnt);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.MistyRose;

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                                        {
                                                            staffinvdiavg = false;
                                                            FpSpread1.Sheets[0].RowCount++;
                                                            if (staffname.Trim() == "")
                                                            {
                                                                k++;
                                                            }
                                                            else if (staffname == ds.Tables[0].Rows[i]["staff_name"].ToString())
                                                            {
                                                                k++; staffrowcount++;
                                                            }
                                                            else
                                                            {
                                                                k = 1; s++;
                                                                //FpSpread1.Sheets[0].RowCount++;                                          
                                                                staffavg = ((staffavg / (Convert.ToDouble(staffrowcount + 1) * 100)) * 100);
                                                                double.TryParse(Convert.ToString(Math.Round(staffavg, 2)), out sumavgpoint);
                                                                staffinvdiavg = true;
                                                            }
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(s);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["department"].ToString();
                                                            staffname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                                            string staff = Convert.ToString(ds.Tables[0].Rows[i]["staff"]);
                                                            string[] staffSplit = staff.Split('-');

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = staffSplit[0];//k.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = staffSplit[1];//k.ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["subject_code"].ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["Subject_Name"].ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["acronym"].ToString();
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;

                                                            double gtotal = 0; double mtotal = 0; double avgper = 0;
                                                            string filterquery = string.Empty;
                                                            string section = Convert.ToString(ds.Tables[0].Rows[i]["Section"]);
                                                            filterquery = "degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["DegreeCode"]) + "' ";
                                                            if (section.Trim() != "")
                                                            {
                                                                filterquery = filterquery + " and Sections='" + section + "'";
                                                            }
                                                            ds.Tables[3].DefaultView.RowFilter = "" + filterquery + "";
                                                            DataView dvnew = ds.Tables[3].DefaultView;
                                                            string totalstudnent = "";
                                                            if (dvnew.Count > 0)
                                                            {
                                                                totalstudnent = Convert.ToString(dvnew[0]["studentcount"]);
                                                            }
                                                            if (totalstudnent.Trim() == "")
                                                                totalstudnent = "0";

                                                            Double attendstrength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                            double maximun = Convert.ToDouble(sum_total) * Convert.ToDouble(attendstrength);
                                                            double QuestionAttendcount = 0;
                                                            if (checkgreatercol == false)
                                                            {
                                                                for (int j = 8; j <= FpSpread1.Columns.Count - 2; j++)
                                                                {
                                                                    string questionmasterPK = string.Empty;
                                                                    DataView dv = new DataView();
                                                                    if (checkheader == false)
                                                                    {
                                                                        questionmasterPK = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, j - 1].Tag);
                                                                        ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";
                                                                    }
                                                                    else if (checkheader == true)
                                                                    {

                                                                        questionmasterPK = Convert.ToString(FpSpread1.Cells[tagrowcount - 1, j - 1].Tag);
                                                                        if (questionmasterPK != "")
                                                                        {

                                                                            ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";
                                                                        }

                                                                    }

                                                                    dv = ds.Tables[2].DefaultView;
                                                                    if (dv.Count > 0)
                                                                    {
                                                                        QuestionAttendcount++;
                                                                        string point1 = Convert.ToString(dv[0]["points"]);
                                                                        if (string.IsNullOrEmpty(point1.Trim()) || point1.Trim() == "-")
                                                                            point1 = "0";
                                                                        double questavgpoint = Convert.ToDouble(point1) / maximun * Convert.ToDouble(sum_total);// 100;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(Math.Round(questavgpoint, 2));
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                                        gtotal += Convert.ToDouble((Math.Round(questavgpoint, 2)));
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = "-";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                                    }
                                                                    FpSpread1.Columns[j - 1].Locked = true;
                                                                    FpSpread1.Columns[4].Locked = true;
                                                                }
                                                            }

                                                            if (checkgreatercol == true)
                                                            {
                                                                for (int j = 8; j <= overallcolcount - 1; j++)//modified by saranya on 27Aug2018 FpSpread1.Columns.Count changed to FpSpread1.Columns.Count - 2
                                                                {
                                                                    string questionmasterPK = string.Empty;
                                                                    DataView dv = new DataView();
                                                                    if (checkheader == false)
                                                                    {
                                                                        questionmasterPK = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, j - 1].Tag);
                                                                        ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";//and FeedbackUnicode='" + ds.Tables[0].Rows[i]["FeedbackUnicode"] + "'

                                                                    }

                                                                    else if (checkheader == true)
                                                                    {

                                                                        questionmasterPK = Convert.ToString(FpSpread1.Cells[tagrowcount - 1, j - 1].Tag);
                                                                        if (questionmasterPK != "")
                                                                        {

                                                                            ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";
                                                                        }

                                                                    }

                                                                    dv = ds.Tables[2].DefaultView;
                                                                    if (dv.Count > 0)
                                                                    {
                                                                        QuestionAttendcount++;
                                                                        string point1 = Convert.ToString(dv[0]["points"]);
                                                                        if (string.IsNullOrEmpty(point1.Trim()) || point1.Trim() == "-")
                                                                            point1 = "0";
                                                                        double questavgpoint = Convert.ToDouble(point1) / maximun * Convert.ToDouble(sum_total);// 100;
                                                                        questavgpoint = Math.Round(questavgpoint, 0, MidpointRounding.AwayFromZero);
                                                                        //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(Math.Round(questavgpoint, 2)); //Convert.ToString(dv[0]["points"]);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(questavgpoint);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                                        gtotal += Convert.ToDouble((Math.Round(questavgpoint, 2)));
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = "-";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                                    }
                                                                    FpSpread1.Columns[j - 1].Locked = true;
                                                                    FpSpread1.Columns[4].Locked = true;
                                                                }

                                                            }


                                                            if (checkgreatercol == false)
                                                            {

                                                                Double strength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                                double calfbcal = Convert.ToDouble(attendstrength) * Convert.ToDouble(question_count) * Convert.ToDouble(sum_total);
                                                                double fbavg = (gtotal / calfbcal) * 100;
                                                                double avg = Convert.ToDouble(Math.Round(fbavg, 2));
                                                                string studentcount = "";
                                                                if (Convert.ToString(ds.Tables[0].Rows[i]["Strength"]).Trim() != "")
                                                                {
                                                                    studentcount = Convert.ToString(ds.Tables[0].Rows[i]["Strength"]);
                                                                }
                                                                else
                                                                {
                                                                    studentcount = "-";
                                                                }
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].CellType = new FarPoint.Web.Spread.TextCellType();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = Convert.ToString(studentcount);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";

                                                                double averag = (Math.Round(Math.Round(gtotal, 2) / Math.Round((question_count * Convert.ToDouble(sum_total)), 2) * 100, 2));
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(averag, 2));
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";


                                                            }

                                                            if (checkgreatercol == true)
                                                            {
                                                                Double strength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                                                double calfbcal = Convert.ToDouble(attendstrength) * Convert.ToDouble(question_count) * Convert.ToDouble(sum_total);
                                                                double fbavg = (gtotal / calfbcal) * 100;
                                                                double avg = Convert.ToDouble(Math.Round(fbavg, 2));
                                                                string studentcount = "";
                                                                if (Convert.ToString(ds.Tables[0].Rows[i]["Strength"]).Trim() != "")
                                                                {
                                                                    studentcount = Convert.ToString(ds.Tables[0].Rows[i]["Strength"]);
                                                                }
                                                                else
                                                                {
                                                                    studentcount = "-";
                                                                }

                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].CellType = new FarPoint.Web.Spread.TextCellType();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = Convert.ToString(studentcount);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";

                                                                double averag = (Math.Round(Math.Round(gtotal, 2) / Math.Round((question_count * Convert.ToDouble(sum_total)), 2) * 100, 2));
                                                                //barath 31.07.17 *100 added
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(averag, 2));
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";

                                                            }
                                                        }
                                                        FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                                                        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                                                        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                                                        FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                                                        FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                                                        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                                                        string selqry = "";
                                                        DataSet dsnew = new DataSet();

                                                        string college_cd = string.Empty;
                                                        if (cbl_clgname.Items.Count > 0)
                                                        {
                                                            for (int i = 0; i < cbl_clgname.Items.Count; i++)
                                                            {
                                                                if (cbl_clgname.Items[i].Selected == true)
                                                                {
                                                                    if (college_cd == "")
                                                                    {
                                                                        college_cd = "" + cbl_clgname.Items[i].Value.ToString() + "";
                                                                    }
                                                                    else
                                                                    {
                                                                        college_cd = college_cd + "','" + Convert.ToString(cbl_clgname.Items[i].Value);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        string Batch_Year = "";
                                                        for (int i = 0; i < cbl_batch.Items.Count; i++)
                                                        {
                                                            if (cbl_batch.Items[i].Selected == true)
                                                            {
                                                                if (Batch_Year == "")
                                                                {
                                                                    Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                                                                }
                                                                else
                                                                {
                                                                    Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                                                                }
                                                            }
                                                        }
                                                        string degree_code = "";
                                                        for (int i = 0; i < cbl_deptname.Items.Count; i++)
                                                        {
                                                            if (cbl_deptname.Items[i].Selected == true)
                                                            {
                                                                if (degree_code == "")
                                                                {
                                                                    degree_code = "" + cbl_deptname.Items[i].Value.ToString() + "";
                                                                }
                                                                else
                                                                {
                                                                    degree_code = degree_code + "','" + cbl_deptname.Items[i].Value.ToString() + "";
                                                                }
                                                            }
                                                        }
                                                        string sections = "";
                                                        for (int i = 0; i < cbl_sec.Items.Count; i++)
                                                        {
                                                            if (cbl_sec.Items[i].Selected == true)
                                                            {
                                                                if (sections == "")
                                                                {
                                                                    sections = "" + cbl_sec.Items[i].Value.ToString() + "";
                                                                }
                                                                else
                                                                {
                                                                    sections = sections + "','" + cbl_sec.Items[i].Value.ToString() + "";
                                                                }
                                                                if (cbl_sec.Items[i].Value == "Empty")
                                                                {
                                                                    sections = "";
                                                                }
                                                            }
                                                        }
                                                        if (sections.Trim() != "")
                                                        {
                                                            sections = sections + "','";
                                                        }
                                                        string semester = "";
                                                        for (int i = 0; i < cbl_sem.Items.Count; i++)
                                                        {
                                                            if (cbl_sem.Items[i].Selected == true)
                                                            {
                                                                if (semester == "")
                                                                {
                                                                    semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                                                                }
                                                                else
                                                                {
                                                                    semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                                                                }
                                                            }
                                                        }




                                                        selqry = " select COUNT( distinct  s.FeedbackUnicode) as Strength,f.FeedBackMasterPK,Batch_Year ,f.semester,f.DegreeCode,f.Section from CO_FeedBackMaster F,CO_StudFeedBack S where s.FeedBackMasterFK =f.FeedBackMasterPK  and f.degreecode in ('" + degree_code + "') and f.Batch_Year in('" + Batch_Year + "') and f.semester in ('" + semester + "') and f.FeedBackName='" + Convert.ToString(feebackname) + "'  and f.InclueCommon='1' and s.FeedbackUnicode<>''";
                                                        if (sections != "")
                                                        {
                                                            selqry = selqry + " and f.Section in ('" + sections + "')  ";
                                                        }
                                                        selqry = selqry + " group by f.FeedBackMasterPK  ,Batch_Year ,f.semester,f.DegreeCode ,f.Section";
                                                        selqry = selqry + " SELECT Course_Name+'-'+Dept_Name Degree,Current_semester,R.degree_code,Sections,Batch_Year,COUNT(*)as TotStrengh FROM Registration R,Degree G,Course C,Department D WHERE R.degree_code = G.Degree_Code AND G.Course_Id = C.Course_Id AND G.college_code = C.college_code AND g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' and r.degree_code in ('" + degree_code + "') and r.Batch_Year in('" + Batch_Year + "') ";//and 
                                                        if (sections != "")
                                                        {
                                                            selqry = selqry + " and r.Sections in ('" + sections + "') GROUP BY Course_Name,R.degree_code, Dept_Name,Current_semester,Sections, Batch_Year ORDER BY R.degree_code,Current_Semester,Sections , Batch_Year,Course_Name,Dept_Name ";
                                                        }
                                                        else
                                                        {
                                                            selqry = selqry + " GROUP BY Course_Name, R.degree_code, Dept_Name,Current_semester,Sections,Batch_Year ORDER BY R.degree_code,Current_Semester,Sections , Batch_Year,Course_Name,Dept_Name";
                                                        }
                                                        selqry = selqry + "   SELECT Course_Name+'-'+Dept_Name Degree,semester,f.DegreeCode,Section,Batch_Year FROM Degree G,Course C,Department D,CO_FeedBackMaster F,CO_StudFeedBack S WHERE f.FeedBackMasterPK=s.FeedBackMasterFK and f.DegreeCode = G.Degree_Code AND G.Course_Id = C.Course_Id AND G.college_code = C.college_code AND g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND  f.degreecode in ('" + degree_code + "') and f.Batch_Year in('" + Batch_Year + "') and f.Semester in ('" + semester + "') and f.FeedBackName='" + Convert.ToString(feebackname) + "'  and f.InclueCommon='1' and s.FeedbackUnicode<>''";
                                                        if (sections != "")
                                                        {
                                                            selqry = selqry + " and f.Section in ('" + sections + "')   GROUP BY Course_Name,f.degreecode, Dept_Name,semester,Section, Batch_Year ORDER BY f.degreecode,Semester,Section , Batch_Year,Course_Name,Dept_Name ";
                                                        }
                                                        else
                                                        {
                                                            selqry = selqry + "   GROUP BY Course_Name,f.degreecode, Dept_Name,semester,Section, Batch_Year ORDER BY f.degreecode,Semester,Section , Batch_Year,Course_Name,Dept_Name ";
                                                        }


                                                        dsnew = d2.select_method_wo_parameter(selqry, "text");
                                                        DataView dvnews = new DataView();
                                                        DataView totalview = new DataView();
                                                        int overallstrength = 0;
                                                        int attended = 0;
                                                        if (dsnew.Tables.Count > 0)
                                                        {
                                                            if (dsnew.Tables[0].Rows.Count > 0 && dsnew.Tables[1].Rows.Count > 0 && dsnew.Tables[2].Rows.Count > 0)
                                                            {

                                                                for (int i = 0; i < dsnew.Tables[2].Rows.Count; i++)
                                                                {
                                                                    string sectons = Convert.ToString(dsnew.Tables[2].Rows[i]["Section"]);
                                                                    string degrecode = Convert.ToString(dsnew.Tables[2].Rows[i]["degreecode"]);
                                                                    string getsem = Convert.ToString(dsnew.Tables[2].Rows[i]["semester"]);
                                                                    string totalfind = " degree_code='" + degrecode + "'  and  Batch_Year='" + dsnew.Tables[2].Rows[i]["Batch_Year"].ToString() + "'";
                                                                    if (sectons.Trim() != "")
                                                                    {
                                                                        totalfind = totalfind + " and Sections='" + sectons + "'";
                                                                    }
                                                                    dsnew.Tables[1].DefaultView.RowFilter = "" + totalfind + "";
                                                                    totalview = dsnew.Tables[1].DefaultView;
                                                                    int total = 0;
                                                                    if (totalview.Count > 0)
                                                                    {

                                                                        string totas = totalview[0]["TotStrengh"].ToString();
                                                                        if (totas.Trim() == "")
                                                                        {
                                                                            totas = "0";
                                                                        }
                                                                        total = Convert.ToInt32(totas);
                                                                        overallstrength = overallstrength + total;
                                                                    }
                                                                    string filterquery = "";

                                                                    int attand = 0;
                                                                    filterquery = "degreecode='" + degrecode + "'  and  semester='" + getsem + "' ";
                                                                    if (sectons.Trim() != "")
                                                                    {
                                                                        filterquery = filterquery + " and Section='" + sectons + "'";
                                                                    }
                                                                    dsnew.Tables[0].DefaultView.RowFilter = "" + filterquery + "";
                                                                    dvnews = dsnew.Tables[0].DefaultView;
                                                                    if (dvnews.Count > 0)
                                                                    {

                                                                        attand = Convert.ToInt32(dvnews[0]["Strength"]);
                                                                    }
                                                                    attended = attended + attand;
                                                                }

                                                            }
                                                        }



                                                        int colcount = FpSpread1.Sheets[0].ColumnCount;
                                                        FpSpread1.Sheets[0].RowCount++;
                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, colcount);


                                                        FpSpread1.Sheets[0].RowCount++;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Students Strength:" + Convert.ToString(overallstrength);
                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, colcount);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].RowCount++;

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "No.Of Students Feedback Obtained:" + Convert.ToString(attended);
                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, colcount);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                                        FpSpread1.Sheets[0].RowCount++;
                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, colcount);


                                                        FpSpread1.Sheets[0].RowCount++;
                                                        FpSpread1.Columns[0].Locked = true;
                                                        FpSpread1.Columns[1].Locked = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "QUESTION USED FOR THE ASSESMENT PROCESS";
                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, colcount);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                        for (int j = 0; j < ds.Tables[1].Rows.Count; j++)//delsi2610
                                                        {
                                                            FpSpread1.Sheets[0].RowCount++;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[1].Rows[j]["questionacr"]) + ".";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[1].Rows[j]["Question"]) + "?";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, colcount);

                                                        }



                                                    }
                                                    else
                                                    {

                                                        FpSpread1.Sheets[0].RowCount--;

                                                        if (feedbak == "")
                                                        {
                                                            feedbak = Convert.ToString(cblfeedbackmulti.Items[fb].Value);
                                                        }
                                                        else
                                                        {
                                                            feedbak = feedbak + "," + Convert.ToString(cblfeedbackmulti.Items[fb].Value);
                                                        }

                                                        lbl_error.Visible = true;
                                                        lbl_error.Text = "No Records Found for" + "-" + feedbak;
                                                        SpreadDiv.Visible = false;


                                                    }
                                                }
                                                else
                                                {
                                                    lbl_error.Visible = true;
                                                    lbl_error.Text = "No Records Found";
                                                    SpreadDiv.Visible = false;
                                                }


                                            }
                                            else
                                            {
                                                SpreadDiv.Visible = false;
                                                lbl_error.Visible = true;
                                                lbl_error.Text = "Please select all fields";
                                            }
                                            checkheader = true;
                                        }
                                    }
                                    FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    FpSpread1.Height = 500;
                                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                    SpreadDiv.Visible = true;
                                }
                            }
                            #endregion
                        }
                    }


                }
                #endregion
            }
            if (rdbloginbased.Checked == true)
            {

                if (rdb_deptwise.Checked == true)
                {
                    FpSpread1.Sheets[0].AutoPostBack = true;

                    lbl_error.Visible = false;
                    Printcontrol1.Visible = false;
                    //string degreecode = rs.GetSelectedItemsValueAsString(cbl_deptname);
                    //string sem = rs.GetSelectedItemsValueAsString(cbl_sem);
                    //string batchyear = rs.GetSelectedItemsValueAsString(cbl_batch);
                    //string clgcode = rs.GetSelectedItemsValueAsString(cbl_clgname);
                    //string StaffAppID = rs.GetSelectedItemsValueAsString(cbl_staffname);
                    string degreecode = rs.GetSelectedItemsValue(cbl_deptname);
                    string sem = rs.GetSelectedItemsValue(cbl_sem);
                    string batchyear = rs.GetSelectedItemsValue(cbl_batch);
                    string clgcode = rs.GetSelectedItemsValue(cbl_clgname);
                    string StaffAppID = rs.GetSelectedItemsValue(cbl_staffname);
                    // string degree = rs.GetSelectedItemsValue(cbl_degree);
                    string sec = string.Empty;
                    for (int i = 0; i < cbl_sec.Items.Count; i++)
                    {
                        if (cbl_sec.Items[i].Selected == true)
                        {
                            if (string.IsNullOrEmpty(sec))
                                sec = cbl_sec.Items[i].Value.ToString();
                            else
                                sec = sec + "," + cbl_sec.Items[i].Value.ToString() + "";
                        }
                    }
                    if (!string.IsNullOrEmpty(degreecode) && !string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(batchyear))
                    {
                        if (ddl_feedback.SelectedItem.Text.Trim() != "--Select--")
                        {
                            string type = "1";
                            string fbpk = " select FeedBackMasterPK,ISNULL(issubjecttype,0)issubjecttype from CO_FeedBackMaster where FeedBackName ='" + ddl_feedback.SelectedItem.Value + "'";// and DegreeCode in ('" + degreecode + "') and semester in ('" + sem + "') and Batch_Year in('" + batchyear + "') and section in ('" + sec + "')";
                            DataSet dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                            string feedbakpk = string.Empty;
                            string issubjecttype = string.Empty;
                            if (dsfb.Tables.Count > 0)
                            {
                                if (dsfb.Tables[0].Rows.Count > 0)
                                {
                                    issubjecttype = Convert.ToString(dsfb.Tables[0].Rows[0]["issubjecttype"]);
                                    for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                                    {
                                        if (string.IsNullOrEmpty(feedbakpk))
                                            feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                        else
                                            feedbakpk = feedbakpk + "," + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                                    }
                                }
                            }
                            rs.Fpreadheaderbindmethod("S No-50/Department-200/StaffCode-100/StaffName-150/Subject Code-150/Subject Name-250/SubjectType-100", FpSpread1, "true");

                            string selqry = " select count( distinct S.FeedbackUnicode)Strength,SUM(M.Point)Points,(convert(varchar(10), f.Batch_Year)+'-'+co.Course_Name+'-'+ dt.dept_acronym+'-'+convert(varchar(10), f.Semester)+'-'+f.Section ) as department,c.subject_code, s.StaffApplNo,sm.staff_code +' - '+staff_name as staff ,f.FeedBackMasterPK,Batch_Year , f.semester,f.DegreeCode,f.Section,SubjectNo,c.subject_name,sm.staff_code,staff_name,c.acronym from CO_FeedBackMaster F,CO_StudFeedBack S,staff_appl_master sa,staffmaster sm ,subject c,Department dt,course co,Degree d  ,CO_MarkMaster M where M.MarkMasterPK =S.MarkMasterPK and  d.Degree_Code =f.degreecode and dt.Dept_Code =d.Dept_Code and co.Course_Id =d.Course_Id and c.subject_no=s.SubjectNo and sa.appl_no=sm.appl_no  and sa.appl_id=s.StaffApplNo and s.FeedBackMasterFK =f.FeedBackMasterPK  and f.Batch_Year in('" + batchyear + "') and f.semester in ('" + sem + "')  and isnull(f.Section,'') in ('" + sec + "') and f.InclueCommon='1' and s.FeedbackUnicode<>'' and f.FeedBackMasterPK in ('" + feedbakpk + "') and s.StaffApplNo in('" + StaffAppID + "') group by staff_code,staff_name, f.FeedBackMasterPK, StaffApplNo,Batch_Year, f.semester,f.DegreeCode ,f.Section,subject_name, SubjectNo,subject_code,Course_Name,dept_acronym,c.acronym order by sm.staff_name ";// and   f.degreecode in('" + degreecode + "')
                            selqry += " SELECT distinct Question,QuestionMasterPK,HeaderCode FROM CO_FeedBackMaster B,CO_QuestionMaster Q ,CO_FeedBackQuestions FB WHERE  b.FeedBackMasterPK =fb.FeedBackMasterFK and q.QuestionMasterPK =fb.QuestionMasterFK and  InclueCommon='1' and FeedBackType = '" + type + "' and B.FeedBackName='" + Convert.ToString(ddl_feedback.SelectedItem.Text) + "'  and B.CollegeCode in ('" + clgcode + "') order by HeaderCode";
                            //  and   b.degreecode in ('" + degreecode + "') //26.12.17 barath added
                            selqry += " SELECT StaffApplNo,sum(M.Point) as points,QuestionMasterfK,SubjectNo,isnull(b.Section,'')Section FROM CO_StudFeedBack F,CO_FeedBackMaster B,CO_MarkMaster M where F.FeedBackMasterFK = B.FeedBackMasterPK AND F.MarkMasterPK = M.MarkMasterPK AND  b.InclueCommon='1' and FeedBackType = '" + type + "' and B.FeedBackMasterpK in ('" + feedbakpk + "') and B.CollegeCode in ('" + clgcode + "') and b.Batch_Year in('" + batchyear + "') and b.semester in ('" + sem + "')  and isnull(b.Section,'') in ('" + sec + "') group by StaffApplNo,QuestionMasterfK,SubjectNo,isnull(b.Section,'')";
                            //selqry += " SELECT StaffApplNo,sum(M.Point) as points,QuestionMasterfK,SubjectNo FROM CO_StudFeedBack F,CO_FeedBackMaster B,CO_MarkMaster M,CO_FeedbackUniCode fu WHERE fu.FeedbackUnicode=f.FeedbackUnicode and fu.FeedbackMasterFK=f.FeedBackMasterFK and F.FeedBackMasterFK = B.FeedBackMasterPK AND F.MarkMasterPK = M.MarkMasterPK AND  b.InclueCommon='1' and FeedBackType = '" + type + "' and B.FeedBackName ='" + Convert.ToString(ddl_feedback.SelectedItem.Text) + "' and B.CollegeCode in ('" + clgcode + "') and b.Batch_Year in('" + batchyear + "') and b.semester in ('" + sem + "')  and isnull(b.Section,'') in ('" + sec + "') group by StaffApplNo,QuestionMasterfK,SubjectNo,isnull(b.Section,'')";//,isnull(b.Section,'') 26.12.17
                            selqry += " select count(App_No)studentcount,degree_code,sections,college_code from Registration where  college_code in('" + clgcode + "') and isnull(Sections,'') in('" + sec + "') and cc=0 and delflag=0 and exam_flag<>'Debar' group by degree_code,college_code,sections ";//degree_code in('" + degreecode + "') and
                            selqry += " select COUNT( distinct QuestionMasterFK)question_count,isnull(SubjectType,'')SubjectType from CO_FeedBackQuestions where FeedBackMasterFK in ('" + feedbakpk + "') group by isnull(SubjectType,'')";
                            // ds = d2.select_method_wo_parameter(selqry, "Text");

                            Hashtable hat = new Hashtable();
                            hat.Add("@CollegeCode", clgcode);
                            hat.Add("@batchyear", batchyear);
                            hat.Add("@Degreecode", degreecode);
                            hat.Add("@semester", sem);
                            hat.Add("@section", sec);
                            hat.Add("@FeedbackName", Convert.ToString(ddl_feedback.SelectedItem.Text));
                            hat.Add("@FeedbackMasterFK", feedbakpk);
                            hat.Add("@StaffAppNo", StaffAppID);
                            hat.Add("@FeedbackType", type);
                            ds = d2.select_method("AnonymousDepartmentwiseReportlogins", hat, "sp");
                            //string question_count = d2.GetFunction("select COUNT( distinct QuestionMasterFK)question_count from CO_FeedBackQuestions where FeedBackMasterFK in ('" + feedbakpk + "')");
                            double question_count = 0;
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[4].Rows.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(ds.Tables[4].Compute("sum(question_count)", "")), out question_count);
                                }
                                string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                    {
                                        FpSpread1.Sheets[0].ColumnCount++;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[1].Rows[i]["QuestionMasterPK"]);
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 300;
                                    }
                                    FpSpread1.Sheets[0].ColumnCount++;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Student Total";
                                    FpSpread1.Sheets[0].ColumnCount++;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Maximum Total";
                                    FpSpread1.Sheets[0].ColumnCount++;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Percentage";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Bold = true;
                                }
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    int k = 0; string staffname = ""; int s = 1;
                                    FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                                    cb.AutoPostBack = true;
                                    FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                                    cb1.AutoPostBack = false;
                                    //FpSpread1.Sheets[0].RowCount++;
                                    double staffavg = 0; bool staffinvdiavg = false; double sumavgpoint = 0; int staffrowcount = 0;
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        staffinvdiavg = false;
                                        FpSpread1.Sheets[0].RowCount++;
                                        if (staffname.Trim() == "")
                                        { k++; }
                                        else if (staffname == ds.Tables[0].Rows[i]["staff_name"].ToString())
                                        { k++; staffrowcount++; }
                                        else
                                        {
                                            k = 1; s++;
                                            FpSpread1.Sheets[0].RowCount++;
                                            //staffavg = (staffavg / Convert.ToDouble(staffrowcount+1));
                                            staffavg = ((staffavg / (Convert.ToDouble(staffrowcount + 1) * 100)) * 100);
                                            double.TryParse(Convert.ToString(Math.Round(staffavg, 2)), out sumavgpoint);
                                            staffinvdiavg = true;
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(s);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["FeedBackMasterPK"]);


                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["department"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]);
                                        staffname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                                        string staff_codeName = ds.Tables[0].Rows[i]["staff"].ToString();
                                        string staff_Code = string.Empty;
                                        string staff_Name = string.Empty;

                                        if (staff_codeName.Contains("-"))
                                        {
                                            string[] splitval = staff_codeName.Split('-');
                                            staff_Code = Convert.ToString(splitval[0]);
                                            staff_Name = Convert.ToString(splitval[1]);

                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = staff_Code;//k.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = staff_Name;//k.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["subject_code"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["Subject_Name"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["acronym"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Bold = true;


                                        double gtotal = 0; double mtotal = 0; double avgper = 0;
                                        string filterquery = string.Empty;
                                        string section = Convert.ToString(ds.Tables[0].Rows[i]["Section"]);
                                        filterquery = "degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["DegreeCode"]) + "' ";
                                        if (section.Trim() != "")
                                        {
                                            filterquery = filterquery + " and Sections='" + section + "'";
                                        }
                                        ds.Tables[3].DefaultView.RowFilter = "" + filterquery + "";
                                        DataView dvnew = ds.Tables[3].DefaultView;
                                        string totalstudnent = "";
                                        if (dvnew.Count > 0)
                                        {
                                            totalstudnent = Convert.ToString(dvnew[0]["studentcount"]);
                                        }
                                        if (totalstudnent.Trim() == "")
                                            totalstudnent = "0";
                                        //double maximun = Convert.ToDouble(question_count) * Convert.ToDouble(sum_total) * Convert.ToDouble(totalstudnent);
                                        Double attendstrength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                        double maximun = Convert.ToDouble(sum_total) * Convert.ToDouble(attendstrength);
                                        double QuestionAttendcount = 0;
                                        for (int j = 8; j <= FpSpread1.Columns.Count - 3; j++)
                                        {
                                            string questionmasterPK = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, j - 1].Tag);
                                            ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "' and Section='" + Convert.ToString(ds.Tables[0].Rows[i]["Section"]) + "'";//and FeedbackUnicode='" + ds.Tables[0].Rows[i]["FeedbackUnicode"] + "'
                                            DataView dv = ds.Tables[2].DefaultView;
                                            if (dv.Count > 0)
                                            {
                                                QuestionAttendcount++;
                                                string point1 = Convert.ToString(dv[0]["points"]);
                                                if (string.IsNullOrEmpty(point1.Trim()) || point1.Trim() == "-")
                                                    point1 = "0";
                                                double questavgpoint = Convert.ToDouble(point1) / maximun * Convert.ToDouble(sum_total);// 100;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(Math.Round(questavgpoint, 2)); //Convert.ToString(dv[0]["points"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                gtotal += Convert.ToDouble((Math.Round(questavgpoint, 2)));
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = "-";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                            }
                                            FpSpread1.Columns[j - 1].Locked = true;
                                            FpSpread1.Columns[4].Locked = true;
                                        }
                                        //Double strength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                        //double calfbcal = Convert.ToDouble(totalstudnent) * Convert.ToDouble(question_count) * Convert.ToDouble(sum_total);
                                        //double fbavg = (gtotal / calfbcal) * 100;
                                        //double avg = Convert.ToDouble(Math.Round(fbavg, 2));
                                        //string studentcount = "";
                                        //if (Convert.ToString(ds.Tables[0].Rows[i]["Strength"]).Trim() != "")
                                        //{
                                        //    studentcount = Convert.ToString(ds.Tables[0].Rows[i]["Strength"]);
                                        //}
                                        //else
                                        //{
                                        //    studentcount = "-";
                                        //}
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].CellType = new FarPoint.Web.Spread.TextCellType();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Text = Convert.ToString(String.Format("{0:0.00}", Math.Round(gtotal, 2)));
                                        if (issubjecttype == "1" || issubjecttype.ToUpper() == "TRUE")
                                        {
                                            question_count = QuestionAttendcount;
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = Convert.ToString(Math.Round((question_count * Convert.ToDouble(sum_total)), 2));
                                        double avg = (Math.Round(Math.Round(gtotal, 2) / Math.Round((question_count * Convert.ToDouble(sum_total)), 2) * 100, 2));
                                        //barath 31.07.17 *100 added
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(avg, 2));
                                        staffavg += avg;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(avg);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Bold = true;
                                        if (staffinvdiavg == true)
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(sumavgpoint, 2));

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].ForeColor = System.Drawing.Color.BlueViolet;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                            sumavgpoint = 0;
                                            staffrowcount = 0; staffavg = 0; staffavg += avg;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Text = "Average";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].ForeColor = System.Drawing.Color.BlueViolet;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                    FpSpread1.Sheets[0].RowCount++;
                                    staffavg = ((staffavg / (Convert.ToDouble(staffrowcount + 1) * 100)) * 100);
                                    double.TryParse(Convert.ToString(Math.Round(staffavg, 2)), out sumavgpoint);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = Convert.ToString(String.Format("{0:0.00}", sumavgpoint));
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].ForeColor = System.Drawing.Color.BlueViolet;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = "Average";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].ForeColor = System.Drawing.Color.BlueViolet;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Columns[FpSpread1.Columns.Count - 1].Locked = true;
                                    FpSpread1.Columns[FpSpread1.Columns.Count - 2].Locked = true;
                                    FpSpread1.Columns[FpSpread1.Columns.Count - 3].Locked = true;
                                    FpSpread1.Columns[0].Locked = true;
                                    FpSpread1.Columns[1].Locked = true;
                                    FpSpread1.Columns[2].Locked = true;
                                    FpSpread1.Columns[3].Locked = true;
                                    FpSpread1.Columns[4].Locked = true;
                                    FpSpread1.Columns[5].Locked = true;
                                    FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    FpSpread1.Height = 500;
                                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                    SpreadDiv.Visible = true;
                                }
                                else
                                {
                                    lbl_error.Visible = true;
                                    lbl_error.Text = "No Records Found";
                                    SpreadDiv.Visible = false;
                                }
                            }
                            else
                            {
                                lbl_error.Visible = true;
                                lbl_error.Text = "No Records Found";
                                SpreadDiv.Visible = false;
                            }
                        }
                        else
                        {
                            SpreadDiv.Visible = false;
                            lbl_error.Visible = true;
                            lbl_error.Text = "Please Select Feedback Name";
                        }
                    }
                    else
                    {
                        SpreadDiv.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "Please select all fields";
                    }


                }


            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        lbl_norec1.Visible = false;
        try
        {
            string reportname = txtexcelname1.Text;
            if (reportname.ToString().Trim() != "")
            {
                if (FpSpread1.Visible == true)
                    d2.printexcelreport(FpSpread1, reportname);
                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch
        {
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            int count1 = 0;
            int batchcount = 0;
            int semcount = 0;
            int staffcount = 0;
            string degree = string.Empty;
            string sec = "";
            int SecCnt = 0;
            int CourseCnt = 0;
            string course = "";
            string sub = string.Empty;
            string batch = string.Empty;
            string semester = string.Empty;
            string singlestaff = string.Empty;
            string SecName = "";
            string dptname = (txtexcelname1.Text == "" ? "Feedback Report" : txtexcelname1.Text);
            string pagename = "AnonymousDepartmentwiseReport.aspx";
            for (int i = 0; i < cbl_deptname.Items.Count; i++)
            {
                if (cbl_deptname.Items[i].Selected == true)
                {
                    count++;
                    degree = cbl_deptname.Items[i].Text;
                }
            }
            for (int i = 0; i < cbl_staffname.Items.Count; i++)
            {
                if (cbl_staffname.Items[i].Selected == true)
                {
                    staffcount++;
                    singlestaff = "Staff Name: " + cbl_staffname.Items[i].Text + "";
                }
            }
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    batchcount++;
                    batch = cbl_batch.Items[i].Text;
                }
            }
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    semcount++;
                    semester = cbl_sem.Items[i].Text;
                }
            }
            //Added by saranya  
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    SecCnt++;
                    sec = cbl_sec.Items[i].Text;
                }
            }
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    CourseCnt++;
                    course = cbl_degree.Items[i].Text;
                }
            }
            if (count == 1 && staffcount == 1)
            {
                dptname = dptname + "@ Course     : " + degree + "      " + singlestaff;
            }
            else if (count == 1)
            {
                dptname = dptname + "@ Course     : " + degree;
            }
            if (count1 == 1 && semcount == 1)
            {
                dptname = dptname + '@' + " Subject  : " + sub + "             Semester : " + semester;
            }
            else if (batchcount == 1 && semcount == 1)
            {
                dptname = dptname + '@' + " Batch  : " + batch + "             Semester : " + semester;
            }
            else if (batchcount == 1)
            {
                dptname = dptname + '@' + " Batch       : " + batch;
            }
            else if (count1 == 1)
            {
                dptname = dptname + '@' + " Subject  : " + sub;
            }
            else if (semcount == 1)
            {
                dptname = dptname + '@' + " Semester : " + semester;
            }
            if (SecCnt == 1)
            {
                SecName = '@' + " Section    : " + sec;
            }

            if (rdb_classwise.Checked == true)
            {
                dptname = "Class Wise FeedBack" + "@ Course     : " + course + " - " + degree + SecName;

                if (Rdbquesacr.Checked == true)
                {
                    dptname = "TEACHING STAFF FEEDBACK" + "@ Course     : " + course + " - " + degree + SecName;
                }
            }
            if (FpSpread1.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpSpread1, pagename, dptname);
            }
            Printcontrol1.Visible = true;
            lbl_norec1.Visible = false;
        }
        catch
        {
        }
    }

    #endregion

    public void rdb_classwise_Click(object sender, EventArgs e)
    {
        Rdbques.Visible = true;
        Rdbquesacr.Visible = true;
        Rdbques.Checked = true;
        Rdbquesacr.Checked = false;
        cb_WithOutRoundOff.Visible = true;
        lbl_Degree.Visible = true;
        Updp_Degree.Visible = true;
        rdb_classwise.Checked = true;
        rdb_deptwise.Checked = false;
        lbl_staffName.Visible = true;
        //Panel3.Visible = false;
        //txtstaffname.Visible = false;
        txtstaffname.Enabled = false;
        UpdatePanel5.Visible = true;
        Txt_Subject.Visible = true;
        lbl_subject.Visible = true;
        Panel_Subject.Visible = true;
        cbmul.Visible = true;
        chkwithcomments.Visible = false;
        if (cbmul.Checked == false)
        {
            ddl_feedback.SelectedIndex = 0;
        }
        if (cbmul.Checked == true)
        {
            txtfeedbackmulti.Visible = false;
            Panel5.Visible = false;
            cbmul.Visible = false;
        }
        BindDegree();
        bindfeedback();
        //cbIndividual.Visible = false;
    }

    public void rdb_deptwise_Click(object sender, EventArgs e)
    {
        chkwithcomments.Visible = true;
        Rdbques.Visible = false;
        Rdbquesacr.Visible = false;
        cb_WithOutRoundOff.Visible = false;
        lbl_Degree.Visible = false;
        Updp_Degree.Visible = false;
        rdb_deptwise.Checked = true;
        rdb_classwise.Checked = false;
        binddept();
        bindstaff();
        lbl_staffName.Visible = true;
        Panel3.Visible = true;
        txtstaffname.Visible = true;
        txtstaffname.Enabled = true;
        lbl_subject.Visible = false;
        Panel_Subject.Visible = false;
        Txt_Subject.Visible = false;
        UpdatePanel5.Visible = false;
        cbmul.Visible = true;
        txtfeedbackmulti.Visible = false;
        Panel5.Visible = false;
        cbmul.Visible = false;
        cbmul.Checked = false;
        ddl_feedback.Visible = true;//delsisref
        bindfeedback();
        //cbIndividual.Visible = true;

    }

    public void BindDegree()//delsi1903
    {
        try
        {
            cbl_degree.Items.Clear();
            string college_cd = rs.GetSelectedItemsValueAsString(cbl_clgname);
            if (college_cd.Trim() != "")
            {
                string query = string.Empty;
                ds.Clear();
                query = "select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in ('" + college_cd + "')";
                ds = d2.select_method_wo_parameter(query, "Text");

                int count1 = ds.Tables[0].Rows.Count;
                if (count1 > 0)
                {
                    cbl_degree.DataSource = ds;
                    cbl_degree.DataTextField = "course_name";
                    cbl_degree.DataValueField = "course_id";
                    cbl_degree.DataBind();
                    if (cbl_degree.Items.Count > 0)
                    {
                        cbl_degree.Items[0].Selected = true;
                        txt_degree.Text = "Degree(1)";
                    }
                }
                binddept();
            }
            else
            {
                cb_degree.Checked = false;
                txt_degree.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, collegecode1, "Feedback_anonymousisgender");
        }
    }

    public void cb_degree_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_degree, cb_degree, txt_degree, "Degree");
        binddept();
        //bindbranch();
        //bindsem();
        //bindsec(); bindfeedback();
    }

    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_degree, cb_degree, txt_degree, "Degree");
        binddept();
        //bindbranch();
        //bindsem();
        //bindsec(); bindfeedback();
    }

    public void Cb_Subject_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(Cbl_Subject, Cb_Subject, Txt_Subject, "Subject");
    }

    public void Cbl_Subject_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(Cbl_Subject, Cb_Subject, Txt_Subject, "Subject");
    }

    protected void bindsubject()//delsi2103
    {
        if (ddl_feedback.Items.Count > 0)
        {
            if (ddl_feedback.SelectedItem.Text != "Select")
            {
                Txt_Subject.Text = "--Select--";
                string college_cd = rs.GetSelectedItemsValueAsString(cbl_clgname);
                string Batch_Year = rs.GetSelectedItemsValueAsString(cbl_batch);
                string degree_code = rs.GetSelectedItemsValueAsString(cbl_deptname);
                string semester = rs.GetSelectedItemsValueAsString(cbl_sem);
                string section = rs.GetSelectedItemsValueAsString(cbl_sec);
                if (section.Trim() != "")
                {
                    section = section + "','";
                }
                if (degree_code.Trim() != "" && semester.Trim() != "" && Batch_Year.Trim() != "")
                {
                    string q1 = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_feedback.SelectedItem.Value + "' and DegreeCode in ('" + degree_code + "') and semester in ('" + semester + "') and Batch_Year in('" + Batch_Year + "')";
                    if (section.Trim() != "")
                    {
                        q1 += " and section in ('" + section + "')";
                    }
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string feedbakpk = GetdatasetRowstring(ds, "FeedBackMasterPK");
                        string query = "select distinct s.subject_name,s.subject_no from subject s,CO_StudFeedBack sf where s.subject_no=sf.SubjectNo and sf.FeedBackMasterFK in('" + feedbakpk + "')";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(query, "text");
                        Cbl_Subject.Items.Clear();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Cbl_Subject.DataSource = ds;
                            Cbl_Subject.DataTextField = "subject_name";
                            Cbl_Subject.DataValueField = "subject_no";
                            Cbl_Subject.DataBind();
                        }
                        if (Cbl_Subject.Items.Count > 0)
                        {
                            for (int row = 0; row < Cbl_Subject.Items.Count; row++)
                            {
                                Cbl_Subject.Items[row].Selected = true;
                                Cb_Subject.Checked = true;
                            }
                            Txt_Subject.Text = "Subject(" + Cbl_Subject.Items.Count + ")";
                        }
                    }
                }
            }
        }
    }

    public string GetdatasetRowstring(DataSet dummy, string collname)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            foreach (DataRow dr in dummy.Tables[0].Rows)
            {
                if (sbSelected.Length == 0)
                {
                    sbSelected.Append(Convert.ToString(dr[collname]));
                }
                else
                {
                    sbSelected.Append("','" + Convert.ToString(dr[collname]));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    public void rdb_Rdbques_Click(object sender, EventArgs e)
    {
        if (Rdbques.Checked == true)
        {
            Rdbquesacr.Checked = false;
        }

    }
    public void rdb_Rdbquesacr_Click(object sender, EventArgs e)
    {
        if (Rdbquesacr.Checked == true)
        {
            Rdbques.Checked = false;
        }

    }
    public void cbfeedbackmulti_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txtfeedbackmulti.Text = "--Select--";
            if (cbfeedbackmulti.Checked == true)
            {
                cout++;
                for (int i = 0; i < cblfeedbackmulti.Items.Count; i++)
                {
                    cblfeedbackmulti.Items[i].Selected = true;
                }
                txtfeedbackmulti.Text = "FeedBack(" + (cblfeedbackmulti.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblfeedbackmulti.Items.Count; i++)
                {
                    cblfeedbackmulti.Items[i].Selected = false;
                }
                txtfeedbackmulti.Text = "--Select--";
            }
            bindsubject1();
        }
        catch (Exception ex)
        {
        }

    }

    public void cblfeedbackmulti_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txtfeedbackmulti.Text = "--Select--";
            cbfeedbackmulti.Checked = false;
            for (int i = 0; i < cblfeedbackmulti.Items.Count; i++)
            {
                if (cblfeedbackmulti.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cbfeedbackmulti.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblfeedbackmulti.Items.Count)
                {
                    cbfeedbackmulti.Checked = true;
                }
                txtfeedbackmulti.Text = "FeedBack(" + commcount.ToString() + ")";
            }
            bindsubject1();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbmul_checkedchange(object sender, EventArgs e)
    {
        bindfeedback();
        if (cbmul.Checked == true)
        {
            bindsubject1();
            ddl_feedback.Visible = false;
            txtfeedbackmulti.Visible = true;
            Panel5.Visible = true;
        }
        if (cbmul.Checked == false)
        {
            ddl_feedback.Visible = true;
            txtfeedbackmulti.Visible = false;
            Panel5.Visible = false;
        }

    }

    protected void bindsubject1()//delsi2103
    {
        string feedback = string.Empty;
        if (cblfeedbackmulti.Items.Count > 0)//delsi2910
        {

            for (int fb = 0; fb < cblfeedbackmulti.Items.Count; fb++)
            {
                if (cblfeedbackmulti.Items[fb].Selected == true)
                {
                    if (feedback == "")
                    {
                        feedback = Convert.ToString(cblfeedbackmulti.Items[fb].Value);
                    }
                    else
                    {
                        feedback = feedback + "','" + Convert.ToString(cblfeedbackmulti.Items[fb].Value);
                    }

                }

            }
        }
        if (feedback != "")
        {
            Txt_Subject.Text = "--Select--";
            string college_cd = rs.GetSelectedItemsValueAsString(cbl_clgname);
            string Batch_Year = rs.GetSelectedItemsValueAsString(cbl_batch);
            string degree_code = rs.GetSelectedItemsValueAsString(cbl_deptname);
            string semester = rs.GetSelectedItemsValueAsString(cbl_sem);
            string section = rs.GetSelectedItemsValueAsString(cbl_sec);
            if (section.Trim() != "")
            {
                section = section + "','";
            }

            if (degree_code.Trim() != "" && semester.Trim() != "" && Batch_Year.Trim() != "")
            {
                string q1 = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName in('" + feedback + "') and DegreeCode in ('" + degree_code + "') and semester in ('" + semester + "') and Batch_Year in('" + Batch_Year + "')";
                if (section.Trim() != "")
                {
                    q1 += " and section in ('" + section + "')";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string feedbakpk = GetdatasetRowstring(ds, "FeedBackMasterPK");
                    string query = "select distinct s.subject_name,s.subject_no from subject s,CO_StudFeedBack sf where s.subject_no=sf.SubjectNo and sf.FeedBackMasterFK in('" + feedbakpk + "')";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(query, "text");
                    Cbl_Subject.Items.Clear();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Cbl_Subject.DataSource = ds;
                        Cbl_Subject.DataTextField = "subject_name";
                        Cbl_Subject.DataValueField = "subject_no";
                        Cbl_Subject.DataBind();
                    }
                    if (Cbl_Subject.Items.Count > 0)
                    {
                        for (int row = 0; row < Cbl_Subject.Items.Count; row++)
                        {
                            Cbl_Subject.Items[row].Selected = true;
                            Cb_Subject.Checked = true;
                        }
                        Txt_Subject.Text = "Subject(" + Cbl_Subject.Items.Count + ")";
                    }
                }
            }
        }
        else
        {
            if (Cbl_Subject.Items.Count > 0)
            {
                for (int row = 0; row < Cbl_Subject.Items.Count; row++)
                {
                    Cbl_Subject.Items[row].Selected = false;
                    Cb_Subject.Checked = false;
                }
                Txt_Subject.Text = "--Select--";
            }
        }


    }
    //protected void cb_individual_checkedchange(object sender, EventArgs e)
    //{
    //    if (cbIndividual.Checked == true)
    //    {
    //        lblstaff_subject.Visible = true;
    //        UpdatePanel2.Visible = true;
    //        if (ddl_feedback.SelectedItem.Text.Trim() != "--Select--")
    //        {
    //            bindstaffsubject();
    //        }
    //    }
    //    else if (cbIndividual.Checked == false)
    //    {
    //        lblstaff_subject.Visible = false;
    //        UpdatePanel2.Visible = false;
    //    }

    //}
    //protected void cbstaffsub_CheckedChanged(object sender, EventArgs e)
    //{
    //    rs.CallCheckBoxChangedEvent(cblsubstaff, cbstaffsub, Txt_Subject, "Subject");
    //}
    //protected void cblsubstaff_selectedindexchanged(object sender, EventArgs e)
    //{
    //    rs.CallCheckBoxListChangedEvent(cblsubstaff, cbstaffsub, Txt_Subject, "Subject");
    //}
    //protected void bindstaffsubject()//delsi2103
    //{
    //    if (ddl_feedback.Items.Count > 0)
    //    {
    //        if (ddl_feedback.SelectedItem.Text != "Select")
    //        {
    //            Txt_Subject.Text = "--Select--";
    //            string college_cd = rs.GetSelectedItemsValueAsString(cbl_clgname);
    //            string Batch_Year = rs.GetSelectedItemsValueAsString(cbl_batch);
    //            string degree_code = rs.GetSelectedItemsValueAsString(cbl_deptname);
    //            string semester = rs.GetSelectedItemsValueAsString(cbl_sem);
    //            string section = rs.GetSelectedItemsValueAsString(cbl_sec);
    //            string applid = rs.GetSelectedItemsValueAsString(cbl_staffname);
    //            if (section.Trim() != "")
    //            {
    //                section = section + "','";
    //            }
    //            if (degree_code.Trim() != "" && semester.Trim() != "" && Batch_Year.Trim() != "" && applid.Trim() != "")
    //            {
    //                string q1 = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_feedback.SelectedItem.Value + "'  and Batch_Year in('" + Batch_Year + "')";

    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(q1, "text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    string feedbakpk = GetdatasetRowstring(ds, "FeedBackMasterPK");
    //                    string query = "select distinct s.subject_name,s.subject_no from subject s,CO_StudFeedBack sf where s.subject_no=sf.SubjectNo and sf.FeedBackMasterFK in('" + feedbakpk + "') and StaffApplNo in('" + applid + "') ";
    //                    ds.Clear();
    //                    ds = d2.select_method_wo_parameter(query, "text");
    //                    cblsubstaff.Items.Clear();
    //                    if (ds.Tables[0].Rows.Count > 0)
    //                    {
    //                        cblsubstaff.DataSource = ds;
    //                        cblsubstaff.DataTextField = "subject_name";
    //                        cblsubstaff.DataValueField = "subject_no";
    //                        cblsubstaff.DataBind();
    //                    }
    //                    if (cblsubstaff.Items.Count > 0)
    //                    {
    //                        for (int row = 0; row < cblsubstaff.Items.Count; row++)
    //                        {
    //                            cblsubstaff.Items[row].Selected = true;
    //                            cbstaffsub.Checked = true;
    //                        }
    //                        txtsubstaff.Text = "Subject(" + cblsubstaff.Items.Count + ")";
    //                    }
    //                    else
    //                    {
    //                        txtsubstaff.Text = "--Select--";
    //                    }
    //                }
    //            }
    //        }
    //    }

    //}

    protected void FpSpread1_CellClick(Object sender, EventArgs e)
    {
        cellflag = true;
    }
    protected void FpSpread1_PreRender(Object sender, EventArgs e)
    {
        try
        {

            if (cellflag == true)//delsis
            {
                if (rdbanonyomous.Checked == true)
                {
                    if (rdb_deptwise.Checked == true)
                    {

                        contentDiv.InnerHtml = ""; StringBuilder html = new StringBuilder();

                        string sql1 = string.Empty;
                        DataSet feedbackds = new DataSet();
                        feedbackds.Clear();
                        string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                        string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                        string collegename = string.Empty;
                        string address1 = string.Empty;
                        string address2 = string.Empty;
                        string address3 = string.Empty;
                        string pincode = string.Empty;
                        sql1 = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                        feedbackds = d2.select_method_wo_parameter(sql1, "Text");
                        if (feedbackds.Tables[0].Rows.Count > 0)
                        {
                            collegename = Convert.ToString(feedbackds.Tables[0].Rows[0]["collname"]);
                            address1 = Convert.ToString(feedbackds.Tables[0].Rows[0]["address1"]);
                            address2 = Convert.ToString(feedbackds.Tables[0].Rows[0]["address2"]);
                            address3 = Convert.ToString(feedbackds.Tables[0].Rows[0]["address3"]);
                            pincode = Convert.ToString(feedbackds.Tables[0].Rows[0]["pincode"]);

                        }
                        string feedbackmasterpk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                        string subjectnum = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                        string department = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                        string staffcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                        string appno = d2.GetFunction("select appl_no from staffmaster where staff_code='" + staffcode + "'");
                        string applid = d2.GetFunction("select appl_id from staff_appl_master where appl_no='" + appno + "'");
                        string dept_staffacr = d2.GetFunction("select dept_acronym from stafftrans st,hrdept_master hr where staff_code='" + staffcode + "' and hr.dept_code=st.dept_code");
                        string staffname = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                        string coursecode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                        string couser_name = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                        string percentage = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                        string batchyear = string.Empty;
                        string branch = string.Empty;
                        string deptacr = string.Empty;
                        string section = string.Empty;
                        string semester = string.Empty;
                        if (department.Contains('-'))
                        {
                            string[] splitval = department.Split('-');
                            batchyear = Convert.ToString(splitval[0]);
                            branch = Convert.ToString(splitval[1]);
                            deptacr = Convert.ToString(splitval[2]);
                            semester = Convert.ToString(splitval[3]);
                            section = Convert.ToString(splitval[4]);

                        }
                        //string selstfimg = d2.GetFunction("select photo from StaffPhoto where (staff_code='" + scode + "' or appl_id='" + appl_id + "')");
                        //if (selstfimg.Trim() != "0" && selstfimg.Trim() != "")
                        //{
                        //    if (!String.IsNullOrEmpty(staffcode.Trim()))
                        //        stf_img.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + staffcode;
                        //    else
                        //        stf_img.ImageUrl = "~/Handler/staffphoto.ashx?appl_id=" + staffcode;
                        //}
                        //else
                        //{
                        //    stf_img.ImageUrl = "";
                        //}

                        string fbpk = " select FeedBackMasterPK,ISNULL(issubjecttype,0)issubjecttype from CO_FeedBackMaster where FeedBackName ='" + ddl_feedback.SelectedItem.Value + "'";
                        DataSet dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                        string feedbakpk = string.Empty;
                        string issubjecttype = string.Empty;
                        if (dsfb.Tables.Count > 0)
                        {
                            if (dsfb.Tables[0].Rows.Count > 0)
                            {
                                issubjecttype = Convert.ToString(dsfb.Tables[0].Rows[0]["issubjecttype"]);
                                for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                                {
                                    if (string.IsNullOrEmpty(feedbakpk))
                                        feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                    else
                                        feedbakpk = feedbakpk + "','" + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                                }
                            }
                        }

                        html.Append("<table style='width: 1600; font-size: Medium;'cellpadding='5' cellspacing='0'>");
                        //html.Append("<center><div style=' page-break-after: always;'><table style='margin-top:50px;margin-left:30px'><tr><td style='margin-left:30px;width:920px'><img src=~/college/Left_Logo(" + Session["collegecode"].ToString() + ").jpeg alt='' style='height: 100px; width: 120px;' /></td><td style='margin-right:30px'><img src=~/Handler/staffphoto.ashx?staff_code='"+staffcode+"').jpg alt='' style='height: 100px; width: 120px;' /></td></tr></table><center><table>");

                        //html.Append("<tr><td><img src='" + "../college/Left_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpeg?'" + " style='height:80px; width:80px;'/></td><td style='text-align: right;' > </td><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: center;'><span style='font-size: Large;font-weight:bold;'>" + collegename + "</span> <br><span style='font-size: medium;'> <br>" + address1 + " , " + address2 + " , - " + address3 + ",<br>" + pincode + " <br><br>STUDENT FEEDBACK <br>  </span></td> <td ><img  src='" + "../Handler/staffphoto.ashx?staff_code='" + staffcode + "'?'" + " style='height:80px; width:80px;' /></td>");


                        string photo = "select * from staffphoto where staff_code='" + staffcode + "'";
                        DataSet photods = new DataSet();
                        photods = d2.select_method_wo_parameter(photo, "text");
                        if (photods.Tables[0].Rows.Count > 0)
                        {
                            MemoryStream memoryStream = new MemoryStream();
                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/Staff Photo/" + staffcode + ".jpeg")))
                            {
                                if (photods.Tables[0].Rows[0]["photo"] != null && photods.Tables[0].Rows[0]["photo"].ToString().Trim() != "")
                                {
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/Staff Photo/" + staffcode + ".jpeg")))
                                    {
                                        byte[] file = (byte[])photods.Tables[0].Rows[0]["photo"];
                                        memoryStream.Write(file, 0, file.Length);
                                        if (file.Length > 0)
                                        {
                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/Staff Photo/" + staffcode + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                        memoryStream.Dispose();
                                        memoryStream.Close();
                                    }
                                }
                            }


                        }

                        html.Append("<tr><td><img src='" + "../college/Left_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpeg'" + " style='height:120px; width:120px;'/></td><td style='text-align: right;' > </td><td style='font-size: 20px; font-family: Times New Roman;  border: 0px solid black; text-align: center;'><span style='font-size: Large;font-weight:bold;'>" + collegename + "</span> <br><span style='font-size: Large;'> <br>" + address1 + " , " + address2 + " , - " + address3 + ",<br>" + pincode + " <br><br>STUDENTS FEEDBACK <br>  </span></td> <td ><img  src='" + "../Staff Photo/" + staffcode + ".jpeg'" + " style='height:120px; width:120px; align=right;' /></td>");

                        html.Append("<table style='width: 1600; font-size: Medium;'cellpadding='5' cellspacing='0'>");

                        html.Append("<tr>");

                        html.Append("<td style='border: thin solid #000000;' align='Left'  class='style1'>Staff Code:</br>" + staffcode + "</td>");
                        html.Append("<td style='border: thin solid #000000;' align='Left'  class='style1'>Staff Name:</br>" + staffname + "</td>");
                        html.Append("<td style='border: thin solid #000000;' align='Left'  class='style1'>Department:</br>" + dept_staffacr + "</td>");
                        html.Append("<td style='border: thin solid #000000;' align='Left'  class='style1'>Course Code:</br>" + coursecode + "</td>");
                        html.Append("<td style='border: thin solid #000000;' align='Left'  class='style1'>Course Name:</br>" + couser_name + "</td>");
                        html.Append("<td style='border: thin solid #000000;' align='Left'  class='style1'>Section:</br>" + section + "</td>");
                        html.Append("<td style='border: thin solid #000000;' align='Left'  class='style1'>Branch:</br>" + branch + "-" + deptacr + "</td>");
                        string query = "select Point,MarkMasterPk,MarkType from CO_MarkMaster where CollegeCode='" + Session["collegecode"].ToString() + "' order by Point Asc";
                        feedbackds.Clear();
                        feedbackds = d2.select_method_wo_parameter(query, "text");
                     
                        html.Append(" </tr>");
                        html.Append("</table>");
                        html.Append("</br>");
                        html.Append("<table style='width: 1600; margin-bottom: 1px; font-size: Medium;'cellpadding='3' cellspacing='0'>");
                        DataSet questds = new DataSet();
                        questds.Clear();

                        string questqury = "SELECT distinct Question,QuestionMasterPK,HeaderCode FROM CO_FeedBackMaster B,CO_QuestionMaster Q ,CO_FeedBackQuestions FB WHERE  b.FeedBackMasterPK =fb.FeedBackMasterFK and q.QuestionMasterPK =fb.QuestionMasterFK and  InclueCommon='1' and FeedBackType='1' and B.FeedBackName='" + Convert.ToString(ddl_feedback.SelectedItem.Text) + "'  and B.CollegeCode='" + Session["collegecode"].ToString() + "' order by HeaderCode";
                        questds = d2.select_method_wo_parameter(questqury, "text");

                        string queryrd = " select  S.FeedbackUnicode,(convert(varchar(10), f.Batch_Year)+'-'+co.Course_Name+'-'+ dt.dept_acronym+'-'+convert(varchar(10), f.Semester)+'-'+f.Section ) as department,c.subject_code,s.StaffApplNo,sm.staff_code +' - '+staff_name as staff ,f.FeedBackMasterPK,Batch_Year ,f.semester,f.DegreeCode,f.Section,SubjectNo,c.subject_name,sm.staff_code,staff_name,c.acronym from CO_FeedBackMaster F,CO_StudFeedBack S,staff_appl_master sa,staffmaster sm ,subject c,Department dt,course co,Degree d  ,CO_MarkMaster M where M.MarkMasterPK =S.MarkMasterPK and  d.Degree_Code =f.degreecode and dt.Dept_Code =d.Dept_Code and co.Course_Id =d.Course_Id and c.subject_no=s.SubjectNo and sa.appl_no=sm.appl_no  and sa.appl_id=s.StaffApplNo and s.FeedBackMasterFK =f.FeedBackMasterPK  and f.Batch_Year in('" + batchyear + "') and f.semester in ('" + semester + "')  and isnull(f.Section,'') in ('" + section + "') and f.InclueCommon='1' and s.FeedbackUnicode<>'' and f.FeedBackMasterPK in('" + feedbakpk + "') and s.StaffApplNo in('" + applid + "') group by staff_code,staff_name, f.FeedBackMasterPK,StaffApplNo,Batch_Year,f.semester,f.DegreeCode ,f.Section,subject_name, SubjectNo,subject_code,Course_Name,dept_acronym,c.acronym, S.FeedbackUnicode order by sm.staff_name";

                        DataSet markds = new DataSet();
                        markds = d2.select_method_wo_parameter(queryrd, "text");
                        DataView dv = new DataView();
                        markds.Tables[0].DefaultView.RowFilter = " SubjectNo='" + subjectnum + "'";
                        dv = markds.Tables[0].DefaultView;
                        string unicode = string.Empty;
                        if (dv.Count > 0)
                        {
                            for (int uni = 0; uni < dv.Count; uni++)
                            {

                                string getunicode = Convert.ToString(dv[uni]["FeedbackUnicode"]);
                                if (unicode == "")
                                {
                                    unicode = getunicode;
                                }
                                else
                                {
                                    unicode = unicode + "','" + getunicode;
                                }

                            }

                        }

                        if (questds.Tables[0].Rows.Count > 0)
                        {
                            int sno = 0;
                            html.Append("<tr>");
                            html.Append("<td style='border: thin solid #000000; width: 10px;' align='center'  class='style1'></td>");
                            html.Append("<td style='border: thin solid #000000; width: 10px;' align='center'  class='style1'>Criteria</td>");
                            if (feedbackds.Tables[0].Rows.Count > 0)
                            {
                                for (int val = 0; val < feedbackds.Tables[0].Rows.Count; val++)
                                {
                                    html.Append("<td style='border: thin solid #000000; width: 10px;' align='center'  class='style1'></br>" + Convert.ToString(feedbackds.Tables[0].Rows[val]["MarkType"]) + "</td>");
                                }
                            }
                            html.Append("</tr>");
                            for (int j = 0; j < questds.Tables[0].Rows.Count; j++)
                            {
                                sno++;
                                html.Append("<tr>");

                                string question = Convert.ToString(questds.Tables[0].Rows[j]["Question"]);
                                string questionpk = Convert.ToString(questds.Tables[0].Rows[j]["QuestionMasterPK"]);
                                html.Append("<td style='border: thin solid #000000; width: 10px;' align='center'  class='style1'>" + sno + "</td>");
                                html.Append("<td style='border: thin solid #000000; width: 10px;font-size:15;' align='left'  class='style1'>" + question + "</td>");

                                feedbackds = d2.select_method_wo_parameter(query, "text");
                                if (feedbackds.Tables[0].Rows.Count > 0)
                                {
                                    for (int val = 0; val < feedbackds.Tables[0].Rows.Count; val++)
                                    {
                                        string markmasterpk = Convert.ToString(feedbackds.Tables[0].Rows[val]["MarkMasterPk"]);
                                        string q = d2.GetFunction(" select Count(FeedbackUnicode) as Strength from CO_StudFeedBack s,CO_MarkMaster m where FeedBackMasterFK='" + feedbackmasterpk + "' and StaffApplNo='" + applid + "' and s.MarkMasterPK=m.MarkMasterPK and m.MarkMasterPK ='" + markmasterpk + "' and questionmasterFK='" + questionpk + "' and FeedbackUnicode in('" + unicode + "')");


                                        html.Append("<td style='border: thin solid #000000;width: 20px;' align='center'  class='style1'></br>" + q + "</td>");
                                    }
                                }


                                html.Append(" </tr>");
                            }
                        }
                        html.Append("</table>");

                        html.Append("</br>");
                        html.Append("<table style='width: 1600;font-size: Medium;'cellpadding='5' cellspacing='0'>");
                        html.Append("<tr>");
                        html.Append("<td style='border: thin solid #000000; width: 20px;' align='Left'  class='style1'>Total</td>");

                        html.Append("<td style='border: thin solid #000000; width: 20px;' align='Right'  class='style1'>" + percentage + "</td>");
                        html.Append("</tr>");
                        html.Append("</table>");
                        html.Append("</div>");
                        html.Append("</center");
                        html.Append("</table>");

                        contentDiv.InnerHtml = html.ToString();
                        contentDiv.Visible = true;

                        ScriptManager.RegisterStartupScript(this, GetType(), "btn_print", "PrintDiv();", true);
                    }
                }
                else if (rdbloginbased.Checked == true)//delsi1112
                {
                    if (rdb_deptwise.Checked == true)
                    {
                        if (chkwithcomments.Checked == true)
                        {
                            contentDiv.InnerHtml = ""; StringBuilder html = new StringBuilder();
                            Hashtable overallcount = new Hashtable();
                            overallcount.Clear();
                            string sql1 = string.Empty;
                            DataSet feedbackds = new DataSet();
                            feedbackds.Clear();
                            string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                            string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                            string collegename = string.Empty;
                            string address1 = string.Empty;
                            string address2 = string.Empty;
                            string address3 = string.Empty;
                            string pincode = string.Empty;
                            sql1 = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                            feedbackds = d2.select_method_wo_parameter(sql1, "Text");
                            if (feedbackds.Tables[0].Rows.Count > 0)
                            {
                                collegename = Convert.ToString(feedbackds.Tables[0].Rows[0]["collname"]);
                                address1 = Convert.ToString(feedbackds.Tables[0].Rows[0]["address1"]);
                                address2 = Convert.ToString(feedbackds.Tables[0].Rows[0]["address2"]);
                                address3 = Convert.ToString(feedbackds.Tables[0].Rows[0]["address3"]);
                                pincode = Convert.ToString(feedbackds.Tables[0].Rows[0]["pincode"]);

                            }
                            string feedbackmasterpk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                            string subjectnum = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                            string department = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                            string staffcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                            string appno = d2.GetFunction("select appl_no from staffmaster where staff_code='" + staffcode + "'");
                            string applid = d2.GetFunction("select appl_id from staff_appl_master where appl_no='" + appno + "'");
                            string dept_staffacr = d2.GetFunction("select dept_acronym from stafftrans st,hrdept_master hr where staff_code='" + staffcode + "' and hr.dept_code=st.dept_code");

                            string staffname = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                            string coursecode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                            string couser_name = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                            string percentage = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                            string batchyear = string.Empty;
                            string branch = string.Empty;
                            string deptacr = string.Empty;
                            string section = string.Empty;
                            string semester = string.Empty;
                            if (department.Contains('-'))
                            {
                                string[] splitval = department.Split('-');
                                batchyear = Convert.ToString(splitval[0]);
                                branch = Convert.ToString(splitval[1]);
                                deptacr = Convert.ToString(splitval[2]);
                                semester = Convert.ToString(splitval[3]);
                                section = Convert.ToString(splitval[4]);

                            }

                            string fbpk = " select FeedBackMasterPK,ISNULL(issubjecttype,0)issubjecttype from CO_FeedBackMaster where FeedBackName ='" + ddl_feedback.SelectedItem.Value + "'";
                            DataSet dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                            string feedbakpk = string.Empty;
                            string issubjecttype = string.Empty;
                            if (dsfb.Tables.Count > 0)
                            {
                                if (dsfb.Tables[0].Rows.Count > 0)
                                {
                                    issubjecttype = Convert.ToString(dsfb.Tables[0].Rows[0]["issubjecttype"]);
                                    for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                                    {
                                        if (string.IsNullOrEmpty(feedbakpk))
                                            feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                        else
                                            feedbakpk = feedbakpk + "','" + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                                    }
                                }
                            }

                            html.Append("<table style='width: 1600; font-size: Medium;'cellpadding='5' cellspacing='0'>");



                            string photo = "select * from staffphoto where staff_code='" + staffcode + "'";
                            DataSet photods = new DataSet();
                            photods = d2.select_method_wo_parameter(photo, "text");
                            if (photods.Tables[0].Rows.Count > 0)
                            {
                                MemoryStream memoryStream = new MemoryStream();
                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/Staff Photo/" + staffcode + ".jpeg")))
                                {
                                    if (photods.Tables[0].Rows[0]["photo"] != null && photods.Tables[0].Rows[0]["photo"].ToString().Trim() != "")
                                    {
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/Staff Photo/" + staffcode + ".jpeg")))
                                        {
                                            byte[] file = (byte[])photods.Tables[0].Rows[0]["photo"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                thumb.Save(HttpContext.Current.Server.MapPath("~/Staff Photo/" + staffcode + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            }
                                            memoryStream.Dispose();
                                            memoryStream.Close();
                                        }
                                    }
                                }


                            }

                            html.Append("<tr><td><img src='" + "../college/Left_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpeg'" + " style='height:120px; width:120px;'/></td><td style='text-align: right;' > </td><td style='font-size: 20px; font-family: Times New Roman;  border: 0px solid black; text-align: center;'><span style='font-size: Large;font-weight:bold;'>" + collegename + "</span> <br><span style='font-size: Large;'> <br>" + address1 + " , " + address2 + " , - " + address3 + ",<br>" + pincode + " <br><br>STUDENTS FEEDBACK <br>  </span></td> <td ><img  src='" + "../Staff Photo/" + staffcode + ".jpeg'" + " style='height:120px; width:120px; align=right;' /></td>");

                            html.Append("<table style='width: 1600; font-size: Medium;'cellpadding='5' cellspacing='0'>");

                            html.Append("<tr>");

                            html.Append("<td style='border: thin solid #000000;' align='Left'  class='style1'>Staff Code:</br>" + staffcode + "</td>");
                            html.Append("<td style='border: thin solid #000000;' align='Left'  class='style1'>Staff Name:</br>" + staffname + "</td>");
                            html.Append("<td style='border: thin solid #000000;' align='Left'  class='style1'>Department:</br>" + dept_staffacr + "</td>");
                            html.Append("<td style='border: thin solid #000000;' align='Left'  class='style1'>Course Code:</br>" + coursecode + "</td>");
                            html.Append("<td style='border: thin solid #000000;' align='Left'  class='style1'>Course Name:</br>" + couser_name + "</td>");
                            html.Append("<td style='border: thin solid #000000;' align='Left'  class='style1'>Section:</br>" + section + "</td>");
                            html.Append("<td style='border: thin solid #000000;' align='Left'  class='style1'>Branch:</br>" + branch + "-" + deptacr + "</td>");
                            string query = "select Point,MarkMasterPk,MarkType from CO_MarkMaster where CollegeCode='" + Session["collegecode"].ToString() + "' order by Point desc";
                            feedbackds.Clear();
                            feedbackds = d2.select_method_wo_parameter(query, "text");

                            html.Append(" </tr>");
                            html.Append("</table>");
                            html.Append("</br>");
                            html.Append("<table style='width: 1600; margin-bottom: 1px; font-size: Medium;'cellpadding='3' cellspacing='0'>");
                            DataSet questds = new DataSet();
                            questds.Clear();

                            string questqury = "SELECT distinct Question,QuestionMasterPK,HeaderCode FROM CO_FeedBackMaster B,CO_QuestionMaster Q ,CO_FeedBackQuestions FB WHERE  b.FeedBackMasterPK =fb.FeedBackMasterFK and q.QuestionMasterPK =fb.QuestionMasterFK and  InclueCommon='0' and FeedBackType='1' and B.FeedBackName='" + Convert.ToString(ddl_feedback.SelectedItem.Text) + "'  and B.CollegeCode='" + Session["collegecode"].ToString() + "' order by HeaderCode";
                            questds = d2.select_method_wo_parameter(questqury, "text");

                            string queryrd = " select  S.App_No,(convert(varchar(10), f.Batch_Year)+'-'+co.Course_Name+'-'+ dt.dept_acronym+'-'+convert(varchar(10), f.Semester)+'-'+f.Section ) as department,c.subject_code,s.StaffApplNo,sm.staff_code +' - '+staff_name as staff ,f.FeedBackMasterPK,Batch_Year ,f.semester,f.DegreeCode,f.Section,SubjectNo,c.subject_name,sm.staff_code,staff_name,c.acronym from CO_FeedBackMaster F,CO_StudFeedBack S,staff_appl_master sa,staffmaster sm ,subject c,Department dt,course co,Degree d  ,CO_MarkMaster M where M.MarkMasterPK =S.MarkMasterPK and  d.Degree_Code =f.degreecode and dt.Dept_Code =d.Dept_Code and co.Course_Id =d.Course_Id and c.subject_no=s.SubjectNo and sa.appl_no=sm.appl_no  and sa.appl_id=s.StaffApplNo and s.FeedBackMasterFK =f.FeedBackMasterPK  and f.Batch_Year in('" + batchyear + "') and f.semester in ('" + semester + "')  and isnull(f.Section,'') in ('" + section + "') and f.InclueCommon='0' and f.FeedBackMasterPK in('" + feedbakpk + "') and s.StaffApplNo in('" + applid + "') group by staff_code,staff_name, f.FeedBackMasterPK,StaffApplNo,Batch_Year,f.semester,f.DegreeCode ,f.Section,subject_name, SubjectNo,subject_code,Course_Name,dept_acronym,c.acronym, S.App_No order by sm.staff_name";

                            DataSet markds = new DataSet();
                            markds = d2.select_method_wo_parameter(queryrd, "text");
                            DataView dv = new DataView();
                            markds.Tables[0].DefaultView.RowFilter = " SubjectNo='" + subjectnum + "'";
                            dv = markds.Tables[0].DefaultView;
                            string unicode = string.Empty;
                            if (dv.Count > 0)
                            {
                                for (int uni = 0; uni < dv.Count; uni++)
                                {

                                    string getunicode = Convert.ToString(dv[uni]["App_No"]);
                                    if (unicode == "")
                                    {
                                        unicode = getunicode;
                                    }
                                    else
                                    {
                                        unicode = unicode + "','" + getunicode;
                                    }

                                }

                            }

                            if (questds.Tables[0].Rows.Count > 0)
                            {
                                int sno = 0;
                                html.Append("<tr>");
                                html.Append("<td style='border: thin solid #000000; width: 10px;' align='center'  class='style1'></td>");
                                html.Append("<td style='border: thin solid #000000; width: 10px;' align='center'  class='style1'>Criteria</td>");
                                if (feedbackds.Tables[0].Rows.Count > 0)
                                {
                                    for (int val = 0; val < feedbackds.Tables[0].Rows.Count; val++)
                                    {
                                        html.Append("<td style='border: thin solid #000000; width: 10px;' align='center'  class='style1'></br>" + Convert.ToString(feedbackds.Tables[0].Rows[val]["MarkType"]) + "</td>");
                                    }
                                }
                                html.Append("<td style='border: thin solid #000000; width: 10px;' align='center'  class='style1'>Total</td>");
                                html.Append("</tr>");

                                int markto = 0;
                                for (int j = 0; j < questds.Tables[0].Rows.Count; j++)
                                {
                                    sno++;
                                    html.Append("<tr>");

                                    string question = Convert.ToString(questds.Tables[0].Rows[j]["Question"]);
                                    string questionpk = Convert.ToString(questds.Tables[0].Rows[j]["QuestionMasterPK"]);
                                    html.Append("<td style='border: thin solid #000000; width: 10px;' align='center'  class='style1'>" + sno + "</td>");
                                    html.Append("<td style='border: thin solid #000000; width: 10px;font-size:15;' align='left'  class='style1'>" + question + "</td>");

                                    feedbackds = d2.select_method_wo_parameter(query, "text");
                                    int tot = 0;
                                    if (feedbackds.Tables[0].Rows.Count > 0)
                                    {

                                        for (int val = 0; val < feedbackds.Tables[0].Rows.Count; val++)
                                        {

                                            string markmasterpk = Convert.ToString(feedbackds.Tables[0].Rows[val]["MarkMasterPk"]);
                                            string q = d2.GetFunction(" select Count(App_No) as Strength from CO_StudFeedBack s,CO_MarkMaster m where FeedBackMasterFK='" + feedbackmasterpk + "' and StaffApplNo='" + applid + "' and s.MarkMasterPK=m.MarkMasterPK and m.MarkMasterPK ='" + markmasterpk + "' and questionmasterFK='" + questionpk + "' and App_No in('" + unicode + "')");
                                            tot = tot + Convert.ToInt32(q);
                                            html.Append("<td style='border: thin solid #000000;width: 20px;' align='center'  class='style1'></br>" + q + "</td>");
                                            markto = Convert.ToInt32(q);
                                            if (!overallcount.ContainsKey(markmasterpk))
                                            {
                                                overallcount.Add(markmasterpk, markto);
                                            }
                                            else
                                            {
                                                if (overallcount.ContainsKey(markmasterpk))
                                                {
                                                    int allval = 0;
                                                    int.TryParse(Convert.ToString(overallcount[Convert.ToString(markmasterpk)]), out allval);
                                                    allval = allval + markto;
                                                    overallcount.Remove(markmasterpk);
                                                    overallcount.Add(markmasterpk, allval);

                                                }
                                            }

                                        }
                                        html.Append("<td style='border: thin solid #000000;width: 20px;' align='center'  class='style1'></br>" + tot + "</td>");

                                        if (!overallcount.ContainsKey("tot"))
                                        {
                                            overallcount.Add("tot", tot);
                                        }
                                        else
                                        {
                                            if (overallcount.ContainsKey("tot"))
                                            {
                                                int allval = 0;
                                                int.TryParse(Convert.ToString(overallcount[Convert.ToString("tot")]), out allval);
                                                allval = allval + tot;
                                                overallcount.Remove("tot");
                                                overallcount.Add("tot", allval);

                                            }
                                        }
                                    }

                                    html.Append(" </tr>");
                                }

                                html.Append("<tr>");
                                html.Append("<td style='border: thin solid #000000; width: 20px;' align='Left'  class='style1'></td>");
                                html.Append("<td style='border: thin solid #000000; width: 20px;' align='Center'  class='style1'>Total</td>");

                                for (int val = 0; val < feedbackds.Tables[0].Rows.Count; val++)
                                {

                                    string markmasterpk = Convert.ToString(feedbackds.Tables[0].Rows[val]["MarkMasterPk"]);
                                    foreach (DictionaryEntry item in overallcount)
                                    {
                                        string key = Convert.ToString(item.Key);

                                        string value = Convert.ToString(item.Value);
                                        if (key == markmasterpk)
                                        {
                                            html.Append("<td style='border: thin solid #000000; width: 20px;' align='Center'  class='style1'>" + value + "</td>");

                                        }

                                    }
                                }
                                foreach (DictionaryEntry item in overallcount)
                                {
                                    string key = Convert.ToString(item.Key);

                                    string value = Convert.ToString(item.Value);
                                    if (key == "tot")
                                    {
                                        html.Append("<td style='border: thin solid #000000; width: 20px;' align='Center'  class='style1'>" + value + "</td>");

                                    }

                                }
                                int markobtained = 0;
                                foreach (DictionaryEntry item in overallcount)
                                {

                                    string key = Convert.ToString(item.Key);

                                    string value = Convert.ToString(item.Value);
                                    if (key != "tot")
                                    {
                                        string point = d2.GetFunction("select point from CO_MarkMaster where MarkMasterPK='" + key + "'");
                                        int getpoint = Convert.ToInt32(point);
                                        int getval = Convert.ToInt32(value);
                                        int tots = getpoint * getval;
                                        markobtained = markobtained + tots;
                                    }
                                }

                                html.Append("</tr>");
                                html.Append("<tr>");
                                html.Append("<td align='Left'  class='style1'></td>");
                                html.Append("<td align='Center'  class='style1'>Percentage:  " + percentage + "</td>");
                                
                                html.Append("</tr>");

                            }
                            html.Append("</table>");




                            DataSet dscomments = new DataSet();
                            dscomments.Clear();
                            string commentsqry = "select distinct isnull(comments,'') as comments from CO_FeedBackMaster F,CO_StudFeedBack S,staff_appl_master sa,staffmaster sm ,subject c,Department dt,course co,Degree d  ,CO_MarkMaster M where M.MarkMasterPK =S.MarkMasterPK and  d.Degree_Code =f.degreecode and dt.Dept_Code =d.Dept_Code and co.Course_Id =d.Course_Id and c.subject_no=s.SubjectNo and sa.appl_no=sm.appl_no  and sa.appl_id=s.StaffApplNo and s.FeedBackMasterFK =f.FeedBackMasterPK  and f.Batch_Year in('" + batchyear + "') and f.semester in ('" + semester + "')  and isnull(f.Section,'') in ('" + section + "') and f.InclueCommon='0' and f.FeedBackMasterPK in('" + feedbakpk + "') and s.StaffApplNo in('" + applid + "')";

                            dscomments = d2.select_method_wo_parameter(commentsqry, "text");
                            html.Append("</br>");
                            if (dscomments.Tables[0].Rows.Count > 0)
                            {
                                html.Append("<table style='width: 1600;font-size: Medium;'cellpadding='5' cellspacing='0'>");
                                html.Append("<tr>");
                                html.Append("<td align='Center'  class='style1'>Feedback Remarks</td>");

                                int signo = 0;
                                html.Append("</tr>");
                                html.Append("<tr>");
                                for (int j = 0; j < dscomments.Tables[0].Rows.Count; j++)
                                {

                                    signo++;
                                    string getcomments = Convert.ToString(dscomments.Tables[0].Rows[j]["comments"]);
                                    if (getcomments != "")
                                    {

                                        html.Append("<td align='Left'  class='style1'>" + signo + ".            " + getcomments + "</td>");
                                    }

                                }
                                html.Append("</tr>");
                                html.Append("</table>");
                                html.Append("</div>");
                                html.Append("</center");
                                html.Append("</table>");

                            }

                            html.Append("</div>");
                            html.Append("</center");
                            html.Append("</table>");

                            contentDiv.InnerHtml = html.ToString();
                            contentDiv.Visible = true;

                            ScriptManager.RegisterStartupScript(this, GetType(), "btn_print", "PrintDiv();", true);

                        }

                       
                    }

                }
            }
        }
        catch (Exception ex)
        {


        }
    }
    protected void rdbanonyomous_Click(object sender, EventArgs e)
    {
        if (rdbanonyomous.Checked == true)
        {
            rdbloginbased.Checked = false;

        }

    }
    protected void rdbloginbased_Click(object sender, EventArgs e)
    {
        if (rdbloginbased.Checked == true)
        {
            rdbanonyomous.Checked = false;
        }

    }
}
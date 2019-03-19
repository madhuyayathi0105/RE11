using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
public partial class FeedBackMOD_Feedbackreport_consolidation : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ReuasableMethods rs = new ReuasableMethods();
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
            rdbtype.SelectedIndex = 0;
            bindclg();
            BindBatch();
            binddept();
            bindsem();
            bindstaff();
            bindfeedback();
            bindSubject();
        }
    }
    protected void cbl_clgnameformat6_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_clgnameformat6, cbl_clgnameformat6, txtclgnameformat6, "College");
        binddept();
        bindfeedback();
    }
    protected void cb_clgnameformat6_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_clgnameformat6, cbl_clgnameformat6, txtclgnameformat6, "College", "--Select--");
        binddept();
        bindfeedback();
    }
    protected void ddlformate6_deptname_selectedindex(object sender, EventArgs e)
    {
        bindstaff();
        bindfeedback();
        bindsem();
        bindSubject();
    }
    protected void ddl_feedbackformate6_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindstaff(); bindSubject();
    }
    protected void cbl_staffnameformat6_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_staffnameformat6, cbl_staffnameformat6, txtstaffnameformat6, "Staff Name");
        bindSubject();
    }
    protected void cb_staffnameformat6_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_staffnameformat6, cbl_staffnameformat6, txtstaffnameformat6, "Staff Name", "--Select--");
        bindSubject();
    }
    protected void cbl_formate6batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_formate6batch, cbl_formate6batch, txt_formate6batch, "Batch");
    }
    protected void cb_formate6batch_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_formate6batch, cbl_formate6batch, txt_formate6batch, "Batch", "--Select--");
    }
    protected void cb_formate6sem_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_formate6sem, cbl_formate6sem, txt_formate6sem, "Sem", "--Select--");
    }
    protected void cbl_formate6sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_formate6sem, cbl_formate6sem, txt_formate6sem, "Sem");
    }
    protected void cb_subjectnameformat6_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_subjectnameformat6, cbl_subjectnameformat6, txtsubjectnameformat6, "Subject", "--Select--");
    }
    protected void cbl_subjectnameformat6_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_staffnameformat6, cbl_subjectnameformat6, txtsubjectnameformat6, "Subject");
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
    protected void binddept()
    {
        try
        {
            ds.Clear();
            ddlformate6_deptname.Items.Clear();
            string college_cd = rs.GetSelectedItemsValueAsString(cbl_clgnameformat6);
            string query = " select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.college_code in ('" + college_cd + "')";
            //string query = " select Dept_Code,Dept_Name from Department where college_code in ('" + college_cd + "') ";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlformate6_deptname.DataSource = ds;
                ddlformate6_deptname.DataTextField = "Dept_Name";
                ddlformate6_deptname.DataValueField = "Dept_Code";
                ddlformate6_deptname.DataBind();
            }
            else
            {
                //txtdeptnameformat6.Text = "--Select--";
                ddlformate6_deptname.Items.Add(new System.Web.UI.WebControls.ListItem("Select", "0"));
            }
        }
        catch { }
    }
    protected void bindstaff()
    {
        try
        {
            ds.Clear();
            cbl_staffnameformat6.Items.Clear(); string degreecode = "";
            if (ddlformate6_deptname.SelectedItem.Text.Trim() != "0")
            {
                degreecode = Convert.ToString(ddlformate6_deptname.SelectedItem.Value);
            }
            //string degreecode = rs.GetSelectedItemsValueAsString(cbl_deptnameformat6);
            string query = " select s.staff_code+'$'+convert(varchar, sa.appl_id) as staff_code,s.staff_name from staff_appl_master sa,staffmaster s,stafftrans t where sa.appl_no =s.appl_no and s.staff_code =t.staff_code and t.latestrec =1 and s.resign =0 and s.settled =0 and t.dept_code in ('" + degreecode + "') order by s.staff_name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_staffnameformat6.DataSource = ds;
                cbl_staffnameformat6.DataTextField = "staff_name";
                cbl_staffnameformat6.DataValueField = "staff_code";//appl_id";
                cbl_staffnameformat6.DataBind();
                if (cbl_staffnameformat6.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_staffnameformat6.Items.Count; row++)
                    {
                        cbl_staffnameformat6.Items[row].Selected = true;
                    }
                    cb_staffnameformat6.Checked = true;
                    txtstaffnameformat6.Text = "Staff Name(" + cbl_staffnameformat6.Items.Count + ")";
                }
            }
            else
            {
                txtstaffnameformat6.Text = "--Select--";
            }
        }
        catch { }
    }
    protected void bindfeedback()
    {
        try
        {
            ddl_feedbackformate6.Items.Clear();
            collegecode = "";
            collegecode = rs.GetSelectedItemsValueAsString(cbl_clgnameformat6);
            //string degreecode = rs.GetSelectedItemsValueAsString(cbl_deptnameformat6);
            string degreecode = Convert.ToString(ddlformate6_deptname.SelectedItem.Value);
            string batchyear = rs.GetSelectedItemsValueAsString(cbl_formate6batch);
            ds.Clear();
            string q1 = ""; string empty = "";
            if (degreecode.Trim() != "")
            {
                q1 = " select d.Degree_Code from Degree d,Department dt where d.Dept_Code =dt.Dept_Code and d.Dept_Code in('" + degreecode + "')";
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
            if (ds.Tables[0].Rows.Count == 0)
            {
                ds.Clear();
                q1 = " select distinct FeedBackName  from CO_FeedBackMaster where CollegeCode in('" + collegecode + "') ";
                ds = d2.select_method_wo_parameter(q1, "Text");
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_feedbackformate6.DataSource = ds;
                ddl_feedbackformate6.DataTextField = "FeedBackName";
                ddl_feedbackformate6.DataValueField = "FeedBackName";
                ddl_feedbackformate6.DataBind();
                ddl_feedbackformate6.Items.Insert(0, "--Select--");
            }
            else
            {
                ddl_feedbackformate6.Items.Insert(0, "--Select--");
            }
        }
        catch { }
    }
    public void bindclg()
    {
        try
        {
            ds.Clear();
            cbl_clgnameformat6.Items.Clear();
            string clgname = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(clgname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_clgnameformat6.DataSource = ds;
                cbl_clgnameformat6.DataTextField = "collname";
                cbl_clgnameformat6.DataValueField = "college_code";
                cbl_clgnameformat6.DataBind();
            }
            if (cbl_clgnameformat6.Items.Count > 0)
            {
                for (int row = 0; row < cbl_clgnameformat6.Items.Count; row++)
                {
                    cbl_clgnameformat6.Items[row].Selected = true;
                    cb_clgnameformat6.Checked = true;
                }
                txtclgnameformat6.Text = "College(" + cbl_clgnameformat6.Items.Count + ")";
            }
            else
            {
                txtclgnameformat6.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void BindBatch()
    {
        try
        {
            txt_formate6batch.Text = "--Select--";
            string college_cd = rs.GetSelectedItemsValueAsString(cbl_clgnameformat6);
            if (college_cd != "")
            {
                ds = d2.BindBatch();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_formate6batch.DataSource = ds;
                    cbl_formate6batch.DataTextField = "batch_year";
                    cbl_formate6batch.DataValueField = "batch_year";
                    cbl_formate6batch.DataBind();
                }
                if (cbl_formate6batch.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_formate6batch.Items.Count; row++)
                    {
                        cbl_formate6batch.Items[row].Selected = true;
                        cb_formate6batch.Checked = true;
                    }
                    txt_formate6batch.Text = "Batch(" + cbl_formate6batch.Items.Count + ")";
                }
                else
                {
                    txt_formate6batch.Text = "--Select--";
                }
            }
            binddept();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }
    protected void bindsem()
    {
        string college_cd = rs.GetSelectedItemsValueAsString(cbl_clgnameformat6);
        string max = d2.GetFunction("select  distinct MAX(duration) from degree where college_code in('" + college_cd + "')  ");
        if (Convert.ToInt32(max) > 0)
        {
            cbl_formate6sem.Items.Clear();
            for (int row = 0; row < Convert.ToInt32(max); row++)
            {
                cbl_formate6sem.Items.Add(new System.Web.UI.WebControls.ListItem((row + 1).ToString(), (row + 1).ToString()));
                cbl_formate6sem.Items[row].Selected = true;
                cb_formate6sem.Checked = true;
            }
            txt_formate6sem.Text = "Sem(" + cbl_formate6sem.Items.Count + ")";
        }
    }
    protected void bindSubject()
    {
        string staffcode = GetSelectedItemsValueAsString(cbl_staffnameformat6, 0);
        string batchyear = rs.GetSelectedItemsValueAsString(cbl_formate6batch);
        //string st_type = d2.GetFunction(" select distinct Subject_Type from CO_FeedBackMaster where FeedBackName ='" + ddl_feedbackformate6.SelectedItem.Value + "'");
        //string[] split = st_type.Split(',');
        //string sub_type = "";
        ds.Clear();
        ds = d2.select_method_wo_parameter("select distinct Subject_Type from CO_FeedBackMaster where FeedBackName ='" + ddl_feedbackformate6.SelectedItem.Value + "'", "text");
        string sub_type = GetdatasetRowstring(ds, "Subject_Type");

        string sub_type1 = sub_type.Replace("','", ",");
        sub_type = sub_type1.Replace(",", "','");
        //for (int i = 0; i < split.Length; i++)
        //{
        //    if (sub_type == "")
        //    {
        //        sub_type = split[i];
        //    }
        //    else
        //    {
        //        sub_type += "','" + split[i];
        //    }
        //}
        string sub_name = "select distinct su.subject_code+'$'+convert(varchar ,su.subject_no) as subject_code,su.subject_name  from staff_selector ss,staffmaster s,subject su,sub_sem sm where ss.staff_code =s.staff_code and ss.subject_no =su.subject_no and su.subType_no =sm.subType_no and ss.staff_code in ('" + staffcode + "')  and ss.batch_year in ('" + batchyear + "')  and sm.subject_type in ('" + sub_type + "') order by subject_name";//and ss.Sections in ('" + section + "')
        ds = d2.select_method_wo_parameter(sub_name, "Text");
        cbl_subjectnameformat6.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_subjectnameformat6.DataSource = ds;
            cbl_subjectnameformat6.DataTextField = "subject_name";
            cbl_subjectnameformat6.DataValueField = "subject_code";
            cbl_subjectnameformat6.DataBind();
        }
        if (cbl_subjectnameformat6.Items.Count > 0)
        {
            for (int row = 0; row < cbl_subjectnameformat6.Items.Count; row++)
            {
                cbl_subjectnameformat6.Items[row].Selected = true;
                cb_subjectnameformat6.Checked = true;
            }
            txtsubjectnameformat6.Text = "Subject(" + cbl_subjectnameformat6.Items.Count + ")";
        }
        else
        {
            txtsubjectnameformat6.Text = "--Select--";
        }
    }
    protected void btn_Go_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_error.Visible = false; Printcontrol1.Visible = false;
            if (ddl_feedbackformate6.SelectedItem.Value.Trim() != "--Select--")
            {
                string q1 = ""; btnprintimag.Visible = false;
                rs.Fpreadheaderbindmethod("S No-50/Question-300", FpSpread1, "false");
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 2, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                string degreecode = Convert.ToString(ddlformate6_deptname.SelectedItem.Value);
                string Batchyear = rs.GetSelectedItemsValueAsString(cbl_formate6batch);
                string semester = rs.GetSelectedItemsValueAsString(cbl_formate6sem);
                string appl_id = GetSelectedItemsValueAsString(cbl_staffnameformat6, 1);
                string subjectno = GetSelectedItemsValueAsString(cbl_subjectnameformat6, 1);
                string collegecode = rs.GetSelectedItemsValueAsString(cbl_clgnameformat6);
                if (rdbtype.SelectedIndex == 0)
                {
                    if (collegecode.Trim() == "--Select--" && degreecode.Trim() == "--Select--" && Batchyear.Trim() == "--Select--" && appl_id.Trim() == "--Select--" && subjectno.Trim() == "--Select--")
                    {
                        lbl_error.Text = "No Records Founds";
                        lbl_error.Visible = true;
                        return;
                    }
                }
                if (rdbtype.SelectedIndex == 1)
                {
                    if (collegecode.Trim() == "--Select--" && degreecode.Trim() == "--Select--")
                    {
                        lbl_error.Text = "No Records Founds";
                        lbl_error.Visible = true;
                        return;
                    }
                }
                ds.Clear();
                q1 = " select FeedBackMasterPK from CO_FeedBackMaster fm,Degree d,Department dt,course c where d.Degree_Code=fm.DegreeCode and d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id and FeedBackName ='" + ddl_feedbackformate6.SelectedItem.Value + "' and fm.CollegeCode=d.college_code";
                if (rdbtype.SelectedIndex == 0)
                {
                    q1 += " and semester in ('" + semester + "') and Batch_Year in('" + Batchyear + "') and Acadamic_Isgeneral='0' and feedbacktype='1' and dt.Dept_Code in ('" + degreecode + "') ";
                }
                else if (rdbtype.SelectedIndex == 1)
                {
                    q1 += " and dt.Dept_Code in ('" + degreecode + "') ";
                }
                else if (rdbtype.SelectedIndex == 2)
                {
                    q1 += " and fm.collegecode in('" + collegecode + "')";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                string feedbackFK = GetdatasetRowstring(ds, "FeedBackMasterPK");
                q1 = " select distinct sf.QuestionMasterFK,qm.Question from  CO_StudFeedBack sf,CO_QuestionMaster qm where qm.QuestionMasterPK=sf.QuestionMasterFK  and FeedBackMasterFK in('" + feedbackFK + "') ";
                if (rdbtype.SelectedIndex == 0)
                    q1 += " and sf.StaffApplNo in('" + appl_id + "') and sf.SubjectNo in('" + subjectno + "')";
                q1 += " select MarkMasterPK,MarkType from CO_MarkMaster where collegecode in('" + collegecode + "')";
                if (rdbtype.SelectedIndex == 0)
                {
                    q1 += " select COUNT(FeedbackUnicode)Studentcount,MarkMasterPK,QuestionMasterFK from CO_StudFeedBack  where  FeedBackMasterFK in('" + feedbackFK + "') and StaffApplNo in ('" + appl_id + "') and SubjectNo in('" + subjectno + "')  and isnull(FeedbackUnicode,'')<>'' group by MarkMasterPK,QuestionMasterFK";
                }
                // if (rdbtype.SelectedIndex == 0)
                // q1 += " and fm.semester in('" + semester + "') and dt.Dept_Code in('" + Convert.ToString(ddlformate6_deptname.SelectedItem.Value) + "')";
                //and sf.StaffApplNo in ('" + appl_id + "') and SubjectNo in('" + subjectno + "')
                if (rdbtype.SelectedIndex == 1)
                {
                    q1 += "  select COUNT(FeedbackUnicode)Studentcount,MarkMasterPK,QuestionMasterFK from CO_FeedBackMaster fm,CO_StudFeedBack sf,Degree d,Department dt where d.Degree_Code=fm.DegreeCode  and d.Dept_Code=dt.Dept_Code and fm.FeedBackMasterPK=sf.FeedBackMasterFK and sf.FeedBackMasterFK in('" + feedbackFK + "')  and dt.Dept_Code in('" + Convert.ToString(ddlformate6_deptname.SelectedItem.Value) + "') and isnull(sf.FeedbackUnicode,'')<>'' and fm.CollegeCode in ('" + collegecode + "') group by MarkMasterPK,QuestionMasterFK";
                }
                if (rdbtype.SelectedIndex == 2)
                {
                    q1 += "   select COUNT(FeedbackUnicode)Studentcount,MarkMasterPK,QuestionMasterFK from CO_FeedBackMaster fm,CO_StudFeedBack sf where FeedBackMasterPK=sf.FeedBackMasterFK and sf.FeedBackMasterFK in('" + feedbackFK + "') and isnull(sf.FeedbackUnicode,'')<>'' and fm.CollegeCode in ('" + collegecode + "') group by MarkMasterPK,QuestionMasterFK";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                bool visibleFpspreadrpt = false;
                if (ds.Tables != null)
                {
                    #region Header
                    int col = 0;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Attended Student";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dr["MarkType"]);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(dr["MarkMasterPK"]);
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = "Total";
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                        int.TryParse(Convert.ToString(ds.Tables[1].Rows.Count), out col);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, col + 1);
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Percentage";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dr["MarkType"]);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(dr["MarkMasterPK"]);
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - col, 1, col);
                    }
                    #endregion
                    #region Row Values
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        double verypoor = 0;
                        double poor = 0; Hashtable charthash1 = new Hashtable();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            FpSpread1.Sheets[0].Rows.Count++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].Rows.Count); FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Locked = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(dr["Question"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Tag = Convert.ToString(dr["QuestionMasterFK"]); FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Locked = true;
                            double total = 0; bool totalcheck = false;
                            for (int c = 2; c < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; c++)
                            {
                                visibleFpspreadrpt = true;
                                string markFK = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, c].Tag);
                                string QuestionFK = Convert.ToString(dr["QuestionMasterFK"]);
                                if (totalcheck == false)
                                {
                                    if (markFK.Trim() != "" && QuestionFK.Trim() != "" && markFK.Trim() != "Total")
                                    {
                                        ds.Tables[2].DefaultView.RowFilter = " MarkMasterPK='" + markFK + "' and  QuestionMasterFK='" + QuestionFK + "'";
                                        DataView dv_studentcount = ds.Tables[2].DefaultView;
                                        if (dv_studentcount.Count > 0)
                                        {
                                            double studentcount = 0;
                                            double.TryParse(Convert.ToString(dv_studentcount[0]["Studentcount"]), out studentcount);
                                            total += studentcount;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, c].Text = Convert.ToString(studentcount);
                                            //07.04.17
                                            string markvalue = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, c].Text);
                                            if (!charthash1.Contains(Convert.ToString(markFK) + "/" + Convert.ToString(markvalue)))
                                            {
                                                charthash1.Add(Convert.ToString(markFK) + "/" + Convert.ToString(markvalue), markvalue + "/" + Convert.ToString(Math.Round(studentcount, 2)));//+ "$" + Convert.ToString(headercode)
                                            }
                                            else
                                            {
                                                string val = Convert.ToString(charthash1[Convert.ToString(markFK) + "/" + Convert.ToString(markvalue)]);
                                                string[] point = val.Split('/');
                                                double previouspoint = 0; double curentpoint = 0;
                                                double.TryParse(Convert.ToString(point[1]), out previouspoint);
                                                double.TryParse(Convert.ToString(Math.Round(studentcount, 2)), out curentpoint);
                                                val = markvalue + "/" + (previouspoint + curentpoint);
                                                charthash1.Remove(Convert.ToString(markFK) + "/" + Convert.ToString(markvalue));
                                                charthash1.Add(Convert.ToString(markFK) + "/" + Convert.ToString(markvalue), val);
                                            }
                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, c].Text = "0";
                                        }
                                    }
                                    else
                                    {
                                        totalcheck = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, c].Text = Convert.ToString(total);
                                    }
                                }
                                else
                                {
                                    ds.Tables[2].DefaultView.RowFilter = " MarkMasterPK='" + markFK + "' and  QuestionMasterFK='" + QuestionFK + "'";
                                    DataView dv_studentcount = ds.Tables[2].DefaultView;
                                    if (dv_studentcount.Count > 0)
                                    {
                                        double studentcount = 0;
                                        double.TryParse(Convert.ToString(dv_studentcount[0]["Studentcount"]), out studentcount);
                                        double percent = (studentcount / total) * 100;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, c].Text = Convert.ToString(Math.Round(percent, 2));
                                        string markvalue = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, c].Text);
                                        if (markvalue.Trim() == "Very Poor")
                                        {
                                            verypoor += Math.Round(percent, 2);
                                        }
                                        if (markvalue.Trim() == "Poor")
                                        {
                                            poor += Math.Round(percent, 2);
                                        }
                                    }
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, c].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, c].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, c].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, c].Locked = true;
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Font.Size = FontUnit.Medium;
                        }
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Grand Total";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Locked = true;
                        for (int k = 2; k < ds.Tables[1].Rows.Count + 2; k++)
                        {
                            string markFK = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, k].Tag);
                            string markvalue = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, k].Text);
                            if (markFK.Trim() != "")
                            {
                                if (charthash1.Count > 0)
                                {
                                    string value = ""; double granttotal = 0;
                                    if (charthash1.Contains(markFK + "/" + markvalue))
                                    {
                                        value = charthash1[markFK + "/" + markvalue].ToString();
                                        string[] val = value.Split('/');
                                        double.TryParse(Convert.ToString(val[1]), out granttotal);
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, k].Text = Convert.ToString(Math.Round(granttotal, 2));
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, k].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, k].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, k].Font.Name = "Book Antiqua";
                                }
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, k].Locked = true;
                        }
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Text = "Very Poor Percentage";//col + 3
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Text = Convert.ToString(verypoor);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].ForeColor = Color.SeaGreen;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].ForeColor = Color.Brown;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Text = "Poor Percentage";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Text = Convert.ToString(poor);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].ForeColor = Color.SeaGreen;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].ForeColor = Color.Brown;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Text = "Average Percentage";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Locked = true;
                        double questioncount = 0;
                        double.TryParse(Convert.ToString(ds.Tables[0].Rows.Count), out questioncount);
                        string avgpercentage = "";
                        if (questioncount != 0)
                        {
                            double avg = (verypoor + poor) / questioncount;
                            avgpercentage = Convert.ToString(Math.Round(avg, 2));
                        }
                        else
                        {
                            avgpercentage = "-";
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Text = Convert.ToString(avgpercentage);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].ForeColor = Color.SeaGreen;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].ForeColor = Color.Brown;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].FrozenColumnCount = 2;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        FpSpread1.Height = 400;
                        FpSpread1.Width = 950;
                        if (visibleFpspreadrpt == true)
                        {
                            rptprint1.Visible = true;
                            FpSpread1.Visible = true;
                        }
                        else
                        {
                            rptprint1.Visible = false;
                            FpSpread1.Visible = false;
                        }
                        if (cb_include.Checked == true)
                        {
                            piechart(charthash1);
                        }
                    }
                    else
                    {
                        rptprint1.Visible = false;
                        FpSpread1.Visible = false;
                        lbl_error.Text = "No Records Founds";
                        lbl_error.Visible = true;
                    }
                    #endregion
                }
                else
                {
                    rptprint1.Visible = false;
                    FpSpread1.Visible = false;
                    lbl_error.Text = "No Records Founds";
                    lbl_error.Visible = true;
                }
            }
            else
            {
                rptprint1.Visible = false;
                FpSpread1.Visible = false;
                lbl_error.Text = "Please Select Feedback Name";
                lbl_error.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            FpSpread1.Visible = false;
            rptprint1.Visible = false;
            lbl_error.Text = ex.ToString();
            lbl_error.Visible = true;
        }
    }
    protected void piechart(Hashtable chartvalue_hash1)
    {
        try
        {
            #region Subject Header chart
            if (chartvalue_hash1.Count > 0)
            {
                string chartname = "";
                if (rdbtype.SelectedIndex == 0)
                    chartname = "Faculty wise";
                else if (rdbtype.SelectedIndex == 1)
                    chartname = "Department wise";
                else if (rdbtype.SelectedIndex == 2)
                    chartname = "College wise";
                Chart1.Titles[0].Text = (chartname + "(" + Convert.ToString(ddl_feedbackformate6.SelectedItem.Value) + ")");
                if (chartvalue_hash1.Count > 0)
                {
                    Chart1.Series.Clear();
                    DataTable dtcol1 = new DataTable();
                    DataRow dtrow1;
                    dtcol1.Columns.Add("Pointvalue");
                    dtcol1.Columns.Add("Point");
                    Chart1.Series.Add("pie");
                    foreach (DictionaryEntry valuedet in chartvalue_hash1)
                    {
                        dtrow1 = dtcol1.NewRow();
                        string[] subjectvalue = Convert.ToString(valuedet.Key).Split('/');
                        string[] headervalpoint = Convert.ToString(valuedet.Value).Split('$');
                        foreach (string pointsval in headervalpoint)
                        {
                            string[] point = pointsval.Split('/');
                            if (point.Length > 1)
                            {
                                dtrow1["Pointvalue"] = Convert.ToString(point[0]);
                                dtrow1["Point"] = Convert.ToString(point[1]);
                            }
                        }
                        dtcol1.Rows.Add(dtrow1);
                    }
                    string[] XPointMember = new string[dtcol1.Rows.Count];
                    double[] YPointMember = new double[dtcol1.Rows.Count];
                    for (int count = 0; count < dtcol1.Rows.Count; count++)
                    {
                        XPointMember[count] = dtcol1.Rows[count]["Pointvalue"].ToString();
                        YPointMember[count] = Convert.ToDouble(dtcol1.Rows[count]["Point"]);
                    }
                    Chart1.Series[0].Points.DataBindXY(XPointMember, YPointMember);
                    Chart1.Series[0].BorderWidth = 10;
                    Chart1.Series[0].ChartType = SeriesChartType.Pie;
                    Chart1.RenderType = RenderType.ImageTag;
                    Chart1.ImageType = ChartImageType.Png;
                    Chart1.ImageStorageMode = ImageStorageMode.UseImageLocation;
                    Chart1.ImageLocation = Path.Combine("~/college/", "feedbackpiechart");
                    foreach (Series charts in Chart1.Series)
                    {
                        foreach (DataPoint point in charts.Points)
                        {
                            switch (point.AxisLabel)
                            {
                                case "Very Good": point.Color = Color.RoyalBlue; break;
                                case "Good": point.Color = Color.SaddleBrown; break;
                                case "Average": point.Color = Color.Tomato; break;
                                case "Poor": point.Color = Color.DarkGoldenrod; break;
                                case "Very Poor": point.Color = Color.YellowGreen; break;
                            }
                            point.Label = string.Format("{0:0} - {1}", point.YValues[0], point.AxisLabel);
                        }
                    }
                    Chart1.Legends[0].Enabled = true;
                    Chart1.Visible = true;
                    btnprintimag.Visible = true;
                }
            }
            else { Chart1.Visible = false; btnprintimag.Visible = false; }
            #endregion
        }
        catch (Exception ex)
        {
            Chart1.Visible = false; btnprintimag.Visible = false;
            lbl_error.Text = ex.ToString();
            lbl_error.Visible = true;
        }
    }
    protected void rdbtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbtype.SelectedIndex == 0)
        {
            txt_formate6batch.Enabled = true;
            ddlformate6_deptname.Enabled = true;
            txt_formate6sem.Enabled = true;
            txtstaffnameformat6.Enabled = true;
            txtsubjectnameformat6.Enabled = true;
        }
        else if (rdbtype.SelectedIndex == 1)
        {
            ddlformate6_deptname.Enabled = true;
            txt_formate6batch.Enabled = false;
            txt_formate6sem.Enabled = false;
            txtstaffnameformat6.Enabled = false;
            txtsubjectnameformat6.Enabled = false;
        }
        else if (rdbtype.SelectedIndex == 2)
        {
            txt_formate6batch.Enabled = false;
            ddlformate6_deptname.Enabled = false;
            txt_formate6sem.Enabled = false;
            txtstaffnameformat6.Enabled = false;
            txtsubjectnameformat6.Enabled = false;
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
    public string GetSelectedItemsValueAsString(CheckBoxList cblSelected, int sp)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value.Split('$')[sp]));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Value.Split('$')[sp]));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        lbl_norec1.Visible = false;
        try
        {
            string reportname = txtexcelname1.Text;
            if (reportname.ToString().Trim() != "")
            {
                if (FpSpread1.Visible == true)
                {
                    d2.printexcelreport(FpSpread1, reportname);
                }
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
    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            int batchcount = 0;
            int semcount = 0;
            string degree = "";
            string batch = "";
            string semester = "";
            string dptname = "Feedback report";
            string pagename = "Feedbackreport_consolidation.aspx";
            count++;
            degree = Convert.ToString(ddlformate6_deptname.SelectedItem.Text);
            string staffname = ""; int staffcount = 0;
            string subject = ""; int subjectcount = 0;
            for (int i = 0; i < cbl_formate6batch.Items.Count; i++)
            {
                if (cbl_formate6batch.Items[i].Selected == true)
                {
                    batchcount++;
                    batch = cbl_formate6batch.Items[i].Text;
                }
            }
            for (int i = 0; i < cbl_formate6sem.Items.Count; i++)
            {
                if (cbl_formate6sem.Items[i].Selected == true)
                {
                    semcount++;
                    semester = cbl_formate6sem.Items[i].Text;
                }
            }
            for (int i = 0; i < cbl_staffnameformat6.Items.Count; i++)
            {
                if (cbl_staffnameformat6.Items[i].Selected == true)
                {
                    staffcount++;
                    staffname = cbl_staffnameformat6.Items[i].Text;
                }
            }
            for (int i = 0; i < cbl_subjectnameformat6.Items.Count; i++)
            {
                if (cbl_subjectnameformat6.Items[i].Selected == true)
                {
                    subjectcount++;
                    subject = cbl_subjectnameformat6.Items[i].Text;
                }
            }
            dptname = " @ Department     : " + degree;
            if (batchcount == 1 && semcount == 1 && staffcount == 1 && subjectcount == 1)
            {
                dptname = dptname + "      Batch : " + batch + "        Semester : " + semester + "          @ Staff Name : " + staffname + "      Subject Name : " + subject;
            }
            else if (batchcount == 1 && semcount == 1 && staffcount == 1)
            {
                dptname += "      Batch : " + batch + "        Semester : " + semester + "          @ Staff Name : " + staffname;
            }
            else if (staffcount == 1 && subjectcount == 1)
            {
                dptname += "     @ Staff Name       : " + staffname + "      @ Subject Name : " + subject;
            }
            else if (batchcount == 1)
            {
                dptname += "     Batch    : " + batch;
            }
            else if (semcount == 1)
            {
                dptname += "     Semester : " + semester;
            }
            else if (staffcount == 1)
            {
                dptname += "     Staff Name      : " + staffname;
            }
            else if (subjectcount == 1)
            {
                dptname += "     Subject Name    : " + staffname;
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
}
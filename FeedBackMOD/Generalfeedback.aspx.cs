using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class FeedBackMOD_Generalfeedback : System.Web.UI.Page
{
    bool cellclk = false;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string query = "";
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
        lbl_norec1.Text = "";
        if (!IsPostBack)
        {
            bindcollege();
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            bindsec();
            bindfeedback();
        }
    }
    public void Cb_college_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(Cbl_college, Cb_college, Txt_college, "College");
    }
    public void Cbl_college_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(Cbl_college, Cb_college, Txt_college, "College");
    }
    public void cb_batch_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_batch, cb_batch, txt_batch, "Batch");
        BindDegree();
        bindbranch();
        bindsem();
        bindsec(); bindfeedback();
    }
    public void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_batch, cb_batch, txt_batch, "Batch");
        BindDegree();
        bindbranch();
        bindsem();
        bindsec(); bindfeedback();
    }
    public void cb_degree_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_degree, cb_degree, txt_degree, "Degree");
        bindbranch();
        bindsem();
        bindsec(); bindfeedback();
    }
    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_degree, cb_degree, txt_degree, "Degree");
        bindbranch();
        bindsem();
        bindsec(); bindfeedback();
    }
    public void cb_branch_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_branch, cb_branch, txt_branch, "Department");
        bindsem();
        bindsec(); bindfeedback();
    }
    public void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_branch, cb_branch, txt_branch, "Department");
        bindsem();
        bindsec(); bindfeedback();
    }
    public void cb_sem_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_sem, cb_sem, txt_sem, "Semester");
    }
    public void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_sem, cb_sem, txt_sem, "Semester");
    }
    public void cb_sec_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_sec, cb_sec, txt_sec, "Section");
    }
    public void cbl_sec_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_sec, cb_sec, txt_sec, "Section");
    }
    protected void ddl_Feedbackname_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public void bindcollege()
    {
        try
        {
            ds.Clear();
            Cbl_college.Items.Clear();
            string clgname = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(clgname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Cbl_college.DataSource = ds;
                Cbl_college.DataTextField = "collname";
                Cbl_college.DataValueField = "college_code";
                Cbl_college.DataBind();
            }
            if (Cbl_college.Items.Count > 0)
            {
                //for (int row = 0; row < Cbl_college.Items.Count; row++)
                //{
                Cbl_college.Items[0].Selected = true;
                Cb_college.Checked = false;
                //}
                Txt_college.Text = "College(1)";
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
            cbl_batch.Items.Clear();
            cb_batch.Checked = false;
            txt_batch.Text = "--Select--";
            string college_cd = rs.GetSelectedItemsValueAsString(Cbl_college);
            if (college_cd != "")
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
                    //for (int row = 0; row < cbl_batch.Items.Count; row++)
                    //{
                    cbl_batch.Items[0].Selected = true;
                    //cb_batch.Checked = true;
                    //}
                    txt_batch.Text = "Batch(1)";
                }
                else
                {
                    txt_batch.Text = "--Select--";
                }
            }
            BindDegree();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Feedback_anonymousisgender");
        }
    }
    public void BindDegree()
    {
        try
        {
            cbl_degree.Items.Clear();
            string college_cd = rs.GetSelectedItemsValueAsString(Cbl_college);
            if (college_cd.Trim() != "")
            {
                ds.Clear();
                query = "select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in ('" + college_cd + "')";
                ds = d2.select_method_wo_parameter(query, "Text");
                // ds = d2.BindDegree(singleuser, group_user, collegecode1, usercode);
                int count1 = ds.Tables[0].Rows.Count;
                if (count1 > 0)
                {
                    cbl_degree.DataSource = ds;
                    cbl_degree.DataTextField = "course_name";
                    cbl_degree.DataValueField = "course_id";
                    cbl_degree.DataBind();
                    if (cbl_degree.Items.Count > 0)
                    {
                        //for (int row = 0; row < cbl_degree.Items.Count; row++)
                        //{
                        cbl_degree.Items[0].Selected = true;
                        //}
                        //cb_degree.Checked = true;
                        txt_degree.Text = "Degree(1)";
                    }
                }
            }
            else
            {
                cb_degree.Checked = false;
                txt_degree.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Feedback_anonymousisgender");
        }
    }
    public void bindbranch()
    {
        try
        {
            cbl_branch.Items.Clear();
            string college_cd = rs.GetSelectedItemsValueAsString(Cbl_college);
            string course_id = rs.GetSelectedItemsValueAsString(cbl_degree);
            string query = "";
            if (course_id != "" && college_cd != "")
            {
                ds.Clear();
                query = " select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + course_id + "') and degree.college_code in ('" + college_cd + "')";
                ds = d2.select_method_wo_parameter(query, "Text");
                //   ds = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode1, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    if (cbl_branch.Items.Count > 0)
                    {
                        //for (int row = 0; row < cbl_branch.Items.Count; row++)
                        //{
                        cbl_branch.Items[0].Selected = true;
                        //}
                        //cb_branch.Checked = true;
                        txt_branch.Text = "Department(1)";
                    }
                }
            }
            else
            {
                cb_branch.Checked = false;
                txt_branch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Feedback_anonymousisgender");
        }
    }
    public void bindsem()
    {
        cbl_sem.Items.Clear();
        txt_sem.Text = "--Select--";
        ds.Clear();
        string branch = rs.GetSelectedItemsValueAsString(cbl_branch);
        string batch = rs.GetSelectedItemsValueAsString(cbl_batch);
        string college_cd = rs.GetSelectedItemsValueAsString(Cbl_college);
        if (branch.Trim() != "" && batch.Trim() != "")
        {
            string query = " select distinct  MAX( ndurations)as ndurations from ndegree where Degree_code in('" + branch + "') and  college_code in('" + college_cd + "') union select distinct  MAX(duration) as ndurations  from degree where Degree_Code in('" + branch + "') and college_code in('" + college_cd + "') ";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_sem.Items.Clear();
                string sem = Convert.ToString(ds.Tables[0].Rows[0]["ndurations"]);
                for (int j = 1; j <= Convert.ToInt32(sem); j++)
                {
                    cbl_sem.Items.Add(new System.Web.UI.WebControls.ListItem(j.ToString(), j.ToString()));
                    cbl_sem.Items[j - 1].Selected = true;
                    cb_sem.Checked = true;
                }
                txt_sem.Text = "Semester(" + sem + ")";
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
            string branchcode1 = rs.GetSelectedItemsValueAsString(cbl_branch);
            if (batch != "" && branchcode1 != "")
            {
                ds = d2.BindSectionDetail("'" + batch + "'", "'" + branchcode1 + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sec.DataSource = ds;
                    cbl_sec.DataTextField = "sections";
                    cbl_sec.DataValueField = "sections";
                    cbl_sec.DataBind();
                    if (cbl_sec.Items.Count > 0)
                    {
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
                    cbl_sec.Items.Add("Empty");
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
                cbl_sec.Items.Add("Empty");
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
        }
    }
    public void bindfeedback()
    {
        try
        {
            ds.Clear();
            string Batch_Year = rs.GetSelectedItemsValueAsString(cbl_batch);
            string college_cd = rs.GetSelectedItemsValueAsString(Cbl_college);
            string degree_code = rs.GetSelectedItemsValueAsString(cbl_branch);
            string semester = rs.GetSelectedItemsValueAsString(cbl_sem);
            string section = rs.GetSelectedItemsValueAsString(cbl_sec);
            if (section.Trim() != "")
                section = section + "','";
            ddl_Feedbackname.Items.Clear();
            //  query = "select distinct  FeedBackName  from CO_FeedBackMaster where   CollegeCode in ('" + college_cd + "')  and DegreeCode in ('" + degree_code + "') and Batch_Year in ('" + Batch_Year + "') and semester in ('" + semester + "') and Acadamic_Isgeneral='1'";
            query = "select distinct  FeedBackName  from CO_FeedBackMaster where   CollegeCode in ('" + college_cd + "')  and FeedBackType ='2'";
            //  if (section.Trim() != "")
            //  query += " and Section in ('" + section + "') ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            ddl_Feedbackname.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Feedbackname.DataSource = ds;
                ddl_Feedbackname.DataTextField = "FeedBackName";
                ddl_Feedbackname.DataValueField = "FeedBackName";
                ddl_Feedbackname.DataBind();
                ddl_Feedbackname.Items.Insert(0, "Select");
            }
            else
            {
                ddl_Feedbackname.Items.Clear();
                ddl_Feedbackname.Items.Insert(0, "Select");
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        Generalfeedback();
    }
    public void Generalfeedback()
    {
        try
        {
            if (ddl_Feedbackname.Text.Trim() != "Select")
            {
                Printcontrol1.Visible = false; lbl_error.Visible = false;
                string header = "S.No/Evaluation Name/Header Name/Questions";
                rs.Fpreadheaderbindmethod(header, FpSpread1, "True");
                string college_cd = rs.GetSelectedItemsValueAsString(Cbl_college);
                string Batch_Year = rs.GetSelectedItemsValueAsString(cbl_batch);
                string degree_code = rs.GetSelectedItemsValueAsString(cbl_branch);
                string semester = rs.GetSelectedItemsValueAsString(cbl_sem);
                string section = rs.GetSelectedItemsValueAsString(cbl_sec);

                if (college_cd.Trim() != "" && Batch_Year.Trim() != "" && degree_code.Trim() != "" && semester.Trim() != "")
                {
                    if (section.Trim() != "")
                    {
                        section = section + "','";
                    }
                    ds.Clear();
                    //  query = " select FeedBackMasterPK,isnull(InclueCommon,0)as FeedBackType,IsType_Individual from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "'";
                    query = " select FeedBackMasterPK,FeedBackType,IsType_Individual from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "'";
                    //  if (section.Trim() != "")
                    // {
                    //   query += " and section in ('" + section + "')";
                    //}
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(query, "text");
                    string FeedBackType = Convert.ToString(ds.Tables[0].Rows[0]["FeedBackType"]);
                    string feedbakpk = GetdatasetRowstring(ds, "FeedBackMasterPK");
                    string condition = ""; string condition1 = "";
                    //if (FeedBackType.Trim() == "0" || FeedBackType.Trim() == "False")
                    //{
                    //    FeedBackType = "0";
                    //    condition = "FeedbackUnicode";
                    //    condition1 = " and sf.App_No is not null and isnull(sf.FeedbackUnicode,'0')=0 ";
                    //}

                    if (FeedBackType.Trim() == "2" || FeedBackType.Trim() == "False")
                    {
                        FeedBackType = "2";
                        condition = "App_No";
                        condition1 = " and sf.App_No is not null and isnull(sf.FeedbackUnicode,'0')=0 ";
                    }
                    if (FeedBackType.Trim() == "1" || FeedBackType.Trim() == "True")
                    {
                        condition = "FeedbackUnicode";
                        FeedBackType = "1";
                        condition1 = " and sf.FeedbackUnicode is not null and isnull(sf.App_No,'0')=0 ";
                    }

                    ds.Clear();
                    //   query = " select distinct f.FeedBackName,TextVal,q.Question,q.HeaderCode,f.FeedBackMasterPK,q.QuestionMasterPK from CO_FeedBackMaster f,CO_StudFeedBack sf,CO_FeedBackQuestions fq ,CO_QuestionMaster q,TextValTable T where f.FeedBackMasterPK=sf.FeedBackMasterFK and q.QuestionMasterPK=fq.QuestionMasterFK and f.FeedBackMasterPK=fq.FeedBackMasterFK and t.TextCode=q.HeaderCode and f.Acadamic_Isgeneral='1' and f.DegreeCode in('" + degree_code + "') and f.CollegeCode in('" + college_cd + "') and f.semester in('" + semester + "')  and f.FeedBackMasterPK in('" + feedbakpk + "') and FeedBackType ='" + FeedBackType + "'  ";
                    query = " select distinct f.FeedBackName,TextVal,q.Question,q.HeaderCode,f.FeedBackMasterPK,q.QuestionMasterPK from CO_FeedBackMaster f,CO_StudFeedBack sf,CO_FeedBackQuestions fq ,CO_QuestionMaster q,TextValTable T where f.FeedBackMasterPK=sf.FeedBackMasterFK and q.QuestionMasterPK=fq.QuestionMasterFK and f.FeedBackMasterPK=fq.FeedBackMasterFK and t.TextCode=q.HeaderCode and f.CollegeCode in('" + college_cd + "')  and f.FeedBackMasterPK in('" + feedbakpk + "') and FeedBackType ='" + FeedBackType + "'  ";
                    //if (section.Trim() != "")
                    //{
                    //    query += " and f.Section in('" + section + "')";
                    //}
                    query += "  select distinct MarkType, MarkMasterPK   from CO_MarkMaster where CollegeCode in('" + college_cd + "')";
                    query += " select sum(Point)Point,COUNT(sf." + condition + ") noofstud ,sf.FeedBackMasterFK,sf.QuestionMasterFK,sf.MarkMasterPK from CO_StudFeedBack sf,CO_MarkMaster m,CO_FeedBackQuestions fq where sf.MarkMasterPK=m.MarkMasterPK and fq.FeedBackMasterFK=sf.FeedBackMasterFK and fq.QuestionMasterFK=sf.QuestionMasterFK and sf.FeedBackMasterFK in('" + feedbakpk + "') " + condition1 + "  group by sf.FeedBackMasterFK,sf.QuestionMasterFK,sf.MarkMasterPK ";
                    query += " select max(m.Point)maximum from CO_StudFeedBack sf,CO_MarkMaster m,CO_FeedBackQuestions fq where sf.MarkMasterPK=m.MarkMasterPK and fq.FeedBackMasterFK=sf.FeedBackMasterFK and fq.QuestionMasterFK=sf.QuestionMasterFK and sf.FeedBackMasterFK in('" + feedbakpk + "') ";
                    ds = d2.select_method_wo_parameter(query, "Text");
                    if (ds.Tables != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dr["MarkType"]);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(dr["MarkMasterPK"]);
                                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Percentage";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                            double total = 0; double point = 0; double sumofstud = 0; double totalsumofstud = 0; double maximummark = 0;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                FpSpread1.Sheets[0].Rows.Count++;
                                FpSpread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                                FpSpread1.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["FeedBackName"].ToString();
                                FpSpread1.Sheets[0].Cells[i, 2].Text = ds.Tables[0].Rows[i]["TextVal"].ToString();
                                FpSpread1.Sheets[0].Cells[i, 3].Text = ds.Tables[0].Rows[i]["Question"].ToString();
                                total = 0; totalsumofstud = 0;
                                for (int r = 4; r < FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2; r++)
                                {
                                    string markfk = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, r].Tag);
                                    ds.Tables[2].DefaultView.RowFilter = "  FeedBackMasterFK='" + Convert.ToString(ds.Tables[0].Rows[i]["FeedBackMasterPK"]) + "' and QuestionMasterFK='" + Convert.ToString(ds.Tables[0].Rows[i]["QuestionMasterPK"]) + "' and MarkMasterPK='" + markfk + "'";
                                    point = 0;
                                    DataView dv = new DataView();
                                    ds.Tables[2].DefaultView.RowFilter = "FeedBackMasterFK='" + Convert.ToString(ds.Tables[0].Rows[i]["FeedBackMasterPK"]) + "' and QuestionMasterFK='" + Convert.ToString(ds.Tables[0].Rows[i]["QuestionMasterPK"]) + "' and MarkMasterPK='" + markfk + "' ";
                                    dv = ds.Tables[2].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        double.TryParse(Convert.ToString(dv[0]["point"]), out point);
                                        double.TryParse(Convert.ToString(dv[0]["noofstud"]), out sumofstud);
                                        totalsumofstud += sumofstud;
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, r].Text = Convert.ToString(point);
                                    total += point;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, r].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, r].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, r].HorizontalAlign = HorizontalAlign.Center;
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(total);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                maximummark = 0;
                                double.TryParse(Convert.ToString(ds.Tables[3].Rows[0]["maximum"]), out maximummark);

                                double per = total / (totalsumofstud * maximummark) * 100;
                                string percent = "";
                                if (Convert.ToString(per).ToUpper() == "NAN")
                                {
                                    percent = " - ";
                                }
                                else
                                {
                                    percent = Convert.ToString(Math.Round(per));
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = percent;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 3].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 3].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                            }
                            FpSpread1.Visible = true;
                            rptprint1.Visible = true;
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alert1.Text = "No Records Found";
                            FpSpread1.Visible = false;
                            rptprint1.Visible = false;
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert1.Text = "No Records Found";
                        FpSpread1.Visible = false;
                        rptprint1.Visible = false;
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "Please Select All Fields";
                    FpSpread1.Visible = false;
                    rptprint1.Visible = false;
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "Please Select Feedback";
                FpSpread1.Visible = false;
                rptprint1.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lbl_error.Text = ex.ToString();
            lbl_error.Visible = true;
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
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
            string pagename = "Generalfeedback.aspx";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    count++;
                    degree = cbl_degree.Items[i].Text;
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

            if (count == 1)
            {
                dptname = dptname + "@ Course     : " + degree;
            }
            else if (batchcount == 1 && semcount == 1)
            {
                dptname = dptname + '@' + " Batch  : " + batch + "             Semester : " + semester;
            }
            else if (batchcount == 1)
            {
                dptname = dptname + '@' + " Batch       : " + batch;
            }
            else if (semcount == 1)
            {
                dptname = dptname + '@' + " Semester : " + semester;
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
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
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
}
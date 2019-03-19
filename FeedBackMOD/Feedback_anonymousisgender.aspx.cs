using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using System.Collections;
public partial class FeedBackMod_Feedback_anonymousisgender : System.Web.UI.Page
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
    Hashtable charthash = new Hashtable();
    Hashtable charthash1 = new Hashtable();
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
    public void Cb_Subject_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(Cbl_Subject, Cb_Subject, Txt_Subject, "Subject");
    }
    public void Cbl_Subject_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(Cbl_Subject, Cb_Subject, Txt_Subject, "Subject");
    }
    protected void ddl_Feedbackname_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsubject();
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
            query = "select distinct  FeedBackName  from CO_FeedBackMaster where   CollegeCode in ('" + college_cd + "')  and DegreeCode in ('" + degree_code + "') and Batch_Year in ('" + Batch_Year + "') and semester in ('" + semester + "') ";
            if (section.Trim() != "")
                query += " and Section in ('" + section + "') ";
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
    protected void bindsubject()
    {
        if (ddl_Feedbackname.Items.Count > 0)
        {
            if (ddl_Feedbackname.SelectedItem.Text != "Select")
            {
                Txt_Subject.Text = "--Select--";
                string college_cd = rs.GetSelectedItemsValueAsString(Cbl_college);
                string Batch_Year = rs.GetSelectedItemsValueAsString(cbl_batch);
                string degree_code = rs.GetSelectedItemsValueAsString(cbl_branch);
                string semester = rs.GetSelectedItemsValueAsString(cbl_sem);
                string section = rs.GetSelectedItemsValueAsString(cbl_sec);
                if (section.Trim() != "")
                {
                    section = section + "','";
                }
                if (degree_code.Trim() != "" && semester.Trim() != "" && Batch_Year.Trim() != "")
                {
                    string q1 = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + degree_code + "') and semester in ('" + semester + "') and Batch_Year in('" + Batch_Year + "')";
                    if (section.Trim() != "")
                    {
                        q1 += " and section in ('" + section + "')";
                    }
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string feedbakpk = GetdatasetRowstring(ds, "FeedBackMasterPK");
                        query = "select distinct s.subject_name,s.subject_no from subject s,CO_StudFeedBack sf where s.subject_no=sf.SubjectNo and sf.FeedBackMasterFK in('" + feedbakpk + "')";
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
    protected void btn_go_Click(object sender, EventArgs e)
    {
        format1();
    }
    public void format1()
    {
        try
        {
            lbl_error.Visible = false; Printcontrol1.Visible = false;
            string college_cd = rs.GetSelectedItemsValueAsString(Cbl_college);
            string Batch_Year = rs.GetSelectedItemsValueAsString(cbl_batch);
            string degree_code = rs.GetSelectedItemsValueAsString(cbl_branch);
            string semester = rs.GetSelectedItemsValueAsString(cbl_sem);
            string section = rs.GetSelectedItemsValueAsString(cbl_sec);
            string subjectno = rs.GetSelectedItemsValueAsString(Cbl_Subject);
            if (section.Trim() != "")
            {
                section = section + "','";
            }
            ds.Clear();
            if (ddl_Feedbackname.SelectedItem.Text != "Select")
            {
                string q1 = " select FeedBackMasterPK,isnull(InclueCommon,0)as FeedBackType from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + degree_code + "') and semester in ('" + semester + "') and Batch_Year in('" + Batch_Year + "') ";
                if (section.Trim() != "")
                {
                    q1 += " and section in ('" + section + "')";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string FeedBackType = Convert.ToString(ds.Tables[0].Rows[0]["FeedBackType"]);
                    string selqry = "";
                    string feedbakpk = GetdatasetRowstring(ds, "FeedBackMasterPK");
                    if (FeedBackType.Trim() == "0" || FeedBackType.Trim() == "False")
                    {
                        FeedBackType = "0";
                        string header = "S.No/Staff Name/Subject Code/Subject Name/Department";
                        rs.Fpreadheaderbindmethod(header, FpSpread1, "true");
                        ds.Clear();
                        selqry = "  select COUNT(distinct App_No)as studentcount,(convert(varchar(10), cf.Batch_Year)+'-'+c.Course_Name+'-'+ dt.dept_acronym+'-'+convert(varchar(10), cf.Semester)+'-'+cf.Section ) as department,Staff_Name,f.SubjectNo,f.StaffApplNo,t.staff_code from CO_StudFeedBack f,CO_MarkMaster M,CO_QuestionMaster Q ,CO_FeedBackMaster CF ,staff_appl_master A,staffmaster T,Degree d, Department dt,Course c  where a.appl_id=f.StaffApplNo and a.appl_no=t.appl_no and d.Degree_Code =cf.degreecode and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and f.MarkMasterPK =M.MarkMasterPK and Q.QuestionMasterPK =f.QuestionMasterFK and cf.FeedBackMasterPK =f.FeedBackMasterFK and SubjectNo in ('" + subjectno + "')  and cf.CollegeCode in ('" + college_cd + "') and cf.Batch_Year in ('" + Batch_Year + "') and cf.degreecode in ('" + degree_code + "') and cf.semester in ('" + semester + "') and cf.FeedBackType ='" + FeedBackType + "' and cf.FeedBackMasterPK in ('" + feedbakpk + "') and f.App_No is not null and isnull(f.FeedbackUnicode,'0')=0  ";
                        if (section != "")
                        {
                            selqry = selqry + "   and cf.Section in ('" + section + "') group by HeaderCode,SubjectNo,StaffApplNo ,convert(varchar(10), cf.Batch_Year)+'-'+c.Course_Name+'-'+ dt.dept_acronym+'-'+convert(varchar(10), cf.Semester)+'-'+cf.Section ,f.SubjectNo,f.StaffApplNo,Staff_Name,t.staff_code  order by f.StaffApplNo";
                        }
                        else
                        {
                            selqry = selqry + " group by SubjectNo,StaffApplNo ,convert(varchar(10), cf.Batch_Year)+'-'+c.Course_Name+'-'+ dt.dept_acronym+'-'+convert(varchar(10), cf.Semester)+'-'+cf.Section ,f.SubjectNo,f.StaffApplNo,Staff_Name,t.staff_code  order by f.StaffApplNo";
                        }
                        //1
                        selqry = selqry + " select distinct (select textval from textvaltable where TextCode= q.HeaderCode)HeaderName, q.HeaderCode from CO_QuestionMaster q,CO_FeedBackQuestions fq where q.QuestionMasterPK=fq.QuestionMasterFK and FeedBackMasterFK in('" + feedbakpk + "') and CollegeCode in('" + college_cd + "')";
                        //2
                        selqry = selqry + " select COUNT(fq.QuestionMasterFK)questioncount,q.HeaderCode,1 dummyrow from CO_FeedBackMaster f,CO_FeedBackQuestions FQ,CO_QuestionMaster Q where  Q.QuestionMasterPK=fq.QuestionMasterFK and fq.FeedBackMasterFK =f.FeedBackMasterPK and FQ.FeedBackMasterFK in('" + feedbakpk + "')  group by q.HeaderCode ";
                        //3
                        selqry = selqry + " select s.subject_code,s.subject_no,s.subject_name from subject s,syllabus_master y where s.syll_code=y.syll_code and y.degree_code in('" + degree_code + "') and y.Batch_Year in('" + Batch_Year + "')";
                        //4
                        selqry = selqry + " select SUM(point) as sumofpoint,HeaderCode,SubjectNo,StaffApplNo from CO_StudFeedBack f,CO_MarkMaster M,CO_QuestionMaster Q ,CO_FeedBackMaster CF where f.MarkMasterPK =M.MarkMasterPK and Q.QuestionMasterPK =f.QuestionMasterFK and cf.FeedBackMasterPK =f.FeedBackMasterFK and SubjectNo in('" + subjectno + "')  and cf.CollegeCode in ('" + college_cd + "') and cf.Batch_Year in ('" + Batch_Year + "') and cf.degreecode in ('" + degree_code + "') and cf.semester in ('" + semester + "') and cf.FeedBackType ='" + FeedBackType + "' and cf.FeedBackMasterPK in ('" + feedbakpk + "') and f.App_No is not null and isnull(f.FeedbackUnicode,'0')=0  group by HeaderCode,SubjectNo,StaffApplNo ";
                    }
                    if (FeedBackType.Trim() == "1" || FeedBackType.Trim() == "True")
                    {
                        FeedBackType = "1";
                        string header = "S.No/Staff Name/Subject Code/Subject Name/Department";
                        rs.Fpreadheaderbindmethod(header, FpSpread1, "true");
                        ds.Clear();
                        selqry = " select SUM(point) as count,COUNT(distinct FeedbackUnicode)as studentcount,(convert(varchar(10), cf.Batch_Year)+'-'+c.Course_Name+'-'+ dt.dept_acronym+'-'+convert(varchar(10), cf.Semester)+'-'+cf.Section ) as department,Staff_Name,f.SubjectNo,f.StaffApplNo,t.staff_code from CO_StudFeedBack f,CO_MarkMaster M,CO_QuestionMaster Q ,CO_FeedBackMaster CF ,staff_appl_master A,staffmaster T,Degree d, Department dt,Course c  where a.appl_id=f.StaffApplNo and a.appl_no=t.appl_no and d.Degree_Code =cf.degreecode and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and f.MarkMasterPK =M.MarkMasterPK and Q.QuestionMasterPK =f.QuestionMasterFK and cf.FeedBackMasterPK =f.FeedBackMasterFK and SubjectNo in('" + subjectno + "')  and cf.CollegeCode in ('" + college_cd + "') and cf.Batch_Year in ('" + Batch_Year + "') and cf.degreecode in ('" + degree_code + "') and cf.semester in ('" + semester + "') and cf.FeedBackType ='" + FeedBackType + "' and cf.FeedBackMasterPK in ('" + feedbakpk + "') and f.FeedbackUnicode is not null and isnull(f.App_No,'0')=0  ";
                        if (section != "")
                        {
                            selqry = selqry + "   and cf.Section in ('" + section + "') group by SubjectNo,StaffApplNo ,convert(varchar(10), cf.Batch_Year)+'-'+c.Course_Name+'-'+ dt.dept_acronym+'-'+convert(varchar(10), cf.Semester)+'-'+cf.Section ,f.SubjectNo,f.StaffApplNo,Staff_Name,t.staff_code  order by f.StaffApplNo";
                        }
                        else
                        {
                            selqry = selqry + " group by SubjectNo,StaffApplNo ,convert(varchar(10), cf.Batch_Year)+'-'+c.Course_Name+'-'+ dt.dept_acronym+'-'+convert(varchar(10), cf.Semester)+'-'+cf.Section ,f.SubjectNo,f.StaffApplNo,Staff_Name,t.staff_code  order by f.StaffApplNo";
                        }
                        //1
                        selqry = selqry + " select distinct (select textval from textvaltable where TextCode= q.HeaderCode)HeaderName, q.HeaderCode from CO_QuestionMaster q,CO_FeedBackQuestions fq where q.QuestionMasterPK=fq.QuestionMasterFK and FeedBackMasterFK in('" + feedbakpk + "') and CollegeCode in('" + college_cd + "')";
                        //2
                        selqry = selqry + " select COUNT(fq.QuestionMasterFK)questioncount,q.HeaderCode,1 dummyrow from CO_FeedBackMaster f,CO_FeedBackQuestions FQ,CO_QuestionMaster Q where  Q.QuestionMasterPK=fq.QuestionMasterFK and fq.FeedBackMasterFK =f.FeedBackMasterPK and FQ.FeedBackMasterFK in('" + feedbakpk + "')  group by q.HeaderCode ";
                        //3
                        selqry = selqry + " select s.subject_code,s.subject_no,s.subject_name from subject s,syllabus_master y where s.syll_code=y.syll_code and y.degree_code in('" + degree_code + "') and y.Batch_Year in('" + Batch_Year + "')";
                        //4
                        selqry = selqry + " select SUM(point) as sumofpoint,HeaderCode,SubjectNo,StaffApplNo from CO_StudFeedBack f,CO_MarkMaster M,CO_QuestionMaster Q ,CO_FeedBackMaster CF where f.MarkMasterPK =M.MarkMasterPK and Q.QuestionMasterPK =f.QuestionMasterFK and cf.FeedBackMasterPK =f.FeedBackMasterFK and SubjectNo in('" + subjectno + "')  and cf.CollegeCode in ('" + college_cd + "') and cf.Batch_Year in ('" + Batch_Year + "') and cf.degreecode in ('" + degree_code + "') and cf.semester in ('" + semester + "') and cf.FeedBackType ='" + FeedBackType + "' and cf.FeedBackMasterPK in ('" + feedbakpk + "') and f.FeedbackUnicode is not null and isnull(f.App_No,'0')=0  group by HeaderCode,SubjectNo,StaffApplNo ";
                    }
                    #region MyRegion
                    /*-----------Login based 
select COUNT(distinct app_no)as count,q.HeaderCode,s.StaffApplNo,SubjectNo   from CO_FeedBackMaster f ,CO_StudFeedBack s,CO_FeedBackQuestions FQ,CO_QuestionMaster Q where f.FeedBackMasterPK =s.FeedBackMasterFK and Q.QuestionMasterPK=fq.QuestionMasterFK and fq.FeedBackMasterFK =f.FeedBackMasterPK and FQ.FeedBackMasterFK =s.FeedBackMasterFK  and s.FeedBackMasterFK in('11')
and StaffApplNo =10 and SubjectNo =594 and HeaderCode =3331
 group by q.HeaderCode,s.StaffApplNo ,SubjectNo 
--2
select COUNT(fq.QuestionMasterFK),q.HeaderCode from CO_FeedBackMaster f,CO_FeedBackQuestions FQ,CO_QuestionMaster Q where  Q.QuestionMasterPK=fq.QuestionMasterFK and fq.FeedBackMasterFK =f.FeedBackMasterPK and FQ.FeedBackMasterFK in('11') and HeaderCode =3331  group by q.HeaderCode 
 --1
  select SUM(point) as count,HeaderCode,App_No,SubjectNo,StaffApplNo from CO_StudFeedBack f,CO_MarkMaster M,CO_QuestionMaster Q ,CO_FeedBackMaster CF where f.MarkMasterPK =M.MarkMasterPK and Q.QuestionMasterPK =f.QuestionMasterFK and cf.FeedBackMasterPK =f.FeedBackMasterFK and FeedBackMasterFK =11 
and StaffApplNo =10 and SubjectNo =594 and HeaderCode =3331
   group by HeaderCode,App_No,SubjectNo,StaffApplNo 
  
  ---------------------------Anoyn
  
  select COUNT(distinct FeedbackUnicode)as count,q.HeaderCode,s.StaffApplNo,SubjectNo   from CO_FeedBackMaster f ,CO_StudFeedBack s,CO_FeedBackQuestions FQ,CO_QuestionMaster Q where f.FeedBackMasterPK =s.FeedBackMasterFK and Q.QuestionMasterPK=fq.QuestionMasterFK and fq.FeedBackMasterFK =f.FeedBackMasterPK and FQ.FeedBackMasterFK =s.FeedBackMasterFK  and s.FeedBackMasterFK in('11') group by q.HeaderCode,s.StaffApplNo ,SubjectNo 
select COUNT(fq.QuestionMasterFK),q.HeaderCode from CO_FeedBackMaster f,CO_FeedBackQuestions FQ,CO_QuestionMaster Q where  Q.QuestionMasterPK=fq.QuestionMasterFK and fq.FeedBackMasterFK =f.FeedBackMasterPK and FQ.FeedBackMasterFK in('11') group by q.HeaderCode 
 
  select SUM(point) as count,HeaderCode,FeedbackUnicode,SubjectNo,StaffApplNo from CO_StudFeedBack f,CO_MarkMaster M,CO_QuestionMaster Q ,CO_FeedBackMaster CF where f.MarkMasterPK =M.MarkMasterPK and Q.QuestionMasterPK =f.QuestionMasterFK and cf.FeedBackMasterPK =f.FeedBackMasterFK and FeedBackMasterFK =11 group by HeaderCode,FeedbackUnicode,SubjectNo,StaffApplNo */
                    #endregion
                    charthash.Clear(); charthash1.Clear(); DataTable dtcol = new DataTable();
                    dtcol.Columns.Add("Subject");
                    DataRow dtrow;
                    if (selqry.Trim() != "")
                    {
                        ds = d2.select_method_wo_parameter(selqry, "Text");
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                FpSpread1.Columns.Count++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Text = Convert.ToString(dr["HeaderName"]);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Tag = Convert.ToString(dr["HeaderCode"]);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                            FpSpread1.Columns.Count++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Text = "Total";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                        double noofquestion = 0; double totalstudentpoint = 0; double studentpoint = 0; double totalstudentcount = 0;
                        if (ds.Tables != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    int k = 0; string staffname = "";
                                    FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                                    cb.AutoPostBack = true;
                                    FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                                    cb1.AutoPostBack = false;
                                    foreach (DataRow dr in ds.Tables[0].Rows)
                                    {
                                        k++; totalstudentpoint = 0; totalstudentcount = 0;
                                        FpSpread1.Sheets[0].RowCount++;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(k);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dr["Staff_Name"].ToString(); FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = dr["staff_code"].ToString();
                                        staffname = dr["staff_name"].ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                        DataView dv = new DataView();
                                        if (ds.Tables[3].Rows.Count > 0)
                                        {
                                            ds.Tables[3].DefaultView.RowFilter = " subject_no='" + dr["SubjectNo"] + "'";
                                            dv = ds.Tables[3].DefaultView;
                                            if (dv.Count > 0)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dv[0]["Subject_Code"].ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dv[0]["Subject_Name"].ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;


                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;


                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = dr["department"].ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;

                                                for (int col = 5; col < FpSpread1.Columns.Count - 1; col++)
                                                {
                                                    DataView dv1 = new DataView(); DataView dv2 = new DataView();
                                                    string headercode = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                                                    string headername = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Text);
                                                    ds.Tables[2].DefaultView.RowFilter = " HeaderCode='" + headercode + "'";
                                                    dv1 = ds.Tables[2].DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        noofquestion = 0;
                                                        double.TryParse(Convert.ToString(dv1[0]["questioncount"]), out noofquestion);
                                                        studentpoint = 0;
                                                        ds.Tables[4].DefaultView.RowFilter = " SubjectNo='" + dr["SubjectNo"] + "' and StaffApplNo='" + dr["StaffApplNo"] + "' and HeaderCode='" + headercode + "'";
                                                        dv2 = ds.Tables[4].DefaultView;
                                                        if (dv2.Count > 0)
                                                        {
                                                            double.TryParse(Convert.ToString(dr["studentcount"]), out totalstudentcount);
                                                            double.TryParse(Convert.ToString(dv2[0]["sumofpoint"]), out studentpoint);
                                                            double point = studentpoint / noofquestion;
                                                            point = point / totalstudentcount;
                                                            totalstudentpoint += Math.Round(point, 1);

                                                            //double percent = Math.Round(point, 1) / 4 * 100;
                                                            //if (percent % 2 == 0 && percent != 100)
                                                            //{
                                                            //    percent += 15;
                                                            //}
                                                            //else if(percent != 100)
                                                            //{
                                                            //    percent += 10;
                                                            //}
                                                            //totalstudentpoint += Math.Round(percent, 1);
                                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(Math.Round(percent, 1));
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(Math.Round(point, 1));
                                                            if (!charthash1.Contains(Convert.ToString(dv[0]["Subject_Code"]) + "/" + Convert.ToString(dv[0]["Subject_Name"])))
                                                            {
                                                                charthash1.Add(Convert.ToString(dv[0]["Subject_Code"]) + "/" + Convert.ToString(dv[0]["Subject_Name"]), headername + "/" + Convert.ToString(Math.Round(point, 1)));//+ "$" + Convert.ToString(headercode)
                                                            }
                                                            else
                                                            {
                                                                string val = Convert.ToString(charthash1[Convert.ToString(dv[0]["Subject_Code"]) + "/" + Convert.ToString(dv[0]["Subject_Name"])]);

                                                                val += "$" + headername + "/" + Convert.ToString(Math.Round(point, 1));
                                                                charthash1.Remove(Convert.ToString(dv[0]["Subject_Code"]) + "/" + Convert.ToString(dv[0]["Subject_Name"]));
                                                                charthash1.Add(Convert.ToString(dv[0]["Subject_Code"]) + "/" + Convert.ToString(dv[0]["Subject_Name"]), val);
                                                            }
                                                        }
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                                double totalpoint = totalstudentpoint / ds.Tables[1].Rows.Count;// totalstudentcount;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = Convert.ToString(Math.Round(totalpoint, 1));
                                                if (!charthash.Contains(Convert.ToString(dv[0]["Subject_Code"]) + "/" + Convert.ToString(dv[0]["Subject_Name"])))
                                                {
                                                    charthash.Add(Convert.ToString(dv[0]["Subject_Code"]) + "/" + Convert.ToString(dv[0]["Subject_Name"]), Convert.ToString(Math.Round(totalpoint, 1)));
                                                }
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                                            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                                            FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                                            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                            FpSpread1.Visible = true;
                                            rptprint1.Visible = true;
                                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                        }
                                    }
                                    if (cb_include.Checked == true)
                                    {
                                        Feedbackisgenderchart(charthash, charthash1);
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
                lbl_alert1.Text = "Please Select Feedback Name";
                FpSpread1.Visible = false;
                rptprint1.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
            d2.sendErrorMail(ex, collegecode1, "Feedback_anonymousisgender");
        }
    }
    protected void Feedbackisgenderchart(Hashtable chartvalue_hash, Hashtable chartvalue_hash1)
    {
        try
        {
            if (rb_Subjectswise.Checked == true)
            {
                if (chartvalue_hash.Count > 0)
                {
                    subjectwise_chart.Titles[0].Text = ("Subject Wise Chart (" + Convert.ToString(ddl_Feedbackname.SelectedItem.Value) + ")");
                    int chartwidth = 20;
                    if (chartvalue_hash.Count > 0)
                    {
                        subjectwise_chart.Series.Clear();
                        DataTable dtcol = new DataTable();
                        DataRow dtrow;
                        dtrow = dtcol.NewRow();
                        foreach (DictionaryEntry valuedet in chartvalue_hash)
                        {
                            string[] subjectvalue = Convert.ToString(valuedet.Key).Split('/');
                            subjectwise_chart.Series.Add(subjectvalue[0].Trim() + '-' + subjectvalue[1]);
                            dtcol.Columns.Add(subjectvalue[0].Trim());
                            //dtrow = dtcol.NewRow();
                            dtrow[subjectvalue[0].Trim()] = Convert.ToString(valuedet.Value);
                            //dtcol.Rows.Add(dtrow);
                        }
                        dtcol.Rows.Add(dtrow);
                        subjectwise_chart.RenderType = RenderType.ImageTag;
                        subjectwise_chart.ImageType = ChartImageType.Png;
                        subjectwise_chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
                        subjectwise_chart.ImageLocation = Path.Combine("~/college/", "feedbackchart");
                        if (dtcol.Columns.Count > 0)
                        {
                            for (int r = 0; r < dtcol.Rows.Count; r++)
                            {
                                for (int c = 0; c < dtcol.Columns.Count; c++)
                                {
                                    subjectwise_chart.Series[r].Points.AddXY(dtcol.Columns[c].ToString(), dtcol.Rows[r][c].ToString());
                                    subjectwise_chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                    subjectwise_chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                                    subjectwise_chart.Series[r].IsValueShownAsLabel = true;
                                    subjectwise_chart.Series[r].IsXValueIndexed = true;
                                    if (ddl_charttype.SelectedItem.Value == "1")
                                    {
                                        subjectwise_chart.Series[r].ChartType = SeriesChartType.Line;
                                    }
                                    else
                                        subjectwise_chart.Series[r].ChartType = SeriesChartType.Column;
                                    subjectwise_chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                    subjectwise_chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                    chartwidth += 40;
                                }
                            }
                        }
                        #region its working
                        //foreach (DictionaryEntry valuedet in charthash)
                        //{
                        //string[] subjectvalue = Convert.ToString(valuedet.Key).Split('/');
                        //subjectwise_chart.Series.Add(subjectvalue[0].Trim() + '-' + subjectvalue[1]);
                        //subjectwise_chart.Series[row].Points.AddXY(Convert.ToString(subjectvalue[0]), Convert.ToString(valuedet.Value));
                        //subjectwise_chart.Series[i].IsValueShownAsLabel = true;
                        //subjectwise_chart.Series[i].IsXValueIndexed = true;
                        //subjectwise_chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                        //subjectwise_chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                        //if (ddl_charttype.SelectedItem.Value == "1")
                        //{
                        //    subjectwise_chart.Series[i].ChartType = SeriesChartType.Line;
                        //}
                        //subjectwise_chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                        //subjectwise_chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                        //chartwidth += 40;
                        //i++;
                        // }
                        #endregion
                    }
                    subjectwise_chart.Legends[0].Enabled = true;
                    if (chartwidth < 500)
                        chartwidth = 500;
                    subjectwise_chart.Width = chartwidth;
                    subjectwise_chart.Visible = true;
                }
            }
            else
            {
                #region Subject Header chart
                if (chartvalue_hash1.Count > 0)
                {
                    subjectheaderwise_chart.Titles[0].Text = ("Subject Wise Chart (" + Convert.ToString(ddl_Feedbackname.SelectedItem.Value) + ")");
                    int chartwidth = 20;
                    if (chartvalue_hash1.Count > 0)
                    {
                        subjectheaderwise_chart.Series.Clear();
                        DataTable dtcol1 = new DataTable();
                        DataRow dtrow1;
                        bool chk = false;
                        foreach (DictionaryEntry valuedet in chartvalue_hash1)
                        {
                            string[] subjectvalue = Convert.ToString(valuedet.Key).Split('/');
                            subjectheaderwise_chart.Series.Add(subjectvalue[0].Trim() + '-' + subjectvalue[1]);
                            string[] headervalpoint = Convert.ToString(valuedet.Value).Split('$');
                            if (chk == false)
                            {
                                foreach (string pointsval in headervalpoint)
                                {
                                    string[] point = pointsval.Split('/');
                                    if (point.Length > 1)
                                    {
                                        dtcol1.Columns.Add(Convert.ToString(point[0]));
                                    } chk = true;
                                }
                            }
                        }
                        foreach (DictionaryEntry valuedet in chartvalue_hash1)
                        {
                            dtrow1 = dtcol1.NewRow();
                            string[] headervalpoint = Convert.ToString(valuedet.Value).Split('$');
                            foreach (string pointsval in headervalpoint)
                            {
                                string[] point = pointsval.Split('/');
                                if (point.Length > 1)
                                {
                                    dtrow1[point[0]] = Convert.ToString(point[1]);
                                }
                            }
                            dtcol1.Rows.Add(dtrow1);
                        }
                        subjectheaderwise_chart.RenderType = RenderType.ImageTag;
                        subjectheaderwise_chart.ImageType = ChartImageType.Png;
                        subjectheaderwise_chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
                        subjectheaderwise_chart.ImageLocation = Path.Combine("~/college/", "feedbackchart");
                        if (dtcol1.Columns.Count > 0)
                        {
                            for (int r = 0; r < dtcol1.Rows.Count; r++)
                            {
                                for (int c = 0; c < dtcol1.Columns.Count; c++)
                                {
                                    subjectheaderwise_chart.Series[r].Points.AddXY(dtcol1.Columns[c].ToString(), dtcol1.Rows[r][c].ToString());
                                    subjectheaderwise_chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                    subjectheaderwise_chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                                    subjectheaderwise_chart.Series[r].IsValueShownAsLabel = true;
                                    subjectheaderwise_chart.Series[r].IsXValueIndexed = true;
                                    if (ddl_charttype.SelectedItem.Value == "1")
                                    {
                                        subjectheaderwise_chart.Series[r].ChartType = SeriesChartType.Line;
                                    }
                                    else
                                        subjectheaderwise_chart.Series[r].ChartType = SeriesChartType.Column;
                                    subjectheaderwise_chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                    subjectheaderwise_chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                    chartwidth += 40;
                                }
                            }
                        }
                    }
                    subjectheaderwise_chart.Legends[0].Enabled = true;
                    if (chartwidth < 500)
                        chartwidth = 500;
                    subjectheaderwise_chart.Width = chartwidth;
                    subjectheaderwise_chart.Visible = true;
                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
            subjectwise_chart.Visible = false; subjectheaderwise_chart.Visible = false; rptprint1.Visible = false;
            d2.sendErrorMail(ex, collegecode1, "Feedback_anonymousisgender");
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
            int count1 = 0;
            int batchcount = 0;
            int semcount = 0;
            string degree = "";
            string branch = "";
            string sub = "";
            string batch = "";
            string semester = "";
            string dptname = "Feedback report";
            string pagename = "Feedback_report.aspx";
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

            for (int i = 0; i < Cbl_Subject.Items.Count; i++)
            {
                if (Cbl_Subject.Items[i].Selected == true)
                {
                    sub = Cbl_Subject.Items[i].Text;
                }
            }
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    branch = cbl_branch.Items[i].Text;
                }
            }
            if (count == 1)
            {
                dptname = dptname + "@ Course     : " + degree + "      " + branch;
            }
            if (count1 == 1 && batchcount == 1 && semcount == 1)
            {
                dptname = dptname + '@' + " Batch       : " + batch + "             Subject  : " + sub + "           Semester : " + semester + "";
            }
            else if (count1 == 1 && batchcount == 1)
            {
                dptname = dptname + '@' + " Batch  : " + batch + "             Subject  : " + sub;
            }
            else if (count1 == 1 && semcount == 1)
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
    protected void rb_Subjectswise_CheckedChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        subjectwise_chart.Visible = false;
        subjectheaderwise_chart.Visible = false; rptprint1.Visible = false;
    }
    protected void rb_Subjectheaderswise_CheckedChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        subjectwise_chart.Visible = false;
        subjectheaderwise_chart.Visible = false;
        rptprint1.Visible = false;
    }
}
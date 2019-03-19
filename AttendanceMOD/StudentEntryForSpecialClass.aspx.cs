using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;

public partial class StudentEntryForSpecialClass : System.Web.UI.Page
{
    #region declaration

    Hashtable hat = new Hashtable();

    Hashtable htSubjectType = new Hashtable();
    DataSet ds_attndmaster = new DataSet();
    DataSet ds1 = new DataSet();
    ReuasableMethods reuse = new ReuasableMethods();
    DAccess2 d2 = new DAccess2();
    DAccess2 dacces2 = new DAccess2();
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();

    string qry = string.Empty;
    string strsec = string.Empty;
    string single_user = string.Empty;
    string group_code = string.Empty;
    string no_of_hrs = string.Empty;
    string sch_order = string.Empty;
    string srt_day = string.Empty;
    string startdate = string.Empty;
    string no_days = string.Empty;
    string date_txt = string.Empty;
    string sem_sched = string.Empty;
    string subject_no = string.Empty;
    string Att_dcolumn = string.Empty;
    string Att_strqueryst = string.Empty;
    string regularflag = string.Empty;
    string genderflag = string.Empty;
    string staffcode = string.Empty;
    string Att_mark = string.Empty;
    string roll_indiv = string.Empty;
    string usercode = string.Empty;
    string branch = string.Empty;
    string batch = string.Empty;
    string college = string.Empty;
    string sec = string.Empty;
    string degree = string.Empty;
    string sem = string.Empty;
    string subject = string.Empty;
    string qrySpecialHourDate = string.Empty;
    string specialHourDate = string.Empty;

    InsproDirectAccess dir = new InsproDirectAccess();

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        BindCollege();
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        group_code = Session["group_code"].ToString();
        if (group_code.Contains(';'))
        {
            string[] group_semi = group_code.Split(';');
            group_code = group_semi[0].ToString();
        }
        if (!IsPostBack)
        {
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            Session["Sex"] = "0";
            Session["flag"] = "-1";
            string grouporusercode = string.Empty;
            string userOrGroupCode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0] + "'";
                userOrGroupCode = Convert.ToString(Session["group_code"]).Trim().Split(';')[0];
            }
            else
            {
                grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                userOrGroupCode = Convert.ToString(Session["usercode"]).Trim();
            }
            BindCollege();
            BindBatch();
            Bind_Degree(Convert.ToString(ddlCollege.SelectedValue).Trim(), userOrGroupCode);
            Bind_Dept(Convert.ToString(ddldegree.SelectedValue).Trim(), Convert.ToString(ddlCollege.SelectedValue).Trim(), userOrGroupCode);
            bindsem();
            BindSectionDetail();
            BindSubject();
            BindSpecialHourDate();
            BindSpecialHourStaffList();
            BindSpecialHourTime();
        }
    }

    public void BindCollege()
    {
        try
        {
            string group_code = Convert.ToString(Session["group_code"]).Trim();
            string columnfield = string.Empty;
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = Convert.ToString(group_semi[0]).Trim();
            }
            if ((Convert.ToString(group_code).Trim() != "") && (Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true" && Convert.ToString(Session["single_user"]).Trim() != "TRUE" && Convert.ToString(Session["single_user"]).Trim() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            hat.Clear();
            hat.Add("column_field", Convert.ToString(columnfield));
            ds = da.select_method("bind_college", hat, "sp");
            ddlCollege.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.Enabled = true;
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
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
            ddlbatch.Items.Clear();
            if (ddlCollege.Items.Count > 0)
            {
                qry = " select distinct batch_year from Registration where batch_year<>'-1' and CC=0 and DelFlag=0 and Exam_Flag<>'debar' and college_code='" + ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : Convert.ToString(Session["collegecode"]).Trim()) + "' order by batch_year desc";
                DataSet ds1 = new DataSet();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    ds1 = d2.select_method_wo_parameter(qry, "text");
                    ddlbatch.DataSource = ds1;
                    ddlbatch.DataTextField = "batch_year";
                    ddlbatch.DataValueField = "batch_year";
                    ddlbatch.DataBind();
                    ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
                }
            }
        }
        catch
        {
        }
    }

    public void Bind_Degree(string college_code, string user_code)
    {
        ddldegree.Items.Clear();
        DataSet ds = new DataSet();
        single_user = d2.GetFunction("select singleuser from usermaster where user_code='" + user_code + "'");

        if (single_user == "1" || single_user == "true" || single_user == "TRUE" || single_user == "True")
        {
            qry = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code;
            ds = dir.selectDataSet(qry);
        }
        else
        {
            if (group_code.Trim() != "")
            {
                qry = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_code;
                ds = dir.selectDataSet(qry);
            }
        }
        if (ds.Tables.Count > 0)
        {
            ddldegree.DataSource = ds;
            ddldegree.DataTextField = "course_name";
            ddldegree.DataValueField = "course_id";
            ddldegree.DataBind();
        }
    }

    public void Bind_Dept(string degree_code, string college_code, string user_code)
    {
        ddlbranch.Items.Clear();
        hat.Clear();
        string usercode = Session["usercode"].ToString();
        string collegecode = ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : Convert.ToString(Session["collegecode"]).Trim());
        string singleuser = Session["single_user"].ToString();
        hat.Add("single_user", singleuser);
        hat.Add("group_code", group_code);
        hat.Add("course_id", ddldegree.SelectedValue);
        hat.Add("college_code", ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : Convert.ToString(Session["collegecode"]).Trim()));
        hat.Add("user_code", usercode);
        ds = d2.select_method("bind_branch", hat, "sp");
        //return ds;
        if (ds.Tables.Count > 0)
        {
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataBind();
        }
    }

    public void bindsem()
    {
        ddlsem.Items.Clear();
        bool first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        if (ddlbranch.Items.Count > 0 && ddlCollege.Items.Count > 0 && ddlbatch.Items.Count > 0)
        {
            qry = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and batch_year='" + ddlbatch.Text.ToString() + "' and college_code='" + ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : Convert.ToString(Session["collegecode"]).Trim()) + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qry, "text");
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlsem.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlsem.Items.Add(i.ToString());
                }
            }
        }
        else
        {
            if (ddlbranch.Items.Count > 0 && ddlCollege.Items.Count > 0)
            {
                qry = "select distinct duration,first_year_nonsemester  from degree where degree_code='" + ddlbranch.Text.ToString() + "' and college_code='" + ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : Convert.ToString(Session["collegecode"]).Trim()) + "'";
                ddlsem.Items.Clear();
                ds = d2.select_method_wo_parameter(qry, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                }
            }
        }
        if (ddlsem.Items.Count > 0)
        {
            ddlsem.SelectedIndex = 0;
            BindSectionDetail();
        }
    }

    public void BindSectionDetail()
    {

        ddlsec.Items.Clear();
        ddlsec.Enabled = false;
        if (ddlbranch.Items.Count > 0 && ddlbatch.Items.Count > 0)
        {
            string branch = ddlbranch.SelectedValue.ToString();
            string batch = ddlbatch.SelectedValue.ToString();
            qry = "select distinct LTRIM(RTRIM(ISNULL(sections,''))) sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'";
            DataSet ds = da.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataValueField = "sections";
                ddlsec.DataBind();
                ddlsec.Enabled = true;
            }
        }
        //ddlsec.Items.Insert(0, "All");
    }

    private void BindSubject()
    {
        try
        {
            ds.Clear();
            ddl_select_subj.Items.Clear();
            //ddl_select_subj.Items.Insert(0, new ListItem("--Select--", "-1"));

            string examMonth = string.Empty;
            string examYear = string.Empty;
            string collegeCodes = string.Empty;
            string degreeCodes = string.Empty;
            string semesters = string.Empty;
            string sections = string.Empty;
            string batchYears = string.Empty;

            string qryCollege = string.Empty;
            string qryDegreeCode = string.Empty;
            string qrySection = string.Empty;
            string qrySemester = string.Empty;
            string qryBatch = string.Empty;
            string qryExamMonth = string.Empty;
            string selectedval = string.Empty;
            if (ddlbatch.Items.Count > 0)
            {
                batchYears = string.Empty;
                foreach (ListItem li in ddlbatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYears))
                            batchYears = "'" + li.Text + "'";
                        else
                            batchYears += ",'" + li.Text + "'";
                    }
                }
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and sm.Batch_year in(" + batchYears + ")";
                }
            }
            if (ddlbranch.Items.Count > 0)
            {
                degreeCodes = string.Empty;
                foreach (ListItem li in ddlbranch.Items)
                {
                    if (li.Selected)
                        if (string.IsNullOrEmpty(degreeCodes))
                            degreeCodes = "'" + li.Value + "'";
                        else
                            degreeCodes += ",'" + li.Value + "'";
                }
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and sm.degree_code in(" + degreeCodes + ")";
                }
            }
            if (ddlsem.Items.Count > 0)
            {
                semesters = string.Empty;
                foreach (ListItem li in ddlsem.Items)
                {
                    if (li.Selected)
                        if (string.IsNullOrEmpty(semesters))
                            semesters = "'" + li.Text + "'";
                        else
                            semesters += ",'" + li.Text + "'";
                }
                if (!string.IsNullOrEmpty(semesters))
                {
                    qrySemester = " and sm.semester in(" + semesters + ")";
                }
            }

            if (ddlsec.Items.Count > 0)
            {
                sections = string.Empty;
                foreach (ListItem li in ddlsec.Items)
                {
                    if (li.Selected)
                        if (string.IsNullOrEmpty(sections))
                            sections = "'" + li.Value + "'";
                        else
                            sections += ",'" + li.Value + "'";
                }
                if (!string.IsNullOrEmpty(sections))
                {
                    qrySection = " and LTRIM(RTRIM(ISNULL(sf.Sections,''))) in(" + sections + ")";
                }
            }
            if (!string.IsNullOrEmpty(qrySemester) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryBatch))
            {
                if (Session["staff_code"] == null || string.IsNullOrEmpty(Convert.ToString(Session["staff_code"]).Trim()))
                {
                    qry = "select distinct s.subject_no,s.subject_code,s.subject_name from subject s,syllabus_master sm,sub_sem ss,subjectChooser sc,staff_selector sf where sc.subject_no=s.subject_no and sf.subject_no=s.subject_no and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.promote_count=1 " + qryBatch + qrySemester + qryDegreeCode + qrySection + " order by s.subject_code";
                }
                else if (Session["staff_code"] != null)
                {
                    qry = "select distinct s.subject_no,s.subject_code,s.subject_name from subject s, syllabus_master sm,sub_sem ss,subjectChooser sc,staff_selector sf where sc.subject_no=s.subject_no and sf.subject_no=s.subject_no and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.promote_count=1 " + qryBatch + qrySemester + qryDegreeCode + qrySection + " and sf.staff_code='" + Convert.ToString(Session["staff_code"]).Trim() + "' order by s.subject_code";
                }
                else
                {
                    qry = "select distinct s.subject_no,s.subject_code,s.subject_name from subject s,syllabus_master sm,sub_sem ss,subjectChooser sc,staff_selector sf where sc.subject_no=s.subject_no and sf.subject_no=s.subject_no and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.promote_count=1 " + qryBatch + qrySemester + qryDegreeCode + qrySection + " order by s.subject_code";
                }
                ds.Clear();
                ds = da.select_method_wo_parameter(qry, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddl_select_subj.DataSource = ds;
                    ddl_select_subj.DataTextField = "subject_name";
                    ddl_select_subj.DataValueField = "subject_no";
                    ddl_select_subj.DataBind();
                    ddl_select_subj.Enabled = true;
                    ddl_select_subj.SelectedIndex = 0;
                }
                else
                {
                    ddl_select_subj.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void BindSpecialHourDate()
    {
        //branch = ddlbranch.SelectedValue.ToString();
        //batch = ddlbatch.SelectedValue.ToString();
        //college = ddlCollege.SelectedValue.ToString();
        //sec = ddlsec.SelectedValue.ToString();
        //degree = ddldegree.SelectedValue.ToString();
        //sem = ddlsem.SelectedValue.ToString();
        //subject = ddl_select_subj.SelectedValue.ToString();

        ddlspecialhour.Items.Clear();

        string examMonth = string.Empty;
        string examYear = string.Empty;
        string collegeCodes = string.Empty;
        string degreeCodes = string.Empty;
        string semesters = string.Empty;
        string sections = string.Empty;
        string batchYears = string.Empty;

        string qryCollege = string.Empty;
        string qryDegreeCode = string.Empty;
        string qrySection = string.Empty;
        string qrySemester = string.Empty;
        string qryBatch = string.Empty;
        string qryExamMonth = string.Empty;
        string selectedval = string.Empty;
        string qrySubjectNo = string.Empty;
        //if (ddlCollege.Items.Count > 0)
        //{
        //    collegeCodes = string.Empty;
        //    if (ddlCollege is DropDownList)
        //    {
        //        collegeCodes = "'" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'";
        //    }
        //    else
        //    {
        //        foreach (ListItem li in ddlCollege.Items)
        //        {
        //            if (li.Selected)
        //            {
        //                if (string.IsNullOrEmpty(collegeCodes))
        //                    collegeCodes = "'" + li.Text + "'";
        //                else
        //                    collegeCodes += ",'" + li.Text + "'";
        //            }
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(collegeCodes))
        //    {
        //        qryCollege = " and college_code in(" + collegeCodes + ")";
        //    }
        //}

        if (ddlbatch.Items.Count > 0)
        {
            batchYears = string.Empty;
            if (ddlbatch is DropDownList)
            {
                batchYears = "'" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "'";
            }
            else
            {
                foreach (ListItem li in ddlbatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYears))
                            batchYears = "'" + li.Text + "'";
                        else
                            batchYears += ",'" + li.Text + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(batchYears))
            {
                qryBatch = " and sm.Batch_year in(" + batchYears + ")";
            }
        }
        if (ddlbranch.Items.Count > 0)
        {
            degreeCodes = string.Empty;
            if (ddlbranch is DropDownList)
            {
                degreeCodes = "'" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "'";
            }
            else
            {
                foreach (ListItem li in ddlbranch.Items)
                {
                    if (li.Selected)
                        if (string.IsNullOrEmpty(degreeCodes))
                            degreeCodes = "'" + li.Value + "'";
                        else
                            degreeCodes += ",'" + li.Value + "'";
                }
            }
            if (!string.IsNullOrEmpty(degreeCodes))
            {
                qryDegreeCode = " and sm.degree_code in(" + degreeCodes + ")";
            }
        }
        if (ddlsem.Items.Count > 0)
        {
            semesters = string.Empty;
            if (ddlsem is DropDownList)
            {
                semesters = "'" + Convert.ToString(ddlsem.SelectedValue).Trim() + "'";
            }
            else
            {
                foreach (ListItem li in ddlsem.Items)
                {
                    if (li.Selected)
                        if (string.IsNullOrEmpty(semesters))
                            semesters = "'" + li.Text + "'";
                        else
                            semesters += ",'" + li.Text + "'";
                }
            }
            if (!string.IsNullOrEmpty(semesters))
            {
                qrySemester = " and sm.semester in(" + semesters + ")";
            }
        }

        if (ddlsec.Items.Count > 0)
        {
            sections = string.Empty;
            if (ddlsec is DropDownList)
            {
                sections = "'" + Convert.ToString(ddlsec.SelectedValue).Trim() + "'";
            }
            else
            {
                foreach (ListItem li in ddlsec.Items)
                {
                    if (li.Selected)
                        if (string.IsNullOrEmpty(sections))
                            sections = "'" + li.Value + "'";
                        else
                            sections += ",'" + li.Value + "'";
                }
            }
            if (!string.IsNullOrEmpty(sections))
            {
                qrySection = " and LTRIM(RTRIM(ISNULL(sm.Sections,''))) in(" + sections + ")";
            }
        }

        if (ddl_select_subj.Items.Count > 0)
        {
            subject = string.Empty;
            if (ddl_select_subj is DropDownList)
            {
                subject = "'" + Convert.ToString(ddl_select_subj.SelectedValue).Trim() + "'";
            }
            else
            {
                foreach (ListItem li in ddl_select_subj.Items)
                {
                    if (li.Selected)
                        if (string.IsNullOrEmpty(subject))
                            subject = "'" + li.Value + "'";
                        else
                            subject += ",'" + li.Value + "'";
                }
            }
            if (!string.IsNullOrEmpty(subject))
            {
                qrySubjectNo = " and sd.subject_no in(" + subject + ")";
            }
        }
        DataSet ds = new DataSet();
        if (!string.IsNullOrEmpty(qryBatch) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qrySemester) && !string.IsNullOrEmpty(qrySubjectNo))
        {
            qry = "select distinct CONVERT(varchar,sm.date,103) as DispDate ,sm.date from specialhr_master sm,specialhr_details sd  where  sd.hrentry_no=sm.hrentry_no  " + qrySection + qryBatch + qryDegreeCode + qrySemester + qrySubjectNo + " order by sm.date";//and sd.subject_no= '" + subject + "' and degree_code='" + branch + "' and semester='" + sem + "' and batch_year='" + batch + "'
            ds = da.select_method_wo_parameter(qry, "text");
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlspecialhour.DataSource = ds;
            ddlspecialhour.DataTextField = "DispDate";
            ddlspecialhour.DataValueField = "date";
            ddlspecialhour.DataBind();
            BindSpecialHourTime();
        }
        else
        {
            ddlspecialhour.Items.Insert(0, new ListItem("--Not Available--", ""));
            ddlspecialhourtiem.Items.Insert(0, new ListItem("--Not Available--", ""));
            Btngo.Enabled = false;
        }
    }

    private void BindSpecialHourStaffList()
    {
        //branch = ddlbranch.SelectedValue.ToString();
        //batch = ddlbatch.SelectedValue.ToString();
        //college = ddlCollege.SelectedValue.ToString();
        //sec = ddlsec.SelectedValue.ToString();
        //degree = ddldegree.SelectedValue.ToString();
        //sem = ddlsem.SelectedValue.ToString();
        //subject = ddl_select_subj.SelectedValue.ToString();

        ddlStaffList.Items.Clear();

        string examMonth = string.Empty;
        string examYear = string.Empty;
        string collegeCodes = string.Empty;
        string degreeCodes = string.Empty;
        string semesters = string.Empty;
        string sections = string.Empty;
        string batchYears = string.Empty;

        string qryCollege = string.Empty;
        string qryDegreeCode = string.Empty;
        string qrySection = string.Empty;
        string qrySemester = string.Empty;
        string qryBatch = string.Empty;
        string qryExamMonth = string.Empty;
        string selectedval = string.Empty;
        string qrySubjectNo = string.Empty;
        qrySpecialHourDate = string.Empty;
        specialHourDate = string.Empty;

        if (ddlbatch.Items.Count > 0)
        {
            batchYears = string.Empty;
            if (ddlbatch is DropDownList)
            {
                batchYears = "'" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "'";
            }
            else
            {
                foreach (ListItem li in ddlbatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYears))
                            batchYears = "'" + li.Text + "'";
                        else
                            batchYears += ",'" + li.Text + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(batchYears))
            {
                qryBatch = " and sm.Batch_year in(" + batchYears + ")";
            }
        }
        if (ddlbranch.Items.Count > 0)
        {
            degreeCodes = string.Empty;
            if (ddlbranch is DropDownList)
            {
                degreeCodes = "'" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "'";
            }
            else
            {
                foreach (ListItem li in ddlbranch.Items)
                {
                    if (li.Selected)
                        if (string.IsNullOrEmpty(degreeCodes))
                            degreeCodes = "'" + li.Value + "'";
                        else
                            degreeCodes += ",'" + li.Value + "'";
                }
            }
            if (!string.IsNullOrEmpty(degreeCodes))
            {
                qryDegreeCode = " and sm.degree_code in(" + degreeCodes + ")";
            }
        }
        if (ddlsem.Items.Count > 0)
        {
            semesters = string.Empty;
            if (ddlsem is DropDownList)
            {
                semesters = "'" + Convert.ToString(ddlsem.SelectedValue).Trim() + "'";
            }
            else
            {
                foreach (ListItem li in ddlsem.Items)
                {
                    if (li.Selected)
                        if (string.IsNullOrEmpty(semesters))
                            semesters = "'" + li.Text + "'";
                        else
                            semesters += ",'" + li.Text + "'";
                }
            }
            if (!string.IsNullOrEmpty(semesters))
            {
                qrySemester = " and sm.semester in(" + semesters + ")";
            }
        }

        if (ddlsec.Items.Count > 0)
        {
            sections = string.Empty;
            if (ddlsec is DropDownList)
            {
                sections = "'" + Convert.ToString(ddlsec.SelectedValue).Trim() + "'";
            }
            else
            {
                foreach (ListItem li in ddlsec.Items)
                {
                    if (li.Selected)
                        if (string.IsNullOrEmpty(sections))
                            sections = "'" + li.Value + "'";
                        else
                            sections += ",'" + li.Value + "'";
                }
            }
            if (!string.IsNullOrEmpty(sections))
            {
                qrySection = " and LTRIM(RTRIM(ISNULL(shm.Sections,''))) in(" + sections + ")";
            }
        }

        if (ddl_select_subj.Items.Count > 0)
        {
            subject = string.Empty;
            if (ddl_select_subj is DropDownList)
            {
                subject = "'" + Convert.ToString(ddl_select_subj.SelectedValue).Trim() + "'";
            }
            else
            {
                foreach (ListItem li in ddl_select_subj.Items)
                {
                    if (li.Selected)
                        if (string.IsNullOrEmpty(subject))
                            subject = "'" + li.Value + "'";
                        else
                            subject += ",'" + li.Value + "'";
                }
            }
            if (!string.IsNullOrEmpty(subject))
            {
                qrySubjectNo = " and sd.subject_no in(" + subject + ")";
            }
        }
        if (ddlspecialhour.Items.Count > 0)
        {
            specialHourDate = string.Empty;
            if (ddlspecialhour is DropDownList)
            {
                specialHourDate = "'" + Convert.ToString(ddlspecialhour.SelectedValue).Trim() + "'";
            }
            else
            {
                foreach (ListItem li in ddlspecialhour.Items)
                {
                    if (li.Selected)
                        if (string.IsNullOrEmpty(specialHourDate))
                            specialHourDate = "'" + li.Value + "'";
                        else
                            specialHourDate += ",'" + li.Value + "'";
                }
            }
            if (!string.IsNullOrEmpty(specialHourDate))
            {
                qrySpecialHourDate = " and shm.date in(" + specialHourDate + ")";
            }
        }
        DataSet ds = new DataSet();
        if (!string.IsNullOrEmpty(qryBatch) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qrySemester) && !string.IsNullOrEmpty(qrySubjectNo))
        {
            //qry = "select distinct CONVERT(varchar,sm.date,103) as DispDate ,sm.date from specialhr_master sm,specialhr_details sd  where  sd.hrentry_no=sm.hrentry_no  " + qrySection + qryBatch + qryDegreeCode + qrySemester + " order by sm.date";//and sd.subject_no= '" + subject + "' and degree_code='" + branch + "' and semester='" + sem + "' and batch_year='" + batch + "'
            qry = "  select distinct sfm.staff_name,sfm.staff_code from subject s,syllabus_master sm,staff_selector ss,staffmaster sfm,specialhr_details sd,specialhr_master shm where sm.syll_code=s.syll_code and ss.staff_code=sfm.staff_code and s.subject_no=ss.subject_no and s.subject_no=sd.subject_no and sd.subject_no=ss.subject_no and sfm.staff_code=sd.staff_code and sd.staff_code=ss.staff_code and sd.hrentry_no=shm.hrentry_no and sm.Batch_Year=shm.batch_year and shm.degree_code=sm.degree_code and sm.semester=shm.semester and LTRIM(RTRIM(ISNULL(ss.Sections,'')))=LTRIM(RTRIM(ISNULL(shm.sections,''))) " + qryBatch + qryDegreeCode + qrySemester + qrySection + qrySpecialHourDate + qrySubjectNo;
            ds = da.select_method_wo_parameter(qry, "text");
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlStaffList.DataSource = ds;
            ddlStaffList.DataTextField = "staff_name";
            ddlStaffList.DataValueField = "staff_code";
            ddlStaffList.DataBind();
        }
        else
        {
            ddlStaffList.Items.Insert(0, new ListItem("--Not Available--", ""));
        }
    }

    private void BindSpecialHourTime()
    {
        try
        {
            string time = string.Empty;
            //branch = ddlbranch.SelectedValue.ToString();
            //batch = ddlbatch.SelectedValue.ToString();
            //college = ddlCollege.SelectedValue.ToString();
            //sec = ddlsec.SelectedValue.ToString();
            //degree = ddldegree.SelectedValue.ToString();
            //sem = ddlsem.SelectedValue.ToString();
            //subject = ddl_select_subj.SelectedValue.ToString();
            ddlspecialhourtiem.Items.Clear();

            string examMonth = string.Empty;
            string examYear = string.Empty;
            string specialHourDate = string.Empty;
            string collegeCodes = string.Empty;
            string degreeCodes = string.Empty;
            string semesters = string.Empty;
            string sections = string.Empty;
            string batchYears = string.Empty;

            string qryCollege = string.Empty;
            string qryDegreeCode = string.Empty;
            string qrySection = string.Empty;
            string qrySemester = string.Empty;
            string qryBatch = string.Empty;
            string qryExamMonth = string.Empty;
            string selectedval = string.Empty;
            string qrySubjectNo = string.Empty;
            string qrySpecialHourDate = string.Empty;
            //if (ddlCollege.Items.Count > 0)
            //{
            //    collegeCodes = string.Empty;
            //    if (ddlCollege is DropDownList)
            //    {
            //        collegeCodes = "'" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'";
            //    }
            //    else
            //    {
            //        foreach (ListItem li in ddlCollege.Items)
            //        {
            //            if (li.Selected)
            //            {
            //                if (string.IsNullOrEmpty(collegeCodes))
            //                    collegeCodes = "'" + li.Text + "'";
            //                else
            //                    collegeCodes += ",'" + li.Text + "'";
            //            }
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(collegeCodes))
            //    {
            //        qryCollege = " and college_code in(" + collegeCodes + ")";
            //    }
            //}

            if (ddlbatch.Items.Count > 0)
            {
                batchYears = string.Empty;
                if (ddlbatch is DropDownList)
                {
                    batchYears = "'" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "'";
                }
                else
                {
                    foreach (ListItem li in ddlbatch.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(batchYears))
                                batchYears = "'" + li.Text + "'";
                            else
                                batchYears += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatch = " and sm.Batch_year in(" + batchYears + ")";
                }
            }
            if (ddlbranch.Items.Count > 0)
            {
                degreeCodes = string.Empty;
                if (ddlbranch is DropDownList)
                {
                    degreeCodes = "'" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "'";
                }
                else
                {
                    foreach (ListItem li in ddlbranch.Items)
                    {
                        if (li.Selected)
                            if (string.IsNullOrEmpty(degreeCodes))
                                degreeCodes = "'" + li.Value + "'";
                            else
                                degreeCodes += ",'" + li.Value + "'";
                    }
                }
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and sm.degree_code in(" + degreeCodes + ")";
                }
            }
            if (ddlsem.Items.Count > 0)
            {
                semesters = string.Empty;
                if (ddlsem is DropDownList)
                {
                    semesters = "'" + Convert.ToString(ddlsem.SelectedValue).Trim() + "'";
                }
                else
                {
                    foreach (ListItem li in ddlsem.Items)
                    {
                        if (li.Selected)
                            if (string.IsNullOrEmpty(semesters))
                                semesters = "'" + li.Text + "'";
                            else
                                semesters += ",'" + li.Text + "'";
                    }
                }
                if (!string.IsNullOrEmpty(semesters))
                {
                    qrySemester = " and sm.semester in(" + semesters + ")";
                }
            }

            if (ddlsec.Items.Count > 0)
            {
                sections = string.Empty;
                if (ddlsec is DropDownList)
                {
                    sections = "'" + Convert.ToString(ddlsec.SelectedValue).Trim() + "'";
                }
                else
                {
                    foreach (ListItem li in ddlsec.Items)
                    {
                        if (li.Selected)
                            if (string.IsNullOrEmpty(sections))
                                sections = "'" + li.Value + "'";
                            else
                                sections += ",'" + li.Value + "'";
                    }
                }
                if (!string.IsNullOrEmpty(sections))
                {
                    qrySection = " and LTRIM(RTRIM(ISNULL(sm.Sections,''))) in(" + sections + ")";
                }
            }

            if (ddl_select_subj.Items.Count > 0)
            {
                subject = string.Empty;
                if (ddl_select_subj is DropDownList)
                {
                    subject = "'" + Convert.ToString(ddl_select_subj.SelectedValue).Trim() + "'";
                }
                else
                {
                    foreach (ListItem li in ddl_select_subj.Items)
                    {
                        if (li.Selected)
                            if (string.IsNullOrEmpty(subject))
                                subject = "'" + li.Value + "'";
                            else
                                subject += ",'" + li.Value + "'";
                    }
                }
                if (!string.IsNullOrEmpty(subject))
                {
                    qrySubjectNo = " and sd.subject_no in(" + subject + ")";
                }
            }

            if (ddlspecialhour.Items.Count > 0)
            {
                specialHourDate = string.Empty;
                if (ddlspecialhour is DropDownList)
                {
                    specialHourDate = "'" + Convert.ToString(ddlspecialhour.SelectedValue).Trim() + "'";
                }
                else
                {
                    foreach (ListItem li in ddlspecialhour.Items)
                    {
                        if (li.Selected)
                            if (string.IsNullOrEmpty(specialHourDate))
                                specialHourDate = "'" + li.Value + "'";
                            else
                                specialHourDate += ",'" + li.Value + "'";
                    }
                }
                if (!string.IsNullOrEmpty(specialHourDate))
                {
                    qrySpecialHourDate = " and sm.date in(" + specialHourDate + ")";
                }
            }
            string staffCode = string.Empty;
            string qryStaffCode = string.Empty;
            if (ddlStaffList.Items.Count > 0)
            {
                staffCode = Convert.ToString(ddlStaffList.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(staffCode) && staffCode.Trim().ToLower() != "all" && staffCode.Trim().ToLower() != "all")
                {
                    qryStaffCode = " and sd.staff_code='" + staffCode + "'";
                }
            }
            if (!string.IsNullOrEmpty(qryBatch) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qrySemester) && !string.IsNullOrEmpty(qrySubjectNo) && !string.IsNullOrEmpty(qryStaffCode))
            {
                qry = "select DISTINCT CONVERT(varchar,start_time,108) +'-'+ CONVERT(varchar,end_time,108) as Time,hrdet_no  from specialhr_details sd,specialhr_master sm where  sd.hrentry_no=sm.hrentry_no " + qryBatch + qryDegreeCode + qrySemester + qrySection + qrySpecialHourDate + qrySubjectNo + qryStaffCode;//and  sd.subject_no='" + subject + "' and sm.degree_code='" + branch + "' and sm.semester ='" + sem + "' and sm.batch_year='" + batch + "'
                DataTable dttym = dir.selectDataTable(qry);
                if (dttym.Rows.Count > 0)
                {
                    ddlspecialhourtiem.DataSource = dttym;
                    ddlspecialhourtiem.DataTextField = "Time";
                    ddlspecialhourtiem.DataValueField = "hrdet_no";
                    ddlspecialhourtiem.DataBind();
                    Btngo.Enabled = true;
                }
                else
                {
                    ddlspecialhourtiem.Items.Insert(0, new ListItem("--Not Available--", ""));
                }
            }
            else
            {
                ddlspecialhourtiem.Items.Insert(0, new ListItem("--Not Available--", ""));
            }
        }
        catch (Exception e)
        {

        }
    }

    protected void Btngo_Click(object sender, EventArgs e)
    {
        GridView1.Visible = true;
        string branchqry = string.Empty;
        string batchqry = string.Empty;
        string collegeqry = string.Empty;
        string Sectionqry = string.Empty;
        string semqry = string.Empty;
        string subjqry = string.Empty;

        if (ddlbranch.Items.Count > 0)
        {
            branch = ddlbranch.SelectedValue.ToString();
            branchqry = "and sm.degree_code='" + branch + "'";
        }
        if (ddlbatch.Items.Count > 0)
        {
            batch = ddlbatch.SelectedValue.ToString();
            batchqry = "and sm.Batch_Year='" + batch + "'";
        }
        if (ddlCollege.Items.Count > 0)
        {
            college = ddlCollege.SelectedValue.ToString();
            collegeqry = "and r.college_code='" + college + "'";
        }
        if (ddlsem.Items.Count > 0)
        {
            sem = ddlsem.SelectedValue.ToString();
            semqry = "and sm.semester='" + sem + "'";
        }
        if (ddl_select_subj.Items.Count > 0)
        {
            subject = ddl_select_subj.SelectedValue.ToString();
            subjqry = "and s.subject_no='" + subject + "'";
        }
        if (ddlsec.Items.Count > 0)
        {
            sec = ddlsec.SelectedValue.ToString();
            Sectionqry = "and r.Sections='" + sec + "'";
        }
        //batch = ddlbatch.SelectedValue.ToString();
        //college = ddlCollege.SelectedValue.ToString();
        //sec = ddlsec.SelectedValue.ToString();
        //degree = ddldegree.SelectedValue.ToString();
        //sem = ddlsem.SelectedValue.ToString();
        //subject = ddl_select_subj.SelectedValue.ToString();
        string qryexistin = string.Empty;

        try
        {
            qry = "select distinct r.Batch_Year,r.degree_code,r.Current_Semester,LTRIM(RTRIM(ISNULL(r.Sections,''))) as Sections,r.Roll_No,r.serialno,r.App_No,r.Roll_Admit,r.Reg_No,r.Stud_Name,r.Stud_Type from Registration r, syllabus_master sm ,Subject s,subjectChooser sc where r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and sm.syll_code=s.syll_code and r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' " + collegeqry + " " + batchqry + "" + branchqry + " " + semqry + "" + subjqry + "" + Sectionqry + " " + orderByStudents(Convert.ToString(ddlCollege.SelectedValue).Trim(), "r");
            ds = da.select_method_wo_parameter(qry, "Text");
            //string orderBy = orderByStudents(collegeCode, "r");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                GridView1.DataSource = ds;
                GridView1.DataBind();
                divgrid.Visible = true;
                btnsave.Visible = true;
            }
            else
            {
                lblAlertMsg.Text = "No Records Found";
                divPopAlert.Visible = true;
                return;
            }

            #region getting the value of hrdet_no

            string hrdet_no = string.Empty;
            foreach (ListItem li in ddlspecialhourtiem.Items)
            {
                if (li.Selected)
                    if (!string.IsNullOrEmpty(hrdet_no))
                        hrdet_no += ",'" + li.Value + "'";
                    else
                        hrdet_no = "'" + li.Value + "'";
            }

            #endregion

            bool isRollNoVisible = ColumnHeaderVisiblity(0);
            bool isRegNoVisible = ColumnHeaderVisiblity(1);
            bool isAdmissionNoVisible = ColumnHeaderVisiblity(2);
            bool isStudentTypeVisible = ColumnHeaderVisiblity(3);
            DataTable dt4 = new DataTable();
            if (!string.IsNullOrEmpty(hrdet_no))
            {
                qryexistin = " select distinct std.appNo as AppNo,spd.subject_no,spd.staff_code from specialhr_details spd,specialHourStudents Std where  std.hrdet_no=spd.hrdet_no  and spd.hrdet_no in(" + hrdet_no + ") ";
                dt4 = dir.selectDataTable(qryexistin);
            }
            string tableappno = string.Empty;
            if (GridView1.Rows.Count > 0)
            {
                foreach (GridViewRow row in GridView1.Rows)
                {
                    CheckBox cbgrd = row.FindControl("gridcb") as CheckBox;
                    cbgrd.Checked = false;
                    string appno = ((Label)row.FindControl("lblgridapplicationno")).Text.Trim();
                    row.Cells[2].Visible = isRollNoVisible;
                    row.Cells[3].Visible = isRegNoVisible;
                    row.Cells[5].Visible = isAdmissionNoVisible;
                    row.Cells[11].Visible = isStudentTypeVisible;
                    GridView1.HeaderRow.Cells[2].Visible = isRollNoVisible;
                    GridView1.HeaderRow.Cells[3].Visible = isRegNoVisible;
                    GridView1.HeaderRow.Cells[5].Visible = isAdmissionNoVisible;
                    GridView1.HeaderRow.Cells[11].Visible = isStudentTypeVisible;
                    DataView dvSelStudent = new DataView();
                    dt4.DefaultView.RowFilter = "AppNo='" + appno + "'";
                    dvSelStudent = dt4.DefaultView;
                    if (dvSelStudent.Count > 0)
                    {
                        cbgrd.Checked = true;
                    }
                }
                select_range.Visible = true;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(Session["collegecode"]), "ChallanReceipt"); 
        }

    }

    protected void Btnsave_Click(object sender, EventArgs e)
    {
        int id = 0;
        bool isSaved = false;
        string hrDetNo = string.Empty;
        foreach (ListItem li in ddlspecialhourtiem.Items)
        {
            if (li.Selected)
            {
                if (!string.IsNullOrEmpty(hrDetNo))
                {
                    hrDetNo += ",'" + li.Value + "'";
                }
                else
                {
                    hrDetNo = "'" + li.Value + "'";
                }
            }
        }
        if (!string.IsNullOrEmpty(hrDetNo))
        {
            qry = "delete from specialHourStudents where hrdet_no in(" + hrDetNo + ")";
            id = dir.insertData(qry);
        }
        if (GridView1.Rows.Count > 0)
        {
            foreach (GridViewRow gvrow in GridView1.Rows)
            {
                CheckBox checkbox = gvrow.FindControl("gridcb") as CheckBox;
                string batchyr = ((Label)gvrow.FindControl("Lblgridbatchyear")).Text.Trim();
                string degreecode = ((Label)gvrow.FindControl("Lblgriddegree")).Text.Trim();
                string currentsem = ((Label)gvrow.FindControl("Lblgridsemester")).Text.Trim();
                string section = ((Label)gvrow.FindControl("Lblgridsection")).Text.Trim();
                string rollno = ((Label)gvrow.FindControl("lblgridrollno")).Text.Trim();
                // string serialno = ((Label)gvrow.FindControl("lblgridserialno")).Text.Trim();
                string appno = ((Label)gvrow.FindControl("lblgridapplicationno")).Text.Trim();
                string admissionno = ((Label)gvrow.FindControl("lblgridadmissionno")).Text.Trim();
                string regno = ((Label)gvrow.FindControl("lblgridregno")).Text.Trim();
                string name = ((Label)gvrow.FindControl("Lblgridname")).Text.Trim();
                string type = ((Label)gvrow.FindControl("lblgridstudenttype")).Text.Trim();

                if (checkbox.Checked == true)
                {
                    #region checkboxlist if needed

                    foreach (ListItem li in ddlspecialhourtiem.Items)
                    {
                        if (li.Selected)
                        {
                            qry = "if not exists ( select * from  specialHourStudents where hrdet_no='" + li.Value.ToString().Trim() + "' and appNo='" + appno + "' )  insert into specialHourStudents(hrdet_no,appNo) values('" + li.Value.ToString().Trim() + "','" + appno + "')";
                            id = dir.insertData(qry);
                            if (id != 0)
                                isSaved = true;
                        }
                    }

                    #endregion
                }
            }
        }
        Btngo_Click(sender, e);
        lblAlertMsg.Text = (isSaved) ? "Saved Successfully" : "Not Saved";
        divPopAlert.Visible = true;
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string grouporusercode = string.Empty;
            string userOrGroupCode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0] + "'";
                userOrGroupCode = Convert.ToString(Session["group_code"]).Trim().Split(';')[0];
            }
            else
            {
                grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                userOrGroupCode = Convert.ToString(Session["usercode"]).Trim();
            }
            lblErrMsg.Text = string.Empty;
            lblErrMsg.Visible = false;
            BindBatch();
            Bind_Degree(Convert.ToString(ddlCollege.SelectedValue).Trim(), userOrGroupCode);
            Bind_Dept(Convert.ToString(ddldegree.SelectedValue).Trim(), Convert.ToString(ddlCollege.SelectedValue).Trim(), userOrGroupCode);
            bindsem();
            BindSectionDetail();
            BindSubject();
            BindSpecialHourDate();
            BindSpecialHourStaffList();
            BindSpecialHourTime();
            GridView1.Visible = false;
            btnsave.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        string grouporusercode = string.Empty;
        string userOrGroupCode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0] + "'";
            userOrGroupCode = Convert.ToString(Session["group_code"]).Trim().Split(';')[0];
        }
        else
        {
            grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            userOrGroupCode = Convert.ToString(Session["usercode"]).Trim();
        }
        Bind_Degree(Convert.ToString(ddlCollege.SelectedValue).Trim(), userOrGroupCode);
        Bind_Dept(Convert.ToString(ddldegree.SelectedValue).Trim(), Convert.ToString(ddlCollege.SelectedValue).Trim(), userOrGroupCode);
        bindsem();
        BindSectionDetail();
        BindSubject();
        BindSpecialHourDate();
        BindSpecialHourStaffList();
        BindSpecialHourTime();
        GridView1.Visible = false;
        btnsave.Visible = false;
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        string grouporusercode = string.Empty;
        string userOrGroupCode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0] + "'";
            userOrGroupCode = Convert.ToString(Session["group_code"]).Trim().Split(';')[0];
        }
        else
        {
            grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            userOrGroupCode = Convert.ToString(Session["usercode"]).Trim();
        }
        Bind_Dept(Convert.ToString(ddldegree.SelectedValue).Trim(), Convert.ToString(ddlCollege.SelectedValue).Trim(), userOrGroupCode);
        bindsem();
        BindSectionDetail();
        BindSubject();
        BindSpecialHourDate();
        BindSpecialHourStaffList();
        BindSpecialHourTime();
        GridView1.Visible = false;
        btnsave.Visible = false;
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsem();
            BindSectionDetail();
            BindSubject();
            BindSpecialHourDate();
            BindSpecialHourStaffList();
            BindSpecialHourTime();
            GridView1.Visible = false;
            btnsave.Visible = false;
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
        }
        BindSubject();
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSectionDetail();
        BindSubject();
        BindSpecialHourDate();
        BindSpecialHourStaffList();
        BindSpecialHourTime();
        string collegecode = Convert.ToString(Session["collegecode"]).Trim();
        GridView1.Visible = false;
        btnsave.Visible = false;
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSubject();
        BindSpecialHourDate();
        BindSpecialHourStaffList();
        BindSpecialHourTime();
        GridView1.Visible = false;
        btnsave.Visible = false;
    }

    public void ddl_select_subj_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSpecialHourDate();
        BindSpecialHourStaffList();
        BindSpecialHourTime();
        GridView1.Visible = false;
        btnsave.Visible = false;
    }

    public void ddlspecialhour_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSpecialHourStaffList();
        BindSpecialHourTime();
        GridView1.Visible = false;
        btnsave.Visible = false;
    }

    public void ddlStaffList_SelectedIndexChanged(object sender, EventArgs e)
    {
        //BindSpecialHourDate();
        BindSpecialHourTime();
        GridView1.Visible = false;
        btnsave.Visible = false;
    }

    public void ddlspecialhourtiem_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.Visible = false;
        btnsave.Visible = false;
    }

    protected void SelectAll_Checked(object sender, EventArgs e)
    {
        CheckBox chckheader = (CheckBox)GridView1.HeaderRow.FindControl("chkselectall");
        foreach (GridViewRow row in GridView1.Rows)
        {
            CheckBox chckrw = (CheckBox)row.FindControl("gridcb");
            chckrw.Checked = chckheader.Checked;
            if (chckheader.Checked == true)
            {
                chckrw.Checked = true;
            }
            else
            {
                String qryexistin = string.Empty;
                #region getting the value of hrdet_no

                foreach (ListItem li in ddlspecialhourtiem.Items)
                {
                    if (li.Selected)
                        qryexistin = " select distinct std.appNo as AppNo,spd.subject_no,spd.staff_code from specialhr_details spd,specialHourStudents Std where  std.hrdet_no=spd.hrdet_no  and spd.hrdet_no='" + li.Value.ToString().Trim() + "' ";
                }
                DataTable dt4 = dir.selectDataTable(qryexistin);
                string tableappno = string.Empty;

                foreach (GridViewRow row1 in GridView1.Rows)
                {
                    CheckBox cbgrd = row1.FindControl("gridcb") as CheckBox;
                    string appno = ((Label)row1.FindControl("lblgridapplicationno")).Text.Trim();
                    foreach (DataRow item in dt4.Rows)
                    {
                        tableappno = item["AppNo"].ToString().Trim();
                        if (appno == tableappno)
                            cbgrd.Checked = true;
                        else
                            chckrw.Checked = false;
                    }
                }
                #endregion
            }
        }
    }

    private string orderByStudents(string collegeCode, string aliasName = null, string tableName = null, byte includeOrderBy = 0)
    {
        string orderBy = string.Empty;
        try
        {
            string orderBySetting = dir.selectScalarString("select value from master_Settings where settings='order_by' ");//and value<>''
            orderBySetting = orderBySetting.Trim();

            string serialNo = dir.selectScalarString("select LinkValue from inssettings where college_code='" + collegeCode + "' and linkname='Student Attendance'");

            string aliasOrTableName = ((string.IsNullOrEmpty(aliasName) && string.IsNullOrEmpty(tableName)) ? "" : ((!string.IsNullOrEmpty(tableName)) ? tableName.Trim() + "." : ((!string.IsNullOrEmpty(aliasName)) ? aliasName.Trim() + "." : "")));

            orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no";
            if (serialNo.Trim().ToLower() == "1" || serialNo.ToLower().Trim() == "true")
                orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "serialno";
            else
                switch (orderBySetting)
                {
                    case "0":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no";
                        break;
                    case "1":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "Reg_No";
                        break;
                    case "2":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "Stud_Name";
                        break;
                    case "0,1,2":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no," + aliasOrTableName + "Reg_No," + aliasOrTableName + "stud_name";
                        break;
                    case "0,1":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no," + aliasOrTableName + "Reg_No";
                        break;
                    case "1,2":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "Reg_No," + aliasOrTableName + "Stud_Name";
                        break;
                    case "0,2":
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no," + aliasOrTableName + "Stud_Name";
                        break;
                    default:
                        orderBy = ((includeOrderBy == 0) ? "ORDER BY " : "") + aliasOrTableName + "roll_no";
                        break;
                }
        }
        catch (Exception ex)
        {

        }
        return orderBy;
    }

    /// <summary>
    /// Developed By Malang Raja on Dec 7 2016
    /// </summary>
    /// <param name="type">0 For Roll No,1 For Register No,2 For Admission No, 3 For Student Type , 4 For Application No</param>
    /// <param name="dsSettingsOptional">it is Optional Parameter</param>
    /// <returns>true or false</returns>
    private bool ColumnHeaderVisiblity(int type, DataSet dsSettingsOptional = null)
    {
        bool hasValues = false;
        try
        {
            DataSet dsSettings = new DataSet();
            if (dsSettingsOptional == null)
            {
                string grouporusercode = string.Empty;
                if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    string groupCode = Convert.ToString(Session["group_code"]).Trim();
                    string[] groupUser = Convert.ToString(groupCode).Trim().Split(';');
                    if (groupUser.Length > 0)
                    {
                        groupCode = groupUser[0].Trim();
                    }
                    if (!string.IsNullOrEmpty(groupCode.Trim()))
                    {
                        grouporusercode = " and  group_code=" + Convert.ToString(groupCode).Trim() + "";
                    }
                }
                else if (Session["usercode"] != null)
                {
                    grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                }
                if (!string.IsNullOrEmpty(grouporusercode))
                {
                    string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type','Application No') and value='1' " + grouporusercode + "";
                    dsSettings = dir.selectDataSet(Master1);
                }
            }
            else
            {
                dsSettings = dsSettingsOptional;
            }
            if (dsSettings.Tables.Count > 0 && dsSettings.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drSettings in dsSettings.Tables[0].Rows)
                {
                    switch (type)
                    {
                        case 0:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "roll no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 1:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "register no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 2:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "admission no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 3:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "student_type")
                            {
                                hasValues = true;
                            }
                            break;
                        case 4:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "application no")
                            {
                                hasValues = true;
                            }
                            break;
                    }
                    if (hasValues)
                        break;
                }
            }
            return hasValues;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    #region Alert Popup Close

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    #endregion

    protected void Btn_range_Click(object sender, EventArgs e)
    {
        if (txt_frange.Text == "" || txt_trange.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Both From And To Range.')", true);
            return;
        }

        if (Convert.ToInt32(txt_frange.Text) > Convert.ToInt32(txt_trange.Text))
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('To Range Should Be Greater Than Or Equal To From Range.')", true);
            return;
        }

        foreach (GridViewRow row in GridView1.Rows)
        {
            Label sno = (Label)row.FindControl("lblSno");
            string sl_no = sno.Text;
            if (sl_no != "")
            {
                CheckBox cbsel = (CheckBox)row.FindControl("gridcb");
                if (Convert.ToInt32(sl_no) >= Convert.ToInt32(txt_frange.Text) && Convert.ToInt32(sl_no) <= Convert.ToInt32(txt_trange.Text))
                {
                    cbsel.Checked = true;
                }
                else
                {
                    if (!cbsel.Checked)
                    cbsel.Checked = false;
                }
            }
        }

        txt_frange.Text = "";
        txt_trange.Text = "";
    }


}

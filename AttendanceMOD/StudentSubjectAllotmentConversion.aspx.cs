using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;

public partial class StudentSubjectAllotmentConversion : System.Web.UI.Page
{
    DAccess2 dacc = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string grouporusercode = string.Empty;
    string qry = string.Empty;
    string group_user = "", singleuser = "", usercode = "", collegecode = string.Empty;
    Hashtable hat = new Hashtable();
    string strquery = string.Empty;
    InsproDataAccess.InsproDirectAccess dir = new InsproDataAccess.InsproDirectAccess();

    Hashtable present_calcflag = new Hashtable();
    Hashtable absent_calcflag = new Hashtable();
    DataSet ds_attndmaster = new DataSet();

    static string minimum_day = string.Empty;
    static string collegename = string.Empty;
    static string coursename = string.Empty;
    string SenderID = string.Empty;
    string Password = string.Empty;

    string userCode = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string collegeCodes = string.Empty;
    string qryCollege = string.Empty;
    string qryBatch = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        GridView1.Visible = false;
        btnmove.Visible = false;
        //lblsubject.Visible = false;
        //ddlsubject.Visible = false;
       
        //lblholireason.Visible = false;
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        errmsg.Visible = false;
        if (!IsPostBack)
        {
            lblhours.Visible = false;
            ddlNewSub.Visible = false;
            lblSubTypeNew.Visible = false;
            ddlNewSubType.Visible = false;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            BindCollege();
            //bindbatch();
            BindRightsBaseBatch();
            binddegree();
            if (ddldegree.Items.Count > 0)
            {
                bindbranch();
                bindsem();
                bindsec();
                load_subjectType();
                //load_subject();
                //load_Newsubject();

            }
            else
            {
                ddldegree.Enabled = false;
                ddlbranch.Enabled = false;
                ddlduration.Enabled = false;
                ddlsec.Enabled = false;
            }
            if (Session["Staff_Code"] != null && Session["Staff_Code"].ToString() != "")
            {
            }
        }
    }

    public void BindCollege()
    {
        try
        {
            string group_code = Convert.ToString(Session["group_code"]).Trim();
            string columnfield = string.Empty;
            if (group_code.Contains(";"))
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
            ds.Clear();
            ds.Reset();
            hat.Add("column_field", Convert.ToString(columnfield));
            ds = d2.select_method("bind_college", hat, "sp");
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

    public void bindbatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds.Clear();
            ds.Reset();
            string dbindbatch = string.Empty;
            dbindbatch = " select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and delflag=0 and exam_flag<>'debar' and college_code='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "' order by batch_year";
            ds = dacc.select_method_wo_parameter(dbindbatch, "Text");
            //int count = ds.Tables[0].Rows.Count;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
            //int count1 = ds.Tables[1].Rows.Count;
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                ddlbatch.SelectedValue = max_bat.ToString();
            }
        }
        catch
        {
        }
    }

    public void BindRightsBaseBatch()
    {
        try
        {
            userCode = string.Empty;
            groupUserCode = string.Empty;
            qryUserOrGroupCode = string.Empty;
            collegeCodes = string.Empty;
            ds.Clear();
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(";"))
                {
                    string[] group_semi = group.Split(';');
                    groupUserCode = Convert.ToString(group_semi[0]);
                }
                if (!string.IsNullOrEmpty(groupUserCode))
                {
                    qryUserOrGroupCode = " and user_id='" + groupUserCode + "'";
                }
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                if (!string.IsNullOrEmpty(userCode))
                {
                    qryUserOrGroupCode = " and user_id='" + userCode + "'";
                }
            }
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCodes = Convert.ToString(ddlCollege.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(collegeCodes))
                {
                    qryCollege = " and r.college_code in(" + collegeCodes + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCodes))
            {
                qryCollege = " and r.college_code in(" + collegeCodes + ")";
            }
            DataSet dsBatch = new DataSet();
            if (!string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                string qry = "select distinct batch_year from tbl_attendance_rights r where batch_year<>'' " + qryUserOrGroupCode + " order by batch_year desc";
                dsBatch = d2.select_method_wo_parameter(qry, "Text");
            }
            qryBatch = string.Empty;
            if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
            {
                //ddlbatch.DataSource = dsBatch;
                //ddlbatch.DataTextField = "Batch_year";
                //ddlbatch.DataValueField = "Batch_year";
                //ddlbatch.DataBind();
                //ddlbatch.SelectedIndex = 0;
                List<int> lstBatch = dsBatch.Tables[0].AsEnumerable().Select(r => r.Field<int>("batch_year")).ToList();
                if (lstBatch.Count > 0)
                    qryBatch = " and r.Batch_Year in('" + string.Join("','", lstBatch.ToArray()) + "')";
            }
            if (!string.IsNullOrEmpty(collegeCodes) && !string.IsNullOrEmpty(qryCollege))
            {
                qry = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1 " + qryCollege + qryBatch + " order by r.Batch_Year desc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(qry, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "Batch_Year";
                ddlbatch.DataValueField = "Batch_Year";
                ddlbatch.DataBind();
                ddlbatch.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void binddegree()
    {
        try
        {
            ddldegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : Convert.ToString(Session["collegecode"]).Trim();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            ds.Clear();
            ds.Reset();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : Convert.ToString(Session["collegecode"]).Trim());
            hat.Add("user_code", usercode);
            ds = dacc.select_method("bind_degree", hat, "sp");
            //int count1 = ds.Tables[0].Rows.Count;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch
        {
        }
    }

    public void bindbranch()
    {
        try
        {
            ddlbranch.Items.Clear();
            ds.Clear();
            ds.Reset();
            hat.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : Convert.ToString(Session["collegecode"]).Trim();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddldegree.SelectedValue);
            hat.Add("college_code", (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : Convert.ToString(Session["collegecode"]).Trim());
            hat.Add("user_code", usercode);
            ds = dacc.select_method("bind_branch", hat, "sp");
            //int count2 = ds.Tables[0].Rows.Count;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch
        {
        }
    }

    public void bindsem()
    {
        try
        {
            ddlduration.Items.Clear();
            string duration = string.Empty;
            Boolean first_year = false;
            ds.Clear();
            ds.Reset();
            hat.Clear();
            collegecode = (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : Convert.ToString(Session["collegecode"]).Trim();
            hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
            hat.Add("batch_year", ddlbatch.SelectedValue.ToString());
            hat.Add("college_code", (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : Convert.ToString(Session["collegecode"]).Trim());
            ds = dacc.select_method("bind_sem", hat, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlduration.Enabled = true;
                duration = ds.Tables[0].Rows[0][0].ToString();
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                {
                    if (first_year == false)
                    {
                        ddlduration.Items.Add(loop_val.ToString());
                    }
                    else if (first_year == true && loop_val != 2)
                    {
                        ddlduration.Items.Add(loop_val.ToString());
                    }
                }
            }
            else
            {
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    ddlduration.Enabled = true;
                    duration = ds.Tables[1].Rows[0][0].ToString();
                    first_year = Convert.ToBoolean(ds.Tables[1].Rows[0][1].ToString());
                    for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                    {
                        if (first_year == false)
                        {
                            ddlduration.Items.Add(loop_val.ToString());
                        }
                        else if (first_year == true && loop_val != 2)
                        {
                            ddlduration.Items.Add(loop_val.ToString());
                        }
                    }
                }
                else
                {
                    ddlduration.Enabled = false;
                }
            }
        }
        catch
        {
        }
    }

    public void bindsec()
    {
        try
        {
            ds.Clear();
            ds.Reset();
            ddlsec.Items.Clear();
            strquery = "select distinct LTRIM(RTRIM(ISNULL(sections,''))) sections from registration where batch_year='" + ddlbatch.SelectedValue.ToString() + "' and college_code='" + ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : Convert.ToString(Session["collegecode"]).Trim()) + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and LTRIM(RTRIM(ISNULL(sections,'')))<>'-1' and LTRIM(RTRIM(ISNULL(sections,'')))<>'' and cc='0' and delflag=0 and exam_flag<>'Debar'";
            ds = dacc.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlsec.Enabled = true;
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataBind();
            }
            else
            {
                ddlsec.Enabled = false;
            }
        }
        catch
        {
        }
    }

    public void load_subject()
    {
        try
        {
            ds.Clear();
            ds.Reset();
            ddlsubject.Items.Clear();
            string currentSemester = Convert.ToString(ddlduration.SelectedItem).Trim();
            int currentSem = 0;
            int.TryParse(currentSemester, out currentSem);
            strquery = "select distinct s.subject_no,(s.subject_code+'-'+s.subject_name) as subject_name,ISNULL(s.subjectpriority,'0') subjectpriority from subject s,sub_sem ss,syllabus_master sm where sm.syll_code=ss.syll_code and s.syll_code=ss.syll_code and sm.syll_code=s.syll_code and s.subType_no=ss.subType_no and sm.Batch_Year=" + ddlbatch.SelectedValue.ToString() + " and sm.degree_code=" + ddlbranch.SelectedValue.ToString() + " and sm.semester='" + (currentSem - 1) + "' and ss.subType_no=" + ddlSubType.SelectedValue.ToString() + " order by subjectpriority";
            ds = dacc.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlsubject.Enabled = true;
                ddlsubject.DataSource = ds;
                ddlsubject.DataTextField = "subject_name";
                ddlsubject.DataValueField = "subject_no";
                ddlsubject.DataBind();
                ddlsubject.Items.Insert(0, "--Select--");
            }
            else
            {
                ddlsubject.Enabled = false;
            }

        }
        catch
        {
        }
    }

    public void load_subjectType()
    {
        try
        {
            ddlSubType.Items.Clear();
            string currentSemester = Convert.ToString(ddlduration.SelectedItem).Trim();
            int currentSem = 0;
            int.TryParse(currentSemester, out currentSem);
            strquery = "select distinct ss.subType_no,ss.subject_type from subject s,sub_sem ss,syllabus_master sm where sm.syll_code=ss.syll_code and s.syll_code=ss.syll_code and sm.syll_code=s.syll_code and s.subType_no=ss.subType_no and sm.Batch_Year=" + ddlbatch.SelectedValue.ToString() + " and sm.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and sm.semester='" + (currentSem - 1) + "' order by ss.subject_type";
            ds = dacc.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSubType.Enabled = true;
                ddlSubType.DataSource = ds;
                ddlSubType.DataTextField = "subject_type";
                ddlSubType.DataValueField = "subType_no";
                ddlSubType.DataBind();
                ddlSubType.Items.Insert(0, "--Select--");
            }
            else
            {
                ddlSubType.Enabled = false;
            }
        }
        catch
        {
        }
    }

    public void loadNewSubType()
    {
        int sem = 0;
        int.TryParse(ddlduration.SelectedItem.ToString(), out sem);
        string semester = (sem).ToString();
        int currentSem = 0;
        int.TryParse(semester, out currentSem);
        ddlNewSub.Items.Clear();

        strquery = "select distinct ss.subType_no,ss.subject_type from subject s,sub_sem ss,syllabus_master sm where sm.syll_code=ss.syll_code and s.syll_code=ss.syll_code and sm.syll_code=s.syll_code and s.subType_no=ss.subType_no and sm.Batch_Year=" + ddlbatch.SelectedValue.ToString() + " and sm.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and sm.semester='" + (currentSem) + "' order by ss.subject_type";

        ds = dacc.select_method_wo_parameter(strquery, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlNewSubType.Enabled = true;
            ddlNewSubType.DataSource = ds;
            ddlNewSubType.DataTextField = "subject_type";
            ddlNewSubType.DataValueField = "subType_no";
            ddlNewSubType.DataBind();
            ddlNewSubType.Items.Insert(0, "--Select--");
        }
        else
        {
            ddlNewSubType.Enabled = false;
        }
    }

    public void load_Newsubject()
    {
        try
        {
            ddlNewSub.Items.Clear();
            int sem = 0;
            int.TryParse(ddlduration.SelectedItem.ToString(), out sem);
            string semester = (sem).ToString();
            ddlNewSub.Items.Clear();
            //strquery = "select distinct S.subject_no,subject_code,subject_name,sem.subject_type from subject as S,syllabus_master  as SM, subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code=" + ddlbranch.SelectedValue.ToString() + " and SM.Semester=" + ddlduration.SelectedItem.ToString().Trim() + " and  SM.batch_year=" + ddlbatch.SelectedValue.ToString() + " and   S.subtype_no = Sem.subtype_no and promote_count=1  order by subject_code";
            strquery = "select distinct s.subject_no,(s.subject_code+'-'+s.subject_name) as subject_name,ISNULL(s.subjectpriority,'0') subjectpriority from subject s,sub_sem ss,syllabus_master sm where sm.syll_code=ss.syll_code and s.syll_code=ss.syll_code and sm.syll_code=s.syll_code and s.subType_no=ss.subType_no and sm.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and sm.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and sm.semester='" + semester + "' and ss.subject_type='" + Convert.ToString(ddlNewSubType.SelectedItem.Text).Trim() + "'  order by subjectpriority";//and ss.subject_type='" + Convert.ToString(ddlSubType.SelectedItem.Text).Trim() + "'
            ds = dacc.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlNewSub.Enabled = true;
                ddlNewSub.DataSource = ds;
                ddlNewSub.DataTextField = "subject_name";
                ddlNewSub.DataValueField = "subject_no";
                ddlNewSub.DataBind();
                ddlNewSub.Items.Insert(0, "--Select--");
            }
            else
            {
                ddlNewSub.Enabled = false;
            }

        }
        catch
        {
        }
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //bindbatch();
            BindRightsBaseBatch();
            binddegree();
            bindbranch();
            bindsem();
            bindsec();
            load_subjectType();
        }
        catch
        {
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_subjectType();
        load_subject();
        load_Newsubject();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {

        bindbranch();
        bindsem();
        bindsec();
        load_subjectType();
        //load_subject();
        //load_Newsubject();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {

        bindsem();
        bindsec();
        load_subjectType();
        //load_subject();
        //load_Newsubject();

    }

    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {

        bindsec();
        load_subjectType();
        //load_subject();
        //load_Newsubject();

    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        //bindsec();
        load_subjectType();
        //load_subject();
        //load_Newsubject();

    }

    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblsubject.Visible = true;
        ddlsubject.Visible = true;
        //lblhours.Visible = true;
        //DropDownList1.Visible = true;
        loadNewSubType();
        load_Newsubject();
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        btnmove.Visible = true;
        lblsubject.Visible = true;
        ddlsubject.Visible = true;
        lblhours.Visible = true;
        ddlNewSub.Visible = true;
        lblSubTypeNew.Visible = true;
        ddlNewSubType.Visible = true;
        GridView1.Visible = false;
        string currentSemester = Convert.ToString(ddlduration.SelectedItem).Trim();
        int currentSem = 0;
        int.TryParse(currentSemester, out currentSem);
        string sec=string.Empty;
        if(!string.IsNullOrEmpty(Convert.ToString(ddlsec.SelectedValue)))
        {
            sec="  and r.Sections='"+ddlsec.SelectedValue+"'";
        }

        //magesh 20/2/2018
        //strquery = "select distinct s.subject_no,s.subject_name,ISNULL(s.subjectpriority,'0') subjectpriority,sc.roll_no,r.Reg_No,r.Roll_Admit,r.Stud_Name,r.Stud_Type from subject s,sub_sem ss,syllabus_master sm,Registration r,subjectChooser sc where sm.syll_code=ss.syll_code and s.syll_code=ss.syll_code and sm.syll_code=s.syll_code and s.subType_no=ss.subType_no and r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and sm.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and sm.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and sm.semester='" + (currentSem - 1) + "' and ss.subType_no='" + ddlSubType.SelectedValue.ToString() + "' and s.subject_no='" + ddlsubject.SelectedValue.ToString() + "' " + orderByStudents(((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : Convert.ToString(Session["collegecode"]).Trim()), "r");

        strquery = "select distinct s.subject_no,s.subject_name,ISNULL(s.subjectpriority,'0') subjectpriority,r.roll_no,r.Reg_No,r.Roll_Admit,r.Stud_Name,r.Stud_Type from subject s,sub_sem ss,syllabus_master sm,Registration r,subjectChooser sc where sm.syll_code=ss.syll_code and s.syll_code=ss.syll_code and sm.syll_code=s.syll_code and s.subType_no=ss.subType_no and r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and sm.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and sm.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and sm.semester='" + (currentSem - 1) + "'"+sec+"  and ss.subType_no='" + ddlSubType.SelectedValue.ToString() + "' and s.subject_no='" + ddlsubject.SelectedValue.ToString() + "' " + orderByStudents(((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : Convert.ToString(Session["collegecode"]).Trim()), "r");

        //strquery = "select distinct s.subject_no,s.subject_name,ISNULL(s.subjectpriority,'0') subjectpriority,sc.roll_no,r.Reg_No,r.Roll_Admit,r.Stud_Name,r.Stud_Type,s.subject_name from subject s,sub_sem ss,syllabus_master sm,Registration r,subjectChooser sc where sm.syll_code=ss.syll_code and s.syll_code=ss.syll_code and sm.syll_code=s.syll_code and s.subType_no=ss.subType_no and r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and sm.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and sm.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and sm.semester='" + (currentSem - 1) + "' and ss.subType_no='" + ddlSubType.SelectedValue.ToString() + "' and s.subject_no='" + ddlsubject.SelectedValue.ToString() + "' order by r.Reg_No,subjectpriority";
        ds = dacc.select_method_wo_parameter(strquery, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            //sda.Fill(dt);
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();
            GridView1.Visible = true;
            bool isRollNoVisible = ColumnHeaderVisiblity(0);
            bool isRegNoVisible = ColumnHeaderVisiblity(1);
            bool isAdmissionNoVisible = ColumnHeaderVisiblity(2);
            bool isStudentTypeVisible = ColumnHeaderVisiblity(3);
            foreach (GridViewRow row in GridView1.Rows)
            {
                row.Cells[1].Visible = isRollNoVisible;
                row.Cells[2].Visible = isRegNoVisible;
                row.Cells[3].Visible = isAdmissionNoVisible;
                row.Cells[5].Visible = isStudentTypeVisible;
                GridView1.HeaderRow.Cells[1].Visible = isRollNoVisible;
                GridView1.HeaderRow.Cells[2].Visible = isRegNoVisible;
                GridView1.HeaderRow.Cells[3].Visible = isAdmissionNoVisible;
                GridView1.HeaderRow.Cells[5].Visible = isStudentTypeVisible;
            }
        }
        else
        {
            //errmsg.Visible = true;
            //errmsg.Text = "No Record Found";
            lblAlertMsg.Visible = true;
            lblAlertMsg.Text = "No Record Found";
            divPopAlert.Visible = true;
        }

    }

    protected void ddlmovingsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnmove.Visible = true;
        lblsubject.Visible = true;
        ddlsubject.Visible = true;
        lblhours.Visible = true;
        ddlNewSub.Visible = true;
        lblSubTypeNew.Visible =true;
        ddlNewSubType.Visible = true;
        GridView1.Visible = true;

    }

    protected void ddlSubType_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblSubTypeNew.Visible = false;
        ddlNewSubType.Visible = false;
        lblsubject.Visible = true;
        ddlsubject.Visible = true;
        //lblhours.Visible = true;
        //DropDownList1.Visible = true;
        load_subject();
        loadNewSubType();
        load_Newsubject();
    }

    protected void ddlNewSubType_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnmove.Visible = true;
        lblsubject.Visible = true;
        ddlsubject.Visible = true;
        lblhours.Visible = true;
        ddlNewSub.Visible = true;
        lblSubTypeNew.Visible = true;
        ddlNewSubType.Visible = true;
        GridView1.Visible = true;
        load_Newsubject();
    }

    protected void btnMove_Click(object sender, EventArgs e)
    {
        GridView1.Visible = true;
        ddlNewSub.Visible = true;
        lblhours.Visible = true;
        btnmove.Visible = true;
        lblSubTypeNew.Visible = true;
        ddlNewSubType.Visible = true;
        int ins = 0;
        bool isSaved = false;
        if (ddlsubject.SelectedValue.ToString() != null && ddlSubType.SelectedValue.ToString() != null)
        {
            //string subjectTypeNo = d2.GetFunctionv("select subType_NO FROM SUBJECT WHERE subject_no='" + Convert.ToString(ddlNewSub.SelectedValue).Trim() + "'");
            string subjectTypeNo = Convert.ToString(ddlNewSubType.SelectedValue);
            foreach (GridViewRow row in GridView1.Rows)
            {
                string paperorder = string.Empty;
                string batch = string.Empty;
                string grpCell = "0";
                int sem = 0;
                int.TryParse(ddlduration.SelectedItem.ToString(), out sem);
                string semester = (sem + 1).ToString();
                Hashtable hat = new Hashtable();
                hat.Add("roll_no", Convert.ToString(row.Cells[1].Text));
                hat.Add("semester", ddlduration.SelectedItem.ToString());
                hat.Add("subject_no", ddlNewSub.SelectedValue.ToString());
                hat.Add("subtype_no", subjectTypeNo);
                hat.Add("paper_order", paperorder);
                hat.Add("batch", batch);
                hat.Add("grp_cell", grpCell);
                //ds = . ("sp_ins_upd_Subjectchooser",hat, "sp");
                ins = d2.insert_method("sp_ins_upd_Subjectchooser", hat, "sp");
                if (ins != 0)
                {
                    isSaved = true;
                }
            }

            //foreach (GridViewRow row in GridView1.Rows)
            //{              
            //   
            //    string rollNo = Convert.ToString(row.Cells[1].Text);
            //    string sem = ddlduration.SelectedItem.ToString();
            //    string subjectNo = ddlNewSub.SelectedValue.ToString();
            //    string subjectType = ddlduration.SelectedItem.ToString();
            //    int paperorder = Convert.ToInt32(null);
            //    int groupCell = Convert.ToInt32(null);
            //    int batch = Convert.ToInt32(null);
            //    string subchooser = string.Empty;
            //    subchooser = " if exists(select * from Subjectchooser where roll_no='" + rollNo + "' and subject_no='" + subjectNo + "' and semester='" + sem + "' and subtype_no='" + subjectType + "')  update subjectChooser set paper_order='" + paperorder + "',grp_cell='" + groupCell + "' where roll_no='" + rollNo + "' and subject_no='" + subjectNo + "' and semester ='" + sem + "' and subtype_no='" + subjectType + "' Else insert into Subjectchooser(semester,roll_no,subject_no,paper_order,subtype_no,batch,grp_cell) values('" + sem + "','" + rollNo + "','" + subjectNo + "','" + paperorder + "','" + subjectType + "','" + groupCell + "','" + batch + "')";
            //    res = dir.insertData(subchooser);
            //    if (res != 0)
            //    {
            //        isSaved = true;
            //    }
            //}

        }
        lblAlertMsg.Visible = true;
        lblAlertMsg.Text = ((isSaved) ? "Subject Alloted Successfully" : "Subject Not Alloted");
        divPopAlert.Visible = true;
    }

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

}
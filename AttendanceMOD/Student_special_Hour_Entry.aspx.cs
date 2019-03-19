using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using InsproDataAccess;

public partial class Student_special_Hour_Entry : System.Web.UI.Page
{

    #region Decalaration

    SqlConnection dc_con = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection dc_con1 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection mysql1 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    InsproDataAccess.InsproDirectAccess dir = new InsproDataAccess.InsproDirectAccess();
    SqlCommand cmd = new SqlCommand();
    //SqlCommand cmd_sem_shed;

    Hashtable hat = new Hashtable();

    Hashtable htSubjectType = new Hashtable();
    DataSet ds_attndmaster = new DataSet();
    DataSet ds1 = new DataSet();
    //DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DAccess2 d2 = new DAccess2();
    DAccess2 dacces2 = new DAccess2();
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();


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
    string strdayflag;
    string regularflag = string.Empty;
    string genderflag = string.Empty;
    string staffcode = string.Empty;
    string Att_mark = string.Empty;
    string roll_indiv = string.Empty;
    string usercode = string.Empty;
    string qry = string.Empty;

    #endregion

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
            lblErrMsg.Text = Convert.ToString(ex);
            lblErrMsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrMsg.Text = string.Empty;
            lblErrMsg.Visible = false;

        }
        catch (Exception ex)
        {
            lblErrMsg.Text = Convert.ToString(ex);
            lblErrMsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public DataSet Bind_Degree(string college_code, string user_code)
    {
        DataSet ds = new DataSet();
        single_user = GetFunction("select singleuser from usermaster where user_code='" + user_code + "'");
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        if (single_user == "1" || single_user == "true" || single_user == "TRUE" || single_user == "True")
        {
            SqlCommand cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }
        else
        {
            // group_code = GetFunction("select group_code from usermaster where user_code="+user_code+"");
            if (group_code.Trim() != "")
            {
                SqlCommand cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_code + "", dcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
        }
        return ds;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (IsPostBack)
        //{
        //    divMainContent.Visible = false;
        //}
        //divMainContent.Visible = false;
        lblset.Visible = false;
        //btnSave.Visible = false;

        if (Session["collegecode"] == null) //Aruna For Back Button
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
            divMainContent.Visible = false;
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtFromDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
            //TxtToDate.Attributes.Add("readonly", "readonly");


            //TxtToDate.Text = DateTime.Today.ToString("dd-MM-yyyy");

            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0] + "'";
            }
            else
            {
                grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }

            BindCollege();
            qry = " select distinct batch_year from Registration where batch_year<>'-1' and CC=0 and DelFlag=0 and Exam_Flag<>'debar'order by batch_year desc";
            DataSet ds1 = new DataSet();
            ds1 = d2.select_method_wo_parameter(qry, "text");
            ddlbatch.DataSource = ds1;
            ddlbatch.DataValueField = "batch_year";
            ddlbatch.DataBind();
            //ddlBatch.Items.Insert(0, new ListItem("--Select--", "-1"));
            string batchcount = ddlbatch.Items.Count.ToString();
            int batch = 0;
            if (int.TryParse(batchcount, out batch))
                batch = batch - 1;
            ddlbatch.SelectedIndex = batch;
            //course
            con.Open();
            string collegecode = Session["collegecode"].ToString();
            string usercode = Session["usercode"].ToString();
            DataSet ds = Bind_Degree(collegecode.ToString(), usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataValueField = "course_id";
                ddldegree.DataTextField = "course_name";
                ddldegree.DataBind();
                //ddlDegree.Items.Insert(0, new ListItem("--Select--", "-1"));
            }
            //cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + ddldegree.SelectedValue.ToString() + " and degree.college_code= " + Session["collegecode"] + " ", con);
            //SqlDataAdapter daBRANCH = new SqlDataAdapter(cmd);
            //DataSet dsbranch = new DataSet();
            //daBRANCH.Fill(dsbranch);
            string course_id = ddldegree.SelectedValue.ToString();
            if (course_id != null && course_id != "")
            {
                DataSet dsbranch = Bind_Dept(course_id, collegecode, usercode);
                ddlbranch.DataSource = dsbranch;
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataBind();
                Btngo.Enabled = true;
                //bind semester
                bindsem();
                //bind section
                // BindSectionDetail();
            }
            else
            {
                Btngo.Enabled = false;
                lblset.Visible = true;
                //ddl_select_subj.Visible = false;
                //lbl_subj_select.Visible = false;
                lblset.Text = "No Degree Rights For This User";
            }
        }
    }

    public void bindsem()
    {
        //--------------------semester load
        ddlsem.Items.Clear();
        bool first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Close();
        con.Open();
        SqlDataReader dr;
        cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
        dr = cmd.ExecuteReader();
        dr.Read();
        if (dr.HasRows == true)
        {
            first_year = Convert.ToBoolean(dr[1].ToString());
            duration = Convert.ToInt16(dr[0].ToString());
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
            dr.Close();
            SqlDataReader dr1;
            con.Close();
            con.Open();
            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
            ddlsem.Items.Clear();
            dr1 = cmd.ExecuteReader();
            dr1.Read();
            if (dr1.HasRows == true)
            {
                first_year = Convert.ToBoolean(dr1[1].ToString());
                duration = Convert.ToInt16(dr1[0].ToString());
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
            dr1.Close();
        }
        if (ddlsem.Items.Count > 0)
        {
            ddlsem.SelectedIndex = 0;
            BindSectionDetail();
        }
        //FpMarkEntry.Visible = false;
        con.Close();
    }

    public void BindSectionDetail()
    {

        string branch = ddlbranch.SelectedValue.ToString();
        string batch = ddlbatch.SelectedValue.ToString();
        con.Close();
        con.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlsec.DataSource = ds;
            ddlsec.DataTextField = "sections";
            ddlsec.DataValueField = "sections";
            ddlsec.DataBind();
        }
        // ddlsec.Items.Insert(0, "All");            //removed by prabha 23/9/2017
        //end*//
        SqlDataReader dr_sec;
        dr_sec = cmd.ExecuteReader();
        dr_sec.Read();
        if (dr_sec.HasRows == true)
        {
            if (dr_sec["sections"].ToString() == "")
            {
                ddlsec.Enabled = false;
            }
            else
            {
                ddlsec.Enabled = true;
            }
        }
        else
        {
            ddlsec.Enabled = false;
        }
    }

    public DataSet Bind_Dept(string degree_code, string college_code, string user_code)
    {
        //SqlCommand cmd;
        //DataSet ds = new DataSet();
        //SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        //dcon.Close();
        //dcon.Open();
        //if (single_user == "1" || single_user == "true" || single_user == "TRUE" || single_user == "True")
        //{
        //     cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
        //     SqlDataAdapter da = new SqlDataAdapter(cmd);           
        //     da.Fill(ds);
        //}
        //else
        //{
        //    if (group_code.Trim() != "")
        //    {
        //        cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_code + "", dcon);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);             
        //        da.Fill(ds);
        //    }
        //}
        //return ds;
        hat.Clear();
        string usercode = Session["usercode"].ToString();
        string collegecode = Session["collegecode"].ToString();
        string singleuser = Session["single_user"].ToString();
        hat.Add("single_user", singleuser);
        hat.Add("group_code", group_code);
        hat.Add("course_id", ddldegree.SelectedValue);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds = d2.select_method("bind_branch", hat, "sp");
        return ds;
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  load_spread();
        divMainContent.Visible = false;
        string course_id = ddldegree.SelectedValue.ToString();
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["UserCode"].ToString();
        //if (ddldegree.SelectedIndex > 0)
        //{
        //    DataSet ds = ClsAttendanceAccess.GetBranchDetail(course_id.ToString(), collegecode.ToString());
        //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        ddlbranch.DataSource = ds;
        //        ddlbranch.DataTextField = "Dept_Name";
        //        ddlbranch.DataValueField = "degree_code";
        //        ddlbranch.DataBind();
        //       // ddlbranch.Items.Insert(0, new ListItem("--Select--", "-1"));
        //    }
        //}
        con.Open();
        //cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + ddldegree.SelectedValue.ToString() + " and degree.college_code= " + Session["collegecode"] + " ", con);
        //SqlDataAdapter daBRANCH = new SqlDataAdapter(cmd);
        //DataSet dsbranch = new DataSet();
        //daBRANCH.Fill(dsbranch);
        //string course_id = ddlDegree.SelectedValue.ToString();
        ddlbranch.Items.Clear();
        if (course_id != null && course_id != "")
        {
            DataSet dsbranch = Bind_Dept(course_id, collegecode, usercode);
            ddlbranch.DataSource = dsbranch;
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataBind();
            con.Close();
            Btngo.Enabled = true;
            bindsem();
            //bind section
            BindSectionDetail();
        }
        else
        {
            Btngo.Enabled = false;
            lblset.Visible = true;
            //ddl_select_subj.Visible = false;
            //lbl_subj_select.Visible = false;
            lblset.Text = "No Degree Rights For This User";
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //lblnorec.Visible = false;
        //  load_spread();
        divMainContent.Visible = false;
        bindsem();
        if (!Page.IsPostBack == false)
        {
            ddlsem.Items.Clear();
        }
        try
        {
            //if ((ddlBranch.SelectedIndex != 0) && (ddlBranch.SelectedIndex > 0))
            //{
            bindsem();
            //}
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        divMainContent.Visible = false;
        if (!Page.IsPostBack == false)
        {
            ddlsec.Items.Clear();
        }
        Btngo.Visible = true;
        //btnok.Visible = false;
        BindSectionDetail();
        string collegecode = Convert.ToString(Session["collegecode"]).Trim();
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        divMainContent.Visible = false;
        //BindSectionDetail();
    }

    protected void GridView1_RowCommand(object sender, EventArgs e)
    {

    }

    protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        divMainContent.Visible = true;
        string qry = string.Empty;
        string subject_No = string.Empty;
        string sem = ddlsem.SelectedItem.Value.ToString();
        string degree = ddlbranch.SelectedItem.Value.ToString();
        string batch = ddlbatch.SelectedItem.Value.ToString();
        string selsec = string.Empty;
        string qrySec = string.Empty;
        DataTable dt;
        if (ddlsec.Items.Count > 0)
        {
            selsec = ddlsec.SelectedItem.Value.ToString();
            if (Convert.ToString(ddlsec.SelectedValue).Trim() == "" || Convert.ToString(ddlsec.SelectedValue).Trim().ToLower() == "all" || Convert.ToString(ddlsec.SelectedValue).Trim() == "-1")
            {

            }
            else
            {
                qrySec = " and LTRIM(RTRIM(ISNULL(ss.Sections,'')))='" + selsec + "'";
            }
        }
        foreach (GridViewRow row in GridView1.Rows)
        {
            DropDownList ddlSubjectNew = (DropDownList)row.FindControl("ddlSubject");
            DropDownList ddlStaffNew = (DropDownList)row.FindControl("ddlStaff");

            subject_No = ddlSubjectNew.SelectedValue.ToString().Trim();
            string data_bind = "select distinct sfm.staff_name,ss.staff_code,s.subject_name,s.subject_no from subject s,syllabus_master sm,staff_selector ss,staffmaster sfm where sm.syll_code=s.syll_code and sfm.staff_code=ss.staff_code and s.subject_no=ss.subject_no and s.subject_no='" + subject_No + "' and sm.Batch_Year='" + batch + "' and sm.degree_code='" + degree + "' and sm.semester='" + sem + "'" + qrySec;

            dt = dir.selectDataTable(data_bind);
            ddlStaffNew.DataSource = dt;
            ddlStaffNew.DataTextField = "staff_name";
            ddlStaffNew.DataValueField = "staff_code";
            ddlStaffNew.DataBind();

        }
    }

    protected void TextBox2_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string fromtime = string.Empty;
            string totime = string.Empty;
            TextBox tb1, tb2;
            string[] frm = new string[5];
            string[] to = new string[5];
            foreach (GridViewRow row in GridView1.Rows)
            {
                tb1 = (TextBox)row.FindControl("TextBox1");
                fromtime = tb1.Text;
                tb2 = (TextBox)row.FindControl("TextBox2");
                totime = tb2.Text;
                frm = fromtime.Split(':');
                to = totime.Split(':');
                if (!string.IsNullOrEmpty(totime) && !string.IsNullOrEmpty(fromtime))
                {
                    Label lblerror = (Label)row.FindControl("err");
                    if (Convert.ToInt32(frm[0]) <= Convert.ToInt32(to[0]))
                    {
                        if (Convert.ToInt32(frm[0]) == Convert.ToInt32(to[0]))
                        {
                            if (Convert.ToInt32(frm[1]) >= 0 && Convert.ToInt32(frm[1]) < Convert.ToInt32(to[1]))
                            {
                                lblerror.Visible = false;
                            }
                            else
                            {
                                divPopAlert.Visible = true;
                                lblAlertMsg.Text = "Please Enter the End time greater than beginning time";
                                lblAlertMsg.Visible = true;
                            }
                        }
                        else
                        {
                            lblerror.Visible = false;
                        }
                    }
                }
                else
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Please Enter the End time greater than beginning time";
                    lblAlertMsg.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }

    }

    public string GetFunction(string Att_strqueryst)
    {
        string sqlstr;
        sqlstr = Att_strqueryst;
        getsql.Close();
        getsql.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, getsql);
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr);
        cmd.Connection = getsql;
        drnew = cmd.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }
    }

    protected void Btngo_Click(object sender, EventArgs e)
    {
        divMainContent.Visible = false;
        this.BindData();
    }

    private void BindData()
    {
        divMainContent.Visible = false;
        string sem = ddlsem.SelectedItem.Value.ToString();
        string degree = ddlbranch.SelectedItem.Value.ToString();
        string batch = ddlbatch.SelectedItem.Value.ToString();
        string selsec = string.Empty;
        string date1;
        string datefrom = string.Empty;
        date1 = txtFromDate.Text.ToString();
        //string[] split = date1.Split(new Char[] { '-' });
        //datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
        DateTime dt1 = new DateTime();// Convert.ToDateTime(datefrom.ToString());
        bool isValid = DateTime.TryParseExact(date1, "dd-MM-yyyy", null, DateTimeStyles.None, out dt1);
        DataSet ds_splhr_query_master = new DataSet();
        string qrySec = string.Empty;
        if (isValid)
        {

        }

        if (ddlsec.Items.Count > 0)
        {
            selsec = ddlsec.SelectedItem.Value.ToString();
            if (Convert.ToString(selsec).Trim() == "" || Convert.ToString(selsec).Trim().ToLower() == "all" || Convert.ToString(selsec).Trim() == "-1")
            {
            }
            else
            {
                qrySec = " and LTRIM(RTRIM(ISNULL(ss.Sections,'')))='" + selsec + "' and LTRIM(RTRIM(ISNULL(sphrm.Sections,'')))='" + selsec + "'";
            }
        }
        if (!string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(degree) && !string.IsNullOrEmpty(sem) && isValid)
        {
            string splhr_query_master = "select distinct s.subject_name,s.subject_no,sfm.staff_name,ss.staff_code,Convert(varchar(5),spHr.start_time,108) as start_time,Convert(varchar(5),spHr.end_time,108) as end_time,spHr.topic_no, spHrm.hrentry_no,sphrm.date,spHr.hrdet_no from specialhr_details spHr,specialhr_master sphrm,subject s,syllabus_master sm,staff_selector ss,staffmaster sfm where sphrm.hrentry_no=spHr.hrentry_no and  s.subject_no=spHr.subject_no and ss.staff_code=spHr.staff_code and sm.syll_code=s.syll_code and spHr.subject_no=ss.subject_no and sfm.staff_code=ss.staff_code and sfm.staff_code=spHr.staff_code and sm.Batch_Year='" + batch + "' and sm.degree_code='" + degree + "' and sm.semester='" + sem + "' and sphrm.date='" + dt1.ToString("MM/dd/yyyy") + "' " + qrySec;
            ds_splhr_query_master = d2.select_method_wo_parameter(splhr_query_master, "text");
        }
        if (ds_splhr_query_master.Tables.Count > 0 && ds_splhr_query_master.Tables[0].Rows.Count > 0)
        {
            divMainContent.Visible = true;
            GridView1.DataSource = ds_splhr_query_master;
            GridView1.DataBind();
            ViewState["CurrentTable"] = ds_splhr_query_master.Tables[0];
        }
        else
        {
            divMainContent.Visible = true;
            GridView1.DataSource = bindSettingGrid();
            GridView1.DataBind();
            ViewState["CurrentTable"] = bindSettingGrid();
            object sender = new object();
            EventArgs e = new EventArgs();
            ddlSubject_SelectedIndexChanged(sender, e);
            foreach (GridViewRow row in GridView1.Rows)
            {
                LinkButton linkbtn = row.FindControl("lnkdelete") as LinkButton;
                linkbtn.Enabled = false;
            }
        }
    }

    protected DataTable bindSettingGrid()
    {
        DataTable dtSetting = new DataTable();
        //dtSetting.Columns.Add("Sno");
        dtSetting.Columns.Add("subject_name");
        dtSetting.Columns.Add("subject_no");
        dtSetting.Columns.Add("staff_name");
        dtSetting.Columns.Add("staff_code");
        dtSetting.Columns.Add("start_time");
        dtSetting.Columns.Add("end_time");
        dtSetting.Columns.Add("topic_no");
        dtSetting.Columns.Add("hrentry_no");
        dtSetting.Columns.Add("date");
        dtSetting.Columns.Add("hrdet_no");

        try
        {
            ArrayList addnew = new ArrayList();
            addnew.Add("1");
            DataRow dr;
            for (int row = 0; row < addnew.Count; row++)
            {
                dr = dtSetting.NewRow();
                //dr["start_time"] = "HH:MM";
                //dr["end_time"] = "HH:MM";
                dtSetting.Rows.Add(dr);
            }
        }
        catch { }
        return dtSetting;
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        divMainContent.Visible = false;
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        //pHeaderatendence.Visible = false;
        //pBodyatendence.Visible = false;
        divMainContent.Visible = false;
        string date1 = string.Empty;
        string datefrom = string.Empty;
        lblfromdate.Visible = false;
        lbltodate.Visible = false;
        if (txtFromDate.Text == "")
        {
            lblfromdate.Text = "Select From Date";
            lblfromdate.Visible = true;
            return;
        }
        date1 = txtFromDate.Text.ToString();
        string[] split = date1.Split(new Char[] { '-' });
        datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
        DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
        if (dt1 > DateTime.Today)
        {
            //lblset.Visible = true;
            ////ddl_select_subj.Visible = false;
            ////lbl_subj_select.Visible = false;
            //lblset.Text = "You can not mark attendance for the date greater than today";
            ////txtFromDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
            //return;
        }
        else
        {
            lblset.Visible = false;
        }
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string staffcode = string.Empty;
        foreach (GridViewRow row in GridView1.Rows)
        {
            DropDownList ddlstaffnew = (DropDownList)row.FindControl("ddlSubject");
            staffcode = ddlstaffnew.SelectedValue.ToString().Trim();


        }
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            string sem = ddlsem.SelectedItem.Value.ToString();
            string degree = ddlbranch.SelectedItem.Value.ToString();
            string batch = ddlbatch.SelectedItem.Value.ToString();
            string selsec = string.Empty;
            DataSet ds_splhr_staff = new DataSet();
            DataSet ds_splhr_subject = new DataSet();
            DataSet ds_bind = new DataSet();
            string qrySec = string.Empty;
            if (ddlsec.Items.Count > 0)
            {
                selsec = ddlsec.SelectedItem.Value.ToString();
                if (Convert.ToString(ddlsec.SelectedValue).Trim() == "" || Convert.ToString(ddlsec.SelectedValue).Trim().ToLower() == "all" || Convert.ToString(ddlsec.SelectedValue).Trim() == "-1")
                {
                }
                else
                {
                    qrySec = " and LTRIM(RTRIM(ISNULL(ss.Sections,'')))='" + selsec + "'";
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList Staff = (e.Row.FindControl("ddlStaff") as DropDownList);
                DropDownList Subject = (e.Row.FindControl("ddlSubject") as DropDownList);//
                Label lblentryNo = (e.Row.FindControl("lblentryNo") as Label);//
                Staff.Enabled = true;
                Subject.Enabled = true;
                Label lblSubjectDet = (e.Row.FindControl("lblSubject") as Label);
                Label lblStaffDet = (e.Row.FindControl("lblStaff") as Label);
                Label hrdet_no = (e.Row.FindControl("lblDetNo") as Label);

                //string data_bind = "select distinct sfm.staff_name,ss.staff_code,s.subject_name,s.subject_no from subject s,syllabus_master sm,staff_selector ss,staffmaster sfm where sm.syll_code=s.syll_code and sfm.staff_code=ss.staff_code and s.subject_no=ss.subject_no and sm.Batch_Year='" + batch + "' and sm.degree_code='" + degree + "' and sm.semester='" + sem + "'" + qrySec;

                string data_bind = "select distinct sfm.staff_name,sf.staff_code,s.subject_no,s.subject_code,s.subject_name from subject s, syllabus_master sm,sub_sem ss,staffmaster sfm,subjectChooser sc,staff_selector sf where sc.subject_no=s.subject_no and sf.subject_no=s.subject_no and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and sfm.staff_code=sf.staff_code and ss.promote_count=1 and sm.Batch_Year='" + batch + "' and sm.degree_code='" + degree + "' and sm.semester='" + sem + "'";
                ds_bind = d2.select_method_wo_parameter(data_bind, "text");

                if (ds_bind.Tables.Count > 0 && ds_bind.Tables[0].Rows.Count > 0)
                {
                    DataTable dtSubject = new DataTable();
                    DataTable dtStaff = new DataTable();
                    dtSubject = ds_bind.Tables[0].DefaultView.ToTable(true, "subject_name", "subject_no");

                    if (dtSubject.Rows.Count > 0)
                    {
                        Subject.DataSource = dtSubject;
                        Subject.DataTextField = "subject_name";
                        Subject.DataValueField = "subject_no";
                        Subject.DataBind();
                        if (string.IsNullOrWhiteSpace(lblSubjectDet.Text.Trim()))
                        {
                            lblSubjectDet.Text = Subject.SelectedValue.Trim();
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(lblSubjectDet.Text.Trim()))
                    {
                        Subject.Items.FindByValue(lblSubjectDet.Text.Trim()).Selected = true;

                        ds_bind.Tables[0].DefaultView.RowFilter = "subject_no='" + lblSubjectDet.Text.Trim() + "'";
                        dtStaff = ds_bind.Tables[0].DefaultView.ToTable(true, "staff_name", "staff_code");
                    }
                    if (dtStaff.Rows.Count > 0)
                    {
                        Staff.DataSource = dtStaff;
                        Staff.DataTextField = "staff_name";
                        Staff.DataValueField = "staff_code";
                        Staff.DataBind();
                        if (string.IsNullOrWhiteSpace(lblStaffDet.Text.Trim()))
                        {
                            lblSubjectDet.Text = Staff.SelectedValue.Trim();
                        }
                        else
                        {
                            Staff.Items.FindByValue(lblStaffDet.Text.Trim()).Selected = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(lblentryNo.Text.Trim()))
                    {
                        Subject.Enabled = false;
                        Staff.Enabled = false;
                    }
                }
            }
        }
        catch
        {
        }

    }

    protected void addnewrow(object sender, EventArgs e)
    {
        AddNewRowToGrid();
    }

    private void AddNewRowToGrid()
    {
        try
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                TextBox box1 = new TextBox();
                TextBox box2 = new TextBox();
                TextBox box3 = new TextBox();
                Label lblentryNo = new Label();
                Label hrdet = new Label();
                DropDownList staff = new DropDownList();
                DropDownList subject = new DropDownList();
                //dtSetting.Columns.Add("Sno");
                //dtSetting.Columns.Add("staff_code");
                //dtSetting.Columns.Add("subject_no");
                //dtSetting.Columns.Add("start_time");
                //dtSetting.Columns.Add("end_time");
                //dtSetting.Columns.Add("topic_no");
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        //hrdet = (Label)GridView1.Rows[i].Cells[0].FindControl("Label1");
                        staff = (DropDownList)GridView1.Rows[i].Cells[2].FindControl("ddlStaff");
                        subject = (DropDownList)GridView1.Rows[i].Cells[1].FindControl("ddlSubject");
                        box1 = (TextBox)GridView1.Rows[i].Cells[3].FindControl("TextBox1");
                        box2 = (TextBox)GridView1.Rows[i].Cells[4].FindControl("TextBox2");
                        box3 = (TextBox)GridView1.Rows[i].Cells[5].FindControl("TextBox3");

                        hrdet = (Label)GridView1.Rows[i].Cells[0].FindControl("lblDetNo");
                        lblentryNo = (Label)GridView1.Rows[i].Cells[0].FindControl("lblentryNo");
                        drCurrentRow = dtCurrentTable.NewRow();

                        //dtCurrentTable.Rows[i]["Sno"] = Convert.ToString((i + 1)).Trim();
                        //dtCurrentTable.Rows[i]["hrdet_no"] = Label1.Text;
                        dtCurrentTable.Rows[i]["subject_name"] = Convert.ToString(subject.SelectedItem.Text).Trim();
                        dtCurrentTable.Rows[i]["subject_no"] = Convert.ToString(subject.SelectedValue).Trim();
                        dtCurrentTable.Rows[i]["staff_name"] = Convert.ToString(staff.SelectedItem.Text).Trim();
                        dtCurrentTable.Rows[i]["staff_code"] = Convert.ToString(staff.SelectedValue).Trim();
                        dtCurrentTable.Rows[i]["start_time"] = box1.Text;
                        dtCurrentTable.Rows[i]["end_time"] = box2.Text;
                        dtCurrentTable.Rows[i]["topic_no"] = box3.Text;
                        dtCurrentTable.Rows[i]["hrentry_no"] = (string.IsNullOrEmpty(lblentryNo.Text) ? "0" : lblentryNo.Text);
                        dtCurrentTable.Rows[i]["hrdet_no"] = (string.IsNullOrEmpty(hrdet.Text) ? "0" : hrdet.Text);

                        rowIndex++;
                    }

                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;
                    GridView1.DataSource = dtCurrentTable;
                    GridView1.DataBind();

                    foreach (GridViewRow row in GridView1.Rows)
                    {
                        LinkButton linkbtn = row.FindControl("lnkdelete") as LinkButton;
                        Label hrdetNEW = row.FindControl("lblDetNo") as Label;
                        Label lblentryNoNEW = row.FindControl("lblentryNo") as Label;
                        if (!string.IsNullOrEmpty(hrdetNEW.Text) || !string.IsNullOrEmpty(lblentryNoNEW.Text))
                            linkbtn.Enabled = true;
                        else
                            linkbtn.Enabled = false;
                    }
                }
                object sender = new object();
                EventArgs e = new EventArgs();
                ddlSubject_SelectedIndexChanged(sender, e);
            }
            else
            {
                GridView1.DataSource = bindSettingGrid();
                GridView1.DataBind();
            }
        }
        catch (Exception)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Unable to add row ')", true);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            bool isSave = false;
            bool isSelectedRow = false;
            string sem = ddlsem.SelectedItem.Value.ToString();
            string degree = ddlbranch.SelectedItem.Value.ToString();
            string batch = ddlbatch.SelectedItem.Value.ToString();
            string selsec = string.Empty;
            string date1;
            string datefrom = string.Empty;
            date1 = txtFromDate.Text.ToString();
            DateTime dt1 = new DateTime();// Convert.ToDateTime(datefrom.ToString());
            bool isValidDate = DateTime.TryParseExact(date1, "dd-MM-yyyy", null, DateTimeStyles.None, out dt1);
            DataSet ds_splhr_query_master = new DataSet();

            string qrySec = string.Empty;
            if (ddlsec.Items.Count > 0)
            {
                selsec = ddlsec.SelectedItem.Value.ToString();
                if (Convert.ToString(ddlsec.SelectedValue).Trim() == "" || Convert.ToString(ddlsec.SelectedValue).Trim().ToLower() == "all" || Convert.ToString(ddlsec.SelectedValue).Trim() == "-1")
                {
                    foreach (ListItem li in ddlsec.Items)
                    {
                        string sec = li.Text.Trim();
                        if (!string.IsNullOrEmpty(Convert.ToString(sec).Trim()) && Convert.ToString(sec).Trim().ToLower() != "all" && Convert.ToString(sec).Trim() != "-1")
                        {
                            string splhr_query_master = "if not exists(select hrentry_no from specialhr_master where Batch_Year='" + batch + "' and degree_code='" + degree + "' and semester='" + sem + "' and date='" + dt1.ToString("MM/dd/yyyy") + "' and LTRIM(RTRIM(ISNULL(Sections,'')))='" + sec + "') insert into specialhr_master(degree_code,semester,Batch_Year,date,sections) values('" + degree + "','" + sem + "','" + batch + "','" + dt1.ToString("MM/dd/yyyy") + "','" + sec + "')";
                            int result = d2.update_method_wo_parameter(splhr_query_master, "text");

                            //string isselect = "select hrentry_no from specialhr_master where Batch_Year='" + batch + "' and degree_code='" + degree + "' and semester='" + sem + "' and date='" + dt1.ToString("MM/dd/yyyy") + "' and LTRIM(RTRIM(ISNULL(Sections,'')))='" + sec + "'";
                            //string val = dir.selectScalarString(isselect);
                        }
                    }
                }
                else
                {
                    qrySec = " and LTRIM(RTRIM(ISNULL(Sections,'')))='" + selsec + "'";
                    string splhr_query_master = "if not exists(select hrentry_no from specialhr_master where Batch_Year='" + batch + "' and degree_code='" + degree + "' and semester='" + sem + "' and date='" + dt1.ToString("MM/dd/yyyy") + "' " + qrySec + ") insert into specialhr_master(degree_code,semester,Batch_Year,date,sections) values('" + degree + "','" + sem + "','" + batch + "','" + dt1.ToString("MM/dd/yyyy") + "','" + selsec + "')";
                    int result = d2.update_method_wo_parameter(splhr_query_master, "text");

                    //string isselect = "select hrentry_no from specialhr_master where Batch_Year='" + batch + "' and degree_code='" + degree + "' and semester='" + sem + "' and date='" + dt1.ToString("MM/dd/yyyy") + "' " + qrySec + "";
                    //string val = dir.selectScalarString(isselect);
                }
            }
            else
            {
                //qrySec = " and LTRIM(RTRIM(ISNULL(Sections,'')))='" + selsec + "'";
                qrySec = string.Empty;
                string splhr_query_master = "if not exists(select hrentry_no from specialhr_master where Batch_Year='" + batch + "' and degree_code='" + degree + "' and semester='" + sem + "' and date='" + dt1.ToString("MM/dd/yyyy") + "' " + qrySec + ") insert into specialhr_master(degree_code,semester,Batch_Year,date,sections) values('" + degree + "','" + sem + "','" + batch + "','" + dt1.ToString("MM/dd/yyyy") + "','" + selsec + "')";
                int result = d2.update_method_wo_parameter(splhr_query_master, "text");

                //string isselect = "select hrentry_no from specialhr_master where Batch_Year='" + batch + "' and degree_code='" + degree + "' and semester='" + sem + "' and date='" + dt1.ToString("MM/dd/yyyy") + "' " + qrySec + "";
                //string val = dir.selectScalarString(isselect);
            }
            if (isValidDate)
            {
                DataSet ds_splhr_insert = new DataSet();
                DataSet ds_sphr_New = new DataSet();
                DataSet isval = new DataSet();

                foreach (GridViewRow row in GridView1.Rows)
                {
                    string selsubject = (row.FindControl("ddlSubject") as DropDownList).SelectedValue;
                    int subj = Convert.ToInt32(selsubject);
                    string selstaff = (row.FindControl("ddlStaff") as DropDownList).SelectedValue;
                    string starttime = (row.FindControl("TextBox1") as TextBox).Text;


                    string endtime = (row.FindControl("TextBox2") as TextBox).Text;
                    DateTime dtEndTime = new DateTime();
                    DateTime.TryParseExact(endtime, "HH:mm", null, DateTimeStyles.None, out dtEndTime);
                    if (!string.IsNullOrEmpty(starttime) || !string.IsNullOrEmpty(endtime))
                    {
                        DateTime dtStartTime = new DateTime();
                        DateTime.TryParseExact(starttime, "HH:mm", null, DateTimeStyles.None, out dtStartTime);
                        DateTime dtStartDateTime = new DateTime(1, 1, 1, dtStartTime.Hour, dtStartTime.Minute, dtStartTime.Second);

                        DateTime dtEndDateTime = new DateTime(1, 1, 1, dtEndTime.Hour, dtEndTime.Minute, dtEndTime.Second);
                        string topic = (row.FindControl("TextBox3") as TextBox).Text;
                        Label lblentryNo1 = (row.FindControl("lblentryNo") as Label);
                        Label det_no = (row.FindControl("lblDetNo") as Label);
                        string splhr_query_details = string.Empty;
                        int res = 0;
                        bool hasSection = false;
                        if (ddlsec.Items.Count > 0)
                        {
                            selsec = ddlsec.SelectedItem.Value.ToString();
                            if (Convert.ToString(ddlsec.SelectedValue).Trim() == "" || Convert.ToString(ddlsec.SelectedValue).Trim().ToLower() == "all" || Convert.ToString(ddlsec.SelectedValue).Trim() == "-1")
                            {
                                hasSection = true;
                                foreach (ListItem li in ddlsec.Items)
                                {
                                    string sec = li.Text.Trim();
                                    if (!string.IsNullOrEmpty(Convert.ToString(sec).Trim()) && Convert.ToString(sec).Trim().ToLower() != "all" && Convert.ToString(sec).Trim() != "-1")
                                    {
                                        string entryNo = dir.selectScalarString("select hrentry_no from specialhr_master where Batch_Year='" + batch + "' and degree_code='" + degree + "' and semester='" + sem + "' and date='" + dt1.ToString("MM/dd/yyyy") + "' and LTRIM(RTRIM(ISNULL(Sections,'')))='" + sec + "'");
                                        lblentryNo1.Text = entryNo;

                                        if (!string.IsNullOrEmpty(entryNo) && entryNo != "0" && !string.IsNullOrEmpty(det_no.Text.Trim()) && det_no.Text.Trim() != "0")
                                        {
                                            splhr_query_details = "if Exists(select hrentry_no,subject_no,staff_code,start_time,end_time,topic_no from specialhr_details where subject_no='" + subj + "' and staff_code='" + selstaff + "' and hrentry_no='" + entryNo + "' and hrdet_no='" + det_no.Text + "') update specialhr_details SET subject_no='" + subj + "',staff_code='" + selstaff + "',start_time='" + dtStartDateTime.ToString("HH:mm") + "',end_time='" + dtEndDateTime.ToString("HH:mm") + "',topic_no='" + topic + "' where subject_no='" + subj + "' and staff_code='" + selstaff + "'and hrentry_no='" + entryNo + "' and hrdet_no='" + det_no.Text + "'  Else insert into specialhr_details(hrentry_no,subject_no,staff_code,start_time,end_time,topic_no) values('" + entryNo + "','" + subj + "','" + selstaff + "','" + dtStartDateTime.ToString("HH:mm") + "','" + dtEndDateTime.ToString("HH:mm") + "','" + topic + "')";// and hrdet_no='" + det_no + "'
                                            res = dir.insertData(splhr_query_details);
                                        }
                                        else if (!string.IsNullOrEmpty(entryNo) && entryNo != "0")
                                        {
                                            splhr_query_details = " if not Exists(select hrentry_no,subject_no,staff_code,start_time,end_time,topic_no from specialhr_details where subject_no='" + subj + "' and staff_code='" + selstaff + "' and hrentry_no='" + entryNo + "' and Convert(varchar(5),start_time,108)='" + dtStartDateTime.ToString("HH:mm") + "' and Convert(varchar(5),end_time,108)='" + dtEndDateTime.ToString("HH:mm") + "') insert into specialhr_details(hrentry_no,subject_no,staff_code,start_time,end_time,topic_no) values('" + entryNo + "','" + subj + "','" + selstaff + "','" + dtStartDateTime.ToString("HH:mm") + "','" + dtEndDateTime.ToString("HH:mm") + "','" + topic + "')";//and  hrdet_no='" + det_no + "'
                                            res = dir.insertData(splhr_query_details);
                                        }
                                        if (res != 0)
                                        {
                                            isSave = true;
                                        }
                                    }
                                }
                            }
                        }
                        if (!hasSection)
                        {
                            string entryNo = dir.selectScalarString("select hrentry_no from specialhr_master where Batch_Year='" + batch + "' and degree_code='" + degree + "' and semester='" + sem + "' and date='" + dt1.ToString("MM/dd/yyyy") + "' " + qrySec + "");
                            lblentryNo1.Text = entryNo;
                            if (!string.IsNullOrEmpty(entryNo) && entryNo != "0" && !string.IsNullOrEmpty(det_no.Text.Trim()) && det_no.Text.Trim() != "0")
                            {
                                splhr_query_details = "if Exists(select hrentry_no,subject_no,staff_code,start_time,end_time,topic_no from specialhr_details where subject_no='" + subj + "' and staff_code='" + selstaff + "' and hrentry_no='" + entryNo + "' and hrdet_no='" + det_no.Text + "' ) update specialhr_details SET subject_no='" + subj + "',staff_code='" + selstaff + "',start_time='" + dtStartDateTime.ToString("HH:mm") + "',end_time='" + dtEndDateTime.ToString("HH:mm") + "',topic_no='" + topic + "' where hrentry_no='" + entryNo + "' and hrdet_no='" + det_no.Text + "'  Else insert into specialhr_details(hrentry_no,subject_no,staff_code,start_time,end_time,topic_no) values('" + entryNo + "','" + subj + "','" + selstaff + "','" + dtStartDateTime.ToString("HH:mm") + "','" + dtEndDateTime.ToString("HH:mm") + "','" + topic + "')";
                                res = dir.insertData(splhr_query_details);
                            }
                            else if (!string.IsNullOrEmpty(entryNo) && entryNo != "0")
                            {
                                splhr_query_details = "if not Exists(select hrentry_no,subject_no,staff_code,start_time,end_time,topic_no from specialhr_details where subject_no='" + subj + "' and staff_code='" + selstaff + "' and hrentry_no='" + entryNo + "' and Convert(varchar(5),start_time,108)='" + dtStartDateTime.ToString("HH:mm") + "' and Convert(varchar(5),end_time,108)='" + dtEndDateTime.ToString("HH:mm") + "') insert into specialhr_details(hrentry_no,subject_no,staff_code,start_time,end_time,topic_no) values('" + entryNo + "','" + subj + "','" + selstaff + "','" + dtStartDateTime.ToString("HH:mm") + "','" + dtEndDateTime.ToString("HH:mm") + "','" + topic + "')";
                                res = dir.insertData(splhr_query_details);
                            }
                            if (res != 0)
                            {
                                isSave = true;
                            }

                        }
                    }
                }
            }
            if (isSave)
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Saved Successfully";
                divPopAlert.Visible = true;
                Btngo_Click(sender, e);
                return;
            }
            else
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Not Saved";
                divPopAlert.Visible = true;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Special Hour Entry");
        }

    }

    //protected void btndelete_Click(object sender, EventArgs e)
    //{
    //    bool isSelectedRow = false;
    //    bool isdelete = false;
    //    bool nosplhr = false;
    //    foreach (GridViewRow row in GridView1.Rows)
    //    {
    //        int del = 0;
    //        bool isSelected = (row.FindControl("chkRow") as CheckBox).Checked;

    //        isSelectedRow = true;
    //        Label lblentryNo1 = (row.FindControl("lblentryNo") as Label);
    //        Label det_no = (row.FindControl("lblDetNo") as Label);
    //        if (!String.IsNullOrEmpty(det_no.Text))
    //        {
    //            string sqlry = "delete from specialHourStudents where hrdet_no='" + det_no.Text + "';";

    //            sqlry += "delete from specialhr_details where hrdet_no=" + det_no.Text + "";

    //            del = dir.deleteData(sqlry);
    //            if (del > 0)
    //            {
    //                isdelete = true;
    //                TextBox tbfromtym = new TextBox();
    //                tbfromtym = row.FindControl("TextBox1") as TextBox;
    //                tbfromtym.Text = "";
    //                TextBox tbtotym = new TextBox();
    //                tbtotym = row.FindControl("TextBox2") as TextBox;
    //                tbtotym.Text = "";
    //                TextBox tbtopic = new TextBox();
    //                tbtopic = row.FindControl("TextBox3") as TextBox;
    //                tbtopic.Text = "";
    //                CheckBox chk = row.FindControl("chkRow") as CheckBox;
    //                chk.Checked = false;
    //            }
    //            else
    //                nosplhr = true;
    //        }
    //    }
    //     if (isdelete)
    //    {
    //        lblAlertMsg.Visible = true;
    //        lblAlertMsg.Text = "Deleted Successfully";
    //        divPopAlert.Visible = true;
    //        Btngo_Click(sender, e);
    //        return;
    //    }
    //    else
    //    {
    //        lblAlertMsg.Visible = true;
    //        lblAlertMsg.Text = "Special Hour Cannot be Deleted";
    //        divPopAlert.Visible = true;
    //    }
    //}

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

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int ActiveRow = e.RowIndex;
            DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
            DataRow drCurrentRow = null;
            TextBox box1 = new TextBox();
            TextBox box2 = new TextBox();
            TextBox box3 = new TextBox();
            Label lblentryNo = new Label();
            Label hrdet = new Label();
            DropDownList staff = new DropDownList();
            DropDownList subject = new DropDownList();
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                {
                    if (ActiveRow == i)
                    {
                        staff = (DropDownList)GridView1.Rows[i].Cells[2].FindControl("ddlStaff");
                        subject = (DropDownList)GridView1.Rows[i].Cells[1].FindControl("ddlSubject");
                        box1 = (TextBox)GridView1.Rows[i].Cells[3].FindControl("TextBox1");
                        box2 = (TextBox)GridView1.Rows[i].Cells[4].FindControl("TextBox2");
                        box3 = (TextBox)GridView1.Rows[i].Cells[5].FindControl("TextBox3");
                        hrdet = (Label)GridView1.Rows[i].Cells[0].FindControl("lblDetNo");
                        lblentryNo = (Label)GridView1.Rows[i].Cells[0].FindControl("lblentryNo");
                        if (hrdet.Text == "0" || String.IsNullOrEmpty(hrdet.Text) || hrdet.Text == "")
                        {
                            DataBindingNew(ActiveRow);
                        }
                        else
                        {
                            string delAttendance = "select * from specialhr_attendance where  hrdet_no='" + hrdet.Text + "' ";
                            DataTable dtExistatt = new DataTable();
                            dtExistatt = dir.selectDataTable(delAttendance);
                            dtExistatt.DefaultView.RowFilter = "ISNULL(attendance,'0')= 0";
                            DataView dvexistingattendance = dtExistatt.DefaultView;

                            if (dtExistatt.Rows.Count > 0)
                            {
                                if (dvexistingattendance.Count == dtExistatt.Rows.Count)
                                {
                                    string sqlry = "delete from specialHourStudents where hrdet_no='" + hrdet.Text + "';";
                                    sqlry += "delete from specialhr_details where hrdet_no=" + hrdet.Text + "";
                                    int del = dir.deleteData(sqlry);
                                    if (del > 0)
                                    {
                                        box1.Text = string.Empty;
                                        box2.Text = string.Empty;
                                        box3.Text = string.Empty;
                                        staff.Enabled = true;
                                        subject.Enabled = true;
                                        lblAlertMsg.Visible = true;
                                        lblAlertMsg.Text = "Deleted Successfully";
                                        divPopAlert.Visible = true;
                                        DataBindingNew(ActiveRow);
                                    }
                                }
                                else
                                {
                                    lblAlertMsg.Visible = true;
                                    lblAlertMsg.Text = "Attendance has been entered.\n Special Hour Cannot be Deleted";
                                    divPopAlert.Visible = true;
                                }
                            }
                            else
                            {
                                string sqlry = "delete from specialHourStudents where hrdet_no='" + hrdet.Text + "';";
                                sqlry += "delete from specialhr_details where hrdet_no=" + hrdet.Text + "";
                                int del = dir.deleteData(sqlry);
                                if (del > 0)
                                {
                                    box1.Text = string.Empty;
                                    box2.Text = string.Empty;
                                    box3.Text = string.Empty;
                                    staff.Enabled = true;
                                    subject.Enabled = true;
                                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted successfully')", true);
                                    //lblAlertMsg.Visible = true;
                                    //lblAlertMsg.Text = "Deleted Successfully";
                                    //divPopAlert.Visible = true;
                                    DataBindingNew(ActiveRow);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Special Hour Entry");
        }
    }

    protected void DataBindingNew(int rowIndex)
    {
        try
        {
            DataTable dtTemp = (DataTable)ViewState["CurrentTable"];
            dtTemp.Rows.RemoveAt(rowIndex);
            if (dtTemp.Rows.Count > 0)
            {
                GridView1.DataSource = null;
                GridView1.DataSource = dtTemp;
                GridView1.DataBind();

                foreach (GridViewRow row in GridView1.Rows)
                {
                    LinkButton linkbtn = row.FindControl("lnkdelete") as LinkButton;
                    Label hrdetNEW = row.FindControl("lblDetNo") as Label;
                    Label lblentryNoNEW = row.FindControl("lblentryNo") as Label;

                    if (!string.IsNullOrEmpty(hrdetNEW.Text) || !string.IsNullOrEmpty(lblentryNoNEW.Text))
                    {
                        linkbtn.Enabled = true;
                    }
                    else
                    {
                        linkbtn.Enabled = false;
                        RequiredFieldValidator rqfstart = row.FindControl("starttime") as RequiredFieldValidator;
                        rqfstart.IsValid = true;
                        RequiredFieldValidator rqfend = row.FindControl("endtime") as RequiredFieldValidator;
                        rqfend.IsValid = true;
                    }
                }
            }
            else
            {
                Object sender = new Object();
                EventArgs e = new EventArgs();
                Btngo_Click(sender, e);
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Student Special Hour Entry");
        }
    }

}

using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Collections;
using System.Data.SqlClient;
using System.Reflection;
using System.Collections.Generic;//added By Srinath 11/2/2013

public partial class notes : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string examcodeval = string.Empty;
    string strgrade = string.Empty;
    string strsec = string.Empty;
    string strsec1 = string.Empty;
    string strsecmark = string.Empty;
    string sturollno = string.Empty;
    string strsubcrd = string.Empty;
    string graders = string.Empty;
    string sqlstr = string.Empty;
    string sqlpercmd, sqlmarkcmd;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    string marks_per, marks_perfinal;
    string tnpsubno = "";
    string rollno;
    string strorder = "";
    string strregorder = "";
    double minmark, mark;
    int subjectctot = 0, criteriatot = 0, tottet;
    string examcode;
    double passperc;
    double presentperc;
    static int subjectcnt = 0;
    public string criteriano, subjno;
    string strsem = string.Empty;
    static int sectioncnt = 0;
    int count4 = 0;
    string order = "";
    string orderreg = "";
    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet dsbind = new DataSet();
    DataTable dt = new DataTable();
    DataSet dsmethodgoper = new DataSet();
    DataSet dsmethodgosyl = new DataSet();
    DataSet dsmethodgosubj = new DataSet();
    DataSet dsmethodgocriteria = new DataSet();
    DataSet dsmethodgomark = new DataSet();
    DataSet dstotd = new DataSet();
    DataSet dsfact = new DataSet();
    DataSet tempdssubj = new DataSet();
    DataSet dssettings = new DataSet();

    static string grouporusercode = "";
    static string groupor_usercode = "";
    string strdayflag;
    string regularflag = "";
    string genderflag = "";
    SqlConnection getcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);


    //Added By Srinath 11/2/2013 For Presentmonthcall()
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0, cum_per_workingdays1 = 0;
    int notconsider_value = 0;
    double conduct_hour_new = 0;
    int mmyycount;
    int moncount;
    string dd = "";
    int i, rows_count;
    string frdate, todate;
    string value_holi_status = "";
    string[] split_holiday_status = new string[1000];
    int unmark;
    TimeSpan ts;
    string tempvalue = "-1";
    int ObtValue = -1;
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    string split_holiday_status_1 = "", split_holiday_status_2 = "";
    string diff_date;
    double dif_date = 0;
    double dif_date1 = 0;
    static Boolean splhr_flag = false;
    int demfcal, demtcal;
    string monthcal;

    static Hashtable ht_sphr = new Hashtable();
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();

    DataSet ds4 = new DataSet();
    DataSet ds3 = new DataSet();

    DateTime per_from_date;
    DateTime per_to_date;
    DateTime per_from_gendate;
    DateTime per_to_gendate;
    DateTime dumm_from_date;
    DateTime Admission_date;

    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    int cal_from_date, cal_from_date_tmp;
    int cal_to_date, cal_to_date_tmp;
    double per_perhrs, per_abshrs;
    double per_ondu, per_leave, per_hhday;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double cum_present_date, cum_ondu_date, cum_leave_date, cum_absent_date;
    double workingdays = 0;
    double per_workingdays = 0;
    double cum_workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    double per_tage_date;
    double cum_tot_point, per_holidate;
    int per_dum_unmark, cum_dum_unmark, dum_unmark;
    int tot_per_hrs;
    double njhr, njdate, per_njdate, cum_njdate;
    double per_per_hrs, cum_per_perhrs;
    double tot_ondu, per_tot_ondu, cum_tot_ondu, cum_tot_ml, tot_ml, per_tot_ml;
    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;
    int count;
    int next = 0;
    int minpresII = 0;
    string value, date;
    int check;

    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0, tot_ml_spl_fals = 0;
    double per_leave_fals = 0;
    int per_abshrs_spl_true = 0, tot_per_hrs_spl_true = 0, tot_conduct_hr_spl_true = 0, tot_ondu_spl_true = 0, tot_ml_spl_true = 0;
    double per_leave_true = 0;
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0, tot_ml_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;

    DataSet dsprint = new DataSet();
    string group_code = "", columnfield = "";
    string dateconcat = "";
    string date1concat = "";
    DataSet dsnotes = new DataSet();
    Boolean cellclick3 = false;
    static string path1 = "";
    DataTable data = new DataTable();
    DataRow drow;


    protected void Page_Load(object sender, EventArgs e)
    {
        lblnorec.Visible = false;
        string staff_code = "";
        staff_code = (string)Session["staff_code"];
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            groupor_usercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            groupor_usercode = " user_code=" + Session["usercode"].ToString().Trim() + "";
        }

        if (!IsPostBack)
        {
            //--------Spread Design Format-----------

            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            //Saran
            //FpSpread1.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
            //FpSpread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
            //FpSpread1.CommandBar.Visible = false;

            string dt1 = DateTime.Today.ToShortDateString();
            string[] dsplit = dt1.Split(new Char[] { '/' });
            dateconcat = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            txtFromDate.Text = dateconcat.ToString();




            string dt2 = DateTime.Today.ToShortDateString();
            string[] dt2split = dt2.Split(new Char[] { '/' });
            date1concat = dt2split[1].ToString() + "/" + dt2split[0].ToString() + "/" + dt2split[2].ToString();
            txtToDate.Text = date1concat.ToString();



            group_code = Session["group_code"].ToString();
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            dsprint = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                lblnorec.Text = "";
                lblnorec.Visible = false;
                ddlcollege.DataSource = dsprint;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
                //ddlcollege_SelectedIndexChanged(sender, e);
            }
            else
            {
                lblnorec.Text = "Set college rights to the staff";
                lblnorec.Visible = true;

                Showgrid.Visible = false;
                lblrptname.Visible = false;
                divMainContents.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnPrint.Visible = false;
                btnprintmaster.Visible = false;
                lblnorec.Visible = false;
                return;
            }
            Showgrid.Visible = false;
            btnPrint.Visible = false;
            divMainContents.Visible = false;
            btnxl.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            lblnorec.Visible = false;

            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;



            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            if (ddldegree.Items.Count >= 1)
            {
                BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                BindSem(strbranch, strbatchyear, collegecode);
                BindSectionDetail(strbatch, strbranch);
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Give degree rights for staff ";
                ddlbatch.Items.Clear();
            }




        }

    }

    //------Load Function for the Batch Details-----

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["QueryString"]) != "")
        {

            PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            // make collection editable
            isreadonly.SetValue(this.Request.QueryString, false, null);
            // remove
            this.Request.QueryString.Remove(Convert.ToString(Session["QueryString"]));
            Request.QueryString.Clear();

        }

        Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();
        Page_Load(sender, e);
    }


    public void BindBatch()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBatch();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds2;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
                ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //------Load Function for the Degree Details-----

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            ddldegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds2;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //------Load Function for the Branch Details-----

    public void BindBranch(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            course_id = ddldegree.SelectedValue.ToString();
            ddlbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds2;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //------Load Function for the Section Details-----

    public void BindSectionDetail(string strbatch, string strbranch)
    {
        try
        {
            strbatch = ddlbatch.SelectedValue.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();

            ddlsection.Items.Clear();
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSectionDetail(strbatch, strbranch);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlsection.DataSource = ds2;
                ddlsection.DataTextField = "sections";
                ddlsection.DataBind();

                if (Convert.ToString(ds2.Tables[0].Columns["sections"]) == string.Empty)
                {
                    ddlsection.Enabled = false;
                    BindSubjecttest(strbatch, strbranch, strsem, strsec);

                }
                else
                {
                    ddlsection.Enabled = true;
                    BindSubjecttest(strbatch, strbranch, strsem, strsec);

                }
            }
            else
            {
                ddlsection.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }

    }

    //------Load Function for the Semester Details-----

    public void BindSem(string strbranch, string strbatchyear, string collegecode)
    {
        try
        {
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();

            ddlsemester.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSem(strbranch, strbatchyear, collegecode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds2.Tables[0].Rows[0][1]).ToString());
                duration = Convert.ToInt32(Convert.ToString(ds2.Tables[0].Rows[0][0]).ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsemester.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsemester.Items.Add(i.ToString());
                    }
                }
            }
            BindSubjecttest(strbatch, strbranch, strsem, strsec);
            //  Bindtest(strbatch, strbranch, strsem, strsec1);
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }



    //------Load Function for the Subject Details-----


    public void BindSubjecttest(string strbatch, string strbranch, string strsem, string strsec)
    {
        try
        {


            string sec = "";

            chklstsubject.Items.Clear();
            if (ddlsection.Text.ToString() == "All" || ddlsection.Text.ToString() == string.Empty || ddlsection.Text.ToString() == "-1")
            {
                strsec = "";
                strsec1 = "";
                sec = "";
            }
            else
            {
                strsec = " and registration.sections='" + ddlsection.Text.ToString() + "'";
                strsec1 = " and sections='" + ddlsection.Text.ToString() + "'";
                sec = "'" + ddlsection.Text.ToString() + "'";
            }
            strbatch = ddlbatch.SelectedValue.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            strsem = ddlsemester.SelectedValue.ToString();

            string syll_code = "";
            syll_code = GetFunction("select syll_code from syllabus_master where degree_code='" + strbranch + "' and batch_year='" + strbatch + "'and semester='" + strsem + "'");

            dsmethodgosubj.Dispose();
            dsmethodgosubj.Reset();


            if (Session["Staff_Code"].ToString() == "")
            {
                string strsql1 = "";
                if (strsec.ToString().Trim() != "" && strsec.ToString().Trim() != "-1")
                {
                    strsql1 = "select distinct subject_name,subject.subject_no from subject,subjectchooser,registration,sub_sem where sub_sem.subType_no=subject.subType_no and delflag = 0 and sub_sem.syll_code = subject.syll_code and sub_sem.promote_count=1 and subject.subject_no = subjectchooser.subject_no and subjectchooser.roll_no =registration.roll_no and registration.degree_code in ( " + strbranch + ") and registration.batch_year in (" + strbatch + ") and registration.sections in (" + sec + ")   and subject.syll_code in (" + syll_code + ") order by subject_name";
                }
                else
                {
                    strsql1 = "select distinct subject_name,subject.subject_no from subject,subjectchooser,registration,sub_sem where sub_sem.subType_no=subject.subType_no and delflag = 0 and sub_sem.syll_code = subject.syll_code and sub_sem.promote_count=1 and subject.subject_no = subjectchooser.subject_no and subjectchooser.roll_no =registration.roll_no and registration.degree_code in ( " + strbranch + ") and registration.batch_year in (" + strbatch + ")   and subject.syll_code in (" + syll_code + ") order by subject_name";
                }
                getcon.Close();
                getcon.Open();
                SqlDataAdapter sqldap = new SqlDataAdapter(strsql1, getcon);
                sqldap.Fill(dsmethodgosubj);

                //dsmethodgosubj = d2.BindSubject(strbatch, strbranch, strsem, sec);
            }
            else if (Session["Staff_Code"].ToString() != "")
            {

                string strsql1 = "";
                if (strsec.ToString().Trim() != "" && strsec.ToString().Trim() != "-1")
                {
                    //string strsql3 = "select distinct subjectchooser.subject_no,subject_name,subject_code,acronym,subject_type from subject,subjectchooser,registration,sub_sem,exam_type e where e.subject_no=subject.subject_no and  sub_sem.subType_no=subject.subType_no and delflag = 0 and sub_sem.syll_code = subject.syll_code and sub_sem.promote_count=1 and subject.subject_no = subjectchooser.subject_no and subjectchooser.roll_no =registration.roll_no   and registration.degree_code='" + strbranch + "' and registration.batch_year='" + strbatch + "' " + strsec + "  and subject.syll_code  in (" + strsyllcode + ") order by subjectchooser.subject_no";
                    strsql1 = "select distinct  subject_name , subject.subject_no,subject_type,subject_code,acronym from subject,subjectchooser,registration,sub_sem,staff_selector stsel where sub_sem.subType_no=subject.subType_no and delflag = 0 and sub_sem.syll_code = subject.syll_code and sub_sem.promote_count=1 and subject.subject_no = subjectchooser.subject_no and subjectchooser.roll_no =registration.roll_no and registration.degree_code in ( " + strbranch + ") and registration.batch_year in (" + strbatch + ") " + strsec + " and subject.subject_no= stsel.subject_no and stsel.staff_code='" + Session["Staff_Code"].ToString() + "'  and subject.syll_code in (" + syll_code + ") order by subject_name";
                }
                else
                {
                    strsql1 = "select distinct subject_name,subject.subject_no,subject_type,subject_code,acronym from subject,subjectchooser,registration,sub_sem,staff_selector stsel where sub_sem.subType_no=subject.subType_no and delflag = 0 and sub_sem.syll_code = subject.syll_code and sub_sem.promote_count=1 and subject.subject_no = subjectchooser.subject_no and subjectchooser.roll_no =registration.roll_no and registration.degree_code in ( " + strbranch + ") and registration.batch_year in (" + strbatch + ") and subject.subject_no= stsel.subject_no and stsel.staff_code='" + Session["Staff_Code"].ToString() + "'  and subject.syll_code in (" + syll_code + ") order by subject_name";
                }
                getcon.Close();
                getcon.Open();
                SqlDataAdapter sqldap = new SqlDataAdapter(strsql1, getcon);
                sqldap.Fill(dsmethodgosubj);

                //dsmethodgosubj = d2.BindparticularstaffSubject(strbatch, strbranch, strsem, strsec, Session["Staff_Code"].ToString());
                // dsmethodgosubj = d2.BindparticularstaffSubject(strbatch, strbranch, strsem, strsec, Session["Staff_Code"].ToString());
            }
            if (dsmethodgosubj.Tables[0].Rows.Count > 0)
            {

                chklstsubject.DataSource = dsmethodgosubj;
                chklstsubject.DataTextField = "subject_name";
                chklstsubject.DataValueField = "subject_no";
                chklstsubject.DataBind();
                //chklstsubject.SelectedIndex = chklstsubject.Items.Count - 1;
                for (int i = 0; i < chklstsubject.Items.Count; i++)
                {
                    chklstsubject.Items[i].Selected = true;
                    if (chklstsubject.Items[i].Selected == true)
                    {
                        count4 += 1;
                    }
                    if (chklstsubject.Items.Count == count4)
                    {
                        chksubject.Checked = true;
                    }
                }

            }
        }
        catch (Exception ex)
        {
            //norecordlbl.Visible = true;

        }
    }



    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Showgrid.Visible = false;
            divMainContents.Visible = false;
            btnxl.Visible = false;
            btnPrint.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            lblnorec.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;

            BindDegree(singleuser, group_user, collegecode, usercode);
            if (ddldegree.Items.Count >= 1)
            {
                BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                BindSem(strbranch, strbatchyear, collegecode);
                BindSectionDetail(strbatch, strbranch);
            }
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Showgrid.Visible = false;
            divMainContents.Visible = false;
            btnxl.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            lblnorec.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            btnPrint.Visible = false;


            BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            BindSem(strbranch, strbatchyear, collegecode);
            BindSectionDetail(strbatch, strbranch);
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }

    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        Showgrid.Visible = false;
        divMainContents.Visible = false;
        btnxl.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        lblnorec.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        btnPrint.Visible = false;

        if (!Page.IsPostBack == false)
        {
            ddlsemester.Items.Clear();
        }
        try
        {
            if ((ddlbranch.SelectedIndex != 0) && (ddlbranch.SelectedIndex > 0))
            {
                BindSem(strbranch, strbatchyear, collegecode);
                BindSectionDetail(strbatch, strbranch);
            }
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }

    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divMainContents.Visible = false;
            Showgrid.Visible = false;
            btnxl.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            lblnorec.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            btnPrint.Visible = false;
            if (!Page.IsPostBack == false)
            {
                ddlsection.Items.Clear();
            }
            DataSet testsubj = new DataSet();
            BindSectionDetail(strbatch, strbranch);
            BindSubjecttest(strbatch, strbranch, strsem, strsec);

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    protected void ddlsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        lblnorec.Visible = false;
        divMainContents.Visible = false;
        Showgrid.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        btnPrint.Visible = false;
        BindSubjecttest(strbatch, strbranch, strsem, strsec);
    }





    // method for button go

    protected void btngo_Click(object sender, EventArgs e)
    {
        string valfromdate = "";
        string valtodate = "";
        string frmconcat = "";


        valfromdate = txtFromDate.Text.ToString();
        string[] split1 = valfromdate.Split(new char[] { '/' });
        frmconcat = split1[1].ToString() + '/' + split1[0].ToString() + '/' + split1[2].ToString();
        DateTime dtfromdate = Convert.ToDateTime(frmconcat.ToString());

        valtodate = txtToDate.Text.ToString();
        string[] split2 = valtodate.Split(new char[] { '/' });
        frmconcat = split2[1].ToString() + '/' + split2[0].ToString() + '/' + split2[2].ToString();
        DateTime dttodate = Convert.ToDateTime(frmconcat.ToString());

        TimeSpan ts = dttodate.Subtract(dtfromdate);
        int days = ts.Days;
        if (days < 0)
        {
            errmsg.Text = "From Date Must Be Less Then To Date";
            Showgrid.Visible = false;
            btnPrint.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            divMainContents.Visible = false;
            btnprintmaster.Visible = false;
            lblnorec.Visible = false;
        }
        else
        {
            DataSet dsnotes = new DataSet();
            DataView dvnotes = new DataView();
            try
            {
                string allsubjectnumber = "";
                for (int i = 0; i < chklstsubject.Items.Count; i++)
                {

                    if (chklstsubject.Items[i].Selected == true)
                    {
                        if (allsubjectnumber == "")
                        {
                            allsubjectnumber = chklstsubject.Items[i].Value;
                        }
                        else
                        {
                            allsubjectnumber = allsubjectnumber + ',' + chklstsubject.Items[i].Value;

                        }
                    }

                }

                if (allsubjectnumber != "")
                {

                    string dtchs = txtFromDate.Text;
                    string[] dsplitchs = dtchs.Split(new Char[] { '/' });
                    string fromdate = dsplitchs[1].ToString() + "/" + dsplitchs[0].ToString() + "/" + dsplitchs[2].ToString();
                    string dtchsto = txtToDate.Text;
                    string[] dsplitchsto = dtchsto.Split(new Char[] { '/' });
                    string todate = dsplitchsto[1].ToString() + "/" + dsplitchsto[0].ToString() + "/" + dsplitchsto[2].ToString();
                    bindspread();
                    string query = "";
                    if (chkdate.Checked == true)
                    {
                        query = "select * from notestbl where subject_no in(" + allsubjectnumber + ") and date between '" + fromdate + "' and '" + todate + "' order by subject_no";

                    }
                    else
                    {
                        query = "select * from notestbl where subject_no in(" + allsubjectnumber + ")  order by subject_no";
                    }

                    getcon.Close();
                    getcon.Open();
                    SqlDataAdapter sqldap = new SqlDataAdapter(query, getcon);
                    sqldap.Fill(dsnotes);
                    Dictionary<int, string> dicsubjspan = new Dictionary<int, string>();
                    Dictionary<string, string> dicsubjrowspan = new Dictionary<string, string>();
                    int span = 1;
                    if (dsnotes.Tables[0].Rows.Count > 0)
                    {
                        int rowstart = -1;
                        for (int cn = 0; cn < chklstsubject.Items.Count; cn++)
                        {
                            if (chklstsubject.Items[cn].Selected == true)
                            {
                                int sno = 0;
                                string subjectnumber = chklstsubject.Items[cn].Value;
                                string subjectname = chklstsubject.Items[cn].Text;
                                dsnotes.Tables[0].DefaultView.RowFilter = "subject_no='" + subjectnumber + "'";
                                dvnotes = dsnotes.Tables[0].DefaultView;

                                if (dvnotes.Count > 0)
                                {

                                    drow = data.NewRow();
                                    drow["SNo"] = subjectname;
                                    data.Rows.Add(drow);
                                    dicsubjspan.Add(span, subjectname);
                                    span = span + 1;
                                    rowstart++;
                                    int rowspan = 0;
                                    string predate = "";
                                    string spanrow = "";
                                    for (int i = 0; i < dvnotes.Count; i++)
                                    {
                                        drow = data.NewRow();
                                        sno++;
                                        span++;

                                        drow["SNo"] = sno.ToString();

                                        string date = dvnotes[i]["date"].ToString();
                                        string[] splitdate = date.Split(' ');
                                        date = Convert.ToString(splitdate[0]);
                                        string[] dates = date.Split('/');
                                        date = dates[1].ToString() + "/" + dates[0].ToString() + "/" + dates[2].ToString();
                                        //if (predate == date)
                                        //{
                                        //    rowspan++;
                                        //    spanrow = rowstart.ToString() + "," + rowspan.ToString();
                                        //    if (!dicsubjrowspan.ContainsKey(date))
                                        //        dicsubjrowspan.Add(date, spanrow);
                                        //    else
                                        //    {
                                        //        dicsubjrowspan.Remove(date);
                                        //        dicsubjrowspan.Add(date, spanrow);
                                        //    }
                                        //    rowstart++;
                                        //}
                                        //else
                                        //{
                                        //    rowstart++;
                                        //    rowspan = 1;
                                        //    spanrow = rowstart.ToString() + "," + rowspan.ToString();
                                        //    dicsubjrowspan.Add(date, spanrow.ToString());
                                        //}
                                        //predate = date;
                                        drow["date"] = date;
                                        string imagetext = Convert.ToString(dvnotes[i]["filename"]);
                                        drow["view"] = imagetext.ToString();
                                        drow["file"] = Convert.ToString(dvnotes[i]["fileid"]);
                                        data.Rows.Add(drow);
                                        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dvnotes[i]["fileid"]);
                                    }
                                }

                            }
                        }


                        if (data.Columns.Count > 0 && data.Rows.Count > 0)//===========on 9/4/12
                        {

                            Showgrid.DataSource = data;
                            Showgrid.DataBind();
                            Showgrid.Visible = true;
                            Showgrid.Width = 500;
                            btnPrint.Visible = true;
                            divMainContents.Visible = true;

                            Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            Showgrid.Rows[0].Font.Bold = true;
                            Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;

                            for (int grCol = 0; grCol < Showgrid.Columns.Count; grCol++)
                                Showgrid.HeaderRow.Cells[grCol].Visible = false;

                            //ColumnSpan
                            if (dicsubjspan.Count > 0)
                            {

                                foreach (KeyValuePair<int, string> dr in dicsubjspan)
                                {
                                    int rowcnt = dr.Key;

                                    int d = Convert.ToInt32(data.Columns.Count);
                                    Showgrid.Rows[rowcnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                    Showgrid.Rows[rowcnt].Cells[0].BackColor = Color.LightBlue;
                                    Showgrid.Rows[rowcnt].Cells[0].ForeColor = Color.Chocolate;
                                    Showgrid.Rows[rowcnt].Cells[0].ColumnSpan = d;

                                    for (int a = 1; a < d - 1; a++)
                                    {
                                        Showgrid.Rows[rowcnt].Cells[a].Visible = false;
                                    }
                                }
                            }
                            //Rowspan
                            for (int i = Showgrid.Rows.Count - 1; i > 0; i--)
                            {
                                GridViewRow row = Showgrid.Rows[i];
                                GridViewRow previousRow = Showgrid.Rows[i - 1];

                                if (!dicsubjspan.ContainsKey(i) || !dicsubjspan.ContainsKey(i - 1))
                                {
                                    Label date = (Label)row.Cells[1].FindControl("lbl_Date");
                                    string nxdate = date.Text;
                                    Label date1 = (Label)previousRow.Cells[1].FindControl("lbl_Date");
                                    string predate = date1.Text;
                                    if (nxdate == predate)
                                    {
                                        if (previousRow.Cells[1].RowSpan == 0)
                                        {
                                            if (row.Cells[1].RowSpan == 0)
                                            {
                                                previousRow.Cells[1].RowSpan += 2;
                                            }
                                            else
                                            {
                                                previousRow.Cells[1].RowSpan = row.Cells[1].RowSpan + 1;
                                            }
                                            row.Cells[1].Visible = false;
                                        }
                                    }
                                }

                            }
                            errmsg.Text = "";
                            lblrptname.Visible = true;
                            txtexcelname.Visible = true;
                            btnxl.Visible = true;
                            btnprintmaster.Visible = true;
                        }
                        else
                        {
                            errmsg.Text = "No Record Found";
                            Showgrid.Visible = false;
                            btnPrint.Visible = false;
                            divMainContents.Visible = false;
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            btnxl.Visible = false;
                            btnprintmaster.Visible = false;
                            lblnorec.Visible = false;
                        }
                    }
                    else
                    {
                        errmsg.Text = "No Record Found";
                        Showgrid.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnPrint.Visible = false;
                        btnxl.Visible = false;
                        btnprintmaster.Visible = false;
                        lblnorec.Visible = false;
                        divMainContents.Visible = false;
                    }


                }
                else
                {
                    errmsg.Text = "Kindly Select Atleast One Subject";
                    Showgrid.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnPrint.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    lblnorec.Visible = false;
                    divMainContents.Visible = false;
                }


            }

            catch
            {
            }
        }

    }

    public void bindspread()
    {
        try
        {
            data.Columns.Add("Sno");
            data.Columns.Add("date");
            data.Columns.Add("view");
            data.Columns.Add("file");

            ArrayList arrColHdrNames1 = new ArrayList();
            arrColHdrNames1.Add("S.No");
            arrColHdrNames1.Add("Date");
            arrColHdrNames1.Add("View");
            arrColHdrNames1.Add("file");

            DataRow drHdr1 = data.NewRow();
            for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                drHdr1[grCol] = arrColHdrNames1[grCol];

            data.Rows.Add(drHdr1);


        }
        catch
        {

        }

    }


    protected void link_View_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkSelected = (LinkButton)sender;
            string rowIndxS = lnkSelected.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            string activerow1 = rowIndx.ToString();
            if (activerow1 != "-1")
            {
                string fileName = string.Empty;
                Label lblsubname = (Showgrid.Rows[rowIndx].FindControl("lblfileid") as Label);
                string fileid = lblsubname.Text;
                Label lblsubno = (Showgrid.Rows[rowIndx].FindControl("lbl_View") as Label);
                path1 = lblsubno.Text;
                //string fileid = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag.ToString();
                //path1 = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;

                SqlCommand cmd = new SqlCommand("SELECT filename,filedata,filetype FROM notestbl WHERE fileid='" + fileid + "' and filename='" + path1 + "'", getcon);// and degree_code="++", con);
                getcon.Close();
                getcon.Open();
                SqlDataReader dReader = cmd.ExecuteReader();
                while (dReader.Read())
                {
                    Response.ContentType = dReader["filetype"].ToString();
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + dReader["filename"] + "\"");
                    Response.BinaryWrite((byte[])dReader["filedata"]);
                    Response.End();
                }
            }
        }
        catch
        {


        }
    }


    //protected void fpspread1_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{

    //    string activerow = FpSpread1.Sheets[0].ActiveRow.ToString();

    //    string activecolumn = FpSpread1.Sheets[0].ActiveColumn.ToString();
    //    int actcol = Convert.ToInt16(activecolumn);
    //    int actrow = Convert.ToInt16(activerow);
    //    if (actcol == 2)
    //    {
    //        string fileName = string.Empty;

    //        string fileid = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag.ToString();
    //        path1 = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;

    //        SqlCommand cmd = new SqlCommand("SELECT filename,filedata,filetype FROM notestbl WHERE fileid='" + fileid + "' and filename='" + path1 + "'", getcon);// and degree_code="++", con);
    //        getcon.Close();
    //        getcon.Open();
    //        SqlDataReader dReader = cmd.ExecuteReader();
    //        while (dReader.Read())
    //        {
    //            Response.ContentType = dReader["filetype"].ToString();
    //            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + dReader["filename"] + "\"");
    //            Response.BinaryWrite((byte[])dReader["filedata"]);
    //            Response.End();
    //            cellclick3 = false;



    //        }
    //    }
    //}


    protected void chkdate_checkedchanged(object sender, EventArgs e)
    {
        if (chkdate.Checked == true)
        {
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;
        }
        else
        {
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
        }
    }
    public string GetFunction(string sqlQuery)
    {
        string sqlstr;
        sqlstr = sqlQuery;
        getcon.Close();
        getcon.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, getcon);
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr);
        cmd.Connection = getcon;
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


    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {

        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value.ToString();
            }
        }

        return null;
    }


    //----------Subject Dropdown Extender-----------------

    protected void chksubject_CheckedChanged(object sender, EventArgs e)
    {
        if (chksubject.Checked == true)
        {
            for (int i = 0; i < chklstsubject.Items.Count; i++)
            {
                chklstsubject.Items[i].Selected = true;
                txtsubject.Text = "Subject(" + (chklstsubject.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstsubject.Items.Count; i++)
            {
                chklstsubject.Items[i].Selected = false;
                txtsubject.Text = "---Select---";
            }
        }
    }

    protected void chklstsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        psubject.Focus();

        int subjectcount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < chklstsubject.Items.Count; i++)
        {

            if (chklstsubject.Items[i].Selected == true)
            {
                chklstsubject.Items[i].Selected = true;
                value = chklstsubject.Items[i].Text;
                code = chklstsubject.Items[i].Value.ToString();
                subjectcount = subjectcount + 1;
                txtsubject.Text = "Subject(" + subjectcount.ToString() + ")";
            }
            else
            {
                chklstsubject.Items[i].Selected = false;
            }

        }

        if (subjectcount == 0)
            txtsubject.Text = "---Select---";
        else
        {
            Label lbl = subjectlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = subjectimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(subjectimg_Click);
        }
        subjectcnt = subjectcount;
        //BindTest(strbatch, strbranch);

    }

    public void subjectimg_Click(object sender, ImageClickEventArgs e)
    {
        subjectcnt = subjectcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstsubject.Items[r].Selected = false;

        txtsubject.Text = "Subject(" + sectioncnt.ToString() + ")";
        if (txtsubject.Text == "Subject(0)")
        {
            txtsubject.Text = "---Select---";

        }

    }

    public Label subjectlabel()
    {
        Label lbc = new Label();

        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton subjectimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }

    //------Method for the Excel Coversion -----
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string print = "";
            string appPath = HttpContext.Current.Server.MapPath("~");
            string strexcelname = "";
            if (appPath != "")
            {
                strexcelname = txtexcelname.Text;
                appPath = appPath.Replace("\\", "/");
                if (strexcelname != "")
                {

                    d2.printexcelreportgrid(Showgrid, strexcelname);
                }
                else
                {
                    lblnorec.Text = "Please enter your Report Name";
                    lblnorec.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
        }

    }

    public override void VerifyRenderingInServerForm(Control control)
    { }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        // Session["column_header_row_count"] = Convert.ToString(FpSpread1.ColumnHeader.RowCount);
        string sections = ddlsection.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
        {
            sections = "";
        }
        string degreedetails = "Student Over All Cam report" + '@' + "Degree: " + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '[' + ddlbranch.SelectedItem.ToString() + ']' + '-' + "Sem-" + ddlsemester.SelectedItem.ToString() + '-' + sections;
        string pagename = "StudentTestReport.aspx";
        Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

}
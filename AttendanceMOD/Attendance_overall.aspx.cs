//==========MANIPRABHA A.
using System;////////////////=====modified on 29/3/12(To month_year var), 30/3/12(contains condition, halfholiday, len(r_no))
//=================4/4/12(output data wrong), 12/4/12(date lables, page setiing panel, complete print setting), 27/4/12(function change-od cnt not in tot pres, absent cnt not d/p)
//================4/6/12(include spl hour),9/6/12(try in pl, p_m_s_n,header_index),22/6/12 (header caption Change)
//=================03/07/12(remov : in header)//====modified on 04.07.12 by mythili(header_alignment),20/7/12(txt celltype fr rol,reg)
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BalAccess;
using System.Data.SqlClient;
using System.Drawing;
using FarPoint.Web.Spread;
public partial class Attendance_overall : System.Web.UI.Page
{
    [Serializable()]
    public class MyImg : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            //''----------strudent photo
            System.Web.UI.WebControls.Image img1 = new System.Web.UI.WebControls.Image();
            img1.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img1.Width = Unit.Percentage(75);
            img1.Height = Unit.Percentage(10);
            return img1;

            //''------------clg logo
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(105);
            img.Height = Unit.Percentage(10);
            return img;

            //'-------------coe sign
            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
            img2.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img2.Width = Unit.Percentage(75);
            img2.Height = Unit.Percentage(10);
            return img2;

        }
    }

    SqlDataReader dr_exam;
    SqlDataReader dr_mnthyr;
    SqlDataReader dr_convert;
    string grade_setting = "";
    //+++++++++++++++++++++++++++++++++++++++++++++++
    double max_cond_hr = 0;
    Boolean sflag = false;
    string date1 = "", datefrom = "", date2 = "", dateto = "", state = "", isonumber = "";
    DateTime dt1 = new DateTime();
    DateTime dt2 = new DateTime();
    SqlConnection con_splhr_query_master = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_sem_roman = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_sem2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Photo = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Load = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Inssetting = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Getfunc = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Examcode = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_loadSubject = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Grade = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Stud = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Grade1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_mrkentry = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_currsem = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_getdetail = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_daters = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_course = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_exam = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_secrs = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_new = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_grademas = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_credit = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_option = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_sem = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_result = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_convertgrade = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_subcrd = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_rs = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Grade_flag = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_fun = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    Hashtable has = new Hashtable();
    DAccess2 d2 = new DAccess2();

    //++++++++++++

    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection readcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con4a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection bind_con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);

    SqlCommand cmd;
    SqlCommand cmd3a;
    SqlCommand studinfocmd;
    SqlDataReader studinfors;


    DataSet ds_has = new DataSet();
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int notconsider_value_spl = 0, njhr_spl = 0;
    //==============0n 12/4/12 PRABHA
    string[] string_session_values = new string[100];
    int temp_count = 0, final_print_col_cnt = 0, split_col_for_footer = 0, col_count = 0, footer_balanc_col = 0, footer_count = 0;
    int col_count_all = 0, span_cnt = 0, child_span_count = 0;
    Boolean check_col_count_flag = false;
    static DataSet dsprint = new DataSet();
    string new_header_string = "", column_field = "", printvar = "";
    string view_footer = "", view_header = "", view_footer_text = "";
    int start_column = 0, end_column = 0;
    string coll_name = "", form_name = "", phoneno = "", faxno = "";
    string footer_text = "", header_alignment = "";
    string degree_deatil = "";
    int new_header_count = 0;
    string[] new_header_string_split;
    string phone = "", fax = "", email_id = "", web_add = "";
    Boolean btnclick_or_print = false;
    int between_visible_col_cnt = 0, between_visible_col_cnt_bal = 0;
    int x = 0;
    int visi_col = 0, visi_col1 = 0;
    // string phone = "", fax = "", email_id = "", web_add = "";
    //---------------------------


    //'----------------------------------------------------------new 
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    string split_holiday_status_1 = "", split_holiday_status_2 = "";
    string value_holi_status = "";
    string[] split_holiday_status = new string[1000];
    int conducted_hrs_new = 0;
    int tot_absent_houes = 0;
    Hashtable has_holi = new Hashtable();
    //----------------
    string collnamenew1 = "";
    string address1 = "";
    string address2 = "";
    string address3 = "";
    string pincode = "";
    string categery = "";
    string Affliated = "";
    string today_date = "";
    string logo1 = "";
    string logo2 = "";
    int njdate_mng = 0, njdate_evng = 0, mng_conducted_half_days = 0, per_workingdays1 = 0;
    int per_holidate_mng = 0, per_holidate_evng = 0, notconsider_value = 0, evng_conducted_half_days = 0;
    Hashtable hat = new Hashtable();
    DAccess2 dacces2 = new DAccess2();
    DAccess2 daccess = new DAccess2();

    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds5 = new DataSet();
    DataSet ds6 = new DataSet();
    DataSet ds_load = new DataSet();
    DataTable dt = new DataTable();
    DataRow dtrow = null;
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    int mmyycount = 0;
    //'----------------------------------------------------------new 
    int moncount;
    double dif_date = 0;
    double dif_date1 = 0;
    int abs = 0, att = 0;
    int dum_diff_date, unmark;
    string roll_no, reg_no, roll_ad, studname;
    int check;
    DateTime per_from_date;
    DateTime per_to_date;
    DateTime dumm_from_date;
    string frdate, todate;
    TimeSpan ts;
    string diff_date;
    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;
    int next = 0;
    int minpresII = 0;
    string value, date;
    int i, rows_count;
    string tempvalue = "-1";
    int ObtValue = -1;
    int cal_from_date;
    int cal_to_date;
    int count = 0;
    double per_perhrs, per_abshrs, cum_perhrs, cum_abshrs;
    double per_ondu, per_leave, per_hhday, cum_ondu, cum_leave, cum_hhday, per_AB, per_P;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    double halfday = 0;
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double cum_present_date, cum_ondu_date, cum_leave_date, cum_absent_date;
    double workingdays = 0;
    double per_workingdays = 0;
    double cum_workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    double per_tage_date, cum_tage_date, per_tage_hrs, cum_tage_hrs;
    double cum_tot_point, per_holidate, cum_per_holidate;
    int per_dum_unmark, cum_dum_unmark, dum_unmark;
    int tot_per_hrs, per_tot_per_hrs, cum_per_tot_per, tot_wok_hrs;
    double per_con_hrs, cum_con_hrs;
    double njhr, njdate, per_njhr, per_njdate, cum_njhr, cum_njdate;
    double per_per_hrs, cum_per_perhrs;
    double tot_ondu, per_tot_ondu, cum_tot_ondu, mmm, nnnn;
    int countds = 0;
    //-----------------------------------------end
    string roll = "";

    //*******
    int else_tot_pass = 0;
    //'---------------------------new
    string address = "";
    string Phoneno = "";
    string Faxno = "";
    string phnfax = "";
    int serialno = 0;
    int exam_code_new = 0;
    int qpassstu = 0;
    //'------------------------------
    int subjectcount = 0;
    string district = "";
    string email = "";
    string website = "";
    string strsec = "";
    int semdec = 0;
    string sections = "";
    string funcgrade = "";
    string mark = "";
    Boolean markflag = false;
    string rol_no = "";
    string courseid = "";
    string atten = "";
    string Master1 = "";
    string regularflag = "";
    string genderflag = "";
    string strdayflag = "";
    string fromdate = "";
    Boolean InsFlag;
    Boolean flag;
    int IntExamCode = 0;
    int column_count = 0;
    string degree_code = "";
    string current_sem = "";
    string batch_year = "";
    string getgradeflag = "";
    string exam_month = "";
    string exam_year = "";
    string getsubno = "";
    string getsubtype = "";
    int rcnt;
    int ExamCode = 0;
    string strmnthyear = "";
    string strexam = "";
    int overallcredit = 0;
    string grade = "";
    string funcsubno = "";
    string funcsubname = "";
    string funcsubcode = "";
    string funcresult = "";
    string funcsemester = "";
    string funccredit = "";
    string EarnedVal = "";
    double cgpa2 = 0;
    string semesterddl = "";
    int cou = 0;



    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    int sl_no1 = 1;
    int allpass_tot_cnt = 0;
    string degree = "";
    int qtot_stu = 0;
    //*********

    DateTime Admission_date;
    static string grouporusercode = "";
    //added by srinath 18/2/2013
    DataSet ds_sphr = new DataSet();
    static Hashtable ht_sphr = new Hashtable();
    string tempdegreesem = "";
    string chkdegreesem = "";
    Boolean splhr_flag = false;
    Boolean datechk = false;
    int tempcallfromdate = 0;
    string tempfromdate = "";

    public void bindbatch()
    {
        ddlBatch.Items.Clear();
        ds_load = daccess.select_method_wo_parameter("bind_batch", "sp");
        int count = ds_load.Tables[0].Rows.Count;
        if (count > 0)
        {
            ddlBatch.DataSource = ds_load;
            ddlBatch.DataTextField = "batch_year";
            ddlBatch.DataValueField = "batch_year";
            ddlBatch.DataBind();
        }
        int count1 = ds_load.Tables[1].Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(ds_load.Tables[1].Rows[0][0].ToString());
            ddlBatch.SelectedValue = max_bat.ToString();
            con.Close();
        }
    }
    public void bindbranch()
    {
        ddlBranch.Items.Clear();
        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("course_id", ddlDegree.SelectedValue);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);

        ds_load = daccess.select_method("bind_branch", hat, "sp");
        int count2 = ds_load.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddlBranch.DataSource = ds_load;
            ddlBranch.DataTextField = "dept_name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();
        }
    }
    public void binddegree()
    {
        ddlDegree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Clear();
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds_load = daccess.select_method("bind_degree", hat, "sp");
        int count1 = ds_load.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            ddlDegree.DataSource = ds_load;
            ddlDegree.DataTextField = "course_name";
            ddlDegree.DataValueField = "course_id";
            ddlDegree.DataBind();
        }
    }
    public void bindsec()
    {
        ddlSec.Items.Clear();
        hat.Clear();
        hat.Add("batch_year", ddlBatch.SelectedValue.ToString());
        hat.Add("degree_code", ddlBranch.SelectedValue);
        ds_load = daccess.select_method("bind_sec", hat, "sp");
        int count5 = ds_load.Tables[0].Rows.Count;
        if (count5 > 0)
        {
            ddlSec.DataSource = ds_load;
            ddlSec.DataTextField = "sections";
            ddlSec.DataValueField = "sections";
            ddlSec.DataBind();
            ddlSec.Enabled = true;
        }
        else
        {
            ddlSec.Enabled = false;
        }
    }
    public void BindBatch()
    {
        ddlBatch.Items.Clear();
        string sqlstr = "";
        int max_bat = 0;


        DataSet ds = ClsAttendanceAccess.GetBatchDetail();
        if (ds.Tables[0].Rows.Count > 0)
        {

            ddlBatch.DataSource = ds;
            ddlBatch.DataTextField = "batch_year";
            ddlBatch.DataValueField = "batch_year";
            ddlBatch.DataBind();
            sqlstr = "select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' ";
            max_bat = Convert.ToInt32(GetFunction(sqlstr));
            ddlBatch.SelectedValue = max_bat.ToString();

            // ddlBatch.Items.Insert(0, new ListItem("- -Select- -", "-1"));

        }
    }
    public void BindDegree()
    {


        ddlDegree.Items.Clear();
        collegecode = Session["collegecode"].ToString();
        DataSet ds = ClsAttendanceAccess.GetDegreeDetail(collegecode.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {

            ddlDegree.DataSource = ds;
            ddlDegree.DataValueField = "Course_Id";
            ddlDegree.DataTextField = "Course_Name";
            ddlDegree.DataBind();
            //ddlDegree.Items.Insert(0, new ListItem("- -Select- -", "-1"));
        }
    }
    public void BindSectionDetail()
    {

        string branch = ddlBranch.SelectedValue.ToString();
        string batch = ddlBatch.SelectedValue.ToString();
        con_Load.Close();
        con_Load.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", con_Load);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlSec.DataSource = ds;
        ddlSec.DataTextField = "sections";
        ddlSec.DataValueField = "sections";
        ddlSec.DataBind();
        //  ddlSec.Items.Insert(0, new ListItem("--Select--", "-1"));
        SqlDataReader dr_sec;
        dr_sec = cmd.ExecuteReader();
        dr_sec.Read();
        if (dr_sec.HasRows == true)
        {
            if (dr_sec["sections"].ToString() == "")
            {
                ddlSec.Enabled = false;
                //  RequiredFieldValidator5.Visible = false;
            }
            else
            {
                ddlSec.Enabled = true;
                //   RequiredFieldValidator5.Visible = true;
            }
        }
        else
        {
            ddlSec.Enabled = false;
            //   RequiredFieldValidator5.Visible = false;
        }

    }
    public void Get_Semester()
    {
        Boolean first_year;
        first_year = false;
        int duration = 0;
        string batch_calcode_degree;
        //int typeval = 4;

        string batch = ddlBatch.SelectedValue.ToString();
        collegecode = Session["collegecode"].ToString();
        string degree = ddlBranch.SelectedValue.ToString();
        batch_calcode_degree = batch.ToString() + "/" + collegecode.ToString() + "/" + degree.ToString();
        //Session["collegecode"].ToString();
        DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());

        if (ds.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

            for (int i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }

            }
            //ddlSemYr.Items.Insert(0, new ListItem("- -Select- -", "-1"));
        }
    }
    public string GetFunction(string sqlQuery)
    {
        string sqlstr;
        sqlstr = sqlQuery;
        con_Getfunc.Close();
        con_Getfunc.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con_Getfunc);
        SqlDataReader drnew;
        SqlCommand funcmd = new SqlCommand(sqlstr);
        funcmd.Connection = con_Getfunc;
        drnew = funcmd.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "0";
        }
    }
    public void clear()
    {
        ddlSemYr.Items.Clear();
        ddlSec.Items.Clear();
    }
    public void bindsem()
    {


        ddlSemYr.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Open();
        SqlDataReader dr;
        cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.Text.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
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
                    ddlSemYr.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }

            }
        }
        else
        {
            dr.Close();
            SqlDataReader dr1;
            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
            //     ddlSemYr.Items.Clear();
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
                        ddlSemYr.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                }
            }

            dr1.Close();
        }

        con.Close();
    }

    protected void ddlBatch_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if ((ddlDegree.SelectedIndex != 0) && (ddlBranch.SelectedIndex != 0))
        {
            ddlSemYr.Items.Clear();
            Get_Semester();
        }

        ddlSec.SelectedIndex = -1;


        //FpEntry.Visible = false;//
        gview.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;

        frmlbl.Visible = false;
        tolbl.Visible = false;
        tofromlbl.Visible = false;
        lblnorec.Visible = false;
    }
    protected void ddlDegree_SelectedIndexChanged1(object sender, EventArgs e)
    {
        //FpEntry.Visible = false;//
        gview.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;

        frmlbl.Visible = false;
        tolbl.Visible = false;
        tofromlbl.Visible = false;
        lblnorec.Visible = false;
        ddlBranch.Items.Clear();
        string course_id = ddlDegree.SelectedValue.ToString();
        collegecode = Session["collegecode"].ToString();
        usercode = Session["UserCode"].ToString();//Session["UserCode"].ToString();
        DataSet ds = ClsAttendanceAccess.GetBranchDetail(course_id.ToString(), collegecode.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlBranch.DataSource = ds;
            ddlBranch.DataTextField = "Dept_Name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();


        }
    }
    protected void ddlBranch_SelectedIndexChanged1(object sender, EventArgs e)
    {
        //FpEntry.Visible = false;//
        gview.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;

        frmlbl.Visible = false;
        tolbl.Visible = false;
        tofromlbl.Visible = false;
        lblnorec.Visible = false;
        clear();


        if (!Page.IsPostBack == false)
        {

        }
        try
        {
            if ((ddlBranch.SelectedIndex != 0) || (ddlBranch.SelectedIndex > 0) || (ddlBranch.SelectedIndex == 0))
            {

                bindsem();
                bindsec();
            }
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }
    protected void ddlSemYr_SelectedIndexChanged1(object sender, EventArgs e)
    {
        //FpEntry.Visible = false;//
        gview.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;

        frmlbl.Visible = false;
        tolbl.Visible = false;
        tofromlbl.Visible = false;
        lblnorec.Visible = false;
        if (!Page.IsPostBack == false)
        {
            ddlSec.Items.Clear();
        }

        bindsec();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        errmsg.Visible = false;
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
        }
        errmsg.Visible = false;
        if (!IsPostBack)
        {
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");

            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            //Added By Srinath 27/2/2013
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            //FpEntry.Visible = false;//
            gview.Visible = false;
            btnxl.Visible = false;

            frmlbl.Visible = false;
            tolbl.Visible = false;
            tofromlbl.Visible = false;
            lblnorec.Visible = false;


            frmlbl.Visible = false;
            tolbl.Visible = false;

            tofromlbl.Visible = false;
            txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            string dt = DateTime.Today.ToShortDateString();
            string[] dsplit = dt.Split(new Char[] { '/' });
            txtFromDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            today_date = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            txtToDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            Session["curr_year"] = dsplit[2].ToString();
            Session["today_date"] = today_date;

            //FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();//
            //MyStyle.Font.Bold = true;//
            //MyStyle.Font.Name = "Book Antiqua";//
            //MyStyle.Font.Size = FontUnit.Medium;//
            //MyStyle.HorizontalAlign = HorizontalAlign.Center;//
            //MyStyle.ForeColor = Color.Black;//
            //MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");//
            //FpEntry.ActiveSheetView.ColumnHeader.DefaultStyle = MyStyle;//

            //FpEntry.ActiveSheetView.DefaultStyle.Font.Name = "Book Antiqua";//
            //FpEntry.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;//
            //FpEntry.CommandBar.Visible = false;//
            //'--------------------------------------
            collegecode = Session["collegecode"].ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            //=======================on 11/4/12

            if (Request.QueryString["val"] == null)
            {
                bindbatch();//-----------------call bind functions
                binddegree();
                if (ddlDegree.Items.Count > 0)
                {
                    ddlDegree.Enabled = true;
                    ddlBranch.Enabled = true;
                    ddlSemYr.Enabled = true;
                    ddlSec.Enabled = true;
                    bindbranch();
                    bindsem();
                    bindsec();
                    Button1.Enabled = true;
                    txtFromDate.Enabled = true;
                    txtToDate.Enabled = true;
                }
                else
                {
                    ddlDegree.Enabled = false;
                    ddlBranch.Enabled = false;
                    ddlSemYr.Enabled = false;
                    ddlSec.Enabled = false;
                    Button1.Enabled = false;
                    txtFromDate.Enabled = false;
                    txtToDate.Enabled = false;
                }

            }
            else
            {
                //=======================page redirect from master print setting
                try
                {
                    string_session_values = Request.QueryString["val"].Split(',');
                    if (string_session_values.GetUpperBound(0) == 6)
                    {
                        bindbatch();
                        ddlBatch.SelectedIndex = Convert.ToInt16(string_session_values[0]);
                        binddegree();
                        if (ddlDegree.Items.Count > 0)
                        {
                            ddlDegree.Enabled = true;
                            ddlBranch.Enabled = true;
                            ddlSemYr.Enabled = true;
                            ddlSec.Enabled = true;
                            txtFromDate.Enabled = true;
                            txtToDate.Enabled = true;
                            ddlDegree.SelectedIndex = Convert.ToInt16(string_session_values[1]);
                            bindbranch();
                            if (ddlBranch.Enabled == true)
                            {
                                ddlBranch.SelectedIndex = Convert.ToInt16(string_session_values[2].ToString());
                            }
                            bindsem();
                            if (ddlSemYr.Enabled == true)
                            {
                                ddlSemYr.SelectedIndex = Convert.ToInt16(string_session_values[3].ToString());
                            }
                            bindsec();
                            if (ddlSec.Enabled == true)
                            {
                                ddlSec.SelectedIndex = Convert.ToInt16(string_session_values[4].ToString());
                            }
                            txtFromDate.Text = string_session_values[5].ToString();
                            txtToDate.Text = string_session_values[6].ToString();

                            print_btngo();

                            if (final_print_col_cnt > 0)
                            {
                                //setheader_print();//Hidden By SRinath 15/5/2013

                                //FpEntry.Width = final_print_col_cnt * 100;//
                                gview.Width = final_print_col_cnt * 100;
                            }
                        }
                        else
                        {
                            ddlDegree.Enabled = false;
                            ddlBranch.Enabled = false;
                            ddlSemYr.Enabled = false;
                            ddlSec.Enabled = false;
                            txtFromDate.Enabled = false;
                            txtToDate.Enabled = false;
                        }
                    }
                }
                catch
                {
                }
                //===================================

            }
            //======================
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            Session["Sex"] = "0";
            Session["flag"] = "-1";
            string Master1 = "";
            string strdayflag = "";
            //  string regularflag = "";
            string genderflag = "";
            Master1 = "select * from Master_Settings where " + grouporusercode + "";

            mysql.Open();
            SqlDataReader mtrdr;

            SqlCommand mtcmd = new SqlCommand(Master1, mysql);
            string regularflag = "";
            mtrdr = mtcmd.ExecuteReader();
            while (mtrdr.Read())
            {
                if (mtrdr.HasRows == true)
                {
                    if (mtrdr["settings"].ToString() == "Roll No" && mtrdr["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (mtrdr["settings"].ToString() == "Register No" && mtrdr["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (mtrdr["settings"].ToString() == "Student_Type" && mtrdr["value"].ToString() == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                    if (mtrdr["settings"].ToString() == "sex" && mtrdr["value"].ToString() == "1")
                    {
                        Session["Sex"] = "1";
                    }

                    //if (mtrdr["settings"].ToString() == "General attend" && mtrdr["value"].ToString() == "1")
                    //{

                    //    option.SelectedValue = "1";
                    //}

                    //if (mtrdr["settings"].ToString() == "Absentees" && mtrdr["value"].ToString() == "1")
                    //{

                    //    option.SelectedValue = "2";

                    //    //PanelindBody.Visible = true;
                    //}


                    //if (mtrdr["settings"].ToString() == "RollNo" && mtrdr["value"].ToString() == "1")
                    //{

                    //    RadioButtonList1.SelectedValue = "1";

                    //}


                    //if (mtrdr["settings"].ToString() == "RegisterNo" && mtrdr["value"].ToString() == "1")
                    //{

                    //    RadioButtonList1.SelectedValue = "2";

                    //}

                    //if (mtrdr["settings"].ToString() == "Admission No" && mtrdr["value"].ToString() == "1")
                    //{

                    //    RadioButtonList1.SelectedValue = "3";

                    //}

                    if (mtrdr["settings"].ToString() == "General" && mtrdr["value"].ToString() == "1")
                    {

                        Session["flag"] = 0;

                    }
                    if (mtrdr["settings"].ToString() == "As Per Lesson" && mtrdr["value"].ToString() == "1")
                    {

                        Session["flag"] = 1;

                    }

                    if (mtrdr["settings"].ToString() == "Male" && mtrdr["value"].ToString() == "1")
                    {

                        genderflag = " and (app.sex='0'";
                    }
                    if (mtrdr["settings"].ToString() == "Female" && mtrdr["value"].ToString() == "1")
                    {
                        if (genderflag != "" && genderflag != "\0")
                        {
                            genderflag = genderflag + " or app.sex='1'";
                        }
                        else
                        {
                            genderflag = " and (app.sex='1'";
                        }

                    }

                    if (mtrdr["settings"].ToString() == "Days Scholor" && mtrdr["value"].ToString() == "1")
                    {
                        strdayflag = " and (r.Stud_Type='Day Scholar'";
                    }

                    if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                    {
                        if (strdayflag != null && strdayflag != "\0")
                        {
                            strdayflag = strdayflag + " or r.Stud_Type='Hostler'";
                        }
                        else
                        {
                            strdayflag = " and (r.Stud_Type='Hostler'";
                        }
                    }
                    if (mtrdr["settings"].ToString() == "Regular")
                    {
                        regularflag = "and ((r.mode=1)";

                        // Session["strvar"] = Session["strvar"] + " and (mode=1)";
                    }
                    if (mtrdr["settings"].ToString() == "Lateral")
                    {
                        if (regularflag != "")
                        {
                            regularflag = regularflag + " or (r.mode=3)";
                        }
                        else
                        {
                            regularflag = regularflag + " and ((r.mode=3)";
                        }
                        //Session["strvar"] = Session["strvar"] + " and (mode=3)";
                    }
                    if (mtrdr["settings"].ToString() == "Transfer")
                    {
                        if (regularflag != "")
                        {
                            regularflag = regularflag + " or (r.mode=2)";
                        }
                        else
                        {
                            regularflag = regularflag + " and ((r.mode=2)";
                        }
                        //Session["strvar"] = Session["strvar"] + " and (mode=2)";
                    }

                }
            }
            mtrdr.Close();
            mysql.Close();
            if (strdayflag != null && strdayflag != "")
            {
                strdayflag = strdayflag + ")";
            }
            Session["strvar"] = strdayflag;

            if (regularflag != "")
            {
                regularflag = regularflag + ")";
            }
            if (genderflag != "")
            {
                genderflag = genderflag + ")";
            }
            Session["strvar"] = Session["strvar"] + regularflag + genderflag;


        }

    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        // try
        {
            //FpEntry.Visible = false;//
            gview.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            //Added By Srinath 27/2/2013
            txtexcelname.Visible = false;
            lblrptname.Visible = false;

            frmlbl.Visible = false;
            tolbl.Visible = false;
            tofromlbl.Visible = false;
            lblnorec.Visible = false;

            btnclick();
            int temp_col = 0;
            //if (FpEntry.Sheets[0].ColumnCount > 0 && FpEntry.Sheets[0].RowCount > 0)//===========on 9/4/12//

            if(gview.HeaderRow.Cells.Count>0&&gview.Rows.Count>0)            
            {
                for (temp_col = 0; temp_col < gview.Columns.Count; temp_col++)
                {
                    gview.HeaderRow.Cells[temp_col].Visible = true;
                }

                if (Session["Regflag"].ToString() == "0")
                {
                    gview.HeaderRow.Cells[2].Visible = false;
                }
                if (Session["Studflag"].ToString() == "0")
                {
                    gview.HeaderRow.Cells[temp_col].Visible = false;
                }

                final_print_col_cnt = 0;
                for (temp_col = 0; temp_col < gview.HeaderRow.Cells.Count; temp_col++)
                {
                    if (gview.HeaderRow.Cells[temp_col].Visible == true)
                    {
                        final_print_col_cnt++;
                    }
                }
                gview.Width = final_print_col_cnt * 100;

            }

        }
        //  catch
        {
        }
    }


    public void btnclick()
    {
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;

        frmlbl.Visible = false;
        tolbl.Visible = false;
        tofromlbl.Visible = false;
        lblnorec.Visible = false;
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

        date1 = txtFromDate.Text;
        if (date1.Trim() != "")
        {
            string[] split = date1.Split(new Char[] { '/' });
            if (split.GetUpperBound(0) == 2)//-------date valid
            {
                if (Convert.ToInt16(split[0].ToString()) <= 31 && Convert.ToInt16(split[1].ToString()) <= 12 && Convert.ToInt16(split[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                {
                    datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                    date2 = txtToDate.Text.ToString();
                    if (date2.Trim() != "")
                    {
                        string[] split1_1 = date2.Split(new Char[] { '/' });
                        if (split1.GetUpperBound(0) == 2)//--date valid
                        {
                            if (Convert.ToInt16(split1_1[0].ToString()) <= 31 && Convert.ToInt16(split1_1[1].ToString()) <= 12 && Convert.ToInt16(split1_1[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                            {

                                TimeSpan ts = dttodate.Subtract(dtfromdate);
                                int days = ts.Days;
                                if (days < 0)
                                {
                                    tofromlbl.Text = "From Date Must Be Less Than To Date";
                                    tofromlbl.Visible = true;

                                    //FpEntry.Visible = false;//
                                    gview.Visible = false;
                                    btnxl.Visible = false;
                                    btnprintmaster.Visible = false;
                                    Printcontrol.Visible = false;
                                    //Added By Srinath 27/2/2013
                                    txtexcelname.Visible = false;
                                    lblrptname.Visible = false;
                                    lblnorec.Visible = false;
                                }
                                else
                                {
                                    lblnorec.Text = "";
                                    lblnorec.Visible = false;
                                    btnxl.Visible = true;
                                    btnprintmaster.Visible = true;
                                    Printcontrol.Visible = false;
                                    //Added By Srinath 27/2/2013
                                    txtexcelname.Visible = true;
                                    lblrptname.Visible = true;
                                    //FpEntry.Visible = true;//
                                    gview.Visible = true;
                                    gobutton();
                                    if (sflag == true)
                                    {
                                        //FpEntry.Visible = true;//
                                        gview.Visible = true;
                                        btnxl.Visible = true;
                                        btnprintmaster.Visible = true;
                                        Printcontrol.Visible = false;
                                        //Added By Srinath 27/2/2013
                                        txtexcelname.Visible = true;
                                        lblrptname.Visible = true;

                                        lblnorec.Visible = false;
                                    }
                                    else
                                    {
                                        //FpEntry.Visible = false;//
                                        gview.Visible = false;
                                        btnxl.Visible = false;
                                        btnprintmaster.Visible = false;
                                        Printcontrol.Visible = false;
                                        //Added By Srinath 27/2/2013
                                        txtexcelname.Visible = false;
                                        lblrptname.Visible = false;

                                        lblnorec.Visible = true;
                                        lblnorec.Text = "No Record(s) Found";
                                    }
                                }
                            }
                            else
                            {
                                //FpEntry.Visible = false;//
                                gview.Visible = false;
                                btnxl.Visible = false;
                                btnprintmaster.Visible = false;
                                Printcontrol.Visible = false;
                                //Added By Srinath 27/2/2013
                                txtexcelname.Visible = false;
                                lblrptname.Visible = false;

                                frmlbl.Visible = false;
                                tolbl.Visible = true;
                                tofromlbl.Visible = false;
                                lblnorec.Visible = false;
                                tolbl.Text = "Enter Valid To Date";
                            }
                        }
                        else
                        {
                            //FpEntry.Visible = false;//
                            gview.Visible = false;
                            btnxl.Visible = false;
                            btnprintmaster.Visible = false;
                            Printcontrol.Visible = false;
                            //Added By Srinath 27/2/2013
                            txtexcelname.Visible = false;
                            lblrptname.Visible = false;

                            frmlbl.Visible = false;
                            tolbl.Visible = true;
                            tofromlbl.Visible = false;
                            lblnorec.Visible = false;
                            tolbl.Text = "Enter Valid To Date";
                        }
                    }
                    else
                    {
                        //FpEntry.Visible = false;//
                        gview.Visible = false;
                        btnxl.Visible = false;
                        btnprintmaster.Visible = false;
                        Printcontrol.Visible = false;
                        //Added By Srinath 27/2/2013
                        txtexcelname.Visible = false;
                        lblrptname.Visible = false;

                        frmlbl.Visible = false;
                        tolbl.Visible = true;
                        tofromlbl.Visible = false;
                        lblnorec.Visible = false;
                        tolbl.Text = "Enter To Date";
                    }
                }
                else
                {
                    //FpEntry.Visible = false;//
                    gview.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    Printcontrol.Visible = false;
                    //Added By Srinath 27/2/2013
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;

                    frmlbl.Visible = false;
                    tolbl.Visible = false;
                    tofromlbl.Visible = true;
                    lblnorec.Visible = false;
                    tolbl.Text = "Enter Valid From Date";
                }
            }
            else
            {
                //FpEntry.Visible = false;//
                gview.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                Printcontrol.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;

                frmlbl.Visible = false;
                tolbl.Visible = false;
                tofromlbl.Visible = true;
                lblnorec.Visible = false;
                tolbl.Text = "Enter Valid From Date";
            }
        }
        else
        {
            //FpEntry.Visible = false;//
            gview.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            //Added By Srinath 27/2/2013
            txtexcelname.Visible = false;
            lblrptname.Visible = false;

            frmlbl.Visible = false;
            tolbl.Visible = false;
            tofromlbl.Visible = true;
            lblnorec.Visible = false;
            tolbl.Text = "Enter From Date";
        }

    }
    public void gobutton()
    {

        //FpEntry.CurrentPage = 0;//
        string date1 = "", date2 = "";
        string datefrom, dateto;
        string sec_txt = "";
        int intNHrs = 0;
        string bind_sql = "";
        int row_cnt = 0;
        int period_cnt = 0;
        int day_diff = 0;
        int date_day = 0;
        int date_mnth = 0;
        int date_yr = 0;
        int tot_mnth = 0;
        string row_date = "";
        string sql = "";
        int col_cnt = 0;
        string disp_text = "";
        double pres = 0;
        double nop = 0;
        double noa = 0;
        double perc = 0;
        double noh = 0;
        double now1 = 0;
        DateTime today;
        int yy = 0;

        //FpEntry.Width = 845;
        //FpEntry.Height = 1500;//
        gview.Height = 1500;
        string dum_tage_date = "";
        string dum_tage_hrs = "";
        int s_no = 0;
        string sections = "";
        string strsec = "";
        double new_variable = 0;
        /*****************************************/
        //FpEntry.Visible = true;//
        gview.Visible = false;
        btnxl.Visible = true;
        btnprintmaster.Visible = true;
        Printcontrol.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = true;
        lblrptname.Visible = true;
        

        /*****************************************/





        MyImg mi = new MyImg();
        mi.ImageUrl = "~/images/10BIT001.jpeg";
        mi.ImageUrl = "Handler/Handler2.ashx?";
        MyImg mi2 = new MyImg();
        mi2.ImageUrl = "~/images/10BIT001.jpeg";
        mi2.ImageUrl = "Handler/Handler5.ashx?";


        //FpEntry.Sheets[0].RowCount = 0;//
        date1 = txtFromDate.Text.ToString();
        string[] split = date1.Split(new Char[] { '/' });


        if (split.GetUpperBound(0) == 2)//-------date valid
        {
            if (Convert.ToInt16(split[0].ToString()) <= 31 && Convert.ToInt16(split[1].ToString()) <= 12 && Convert.ToInt16(split[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
            {
                datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                date2 = txtToDate.Text.ToString();
                string[] split1 = date2.Split(new Char[] { '/' });
                if (split1.GetUpperBound(0) == 2)//--date valid
                {
                    if (Convert.ToInt16(split1[0].ToString()) <= 31 && Convert.ToInt16(split1[1].ToString()) <= 12 && Convert.ToInt16(split1[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                    {
                        dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                        DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                        DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                        TimeSpan t = dt2.Subtract(dt1);
                        long days = t.Days;
                        if (days >= 0)//-----check date difference
                        {


                            //  logoset();

                            //=============================0n 9/4/12
                            has.Clear();
                            has.Add("college_code", Session["collegecode"].ToString());
                            has.Add("form_name", "Attendance_overall.aspx");
                            dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", has, "sp");
                            //===========================================


                            //======================0n 11/4/12 PRABHA
                            if (dsprint.Tables[0].Rows.Count > 0)
                            {
                                isonumber = dsprint.Tables[0].Rows[0]["ISOCode"].ToString();
                                if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
                                {
                                    
                                    new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                                    new_header_string_split = new_header_string.Split(',');
                                    
                                }
                            }
                            //=====================================

                            Boolean first_stud = false;


                            sections = ddlSec.SelectedValue.ToString();
                            //'---------------------------------------------------------
                            bind_con.Close();
                            bind_con.Open();

                            dt.Columns.Add("S.No");
                            dt.Columns.Add("Roll No");
                            dt.Columns.Add("Reg No");
                            dt.Columns.Add("Name of the Student");
                            dt.Columns.Add("Stud Type");
                            dt.Columns.Add("Conducted Periods");
                            dt.Columns.Add("Periods Present");
                            dt.Columns.Add("OD Periods");
                            dt.Columns.Add("Total Periods");
                            dt.Columns.Add("Periods Absent");
                            dt.Columns.Add("% of Attendance");

                            dtrow = dt.NewRow();
                            dtrow["S.No"] = "S.No";
                            dtrow["Roll No"] = "Roll No";
                            dtrow["Reg No"] = "Reg No";
                            dtrow["Name of the Student"] = "Name of the Student";
                            dtrow["Stud Type"] = "Stud Type";
                            dtrow["Conducted Periods"] = "Conducted Periods";
                            dtrow["Periods Present"] = "Periods Present";
                            dtrow["OD Periods"] = "OD Periods";
                            dtrow["Total Periods"] = "Total Periods";
                            dtrow["Periods Absent"] = "Periods Absent";
                            dtrow["% of Attendance"] = "% of Attendance";
                            dt.Rows.Add(dtrow);
                            //added By Srinath 15/8/2013

                            string strorder = "ORDER BY len(roll_no),roll_no";
                            string strserial = d2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
                            if (strserial != "" && strserial != "0" && strserial != null)
                            {

                                strorder = "ORDER BY serialno";
                            }
                            else
                            {
                                string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                                if (orderby_Setting == "0")
                                {
                                    strorder = "ORDER BY len(roll_no),roll_no";
                                }
                                else if (orderby_Setting == "1")
                                {
                                    strorder = "ORDER BY len(Reg_No),reg_no";
                                }
                                else if (orderby_Setting == "2")
                                {
                                    strorder = "ORDER BY Stud_Name";
                                }
                                else if (orderby_Setting == "0,1,2")
                                {
                                    strorder = "ORDER BY len(roll_no),roll_no,len(Reg_No),reg_no,r.stud_name";
                                }
                                else if (orderby_Setting == "0,1")
                                {
                                    strorder = "ORDER BY len(roll_no),roll_no,len(Reg_No),reg_no";
                                }
                                else if (orderby_Setting == "1,2")
                                {
                                    strorder = "ORDER BY len(Reg_No),reg_no,Stud_Name";
                                }
                                else if (orderby_Setting == "0,2")
                                {
                                    strorder = "ORDER BY len(roll_no),roll_no,Stud_Name";
                                }
                            }

                            if (sections != "")
                            {
                                bind_sql = "select roll_no,reg_no,stud_name,stud_type,len(roll_no), convert(varchar(15),adm_date,103) as adm_date,len(Reg_No),serialno  from registration where degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar' and registration.sections='" + sections + "' " + strorder + "";
                            }
                            else
                            {
                                bind_sql = "select roll_no,reg_no,stud_name,stud_type,len(roll_no), convert(varchar(15),adm_date,103) as adm_date,len(Reg_No),serialno from registration where degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar'  " + strorder + "";
                            }

                            //if (sections != "")
                            //{
                            //    bind_sql = "select roll_no,reg_no,stud_name,stud_type,len(roll_no), convert(varchar(15),adm_date,103) as adm_date from registration where degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar' and registration.sections='" + sections + "' order by (roll_no),reg_no,stud_name";
                            //}
                            //else
                            //{
                            //    bind_sql = "select roll_no,reg_no,stud_name,stud_type,len(roll_no), convert(varchar(15),adm_date,103) as adm_date from registration where degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar' order by (roll_no),reg_no,stud_name";
                            //}
                            studinfocmd = new SqlCommand(bind_sql, bind_con);

                            studinfors = studinfocmd.ExecuteReader();
                            if (studinfors.HasRows == true)
                            {
                                string strsplsec = "";
                                if (sections != "")
                                {
                                    strsplsec = " and Sections='" + sections + "'";

                                }
                                else
                                {
                                    strsplsec = "";
                                }
                                //added By srinath 18/2/2013 ===========STart
                                string[] fromdatespit = txtFromDate.Text.Split('/');
                                string[] todatespit = txtToDate.Text.Split('/');
                                DateTime spfromdate = Convert.ToDateTime(fromdatespit[1] + '/' + fromdatespit[0] + '/' + fromdatespit[2]);
                                DateTime sptodate = Convert.ToDateTime(todatespit[1] + '/' + todatespit[0] + '/' + todatespit[2]);
                                ht_sphr.Clear();
                                string hrdetno = "";
                                string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and date between '" + spfromdate.ToString() + "' and '" + sptodate.ToString() + "' " + strsplsec + "";
                                ds_sphr = d2.select_method(getsphr, hat, "Text");
                                if (ds_sphr.Tables[0].Rows.Count > 0)
                                {
                                    for (int sphr = 0; sphr < ds_sphr.Tables[0].Rows.Count; sphr++)
                                    {
                                        if (ht_sphr.Contains(Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"])))
                                        {
                                            hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"]), ht_sphr));
                                            hrdetno = hrdetno + "," + Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["hrdet_no"]);
                                            ht_sphr[Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"])] = hrdetno;
                                        }
                                        else
                                        {
                                            ht_sphr.Add(Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["date"]), Convert.ToString(ds_sphr.Tables[0].Rows[sphr]["hrdet_no"]));
                                        }
                                    }
                                }

                                //'----------------------------------------new start----------------

                                hat.Clear();
                                hat.Add("degree_code", ddlBranch.SelectedValue.ToString());
                                hat.Add("sem_ester", int.Parse(ddlSemYr.SelectedValue.ToString()));


                                ds = d2.select_method("period_attnd_schedule", hat, "sp");
                                if (ds.Tables[0].Rows.Count != 0)
                                {
                                    NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                                    fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                                    anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                                    minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                                    minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                                }
                                hat.Clear();
                                hat.Add("colege_code", Session["collegecode"].ToString());
                                ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
                                countds = ds1.Tables[0].Rows.Count;

                                //'------------------------------cal the func for find the att %
                                //=========== End



                                while (studinfors.Read())
                                {
                                    first_stud = true;
                                    lblnorec.Text = "";
                                    lblnorec.Visible = false;
                                    roll = studinfors["roll_no"].ToString();

                                    string admdate = studinfors["adm_date"].ToString();
                                    string[] admdatesp = admdate.Split(new Char[] { '/' });
                                    admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
                                    Admission_date = Convert.ToDateTime(admdate);

                                    //'----------------------------------------new start----------------

                                    //Hidden By Srinath 25/2/2013 ======Start
                                    //hat.Clear();
                                    //hat.Add("degree_code", ddlBranch.SelectedValue.ToString());
                                    //hat.Add("sem_ester", int.Parse(ddlSemYr.SelectedValue.ToString()));


                                    //ds = d2.select_method("period_attnd_schedule", hat, "sp");
                                    //if (ds.Tables[0].Rows.Count != 0)
                                    //{
                                    //    NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                                    //    fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                                    //    anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                                    //    minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                                    //    minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                                    //}
                                    //hat.Clear();
                                    //hat.Add("colege_code", Session["collegecode"].ToString());
                                    //ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
                                    //countds = ds1.Tables[0].Rows.Count;
                                    //==============End 
                                    //'------------------------------cal the func for find the att %
                                    persentmonthcal_new();



                                    per_tage_date = ((pre_present_date / per_workingdays) * 100);
                                    if (per_tage_date > 100)
                                    {
                                        per_tage_date = 100;
                                    }

                                    //per_con_hrs = ((per_workingdays * NoHrs) - per_dum_unmark);
                                    //per_tage_hrs = ((per_per_hrs / per_con_hrs) * 100);

                                    //  per_con_hrs = ((conducted_hrs_new - (per_holidate * NoHrs)) - per_dum_unmark);
                                    per_con_hrs = (per_workingdays1 - per_dum_unmark) + tot_conduct_hr_spl;
                                    per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl) / per_con_hrs) * 100);


                                    //if (first_stud == true)
                                    //{
                                    //    first_stud = false;
                                    //    max_cond_hr = per_con_hrs;
                                    //}
                                    //else
                                    //{
                                    //    if (max_cond_hr < per_con_hrs)
                                    //    {
                                    //        max_cond_hr = per_con_hrs;
                                    //    }
                                    //}

                                    // if (yy == 0)
                                    if (max_cond_hr < per_con_hrs)
                                    {
                                        max_cond_hr = per_con_hrs;
                                        //Modified By SRinath 15/5/2013   
                                        //FpEntry.Sheets[0].ColumnHeader.Cells[7, 7].Text = "Total number of working hours : " + per_con_hrs;
                                        //FpEntry.Sheets[0].ColumnHeader.Cells[7, 7].HorizontalAlign = HorizontalAlign.Left;
                                        //FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 1), 7].Text = "Total number of working Periods :  " + max_cond_hr + " (a), Attended Periods d=(b+c)"; // + per_con_hrs;
                                        //FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 1), 7].HorizontalAlign = HorizontalAlign.Left;
                                        //FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 1), 7].Tag = max_cond_hr.ToString();//16/4/2014//
                                        new_variable = per_con_hrs;
                                    }
                                    yy++;

                                    if (per_tage_hrs > 100)
                                    {
                                        per_tage_hrs = 100;
                                    }

                                    dum_tage_date = String.Format("{0:0,0.00}", float.Parse(per_tage_date.ToString()));
                                    dum_tage_hrs = String.Format("{0:0,0.00}", float.Parse(per_tage_hrs.ToString()));

                                    if (dum_tage_hrs == "NaN")
                                    {
                                        dum_tage_hrs = "0";
                                    }
                                    else if (dum_tage_hrs == "Infinity")
                                    {
                                        dum_tage_hrs = "0";
                                    }

                                    if (dum_tage_date == "NaN")
                                    {
                                        dum_tage_date = "0";
                                    }
                                    else if (dum_tage_date == "Infinity")
                                    {
                                        dum_tage_date = "0";
                                    }

                                    //'------------------------------------------------new end------------

                                    lblnorec.Text = "";
                                    lblnorec.Visible = false;
                                    sflag = true;
                                    s_no++;

                                    

                                    dtrow = dt.NewRow();

                                    dtrow["S.No"] = s_no.ToString();

                                    dtrow["Roll No"] = studinfors["roll_no"].ToString();

                                    dtrow["Reg No"] = studinfors["reg_no"].ToString();

                                    dtrow["Name of the Student"] = studinfors["stud_name"].ToString();

                                    dtrow["Stud Type"] = studinfors["stud_type"].ToString();

                                    dtrow["Periods Present"] = (per_per_hrs + tot_per_hrs_spl).ToString();

                                    dtrow["OD Periods"] = (per_tot_ondu + tot_ondu_spl).ToString();

                                    dtrow["Total Periods"] = (Convert.ToDouble(per_per_hrs + tot_per_hrs_spl) + Convert.ToDouble(per_tot_ondu + tot_ondu_spl)).ToString();

                                    for (int j = 0; j < countds; j++)
                                    {

                                        if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == "3")
                                        {
                                            ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                        }
                                    }
                                    double d = 0;
                                    if (ObtValue != 2)
                                    {
                                        d = (Convert.ToDouble(per_per_hrs + tot_per_hrs_spl) + Convert.ToDouble(per_tot_ondu + tot_ondu_spl));
                                    }
                                    else
                                    {
                                        d = (Convert.ToDouble(per_per_hrs + tot_per_hrs_spl));
                                    }
                                    //modified by srinath 1/9/2014
                                    //  double avg_temp = (d /( new_variable )) * 100;
                                    // double avg_temp = (d / (new_variable - per_dum_unmark)) * 100;
                                    double avg_temp = (d / (per_con_hrs)) * 100;
                                    double avg = Math.Round(avg_temp, 2);
                                    if (avg + "" == "NaN")
                                    {
                                        avg = 0;
                                    }

                                    //*******End****//
                                    //  FpEntry.Sheets[0].Cells[row_cnt - 1, 8].Text = mmm.ToString();
                                    //  FpEntry.Sheets[0].Cells[row_cnt - 1, 8].Text = tot_absent_houes .ToString();per_abshrs


                                    dtrow["Conducted Periods"] = per_con_hrs.ToString();

                                    dtrow["Periods Absent"] = (per_abshrs + per_abshrs_spl).ToString();

                                    dtrow["% of Attendance"] = avg.ToString();

                                    dt.Rows.Add(dtrow);

                                    lblnorec.Visible = false;
                                    //}                                      
                                }
                                gview.DataSource = dt;
                                gview.DataBind();
                                gview.Visible = true;

                                gview.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                gview.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                                gview.Rows[0].Font.Bold = true;
                                gview.Rows[0].Font.Name = "Book Antique";

                                for (int i = 1; i < gview.Rows.Count; i++)
                                {
                                    for (int cell = 0; cell < gview.Rows[i].Cells.Count; cell++)
                                    {
                                        if (gview.HeaderRow.Cells[cell].Text != "Name of the Student" && gview.HeaderRow.Cells[cell].Text != "Stud Type")
                                        {
                                            gview.Rows[i].Cells[cell].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                }

                                    if (Convert.ToInt32(gview.Rows.Count) != 0)
                                    {

                                        double totalRows = 0;

                                        totalRows = Convert.ToInt32(gview.Rows.Count);


                                        if (totalRows >= 10)
                                        {

                                            gview.PageSize = 10;




                                            gview.PageSize = (Convert.ToInt32(totalRows));
                                        }
                                        else if (totalRows == 0)
                                        {

                                        }
                                        else
                                        {

                                            gview.PageSize = (Convert.ToInt32(totalRows));

                                        }

                                        if (Convert.ToInt32(gview.Rows.Count) > 10)
                                        {

                                        }

                                        Session["totalPages"] = (int)Math.Ceiling(totalRows / gview.PageSize);
                                    }
                            }
                            else
                            {
                                lblnorec.Text = "No Record(S) Found";
                                lblnorec.Visible = true;
                                
                                gview.Visible = false;
                                btnxl.Visible = false;
                                btnprintmaster.Visible = false;
                                Printcontrol.Visible = false;
                                //Added By Srinath 27/2/2013
                                txtexcelname.Visible = false;
                                lblrptname.Visible = false;
                            }
                            //'------------------------------
                        }
                    }
                }

            }

        }

    }

    public void persentmonthcal_new()
    {
        try
        {
            Boolean isadm = false;
            per_abshrs_spl = 0;
            tot_per_hrs_spl = 0;
            tot_ondu_spl = 0;
            per_hhday_spl = 0;
            unmark_spl = 0;
            tot_conduct_hr_spl = 0;

            per_abshrs = 0;
            tot_absent_houes = 0;
            per_workingdays1 = 0;
            mng_conducted_half_days = 0;
            evng_conducted_half_days = 0;
            njdate_mng = 0;
            njdate_evng = 0;
            notconsider_value = 0;
            conducted_hrs_new = 0;
            workingdays = 0;
            per_holidate_mng = 0;
            per_holidate_evng = 0;
            notconsider_value = 0;
            int demfcal, demtcal;
            //Added By Srinath 25/2/2013 ==Start
            if (datechk != true)
            {
                datechk = true;
                //===End
                frdate = txtFromDate.Text.ToString();
                todate = txtToDate.Text.ToString();
                string dt = frdate;
                string[] dsplit = dt.Split(new Char[] { '/' });
                frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                demfcal = int.Parse(dsplit[2].ToString());
                demfcal = demfcal * 12;
                cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                dt = todate;
                dsplit = dt.Split(new Char[] { '/' });
                todate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                demtcal = int.Parse(dsplit[2].ToString());
                demtcal = demtcal * 12;
                cal_to_date = demtcal + int.Parse(dsplit[1].ToString());

                //Modified  By Srinath 25/2/2013 =========Start
                //per_from_date = Convert.ToDateTime(frdate);
                //per_to_date = Convert.ToDateTime(todate);
                //dumm_from_date = per_from_date;
                //Added By Srinath ==Start
                tempfromdate = frdate;
                tempcallfromdate = cal_from_date;
            }

            frdate = tempfromdate;
            cal_from_date = tempcallfromdate;
            per_from_date = Convert.ToDateTime(frdate);
            per_to_date = Convert.ToDateTime(todate);
            dumm_from_date = per_from_date;
            //===============End
            hat.Clear();

            hat.Add("std_rollno", roll.ToString());
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds2 = d2.select_method("STUD_ATTENDANCE", hat, "sp");

            //added By Srinath 25/2/2013 =============Start
            mmyycount = ds2.Tables[0].Rows.Count;
            moncount = mmyycount - 1;
            //End

            chkdegreesem = ddlBranch.SelectedValue.ToString() + '/' + ddlSemYr.SelectedItem.ToString();
            if (tempdegreesem != chkdegreesem)
            {
                tempdegreesem = chkdegreesem;
                hat.Clear();
                hat.Add("degree_code", ddlBranch.SelectedValue.ToString());
                hat.Add("sem", int.Parse(ddlSemYr.SelectedItem.ToString()));
                hat.Add("from_date", frdate.ToString());
                hat.Add("to_date", todate.ToString());
                hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));

                //------------------------------------------------------------------
                int iscount = 0;
                holidaycon.Close();
                holidaycon.Open();
                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedItem.ToString() + "";
                SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, holidaycon);
                SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);
                DataSet dsholiday = new DataSet();
                daholiday.Fill(dsholiday);
                if (dsholiday.Tables[0].Rows.Count > 0)
                {
                    iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                }
                hat.Add("iscount", iscount);
                //hiden By Srinath 25/2/2013
                //mmyycount = ds2.Tables[0].Rows.Count;
                //moncount = mmyycount - 1;

                ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");

                Dictionary<string, int> holiday_table1 = new Dictionary<string, int>();
                Dictionary<string, int> holiday_table2 = new Dictionary<string, int>();
                Dictionary<string, int> holiday_table3 = new Dictionary<string, int>();

                holiday_table11.Clear();
                holiday_table21.Clear();
                holiday_table31.Clear();
                if (ds3.Tables[0].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[0].Rows.Count; k++)
                    {
                        if (ds3.Tables[0].Rows[0]["halforfull"].ToString() == "False")
                        {
                            halforfull = "0";
                        }
                        else
                        {
                            halforfull = "1";
                        }
                        if (ds3.Tables[0].Rows[0]["morning"].ToString() == "False")
                        {
                            mng = "0";
                        }
                        else
                        {
                            mng = "1";
                        }
                        if (ds3.Tables[0].Rows[0]["evening"].ToString() == "False")
                        {
                            evng = "0";
                        }
                        else
                        {
                            evng = "1";
                        }

                        holiday_sched_details = halforfull + "*" + mng + "*" + evng;

                        string[] split_date_time1 = ds3.Tables[0].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                        // holiday_table1.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], holiday_sched_details);
                    }
                }

                if (ds3.Tables[1].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[1].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        holiday_table21.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);

                        if (ds3.Tables[1].Rows[k]["halforfull"].ToString() == "False")
                        {
                            halforfull = "0";
                        }
                        else
                        {
                            halforfull = "1";
                        }
                        if (ds3.Tables[1].Rows[k]["morning"].ToString() == "False")
                        {
                            mng = "0";
                        }
                        else
                        {
                            mng = "1";
                        }
                        if (ds3.Tables[1].Rows[k]["evening"].ToString() == "False")
                        {
                            evng = "0";
                        }
                        else
                        {
                            evng = "1";
                        }

                        holiday_sched_details = halforfull + "*" + mng + "*" + evng;

                        if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                        }
                        holiday_table2.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                    }
                }

                if (ds3.Tables[2].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[2].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[2].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        holiday_table31.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);

                        if (ds3.Tables[2].Rows[k]["halforfull"].ToString() == "False")
                        {
                            halforfull = "0";
                        }
                        else
                        {
                            halforfull = "1";
                        }
                        if (ds3.Tables[2].Rows[k]["morning"].ToString() == "False")
                        {
                            mng = "0";
                        }
                        else
                        {
                            mng = "1";
                        }
                        if (ds3.Tables[2].Rows[k]["evening"].ToString() == "False")
                        {
                            evng = "0";
                        }
                        else
                        {
                            evng = "1";
                        }

                        holiday_sched_details = halforfull + "*" + mng + "*" + evng;

                        if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                        }

                        holiday_table3.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                    }
                }


                //Boolean splhr_flag = false; //Hidden By Srinath 25/2/2013
                //=====================================4/5/12 PRABHA
                con.Close();
                cmd = new SqlCommand("select rights from  special_hr_rights where " + grouporusercode + "", con);
                //  cmd.Connection = con;
                con.Open();
                SqlDataReader dr_rights_spl_hr = cmd.ExecuteReader();
                if (dr_rights_spl_hr.HasRows)
                {
                    while (dr_rights_spl_hr.Read())
                    {
                        string spl_hr_rights = "";
                        Hashtable od_has = new Hashtable();

                        spl_hr_rights = dr_rights_spl_hr["rights"].ToString();
                        if (spl_hr_rights == "True" || spl_hr_rights == "true")
                        {
                            splhr_flag = true;
                            //getspecial_hr();
                        }
                    }
                }
            }//Added By Srinath 23/2/2013 
            //===================================
            //------------------------------------------------------------------
            if (ds3.Tables[0].Rows.Count != 0)
            {
                ts = DateTime.Parse(ds3.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                diff_date = Convert.ToString(ts.Days);
                dif_date1 = double.Parse(diff_date.ToString());
            }
            next = 0;

            if (ds2.Tables[0].Rows.Count != 0)
            {
                int rowcount = 0;
                int ccount;
                ccount = ds3.Tables[1].Rows.Count;
                ccount = ccount - 1;
                //if ( == ds2.Tables [0].Rows [mmyycount].["Month_year"])
                tempvalue = "";
                while (dumm_from_date <= (per_to_date))
                {
                    isadm = false;
                    if (dumm_from_date >= Admission_date)
                    {
                        isadm = true;
                        if (splhr_flag == true)
                        {
                            //modified By Srinath 18/2/2013
                            if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
                            {
                                getspecial_hr();
                            }
                        }

                        for (int i = 1; i <= mmyycount; i++)
                        {
                            if (cal_from_date == int.Parse(ds2.Tables[0].Rows[next]["month_year"].ToString()))
                            {
                                string[] split_date_time1 = dumm_from_date.ToString().Split(' ');
                                string[] dummy_split = split_date_time1[0].Split('/');


                                if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()))
                                {
                                    holiday_table11.Add(((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()), "3*0*0");
                                }

                                if (holiday_table11.Contains((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()))
                                {
                                    value_holi_status = GetCorrespondingKey(dummy_split[1].ToString() + "/" + dummy_split[0].ToString() + "/" + dummy_split[2].ToString(), holiday_table11).ToString();
                                    split_holiday_status = value_holi_status.Split('*');

                                    if (split_holiday_status[0].ToString() == "3")//=========ful day working day
                                    {
                                        split_holiday_status_1 = "1";
                                        split_holiday_status_2 = "1";
                                    }
                                    else if (split_holiday_status[0].ToString() == "1")//=============half day working day
                                    {
                                        if (split_holiday_status[1].ToString() == "1")//==============mng holiday//evng working day
                                        {
                                            split_holiday_status_1 = "0";
                                            split_holiday_status_2 = "1";
                                        }

                                        if (split_holiday_status[2].ToString() == "1")//==============evng holiday//mng working day
                                        {
                                            split_holiday_status_1 = "1";
                                            split_holiday_status_2 = "0";
                                        }
                                    }
                                    else if (split_holiday_status[0].ToString() == "0")
                                    {
                                        dumm_from_date = dumm_from_date.AddDays(1);
                                        if (dumm_from_date.Day == 1)
                                        {
                                            cal_from_date++;
                                            if (moncount > next)
                                            {
                                                next++;

                                            }
                                        }
                                        break;
                                    }

                                    if (ds3.Tables[1].Rows.Count != 0)
                                    {
                                        ts = DateTime.Parse(ds3.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                                        diff_date = Convert.ToString(ts.Days);
                                        dif_date = double.Parse(diff_date.ToString());
                                    }
                                    else
                                    {
                                        dif_date = 0;
                                    }



                                    if (split_holiday_status_1 == "1")
                                    {

                                        for (i = 1; i <= fnhrs; i++)
                                        {
                                            date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                            value = ds2.Tables[0].Rows[next][date].ToString();
                                            if (value != null && value != "0" && value != "7" && value != "")
                                            {
                                                if (tempvalue != value)
                                                {
                                                    tempvalue = value;
                                                    for (int j = 0; j < countds; j++)
                                                    {

                                                        if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                                        {
                                                            ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                            j = countds;
                                                        }
                                                    }
                                                }
                                                if (ObtValue == 1)
                                                {
                                                    per_abshrs += 1;
                                                }
                                                else if (ObtValue == 2)
                                                {
                                                    notconsider_value += 1;
                                                    njhr += 1;
                                                }

                                                else if (ObtValue == 0)
                                                {
                                                    if (value != "3")
                                                    {
                                                        per_perhrs += 1;
                                                        tot_per_hrs += 1;
                                                    }
                                                }
                                                if (value == "3")
                                                {
                                                    per_ondu += 1;
                                                    tot_ondu += 1;
                                                }
                                                else if (value == "10")
                                                {
                                                    per_leave += 1;
                                                }
                                            }
                                            else if (value == "7")
                                            {
                                                per_hhday += 1;

                                            }
                                            else
                                            {
                                                unmark += 1;
                                            }
                                        }

                                        //  if (per_perhrs >= minpresI)
                                        if (per_perhrs + njhr >= minpresI)
                                        {
                                            Present += 0.5;
                                        }

                                        else if (per_leave >= 1)
                                        {
                                            leave_point += leave_pointer / 2;
                                            Leave += 0.5;
                                        }

                                        else if (per_abshrs >= 1)
                                        {
                                            Absent += 0.5;
                                            absent_point += absent_pointer / 2;
                                        }
                                        else if (njhr >= minpresI)
                                        {
                                            njdate += 0.5;
                                            njdate_mng += 1;
                                        }
                                        if (per_ondu >= 1)
                                        {
                                            Onduty += 0.5;
                                        }

                                        if (unmark == fnhrs)
                                        {
                                            per_holidate_mng += 1;
                                            per_holidate += 0.5;
                                            unmark = 0;
                                        }

                                        workingdays += 0.5;
                                        mng_conducted_half_days += 1;

                                    }
                                    per_perhrs = 0;
                                    per_ondu = 0;
                                    per_leave = 0;
                                    //   per_abshrs = 0;
                                    // unmark = 0;
                                    njhr = 0;
                                    int temp_unmark = 0;
                                    int k = fnhrs + 1;

                                    if (split_holiday_status_2 == "1")
                                    {
                                        for (i = k; i <= NoHrs; i++)
                                        {
                                            date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                            value = ds2.Tables[0].Rows[next][date].ToString();

                                            if (value != null && value != "0" && value != "7" && value != "")
                                            {
                                                if (tempvalue != value)
                                                {
                                                    tempvalue = value;
                                                    for (int j = 0; j < countds; j++)
                                                    {

                                                        if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                                        {
                                                            ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                                            j = countds;
                                                        }
                                                    }
                                                }
                                                if (ObtValue == 1)
                                                {
                                                    per_abshrs += 1;
                                                }
                                                else if (ObtValue == 2)
                                                {
                                                    notconsider_value += 1;
                                                    njhr += 1;
                                                }
                                                else if (ObtValue == 0)
                                                {
                                                    if (value != "3")
                                                    {
                                                        per_perhrs += 1;
                                                        tot_per_hrs += 1;
                                                    }
                                                }
                                                if (value == "3")
                                                {
                                                    per_ondu += 1;
                                                    tot_ondu += 1;
                                                }
                                                else if (value == "10")

                                                    per_leave += 1;
                                            }
                                            else if (value == "7")
                                            {
                                                per_hhday += 1;
                                            }
                                            else
                                            {
                                                unmark += 1;
                                            }
                                        }
                                        //   if (per_perhrs >= minpresII)
                                        if (per_perhrs + njhr >= minpresII)
                                        {
                                            Present += 0.5;
                                        }

                                        else if (per_leave >= 1)
                                        {

                                            leave_point += leave_pointer / 2;
                                            Leave += 0.5;
                                        }

                                        else if (per_abshrs >= 1)
                                        {
                                            Absent += 0.5;
                                            absent_point += absent_pointer / 2;
                                        }
                                        else if (njhr >= minpresII)
                                        {
                                            njdate_evng += 1;
                                            njdate += 0.5;
                                        }
                                        if (per_ondu >= 1)
                                        {
                                            Onduty += 0.5;
                                        }



                                        if (unmark == NoHrs - fnhrs)
                                        {
                                            per_holidate_evng += 1;
                                            per_holidate += 0.5;
                                            unmark = 0;
                                        }
                                        else
                                        {
                                            dum_unmark += unmark;
                                        }



                                        workingdays += 0.5;
                                        evng_conducted_half_days += 1;

                                    }

                                    per_perhrs = 0;
                                    per_ondu = 0;
                                    per_leave = 0;
                                    //per_abshrs = 0;
                                    unmark = 0;
                                    njhr = 0;


                                    dumm_from_date = dumm_from_date.AddDays(1);
                                    if (dumm_from_date.Day == 1)
                                    {
                                        cal_from_date++;
                                        if (moncount > next)
                                        {
                                            next++;

                                        }
                                    }


                                    per_perhrs = 0;

                                }

                            }
                            else
                            {

                                //DateTime dumm_fdate = dumm_from_date.AddDays(1 - Convert.ToInt16(dumm_from_date.Day.ToString()));
                                //dumm_fdate = dumm_fdate.AddMonths(1);
                                //dumm_from_date = dumm_fdate;

                                //if (dumm_from_date.Day == 1)
                                //{

                                //    cal_from_date++;


                                //    if (moncount > next)
                                //    {
                                //        //  next++;
                                //    }

                                //}

                                //if (moncount > next)
                                //{
                                //    i--;
                                //}

                                dumm_from_date = dumm_from_date.AddDays(1);
                                if (dumm_from_date.Day == 1)
                                {

                                    cal_from_date++;


                                    if (moncount > next)
                                    {
                                        next++; //  next++;
                                    }

                                }

                            }

                        }
                    }
                    if (isadm == false)
                    {
                        dumm_from_date = dumm_from_date.AddDays(1);
                        if (dumm_from_date.Day == 1)
                        {
                            cal_from_date++;
                            if (moncount > next)
                            {
                                next++;

                            }
                        }
                    }
                }
                int diff_Date = per_from_date.Day - dumm_from_date.Day;
            }


            per_tot_ondu = tot_ondu;
            per_njdate = njdate;
            pre_present_date = Present;
            per_per_hrs = tot_per_hrs;
            per_absent_date = Absent;
            pre_ondu_date = Onduty;
            pre_leave_date = Leave;
            per_workingdays = workingdays - per_holidate - per_njdate;
            per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - ((Convert.ToInt16(per_holidate_mng) * fnhrs) + (Convert.ToInt16(per_holidate_evng) * (NoHrs - fnhrs))) - notconsider_value;// ((Convert.ToInt16(njdate_mng) * fnhrs) + (Convert.ToInt16(njdate_evng) * (NoHrs - fnhrs)));

            // per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs)))) - notconsider_value;

            per_dum_unmark = dum_unmark;




            Present = 0;
            tot_per_hrs = 0;
            Absent = 0;
            Onduty = 0;
            Leave = 0;
            workingdays = 0;
            per_holidate = 0;
            dum_unmark = 0;
            absent_point = 0;
            leave_point = 0;
            njdate = 0;
            tot_ondu = 0;
        }
        catch
        {

        }
    }

    //public void logoset()
    //{
    //    SqlConnection con_header = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    //    string query_header = "";
    //    FpEntry.Sheets[0].SheetName = " ";
    //    FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
    //    style.Font.Size = 12;
    //    style.Font.Bold = true;
    //    FpEntry.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
    //    FpEntry.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
    //    FpEntry.Sheets[0].AllowTableCorner = true;


    //    FpEntry.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
    //    FpEntry.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;


    //    MyImg mi = new MyImg();
    //    mi.ImageUrl = "~/images/10BIT001.jpeg";
    //    mi.ImageUrl = "Handler/Handler2.ashx?";
    //    MyImg mi2 = new MyImg();
    //    mi2.ImageUrl = "~/images/10BIT001.jpeg";
    //    mi2.ImageUrl = "Handler/Handler5.ashx?";

    //    con_header.Close();
    //    con_header.Open();
    //    query_header = "select collname,category,affliatedby,address1,address2,address3,phoneno,faxno,email,website from collinfo where college_code=" + Session["collegecode"] + "";
    //    SqlCommand com_header = new SqlCommand(query_header, con_header);
    //    SqlDataReader sdr_header;
    //    sdr_header = com_header.ExecuteReader();
    //    while (sdr_header.Read())
    //    {

    //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 5, 1);
    //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;


    //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, 8);
    //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].Text = sdr_header["collname"].ToString();
    //       // FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;



    //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, 8);
    //        FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Text = sdr_header["category"].ToString() + ", Affliated to " + sdr_header["affliatedby"].ToString();
    //       // FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Font.Bold = true;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;

    //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, 8);
    //        FpEntry.Sheets[0].ColumnHeader.Cells[2, 1].Text = sdr_header["address1"].ToString() + "-" + sdr_header["address2"].ToString() + "-" + sdr_header["address1"].ToString();
    //        //FpEntry.Sheets[0].ColumnHeader.Cells[2, 1].Font.Bold = true;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[2, 1].HorizontalAlign = HorizontalAlign.Center;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;

    //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, 8);
    //        FpEntry.Sheets[0].ColumnHeader.Cells[3, 1].Text = "Phone : " + sdr_header["phoneno"].ToString() + "  Fax : " + sdr_header["faxno"].ToString();
    //       // FpEntry.Sheets[0].ColumnHeader.Cells[3, 1].Font.Bold = true;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[3, 1].HorizontalAlign = HorizontalAlign.Center;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;

    //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, 8);//5th row span
    //        FpEntry.Sheets[0].ColumnHeader.Cells[4, 1].Text = "E-Mail : " + sdr_header["email"].ToString() + "  Web Site : " + sdr_header["website"].ToString();
    //        //FpEntry.Sheets[0].ColumnHeader.Cells[4, 1].Font.Bold = true;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[4, 1].HorizontalAlign = HorizontalAlign.Center;


    //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 5, 1);
    //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 9].CellType = mi2;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;


    //    }
    //    //++++++++++++++++++++++++++++++++++++++++++++++++++ End logoset ++++++++++++++++++++
    //}

    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value;
            }
        }

        return null;
    }



    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string batch = "", sections = "", semester = "", degreecode = "", subcolumntext = "", strsec = "";
        Boolean child_flag = false;
        int sec_index = 0, sem_index = 0;
        batch = ddlBatch.SelectedValue.ToString();
        sections = ddlSec.SelectedValue.ToString();
        semester = ddlSemYr.SelectedValue.ToString();
        degreecode = ddlBranch.SelectedValue.ToString();


        if (ddlSec.Text == "")
        {
            strsec = "";
        }
        else
        {
            if (ddlSec.SelectedItem.ToString() == "")
            {
                strsec = "";
            }
            else
            {
                strsec = " - " + ddlSec.SelectedItem.ToString();
            }
        }


        if (ddlSec.Enabled == false)
        {
            sec_index = -1;
        }
        else
        {
            sec_index = ddlSec.SelectedIndex;
        }

        if (ddlSemYr.Enabled == false)
        {
            sem_index = -1;
        }
        else
        {
            sem_index = ddlSemYr.SelectedIndex;
        }

        Session["page_redirect_value"] = ddlBatch.SelectedIndex + "," + ddlDegree.SelectedIndex + "," + ddlBranch.SelectedIndex + "," + sem_index + "," + sec_index + "," + txtFromDate.Text + "," + txtToDate.Text;

        // first_btngo();
        btnGo_Click(sender, e);

        #region Print_Option
        //if (tofromlbl.Visible == false)
        //{

        //    string clmnheadrname = "";
        //    //int total_clmn_count = FpEntry.Sheets[0].ColumnCount;//
        //    int total_clmn_count = gview.Columns.Count;

            
        //    for (int srtcnt = 0; srtcnt < total_clmn_count; srtcnt++)
        //    {
        //        //if (FpEntry.Sheets[0].Columns[srtcnt].Visible == true)//
        //        if (gview.Visible == true)
        //        {
        //            if (FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text != "")
        //            {
        //                subcolumntext = "";
        //                if (clmnheadrname == "")
        //                {
        //                    clmnheadrname = FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
        //                }
        //                else
        //                {
        //                    if (child_flag == false)
        //                    {
        //                        clmnheadrname = clmnheadrname + "," + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
        //                    }
        //                    else
        //                    {
        //                        clmnheadrname = clmnheadrname + "$)," + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
        //                    }

        //                }
        //                child_flag = false;
        //            }
        //            #region Command_OUT_J
        //            //else
        //            //{
        //            //    child_flag = true;
        //            //    if (subcolumntext == "")
        //            //    {
        //            //        for (int te = srtcnt - 1; te <= srtcnt; te++)
        //            //        {
        //            //            if (te == srtcnt - 1)
        //            //            {
        //            //                clmnheadrname = clmnheadrname + "* ($" + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
        //            //                subcolumntext = clmnheadrname + "* ($" + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
        //            //            }
        //            //            else
        //            //            {
        //            //                clmnheadrname = clmnheadrname + "$" + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
        //            //                subcolumntext = clmnheadrname + "$" + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, te].Text;

        //            //            }
        //            //        }
        //            //    }
        //            //    else
        //            //    {
        //            //        subcolumntext = subcolumntext + "$" + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
        //            //        clmnheadrname = clmnheadrname + "$" + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
        //            //    }
        //            //}
        //            #endregion
        //        }
        //    }
        //    Response.Redirect("Print_Master_Setting_new.aspx?ID=" + clmnheadrname.ToString() + ":" + "Attendance_overall.aspx" + ":" + ddlBatch.SelectedItem.ToString() + " Batch - " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlSemYr.SelectedItem.ToString())) + "  Semester ] " + strsec + " :" + "Overall Attendance Details -Splitup Report");
        //}
        //else
        //{

        //}
        #endregion
    }

    public string sem_roman(int sem)
    {
        string sql = "";
        string sem_roman = "";
        SqlDataReader rsChkSet;
        con_sem_roman.Close();
        con_sem_roman.Open();
        sql = "select * from inssettings where college_code=" + Session["collegecode"] + " and LinkName ='Semester Display'";
        SqlCommand cmd1 = new SqlCommand(sql, con_sem_roman);
        rsChkSet = cmd1.ExecuteReader();
        rsChkSet.Read();
        if (rsChkSet.HasRows == true)
        {
            if (rsChkSet["linkvalue"].ToString() == "1")
            {
                switch (sem)
                {
                    case 1:
                        sem_roman = "1";
                        break;
                    case 2:
                        sem_roman = "1-II";
                        break;
                    case 3:
                        sem_roman = "2-I";
                        break;
                    case 4:
                        sem_roman = "2-II";
                        break;
                    case 5:
                        sem_roman = "3-I";
                        break;
                    case 6:
                        sem_roman = "3-II";
                        break;
                    case 7:
                        sem_roman = "4-I";
                        break;
                    case 8:
                        sem_roman = "4-II";
                        break;
                    default:
                        sem_roman = " ";
                        break;
                }
            }
            else
            {
                switch (sem)
                {
                    case 1:
                        sem_roman = "I";
                        break;
                    case 2:
                        sem_roman = "II";
                        break;
                    case 3:
                        sem_roman = "III";
                        break;
                    case 4:
                        sem_roman = "IV";
                        break;
                    case 5:
                        sem_roman = "V";
                        break;
                    case 6:
                        sem_roman = "VI";
                        break;
                    case 7:
                        sem_roman = "VII";
                        break;
                    case 8:
                        sem_roman = "VIII";
                        break;
                    case 9:
                        sem_roman = "IX";
                        break;
                    case 10:
                        sem_roman = "X";
                        break;
                    default:
                        sem_roman = " ";
                        break;

                }
            }
        }
        return sem_roman;
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        
        gview.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        
        txtexcelname.Visible = false;
        lblrptname.Visible = false;

        frmlbl.Visible = false;
        tolbl.Visible = false;
        tofromlbl.Visible = false;
        lblnorec.Visible = false;
    }
    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        
        gview.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        
        txtexcelname.Visible = false;
        lblrptname.Visible = false;

        frmlbl.Visible = false;
        tolbl.Visible = false;
        tofromlbl.Visible = false;
        lblnorec.Visible = false;
    }


    //Hidden By Srinath 15/5/2013
    //public void setheader_print()
    //{
    //    // FpEntry.Sheets[0].RemoveSpanCell
    //    //================header
    //    temp_count = 0;

    //    double logo_length = Convert.ToInt64(GetFunction("select datalength(logo2) from collinfo"));
    //    double logo_length_left = Convert.ToInt64(GetFunction("select datalength(logo1) from collinfo"));

    //    MyImg mi = new MyImg();
    //    mi.ImageUrl = "~/images/10BIT001.jpeg";
    //    mi.ImageUrl = "Handler/Handler2.ashx?";
    //    MyImg mi2 = new MyImg();
    //    mi2.ImageUrl = "~/images/10BIT001.jpeg";
    //    mi2.ImageUrl = "Handler/Handler5.ashx?";

    //    if (final_print_col_cnt == 1)
    //    {
    //        for (col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (FpEntry.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                // one_column();
    //                more_column();
    //                break;
    //            }
    //        }

    //    }

    //    else if (final_print_col_cnt == 2)
    //    {
    //        for (col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (FpEntry.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 0)
    //                {
    //                    start_column = col_count;
    //                    //   FpEntry.Sheets[0].ColumnHeader.Columns[col_count].Width = 100;//
    //                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpEntry.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                    if (logo_length_left > 0 && logo_length_left.ToString() != "")
    //                    {
    //                        FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                    }
    //                    FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                }
    //                else
    //                {
    //                    //  one_column();
    //                    more_column();
    //                    for (int row_cnt = 0; row_cnt < FpEntry.Sheets[0].ColumnHeader.RowCount - 2; row_cnt++)
    //                    {
    //                        FpEntry.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                    }
    //                }
    //                temp_count++;
    //                if (temp_count == 2)
    //                {
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //    else if (final_print_col_cnt == 3)
    //    {
    //        for (col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (FpEntry.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 0)
    //                {
    //                    start_column = col_count;
    //                    //   FpEntry.Sheets[0].ColumnHeader.Columns[col_count].Width = 100;//
    //                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpEntry.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                    if (logo_length_left > 0 && logo_length_left.ToString() != "")
    //                    {
    //                        FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                    }
    //                    FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                }
    //                else if (temp_count == 1)
    //                {
    //                    // one_column();
    //                    more_column();
    //                    for (int row_cnt = 0; row_cnt < FpEntry.Sheets[0].ColumnHeader.RowCount - 2; row_cnt++)
    //                    {
    //                        FpEntry.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                        FpEntry.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                }
    //                else if (temp_count == 2)
    //                {
    //                    if (isonumber != string.Empty)
    //                    {
    //                        FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].Text = "ISO CODE:" + isonumber;
    //                        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, (FpEntry.Sheets[0].ColumnHeader.RowCount - 3), 1);
    //                        if (logo_length > 0 && logo_length.ToString() != "")
    //                        {
    //                            FpEntry.Sheets[0].ColumnHeader.Cells[1, col_count].CellType = mi2;
    //                        }
    //                        FpEntry.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                    else
    //                    {
    //                        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpEntry.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                        if (logo_length > 0 && logo_length.ToString() != "")
    //                        {
    //                            FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi2;
    //                        }
    //                        FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                    //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpEntry.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                    //if (logo_length > 0 && logo_length.ToString() != "")
    //                    //{
    //                    //    FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi2;
    //                    //}
    //                    //FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                }
    //                temp_count++;
    //                if (temp_count == 3)
    //                {
    //                    break;
    //                }
    //            }
    //        }

    //    }
    //    else//-----------column count more than 3
    //    {
    //        for (col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (FpEntry.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 0)
    //                {
    //                    start_column = col_count;
    //                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (6), 1);
    //                    if (logo_length_left > 0 && logo_length_left.ToString() != "")
    //                    {
    //                        FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                    }
    //                    FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                    FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
    //                }

    //                end_column = col_count;

    //                temp_count++;
    //                if (final_print_col_cnt == temp_count)
    //                {
    //                    break;
    //                }
    //            }
    //        }

    //        if (isonumber != string.Empty)
    //        {
    //            FpEntry.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Text = "ISO CODE:";// +isonumber;
    //            FpEntry.Sheets[0].ColumnHeader.Cells[0, end_column].Text = isonumber;
    //            FpEntry.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Border.BorderColorRight = Color.White;
    //            FpEntry.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorLeft = Color.White;
    //            FpEntry.Sheets[0].ColumnHeader.Cells[0, end_column].HorizontalAlign = HorizontalAlign.Left;
    //            FpEntry.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorBottom = Color.White;
    //            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, end_column, (5), 1);
    //            if (logo_length > 0 && logo_length.ToString() != "")
    //            {
    //                FpEntry.Sheets[0].ColumnHeader.Cells[1, end_column].CellType = mi2;
    //            }
    //            FpEntry.Sheets[0].ColumnHeader.Columns[end_column].Width = 150;
    //            FpEntry.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorTop = Color.White;
    //            FpEntry.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorBottom = Color.Black;
    //            FpEntry.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Border.BorderColorBottom = Color.White;
    //            FpEntry.Sheets[0].ColumnHeader.Cells[1, end_column - 1].Border.BorderColorTop = Color.White;
    //            FpEntry.Sheets[0].ColumnHeader.Cells[1, end_column - 1].Border.BorderColorBottom = Color.White;
    //            //if (dsprint.Tables[0].Rows.Count > 0)
    //            //{
    //            //    if (dsprint.Tables[0].Rows[0]["header_align_index"].ToString() != "")
    //            //    {
    //            //        FpEntry.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorBottom = Color.Black ;
    //            //    }
    //            //}
    //            FpEntry.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorLeft = Color.White;

    //            FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 1), end_column].Border.BorderColorTop = Color.Black;
    //        }
    //        else
    //        {
    //            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, end_column, (6), 1);

    //            if (logo_length > 0 && logo_length.ToString() != "")
    //            {
    //                FpEntry.Sheets[0].ColumnHeader.Cells[0, end_column].CellType = mi2;
    //            }
    //            FpEntry.Sheets[0].ColumnHeader.Columns[end_column].Width = 150;
    //            // FpEntry.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorRight = Color.Black;

    //            FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 2), end_column].Border.BorderColorTop = Color.Black;
    //        }
    //            //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, end_column, (6), 1);
    //            //if (logo_length > 0 && logo_length.ToString() != "")
    //            //{
    //            //    FpEntry.Sheets[0].ColumnHeader.Cells[0, end_column].CellType = mi2;
    //            //}
    //            //FpEntry.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorLeft = Color.White;
    //            //FpEntry.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorBottom = Color.White;


    //        temp_count = 0;
    //        for (col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (FpEntry.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 1)
    //                {
    //                    more_column();
    //                    for (int row_cnt = 0; row_cnt < FpEntry.Sheets[0].ColumnHeader.RowCount - 2; row_cnt++)
    //                    {
    //                        FpEntry.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                        FpEntry.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                }
    //                temp_count++;
    //            }
    //        }
    //    }
    //    //=========================



    //    //2.Footer setting

    //    if (dsprint.Tables[0].Rows.Count > 0)
    //    {
    //        if (dsprint.Tables[0].Rows[0]["footer"].ToString() != null && dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
    //        {
    //            footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());
    //            FpEntry.Sheets[0].RowCount = FpEntry.Sheets[0].RowCount + 3;

    //            FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 3), start_column].ColumnSpan = FpEntry.Sheets[0].ColumnCount - start_column;
    //            FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 2), start_column].ColumnSpan = FpEntry.Sheets[0].ColumnCount - start_column;

    //            FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 3), start_column].Border.BorderColorBottom = Color.White;
    //            FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 2), start_column].Border.BorderColorTop = Color.White;
    //            FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 2), start_column].Border.BorderColorBottom = Color.White;
    //            FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), start_column].Border.BorderColorTop = Color.White;


    //            footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
    //            string[] footer_text_split = footer_text.Split(',');
    //            footer_text = "";




    //            if (final_print_col_cnt < footer_count)
    //            {
    //                for (int concod_footer = 0; concod_footer < footer_count; concod_footer++)
    //                {
    //                    if (footer_text == "")
    //                    {
    //                        footer_text = footer_text_split[concod_footer].ToString();
    //                    }
    //                    else
    //                    {
    //                        footer_text = footer_text + "   " + footer_text_split[concod_footer].ToString();
    //                    }
    //                }

    //                for (col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount; col_count++)
    //                {
    //                    if (FpEntry.Sheets[0].Columns[col_count].Visible == true)
    //                    {
    //                        FpEntry.Sheets[0].SpanModel.Add((FpEntry.Sheets[0].RowCount - 1), col_count, 1, final_print_col_cnt);
    //                        FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Text = footer_text;
    //                        FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                        break;
    //                    }
    //                }

    //            }

    //            else if (final_print_col_cnt == footer_count)
    //            {
    //                temp_count = 0;
    //                for (col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount; col_count++)
    //                {
    //                    if (FpEntry.Sheets[0].Columns[col_count].Visible == true)
    //                    {
    //                        FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                        FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                        FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                        FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                        temp_count++;
    //                        if (temp_count == footer_count)
    //                        {
    //                            break;
    //                        }
    //                    }
    //                }

    //            }

    //            else
    //            {
    //                temp_count = 0;
    //                split_col_for_footer = final_print_col_cnt / footer_count;
    //                footer_balanc_col = final_print_col_cnt % footer_count;

    //                for (col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount; col_count++)
    //                {
    //                    if (FpEntry.Sheets[0].Columns[col_count].Visible == true)
    //                    {
    //                        if (temp_count == 0)
    //                        {
    //                            FpEntry.Sheets[0].SpanModel.Add((FpEntry.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer + footer_balanc_col);
    //                        }
    //                        else
    //                        {

    //                            FpEntry.Sheets[0].SpanModel.Add((FpEntry.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer);

    //                        }
    //                        FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                        FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                        if (col_count - 1 >= 0)
    //                        {
    //                            FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorLeft = Color.White;
    //                            FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorRight = Color.White;
    //                        }
    //                        FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                        FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                        if (col_count + 1 < FpEntry.Sheets[0].ColumnCount)
    //                        {
    //                            FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorLeft = Color.White;
    //                            FpEntry.Sheets[0].Cells[(FpEntry.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorRight = Color.White;
    //                        }


    //                        temp_count++;
    //                        if (temp_count == 0)
    //                        {
    //                            col_count = col_count + split_col_for_footer + footer_balanc_col;
    //                        }
    //                        else
    //                        {
    //                            col_count = col_count + split_col_for_footer;
    //                        }
    //                        if (temp_count == footer_count)
    //                        {
    //                            break;
    //                        }
    //                    }
    //                }
    //            }



    //        }
    //    }

    //    //2 end.Footer setting
    //}

    public void more_column()
    {
        header_text();

        
        //  FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, final_print_col_cnt - 2);
        if (final_print_col_cnt > 3)
        {
            if (isonumber != string.Empty)
            {
             
            }
            else
            {
             
            }
            
        }
        

        if (phoneno != "" && phoneno != null)
        {
            phone = "Phone:" + phoneno;
        }
        else
        {
            phone = "";
        }

        if (faxno != "" && faxno != null)
        {
            fax = "  Fax:" + faxno;
        }
        else
        {
            fax = "";
        }

        

        if (email != "" && faxno != null)
        {
            email_id = "Email:" + email;
        }
        else
        {
            email_id = "";
        }


        if (website != "" && website != null)
        {
            web_add = "  Web Site:" + website;
        }
        else
        {
            web_add = "";
        }

        //FpEntry.Sheets[0].ColumnHeader.Cells[3, col_count].Text = email_id + web_add;//
        //FpEntry.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorTop = Color.White;//
        //FpEntry.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorBottom = Color.White;//

        if (form_name != "" && form_name != null)
        {
            //FpEntry.Sheets[0].ColumnHeader.Cells[4, col_count].Text = form_name;//
            //FpEntry.Sheets[0].ColumnHeader.Cells[5, col_count].Text = "----------------------------------------------------";//modified on 04.07.12
        }
        if (final_print_col_cnt <= 3)
        {
            //FpEntry.Sheets[0].ColumnHeader.Cells[6, col_count].Text = "Degree & Branch: " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "                   Regulation:" + GetFunction(" select regulation from degree  where degree_code=" + ddlBranch.SelectedValue.ToString() + "");//"Name of the Program & Branch:" //
            //FpEntry.Sheets[0].ColumnHeader.Cells[7, col_count].Text = "Semester Number & Academic Year:" + ddlSemYr.SelectedValue.ToString() + " & " + Session["curr_year"].ToString() + "    Total number of working hours (a):" + max_cond_hr.ToString();//
            //FpEntry.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorTop = Color.White;//
            //FpEntry.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorBottom = Color.White;//
            //FpEntry.Sheets[0].ColumnHeader.Cells[7, col_count].Border.BorderColorTop = Color.White;//
        }
        else
        {
            // between_visible_col_cnt = (end_column - col_count)/2;
            between_visible_col_cnt = (final_print_col_cnt - 1) / 2;
            between_visible_col_cnt_bal = (final_print_col_cnt - 1) % 2;

            //for ( x = start_column ; x <FpEntry.Sheets[0].ColumnCount-1; x++)
            //{
            //    if(FpEntry.Sheets[0].Columns[x].Visible==true)
            //    {
            //        visi_col++;
            //        if (visi_col == start_column + between_visible_col_cnt + between_visible_col_cnt_bal)
            //        {
            //            visi_col = x;
            //            break;
            //        }                   
            //    }
            //}

            //   FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(6, start_column, 1, FpEntry.Sheets[0].ColumnCount-1);//added on 04.07.12

            //FpEntry.Sheets[0].ColumnHeader.Cells[6, start_column].Text = "Degree & Branch: " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString();// +"                  Regulation: " + GetFunction(" select regulation from degree  where degree_code=" + ddlBranch.SelectedValue.ToString() + ""); //"Name of the Program & Branch:"//
            //FpEntry.Sheets[0].ColumnHeader.Cells[6, start_column].HorizontalAlign = HorizontalAlign.Left;//
            //FpEntry.Sheets[0].ColumnHeader.Cells[6, start_column].Border.BorderColorBottom = Color.White;//
            //FpEntry.Sheets[0].ColumnHeader.Cells[6, start_column].Border.BorderColorRight = Color.White;//

            //for (x = start_column; x <= FpEntry.Sheets[0].ColumnCount - 1; x++)//
            for (x = start_column; x <= gview.Columns.Count; x++)
            {
//                if (FpEntry.Sheets[0].Columns[x].Visible == true)//
                if (gview.Columns[x].Visible == true)
                {
                    visi_col1++;
                    if (visi_col1 == between_visible_col_cnt + between_visible_col_cnt_bal)
                    {
                        break;
                    }
                }
            }

            //for (int xx = start_column + visi_col1 + 1; xx < FpEntry.Sheets[0].ColumnCount - 1; xx++)//
            for (int xx = start_column + visi_col1 + 1; xx < gview.Columns.Count; xx++)
            {
                //if (FpEntry.Sheets[0].Columns[xx].Visible == true)//
                if (gview.Visible == true)
                {
                    visi_col = xx;
                    break;
                }
            }

            //  FpEntry.Sheets[0].ColumnHeader.Cells[6, visi_col].Text = "Regulation:"; //hided on 04.07.12
            //FpEntry.Sheets[0].ColumnHeader.Cells[6, visi_col].Border.BorderColorLeft = Color.White;//
            //FpEntry.Sheets[0].ColumnHeader.Cells[6, visi_col].Border.BorderColorRight = Color.White;//
            //FpEntry.Sheets[0].ColumnHeader.Cells[6, visi_col + 1].Border.BorderColorBottom = Color.White;//
            //FpEntry.Sheets[0].ColumnHeader.Cells[6, visi_col + 1].HorizontalAlign = HorizontalAlign.Left;//

            //FpEntry.Sheets[0].ColumnHeader.Cells[6, end_column].HorizontalAlign = HorizontalAlign.Left;//
            //    FpEntry.Sheets[0].ColumnHeader.Cells[6, end_column].Text = GetFunction(" select regulation from degree  where degree_code=" + ddlBranch .SelectedValue.ToString() + "");////hided on 04.07.12

            int visi_col3 = 0, last_col = 0;
            for (int y = visi_col; y < end_column; y++)
            {
                //if (FpEntry.Sheets[0].Columns[y].Visible == true)//
                if (gview.Columns[y].Visible == true)
                {
                    visi_col3++;
                    last_col = y;
                }
            }

            //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(6, start_column, 1, visi_col1 + 2); //modified on 04.07.12 visi_col1//

            //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(6, visi_col + 1, 1, visi_col3);//
            //FpEntry.Sheets[0].ColumnHeader.Cells[6, end_column].Border.BorderColorBottom = Color.White;//

            //FpEntry.Sheets[0].ColumnHeader.Cells[7, start_column].Text = "Semester Number & Academic Year:" + ddlSemYr.SelectedValue.ToString() + " & " + Session["curr_year"].ToString();//
            //FpEntry.Sheets[0].ColumnHeader.Cells[7, start_column].HorizontalAlign = HorizontalAlign.Left;//
            //FpEntry.Sheets[0].ColumnHeader.Cells[7, start_column].Border.BorderColorRight = Color.White;//
            //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(7, start_column, 1, visi_col1 + 2);//

            //FpEntry.Sheets[0].ColumnHeader.Cells[6, visi_col + 1].Text = "Regulation: " + GetFunction(" select regulation from degree  where degree_code=" + ddlBranch.SelectedValue.ToString() + ""); //changed on 04.07.12//
            ////FpEntry.Sheets[0].ColumnHeader.Cells[6, end_column].Text = GetFunction(" select regulation from degree  where degree_code=" + ddlBranch.SelectedValue.ToString() + "");////hided on 04.07.12

            //FpEntry.Sheets[0].ColumnHeader.Cells[7, visi_col + 1].Text = "Total number of working hours (a):" + max_cond_hr.ToString(); //
            //FpEntry.Sheets[0].ColumnHeader.Cells[7, visi_col].Border.BorderColorTop = Color.White;//
            //FpEntry.Sheets[0].ColumnHeader.Cells[7, visi_col].Border.BorderColorLeft = Color.White;//
            //FpEntry.Sheets[0].ColumnHeader.Cells[7, visi_col].Border.BorderColorRight = Color.White;//
            //FpEntry.Sheets[0].ColumnHeader.Cells[7, visi_col + 1].HorizontalAlign = HorizontalAlign.Left;//

            //FpEntry.Sheets[0].ColumnHeader.Cells[7, end_column].HorizontalAlign = HorizontalAlign.Left;//
            ////   FpEntry.Sheets[0].ColumnHeader.Cells[7, end_column].Text =max_cond_hr .ToString();//hided on 04.07.12
            //FpEntry.Sheets[0].ColumnHeader.Cells[7, end_column].Border.BorderColorTop = Color.White;//
            //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(7, visi_col + 1, 1, visi_col3);//
        }

        //FpEntry.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorTop = Color.White;//
        //FpEntry.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorBottom = Color.White;//
        //FpEntry.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorTop = Color.White;//
        //FpEntry.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorBottom = Color.White;//




        int temp_count_temp = 0;
        string[] header_align_index;

        if (dsprint.Tables[0].Rows.Count > 0)
        {

            if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
            {
                header_align_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString().Split(',');

                //FpEntry.Sheets[0].ColumnHeader.Cells[7, start_column].Border.BorderColorBottom = Color.White;//
                //FpEntry.Sheets[0].ColumnHeader.Cells[7, end_column].Border.BorderColorBottom = Color.White;//
                //FpEntry.Sheets[0].ColumnHeader.Cells[7, visi_col].Border.BorderColorBottom = Color.White;//
                for (int row_head_count = 8; row_head_count < (8 + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
                {
                    //FpEntry.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Text = new_header_string_split[temp_count_temp].ToString();//
                    //if (final_print_col_cnt > 3)
                    {
                        //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(row_head_count, start_column, 1, (FpEntry.Sheets[0].ColumnCount - start_column + 1));//
                    }
                    //FpEntry.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorTop = Color.White;//
                    if (row_head_count != (8 + new_header_string_split.GetUpperBound(0)))
                    {
                        //FpEntry.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorBottom = Color.White;//
                    }

                    if (temp_count_temp <= header_align_index.GetUpperBound(0))
                    {
                        if (header_align_index[temp_count_temp].ToString() != string.Empty)
                        {
                            header_alignment = header_align_index[temp_count_temp].ToString();
                            if (header_alignment == "2")
                            {
                                //FpEntry.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Center;//
                            }
                            else if (header_alignment == "1")
                            {
                                //FpEntry.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Left;//
                            }
                            else
                            {
                                //FpEntry.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Right;//
                            }
                        }
                    }

                    temp_count_temp++;
                }
            }
        }
    }


    public void header_text()
    {

        Boolean check_print_row = false;

        SqlDataReader dr_collinfo;
        con.Close();
        con.Open();
        cmd = new SqlCommand("select isnull(college_name,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website,isnull(form_heading_name,'') as form_name,isnull(batch_degree_branch,'') as degree_deatil,isnull(header_align,'') as header_alignment,isnull(header_flag_value,'') as view_header,isnull(state,'')  as state,isnull(pincode,'') as pincode  from print_master_setting  where form_name='Attendance_overall.aspx'", con);
        dr_collinfo = cmd.ExecuteReader();
        while (dr_collinfo.Read())
        {
            if (dr_collinfo.HasRows == true)
            {
                check_print_row = true;
                coll_name = dr_collinfo["collname"].ToString();
                address1 = dr_collinfo["address1"].ToString();
                address2 = dr_collinfo["address2"].ToString();
                address3 = dr_collinfo["address3"].ToString();
                phoneno = dr_collinfo["phoneno"].ToString();
                faxno = dr_collinfo["faxno"].ToString();
                email = dr_collinfo["email"].ToString();
                website = dr_collinfo["website"].ToString();
                form_name = dr_collinfo["form_name"].ToString();
                degree_deatil = dr_collinfo["degree_deatil"].ToString();
                header_alignment = dr_collinfo["header_alignment"].ToString();
                view_header = dr_collinfo["view_header"].ToString();
                pincode = dr_collinfo["pincode"].ToString();
                state = dr_collinfo["state"].ToString();
            }

        }
        if (check_print_row == false)
        {

            con.Close();
            con.Open();
            cmd = new SqlCommand("select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website,isnull(state,'') as state,isnull(pincode,'') as pincode   from collinfo  where college_code=" + Session["collegecode"] + "", con);
            dr_collinfo = cmd.ExecuteReader();
            while (dr_collinfo.Read())
            {
                if (dr_collinfo.HasRows == true)
                {

                    string sec_val = "";

                    if (ddlSec.SelectedValue.ToString() != string.Empty && ddlSec.SelectedValue.ToString() != null)
                    {
                        sec_val = "Section: " + ddlSec.SelectedItem.ToString();
                    }
                    else
                    {
                        sec_val = "";
                    }


                    check_print_row = true;
                    coll_name = dr_collinfo["collname"].ToString();
                    address1 = dr_collinfo["address1"].ToString();
                    address2 = dr_collinfo["address2"].ToString();
                    address3 = dr_collinfo["address3"].ToString();
                    phoneno = dr_collinfo["phoneno"].ToString();
                    faxno = dr_collinfo["faxno"].ToString();
                    email = dr_collinfo["email"].ToString();
                    website = dr_collinfo["website"].ToString();
                    form_name = " Overall Attendance Details -Splitup Report";
                    pincode = dr_collinfo["pincode"].ToString();
                    state = dr_collinfo["state"].ToString();
                    degree_deatil = ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlSemYr.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
                    // header_alignment = ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlSemYr.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
                    // view_header = dr_collinfo["view_header"].ToString();
                }

            }
        }
    }


    public void print_btngo()
    {
        final_print_col_cnt = 0;
        errmsg.Visible = false;
        check_col_count_flag = false;


        //FpEntry.Sheets[0].SheetCorner.RowCount = 0;//
        //FpEntry.Sheets[0].ColumnCount = 0;//
        //FpEntry.Sheets[0].RowCount = 0;//
        //FpEntry.Sheets[0].SheetCorner.RowCount = 8;//
        //FpEntry.Sheets[0].ColumnCount = 5;//


        has.Clear();
        has.Add("college_code", Session["collegecode"].ToString());
        has.Add("form_name", "Attendance_overall.aspx");
        dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", has, "sp");
        if (dsprint.Tables[0].Rows.Count > 0)
        {


            //3. header add
            //if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
            //{
            //    new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
            //    new_header_string_split = new_header_string.Split(',');
            //    FpEntry.Sheets[0].SheetCorner.RowCount = FpEntry.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
            //}
            //3. end header add


            btnclick();



            //1.set visible columns
            column_field = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------------get column field
            if (column_field != "" && column_field != null)
            {
                //  check_col_count_flag = true;

                //for (col_count_all = 0; col_count_all < FpEntry.Sheets[0].ColumnCount; col_count_all++)//
                for (col_count_all = 0; col_count_all < gview.Columns.Count; col_count_all++)
                {
                    //FpEntry.Sheets[0].Columns[col_count_all].Visible = false;//------------invisible all column//
                    gview.Columns[col_count_all].Visible = false;//------------invisible all column
                }


                printvar = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------visible setting columns
                string[] split_printvar = printvar.Split(',');
                for (int splval = 0; splval <= split_printvar.GetUpperBound(0); splval++)
                {
                    span_cnt = 0;
                    string[] split_star = split_printvar[splval].Split('*');
                    if (split_star.GetUpperBound(0) > 0)
                    {
                        //for (col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount - 1; col_count++)//
                        {
                            //if (FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 1), col_count].Text == split_star[0])//
                            {
                                child_span_count = 0;

                                string[] split_star_doller = split_star[1].Split('$');
                                for (int doller_count = 1; doller_count < split_star_doller.GetUpperBound(0); doller_count++)
                                {
                                    for (int child_node = col_count; child_node <= col_count + split_star_doller.GetUpperBound(0); child_node++)
                                    {
                                        //if (FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 1), child_node].Text == split_star_doller[doller_count])//
                                        {
                                            span_cnt++;
                                            if (span_cnt == 1 && child_node == col_count + 1)
                                            {
                                                //FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 2), col_count + 1].Text = split_star[0].ToString();//
                                                col_count++;
                                            }

                                            if (child_node != col_count)
                                            {
                                                span_cnt = child_node - (child_span_count - 1);
                                            }
                                            else
                                            {
                                                child_span_count = col_count;
                                            }


                                            //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add((FpEntry.Sheets[0].ColumnHeader.RowCount - 2), col_count, 1, span_cnt);//


                                            //FpEntry.Sheets[0].Columns[child_node].Visible = true;//
                                            gview.Columns[child_node].Visible = true;

                                            final_print_col_cnt++;
                                            if (span_cnt == split_star_doller.GetUpperBound(0) - 1)
                                            {
                                                break;
                                            }

                                        }
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        for (col_count = 0; col_count < gview.Columns.Count; col_count++)
                        {
                            //if (FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 1), col_count].Text == split_printvar[splval])//
                            {
                                //FpEntry.Sheets[0].Columns[col_count].Visible = true;//
                                gview.Columns[col_count].Visible = true;
                                final_print_col_cnt++;
                                break;
                            }
                        }
                    }
                }
                //1 end.set visible columns
            }
            else
            {
                //FpEntry.Visible = false;//
                gview.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                Printcontrol.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;


                lblnorec.Visible = true;
                lblnorec.Text = "Select Atleast One Column Field From The Treeview";
            }
        }
        // FpEntry.Width = final_print_col_cnt * 100;
    }

    public void getspecial_hr()
    {
        //Added By Srinath 25/2/2013 ======Start
        string hrdetno = "";
        if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
        {
            hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(dumm_from_date), ht_sphr));

        }
        if (hrdetno != "")
        {
            //==========End
            con_splhr_query_master.Close();
            con_splhr_query_master.Open();
            DataSet ds_splhr_query_master = new DataSet();
            //  no_stud_flag = false;
            //Modified By Srinath 25/2/2013
            //string splhr_query_master = "select attendance from specialhr_attendance sa,registration r where hrdet_no in(select hrdet_no from specialhr_details where hrentry_no=(select hrentry_no from  specialhr_master where batch_year=" + ddlBatch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and date='" + dumm_from_date + "')  ) and r.roll_no=sa.roll_no and batch_year=" + ddlBatch.SelectedValue.ToString() + " and current_semester=" + ddlSemYr.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + "  and (CC = 0)  AND (DelFlag = 0)  AND (Exam_Flag <> 'debar') and sa.roll_no='" + roll.ToString() + "'  order by r.roll_no asc";
            string splhr_query_master = "select attendance from specialhr_attendance where roll_no='" + roll.ToString() + "'  and hrdet_no in(" + hrdetno + ")";
            SqlDataReader dr_splhr_query_master;
            cmd = new SqlCommand(splhr_query_master, con_splhr_query_master);
            dr_splhr_query_master = cmd.ExecuteReader();

            while (dr_splhr_query_master.Read())
            {
                if (dr_splhr_query_master.HasRows)
                {
                    value = dr_splhr_query_master[0].ToString();

                    if (value != null && value != "0" && value != "7" && value != "")
                    {
                        if (tempvalue != value)
                        {
                            tempvalue = value;
                            for (int j = 0; j < countds; j++)
                            {

                                if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                {
                                    ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                    j = countds;
                                }
                            }
                        }


                        if (ObtValue == 1)
                        {
                            per_abshrs_spl += 1;
                        }
                        else if (ObtValue == 2)
                        {
                            notconsider_value_spl += 1;
                            njhr_spl += 1;
                        }
                        else if (ObtValue == 0)
                        {
                            if (value != "3")
                            {
                                tot_per_hrs_spl += 1;
                            }
                        }

                        if (value == "3")
                        {
                            tot_ondu_spl += 1;
                        }
                        else if (value == "10")
                        {
                            per_leave += 1;
                        }
                        tot_conduct_hr_spl++;
                    }
                    else if (value == "7")
                    {
                        per_hhday_spl += 1;
                        tot_conduct_hr_spl--;
                    }
                    else
                    {
                        unmark_spl += 1;
                        tot_conduct_hr_spl--;
                    }

                    if (ObtValue == 2)
                    {
                        tot_conduct_hr_spl--;
                    }
                }
            }
        }//Added By Srinath 25/2/2013
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        //Modified by Srinath 27/2/2013
        string reportname = txtexcelname.Text;

        if (reportname.ToString().Trim() != "")
        {
            d2.printexcelreportgrid(gview, reportname);
            txtexcelname.Text = "";
        }
        else
        {
            errmsg.Text = "Please Enter Your Report Name";
            errmsg.Visible = true;
        }
        //string appPath = HttpContext.Current.Server.MapPath("~");
        //string print = "";
        //if (appPath != "")
        //{
        //    int i = 1;
        //    appPath = appPath.Replace("\\", "/");
        //e:
        //    try
        //    {
        //        print = "Overall Attendance Details -Splitup Report" + i;
        //        //FpEntry.SaveExcel(appPath + "/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
        //        //Aruna on 26feb2013============================
        //        string szPath = appPath + "/Report/";
        //        string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

        //        FpEntry.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
        //        Response.Clear();
        //        Response.ClearHeaders();
        //        Response.ClearContent();
        //        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
        //        Response.ContentType = "application/vnd.ms-excel";
        //        Response.Flush();
        //        Response.WriteFile(szPath + szFile);
        //        //=============================================

        //    }
        //    catch
        //    {
        //        i++;
        //        goto e;

        //    }
        //}
        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);

    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        //Session["column_header_row_count"] = Convert.ToString(FpEntry.ColumnHeader.RowCount);//
        Session["column_header_row_count"] = Convert.ToString(gview.Columns.Count);
        string sections = ddlSec.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
        {
            sections = "";
        }
        else
        {
            sections = "- Sec-" + sections;
        }
        // string periods = FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 1), 7].Tag.ToString();
        // periods = "@Conducted Periods : " + periods + "";
        //string degreedetails = "Overall Attendance Details -Splitup Report" + '@' + "Degree: " + ddlBatch.SelectedItem.ToString() + '-' + ddlDegree.SelectedItem.ToString() + '-' + ddlBranch.SelectedItem.ToString() + '-' + "Sem-" + ddlSemYr.SelectedItem.ToString() + sections + '@' + "Date :" + txtFromDate.Text.ToString() + " To " + txtToDate.Text.ToString()+" @Conducted Period (a):"+periods+"";
        string degreedetails = "OVERALL ATTENDANCE DETAILS - SPLITUP REPORT" + '@' + "Degree: " + ddlBatch.SelectedItem.ToString() + '-' + ddlDegree.SelectedItem.ToString() + '-' + ddlBranch.SelectedItem.ToString() + '-' + "Sem-" + ddlSemYr.SelectedItem.ToString() + sections + '@' + "Period :" + txtFromDate.Text.ToString() + " to " + txtToDate.Text.ToString() + "";//modified by srinath 6/1/2014
        string pagename = "Attendance_Overall.aspx";
        string ss = null;
        Printcontrol1.loadspreaddetails(gview, pagename, degreedetails, 0, ss);
        Printcontrol1.Visible = true;
    }
}




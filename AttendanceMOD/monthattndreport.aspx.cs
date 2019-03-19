#region Namespace
//==========MANIPRABHA A.
using System;//=====================================modified on 9/1/12,13/2/12, 14/2/12, 21/2/12(month_year no record), 22/2/12(holiday mark->H)
//-----24/2/12(holiday,logo,leav pts,.date check,XL), 15/3/12(dont tak HS for tot_count, half holiday count,header span setting)
//======(23/3/12)err lbl false,show no rec lbl,30/3/12(len(r_no)), 11/4/12(print setting from raja, condition), 15/5/12(if footer mt)
//===========================16/5/12(session->query string),11/6/12(include spl hr,p_m_s_n, try in p_l), 15/6/12(hide column)
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Text;

#endregion
public partial class NewAttendance : System.Web.UI.Page
{
    string sem_start = string.Empty;
    string sem_end = string.Empty;

    #region Field Declaration

    
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection cona = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con4a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection bind_con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection readcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection gradecon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_splhr_query_master = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection gradecon1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    static Boolean forschoolsetting = false;// Added by sridharan
    SqlCommand cmd;
    SqlCommand cmd1a;
    SqlCommand cmd3a;
    SqlCommand cmd4a;
    DataSet ds_attnd_pts = new DataSet();
    Boolean rowflag = false;
    //saravana strat
    int per_dif_dates;
    int difdate;
    int count_has = 1;
    int d_date;
    string head;
    int moncount;
    int mmyycount;
    string checknull;
    int tot_conducted_hours_count = 0;
    int tot_conducted_hours_count22 =0;
    int setfp;
    int ds3count = 0;
    double dif_date1 = 0;
    double dif_date = 0;
    double Ihof, IIhof;
    string pp;
    int rrowcount = 0;
    int rowcount = 0;
    int rcount = 0;
    int ccount;
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int cons_mng_start_hr = 0, cons_evng_start_hr = 0;
    Hashtable hath = new Hashtable();
    Hashtable hat = new Hashtable();
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    string value_holi_status = string.Empty;
    string[] split_holiday_status = new string[1000];
    int leav_pt = 0, absent_pt = 0, holi_leav = 0, holi_absent = 0;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    int ddiff;
    int abs = 0, att = 0;
    int dum_diff_date, unmark;
    string roll_no, reg_no, roll_ad, studname;
    int check;
    DateTime per_from_date;
    DateTime per_to_date;
    DateTime dumm_from_date;
    DateTime dumm_from_date1;
    string frdate, todate;
    TimeSpan ts;
    string diff_date;
    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;
    int count;
    int next = 0;
    int minpresII = 0;
    int minpresday = 0;
    string value;
    string date;
    int i, rows_count;
    string tempvalue = "-1";
    int ObtValue = -1;
    int cal_from_date;
    int cal_to_date;
    double per_perhrs, per_abshrs, cum_perhrs, cum_abshrs;
    double per_ondu, per_leave, per_hhday, cum_ondu, cum_leave, cum_hhday;
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
    int tot_per_hrs, per_tot_per_hrs, cum_per_tot_per, tot_wok_hrs, tot_abshrs, tot_per_abshrs, tot_leave, tot_per_leave;
    double per_con_hrs, cum_con_hrs, cum_con_hrs1;
    double njhr, njdate, per_njhr, per_njdate, cum_njhr, cum_njdate;
    double per_per_hrs, cum_per_perhrs;
    double tot_ondu, per_tot_ondu, cum_tot_ondu;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string qry = string.Empty;
    double holiday;
    int temp_unmark = 0;
    //raja
    static string[] string_session_values;
    int final_print_col_cnt = 0;
    Boolean check_col_count_flag = false;
    DataSet dsprint = new DataSet();
    DAccess2 dacces2 = new DAccess2();
    string column_field = string.Empty;
    int col_count_all = 0;
    string printvar = string.Empty;
    int span_cnt = 0;
    int col_count = 0;
    int child_span_count = 0;
    int footer_count = 0;
    string footer_text = string.Empty;
    int temp_count = 0;
    int split_col_for_footer = 0;
    int footer_balanc_col = 0;
    int tf = 0;
    FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
    DAccess2 da = new DAccess2();
    string view_footer = string.Empty;
    string view_header = string.Empty;
    string view_footer_text = string.Empty;
    //raja
    static string grouporusercode = string.Empty;
    //added by srinath
    static Hashtable ht_sphr = new Hashtable();
    DataSet ds_sphr = new DataSet();
    string tempdegreesem = string.Empty;
    string chkdegreesem = string.Empty;
    DateTime dtadm;
    string UnmarkHours = string.Empty;
    string CurrentDate = string.Empty;
    

    //added by rajasekar 20/09/2018
    DataTable dtl = new DataTable();
    DataRow dtrow = null;

    string[] headdate;
    //string[] subcode;
    int coln = 0;
    int rown = 0;
    string tag1 = "";
    string tag2 = "";
    string daywise = "";
    string hourwise = "";
    string cumdaywise = "";
    string cumhourwise = "";
    System.Text.StringBuilder Hrs = new System.Text.StringBuilder();

    ArrayList sph_datewise = new ArrayList();
    int tot_sphval=0;
    bool splhr_flag_head = false;
    //=================================//

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        norecordlbl.Visible = false;
        if (!Page.IsPostBack)
        {
            divNote.Visible = false;
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            cumfromtxt.Attributes.Add("readonly", "readonly");
            cumtotxt.Attributes.Add("readonly", "readonly");
            if (Session["group_code"] != null && (Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code='" + Session["group_code"].ToString().Trim().Split(';')[0] + "'";
            }
            else
            {
                grouporusercode = " usercode='" + Session["usercode"].ToString().Trim() + "'";
            }

            pagesetpanel.Visible = false;
            
            
            Session["attdaywisecla"] = "0";
            string daywisecal = d2.GetFunction("select value from Master_Settings where settings='Attendance Day Wise Calculation'");
            if (daywisecal.Trim() == "1")
            {
                Session["attdaywisecla"] = "1";
            }
            
            if (Request.QueryString["val"] == null)
            {
                bindbatch();
                binddegree();
                if (ddldegree.Items.Count > 0)
                {
                    ddldegree.Enabled = true;
                    ddlbranch.Enabled = true;
                    ddlduration.Enabled = true;
                    ddlsec.Enabled = true;
                    btnGo.Enabled = true;
                    txtFromDate.Enabled = true;
                    txtToDate.Enabled = true;
                    cumcheck.Enabled = true;
                    bindbranch();
                    bindsem();
                    bindsec();
                }
                else
                {
                    ddldegree.Enabled = false;
                    ddlbranch.Enabled = false;
                    ddlduration.Enabled = false;
                    ddlsec.Enabled = false;
                    btnGo.Enabled = false;
                    txtFromDate.Enabled = false;
                    txtToDate.Enabled = false;
                    cumcheck.Enabled = false;
                }
                
                Showgrid.Visible = false;
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                Printcontrol.Visible = false;
                pagesetpanel.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                Printcontrol.Visible = false;
                //Added by Srinath 27/2/2
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                Panel3.Visible = false;
                
                cumfromlbl.Visible = false;
                cumfromtxt.Visible = false;
                cumtolbl.Visible = false;
                cumtotxt.Visible = false;
                norecordlbl.Visible = false;
                ne.Visible = false;
                pointchk.Visible = false;
                pageddltxt.Visible = false;
                errmsg.Visible = false;
                tolbl.Visible = false;
                frmlbl.Visible = false;
                tofromlbl.Visible = false;
                //**
                lablepage.Visible = false;
                ddlpage_new.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = true;
                btnPrint.Visible = true;
            }
            else
            {
                string_session_values = Request.QueryString["val"].Split(',');
                if (string_session_values.GetUpperBound(0) == 7)
                {
                    try
                    {
                        bindbatch();
                        ddlbatch.SelectedIndex = Convert.ToInt16(string_session_values[0]);
                        binddegree();
                        ddldegree.SelectedIndex = Convert.ToInt16(string_session_values[1]);
                        bindbranch();
                        if (ddlbranch.Enabled == true)
                        {
                            ddlbranch.SelectedIndex = Convert.ToInt16(string_session_values[2].ToString());
                        }
                        bindsem();
                        if (ddlduration.Enabled == true)
                        {
                            ddlduration.SelectedIndex = Convert.ToInt16(string_session_values[3].ToString());
                        }
                        bindsec();
                        if (ddlsec.Enabled == true)
                        {
                            ddlsec.SelectedIndex = Convert.ToInt16(string_session_values[4].ToString());
                        }
                        txtFromDate.Text = string_session_values[5].ToString();
                        txtToDate.Text = string_session_values[6].ToString();
                        print_btngo();
                        view_header_setting();
                        lablepage.Visible = true;
                        ddlpage_new.Visible = true;
                        Showgrid.Visible = true;
                        btnprintmaster.Visible = true;
                        btnPrint.Visible = true;
                        Printcontrol.Visible = false;
                        cumfromlbl.Visible = false;
                        cumfromtxt.Visible = false;
                        cumtolbl.Visible = false;
                        cumtotxt.Visible = false;
                        pointchk.Visible = false;
                    }
                    catch
                    {
                    }
                }
                else
                {
                    try
                    {
                        bindbatch();
                        ddlbatch.SelectedIndex = Convert.ToInt16(string_session_values[0]);
                        binddegree();
                        ddldegree.SelectedIndex = Convert.ToInt16(string_session_values[1]);
                        bindbranch();
                        if (ddlbranch.Enabled == true)
                        {
                            ddlbranch.SelectedIndex = Convert.ToInt16(string_session_values[2].ToString());
                        }
                        bindsem();
                        if (ddlduration.Enabled == true)
                        {
                            ddlduration.SelectedIndex = Convert.ToInt16(string_session_values[3].ToString());
                        }
                        bindsec();
                        if (ddlsec.Enabled == true)
                        {
                            ddlsec.SelectedIndex = Convert.ToInt16(string_session_values[4].ToString());
                        }
                        txtFromDate.Text = string_session_values[5].ToString();
                        txtToDate.Text = string_session_values[6].ToString();
                        //raja
                        cumcheck.Checked = Convert.ToBoolean(string_session_values[7].ToString());
                        cumfromtxt.Text = string_session_values[8].ToString();
                        cumtotxt.Text = string_session_values[9].ToString();
                        pointchk.Checked = Convert.ToBoolean(string_session_values[10].ToString());
                        //raja
                        print_btngo();
                        view_header_setting();
                        lablepage.Visible = true;
                        ddlpage_new.Visible = true;
                        Showgrid.Visible = true;
                        btnprintmaster.Visible = true;
                        btnPrint.Visible = true;
                        Printcontrol.Visible = false;
                    }
                    catch
                    {
                    }
                }
            }
            //------------initial date picker value
            string dt = DateTime.Today.ToShortDateString();
            string[] dsplit = dt.Split(new Char[] { '/' });
            txtToDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            txtFromDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            cumfromtxt.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            cumtotxt.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            Session["curr_year"] = dsplit[2].ToString();
            //----------------------------------------------
            //-------------------------------Master settings
            string strdayflag;
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            Session["admissionno"] = "0";
            searchcheck.Visible = false;
            Session["daywise"] = "0";
            Session["hourwise"] = "0";
            if (Session["usercode"] != string.Empty)
            {
                string Master = string.Empty;
                Master = "select * from Master_Settings where " + grouporusercode + "";
                readcon.Close();
                readcon.Open();
                SqlDataReader mtrdr;
                SqlCommand mtcmd = new SqlCommand(Master, readcon);
                mtrdr = mtcmd.ExecuteReader();
                strdayflag = string.Empty;
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
                        if (mtrdr["settings"].ToString() == "Admission No" && mtrdr["value"].ToString() == "1")
                        {
                            Session["admissionno"] = "1";
                        }
                        if (mtrdr["settings"].ToString() == "Days Scholor" && mtrdr["value"].ToString() == "1")
                        {
                            if (strdayflag != "" && strdayflag != "\0")
                            {
                                strdayflag = " and (registration.Stud_Type='Day Scholar' or registration.Stud_Type='Hostler')";
                            }
                            else
                            {
                                strdayflag = " and (registration.Stud_Type='Day Scholar')";
                            }
                        }
                        if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                        {
                            if (strdayflag != "" && strdayflag != "\0")
                            {
                                strdayflag = " and (registration.Stud_Type='Day Scholar' or registration.Stud_Type='Hostler')";
                            }
                            else
                            {
                                strdayflag = " and (registration.Stud_Type='Hostler')";
                            }
                        }
                        if (mtrdr["settings"].ToString() == "Regular" && mtrdr["value"].ToString() == "1")
                        {
                            strdayflag = " and mode='1'";
                        }
                        if (mtrdr["settings"].ToString() == "Lateral" && mtrdr["value"].ToString() == "1")
                        {
                            strdayflag = " and mode='3'";
                        }
                        if (mtrdr["settings"].ToString() == "Transfer" && mtrdr["value"].ToString() == "1")
                        {
                            strdayflag = " and mode='2'";
                        }
                        if (mtrdr["settings"].ToString() == "Day Wise" && mtrdr["value"].ToString() == "1")
                        {
                            Session["Daywise"] = "1";
                            searchcheck.Items[0].Selected = true;
                        }
                        if (mtrdr["settings"].ToString() == "Hour Wise" && mtrdr["value"].ToString() == "1")
                        {
                            Session["Hourwise"] = "1";
                            searchcheck.Items[1].Selected = true;
                        }
                        Session["strvar"] = strdayflag;
                    }
                }
            }
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            Printcontrol.Visible = false;
            // Added By Sridharan 12 Mar 2015
            //{
            string grouporusercodeschool = string.Empty;
            if (Session["group_code"] != null && (Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercodeschool = " group_code=" + Session["group_code"].ToString().Trim().Split(';')[0] + "";
            }
            else
            {
                grouporusercodeschool = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercodeschool + "";
            schoolds.Clear();
            schoolds.Dispose();
            schoolds = d2.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                if (schoolvalue.Trim() == "0")
                {
                    forschoolsetting = true;
                    // lblcollege.Text = "School";
                    lblbatch.Text = "Year";
                    lbldegree.Text = "School Type";
                    lblbranch.Text = "Standard";
                    lblduration.Text = "Term";
                }
                else
                {
                    forschoolsetting = false;
                }
            }
            else
            {
                forschoolsetting = false;
            }
        }
        
    }

    public void bindbatch()
    {
        ////batch
        ddlbatch.Items.Clear();
        string sqlstr = string.Empty;
        int max_bat = 0;
        con.Close();
        con.Open();
        cmd = new SqlCommand(" select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year", con);
        SqlDataAdapter da1 = new SqlDataAdapter(cmd);
        DataSet ds1 = new DataSet();
        da1.Fill(ds1);
        ddlbatch.DataSource = ds1;
        ddlbatch.DataValueField = "batch_year";
        ddlbatch.DataBind();
        ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
    }

    public void binddegree()
    {
        ddldegree.Items.Clear();
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
        hat.Add("single_user", singleuser);
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds = d2.select_method("bind_degree", hat, "sp");
        int count1 = ds.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            ddldegree.DataSource = ds;
            ddldegree.DataTextField = "course_name";
            ddldegree.DataValueField = "course_id";
            ddldegree.DataBind();
        }
    }

    public void bindsem()
    {
        //--------------------semester load
        ddlduration.Items.Clear();
        Boolean first_year;
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
                    ddlduration.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlduration.Items.Add(i.ToString());
                }
            }
        }
        else
        {
            dr.Close();
            SqlDataReader dr1;
            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and college_code=" + Session["collegecode"] + "", con);
            ddlduration.Items.Clear();
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
                        ddlduration.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlduration.Items.Add(i.ToString());
                    }
                }
            }
            dr1.Close();
        }
        con.Close();
    }

    public void bindsec()
    {
        //----------load section
        ddlsec.Items.Clear();
        con.Close();
        con.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlsec.DataSource = ds;
        ddlsec.DataTextField = "sections";
        ddlsec.DataBind();
        ddlsec.Items.Insert(0, "All");
        SqlDataReader dr_sec;
        dr_sec = cmd.ExecuteReader();
        dr_sec.Read();
        if (dr_sec.HasRows == true)
        {
            if (dr_sec["sections"].ToString() == string.Empty)
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
        con.Close();
    }

    public void bindbranch()
    {
        ddlbranch.Items.Clear();
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
        hat.Add("single_user", singleuser);
        hat.Add("group_code", group_user);
        hat.Add("course_id", ddldegree.SelectedValue);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds = d2.select_method("bind_branch", hat, "sp");
        int count2 = ds.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataBind();
        }
    }
    public void binddate()
    {
        try
        {
            con.Close();
            con.Open();
            string from_date = string.Empty;
            string to_date = string.Empty;
            string final_from = string.Empty;
            string final_to = string.Empty;
            SqlDataReader dr_dateset;
            cmd = new SqlCommand("select start_date,end_date from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " ", con);
            dr_dateset = cmd.ExecuteReader();
            dr_dateset.Read();
            if (dr_dateset.HasRows == true)
            {
                //------------get from date
                from_date = dr_dateset[0].ToString();
                string[] from_split = from_date.Split(' ');
                string[] date_split_from = from_split[0].Split('/');
                final_from = date_split_from[1] + "/" + date_split_from[0] + "/" + date_split_from[2];
                //sem_start=date_split_from[0] + "/" + date_split_from[1] + "/" + date_split_from[2];
                sem_start = final_from;
                txtFromDate.Text = final_from;
                Session["fromdate"] = final_from;
                //------------get to date
                to_date = dr_dateset[1].ToString();
                string[] to_split = to_date.Split(' ');
                string[] date_split_to = to_split[0].Split('/');
                final_to = date_split_to[1] + "/" + date_split_to[0] + "/" + date_split_to[2];
                txtToDate.Text = final_to;
                Session["todate"] = final_to;
                sem_end = final_to;
            }
            else
            {
                string dt = DateTime.Today.ToShortDateString();
                string[] dsplit = dt.Split(new Char[] { '/' });
                txtToDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();

                txtFromDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            }


        }
        catch
        {
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        pagesetpanel.Visible = false;
        //FpSpread1.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        btnxl.Visible = false;
        //Added by Srinath 27/2/2
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Panel3.Visible = false;
        norecordlbl.Visible = false;
        ne.Visible = false;
        if (Page.IsPostBack == false)
        {
            ddlduration.Items.Clear();
        }
        frmlbl.Visible = false;
        tolbl.Visible = false;
        bindsem();
        bindsec();
        binddate();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        pagesetpanel.Visible = false;
        ne.Visible = false;
        //FpSpread1.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        btnxl.Visible = false;
        //Added by Srinath 27/2/2
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Panel3.Visible = false;
        norecordlbl.Visible = false;
        bindbranch();
        bindsem();
        bindsec();
        binddate();
    }

    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {
        pagesetpanel.Visible = false;
        //FpSpread1.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        btnxl.Visible = false;
        //Added by Srinath 27/2/2
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        ne.Visible = false;
        Panel3.Visible = false;
        norecordlbl.Visible = false;
        bindsec();
        binddate();
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        pagesetpanel.Visible = false;
        ne.Visible = false;
        //FpSpread1.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        btnxl.Visible = false;
        //Added by Srinath 27/2/2
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Panel3.Visible = false;
        norecordlbl.Visible = false;
        //bindbranch();
        //bindsem();
        //bindsec();
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        pagesetpanel.Visible = false;
        ne.Visible = false;
        //FpSpread1.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        btnxl.Visible = false;
        //Added by Srinath 27/2/2
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Panel3.Visible = false;
        norecordlbl.Visible = false;
        frmlbl.Visible = false;
        tolbl.Visible = false;
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        tofromlbl.Visible = false;
        pagesetpanel.Visible = false;
        ne.Visible = false;
        //FpSpread1.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        btnxl.Visible = false;
        //Added by Srinath 27/2/2
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Panel3.Visible = false;
        norecordlbl.Visible = false;
        frmlbl.Visible = false;
        tolbl.Visible = false;
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        tofromlbl.Visible = false;
        pagesetpanel.Visible = false;
        ne.Visible = false;
        //FpSpread1.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        btnxl.Visible = false;
        //Added by Srinath 27/2/2
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Panel3.Visible = false;
        norecordlbl.Visible = false;
        tolbl.Visible = false;
    }

    protected void cumcheck_CheckedChanged(object sender, EventArgs e)
    {
        if (cumcheck.Checked == true)
        {
            pointchk.Visible = true;
            cumfromlbl.Visible = true;
            cumfromtxt.Visible = true;
            cumtolbl.Visible = true;
            cumtotxt.Visible = true;
        }
        else
        {
            pointchk.Visible = false;
            cumfromlbl.Visible = false;
            cumfromtxt.Visible = false;
            cumtolbl.Visible = false;
            cumtotxt.Visible = false;
        }
    }

    protected void cumfromtxt_TextChanged(object sender, EventArgs e)
    {
        norecordlbl.Visible = false;
        pagesetpanel.Visible = false;
        ne.Visible = false;
    }

    protected void cumtotxt_TextChanged(object sender, EventArgs e)
    {
        norecordlbl.Visible = false;
        pagesetpanel.Visible = false;
        ne.Visible = false;
    }

    protected void RadioHeader_CheckedChanged(object sender, EventArgs e)
    {
    
    }

    protected void Radiowithoutheader_CheckedChanged(object sender, EventArgs e)
    {
    
    }

    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {
    //    norecordlbl.Visible = false;
    //    if (RadioHeader.Checked == true)
    //    {
    //        for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
    //        {
    //            FpSpread1.Sheets[0].Rows[i].Visible = false;
    //        }
    //        int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
    //        int end = start + 24;
    //        if (end >= FpSpread1.Sheets[0].RowCount)
    //        {
    //            end = FpSpread1.Sheets[0].RowCount;
    //        }
    //        int rowstart = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(start);
    //        int rowend = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(end);
    //        for (int i = start - 1; i < end; i++)
    //        {
    //            FpSpread1.Sheets[0].Rows[i].Visible = true;
    //        }
    //        FpSpread1.Sheets[0].ColumnHeader.Rows[0].Visible = true;
    //        FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = true;
    //        FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = true;
    //        FpSpread1.Sheets[0].ColumnHeader.Rows[3].Visible = true;
    //        FpSpread1.Sheets[0].ColumnHeader.Rows[4].Visible = true;
    //        FpSpread1.Sheets[0].ColumnHeader.Rows[5].Visible = true;
    //        FpSpread1.Sheets[0].ColumnHeader.Rows[6].Visible = true;
    //        FpSpread1.Sheets[0].ColumnHeader.Rows[7].Visible = true;
    //    }
    //    else if (Radiowithoutheader.Checked == true)
    //    {
    //        for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
    //        {
    //            FpSpread1.Sheets[0].Rows[i].Visible = false;
    //        }
    //        int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
    //        int end = start + 24;
    //        if (end >= FpSpread1.Sheets[0].RowCount)
    //        {
    //            end = FpSpread1.Sheets[0].RowCount;
    //        }
    //        int rowstart = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(start);
    //        int rowend = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(end);
    //        for (int i = start - 1; i < end; i++)
    //        {
    //            FpSpread1.Sheets[0].Rows[i].Visible = true;
    //        }
    //        if (Convert.ToInt32(ddlpage.SelectedValue.ToString()) == 1)
    //        {
    //            FpSpread1.Sheets[0].ColumnHeader.Rows[0].Visible = true;
    //            FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = true;
    //            FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = true;
    //            FpSpread1.Sheets[0].ColumnHeader.Rows[3].Visible = true;
    //            FpSpread1.Sheets[0].ColumnHeader.Rows[4].Visible = true;
    //            FpSpread1.Sheets[0].ColumnHeader.Rows[5].Visible = true;
    //            FpSpread1.Sheets[0].ColumnHeader.Rows[6].Visible = true;
    //            FpSpread1.Sheets[0].ColumnHeader.Rows[7].Visible = true;
    //        }
    //        else
    //        {
    //            FpSpread1.Sheets[0].ColumnHeader.Rows[0].Visible = false;
    //            FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = false;
    //            FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = false;
    //            FpSpread1.Sheets[0].ColumnHeader.Rows[3].Visible = false;
    //            FpSpread1.Sheets[0].ColumnHeader.Rows[4].Visible = false;
    //            FpSpread1.Sheets[0].ColumnHeader.Rows[5].Visible = false;
    //            FpSpread1.Sheets[0].ColumnHeader.Rows[6].Visible = false;
    //            FpSpread1.Sheets[0].ColumnHeader.Rows[7].Visible = false;
    //        }
    //    }
    //    if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
    //    {
    //        FpSpread1.Sheets[0].ColumnHeader.Rows[0].Visible = true;
    //        FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = true;
    //        FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = true;
    //        FpSpread1.Sheets[0].ColumnHeader.Rows[3].Visible = true;
    //        FpSpread1.Sheets[0].ColumnHeader.Rows[4].Visible = true;
    //        FpSpread1.Sheets[0].ColumnHeader.Rows[5].Visible = true;
    //        FpSpread1.Sheets[0].ColumnHeader.Rows[6].Visible = true;
    //        FpSpread1.Sheets[0].ColumnHeader.Rows[7].Visible = true;
    //        for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
    //        {
    //            FpSpread1.Sheets[0].Rows[i].Visible = true;
    //        }
    //        Double totalRows = 0;
    //        totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
    //        Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);
    //        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
    //        DropDownListpage.Items.Clear();
    //        if (totalRows >= 10)
    //        {
    //            FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //            for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
    //            {
    //                DropDownListpage.Items.Add((k + 10).ToString());
    //            }
    //            DropDownListpage.Items.Add("Others");
    //            DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //            FpSpread1.Height = 335;
    //        }
    //        else if (totalRows == 0)
    //        {
    //            DropDownListpage.Items.Add("0");
    //            FpSpread1.Height = 100;
    //        }
    //        else
    //        {
    //            FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //            DropDownListpage.Items.Add(FpSpread1.Sheets[0].PageSize.ToString());
    //            FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //        }
    //        if (Convert.ToInt32(FpSpread1.Sheets[0].RowCount) > 10)
    //        {
    //            DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //            FpSpread1.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
    //            //  FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //            CalculateTotalPages();
    //        }
    //        Panel3.Visible = false;
    //    }
    //    else
    //    {
    //        Panel3.Visible = false;
    //    }
    }

    protected void ddlpage_new_SelectedIndexChanged(object sender, EventArgs e)
    {
    //    hat.Clear();
    //    hat.Add("college_code", Session["collegecode"].ToString());
    //    hat.Add("form_name", "monthattndreport.aspx");
    //    dsprint = da.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
    //    if (dsprint.Tables[0].Rows.Count > 0)
    //    {
    //        view_footer = dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString();
    //        view_header = dsprint.Tables[0].Rows[0]["header_flag_value"].ToString();
    //        view_footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
    //        errmsg.Visible = false;
    //        if (view_header == "0")
    //        {
    //            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
    //            {
    //                FpSpread1.Sheets[0].Rows[i].Visible = false;
    //            }
    //            int start = Convert.ToInt32(ddlpage_new.SelectedValue.ToString());
    //            int end = start + 24;
    //            if (end >= FpSpread1.Sheets[0].RowCount)
    //            {
    //                end = FpSpread1.Sheets[0].RowCount;
    //            }
    //            int rowstart = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(start);
    //            int rowend = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(end);
    //            for (int i = start - 1; i < end; i++)
    //            {
    //                FpSpread1.Sheets[0].Rows[i].Visible = true;
    //            }
    //            for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
    //            {
    //                FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
    //            }
    //        }
    //        else if (view_header == "1")
    //        {
    //            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
    //            {
    //                FpSpread1.Sheets[0].Rows[i].Visible = false;
    //            }
    //            int start = Convert.ToInt32(ddlpage_new.SelectedValue.ToString());
    //            int end = start + 24;
    //            if (end >= FpSpread1.Sheets[0].RowCount)
    //            {
    //                end = FpSpread1.Sheets[0].RowCount;
    //            }
    //            int rowstart = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(start);
    //            int rowend = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(end);
    //            for (int i = start - 1; i < end; i++)
    //            {
    //                FpSpread1.Sheets[0].Rows[i].Visible = true;
    //            }
    //            if (Convert.ToInt32(ddlpage_new.SelectedValue.ToString()) == 1)
    //            {
    //                for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
    //                }
    //            }
    //            else
    //            {
    //                for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
    //            {
    //                FpSpread1.Sheets[0].Rows[i].Visible = false;
    //            }
    //            int start = Convert.ToInt32(ddlpage_new.SelectedValue.ToString());
    //            int end = start + 24;
    //            if (end >= FpSpread1.Sheets[0].RowCount)
    //            {
    //                end = FpSpread1.Sheets[0].RowCount;
    //            }
    //            int rowstart = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(start);
    //            int rowend = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(end);
    //            for (int i = start - 1; i < end; i++)
    //            {
    //                FpSpread1.Sheets[0].Rows[i].Visible = true;
    //            }
    //            {
    //                for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
    //                }
    //            }
    //        }
    //        if ((ddlpage_new.SelectedValue.ToString() == string.Empty) || (ddlpage_new.SelectedValue.ToString() == "0"))
    //        {
    //            if (view_header == "1" || view_header == "0")
    //            {
    //                for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
    //                }
    //            }
    //            else
    //            {
    //                for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
    //                }
    //            }
    //            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
    //            {
    //                FpSpread1.Sheets[0].Rows[i].Visible = true;
    //            }
    //            Double totalRows = 0;
    //            totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
    //            Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);
    //            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
    //            DropDownListpage.Items.Clear();
    //            if (totalRows >= 10)
    //            {
    //                FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
    //                {
    //                    DropDownListpage.Items.Add((k + 10).ToString());
    //                }
    //                DropDownListpage.Items.Add("Others");
    //                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //                FpSpread1.Height = 335;
    //            }
    //            else if (totalRows == 0)
    //            {
    //                DropDownListpage.Items.Add("0");
    //                FpSpread1.Height = 100;
    //            }
    //            else
    //            {
    //                FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //                DropDownListpage.Items.Add(FpSpread1.Sheets[0].PageSize.ToString());
    //                FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //            }
    //            if (Convert.ToInt32(FpSpread1.Sheets[0].RowCount) > 10)
    //            {
    //                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //                FpSpread1.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
    //                //  subject_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //                CalculateTotalPages();
    //            }
    //            //setpanel.Visible = true;
    //        }
    //        else
    //        {
    //            //setpanel.Visible = false;
    //        }
    //        if (view_footer_text != "")
    //        {
    //            if (view_footer == "0")
    //            {
    //                FpSpread1.Sheets[0].Rows[(FpSpread1.Sheets[0].RowCount - 1)].Visible = true;
    //                FpSpread1.Sheets[0].Rows[(FpSpread1.Sheets[0].RowCount - 2)].Visible = true;
    //                FpSpread1.Sheets[0].Rows[(FpSpread1.Sheets[0].RowCount - 3)].Visible = true;
    //            }
    //            else
    //            {
    //                if (ddlpage_new.Text != "")
    //                {
    //                    if (ddlpage_new.SelectedIndex != ddlpage_new.Items.Count - 1)
    //                    {
    //                        FpSpread1.Sheets[0].Rows[(FpSpread1.Sheets[0].RowCount - 1)].Visible = false;
    //                        FpSpread1.Sheets[0].Rows[(FpSpread1.Sheets[0].RowCount - 2)].Visible = false;
    //                        FpSpread1.Sheets[0].Rows[(FpSpread1.Sheets[0].RowCount - 3)].Visible = false;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        errmsg.Visible = false;
    //        errmsg.Text = "No Header and Footer setting Assigned";
    //    }
    }

    //----------------------------GO button
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            main_fun();
            if (dtl.Columns.Count > 0)
            {
                //for (int col = 0; col < FpSpread1.Sheets[0].ColumnCount - 1; col++)
                //{
                //    FpSpread1.Sheets[0].Columns[col].Visible = true;
                //}
                //'===============================settings====================================
                if (Session["Rollflag"].ToString() == "0")
                {
                    
                }
                else
                {
                    
                }
                if (Session["Regflag"].ToString() == "0")
                {
                   
                }
                else
                {
                    
                }
                if (Session["admissionno"].ToString() == "0")
                {
                    
                }
                else
                {
                    
                }
                //================================================
            }
           

            //last modified by prabha on jan 23 2018
            if (!string.IsNullOrEmpty(UnmarkHours))
            {
                string qryUserCodeOrGroupCode = string.Empty;
                string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = Convert.ToString(group_semi[0]);
                }
                if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
                {
                    qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
                }
                else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
                {
                    qryUserCodeOrGroupCode = " and usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                }
                string alertRights = dirAcc.selectScalarString("select value from Master_Settings where settings='AlertMessageForAttendance' " + qryUserCodeOrGroupCode + "");
                string Noresult = UnmarkHours;
                if (alertRights == "1")
                {
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = Noresult + " " + " Attendance not Found";
                    divPopAlert.Visible = true;
                    Printcontrol.Visible = false;
                    btnprintmaster.Visible = false;
                    btnPrint.Visible = false;
                    btnxl.Visible = false;
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                }
                //return;
            }

        }
        catch
        {
        }
    }

    public void main_fun()
    {
        try
        {
            
            // FpSpread1.Sheets[0].ColumnHeader.RowCount = 6;
            norecordlbl.Visible = false;
            tofromlbl.Visible = false;
            Boolean cum_flag = false;
            string date1 = "", date2 = string.Empty;
            date1 = txtFromDate.Text.ToString();
            string[] split = date1.Split(new Char[] { '/' });
            if (split.GetUpperBound(0) == 2)//-------date valid
            {
                if (Convert.ToInt16(split[0].ToString()) <= 31 && Convert.ToInt16(split[1].ToString()) <= 12 && Convert.ToInt16(split[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                {
                    date2 = txtToDate.Text.ToString();
                    string[] split1 = date2.Split(new Char[] { '/' });
                    if (split1.GetUpperBound(0) == 2)//--date valid
                    {
                        if (Convert.ToInt16(split1[0].ToString()) <= 31 && Convert.ToInt16(split1[1].ToString()) <= 12 && Convert.ToInt16(split1[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                        {
                            int days = 0;
                            DateTime dt1 = Convert.ToDateTime(split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString());
                            DateTime dt2 = Convert.ToDateTime(split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString());
                            TimeSpan t = dt2.Subtract(dt1);
                            days = t.Days;
                            if (days >= 0)
                            {
                                if (cumcheck.Checked == true)
                                {
                                    string cum_from = cumfromtxt.Text;
                                    string cum_to = cumtotxt.Text;
                                    string[] cum_from_split = cum_from.Split('/');
                                    string[] cum_to_split = cum_to.Split('/');
                                    DateTime dt1_cum = Convert.ToDateTime(cum_from_split[1].ToString() + "/" + cum_from_split[0].ToString() + "/" + cum_from_split[2].ToString());
                                    DateTime dt2_cum = Convert.ToDateTime(cum_to_split[1].ToString() + "/" + cum_to_split[0].ToString() + "/" + cum_to_split[2].ToString());
                                    t = dt2_cum.Subtract(dt1_cum);
                                    days = t.Days;
                                    if (days >= 0)
                                    {
                                        cum_flag = true;
                                    }
                                    else
                                    {
                                        cum_flag = false;
                                    }
                                }
                                else
                                {
                                    cum_flag = true;
                                }
                                if (cum_flag == true)
                                {
                                    con.Close();
                                    con.Open();
                                    string attnd_points = "select * from leave_points";
                                    SqlDataAdapter da_attnd_pts;
                                    da_attnd_pts = new SqlDataAdapter(attnd_points, con);
                                    da_attnd_pts.Fill(ds_attnd_pts);
                                    if (ds_attnd_pts.Tables[0].Rows.Count > 0)
                                    {
                                        holi_leav = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["leave_bef_aft"].ToString());
                                        holi_absent = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["absent_bef_aft"].ToString());
                                        leav_pt = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["leave"].ToString());
                                        absent_pt = Convert.ToInt16(ds_attnd_pts.Tables[0].Rows[0]["absent"].ToString());
                                    }
                                    if (ddlsec.Enabled == true && ddlsec.Text != "-1" && txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
                                    {
                                        gobutton();
                                    }
                                    if (ddlsec.Enabled == false && txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
                                    {
                                        gobutton();
                                    }
                                }
                                else
                                {
                                    norecordlbl.Visible = true;
                                    norecordlbl.Text = "Cumulative From Date Should Be Greater Then To Date";
                                }
                            }
                            else
                            {
                                tofromlbl.Visible = true;
                            }
                        }
                        else
                        {
                            tolbl.Visible = true;
                            tolbl.Text = "Select Valid To Date";
                        }
                    }
                    else
                    {
                        tolbl.Visible = true;
                        tolbl.Text = "Select Valid To Date";
                    }
                }
                else
                {
                    frmlbl.Visible = true;
                    frmlbl.Text = "Select Valid From Date";
                }
            }
            else
            {
                frmlbl.Visible = true;
                frmlbl.Text = "Select Valid From Date";
            }
            
        }
        catch
        {
        }
    }

    //public void setheader()
    //{
    //    try
    //    {
    //        string coll_name = "", address1 = "", address2 = "", address3 = "", phoneno = "", faxno = "", email = "", website =string.Empty;
    //        MyImg mi = new MyImg();
    //        mi.ImageUrl = "~/images/10BIT001.jpeg";
    //        mi.ImageUrl = "Handler/Handler2.ashx?";
    //        MyImg mi2 = new MyImg();
    //        mi2.ImageUrl = "~/images/10BIT001.jpeg";
    //        mi2.ImageUrl = "Handler/Handler5.ashx?";
    //        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
    //        {
    //            SqlDataReader dr_collinfo;
    //            con.Close();
    //            con.Open();
    //            cmd = new SqlCommand("select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website from collinfo  where college_code=" + Session["collegecode"] + "", con);
    //            dr_collinfo = cmd.ExecuteReader();
    //            while (dr_collinfo.Read())
    //            {
    //                if (dr_collinfo.HasRows == true)
    //                {
    //                    coll_name = dr_collinfo["collname"].ToString();
    //                    address1 = dr_collinfo["address1"].ToString();
    //                    address2 = dr_collinfo["address2"].ToString();
    //                    address3 = dr_collinfo["address3"].ToString();
    //                    phoneno = dr_collinfo["phoneno"].ToString();
    //                    faxno = dr_collinfo["faxno"].ToString();
    //                    email = dr_collinfo["email"].ToString();
    //                    website = dr_collinfo["website"].ToString();
    //                }
    //            }
    //            {
    //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, 1);
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;
    //                FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 250;
    //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 2, FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, 2);
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].CellType = mi2;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[2, 4].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[3, 4].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[4, 4].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[5, 4].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[6, 4].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[7, 4].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColorLeft = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Border.BorderColorLeft = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[2, 4].Border.BorderColorLeft = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[3, 4].Border.BorderColorLeft = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[4, 4].Border.BorderColorLeft = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[5, 4].Border.BorderColorLeft = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[6, 4].Border.BorderColorLeft = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[7, 4].Border.BorderColorLeft = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorLeft = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorLeft = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorLeft = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, (FpSpread1.Sheets[0].ColumnCount - 2)].Border.BorderColorLeft = Color.White;
    //                int dd = FpSpread1.Sheets[0].ColumnCount - 6;
    //                int span_col = 0;
    //                if (dd >= 354)
    //                {
    //                    int span_col_count = 0, span_balanc = 0;
    //                    span_col_count = dd / 354;
    //                    span_balanc = dd % 354;
    //                    for (span_col = 1; span_col <= span_col_count; span_col++)
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, ((span_col - 1) * 354) + 4].ColumnSpan = 354;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, ((span_col - 1) * 354) + 4].ColumnSpan = 354;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, ((span_col - 1) * 354) + 4].ColumnSpan = 354;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, 4].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, 4].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[3, ((span_col - 1) * 354) + 4].ColumnSpan = 354;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[3, 4].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[3, 4].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[4, ((span_col - 1) * 354) + 4].ColumnSpan = 354;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[4, 4].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[4, 4].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[5, ((span_col - 1) * 354) + 4].ColumnSpan = 354;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[5, 4].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[5, 4].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[6, ((span_col - 1) * 354) + 4].ColumnSpan = 354;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[6, 4].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[6, 4].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[7, ((span_col - 1) * 354) + 4].ColumnSpan = 354;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[7, 4].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[7, 4].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, ((span_col - 1) * 354) + 4].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, ((span_col - 1) * 354) + 4].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, ((span_col - 1) * 354) + 4].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[3, ((span_col - 1) * 354) + 4].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[4, ((span_col - 1) * 354) + 4].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[5, ((span_col - 1) * 354) + 4].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[6, ((span_col - 1) * 354) + 4].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[7, ((span_col - 1) * 354) + 4].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, ((span_col - 1) * 354) + 4].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, ((span_col - 1) * 354) + 4].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, ((span_col - 1) * 354) + 4].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[3, ((span_col - 1) * 354) + 4].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[4, ((span_col - 1) * 354) + 4].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[5, ((span_col - 1) * 354) + 4].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[6, ((span_col - 1) * 354) + 4].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[7, ((span_col - 1) * 354) + 4].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, ((span_col - 1) * 354) + 4].Border.BorderColorBottom = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, ((span_col - 1) * 354) + 4].Border.BorderColorTop = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, ((span_col - 1) * 354) + 4].Border.BorderColorBottom = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, ((span_col - 1) * 354) + 4].Border.BorderColorTop = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, ((span_col - 1) * 354) + 4].Border.BorderColorBottom = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[3, ((span_col - 1) * 354) + 4].Border.BorderColorTop = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[3, ((span_col - 1) * 354) + 4].Border.BorderColorBottom = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[4, ((span_col - 1) * 354) + 4].Border.BorderColorTop = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[4, ((span_col - 1) * 354) + 4].Border.BorderColorBottom = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[5, ((span_col - 1) * 354) + 4].Border.BorderColorTop = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[5, ((span_col - 1) * 354) + 4].Border.BorderColorBottom = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[6, ((span_col - 1) * 354) + 4].Border.BorderColorTop = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[6, ((span_col - 1) * 354) + 4].Border.BorderColorBottom = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[7, ((span_col - 1) * 354) + 4].Border.BorderColorTop = Color.White;
    //                    }
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, ((span_col - 1) * 354) + 4].ColumnSpan = span_balanc;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, ((span_col - 1) * 354) + 4].Border.BorderColorRight = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, ((span_col - 1) * 354) + 4].Border.BorderColorLeft = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, ((span_col - 1) * 354) + 4].ColumnSpan = span_balanc;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, ((span_col - 1) * 354) + 4].Border.BorderColorRight = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, ((span_col - 1) * 354) + 4].Border.BorderColorLeft = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, ((span_col - 1) * 354) + 4].ColumnSpan = span_balanc;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, ((span_col - 1) * 354) + 4].Border.BorderColorRight = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, ((span_col - 1) * 354) + 4].Border.BorderColorLeft = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[3, ((span_col - 1) * 354) + 4].ColumnSpan = span_balanc;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[3, ((span_col - 1) * 354) + 4].Border.BorderColorRight = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[3, ((span_col - 1) * 354) + 4].Border.BorderColorLeft = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[4, ((span_col - 1) * 354) + 4].ColumnSpan = span_balanc;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[4, ((span_col - 1) * 354) + 4].Border.BorderColorRight = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[4, ((span_col - 1) * 354) + 4].Border.BorderColorLeft = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[5, ((span_col - 1) * 354) + 4].ColumnSpan = span_balanc;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[5, ((span_col - 1) * 354) + 4].Border.BorderColorRight = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[5, ((span_col - 1) * 354) + 4].Border.BorderColorLeft = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[6, ((span_col - 1) * 354) + 4].ColumnSpan = span_balanc;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[6, ((span_col - 1) * 354) + 4].Border.BorderColorRight = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[6, ((span_col - 1) * 354) + 4].Border.BorderColorLeft = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[7, ((span_col - 1) * 354) + 4].ColumnSpan = span_balanc;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[7, ((span_col - 1) * 354) + 4].Border.BorderColorRight = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[7, ((span_col - 1) * 354) + 4].Border.BorderColorLeft = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, ((span_col - 1) * 354) + 4].Border.BorderColorBottom = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, ((span_col - 1) * 354) + 4].Border.BorderColorTop = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, ((span_col - 1) * 354) + 4].Border.BorderColorBottom = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, ((span_col - 1) * 354) + 4].Border.BorderColorTop = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, ((span_col - 1) * 354) + 4].Border.BorderColorBottom = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[3, ((span_col - 1) * 354) + 4].Border.BorderColorTop = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[3, ((span_col - 1) * 354) + 4].Border.BorderColorBottom = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[4, ((span_col - 1) * 354) + 4].Border.BorderColorTop = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[4, ((span_col - 1) * 354) + 4].Border.BorderColorBottom = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[5, ((span_col - 1) * 354) + 4].Border.BorderColorTop = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[5, ((span_col - 1) * 354) + 4].Border.BorderColorBottom = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[6, ((span_col - 1) * 354) + 4].Border.BorderColorTop = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[6, ((span_col - 1) * 354) + 4].Border.BorderColorBottom = Color.White;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[7, ((span_col - 1) * 354) + 4].Border.BorderColorTop = Color.White;
    //                }
    //                else
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ColumnSpan = dd;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].ColumnSpan = dd;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, 4].ColumnSpan = dd;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[3, 4].ColumnSpan = dd;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[4, 4].ColumnSpan = dd;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[5, 4].ColumnSpan = dd;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[6, 4].ColumnSpan = dd;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[7, 4].ColumnSpan = dd;
    //                }
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = coll_name;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Text = address1 + "-" + address2 + "-" + address3;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[2, 4].Text = "Phone:" + phoneno + "  Fax:" + faxno;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[3, 4].Text = "Email:" + email + "  Web Site:" + website;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[4, 4].Text = "Monthly Student Attendance Report";
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[5, 4].Text = "----------------------------------------------------";
    //                //Ila
    //                //Ila
    //                string header_alignment =string.Empty;
    //                hat.Clear();
    //                hat.Add("college_code", Session["collegecode"].ToString());
    //                hat.Add("form_name", "monthattndreport.aspx");
    //                dsprint = d2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
    //                if (dsprint.Tables[0].Rows.Count > 0)
    //                {
    //                    string new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
    //                    string new_header_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString();
    //                    string[] new_header_string_split = new_header_string.Split(',');
    //                    for (int y = 0; y <= new_header_string_split.GetUpperBound(0); y++)
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[8 + y, 4].ColumnSpan = dd;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[(8 + y), 4].Text = new_header_string_split[y].ToString();
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[(8 + y), 4].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[(8 + y), 4].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[(8 + y), 1].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[(8 + y), 1].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[(8 + y), 2].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[(8 + y), 2].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[(8 + y), 3].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[(8 + y), 3].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[7, 4].Border.BorderColorBottom = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorBottom = Color.White;
    //                        //***
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[6, 1].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[7, 1].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorBottom = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[6, 1].Border.BorderColorBottom = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[7, 1].Border.BorderColorBottom = Color.White;
    //                        //***
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[8, 2].Border.BorderColorTop = Color.White;
    //                        if (y < new_header_string_split.GetUpperBound(0))
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[(8 + y), 2].Border.BorderColorBottom = Color.White;
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[(8 + y), 1].Border.BorderColorBottom = Color.White;
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[(8 + y), 4].Border.BorderColorBottom = Color.White;
    //                        }
    //                        //=========================11/6/12 PRABHA
    //                        //   string new_header_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString(); 
    //                        if (new_header_string_split[y].ToString() != string.Empty)
    //                        {
    //                            header_alignment = new_header_string_split[y].ToString();
    //                            if (header_alignment == "2")
    //                            {
    //                                FpSpread1.Sheets[0].ColumnHeader.Cells[(8 + y), 0].HorizontalAlign = HorizontalAlign.Center;
    //                            }
    //                            else if (header_alignment == "1")
    //                            {
    //                                FpSpread1.Sheets[0].ColumnHeader.Cells[(8 + y), 0].HorizontalAlign = HorizontalAlign.Left;
    //                            }
    //                            else
    //                            {
    //                                FpSpread1.Sheets[0].ColumnHeader.Cells[(8 + y), 0].HorizontalAlign = HorizontalAlign.Right;
    //                            }
    //                        }
    //                        //======================
    //                    }
    //                }
    //                //Ila
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[6, 1].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[7, 1].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorBottom = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[6, 1].Border.BorderColorBottom = Color.White;
    //                //FpSpread1.Sheets[0].ColumnHeader.Cells[7, 1].Border.BorderColorBottom = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColorBottom = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Border.BorderColorTop = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Border.BorderColorBottom = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[2, 4].Border.BorderColorTop = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[2, 4].Border.BorderColorBottom = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[3, 4].Border.BorderColorTop = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[3, 4].Border.BorderColorBottom = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[4, 4].Border.BorderColorTop = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[4, 4].Border.BorderColorBottom = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[5, 4].Border.BorderColorTop = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[5, 4].Border.BorderColorBottom = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[6, 4].Border.BorderColorTop = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[6, 4].Border.BorderColorBottom = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[7, 4].Border.BorderColorTop = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorLeft = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorLeft = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorRight = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorLeft = Color.White;
    //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 8, 1);
    //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 8, 1);
    //                string sec_val =string.Empty;
    //                if (ddlsec.SelectedValue.ToString() != string.Empty && ddlsec.SelectedValue.ToString() != null)
    //                {
    //                    sec_val = "Section: " + ddlsec.SelectedItem.ToString();
    //                }
    //                else
    //                {
    //                    sec_val =string.Empty;
    //                }
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[6, 4].Text = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
    //                string dt = DateTime.Today.ToShortDateString();
    //                string[] dsplit = dt.Split(new Char[] { '/' });
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[7, 4].Text = "From: " + txtFromDate.Text + "      To: " + txtToDate.Text + "       Date: " + dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
    //            }
    //        }
    //        int overall_colcount = 0;
    //        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
    //        overall_colcount = FpSpread1.Sheets[0].ColumnCount;
    //        FpSpread1.Sheets[0].Columns[0].Width = 100;
    //        FpSpread1.Width = overall_colcount * 20;
    //    }
    //    catch
    //    {
    //    }
    //}

    public void gobutton()
    {
        try
        {
            hat.Clear();
            ds.Clear();
            hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
            hat.Add("sem_ester", int.Parse(ddlduration.SelectedValue.ToString()));
            ds = d2.select_method("period_attnd_schedule", hat, "sp");
            if (ds.Tables[0].Rows.Count != 0)
            {
                NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                minpresday = int.Parse(ds.Tables[0].Rows[0]["MIN PREE PER DAY"].ToString());
            }
            
            if (cumcheck.Checked == true)
            {
                spsizeforcum();
            }
            else
            {
                spsize();
            }
            string sec;
            string splhrsec = string.Empty;
            if (ddlsec.Enabled == true)
            {
                if (ddlsec.SelectedItem.ToString() == string.Empty || ddlsec.Text == "All")
                {
                    sec = string.Empty;
                    splhrsec = string.Empty;
                }
                else
                {
                    sec = " and sections='" + ddlsec.SelectedItem.ToString() + "'";
                    splhrsec = " and sm.sections='" + ddlsec.SelectedItem.ToString() + "'";
                }
            }
            else
            {
                sec = string.Empty;
            }
            //added By Srinath 11/8/2013
            string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
            string strorder = "ORDER BY registration.roll_no";
            if (orderby_Setting == "0")
            {
                strorder = "ORDER BY registration.roll_no";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "ORDER BY registration.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strorder = "ORDER BY registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "ORDER BY registration.roll_no,registration.Reg_No,registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "ORDER BY registration.roll_no,registration.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "ORDER BY registration.Reg_No,registration.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "ORDER BY registration.roll_no,registration.Stud_Name";
            }
            #region Attendance
            bool incRedo = false;
            string stvfa = d2.GetFunctionv("select value from Master_Settings where settings = 'Include Redo student in Attendance'");
            if (stvfa.Trim() == "1")
            {
                incRedo = true;
            }
            #endregion
            //hat.Clear();
            //hat.Add("bath", int.Parse(ddlbatch.SelectedItem.ToString()));
            //hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
            //hat.Add("sec", sec.ToString());
            //ds4 = d2.select_method("ALL_STUDENT_DETAILS", hat, "sp");
            //============================
            ds4.Clear();
            con.Close();
            con.Open();
            string includediscon = " and delflag=0";
            string getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Discount' and" + grouporusercode + "");
            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
            {
                includediscon = string.Empty;
            }
            string includedebar = " and exam_flag <> 'DEBAR'";

            getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Debar' and" + grouporusercode + "");
            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
            {
                includedebar = string.Empty;
            }
            //    cmd = new SqlCommand(" select distinct roll_no as 'ROLL_NO',Reg_No as 'REG_NO',Stud_Name as 'STUD_NAME',Roll_Admit as 'ADMIT_NO' from registration where cc=0 and exam_flag<>'debar' and delflag=0 and batch_year="+ddlbatch.SelectedItem.ToString()+" and degree_code="+ddlbranch.SelectedValue.ToString()+"  "+sec+"  "+Session["strvar"].ToString() +"", con);
            //cmd = new SqlCommand("  select distinct roll_no as 'ROLL_NO',Reg_No as 'REG_NO',registration.Stud_Name as 'STUD_NAME',Roll_Admit as 'ADMIT_NO',len(roll_no),adm_date from registration,applyn where cc=0 and exam_flag<>'debar' and delflag=0 and registration.batch_year=" + ddlbatch.SelectedItem.ToString() + " and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + "  " + sec + " and registration.current_semester=" + ddlduration.SelectedValue.ToString() + " and registration.app_no=applyn.app_no  " + sec + "  " + Session["strvar"].ToString() + " order by len(roll_no)", con);//Hidden By SRinath 11/8/201
            //cmd = new SqlCommand("  select distinct roll_no as 'ROLL_NO',Reg_No as 'REG_NO',registration.Stud_Name as 'STUD_NAME',Roll_Admit as 'ADMIT_NO',len(roll_no),adm_date,cc,delflag,exam_flag from registration,applyn where cc=0 " + includedebar + " " + includediscon + " and registration.batch_year=" + ddlbatch.SelectedItem.ToString() + " and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + "  " + sec + " and registration.app_no=applyn.app_no  " + sec + "  " + Session["strvar"].ToString() + " " + strorder + " ", con);
            //cmd = new SqlCommand(" select Registration.roll_no,Registration.reg_no, Registration.stud_name as 'STUD_NAME',Registration.stud_type,registration.serialno,Registration.Adm_Date,delflag,exam_flag from registration, applyn a where a.app_no=registration.app_no and Registration.Roll_No not in(select s.roll_no from stucon s where s.roll_no=Registration.roll_no and s.semester=Registration.Current_Semester and s.ack_fee_of_roll=1) and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + "  and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0   ", con);
            qry = "select distinct roll_no as 'ROLL_NO',Reg_No as 'REG_NO',registration.Stud_Name as 'STUD_NAME',Roll_Admit as 'ADMIT_NO',len(roll_no),adm_date,cc,delflag,exam_flag from registration,applyn where cc=0 " + includedebar + " " + includediscon + " and registration.batch_year=" + ddlbatch.SelectedItem.ToString() + " and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + "  " + sec + " and registration.app_no=applyn.app_no  " + sec + "  " + Session["strvar"].ToString() + " " + strorder + " ";
            ds4.Clear();
            ds4 = da.select_method_wo_parameter(qry, "text");

            string Qryredo = "select distinct roll_no as 'ROLL_NO',Reg_No as 'REG_NO',registration.Stud_Name as 'STUD_NAME',Roll_Admit as 'ADMIT_NO',len(roll_no),adm_date,cc,delflag,exam_flag from registration,applyn where cc=0  and registration.batch_year=" + ddlbatch.SelectedItem.ToString() + " and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + "  " + sec + " and registration.app_no=applyn.app_no  " + sec + " and ISNULL(isRedo,0)=1  " + Session["strvar"].ToString() + " " + strorder + " ";
            DataSet dsredo = d2.select_method_wo_parameter(Qryredo, "text");

            if (incRedo)
            {
                if (dsredo.Tables[0].Rows.Count > 0)
                    ds4.Tables[0].Merge(dsredo.Tables[0]);
            }
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(ds4);
            ds1.Clear();
            hat.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
            count = ds1.Tables[0].Rows.Count;
            int stu_count = ds4.Tables[0].Rows.Count;
            if (stu_count > 0)
            {
                // Added By Srinath 18/2/2013=====================
                frdate = txtFromDate.Text;
                todate = txtToDate.Text;
                string[] fromdatespilt = frdate.Split('/');
                DateTime frdatetime = Convert.ToDateTime(fromdatespilt[1] + '/' + fromdatespilt[0] + '/' + fromdatespilt[2]);
                string[] todatespilt = todate.Split('/');
                DateTime todatetimes = Convert.ToDateTime(todatespilt[1] + '/' + todatespilt[0] + '/' + todatespilt[2]);
                ht_sphr.Clear();
                string hrdetno = string.Empty;
                string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and date between '" + frdatetime.ToString() + "' and '" + todatetimes.ToString() + "' " + splhrsec + "";
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
                //end===================================
                norecordlbl.Visible = false;
                for (rows_count = 0; rows_count < stu_count; rows_count++)
                {
                    tot_per_hrs_spl = 0;
                    tot_ondu_spl = 0;
                    per_leave = 0;
                    tot_conduct_hr_spl = 0;
                    per_hhday_spl = 0;
                    unmark_spl = 0;
                    print();
                    string strdelval = tag1;
                    string strexamflag = tag2;

                    
                    
                    if (incRedo)//Rajkumar add for include redo
                    {
                        check = 1;
                        presentdays();
                        print1();
                        if (cumcheck.Checked == true)
                        {
                            tot_per_hrs_spl = 0;
                            tot_ondu_spl = 0;
                            per_leave = 0;
                            tot_conduct_hr_spl = 0;
                            per_hhday_spl = 0;
                            unmark_spl = 0;
                            //print();
                            check = 2;
                            cumpresentdays();
                            print2();
                        }
                    }
                    else if(!incRedo)
                    {
                        check = 1;
                        presentdays();
                        print1();
                        if (cumcheck.Checked == true)
                        {
                            tot_per_hrs_spl = 0;
                            tot_ondu_spl = 0;
                            per_leave = 0;
                            tot_conduct_hr_spl = 0;
                            per_hhday_spl = 0;
                            unmark_spl = 0;
                            //print();
                            check = 2;
                            cumpresentdays();
                            print2();
                        }
                    }
                    else
                    {
                        
                        if (strdelval.Trim().ToUpper() == "1" || strdelval.Trim().ToUpper() == "TRUE" || strexamflag.Trim().ToUpper() == "1" || strexamflag.Trim().ToUpper() == "TRUE")
                        {
                            

                            dtrow[coln] = "LEFT";
                            
                        }
                    
                    }
                    //if (strdelval.Trim().ToUpper() != "1" || strdelval.Trim().ToUpper() != "TRUE")
                    //{
                    //    check = 1;
                    //    presentdays();
                    //    print1();
                    //    if (cumcheck.Checked == true)
                    //    {
                    //        tot_per_hrs_spl = 0;
                    //        tot_ondu_spl = 0;
                    //        per_leave = 0;
                    //        tot_conduct_hr_spl = 0;
                    //        per_hhday_spl = 0;
                    //        unmark_spl = 0;
                    //        //print();
                    //        check = 2;
                    //        cumpresentdays();
                    //        print2();
                    //    }
                    //}
                    
                    dtl.Rows.Add(dtrow);
                }
            }
            else
            {
                divNote.Visible = false;
                norecordlbl.Visible = true;
                norecordlbl.Text = "No Student(s) Available";
                Showgrid.Visible = false;
                return;
            }
            // print3();
            if (Convert.ToInt32(dtl.Rows.Count) == 0)
            {
                divNote.Visible = false;
                Buttontotal.Visible = false;
                DropDownListpage.Visible = false;
                Showgrid.Visible = false;
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                Printcontrol.Visible = false;
                btnxl.Visible = false;
                //Added by Srinath 27/2/2
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
            }
            else
            {

                if (dtl.Rows.Count > 0)
                {
                    Showgrid.DataSource = dtl;
                    Showgrid.DataBind();
                    Showgrid.Visible = true;
                    Showgrid.HeaderRow.Visible = false;
                    int colofin = 0;
                    int lastdatecol = 0;
                    int tempt = Convert.ToInt32(ViewState["temp_table"]);

                    int hrss = Convert.ToInt32(ViewState["Hrs"]);

                    int lastcol = Convert.ToInt32(ViewState["lastcolspan"]);
                    int ccc = tempt;

                    if (headdate != null)
                    {
                        lastdatecol = ((headdate.Length * hrss) + tempt) + tot_sphval;

                    }
                    for (int i = 0; i < Showgrid.Rows.Count; i++)
                    {

                        for (int j = 0; j < Showgrid.HeaderRow.Cells.Count; j++)
                        {
                            
                            if (i == 0 || i == 1)
                            {
                                Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                Showgrid.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                Showgrid.Rows[i].Cells[j].Font.Bold = true;

                                if (i == 0)
                                {
                                    if (j < tempt || j >= Showgrid.HeaderRow.Cells.Count - lastcol)
                                    {
                                        Showgrid.Rows[i].Cells[j].RowSpan = 2;
                                        for (int a = i; a < 1; a++)
                                            Showgrid.Rows[a + 1].Cells[j].Visible = false;
                                    }
                                    else if (ccc == j && j < lastdatecol)
                                    {
                                        if (sph_datewise.Count == 0)
                                        {
                                            Showgrid.Rows[i].Cells[j].ColumnSpan = hrss;
                                            for (int a = j + 1; a < j + hrss; a++)
                                                Showgrid.Rows[i].Cells[a].Visible = false;

                                            ccc += hrss;
                                        }
                                        else
                                        {
                                            int vall=Convert.ToInt32(sph_datewise[colofin].ToString());
                                            Showgrid.Rows[i].Cells[j].ColumnSpan = hrss + vall;
                                            for (int a = j + 1; a < j + hrss + vall; a++)
                                                Showgrid.Rows[i].Cells[a].Visible = false;

                                            ccc += hrss + vall;
                                            colofin++;
                                        }
                                    }
                                    else if (ccc == j && j >= lastdatecol)
                                    {
                                        Showgrid.Rows[i].Cells[j].ColumnSpan = 3;
                                        for (int a = j + 1; a < j + 3; a++)
                                            Showgrid.Rows[i].Cells[a].Visible = false;

                                        ccc += 3;
                                    }

                                }
                            }
                            else
                            {
                                if (Showgrid.HeaderRow.Cells[j].Text == "Admission No" || Showgrid.HeaderRow.Cells[j].Text == "Roll No" || Showgrid.HeaderRow.Cells[j].Text == "Register No" || Showgrid.HeaderRow.Cells[j].Text == "Name")
                                {
                                    Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;


                                    string rrr = Showgrid.Rows[i].Cells[j].Text;
                                    string[] splitval = rrr.Split('^');

                                    if (splitval.Length > 1)
                                    {
                                        if (splitval.Length > 1)
                                        {
                                            Showgrid.Rows[i].Cells[j].Text = splitval[0].ToString();

                                            Showgrid.Rows[i].Cells[j].ForeColor = Color.Red;
                                            Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;

                                        }
                                    }

                                }

                                else
                                {
                                    
                                    

                                    Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;

                                    if (Showgrid.Rows[i].Cells[j].Text == "LEFT")
                                    {
                                        Showgrid.Rows[i].ForeColor = Color.Red;
                                        Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                    }
                                    else if (Showgrid.Rows[i].Cells[j].Text == "P")
                                    {
                                        Showgrid.Rows[i].Cells[j].ForeColor = ColorTranslator.FromHtml("#035523");
                                        Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                    }
                                    else if (Showgrid.Rows[i].Cells[j].Text == "A")
                                    {
                                        Showgrid.Rows[i].Cells[j].ForeColor = ColorTranslator.FromHtml("#F21C03");
                                        Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                    }
                                    else if (Showgrid.Rows[i].Cells[j].Text == "OD")
                                    {
                                        Showgrid.Rows[i].Cells[j].ForeColor = ColorTranslator.FromHtml("#3000D3");
                                        Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                    }
                                    else if (Showgrid.Rows[i].Cells[j].Text == "NE")
                                    {
                                        Showgrid.Rows[i].Cells[j].ForeColor = ColorTranslator.FromHtml("#000000");
                                        Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                    }
                                    else if (Showgrid.Rows[i].Cells[j].Text == "NJ")
                                    {
                                        Showgrid.Rows[i].Cells[j].ForeColor = ColorTranslator.FromHtml("#9057C3");
                                        Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                    }
                                    else if (Showgrid.Rows[i].Cells[j].Text == "H")
                                    {
                                        Showgrid.Rows[i].Cells[j].ForeColor = ColorTranslator.FromHtml("#C41D9E");
                                        Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                    }
                                    else
                                    {
                                        Showgrid.Rows[i].Cells[j].ForeColor = ColorTranslator.FromHtml("#000000");
                                        Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                    }



                                }

                            }
                        }

                    }
                }

                Buttontotal.Visible = true;
                pagesetpanel.Visible = false;
                DropDownListpage.Visible = true;
                Showgrid.Visible = true;
                divNote.Visible = true;
                btnprintmaster.Visible = true;
                btnPrint.Visible = true;
                Printcontrol.Visible = false;
                btnxl.Visible = true;
                //Added by Srinath 27/2/2
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                pagesetpanel.Visible = false;
                Panel3.Visible = false;
                Double totalRows = 0;
                totalRows = Convert.ToInt32(dtl.Rows.Count);
                DropDownListpage.Items.Clear();
                if (totalRows >= 10)
                {
                    
                    for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                    {
                        DropDownListpage.Items.Add((k + 10).ToString());
                    }
                    DropDownListpage.Items.Add("Others");
                    DropDownListpage.SelectedIndex = DropDownListpage.Items.Count - 2;
                    
                }
                else if (totalRows == 0)
                {
                    DropDownListpage.Items.Add("0");
                    
                }
                else
                {
                    
                    //DropDownListpage.Items.Add(FpSpread1.Sheets[0].PageSize.ToString());
                    
                }
                totalRows = Convert.ToInt32(dtl.Rows.Count);
                Session["totalPages"] = (int)Math.Ceiling(totalRows / dtl.Rows.Count);
                Buttontotal.Text = "Records : " + totalRows + "          Pages : " + 1;
            }
            hat.Clear();
            hat.Add("college_code", Session["collegecode"].ToString());
            hat.Add("form_name", "monthattndreport.aspx");
            dsprint = d2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                if (dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
                {
                    string new_header_string = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
                    string[] new_header_string_split = new_header_string.Split(',');
                    if (new_header_string_split[0] != "")
                    {
                        
                    }
                    int az = 0;
                    int fotcnt = Convert.ToInt32(dsprint.Tables[0].Rows[0]["footer"].ToString());
                    int colcountt = dtl.Columns.Count;
                    if (colcountt >= fotcnt)
                    {
                        az = colcountt / fotcnt;
                    }
                    int n = 0;
                    
                    for (int y = 0; y < dtl.Columns.Count - 1; y = y + az)
                    {
                        if (n <= new_header_string_split.GetUpperBound(0))
                        {
                            
                        }
                        
                        n++;
                    }
                }
            }
            //view_header_setting();
            

            //Rajkumar 12/22/2017
            if (!string.IsNullOrEmpty(UnmarkHours))
            {
                string qryUserCodeOrGroupCode = string.Empty;
                string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = Convert.ToString(group_semi[0]);
                }
                if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
                {
                    qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
                }
                else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
                {
                    qryUserCodeOrGroupCode = " and usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                }
                string alertRights = dirAcc.selectScalarString("select value from Master_Settings where settings='AlertMessageForAttendance'  " + qryUserCodeOrGroupCode + "");
                string Noresult = UnmarkHours;
                if (alertRights == "1")
                {
                    
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = Noresult + " " + " Attendance not Found";
                    divPopAlert.Visible = true;
                }
                //return;  //modified on prabha on 15/12/2017
            }


        }

        catch
        {
        }
    }

    private void print2()
    {
        double ConductedHours = 0;
        double AttendedHours = 0;
        string dum_tage_date, dum_tage_hrs;
        string dum_cum_tage_date, dum_cum_tage_hrs;
        per_tage_date = ((pre_present_date / per_workingdays) * 100);
        if (per_tage_date > 100)
        {
            per_tage_date = 100;
        }
        per_con_hrs = ((per_workingdays * NoHrs) - per_dum_unmark + tot_conduct_hr_spl);
        per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl) / per_con_hrs) * 100);
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
        cum_tage_date = ((cum_present_date / cum_workingdays) * 100);
        if (cum_tage_date > 100)
        {
            cum_tage_date = 100;
        }
       // cum_con_hrs = ((cum_workingdays * NoHrs) - cum_dum_unmark);
        cum_con_hrs = tot_conducted_hours_count; //added by Mullai
        cum_tage_hrs = ((cum_per_perhrs / cum_con_hrs) * 100);
        if (cum_tage_hrs > 100)
        {
            cum_tage_hrs = 100;
        }
        dum_cum_tage_date = String.Format("{0:0,0.00}", float.Parse(cum_tage_date.ToString()));
        dum_cum_tage_hrs = String.Format("{0:0,0.00}", float.Parse(cum_tage_hrs.ToString()));
        if (dum_cum_tage_date == "NaN")
        {
            dum_cum_tage_date = "0";
        }
        else if (dum_cum_tage_date == "Infinity")
        {
            dum_cum_tage_date = "0";
        }
        if (dum_cum_tage_hrs == "NaN")
        {
            dum_cum_tage_hrs = "0";
        }
        else if (dum_cum_tage_hrs == "Infinity")
        {
            dum_cum_tage_hrs = "0";
        }
        //per_con_hrs = ((per_workingdays * NoHrs) - per_dum_unmark);

        per_con_hrs = tot_conducted_hours_count22;
        setfp += 1;
        
        setfp += 1;
        if (daywise != "")
        {
            dtrow[coln] = (per_workingdays).ToString();
            coln++;
        }
        
        setfp += 1;
        if (daywise != "")
        {
            dtrow[coln] = (pre_present_date).ToString();
            coln++;
        }
        //added by sudhagar
        double roundPer = 0;
        if (cbincround.Checked)
            double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(dum_tage_date))), out roundPer);
        else
            double.TryParse(Convert.ToString(dum_tage_date), out roundPer);
        

        if (daywise != "")
        {
            dtrow[coln] = (roundPer).ToString();
            coln++;
        }
        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = (dum_tage_date).ToString();
        setfp += 1;
        
        setfp += 1;

        if (hourwise != "")
        {
            ConductedHours = per_con_hrs + tot_conduct_hr_spl;
            
            dtrow[coln] = (per_con_hrs + tot_conduct_hr_spl).ToString();
            coln++;
        }
        
        setfp += 1;

        if (hourwise != "")
        {
            AttendedHours = per_per_hrs + tot_per_hrs_spl;
            dtrow[coln] = (per_per_hrs + tot_per_hrs_spl).ToString();
            coln++;
        }
        double roundPers = 0;
        if (cbincround.Checked)
            double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(dum_tage_hrs))), out roundPers);
        else
            double.TryParse(Convert.ToString(dum_tage_hrs), out roundPers);
        

        if (hourwise != "")
        {
            //dtrow[coln] = roundPers.ToString();
            string per = "0";
            if (AttendedHours != 0 && ConductedHours != 0)
            {
                if (cbincround.Checked)
                    per = Convert.ToString(Math.Round(Convert.ToDecimal((AttendedHours / ConductedHours) * 100)));
                else
                    per = Convert.ToString(Math.Round(Convert.ToDecimal((AttendedHours / ConductedHours) * 100), 2));
            }
            dtrow[coln] = per;
           
            coln++;
        }
        //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = dum_tage_hrs.ToString();
        //  setfp += 1;
        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = per_tot_ondu.ToString();
        setfp += 1;
        
        if (cumdaywise != "")
        {
            dtrow[coln] = cum_workingdays.ToString();
            coln++;
        }
        setfp += 1;
       
        setfp += 1;
        if (cumdaywise != "")
        {
            dtrow[coln] = cum_present_date.ToString();
            coln++;
        }
        
        setfp += 1;
        if (cumdaywise != "")
        {
            dtrow[coln] = (dum_cum_tage_date).ToString();
            coln++;
        }
        
        setfp += 1;
        if (cumhourwise != "")
        {
            dtrow[coln] = (cum_con_hrs + tot_conduct_hr_spl).ToString();
            coln++;
        }
        
        setfp += 1;
        if (cumhourwise != "")
        {
            dtrow[coln] = (cum_per_perhrs + tot_per_hrs_spl).ToString();
            coln++;
        }
        
        if (cumhourwise != "")
        {
            dtrow[coln] = dum_cum_tage_hrs.ToString();
            coln++;
        }
        //  setfp += 1;
        //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = cum_tot_ondu.ToString();
        setfp += 1;
        
        if (daywise != "")
        {
            //dtrow[coln] = (per_absent_date + per_abshrs_spl).ToString();
            dtrow[coln] = (per_absent_date).ToString();
            coln++;
        }
        setfp += 1;
        
        setfp += 1;
        if (daywise != "")
        {
            dtrow[coln] = (pre_leave_date + per_leave).ToString();
            coln++;
        }
        
        setfp += 1;
        if (daywise != "")
        {

            //dtrow[coln] = (pre_ondu_date + tot_ondu_spl).ToString();
            dtrow[coln] = (pre_ondu_date ).ToString();
            coln++;
        }
        
        setfp += 1;
        if (hourwise != "")
        {
            
            dtrow[coln] = (ConductedHours - AttendedHours).ToString();
                
            //dtrow[coln] = tot_per_abshrs.ToString();
            coln++;
        }
        
        setfp += 1;
        if (hourwise != "")
        {
            dtrow[coln] = tot_per_leave.ToString();
            coln++;
        }
        
        if (hourwise != "")
        {
            
            //dtrow[coln] = per_tot_ondu.ToString();
            dtrow[coln] = (per_tot_ondu + tot_ondu_spl).ToString();
            coln++;
        }
        if (pointchk.Checked == true)
        {
            setfp += 1;
            
            dtrow[coln] = cum_tot_point.ToString();
            coln++;
        }
        pre_present_date = 0;
        per_per_hrs = 0;
        cum_per_perhrs = 0;
        per_absent_date = 0;
        pre_ondu_date = 0;
        pre_leave_date = 0;
        per_workingdays = 0;
        cum_tot_ondu = 0;
        cum_present_date = 0;
        cum_perhrs = 0;
        cum_absent_date = 0;
        cum_ondu_date = 0;
        cum_leave_date = 0;
        cum_workingdays = 0;
        cum_tot_point = 0;
    }

    private void print1()
    {
        if (cumcheck.Checked == false)
        {
            double  ConductedHours = 0;
            double AttendedHours = 0;
            string dum_tage_date, dum_tage_hrs;
            string dum_cum_tage_date, dum_cum_tage_hrs;
            per_tage_date = ((pre_present_date / per_workingdays) * 100);
            if (per_tage_date > 100)
            {
                per_tage_date = 100;
            }
            per_con_hrs = tot_conducted_hours_count;
           // per_con_hrs = ((per_workingdays * NoHrs) - per_dum_unmark + tot_conduct_hr_spl);
            //      per_tage_hrs = ((per_per_hrs / per_con_hrs) * 100);
            per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl) / tot_conducted_hours_count) * 100);
            //per_con_hrs = (per_workingdays1 - cum_dum_unmark) + tot_conduct_hr_spl;
            //per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl) / per_con_hrs) * 100);
            if (per_tage_hrs > 100)
            {
                per_tage_hrs = 100;
            }
            dum_tage_date = String.Format("{0:0,0.00}", float.Parse(per_tage_date.ToString()));
            if (dum_tage_date == "NaN")
            {
                dum_tage_date = "0";
            }
            else if (dum_tage_date == "Infinity")
            {
                dum_tage_date = "0";
            }
            dum_tage_hrs = String.Format("{0:0,0.00}", float.Parse(per_tage_hrs.ToString()));
            if (dum_tage_hrs == "NaN")
            {
                dum_tage_hrs = "0";
            }
            else if (dum_tage_hrs == "Infinity")
            {
                dum_tage_hrs = "0";
            }
            per_con_hrs = ((per_workingdays * NoHrs) - per_dum_unmark);
            setfp += 1;
            

            if (daywise != "")
            {
                dtrow[coln] = per_workingdays.ToString();
                coln++;
            }
            setfp += 1;
            
            if (daywise != "")
            {
                dtrow[coln] = pre_present_date.ToString();
                coln++;
            }
            setfp += 1;
            //added by sudhagar
            double roundPer = 0;
            if (cbincround.Checked)
                double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(dum_tage_date))), out roundPer);
            else
                double.TryParse(Convert.ToString(dum_tage_date), out roundPer);
            
            if (daywise != "")
            {
                dtrow[coln] = Convert.ToString(roundPer);
                coln++;
            }
            //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = dum_tage_date.ToString();
            //Added by srinath 21/8/2013
            setfp += 1;
            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = per_con_hrs.ToString();
           
            if (hourwise != "")
            {
                ConductedHours = tot_conducted_hours_count + tot_conduct_hr_spl;
                dtrow[coln] = (tot_conducted_hours_count + tot_conduct_hr_spl).ToString();
                coln++;
            }
            setfp += 1;
            
            if (hourwise != "")
            {
                
                AttendedHours = per_per_hrs + tot_per_hrs_spl;
                dtrow[coln] = (per_per_hrs + tot_per_hrs_spl).ToString();
                coln++;
            }
            setfp += 1;
            //added by sudhagar
            double roundPers = 0;
            if (cbincround.Checked)
                double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(dum_tage_hrs))), out roundPers);
            else
                double.TryParse(Convert.ToString(dum_tage_hrs), out roundPers);
            

            if (hourwise != "")
            {
                //dtrow[coln] = Convert.ToString(roundPers);
                string per="0";
                if (AttendedHours != 0 && ConductedHours != 0)
                {
                    if (cbincround.Checked)
                        per = Convert.ToString(Math.Round(Convert.ToDecimal((AttendedHours / ConductedHours) * 100)));
                    else
                        per = Convert.ToString(Math.Round(Convert.ToDecimal((AttendedHours / ConductedHours) * 100), 2));
                }
                dtrow[coln] = per;
                coln++;
            }
            //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = dum_tage_hrs.ToString();
            // setfp += 1;
            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = per_tot_ondu.ToString();
            setfp += 1;
            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = (per_absent_date + per_abshrs_spl).ToString();
            
            if (daywise != "")
            {
                dtrow[coln] = (per_absent_date).ToString();
                coln++;
            }
            setfp += 1;
            
            if (daywise != "")
            {
                dtrow[coln] = (pre_leave_date + per_leave).ToString();
                coln++;
            }
            setfp += 1;
            
            if (daywise != "")
            {
                //dtrow[coln] = (pre_ondu_date + tot_ondu_spl).ToString();
                dtrow[coln] = (pre_ondu_date ).ToString();
                coln++;
            }
            setfp += 1;
            
            if (hourwise != "")
            {
                //dtrow[coln] = tot_per_abshrs.ToString();
                dtrow[coln] = (ConductedHours - AttendedHours).ToString();
                
                coln++;
            }
            
            setfp += 1;
            
            if (hourwise != "")
            {
                dtrow[coln] = tot_per_leave.ToString();
                coln++;
            }
            
            setfp += 1;
            
            if (hourwise != "")
            {
                dtrow[coln] = (per_tot_ondu + tot_ondu_spl).ToString();
                coln++;
            }
            
            pre_present_date = 0;
            per_per_hrs = 0;
            cum_per_perhrs = 0;
            per_absent_date = 0;
            pre_ondu_date = 0;
            pre_leave_date = 0;
            per_workingdays = 0;
            cum_tot_ondu = 0;
            cum_present_date = 0;
            cum_perhrs = 0;
            cum_absent_date = 0;
            cum_ondu_date = 0;
            cum_leave_date = 0;
            cum_workingdays = 0;
            cum_tot_point = 0;
            per_abshrs_spl = 0;
        }
    }

    private void print()
    {
        

        ++rown;
        dtrow = dtl.NewRow();
        coln = 0;
        dtrow[coln] = Convert.ToString(rown).Trim();
        coln++;
        tag1 = ds4.Tables[0].Rows[rows_count]["delflag"].ToString();
        tag2 = ds4.Tables[0].Rows[rows_count]["exam_flag"].ToString();

        if (Session["Rollflag"].ToString() == "0")
        {
            
        }
        else
        {
            if (tag1.Trim().ToUpper() == "1" || tag1.Trim().ToUpper() == "TRUE" || tag1.Trim().ToUpper() == "1" || tag1.Trim().ToUpper() == "TRUE" || tag2 == "Debar")
            {
                dtrow[coln] = ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString() + "^red";
                coln++;

            }
            else
            {
                dtrow[coln] = ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString();
                coln++;
            }
        }
        if (Session["Regflag"].ToString() == "0")
        {
            
        }
        else
        {
            if (tag1.Trim().ToUpper() == "1" || tag1.Trim().ToUpper() == "TRUE" || tag1.Trim().ToUpper() == "1" || tag1.Trim().ToUpper() == "TRUE" || tag2 == "Debar")
            {
                dtrow[coln] = ds4.Tables[0].Rows[rows_count]["REG_NO"].ToString() + "^red";
                coln++;
            }
            else
            {
                dtrow[coln] = ds4.Tables[0].Rows[rows_count]["REG_NO"].ToString() ;
                coln++;
            }
        }
        if (Session["admissionno"].ToString() == "0")
        {
            
        }
        else
        {
            if (tag1.Trim().ToUpper() == "1" || tag1.Trim().ToUpper() == "TRUE" || tag1.Trim().ToUpper() == "1" || tag1.Trim().ToUpper() == "TRUE" || tag2 == "Debar")
            {
                dtrow[coln] = ds4.Tables[0].Rows[rows_count]["ADMIT_NO"].ToString() + "^red";
                coln++;
            }
            else
            {
                dtrow[coln] = ds4.Tables[0].Rows[rows_count]["ADMIT_NO"].ToString(); 
                coln++;
            }
        }

        if (tag1.Trim().ToUpper() == "1" || tag1.Trim().ToUpper() == "TRUE" || tag1.Trim().ToUpper() == "1" || tag1.Trim().ToUpper() == "TRUE" || tag2 == "Debar")
        {
            dtrow[coln] = ds4.Tables[0].Rows[rows_count]["STUD_NAME"].ToString() + "^red";
            coln++;
        }
        else
        {
            dtrow[coln] = ds4.Tables[0].Rows[rows_count]["STUD_NAME"].ToString(); 
            coln++;
        }

       

        //added by anandan for adding cellnote
        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = ds4.Tables[0].Rows[rows_count]["ADM_DATE"].ToString();
        setfp = 4;
    }

    private void perdats()
    {
        setfp += 1;
        Color foreColor = ColorTranslator.FromHtml("#000000");
        switch (pp)
        {
            case "P":
                foreColor = ColorTranslator.FromHtml("#035523");
                break;
            case "A":
                foreColor = ColorTranslator.FromHtml("#F21C03");
                break;
            case "OD":
                foreColor = ColorTranslator.FromHtml("#3000D3");
                break;
            case "NE":
                foreColor = ColorTranslator.FromHtml("#000000");
                break;
            case "NJ":
                foreColor = ColorTranslator.FromHtml("#9057C3");
                break;
            case "H":
                foreColor = ColorTranslator.FromHtml("#C41D9E");
                break;
            default:
                foreColor = ColorTranslator.FromHtml("#000000");
                break;
        }
        
       

        dtrow[coln] = pp.ToString();
        coln++;

        pp = string.Empty;
    }

    private void spsizeforcum()
    {
        frdate = txtFromDate.Text;
        todate = txtToDate.Text;
        year_fromat();
        ts = DateTime.Parse(todate.ToString()).Subtract(DateTime.Parse(frdate.ToString()));
        diff_date = Convert.ToString(ts.Days);
        difdate = int.Parse(diff_date.ToString());
        difdate += 1;
        // per_dif_dates = difdate - 3;
        difdate = (difdate * NoHrs) + 3;


        //------------------------------------------------------------------
        //=====================================11/6/12 PRABHA
        //Added by srinath 21/8/2013s
        if (Session["group_code"] != null && (Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim().Split(';')[0] + "";
        }
        else
        {
            grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
        }
        
        //con.Close();
        //cmd.CommandText = "select rights from  special_hr_rights where " + grouporusercode + "";
        //cmd.Connection = con;
        //con.Open();
        //SqlDataReader dr_rights_spl_hr = cmd.ExecuteReader();
        DataSet dsRights = new DataSet();
        if (!string.IsNullOrEmpty(grouporusercode))
        {
            qry = "select rights from  special_hr_rights where " + grouporusercode + "";
            dsRights = da.select_method_wo_parameter(qry, "text");
        }
        //SqlDataReader dr_rights_spl_hr = cmd.ExecuteReader();
        if (dsRights.Tables.Count > 0 && dsRights.Tables[0].Rows.Count > 0)
        {
            //while (dr_rights_spl_hr.Read())
            foreach (DataRow dr_rights_spl_hr in dsRights.Tables[0].Rows)
            {
                string spl_hr_rights = string.Empty;
                Hashtable od_has = new Hashtable();
                spl_hr_rights = dr_rights_spl_hr["rights"].ToString();
                if (spl_hr_rights.Trim().ToLower() == "true" || spl_hr_rights.Trim().ToLower() == "1")
                {
                    splhr_flag_head = true;
                    //getspecial_hr();
                }
            }
        }
        //===================================

        
        if (pointchk.Checked == true)
        {
            
        }
        else
        {
            
        }
        
        //'===============================settings====================================

        dtrow = dtl.NewRow();
        dtl.Rows.Add(dtrow);

        dtrow = dtl.NewRow();
        dtl.Rows.Add(dtrow);
        int colu = 0;


        dtl.Columns.Add("S.No", typeof(string));
        dtl.Rows[0][colu] = "S.No";
        colu++;

        if (Session["Rollflag"].ToString() == "0")
        {
            
        }
        else
        {
            dtl.Columns.Add("Roll No", typeof(string));
            dtl.Rows[0][colu] = "Roll No";
            colu++;
        }
        if (Session["Regflag"].ToString() == "0")
        {
            
        }
        else
        {
            dtl.Columns.Add("Register No", typeof(string));
            dtl.Rows[0][colu] = "Register No";
            colu++;
        }
        if (Session["admissionno"].ToString() == "0")
        {
            
        }
        else
        {
            dtl.Columns.Add("Admission No", typeof(string));
            dtl.Rows[0][colu] = "Admission No";
            colu++;
        }

        dtl.Columns.Add("Name", typeof(string));
        dtl.Rows[0][colu] = "Name";
        colu++;

        ViewState["temp_table"] = dtl.Columns.Count;
        //================================================
        for (int col = 0; col < dtl.Columns.Count; col++)
        {
            if (col >= 5)
            {
                
            }
            if (col == 4)
            {
                
            }
           
        }
        ddiff = 5;
        int headdatecol = 0;
        int headdatecolcount = 0;
        dumm_from_date1 = dumm_from_date;
        while (Convert.ToDateTime(todate) >= dumm_from_date1)
        {
            headdatecolcount++;
            dumm_from_date1 = dumm_from_date1.AddDays(1);
        }
        headdate = new string[headdatecolcount];
        string ssss = "";
        ViewState["Hrs"] = NoHrs;
        if (splhr_flag_head == true)
        {
            string sec;
            string splhrsec = string.Empty;
            if (ddlsec.Enabled == true)
            {
                if (ddlsec.SelectedItem.ToString() == string.Empty || ddlsec.Text == "All")
                {
                    sec = string.Empty;
                    splhrsec = string.Empty;
                }
                else
                {
                    sec = " and sections='" + ddlsec.SelectedItem.ToString() + "'";
                    splhrsec = " and sm.sections='" + ddlsec.SelectedItem.ToString() + "'";
                }
            }
            else
            {
                sec = string.Empty;
            }
            string frdate22 = txtFromDate.Text;
            string todate22 = txtToDate.Text;
            string[] fromdatespilt = frdate22.Split('/');
            DateTime frdatetime = Convert.ToDateTime(fromdatespilt[1] + '/' + fromdatespilt[0] + '/' + fromdatespilt[2]);
            string[] todatespilt = todate22.Split('/');
            DateTime todatetimes = Convert.ToDateTime(todatespilt[1] + '/' + todatespilt[0] + '/' + todatespilt[2]);
            string getsphr = "select distinct  date,hrdet_no,sd.start_time,sd.end_time from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and date between '" + frdatetime.ToString() + "' and '" + todatetimes.ToString() + "' " + splhrsec + "";
            ds_sphr = d2.select_method(getsphr, hat, "Text");
        }
        string temp = " ";
        while (Convert.ToDateTime(todate) >= dumm_from_date)
        {

            

            headdate[headdatecol] = dumm_from_date.ToString("dd") + "/" + dumm_from_date.ToString("MM") + "/" + dumm_from_date.ToString("yyyy");
            dtl.Columns.Add(" ", typeof(string));
            dtl.Rows[0][colu] = dumm_from_date.ToString("dd") + "/" + dumm_from_date.ToString("MM") + "/" + dumm_from_date.ToString("yyyy");

            ssss += " ";
            
            for (int sss = 1; sss <= NoHrs; sss++)
            {
                
                ddiff = ddiff + 1;


                if (sss == 1)
                    dtl.Columns[colu].ColumnName = Convert.ToString(sss) + ssss;
                else 
                    dtl.Columns.Add(Convert.ToString(sss) + ssss, typeof(string));

                dtl.Rows[1][colu] = Convert.ToString(sss);
                colu++;
                //Hrs = new System.Text.StringBuilder(sss.ToString());
                //AddTableColumn(dtl, Hrs);
            }

            if (splhr_flag_head == true)
            {

                DateTime spfromdate = Convert.ToDateTime(dumm_from_date.ToString("MM") + '/' + dumm_from_date.ToString("dd") + '/' + dumm_from_date.ToString("yyyy"));
                ds_sphr.Tables[0].DefaultView.RowFilter = "date = '" + spfromdate.ToString() + "' ";
                DataView sph = ds_sphr.Tables[0].DefaultView;
                ArrayList ht_sphr_head = new ArrayList();
                sph_datewise.Add("0");
                if (sph.Count > 0)
                {

                    int hr = 0;
                    for (int sphr = 0; sphr < sph.Count; sphr++)
                    {

                        string strtime = Convert.ToString(sph[sphr]["start_time"]);
                        string endtime = Convert.ToString(sph[sphr]["end_time"]);
                        if (ht_sphr_head.Contains(strtime + "-" + endtime))
                        {

                        }
                        else
                        {
                            ht_sphr_head.Add(Convert.ToString(strtime + "-" + endtime));
                            hr++;
                            temp = temp + " ";
                            //colcount = colcount + 2;
                            //dtl.Columns.Add(" ", typeof(string));
                            dtl.Columns.Add(" SH " + hr + "" + temp, typeof(string));



                            dtl.Rows[1][colu] = Convert.ToString(" SH " + hr + "");
                            colu++;

                            tot_sphval++;
                            sph_datewise[sph_datewise.Count - 1] = hr;
                        }
                    }
                }

            }
            headdatecol++;
            dumm_from_date = dumm_from_date.AddDays(1);
        }
        
        ddiff = ddiff + 1;
       
        ddiff = ddiff + 1;
        
        if (Session["daywise"].ToString() == "1")
        {

            
            daywise = "Days Percentage From " + txtFromDate.Text + " To " + txtToDate.Text;
            dtl.Columns.Add(" ", typeof(string));
            dtl.Rows[0][colu] = "Days Percentage From " + txtFromDate.Text + " To " + txtToDate.Text;
         
            
            dtl.Columns[colu].ColumnName = "Conducted days";
            dtl.Rows[1][colu] = "Conducted days";
            colu++;
            dtl.Columns.Add("Attended days", typeof(string));
            dtl.Rows[1][colu] = "Attended days";
            colu++;
            dtl.Columns.Add("Percentage", typeof(string));
            dtl.Rows[1][colu] = "Percentage";
            colu++;

            
        }
        else
        {
            
        }
        ddiff = ddiff + 1;
        
        ddiff = ddiff + 1;
        
        ddiff = ddiff + 1;
        
        if (Session["hourwise"].ToString() == "1")
        {
            
            

            hourwise = "Hours Percentage From " + txtFromDate.Text + " To " + txtToDate.Text;
            dtl.Columns.Add(" ", typeof(string));
            dtl.Rows[0][colu] = "Hours Percentage From " + txtFromDate.Text + " To " + txtToDate.Text;

            
            dtl.Columns[colu].ColumnName = "Conducted Hours";
            dtl.Rows[1][colu] = "Conducted Hours";
            colu++;

            dtl.Columns.Add("Attended Hours", typeof(string));
            dtl.Rows[1][colu] = "Attended Hours";
            colu++;

            dtl.Columns.Add(" Percentage ", typeof(string));
            dtl.Rows[1][colu] = " Percentage ";
            colu++;
        }
        else
        {
            
        }
        //--------------------------------------------------------------------------
        ddiff = ddiff + 1;
        
        ddiff = ddiff + 1;
        
        ddiff = ddiff + 1;
        
        if (Session["daywise"].ToString() == "1")
        {
            

            cumdaywise = "Days Cumulative Percentage From " + cumfromtxt.Text + " To " + cumtotxt.Text;
            dtl.Columns.Add(" ", typeof(string));
            dtl.Rows[0][colu] = "Days Cumulative Percentage From " + cumfromtxt.Text + " To " + cumtotxt.Text;
            
            dtl.Columns[colu].ColumnName = " Conducted days ";
            dtl.Rows[1][colu] = "Conducted days";
            colu++;
            dtl.Columns.Add(" Attended days ", typeof(string));
            dtl.Rows[1][colu] = "Attended days";
            colu++;
            dtl.Columns.Add("Percentage ", typeof(string));
            dtl.Rows[1][colu] = "Percentage";
            colu++;
        }
        else
        {
            
        }
        ddiff = ddiff + 1;
        
        ddiff = ddiff + 1;
       
        ddiff = ddiff + 1;
        
        if (Session["hourwise"].ToString() == "1")
        {
            

            cumhourwise = "Hours Cumulative Percentage From " + cumfromtxt.Text + " To " + cumtotxt.Text;
            dtl.Columns.Add(" ", typeof(string));
            dtl.Rows[0][colu] = "Hours Cumulative Percentage From " + cumfromtxt.Text + " To " + cumtotxt.Text;

            
            dtl.Columns[colu].ColumnName = " Conducted Hours ";
            dtl.Rows[1][colu] = " Conducted Hours ";
            colu++;
            dtl.Columns.Add(" Attended Hours ", typeof(string));
            dtl.Rows[1][colu] = " Attended Hours ";
            colu++;
            dtl.Columns.Add(" Percentage", typeof(string));
            dtl.Rows[1][colu] = " Percentage";
            colu++;
        }
        else
        {
            
        }

        int cccc = dtl.Columns.Count;
        ddiff = ddiff + 1;
        //--------------------------------------------------------------------------
        //--------------------------------------------------------------------------
        
        ddiff = ddiff + 1;
        
        ddiff = ddiff + 1;
        
        if (Session["daywise"].ToString() == "1")
        {
            

            dtl.Columns.Add("No of Days Absent ", typeof(string));
            dtl.Rows[0][colu] = "No of Days Absent ";
            colu++;
            dtl.Columns.Add("No of Days Leave ", typeof(string));
            dtl.Rows[0][colu] = "No of Days Leave ";
            colu++;
            dtl.Columns.Add("No of Days OD ", typeof(string));
            dtl.Rows[0][colu] = "No of Days OD ";
            colu++;
        }
        else
        {
            
        }
        ddiff = ddiff + 1;
        
        ddiff = ddiff + 1;
        
        ddiff = ddiff + 1;
        
        if (Session["hourwise"].ToString() == "1")
        {
            

            dtl.Columns.Add("No of Hours Absent", typeof(string));
            dtl.Rows[0][colu] = "No of Hours Absent";
            colu++;
            dtl.Columns.Add("No of Hours Leave ", typeof(string));
            dtl.Rows[0][colu] = "No of Hours Leave ";
            colu++;
            dtl.Columns.Add("No of Hours OD ", typeof(string));
            dtl.Rows[0][colu] = "No of Hours OD ";
            colu++;
        }
        else
        {
            
        }
        if (pointchk.Checked == true)
        {
            ddiff = ddiff + 1;
            

            dtl.Columns.Add("PTS", typeof(string));
            dtl.Rows[0][colu] = "PTS";
            colu++;
        }
        ddiff = ddiff + 1;
        
        ddiff = ddiff + 1;
        

        dtl.Columns.Add("Signature of the Student ", typeof(string));
        dtl.Rows[0][colu] = "Signature of the Student ";
        colu++;
        dtl.Columns.Add("Remarks", typeof(string));
        dtl.Rows[0][colu] = "Remarks";
        colu++;

        ViewState["lastcolspan"] = dtl.Columns.Count - cccc;
    }

    private void spsize()
    {
        frdate = txtFromDate.Text;
        todate = txtToDate.Text;
        year_fromat();
        ts = DateTime.Parse(todate.ToString()).Subtract(DateTime.Parse(frdate.ToString()));
        diff_date = Convert.ToString(ts.Days + 4);
        difdate = int.Parse(diff_date.ToString());
        per_dif_dates = difdate - 3;
        difdate = (per_dif_dates * NoHrs) + 14;

        //------------------------------------------------------------------
        //=====================================11/6/12 PRABHA
        //Added by srinath 21/8/2013s
        if (Session["group_code"] != null && (Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim().Split(';')[0] + "";
        }
        else
        {
            grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
        }
        
        //con.Close();
        //cmd.CommandText = "select rights from  special_hr_rights where " + grouporusercode + "";
        //cmd.Connection = con;
        //con.Open();
        //SqlDataReader dr_rights_spl_hr = cmd.ExecuteReader();
        DataSet dsRights = new DataSet();
        if (!string.IsNullOrEmpty(grouporusercode))
        {
            qry = "select rights from  special_hr_rights where " + grouporusercode + "";
            dsRights = da.select_method_wo_parameter(qry, "text");
        }
        //SqlDataReader dr_rights_spl_hr = cmd.ExecuteReader();
        if (dsRights.Tables.Count > 0 && dsRights.Tables[0].Rows.Count > 0)
        {
            //while (dr_rights_spl_hr.Read())
            foreach (DataRow dr_rights_spl_hr in dsRights.Tables[0].Rows)
            {
                string spl_hr_rights = string.Empty;
                Hashtable od_has = new Hashtable();
                spl_hr_rights = dr_rights_spl_hr["rights"].ToString();
                if (spl_hr_rights.Trim().ToLower() == "true" || spl_hr_rights.Trim().ToLower() == "1")
                {
                    splhr_flag_head = true;
                    //getspecial_hr();
                }
            }
        }
        //===================================



        dtrow = dtl.NewRow();
        dtl.Rows.Add(dtrow);

        dtrow = dtl.NewRow();
        dtl.Rows.Add(dtrow);
        int colu = 0;
        dtl.Columns.Add("S.No", typeof(string));
        dtl.Rows[0][colu] = "S.No";
        colu++;

        if (Session["Rollflag"].ToString() == "0")
        {
            
        }
        else
        {
            dtl.Columns.Add("Roll No", typeof(string));
            dtl.Rows[0][colu] = "Roll No";
            colu++;
        }
        if (Session["Regflag"].ToString() == "0")
        {
            
        }
        else
        {
            dtl.Columns.Add("Register No", typeof(string));
            dtl.Rows[0][colu] = "Register No";
            colu++;
        }
        if (Session["admissionno"].ToString() == "0")
        {
           
        }
        else
        {
            dtl.Columns.Add("Admission No", typeof(string));
            dtl.Rows[0][colu] = "Admission No";
            colu++;
        }

        dtl.Columns.Add("Name", typeof(string));
        dtl.Rows[0][colu] = "Name";
        colu++;
        ViewState["temp_table"] = dtl.Columns.Count;
        for (int col = 0; col < dtl.Columns.Count; col++)
        {
            if (col >= 5)
            {
                
            }
            if (col == 4)
            {
                
            }
            
        }
        ddiff = 5;
        int headdatecol = 0;
        int headdatecolcount = 0;
        dumm_from_date1 = dumm_from_date;
        while (Convert.ToDateTime(todate) >= dumm_from_date1)
        {
            headdatecolcount++;
            dumm_from_date1 = dumm_from_date1.AddDays(1);
        }
        headdate = new string[headdatecolcount];
        string ssss = "";
        ViewState["Hrs"] = NoHrs;
        if (splhr_flag_head == true)
        {
            string sec;
            string splhrsec = string.Empty;
            if (ddlsec.Enabled == true)
            {
                if (ddlsec.SelectedItem.ToString() == string.Empty || ddlsec.Text == "All")
                {
                    sec = string.Empty;
                    splhrsec = string.Empty;
                }
                else
                {
                    sec = " and sections='" + ddlsec.SelectedItem.ToString() + "'";
                    splhrsec = " and sm.sections='" + ddlsec.SelectedItem.ToString() + "'";
                }
            }
            else
            {
                sec = string.Empty;
            }
            string frdate22 = txtFromDate.Text;
            string todate22 = txtToDate.Text;
            string[] fromdatespilt = frdate22.Split('/');
            DateTime frdatetime = Convert.ToDateTime(fromdatespilt[1] + '/' + fromdatespilt[0] + '/' + fromdatespilt[2]);
            string[] todatespilt = todate22.Split('/');
            DateTime todatetimes = Convert.ToDateTime(todatespilt[1] + '/' + todatespilt[0] + '/' + todatespilt[2]);
            string getsphr = "select distinct  date,hrdet_no,sd.start_time,sd.end_time from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and date between '" + frdatetime.ToString() + "' and '" + todatetimes.ToString() + "' " + splhrsec + "";
            ds_sphr = d2.select_method(getsphr, hat, "Text");
        }
        string temp = " ";
        while (Convert.ToDateTime(todate) >= dumm_from_date)
        {
            

            headdate[headdatecol] = dumm_from_date.ToString("dd") + "/" + dumm_from_date.ToString("MM") + "/" + dumm_from_date.ToString("yyyy");
            dtl.Columns.Add(" ", typeof(string));
            dtl.Rows[0][colu] = dumm_from_date.ToString("dd") + "/" + dumm_from_date.ToString("MM") + "/" + dumm_from_date.ToString("yyyy");
            ssss += " ";
            for (int sss = 1; sss <= NoHrs; sss++)
            {
                
                ddiff = ddiff + 1;
                if (sss == 1)
                    dtl.Columns[colu].ColumnName = Convert.ToString(sss) + ssss;
                else
                    dtl.Columns.Add(Convert.ToString(sss) + ssss, typeof(string));

                dtl.Rows[1][colu] = Convert.ToString(sss);
                colu++;

                
            }
            
            if (splhr_flag_head == true)
            {
               
                DateTime spfromdate = Convert.ToDateTime(dumm_from_date.ToString("MM") + '/' + dumm_from_date.ToString("dd") + '/' + dumm_from_date.ToString("yyyy"));
                ds_sphr.Tables[0].DefaultView.RowFilter = "date = '" + spfromdate.ToString() + "' ";
                DataView sph = ds_sphr.Tables[0].DefaultView;
                ArrayList ht_sphr_head = new ArrayList();
                sph_datewise.Add("0");
                if (sph.Count > 0)
                {
                    
                    int hr = 0;
                    for (int sphr = 0; sphr < sph.Count; sphr++)
                    {

                        string strtime = Convert.ToString(sph[sphr]["start_time"]);
                        string endtime = Convert.ToString(sph[sphr]["end_time"]);
                        if (ht_sphr_head.Contains(strtime + "-" + endtime))
                        {

                        }
                        else
                        {
                            ht_sphr_head.Add(Convert.ToString(strtime + "-" + endtime));
                            hr++;
                            temp = temp + " ";
                            //colcount = colcount + 2;
                            //dtl.Columns.Add(" ", typeof(string));
                            dtl.Columns.Add(" SH " + hr + "" + temp, typeof(string));

                            

                            dtl.Rows[1][colu] = Convert.ToString(" SH " + hr + "");
                            colu++;

                            tot_sphval++;
                            sph_datewise[sph_datewise.Count - 1] = hr;
                        }
                    }
                }

            }
            headdatecol++;
            dumm_from_date = dumm_from_date.AddDays(1);
        }
        
        ddiff = ddiff + 1;
        
        ddiff = ddiff + 1;
        
        if (Session["daywise"].ToString() == "1")
        {
            

            daywise = "Days Percentage From " + txtFromDate.Text + " To " + txtToDate.Text;
            dtl.Columns.Add(" ", typeof(string));
            dtl.Rows[0][colu] = "Days Percentage From " + txtFromDate.Text + " To " + txtToDate.Text;
            
            
            dtl.Columns[colu].ColumnName = "Conducted days";
            dtl.Rows[1][colu] = "Conducted days";
            colu++;
            dtl.Columns.Add("Attended days", typeof(string));
            dtl.Rows[1][colu] = "Attended days";
            colu++;
            dtl.Columns.Add("Percentage", typeof(string));
            dtl.Rows[1][colu] = "Percentage";
            colu++;
        }
        else
        {
            
        }
        //--------------------------------------------------------------------------
        ddiff = ddiff + 1;
        
        ddiff = ddiff + 1;
        
        ddiff = ddiff + 1;
        
        if (Session["hourwise"].ToString() == "1")
        {
            

            hourwise = "Hours Percentage From " + txtFromDate.Text + " To " + txtToDate.Text;
            dtl.Columns.Add(" ", typeof(string));
            dtl.Rows[0][colu] = "Hours Percentage From " + txtFromDate.Text + " To " + txtToDate.Text;

            dtl.Columns[colu].ColumnName = "Conducted Hours";
            dtl.Rows[1][colu] = "Conducted Hours";
            colu++;
            dtl.Columns.Add("Attended Hours", typeof(string));
            dtl.Rows[1][colu] = "Attended Hours";
            colu++;
            dtl.Columns.Add("Percentage ", typeof(string));
            dtl.Rows[1][colu] = "Percentage ";
            colu++;
        }
        else
        {
            
        }
        //--------------------------------------------------------------------------
        int ccc = dtl.Columns.Count;
        ddiff = ddiff + 1;
        
        ddiff = ddiff + 1;
        
        ddiff = ddiff + 1;
        
        if (Session["daywise"].ToString() == "1")
        {
            

            dtl.Columns.Add("No of Days Absent ", typeof(string));
            dtl.Rows[0][colu] = "No of Days Absent ";
            colu++;
            dtl.Columns.Add("No of Days Leave ", typeof(string));
            dtl.Rows[0][colu] = "No of Days Leave ";
            colu++;
            dtl.Columns.Add("No of Days OD ", typeof(string));
            dtl.Rows[0][colu] = "No of Days OD ";
            colu++;
        }
        else
        {
            
        }
        
        
        if (Session["hourwise"].ToString() == "1")
        {
            

            dtl.Columns.Add("No of Periods Absent", typeof(string));
            dtl.Rows[0][colu] = "No of Periods Absent";
            colu++;
            dtl.Columns.Add("No of Periods Leave ", typeof(string));
            dtl.Rows[0][colu] = "No of Periods Leave ";
            colu++;
            dtl.Columns.Add("No of Periods OD ", typeof(string));
            dtl.Rows[0][colu] = "No of Periods OD ";
            colu++;
        }
        else
        {
            
        }

        dtl.Columns.Add("Signature of the Student", typeof(string));
        dtl.Rows[0][colu] = "Signature of the Student";
        colu++;
        dtl.Columns.Add("Remarks", typeof(string));
        dtl.Rows[0][colu] = "Remarks";
        colu++;

        ViewState["lastcolspan"] = dtl.Columns.Count - ccc;
    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FpSpread1.CurrentPage = 0;
        pagesearch_txt.Text = string.Empty;
        errmsg.Visible = false;
        pagesearch_txt.Text = string.Empty;
        pageddltxt.Text = string.Empty;
        pageddltxt.Text = string.Empty;
        if (DropDownListpage.Text == "Others")
        {
            pageddltxt.Visible = true;
            pageddltxt.Focus();
        }
        else
        {
            pageddltxt.Visible = false;
            //FpSpread1.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            CalculateTotalPages();
        }
    }

    protected void pageddltxt_TextChanged(object sender, EventArgs e)
    {
        //FpSpread1.CurrentPage = 0;
        pagesearch_txt.Text = string.Empty;
        //try
        {
            if (pageddltxt.Text != string.Empty)
            {
                //if (FpSpread1.Sheets[0].RowCount >= Convert.ToInt16(pageddltxt.Text.ToString()) && Convert.ToInt16(pageddltxt.Text.ToString()) != 0)
                //{
                //    FpSpread1.Sheets[0].PageSize = Convert.ToInt16(pageddltxt.Text.ToString());
                //    errmsg.Visible = false;
                //    CalculateTotalPages();
                //}
                //else
                //{
                //    errmsg.Visible = true;
                //    errmsg.Text = "Please Enter valid Record count";
                //    pageddltxt.Text = string.Empty;
                //}
            }
        }
        //catch
        //{
        //    errmsg.Visible = true;
        //    errmsg.Text = "Please Enter valid Record count";
        //    pageddltxt.Text =string.Empty;
        //}
    }

    protected void pagesearch_txt_TextChanged(object sender, EventArgs e)
    {
        if (pagesearch_txt.Text.Trim() != "")
        {
            if (Convert.ToInt64(pagesearch_txt.Text) > Convert.ToInt64(Session["totalPages"]))
            {
                errmsg.Visible = true;
                errmsg.Text = "Exceed The Page Limit";
                Showgrid.Visible = true;
                btnprintmaster.Visible = true;
                btnPrint.Visible = true;
                Printcontrol.Visible = false;
                btnxl.Visible = true;
                //Added by Srinath 27/2/2
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                pagesearch_txt.Text = string.Empty;
            }
            else if (Convert.ToInt64(pagesearch_txt.Text) == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = "Page search should be more than 0";
                Showgrid.Visible = true;
                btnprintmaster.Visible = true;
                btnPrint.Visible = true;
                Printcontrol.Visible = false;
                btnxl.Visible = true;
                //Added by Srinath 27/2/2
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                pagesearch_txt.Text = string.Empty;
            }
            else
            {
                errmsg.Visible = false;
               // FpSpread1.CurrentPage = Convert.ToInt16(pagesearch_txt.Text) - 1;
                Showgrid.Visible = true;
                btnprintmaster.Visible = true;
                btnPrint.Visible = true;
                btnPrint.Visible = true;
                Printcontrol.Visible = false;
                btnxl.Visible = true;
                //Added by Srinath 27/2/2
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
            }
        }
    }

    void CalculateTotalPages()
    {
        Double totalRows = 0;
        totalRows = Convert.ToInt32(dtl.Rows.Count);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / dtl.Rows.Count);
        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        Buttontotal.Visible = true;
    }

    public void presentdays()
    {
        frdate = txtFromDate.Text;
        todate = txtToDate.Text;
        persentmonthcal();
    }

    private void persentmonthcal()
    {
        try
        {
            tot_conducted_hours_count = 0;
            if (check == 1)
                tot_conducted_hours_count22 = 0;
            string holiday_sched_details = "", halforfull = "", mng = "", evng = string.Empty;
            year_fromat();
            hat.Clear();
            ds2.Clear();
            int temp_month = 0;
            string rollNum = ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString();
            temp_month = cal_from_date;
            hat.Add("std_rollno", ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString());
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds2 = d2.select_method("STUD_ATTENDANCE", hat, "sp");
            string admdat = ds4.Tables[0].Rows[rows_count]["adm_date"].ToString();
            if (admdat.Trim() != "")
            {
                dtadm = Convert.ToDateTime(admdat);
            }
            else
            {
                dtadm = dumm_from_date;
            }
            // if (rows_count == 0)
            {
                chkdegreesem = ddlbranch.SelectedValue.ToString() + '/' + ddlduration.SelectedItem.ToString();
                if (chkdegreesem != tempdegreesem)
                {
                    tempdegreesem = chkdegreesem;
                    hat.Clear();
                    ds3.Clear();
                    hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
                    hat.Add("sem", int.Parse(ddlduration.SelectedItem.ToString()));
                    hat.Add("from_date", frdate.ToString());
                    hat.Add("to_date", todate.ToString());
                    hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));
                    //ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
                    //------------------------------------------------------------------
                    int iscount = 0;
                    DataSet dsholiday = new DataSet();
                    dsholiday.Clear();
                    holidaycon.Close();
                    holidaycon.Open();
                    string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedItem.ToString() + "";
                    //SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, holidaycon);
                    //SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);               
                    //daholiday.Fill(dsholiday);
                    qry = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedItem.ToString() + "";
                    dsholiday.Clear();
                    dsholiday = da.select_method_wo_parameter(qry, "text");
                    if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
                    {
                        iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                    }
                    hat.Add("iscount", iscount);
                    ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
                    Dictionary<string, int> holiday_table1 = new Dictionary<string, int>();
                    //Dictionary<string,int> holiday_table2 = new Dictionary<string,int>();
                    //Dictionary<string,int> holiday_table3 = new Dictionary<string,int>();
                    holiday_table11.Clear();
                    //holiday_table21.Clear();
                    //holiday_table31.Clear();
                    if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds3.Tables[0].Rows.Count; k++)
                        {
                            string[] split_date_time1 = ds3.Tables[0].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                            if (ds3.Tables[0].Rows[k]["halforfull"].ToString() == "False")
                            {
                                halforfull = "0";
                            }
                            else
                            {
                                halforfull = "1";
                            }
                            if (ds3.Tables[0].Rows[k]["morning"].ToString() == "False")
                            {
                                mng = "0";
                            }
                            else
                            {
                                mng = "1";
                            }
                            if (ds3.Tables[0].Rows[k]["evening"].ToString() == "False")
                            {
                                evng = "0";
                            }
                            else
                            {
                                evng = "1";
                            }
                            holiday_sched_details = halforfull + "*" + mng + "*" + evng;
                            string[] dummy_split = split_date_time1[0].Split('/');
                            holiday_table11.Add(Convert.ToInt16(dummy_split[2]) + "/" + Convert.ToInt16(dummy_split[1]) + "/" + Convert.ToInt16(dummy_split[0]), holiday_sched_details);
                            // holiday_table1.Add(Convert.ToInt16(dummy_split[2]) + "/" + Convert.ToInt16(dummy_split[1]) + "/" + Convert.ToInt16(dummy_split[0]), holiday_sched_details);
                        }
                    }
                    if (ds3.Tables.Count > 1 && ds3.Tables[1].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds3.Tables[1].Rows.Count; k++)
                        {
                            string[] split_date_time1 = ds3.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                            string[] dummy_split = split_date_time1[0].Split('/');
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
                            if (!holiday_table11.ContainsKey(Convert.ToInt16(dummy_split[2]) + "/" + Convert.ToInt16(dummy_split[1]) + "/" + Convert.ToInt16(dummy_split[0])))
                            {
                                holiday_table11.Add(Convert.ToInt16(dummy_split[2]) + "/" + Convert.ToInt16(dummy_split[1]) + "/" + Convert.ToInt16(dummy_split[0]), holiday_sched_details);
                            }
                            //  holiday_table2.Add(Convert.ToInt16(dummy_split[2]) + "/" + Convert.ToInt16(dummy_split[1]) + "/" + Convert.ToInt16(dummy_split[0]), k);
                        }
                    }
                    if (ds3.Tables.Count > 2 && ds3.Tables[2].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds3.Tables[2].Rows.Count; k++)
                        {
                            string[] split_date_time1 = ds3.Tables[2].Rows[k]["HOLI_DATE"].ToString().Split(' ');
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
                            string[] dummy_split = split_date_time1[0].Split('/');
                            if (!holiday_table11.ContainsKey(Convert.ToInt16(dummy_split[2]) + "/" + Convert.ToInt16(dummy_split[1]) + "/" + Convert.ToInt16(dummy_split[0])))
                            {
                                holiday_table11.Add(Convert.ToInt16(dummy_split[2]) + "/" + Convert.ToInt16(dummy_split[1]) + "/" + Convert.ToInt16(dummy_split[0]), holiday_sched_details);
                            }
                            //holiday_table3.Add(Convert.ToInt16(dummy_split[2]) + "/" + Convert.ToInt16(dummy_split[1]) + "/" + Convert.ToInt16(dummy_split[0]), k);
                        }
                    }
                }
            }
            //------------------------------------------------------------------
            //=====================================11/6/12 PRABHA
            //Added by srinath 21/8/2013s
            if (Session["group_code"] != null && (Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim().Split(';')[0] + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            bool splhr_flag = false;
            //con.Close();
            //cmd.CommandText = "select rights from  special_hr_rights where " + grouporusercode + "";
            //cmd.Connection = con;
            //con.Open();
            //SqlDataReader dr_rights_spl_hr = cmd.ExecuteReader();
            DataSet dsRights = new DataSet();
            if (!string.IsNullOrEmpty(grouporusercode))
            {
                qry = "select rights from  special_hr_rights where " + grouporusercode + "";
                dsRights = da.select_method_wo_parameter(qry, "text");
            }
            //SqlDataReader dr_rights_spl_hr = cmd.ExecuteReader();
            if (dsRights.Tables.Count > 0 && dsRights.Tables[0].Rows.Count > 0)
            {
                //while (dr_rights_spl_hr.Read())
                foreach (DataRow dr_rights_spl_hr in dsRights.Tables[0].Rows)
                {
                    string spl_hr_rights = string.Empty;
                    Hashtable od_has = new Hashtable();
                    spl_hr_rights = dr_rights_spl_hr["rights"].ToString();
                    if (spl_hr_rights.Trim().ToLower() == "true" || spl_hr_rights.Trim().ToLower() == "1")
                    {
                        splhr_flag = true;
                        //getspecial_hr();
                    }
                }
            }
            //===================================
            holiday = 0;
            mmyycount = ds2.Tables[0].Rows.Count;
            moncount = mmyycount - 1;
            ds3count = ds3.Tables[1].Rows.Count;
            ds3count = ds3count - 1;
            if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count != 0)
            {
                ts = DateTime.Parse(ds3.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                diff_date = Convert.ToString(ts.Days);
                dif_date1 = double.Parse(diff_date.ToString());
                next = 0;
            }
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                rrowcount = 0;
                rowcount = 0;
                rcount = 0;
                ccount = 0;
                ccount = ds3.Tables[1].Rows.Count;
                ccount = ccount - 1;
                while (dumm_from_date <= (per_to_date))
                {
                    if (splhr_flag == true)
                    {
                        //modified by Srinath 18/2/2013
                        if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
                        {
                            getspecial_hr();
                        }
                    }
                    for (int i = rcount; i < mmyycount; i++)
                    {
                        cal_from_date = (int.Parse(dumm_from_date.ToString("yyyy")) * 12) + (int.Parse(dumm_from_date.ToString("MM")));
                        if (next < ds2.Tables[0].Rows.Count)
                        {
                            if (cal_from_date == int.Parse(ds2.Tables[0].Rows[next]["month_year"].ToString()))
                            {
                                string[] split_date_time1 = dumm_from_date.ToString().Split(' ');
                                string[] dummy_split = split_date_time1[0].Split('/');
                                if (!holiday_table11.ContainsKey(dummy_split[1].ToString() + "/" + dummy_split[0].ToString() + "/" + dummy_split[2].ToString()))
                                {//------------not a holiday
                                    cons_mng_start_hr = 1;
                                    cons_evng_start_hr = 1;
                                    find_attnd_values();
                                }
                                else
                                {//-------if holiday
                                    value_holi_status = GetCorrespondingKey(dummy_split[1].ToString() + "/" + dummy_split[0].ToString() + "/" + dummy_split[2].ToString(), holiday_table11).ToString();
                                    split_holiday_status = value_holi_status.Split('*');
                                    if (split_holiday_status[0].ToString() == "0")//---------full day holiday
                                    {
                                        // workingdays += 1;
                                        per_holidate += 1;
                                        holiday = 1;
                                        dumm_from_date = dumm_from_date.AddDays(1);
                                        if (dumm_from_date.Day == 1)
                                        {
                                            if (moncount > next)
                                            {
                                                next++;
                                                rcount++;
                                            }
                                        }
                                        rrowcount++;
                                        if (check == 1)
                                        {
                                            if (holiday == 1)
                                            {
                                                for (int count = 0; count < NoHrs; count++)
                                                {
                                                    pp = "H";
                                                    perdats();
                                                }

                                                if (splhr_flag_head == true)
                                                {

                                                    DataSet ds_sphrr = new DataSet();
                                                    string getsphr1 = "select sa.attendance,start_time from specialhr_details sd,specialhr_master sm,specialhr_attendance sa where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and date ='" + dumm_from_date.AddDays(-1) + "'  and sa.hrdet_no=sd.hrdet_no and sa.roll_no='" + ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString() + "' order by start_time ";
                                                    getsphr1 +=
                                "select distinct start_time from specialhr_master sm ,specialhr_details sd where  sd.hrentry_no=sm.hrentry_no and date ='" + dumm_from_date.AddDays(-1) + "' and batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " order by start_time";

                                                    ds_sphrr = d2.select_method(getsphr1, hat, "Text");
                                                    if (ds_sphrr.Tables[1].Rows.Count > 0)
                                                    {


                                                        int sphr1 = 0;
                                                        for (int sphr2 = 0; sphr2 < ds_sphrr.Tables[1].Rows.Count; sphr2++)
                                                        {
                                                            if (sphr1 < ds_sphrr.Tables[0].Rows.Count)
                                                            {

                                                                if (ds_sphrr.Tables[1].Rows[sphr2]["start_time"].ToString() == ds_sphrr.Tables[0].Rows[sphr1]["start_time"].ToString())
                                                                {
                                                                    string value2 = ds_sphrr.Tables[0].Rows[sphr1]["attendance"].ToString();

                                                                    if (check == 1)
                                                                    {
                                                                        if (value2 != null && value2 != "0" && value2 != "7" && value2 != "")
                                                                        {
                                                                            present_mark(value2.ToString());
                                                                        }
                                                                        else
                                                                        {
                                                                            pp = "NE";
                                                                            perdats();
                                                                        }
                                                                    }
                                                                    sphr1++;
                                                                }
                                                                else
                                                                {
                                                                    if (check == 1)
                                                                    {
                                                                        dtrow[coln] = "-";
                                                                        coln++;
                                                                    }
                                                                }


                                                            }
                                                            else
                                                            {
                                                                if (check == 1)
                                                                {
                                                                    dtrow[coln] = "-";
                                                                    coln++;
                                                                }
                                                            }
                                                        }

                                                    }

                                                }

                                            }
                                        }
                                    }
                                    else//---------halfday holiday
                                    {
                                        if (split_holiday_status[1].ToString() == "1")//---mng only holiday
                                        {
                                            cons_mng_start_hr = 0;
                                            cons_evng_start_hr = 1;
                                            //   workingdays += 0.5;
                                            per_holidate += 0.5;
                                            holiday = 1;
                                            //   dumm_from_date = dumm_from_date.AddDays(1);
                                            if (dumm_from_date.Day == 1)
                                            {
                                                if (moncount > next)
                                                {
                                                    next++;
                                                    rcount++;
                                                }
                                            }
                                            rrowcount++;
                                            if (check == 1)
                                            {
                                                if (holiday == 1)
                                                {
                                                    for (int count = 0; count < fnhrs; count++)
                                                    {
                                                        pp = "H";
                                                        perdats();
                                                    }
                                                    if (splhr_flag_head == true)
                                                    {

                                                        DataSet ds_sphrr = new DataSet();
                                                        string getsphr1 = "select sa.attendance,start_time from specialhr_details sd,specialhr_master sm,specialhr_attendance sa where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and date ='" + dumm_from_date.ToString() + "'  and sa.hrdet_no=sd.hrdet_no and sa.roll_no='" + ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString() + "' order by start_time ";
                                                        getsphr1 +=
                                    "select distinct start_time from specialhr_master sm ,specialhr_details sd where  sd.hrentry_no=sm.hrentry_no and date ='" + dumm_from_date.ToString() + "' and batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " order by start_time";

                                                        ds_sphrr = d2.select_method(getsphr1, hat, "Text");
                                                        if (ds_sphrr.Tables[1].Rows.Count > 0)
                                                        {


                                                            int sphr1 = 0;
                                                            for (int sphr2 = 0; sphr2 < ds_sphrr.Tables[1].Rows.Count; sphr2++)
                                                            {
                                                                if (sphr1 < ds_sphrr.Tables[0].Rows.Count)
                                                                {

                                                                    if (ds_sphrr.Tables[1].Rows[sphr2]["start_time"].ToString() == ds_sphrr.Tables[0].Rows[sphr1]["start_time"].ToString())
                                                                    {
                                                                        string value2 = ds_sphrr.Tables[0].Rows[sphr1]["attendance"].ToString();

                                                                        if (check == 1)
                                                                        {
                                                                            if (value2 != null && value2 != "0" && value2 != "7" && value2 != "")
                                                                            {
                                                                                present_mark(value2.ToString());
                                                                            }
                                                                            else
                                                                            {
                                                                                pp = "NE";
                                                                                perdats();
                                                                            }
                                                                        }
                                                                        sphr1++;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (check == 1)
                                                                        {
                                                                            dtrow[coln] = "-";
                                                                            coln++;
                                                                        }
                                                                    }


                                                                }
                                                                else
                                                                {
                                                                    if (check == 1)
                                                                    {
                                                                        dtrow[coln] = "-";
                                                                        coln++;
                                                                    }
                                                                }
                                                            }

                                                        }

                                                    }
                                                }
                                            }
                                            find_attnd_values();
                                        }
                                        if (split_holiday_status[2].ToString() == "1")//---envg only holiday
                                        {
                                            cons_mng_start_hr = 1;
                                            cons_evng_start_hr = 0;
                                            find_attnd_values();
                                            //    workingdays += 0.5;
                                            per_holidate += 0.5;
                                            holiday = 1;
                                            //   dumm_from_date = dumm_from_date.AddDays(1);
                                            if (dumm_from_date.Day == 1)
                                            {
                                                if (moncount > next)
                                                {
                                                    next++;
                                                    rcount++;
                                                }
                                            }
                                            rrowcount++;
                                            if (check == 1)
                                            {
                                                if (holiday == 1)
                                                {
                                                    for (int count = fnhrs; count < NoHrs; count++)
                                                    {
                                                        pp = "H";
                                                        perdats();
                                                    }
                                                    if (splhr_flag_head == true)
                                                    {

                                                        DataSet ds_sphrr = new DataSet();
                                                        string getsphr1 = "select sa.attendance,start_time from specialhr_details sd,specialhr_master sm,specialhr_attendance sa where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and date ='" + dumm_from_date.ToString() + "'  and sa.hrdet_no=sd.hrdet_no and sa.roll_no='" + ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString() + "' order by start_time ";
                                                        getsphr1 +=
                                    "select distinct start_time from specialhr_master sm ,specialhr_details sd where  sd.hrentry_no=sm.hrentry_no and date ='" + dumm_from_date.ToString() + "' and batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " order by start_time";

                                                        ds_sphrr = d2.select_method(getsphr1, hat, "Text");
                                                        if (ds_sphrr.Tables[1].Rows.Count > 0)
                                                        {


                                                            int sphr1 = 0;
                                                            for (int sphr2 = 0; sphr2 < ds_sphrr.Tables[1].Rows.Count; sphr2++)
                                                            {
                                                                if (sphr1 < ds_sphrr.Tables[0].Rows.Count)
                                                                {

                                                                    if (ds_sphrr.Tables[1].Rows[sphr2]["start_time"].ToString() == ds_sphrr.Tables[0].Rows[sphr1]["start_time"].ToString())
                                                                    {
                                                                        string value2 = ds_sphrr.Tables[0].Rows[sphr1]["attendance"].ToString();

                                                                        if (check == 1)
                                                                        {
                                                                            if (value2 != null && value2 != "0" && value2 != "7" && value2 != "")
                                                                            {
                                                                                present_mark(value2.ToString());
                                                                            }
                                                                            else
                                                                            {
                                                                                pp = "NE";
                                                                                perdats();
                                                                            }
                                                                        }
                                                                        sphr1++;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (check == 1)
                                                                        {
                                                                            dtrow[coln] = "-";
                                                                            coln++;
                                                                        }
                                                                    }


                                                                }
                                                                else
                                                                {
                                                                    if (check == 1)
                                                                    {
                                                                        dtrow[coln] = "-";
                                                                        coln++;
                                                                    }
                                                                }
                                                            }

                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (check == 1)
                                {
                                    string ddii = ds2.Tables[0].Rows[next]["month_year"].ToString();
                                    int ddiii = int.Parse(ddii.ToString());
                                    ddiii = ddiii - cal_from_date;
                                    if (ddiii != -1)
                                    {
                                        DateTime dumm_fdate = dumm_from_date.AddDays(1);
                                        while (dumm_from_date < (dumm_fdate))
                                        {
                                            if (dumm_from_date <= (per_to_date))
                                            {
                                                for (int count = 0; count < NoHrs; count++)
                                                {
                                                    if (dtadm <= dumm_from_date)
                                                    {
                                                        //Modified By Srinath 25/4/2013
                                                        string date = dumm_from_date.ToString();
                                                        string[] spiltdate1 = date.Split(' ');
                                                        string[] spiltdate = spiltdate1[0].Split('/');
                                                        if (holiday_table11.Contains(spiltdate[1] + '/' + spiltdate[0] + '/' + spiltdate[2]))
                                                        {
                                                            pp = "H";
                                                            perdats();
                                                        }
                                                        else
                                                        {
                                                            pp = "NE";
                                                            perdats();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        pp = "NJ";
                                                        perdats();
                                                    }
                                                }
                                                if (splhr_flag_head == true)
                                                {

                                                    DataSet ds_sphrr = new DataSet();
                                                    string getsphr1 = "select sa.attendance,start_time from specialhr_details sd,specialhr_master sm,specialhr_attendance sa where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and date ='" + dumm_from_date.ToString() + "'  and sa.hrdet_no=sd.hrdet_no and sa.roll_no='" + ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString() + "' order by start_time ";
                                                    getsphr1 +=
                                "select distinct start_time from specialhr_master sm ,specialhr_details sd where  sd.hrentry_no=sm.hrentry_no and date ='" + dumm_from_date.ToString() + "' and batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " order by start_time";

                                                    ds_sphrr = d2.select_method(getsphr1, hat, "Text");
                                                    if (ds_sphrr.Tables[1].Rows.Count > 0)
                                                    {


                                                        int sphr1 = 0;
                                                        for (int sphr2 = 0; sphr2 < ds_sphrr.Tables[1].Rows.Count; sphr2++)
                                                        {
                                                            if (sphr1 < ds_sphrr.Tables[0].Rows.Count)
                                                            {

                                                                if (ds_sphrr.Tables[1].Rows[sphr2]["start_time"].ToString() == ds_sphrr.Tables[0].Rows[sphr1]["start_time"].ToString())
                                                                {
                                                                    string value2 = ds_sphrr.Tables[0].Rows[sphr1]["attendance"].ToString();

                                                                    if (check == 1)
                                                                    {
                                                                        if (value2 != null && value2 != "0" && value2 != "7" && value2 != "")
                                                                        {
                                                                            present_mark(value2.ToString());
                                                                        }
                                                                        else
                                                                        {
                                                                            pp = "NE";
                                                                            perdats();
                                                                        }
                                                                    }
                                                                    sphr1++;
                                                                }
                                                                else
                                                                {
                                                                    if (check == 1)
                                                                    {
                                                                        dtrow[coln] = "-";
                                                                        coln++;
                                                                    }
                                                                }


                                                            }
                                                            else
                                                            {
                                                                if (check == 1)
                                                                {
                                                                    dtrow[coln] = "-";
                                                                    coln++;
                                                                }
                                                            }
                                                        }

                                                    }

                                                }
                                                dumm_from_date = dumm_from_date.AddDays(1);
                                            }
                                        }
                                        cal_from_date++;
                                    }
                                    else if (ddiii == -1)
                                    {
                                        while (dumm_from_date <= (per_to_date))
                                        {
                                            for (int count = 0; count < NoHrs; count++)
                                            {//Modified By Srinath 25/4/2013
                                                if (dtadm <= dumm_from_date)
                                                {
                                                    string date = dumm_from_date.ToString();
                                                    string[] spiltdate1 = date.Split(' ');
                                                    string[] spiltdate = spiltdate1[0].Split('/');
                                                    if (holiday_table11.Contains(spiltdate[1] + '/' + spiltdate[0] + '/' + spiltdate[2]))
                                                    {
                                                        pp = "H";
                                                        perdats();
                                                    }
                                                    else
                                                    {
                                                        pp = "NE";
                                                        perdats();
                                                    }
                                                }
                                                else
                                                {
                                                    pp = "NJ";
                                                    perdats();
                                                }
                                            }

                                            if (splhr_flag_head == true)
                                            {

                                                DataSet ds_sphrr = new DataSet();
                                                string getsphr1 = "select sa.attendance,start_time from specialhr_details sd,specialhr_master sm,specialhr_attendance sa where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and date ='" + dumm_from_date.ToString() + "'  and sa.hrdet_no=sd.hrdet_no and sa.roll_no='" + ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString() + "' order by start_time ";
                                                getsphr1 +=
                            "select distinct start_time from specialhr_master sm ,specialhr_details sd where  sd.hrentry_no=sm.hrentry_no and date ='" + dumm_from_date.ToString() + "' and batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " order by start_time";

                                                ds_sphrr = d2.select_method(getsphr1, hat, "Text");
                                                if (ds_sphrr.Tables[1].Rows.Count > 0)
                                                {


                                                    int sphr1 = 0;
                                                    for (int sphr2 = 0; sphr2 < ds_sphrr.Tables[1].Rows.Count; sphr2++)
                                                    {
                                                        if (sphr1 < ds_sphrr.Tables[0].Rows.Count)
                                                        {

                                                            if (ds_sphrr.Tables[1].Rows[sphr2]["start_time"].ToString() == ds_sphrr.Tables[0].Rows[sphr1]["start_time"].ToString())
                                                            {
                                                                string value2 = ds_sphrr.Tables[0].Rows[sphr1]["attendance"].ToString();

                                                                if (check == 1)
                                                                {
                                                                    if (value2 != null && value2 != "0" && value2 != "7" && value2 != "")
                                                                    {
                                                                        present_mark(value2.ToString());
                                                                    }
                                                                    else
                                                                    {
                                                                        pp = "NE";
                                                                        perdats();
                                                                    }
                                                                }
                                                                sphr1++;
                                                            }
                                                            else
                                                            {
                                                                if (check == 1)
                                                                {
                                                                    dtrow[coln] = "-";
                                                                    coln++;
                                                                }
                                                            }


                                                        }
                                                        else
                                                        {
                                                            if (check == 1)
                                                            {
                                                                dtrow[coln] = "-";
                                                                coln++;
                                                            }
                                                        }
                                                    }

                                                }

                                            }
                                            dumm_from_date = dumm_from_date.AddDays(1);
                                        }
                                    }
                                    if (ddiii == -1)
                                    {
                                        cal_from_date++;
                                        if (moncount > next)
                                        {
                                            next++;
                                            rcount++;
                                        }
                                    }
                                }
                                if (check == 2)
                                {
                                    string diii = ds2.Tables[0].Rows[next]["month_year"].ToString();
                                    int ddiiii = int.Parse(diii.ToString());
                                    ddiiii = ddiiii - cal_from_date;
                                    if (ddiiii == 1 || ddiiii == -1)
                                    {
                                        cal_from_date++;
                                        if (moncount > next)
                                        {
                                            next++;
                                            rcount++;
                                        }
                                    }
                                    //workingdays += 1;
                                    dumm_from_date = dumm_from_date.AddDays(1);
                                }
                                if (moncount > next)
                                {
                                    i--;
                                }
                            }
                        }
                    }
                }
                int diff_Date = per_from_date.Day - dumm_from_date.Day;
                dumm_from_date = dumm_from_date.AddDays(1);
            }
            else
            {
                if (check == 1)
                {
                    while (dumm_from_date <= (per_to_date))
                    {
                        for (int count = 0; count < NoHrs; count++)
                        {
                            if (dtadm <= dumm_from_date)
                            {
                                //Modified By Srinath 25/4/2013
                                string date = dumm_from_date.ToString();
                                string[] spiltdate1 = date.Split(' ');
                                string[] spiltdate = spiltdate1[0].Split('/');
                                if (holiday_table11.Contains(spiltdate[1] + '/' + spiltdate[0] + '/' + spiltdate[2]))
                                {
                                    pp = "H";
                                    perdats();
                                }
                                else
                                {
                                    pp = "NE";
                                    perdats();
                                }
                                //==========End
                            }
                            else
                            {
                                pp = "NJ";
                                perdats();
                            }
                        }
                        //ppppp

                        if (splhr_flag_head == true)
                        {

                            DataSet ds_sphrr = new DataSet();
                            string getsphr1 = "select sa.attendance,start_time from specialhr_details sd,specialhr_master sm,specialhr_attendance sa where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and date ='" + dumm_from_date.ToString() + "'  and sa.hrdet_no=sd.hrdet_no and sa.roll_no='" + ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString() + "' order by start_time ";
                            getsphr1 +=
        "select distinct start_time from specialhr_master sm ,specialhr_details sd where  sd.hrentry_no=sm.hrentry_no and date ='" + dumm_from_date.ToString() + "' and batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " order by start_time";

                            ds_sphrr = d2.select_method(getsphr1, hat, "Text");
                            if (ds_sphrr.Tables[1].Rows.Count > 0)
                            {


                                int sphr1 = 0;
                                for (int sphr2 = 0; sphr2 < ds_sphrr.Tables[1].Rows.Count; sphr2++)
                                {
                                    if (sphr1 < ds_sphrr.Tables[0].Rows.Count)
                                    {

                                        if (ds_sphrr.Tables[1].Rows[sphr2]["start_time"].ToString() == ds_sphrr.Tables[0].Rows[sphr1]["start_time"].ToString())
                                        {
                                            string value2 = ds_sphrr.Tables[0].Rows[sphr1]["attendance"].ToString();

                                            if (check == 1)
                                            {
                                                if (value2 != null && value2 != "0" && value2 != "7" && value2 != "")
                                                {
                                                    present_mark(value2.ToString());
                                                }
                                                else
                                                {
                                                    pp = "NE";
                                                    perdats();
                                                }
                                            }
                                            sphr1++;
                                        }
                                        else
                                        {
                                            if (check == 1)
                                            {
                                                dtrow[coln] = "-";
                                                coln++;
                                            }
                                        }


                                    }
                                    else
                                    {
                                        if (check == 1)
                                        {
                                            dtrow[coln] = "-";
                                            coln++;
                                        }
                                    }
                                }

                            }

                        }
                        dumm_from_date = dumm_from_date.AddDays(1);
                    }
                }
                else
                {
                    dumm_from_date = dumm_from_date.AddDays(1);
                }
            }
            if (check == 1)
            {
                per_tot_ondu = tot_ondu;
                per_njdate = njdate;
                pre_present_date = Present - njdate;
                per_per_hrs = tot_per_hrs;
                per_absent_date = Absent;
                pre_ondu_date = Onduty;
                pre_leave_date = Leave;
                //per_workingdays = workingdays - per_holidate - per_njdate;
                per_workingdays = workingdays - per_njdate;
                //  per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value; //added on 08.08.12,notconsider value added for hr suspension,09.08.12 mythili
                per_dum_unmark = dum_unmark;
                tot_per_abshrs = tot_abshrs;
                tot_per_leave = tot_leave;
            }
            if (check == 2)
            {
                cum_tot_ondu = tot_ondu;
                cum_njdate = njdate;
                cum_present_date = Present;
                cum_per_perhrs = tot_per_hrs;
                cum_absent_date = Absent;
                cum_ondu_date = Onduty;
                cum_leave_date = Leave;
                //cum_workingdays = workingdays - per_holidate - cum_njdate;
                cum_workingdays = workingdays - cum_njdate;   //added by Mullai
                cum_dum_unmark = dum_unmark;
                cum_tot_point = absent_point + leave_point;
            }
            tot_leave = 0;
            tot_abshrs = 0;
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
            next = 0;
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, Convert.ToString(Session["collegecode"]), "MothAttndReport");
        }
    }

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

    private void present_mark(string Attstr_mark)
    {
        switch (Attstr_mark)
        {
            case "1":
                pp = "P";
                break;
            case "2":
                pp = "A";
                break;
            case "3":
                pp = "OD";
                break;
            case "4":
                pp = "ML";
                break;
            case "5":
                pp = "SOD";
                break;
            case "6":
                pp = "NSS";
                break;
            case "7":
                pp = "H";
                break;
            case "8":
                pp = "NJ";
                break;
            case "9":
                pp = "S";
                break;
            case "10":
                pp = "L";
                break;
            case "11":
                pp = "NCC";
                break;
            case "12":
                pp = "HS";
                break;
            case "13":
                pp = "PP";
                break;
            case "14":
                pp = "SYOD";
                break;
            case "15":
                pp = "COD";
                break;
            case "16":
                pp = "OOD";
                break;
            case "17":
                pp = "LA";
                break;
        }
        Ihof = 0;
        IIhof = 0;
        holiday = 0;
        checknull = string.Empty;
        perdats();
    }

    private void year_fromat()
    {
        int demfcal, demtcal;
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
        per_from_date = Convert.ToDateTime(frdate);
        per_to_date = Convert.ToDateTime(todate);
        dumm_from_date = DateTime.Parse(frdate.ToString());
    }

    public void cumpresentdays()
    {
        if (cumcheck.Checked == true)
        {
            frdate = cumfromtxt.Text;
            todate = cumtotxt.Text;
            persentmonthcal();
        }
    }

    public string sem_roman(int sem)
    {
        string sql = string.Empty;
        string sem_roman = string.Empty;
        SqlDataReader rsChkSet;
        con1.Close();
        con1.Open();
        sql = "select * from inssettings where college_code=" + Session["collegecode"] + " and LinkName ='Semester Display'";
        SqlCommand cmd1 = new SqlCommand(sql, con1);
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

    //protected override void Render(System.Web.UI.HtmlTextWriter writer)
    //{
    //           Control cntPageNextBtn = FpSpread1.FindControl("Next");
    //    Control cntPagePreviousBtn = FpSpread1.FindControl("Prev");
    //    if ((cntPageNextBtn != null))
    //    {
    //        TableCell tc = (TableCell)cntPageNextBtn.Parent;
    //        TableRow tr = (TableRow)tc.Parent;
    //        tr.Cells.Remove(tc);
    //        tc = (TableCell)cntPagePreviousBtn.Parent;
    //        tr.Cells.Remove(tc);
    //    }
    //    base.Render(writer);
    //}

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
        //Control cntUpdateBtn = FpSpread1.FindControl("Update");
        //Control cntCancelBtn = FpSpread1.FindControl("Cancel");
        //Control cntCopyBtn = FpSpread1.FindControl("Copy");
        //Control cntCutBtn = FpSpread1.FindControl("Clear");
        //Control cntPasteBtn = FpSpread1.FindControl("Paste");
        //Control cntPageNextBtn = FpSpread1.FindControl("Next");
        //Control cntPagePreviousBtn = FpSpread1.FindControl("Prev");
        //if ((cntUpdateBtn != null))
        //{
        //    TableCell tc = (TableCell)cntUpdateBtn.Parent;
        //    TableRow tr = (TableRow)tc.Parent;
        //    tr.Cells.Remove(tc);
        //    tc = (TableCell)cntCancelBtn.Parent;
        //    tr.Cells.Remove(tc);
        //    //tc = (TableCell)cntCopyBtn.Parent;
        //    //tr.Cells.Remove(tc);
        //    //tc = (TableCell)cntCutBtn.Parent;
        //    //tr.Cells.Remove(tc);
        //    //tc = (TableCell)cntPasteBtn.Parent;
        //    //tr.Cells.Remove(tc);
        //    //tc = (TableCell)cntPageNextBtn.Parent;
        //    //tr.Cells.Remove(tc);
        //    //tc = (TableCell)cntPagePreviousBtn.Parent;
        //    //tr.Cells.Remove(tc);
        //}
        base.Render(writer);
    }

    public void find_attnd_values()
    {
        try
        {
            int nohrsprsentperday = 0;
            double noofdaypresen = 0;
            int hour_sus_count1 = 0, hour_sus_count2 = 0;
            DateTime prevdate = dumm_from_date.AddDays(-1);
            string[] prev_date_string = (prevdate.ToShortDateString()).Split('/');
            DateTime nextdate = dumm_from_date.AddDays(1);
            string[] next_date_string = (nextdate.ToShortDateString()).Split('/');
            if (holiday_table21.ContainsKey((next_date_string[1].ToString() + "/" + next_date_string[0].ToString() + "/" + next_date_string[2].ToString())))
            {
                dif_date = 1;
            }
            else if (holiday_table21.ContainsKey((prev_date_string[1].ToString() + "/" + prev_date_string[0].ToString() + "/" + prev_date_string[2].ToString())))
            {
                dif_date = -1;
            }
            else
            {
                ts = (DateTime.Parse(dumm_from_date.ToString())).Subtract(DateTime.Parse(dumm_from_date.ToString()));
            }
            if (dif_date == 1)
            {
                leave_pointer = holi_leav;
                absent_pointer = holi_absent;
                //rrowcount++;
            }
            else if (dif_date == -1)
            {
                leave_pointer = holi_leav;
                absent_pointer = holi_absent;
                if (ccount > rowcount)
                {
                    rowcount += 1;
                }
            }
            else
            {
                leave_pointer = leav_pt;
                absent_pointer = absent_pt;
            }
            if (ds3.Tables.Count > 2 && ds3.Tables[2].Rows.Count > 0)
            {
                nextdate = dumm_from_date.AddDays(1);
                next_date_string = (nextdate.ToShortDateString()).Split('/');
                if (holiday_table31.ContainsKey((next_date_string[1].ToString() + "/" + next_date_string[0].ToString() + "/" + next_date_string[2].ToString())))
                {
                    dif_date = 1;
                }
                if (dif_date == 1)
                {
                    leave_pointer = holi_leav;
                    absent_pointer = holi_absent;
                }
            }
            if (ds3.Tables.Count > 1 && ds3.Tables[1].Rows.Count > 0)
            {
                if (ds3count >= rrowcount)
                {
                    if (dumm_from_date == DateTime.Parse(ds3.Tables[1].Rows[rrowcount]["HOLI_DATE"].ToString()))
                    {
                        holiday = 0;
                        holiday = 1;
                        IIhof = 2;
                        Ihof = 2;
                        rrowcount++;
                    }
                }
            }
            if (dif_date1 == -1)
            {
                leave_pointer = holi_leav;
                absent_pointer = holi_absent;
            }
            if (dumm_from_date < DateTime.Parse(ds4.Tables[0].Rows[rows_count]["ADM_DATE"].ToString()))
            {
                if (cons_mng_start_hr == 1)
                {
                    for (i = 1; i <= fnhrs; i++)
                    {
                        value = "8";
                        pp = "NJ";
                        perdats();
                    }
                }
                if (cons_evng_start_hr == 1)
                {
                    for (i = fnhrs + 1; i <= NoHrs; i++)
                    {
                        value = "8";
                        pp = "NJ";
                        perdats();
                    }
                }
            }
            else
            {
                dif_date1 = 0;
                if (cons_mng_start_hr == 1)
                {
                    UnmarkHours = string.Empty;
                    CurrentDate = dumm_from_date.ToString("dd/MM/yyyy");
                    temp_unmark = 0;
                    for (i = 1; i <= fnhrs; i++)
                    {
                        date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                        value = ds2.Tables[0].Rows[next][date].ToString();
                        if (value == "0" || value == "" || value == null)//Rajkumar NEC
                        {
                            //string date1=dumm_from_date.ToString("DD/MM/YYYY");
                            UnmarkHours = UnmarkHours + "Date: " + CurrentDate + " " + "Hour: " + i.ToString() + ",";
                        }
                        //added by anandan for checking date
                        if (check == 1)
                        {
                            if (value != null && value != "0" && value != "7" && value != "")
                            {
                                present_mark(value.ToString());
                            }
                            else
                            {
                                pp = "NE";
                                perdats();
                            }
                        }
                        //if (value != null && value != "0" && value != "7" && value != "" && value != "12" && value != "8")//Rajkumar 6/1/2018
                        if (value != null && value != "0" && value != "" && value != "7")
                        {
                            if (check == 1)
                                tot_conducted_hours_count22++;
                            tot_conducted_hours_count++;
                            if (tempvalue != value)
                            {
                                tempvalue = value;
                                for (int j = 0; j < count; j++)
                                {
                                    if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                    {
                                        ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                        j = count;
                                    }
                                }
                            }
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].ForeColor = Color.Black;
                            if (ObtValue == 1)
                            {
                                per_abshrs += 1;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].ForeColor = Color.Red;
                            }
                            else if (ObtValue == 2)
                            {
                                njhr += 1;
                            }
                            else if (ObtValue == 0)
                            {
                                per_perhrs += 1;
                                tot_per_hrs += 1;
                                nohrsprsentperday++;
                            }
                            if (value == "3")
                            {
                                per_ondu += 1;
                                tot_ondu += 1;
                            }
                            if (value == "2")
                            {
                                tot_abshrs++;
                            }
                            else if (value == "10")
                            {
                                per_leave += 1;
                                tot_leave++;
                            }
                            //Rajkumar  6/1/2018
                            else if (value == "12")
                            {

                                tot_conducted_hours_count--;
                                if (check == 1)
                                    tot_conducted_hours_count22--;
                            }
                            //rajkumar
                        }
                        else if (value == "7")
                        {
                            per_hhday += 1;
                        }
                        else if (value == "12")
                        {
                            hour_sus_count1++;

                        }
                        else
                        {
                            temp_unmark++;
                            unmark += 1;
                        }
                    }
                    if (per_perhrs + njhr >= minpresI)
                    {
                        Present += 0.5;
                        Ihof = 0.5;
                        noofdaypresen = 0.5;
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
                    if (njhr >= minpresI)
                    {
                        njdate += 0.5;
                        Ihof = 1;
                    }
                    if (per_ondu >= 1)
                    {
                        Onduty += 0.5;
                    }
                    if (fnhrs - temp_unmark >= minpresI)
                    {
                        workingdays += 0.5;
                    }
                    per_perhrs = 0;//lllll
                    per_ondu = 0;
                    per_leave = 0;
                    per_abshrs = 0;
                }
                njhr = 0;
                temp_unmark = 0;
                int k = i;
                if (cons_evng_start_hr == 1)
                {
                    for (i = fnhrs + 1; i <= NoHrs; i++)
                    {
                        date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                        value = ds2.Tables[0].Rows[next][date].ToString();
                        if (value == "0" || value == "" || value == null)//Rajkumar NEC
                        {
                            //string date1=dumm_from_date.ToString("DD/MM/YYYY");
                            UnmarkHours = UnmarkHours + "Date: " + CurrentDate + " " + "Hour: " + i.ToString() + ",";
                        }
                        //added by anandan for checking date
                        if (dumm_from_date < DateTime.Parse(ds4.Tables[0].Rows[rows_count]["ADM_DATE"].ToString()))
                        {
                            value = "8";
                        }
                        if (check == 1)
                        {
                            if (value != null && value != "0" && value != "7" && value != "")
                            {
                                present_mark(value.ToString());
                            }
                            else
                            {
                                pp = "NE";
                                perdats();
                            }
                        }
                        //if (value != null && value != "0" && value != "7" && value != "" && value != "12" && value != "8")
                        if (value != null && value != "0" && value != "7" && value != "")
                        {
                            if (check == 1)
                                tot_conducted_hours_count22++;
                            tot_conducted_hours_count++;
                            if (tempvalue != value)
                            {
                                tempvalue = value;
                                for (int j = 0; j < count; j++)
                                {
                                    if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                    {
                                        ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                        j = count;
                                    }
                                }
                            }
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].ForeColor = Color.Black;
                            if (ObtValue == 1)
                            {
                                per_abshrs += 1;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].ForeColor = Color.Red;
                            }
                            else if (ObtValue == 2)
                            {
                                njhr += 1;
                            }
                            else if (ObtValue == 0)
                            {
                                per_perhrs += 1;
                                tot_per_hrs += 1;
                                nohrsprsentperday++;
                            }
                            if (value == "3")
                            {
                                per_ondu += 1;
                                tot_ondu += 1;
                            }
                            if (value == "2")
                            {
                                tot_abshrs++;
                            }
                            else if (value == "10")
                            {
                                per_leave += 1;
                                tot_leave++;
                            }
                            //Rajkumar  6/1/2018
                            else if (value == "12")
                            {
                                tot_conducted_hours_count--;
                                if (check == 1)
                                    tot_conducted_hours_count22--;
                            }
                            //rajkumar
                        }
                        else if (value == "7")
                        {
                            per_hhday += 1;
                        }
                        else
                        {
                            unmark += 1;
                            temp_unmark++;
                        }
                        if (value == "12")
                        {
                            hour_sus_count2++;

                        }
                    }

                    
                    if (per_perhrs + njhr >= minpresII)
                    {
                        Present += 0.5;
                        IIhof = 0.5;
                        noofdaypresen = noofdaypresen + 0.5;
                    }
                    else if (per_leave > 1)
                    {
                        leave_point += leave_pointer / 2;
                        Leave += 0.5;
                    }
                    else if (per_abshrs >= 1)
                    {
                        Absent += 0.5;
                        absent_point += absent_pointer / 2;
                    }
                    if (njhr >= minpresII)
                    {
                        IIhof = 1;
                        njdate += 0.5;
                    }
                    if (Session["attdaywisecla"].ToString() == "1")
                    {
                        if (nohrsprsentperday < minpresday)
                        {
                            Present = Present - noofdaypresen;
                            Absent = Absent + noofdaypresen;
                        }
                    }
                    nohrsprsentperday = 0;
                    noofdaypresen = 0;
                    if (unmark == NoHrs)
                    {
                        per_holidate += 1;
                        unmark = 0;
                    }
                    else
                    {
                        dum_unmark += unmark;
                    }
                    if (per_ondu >= 1)
                    {
                        Onduty += 0.5;
                    }
                    if ((NoHrs - fnhrs) - temp_unmark >= minpresII)
                    {
                        workingdays += 0.5;
                    }
                }
                

                if (splhr_flag_head == true)
                {

                    DataSet ds_sphrr = new DataSet();
                    string getsphr1 = "select sa.attendance,start_time from specialhr_details sd,specialhr_master sm,specialhr_attendance sa where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and date ='" + dumm_from_date.ToString() + "'  and sa.hrdet_no=sd.hrdet_no and sa.roll_no='" + ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString() + "' order by start_time ";
                    getsphr1 +=
"select distinct start_time from specialhr_master sm ,specialhr_details sd where  sd.hrentry_no=sm.hrentry_no and date ='" + dumm_from_date.ToString() + "' and batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " order by start_time"; 

                    ds_sphrr = d2.select_method(getsphr1, hat, "Text");
                    if (ds_sphrr.Tables[1].Rows.Count > 0)
                    {


                        int sphr1 = 0;
                            for (int sphr2 = 0; sphr2 < ds_sphrr.Tables[1].Rows.Count; sphr2++)
                            {
                                if (sphr1 < ds_sphrr.Tables[0].Rows.Count)
                                {

                                    if (ds_sphrr.Tables[1].Rows[sphr2]["start_time"].ToString() == ds_sphrr.Tables[0].Rows[sphr1]["start_time"].ToString())
                                    {
                                        string value2 = ds_sphrr.Tables[0].Rows[sphr1]["attendance"].ToString();

                                        if (check == 1)
                                        {
                                            if (value2 != null && value2 != "0" && value2 != "7" && value2 != "")
                                            {
                                                present_mark(value2.ToString());
                                            }
                                            else
                                            {
                                                pp = "NE";
                                                perdats();
                                            }
                                        }
                                        sphr1++;
                                    }
                                    else
                                    {
                                        if (check == 1)
                                        {
                                            dtrow[coln] = "-";
                                            coln++;
                                        }
                                    }

                                    
                                }
                                else
                                {
                                    if (check == 1)
                                    {
                                        dtrow[coln] = "-";
                                        coln++;
                                    }
                                }
                            }
                        
                    }

                }

            }
            if (!string.IsNullOrEmpty(UnmarkHours))
            {
                usercode = Session["usercode"].ToString().Trim();
                string alertRights = dirAcc.selectScalarString("select value from Master_Settings where settings='AlertMessageForAttendance' and usercode=' " + usercode + "'");
                string Noresult = UnmarkHours;
                if (alertRights == "1")
                {
                    //lblAlertMsg.Visible = true;
                    //lblAlertMsg.Text = Noresult + " " + " Attendance not Found";
                    //divPopAlert.Visible = true;
                }
                //return;
            }
            per_perhrs = 0;
            per_ondu = 0;
            per_leave = 0;
            per_abshrs = 0;
            unmark = 0;
            njhr = 0;
            if (check == 1)
            {
            }
            dumm_from_date = dumm_from_date.AddDays(1);
            if (dumm_from_date.Day == 1)
            {
                cal_from_date++;
                if (moncount > next)
                {
                    next++;
                    rcount++;
                }
            }
            // workingdays += 1;
            //if (hour_sus_count1 >= minpresI)
            //{
            //    workingdays -= 0.5;
            //}
            //if (hour_sus_count2 >= minpresII)
            //{
            //    workingdays -= 0.5;
            //}
            per_perhrs = 0;
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, Convert.ToString(Session["collegecode"]), "MonthAttndReport"); 
        }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        //Modified By Srinath 
        string reportname = txtexcelname.Text;
        if (reportname.ToString().Trim() != "")
        {
            
            d2.printexcelreportgrid(Showgrid, reportname);
            txtexcelname.Text = string.Empty;
        }
        else
        {
            norecordlbl.Text = "Please Enter Your Report Name";
            norecordlbl.Visible = true;
        }
        
    }

    protected void btn_print_setting_Click(object sender, EventArgs e)
    {
        //string batch = "", sections = "", semester = "", degreecode = "", subcolumntext = "", strsec = string.Empty;
        //Boolean child_flag = false;
        //int sec_index = 0, sem_index = 0;
        //batch = ddlbatch.SelectedValue.ToString();
        //sections = ddlsec.SelectedValue.ToString();
        //semester = ddlduration.SelectedValue.ToString();
        //degreecode = ddlbranch.SelectedValue.ToString();
        //if (ddlsec.Text == "")
        //{
        //    strsec = string.Empty;
        //}
        //else
        //{
        //    if (ddlsec.SelectedItem.ToString() == "")
        //    {
        //        strsec = string.Empty;
        //    }
        //    else
        //    {
        //        strsec = " - " + ddlsec.SelectedItem.ToString();
        //    }
        //}
        //if (ddlsec.Enabled == false)
        //{
        //    sec_index = -1;
        //}
        //else
        //{
        //    sec_index = ddlsec.SelectedIndex;
        //}
        //if (ddlduration.Enabled == false)
        //{
        //    sem_index = -1;
        //}
        //else
        //{
        //    sem_index = ddlduration.SelectedIndex;
        //}
        //if (cumcheck.Checked == false)
        //{
        //    Session["page_redirect_value"] = ddlbatch.SelectedIndex + "," + ddldegree.SelectedIndex + "," + ddlbranch.SelectedIndex + "," + sem_index + "," + sec_index + "," + txtFromDate.Text + "," + txtToDate.Text + "," + cumcheck.Checked;
        //}
        //else
        //{
        //    Session["page_redirect_value"] = ddlbatch.SelectedIndex + "," + ddldegree.SelectedIndex + "," + ddlbranch.SelectedIndex + "," + sem_index + "," + sec_index + "," + txtFromDate.Text + "," + txtToDate.Text + "," + cumcheck.Checked + "," + cumfromtxt.Text + "," + cumtotxt.Text + "," + pointchk.Checked;
        //}
        //btnGo_Click(sender, e);
        //lblpages.Visible = true;
        //ddlpage.Visible = true;
        //string clmnheadrname = string.Empty;
        //int total_clmn_count = FpSpread1.Sheets[0].ColumnCount;
        //for (int srtcnt = 0; srtcnt < total_clmn_count; srtcnt++)
        //{
        //    if (FpSpread1.Sheets[0].Columns[srtcnt].Visible == true)
        //    {
        //        if (FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text != "")
        //        {
        //            subcolumntext = string.Empty;
        //            if (clmnheadrname == "")
        //            {
        //                clmnheadrname = FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
        //            }
        //            else
        //            {
        //                if (child_flag == false)
        //                {
        //                    clmnheadrname = clmnheadrname + "," + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
        //                }
        //                else
        //                {
        //                    clmnheadrname = clmnheadrname + "$)," + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
        //                }
        //            }
        //            child_flag = false;
        //        }
        //        else
        //        {
        //            child_flag = true;
        //            if (subcolumntext == "")
        //            {
        //                for (int te = srtcnt - 1; te <= srtcnt; te++)
        //                {
        //                    if (te == srtcnt - 1)
        //                    {
        //                        clmnheadrname = clmnheadrname + "* ($" + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
        //                        subcolumntext = clmnheadrname + "* ($" + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
        //                    }
        //                    else
        //                    {
        //                        clmnheadrname = clmnheadrname + "$" + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
        //                        subcolumntext = clmnheadrname + "$" + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                subcolumntext = subcolumntext + "$" + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
        //                clmnheadrname = clmnheadrname + "$" + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
        //            }
        //        }
        //    }
        //}
        //Session["redirect_query_string"] = clmnheadrname.ToString() + ":" + "monthattndreport.aspx" + ":" + ddlbatch.SelectedItem.ToString() + " Batch - " + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] " + strsec + " :" + "Students Attendance Report";
        //Response.Redirect("Print_Master_Setting_new.aspx");//?ID=" + clmnheadrname.ToString() + ":" + "monthattndreport.aspx" + ":" + ddlbatch.SelectedItem.ToString() + " Batch - " + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] " + strsec + " :" + "Monthly Students Attendance Report");
    }

    public void print_btngo()
    {
        //final_print_col_cnt = 0;
        //norecordlbl.Visible = false;
        //check_col_count_flag = false;
        ////FpSpread1.Sheets[0].SheetCorner.Cells[5, 0].BackColor = Color.Red;
        //main_fun();
        ////string ffffds = FpSpread1.Sheets[0].ColumnHeader.Cells[8, 1].Text;
        //hat.Clear();
        //hat.Add("college_code", Session["collegecode"].ToString());
        //hat.Add("form_name", "monthattndreport.aspx");
        //dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        ////9999999999999999999999999999999999999999999999999999999
        //if (dsprint.Tables[0].Rows.Count > 0)
        //{
        //    string split_header_new = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
        //    string[] vat = split_header_new.Split(new char[] { ',' });
        //    for (int yp = 0; yp <= vat.GetUpperBound(0); yp++)
        //    {
        //        tf += 1;
        //    }
        //    lblpages.Visible = true;
        //    ddlpage.Visible = true;
        //    column_field = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------------get column field
        //    if (column_field != "" && column_field != null)
        //    {
        //        check_col_count_flag = true;
        //        for (col_count_all = 0; col_count_all < FpSpread1.Sheets[0].ColumnCount; col_count_all++)
        //        {
        //            FpSpread1.Sheets[0].Columns[col_count_all].Visible = false;//------------invisible all column                                
        //        }
        //        printvar = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------visible setting columns
        //        string[] split_printvar = printvar.Split(',');
        //        for (int splval = 0; splval <= split_printvar.GetUpperBound(0); splval++)
        //        {
        //            span_cnt = 0;
        //            string[] split_star = split_printvar[splval].Split('*');
        //            if (split_star.GetUpperBound(0) > 0)
        //            {
        //                for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
        //                {
        //                    if (FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), col_count].Text
        //                        == split_star[0])
        //                    {
        //                        string[] split_star_doller = split_star[1].Split('$');
        //                        //   int temp_child = 1;
        //                        for (int y = 1; y < split_star_doller.GetUpperBound(0); y++)
        //                        {
        //                            if (FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), col_count].Text ==
        //                                      split_star_doller[y])
        //                            {
        //                                FpSpread1.Sheets[0].Columns[col_count].Visible = true;
        //                            }
        //                            col_count++;
        //                        }
        //                    }
        //                }
        //            }
        //            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //            else
        //            {
        //                for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
        //                {
        //                    if (FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), col_count].Text == split_printvar[splval])
        //                    {
        //                        FpSpread1.Sheets[0].Columns[col_count].Visible = true;
        //                        final_print_col_cnt++;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        //2.Footer setting
        //        if (dsprint.Tables[0].Rows[0]["footer"].ToString() != null && dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
        //        {
        //            footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());
        //            footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
        //            string[] footer_text_split = footer_text.Split(',');
        //            footer_text = string.Empty;
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
        //                for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
        //                {
        //                    if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
        //                    {
        //                        break;
        //                    }
        //                }
        //            }
        //            else if (final_print_col_cnt == footer_count)
        //            {
        //                for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
        //                {
        //                    if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
        //                    {
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
        //                for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
        //                {
        //                    if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
        //                    {
        //                        if (temp_count == 0)
        //                        {
        //                            //FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer + footer_balanc_col);
        //                        }
        //                        else
        //                        {
        //                            //FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer);
        //                        }
        //                        //FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
        //                        if (col_count - 1 >= 0)
        //                        {
        //                            //FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorLeft = Color.White;
        //                            //FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorRight = Color.White;
        //                        }
        //                        FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
        //                        FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
        //                        if (col_count + 1 < FpSpread1.Sheets[0].ColumnCount)
        //                        {
        //                            //FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorLeft = Color.White;
        //                            //FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorRight = Color.White;
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
        //        ///////////+++++++++++++++++++++++++
        //        int rrr = 0, rt = 0, ty = 0, mn = 0, nb = 0;
        //        for (int yy = 0; yy < FpSpread1.Sheets[0].ColumnCount; yy++)
        //        {
        //            if (FpSpread1.Sheets[0].Columns[yy].Visible == true)
        //            {
        //                rrr += 1;
        //                if (mn == 0)
        //                {
        //                    mn = yy;
        //                }
        //                if (nb == 1)
        //                {
        //                    rt = yy;
        //                }
        //                ty = yy;
        //                mn++;
        //                nb++;
        //            }
        //        }
        //        int fkm = rrr;
        //        if (fkm <= 2)
        //        {
        //            FpSpread1.Width = 300;
        //            FpSpread1.Sheets[0].SheetCorner.Columns[0].Width = 70;
        //        }
        //        else if (fkm < 5)
        //        {
        //            FpSpread1.Width = 500;
        //            FpSpread1.Sheets[0].SheetCorner.Columns[0].Width = 70;
        //        }
        //        else if (fkm < 10)
        //        {
        //            FpSpread1.Width = 700;
        //            FpSpread1.Sheets[0].SheetCorner.Columns[0].Width = 70;
        //        }
        //        else if (fkm < 15)
        //        {
        //            FpSpread1.Width = 900;
        //            FpSpread1.Sheets[0].SheetCorner.Columns[0].Width = 70;
        //        }
        //        else if (fkm < 20)
        //        {
        //            FpSpread1.Width = 1000;
        //            FpSpread1.Sheets[0].SheetCorner.Columns[0].Width = 70;
        //        }
        //        else if (fkm < 25)
        //        {
        //            FpSpread1.Width = 1200;
        //            FpSpread1.Sheets[0].SheetCorner.Columns[0].Width = 70;
        //        }
        //        else if (fkm < 30)
        //        {
        //            FpSpread1.Width = 1300;
        //            FpSpread1.Sheets[0].SheetCorner.Columns[0].Width = 70;
        //        }
        //        else if (fkm < 35)
        //        {
        //            FpSpread1.Width = 1350;
        //            FpSpread1.Sheets[0].SheetCorner.Columns[0].Width = 70;
        //        }
        //        else if (32 < fkm)
        //        {
        //            FpSpread1.Width = 1350;
        //            FpSpread1.Sheets[0].SheetCorner.Columns[0].Width = 70;
        //        }
        //        //++++++++++++raja+++++++++.
        //        FpSpread1.Sheets[0].ColumnHeader.RowCount = 0;
        //        FpSpread1.Sheets[0].ColumnHeader.RowCount = 10 + tf;
        //        int dp = 0;
        //        int np = 0;
        //        int flag_new = 0;
        //        int temp_var = 0;
        //        int yop = 0;
        //        int[] f = new int[200];
        //        int inn = 0;
        //        int h = 0;
        //        for (h = 0; h < FpSpread1.Sheets[0].ColumnCount; h++)
        //        {
        //            if (FpSpread1.Sheets[0].Columns[h].Visible == true)
        //            {
        //                f[inn] = h;
        //                inn++;
        //            }
        //        }
        //        int hh = 0;
        //        int start = 0, colcount = 0;
        //        for (np = 0; np <= split_printvar.GetUpperBound(0); np++)
        //        {
        //            h = f[hh];
        //            string[] split_star1 = split_printvar[np].Split('*');
        //            if (split_star1.GetUpperBound(0) > 0)
        //            {
        //                flag_new = 0;
        //                for (int d = 0; d < split_star1.GetUpperBound(0); d++)
        //                {
        //                    temp_var = 0;
        //                    string[] split_star_doller = split_star1[1].Split('$');
        //                    int n = 1;
        //                    for (n = 1; n < split_star_doller.GetUpperBound(0); n++)
        //                    {
        //                        if (n > 1)
        //                            hh++;
        //                        dp = f[hh];
        //                        if (start == 0)
        //                        {
        //                            colcount = dp;
        //                        }
        //                        start++;
        //                        if (n == 1)
        //                            flag_new = dp;
        //                        temp_var++;
        //                        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, dp].Text = split_star_doller[n].ToString();
        //                        FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, dp].HorizontalAlign = HorizontalAlign.Center;
        //                    }
        //                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, flag_new, 1, n - 1);
        //                    FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, flag_new].Text = split_star1[0].ToString();
        //                    FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, flag_new].HorizontalAlign = HorizontalAlign.Center;
        //                }
        //            }
        //            else
        //            {
        //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, h, 2, 1);
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, h].Text = split_star1[0].ToString();
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, h].HorizontalAlign = HorizontalAlign.Center;
        //                if (start == 0)
        //                {
        //                    colcount = h;
        //                }
        //                start++;
        //            }
        //            hh++;
        //        }
        //        FpSpread1.Sheets[0].SheetName = " ";
        //        style.Font.Size = 12;
        //        style.Font.Bold = true;
        //        style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        //        style.ForeColor = Color.Black;
        //        FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
        //        FpSpread1.Sheets[0].RowHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
        //        FpSpread1.ActiveSheetView.RowHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
        //        FpSpread1.Sheets[0].AllowTableCorner = true;
        //        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        //        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        //        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        //        FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        //        FpSpread1.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
        //        SqlConnection con_header = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        //        MyImg mi = new MyImg();
        //        mi.ImageUrl = "~/images/10BIT001.jpeg";
        //        mi.ImageUrl = "Handler/Handler2.ashx?";
        //        MyImg mi2 = new MyImg();
        //        mi2.ImageUrl = "~/images/10BIT001.jpeg";
        //        mi2.ImageUrl = "Handler/Handler5.ashx?";
        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, colcount, FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, 1);
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, colcount].CellType = mi;
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, colcount].HorizontalAlign = HorizontalAlign.Center;
        //        if (ty == 0)
        //        {
        //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, rt, 1, ty + 1);
        //        }
        //        else
        //        {
        //            if (ty != rt)
        //            {
        //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, rt, 1, ty - rt);
        //            }
        //            else
        //            {
        //            }
        //        }
        //        if (dsprint.Tables[0].Rows[0]["college_name"].ToString() != "")
        //        {
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, rt].Text = dsprint.Tables[0].Rows[0]["college_name"].ToString();
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, rt].Font.Bold = true;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, rt].HorizontalAlign = HorizontalAlign.Center;
        //        }
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, rt].Border.BorderColorBottom = Color.White;
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, colcount].Border.BorderColorRight = Color.White;
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, rt].Border.BorderColorRight = Color.White;
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[1, rt].Border.BorderColorRight = Color.White;
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[2, rt].Border.BorderColorRight = Color.White;
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[3, rt].Border.BorderColorRight = Color.White;
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[4, rt].Border.BorderColorRight = Color.White;
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[5, rt].Border.BorderColorRight = Color.White;
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[6, rt].Border.BorderColorRight = Color.White;
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[7, rt].Border.BorderColorRight = Color.White;
        //        if (ty == 0)
        //        {
        //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, rt, 1, ty + 1);
        //        }
        //        else
        //        {
        //            if (ty != rt)
        //            {
        //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, rt, 1, ty - rt);
        //            }
        //        }
        //        if (dsprint.Tables[0].Rows[0]["address1"].ToString() != "" || dsprint.Tables[0].Rows[0]["address2"].ToString() != "" || dsprint.Tables[0].Rows[0]["address3"].ToString() != "")
        //        {
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, rt].Text = dsprint.Tables[0].Rows[0]["address1"].ToString() + "," + dsprint.Tables[0].Rows[0]["address2"].ToString() + "," + dsprint.Tables[0].Rows[0]["address3"].ToString();
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, rt].Font.Bold = true;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, rt].HorizontalAlign = HorizontalAlign.Center;
        //        }
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[1, rt].Border.BorderColorBottom = Color.White;
        //        if (ty == 0)
        //        {
        //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, rt, 1, ty + 1);
        //        }
        //        else
        //        {
        //            if (ty != rt)
        //            {
        //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, rt, 1, ty - rt);
        //            }
        //        }
        //        if (dsprint.Tables[0].Rows[0]["phoneno"].ToString() != "" || dsprint.Tables[0].Rows[0]["faxno"].ToString() != "")
        //        {
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, rt].Text = "Phone:" + dsprint.Tables[0].Rows[0]["phoneno"].ToString() + "Fax:" + dsprint.Tables[0].Rows[0]["faxno"].ToString();
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, rt].Font.Bold = true;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, rt].HorizontalAlign = HorizontalAlign.Center;
        //        }
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[2, rt].Border.BorderColorBottom = Color.White;
        //        if (ty == 0)
        //        {
        //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, rt, 1, ty + 1);
        //        }
        //        else
        //        {
        //            if (ty != rt)
        //            {
        //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, rt, 1, ty - rt);
        //            }
        //        }
        //        if (dsprint.Tables[0].Rows[0]["email"].ToString() != "" || dsprint.Tables[0].Rows[0]["website"].ToString() != "")
        //        {
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, rt].Text = "Email:" + dsprint.Tables[0].Rows[0]["email"].ToString() + "Web Site:" + dsprint.Tables[0].Rows[0]["website"].ToString();
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, rt].Font.Bold = true;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, rt].HorizontalAlign = HorizontalAlign.Center;
        //        }
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[3, rt].Border.BorderColorBottom = Color.White;
        //        if (ty == 0)
        //        {
        //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(4, rt, 1, ty + 1);
        //        }
        //        else
        //        {
        //            if (ty != rt)
        //            {
        //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(4, rt, 1, ty - rt);
        //            }
        //        }
        //        if (dsprint.Tables[0].Rows[0]["form_heading_name"].ToString() != "")
        //        {
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, rt].Text = dsprint.Tables[0].Rows[0]["form_heading_name"].ToString();
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, rt].Font.Bold = true;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, rt].HorizontalAlign = HorizontalAlign.Center;
        //        }
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[4, rt].Border.BorderColorBottom = Color.White;
        //        if (ty == 0)
        //        {
        //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(5, rt, 1, ty + 1);
        //        }
        //        else
        //        {
        //            if (ty != rt)
        //            {
        //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(5, rt, 1, ty - rt);
        //            }
        //        }
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[5, rt].Text = "- - - - - - - - - - - - - - - - - - ";
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[5, rt].HorizontalAlign = HorizontalAlign.Center;
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[5, rt].Border.BorderColorBottom = Color.White;
        //        if (ty == 0)
        //        {
        //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, rt, 1, ty + 1);
        //        }
        //        else
        //        {
        //            if (ty != rt)
        //            {
        //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, rt, 1, ty - rt);
        //            }
        //        }
        //        if (dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString() != "")
        //        {
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, rt].Text = dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString();
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, rt].Font.Bold = true;
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, rt].HorizontalAlign = HorizontalAlign.Center;
        //        }
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[6, rt].Border.BorderColorBottom = Color.White;
        //        if (ty == 0)
        //        {
        //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(7, rt, 1, ty + 1);
        //        }
        //        else
        //        {
        //            if (ty != rt)
        //            {
        //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(7, rt, 1, ty - rt);
        //            }
        //        }
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[7, rt].Text = "From:" + txtFromDate.Text + "  To:" + txtToDate.Text + "  Date:" + DateTime.Now.ToString("dd/MM/yyyy");
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[7, rt].Font.Bold = true;
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[7, rt].HorizontalAlign = HorizontalAlign.Center;
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[7, rt].Border.BorderColorBottom = Color.White;
        //        string fop = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
        //        string[] fvc = fop.Split(new char[] { ',' });
        //        int fqq = 8;
        //        for (int yx = 0; yx <= fvc.GetUpperBound(0); yx++)
        //        {
        //            if (ty == 0)
        //            {
        //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(fqq, rt, 1, ty + 1);
        //            }
        //            else
        //            {
        //                if (ty != rt)
        //                {
        //                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(fqq, rt, 1, ty - rt);
        //                }
        //            }
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[fqq, rt].Text = fvc[yx].ToString();
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[fqq, rt].Font.Bold = true;
        //            if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Center")
        //            {
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[fqq, rt].HorizontalAlign = HorizontalAlign.Center;
        //            }
        //            else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Right")
        //            {
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[fqq, rt].HorizontalAlign = HorizontalAlign.Right;
        //            }
        //            else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Left")
        //            {
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[fqq, rt].HorizontalAlign = HorizontalAlign.Left;
        //            }
        //            if (yx < fvc.GetUpperBound(0))
        //            {
        //                FpSpread1.Sheets[0].ColumnHeader.Cells[fqq, rt].Border.BorderColorBottom = Color.White;
        //            }
        //            FpSpread1.Sheets[0].ColumnHeader.Cells[fqq, rt].Border.BorderColorRight = Color.White;
        //            fqq++;
        //        }
        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, ty, FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, 1);
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, ty].CellType = mi2;
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, ty].HorizontalAlign = HorizontalAlign.Center;
        //        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        //        int r = 0;
        //        for (int b = 0; b < 200; b++)
        //        {
        //            if (f[b] != 0)
        //            {
        //                r = r + 1;
        //            }
        //        }
        //        int[] dd = new int[r];
        //        for (int hj = 0; hj < r; hj++)
        //        {
        //            dd[hj] = f[hj];
        //        }
        //        string dexs = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
        //        string[] split_footer = dexs.Split(new char[] { ',' });
        //        int az = 0;
        //        if (dsprint.Tables[0].Rows[0]["footer"].ToString() != string.Empty)
        //        {
        //            int fotcnt = Convert.ToInt32(dsprint.Tables[0].Rows[0]["footer"].ToString());
        //            int colcountt = dd.GetUpperBound(0) + 1;
        //            // if (colcountt >= fotcnt)
        //            {
        //                if (colcountt >= fotcnt)
        //                {
        //                    az = colcountt / fotcnt;
        //                }
        //                int nd = 0;
        //                for (int t = 0; t < FpSpread1.Sheets[0].ColumnCount - 1; t++)
        //                {
        //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, t].Border.BorderColorRight = Color.White;
        //                }
        //                int yd = 0;
        //                for (int y = 0; y < FpSpread1.Sheets[0].ColumnCount - 1; y = y + az)
        //                {
        //                    if (nd <= split_footer.GetUpperBound(0))
        //                    {
        //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, dd[y]].Text = split_footer[nd].ToString();
        //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, dd[y]].HorizontalAlign = HorizontalAlign.Center;
        //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, dd[y]].VerticalAlign = VerticalAlign.Bottom;
        //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, dd[y]].Font.Bold = true;
        //                    }
        //                    nd++;
        //                    yd++;
        //                }
        //            }
        //            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Border.BorderColor = Color.White;
        //        }
        //    }
        //    else
        //    {
        //        lblpages.Visible = false;
        //        ddlpage.Visible = false;
        //        norecordlbl.Visible = true;
        //        norecordlbl.Text = "Select Atleast One Column Field From The Treeview";
        //    }
        //}

    }

    public void view_header_setting()
    {
        //if (dsprint.Tables[0].Rows.Count > 0)
        //{
        //    //ddlpage.Visible = true;
        //    //lblpages.Visible = true;
        //    view_footer = dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString();
        //    view_header = dsprint.Tables[0].Rows[0]["header_flag_value"].ToString();
        //    view_footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
        //    if (view_header == "0" || view_header == "1")
        //    {
        //        errmsg.Visible = false;
        //        for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
        //        {
        //            FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
        //        }
        //        int i = 0;
        //        ddlpage_new.Items.Clear();
        //        int totrowcount = FpSpread1.Sheets[0].RowCount;
        //        int pages = totrowcount / 25;
        //        int intialrow = 1;
        //        int remainrows = totrowcount % 25;
        //        if (FpSpread1.Sheets[0].RowCount > 0)
        //        {
        //            int i5 = 0;
        //            ddlpage_new.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
        //            for (i = 1; i <= pages; i++)
        //            {
        //                i5 = i;
        //                ddlpage_new.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
        //                intialrow = intialrow + 25;
        //            }
        //            if (remainrows > 0)
        //            {
        //                i = i5 + 1;
        //                ddlpage_new.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
        //            }
        //        }
        //        if ((ddlpage_new.SelectedValue.ToString() == string.Empty) || (ddlpage_new.SelectedValue.ToString() == "0"))
        //        {
        //            for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
        //            {
        //                FpSpread1.Sheets[0].Rows[i].Visible = true;
        //            }
        //            Double totalRows = 0;
        //            totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
        //            Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);
        //            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        //            DropDownListpage.Items.Clear();
        //            if (totalRows >= 10)
        //            {
        //                FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
        //                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
        //                {
        //                    DropDownListpage.Items.Add((k + 10).ToString());
        //                }
        //                DropDownListpage.Items.Add("Others");
        //                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
        //                FpSpread1.Height = 335;
        //            }
        //            else if (totalRows == 0)
        //            {
        //                DropDownListpage.Items.Add("0");
        //                FpSpread1.Height = 100;
        //            }
        //            else
        //            {
        //                FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
        //                DropDownListpage.Items.Add(FpSpread1.Sheets[0].PageSize.ToString());
        //                FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
        //            }
        //            if (Convert.ToInt32(FpSpread1.Sheets[0].RowCount) > 10)
        //            {
        //                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
        //                FpSpread1.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
        //                FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
        //                CalculateTotalPages();
        //            }
        //            //setpanel.Visible = true;
        //        }
        //        else
        //        {
        //            errmsg.Visible = false;
        //            //setpanel.Visible = false;
        //        }
        //    }
        //    else if (view_header == "2")
        //    {
        //        for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
        //        {
        //            FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
        //        }
        //        errmsg.Visible = false;
        //        int i = 0;
        //        ddlpage_new.Items.Clear();
        //        int totrowcount = FpSpread1.Sheets[0].RowCount;
        //        int pages = totrowcount / 25;
        //        int intialrow = 1;
        //        int remainrows = totrowcount % 25;
        //        if (FpSpread1.Sheets[0].RowCount > 0)
        //        {
        //            int i5 = 0;
        //            ddlpage_new.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
        //            for (i = 1; i <= pages; i++)
        //            {
        //                i5 = i;
        //                ddlpage_new.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
        //                intialrow = intialrow + 25;
        //            }
        //            if (remainrows > 0)
        //            {
        //                i = i5 + 1;
        //                ddlpage_new.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
        //            }
        //        }
        //        if ((ddlpage_new.SelectedValue.ToString() == string.Empty) || (ddlpage_new.SelectedValue.ToString() == "0"))
        //        {
        //            for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
        //            {
        //                FpSpread1.Sheets[0].Rows[i].Visible = true;
        //            }
        //            Double totalRows = 0;
        //            totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
        //            Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);
        //            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        //            DropDownListpage.Items.Clear();
        //            if (totalRows >= 10)
        //            {
        //                FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
        //                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
        //                {
        //                    DropDownListpage.Items.Add((k + 10).ToString());
        //                }
        //                DropDownListpage.Items.Add("Others");
        //                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
        //                FpSpread1.Height = 335;
        //            }
        //            else if (totalRows == 0)
        //            {
        //                DropDownListpage.Items.Add("0");
        //                FpSpread1.Height = 100;
        //            }
        //            else
        //            {
        //                FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
        //                DropDownListpage.Items.Add(FpSpread1.Sheets[0].PageSize.ToString());
        //                FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
        //            }
        //            if (Convert.ToInt32(FpSpread1.Sheets[0].RowCount) > 10)
        //            {
        //                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
        //                FpSpread1.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
        //                //  subject_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
        //                CalculateTotalPages();
        //            }
        //            //setpanel.Visible = true;
        //        }
        //        else
        //        {
        //            //setpanel.Visible = false;
        //        }
        //    }
        //    else
        //    {
        //    }
        //}
    }

    public void getspecial_hr()
    {
        //added By Srinath 22/2/2013
        string hrdetno = string.Empty;
        if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
        {
            hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(dumm_from_date), ht_sphr));
        }
        if (hrdetno != "")
        {
            con_splhr_query_master.Close();
            con_splhr_query_master.Open();
            DataSet ds_splhr_query_master = new DataSet();
            //  no_stud_flag = false;
            //modified By Srinath 22/2/2013
            //string splhr_query_master = "select attendance from specialhr_attendance sa,registration r where hrdet_no in(select hrdet_no from specialhr_details where hrentry_no=(select hrentry_no from  specialhr_master where batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and date='" + dumm_from_date + "')  ) and r.roll_no=sa.roll_no and batch_year=" + ddlbatch.SelectedValue.ToString() + " and current_semester=" + ddlduration.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + "  and (CC = 0)  AND (DelFlag = 0)  AND (Exam_Flag <> 'debar') and sa.roll_no='" + ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString() + "'  order by r.roll_no asc";
            string splhr_query_master = "select attendance from specialhr_attendance where roll_no= '" + ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString() + "' and hrdet_no in(" + hrdetno + ")";
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
                            for (int j = 0; j < count; j++)
                            {
                                if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
                                {
                                    ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
                                    j = count;
                                }
                            }
                        }
                        if (ObtValue == 1)
                        {
                            per_abshrs_spl += 1;
                        }
                        else if (ObtValue == 2)
                        {
                            //       notconsider_value += 1;
                            njhr += 1;
                        }
                        else if (ObtValue == 0)
                        {
                            tot_per_hrs_spl += 1;
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
                       // tot_conduct_hr_spl--;
                       //tot_per_hrs_spl--;
                    }
                }
            }
        }
    }

    //public void getspecial_hr()
    //{
    //    con_splhr_query_master.Close();
    //    con_splhr_query_master.Open();
    //    DataSet ds_splhr_query_master = new DataSet();
    //    //  no_stud_flag = false;
    //    string splhr_query_master = "select attendance from specialhr_attendance sa,registration r where hrdet_no in(select hrdet_no from specialhr_details where hrentry_no=(select hrentry_no from  specialhr_master where batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and date='" + dumm_from_date + "')  ) and r.roll_no=sa.roll_no and batch_year=" + ddlbatch.SelectedValue.ToString() + " and current_semester=" + ddlduration.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + "  and (CC = 0)  AND (DelFlag = 0)  AND (Exam_Flag <> 'debar') and sa.roll_no='" + ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString() + "'  order by r.roll_no asc";
    //    SqlDataReader dr_splhr_query_master;
    //    cmd = new SqlCommand(splhr_query_master, con_splhr_query_master);
    //    dr_splhr_query_master = cmd.ExecuteReader();
    //    while (dr_splhr_query_master.Read())
    //    {
    //        if (dr_splhr_query_master.HasRows)
    //        {
    //            value = dr_splhr_query_master[0].ToString();
    //            if (value != null && value != "0" && value != "7" && value != "")
    //            {
    //                if (tempvalue != value)
    //                {
    //                    tempvalue = value;
    //                    for (int j = 0; j < count; j++)
    //                    {
    //                        if (ds1.Tables[0].Rows[j]["LeaveCode"].ToString() == value.ToString())
    //                        {
    //                            ObtValue = int.Parse(ds1.Tables[0].Rows[j]["CalcFlag"].ToString());
    //                            j = count;
    //                        }
    //                    }
    //                }
    //                if (ObtValue == 1)
    //                {
    //                    per_abshrs_spl += 1;
    //                }
    //                else if (ObtValue == 2)
    //                {
    //                    //       notconsider_value += 1;
    //                    njhr += 1;
    //                }
    //                else if (ObtValue == 0)
    //                {
    //                    tot_per_hrs_spl += 1;
    //                }
    //                if (value == "3")
    //                {
    //                    tot_ondu_spl += 1;
    //                }
    //                else if (value == "10")
    //                {
    //                    per_leave += 1;
    //                }
    //                tot_conduct_hr_spl++;
    //            }
    //            else if (value == "7")
    //            {
    //                per_hhday_spl += 1;
    //                tot_conduct_hr_spl--;
    //            }
    //            else
    //            {
    //                unmark_spl += 1;
    //                tot_conduct_hr_spl--;
    //            }
    //        }
    //    }
    //}
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Session["column_header_row_count"] = 2;
        string sections = ddlsec.SelectedValue.ToString().Trim();
        if (sections.ToString().Trim().ToLower() == "all" || sections.ToString().Trim().ToLower() == string.Empty || sections.ToString().Trim().ToLower() == "-1")
        {
            sections = string.Empty;
        }
        else
        {
            sections = "- Sec-" + sections + "";
        }
        string degreedetails = "Student Attendance Report" + '@' + ((!forschoolsetting) ? "Degree :" : "Standard : ") + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '[' + ddlbranch.SelectedItem.ToString() + ']' + '-' + ((!forschoolsetting) ? "Sem -" : "Term -") + ddlduration.SelectedItem.ToString() + sections + '@' + "Date :" + txtFromDate.Text.ToString() + " To " + txtToDate.Text.ToString();
        string pagename = "monthattndreport.aspx";
        //Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        string ss = null;
        Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
        ////Printcontrol.loadspreaddetails(attnd_report, pagename, degreedetails);
        Printcontrol.Visible = true;
       
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

    
    private static void AddTableColumn(DataTable resultsTable, StringBuilder ColumnName)
    {
        try
        {
            DataColumn tableCol = new DataColumn(ColumnName.ToString());
            resultsTable.Columns.Add(tableCol);
        }
        catch (System.Data.DuplicateNameException)
        {
            ColumnName.Append(" ");
            AddTableColumn(resultsTable, ColumnName);
        }
    }


    public void btnPrint11()
    {
        DAccess2 d2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = d2.select_method_wo_parameter(colQ, "Text");
        string collegeName = string.Empty;
        string collegeCateg = string.Empty;
        string collegeAff = string.Empty;
        string collegeAdd = string.Empty;
        string collegePhone = string.Empty;
        string collegeFax = string.Empty;
        string collegeWeb = string.Empty;
        string collegeEmai = string.Empty;
        string collegePin = string.Empty;
        string acr = string.Empty;
        string City = string.Empty;
        if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
        {
            collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
            City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
            collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
            collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
            collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
            collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
            collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
            collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
        }
        DateTime dt = DateTime.Now;
        int year = dt.Year;
        spCollegeName.InnerHtml = collegeName;
        spAddr.InnerHtml = collegeAdd;
        spDegreeName.InnerHtml = acr;
        spReportName.InnerHtml = "Monthly Student Attendance Report";
        


    }


    public override void VerifyRenderingInServerForm(Control control)
    { }
}
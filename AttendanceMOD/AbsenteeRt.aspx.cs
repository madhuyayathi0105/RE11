//==========MANIPRABHA A.
using System;//=====================================modified on 7/1/12, 2/2/12, 6/2/12 ,8/2/12, 14/2/12, 29/2/12(border,spread width)
//========10/03/12(hide print setting btn, show header setting radio btn, set holiday condition(halfday),21/3/12(no rec ->flag)
//==========23/3/12, 24/3/12(select any one option lbl)),26/3/12(check some condition in findday), 30/3/12(len(rollno))
//=========2/4/12(size,if condition ), 5/4/12(visible print setting),14/5/12 (if "not consider" value means, cant add in absent hrs)
//=========26/5/12(special hours added)
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using FarPoint.Web.Spread;
using System.Text;
using System.Globalization;
using System.IO;
using System.Text;




public partial class ReportClassLog : System.Web.UI.Page
{


    SqlConnection con_splhr_query_master = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection mysql1 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection cona = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection readcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);

    SqlCommand cmd;
    SqlCommand cmd1a;
    SqlCommand cmd3a;
    SqlCommand cmd4a;


    Hashtable hat = new Hashtable();
    Hashtable hat_days_first = new Hashtable();
    Hashtable hat_days_end = new Hashtable();
    Hashtable hat_holy = new Hashtable();
    DAccess2 dacces2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds_holi = new DataSet();

    DataTable dat1 = new DataTable();
    DataTable dat2 = new DataTable();
    DataTable dat3 = new DataTable();
    DataSet ds_attndmaster = new DataSet();

    DataTable dscount = new DataTable();
    DataRow dr;
    DataRow dr1, dr2;
    DataTable dtcout = new DataTable();
    DataTable dtcont = new DataTable();

    System.Text.StringBuilder critirianame = new System.Text.StringBuilder();

    int count_master = 0;
    string absent_calcflag = "";
    Hashtable absent_hash = new Hashtable();
    int sqlstrq = 0;
    static Boolean spl_hr_flag = false;
    Boolean leave_flag = false;
    DataSet dsprint = new DataSet();
    Hashtable has_visible_column = new Hashtable();
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    string coll_name = "", address1 = "", address2 = "", address3 = "", form_name = "", phoneno = "", faxno = "", email = "", website = "", degree_val = "";
    string column_field = "";
    int col_count_all = 0;
    int col_count = 0;
    int child_span_count = 0;
    Boolean check_col_count_flag = false;
    string printvar = "", degree_deatil = "";
    int footer_count = 0, footer_balanc_col = 0;
    int start_column = 0, end_column = 0;
    string footer_text = "", header_alignment = "";
    int split_col_for_footer = 0;
    int span_cnt = 0;
    int final_print_col_cnt = 0;
    string phone = "", fax = "", email_id = "", web_add = "";
    int temp_count = 0;
    string split_holiday_status_1 = "", split_holiday_status_2 = "";
    string value_holi_status = "";
    string[] split_holiday_status;
    int daysofrom;
    //----------------

    int count = 0;
    static string view_header = "", view_footer = "", view_footer_text = "";
    string date1 = "";
    string strsec_sub = "";
    int new_header_count = 0;
    string new_header_string = "";
    string[] new_header_string_split;
    int rc = 0;
    string Atmonth = "";
    string Atyear = "";
    int monthyr = 0;
    int Atday = 0, endk = 0;
    string datefrom = "";
    string dateto = "";
    string date2 = "", collegecode, usercode;
    int day_val = 0;
    int day_diff = 0;
    int sno = 0;
    Boolean sflag = false;
    Boolean rowflag = false;
    Boolean dayflag = false;
    DateTime dt1, dt2;
    DateTime date_today;
    string abshrs_temp = "";
    string abshrs_sus = string.Empty;
    string abshrs_ne = string.Empty;
    string abshrs_list = "";
    double totpresentday;
    double perprest, perpresthrs, perabsent, perabsenthrs, perondu, peronduhrs, perleave, perleavehrs, perabsenthrs1;
    double pertothrs, pertotondu, pertotleavehrs, pertotabsenthrs, onduday, cumcontotpresentday, percontotpresentday, hollyhrs, condhrs, condhrs_2, balamonday, att_points;
    int i = 0, minI, minII, perdayhrs, wk1, wk2, wk3, wk4, wk5, wk6, wk7, wk8, wk9, Ihof, IIhof, fullday, cumfullday, cc = 0;
    double cumperprest, cumperpresthrs, cumperabsent, cumperabsenthrs, cumperondu, cumperonduhrs, cumperleave, cumperleavehrs, checkpre, baldate, totmonth, cummcc, cumcondhrs, percondhrs = 0, cumatt_points;
    string m7, m2, m3, m4, m5, m6, m1, m8, m9;
    Double totalRows = 0;

    int hour1, hour2, hour3, hour4, hour5, hour6, hour7, hour8, hour9, condhrs1, condhrs2, condhrs3, condhrs4, condhrs5, condhrs6, condhrs7, condhrs8, condhrs9;
    int ondu1, ondu2, ondu3, ondu4, ondu5, ondu6, ondu7, ondu8, ondu9, leave1, leave2, leave3, leave4, leave5, leave6, leave7, leave8, leave9;
    string holi_month;
    string fmLength;

    int holi_days, abse_point, leave_point, diff_date;

    Boolean unmark_flag = false;
    double par = 0, abse = 0;
    double present, absent, hollydats, leaves, ondu;
    double presenthrs, absenthrs, hollydatshrs, leaveshrs, onduhrs, splhrabs;
    int perhr, abshr, rcc = 0;
    int ond, le, fyyy, mm = 1, att;
    int daycount, betdays;
    int dd = 0, dat, dumm;
    double onhr, lehr;
    int fm, fyy, fd, tm, tyy, td, fcal, tcal, k;
    double wkhr, wkhd, dumwkhr, dumwkhd, dumper, per;
    int kk = 0, cumdays, printcheck;
    string roll_no, reg_no, roll_ad, studname;
    double dumprest, dumpresthrs, dumpresenthrs, dumleaveshrs, dumonduhrs, dumabsenthrs, dumabsent, dumondu, dumleavehrs, dumleave, attday, dumattday;
    int diff = 1, att2, lea1, lea2, on_1, on_2, hdate = 0;
    double holldays, totworkday, dumtotworkday, dumperhrs, dumtoterhrs, perhrs, totperhrs;
    string frdate, todate;
    string singleuser = "", group_user = "";
    string regularflag = "";
    string genderflag = "";
    string strdayflag = "";
    static string[] string_session_values;
    static string grouporusercode = "";
    DataSet ds_sphr = new DataSet();
    static Hashtable ht_sphr = new Hashtable();
    DAccess2 d2 = new DAccess2();
    string pp = string.Empty;
    string coln = string.Empty;
    string splhrval = string.Empty;

    //added by annyutha//
    string strsec;
    string rstrsec1;
    string splhrsec;
    Boolean chkflag = false;
    Hashtable has = new Hashtable();
    Hashtable has_attnd_masterset = new Hashtable();
    Hashtable has_attnd_masterset_notconsider = new Hashtable();
    Hashtable hatabsentvalues = new Hashtable();
    DateTime temp_date = new DateTime();
    string full_hour = "";
    string single_hour = "";
    DateTime dattimefrom1 = new DateTime();
    Boolean recflag = false;
    int mng_hrs = 0, evng_hrs = 0;
    int no_of_hrs = 0;
    Boolean holiflag = false;
    string strDay = "", dummy_date = "", temp_hr_field = "", subject_no = "";
    string order = "";
    string date_temp_field = "", month_year = "";
    Hashtable temp_has_subj_code = new Hashtable();
    Boolean check_alter = false;
    DataSet ds_subject = new DataSet();
    DataSet stabsteen = new DataSet();
    DataSet dsalldetails = new DataSet();
    Boolean sunday_holiday = false;
    List<DateTime> li = new List<DateTime>();
    Hashtable checking = new Hashtable();
    Boolean setcheck = false;
    Boolean norecord = false;
    string errordate = "";
    string sem_end = string.Empty;
    string sem_start = string.Empty;
    ArrayList arrColHdrNames1 = new ArrayList();
    ArrayList arrColHdrNames2 = new ArrayList();
    Dictionary<int, string> diccount = new Dictionary<int, string>();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        norecordlbl.Visible = false;
        //GridView1.Visible = false;
        //GridView2.Visible = false;
        //GridView3.Visible = false;
        if (!Page.IsPostBack)
        {
            rdiobtndetailornot.SelectedIndex = 0;
            lblsubject.Visible = false;
            txt_subject.Visible = false;
            panel_Department.Visible = false;
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
           
           // ddlreport.Items.Insert(0, "");
            //ddlreport.Items.Insert(1, "P");
            //ddlreport.Items.Insert(2, "A");
            //ddlreport.Items.Insert(3, "OD");
            //ddlreport.Items.Insert(4, "ML");
            //ddlreport.Items.Insert(5, "SOD");
            //ddlreport.Items.Insert(6, "NSS");
            // ddlreport.Items.Insert(7, "H");
            // ddlreport.Items.Insert(8, "NJ");
            //ddlreport.Items.Insert(9, "S");
            //ddlreport.Items.Insert(10, "L");
            //ddlreport.Items.Insert(11, "NCC");
            //ddlreport.Items.Insert(12, "HS");
            //ddlreport.Items.Insert(13, "PP");
            //ddlreport.Items.Insert(14, "SYOD");
            //ddlreport.Items.Insert(15, "COD");
            //ddlreport.Items.Insert(16, "OOD");
            ////****added by subburaj********//
            //ddlreport.Items.Insert(17, "LA");
            rdiobtndetailornot.Visible = false;
            Label2.Visible = false;
            TextBox1.Visible = false;
            pnlCustomers.Visible = false;

            grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }

            lblpage.Visible = false;
            ddlpage.Visible = false;

            string date = Convert.ToString(DateTime.Today.ToShortDateString());
            string[] split = date.Split(new Char[] { '/' });
            string date_disp = split[1] + "/" + split[0] + "/" + split[2];
            txtFromDate.Text = date_disp.ToString();
            Session["curr_year"] = split[2].ToString();
            optradio.SelectedValue = "day";
            GridView1.Visible = false;
            GridView2.Visible = false;
            GridView3.Visible = false;

            btnxl.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;

            //------------initial date picker value
            string dt = DateTime.Today.ToShortDateString();
            string[] dsplit = dt.Split(new Char[] { '/' });
            txtToDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();

            txtFromDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            //----------------------------------------------

            collegecode = Session["collegecode"].ToString();
            TextBoxother.Visible = false;
            tofromlbl.Visible = false;
            pageset_pnl.Visible = false;
            spl_hr_flag = false;
            //********modified by annyutha*** 2/9/2014******//
            string str = ("select rights from  special_hr_rights where " + grouporusercode + "");

            DataSet rightsdatset = new DataSet();
            rightsdatset = dacces2.select_method_wo_parameter(str, "text");
            if (rightsdatset.Tables[0].Rows.Count > 0)
            {

                string spl_hr_rights = "";
                Hashtable od_has = new Hashtable();

                spl_hr_rights = rightsdatset.Tables[0].Rows[0]["rights"].ToString();
                if (spl_hr_rights == "True" || spl_hr_rights == "true")
                {
                    spl_hr_flag = true;

                }

            }
            //**********end*********//

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
                    ddlformat.Enabled = true;
                    bindbranch();
                    bindsem();
                    bindsec();
                    subject();

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
                    ddlformat.Enabled = false;
                }
            }
            else
            {
                //=======================page redirect from master print setting
                try
                {
                    string_session_values = Request.QueryString["val"].Split(',');
                    if (string_session_values.GetUpperBound(0) == 8)
                    {
                        bindbatch();
                        ddlbatch.SelectedIndex = Convert.ToInt16(string_session_values[0]);
                        binddegree(); if (ddldegree.Items.Count > 0)
                        {
                            ddldegree.Enabled = true;
                            ddlbranch.Enabled = true;
                            ddlduration.Enabled = true;
                            ddlsec.Enabled = true;
                            btnGo.Enabled = true;
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
                            if (string_session_values[7].ToString() == "True")
                            {
                                optradio.Items[0].Selected = true;
                            }
                            else
                            {
                                optradio.Items[0].Selected = false;
                            }

                            if (string_session_values[8].ToString() == "True")
                            {
                                optradio.Items[1].Selected = true;
                            }
                            else
                            {
                                optradio.Items[1].Selected = false;
                            }
                           
                        }
                        else
                        {
                            ddldegree.Enabled = false;
                            ddlbranch.Enabled = false;
                            ddlduration.Enabled = false;
                            ddlsec.Enabled = false;
                            btnGo.Enabled = false;
                        }
                    }
                    //===================================
                }
                catch
                {
                }
            }


            //-------------------------------Master settings


            string strdayflag;
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            Session["studtypeflag"] = "";
            strdayflag = "";
            if (Session["usercode"] != string.Empty)
            {
                string Master1 = "";
                Master1 = "select * from Master_Settings where " + grouporusercode + "";
                readcon.Close();
                readcon.Open();
                SqlDataReader mtrdr;

                SqlCommand mtcmd = new SqlCommand(Master1, readcon);
                mtrdr = mtcmd.ExecuteReader();

                Session["strvar"] = "";
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                if (mtrdr.HasRows)
                {
                    while (mtrdr.Read())
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
                        if (mtrdr["settings"].ToString() == "Days Scholor" && mtrdr["value"].ToString() == "1")
                        {
                            if (strdayflag != "" && strdayflag != "\0")
                            {
                                strdayflag = " and (r.Stud_Type='Day Scholar' or  r.Stud_Type='Hostler')";
                            }
                            else
                            {
                                strdayflag = " and r.Stud_Type='Day Scholar'";
                            }

                        }
                        if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                        {
                            if (strdayflag != "" && strdayflag != "\0")
                            {
                                strdayflag = " and (r.Stud_Type='Day Scholar' or  r.Stud_Type='Hostler')";
                            }
                            else
                            {
                                strdayflag = " and r.Stud_Type='Hostler'";
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

                        if (mtrdr["settings"].ToString() == "Male" && mtrdr["value"].ToString() == "1")
                        {
                            genderflag = " and (applyn.sex='0'";
                        }
                        if (mtrdr["settings"].ToString() == "Female" && mtrdr["value"].ToString() == "1")
                        {
                            if (genderflag != "" && genderflag != "\0")
                            {
                                genderflag = genderflag + " or applyn.sex='1'";
                            }
                            else
                            {
                                genderflag = " and (applyn.sex='1'";
                            }

                        }
                    }
                }

                Session["strvar"] = strdayflag;
                if (regularflag != "")
                {
                    regularflag = regularflag + ")";
                }
                Session["strvar"] = Session["strvar"] + regularflag;
                if (genderflag != "")
                {
                    genderflag = genderflag + ")";
                }
                Session["strvar"] = Session["strvar"] + genderflag;
                string da233 = Session["strvar"].ToString();
            }
            string grouporusercodeschool = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercodeschool = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercodeschool = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            // Added By Sridharan 12 Mar 2015
            //{
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

                    lblbatch.Text = "Year";
                    lbldegree.Text = "School Type";
                    lblbranch.Text = "Standard";
                    lblduration.Text = "Term";
                    //utes.Add("style", " width: 241px;");
                }
                else
                {
                    // forschoolsetting = false;
                }
            }
            //} Sridharan

        }

    }



    #region "before Vetri1"

    public void bindbatch()
    {
        ddlbatch.Items.Clear();
        ds = dacces2.select_method_wo_parameter("bind_batch", "sp");
        int count = ds.Tables[0].Rows.Count;
        if (count > 0)
        {
            ddlbatch.DataSource = ds;
            ddlbatch.DataTextField = "batch_year";
            ddlbatch.DataValueField = "batch_year";
            ddlbatch.DataBind();
        }
        int count1 = ds.Tables[1].Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
            ddlbatch.SelectedValue = max_bat.ToString();
            con.Close();
        }
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
        ds = dacces2.select_method("bind_degree", hat, "sp");
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
        ddlduration.Items.Clear();
        string duration = "";
        Boolean first_year = false;
        hat.Clear();
        collegecode = Session["collegecode"].ToString();
        hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
        hat.Add("batch_year", ddlbatch.SelectedValue.ToString());
        hat.Add("college_code", collegecode);
        ds = dacces2.select_method("bind_sem", hat, "sp");
        int count3 = ds.Tables[0].Rows.Count;
        if (count3 > 0)
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
            count3 = ds.Tables[1].Rows.Count;
            if (count3 > 0)
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


    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  pagesetpanel.Visible = false;
        lblnote.Visible = false;
        LabelE.Visible = false;
        LabelE.Text = string.Empty;
        pageset_pnl.Visible = false;
        // FpSpread1.Visible = false;
        GridView1.Visible = false;
        GridView2.Visible = false;
        GridView3.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        norecordlbl.Visible = false;
        tofromlbl.Visible = false;
        bindsec();
        binddate();
        if (ddlformat.SelectedItem.Text == "Hourwise Absentees")
        {
            subject();
            //binddate();
        }
    }

    public void bindsec()
    {
        ddlsec.Items.Clear();
        hat.Clear();
        hat.Add("batch_year", ddlbatch.SelectedValue.ToString());
        hat.Add("degree_code", ddlbranch.SelectedValue);
        ds = dacces2.select_method("bind_sec", hat, "sp");
        int count5 = ds.Tables[0].Rows.Count;
        if (count5 > 0)
        {
            //for (int sec_cnt = 0; sec_cnt < count5; sec_cnt++)
            //{

            //}
            ddlsec.DataSource = ds;
            ddlsec.DataTextField = "sections";
            ddlsec.DataValueField = "sections";
            ddlsec.DataBind();
            ddlsec.Items.Insert(0, "All");
            ddlsec.Enabled = true;
        }
        else
        {
            ddlsec.Enabled = false;
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        LabelE.Visible = false;
        lblnote.Visible = false;
        pageset_pnl.Visible = false;
        GridView1.Visible = false;
        GridView2.Visible = false;
        GridView3.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        norecordlbl.Visible = false;
        tofromlbl.Visible = false;
        bindbranch();
        bindsem();
        bindsec();
        binddate();
        if (ddlformat.SelectedItem.Text == "Hourwise Absentees")
        {
            subject();

        }
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

        ds = dacces2.select_method("bind_branch", hat, "sp");
        int count2 = ds.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataBind();
        }

    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        LabelE.Visible = false;
        lblnote.Visible = false;
        pageset_pnl.Visible = false;
        GridView1.Visible = false;
        GridView2.Visible = false;
        GridView3.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        norecordlbl.Visible = false;
        tofromlbl.Visible = false;
        if (Page.IsPostBack == false)
        {
            ddlduration.Items.Clear();
        }
        tofromlbl.Visible = false;
        bindsem();
        bindsec();
        binddate();
        if (ddlformat.SelectedItem.Text == "Hourwise Absentees")
        {
            subject();
        }
    }



    //public void print_btngo()
    //{
    //    final_print_col_cnt = 0;
    //    norecordlbl.Visible = false;
    //    check_col_count_flag = false;

    //    hat.Clear();
    //    hat.Add("college_code", Session["collegecode"].ToString());
    //    hat.Add("form_name", "AbsenteeRt.aspx");
    //    dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
    //    if (dsprint.Tables[0].Rows.Count > 0)
    //    {
    //        lblpage.Visible = false;
    //        ddlpage.Visible = true;

    //        //3. header add
    //        //if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
    //        //{
    //        //    new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
    //        //    new_header_string_split = new_header_string.Split(',');
    //        //    FpSpread1.Sheets[0].SheetCorner.RowCount = FpSpread1.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
    //        //}
    //        //3. end header add


    //        first_btngo();



    //        //1.set visible columns
    //        column_field = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------------get column field
    //        if (column_field != "" && column_field != null)
    //        {
    //            check_col_count_flag = true;

    //            printvar = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------visible setting columns
    //            string[] split_printvar = printvar.Split(',');
    //            for (int splval = 0; splval <= split_printvar.GetUpperBound(0); splval++)
    //            {
    //                span_cnt = 0;
    //                string[] split_star = split_printvar[splval].Split('*');
    //                if (split_star.GetUpperBound(0) > 0)
    //                {
    //                    for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount - 1; col_count++)
    //                    {
    //                        if (FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), col_count].Text == split_star[0])
    //                        {
    //                            child_span_count = 0;

    //                            string[] split_star_doller = split_star[1].Split('$');
    //                            for (int doller_count = 1; doller_count < split_star_doller.GetUpperBound(0); doller_count++)
    //                            {
    //                                for (int child_node = col_count; child_node <= col_count + split_star_doller.GetUpperBound(0); child_node++)
    //                                {
    //                                    if (FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), child_node].Text == split_star_doller[doller_count])
    //                                    {
    //                                        span_cnt++;
    //                                        if (span_cnt == 1 && child_node == col_count + 1)
    //                                        {
    //                                            FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), col_count + 1].Text = split_star[0].ToString();
    //                                            col_count++;
    //                                        }

    //                                        if (child_node != col_count)
    //                                        {
    //                                            span_cnt = child_node - (child_span_count - 1);
    //                                        }
    //                                        else
    //                                        {
    //                                            child_span_count = col_count;
    //                                        }


    //                                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), col_count, 1, span_cnt);


    //                                        FpSpread1.Sheets[0].Columns[child_node].Visible = true;

    //                                        final_print_col_cnt++;
    //                                        if (span_cnt == split_star_doller.GetUpperBound(0) - 1)
    //                                        {
    //                                            break;
    //                                        }

    //                                    }
    //                                }
    //                            }

    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //                    {
    //                        if (FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), col_count].Text == split_printvar[splval])
    //                        {
    //                            FpSpread1.Sheets[0].Columns[col_count].Visible = true;



    //                            final_print_col_cnt++;
    //                            break;
    //                        }
    //                    }
    //                }
    //            }



    //            if (dsprint.Tables[0].Rows[0]["footer"].ToString() != null && dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
    //            {
    //                footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());
    //                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 3;

    //                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 3), 0].ColumnSpan = FpSpread1.Sheets[0].ColumnCount;
    //                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 2), 0].ColumnSpan = FpSpread1.Sheets[0].ColumnCount;

    //                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 3), 0].Border.BorderColorBottom = Color.White;
    //                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 2), 0].Border.BorderColorTop = Color.White;
    //                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 2), 0].Border.BorderColorBottom = Color.White;
    //                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), 0].Border.BorderColorTop = Color.White;


    //                footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
    //                string[] footer_text_split = footer_text.Split(',');
    //                footer_text = "";




    //                if (final_print_col_cnt < footer_count)
    //                {
    //                    for (int concod_footer = 0; concod_footer < footer_count; concod_footer++)
    //                    {
    //                        if (footer_text == "")
    //                        {
    //                            footer_text = footer_text_split[concod_footer].ToString();
    //                        }
    //                        else
    //                        {
    //                            footer_text = footer_text + "   " + footer_text_split[concod_footer].ToString();
    //                        }
    //                    }

    //                    for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //                    {
    //                        if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                        {
    //                            FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, final_print_col_cnt);
    //                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text;
    //                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                            break;
    //                        }
    //                    }

    //                }

    //                else if (final_print_col_cnt == footer_count)
    //                {
    //                    for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //                    {
    //                        if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                        {
    //                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                            temp_count++;
    //                            if (temp_count == footer_count)
    //                            {
    //                                break;
    //                            }
    //                        }
    //                    }

    //                }

    //                else
    //                {
    //                    temp_count = 0;
    //                    split_col_for_footer = final_print_col_cnt / footer_count;
    //                    footer_balanc_col = final_print_col_cnt % footer_count;

    //                    for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //                    {
    //                        if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                        {
    //                            if (temp_count == 0)
    //                            {
    //                                FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer + footer_balanc_col);
    //                            }
    //                            else
    //                            {

    //                                FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer);

    //                            }
    //                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                            if (col_count - 1 >= 0)
    //                            {
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorLeft = Color.White;
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorRight = Color.White;
    //                            }
    //                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                            if (col_count + 1 < FpSpread1.Sheets[0].ColumnCount)
    //                            {
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorLeft = Color.White;
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorRight = Color.White;
    //                            }


    //                            temp_count++;
    //                            if (temp_count == 0)
    //                            {
    //                                col_count = col_count + split_col_for_footer + footer_balanc_col;
    //                            }
    //                            else
    //                            {
    //                                col_count = col_count + split_col_for_footer;
    //                            }
    //                            if (temp_count == footer_count)
    //                            {
    //                                break;
    //                            }
    //                        }
    //                    }
    //                }



    //            }

    //            //2 end.Footer setting






    //            //4.college information setting

    //            setheader_print();

    //            //4 end.college information setting

    //        }
    //        else
    //        {

    //            FpSpread1.Visible = false;
    //            btnxl.Visible = false;
    //            lblrptname.Visible = false;
    //            txtexcelname.Visible = false;
    //            Printcontrol.Visible = false;
    //            btnprintmaster.Visible = false;
    //            pageset_pnl.Visible = false;
    //            lblpage.Visible = false;
    //            ddlpage.Visible = false;
    //            norecordlbl.Visible = true;
    //            norecordlbl.Text = "Select Atleast One Column Field From The Treeview";
    //        }
    //    }
    //    // FpSpread1.Width = final_print_col_cnt * 100;
    //}
    //Hiden By Srinath 14/5/2013

    //public void setheader()
    //{

    //    string coll_name = "", address1 = "", address2 = "", address3 = "", phoneno = "", faxno = "", email = "", website = "", degree_val = "";

    //    MyImg mi = new MyImg();
    //    mi.ImageUrl = "~/images/10BIT001.jpeg";
    //    mi.ImageUrl = "Handler/Handler2.ashx?";
    //    MyImg mi2 = new MyImg();
    //    mi2.ImageUrl = "~/images/10BIT001.jpeg";
    //    mi2.ImageUrl = "Handler/Handler5.ashx?";


    //    if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
    //    {
    //        SqlDataReader dr_collinfo;
    //        con.Close();
    //        con.Open();
    //        cmd = new SqlCommand("select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website from collinfo  where college_code=" + Session["collegecode"] + "", con);
    //        dr_collinfo = cmd.ExecuteReader();
    //        while (dr_collinfo.Read())
    //        {
    //            if (dr_collinfo.HasRows == true)
    //            {

    //                coll_name = dr_collinfo["collname"].ToString();
    //                address1 = dr_collinfo["address1"].ToString();
    //                address2 = dr_collinfo["address2"].ToString();
    //                address3 = dr_collinfo["address3"].ToString();
    //                phoneno = dr_collinfo["phoneno"].ToString();
    //                faxno = dr_collinfo["faxno"].ToString();
    //                email = dr_collinfo["email"].ToString();
    //                website = dr_collinfo["website"].ToString();
    //            }
    //        }



    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColorRight = Color.White;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Border.BorderColorRight = Color.White;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[2, 4].Border.BorderColorRight = Color.White;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[3, 4].Border.BorderColorRight = Color.White;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[4, 4].Border.BorderColorRight = Color.White;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, (FpSpread1.Sheets[0].ColumnCount - 1)].Border.BorderColorLeft = Color.White;


    //        if (FpSpread1.Sheets[0].Columns[2].Visible == true)
    //        {
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, (FpSpread1.Sheets[0].ColumnCount - 3));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = coll_name;


    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorTop = Color.White;


    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, (FpSpread1.Sheets[0].ColumnCount - 1)].Border.BorderColorLeft = Color.White;


    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 1, (FpSpread1.Sheets[0].ColumnCount - 3));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Text = address1 + "-" + address2 + "-" + address3;
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, 2, 1, (FpSpread1.Sheets[0].ColumnCount - 3));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 2].Text = "Phone:" + phoneno + "  Fax:" + faxno;
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, 2, 1, (FpSpread1.Sheets[0].ColumnCount - 3));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 2].Text = "Email:" + email + "  Web Site:" + website;
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(4, 2, 1, (FpSpread1.Sheets[0].ColumnCount - 3));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 2].Text = "Hourwise/Daywise Absentees Report";
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(5, 2, 1, (FpSpread1.Sheets[0].ColumnCount - 3));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 2].Text = "----------------------------------------------------";
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[7, 2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[7, 2].Border.BorderColorTop = Color.White;
    //            string sec_val = "";

    //            if (ddlsec.SelectedValue.ToString() != string.Empty && ddlsec.SelectedValue.ToString() != null)
    //            {

    //                sec_val = "- Section: " + ddlsec.SelectedItem.ToString();
    //            }
    //            else
    //            {
    //                sec_val = "";
    //            }

    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 2, 1, (FpSpread1.Sheets[0].ColumnCount - 3));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 2].Text = ddlbatch.SelectedItem.ToString() + " Batch - " + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] " + sec_val + " ";


    //            string dt = DateTime.Today.ToShortDateString();
    //            string[] dsplit = dt.Split(new Char[] { '/' });


    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(7, 2, 1, (FpSpread1.Sheets[0].ColumnCount - 3));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[7, 2].Text = "From: " + txtFromDate.Text + "      To: " + txtToDate.Text + "       Date: " + dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();

    //            hat.Clear();
    //            hat.Add("college_code", Session["collegecode"].ToString());
    //            hat.Add("form_name", "AbsenteeRt.aspx");
    //            dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
    //            if (dsprint.Tables[0].Rows.Count > 0)
    //            {

    //                int temp_count_temp = 0;

    //                if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
    //                {
    //                    header_alignment = dsprint.Tables[0].Rows[0]["header_align"].ToString();
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[7, 2].Border.BorderColorBottom = Color.White;
    //                    for (int row_head_count = 8; row_head_count < (8 + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(row_head_count, 2, 1, (FpSpread1.Sheets[0].ColumnCount - 3));
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 2].Text = new_header_string_split[temp_count_temp].ToString();
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 2].Border.BorderColorTop = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 2].Border.BorderColorRight = Color.White;
    //                        if (row_head_count != (8 + new_header_string_split.GetUpperBound(0)))
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 2].Border.BorderColorBottom = Color.White;
    //                        }

    //                        if (header_alignment == "Center")
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 2].HorizontalAlign = HorizontalAlign.Center;
    //                        }
    //                        else if (header_alignment == "Left")
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 2].HorizontalAlign = HorizontalAlign.Left;
    //                        }
    //                        else
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 2].HorizontalAlign = HorizontalAlign.Right;
    //                        }

    //                        temp_count_temp++;
    //                    }
    //                }


    //                //----footer
    //                if (dsprint.Tables[0].Rows[0]["footer"].ToString() != null && dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
    //                {
    //                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 3;


    //                    FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 2), 0, 1, (FpSpread1.Sheets[0].ColumnCount));
    //                    FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 3), 0, 1, (FpSpread1.Sheets[0].ColumnCount));

    //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 3), 0].Border.BorderColorBottom = Color.White;
    //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 2), 0].Border.BorderColorTop = Color.White;
    //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 2), 0].Border.BorderColorBottom = Color.White;
    //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), 0].Border.BorderColorTop = Color.White;


    //                    footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());
    //                    //     FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount+3;
    //                    // FpSpread1.Sheets[0].RowCount++;
    //                    footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
    //                    string[] footer_text_split = footer_text.Split(',');
    //                    footer_text = "";
    //                    final_print_col_cnt = 0;
    //                    for (col_count_all = 0; col_count_all < FpSpread1.Sheets[0].ColumnCount; col_count_all++)
    //                    {
    //                        if (FpSpread1.Sheets[0].Columns[col_count_all].Visible == true)
    //                        {//------------invisible all column            
    //                            final_print_col_cnt++;
    //                        }
    //                    }


    //                    if (final_print_col_cnt < footer_count)
    //                    {
    //                        for (int concod_footer = 0; concod_footer < footer_count; concod_footer++)
    //                        {
    //                            if (footer_text == "")
    //                            {
    //                                footer_text = footer_text_split[concod_footer].ToString();
    //                            }
    //                            else
    //                            {
    //                                footer_text = footer_text + "   " + footer_text_split[concod_footer].ToString();
    //                            }
    //                        }

    //                        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //                        {
    //                            if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                            {

    //                                FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, final_print_col_cnt);
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text;
    //                                break;
    //                            }
    //                        }

    //                    }

    //                    else if (final_print_col_cnt == footer_count)
    //                    {
    //                        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //                        {
    //                            if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                            {
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                                temp_count++;
    //                                if (temp_count == footer_count)
    //                                {
    //                                    break;
    //                                }
    //                            }
    //                        }

    //                    }

    //                    else
    //                    {
    //                        temp_count = 0;
    //                        split_col_for_footer = final_print_col_cnt / footer_count;
    //                        footer_balanc_col = final_print_col_cnt % footer_count;

    //                        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //                        {
    //                            if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                            {
    //                                if (temp_count == 0)
    //                                {
    //                                    FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer + footer_balanc_col);
    //                                }
    //                                else
    //                                {

    //                                    FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer);

    //                                }
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                                if (col_count - 1 >= 0)
    //                                {
    //                                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorLeft = Color.White;
    //                                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorRight = Color.White;
    //                                }
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                                if (col_count + 1 < FpSpread1.Sheets[0].ColumnCount)
    //                                {
    //                                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorLeft = Color.White;
    //                                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorRight = Color.White;
    //                                }


    //                                temp_count++;
    //                                if (temp_count == 0)
    //                                {
    //                                    col_count = col_count + split_col_for_footer + footer_balanc_col;
    //                                }
    //                                else
    //                                {
    //                                    col_count = col_count + split_col_for_footer;
    //                                }
    //                                if (temp_count == footer_count)
    //                                {
    //                                    break;
    //                                }
    //                            }
    //                        }
    //                    }



    //                }
    //            }
    //            //-footer
    //        }
    //        if (FpSpread1.Sheets[0].Columns[2].Visible == false && FpSpread1.Sheets[0].Columns[3].Visible == true)
    //        {
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, (FpSpread1.Sheets[0].ColumnCount - 4));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = coll_name;



    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 3].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 3].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 3].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 3].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 3].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 3].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 3].Border.BorderColorTop = Color.White;

    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;

    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 3].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 3].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 3].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 3].Border.BorderColorLeft = Color.White;

    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 3].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 3].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 3].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 3].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, (FpSpread1.Sheets[0].ColumnCount - 1)].Border.BorderColorLeft = Color.White;

    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 3, 1, (FpSpread1.Sheets[0].ColumnCount - 4));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 3].Text = address1 + "-" + address2 + "-" + address3;
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, 3, 1, (FpSpread1.Sheets[0].ColumnCount - 4));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 3].Text = "Phone:" + phoneno + "  Fax:" + faxno;
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, 3, 1, (FpSpread1.Sheets[0].ColumnCount - 4));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 3].Text = "Email:" + email + "  Web Site:" + website;
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(4, 3, 1, (FpSpread1.Sheets[0].ColumnCount - 4));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 3].Text = "Hourwise/Daywise Absentees Report";
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(5, 3, 1, (FpSpread1.Sheets[0].ColumnCount - 4));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 3].Text = "----------------------------------------------------";

    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 3].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 3].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 3].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 3].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[7, 3].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 3].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 3].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 3].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[7, 3].Border.BorderColorTop = Color.White;
    //            string sec_val = "";

    //            if (ddlsec.SelectedValue.ToString() != string.Empty && ddlsec.SelectedValue.ToString() != null)
    //            {
    //                sec_val = "- Section: " + ddlsec.SelectedItem.ToString();
    //            }
    //            else
    //            {
    //                sec_val = "";
    //            }

    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 3, 1, (FpSpread1.Sheets[0].ColumnCount - 4));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 3].Text = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";


    //            string dt = DateTime.Today.ToShortDateString();
    //            string[] dsplit = dt.Split(new Char[] { '/' });


    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(7, 3, 1, (FpSpread1.Sheets[0].ColumnCount - 4));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[7, 3].Text = "From: " + txtFromDate.Text + "      To: " + txtToDate.Text + "       Date: " + dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();

    //            hat.Clear();
    //            hat.Add("college_code", Session["collegecode"].ToString());
    //            hat.Add("form_name", "AbsenteeRt.aspx");
    //            dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
    //            if (dsprint.Tables[0].Rows.Count > 0)
    //            {
    //                int temp_count_temp = 0;
    //                string[] header_align_index;

    //                if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
    //                {
    //                    header_alignment = dsprint.Tables[0].Rows[0]["header_align"].ToString();
    //                    header_align_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString().Split(',');

    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[7, 3].Border.BorderColorBottom = Color.White;
    //                    for (int row_head_count = 8; row_head_count < (8 + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(8, 3, 1, (FpSpread1.Sheets[0].ColumnCount - 4));
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 3].Text = new_header_string_split[temp_count_temp].ToString();
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 3].Border.BorderColorTop = Color.White;
    //                        if (row_head_count != (8 + new_header_string_split.GetUpperBound(0)))
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 3].Border.BorderColorBottom = Color.White;
    //                        }

    //                        if (header_align_index[temp_count_temp] != string.Empty)
    //                        {
    //                            header_alignment = header_align_index[temp_count_temp].ToString();
    //                            if (header_alignment == "2")
    //                            {
    //                                FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 3].HorizontalAlign = HorizontalAlign.Center;
    //                            }
    //                            else if (header_alignment == "1")
    //                            {
    //                                FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 3].HorizontalAlign = HorizontalAlign.Left;
    //                            }
    //                            else
    //                            {
    //                                FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 3].HorizontalAlign = HorizontalAlign.Right;
    //                            }
    //                        }

    //                        temp_count_temp++;
    //                    }
    //                }

    //                //----footer
    //                if (dsprint.Tables[0].Rows[0]["footer"].ToString() != null && dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
    //                {
    //                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 3;


    //                    FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 2), 0, 1, (FpSpread1.Sheets[0].ColumnCount));
    //                    FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 3), 0, 1, (FpSpread1.Sheets[0].ColumnCount));

    //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 3), 0].Border.BorderColorBottom = Color.White;
    //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 2), 0].Border.BorderColorTop = Color.White;
    //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 2), 0].Border.BorderColorBottom = Color.White;
    //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), 0].Border.BorderColorTop = Color.White;


    //                    footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());

    //                    footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
    //                    string[] footer_text_split = footer_text.Split(',');
    //                    footer_text = "";
    //                    final_print_col_cnt = 0;
    //                    for (col_count_all = 0; col_count_all < FpSpread1.Sheets[0].ColumnCount; col_count_all++)
    //                    {
    //                        FpSpread1.Sheets[0].Columns[col_count_all].Visible = false;//------------invisible all column            
    //                        final_print_col_cnt++;
    //                    }


    //                    if (final_print_col_cnt < footer_count)
    //                    {
    //                        for (int concod_footer = 0; concod_footer < footer_count; concod_footer++)
    //                        {
    //                            if (footer_text == "")
    //                            {
    //                                footer_text = footer_text_split[concod_footer].ToString();
    //                            }
    //                            else
    //                            {
    //                                footer_text = footer_text + "   " + footer_text_split[concod_footer].ToString();
    //                            }
    //                        }

    //                        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //                        {
    //                            if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                            {
    //                                FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, final_print_col_cnt);
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text;
    //                                break;
    //                            }
    //                        }

    //                    }

    //                    else if (final_print_col_cnt == footer_count)
    //                    {
    //                        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //                        {
    //                            if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                            {
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                                temp_count++;
    //                                if (temp_count == footer_count)
    //                                {
    //                                    break;
    //                                }
    //                            }
    //                        }

    //                    }

    //                    else
    //                    {
    //                        temp_count = 0;
    //                        split_col_for_footer = final_print_col_cnt / footer_count;
    //                        footer_balanc_col = final_print_col_cnt % footer_count;

    //                        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //                        {
    //                            if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                            {
    //                                if (temp_count == 0)
    //                                {
    //                                    FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer + footer_balanc_col);
    //                                }
    //                                else
    //                                {

    //                                    FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer);

    //                                }
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                                if (col_count - 1 >= 0)
    //                                {
    //                                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorLeft = Color.White;
    //                                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorRight = Color.White;
    //                                }
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                                if (col_count + 1 < FpSpread1.Sheets[0].ColumnCount)
    //                                {
    //                                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorLeft = Color.White;
    //                                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorRight = Color.White;
    //                                }


    //                                temp_count++;
    //                                if (temp_count == 0)
    //                                {
    //                                    col_count = col_count + split_col_for_footer + footer_balanc_col;
    //                                }
    //                                else
    //                                {
    //                                    col_count = col_count + split_col_for_footer;
    //                                }
    //                                if (temp_count == footer_count)
    //                                {
    //                                    break;
    //                                }
    //                            }
    //                        }
    //                    }



    //                }
    //            }
    //            //-footer

    //        }
    //        if (FpSpread1.Sheets[0].Columns[2].Visible == false && FpSpread1.Sheets[0].Columns[3].Visible == false)
    //        {
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, (FpSpread1.Sheets[0].ColumnCount - 5));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = coll_name;

    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 5, 1);
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 5, 1);
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 5, 1);
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 4].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 4].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 4].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 4].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 4].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 4].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 4].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 4].Border.BorderColorTop = Color.White;


    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 4, 1, (FpSpread1.Sheets[0].ColumnCount - 5));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Text = address1 + "-" + address2 + "-" + address3;
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, 4, 1, (FpSpread1.Sheets[0].ColumnCount - 5));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 4].Text = "Phone:" + phoneno + "  Fax:" + faxno;
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, 4, 1, (FpSpread1.Sheets[0].ColumnCount - 5));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 4].Text = "Email:" + email + "  Web Site:" + website;
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(4, 4, 1, (FpSpread1.Sheets[0].ColumnCount - 5));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 4].Text = "Hourwise/Daywise Absentees Report";
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(5, 4, 1, (FpSpread1.Sheets[0].ColumnCount - 5));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 4].Text = "----------------------------------------------------";

    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 4].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 4].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 4].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 4].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[7, 4].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 4].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 4].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 4].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[7, 4].Border.BorderColorTop = Color.White;
    //            string sec_val = "";

    //            if (ddlsec.SelectedValue.ToString() != string.Empty && ddlsec.SelectedValue.ToString() != null)
    //            {
    //                sec_val = "Section: " + ddlsec.SelectedItem.ToString();
    //            }
    //            else
    //            {
    //                sec_val = "";
    //            }

    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 4, 1, (FpSpread1.Sheets[0].ColumnCount - 5));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 4].Text = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] " + sec_val + " ";


    //            string dt = DateTime.Today.ToShortDateString();
    //            string[] dsplit = dt.Split(new Char[] { '/' });


    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(7, 4, 1, (FpSpread1.Sheets[0].ColumnCount - 5));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[7, 4].Text = "From: " + txtFromDate.Text + "      To: " + txtToDate.Text + "       Date: " + dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
    //            hat.Clear();
    //            hat.Add("college_code", Session["collegecode"].ToString());
    //            hat.Add("form_name", "AbsenteeRt.aspx");
    //            dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
    //            if (dsprint.Tables[0].Rows.Count > 0)
    //            {

    //                int temp_count_temp = 0;

    //                if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
    //                {

    //                    header_alignment = dsprint.Tables[0].Rows[0]["header_align"].ToString();
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[7, 4].Border.BorderColorBottom = Color.White;
    //                    for (int row_head_count = 8; row_head_count < (8 + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(8, 4, 1, (FpSpread1.Sheets[0].ColumnCount - 5));
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 4].Text = new_header_string_split[temp_count_temp].ToString();
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 4].Border.BorderColorTop = Color.White;
    //                        if (row_head_count != (8 + new_header_string_split.GetUpperBound(0)))
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 4].Border.BorderColorBottom = Color.White;
    //                        }

    //                        if (header_alignment == "Center")
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 4].HorizontalAlign = HorizontalAlign.Center;
    //                        }
    //                        else if (header_alignment == "Left")
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 4].HorizontalAlign = HorizontalAlign.Left;
    //                        }
    //                        else
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, 4].HorizontalAlign = HorizontalAlign.Right;
    //                        }

    //                        temp_count_temp++;
    //                    }
    //                }

    //                //----footer
    //                if (dsprint.Tables[0].Rows[0]["footer"].ToString() != null && dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
    //                {
    //                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 3;


    //                    FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 2), 0, 1, (FpSpread1.Sheets[0].ColumnCount));
    //                    FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 3), 0, 1, (FpSpread1.Sheets[0].ColumnCount));

    //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 3), 0].Border.BorderColorBottom = Color.White;
    //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 2), 0].Border.BorderColorTop = Color.White;
    //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 2), 0].Border.BorderColorBottom = Color.White;
    //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), 0].Border.BorderColorTop = Color.White;


    //                    footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());

    //                    footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
    //                    string[] footer_text_split = footer_text.Split(',');
    //                    footer_text = "";
    //                    final_print_col_cnt = 0;
    //                    for (col_count_all = 0; col_count_all < FpSpread1.Sheets[0].ColumnCount; col_count_all++)
    //                    {
    //                        FpSpread1.Sheets[0].Columns[col_count_all].Visible = false;//------------invisible all column            
    //                        final_print_col_cnt++;
    //                    }


    //                    if (final_print_col_cnt < footer_count)
    //                    {
    //                        for (int concod_footer = 0; concod_footer < footer_count; concod_footer++)
    //                        {
    //                            if (footer_text == "")
    //                            {
    //                                footer_text = footer_text_split[concod_footer].ToString();
    //                            }
    //                            else
    //                            {
    //                                footer_text = footer_text + "   " + footer_text_split[concod_footer].ToString();
    //                            }
    //                        }

    //                        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //                        {
    //                            if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                            {
    //                                FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, final_print_col_cnt);
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text;
    //                                break;
    //                            }
    //                        }

    //                    }

    //                    else if (final_print_col_cnt == footer_count)
    //                    {
    //                        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //                        {
    //                            if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                            {
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                                temp_count++;
    //                                if (temp_count == footer_count)
    //                                {
    //                                    break;
    //                                }
    //                            }
    //                        }

    //                    }

    //                    else
    //                    {
    //                        temp_count = 0;
    //                        split_col_for_footer = final_print_col_cnt / footer_count;
    //                        footer_balanc_col = final_print_col_cnt % footer_count;

    //                        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //                        {
    //                            if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                            {
    //                                if (temp_count == 0)
    //                                {
    //                                    FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer + footer_balanc_col);
    //                                }
    //                                else
    //                                {

    //                                    FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer);

    //                                }
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                                if (col_count - 1 >= 0)
    //                                {
    //                                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorLeft = Color.White;
    //                                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorRight = Color.White;
    //                                }
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                                if (col_count + 1 < FpSpread1.Sheets[0].ColumnCount)
    //                                {
    //                                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorLeft = Color.White;
    //                                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorRight = Color.White;
    //                                }


    //                                temp_count++;
    //                                if (temp_count == 0)
    //                                {
    //                                    col_count = col_count + split_col_for_footer + footer_balanc_col;
    //                                }
    //                                else
    //                                {
    //                                    col_count = col_count + split_col_for_footer;
    //                                }
    //                                if (temp_count == footer_count)
    //                                {
    //                                    break;
    //                                }
    //                            }
    //                        }
    //                    }



    //                }
    //                //-footer

    //            }
    //        }
    //        //   FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 100;//
    //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, 1);
    //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, ((FpSpread1.Sheets[0].ColumnCount - 1)), FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, 1);
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, (FpSpread1.Sheets[0].ColumnCount - 1)].CellType = mi2;



    //    }

    //    int overall_colcount = 0;
    //    overall_colcount = FpSpread1.Sheets[0].ColumnCount;
    //    //    FpSpread1.Width = overall_colcount * 100;


    //}


    //public void setheader_print()
    //{
    //    // dsprint.Tables[0].Rows[0]["column_fields"].ToString();

    //    temp_count = 0;


    //    MyImg mi = new MyImg();
    //    mi.ImageUrl = "~/images/10BIT001.jpeg";
    //    mi.ImageUrl = "Handler/Handler2.ashx?";
    //    MyImg mi2 = new MyImg();
    //    mi2.ImageUrl = "~/images/10BIT001.jpeg";
    //    mi2.ImageUrl = "Handler/Handler5.ashx?";

    //    if (final_print_col_cnt == 1)
    //    {
    //        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                one_column();
    //                break;
    //            }
    //        }

    //    }

    //    else if (final_print_col_cnt == 2)
    //    {
    //        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 0)
    //                {
    //                    //   FpSpread1.Sheets[0].ColumnHeader.Columns[col_count].Width = 100;//
    //                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                }
    //                else
    //                {
    //                    one_column();
    //                    for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount - 2; row_cnt++)
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
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
    //        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 0)
    //                {
    //                    //   FpSpread1.Sheets[0].ColumnHeader.Columns[col_count].Width = 100;//
    //                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                }
    //                else if (temp_count == 1)
    //                {
    //                    one_column();
    //                    for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount - 2; row_cnt++)
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                }
    //                else if (temp_count == 2)
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi2;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
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
    //        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 0)
    //                {
    //                    start_column = col_count;
    //                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                }


    //                if (final_print_col_cnt == temp_count + 1)
    //                {
    //                    end_column = col_count;
    //                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi2;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                }
    //                temp_count++;
    //                if (final_print_col_cnt == temp_count)
    //                {
    //                    break;
    //                }
    //            }
    //        }
    //        temp_count = 0;
    //        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 1)
    //                {
    //                    more_column();
    //                    for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount - 2; row_cnt++)
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                }
    //                temp_count++;
    //            }
    //        }
    //    }
    //}

    //public void one_column()
    //{


    //    header_text();

    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Text = coll_name;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;

    //    FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Text = address1 + "-" + address2 + "-" + address3;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorTop = Color.White;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorBottom = Color.White;

    //    if (phoneno != "" && phoneno != null)
    //    {
    //        phone = "Phone:" + phoneno;
    //    }
    //    else
    //    {
    //        phone = "";
    //    }

    //    if (faxno != "" && faxno != null)
    //    {
    //        fax = "  Fax:" + faxno;
    //    }
    //    else
    //    {
    //        fax = "";
    //    }

    //    FpSpread1.Sheets[0].ColumnHeader.Cells[2, col_count].Text = phone + fax;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorTop = Color.White;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorBottom = Color.White;

    //    if (email != "" && faxno != null)
    //    {
    //        email_id = "Email:" + email;
    //    }
    //    else
    //    {
    //        email_id = "";
    //    }


    //    if (website != "" && website != null)
    //    {
    //        web_add = "  Web Site:" + website;
    //    }
    //    else
    //    {
    //        web_add = "";
    //    }

    //    FpSpread1.Sheets[0].ColumnHeader.Cells[3, col_count].Text = email_id + web_add;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorTop = Color.White;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorBottom = Color.White;

    //    if (form_name != "" && form_name != null)
    //    {
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[4, col_count].Text = form_name;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[5, col_count].Text = "----------------------------------------------------";
    //    }
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorTop = Color.White;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorBottom = Color.White;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorTop = Color.White;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorBottom = Color.White;



    //    FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Text = degree_deatil;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorTop = Color.White;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorBottom = Color.White;

    //    FpSpread1.Sheets[0].ColumnHeader.Cells[7, col_count].Text = "From: " + txtFromDate.Text + "      To: " + txtToDate.Text + "       Date: " + DateTime.Now.ToString("dd/MM/yyyy");
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[7, col_count].Border.BorderColorTop = Color.White;


    //    int temp_count_temp = 0;

    //    if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
    //    {
    //        string[] new_header_string_index_split = dsprint.Tables[0].Rows[0]["header_align_index"].ToString().Split(',');

    //        FpSpread1.Sheets[0].ColumnHeader.Cells[7, col_count].Border.BorderColorBottom = Color.White;
    //        for (int row_head_count = 8; row_head_count < (8 + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
    //        {
    //            if (temp_count_temp <= new_header_string_index_split.GetUpperBound(0))
    //            {
    //                header_alignment = new_header_string_index_split[temp_count_temp].ToString();
    //            }

    //            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Text = new_header_string_split[temp_count_temp].ToString();
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorTop = Color.White;
    //            if (row_head_count != (8 + new_header_string_split.GetUpperBound(0)))
    //            {
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorBottom = Color.White;
    //            }

    //            if (header_alignment != string.Empty)
    //            {
    //                if (header_alignment == "2")
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Center;
    //                }
    //                else if (header_alignment == "1")
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Left;
    //                }
    //                else
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Right;
    //                }
    //            }

    //            temp_count_temp++;
    //        }
    //    }



    //}

    //public void more_column()
    //{


    //    header_text();

    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Text = coll_name;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
    //    //  FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, final_print_col_cnt - 2);
    //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count));

    //    FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Text = address1 + "-" + address2 + "-" + address3;
    //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, 1, (end_column - col_count));
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorTop = Color.White;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorBottom = Color.White;

    //    if (phoneno != "" && phoneno != null)
    //    {
    //        phone = "Phone:" + phoneno;
    //    }
    //    else
    //    {
    //        phone = "";
    //    }

    //    if (faxno != "" && faxno != null)
    //    {
    //        fax = "  Fax:" + faxno;
    //    }
    //    else
    //    {
    //        fax = "";
    //    }

    //    FpSpread1.Sheets[0].ColumnHeader.Cells[2, col_count].Text = phone + fax;
    //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, col_count, 1, (end_column - col_count));
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorTop = Color.White;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorBottom = Color.White;

    //    if (email != "" && faxno != null)
    //    {
    //        email_id = "Email:" + email;
    //    }
    //    else
    //    {
    //        email_id = "";
    //    }


    //    if (website != "" && website != null)
    //    {
    //        web_add = "  Web Site:" + website;
    //    }
    //    else
    //    {
    //        web_add = "";
    //    }

    //    FpSpread1.Sheets[0].ColumnHeader.Cells[3, col_count].Text = email_id + web_add;
    //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, col_count, 1, (end_column - col_count));
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorTop = Color.White;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorBottom = Color.White;

    //    if (form_name != "" && form_name != null)
    //    {
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[4, col_count].Text = form_name;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[5, col_count].Text = "----------------------------------------------------";

    //    }
    //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(4, col_count, 1, (end_column - col_count));
    //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(5, col_count, 1, (end_column - col_count));
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorTop = Color.White;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorBottom = Color.White;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorTop = Color.White;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorBottom = Color.White;


    //    FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Text = degree_deatil;
    //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, col_count, 1, (end_column - col_count));
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorTop = Color.White;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorBottom = Color.White;

    //    FpSpread1.Sheets[0].ColumnHeader.Cells[7, col_count].Text = "From: " + txtFromDate.Text + "      To: " + txtToDate.Text + "       Date: " + DateTime.Now.ToString("dd/MM/yyyy");
    //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(7, col_count, 1, (end_column - col_count));
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[7, col_count].Border.BorderColorTop = Color.White;


    //    int temp_count_temp = 0;

    //    if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
    //    {
    //        string[] new_header_string_index_split = dsprint.Tables[0].Rows[0]["header_align_index"].ToString().Split(',');
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[7, col_count].Border.BorderColorBottom = Color.White;
    //        for (int row_head_count = 8; row_head_count < (8 + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
    //        {
    //            if (temp_count_temp <= new_header_string_index_split.GetUpperBound(0))
    //            {
    //                header_alignment = new_header_string_index_split[temp_count_temp].ToString();
    //            }

    //            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Text = new_header_string_split[temp_count_temp].ToString();
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(row_head_count, col_count, 1, (end_column - col_count));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorTop = Color.White;
    //            if (row_head_count != (8 + new_header_string_split.GetUpperBound(0)))
    //            {
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorBottom = Color.White;
    //            }


    //            if (header_alignment != string.Empty)
    //            {

    //                if (header_alignment == "2")
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Center;
    //                }
    //                else if (header_alignment == "1")
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Left;
    //                }
    //                else
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Right;
    //                }
    //            }
    //            temp_count_temp++;
    //        }
    //    }



    //}

    public void header_text()
    {
        SqlDataReader dr_collinfo;
        con.Close();
        con.Open();
        cmd = new SqlCommand("select isnull(college_name,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website,isnull(form_heading_name,'') as form_name,isnull(batch_degree_branch,'') as degree_deatil,isnull(header_align,'') as header_alignment,isnull(header_flag_value,'') as view_header from print_master_setting  where form_name='AbsenteeRt.aspx'", con);
        dr_collinfo = cmd.ExecuteReader();
        while (dr_collinfo.Read())
        {
            if (dr_collinfo.HasRows == true)
            {

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
            }
        }

    }



    public void cal_date(double cumd)
    {

        int calm1 = fyy * 12 + 1;
        int calm2 = fyy * 12 + 2;
        int calm3 = fyy * 12 + 3;
        int calm4 = fyy * 12 + 4;
        int calm5 = fyy * 12 + 5;
        int calm6 = fyy * 12 + 6;
        int calm7 = fyy * 12 + 7;
        int calm8 = fyy * 12 + 8;
        int calm9 = fyy * 12 + 9;
        int calm10 = fyy * 12 + 10;
        int calm11 = fyy * 12 + 11;
        int calm12 = fyy * 12 + 12;
        if (calm1 == cumd || calm3 == cumd || calm5 == cumd || calm7 == cumd || calm8 == cumd || calm10 == cumd || calm12 == cumd)
        {
            daycount = 31;
        }
        if (calm4 == cumd || calm6 == cumd || calm9 == cumd || calm11 == cumd)
        {
            daycount = 30;
        }


        if (calm2 == cumd)
        {

            int lyear = 2000;
            int ly;
            if (lyear <= fyy)
            {
                ly = lyear - fyy;
            }
            else
            {
                ly = fyy - lyear;
            }
            if (ly == 4)
            {
                daycount = 29;
            }
            else
            {
                daycount = 28;

            }
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

    public void findday()
    {

        perabsenthrs = 0;
        from_today();
        int i = 0;
        double k = 1;
        dat = fd;
        date_today = dt1;

        for (int cumd = fcal; cumd <= tcal; cumd++)
        {
        nextmonth:

            totpresentday = 0;
            if (count == 0)
            {
                if (cumd == tcal)
                {
                    cal_date(cumd);

                    if (fd == td)
                    {
                        totpresentday += 1;
                    }
                    else if (td == daycount)
                    {
                        totpresentday += daycount;
                    }

                    else
                    {
                        totpresentday += td - (fd - 1);
                    }

                }

                if (cumd != tcal)
                {

                    cal_date(cumd);

                    totpresentday += daycount;
                }

                //------------find start date
                if (cumd == fcal)
                {
                    k = fd;
                }
                else
                {
                    k = 1;
                }

                if (cumd == tcal)
                {
                    endk = td;
                }
                else
                {
                    endk = int.Parse(totpresentday.ToString());
                }

                hat_days_first.Add(cumd, k);
                hat_days_end.Add(cumd, endk);
            }

            else
            {
                k = int.Parse(GetCorrespondingKey(cumd, hat_days_first).ToString());
                endk = int.Parse(GetCorrespondingKey(cumd, hat_days_end).ToString());
            }

            for (k = k; k <= endk; k++)
            {
            nextday:
                absenthrs = 0;
            splhrabs = 0;
            splhrval = string.Empty;
                if (spl_hr_flag == true)
                {
                    if (ht_sphr.Contains(Convert.ToString(date_today)))
                    {
                        getspecial_hr();
                    }
                }

                if (count == 0)
                {
                    findholy();
                    if (ds_holi.Tables[0].Rows.Count > 0)
                    {
                        if (ds_holi.Tables[0].Rows[0]["halforfull"].ToString() == "False")
                        {
                            halforfull = "0";
                        }
                        else
                        {
                            halforfull = "1";
                        }
                        if (ds_holi.Tables[0].Rows[0]["morning"].ToString() == "False")
                        {
                            mng = "0";
                        }
                        else
                        {
                            mng = "1";
                        }
                        if (ds_holi.Tables[0].Rows[0]["evening"].ToString() == "False")
                        {
                            evng = "0";
                        }
                        else
                        {
                            evng = "1";
                        }

                        holiday_sched_details = halforfull + "*" + mng + "*" + evng;
                        if (!hat_holy.ContainsKey(date_today.ToString("dd") + "/" + date_today.ToString("MM") + "/" + date_today.ToString("yyy")))
                        {
                            hat_holy.Add(date_today.ToString("dd") + "/" + date_today.ToString("MM") + "/" + date_today.ToString("yyy"), holiday_sched_details);
                        }
                        //hat_holy.Add(date_today, date_today);

                    }

                    else
                    {
                        holiday_sched_details = "3*0*0";
                        if (!hat_holy.ContainsKey(date_today.ToString("dd") + "/" + date_today.ToString("MM") + "/" + date_today.ToString("yyy")))
                        {
                            hat_holy.Add(date_today.ToString("dd") + "/" + date_today.ToString("MM") + "/" + date_today.ToString("yyy"), holiday_sched_details);
                        }
                    }

                }
                if (hat_holy.ContainsKey(date_today.ToString("dd") + "/" + date_today.ToString("MM") + "/" + date_today.ToString("yyy")))
                {
                    value_holi_status = GetCorrespondingKey(date_today.ToString("dd") + "/" + date_today.ToString("MM") + "/" + date_today.ToString("yyy"), hat_holy).ToString();
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
                        date_today = date_today.AddDays(1);
                        k++;
                        if (k <= endk)
                        {
                            goto nextday;
                        }
                        else
                        {
                            i++;
                            cumd++;
                            goto nextmonth;
                        }
                        // break;
                    }


                    //=============================================

                    {
                        m1 = "d" + k + "d1";
                        m2 = "d" + k + "d2";
                        m3 = "d" + k + "d3";
                        m4 = "d" + k + "d4";
                        m5 = "d" + k + "d5";
                        m6 = "d" + k + "d6";
                        m7 = "d" + k + "d7";
                        m8 = "d" + k + "d8";
                        m9 = "d" + k + "d9";
                        bool hsflag1 = false;
                        bool hsflag2 = false;
                        bool hsflag3 = false;
                        bool hsflag4 = false;
                        bool hsflag5 = false;
                        bool hsflag6 = false;
                        bool hsflag7 = false;
                        bool hsflag8 = false;
                        bool hsflag9 = false;
                        bool neflag1 = false; bool neflag2 = false; bool neflag3 = false; bool neflag4 = false; bool neflag5 = false; bool neflag6 = false; bool neflag7 = false; bool neflag8 = false; bool neflag9 = false;

                        int count1 = ds1.Tables[0].Rows.Count;
                        {
                            if (count1 > 0)
                            {
                                if (i < count1)
                                //  if(Convert.ToInt16( ds1.Tables[0].Rows[i]["month_year"].ToString())==cumd)
                                {
                                    if (ds1.Tables[0].Rows[i]["month_year"].ToString() == cumd.ToString())
                                    {
                                        //i++;
                                        if ((split_holiday_status_1 == "1" && Ihof >= 1) || (split_holiday_status_2 == "1" && IIhof <= 1 && Ihof < 1))
                                        {
                                            if (ds1.Tables[0].Rows[i][m1].ToString() != string.Empty)
                                            {
                                                unmark_flag = true;
                                                hour1 = int.Parse(ds1.Tables[0].Rows[i][m1].ToString());
                                                if (hour1 == 12)
                                                    hsflag1 = true;
                                                if ((string.IsNullOrEmpty(Convert.ToString(hour1))) || hour1 == 0)
                                                    neflag1 = true;

                                            }
                                            else
                                            {
                                                neflag1 = true;
                                            }
                                        }
                                        if ((split_holiday_status_1 == "1" && Ihof >= 2) || (split_holiday_status_2 == "1" && IIhof <= 2 && Ihof < 2))
                                        {
                                            if (ds1.Tables[0].Rows[i][m2].ToString() != string.Empty)
                                            {
                                                unmark_flag = true;
                                                hour2 = int.Parse(ds1.Tables[0].Rows[i][m2].ToString());
                                                if (hour2 == 12)
                                                    hsflag2 = true;
                                                if ((string.IsNullOrEmpty(Convert.ToString(hour2))) || hour2 == 0)
                                                    neflag2 = true;
                                            }
                                            else
                                            {
                                                neflag2 = true;
                                            }
                                        }
                                        if ((split_holiday_status_1 == "1" && Ihof >= 3) || (split_holiday_status_2 == "1" && IIhof <= 3 && Ihof < 3))
                                        {
                                            if (ds1.Tables[0].Rows[i][m3].ToString() != string.Empty)
                                            {
                                                unmark_flag = true;
                                                hour3 = int.Parse(ds1.Tables[0].Rows[i][m3].ToString());
                                                if (hour3 == 12)
                                                    hsflag3 = true;
                                                if ((string.IsNullOrEmpty(Convert.ToString(hour3))) || hour3 == 0)
                                                    neflag3 = true;
                                            }
                                            else
                                            {
                                                neflag3 = true;
                                            }
                                        }
                                        if ((split_holiday_status_1 == "1" && Ihof >= 4) || (split_holiday_status_2 == "1" && IIhof <= 4 && Ihof < 4))
                                        {
                                            if (ds1.Tables[0].Rows[i][m4].ToString() != string.Empty)
                                            {
                                                unmark_flag = true;
                                                hour4 = int.Parse(ds1.Tables[0].Rows[i][m4].ToString());
                                                if (hour4 == 12)
                                                    hsflag4 = true;
                                                if ((string.IsNullOrEmpty(Convert.ToString(hour4))) || hour4 == 0)
                                                    neflag4 = true;
                                            }
                                            else
                                            {
                                                neflag4 = true;
                                            }
                                        }
                                        if ((split_holiday_status_1 == "1" && Ihof >= 5) || (split_holiday_status_2 == "1" && IIhof <= 5 && Ihof < 5))
                                        {
                                            if (ds1.Tables[0].Rows[i][m5].ToString() != string.Empty)
                                            {
                                                unmark_flag = true;
                                                hour5 = int.Parse(ds1.Tables[0].Rows[i][m5].ToString());
                                                if (hour5 == 12)
                                                    hsflag5 = true;
                                                if ((string.IsNullOrEmpty(Convert.ToString(hour5))) || hour5 == 0)
                                                    neflag5 = true;
                                            }
                                            else
                                            {
                                                neflag5 = true;
                                            }
                                        }
                                        if ((split_holiday_status_1 == "1" && Ihof >= 6) || (split_holiday_status_2 == "1" && IIhof <= 6 && Ihof < 6))
                                        {
                                            if (ds1.Tables[0].Rows[i][m6].ToString() != string.Empty)
                                            {
                                                unmark_flag = true;
                                                hour6 = int.Parse(ds1.Tables[0].Rows[i][m6].ToString());
                                                if (hour6 == 12)
                                                    hsflag6 = true;
                                                if ((string.IsNullOrEmpty(Convert.ToString(hour6))) || hour6 == 0)
                                                    neflag6 = true;
                                            }
                                            else
                                            {
                                                neflag6 = true;
                                            }
                                        }
                                        if ((split_holiday_status_1 == "1" && Ihof <= 7) || (split_holiday_status_2 == "1" && IIhof <= 7 && Ihof < 7))
                                        {
                                            if (ds1.Tables[0].Rows[i][m7].ToString() != string.Empty)
                                            {
                                                unmark_flag = true;
                                                hour7 = int.Parse(ds1.Tables[0].Rows[i][m7].ToString());
                                                if (hour7 == 12)
                                                    hsflag7 = true;
                                                if ((string.IsNullOrEmpty(Convert.ToString(hour7))) || hour7 == 0)
                                                    neflag7 = true;
                                            }
                                            else
                                            {
                                                neflag7 = true;
                                            }
                                        }
                                        if ((split_holiday_status_1 == "1" && Ihof >= 8) || (split_holiday_status_2 == "1" && IIhof <= 8 && Ihof < 8))
                                        {
                                            if (ds1.Tables[0].Rows[i][m8].ToString() != string.Empty)
                                            {
                                                unmark_flag = true;
                                                hour8 = int.Parse(ds1.Tables[0].Rows[i][m8].ToString());
                                                if (hour8 == 12)
                                                    hsflag8 = true;
                                                if ((string.IsNullOrEmpty(Convert.ToString(hour8))) || hour8 == 0)
                                                    neflag8 = true;
                                            }
                                            else
                                            {
                                                neflag8 = true;
                                            }
                                        }
                                        if ((split_holiday_status_1 == "1" && Ihof >= 9) || (split_holiday_status_2 == "1" && IIhof <= 9 && Ihof < 9))
                                        {
                                            if (ds1.Tables[0].Rows[i][m9].ToString() != string.Empty)
                                            {
                                                unmark_flag = true;
                                                hour9 = int.Parse(ds1.Tables[0].Rows[i][m9].ToString());
                                                if (hour9 == 12)
                                                    hsflag9 = true;
                                                if ((string.IsNullOrEmpty(Convert.ToString(hour9))) || hour9 == 0)
                                                    neflag9 = true;
                                            }
                                            else
                                            {
                                                neflag9 = true;
                                            }
                                        }

                                        hat.Clear();
                                        hat.Add("m1", hour1.ToString());
                                        hat.Add("m2", hour2.ToString());
                                        hat.Add("m3", hour3.ToString());
                                        hat.Add("m4", hour4.ToString());
                                        hat.Add("m5", hour5.ToString());
                                        hat.Add("m6", hour6.ToString());
                                        hat.Add("m7", hour7.ToString());
                                        hat.Add("m8", hour8.ToString());
                                        hat.Add("m9", hour9.ToString());

                                        ds2 = dacces2.select_method("CAL_DAYS", hat, "sp");

                                        if ((split_holiday_status_1 == "1" && Ihof >= 1) || (split_holiday_status_2 == "1" && IIhof <= 1 && Ihof < 1))
                                        {
                                            if (ds2.Tables[0].Rows.Count != 0)
                                            {
                                                if (ds2.Tables[0].Rows[0]["FLAG"].ToString() == "0")
                                                {
                                                    wk1 = 1;
                                                }
                                                else 
                                                {
                                                    if (ds2.Tables[0].Rows[0]["FLAG"].ToString() != "2")//==========15/5/12 PRABHA
                                                    {
                                                        dayflag = true;
                                                        absenthrs = absenthrs + 1;
                                                        
    
                                                        
                                                    }
                                                        
                                                    else//==========26/5/12 PRABHA
                                                    {
                                                        wk1 = 1;//==========26/5/12 PRABHA
                                                    }
                                                    if (hsflag1 == true)
                                                        abshrs_sus = "HS-1";

                                                    else
                                                        abshrs_temp = "1";
                                                }

                                            }
                                            else
                                            {
                                                condhrs1 = 1;
                                                if (neflag1 == true)
                                                {
                                                    abshrs_ne = "NE-1";
                                                   
                                                }
                                            }
                                        }
                                        if ((split_holiday_status_1 == "1" && Ihof >= 2) || (split_holiday_status_2 == "1" && IIhof <= 2 && Ihof < 2))
                                        {
                                            if (ds2.Tables[1].Rows.Count != 0)
                                            {
                                                if (ds2.Tables[1].Rows[0]["FLAG"].ToString() == "0")
                                                {
                                                    wk2 = 1;
                                                }
                                                else
                                                {
                                                    if (ds2.Tables[1].Rows[0]["FLAG"].ToString() != "2")//==========15/5/12 PRABHA
                                                    {
                                                        absenthrs = absenthrs + 1;
                                                       
                                                       
                                                        //else
                                                        //{

                                                            if (abshrs_temp == string.Empty)
                                                            {
                                                                dayflag = true;
                                                                abshrs_temp = "2";
                                                            }
                                                            else
                                                            {
                                                                dayflag = true;
                                                                abshrs_temp = abshrs_temp + ",2";
                                                            }
                                                       // }


                                                    }
                                                    else//==========26/5/12 PRABHA
                                                    {
                                                        wk2 = 1;//==========26/5/12 PRABHA
                                                        if (hsflag2 == true)
                                                        {
                                                            if (string.IsNullOrEmpty(abshrs_sus))
                                                                abshrs_sus = "HS-2";
                                                            else
                                                                abshrs_sus = abshrs_sus + ",2";
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                condhrs2 = 1;
                                                if (neflag2 == true)
                                                {
                                                   
                                                    if (string.IsNullOrEmpty(abshrs_ne))
                                                        abshrs_ne = "NE-2";
                                                    else
                                                        abshrs_ne = abshrs_ne + ",2";
                                                }
                                                                    
                                                     
                                               
                                            }
                                        }
                                        if ((split_holiday_status_1 == "1" && Ihof >= 3) || (split_holiday_status_2 == "1" && IIhof <= 3 && Ihof < 3))
                                        {
                                            if (ds2.Tables[2].Rows.Count != 0)
                                            {
                                                if (ds2.Tables[2].Rows[0]["FLAG"].ToString() == "0")
                                                {
                                                    wk3 = 1;
                                                }
                                                else
                                                {
                                                    if (ds2.Tables[2].Rows[0]["FLAG"].ToString() != "2")//==========15/5/12 PRABHA
                                                    {
                                                        absenthrs = absenthrs + 1;
                                                        
                                                        
                                                        //else
                                                        //{
                                                            if (abshrs_temp == string.Empty)
                                                            {
                                                                dayflag = true;
                                                                abshrs_temp = "3";
                                                            }
                                                            else
                                                            {
                                                                dayflag = true;
                                                                abshrs_temp = abshrs_temp + ",3";
                                                            }
                                                      //  }



                                                    }
                                                    else//==========26/5/12 PRABHA
                                                    {
                                                        wk3 = 1;//==========26/5/12 PRABHA
                                                        if (hsflag3 == true)
                                                        {
                                                            if (string.IsNullOrEmpty(abshrs_sus))
                                                                abshrs_sus = "HS-3";
                                                            else
                                                                abshrs_sus = abshrs_sus + ",3";
                                                        }
                                                    }
                                                }

                                            }

                                            else
                                            {
                                                condhrs3 = 1;
                                              if (neflag3 == true)
                                                {
                                                   
                                                    if (string.IsNullOrEmpty(abshrs_ne))
                                                             abshrs_ne = "NE-3";
                                                     else
                                                             abshrs_ne = abshrs_ne + ",3";

                                                  }
                                                

                                            }
                                        }

                                        if ((split_holiday_status_1 == "1" && Ihof >= 4) || (split_holiday_status_2 == "1" && IIhof <= 4 && Ihof < 4))
                                        {
                                            if (ds2.Tables[3].Rows.Count != 0)
                                            {
                                                if (ds2.Tables[3].Rows[0]["FLAG"].ToString() == "0")
                                                {
                                                    wk4 = 1;
                                                }
                                                else
                                                {
                                                    if (ds2.Tables[3].Rows[0]["FLAG"].ToString() != "2")//==========15/5/12 PRABHA
                                                    {
                                                        absenthrs = absenthrs + 1;
                                                       
                                                       
                                                        //else
                                                        //{
                                                            if (abshrs_temp == string.Empty)
                                                            {
                                                                dayflag = true;
                                                                abshrs_temp = "4";
                                                            }
                                                            else
                                                            {
                                                                dayflag = true;
                                                                abshrs_temp = abshrs_temp + ",4";
                                                            }
                                                       // }


                                                    }
                                                    else//==========26/5/12 PRABHA
                                                    {
                                                        wk4 = 1;//==========26/5/12 PRABHA
                                                        if (hsflag4 == true)
                                                        {
                                                            if (string.IsNullOrEmpty(abshrs_sus))
                                                                abshrs_sus = "HS-4";
                                                            else
                                                                abshrs_sus = abshrs_sus + ",4";
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                condhrs4 = 1;
                                                  if (neflag4 == true)
                                                        {
                                                           
                                                            if (string.IsNullOrEmpty(abshrs_ne))
                                                                abshrs_ne = "NE-4";
                                                            else
                                                                abshrs_ne = abshrs_ne + ",4";

                                                        }
                                                

                                            }
                                        }

                                        if ((split_holiday_status_1 == "1" && Ihof >= 5) || (split_holiday_status_2 == "1" && IIhof <= 5 && Ihof < 5))
                                        {
                                            if (ds2.Tables[4].Rows.Count != 0)
                                            {
                                                if (ds2.Tables[4].Rows[0]["FLAG"].ToString() == "0")
                                                {
                                                    wk5 = 1;
                                                }


                                                else
                                                {
                                                    if (ds2.Tables[4].Rows[0]["FLAG"].ToString() != "2")//==========15/5/12 PRABHA
                                                    {
                                                        absenthrs = absenthrs + 1;
                                                      
                                                       
                                                        //else
                                                        //{
                                                            if (abshrs_temp == string.Empty)
                                                            {
                                                                dayflag = true;
                                                                abshrs_temp = "5";
                                                            }
                                                            else
                                                            {
                                                                dayflag = true;
                                                                abshrs_temp = abshrs_temp + ",5";
                                                            }
                                                      //  }


                                                    }
                                                    else//==========26/5/12 PRABHA
                                                    {
                                                        wk5 = 1;//==========26/5/12 PRABHA
                                                        if (hsflag5 == true)
                                                        {
                                                            if (string.IsNullOrEmpty(abshrs_sus))
                                                                abshrs_sus = "HS-5";
                                                            else
                                                                abshrs_sus = abshrs_sus + ",5";
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                condhrs5 = 1;
                                                     if (neflag5 == true)
                                                        {
                                                           
                                                            if (string.IsNullOrEmpty(abshrs_ne))
                                                                abshrs_ne = "NE-5";
                                                            else
                                                                abshrs_ne = abshrs_ne + ",5";

                                                        }
                                               
                                            }
                                        }

                                        if ((split_holiday_status_1 == "1" && Ihof >= 6) || (split_holiday_status_2 == "1" && IIhof <= 6 && Ihof < 6))
                                        {
                                            if (ds2.Tables[5].Rows.Count != 0)
                                            {
                                                if (ds2.Tables[5].Rows[0]["FLAG"].ToString() == "0")
                                                {
                                                    wk6 = 1;
                                                }
                                                else
                                                {
                                                    if (ds2.Tables[5].Rows[0]["FLAG"].ToString() != "2")//==========15/5/12 PRABHA
                                                    {
                                                        absenthrs = absenthrs + 1;
                                                        
                                                       
                                                        //else
                                                        //{
                                                            if (abshrs_temp == string.Empty)
                                                            {
                                                                dayflag = true;
                                                                abshrs_temp = "6";
                                                            }
                                                            else
                                                            {
                                                                dayflag = true;
                                                                abshrs_temp = abshrs_temp + ",6";
                                                            }
                                                      //  }


                                                    }
                                                    else//==========26/5/12 PRABHA
                                                    {
                                                        wk6 = 1;//==========26/5/12 PRABHA
                                                        if (hsflag6 == true)
                                                        {
                                                            if (string.IsNullOrEmpty(abshrs_sus))
                                                                abshrs_sus = "HS-6";
                                                            else
                                                                abshrs_sus = abshrs_sus + ",6";
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                condhrs6 = 1;
                                                    if (neflag6 == true)
                                                        {
                                                            
                                                            if (string.IsNullOrEmpty(abshrs_ne))
                                                                abshrs_ne = "NE-6";
                                                            else
                                                                abshrs_ne = abshrs_ne + ",6";

                                                        }
                                               
                                            }
                                        }

                                        if ((split_holiday_status_1 == "1" && Ihof >= 7) || (split_holiday_status_2 == "1" && IIhof <= 7 && Ihof < 7))
                                        {
                                            if (ds2.Tables[6].Rows.Count != 0)
                                            {
                                                if (ds2.Tables[6].Rows[0]["FLAG"].ToString() == "0")
                                                {
                                                    wk7 = 1;
                                                }
                                                else
                                                {

                                                    if (ds2.Tables[6].Rows[0]["FLAG"].ToString() != "2")//==========15/5/12 PRABHA
                                                    {
                                                        absenthrs = absenthrs + 1;
                                                       
                                                        
                                                        //else
                                                        //{
                                                            if (abshrs_temp == string.Empty)
                                                            {
                                                                dayflag = true;
                                                                abshrs_temp = "7";
                                                            }
                                                            else
                                                            {
                                                                dayflag = true;
                                                                abshrs_temp = abshrs_temp + ",7";
                                                            }
                                                       // }


                                                    }
                                                    else//==========26/5/12 PRABHA
                                                    {
                                                        wk7 = 1;//==========26/5/12 PRABHA
                                                        if (hsflag7 == true)
                                                        {
                                                            if (string.IsNullOrEmpty(abshrs_sus))
                                                                abshrs_sus = "HS-7";
                                                            else
                                                                abshrs_sus = abshrs_sus + ",7";
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                condhrs7 = 1;
                                                       if (neflag7 == true)
                                                        {
                                                           
                                                            if (string.IsNullOrEmpty(abshrs_ne))
                                                                abshrs_ne = "NE-7";
                                                            else
                                                                abshrs_ne = abshrs_ne + ",7";

                                                        }
                                               
                                            }

                                        }
                                        if ((split_holiday_status_1 == "1" && Ihof >= 8) || (split_holiday_status_2 == "1" && IIhof <= 8 && Ihof < 8))
                                        {
                                            if (ds2.Tables[7].Rows.Count != 0)
                                            {
                                                if (ds2.Tables[7].Rows[0]["FLAG"].ToString() == "0")
                                                {
                                                    wk8 = 1;
                                                }
                                                else
                                                {
                                                    if (ds2.Tables[7].Rows[0]["FLAG"].ToString() != "2")//==========15/5/12 PRABHA
                                                    {
                                                        absenthrs = absenthrs + 1;
                                                       
                                                        
                                                        //else
                                                        //{
                                                            if (abshrs_temp == string.Empty)
                                                            {
                                                                dayflag = true;
                                                                abshrs_temp = "8";
                                                            }
                                                            else
                                                            {
                                                                dayflag = true;
                                                                abshrs_temp = abshrs_temp + ",8";
                                                            }
                                                       // }


                                                    }
                                                    else//==========26/5/12 PRABHA
                                                    {
                                                        wk8 = 1;//==========26/5/12 PRABHA
                                                        if (hsflag8 == true)
                                                        {
                                                            if (string.IsNullOrEmpty(abshrs_sus))
                                                                abshrs_sus = "HS-8";
                                                            else
                                                                abshrs_sus = abshrs_sus + ",8";
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                condhrs8 = 1;
                                                 if (neflag8 == true)
                                                        {
                                                           
                                                            if (string.IsNullOrEmpty(abshrs_ne))
                                                                abshrs_ne = "NE-8";
                                                            else
                                                                abshrs_ne = abshrs_ne + ",8";

                                                        }
                                               
                                            }

                                        }
                                        if ((split_holiday_status_1 == "1" && Ihof >= 9) || (split_holiday_status_2 == "1" && IIhof <= 9 && Ihof < 9))
                                        {
                                            if (ds2.Tables[8].Rows.Count != 0)
                                            {
                                                if (ds2.Tables[8].Rows[0]["FLAG"].ToString() == "0")
                                                {
                                                    wk9 = 1;
                                                }
                                                else
                                                {

                                                    if (ds2.Tables[8].Rows[0]["FLAG"].ToString() != "2")//==========15/5/12 PRABHA
                                                    {
                                                        absenthrs = absenthrs + 1;
                                                       
                                                       
                                                        //else
                                                        //{
                                                            if (abshrs_temp == string.Empty)
                                                            {
                                                                dayflag = true;
                                                                abshrs_temp = "9";
                                                            }
                                                            else
                                                            {
                                                                dayflag = true;
                                                                abshrs_temp = abshrs_temp + ",9";
                                                            }
                                                       // }


                                                    }
                                                    else//==========26/5/12 PRABHA
                                                    {
                                                        wk9 = 1;//==========26/5/12 PRABHA
                                                        if (hsflag9 == true)
                                                        {
                                                            if (string.IsNullOrEmpty(abshrs_sus))
                                                                abshrs_sus = "HS-9";
                                                            else
                                                                abshrs_sus = abshrs_sus + ",9";
                                                        }
                                                    }
                                                }


                                            }
                                            else
                                            {
                                                condhrs9 = 1;
                                                       if (neflag9 == true)
                                                        {
                                                           
                                                            if (string.IsNullOrEmpty(abshrs_ne))
                                                                abshrs_ne = "NE-9";
                                                            else
                                                                abshrs_ne = abshrs_ne + ",9";

                                                        }
                                                
                                            }
                                        }

                                        if (fullday == 9)
                                        {
                                            // condhrs = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8 + condhrs9;
                                            //  if (hour1 != 0 && hour2 != 0 && hour3 != 0 && hour4 != 0 && hour5 != 0 && hour6 != 0 && hour7 != 0 && hour8 != 0 && hour9 != 0)
                                            {

                                                if (Ihof == 0 && IIhof == 9)
                                                {
                                                    condhrs = 0;
                                                    condhrs_2 = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8 + condhrs9;
                                                    att = 0;
                                                    att2 = wk1 + wk2 + wk3 + wk4 + wk5 + wk6 + wk7 + wk8 + wk9;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                }
                                                else if (Ihof == 1 && IIhof == 8)
                                                {
                                                    condhrs = condhrs1;
                                                    condhrs_2 = condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8 + condhrs9;
                                                    att = wk1;
                                                    att2 = wk2 + wk3 + wk4 + wk5 + wk6 + wk7 + wk8 + wk9;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 2 && IIhof == 7)
                                                {
                                                    condhrs = condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8 + condhrs9;
                                                    att = wk1 + wk2;
                                                    att2 = wk3 + wk4 + wk5 + wk6 + wk7 + wk8 + wk9;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }
                                                else if (Ihof == 3 && IIhof == 6)
                                                {
                                                    condhrs = condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8 + condhrs9;
                                                    att = wk1 + wk2 + wk3;
                                                    att2 = wk4 + wk5 + wk6 + wk7 + wk8 + wk9;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }


                                                else if (Ihof == 4 && IIhof == 5)
                                                {
                                                    condhrs = condhrs4 + condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs5 + condhrs6 + condhrs7 + condhrs8 + condhrs9;
                                                    att = wk1 + wk2 + wk3 + wk4;
                                                    att2 = wk5 + wk6 + wk7 + wk8 + wk9;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }


                                                else if (Ihof == 5 && IIhof == 4)
                                                {
                                                    condhrs = condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs6 + condhrs7 + condhrs8 + condhrs9;
                                                    att = wk1 + wk2 + wk3 + wk4 + wk5;
                                                    att2 = wk6 + wk7 + wk8 + wk9;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 6 && IIhof == 3)
                                                {
                                                    condhrs = condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs7 + condhrs8 + condhrs9;
                                                    att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6;
                                                    att2 = wk7 + wk8 + wk9;

                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                }

                                                else if (Ihof == 7 && IIhof == 2)
                                                {
                                                    condhrs = condhrs7 + condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs8 + condhrs9;
                                                    att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6 + wk7;
                                                    att2 = wk8 + wk9;


                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 8 && IIhof == 1)
                                                {

                                                    condhrs = condhrs8 + condhrs7 + condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs9;
                                                    att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6 + wk7 + wk8;
                                                    att2 = wk9;

                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 9 && IIhof == 0)
                                                {
                                                    condhrs = condhrs9 + condhrs8 + condhrs7 + condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = 0;
                                                    att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6 + wk7 + wk8 + wk9;
                                                    att2 = 0;

                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                            }


                                        }
                                        else if (fullday == 8)
                                        {
                                            //condhrs = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8;
                                            //     if (hour1 != 0 && hour2 != 0 && hour3 != 0 && hour4 != 0 && hour5 != 0 && hour6 != 0 && hour7 != 0 && hour8 != 0)
                                            {
                                                if (Ihof == 0 && IIhof == 8)
                                                {
                                                    condhrs = 0;
                                                    condhrs_2 = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8;
                                                    att = 0;
                                                    att2 = wk1 + wk2 + wk3 + wk4 + wk5 + wk6 + wk7 + wk8;

                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 1 && IIhof == 7)
                                                {

                                                    condhrs = condhrs1;
                                                    condhrs_2 = condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8;
                                                    att = wk1;
                                                    att2 = wk2 + wk3 + wk4 + wk5 + wk6 + wk7 + wk8;

                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }


                                                else if (Ihof == 2 && IIhof == 6)
                                                {

                                                    condhrs = condhrs1 + condhrs2;
                                                    condhrs_2 = condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8;
                                                    att = wk1 + wk2;
                                                    att2 = wk3 + wk4 + wk5 + wk6 + wk7 + wk8;

                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 3 && IIhof == 5)
                                                {
                                                    condhrs = condhrs3 + condhrs1 + condhrs2;
                                                    condhrs_2 = condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8;
                                                    att = wk1 + wk2 + wk3;
                                                    att2 = wk4 + wk5 + wk6 + wk7 + wk8;


                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 4 && IIhof == 4)
                                                {
                                                    condhrs = condhrs4 + condhrs3 + condhrs1 + condhrs2;
                                                    condhrs_2 = condhrs5 + condhrs6 + condhrs7 + condhrs8;
                                                    att = wk1 + wk2 + wk3 + wk4;
                                                    att2 = wk5 + wk6 + wk7 + wk8;


                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 5 && IIhof == 3)
                                                {
                                                    condhrs = condhrs5 + condhrs4 + condhrs3 + condhrs1 + condhrs2;
                                                    condhrs_2 = condhrs6 + condhrs7 + condhrs8;
                                                    att = wk1 + wk2 + wk3 + wk4 + wk5;
                                                    att2 = wk6 + wk7 + wk8;


                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }


                                                else if (Ihof == 6 && IIhof == 2)
                                                {
                                                    condhrs = condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs1 + condhrs2;
                                                    condhrs_2 = condhrs7 + condhrs8;
                                                    att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6;
                                                    att2 = wk7 + wk8;

                                                    if (split_holiday_status_1 == "1")
                                                    {

                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;

                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }


                                                else if (Ihof == 7 && IIhof == 1)
                                                {
                                                    condhrs = condhrs7 + condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs1 + condhrs2;
                                                    condhrs_2 = condhrs8;
                                                    att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6 + wk7;
                                                    att2 = wk8;

                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                            wk8 = 0;

                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }


                                                else if (Ihof == 8 && IIhof == 0)
                                                {
                                                    condhrs = condhrs7 + condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs1 + condhrs2 + condhrs8;
                                                    att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6 + wk7 + wk8;
                                                    att2 = 0;

                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                            }



                                        }

                                        else if (fullday == 7)
                                        {
                                            //condhrs = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7;
                                            //  if (hour1 != 0 && hour2 != 0 && hour3 != 0 && hour4 != 0 && hour5 != 0 && hour6 != 0 && hour7 != 0)
                                            {

                                                if (Ihof == 0 && IIhof == 7)
                                                {
                                                    condhrs = 0;
                                                    condhrs_2 = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7;
                                                    att = 0;
                                                    att2 = wk1 + wk2 + wk3 + wk4 + wk5 + wk6 + wk7;

                                                    if (split_holiday_status_1 == "1")
                                                    {

                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 1 && IIhof == 6)
                                                {
                                                    condhrs = condhrs1;
                                                    condhrs_2 = condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7;
                                                    att = wk1;
                                                    att2 = wk2 + wk3 + wk4 + wk5 + wk6 + wk7;

                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            att = 0;
                                                            present += 0.5;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }


                                                else if (Ihof == 2 && IIhof == 5)
                                                {
                                                    condhrs = condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7;
                                                    att = wk1 + wk2;
                                                    att2 = wk3 + wk4 + wk5 + wk6 + wk7;

                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {

                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }


                                                else if (Ihof == 3 && IIhof == 4)
                                                {
                                                    condhrs = condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs4 + condhrs5 + condhrs6 + condhrs7;
                                                    att = wk1 + wk2 + wk3;
                                                    att2 = wk4 + wk5 + wk6 + wk7;

                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 4 && IIhof == 3)
                                                {
                                                    condhrs = condhrs4 + condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs5 + condhrs6 + condhrs7;
                                                    att = wk1 + wk2 + wk3 + wk4;
                                                    att2 = wk5 + wk6 + wk7;

                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 5 && IIhof == 2)
                                                {
                                                    condhrs = condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs6 + condhrs7;
                                                    att = wk1 + wk2 + wk3 + wk4 + wk5;
                                                    att2 = wk6 + wk7;
                                                    if (split_holiday_status_1 == "1")
                                                    {

                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }

                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }


                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 6 && IIhof == 1)
                                                {
                                                    condhrs = condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs7;
                                                    att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6;
                                                    att2 = wk7;

                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 7 && IIhof == 0)
                                                {
                                                    condhrs = condhrs7 + condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = 0;
                                                    att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6 + wk7;
                                                    att2 = 0;

                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }
                                            }
                                        }


                                        else if (fullday == 6)
                                        {
                                            //  condhrs = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6;
                                            //   if (hour1 != 0 && hour2 != 0 && hour3 != 0 && hour4 != 0 && hour5 != 0 && hour6 != 0)
                                            {

                                                if (Ihof == 0 && IIhof == 6)
                                                {
                                                    condhrs = 0;
                                                    condhrs_2 = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6;

                                                    att = 0;
                                                    att2 = wk1 + wk2 + wk3 + wk4 + wk5 + wk6;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 1 && IIhof == 5)
                                                {

                                                    condhrs = condhrs1;
                                                    condhrs_2 = condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6;
                                                    att = wk1;
                                                    att2 = wk2 + wk3 + wk4 + wk5 + wk6;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            att = 0;
                                                            present += 0.5;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {

                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }


                                                else if (Ihof == 2 && IIhof == 4)
                                                {

                                                    condhrs = condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs3 + condhrs4 + condhrs5 + condhrs6;
                                                    att = wk1 + wk2;
                                                    att2 = wk3 + wk4 + wk5 + wk6;

                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                    if (split_holiday_status_2 == "1")
                                                    {

                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }


                                                else if (Ihof == 3 && IIhof == 3)
                                                {
                                                    condhrs = condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs4 + condhrs5 + condhrs6;

                                                    att = wk1 + wk2 + wk3;
                                                    att2 = wk4 + wk5 + wk6;

                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 4 && IIhof == 2)
                                                {

                                                    condhrs = condhrs4 + condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs5 + condhrs6;
                                                    att = wk1 + wk2 + wk3 + wk4;
                                                    att2 = wk5 + wk6;

                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 5 && IIhof == 1)
                                                {

                                                    condhrs = condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs6;
                                                    att = wk1 + wk2 + wk3 + wk4 + wk5;
                                                    att2 = wk6;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }


                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 6 && IIhof == 0)
                                                {


                                                    condhrs = condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = 0;
                                                    att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6;
                                                    att2 = 0;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }

                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                            }


                                        }
                                        else if (fullday == 5)
                                        {
                                            //condhrs = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5;
                                            //  if (hour1 != 0 && hour2 != 0 && hour3 != 0 && hour4 != 0 && hour5 != 0)


                                            if (Ihof == 0 && IIhof == 5)
                                            {
                                                condhrs = 0;
                                                condhrs_2 = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5;
                                                att = 0;
                                                att2 = wk1 + wk2 + wk3 + wk4 + wk5;
                                                lea2 = leave1 + leave2 + leave3 + leave4 + leave5;
                                                on_2 = ondu1 + ondu2 + ondu3 + ondu4 + ondu5;
                                                lea1 = 0;
                                                on_1 = 0;


                                                if (split_holiday_status_1 == "1")
                                                {
                                                    if (minI - condhrs <= att)
                                                    {
                                                        present += 0.5;
                                                    }
                                                    else if (minI <= lea1)
                                                    {
                                                        leaves += 0.5;
                                                    }
                                                    else if (minI <= on_1)
                                                    {

                                                        pertotondu += 0.5;

                                                    }
                                                    else
                                                    {
                                                        absent += 0.5;
                                                    }
                                                }

                                                if (split_holiday_status_2 == "1")
                                                {
                                                    if (minII - condhrs_2 <= att2)
                                                    {
                                                        present += 0.5;
                                                        att2 = 0;

                                                    }
                                                    else if (minII <= lea1)
                                                    {
                                                        leaves += 0.5;
                                                    }
                                                    else if (minII <= on_1)
                                                    {

                                                        pertotondu += 0.5;

                                                    }
                                                    else
                                                    {
                                                        absent += 0.5;
                                                    }
                                                }
                                            }

                                            else if (Ihof == 1 && IIhof == 4)
                                            {
                                                condhrs = condhrs1;
                                                condhrs_2 = condhrs2 + condhrs3 + condhrs4 + condhrs5;
                                                att = wk1;
                                                att2 = wk2 + wk3 + wk4 + wk5;
                                                lea2 = leave2 + leave3 + leave4 + leave5;
                                                on_2 = ondu2 + ondu3 + ondu4 + ondu5;
                                                lea1 = leave1;
                                                on_1 = ondu1;

                                                if (split_holiday_status_1 == "1")
                                                {
                                                    if (minI - condhrs <= att)
                                                    {
                                                        att = 0;
                                                        present += 0.5;
                                                    }
                                                    else if (minI <= lea1)
                                                    {
                                                        leaves += 0.5;
                                                    }
                                                    else if (minI <= on_1)
                                                    {

                                                        pertotondu += 0.5;

                                                    }
                                                    else
                                                    {
                                                        absent += 0.5;
                                                    }
                                                }

                                                if (split_holiday_status_2 == "1")
                                                {
                                                    if (minII - condhrs_2 <= att2)
                                                    {
                                                        present += 0.5;
                                                        att2 = 0;
                                                    }
                                                    else if (minII <= lea1)
                                                    {
                                                        leaves += 0.5;
                                                    }
                                                    else if (minII <= on_1)
                                                    {

                                                        pertotondu += 0.5;

                                                    }
                                                    else
                                                    {
                                                        absent += 0.5;
                                                    }
                                                }
                                            }


                                            else if (Ihof == 2 && IIhof == 3)
                                            {

                                                condhrs = condhrs2 + condhrs1;
                                                condhrs_2 = condhrs3 + condhrs4 + condhrs5;
                                                att = wk1 + wk2;
                                                att2 = wk3 + wk4 + wk5;
                                                lea2 = leave3 + leave4 + leave5;
                                                on_2 = ondu3 + ondu4 + ondu5;
                                                lea1 = leave1 + leave2;
                                                on_1 = ondu1 + ondu2;

                                                if (split_holiday_status_1 == "1")
                                                {
                                                    if (minI - condhrs <= att)
                                                    {
                                                        present += 0.5;
                                                    }
                                                    else if (minI <= lea1)
                                                    {
                                                        leaves += 0.5;
                                                    }
                                                    else if (minI <= on_1)
                                                    {

                                                        pertotondu += 0.5;

                                                    }
                                                    else
                                                    {
                                                        absent += 0.5;
                                                    }
                                                }

                                                if (split_holiday_status_2 == "1")
                                                {
                                                    if (minII - condhrs_2 <= att2)
                                                    {
                                                        present += 0.5;
                                                        att2 = 0;

                                                    }
                                                    else if (minII <= lea1)
                                                    {
                                                        leaves += 0.5;
                                                    }
                                                    else if (minII <= on_1)
                                                    {

                                                        pertotondu += 0.5;

                                                    }
                                                    else
                                                    {
                                                        absent += 0.5;
                                                    }
                                                }
                                            }


                                            else if (Ihof == 3 && IIhof == 2)
                                            {

                                                condhrs = condhrs3 + condhrs2 + condhrs1;
                                                condhrs_2 = condhrs4 + condhrs5;
                                                att = wk1 + wk2 + wk3;
                                                att2 = wk4 + wk5;
                                                lea2 = leave4 + leave5;
                                                on_2 = ondu4 + ondu5;
                                                lea1 = leave1 + leave2 + leave3;
                                                on_1 = ondu1 + ondu2 + ondu3;

                                                if (split_holiday_status_1 == "1")
                                                {
                                                    if (minI - condhrs <= att)
                                                    {
                                                        present += 0.5;
                                                        att = 0;
                                                    }
                                                    else if (minI <= lea1)
                                                    {
                                                        leaves += 0.5;
                                                    }
                                                    else if (minI <= on_1)
                                                    {

                                                        pertotondu += 0.5;

                                                    }
                                                    else
                                                    {
                                                        absent += 0.5;
                                                    }
                                                }

                                                if (split_holiday_status_2 == "1")
                                                {
                                                    if (minII - condhrs_2 <= att2)
                                                    {
                                                        present += 0.5;
                                                        att2 = 0;

                                                    }
                                                    else if (minII <= lea1)
                                                    {
                                                        leaves += 0.5;
                                                    }
                                                    else if (minII <= on_1)
                                                    {

                                                        pertotondu += 0.5;

                                                    }
                                                    else
                                                    {
                                                        absent += 0.5;
                                                    }
                                                }
                                            }

                                            else if (Ihof == 4 && IIhof == 1)
                                            {
                                                condhrs = condhrs4 + condhrs3 + condhrs2 + condhrs1;
                                                condhrs_2 = condhrs5;
                                                att = wk1 + wk2 + wk3 + wk4;
                                                att2 = wk5;
                                                lea2 = leave5;
                                                on_2 = ondu5;
                                                lea1 = leave1 + leave2 + leave3 + leave4;
                                                on_1 = ondu1 + ondu2 + ondu3 + ondu4;

                                                if (split_holiday_status_1 == "1")
                                                {
                                                    if (minI - condhrs <= att)
                                                    {
                                                        present += 0.5;
                                                        att = 0;
                                                    }
                                                    else if (minI <= lea1)
                                                    {
                                                        leaves += 0.5;
                                                    }
                                                    else if (minI <= on_1)
                                                    {

                                                        pertotondu += 0.5;

                                                    }
                                                    else
                                                    {
                                                        absent += 0.5;
                                                    }
                                                }

                                                if (split_holiday_status_2 == "1")
                                                {
                                                    if (minII - condhrs_2 <= att2)
                                                    {
                                                        present += 0.5;
                                                        att2 = 0;

                                                    }
                                                    else if (minII <= lea1)
                                                    {
                                                        leaves += 0.5;
                                                    }
                                                    else if (minII <= on_1)
                                                    {

                                                        pertotondu += 0.5;

                                                    }
                                                    else
                                                    {
                                                        absent += 0.5;
                                                    }
                                                }
                                            }

                                            else if (Ihof == 5 && IIhof == 0)
                                            {

                                                condhrs = condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
                                                condhrs_2 = 0;
                                                att = wk1 + wk2 + wk3 + wk4 + wk5;
                                                att2 = 0;
                                                lea2 = 0;
                                                on_2 = 0;
                                                lea1 = leave1 + leave2 + leave3 + leave4 + leave5;
                                                on_1 = ondu1 + ondu2 + ondu3 + ondu4 + ondu5;

                                                if (split_holiday_status_1 == "1")
                                                {
                                                    if (minI - condhrs <= att)
                                                    {
                                                        present += 0.5;
                                                        att = 0;
                                                    }
                                                    else if (minI <= lea1)
                                                    {
                                                        leaves += 0.5;
                                                    }
                                                    else if (minI <= on_1)
                                                    {

                                                        pertotondu += 0.5;

                                                    }
                                                    else
                                                    {
                                                        absent += 0.5;
                                                    }
                                                }

                                                if (split_holiday_status_2 == "1")
                                                {
                                                    if (minI - condhrs_2 <= att2)
                                                    {
                                                        present += 0.5;
                                                        att2 = 0;

                                                    }
                                                    else if (minII <= lea1)
                                                    {
                                                        leaves += 0.5;
                                                    }
                                                    else if (minII <= on_1)
                                                    {

                                                        pertotondu += 0.5;

                                                    }
                                                    else
                                                    {
                                                        absent += 0.5;
                                                    }
                                                }
                                            }


                                        }

                                        else if (fullday == 4)
                                        {
                                            // condhrs = condhrs1 + condhrs2 + condhrs3 + condhrs4;
                                            // if (hour1 != 0 && hour2 != 0 && hour3 != 0 && hour4 != 0)
                                            {


                                                if (Ihof == 0 && IIhof == 4)
                                                {
                                                    condhrs = 0;
                                                    condhrs_2 = condhrs1 + condhrs2 + condhrs3 + condhrs4;
                                                    att = 0;
                                                    att2 = wk1 + wk2 + wk3 + wk4;
                                                    lea1 = 0;
                                                    on_1 = 0;
                                                    lea2 = leave1 + leave2 + leave3 + leave4;
                                                    on_2 = ondu1 + ondu2 + ondu3 + ondu4;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                        }
                                                        else if (minI <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minI <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }
                                                        else if (minII <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minII <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 1 && IIhof == 3)
                                                {
                                                    condhrs = condhrs1;
                                                    condhrs_2 = condhrs2 + condhrs3 + condhrs4;
                                                    att = wk1;
                                                    att2 = wk2 + wk3 + wk4;
                                                    lea1 = leave1;
                                                    on_1 = ondu1;
                                                    lea2 = leave2 + leave3 + leave4;
                                                    on_2 = ondu2 + ondu3 + ondu4;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            att = 0;
                                                            present += 0.5;
                                                        }
                                                        else if (minI <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minI <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }
                                                        else if (minII <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minII <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }


                                                else if (Ihof == 2 && IIhof == 2)
                                                {
                                                    condhrs = condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs3 + condhrs4;
                                                    att = wk1 + wk2;
                                                    att2 = wk3 + wk4;
                                                    lea1 = leave1 + leave2;
                                                    on_1 = ondu1 + ondu2;
                                                    lea2 = leave3 + leave4;
                                                    on_2 = ondu3 + ondu4;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                        }
                                                        else if (minI <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minI <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }
                                                        else if (minII <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minII <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }


                                                else if (Ihof == 3 && IIhof == 1)
                                                {

                                                    condhrs = condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs4;
                                                    att = wk1 + wk2 + wk3;
                                                    att2 = wk4;
                                                    lea1 = leave1 + leave2 + leave3;
                                                    on_1 = ondu1 + ondu2 + ondu3;
                                                    lea2 = leave4;
                                                    on_2 = ondu4;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }
                                                        else if (minI <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minI <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }
                                                        else if (minII <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minII <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 4 && IIhof == 0)
                                                {
                                                    condhrs = condhrs4 + condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = 0;
                                                    att = wk1 + wk2 + wk3 + wk4;
                                                    att2 = 0;
                                                    lea1 = leave1 + leave2 + leave3 + leave4;
                                                    on_1 = ondu1 + ondu2 + ondu3 + ondu4;
                                                    lea2 = 0;
                                                    on_2 = 0;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }
                                                        else if (minI <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minI <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }
                                                        else if (minII <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minII <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                            }
                                        }

                                        else if (fullday == 3)
                                        {
                                            //  condhrs = condhrs1 + condhrs2 + condhrs3;
                                            //   if (hour1 != 0 && hour2 != 0 && hour3 != 0)
                                            {

                                                if (Ihof == 0 && IIhof == 3)
                                                {
                                                    condhrs = 0;

                                                    condhrs_2 = condhrs1 + condhrs2 + condhrs3;
                                                    att = 0;
                                                    att2 = wk1 + wk2 + wk3;
                                                    lea2 = leave1 + leave2 + leave3;
                                                    on_2 = ondu1 + ondu2 + ondu3;
                                                    lea1 = 0;
                                                    on_1 = 0;

                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                        }
                                                        else if (minI <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minI <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;
                                                        }
                                                        else if (minII <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minII <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 1 && IIhof == 2)
                                                {
                                                    condhrs = condhrs1;
                                                    condhrs_2 = condhrs2 + condhrs3;
                                                    att = wk1;
                                                    att2 = wk2 + wk3;
                                                    lea2 = leave2 + leave3;
                                                    on_2 = ondu2 + ondu3;
                                                    lea1 = leave1;
                                                    on_1 = ondu1;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            att = 0;
                                                            present += 0.5;
                                                        }
                                                        else if (minI <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minI <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }
                                                        else if (minII <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minII <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }

                                                    }
                                                }


                                                else if (Ihof == 2 && IIhof == 1)
                                                {

                                                    condhrs = condhrs2 + condhrs1;
                                                    condhrs_2 = condhrs3;
                                                    att = wk1 + wk2;
                                                    att2 = wk3;
                                                    lea2 = leave3;
                                                    on_2 = ondu3;
                                                    lea1 = leave1 + leave2;
                                                    on_1 = ondu1 + ondu2;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                        }
                                                        else if (minI <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minI <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }
                                                        else if (minII <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minII <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }


                                                else if (Ihof == 3 && IIhof == 0)
                                                {

                                                    condhrs = condhrs3 + condhrs2 + condhrs1;
                                                    condhrs_2 = 0;
                                                    att = wk1 + wk2 + wk3;
                                                    att2 = 0;
                                                    lea2 = 0;
                                                    on_2 = 0;
                                                    lea1 = leave1 + leave2 + leave3;
                                                    on_1 = ondu1 + ondu2 + ondu3;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                            att = 0;
                                                        }
                                                        else if (minI <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minI <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }

                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }
                                                        else if (minII <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minII <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                            }


                                        }

                                        else if (fullday == 2)
                                        {
                                            //  condhrs = condhrs1 + condhrs2;
                                            //  if (hour1 != 0 && hour2 != 0)
                                            {
                                                if (Ihof == 0 && IIhof == 2)
                                                {
                                                    condhrs = 0;
                                                    condhrs_2 = condhrs1 + condhrs2;

                                                    att = 0;
                                                    att2 = wk1 + wk2;
                                                    lea1 = 0;
                                                    on_1 = 0;
                                                    lea2 = leave1 + leave2;
                                                    on_2 = ondu1 + ondu2;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                        }
                                                        else if (minI <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minI <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }
                                                        else if (minII <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minII <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }

                                                else if (Ihof == 1 && IIhof == 1)
                                                {

                                                    condhrs = condhrs1;
                                                    condhrs_2 = condhrs2;
                                                    att = wk1;
                                                    att2 = wk2;
                                                    lea1 = leave1;
                                                    on_1 = ondu1;
                                                    lea2 = leave2;
                                                    on_2 = ondu2;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            att = 0;
                                                            present += 0.5;
                                                        }
                                                        else if (minI <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minI <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }
                                                        else if (minII <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minII <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }


                                                else if (Ihof == 2 && IIhof == 0)
                                                {

                                                    condhrs = condhrs1 + condhrs2;
                                                    condhrs_2 = 0;
                                                    att = wk1 + wk2;
                                                    att2 = 0;
                                                    lea1 = leave1 + leave2;
                                                    on_1 = ondu1 + ondu2;
                                                    lea2 = 0;
                                                    on_2 = 0;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                        }
                                                        else if (minI <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minI <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {

                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }
                                                        else if (minII <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minII <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;
                                                        }
                                                    }
                                                }


                                            }

                                        }

                                        else if (fullday == 1)
                                        {
                                            //  condhrs = condhrs1;

                                            //    if (hour1 != 0)
                                            {
                                                if (Ihof == 0 && IIhof == 1)
                                                {
                                                    condhrs = 0;
                                                    condhrs_2 = condhrs1;
                                                    att = 0;
                                                    att2 = wk1;
                                                    lea2 = leave1;
                                                    on_2 = ondu1;
                                                    lea1 = 0;
                                                    on_1 = 0;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            present += 0.5;
                                                        }
                                                        else if (minI <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minI <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;

                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {
                                                        if (minII <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }
                                                        else if (minII - condhrs_2 <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minII <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;

                                                        }
                                                    }
                                                }

                                                else if (Ihof == 1 && IIhof == 0)
                                                {
                                                    condhrs = condhrs1;
                                                    condhrs_2 = 0;
                                                    att = wk1;
                                                    att2 = 0;
                                                    lea1 = leave1;
                                                    on_1 = ondu1;
                                                    lea2 = 0;
                                                    on_2 = 0;
                                                    if (split_holiday_status_1 == "1")
                                                    {
                                                        if (minI - condhrs <= att)
                                                        {
                                                            att = 0;
                                                            present += 0.5;
                                                        }
                                                        else if (minI <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minI <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;

                                                        }
                                                    }

                                                    if (split_holiday_status_2 == "1")
                                                    {

                                                        if (minII - condhrs_2 <= att2)
                                                        {
                                                            present += 0.5;
                                                            att2 = 0;

                                                        }
                                                        else if (minII <= lea1)
                                                        {
                                                            leaves += 0.5;
                                                        }
                                                        else if (minII <= on_1)
                                                        {

                                                            pertotondu += 0.5;

                                                        }
                                                        else
                                                        {
                                                            absent += 0.5;

                                                        }
                                                    }
                                                }
                                            }

                                        }
                                        att = 0;
                                        att2 = 0;
                                        wk1 = 0;
                                        wk2 = 0;
                                        wk3 = 0;
                                        wk4 = 0;
                                        wk5 = 0;
                                        wk6 = 0;
                                        wk7 = 0;
                                        wk8 = 0;
                                        condhrs1 = 0;
                                        condhrs2 = 0;
                                        condhrs2 = 0;
                                        condhrs3 = 0;
                                        condhrs4 = 0;
                                        condhrs5 = 0;
                                        condhrs6 = 0;
                                        condhrs7 = 0;
                                        condhrs8 = 0;
                                        condhrs9 = 0;
                                        hour1 = 0;
                                        hour2 = 0;
                                        hour3 = 0;
                                        hour4 = 0;
                                        hour5 = 0;
                                        hour6 = 0;
                                        hour7 = 0;
                                        hour8 = 0;
                                        hour9 = 0;

                                    }
    

                                    if (optradio.Items[1].Selected == true)
                                    {
                                        
                                        cc++;
                                        string griddate = date_today.ToString("dd") + "/" + date_today.ToString("MM") + "/" + date_today.ToString("yyyy");

                                        if (!cbdispne.Checked)
                                        {
                                            abshrs_ne = string.Empty;
                                        }
                                        string hrval = string.Empty ;
                                        if (!string.IsNullOrEmpty(abshrs_sus) && !string.IsNullOrEmpty(abshrs_ne) && !string.IsNullOrEmpty(abshrs_temp))
                                            hrval = "AB-"+ abshrs_temp + ";" + abshrs_sus + ";" + abshrs_ne;
                                        if (string.IsNullOrEmpty(abshrs_temp) && string.IsNullOrEmpty(abshrs_ne) && !string.IsNullOrEmpty(abshrs_sus))
                                            hrval = abshrs_sus;
                                        if (string.IsNullOrEmpty(abshrs_temp) && string.IsNullOrEmpty(abshrs_sus) && !string.IsNullOrEmpty(abshrs_ne))
                                            hrval = abshrs_ne;
                                        if (!string.IsNullOrEmpty(abshrs_temp) && string.IsNullOrEmpty(abshrs_sus) && string.IsNullOrEmpty(abshrs_ne))
                                            hrval = "AB-"+abshrs_temp;
                                        if (!string.IsNullOrEmpty(abshrs_temp) && !string.IsNullOrEmpty(abshrs_ne) && string.IsNullOrEmpty(abshrs_sus))
                                            hrval = "AB-" + abshrs_temp + ";" + abshrs_ne;
                                        if (!string.IsNullOrEmpty(abshrs_temp) && string.IsNullOrEmpty(abshrs_ne) && !string.IsNullOrEmpty(abshrs_sus))
                                            hrval = "AB-" + abshrs_temp + ";" + abshrs_sus;
                                        if (string.IsNullOrEmpty(abshrs_temp) && !string.IsNullOrEmpty(abshrs_ne) && !string.IsNullOrEmpty(abshrs_sus))
                                            hrval = abshrs_ne + ";" + abshrs_sus;
                                        if (!string.IsNullOrEmpty(splhrval))
                                        {
                                            splhrabs++;
                                            if(string.IsNullOrEmpty(hrval))
                                                 hrval ="SP(" + splhrval + ")";
                                            else
                                                hrval = hrval+ ";SP(" + splhrval + ")";
                                        }

                                        dr[griddate.ToString()] = hrval;

                                        if (abshrs_temp.Trim() == string.Empty && string.IsNullOrEmpty(abshrs_sus) && string.IsNullOrEmpty(abshrs_ne) && string.IsNullOrEmpty(splhrval))
                                        {
                                            dr[griddate.ToString()] = "-";
                                        }

                                        perabsenthrs = perabsenthrs + Convert.ToInt16(absenthrs);
                                        abshrs_ne = string.Empty;
                                        abshrs_sus = string.Empty;
                                        abshrs_temp = string.Empty;


                                    }
                                    //}
                                }//======if i
                            }

                        }
                        //nextday:
                        //---------------------------------------------------------------------------------------------------
                        abshrs_temp = "";
                        date_today = date_today.AddDays(1);

                    }


                    //======================================
                }

            }//=======day

            dat = 1;
            i++;


        }//end month

        perabsenthrs1 = 0;
        if (optradio.Items[1].Selected == true)
        {
            if (perabsenthrs != 0)//cc > 5 &&
            {
                //if (FpSpread1.Sheets[0].RowCount == 1)
                //{
                //    sflag = true;
                //    FpSpread1.Sheets[0].ColumnCount++;
                //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 5, 1, (FpSpread1.Sheets[0].ColumnCount - 6));
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 5].Text = "Absent Hour";
                //    FpSpread1.Sheets[0].Columns[(cc)].HorizontalAlign = HorizontalAlign.Center;
                //    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), (cc)].Text = "Total Absent Hours";
                //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), (cc), 2, 1);


                //}
                //if (cc < FpSpread1.Sheets[0].ColumnCount)
                //{
                //    FpSpread1.Sheets[0].Cells[rc, (cc)].Text = perabsenthrs.ToString();
                perabsenthrs1 = perabsenthrs;
                //}

                //  FpSpread1.Sheets[0].Columns[cc].Width = 100;//
            }
            else
            {
                //FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount - 1;
                //sno--;
                //if (FpSpread1.Sheets[0].RowCount == 0)
                //{
                //    FpSpread1.Sheets[0].ColumnCount = 5;
                //}
            }
            //  }//============for i
        }

        perabsent = absent;
        present = 0;
        presenthrs = 0;
        absent = 0;
        absenthrs = 0;
        totpresentday = 0;

    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        LabelE.Visible = false;
        lblnote.Visible = false;
        pageset_pnl.Visible = false;
        GridView1.Visible = false;
        GridView2.Visible = false;
        GridView3.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        norecordlbl.Visible = false;
        tofromlbl.Visible = false;
        if (ddlformat.SelectedItem.Text == "Hourwise Absentees")
        {
            subject();
        }
       
    }
    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblnote.Visible = false;
        pageset_pnl.Visible = false;
        GridView1.Visible = false;
        GridView2.Visible = false;
        GridView3.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        norecordlbl.Visible = false;
        tofromlbl.Visible = false;
        if (ddlformat.SelectedItem.Text == "Hourwise Absentees")
        {
            subject();
        }
    }


    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        pageset_pnl.Visible = false;
        GridView1.Visible = false;
        GridView2.Visible = false;
        GridView3.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        norecordlbl.Visible = false;
        tofromlbl.Visible = false;

        //added by annyutha//
        DateTime dtnow = DateTime.Now;
        norecordlbl.Visible = false;
        string datefad, dtfromad;
        string datefromad;
        string yr4, m4, d4;
        datefad = txtFromDate.Text.ToString();
        string[] split4 = datefad.Split(new Char[] { '/' });
        if (split4.Length == 3)
        {
            datefromad = split4[0].ToString() + "/" + split4[1].ToString() + "/" + split4[2].ToString();
            yr4 = split4[2].ToString();
            m4 = split4[1].ToString();
            d4 = split4[0].ToString();
            dtfromad = m4 + "/" + d4 + "/" + yr4;
            DateTime dt1 = Convert.ToDateTime(dtfromad);
            if (dt1 > dtnow)
            {
                GridView1.Visible = false;
                GridView2.Visible = false;
                GridView3.Visible = false;
                norecordlbl.Text = "From Date Can't Be Greater Than To Date";
                norecordlbl.Visible = true;
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyy");

            }
            else
            {
                norecordlbl.Visible = false;
                GridView1.Visible = false;
                GridView2.Visible = false;
                GridView3.Visible = false;
            }
        }
    }
    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBoxother.Text = "";
        LabelE.Visible = false;
        if (DropDownListpage.Text == "Others")
        {

            TextBoxother.Visible = true;
            TextBoxother.Focus();

        }
        else
        {
            TextBoxother.Visible = false;
        }
    }

    protected void TextBoxpage_TextChanged(object sender, EventArgs e)
    {
        if (TextBoxpage.Text.Trim() != string.Empty)
        {
            if (Convert.ToInt64(TextBoxpage.Text) > Convert.ToInt64(Session["totalPages"]))
            {
                LabelE.Visible = true;
                LabelE.Text = "Exceed The Page Limit";
                btnxl.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = true;

                TextBoxpage.Text = "";
            }
            else if (Convert.ToInt64(TextBoxpage.Text) == 0)
            {
                LabelE.Visible = true;
                LabelE.Text = "Page search should be more than 0";
                //FpSpread1.Visible = true;
                btnxl.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = true;
                TextBoxpage.Text = "";
            }

            else
            {
                LabelE.Visible = false;
                //FpSpread1.CurrentPage = Convert.ToInt16(TextBoxpage.Text) - 1;
                //FpSpread1.Visible = true;
                btnxl.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = true;
            }
        }
    }
    protected void TextBoxother_TextChanged(object sender, EventArgs e)
    {
        //FpSpread1.CurrentPage = 0;
        TextBoxpage.Text = "";
        LabelE.Visible = false;
        try
        {
            //if (FpSpread1.Sheets[0].RowCount >= Convert.ToInt16(TextBoxother.Text.ToString()) && Convert.ToInt16(TextBoxother.Text.ToString()) != 0)
            //{
            //    if (TextBoxother.Text != string.Empty)
            //    {
            //        LabelE.Visible = false;
            //        FpSpread1.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
            //        CalculateTotalPages();
            //    }
            //}
            //else
            //{
            //    LabelE.Visible = true;
            //    LabelE.Text = "Please Enter valid Record count";
            //    TextBoxother.Text = "";
            //}
        }
        catch
        {
            norecordlbl.Visible = true;
            norecordlbl.Text = "Please Enter valid Record count";
            TextBoxother.Text = "";
        }

    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        // pagesetpanel.Visible = false;
        pageset_pnl.Visible = false;
        GridView1.Visible = false;
        GridView2.Visible = false;
        GridView3.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        norecordlbl.Visible = false;
        tofromlbl.Visible = false;
        DateTime dtnow = DateTime.Now;
        norecordlbl.Visible = false;
        string datefad, dtfromad;
        string datefromad;
        string yr4, m4, d4;
        datefad = txtToDate.Text.ToString();
        string[] split4 = datefad.Split(new Char[] { '/' });
        if (split4.Length == 3)
        {
            datefromad = split4[0].ToString() + "/" + split4[1].ToString() + "/" + split4[2].ToString();
            yr4 = split4[2].ToString();
            m4 = split4[1].ToString();
            d4 = split4[0].ToString();
            dtfromad = m4 + "/" + d4 + "/" + yr4;
            DateTime dt1 = Convert.ToDateTime(dtfromad);
            if (dt1 > dtnow)
            {
                GridView1.Visible = false;
                GridView2.Visible = false;
                GridView3.Visible = false;
                norecordlbl.Text = "Date Can't Be Greater Than To Date";
                norecordlbl.Visible = true;
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyy");

            }
            else
            {
                norecordlbl.Visible = false;
                GridView1.Visible = false;
                GridView2.Visible = false;
                GridView3.Visible = false;
            }
        }
    }

    public void from_today()
    {

        hat.Clear();
        hat.Add("f_date", int.Parse(fcal.ToString()));
        hat.Add("t_date", int.Parse(tcal.ToString()));
        hat.Add("roll_no", ds.Tables[0].Rows[i]["ROLL NO"].ToString());

        ds1 = dacces2.select_method("ATT_REPORTS_DETAILS", hat, "sp");
        dat = fd;
    }


    public void findholy()
    {
        hat.Clear();
        hat.Add("date_val", date_today);
        hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
        hat.Add("sem_val", ddlduration.SelectedValue.ToString());
        ds_holi = dacces2.select_method("holiday_sp", hat, "sp");
    }


    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {


        //norecordlbl .Visible = false;
        //if (RadioHeader.Checked == true)
        //{

        //    for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
        //    {
        //        FpSpread1.Sheets[0].Rows[i].Visible = false;
        //    }
        //    int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
        //    int end = start + 24;
        //    if (end >= FpSpread1.Sheets[0].RowCount)
        //    {
        //        end = FpSpread1.Sheets[0].RowCount;
        //    }
        //    int rowstart = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(start);
        //    int rowend = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(end);
        //    for (int i = start - 1; i < end; i++)
        //    {
        //        FpSpread1.Sheets[0].Rows[i].Visible = true;
        //    }
        //    FpSpread1.Sheets[0].ColumnHeader.Rows[0].Visible = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Rows[3].Visible = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Rows[4].Visible = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Rows[5].Visible = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Rows[6].Visible = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Rows[7].Visible = true;

        //}
        //else if (Radiowithoutheader.Checked == true)
        //{

        //    for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
        //    {
        //        FpSpread1.Sheets[0].Rows[i].Visible = false;
        //    }
        //    int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
        //    int end = start + 24;
        //    if (end >= FpSpread1.Sheets[0].RowCount)
        //    {
        //        end = FpSpread1.Sheets[0].RowCount;
        //    }
        //    int rowstart = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(start);
        //    int rowend = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(end);
        //    for (int i = start - 1; i < end; i++)
        //    {
        //        FpSpread1.Sheets[0].Rows[i].Visible = true;
        //    }
        //    if (Convert.ToInt32(ddlpage.SelectedValue.ToString()) == 1)
        //    {
        //        FpSpread1.Sheets[0].ColumnHeader.Rows[0].Visible = true;
        //        FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = true;
        //        FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = true;
        //        FpSpread1.Sheets[0].ColumnHeader.Rows[3].Visible = true;
        //        FpSpread1.Sheets[0].ColumnHeader.Rows[4].Visible = true;
        //        FpSpread1.Sheets[0].ColumnHeader.Rows[5].Visible = true;
        //        FpSpread1.Sheets[0].ColumnHeader.Rows[6].Visible = true;
        //        FpSpread1.Sheets[0].ColumnHeader.Rows[7].Visible = true;
        //    }
        //    else
        //    {
        //        FpSpread1.Sheets[0].ColumnHeader.Rows[0].Visible = false;
        //        FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = false;
        //        FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = false;
        //        FpSpread1.Sheets[0].ColumnHeader.Rows[3].Visible = false;
        //        FpSpread1.Sheets[0].ColumnHeader.Rows[4].Visible = false;
        //        FpSpread1.Sheets[0].ColumnHeader.Rows[5].Visible = false;
        //        FpSpread1.Sheets[0].ColumnHeader.Rows[6].Visible = false;
        //        FpSpread1.Sheets[0].ColumnHeader.Rows[7].Visible = false;
        //    }

        //}
        //if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
        //{


        //    FpSpread1.Sheets[0].ColumnHeader.Rows[0].Visible = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Rows[3].Visible = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Rows[4].Visible = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Rows[5].Visible = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Rows[6].Visible = true;
        //    FpSpread1.Sheets[0].ColumnHeader.Rows[7].Visible = true;

        //    for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
        //    {
        //        FpSpread1.Sheets[0].Rows[i].Visible = true;
        //    }
        //    Double totalRows = 0;
        //    totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
        //    Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);
        //    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        //    DropDownListpage.Items.Clear();
        //    if (totalRows >= 10)
        //    {
        //        FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
        //        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
        //        {
        //            DropDownListpage.Items.Add((k + 10).ToString());
        //        }
        //        DropDownListpage.Items.Add("Others");
        //        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
        //        FpSpread1.Height = 335;

        //    }
        //    else if (totalRows == 0)
        //    {
        //        DropDownListpage.Items.Add("0");
        //        FpSpread1.Height = 100;
        //    }
        //    else
        //    {
        //        FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
        //        DropDownListpage.Items.Add(FpSpread1.Sheets[0].PageSize.ToString());
        //        FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
        //    }
        //    if (Convert.ToInt32(FpSpread1.Sheets[0].RowCount) > 10)
        //    {
        //        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
        //        FpSpread1.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
        //        //  FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
        //        CalculateTotalPages();
        //    }
        //    pageset_pnl.Visible = true;
        //}
        //else
        //{
        //    pageset_pnl.Visible = false;

        //}
        norecordlbl.Visible = false;
        if (view_header == "0")
        {

            //    for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            //    {
            //        FpSpread1.Sheets[0].Rows[i].Visible = false;
            //    }
            //    int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
            //    int end = start + 24;
            //    if (end >= FpSpread1.Sheets[0].RowCount)
            //    {
            //        end = FpSpread1.Sheets[0].RowCount;
            //    }
            //    int rowstart = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(start);
            //    int rowend = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(end);
            //    for (int i = start - 1; i < end; i++)
            //    {
            //        FpSpread1.Sheets[0].Rows[i].Visible = true;
            //    }
            //    for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
            //    {
            //        FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
            //    }

            //}
            //else if (view_header == "1")
            //{

            //    for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            //    {
            //        FpSpread1.Sheets[0].Rows[i].Visible = false;
            //    }
            //    int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
            //    int end = start + 24;
            //    if (end >= FpSpread1.Sheets[0].RowCount)
            //    {
            //        end = FpSpread1.Sheets[0].RowCount;
            //    }
            //    int rowstart = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(start);
            //    int rowend = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(end);
            //    for (int i = start - 1; i < end; i++)
            //    {
            //        FpSpread1.Sheets[0].Rows[i].Visible = true;
            //    }
            //    if (Convert.ToInt32(ddlpage.SelectedValue.ToString()) == 1)
            //    {
            //        for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
            //        {
            //            FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
            //        }
            //    }
            //    else
            //    {
            //        for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
            //        {
            //            FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
            //        }
            //    }
        }
        else
        {
            //for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            //{
            //    FpSpread1.Sheets[0].Rows[i].Visible = false;
            //}
            //int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
            //int end = start + 24;
            //if (end >= FpSpread1.Sheets[0].RowCount)
            //{
            //    end = FpSpread1.Sheets[0].RowCount;
            //}
            //int rowstart = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(start);
            //int rowend = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(end);
            //for (int i = start - 1; i < end; i++)
            //{
            //    FpSpread1.Sheets[0].Rows[i].Visible = true;
            //}

            //{
            //    for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
            //    {
            //        FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
            //    }
            //}
        }
        if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
        {

            //if (view_header == "1" || view_header == "0")
            //{
            //    for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
            //    {
            //        FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
            //    }
            //}
            //else
            //{
            //    for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
            //    {
            //        FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
            //    }
            //}

            //for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            //{
            //    FpSpread1.Sheets[0].Rows[i].Visible = true;
            //}
            //Double totalRows = 0;
            //totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
            //Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);
            //Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
            //DropDownListpage.Items.Clear();
            //if (totalRows >= 10)
            //{
            //    FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
            //    for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
            //    {
            //        DropDownListpage.Items.Add((k + 10).ToString());
            //    }
            //    DropDownListpage.Items.Add("Others");
            //    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
            //    FpSpread1.Height = 335;
            //}
            //else if (totalRows == 0)
            //{
            //    DropDownListpage.Items.Add("0");
            //    FpSpread1.Height = 100;
            //}
            //else
            //{
            //    FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
            //    DropDownListpage.Items.Add(FpSpread1.Sheets[0].PageSize.ToString());
            //    FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
            //}
            //if (Convert.ToInt32(FpSpread1.Sheets[0].RowCount) > 10)
            //{
            //    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
            //    FpSpread1.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
            //    //  FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
            //    CalculateTotalPages();
        }

        pageset_pnl.Visible = false;
        // }
        //else
        //{
        //    pageset_pnl.Visible = false;
        //}

        //if (view_footer_text != "")
        //{
        //    if (view_footer == "0")
        //    {
        //        FpSpread1.Sheets[0].Rows[(FpSpread1.Sheets[0].RowCount - 1)].Visible = true;
        //        FpSpread1.Sheets[0].Rows[(FpSpread1.Sheets[0].RowCount - 2)].Visible = true;
        //        FpSpread1.Sheets[0].Rows[(FpSpread1.Sheets[0].RowCount - 3)].Visible = true;
        //    }
        //    else
        //    {
        //        if (ddlpage.Text != "")
        //        {
        //            if (ddlpage.SelectedIndex != ddlpage.Items.Count - 1)
        //            {
        //                FpSpread1.Sheets[0].Rows[(FpSpread1.Sheets[0].RowCount - 1)].Visible = false;
        //                FpSpread1.Sheets[0].Rows[(FpSpread1.Sheets[0].RowCount - 2)].Visible = false;
        //                FpSpread1.Sheets[0].Rows[(FpSpread1.Sheets[0].RowCount - 3)].Visible = false;
        //            }
        //        }
        //    }
        //}
    }
    public string sem_roman(int sem)
    {
        string sql = "";
        string sem_roman = "";
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
    protected void btnPrint_Click(object sender, EventArgs e)
    {

        string batch = "", sections = "", semester = "", degreecode = "", subcolumntext = "", strsec = "";
        Boolean child_flag = false;
        int sec_index = 0, sem_index = 0;
        batch = ddlbatch.SelectedValue.ToString();
        sections = ddlsec.SelectedValue.ToString();
        semester = ddlduration.SelectedValue.ToString();
        degreecode = ddlbranch.SelectedValue.ToString();


        if (ddlsec.Text == "")
        {
            strsec = "";
        }
        else
        {
            if (ddlsec.SelectedItem.ToString() == "")
            {
                strsec = "";
            }
            else
            {
                strsec = " - " + ddlsec.SelectedItem.ToString();
            }
        }


        if (ddlsec.Enabled == false)
        {
            sec_index = -1;
        }
        else
        {
            sec_index = ddlsec.SelectedIndex;
        }

        if (ddlduration.Enabled == false)
        {
            sem_index = -1;
        }
        else
        {
            sem_index = ddlduration.SelectedIndex;
        }

        Session["page_redirect_value"] = ddlbatch.SelectedIndex + "," + ddldegree.SelectedIndex + "," + ddlbranch.SelectedIndex + "," + sem_index + "," + sec_index + "," + txtFromDate.Text + "," + txtToDate.Text + "," + optradio.Items[0].Selected + "," + optradio.Items[1].Selected;

        // first_btngo();
        btnGo_Click(sender, e);

        //if (tofromlbl.Visible == false)
        //{
        //    lblpage.Visible = false;
        //    ddlpage.Visible = true;
        //    string clmnheadrname = "";
        //    int total_clmn_count = FpSpread1.Sheets[0].ColumnCount;

        //    for (int srtcnt = 0; srtcnt < total_clmn_count; srtcnt++)
        //    {
        //        if (FpSpread1.Sheets[0].Columns[srtcnt].Visible == true)
        //        {
        //            if (FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text != "")
        //            {
        //                subcolumntext = "";
        //                if (clmnheadrname == "")
        //                {
        //                    clmnheadrname = FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
        //                }
        //                else
        //                {
        //                    if (child_flag == false)
        //                    {
        //                        clmnheadrname = clmnheadrname + "," + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
        //                    }
        //                    else
        //                    {
        //                        clmnheadrname = clmnheadrname + "$)," + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
        //                    }

        //                }
        //                child_flag = false;
        //            }
        //            else
        //            {
        //                child_flag = true;
        //                if (subcolumntext == "")
        //                {
        //                    for (int te = srtcnt - 1; te <= srtcnt; te++)
        //                    {
        //                        if (te == srtcnt - 1)
        //                        {
        //                            clmnheadrname = clmnheadrname + "* ($" + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
        //                            subcolumntext = clmnheadrname + "* ($" + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
        //                        }
        //                        else
        //                        {
        //                            clmnheadrname = clmnheadrname + "$" + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
        //                            subcolumntext = clmnheadrname + "$" + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, te].Text;

        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    subcolumntext = subcolumntext + "$" + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
        //                    clmnheadrname = clmnheadrname + "$" + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
        //                }
        //            }
        //        }
        //    }
        //    //   Session["columnheader_value"]=clmnheadrname.ToString()+":AbsenteeRt.aspx :" + batch + "-" + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "-" + ddlduration.SelectedItem.ToString() + strsec + ": Day and Hour Wise Attendance Report";
        //    //  Response.Write("<script>window.open('Print_Master_Setting.aspx')</script>");
        //    Response.Redirect("Print_Master_Setting_new.aspx?ID=" + clmnheadrname.ToString() + ":" + "AbsenteeRt.aspx" + ":" + ddlbatch.SelectedItem.ToString() + " Batch - " + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] " + strsec + " :" + "Day and Hour Wise Attendance Report");
        //}
        //else
        //{
        //    lblpage.Visible = false;
        //    ddlpage.Visible = false;
        //}
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            norecordlbl.Visible = false;
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                if (ddlformat.SelectedIndex == 0)
                {
                    d2.printexcelreportgrid(GridView1, reportname);
                }
                else if (ddlformat.SelectedIndex == 1)
                {
                    d2.printexcelreportgrid(GridView2, reportname);
                }
                else
                {
                    d2.printexcelreportgrid(GridView3, reportname);
                }

            }
            else
            {
                norecordlbl.Text = "Please Enter Your Report Name";
                norecordlbl.Visible = true;
            }

        }
        catch (Exception ex)
        {
            norecordlbl.Text = ex.ToString();
        }
    }
  
    public void getspecial_hr()
    {

        string hrdetno = "";
        splhrval = string.Empty;
        if (ht_sphr.Contains(Convert.ToString(date_today)))
        {
            hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(date_today), ht_sphr));

        }
        if (hrdetno != "")
        {
            //**********modified by annyutha**2/09/2014*****//
            DataSet ds_splhr_query_master = new DataSet();
            string splhr_query_master = "select attendance,sa.hrdet_no,CONVERT(VARCHAR(5),start_time,108) as start_time, CONVERT(VARCHAR(5),end_time,108) as end_time from specialhr_attendance sa,specialhr_details sd where roll_no='" + ds.Tables[0].Rows[count]["ROLL NO"].ToString() + "'  and sa.hrdet_no in(" + hrdetno + ") and sd.hrdet_no=sa.hrdet_no";

            DateTime dtActualPeriodStartTime = new DateTime();
            DateTime dtActualPeriodendTime = new DateTime();

            ds_splhr_query_master = dacces2.select_method_wo_parameter(splhr_query_master, "text");
            if (ds_splhr_query_master.Tables[0].Rows.Count > 0)
            {
                if (absent_hash.Contains(ds_splhr_query_master.Tables[0].Rows[0]["attendance"].ToString()))
                {
                    dayflag = true;
                   
                    for (int j = 0; j < ds_splhr_query_master.Tables[0].Rows.Count; j++)
                    {
                        string atnd = Convert.ToString(ds_splhr_query_master.Tables[0].Rows[j]["attendance"]);
                        if (!string.IsNullOrEmpty(atnd) && atnd != "0")
                        {
                            string timmrng = Convert.ToString(ds_splhr_query_master.Tables[0].Rows[j]["start_time"]);
                            string timeaft = Convert.ToString(ds_splhr_query_master.Tables[0].Rows[j]["end_time"]);

                            
                            if (string.IsNullOrEmpty(splhrval))
                            {

                                splhrval = timmrng + "-" + timeaft;
                            }
                            else
                            {
                                splhrval = splhrval + "," + timmrng + "-" + timeaft;
                            }

                            perabsenthrs = perabsenthrs + 1;

                        }



                    }
                   
                   
                  //  absenthrs = perabsenthrs;
                }
            }
            //***********end*********//

        }
    }

    #endregion

    protected void btnGo_Click(object sender, EventArgs e)
    {

        try
        {
            GridView1.Visible = false;
            GridView2.Visible = false;
            GridView3.Visible = false;
            LabelE.Visible = false;
            LabelE.Text = string.Empty;
            if (ddlformat.SelectedValue.ToString() == "Absentees")
            {

                if (optradio.Items[0].Selected == true || optradio.Items[1].Selected == true)
                {
                    first_btngo();
                }
                else
                {
                    norecordlbl.Visible = true;
                    norecordlbl.Text = "Select Atleast One CheckBox";
                    GridView1.Visible = false;
                    GridView2.Visible = false;
                    GridView3.Visible = false;
                    btnxl.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    Printcontrol.Visible = false;
                    btnprintmaster.Visible = false;
                    norecordlbl.Visible = true;
                    pageset_pnl.Visible = false;
                    return;
                }

                if (sflag == false)
                {
                    GridView1.Visible = false;
                    GridView2.Visible = false;
                    GridView3.Visible = false;
                    btnxl.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    Printcontrol.Visible = false;
                    btnprintmaster.Visible = false;
                    norecordlbl.Visible = true;
                    pageset_pnl.Visible = false;
                    if (tofromlbl.Visible == false)
                    {
                        norecordlbl.Text = "No Record(s) Found";
                    }
                }
                else
                {
                    if (unmark_flag == true)
                    {
                        tofromlbl.Visible = false;
                        // FpSpread1.Visible = true;
                        GridView1.Visible = true;
                        GridView2.Visible = false;
                        GridView3.Visible = false;
                        btnxl.Visible = true;
                        lblrptname.Visible = true;
                        txtexcelname.Visible = true;
                        Printcontrol.Visible = false;
                        btnprintmaster.Visible = true;
                        norecordlbl.Visible = false;
                        pageset_pnl.Visible = false;
                        //setheader();//Hiden By Srinath 14/5/2013
                        //view_header_setting();
                    }
                }
                if (unmark_flag == false)
                {
                    // FpSpread1.Visible = false;
                    GridView1.Visible = false;
                    GridView2.Visible = false;
                    GridView3.Visible = false;
                    btnxl.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    Printcontrol.Visible = false;
                    btnprintmaster.Visible = false;
                    pageset_pnl.Visible = false;
                    if (tofromlbl.Visible == true)
                    {
                        norecordlbl.Visible = false;
                        lblpage.Visible = false;
                        ddlpage.Visible = false;
                    }
                    else
                    {
                        norecordlbl.Visible = true;
                        norecordlbl.Text = "Attendance Till Not Marked Between Given Dates";
                    }
                }
                //  FpSpread1.Visible = true;
            }
            else if (ddlformat.SelectedValue.ToString() == "General")
            {

                methodgpformat2();

            }
            else if (ddlformat.SelectedValue.ToString() == "Hourwise Absentees")
            {
                absentees();
            }

        }

        catch
        {

        }
    }
    //added by annyutha//
    public void absentees()
    {
        Hashtable subjectht = new Hashtable();
        if (ddlsubject.Items.Count > 0)
        {
            for (int jsub = 0; jsub < ddlsubject.Items.Count; jsub++)
            {
                if (ddlsubject.Items[jsub].Selected == true)
                {
                    if (!subjectht.Contains(ddlsubject.Items[jsub].Value))
                    {
                        subjectht.Add(ddlsubject.Items[jsub].Value, ddlsubject.Items[jsub].Value);
                    }
                }
            }
        }

        has.Clear();
        string dfrom = "", dto = "", tempfromd = "", temptod = "";
        string strsec = "";
        int days = 0;
        int cal_from_date;
        int cal_to_date;

        DateTime dattimefrom = new DateTime();
        DateTime dattimeto = new DateTime();
        DataSet dsstude = new DataSet();


        arrColHdrNames1.Add("S.No");
        arrColHdrNames1.Add("Roll No");
        arrColHdrNames1.Add("Reg No");
        arrColHdrNames1.Add("Student Name");
        arrColHdrNames1.Add("Date/Hour");
        arrColHdrNames1.Add("Day");

        dtcont.Columns.Add("S.No");
        dtcont.Columns.Add("Roll_No");
        dtcont.Columns.Add("RegisterNo");
        dtcont.Columns.Add("StudentName");
        dtcont.Columns.Add("Date/Hour");
        dtcont.Columns.Add("Day");

        string sqlsquery = "select No_of_hrs_per_day,schorder,no_of_hrs_I_half_day,no_of_hrs_II_half_day from PeriodAttndSchedule where degree_code='" + ddlbranch.SelectedValue + "' and semester='" + ddlduration.SelectedValue + "'";
        DataSet df = new DataSet();
        df = dacces2.select_method_wo_parameter(sqlsquery, "text");
        monthyr = Convert.ToInt32(df.Tables[0].Rows[0]["no_of_hrs_I_half_day"]);
        no_of_hrs = Convert.ToInt32(df.Tables[0].Rows[0]["No_of_hrs_per_day"]);
        evng_hrs = Convert.ToInt32(df.Tables[0].Rows[0]["no_of_hrs_II_half_day"]);
        order = df.Tables[0].Rows[0]["schorder"].ToString();
        if (df.Tables[0].Rows.Count > 0)
        {
            for (int j = 1; j <= Convert.ToInt32(df.Tables[0].Rows[0]["No_of_hrs_per_day"]); j++)
            {
                arrColHdrNames1.Add(j.ToString());
                dtcont.Columns.Add(j.ToString());
            }
        }

        DataRow drHdr1 = dtcont.NewRow();
        for (int grCol = 0; grCol < dtcont.Columns.Count; grCol++)
            drHdr1[grCol] = arrColHdrNames1[grCol];
        dtcont.Rows.Add(drHdr1);

        hat.Add("colege_code", Session["collegecode"].ToString());
        ds_attndmaster = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
        count_master = (ds_attndmaster.Tables[0].Rows.Count);
        if (count_master > 0)
        {
            for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
            {

                if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                {
                    if (absent_calcflag == "")
                    {
                        absent_calcflag = ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString();
                        if (!absent_hash.Contains(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                        {
                            absent_hash.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                        }
                    }
                    else
                    {
                        absent_calcflag = absent_calcflag + "," + ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString();
                        if (!absent_hash.Contains(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                        {
                            absent_hash.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                        }
                    }
                }
            }
        }
        dfrom = txtFromDate.Text.ToString();
        string[] split = dfrom.Split(new Char[] { '/' });
        tempfromd = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
        cal_from_date = (Convert.ToInt32(split[1].ToString()) + (Convert.ToInt32(split[2].ToString()) * 12));

        dto = txtToDate.Text.ToString();
        string[] splitto = dto.Split(new Char[] { '/' });
        temptod = splitto[1].ToString() + "-" + splitto[0].ToString() + "-" + splitto[2].ToString();
        cal_to_date = (Convert.ToInt32(splitto[1].ToString()) + (Convert.ToInt32(splitto[2].ToString()) * 12));

        dattimefrom = Convert.ToDateTime(tempfromd);
        dattimeto = Convert.ToDateTime(temptod);
        strsec = ddlsec.Text;
        string strdayflag = "";
        if (ddlsec.Text.ToString() == "All" || ddlsec.Text.ToString() == string.Empty || ddlsec.Text.ToString() == "-1")
        {
            strsec = "";
            strdayflag = "";

        }
        else
        {
            strsec = " and registration.sections='" + ddlsec.Text.ToString() + "'";
            strdayflag = "and sections='" + ddlsec.Text.ToString() + "'";
        }
        string strsec1 = "";
        string rstrsec1 = "";
        string splhrsec1 = "";
        if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1" || ddlsec.SelectedValue.ToString() == "All")
        {
            strsec1 = "";
            rstrsec1 = "";
            splhrsec1 = "";
        }
        else
        {
            strsec1 = " and sections='" + ddlsec.SelectedItem.ToString() + "'";
            rstrsec1 = " and r.sections='" + ddlsec.SelectedItem.ToString() + "'";
            splhrsec1 = "and sections='" + ddlsec.SelectedItem.ToString() + "'";
        }


        string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
        string strorder = "registration.roll_no";
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
        string sections = "";
        if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1" || ddlsec.SelectedValue.ToString() == "All")
        {
            sections = "";

        }
        else
        {
            sections = ddlsec.SelectedItem.ToString();
        }

        DataSet dsmark = new DataSet();
        DataView dvmark = new DataView();
        has.Clear();
        has.Add("colege_code", Session["collegecode"].ToString());
        ds_attndmaster = dacces2.select_method("ATT_MASTER_SETTING", has, "sp");
        string getdeteails = "select convert(nvarchar(15),s.start_date,101) as start_date,nodays,s.starting_dayorder from seminfo s,PeriodAttndSchedule p where s.degree_code=p.degree_code and p.semester=s.semester and s.semester='" + ddlduration.SelectedItem.ToString() + "' and s.batch_year='" + ddlbatch.Text.ToString() + "'  and s.degree_code='" + ddlbranch.SelectedValue.ToString() + "'";
        getdeteails = getdeteails + ";select subject_code,subject_no,s.subType_no from subject s, sub_sem sb,syllabus_master sm where  sm.syll_code=sb.syll_code and s.syll_code=sb.syll_code and sb.subType_no=s.subType_no and Batch_Year='" + ddlbatch.Text.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester='" + ddlduration.SelectedItem.ToString() + "'";
        getdeteails = getdeteails + "; select subject_type,LAB,subType_no From sub_sem";
        DataSet dssem = d2.select_method_wo_parameter(getdeteails, "Text");
        string semstartdate = "";
        string noofdays = "";
        string startday = "";
        if (dssem.Tables[0].Rows.Count > 0)
        {
            semstartdate = dssem.Tables[0].Rows[0]["start_date"].ToString();
            noofdays = dssem.Tables[0].Rows[0]["nodays"].ToString();
            startday = dssem.Tables[0].Rows[0]["starting_dayorder"].ToString();
        }
        string includediscon = " and delflag=0";
        string getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Discount' and " + grouporusercode + "");
        if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
        {
            includediscon = string.Empty;
        }
        string includedebar = " and exam_flag <> 'DEBAR'";

        getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Debar' and " + grouporusercode + "");
        if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
        {
            includedebar = string.Empty;
        }

        string sqlquery = "select * from Registration  where  Registration.Batch_Year='" + ddlbatch.SelectedValue + "' and CC=0 and DelFlag=0 and Exam_Flag<>'debar' and Registration.degree_code='" + ddlbranch.SelectedValue + "' " + strsec + "" + strorder + " ";
        string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + tempfromd.ToString() + "' and '" + temptod.ToString() + "' and degree_code=" + ddlbranch.SelectedValue + " and semester=" + ddlduration.SelectedValue + "";
        int iscount = 0;
        string sql2 = "select * from attendance a,Registration r where a.roll_no=r.Roll_No and r.CC=0 " + includediscon + " " + includedebar + " and r.Batch_Year='" + ddlbatch.SelectedValue + "' and r.degree_code='" + ddlbranch.SelectedValue + "'";
        ds2 = dacces2.select_method_wo_parameter(sql2, "text");

        //DateTime stdt = DateTime.ParseExact(txtFromDate.Text, "d/MM/yyyy", null);
        //  DateTime endt = DateTime.ParseExact(txtToDate.Text, "d/MM/yyyy", null);

        dfrom = txtFromDate.Text.ToString();
        string[] split1 = dfrom.Split(new Char[] { '/' });
        tempfromd = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
        DateTime stdt = Convert.ToDateTime(tempfromd);

        dto = txtToDate.Text.ToString();
        string[] split2 = dto.Split(new Char[] { '/' });
        temptod = split2[1].ToString() + "/" + split2[0].ToString() + "/" + split2[2].ToString();
        DateTime endt = Convert.ToDateTime(temptod);
        string hrdetno = "";
        string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " " + splhrsec + " and date between '" + stdt.ToString() + "' and '" + endt.ToString() + "'";
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
        DataSet dsholiday = new DataSet();
        dsholiday = dacces2.select_method_wo_parameter(sqlstr_holiday, "text");
        if (dsholiday.Tables[0].Rows.Count > 0)
        {
            iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
        }
        hat.Clear();
        hat.Add("degree_code", int.Parse(ddlbranch.SelectedValue));
        hat.Add("sem", int.Parse(ddlduration.SelectedValue));
        hat.Add("from_date", dattimefrom.ToString("yyyy/MM/dd"));
        hat.Add("to_date", dattimeto.ToString("yyyy/MM/dd"));
        hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));
        hat.Add("iscount", iscount);
        dsholiday = dacces2.select_method("HOLIDATE_DETAILS_FINE", hat, "sp");
        Hashtable dsholi = new Hashtable();
        if (dsholiday.Tables[0].Rows.Count > 0 && dsholiday.Tables != null && dsholiday != null)
        {
            for (int ho = 0; ho < dsholiday.Tables[0].Rows.Count; ho++)
            {
                if (!dsholi.Contains(dsholiday.Tables[0].Rows[ho][0].ToString()))
                {

                    dsholi.Add(dsholiday.Tables[0].Rows[ho][0].ToString(), dsholiday.Tables[0].Rows[ho]["halforfull"].ToString() + "," + dsholiday.Tables[0].Rows[ho]["morning"].ToString() + "," + dsholiday.Tables[0].Rows[ho]["evening"].ToString());
                }
            }
        }
        stabsteen = dacces2.select_method_wo_parameter(sqlquery, "text");
        DataSet ds_alter1 = new DataSet();
        ds_alter1.Clear();
        ds_alter1.Dispose();
        ds_alter1.Reset();

        string alterquery = "select  * from alternate_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedItem.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + "  " + strdayflag + " order by FromDate Desc";
        alterquery = alterquery + "; select r.roll_no,s.subject_no,s.batch,r.adm_date from registration r,subjectchooser s where s.roll_no=r.roll_no and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' " + rstrsec1 + "";
        alterquery = alterquery + "; select r.roll_no,s.subject_no,s.batch,r.adm_date from registration r,subjectchooser_new s where s.roll_no=r.roll_no and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' " + rstrsec1 + "";
        alterquery = alterquery + "; select day_value,hour_value,stu_batch,subject_no,timetablename from laballoc where batch_year='" + ddlbatch.SelectedValue.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "'  " + strsec1 + "";
        alterquery = alterquery + "; select day_value,hour_value,stu_batch,subject_no,fdate from laballoc_new where batch_year='" + ddlbatch.SelectedValue.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' " + strsec1 + " ";
        ds_alter1 = dacces2.select_method(alterquery, hat, "Text");
        ds.Clear();
        ds.Dispose();
        ds.Reset();
        string query = "select * from semester_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedItem.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + "  " + strdayflag + " order by FromDate Desc";
        ds = dacces2.select_method(query, hat, "Text");
        TimeSpan t = stdt.Subtract(endt);
        days = t.Days;
        Boolean hrf = false;
        while (stdt <= endt)
        {

            if (stdt.ToString("dddd") == "Sunday")
            {
                sunday_holiday = true;
                if (errordate == "")
                {
                    errordate = "" + stdt.ToString("dd-MM-yyyy");

                }
                else
                {
                    errordate = errordate + "," + stdt.ToString("dd-MM-yyyy");
                }
                stdt = stdt.AddDays(1);
            }
            else
            {
                li.Add(stdt);
                stdt = stdt.AddDays(1);

            }
        }
        int sn = 0;
        Boolean checkcondution = false;
        Boolean sn1 = false;
        Boolean sn3 = false;
        if (days > 0)
        {
            norecordlbl.Text = "From Date Should Be Lesser Than To Date";
            norecordlbl.Visible = true;
            GridView1.Visible = false;

        }
        else
        {
            if (stabsteen.Tables[0].Rows.Count > 0)
            {
                int sno = 0;
                for (int k = 0; k < stabsteen.Tables[0].Rows.Count; k++)
                {
                    sn1 = false;
                    count = k;
                    rowflag = false;

                    for (int g = 0; g < li.Count; g++)
                    {
                        sn3 = false;
                        checkcondution = false;
                        if (li[g].ToString("dddd") == "Sunday")
                        {
                            sunday_holiday = true;
                        }
                        else
                        {

                            temp_date = li[g];

                            if (order != "0")
                            {
                                strDay = temp_date.ToString("ddd");
                            }
                            else
                            {
                                dummy_date = temp_date.ToString();
                                string[] sp = dummy_date.Split('/');
                                string curdate = sp[0] + '/' + sp[1] + '/' + sp[2];
                                strDay = d2.findday(curdate, ddlbranch.SelectedValue.ToString(), ddlduration.SelectedItem.ToString(), ddlbatch.Text.ToString(), semstartdate, noofdays, startday);
                            }

                            date_today = temp_date;
                            //if (ht_sphr.Contains(Convert.ToString(temp_date)) && spl_hr_flag == true)
                            //{
                            //    getspecial_hr();

                            //}
                            //else
                            //{
                            int i = 1;
                            string noofhaours = "";

                            if (!dsholi.Contains(li[g].ToString()))
                            {
                                noofhaours = "No_of_hrs_per_day";

                            }
                            else
                            {
                                value_holi_status = GetCorrespondingKey(temp_date, dsholi).ToString();
                                split_holiday_status = value_holi_status.Split(',');
                                if (split_holiday_status[0].ToString() == "False")
                                {
                                    noofhaours = "No_of_hrs_per_day";
                                }
                                else if (split_holiday_status[0].ToString() == "True")
                                {
                                    if (split_holiday_status[1].ToString() == "True")
                                    {
                                        noofhaours = "No_of_hrs_per_day";
                                        i = Convert.ToInt32(df.Tables[0].Rows[0][noofhaours]) - Convert.ToInt32(df.Tables[0].Rows[0]["no_of_hrs_I_half_day"]) + 1;
                                    }
                                    else if (split_holiday_status[2].ToString() == "True")
                                    {
                                        noofhaours = "no_of_hrs_I_half_day";

                                    }

                                }
                            }
                            string sectiontime = stabsteen.Tables[0].Rows[k]["Sections"].ToString();
                            if (sectiontime != "")
                            {
                                sectiontime = " and sections='" + sectiontime + "'";
                            }
                            else
                            {
                                sectiontime = "";
                            }
                            DateTime date2 = li[g];
                            string strDate = date2.ToString("dd/MM/yyyy");
                            string dateday = date2.DayOfWeek.ToString();
                            string[] name = strDate.Split(new char[] { '/' });
                            int yr = (int.Parse(name[2])) * 12;
                            int mn = yr + (int.Parse(name[1]));
                            int dd = Convert.ToInt32(name[0].ToString());
                            DataView dv = new DataView();
                            ds2.Tables[0].DefaultView.RowFilter = "roll_no='" + stabsteen.Tables[0].Rows[k]["Roll_No"].ToString() + "' and month_year='" + mn + "' " + sectiontime + "";
                            dv = ds2.Tables[0].DefaultView;
                            Boolean daa = false;

                            if (dv.Count > 0)
                            {
                                checking.Clear();

                                for (int jk1 = i; jk1 <= Convert.ToInt32(df.Tables[0].Rows[0][noofhaours]); jk1++)
                                {
                                    checking.Add("d" + dd + "d" + jk1 + "", dv[0]["d" + dd + "d" + jk1 + ""].ToString());
                                }

                                foreach (DictionaryEntry entry in absent_hash)
                                {
                                    if (checking.ContainsValue(entry.Value))
                                    {
                                        daa = true;
                                    }
                                }

                                if (daa == true)
                                {
                                    sno++;
                                    hrf = true;
                                    sn3 = true;
                                    dr2 = dtcont.NewRow();
                                    string da = stabsteen.Tables[0].Rows[k]["Roll_No"].ToString();
                                    dr2["S.No"] = sno.ToString();
                                    dr2["Roll_No"] = stabsteen.Tables[0].Rows[k]["Roll_No"].ToString();
                                    dr2["RegisterNo"] = stabsteen.Tables[0].Rows[k]["Reg_No"].ToString();
                                    dr2["StudentName"] = stabsteen.Tables[0].Rows[k]["Stud_Name"].ToString();
                                    dr2["Date/Hour"] = strDate.ToString();
                                    dr2["Day"] = dateday.ToString();

                                    if (i != 1)
                                    {
                                        for (int gh = 1; gh < i; gh++)
                                        {
                                            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, gh + 5].Text = "H";
                                        }
                                    }
                                    else
                                    {
                                        if (noofhaours == "no_of_hrs_I_half_day")
                                        {
                                            int fs = Convert.ToInt32(df.Tables[0].Rows[0]["No_of_hrs_per_day"]) - Convert.ToInt32(df.Tables[0].Rows[0][noofhaours]);
                                            for (int ja = Convert.ToInt32(df.Tables[0].Rows[0][noofhaours]) + 1; ja <= Convert.ToInt32(df.Tables[0].Rows[0]["No_of_hrs_per_day"]); ja++)
                                            {
                                                //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ja + 5].Text = "H";
                                            }
                                        }
                                    }
                                    for (int jk = i; jk <= Convert.ToInt32(df.Tables[0].Rows[0][noofhaours]); jk++)
                                    {

                                        string lsub121 = "";
                                        string valu = checking["d" + dd + "d" + jk + ""].ToString();
                                        if (absent_hash.Contains(valu))
                                        {
                                            DataView ds_alter = new DataView();
                                            ds_alter1.Tables[0].DefaultView.RowFilter = "FromDate='" + date2 + "'  " + sectiontime + "";
                                            ds_alter = ds_alter1.Tables[0].DefaultView;
                                            DataView ds_sem1 = new DataView();
                                            ds.Tables[0].DefaultView.RowFilter = " FromDate <='" + temp_date + "'  " + sectiontime + " ";
                                            ds_sem1 = ds.Tables[0].DefaultView;
                                            if (ds_alter.Count > 0)
                                            {
                                                temp_hr_field = strDay + jk;
                                                full_hour = ds_alter[0][temp_hr_field].ToString();
                                                if (full_hour == "")
                                                {
                                                    string dateper = strDay + jk;
                                                    string full_hour1 = ds_sem1[0][dateper].ToString();
                                                    if (full_hour1 != "")
                                                    {
                                                        string[] split_full_hour_sem12 = full_hour1.Split(';');

                                                        for (int gt = 0; gt <= split_full_hour_sem12.GetUpperBound(0); gt++)
                                                        {
                                                            string[] valhr = split_full_hour_sem12[gt].ToString().Split('-');
                                                            if (valhr.GetUpperBound(0) > 1)
                                                            {
                                                                lsub121 = valhr[0].ToString();
                                                                if (subjectht.Contains(lsub121))
                                                                {
                                                                    DataView dv2 = new DataView();
                                                                    DataView dv1 = new DataView();
                                                                    dssem.Tables[1].DefaultView.RowFilter = "subject_no=" + lsub121 + "";
                                                                    dv1 = dssem.Tables[1].DefaultView;
                                                                    string subj = dv1[0]["subType_no"].ToString();
                                                                    dssem.Tables[2].DefaultView.RowFilter = "subtype_no='" + subj + "'";
                                                                    dv2 = dssem.Tables[2].DefaultView;
                                                                    string subj_type = dv2[0]["Lab"].ToString();
                                                                    if (subj_type != "1" && subj_type != "True" && subj_type != "TRUE" && subj_type.Trim().ToLower() != "true")
                                                                    {
                                                                        ds_alter1.Tables[1].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + lsub121 + "' and roll_no='" + da + "'";
                                                                        DataView dvlabhr = ds_alter1.Tables[1].DefaultView;
                                                                        if (dvlabhr.Count > 0)
                                                                        {
                                                                            string lsub1212 = dv1[0]["subject_code"].ToString();
                                                                            dr2[jk.ToString()] = Convert.ToString(lsub1212);
                                                                            norecord = true;
                                                                            checkcondution = true;
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        ds_alter1.Tables[3].DefaultView.RowFilter = "hour_value=" + jk + " and subject_no='" + lsub121 + "'  and day_value='" + strDay + "' and timetablename='" + ds_sem1[0]["ttname"].ToString() + "'"; ;
                                                                        DataView dvlabbatch = ds_alter1.Tables[3].DefaultView;
                                                                        for (int lb = 0; lb < dvlabbatch.Count; lb++)
                                                                        {
                                                                            string batch = dvlabbatch[lb]["stu_batch"].ToString();
                                                                            if (batch != null && batch.Trim() != "")
                                                                            {

                                                                                ds_alter1.Tables[1].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + lsub121 + "' and batch='" + batch + "' and roll_no='" + da + "' ";
                                                                                DataView dvlabhr = ds_alter1.Tables[1].DefaultView;
                                                                                if (dvlabhr.Count > 0)
                                                                                {
                                                                                    string lsub1212 = dv1[0]["subject_code"].ToString();
                                                                                    dr2[jk.ToString()] = Convert.ToString(lsub1212);
                                                                                    norecord = true;
                                                                                    checkcondution = true;
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
                                                        dr2[jk.ToString()] = "No Period";

                                                    }
                                                }
                                                else if (full_hour != "")
                                                {
                                                    string[] split_full_hour_sem12 = full_hour.Split(';');

                                                    for (int gt = 0; gt <= split_full_hour_sem12.GetUpperBound(0); gt++)
                                                    {
                                                        string[] valhr = split_full_hour_sem12[gt].ToString().Split('-');
                                                        if (valhr.GetUpperBound(0) > 1)
                                                        {

                                                            lsub121 = valhr[0].ToString();
                                                            if (subjectht.Contains(lsub121))
                                                            {
                                                                DataView dv2 = new DataView();
                                                                DataView dv1 = new DataView();
                                                                dssem.Tables[1].DefaultView.RowFilter = "subject_no=" + lsub121 + "";
                                                                dv1 = dssem.Tables[1].DefaultView;
                                                                string subj = dv1[0]["subType_no"].ToString();
                                                                dssem.Tables[2].DefaultView.RowFilter = "subtype_no='" + subj + "'";
                                                                dv2 = dssem.Tables[2].DefaultView;
                                                                string subj_type = dv2[0]["Lab"].ToString();
                                                                if (subj_type != "1" && subj_type != "True" && subj_type != "TRUE" && subj_type.Trim().ToLower() != "true")
                                                                {
                                                                    ds_alter1.Tables[1].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + lsub121 + "' and roll_no='" + da + "'";
                                                                    DataView dvlabhr = ds_alter1.Tables[0].DefaultView;
                                                                    if (dvlabhr.Count > 0)
                                                                    {
                                                                        string lsub1212 = dv1[0]["subject_code"].ToString();
                                                                        dr2[jk.ToString()] = Convert.ToString(lsub1212);
                                                                        norecord = true;
                                                                        checkcondution = true;
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    ds_alter1.Tables[4].DefaultView.RowFilter = "hour_value=" + jk + "  and day_value='" + strDay + "' and subject_no='" + lsub121 + "' and fdate='" + temp_date.ToString("MM/dd/yyyy").ToString() + "'";
                                                                    DataView dvlabbatch = ds_alter1.Tables[4].DefaultView;
                                                                    for (int lb = 0; lb < dvlabbatch.Count; lb++)
                                                                    {
                                                                        string batch = dvlabbatch[lb]["stu_batch"].ToString();
                                                                        if (batch != null && batch.Trim() != "")
                                                                        {
                                                                            ds_alter1.Tables[1].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + lsub121 + "' and batch='" + batch + "' ";
                                                                            DataView dvlabhr = ds_alter1.Tables[1].DefaultView;
                                                                            if (dvlabhr.Count > 0)
                                                                            {
                                                                                string lsub1212 = dv1[0]["subject_code"].ToString();
                                                                                dr2[jk.ToString()] = Convert.ToString(lsub1212);
                                                                                norecord = true;
                                                                                checkcondution = true;
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
                                                    dr2[jk.ToString()] = "No Period";
                                                }

                                            }
                                            else if (ds_sem1.Count > 0)
                                            {
                                                string dateper = strDay + jk;
                                                full_hour = ds_sem1[0][dateper].ToString();
                                                if (full_hour != "")
                                                {
                                                    string[] split_full_hour_sem12 = full_hour.Split(';');

                                                    for (int gt = 0; gt <= split_full_hour_sem12.GetUpperBound(0); gt++)
                                                    {
                                                        string[] valhr = split_full_hour_sem12[gt].ToString().Split('-');
                                                        if (valhr.GetUpperBound(0) > 1)
                                                        {
                                                            lsub121 = valhr[0].ToString();
                                                            if (subjectht.Contains(lsub121))
                                                            {
                                                                DataView dv2 = new DataView();
                                                                DataView dv1 = new DataView();
                                                                dssem.Tables[1].DefaultView.RowFilter = "subject_no=" + lsub121 + "";
                                                                dv1 = dssem.Tables[1].DefaultView;
                                                                string subj = dv1[0]["subType_no"].ToString();
                                                                dssem.Tables[2].DefaultView.RowFilter = "subtype_no='" + subj + "'";
                                                                dv2 = dssem.Tables[2].DefaultView;
                                                                string subj_type = dv2[0]["Lab"].ToString();
                                                                if (subj_type != "1" && subj_type != "True" && subj_type != "TRUE" && subj_type.Trim().ToLower() != "true")
                                                                {
                                                                    ds_alter1.Tables[1].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + lsub121 + "' and roll_no='" + da + "'";
                                                                    DataView dvlabhr = ds_alter1.Tables[1].DefaultView;
                                                                    if (dvlabhr.Count > 0)
                                                                    {
                                                                        string lsub1212 = dv1[0]["subject_code"].ToString();
                                                                        dr2[jk.ToString()] = Convert.ToString(lsub1212);
                                                                        norecord = true;
                                                                        checkcondution = true;
                                                                    }


                                                                }
                                                                else
                                                                {
                                                                    ds_alter1.Tables[3].DefaultView.RowFilter = "hour_value=" + jk + " and subject_no='" + lsub121 + "'  and day_value='" + strDay + "' and timetablename='" + ds_sem1[0]["ttname"].ToString() + "'"; ;
                                                                    DataView dvlabbatch = ds_alter1.Tables[3].DefaultView;
                                                                    for (int lb = 0; lb < dvlabbatch.Count; lb++)
                                                                    {
                                                                        string batch = dvlabbatch[lb]["stu_batch"].ToString();
                                                                        if (batch != null && batch.Trim() != "")
                                                                        {
                                                                            ds_alter1.Tables[1].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + lsub121 + "' and batch='" + batch + "' and roll_no='" + da + "' ";
                                                                            DataView dvlabhr = ds_alter1.Tables[1].DefaultView;
                                                                            if (dvlabhr.Count > 0)
                                                                            {
                                                                                string lsub1212 = dv1[0]["subject_code"].ToString(); dr2[jk.ToString()] = Convert.ToString(lsub1212);
                                                                                norecord = true;
                                                                                checkcondution = true;
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

                                                    dr2[jk.ToString()] = "No Period";

                                                }
                                            }

                                        }
                                        else
                                        {

                                            dr2[jk.ToString()] = "-";

                                        }

                                    }

                                    dtcont.Rows.Add(dr2);
                                }
                                GridView3.DataSource = dtcont;
                                GridView3.DataBind();

                                for (int r = 0; r < GridView3.Rows.Count; r++)
                                {
                                    for (int j1 = 0; j1 < GridView3.HeaderRow.Cells.Count; j1++)
                                    {

                                        //if (r == 0 || r == 1)
                                        //{         
                                            GridView3.Rows[0].Cells[j1].HorizontalAlign = HorizontalAlign.Center;
                                       // }
                                    }
                                }

                                GridView1.Visible = false;
                                GridView2.Visible = false;
                                GridView3.Visible = true;
                                divMainContents.Visible = true;
                                GridView3.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                GridView3.Rows[0].Font.Bold = true;
                                GridView3.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                            }

                        }
                        if (sn3 == true)
                        {
                            if (sn1 == false)
                            {
                                sn++;
                            }

                            sn1 = true;
                        }
                    }

                }


            }
            else
            {
                LabelE.Visible = true;
                LabelE.Text = "No Records Found";
                GridView3.Visible = false;
                GridView2.Visible = false;
                GridView1.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                Printcontrol.Visible = false;
            }

            if (norecord == false)
            {
                LabelE.Visible = true;
                LabelE.Text = "No Records Found";
                GridView3.Visible = false;
                GridView2.Visible = false;
                GridView1.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                Printcontrol.Visible = false;
                return;
            }
            if (sunday_holiday == true)
            {
                norecordlbl.Visible = true;
                norecordlbl.Text = " " + errordate + " Day is Sunday";
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;

            }
            if (GridView3.Rows.Count > 0)
            {
                btnxl.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = true;
                GridView3.Visible = true;
            }
            else
            {
                btnxl.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
                GridView3.Visible = false;
            }
           

        }


    }

    public void first_btngo()
    {
        pageset_pnl.Visible = false;
        // FpSpread1.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        norecordlbl.Visible = false;
        tofromlbl.Visible = false;
        if (txtFromDate.Text == string.Empty)
        {
            tofromlbl.Visible = true;
            tofromlbl.Text = "Select From Date";
        }
        if (txtToDate.Text == string.Empty)
        {
            tofromlbl.Visible = true;
            tofromlbl.Text = "Select To Date";
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

    protected void gridview1_DataBound(object sender, GridViewRowEventArgs e)
    {

        try
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int grCol = 0; grCol < dscount.Columns.Count; grCol++)
                {
                    e.Row.Cells[grCol].Visible = false;
                }
                e.Row.HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                e.Row.HorizontalAlign = HorizontalAlign.Center;

                e.Row.Cells[1].Visible = false;
               
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
               // e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[1].Visible = false;
               // e.Row.HorizontalAlign = HorizontalAlign.Left;
               // e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                
                for (int r = 5; r < dscount.Columns.Count; r++)
                    e.Row.Cells[r].HorizontalAlign = HorizontalAlign.Center;

            }


        }
        catch
        {
        }
    }

    public void gobutton()
    {

        hat.Clear();
      

        //==============================25/5/12 PRABHA
        GridView2.Visible = false;
        GridView3.Visible = false;
        GridView1.Visible = false;
        ds_attndmaster.Clear();
        count_master = 0;
        absent_calcflag = "";
        absent_hash.Clear();
        hat.Add("colege_code", Session["collegecode"].ToString());
        ds_attndmaster = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
        count_master = (ds_attndmaster.Tables[0].Rows.Count);
        if (count_master > 0)
        {
            for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
            {

                if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                {
                    if (absent_calcflag == "")
                    {
                        absent_calcflag = ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString();
                        if (!absent_hash.Contains(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                        {
                            absent_hash.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                        }
                    }
                    else
                    {
                        absent_calcflag = absent_calcflag + "," + ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString();
                        if (!absent_hash.Contains(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                        {
                            absent_hash.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                        }
                    }
                }
            }
        }
        //========================

        hat_days_end.Clear();
        hat_days_first.Clear();
        string tag_val_roll = "";

        try
        {

            {

                hat.Clear();
                hat.Add("college_code", Session["collegecode"].ToString());
                hat.Add("form_name", "AbsenteeRt.aspx");
                dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
                if (dsprint.Tables[0].Rows.Count > 0)
                {

                    lblpage.Visible = false;
                    ddlpage.Visible = true;

                    if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
                    {
                        new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                        new_header_string_split = new_header_string.Split(',');
                    }

                }

                string strsplhrsec = "";
                if (ddlsec.Text.ToString() == "All" || ddlsec.Text.ToString() == string.Empty || ddlsec.SelectedValue.ToString() == "-1")
                {
                    strsec_sub = "";
                    strsplhrsec = "";
                }
                else
                {
                    strsec_sub = " and r.sections='" + ddlsec.SelectedValue.ToString() + "'";
                    strsplhrsec = " and sm.sections='" + ddlsec.SelectedValue.ToString() + "'";
                }


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
                                dt1 = Convert.ToDateTime(datefrom.ToString());
                                dt2 = Convert.ToDateTime(dateto.ToString());
                                TimeSpan t = dt2.Subtract(dt1);
                                long days = t.Days;

                                //------------------------------------------------------
                                ht_sphr.Clear();
                                string hrdetno = "";
                                string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and date between '" + dt1.ToString() + "' and '" + dt2.ToString() + "' " + strsplhrsec + "";
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
                                //------------------------------------------------------
                                if (days >= 0)//-----check date difference
                                {
                                    norecordlbl.Visible = false;
                                    tofromlbl.Visible = false;
                                    //added By Srinath 11/8/2013 
                                    string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                                    string strorder = "ORDER BY 'ROLL NO'";
                                    if (orderby_Setting == "0")
                                    {
                                        strorder = "ORDER BY 'ROLL NO'";
                                    }
                                    else if (orderby_Setting == "1")
                                    {
                                        strorder = "ORDER BY 'REG NO'";
                                    }
                                    else if (orderby_Setting == "2")
                                    {
                                        strorder = "ORDER BY 'STUD NAME'";
                                    }
                                    else if (orderby_Setting == "0,1,2")
                                    {
                                        strorder = "ORDER BY 'ROLL NO','REG NO','STUD NAME'";
                                    }
                                    else if (orderby_Setting == "0,1")
                                    {
                                        strorder = "ORDER BY 'ROLL NO','REG NO'";
                                    }
                                    else if (orderby_Setting == "1,2")
                                    {
                                        strorder = "ORDER BY 'REG NO','STUD NAME'";
                                    }
                                    else if (orderby_Setting == "0,2")
                                    {
                                        strorder = "ORDER BY 'ROLL NO','STUD NAME'";
                                    }
                                    ds.Clear();
                                    con.Close();
                                    con.Open();
                                 
                                    string includediscon = " and delflag=0";
                                    string getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Discount' and " + grouporusercode + "");
                                    if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                                    {
                                        includediscon = string.Empty;
                                    }
                                    string includedebar = " and exam_flag <> 'DEBAR'";

                                    getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Debar' and " + grouporusercode + "");
                                    if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                                    {
                                        includedebar = string.Empty;
                                    }
                                    diccount.Clear();
                                    cmd = new SqlCommand("select distinct a.roll_no as 'ROLL NO', r.Batch_Year as 'BATCH YEAR', r.degree_code as 'DEGREE CODE', r.Sections as 'SECTIONS', r.college_code as 'COLLEGE CODE', d.Dept_Name as 'DEGREE NAME', r.stud_name as 'STUD NAME', r.reg_no as 'REG NO', r.Roll_Admit as 'ROLL ADMIT', p.No_of_hrs_per_day as 'PER DAY', p.no_of_hrs_I_half_day as 'I_HALF_DAY' , p.no_of_hrs_II_half_day as 'II_HALF_DAY', p.min_pres_I_half_day as 'MIN PREE I DAY', p.min_pres_II_half_day as 'MIN PREE II DAY',len(r.roll_no),delflag FROM attendance a, registration r , Department d ,PeriodAttndSchedule p,applyn,Course c,degree de WHERE r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedValue.ToString() + "   " + strsec_sub + "   and r.Current_Semester=" + ddlduration.SelectedItem.ToString() + "  and (r.CC = 0)  " + includediscon + "  " + includedebar + "  AND (r.Current_Semester IS NOT NULL) and p.semester=r.Current_Semester and a.roll_no=r.roll_no  and r.degree_code=p.degree_code  and r.app_no=applyn.app_no  " + Session["strvar"].ToString() + " and de.degree_code=r.degree_code and de.dept_code=d.dept_code " + strorder + "", con);//added by Srinath 
                                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                                    da.Fill(ds);
                                    //===============================

                                    int stud_count = 0;
                                    stud_count = ds.Tables[0].Rows.Count;

                                    if (stud_count > 0)
                                    {
                                        sqlstrq = int.Parse((ds.Tables[0].Rows[0]["PER DAY"].ToString()));
                                        minI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                                        minII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                                        Ihof = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                                        IIhof = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                                        fullday = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());

                                        fd = Convert.ToInt16(dt1.ToString("dd"));
                                        fyy = Convert.ToInt16(dt1.ToString("yyyy"));
                                        fm = Convert.ToInt16(dt1.ToString("MM"));

                                        td = Convert.ToInt16(dt2.ToString("dd"));
                                        tyy = Convert.ToInt16(dt2.ToString("yyyy"));
                                        tm = Convert.ToInt16(dt2.ToString("MM"));

                                        fcal = ((fyy * 12) + fm);
                                        tcal = ((tyy * 12) + tm);
                                        totmonth = fcal;


                                        arrColHdrNames1.Add("S.No");
                                        arrColHdrNames1.Add("Admission No");
                                        arrColHdrNames1.Add("Roll No");
                                        arrColHdrNames1.Add("Register No");
                                        arrColHdrNames1.Add("Student Name");


                                        arrColHdrNames2.Add("S.No");
                                        arrColHdrNames2.Add("Admission No");
                                        arrColHdrNames2.Add("Roll No");
                                        arrColHdrNames2.Add("Register No");
                                        arrColHdrNames2.Add("Student Name");

                                        dscount.Columns.Add("S.No");
                                        dscount.Columns.Add("AdmissionNo");
                                        dscount.Columns.Add("RollNo");
                                        dscount.Columns.Add("RegisterNo");
                                        dscount.Columns.Add("StudentName");
                                        DateTime datfr = new DateTime();
                                        DateTime datto = new DateTime();
                                        datfr = Convert.ToDateTime(datefrom);
                                        datto = Convert.ToDateTime(dateto);
                                        LabelE.Text = string.Empty;
                                        if (optradio.Items[1].Selected == true)
                                        {
                                            while (datfr <= datto)
                                            {

                                                halforfull = string.Empty;
                                                hat.Clear();
                                                hat.Add("date_val", datfr);
                                                hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
                                                hat.Add("sem_val", ddlduration.SelectedValue.ToString());
                                                ds_holi = dacces2.select_method("holiday_sp", hat, "sp");
                                                if (ds_holi.Tables[0].Rows.Count > 0)
                                                {
                                                    if (ds_holi.Tables[0].Rows[0]["halforfull"].ToString() == "False")
                                                    {
                                                        halforfull = "0";
                                                    }
                                                    else
                                                    {
                                                        halforfull = "1";
                                                    }
                                                    if (ds_holi.Tables[0].Rows[0]["morning"].ToString() == "False")
                                                    {
                                                        mng = "0";
                                                    }
                                                    else
                                                    {
                                                        mng = "1";
                                                    }
                                                    if (ds_holi.Tables[0].Rows[0]["evening"].ToString() == "False")
                                                    {
                                                        evng = "0";
                                                    }
                                                    else
                                                    {
                                                        evng = "1";
                                                    }

                                                }
                                                if (halforfull == "1" || string.IsNullOrEmpty(halforfull))
                                                {
                                                    string dtfrom1 = datfr.ToString("dd/MM/yyyy");
                                                    dscount.Columns.Add(dtfrom1.ToString());
                                                    arrColHdrNames2.Add("Absent Hours");
                                                    arrColHdrNames1.Add(dtfrom1.ToString());
                                                }
                                                else
                                                {

                                                    if (cbldisholy.Checked)
                                                    {
                                                        LabelE.Visible = true;
                                                        if (string.IsNullOrEmpty(Convert.ToString(LabelE.Text)))
                                                            LabelE.Text = "Holiday Date - " + datfr.ToString("dd/MM/yyyy") + "";
                                                        else
                                                            LabelE.Text = LabelE.Text + " , " + datfr.ToString("dd/MM/yyyy");
                                                    }
                                                }
                                                


                                              

                                                datfr = datfr.AddDays(1);
                                            }
                                                
                                            
                                        }
                                        if (optradio.Items[1].Selected == true)
                                        {
                                            dscount.Columns.Add("Total_Absent_Hours");
                                            arrColHdrNames1.Add("Total Absent Hours");
                                            arrColHdrNames2.Add("Total Absent Hours");
                                        }
                                        if (optradio.Items[0].Selected == true)
                                        {
                                            dscount.Columns.Add("Total_Absent_Days");
                                            arrColHdrNames2.Add("Total Absent Days");
                                            arrColHdrNames1.Add("Total Absent Days");
                                        }

                                        DataRow drHdr1 = dscount.NewRow();
                                        for (int grCol = 0; grCol < dscount.Columns.Count; grCol++)
                                            drHdr1[grCol] = arrColHdrNames2[grCol];
                                        dscount.Rows.Add(drHdr1);

                                        if (optradio.Items[1].Selected == true)
                                        {
                                            DataRow drHdr2 = dscount.NewRow();
                                            for (int grCol = 0; grCol < dscount.Columns.Count; grCol++)
                                                drHdr2[grCol] = arrColHdrNames1[grCol];
                                            dscount.Rows.Add(drHdr2);
                                        }
                                        int sno = 0;
                                        bool snflag = false;
                                        for (count = 0; count < stud_count; count++)
                                        {
                                            dr = dscount.NewRow();
                                            dayflag = false;
                                            perabsent = 0;

                                            findday();
                                            if (optradio.Items[1].Selected == true)
                                            {
                                                if (perabsenthrs1 != 0)
                                                {
                                                    sno++;
                                                    sflag = true;
                                                    snflag = true;
                                                    dr["S.No"] = sno.ToString();
                                                    dr["AdmissionNo"] = ds.Tables[0].Rows[count]["ROLL ADMIT"].ToString();
                                                    dr["RollNo"] = ds.Tables[0].Rows[count]["ROLL NO"].ToString();
                                                    string rolno = ds.Tables[0].Rows[count]["ROLL NO"].ToString();
                                                    string delflag = ds.Tables[0].Rows[count]["delflag"].ToString();
                                                    if (delflag == "1")
                                                    {
                                                        diccount.Add(sno, rolno);
                                                    }
                                                    dr["RegisterNo"] = ds.Tables[0].Rows[count]["REG NO"].ToString();
                                                    dr["StudentName"] = ds.Tables[0].Rows[count]["STUD NAME"].ToString();
                                                    dr["Total_Absent_Hours"] = perabsenthrs1.ToString();
                                                    // dscount.Rows.Add(dr);
                                                }
                                            }

                                            if (optradio.Items[0].Selected == true)
                                            {
                                                if (perabsent != 0 || splhrabs!=0)
                                                {
                                                    //if (optradio.Items[1].Selected == false)
                                                    //{
                                                    string rolno = ds.Tables[0].Rows[count]["ROLL NO"].ToString();
                                                    string delflag = ds.Tables[0].Rows[count]["delflag"].ToString();
                                                        sflag = true;
                                                        if (snflag == false)
                                                        {
                                                            sno++;
                                                            if (delflag == "1")
                                                            {
                                                                diccount.Add(sno, rolno);
                                                            }
                                                        }
                                                        string rol_no = ds.Tables[0].Rows[count]["ROLL NO"].ToString();
                                                        dr["S.No"] = sno.ToString();
                                                        dr["AdmissionNo"] = ds.Tables[0].Rows[count]["ROLL ADMIT"].ToString();
                                                        dr["RollNo"] = ds.Tables[0].Rows[count]["ROLL NO"].ToString();
                                                       
                                                       
                                                        dr["RegisterNo"] = ds.Tables[0].Rows[count]["REG NO"].ToString();
                                                        dr["StudentName"] = ds.Tables[0].Rows[count]["STUD NAME"].ToString();
                                                   // }

                                                    dr["Total_Absent_Days"] = Convert.ToString(perabsent);
                                                    snflag = false;
                                                    //dscount.Rows.Add(dr);
                                                }
                                                else
                                                {
                                                    if (perabsenthrs1 != 0)
                                                    {
                                                        rcc++;
                                                        dr["Total_Absent_Days"] = Convert.ToString(perabsent);

                                                    }
                                                }
                                              

                                            }
                                            if (perabsent != 0 || perabsenthrs1 != 0)
                                            {
                                                dscount.Rows.Add(dr);
                                            }
                                            i++;

                                        }


                                        divMainContents.Visible = true;
                                        GridView1.DataSource = dscount;
                                        GridView1.DataBind();
                                        lblnote.Visible = true;
                                       
                                        GridView1.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                        GridView1.Rows[0].Font.Bold = true;
                                        GridView1.Rows[0].HorizontalAlign = HorizontalAlign.Center;

                                        if (optradio.Items[1].Selected == true)
                                        {
                                            //Rowspan
                                            GridView1.Rows[1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                            GridView1.Rows[1].Font.Bold = true;
                                            GridView1.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                                            GridViewRow row = GridView1.Rows[0];
                                            GridViewRow previousRow = GridView1.Rows[1];
                                            for (int i = 0; i < row.Cells.Count; i++)
                                            {
                                                if (row.Cells[i].Text == previousRow.Cells[i].Text)
                                                {
                                                    row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                                           previousRow.Cells[i].RowSpan + 1;
                                                    previousRow.Cells[i].Visible = false;
                                                }

                                            }
                                            //Column Span
                                            for (int cell = GridView1.Rows[0].Cells.Count - 1; cell > 0; cell--)
                                            {
                                                TableCell colum = GridView1.Rows[0].Cells[cell];
                                                TableCell previouscol = GridView1.Rows[0].Cells[cell - 1];
                                                if (colum.Text == previouscol.Text)
                                                {
                                                    if (previouscol.ColumnSpan == 0)
                                                    {
                                                        if (colum.ColumnSpan == 0)
                                                        {
                                                            previouscol.ColumnSpan += 2;

                                                        }
                                                        else
                                                        {
                                                            previouscol.ColumnSpan += colum.ColumnSpan + 1;

                                                        }
                                                        colum.Visible = false;

                                                    }
                                                }
                                              //  for(int cl=0;
                                            }
                                            for (int r = 0; r < GridView1.Rows.Count; r++)
                                            {
                                                for (int j1 = 0; j1 < GridView1.HeaderRow.Cells.Count; j1++)
                                                {

                                                    if (r == 0 || r == 1)
                                                    {
                                                        GridView1.Rows[r].Cells[j1].HorizontalAlign = HorizontalAlign.Center;
                                                    }
                                                }
                                            }
                                            if (diccount.Count > 0)
                                            {
                                                foreach (KeyValuePair<int, string> dicval in diccount)
                                                {
                                                    int s_no = dicval.Key;
                                                    string rol_no = dicval.Value;
                                                    GridView1.Rows[s_no+1].BackColor = Color.Red;
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        pageset_pnl.Visible = false;
                                        norecordlbl.Visible = false;
                                        GridView1.Visible = false;
                                        btnxl.Visible = false;
                                        lblrptname.Visible = false;
                                        txtexcelname.Visible = false;
                                        Printcontrol.Visible = false;
                                        btnprintmaster.Visible = false;
                                        norecordlbl.Visible = false;
                                        tofromlbl.Visible = true;
                                        tofromlbl.Text = "No Student(s) Available";
                                        return;
                                    }

                                    //////////////////////
                                }
                                else
                                {
                                    pageset_pnl.Visible = false;
                                    norecordlbl.Visible = false;
                                    GridView1.Visible = false;
                                    btnxl.Visible = false;
                                    lblrptname.Visible = false;
                                    txtexcelname.Visible = false;
                                    Printcontrol.Visible = false;
                                    btnprintmaster.Visible = false;
                                    tofromlbl.Visible = true;
                                    tofromlbl.Text = "From Date Should Be Less Than To Date";
                                    return;
                                }
                            }
                            else
                            {
                                tofromlbl.Visible = true;
                                tofromlbl.Text = "Select Valid to date";
                                pageset_pnl.Visible = false;
                                norecordlbl.Visible = false;
                                btnxl.Visible = false;
                                lblrptname.Visible = false;
                                txtexcelname.Visible = false;
                                Printcontrol.Visible = false;
                                btnprintmaster.Visible = false;
                                GridView1.Visible = false;
                                return;

                            }
                        }
                        else
                        {
                            tofromlbl.Visible = true;
                            tofromlbl.Text = "Select Valid to date";
                            pageset_pnl.Visible = false;
                            norecordlbl.Visible = false;
                            GridView1.Visible = false;
                            btnxl.Visible = false;
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            Printcontrol.Visible = false;
                            btnprintmaster.Visible = false;
                            return;
                        }
                    }
                    else
                    {
                        tofromlbl.Visible = true;
                        tofromlbl.Text = "Select Valid from date";
                        pageset_pnl.Visible = false;
                        norecordlbl.Visible = false;
                        GridView1.Visible = false;
                        btnxl.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        Printcontrol.Visible = false;
                        btnprintmaster.Visible = false;
                        return;
                    }
                }
                else
                {
                    tofromlbl.Visible = true;
                    tofromlbl.Text = "Select Valid from date";
                    pageset_pnl.Visible = false;
                    norecordlbl.Visible = false;
                    GridView1.Visible = false;
                    btnxl.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    Printcontrol.Visible = false;
                    btnprintmaster.Visible = false;
                    return;
                }
            }

            if (dscount.Rows.Count > 0)
            {
                Buttontotal.Visible = true;
                lblrecord.Visible = true;
                DropDownListpage.Visible = true;
                TextBoxother.Visible = false;
                lblpage.Visible = false;
                TextBoxpage.Visible = true;
                GridView1.Visible = true;
                GridView2.Visible = false;
                GridView3.Visible = false;
                btnxl.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;

            }
        }
        catch
        {

        }
    }


    #region "vetri now"

    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        //FpEntry.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
    }


    protected void SelectAll_CheckedChanged(object sender, EventArgs e)
    {
        lblnote.Visible = false;
        if (SelectAll.Checked == true)
        {
            foreach (System.Web.UI.WebControls.ListItem li in ddlreport.Items)
            {
                li.Selected = true;
                TextBox1.Text = "criteria(" + (ddlreport.Items.Count) + ")";

                // FpEntry.Visible = false;
                btnxl.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
            }
        }
        else
        {
            foreach (System.Web.UI.WebControls.ListItem li in ddlreport.Items)
            {
                li.Selected = false;
                TextBox1.Text = "--Select--";
                //  FpEntry.Visible = false;
                btnxl.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
            }
        }

        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        ddlpage.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
    }



    #endregion

    private void present_mark(string Attstr_mark)
    {
        switch (Attstr_mark)
        {
            case "1":
                pp = "0";
                coln = "P";
                break;
            case "2":
                pp = "1";
                coln = "A";
                break;
            case "3":
                pp = "2";
                coln = "OD";
                break;
            case "4":
                pp = "3";
                coln = "ML";
                break;
            case "5":
                pp = "4";
                coln = "SOD";
                break;
            case "6":
                pp = "5";
                coln = "NSS";
                break;
           
            case "7":
                pp = "6";
                coln = "H";
                break;
            case "8":
                pp = "9";
                coln = "NJ";
                break;
            case "9":
                pp = "10";
                coln = "S";
                break;
            case "10":
                pp = "11";
                coln = "L";
                break;
            case "11":
                pp = "12";
                coln = "NCC";
                break;
            case "12":
                pp = "13";
                coln = "HS";
                break;
            case "13":
                pp = "14";
                coln = "PP";
                break;
            case "14":
                pp = "15";
                coln = "SYOD";
                break;
            case "15":
                pp = "16";
                coln = "COD";
                break;
            case "16":
                pp = "17";
                coln = "OOD";
                break;
            case "17":
                pp = "18";
                coln = "LA";
                break;
        }
       
    }

    public void methodgpformat2()
    {
        try
        {
            if (rdiobtndetailornot.Text == "Detail" || rdiobtndetailornot.Text == "Count")
            {
                int selectedcount = 0;
                string dfrom = "", dto = "", strcurday = "", tempfromd = "", temptod = "";
                string strsec = "";
                string sqlpercmd = "";
                int days = 0;
                int cal_from_date;
                int cal_to_date;
                int cal_from_date1;
                DateTime dattimefrom = new DateTime();
                DateTime dattimeto = new DateTime();
                DateTime curday = new DateTime();
                DataSet dsstude = new DataSet();
                bool flagchk = true;
                DataSet dsdetail = new DataSet();
                DataSet dsholyday = new DataSet();//Added By SRinath 13/8/2013



                arrColHdrNames1.Add("S.No");
                arrColHdrNames1.Add("Roll No");
                arrColHdrNames1.Add("Register No");
                arrColHdrNames1.Add("Student Name");


                arrColHdrNames2.Add("S.No");
                arrColHdrNames2.Add("Roll No");
                arrColHdrNames2.Add("Register No");
                arrColHdrNames2.Add("Student Name");

                dtcout.Columns.Add("Sno");
                dtcout.Columns.Add("RollNo");
                dtcout.Columns.Add("RegisterNo");
                dtcout.Columns.Add("StudentName");

                DataTable dtg = new DataTable();

                dfrom = txtFromDate.Text.ToString();
                string[] split = dfrom.Split(new Char[] { '/' });
                tempfromd = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
                cal_from_date = (Convert.ToInt32(split[1].ToString()) + (Convert.ToInt32(split[2].ToString()) * 12));

                dto = txtToDate.Text.ToString();
                string[] splitto = dto.Split(new Char[] { '/' });
                temptod = splitto[1].ToString() + "-" + splitto[0].ToString() + "-" + splitto[2].ToString();
                cal_to_date = (Convert.ToInt32(splitto[1].ToString()) + (Convert.ToInt32(splitto[2].ToString()) * 12));

                dattimefrom = Convert.ToDateTime(tempfromd);
                dattimefrom1 = Convert.ToDateTime(tempfromd);
                dattimeto = Convert.ToDateTime(temptod);

                if (Convert.ToInt16(split[0].ToString()) <= 31 && Convert.ToInt16(split[1].ToString()) <= 12 && Convert.ToInt16(split[0].ToString()) <= Convert.ToInt32(DateTime.Now.Year))
                {
                    if (Convert.ToInt16(splitto[0].ToString()) <= 31 && Convert.ToInt16(splitto[1].ToString()) <= 12 && Convert.ToInt16(splitto[0].ToString()) <= Convert.ToInt32(DateTime.Now.Year))
                    {

                        try
                        {

                            TimeSpan t = dattimeto.Subtract(dattimefrom);
                            days = t.Days;
                            daysofrom = t.Days;
                        }
                        catch
                        {
                            try
                            {
                                dt1 = Convert.ToDateTime(date1);
                                dt2 = Convert.ToDateTime(date2);
                                TimeSpan t = dt2.Subtract(dt1);
                                days = t.Days;
                                daysofrom = t.Days;

                            }
                            catch
                            {

                            }
                        }
                    }
                }

                int ini_column = 0, no_column = 0, chkcount = 0;

                //get student details

                if (ddlsec.Text.ToString() == "All" || ddlsec.Text.ToString() == string.Empty || ddlsec.Text.ToString() == "-1")
                {
                    strsec = "";

                }
                else
                {
                    strsec = " and registration.sections='" + ddlsec.Text.ToString() + "'";

                }
                //added by Srinath
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


                string includediscon = " and delflag=0";
                string getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Discount' and "+ grouporusercode+"");
                if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                {
                    includediscon = string.Empty;
                }
                string includedebar = " and exam_flag <> 'DEBAR'";

                getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Debar' and " + grouporusercode + "");
                if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
                {
                    includedebar = string.Empty;
                }



                sqlpercmd = "select ROW_NUMBER() OVER (" + strorder + ") As SrNo,roll_no,reg_no,registration.stud_name,registration.delflag  from registration inner join applyn on applyn.app_no = registration.app_no where registration.degree_code='" + ddlbranch.SelectedValue + "' and Registration.Current_Semester='" + ddlduration.SelectedItem.Value + "'  " + strsec + "  and  RollNo_Flag<>0 and cc=0 " + includediscon + " " + includedebar + " and registration.batch_year='" + ddlbatch.SelectedValue + "' " + strdayflag + " " + genderflag + " " + regularflag + " " + strorder + "";
                //************End***************************//
                dsstude = d2.select_method(sqlpercmd, hat, "");
                if (dsstude != null && dsstude.Tables[0] != null && dsstude.Tables[0].Rows.Count > 0)
                {

                    //Print Detail 
                    int criteriacout = 0;
                    if (ddlreport.Items.Count > 0)
                    {
                        StringBuilder columnname = new StringBuilder();
                        //AddTableColumn(dtcout,columnname);
                        for (int daycount = 0; daycount <= days; daycount++)
                        {
                            criteriacout = criteriacout + 1;
                            strcurday = Convert.ToString(dattimefrom.AddDays(daycount));

                            curday = dattimefrom.AddDays(daycount);
                            no_column = 0;
                            // ini_column = FpSpread1.Sheets[0].ColumnCount;

                            //Added By Srinath 13/8/2013
                            string holydayquery = "select * from holidaystudents where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + "";
                            dsholyday.Dispose();
                            dsholyday.Reset();
                            dsholyday = d2.select_method(holydayquery, hat, "Text");
                            DataRow drholyday = dsholyday.Tables[0].AsEnumerable().FirstOrDefault(tt => tt.Field<DateTime>("holiday_date") == curday);
                            if (drholyday == null)
                            {
                                for (int i = 0; i < ddlreport.Items.Count; i++)
                                {
                                    if (ddlreport.Items[i].Selected == true)
                                    {
                                       
                                     
                                        string criteria = ddlreport.Items[i].Text.ToString();
                                        if (!string.IsNullOrEmpty(criteria))
                                        {
                                            chkcount++;
                                            critirianame = new System.Text.StringBuilder(criteria);
                                            AddTableColumn(dtcout, critirianame);
                                            arrColHdrNames1.Add(curday.ToString("d-MM-yyyy"));
                                            arrColHdrNames2.Add(critirianame);

                                            no_column = no_column + 1;
                                            if (flagchk == true)
                                            {
                                                selectedcount++;
                                            }
                                        }

                                    }
                                }

                            }
                        }

                        int getnoofcolumn = no_column;
                        int day = 0;
                        int month = 0, year = 0;
                        Hashtable htattncount = new Hashtable();

                        if (selectedcount == 0)
                        {
                            LabelE.Visible = true;
                            LabelE.Text = "Please Choose Any Criteria";
                            btnxl.Visible = false;
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            Printcontrol.Visible = false;
                            btnprintmaster.Visible = false;
                        }

                        int colct = 0;
                        DataRow drHdr1 = dtcout.NewRow();
                        DataRow drHdr2 = dtcout.NewRow();
                        for (int grCol = 0; grCol < dtcout.Columns.Count; grCol++)
                        {
                            drHdr1[grCol] = arrColHdrNames1[grCol];
                            drHdr2[grCol] = arrColHdrNames2[grCol];
                        }
                        dtcout.Rows.Add(drHdr1);
                        dtcout.Rows.Add(drHdr2);
                        int studsno = 0;
                        string sqlsquery = d2.GetFunction("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code='" + ddlbranch.SelectedValue + "' and semester='" + ddlduration.SelectedValue + "'");
                        //DataSet df = new DataSet();
                        //df = dacces2.select_method_wo_parameter(sqlsquery, "text");
                        //monthyr = Convert.ToInt32(df.Tables[0].Rows[0]["no_of_hrs_I_half_day"]);
                        //no_of_hrs = Convert.ToInt32(df.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                        //evng_hrs = Convert.ToInt32(df.Tables[0].Rows[0]["no_of_hrs_II_half_day"]);
                        //order = df.Tables[0].Rows[0]["schorder"].ToString();
                        for (int stude1 = 0; stude1 < dsstude.Tables[0].Rows.Count; stude1++)
                        {

                            htattncount.Clear();
                            int printvaluelocation = 4;
                            studsno++;
                            dr1 = dtcout.NewRow();
                            dr1["Sno"] = studsno.ToString();
                            string rollno = Convert.ToString(dsstude.Tables[0].Rows[stude1]["roll_no"]);
                            dr1["RollNo"] = rollno;
                            dr1["RegisterNo"] = Convert.ToString(dsstude.Tables[0].Rows[stude1]["reg_no"]);
                            dr1["StudentName"] = Convert.ToString(dsstude.Tables[0].Rows[stude1]["stud_name"]);

                            int reportselcount = 0;
                            for (int rtct = 0; rtct < ddlreport.Items.Count; rtct++)
                            {
                                if (ddlreport.Items[rtct].Selected == true)
                                {
                                    string criteria1 = ddlreport.Items[rtct].Text.ToString();
                                    if (!string.IsNullOrEmpty(criteria1))
                                    {
                                        reportselcount++;
                                    }
                                }
                            }
                            int ctrept = reportselcount;

                            hat.Clear();
                            hat.Add("std_rollno", rollno);
                            hat.Add("from_month", cal_from_date);
                            hat.Add("to_month", cal_to_date);
                            dsdetail.Dispose();
                            dsdetail.Reset();
                            dsdetail = d2.select_method("STUD_ATTENDANCE", hat, "sp");
                            int crticout = 0;
                            int day3 = 0;
                            int m = 0;
                            if (dsdetail != null && dsdetail.Tables[0] != null && dsdetail.Tables[0].Rows.Count > 0)
                            {

                                for (int dayct = 0; dayct <= days; dayct++)
                                {
                                    htattncount.Clear();
                                    int count = 0;
                                    m++;
                                    crticout = crticout + 1;
                                    string stcountr = "";
                                    curday = dattimefrom.AddDays(dayct);
                                    string hourtemp = "", detailhourprint = "", detailhrsprint = "";
                                    string detval = string.Empty;
                                    string strdayhour = "";
                                    day = curday.Day;
                                    // days3 = day;
                                    month = curday.Month;
                                    year = curday.Year;
                                    cal_from_date1 = (Convert.ToInt32(month) + Convert.ToInt32(year) * 12);
                                    bool flagc = true;
                                    DataRow drholyday = dsholyday.Tables[0].AsEnumerable().FirstOrDefault(tt => tt.Field<DateTime>("holiday_date") == curday);
                                    // int day3 = 0;

                                    if (drholyday == null)
                                    {
                                        foreach (DataRow drr in dsdetail.Tables[0].Rows)
                                        {
                                            if (drr["roll_no"].ToString() != "" && drr["month_year"].ToString() != "")
                                            {
                                                if (drr["roll_no"].ToString().Trim().ToLower() == rollno.Trim().ToLower() && drr["month_year"].ToString() == cal_from_date1.ToString())
                                                {
                                                    for (int y = 1; y <= Convert.ToInt32(sqlsquery); y++)
                                                    {
                                                        hourtemp = "d" + day + "d" + y;
                                                        strdayhour = drr[hourtemp].ToString();
                                                        present_mark(strdayhour);
                                                        if (strdayhour != "")
                                                        {
                                                            if (rdiobtndetailornot.Text == "Count")
                                                            {
                                                                if (htattncount.Contains(Convert.ToString(strdayhour)))
                                                                {
                                                                    count = Convert.ToInt32(GetCorrespondingKey(Convert.ToString(strdayhour), htattncount));
                                                                    count = count + 1;
                                                                    htattncount[Convert.ToString(strdayhour)] = count;
                                                                }
                                                                else
                                                                {
                                                                    htattncount.Add(Convert.ToString(strdayhour), 1);
                                                                    count = 1;
                                                                }
                                                            }


                                                            else if (rdiobtndetailornot.Text == "Detail")
                                                            {

                                                                if (htattncount.Contains(Convert.ToString(strdayhour)))
                                                                {
                                                                    stcountr = Convert.ToString(GetCorrespondingKey(Convert.ToString(strdayhour), htattncount));
                                                                    if (stcountr != "")
                                                                    {
                                                                        if (detailhourprint == "")
                                                                        {
                                                                            detailhourprint = stcountr + "," + y;
                                                                        }
                                                                        else
                                                                        {
                                                                            detailhourprint = detailhourprint + "," + y;
                                                                        }
                                                                        htattncount[Convert.ToString(strdayhour)] = detailhourprint;
                                                                        detailhrsprint = detailhourprint;
                                                                        detailhourprint = "";
                                                                        detval = string.Empty;
                                                                    }


                                                                }
                                                                else
                                                                {
                                                                    htattncount.Add(Convert.ToString(strdayhour), y);

                                                                    detval = Convert.ToString(y);
                                                                   

                                                                }

                                                            }
                                                        }
                                                        else
                                                        {
                                                        }
                                                        if (rdiobtndetailornot.Text == "Count")
                                                        {
                                                            string datrow = string.Empty;
                                                            if (strdayhour != "" && strdayhour != "0")
                                                            {
                                                                int counti = Convert.ToInt32(strdayhour);

                                                                if (day3 > 0)
                                                                {
                                                                    if (m != 0)
                                                                    {
                                                                        if (day3 > 1)
                                                                        {
                                                                            reportselcount = ctrept + reportselcount;
                                                                            colct = Convert.ToInt32(strdayhour) + 3 + reportselcount;

                                                                        }
                                                                        else
                                                                        {
                                                                            colct = Convert.ToInt32(strdayhour) + 3 + reportselcount;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        colct = Convert.ToInt32(strdayhour) + 3 + reportselcount;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    colct = Convert.ToInt32(strdayhour) + 3;
                                                                }
                                                                // if(ddlreport.SelectedItem.Text
                                                                if (ddlreport.Items[Convert.ToInt32(pp)].Selected == true)
                                                                {
                                                                   // datrow = dtcout.Columns[Convert.ToInt32(colct)].ColumnName;
                                                                    dr1[coln] = count;
                                                                }

                                                            }
                                                        }
                                                        else if (rdiobtndetailornot.Text == "Detail")
                                                        {
                                                            string datrow = string.Empty;
                                                            if (strdayhour != "" && strdayhour != "0")
                                                            {
                                                                //int colct = 0;
                                                                int counti = Convert.ToInt32(strdayhour);
                                                                if (day3 > 0)
                                                                {
                                                                    if (m != 0)
                                                                    {
                                                                        if (day3 > 1)
                                                                        {
                                                                            reportselcount = ctrept + reportselcount;
                                                                            colct = Convert.ToInt32(strdayhour) + 3 + reportselcount;

                                                                        }
                                                                        else
                                                                        {
                                                                            colct = Convert.ToInt32(strdayhour) + 3 + reportselcount;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        colct = Convert.ToInt32(strdayhour) + 3 + reportselcount;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    colct = Convert.ToInt32(strdayhour) + 3;
                                                                }

                                                                if (ddlreport.Items[Convert.ToInt32(pp)].Selected == true)
                                                                {
                                                                   // datrow = dtcout.Columns[Convert.ToInt32(colct)].ColumnName;
                                                                    if(string.IsNullOrEmpty(detval))
                                                                        dr1[coln] = detailhrsprint;
                                                                    else
                                                                        dr1[coln] = detval;
                                                                }

                                                            }
                                                        }
                                                        m = 0;

                                                    }
                                                  //  diccount.Add(

                                                    // }

                                                }
                                            }
                                            else
                                            {
                                                //Do nothing
                                            }
                                        }
                                    }
                                    day3++;

                                }
                            }

                            dtcout.Rows.Add(dr1);
                        }
                        GridView2.DataSource = dtcout;
                        GridView2.DataBind();

                        for (int r = 0; r < GridView2.Rows.Count; r++)
                        {
                            for (int j1 = 0; j1 < GridView2.HeaderRow.Cells.Count; j1++)
                            {

                                if (r == 0 || r == 1)
                                {
                                    GridView2.Rows[r].Cells[j1].HorizontalAlign = HorizontalAlign.Center;
                                }
                                if (j1 > 4)
                                {
                                    GridView2.Rows[r].Cells[j1].Width = 50;
                                }
                            }
                        }

                        GridView2.Visible = true;
                        GridView1.Visible = false;
                        divMainContents.Visible = true;

                        GridView2.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        GridView2.Rows[0].Font.Bold = true;
                        GridView2.Rows[0].HorizontalAlign = HorizontalAlign.Center;


                        //Rowspan
                        GridView2.Rows[1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        GridView2.Rows[1].Font.Bold = true;
                        GridView2.Rows[1].HorizontalAlign = HorizontalAlign.Center;
                        GridViewRow row = GridView2.Rows[0];
                        GridViewRow previousRow = GridView2.Rows[1];
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            if (row.Cells[i].Text == previousRow.Cells[i].Text)
                            {
                                row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                       previousRow.Cells[i].RowSpan + 1;
                                previousRow.Cells[i].Visible = false;
                            }

                        }
                        //Column Span
                        for (int cell = GridView2.Rows[0].Cells.Count - 1; cell > 0; cell--)
                        {
                            TableCell colum = GridView2.Rows[0].Cells[cell];
                            TableCell previouscol = GridView2.Rows[0].Cells[cell - 1];
                            if (colum.Text == previouscol.Text)
                            {
                                if (previouscol.ColumnSpan == 0)
                                {
                                    if (colum.ColumnSpan == 0)
                                    {
                                        previouscol.ColumnSpan += 2;

                                    }
                                    else
                                    {
                                        previouscol.ColumnSpan += colum.ColumnSpan + 1;

                                    }
                                    colum.Visible = false;

                                }
                            }
                        }

                        GridView2.Visible = true;

                        if (dtcout.Rows.Count < 0)
                        {
                            LabelE.Visible = true;
                            LabelE.Text = "No Records Found";
                            GridView2.Visible = false;
                            GridView1.Visible = false;
                            GridView3.Visible = false;
                            btnxl.Visible = false;
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            Printcontrol.Visible = false;
                            btnprintmaster.Visible = false;
                        }
                    }
                    else
                    {
                        LabelE.Visible = true;
                        LabelE.Text = "Please Choose Atleast One Criteria";
                        btnxl.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        Printcontrol.Visible = false;
                        btnprintmaster.Visible = false;
                        GridView2.Visible = false;
                        GridView1.Visible = false;
                        GridView3.Visible = false;
                    }

                    if (chkcount == 0)
                    {
                        LabelE.Visible = true;
                        LabelE.Text = "Please Choose Atleast One Criteria";
                        btnxl.Visible = false;
                        GridView1.Visible = false;
                        GridView2.Visible = false;
                        GridView3.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        Printcontrol.Visible = false;
                        btnprintmaster.Visible = false;
                    }

                }
                else
                {
                    LabelE.Visible = true;
                    LabelE.Text = "No Student(s) Available";
                    GridView2.Visible = false;
                    GridView1.Visible = false;
                    GridView3.Visible = false;
                }
            }
            else
            {
                LabelE.Visible = true;
                LabelE.Text = "Please Choose Count/ Detail";
                //*****anyutha 3nd sep 14*****//
                GridView2.Visible = false;
                GridView1.Visible = false;
                GridView3.Visible = false;
                ////*end*//

            }
            btnxl.Visible = true;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnprintmaster.Visible = true;

        }
        catch (Exception exce)
        {
            string exc = exce.ToString();
        }

    }

    protected void gridview2_DataBound(object sender, GridViewRowEventArgs e)
    {

        try
        {



            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int grCol = 0; grCol < dtcout.Columns.Count; grCol++)
                {
                    e.Row.Cells[grCol].Visible = false;
                    
                }

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;

                for (int r = 4; r < dtcout.Columns.Count; r++)
                {
                    e.Row.Cells[r].HorizontalAlign = HorizontalAlign.Center;
                   
                }

            }

           

        }
        catch
        {
        }
    }
    protected void gridview3_DataBound(object sender, EventArgs e)
    {

        try
        {
            for (int i = GridView3.Rows.Count - 1; i > 0; i--)
            {

                GridViewRow row = GridView3.Rows[i];
                GridViewRow previousRow = GridView3.Rows[i - 1];
                for (int j = 1; j < row.Cells.Count; j++)
                {
                    if (j < 4)
                    {
                        if (row.Cells[j].Text == previousRow.Cells[j].Text)
                        {
                            if (previousRow.Cells[j].RowSpan == 0)
                            {
                                if (row.Cells[j].RowSpan == 0)
                                {
                                    previousRow.Cells[j].RowSpan += 2;
                                }
                                else
                                {
                                    previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                }
                                row.Cells[j].Visible = false;
                            }
                        }
                    }
                }
            }


        }
        catch
        {
        }
    }
    protected void gridview3_DataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            for (int grCol = 0; grCol < dtcont.Columns.Count; grCol++)
                e.Row.Cells[grCol].Visible = false;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Left;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Left;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[13].HorizontalAlign = HorizontalAlign.Center;
        }
    }


    protected void onselected_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblnote.Visible = false;
        tofromlbl.Visible = false;//Added by srinath 21/8/2013
        norecordlbl.Visible = false;
        if (ddlformat.SelectedItem.Text == "Absentees")
        {
            LabelE.Visible = false;
            LabelE.Text = string.Empty;
            rdiobtndetailornot.Visible = false;
            optradio.Visible = true;
            pnlCustomers.Visible = false;
            Label2.Visible = false;
            TextBox1.Visible = false;
            pnlCustomers.Visible = false;
            lblsubject.Visible = false;
            txt_subject.Visible = false;
            panel_Department.Visible = false;
            btnxl.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            cbldisholy.Visible = true;
            cbdispne.Visible = true;
            GridView1.Visible = false;
            GridView2.Visible = false;
            GridView3.Visible = false;
        }
        else if (ddlformat.SelectedItem.Text == "General")
        {
            LabelE.Visible = false;
            LabelE.Text = string.Empty;
            rdiobtndetailornot.Visible = true;
            optradio.Visible = false;
            Label2.Visible = true;
            TextBox1.Visible = true;
            pnlCustomers.Visible = true;
            lblsubject.Visible = false;
            txt_subject.Visible = false;
            panel_Department.Visible = false;
            btnxl.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            cbldisholy.Visible = false;
            cbdispne.Visible = false;
            GridView1.Visible = false;
            GridView2.Visible = false;
            GridView3.Visible = false;
        }
        else if (ddlformat.SelectedItem.Text == "Hourwise Absentees")
        {
            LabelE.Visible = false;
            LabelE.Text = string.Empty;
            rdiobtndetailornot.Visible = false;
            optradio.Visible = false;
            pnlCustomers.Visible = false;
            Label2.Visible = false;
            TextBox1.Visible = false;
            pnlCustomers.Visible = false;
            lblsubject.Visible = true;
            txt_subject.Visible = true;
            panel_Department.Visible = true;
            btnxl.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            subject();
            cbldisholy.Visible = false;
            cbdispne.Visible = false;
            GridView1.Visible = false;
            GridView2.Visible = false;
            GridView3.Visible = false;
        }
    }

    public int getattendance(string Att_str1)
    {
        int Attvalue;

        Attvalue = 0;
        if (Att_str1 == "P")
        {
            Attvalue = 1;

        }
        else if (Att_str1 == "A")
        {
            Attvalue = 2;

        }
        else if (Att_str1 == "OD")
        {
            Attvalue = 3;

        }
        else if (Att_str1 == "ML")
        {
            Attvalue = 4;

        }
        else if (Att_str1 == "SOD")
        {
            Attvalue = 5;
        }

        else if (Att_str1 == "NSS")
        {
            Attvalue = 6;

        }
        else if (Att_str1 == "H")
        {
            Attvalue = 7;

        }

        else if (Att_str1 == "NJ")
        {
            Attvalue = 8;

        }
        else if (Att_str1 == "S")
        {
            Attvalue = 9;

        }
        else if (Att_str1 == "L")
        {
            Attvalue = 10;

        }
        else if (Att_str1 == "NCC")
        {
            Attvalue = 11;

        }
        else if (Att_str1 == "HS")
        {
            Attvalue = 12;
        }

        else if (Att_str1 == "PP")
        {
            Attvalue = 13;
        }
        else if (Att_str1 == "SYOD")
        {
            Attvalue = 14;
        }
        else if (Att_str1 == "COD")
        {
            Attvalue = 15;
        }
        else if (Att_str1 == "OOD")
        {
            Attvalue = 16;
        }
        else if (Att_str1 == "LA")
        {
            Attvalue = 17;
        }
        //*******added by subburaj*****//
        else if (Att_str1 == "RAA")
        {
            Attvalue = 18;
        }
        //*****end****// 
        else
        {
            Attvalue = 0;
        }
        return Attvalue;

    }
    protected void Button1_Click(object sender, EventArgs e)
    {




        // FarPoint.Web.Spread.PrintInfo pi = new FarPoint.Web.Spread.PrintInfo();
        // pi.Orientation = FarPoint.Web.Spread.PrintOrientation.Portrait;
        // //pi.SmartPrintPagesTall = 1;
        // // pi.ZoomFactor = 2;
        // //pi.Footer = "This is Page /p/nof /pc Pages";
        // //pi.Header = "Print Job For /nFPT Inc.";
        // // pi.Colors = new System.Drawing.Color[] { System.Drawing.Color.Red, System.Drawing.Color.Blue };
        // //pi.RepeatColEnd = 25;
        // //pi.RepeatColStart = 1;
        // //pi.RepeatRowEnd = 25;
        // //pi.RepeatRowStart = 1;
        // this.FpSpread1.Sheets[0].PrintInfo = pi;
        //// //this.FpSpread1.SavePdf("c:\\test.pdf");
        //// this.FpSpread1.SavePdfToResponse();

        //FarPoint.Web.Spread.SmartPrintRulesCollection printrules = new FarPoint.Web.Spread.SmartPrintRulesCollection();
        //printrules.Add(new FarPoint.Web.Spread.BestFitColumnRule(FarPoint.Web.Spread.ResetOption.None));
        //printrules.Add(new FarPoint.Web.Spread.LandscapeRule(FarPoint.Web.Spread.ResetOption.None));
        //printrules.Add(new FarPoint.Web.Spread.ScaleRule(FarPoint.Web.Spread.ResetOption.All, 1.0f, .4f, .2f));
        //// Create a PrintInfo object and set the properties.
        //FarPoint.Web.Spread.PrintInfo printset = new FarPoint.Web.Spread.PrintInfo();
        //printset.SmartPrintRules = printrules;
        //printset.UseSmartPrint = true;
        //FpSpread1.Sheets[0].PrintInfo = printset;
        //FpSpread1.SavePdf("c:\\test.pdf"); //Print the sheet
        //FpSpread1.Save("jjj", true);
        //FpSpread1.SavePdfToResponse();

        //FarPoint.Web.Spread.SmartPrintRulesCollection rules = new FarPoint.Web.Spread.SmartPrintRulesCollection();
        //FarPoint.Web.Spread.PrintInfo pi = new FarPoint.Web.Spread.PrintInfo();
        //FarPoint.Web.Spread.LandscapeRule lr = new FarPoint.Web.Spread.LandscapeRule();
        //FarPoint.Web.Spread.ScaleRule sr = new FarPoint.Web.Spread.ScaleRule();
        //FarPoint.Web.Spread.BestFitColumnRule bfcr = new FarPoint.Web.Spread.BestFitColumnRule();
        //lr.ResetOption = FarPoint.Web.Spread.ResetOption.None;
        //sr.ResetOption = FarPoint.Web.Spread.ResetOption.None;
        //sr.StartFactor = 1;
        //sr.EndFactor = 2;
        //sr.Interval = 0.5f;
        //bfcr.ResetOption = FarPoint.Web.Spread.ResetOption.None;
        //rules.Add(lr);
        //rules.Add(sr);
        //rules.Add(bfcr);
        //pi.SmartPrintRules = rules;
        //FpSpread1.ActiveSheetView.PrintInfo = pi;

    }


    protected void ddlreport_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
             
        {
            lblnote.Visible = false;
            int commcount = 0;
            for (int i = 0; i < ddlreport.Items.Count; i++)
            {
                if (ddlreport.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    TextBox1.Text = "Criteria(" + commcount.ToString() + ")";

                }
            }
            if (commcount == 0)
            {
                TextBox1.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {
            //  errmsg.Text = ex.ToString();
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        //Session["column_header_row_count"] = FpSpread1.Sheets[0].ColumnHeader.RowCount;



        string sections = ddlsec.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
        {
            sections = "";
        }
        else
        {
            sections = "- Sec-" + sections;
        }
        string degreedetails = "Hourwise/Daywise Absentees Report" + '@' + "Degree: " + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '-' + ddlbranch.SelectedItem.ToString() + '-' + "Sem-" + ddlduration.SelectedItem.ToString() + sections + '@' + "Date :" + txtFromDate.Text.ToString() + " To " + txtToDate.Text.ToString();
        string pagename = "AbsenteeRt.aspx";


        string ss = null;



        if (ddlformat.SelectedIndex == 0)
        {
            GridView1.Visible = true;
            Printcontrol.loadspreaddetails(GridView1, pagename, degreedetails, 0, ss);
            Printcontrol.Visible = true;
           // GridView1.Visible = true;
        }
        else if (ddlformat.SelectedIndex == 1)
        {
            GridView2.Visible = true;
            Printcontrol.loadspreaddetails(GridView2, pagename, degreedetails, 0, ss);
            Printcontrol.Visible = true;
          //  GridView2.Visible = true;
        }
        else
        {
            GridView3.Visible = true;
            Printcontrol.loadspreaddetails(GridView3, pagename, degreedetails, 0, ss);
            Printcontrol.Visible = true;
           // GridView3.Visible = true;

        }
    }


    protected void subject()
    {
        txt_subject.Text = "--Select--";
        ddlsubject.Items.Clear();
        if (ddlduration.Text != "")
        {
            string sql = "select distinct sem.subject_type,s.subject_code,S.subject_no,subject_name,s.acronym from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code  and SM.degree_code='" + ddlbranch.SelectedItem.Value + "' and SM.semester='" + ddlduration.SelectedItem.Value + "' and  S.subtype_no = Sem.subtype_no and SM.batch_year='" + ddlbatch.SelectedItem.Value + "' order by S.subject_no";
            ddlsubject.Items.Clear();
            ds1 = d2.select_method_wo_parameter(sql, "text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddlsubject.DataSource = ds1;
                ddlsubject.DataTextField = "subject_name";
                ddlsubject.DataValueField = "subject_no";
                ddlsubject.DataBind();
            }
        }
        else
        {

        }
    }
    protected void chksubject_checkedchanged(object sender, EventArgs e)
    {
        txt_subject.Text = "--Select--";
        if (chktesr.Checked == true)
        {
            for (int i = 0; i < ddlsubject.Items.Count; i++)
            {
                ddlsubject.Items[i].Selected = true;
                txt_subject.Text = "Subject(" + (ddlsubject.Items.Count) + ")";
            }

        }
        else
        {
            for (int i = 0; i < ddlsubject.Items.Count; i++)
            {
                ddlsubject.Items[i].Selected = false;
                txt_subject.Text = "--Select--";
            }
        }

    }

    protected void ddlsubject_selectedchanged(object sender, EventArgs e)
    {
        int ddlcount = 0;
        try
        {
            txt_subject.Text = "--Select--";
            string value = "";
            string code = "";
            for (int i = 0; i < ddlsubject.Items.Count; i++)
            {

                if (ddlsubject.Items[i].Selected == true)
                {
                    value = ddlsubject.Items[i].Text;
                    code = ddlsubject.Items[i].Value.ToString();
                    ddlcount = ddlcount + 1;
                    txt_subject.Text = "Subject(" + ddlcount.ToString() + ")";
                }
            }

            if (ddlcount == 0)
                txt_subject.Text = "---Select---";
        }
        catch
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
}
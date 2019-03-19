using System;
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
using InsproDataAccess;
using System.Text;


public partial class cumreport : System.Web.UI.Page
{
    double dup_conptnval1 = 0;
    string sem_start = string.Empty;
    string sem_end = string.Empty;

    //static conducted
    [Serializable()]
    public class MyImg : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl;

            img.Width = Unit.Percentage(90);
            return img;
        }
    }
    #region Variable Decalaration

    SqlConnection cona = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection dar_con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection bind_con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_splhr_query_master = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con4a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getcona = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection tempcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con4 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection cbdaycon = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection con5 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection getdaycon = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection getdaycon1 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection getdaycon2 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection getdaycon3 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection loadcon = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection getloadcon = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection leavecon = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection leavecon1 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection gradecon = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection gradecon1 = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection readcon = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());

    SqlCommand cmd8 = new SqlCommand();
    SqlCommand cmda;
    SqlCommand cmd1a;
    SqlCommand cmd3a;
    SqlCommand cmd4a;
    SqlCommand cmd6a;
    SqlDataReader ratt;
    SqlCommand cmd = new SqlCommand();

    static Hashtable hasdaywise = new Hashtable();
    static Hashtable hashrwise = new Hashtable();
    static Boolean forschoolsetting = false;
    InsproDirectAccess dirAcc = new InsproDirectAccess();

    string regularflag = "", new_header_string = "", new_header_string_index = string.Empty;
    string genderflag = string.Empty;
    //saravana strat 
    int mmyycount;
    string dd = string.Empty;
    Hashtable hat = new Hashtable();
    static Boolean splhr_flag = false;
    Hashtable hatdicOD = new Hashtable();
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    int days1 = 0;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet dsprint = new DataSet();
    DataSet dsalterattndschd = new DataSet();
    string UnmarkHours = string.Empty;
    string CurrentDate = string.Empty;
    int commcount = 0;

    Boolean yesflag = false;
    string leftlogo = "", rightlogo = "", leftlength = "", rightlength = "", multi_iso = string.Empty;
    //===================12/6/12 PRABHA
    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0, tot_ml_spl_fals = 0;
    double per_leave_fals = 0;
    int per_abshrs_spl_true = 0, tot_per_hrs_spl_true = 0, tot_conduct_hr_spl_true = 0, tot_ondu_spl_true = 0, tot_ml_spl_true = 0;
    double per_leave_true = 0;
    //============================
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0, tot_ml_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0, cum_per_workingdays1 = 0;
    int notconsider_value = 0;
    double conduct_hour_new = 0;
    static string view_footer = "", view_header = "", view_footer_text = string.Empty;
    int moncount;
    double dif_date = 0;
    double dif_date1 = 0;
    int abs = 0, att = 0;
    int dum_diff_date, unmark;
    string roll_no, reg_no, roll_ad, studname;
    int check;
    string temp_reg_no = string.Empty;

    DateTime per_from_date;
    DateTime per_to_date;
    DateTime per_from_gendate;
    DateTime per_to_gendate;
    DateTime per_from_cumdate;
    DateTime per_to_cumdate;
    DateTime dumm_from_date;
    DateTime Admission_date;

    string frdate, todate, new_header_name = string.Empty;
    string halforfull = "", mng = "", evng = "", holiday_sched_details = string.Empty;
    int final_print_col_cnt = 0;

    Boolean check_col_count_flag = false;
    string column_field = "", printvar = string.Empty;
    int col_count_all = 0, span_cnt = 0, child_span_count = 0;
    int footer_count = 0, temp_count = 0, split_col_for_footer = 0, footer_balanc_col = 0;
    string footer_text = string.Empty;
    TimeSpan ts;
    string coll_name = "", address1 = "", degree_deatil = "", header_alignment = "", address2 = "", phoneno = "", faxno = "", email = "", address3 = "", website = "", form_name = "", pincode = string.Empty;
    string[] new_header_string_split;
    int end_column = 0;
    int temp_count_temp = 0;
    string phone = "", fax = "", email_id = "", web_add = string.Empty;
    string diff_date;

    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;
    int col_count = 0;
    int count;
    int next = 0;
    int minpresII = 0;

    string value, date;
    int i, rows_count;
    string tempvalue = "-1";
    int ObtValue = -1;
    Boolean colon = false;

    //Opt------------
    int cal_from_date, cal_from_date_tmp, cal_from_cumdate_tmp;
    int cal_to_date, start_column = 0, cal_to_date_tmp, cal_to_cumdate_tmp;
    //---------------

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
    int tot_per_hrs, per_tot_per_hrs, cum_per_tot_per, tot_wok_hrs;
    double per_con_hrs, cum_con_hrs;
    double njhr, njdate, per_njhr, per_njdate, cum_njhr, cum_njdate;
    double per_per_hrs, cum_per_perhrs;
    double tot_ondu, per_tot_ondu, cum_tot_ondu, cum_tot_ml, tot_ml, per_tot_ml;
    string usercode = "", collegecode = "", singleuser = "", group_user = string.Empty;
    string[] string_session_values;
    DataSet ds_attnd_pts = new DataSet();
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    string value_holi_status = string.Empty;
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = "", split_holiday_status_2 = string.Empty;
    string isonumber = string.Empty;
    int inirow_count = 0;
    static string grouporusercode = string.Empty;
    //opt----
    int demfcal, demtcal;
    string monthcal;
    DataSet ds_sphr = new DataSet();
    static Hashtable ht_sphr = new Hashtable();

    //-------
    string strregorder = string.Empty;//added by gowtham 27/8/2013

    string strorder = string.Empty;//added by gowtham 27/8/2013
    Hashtable hatonduty = new Hashtable();
    Hashtable hatcumonduty = new Hashtable();
    string strondutyvalue = string.Empty;
    int ondutycount = 0;
    string stronduquery = string.Empty;
    Boolean cumlaflag = false;
    Double minpresday = 0, nohrsprsentperday = 0, noofdaypresen = 0;
    DataTable data = new DataTable();
    DataRow drow;
    int colstartcnt = 0;
    static Dictionary<int, string> dicletterreport = new Dictionary<int, string>();
    int colscnt = 0;
    int colspancnt = 0;
    int cumcolscnt = 0;
    int cumcolspancnt = 0;
    int cumcolspancnt1 = 0;
    int colcount2 = 0;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        lblnorec.Visible = false;
        string college = string.Empty;
        college = Session["collegecode"].ToString();
        lblnorec.Visible = false;
        if (!IsPostBack)
        {
            txtfromdate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            tbfmcumdate.Attributes.Add("readonly", "readonly");
            tbtocumdate.Attributes.Add("readonly", "readonly");

            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            Session["checkflag"] = "0";
            //if (Convert.ToString(Session["value"]) == "1")//==========back button visible
            //{
            //    LinkButton3.Visible = false;
            //    LinkButton2.Visible = true;
            //}
            //else
            //{
            //    LinkButton3.Visible = true;
            //    LinkButton2.Visible = false;
            //}

            //pagesetpanel.Visible = false;

            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            Printcontrol.Visible = false;
            //lblpages.Visible = false;
            //ddlpage.Visible = false;
            Showgrid.Visible = false;
            btnletter.Visible = false;
            btnxl.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;

            lblnorec.Visible = false;
            cbpoints.Visible = false;
            lblcumfrm.Visible = false;
            tbfmcumdate.Visible = false;
            lblcumto.Visible = false;
            tbtocumdate.Visible = false;
            dateerr.Visible = false;

            txtfromdate.Text = DateTime.Today.ToString("d/MM/yyyy");
            txttodate.Text = DateTime.Today.ToString("d/MM/yyyy");
            tbfmcumdate.Text = DateTime.Today.ToString("d/MM/yyyy");
            tbtocumdate.Text = DateTime.Today.ToString("d/MM/yyyy");

            Session["daywise"] = "0";
            Session["hourwise"] = "0";
            string strdayflag;
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            Session["attdaywisecla"] = "0";

            Session["Gridcellrow"] = "0";
            string daywisecal = d2.GetFunction("select value from Master_Settings where settings='Attendance Day Wise Calculation'");
            if (daywisecal.Trim() == "1")
            {
                Session["attdaywisecla"] = "1";
            }


            if (Session["usercode"] != "")
            {
                string Master1 = string.Empty;
                Master1 = "select * from Master_Settings where " + grouporusercode + "";
                readcon.Close();
                readcon.Open();
                SqlDataReader mtrdr;

                SqlCommand mtcmd = new SqlCommand(Master1, readcon);
                mtrdr = mtcmd.ExecuteReader();
                strdayflag = string.Empty;
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
                            strdayflag = " and (a.Stud_Type='Day Scholar'";
                        }
                        if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                        {
                            if (strdayflag != "" && strdayflag != "\0")
                            {
                                strdayflag = strdayflag + " or a.Stud_Type='Hostler'";
                            }
                            else
                            {
                                strdayflag = " and (a.Stud_Type='Hostler'";
                            }
                        }
                        if (mtrdr["settings"].ToString() == "Regular" && mtrdr["value"].ToString() == "1")
                        {
                            regularflag = "and ((registration.mode=1)";

                            // Session["strvar"] = Session["strvar"] + " and (mode=1)";
                        }
                        if (mtrdr["settings"].ToString() == "Lateral" && mtrdr["value"].ToString() == "1")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (registration.mode=3)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((registration.mode=3)";
                            }
                            //Session["strvar"] = Session["strvar"] + " and (mode=3)";
                        }
                        if (mtrdr["settings"].ToString() == "Transfer" && mtrdr["value"].ToString() == "1")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (registration.mode=2)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((registration.mode=2)";
                            }
                            //Session["strvar"] = Session["strvar"] + " and (mode=2)";
                        }

                        if (mtrdr["settings"].ToString() == "Male" && mtrdr["value"].ToString() == "1")
                        {
                            genderflag = " and (sex='0'";
                        }
                        if (mtrdr["settings"].ToString() == "Female" && mtrdr["value"].ToString() == "1")
                        {
                            if (genderflag != "" && genderflag != "\0")
                            {
                                genderflag = genderflag + " or sex='1'";
                            }
                            else
                            {
                                genderflag = " and (sex='1'";
                            }

                        }
                        if (mtrdr["settings"].ToString() == "Day Wise" && mtrdr["value"].ToString() == "1")
                        {
                            Session["Daywise"] = "1";
                        }
                        if (mtrdr["settings"].ToString() == "Hour Wise" && mtrdr["value"].ToString() == "1")
                        {
                            Session["Hourwise"] = "1";
                        }



                    }
                }
                if (strdayflag != "")
                {
                    strdayflag = strdayflag + ")";
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


                Session["strvar"] = Session["strvar"] + regularflag + genderflag;
            }

            BindBatch();
            BindDegree();
            if (ddldegree.Items.Count > 0)
            {
                ddldegree.Enabled = true;
                ddlbranch.Enabled = true;
                ddlsemester.Enabled = true;
                ddlsection.Enabled = true;
                btngo.Enabled = true;
                txtfromdate.Enabled = true;
                txttodate.Enabled = true;
                cbcumpercent.Enabled = true;
                bindbranch();
                bindsem();
                BindSectionDetail();
            }
            else
            {
                ddldegree.Enabled = false;
                ddlbranch.Enabled = false;
                ddlsemester.Enabled = false;
                ddlsection.Enabled = false;
                btngo.Enabled = false;
                cbcumpercent.Enabled = false;
            }
            chkonduty.Checked = true;//added by srinath 22/1/2014
            loadonduty();
            chkondutyspit.Checked = true;
            txtonduty.Visible = true;
            ponduty.Visible = true;
            loadonduty();
            btnclose.Visible = false;
            string grouporusercodeschool = string.Empty;
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
                    forschoolsetting = true;
                    //lblcolg.Text = "School";
                    Label1.Text = "Year";
                    Label2.Text = "School Type";
                    lblbranch.Text = "Standard";
                    lblsem.Text = "Term";
                    //lblDegree.Attributes.Add("style", " width: 95px;");
                    //lblBranch.Attributes.Add("style", " width: 67px;");
                    //ddlBranch.Attributes.Add("style", " width: 241px;");
                }
                else
                {
                    // forschoolsetting = false;
                }
            }
            //} Sridharan


        }

    }

    public void BindSectionDetail()
    {

        ddlsection.Items.Clear();
        con.Close();
        con.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlsection.DataSource = ds;
        ddlsection.DataTextField = "sections";
        ddlsection.DataBind();
        ddlsection.Items.Insert(0, "All");
        SqlDataReader dr_sec;
        dr_sec = cmd.ExecuteReader();
        dr_sec.Read();
        if (dr_sec.HasRows == true)
        {
            if (dr_sec["sections"].ToString() == string.Empty)
            {
                ddlsection.Enabled = false;
            }
            else
            {
                ddlsection.Enabled = true;
            }
        }
        else
        {
            ddlsection.Enabled = false;
        }
        con.Close();
    }

    public void bindsem()
    {

        //--------------------semester load
        ddlsemester.Items.Clear();
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
                    ddlsemester.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlsemester.Items.Add(i.ToString());
                }

            }
        }
        else
        {
            dr.Close();
            SqlDataReader dr1;
            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
            ddlsemester.Items.Clear();
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
                        ddlsemester.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsemester.Items.Add(i.ToString());
                    }
                }
            }

            dr1.Close();
        }
        //FpMarkEntry.Visible = false;
        con.Close();
    }

    public void Get_Semester()
    {
        ddlsemester.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        string batch_calcode_degree;

        string batch = ddlbatch.SelectedValue.ToString();
        string collegecode = Session["collegecode"].ToString();
        string degree = ddlbranch.SelectedValue.ToString();
        batch_calcode_degree = batch.ToString() + "/" + collegecode.ToString() + "/" + degree.ToString();
        DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlsemester.Enabled = true;
            first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

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
        else
        {
            ddlsemester.Enabled = false;
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
            cmd = new SqlCommand("select start_date,end_date from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsemester.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " ", con);
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
                txtfromdate.Text = final_from;
                Session["fromdate"] = final_from;
                //------------get to date
                to_date = dr_dateset[1].ToString();
                string[] to_split = to_date.Split(' ');
                string[] date_split_to = to_split[0].Split('/');
                final_to = date_split_to[1] + "/" + date_split_to[0] + "/" + date_split_to[2];
                txttodate.Text = final_to;
                Session["todate"] = final_to;
                sem_end = final_to;
            }
            else
            {
                string dt = DateTime.Today.ToShortDateString();
                string[] dsplit = dt.Split(new Char[] { '/' });
                txttodate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();

                txtfromdate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            }


        }
        catch
        {
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        //Session["attdaywisecla"] = "1";
        btnPrint11();
        btnclose.Visible = false;
        btnletter.Visible = false;
        txtexcelname.Text = string.Empty;
        Session["checkflag"] = "1";
        if (txtfromrange.Text.Trim() != "" && txttorange.Text.Trim() == "")
        {
            lblnorec.Visible = true;
            lblnorec.Text = "Please Enter To Range Value";
            return;
        }
        if (txtfromrange.Text.Trim() == "" && txttorange.Text.Trim() != "")
        {
            lblnorec.Visible = true;
            lblnorec.Text = "Please Enter From Range Value";
            return;
        }
        function_btnclick();
        //make_border1();


    }

    public void function_btnclick()
    {
        //  try
        {
            dateerr.Visible = false;
            if ((txtfromdate.Text == string.Empty) || (tbfmcumdate.Text == string.Empty))
            {
                dateerr.Visible = true;
                dateerr.Text = "Select From Date";
                Showgrid.Visible = false;
                btnletter.Visible = false;
                btnxl.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                Printcontrol.Visible = false;
                btnletter.Visible = false;
            }
            else
            {
                dateerr.Visible = false;
                dateerr.Text = string.Empty;
            }

            if ((txttodate.Text == string.Empty) || (tbtocumdate.Text == string.Empty))
            {
                dateerr.Visible = true;
                dateerr.Text = "Select To Date";
                Showgrid.Visible = false;
                btnletter.Visible = false;
                btnxl.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                Printcontrol.Visible = false;
            }
            else
            {
                dateerr.Visible = false;
                dateerr.Text = string.Empty;
            }


            first_btngo();


            //  if (Request.QueryString["val"] == null)
            {

                //if (FpSpread1.Sheets[0].RowCount > 0)
                //{
                //lblpages.Visible = true;
                //ddlpage.Visible = true;

                //--------------------------------------------------- defn for settings
                //if (Session["Rollflag"].ToString() == "0")
                //{
                //    FpSpread1.Sheets[0].ColumnHeader.Columns[1].Visible = false;
                //}
                //if (Session["Regflag"].ToString() == "0")
                //{
                //    FpSpread1.Sheets[0].ColumnHeader.Columns[2].Visible = false;
                //}
                //if (Session["studflag"].ToString() == "0")
                //{
                //    FpSpread1.Sheets[0].ColumnHeader.Columns[4].Visible = false;
                //}
                //-----------------------------------------------13/6/12 PRABHA
                //setheader();////////////////////////////////
                //{
                //    final_print_col_cnt = 0;
                //    for (int col = 0; col < FpSpread1.Sheets[0].ColumnCount; col++)
                //    {
                //        if (FpSpread1.Sheets[0].Columns[col].Visible == true)
                //        {
                //            final_print_col_cnt++;
                //        }
                //    }



                //MyImg mi = new MyImg();
                //mi.ImageUrl = "~/images/10BIT001.jpeg";
                //mi.ImageUrl = "Handler/Handler2.ashx?";
                //MyImg mi2 = new MyImg();
                //mi2.ImageUrl = "~/images/10BIT001.jpeg";
                //mi2.ImageUrl = "Handler/Handler5.ashx?";

                //for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
                //{
                //    if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
                //    {
                //        if (temp_count == 0)
                //        {
                //            start_column = col_count;
                //            FpSpread1.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
                //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, start_column, (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1);
                //            //if (leftlogo == "1" && leftlength != "")
                //            //{
                //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
                //            //}
                //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
                //        }


                //        if (final_print_col_cnt == temp_count + 1)
                //        {
                //            end_column = col_count;

                //            {
                //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1);
                //                //if (rightlogo == "1" && rightlength != "")
                //                //{
                //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi2;
                //                //}
                //                FpSpread1.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
                //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
                //            }

                //        }
                //        temp_count++;
                //        if (final_print_col_cnt == temp_count)
                //        {
                //            break;
                //        }
                //    }
                //}
                //    temp_count = 0;
                //    for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
                //    {
                //        if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
                //        {
                //            if (temp_count == 1)
                //            {
                //                more_column();

                //                //aruna FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, start_column, (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1);
                //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, start_column, (FpSpread1.Sheets[0].ColumnHeader.RowCount - 3), 1);
                //                if (leftlogo == "1" && leftlength != "")
                //                {
                //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, start_column].CellType = mi;
                //                }
                //                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, end_column, (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1);
                //                if (rightlogo == "1" && rightlength != "")
                //                {
                //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, end_column].CellType = mi2;
                //                }

                //                for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount - 2; row_cnt++)
                //                {
                //                    FpSpread1.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
                //                    FpSpread1.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
                //                }
                //            }
                //            temp_count++;
                //        }
                //    }
                //}
                //-------------------------------------------------------------------------------------------------
                //load_pageddl();
                //}
                //else
                //{
                //    Showgrid.Visible = false;
                //    btnletter.Visible = false;
                //    btnprintmaster.Visible = false;
                //    Printcontrol.Visible = false;
                //    // pagesetpanel.Visible = false;
                //}

                if (data.Columns.Count > 0)
                {
                    int wid = 0;


                }
                else
                {
                    Showgrid.Visible = false;
                    btnletter.Visible = false;
                    btnprintmaster.Visible = false;
                    btnPrint.Visible = false;
                    Printcontrol.Visible = false;

                }
            }
            inirow_count = Showgrid.Rows.Count;
        }
        //   catch
        {
        }
    }

    public void first_btngo()
    {
        try
        {


            if (((txtfromdate.Text != string.Empty) && (tbfmcumdate.Text != string.Empty)) && ((txttodate.Text != string.Empty) && (tbtocumdate.Text != string.Empty)))
            {
                //'----------------------------------------font style---------------------------

                //'----------------------------------------------------------------------------------
                //'---------------------------------------------date validate-------------
                string valfromdate = string.Empty;
                string valtodate = string.Empty;
                string frmconcat = string.Empty;

                valfromdate = txtfromdate.Text.ToString();
                string[] split1 = valfromdate.Split(new char[] { '/' });
                frmconcat = split1[1].ToString() + '/' + split1[0].ToString() + '/' + split1[2].ToString();
                DateTime dtfromdate = Convert.ToDateTime(frmconcat.ToString());

                valtodate = txttodate.Text.ToString();
                string[] split2 = valtodate.Split(new char[] { '/' });
                frmconcat = split2[1].ToString() + '/' + split2[0].ToString() + '/' + split2[2].ToString();
                DateTime dttodate = Convert.ToDateTime(frmconcat.ToString());
                Boolean date_diff_flag_cum = false;
                //===================cum date check
                if (cbcumpercent.Checked == true)
                {
                    valfromdate = tbfmcumdate.Text.ToString();
                    string[] split1_cum = valfromdate.Split(new char[] { '/' });
                    frmconcat = split1_cum[1].ToString() + '/' + split1_cum[0].ToString() + '/' + split1_cum[2].ToString();
                    DateTime dtfromdate_cum = Convert.ToDateTime(frmconcat.ToString());

                    valtodate = tbtocumdate.Text.ToString();
                    string[] split2_cum = valtodate.Split(new char[] { '/' });
                    frmconcat = split2_cum[1].ToString() + '/' + split2_cum[0].ToString() + '/' + split2_cum[2].ToString();
                    DateTime dttodate_cum = Convert.ToDateTime(frmconcat.ToString());
                    //=====================
                    TimeSpan ts_cum = dttodate_cum.Subtract(dtfromdate_cum);
                    int days_cum = ts_cum.Days;
                    if (days_cum > 0)
                    {
                        date_diff_flag_cum = true;
                    }
                }
                else
                {
                    date_diff_flag_cum = true;
                }
                TimeSpan ts = dttodate.Subtract(dtfromdate);
                int days = ts.Days;
                if (days < 0)
                {
                    dateerr.Text = "From Date Should Be Less Than To Date";
                    dateerr.Visible = true;
                    Showgrid.Visible = false;
                    btnletter.Visible = false;
                    btnxl.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnprintmaster.Visible = false;
                    Printcontrol.Visible = false;
                    btnPrint.Visible = false;
                    //pagesetpanel.Visible = false;


                }
                else
                {
                    if (date_diff_flag_cum == true)
                    {
                        con.Close();
                        con.Open();
                        string attnd_points = "select *from leave_points";
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



                        dateerr.Text = string.Empty;
                        dateerr.Visible = false;
                        Showgrid.Visible = true;
                        btnletter.Visible = true;
                        btnxl.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnprintmaster.Visible = false;
                        btnPrint.Visible = false;
                        Printcontrol.Visible = false;
                        if (lblnorec.Visible == false)
                        {
                            if (cbcumpercent.Checked == true)
                            {
                                //  FpSpread1.Sheets[0].Columns[3].Width = 400;
                                //----------------------------date validation for cumulative %-----------------------
                                string valfromdate1 = string.Empty;
                                string valtodate1 = string.Empty;
                                string frmconcat1 = string.Empty;

                                valfromdate1 = tbfmcumdate.Text.ToString();
                                string[] split3 = valfromdate1.Split(new char[] { '/' });
                                frmconcat1 = split3[1].ToString() + '/' + split3[0].ToString() + '/' + split3[2].ToString();
                                DateTime dtfromdate1 = Convert.ToDateTime(frmconcat1.ToString());

                                valtodate1 = tbtocumdate.Text.ToString();
                                string[] split4 = valtodate1.Split(new char[] { '/' });
                                frmconcat1 = split4[1].ToString() + '/' + split4[0].ToString() + '/' + split4[2].ToString();
                                DateTime dttodate1 = Convert.ToDateTime(frmconcat1.ToString());
                                TimeSpan ts1 = dttodate1.Subtract(dtfromdate1);
                                days1 = ts1.Days;
                                if (days1 < 0)
                                {

                                    lblnorec.Visible = false;
                                    dateerr.Text = "From Date Should Be Less Than To Date";
                                    dateerr.Visible = true;
                                    Showgrid.Visible = false;
                                    btnletter.Visible = false;
                                    btnxl.Visible = false;
                                    lblrptname.Visible = false;
                                    txtexcelname.Visible = false;
                                    btnprintmaster.Visible = false;
                                    Printcontrol.Visible = false;
                                    btnPrint.Visible = false;


                                }
                                else
                                {
                                    dateerr.Text = string.Empty;
                                    dateerr.Visible = false;
                                    Showgrid.Visible = true;
                                    btnletter.Visible = true;
                                    btnxl.Visible = false;
                                    lblrptname.Visible = false;
                                    txtexcelname.Visible = false;
                                    btnprintmaster.Visible = false;
                                    Printcontrol.Visible = false;
                                    btnPrint.Visible = false;
                                    //'--------------------------------------------------------------------
                                    //  spsize();
                                    // spsizeforcum();
                                    //  spsizeforcum_header();

                                }
                            }
                            else
                            {
                                //spsize();
                            }
                            loadheader();
                            if (lblnorec.Visible == false)
                            {
                                load_students();
                            }
                        }
                    }
                    else
                    {
                        dateerr.Text = "Cumulative From Date Should Be Less Than To Date";
                        dateerr.Visible = true;
                        Showgrid.Visible = false;
                        btnletter.Visible = false;
                        btnxl.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnprintmaster.Visible = false;
                        Printcontrol.Visible = false;
                        btnPrint.Visible = false;

                    }
                }
            }
            //Rajkumar 22/12/2017 //last modified by prabha kan 23 2018
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
                    Showgrid.Visible = false;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = Noresult + " " + " Attendance not Found";
                    divPopAlert.Visible = true;
                    btnletter.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnprintmaster.Visible = false;
                    btnxl.Visible = false;

                }
                //return;  //modified on prabha on 15/12/2017
            }
        }
        catch
        {
        }
    }

    private void spsizeforcum_header()
    {
        ////   try
        //{
        //    if (cbpoints.Checked == true)
        //    {
        //        FpSpread1.Sheets[0].RowCount = 0;
        //        FpSpread1.Sheets[0].ColumnCount = 28;
        //    }
        //    else
        //    {

        //        FpSpread1.Sheets[0].ColumnCount = 27;
        //    }
        //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 17, 1, 6);
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 17].Text = "Period Wise Cumulative Percentage From " + tbfmcumdate.Text + " To " + tbtocumdate.Text;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 17].Text = "Cum. Conducted Periods";
        //    FpSpread1.Sheets[0].Columns[17].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 18].Text = "Cum. Attended Periods";
        //    FpSpread1.Sheets[0].Columns[18].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 19].Text = "Cum Onduty Periods";
        //    FpSpread1.Sheets[0].Columns[19].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 20].Text = "Cum ML Periods";
        //    FpSpread1.Sheets[0].Columns[20].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 21].Text = "Cum Absent Periods";
        //    FpSpread1.Sheets[0].Columns[21].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 22].Text = "Att Period Percentage";
        //    FpSpread1.Sheets[0].Columns[22].HorizontalAlign = HorizontalAlign.Center;

        //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 23, 2, 1);
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 23].Text = "No Of Days Absent";
        //    FpSpread1.Sheets[0].Columns[23].HorizontalAlign = HorizontalAlign.Center;

        //    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 24].Text = " Cum No Of Days Absent";
        //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 24, 2, 1);
        //    FpSpread1.Sheets[0].Columns[24].HorizontalAlign = HorizontalAlign.Center;

        //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 25, 2, 1);
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 25].Text = "No of Days Leave ";
        //    FpSpread1.Sheets[0].Columns[25].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 26, 2, 1);
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 26].Text = "No Of Days OD ";
        //    FpSpread1.Sheets[0].Columns[26].HorizontalAlign = HorizontalAlign.Center;

        //    if (cbpoints.Checked == true)
        //    {
        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 27, 2, 1);
        //        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 27].Text = "PTS";
        //    }

        //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 14, 1, 3);
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 14].Text = "Days Wise Cumulative Percentage From " + tbfmcumdate.Text + " To " + tbtocumdate.Text;
        //    FpSpread1.Sheets[0].Columns[14].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 15].Text = "Cum Attended Days";
        //    FpSpread1.Sheets[0].Columns[15].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 14].Text = "Cum Conducted Days";
        //    FpSpread1.Sheets[0].Columns[14].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 16].Text = "Att Percentage";
        //    FpSpread1.Sheets[0].Columns[16].HorizontalAlign = HorizontalAlign.Center;
        //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 25, 2, 1);

        //}
        ////  catch
        //{
        //}
    }

    private void spsize()
    {
        ////  try
        //{
        //    FpSpread1.Sheets[0].RowCount = 0;
        //    FpSpread1.Sheets[0].ColumnCount = 0;
        //    // FpSpread1.Sheets[0].ColumnHeader.RowCount = 7;
        //    FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;//Modified By Srinath 13/5/2013
        //    FpSpread1.Sheets[0].ColumnCount = 17;
        //    FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
        //    FpSpread1.Sheets[0].Columns[1].CellType = textcel_type;
        //    FpSpread1.Sheets[0].Columns[2].CellType = textcel_type;
        //    FpSpread1.Sheets[0].Columns[3].Width = 200;
        //    FpSpread1.Sheets[0].Columns[2].Width = 150;
        //    // FpSpread1.Sheets[0].Columns[0].Width = 200;
        //    FpSpread1.Sheets[0].Columns[1].Width = 100;
        //    FpSpread1.Sheets[0].Columns[4].Width = 100;
        //    FpSpread1.Sheets[0].Columns[5].Width = 100;
        //    FpSpread1.Sheets[0].Columns[6].Width = 100;
        //    FpSpread1.Sheets[0].Columns[7].Width = 100;
        //    FpSpread1.Sheets[0].Columns[8].Width = 100;
        //    FpSpread1.Sheets[0].Columns[9].Width = 100;
        //    FpSpread1.Sheets[0].Columns[10].Width = 100;
        //    FpSpread1.Sheets[0].Columns[11].Width = 100;
        //    FpSpread1.Sheets[0].Columns[12].Width = 100;
        //    FpSpread1.Sheets[0].Columns[13].Width = 100;
        //    FpSpread1.Sheets[0].Columns[14].Width = 100;
        //    spsize_header();
        //    if (cbcumpercent.Checked == true)
        //    {

        //        if (days1 < 0)
        //        {

        //        }
        //        else
        //        {
        //            if (cbpoints.Checked == true)
        //            {
        //                FpSpread1.Sheets[0].RowCount = 0;
        //                FpSpread1.Sheets[0].ColumnCount = 25;
        //            }
        //            else
        //            {

        //                FpSpread1.Sheets[0].ColumnCount = 24;
        //            }
        //            spsize_header();
        //            spsizeforcum_header();


        //        }
        //    }
        //    else
        //    {
        //        spsize_header();
        //    }



        //}
        ////  catch
        //{
        //}
    }

    public void loadheader()
    {

        try
        {


            cumcolscnt = 0;
            cumcolspancnt = 0;
            ArrayList arrColHdrNames1 = new ArrayList();
            ArrayList arrColHdrNames2 = new ArrayList();
            ArrayList arrColHdrNames3 = new ArrayList();

            arrColHdrNames1.Add("S.No");
            arrColHdrNames2.Add("S.No");
            arrColHdrNames3.Add("S.No");
            data.Columns.Add("SNo", typeof(string));
            if (Session["Rollflag"].ToString() == "1")
            {
                colstartcnt++;
                data.Columns.Add("Roll No", typeof(string));
                arrColHdrNames1.Add("RollNo");
                arrColHdrNames2.Add("RollNo");
                arrColHdrNames3.Add("RollNo");
            }
            else
            {
                colstartcnt++;
                data.Columns.Add("Roll No", typeof(string));
                arrColHdrNames1.Add("RollNo");
                arrColHdrNames2.Add("RollNo");
                arrColHdrNames3.Add("RollNo");

            }
            if (Session["Regflag"].ToString() == "1")
            {
                colstartcnt++;
                data.Columns.Add("Register No", typeof(string));
                arrColHdrNames1.Add("Register No");
                arrColHdrNames2.Add("Register No");
                arrColHdrNames3.Add("Register No");
            }
            arrColHdrNames1.Add("Name of the Student");
            arrColHdrNames2.Add("Name of the Student");
            arrColHdrNames3.Add("Name of the Student");
            data.Columns.Add("Name of the Student", typeof(string));
            if (Session["studflag"].ToString() == "1")
            {
                colstartcnt++;
                data.Columns.Add("Student Type", typeof(string));
                arrColHdrNames1.Add("Student Type");
                arrColHdrNames2.Add("Student Type");
                arrColHdrNames3.Add("Student Type");
            }
            colstartcnt = colstartcnt + 2;
            colscnt = colstartcnt;

            if (Session["daywise"].ToString() != "0")
            {
                arrColHdrNames1.Add("Working Days");
                arrColHdrNames1.Add("Days Present");
                arrColHdrNames1.Add("% of Attendance");
                arrColHdrNames2.Add("Working Days");
                arrColHdrNames2.Add("Days Present");
                arrColHdrNames2.Add("% of Attendance");
                arrColHdrNames3.Add("Working Days");
                arrColHdrNames3.Add("Days Present");
                arrColHdrNames3.Add("% of Attendance");

                data.Columns.Add("Working Days", typeof(string));
                data.Columns.Add("Days Present", typeof(string));
                data.Columns.Add("% of Attendance", typeof(string));

                colscnt = colscnt + 3;
            }

            if (Session["hourwise"].ToString() != "0")
            {

                arrColHdrNames1.Add("Hour Wise Percentage From " + txtfromdate.Text + " To " + txttodate.Text);
                arrColHdrNames1.Add("Hour Wise Percentage From " + txtfromdate.Text + " To " + txttodate.Text);


                arrColHdrNames2.Add("Conducted Hours");
                arrColHdrNames2.Add("Attended Hours");

                arrColHdrNames3.Add("Conducted Hours");
                arrColHdrNames3.Add("Attended Hours");

                data.Columns.Add("Conducted Hours", typeof(string));
                data.Columns.Add("Attended Hours", typeof(string));
            }



            if (chkondutyspit.Checked == true)
            {
                colon = true;
                for (int i = 0; i < chklsonduty.Items.Count; i++)
                {
                    if (chklsonduty.Items[i].Selected == true)
                    {
                        ondutycount++;
                        string val = chklsonduty.Items[i].Text.Trim().ToLower();
                        string val1 = chklsonduty.Items[i].Text;

                        if (hatonduty.Contains(val))
                        {

                            hatonduty.Add(val, "0");
                        }
                        if (strondutyvalue == "")
                        {
                            strondutyvalue = val;
                        }
                        else
                        {
                            strondutyvalue = strondutyvalue + ',' + val;
                        }
                        data.Columns.Add(val1, typeof(string));
                        arrColHdrNames1.Add("Hour Wise Percentage From " + txtfromdate.Text + " To " + txttodate.Text);
                        arrColHdrNames2.Add("Onduty Hours");
                        arrColHdrNames3.Add(val1);
                        colspancnt++;
                    }
                }
            }
            else
            {
                ondutycount = 0;
            }
            if (ondutycount <= 0)
            {

                ondutycount = 1;

            }
            else
            {

            }

            if (!colon)
            {
                data.Columns.Add("Onduty Hours");
                arrColHdrNames1.Add("Hour Wise Percentage From " + txtfromdate.Text + " To " + txttodate.Text);
                arrColHdrNames2.Add("Onduty Hours");
                arrColHdrNames3.Add("Onduty Hours");
                colspancnt++;

            }
            //arrColHdrNames2.Add("Onduty Hours");
            //arrColHdrNames3.Add("Onduty Hours");
            //data.Columns.Add("Onduty Hours", typeof(string));
            if (Session["hourwise"].ToString() != "0")
            {
                arrColHdrNames1.Add("Hour Wise Percentage From " + txtfromdate.Text + " To " + txttodate.Text);
                arrColHdrNames1.Add("Hour Wise Percentage From " + txtfromdate.Text + " To " + txttodate.Text);


                arrColHdrNames2.Add("ML Hours");
                arrColHdrNames2.Add("Absent Hours");

                arrColHdrNames3.Add("ML Hours");
                arrColHdrNames3.Add("Absent Hours");

                data.Columns.Add("ML Hours", typeof(string));
                data.Columns.Add("Absent Hours", typeof(string));
                colspancnt = colspancnt + 2;
            }

            arrColHdrNames1.Add("% of Attendance");
            arrColHdrNames2.Add("% of Attendance");
            arrColHdrNames3.Add("% of Attendance");
            colspancnt++;
            System.Text.StringBuilder att = new System.Text.StringBuilder("% of Attendance");

            AddTableColumn(data, att);

            if (cbcumpercent.Checked == true)
            {
                if (Session["daywise"].ToString() != "0")
                {
                    arrColHdrNames1.Add("Days Wise Cumulative Percentage From " + tbfmcumdate.Text + " To " + tbtocumdate.Text);
                    arrColHdrNames1.Add("Days Wise Cumulative Percentage From " + tbfmcumdate.Text + " To " + tbtocumdate.Text);
                    arrColHdrNames1.Add("Days Wise Cumulative Percentage From " + tbfmcumdate.Text + " To " + tbtocumdate.Text);

                    arrColHdrNames2.Add("Cum Conducted Days");
                    arrColHdrNames2.Add("Cum Attended Days");
                    arrColHdrNames2.Add("Cum Att Percentage");

                    arrColHdrNames3.Add("Cum Conducted Days");
                    arrColHdrNames3.Add("Cum Attended Days");
                    arrColHdrNames3.Add("Cum Att Percentage");


                    data.Columns.Add("Cum Conducted Days", typeof(string));
                    data.Columns.Add("Cum Attended Days", typeof(string));
                    data.Columns.Add("Cum Att Percentage", typeof(string));

                    cumcolscnt = cumcolscnt + 3;
                }

                if (Session["hourwise"].ToString() != "0")
                {

                    arrColHdrNames1.Add("Hours Wise Cumulative Percentage From " + tbfmcumdate.Text + " To " + tbtocumdate.Text);
                    arrColHdrNames1.Add("Hours Wise Cumulative Percentage From " + tbfmcumdate.Text + " To " + tbtocumdate.Text);


                    arrColHdrNames2.Add("Cum Conducted Hours");
                    arrColHdrNames2.Add("Cum Attended Hours");


                    arrColHdrNames3.Add("Cum Conducted Hours");
                    arrColHdrNames3.Add("Cum Attended Hours");


                    data.Columns.Add("Cum. Conducted Hours", typeof(string));
                    data.Columns.Add("Cum. Attended Hours", typeof(string));

                    cumcolscnt = cumcolscnt + 2;
                }



                ondutycount = 0;


                if (chkondutyspit.Checked == true)
                {
                    colon = true;
                    for (int i = 0; i < chklsonduty.Items.Count; i++)
                    {
                        if (chklsonduty.Items[i].Selected == true)
                        {
                            ondutycount++;

                            string val = chklsonduty.Items[i].Text;
                            string val1 = chklsonduty.Items[i].Text;

                            if (hatonduty.Contains(val))
                            {

                                hatonduty.Add(val, "0");
                            }
                            if (strondutyvalue == "")
                            {
                                strondutyvalue = val;
                            }
                            else
                            {
                                strondutyvalue = strondutyvalue + ',' + val;
                            }
                            System.Text.StringBuilder ondut = new System.Text.StringBuilder(val1);

                            AddTableColumn(data, ondut);

                            arrColHdrNames1.Add("Hours Wise Cumulative Percentage From " + tbfmcumdate.Text + " To " + tbtocumdate.Text);
                            arrColHdrNames2.Add("Cum Onduty Hours");
                            arrColHdrNames3.Add(val1);
                            cumcolspancnt++;
                        }
                    }
                }
                else
                {
                    ondutycount = 0;
                }
                if (ondutycount == 0)
                {

                    ondutycount = 1;
                }
                if (!colon)
                {
                    data.Columns.Add("Cum Onduty Hours");
                    arrColHdrNames1.Add("Hours Wise Cumulative Percentage From " + tbfmcumdate.Text + " To " + tbtocumdate.Text);
                    arrColHdrNames2.Add("Cum Onduty Hours");
                    arrColHdrNames3.Add("Cum Onduty Hours");
                    colspancnt++;
                }

                if (Session["hourwise"].ToString() != "0")
                {

                    arrColHdrNames1.Add("Hours Wise Cumulative Percentage From " + tbfmcumdate.Text + " To " + tbtocumdate.Text);
                    arrColHdrNames1.Add("Hours Wise Cumulative Percentage From " + tbfmcumdate.Text + " To " + tbtocumdate.Text);
                    arrColHdrNames1.Add("Hours Wise Cumulative Percentage From " + tbfmcumdate.Text + " To " + tbtocumdate.Text);


                    arrColHdrNames2.Add("Cum ML Hours");
                    arrColHdrNames2.Add("Cum Absent Hours");
                    arrColHdrNames2.Add("% of Attendance");

                    arrColHdrNames3.Add("Cum ML Hours");
                    arrColHdrNames3.Add("Cum Absent Hours");
                    arrColHdrNames3.Add("% of Attendance");

                    data.Columns.Add("Cum ML Hours", typeof(string));
                    data.Columns.Add("Cum Absent Hours", typeof(string));


                    System.Text.StringBuilder onduty = new System.Text.StringBuilder("% of Attendance");
                    AddTableColumn(data, onduty);

                    cumcolspancnt1 = 3;
                }


            }
            arrColHdrNames1.Add("Days Absent");
            arrColHdrNames2.Add("Days Absent");
            arrColHdrNames3.Add("Days Absent");
            data.Columns.Add("Days Absent", typeof(string));
            if (cbcumpercent.Checked == true)
            {
                arrColHdrNames1.Add("Cum No Of Days Absent");
                arrColHdrNames2.Add("Cum No Of Days Absent");
                arrColHdrNames3.Add("Cum No Of Days Absent");
                data.Columns.Add(" Cum No Of Days Absent", typeof(string));
            }
            arrColHdrNames1.Add("Days Leave");
            arrColHdrNames2.Add("Days Leave");
            arrColHdrNames3.Add("Days Leave");
            arrColHdrNames1.Add("OD");
            arrColHdrNames2.Add("OD");
            arrColHdrNames3.Add("OD");
            data.Columns.Add("Days Leave", typeof(string));

            System.Text.StringBuilder text = new System.Text.StringBuilder("OD");
            AddTableColumn(data, text);

            if (cbpoints.Checked == true)
            {
                arrColHdrNames1.Add("PTS");
                arrColHdrNames2.Add("PTS");
                arrColHdrNames3.Add("PTS");
                data.Columns.Add("PTS", typeof(string));

            }
            arrColHdrNames1.Add("Signature");
            arrColHdrNames2.Add("Signature");
            arrColHdrNames3.Add("Signature");
            data.Columns.Add("Signature", typeof(string));


            //dicrowspan.Add(0,);

            DataRow drHdr1 = data.NewRow();
            DataRow drHdr2 = data.NewRow();
            DataRow drHdr3 = data.NewRow();
            DataRow drHdr4 = data.NewRow();

            for (int grCol = 0; grCol < data.Columns.Count; grCol++)
            {
                if (arrColHdrNames1.Count > 0)
                    drHdr1[grCol] = arrColHdrNames1[grCol];
                if (arrColHdrNames2.Count > 0)
                    drHdr2[grCol] = arrColHdrNames2[grCol];
                if (arrColHdrNames3.Count > 0)
                    drHdr3[grCol] = arrColHdrNames3[grCol];

            }
            if (arrColHdrNames1.Count > 0)
                data.Rows.Add(drHdr1);
            if (arrColHdrNames2.Count > 0)
                data.Rows.Add(drHdr2);
            if (arrColHdrNames3.Count > 0)
                data.Rows.Add(drHdr3);
            colcount2 = data.Rows.Count;
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

    private void spsize_header()
    {
        //  try
        {


            // FpSpread1.Sheets[0].ColumnHeader.RowCount = FpSpread1.Sheets[0].ColumnHeader.RowCount + 2;//Hidden By Srinath 13/5/2013

            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 0, 2, 1);
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 0].Text = "S.No";
            //FpSpread1.Sheets[0].Columns[0].Width = 50;
            //FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1, 2, 1);
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1].Text = "Roll No";
            //FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 2, 2, 1);
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 2].Text = "Register No";
            //FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 3, 2, 1);
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 3].Text = "Name";
            //FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 4, 2, 1);
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 4].Text = "StudentType";
            //FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 5, 1, 3);
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 5].Text = "Day Wise Percentage From " + txtfromdate.Text + " To " + txttodate.Text;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 6].Text = "Attended Days";
            //FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 5].Text = "Conducted Days";
            //FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 7].Text = "% of Attendance";
            //FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 8, 1, 6);
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 8].Text = "Hour Wise Percentage From " + txtfromdate.Text + " To " + txttodate.Text;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 9].Text = "Attended Hours";
            //FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 8].Text = "Conducted Hours";
            //FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 12].Text = "Absent Hours";
            //FpSpread1.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 13].Text = "Att Hours Percentage";
            //FpSpread1.Sheets[0].Columns[13].HorizontalAlign = HorizontalAlign.Center;

            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 8, 2, 1);
            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 9, 2, 1);

            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 10, 1, 1);
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 10].Text = "Onduty Hours";

            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 11, 1, 1);
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 11].Text = "ML Hours";



            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 13, 2, 1);
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 14].Text = "No of Days Absent ";
            //FpSpread1.Sheets[0].Columns[13].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 14, 2, 1);
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 15].Text = "No of Days Leave ";
            //FpSpread1.Sheets[0].Columns[14].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 15, 2, 1);
            //FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 16].Text = "No of Days OD ";
            //FpSpread1.Sheets[0].Columns[15].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].Columns[16].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 16, 2, 1);

            //FpSpread1.Sheets[0].ColumnHeader.Rows[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1)].Tag = 1;
            //FpSpread1.Sheets[0].ColumnHeader.Rows[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2)].Tag = 1;
        }
        //  catch
        {
        }
    }

    public void filteration()
    {

        string orderby_Setting = GetFunction("select value from master_Settings where settings='order_by'");

        if (orderby_Setting == "")
        {
            strorder = string.Empty;
            strregorder = string.Empty;
        }
        else
        {
            if (orderby_Setting == "0")
            {
                strorder = "ORDER BY r.Roll_No";
                strregorder = "ORDER BY registration.Roll_No";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "ORDER BY r.Reg_No";
                strregorder = "ORDER BY registration.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strregorder = "ORDER BY registration.Stud_Name";
                strorder = "ORDER BY r.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                strregorder = "ORDER BY registration.Roll_No,registration.Reg_No,registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "ORDER BY r.Roll_No,r.Reg_No";
                strregorder = "ORDER BY registration.Roll_No,registration.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "ORDER BY r.Reg_No,r.Stud_Name";
                strregorder = "ORDER BY registration.Reg_No,registration.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "ORDER BY r.Roll_No,r.Stud_Name";
                strregorder = "ORDER BY registration.Roll_No,registration.Stud_Name";
            }
        }

    }

    public void load_students()
    {
        dicletterreport.Clear();
        hasdaywise.Clear();
        hashrwise.Clear();
        bool cbDayorderwise = false;
        try
        {
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            DataSet dsSettings = new DataSet();

            dsSettings = d2.select_method_wo_parameter("select * from Master_Settings where settings='DayOrderWisePeriodAttendanceSettings' and" + grouporusercode + "", "Text");
            if (dsSettings.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(dsSettings.Tables[0].Rows[0]["value"]) == "0")
                {
                    cbDayorderwise = false;
                }
                else if (Convert.ToString(dsSettings.Tables[0].Rows[0]["value"]) == "1")
                {
                    cbDayorderwise = true;
                }
            }
            string sec;
            if (ddlsection.Enabled == true)
            {
                if (ddlsection.SelectedItem.ToString() == string.Empty || ddlsection.Text == "All")
                {
                    sec = string.Empty;
                }
                else
                {
                    sec = ddlsection.SelectedItem.ToString();

                }
            }
            else
            {
                sec = string.Empty;
            }
            #region Attendance
            bool incRedo = false;
            string stvfa = d2.GetFunctionv("select value from Master_Settings where settings = 'Include Redo student in Attendance'");
            if (stvfa.Trim() == "1")
            {
                incRedo = true;
            }
            #endregion

            string includedel = "and delflag=0";
            if (CheckBox1.Checked)
                includedel = string.Empty;

            filteration();
            string filterwithsection = "exam_flag<>'debar' " + includedel + " and batch_year='" + ddlbatch.SelectedItem.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "'   and sections='" + sec.ToString() + "' " + strorder + "";//

            string filterwithoutsection = "exam_flag<>'debar' " + includedel + " and batch_year='" + ddlbatch.SelectedItem.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' " + strorder + "";// 

            string filteredo = "select distinct roll_no as ROLL_NO,Reg_No as REG_NO,Stud_Name as STUD_NAME,Roll_Admit as ADMIT_NO, stud_type as Student_Type, len(roll_no ), convert(varchar(15),adm_date,103) as adm_date from registration r where batch_year='" + ddlbatch.SelectedItem.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and ISNULL(isRedo,0)=1";
            DataSet dsredo = d2.select_method_wo_parameter(filteredo, "text");
            // DataTable dtrero = dirAcc.selectDataTable(filteredo);

            hat.Clear();
            hat.Add("bath", int.Parse(ddlbatch.SelectedItem.ToString()));
            hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
            hat.Add("sec", sec.ToString());
            hat.Add("filterwithsection", filterwithsection.ToString());
            hat.Add("filterwithoutsection", filterwithoutsection.ToString());

            ds4 = d2.select_method("ALL_STUDENT_DETAILS", hat, "sp");
            if (incRedo)
            {
                if (dsredo.Tables[0].Rows.Count > 0)
                    ds4.Tables[0].Merge(dsredo.Tables[0]);
            }
            // string sqlStr =string.Empty;
            string sections = string.Empty;
            string strsec = string.Empty;
            sections = ddlsection.SelectedValue.ToString();
            string sectvalc = string.Empty;
            if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
            {
                strsec = string.Empty;
            }
            else
            {
                strsec = " and sections='" + sections.ToString() + "'";
                sectvalc = " and section='" + sections.ToString() + "'";
            }

            string strval = "select * from PeriodAttndScheduleNew where batch_year='" + ddlbatch.SelectedValue.ToString() + "' and semester='" + ddlsemester.SelectedValue.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' " + sectvalc + "";
            dsalterattndschd = d2.select_method_wo_parameter(strval, "Text");

            if (!cbDayorderwise)
            {
                hat.Clear();
                hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
                hat.Add("sem_ester", int.Parse(ddlsemester.SelectedValue.ToString()));
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
            }

            hat.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
            count = ds1.Tables[0].Rows.Count;

            splhr_flag = false;
            string strgetspaval = d2.GetFunction("select rights from  special_hr_rights where " + grouporusercode + "");
            if (strgetspaval.Trim() == "" || strgetspaval.Trim().ToLower() == "true")
            {
                splhr_flag = true;
            }
            int stu_count = 0;
            if (ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count > 0)
            {
                stu_count = ds4.Tables[0].Rows.Count;
            }
            //'===============================settings====================================


            //'===========================================================================
            int srno = 0;
            Boolean rowflag = false;
            int rowcnt = 0;
            Boolean rowflag1 = false;
            for (rows_count = 0; rows_count < stu_count; rows_count++)
            {
                rowflag1 = false;
                rowcnt++;
                per_abshrs_spl = 0;
                tot_per_hrs_spl = 0;
                tot_ondu_spl = 0;
                tot_ml_spl = 0;
                per_hhday_spl = 0;
                unmark_spl = 0;
                tot_conduct_hr_spl = 0;
                check = 1;
                per_workingdays1 = 0;
                cum_per_workingdays1 = 0;

                if (rows_count == 0)
                {
                    //opt-------
                    frdate = txtfromdate.Text;
                    todate = txttodate.Text;
                    string dt = frdate;
                    string[] dsplit = dt.Split(new Char[] { '/' });
                    frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                    demfcal = int.Parse(dsplit[2].ToString());
                    demfcal = demfcal * 12;
                    cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                    cal_from_date_tmp = demfcal + int.Parse(dsplit[1].ToString());

                    monthcal = cal_from_date.ToString();
                    dt = todate;
                    dsplit = dt.Split(new Char[] { '/' });
                    todate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                    demtcal = int.Parse(dsplit[2].ToString());
                    demtcal = demtcal * 12;
                    cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
                    cal_to_date_tmp = demtcal + int.Parse(dsplit[1].ToString());

                    per_from_gendate = Convert.ToDateTime(frdate);
                    per_to_gendate = Convert.ToDateTime(todate);

                    ht_sphr.Clear();
                    string hrdetno = string.Empty;
                    string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlsemester.SelectedValue.ToString() + " and date between '" + per_from_gendate.ToString() + "' and '" + per_to_gendate.ToString() + "' " + strsec + "";
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
                    //----------
                }
                hatcumonduty.Clear();
                hatcumonduty.Clear();
                presentdays();

                if (cbcumpercent.Checked == true)
                {
                    //opt-------
                    if (rows_count == 0)
                    {
                        frdate = tbfmcumdate.Text;
                        todate = tbtocumdate.Text;
                        string dt = frdate;
                        string[] dsplit = dt.Split(new Char[] { '/' });
                        frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                        demfcal = int.Parse(dsplit[2].ToString());
                        demfcal = demfcal * 12;
                        cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                        cal_from_cumdate_tmp = demfcal + int.Parse(dsplit[1].ToString());

                        monthcal = cal_from_date.ToString();
                        dt = todate;
                        dsplit = dt.Split(new Char[] { '/' });
                        todate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                        demtcal = int.Parse(dsplit[2].ToString());
                        demtcal = demtcal * 12;
                        cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
                        cal_to_cumdate_tmp = demtcal + int.Parse(dsplit[1].ToString());

                        per_from_cumdate = Convert.ToDateTime(frdate);
                        per_to_cumdate = Convert.ToDateTime(todate);

                        ht_sphr.Clear();
                        string hrdetno = string.Empty;
                        string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlsemester.SelectedValue.ToString() + " and date between '" + per_from_cumdate.ToString() + "' and '" + per_to_cumdate.ToString() + "'";
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
                    }
                    //----------

                    check = 2;
                    cumpresentdays();
                }
                if (cbcumpercent.Checked == false)
                {
                    string dum_tage_date, dum_tage_hrs;
                    per_tage_date = ((pre_present_date / per_workingdays) * 100);
                    if (per_tage_date > 100)
                    {
                        per_tage_date = 100;
                    }

                    per_con_hrs = per_workingdays1;//added 080812//my

                    per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl_fals) / (per_con_hrs + tot_conduct_hr_spl_fals)) * 100);

                    if (per_tage_hrs > 100)
                    {
                        per_tage_hrs = 100;
                    }

                    dum_tage_date = String.Format("{0:0,0.00}", float.Parse(per_tage_date.ToString()));
                    dum_tage_hrs = String.Format("{0:0,0.00}", float.Parse(per_tage_hrs.ToString()));

                    per_tage_hrs = Math.Round(per_tage_hrs, 2);
                    dum_tage_hrs = per_tage_hrs.ToString();

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
                    //


                    srno++;

                    if (txtfromrange.Text.Trim() != "" && txttorange.Text.Trim() != "")
                    {
                        Double frange = Convert.ToDouble(txtfromrange.Text.ToString());
                        Double torange = Convert.ToDouble(txttorange.Text.ToString());
                        if (Session["daywise"].ToString() != "0")
                        {
                            if (frange <= Convert.ToDouble(dum_tage_date))
                            {
                                if (torange >= Convert.ToDouble(dum_tage_date))
                                {
                                   // rowflag = true;
                                    rowflag1 = true;
                                    //drow = data.NewRow();
                                    //data.Rows.Add(drow);
                                    //data.Rows[data.Rows.Count - 1]["SNo"] = data.Rows.Count - 3;



                                }
                                //else
                                //{
                                //    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Visible = false;

                                //}
                            }
                            else
                            {
                                rowflag1 = false;
                                //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Visible = false;
                                srno = srno - 1;
                            }
                        }

                        if (Session["hourwise"].ToString() != "0")
                        {

                            if (frange <= Convert.ToDouble(dum_tage_hrs))
                            {
                                if (torange >= Convert.ToDouble(dum_tage_hrs))
                                {
                                    if (Session["daywise"].ToString() == "0")
                                    {
                                        rowflag = true;
                                        rowflag1 = true;
                                        drow = data.NewRow();
                                        data.Rows.Add(drow);
                                        data.Rows[data.Rows.Count - 1]["SNo"] = data.Rows.Count - 3;

                                    }
                                    else
                                    {
                                        rowflag = true;
                                        rowflag1 = true;
                                        drow = data.NewRow();
                                        data.Rows.Add(drow);
                                        data.Rows[data.Rows.Count - 1]["SNo"] = data.Rows.Count - 3;
                                    }
                                }
                                else
                                {
                                    rowflag1 = false;
                                    srno = srno - 1;
                                }
                            }
                            else
                            {

                                //srno = srno - 1;

                            }
                        }

                    }
                    else
                    {
                        rowflag = true;
                        rowflag1 = true;
                        drow = data.NewRow();
                        data.Rows.Add(drow);
                        data.Rows[data.Rows.Count - 1]["SNo"] = rowcnt.ToString();
                    }

                    if (rowflag1)
                    {

                        if (Session["Rollflag"].ToString() == "1")
                            data.Rows[data.Rows.Count - 1]["Roll No"] = ds4.Tables[0].Rows[rows_count]["Roll_No"].ToString();

                        else
                            data.Rows[data.Rows.Count - 1]["Roll No"] = ds4.Tables[0].Rows[rows_count]["Roll_No"].ToString();

                        if (Session["Regflag"].ToString() == "1")
                            data.Rows[data.Rows.Count - 1]["Register No"] = ds4.Tables[0].Rows[rows_count]["Reg_No"].ToString();

                        if (Session["Studflag"].ToString() == "1")
                            data.Rows[data.Rows.Count - 1]["Student Type"] = ds4.Tables[0].Rows[rows_count]["Student_Type"].ToString();
                        data.Rows[data.Rows.Count - 1]["Name Of The Student"] = ds4.Tables[0].Rows[rows_count]["Stud_Name"].ToString();





                        dicletterreport.Add(rowcnt, ds4.Tables[0].Rows[rows_count]["Reg_No"].ToString());

                        string rollno = ds4.Tables[0].Rows[rows_count]["Roll_No"].ToString();
                        if (chkonduty.Checked == false)
                        {
                            pre_present_date = pre_present_date - pre_ondu_date;
                        }
                        int col = colstartcnt;
                        data.Rows[data.Rows.Count - 1][col] = per_workingdays.ToString();
                        data.Rows[data.Rows.Count - 1][col + 1] = pre_present_date.ToString();

                        //added by sudhagar 03.03.2017
                        double roundPer = 0;
                        if (cbincround.Checked)
                        {
                            double.TryParse(Convert.ToString(Math.Round(Convert.ToDouble(dum_tage_date))), out roundPer);
                            if (hasdaywise.Contains(Convert.ToString(rollno)) != true)
                            {
                                hasdaywise.Add(Convert.ToString(rollno), roundPer);
                            }
                        }
                        else
                        {
                            double.TryParse(Convert.ToString(dum_tage_date), out roundPer);
                            if (hasdaywise.Contains(Convert.ToString(rollno)) != true)
                            {
                                hasdaywise.Add(Convert.ToString(rollno), dum_tage_date);
                            }
                        }
                        data.Rows[data.Rows.Count - 1][col + 2] = Convert.ToString(roundPer);

                        //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = dum_tage_date.ToString();
                        //if (hasdaywise.Contains(Convert.ToString(rollno)) != true)
                        //{
                        //    hasdaywise.Add(Convert.ToString(rollno), dum_tage_date);
                        //}
                        data.Rows[data.Rows.Count - 1][col + 3] = (per_con_hrs + tot_conduct_hr_spl_fals).ToString();


                        string conducted_hours_prtn = (per_con_hrs + tot_conduct_hr_spl_fals).ToString();
                        if (conducted_hours_prtn.Trim() != "" && conducted_hours_prtn.Trim() != null)
                        {
                            double conptnval1 = per_con_hrs + tot_conduct_hr_spl_fals;

                            if (conptnval1 > dup_conptnval1)
                            {

                                dup_conptnval1 = conptnval1;

                                //saran
                                // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Note = Convert.ToString(dup_conptnval1);

                            }

                        }
                        //added by srinath 22/1/2014
                        Double absenthours = (per_con_hrs + tot_conduct_hr_spl_fals) - (per_per_hrs + tot_per_hrs_spl_fals);
                        if (chkonduty.Checked == false)
                        {
                            per_per_hrs = per_per_hrs - per_tot_ondu;
                            tot_per_hrs_spl_fals = tot_per_hrs_spl_fals - tot_ondu_spl_fals;
                        }
                        data.Rows[data.Rows.Count - 1][col + 4] = (per_per_hrs + tot_per_hrs_spl_fals).ToString();
                        int odrowcnt = 0;
                        if (!colon)
                        {
                            data.Rows[data.Rows.Count - 1][col + 5] = (per_tot_ondu + tot_ondu_spl_fals).ToString();
                            odrowcnt = col + 5;
                        }
                        else
                        {
                            odrowcnt = col + 5;
                        }
                        int setcolumn = ondutycount - 1;

                        //Added by srinath 31/1/2014
                        if (setcolumn > 0)
                        {
                            for (int odrow = odrowcnt; odrow < odrowcnt + ondutycount; odrow++)
                            {
                                Double odhrval = 0;
                                string colval = data.Columns[odrow].ColumnName.ToString().Trim().ToLower();
                                if (hatonduty.Contains(colval))
                                {
                                    odhrval = Convert.ToDouble(GetCorrespondingKey(colval, hatonduty));
                                }
                                data.Rows[data.Rows.Count - 1][odrow] = odhrval.ToString();

                            }
                        }
                        data.Rows[data.Rows.Count - 1][(odrowcnt + 1) + setcolumn] = (per_tot_ml + tot_ml_spl_fals).ToString();
                        data.Rows[data.Rows.Count - 1][(odrowcnt + 2) + setcolumn] = absenthours.ToString();


                        double roundPers = 0;
                        if (cbincround.Checked)
                            double.TryParse(Convert.ToString(Math.Round(Convert.ToDouble(dum_tage_hrs))), out roundPers);
                        else
                            double.TryParse(Convert.ToString(dum_tage_hrs), out roundPers);
                        data.Rows[data.Rows.Count - 1][(odrowcnt + 3) + setcolumn] = (roundPers).ToString();




                        if (hashrwise.Contains(Convert.ToString(rollno)) != true)
                        {
                            hashrwise.Add(rollno, dum_tage_hrs);
                        }


                        data.Rows[data.Rows.Count - 1][(odrowcnt + 4) + setcolumn] = per_absent_date.ToString();
                        data.Rows[data.Rows.Count - 1][(odrowcnt + 5) + setcolumn] = pre_leave_date.ToString();
                        data.Rows[data.Rows.Count - 1][(odrowcnt + 6) + setcolumn] = pre_ondu_date.ToString();



                        pre_present_date = 0;
                        per_perhrs = 0;
                        per_absent_date = 0;
                        pre_ondu_date = 0;
                        pre_leave_date = 0;
                        per_workingdays = 0;
                        per_tot_ondu = 0;
                        per_tot_ml = 0;


                    }

                }
                else
                {

                    string dum_tage_date, dum_tage_hrs;
                    string dum_cum_tage_date, dum_cum_tage_hrs;
                    per_tage_date = ((pre_present_date / per_workingdays) * 100);
                    if (per_tage_date > 100)
                    {
                        per_tage_date = 100;
                    }


                    per_con_hrs = per_workingdays1; //added on 08.08.12//my

                    per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl_fals) / (per_con_hrs + tot_conduct_hr_spl_fals)) * 100);

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

                    // cum_con_hrs = (cum_per_workingdays1 - per_dum_unmark) + tot_conduct_hr_spl_true; //hided on 08.08.12prabha code
                    cum_con_hrs = cum_per_workingdays1; //added on 08.08.12//my

                    cum_tage_hrs = (((cum_per_perhrs + tot_per_hrs_spl_true) / (cum_con_hrs + tot_conduct_hr_spl_true)) * 100);
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



                    if (txtfromrange.Text.Trim() != "" && txttorange.Text.Trim() != "")
                    {
                        Double frange = Convert.ToDouble(txtfromrange.Text.ToString());
                        Double torange = Convert.ToDouble(txttorange.Text.ToString());
                        if (Session["daywise"].ToString() != "0")
                        {
                            if (frange <= Convert.ToDouble(dum_tage_date))
                            {
                                if (torange >= Convert.ToDouble(dum_tage_date))
                                {
                                   // rowflag = true;
                                    rowflag1 = true;
                                    //drow = data.NewRow();
                                    //data.Rows.Add(drow);
                                    //data.Rows[data.Rows.Count - 1]["SNo"] = data.Rows.Count - 3;

                                }
                                else
                                {
                                    rowflag1 = false;
                                }
                            }
                            else
                            {

                            }
                        }

                        if (Session["hourwise"].ToString() != "0")
                        {
                            if (frange <= Convert.ToDouble(dum_tage_hrs))
                            {
                                if (torange >= Convert.ToDouble(dum_tage_hrs))
                                {
                                    if (Session["daywise"].ToString() == "0")
                                    {
                                        rowflag = true;
                                        rowflag1 = true;
                                        srno++;
                                        drow = data.NewRow();
                                        data.Rows.Add(drow);
                                        data.Rows[data.Rows.Count - 1]["SNo"] = data.Rows.Count - 3;

                                    }
                                    else
                                    {
                                        rowflag = true;
                                        rowflag1 = true;
                                        srno++;
                                        drow = data.NewRow();
                                        data.Rows.Add(drow);
                                        data.Rows[data.Rows.Count - 1]["SNo"] = data.Rows.Count - 3;
                                    }
                                }
                                else
                                {
                                    rowflag1 = false;
                                }
                            }
                            else
                            {

                            }
                        }

                    }
                    else
                    {
                        rowflag = true;
                        rowflag1 = true;
                        drow = data.NewRow();
                        data.Rows.Add(drow);
                        data.Rows[data.Rows.Count - 1]["SNo"] = rowcnt.ToString();

                    }
                    if (rowflag1)
                    {

                        if (Session["Rollflag"].ToString() == "1")
                            data.Rows[data.Rows.Count - 1]["Roll No"] = ds4.Tables[0].Rows[rows_count]["Roll_No"].ToString();

                        else
                            data.Rows[data.Rows.Count - 1]["Roll No"] = ds4.Tables[0].Rows[rows_count]["Roll_No"].ToString();

                        if (Session["Regflag"].ToString() == "1")
                            data.Rows[data.Rows.Count - 1]["Register No"] = ds4.Tables[0].Rows[rows_count]["Reg_No"].ToString();

                        if (Session["Studflag"].ToString() == "1")
                            data.Rows[data.Rows.Count - 1]["Student Type"] = ds4.Tables[0].Rows[rows_count]["Student_Type"].ToString();
                        data.Rows[data.Rows.Count - 1]["Name Of The Student"] = ds4.Tables[0].Rows[rows_count]["Stud_Name"].ToString();


                        dicletterreport.Add(rowcnt, ds4.Tables[0].Rows[rows_count]["Reg_No"].ToString());

                        int col = colstartcnt;

                        //'=============================================================================================
                        //  if (Convert.ToInt32(Session["daywise"]) == 1)
                        {
                            if (chkonduty.Checked == false)
                            {
                                pre_present_date = pre_present_date - pre_ondu_date;
                                cum_present_date = cum_present_date - cum_ondu_date;
                            }
                            data.Rows[data.Rows.Count - 1][col] = (per_workingdays).ToString();
                            data.Rows[data.Rows.Count - 1][col + 1] = pre_present_date.ToString();

                            // added by sudhagar
                            double roundPer = 0;
                            if (cbincround.Checked)
                                double.TryParse(Convert.ToString(Math.Round(Convert.ToDouble(dum_tage_date))), out roundPer);
                            else
                                double.TryParse(Convert.ToString(dum_tage_date), out roundPer);
                            data.Rows[data.Rows.Count - 1][col + 2] = Convert.ToString(roundPer);

                        }
                        int colcnt = 0;
                        int setcolumn = ondutycount - 1;
                        int odrowcnt = 0;
                        {
                            //added by srinath 22/1/2014
                            Double absenthours = (per_con_hrs + tot_conduct_hr_spl_fals) - (per_per_hrs + tot_per_hrs_spl_fals);
                            if (chkonduty.Checked == false)
                            {
                                per_per_hrs = per_per_hrs - per_tot_ondu;
                                tot_per_hrs_spl_true = tot_per_hrs_spl_true - tot_ondu_spl_fals;
                            }

                            data.Rows[data.Rows.Count - 1][col + 3] = (per_con_hrs + tot_conduct_hr_spl_fals).ToString();
                            data.Rows[data.Rows.Count - 1][col + 4] = (per_per_hrs + tot_per_hrs_spl_fals).ToString();
                            if (!colon)
                            {
                                data.Rows[data.Rows.Count - 1][col + 5] = (per_tot_ondu + tot_ondu_spl_fals).ToString();
                                odrowcnt = col + 5;
                            }
                            else
                            {


                                //Added by srinath 31/1/2014
                                odrowcnt = col + 5;
                            }
                            if (setcolumn > 0)
                            {
                                for (int odrow = odrowcnt; odrow < odrowcnt + ondutycount; odrow++)
                                {
                                    Double odhrval = 0;
                                    string colval = data.Columns[odrow].ColumnName.ToString().Trim().ToLower();
                                    if (hatonduty.Contains(colval))
                                    {
                                        odhrval = Convert.ToDouble(GetCorrespondingKey(colval, hatonduty));
                                    }
                                    data.Rows[data.Rows.Count - 1][odrow] = odhrval.ToString();

                                }
                            }
                            data.Rows[data.Rows.Count - 1][(odrowcnt + 1) + setcolumn] = (per_tot_ml + tot_ml_spl_fals).ToString();
                            data.Rows[data.Rows.Count - 1][(odrowcnt + 2) + setcolumn] = absenthours.ToString();


                            //s
                            // added by sudhagar
                            double roundPersnd = 0;
                            if (cbincround.Checked)
                                double.TryParse(Convert.ToString(Math.Round(Convert.ToDouble(dum_tage_hrs))), out roundPersnd);
                            else
                                double.TryParse(Convert.ToString(dum_tage_hrs), out roundPersnd);
                            data.Rows[data.Rows.Count - 1][(odrowcnt + 3) + setcolumn] = Convert.ToString(roundPersnd);


                        }


                        //    if (Convert.ToInt32(Session["daywise"]) == 1)
                        {
                            data.Rows[data.Rows.Count - 1][(odrowcnt + 5) + setcolumn] = cum_present_date.ToString();
                            data.Rows[data.Rows.Count - 1][(odrowcnt + 4) + setcolumn] = cum_workingdays.ToString();

                            //s
                            // added by sudhagar
                            double roundPerthrd = 0;
                            if (cbincround.Checked)
                                double.TryParse(Convert.ToString(Math.Round(Convert.ToDouble(dum_cum_tage_date))), out roundPerthrd);
                            else
                                double.TryParse(Convert.ToString(dum_cum_tage_date), out roundPerthrd);
                            data.Rows[data.Rows.Count - 1][(odrowcnt + 6) + setcolumn] = Convert.ToString(roundPerthrd);

                            Double absenthours = (cum_con_hrs + tot_conduct_hr_spl_true) - (cum_per_perhrs + tot_per_hrs_spl_true);
                            if (chkonduty.Checked == false)
                            {
                                cum_per_perhrs = cum_per_perhrs - cum_tot_ondu;
                            }

                            data.Rows[data.Rows.Count - 1][(odrowcnt + 8) + setcolumn] = (cum_per_perhrs + tot_per_hrs_spl_true).ToString();
                            data.Rows[data.Rows.Count - 1][(odrowcnt + 7) + setcolumn] = (cum_con_hrs + tot_conduct_hr_spl_true).ToString();


                            if (!colon)
                            {
                                data.Rows[data.Rows.Count - 1][(odrowcnt + 9) + setcolumn] = cum_tot_ondu.ToString();
                                colcnt = 10;
                            }
                            else
                            {

                            }


                            //Added by srinath 31/1/2014
                            if (setcolumn > 0)
                            {
                                for (int odrow = ((odrowcnt + 9) + setcolumn); odrow < ((odrowcnt + 10) + setcolumn + ondutycount); odrow++)
                                {
                                    Double odhrval = 0;
                                    string colval = data.Columns[odrow].ColumnName.ToString();
                                    if (hatcumonduty.Contains(colval))
                                    {
                                        odhrval = Convert.ToDouble(GetCorrespondingKey(colval, hatcumonduty));
                                    }
                                    data.Rows[data.Rows.Count - 1][odrow] = odhrval.ToString();

                                }
                            }


                        }


                        //   if (Convert.ToInt32(Session["hourwise"]) == 1)
                        {
                            //added by srinath 22/1/2014
                            Double absenthours = (cum_con_hrs + tot_conduct_hr_spl_true) - (cum_per_perhrs + tot_per_hrs_spl_true);
                            data.Rows[data.Rows.Count - 1][(odrowcnt + 10) + (setcolumn + setcolumn)] = cum_tot_ml.ToString();
                            //s
                            // added by sudhagar
                            double roundPerfthd = 0;
                            if (cbincround.Checked)
                                double.TryParse(Convert.ToString(Math.Round(Convert.ToDouble(dum_cum_tage_hrs))), out roundPerfthd);
                            else
                                double.TryParse(Convert.ToString(dum_cum_tage_hrs), out roundPerfthd);

                            data.Rows[data.Rows.Count - 1][(odrowcnt + 12) + (setcolumn + setcolumn)] = Convert.ToString(roundPerfthd);
                            data.Rows[data.Rows.Count - 1][(odrowcnt + 11) + (setcolumn + setcolumn)] = absenthours.ToString();

                        }
                        data.Rows[data.Rows.Count - 1][(odrowcnt + 13) + (setcolumn + setcolumn)] = per_absent_date.ToString();
                        data.Rows[data.Rows.Count - 1][(odrowcnt + 15) + (setcolumn + setcolumn)] = pre_leave_date.ToString();
                        data.Rows[data.Rows.Count - 1][(odrowcnt + 16) + (setcolumn + setcolumn)] = pre_ondu_date.ToString();
                        data.Rows[data.Rows.Count - 1][(odrowcnt + 14) + (setcolumn + setcolumn)] = cum_absent_date.ToString();




                        //}
                        //'========================================================================================
                        if (cbpoints.Checked == true)
                        {
                            data.Rows[data.Rows.Count - 1][data.Columns.Count - 2] = cum_tot_point.ToString();
                            //data.Rows[data.Rows.Count - 1][data.Columns.Count - 2] = "1";
                        }
                        pre_present_date = 0;
                        per_per_hrs = 0;
                        cum_per_perhrs = 0;
                        per_absent_date = 0;
                        pre_ondu_date = 0;
                        pre_leave_date = 0;
                        per_workingdays = 0;
                        per_workingdays1 = 0;
                        cum_per_workingdays1 = 0;
                        cum_tot_ondu = 0;
                        cum_tot_ml = 0;
                        cum_present_date = 0;
                        cum_perhrs = 0;
                        cum_absent_date = 0;
                        cum_ondu_date = 0;
                        cum_leave_date = 0;
                        cum_workingdays = 0;
                        cum_tot_point = 0;
                    }
                }
            }
            if (rowflag == false)
            {
                Showgrid.Visible = false;
                btnletter.Visible = false;
                btnxl.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                lblnorec.Text = "No Record(s) Found";
                lblnorec.Visible = true;
                btnprintmaster.Visible = false;
                Printcontrol.Visible = false;
                btnPrint.Visible = false;
            }
            else
            {
                bool removeflag = false;
                int coun = 0;
                int colspan = colstartcnt;
                if (chkondutyspit.Checked)
                {
                    if (hatdicOD.Count == 0)
                    {

                        string txt = txtonduty.Text;

                        string[] spl1 = txt.Split('(');
                        string[] spl2 = spl1[1].Split(')');
                        string m = Convert.ToString(spl2[0]).Trim();
                        int.TryParse(m, out coun);
                        coun += 10;
                        int odrowcnt = colstartcnt + 5;
                        for (int i = odrowcnt; i < coun - 1; i++)
                        {
                            removeflag = true;
                            data.Columns.RemoveAt(odrowcnt);
                            colspancnt = colspancnt - 1;

                        }

                    }
                    else
                    {

                        string txt = txtonduty.Text;

                        string[] spl1 = txt.Split('(');
                        string[] spl2 = spl1[1].Split(')');
                        string m = Convert.ToString(spl2[0]).Trim();
                        int.TryParse(m, out coun);
                        coun += 10;
                        int odrowcnt = colstartcnt + 5;
                        for (int i = odrowcnt; i < coun - 1; i++)
                        {
                            string header = data.Columns[odrowcnt].ColumnName.ToLower();

                            if (hatdicOD.ContainsKey(header) == false)
                            {
                                removeflag = true;
                                data.Columns.RemoveAt(odrowcnt);
                                colspancnt = colspancnt - 1;

                            }
                            else
                            {
                                odrowcnt++;
                            }


                        }

                    }
                }

                Showgrid.DataSource = data;
                Showgrid.DataBind();
                Showgrid.Visible = true;

                btnletter.Visible = true;
                btnxl.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnprintmaster.Visible = true;
                Printcontrol.Visible = false;
                btnPrint.Visible = true;


                Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                Showgrid.Rows[0].Font.Bold = true;
                Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                Showgrid.Rows[1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                Showgrid.Rows[1].Font.Bold = true;
                Showgrid.Rows[1].HorizontalAlign = HorizontalAlign.Center;

                Showgrid.Rows[2].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                Showgrid.Rows[2].Font.Bold = true;
                Showgrid.Rows[2].HorizontalAlign = HorizontalAlign.Center;

                int rowcnt1 = 0;
                if (removeflag == true)
                    rowcnt1 = Showgrid.Rows.Count - 2;
                else
                    rowcnt1 = Showgrid.Rows.Count - 3;




                //Rowspan
                for (int rowIndex = Showgrid.Rows.Count - rowcnt1 - 1; rowIndex >= 0; rowIndex--)
                {
                    GridViewRow row = Showgrid.Rows[rowIndex];
                    GridViewRow previousRow = Showgrid.Rows[rowIndex + 1];
                    Showgrid.Rows[rowIndex].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Showgrid.Rows[rowIndex].Font.Bold = true;
                    Showgrid.Rows[rowIndex].HorizontalAlign = HorizontalAlign.Center;

                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (row.Cells[i].Text == previousRow.Cells[i].Text)
                        {

                            row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                   previousRow.Cells[i].RowSpan + 1;
                            previousRow.Cells[i].Visible = false;
                        }
                    }

                }


                //ColumnSpan
                for (int rowIndex = Showgrid.Rows.Count - rowcnt1 - 1; rowIndex >= 0; rowIndex--)
                {
                    for (int cell = Showgrid.Rows[0].Cells.Count - 1; cell > 0; cell--)
                    {
                        TableCell colum = Showgrid.Rows[rowIndex].Cells[cell];
                        TableCell previouscol = Showgrid.Rows[rowIndex].Cells[cell - 1];
                        if (colum.Visible == true && previouscol.Visible == true)
                        {
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

                    }
                }

                Double totalRows = 0;
                totalRows = Convert.ToInt32(data.Rows.Count);
                if (totalRows >= 10)
                {
                    Showgrid.PageSize = Convert.ToInt32(totalRows);
                    Showgrid.Height = 350;

                }
                else if (totalRows == 0)
                {
                    Showgrid.Height = 200;
                }
                else
                {
                    Showgrid.PageSize = Convert.ToInt32(totalRows);

                    Showgrid.Height = 200 + (10 * Convert.ToInt32(totalRows));
                }
                totalRows = Convert.ToInt32(data.Rows.Count);
                Session["totalPages"] = (int)Math.Ceiling(totalRows / Showgrid.PageSize);

            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
        }
    }

    protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    e.Row.Cells[grCol].Visible = false;
            }


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;

                if (Session["Rollflag"].ToString() == "0")
                    e.Row.Cells[1].Visible = false;


                for (int j = colstartcnt; j < data.Columns.Count; j++)
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;


            }
        }
        catch
        {


        }

    }

    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void SelectedIndexChanged(Object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
        Showgrid.Rows[rowIndex].Cells[selectedCellIndex].BackColor = Color.LightCoral;
        Showgrid.Rows[rowIndex].Cells[selectedCellIndex].BorderColor = Color.Black;
        Session["Gridcellrow"] = Convert.ToString(rowIndex);
    }

    public void presentdays()
    {
        //frdate = txtfromdate.Text;
        //todate = txttodate.Text;
        bool cbDaywisePeriodAttSchedule = false;

        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
        }
        DataSet dsSettings = new DataSet();

        dsSettings = d2.select_method_wo_parameter("select * from Master_Settings where settings='DayOrderWisePeriodAttendanceSettings' and" + grouporusercode + "", "Text");
        if (dsSettings.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToString(dsSettings.Tables[0].Rows[0]["value"]) == "0")
            {
                cbDaywisePeriodAttSchedule = false;
            }
            else if (Convert.ToString(dsSettings.Tables[0].Rows[0]["value"]) == "1")
            {
                cbDaywisePeriodAttSchedule = true;
            }
        }
        if (cbDaywisePeriodAttSchedule)
        {
            yesflag = true;

            persentmonthcal1();
        }
        else
        {

            yesflag = true;

            persentmonthcal();
        }
    }

    public void cumpresentdays()
    {
        bool cbDaywisePeriodAttSchedule = false;

        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
        }
        DataSet dsSettings = new DataSet();

        dsSettings = d2.select_method_wo_parameter("select * from Master_Settings where settings='DayOrderWisePeriodAttendanceSettings' and" + grouporusercode + "", "Text");
        if (dsSettings.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToString(dsSettings.Tables[0].Rows[0]["value"]) == "0")
            {
                cbDaywisePeriodAttSchedule = false;
            }
            else if (Convert.ToString(dsSettings.Tables[0].Rows[0]["value"]) == "1")
            {
                cbDaywisePeriodAttSchedule = true;
            }
        }
        if (cbDaywisePeriodAttSchedule)
        {
            yesflag = false;
            //frdate = tbfmcumdate.Text;
            //todate = tbtocumdate.Text;
            persentmonthcal1();
        }
        else
        {
            if (cbcumpercent.Checked == true)
            {
                yesflag = false;
                //frdate = tbfmcumdate.Text;
                //todate = tbtocumdate.Text;
                persentmonthcal();
            }
        }
    }

    public void persentmonthcal()
    {
        Hashtable hatday = new Hashtable();
        hatday.Add("sun", 0);
        hatday.Add("mon", 1);
        hatday.Add("tue", 2);
        hatday.Add("wed", 3);
        hatday.Add("thu", 4);
        hatday.Add("fri", 5);
        hatday.Add("sat", 6);

        DataSet dsondutyval = new DataSet();
        Boolean isadm = false;
        hatonduty.Clear();
        // try
        {
            per_abshrs_spl = 0;
            tot_per_hrs_spl = 0;
            per_leave = 0;
            tot_conduct_hr_spl = 0;
            tot_ondu_spl = 0;
            tot_ml_spl = 0;
            int my_un_mark = 0;
            int njdate_mng = 0, njdate_evng = 0;
            int per_holidate_mng = 0, per_holidate_evng = 0;

            mng_conducted_half_days = 0;
            evng_conducted_half_days = 0;

            notconsider_value = 0;
            conduct_hour_new = 0;

            //Opt--------
            if (yesflag == false)
            {
                cal_from_date = cal_from_cumdate_tmp;
                cal_to_date = cal_to_cumdate_tmp;
                per_from_date = per_from_cumdate;
                per_to_date = per_to_cumdate;
            }
            else
            {
                cal_from_date = cal_from_date_tmp;
                cal_to_date = cal_to_date_tmp;
                per_from_date = per_from_gendate;
                per_to_date = per_to_gendate;
            }

            //-----------
            dumm_from_date = per_from_date;

            string admdate = ds4.Tables[0].Rows[rows_count]["adm_date"].ToString();
            string[] admdatesp = admdate.Split(new Char[] { '/' });
            admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
            Admission_date = Convert.ToDateTime(admdate);

            dd = ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString();
            strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
            dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");
            hat.Clear();
            hat.Add("std_rollno", ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString());
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds2 = d2.select_method("STUD_ATTENDANCE", hat, "sp");
            mmyycount = ds2.Tables[0].Rows.Count;
            moncount = mmyycount - 1;
            if (rows_count == 0)
            {
                hat.Clear();
                hat.Add("degree_code", int.Parse(ddlbranch.SelectedValue.ToString()));
                hat.Add("sem", int.Parse(ddlsemester.SelectedItem.ToString()));
                hat.Add("from_date", frdate.ToString());
                hat.Add("to_date", todate.ToString());
                hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));


                //------------------------------------------------------------------
                int iscount = 0;
                holidaycon.Close();
                holidaycon.Open();
                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsemester.SelectedItem.ToString() + "";
                SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, holidaycon);
                SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);
                DataSet dsholiday = new DataSet();
                daholiday.Fill(dsholiday);
                if (dsholiday.Tables[0].Rows.Count > 0)
                {
                    iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                }
                hat.Add("iscount", iscount);

                ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");


                DataSet dsondutyva = new DataSet();

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
                        if (!holiday_table11.Contains((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                        }
                        // holiday_table1.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], holiday_sched_details);
                    }
                }

                if (ds3.Tables[1].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[1].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');

                        //modified
                        if (!holiday_table21.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                        {
                            holiday_table21.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
                        }


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
                        if (!holiday_table2.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table2.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                        }
                    }
                }

                if (ds3.Tables[2].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[2].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[2].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        if (!holiday_table31.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                        {
                            holiday_table31.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
                        }

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
                        if (holiday_table3.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table3.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                        }
                    }
                }
            }

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

                while (dumm_from_date <= (per_to_date))
                {
                    string getday = dumm_from_date.ToString("ddd").Trim().ToLower();

                    if (dsalterattndschd.Tables[0].Rows.Count > 0)
                    {
                        NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                        fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                        anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                        minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                        minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                        minpresday = int.Parse(ds.Tables[0].Rows[0]["MIN PREE PER DAY"].ToString());

                        if (hatday.Contains(getday))
                        {
                            int dayorder = Convert.ToInt32(hatday[getday].ToString());
                            dsalterattndschd.Tables[0].DefaultView.RowFilter = "dayorder='" + dayorder + "'";
                            DataView dvattschchange = dsalterattndschd.Tables[0].DefaultView;
                            if (dvattschchange.Count > 0)
                            {
                                NoHrs = int.Parse(dvattschchange[0]["No_of_hrs_per_day"].ToString());
                                fnhrs = int.Parse(dvattschchange[0]["no_of_hrs_I_half_day"].ToString());
                                anhrs = int.Parse(dvattschchange[0]["no_of_hrs_II_half_day"].ToString());
                                minpresI = int.Parse(dvattschchange[0]["min_pres_I_half_day"].ToString());
                                minpresII = int.Parse(dvattschchange[0]["min_pres_II_half_day"].ToString());
                                minpresday = minpresI + minpresII;
                            }
                        }
                    }

                    nohrsprsentperday = 0;
                    noofdaypresen = 0;
                    isadm = false;
                    if (dumm_from_date >= Admission_date)
                    {
                        isadm = true;
                        int temp_unmark = 0;
                        if (splhr_flag == true)
                        {
                            if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
                            {
                                getspecial_hr();
                            }
                        }

                        for (int i = 1; i <= mmyycount; i++)
                        {
                            ds2.Tables[0].DefaultView.RowFilter = "month_year='" + cal_from_date + "' and roll_no='" + ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString() + "'";
                            DataView dvattvalue = ds2.Tables[0].DefaultView;
                            if (dvattvalue.Count > 0)//Added by srinath 13/10/2014
                            {
                                // if (cal_from_date == int.Parse(ds2.Tables[0].Rows[next]["month_year"].ToString()))
                                if (cal_from_date == int.Parse(dvattvalue[0]["month_year"].ToString()))
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
                                        if (dif_date == 1)
                                        {
                                            leave_pointer = holi_leav;
                                            absent_pointer = holi_absent;
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
                                        if (ds3.Tables[2].Rows.Count != 0)
                                        {
                                            ts = DateTime.Parse(ds3.Tables[2].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                                            diff_date = Convert.ToString(ts.Days);
                                            dif_date = double.Parse(diff_date.ToString());
                                            if (dif_date == 1)
                                            {
                                                leave_pointer = holi_leav;
                                                absent_pointer = holi_absent;
                                            }

                                        }
                                        if (dif_date1 == -1)
                                        {
                                            leave_pointer = holi_leav;
                                            absent_pointer = holi_absent;
                                        }
                                        dif_date1 = 0;

                                        UnmarkHours = string.Empty;
                                        CurrentDate = dumm_from_date.ToString("dd/MM/yyyy");

                                        if (split_holiday_status_1 == "1")
                                        {

                                            for (i = 1; i <= fnhrs; i++)
                                            {
                                                date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                                //value = ds2.Tables[0].Rows[next][date].ToString();
                                                value = dvattvalue[0][date].ToString();
                                                //Added by srinath 31/1/2014=========Start
                                                if (value == "0" || value == "" || value == null)//Rajkumar NEC
                                                {
                                                    //string date1=dumm_from_date.ToString("DD/MM/YYYY");
                                                    UnmarkHours = UnmarkHours + "Date: " + CurrentDate + " " + "Hour: " + i.ToString() + ",";
                                                }
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
                                                        per_abshrs += 1;
                                                    }
                                                    else if (ObtValue == 2)
                                                    {
                                                        notconsider_value += 1;
                                                        njhr += 1;
                                                    }
                                                    else if (ObtValue == 0)
                                                    {
                                                        per_perhrs += 1;
                                                        tot_per_hrs += 1;
                                                    }
                                                    if (value == "3")
                                                    {
                                                        per_ondu += 1;
                                                        tot_ondu += 1;

                                                        if (chkondutyspit.Checked == true)
                                                        {
                                                            if (yesflag == true)
                                                            {
                                                                if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                {
                                                                    if (cal_from_date != int.Parse(dsondutyval.Tables[0].Rows[0]["month_year"].ToString()))
                                                                    {
                                                                        strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
                                                                        dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");
                                                                        if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString().Trim().ToLower();
                                                                            if (hatonduty.Contains(strondutyvalue))
                                                                            {
                                                                                int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatonduty));
                                                                                odval++;
                                                                                hatonduty[strondutyvalue] = odval;
                                                                            }
                                                                            else
                                                                            {
                                                                                hatonduty.Add(strondutyvalue, 1);
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString().Trim().ToLower();
                                                                            if (hatonduty.Contains(strondutyvalue))
                                                                            {
                                                                                int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatonduty));
                                                                                odval++;
                                                                                hatonduty[strondutyvalue] = odval;
                                                                            }
                                                                            else
                                                                            {
                                                                                hatonduty.Add(strondutyvalue, 1);
                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
                                                                    dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");
                                                                    if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString().Trim().ToLower();
                                                                        if (hatonduty.Contains(strondutyvalue))
                                                                        {
                                                                            int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatonduty));
                                                                            odval++;
                                                                            hatonduty[strondutyvalue] = odval;
                                                                        }
                                                                        else
                                                                        {
                                                                            hatonduty.Add(strondutyvalue, 1);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                {
                                                                    if (cal_from_date != int.Parse(dsondutyval.Tables[0].Rows[0]["month_year"].ToString()))
                                                                    {
                                                                        strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
                                                                        dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");
                                                                        if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString();
                                                                            if (hatcumonduty.Contains(strondutyvalue))
                                                                            {
                                                                                int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatcumonduty));
                                                                                odval++;
                                                                                hatcumonduty[strondutyvalue] = odval;
                                                                            }
                                                                            else
                                                                            {
                                                                                hatcumonduty.Add(strondutyvalue, 1);
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString();
                                                                            if (hatcumonduty.Contains(strondutyvalue))
                                                                            {
                                                                                int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatcumonduty));
                                                                                odval++;
                                                                                hatcumonduty[strondutyvalue] = odval;
                                                                            }
                                                                            else
                                                                            {
                                                                                hatcumonduty.Add(strondutyvalue, 1);
                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
                                                                    dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");
                                                                    if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString();
                                                                        if (hatcumonduty.Contains(strondutyvalue))
                                                                        {
                                                                            int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatcumonduty));
                                                                            odval++;
                                                                            hatcumonduty[strondutyvalue] = odval;
                                                                        }
                                                                        else
                                                                        {
                                                                            hatcumonduty.Add(strondutyvalue, 1);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (!string.IsNullOrEmpty(strondutyvalue))//raj new
                                                        {
                                                            if (!hatdicOD.ContainsKey(strondutyvalue))
                                                            {
                                                                hatdicOD.Add(strondutyvalue, strondutyvalue);
                                                            }
                                                        }
                                                        //=============End ========================
                                                    }
                                                    else if (value == "10")
                                                    {
                                                        per_leave += 1;
                                                    }
                                                    else if (value == "4")
                                                    {
                                                        tot_ml += 1;
                                                    }

                                                }
                                                else if (value == "7")
                                                {
                                                    per_hhday += 1;

                                                }
                                                else
                                                {
                                                    unmark += 1;
                                                    temp_unmark++;

                                                    my_un_mark++;//added 080812
                                                }
                                            }
                                            nohrsprsentperday = per_perhrs + njhr;
                                            //  if (per_perhrs >= minpresI)
                                            if (per_perhrs + njhr >= minpresI)
                                            {
                                                Present += 0.5;
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
                                                njdate_mng += 1;
                                            }
                                            if (per_ondu >= 1)
                                            {
                                                Onduty += 0.5;
                                            }

                                            if (temp_unmark == fnhrs)
                                            {
                                                per_holidate_mng += 1;
                                                per_holidate += 0.5;
                                                unmark = 0;
                                            }
                                            else
                                            {
                                                dum_unmark = temp_unmark;
                                            }
                                            if (fnhrs - temp_unmark >= minpresI)
                                            {
                                                workingdays += 0.5;
                                            }
                                            mng_conducted_half_days += 1;


                                        }

                                        per_perhrs = 0;
                                        per_ondu = 0;
                                        per_leave = 0;
                                        per_abshrs = 0;
                                        //   unmark = 0;
                                        temp_unmark = 0;
                                        njhr = 0;

                                        int k = fnhrs + 1;

                                        if (split_holiday_status_2 == "1")
                                        {
                                            for (i = k; i <= NoHrs; i++)
                                            {
                                                date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                                //  value = ds2.Tables[0].Rows[next][date].ToString();
                                                value = dvattvalue[0][date].ToString();
                                                if (value == "0" || value == "" || value == null)//Rajkumar NEC
                                                {
                                                    //string date1=dumm_from_date.ToString("DD/MM/YYYY");
                                                    UnmarkHours = UnmarkHours + "Date: " + CurrentDate + " " + "Hour: " + i.ToString() + ",";
                                                }
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
                                                        per_abshrs += 1;
                                                    }
                                                    else if (ObtValue == 2)
                                                    {
                                                        notconsider_value += 1;
                                                        njhr += 1;
                                                    }
                                                    else if (ObtValue == 0)
                                                    {
                                                        per_perhrs += 1;
                                                        tot_per_hrs += 1;
                                                    }
                                                    if (value == "3")
                                                    {
                                                        per_ondu += 1;
                                                        tot_ondu += 1;

                                                        //================Added by srinath 31/1/2014 =================
                                                        if (chkondutyspit.Checked == true)
                                                        {
                                                            if (yesflag == true)
                                                            {
                                                                if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                {
                                                                    if (cal_from_date != int.Parse(dsondutyval.Tables[0].Rows[0]["month_year"].ToString()))
                                                                    {
                                                                        strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
                                                                        dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");
                                                                        if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString().Trim().ToLower();
                                                                            if (hatonduty.Contains(strondutyvalue))
                                                                            {
                                                                                int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatonduty));
                                                                                odval++;
                                                                                hatonduty[strondutyvalue] = odval;
                                                                            }
                                                                            else
                                                                            {
                                                                                hatonduty.Add(strondutyvalue, 1);
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString().Trim().ToLower();
                                                                            if (hatonduty.Contains(strondutyvalue))
                                                                            {
                                                                                int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatonduty));
                                                                                odval++;
                                                                                hatonduty[strondutyvalue] = odval;
                                                                            }
                                                                            else
                                                                            {
                                                                                hatonduty.Add(strondutyvalue, 1);
                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
                                                                    dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");
                                                                    if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString().Trim().ToLower();
                                                                        if (hatonduty.Contains(strondutyvalue))
                                                                        {
                                                                            int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatonduty));
                                                                            odval++;
                                                                            hatonduty[strondutyvalue] = odval;
                                                                        }
                                                                        else
                                                                        {
                                                                            hatonduty.Add(strondutyvalue, 1);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                {
                                                                    if (cal_from_date != int.Parse(dsondutyval.Tables[0].Rows[0]["month_year"].ToString()))
                                                                    {
                                                                        strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
                                                                        dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");
                                                                        if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString();
                                                                            if (hatcumonduty.Contains(strondutyvalue))
                                                                            {
                                                                                int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatcumonduty));
                                                                                odval++;
                                                                                hatcumonduty[strondutyvalue] = odval;
                                                                            }
                                                                            else
                                                                            {
                                                                                hatcumonduty.Add(strondutyvalue, 1);
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString();
                                                                            if (hatcumonduty.Contains(strondutyvalue))
                                                                            {
                                                                                int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatcumonduty));
                                                                                odval++;
                                                                                hatcumonduty[strondutyvalue] = odval;
                                                                            }
                                                                            else
                                                                            {
                                                                                hatcumonduty.Add(strondutyvalue, 1);
                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
                                                                    dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");
                                                                    if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString();
                                                                        if (hatcumonduty.Contains(strondutyvalue))
                                                                        {
                                                                            int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatcumonduty));
                                                                            odval++;
                                                                            hatcumonduty[strondutyvalue] = odval;
                                                                        }
                                                                        else
                                                                        {
                                                                            hatcumonduty.Add(strondutyvalue, 1);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        //=======================End =============================
                                                        if (!string.IsNullOrEmpty(strondutyvalue))//raj new
                                                        {
                                                            if (!hatdicOD.ContainsKey(strondutyvalue))
                                                            {
                                                                hatdicOD.Add(strondutyvalue, strondutyvalue);
                                                            }
                                                        }
                                                    }
                                                    else if (value == "10")
                                                    {
                                                        per_leave += 1;
                                                    }
                                                    if (value == "4")
                                                    {
                                                        tot_ml += 1;
                                                    }
                                                }
                                                else if (value == "7")
                                                {
                                                    per_hhday += 1;
                                                }
                                                else
                                                {
                                                    unmark += 1;
                                                    temp_unmark++;

                                                    my_un_mark++; //added 080812
                                                }
                                            }
                                            //   if (per_perhrs >= minpresII)
                                            nohrsprsentperday = nohrsprsentperday + per_perhrs + njhr;
                                            if (per_perhrs + njhr >= minpresII)
                                            {
                                                Present += 0.5;
                                                noofdaypresen = noofdaypresen + 0.5;
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
                                            if (njhr >= minpresII)
                                            {
                                                njdate_evng += 1;
                                                njdate += 0.5;
                                            }
                                            if (per_ondu >= 1)
                                            {
                                                Onduty += 0.5;
                                            }
                                            if (Session["attdaywisecla"] != null && Session["attdaywisecla"].ToString() == "1")
                                            {
                                                if (nohrsprsentperday < minpresday)
                                                {
                                                    Present = Present - noofdaypresen;
                                                    Absent = Absent + noofdaypresen;
                                                }
                                            }
                                            nohrsprsentperday = 0;
                                            noofdaypresen = 0;
                                            if (temp_unmark == NoHrs - fnhrs)
                                            {
                                                per_holidate_evng += 1;
                                                per_holidate += 0.5;

                                                unmark = 0;
                                            }
                                            else
                                            {
                                                dum_unmark += unmark;
                                            }


                                            if ((NoHrs - fnhrs) - temp_unmark >= minpresII)
                                            {
                                                workingdays += 0.5;
                                            }
                                            evng_conducted_half_days += 1;


                                        }
                                        if (!string.IsNullOrEmpty(UnmarkHours))
                                        {
                                            usercode = Session["usercode"].ToString().Trim();
                                            string alertRights = dirAcc.selectScalarString("select value from Master_Settings where settings='AlertMessageForAttendance' and usercode='" + usercode + "'");
                                            string Noresult = UnmarkHours;
                                            if (alertRights == "1")
                                            {
                                                //lblAlertMsg.Visible = true;
                                                //lblAlertMsg.Text = Noresult + " " + " Attendance not Found";
                                                //divPopAlert.Visible = true;
                                            }
                                            //return;  //modified on prabha on 15/12/2017
                                        }
                                        per_perhrs = 0;
                                        per_ondu = 0;
                                        per_leave = 0;
                                        per_abshrs = 0;
                                        unmark = 0; //hided
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
                                    dumm_from_date = dumm_from_date.AddDays(1);
                                    if (dumm_from_date.Day == 1)
                                    {
                                        cal_from_date++;
                                        if (moncount > next)
                                        {
                                            next++; //  next++;
                                        }
                                    }
                                    //if (moncount > next)
                                    //{
                                    //    i--;
                                    //}
                                }
                                i = mmyycount + 1;
                            }
                            else//Added by srinath 13/10/2014
                            {
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
                    nohrsprsentperday = 0;
                    noofdaypresen = 0;
                }
                int diff_Date = per_from_date.Day - dumm_from_date.Day;
            }

            if (check == 1)
            {

                per_tot_ondu = tot_ondu;
                per_tot_ml = tot_ml;
                per_njdate = njdate;
                pre_present_date = Present - njdate;
                per_per_hrs = tot_per_hrs;
                per_absent_date = Absent;
                pre_ondu_date = Onduty;
                pre_leave_date = Leave;
                // per_workingdays = workingdays - per_holidate - per_njdate;
                per_workingdays = workingdays - per_njdate;
                per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - ((Convert.ToInt16(per_holidate_mng) * fnhrs) + (Convert.ToInt16(per_holidate_evng) * (NoHrs - fnhrs))) - notconsider_value - dum_unmark; //dum_unmark hided on 08.08.12 // ((Convert.ToInt16(njdate_mng) * fnhrs) + (Convert.ToInt16(njdate_evng) * (NoHrs - fnhrs)));

                per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value; //added on 08.08.12,notconsider value added for hr suspension,09.08.12 mythili

                //  per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) -  notconsider_value - dum_unmark;
                per_dum_unmark = dum_unmark; //hided on 08.08.12
            }
            if (check == 2)
            {
                cum_tot_ondu = tot_ondu;
                cum_tot_ml = tot_ml;
                cum_njdate = njdate;
                cum_present_date = Present - njdate;
                cum_per_perhrs = tot_per_hrs;
                cum_absent_date = Absent;
                cum_ondu_date = Onduty;
                cum_leave_date = Leave;
                //cum_workingdays = workingdays - per_holidate - cum_njdate;
                cum_workingdays = workingdays - cum_njdate;
                cum_per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - ((Convert.ToInt16(per_holidate_mng) * fnhrs) + (Convert.ToInt16(per_holidate_evng) * (NoHrs - fnhrs))) - notconsider_value - dum_unmark; //added dum_unmrk on 08.08.12
                //     cum_per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - notconsider_value;

                cum_per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value; //added on 08.08.12,notconsider value added for hr suspension,09.08.12 mythili

                cum_dum_unmark = dum_unmark; //hided on 08.08.12
                cum_tot_point = absent_point + leave_point;
            }
            if (hatdicOD.Count > 0)
            {

            }
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
            tot_ml = 0;
        }
        //   catch
        {
        }
    }

    public void persentmonthcal1()
    {
        DataSet dsondutyval = new DataSet();
        Boolean isadm = false;
        hatonduty.Clear();
        // try
        {
            per_abshrs_spl = 0;
            tot_per_hrs_spl = 0;
            per_leave = 0;
            tot_conduct_hr_spl = 0;
            tot_ondu_spl = 0;
            tot_ml_spl = 0;
            int my_un_mark = 0;
            int njdate_mng = 0, njdate_evng = 0;
            int per_holidate_mng = 0, per_holidate_evng = 0;

            mng_conducted_half_days = 0;
            evng_conducted_half_days = 0;




            notconsider_value = 0;
            conduct_hour_new = 0;

            //Opt--------
            if (yesflag == false)
            {
                cal_from_date = cal_from_cumdate_tmp;
                cal_to_date = cal_to_cumdate_tmp;
                per_from_date = per_from_cumdate;
                per_to_date = per_to_cumdate;
            }
            else
            {
                cal_from_date = cal_from_date_tmp;
                cal_to_date = cal_to_date_tmp;
                per_from_date = per_from_gendate;
                per_to_date = per_to_gendate;
            }

            //-----------
            dumm_from_date = per_from_date;

            string admdate = ds4.Tables[0].Rows[rows_count]["adm_date"].ToString();
            string[] admdatesp = admdate.Split(new Char[] { '/' });
            admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
            Admission_date = Convert.ToDateTime(admdate);

            dd = ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString();

            strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
            dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");

            DataSet dsStud = new DataSet();
            dsStud = d2.select_method_wo_parameter("select * from Registration where Roll_No='" + dd + "'", "Text");
            int tot_morn_con_hrs = 0;
            int tot_evng_con_hrs = 0;
            int morn_conduct_hr = 0;
            int evng_conduct_hr = 0;
            string semester = Convert.ToString(ddlsemester.SelectedItem.Text);
            string section = string.Empty;
            string batchyear = string.Empty;
            string degreeCode = ddlbranch.SelectedValue.ToString();
            if (dsStud.Tables[0].Rows.Count > 0)
            {
                semester = Convert.ToString(dsStud.Tables[0].Rows[0]["Current_Semester"]);
                batchyear = Convert.ToString(dsStud.Tables[0].Rows[0]["Batch_Year"]);
                section = Convert.ToString(dsStud.Tables[0].Rows[0]["Sections"]);
                degreeCode = Convert.ToString(dsStud.Tables[0].Rows[0]["degree_code"]);
            }
            else
            {
                if (ddlsection.Enabled == true)
                {
                    if (ddlsection.Items.Count > 0)
                    {
                        section = Convert.ToString(ddlsection.SelectedItem.Text);
                    }
                    else
                    {
                        section = string.Empty;
                    }
                    if (section.ToLower() == "all")
                    {
                        section = string.Empty;
                    }
                }
                else
                {
                    section = string.Empty;
                }

                if (ddlbatch.Items.Count > 0)
                {
                    batchyear = Convert.ToString(ddlbatch.SelectedItem.Text);
                }
            }
            DataSet dsSchdule = new DataSet();
            DataView dvSchedule = new DataView();

            dsSchdule = d2.select_method_wo_parameter("select * from PeriodAttndScheduleNew where degree_code='" + degreeCode + "' and batch_year='" + batchyear + "' and semester='" + semester + "' and section='" + section + "'", "Text");

            hat.Clear();
            hat.Add("std_rollno", ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString());
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds2 = d2.select_method("STUD_ATTENDANCE", hat, "sp");
            mmyycount = ds2.Tables[0].Rows.Count;
            moncount = mmyycount - 1;
            if (rows_count == 0)
            {
                hat.Clear();
                hat.Add("degree_code", int.Parse(ddlbranch.SelectedValue.ToString()));
                hat.Add("sem", int.Parse(ddlsemester.SelectedItem.ToString()));
                hat.Add("from_date", frdate.ToString());
                hat.Add("to_date", todate.ToString());
                hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));


                //------------------------------------------------------------------
                int iscount = 0;
                holidaycon.Close();
                holidaycon.Open();
                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsemester.SelectedItem.ToString() + "";
                SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, holidaycon);
                SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);
                DataSet dsholiday = new DataSet();
                daholiday.Fill(dsholiday);
                if (dsholiday.Tables[0].Rows.Count > 0)
                {
                    iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                }
                hat.Add("iscount", iscount);

                ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");


                DataSet dsondutyva = new DataSet();

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
                        if (!holiday_table11.Contains((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                        }
                        // holiday_table1.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], holiday_sched_details);
                    }
                }

                if (ds3.Tables[1].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[1].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        //modified
                        if (!holiday_table21.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                        {
                            holiday_table21.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
                        }


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
                        if (!holiday_table2.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table2.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                        }
                    }
                }

                if (ds3.Tables[2].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[2].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[2].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');

                        if (!holiday_table31.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                        {
                            holiday_table31.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
                        }

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
                        if (holiday_table3.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                        {
                            holiday_table3.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
                        }
                    }
                }



            }

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

                while (dumm_from_date <= (per_to_date))
                {
                    nohrsprsentperday = 0;
                    noofdaypresen = 0;

                    morn_conduct_hr = 0;
                    evng_conduct_hr = 0;
                    int Dayorder = getDayOrder(dumm_from_date);
                    dsSchdule.Tables[0].DefaultView.RowFilter = "DayOrder='" + Dayorder + "'";
                    dvSchedule = dsSchdule.Tables[0].DefaultView;

                    if (dvSchedule.Count > 0)
                    {
                        NoHrs = int.Parse(dvSchedule[0]["No_of_hrs_per_day"].ToString());
                        fnhrs = int.Parse(dvSchedule[0]["no_of_hrs_I_half_day"].ToString());
                        anhrs = int.Parse(dvSchedule[0]["no_of_hrs_II_half_day"].ToString());
                        minpresI = int.Parse(dvSchedule[0]["min_pres_I_half_day"].ToString());
                        minpresII = int.Parse(dvSchedule[0]["min_pres_II_half_day"].ToString());
                    }

                    isadm = false;
                    if (dumm_from_date >= Admission_date)
                    {
                        isadm = true;
                        int temp_unmark = 0;
                        if (splhr_flag == true)
                        {
                            if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
                            {
                                getspecial_hr();
                            }
                        }

                        for (int i = 1; i <= mmyycount; i++)
                        {
                            ds2.Tables[0].DefaultView.RowFilter = "month_year='" + cal_from_date + "' and roll_no='" + ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString() + "'";
                            DataView dvattvalue = ds2.Tables[0].DefaultView;
                            if (dvattvalue.Count > 0)//Added by srinath 13/10/2014
                            {
                                // if (cal_from_date == int.Parse(ds2.Tables[0].Rows[next]["month_year"].ToString()))
                                if (cal_from_date == int.Parse(dvattvalue[0]["month_year"].ToString()))
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
                                        if (dif_date == 1)
                                        {
                                            leave_pointer = holi_leav;
                                            absent_pointer = holi_absent;
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

                                        if (ds3.Tables[2].Rows.Count != 0)
                                        {
                                            ts = DateTime.Parse(ds3.Tables[2].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                                            diff_date = Convert.ToString(ts.Days);
                                            dif_date = double.Parse(diff_date.ToString());
                                            if (dif_date == 1)
                                            {
                                                leave_pointer = holi_leav;
                                                absent_pointer = holi_absent;
                                            }

                                        }
                                        if (dif_date1 == -1)
                                        {
                                            leave_pointer = holi_leav;
                                            absent_pointer = holi_absent;
                                        }
                                        dif_date1 = 0;
                                        if (split_holiday_status_1 == "1")
                                        {

                                            for (i = 1; i <= fnhrs; i++)
                                            {
                                                date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                                //value = ds2.Tables[0].Rows[next][date].ToString();
                                                value = dvattvalue[0][date].ToString();
                                                //Added by srinath 31/1/2014=========Start
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
                                                        per_abshrs += 1;
                                                    }
                                                    else if (ObtValue == 2)
                                                    {
                                                        notconsider_value += 1;
                                                        njhr += 1;
                                                    }
                                                    else if (ObtValue == 0)
                                                    {
                                                        per_perhrs += 1;
                                                        tot_per_hrs += 1;
                                                    }
                                                    if (value == "3")
                                                    {
                                                        per_ondu += 1;
                                                        tot_ondu += 1;

                                                        if (chkondutyspit.Checked == true)
                                                        {
                                                            if (yesflag == true)
                                                            {
                                                                if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                {
                                                                    if (cal_from_date != int.Parse(dsondutyval.Tables[0].Rows[0]["month_year"].ToString()))
                                                                    {
                                                                        strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
                                                                        dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");
                                                                        if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString().Trim().ToLower();
                                                                            if (hatonduty.Contains(strondutyvalue))
                                                                            {
                                                                                int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatonduty));
                                                                                odval++;
                                                                                hatonduty[strondutyvalue] = odval;
                                                                            }
                                                                            else
                                                                            {
                                                                                hatonduty.Add(strondutyvalue, 1);
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString().Trim().ToLower();
                                                                            if (hatonduty.Contains(strondutyvalue))
                                                                            {
                                                                                int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatonduty));
                                                                                odval++;
                                                                                hatonduty[strondutyvalue] = odval;
                                                                            }
                                                                            else
                                                                            {
                                                                                hatonduty.Add(strondutyvalue, 1);
                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
                                                                    dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");
                                                                    if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString().Trim().ToLower();
                                                                        if (hatonduty.Contains(strondutyvalue))
                                                                        {
                                                                            int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatonduty));
                                                                            odval++;
                                                                            hatonduty[strondutyvalue] = odval;
                                                                        }
                                                                        else
                                                                        {
                                                                            hatonduty.Add(strondutyvalue, 1);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                {
                                                                    if (cal_from_date != int.Parse(dsondutyval.Tables[0].Rows[0]["month_year"].ToString()))
                                                                    {
                                                                        strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
                                                                        dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");
                                                                        if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString();
                                                                            if (hatcumonduty.Contains(strondutyvalue))
                                                                            {
                                                                                int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatcumonduty));
                                                                                odval++;
                                                                                hatcumonduty[strondutyvalue] = odval;
                                                                            }
                                                                            else
                                                                            {
                                                                                hatcumonduty.Add(strondutyvalue, 1);
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString();
                                                                            if (hatcumonduty.Contains(strondutyvalue))
                                                                            {
                                                                                int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatcumonduty));
                                                                                odval++;
                                                                                hatcumonduty[strondutyvalue] = odval;
                                                                            }
                                                                            else
                                                                            {
                                                                                hatcumonduty.Add(strondutyvalue, 1);
                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
                                                                    dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");
                                                                    if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString();
                                                                        if (hatcumonduty.Contains(strondutyvalue))
                                                                        {
                                                                            int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatcumonduty));
                                                                            odval++;
                                                                            hatcumonduty[strondutyvalue] = odval;
                                                                        }
                                                                        else
                                                                        {
                                                                            hatcumonduty.Add(strondutyvalue, 1);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (!string.IsNullOrEmpty(strondutyvalue))//raj new
                                                        {
                                                            if (!hatdicOD.ContainsKey(strondutyvalue))
                                                            {
                                                                hatdicOD.Add(strondutyvalue, strondutyvalue);
                                                            }
                                                        }
                                                        //=============End ========================
                                                    }
                                                    else if (value == "10")
                                                    {
                                                        per_leave += 1;
                                                    }
                                                    else if (value == "4")
                                                    {
                                                        tot_ml += 1;
                                                    }

                                                }
                                                else if (value == "7")
                                                {
                                                    per_hhday += 1;

                                                }
                                                else
                                                {
                                                    unmark += 1;
                                                    temp_unmark++;

                                                    my_un_mark++;//added 080812
                                                }
                                            }
                                            nohrsprsentperday = per_perhrs + njhr;
                                            //  if (per_perhrs >= minpresI)
                                            if (per_perhrs + njhr >= minpresI)
                                            {
                                                Present += 0.5;
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
                                                njdate_mng += 1;
                                            }
                                            if (per_ondu >= 1)
                                            {
                                                Onduty += 0.5;
                                            }

                                            if (temp_unmark == fnhrs)
                                            {
                                                per_holidate_mng += 1;
                                                per_holidate += 0.5;
                                                unmark = 0;
                                            }
                                            else
                                            {
                                                dum_unmark = temp_unmark;
                                            }
                                            if (fnhrs - temp_unmark >= minpresI)
                                            {
                                                workingdays += 0.5;
                                            }
                                            mng_conducted_half_days += 1;
                                            morn_conduct_hr += 1;
                                            tot_morn_con_hrs += morn_conduct_hr * fnhrs;

                                        }
                                        per_perhrs = 0;
                                        per_ondu = 0;
                                        per_leave = 0;
                                        per_abshrs = 0;
                                        //   unmark = 0;
                                        temp_unmark = 0;
                                        njhr = 0;

                                        int k = fnhrs + 1;

                                        if (split_holiday_status_2 == "1")
                                        {
                                            for (i = k; i <= NoHrs; i++)
                                            {
                                                date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
                                                //  value = ds2.Tables[0].Rows[next][date].ToString();
                                                value = dvattvalue[0][date].ToString();
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
                                                        per_abshrs += 1;
                                                    }
                                                    else if (ObtValue == 2)
                                                    {
                                                        notconsider_value += 1;
                                                        njhr += 1;
                                                    }
                                                    else if (ObtValue == 0)
                                                    {
                                                        per_perhrs += 1;
                                                        tot_per_hrs += 1;
                                                    }
                                                    if (value == "3")
                                                    {
                                                        per_ondu += 1;
                                                        tot_ondu += 1;

                                                        //================Added by srinath 31/1/2014 =================
                                                        if (chkondutyspit.Checked == true)
                                                        {
                                                            if (yesflag == true)
                                                            {
                                                                if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                {
                                                                    if (cal_from_date != int.Parse(dsondutyval.Tables[0].Rows[0]["month_year"].ToString()))
                                                                    {
                                                                        strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
                                                                        dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");
                                                                        if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString().Trim().ToLower();
                                                                            if (hatonduty.Contains(strondutyvalue))
                                                                            {
                                                                                int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatonduty));
                                                                                odval++;
                                                                                hatonduty[strondutyvalue] = odval;
                                                                            }
                                                                            else
                                                                            {
                                                                                hatonduty.Add(strondutyvalue, 1);
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString().Trim().ToLower();
                                                                            if (hatonduty.Contains(strondutyvalue))
                                                                            {
                                                                                int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatonduty));
                                                                                odval++;
                                                                                hatonduty[strondutyvalue] = odval;
                                                                            }
                                                                            else
                                                                            {
                                                                                hatonduty.Add(strondutyvalue, 1);
                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
                                                                    dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");
                                                                    if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString().Trim().ToLower();
                                                                        if (hatonduty.Contains(strondutyvalue))
                                                                        {
                                                                            int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatonduty));
                                                                            odval++;
                                                                            hatonduty[strondutyvalue] = odval;
                                                                        }
                                                                        else
                                                                        {
                                                                            hatonduty.Add(strondutyvalue, 1);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                {
                                                                    if (cal_from_date != int.Parse(dsondutyval.Tables[0].Rows[0]["month_year"].ToString()))
                                                                    {
                                                                        strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
                                                                        dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");
                                                                        if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString();
                                                                            if (hatcumonduty.Contains(strondutyvalue))
                                                                            {
                                                                                int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatcumonduty));
                                                                                odval++;
                                                                                hatcumonduty[strondutyvalue] = odval;
                                                                            }
                                                                            else
                                                                            {
                                                                                hatcumonduty.Add(strondutyvalue, 1);
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString();
                                                                            if (hatcumonduty.Contains(strondutyvalue))
                                                                            {
                                                                                int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatcumonduty));
                                                                                odval++;
                                                                                hatcumonduty[strondutyvalue] = odval;
                                                                            }
                                                                            else
                                                                            {
                                                                                hatcumonduty.Add(strondutyvalue, 1);
                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
                                                                    dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");
                                                                    if (dsondutyval.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        strondutyvalue = dsondutyval.Tables[0].Rows[0][date].ToString();
                                                                        if (hatcumonduty.Contains(strondutyvalue))
                                                                        {
                                                                            int odval = Convert.ToInt32(GetCorrespondingKey(strondutyvalue, hatcumonduty));
                                                                            odval++;
                                                                            hatcumonduty[strondutyvalue] = odval;
                                                                        }
                                                                        else
                                                                        {
                                                                            hatcumonduty.Add(strondutyvalue, 1);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (!string.IsNullOrEmpty(strondutyvalue))//raj new
                                                        {
                                                            if (!hatdicOD.ContainsKey(strondutyvalue))
                                                            {
                                                                hatdicOD.Add(strondutyvalue, strondutyvalue);
                                                            }
                                                        }
                                                        //=======================End =============================
                                                    }
                                                    else if (value == "10")
                                                    {
                                                        per_leave += 1;
                                                    }
                                                    if (value == "4")
                                                    {
                                                        tot_ml += 1;
                                                    }
                                                }
                                                else if (value == "7")
                                                {
                                                    per_hhday += 1;
                                                }
                                                else
                                                {
                                                    unmark += 1;
                                                    temp_unmark++;

                                                    my_un_mark++; //added 080812
                                                }
                                            }
                                            //   if (per_perhrs >= minpresII)
                                            nohrsprsentperday = nohrsprsentperday + per_perhrs + njhr;
                                            if (per_perhrs + njhr >= minpresII)
                                            {
                                                Present += 0.5;
                                                noofdaypresen = noofdaypresen + 0.5;
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
                                            if (njhr >= minpresII)
                                            {
                                                njdate_evng += 1;
                                                njdate += 0.5;
                                            }
                                            if (per_ondu >= 1)
                                            {
                                                Onduty += 0.5;
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
                                            if (temp_unmark == NoHrs - fnhrs)
                                            {
                                                per_holidate_evng += 1;
                                                per_holidate += 0.5;

                                                unmark = 0;
                                            }
                                            else
                                            {
                                                dum_unmark += unmark;
                                            }


                                            if ((NoHrs - fnhrs) - temp_unmark >= minpresII)
                                            {
                                                workingdays += 0.5;
                                            }
                                            evng_conducted_half_days += 1;
                                            evng_conduct_hr += 1;
                                            tot_evng_con_hrs += (evng_conduct_hr * (NoHrs - fnhrs));
                                        }

                                        per_perhrs = 0;
                                        per_ondu = 0;
                                        per_leave = 0;
                                        per_abshrs = 0;
                                        unmark = 0; //hided
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
                                    dumm_from_date = dumm_from_date.AddDays(1);
                                    if (dumm_from_date.Day == 1)
                                    {
                                        cal_from_date++;
                                        if (moncount > next)
                                        {
                                            next++; //  next++;
                                        }
                                    }
                                    //if (moncount > next)
                                    //{
                                    //    i--;
                                    //}
                                }
                                i = mmyycount + 1;
                            }
                            else//Added by srinath 13/10/2014
                            {
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
                    nohrsprsentperday = 0;
                    noofdaypresen = 0;
                }
                int diff_Date = per_from_date.Day - dumm_from_date.Day;
            }

            if (check == 1)
            {

                per_tot_ondu = tot_ondu;
                per_tot_ml = tot_ml;
                per_njdate = njdate;
                pre_present_date = Present - njdate;
                per_per_hrs = tot_per_hrs;
                per_absent_date = Absent;
                pre_ondu_date = Onduty;
                pre_leave_date = Leave;
                // per_workingdays = workingdays - per_holidate - per_njdate;
                per_workingdays = workingdays - per_njdate;
                per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - ((Convert.ToInt16(per_holidate_mng) * fnhrs) + (Convert.ToInt16(per_holidate_evng) * (NoHrs - fnhrs))) - notconsider_value - dum_unmark; //dum_unmark hided on 08.08.12 // ((Convert.ToInt16(njdate_mng) * fnhrs) + (Convert.ToInt16(njdate_evng) * (NoHrs - fnhrs)));

                per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value; //added on 08.08.12,notconsider value added for hr suspension,09.08.12 mythili

                per_workingdays1 = ((tot_morn_con_hrs + tot_evng_con_hrs) - my_un_mark) - notconsider_value;
                //  per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) -  notconsider_value - dum_unmark;
                per_dum_unmark = dum_unmark; //hided on 08.08.12
            }
            if (check == 2)
            {
                cum_tot_ondu = tot_ondu;
                cum_tot_ml = tot_ml;
                cum_njdate = njdate;
                cum_present_date = Present - njdate;
                cum_per_perhrs = tot_per_hrs;
                cum_absent_date = Absent;
                cum_ondu_date = Onduty;
                cum_leave_date = Leave;
                //cum_workingdays = workingdays - per_holidate - cum_njdate;
                cum_workingdays = workingdays - cum_njdate;
                cum_per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - ((Convert.ToInt16(per_holidate_mng) * fnhrs) + (Convert.ToInt16(per_holidate_evng) * (NoHrs - fnhrs))) - notconsider_value - dum_unmark; //added dum_unmrk on 08.08.12
                //     cum_per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - notconsider_value;

                cum_per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value; //added on 08.08.12,notconsider value added for hr suspension,09.08.12 mythili

                cum_per_workingdays1 = ((tot_morn_con_hrs + tot_evng_con_hrs) - my_un_mark) - notconsider_value;
                cum_dum_unmark = dum_unmark; //hided on 08.08.12
                cum_tot_point = absent_point + leave_point;
            }
            if (hatdicOD.Count > 0)
            {

            }
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
            tot_ml = 0;
        }
        //   catch
        {
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

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {

        Showgrid.Visible = false;
        btnletter.Visible = false;
        lblnorec.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;

        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnclose.Visible = false;
        binddate();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        Showgrid.Visible = false;
        btnletter.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        bindbranch();
        //   Get_Semester();
        bindsem();
        BindSectionDetail();
        binddate();
        btnclose.Visible = false;

    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblnorec.Visible = false;
        Showgrid.Visible = false;
        btnletter.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        btnclose.Visible = false;
        if (!Page.IsPostBack == false)
        {
            ddlsemester.Items.Clear();
        }
        try
        {
            if (ddlbranch.Items.Count > 0)
            {
                //  Get_Semester();
                bindsem();
                BindSectionDetail();
                binddate();
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
        lblnorec.Visible = false;
        Showgrid.Visible = false;
        btnletter.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        btnPrint.Visible = false;
        if (!Page.IsPostBack == false)
        {
            ddlsection.Items.Clear();
        }
        BindSectionDetail();
        binddate();
        btnclose.Visible = false;
    }

    protected void cbcumpercent_CheckedChanged(object sender, EventArgs e)
    {
        Showgrid.Visible = false;
        btnletter.Visible = false;
        btnclose.Visible = false;
        if (cbcumpercent.Checked == true)
        {
            Showgrid.Visible = false;
            btnletter.Visible = false;
            btnxl.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            Showgrid.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            btnPrint.Visible = false;
            cbpoints.Visible = true;
            lblcumfrm.Visible = true;
            tbfmcumdate.Visible = true;
            lblcumto.Visible = true;
            tbtocumdate.Visible = true;

        }
        else
        {
            btnletter.Visible = false;
            btnxl.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            Showgrid.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            cbpoints.Visible = false;
            lblcumfrm.Visible = false;
            tbfmcumdate.Visible = false;
            lblcumto.Visible = false;
            tbtocumdate.Visible = false;
            btnPrint.Visible = false;
        }

    }

    public void BindBatch()
    {

        DataSet ds = ClsAttendanceAccess.GetBatchDetail();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlbatch.DataSource = ds;
            ddlbatch.DataTextField = "Batch_year";
            ddlbatch.DataValueField = "Batch_year";
            ddlbatch.DataBind();
            //ddlbatch.Items.Insert(0, new ListItem(DateTime.Today.ToString("yyyy"), "-1"));
            ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;

        }


    }

    public void BindDegree()
    {


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

    //public void print_btngo()
    //{
    //    try
    //    {
    //        FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
    //        FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;

    //        FpSpread1.Sheets[0].RowHeader.Columns[0].Visible = false;
    //        final_print_col_cnt = 0;
    //        norecordlbl.Visible = false;
    //        check_col_count_flag = false;


    //        FpSpread1.Sheets[0].SheetCorner.RowCount = 0;
    //        FpSpread1.Sheets[0].ColumnCount = 0;
    //        FpSpread1.Sheets[0].RowCount = 0;
    //        FpSpread1.Sheets[0].SheetCorner.RowCount = 8;
    //        FpSpread1.Sheets[0].ColumnCount = 6;


    //        hat.Clear();
    //        hat.Add("college_code", Session["collegecode"].ToString());
    //        hat.Add("form_name", "cumreport.aspx");
    //        dsprint = d2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
    //        if (dsprint.Tables[0].Rows.Count > 0)
    //        {
    //            //lblpages.Visible = true;
    //            //ddlpage.Visible = true;

    //            // first_btngo();
    //            function_btnclick();

    //            final_print_col_cnt = 0;
    //            isonumber = dsprint.Tables[0].Rows[0]["ISOCode"].ToString();
    //            //3. header add
    //            if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
    //            {
    //                new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
    //                string[] new_header_string_split = new_header_string.Split(',');

    //                new_header_string_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString();
    //                string[] header_align_index_split = new_header_string_index.Split(',');
    //                // FpSpread1.Sheets[0].SheetCorner.RowCount = FpSpread1.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
    //            }
    //            //3. end header add

    //            //1.set visible columns
    //            column_field = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------------get column field
    //            if (column_field != "" && column_field != null)
    //            {
    //                check_col_count_flag = true;

    //                for (col_count_all = 0; col_count_all < FpSpread1.Sheets[0].ColumnCount; col_count_all++)
    //                {
    //                    FpSpread1.Sheets[0].Columns[col_count_all].Visible = false;//------------invisible all column                                
    //                }

    //                int first_child = 0;
    //                printvar = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------visible setting columns
    //                string[] split_printvar = printvar.Split(',');
    //                for (int splval = 0; splval <= split_printvar.GetUpperBound(0); splval++)
    //                {
    //                    span_cnt = 0;
    //                    string[] split_star = split_printvar[splval].Split('*');
    //                    if (split_star.GetUpperBound(0) > 0)
    //                    {
    //                        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount - 1; col_count++)
    //                        {
    //                            int child_node = 0;
    //                            if (FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), col_count].Text == split_star[0])
    //                            {
    //                                child_span_count = 0;
    //                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), col_count, 1, 1);
    //                                string[] split_star_doller = split_star[1].Split('$');
    //                                for (int doller_count = 1; doller_count < (split_star_doller.GetUpperBound(0)); doller_count++)
    //                                {
    //                                    for (int col_count_child = col_count; col_count_child < FpSpread1.Sheets[0].ColumnCount - 1; col_count_child++)
    //                                    {
    //                                        if (FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), col_count_child].Text == split_star_doller[doller_count])
    //                                        {
    //                                            FpSpread1.Sheets[0].Columns[col_count_child].Visible = true;
    //                                            final_print_col_cnt++;
    //                                            child_node++;
    //                                            if (child_node == 1)
    //                                            {
    //                                                first_child = col_count_child;
    //                                                FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), col_count_child].Text = FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), col_count].Text;
    //                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), col_count_child, 1, 1);
    //                                            }
    //                                            else
    //                                            {
    //                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), first_child, 1, 1);


    //                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), first_child, 1, child_node);
    //                                            }

    //                                            // 
    //                                            col_count = col_count_child++;
    //                                            break;


    //                                        }
    //                                    }
    //                                }


    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //                        {
    //                            if (FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), col_count].Text == split_printvar[splval])
    //                            {
    //                                FpSpread1.Sheets[0].Columns[col_count].Visible = true;



    //                                final_print_col_cnt++;
    //                                break;
    //                            }
    //                        }
    //                    }
    //                }


    //                //1 end.set visible columns


    //                //2.Footer setting
    //               // if (dsprint.Tables[0].Rows[0]["footer"].ToString() != null && dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
    //                //{
    //                //    footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());
    //                //    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 3;

    //                //    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 3), 0].ColumnSpan = FpSpread1.Sheets[0].ColumnCount;
    //                //    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 2), 0].ColumnSpan = FpSpread1.Sheets[0].ColumnCount;

    //                //    //FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 3), 0].Border.BorderColorBottom = Color.White;
    //                //    //FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 2), 0].Border.BorderColorTop = Color.White;
    //                //    //FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 2), 0].Border.BorderColorBottom = Color.White;
    //                //    //FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), 0].Border.BorderColorTop = Color.White;


    //                //    footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
    //                //    string[] footer_text_split = footer_text.Split(',');
    //                //    footer_text =string.Empty;




    //                //    if (final_print_col_cnt < footer_count)
    //                //    {
    //                //        for (int concod_footer = 0; concod_footer < footer_count; concod_footer++)
    //                //        {
    //                //            if (footer_text == "")
    //                //            {
    //                //                footer_text = footer_text_split[concod_footer].ToString();
    //                //            }
    //                //            else
    //                //            {
    //                //                footer_text = footer_text + "   " + footer_text_split[concod_footer].ToString();
    //                //            }
    //                //        }

    //                //        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount - 1; col_count++)
    //                //        {
    //                //            if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                //            {
    //                //                FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, final_print_col_cnt);
    //                //                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text;
    //                //                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].HorizontalAlign = HorizontalAlign.Center;
    //                //                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                //                break;
    //                //            }
    //                //        }

    //                //    }

    //                //    else if (final_print_col_cnt == footer_count)
    //                //    {
    //                //        int x = 0, y = 0;
    //                //        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount - 1; col_count++)
    //                //        {
    //                //            if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                //            {
    //                //                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                //                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].HorizontalAlign = HorizontalAlign.Center;
    //                //                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                //                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                //                if (col_count + 1 < FpSpread1.Sheets[0].ColumnCount)
    //                //                {
    //                //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorLeft = Color.White;
    //                //                }
    //                //                if (temp_count != 0)
    //                //                {
    //                //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                //                }
    //                //                else
    //                //                {
    //                //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.Black;
    //                //                }
    //                //                x = col_count;
    //                //                temp_count++;
    //                //                if (temp_count == footer_count)
    //                //                {
    //                //                    break;
    //                //                }
    //                //            }
    //                //        }
    //                //        //   FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), x].Border.BorderColorRight = Color.Black ;
    //                //    }

    //                //    else
    //                //    {
    //                //        temp_count = 0;
    //                //        split_col_for_footer = final_print_col_cnt / footer_count;
    //                //        footer_balanc_col = final_print_col_cnt % footer_count;
    //                //        int x = 0;
    //                //        for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount - 1; col_count++)
    //                //        {
    //                //            if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                //            {
    //                //                x = col_count;
    //                //                if (temp_count == 0)
    //                //                {
    //                //                    FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer + footer_balanc_col);
    //                //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.Black;
    //                //                }
    //                //                else
    //                //                {
    //                //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                //                    FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer);

    //                //                }
    //                //                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                //                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].HorizontalAlign = HorizontalAlign.Center;
    //                //                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                //                if (col_count - 1 >= 0)
    //                //                {
    //                //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorLeft = Color.White;
    //                //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorRight = Color.White;
    //                //                }

    //                //                FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                //                if (col_count + 1 < FpSpread1.Sheets[0].ColumnCount - 1)
    //                //                {
    //                //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorLeft = Color.White;
    //                //                }
    //                //                if (col_count + 1 < FpSpread1.Sheets[0].ColumnCount - 1)
    //                //                {
    //                //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorLeft = Color.White;
    //                //                    FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorRight = Color.White;
    //                //                }


    //                //                temp_count++;
    //                //                if (temp_count == 0)
    //                //                {
    //                //                    col_count = col_count + split_col_for_footer + footer_balanc_col;
    //                //                }
    //                //                else
    //                //                {
    //                //                    col_count = col_count + split_col_for_footer;
    //                //                }
    //                //                if (temp_count == footer_count)
    //                //                {
    //                //                    if (col_count < FpSpread1.Sheets[0].ColumnCount - 1)
    //                //                    {
    //                //                        FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.Black;
    //                //                        break;
    //                //                    }
    //                //                }


    //                //            }
    //                //        }
    //                //        Boolean temp_check = false;
    //                //        for (int xx = x + 1; xx < FpSpread1.Sheets[0].ColumnCount; xx++)
    //                //        {
    //                //            if (FpSpread1.Sheets[0].Columns[xx].Visible == true)
    //                //            {
    //                //                temp_check = true;
    //                //            }
    //                //        }
    //                //        if (temp_check == false)
    //                //        {
    //                //            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), x].Border.BorderColorRight = Color.Black;
    //                //        }
    //                //    }



    //                //}

    //                //2 end.Footer setting






    //                //4.college information setting

    //               // setheader_print();

    //                //4 end.college information setting

    //            }
    //            else
    //            {
    //                FpSpread1.Visible = false;
    //                btnletter.Visible = false;
    //                btnxl.Visible = false;
    //                lblrptname.Visible = false;
    //                txtexcelname.Visible = false;
    //                pageset_pnl.Visible = false;
    //                //lblpages.Visible = false;
    //                //ddlpage.Visible = false;
    //                norecordlbl.Visible = true;
    //                btnprintmaster.Visible = false;
    //                Printcontrol.Visible = false;
    //                norecordlbl.Text = "Select Atleast One Column Field From The Treeview";
    //            }
    //        }
    //        FpSpread1.Sheets[0].Columns[0].Width = 50;
    //        FpSpread1.Width = final_print_col_cnt * 100;
    //    }
    //    catch
    //    {
    //    }
    //}

    //public void setheader_print()
    //{
    //    // dsprint.Tables[0].Rows[0]["column_fields"].ToString();
    //    //  try
    //    {
    //        FpSpread1.Sheets[0].ColumnHeader.RowCount = 0;
    //        FpSpread1.Sheets[0].ColumnHeader.RowCount = 7;
    //        //header_text();//hidden By Srinath


    //        temp_count = 0;


    //        MyImg mi = new MyImg();
    //        mi.ImageUrl = "~/images/10BIT001.jpeg";
    //        mi.ImageUrl = "Handler/Handler2.ashx?";
    //        MyImg mi2 = new MyImg();
    //        mi2.ImageUrl = "~/images/10BIT001.jpeg";
    //        mi2.ImageUrl = "Handler/Handler5.ashx?";

    //        if (final_print_col_cnt == 1)
    //        {
    //            for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    one_column();
    //                    break;
    //                }
    //            }

    //        }

    //        else if (final_print_col_cnt == 2)
    //        {
    //            for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    if (temp_count == 0)
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
    //                        //aruna FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount), 1);
    //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount - 3), 1);
    //                        if (leftlogo == "1" && leftlength != "")
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                        }
    //                       // FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                    else
    //                    {
    //                        one_column();
    //                        for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount - 2; row_cnt++)
    //                        {
    //                           // FpSpread1.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                        }
    //                    }
    //                    temp_count++;
    //                    if (temp_count == 2)
    //                    {
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //        else if (final_print_col_cnt == 3)
    //        {
    //            for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    if (temp_count == 0)
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
    //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount), 1);
    //                        if (leftlogo == "1" && leftlength != "")
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                        }
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                    else if (temp_count == 1)
    //                    {
    //                        one_column();
    //                        for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount - 2; row_cnt++)
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
    //                        }
    //                    }
    //                    else if (temp_count == 2)
    //                    {

    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount), 1);
    //                            if (rightlogo == "1" && rightlength != "")
    //                            {
    //                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi2;
    //                            }
    //                            FpSpread1.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.Black;
    //                        }
    //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col_count].Border.BorderColorRight = Color.Black;
    //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col_count].Border.BorderColorLeft = Color.White;
    //                    }
    //                    temp_count++;
    //                    if (temp_count == 3)
    //                    {
    //                        break;
    //                    }
    //                }
    //            }

    //        }
    //        else//-----------column count more than 3
    //        {
    //            for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    if (temp_count == 0)
    //                    {
    //                        start_column = col_count;
    //                        FpSpread1.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
    //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount), 1);
    //                        if (leftlogo == "1" && leftlength != "")
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                        }
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;

    //                    }


    //                    if (final_print_col_cnt == temp_count + 1)
    //                    {
    //                        end_column = col_count;

    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount), 1);
    //                            if (rightlogo == "1" && rightlength != "")
    //                            {
    //                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi2;
    //                            }
    //                            FpSpread1.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.Black;

    //                        }

    //                    }
    //                    temp_count++;
    //                    if (final_print_col_cnt == temp_count)
    //                    {
    //                        break;
    //                    }
    //                }
    //            }
    //            temp_count = 0;
    //            for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    if (temp_count == 1)
    //                    {
    //                        more_column();
    //                        for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount - 2; row_cnt++)
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
    //                        }
    //                    }
    //                    temp_count++;
    //                }
    //            }
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, end_column].Border.BorderColorRight = Color.Black;
    //            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, end_column].Border.BorderColorLeft = Color.White;
    //        }
    //    }
    //    // catch
    //    {
    //    }
    //}


    //public void one_column()
    //{
    //    //  try
    //    {



    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Text = coll_name;
    //       // FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;

    //        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Text = address1 + "-" + address2 + "-" + address3;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorTop = Color.White;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorBottom = Color.White;

    //        if (phoneno != "" && phoneno != null)
    //        {
    //            phone = "Phone:" + phoneno;
    //        }
    //        else
    //        {
    //            phone =string.Empty;
    //        }

    //        if (faxno != "" && faxno != null)
    //        {
    //            fax = "  Fax:" + faxno;
    //        }
    //        else
    //        {
    //            fax =string.Empty;
    //        }

    //        FpSpread1.Sheets[0].ColumnHeader.Cells[2, col_count].Text = phone + fax;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorTop = Color.White;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorBottom = Color.White;

    //        if (email != "" && faxno != null)
    //        {
    //            email_id = "Email:" + email;
    //        }
    //        else
    //        {
    //            email_id =string.Empty;
    //        }


    //        if (website != "" && website != null)
    //        {
    //            web_add = "  Web Site:" + website;
    //        }
    //        else
    //        {
    //            web_add =string.Empty;
    //        }

    //        FpSpread1.Sheets[0].ColumnHeader.Cells[3, col_count].Text = email_id + web_add;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorTop = Color.White;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorBottom = Color.White;

    //        if (form_name != "" && form_name != null && form_name == "Cumulative Attendance Report")
    //        {
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, col_count].Text = "Cumulative Attendance Report";
    //        }
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorTop = Color.White;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorBottom = Color.White;



    //        //  FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Text = degree_deatil;
    //        if (degree_deatil != "")
    //        {
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, col_count].Text = ddlbatch.SelectedItem.ToString() + " Batch - " + ddldegree.SelectedItem.ToString() + " - " + ddlbranch.SelectedItem.ToString() + " [ " + sem_roman(Convert.ToInt16(ddlsemester.SelectedItem.ToString())) + " Semester ] - " + ddlsection.SelectedItem;
    //        }
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorTop = Color.White;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorBottom = Color.White;

    //        FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Text =string.Empty;// "From: " + txtfromdate.Text + "      To: " + txttodate.Text + "       Date: " + DateTime.Now.ToString("dd/MM/yyyy");
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorTop = Color.White;


    //        int temp_count_temp = 0;


    //        if (new_header_name != null && new_header_name != "")
    //        {
    //            new_header_string_split = new_header_name.Split(',');
    //            string[] new_header_string_index_split = new_header_string_index.Split(',');
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorBottom = Color.White;
    //            for (int row_head_count = 7; row_head_count < (7 + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
    //            {
    //                FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Text = new_header_string_split[temp_count_temp].ToString();



    //                FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorTop = Color.White;
    //                if (row_head_count != (7 + new_header_string_split.GetUpperBound(0)))
    //                {
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorBottom = Color.White;
    //                }

    //                if (temp_count_temp <= new_header_string_index_split.GetUpperBound(0))
    //                {
    //                    header_alignment = new_header_string_index_split[temp_count_temp].ToString();
    //                    if (header_alignment != string.Empty)
    //                    {
    //                        if (header_alignment == "2")
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Center;
    //                        }
    //                        else if (header_alignment == "1")
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Left;
    //                        }
    //                        else
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Right;
    //                        }
    //                    }
    //                }
    //                temp_count_temp++;
    //            }
    //        }

    //    }
    //    //  catch
    //    {
    //    }

    //}

    //public void more_column()
    //{


    //    try
    //    {
    //        //  //  header_text();

    //        //    if (multi_iso.Trim() == "")
    //        //    {
    //        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count));
    //        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, 1, (end_column - col_count));
    //        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, col_count, 1, (end_column - col_count));
    //        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, col_count, 1, (end_column - col_count));
    //        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(4, col_count, 1, (end_column - col_count));
    //        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(5, col_count, 1, (end_column - col_count));
    //        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, col_count, 1, (end_column - col_count));
    //        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(7, col_count, 1, (end_column - col_count));
    //        //    }
    //        //    else
    //        //    {
    //        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count - 2));
    //        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, 1, (end_column - col_count - 2));
    //        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, col_count, 1, (end_column - col_count - 2));
    //        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, col_count, 1, (end_column - col_count - 2));
    //        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(4, col_count, 1, (end_column - col_count - 2));
    //        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(5, col_count, 1, (end_column - col_count - 2));
    //        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, col_count, 1, (end_column - col_count - 2));
    //        //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(7, col_count, 1, (end_column - col_count - 2));
    //        //    }


    //        //    if (coll_name.Trim() != "")
    //        //    {
    //        //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Text = coll_name;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[0].Tag = 1;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[0].Visible = true;
    //        //    }
    //        //    else
    //        //    {
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[0].Tag = 0;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[0].Visible = false;
    //        //    }
    //        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;



    //        //    if (address1.Trim() != "" && address2.Trim() != "" && address3.Trim() != "")
    //        //    {
    //        //        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Text = address1 + "-" + address2 + "-" + address3;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[1].Tag = 1;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = true;
    //        //    }
    //        //    else if (address1 != string.Empty && address3 != string.Empty && pincode != string.Empty)
    //        //    {
    //        //        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Text = address1 + "," + address3 + "-" + pincode;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[1].Tag = 1;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = true;

    //        //    }
    //        //    else
    //        //    {
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[1].Tag = 0;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = false;
    //        //    }

    //        //    FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorTop = Color.White;
    //        //    FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorBottom = Color.White;

    //        //    if (phoneno != "" && phoneno != null)
    //        //    {
    //        //        phone = "Phone:" + phoneno;
    //        //    }
    //        //    else
    //        //    {
    //        //        phone =string.Empty;
    //        //    }

    //        //    if (faxno != "" && faxno != null)
    //        //    {
    //        //        fax = "  Fax:" + faxno;
    //        //    }
    //        //    else
    //        //    {
    //        //        fax =string.Empty;
    //        //    }


    //        //    if (phone.Trim() == "" && fax.Trim() == "")
    //        //    {
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[2].Tag = 0;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = false;
    //        //    }
    //        //    else
    //        //    {

    //        //        FpSpread1.Sheets[0].ColumnHeader.Cells[2, col_count].Text = phone + fax;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[2].Tag = 1;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = true;
    //        //    }
    //        //    FpSpread1.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorTop = Color.White;
    //        //    FpSpread1.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorBottom = Color.White;

    //        //    if (email != "" && faxno != null)
    //        //    {
    //        //        email_id = "Email:" + email;
    //        //    }
    //        //    else
    //        //    {
    //        //        email_id =string.Empty;
    //        //    }


    //        //    if (website != "" && website != null)
    //        //    {
    //        //        web_add = "  Web Site:" + website;
    //        //    }
    //        //    else
    //        //    {
    //        //        web_add =string.Empty;
    //        //    }


    //        //    if (email_id.Trim() == "" && web_add.Trim() == "")
    //        //    {

    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[3].Tag = 0;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[3].Visible = false;
    //        //    }

    //        //    else
    //        //    {
    //        //        FpSpread1.Sheets[0].ColumnHeader.Cells[3, col_count].Text = email_id + web_add;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[3].Tag = 1;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[3].Visible = true;
    //        //    }

    //        //    FpSpread1.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorTop = Color.White;
    //        //    FpSpread1.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorBottom = Color.White;

    //        //    if (form_name != "" && form_name != null && form_name == "Cumulative Attendance Report")
    //        //    {
    //        //        FpSpread1.Sheets[0].ColumnHeader.Cells[4, col_count].Text = "Cumulative Attendance Report";

    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[4].Tag = 1;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[4].Visible = true;
    //        //    }
    //        //    else
    //        //    {
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[4].Tag = 0;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[4].Visible = false;
    //        //    }

    //        //    FpSpread1.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorTop = Color.White;
    //        //    FpSpread1.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorBottom = Color.White;


    //        //    //    FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Text = degree_deatil;

    //        //    if (degree_deatil.Trim() != "")
    //        //    {
    //        //        FpSpread1.Sheets[0].ColumnHeader.Cells[5, col_count].Text = ddlbatch.SelectedItem.ToString() + " Batch - " + ddldegree.SelectedItem.ToString() + " - " + ddlbranch.SelectedItem.ToString() + " [ " + sem_roman(Convert.ToInt16(ddlsemester.SelectedItem.ToString())) + " Semester ] - " + ddlsection.SelectedItem;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[5].Tag = 1;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[5].Visible = true;
    //        //    }
    //        //    else
    //        //    {
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[5].Tag = 0;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Rows[5].Visible = false;
    //        //    }

    //        //    FpSpread1.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorTop = Color.White;
    //        //    FpSpread1.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorBottom = Color.White;

    //        //    FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Text =string.Empty;//"From: " + txtfromdate.Text + "      To: " + txttodate.Text + "       Date: " + DateTime.Now.ToString("dd/MM/yyyy");

    //        //    FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorTop = Color.White;

    //        //    FpSpread1.Sheets[0].ColumnHeader.Rows[6].Tag = 1;


    //        //    //------------------------------multi iso set
    //        //    int col_val_iso = 0, iso_start_col = 0, iso_upper_bound = 0;

    //        //    if (multi_iso.Trim() != "")
    //        //    {
    //        //        for (col_val_iso = (FpSpread1.Sheets[0].ColumnCount - 1); col_val_iso >= 0; col_val_iso--)
    //        //        {
    //        //            if (FpSpread1.Sheets[0].Columns[col_val_iso].Visible == true)
    //        //            {
    //        //                iso_start_col++;
    //        //                if (iso_start_col == 3)
    //        //                {
    //        //                    break;
    //        //                }
    //        //            }
    //        //        }


    //        //        //--------------------------------ISO Set
    //        //        int row_val = 0;
    //        //        if (multi_iso.Trim() != "")
    //        //        {
    //        //            string[] multi_iso_spt = multi_iso.Split(',');

    //        //            for (int iso = 0; iso <= multi_iso_spt.GetUpperBound(0); iso++)
    //        //            {
    //        //                if (row_val > 6)
    //        //                {
    //        //                    FpSpread1.Sheets[0].ColumnHeader.RowCount++;
    //        //                    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 0].Text = multi_iso_spt[iso];
    //        //                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 0, 1, col_val_iso);
    //        //                    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 0].Border.BorderColorRight = Color.White;
    //        //                    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 0].Border.BorderColorTop = Color.White;
    //        //                    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), 0].Border.BorderColorBottom = Color.White;
    //        //                    FpSpread1.Sheets[0].ColumnHeader.Rows[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1)].Tag = "1";
    //        //                    if (rightlogo == "1")
    //        //                    {
    //        //                        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), (FpSpread1.Sheets[0].ColumnCount - 1)].Text = multi_iso_spt[iso];
    //        //                        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), (FpSpread1.Sheets[0].ColumnCount - 1)].Border.BorderColorLeft = Color.White;
    //        //                        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), (FpSpread1.Sheets[0].ColumnCount - 1)].Border.BorderColorTop = Color.White;
    //        //                        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), (FpSpread1.Sheets[0].ColumnCount - 1)].Border.BorderColorBottom = Color.White;
    //        //                    }
    //        //                }
    //        //                if (FpSpread1.Sheets[0].ColumnHeader.Rows[row_val].Tag.ToString() == "1")
    //        //                {
    //        //                    FpSpread1.Sheets[0].ColumnHeader.Cells[row_val, col_val_iso].Text = multi_iso_spt[iso];
    //        //                    FpSpread1.Sheets[0].ColumnHeader.Cells[row_val, col_val_iso].HorizontalAlign = HorizontalAlign.Left;
    //        //                    FpSpread1.Sheets[0].ColumnHeader.Cells[row_val, col_val_iso].Border.BorderColorLeft = Color.White;
    //        //                    if (iso != 0)
    //        //                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_val, col_val_iso].Border.BorderColorTop = Color.White;
    //        //                    FpSpread1.Sheets[0].ColumnHeader.Cells[row_val, col_val_iso].Border.BorderColorBottom = Color.White;

    //        //                    if (rightlogo == "1")
    //        //                    {
    //        //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(row_val, col_val_iso, 1, 2);
    //        //                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_val, col_val_iso].Border.BorderColorRight = Color.White;
    //        //                    }
    //        //                    else
    //        //                    {
    //        //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(row_val, col_val_iso, 1, 3);
    //        //                    }
    //        //                }
    //        //                else
    //        //                {
    //        //                    FpSpread1.Sheets[0].ColumnHeader.Rows[row_val].Visible = false;
    //        //                    iso--;
    //        //                }


    //        //                row_val++;


    //        //            }

    //        //            for (int yy = multi_iso_spt.GetUpperBound(0) + 1; yy <= 6; yy++)
    //        //            {
    //        //                if (FpSpread1.Sheets[0].ColumnHeader.Cells[yy, col_val_iso].Text == "")
    //        //                {
    //        //                    if (rightlogo == "1")
    //        //                    {
    //        //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(yy, col_val_iso, 1, 2);
    //        //                        FpSpread1.Sheets[0].ColumnHeader.Cells[yy, col_val_iso].Border.BorderColorRight = Color.White;
    //        //                        FpSpread1.Sheets[0].ColumnHeader.Cells[yy, col_val_iso].Border.BorderColorTop = Color.White;
    //        //                        FpSpread1.Sheets[0].ColumnHeader.Cells[yy, col_val_iso].Border.BorderColorBottom = Color.White;
    //        //                    }
    //        //                    else
    //        //                    {
    //        //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(yy, col_val_iso, 1, 3);
    //        //                    }
    //        //                }

    //        //            }
    //        //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_val_iso].Border.BorderColorBottom = Color.Black;
    //        //        }
    //        //    }
    //        //    //-------------------------------------------

    //        //    temp_count_temp = 0;
    //        //    int row_cnt_after_iso = 0;
    //        //    row_cnt_after_iso = FpSpread1.Sheets[0].ColumnHeader.RowCount;

    //        //    if (new_header_name != null && new_header_name != "")
    //        //    {
    //        //        new_header_string_split = new_header_name.Split(',');

    //        //        FpSpread1.Sheets[0].ColumnHeader.RowCount = FpSpread1.Sheets[0].ColumnHeader.RowCount + new_header_string_split.GetUpperBound(0) + 1;

    //        //        string[] new_header_string_index_split = new_header_string_index.Split(',');
    //        //        FpSpread1.Sheets[0].ColumnHeader.Cells[row_cnt_after_iso - 1, col_count].Border.BorderColorBottom = Color.White;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Cells[row_cnt_after_iso - 1, start_column].Border.BorderColorBottom = Color.White;
    //        //        FpSpread1.Sheets[0].ColumnHeader.Cells[row_cnt_after_iso - 1, end_column].Border.BorderColorBottom = Color.White;

    //        //        for (int row_head_count = row_cnt_after_iso; row_head_count < (row_cnt_after_iso + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
    //        //        {
    //        //            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Text = new_header_string_split[temp_count_temp].ToString();
    //        //            FpSpread1.Sheets[0].ColumnHeader.Rows[row_head_count].Tag = 1;
    //        //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(row_head_count, col_count, 1, (end_column - col_count));



    //        //            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorTop = Color.White;
    //        //            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorTop = Color.White;
    //        //            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, end_column].Border.BorderColorTop = Color.White;
    //        //            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorLeft = Color.White;
    //        //            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorRight = Color.White;
    //        //            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorRight = Color.White;
    //        //            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, end_column].Border.BorderColorLeft = Color.White;
    //        //            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorTop = Color.White;
    //        //            FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, end_column].Border.BorderColorBottom = Color.White;

    //        //            if (row_head_count != (row_cnt_after_iso + new_header_string_split.GetUpperBound(0)))
    //        //            {
    //        //                FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorBottom = Color.White;
    //        //                FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorBottom = Color.White;
    //        //                FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, end_column].Border.BorderColorBottom = Color.White;
    //        //            }

    //        //            if (temp_count_temp <= new_header_string_index_split.GetUpperBound(0))
    //        //            {
    //        //                header_alignment = new_header_string_index_split[temp_count_temp].ToString();
    //        //                if (header_alignment != string.Empty)
    //        //                {
    //        //                    if (header_alignment == "2")
    //        //                    {
    //        //                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Center;
    //        //                    }
    //        //                    else if (header_alignment == "1")
    //        //                    {
    //        //                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Left;
    //        //                    }
    //        //                    else
    //        //                    {
    //        //                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Right;
    //        //                    }
    //        //                }
    //        //            }
    //        //            temp_count_temp++;
    //        //        }
    //        //    }

    //        //------------------header

    //        if (cbcumpercent.Checked == true)
    //        {

    //            if (days1 < 0)
    //            {



    //            }
    //            else
    //            {

    //                spsize_header();
    //                spsizeforcum_header();


    //            }
    //        }
    //        else
    //        {
    //            spsize_header();
    //        }
    //    }
    //    catch
    //    {
    //    }


    //}

    public void header_text()
    {

        string sec_val = string.Empty;

        if (ddlsection.SelectedValue.ToString() != string.Empty && ddlsection.SelectedValue.ToString() != null)
        {
            sec_val = "Section: " + ddlsection.SelectedItem.ToString();
        }
        else
        {
            sec_val = string.Empty;
        }

        Boolean check_print_row = false;
        SqlDataReader dr_collinfo;
        con.Close();
        con.Open();
        cmd = new SqlCommand("select isnull(college_name,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website,isnull(form_heading_name,'') as form_name,isnull(batch_degree_branch,'') as degree_deatil,isnull(header_align,'') as header_alignment,isnull(header_flag_value,'') as view_header,isnull(state,'')  as state,isnull(pincode,'') as pincode,affliated,leftlogo,rightlogo,datalength(leftlogo) as leftlength,datalength(rightlogo) as rightlength,MultiISOCode,new_header_name  from print_master_setting  where form_name='cumreport.aspx'", con);
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
                if (Convert.ToString(dr_collinfo["degree_deatil"]) != "")
                {
                    degree_deatil = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlsemester.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
                }


                header_alignment = dr_collinfo["header_alignment"].ToString();
                view_header = dr_collinfo["view_header"].ToString();
                new_header_name = dr_collinfo["new_header_name"].ToString();
                leftlogo = dr_collinfo["leftlogo"].ToString();
                rightlogo = dr_collinfo["rightlogo"].ToString();
                leftlength = dr_collinfo["leftlength"].ToString();
                rightlength = dr_collinfo["rightlength"].ToString();
                multi_iso = dr_collinfo["MultiISOCode"].ToString();
                pincode = dr_collinfo["pincode"].ToString();

            }
        }
        if (check_print_row == false)
        {

            con.Close();
            con.Open();
            cmd = new SqlCommand("select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website from collinfo  where college_code=" + Session["collegecode"] + "", con);
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
                    form_name = "  Cumulative Attendance Report ";
                    degree_deatil = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlsemester.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";

                    leftlogo = string.Empty;
                    rightlogo = string.Empty;
                    leftlength = string.Empty;
                    rightlength = string.Empty;
                    multi_iso = string.Empty;
                    // header_alignment = ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlSemYr.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
                    // view_header = dr_collinfo["view_header"].ToString();
                }

            }
        }
    }

    protected void txtfromdate_TextChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        dateerr.Visible = false;
        lblnorec.Visible = false;
        Showgrid.Visible = false;
        btnletter.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        btnPrint.Visible = false;
        btnclose.Visible = false;
    }

    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        dateerr.Visible = false;
        lblnorec.Visible = false;
        Showgrid.Visible = false;
        btnletter.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        btnclose.Visible = false;
    }

    protected void btngo1_Click(object sender, EventArgs e)
    {

        spsize();
        // load_students();
    }

    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {

        //ddlpageload();
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

    protected void tbfmcumdate_TextChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Showgrid.Visible = false;
        btnletter.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        btnclose.Visible = false;
        btnPrint.Visible = false;
    }

    protected void tbtocumdate_TextChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Showgrid.Visible = false;
        btnletter.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        btnclose.Visible = false;
        btnPrint.Visible = false;
    }

    protected void ddlsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Showgrid.Visible = false;
        btnletter.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        btnclose.Visible = false;
        btnPrint.Visible = false;
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(Showgrid, reportname);
            }
            else
            {
                lblnorec.Text = "Please Enter Your Report Name";
                lblnorec.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

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

    public void getspecial_hr()
    {
        //  try
        {
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

                string splhr_query_master = "select attendance from specialhr_attendance where roll_no='" + dd + "'  and hrdet_no in(" + hrdetno + ")";

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
                                notconsider_value += 1;
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
                            if (value == "4")
                            {
                                tot_ml_spl += 1;
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
                        }
                    }
                }
                if (check == 1)
                {

                    per_abshrs_spl_fals = per_abshrs_spl;
                    tot_per_hrs_spl_fals = tot_per_hrs_spl;
                    per_leave_fals = per_leave;
                    tot_conduct_hr_spl_fals = tot_conduct_hr_spl;
                    tot_ondu_spl_fals = tot_ondu_spl;
                    tot_ml_spl_fals = tot_ml_spl;
                }
                else if (check == 2)
                {
                    per_abshrs_spl_true = per_abshrs_spl;
                    tot_per_hrs_spl_true = tot_per_hrs_spl;
                    per_leave_true = per_leave;
                    tot_conduct_hr_spl_true = tot_conduct_hr_spl;
                    tot_ondu_spl_true = tot_ondu_spl;
                    tot_ml_spl_true = tot_ml_spl;
                }
            }
        }
        //  catch
        {
        }
    }

    protected void btnletter_Click(object sender, EventArgs e)
    {

        int activerow = Convert.ToInt32(Session["Gridcellrow"]);

        btnclose.Visible = false;
        if (activerow >= 0)
        {
            load_certificate(activerow);
            // btngo_Click(sender, e);

        }


    }

    protected void load_certificate(int res)
    {
        try
        {
            if (res > 2)
            {

                string dt2 = DateTime.Today.ToShortDateString();
                string[] dt2split = dt2.Split(new Char[] { '/' });


                int rowcount;

                string roll_admit = "";
                string studentname = Convert.ToString(Showgrid.Rows[res].Cells[3].Text);
                string rollno = Convert.ToString(Showgrid.Rows[res].Cells[1].Text);
                //string semester = dicletterreport[res];


                contentDiv.InnerHtml = "";
                StringBuilder html = new StringBuilder();



                string str = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,pincode,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website from collinfo  where college_code=" + Session["collegecode"] + "";
                con.Close();
                con.Open();
                SqlCommand comm = new SqlCommand(str, con);
                SqlDataReader drr = comm.ExecuteReader();
                drr.Read();

                string coll_name = Convert.ToString(drr["collname"]);
                string coll_address1 = Convert.ToString(drr["address1"]);
                string coll_address2 = Convert.ToString(drr["address2"]);
                string coll_address3 = Convert.ToString(drr["address3"]);
                string pin_code = Convert.ToString(drr["pincode"]);

                html.Append("<center> <div style='height: 990px; width: 100%; border: 0px solid black; margin-left: 5px; margin: 0px; page-break-after: always;'> <center><div style='border: 0px solid black'>  <center>");

                html.Append("<table cellspacing='0' cellpadding='0' style='width: 95%; ' border='0'>");

                html.Append("<tr><td style='width: 50px;'></td><td style='text-align: left;' > <img  src='" + "../college/Left_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpeg?'" + "  alt='' style='height: 100px; width: 120px;' /></td><td style='font-size: 12px; font-family: Book Antiqua;  border: 0px solid black; text-align: center;'><span style='font-size: 14px;font-weight:bold;'>" + coll_name + "</span> <br><span style='font-size: 14px;font-weight:bold;'>" + coll_address1 + coll_address2 + coll_address3 + "-" + "    " + pin_code + "." + " </span></td><td style='text-align: right;' > <img  src='" + "../college/Right_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpeg?'" + " alt='' style='height: 100px; width: 120px;' /></td></tr><tr><td style='width: 50px;'></td><td></td><td ></td><td style='text-align: right;' ><span style='font-size: 14px;font-weight:bold;'> </span></td></tr> ");

                html.Append(" </table>");

                string sqlbalance = "select isnull(sum(balance),0) as balance from fee_status where roll_admit='" + roll_admit + "'";
                SqlDataAdapter dabal = new SqlDataAdapter(sqlbalance, con);
                con.Close();
                con.Open();
                DataSet dsbal = new DataSet();
                dabal.Fill(dsbal);
                string balances = string.Empty;
                if (dsbal.Tables[0].Rows.Count > 0)
                {
                    balances = dsbal.Tables[0].Rows[0]["balance"].ToString();
                }


                html.Append("<center> <table style='width: 95%; margin-top: 1px; margin-bottom: 2px; font-size: medium;'cellpadding='5' cellspacing='0'> <tr><td  style='text-align: left; width: auto;'>Ref:SREC/ATTN</td><td></td><td></td><td style='text-align: right; width: auto;'  &nbsp;&nbsp;>Date:" + txttodate.Text.ToString() + "</td> </tr><tr></tr><tr><td  style='text-align:left; width:auto;'>Dear Parent,</td></tr></table>");

                string percentage = string.Empty;

                if (Session["daywise"].ToString() == "0")
                {
                    if (hashrwise.Contains(Convert.ToString(rollno)))
                    {

                        percentage = hashrwise[rollno].ToString();
                    }
                }
                else
                {
                    if (hasdaywise.Contains(Convert.ToString(rollno)))
                    {
                        percentage = hasdaywise[rollno].ToString();
                    }
                }
                string acryn = string.Empty;
                con2a.Open();
                SqlCommand cmdacr = new SqlCommand("select acronym from degree where dept_code=" + ddlbranch.SelectedValue.ToString() + "", con2a);
                SqlDataReader dracr = cmdacr.ExecuteReader();
                if (dracr.Read())
                {
                    acryn = dracr["acronym"].ToString();
                }
                dracr.Close();
                con2a.Close();

                html.Append(" <div>Subject:Shortage of your ward's attendance-Reg.<br /><br />Your  ward " + studentname + " of  " + ddlbatch.Text.ToString() + "  Year  " + ddldegree.SelectedItem.ToString() + " , " + acryn + " Branch <br /> (Reg.No : " + rollno + " ) has earned only " + percentage + " % of attendance for the period of ending " + txttodate.Text.ToString() + ".<br /><br />As per the Regulations,a candidate can be permitted for the end semester examination only <br />");






                int eligible = 0;
                con2a.Open();
                SqlCommand cmdpercentage = new SqlCommand(" select percent_eligible_for_exam from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsemester.SelectedValue.ToString() + "", con2a);
                SqlDataReader dreligible = cmdpercentage.ExecuteReader();
                if (dreligible.Read())
                {
                    eligible = Convert.ToInt32(dreligible["percent_eligible_for_exam"]);
                }
                dreligible.Close();
                con2a.Close();

                html.Append("on earning " + eligible + " % of attendance.<br /><br />In this regard,your are requested to meet the Acadamic Coordinator/HOD at the earliest.</div><br /><br /> </center> </center> ");





                string sextype = string.Empty;
                con2a.Open();
                SqlCommand cmdstype = new SqlCommand("select applyn.sex from applyn,registration where applyn.app_no=registration.app_no  and registration.roll_no='" + rollno + "'", con2a);
                SqlDataReader drstype = cmdstype.ExecuteReader();
                if (drstype.Read())
                {
                    sextype = drstype["sex"].ToString();
                }
                drstype.Close();
                con2a.Close();
                if (sextype == "0")
                {
                    sextype = "kfdpd;";
                }
                else
                {
                    sextype = "kfspd;";
                }

                html.Append(" <table  style='width: 95%; margin-top: 1px; margin-bottom: 2px; font-size: medium;'cellpadding='5' cellspacing='0'><tr><td style='text-align: justify; width: auto; font-family:SunTommy;'>md;GilaPH </td><td></td><td></td><td></td></tr></table>");


                html.Append("<div style='text-align:justify; width: 836px; font-family: SunTommy;'>  nghUs; jq;fs; kfspd; tUifg; gjpT-njhlu;ghf<br /> " + ddldegree.SelectedItem.ToString() + ", " + acryn + "tFg;gpy; gapYk; jq;fs; " + sextype + " " + studentname + " <br />(gjpT vz;." + rollno + ") tUifg; gjpT " + percentage + " tpOf;fhL  kl;LNk.  nrk];lu; ,Wjpapy; Fiwe;jJ " + eligible + " <br />tpOf;fhL ,Ue;jhy; kl;LNk ,Wjpj; Nju;Tfs; vOj KlpAk; vd;gjid jq;fs; ftdj;jpw;F nfhz;L;tUfpNwhk;.<br />,J njhlu;ghf fy;tp xUq;fpizg;ghsu;/Jiwj;jiytiu  clNd Neupy; re;jpj;Jg; NgrTk;.  </div> <br /><br />  ");

                html.Append("<table  style='width: 95%;'><tr><td style='text-align: justify; width: auto; font-family:SunTommy;'>Mrpupau; </td><td style='text-align: justify; width: auto; font-family:SunTommy;'>fy;tp xUq;fpizg;ghsu;</td><td style='text-align: justify; width: auto; font-family:SunTommy;'>Jiwj;jiytu;</td><td style='text-align: justify; width: auto; font-family:SunTommy;'>Kjy;tH</td></tr><tr><td>Faculty Advisor</td><td>Academic Coordinator</td><td>HOD</td><td>Principal</td></tr><tr></tr><tr></tr><tr></tr><tr></tr>");


                html.Append("</table></center></div></center>");


                contentDiv.InnerHtml = html.ToString();
                contentDiv.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "btnprint", "PrintDiv();", true);




                btnclose.Visible = true;
                if (count == 0)
                {

                    btnclose.Visible = false;
                }

            }
        }
        catch
        {

        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {

        Session["column_header_row_count"] = 2;
        string sections = ddlsection.SelectedValue.ToString();
        if (sections.ToString().ToLower().Trim() == "all" || sections.ToString().ToLower().Trim() == string.Empty || sections.ToString().Trim() == "-1")
        {
            sections = string.Empty;
            Session["Sign"] = "" + ddlbatch.SelectedItem.ToString().Trim() + "," + ddlbranch.SelectedValue.ToString().Trim() + "," + ddlsemester.SelectedItem.ToString().Trim() + "";
        }
        else
        {
            Session["Sign"] = "" + ddlbatch.SelectedItem.ToString().Trim() + "," + ddlbranch.SelectedValue.ToString().Trim() + "," + ddlsemester.SelectedItem.ToString().Trim() + "," + sections + "";
            sections = "- Sec-" + sections.Trim();

        }
        string ss = null;
        // string conductedhours_ptn = FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Note.ToString();
        string degreedetails = "CUMULATIVE ATTENDANCE REPORT " + '@' + ((!forschoolsetting) ? "Class & Group : " : "Standard & Section : ") + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '-' + ddlbranch.SelectedItem.ToString() + '-' + lblsem.Text + "-" + ddlsemester.SelectedItem.ToString() + sections + '@' + "Period               : " + txtfromdate.Text.ToString() + " to " + txttodate.Text.ToString() + " ";
        string pagename = "cumreport.aspx";
        Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
        Printcontrol.Visible = true;
    }

    protected void chklsonduty_SelectedIndexChanged(object sender, EventArgs e)
    {

        //  int commcount = 0;
        for (int i = 0; i < chklsonduty.Items.Count; i++)
        {
            if (chklsonduty.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }

        if (commcount == 0)
        {
            txtonduty.Text = "--Select--";
            chksonduty.Checked = false;
        }
        else if (commcount == chklsonduty.Items.Count)
        {
            chksonduty.Checked = true;
            txtonduty.Text = "Onduty (" + commcount.ToString() + ")";
        }
        else
        {
            chksonduty.Checked = false;
            txtonduty.Text = "Onduty (" + commcount.ToString() + ")";
        }
    }

    protected void chksonduty_ChekedChange(object sender, EventArgs e)
    {
        if (chksonduty.Checked == true)
        {
            for (int i = 0; i < chklsonduty.Items.Count; i++)
            {
                chklsonduty.Items[i].Selected = true;
                txtonduty.Text = "Onduty(" + chklsonduty.Items.Count + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklsonduty.Items.Count; i++)
            {
                chklsonduty.Items[i].Selected = false;
            }
            txtonduty.Text = "--Select--";
        }
    }

    protected void chkondutyspit_CheckedChanged(object sender, EventArgs e)
    {
        if (chkondutyspit.Checked == true)
        {
            txtonduty.Visible = true;
            ponduty.Visible = true;
        }
        else
        {
            txtonduty.Visible = false;
            ponduty.Visible = false;
        }
    }

    public void loadonduty()
    {
        chklsonduty.Items.Clear();
        string collegecode = Session["collegecode"].ToString();
        string query = "select TextCode,Textval from textvaltable where TextCriteria='Attrs' and college_code=" + collegecode + " order by Textval";
        DataSet ds = new DataSet();
        ds.Dispose(); ds.Reset();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            chklsonduty.DataSource = ds;
            chklsonduty.DataTextField = "Textval";
            chklsonduty.DataValueField = "TextCode";
            chklsonduty.DataBind();
        }

        for (int i = 0; i < chklsonduty.Items.Count; i++)
        {
            chklsonduty.Items[i].Selected = true;
        }
        chksonduty.Checked = true;
        txtonduty.Text = " Onduty (" + chklsonduty.Items.Count + ")";
    }

    protected void txtfromrange_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Showgrid.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            btnletter.Visible = false;
            lblnorec.Visible = false;
            btnPrint.Visible = false;
            if (txtfromrange.Text.ToString().Trim() != "")
            {
                int frange = Convert.ToInt32(txtfromrange.Text.ToString());
                if (frange > 100)
                {
                    txtfromrange.Text = string.Empty;
                    lblnorec.Visible = true;
                    lblnorec.Text = "Please Enter Lesser than equal to 100";

                }
                if (txttorange.Text.ToString().Trim() != "")
                {
                    int trange = Convert.ToInt32(txttorange.Text.ToString());

                    if (frange > trange)
                    {
                        txtfromrange.Text = string.Empty;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Enter From Lesser than or equal to To";
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void txttorange_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Showgrid.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            lblnorec.Visible = false;
            btnletter.Visible = false;
            btnPrint.Visible = false;
            if (txttorange.Text.ToString().Trim() != "")
            {
                int trange = Convert.ToInt32(txttorange.Text.ToString());
                if (trange > 100)
                {

                    txttorange.Text = string.Empty;
                    lblnorec.Visible = true;
                    lblnorec.Text = "Please Enter Lesser than equal to 100";
                }
                if (txtfromrange.Text.ToString().Trim() != "")
                {
                    int frange = Convert.ToInt32(txtfromrange.Text.ToString());
                    if (frange > trange)
                    {
                        txttorange.Text = string.Empty;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Enter From Lesser than or equal to To";
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void btnclose_Click(object sender, EventArgs e)
    {

        btnclose.Visible = false;
    }

    public int getDayOrder(DateTime dt)
    {
        DateTime dt1 = dt;
        string DayofWeek = string.Empty;
        int dayofwe;
        DayofWeek = dt.DayOfWeek.ToString();
        dayofwe = (int)dt.DayOfWeek;
        return dayofwe;
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

    public void btnPrint11()
    {
        DAccess2 ddd2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = ddd2.select_method_wo_parameter(colQ, "Text");
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
        spReportName.InnerHtml = "Cumulative Attendance Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }
}
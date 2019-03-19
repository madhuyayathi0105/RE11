using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BalAccess;
using System.Data.SqlClient;
using System.Drawing;
using Gios.Pdf;
using System.Text;
public partial class attandance_letterfmt : System.Web.UI.Page
{

    DataSet ds = new DataSet();
    DataSet ds_attnd_pts = new DataSet();
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["con"].ToString());
    SqlCommand cmd = new SqlCommand();
    static Hashtable hasdaywise = new Hashtable();
    static Hashtable hashrwise = new Hashtable();



    int mmyycount;
    string dd = "";
    Hashtable hat = new Hashtable();
    static Boolean splhr_flag = false;
    double conduct_hour_new = 0;
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();

    DAccess2 d2 = new DAccess2();
    string strregorder = "";
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet dsprint = new DataSet();

    DataTable dtl = new DataTable();//added by rajasekar 22/08/2018
    DataRow dtrow = null;//added by rajasekar 22/08/2018

    Boolean yesflag = false;
    DateTime per_from_cumdate;
    DateTime per_to_cumdate;

    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0, tot_ml_spl_fals = 0;
    double per_leave_fals = 0;
    int per_abshrs_spl_true = 0, tot_per_hrs_spl_true = 0, tot_conduct_hr_spl_true = 0, tot_ondu_spl_true = 0, tot_ml_spl_true = 0;
    double per_leave_true = 0;

    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0, tot_ml_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0, cum_per_workingdays1 = 0;
    int notconsider_value = 0;
    string regularflag = "", new_header_string = "", new_header_string_index = "";
    string genderflag = "";
    int moncount;
    double dif_date = 0;
    double dif_date1 = 0;

    int unmark;

    int check;
    string temp_reg_no = "";
    DateTime per_from_date;
    DateTime per_to_date;
    DateTime per_from_gendate;
    DateTime per_to_gendate;

    DateTime dumm_from_date;
    DateTime Admission_date;
    string frdate, todate;
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    int final_print_col_cnt = 0;



    TimeSpan ts;

    string sem = "";
    string sec = "";
    string gsem3 = "";
    string collnamenew1 = "";
    string address1 = "";
    string address2 = "";
    string address3 = "";
    string pincode = "";
    string district = "";
    string yr_val = "";
    string yr_string = "";
    int res = 0;
    int isval = 0;
    DataSet ds11 = new DataSet();
    DAccess2 d22 = new DAccess2();

    
    string phone = "";
    string diff_date;
    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;

    int count;
    int next = 0;
    int minpresII = 0;
    string value, date;
    int i, rows_count;
    string tempvalue = "-1";
    int ObtValue = -1;
    //Opt------------
    int cal_from_date, cal_from_date_tmp, cal_from_cumdate_tmp;
    int cal_to_date, cal_to_date_tmp, cal_to_cumdate_tmp;
    //---------------
    double per_perhrs, per_abshrs, cum_perhrs;
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
    double per_tage_date, cum_tage_date, per_tage_hrs, cum_tage_hrs;
    double cum_tot_point, per_holidate;
    int per_dum_unmark, cum_dum_unmark, dum_unmark;
    int tot_per_hrs;
    double per_con_hrs, cum_con_hrs;
    double njhr, njdate, per_njdate, cum_njdate;
    double per_per_hrs, cum_per_perhrs;
    double tot_ondu, per_tot_ondu, cum_tot_ondu, cum_tot_ml, tot_ml, per_tot_ml;
    string usercode = "", collegecode = "", singleuser = "", group_user = "";


    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    string value_holi_status = "";
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = "", split_holiday_status_2 = "";

    int inirow_count = 0;

    int demfcal, demtcal;
    string monthcal;
    DataSet ds_sphr = new DataSet();
    static Hashtable ht_sphr = new Hashtable();


    string strorder = "";
    Hashtable hatonduty = new Hashtable();
    Hashtable hatcumonduty = new Hashtable();
    string strondutyvalue = "";
    int ondutycount = 0;
    static string grouporusercode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        string college = "";
        college = Session["collegecode"].ToString();
        lblnorec.Visible = false;
        if (!IsPostBack)
        {
            txtfromdate.Text = System.DateTime.Now.ToString("dd/MM/yyy");
            txttodate.Text = System.DateTime.Now.ToString("dd/MM/yyy");
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            string strdayflag;
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            Session["daywise"] = "0";
            Session["hourwise"] = "0";
            Session["checkflag"] = "0";
            con.Close();
            cmd.CommandText = "select rights from  special_hr_rights where " + grouporusercode + "";
            cmd.Connection = con;
            con.Open();
            SqlDataReader dr_rights_spl_hr = cmd.ExecuteReader();
            splhr_flag = false;
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

                    }
                }
            }

            if (Session["usercode"] != "")
            {
                string Master1 = "";
                Master1 = "select * from Master_Settings where " + grouporusercode + "";
                con.Close();
                con.Open();
                SqlDataReader mtrdr;

                SqlCommand mtcmd = new SqlCommand(Master1, con);
                mtrdr = mtcmd.ExecuteReader();
                strdayflag = "";
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

            }
            txtfromdate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");

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
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        lblnorec.Visible = false;
        Session["column_header_row_count"] = 2;
        string sections = ddlsection.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
        {
            sections = "";
            Session["Sign"] = "" + ddlbatch.SelectedItem.ToString() + "," + ddlbranch.SelectedValue.ToString() + "," + ddlsemester.SelectedItem.ToString() + "";
        }
        else
        {
            Session["Sign"] = "" + ddlbatch.SelectedItem.ToString() + "," + ddlbranch.SelectedValue.ToString() + "," + ddlsemester.SelectedItem.ToString() + "," + sections + "";
            sections = "- Sec-" + sections;

        }

        string degreedetails = "Attendance Letter Report" + '@' + "Degree :" + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '-' + ddlbranch.SelectedItem.ToString() + '-' + "Sem-" + ddlsemester.SelectedItem.ToString() + sections + '@' + "Date :" + txtfromdate.Text.ToString() + " To " + txttodate.Text.ToString();
        string pagename = "cumreport.aspx";
        
        string ss = null;
        Printcontrol.loadspreaddetails(grdover, pagename, degreedetails, 0, ss);
        Printcontrol.Visible = true;
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                
                d2.printexcelreportgrid(grdover, reportname);
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
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblnorec.Visible = false;
        grdover.Visible = false;
        
        grdover.Visible = false;
        
        grdover.Visible = false;
        btnletter.Visible = false;
        btntamilletter.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        Pnltamilformat.Visible = false;
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
            }
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }
    public void persentmonthcal()
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
                                            if (njhr >= minpresII)
                                            {
                                                njdate_evng += 1;
                                                njdate += 0.5;
                                            }
                                            if (per_ondu >= 1)
                                            {
                                                Onduty += 0.5;
                                            }



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
    public void getspecial_hr()
    {
        //  try
        {
            string hrdetno = "";
            if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
            {
                hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(dumm_from_date), ht_sphr));

            }
            if (hrdetno != "")
            {
                con.Close();
                con.Open();
                DataSet ds_splhr_query_master = new DataSet();

                string splhr_query_master = "select attendance from specialhr_attendance where roll_no='" + dd + "'  and hrdet_no in(" + hrdetno + ")";

                SqlDataReader dr_splhr_query_master;
                cmd = new SqlCommand(splhr_query_master, con);
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
                            tot_conduct_hr_spl--;
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
    public void first_btngo()
    {
        //  try
        {
            

            if (((txtfromdate.Text != string.Empty) && ((txttodate.Text != string.Empty))))
            {
                //'----------------------------------------font style---------------------------

                //'----------------------------------------------------------------------------------
                //'---------------------------------------------date validate-------------
                string valfromdate = "";
                string valtodate = "";
                string frmconcat = "";

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


                date_diff_flag_cum = true;

                TimeSpan ts = dttodate.Subtract(dtfromdate);
                int days = ts.Days;
                if (days < 0)
                {
                    dateerr.Text = "From Date Should Be Less Than To Date";
                    dateerr.Visible = true;
                    grdover.Visible = false;
                    
                    grdover.Visible = false;
                    btnletter.Visible = false;
                    btntamilletter.Visible = false;
                    btnxl.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnprintmaster.Visible = false;
                    btnPrint.Visible = false;
                    Printcontrol.Visible = false;
                    //pagesetpanel.Visible = false;
                    
                    Pnltamilformat.Visible = false;
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



                        dateerr.Text = "";
                        dateerr.Visible = false;
                        grdover.Visible = true;
                       
                        grdover.Visible = true;
                        btnletter.Visible = true;
                        btntamilletter.Visible = true;
                        btnxl.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnprintmaster.Visible = false;
                        btnPrint.Visible = false;
                        Printcontrol.Visible = false;
                        Pnltamilformat.Visible = false;
                        if (lblnorec.Visible == false)
                        {

                            loadheader();
                            if (lblnorec.Visible == false)
                            {
                                load_students();
                            }

                        }
                    }
                    else
                    {

                        dateerr.Text = " From Date Should Be Less Than To Date";
                        dateerr.Visible = true;
                        grdover.Visible = false;
                        
                        grdover.Visible = false;
                        btnletter.Visible = false;
                        Pnltamilformat.Visible = false;
                        btntamilletter.Visible = false;
                        btnxl.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnprintmaster.Visible = false;
                        btnPrint.Visible = false;
                        Printcontrol.Visible = false;
                        
                    }
                }

            }
        }
        // catch
        {
        }
    }
    public string GetFunction(string Att_strqueryst)
    {
        string sqlstr;
        sqlstr = Att_strqueryst;
        con.Close();
        con.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con);
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr);
        cmd.Connection = con;
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
    public void filteration()
    {

        string orderby_Setting = GetFunction("select value from master_Settings where settings='order_by'");

        if (orderby_Setting == "")
        {
            strorder = "";
            strregorder = "";
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
    public void presentdays()
    {
        //frdate = txtfromdate.Text;
        //todate = txttodate.Text;

        yesflag = true;

        persentmonthcal();
    }
    
    public void load_students()
    {
        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
        hasdaywise.Clear();
        hashrwise.Clear();
        //  try
        {
            string sec;
            if (ddlsection.Enabled == true)
            {
                if (ddlsection.SelectedItem.ToString() == string.Empty || ddlsection.Text == "All")
                {
                    sec = "";
                }
                else
                {
                    sec = ddlsection.SelectedItem.ToString();

                }
            }
            else
            {
                sec = "";
            }

            filteration();
            string filterwithsection = "exam_flag<>'debar' and delflag=0 and batch_year='" + ddlbatch.SelectedItem.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and Current_Semester='" + ddlsemester.SelectedItem.ToString() + "'  and sections='" + sec.ToString() + "'" + strorder + "";
            string filterwithoutsection = "exam_flag<>'debar' and delflag=0 and batch_year='" + ddlbatch.SelectedItem.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and Current_Semester='" + ddlsemester.SelectedItem.ToString() + "' " + strorder + "";
            hat.Clear();
            hat.Add("bath", int.Parse(ddlbatch.SelectedItem.ToString()));
            hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
            hat.Add("sec", sec.ToString());
            hat.Add("filterwithsection", filterwithsection.ToString());
            hat.Add("filterwithoutsection", filterwithoutsection.ToString());
            ds4 = d2.select_method("ALL_STUDENT_DETAILS", hat, "sp");
            // string sqlStr = "";
            string sections = "";
            string strsec = "";
            sections = ddlsection.SelectedValue.ToString();
            if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
            {
                strsec = "";
            }
            else
            {
                strsec = " and sections='" + sections.ToString() + "'";
            }

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
            }
            hat.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
            count = ds1.Tables[0].Rows.Count;

            int stu_count = ds4.Tables[0].Rows.Count;
            
            int srno = 0;
            Boolean rowflag = false;
            

            
            for (rows_count = 0; rows_count < stu_count; rows_count++)
            {
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
                    string hrdetno = "";
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




                {

                    string dum_tage_date, dum_tage_hrs;
                    string dum_cum_tage_date, dum_cum_tage_hrs;
                    per_tage_date = ((pre_present_date / per_workingdays) * 100);
                    if (per_tage_date > 100)
                    {
                        per_tage_date = 100;
                    }

                    //     per_con_hrs = (per_workingdays1 - cum_dum_unmark)+tot_conduct_hr_spl_fals;///chaged on 080812
                    //per_tage_hrs = ((per_per_hrs / per_con_hrs) * 100);

                    per_con_hrs = per_workingdays1; //added on 08.08.12//my

                    // per_con_hrs = (per_workingdays1 - cum_dum_unmark) + tot_conduct_hr_spl_fals; //hided 080812//prabha code
                    //  per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl_fals) / per_con_hrs) * 100);

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
                                    rowflag = true;
                                   

                                    srno++;
                                   

                                    dtrow = dtl.NewRow();
                                    int col = 0;

                                    dtrow[col] = (dtl.Rows.Count + 1) - 2;
                                    col++;


                                    if (Session["Rollflag"].ToString() != "0")
                                    {
                                        dtrow[col] = ds4.Tables[0].Rows[rows_count]["Roll_No"].ToString();
                                        col++;

                                    }
                                    if (Session["Regflag"].ToString() != "0")
                                    {
                                        dtrow[col] = ds4.Tables[0].Rows[rows_count]["Reg_No"].ToString();
                                        col++;

                                    }

                                    dtrow[col] = ds4.Tables[0].Rows[rows_count]["Stud_Name"].ToString();
                                    col++;


                                    if (Session["Studflag"].ToString() != "0")
                                    {
                                        dtrow[col] = ds4.Tables[0].Rows[rows_count]["Student_Type"].ToString();
                                        col++;

                                    }


                                    ViewState["temp_table"] = col;


                                    string nmnn = Convert.ToString(Session["daywise"]);
                                    if (nmnn != "")
                                    {
                                        if (Session["daywise"].ToString() == "0")
                                        {

                                        }
                                        else
                                        {
                                            dtrow[col] = (per_workingdays).ToString();
                                            col++;
                                            dtrow[col] = pre_present_date.ToString();
                                            col++;
                                            dtrow[col] = dum_tage_date.ToString();
                                            col++;

                                        }
                                    }



                                    string jkk = Convert.ToString(Session["hourwise"]);
                                    if (jkk != "")
                                    {
                                        if (Session["hourwise"].ToString() == "0")
                                        {


                                        }
                                        else
                                        {
                                            dtrow[col] = (per_con_hrs + tot_conduct_hr_spl_fals).ToString(); //per_con_hrs.ToString();
                                            col++;
                                            dtrow[col] = (per_per_hrs + tot_per_hrs_spl_fals).ToString();
                                            col++;
                                            dtrow[col] = dum_tage_hrs.ToString();
                                            col++;





                                        }


                                    }
                                    dtl.Rows.Add(dtrow);

                                }
                                else
                                {
                                    
                                    if (srno != 0)
                                    {
                                        srno--;
                                    }
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
                                       
                                        srno++;
                                       


                                        dtrow = dtl.NewRow();
                                        int col = 0;

                                        dtrow[col] = (dtl.Rows.Count + 1) - 2;
                                        col++;


                                        if (Session["Rollflag"].ToString() != "0")
                                        {
                                            dtrow[col] = ds4.Tables[0].Rows[rows_count]["Roll_No"].ToString();
                                            col++;

                                        }
                                        if (Session["Regflag"].ToString() != "0")
                                        {
                                            dtrow[col] = ds4.Tables[0].Rows[rows_count]["Reg_No"].ToString();
                                            col++;

                                        }

                                        dtrow[col] = ds4.Tables[0].Rows[rows_count]["Stud_Name"].ToString();
                                        col++;


                                        if (Session["Studflag"].ToString() != "0")
                                        {
                                            dtrow[col] = ds4.Tables[0].Rows[rows_count]["Student_Type"].ToString();
                                            col++;

                                        }


                                        ViewState["temp_table"] = col;


                                        string nmnn = Convert.ToString(Session["daywise"]);
                                        if (nmnn != "")
                                        {
                                            if (Session["daywise"].ToString() == "0")
                                            {

                                            }
                                            else
                                            {
                                                dtrow[col] = (per_workingdays).ToString();
                                                col++;
                                                dtrow[col] = pre_present_date.ToString();
                                                col++;
                                                dtrow[col] = dum_tage_date.ToString();
                                                col++;

                                            }
                                        }



                                        string jkk = Convert.ToString(Session["hourwise"]);
                                        if (jkk != "")
                                        {
                                            if (Session["hourwise"].ToString() == "0")
                                            {


                                            }
                                            else
                                            {
                                                dtrow[col] = (per_con_hrs + tot_conduct_hr_spl_fals).ToString(); //per_con_hrs.ToString();
                                                col++;
                                                dtrow[col] = (per_per_hrs + tot_per_hrs_spl_fals).ToString();
                                                col++;
                                                dtrow[col] = dum_tage_hrs.ToString();
                                                col++;





                                            }


                                        }
                                        dtl.Rows.Add(dtrow);

                                    }
                                }
                                else
                                {
                                    
                                    if (srno != 0)
                                    {
                                        srno--;
                                    }
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
                        
                        dtrow = dtl.NewRow();
                        int col = 0;

                        dtrow[col] = (dtl.Rows.Count + 1) - 2;
                        col++;


                        if (Session["Rollflag"].ToString() != "0")
                        {
                            dtrow[col] = ds4.Tables[0].Rows[rows_count]["Roll_No"].ToString();
                            col++;

                        }
                        if (Session["Regflag"].ToString() != "0")
                        {
                            dtrow[col] = ds4.Tables[0].Rows[rows_count]["Reg_No"].ToString();
                            col++;

                        }

                        dtrow[col] = ds4.Tables[0].Rows[rows_count]["Stud_Name"].ToString();
                        col++;


                        if (Session["Studflag"].ToString() != "0")
                        {
                            dtrow[col] = ds4.Tables[0].Rows[rows_count]["Student_Type"].ToString();
                            col++;

                        }


                        ViewState["temp_table"] = col;


                        string nmnn = Convert.ToString(Session["daywise"]);
                        if (nmnn != "")
                        {
                            if (Session["daywise"].ToString() == "0")
                            {

                            }
                            else
                            {
                                dtrow[col] = (per_workingdays).ToString();
                                col++;
                                dtrow[col] = pre_present_date.ToString();
                                col++;
                                dtrow[col] = dum_tage_date.ToString();
                                col++;

                            }
                        }



                        string jkk = Convert.ToString(Session["hourwise"]);
                        if (jkk != "")
                        {
                            if (Session["hourwise"].ToString() == "0")
                            {


                            }
                            else
                            {
                                dtrow[col] = (per_con_hrs + tot_conduct_hr_spl_fals).ToString(); //per_con_hrs.ToString();
                                col++;
                                dtrow[col] = (per_per_hrs + tot_per_hrs_spl_fals).ToString();
                                col++;
                                dtrow[col] = dum_tage_hrs.ToString();
                                col++;





                            }


                        }
                        dtl.Rows.Add(dtrow);

                    }
                    
                    
                    temp_reg_no = ds4.Tables[0].Rows[rows_count]["Reg_No"].ToString();
                    
                    
                    //'=============================================================================================
                    //  if (Convert.ToInt32(Session["daywise"]) == 1)
                    {
                        //if (chkonduty.Checked == false)
                        {
                            pre_present_date = pre_present_date - pre_ondu_date;
                            cum_present_date = cum_present_date - cum_ondu_date;
                        }
                        
                        
                    }

                    int setcolumn = ondutycount - 1;

                    {
                        //added by srinath 22/1/2014
                        Double absenthours = (per_con_hrs + tot_conduct_hr_spl_fals) - (per_per_hrs + tot_per_hrs_spl_fals);
                        //   if (chkonduty.Checked == false)
                        {
                            per_per_hrs = per_per_hrs - per_tot_ondu;
                            tot_per_hrs_spl_true = tot_per_hrs_spl_true - tot_ondu_spl_fals;
                        }
                        
                        //Added by srinath 31/1/2014
                        if (setcolumn > 0)
                        {
                            for (int odrow = 10; odrow < 10 + ondutycount; odrow++)
                            {
                                Double odhrval = 0;
                                string colval = grdover.HeaderRow.Cells[odrow].Text.ToString().Trim().ToLower();
                                if (hatonduty.Contains(colval))
                                {
                                    odhrval = Convert.ToDouble(GetCorrespondingKey(colval, hatonduty));
                                }
                                
                            }
                        }
                       
                        cb.AutoPostBack = true;
                    }


                    
                    {
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 15 + setcolumn].Text = cum_present_date.ToString();
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 14 + setcolumn].Text = cum_workingdays.ToString();
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 16 + setcolumn].Text = dum_cum_tage_date.ToString();
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 19 + setcolumn].Text = cum_tot_ondu.ToString();

                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 19 + setcolumn].HorizontalAlign = HorizontalAlign.Center;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 15 + setcolumn].HorizontalAlign = HorizontalAlign.Center;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 14 + setcolumn].HorizontalAlign = HorizontalAlign.Center;
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 16 + setcolumn].HorizontalAlign = HorizontalAlign.Center;
                        //Added by srinath 31/1/2014
                        //if (setcolumn > 0)
                        //{
                        //    for (int odrow = (19 + setcolumn); odrow < (19 + setcolumn + ondutycount); odrow++)
                        //    {
                        //        Double odhrval = 0;
                        //        string colval = FpSpread1.Sheets[0].ColumnHeader.Cells[2, odrow].Text.ToString();
                        //        if (hatcumonduty.Contains(colval))
                        //        {
                        //            odhrval = Convert.ToDouble(GetCorrespondingKey(colval, hatcumonduty));
                        //        }
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, odrow].Text = odhrval.ToString();
                        //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, odrow].HorizontalAlign = HorizontalAlign.Center;
                        //    }
                        //}
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 20 + (setcolumn + setcolumn)].Text = cum_tot_ml.ToString();
                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 20 + (setcolumn + setcolumn)].HorizontalAlign = HorizontalAlign.Center;
                    }

                    //   if (Convert.ToInt32(Session["hourwise"]) == 1)
                    //{
                    //added by srinath 22/1/2014
                    //    Double absenthours = (cum_con_hrs + tot_conduct_hr_spl_true) - (cum_per_perhrs + tot_per_hrs_spl_true);
                    // //   if (chkonduty.Checked == false)
                    //    {
                    //        cum_per_perhrs = cum_per_perhrs - cum_tot_ondu;
                    //    }
                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 18 + setcolumn].Text = (cum_per_perhrs + tot_per_hrs_spl_true).ToString();
                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 17 + setcolumn].Text = (cum_con_hrs + tot_conduct_hr_spl_true).ToString(); //cum_con_hrs.ToString();

                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 18 + setcolumn].HorizontalAlign = HorizontalAlign.Center;
                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 17 + setcolumn].HorizontalAlign = HorizontalAlign.Center;
                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 22 + (setcolumn + setcolumn)].HorizontalAlign = HorizontalAlign.Center;
                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 21 + (setcolumn + setcolumn)].HorizontalAlign = HorizontalAlign.Center;

                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 22 + (setcolumn + setcolumn)].Text = dum_cum_tage_hrs.ToString();
                    //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 21 + (setcolumn + setcolumn)].Text = absenthours.ToString();
                    //}
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 23 + (setcolumn + setcolumn)].Text = per_absent_date.ToString();
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 25 + (setcolumn + setcolumn)].Text = pre_leave_date.ToString();
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 26 + (setcolumn + setcolumn)].Text = pre_ondu_date.ToString();
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 24 + (setcolumn + setcolumn)].Text = cum_absent_date.ToString();

                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 23 + (setcolumn + setcolumn)].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 24 + (setcolumn + setcolumn)].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 25 + (setcolumn + setcolumn)].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 26 + (setcolumn + setcolumn)].HorizontalAlign = HorizontalAlign.Center;


                    //if (Session["hourwise"].ToString() == "0")
                    //{
                    //FpSpread1.Sheets[0].ColumnHeader.Columns[8].Visible = false;
                    //FpSpread1.Sheets[0].ColumnHeader.Columns[9].Visible = false;
                    //FpSpread1.Sheets[0].ColumnHeader.Columns[10].Visible = false;
                    //FpSpread1.Sheets[0].ColumnHeader.Columns[11].Visible = false;
                    //FpSpread1.Sheets[0].ColumnHeader.Columns[12].Visible = false;

                    //FpSpread1.Sheets[0].ColumnHeader.Columns[16].Visible = false;
                    //FpSpread1.Sheets[0].ColumnHeader.Columns[17].Visible = false;
                    //FpSpread1.Sheets[0].ColumnHeader.Columns[18].Visible = false;
                    //FpSpread1.Sheets[0].ColumnHeader.Columns[19].Visible = false;
                    //FpSpread1.Sheets[0].ColumnHeader.Columns[20].Visible = false;

                    //}
                    //if (Session["daywise"].ToString() == "0")
                    //{
                    //FpSpread1.Sheets[0].ColumnHeader.Columns[5].Visible = false;
                    //FpSpread1.Sheets[0].ColumnHeader.Columns[6].Visible = false;
                    //FpSpread1.Sheets[0].ColumnHeader.Columns[7].Visible = false;

                    //FpSpread1.Sheets[0].ColumnHeader.Columns[13].Visible = false;
                    //FpSpread1.Sheets[0].ColumnHeader.Columns[14].Visible = false;
                    //FpSpread1.Sheets[0].ColumnHeader.Columns[15].Visible = false;


                    //}
                    //'========================================================================================

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
                    if (Convert.ToInt32(grdover.Rows.Count) == 0)
                    {
                        lblnorec.Visible = true;
                        grdover.Visible = true;
                        
                        grdover.Visible = true;
                        btnletter.Visible = true;
                        btntamilletter.Visible = true;
                        btnxl.Visible = true;
                        lblrptname.Visible = true;
                        txtexcelname.Visible = true;
                        btnprintmaster.Visible = true;
                        btnPrint.Visible = true;
                        Printcontrol.Visible = false;
                        Pnltamilformat.Visible = false;
                    }
                    else
                    {
                        grdover.Visible = true;
                        
                        grdover.Visible = true;
                        btnletter.Visible = true;
                        Pnltamilformat.Visible = false;
                        btntamilletter.Visible = true;
                        btnxl.Visible = true;
                        lblrptname.Visible = true;
                        txtexcelname.Visible = true;
                        lblnorec.Visible = false;
                        btnprintmaster.Visible = true;
                        btnPrint.Visible = true;
                        Printcontrol.Visible = false;

                        Double totalRows = 0;
                        totalRows = Convert.ToInt32(grdover.Rows.Count);

                        if (totalRows >= 10)
                        {
                            
                        }
                        else if (totalRows == 0)
                        {
                           
                        }
                        else
                        {
                            
                        }
                        totalRows = Convert.ToInt32(grdover.Rows.Count);
                        Session["totalPages"] = (int)Math.Ceiling(totalRows / grdover.Rows.Count);
                    }

                }
            }


            grdover.DataSource = dtl;
            grdover.DataBind();
            grdover.HeaderRow.Visible = false;

            int tempt = Convert.ToInt32(ViewState["temp_table"]);
            int ccc = tempt + 1;
            for (int i = 0; i < grdover.Rows.Count; i++)
            {
                for (int j = 0; j < grdover.HeaderRow.Cells.Count; j++)
                {

                    if (i == 0 || i == 1)
                    {
                        grdover.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                        grdover.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        grdover.Rows[i].Cells[j].BorderColor = Color.Black;
                        grdover.Rows[i].Cells[j].Font.Bold = true;

                        if (i == 0)
                        {
                            if (j == 0)
                            {
                                var checkbox1 = grdover.Rows[i].Cells[0].FindControl("lbl_cb") as CheckBox;
                                checkbox1.Visible = false;

                            }
                            if (j < tempt+1)
                            {
                                grdover.Rows[i].Cells[j].RowSpan = 2;
                                for (int a = i; a < 1; a++)
                                    grdover.Rows[a + 1].Cells[j].Visible = false;
                            }
                            else if (ccc == j )
                            {
                                grdover.Rows[i].Cells[j].ColumnSpan = 3;
                                for (int a = j + 1; a < j + 3; a++)
                                    grdover.Rows[i].Cells[a].Visible = false;

                                ccc += 3;
                            }

                        }
                    }
                    else
                    {
                        if (j == 0)
                        {
                            var checkbox1 = grdover.Rows[i].Cells[0].FindControl("chkselectall") as CheckBox;
                            checkbox1.Visible = false;

                        }
                        if (grdover.HeaderRow.Cells[j].Text == "Roll No" || grdover.HeaderRow.Cells[j].Text == "Register No" || grdover.HeaderRow.Cells[j].Text == "Name")
                            grdover.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;

                        else
                            grdover.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;

                    }
                }

            }

            if (grdover.Rows.Count == 0 || rowflag == false)
            {
                grdover.Visible = false;
                
                grdover.Visible = false;
                btnletter.Visible = false;
                Pnltamilformat.Visible = false;
                btntamilletter.Visible = false;
                btnxl.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                lblnorec.Text = "No Record(s) Found";
                lblnorec.Visible = true;
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                Printcontrol.Visible = false;
            }
            int m = 0;
            

        }
        // catch
        {
        }
    }
    public void loadheader()
    {
        
        

        //added by rajasekar 27/08/2018


        dtrow = dtl.NewRow();
        dtl.Rows.Add(dtrow);

        dtrow = dtl.NewRow();
        dtl.Rows.Add(dtrow);
        int colu = 0;

        dtl.Columns.Add("S.No", typeof(string));
        dtl.Rows[0][colu] = "S.No";
        colu++;
        
        if (Session["Rollflag"].ToString() != "0")
        {
            dtl.Columns.Add("Roll No", typeof(string));
            dtl.Rows[0][colu] = "Roll No";
            colu++;
        }
        if (Session["Regflag"].ToString() != "0")
        {
            
            dtl.Columns.Add("Register No", typeof(string));
            dtl.Rows[0][colu] = "Register No";
            colu++;
        }

        dtl.Columns.Add("Name", typeof(string));
        dtl.Rows[0][colu] = "Name";
        colu++;
        
        if (Session["Studflag"].ToString() != "0")
        {
            
            dtl.Columns.Add("StudentType", typeof(string));
            dtl.Rows[0][colu] = "StudentType";
            colu++;
        }

        



        string nmnn = Convert.ToString(Session["daywise"]);
        if (nmnn != "")
        {
            if (Session["daywise"].ToString() == "0")
            {
                
            }
            else
            {
                dtl.Columns.Add("Conducted Days", typeof(string));
                dtl.Rows[0][colu] = "Day Wise Percentage From " + txtfromdate.Text + " To " + txttodate.Text;
                dtl.Rows[1][colu] = "Conducted Days";
                colu++;
                dtl.Columns.Add("Attended Days", typeof(string));
                dtl.Rows[1][colu] = "Attended Days";
                colu++;
                dtl.Columns.Add("Att Day Percentage", typeof(string));
                dtl.Rows[1][colu] = "Att Day Percentage";
                colu++;

               
            }
        }



        string jkk = Convert.ToString(Session["hourwise"]);
        if (jkk != "")
        {
            if (Session["hourwise"].ToString() == "0")
            {
                

            }
            else
            {
                dtl.Columns.Add("Conducted Periods", typeof(string));
                dtl.Rows[0][colu] = "Period Wise Percentage From " + txtfromdate.Text + " To " + txttodate.Text;
                dtl.Rows[1][colu] = "Conducted Periods";
                colu++;
                dtl.Columns.Add("Attended Periods", typeof(string));
                dtl.Rows[1][colu] = "Attended Periods";
                colu++;
                dtl.Columns.Add("Att Period Percentage", typeof(string));
                dtl.Rows[1][colu] = "Att Period Percentage";
                colu++;


            }
            
            

        }

        //=======================================//

    }
    public void function_btnclick()
    {
        //  try
        {
            dateerr.Visible = false;



            first_btngo();


            //  if (Request.QueryString["val"] == null)
            {

                if (grdover.Rows.Count > 0)
                {
                    
                    //-------------------------------------------------------------------------------------------------
                    //load_pageddl();
                }
                else
                {
                    grdover.Visible = false;
                   
                    grdover.Visible = false;
                    btnletter.Visible = false;
                    Pnltamilformat.Visible = false;
                    btntamilletter.Visible = false;
                    btnprintmaster.Visible = false;
                    btnPrint.Visible = false;
                    Printcontrol.Visible = false;
                    // pagesetpanel.Visible = false;
                    if (dateerr.Visible != true)
                    {
                        lblnorec.Text = "No Record(s) Found";
                        lblnorec.Visible = true;
                    }
                }

                if (grdover.Columns.Count > 0)
                {
                   
                }
                else
                {
                    grdover.Visible = false;
                    
                    grdover.Visible = false;
                    btnletter.Visible = false;
                    Pnltamilformat.Visible = false;
                    btntamilletter.Visible = false;
                    btnprintmaster.Visible = false;
                    btnPrint.Visible = false;
                    Printcontrol.Visible = false;
                    if (dateerr.Visible != true)
                    {
                        lblnorec.Text = "No Record(s) Found";
                        lblnorec.Visible = true;
                    }
                    //pagesetpanel.Visible = false;
                }
            }
            inirow_count = grdover.Rows.Count;
        }
        //   catch
        {
        }
    }
    //added by rajasekar 28/08/2018
    
    //==================================//
    protected void btngo_Click(object sender, EventArgs e)
    {

        btnPrint11();
        btnletter.Visible = false;
        Pnltamilformat.Visible = false;
        btntamilletter.Visible = false;
        txtexcelname.Text = "";
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
    protected void txtfromrange_TextChanged(object sender, EventArgs e)
    {
        try
        {
            grdover.Visible = false;
            
            grdover.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            btnletter.Visible = false;
            Pnltamilformat.Visible = false;
            btntamilletter.Visible = false;
            lblnorec.Visible = false;
            if (txtfromrange.Text.ToString().Trim() != "")
            {
                int frange = Convert.ToInt32(txtfromrange.Text.ToString());
                if (frange > 100)
                {
                    txtfromrange.Text = "";
                    lblnorec.Visible = true;
                    lblnorec.Text = "Please Enter Lesser than equal to 100";

                }
                if (txttorange.Text.ToString().Trim() != "")
                {
                    int trange = Convert.ToInt32(txttorange.Text.ToString());

                    if (frange > trange)
                    {
                        txtfromrange.Text = "";
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
    public void getyear()
    {
        if (yr_val != "")
        {
            if (yr_val == "1" || yr_val == "2")
            {
                yr_string = "First Year";
            }
            if (yr_val == "3" || yr_val == "4")
            {
                yr_string = "Second Year";
            }
            if (yr_val == "5" || yr_val == "6")
            {
                yr_string = "Third Year";
            }
            if (yr_val == "7" || yr_val == "8")
            {
                yr_string = "Fourth Year";
            }
            if (yr_val == "9" || yr_val == "10")
            {
                yr_string = "Fifth Year";
            }
            if (yr_val == "11" || yr_val == "12")
            {
                yr_string = "Sixth Year";
            }
        }
    }


    //added by rajasekar 12/07/2018
    protected void btntamilletter_Click(object sender, EventArgs e)
    {

        Pnltamilformat.Visible = true;
        
    }

    protected void btntamilprint_Click(object sender, EventArgs e)
    {
        
        try
        {
            lblsave.Visible = false;
            contentDiv.InnerHtml = ""; StringBuilder html = new StringBuilder();
            DataSet ds22 = new DataSet();
            DAccess2 da22 = new DAccess2();
            int lp = 0;
            string odd_r_even = "";
            string registernumber = "";
            string year = "";
            sem = ddlsemester.SelectedValue.ToString();

            sec = "";
            if (ddlsection.Items.Count > 0)
            {
                if ((ddlsection.Enabled == true && ddlsection.Text != "-1"))
                {
                    sec = ddlsection.SelectedValue;
                }
            }
            odd_r_even = Convert.ToString(int.Parse(sem) % 2);
            if (odd_r_even == "0")
                odd_r_even = "EVEN";
            else
                odd_r_even = "ODD";
            if (sem == "1")
            {
                gsem3 = "I";
                year = "I";
            }
            else if (sem == "2")
            {
                gsem3 = "II";
                year = "I";
            }
            else if (sem == "3")
            {
                gsem3 = "III";
                year = "II";
            }
            else if (sem == "4")
            {
                gsem3 = "IV";
                year = "II";
            }
            else if (sem == "5")
            {
                gsem3 = "V";
                year = "III";
            }
            else if (sem == "6")
            {
                gsem3 = "VI";
                year = "III";
            }
            else if (sem == "7")
            {
                gsem3 = "VII";
                year = "IV";
            }
            else if (sem == "8")
            {
                gsem3 = "VIII";
                year = "IV";
            }
            else if (sem == "9")
            {
                gsem3 = "IX";
                year = "V";
            }
            else if (sem == "10")
            {
                gsem3 = "X";
                year = "V";
            }
            string acr = "";
            string collcode = string.Empty;
            string phonenum = string.Empty;
            string examstartdate = "";
            string examenddate = "";
            con.Close();
            con.Open();
            
            int pagerowcount = 0;
            int bindpagerowcount = 0;
            string deptacr = GetFunction("select Acronym from Degree where Degree_Code='" + ddlbranch.SelectedValue.ToString() + "'");
            if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
            {
                string strquery = "Select * from Collinfo where college_code=" + Convert.ToString(Session["collegecode"]) + "";
                strquery += " select linkvalue from New_InsSettings where LinkName='TC_SerialNoSettings' and college_code='" + Convert.ToString(Session["collegecode"]) + "'";
                ds11.Clear();
                ds11 = d22.select_method_wo_parameter(strquery, "Text");
                string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo,district,college_code,phoneno,acr from collinfo where college_code=" + Session["collegecode"] + "";
                SqlCommand collegecmd = new SqlCommand(college, con);
                SqlDataReader collegename;
                con.Close();
                con.Open();
                collegename = collegecmd.ExecuteReader();
                if (collegename.HasRows)
                {
                    while (collegename.Read())
                    {
                        collnamenew1 = collegename["collname"].ToString();
                        address1 = collegename["address1"].ToString();
                        address2 = collegename["address2"].ToString();
                        address3 = collegename["address3"].ToString();
                        pincode = collegename["pincode"].ToString();
                        district = collegename["district"].ToString();
                        collcode = collegename["college_code"].ToString();
                        phonenum = collegename["phoneno"].ToString();
                        acr = Convert.ToString(collegename["acr"]);
                    }
                }
                con.Close();
            }
            yr_val = ddlsemester.SelectedItem.ToString();
            getyear();
            int cycleflag = 0;
            bool studflag = false;
            int colsetdays = 0, colsetperiods = 0, colsetroll = 0, colsetreg = 0, colsetname = 0;

            for (int a = 0; a < grdover.HeaderRow.Cells.Count; a++)
            {
                if (grdover.HeaderRow.Cells[a].Text == "Conducted Days")
                    colsetdays = a;
                else if (grdover.HeaderRow.Cells[a].Text == "Conducted Periods")
                    colsetperiods = a;
                else if (grdover.HeaderRow.Cells[a].Text == "Roll No")
                    colsetroll = a;
                else if (grdover.HeaderRow.Cells[a].Text == "Register No")
                    colsetreg = a;
                else if (grdover.HeaderRow.Cells[a].Text == "Name")
                    colsetname = a;
            }

            for (res = 3; res < Convert.ToInt32(grdover.Rows.Count + 1); res++)
            {
                isval = 0;
                //isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 11].Value);
                var checkbox = grdover.Rows[res - 1].Cells[0].FindControl("lbl_cb") as CheckBox;
                if (checkbox.Checked)
                    isval = 1;
                int status = 0;
                if (isval == 1)
                {
                    studflag = true;
                    if (grdover.Rows[res - 1].Visible == true)
                    {
                        lp = 1;
                        string contuct = "";
                        string attend = "";
                        string percen = "";
                        string year1 = txtfromdate.Text.ToString();
                        string[] split1 = year1.Split(new char[] { '/' });
                        string year2 = split1[2].ToString();

                        string roll = grdover.Rows[res - 1].Cells[colsetroll].Text;
                        string reg = grdover.Rows[res - 1].Cells[colsetreg].Text;
                        string name = grdover.Rows[res - 1].Cells[colsetname].Text;
                        
                        sem = ddlsemester.SelectedItem.Text + " & " + ddlbranch.SelectedItem.Text;
                        string class1 = ddldegree.SelectedItem.Text + " - " + ddlbranch.SelectedItem.Text;
                        if (Session["daywise"].ToString() != "0")
                        {
                            if (colsetdays != 0)
                            {
                                contuct = grdover.Rows[res - 1].Cells[colsetdays].Text;
                                attend = grdover.Rows[res - 1].Cells[colsetdays + 1].Text;
                                percen = grdover.Rows[res - 1].Cells[colsetdays + 2].Text;
                            }
                            
                        }
                        if (Session["hourwise"].ToString() != "0")
                        {
                            if (colsetperiods != 0)
                            {
                                contuct = grdover.Rows[res - 1].Cells[colsetperiods].Text;
                                attend = grdover.Rows[res - 1].Cells[colsetperiods + 1].Text;
                                percen = grdover.Rows[res - 1].Cells[colsetperiods + 2].Text;
                            }
                        }
                        string enddate = d2.GetFunction("select  CONVERT(VARCHAR(2),DATEPART(DAY, end_date))+'-'+ CONVERT(VARCHAR(2),DATEPART(MONTH, end_date)) +'-'+ CONVERT(VARCHAR(4),DATEPART(YEAR, end_date))as exam_date from seminfo where degree_code=" + ddlbranch.SelectedValue + " and semester=" + ddlsemester.SelectedItem.Text + " and batch_year=" + ddlbatch.SelectedItem.Text + "");
                        string dob = d2.GetFunction("select CONVERT(VARCHAR(2),DATEPART(DAY, dob))+'-'+ CONVERT(VARCHAR(2),DATEPART(MONTH, dob)) +'-'+ CONVERT(VARCHAR(4),DATEPART(YEAR, dob))as exam_date from applyn a,Registration r where r.App_No=a.app_no and r.Roll_No='" + roll + "' and r.college_code=" + Session["collegecode"].ToString() + "");

                        string photo = "";
                        byte[] photoid = new byte[0];
                        if (ds11.Tables[0].Rows.Count > 0)
                        {
                            if (ds11.Tables[0].Rows[0]["logo1"] != null && Convert.ToString(ds11.Tables[0].Rows[0]["logo1"]) != "")
                            {
                                photoid = (byte[])(ds11.Tables[0].Rows[0]["logo1"]);
                                if (photoid.Length > 0)
                                {
                                    photo = "'data:image/png;base64," + Convert.ToBase64String(photoid) + "'";
                                }
                            }
                        }

                        html.Append("<div style='height: 1200px; width: 700px; border: 0px solid black; margin-left: 5px;margin:0px;page-break-after: always;'>");

                        html.Append("<table cellspacing='0' cellpadding='0' style='width: 700px;' border='0'>");

                        html.Append("<tr><td style='width: 100px;'></td><td style='text-align: right;' > <img src=" + photo + " alt='' style='height: 100px; width: 120px;' /></td><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: center;'><span style='font-size: Large;font-weight:bold;'>" + collnamenew1 + "</span> <br><span style='font-size: medium;'> Institution code:" + acr + "<br>" + address1 + " , " + district + " , - " + pincode + ",<br>" + phonenum + " </span></td></tr> ");

                        html.Append(" </table><hr style='width: 700px;margin-left: 125px;'>");


                        html.Append("<table cellspacing='0' cellpadding='0' style='width: 700px; margin-left: 125px;' border='0' >");
                        html.Append("<tr><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: left;'>அன்பார்ந்த அய்யா/அம்மையீர்: </td><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: left;'>Date:</td></tr>");

                        html.Append(" </table><br>");

                        html.Append("<table cellspacing='0' cellpadding='0' style='width: 700px; margin-left: 125px;' border='0'>");
                        html.Append("<tr><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align:  justify;'>வணக்கம், தாங்கள் மகன்/மகள் " + year2 + " ஆம் ஆண்டில் " + class1 + " வகுப்பில் " + collnamenew1 + " யில் பயின்று வருகிறார். கல்லூரி தொடங்கிய நாள் முதல் இன்று வரை கல்லூரிக்கு சரியாக வருகை புரியவில்லை. இப்பயிலகத்தில் பயின்று வரும் தங்கள் மகன்/மகள் வருகைப்பதிவு குறைந்தபட்ச தேவையான 80% விழுக்காட்டிற்கும் குறைவாக உள்ளது என்பதை தெரிவிதுக்கொள்கிறோம்.</td></tr>");

                        html.Append(" </table><br>");

                        html.Append("<table cellspacing='0' cellpadding='0' style='width: 700px; margin-left: 125px;' border='0'>");

                        html.Append("<tr><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: center;'> மாணவா்/மாணவி வருகைப்பதிவு கடிதம் <td>");


                        html.Append(" </table><br>");

                        html.Append("<table cellspacing='0' cellpadding='0' style='width: 700px; margin-left: 125px;' border='0'>");

                        html.Append("<tr><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: left;'><span style='font-size: 12px;'><br>பயிலக எண்</td><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: left;'>: " + acr + "</td></tr><tr><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: left;'><span style='font-size: 12px;'><br>மாணவா்/மாணவி பெயர்</td><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: left;'>: " + name + "</td></tr><tr><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: left;'><span style='font-size: 12px;'><br>தேர்வு பதிவு எண்</td><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: left;'>: " + reg + "</td></tr></tr><tr><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: left;'><span style='font-size: 12px;'><br>சுழல் எண்</td><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: left;'>: " + roll + "</td></tr><tr><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: left;'><span style='font-size: 12px;'><br>பருவம் மற்றும் பாடப்பிரிவு</td><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: left;'>: " + sem + "</td></tr><tr><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: left;'><span style='font-size: 12px;'><br>கணக்கிடப்பட்ட காலம்</td><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: left;'>: " + txtfromdate.Text + " லிருந்து " + txttodate.Text + " வரை </td></tr>");

                        html.Append(" </table> <br>");


                        html.Append("<table style='width: 95%; margin-left: 125px; margin-top: 0px; margin-bottom: 2px; font-size: medium;'cellpadding='5' cellspacing='0'>");

                        html.Append("<tr><td style='border: thin solid #000000;' align='center'  class='style1'><span style='font-size: 12px;'>நடைபெற்ற மொத்த<br><வகுப்புகள்/நாட்கள்></span></td><td style='border: thin solid #000000;' align='center'  class='style1'><span style='font-size: 12px;'>மாணவா் வருகை தந்த<br><வகுப்புகள்/நாட்கள்></span></td><td style='border: thin solid #000000;' align='center'  class='style1'><span style='font-size: 12px;'>விழுக்காடு</span></td></tr><tr><td style='border: thin solid #000000;' align='center'  class='style1'><span style='font-size: 12px;'>" + contuct + "</span></td><td style='border: thin solid #000000;' align='center'  class='style1'><span style='font-size: 12px;'>" + attend + "</span></td><td style='border: thin solid #000000;' align='center'  class='style1'><span style='font-size: 12px;'>" + percen + "</span></td></tr></table>");



                        html.Append("<table cellspacing='0' cellpadding='0' style='width: 700px; margin-left: 125px;' border='0'>");

                        html.Append("<tr><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: left;'><span style='font-size: 12px;'> கடைசி வேலை நாள் <br> (உத்தேச  அட்டவணையின் படி) <br><br><span style='font-size: 12px;'>மேற்படி மாணவரின் வருகைப்பதிவு இப்பருவத்தின் கடைசி வேலை நாளன்று 80 விழுக்காடிற்கு குறைவாக இருக்குமேயானால்</span><br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<span style='font-size: 26px; font-weight:bold;'>.</span><span style='font-size: 12px;'>&nbsp&nbsp&nbsp&nbsp மாணவா் வாரியத்தேர்வு எழுத அனுமதிக்கப்படமாட்டார். </span><br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<span style='font-size: 26px; font-weight:bold;'>.</span><span style='font-size: 12px;'>&nbsp&nbsp&nbsp&nbsp அரசின் கல்வி உதவித்தொகையும்  பெற முடியாது.</span><br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<span style='font-size: 26px; font-weight:bold;'>.</span><span style='font-size: 12px;'>&nbsp&nbsp&nbsp&nbsp 80% க்கு குறைவான வருகைப்பதிவு உள்ளவர்கள் தங்கள் மகனுடன் இக்கடிதம் கண்டவுடன் </span><br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</span><span style='font-size: 12px;'>&nbsp&nbsp&nbsp&nbsp துறைத் தலைவர்/முதல்வரை நேரில் சந்திக்கவும். தொலைபேசியில் தொடர்பு கொள்வதை தவிர்க்கவும். </span>");

                        string strquery1 = "Select * from note where college_code=" + Convert.ToString(Session["collegecode"]) + " and letter_name='" + btntamilletter.Text +"'";

                        ds22.Clear();
                        ds22 = da22.select_method_wo_parameter(strquery1, "Text");
                        if (ds22.Tables[0].Rows.Count > 0)
                        {

                            html.Append("<br>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<span style='font-size: 26px; font-weight:bold;'>.</span><span style='font-size: 12px;'>&nbsp&nbsp&nbsp&nbsp " + Convert.ToString(ds22.Tables[0].Rows[0]["note"]) +"</span>");


                        }



                        html.Append("<br><br><span style='font-size: 12px;'>குறிப்பு: </span></td>");

                        html.Append(" </table>");

                        html.Append("<br><br><br><br><br><br><table cellspacing='0' cellpadding='0' style='width: 700px; margin-left: 125px;' border='0'>");
                        html.Append("<tr><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: left;'> வகுப்பு ஆசிரியர்</td><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: left;'> துறைத்தலைவர்</td><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: left;'> முதல்வர்</td></tr>");

                        html.Append(" </table>");

                        html.Append("</div>");

                        html.Append("<br>");
                        contentDiv.InnerHtml = html.ToString();
                        contentDiv.Visible = true;

                        ScriptManager.RegisterStartupScript(this, GetType(), "btn_print", "PrintDiv();", true);

                        

                    }
                }
            }//
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnnotesave_Click(object sender, EventArgs e)
    {
        try
        {
            lblsave.Visible = false;
            DAccess2 dacces2 = new DAccess2();
            string querystu1 = " if exists (select * from note where college_code ='" + Session["collegecode"] + "' and letter_name='" + btntamilletter.Text + "') update note set note=N'" + txttamilnote.Text + "'  where college_code ='" + Session["collegecode"] + "' and letter_name='" + btntamilletter.Text + "' else insert into note (note,letter_name,college_code)  values (N'" + txttamilnote.Text + "','" + btntamilletter.Text + "','" + Session["collegecode"] + "')";
            int saveupdate = dacces2.update_method_wo_parameter(querystu1, "Text");
            if (saveupdate == 1)
            {
                lblsave.Visible = true;
                lblsave.Text = "saved successfully";
                txttamilnote.Text = "";
            }
        }
        catch (Exception ex)
        {

        }
    }

    //=======================================//

    protected void btnletter_Click(object sender, EventArgs e)
    {
        try
        {
            string websitelink = d2.GetFunctionv("select value from Master_Settings where  settings='Letter Web Site'");
            int lp = 0;
            string collnamenew1 = "";
            string address1 = "";
            string address2 = "";
            string address = "";
            string address3 = "";
            string pincode = "";
            string Phoneno = "";
            string Faxno = "";
            string phnfax = "";
            string acr = "";
            int isval = 0;
            string nm = Convert.ToString(grdover.Rows.Count + 1);
          
            Font Fontbold = new Font("Book Antiqua", 17, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font Fontmedium = new Font("Book Antiqua", 15, FontStyle.Regular);
            Font Fontbold1 = new Font("Book Antiqua", 15, FontStyle.Bold);

            Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
            {
                string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo,acr from collinfo where college_code=" + Session["collegecode"] + "";
                SqlCommand collegecmd = new SqlCommand(college, con);
                SqlDataReader collegename;
                con.Close();
                con.Open();
                collegename = collegecmd.ExecuteReader();
                if (collegename.HasRows)
                {
                    while (collegename.Read())
                    {
                        collnamenew1 = collegename["collname"].ToString();
                        address1 = collegename["address1"].ToString();
                        address2 = collegename["address2"].ToString();
                        address3 = collegename["address3"].ToString();
                        pincode = collegename["pincode"].ToString();
                        address = address1 + ", " + " " + address3 + "-" + " " + pincode + ".";
                        acr = Convert.ToString(collegename["acr"]);
                    }
                }
                con.Close();
            }
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);

            int colsetdays=0,colsetperiods=0,colsetroll=0,colsetreg=0,colsetname=0;

            for(int a=0;a<grdover.HeaderRow.Cells.Count;a++)
            {
                if(grdover.HeaderRow.Cells[a].Text=="Conducted Days")
                    colsetdays=a;
                else if(grdover.HeaderRow.Cells[a].Text=="Conducted Periods")
                    colsetperiods=a;
                else if (grdover.HeaderRow.Cells[a].Text == "Roll No")
                    colsetroll = a;
                else if (grdover.HeaderRow.Cells[a].Text == "Register No")
                    colsetreg = a;
                else if (grdover.HeaderRow.Cells[a].Text == "Name")
                    colsetname = a;
            }


            for (int res = 3; res < Convert.ToInt32(grdover.Rows.Count + 1); res++)
            {
                isval = 0;
                //isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 11].Value);

                var checkbox = grdover.Rows[res - 1].Cells[0].FindControl("lbl_cb") as CheckBox;
                if (checkbox.Checked)
                    isval = 1;


                if (isval == 1)
                {
                    if (grdover.Rows[res - 1].Visible == true)
                    {
                        
                        lp = 1;
                        string contuct = "";
                        string attend = "";
                        string percen = "";
                        string roll = grdover.Rows[res - 1].Cells[colsetroll].Text;
                        string reg = grdover.Rows[res - 1].Cells[colsetreg].Text;
                        string name = grdover.Rows[res - 1].Cells[colsetname].Text;
                        string sem = ddlsemester.SelectedItem.Text + " & " + ddlbranch.SelectedItem.Text;
                        if (Session["daywise"].ToString() != "0")
                        {
                            

                            if (colsetdays != 0)
                            {
                                contuct = grdover.Rows[res - 1].Cells[colsetdays].Text;
                                attend = grdover.Rows[res - 1].Cells[colsetdays + 1].Text;
                                percen = grdover.Rows[res - 1].Cells[colsetdays + 2].Text;
                            }
                        }
                        if (Session["hourwise"].ToString() != "0")
                        {
                            
                            if (colsetperiods != 0)
                            {
                                contuct = grdover.Rows[res - 1].Cells[colsetperiods].Text;
                                attend = grdover.Rows[res - 1].Cells[colsetperiods + 1].Text;
                                percen = grdover.Rows[res - 1].Cells[colsetperiods + 2].Text;
                            }
                        }
                        string enddate = d2.GetFunction("select  CONVERT(VARCHAR(2),DATEPART(DAY, end_date))+'-'+ CONVERT(VARCHAR(2),DATEPART(MONTH, end_date)) +'-'+ CONVERT(VARCHAR(4),DATEPART(YEAR, end_date))as exam_date from seminfo where degree_code=" + ddlbranch.SelectedValue + " and semester=" + ddlsemester.SelectedItem.Text + " and batch_year=" + ddlbatch.SelectedItem.Text + "");
                        string dob = d2.GetFunction("select CONVERT(VARCHAR(2),DATEPART(DAY, dob))+'-'+ CONVERT(VARCHAR(2),DATEPART(MONTH, dob)) +'-'+ CONVERT(VARCHAR(4),DATEPART(YEAR, dob))as exam_date from applyn a,Registration r where r.App_No=a.app_no and r.Roll_No='" + roll + "' and r.college_code=" + Session["collegecode"].ToString() + "");
                        Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                        //PdfArea tete = new PdfArea(mydoc, 10, 10, 570, 820);
                        //PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);
                        //mypdfpage.Add(pr1);
                        //PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                        //                                new PdfArea(mydoc, 100, 40, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, collnamenew1);
                        //PdfTextArea pts = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                        //                                   new PdfArea(mydoc, 100, 70, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, address1 + "  " + address2);
                        //PdfTextArea ptsp = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                        //                                   new PdfArea(mydoc, 100, 90, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, address3 + "  " + pincode);
                        //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))//Aruna
                        //{
                        //    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        //    mypdfpage.Add(LogoImage, 25, 25, 300);
                        //}
                        PdfTextArea pt1 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, 20, 120, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Sir/Madam,");
                        PdfTextArea pt2 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 20, 160, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "The attendance percentage of your son / daughter / ward studying in this institution is below 80%,as detailed below:");
                        PdfTextArea pt3 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 20, 200, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Institution Code          : " + acr + "");
                        PdfTextArea pt4 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, 20, 220, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Reg.No                       : " + reg + "");
                        PdfTextArea pt5 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, 20, 240, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name of The Student : " + name + "");
                        PdfTextArea pt6 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
                                   new PdfArea(mydoc, 20, 260, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Semester & Branch    : " + sem + "");
                        PdfTextArea pt7 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
                                new PdfArea(mydoc, 20, 280, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date of Birth               : " + dob + "");
                        PdfTextArea pt8 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
                               new PdfArea(mydoc, 20, 300, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Period of Calculation  :" + txtfromdate.Text + " to " + txttodate.Text + "");
                        PdfTextArea pt9 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
                               new PdfArea(mydoc, 20, 330, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Total Number of Hours : " + contuct + "");
                        PdfTextArea pt10 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
                              new PdfArea(mydoc, 320, 330, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Hours Attended by the Student : " + attend + "");
                        PdfTextArea pt11 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
                             new PdfArea(mydoc, 20, 360, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Percentage                   : ");
                        PdfTextArea pt121 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                         new PdfArea(mydoc, 180, 360, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, " " + percen + "");
                        PdfTextArea pt12 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
                 new PdfArea(mydoc, 20, 390, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Last Working Day         : " + enddate + "                (as per tentative schedule)");
                        PdfTextArea pt13 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
                            new PdfArea(mydoc, 20, 420, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "If the student fails to secure 80% attendance on the last working day:");

                        PdfTextArea pt14 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
                       new PdfArea(mydoc, 20, 450, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "He / She will not be allowed to write the Board Examination as per the regulations ,");
                        PdfTextArea pt15 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
                      new PdfArea(mydoc, 20, 480, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "He / She  has to redo  the present semester by seeking re-admission  in  the next");
                        PdfTextArea pt16 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
                      new PdfArea(mydoc, 20, 510, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "academic year(within the stipulated maximum period) .");
                        PdfTextArea pt17 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
                  new PdfArea(mydoc, 20, 540, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "You  can also monitor the attendance of your student  by visiting e - Governance");
                        PdfTextArea pt18 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
                            //  new PdfArea(mydoc, 20, 570, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "portal at http://www.tndte.gov.in");
          new PdfArea(mydoc, 20, 570, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "portal at " + websitelink);

                        PdfTextArea pt19 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
    new PdfArea(mydoc, 20, 660, 150, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the HOD");

                        PdfTextArea pt20 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
    new PdfArea(mydoc, 20, 690, 150, 30), System.Drawing.ContentAlignment.MiddleLeft, "with Date");
                        PdfTextArea pt21 = new PdfTextArea(Fontmedium, System.Drawing.Color.Black,
    new PdfArea(mydoc, 330, 660, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Principal");
                        //mypdfpage.Add(ptc);
                        //mypdfpage.Add(pts);
                        //mypdfpage.Add(ptsp);
                        mypdfpage.Add(pt1);
                        mypdfpage.Add(pt2);
                        mypdfpage.Add(pt3);
                        mypdfpage.Add(pt4);
                        mypdfpage.Add(pt5);
                        mypdfpage.Add(pt6);
                        mypdfpage.Add(pt7);
                        mypdfpage.Add(pt8);
                        mypdfpage.Add(pt9);
                        mypdfpage.Add(pt10);
                        mypdfpage.Add(pt11);
                        mypdfpage.Add(pt121);
                        mypdfpage.Add(pt12);
                        mypdfpage.Add(pt13);
                        mypdfpage.Add(pt14);
                        mypdfpage.Add(pt15);
                        mypdfpage.Add(pt16);
                        mypdfpage.Add(pt17);
                        mypdfpage.Add(pt18);
                        mypdfpage.Add(pt19);
                        mypdfpage.Add(pt20);
                        mypdfpage.Add(pt21);
                        string appPath = HttpContext.Current.Server.MapPath("~");
                        if (appPath != "")
                        {

                            string szPath = appPath + "/Report/";
                            //string szFile = "Format1.pdf";
                            //Modified By Srinath 24/9/2014
                            string szFile = "Format1" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                            mypdfpage.SaveToDocument();

                            mydoc.SaveToFile(szPath + szFile);
                            Response.ClearHeaders();
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                            Response.ContentType = "application/pdf";
                            Response.WriteFile(szPath + szFile);


                        }
                    }
                }

            }
            if (lp == 0)
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Please Select Atleast One Student";
            }
        }
        catch (Exception ex)
        {
        }


    }
    //protected void btnclose_Click(object sender, EventArgs e)
    //{
    //    FpSpread2.Visible = false;
    //    btnclose.Visible = false;
    //}
    protected void txttorange_TextChanged(object sender, EventArgs e)
    {
        try
        {
            grdover.Visible = false;
            
            grdover.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            lblnorec.Visible = false;
            btnletter.Visible = false;
            Pnltamilformat.Visible = false;
            btntamilletter.Visible = false;
            if (txttorange.Text.ToString().Trim() != "")
            {
                int trange = Convert.ToInt32(txttorange.Text.ToString());
                if (trange > 100)
                {

                    txttorange.Text = "";
                    lblnorec.Visible = true;
                    lblnorec.Text = "Please Enter Lesser than equal to 100";
                }
                if (txtfromrange.Text.ToString().Trim() != "")
                {
                    int frange = Convert.ToInt32(txtfromrange.Text.ToString());
                    if (frange > trange)
                    {
                        txttorange.Text = "";
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
    protected void txtfromdate_TextChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        dateerr.Visible = false;
        lblnorec.Visible = false;
        grdover.Visible = false;
       
        grdover.Visible = false;
        btnletter.Visible = false;
        Pnltamilformat.Visible = false;
        btntamilletter.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;

    }
    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        dateerr.Visible = false;
        lblnorec.Visible = false;
        grdover.Visible = false;
        
        grdover.Visible = false;
        btnletter.Visible = false;
        Pnltamilformat.Visible = false;
        btntamilletter.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;

    }
    protected void ddlsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        grdover.Visible = false;
        
        grdover.Visible = false;
        btnletter.Visible = false;
        Pnltamilformat.Visible = false;
        btntamilletter.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;

    }
    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblnorec.Visible = false;
        grdover.Visible = false;
        
        grdover.Visible = false;
        btnletter.Visible = false;
        Pnltamilformat.Visible = false;
        btntamilletter.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        if (!Page.IsPostBack == false)
        {
            ddlsection.Items.Clear();
        }
        BindSectionDetail();

    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdover.Visible = false;
       
        grdover.Visible = false;
        btnletter.Visible = false;
        Pnltamilformat.Visible = false;
        btntamilletter.Visible = false;
        lblnorec.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;

        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;

    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdover.Visible = false;
       
        grdover.Visible = false;
        btnletter.Visible = false;
        Pnltamilformat.Visible = false;
        btntamilletter.Visible = false;
        btnxl.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        bindbranch();
        //   Get_Semester();
        bindsem();
        BindSectionDetail();


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
        spReportName.InnerHtml = "Attendance Letter Report";



    }
    protected void chkselectall_CheckedChanged(object sender, EventArgs e)
    {

        var checkbox = grdover.Rows[0].Cells[0].FindControl("chkselectall") as CheckBox;

        for (int i = 1; i < grdover.Rows.Count; i++)
        {


            if (checkbox.Checked == true)
            {
                var checkbox1 = grdover.Rows[i].Cells[0].FindControl("lbl_cb") as CheckBox;
                checkbox1.Checked = true;

            }
            else
            {
                var checkbox1 = grdover.Rows[i].Cells[0].FindControl("lbl_cb") as CheckBox;
                checkbox1.Checked = false;
            }
        }


    }

    public override void VerifyRenderingInServerForm(Control control)
    { }
}
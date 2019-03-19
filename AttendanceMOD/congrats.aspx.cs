//==========MANIPRABHA A.
using System;//----------29/2/12(border width,XL), 21/3/12(increase border width),24/3/12(bold footer,remove pdf)
//==================28/3/12(holiday halfday), 30/3/12(contains condition),30/3/12(len(r_no)), 24/4/12(print setting completion)
//==================11/6/12(include spl hrs,p_m_s_n, try in p_l),5/7/12(iso code),6/7/12(complete iso)
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BalAccess;
using DalConnection;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using FarPoint.Web.Spread;


public partial class congrats : System.Web.UI.Page
{
    [Serializable()]
    public class MyImg : ImageCellType
    {

        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(75);
            return img;


        }
    }
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection readcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con4a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection bind_con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    static Boolean forschoolsetting = false;// Added by sridharan
    SqlConnection con_splhr_query_master = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd;
    SqlCommand cmd3a;
    string collnamenew1 = "";
    string address1 = "";
    string address2 = "";
    string address3 = "";
    string pincode = "", state = "";
    string categery = "";
    string Affliated = "";
    string today_date = "";
    string logo1 = "";
    string logo2 = "";
    Hashtable hat = new Hashtable();
    Hashtable has_holi = new Hashtable();
    DAccess2 dacces2 = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds5 = new DataSet();
    DataSet ds6 = new DataSet();
    bool stud_flag = false;
    DateTime Admission_date;
    int moncount;
    //'----------------------------------------------------------new 
    string new_header_string_index = "", isonumber = "";
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    string split_holiday_status_1 = "", split_holiday_status_2 = "";
    string value_holi_status = "";
    string[] split_holiday_status = new string[1000];
    int conducted_hrs_new = 0;
    double tot_ondu, per_tot_ondu, cum_tot_ondu, cum_tot_ml, tot_ml, per_tot_ml;
    //----------------
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0, cum_per_workingdays1 = 0;
    int notconsider_value = 0;

    int mmyycount;
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
    // int count;
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

    int countds = 0;
    //-----------------------------------------end
    string roll = "";
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    //==============0n 23/4/12 PRABHA
    string address1_new = "", address2_new = "", address3_new = "", phoneno_new = "", coll_name_new = "";
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
    string date1 = "", datefrom = "", date2 = "", dateto = "";
    DateTime dt1 = new DateTime();
    DateTime dt2 = new DateTime();
    string email = "";
    string website = "";
    Boolean check_print_row = false;
    //---------------------------
    static string grouporusercode = "";
    static string groupor_usercode = "";

    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0, tot_ml_spl_fals = 0;
    double per_leave_fals = 0;

    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    bool splhr_flag = false;
    DataTable dt = new DataTable();
    DataRow dtrow = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
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

        lblnorec.Visible = false;
        if (!Page.IsPostBack)
        {
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");

            //bindbatch();//-----------------call bind functions
            //binddegree();
            //bindbranch();
            //bindsem();
            //bindsec();
            //month_spd.Visible = false;//
            gview.Visible = false;
            btnxl.Visible = false;
            //Added By Srinath 27/2/2013
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnprintmaster.Visible = false;
            lblpage_search.Visible = false;
            lblpages.Visible = false;


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

            //month_spd.Sheets[0].AutoPostBack = true;//            
            frmlbl.Visible = false;
            tolbl.Visible = false;
            tofromlbl.Visible = false;

            //month_spd.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            //month_spd.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            //month_spd.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
            //month_spd.ActiveSheetView.ColumnHeader.DefaultStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            //month_spd.ActiveSheetView.ColumnHeader.DefaultStyle.ForeColor = Color.Black;
            //month_spd.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            //month_spd.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
            ////month_spd.Sheets[0].Visible = false;
            //month_spd.CommandBar.Visible = false;
            //----------------------set date
            string dt = DateTime.Today.ToShortDateString();
            string[] dsplit = dt.Split(new Char[] { '/' });
            txtFromDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            today_date = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            txtToDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            Session["curr_year"] = dsplit[2].ToString();
            Session["today_date"] = today_date;


            //=======================on 11/4/12

            if (Request.QueryString["val"] == null)
            {
                bindbatch();//-----------------call bind functions
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
                }

            }
            else
            {
                //=======================page redirect from master print setting

                string_session_values = Request.QueryString["val"].Split(',');
                if (string_session_values.GetUpperBound(0) == 6)
                {
                    //   try
                    {
                        bindbatch();
                        ddlbatch.SelectedIndex = Convert.ToInt16(string_session_values[0]);
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

                            if (final_print_col_cnt > 0)
                            {
                                //setheader_print();
                                view_header_setting();
                                //month_spd.Width = final_print_col_cnt * 100;//
                                gview.Width = final_print_col_cnt * 100;
                            }
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
                        }
                    }
                    //    catch
                    {
                    }
                }
                //===================================

            }
            //======================

            //-------------------------------Master settings
            string strdayflag;
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            Session["Admissionno"] = "0";
            if (Session["usercode"] != "")
            {
                string Master1 = "";
                Master1 = "select * from Master_Settings where " + grouporusercode + "";
                readcon.Close();
                readcon.Open();
                SqlDataReader mtrdr;

                SqlCommand mtcmd = new SqlCommand(Master1, readcon);
                mtrdr = mtcmd.ExecuteReader();
                strdayflag = "";
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
                        if (mtrdr["settings"].ToString() == "Days Scholor" && mtrdr["value"].ToString() == "1")
                        {
                            strdayflag = " and (Stud_Type='Day Scholar'";
                        }
                        if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                        {
                            if (strdayflag != "" && strdayflag != "\0")
                            {
                                strdayflag = strdayflag + " or Stud_Type='Hostler'";
                            }
                            else
                            {
                                strdayflag = " and (Stud_Type='Hostler'";
                            }
                        }
                        if (mtrdr["settings"].ToString() == "Day Wise" && mtrdr["value"].ToString() == "1")
                        {
                            Session["daywise"] = "1";
                        }
                        if (mtrdr["settings"].ToString() == "Hour Wise" && mtrdr["value"].ToString() == "1")
                        {
                            Session["hourwise"] = "1";
                        }
                        if (mtrdr["settings"].ToString() == "Admission No" && mtrdr["value"].ToString() == "1")
                        {
                            Session["Admissionno"] = "1";
                        }
                    }
                }
                if (strdayflag != string.Empty)
                {
                    strdayflag = strdayflag + ")";
                }
                Session["strvar"] = strdayflag;
            }//      
            //month_spd.Visible = false;//
            gview.Visible = false;
            ddlpage.Visible = false;
            lblpages.Visible = false;
            pnl_pageset.Visible = false;
            frmlbl.Visible = false;
            tolbl.Visible = false;
            tofromlbl.Visible = false;
            lblnorec.Visible = false;
            btnprintmaster.Visible = false;
            // Added By Sridharan 12 Mar 2015
            //{
            string grouporusercodeschool = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercodeschool = " group_code=" + Session["group_code"].ToString().Trim() + "";
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
                    //lblcollege.Text = "School";
                    lblbatch.Text = "Year";
                    lbldegree.Text = "School Type";
                    lblbranch.Text = "Standard";
                    lblduration.Text = "Term";
                    //Label1.Text = "Test Mark R11-Continuous Assessment Report";
                    //lbldeg.Attributes.Add("Style", "font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    left: 229px;    position: absolute;    top: 210px;");
                    //tbdeg.Attributes.Add("Style", "   font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    height: 20px;    left: 328px;    margin-right: 15px;    position: absolute;    top: 210px;    width: 100px;");
                    //lblbranch.Attributes.Add("Style", " color: Black;    display: inline-block;    font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    left: 439px;    position: absolute;    top: 212px;    width: 90px;");
                    //txtbranch.Attributes.Add("Style", "font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    height: 20px;    left: 509px;    position: absolute;    top: 210px;    width: 180px;");
                    //lblsection.Attributes.Add("Style", " color: Black;    display: inline-block;    font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    left: 702px;    position: absolute;    top: 211px;    width: 100px;");


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

            //} Sridharan
        }
    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {



        //Control cntUpdateBtn = month_spd.FindControl("Update");
        //Control cntCancelBtn = month_spd.FindControl("Cancel");
        //Control cntCopyBtn = month_spd.FindControl("Copy");
        //Control cntCutBtn = month_spd.FindControl("Clear");
        //Control cntPasteBtn = month_spd.FindControl("Paste");
        //Control cntPageNextBtn = month_spd.FindControl("Next");//
        //Control cntPagePreviousBtn = month_spd.FindControl("Prev");//

        Control cntPageNextBtn1 = gview.FindControl("Next");
        Control cntPagePreviousBtn2 = gview.FindControl("Prev");
        // Control cntPagePrintBtn = month_spd.FindControl("Print");

        if ((cntPageNextBtn1 != null))
        {

            TableCell tc = (TableCell)cntPageNextBtn1.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            //tc = (TableCell)cntCancelBtn.Parent;
            //tr.Cells.Remove(tc);


            //tc = (TableCell)cntCopyBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntCutBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPasteBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPageNextBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPagePreviousBtn.Parent;
            tr.Cells.Remove(tc);

            ////tc = (TableCell)cntPagePrintBtn.Parent;
            ////tr.Cells.Remove(tc);

        }

        base.Render(writer);
    }

    public void bindbatch()
    {
        ////batch
        ddlbatch.Items.Clear();
        string sqlstring = "";
        int max_bat = 0;
        con.Close();
        con.Open();
        cmd = new SqlCommand(" select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year", con);
        SqlDataAdapter da1 = new SqlDataAdapter(cmd);
        DataSet ds1 = new DataSet();
        da1.Fill(ds1);
        ddlbatch.DataSource = ds1;
        ddlbatch.DataValueField = "batch_year";
        ddlbatch.DataTextField = "batch_year";
        ddlbatch.DataBind();

        //----------------display max year value 
        sqlstring = "select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' ";
        max_bat = Convert.ToInt32(GetFunction(sqlstring));
        ddlbatch.SelectedValue = max_bat.ToString();
        con.Close();
        //binddegree();

    }


    public void binddegree()
    {
        ////degree
        ddldegree.Items.Clear();
        con.Close();
        con.Open();
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["usercode"].ToString();

        DataSet ds = Bind_Degree(collegecode, usercode);
        ddldegree.DataSource = ds;
        ddldegree.DataValueField = "course_id";
        ddldegree.DataTextField = "course_name";
        ddldegree.DataBind();
        //bindbranch();

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
        //bindsec();
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

    public DataSet Bind_Degree(string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        SqlCommand cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where " + groupor_usercode + " and course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code ", dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    public DataSet Bind_Dept(string degree_code, string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        SqlCommand cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where " + groupor_usercode + " and course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code ", dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }

    public void bindbranch()
    {
        //--------load degree
        ddlbranch.Items.Clear();
        con.Close();
        con.Open();
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["usercode"].ToString();
        string course_id = ddldegree.SelectedValue.ToString();
        DataSet ds = Bind_Dept(course_id, collegecode.ToString(), usercode);
        ddlbranch.DataSource = ds;
        ddlbranch.DataTextField = "dept_name";
        ddlbranch.DataValueField = "degree_code";
        ddlbranch.DataBind();
        con.Close();
    }


    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {

        lblnorec.Visible = false;
        tolbl.Visible = false;
        frmlbl.Visible = false;
        tofromlbl.Visible = false;
        //month_spd.Visible = false;//
        gview.Visible = false;
        pnl_pageset.Visible = false;
        btnxl.Visible = false;
        //Added By Srinath 27/2/2013
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnprintmaster.Visible = false;

    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblnorec.Visible = false;
        tolbl.Visible = false;
        frmlbl.Visible = false;
        tofromlbl.Visible = false;
        //month_spd.Visible = false;//
        gview.Visible = false;
        pnl_pageset.Visible = false;
        btnxl.Visible = false;
        //Added By Srinath 27/2/2013
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        bindbranch();
        bindsem();
        bindsec();
        btnprintmaster.Visible = false;

    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {

        lblnorec.Visible = false;
        tolbl.Visible = false;
        frmlbl.Visible = false;
        tofromlbl.Visible = false;
        //month_spd.Visible = false;//
        gview.Visible = false;
        pnl_pageset.Visible = false;
        btnxl.Visible = false;
        //Added By Srinath 27/2/2013
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        bindsem();
        bindsec();
        btnprintmaster.Visible = false;


    }
    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblnorec.Visible = false;
        tolbl.Visible = false;
        frmlbl.Visible = false;
        tofromlbl.Visible = false;
        //month_spd.Visible = false;//
        gview.Visible = false;
        pnl_pageset.Visible = false;
        btnxl.Visible = false;
        //Added By Srinath 27/2/2013
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        bindsec();
        btnprintmaster.Visible = false;
    }
    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblnorec.Visible = false;
        tolbl.Visible = false;
        frmlbl.Visible = false;
        tofromlbl.Visible = false;
        //month_spd.Visible = false;//
        gview.Visible = false;
        pnl_pageset.Visible = false;
        btnxl.Visible = false;
        //Added By Srinath 27/2/2013
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
    }


    public string GetFunction(string Att_strqueryst)
    {
        string sqlstr;
        sqlstr = Att_strqueryst;
        getcon.Close();
        getcon.Open();
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr, getcon);
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
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        lblnorec.Visible = false;
        tolbl.Visible = false;
        frmlbl.Visible = false;
        tofromlbl.Visible = false;
        //month_spd.Visible = false;//
        gview.Visible = false;
        pnl_pageset.Visible = false;
        btnxl.Visible = false;
        //Added By Srinath 27/2/2013
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnprintmaster.Visible = false;
    }
    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        lblnorec.Visible = false;
        tolbl.Visible = false;
        frmlbl.Visible = false;
        tofromlbl.Visible = false;
        //month_spd.Visible = false;//
        gview.Visible = false;
        pnl_pageset.Visible = false;
        btnxl.Visible = false;
        //Added By Srinath 27/2/2013
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
    }
    public string Attmark(string Attstr_mark)
    {

        string Att_mark;
        Att_mark = "";
        if (Attstr_mark == "1")
        {
            Att_mark = "P";

        }

        else if (Attstr_mark == "2")
        {
            Att_mark = "A";

        }
        else if (Attstr_mark == "3")
        {
            Att_mark = "OD";

        }
        else if (Attstr_mark == "4")
        {
            Att_mark = "ML";

        }
        else if (Attstr_mark == "5")
        {
            Att_mark = "SOD";

        }
        else if (Attstr_mark == "10")
        {
            Att_mark = "L";

        }
        else if (Attstr_mark == "11")
        {
            Att_mark = "NSS";

        }
        else if (Attstr_mark == "12")
        {
            Att_mark = "HS";

        }
        else if (Attstr_mark == "13")
        {
            Att_mark = "PP";

        }

        else if (Attstr_mark == "14")
        {
            Att_mark = "SYOD";

        }
        else if (Attstr_mark == "15")
        {
            Att_mark = "COD";

        }
        else if (Attstr_mark == "16")
        {
            Att_mark = "OOD";
        }
        else if (Attstr_mark == "17")
        {
            Att_mark = "LA";
        }
        return Att_mark;
    }


    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {

            //month_spd.Visible = false;//
            gview.Visible = true;
            ddlpage.Visible = false;
            lblpages.Visible = false;
            pnl_pageset.Visible = false;
            frmlbl.Visible = false;
            tolbl.Visible = false;
            tofromlbl.Visible = false;
            lblnorec.Visible = false;
            btnprintmaster.Visible = false;
            btnclick();

            int temp_col = 0;
            //if (month_spd.Sheets[0].ColumnCount > 0 && month_spd.Sheets[0].RowCount > 0)//===========on 9/4/12//
            if (gview.HeaderRow.Cells.Count > 0 && gview.Rows.Count > 0)
            {
                //month_spd.Visible = true;//
                gview.Visible = true;
                btnprintmaster.Visible = true;
                pnl_pageset.Visible = false;

                pnl_pageset.Visible = false;
                
                for (temp_col = 0; temp_col < gview.Columns.Count; temp_col++)
                {

                    gview.HeaderRow.Cells[temp_col].Visible = true;
                }
                for (temp_col = 0; temp_col < gview.Columns.Count; temp_col++)
                {

                    gview.HeaderRow.Cells[temp_col].Visible = true;
                }


                //'--------------------settings
                int tot = gview.Columns.Count;
                if (Session["Rollflag"].ToString() == "0")
                {

                    gview.HeaderRow.Cells[1].Visible = false;
                    
                }
                if (Session["Regflag"].ToString() == "0")
                {

                    gview.HeaderRow.Cells[2].Visible = false;
                }
                if (Session["Studflag"].ToString() == "0")
                {

                    gview.HeaderRow.Cells[5].Visible = false;
                }
                if (Session["Admissionno"].ToString() == "0")
                {

                    gview.HeaderRow.Cells[3].Visible = false;
                }
                //'---------------------------------------------------------

                final_print_col_cnt = 0;

                for (temp_col = 0; temp_col < gview.HeaderRow.Cells.Count; temp_col++)
                {

                    if (gview.HeaderRow.Cells[temp_col].Visible == true)
                    {
                        final_print_col_cnt++;
                    }
                }
                

                
                view_header_setting();
                //4 end.college information setting
            }
            else
            {
                //month_spd.Visible = false;//
                gview.Visible = false;
                btnprintmaster.Visible = false;
                pnl_pageset.Visible = false;
                lblnorec.Visible = true;
            }
        }
        catch
        {
        }
    }


    public void btnclick()
    {
        //  try
        {
            //------------------------------------------date validation-------------------------------
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
                lblnorec.Text = "From Date Must Be Less Than To Date";
                lblnorec.Visible = true;
                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                TextBoxother.Visible = false;
                lblpages.Visible = false;
                TextBoxpage.Visible = false;
                //month_spd.Visible = false;//
                //month_spd.Sheets[0].RowCount = 0;//
                gview.Visible = false;                

            }
            else
            {
                lblnorec.Text = "";
                lblnorec.Visible = false;
                //Buttontotal.Visible = true;
                //lblrecord.Visible = true;
                //DropDownListpage.Visible = true;
                //TextBoxother.Visible = true;
                //lblpages.Visible = true;
                //TextBoxpage.Visible = true;
                //month_spd.Visible = true;
                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                TextBoxother.Visible = false;
                lblpages.Visible = false;
                TextBoxpage.Visible = false;
                //month_spd.Visible = false;//
                gview.Visible = false;
                btnxl.Visible = false;
                //Added By Srinath 27/2/2013
                lblrptname.Visible = false;
                txtexcelname.Visible = false;

                gobutton();
            }
            // persentmonthcal_new();
            //if (Convert.ToInt32(month_spd.Sheets[0].RowCount) == 0)//
            if (Convert.ToInt32(gview.Rows.Count) == 0)
            {
                lblnorec.Visible = true;
                //month_spd.Visible = false;//
                gview.Visible=false;
                btnxl.Visible = false;
                //Added By Srinath 27/2/2013
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
            }
            else
            {
                lblnorec.Visible = false;
                Buttontotal.Visible = true;
                lblrecord.Visible = true;
                DropDownListpage.Visible = true;
                TextBoxother.Visible = false;
                lblpages.Visible = true;
                TextBoxpage.Visible = true;
                //month_spd.Visible = true;//
                gview.Visible = true;
                btnxl.Visible = true;
                //Added By Srinath 27/2/2013
                lblrptname.Visible = true;
                txtexcelname.Visible = true;

                Double totalRows = 0;
                //totalRows = Convert.ToInt32(month_spd.Sheets[0].RowCount);//
                totalRows = Convert.ToInt32(gview.Rows.Count);
                //Session["totalPages"] = (int)Math.Ceiling(totalRows / month_spd.Sheets[0].PageSize);//
                Session["totalPages"] = (int)Math.Ceiling(totalRows / gview.PageSize);
                Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                DropDownListpage.Items.Clear();
                if (totalRows >= 10)
                {
                    //month_spd.Sheets[0].PageSize = Convert.ToInt32(totalRows);//
                    gview.PageSize = Convert.ToInt32(totalRows);
                    for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                    {
                        DropDownListpage.Items.Add((k + 10).ToString());
                    }
                    DropDownListpage.Items.Add("Others");
                    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                    //month_spd.Height = 335;//
                    gview.Height = 335;

                }
                else if (totalRows == 0)
                {
                    DropDownListpage.Items.Add("0");
                    //month_spd.Height = 100;//
                    gview.Height = 100;
                }
                else
                {
                    //month_spd.Sheets[0].PageSize = Convert.ToInt32(totalRows);//
                    //DropDownListpage.Items.Add(month_spd.Sheets[0].PageSize.ToString());//
                    //month_spd.Height = 100 + (10 * Convert.ToInt32(totalRows));//

                    gview.PageSize = Convert.ToInt32(totalRows);
                    DropDownListpage.Items.Add(gview.PageSize.ToString());
                    gview.Height = 100 + (10 * Convert.ToInt32(totalRows));
                }
                //if (Convert.ToInt32(month_spd.Sheets[0].RowCount) > 10)
                if (Convert.ToInt32(gview.Rows.Count) > 10)
                {
                    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                    //month_spd.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);//
                    gview.PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                    CalculateTotalPages();
                }
                // month_spd.Sheets[0].PageSize = Convert.ToInt32(ddlto.SelectedValue.ToString()) - Convert.ToInt32(ddlfrom.SelectedValue.ToString()) + 1 + spancount + count+1;
            }
            //month_spd.Height = month_spd.Sheets[0].RowCount * 20 + 200;//
            //month_spd.Width = month_spd.Sheets[0].ColumnCount * 74;//

            gview.Height = gview.Rows.Count * 20 + 200;
            gview.Width = gview.Columns.Count * 74;
        }
        //  catch
        {
        }
    }
    //'--------------------------------------

    public void persentmonthcal_new()
    {
        //   try
        {
            int my_un_mark = 0;
            Boolean isadm = false;
            evng_conducted_half_days = 0;
            mng_conducted_half_days = 0;
            int njdate_mng = 0, per_holidate_mng = 0;
            int njdate_evng = 0, per_holidate_evng = 0;

            per_abshrs_spl = 0;
            tot_per_hrs_spl = 0;
            tot_ondu_spl = 0;
            per_hhday_spl = 0;
            unmark_spl = 0;
            tot_conduct_hr_spl = 0;
            tot_per_hrs = 0;
            per_holidate = 0;
            conducted_hrs_new = 0;
            int demfcal, demtcal;
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
            per_from_date = Convert.ToDateTime(frdate);
            per_to_date = Convert.ToDateTime(todate);
            dumm_from_date = per_from_date;
            notconsider_value = 0;    //Added by srinath 24/8/2013
            hat.Clear();
            //  ds5.Tables[0].Rows[rows_count]["RollNumber"].ToString();
            hat.Add("std_rollno", roll.ToString());
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds2 = d2.select_method("STUD_ATTENDANCE", hat, "sp");

            mmyycount = ds2.Tables[0].Rows.Count;
            moncount = mmyycount - 1;

            count = ds1.Tables[0].Rows.Count;
            if (stud_flag == false)
            {
                stud_flag = true;
                hat.Clear();
                hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
                hat.Add("sem", int.Parse(ddlduration.SelectedItem.ToString()));
                hat.Add("from_date", frdate.ToString());
                hat.Add("to_date", todate.ToString());
                hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));


                int iscount = 0;
                holidaycon.Close();
                holidaycon.Open();
                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedItem.ToString() + "";
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
                        //modified by prabha
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

                        //modified
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

                        if (!holiday_table3.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
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
                            getspecial_hr();

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
                                            value = ds2.Tables[0].Rows[next][date].ToString();

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
                                            value = ds2.Tables[0].Rows[next][date].ToString();

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
            per_tot_ml = tot_ml;
            per_njdate = njdate;
            pre_present_date = Present - njdate;
            per_per_hrs = tot_per_hrs;
            per_absent_date = Absent;
            pre_ondu_date = Onduty;
            pre_leave_date = Leave;

            //per_workingdays = workingdays - per_njdate;
            //per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - ((Convert.ToInt16(per_holidate_mng) * fnhrs) + (Convert.ToInt16(per_holidate_evng) * (NoHrs - fnhrs))) - notconsider_value - dum_unmark; //dum_unmark hided on 08.08.12 // ((Convert.ToInt16(njdate_mng) * fnhrs) + (Convert.ToInt16(njdate_evng) * (NoHrs - fnhrs)));

            //per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value; //added on 08.08.12,notconsider value added for hr suspension,09.08.12 mythili

            per_workingdays = workingdays - per_njdate;
            per_workingdays1 = ((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - ((Convert.ToInt16(per_holidate_mng) * fnhrs) + (Convert.ToInt16(per_holidate_evng) * (NoHrs - fnhrs))) - notconsider_value - dum_unmark; //dum_unmark hided on 08.08.12 // ((Convert.ToInt16(njdate_mng) * fnhrs) + (Convert.ToInt16(njdate_evng) * (NoHrs - fnhrs)));

            per_workingdays1 = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value; //added on 08.08.12,notconsider value added for hr suspension,09.08.12 mythili

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
        // catch
        {
        }
    }
    //'------------------------------------
    public void gobutton()
    {
        Buttontotal.Visible = true;
        lblrecord.Visible = true;
        DropDownListpage.Visible = true;
        TextBoxother.Visible = true;
        lblpages.Visible = true;
        TextBoxpage.Visible = true;


        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();//added by srinath 31/1/2013
        //nepnl.Visible = false;
        // month_spd.Visible = false;
        //Panel3.Visible = false;
        //month_spd.CurrentPage = 0;//        
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
        Boolean sflag = false;
        
        gview.Width = 845;
        gview.Height = 1500;
        string dum_tage_date = "";
        string dum_tage_hrs = "";
        int s_no = 0;
        string sections = "";
        string strsec = "";
        /*****************************************/
        
        gview.Visible = true;
        btnxl.Visible = true;
        //Added By Srinath 27/2/2013
        lblrptname.Visible = true;
        txtexcelname.Visible = true;
        
        

        //  month_spd.Sheets[0].PageSize = 45;
        /*****************************************/
        //===========Hided by Manikandan 18/05/2013
        //MyImg mi = new MyImg();
        //mi.ImageUrl = "~/images/10BIT001.jpeg";
        //mi.ImageUrl = "Handler/Handler2.ashx?";
        //MyImg mi2 = new MyImg();
        //mi2.ImageUrl = "~/images/10BIT001.jpeg";
        //mi2.ImageUrl = "Handler/Handler5.ashx?";
        //====================

        //month_spd.Sheets[0].RowCount = 0;//        
        sec_txt = ddlsec.Text;
        date1 = txtFromDate.Text.ToString();
        string[] split = date1.Split(new Char[] { '/' });
        sections = ddlsec.SelectedValue.ToString();
        if (sections.ToString() != "")
        {
            if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
            {
                strsec = "";
            }
            else
            {
                strsec = " and registration.sections='" + sections.ToString() + "'";
            }
        }
        else
        {
            strsec = "";
        }

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


                           
                            
                            


                            /******************************/
                            //=============================0n 9/4/12
                            hat.Clear();
                            hat.Add("college_code", Session["collegecode"].ToString());
                            hat.Add("form_name", "congrats.aspx");
                            dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
                            //===========================================


                            //======================0n 11/4/12 PRABHA 
                            if (dsprint.Tables[0].Rows.Count > 0)
                            {
                                //isonumber = dsprint.Tables[0].Rows[0]["ISOCode"].ToString();
                                if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
                                {
                                    new_header_string_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString();
                                    
                                    new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                                    new_header_string_split = new_header_string.Split(',');
                                    
                                }
                            }

                            string splhrrights = d2.GetFunction("select rights from  special_hr_rights where " + grouporusercode + "");
                            if (splhrrights.Trim().ToLower() == "true")
                            {
                                splhr_flag = true;
                            }

                            //=====================================

                            

                            //added By Srinath 11/8/2013
                            dt.Columns.Add("S.No", typeof(string));
                            dt.Columns.Add("Roll No", typeof(string));
                            dt.Columns.Add("Reg No", typeof(string));
                            dt.Columns.Add("Admission No", typeof(string));
                            dt.Columns.Add("Name of the Student", typeof(string));
                            dt.Columns.Add("Student Type", typeof(string));
                            dt.Columns.Add("Cond Hrs", typeof(string));
                            dt.Columns.Add("Atten Hrs", typeof(string));
                            dt.Columns.Add("Atten %", typeof(string));

                            dtrow = dt.NewRow();
                            dtrow["S.No"] = "S.No";
                            dtrow["Roll No"] = "Roll No";
                            dtrow["Reg No"] = "Reg No";
                            dtrow["Admission No"] = "Admission No";
                            dtrow["Name of the Student"] = "Name of the Student";
                            dtrow["Student Type"] = "Student Type";
                            dtrow["Cond Hrs"] = "Cond Hrs";
                            dtrow["Atten Hrs"] = "Atten Hrs";
                            dtrow["Atten %"] = "Atten %";
                            dt.Rows.Add(dtrow);

                            string orderby_Setting = dacces2.GetFunction("select value from master_Settings where settings='order_by'");
                            string strorder = "ORDER BY roll_no";
                            if (orderby_Setting == "0")
                            {
                                strorder = "ORDER BY roll_no";
                            }
                            else if (orderby_Setting == "1")
                            {
                                strorder = "ORDER BY Reg_No";
                            }
                            else if (orderby_Setting == "2")
                            {
                                strorder = "ORDER BY Stud_Name";
                            }
                            else if (orderby_Setting == "0,1,2")
                            {
                                strorder = "ORDER BY roll_no,Reg_No,Stud_Name";
                            }
                            else if (orderby_Setting == "0,1")
                            {
                                strorder = "ORDER BY roll_no,Reg_No";
                            }
                            else if (orderby_Setting == "1,2")
                            {
                                strorder = "ORDER BY Reg_No,Stud_Name";
                            }
                            else if (orderby_Setting == "0,2")
                            {
                                strorder = "ORDER BY roll_no,Stud_Name";
                            }

                            bind_con.Close();
                            bind_con.Open();
                            //     bind_sql = "select roll_no,reg_no,stud_name,stud_type from registration where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar' and current_semester=" + ddlduration.SelectedValue.ToString() + " " + strsec + " " + Session["strvar"] + "order by roll_no,reg_no,stud_name";
                            bind_sql = "select roll_no,reg_no,stud_name,stud_type,len(roll_no),convert(varchar(15),adm_date,103) as adm_date,Roll_Admit  from registration where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar' " + strsec + " " + Session["strvar"] + " " + strorder + "";
                            //bind_sql = "select roll_no,reg_no,stud_name,stud_type,len(roll_no),convert(varchar(15),adm_date,103) as adm_date  from registration where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar' " + strsec + " " + Session["strvar"] + "order by roll_no,reg_no,stud_name";
                            SqlCommand studinfocmd = new SqlCommand(bind_sql, bind_con);
                            SqlDataReader studinfors;
                            studinfors = studinfocmd.ExecuteReader();
                            
                            if (studinfors.HasRows == true)
                            {
                                int sno = 0;
                                while (studinfors.Read())
                                {
                                    lblnorec.Text = "";
                                    lblnorec.Visible = false;
                                    roll = studinfors["roll_no"].ToString();

                                    string admdate = studinfors["adm_date"].ToString();
                                    string[] admdatesp = admdate.Split(new Char[] { '/' });
                                    admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
                                    Admission_date = Convert.ToDateTime(admdate);

                                    //'----------------------------------------new start----------------

                                    hat.Clear();
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
                                    }
                                    hat.Clear();
                                    hat.Add("colege_code", Session["collegecode"].ToString());
                                    ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
                                    countds = ds1.Tables[0].Rows.Count;


                                    persentmonthcal_new();



                                    per_con_hrs = per_workingdays1;

                                    per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl_fals) / (per_con_hrs + tot_conduct_hr_spl_fals)) * 100);

                                    if (per_tage_hrs > 100)
                                    {
                                        per_tage_hrs = 100;
                                    }

                                    dum_tage_hrs = Convert.ToString(per_tage_hrs);
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

                                    if ((Convert.ToDouble(dum_tage_hrs) == Convert.ToDouble(100)))
                                    {
                                        lblnorec.Text = "";
                                        lblnorec.Visible = false;
                                        sflag = true;
                                        sno++;
                                      
                                        
                                        dtrow = dt.NewRow();

                                        dtrow["S.No"] = Convert.ToString(sno);

                                        dtrow["Roll No"] = studinfors["roll_no"].ToString();

                                        dtrow["Reg No"] = studinfors["reg_no"].ToString();

                                        dtrow["Admission No"] = studinfors["Roll_Admit"].ToString();

                                        dtrow["Name of the Student"] = studinfors["stud_name"].ToString();

                                        dtrow["Student Type"] = studinfors["stud_type"].ToString();

                                        dtrow["Cond Hrs"] = (per_con_hrs + tot_conduct_hr_spl).ToString();

                                        dtrow["Atten Hrs"] = (per_per_hrs + tot_per_hrs_spl).ToString();

                                        dtrow["Atten %"] = dum_tage_hrs.ToString();

                                        dt.Rows.Add(dtrow);
                                    }
                                    else
                                    {
                                        lblnorec.Text = "No Record(s) Found";
                                        lblnorec.Visible = true;
                                        
                                        gview.Visible = false;
                                        btnxl.Visible = false;
                                        //Added By Srinath 27/2/2013
                                        lblrptname.Visible = false;
                                        txtexcelname.Visible = false;
                                        Buttontotal.Visible = false;
                                        lblrecord.Visible = false;
                                        DropDownListpage.Visible = false;
                                        TextBoxother.Visible = false;
                                        lblpages.Visible = false;
                                        TextBoxpage.Visible = false;
                                        lblpages.Visible = false;
                                        ddlpage.Visible = false;
                                    }
                                }
                                gview.DataSource = dt;
                                gview.DataBind();
                                gview.Visible = true;
                                RowHead(gview, 1);

                                for (int row = 1; row < gview.Rows.Count; row++)
                                {
                                    for (int cell = 0; cell < gview.Rows[row].Cells.Count; cell++)
                                    {
                                        if (gview.HeaderRow.Cells[cell].Text != "Name of the Student" && gview.HeaderRow.Cells[cell].Text != "Student Type")
                                        {
                                            gview.Rows[row].Cells[cell].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                }



                            }
                            else
                            {
                                lblnorec.Text = "No Record(S) Found";
                                lblnorec.Visible = true;
                                
                                gview.Visible = false;
                                btnxl.Visible = false;
                                
                                lblrptname.Visible = false;
                                txtexcelname.Visible = false;
                                Buttontotal.Visible = false;
                                lblrecord.Visible = false;
                                DropDownListpage.Visible = false;
                                TextBoxother.Visible = false;
                                lblpages.Visible = false;
                                TextBoxpage.Visible = false;
                            }

                            //'------------------------------


                        }
                        else
                        {
                            tofromlbl.Visible = true;
                            frmlbl.Visible = false;
                            tolbl.Visible = false;
                            
                            gview.Visible = false;
                            pnl_pageset.Visible = false;
                            lblnorec.Visible = false;
                        }
                    }
                    else
                    {
                        tofromlbl.Visible = false;
                        frmlbl.Visible = false;
                        tolbl.Visible = true;
                        
                        gview.Visible = false;
                        pnl_pageset.Visible = false;
                        lblnorec.Visible = false;
                    }
                }
                else
                {
                    tofromlbl.Visible = false;
                    frmlbl.Visible = false;
                    tolbl.Visible = true;
                    
                    gview.Visible = false;
                    pnl_pageset.Visible = false;
                    lblnorec.Visible = false;
                }

            }
            else
            {
                tofromlbl.Visible = false;
                frmlbl.Visible = true;
                tolbl.Visible = false;
                
                gview.Visible = false;
                pnl_pageset.Visible = false;
                lblnorec.Visible = false;
            }

        }
        else
        {
            tofromlbl.Visible = false;
            frmlbl.Visible = true;
            tolbl.Visible = false;
            
            gview.Visible = false;
            pnl_pageset.Visible = false;
            lblnorec.Visible = false;
        }
    }

    protected void RowHead(GridView gview, int count)
    {
        for (int head = 0; head < count; head++)
        {
            gview.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            gview.Rows[head].Font.Name = "Book Antique";
        }
    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        LabelE.Visible = false;
        TextBoxother.Text = "";
        if (DropDownListpage.Text == "Others")
        {

            TextBoxother.Visible = true;
            TextBoxother.Focus();

        }
        else
        {
            TextBoxother.Visible = false;
            //month_spd.Visible = true;
            btnxl.Visible = true;
            //Added By Srinath 27/2/2013
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            //month_spd.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            CalculateTotalPages();
            //  month_spd.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
        }
        //month_spd.SaveChanges();
        //month_spd.CurrentPage = 0;
    }
    protected void TextBoxpage_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (TextBoxpage.Text.Trim() != "")
            {
                if (Convert.ToInt16(TextBoxpage.Text) > Convert.ToInt16(Session["totalPages"]))
                {
                    LabelE.Visible = true;
                    LabelE.Text = "Exceed The Page Limit";
                    //month_spd.Visible = true;
                    btnxl.Visible = true;
                    //Added By Srinath 27/2/2013
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    TextBoxpage.Text = "";
                }
                else if (Convert.ToInt32(TextBoxpage.Text) == 0)
                {
                    LabelE.Visible = true;
                    LabelE.Text = "Search should be greater than zero";
                    TextBoxpage.Text = "";
                }
                else
                {
                    LabelE.Visible = false;
                    //month_spd.CurrentPage = Convert.ToInt16(TextBoxpage.Text) - 1;
                    //month_spd.Visible = true;
                    btnxl.Visible = true;
                    //Added By Srinath 27/2/2013
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                }
            }
        }
        catch
        {
            TextBoxpage.Text = "";
        }

    }
    protected void TextBoxother_TextChanged(object sender, EventArgs e)
    {
        try
        {

            if (TextBoxother.Text != "")
            {

                //month_spd.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
                CalculateTotalPages();
            }
        }
        catch
        {
            TextBoxother.Text = "";
        }

    }
    void CalculateTotalPages()
    {
        Double totalRows = 0;
        //totalRows = Convert.ToInt32(month_spd.Sheets[0].RowCount - 7);
        //Session["totalPages"] = (int)Math.Ceiling(totalRows / month_spd.Sheets[0].PageSize);

        totalRows = Convert.ToInt32(gview.Rows.Count - 7);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / gview.PageSize);

        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        Buttontotal.Visible = true;
    }
    protected void RadioHeader_CheckedChanged(object sender, EventArgs e)
    {
        ddlpage.Items.Clear();
        //int totrowcount = month_spd.Sheets[0].RowCount;//
        int totrowcount = gview.Rows.Count;
        int pages = totrowcount / 30;
        int intialrow = 1;
        int remainrows = totrowcount % 30;
        //if (month_spd.Sheets[0].RowCount > 0)//
        if (gview.Rows.Count > 0)
        {
            int i5 = 0;
            ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
            for (int i = 1; i <= pages; i++)
            {
                i5 = i;

                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                intialrow = intialrow + 30;
            }
            if (remainrows > 0)
            {
                i = i5 + 1;
                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
            }
        }
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpages.Visible = false;
        TextBoxpage.Visible = false;


        lblpages.Visible = true;
        ddlpage.Visible = true;
    }
    protected void Radiowithoutheader_CheckedChanged(object sender, EventArgs e)
    {
        ddlpage.Items.Clear();
        //int totrowcount = month_spd.Sheets[0].RowCount;//
        int totrowcount = gview.Rows.Count;
        int pages = totrowcount / 35;
        int intialrow = 1;
        int remainrows = totrowcount % 35;
        //if (month_spd.Sheets[0].RowCount > 0)//
        if (gview.Rows.Count > 0)
        {
            int i5 = 0;
            ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
            for (int i = 1; i <= pages; i++)
            {
                i5 = i;

                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                intialrow = intialrow + 35;
            }
            if (remainrows > 0)
            {
                i = i5 + 1;
                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
            }
        }
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        TextBoxother.Visible = false;
        lblpages.Visible = false;
        TextBoxpage.Visible = false;
        lblpages.Visible = true;
        ddlpage.Visible = true;
        lblnorec.Visible = false;
    }
    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        hat.Clear();
        hat.Add("college_code", Session["collegecode"].ToString());
        hat.Add("form_name", "congrats.aspx");
        dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        if (dsprint.Tables[0].Rows.Count > 0)
        {
            view_footer = dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString();
            view_header = dsprint.Tables[0].Rows[0]["header_flag_value"].ToString();
            view_footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();

            lblnorec.Visible = false;
            if (view_header == "0")
            {
                for (int i = 0; i < gview.Rows.Count; i++)
                {
                    gview.Visible = false;
                }
                int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
                int end = start + 24;
                
                if (end >= gview.Rows.Count)
                {
                    
                    end = gview.Rows.Count;
                }

                int rowstat = gview.Rows.Count - Convert.ToInt32(start);
                int rowend1 = gview.Rows.Count - Convert.ToInt32(end);
                for (int i = start - 1; i < end; i++)
                {                    
                    gview.Rows[i].Visible = true;
                }
                
                for (int row_cnt = 0; row_cnt < gview.Columns.Count; row_cnt++)
                {
                    gview.HeaderRow.Cells[row_cnt].Visible = true;
                }

            }
            else if (view_header == "1")
            {
                
                for (int i = 0; i < gview.Rows.Count; i++)
                {                    
                    gview.Rows[i].Visible = false;
                }
                int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
                int end = start + 24;
                
                if (end >= gview.Rows.Count)
                {                    
                    end = gview.Rows.Count;
                }
                //int rowstart = month_spd.Sheets[0].RowCount - Convert.ToInt32(start);//
                //int rowend = month_spd.Sheets[0].RowCount - Convert.ToInt32(end);//

                int rowstart = gview.Rows.Count - Convert.ToInt32(start);
                int rowend = gview.Rows.Count - Convert.ToInt32(end);
                for (int i = start - 1; i < end; i++)
                {
                    gview.Rows[i].Visible = true;
                }
                if (Convert.ToInt32(ddlpage.SelectedValue.ToString()) == 1)
                {   
                    for (int row_cnt = 0; row_cnt < gview.Columns.Count; row_cnt++)
                    {
                        gview.HeaderRow.Cells[row_cnt].Visible = true;
                    }
                }
                else
                {
                    //for (int row_cnt = 0; row_cnt < month_spd.Sheets[0].ColumnHeader.RowCount; row_cnt++)//
                    for (int row_cnt = 0; row_cnt < gview.Columns.Count; row_cnt++)
                    {
                        //month_spd.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;//
                        gview.HeaderRow.Cells[row_cnt].Visible = true;
                    }
                }
            }
            else
            {
                //for (int i = 0; i < month_spd.Sheets[0].RowCount; i++)//
                for (int i = 0; i < gview.Rows.Count; i++)
                {
                    //month_spd.Sheets[0].Rows[i].Visible = false;//
                    gview.Rows[i].Visible = false;
                }
                int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
                int end = start + 24;
                //if (end >= month_spd.Sheets[0].RowCount)//
                if (end >= gview.Rows.Count)
                {
                    //end = month_spd.Sheets[0].RowCount;//
                    end = gview.Rows.Count;
                }
                //int rowstart = month_spd.Sheets[0].RowCount - Convert.ToInt32(start);//
                //int rowend = month_spd.Sheets[0].RowCount - Convert.ToInt32(end);//

                int rowstart = gview.Rows.Count - Convert.ToInt32(start);
                int rowend = gview.Rows.Count - Convert.ToInt32(end);
                for (int i = start - 1; i < end; i++)
                {
                    //month_spd.Sheets[0].Rows[i].Visible = true;//
                    gview.Rows[i].Visible = true;
                }

                {
                    //for (int row_cnt = 0; row_cnt < month_spd.Sheets[0].ColumnHeader.RowCount; row_cnt++)//
                    for (int row_cnt = 0; row_cnt < gview.Columns.Count; row_cnt++)
                    {
                        //month_spd.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;//
                        gview.HeaderRow.Cells[row_cnt].Visible = true;
                    }
                }
            }
            if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
            {

                if (view_header == "1" || view_header == "0")
                {
                    //for (int row_cnt = 0; row_cnt < month_spd.Sheets[0].ColumnHeader.RowCount; row_cnt++)//
                    for (int row_cnt = 0; row_cnt < gview.Columns.Count; row_cnt++)
                    {
                        //month_spd.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;//
                        gview.HeaderRow.Cells[row_cnt].Visible = true;
                    }
                }
                else
                {
                    //for (int row_cnt = 0; row_cnt < month_spd.Sheets[0].ColumnHeader.RowCount; row_cnt++)//
                    for (int row_cnt = 0; row_cnt < gview.Columns.Count; row_cnt++)
                    {
                        //month_spd.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;//
                        gview.HeaderRow.Cells[row_cnt].Visible = true;
                    }
                }

                //for (int i = 0; i < month_spd.Sheets[0].RowCount; i++)//
                for (int i = 0; i < gview.Rows.Count; i++)
                {
                    //month_spd.Sheets[0].Rows[i].Visible = true;//
                    gview.Rows[i].Visible = true;
                }
                Double totalRows = 0;
                //totalRows = Convert.ToInt32(month_spd.Sheets[0].RowCount);//
                //Session["totalPages"] = (int)Math.Ceiling(totalRows / month_spd.Sheets[0].PageSize);//

                totalRows = Convert.ToInt32(gview.Rows.Count);
                Session["totalPages"] = (int)Math.Ceiling(totalRows / gview.PageSize);

                Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                DropDownListpage.Items.Clear();
                if (totalRows >= 10)
                {
                    //month_spd.Sheets[0].PageSize = Convert.ToInt32(totalRows);//
                    gview.PageSize = Convert.ToInt32(totalRows);
                    
                    for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                    {
                        DropDownListpage.Items.Add((k + 10).ToString());
                    }
                    DropDownListpage.Items.Add("Others");
                    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                    //month_spd.Height = 335;//
                    gview.Height = 335;
                }
                else if (totalRows == 0)
                {
                    DropDownListpage.Items.Add("0");
                    //month_spd.Height = 100;//
                    gview.Height = 100;
                }
                else
                {
                    //month_spd.Sheets[0].PageSize = Convert.ToInt32(totalRows);//
                    //DropDownListpage.Items.Add(month_spd.Sheets[0].PageSize.ToString());//
                    //month_spd.Height = 100 + (10 * Convert.ToInt32(totalRows));//

                    gview.PageSize = Convert.ToInt32(totalRows);
                    DropDownListpage.Items.Add(gview.PageSize.ToString());
                    gview.Height = 100 + (10 * Convert.ToInt32(totalRows));
                }
                //if (Convert.ToInt32(month_spd.Sheets[0].RowCount) > 10)//
                if (Convert.ToInt32(gview.Rows.Count) > 10)
                {
                    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                    //month_spd.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);//
                    //month_spd.Height = 100 + (10 * Convert.ToInt32(totalRows));//

                    gview.Height = 100 + (10 * Convert.ToInt32(totalRows));
                    
                    gview.PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);

                    CalculateTotalPages();
                }

                pnl_pageset.Visible = false;
            }
            else
            {
                pnl_pageset.Visible = false;
            }

            if (view_footer_text != "")
            {
                if (view_footer == "0")
                {
                    //month_spd.Sheets[0].Rows[(month_spd.Sheets[0].RowCount - 1)].Visible = true;//
                    //month_spd.Sheets[0].Rows[(month_spd.Sheets[0].RowCount - 2)].Visible = true;//
                    //month_spd.Sheets[0].Rows[(month_spd.Sheets[0].RowCount - 3)].Visible = true;//

                    gview.Rows[(gview.Rows.Count - 1)].Visible = true;
                    gview.Rows[(gview.Rows.Count - 2)].Visible = true;
                    gview.Rows[(gview.Rows.Count - 3)].Visible = true;
                }
                else
                {
                    if (ddlpage.Text != "")
                    {
                        if (ddlpage.SelectedIndex != ddlpage.Items.Count - 1)
                        {
                            //month_spd.Sheets[0].Rows[(month_spd.Sheets[0].RowCount - 1)].Visible = false;//
                            //month_spd.Sheets[0].Rows[(month_spd.Sheets[0].RowCount - 2)].Visible = false;//
                            //month_spd.Sheets[0].Rows[(month_spd.Sheets[0].RowCount - 3)].Visible = false;//

                            gview.Rows[(gview.Rows.Count - 1)].Visible = false;
                            gview.Rows[(gview.Rows.Count - 2)].Visible = false;
                            gview.Rows[(gview.Rows.Count - 3)].Visible = false;
                        }
                    }
                }
            }

            //month_spd.Visible = true;//
            gview.Visible = true;

            ddlpage.Visible = true;
            lblpages.Visible = true;
        }
        else
        {
            //month_spd.Visible = false;//
            gview.Visible = false;
            pnl_pageset.Visible = false;
            lblnorec.Visible = false;
            lblnorec.Text = "No Header and Footer setting Assigned";
        }

    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        string reportname = txtexcelname.Text;
        if (reportname.ToString().Trim() != "")
        {
            d2.printexcelreportgrid(gview, reportname);
        }
        else
        {
            lblnorec.Visible = true;
            lblnorec.Text = "Please Enter Report Name";
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
        //        print = "Congratulations Report" + i;
        //        //month_spd.SaveExcel(appPath + "/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
        //        //Aruna on 26feb2013============================
        //        string szPath = appPath + "/Report/";
        //        string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

        //        month_spd.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
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
        ////ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);

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

    public override void VerifyRenderingInServerForm(Control control)
    { }

    //============Hided by Manikandan 18/05/2013
    //public void setheader_print()
    //{
    //    // month_spd.Sheets[0].RemoveSpanCell
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
    //        for (col_count = 0; col_count < month_spd.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (month_spd.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                // one_column();
    //                more_column();
    //                break;
    //            }
    //        }

    //    }

    //    else if (final_print_col_cnt == 2)
    //    {
    //        for (col_count = 0; col_count < month_spd.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (month_spd.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 0)
    //                {
    //                    start_column = col_count;
    //                    //   month_spd.Sheets[0].ColumnHeader.Columns[col_count].Width = 100;//
    //                    month_spd.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (month_spd.Sheets[0].ColumnHeader.RowCount - 1), 1);
    //                    if (logo_length_left > 0 && logo_length_left.ToString() != "")
    //                    {
    //                        month_spd.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                    }
    //                    month_spd.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                }
    //                else
    //                {
    //                    //  one_column();
    //                    more_column();
    //                    for (int row_cnt = 0; row_cnt < month_spd.Sheets[0].ColumnHeader.RowCount - 2; row_cnt++)
    //                    {
    //                        month_spd.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
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
    //        for (col_count = 0; col_count < month_spd.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (month_spd.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 0)
    //                {
    //                    start_column = col_count;
    //                    //   month_spd.Sheets[0].ColumnHeader.Columns[col_count].Width = 100;//
    //                    month_spd.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (month_spd.Sheets[0].ColumnHeader.RowCount - 1), 1);
    //                    if (logo_length_left > 0 && logo_length_left.ToString() != "")
    //                    {
    //                        month_spd.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                    }
    //                    month_spd.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                }
    //                else if (temp_count == 1)
    //                {
    //                    // one_column();
    //                    more_column();
    //                    for (int row_cnt = 0; row_cnt < month_spd.Sheets[0].ColumnHeader.RowCount - 2; row_cnt++)
    //                    {
    //                        month_spd.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                        month_spd.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                }
    //                else if (temp_count == 2)
    //                {
    //                    if (isonumber != string.Empty)
    //                    {
    //                        month_spd.Sheets[0].ColumnHeader.Cells[0, col_count].Text = "ISO CODE:" + isonumber;
    //                        month_spd.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, (month_spd.Sheets[0].ColumnHeader.RowCount -2), 1);
    //                        if (logo_length > 0 && logo_length.ToString() != "")
    //                        {
    //                            month_spd.Sheets[0].ColumnHeader.Cells[1, col_count].CellType = mi2;
    //                        }
    //                        month_spd.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                    else
    //                    {
    //                        month_spd.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (month_spd.Sheets[0].ColumnHeader.RowCount - 1), 1);
    //                        if (logo_length > 0 && logo_length.ToString() != "")
    //                        {
    //                            month_spd.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi2;
    //                        }
    //                        month_spd.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                    //month_spd.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (month_spd.Sheets[0].ColumnHeader.RowCount - 1), 1);
    //                    //if (logo_length > 0 && logo_length.ToString() != "")
    //                    //{
    //                    //    month_spd.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi2;
    //                    //}
    //                    //month_spd.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
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
    //        for (col_count = 0; col_count < month_spd.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (month_spd.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 0)
    //                {
    //                    start_column = col_count;
    //                    month_spd.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, ((month_spd.Sheets[0].ColumnHeader.RowCount - 1)), 1);
    //                    if (logo_length_left > 0 && logo_length_left.ToString() != "")
    //                    {
    //                        month_spd.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                    }
    //                    month_spd.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                    // month_spd.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
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
    //            month_spd.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Text = "ISO CODE:";// +isonumber;
    //            month_spd.Sheets[0].ColumnHeader.Cells[0, end_column].Text = isonumber;
    //            month_spd.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Border.BorderColorRight = Color.White;
    //            month_spd.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorLeft = Color.White;
    //            month_spd.Sheets[0].ColumnHeader.Cells[0, end_column].HorizontalAlign = HorizontalAlign.Left;
    //            month_spd.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorBottom = Color.White;
    //            month_spd.Sheets[0].ColumnHeaderSpanModel.Add(1, end_column, ((month_spd.Sheets[0].ColumnHeader.RowCount - 2)), 1);
    //            if (logo_length > 0 && logo_length.ToString() != "")
    //            {
    //                month_spd.Sheets[0].ColumnHeader.Cells[1, end_column].CellType = mi2;
    //            }
    //            month_spd.Sheets[0].ColumnHeader.Columns[end_column].Width = 150;
    //            month_spd.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorTop = Color.White;
    //            month_spd.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorBottom = Color.Black;
    //            month_spd.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Border.BorderColorBottom = Color.White;
    //            month_spd.Sheets[0].ColumnHeader.Cells[1, end_column - 1].Border.BorderColorTop = Color.White;
    //            month_spd.Sheets[0].ColumnHeader.Cells[1, end_column - 1].Border.BorderColorBottom = Color.White;
    //            //if (dsprint.Tables[0].Rows.Count > 0)
    //            //{
    //            //    if (dsprint.Tables[0].Rows[0]["header_align_index"].ToString() != "")
    //            //    {
    //            //        month_spd.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorBottom = Color.Black ;
    //            //    }
    //            //}
    //            month_spd.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorLeft = Color.White;

    //            month_spd.Sheets[0].ColumnHeader.Cells[(month_spd.Sheets[0].ColumnHeader.RowCount - 1), end_column].Border.BorderColorTop = Color.Black;
    //        }
    //        else
    //        {
    //            month_spd.Sheets[0].ColumnHeaderSpanModel.Add(0, end_column, ((month_spd.Sheets[0].ColumnHeader.RowCount - 1)), 1);

    //            if (logo_length > 0 && logo_length.ToString() != "")
    //            {
    //                month_spd.Sheets[0].ColumnHeader.Cells[0, end_column].CellType = mi2;
    //            }
    //            month_spd.Sheets[0].ColumnHeader.Columns[end_column].Width = 150;
    //            // month_spd.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorRight = Color.Black;

    //            month_spd.Sheets[0].ColumnHeader.Cells[(month_spd.Sheets[0].ColumnHeader.RowCount - 2), end_column].Border.BorderColorTop = Color.Black;
    //        }
    //            //month_spd.Sheets[0].ColumnHeaderSpanModel.Add(0, end_column, (month_spd.Sheets[0].ColumnHeader.RowCount-1), 1);
    //            //if (logo_length > 0 && logo_length.ToString() != "")
    //            //{
    //            //    month_spd.Sheets[0].ColumnHeader.Cells[0, end_column].CellType = mi2;
    //            //}
    //            //month_spd.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorLeft = Color.White;


    //        temp_count = 0;
    //        for (col_count = 0; col_count < month_spd.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (month_spd.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 1)
    //                {
    //                    more_column();
    //                    for (int row_cnt = 0; row_cnt < month_spd.Sheets[0].ColumnHeader.RowCount - 2; row_cnt++)
    //                    {
    //                        month_spd.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                        month_spd.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
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
    //            month_spd.Sheets[0].RowCount = month_spd.Sheets[0].RowCount + 3;

    //            month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 3), start_column].ColumnSpan = month_spd.Sheets[0].ColumnCount - start_column;
    //            month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 2), start_column].ColumnSpan = month_spd.Sheets[0].ColumnCount - start_column;

    //            month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 3), start_column].Border.BorderColorBottom = Color.White;
    //            month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 2), start_column].Border.BorderColorTop = Color.White;
    //            month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 2), start_column].Border.BorderColorBottom = Color.White;
    //            month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 1), start_column].Border.BorderColorTop = Color.White;


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

    //                for (col_count = 0; col_count < month_spd.Sheets[0].ColumnCount; col_count++)
    //                {
    //                    if (month_spd.Sheets[0].Columns[col_count].Visible == true)
    //                    {
    //                        month_spd.Sheets[0].SpanModel.Add((month_spd.Sheets[0].RowCount - 1), col_count, 1, final_print_col_cnt);
    //                        month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 1), col_count].Text = footer_text;
    //                        month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                        break;
    //                    }
    //                }

    //            }

    //            else if (final_print_col_cnt == footer_count)
    //            {
    //                temp_count = 0;
    //                for (col_count = 0; col_count < month_spd.Sheets[0].ColumnCount; col_count++)
    //                {
    //                    if (month_spd.Sheets[0].Columns[col_count].Visible == true)
    //                    {
    //                        month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                        month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                        month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                        month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
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

    //                for (col_count = 0; col_count < month_spd.Sheets[0].ColumnCount; col_count++)
    //                {
    //                    if (month_spd.Sheets[0].Columns[col_count].Visible == true)
    //                    {
    //                        if (temp_count == 0)
    //                        {
    //                            month_spd.Sheets[0].SpanModel.Add((month_spd.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer + footer_balanc_col);
    //                        }
    //                        else
    //                        {

    //                            month_spd.Sheets[0].SpanModel.Add((month_spd.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer);

    //                        }
    //                        month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                        month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                        if (col_count - 1 >= 0)
    //                        {
    //                            month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorLeft = Color.White;
    //                            month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorRight = Color.White;
    //                        }
    //                        month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                        month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                        if (col_count + 1 < month_spd.Sheets[0].ColumnCount)
    //                        {
    //                            month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorLeft = Color.White;
    //                            month_spd.Sheets[0].Cells[(month_spd.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorRight = Color.White;
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

    //===================================

    //==============Hided by Manikandan 15/05/2013

    //public void more_column()
    //{

    //    header_text();

    //    if (final_print_col_cnt > 3)
    //    {
    //        if (isonumber != string.Empty)
    //        {
    //            month_spd.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count - 1));
    //        }
    //        else
    //        {
    //            month_spd.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count));
    //        }
    //        month_spd.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, 1, (end_column - col_count));
    //        month_spd.Sheets[0].ColumnHeaderSpanModel.Add(2, col_count, 1, (end_column - col_count));
    //        month_spd.Sheets[0].ColumnHeaderSpanModel.Add(3, col_count, 1, (end_column - col_count));
    //        month_spd.Sheets[0].ColumnHeaderSpanModel.Add(4, col_count, 1, (end_column - col_count));
    //        month_spd.Sheets[0].ColumnHeaderSpanModel.Add(5, col_count, 1, (end_column - col_count));
    //        month_spd.Sheets[0].ColumnHeaderSpanModel.Add(6, col_count, 1, (end_column - col_count));

    //    }

    //    if (check_print_row == true && coll_name_new != "")
    //    {
    //        month_spd.Sheets[0].ColumnHeader.Cells[0, col_count].Text = collnamenew1 + address1 + ", " + address2 + ", " + address3 + ", "+state+"-" + pincode + ".";
    //    }
    //    else if (check_print_row == false)
    //    {
    //        month_spd.Sheets[0].ColumnHeader.Cells[0, col_count].Text = collnamenew1 + address1 + ", " + address2 + ", " + address3 + "," + state + "- " + pincode + ".";
    //    }
    //    month_spd.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;  
    //    month_spd.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorTop = Color.White;
    //    month_spd.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorBottom = Color.White;


    //    if (check_print_row == true && address1_new != "")
    //    {
    //        month_spd.Sheets[0].ColumnHeader.Cells[1, col_count].Text = categery + ", Affiliated to " + Affliated + ".";
    //    }
    //    else if (check_print_row==false )
    //    {
    //        month_spd.Sheets[0].ColumnHeader.Cells[1, col_count].Text = categery + ", Affiliated to " + Affliated + ".";
    //    }

    //    if (check_print_row == true && address2_new != "")
    //    {
    //        month_spd.Sheets[0].ColumnHeader.Cells[2, col_count].Text = "CONGRATULATIONS REPORT";
    //    }
    //    else if (check_print_row == false)
    //    {
    //        month_spd.Sheets[0].ColumnHeader.Cells[2, col_count].Text = "CONGRATULATIONS REPORT";
    //    }


    //    if (check_print_row == true && address3_new != "")
    //    {
    //        month_spd.Sheets[0].ColumnHeader.Cells[3, col_count].Text = "The Management, Director, Principal, HOD (" + ddlbranch.SelectedItem.ToString() + "),";
    //         month_spd.Sheets[0].ColumnHeader.Cells[4, col_count].Text = "Faculty Members of " + ddlbranch.SelectedItem.ToString() + " Department Congratulates the";
    //              month_spd.Sheets[0].ColumnHeader.Cells[5, col_count].Text = "following Students for getting 100% attedance during " + date1 + " to " + date2;
    //    }
    //    else if (check_print_row == false)
    //    {
    //        month_spd.Sheets[0].ColumnHeader.Cells[3, col_count].Text = "The Management, Director, Principal, HOD (" + ddlbranch.SelectedItem.ToString() + "),";
    //         month_spd.Sheets[0].ColumnHeader.Cells[4, col_count].Text = "Faculty Members of " + ddlbranch.SelectedItem.ToString() + " Department Congratulates the";
    //         month_spd.Sheets[0].ColumnHeader.Cells[5, col_count].Text = "following Students for getting 100% attedance during " + date1 + " to " + date2;
    //    }


    //    if ((check_print_row == true && phoneno_new  != "") || check_print_row == false)
    //    {

    //        if (ddlsec.SelectedValue.ToString() == "" && ddlsec.SelectedValue.ToString() == null)
    //        {
    //            month_spd.Sheets[0].ColumnHeader.Cells[6, col_count].Text = ddlbranch.SelectedItem.ToString();

    //        }
    //        else
    //        {
    //            month_spd.Sheets[0].ColumnHeader.Cells[6, col_count].Text = ddlbranch.SelectedItem.ToString() + " '" + ddlsec.SelectedValue.ToString() + "'";
    //        }
    //    }


    //   ////-----------------


    //    // 




    //    month_spd.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorTop = Color.White;
    //    month_spd.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorBottom = Color.White;



    //    month_spd.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorTop = Color.White;
    //    month_spd.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorBottom = Color.White;

    //    month_spd.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorTop = Color.White;
    //    month_spd.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorBottom = Color.White;
    //    month_spd.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorTop = Color.White;
    //    month_spd.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorBottom = Color.White;
    //    month_spd.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorTop = Color.White;

    //    int temp_count_temp = 0;
    //    if (dsprint.Tables[0].Rows.Count > 0)
    //    {
    //        if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
    //        {
    //            new_header_string_split = (dsprint.Tables[0].Rows[0]["new_header_name"].ToString()).Split(',');
    //           string[] header_align_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString().Split(',');
    //            month_spd.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorBottom = Color.White;
    //            for (int row_head_count = 7; row_head_count < (7 + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
    //            {
    //                month_spd.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Text = new_header_string_split[temp_count_temp].ToString();
    //                month_spd.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorRight = Color.White;
    //                if (final_print_col_cnt > 3)
    //                {
    //                    month_spd.Sheets[0].ColumnHeaderSpanModel.Add(row_head_count, col_count, 1, (end_column - col_count));
    //                }
    //                month_spd.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorTop = Color.White;
    //                if (row_head_count != (7 + new_header_string_split.GetUpperBound(0)))
    //                {
    //                    month_spd.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorBottom = Color.White;
    //                }

    //                if (header_align_index.GetUpperBound(0) >= temp_count_temp)
    //                {
    //                    if (header_align_index[temp_count_temp] != string.Empty)
    //                    {
    //                        header_alignment = header_align_index[temp_count_temp].ToString();
    //                        if (header_alignment == "2")
    //                        {
    //                            month_spd.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Center;
    //                        }
    //                        else if (header_alignment == "1")
    //                        {
    //                            month_spd.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Left;
    //                        }
    //                        else
    //                        {
    //                            month_spd.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Right;
    //                        }
    //                    }
    //                }
    //                temp_count_temp++;
    //            }
    //        }
    //    }
    //}
    //public void header_text()
    //{    

    //    SqlDataReader dr_collinfo;
    //    con.Close();
    //    con.Open();
    //    cmd = new SqlCommand("select isnull(college_name,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website,isnull(form_heading_name,'') as form_name,isnull(batch_degree_branch,'') as degree_deatil,isnull(header_align,'') as header_alignment,isnull(header_flag_value,'') as view_header from print_master_setting  where form_name='congrats.aspx'", con);
    //    dr_collinfo = cmd.ExecuteReader();
    //    while (dr_collinfo.Read())
    //    {
    //        if (dr_collinfo.HasRows == true)
    //        {                
    //            check_print_row = true;
    //            coll_name_new = dr_collinfo["collname"].ToString();
    //            address1_new = dr_collinfo["address1"].ToString();
    //            address2_new = dr_collinfo["address2"].ToString();
    //            address3_new = dr_collinfo["address3"].ToString();
    //            phoneno_new = dr_collinfo["phoneno"].ToString();              
    //            con.Close();
    //            con.Open();
    //            // cmd = new SqlCommand("select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website from collinfo  where college_code=" + Session["collegecode"] + "", con);
    //            cmd = new SqlCommand("select isnull(collname, ' ') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode,isnull(logo1,'') as logo1,isnull(logo2,'') as logo2,isnull(state,'') as state from collinfo where college_code='" + Session["collegecode"].ToString() + "'", con);
    //            dr_collinfo = cmd.ExecuteReader();
    //            while (dr_collinfo.Read())
    //            {
    //                if (dr_collinfo.HasRows == true)
    //                {
    //                    collnamenew1 = dr_collinfo["collname"].ToString();
    //                    address1 = dr_collinfo["address1"].ToString();
    //                    address2 = dr_collinfo["address2"].ToString();
    //                    address3 = dr_collinfo["address3"].ToString();
    //                    pincode = dr_collinfo["pincode"].ToString();
    //                    categery = dr_collinfo["category"].ToString();
    //                    state = dr_collinfo["state"].ToString();
    //                    Affliated = dr_collinfo["affliated"].ToString();
    //                }

    //            }
    //        }
    //    }
    //    if (check_print_row == false)
    //    {

    //        con.Close();
    //        con.Open();
    //       // cmd = new SqlCommand("select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website from collinfo  where college_code=" + Session["collegecode"] + "", con);
    //        cmd = new SqlCommand("select isnull(collname, ' ') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode,isnull(logo1,'') as logo1,isnull(logo2,'') as logo2,isnull(state,'') as state from collinfo where college_code='" + Session["collegecode"].ToString() + "'", con);
    //        dr_collinfo = cmd.ExecuteReader();
    //        while (dr_collinfo.Read())
    //        {
    //            if (dr_collinfo.HasRows == true)
    //            {
    //                collnamenew1 = dr_collinfo["collname"].ToString();
    //                address1 = dr_collinfo["address1"].ToString();
    //                address2 = dr_collinfo["address2"].ToString();
    //                address3 = dr_collinfo["address3"].ToString();
    //                pincode = dr_collinfo["pincode"].ToString();
    //                categery = dr_collinfo["category"].ToString();
    //                Affliated = dr_collinfo["affliated"].ToString();
    //                state = dr_collinfo["state"].ToString();
    //            }

    //        }
    //    }
    //}

    //===================================

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

    //============Hided by Manikandan 18/05/2013

    public void view_header_setting()
    {
        if (dsprint.Tables[0].Rows.Count > 0)
        {

            ddlpage.Visible = true;
            lblpages.Visible = true;

            view_footer = dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString();
            view_header = dsprint.Tables[0].Rows[0]["header_flag_value"].ToString();
            view_footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
            if (view_header == "0" || view_header == "1")
            {
                lblnorec.Visible = false;

                #region sprd
                //for (int row_cnt = 0; row_cnt < month_spd.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                //{
                //    month_spd.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
                //}
                #endregion

                int i = 0;
                ddlpage.Items.Clear();
                //int totrowcount = month_spd.Sheets[0].RowCount;//
                int totrowcount = gview.Rows.Count;
                int pages = totrowcount / 25;
                int intialrow = 1;
                int remainrows = totrowcount % 25;
                //if (month_spd.Sheets[0].RowCount > 0)//
                if (gview.Rows.Count > 0)
                {
                    int i5 = 0;
                    ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
                    for (i = 1; i <= pages; i++)
                    {
                        i5 = i;

                        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                        intialrow = intialrow + 25;
                    }
                    if (remainrows > 0)
                    {
                        i = i5 + 1;
                        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                    }
                }
                if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
                {
                    //for (i = 0; i < month_spd.Sheets[0].RowCount; i++)//
                    for (i = 0; i < gview.Rows.Count; i++)
                    {
                        //month_spd.Sheets[0].Rows[i].Visible = true;//
                        gview.Rows[i].Visible = true;
                    }
                    Double totalRows = 0;
                    //totalRows = Convert.ToInt32(month_spd.Sheets[0].RowCount);//
                    //Session["totalPages"] = (int)Math.Ceiling(totalRows / month_spd.Sheets[0].PageSize);//

                    totalRows = Convert.ToInt32(gview.Rows.Count);                    
                    Session["totalPages"] = (int)Math.Ceiling(totalRows / gview.PageSize);

                    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                    DropDownListpage.Items.Clear();
                    if (totalRows >= 10)
                    {
                        //month_spd.Sheets[0].PageSize = Convert.ToInt32(totalRows);//
                        gview.PageSize = Convert.ToInt32(totalRows);
                        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                        {
                            DropDownListpage.Items.Add((k + 10).ToString());
                        }
                        DropDownListpage.Items.Add("Others");
                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        //month_spd.Height = 335;//
                        gview.Height = 335;

                    }
                    else if (totalRows == 0)
                    {
                        DropDownListpage.Items.Add("0");
                        //month_spd.Height = 100;//
                        gview.Height = 100;
                    }
                    else
                    {
                        //month_spd.Sheets[0].PageSize = Convert.ToInt32(totalRows);//
                        //DropDownListpage.Items.Add(month_spd.Sheets[0].PageSize.ToString());//
                        //month_spd.Height = 100 + (10 * Convert.ToInt32(totalRows));//

                        gview.PageSize = Convert.ToInt32(totalRows);
                        DropDownListpage.Items.Add(gview.PageSize.ToString());
                        gview.Height = 100 + (10 * Convert.ToInt32(totalRows));
                    }
                    //if (Convert.ToInt32(month_spd.Sheets[0].RowCount) > 10)//
                    if (Convert.ToInt32(gview.Rows.Count) > 10)
                    {
                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        //month_spd.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);//
                        //month_spd.Height = 100 + (10 * Convert.ToInt32(totalRows));//
                        CalculateTotalPages();

                        gview.PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                        gview.Height = 100 + (10 * Convert.ToInt32(totalRows));
                    }


                    pnl_pageset.Visible = false;


                }
                else
                {
                    lblnorec.Visible = false;
                    pnl_pageset.Visible = false;
                }
            }
            else if (view_header == "2")
            {
                #region sprd
                //for (int row_cnt = 0; row_cnt < month_spd.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                ////for (int row_cnt = 0; row_cnt < gview.Columns.Count;row_cnt++ )
                //{
                //    month_spd.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
                //}
                #endregion

                lblnorec.Visible = false;
                int i = 0;
                ddlpage.Items.Clear();
                //int totrowcount = month_spd.Sheets[0].RowCount;//
                int totrowcount = gview.Rows.Count;
                int pages = totrowcount / 25;
                int intialrow = 1;
                int remainrows = totrowcount % 25;
                //if (month_spd.Sheets[0].RowCount > 0)//
                if (gview.Rows.Count > 0)
                {
                    int i5 = 0;
                    ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
                    for (i = 1; i <= pages; i++)
                    {
                        i5 = i;

                        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                        intialrow = intialrow + 25;
                    }
                    if (remainrows > 0)
                    {
                        i = i5 + 1;
                        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                    }
                }
                if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
                {
                    //for (i = 0; i < month_spd.Sheets[0].RowCount; i++)//
                    for (i = 0; i < gview.Rows.Count; i++)
                    {
                        //month_spd.Sheets[0].Rows[i].Visible = true;//
                        gview.Rows[i].Visible = true;
                    }
                    Double totalRows = 0;
                    //totalRows = Convert.ToInt32(month_spd.Sheets[0].RowCount);//
                    //Session["totalPages"] = (int)Math.Ceiling(totalRows / month_spd.Sheets[0].PageSize);//

                    totalRows = Convert.ToInt32(gview.Rows.Count);
                    Session["totalPages"] = (int)Math.Ceiling(totalRows / gview.PageSize);

                    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                    DropDownListpage.Items.Clear();
                    if (totalRows >= 10)
                    {
                        //month_spd.Sheets[0].PageSize = Convert.ToInt32(totalRows);//
                        gview.PageSize = Convert.ToInt32(totalRows);
                        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                        {
                            DropDownListpage.Items.Add((k + 10).ToString());
                        }
                        DropDownListpage.Items.Add("Others");
                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        //month_spd.Height = 335;//
                        gview.Height = 335;

                    }
                    else if (totalRows == 0)
                    {
                        DropDownListpage.Items.Add("0");
                        //month_spd.Height = 100;//
                        gview.Height = 100;
                    }
                    else
                    {
                        //month_spd.Sheets[0].PageSize = Convert.ToInt32(totalRows);//
                        //DropDownListpage.Items.Add(month_spd.Sheets[0].PageSize.ToString());//
                        //month_spd.Height = 100 + (10 * Convert.ToInt32(totalRows));//

                        gview.PageSize = Convert.ToInt32(totalRows);
                        DropDownListpage.Items.Add(gview.PageSize.ToString());
                        gview.Height = 100 + (10 * Convert.ToInt32(totalRows));
                    }
                    //if (Convert.ToInt32(month_spd.Sheets[0].RowCount) > 10)//
                    if (Convert.ToInt32(gview.Rows.Count) > 10)
                    {
                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        //month_spd.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);//
                        gview.PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                        //  month_spd.Height = 100 + (10 * Convert.ToInt32(totalRows));
                        CalculateTotalPages();
                    }
                    pnl_pageset.Visible = false;
                }
                else
                {
                    pnl_pageset.Visible = false;
                }
            }
            else
            {

            }
            lblpages.Visible = true;
            ddlpage.Visible = true;
        }
        else
        {
            lblpages.Visible = false;
            ddlpage.Visible = false;
        }
    }

    //=============================

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ddlpage.Visible = true;
        lblpages.Visible = true;
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

        Session["page_redirect_value"] = ddlbatch.SelectedIndex + "," + ddldegree.SelectedIndex + "," + ddlbranch.SelectedIndex + "," + sem_index + "," + sec_index + "," + txtFromDate.Text + "," + txtToDate.Text;

        // first_btngo();
        btnGo_Click(sender, e);

        lblpages.Visible = true;
        ddlpage.Visible = true;
        string clmnheadrname = "";
        //int total_clmn_count = month_spd.Sheets[0].ColumnCount;//
        int total_clmn_count = gview.Columns.Count;

        for (int srtcnt = 0; srtcnt < total_clmn_count; srtcnt++)
        {
            //if (month_spd.Sheets[0].Columns[srtcnt].Visible == true)//
            //if (gview.Columns[srtcnt].Visible == true)
            //{
            //    if (month_spd.Sheets[0].ColumnHeader.Cells[month_spd.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text != "")
            //    {
            //        subcolumntext = "";
            //        if (clmnheadrname == "")
            //        {
            //            clmnheadrname = month_spd.Sheets[0].ColumnHeader.Cells[month_spd.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
            //        }
            //        else
            //        {
            //            if (child_flag == false)
            //            {
            //                clmnheadrname = clmnheadrname + "," + month_spd.Sheets[0].ColumnHeader.Cells[month_spd.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
            //            }
            //            else
            //            {
            //                clmnheadrname = clmnheadrname + "$)," + month_spd.Sheets[0].ColumnHeader.Cells[month_spd.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
            //            }

            //        }
            //        child_flag = false;
            //    }

            //}
        }
        Response.Redirect("Print_Master_Setting_new.aspx?ID=" + clmnheadrname.ToString() + ":" + "congrats.aspx" + ":" + ddlbatch.SelectedItem.ToString() + " Batch - " + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] " + strsec + " :" + "Congratulation Report");

    }

    public void print_btngo()
    {
        final_print_col_cnt = 0;
        lblnorec.Visible = false;
        check_col_count_flag = false;

        //month_spd.Sheets[0].SheetCorner.RowCount = 0;
        //month_spd.Sheets[0].ColumnCount = 0;
        //month_spd.Sheets[0].RowCount = 0;
        //month_spd.Sheets[0].SheetCorner.RowCount = 8;
        //month_spd.Sheets[0].ColumnCount = 5;


        hat.Clear();
        hat.Add("college_code", Session["collegecode"].ToString());
        hat.Add("form_name", "congrats.aspx");
        dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        if (dsprint.Tables[0].Rows.Count > 0)
        {
            lblpages.Visible = true;
            ddlpage.Visible = true;

            //3. header add
            //if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
            //{
            //    new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
            //    new_header_string_split = new_header_string.Split(',');
            //    month_spd.Sheets[0].SheetCorner.RowCount = month_spd.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
            //}
            //3. end header add


            btnclick();



            //1.set visible columns
            column_field = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------------get column field
            if (column_field != "" && column_field != null)
            {
                //  check_col_count_flag = true;

                //for (col_count_all = 0; col_count_all < month_spd.Sheets[0].ColumnCount; col_count_all++)//
                for (col_count_all = 0; col_count_all < gview.Columns.Count; col_count_all++)
                {
                    //month_spd.Sheets[0].Columns[col_count_all].Visible = false;//------------invisible all column
                    gview.Columns[col_count_all].Visible = false;
                }


                printvar = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------visible setting columns
                string[] split_printvar = printvar.Split(',');
                for (int splval = 0; splval <= split_printvar.GetUpperBound(0); splval++)
                {
                    span_cnt = 0;
                    string[] split_star = split_printvar[splval].Split('*');
                    //if (split_star.GetUpperBound(0) > 0)
                    //{
                    //    for (col_count = 0; col_count < month_spd.Sheets[0].ColumnCount - 1; col_count++)
                    //    {
                    //        if (month_spd.Sheets[0].ColumnHeader.Cells[(month_spd.Sheets[0].ColumnHeader.RowCount - 1), col_count].Text == split_star[0])
                    //        {
                    //            child_span_count = 0;

                    //            string[] split_star_doller = split_star[1].Split('$');
                    //            for (int doller_count = 1; doller_count < split_star_doller.GetUpperBound(0); doller_count++)
                    //            {
                    //                for (int child_node = col_count; child_node <= col_count + split_star_doller.GetUpperBound(0); child_node++)
                    //                {
                    //                    if (month_spd.Sheets[0].ColumnHeader.Cells[(month_spd.Sheets[0].ColumnHeader.RowCount - 1), child_node].Text == split_star_doller[doller_count])
                    //                    {
                    //                        span_cnt++;
                    //                        if (span_cnt == 1 && child_node == col_count + 1)
                    //                        {
                    //                            month_spd.Sheets[0].ColumnHeader.Cells[(month_spd.Sheets[0].ColumnHeader.RowCount - 2), col_count + 1].Text = split_star[0].ToString();
                    //                            col_count++;
                    //                        }

                    //                        if (child_node != col_count)
                    //                        {
                    //                            span_cnt = child_node - (child_span_count - 1);
                    //                        }
                    //                        else
                    //                        {
                    //                            child_span_count = col_count;
                    //                        }


                    //                        month_spd.Sheets[0].ColumnHeaderSpanModel.Add((month_spd.Sheets[0].ColumnHeader.RowCount - 2), col_count, 1, span_cnt);

                    //                        month_spd.Sheets[0].Columns[child_node].Visible = true;

                    //                        final_print_col_cnt++;
                    //                        if (span_cnt == split_star_doller.GetUpperBound(0) - 1)
                    //                        {
                    //                            break;
                    //                        }

                    //                    }
                    //                }
                    //            }

                    //        }
                    //    }
                    //}
                    //  else
                    {
                        #region prnt_spred
                        //for (col_count = 0; col_count < month_spd.Sheets[0].ColumnCount; col_count++)//
                        //for (col_count = 0; col_count < gview.Columns.Count; col_count++)
                        //{
                        //    if (month_spd.Sheets[0].ColumnHeader.Cells[(month_spd.Sheets[0].ColumnHeader.RowCount - 1), col_count].Text == split_printvar[splval])
                        //    {
                        //        //month_spd.Sheets[0].Columns[col_count].Visible = true;//
                        //        gview.Columns[col_count].Visible = true;
                        //        final_print_col_cnt++;
                        //        break;
                        //    }
                        //}
                        #endregion
                    }
                }
                //1 end.set visible columns
            }
            else
            {
                //month_spd.Visible = false;//
                gview.Visible = false;
                pnl_pageset.Visible = false;
                lblpages.Visible = false;
                ddlpage.Visible = false;
                lblnorec.Visible = true;
                lblnorec.Text = "Select Atleast One Column Field From The Treeview";
            }
        }
        // month_spd.Width = final_print_col_cnt * 100;
    }
    public void getspecial_hr()
    {
        string strsplsec = "";

        if (ddlsec.SelectedValue.ToString() == "All" || ddlsec.SelectedValue.ToString() == string.Empty || ddlsec.SelectedValue.ToString() == "-1")
        {
            strsplsec = "";
        }
        else
        {
            strsplsec = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
        }

        con_splhr_query_master.Close();
        con_splhr_query_master.Open();
        DataSet ds_splhr_query_master = new DataSet();
        //  no_stud_flag = false;
        //string splhr_query_master = "select attendance from specialhr_attendance sa,registration r where hrdet_no =(select hrdet_no from specialhr_details where hrentry_no in(select hrentry_no from  specialhr_master where batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and date='" + dumm_from_date + "')  ) and r.roll_no=sa.roll_no and batch_year=" + ddlbatch.SelectedValue.ToString() + " and current_semester=" + ddlduration.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + "  and (CC = 0)  AND (DelFlag = 0)  AND (Exam_Flag <> 'debar') and sa.roll_no='" + roll + "'  order by r.roll_no asc";
        string splhr_query_master = "select attendance from specialhr_attendance sa,registration r where hrdet_no in(select hrdet_no from specialhr_details where hrentry_no in(select hrentry_no from  specialhr_master where batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and date='" + dumm_from_date + "' " + strsplsec + ")  ) and r.roll_no=sa.roll_no and batch_year=" + ddlbatch.SelectedValue.ToString() + " and current_semester=" + ddlduration.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + "  and (CC = 0)  AND (DelFlag = 0)  AND (Exam_Flag <> 'debar') and sa.roll_no='" + roll + "'  order by r.roll_no asc";
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
                    tot_conduct_hr_spl++;
                }

            }
        }

        per_abshrs_spl_fals = per_abshrs_spl;
        tot_per_hrs_spl_fals = tot_per_hrs_spl;
        per_leave_fals = per_leave;
        tot_conduct_hr_spl_fals = tot_conduct_hr_spl;
        tot_ondu_spl_fals = tot_ondu_spl;


    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {

        
        Session["column_header_row_count"] = Convert.ToString(gview.Columns.Count);
        string degreedetails = string.Empty;

        string deg_acronym = "select acronym from degree where degree_code=" + ddlbranch.SelectedItem.Value + "";
        SqlDataAdapter da_acronym = new SqlDataAdapter(deg_acronym, con);
        DataTable dt_acronym = new DataTable();
        da_acronym.Fill(dt_acronym);

        string selected_sec = string.Empty;
        if (ddlsec.Text != "")
        {
            selected_sec = "-Sec-" + ddlsec.SelectedItem.ToString() + "";
        }
        else
        {
            selected_sec = "";
        }
        // degreedetails = "CONGRATULATIONS REPORT The Management, Deirector, Principal, HOD (" + ddlbranch.SelectedItem.ToString() + "), Faculty Members of " + ddlbranch.SelectedItem.ToString() + " Department Congratulates the following Students for getting 100% attendance during to " + ddlbranch.SelectedItem.ToString() + selected_sec + "@Degree: " + ddlbatch.SelectedItem.Text.ToString() + "-" + ddldegree.SelectedItem.Text.ToString() + " [" + dt_acronym.Rows[0][0].ToString() + "]- Sem " + ddlduration.SelectedItem.Text.ToString() + "-" + ddlsec.SelectedItem.Text.ToString() + " Sec";

        degreedetails = "Congratulation Report" + "@Degree: " + ddlbatch.SelectedItem.Text.ToString() + "-" + ddldegree.SelectedItem.Text.ToString() + "-" + dt_acronym.Rows[0][0].ToString() + "- Sem -" + ddlduration.SelectedItem.Text.ToString() + selected_sec + "";
        string pagename = "StudentTestReport.aspx";
        
        string ss = null;
        NEWPrintMater1.loadspreaddetails(gview, pagename, degreedetails, 0, ss);
        NEWPrintMater1.Visible = true;
    }
}



//==========MANIPRABHA A.
using System;//============modified 30/3/12(len(r_no), todate var), 31/3/12(halfday holiday), 14/5/12(no rec msg d/p
//=========================, roll series hardcode remove),25/5/12(notconsider_value=0),26/5/12(issue)
//============================
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Reflection;
using System.Drawing;

public partial class Attendance : System.Web.UI.Page
{
    

    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection readcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con4a = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection bind_con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd;
    static Boolean forschoolsetting = false;// Added by sridharan
    SqlCommand cmd3a;
    SqlCommand studinfocmd;
    SqlDataReader studinfors;
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
    Hashtable hat = new Hashtable();
    DAccess2 dacces2 = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds5 = new DataSet();
    DataSet ds6 = new DataSet();
    int mmyycount = 0;
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    //'----------------------------------------------------------new 
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0;
    int notconsider_value = 0;
    //'----------------------------------------------------------new 
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    string split_holiday_status_1 = "", split_holiday_status_2 = "";
    string value_holi_status = "";
    string[] split_holiday_status = new string[1000];
    int conducted_hrs_new = 0;
    Hashtable has_holi = new Hashtable();
    //----------------

    //==============0n 16/4/12 PRABHA
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
    //---------------------------
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
    double tot_ondu, per_tot_ondu, cum_tot_ondu;
    int countds = 0;
    //-----------------------------------------end
    string roll = "";
    string group_code = "", columnfield = "";
    SqlConnection con_all = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    static string grouporusercode = "";
    //Added By Srinath 26/2/2013
    string tempdegreesem = "";
    string chkdegreesem = "";
    string tempdegreesemchk = "";
    //Added By Srinath 3/4/2013
    int frommonthyear = 0;
    static Boolean splhr_flag = false;
    Hashtable ht_sphr = new Hashtable();
    DataSet ds_sphr = new DataSet();
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0, tot_ml_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int spl_per_abshrs_spl = 0, spl_tot_per_hrs_spl = 0, spl_tot_conduct_hr_spl_fals = 0, spl_tot_ondu_spl = 0, spl_tot_ml_spl = 0, spl_tot_conduct_hr_spl = 0;

    DataTable dtl = new DataTable();//added by rajasekar 07/09/2018
    DataRow dtrow = null;//added by rajasekar 07/09/2018

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        lblnorec.Visible = false;
        if (!IsPostBack)
        {
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            Session["QueryString"] = "";
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
            dsprint = dacces2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                btnGo.Enabled = true;
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                ddlcollege.DataSource = dsprint;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
                ddlcollege_SelectedIndexChanged(sender, e);
            }
            else
            {
                ddlcollege.Enabled = false;
                btnGo.Enabled = false;
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
            }


            Pageload(sender, e);
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
                    Label4.Text = "School";
                    //lblbatch.Text = "Year";
                    //lbldeg.Text = "School Type";
                    //lblbranch.Text = "Standard";
                    //lblDuration.Text = "Term";
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
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        btnxl.Visible = false;
        Showgrid.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        //pnl_pagesetting.Visible = false;
        frmlbl.Visible = false;
        tolbl.Visible = false;
        tofromlbl.Visible = false;
        lblnorec.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        if (TextBox1.Text != null && TextBox1.Text != "")
        {
            if (int.Parse(TextBox1.Text) >= 0 && int.Parse(TextBox1.Text) <= 100)
            {
                Label3.Visible = false;
            }
            else
            {
                Label3.Visible = true;
                Label3.Text = "From Percentage Must be 0 to 100";
                TextBox1.Text = "";
            }
        }


    }
    protected void TextBox2_TextChanged(object sender, EventArgs e)
    {
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        btnxl.Visible = false;
        Showgrid.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        //pnl_pagesetting.Visible = false;
        frmlbl.Visible = false;
        tolbl.Visible = false;
        tofromlbl.Visible = false;
        lblnorec.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;

        if (TextBox2.Text != null && TextBox2.Text != "")
        {

            if (int.Parse(TextBox2.Text) >= 0 && int.Parse(TextBox2.Text) <= 100)
            {
                Label3.Visible = false;
            }
            else
            {
                Label3.Visible = true;
                Label3.Text = "To Percentage Must be 0 to 100";
                TextBox2.Text = "";
            }
        }
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        //------------------------------------------date validation-------------------------------
        try
        {

            btnPrint11();
            txtexcelname.Text = "";
            Showgrid.Visible = false;
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            frmlbl.Visible = false;
            tolbl.Visible = false;
            tofromlbl.Visible = false;
            lblnorec.Visible = false;

            btnclick();

            int temp_col = 0;
            if (Showgrid.Rows.Count > 0)//===========on 9/4/12
            {
                
                
            }
            else
            {
                
                Showgrid.Visible = false;
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                lblnorec.Visible = true;
            }
            
        }
        catch
        {
        }

    }


    public void btnclick()
    {
        try
        {
            if (TextBox1.Text != "" && TextBox2.Text != "")
            {
                if (Convert.ToDouble(TextBox1.Text) <= Convert.ToDouble(TextBox2.Text))
                {
                    string valfromdate = "";
                    string valtodate = "";
                    string frmconcat = "";


                    valfromdate = txtFromDate.Text.ToString();
                    string[] split = valfromdate.Split(new char[] { '/' });
                    frmconcat = split[1].ToString() + '/' + split[0].ToString() + '/' + split[2].ToString();
                    DateTime dtfromdate = Convert.ToDateTime(frmconcat.ToString());

                    valtodate = txtToDate.Text.ToString();
                    string[] split2 = valtodate.Split(new char[] { '/' });
                    frmconcat = split2[1].ToString() + '/' + split2[0].ToString() + '/' + split2[2].ToString();
                    DateTime dttodate = Convert.ToDateTime(frmconcat.ToString());

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
                                    if (days < 0)//-----check date difference
                                    {

                                        lblnorec.Text = "From Date Must Be Less Than To Date";
                                        lblnorec.Visible = true;
                                        Showgrid.Visible = false;
                                        btnprintmaster.Visible = false;
                                        btnPrint.Visible = false;
                                        tofromlbl.Visible = false;
                                        

                                    }
                                    else
                                    {
                                        lblnorec.Text = "";
                                        lblnorec.Visible = false;
                                        Showgrid.Visible = true;
                                        btnprintmaster.Visible = true;
                                        btnPrint.Visible = true;
                                        gobutton();
                                        if (Convert.ToInt32(Showgrid.Rows.Count) > 0)
                                        {
                                            
                                            Showgrid.Visible = true;
                                            btnprintmaster.Visible = true;
                                            btnPrint.Visible = true;
                                            btnxl.Visible = true;
                                            txtexcelname.Visible = true;
                                            lblrptname.Visible = true;
                                            
                                            //FpEntry.Height = 350;
                                        }
                                        else
                                        {
                                            
                                            Showgrid.Visible = false;
                                            btnprintmaster.Visible = false;
                                            btnPrint.Visible = false;
                                            txtexcelname.Visible = false;
                                            btnxl.Visible = false;
                                            lblrptname.Visible = false;
                                            lblnorec.Visible = true;
                                            lblnorec.Text = "No Records Found";
                                        }
                                    }
                                }
                                else
                                {

                                    
                                    Showgrid.Visible = false;
                                    btnprintmaster.Visible = false;
                                    btnPrint.Visible = false;
                                    frmlbl.Visible = false;
                                    tolbl.Visible = true;
                                    tofromlbl.Visible = false;
                                    lblnorec.Visible = false;
                                    tolbl.Text = "Enter Valid To Date";
                                }
                            }
                            else
                            {

                                
                                Showgrid.Visible = false;
                                btnprintmaster.Visible = false;
                                btnPrint.Visible = false;
                                frmlbl.Visible = false;
                                tolbl.Visible = true;
                                tofromlbl.Visible = false;
                                lblnorec.Visible = false;
                                tolbl.Text = "Enter Valid To Date";
                            }
                        }
                        else
                        {

                            
                            Showgrid.Visible = false;
                            btnprintmaster.Visible = false;
                            btnPrint.Visible = false;
                            frmlbl.Visible = true;
                            tolbl.Visible = false;
                            tofromlbl.Visible = false;
                            lblnorec.Visible = false;
                            frmlbl.Text = "Enter Valid From Date";
                        }
                    }
                    else
                    {

                        
                        Showgrid.Visible = false;
                        btnprintmaster.Visible = false;
                        btnPrint.Visible = false;
                        frmlbl.Visible = true;
                        tolbl.Visible = false;
                        tofromlbl.Visible = false;
                        lblnorec.Visible = false;
                        frmlbl.Text = "Enter Valid From Date";
                    }

                    if (Convert.ToInt32(Showgrid.Rows.Count) == 0)
                    {
                        
                        Showgrid.Visible = false;
                        btnprintmaster.Visible = false;
                        btnPrint.Visible = false;
                    }
                    else
                    {
                        lblnorec.Visible = false;
                        Showgrid.Visible = true;
                        btnprintmaster.Visible = true;
                        btnPrint.Visible = true;
                    }
                }
                else
                {
                    Label3.Text = "From Percentage Must Be Less than To Percentage";
                    Label3.Visible = true;
                }
            }
            else
            {
                Label3.Text = "Give The Percentage";
                Label3.Visible = true;
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                Showgrid.Visible = false;
            }
        }
        catch
        {
        }

    }
    public void gobutton()
    {
        try
        {
            

            
            string date1 = "", date2 = "";
            string datefrom, dateto;
            string bind_sql = "";
            int row_cnt = 0;
            Boolean sflag = false;
            //FpEntry.Height = 1500;
            string dum_tage_hrs = "";
            int s_no = 0;
            /*****************************************/
            
            Showgrid.Visible = true;
            btnprintmaster.Visible = true;
            btnPrint.Visible = true;
            /*****************************************/



            
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


                                //    logoset();




                                /******************************/
                                //=============================0n 9/4/12
                                //hat.Clear();
                                //hat.Add("college_code", Session["InternalCollegeCode"].ToString());
                                //hat.Add("form_name", "Attendance.aspx");
                                //dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
                                ////===========================================


                                ////======================0n 11/4/12 PRABHA
                                //if (dsprint.Tables[0].Rows.Count > 0)
                                //{
                                //    if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
                                //    {
                                //        FpEntry.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorBottom = Color.White;
                                //        new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                                //        new_header_string_split = new_header_string.Split(',');
                                //        FpEntry.Sheets[0].SheetCorner.RowCount = FpEntry.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
                                //    }
                                //}
                                ////=====================================

                                //added by rajasekar 07/09/2018

                                dtrow = dtl.NewRow();
                                dtl.Rows.Add(dtrow);
                                int colu = 0;

                                dtl.Columns.Add("S.No", typeof(string));
                                dtl.Rows[0][colu] = "S.No";
                                colu++;

                                if (forschoolsetting == true)
                                {


                                    dtl.Columns.Add("Year", typeof(string));

                                    dtl.Rows[0][colu] = "Year";
                                    colu++;

                                    dtl.Columns.Add("Standard", typeof(string));
                                    dtl.Rows[0][colu] = "Standard";
                                    colu++;

                                    dtl.Columns.Add("Term", typeof(string));
                                    dtl.Rows[0][colu] = "Term";
                                    colu++;

                                }
                                else
                                {

                                    dtl.Columns.Add("Batch Year", typeof(string));
                                    dtl.Rows[0][colu] = "Batch Year";
                                    colu++;

                                    dtl.Columns.Add("Degree-Dept", typeof(string));
                                    dtl.Rows[0][colu] = "Degree-Dept";
                                    colu++;

                                    dtl.Columns.Add("Semester", typeof(string));
                                    dtl.Rows[0][colu] = "Semester";
                                    colu++;

                                    

                                }



                                dtl.Columns.Add("Roll No", typeof(string));
                                dtl.Rows[0][colu] = "Roll No";
                                colu++;


                                dtl.Columns.Add("Name of The Student", typeof(string));
                                dtl.Rows[0][colu] = "Name of The Student";
                                colu++;


                                dtl.Columns.Add("Cond Hrs", typeof(string));
                                dtl.Rows[0][colu] = "Cond Hrs";
                                colu++;


                                dtl.Columns.Add("Atten Hrs", typeof(string));
                                dtl.Rows[0][colu] = "Atten Hrs";
                                colu++;


                                dtl.Columns.Add("Cond Days", typeof(string));
                                dtl.Rows[0][colu] = "Cond Days";
                                colu++;


                                dtl.Columns.Add("Atten Days", typeof(string));
                                dtl.Rows[0][colu] = "Atten Days";
                                colu++;


                                dtl.Columns.Add("Attendance %", typeof(string));
                                dtl.Rows[0][colu] = "Attendance %";
                                colu++;



                                
                                //'---------------------------------------------------------
                                //Calculate Month_year=============
                                //Added By Srinath 26/2/2013 =====Start
                                double t1 = Convert.ToDouble(TextBox1.Text);
                                double t2 = Convert.ToDouble(TextBox2.Text);


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
                                frommonthyear = cal_from_date;
                                string querymonth_year = "";
                                for (int monthyear = cal_from_date; monthyear <= cal_to_date; monthyear++)
                                {
                                    if (querymonth_year == "")
                                    {
                                        querymonth_year = monthyear.ToString();
                                    }
                                    else
                                    {
                                        querymonth_year = querymonth_year + ',' + monthyear.ToString();
                                    }
                                }
                                querymonth_year = "and a.month_year in (" + querymonth_year + ")";
                                //End==================================================================

                                bind_con.Close();
                                bind_con.Open();
                                //  bind_sql = "select distinct r.roll_no,r.stud_name,r.degree_code,d.acronym,r.batch_year,r.current_semester,c.course_id,c.course_name,len(r.roll_no) from attendance a,registration r,course c,degree d where r.roll_no=a.roll_no and cc=0 and delflag=0 and exam_flag<>'debar' and c.course_id=d.course_id and r.degree_code=d.degree_code and r.roll_no like '%MER%' order by  len(r.roll_no),r.degree_code,r.batch_year,r.current_semester,c.course_id";
                                //bind_sql = "select distinct r.roll_no,r.stud_name,r.degree_code,d.acronym,r.batch_year,r.current_semester,c.course_id,c.course_name,len(r.roll_no) from attendance a,registration r,course c,degree d where r.roll_no=a.roll_no and cc=0 and delflag=0 and exam_flag<>'debar' and c.course_id=d.course_id and r.degree_code=d.degree_code  and d.college_code=" + Session["InternalCollegeCode"].ToString() + " order by  len(r.roll_no),r.degree_code,r.batch_year,r.current_semester,c.course_id";
                                bind_sql = "select distinct r.roll_no,r.stud_name,r.degree_code,d.acronym,r.batch_year,r.current_semester,c.course_id,c.course_name,len(r.roll_no) from attendance a,registration r,course c,degree d where r.roll_no=a.roll_no and cc=0 and delflag=0 and exam_flag<>'debar' and c.course_id=d.course_id and r.degree_code=d.degree_code  and d.college_code=" + Session["InternalCollegeCode"].ToString() + " " + querymonth_year + " order by  len(r.roll_no),r.degree_code,r.batch_year,r.current_semester,c.course_id";
                                studinfocmd = new SqlCommand(bind_sql, bind_con);

                                studinfors = studinfocmd.ExecuteReader();

                                if (studinfors.HasRows == true)
                                {
                                    hat.Clear();
                                    hat.Add("colege_code", Session["InternalCollegeCode"].ToString());
                                    ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
                                    countds = ds1.Tables[0].Rows.Count;

                                    //Added By SRinath  con.Close();
                                    string strsplright = "select rights from  special_hr_rights where usercode=" + Session["usercode"].ToString() + "";
                                    DataSet dssplrighte = d2.select_method(strsplright, hat, "Text");
                                    if (dssplrighte.Tables[0].Rows.Count > 0)
                                    {
                                        string spl_hr_rights = "";
                                        Hashtable od_has = new Hashtable();

                                        spl_hr_rights = dssplrighte.Tables[0].Rows[0]["rights"].ToString();
                                        if (spl_hr_rights == "True" || spl_hr_rights == "true")
                                        {
                                            splhr_flag = true;

                                        }
                                    }

                                    //int demfcal, demtcal;
                                    //frdate = txtFromDate.Text.ToString();
                                    //todate = txtToDate.Text.ToString();
                                    //string dt = frdate;
                                    //string[] dsplit = dt.Split(new Char[] { '/' });
                                    //frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                                    //demfcal = int.Parse(dsplit[2].ToString());
                                    //demfcal = demfcal * 12;
                                    //cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                                    //dt = todate;
                                    //dsplit = dt.Split(new Char[] { '/' });
                                    //todate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                                    //demtcal = int.Parse(dsplit[2].ToString());
                                    //demtcal = demtcal * 12;
                                    //cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
                                    //End==================================================================
                                    while (studinfors.Read())
                                    {
                                        lblnorec.Text = "";
                                        lblnorec.Visible = false;
                                        roll = studinfors["roll_no"].ToString();

                                        //'----------------------------------------new start----------------

                                        //added By Srinath 20/2/2013 ==Start

                                        chkdegreesem = studinfors["degree_code"].ToString() + '/' + int.Parse(studinfors["current_semester"].ToString());
                                        if (tempdegreesem != chkdegreesem)
                                        {
                                            tempdegreesem = chkdegreesem;
                                            hat.Clear();
                                            //hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
                                            //hat.Add("sem_ester", int.Parse(ddlduration.SelectedValue.ToString()));
                                            hat.Add("degree_code", studinfors["degree_code"].ToString());
                                            hat.Add("sem_ester", int.Parse(studinfors["current_semester"].ToString()));
                                            ds = d2.select_method("period_attnd_schedule", hat, "sp");
                                            if (ds.Tables[0].Rows.Count != 0)
                                            {
                                                NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                                                fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                                                anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                                                minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                                                minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                                            }
                                            //Added By Srinath 3/4/2013
                                            ht_sphr.Clear();
                                            string hrdetno = "";
                                            string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + studinfors["degree_code"].ToString() + " and batch_year=" + studinfors["Batch_year"].ToString() + " and semester=" + studinfors["current_semester"].ToString() + " and date between '" + frdate + "' and '" + todate + "'";
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
                                        //Hidden By Srinath 26/2/2013
                                        //Tempdegree = studinfors["degree_code"].ToString() + '/' + int.Parse(studinfors["current_semester"].ToString());
                                        //==End

                                        //Hiden By Srinath 20/2/2013 =Start
                                        //hat.Clear();
                                        //hat.Add("colege_code", Session["InternalCollegeCode"].ToString());
                                        //ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
                                        //countds = ds1.Tables[0].Rows.Count;
                                        //End

                                        //'------------------------------cal the func for find the att %
                                        per_con_hrs = 0;
                                        //Added By Srinath 3/4/2013 ========Start
                                        spl_tot_per_hrs_spl = 0;
                                        spl_tot_conduct_hr_spl = 0;
                                        //====================End
                                        persentmonthcal_new();

                                        per_per_hrs = per_per_hrs + spl_tot_per_hrs_spl;//Added By Srinath 3/4/2013

                                        //per_con_hrs = (per_workingdays1 - per_dum_unmark);//- notconsider_value);
                                        per_con_hrs = (per_workingdays1 - per_dum_unmark) + spl_tot_conduct_hr_spl;//Modified By Srinath 3/4/2013
                                        per_tage_hrs = ((per_per_hrs / per_con_hrs) * 100);

                                        if (per_tage_hrs > 100)
                                        {
                                            per_tage_hrs = 100;
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



                                        //'------------------------------------------------new end------------
                                        //Hidden By Srinath 26/2/2013
                                        //double t1 = Convert.ToDouble(TextBox1.Text);
                                        //double t2 = Convert.ToDouble(TextBox2.Text);

                                        if (t1 <= Convert.ToDouble(dum_tage_hrs) && t2 >= Convert.ToDouble(dum_tage_hrs))
                                        {
                                            lblnorec.Text = "";
                                            lblnorec.Visible = false;
                                            sflag = true;
                                            s_no++;
                                           
                                            

                                            

                                            //added by rajasekar 07/09/2018
                                            int col = 0;
                                            dtrow = dtl.NewRow();
                                            dtrow[col] = s_no.ToString();
                                            col++;


                                            dtrow[col] = studinfors["Batch_year"].ToString();
                                            col++;


                                            dtrow[col] = studinfors["course_name"].ToString() + " - " + studinfors["Acronym"].ToString();
                                            col++;


                                            dtrow[col] = studinfors["current_semester"].ToString();
                                            col++;


                                            dtrow[col] = studinfors["roll_no"].ToString();
                                            col++;


                                            dtrow[col] = studinfors["stud_name"].ToString();
                                            col++;


                                            dtrow[col] = per_con_hrs.ToString();
                                            col++;


                                            dtrow[col] = per_per_hrs.ToString();
                                            col++;


                                            dtrow[col] = per_workingdays.ToString();
                                            col++;


                                            dtrow[col] = pre_present_date.ToString();
                                            col++;


                                            dtrow[col] = dum_tage_hrs.ToString();
                                            col++;


                                            dtl.Rows.Add(dtrow);
                                            //======================================//


                                            lblnorec.Visible = false;

                                        }



                                    }


                                    //Added by rajasekar 07/09/2018


                                    Showgrid.DataSource = dtl;
                                    Showgrid.DataBind();
                                    Showgrid.HeaderRow.Visible = false;
                                    for (int i = 0; i < Showgrid.Rows.Count; i++)
                                    {
                                        for (int j = 0; j < Showgrid.HeaderRow.Cells.Count; j++)
                                        {

                                            if (i == 0)
                                            {
                                                Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                                Showgrid.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                                Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                                Showgrid.Rows[i].Cells[j].Font.Bold = true;
                                            }
                                            else
                                            {
                                                if (Showgrid.HeaderRow.Cells[j].Text == "Degree-Dept" || Showgrid.HeaderRow.Cells[j].Text == "Roll No" || Showgrid.HeaderRow.Cells[j].Text == "Name of The Student")
                                                    Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;

                                                else
                                                    Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                            }

                                        }

                                    }


                                    //===============================//


                                }
                                else
                                {
                                    lblnorec.Text = "No Record(s) Found";
                                    lblnorec.Visible = true;
                                    Showgrid.Visible = false;
                                    btnprintmaster.Visible = false;
                                    btnPrint.Visible = false;
                                }

                                //'------------------------------


                            }
                            else
                            {

                                
                                Showgrid.Visible = false;
                                btnprintmaster.Visible = false;
                                btnPrint.Visible = false;
                                frmlbl.Visible = false;
                                tolbl.Visible = false;
                                tofromlbl.Visible = false;
                                lblnorec.Visible = true;
                                lblnorec.Text = "Enter Valid To Date";
                            }
                        }

                        if (sflag == false)
                        {

                            lblnorec.Text = "No Record(s) Found";
                            lblnorec.Visible = true;
                            Showgrid.Visible = false;
                            btnprintmaster.Visible = false;
                            btnPrint.Visible = false;
                        }
                    }

                }

            }
        }
        catch
        {

        }
    }
    public void persentmonthcal_new()
    {
        notconsider_value = 0;
        per_workingdays1 = 0;
        int njdate_mng = 0, njdate_evng = 0;
        int per_holidate_mng = 0, per_holidate_evng = 0;
        conducted_hrs_new = 0;
        workingdays = 0;
        mng_conducted_half_days = 0;
        evng_conducted_half_days = 0;

        // Srinath 20/2/2013 ==========================start
        //int demfcal, demtcal;
        //frdate = txtFromDate.Text.ToString();
        //todate = txtToDate.Text.ToString();
        //string dt = frdate;
        //string[] dsplit = dt.Split(new Char[] { '/' });
        //frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
        //demfcal = int.Parse(dsplit[2].ToString());
        //demfcal = demfcal * 12;
        //cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
        //dt = todate;
        //dsplit = dt.Split(new Char[] { '/' });
        //todate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
        //demtcal = int.Parse(dsplit[2].ToString());
        //demtcal = demtcal * 12;
        //cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
        //end==============================================


        per_from_date = Convert.ToDateTime(frdate);
        per_to_date = Convert.ToDateTime(todate);
        dumm_from_date = per_from_date;
        cal_from_date = frommonthyear;

        hat.Clear();

        hat.Add("std_rollno", roll.ToString());
        hat.Add("from_month", cal_from_date);
        hat.Add("to_month", cal_to_date);
        ds2 = d2.select_method("STUD_ATTENDANCE", hat, "sp");

        //Added By Srinath 26/2/2013 ==========Start
        mmyycount = ds2.Tables[0].Rows.Count;
        moncount = mmyycount - 1;
        //========End

        ////Added By Srinath 26/2/2013 ==========Start
        if (chkdegreesem != tempdegreesemchk)
        {
            tempdegreesemchk = chkdegreesem;
            hat.Clear();
            hat.Add("degree_code", studinfors["degree_code"].ToString());
            hat.Add("sem", studinfors["current_semester"].ToString());
            hat.Add("from_date", frdate.ToString());
            hat.Add("to_date", todate.ToString());
            hat.Add("coll_code", int.Parse(Session["InternalCollegeCode"].ToString()));


            //------------------------------------------------------------------
            int iscount = 0;
            holidaycon.Close();
            holidaycon.Open();
            string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + studinfors["degree_code"].ToString() + " and semester=" + studinfors["current_semester"].ToString() + "";
            SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, holidaycon);
            SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);
            DataSet dsholiday = new DataSet();
            daholiday.Fill(dsholiday);
            if (dsholiday.Tables[0].Rows.Count > 0)
            {
                iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
            }

            hat.Add("iscount", iscount);

            //Hidden By Srinath 26/2/2013 
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
                    if(!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
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
                    if(!holiday_table21.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
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
                        holiday_table3.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), k);
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
        }//Added By Srinath 26/2/2013 
        //===========End
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
                //Adde By Srinath 3/4/2013
                if (splhr_flag == true)
                {
                    if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
                    {
                        getspecial_hr();
                    }
                }
                //==End
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


                            if (ds3.Tables[2].Rows.Count != 0)
                            {
                                ts = DateTime.Parse(ds3.Tables[2].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                                diff_date = Convert.ToString(ts.Days);
                                dif_date = double.Parse(diff_date.ToString());

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
                            per_abshrs = 0;
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
                                            per_perhrs += 1;
                                            tot_per_hrs += 1;
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
                            per_abshrs = 0;
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

                        DateTime dumm_fdate = dumm_from_date.AddDays(1 - Convert.ToInt16(dumm_from_date.Day.ToString()));
                        dumm_fdate = dumm_fdate.AddMonths(1);
                        dumm_from_date = dumm_fdate;

                        if (dumm_from_date.Day == 1)
                        {

                            cal_from_date++;


                            if (moncount > next)
                            {
                                next++;//==============================remov cmd 26/5/12
                            }

                        }

                        if (moncount > next)
                        {
                            i--;
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
        per_dum_unmark = dum_unmark;
        if (per_workingdays1 < 0)
        {
            int a = 0;
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
    }
    //void CalculateTotalPages()
    //{
    //    Double totalRows = 0;
    //    totalRows = Convert.ToInt32(FpEntry.Sheets[0].RowCount - 7);
    //    Session["totalPages"] = (int)Math.Ceiling(totalRows / FpEntry.Sheets[0].PageSize);
    //    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
    //    Buttontotal.Visible = true;
    //}
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

    //    FpEntry.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
    //    FpEntry.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
    //    FpEntry.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
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
    //    query_header = "select collname,category,affliatedby,address1,address2,address3,phoneno,faxno,email,website from collinfo where college_code=" + Session["InternalCollegeCode"] + "";
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
    //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;



    //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, 8);
    //        FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Text = sdr_header["category"].ToString() + ", Affliated to " + sdr_header["affliatedby"].ToString();
    //        FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Font.Bold = true;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;

    //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1,8);
    //        FpEntry.Sheets[0].ColumnHeader.Cells[2, 1].Text = sdr_header["address1"].ToString() + "-" + sdr_header["address2"].ToString() + "-" + sdr_header["address1"].ToString();
    //        FpEntry.Sheets[0].ColumnHeader.Cells[2, 1].Font.Bold = true;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[2, 1].HorizontalAlign = HorizontalAlign.Center;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;

    //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1,8);
    //        FpEntry.Sheets[0].ColumnHeader.Cells[3, 1].Text = "Phone : " + sdr_header["phoneno"].ToString() + "  Fax : " + sdr_header["faxno"].ToString();
    //        FpEntry.Sheets[0].ColumnHeader.Cells[3, 1].Font.Bold = true;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[3, 1].HorizontalAlign = HorizontalAlign.Center;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;

    //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, 8);//5th row span
    //        FpEntry.Sheets[0].ColumnHeader.Cells[4, 1].Text = "E-Mail : " + sdr_header["email"].ToString() + "  Web Site : " + sdr_header["website"].ToString();
    //        FpEntry.Sheets[0].ColumnHeader.Cells[4, 1].Font.Bold = true;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[4, 1].HorizontalAlign = HorizontalAlign.Center;


    //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 5, 1);
    //        FpEntry.Sheets[0].ColumnHeader.Cells[0,9].CellType = mi2;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[0,9].HorizontalAlign = HorizontalAlign.Center;


    //    }
    //    //++++++++++++++++++++++++++++++++++++++++++++++++++ End logoset ++++++++++++++++++++
    //}

    //=============Hided by Manikandan 15/05/2013
    //protected void btnPrint_Click(object sender, EventArgs e)
    //{

    //    string subcolumntext = "";
    //    Boolean child_flag = false;

    //    Session["page_redirect_value"] = txtFromDate.Text + "," + txtToDate.Text + "," + TextBox1.Text + "," + TextBox2.Text + "," + ddlcollege.SelectedIndex.ToString();

    //    // first_btngo();
    //    btnGo_Click(sender, e);

    //   // if (tofromlbl.Visible == false)
    //    {
    //        lblpages.Visible = true;
    //        ddlpage.Visible = true;
    //        string clmnheadrname = "";
    //        int total_clmn_count = FpEntry.Sheets[0].ColumnCount;


    //        for (int srtcnt = 0; srtcnt < total_clmn_count; srtcnt++)
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
    //        }

    //        //Response.Redirect("Print_Master_Setting.aspx?ID=" + clmnheadrname.ToString() + ":" + "Attendance.aspx" + ":" + "Overall College Attendance Percentage Report");

    //        Response.Redirect("Print_Master_Setting_New.aspx?ID=" + clmnheadrname.ToString() + ":" + "Attendance.aspx" + ":" + ":" + "Overall College Attendance Percentage Report");
    //    }

    //}

    //==============================

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

    //=============Hided by Manikandan 15/05/2013

    //public void setheader_print()
    //{
    //    // FpEntry.Sheets[0].RemoveSpanCell
    //    //================header
    //    temp_count = 0;


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
    //                    FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
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
    //                    FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
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
    //                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpEntry.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                    FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi2;
    //                    FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
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
    //                    FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                    FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                   // FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
    //                }

    //                end_column = col_count;

    //                temp_count++;
    //                if (final_print_col_cnt == temp_count)
    //                {
    //                    break;
    //                }
    //            }
    //        }

    //        // if (final_print_col_cnt == temp_count + 1)
    //        {
    //            //end_column = col_count;
    //            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, end_column, (6), 1);
    //            FpEntry.Sheets[0].ColumnHeader.Cells[0, end_column].CellType = mi2;
    //            FpEntry.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorLeft = Color.White;
    //           // FpEntry.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorBottom = Color.White;
    //        }

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

    //==========================

    //=============Hided by Manikandan 15/05/2013

    //public void more_column()
    //{
    //    header_text();

    //    FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].Text = coll_name;
    //    FpEntry.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
    //    //  FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, final_print_col_cnt - 2);
    //    if (final_print_col_cnt > 3)
    //    {
    //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count));
    //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, 1, (end_column - col_count));
    //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(2, col_count, 1, (end_column - col_count));
    //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(3, col_count, 1, (end_column - col_count));
    //        //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(6, col_count, 1, (end_column - col_count));
    //        //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(7, col_count, 1, (end_column - col_count));
    //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(4, col_count, 1, (end_column - col_count));
    //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(5, col_count, 1, (end_column - col_count));
    //    }
    //    FpEntry.Sheets[0].ColumnHeader.Cells[1, col_count].Text = address1 + "-" + address2 + "-" + address3;
    //    FpEntry.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorTop = Color.White;
    //    FpEntry.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorBottom = Color.White;

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

    //    FpEntry.Sheets[0].ColumnHeader.Cells[2, col_count].Text = phone + fax;
    //    FpEntry.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorTop = Color.White;
    //    FpEntry.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorBottom = Color.White;

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

    //    FpEntry.Sheets[0].ColumnHeader.Cells[3, col_count].Text = email_id + web_add;
    //    FpEntry.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorTop = Color.White;
    //    FpEntry.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorBottom = Color.White;

    //    if (form_name != "" && form_name != null)
    //    {
    //        FpEntry.Sheets[0].ColumnHeader.Cells[4, col_count].Text = form_name;
    //        FpEntry.Sheets[0].ColumnHeader.Cells[5, col_count].Text = "----------------------------------------------------";
    //    }


    //    if (final_print_col_cnt <= 3)
    //    {
    //       // FpEntry.Sheets[0].ColumnHeader.Cells[6, col_count].Text = "Name of the Program & Branch:" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "     Regulation:" + GetFunction(" select regulation from degree  where degree_code=" + ddlBranch.SelectedValue.ToString() + "");
    //       // FpEntry.Sheets[0].ColumnHeader.Cells[7, col_count].Text = "Academic Year:" + Session["curr_year"].ToString() + "Semester Number:" + ddlSemYr.SelectedValue.ToString();
    //       // FpEntry.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorTop = Color.White;
    //       // FpEntry.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorBottom = Color.White;
    //        //FpEntry.Sheets[0].ColumnHeader.Cells[7, col_count].Border.BorderColorTop = Color.White;
    //    }

    //    else
    //    {

    //        // between_visible_col_cnt = (end_column - col_count)/2;
    //        between_visible_col_cnt = (final_print_col_cnt - 1) / 2;
    //        between_visible_col_cnt_bal = (final_print_col_cnt - 1) % 2;


    //        //for ( x = start_column ; x <FpEntry.Sheets[0].ColumnCount-1; x++)
    //        //{
    //        //    if(FpEntry.Sheets[0].Columns[x].Visible==true)
    //        //    {
    //        //        visi_col++;
    //        //        if (visi_col == start_column + between_visible_col_cnt + between_visible_col_cnt_bal)
    //        //        {
    //        //            visi_col = x;
    //        //            break;
    //        //        }                   
    //        //    }
    //        //}

    //        //FpEntry.Sheets[0].ColumnHeader.Cells[6, start_column].Text = "Name of the Program & Branch:" + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString();
    //        //FpEntry.Sheets[0].ColumnHeader.Cells[6, start_column].HorizontalAlign = HorizontalAlign.Left;
    //        //FpEntry.Sheets[0].ColumnHeader.Cells[6, start_column].Border.BorderColorBottom = Color.White;
    //        //FpEntry.Sheets[0].ColumnHeader.Cells[6, start_column].Border.BorderColorRight = Color.White;




    //        for (x = start_column; x <= FpEntry.Sheets[0].ColumnCount - 1; x++)
    //        {
    //            if (FpEntry.Sheets[0].Columns[x].Visible == true)
    //            {
    //                visi_col1++;
    //                if (visi_col1 == between_visible_col_cnt + between_visible_col_cnt_bal)
    //                {
    //                    break;
    //                }
    //            }
    //        }



    //        for (int xx = start_column + visi_col1 + 1; xx < FpEntry.Sheets[0].ColumnCount - 1; xx++)
    //        {
    //            if (FpEntry.Sheets[0].Columns[xx].Visible == true)
    //            {
    //                visi_col = xx;
    //                break;
    //            }
    //        }



    //     //   FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(6, start_column, 1, visi_col1 + 1);


    //        //FpEntry.Sheets[0].ColumnHeader.Cells[6, visi_col].Text = "Regulation:";
    //        //FpEntry.Sheets[0].ColumnHeader.Cells[6, visi_col].Border.BorderColorLeft = Color.White;
    //        //FpEntry.Sheets[0].ColumnHeader.Cells[6, visi_col].Border.BorderColorRight = Color.White;
    //        //FpEntry.Sheets[0].ColumnHeader.Cells[6, visi_col].Border.BorderColorBottom = Color.White;
    //        //FpEntry.Sheets[0].ColumnHeader.Cells[6, visi_col].HorizontalAlign = HorizontalAlign.Right;

    //        //FpEntry.Sheets[0].ColumnHeader.Cells[6, end_column].HorizontalAlign = HorizontalAlign.Left;
    //        //FpEntry.Sheets[0].ColumnHeader.Cells[6, end_column].Text = GetFunction(" select regulation from degree  where degree_code=" + ddlBranch.SelectedValue.ToString() + "");

    //        int visi_col3 = 0, last_col = 0;
    //        for (int y = visi_col; y < end_column; y++)
    //        {
    //            if (FpEntry.Sheets[0].Columns[y].Visible == true)
    //            {
    //                visi_col3++;
    //                last_col = y;
    //            }
    //        }

    //        //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(6, visi_col, 1, visi_col3);
    //        //FpEntry.Sheets[0].ColumnHeader.Cells[6, end_column].Border.BorderColorBottom = Color.White;

    //        //FpEntry.Sheets[0].ColumnHeader.Cells[7, start_column].Text = "Academic Year:" + Session["curr_year"].ToString();
    //        //FpEntry.Sheets[0].ColumnHeader.Cells[7, start_column].HorizontalAlign = HorizontalAlign.Left;
    //        //FpEntry.Sheets[0].ColumnHeader.Cells[7, start_column].Border.BorderColorRight = Color.White;
    //        //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(7, start_column, 1, visi_col1 + 1);


    //        //FpEntry.Sheets[0].ColumnHeader.Cells[7, visi_col].Text = "Semester Number:";
    //        //FpEntry.Sheets[0].ColumnHeader.Cells[7, visi_col].Border.BorderColorTop = Color.White;
    //        //FpEntry.Sheets[0].ColumnHeader.Cells[7, visi_col].Border.BorderColorLeft = Color.White;
    //        //FpEntry.Sheets[0].ColumnHeader.Cells[7, visi_col].Border.BorderColorRight = Color.White;
    //        //FpEntry.Sheets[0].ColumnHeader.Cells[7, visi_col].HorizontalAlign = HorizontalAlign.Right;

    //    //    FpEntry.Sheets[0].ColumnHeader.Cells[7, end_column].HorizontalAlign = HorizontalAlign.Left;
    //    ////    FpEntry.Sheets[0].ColumnHeader.Cells[7, end_column].Text = ddlSemYr.SelectedValue.ToString();
    //    //    FpEntry.Sheets[0].ColumnHeader.Cells[7, end_column].Border.BorderColorTop = Color.White;
    //    //    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(7, visi_col, 1, visi_col3);
    //    }

    //    FpEntry.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorTop = Color.White;
    //    FpEntry.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorBottom = Color.White;
    //    FpEntry.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorTop = Color.White;
    // //   FpEntry.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorBottom = Color.White;




    //    int temp_count_temp = 0;

    //    if (dsprint.Tables[0].Rows.Count > 0)
    //    {

    //        if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
    //        {
    //            FpEntry.Sheets[0].ColumnHeader.Cells[5, start_column].Border.BorderColorBottom = Color.White;
    //            FpEntry.Sheets[0].ColumnHeader.Cells[5, end_column].Border.BorderColorBottom = Color.White;
    //            FpEntry.Sheets[0].ColumnHeader.Cells[5, visi_col].Border.BorderColorBottom = Color.White;
    //            for (int row_head_count = 6; row_head_count < (6 + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
    //            {
    //                FpEntry.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Text = new_header_string_split[temp_count_temp].ToString();
    //                //if (final_print_col_cnt > 3)
    //                {
    //                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(row_head_count, start_column, 1, (FpEntry.Sheets[0].ColumnCount - start_column + 1));
    //                }
    //                FpEntry.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorTop = Color.White;
    //                if (row_head_count != (6 + new_header_string_split.GetUpperBound(0)))
    //                {
    //                    FpEntry.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorBottom = Color.White;
    //                }

    //                if (header_alignment == "Center")
    //                {
    //                    FpEntry.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Center;
    //                }
    //                else if (header_alignment == "Left")
    //                {
    //                    FpEntry.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Left;
    //                }
    //                else
    //                {
    //                    FpEntry.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Right;
    //                }

    //                temp_count_temp++;
    //            }
    //        }
    //    }
    //}

    //================================

    //=============Hided by Manikandan 15/05/2013

    //public void header_text()
    //{

    //    Boolean check_print_row = false;

    //    SqlDataReader dr_collinfo;
    //    con.Close();
    //    con.Open();
    //    cmd = new SqlCommand("select isnull(college_name,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website,isnull(form_heading_name,'') as form_name,isnull(batch_degree_branch,'') as degree_deatil,isnull(header_align,'') as header_alignment,isnull(header_flag_value,'') as view_header from print_master_setting  where form_name='Attendance.aspx' and college_code=" + Session["InternalCollegeCode"].ToString() + "", con);
    //    dr_collinfo = cmd.ExecuteReader();
    //    while (dr_collinfo.Read())
    //    {
    //        if (dr_collinfo.HasRows == true)
    //        {
    //            check_print_row = true;
    //            coll_name = dr_collinfo["collname"].ToString();
    //            address1 = dr_collinfo["address1"].ToString();
    //            address2 = dr_collinfo["address2"].ToString();
    //            address3 = dr_collinfo["address3"].ToString();
    //            phoneno = dr_collinfo["phoneno"].ToString();
    //            faxno = dr_collinfo["faxno"].ToString();
    //            email = dr_collinfo["email"].ToString();
    //            website = dr_collinfo["website"].ToString();
    //            form_name = dr_collinfo["form_name"].ToString();
    //            degree_deatil = dr_collinfo["degree_deatil"].ToString();
    //            header_alignment = dr_collinfo["header_alignment"].ToString();
    //            view_header = dr_collinfo["view_header"].ToString();
    //        }

    //    }
    //    if (check_print_row == false)
    //    {

    //        con.Close();
    //        con.Open();
    //        cmd = new SqlCommand("select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website from collinfo  where college_code=" + Session["InternalCollegeCode"] + "", con);
    //        dr_collinfo = cmd.ExecuteReader();
    //        while (dr_collinfo.Read())
    //        {
    //            if (dr_collinfo.HasRows == true)
    //            {

    //                string sec_val = "";

    //                //if (ddlSec.SelectedValue.ToString() != string.Empty && ddlSec.SelectedValue.ToString() != null)
    //                //{
    //                //    sec_val = "Section: " + ddlSec.SelectedItem.ToString();
    //                //}
    //                //else
    //                //{
    //                //    sec_val = "";
    //                //}


    //                check_print_row = true;
    //                coll_name = dr_collinfo["collname"].ToString();
    //                address1 = dr_collinfo["address1"].ToString();
    //                address2 = dr_collinfo["address2"].ToString();
    //                address3 = dr_collinfo["address3"].ToString();
    //                phoneno = dr_collinfo["phoneno"].ToString();
    //                faxno = dr_collinfo["faxno"].ToString();
    //                email = dr_collinfo["email"].ToString();
    //                website = dr_collinfo["website"].ToString();
    //                form_name = "  Attendance Shortage Details - Regulation Report ";
    //              //  degree_deatil = ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlSemYr.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
    //                // header_alignment = ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlSemYr.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
    //                // view_header = dr_collinfo["view_header"].ToString();
    //            }

    //        }
    //    }
    //}

    //==========================



    //public void print_btngo()
    //{
    //    final_print_col_cnt = 0;
    //    lblnorec.Visible = false;
    //    check_col_count_flag = false;

    //    FpEntry.Sheets[0].SheetCorner.RowCount = 0;
    //    FpEntry.Sheets[0].ColumnCount = 0;
    //    FpEntry.Sheets[0].RowCount = 0;
    //    FpEntry.Sheets[0].SheetCorner.RowCount = 8;
    //    FpEntry.Sheets[0].ColumnCount = 5;


    //    hat.Clear();
    //    hat.Add("college_code", Session["InternalCollegeCode"].ToString());
    //    hat.Add("form_name", "Attendance.aspx");
    //    dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
    //    if (dsprint.Tables[0].Rows.Count > 0)
    //    {
    //        //lblpages.Visible = true;
    //        //ddlpage.Visible = true;

    //        //3. header add
    //        //if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
    //        //{
    //        //    new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
    //        //    new_header_string_split = new_header_string.Split(',');
    //        //    FpEntry.Sheets[0].SheetCorner.RowCount = FpEntry.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
    //        //}
    //        //3. end header add


    //        btnclick();



    //        //1.set visible columns
    //        column_field = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------------get column field
    //        if (column_field != "" && column_field != null)
    //        {
    //            //  check_col_count_flag = true;

    //            for (col_count_all = 0; col_count_all < FpEntry.Sheets[0].ColumnCount; col_count_all++)
    //            {
    //                FpEntry.Sheets[0].Columns[col_count_all].Visible = false;//------------invisible all column                                
    //            }


    //            printvar = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------visible setting columns
    //            string[] split_printvar = printvar.Split(',');
    //            for (int splval = 0; splval <= split_printvar.GetUpperBound(0); splval++)
    //            {
    //                span_cnt = 0;
    //                string[] split_star = split_printvar[splval].Split('*');
    //                //if (split_star.GetUpperBound(0) > 0)
    //                //{
    //                //    for (col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount - 1; col_count++)
    //                //    {
    //                //        if (FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 1), col_count].Text == split_star[0])
    //                //        {
    //                //            child_span_count = 0;

    //                //            string[] split_star_doller = split_star[1].Split('$');
    //                //            for (int doller_count = 1; doller_count < split_star_doller.GetUpperBound(0); doller_count++)
    //                //            {
    //                //                for (int child_node = col_count; child_node <= col_count + split_star_doller.GetUpperBound(0); child_node++)
    //                //                {
    //                //                    if (FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 1), child_node].Text == split_star_doller[doller_count])
    //                //                    {
    //                //                        span_cnt++;
    //                //                        if (span_cnt == 1 && child_node == col_count + 1)
    //                //                        {
    //                //                            FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 2), col_count + 1].Text = split_star[0].ToString();
    //                //                            col_count++;
    //                //                        }

    //                //                        if (child_node != col_count)
    //                //                        {
    //                //                            span_cnt = child_node - (child_span_count - 1);
    //                //                        }
    //                //                        else
    //                //                        {
    //                //                            child_span_count = col_count;
    //                //                        }


    //                //                        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add((FpEntry.Sheets[0].ColumnHeader.RowCount - 2), col_count, 1, span_cnt);

    //                //                        FpEntry.Sheets[0].Columns[child_node].Visible = true;

    //                //                        final_print_col_cnt++;
    //                //                        if (span_cnt == split_star_doller.GetUpperBound(0) - 1)
    //                //                        {
    //                //                            break;
    //                //                        }

    //                //                    }
    //                //                }
    //                //            }

    //                //        }
    //                //    }
    //                //}
    //                //  else
    //                //{
    //                //    for (col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount; col_count++)
    //                //    {
    //                //        if (FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 1), col_count].Text == split_printvar[splval])
    //                //        {
    //                //            FpEntry.Sheets[0].Columns[col_count].Visible = true;
    //                //            final_print_col_cnt++;
    //                //            break;
    //                //        }
    //                //    }
    //                //}
    //            }
    //            //1 end.set visible columns
    //        }
    //        else
    //        {
    //            FpEntry.Visible = false;
    //            btnprintmaster.Visible = false;
    //            //pnl_pagesetting.Visible = false;
    //            //lblpages.Visible = false;
    //            //ddlpage.Visible = false;
    //            lblnorec.Visible = true;
    //            lblnorec.Text = "Select Atleast One Column Field From The Treeview";
    //        }
    //    }
    //    // FpEntry.Width = final_print_col_cnt * 100;
    //}

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {

        
        Showgrid.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        // pnl_pagesetting.Visible = false;
        frmlbl.Visible = false;
        tolbl.Visible = false;
        tofromlbl.Visible = false;
        lblnorec.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        btnxl.Visible = false;
    }
    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        
        Showgrid.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        //pnl_pagesetting.Visible = false;
        frmlbl.Visible = false;
        tolbl.Visible = false;
        tofromlbl.Visible = false;
        lblnorec.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        btnxl.Visible = false;
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        //Modified by Srinath 27/2/2013
        string reportname = txtexcelname.Text;

        if (reportname.ToString().Trim() != "")
        {
            
            d2.printexcelreportgrid(Showgrid, reportname);
            txtexcelname.Text = "";
        }
        else
        {
            lblnorec.Text = "Please Enter Your Report Name";
            lblnorec.Visible = true;
        }
        ////string appPath = HttpContext.Current.Server.MapPath("~");
        ////string print = "";
        ////if (appPath != "")
        ////{
        ////    int i = 1;
        ////    appPath = appPath.Replace("\\", "/");
        ////e:
        ////    try
        ////    {
        ////        print = "Overall College Attendance Percentage Report" + i;
        ////        FpEntry.SaveExcel(appPath + "/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
        ////        //FpEntry.SaveExcel(appPath + "/" + print + ".xls");
        ////    }
        ////    catch
        ////    {
        ////        i++;
        ////        goto e;

        ////    }
        ////}
        ////ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);
    }

    

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
        Pageload(sender, e);
    }
    public void Pageload(object sender, EventArgs e)
    {


        //if (Session["prntvissble"].ToString() == "true")
        //{
        //    btnPrint.Visible = true;
        //}
        //else
        //{
        //    btnPrint.Visible = false;
        //}
        if (Request.QueryString["val"] == null)
        {
            Session["QueryString"] = "";
            tolbl.Visible = false;
            frmlbl.Visible = false;
            tofromlbl.Visible = false;
            //ddlpage.Visible = false;
            //lblpages.Visible = false;
            btnxl.Visible = false;
            //Added By Srinath 27/2/2013
            txtexcelname.Visible = false;
            lblrptname.Visible = false;

           


            txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            string dt = DateTime.Today.ToShortDateString();
            string[] dsplit = dt.Split(new Char[] { '/' });
            txtFromDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            today_date = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            txtToDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            Session["curr_year"] = dsplit[2].ToString();
            Session["today_date"] = today_date;

            Session["QueryString"] = "";
        }
        else
        {
            //=======================page redirect from master print setting
            Session["QueryString"] = Request.QueryString["val"];
            string_session_values = Request.QueryString["val"].Split(',');
            if (string_session_values.GetUpperBound(0) >= 3)
            {

                txtFromDate.Text = string_session_values[0].ToString();
                txtToDate.Text = string_session_values[1].ToString();


                TextBox1.Text = string_session_values[2].ToString();
                TextBox2.Text = string_session_values[3].ToString();

                ddlcollege.SelectedIndex = Convert.ToInt16(string_session_values[4].ToString());
                Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();
                //print_btngo();

                if (final_print_col_cnt > 0)
                {
                    //setheader_print();

                    
                }
            }
            tolbl.Visible = false;
            frmlbl.Visible = false;
            tofromlbl.Visible = false;

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
    public void getspecial_hr()
    {
        //  try
        {
            tot_conduct_hr_spl = 0;
            tot_per_hrs_spl = 0;
            tot_ml_spl = 0;
            tot_ondu_spl = 0;
            per_abshrs_spl = 0;
            spl_per_abshrs_spl = 0;
            spl_tot_per_hrs_spl = 0;
            spl_tot_conduct_hr_spl = 0;
            spl_tot_ondu_spl = 0;
            spl_tot_ml_spl = 0;

            string hrdetno = "";
            if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
            {
                hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(dumm_from_date), ht_sphr));

            }
            if (hrdetno != "")
            {
                DataSet ds_splhr_query_master = new DataSet();
                string splhr_query_master = "select attendance from specialhr_attendance where roll_no='" + roll.ToString() + "'  and hrdet_no in(" + hrdetno + ")";
                ds_splhr_query_master = d2.select_method(splhr_query_master, hat, "Text");
                if (ds_splhr_query_master.Tables[0].Rows.Count > 0)
                {
                    for (int splhr = 0; splhr < ds_splhr_query_master.Tables[0].Rows.Count; splhr++)
                    {
                        value = ds_splhr_query_master.Tables[0].Rows[splhr]["attendance"].ToString();

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

                spl_per_abshrs_spl = per_abshrs_spl;
                spl_tot_per_hrs_spl = tot_per_hrs_spl;
                spl_tot_conduct_hr_spl = tot_conduct_hr_spl;
                spl_tot_ondu_spl = tot_ondu_spl;
                spl_tot_ml_spl = tot_ml_spl;


            }
        }
        //  catch
        {
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {


        //Session["column_header_row_count"] = Convert.ToString(FpEntry.ColumnHeader.RowCount);
        string degreedetails = string.Empty;
        degreedetails = "Overall Attendance Percentage Report" + '@' + " Date : " + txtFromDate.Text.ToString() + " - " + txtToDate.Text.ToString() + "@Percentage : " + TextBox1.Text.ToString() + " - " + TextBox2.Text.ToString();
        string pagename = "Attendance.aspx";


        //Printcontrol.loadspreaddetails(FpEntry, pagename, degreedetails);
        Printcontrol.Visible = true;

        string ss = null;
        Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
        ////Printcontrol.loadspreaddetails(attnd_report, pagename, degreedetails);
        Printcontrol.Visible = true;
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
        spReportName.InnerHtml = "Overall Attendance Percentage Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }


    public override void VerifyRenderingInServerForm(Control control)
    { }
}


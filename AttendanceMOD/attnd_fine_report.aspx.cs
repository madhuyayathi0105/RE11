using System;//----------------------29/2/12(spread width,XL)btn, 24/3/12(half holiday , remov curr_sem in stud_load)
//----26/3/12(sem date d/p in date txt),complete holiday half), 27/3/12(date format err,remove date set),=(28/3/12)hide txtdate change
//================30/3/12(len(r_no)), 19/4/12(wrong output), 21/4/12(tot amt wrong),3/7/12(iso,p_m_s_n,header setting,trycatch)
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Drawing;


public partial class attnd_fine_report : System.Web.UI.Page
{

   

    
    SqlConnection con_date = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection readcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd;
    Hashtable hat = new Hashtable();
    Hashtable hat_days_first = new Hashtable();
    Hashtable hat_days_end = new Hashtable();
    Hashtable hat_holy = new Hashtable();
    DAccess2 dacces2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds_holi = new DataSet();
    DataSet ds_holi1 = new DataSet();
    ArrayList find_tothour = new ArrayList();
    Boolean fflag = false;
    string collegecode = "";
    //=============3/7/12
    Hashtable has = new Hashtable();
    string new_header_string = "";
    string[] new_header_string_split;
    int start_column = 0, end_column = 0;
    string isonumber = "";
    string view_footer = "", view_header = "", view_footer_text = "";
    string coll_name = "", address1 = "", address2 = "", address3 = "";
    string phone = "", fax = "", email_id = "";
    string web_add = "", form_name = "", header_alignment = "", degree_deatil = "";
    //--------date
    DataSet ds_date = new DataSet();
    static DateTime from_date = new DateTime();
    static DateTime to_date = new DateTime();
    static string from_date_sem = "", to_date_sem = "";
    //---------------
    string stud_name = "";
    Boolean fine_flag = false;
    double nop = 0;
    double noa = 0;
    double nol = 0;
    int abs = 0, att = 0, endk, temp, max;
    int day_val = 0, loop_val = 0;
    string fine_amount = "";
    int overall_hours = 0;
    string strsec = "";
    string date1 = "";
    string datefrom = "";
    string date2 = "";
    string dateto = "";
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    string split_holiday_status_1 = "", split_holiday_status_2 = "";
    string value_holi_status = "";
    string[] split_holiday_status = new string[1000];
    //----------------
    double totpresentday, fine_temp, fine_value;
    double perprest, perpresthrs, perabsent, perabsenthrs, perondu, peronduhrs, perleave, perleavehrs;
    double pertothrs, pertotondu, pertotleavehrs, pertotabsenthrs, onduday, cumcontotpresentday, percontotpresentday, hollyhrs, condhrs, balamonday;
    int i, minI, minII, perdayhrs, wk1, wk2, wk3, wk4, wk5, wk6, wk7, wk8, wk9, Ihof, IIhof, fullday;
    double cumperprest, cumperpresthrs, cumperabsent, cumperabsenthrs, cumperondu, cumperonduhrs, cumperleave, cumperleavehrs, checkpre, baldate, totmonth, cummcc, cumcondhrs, percondhrs = 0;
    string m7, m2, m3, m4, m5, m6, m1, m8, m9, m10;
    int hour1 = 0, hour2 = 0, hour3 = 0, hour4 = 0, hour5 = 0, hour6 = 0, hour7 = 0, hour8 = 0, hour9 = 0, hour10 = 0, condhrs1, condhrs2, condhrs3, condhrs4, condhrs5, condhrs6, condhrs7, condhrs8, condhrs9;
    DateTime from_dt, to_dt, dummy_dt, dt1, dt2;
    string ddd = "";
    string regularflag = "";
    string genderflag = "";
    string phoneno = "", faxno = "", email = "", website = "";
    //--------------------
    //--------------------------
    double par = 0, abse = 0;
    double present, absent, hollydats, leaves, ondu;
    double presenthrs, absenthrs, hollydatshrs, leaveshrs, onduhrs;
    int perhr, abshr, nohrs, roll_no_count = 0;
    int ond, le, fyyy, mm = 1, mon_fine_amt, end_fine_amt;
    int daycount, betdays;
    int dd = 0, dat, dumm;
    double onhr, lehr;
    int fm, fyy, fd, tm, tyy, td, fcal, tcal, k;
    double wkhr, wkhd, dumwkhr, dumwkhd, dumper, per, tot_fine;
    int kk = 0, cumdays, printcheck;
    string roll_no, reg_no, roll_ad, studname, usercode;
    double dumprest, dumpresthrs, dumpresenthrs, dumleaveshrs, dumonduhrs, dumabsenthrs, dumabsent, dumondu, dumleavehrs, dumleave, attday, dumattday;
    int diff = 1, att2;
    double holldays, totworkday, dumtotworkday, dumperhrs, dumtoterhrs, perhrs, totperhrs;
    string frdate, todate;
    string singleuser = "", group_user = "";
    static string[] string_session_values;
    //----------------------------------
    int final_print_col_cnt = 0;
    Boolean check_col_count_flag = false;
    DataSet dsprint = new DataSet();
    //  DAccess2 dacces2 = new DAccess2();
    string column_field = "";
    int col_count_all = 0;
    string printvar = "";
    int span_cnt = 0;
    int col_count = 0;
    int child_span_count = 0;
    int footer_count = 0;
    string footer_text = "";
    // int temp_count = 0;
    int split_col_for_footer = 0;
    int footer_balanc_col = 0;
    int tf = 0;
    Boolean payflag = false;
    int temp_count = 0;
    static string grouporusercode = "";

    DataTable dt = new DataTable();//added by rajasekar 22/08/2018
    DataRow dtrow = null;//added by rajasekar 22/08/2018

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        errlbl.Visible = false;
        if (!Page.IsPostBack)
        {
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");

            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }

            

            //------------initial date picker value
            DateTime dt = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            txtToDate.Text = dt.ToString("dd") + "/" + dt.ToString("MM") + "/" + dt.ToString("yyyy");
            Session["curr_year"] = dt.ToString("yyyy");
            txtFromDate.Text = txtToDate.Text;
            //----------------------------------------------
            grdover.Visible = false;
            
            btnxl.Visible = false;
            //Added By Srinath 27/2/2013
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            Panel3.Visible = false;
            //pageddltxt.Visible = false;//Hidden By Srinath 13/8/2013
            noreclbl.Visible = false;
            errmsg.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            //  pagePanel3.Visible = false;
           
            //-------------------------------Master settings
            string strdayflag;
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            Session["daywise"] = "0";
            Session["hourwise"] = "0";
            if (Session["usercode"] != string.Empty)
            {
                string Master1 = "";
                Master1 = "select * from Master_Settings where " + grouporusercode + "";
                readcon.Close();
                readcon.Open();
                SqlDataReader mtrdr;

                SqlCommand mtcmd = new SqlCommand(Master1, readcon);
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
                            strdayflag = " and (r.Stud_Type='Day Scholar'";
                        }
                        if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                        {
                            if (strdayflag != "" && strdayflag != "\0")
                            {
                                strdayflag = strdayflag + " or r.Stud_Type='Hostler'";
                            }
                            else
                            {
                                strdayflag = " and (r.Stud_Type='Hostler'";
                            }
                        }
                        if (mtrdr["settings"].ToString() == "Regular" && mtrdr["value"].ToString() == "1")
                        {
                            regularflag = "and ((r.mode=1)";

                            // Session["strvar"] = Session["strvar"] + " and (mode=1)";
                        }
                        if (mtrdr["settings"].ToString() == "Lateral" && mtrdr["value"].ToString() == "1")
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
                        if (mtrdr["settings"].ToString() == "Transfer" && mtrdr["value"].ToString() == "1")
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
                    bindbranch();
                    bindsem();
                    bindsec();
                    txtFromDate.Enabled = true;
                    txtToDate.Enabled = true;
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
                
                grdover.Visible = false;
            }
            else
            {
                //=======================page redirect from master print setting
                try
                {
                    string_session_values = Request.QueryString["val"].Split(',');

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
                        // setheader_print();//Hidden By Srinath 15/5/2013

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
                catch
                {
                }

            }
        }
    }




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


    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {

        //bindbranch();
        //bindsem();
        //bindsec();
        // binddate();
        //  pagePanel3.Visible = false;
        Panel3.Visible = false;
        errmsg.Visible = false;
        grdover.Visible = false;
        
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        noreclbl.Visible = false;
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  pagePanel3.Visible = false;
        bindbranch();
        bindsem();
        bindsec();
        //   binddate();
        Panel3.Visible = false;
        errmsg.Visible = false;
        grdover.Visible = false;
        
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        noreclbl.Visible = false;
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  pagePanel3.Visible = false;
        bindsem();
        bindsec();
        // binddate();
        Panel3.Visible = false;
        errmsg.Visible = false;
        grdover.Visible = false;
        
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        noreclbl.Visible = false;
    }
    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  pagePanel3.Visible = false;
        bindsec();
        //  binddate();
        Panel3.Visible = false;
        errmsg.Visible = false;
        grdover.Visible = false;
        
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;

        noreclbl.Visible = false;
    }
    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        // binddate();
        //   pagePanel3.Visible = false;
        Panel3.Visible = false;
        errmsg.Visible = false;
        grdover.Visible = false;
        
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        noreclbl.Visible = false;
    }
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        errmsg.Visible = false;


        Panel3.Visible = false;
        grdover.Visible = false;
        
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        noreclbl.Visible = false;
    }
    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {

        errmsg.Visible = false;

       
        Panel3.Visible = false;
        grdover.Visible = false;
        
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        noreclbl.Visible = false;
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            btn_go_click();
            int temp_col = 0;

            

            if (grdover.Rows.Count > 0)//===========on 9/4/12
            {

                
                final_print_col_cnt = 0;
                
                if (grdover.Rows[0].Cells.Count >0)
                {
                    final_print_col_cnt =grdover.Rows[0].Cells.Count;
                }
                

            }
        }
        catch
        {
        }
    }
    public void btn_go_click()
    {
        //  pagePanel3.Visible = false;
        Panel3.Visible = false;
        errmsg.Visible = false;
        grdover.Visible = false;
        
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        noreclbl.Visible = false;

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
                        if (days >= 0)//-----check date difference
                        {
                            errmsg.Visible = false;

                            
                  

                            //added by rajasekar 22/08/2018
                            dtrow = dt.NewRow();
                            dt.Rows.Add(dtrow);
                            int colu = 0;

                            dt.Columns.Add("S.No.", typeof(string));

                            dt.Rows[0][colu] = "S.No";
                            colu++;
                            if (Session["Rollflag"].ToString() != "0")
                            {
                                dt.Columns.Add("Roll No.", typeof(string));
                                dt.Rows[0][colu] = "Roll No.";
                                colu++;
                            }
                            if (Session["Regflag"].ToString() != "0")
                            {
                                dt.Columns.Add("Reg No.", typeof(string));
                                dt.Rows[0][colu] = "Reg No.";
                                colu++;
                            }

                            
                            
                            dt.Columns.Add("Student Name", typeof(string));
                            dt.Rows[0][colu] = "Student Name";
                            colu++;

                            dt.Columns.Add("Attend Hours", typeof(string));
                            dt.Rows[0][colu] = "Attend Hours";
                            colu++;

                            dt.Columns.Add("Absent Hours", typeof(string));
                            dt.Rows[0][colu] = "Absent Hours";
                            colu++;

                            dt.Columns.Add("Leave Hours", typeof(string));
                            dt.Rows[0][colu] = "Leave Hours";
                            colu++;

                            dt.Columns.Add("Week First", typeof(string));
                            dt.Rows[0][colu] = "Week First";
                            colu++;

                            dt.Columns.Add("Week End", typeof(string));
                            dt.Rows[0][colu] = "Week End";
                            colu++;

                            dt.Columns.Add("Tot Fine", typeof(string));
                            dt.Rows[0][colu] = "Tot Fine";
                            colu++;
                            
                            //===========================//

                            loadstudent();//----------------------------function

                            if (fflag == true)
                            {
                                Panel3.Visible = true;
                                grdover.Visible = true;
                                
                                btnxl.Visible = true;
                                Printcontrol.Visible = false;
                                btnprintmaster.Visible = true;
                                btnPrint.Visible = true;
                                //Added By Srinath 27/2/2013
                                txtexcelname.Visible = true;
                                lblrptname.Visible = true;
                                noreclbl.Visible = false;
                                errmsg.Visible = false;
                                errlbl.Visible = false;
                                //    pagePanel3.Visible = true;
                            }
                            else
                            {
                                Panel3.Visible = false;
                                grdover.Visible = false;
                                
                                btnxl.Visible = false;
                                Printcontrol.Visible = false;
                                btnprintmaster.Visible = false;
                                btnPrint.Visible = false;
                                //Added By Srinath 27/2/2013
                                txtexcelname.Visible = false;
                                lblrptname.Visible = false;
                                errmsg.Visible = false;
                                noreclbl.Visible = true;
                                //     pagePanel3.Visible = false;
                                if (fine_flag == false)
                                {
                                    noreclbl.Text = "No Record(s) Found";
                                }
                            }
                 
                        }
                        else
                        {
                            errmsg.Visible = true;
                            errmsg.Text = "From Date Should Be Less Than To Date";
                            grdover.Visible = false;
                            
                            btnxl.Visible = false;
                            Printcontrol.Visible = false;
                            btnprintmaster.Visible = false;
                            btnPrint.Visible = false;
                            //Added By Srinath 27/2/2013
                            txtexcelname.Visible = false;
                            lblrptname.Visible = false;
                            Panel3.Visible = false;
                            noreclbl.Visible = false;
                        }
                    }
                    else
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "Select Valid To Date";
                        grdover.Visible = false;
                        
                        btnxl.Visible = false;
                        Printcontrol.Visible = false;
                        btnprintmaster.Visible = false;
                        btnPrint.Visible = false;
                        //Added By Srinath 27/2/2013
                        txtexcelname.Visible = false;
                        lblrptname.Visible = false;
                        Panel3.Visible = false;
                        noreclbl.Visible = false;
                    }
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Select Valid To Date";
                    grdover.Visible = false;
                    
                    btnxl.Visible = false;
                    Printcontrol.Visible = false;
                    btnprintmaster.Visible = false;
                    btnPrint.Visible = false;
                    //Added By Srinath 27/2/2013
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    Panel3.Visible = false;
                    noreclbl.Visible = false;
                }
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Select Valid From Date";
                grdover.Visible = false;
                
                btnxl.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                Panel3.Visible = false;
                noreclbl.Visible = false;
            }
        }
        else
        {
            errmsg.Visible = true;
            errmsg.Text = "Select Valid From Date";
            grdover.Visible = false;
            
            btnxl.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            //Added By Srinath 27/2/2013
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            Panel3.Visible = false;
            noreclbl.Visible = false;
        }

        
    }



    public void loadstudent()
    {
        try
        {

            

            if (ddlsec.SelectedValue.ToString() != string.Empty && ddlsec.SelectedValue.ToString() != null && ddlsec.SelectedItem.ToString() != "All")
            {
                strsec = " and r.sections='" + ddlsec.SelectedItem.ToString() + "'";
            }
            else
            {
                strsec = "";
            }


            //========find holiday

            hat.Clear();
            hat.Add("from_date", dt1);
            hat.Add("to_date", dt2);
            hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
            hat.Add("sem", ddlduration.SelectedValue.ToString());
            hat.Add("coll_code", Session["collegecode"].ToString());

            int iscount = 0;
            holidaycon.Close();
            holidaycon.Open();
            string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + dt1.ToString() + "' and '" + dt2.ToString() + "' and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedItem.ToString() + "";
            SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, holidaycon);
            SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);
            DataSet dsholiday = new DataSet();
            daholiday.Fill(dsholiday);
            if (dsholiday.Tables[0].Rows.Count > 0)
            {
                iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
            }
            hat.Add("iscount", iscount);
            ds_holi = dacces2.select_method("HOLIDATE_DETAILS_FINE", hat, "sp");

            if (ds_holi.Tables[0].Rows.Count > 0)
            {
                for (int holi = 0; holi < ds_holi.Tables[0].Rows.Count; holi++)
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

                    if (!hat_holy.ContainsKey(Convert.ToDateTime(ds_holi.Tables[0].Rows[holi]["HOLI_DATE"].ToString())))
                    {
                        hat_holy.Add(Convert.ToDateTime(ds_holi.Tables[0].Rows[holi]["HOLI_DATE"].ToString()), holiday_sched_details);
                    }
                }
            }
            //===========================

            con.Close();
            con.Open();
            cmd = new SqlCommand("select *From PeriodAttndSchedule as p,seminfo as s where p.degree_code=" + ddlbranch.SelectedValue.ToString() + " and p.semester=" + ddlduration.SelectedValue.ToString() + " and p.degree_code=s.degree_code and p.semester=s.semester and s.batch_year=" + ddlbatch.SelectedValue.ToString() + "", con);
            // cmd = new SqlCommand(" select distinct a.roll_no as 'ROLL NO', r.stud_name as 'STUD NAME', r.reg_no as 'REG NO',p.No_of_hrs_per_day as 'PER DAY' FROM attendance a left join registration r on a.roll_no=r.roll_no left join Department d on r.degree_code=d.Dept_Code left join PeriodAttndSchedule p on r.degree_code=p.degree_code  WHERE  r.Batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and  d.Dept_code= " + ddlbranch.SelectedValue.ToString() + " and r.Current_Semester=" + ddlduration.SelectedValue.ToString() + " and p.semester=" + ddlduration.SelectedValue.ToString() + "  and (r.CC = 0)  AND (r.DelFlag = 0)  AND (r.Exam_Flag <> 'debar') AND (r.Current_Semester IS NOT NULL) " + strsec + "  " + Session["strvar"] + " order by a.roll_no  ", con);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd);
            ds.Clear();
            da1.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                nohrs = Convert.ToInt16(ds.Tables[0].Rows[0]["no_of_hrs_per_day"].ToString());
                Ihof = Convert.ToInt16(ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString());
                IIhof = Convert.ToInt16(ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString());
            }
            //added By Srinath 11/8/2013
            string orderby_Setting = dacces2.GetFunction("select value from master_Settings where settings='order_by'");
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
            cmd = new SqlCommand("select distinct a.roll_no as 'ROLL NO', r.Batch_Year as 'BATCH YEAR', r.degree_code as 'DEGREE CODE', r.Sections as 'SECTIONS', r.college_code as 'COLLEGE CODE', d.Dept_Name as 'DEGREE NAME', r.stud_name as 'STUD NAME', r.reg_no as 'REG NO', r.Roll_Admit as 'ROLL ADMIT', p.No_of_hrs_per_day as 'PER DAY', p.no_of_hrs_I_half_day as 'I_HALF_DAY' , p.no_of_hrs_II_half_day as 'II_HALF_DAY', p.min_pres_I_half_day as 'MIN PREE I DAY', p.min_pres_II_half_day as 'MIN PREE II DAY',len(a.roll_no) FROM attendance a, registration r , Department d ,PeriodAttndSchedule p,applyn,Course c,degree de WHERE r.degree_code=" + ddlbranch.SelectedValue.ToString() + "   " + strsec + "  and p.semester=" + ddlduration.SelectedItem.ToString() + "  and (r.CC = 0)  AND (r.DelFlag = 0)  AND (r.Exam_Flag <> 'debar')  AND (r.Current_Semester IS NOT NULL)    and a.roll_no=r.roll_no  and r.degree_code=p.degree_code  and r.app_no=applyn.app_no  " + Session["strvar"].ToString() + " and de.degree_code=r.degree_code and de.dept_code=d.dept_code and r.batch_year=" + ddlbatch.SelectedValue.ToString() + " " + strorder + "", con);
            //cmd = new SqlCommand("select distinct a.roll_no as 'ROLL NO', r.Batch_Year as 'BATCH YEAR', r.degree_code as 'DEGREE CODE', r.Sections as 'SECTIONS', r.college_code as 'COLLEGE CODE', d.Dept_Name as 'DEGREE NAME', r.stud_name as 'STUD NAME', r.reg_no as 'REG NO', r.Roll_Admit as 'ROLL ADMIT', p.No_of_hrs_per_day as 'PER DAY', p.no_of_hrs_I_half_day as 'I_HALF_DAY' , p.no_of_hrs_II_half_day as 'II_HALF_DAY', p.min_pres_I_half_day as 'MIN PREE I DAY', p.min_pres_II_half_day as 'MIN PREE II DAY',len(a.roll_no) FROM attendance a, registration r , Department d ,PeriodAttndSchedule p,applyn,Course c,degree de WHERE r.degree_code=" + ddlbranch.SelectedValue.ToString() + "   " + strsec + "  and p.semester=" + ddlduration.SelectedItem.ToString() + "  and (r.CC = 0)  AND (r.DelFlag = 0)  AND (r.Exam_Flag <> 'debar')  AND (r.Current_Semester IS NOT NULL)    and a.roll_no=r.roll_no  and r.degree_code=p.degree_code  and r.app_no=applyn.app_no  " + Session["strvar"].ToString() + " and de.degree_code=r.degree_code and de.dept_code=d.dept_code and r.batch_year=" + ddlbatch.SelectedValue.ToString() + " order by len(a.roll_no)", con);//Hidden By SRinath 
            // cmd = new SqlCommand(" select distinct a.roll_no as 'ROLL NO', r.stud_name as 'STUD NAME', r.reg_no as 'REG NO',p.No_of_hrs_per_day as 'PER DAY' FROM attendance a left join registration r on a.roll_no=r.roll_no left join Department d on r.degree_code=d.Dept_Code left join PeriodAttndSchedule p on r.degree_code=p.degree_code  WHERE  r.Batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and  d.Dept_code= " + ddlbranch.SelectedValue.ToString() + " and r.Current_Semester=" + ddlduration.SelectedValue.ToString() + " and p.semester=" + ddlduration.SelectedValue.ToString() + "  and (r.CC = 0)  AND (r.DelFlag = 0)  AND (r.Exam_Flag <> 'debar') AND (r.Current_Semester IS NOT NULL) " + strsec + "  " + Session["strvar"] + " order by a.roll_no  ", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);


            int stud_count = 0;
            stud_count = ds.Tables[0].Rows.Count;

            if (stud_count > 0)
            {
                // nohrs = int.Parse((ds.Tables[0].Rows[0]["PER DAY"].ToString()));
                ds4 = dacces2.select_method_wo_parameter("absent_fine_amt_sp", "sp");//sp for fine table

                if (ds4.Tables[0].Rows.Count > 0)
                {
                    fflag = true;
                    fd = Convert.ToInt16(dt1.ToString("dd"));
                    fyy = Convert.ToInt16(dt1.ToString("yyyy"));
                    fm = Convert.ToInt16(dt1.ToString("MM"));
                    td = Convert.ToInt16(dt2.ToString("dd"));
                    tyy = Convert.ToInt16(dt2.ToString("yyyy"));
                    tm = Convert.ToInt16(dt2.ToString("MM"));

                    fcal = ((fyy * 12) + fm);
                    tcal = ((tyy * 12) + tm);
                    totmonth = fcal;

                    for (loop_val = 0; loop_val < stud_count; loop_val++)
                    {
                        mon_fine_amt = 0;
                        end_fine_amt = 0;
                        fine_value = 0;

                        findhours();//===============function             


                       
                        //Hidden By Srinath 15/5/2013
                        //fine_spread.Sheets[0].ColumnHeader.Rows[8].Border.BorderStyleBottom = BorderStyle.Dashed;
                        // fine_spread.Sheets[0].ColumnHeader.Rows[8].Border.BorderColorBottom = Color.Black;
                        

                        //Added by rajasekar 22/08/2018
                        int col = 0;
                        dtrow = dt.NewRow();
                        dtrow[col] = (loop_val + 1).ToString();
                        col++;
                        if (Session["Rollflag"].ToString() != "0")
                        {
                            dtrow[col] = ds.Tables[0].Rows[loop_val]["ROLL NO"].ToString();
                            col++;
                        }
                        if (Session["Regflag"].ToString() != "0")
                        {
                            dtrow[col] = ds.Tables[0].Rows[loop_val]["REG NO"].ToString();
                            col++;
                        }


                        dtrow[col] = ds.Tables[0].Rows[loop_val]["STUD NAME"].ToString();
                        col++;
                        dtrow[col] = perpresthrs.ToString();
                        col++;
                        dtrow[col] = perabsenthrs.ToString();
                        col++;
                        dtrow[col] = perleavehrs.ToString();
                        col++;
                        dtrow[col] = mon_fine_amt.ToString();
                        col++;
                        dtrow[col] = end_fine_amt.ToString();
                        col++;
                        dtrow[col] = fine_value.ToString();

                        dt.Rows.Add(dtrow);


                        //===============================//

                    }
                    //Added by rajasekar 22/08/2018
                    
                   
                    grdover.DataSource = dt;
                    grdover.DataBind();
                    grdover.HeaderRow.Visible = false;
                    for (int i = 0; i < grdover.Rows.Count; i++)
                    {
                        for (int j = 0; j < grdover.HeaderRow.Cells.Count; j++)
                        {

                            if (i == 0)
                            {
                                grdover.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                grdover.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                grdover.Rows[i].Cells[j].BorderColor = Color.Black;
                                grdover.Rows[i].Cells[j].Font.Bold = true;
                            }
                            else
                            {
                                if (grdover.HeaderRow.Cells[j].Text == "Roll No." || grdover.HeaderRow.Cells[j].Text == "Reg No." || grdover.HeaderRow.Cells[j].Text == "Student Name")
                                    grdover.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;

                                else
                                    grdover.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;

                            }
                        }

                    }

                    
            
                    if (fflag == true)
                    {
                        grdover.Visible = true;
                        
                        btnxl.Visible = true;
                        Printcontrol.Visible = false;
                        btnprintmaster.Visible = true;
                        btnPrint.Visible = true;
                        //Added By Srinath 27/2/2013
                        txtexcelname.Visible = true;
                        lblrptname.Visible = true;
                    }
                    int row_count = 0;
                    row_count = grdover.Rows.Count;
                    
                }
                else
                {
                    fine_flag = true;
                    Panel3.Visible = false;
                    grdover.Visible = false;
                    
                    btnxl.Visible = false;
                    Printcontrol.Visible = false;
                    btnprintmaster.Visible = false;
                    btnPrint.Visible = false;
                    //Added By Srinath 27/2/2013
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    errmsg.Visible = false;
                    noreclbl.Visible = true;
                    // pagePanel3.Visible = false;
                    noreclbl.Text = "Set Fine Amount";
                    return;
                }
            }
            else
            {

                Panel3.Visible = false;
                grdover.Visible = false;
                
                btnxl.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                errmsg.Visible = false;
                noreclbl.Visible = true;
                //    pagePanel3.Visible = false;
                noreclbl.Text = "No Student(s) Available";
                return;
            }
        }
        catch
        {
        }
    }
    //public void findholy()
    //{
    //    hat.Clear();
    //    hat.Add("date_val", dummy_dt);
    //    hat.Add("date_val_next", dummy_dt.AddDays(1));
    //    hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
    //    hat.Add("sem_val", ddlduration.SelectedValue.ToString());
    //    ds_holi = dacces2.select_method("holiday_sp_fine", hat, "sp");
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
    public void findhours()
    {
        overall_hours = 0;

        from_today();
        int i = 0;
        dat = fd;
        dummy_dt = dt1;
        for (int cumd = fcal; cumd <= tcal; cumd++)
        {
            fyy = (cumd - 1) / 12;
            totpresentday = 0;
            if (loop_val == 0)
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
                ddd = dummy_dt.ToString("ddd");
                //if (loop_val == 0)
                //{
                //    findholy();
                //    if (ds_holi.Tables[0].Rows.Count == 0)
                //    {
                //        hat_holy.Add(dummy_dt, dummy_dt);
                //    }
                //}
                if (!hat_holy.ContainsKey(dummy_dt))
                {
                    hat_holy.Add(dummy_dt, "3*0*0");
                }


                value_holi_status = GetCorrespondingKey(dummy_dt, hat_holy).ToString();
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
                    // dummy_dt = dummy_dt.AddDays(1);
                    //split_holiday_status_1 = "0";
                    //split_holiday_status_2 = "0";
                    goto for_loop;
                }


                //=============================================
                //====================
                //else
                {
                    if (ddd == "Mon")
                    {
                        day_val = 0;
                    }
                    else if (ddd == "Tue")
                    {
                        day_val = 1;
                    }
                    else if (ddd == "Wed")
                    {
                        day_val = 2;
                    }
                    else if (ddd == "Thu")
                    {
                        day_val = 3;
                    }
                    else if (ddd == "Fri")
                    {
                        day_val = 4;
                    }
                    else if (ddd == "Sat")
                    {
                        day_val = 5;
                    }

                    //==========================


                    if (int.Parse(dummy_dt.ToString("dd")) <= endk)
                    {
                        dat = Convert.ToInt16(dummy_dt.ToString("dd"));
                        m1 = "d" + dat + "d1";
                        m2 = "d" + dat + "d2";
                        m3 = "d" + dat + "d3";
                        m4 = "d" + dat + "d4";
                        m5 = "d" + dat + "d5";
                        m6 = "d" + dat + "d6";
                        m7 = "d" + dat + "d7";
                        m8 = "d" + dat + "d8";
                        m9 = "d" + dat + "d9";

                        int count1 = ds1.Tables[0].Rows.Count;

                        if (count1 > 0 && count1 > i)
                        {
                            if (((split_holiday_status_1 == "1" && Ihof >= 1) || (split_holiday_status_2 == "1" && IIhof <= 1 && Ihof < 1)) && nohrs >= 1)
                            {
                                if (ds1.Tables[0].Rows[i][m1].ToString() != string.Empty && ds1.Tables[0].Rows[i][m1].ToString() != null && ds1.Tables[0].Rows[i][m1].ToString() != "NULL" && ds1.Tables[0].Rows[i][m1].ToString() != "null" && ds1.Tables[0].Rows[i][m1].ToString() != "0")
                                {
                                    hour1 = int.Parse(ds1.Tables[0].Rows[i][m1].ToString());
                                    overall_hours = overall_hours + 1;
                                }
                            }
                            if (((split_holiday_status_1 == "1" && Ihof >= 2) || (split_holiday_status_2 == "1" && IIhof <= 2)) && nohrs >= 2)
                            {
                                if (ds1.Tables[0].Rows[i][m2].ToString() != string.Empty && ds1.Tables[0].Rows[i][m2].ToString() != null && ds1.Tables[0].Rows[i][m2].ToString() != "NULL" && ds1.Tables[0].Rows[i][m2].ToString() != "null" && ds1.Tables[0].Rows[i][m2].ToString() != "0")
                                {
                                    hour2 = int.Parse(ds1.Tables[0].Rows[i][m2].ToString());
                                    overall_hours = overall_hours + 1;
                                }
                            }

                            if (((split_holiday_status_1 == "1" && Ihof >= 3) || (split_holiday_status_2 == "1" && IIhof <= 3 && Ihof < 3)) && nohrs >= 3)
                            {
                                if (ds1.Tables[0].Rows[i][m3].ToString() != string.Empty && ds1.Tables[0].Rows[i][m3].ToString() != null && ds1.Tables[0].Rows[i][m3].ToString() != "NULL" && ds1.Tables[0].Rows[i][m3].ToString() != "null" && ds1.Tables[0].Rows[i][m3].ToString() != "0")
                                {
                                    hour3 = int.Parse(ds1.Tables[0].Rows[i][m3].ToString());
                                    overall_hours = overall_hours + 1;
                                }
                            }

                            if (((split_holiday_status_1 == "1" && Ihof >= 4) || (split_holiday_status_2 == "1" && IIhof <= 4 && Ihof < 4)) && nohrs >= 4)
                            {
                                if (ds1.Tables[0].Rows[i][m4].ToString() != string.Empty && ds1.Tables[0].Rows[i][m4].ToString() != null && ds1.Tables[0].Rows[i][m4].ToString() != "NULL" && ds1.Tables[0].Rows[i][m4].ToString() != "null" && ds1.Tables[0].Rows[i][m4].ToString() != "0")
                                {
                                    hour4 = int.Parse(ds1.Tables[0].Rows[i][m4].ToString());
                                    overall_hours = overall_hours + 1;
                                }
                            }

                            if (((split_holiday_status_1 == "1" && Ihof >= 5) || (split_holiday_status_2 == "1" && IIhof <= 5 && Ihof < 5)) && nohrs >= 5)
                            {
                                if (ds1.Tables[0].Rows[i][m5].ToString() != string.Empty && ds1.Tables[0].Rows[i][m5].ToString() != null && ds1.Tables[0].Rows[i][m5].ToString() != "NULL" && ds1.Tables[0].Rows[i][m5].ToString() != "null" && ds1.Tables[0].Rows[i][m5].ToString() != "0")
                                {
                                    hour5 = int.Parse(ds1.Tables[0].Rows[i][m5].ToString());
                                    overall_hours = overall_hours + 1;
                                }
                            }

                            if (((split_holiday_status_1 == "1" && Ihof >= 6) || (split_holiday_status_2 == "1" && IIhof <= 6 && Ihof < 6)) && nohrs >= 6)
                            {
                                if (ds1.Tables[0].Rows[i][m6].ToString() != string.Empty && ds1.Tables[0].Rows[i][m6].ToString() != null && ds1.Tables[0].Rows[i][m6].ToString() != "NULL" && ds1.Tables[0].Rows[i][m6].ToString() != "null" && ds1.Tables[0].Rows[i][m6].ToString() != "0")
                                {
                                    hour6 = int.Parse(ds1.Tables[0].Rows[i][m6].ToString());
                                    overall_hours = overall_hours + 1;
                                }
                            }

                            if (((split_holiday_status_1 == "1" && Ihof >= 7) || (split_holiday_status_2 == "1" && IIhof <= 7 && Ihof < 7)) && nohrs >= 7)
                            {
                                if (ds1.Tables[0].Rows[i][m7].ToString() != string.Empty && ds1.Tables[0].Rows[i][m7].ToString() != null && ds1.Tables[0].Rows[i][m7].ToString() != "null" && ds1.Tables[0].Rows[i][m7].ToString() != "NULL" && ds1.Tables[0].Rows[i][m7].ToString() != "0")
                                {
                                    hour7 = int.Parse(ds1.Tables[0].Rows[i][m7].ToString());
                                    overall_hours = overall_hours + 1;
                                }
                            }

                            if (((split_holiday_status_1 == "1" && Ihof >= 8) || (split_holiday_status_2 == "1" && IIhof <= 8 && Ihof < 8)) && nohrs >= 8)
                            {
                                if (ds1.Tables[0].Rows[i][m8].ToString() != string.Empty && ds1.Tables[0].Rows[i][m8].ToString() != null && ds1.Tables[0].Rows[i][m8].ToString() != "NULL" && ds1.Tables[0].Rows[i][m8].ToString() != "null" && ds1.Tables[0].Rows[i][m8].ToString() != "0")
                                {
                                    hour8 = int.Parse(ds1.Tables[0].Rows[i][m8].ToString());
                                    overall_hours = overall_hours + 1;
                                }
                            }

                            if (((split_holiday_status_1 == "1" && Ihof >= 9) || (split_holiday_status_2 == "1" && IIhof <= 9 && Ihof < 9)) && nohrs >= 9)
                            {
                                if (ds1.Tables[0].Rows[i][m9].ToString() != string.Empty && ds1.Tables[0].Rows[i][m9].ToString() != null && ds1.Tables[0].Rows[i][m9].ToString() != "null" && ds1.Tables[0].Rows[i][m9].ToString() != "NULL" && ds1.Tables[0].Rows[i][m9].ToString() != "0")
                                {
                                    hour9 = int.Parse(ds1.Tables[0].Rows[i][m9].ToString());
                                    overall_hours = overall_hours + 1;
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
                            if (((split_holiday_status_1 == "1" && Ihof >= 1) || (split_holiday_status_2 == "1" && IIhof <= 1 && Ihof < 1)) && nohrs >= 1)
                            {
                                if (hour1 != 8)
                                {

                                    if (ds2.Tables[0].Rows.Count != 0)
                                    {
                                        if (ds2.Tables[0].Rows[0]["FLAG"].ToString() == "0")
                                        {
                                            if (hour1.ToString() == "10")
                                            {
                                                leaveshrs = leaveshrs + 1;
                                            }
                                            presenthrs = presenthrs + 1;
                                        }
                                        else
                                        {
                                            if (hour1.ToString() == "10")
                                            {
                                                leaveshrs = leaveshrs + 1;

                                            }
                                            if (hour1.ToString() != "10")
                                            {
                                                absenthrs = absenthrs + 1;
                                            }
                                            //------------------------------------------
                                            if (hour1.ToString() != "8")
                                            {
                                                find_fine_amount(1);
                                            }
                                        }

                                    }
                                }
                            }
                            if (((split_holiday_status_1 == "1" && Ihof >= 2) || (split_holiday_status_2 == "1" && IIhof <= 2 && Ihof < 2)) && nohrs >= 2)
                            {
                                if (hour2 != 8)
                                {
                                    if (ds2.Tables[1].Rows.Count != 0)
                                    {
                                        if (ds2.Tables[1].Rows[0]["FLAG"].ToString() == "0")
                                        {
                                            if (hour2.ToString() == "10")
                                            {
                                                leaveshrs = leaveshrs + 1;
                                            }
                                            presenthrs = presenthrs + 1;
                                        }
                                        else
                                        {
                                            if (hour2.ToString() == "10")
                                            {
                                                leaveshrs = leaveshrs + 1;

                                            }

                                            if (hour2.ToString() != "10")
                                            {
                                                absenthrs = absenthrs + 1;
                                            }
                                            if (hour1.ToString() != "8")
                                            {
                                                find_fine_amount(2);
                                            }

                                        }

                                    }
                                }
                            }

                            if (((split_holiday_status_1 == "1" && Ihof >= 3) || (split_holiday_status_2 == "1" && IIhof <= 3 && Ihof < 3)) && nohrs >= 3)
                            {
                                if (hour2 != 8)
                                {
                                    if (ds2.Tables[2].Rows.Count != 0)
                                    {
                                        if (ds2.Tables[2].Rows[0]["FLAG"].ToString() == "0")
                                        {
                                            if (hour3.ToString() == "10")
                                            {
                                                leaveshrs = leaveshrs + 1;
                                            }

                                            presenthrs = presenthrs + 1;

                                        }
                                        else
                                        {
                                            if (hour3.ToString() == "10")
                                            {
                                                leaveshrs = leaveshrs + 1;

                                            }

                                            if (hour3.ToString() != "10")
                                            {
                                                absenthrs = absenthrs + 1;
                                            }
                                            if (hour1.ToString() != "8")
                                            {

                                                find_fine_amount(3);
                                            }

                                        }
                                    }
                                }
                            }

                            if (((split_holiday_status_1 == "1" && Ihof >= 4) || (split_holiday_status_2 == "1" && IIhof <= 4 && Ihof < 4)) && nohrs >= 4)
                            {
                                if (hour4 != 8)
                                {
                                    if (ds2.Tables[3].Rows.Count != 0)
                                    {
                                        if (ds2.Tables[3].Rows[0]["FLAG"].ToString() == "0")
                                        {
                                            if (hour4.ToString() == "10")
                                            {
                                                leaveshrs = leaveshrs + 1;
                                            }

                                            presenthrs = presenthrs + 1;

                                        }
                                        else
                                        {
                                            if (hour4.ToString() == "10")
                                            {
                                                leaveshrs = leaveshrs + 1;

                                            }

                                            if (hour4.ToString() != "10")
                                            {
                                                absenthrs = absenthrs + 1;
                                            }

                                            if (hour1.ToString() != "8")
                                            {
                                                find_fine_amount(4);
                                            }

                                        }
                                    }
                                }
                            }


                            if (((split_holiday_status_1 == "1" && Ihof >= 5) || (split_holiday_status_2 == "1" && IIhof <= 5 && Ihof < 5)) && nohrs >= 5)
                            {
                                if (hour5 != 8)
                                {

                                    if (ds2.Tables[4].Rows.Count != 0)
                                    {
                                        if (ds2.Tables[4].Rows[0]["FLAG"].ToString() == "0")
                                        {

                                            if (hour5.ToString() == "10")
                                            {
                                                leaveshrs = leaveshrs + 1;
                                            }

                                            presenthrs = presenthrs + 1;

                                        }
                                        else
                                        {
                                            if (hour5.ToString() == "10")
                                            {
                                                leaveshrs = leaveshrs + 1;

                                            }


                                            if (hour5.ToString() != "10")
                                            {
                                                absenthrs = absenthrs + 1;
                                            }
                                            if (hour1.ToString() != "8")
                                            {
                                                find_fine_amount(5);
                                            }

                                        }
                                    }
                                }
                            }

                            if (((split_holiday_status_1 == "1" && Ihof >= 6) || (split_holiday_status_2 == "1" && IIhof <= 6 && Ihof < 6)) && nohrs >= 6)
                            {
                                if (hour6 != 8)
                                {
                                    if (ds2.Tables[5].Rows.Count != 0)
                                    {
                                        if (ds2.Tables[5].Rows[0]["FLAG"].ToString() == "0")
                                        {
                                            if (hour6.ToString() == "10")
                                            {
                                                leaveshrs = leaveshrs + 1;
                                            }
                                            presenthrs = presenthrs + 1;

                                        }
                                        else
                                        {
                                            if (hour6.ToString() == "10")
                                            {
                                                leaveshrs = leaveshrs + 1;

                                            }

                                            if (hour6.ToString() != "10")
                                            {
                                                absenthrs = absenthrs + 1;
                                            }

                                            find_fine_amount(6);

                                        }

                                    }
                                }
                            }

                            if (((split_holiday_status_1 == "1" && Ihof >= 7) || (split_holiday_status_2 == "1" && IIhof <= 7 && Ihof < 7)) && nohrs >= 7)
                            {
                                if (hour7 != 8)
                                {
                                    if (ds2.Tables[6].Rows.Count != 0)
                                    {
                                        if (ds2.Tables[6].Rows[0]["FLAG"].ToString() == "0")
                                        {
                                            if (hour7.ToString() == "10")
                                            {
                                                leaveshrs = leaveshrs + 1;
                                            }

                                            presenthrs = presenthrs + 1;
                                        }
                                        else
                                        {
                                            if (hour7.ToString() == "10")
                                            {
                                                leaveshrs = leaveshrs + 1;

                                            }

                                            if (hour7.ToString() != "10")
                                            {
                                                absenthrs = absenthrs + 1;
                                            }

                                            find_fine_amount(7);

                                        }

                                    }
                                }
                            }

                            if (((split_holiday_status_1 == "1" && Ihof >= 8) || (split_holiday_status_2 == "1" && IIhof <= 8 && Ihof < 8)) && nohrs >= 8)
                            {
                                if (hour8 != 8)
                                {

                                    if (ds2.Tables[7].Rows.Count != 0)
                                    {
                                        if (ds2.Tables[7].Rows[0]["FLAG"].ToString() == "0")
                                        {
                                            if (hour8.ToString() == "10")
                                            {
                                                leaveshrs = leaveshrs + 1;
                                            }
                                            presenthrs = presenthrs + 1;
                                        }
                                        else
                                        {
                                            if (hour8.ToString() == "10")
                                            {
                                                leaveshrs = leaveshrs + 1;

                                            }

                                            if (hour8.ToString() != "10")
                                            {
                                                absenthrs = absenthrs + 1;
                                            }

                                            find_fine_amount(8);


                                        }

                                    }
                                }
                            }

                            if (((split_holiday_status_1 == "1" && Ihof >= 9) || (split_holiday_status_2 == "1" && IIhof <= 9 && Ihof < 9)) && nohrs >= 9)
                            {
                                if (hour9 != 8)
                                {
                                    if (ds2.Tables[8].Rows.Count != 0)
                                    {
                                        if (ds2.Tables[8].Rows[0]["FLAG"].ToString() == "0")
                                        {
                                            if (hour9.ToString() == "10")
                                            {
                                                leaveshrs = leaveshrs + 1;
                                            }
                                            presenthrs = presenthrs + 1;
                                        }
                                        else
                                        {
                                            if (hour9.ToString() == "10")
                                            {
                                                leaveshrs = leaveshrs + 1;

                                            }
                                            if (hour9.ToString() != "10")
                                            {

                                                absenthrs = absenthrs + 1;
                                            }

                                            find_fine_amount(9);

                                        }
                                    }
                                }
                            }
                        }
                        dat++;
                    }
                }
            for_loop:
                dummy_dt = dummy_dt.AddDays(1);
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
            i++;
        }
        find_tothour.Add(overall_hours);
        perpresthrs = presenthrs;
        perabsenthrs = absenthrs;
        perleavehrs = leaveshrs;

        fine_value = fine_temp;
        present = 0;
        presenthrs = 0;
        absent = 0;
        absenthrs = 0;
        leaves = 0;
        leaveshrs = 0;
        totpresentday = 0;
        fine_temp = 0;

    }

    public void find_fine_amount(int hour_val)
    {
        if (ddd == "Mon")
        {
            if (hour_val == 1)
            {
                mon_fine_amt++;
            }

            if (hat_holy.Contains(dummy_dt.AddDays(1)) && hat_holy.Contains(dummy_dt.AddDays(2)) && hat_holy.Contains(dummy_dt.AddDays(3)) && hat_holy.Contains(dummy_dt.AddDays(4)) && hat_holy.Contains(dummy_dt.AddDays(5)) && nohrs == hour_val)
            {
                end_fine_amt++;
                tot_fine = Convert.ToDouble(ds4.Tables[0].Rows[5][nohrs].ToString());
                fine_temp = fine_temp + tot_fine;
            }
            else
            {
                if (ds4.Tables[0].Rows.Count > 0)
                {
                    tot_fine = Convert.ToDouble(ds4.Tables[0].Rows[day_val][hour_val].ToString());
                    fine_temp = fine_temp + tot_fine;
                }
            }
        }
        else if (ddd == "Tue")
        {
            if (hat_holy.Contains(dummy_dt.AddDays(1)) && hat_holy.Contains(dummy_dt.AddDays(2)) && hat_holy.Contains(dummy_dt.AddDays(3)) && hat_holy.Contains(dummy_dt.AddDays(4)) && nohrs == hour_val)
            {
                end_fine_amt++;
                tot_fine = Convert.ToDouble(ds4.Tables[0].Rows[5][nohrs].ToString());
                fine_temp = fine_temp + tot_fine;
            }
            else
            {
                if (ds4.Tables[0].Rows.Count > 0)
                {
                    tot_fine = Convert.ToDouble(ds4.Tables[0].Rows[day_val][hour_val].ToString());
                    fine_temp = fine_temp + tot_fine;
                }
            }
        }
        else if (ddd == "Wed")
        {
            if (hat_holy.Contains(dummy_dt.AddDays(1)) && hat_holy.Contains(dummy_dt.AddDays(2)) && hat_holy.Contains(dummy_dt.AddDays(3)) && nohrs == hour_val)
            {
                end_fine_amt++;
                tot_fine = Convert.ToDouble(ds4.Tables[0].Rows[5][nohrs].ToString());
                fine_temp = fine_temp + tot_fine;
            }
            else
            {
                if (ds4.Tables[0].Rows.Count > 0)
                {
                    tot_fine = Convert.ToDouble(ds4.Tables[0].Rows[day_val][hour_val].ToString());
                    fine_temp = fine_temp + tot_fine;
                }
            }
        }

        else if (ddd == "Thu")
        {
            if (hat_holy.Contains(dummy_dt.AddDays(1)) && hat_holy.Contains(dummy_dt.AddDays(2)) && nohrs == hour_val)
            {
                end_fine_amt++;
                tot_fine = Convert.ToDouble(ds4.Tables[0].Rows[5][nohrs].ToString());
                fine_temp = fine_temp + tot_fine;
            }
            else
            {
                if (ds4.Tables[0].Rows.Count > 0)
                {
                    tot_fine = Convert.ToDouble(ds4.Tables[0].Rows[day_val][hour_val].ToString());
                    fine_temp = fine_temp + tot_fine;
                }
            }
        }
        else if (ddd == "Fri")
        {
            
            if (hat_holy.Contains(dummy_dt.AddDays(1)) && hat_holy.Contains(dummy_dt.AddDays(2)) && nohrs == hour_val)
            {
                end_fine_amt++;
                tot_fine = Convert.ToDouble(ds4.Tables[0].Rows[5][nohrs].ToString());
                fine_temp = fine_temp + tot_fine;
            }
            else
            {
                if (ds4.Tables[0].Rows.Count > 0)
                {
                    tot_fine = Convert.ToDouble(ds4.Tables[0].Rows[day_val][hour_val].ToString());
                    fine_temp = fine_temp + tot_fine;
                }
            }
        }
        else if (ddd == "Sat")
        {
            if (hat_holy.Contains(dummy_dt.AddDays(1)) && nohrs == hour_val)
            {
                end_fine_amt++;
                tot_fine = Convert.ToDouble(ds4.Tables[0].Rows[5][nohrs].ToString());
                fine_temp = fine_temp + tot_fine;
            }
            else
            {
                if (ds4.Tables[0].Rows.Count > 0)
                {
                    tot_fine = Convert.ToDouble(ds4.Tables[0].Rows[day_val][hour_val].ToString());
                    fine_temp = fine_temp + tot_fine;
                }
            }
        }
    }
    public void from_today()
    {

        hat.Clear();
        hat.Add("f_date", int.Parse(fcal.ToString()));
        hat.Add("t_date", int.Parse(tcal.ToString()));
        hat.Add("roll_no", ds.Tables[0].Rows[loop_val]["ROLL NO"].ToString());

        ds1 = dacces2.select_method("ATT_REPORTS_DETAILS", hat, "sp");
        dat = fd;

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
            
            if (fyy % 4 == 0)
            {
                daycount = 29;
            }
            else
            {
                daycount = 28;
            }

        }
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
    



    protected void btnxl_Click(object sender, EventArgs e)
    {
        //Modified by Srinath 27/2/2013
        string reportname = txtexcelname.Text;

        if (reportname.ToString().Trim() != "")
        {
            
            dacces2.printexcelreportgrid(grdover, reportname);
            txtexcelname.Text = "";
        }
        else
        {
            errlbl.Text = "Please Enter Your Report Name";
            errlbl.Visible = true;
        }
  

    }

    public void binddate()
    {
        con_date.Close();
        con_date.Open();
        string str_query = "select start_date , end_date from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + "";
        SqlDataAdapter da_date = new SqlDataAdapter(str_query, con_date);
        da_date.Fill(ds_date);
        if (ds_date.Tables[0].Rows.Count > 0)
        {
            from_date = Convert.ToDateTime(ds_date.Tables[0].Rows[0][0].ToString());
            Session["from_date"] = from_date.ToString();
            to_date = Convert.ToDateTime(ds_date.Tables[0].Rows[0][1].ToString());
            Session["to_date"] = to_date.ToShortDateString();
            from_date_sem = from_date.Day + "/" + from_date.Month + "/" + from_date.Year;
            to_date_sem = to_date.Day + "/" + to_date.Month + "/" + to_date.Year;
            txtFromDate.Text = from_date_sem;
            txtToDate.Text = to_date_sem;
            txtFromDate.Enabled = true;
            txtToDate.Enabled = true;

        }
        else
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;

        }
    }

    protected void btn_print_setting_Click(object sender, EventArgs e)
    {
        string batch = "", sections = "", semester = "", degreecode = "", subcolumntext = "", strsec = "";
        Boolean child_flag = false;
        int sec_index = 0, sem_index = 0;
        batch = ddlbatch.SelectedValue.ToString();
        sections = ddlsec.SelectedValue.ToString();
        semester = ddlduration.SelectedValue.ToString();
        degreecode = ddlbranch.SelectedValue.ToString();

        string clmnheadrname = "";

        Session["page_redirect_value"] = txtFromDate.Text;

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
    
        btnGo_Click(sender, e);
  
        Session["page_redirect_value"] = ddlbatch.SelectedIndex + "," + ddldegree.SelectedIndex + "," + ddlbranch.SelectedIndex + "," + sem_index + "," + sec_index + "," + txtFromDate.Text + "," + txtToDate.Text;


        Response.Redirect("Print_Master_Setting_New.aspx?ID=" + clmnheadrname.ToString() + ":" + "attnd_fine_report.aspx" + ":" + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + ", " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + " Semester  " + "," + "Section" + strsec + ":" + "Attendance Fine Report");

        // }
    }

    public void print_btngo()
    {
        final_print_col_cnt = 0;
        errmsg.Visible = false;
        check_col_count_flag = false;

       


        has.Clear();
        has.Add("college_code", Session["collegecode"].ToString());
        has.Add("form_name", "attnd_fine_report.aspx");
        dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", has, "sp");
        if (dsprint.Tables[0].Rows.Count > 0)
        {

            isonumber = dsprint.Tables[0].Rows[0]["ISOCode"].ToString();



            btn_go_click();



            //1.set visible columns
            column_field = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------------get column field
            if (column_field != "" && column_field != null)
            {
     


                printvar = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------visible setting columns
                string[] split_printvar = printvar.Split(',');
                for (int splval = 0; splval <= split_printvar.GetUpperBound(0); splval++)
                {
                    span_cnt = 0;
                    string[] split_star = split_printvar[splval].Split('*');


                    {
           
                    }
                }
                
            }
            else
            {
                grdover.Visible = false;
                
                btnxl.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                Panel3.Visible = false;

                errlbl.Visible = true;
                errlbl.Text = "Select Atleast One Column Field From The Treeview";
            }
        }
        
    }
   



    public void more_column()
    {
        header_text();

        
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

        
        if (form_name != "" && form_name != null)
        {
           
        }
        
        foreach (Int32 s in find_tothour)
        {
            temp = Convert.ToInt32(s);
            if (max < temp)
            {

                max = temp;
            }
        }

        

        string dt = DateTime.Today.ToShortDateString();
        string[] dsplit = dt.Split(new Char[] { '/' });
        
        int temp_count_temp = 0;
        string[] header_align_index;

        if (dsprint.Tables[0].Rows.Count > 0)
        {

            if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
            {
                header_align_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString().Split(',');

                

                for (int row_head_count = 10; row_head_count < (10 + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
                {
                    
                    {
                        
                    }
                    
                    if (row_head_count != (10 + new_header_string_split.GetUpperBound(0)))
                    {
                       
                    }

                    if (temp_count_temp <= header_align_index.GetUpperBound(0))
                    {
                        if (header_align_index[temp_count_temp].ToString() != string.Empty)
                        {
                            header_alignment = header_align_index[temp_count_temp].ToString();
                            if (header_alignment == "2")
                            {
                               
                            }
                            else if (header_alignment == "1")
                            {
                               
                            }
                            else
                            {
                                
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
        cmd = new SqlCommand("select isnull(college_name,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website,isnull(form_heading_name,'') as form_name,isnull(batch_degree_branch,'') as degree_deatil,isnull(header_align,'') as header_alignment,isnull(header_flag_value,'') as view_header from print_master_setting  where form_name='attnd_fine_report.aspx'", con);
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
                header_alignment = dr_collinfo["header_alignment"].ToString();
                view_header = dr_collinfo["view_header"].ToString();
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
                    string sec_val = "";

                    if (ddlsec.SelectedValue.ToString() != string.Empty && ddlsec.SelectedValue.ToString() != null)
                    {
                        sec_val = "Section: " + ddlsec.SelectedItem.ToString();
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
                    form_name = "Attendance Fine Report";
                    degree_deatil = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
                    // header_alignment = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
                    // view_header = dr_collinfo["view_header"].ToString();
                }

            }
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        //Session["column_header_row_count"] = Convert.ToString(fine_spread.ColumnHeader.RowCount);
        string sections = ddlsec.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
        {
            sections = "";
        }
        else
        {
            sections = "- Sec-" + sections;
        }
        string degreedetails = "Attendance Fine Report" + '@' + "Degree: " + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '-' + ddlbranch.SelectedItem.ToString() + '-' + "Sem-" + ddlduration.SelectedItem.ToString() + sections + '@' + "Date :" + txtFromDate.Text.ToString() + " To " + txtToDate.Text.ToString();
        string pagename = "attnd_fine_report.aspx";
        //Printcontrol.loadspreaddetails(fine_spread, pagename, degreedetails);
       

        string ss = null;
        
        
        Printcontrol.loadspreaddetails(grdover, pagename, degreedetails, 0, ss);

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
        spReportName.InnerHtml = "Attendance Fine Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);
       

    }


    public override void VerifyRenderingInServerForm(Control control)
    { }
}
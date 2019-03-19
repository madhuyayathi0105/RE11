using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FarPoint.Web.Spread;
using System.Text;
using InsproDataAccess;

public partial class subjwiseattndreport : System.Web.UI.Page
{
    #region variable

    [Serializable()]
    public class MyImg : ImageCellType
    {
        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, bool upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl;
            img.Width = Unit.Percentage(80);
            return img;
        }
    }
    InsproDirectAccess dir = new InsproDirectAccess();
    bool check_col_count_flag = false;
    bool btnclick_or_print = false;
    bool Isfirst = false;
    bool IsFirstcol = false;
    bool Cond = false;
    bool start_flag = false;
    bool rec_flag = false;
    bool joinflag = false;
    bool binjoin = false;
    bool binprocessed = false;
    bool filter_flag = false;
    bool cellclick = false;
    bool recflag = false;

    decimal avgstudent1 = 0;
    decimal avgstudent2 = 0;
    double avgstudent3 = 0;
    double perofpass = 0;
    double avg = 0;
    double attnd_perc_val = 0;

    DateTime dt1 = new DateTime();
    DateTime dt2 = new DateTime();
    DateTime f_date, t_date, s_date;
    DateTime from_date, to_date, dummy_from_date, dummy_to_date;

    DAccess2 da = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet roll_data = new DataSet();
    static DataSet dsprint = new DataSet();

    Hashtable hat = new Hashtable();
    Hashtable holiday = new Hashtable();
    Hashtable hasspl_tot = new Hashtable();
    Hashtable hasspl_pres = new Hashtable();

    int count1 = 0;
    int rollcount = 0;
    int attroll = 0;
    int between_visible_col_cnt = 0;
    int between_visible_col_cnt_bal = 0;
    int x = 0;
    int visi_col = 0;
    int visi_col1 = 0;
    int f_month_year = 0;
    int t_month_year = 0;
    int rollmonthcount = 0;
    int tempfdate = 0;
    int temptdate = 0;
    int date_day = 0;
    int date_mnth = 0;
    int date_yr = 0;
    int tot_mnth = 0;
    int rollcolumncount = 0;
    int total_conducted_hrs = 0;
    int total_attended_hrs = 0;
    int whole_total_conducted_hrs = 0;
    int row = 0; int table = 0;
    int sdate = 0; int enddate = 0;
    int month = 0; int day = 0;
    int year = 0;
    int spl_total_conducted_hrs = 0;
    int spl_total_attended_hrs = 0;
    int temp_count = 0;
    int final_print_col_cnt = 0; int split_col_for_footer = 0; int col_count = 0; int footer_balanc_col = 0; int footer_count = 0;
    int col_count_all = 0; int span_cnt = 0; int child_span_count = 0;
    int start_column = 0; int end_column = 0;
    int new_header_count = 0;
    int tval = 0;
    int split_holiday_status_1 = 0; int split_holiday_status_2 = 0;
    int mm = 0;
    int fyy = 0;
    int fromdate_month_year = 0;
    int loop_val_from = 0;
    int loop_val_to = 0;
    int attendval = 0;
    int no_of_hrs = 0; int first_half = 0; int sec_half = 0;
    int tot_hrs_value = 0;
    int tot_pres = 0;
    int tot_hrs = 0;
    int row_cnt = 0;
    int column_cnt = 0;
    int tothr = 0;
    int subcount = 0;
    int temp_cnt = 0;
    int col = 0;
    int todate_month_year = 0;
    int total_present = 0;
    int present_day = 0;
    int i = 0;
    int field_col_val = 0;
    int days_cnt = 0;
    int subject_count = 0;
    int fd = 0;
    int td = 0;
    int fcal = 0;
    int tcal = 0;
    int totpresentday = 0;
    int daycount = 0;
    int balamonday = 0;
    int endk = 0;
    int k = 0;
    int rec_cnt = 0;
    int ProcessNJHrs = 0;
    int PresentSum = 0;
    int presentdays = 0;
    int TotalPres = 0;
    int TotalAbs = 0;
    int inner_loop = 0;
    int month_year_from = 0;
    int month_year_to = 0;
    int textmark = 0;
    int no_of_days = 0;
    int strorder = 0;
    int month_year = 0;
    int count_val = 0;
    int upper_bnd = 0;
    int processthrs = 0;
    int processphrs = 0;
    int ct = 0;

    List<string> present_table = new List<string>();

    string[] s_code;
    string[] split_date_time1;
    string[] dummy_split;
    string[] split_holiday_status = new string[1000];
    string[] new_header_string_split;
    string[] string_session_values = new string[100];

    string coll_name = string.Empty;
    string form_name = string.Empty;
    string phoneno = string.Empty;
    string faxno = string.Empty;
    string footer_text = string.Empty;
    string header_alignment = string.Empty;
    string degree_deatil = string.Empty;
    string isonumber = string.Empty;
    string new_header_string_index = string.Empty;
    string key_value = string.Empty;
    string attnd_val = string.Empty;
    string subject_num_spl = string.Empty;
    string new_header_string = string.Empty;
    string column_field = string.Empty;
    string printvar = string.Empty;
    string view_footer = string.Empty;
    string view_header = string.Empty;
    string view_footer_text = string.Empty;
    string address1 = string.Empty;
    string address2 = string.Empty;
    string address3 = string.Empty;
    string phone = string.Empty;
    string fax = string.Empty;
    string email_id = string.Empty;
    string web_add = string.Empty;
    string email = string.Empty;
    string website = string.Empty;
    string bg, out1 = string.Empty;
    string strsec1;
    string day_find;
    string get_date_holiday = string.Empty;
    string date1 = string.Empty;
    string datefrom = string.Empty;
    string date2 = string.Empty;
    string dateto = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string fdate = string.Empty;
    string tdate = string.Empty;
    string d = string.Empty;
    string d1 = string.Empty;
    string totoal_c_hrs = string.Empty;
    string h = string.Empty;
    string da1 = string.Empty;
    string davalue = string.Empty;
    string sume = string.Empty;
    string sem_start = string.Empty;
    string sem_end = string.Empty;
    string halforfull = string.Empty;
    string mng = string.Empty;
    string evng = string.Empty;
    string holiday_sched_details = string.Empty;
    string value_holi_status = string.Empty;
    string strsec = string.Empty;
    string stud_roll_no = string.Empty;
    string process_hr = string.Empty;
    string markglag = string.Empty;
    string rol_no = string.Empty;
    string courseid = string.Empty;
    string atten = string.Empty;
    string Master = string.Empty;
    string regularflag = string.Empty;
    string genderflag = string.Empty;
    string strdayflag = string.Empty;
    string staff = string.Empty;
    string code = string.Empty;
    string text = string.Empty;
    string temp1 = string.Empty;
    string str_order = string.Empty;
    string start_date = string.Empty;
    string admiss_date = string.Empty;
    string day_val = string.Empty;
    string srt_day = string.Empty;
    string subjcode_staffcode = string.Empty;
    string day_hour = string.Empty;
    string sttnd_temp_val = string.Empty;
    string total_hrs_val_val = string.Empty;
    string dd = string.Empty;
    string mon_year = string.Empty;
    string dd_hr = string.Empty;
    string process_hr_val = string.Empty;
    string field_val = string.Empty;
    string subject_num = string.Empty;
    string subject_code = string.Empty;
    string att_val = string.Empty;
    string noofhrs = string.Empty;
    string mng_hrs = string.Empty;
    string evng_hrs = string.Empty;
    string examflag = string.Empty;
    string debar_reason = string.Empty;
    string str_tothr = string.Empty;
    string attnd_perc = string.Empty;
    string noofday = string.Empty;
    string temp = string.Empty;
    string str_date = string.Empty;
    string total_hrs = "0";
    string present_hrs = string.Empty;
    string present_hrs_val = string.Empty;
    string str_from = string.Empty;
    string str_to = string.Empty;
    static string grouporusercode = string.Empty;
    static string grouporusercode1 = string.Empty;
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_sem = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_man = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection syll_con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_spl1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_spl2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_spl3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection readcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_splhr_query_master = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);

    SqlCommand cmd;
    SqlCommand syll_cmd;
    SqlCommand cmd1;
    SqlCommand cmd2;
    SqlCommand cmd3;
    SqlCommand cmd_sem;
    SqlCommand cmd_man;
    SqlCommand cmd_spl1;
    SqlCommand cmd_spl2;
    SqlCommand cmd_spl3;
    DataTable data = new DataTable();
    DataRow drow;
    int colcount = 0;

    Dictionary<string, string> dicsubhead = new Dictionary<string, string>();
    Dictionary<int, string> dicdiscon = new Dictionary<int, string>();
    ArrayList arrColHdrNames1 = new ArrayList();
    ArrayList arrColHdrNames2 = new ArrayList();
    int colHdrIndx = 0;
    string includediscon = "";
    string includedebar = "";
    string includedisco = "";
    string includedeba = "";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        errmsg.Visible = false;
        if (!Page.IsPostBack)
        {
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                grouporusercode1 = " and group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                grouporusercode1 = " and usercode=" + Session["usercode"].ToString().Trim() + "";
            }


            perddl.Visible = false;//----visibility
            pertxt.Visible = false;
            frmlbl.Visible = false;
            tolbl.Visible = false;
            tofromlbl.Visible = false;
            Showgrid.Visible = false;
            btnxl.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            btndirtPrint.Visible = false;
            //Added By Srinath 27/2/2013
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            errmsg.Visible = false;
            setpanel.Visible = false;
            pageddltxt.Visible = false;
            setpanel.Visible = false;



            //----------------------set date
            string dt = DateTime.Today.ToShortDateString();
            string[] dsplit = dt.Split(new Char[] { '/' });
            txtFromDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            txtToDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            Session["curr_year"] = dsplit[2].ToString();
            //=====================
            if (Request.QueryString["val"] == null)
            {
                try
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
                catch
                {
                }
            }
            else
            {
                //=======================page redirect from master print setting
                try
                {
                    string_session_values = Request.QueryString["val"].Split(',');
                    if (string_session_values.GetUpperBound(0) == 10)
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
                            basedddl.SelectedIndex = Convert.ToInt16(string_session_values[7].ToString());
                            if (string_session_values[8].ToString() == "True")
                            {
                                perddl.Visible = true;
                                pertxt.Visible = true;
                                perddl.SelectedIndex = Convert.ToInt16(string_session_values[9].ToString());
                                pertxt.Text = string_session_values[10].ToString();
                            }
                            else
                            {
                                perddl.Visible = false;
                                pertxt.Visible = false;
                            }
                            print_btngo();
                            if (Showgrid.Rows.Count > 0 && Showgrid.Columns.Count > 0)
                            {
                                Showgrid.Width = final_print_col_cnt * 100;
                                Showgrid.Visible = true;
                                setpanel.Visible = false;
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
                }
                catch
                {
                }
                //===================================
            }
            //---------------------------
            //-------------------------------Master settings
            strdayflag = string.Empty;
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            ViewState["strvar"] = string.Empty;
            if (Session["usercode"] != "")
            {
                Master = "select * from Master_Settings where " + grouporusercode + "";
                setcon.Close();
                setcon.Open();
                SqlDataReader mtrdr;
                SqlCommand mtcmd = new SqlCommand(Master, setcon);
                mtrdr = mtcmd.ExecuteReader();
                ViewState["strvar"] = string.Empty;
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                Session["Daywise"] = "0";
                Session["Hourwise"] = "0";
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
                            strdayflag = " and (registration.Stud_Type='Day Scholar'";
                        }
                        if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                        {
                            if (strdayflag != "" && strdayflag != "\0")
                            {
                                strdayflag = strdayflag + " or registration.Stud_Type='Hostler'";
                            }
                            else
                            {
                                strdayflag = " and (registration.Stud_Type='Hostler'";
                            }
                        }
                        if (mtrdr["settings"].ToString() == "Regular")
                        {
                            regularflag = "and ((registration.mode=1)";
                            // ViewState["strvar"] = ViewState["strvar"] + " and (mode=1)";
                        }
                        if (mtrdr["settings"].ToString() == "Lateral")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (registration.mode=3)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((registration.mode=3)";
                            }
                            //ViewState["strvar"] = ViewState["strvar"] + " and (mode=3)";
                        }
                        if (mtrdr["settings"].ToString() == "Transfer")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (registration.mode=2)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((registration.mode=2)";
                            }
                            //ViewState["strvar"] = ViewState["strvar"] + " and (mode=2)";
                        }
                        if (mtrdr["settings"].ToString() == "Male" && mtrdr["value"].ToString() == "1")
                        {
                            genderflag = " and (a.sex='0'";
                        }
                        if (mtrdr["settings"].ToString() == "Female" && mtrdr["value"].ToString() == "1")
                        {
                            if (genderflag != "" && genderflag != "\0")
                            {
                                genderflag = genderflag + " or a.sex='1'";
                            }
                            else
                            {
                                genderflag = " and (a.sex='1'";
                            }
                        }
                        //if (mtrdr["settings"].ToString() == "Day Wise" && mtrdr["value"].ToString() == "1")
                        //{
                        //    Session["Daywise"] = "1";
                        //}
                        //if (mtrdr["settings"].ToString() == "Hour Wise" && mtrdr["value"].ToString() == "1")
                        //{
                        //    Session["Hourwise"] = "1";
                        //}
                    }
                }
                if (strdayflag != "")
                {
                    strdayflag = strdayflag + ")";
                }
                ViewState["strvar"] = strdayflag;
                if (regularflag != "")
                {
                    regularflag = regularflag + ")";
                }
                ViewState["strvar"] = ViewState["strvar"] + regularflag;
                if (genderflag != "")
                {
                    genderflag = genderflag + ")";
                }
                ViewState["strvar"] = ViewState["strvar"] + regularflag + genderflag;
            }


        }
    }

    public void bindbatch()
    {
        ////batch
        ddlbatch.Items.Clear();
        string sqlstring = string.Empty;
        int max_bat = 0;
        con.Close();
        con.Open();

        includediscon = " and delflag=0";
        includedebar = " and exam_flag <> 'DEBAR'";
        includedisco = " and r.delflag=0";
        includedeba = " and r.exam_flag <> 'DEBAR'";
        string getshedulockva = da.GetFunctionv("select value from Master_Settings where settings='Attendance Discount' " + grouporusercode1 + "");
        if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
        {
            includediscon = string.Empty;
            includedisco = "";
        }
        getshedulockva = da.GetFunctionv("select value from Master_Settings where  settings='Attendance Debar' " + grouporusercode1 + " ");
        if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
        {
            includedebar = string.Empty;
            includedeba = "";
        }

        cmd = new SqlCommand(" select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' " + includediscon + includedebar + " order by batch_year", con);
        SqlDataAdapter da1 = new SqlDataAdapter(cmd);
        DataSet ds1 = new DataSet();
        da1.Fill(ds1);
        ddlbatch.DataSource = ds1;
        ddlbatch.DataValueField = "batch_year";
        ddlbatch.DataBind();
        //----------------display max year value 
        sqlstring = "select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' " + includediscon + includedebar + "";
        max_bat = Convert.ToInt32(GetFunction(sqlstring));
        ddlbatch.SelectedValue = max_bat.ToString();
        con.Close();
        //binddegree();
    }

    public void binddegree()
    {
        ddldegree.Items.Clear();
        ds.Clear();
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
        ds = da.select_method("bind_degree", hat, "sp");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
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

    public void bindsem()
    {
        //--------------------semester load
        ddlduration.Items.Clear();
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
        bindsec();
    }

    public void bindsec()
    {
        includediscon = " and delflag=0";
        includedebar = " and exam_flag <> 'DEBAR'";
        includedisco = " and r.delflag=0";
        includedeba = " and r.exam_flag <> 'DEBAR'";
        string getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Discount' " + grouporusercode1 + "");
        if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
        {
            includediscon = string.Empty;
            includedisco = "";
        }
        getshedulockva = d2.GetFunctionv("select value from Master_Settings where  settings='Attendance Debar' " + grouporusercode1 + " ");
        if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
        {
            includedebar = string.Empty;
            includedeba = "";
        }

        //----------load section
        ddlsec.Items.Clear();


        con.Close();
        con.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' '  " + includediscon + includedebar + "", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlsec.DataSource = ds;
        ddlsec.DataTextField = "sections";
        ddlsec.DataBind();
        //ddlsec.Items.Insert(0, "All");
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
        frmlbl.Visible = false;
        tolbl.Visible = false;
        con.Close();
    }

    public void bindbranch()
    {
        ddlbranch.Items.Clear();
        hat.Clear();
        ds.Clear();
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
        ds = da.select_method("bind_branch", hat, "sp");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        setpanel.Visible = false;
        errmsg.Visible = false;
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btndirtPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        setpanel.Visible = false;
        seterr.Visible = false;
        //bindbranch();
        //bindsem();
        //bindsec();
        //binddate();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        setpanel.Visible = false;
        errmsg.Visible = false;
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btndirtPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        setpanel.Visible = false;
        seterr.Visible = false;
        bindbranch();
        bindsem();
        bindsec();
        binddate();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        setpanel.Visible = false;
        errmsg.Visible = false;
        Showgrid.Visible = false;
        setpanel.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btndirtPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        seterr.Visible = false;
        bindsem();
        bindsec();
        binddate();
    }

    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {
        setpanel.Visible = false;
        errmsg.Visible = false;
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btndirtPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        setpanel.Visible = false;
        seterr.Visible = false;
        bindsec();
        binddate();
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        setpanel.Visible = false;
        binddate();
        errmsg.Visible = false;
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btndirtPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        setpanel.Visible = false;
        seterr.Visible = false;
        frmlbl.Visible = false;
        tolbl.Visible = false;
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        setpanel.Visible = false;
        errmsg.Visible = false;
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btndirtPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        setpanel.Visible = false;
        seterr.Visible = false;
        frmlbl.Visible = false;
        tolbl.Visible = false;
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        setpanel.Visible = false;
        errmsg.Visible = false;
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btndirtPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        setpanel.Visible = false;
        seterr.Visible = false;
        tolbl.Visible = false;
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
        }
        catch
        {
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        string colname1 = "";
        try
        {

            Showgrid.Visible = false;
            //ddlpage.Visible = false;
            //lblpage.Visible = false;
            setpanel.Visible = false;
            frmlbl.Visible = false;
            tolbl.Visible = false;
            tofromlbl.Visible = false;
            errmsg.Visible = false;
            btnclick();
            btnPrint11();
            int temp_col = 0;
            if (data.Columns.Count > 0 && data.Rows.Count > 2)
            {

                if (data.Rows.Count > 0)
                {

                    Showgrid.DataSource = data;
                    Showgrid.DataBind();
                    Showgrid.Visible = true;

                    foreach (KeyValuePair<int, string> dr in dicdiscon)
                    {
                        int key = dr.Key;
                        Showgrid.Rows[key].BackColor = ColorTranslator.FromHtml("RED");


                    }

                    if (basedddl.SelectedValue.ToString() == "0")
                    {
                        if (dicsubhead.Count > 0)
                        {
                            foreach (KeyValuePair<string, string> dr in dicsubhead)
                            {
                                colname1 = dr.Value;
                                data.Columns["0"].ColumnName = colname1.ToString();
                                data.AcceptChanges();
                            }
                        }
                    }
                    else
                    {
                        int colindex = colcount;
                        if (dicsubhead.Count > 0)
                        {
                            foreach (KeyValuePair<string, string> dr in dicsubhead)
                            {
                                colname1 = dr.Value;
                                Showgrid.Rows[1].Cells[colindex].Text = colname1.ToString();
                                colindex = colindex + 2;
                            }
                        }

                    }

                    int rowcnt = Showgrid.Rows.Count - 2;
                    //Rowspan
                    for (int rowIndex = Showgrid.Rows.Count - rowcnt - 1; rowIndex >= 0; rowIndex--)
                    {
                        GridViewRow row = Showgrid.Rows[rowIndex];
                        GridViewRow previousRow = Showgrid.Rows[rowIndex + 1];
                        Showgrid.Rows[rowIndex].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        Showgrid.Rows[rowIndex].Font.Bold = true;
                        Showgrid.Rows[rowIndex].HorizontalAlign = HorizontalAlign.Center;
                        if (rowIndex == 1)
                        {
                            for (int i = 0; i < colcount; i++)
                            {
                                if (row.Cells[i].Text == previousRow.Cells[i].Text)
                                {

                                    row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                           previousRow.Cells[i].RowSpan + 1;
                                    previousRow.Cells[i].Visible = false;
                                }
                            }
                        }
                        else
                        {
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

                    }

                    //ColumnSpan
                    for (int rowIndex = 0; rowIndex >= 0; rowIndex--)
                    {
                        for (int cell = Showgrid.Rows[rowIndex].Cells.Count - 1; cell > 0; cell--)
                        {
                            TableCell colum = Showgrid.Rows[rowIndex].Cells[cell];
                            TableCell previouscol = Showgrid.Rows[rowIndex].Cells[cell - 1];
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
                    if (pertxt.Text != "")
                    {
                        for (int i = 2; i < Showgrid.Rows.Count; i++)
                        {
                            
                            for (int col = 5; col < Showgrid.HeaderRow.Cells.Count;col++)
                            {
                                string colname = Showgrid.Rows[0].Cells[col].Text;
                                if (colname != "Percentage")
                                {
                                    string cellte = Showgrid.Rows[i].Cells[col].Text;
                                    if (!string.IsNullOrEmpty(cellte) && cellte != " " && !cellte.Contains("nsb"))
                                    //if (Convert.ToString(cellte).All(char.IsNumber))
                                    {
                                        find_filter(cellte);
                                        if (filter_flag == true)
                                        {
                                            Showgrid.Rows[i].Cells[col].BackColor = ColorTranslator.FromHtml("LawnGreen");


                                        }
                                    }
                                    col++;
                                }
                                else
                                    col = Showgrid.HeaderRow.Cells.Count;

                            }
                        }
                    }
                }

                setpanel.Visible = false;

            }
            else
            {
                if (errmsg.Visible != true)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
                    Showgrid.Visible = false;
                    setpanel.Visible = false;
                    errmsg.Visible = true;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    btndirtPrint.Visible = false;
                }
            }

        }
        catch
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
        spReportName.InnerHtml = "Subject Wise Attendance With Percentage Report ";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }

    public void btnclick()
    {
        try
        {
            dicsubhead.Clear();
            string date1 = string.Empty;
            string datefrom = string.Empty;
            string date2 = string.Empty;
            string dateto = string.Empty;

            errmsg.Visible = false;
            Showgrid.Visible = false;
            btnxl.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            btndirtPrint.Visible = false;
            //Added By Srinath 27/2/2013
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            setpanel.Visible = false;
            seterr.Visible = false;
            tofromlbl.Visible = false;
            date1 = txtFromDate.Text;
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
                            Session["from_date_time"] = dt1;
                            DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                            Session["to_date_time"] = dt2;
                            TimeSpan t = dt2.Subtract(dt1);
                            long days = t.Days;
                            Session["days"] = days;
                            if (days >= 0)//-----check date difference
                            {
                                loadsubject();
                                loadstudent();
                                //    setheader_print ();
                            }
                            else
                            {
                                tofromlbl.Visible = true;
                                errmsg.Visible = false;
                                Showgrid.Visible = false;
                                btnxl.Visible = false;
                                Printcontrol.Visible = false;
                                btnprintmaster.Visible = false;
                                btndirtPrint.Visible = false;
                                //Added By Srinath 27/2/2013
                                txtexcelname.Visible = false;
                                lblrptname.Visible = false;
                                setpanel.Visible = false;
                                setpanel.Visible = false;
                                goto labe;
                            }
                        }
                        else
                        {
                            setpanel.Visible = false;
                            tolbl.Visible = true;
                            tolbl.Text = "Enter Valid To Date";
                            errmsg.Visible = false;
                            Showgrid.Visible = false;
                            btnxl.Visible = false;
                            Printcontrol.Visible = false;
                            btnprintmaster.Visible = false;
                            btndirtPrint.Visible = false;
                            //Added By Srinath 27/2/2013
                            txtexcelname.Visible = false;
                            lblrptname.Visible = false;
                            setpanel.Visible = false;
                            errmsg.Visible = false;
                            tofromlbl.Visible = false;
                            goto labe;
                        }
                    }
                    else
                    {
                        tolbl.Visible = true;
                        setpanel.Visible = false;
                        tolbl.Text = "Enter Valid To Date";
                        errmsg.Visible = false;
                        Showgrid.Visible = false;
                        btnxl.Visible = false;
                        Printcontrol.Visible = false;
                        btnprintmaster.Visible = false;
                        btndirtPrint.Visible = false;
                        //Added By Srinath 27/2/2013
                        txtexcelname.Visible = false;
                        lblrptname.Visible = false;
                        setpanel.Visible = false;
                        errmsg.Visible = false;
                        tofromlbl.Visible = false;
                        goto labe;
                    }
                }
                else
                {
                    frmlbl.Visible = true;
                    frmlbl.Text = "Enter Valid From Date";
                    errmsg.Visible = false;
                    Showgrid.Visible = false;
                    btnxl.Visible = false;
                    Printcontrol.Visible = false;
                    btnprintmaster.Visible = false;
                    btndirtPrint.Visible = false;
                    //Added By Srinath 27/2/2013
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    setpanel.Visible = false;
                    errmsg.Visible = false;
                    tofromlbl.Visible = false;
                    goto labe;
                }
            }
            else
            {
                frmlbl.Visible = true;
                frmlbl.Text = "Enter Valid From Date";
                errmsg.Visible = false;
                Showgrid.Visible = false;
                btnxl.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
                btndirtPrint.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                setpanel.Visible = false;
                errmsg.Visible = false;
                tofromlbl.Visible = false;
                goto labe;
            }
            //-----------------------page search-total value display
            if (Convert.ToInt32(Showgrid.Rows.Count) > 2 || data.Rows.Count > 2)
            {
                setpanel.Visible = false;
                Showgrid.Visible = true;
                errmsg.Visible = false;
                errmsg.Text = "";
                //Added By Srinath 27/2/2013
                btnxl.Visible = true;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = true;
                btndirtPrint.Visible = true;
                txtexcelname.Visible = true;
                lblrptname.Visible = true;
                double totalRows = 0;
                totalRows = Convert.ToInt32(Showgrid.Rows.Count);
                DropDownListpage.Items.Clear();
                if (totalRows >= 10)
                {
                    Showgrid.PageSize = 10;
                    for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                    {
                        DropDownListpage.Items.Add((k + 10).ToString());
                    }
                    DropDownListpage.Items.Add("Others");
                    Showgrid.Height = 410;
                    //Showgrid.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    //Showgrid.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                }
                else if (totalRows == 0)
                {
                    DropDownListpage.Items.Add("0");
                    Showgrid.Height = 200;
                }
                else
                {
                    Showgrid.PageSize = Convert.ToInt32(totalRows);
                    DropDownListpage.Items.Add(Showgrid.PageSize.ToString());
                    Showgrid.Height = 30 + (38 * Convert.ToInt32(totalRows));
                }
                Session["totalPages"] = (int)Math.Ceiling(totalRows / Showgrid.PageSize);
                Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                if (Convert.ToInt16(Session["totalPages"].ToString()) > 0)
                {
                    pagesearch_txt.Visible = true;
                    pgsearch_lbl.Visible = true;
                }
                else
                {
                    pagesearch_txt.Visible = false;
                    pgsearch_lbl.Visible = false;
                }
                if (Convert.ToInt32(Showgrid.Rows.Count) > 10)
                {
                    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                    Showgrid.PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                    CalculateTotalPages();
                }
                //if (Convert.ToInt32(subject_report.Sheets[0].RowCount) == 0)
                //{
                //    setpanel.Visible = false;
                //    subject_report.Visible = false;
                //    errmsg.Visible = true;
                //    errmsg.Text = "No record found";
                //}
            }
            else
            {
                setpanel.Visible = false;
                Showgrid.Visible = false;
                btnxl.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
                btndirtPrint.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                setpanel.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "No Record(s) found";
            }
        labe: h = string.Empty;
        }
        catch
        {
        }
    }

    //Hidden By Srinath 15/5/2013
    //public void setheader_print()
    //{
    //    try
    //    {
    //        // subject_report.Sheets[0].RemoveSpanCell
    //        //================header
    //        temp_count = 0;
    //        MyImg mi = new MyImg();
    //        mi.ImageUrl = "~/images/10BIT001.jpeg";
    //        mi.ImageUrl = "Handler/Handler2.ashx?";
    //        MyImg mi2 = new MyImg();
    //        mi2.ImageUrl = "~/images/10BIT001.jpeg";
    //        mi2.ImageUrl = "Handler/Handler5.ashx?";
    //        if (final_print_col_cnt == 1)
    //        {
    //            for (col_count = 0; col_count < subject_report.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (subject_report.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    // one_column();
    //                    more_column();
    //                    break;
    //                }
    //            }
    //        }
    //        else if (final_print_col_cnt == 2)
    //        {
    //            for (col_count = 0; col_count < subject_report.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (subject_report.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    if (temp_count == 0)
    //                    {
    //                        start_column = col_count;
    //                        //   subject_report.Sheets[0].ColumnHeader.Columns[col_count].Width = 100;//
    //                        subject_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (subject_report.Sheets[0].ColumnHeader.RowCount - 3), 1);
    //                        subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                        subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                    else
    //                    {
    //                        //  one_column();
    //                        more_column();
    //                        for (int row_cnt = 0; row_cnt < subject_report.Sheets[0].ColumnHeader.RowCount - 3; row_cnt++)
    //                        {
    //                            subject_report.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
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
    //            for (col_count = 0; col_count < subject_report.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (subject_report.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    if (temp_count == 0)
    //                    {
    //                        start_column = col_count;
    //                        //   subject_report.Sheets[0].ColumnHeader.Columns[col_count].Width = 100;//
    //                        subject_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (subject_report.Sheets[0].ColumnHeader.RowCount - 3), 1);
    //                        subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                        subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                    else if (temp_count == 1)
    //                    {
    //                        // one_column();
    //                        more_column();
    //                        for (int row_cnt = 0; row_cnt < subject_report.Sheets[0].ColumnHeader.RowCount - 3; row_cnt++)
    //                        {
    //                            subject_report.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                            subject_report.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
    //                        }
    //                    }
    //                    else if (temp_count == 2)
    //                    {
    //                        //--------------------ISO CODE 29/6/12 PRABAH
    //                        if (isonumber != string.Empty)
    //                        {
    //                            subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].Text = "ISO CODE:" + isonumber;
    //                            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, (subject_report.Sheets[0].ColumnHeader.RowCount - 4), 1);
    //                            subject_report.Sheets[0].ColumnHeader.Cells[1, col_count].CellType = mi2;
    //                            subject_report.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorRight = Color.Black;
    //                            subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.Black;
    //                            subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
    //                            subject_report.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorTop = Color.White;
    //                        }
    //                        else
    //                        {
    //                            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (subject_report.Sheets[0].ColumnHeader.RowCount - 3), 1);
    //                            subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi2;
    //                            subject_report.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
    //                            subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.Black;
    //                        }
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
    //            for (col_count = 0; col_count < subject_report.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (subject_report.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    if (temp_count == 0)
    //                    {
    //                        start_column = col_count;
    //                        subject_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, ((subject_report.Sheets[0].ColumnHeader.RowCount - 3)), 1);
    //                        subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                        subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                        // subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
    //                    }
    //                    end_column = col_count;
    //                    temp_count++;
    //                    if (final_print_col_cnt == temp_count)
    //                    {
    //                        break;
    //                    }
    //                }
    //            }
    //            // if (final_print_col_cnt == temp_count + 1)
    //            //{
    //            //    //end_column = col_count;
    //            //    subject_report.Sheets[0].ColumnHeaderSpanModel.Add(0, end_column, ((subject_report.Sheets[0].ColumnHeader.RowCount - 3)), 1);
    //            //    subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].CellType = mi2;
    //            //    subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorLeft = Color.White;
    //            //    // subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorBottom = Color.White;
    //            //}
    //            //--------------ISO 29/6/12 PRABHA
    //            if (isonumber != string.Empty)
    //            {
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Text = "ISO CODE:";// +isonumber;
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].Text = isonumber;
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].HorizontalAlign = HorizontalAlign.Left;
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorBottom = Color.White;
    //                subject_report.Sheets[0].ColumnHeaderSpanModel.Add(1, end_column, ((subject_report.Sheets[0].ColumnHeader.RowCount - 4)), 1);
    //                subject_report.Sheets[0].ColumnHeader.Cells[1, end_column].CellType = mi2;
    //                subject_report.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorBottom = Color.White;
    //                subject_report.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorRight = Color.Black;
    //                subject_report.Sheets[0].ColumnHeader.Columns[end_column].Width = 150;
    //                subject_report.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorTop = Color.White;
    //                subject_report.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorRight = Color.Black;
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Border.BorderColorBottom = Color.White;
    //                subject_report.Sheets[0].ColumnHeader.Cells[1, end_column - 1].Border.BorderColorTop = Color.White;
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Border.BorderColorRight = Color.White;
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorLeft = Color.White;
    //                subject_report.Sheets[0].ColumnHeader.Cells[1, end_column ].Border.BorderColorBottom = Color.Black ;
    //            }
    //            else
    //            {
    //                subject_report.Sheets[0].ColumnHeaderSpanModel.Add(0, end_column, ((subject_report.Sheets[0].ColumnHeader.RowCount - 3)), 1);
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].CellType = mi2;
    //                subject_report.Sheets[0].ColumnHeader.Columns[end_column].Width = 150;
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorRight = Color.Black;
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorBottom = Color.Black;
    //            }
    //            temp_count = 0;
    //            for (col_count = 0; col_count < subject_report.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (subject_report.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    if (temp_count == 1)
    //                    {
    //                        more_column();
    //                        for (int row_cnt = 0; row_cnt < subject_report.Sheets[0].ColumnHeader.RowCount - 3; row_cnt++)
    //                        {
    //                            subject_report.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                            subject_report.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
    //                        }
    //                    }
    //                    temp_count++;
    //                }
    //            }
    //        }
    //        //=========================
    //        //2.Footer setting
    //        if (dsprint.Tables.Count>0 && dsprint.Tables[0].Rows.Count > 0)
    //        {
    //            if (dsprint.Tables[0].Rows[0]["footer"].ToString() != null && dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
    //            {
    //                footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());
    //                subject_report.Sheets[0].RowCount = subject_report.Sheets[0].RowCount + 3;
    //                subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 3), start_column].ColumnSpan = subject_report.Sheets[0].ColumnCount - start_column;
    //                subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 2), start_column].ColumnSpan = subject_report.Sheets[0].ColumnCount - start_column;
    //                subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 3), start_column].Border.BorderColorBottom = Color.White;
    //                subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 2), start_column].Border.BorderColorTop = Color.White;
    //                subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 2), start_column].Border.BorderColorBottom = Color.White;
    //                subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 1), start_column].Border.BorderColorTop = Color.White;
    //                footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
    //                string[] footer_text_split = footer_text.Split(',');
    //                footer_text =string.Empty;
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
    //                    for (col_count = 0; col_count < subject_report.Sheets[0].ColumnCount; col_count++)
    //                    {
    //                        if (subject_report.Sheets[0].Columns[col_count].Visible == true)
    //                        {
    //                            subject_report.Sheets[0].SpanModel.Add((subject_report.Sheets[0].RowCount - 1), col_count, 1, final_print_col_cnt);
    //                            subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 1), col_count].Text = footer_text;
    //                            subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                            break;
    //                        }
    //                    }
    //                }
    //                else if (final_print_col_cnt == footer_count)
    //                {
    //                    temp_count = 0;
    //                    for (col_count = 0; col_count < subject_report.Sheets[0].ColumnCount; col_count++)
    //                    {
    //                        if (subject_report.Sheets[0].Columns[col_count].Visible == true)
    //                        {
    //                            subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                            subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                            subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                            subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
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
    //                    for (col_count = 0; col_count < subject_report.Sheets[0].ColumnCount; col_count++)
    //                    {
    //                        if (subject_report.Sheets[0].Columns[col_count].Visible == true)
    //                        {
    //                            if (temp_count == 0)
    //                            {
    //                                subject_report.Sheets[0].SpanModel.Add((subject_report.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer + footer_balanc_col);
    //                            }
    //                            else
    //                            {
    //                                subject_report.Sheets[0].SpanModel.Add((subject_report.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer);
    //                            }
    //                            subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                            subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                            if (col_count - 1 >= 0)
    //                            {
    //                                subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorLeft = Color.White;
    //                                subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorRight = Color.White;
    //                            }
    //                            subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                            subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                            if (col_count + 1 < subject_report.Sheets[0].ColumnCount)
    //                            {
    //                                subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorLeft = Color.White;
    //                                subject_report.Sheets[0].Cells[(subject_report.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorRight = Color.White;
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
    //        }
    //        //2 end.Footer setting
    //    }
    //    catch
    //    {
    //    }
    //}

    #region Cmd Saran
    //Cmd Saran 
    //public void more_column()
    //{
    //    try
    //    {
    //        header_text();
    //        Showgrid.Sheets[0].ColumnHeader.Cells[0, col_count].Text = coll_name;
    //        subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
    //        //  subject_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, final_print_col_cnt - 2);
    //        if (final_print_col_cnt > 3)
    //        {
    //            if (isonumber != string.Empty)//------29/6/12 PRABHA
    //            {
    //                subject_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count - 1));
    //            }
    //            else
    //            {
    //                subject_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count));
    //            }
    //        }
    //        subject_report.Sheets[0].ColumnHeader.Cells[1, col_count].Text = address1 + "-" + address2 + "-" + address3;
    //        if (final_print_col_cnt > 3)
    //        {
    //            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, 1, (end_column - col_count));
    //        }
    //        subject_report.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorTop = Color.White;
    //        subject_report.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorBottom = Color.White;
    //        if (phoneno != "" && phoneno != null)
    //        {
    //            phone = "Phone:" + phoneno;
    //        }
    //        else
    //        {
    //            phone = string.Empty;
    //        }
    //        if (faxno != "" && faxno != null)
    //        {
    //            fax = "  Fax:" + faxno;
    //        }
    //        else
    //        {
    //            fax = string.Empty;
    //        }
    //        subject_report.Sheets[0].ColumnHeader.Cells[2, col_count].Text = phone + fax;
    //        if (final_print_col_cnt > 3)
    //        {
    //            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(2, col_count, 1, (end_column - col_count));
    //        }
    //        subject_report.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorTop = Color.White;
    //        subject_report.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorBottom = Color.White;
    //        if (email != "" && faxno != null)
    //        {
    //            email_id = "Email:" + email;
    //        }
    //        else
    //        {
    //            email_id = string.Empty;
    //        }
    //        if (website != "" && website != null)
    //        {
    //            web_add = "  Web Site:" + website;
    //        }
    //        else
    //        {
    //            web_add = string.Empty;
    //        }
    //        subject_report.Sheets[0].ColumnHeader.Cells[3, col_count].Text = email_id + web_add;
    //        if (final_print_col_cnt > 3)
    //        {
    //            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(3, col_count, 1, (end_column - col_count));
    //        }
    //        subject_report.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorTop = Color.White;
    //        subject_report.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorBottom = Color.White;
    //        if (form_name != "" && form_name != null)
    //        {
    //            subject_report.Sheets[0].ColumnHeader.Cells[4, col_count].Text = form_name;
    //            subject_report.Sheets[0].ColumnHeader.Cells[5, col_count].Text = "----------------------------------------------------";
    //            subject_report.Sheets[0].Rows[5].Visible = false;
    //        }
    //        if (final_print_col_cnt > 3)
    //        {
    //            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(4, col_count, 1, (end_column - col_count));
    //            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(5, col_count, 1, (end_column - col_count));
    //        }
    //        subject_report.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorTop = Color.White;
    //        subject_report.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorBottom = Color.White;
    //        subject_report.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorTop = Color.White;
    //        subject_report.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorBottom = Color.White;
    //        subject_report.Sheets[0].ColumnHeader.Cells[6, col_count].Text = degree_deatil;
    //        if (final_print_col_cnt > 3)
    //        {
    //            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(6, col_count, 1, (end_column - col_count));
    //        }
    //        subject_report.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorTop = Color.White;
    //        subject_report.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorBottom = Color.White;
    //        subject_report.Sheets[0].ColumnHeader.Cells[7, col_count].Text = "From: " + txtFromDate.Text + "      To: " + txtToDate.Text + "       Date: " + DateTime.Now.ToString("dd/MM/yyyy");
    //        if (final_print_col_cnt > 3)
    //        {
    //            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(7, col_count, 1, (end_column - col_count));
    //        }
    //        subject_report.Sheets[0].ColumnHeader.Cells[7, col_count].Border.BorderColorTop = Color.White;
    //        int temp_count_temp = 0;
    //        if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
    //        {
    //            if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
    //            {
    //                string[] new_header_string_index_split = new_header_string_index.Split(',');
    //                new_header_string_split = (dsprint.Tables[0].Rows[0]["new_header_name"].ToString()).Split(',');
    //                subject_report.Sheets[0].ColumnHeader.Cells[7, col_count].Border.BorderColorBottom = Color.White;
    //                for (int row_head_count = 8; row_head_count < (8 + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
    //                {
    //                    subject_report.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Text = new_header_string_split[temp_count_temp].ToString();
    //                    if (final_print_col_cnt > 3)
    //                    {
    //                        subject_report.Sheets[0].ColumnHeaderSpanModel.Add(row_head_count, col_count, 1, (end_column - col_count));
    //                    }
    //                    subject_report.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorTop = Color.White;
    //                    if (row_head_count != (8 + new_header_string_split.GetUpperBound(0)))
    //                    {
    //                        subject_report.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorBottom = Color.White;
    //                    }
    //                    if (temp_count_temp <= new_header_string_index_split.GetUpperBound(0))//----29/6/12 PRABAH
    //                    {
    //                        header_alignment = new_header_string_index_split[temp_count_temp].ToString();
    //                        if (header_alignment != string.Empty)
    //                        {
    //                            if (header_alignment == "2")
    //                            {
    //                                subject_report.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Center;
    //                            }
    //                            else if (header_alignment == "1")
    //                            {
    //                                subject_report.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Left;
    //                            }
    //                            else
    //                            {
    //                                subject_report.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Right;
    //                            }
    //                        }
    //                    }
    //                    temp_count_temp++;
    //                }
    //            }
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    //End Cmd Saran
    #endregion

    public void header_text()
    {
        try
        {
            bool check_print_row = false;
            SqlDataReader dr_collinfo;
            con.Close();
            con.Open();
            cmd = new SqlCommand("select isnull(college_name,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website,isnull(form_heading_name,'') as form_name,isnull(batch_degree_branch,'') as degree_deatil,isnull(header_align,'') as header_alignment,isnull(header_flag_value,'') as view_header from print_master_setting  where form_name='subjwiseattndreport.aspx'", con);
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
                        string sec_val = string.Empty;
                        //if (ddlSec.SelectedValue.ToString() != string.Empty && ddlSec.SelectedValue.ToString() != null)
                        //{
                        //    sec_val = "Section: " + ddlSec.SelectedItem.ToString();
                        //}
                        //else
                        //{
                        //    sec_val =string.Empty;
                        //}
                        check_print_row = true;
                        coll_name = dr_collinfo["collname"].ToString();
                        address1 = dr_collinfo["address1"].ToString();
                        address2 = dr_collinfo["address2"].ToString();
                        address3 = dr_collinfo["address3"].ToString();
                        phoneno = dr_collinfo["phoneno"].ToString();
                        faxno = dr_collinfo["faxno"].ToString();
                        email = dr_collinfo["email"].ToString();
                        website = dr_collinfo["website"].ToString();
                        form_name = "  Attendance Shortage Details - Regulation Report ";
                        degree_deatil = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
                        //  header_alignment = ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlSemYr.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
                        // view_header = dr_collinfo["view_header"].ToString();
                    }
                }
            }
        }
        catch
        {
        }
    }

    public void print_btngo()
    {
        try
        {
            final_print_col_cnt = 0;
            errmsg.Visible = false;
            check_col_count_flag = false;
            hat.Clear();
            hat.Add("college_code", Session["collegecode"].ToString());
            hat.Add("form_name", "subjwiseattndreport.aspx");
            dsprint = da.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {

                btnclick();
                //1.set visible columns
                column_field = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------------get column field
                if (column_field != "" && column_field != null)
                {
                    //  check_col_count_flag = true;
                    //for (col_count_all = 0; col_count_all < data.Columns.Count; col_count_all++)
                    //{
                    //    data.Columns[col_count_all].Visible = false;//------------invisible all column                                
                    //}
                    printvar = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------visible setting columns
                    string[] split_printvar = printvar.Split(',');
                    for (int splval = 0; splval <= split_printvar.GetUpperBound(0); splval++)
                    {
                        span_cnt = 0;
                        string[] split_star = split_printvar[splval].Split('*');
                        //if (split_star.GetUpperBound(0) > 0)
                        //{
                        //    for (col_count = 0; col_count < subject_report.Sheets[0].ColumnCount - 2; col_count++)
                        //    {
                        //        if (subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].ColumnHeader.RowCount - 2), col_count].Text == split_star[0])
                        //        {
                        //            child_span_count = 0;
                        //            string[] split_star_doller = split_star[1].Split('$');
                        //            for (int doller_count = 1; doller_count < split_star_doller.GetUpperBound(0); doller_count++)
                        //            {
                        //                for (int child_node = col_count; child_node <= col_count + split_star_doller.GetUpperBound(0); child_node++)
                        //                {
                        //                    if (subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].ColumnHeader.RowCount - 2), child_node].Text == split_star_doller[doller_count])
                        //                    {
                        //                        span_cnt++;
                        //                        if (span_cnt == 1 && child_node == col_count + 1)
                        //                        {
                        //                            subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].ColumnHeader.RowCount - 3), col_count + 1].Text = split_star[0].ToString();
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
                        //                        subject_report.Sheets[0].ColumnHeaderSpanModel.Add((subject_report.Sheets[0].ColumnHeader.RowCount - 3), col_count, 1, span_cnt);
                        //                        subject_report.Sheets[0].Columns[child_node].Visible = true;
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
                        // else
                        {
                            //for (col_count = 0; col_count < subject_report.Sheets[0].ColumnCount; col_count++)
                            //{
                            //    if (subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].ColumnHeader.RowCount - 3), col_count].Text == split_printvar[splval])
                            //    {
                            //        subject_report.Sheets[0].Columns[col_count].Visible = true;
                            //        if (basedddl.SelectedIndex.ToString() == "1")
                            //        {
                            //            if (col_count > 4 && col_count < subject_report.Sheets[0].ColumnCount - 3)
                            //            {
                            //                subject_report.Sheets[0].Columns[col_count + 1].Visible = true;
                            //                final_print_col_cnt++;
                            //            }
                            //        }
                            //        final_print_col_cnt++;
                            //        break;
                            //    }
                            //}
                        }
                    }
                    //1 end.set visible columns
                }
                else
                {
                    Showgrid.Visible = false;
                    setpanel.Visible = false;
                    errmsg.Visible = true;
                    errmsg.Text = "Select Atleast One Column Field From The Treeview";
                }
            }
            // subject_report.Width = final_print_col_cnt * 100;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void loadstudent()
    {
        try
        {


            includediscon = " and delflag=0";
            includedebar = " and exam_flag <> 'DEBAR'";
            includedisco = " and r.delflag=0";
            includedeba = " and r.exam_flag <> 'DEBAR'";
            string getshedulockva = da.GetFunctionv("select value from Master_Settings where settings='Attendance Discount' " + grouporusercode1 + "");
            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
            {
                includediscon = string.Empty;
                includedisco = "";
            }
            getshedulockva = da.GetFunctionv("select value from Master_Settings where  settings='Attendance Debar' " + grouporusercode1 + " ");
            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
            {
                includedebar = string.Empty;
                includedeba = "";
            }

            Double checkPercentage = 0;
            double.TryParse(Convert.ToString(pertxt.Text), out checkPercentage);
            Dictionary<DateTime, byte> dicAlternateDayOrder = new Dictionary<DateTime, byte>();//magesh 3.9.18
            int sturollcount = 0;
            Dictionary<string, int> totlabbatchconhr = new Dictionary<string, int>();
            //-------------set section value
            strsec = string.Empty;
            if (ddlsec.Items.Count > 0)
            {
                if (ddlsec.SelectedValue.ToString().Trim() == "" || ddlsec.SelectedValue.ToString().Trim() == "-1" || ddlsec.SelectedValue.ToString().Trim().ToLower() == "all")
                {
                    strsec = string.Empty;
                }
                else
                {
                    // strsec = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
                    strsec = ddlsec.SelectedValue.ToString().Trim();
                }
            }

            if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
            {
                noofhrs = ds.Tables[2].Rows[0][0].ToString();
                no_of_hrs = Convert.ToInt16(noofhrs.ToString());
                mng_hrs = ds.Tables[2].Rows[0][1].ToString();
                first_half = Convert.ToInt16(mng_hrs.ToString());
                evng_hrs = ds.Tables[2].Rows[0][2].ToString();
                sec_half = Convert.ToInt16(evng_hrs.ToString());
            }
            //noofhrs = GetFunction("select No_of_hrs_per_day from periodattndschedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + "");

            arrColHdrNames1.Add("Tot No Hrs");
            arrColHdrNames1.Add("Percentage");
            arrColHdrNames1.Add("Remarks%");

            colHdrIndx++;
            arrColHdrNames2.Add("Tot No Hrs");
            data.Columns.Add("col" + colHdrIndx);
            colHdrIndx++;
            arrColHdrNames2.Add("Percentage");
            data.Columns.Add("col" + colHdrIndx);
            arrColHdrNames2.Add("Remarks%");
            colHdrIndx++;
            data.Columns.Add("col" + colHdrIndx);


            DataRow drHdr1 = data.NewRow();
            DataRow drHdr2 = data.NewRow();


            for (int grCol = 0; grCol < data.Columns.Count; grCol++)
            {

                drHdr1["col" + grCol] = arrColHdrNames1[grCol];
                drHdr2["col" + grCol] = arrColHdrNames2[grCol];


            }

            data.Rows.Add(drHdr1);
            data.Rows.Add(drHdr2);


            //-----------get student
            string[] dm_splt_new = fdate.ToString().Split('/');
            string[] date_increment_splt_new = tdate.ToString().Split('/');
            DateTime alt;
            from_date = Convert.ToDateTime(dm_splt_new[1].ToString() + "/" + dm_splt_new[0].ToString() + "/" + dm_splt_new[2].ToString());
            to_date = Convert.ToDateTime(date_increment_splt_new[1].ToString() + "/" + date_increment_splt_new[0].ToString() + "/" + date_increment_splt_new[2].ToString());
            t_date = to_date;
            f_date = from_date;
            //Added by Srinath 9/9/2014=====Start====================================================
            string getsec = string.Empty;
            string includePastout = string.Empty;
            string includePastout1 = string.Empty;

            if (!chkincludepastout.Checked)
            {
                includePastout = "and r.CC=0";
                includePastout1 = "and CC=0";
            }

            //else
            //{
            //    includePastout = "and r.CC=1";
            //}


            if (strsec != "")
            {
                getsec = " and Sections='" + strsec + "'";
            }



            string getdeteails = "select convert(nvarchar(15),s.start_date,101) as start_date,nodays,s.starting_dayorder from seminfo s,PeriodAttndSchedule p where s.degree_code=p.degree_code and p.semester=s.semester and s.semester='" + ddlduration.SelectedItem.ToString() + "' and s.batch_year='" + ddlbatch.Text.ToString() + "'  and s.degree_code='" + ddlbranch.SelectedValue.ToString() + "'";
            getdeteails = getdeteails + " ; select * from tbl_consider_day_order where semester='" + ddlduration.SelectedItem.ToString() + "' and batch_year='" + ddlbatch.Text.ToString() + "'  and degree_code='" + ddlbranch.SelectedValue.ToString() + "'";

            getdeteails = getdeteails + " ; select s.Batch,s.subject_no,r.Roll_No,s.fromdate,l.Hour_Value,l.Day_Value  from subjectChooser_New s,Registration r,LabAlloc_New l where r.Roll_No=s.roll_no and r.Current_Semester=s.semester and l.Stu_Batch=s.Batch and l.Batch_Year=r.Batch_Year and l.Degree_Code=r.degree_code and l.Semester=r.Current_Semester and l.Sections=r.Sections and l.Subject_No=s.subject_no and l.Semester=s.semester and l.fdate=s.fromdate " + includePastout + " " + includedisco + includedeba + " and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' " + ((chkincludepastout.Checked) ? "  and r.current_semester='" + ddlduration.SelectedItem.ToString() + "'" : "") + " and s.fromdate between '" + f_date.ToString("MM/dd/yyyy") + "' and '" + t_date.ToString("MM/dd/yyyy") + "'";

            getdeteails = getdeteails + " ; Select * from Attendance where roll_no in(Select Roll_no from Registration  where  degree_code='" + ddlbranch.SelectedValue.ToString() + "' " + includediscon + includePastout1 + includedebar + " and Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' " + ((chkincludepastout.Checked) ? " and current_semester='" + ddlduration.SelectedItem.ToString() + "'" : "") + " " + getsec + ")";
            getdeteails = getdeteails + " ; select * from Semester_Schedule where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' and semester='" + ddlduration.SelectedItem.ToString() + "' " + getsec + " order by FromDate desc";   //((!chkincludepastout.Checked) ? has been changed by prabha  feb 03 2018
            DataSet dssem = da.select_method_wo_parameter(getdeteails, "Text");
            string semstartdate = string.Empty;
            string noofdays = string.Empty;
            string startday = string.Empty;
            if (dssem.Tables.Count > 0 && dssem.Tables[0].Rows.Count > 0)
            {
                semstartdate = dssem.Tables[0].Rows[0]["start_date"].ToString();
                noofdays = dssem.Tables[0].Rows[0]["nodays"].ToString();
                startday = dssem.Tables[0].Rows[0]["starting_dayorder"].ToString();
            }
            Hashtable hatdc = new Hashtable();
            try
            {
                if (dssem.Tables.Count > 1 && dssem.Tables[1].Rows.Count > 0)
                {
                    for (int dc = 0; dc < dssem.Tables[1].Rows.Count; dc++)
                    {
                        DateTime dtdcf = Convert.ToDateTime(dssem.Tables[1].Rows[dc]["from_date"].ToString());
                        DateTime dtdct = Convert.ToDateTime(dssem.Tables[1].Rows[dc]["to_date"].ToString());
                        string asperday = Convert.ToString(dssem.Tables[1].Rows[dc]["include_attendance"].ToString());
                        string alternateDayOrder = Convert.ToString(dssem.Tables[1].Rows[dc]["DayOrder"]).Trim();
                        byte alternateDay = 0;
                        byte.TryParse(alternateDayOrder, out alternateDay);
                        for (DateTime dtc = dtdcf; dtc <= dtdct; dtc = dtc.AddDays(1))
                        {
                            if (asperday != "1") //magesh 3.9.18
                            {
                                if (!hatdc.Contains(dtc))
                                {
                                    hatdc.Add(dtc, dtc);
                                }
                            }
                            else
                            {
                                //magesh 3.9.18
                                if (!dicAlternateDayOrder.ContainsKey(dtc))
                                {
                                    dicAlternateDayOrder.Add(dtc, alternateDay);
                                } //magesh 3.9.18
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            Dictionary<string, int> diclabsub = new Dictionary<string, int>();
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                for (int ls = 0; ls < ds.Tables[1].Rows.Count; ls++)
                {
                    string subno = ds.Tables[1].Rows[ls]["Subject_no"].ToString();
                    string strlab = ds.Tables[1].Rows[ls]["Lab"].ToString();
                    if (strlab.Trim().ToLower() == "true" || strlab.Trim() == "1")
                    {
                        if (!diclabsub.ContainsKey(subno))
                        {
                            diclabsub.Add(subno, 1);
                        }
                    }
                }
            }
            //====================End=====================================================
            //===========================End===========================================================
            if (basedddl.SelectedValue.ToString() == "0")
            {
                int arow = 0;
                int[] array_subject = new int[data.Columns.Count - 7];
                int[] array_value = new int[data.Columns.Count - 7];
                int[] array_attnd = new int[data.Columns.Count - 7];
                int[] array_subject_hour_count = new int[data.Columns.Count - 7];
                int[] array_individualsubject_hour_count = new int[data.Columns.Count - 7];
                int[] array_attnd_individualsubject_hour_count = new int[data.Columns.Count - 7];
                int[] array_lab_hour_count = new int[data.Columns.Count - 7];
                Array.Clear(array_subject_hour_count, 0, array_subject_hour_count.Length);
                Array.Clear(array_subject, 0, array_subject.Length);
                Array.Clear(array_value, 0, array_value.Length);
                Array.Clear(array_attnd, 0, array_attnd.Length);
                Array.Clear(array_individualsubject_hour_count, 0, array_individualsubject_hour_count.Length);
                Array.Clear(array_attnd_individualsubject_hour_count, 0, array_attnd_individualsubject_hour_count.Length);
                Array.Clear(array_lab_hour_count, 0, array_lab_hour_count.Length);
                List<string> roll_no = new List<string>();
                List<string> daily = new List<string>();
                List<string> present_table = new List<string>();
                List<string> roll_count = new List<string>();
                Dictionary<string, string> special = new Dictionary<string, string>();
                //  List<string> holiday = new List<string>();
                Dictionary<string, string> dayvalue = new Dictionary<string, string>();
                // Dictionary<string, string> null_table = new Dictionary<string, string>();
                Dictionary<string, string> attend_table = new Dictionary<string, string>();
                Dictionary<string, string> subject = new Dictionary<string, string>();
                Dictionary<string, string> lab = new Dictionary<string, string>();
                i = 0;
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        array_subject[i] = Convert.ToInt16(ds.Tables[1].Rows[i][1].ToString());
                    }
                    subject_count = ds.Tables[1].Rows.Count;
                }
                if ((ds.Tables.Count > 15 && ds.Tables[15].Rows.Count > 0) || (ds.Tables.Count > 17 && ds.Tables[17].Rows.Count > 0))
                //if ((ds.Tables[15].Rows.Count != 0 || ds.Tables[17].Rows.Count != 0))
                // if ((roll_data.Tables[0].Rows.Count != 0) || (ds.Tables[15].Rows.Count != 0 && ds.Tables[16].Rows.Count != 0))
                {
                    if (ds.Tables.Count > 11 && ds.Tables[11].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[11].Rows.Count; k++)
                        {
                            if (!roll_no.Contains(ds.Tables[11].Rows[k][0].ToString() + "-" + ds.Tables[11].Rows[k][5].ToString()))
                            {
                                roll_no.Add(ds.Tables[11].Rows[k][0].ToString() + "-" + ds.Tables[11].Rows[k][5].ToString());
                            }
                        }
                    }
                    if (ds.Tables.Count > 9 && ds.Tables[9].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[9].Rows.Count; k++)
                        {
                            if (!present_table.Contains(ds.Tables[9].Rows[k][1].ToString()))
                            {
                                present_table.Add(ds.Tables[9].Rows[k][1].ToString());
                            }
                        }
                    }
                    if (ds.Tables.Count > 16 && ds.Tables[16].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[16].Rows.Count; k++)
                        {
                            if (!special.ContainsKey(ds.Tables[16].Rows[k][1].ToString() + "-" + ds.Tables[16].Rows[k][2].ToString() + "-" + ds.Tables[16].Rows[k][3].ToString()))
                            {
                                special.Add(ds.Tables[16].Rows[k][1].ToString() + "-" + ds.Tables[16].Rows[k][2].ToString() + "-" + ds.Tables[16].Rows[k][3].ToString(), ds.Tables[16].Rows[k][4].ToString());
                            }
                        }
                    }
                    if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                    {
                        noofhrs = ds.Tables[2].Rows[0][0].ToString();
                        no_of_hrs = Convert.ToInt16(noofhrs.ToString());
                    }
                    if (ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
                    {
                        noofday = ds.Tables[3].Rows[0][0].ToString();
                        no_of_days = Convert.ToInt16(noofday.ToString());
                    }
                    if (ds.Tables.Count > 5 && ds.Tables[5].Rows.Count > 0)
                    {
                        str_order = ds.Tables[5].Rows[0][0].ToString();
                        strorder = Convert.ToInt16(str_order.ToString());
                    }
                    if (ds.Tables.Count > 4 && ds.Tables[4].Rows.Count > 0)
                    {
                        start_date = ds.Tables[4].Rows[0][0].ToString();
                        s_date = Convert.ToDateTime(start_date);
                    }
                    if (ds.Tables.Count > 18 && ds.Tables[18].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[18].Rows.Count; k++)
                        {
                            //Added By Srinath 29/8/2013
                            if (!subject.ContainsKey(ds.Tables[18].Rows[k][0].ToString() + "-" + ds.Tables[18].Rows[k][1].ToString()))
                            {
                                subject.Add(ds.Tables[18].Rows[k][0].ToString() + "-" + ds.Tables[18].Rows[k][1].ToString(), ds.Tables[18].Rows[k][2].ToString());
                            }
                        }
                    }
                    if (ds.Tables.Count > 19 && ds.Tables[19].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[19].Rows.Count; k++)
                        {
                            get_date_holiday = ds.Tables[19].Rows[k][0].ToString();
                            string[] split_date = ds.Tables[19].Rows[k][0].ToString().Split('/');
                            get_date_holiday = ((split_date[1].ToString())).ToString() + "/" + ((split_date[0].ToString())).ToString() + "/" + ((split_date[2].ToString())).ToString();
                            if (ds.Tables[19].Rows[k]["halforfull"].ToString() == "False")
                            {
                                halforfull = "0";
                            }
                            else
                            {
                                halforfull = "1";
                            }
                            if (ds.Tables[19].Rows[k]["morning"].ToString() == "False")
                            {
                                mng = "0";
                            }
                            else
                            {
                                mng = "1";
                            }
                            if (ds.Tables[19].Rows[k]["evening"].ToString() == "False")
                            {
                                evng = "0";
                            }
                            else
                            {
                                evng = "1";
                            }
                            holiday_sched_details = halforfull + "*" + mng + "*" + evng;
                            if (!holiday.ContainsKey(get_date_holiday))
                            {
                                holiday.Add(get_date_holiday, holiday_sched_details);
                            }
                        }
                    }
                    if (ds.Tables.Count > 20 && ds.Tables[20].Rows.Count != 0)
                    {
                        for (int k = 0; k < ds.Tables[20].Rows.Count; k++)
                        {
                            if (!dayvalue.ContainsKey(ds.Tables[20].Rows[k][0].ToString() + "-" + ds.Tables[20].Rows[k][2].ToString() + "-" + ds.Tables[20].Rows[k][3].ToString()))
                            {
                                dayvalue.Add(ds.Tables[20].Rows[k][0].ToString() + "-" + ds.Tables[20].Rows[k][2].ToString() + "-" + ds.Tables[20].Rows[k][3].ToString(), ds.Tables[20].Rows[k][1].ToString());
                            }
                        }
                    }

                    while (roll_data.Tables.Count > 0 && rollcount < roll_data.Tables[0].Rows.Count)
                    {
                        tval = 0;
                        Array.Clear(array_subject_hour_count, 0, array_subject_hour_count.Length);
                        Array.Clear(array_subject, 0, array_subject.Length);
                        Array.Clear(array_value, 0, array_value.Length);
                        Array.Clear(array_attnd, 0, array_attnd.Length);
                        // Array.Clear(array_attnd_individualsubject_hour_count, 0, array_attnd_individualsubject_hour_count.Length);
                        Array.Clear(array_individualsubject_hour_count, 0, array_individualsubject_hour_count.Length);
                        total_conducted_hrs = 0;
                        total_attended_hrs = 0;
                        whole_total_conducted_hrs = 0;



                        drow = data.NewRow();
                        data.Rows.Add(drow);
                        for (int c = 0; c < colcount; c++)
                        {
                            data.Rows[data.Rows.Count - 1][c] = (Convert.ToInt16(arow) + 1).ToString();
                            if (Convert.ToString(Session["Rollflag"]) == "1")
                            {
                                c++;
                                data.Rows[data.Rows.Count - 1][c] = roll_data.Tables[0].Rows[rollcount][0].ToString();


                            }
                            else
                            {
                                c++;
                                data.Rows[data.Rows.Count - 1][c] = roll_data.Tables[0].Rows[rollcount][0].ToString();

                            }
                            if (Convert.ToString(Session["Regflag"]) == "1")
                            {
                                c++;
                                data.Rows[data.Rows.Count - 1][c] = roll_data.Tables[0].Rows[rollcount][2].ToString();

                            }

                            c++;
                            data.Rows[data.Rows.Count - 1][c] = roll_data.Tables[0].Rows[rollcount][3].ToString();

                            string del = roll_data.Tables[0].Rows[rollcount][7].ToString();
                            string examf = roll_data.Tables[0].Rows[rollcount][8].ToString();
                            if (del == "1" || examf.ToUpper() == "DEBAR")
                            {
                                dicdiscon.Add(data.Rows.Count - 1, roll_data.Tables[0].Rows[rollcount][0].ToString());
                            }
                        }
                        int daily_count = 0;
                        if (ds.Tables.Count > 15)
                        {
                            while (daily_count < ds.Tables[15].Rows.Count)
                            {
                                d = string.Empty;
                                d1 = string.Empty;
                                d = ds.Tables[15].Rows[daily_count][1].ToString();
                                f_date = Convert.ToDateTime(d);
                                d1 = "d" + f_date.Day.ToString() + "d" + ds.Tables[15].Rows[daily_count]["hr"].ToString();
                                rollcolumncount = (Convert.ToInt32(f_date.Month.ToString()) + (Convert.ToInt32(f_date.Year.ToString()) * 12));
                                if (roll_no.Contains(roll_data.Tables[0].Rows[rollcount][1].ToString() + "-" + rollcolumncount.ToString()))
                                {
                                    rollcolumncount = roll_no.IndexOf(roll_data.Tables[0].Rows[rollcount][1].ToString() + "-" + rollcolumncount.ToString());
                                    s_code = ds.Tables[15].Rows[daily_count][6].ToString().Split(';');
                                    if (s_code.GetUpperBound(0) >= 0)
                                    {
                                        for (upper_bnd = 0; upper_bnd <= s_code.GetUpperBound(0); upper_bnd++)
                                        {
                                            dummy_split = s_code[upper_bnd].ToString().Split('-');
                                            if (subject.ContainsKey(ds.Tables[11].Rows[rollcolumncount]["roll_no"].ToString() + "-" + dummy_split[0].ToString()))
                                            // if (subject.Contains(ds.Tables[11].Rows[rollcolumncount]["roll_no"].ToString() + "-" + dummy_split[0].ToString()))
                                            {
                                                da1 = subject[ds.Tables[11].Rows[rollcolumncount]["roll_no"].ToString() + "-" + dummy_split[0].ToString()];
                                                if (da1 == "")
                                                {
                                                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                                                    {
                                                        for (k = 0; k < ds.Tables[1].Rows.Count; k++)
                                                        {
                                                            if (s_code[upper_bnd].Contains(ds.Tables[1].Rows[k][1].ToString()))
                                                            {
                                                                //array_subject_hour_count[k] = array_subject_hour_count[k] + 1;
                                                                array_value[k] = array_value[k] + 1;
                                                                if (present_table.Contains(ds.Tables[11].Rows[rollcolumncount][d1].ToString()))
                                                                {
                                                                    sume = (f_date).ToString("M/d/yyyy");
                                                                    h = string.Empty;
                                                                    if (attend_table.ContainsKey(sume + "-" + ds.Tables[15].Rows[daily_count]["hr"].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                                                    {
                                                                        h = attend_table[sume + "-" + ds.Tables[15].Rows[daily_count]["hr"].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()];
                                                                        attend_table[sume + "-" + ds.Tables[15].Rows[daily_count]["hr"].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()] = (Convert.ToInt16(h) + 1).ToString();
                                                                    }
                                                                    else
                                                                    {
                                                                        attend_table.Add(sume + "-" + ds.Tables[15].Rows[daily_count]["hr"].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString(), "1");
                                                                    }
                                                                    array_attnd[k] = array_attnd[k] + 1;
                                                                    k = ds.Tables[1].Rows.Count;
                                                                    upper_bnd = s_code.GetUpperBound(0);
                                                                }
                                                                else if ((ds.Tables[11].Rows[rollcolumncount][d1].ToString() == "") || (ds.Tables[11].Rows[rollcolumncount][d1].ToString() == null) || (ds.Tables[11].Rows[rollcolumncount][d1].ToString() == "0") || (ds.Tables[11].Rows[rollcolumncount][d1].ToString() == "8"))
                                                                {
                                                                    //sume = (f_date).ToString("MM/dd/yyyy");
                                                                    //if (holiday.Contains(sume))
                                                                    //{
                                                                    //    array_value[k] = array_value[k] - 1;
                                                                    //    k = ds.Tables[1].Rows.Count;
                                                                    //    upper_bnd = s_code.GetUpperBound(0);
                                                                    //    //    array_subject_hour_count[k] = array_subject_hour_count[k] - 1;
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    //    array_value[k] = array_value[k] - 1;
                                                                    //    k = ds.Tables[1].Rows.Count;
                                                                    //    upper_bnd = s_code.GetUpperBound(0);
                                                                    //}
                                                                    array_value[k] = array_value[k] - 1;
                                                                    k = ds.Tables[1].Rows.Count;
                                                                    upper_bnd = s_code.GetUpperBound(0);
                                                                }
                                                                else
                                                                {
                                                                    sume = (f_date).ToString("M/d/yyyy");
                                                                    h = string.Empty;
                                                                    if (attend_table.ContainsKey(sume + "-" + ds.Tables[15].Rows[daily_count]["hr"].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                                                    {
                                                                        h = attend_table[sume + "-" + ds.Tables[15].Rows[daily_count]["hr"].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()];
                                                                        attend_table[sume + "-" + ds.Tables[15].Rows[daily_count]["hr"].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()] = (Convert.ToInt16(h) + 1).ToString();
                                                                    }
                                                                    else
                                                                    {
                                                                        attend_table.Add(sume + "-" + ds.Tables[15].Rows[daily_count]["hr"].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString(), "1");
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                srt_day = f_date.ToString("ddd");
                                                if (dayvalue.ContainsKey(srt_day + "-" + dummy_split[0].ToString() + "-" + ds.Tables[15].Rows[daily_count]["hr"].ToString()))
                                                {
                                                    davalue = dayvalue[srt_day + "-" + dummy_split[0].ToString() + "-" + ds.Tables[15].Rows[daily_count]["hr"].ToString()];
                                                    if (da1 == davalue)
                                                    {
                                                        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                                                        {
                                                            for (k = 0; k < ds.Tables[1].Rows.Count; k++)
                                                            {
                                                                if (s_code[upper_bnd].Contains(ds.Tables[1].Rows[k][1].ToString()))
                                                                {
                                                                    array_subject_hour_count[k] = array_subject_hour_count[k] + 1;
                                                                    tval = tval + 1;
                                                                    array_value[k] = array_value[k] + 1;
                                                                    if (present_table.Contains(ds.Tables[11].Rows[rollcolumncount][d1].ToString()))
                                                                    {
                                                                        sume = (f_date).ToString("M/d/yyyy");
                                                                        h = string.Empty;
                                                                        if (lab.ContainsKey(sume + "-" + ds.Tables[15].Rows[daily_count]["hr"].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                                                        {
                                                                            h = lab[sume + "-" + ds.Tables[15].Rows[daily_count]["hr"].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()];
                                                                            lab[sume + "-" + ds.Tables[15].Rows[daily_count]["hr"].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()] = (Convert.ToInt16(h) + 1).ToString();
                                                                        }
                                                                        else
                                                                        {
                                                                            lab.Add(sume + "-" + ds.Tables[15].Rows[daily_count]["hr"].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString(), "1");
                                                                        }
                                                                        array_attnd[k] = array_attnd[k] + 1;
                                                                        k = ds.Tables[1].Rows.Count;
                                                                        upper_bnd = s_code.GetUpperBound(0);
                                                                    }
                                                                    else if ((ds.Tables[11].Rows[rollcolumncount][d1].ToString() == "") || (ds.Tables[11].Rows[rollcolumncount][d1].ToString() == null) || (ds.Tables[11].Rows[rollcolumncount][d1].ToString() == "0") || (ds.Tables[11].Rows[rollcolumncount][d1].ToString() == "8"))
                                                                    {
                                                                        //sume = (f_date).ToString("MM/dd/yyyy");
                                                                        //if (holiday.Contains(sume))
                                                                        //{
                                                                        //    array_value[k] = array_value[k] - 1;
                                                                        //    k = ds.Tables[1].Rows.Count;
                                                                        //    upper_bnd = s_code.GetUpperBound(0);
                                                                        //  //  array_subject_hour_count[k] = array_subject_hour_count[k] - 1;
                                                                        // //   tval = tval - 1;
                                                                        //}
                                                                        //else
                                                                        //{
                                                                        //    array_value[k] = array_value[k] - 1;
                                                                        //    k = ds.Tables[1].Rows.Count;
                                                                        //    upper_bnd = s_code.GetUpperBound(0);
                                                                        //}
                                                                        array_value[k] = array_value[k] - 1;
                                                                        k = ds.Tables[1].Rows.Count;
                                                                        upper_bnd = s_code.GetUpperBound(0);
                                                                    }
                                                                    else
                                                                    {
                                                                        sume = (f_date).ToString("M/d/yyyy");
                                                                        h = string.Empty;
                                                                        if (lab.ContainsKey(sume + "-" + ds.Tables[15].Rows[daily_count]["hr"].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                                                        {
                                                                            h = lab[sume + "-" + ds.Tables[15].Rows[daily_count]["hr"].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()];
                                                                            lab[sume + "-" + ds.Tables[15].Rows[daily_count]["hr"].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()] = (Convert.ToInt16(h) + 1).ToString();
                                                                        }
                                                                        else
                                                                        {
                                                                            lab.Add(sume + "-" + ds.Tables[15].Rows[daily_count]["hr"].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString(), "1");
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
                                daily_count++;
                            }
                        }
                        else if (ds.Tables.Count > 16 && ds.Tables[16].Rows.Count > 0)
                        {
                            daily_count = 0;
                            bg = string.Empty;
                            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                            {
                                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                            }
                            else
                            {
                                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                            }
                            con.Close();
                            cmd = new SqlCommand("select rights from  special_hr_rights where " + grouporusercode + "", con);
                            con.Open();
                            SqlDataReader dr_rights_spl_hr = cmd.ExecuteReader();
                            if (dr_rights_spl_hr.HasRows)
                            {
                                while (dr_rights_spl_hr.Read())
                                {
                                    string spl_hr_rights = string.Empty;
                                    Hashtable od_has = new Hashtable();
                                    spl_hr_rights = dr_rights_spl_hr["rights"].ToString();
                                    if (spl_hr_rights == "True" || spl_hr_rights == "true")
                                    {
                                        spl_hrs();
                                    }
                                }
                            }
                            while (ds.Tables.Count > 17 && daily_count < ds.Tables[17].Rows.Count)
                            {
                                if (special.ContainsKey(ds.Tables[17].Rows[daily_count][1].ToString() + "-" + ds.Tables[17].Rows[daily_count][2].ToString() + "-" + ds.Tables[16].Rows[rollcount][3].ToString()))
                                {
                                    bg = special[ds.Tables[17].Rows[daily_count][1].ToString() + "-" + ds.Tables[17].Rows[daily_count][2].ToString() + "-" + ds.Tables[16].Rows[rollcount][3].ToString()];
                                    d = string.Empty;
                                    d1 = string.Empty;
                                    d = ds.Tables[17].Rows[daily_count][0].ToString();
                                    f_date = Convert.ToDateTime(d);
                                    //  d1 = "d" + f_date .Day.ToString()+ "d" + ds.Tables[15].Rows[daily_count]["hr"].ToString();
                                    rollcolumncount = (Convert.ToInt32(f_date.Month.ToString()) + (Convert.ToInt32(f_date.Year.ToString()) * 12));
                                    if (roll_no.Contains(roll_data.Tables[0].Rows[rollcount][1].ToString() + "-" + rollcolumncount.ToString()))
                                    {
                                        rollcolumncount = roll_no.IndexOf(roll_data.Tables[0].Rows[rollcount][1].ToString() + "-" + rollcolumncount.ToString());
                                        s_code = ds.Tables[17].Rows[daily_count][1].ToString().Split(';');
                                        if (s_code.GetUpperBound(0) >= 0)
                                        {
                                            for (upper_bnd = 0; upper_bnd <= s_code.GetUpperBound(0); upper_bnd++)
                                            {
                                                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                                                {
                                                    for (k = 0; k < ds.Tables[1].Rows.Count; k++)
                                                    {
                                                        if (s_code[upper_bnd].Contains(ds.Tables[1].Rows[k][1].ToString()))
                                                        {
                                                            // array_subject_hour_count[k] = array_subject_hour_count[k] + 1;
                                                            array_value[k] = array_value[k] + 1;
                                                            if (present_table.Contains(bg))
                                                            {
                                                                sume = (f_date).ToString("M/d/yyyy");
                                                                h = string.Empty;
                                                                if (attend_table.ContainsKey(sume + "-" + ds.Tables[17].Rows[daily_count][2].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                                                {
                                                                    h = attend_table[sume + "-" + ds.Tables[17].Rows[daily_count][2].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()];
                                                                    attend_table[sume + "-" + ds.Tables[17].Rows[daily_count][2].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()] = (Convert.ToInt16(h) + 1).ToString();
                                                                }
                                                                else
                                                                {
                                                                    attend_table.Add(sume + "-" + ds.Tables[17].Rows[daily_count][2].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString(), "1");
                                                                }
                                                                array_attnd[k] = array_attnd[k] + 1;
                                                                k = ds.Tables[1].Rows.Count;
                                                                upper_bnd = s_code.GetUpperBound(0);
                                                            }
                                                            else if ((bg == "") || (bg == null) || (bg == "0") || (bg == "8"))
                                                            {
                                                                //array_individualsubject_hour_count[k] = array_individualsubject_hour_count[k] + 1;
                                                                array_value[k] = array_value[k] - 1;
                                                                k = ds.Tables[1].Rows.Count;
                                                                upper_bnd = s_code.GetUpperBound(0);
                                                            }
                                                            else
                                                            {
                                                                sume = (f_date).ToString("M/d/yyyy");
                                                                h = string.Empty;
                                                                if (attend_table.ContainsKey(sume + "-" + ds.Tables[17].Rows[daily_count][2].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                                                {
                                                                    h = attend_table[sume + "-" + ds.Tables[17].Rows[daily_count][2].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()];
                                                                    attend_table[sume + "-" + ds.Tables[17].Rows[daily_count][2].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()] = (Convert.ToInt16(h) + 1).ToString();
                                                                }
                                                                else
                                                                {
                                                                    attend_table.Add(sume + "-" + ds.Tables[17].Rows[daily_count][2].ToString() + "-" + ds.Tables[1].Rows[k][1].ToString(), "1");
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                daily_count++;
                            }
                        }
                        else
                        {
                            data.Rows.Remove(drow);
                            //subject_report.Sheets[0].RowCount = 0;
                            btnprintmaster.Visible = false;
                            btndirtPrint.Visible = false;
                            btnxl.Visible = false;
                            goto lab;
                        }
                        row = 5;
                        //if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                        //{
                        //    for (i = 5; i < (ds.Tables[1].Rows.Count * 2) + 4; i = i + 2)
                        //    {
                        //if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count != 0)
                        //{
                        //for (int j = 0; j < (ds.Tables[1].Rows.Count); j++)
                        //{
                        if (data.Columns.Count > 0)
                        {

                            for (int g = colcount; g < data.Columns.Count - 3; g = g + 2)
                            {

                                //   whole_total_conducted_hrs = ((whole_total_conducted_hrs) + Convert.ToInt16(array_subject_hour_count[row-4].ToString()));
                                total_conducted_hrs = ((total_conducted_hrs) + Convert.ToInt16(array_value[row - 4].ToString()));
                                total_attended_hrs = ((total_attended_hrs) + Convert.ToInt16(array_attnd[row - 4].ToString()));
                                // if (array_value[row - 5] == 0)



                                if (array_value[row - 4] == 0)
                                {
                                    data.Rows[data.Rows.Count - 1][g] = "-";
                                    data.Rows[data.Rows.Count - 1][g + 1] = "-";

                                }
                                //else if (array_attnd[row - 5] == 0)
                                else if (array_attnd[row - 4] == 0)
                                {

                                    data.Rows[data.Rows.Count - 1][g] = "0";
                                    data.Rows[data.Rows.Count - 1][g + 1] = "0";


                                }
                                else
                                {
                                    // attnd_perc_val = ((Convert.ToDouble(array_attnd[row - 5]) / Convert.ToDouble(array_value[row - 5])) * 100);
                                    attnd_perc_val = ((Convert.ToDouble(array_attnd[row - 4]) / Convert.ToDouble(array_value[row - 4])) * 100);
                                    avgstudent1 = Convert.ToDecimal(attnd_perc_val);
                                    avgstudent2 = Math.Round(avgstudent1);
                                    avgstudent3 = Convert.ToDouble(avgstudent2);
                                    attnd_perc = Convert.ToString(avgstudent3);

                                    data.Rows[data.Rows.Count - 1][g] = array_attnd[row - 4].ToString();
                                    data.Rows[data.Rows.Count - 1][g + 1] = attnd_perc.ToString();

                                }
                                row++;
                            }
                        }
                        if (total_conducted_hrs == 0)
                        {
                            attnd_perc = "-";
                        }
                        else if (total_attended_hrs == 0)
                        {
                            attnd_perc = "0";
                        }
                        else
                        {
                            if (ds.Tables.Count > 7 && ds.Tables[7].Rows.Count > 0)
                            {
                                if (ds.Tables[7].Rows[0][0].ToString() == "1")
                                {
                                    if (ds.Tables.Count > 8 && ds.Tables[8].Rows.Count != 0)
                                    {
                                        if (Convert.ToInt16(total_attended_hrs) >= Convert.ToInt16(ds.Tables[8].Rows[0][0].ToString()))
                                        {
                                            attnd_perc = "100";
                                        }
                                        else
                                        {
                                            attnd_perc_val = (Convert.ToDouble(total_attended_hrs) / Convert.ToDouble(total_conducted_hrs)) * 100;
                                            //-------convert
                                            avgstudent1 = Convert.ToDecimal(attnd_perc_val);
                                            avgstudent2 = Math.Round(avgstudent1);
                                            avgstudent3 = Convert.ToDouble(avgstudent2);
                                            attnd_perc = Convert.ToString(avgstudent3);
                                        }
                                    }
                                    else
                                    {
                                        attnd_perc_val = (Convert.ToDouble(total_attended_hrs) / Convert.ToDouble(total_conducted_hrs)) * 100;
                                        //-------convert
                                        avgstudent1 = Convert.ToDecimal(attnd_perc_val);
                                        avgstudent2 = Math.Round(avgstudent1);
                                        avgstudent3 = Convert.ToDouble(avgstudent2);
                                        attnd_perc = Convert.ToString(avgstudent3);
                                    }
                                }
                                else
                                {
                                    attnd_perc_val = (Convert.ToDouble(total_attended_hrs) / Convert.ToDouble(total_conducted_hrs)) * 100;
                                    //-------convert
                                    avgstudent1 = Convert.ToDecimal(attnd_perc_val);
                                    avgstudent2 = Math.Round(avgstudent1);
                                    avgstudent3 = Convert.ToDouble(avgstudent2);
                                    attnd_perc = Convert.ToString(avgstudent3);
                                }
                            }
                            else
                            {
                                attnd_perc_val = (Convert.ToDouble(total_hrs) / Convert.ToDouble(total_conducted_hrs)) * 100;
                                avgstudent1 = Convert.ToDecimal(attnd_perc_val);
                                avgstudent2 = Math.Round(avgstudent1);
                                avgstudent3 = Convert.ToDouble(avgstudent2);
                                attnd_perc = Convert.ToString(avgstudent3);
                            }
                        }
                        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                        {
                            i = (ds.Tables[1].Rows.Count * 2) + 5;
                        }
                        if (total_conducted_hrs == 0)
                        {
                            data.Rows[data.Rows.Count - 1][data.Columns.Count - 3] = "-";

                        }
                        else
                        {
                            data.Rows[data.Rows.Count - 1][data.Columns.Count - 3] = total_attended_hrs.ToString();


                        }
                        data.Rows[data.Rows.Count - 1][data.Columns.Count - 2] = attnd_perc.ToString();

                        data.Rows[data.Rows.Count - 1][data.Columns.Count - 1] = "";
                        if (percheck.Checked == true)
                        {
                            if (pertxt.Text != "")
                            {
                                find_filter(attnd_perc);
                                if (filter_flag == false)
                                {
                                    arow--;
                                    dicdiscon.Remove(data.Rows.Count - 1);
                                    data.Rows.RemoveAt(data.Rows.Count - 1);

                                }
                                else
                                {
                                    sturollcount++;
                                    // data.Rows[data.Rows.Count - 1][data.Columns.Count - 1] = sturollcount.ToString();

                                }
                            }
                        }

                        row = 5;
                        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                        {
                            for (i = 0; i < (ds.Tables[1].Rows.Count); i++)
                            {
                                if (array_individualsubject_hour_count[row - 5] > 0)
                                {
                                    array_attnd_individualsubject_hour_count[row - 5] = (array_attnd_individualsubject_hour_count[row - 5] + array_individualsubject_hour_count[row - 4]);
                                }
                                row++;
                            }
                        }
                        rollcount++;
                        arow++;

                    }
                    f_date = from_date;
                    f_date = from_date;
                    int count = 0;
                    tval = 0;
                    while (f_date <= t_date)
                    {
                        sume = f_date.Month.ToString() + "/" + f_date.Day.ToString() + "/" + f_date.Year.ToString();
                        for (int sdate = 1; sdate <= no_of_hrs; sdate++)
                        {
                            count = 0;
                            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                            {
                                for (k = 0; k < ds.Tables[1].Rows.Count; k++)
                                {
                                    if (attend_table.ContainsKey(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                    {
                                        count++;
                                        array_attnd_individualsubject_hour_count[k] = array_attnd_individualsubject_hour_count[k] + 1;
                                        k = ds.Tables[1].Rows.Count;
                                    }
                                }
                            }
                            if (count == 0)
                            {
                                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                                {
                                    for (k = 0; k < ds.Tables[1].Rows.Count; k++)
                                    {
                                        if (lab.ContainsKey(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                        {
                                            count++;
                                            array_lab_hour_count[k] = array_lab_hour_count[k] + 1;
                                        }
                                    }
                                }
                                if (count > 0)
                                {
                                    tval = tval + 1;
                                }
                            }
                        }
                        f_date = f_date.AddDays(1);
                    }
                    row = 5;
                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        int cnt = 0;
                        for (i = 0; i < (ds.Tables[1].Rows.Count); i++)
                        {
                            string subcode = ds.Tables[1].Rows[i][0].ToString();
                            dicsubhead.Add(subcode, (array_attnd_individualsubject_hour_count[row - 5] + array_lab_hour_count[row - 5]).ToString());
                            //subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].ColumnHeader.RowCount - 1), i].Text = (array_attnd_individualsubject_hour_count[row - 5] + array_lab_hour_count[row - 5]).ToString();
                            whole_total_conducted_hrs = whole_total_conducted_hrs + (array_attnd_individualsubject_hour_count[row - 5]);
                            row++;
                        }
                        i = (ds.Tables[1].Rows.Count * 2) + 5;
                    }
                    if (whole_total_conducted_hrs == 0)
                    {
                        // subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].ColumnHeader.RowCount - 1), i].Text = "-";
                    }
                    else
                    {
                        //subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].ColumnHeader.RowCount - 1), i].Text = (whole_total_conducted_hrs + tval).ToString();
                    }
                lab: out1 = string.Empty;
                }
            }
            else if (basedddl.SelectedValue.ToString() == "1")
            {
                int[] array_subject = new int[data.Columns.Count - 7];
                int[] array_value = new int[data.Columns.Count - 7];
                int[] array_attnd = new int[data.Columns.Count - 7];
                int[] array_subject_hour_count = new int[data.Columns.Count - 7];
                //  int[] array_individualsubject_hour_count = new int[subject_report.Sheets[0].ColumnCount - 7];
                int[] array_attnd_individualsubject_hour_count = new int[data.Columns.Count - 7];
                int[] array_individualsubject_hour_count = new int[no_of_hrs];
                int[] array_lab_hour_count = new int[data.Columns.Count - 7];
                Array.Clear(array_subject_hour_count, 0, array_subject_hour_count.Length);
                Array.Clear(array_subject, 0, array_subject.Length);
                Array.Clear(array_value, 0, array_value.Length);
                Array.Clear(array_attnd, 0, array_attnd.Length);
                Array.Clear(array_individualsubject_hour_count, 0, array_individualsubject_hour_count.Length);
                Array.Clear(array_attnd_individualsubject_hour_count, 0, array_attnd_individualsubject_hour_count.Length);
                Array.Clear(array_lab_hour_count, 0, array_lab_hour_count.Length);
                List<string> roll_no = new List<string>();
                List<string> alternate = new List<string>();
                List<string> semester = new List<string>();
                present_table.Clear();
                Dictionary<string, string> dicAutoSwitchLab = new Dictionary<string, string>();
                Dictionary<string, double> dicAutoLabConductHour = new Dictionary<string, double>();
                Dictionary<string, string> subject = new Dictionary<string, string>();
                Dictionary<string, string> find_day = new Dictionary<string, string>();
                //  List<string> holiday = new List<string>();
                Dictionary<string, string> dayvalue = new Dictionary<string, string>();
                Dictionary<string, string> null_table = new Dictionary<string, string>();
                Dictionary<string, string> attend_table = new Dictionary<string, string>();
                Dictionary<string, string> lab = new Dictionary<string, string>();
                DataSet dsAutoSwitch = new DataSet();
                if (!string.IsNullOrEmpty(Convert.ToString(ddlbatch.SelectedItem.Text).Trim()) && !string.IsNullOrEmpty(Convert.ToString(ddlbranch.SelectedValue).Trim()) && !string.IsNullOrEmpty(Convert.ToString(ddlduration.SelectedItem.Text).Trim()))
                {
                    string qry = "SELECT distinct sm.Batch_Year,sm.degree_code,sm.semester,ltrim(rtrim(isnull(l.Sections,''))) Sections,l.Timetablename,sch.FromDate,l.Day_Value,case when l.Day_Value='mon' then '1' when l.Day_Value='tue' then '2' when l.Day_Value='wed' then '3' when l.Day_Value='thu' then '4' when l.Day_Value='fri' then '5' when l.Day_Value='sat' then '6' when l.Day_Value='sun' then '7' end as Day_Code,l.Hour_Value,ltrim(rtrim(isnull(l.Auto_Switch,''))) as Auto_Switch,Count(l.Stu_Batch) as noOfBatch from LabAlloc l,syllabus_master sm,Semester_Schedule sch where l.Degree_Code=sm.degree_code and l.Batch_Year=sm.Batch_Year and sm.semester=l.Semester and sm.degree_code=sch.degree_code and l.Degree_Code=sch.degree_code and sch.batch_year=l.Batch_Year and sch.batch_year=l.Batch_Year and sch.semester=sm.semester and sch.semester=l.Semester and ltrim(rtrim(isnull(sch.Sections,'')))=ltrim(rtrim(isnull(l.Sections,''))) and TTName=l.Timetablename and sm.Batch_Year='" + Convert.ToString(ddlbatch.SelectedItem.Text).Trim() + "' and sm.degree_code='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and sm.semester='" + Convert.ToString(ddlduration.SelectedItem.Text).Trim() + "' and ltrim(rtrim(isnull(l.Sections,''))) ='" + strsec + "' group by sm.Batch_Year,sm.degree_code,sm.semester,l.Sections,l.Timetablename,sch.FromDate,l.Day_Value,l.Hour_Value,Auto_Switch order by sm.Batch_Year desc,sm.degree_code,sm.semester asc,Sections,l.Timetablename,l.Day_Value,l.Hour_Value,sch.FromDate,Day_Code";//l.Sections
                    dsAutoSwitch = da.select_method_wo_parameter(qry, "Text");
                }
                i = 0;
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        array_subject[i] = Convert.ToInt16(ds.Tables[1].Rows[i][1].ToString());
                    }
                    subject_count = ds.Tables[1].Rows.Count;
                }
                //  if(ds.Tables[11].Rows.Count != 0 )//
                {
                    if ((ds.Tables.Count > 12 && ds.Tables[12].Rows.Count > 0) || (ds.Tables.Count > 13 && ds.Tables[13].Rows.Count > 0))
                    // if ((roll_data.Tables[0].Rows.Count!=0) || (ds.Tables[12].Rows.Count != 0 && ds.Tables[13].Rows.Count != 0))
                    {
                        if (ds.Tables.Count > 12 && ds.Tables[12].Rows.Count > 0)
                        {
                            for (int k = 0; k < ds.Tables[12].Rows.Count; k++)
                            {
                                //alt = Convert.ToDateTime(ds.Tables[12].Rows[k][1].ToString());
                                string dateNew = Convert.ToString(ds.Tables[12].Rows[k][1]).Trim();
                                DateTime.TryParseExact(dateNew, "MM/dd/yyyy", null, DateTimeStyles.None, out alt);//alter By MAlang Raja
                                //date_increment_splt_new = ds.Tables[12].Rows[k][1].ToString().Split('/');
                                if (!alternate.Contains(alt.Month.ToString() + "/" + alt.Day.ToString() + "/" + alt.Year.ToString()))//Added By Srinath 14/8/2013
                                {
                                    alternate.Add(alt.Month.ToString() + "/" + alt.Day.ToString() + "/" + alt.Year.ToString());
                                }
                            }
                        }
                        if (ds.Tables.Count > 11 && ds.Tables[11].Rows.Count > 0)
                        {
                            for (int k = 0; k < ds.Tables[11].Rows.Count; k++)
                            {
                                if (!rol_no.Contains(ds.Tables[11].Rows[k][0].ToString() + "-" + ds.Tables[11].Rows[k][5].ToString()))//Added By Srinath 14/8/2013
                                {
                                    roll_no.Add(ds.Tables[11].Rows[k][0].ToString() + "-" + ds.Tables[11].Rows[k][5].ToString());
                                }
                            }
                        }
                        if (ds.Tables.Count > 9 && ds.Tables[9].Rows.Count > 0)
                        {
                            for (int k = 0; k < ds.Tables[9].Rows.Count; k++)
                            {
                                if (!present_table.Contains(ds.Tables[9].Rows[k][1].ToString()))//added by srinath 14/8/2013
                                {
                                    present_table.Add(ds.Tables[9].Rows[k][1].ToString());
                                }
                            }
                        }
                        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                        {
                            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                        }
                        else
                        {
                            grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                        }
                        con.Close();
                        cmd = new SqlCommand("select rights from special_hr_rights where " + grouporusercode + "", con);
                        con.Open();
                        SqlDataReader dr_rights_spl_hr = cmd.ExecuteReader();
                        if (dr_rights_spl_hr.HasRows)
                        {
                            while (dr_rights_spl_hr.Read())
                            {
                                string spl_hr_rights = string.Empty;
                                Hashtable od_has = new Hashtable();
                                spl_hr_rights = dr_rights_spl_hr["rights"].ToString();
                                if (spl_hr_rights == "True" || spl_hr_rights == "true")
                                {
                                    spl_hrs();
                                }
                            }
                        }
                        //===================================
                        if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                        {
                            noofhrs = ds.Tables[2].Rows[0][0].ToString();
                            no_of_hrs = Convert.ToInt16(noofhrs.ToString());
                        }
                        if (ds.Tables.Count > 3 && ds.Tables[3].Rows.Count != 0)
                        {
                            noofday = ds.Tables[3].Rows[0][0].ToString();
                            no_of_days = Convert.ToInt16(noofday.ToString());
                        }
                        if (ds.Tables.Count > 5 && ds.Tables[5].Rows.Count != 0)
                        {
                            str_order = ds.Tables[5].Rows[0][0].ToString();
                            strorder = Convert.ToInt16(str_order.ToString());
                        }
                        if (ds.Tables.Count > 4 && ds.Tables[4].Rows.Count != 0)
                        {
                            start_date = ds.Tables[4].Rows[0][0].ToString();
                            s_date = Convert.ToDateTime(start_date);
                            while (s_date <= t_date)
                            {
                                //Modified by Srinath 5/9/2014 For Day Order Change=======Start====================
                                //day_find = findday(no_of_days, s_date.Month.ToString() + "/" + s_date.Day.ToString() + "/" + s_date.Year.ToString(), t_date.Month.ToString() + "/" + t_date.Day.ToString() + "/" + t_date.Year.ToString());
                                day_find = da.findday(s_date.ToString(), ddlbranch.SelectedValue.ToString(), ddlduration.SelectedItem.ToString(), ddlbatch.Text.ToString(), semstartdate, noofdays, startday);
                                //======================ENd=========================================================
                                //magesh 3.9.18
                                if (dicAlternateDayOrder.ContainsKey(s_date))
                                {
                                    day_find = da.findDayName(dicAlternateDayOrder[s_date]);
                                    string Day_Order = Convert.ToString(dicAlternateDayOrder[s_date]).Trim();
                                } //magesh 3.9.18
                                if (!find_day.ContainsKey(s_date.Month.ToString() + "/" + s_date.Day.ToString() + "/" + s_date.Year.ToString()))
                                {
                                    find_day.Add(s_date.Month.ToString() + "/" + s_date.Day.ToString() + "/" + s_date.Year.ToString(), day_find);
                                }
                                s_date = s_date.AddDays(1);
                            }
                            //added by srinath 19/9/2013
                            string sectionvalue = string.Empty;
                            string sec = string.Empty;
                            if (ddlsec.Items.Count > 0)
                            {
                                if (ddlsec.SelectedValue.ToString().Trim() == "" && ddlsec.SelectedValue.ToString() != null && ddlsec.SelectedValue.ToString().Trim().ToLower() != "all")
                                {
                                    sectionvalue = string.Empty;
                                }
                                else
                                {
                                    sectionvalue = "and r.Sections='" + ddlsec.SelectedItem.ToString().Trim() + "'";
                                    sec="  and Sections='" + ddlsec.SelectedItem.ToString().Trim() + "'";
                                }
                            }
                            //string subjectlab = "select distinct subject_no,lab from subjectChooser s,Registration r,sub_sem se where r.roll_no=s.roll_no and s.semester=r.Current_Semester and s.subtype_no=se.subType_no and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.Current_Semester='" + ddlduration.SelectedValue.ToString() + "'" + sectionvalue + " and lab=1";  //     existing
                            string subjectlab = "select distinct subject_no,lab from subjectChooser s,Registration r,sub_sem se where r.roll_no=s.roll_no  and s.subtype_no=se.subType_no and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' " + sectionvalue + "and s.semester='" + ddlduration.SelectedValue.ToString().Trim() + "' and lab=1";  //     modified by prabha on feb 08 2018



                            DataSet dslab = da.select_method(subjectlab, hat, "Text");
                            Hashtable hatlab = new Hashtable();
                            if (dslab.Tables.Count > 0 && dslab.Tables[0].Rows.Count != 0)
                            {
                                for (int la = 0; la < dslab.Tables[0].Rows.Count; la++)
                                {
                                    if (!hatlab.Contains(dslab.Tables[0].Rows[la]["subject_no"].ToString()))
                                    {
                                        hatlab.Add(dslab.Tables[0].Rows[la]["subject_no"].ToString(), "1");
                                    }
                                }
                            }
                            if (ds.Tables.Count > 18 && ds.Tables[18].Rows.Count != 0)
                            {
                                for (int k = 0; k < ds.Tables[18].Rows.Count; k++)
                                {
                                    if (!subject.ContainsKey(ds.Tables[18].Rows[k][0].ToString() + "-" + ds.Tables[18].Rows[k][1].ToString()))//Added By Srinath 14/8/2013
                                    {
                                        if (hatlab.Contains(ds.Tables[18].Rows[k][1].ToString()))
                                        {
                                            subject.Add(ds.Tables[18].Rows[k][0].ToString().Trim().ToLower() + "-" + ds.Tables[18].Rows[k][1].ToString().Trim().ToLower(), ds.Tables[18].Rows[k][2].ToString());
                                        }
                                        else
                                        {
                                            subject.Add(ds.Tables[18].Rows[k][0].ToString().Trim().ToLower() + "-" + ds.Tables[18].Rows[k][1].ToString().Trim().ToLower(), "");
                                        }
                                    }
                                }
                            }
                            if (ds.Tables.Count > 19 && ds.Tables[19].Rows.Count != 0)
                            {
                                for (int k = 0; k < ds.Tables[19].Rows.Count; k++)
                                {
                                    get_date_holiday = ds.Tables[19].Rows[k][0].ToString();
                                    string[] split_date = ds.Tables[19].Rows[k][0].ToString().Split('/');
                                    get_date_holiday = ((split_date[1].ToString())).ToString() + "/" + ((split_date[0].ToString())).ToString() + "/" + ((split_date[2].ToString())).ToString();
                                    if (ds.Tables[19].Rows[k]["halforfull"].ToString() == "False")
                                    {
                                        halforfull = "0";
                                    }
                                    else
                                    {
                                        halforfull = "1";
                                    }
                                    if (ds.Tables[19].Rows[k]["morning"].ToString() == "False")
                                    {
                                        mng = "0";
                                    }
                                    else
                                    {
                                        mng = "1";
                                    }
                                    if (ds.Tables[19].Rows[k]["evening"].ToString() == "False")
                                    {
                                        evng = "0";
                                    }
                                    else
                                    {
                                        evng = "1";
                                    }
                                    holiday_sched_details = halforfull + "*" + mng + "*" + evng;
                                    if (!holiday.ContainsKey(get_date_holiday))
                                    {
                                        holiday.Add(get_date_holiday, holiday_sched_details);
                                    }
                                }
                            }
                            if (ds.Tables.Count > 20 && ds.Tables[20].Rows.Count > 0)
                            {
                                for (int k = 0; k < ds.Tables[20].Rows.Count; k++)
                                {
                                    if (!dayvalue.ContainsKey(ds.Tables[20].Rows[k][0].ToString().ToLower() + "-" + ds.Tables[20].Rows[k][2].ToString() + "-" + ds.Tables[20].Rows[k][3].ToString() + "-" + ds.Tables[20].Rows[k][4].ToString() + "-" + ds.Tables[20].Rows[k][1].ToString()))
                                    {
                                        string value = ds.Tables[20].Rows[k][0].ToString().ToLower() + "-" + ds.Tables[20].Rows[k][2].ToString() + "-" + ds.Tables[20].Rows[k][3].ToString() + "-" + ds.Tables[20].Rows[k][4].ToString();
                                        dayvalue.Add(ds.Tables[20].Rows[k][0].ToString().ToLower() + "-" + ds.Tables[20].Rows[k][2].ToString() + "-" + ds.Tables[20].Rows[k][3].ToString() + "-" + ds.Tables[20].Rows[k][4].ToString() + "-" + ds.Tables[20].Rows[k][1].ToString(), ds.Tables[20].Rows[k][1].ToString());
                                    }
                                }
                            }
                            DateTime Admission_date;
                            int count1 = 0;
                            int arow = 0;
                            while (roll_data.Tables.Count > 0 && rollcount < roll_data.Tables[0].Rows.Count)
                            // while (rollcount < ds.Tables[6].Rows.Count)
                            {

                                tval = 0;
                                Array.Clear(array_subject_hour_count, 0, array_subject_hour_count.Length);
                                Array.Clear(array_subject, 0, array_subject.Length);
                                Array.Clear(array_value, 0, array_value.Length);
                                Array.Clear(array_attnd, 0, array_attnd.Length);
                                //  Array.Clear(array_attnd_individualsubject_hour_count, 0, array_attnd_individualsubject_hour_count.Length);
                                Array.Clear(array_individualsubject_hour_count, 0, array_individualsubject_hour_count.Length);
                                total_conducted_hrs = 0;
                                total_attended_hrs = 0;
                                whole_total_conducted_hrs = 0;

                                drow = data.NewRow();
                                data.Rows.Add(drow);
                                for (int c = 0; c < colcount; c++)
                                {
                                    data.Rows[data.Rows.Count - 1][c] = (Convert.ToInt16(arow) + 1).ToString();
                                    if (Convert.ToString(Session["Rollflag"]) == "1")
                                    {
                                        c++;
                                        data.Rows[data.Rows.Count - 1][c] = roll_data.Tables[0].Rows[rollcount][0].ToString();


                                    }
                                    else
                                    {
                                        c++;
                                        data.Rows[data.Rows.Count - 1][c] = roll_data.Tables[0].Rows[rollcount][0].ToString();


                                    }
                                    if (Convert.ToString(Session["Regflag"]) == "1")
                                    {
                                        c++;
                                        data.Rows[data.Rows.Count - 1][c] = roll_data.Tables[0].Rows[rollcount][2].ToString();

                                    }

                                    c++;
                                    data.Rows[data.Rows.Count - 1][c] = roll_data.Tables[0].Rows[rollcount][3].ToString();
                                    string del = roll_data.Tables[0].Rows[rollcount][7].ToString();
                                    string examf = roll_data.Tables[0].Rows[rollcount][8].ToString();
                                    if (del == "1" || examf.ToUpper() == "DEBAR")
                                    {
                                        dicdiscon.Add(data.Rows.Count - 1, roll_data.Tables[0].Rows[rollcount][0].ToString());
                                    }

                                }
                                string admdate = roll_data.Tables[0].Rows[rollcount]["adm_date"].ToString();
                                string[] admdatesp = admdate.Split(new Char[] { '/' });
                                admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
                                Admission_date = Convert.ToDateTime(admdate);
                                Dictionary<string, int> labbatchhour = new Dictionary<string, int>();//Added by srinath 11/9/2014
                                t_date = to_date;
                                f_date = from_date;
                                split_date_time1 = f_date.ToString().Split(' ');
                                dummy_split = split_date_time1[0].Split('/');
                                tot_mnth = (Convert.ToInt32(dummy_split[0].ToString()) + (Convert.ToInt32(dummy_split[2].ToString()) * 12));
                                split_date_time1 = t_date.ToString().Split(' ');
                                dummy_split = split_date_time1[0].Split('/');
                                tempfdate = (Convert.ToInt32(dummy_split[0].ToString()) + (Convert.ToInt32(dummy_split[2].ToString()) * 12));
                                f_month_year = tot_mnth;
                            label: if (roll_no.Contains(roll_data.Tables[0].Rows[rollcount][1].ToString() + "-" + tot_mnth.ToString()))
                                //   label: if (roll_no.Contains(ds.Tables[6].Rows[rollcount][1].ToString() + "-" + tot_mnth.ToString()))                    
                                {
                                    //rollcolumncount = roll_no.IndexOf(ds.Tables[6].Rows[rollcount][1].ToString() + "-" + tot_mnth.ToString());
                                    rollcolumncount = roll_no.IndexOf(roll_data.Tables[0].Rows[rollcount][1].ToString() + "-" + tot_mnth.ToString());
                                    if (tot_mnth == f_month_year && tot_mnth == tempfdate)
                                    {
                                        t_date = to_date;
                                    }
                                    else if (tot_mnth == tempfdate)
                                    {
                                        t_date = to_date;
                                    }
                                    else if (tot_mnth == f_month_year)
                                    {
                                        t_date = f_date.AddMonths(1);
                                        t_date = t_date.AddDays(-Convert.ToInt16(f_date.Day.ToString()));
                                    }
                                    else
                                    {
                                        t_date = f_date.AddMonths(1);
                                    }
                                while_loop:
                                    while (f_date <= t_date)
                                    {
                                        if (!hatdc.Contains(f_date))
                                        {
                                            if (f_date >= Admission_date)
                                            {
                                                if (!holiday.ContainsKey(f_date.ToString("dd") + "/" + f_date.ToString("MM") + "/" + f_date.ToString("yyyy")))
                                                {
                                                    holiday.Add(f_date.ToString("dd") + "/" + f_date.ToString("MM") + "/" + f_date.ToString("yyyy"), "3*0*0");
                                                }
                                                //========================
                                                value_holi_status = GetCorrespondingKey(f_date.ToString("dd") + "/" + f_date.ToString("MM") + "/" + f_date.ToString("yyyy"), holiday).ToString();
                                                split_holiday_status = value_holi_status.Split('*');
                                                if (split_holiday_status[0].ToString() == "3")//=========ful day working day
                                                {
                                                    split_holiday_status_1 = 1;
                                                    split_holiday_status_2 = no_of_hrs;
                                                }
                                                else if (split_holiday_status[0].ToString() == "1")//=============half day working day
                                                {
                                                    if (split_holiday_status[1].ToString() == "1")//==============mng holiday//evng working day
                                                    {
                                                        split_holiday_status_1 = first_half + 1;
                                                        split_holiday_status_2 = no_of_hrs;
                                                    }
                                                    if (split_holiday_status[2].ToString() == "1")//==============evng holiday//mng working day
                                                    {
                                                        split_holiday_status_1 = 1;
                                                        split_holiday_status_2 = first_half;
                                                    }
                                                }
                                                else if (split_holiday_status[0].ToString() == "0")
                                                {
                                                    f_date = f_date.AddDays(1);
                                                    //split_holiday_status_1 = "0";
                                                    //split_holiday_status_2 = "0";
                                                    goto while_loop;
                                                }
                                                //=========================
                                                sume = (f_date).ToString("M/d/yyyy");
                                                if (strorder == 1)
                                                {
                                                    srt_day = f_date.ToString("ddd");
                                                }
                                                else
                                                {
                                                    srt_day = find_day[sume];
                                                }
                                                d = string.Empty;
                                                d1 = string.Empty;
                                                row = 0;
                                                table = 0;
                                                if (alternate.Contains(sume))
                                                {
                                                    row = alternate.IndexOf(sume);
                                                    table = 12;
                                                }
                                                else
                                                {
                                                    row = 0;
                                                    table = 13;
                                                }


                                                for (int sdate = split_holiday_status_1; sdate <= split_holiday_status_2; sdate++)
                                                {
                                                    d = srt_day + sdate.ToString();
                                                    d1 = "d" + f_date.Day.ToString() + "d" + sdate.ToString();
                                                    bool altflag = true;
                                                    //if (ds.Tables[table].Rows[row][d].ToString() != "" || ds.Tables[table].Rows[row][d].ToString() == null)
                                                    //{
                                                    //    s_code = ds.Tables[table].Rows[row][d].ToString().Split(';');
                                                    //    altflag = false;
                                                    //}
                                                    //else
                                                    //{
                                                    //    s_code = ds.Tables[13].Rows[0][d].ToString().Split(';');
                                                    //}
                                                    //Rajkumar on 9-11-2018

                                                    bool isAlterNew = false;
                                                    string semsch = string.Empty;
                                                    if (ds.Tables[table].Rows[row][d].ToString() != "" || ds.Tables[table].Rows[row][d].ToString() == null)
                                                    {
                                                        string[] s_codeSem;
                                                        ds.Tables[13].DefaultView.RowFilter = "FromDate1<='" + f_date.ToString("MM/dd/yyyy") + "'";
                                                        DataView dvsemstersem = ds.Tables[13].DefaultView;
                                                        dvsemstersem.Sort = "FromDate1 Desc";
                                                        if (dvsemstersem.Count > 0)
                                                        {
                                                            s_codeSem = dvsemstersem[0][d].ToString().Split(';');
                                                        }
                                                        else
                                                        {
                                                            s_codeSem = ds.Tables[13].Rows[0][d].ToString().Split(';');
                                                        }
                                                        
                                                        //Rajkumar  10-
                                                        string strAltDet = "select * from AlternateDetails where AlternateDate='" + f_date.ToString("MM/dd/yyyy") + "' and AlterHour='" + sdate + "' and Degree_code='" + ddlbranch.SelectedValue.ToString() + "' and Semester='" + ddlduration.SelectedValue.ToString().Trim() + "' and batch_Year='" + ddlbatch.SelectedValue.ToString() + "' " + sec + " order by Noalter desc";
                                                        DataTable dtAlt = dir.selectDataTable(strAltDet);

                                                        if (dtAlt.Rows.Count > 0)
                                                        {
                                                            for (int a = 0; a < s_codeSem.Length; a++)
                                                            {
                                                                string sno = Convert.ToString(s_codeSem[a]).Split('-')[0];
                                                                dtAlt.DefaultView.RowFilter = "ActSubNo='" + sno + "'";
                                                                DataView dvaltsub = dtAlt.DefaultView;
                                                                if (dvaltsub.Count > 0)
                                                                {
                                                                    isAlterNew = true;
                                                                    ds.Tables[table].DefaultView.RowFilter = "FromDate1<='" + f_date.ToString("MM/dd/yyyy") + "'";
                                                                    DataView dvsemster = ds.Tables[table].DefaultView;
                                                                    dvsemster.Sort = "FromDate1 Desc";

                                                                    if (dvsemster.Count > 0)
                                                                    {
                                                                        s_code = dvsemster[0][d].ToString().Split(';');
                                                                        altflag = false;
                                                                        //if (semsch.Contains(Convert.ToString(dvsemster[0][d])))
                                                                        //{
                                                                            if (string.IsNullOrEmpty(semsch))
                                                                                semsch = Convert.ToString(dvsemster[0][d]);
                                                                            else
                                                                                semsch = semsch + ";" + Convert.ToString(dvsemster[0][d]);
                                                                        //}
                                                                    }
                                                                    else
                                                                    {
                                                                        s_code = ds.Tables[table].Rows[row][d].ToString().Split(';');
                                                                        altflag = false;
                                                                        //if (semsch.Contains(Convert.ToString(dvsemster[0][d])))
                                                                        //{
                                                                            if (string.IsNullOrEmpty(semsch))
                                                                                semsch = Convert.ToString(ds.Tables[table].Rows[row][d]);
                                                                            else
                                                                                semsch = semsch + ";" + Convert.ToString(ds.Tables[table].Rows[row][d]);
                                                                        //}
                                                                    }


                                                                }
                                                                else
                                                                {
                                                                    //s_code = Convert.ToString(s_codeSem[a]).Split(';');

                                                                    if (string.IsNullOrEmpty(semsch))
                                                                        semsch = Convert.ToString(s_codeSem[a]);
                                                                    else
                                                                        semsch = semsch + ";" + Convert.ToString(s_codeSem[a]);
                                                                }
                                                            }

                                                        }
                                                        else
                                                        {

                                                            ds.Tables[table].DefaultView.RowFilter = "FromDate1<='" + f_date.ToString("MM/dd/yyyy") + "'";
                                                            DataView dvsemster = ds.Tables[table].DefaultView;
                                                            dvsemster.Sort = "FromDate1 Desc";
                                                            if (dvsemster.Count > 0)
                                                            {
                                                                s_code = dvsemster[0][d].ToString().Split(';');
                                                                altflag = false;
                                                            }
                                                            else
                                                            {
                                                                s_code = ds.Tables[table].Rows[row][d].ToString().Split(';');
                                                                altflag = false;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ds.Tables[13].DefaultView.RowFilter = "FromDate1<='" + f_date.ToString("MM/dd/yyyy") + "'";
                                                        DataView dvsemster = ds.Tables[13].DefaultView;
                                                        dvsemster.Sort = "FromDate1 Desc";
                                                        if (dvsemster.Count > 0)
                                                        {
                                                            s_code = dvsemster[0][d].ToString().Split(';');

                                                        }
                                                        else
                                                        {
                                                            s_code = ds.Tables[13].Rows[0][d].ToString().Split(';');
                                                        }
                                                    }
                                                    if (s_code.Length == 0 || s_code.Length == 1)
                                                    {
                                                        string notValid = "Empty";
                                                    }
                                                    if (!string.IsNullOrEmpty(semsch))
                                                        s_code = semsch.Split(';');

                                                    if (s_code.GetUpperBound(0) >= 0)
                                                    {
                                                        for (upper_bnd = 0; upper_bnd <= s_code.GetUpperBound(0); upper_bnd++)
                                                        {
                                                            dummy_split = s_code[upper_bnd].ToString().Split('-');
                                                            if (subject.ContainsKey(ds.Tables[11].Rows[rollcolumncount]["roll_no"].ToString().Trim().ToLower() + "-" + dummy_split[0].ToString().Trim().ToLower()))
                                                            // if (subject.Contains(ds.Tables[11].Rows[rollcolumncount]["roll_no"].ToString() + "-" + dummy_split[0].ToString()))
                                                            {
                                                                da1 = subject[ds.Tables[11].Rows[rollcolumncount]["roll_no"].ToString().Trim().ToLower() + "-" + dummy_split[0].ToString().Trim().ToLower()];
                                                                if (!diclabsub.ContainsKey(dummy_split[0].ToString()))
                                                                {
                                                                    if (da1 == "")
                                                                    {
                                                                        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                                                                        {
                                                                            for (k = 0; k < ds.Tables[1].Rows.Count; k++)
                                                                            {
                                                                                if (s_code[upper_bnd].Contains(ds.Tables[1].Rows[k][1].ToString()))
                                                                                {
                                                                                    //array_subject_hour_count[k] = array_subject_hour_count[k] + 1;
                                                                                    array_value[k] = array_value[k] + 1;
                                                                                    //Added by srinath 3/9/2013
                                                                                    string[] spitdatevalue = f_date.ToString().Split(' ');
                                                                                    string[] spitmointh = spitdatevalue[0].Split('/');
                                                                                    int year = Convert.ToInt32(spitmointh[2]);
                                                                                    int month = Convert.ToInt32(spitmointh[0]);
                                                                                    int monthvalue = (12 * year) + month;
                                                                                    dssem.Tables[3].DefaultView.RowFilter = "month_year='" + monthvalue + "' and roll_no='" + roll_data.Tables[0].Rows[rollcount][1].ToString() + "'";
                                                                                    DataView dvatteva = dssem.Tables[3].DefaultView;
                                                                                    String attendancequery = string.Empty;
                                                                                    if (dvatteva.Count > 0)
                                                                                    {
                                                                                        attendancequery = dvatteva[0][d1].ToString();
                                                                                    }
                                                                                    // if (present_table.Contains(ds.Tables[11].Rows[rollcolumncount][d1].ToString()))//Modified by srinath 3/9/2013
                                                                                    if (present_table.Contains(attendancequery))
                                                                                    {
                                                                                        sume = (f_date).ToString("M/d/yyyy");
                                                                                        h = string.Empty;
                                                                                        if (attend_table.ContainsKey(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                                                                        {
                                                                                            h = attend_table[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()];
                                                                                            attend_table[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()] = (Convert.ToInt16(h) + 1).ToString();
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            attend_table.Add(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString(), "1");
                                                                                        }
                                                                                        array_attnd[k] = array_attnd[k] + 1;
                                                                                        k = ds.Tables[1].Rows.Count;
                                                                                        upper_bnd = s_code.GetUpperBound(0);
                                                                                    }
                                                                                    //else if ((ds.Tables[11].Rows[rollcolumncount][d1].ToString() == "") || (ds.Tables[11].Rows[rollcolumncount][d1].ToString() == null) || (ds.Tables[11].Rows[rollcolumncount][d1].ToString() == "0") || (ds.Tables[11].Rows[rollcolumncount][d1].ToString() == "8"))//Mpdified by srinath 3/9/2013
                                                                                    else if (attendancequery == "" || attendancequery == null || attendancequery == "0" || attendancequery == "8")
                                                                                    {
                                                                                        array_value[k] = array_value[k] - 1;
                                                                                        k = ds.Tables[1].Rows.Count;
                                                                                        upper_bnd = s_code.GetUpperBound(0);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        sume = (f_date).ToString("M/d/yyyy");
                                                                                        h = string.Empty;
                                                                                        if (attend_table.ContainsKey(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                                                                        {
                                                                                            h = attend_table[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()];
                                                                                            attend_table[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()] = (Convert.ToInt16(h) + 1).ToString();
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            attend_table.Add(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString(), "1");
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    string ttname = string.Empty;
                                                                    dssem.Tables[4].DefaultView.RowFilter = "batch_year='" + ddlbatch.Text.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester='" + ddlduration.SelectedItem.ToString() + "' " + getsec + " and FromDate<='" + f_date.ToString("MM/dd/yyyy") + "'";
                                                                    DataView dvsemster = dssem.Tables[4].DefaultView;
                                                                    if (dvsemster.Count > 0)
                                                                    {
                                                                        ttname = dvsemster[0]["ttname"].ToString();
                                                                    }
                                                                    bool isCalculate = false;
                                                                    LoadAutoSwitchLab(Convert.ToString(ddlbatch.SelectedItem.Text).Trim(), Convert.ToString(ddlbranch.SelectedValue).Trim(), Convert.ToString(ddlduration.SelectedItem.Text).Trim(), strsec, ttname, ref dicAutoSwitchLab);


                                                                    //added By Srinath 9/9/2014=======Start======================================


                                                                    if (altflag == false && !isCalculate || (isAlterNew))// && !isCalculate
                                                                    {
                                                                        string[] spval = s_code[upper_bnd].ToString().Split('-');
                                                                        //if (isAlterNew && !string.IsNullOrEmpty(semsch))
                                                                        //    spval = semsch[upper_bnd].ToString().Split('-');

                                                                        if (spval.GetUpperBound(0) > 0)
                                                                        {
                                                                            string altsubno = spval[0].ToString();
                                                                            if (altsubno.Trim() != "" && altsubno != null)
                                                                            {
                                                                                dssem.Tables[2].DefaultView.RowFilter = " roll_no='" + ds.Tables[11].Rows[rollcolumncount]["roll_no"].ToString() + "' and subject_no='" + altsubno + "'  and Hour_Value='" + sdate + "' and fromdate='" + f_date.ToString("MM/dd/yyyy") + "'";
                                                                                DataView dvstubatch = dssem.Tables[2].DefaultView;
                                                                                if (dvstubatch.Count > 0)
                                                                                {
                                                                                    isCalculate = true;
                                                                                    string batch = dvstubatch[0]["Batch"].ToString();
                                                                                    if (batch.Trim() != "" && batch != null)
                                                                                    {
                                                                                        string batchsubject = da1 + '-' + altsubno;
                                                                                        if (!labbatchhour.ContainsKey(batchsubject))
                                                                                        {
                                                                                            labbatchhour.Add(batchsubject, 1);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            int adbatchsubject = labbatchhour[batchsubject];
                                                                                            adbatchsubject = adbatchsubject + 1;
                                                                                            labbatchhour[batchsubject] = adbatchsubject;
                                                                                        }
                                                                                        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                                                                                        {
                                                                                            for (k = 0; k < ds.Tables[1].Rows.Count; k++)
                                                                                            {
                                                                                                if (s_code[upper_bnd].Contains(ds.Tables[1].Rows[k][1].ToString()))
                                                                                                {
                                                                                                    tval = tval + 1;
                                                                                                    array_value[k] = array_value[k] + 1;
                                                                                                    string[] spitdatevalue = f_date.ToString().Split(' ');
                                                                                                    string[] spitmointh = spitdatevalue[0].Split('/');
                                                                                                    int year = Convert.ToInt32(spitmointh[2]);
                                                                                                    int month = Convert.ToInt32(spitmointh[0]);
                                                                                                    int monthvalue = (12 * year) + month;
                                                                                                    dssem.Tables[3].DefaultView.RowFilter = "month_year='" + monthvalue + "' and roll_no='" + roll_data.Tables[0].Rows[rollcount][1].ToString() + "'";
                                                                                                    DataView dvatteva = dssem.Tables[3].DefaultView;
                                                                                                    String attendancequery = string.Empty;
                                                                                                    if (dvatteva.Count > 0)
                                                                                                    {
                                                                                                        attendancequery = dvatteva[0][d1].ToString();
                                                                                                    }
                                                                                                    //if (present_table.Contains(ds.Tables[11].Rows[rollcolumncount][d1].ToString()))
                                                                                                    //if (!dicAutoSwitchLab.ContainsKey(altsubno.Trim()))
                                                                                                    //{
                                                                                                    //    dicAutoSwitchLab.Add(altsubno.Trim(), 0);
                                                                                                    //}

                                                                                                    if (present_table.Contains(attendancequery))
                                                                                                    {
                                                                                                        sume = (f_date).ToString("M/d/yyyy");
                                                                                                        h = string.Empty;
                                                                                                        if (lab.ContainsKey(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                                                                                        {
                                                                                                            h = lab[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()];
                                                                                                            lab[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()] = (Convert.ToInt16(h) + 1).ToString();
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            lab.Add(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString(), "1");
                                                                                                        }
                                                                                                        array_attnd[k] = array_attnd[k] + 1;
                                                                                                        k = ds.Tables[1].Rows.Count;
                                                                                                        upper_bnd = s_code.GetUpperBound(0);
                                                                                                    }
                                                                                                    //else if ((ds.Tables[11].Rows[rollcolumncount][d1].ToString() == "") || (ds.Tables[11].Rows[rollcolumncount][d1].ToString() == null) || (ds.Tables[11].Rows[rollcolumncount][d1].ToString() == "0") || (ds.Tables[11].Rows[rollcolumncount][d1].ToString() == "8"))
                                                                                                    else if (attendancequery == "" || attendancequery == null || attendancequery == "0" || attendancequery == "8")
                                                                                                    {
                                                                                                        array_value[k] = array_value[k] - 1;
                                                                                                        k = ds.Tables[1].Rows.Count;
                                                                                                        upper_bnd = s_code.GetUpperBound(0);
                                                                                                        if (!labbatchhour.ContainsKey(batchsubject))
                                                                                                        {
                                                                                                            labbatchhour.Add(batchsubject, 0);
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            int adbatchsubject = labbatchhour[batchsubject];
                                                                                                            adbatchsubject = adbatchsubject - 1;
                                                                                                            labbatchhour[batchsubject] = adbatchsubject;
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        sume = (f_date).ToString("M/d/yyyy");
                                                                                                        h = string.Empty;
                                                                                                        if (lab.ContainsKey(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                                                                                        {
                                                                                                            h = lab[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()];
                                                                                                            lab[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()] = (Convert.ToInt16(h) + 1).ToString();
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            lab.Add(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString(), "1");
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
                                                                    if (dayvalue.ContainsKey(srt_day.ToString().ToLower() + "-" + dummy_split[0].ToString() + "-" + sdate.ToString() + "-" + ttname.ToString() + "-" + da1))
                                                                    {
                                                                        davalue = dayvalue[srt_day.ToString().ToLower() + "-" + dummy_split[0].ToString() + "-" + sdate.ToString() + "-" + ttname.ToString() + "-" + da1];
                                                                        if (altflag == false && !isCalculate || altflag == true || isAlterNew)//|| altflag == true
                                                                        {
                                                                            if (da1 == davalue)
                                                                            {
                                                                                //if (!dicAutoSwitchLab.ContainsKey(srt_day.ToString().ToLower() + dummy_split[0].ToString()))
                                                                                //{
                                                                                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                                                                                {
                                                                                    for (k = 0; k < ds.Tables[1].Rows.Count; k++)
                                                                                    {
                                                                                        if (s_code[upper_bnd].Contains(ds.Tables[1].Rows[k][1].ToString()))
                                                                                        {
                                                                                            // array_subject_hour_count[k] = array_subject_hour_count[k] + 1;
                                                                                            if (s_code[upper_bnd].Contains("1257"))
                                                                                            {
                                                                                                string subnak = s_code[upper_bnd].ToString();
                                                                                            }
                                                                                            tval = tval + 1;
                                                                                            array_value[k] = array_value[k] + 1;
                                                                                            string[] spval = s_code[upper_bnd].ToString().Split('-');
                                                                                            if (spval.GetUpperBound(0) > 0)
                                                                                            {
                                                                                                string altsubno = spval[0].ToString();
                                                                                                string batchsubject = da1 + '-' + altsubno;
                                                                                                if (!labbatchhour.ContainsKey(batchsubject))
                                                                                                {
                                                                                                    labbatchhour.Add(batchsubject, 1);
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    int adbatchsubject = labbatchhour[batchsubject];
                                                                                                    adbatchsubject = adbatchsubject + 1;
                                                                                                    labbatchhour[batchsubject] = adbatchsubject;
                                                                                                }
                                                                                            }
                                                                                            string[] spitdatevalue = f_date.ToString().Split(' ');
                                                                                            string[] spitmointh = spitdatevalue[0].Split('/');
                                                                                            int year = Convert.ToInt32(spitmointh[2]);
                                                                                            int month = Convert.ToInt32(spitmointh[0]);
                                                                                            int monthvalue = (12 * year) + month;
                                                                                            dssem.Tables[3].DefaultView.RowFilter = "month_year='" + monthvalue + "' and roll_no='" + roll_data.Tables[0].Rows[rollcount][1].ToString() + "'";
                                                                                            DataView dvatteva = dssem.Tables[3].DefaultView;
                                                                                            String attendancequery = string.Empty;
                                                                                            if (dvatteva.Count > 0)
                                                                                            {
                                                                                                attendancequery = dvatteva[0][d1].ToString();
                                                                                            }
                                                                                            isCalculate = true;
                                                                                            //Modified By Srinath 11/9/2014
                                                                                            //if (present_table.Contains(ds.Tables[11].Rows[rollcolumncount][d1].ToString()))//Mpdified by srinath 3/9/2013
                                                                                            //    if (present_table.Contains(ds.Tables[11].Rows[rollcolumncount][d1].ToString()))
                                                                                            if (present_table.Contains(attendancequery))
                                                                                            {
                                                                                                sume = (f_date).ToString("M/d/yyyy");
                                                                                                h = string.Empty;
                                                                                                if (lab.ContainsKey(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                                                                                {
                                                                                                    h = lab[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()];
                                                                                                    lab[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()] = (Convert.ToInt16(h) + 1).ToString();
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    lab.Add(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString(), "1");
                                                                                                }
                                                                                                array_attnd[k] = array_attnd[k] + 1;
                                                                                                k = ds.Tables[1].Rows.Count;
                                                                                                upper_bnd = s_code.GetUpperBound(0);
                                                                                            }
                                                                                            else if (attendancequery == "" || attendancequery == null || attendancequery == "0" || attendancequery == "8")
                                                                                            {
                                                                                                array_value[k] = array_value[k] - 1;
                                                                                                k = ds.Tables[1].Rows.Count;
                                                                                                upper_bnd = s_code.GetUpperBound(0);
                                                                                                string altsubno = spval[0].ToString();
                                                                                                string batchsubject = da1 + '-' + altsubno;
                                                                                                if (!labbatchhour.ContainsKey(batchsubject))
                                                                                                {
                                                                                                    labbatchhour.Add(batchsubject, 0);
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    int adbatchsubject = labbatchhour[batchsubject];
                                                                                                    adbatchsubject = adbatchsubject - 1;
                                                                                                    labbatchhour[batchsubject] = adbatchsubject;
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                sume = (f_date).ToString("M/d/yyyy");
                                                                                                h = string.Empty;
                                                                                                if (lab.ContainsKey(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                                                                                {
                                                                                                    h = lab[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()];
                                                                                                    lab[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()] = (Convert.ToInt16(h) + 1).ToString();
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    lab.Add(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString(), "1");
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                //}
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                //================================End========================================
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }//Added by Srinath For Day Order Change===========
                                        f_date = f_date.AddDays(1);
                                    }
                                    if (tot_mnth <= tempfdate)
                                    {
                                        tot_mnth = tot_mnth + 1;
                                        if (tot_mnth > tempfdate)
                                        {
                                            goto label1;
                                        }
                                        goto label;
                                    }
                                }
                                else
                                {
                                    f_date = f_date.AddMonths(1);
                                    f_date = f_date.AddDays(-Convert.ToInt16(f_date.Day.ToString()));
                                    tot_mnth = tot_mnth + 1;
                                    if (tot_mnth > tempfdate)
                                    {
                                        goto label1;
                                    }
                                    goto label;
                                }
                            label1: row = 5;
                                int x = 0;

                                //double tempPer = 0;
                                bool boolCheck = false;
                                //double.TryParse(attnd_perc, out tempPer);
                                //if (tempPer > checkPercentage)
                                //{

                                //}
                                //for (int i = 0; i < length; i++)
                                //{
                                //    bool booTemp = find_filter(attnd_perc);
                                //    if (!boolCheck && booTemp)
                                //        boolCheck = booTemp;
                                //}
                                //if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                                //{
                                //    for (i = 5; i < (ds.Tables[1].Rows.Count * 2) + 5; i = i + 2)
                                //    {



                                if (data.Columns.Count > 0)
                                {
                                    int j = 0;
                                    for (int g = colcount; g < data.Columns.Count - 3; g = g + 2)
                                    {
                                        string conhrname = data.Columns[g].ColumnName;
                                        string pername = data.Columns[g + 1].ColumnName;
                                        //   whole_total_conducted_hrs = ((whole_total_conducted_hrs) + Convert.ToInt16(array_subject_hour_count[row-4].ToString()));
                                        total_conducted_hrs = ((total_conducted_hrs) + Convert.ToInt16(array_value[row - 5].ToString()));
                                        total_attended_hrs = ((total_attended_hrs) + Convert.ToInt16(array_attnd[row - 5].ToString()));

                                        //------------------------------spl hrs 16/6/12(PRABHA)
                                        subject_num_spl = ds.Tables[1].Rows[x][1].ToString();
                                        //ds.Tables[1].Rows[k][1]
                                        spl_total_conducted_hrs = 0;
                                        spl_total_attended_hrs = 0;
                                        x++;
                                        for (DateTime temp_date = from_date; temp_date <= to_date; temp_date = temp_date.AddDays(1))
                                        {
                                            spl_total_conducted_hrs = spl_total_conducted_hrs + Convert.ToInt32(GetCorrespondingKey(roll_data.Tables[0].Rows[rollcount][1].ToString() + "$" + temp_date + "$" + subject_num_spl, hasspl_tot));
                                            spl_total_attended_hrs = spl_total_attended_hrs + Convert.ToInt32(GetCorrespondingKey(roll_data.Tables[0].Rows[rollcount][1].ToString() + "$" + temp_date + "$" + subject_num_spl, hasspl_pres));
                                        }
                                        total_conducted_hrs = total_conducted_hrs + spl_total_conducted_hrs;
                                        total_attended_hrs = total_attended_hrs + spl_total_attended_hrs;
                                        //--------------------------------------------------------
                                        if (array_value[row - 5] == 0 && spl_total_attended_hrs == 0)
                                        {
                                            data.Rows[data.Rows.Count - 1][g] = "-";
                                            data.Rows[data.Rows.Count - 1][g + 1] = "-";


                                        }
                                        else if (array_attnd[row - 5] == 0 && spl_total_conducted_hrs == 0)
                                        {
                                            data.Rows[data.Rows.Count - 1][g] = "0";
                                            data.Rows[data.Rows.Count - 1][g + 1] = "0";

                                        }
                                        else
                                        {
                                            attnd_perc_val = (((Convert.ToDouble(array_attnd[row - 5]) + spl_total_attended_hrs) / (Convert.ToDouble(array_value[row - 5]) + spl_total_conducted_hrs)) * 100);
                                            avgstudent1 = Convert.ToDecimal(attnd_perc_val);
                                            avgstudent2 = Math.Round(avgstudent1);
                                            avgstudent3 = Convert.ToDouble(avgstudent2);
                                            attnd_perc = Convert.ToString(avgstudent3);

                                            data.Rows[data.Rows.Count - 1][g] = (Convert.ToInt32(array_attnd[row - 5]) + spl_total_attended_hrs).ToString();
                                            data.Rows[data.Rows.Count - 1][g + 1] = attnd_perc.ToString();



                                            //rajkumar

                                            bool booTemp = find_filter(Convert.ToString(avgstudent2));
                                            if (!boolCheck && booTemp)
                                            {
                                                boolCheck = booTemp;
                                            }
                                            //end
                                        }
                                        string rollNo = Convert.ToString(roll_data.Tables[0].Rows[rollcount][1]).Trim().ToLower();
                                        if (!dicAutoLabConductHour.ContainsKey(subject_num_spl.Trim()))
                                        {
                                            dicAutoLabConductHour.Add(subject_num_spl.Trim(), Convert.ToDouble(array_value[row - 5]) + spl_total_conducted_hrs);
                                        }
                                        else
                                        {
                                            if (dicAutoLabConductHour[subject_num_spl.Trim()] <= Convert.ToDouble(array_value[row - 5]) + spl_total_conducted_hrs)
                                            {
                                                dicAutoLabConductHour[subject_num_spl.Trim()] = Convert.ToDouble(array_value[row - 5]) + spl_total_conducted_hrs;
                                            }
                                        }
                                        //====================================SRINATH 11/9/2014
                                        // string subnote = subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.SheetCorner.RowCount - 2), i].Note;
                                        string subnote = ds.Tables[1].Rows[j][1].ToString();
                                        foreach (var kvp in labbatchhour)
                                        {
                                            string setval = kvp.Key.ToString();
                                            int setvalva = Convert.ToInt32(kvp.Value.ToString());
                                            string[] getsubno = setval.Split('-');
                                            if (getsubno[1].ToString() == subnote)
                                            {
                                                if (!totlabbatchconhr.ContainsKey(subnote))
                                                {
                                                    totlabbatchconhr.Add(subnote, setvalva);
                                                }
                                                else
                                                {
                                                    int getsub = totlabbatchconhr[subnote];
                                                    if (getsub < setvalva)
                                                    {
                                                        totlabbatchconhr[subnote] = setvalva;
                                                    }
                                                }
                                            }
                                        }
                                        j++;
                                        //totlabbatchconhr
                                        //=======================================
                                        row++;
                                    }
                                }

                                if (total_conducted_hrs == 0)
                                {
                                    attnd_perc = "-";
                                }
                                else if (total_attended_hrs == 0)
                                {
                                    attnd_perc = "0";
                                }
                                else
                                {
                                    if (ds.Tables.Count > 7 && ds.Tables[7].Rows.Count != 0)
                                    {
                                        if (ds.Tables[7].Rows[0][0].ToString() == "1")
                                        {
                                            if (ds.Tables.Count > 8 && ds.Tables[8].Rows.Count != 0)
                                            {
                                                if (Convert.ToInt16(total_attended_hrs) >= Convert.ToInt16(ds.Tables[8].Rows[0][0].ToString()))
                                                {
                                                    attnd_perc = "100";
                                                }
                                                else
                                                {
                                                    attnd_perc_val = (Convert.ToDouble(total_attended_hrs) / Convert.ToDouble(total_conducted_hrs)) * 100;
                                                    //-------convert
                                                    avgstudent1 = Convert.ToDecimal(attnd_perc_val);
                                                    avgstudent2 = Math.Round(avgstudent1);
                                                    avgstudent3 = Convert.ToDouble(avgstudent2);
                                                    attnd_perc = Convert.ToString(avgstudent3);
                                                }
                                            }
                                            else
                                            {
                                                attnd_perc_val = (Convert.ToDouble(total_attended_hrs) / Convert.ToDouble(total_conducted_hrs)) * 100;
                                                //-------convert
                                                avgstudent1 = Convert.ToDecimal(attnd_perc_val);
                                                avgstudent2 = Math.Round(avgstudent1);
                                                avgstudent3 = Convert.ToDouble(avgstudent2);
                                                attnd_perc = Convert.ToString(avgstudent3);
                                            }
                                        }
                                        else
                                        {
                                            attnd_perc_val = (Convert.ToDouble(total_attended_hrs) / Convert.ToDouble(total_conducted_hrs)) * 100;
                                            //-------convert
                                            avgstudent1 = Convert.ToDecimal(attnd_perc_val);
                                            avgstudent2 = Math.Round(avgstudent1);
                                            avgstudent3 = Convert.ToDouble(avgstudent2);
                                            attnd_perc = Convert.ToString(avgstudent3);
                                        }
                                    }
                                    else
                                    {
                                        attnd_perc_val = (Convert.ToDouble(total_hrs) / Convert.ToDouble(total_conducted_hrs)) * 100;
                                        //-------convert
                                        avgstudent1 = Convert.ToDecimal(attnd_perc_val);
                                        avgstudent2 = Math.Round(avgstudent1);
                                        avgstudent3 = Convert.ToDouble(avgstudent2);
                                        attnd_perc = Convert.ToString(avgstudent3);
                                    }
                                }
                                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                                {
                                    i = (ds.Tables[1].Rows.Count * 2) + 5;
                                }
                                if (total_conducted_hrs == 0)
                                {

                                    data.Rows[data.Rows.Count - 1][data.Columns.Count - 3] = "-";

                                }
                                else
                                {
                                    data.Rows[data.Rows.Count - 1][data.Columns.Count - 3] = total_attended_hrs.ToString();

                                }

                                data.Rows[data.Rows.Count - 1][data.Columns.Count - 2] = attnd_perc.ToString();

                                data.Rows[data.Rows.Count - 1][data.Columns.Count - 1] = "";


                                if (percheck.Checked == true)
                                {
                                    if (pertxt.Text != "")
                                    {
                                        //find_filter(attnd_perc);
                                        //if (filter_flag == false)
                                        if (!boolCheck)
                                        {

                                            dicdiscon.Remove(data.Rows.Count - 1);
                                            data.Rows.RemoveAt(data.Rows.Count - 1);
                                            // rollcount--;
                                            arow--;
                                            //subject_report.Sheets[0].RowCount = subject_report.Sheets[0].RowCount - 1;
                                        }
                                        else
                                        {
                                            sturollcount++;
                                            //data.Rows[data.Rows.Count - 1][data.Columns.Count - 1] = sturollcount.ToString();


                                        }
                                        //find_filter(attnd_perc);
                                        //if (!boolCheck)
                                        //{
                                        //    subject_report.Sheets[0].RowCount = subject_report.Sheets[0].RowCount - 1;
                                        //}
                                        //else
                                        //{
                                        //    sturollcount++;
                                        //    subject_report.Sheets[0].Cells[row_cnt, 0].Text = sturollcount.ToString();
                                        //}
                                    }

                                }


                                rollcount++;
                                arow++;

                            }



                            f_date = from_date;
                            int count = 0;
                            tval = 0;
                            while (f_date <= t_date)
                            {
                                sume = f_date.Month.ToString() + "/" + f_date.Day.ToString() + "/" + f_date.Year.ToString();
                                for (int sdate = split_holiday_status_1; sdate <= split_holiday_status_2; sdate++)
                                {
                                    count = 0;
                                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                                    {
                                        for (k = 0; k < ds.Tables[1].Rows.Count; k++)
                                        {
                                            if (attend_table.ContainsKey(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                            {
                                                count++;
                                                array_attnd_individualsubject_hour_count[k] = array_attnd_individualsubject_hour_count[k] + 1;

                                                //k = ds.Tables[1].Rows.Count;//magesh 28.9.18
                                            }
                                        }
                                    }
                                    if (count == 0)
                                    {
                                        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                                        {
                                            for (k = 0; k < ds.Tables[1].Rows.Count; k++)
                                            {
                                                //if (lab.ContainsKey(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                                //{
                                                //    count++;
                                                //    array_lab_hour_count[k] = array_lab_hour_count[k] + 1;
                                                //}
                                                if (totlabbatchconhr.ContainsKey(ds.Tables[1].Rows[k][1].ToString()))
                                                {
                                                    array_lab_hour_count[k] = totlabbatchconhr[ds.Tables[1].Rows[k][1].ToString()];
                                                }
                                            }
                                        }
                                        if (count > 0)
                                        {
                                            tval = tval + 1;
                                        }
                                    }
                                }
                                f_date = f_date.AddDays(1);
                            }
                            row = 5;
                            int spl_total_attended_hrs_tot = 0;
                            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                            {
                                for (i = 0; i < (ds.Tables[1].Rows.Count); i++)
                                {
                                    //------------------------29/6/12 PRABHA
                                    subject_num_spl = ds.Tables[1].Rows[x][1].ToString();
                                    spl_total_conducted_hrs = 0;
                                    spl_total_attended_hrs = 0;
                                    int maxspl_total_attended_hrs_tot = 0;
                                    int maxhr = 0;
                                    int varaiab = 0;
                                    Hashtable hsadd = new Hashtable();
                                    x++;
                                    if (roll_data.Tables.Count > 0 && roll_data.Tables[0].Rows.Count > 0)
                                    {
                                        for (int a = 0; a < roll_data.Tables[0].Rows.Count; a++)
                                        {
                                            spl_total_conducted_hrs = 0;

                                            for (DateTime temp_date = from_date; temp_date <= to_date; temp_date = temp_date.AddDays(1))
                                            {
                                                spl_total_conducted_hrs = 0;
                                                int hrs = 0;
                                                spl_total_conducted_hrs = spl_total_conducted_hrs + Convert.ToInt32(GetCorrespondingKey(roll_data.Tables[0].Rows[a][1].ToString() + "$" + temp_date + "$" + subject_num_spl, hasspl_tot));
                                                spl_total_attended_hrs_tot = spl_total_attended_hrs_tot + spl_total_conducted_hrs;// = spl_total_attended_hrs + Convert.ToInt32(GetCorrespondingKey(roll_data.Tables[0].Rows[rollcount][1].ToString() + "$" + temp_date + "$" + subject_num_spl, hasspl_pres));
                                                //if (spl_total_conducted_hrs > 1)
                                                //{
                                                //    if (hrs < maxhr)
                                                //        hrs = spl_total_conducted_hrs;
                                                //}
                                                //else if (spl_total_conducted_hrs == 1)
                                                //{
                                                //    if (hrs < maxhr)
                                                //    {
                                                if (spl_total_conducted_hrs == 1)
                                                {
                                                    if (!hsadd.ContainsKey(temp_date))
                                                    {
                                                        hsadd.Add(temp_date, spl_total_conducted_hrs);
                                                        if (spl_total_conducted_hrs > 1)
                                                            varaiab++;

                                                    }
                                                }
                                                //        hrs = 1;
                                                //    }



                                                //}
                                                //maxhr = hrs;
                                            }
                                            //if (spl_total_conducted_hrs > maxspl_total_attended_hrs_tot)
                                            //    maxspl_total_attended_hrs_tot = spl_total_conducted_hrs;
                                        }
                                        //maxspl_total_attended_hrs_tot = maxhr;
                                        maxspl_total_attended_hrs_tot = hsadd.Count;
                                    }
                                    //---------------------------------------
                                    bool setlabatchflag = false;
                                    string subcode = ds.Tables[1].Rows[i][0].ToString();
                                    if (chklabsubbatch.Checked == true)
                                    {
                                        string subval = ds.Tables[1].Rows[i][1].ToString();
                                        foreach (var kvp in totlabbatchconhr)
                                        {
                                            string getval = kvp.Key.ToString();
                                            int setvalva = Convert.ToInt32(kvp.Value.ToString());
                                            if (getval == subval)
                                            {
                                                setlabatchflag = true;
                                                dicsubhead.Add(subcode, setvalva.ToString());
                                                // subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].ColumnHeader.RowCount - 1), i].Text = setvalva.ToString();
                                            }
                                        }
                                    }
                                    if (setlabatchflag == false)
                                    {
                                        //subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].ColumnHeader.RowCount - 1), i].Text = (array_attnd_individualsubject_hour_count[row - 5] + array_lab_hour_count[row - 5] + spl_total_conducted_hrs).ToString();
                                        dicsubhead.Add(subcode, (array_attnd_individualsubject_hour_count[row - 5] + array_lab_hour_count[row - 5] + maxspl_total_attended_hrs_tot).ToString());

                                    }
                                    whole_total_conducted_hrs = whole_total_conducted_hrs + (array_attnd_individualsubject_hour_count[row - 5]);
                                    row++;
                                }
                                i = (ds.Tables[1].Rows.Count * 2) + 5;
                            }
                            if (whole_total_conducted_hrs == 0)
                            {
                                // subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].ColumnHeader.RowCount - 1), i].Text = "-";
                            }
                            else
                            {
                                //subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].ColumnHeader.RowCount - 1), i].Text = (whole_total_conducted_hrs + tval + spl_total_attended_hrs_tot).ToString();
                            }
                        }
                    }
                }

            }
            else
            {
                int arow = 0;
                int[] array_subject = new int[data.Columns.Count - 7];
                int[] array_value = new int[data.Columns.Count - 7];
                int[] array_attnd = new int[data.Columns.Count - 7];
                int[] array_subject_hour_count = new int[data.Columns.Count - 7];
                int[] array_individualsubject_hour_count = new int[data.Columns.Count - 7];
                int[] array_attnd_individualsubject_hour_count = new int[data.Columns.Count - 7];
                int[] array_lab_hour_count = new int[data.Columns.Count - 7];
                Array.Clear(array_subject_hour_count, 0, array_subject_hour_count.Length);
                Array.Clear(array_subject, 0, array_subject.Length);
                Array.Clear(array_value, 0, array_value.Length);
                Array.Clear(array_attnd, 0, array_attnd.Length);
                Array.Clear(array_individualsubject_hour_count, 0, array_individualsubject_hour_count.Length);
                Array.Clear(array_attnd_individualsubject_hour_count, 0, array_attnd_individualsubject_hour_count.Length);
                Array.Clear(array_lab_hour_count, 0, array_lab_hour_count.Length);


                List<string> roll_no = new List<string>();
                List<string> direct = new List<string>();
                List<string> present_table = new List<string>();
                //  List<string> criteria_roll = new List<string>();
                //  List<string> holiday = new List<string>();
                Dictionary<string, string> dayvalue = new Dictionary<string, string>();
                //Dictionary<string, string> null_table = new Dictionary<string, string>();
                Dictionary<string, string> subject = new Dictionary<string, string>();
                Dictionary<string, string> attend_table = new Dictionary<string, string>();
                Dictionary<string, string> lab = new Dictionary<string, string>();
                i = 0;
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    for (i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        array_subject[i] = Convert.ToInt16(ds.Tables[1].Rows[i][1].ToString());
                    }
                    subject_count = ds.Tables[1].Rows.Count;
                }
                if (ds.Tables.Count > 11 && ds.Tables[11].Rows.Count > 0)
                {
                    //   if ((roll_data.Tables[0].Rows.Count != 0) &&  (ds.Tables[14].Rows.Count != 0))
                    if (ds.Tables.Count > 14 && ds.Tables[14].Rows.Count > 0)
                    {
                        if (ds.Tables.Count > 14 && ds.Tables[14].Rows.Count > 0)
                        {
                            for (int k = 0; k < ds.Tables[14].Rows.Count; k++)
                            {
                                if (!direct.Contains(ds.Tables[14].Rows[k]["month_year"].ToString()))
                                {
                                    direct.Add(ds.Tables[14].Rows[k]["month_year"].ToString());
                                }
                            }
                        }
                        if (ds.Tables.Count > 11 && ds.Tables[11].Rows.Count > 0)
                        {
                            for (int k = 0; k < ds.Tables[11].Rows.Count; k++)
                            {
                                if (!roll_no.Contains(ds.Tables[11].Rows[k][0].ToString() + "-" + ds.Tables[11].Rows[k][5].ToString()))
                                {
                                    roll_no.Add(ds.Tables[11].Rows[k][0].ToString() + "-" + ds.Tables[11].Rows[k][5].ToString());
                                }
                            }
                        }
                        if (ds.Tables.Count > 9 && ds.Tables[9].Rows.Count > 0)
                        {
                            for (int k = 0; k < ds.Tables[9].Rows.Count; k++)
                            {
                                if (!present_table.Contains(ds.Tables[9].Rows[k][1].ToString()))
                                {
                                    present_table.Add(ds.Tables[9].Rows[k][1].ToString());
                                }
                            }
                        }
                        if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                        {
                            noofhrs = ds.Tables[2].Rows[0][0].ToString();
                            no_of_hrs = Convert.ToInt16(noofhrs.ToString());
                        }
                        if (ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
                        {
                            noofday = ds.Tables[3].Rows[0][0].ToString();
                            no_of_days = Convert.ToInt16(noofday.ToString());
                        }
                        if (ds.Tables.Count > 5 && ds.Tables[5].Rows.Count > 0)
                        {
                            str_order = ds.Tables[3].Rows[0][0].ToString();
                            strorder = Convert.ToInt16(str_order.ToString());
                        }
                        if (ds.Tables.Count > 4 && ds.Tables[4].Rows.Count > 0)
                        {
                            start_date = ds.Tables[4].Rows[0][0].ToString();
                            s_date = Convert.ToDateTime(start_date);
                        }
                        if (ds.Tables.Count > 18 && ds.Tables[18].Rows.Count > 0)
                        {
                            for (int k = 0; k < ds.Tables[18].Rows.Count; k++)
                            {
                                if (!subject.ContainsKey(ds.Tables[18].Rows[k][0].ToString() + "-" + ds.Tables[18].Rows[k][1].ToString()))
                                {
                                    subject.Add(ds.Tables[18].Rows[k][0].ToString() + "-" + ds.Tables[18].Rows[k][1].ToString(), ds.Tables[18].Rows[k][2].ToString());
                                }
                            }
                        }
                        if (ds.Tables.Count > 19 && ds.Tables[19].Rows.Count > 0)
                        {
                            for (int k = 0; k < ds.Tables[19].Rows.Count; k++)
                            {
                                get_date_holiday = ds.Tables[19].Rows[k][0].ToString();
                                string[] split_date = ds.Tables[19].Rows[k][0].ToString().Split('/');
                                get_date_holiday = ((split_date[1].ToString())).ToString() + "/" + ((split_date[0].ToString())).ToString() + "/" + ((split_date[2].ToString())).ToString();
                                if (ds.Tables[19].Rows[k]["halforfull"].ToString() == "False")
                                {
                                    halforfull = "0";
                                }
                                else
                                {
                                    halforfull = "1";
                                }
                                if (ds.Tables[19].Rows[k]["morning"].ToString() == "False")
                                {
                                    mng = "0";
                                }
                                else
                                {
                                    mng = "1";
                                }
                                if (ds.Tables[19].Rows[k]["evening"].ToString() == "False")
                                {
                                    evng = "0";
                                }
                                else
                                {
                                    evng = "1";
                                }
                                holiday_sched_details = halforfull + "*" + mng + "*" + evng;
                                if (!holiday.ContainsKey(get_date_holiday))
                                {
                                    holiday.Add(get_date_holiday, holiday_sched_details);
                                }
                            }
                        }
                        if (ds.Tables.Count > 20 && ds.Tables[20].Rows.Count > 0)
                        {
                            for (int k = 0; k < ds.Tables[20].Rows.Count; k++)
                            {
                                if (!dayvalue.ContainsKey(ds.Tables[20].Rows[k][0].ToString() + "-" + ds.Tables[20].Rows[k][2].ToString() + "-" + ds.Tables[20].Rows[k][3].ToString()))
                                {
                                    dayvalue.Add(ds.Tables[20].Rows[k][0].ToString() + "-" + ds.Tables[20].Rows[k][2].ToString() + "-" + ds.Tables[20].Rows[k][3].ToString(), ds.Tables[20].Rows[k][1].ToString());
                                }
                            }
                        }
                        while (roll_data.Tables.Count > 0 && rollcount < roll_data.Tables[0].Rows.Count)
                        // while (rollcount < ds.Tables[6].Rows.Count)
                        {
                            tval = 0;
                            Array.Clear(array_subject_hour_count, 0, array_subject_hour_count.Length);
                            Array.Clear(array_subject, 0, array_subject.Length);
                            Array.Clear(array_value, 0, array_value.Length);
                            Array.Clear(array_attnd, 0, array_attnd.Length);
                            Array.Clear(array_individualsubject_hour_count, 0, array_individualsubject_hour_count.Length);
                            //   Array.Clear(array_attnd_individualsubject_hour_count, 0, array_attnd_individualsubject_hour_count.Length);
                            total_conducted_hrs = 0;
                            total_attended_hrs = 0;
                            whole_total_conducted_hrs = 0;
                            drow = data.NewRow();
                            data.Rows.Add(drow);
                            for (int c = 0; c < colcount; c++)
                            {
                                data.Rows[data.Rows.Count - 1][c] = (Convert.ToInt16(arow) + 1).ToString();
                                if (Convert.ToString(Session["Rollflag"]) == "1")
                                {
                                    c++;
                                    data.Rows[data.Rows.Count - 1][c] = roll_data.Tables[0].Rows[rollcount][0].ToString();


                                }
                                else
                                {
                                    c++;
                                    data.Rows[data.Rows.Count - 1][c] = roll_data.Tables[0].Rows[rollcount][0].ToString();
                                }
                                if (Convert.ToString(Session["Regflag"]) == "1")
                                {
                                    c++;
                                    data.Rows[data.Rows.Count - 1][c] = roll_data.Tables[0].Rows[rollcount][2].ToString();

                                }

                                c++;
                                data.Rows[data.Rows.Count - 1][c] = roll_data.Tables[0].Rows[rollcount][3].ToString();

                                string del = roll_data.Tables[0].Rows[rollcount][7].ToString();
                                string examf = roll_data.Tables[0].Rows[rollcount][8].ToString();
                                if (del == "1" || examf.ToUpper() == "DEBAR")
                                {
                                    dicdiscon.Add(data.Rows.Count - 1, roll_data.Tables[0].Rows[rollcount][0].ToString());
                                }
                            }

                            t_date = to_date;
                            f_date = from_date;
                            split_date_time1 = f_date.ToString().Split(' ');
                            dummy_split = split_date_time1[0].Split('/');
                            tot_mnth = (Convert.ToInt32(dummy_split[0].ToString()) + (Convert.ToInt32(dummy_split[2].ToString()) * 12));
                            split_date_time1 = t_date.ToString().Split(' ');
                            dummy_split = split_date_time1[0].Split('/');
                            tempfdate = (Convert.ToInt32(dummy_split[0].ToString()) + (Convert.ToInt32(dummy_split[2].ToString()) * 12));
                            f_month_year = tot_mnth;
                        label: if (roll_no.Contains(roll_data.Tables[0].Rows[rollcount][1].ToString() + "-" + tot_mnth.ToString()))
                            //   label: if (roll_no.Contains(ds.Tables[6].Rows[rollcount][1].ToString() + "-" + tot_mnth.ToString()))                    
                            {
                                //rollcolumncount = roll_no.IndexOf(ds.Tables[6].Rows[rollcount][1].ToString() + "-" + tot_mnth.ToString());
                                rollcolumncount = roll_no.IndexOf(roll_data.Tables[0].Rows[0][1].ToString() + "-" + tot_mnth.ToString());
                                if (tot_mnth == f_month_year && tot_mnth == tempfdate)
                                {
                                    t_date = to_date;
                                }
                                else if (tot_mnth == tempfdate)
                                {
                                    t_date = to_date;
                                }
                                else
                                {
                                    t_date = f_date.AddMonths(1);
                                    t_date = t_date.AddDays(-1);
                                }
                            while_loop1:
                                while (f_date <= t_date)
                                {
                                    if (!holiday.ContainsKey(f_date.ToString("dd") + "/" + f_date.ToString("MM") + "/" + f_date.ToString("yyyy")))
                                    {
                                        holiday.Add(f_date.ToString("dd") + "/" + f_date.ToString("MM") + "/" + f_date.ToString("yyyy"), "3*0*0");
                                    }
                                    //========================
                                    value_holi_status = GetCorrespondingKey(f_date.ToString("dd") + "/" + f_date.ToString("MM") + "/" + f_date.ToString("yyyy"), holiday).ToString();
                                    split_holiday_status = value_holi_status.Split('*');
                                    if (split_holiday_status[0].ToString() == "3")//=========ful day working day
                                    {
                                        split_holiday_status_1 = 1;
                                        split_holiday_status_2 = no_of_hrs;
                                    }
                                    else if (split_holiday_status[0].ToString() == "1")//=============half day working day
                                    {
                                        if (split_holiday_status[1].ToString() == "1")//==============mng holiday//evng working day
                                        {
                                            split_holiday_status_1 = first_half + 1;
                                            split_holiday_status_2 = no_of_hrs;
                                        }
                                        if (split_holiday_status[2].ToString() == "1")//==============evng holiday//mng working day
                                        {
                                            split_holiday_status_1 = 1;
                                            split_holiday_status_2 = first_half;
                                        }
                                    }
                                    else if (split_holiday_status[0].ToString() == "0")
                                    {
                                        f_date = f_date.AddDays(1);
                                        //split_holiday_status_1 = "0";
                                        //split_holiday_status_2 = "0";
                                        goto while_loop1;
                                    }
                                    //=========================
                                    row = 0;
                                    table = 0;
                                    if (direct.Contains(tot_mnth.ToString()))
                                    {
                                        row = direct.IndexOf(tot_mnth.ToString());
                                        table = 14;
                                        d1 = string.Empty;
                                    }
                                    else
                                    {
                                        goto lkl;
                                    }
                                    for (int sdate = split_holiday_status_1; sdate <= split_holiday_status_2; sdate++)
                                    {
                                        d1 = "d" + f_date.Day.ToString() + "d" + sdate.ToString();
                                        s_code = ds.Tables[table].Rows[row][d].ToString().Split(';');
                                        if (s_code.GetUpperBound(0) >= 0)
                                        {
                                            for (upper_bnd = 0; upper_bnd <= s_code.GetUpperBound(0); upper_bnd++)
                                            {
                                                dummy_split = s_code[upper_bnd].ToString().Split('-');
                                                if (subject.ContainsKey(ds.Tables[11].Rows[rollcolumncount]["roll_no"].ToString() + "-" + dummy_split[0].ToString()))
                                                // if (subject.Contains(ds.Tables[11].Rows[rollcolumncount]["roll_no"].ToString() + "-" + dummy_split[0].ToString()))
                                                {
                                                    da1 = subject[ds.Tables[11].Rows[rollcolumncount]["roll_no"].ToString() + "-" + dummy_split[0].ToString()];
                                                    if (da1 == "")
                                                    {
                                                        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                                                        {
                                                            for (k = 0; k < ds.Tables[1].Rows.Count; k++)
                                                            {
                                                                if (s_code[upper_bnd].Contains(ds.Tables[1].Rows[k][1].ToString()))
                                                                {
                                                                    sume = (f_date).ToString("M/d/yyyy");
                                                                    h = string.Empty;
                                                                    if (attend_table.ContainsKey(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                                                    {
                                                                        h = attend_table[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()];
                                                                        attend_table[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()] = (Convert.ToInt16(h) + 1).ToString();
                                                                    }
                                                                    else
                                                                    {
                                                                        attend_table.Add(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString(), "1");
                                                                    }
                                                                    //array_subject_hour_count[k] = array_subject_hour_count[k] + 1;
                                                                    array_value[k] = array_value[k] + 1;
                                                                    if (present_table.Contains(ds.Tables[11].Rows[rollcolumncount][d1].ToString()))
                                                                    {
                                                                        array_attnd[k] = array_attnd[k] + 1;
                                                                        k = ds.Tables[1].Rows.Count;
                                                                        upper_bnd = s_code.GetUpperBound(0);
                                                                    }
                                                                    else if ((ds.Tables[11].Rows[rollcolumncount][d1].ToString() == "") || (ds.Tables[11].Rows[rollcolumncount][d1].ToString() == null) || (ds.Tables[11].Rows[rollcolumncount][d1].ToString() == "0") || (ds.Tables[11].Rows[rollcolumncount][d1].ToString() == "8"))
                                                                    {
                                                                        //sume = (f_date).ToString("MM/dd/yyyy");
                                                                        //if (holiday.Contains(sume))
                                                                        //{
                                                                        //    array_value[k] = array_value[k] - 1;
                                                                        //    k = ds.Tables[1].Rows.Count;
                                                                        //    upper_bnd = s_code.GetUpperBound(0);
                                                                        //    //    array_subject_hour_count[k] = array_subject_hour_count[k] - 1;
                                                                        //}
                                                                        //else
                                                                        //{                                                                  
                                                                        //    array_value[k] = array_value[k] - 1;
                                                                        //    k = ds.Tables[1].Rows.Count;
                                                                        //    upper_bnd = s_code.GetUpperBound(0);
                                                                        //}
                                                                        array_value[k] = array_value[k] - 1;
                                                                        k = ds.Tables[1].Rows.Count;
                                                                        upper_bnd = s_code.GetUpperBound(0);
                                                                    }
                                                                    else
                                                                    {
                                                                        sume = (f_date).ToString("M/d/yyyy");
                                                                        h = string.Empty;
                                                                        if (attend_table.ContainsKey(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                                                        {
                                                                            h = attend_table[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()];
                                                                            attend_table[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()] = (Convert.ToInt16(h) + 1).ToString();
                                                                        }
                                                                        else
                                                                        {
                                                                            attend_table.Add(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString(), "1");
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    if (dayvalue.ContainsKey(srt_day + "-" + dummy_split[0].ToString() + "-" + sdate.ToString()))
                                                    {
                                                        davalue = dayvalue[srt_day + "-" + dummy_split[0].ToString() + "-" + sdate.ToString()];
                                                        if (da1 == davalue)
                                                        {
                                                            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                                                            {
                                                                for (k = 0; k < ds.Tables[1].Rows.Count; k++)
                                                                {
                                                                    if (s_code[upper_bnd].Contains(ds.Tables[1].Rows[k][1].ToString()))
                                                                    {
                                                                        // array_subject_hour_count[k] = array_subject_hour_count[k] + 1;
                                                                        //   tval = tval + 1;
                                                                        array_value[k] = array_value[k] + 1;
                                                                        if (present_table.Contains(ds.Tables[11].Rows[rollcolumncount][d1].ToString()))
                                                                        {
                                                                            sume = (f_date).ToString("M/d/yyyy");
                                                                            h = string.Empty;
                                                                            if (lab.ContainsKey(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                                                            {
                                                                                h = lab[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()];
                                                                                lab[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()] = (Convert.ToInt16(h) + 1).ToString();
                                                                            }
                                                                            else
                                                                            {
                                                                                lab.Add(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString(), "1");
                                                                            }
                                                                            array_attnd[k] = array_attnd[k] + 1;
                                                                            k = ds.Tables[1].Rows.Count;
                                                                            upper_bnd = s_code.GetUpperBound(0);
                                                                        }
                                                                        else if ((ds.Tables[11].Rows[rollcolumncount][d1].ToString() == "") || (ds.Tables[11].Rows[rollcolumncount][d1].ToString() == null) || (ds.Tables[11].Rows[rollcolumncount][d1].ToString() == "0") || (ds.Tables[11].Rows[rollcolumncount][d1].ToString() == "8"))
                                                                        {
                                                                            //sume = (f_date).ToString("MM/dd/yyyy");
                                                                            //if (holiday.Contains(sume))
                                                                            //{
                                                                            //    array_value[k] = array_value[k] - 1;
                                                                            //    k = ds.Tables[1].Rows.Count;
                                                                            //    upper_bnd = s_code.GetUpperBound(0);
                                                                            //   // array_subject_hour_count[k] = array_subject_hour_count[k] - 1;
                                                                            //  //  tval = tval - 1;
                                                                            //}
                                                                            //else
                                                                            //{                                                                     
                                                                            //    array_value[k] = array_value[k] - 1;
                                                                            //    k = ds.Tables[1].Rows.Count;
                                                                            //    upper_bnd = s_code.GetUpperBound(0);
                                                                            //}
                                                                            array_value[k] = array_value[k] - 1;
                                                                            k = ds.Tables[1].Rows.Count;
                                                                            upper_bnd = s_code.GetUpperBound(0);
                                                                        }
                                                                        else
                                                                        {
                                                                            sume = (f_date).ToString("M/d/yyyy");
                                                                            h = string.Empty;
                                                                            if (lab.ContainsKey(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                                                            {
                                                                                h = lab[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()];
                                                                                lab[sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()] = (Convert.ToInt16(h) + 1).ToString();
                                                                            }
                                                                            else
                                                                            {
                                                                                lab.Add(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString(), "1");
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
                                lkl: f_date = f_date.AddDays(1);
                                }
                                if (tot_mnth <= tempfdate)
                                {
                                    tot_mnth = tot_mnth + 1;
                                    if (tot_mnth > tempfdate)
                                    {
                                        goto label1;
                                    }
                                    goto label;
                                }
                            }
                            else
                            {
                                f_date = f_date.AddMonths(1);
                                f_date = f_date.AddDays(-Convert.ToInt16(f_date.Day.ToString()));
                                tot_mnth = tot_mnth + 1;
                                if (tot_mnth > tempfdate)
                                {
                                    goto label1;
                                }
                                goto label;
                            }
                        label1: row = 5;
                            if (data.Columns.Count > 0)
                            {
                                for (int g = colcount; g < data.Columns.Count - 3; g = g + 2)
                                {
                                    total_conducted_hrs = ((total_conducted_hrs) + Convert.ToInt16(array_value[row - 5].ToString()));
                                    total_attended_hrs = ((total_attended_hrs) + Convert.ToInt16(array_attnd[row - 5].ToString()));
                                    if (array_value[row - 5] == 0)
                                    {
                                        data.Rows[data.Rows.Count - 1][g] = "-";
                                        data.Rows[data.Rows.Count - 1][g + 1] = "-";

                                    }
                                    else if (array_attnd[row - 5] == 0)
                                    {
                                        data.Rows[data.Rows.Count - 1][g] = "-";
                                        data.Rows[data.Rows.Count - 1][g + 1] = "-";
                                    }
                                    else
                                    {
                                        attnd_perc_val = ((Convert.ToDouble(array_attnd[row - 5]) / Convert.ToDouble(array_value[row - 5])) * 100);
                                        avgstudent1 = Convert.ToDecimal(attnd_perc_val);
                                        avgstudent2 = Math.Round(avgstudent1, 0, MidpointRounding.AwayFromZero);
                                        avgstudent3 = Convert.ToDouble(avgstudent2);
                                        attnd_perc = Convert.ToString(avgstudent3);

                                        data.Rows[data.Rows.Count - 1][g] = array_attnd[row - 5].ToString();
                                        data.Rows[data.Rows.Count - 1][g + 1] = attnd_perc.ToString();

                                    }
                                    row++;
                                }
                            }
                            if (total_conducted_hrs == 0)
                            {
                                attnd_perc = "-";
                            }
                            else if (total_attended_hrs == 0)
                            {
                                attnd_perc = "0";
                            }
                            else
                            {
                                if (ds.Tables.Count > 7 && ds.Tables[7].Rows.Count > 0)
                                {
                                    if (ds.Tables[7].Rows[0][0].ToString() == "1")
                                    {
                                        if (ds.Tables.Count > 8 && ds.Tables[8].Rows.Count > 0)
                                        {
                                            if (Convert.ToInt16(total_attended_hrs) >= Convert.ToInt16(ds.Tables[8].Rows[0][0].ToString()))
                                            {
                                                attnd_perc = "100";
                                            }
                                            else
                                            {
                                                attnd_perc_val = (Convert.ToDouble(total_attended_hrs) / Convert.ToDouble(total_conducted_hrs)) * 100;
                                                //-------convert
                                                avgstudent1 = Convert.ToDecimal(attnd_perc_val);
                                                avgstudent2 = Math.Round(avgstudent1);
                                                avgstudent3 = Convert.ToDouble(avgstudent2);
                                                attnd_perc = Convert.ToString(avgstudent3);
                                            }
                                        }
                                        else
                                        {
                                            attnd_perc_val = (Convert.ToDouble(total_attended_hrs) / Convert.ToDouble(total_conducted_hrs)) * 100;
                                            //-------convert
                                            avgstudent1 = Convert.ToDecimal(attnd_perc_val);
                                            avgstudent2 = Math.Round(avgstudent1);
                                            avgstudent3 = Convert.ToDouble(avgstudent2);
                                            attnd_perc = Convert.ToString(avgstudent3);
                                        }
                                    }
                                    else
                                    {
                                        attnd_perc_val = (Convert.ToDouble(total_attended_hrs) / Convert.ToDouble(total_conducted_hrs)) * 100;
                                        //-------convert
                                        avgstudent1 = Convert.ToDecimal(attnd_perc_val);
                                        avgstudent2 = Math.Round(avgstudent1);
                                        avgstudent3 = Convert.ToDouble(avgstudent2);
                                        attnd_perc = Convert.ToString(avgstudent3);
                                    }
                                }
                                else
                                {
                                    attnd_perc_val = (Convert.ToDouble(total_hrs) / Convert.ToDouble(total_conducted_hrs)) * 100;
                                    //-------convert
                                    avgstudent1 = Convert.ToDecimal(attnd_perc_val);
                                    avgstudent2 = Math.Round(avgstudent1);
                                    avgstudent3 = Convert.ToDouble(avgstudent2);
                                    attnd_perc = Convert.ToString(avgstudent3);
                                }
                            }
                            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                            {
                                i = (ds.Tables[1].Rows.Count * 2) + 5;
                            }
                            if (total_conducted_hrs == 0)
                            {
                                data.Rows[data.Rows.Count - 1][data.Columns.Count - 3] = "-";

                            }
                            else
                            {
                                data.Rows[data.Rows.Count - 1][data.Columns.Count - 3] = total_attended_hrs.ToString();

                            }
                            data.Rows[data.Rows.Count - 1][data.Columns.Count - 2] = attnd_perc.ToString();


                            //subject_report.Sheets[0].Cells[row_cnt, i + 1].Text = attnd_perc.ToString();
                            data.Rows[data.Rows.Count - 1][data.Columns.Count - 1] = "";
                            if (percheck.Checked == true)
                            {
                                if (pertxt.Text != "")
                                {
                                    find_filter(attnd_perc);
                                    if (filter_flag == false)
                                    {
                                        dicdiscon.Remove(data.Rows.Count - 1);
                                        data.Rows.RemoveAt(data.Rows.Count - 1);
                                        arow--;

                                        //subject_report.Sheets[0].RowCount = subject_report.Sheets[0].RowCount - 1;
                                    }
                                    else
                                    {
                                        sturollcount++;
                                        // data.Rows[data.Rows.Count - 1][data.Columns.Count - 1] = sturollcount.ToString();


                                    }
                                }
                            }

                            row = 5;
                            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                            {
                                for (i = 0; i < (ds.Tables[1].Rows.Count); i++)
                                {
                                    if (array_individualsubject_hour_count[row - 5] > 0)
                                    {
                                        array_attnd_individualsubject_hour_count[row - 5] = (array_attnd_individualsubject_hour_count[row - 5] + array_individualsubject_hour_count[row - 5]);
                                    }
                                    row++;
                                }
                            }
                            rollcount++;
                            arow++;

                        }
                        f_date = from_date;
                        int count = 0;
                        tval = 0;
                        while (f_date <= t_date)
                        {
                            sume = f_date.Month.ToString() + "/" + f_date.Day.ToString() + "/" + f_date.Year.ToString();
                            for (int sdate = 1; sdate <= no_of_hrs; sdate++)
                            {
                                count = 0;
                                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                                {
                                    for (k = 0; k < ds.Tables[1].Rows.Count; k++)
                                    {
                                        if (attend_table.ContainsKey(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                        {
                                            count++;
                                            array_attnd_individualsubject_hour_count[k] = array_attnd_individualsubject_hour_count[k] + 1;
                                            k = ds.Tables[1].Rows.Count;
                                        }
                                    }
                                }
                                if (count == 0)
                                {
                                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                                    {
                                        for (k = 0; k < ds.Tables[1].Rows.Count; k++)
                                        {
                                            if (lab.ContainsKey(sume + "-" + sdate.ToString() + "-" + ds.Tables[1].Rows[k][1].ToString()))
                                            {
                                                count++;
                                                array_lab_hour_count[k] = array_lab_hour_count[k] + 1;
                                            }
                                        }
                                    }
                                    if (count > 0)
                                    {
                                        tval = tval + 1;
                                    }
                                }
                            }
                            f_date = f_date.AddDays(1);
                        }
                        row = 5;
                        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                        {
                            for (i = 0; i < (ds.Tables[1].Rows.Count); i++)
                            {

                                string subcode = ds.Tables[1].Rows[i][0].ToString();
                                dicsubhead.Add(subcode, (array_attnd_individualsubject_hour_count[row - 5] + array_lab_hour_count[row - 5]).ToString());
                                //subject_report.Sheets[0].ColumnHeader.Cells[10, i].Text = (array_attnd_individualsubject_hour_count[row - 5] + array_lab_hour_count[row - 5]).ToString();
                                whole_total_conducted_hrs = whole_total_conducted_hrs + (array_attnd_individualsubject_hour_count[row - 5]);
                                row++;
                            }
                            i = (ds.Tables[1].Rows.Count * 2) + 5;
                        }
                        if (whole_total_conducted_hrs == 0)
                        {
                            //subject_report.Sheets[0].ColumnHeader.Cells[10, i].Text = "-";
                        }
                        else
                        {
                            //subject_report.Sheets[0].ColumnHeader.Cells[10, i].Text = (whole_total_conducted_hrs + tval).ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public bool find_filter(string attnd_perc_val)
    {
        string filt_val = string.Empty;
        filt_val = pertxt.Text;
        filter_flag = false;
        if (filt_val != "")
        {
            if (attnd_perc_val != "-")
            {
                if (perddl.SelectedValue.ToString() == "0")
                {
                    if (Convert.ToInt16(attnd_perc_val.ToString()) > Convert.ToInt16(filt_val.ToString()))
                    {
                        filter_flag = true;

                    }
                }
                if (perddl.SelectedValue.ToString() == "1")
                {
                    if (Convert.ToInt16(attnd_perc_val.ToString()) >= Convert.ToInt16(filt_val.ToString()))
                    {
                        filter_flag = true;
                    }
                }
                if (perddl.SelectedValue.ToString() == "2")
                {
                    if (Convert.ToInt16(attnd_perc_val.ToString()) < Convert.ToInt16(filt_val.ToString()))
                    {
                        filter_flag = true;
                    }
                }
                if (perddl.SelectedValue.ToString() == "3")
                {
                    if (Convert.ToInt16(attnd_perc_val.ToString()) <= Convert.ToInt16(filt_val.ToString()))
                    {
                        filter_flag = true;
                    }
                }
                if (perddl.SelectedValue.ToString() == "4")
                {
                    if (Convert.ToInt16(attnd_perc_val.ToString()) != Convert.ToInt16(filt_val.ToString()))
                    {
                        filter_flag = true;
                    }
                }
                if (perddl.SelectedValue.ToString() == "5")
                {
                    if (Convert.ToInt16(attnd_perc_val.ToString()) == Convert.ToInt16(filt_val.ToString()))
                    {
                        filter_flag = true;
                    }
                }
            }
        }
        return filter_flag;
    }
    //public void daily_entry(string roll_no, string sub_no, out string total_hrs_val, out string present_hrs, out bool joinflag, out string process_hr)
    //{
    //    DateTime datefrom = Convert.ToDateTime(Session["from_date_time"].ToString());
    //    str_from = datefrom.ToString();
    //    string[] from_split = str_from.Split(' ');
    //    if (from_split.GetUpperBound(0) >= 0)
    //    {
    //        string[] dummy_split1 = from_split[0].Split('/');
    //        month_year_from = Convert.ToInt16(dummy_split1[0].ToString()) + (Convert.ToInt16(dummy_split1[2].ToString()) * 12);
    //    }
    //    DateTime dateto = Convert.ToDateTime(Session["to_date_time"].ToString());
    //    str_to = dateto.ToString();
    //    string[] to_split = str_to.Split(' ');
    //    if (to_split.GetUpperBound(0) >= 0)
    //    {
    //        string[] dummy_split2 = to_split[0].Split('/');
    //        month_year_to = Convert.ToInt16(dummy_split2[0].ToString()) + (Convert.ToInt16(dummy_split2[2].ToString()) * 12);
    //    }
    //    binjoin = false;
    //    //-------------set section value
    //    if (ddlsec.SelectedValue.ToString() == "")
    //    {
    //        strsec =string.Empty;
    //    }
    //    else
    //    {
    //       strsec = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
    //       // strsec = ddlsec.SelectedValue.ToString();
    //    }
    //    //SqlDataReader daily_dr;
    //    string str =string.Empty;
    //    con2.Close();
    //    con2.Open();
    //    str = "select distinct dm.sch_date,dm.degree_code,dm.semester,dd.hr from dailyStaffEntry as dm,dailyentdet as dd where dm.lp_code=dd.lp_code and dd.subject_no=" + sub_no + " and dm.degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester='" + ddlduration.SelectedValue.ToString() + "' and dm.sch_date between '" + datefrom + "' and '" + dateto + "'" + strsec + " order by sch_date";
    //    SqlDataAdapter da = new SqlDataAdapter(str, con2);
    //    DataSet ds = new DataSet();
    //    da.Fill(ds);
    //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //    {
    //        // if (daily_dr.HasRows == true)
    //        {
    //            rec_cnt = ds.Tables[0].Rows.Count;
    //        }
    //    }
    //    SqlDataReader daily_dr2;
    //    con.Close();
    //    con.Open();
    //    cmd = new SqlCommand("select sdet.* from specialhr_master as smas ,specialhr_details as sdet where smas.hrentry_no = sdet.hrentry_no and [date] between '" + datefrom + "' and '" + dateto + "' and sdet.subject_no =" + sub_no + " and smas.degree_code  = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " ", con);
    //    daily_dr2 = cmd.ExecuteReader();
    //    daily_dr2.Read();
    //    if (rec_cnt == 0 && daily_dr2.HasRows == false)
    //    {
    //        total_hrs_val_val = "-";
    //        present_hrs_val = "-";
    //    }
    //    else
    //    {
    //        if (rec_cnt > 0)
    //        {
    //            SqlDataReader daily_dr3;
    //            con_man.Close();
    //            con_man.Open();
    //            cmd_man = new SqlCommand("select * from attendance where roll_no ='" + roll_no + "' and month_year between " + month_year_from + " and " + month_year_to + " ", con_man);
    //            daily_dr3 = cmd_man.ExecuteReader();
    //            while (daily_dr3.Read())
    //            {
    //                mon_year =string.Empty;
    //                if (daily_dr3.HasRows == true)
    //                {
    //                    for (inner_loop = 1; inner_loop <= rec_cnt; inner_loop++)
    //                    {
    //                        string sch_date_val =string.Empty;
    //                        int monyr_val = 0;
    //                        sch_date_val = ds.Tables[0].Rows[inner_loop - 1][0].ToString();
    //                        string[] split_sch = sch_date_val.Split(' ');
    //                        string[] split_sch2 = split_sch[0].Split('/');
    //                        monyr_val = Convert.ToInt16(split_sch2[0].ToString()) + (Convert.ToInt16(split_sch2[2].ToString()) * 12);
    //                        if (mon_year != "month_year=" + monyr_val)
    //                        {
    //                            mon_year = "month_year=" + monyr_val;
    //                            Cond = true;
    //                        }
    //                        if (Cond == true)
    //                        {
    //                            temp1 = "d" + split_sch2[1].ToString() + "d" + ds.Tables[0].Rows[inner_loop - 1]["hr"].ToString();
    //                            if (daily_dr3[temp1].ToString() != "")
    //                            {
    //                                textmark = Convert.ToInt16(daily_dr3[temp1].ToString());
    //                                if (textmark == 8)
    //                                {
    //                                    binjoin = true;
    //                                    ProcessNJHrs = ProcessNJHrs + 1;
    //                                    presentdays = presentdays + 1;
    //                                }
    //                                else
    //                                {
    //                                    presentdays = presentdays + 1;
    //                                    if (textmark.ToString() == "")
    //                                    {
    //                                        textmark = 0;
    //                                    }
    //                                    SqlDataReader calc_dr;
    //                                    readcon.Close();
    //                                    readcon.Open();
    //                                    cmd1 = new SqlCommand("select * from AttMasterSetting where LeaveCode=" + textmark + " and CollegeCode=" + Session["collegecode"] + " ", readcon);
    //                                    calc_dr = cmd1.ExecuteReader();
    //                                    calc_dr.Read();
    //                                    if (calc_dr.HasRows == true)
    //                                    {
    //                                        if (calc_dr["CalcFlag"].ToString() == "0")
    //                                        {
    //                                            TotalPres = TotalPres + 1;
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }//-----end
    //        }//------------daily end
    //        PresentSum = TotalPres;
    //        TotalPres = 0;
    //        TotalAbs = 0;
    //        int phrs = 0;
    //        int ahrs = 0;
    //        int chrs = 0;
    //        int mhrs = 0;
    //        if (daily_dr2.HasRows == true)
    //        {
    //            int sub_num = Convert.ToInt16(sub_no);
    //            nonofsplhours(roll_no, datefrom, dateto, sub_num, out phrs, out ahrs, out chrs, out mhrs);
    //            presentdays = presentdays + phrs;
    //            PresentSum = PresentSum + mhrs;
    //        }
    //    }
    //    total_hrs_val = presentdays.ToString();
    //    present_hrs = PresentSum.ToString();
    //    process_hr = ProcessNJHrs.ToString();
    //    if (binjoin == true)
    //    {
    //        joinflag = true;
    //    }
    //    else
    //    {
    //        joinflag = false;
    //    }
    //}
    public void loadsubject()
    {
        try
        {
            includediscon = " and delflag=0";
            includedebar = " and exam_flag <> 'DEBAR'";
            includedisco = " and r.delflag=0";
            includedeba = " and r.exam_flag <> 'DEBAR'";
            string getshedulockva = da.GetFunctionv("select value from Master_Settings where settings='Attendance Discount' " + grouporusercode1 + "");
            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
            {
                includediscon = string.Empty;
                includedisco = "";
            }
            getshedulockva = da.GetFunctionv("select value from Master_Settings where  settings='Attendance Debar' " + grouporusercode1 + " ");
            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
            {
                includedebar = string.Empty;
                includedeba = "";
            }

            Dictionary<string, string> diccode = new Dictionary<string, string>();
            int syllabus_year = 0;
            //----------------------setting
            setpanel.Visible = false;
            Showgrid.Visible = false;
            btnxl.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            btndirtPrint.Visible = false;
            //Added By Srinath 27/2/2013
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            errmsg.Visible = false;
            setpanel.Visible = false;

            dsprint.Clear();
            hat.Clear();
            hat.Add("college_code", Session["collegecode"].ToString());
            hat.Add("form_name", "subjwiseattndreport.aspx");
            dsprint = da.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
            //===========================================
            //======================0n 11/4/12 PRABHA
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                isonumber = dsprint.Tables[0].Rows[0]["ISOCode"].ToString();
                if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
                {
                    new_header_string_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString();
                    // subject_report.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorBottom = Color.White;
                    new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                    new_header_string_split = new_header_string.Split(',');
                    //subject_report.Sheets[0].SheetCorner.RowCount = subject_report.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
                }
            }
            //=====================================
            colcount = 0;


            arrColHdrNames1.Clear();
            arrColHdrNames2.Clear();
            arrColHdrNames1.Add("S.No");
            arrColHdrNames2.Add("S.No");

            data.Columns.Add("col0");
            if (Session["Rollflag"].ToString() == "1")
            {
                arrColHdrNames1.Add("Roll No");
                arrColHdrNames2.Add("Roll No");
                colcount++;
                data.Columns.Add("col" + colcount);

            }
            else
            {

                arrColHdrNames1.Add("Roll No");
                arrColHdrNames2.Add("Roll No");
                colcount++;
                data.Columns.Add("col" + colcount);
            }

            if (Session["Regflag"].ToString() == "1")
            {
                arrColHdrNames1.Add("Reg No");
                arrColHdrNames2.Add("Reg No");

                colcount++;
                data.Columns.Add("col" + colcount);
            }
            colcount++;
            arrColHdrNames1.Add("Student Name");
            arrColHdrNames2.Add("Student Name");
            data.Columns.Add("col" + colcount);
            colcount = colcount + 1;
            colHdrIndx = colcount - 1;
            //-----------------get syllabus year
            if (ddlsec.Items.Count > 0)
            {
                if (ddlsec.SelectedValue.ToString() == "")
                {
                    strsec = string.Empty;
                    strsec1 = string.Empty;
                }
                else
                {
                    strsec = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
                    strsec1 = ddlsec.SelectedValue.ToString();
                    //  strsec = ddlsec.SelectedValue.ToString();
                }
            }
            else
            {
                strsec = string.Empty;
                strsec1 = string.Empty;
            }
            //added By Srinath 11/8/2013
            string orderby_Setting = da.GetFunction("select value from master_Settings where settings='order_by'");
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
            string includePastout = string.Empty;

            if (!chkincludepastout.Checked)
            {
                includePastout = "and CC=0";
            }



            string sqlStr = "select distinct registration.Roll_Admit as Roll_Admit,registration.Roll_No as RollNumber,registration.Reg_No as RegistrationNumber,registration.stud_name as Student_Name,registration.stud_type as StudentType,registration.App_No as ApplicationNumber, convert(varchar(15),adm_date,103) as adm_date,registration.delflag,registration.Exam_Flag from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + "  and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 " + includePastout + includediscon + includedebar + " " + strsec + " " + ViewState["strvar"] + "  " + strorder + " ";//Hidden By Srinath
            //string sqlStr = "select distinct registration.Roll_Admit as Roll_Admit,registration.Roll_No as RollNumber,registration.Reg_No as RegistrationNumber,registration.stud_name as Student_Name,registration.stud_type as StudentType,registration.App_No as ApplicationNumber, convert(varchar(15),adm_date,103) as adm_date from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + "   and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0  " + strsec + " " + ViewState["strvar"] + " "+strorder+" ";
            con.Close();
            con.Open();
            if (sqlStr != "")
            {
                SqlDataAdapter adaSyll1 = new SqlDataAdapter(sqlStr, con);
                adaSyll1.Fill(roll_data);
            }

            date1 = txtFromDate.Text;
            string[] split = date1.Split(new Char[] { '/' });
            fdate = string.Empty;
            tdate = string.Empty;
            fdate = txtFromDate.Text;
            tdate = txtToDate.Text;
            string[] f_split = fdate.Split(new Char[] { '/' });
            string[] t_split = tdate.Split(new Char[] { '/' });
            string valu = string.Empty;
            f_month_year = Convert.ToInt16(f_split[1].ToString()) + (Convert.ToInt16(f_split[2].ToString()) * 12);
            t_month_year = Convert.ToInt16(t_split[1].ToString()) + (Convert.ToInt16(t_split[2].ToString()) * 12);
            hat.Clear();
            ds.Clear();
            hat.Add("Batch_Year", ddlbatch.SelectedValue.ToString());
            hat.Add("semester", ddlduration.SelectedValue.ToString());
            hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
            hat.Add("stdsec", strsec1);
            hat.Add("sc_code", Session["collegecode"]);
            hat.Add("ff_date", f_month_year);
            hat.Add("tt_date", t_month_year);
            hat.Add("ssf_cdate", Convert.ToDateTime(f_split[1].ToString() + "/" + f_split[0].ToString() + "/" + f_split[2].ToString()));
            hat.Add("sst_cdate", Convert.ToDateTime(t_split[1].ToString() + "/" + t_split[0].ToString() + "/" + t_split[2].ToString()));
            hat.Add("ispassedout", (chkincludepastout.Checked) ? "1" : "0");
            ds = da.select_method("LOAD_SUBJECT", hat, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() != string.Empty)
                {
                    syllabus_year = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count != 0)
                    {
                        int columcount = 0;

                        for (int i = 0; i < (ds.Tables[1].Rows.Count); i++)
                        {
                            columcount++;
                            arrColHdrNames1.Add(ds.Tables[1].Rows[i][0].ToString());
                            arrColHdrNames1.Add(ds.Tables[1].Rows[i][0].ToString());
                            arrColHdrNames2.Add("0");
                            colHdrIndx++;
                            data.Columns.Add("col" + colHdrIndx);

                            arrColHdrNames2.Add("%");
                            colHdrIndx++;
                            data.Columns.Add("col" + colHdrIndx);


                        }
                    }
                }
                else
                {
                    syllabus_year = -1;
                    errmsg.Visible = false;
                    errmsg.Text = "Update Semester Information";
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }

    }

    //protected override void Render(System.Web.UI.HtmlTextWriter writer)
    //{
    //    Control cntPageNextBtn = subject_report.FindControl("Next");
    //    Control cntPagePreviousBtn = subject_report.FindControl("Prev");
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

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        //subject_report.CurrentPage = 0;
        pagesearch_txt.Text = string.Empty;
        seterr.Visible = false;
        pagesearch_txt.Text = string.Empty;
        pageddltxt.Text = string.Empty;
        if (DropDownListpage.Text == "Others")
        {
            pageddltxt.Visible = true;
            pageddltxt.Focus();
        }
        else
        {
            pageddltxt.Visible = false;
            //subject_report.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            CalculateTotalPages();
        }
    }

    protected void pageddltxt_TextChanged(object sender, EventArgs e)
    {
        //subject_report.CurrentPage = 0;
        int row_cnt = 0;
        row_cnt = Showgrid.Rows.Count;
        pagesearch_txt.Text = string.Empty;
        try
        {
            if (Showgrid.Rows.Count > Convert.ToInt16(pageddltxt.Text.ToString()) && Convert.ToInt16(pageddltxt.Text.ToString()) != 0)
            {
                if (pageddltxt.Text != "")
                {
                    seterr.Visible = false;
                    Showgrid.PageSize = Convert.ToInt16(pageddltxt.Text.ToString());
                    CalculateTotalPages();
                }
            }
            else
            {
                seterr.Visible = true;
                seterr.Text = "Please Enter valid Record count";
                pageddltxt.Text = string.Empty;
            }
        }
        catch
        {
            seterr.Visible = true;
            seterr.Text = "Please Enter valid Record count";
            pageddltxt.Text = string.Empty;
        }
    }

    protected void pagesearch_txt_TextChanged(object sender, EventArgs e)
    {
        if (pagesearch_txt.Text.Trim() != "")
        {
            if (Convert.ToInt64(pagesearch_txt.Text) > Convert.ToInt64(Session["totalPages"]))
            {
                //subject_report.CurrentPage = 0;
                seterr.Visible = true;
                seterr.Text = "Exceed The Page Limit";
                pagesearch_txt.Text = string.Empty;
                Showgrid.Visible = true;
                btnxl.Visible = true;
                Printcontrol.Visible = true;
                btnprintmaster.Visible = false;
                btndirtPrint.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = true;
                lblrptname.Visible = true;
            }
            else if (Convert.ToInt64(pagesearch_txt.Text) == 0)
            {
                //subject_report.CurrentPage = 0;
                seterr.Visible = true;
                seterr.Text = "Page search should be more than 0";
                pagesearch_txt.Text = string.Empty;
                Showgrid.Visible = true;
                btnxl.Visible = true;
                Printcontrol.Visible = true;
                btnprintmaster.Visible = false;
                btndirtPrint.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = true;
                lblrptname.Visible = true;
            }
            else
            {
                errmsg.Visible = false;
                //subject_report.CurrentPage = Convert.ToInt16(pagesearch_txt.Text) - 1;
                Showgrid.Visible = true;
                btnxl.Visible = true;
                Printcontrol.Visible = true;
                btnprintmaster.Visible = false;
                btndirtPrint.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = true;
                lblrptname.Visible = true;
            }
        }
    }

    void CalculateTotalPages()
    {
        double totalRows = 0;
        totalRows = Convert.ToInt32(Showgrid.Rows.Count);
        Showgrid.Height = (Showgrid.PageSize * 23) + 50;
        Session["totalPages"] = (int)Math.Ceiling(totalRows / Showgrid.PageSize);
        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        Buttontotal.Visible = true;
    }

    protected void percheck_CheckedChanged(object sender, EventArgs e)
    {
        pertxt.Text = string.Empty;
        perddl.SelectedValue = "0";
        if (percheck.Checked == true)
        {
            perddl.Visible = true;
            pertxt.Visible = true;
        }
        else
        {
            perddl.Visible = false;
            pertxt.Visible = false;
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

    private string findday(int no, string sdate, string todate)
    {
        int order, holino;
        holino = 0;
        string day_order = string.Empty;
        string from_date = string.Empty;
        string fdate = string.Empty;
        int diff_work_day = 0;
        con.Close();
        con.Open();
        SqlDataReader dr;
        cmd = new SqlCommand("select count(*) from holidaystudents where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and holiday_date between '" + sdate.ToString() + "' and  '" + todate.ToString() + "' and halforfull='0' and isnull(Not_include_dayorder,0)<>'1'", con);
        dr = cmd.ExecuteReader();
        dr.Read();
        if (dr.HasRows == true)
        {
            holino = Convert.ToInt16(dr[0].ToString());
        }
        //  DateTime dt1 = Convert.ToDateTime(fdate.ToString());
        DateTime dt1 = Convert.ToDateTime(todate.ToString());
        DateTime dt2 = Convert.ToDateTime(sdate.ToString());
        TimeSpan t = dt1.Subtract(dt2);
        int days = t.Days;
        diff_work_day = days - holino;
        order = Convert.ToInt16(diff_work_day.ToString()) % no;
        if (order.ToString() == "0")
        {
            order = no;
        }
        if (order.ToString() == "1")
        {
            day_order = "mon";
        }
        else if (order.ToString() == "2")
        {
            day_order = "tue";
        }
        else if (order.ToString() == "3")
        {
            day_order = "wed";
        }
        else if (order.ToString() == "4")
        {
            day_order = "thu";
        }
        else if (order.ToString() == "5")
        {
            day_order = "fri";
        }
        else if (order.ToString() == "6")
        {
            day_order = "sat";
        }
        else if (order.ToString() == "7")
        {
            day_order = "sun";
        }
        con.Close();
        return (day_order);
    }

    protected void basedddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        setpanel.Visible = false;
        errmsg.Visible = false;
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btndirtPrint.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        setpanel.Visible = false;
        seterr.Visible = false;
    }

    protected void perddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        setpanel.Visible = false;
        errmsg.Visible = false;
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btndirtPrint.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        setpanel.Visible = false;
        seterr.Visible = false;
    }

    protected void subject_report_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        cellclick = true;
    }

    protected void subject_report_SelectedIndexChanged(Object sender, EventArgs e)
    {
        if (cellclick == true)
        {
            int act_row = 0;
            int act_col = 0;

            //saran
            //act_row = int.Parse(subject_report.ActiveSheetView.ActiveRow.ToString());
            //act_col = int.Parse(subject_report.ActiveSheetView.ActiveColumn.ToString());
            //if (act_col != null && act_row != null && act_col != -1 && act_row != -1)
            //{
            //    string roll_no = string.Empty;
            //    roll_no = subject_report.Sheets[0].Cells[act_row, 1].Text;
            //    Session["roll_no_session"] = roll_no.ToString();
            //    //Response.Redirect("dailystudentattndreport.aspx");
            //}
        }
        cellclick = false;
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

    protected void btnxl_Click(object sender, EventArgs e)
    {
        //Modified by Srinath 27/2/2013
        string reportname = txtexcelname.Text;
        if (reportname.ToString().Trim() != "")
        {
            da.printexcelreportgrid(Showgrid, reportname);
            txtexcelname.Text = string.Empty;
        }
        else
        {
            errmsg.Text = "Please Enter Your Report Name";
            errmsg.Visible = true;
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

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            string batch = "", sections = "", semester = "", degreecode = "", subcolumntext = "", strsec = string.Empty;
            bool child_flag = false;
            int sec_index = 0, sem_index = 0;
            batch = ddlbatch.SelectedValue.ToString();
            sections = ddlsec.SelectedValue.ToString();
            semester = ddlduration.SelectedValue.ToString();
            degreecode = ddlbranch.SelectedValue.ToString();
            if (ddlsec.Text == "")
            {
                strsec = string.Empty;
            }
            else
            {
                if (ddlsec.SelectedItem.ToString() == "")
                {
                    strsec = string.Empty;
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
            Session["page_redirect_value"] = ddlbatch.SelectedIndex + "," + ddldegree.SelectedIndex + "," + ddlbranch.SelectedIndex + "," + sem_index + "," + sec_index + "," + txtFromDate.Text + "," + txtToDate.Text + "," + basedddl.SelectedIndex + "," + percheck.Checked + "," + perddl.SelectedIndex + "," + pertxt.Text;
            // first_btngo();
            btnGo_Click(sender, e);
            errmsg.Visible = true;
            string clmnheadrname = string.Empty;
            int total_clmn_count = data.Columns.Count;
            //for (int srtcnt = 0; srtcnt < total_clmn_count; srtcnt++)
            //{
            //    if (subject_report.Sheets[0].Columns[srtcnt].Visible == true)
            //    {
            //        if (subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 3, srtcnt].Text != "")
            //        {
            //            subcolumntext = string.Empty;
            //            if (clmnheadrname == "")
            //            {
            //                clmnheadrname = subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 3, srtcnt].Text;
            //            }
            //            else
            //            {
            //                if (child_flag == false)
            //                {
            //                    clmnheadrname = clmnheadrname + "," + subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 3, srtcnt].Text;
            //                }
            //                else
            //                {
            //                    clmnheadrname = clmnheadrname + "$)," + subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 3, srtcnt].Text;
            //                }
            //            }
            //            child_flag = false;
            //        }
            //        //  else
            //        //{
            //        //    child_flag = true;
            //        //    if (subcolumntext == "")
            //        //    {
            //        //        for (int te = srtcnt - 1; te <= srtcnt; te++)
            //        //        {
            //        //            if (te == srtcnt - 1)
            //        //            {
            //        //                clmnheadrname = clmnheadrname + "* ($" + subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 2, te].Text;
            //        //                subcolumntext = clmnheadrname + "* ($" + subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 2, te].Text;
            //        //            }
            //        //            else
            //        //            {
            //        //                clmnheadrname = clmnheadrname + "$" + subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 2, te].Text;
            //        //                subcolumntext = clmnheadrname + "$" + subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 2, te].Text;
            //        //            }
            //        //        }
            //        //    }
            //        //    else
            //        //    {
            //        //        subcolumntext = subcolumntext + "$" + subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
            //        //        clmnheadrname = clmnheadrname + "$" + subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
            //        //    }
            //        //}
            //    }
            //}
            Response.Redirect("Print_Master_Setting_New.aspx?ID=" + clmnheadrname.ToString() + ":" + "subjwiseattndreport.aspx" + ":" + ddlbatch.SelectedItem.ToString() + " Batch - " + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] " + strsec + " :" + "Subject Wise Attendance With Percentage Report");
        }
        catch
        {
        }
    }

    public void spl_hrs()
    {
        try
        {

            includediscon = " and delflag=0";
            includedebar = " and exam_flag <> 'DEBAR'";
            includedisco = " and r.delflag=0";
            includedeba = " and r.exam_flag <> 'DEBAR'";
            string getshedulockva = da.GetFunctionv("select value from Master_Settings where settings='Attendance Discount' " + grouporusercode1 + "");
            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
            {
                includediscon = string.Empty;
                includedisco = "";
            }
            getshedulockva = da.GetFunctionv("select value from Master_Settings where  settings='Attendance Debar' " + grouporusercode1 + " ");
            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
            {
                includedebar = string.Empty;
                includedeba = "";
            }

            hasspl_tot.Clear();
            hasspl_pres.Clear();
            string splhrsec = string.Empty;
            if (ddlsec.Items.Count > 0)
            {
                if (Convert.ToString(ddlsec.SelectedValue).Trim() == "" || Convert.ToString(ddlsec.SelectedValue).Trim() == "-1" || Convert.ToString(ddlsec.SelectedValue).Trim().ToLower() == "all")
                {
                    splhrsec = string.Empty;
                }
                else
                {
                    // strsec = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
                    splhrsec = "and sm.sections='" + Convert.ToString(ddlsec.SelectedValue).Trim() + "' and sm.sections=r.sections";
                }
            }
            else
            {
                splhrsec = string.Empty;
            }
            con_splhr_query_master.Close();
            con_splhr_query_master.Open();
            DataSet ds_splhr_query_master = new DataSet();
            string includePastout = string.Empty;
            if (!chkincludepastout.Checked)
            {
                includePastout = "CC=0";
            }


            //  no_stud_flag = false;
            string splhr_query_master = "select attendance,sa.roll_no,sm.date,sd.subject_no from specialhr_attendance sa,registration r,specialhr_master sm ,specialhr_details sd where  r.roll_no=sa.roll_no and r.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and r.degree_code=" + ddlbranch.SelectedValue.ToString() + "  and (" + includePastout + ") " + includedisco + includedeba + " and  sd.hrdet_no=sa.hrdet_no and sd.hrentry_no=sm.hrentry_no and r.batch_year=sm.batch_year and r.degree_code=sm.degree_code and date between '" + from_date + "' and '" + to_date + "' " + splhrsec + "  order by r.roll_no asc";  //and r.current_semester=" + ddlduration.SelectedValue.ToString() + "
            SqlDataReader dr_splhr_query_master;
            cmd = new SqlCommand(splhr_query_master, con_splhr_query_master);
            dr_splhr_query_master = cmd.ExecuteReader();
            while (dr_splhr_query_master.Read())
            {
                if (dr_splhr_query_master.HasRows)
                {
                    key_value = dr_splhr_query_master[1].ToString() + "$" + dr_splhr_query_master[2].ToString() + "$" + dr_splhr_query_master[3].ToString();
                    if (!hasspl_tot.ContainsKey(key_value))
                    {
                        //  hasspl_tot.Add(key_value,Convert.ToInt32( dr_splhr_query_master[0].ToString()));
                        hasspl_tot.Add(key_value, "1");
                    }
                    else
                    {
                        attnd_val = Convert.ToString(GetCorrespondingKey(key_value, hasspl_tot));
                        hasspl_tot[key_value] = (Convert.ToInt32(attnd_val) + 1).ToString();
                    }
                    if (present_table.Contains((dr_splhr_query_master[0].ToString())))
                    {
                        if (!hasspl_pres.ContainsKey(key_value))
                        {
                            hasspl_pres.Add(key_value, "1");
                        }
                        else
                        {
                            attnd_val = Convert.ToString(GetCorrespondingKey(key_value, hasspl_pres));
                            hasspl_pres[key_value] = (Convert.ToInt32(attnd_val) + 1).ToString();
                        }
                    }
                    else
                    {
                        if (!hasspl_pres.ContainsKey(key_value))
                        {
                            hasspl_pres.Add(key_value, "0");
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string sections = string.Empty;
        if (ddlsec.Items.Count > 0)
        {
            sections = Convert.ToString(ddlsec.SelectedValue).Trim();
            if (Convert.ToString(sections).Trim().ToLower() == "all" || Convert.ToString(sections).Trim() == string.Empty || Convert.ToString(sections).Trim() == "-1")
            {
                sections = string.Empty;
            }
            else
            {
                sections = "- Sec-" + sections;
            }
        }
        string degreedetails = "Subject Wise Attendance With Percentage Report" + "@ Degree :" + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '-' + ddlbranch.SelectedItem.ToString() + '-' + "Sem-" + ddlduration.SelectedItem.ToString() + sections + '@' + "Date :" + txtFromDate.Text.ToString() + " To " + txtToDate.Text.ToString();
        string pagename = "subjwiseattndreport.aspx";
        string ss = null;
        Printcontrol.Visible = true;
        Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);


    }

    protected void chklabsubbatch_CheckedChanged(object sender, EventArgs e)
    {
        Showgrid.Visible = false;
        setpanel.Visible = false;
        frmlbl.Visible = false;
        tolbl.Visible = false;
        tofromlbl.Visible = false;
        errmsg.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btndirtPrint.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        txtexcelname.Text = string.Empty;
    }

    private void LoadAutoSwitchLab(string batchYear, string degreeCode, string semester, string section, string timeTableName, ref Dictionary<string, string> dicAutoSwitchLab, DataSet dsAutoSwitch = null)
    {
        try
        {
            if (dsAutoSwitch == null || (dsAutoSwitch.Tables.Count > 0 && dsAutoSwitch.Tables[0].Rows.Count == 0))
            {
                dsAutoSwitch = new DataSet();
                if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(timeTableName))
                {
                    string qry = "SELECT distinct sm.Batch_Year,sm.degree_code,sm.semester,ltrim(rtrim(isnull(l.Sections,''))) Sections,l.Timetablename,sch.FromDate,l.Day_Value,case when l.Day_Value='mon' then '1' when l.Day_Value='tue' then '2' when l.Day_Value='wed' then '3' when l.Day_Value='thu' then '4' when l.Day_Value='fri' then '5' when l.Day_Value='sat' then '6' when l.Day_Value='sun' then '7' end as Day_Code,l.Hour_Value,ltrim(rtrim(isnull(l.Auto_Switch,''))) as Auto_Switch,Count(l.Stu_Batch) as noOfBatch from LabAlloc l,syllabus_master sm,Semester_Schedule sch where l.Degree_Code=sm.degree_code and l.Batch_Year=sm.Batch_Year and sm.semester=l.Semester and sm.degree_code=sch.degree_code and l.Degree_Code=sch.degree_code and sch.batch_year=l.Batch_Year and sch.batch_year=l.Batch_Year and sch.semester=sm.semester and sch.semester=l.Semester and ltrim(rtrim(isnull(sch.Sections,'')))=ltrim(rtrim(isnull(l.Sections,''))) and TTName=l.Timetablename and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and ltrim(rtrim(isnull(l.Sections,''))) ='" + section + "' and Timetablename='" + timeTableName + "' group by sm.Batch_Year,sm.degree_code,sm.semester,l.Sections,l.Timetablename,sch.FromDate,l.Day_Value,l.Hour_Value,Auto_Switch order by sm.Batch_Year desc,sm.degree_code,sm.semester asc,ltrim(rtrim(isnull(l.Sections,''))),l.Timetablename,l.Day_Value,l.Hour_Value,sch.FromDate,Day_Code,ltrim(rtrim(isnull(l.Auto_Switch,'')))";//,ltrim(rtrim(isnull(l.Auto_Switch,'')))
                    dsAutoSwitch = da.select_method_wo_parameter(qry, "Text");
                }
            }
            dicAutoSwitchLab = new Dictionary<string, string>();
            if (dsAutoSwitch.Tables.Count > 0 && dsAutoSwitch.Tables[0].Rows.Count > 0)
            {
                dsAutoSwitch.Tables[0].DefaultView.RowFilter = "Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + semester + "' and Sections='" + section + "' and Timetablename='" + timeTableName + "' and auto_switch<>''";//and auto_switch<>''
                DataTable dtDate = new DataTable();
                dtDate = dsAutoSwitch.Tables[0].DefaultView.ToTable();
                if (dtDate.Rows.Count > 0)
                {
                    for (int au = 0; au < dtDate.Rows.Count; au++)
                    {
                        string autoSwitch = Convert.ToString(dtDate.Rows[au]["Day_Value"]).Trim() + Convert.ToString(dtDate.Rows[au]["Hour_Value"]).Trim();
                        if (!dicAutoSwitchLab.ContainsKey(autoSwitch.Trim().ToLower()))
                        {
                            dicAutoSwitchLab.Add(autoSwitch.Trim().ToLower(), Convert.ToString(dtDate.Rows[au]["auto_switch"]).Trim() + '-' + Convert.ToString(dtDate.Rows[au]["noOfBatch"]).Trim());
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private bool HasAutoSwitch(Dictionary<string, string> dicAutoSwitchLab, string dayName, string hourValue)
    {
        bool hasAutoSwitch = false;
        try
        {
            string keyValue = dayName.Trim().ToLower() + hourValue;
            if (dicAutoSwitchLab.ContainsKey(keyValue.Trim()))
            {
                hasAutoSwitch = true;
            }
            return hasAutoSwitch;
        }
        catch (Exception ex)
        {
            return hasAutoSwitch;
        }
    }

    protected void includepastout_CheckedChanged(object sender, EventArgs e)
    {

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
                for (int j = colcount; j < data.Columns.Count; j++)
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }

        }
        catch
        {
        }
    }


}
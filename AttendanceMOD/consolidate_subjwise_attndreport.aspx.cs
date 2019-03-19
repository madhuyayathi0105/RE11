//==========MANIPRABHA A.
using System;//=====14/3/12(complete), 15/3/12,16/3/12(XL), 28/3/12(<70), 29/3/12(halfholi day)
//=====================09/4/12(print setting complete), 11/05/12( halforfull='0'), 30/5/12(instead of > mension <)
//=====================16/6/12(include spl hr, try-catch,ISOcode,p_m_s_n)
//===========modified on 04.07.12 by mythili (header_alignment),20/7/12(txtcelltype, logo check->length)
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FarPoint.Web.Spread;
using InsproDataAccess;
using System.Text;

public partial class consolidate_subjwise_attndreport : System.Web.UI.Page
{
    #region variable init

    [Serializable()]
    public class MyImg : ImageCellType
    {
        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(100);
            return img;
        }
    }

    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    Hashtable hat = new Hashtable();
    Hashtable has = new Hashtable();
    Hashtable hasspl_tot = new Hashtable();
    Hashtable hasspl_pres = new Hashtable();
    static Boolean forschoolsetting = false;// Added by sridharan
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    // SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    // SqlConnection getcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    //SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    //   SqlConnection con_splhr_query_master = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    //SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    //SqlConnection con_attnd = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    //SqlConnection con_attnd1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd;
    //--------------------------------on 13/6/12 PRABHA
    string isonumber = "", new_header_string_index = "";
    List<string> present_table = new List<string>();
    string key_value = "", attnd_val = "";
    string subject_num_spl = "";
    int spl_total_conducted_hrs = 0, spl_total_attended_hrs = 0;
    //==============0n 6/4/12 PRABHA
    string[] string_session_values = new string[100];
    int temp_count = 0, final_print_col_cnt = 0, split_col_for_footer = 0, col_count = 0, footer_balanc_col = 0, footer_count = 0;
    int col_count_all = 0, span_cnt = 0, child_span_count = 0;
    Boolean check_col_count_flag = false;
    static DataSet dsprint = new DataSet();
    string new_header_string = "", column_field = "", printvar = "";
    string view_footer = "", view_header = "", view_footer_text = "";
    int start_column = 0, end_column = 0;
    string coll_name = "", address1 = "", address2 = "", address3 = "", form_name = "", phoneno = "", faxno = "", email = "", website = "", degree_val = "";
    string footer_text = "", header_alignment = "";
    string degree_deatil = "";
    int new_header_count = 0;
    string[] new_header_string_split;
    string phone = "", fax = "", email_id = "", web_add = "";
    Boolean btnclick_or_print = false;
    int between_visible_col_cnt = 0, between_visible_col_cnt_bal = 0;
    int x = 0;
    int visi_col = 0, visi_col1 = 0;
    //---------------------------
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    int split_holiday_status_1 = 0, split_holiday_status_2 = 0;
    string value_holi_status = "";
    string[] split_holiday_status = new string[1000];
    string get_date_holiday = "";
    Hashtable holiday = new Hashtable();
    int mng_hrs = 0, evng_hrs = 0;
    int first_half = 0, sec_half = 0;
    string usercode = "", collegecode = "", singleuser = "", group_user = "";
    string sem_start = "", sem_end = "";
    string strsec = "", strsec1 = "";
    DataSet roll_data = new DataSet();
    string date1 = "", fdate = "", tdate = "";
    string Unmark_hours = string.Empty;
    int f_month_year = 0, t_month_year = 0;
    string noofhrs = "", noofday = "", str_order = "", start_date = "", day_find = "";
    int no_of_hrs = 0, i = 0, subject_count = 0, no_of_days = 0, strorder = 0, rollcount = 0, tval = 0;
    DateTime from_date = new DateTime();
    DateTime to_date = new DateTime();
    DateTime t_date = new DateTime();
    DateTime f_date = new DateTime();
    DateTime s_date = new DateTime();
    DateTime dt1 = new DateTime();
    DateTime dt2 = new DateTime();
    DateTime Admission_date;
    int total_conducted_hrs = 0, total_attended_hrs = 0, whole_total_conducted_hrs = 0, row_cnt = 0;
    string[] split_date_time1;
    string[] dummy_split;
    int tempfdate = 0, tot_mnth = 0;
    int rollcolumncount = 0;
    string sume = "", srt_day = "";
    string d = "", d1 = "";
    int row = 0, table = 0, upper_bnd = 0;
    string[] s_code;
    string da1 = "", h = "";
    int k = 0;
    string davalue = "", attnd_perc = "";
    double attnd_perc_val = 0, avgstudent3 = 0;
    decimal avgstudent1 = 0, avgstudent2 = 0;
    int total_hrs = 0;
    string strdayflag = "", Master = "";
    string regularflag = "", genderflag = "";
    DataSet ds_student = new DataSet();
    DataSet ds_holi = new DataSet();
    DAccess2 dacc = new DAccess2();
    Hashtable hat_holy = new Hashtable();
    int stud_count = 0;
    DAccess2 d2 = new DAccess2();
    string order = "";
    string sem_start_date = "";
    int row_count = 0;
    Hashtable has_load_rollno = new Hashtable();
    Hashtable has_total_attnd_hour = new Hashtable();
    Hashtable has_total_onduty_hour = new Hashtable();
    DateTime temp_date = new DateTime();
    DataSet ds_attndmaster = new DataSet();
    int count_master = 0;
    Hashtable has_attnd_masterset = new Hashtable();
    Hashtable temp_has_subj_code = new Hashtable();
    int span_count = 0;
    Boolean holiflag = false;
    DataSet ds_alter = new DataSet();
    SqlCommand cmd_alt_shed;
    SqlCommand cmd_sem_shed;
    SqlCommand cmd_attnd;
    string strDay = "", dummy_date = "", temp_hr_field = "", subject_no = "";
    string date_temp_field = "", month_year = "";
    int roll_count = 0;
    int present_count = 0, onduty = 0;
    string full_hour = "";
    string single_hour = "";
    Boolean check_alter = false;
    Boolean recflag = false;
    Boolean no_stud_flag = false;
    Hashtable has_hs = new Hashtable();
    string Att_mark;
    Boolean spl_hr_flag = false;
    string roll_no = "";
    DataSet subds = new DataSet();
    int subj_count = 0;
    string section_lab = "";
    Hashtable stud_perccnt = new Hashtable();
    static string grouporusercode = "";
    static string grouporusercode1 = "";

    //added by srinath 18/2/2013
    DataSet ds_sphr = new DataSet();
    static Hashtable ht_sphr = new Hashtable();
    Boolean chkflag = false;
    Boolean splhr_flag = false;
    Hashtable has_attnd_masterset_notconsider = new Hashtable();
    string querystring = "";
    Hashtable hatsplhrattendance = new Hashtable();
    string CurrentDate = string.Empty;


    DataTable data = new DataTable();
    DataRow drow;
    Dictionary<int, string> dicstdroll = new Dictionary<int, string>();
    Dictionary<int, string> dicstdsubj = new Dictionary<int, string>();
    Dictionary<int, string> dicdicont = new Dictionary<int, string>();
    int columncnt = 0;

    string includediscon = "";
    string includedebar = "";

    string includedisco = "";
    string includedeba = "";
    InsproDirectAccess dir = new InsproDirectAccess();
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        if (!Page.IsPostBack)
        {
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                grouporusercode1 = " and  group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                grouporusercode1 = " and group_code=" + Session["usercode"].ToString().Trim() + "";
            }
            //frmlbl.Visible = false;
            //tolbl.Visible = false;
            //tofromlbl.Visible = false;
            errmsg.Visible = false;
            Showgrid.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            btnPrint.Visible = false;
            //Added By Srinath 27/2/2013
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            //setpanel.Visible = false;
            //ddlpage.Visible = false;
            //lblpages.Visible = false;
            chkondutyvisble.Checked = true;
            chkabsentvisble.Checked = true;

            //---------------------------

            //----------------------set date
            string dt = DateTime.Today.ToShortDateString();
            string[] dsplit = dt.Split(new Char[] { '/' });
            txtFromDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            txtToDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            Session["curr_year"] = dsplit[2].ToString();
            //-------------------------------Master settings
            //=======================on 09/4/12
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
                    bindbranch();
                    bindsem();
                    bindsec();
                    binddate();
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
                try
                {
                    string_session_values = Request.QueryString["val"].Split(',');
                    if (string_session_values.GetUpperBound(0) == 6)
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
                            //  setheader_print();//Hidden By Srinath 15/5/2013
                            view_header_setting();
                            Showgrid.Width = final_print_col_cnt * 100;
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
                    //===================================
                }
                catch
                {
                }
            }
            //======================
            strdayflag = "";
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            if (Session["usercode"] != "")
            {
                Master = "select * from Master_Settings where " + grouporusercode + "";
                ds.Dispose();
                ds.Reset();
                ds = da.select_method(Master, hat, "Text");
                Session["strvar"] = "";
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                Session["Daywise"] = "0";
                Session["Hourwise"] = "0";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Days Scholor" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            strdayflag = " and (r.Stud_Type='Day Scholar'";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Hostel" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
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
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Regular")
                        {
                            regularflag = "and ((r.mode=1)";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Lateral")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (r.mode=3)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((r.mode=3)";
                            }
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Transfer")
                        {
                            if (regularflag != "")
                            {
                                regularflag = regularflag + " or (r.mode=2)";
                            }
                            else
                            {
                                regularflag = regularflag + " and ((r.mode=2)";
                            }
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Male" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            genderflag = " and (app.sex='0'";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Female" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
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
            chksorthead.Visible = false;
            chkonduty.Checked = true;
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
                    // lblcollege.Text = "School";
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

    public void bindbatch()
    {
        ////batch

        includediscon = " and delflag=0";
        includedebar = " and exam_flag <> 'DEBAR'";
        includedisco = " and r.delflag=0";
        includedeba = " and r.exam_flag <> 'DEBAR'";
        string getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Discount' " + grouporusercode1 + " ");
        if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
        {
            includediscon = string.Empty;
            includedisco = string.Empty;
        }
        getshedulockva = d2.GetFunctionv("select value from Master_Settings where  settings='Attendance Debar' " + grouporusercode1 + "  ");
        if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
        {
            includedebar = string.Empty;
            includedeba = string.Empty;
        }
        ddlbatch.Items.Clear();
        string sqlstring = "";
        int max_bat = 0;
        DataSet ds1 = new DataSet();
        querystring = " select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 " + includediscon + includedebar + " order by batch_year";
        ds1 = da.select_method(querystring, hat, "Text");
        //da1.Fill(ds1);
        ddlbatch.DataSource = ds1;
        ddlbatch.DataValueField = "batch_year";
        ddlbatch.DataBind();
        //----------------display max year value 
        sqlstring = "select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 " + includediscon + includedebar + "";
        max_bat = Convert.ToInt32(da.GetFunction(sqlstring));
        ddlbatch.SelectedValue = max_bat.ToString();
        // con.Close();
        //binddegree();
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
        ds = da.select_method("bind_degree", hat, "sp");
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
        //bindsec();
    }

    public void bindsec()
    {
        //----------load section
        includediscon = " and delflag=0";
        includedebar = " and exam_flag <> 'DEBAR'";
        includedisco = " and r.delflag=0";
        includedeba = " and r.exam_flag <> 'DEBAR'";
        string getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Discount' " + grouporusercode1 + " ");
        if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
        {
            includediscon = string.Empty;
            includedisco = string.Empty;
        }
        getshedulockva = d2.GetFunctionv("select value from Master_Settings where  settings='Attendance Debar' " + grouporusercode1 + "  ");
        if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
        {
            includedebar = string.Empty;
            includedeba = string.Empty;
        }
        ddlsec.Items.Clear();
        con.Close();
        con.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' " + includediscon + includedebar + "", con);
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
        //frmlbl.Visible = false;
        //tolbl.Visible = false;
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
        ds = da.select_method("bind_branch", hat, "sp");
        int count2 = ds.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataBind();
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            chksorthead.Visible = false;
            stud_perccnt.Clear();
            btnclick();
            if (data.Rows.Count > 0)
            {

                int less70perc = 0;
                for (int rowcount = 2; rowcount < data.Rows.Count; rowcount++)
                {
                    less70perc = 0;

                    string roll_number = data.Rows[rowcount][1].ToString();
                    if (roll_number.Trim().ToString() != "")
                    {
                        if (stud_perccnt.ContainsKey(data.Rows[rowcount][1].ToString()))
                        {
                            less70perc = Convert.ToInt16(GetCorrespondingKey(data.Rows[rowcount][1].ToString(), stud_perccnt));
                        }
                        data.Rows[rowcount][data.Columns.Count - 1] = less70perc.ToString();


                    }

                    chksorthead.Visible = true;
                }

                int temp_col = 0;
                if (data.Columns.Count > 0 && data.Rows.Count > 0)//===========on 9/4/12
                {
                    Showgrid.DataSource = data;
                    Showgrid.DataBind();
                    Showgrid.Visible = true;
                    lblrptname.Visible = true;
                    foreach (KeyValuePair<int, string> dr in dicdicont)
                    {
                        int key = dr.Key;
                        Showgrid.Rows[key].BackColor = ColorTranslator.FromHtml("RED");


                    }


                    int rct = Showgrid.Rows.Count - 2;
                    //Rowspan
                    GridViewRow row = Showgrid.Rows[0];
                    GridViewRow previousRow = Showgrid.Rows[1];
                    Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Showgrid.Rows[0].Font.Bold = true;
                    Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                    Showgrid.Rows[1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Showgrid.Rows[1].Font.Bold = true;
                    Showgrid.Rows[1].HorizontalAlign = HorizontalAlign.Center;

                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        if (row.Cells[i].Text == previousRow.Cells[i].Text)
                        {
                            row.Cells[i].RowSpan = 2;
                            previousRow.Cells[i].Visible = false;


                            //row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                            //                       previousRow.Cells[i].RowSpan + 1;
                            //previousRow.Cells[i].Visible = false;
                        }
                    }

                    //ColumnSpan
                    for (int rowIndex = Showgrid.Rows.Count - rct - 1; rowIndex >= 0; rowIndex--)
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

                }
                else
                {
                    //setpanel .Visible = false;
                    btnxl.Visible = false;
                    Printcontrol.Visible = false;
                    btnprintmaster.Visible = false;
                    btnPrint.Visible = false;
                    //Added By Srinath 27/2/2013
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    Showgrid.Visible = false;
                    errmsg.Visible = true;
                    errmsg.Text = "No Record(s) Found";
                }
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
            //=============================0n 9/4/12
            //hidden by srinath 19/9/2013
            //hat.Clear();
            //hat.Add("college_code", Session["collegecode"].ToString());
            //hat.Add("form_name", "consolidate_subjwise_attndreport.aspx");
            //dsprint = da.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
            //===========================================
            string date1 = "";
            string datefrom = "";
            string date2 = "";
            string dateto = "";

            errmsg.Visible = false;
            btnxl.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            //Added By Srinath 27/2/2013
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            Showgrid.Visible = false;
            btnxl.Visible = false;
            //setpanel.Visible = false;
            //seterr.Visible = false;
            // tofromlbl.Visible = false;
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
                                //loadsubject();
                                loadstudent();
                                //    setheader();
                            }
                            else
                            {
                                //tofromlbl.Visible = true;
                                Showgrid.Visible = false;
                                //setpanel.Visible = false;
                                btnxl.Visible = false;
                                Printcontrol.Visible = false;
                                btnprintmaster.Visible = false;
                                btnPrint.Visible = false;
                                //Added By Srinath 27/2/2013
                                txtexcelname.Visible = false;
                                lblrptname.Visible = false;
                                // pagesetpanel.Visible = false;
                                errmsg.Visible = true;
                                errmsg.Text = "To Date Must be Greater than From Date";
                                goto labe;
                            }
                        }
                        else
                        {
                            //  pagesetpanel.Visible = false;
                            //tolbl.Visible = true;
                            //tolbl.Text = "Enter Valid To Date";
                            errmsg.Visible = false;
                            Showgrid.Visible = false;
                            btnxl.Visible = false;
                            Printcontrol.Visible = false;
                            btnprintmaster.Visible = false;
                            btnPrint.Visible = false;
                            //Added By Srinath 27/2/2013
                            txtexcelname.Visible = false;
                            lblrptname.Visible = false;
                            //setpanel.Visible = false;
                            errmsg.Visible = true;
                            errmsg.Text = "Enter Valid To Date";
                            goto labe;
                        }
                    }
                    else
                    {
                        //tolbl.Visible = true;
                        // pagesetpanel.Visible = false;
                        //tolbl.Text = "Enter Valid To Date";
                        errmsg.Visible = true;
                        Showgrid.Visible = false;
                        btnxl.Visible = false;
                        Printcontrol.Visible = false;
                        btnprintmaster.Visible = false;
                        btnPrint.Visible = false;
                        //Added By Srinath 27/2/2013
                        txtexcelname.Visible = false;
                        lblrptname.Visible = false;
                        // setpanel.Visible = false;
                        errmsg.Text = "Enter Valid To Date";
                        goto labe;
                    }
                }
                else
                {
                    //frmlbl.Visible = true;
                    //frmlbl.Text = "Enter Valid From Date";
                    //errmsg.Visible = false;
                    Showgrid.Visible = false;
                    btnxl.Visible = false;
                    Printcontrol.Visible = false;
                    btnprintmaster.Visible = false;
                    btnPrint.Visible = false;
                    //Added By Srinath 27/2/2013
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    //setpanel.Visible = false;
                    errmsg.Visible = true;
                    // tofromlbl.Visible = false;
                    errmsg.Text = "Enter Valid From Date";
                    goto labe;
                }
            }
            else
            {
                //frmlbl.Visible = true;
                //frmlbl.Text = "Enter Valid From Date";
                errmsg.Visible = false;
                Showgrid.Visible = false;
                btnxl.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                //setpanel.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "Enter Valid From Date";
                goto labe;
            }
            //-----------------------page search-total value display
            if (Convert.ToInt32(data.Rows.Count) != 0)
            {
                // setpanel.Visible = false;
                // pagesetpanel.Visible = true;
                Showgrid.Visible = true;
                txtexcelname.Visible = true;
                // lblrptname.Visible = true;
                btnxl.Visible = true;
                btnprintmaster.Visible = true;
                btnPrint.Visible = true;
                Printcontrol.Visible = false;
                //Double totalRows = 0;
                //totalRows = Convert.ToInt32(subject_report.Sheets[0].RowCount);
                //DropDownListpage.Items.Clear();
                //if (totalRows >= 10)
                //{
                //    subject_report.Sheets[0].PageSize = 10;
                //    for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                //    {
                //        DropDownListpage.Items.Add((k + 10).ToString());
                //    }
                //    DropDownListpage.Items.Add("Others");
                //    subject_report.Height = 410;
                //    subject_report.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                //    subject_report.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                //}
                //else if (totalRows == 0)
                //{
                //    DropDownListpage.Items.Add("0");
                //    subject_report.Height = 200;
                //}
                //else
                //{
                //    subject_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                //    DropDownListpage.Items.Add(subject_report.Sheets[0].PageSize.ToString());
                //    subject_report.Height = 30 + (38 * Convert.ToInt32(totalRows));
                //}
                //Session["totalPages"] = (int)Math.Ceiling(totalRows / subject_report.Sheets[0].PageSize);
                //Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                //if (Convert.ToInt16(Session["totalPages"].ToString()) > 0)
                //{
                //    pagesearch_txt.Visible = true;
                //    pgsearch_lbl.Visible = true;
                //}
                //else
                //{
                //    pagesearch_txt.Visible = false;
                //    pgsearch_lbl.Visible = false;
                //}
                //if (Convert.ToInt32(subject_report.Sheets[0].RowCount) > 10)
                //{
                //    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                //    subject_report.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                //    CalculateTotalPages();
                //}
                ////if (Convert.ToInt32(subject_report.Sheets[0].RowCount) == 0)
                ////{
                ////    setpanel.Visible = false;
                ////    subject_report.Visible = false;
                ////    errmsg.Visible = true;
                ////    errmsg.Text = "No record found";
                ////}
            }
            else
            {
                // pagesetpanel.Visible = false;
                Showgrid.Visible = false;
                btnxl.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                // setpanel.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "No record found";
            }
        labe: h = "";
        }
        catch
        {
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  pagesetpanel.Visible = false;
        errmsg.Visible = false;
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        //setpanel.Visible = false;
        // seterr.Visible = false;
        //bindbranch();
        //bindsem();
        //bindsec();
        binddate();
        chksorthead.Visible = false;
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        // pagesetpanel.Visible = false;
        errmsg.Visible = false;
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        //setpanel.Visible = false;
        //seterr.Visible = false;
        bindbranch();
        bindsem();
        bindsec();
        binddate();
        chksorthead.Visible = false;
    }

    public void binddate()
    {
        con.Close();
        con.Open();
        string from_date = "";
        string to_date = "";
        string final_from = "";
        string final_to = "";
        //Modified by srinath 9/10/2013
        ////SqlDataReader dr_dateset;
        //cmd = new SqlCommand("select start_date,end_date from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " ", con);
        //dr_dateset = cmd.ExecuteReader();
        //dr_dateset.Read();
        //if (dr_dateset.HasRows == true)
        //{
        ds.Reset();
        ds.Dispose();
        querystring = "select start_date,end_date from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " ";
        ds = da.select_method(querystring, hat, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            //------------get from date
            //from_date = dr_dateset[0].ToString();
            from_date = ds.Tables[0].Rows[0]["start_date"].ToString();
            string[] from_split = from_date.Split(' ');
            string[] date_split_from = from_split[0].Split('/');
            final_from = date_split_from[1] + "/" + date_split_from[0] + "/" + date_split_from[2];
            //sem_start=date_split_from[0] + "/" + date_split_from[1] + "/" + date_split_from[2];
            sem_start = final_from;
            txtFromDate.Text = final_from;
            Session["fromdate"] = final_from;
            //------------get to date
            //to_date = dr_dateset[1].ToString();
            to_date = ds.Tables[0].Rows[0]["end_date"].ToString();
            string[] to_split = to_date.Split(' ');
            string[] date_split_to = to_split[0].Split('/');
            final_to = date_split_to[1] + "/" + date_split_to[0] + "/" + date_split_to[2];
            txtToDate.Text = final_to;
            Session["todate"] = final_to;
            sem_end = final_to;
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //pagesetpanel.Visible = false;
        errmsg.Visible = false;
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        // setpanel.Visible = false;
        //seterr.Visible = false;
        bindsem();
        bindsec();
        binddate();
    }

    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  pagesetpanel.Visible = false;
        errmsg.Visible = false;
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        //setpanel.Visible = false;
        //seterr.Visible = false;
        bindsec();
        binddate();
        chksorthead.Visible = false;
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  pagesetpanel.Visible = false;
        binddate();
        errmsg.Visible = false;
        Showgrid.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        // setpanel.Visible = false;
        //seterr.Visible = false;
        //frmlbl.Visible = false;
        //tolbl.Visible = false;
        chksorthead.Visible = false;
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            errmsg.Visible = false;
            Showgrid.Visible = false;
            btnxl.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            chksorthead.Visible = false;
            string[] spiltfromdate = txtFromDate.Text.Split('/');
            string[] spilttodate = txtToDate.Text.Split('/');
            DateTime dtfrom = Convert.ToDateTime(spiltfromdate[1] + '/' + spiltfromdate[0] + '/' + spiltfromdate[2]);
            DateTime dtto = Convert.ToDateTime(spilttodate[1] + '/' + spilttodate[0] + '/' + spilttodate[2]);
            if (dtto < dtfrom)
            {
                errmsg.Visible = true;
                errmsg.Text = "To Date Must be Greater than From Date";
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = "Please Enter Valid Date";
        }
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            errmsg.Visible = false;
            Showgrid.Visible = false;
            btnxl.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            chksorthead.Visible = false;
            string[] spiltfromdate = txtFromDate.Text.Split('/');
            string[] spilttodate = txtToDate.Text.Split('/');
            DateTime dtfrom = Convert.ToDateTime(spiltfromdate[1] + '/' + spiltfromdate[0] + '/' + spiltfromdate[2]);
            DateTime dtto = Convert.ToDateTime(spilttodate[1] + '/' + spilttodate[0] + '/' + spilttodate[2]);
            if (dtto < dtfrom)
            {
                errmsg.Visible = true;
                errmsg.Text = "To Date Must be Greater than From Date";
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = "Please Enter Valid Date";
        }
    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void pageddltxt_TextChanged(object sender, EventArgs e)
    {
    }

    protected void pagesearch_txt_TextChanged(object sender, EventArgs e)
    {
    }

    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        //hat.Clear();
        //hat.Add("college_code", Session["collegecode"].ToString());
        //hat.Add("form_name", "consolidate_subjwise_attndreport.aspx");
        //dsprint = da.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        //if (dsprint.Tables[0].Rows.Count > 0)
        //{
        //    view_footer = dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString();
        //    view_header = dsprint.Tables[0].Rows[0]["header_flag_value"].ToString();
        //    view_footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
        //    errmsg.Visible = false;
        //    if (view_header == "0")
        //    {
        //        for (int i = 0; i < subject_report.Sheets[0].RowCount; i++)
        //        {
        //            subject_report.Sheets[0].Rows[i].Visible = false;
        //        }
        //        int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
        //        int end = start + 24;
        //        if (end >= subject_report.Sheets[0].RowCount)
        //        {
        //            end = subject_report.Sheets[0].RowCount;
        //        }
        //        int rowstart = subject_report.Sheets[0].RowCount - Convert.ToInt32(start);
        //        int rowend = subject_report.Sheets[0].RowCount - Convert.ToInt32(end);
        //        for (int i = start - 1; i < end; i++)
        //        {
        //            subject_report.Sheets[0].Rows[i].Visible = true;
        //        }
        //        for (int row_cnt = 0; row_cnt < subject_report.Sheets[0].ColumnHeader.RowCount; row_cnt++)
        //        {
        //            subject_report.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
        //        }
        //    }
        //    else if (view_header == "1")
        //    {
        //        for (int i = 0; i < subject_report.Sheets[0].RowCount; i++)
        //        {
        //            subject_report.Sheets[0].Rows[i].Visible = false;
        //        }
        //        int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
        //        int end = start + 24;
        //        if (end >= subject_report.Sheets[0].RowCount)
        //        {
        //            end = subject_report.Sheets[0].RowCount;
        //        }
        //        int rowstart = subject_report.Sheets[0].RowCount - Convert.ToInt32(start);
        //        int rowend = subject_report.Sheets[0].RowCount - Convert.ToInt32(end);
        //        for (int i = start - 1; i < end; i++)
        //        {
        //            subject_report.Sheets[0].Rows[i].Visible = true;
        //        }
        //        if (Convert.ToInt32(ddlpage.SelectedValue.ToString()) == 1)
        //        {
        //            for (int row_cnt = 0; row_cnt < subject_report.Sheets[0].ColumnHeader.RowCount; row_cnt++)
        //            {
        //                subject_report.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
        //            }
        //        }
        //        else
        //        {
        //            for (int row_cnt = 0; row_cnt < subject_report.Sheets[0].ColumnHeader.RowCount; row_cnt++)
        //            {
        //                subject_report.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < subject_report.Sheets[0].RowCount; i++)
        //        {
        //            subject_report.Sheets[0].Rows[i].Visible = false;
        //        }
        //        int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
        //        int end = start + 24;
        //        if (end >= subject_report.Sheets[0].RowCount)
        //        {
        //            end = subject_report.Sheets[0].RowCount;
        //        }
        //        int rowstart = subject_report.Sheets[0].RowCount - Convert.ToInt32(start);
        //        int rowend = subject_report.Sheets[0].RowCount - Convert.ToInt32(end);
        //        for (int i = start - 1; i < end; i++)
        //        {
        //            subject_report.Sheets[0].Rows[i].Visible = true;
        //        }
        //        {
        //            for (int row_cnt = 0; row_cnt < subject_report.Sheets[0].ColumnHeader.RowCount; row_cnt++)
        //            {
        //                subject_report.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
        //            }
        //        }
        //    }
        //    if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
        //    {
        //        if (view_header == "1" || view_header == "0")
        //        {
        //            for (int row_cnt = 0; row_cnt < subject_report.Sheets[0].ColumnHeader.RowCount; row_cnt++)
        //            {
        //                subject_report.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
        //            }
        //        }
        //        else
        //        {
        //            for (int row_cnt = 0; row_cnt < subject_report.Sheets[0].ColumnHeader.RowCount; row_cnt++)
        //            {
        //                subject_report.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
        //            }
        //        }
        //        for (int i = 0; i < subject_report.Sheets[0].RowCount; i++)
        //        {
        //            subject_report.Sheets[0].Rows[i].Visible = true;
        //        }
        //        Double totalRows = 0;
        //        totalRows = Convert.ToInt32(subject_report.Sheets[0].RowCount);
        //    //    //Session["totalPages"] = (int)Math.Ceiling(totalRows / subject_report.Sheets[0].PageSize);
        //    //    //Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        //    //    //DropDownListpage.Items.Clear();
        //    //    //if (totalRows >= 10)
        //    //    //{
        //    //    //    subject_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
        //    //    //    for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
        //    //    //    {
        //    //    //        DropDownListpage.Items.Add((k + 10).ToString());
        //    //    //    }
        //    //    //    DropDownListpage.Items.Add("Others");
        //    //    //    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
        //    //    //    subject_report.Height = 335;
        //    //    //}
        //    //    //else if (totalRows == 0)
        //    //    //{
        //    //    //    DropDownListpage.Items.Add("0");
        //    //    //    subject_report.Height = 100;
        //    //    //}
        //    //    //else
        //    //    //{
        //    //    //    subject_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
        //    //    //    DropDownListpage.Items.Add(subject_report.Sheets[0].PageSize.ToString());
        //    //    //    subject_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
        //    //    //}
        //    //    //if (Convert.ToInt32(subject_report.Sheets[0].RowCount) > 10)
        //    //    //{
        //    //    //    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
        //    //    //    subject_report.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
        //    //    //    //  subject_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
        //    //    //    CalculateTotalPages();
        //    //    //}
        //    //    //setpanel.Visible = true;
        //    //}
        //    //else
        //    //{
        //    //    setpanel.Visible = false;
        //    //}
        //    if (view_footer_text != "")
        //    {
        //        if (view_footer == "0")
        //        {
        //            subject_report.Sheets[0].Rows[(subject_report.Sheets[0].RowCount - 1)].Visible = true;
        //            subject_report.Sheets[0].Rows[(subject_report.Sheets[0].RowCount - 2)].Visible = true;
        //            subject_report.Sheets[0].Rows[(subject_report.Sheets[0].RowCount - 3)].Visible = true;
        //        }
        //        else
        //        {
        //            if (ddlpage.Text != "")
        //            {
        //                if (ddlpage.SelectedIndex != ddlpage.Items.Count - 1)
        //                {
        //                    subject_report.Sheets[0].Rows[(subject_report.Sheets[0].RowCount - 1)].Visible = false;
        //                    subject_report.Sheets[0].Rows[(subject_report.Sheets[0].RowCount - 2)].Visible = false;
        //                    subject_report.Sheets[0].Rows[(subject_report.Sheets[0].RowCount - 3)].Visible = false;
        //                }
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    errmsg.Visible = false;
        //    errmsg.Text = "No Header and Footer setting Assigned";
        //}
    }

    protected void RadioHeader_CheckedChanged(object sender, EventArgs e)
    {
    }

    protected void Radiowithoutheader_CheckedChanged(object sender, EventArgs e)
    {
    }

    //public string GetFunction(string Att_strqueryst)
    //{
    //    string sqlstr;
    //    sqlstr = Att_strqueryst;
    //    getcon.Close();
    //    getcon.Open();
    //    SqlDataReader drnew;
    //    SqlCommand cmd = new SqlCommand(sqlstr, getcon);
    //    drnew = cmd.ExecuteReader();
    //    drnew.Read();
    //    if (drnew.HasRows == true)
    //    {
    //        return drnew[0].ToString();
    //    }
    //    else
    //    {
    //        return "";
    //    }
    //}

    void CalculateTotalPages()
    {
        //Double totalRows = 0;
        //totalRows = Convert.ToInt32(subject_report.Sheets[0].RowCount);
        //subject_report.Height = (subject_report.Sheets[0].PageSize * 23) + 50;//--------------set heidht
        //Session["totalPages"] = (int)Math.Ceiling(totalRows / subject_report.Sheets[0].PageSize);
        //Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        //Buttontotal.Visible = true;
    }

    private string findday(int no, string sdate, string todate)//------------------find day order 
    {
        int order, holino;
        holino = 0;
        string day_order = "";
        string from_date = "";
        string fdate = "";
        int diff_work_day = 0;
        string[] spiltdate = todate.Split('/');
        todate = spiltdate[1] + '/' + spiltdate[0] + '/' + spiltdate[2];
        con.Close();
        con.Open();
        SqlDataReader dr;
        cmd = new SqlCommand("select count(*) from holidaystudents where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and holiday_date between '" + sdate.ToString() + "' and  '" + todate.ToString() + "' and halforfull='0'  and isnull(Not_include_dayorder,0)<>'1'", con);//01.03.17 barath
        dr = cmd.ExecuteReader();
        dr.Read();
        if (dr.HasRows == true)
        {
            holino = Convert.ToInt16(dr[0].ToString());
        }
        string quer = "select nodays from PeriodAttndSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString();
        string nodays = da.GetFunction(quer);
        if (nodays.Trim() == "" && nodays == null)
        {
            nodays = "0";
        }
        int no_days = Convert.ToInt32(nodays);
        //  DateTime dt1 = Convert.ToDateTime(fdate.ToString());
        DateTime dt1 = Convert.ToDateTime(todate.ToString());
        DateTime dt2 = Convert.ToDateTime(sdate.ToString());
        TimeSpan t = dt1.Subtract(dt2);
        int days = t.Days;
        diff_work_day = days - holino;
        //Modified by srinath 12/9/2013
        // order = Convert.ToInt16(diff_work_day.ToString()) % no;
        order = Convert.ToInt16(diff_work_day.ToString()) % no_days;
        order = order + 1;//Added by srinath 12/9/2013
        string stastdayorder = "";
        stastdayorder = da.GetFunction("select starting_dayorder from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + "");
        if (stastdayorder.ToString().Trim() != "")
        {
            if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
            {
                order = order + (Convert.ToInt16(stastdayorder) - 1);
                if (order == (no_days + 1))
                    order = 1;
                else if (order > no_days)
                    order = order % no_days;
            }
        }
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

    public string sem_roman(int sem)
    {
        string sql = "";
        string sem_roman = "";
        //Modified by srinath 9/10/2013
        //SqlDataReader rsChkSet;
        //con1.Close();
        //con1.Open();
        sql = "select * from inssettings where college_code=" + Session["collegecode"] + " and LinkName ='Semester Display'";
        //SqlCommand cmd1 = new SqlCommand(sql, con1);
        //rsChkSet = cmd1.ExecuteReader();
        //rsChkSet.Read();
        //if (rsChkSet.HasRows == true)
        ds.Dispose();
        ds.Reset();
        ds = da.select_method(sql, hat, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            //if (rsChkSet["linkvalue"].ToString() == "1")
            if (ds.Tables[0].Rows[0]["linkvalue"].ToString() == "1")
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
            d2.printexcelreportgrid(Showgrid, reportname);
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
        //        print = "Consolidate Subject Wise Attendance Report" + i;
        //        //subject_report.SaveExcel(appPath + "/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
        //        //Aruna on 26feb2013============================
        //        string szPath = appPath + "/Report/";
        //        string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")
        //        subject_report.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
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
        //try
        //{
        //    string batch = "", sections = "", semester = "", degreecode = "", subcolumntext = "", strsec = "";
        //    Boolean child_flag = false;
        //    int sec_index = 0, sem_index = 0;
        //    batch = ddlbatch.SelectedValue.ToString();
        //    sections = ddlsec.SelectedValue.ToString();
        //    semester = ddlduration.SelectedValue.ToString();
        //    degreecode = ddlbranch.SelectedValue.ToString();
        //    if (ddlsec.Text == "")
        //    {
        //        strsec = "";
        //    }
        //    else
        //    {
        //        if (ddlsec.SelectedItem.ToString() == "")
        //        {
        //            strsec = "";
        //        }
        //        else
        //        {
        //            strsec = " - " + ddlsec.SelectedItem.ToString();
        //        }
        //    }
        //    if (ddlsec.Enabled == false)
        //    {
        //        sec_index = -1;
        //    }
        //    else
        //    {
        //        sec_index = ddlsec.SelectedIndex;
        //    }
        //    if (ddlduration.Enabled == false)
        //    {
        //        sem_index = -1;
        //    }
        //    else
        //    {
        //        sem_index = ddlduration.SelectedIndex;
        //    }
        //    Session["page_redirect_value"] = ddlbatch.SelectedIndex + "," + ddldegree.SelectedIndex + "," + ddlbranch.SelectedIndex + "," + sem_index + "," + sec_index + "," + txtFromDate.Text + "," + txtToDate.Text;
        //    // first_btngo();
        //    btnGo_Click(sender, e);
        //    if (tofromlbl.Visible == false)
        //    {
        //        lblpages.Visible = true;
        //        ddlpage.Visible = true;
        //        string clmnheadrname = "";
        //        int total_clmn_count = subject_report.Sheets[0].ColumnCount;
        //        for (int srtcnt = 0; srtcnt < total_clmn_count; srtcnt++)
        //        {
        //            if (subject_report.Sheets[0].Columns[srtcnt].Visible == true)
        //            {
        //                if (subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text != "")
        //                {
        //                    subcolumntext = "";
        //                    if (clmnheadrname == "")
        //                    {
        //                        clmnheadrname = subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
        //                    }
        //                    else
        //                    {
        //                        if (child_flag == false)
        //                        {
        //                            clmnheadrname = clmnheadrname + "," + subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
        //                        }
        //                        else
        //                        {
        //                            clmnheadrname = clmnheadrname + "$)," + subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
        //                        }
        //                    }
        //                    child_flag = false;
        //                }
        //                else
        //                {
        //                    child_flag = true;
        //                    if (subcolumntext == "")
        //                    {
        //                        for (int te = srtcnt - 1; te <= srtcnt; te++)
        //                        {
        //                            if (te == srtcnt - 1)
        //                            {
        //                                clmnheadrname = clmnheadrname + "* ($" + subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
        //                                subcolumntext = clmnheadrname + "* ($" + subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
        //                            }
        //                            else
        //                            {
        //                                clmnheadrname = clmnheadrname + "$" + subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
        //                                subcolumntext = clmnheadrname + "$" + subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        subcolumntext = subcolumntext + "$" + subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
        //                        clmnheadrname = clmnheadrname + "$" + subject_report.Sheets[0].ColumnHeader.Cells[subject_report.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
        //                    }
        //                }
        //            }
        //        }
        //        Session["redirect_query_string"] = clmnheadrname.ToString() + ":" + "consolidate_subjwise_attndreport.aspx" + ":" + ddlbatch.SelectedItem.ToString() + " Batch - " + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] " + strsec + " :" + "Consolidate Attendance Details - Subject Wise Report";
        //        Response.Redirect("Print_Master_Setting_new.aspx");//?ID=" + clmnheadrname.ToString() + ":" + "consolidate_subjwise_attndreport.aspx" + ":" + ddlbatch.SelectedItem.ToString() + " Batch - " + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] " + strsec + " :" + "Consolidate Attendance Details - Subject Wise Report");
        //    }
        //    else
        //    {
        //        //lblpages.Visible = false;
        //        //ddlpage.Visible = false;
        //    }
        //}
        //catch
        //{
        //}
    }

    public void print_btngo()
    {
        //try
        //{
        //    final_print_col_cnt = 0;
        //    errmsg.Visible = false;
        //    check_col_count_flag = false;
        //    subject_report.Sheets[0].SheetCorner.RowCount = 0;
        //    subject_report.Sheets[0].ColumnCount = 0;
        //    subject_report.Sheets[0].RowCount = 0;
        //    subject_report.Sheets[0].SheetCorner.RowCount = 8;
        //    subject_report.Sheets[0].ColumnCount = 5;
        //    hat.Clear();
        //    hat.Add("college_code", Session["collegecode"].ToString());
        //    hat.Add("form_name", "consolidate_subjwise_attndreport.aspx");
        //    dsprint = da.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        //    if (dsprint.Tables[0].Rows.Count > 0)
        //    {
        //        lblpages.Visible = true;
        //        ddlpage.Visible = true;
        //        isonumber = dsprint.Tables[0].Rows[0]["ISOCode"].ToString();
        //        new_header_string_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString();
        //        //3. header add
        //        //if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
        //        //{
        //        //    new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
        //        //    new_header_string_split = new_header_string.Split(',');
        //        //    subject_report.Sheets[0].SheetCorner.RowCount = subject_report.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
        //        //}
        //        //3. end header add
        //        btnclick();
        //        //1.set visible columns
        //        column_field = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------------get column field
        //        if (column_field != "" && column_field != null)
        //        {
        //            //  check_col_count_flag = true;
        //            for (col_count_all = 0; col_count_all < subject_report.Sheets[0].ColumnCount; col_count_all++)
        //            {
        //                subject_report.Sheets[0].Columns[col_count_all].Visible = false;//------------invisible all column                                
        //            }
        //            printvar = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------visible setting columns
        //            string[] split_printvar = printvar.Split(',');
        //            for (int splval = 0; splval <= split_printvar.GetUpperBound(0); splval++)
        //            {
        //                span_cnt = 0;
        //                string[] split_star = split_printvar[splval].Split('*');
        //                if (split_star.GetUpperBound(0) > 0)
        //                {
        //                    for (col_count = 0; col_count < subject_report.Sheets[0].ColumnCount - 1; col_count++)
        //                    {
        //                        if (subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].ColumnHeader.RowCount - 2), col_count].Text == split_star[0])
        //                        {
        //                            child_span_count = 0;
        //                            string[] split_star_doller = split_star[1].Split('$');
        //                            for (int doller_count = 1; doller_count < split_star_doller.GetUpperBound(0); doller_count++)
        //                            {
        //                                for (int child_node = col_count; child_node <= col_count + split_star_doller.GetUpperBound(0); child_node++)
        //                                {
        //                                    if (subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].ColumnHeader.RowCount - 1), child_node].Text == split_star_doller[doller_count])
        //                                    {
        //                                        span_cnt++;
        //                                        if (span_cnt == 1 && child_node == col_count + 1)
        //                                        {
        //                                            subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].ColumnHeader.RowCount - 2), col_count + 1].Text = split_star[0].ToString();
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
        //                                        subject_report.Sheets[0].ColumnHeaderSpanModel.Add((subject_report.Sheets[0].ColumnHeader.RowCount - 2), col_count, 1, span_cnt);
        //                                        subject_report.Sheets[0].Columns[child_node].Visible = true;
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
        //                    for (col_count = 0; col_count < subject_report.Sheets[0].ColumnCount; col_count++)
        //                    {
        //                        if (subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].ColumnHeader.RowCount - 2), col_count].Text == split_printvar[splval])
        //                        {
        //                            subject_report.Sheets[0].Columns[col_count].Visible = true;
        //                            final_print_col_cnt++;
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            //1 end.set visible columns
        //        }
        //        else
        //        {
        //            subject_report.Visible = false;
        //            btnxl.Visible = false;
        //            Printcontrol.Visible = false;
        //            btnprintmaster.Visible = false;
        //            //Added By Srinath 27/2/2013
        //            txtexcelname.Visible = false;
        //            lblrptname.Visible = false;
        //            //setpanel.Visible = false;
        //            //lblpages.Visible = false;
        //            //ddlpage.Visible = false;
        //            errmsg.Visible = true;
        //            errmsg.Text = "Select Atleast One Column Field From The Treeview";
        //        }
        //    }
        //    // subject_report.Width = final_print_col_cnt * 100;
        //}
        //catch
        //{
        //}
    }

    //Hidden By Srinath 15/6/2013
    //public void setheader_print()
    //{
    //    try
    //    {
    //        // subject_report.Sheets[0].RemoveSpanCell
    //        //================header
    //        temp_count = 0;
    //        double logo_length = Convert.ToInt64(GetFunction("select datalength(logo2) from collinfo"));
    //        double logo_length_left = Convert.ToInt64(GetFunction("select datalength(logo1) from collinfo"));
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
    //                        subject_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (subject_report.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                        if (logo_length_left > 0 && logo_length_left.ToString() != "")
    //                        {
    //                            subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                        }
    //                        subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                    else
    //                    {
    //                        //  one_column();
    //                        more_column();
    //                        for (int row_cnt = 0; row_cnt < subject_report.Sheets[0].ColumnHeader.RowCount - 2; row_cnt++)
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
    //                        subject_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (subject_report.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                        if (logo_length_left > 0 && logo_length_left.ToString() != "")
    //                        {
    //                            subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                        }
    //                        subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                    else if (temp_count == 1)
    //                    {
    //                        // one_column();
    //                        more_column();
    //                        for (int row_cnt = 0; row_cnt < subject_report.Sheets[0].ColumnHeader.RowCount - 2; row_cnt++)
    //                        {
    //                            subject_report.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                            subject_report.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
    //                        }
    //                    }
    //                    else if (temp_count == 2)
    //                    {
    //                        //--------------------ISO CODE 13/6/12 PRABAH
    //                        if (isonumber != string.Empty)
    //                        {
    //                            subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].Text = "ISO CODE:" + isonumber;
    //                            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, (subject_report.Sheets[0].ColumnHeader.RowCount - 3), 1);
    //                            if (logo_length > 0 && logo_length.ToString() != "")
    //                            {
    //                                subject_report.Sheets[0].ColumnHeader.Cells[1, col_count].CellType = mi2;
    //                            }
    //                            subject_report.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorRight = Color.Black;
    //                            subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.Black;
    //                            subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
    //                            subject_report.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorTop = Color.White;
    //                        }
    //                        else
    //                        {
    //                            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (subject_report.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                            if (logo_length > 0 && logo_length.ToString() != "")
    //                            {
    //                                subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi2;
    //                            }
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
    //                        subject_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (6), 1);
    //                        if (logo_length_left > 0 && logo_length_left.ToString() != "")
    //                        {
    //                            subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                        }
    //                        subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                        subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
    //                    }
    //                    end_column = col_count;
    //                    temp_count++;
    //                    if (final_print_col_cnt == temp_count)
    //                    {
    //                        break;
    //                    }
    //                }
    //            }
    //            //--------------ISO 13/6/12 PRABHA
    //            if (isonumber != string.Empty)
    //            {
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Text = "ISO CODE:";// +isonumber;
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].Text = isonumber;
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].HorizontalAlign = HorizontalAlign.Left;
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorBottom = Color.White;
    //                subject_report.Sheets[0].ColumnHeaderSpanModel.Add(1, end_column, (5), 1);
    //                if (logo_length > 0 && logo_length.ToString() != "")
    //                {
    //                    subject_report.Sheets[0].ColumnHeader.Cells[1, end_column].CellType = mi2;
    //                }
    //                subject_report.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorBottom = Color.White;
    //                subject_report.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorRight = Color.Black;
    //                subject_report.Sheets[0].ColumnHeader.Columns[end_column].Width = 150;
    //                subject_report.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorTop = Color.White;
    //                subject_report.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorRight = Color.Black;
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Border.BorderColorBottom = Color.White;
    //                subject_report.Sheets[0].ColumnHeader.Cells[1, end_column - 1].Border.BorderColorTop = Color.White;
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Border.BorderColorRight = Color.White;
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorLeft = Color.White;
    //            }
    //            else
    //            {
    //                subject_report.Sheets[0].ColumnHeaderSpanModel.Add(0, end_column, (6), 1);
    //                if (logo_length > 0 && logo_length.ToString() != "")
    //                {
    //                    subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].CellType = mi2;
    //                }
    //                subject_report.Sheets[0].ColumnHeader.Columns[end_column].Width = 150;
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorRight = Color.Black;
    //                subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorBottom = Color.White;
    //            }
    //            //subject_report.Sheets[0].ColumnHeaderSpanModel.Add(0, end_column, (6), 1);
    //            //subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].CellType = mi2;
    //            //subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorLeft = Color.White;
    //            //subject_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorBottom = Color.White;
    //            temp_count = 0;
    //            for (col_count = 0; col_count < subject_report.Sheets[0].ColumnCount; col_count++)
    //            {
    //                if (subject_report.Sheets[0].Columns[col_count].Visible == true)
    //                {
    //                    if (temp_count == 1)
    //                    {
    //                        more_column();
    //                        for (int row_cnt = 0; row_cnt < subject_report.Sheets[0].ColumnHeader.RowCount - 2; row_cnt++)
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
    //        if (dsprint.Tables[0].Rows.Count > 0)
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

    #region Cmd_Saran
    //public void more_column()
    //{
    //    try
    //    {
    //        header_text();
    //        subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].Text = coll_name;
    //        subject_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
    //        //  subject_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, final_print_col_cnt - 2);
    //        if (final_print_col_cnt > 3)
    //        {
    //            if (isonumber != string.Empty)
    //            {
    //                subject_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count - 1));
    //            }
    //            else
    //            {
    //                subject_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count));
    //            }
    //            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, 1, (end_column - col_count));
    //            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(2, col_count, 1, (end_column - col_count));
    //            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(3, col_count, 1, (end_column - col_count));
    //            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(6, col_count, 1, (end_column - col_count));
    //            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(7, col_count, 1, (end_column - col_count));
    //            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(4, col_count, 1, (end_column - col_count));
    //            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(5, col_count, 1, (end_column - col_count));
    //        }
    //        subject_report.Sheets[0].ColumnHeader.Cells[1, col_count].Text = address1 + "-" + address2 + "-" + address3;
    //        subject_report.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorTop = Color.White;
    //        subject_report.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorBottom = Color.White;
    //        if (phoneno != "" && phoneno != null)
    //        {
    //            phone = "Phone:" + phoneno;
    //        }
    //        else
    //        {
    //            phone = "";
    //        }
    //        if (faxno != "" && faxno != null)
    //        {
    //            fax = "  Fax:" + faxno;
    //        }
    //        else
    //        {
    //            fax = "";
    //        }
    //        subject_report.Sheets[0].ColumnHeader.Cells[2, col_count].Text = phone + fax;
    //        subject_report.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorTop = Color.White;
    //        subject_report.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorBottom = Color.White;
    //        if (email != "" && faxno != null)
    //        {
    //            email_id = "Email:" + email;
    //        }
    //        else
    //        {
    //            email_id = "";
    //        }
    //        if (website != "" && website != null)
    //        {
    //            web_add = "  Web Site:" + website;
    //        }
    //        else
    //        {
    //            web_add = "";
    //        }
    //        subject_report.Sheets[0].ColumnHeader.Cells[3, col_count].Text = email_id + web_add;
    //        subject_report.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorTop = Color.White;
    //        subject_report.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorBottom = Color.White;
    //        if (form_name != "" && form_name != null)
    //        {
    //            subject_report.Sheets[0].ColumnHeader.Cells[4, col_count].Text = form_name;
    //            //    subject_report.Sheets[0].ColumnHeader.Cells[5, col_count].Text = "----------------------------------------------------"; //hided on 04.07.12
    //        }
    //        if (final_print_col_cnt <= 3)
    //        {
    //            subject_report.Sheets[0].ColumnHeader.Cells[6, col_count].Text = "Degree & Branch:" + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "     Regulation:" + da.GetFunction(" select regulation from degree  where degree_code=" + ddlbranch.SelectedValue.ToString() + ""); //"Name of the Program & Branch:"
    //            subject_report.Sheets[0].ColumnHeader.Cells[7, col_count].Text = "Academic Year:" + Session["curr_year"].ToString() + "Semester Number:" + ddlduration.SelectedValue.ToString();
    //            subject_report.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorTop = Color.White;
    //            subject_report.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorBottom = Color.White;
    //            subject_report.Sheets[0].ColumnHeader.Cells[7, col_count].Border.BorderColorTop = Color.White;
    //        }
    //        else
    //        {
    //            // between_visible_col_cnt = (end_column - col_count)/2;
    //            between_visible_col_cnt = (final_print_col_cnt - 1) / 2;
    //            between_visible_col_cnt_bal = (final_print_col_cnt - 1) % 2;
    //            //for ( x = start_column ; x <subject_report.Sheets[0].ColumnCount-1; x++)
    //            //{
    //            //    if(subject_report.Sheets[0].Columns[x].Visible==true)
    //            //    {
    //            //        visi_col++;
    //            //        if (visi_col == start_column + between_visible_col_cnt + between_visible_col_cnt_bal)
    //            //        {
    //            //            visi_col = x;
    //            //            break;
    //            //        }                   
    //            //    }
    //            //}
    //            subject_report.Sheets[0].ColumnHeader.Cells[6, start_column].Text = "Degree & Branch:" + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString();//"Name of the Program & Branch:"
    //            subject_report.Sheets[0].ColumnHeader.Cells[6, start_column].HorizontalAlign = HorizontalAlign.Left;
    //            subject_report.Sheets[0].ColumnHeader.Cells[6, start_column].Border.BorderColorBottom = Color.White;
    //            subject_report.Sheets[0].ColumnHeader.Cells[6, start_column].Border.BorderColorRight = Color.White;
    //            for (x = start_column; x <= subject_report.Sheets[0].ColumnCount - 1; x++)
    //            {
    //                if (subject_report.Sheets[0].Columns[x].Visible == true)
    //                {
    //                    visi_col1++;
    //                    if (visi_col1 == between_visible_col_cnt + between_visible_col_cnt_bal)
    //                    {
    //                        break;
    //                    }
    //                }
    //            }
    //            for (int xx = start_column + visi_col1 + 1; xx < subject_report.Sheets[0].ColumnCount - 1; xx++)
    //            {
    //                if (subject_report.Sheets[0].Columns[xx].Visible == true)
    //                {
    //                    visi_col = xx;
    //                    break;
    //                }
    //            }
    //            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(6, start_column, 1, visi_col1 + 1);
    //            subject_report.Sheets[0].ColumnHeader.Cells[6, visi_col].Text = "Regulation: " + da.GetFunction(" select regulation from degree  where degree_code=" + ddlbranch.SelectedValue.ToString() + "");  //modified on 04.07.12
    //            subject_report.Sheets[0].ColumnHeader.Cells[6, visi_col].Border.BorderColorLeft = Color.White;
    //            subject_report.Sheets[0].ColumnHeader.Cells[6, visi_col].Border.BorderColorRight = Color.White;
    //            subject_report.Sheets[0].ColumnHeader.Cells[6, visi_col].Border.BorderColorBottom = Color.White;
    //            subject_report.Sheets[0].ColumnHeader.Cells[6, visi_col].HorizontalAlign = HorizontalAlign.Right;//modified on 04.07.12
    //            subject_report.Sheets[0].ColumnHeader.Cells[6, end_column].HorizontalAlign = HorizontalAlign.Left;
    //            //   subject_report.Sheets[0].ColumnHeader.Cells[6, end_column].Text = GetFunction(" select regulation from degree  where degree_code=" + ddlbranch.SelectedValue.ToString() + "");
    //            subject_report.Sheets[0].ColumnHeader.Cells[6, end_column].Border.BorderColorTop = Color.White;
    //            int visi_col3 = 0, last_col = 0;
    //            for (int y = visi_col; y < end_column; y++)
    //            {
    //                if (subject_report.Sheets[0].Columns[y].Visible == true)
    //                {
    //                    visi_col3++;
    //                    last_col = y;
    //                }
    //            }
    //            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(6, visi_col, 1, visi_col3);
    //            subject_report.Sheets[0].ColumnHeader.Cells[6, end_column].Border.BorderColorBottom = Color.White;
    //            subject_report.Sheets[0].ColumnHeader.Cells[7, start_column].Text = "Academic Year:" + Session["curr_year"].ToString();
    //            subject_report.Sheets[0].ColumnHeader.Cells[7, start_column].HorizontalAlign = HorizontalAlign.Left;
    //            subject_report.Sheets[0].ColumnHeader.Cells[7, start_column].Border.BorderColorRight = Color.White;
    //            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(7, start_column, 1, visi_col1 + 1);
    //            subject_report.Sheets[0].ColumnHeader.Cells[7, visi_col].Text = "Semester Number: " + ddlduration.SelectedValue.ToString();  //modified on 04.07.12
    //            subject_report.Sheets[0].ColumnHeader.Cells[7, visi_col].Border.BorderColorTop = Color.White;
    //            subject_report.Sheets[0].ColumnHeader.Cells[7, visi_col].Border.BorderColorLeft = Color.White;
    //            subject_report.Sheets[0].ColumnHeader.Cells[7, visi_col].Border.BorderColorRight = Color.White;
    //            subject_report.Sheets[0].ColumnHeader.Cells[7, visi_col].HorizontalAlign = HorizontalAlign.Right; //modified on 04.07.12
    //            subject_report.Sheets[0].ColumnHeader.Cells[7, end_column].HorizontalAlign = HorizontalAlign.Left;
    //            //  subject_report.Sheets[0].ColumnHeader.Cells[7, end_column].Text = ddlduration.SelectedValue.ToString();
    //            subject_report.Sheets[0].ColumnHeader.Cells[7, end_column].Border.BorderColorTop = Color.White;
    //            subject_report.Sheets[0].ColumnHeaderSpanModel.Add(7, visi_col, 1, visi_col3);
    //        }
    //        subject_report.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorTop = Color.White;
    //        subject_report.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorBottom = Color.White;
    //        subject_report.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorTop = Color.White;
    //        subject_report.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorBottom = Color.White;
    //        int temp_count_temp = 0;
    //        if (dsprint.Tables[0].Rows.Count > 0)
    //        {
    //            if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
    //            {
    //                string[] new_header_string_index_split = new_header_string_index.Split(',');
    //                subject_report.Sheets[0].ColumnHeader.Cells[7, start_column].Border.BorderColorBottom = Color.White;
    //                subject_report.Sheets[0].ColumnHeader.Cells[7, end_column].Border.BorderColorBottom = Color.White;
    //                subject_report.Sheets[0].ColumnHeader.Cells[7, visi_col].Border.BorderColorBottom = Color.White;
    //                for (int row_head_count = 8; row_head_count < (8 + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
    //                {
    //                    subject_report.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Text = new_header_string_split[temp_count_temp].ToString();
    //                    //if (final_print_col_cnt > 3)
    //                    {
    //                        subject_report.Sheets[0].ColumnHeaderSpanModel.Add(row_head_count, start_column, 1, (subject_report.Sheets[0].ColumnCount - start_column + 1));
    //                    }
    //                    subject_report.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorTop = Color.White;
    //                    if (row_head_count != (8 + new_header_string_split.GetUpperBound(0)))
    //                    {
    //                        subject_report.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorBottom = Color.White;
    //                    }
    //                    if (temp_count_temp <= new_header_string_index_split.GetUpperBound(0))
    //                    {
    //                        header_alignment = new_header_string_index_split[temp_count_temp].ToString();
    //                        if (header_alignment != string.Empty)
    //                        {
    //                            if (header_alignment == "2")
    //                            {
    //                                subject_report.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Center;
    //                            }
    //                            else if (header_alignment == "1")
    //                            {
    //                                subject_report.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Left;
    //                            }
    //                            else
    //                            {
    //                                subject_report.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Right;
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

    #endregion
    public void header_text()
    {
        Boolean check_print_row = false;
        SqlDataReader dr_collinfo;
        con.Close();
        con.Open();
        cmd = new SqlCommand("select isnull(college_name,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website,isnull(form_heading_name,'') as form_name,isnull(batch_degree_branch,'') as degree_deatil,isnull(header_align,'') as header_alignment,isnull(header_flag_value,'') as view_header from print_master_setting  where form_name='consolidate_subjwise_attndreport.aspx'", con);
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
                    form_name = "Consolidate Attendance Details- Subject Wise Report";
                    degree_deatil = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
                    // header_alignment = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
                    // view_header = dr_collinfo["view_header"].ToString();
                }
            }
        }
    }

    public void view_header_setting()
    {
        try
        {
            //if (dsprint.Tables[0].Rows.Count > 0)
            //{
            //    ddlpage.Visible = true;
            //    lblpages.Visible = true;
            //    view_footer = dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString();
            //    view_header = dsprint.Tables[0].Rows[0]["header_flag_value"].ToString();
            //    view_footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
            //    if (view_header == "0" || view_header == "1")
            //    {
            //        errmsg.Visible = false;
            //        for (int row_cnt = 0; row_cnt < subject_report.Sheets[0].ColumnHeader.RowCount; row_cnt++)
            //        {
            //            subject_report.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
            //        }
            //        int i = 0;
            //        ddlpage.Items.Clear();
            //        int totrowcount = subject_report.Sheets[0].RowCount;
            //        int pages = totrowcount / 25;
            //        int intialrow = 1;
            //        int remainrows = totrowcount % 25;
            //        if (subject_report.Sheets[0].RowCount > 0)
            //        {
            //            int i5 = 0;
            //            ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
            //            for (i = 1; i <= pages; i++)
            //            {
            //                i5 = i;
            //                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
            //                intialrow = intialrow + 25;
            //            }
            //            if (remainrows > 0)
            //            {
            //                i = i5 + 1;
            //                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
            //            }
            //        }
            //        if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
            //        {
            //            for (i = 0; i < subject_report.Sheets[0].RowCount; i++)
            //            {
            //                subject_report.Sheets[0].Rows[i].Visible = true;
            //            }
            //            Double totalRows = 0;
            //            totalRows = Convert.ToInt32(subject_report.Sheets[0].RowCount);
            //            Session["totalPages"] = (int)Math.Ceiling(totalRows / subject_report.Sheets[0].PageSize);
            //            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
            //            DropDownListpage.Items.Clear();
            //            if (totalRows >= 10)
            //            {
            //                subject_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
            //                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
            //                {
            //                    DropDownListpage.Items.Add((k + 10).ToString());
            //                }
            //                DropDownListpage.Items.Add("Others");
            //                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
            //                subject_report.Height = 335;
            //            }
            //            else if (totalRows == 0)
            //            {
            //                DropDownListpage.Items.Add("0");
            //                subject_report.Height = 100;
            //            }
            //            else
            //            {
            //                subject_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
            //                DropDownListpage.Items.Add(subject_report.Sheets[0].PageSize.ToString());
            //                subject_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
            //            }
            //            if (Convert.ToInt32(subject_report.Sheets[0].RowCount) > 10)
            //            {
            //                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
            //                subject_report.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
            //                subject_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
            //                CalculateTotalPages();
            //            }
            //            setpanel.Visible = true;
            //        }
            //        else
            //        {
            //            errmsg.Visible = false;
            //            setpanel.Visible = false;
            //        }
            //    }
            //    else if (view_header == "2")
            //    {
            //        for (int row_cnt = 0; row_cnt < subject_report.Sheets[0].ColumnHeader.RowCount; row_cnt++)
            //        {
            //            subject_report.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
            //        }
            //        errmsg.Visible = false;
            //        int i = 0;
            //        ddlpage.Items.Clear();
            //        int totrowcount = subject_report.Sheets[0].RowCount;
            //        int pages = totrowcount / 25;
            //        int intialrow = 1;
            //        int remainrows = totrowcount % 25;
            //        if (subject_report.Sheets[0].RowCount > 0)
            //        {
            //            int i5 = 0;
            //            ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
            //            for (i = 1; i <= pages; i++)
            //            {
            //                i5 = i;
            //                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
            //                intialrow = intialrow + 25;
            //            }
            //            if (remainrows > 0)
            //            {
            //                i = i5 + 1;
            //                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
            //            }
            //        }
            //        if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
            //        {
            //            for (i = 0; i < subject_report.Sheets[0].RowCount; i++)
            //            {
            //                subject_report.Sheets[0].Rows[i].Visible = true;
            //            }
            //            Double totalRows = 0;
            //            totalRows = Convert.ToInt32(subject_report.Sheets[0].RowCount);
            //            Session["totalPages"] = (int)Math.Ceiling(totalRows / subject_report.Sheets[0].PageSize);
            //            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
            //            DropDownListpage.Items.Clear();
            //            if (totalRows >= 10)
            //            {
            //                subject_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
            //                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
            //                {
            //                    DropDownListpage.Items.Add((k + 10).ToString());
            //                }
            //                DropDownListpage.Items.Add("Others");
            //                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
            //                subject_report.Height = 335;
            //            }
            //            else if (totalRows == 0)
            //            {
            //                DropDownListpage.Items.Add("0");
            //                subject_report.Height = 100;
            //            }
            //            else
            //            {
            //                subject_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
            //                DropDownListpage.Items.Add(subject_report.Sheets[0].PageSize.ToString());
            //                subject_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
            //            }
            //            if (Convert.ToInt32(subject_report.Sheets[0].RowCount) > 10)
            //            {
            //                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
            //                subject_report.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
            //                //  subject_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
            //                CalculateTotalPages();
            //            }
            //            setpanel.Visible = true;
            //        }
            //        else
            //        {
            //            setpanel.Visible = false;
            //        }
            //    }
            //    else
            //    {
            //    }
            //}
        }
        catch
        {
        }
    }

    public void spl_hrs()
    {
        try
        {
            hasspl_tot.Clear();
            hasspl_pres.Clear();
            //con_splhr_query_master.Close();
            //con_splhr_query_master.Open();
            DataSet ds_splhr_query_master = new DataSet();
            //  no_stud_flag = false;
            string splhr_query_master = "select attendance,sa.roll_no,sm.date,sd.subject_no from specialhr_attendance sa,registration r,specialhr_master sm ,specialhr_details sd where  r.roll_no=sa.roll_no and r.batch_year=" + ddlbatch.SelectedValue.ToString() + " and r.current_semester=" + ddlduration.SelectedValue.ToString() + " and r.degree_code=" + ddlbranch.SelectedValue.ToString() + "  and (CC = 0)  AND (DelFlag = 0)  AND (Exam_Flag <> 'debar')  and  sd.hrdet_no=sa.hrdet_no and sd.hrentry_no=sm.hrentry_no and r.batch_year=sm.batch_year and r.degree_code=sm.degree_code and date between '" + from_date + "' and '" + to_date + "'  order by r.roll_no asc";
            //SqlDataReader dr_splhr_query_master;
            //cmd = new SqlCommand(splhr_query_master, con_splhr_query_master);
            //dr_splhr_query_master = cmd.ExecuteReader();
            ds_splhr_query_master = da.select_method(splhr_query_master, hat, "Text");
            for (int k = 0; k < ds_splhr_query_master.Tables[0].Rows.Count; k++)
            // while (dr_splhr_query_master.Read())
            {
                // if (dr_splhr_query_master.HasRows)
                //{
                key_value = ds_splhr_query_master.Tables[0].Rows[k][1].ToString() + "$" + ds_splhr_query_master.Tables[0].Rows[k][2].ToString() + "$" + ds_splhr_query_master.Tables[0].Rows[k][3].ToString();
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
                if (present_table.Contains((ds_splhr_query_master.Tables[0].Rows[k][0].ToString())))
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
                // }
            }
        }
        catch
        {
        }
    }

    public string filerfunction()
    {
        string orderby_Setting = dacc.GetFunction("select value from master_Settings where settings='order_by'");
        string strorder = "";
        string serialno = dacc.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
        if (serialno == "1")
        {
            strorder = "ORDER BY r.serialno";
        }
        else
        {
            if (orderby_Setting == "0")
            {
                strorder = "ORDER BY len (r.roll_no),r.roll_no";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "ORDER BY r.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strorder = "ORDER BY r.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "ORDER BY len (r.roll_no),r.roll_no,r.Reg_No,r.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "ORDER BY len (r.roll_no),r.roll_no,r.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "ORDER BY r.Reg_No,r.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "ORDER BY len (r.roll_no),r.roll_no,r.Stud_Name";
            }
        }
        return strorder;
    }

    public void loadstudent()
    {
        try
        {

            includediscon = " and delflag=0";
            includedebar = " and exam_flag <> 'DEBAR'";
            includedisco = " and r.delflag=0";
            includedeba = " and r.exam_flag <> 'DEBAR'";
            string getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Discount' " + grouporusercode1 + " ");
            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
            {
                includediscon = string.Empty;
                includedisco = string.Empty;
            }
            getshedulockva = d2.GetFunctionv("select value from Master_Settings where  settings='Attendance Debar' " + grouporusercode1 + "  ");
            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
            {
                includedebar = string.Empty;
                includedeba = string.Empty;
            }

            int colcnt = 0;
            System.Text.StringBuilder conhr = new System.Text.StringBuilder();
            fdate = "";
            tdate = "";
            fdate = txtFromDate.Text;
            tdate = txtToDate.Text;
            string[] dm_splt_new = fdate.ToString().Split('/');
            string[] date_increment_splt_new = tdate.ToString().Split('/');
            DateTime alt;
            from_date = Convert.ToDateTime(dm_splt_new[1].ToString() + "/" + dm_splt_new[0].ToString() + "/" + dm_splt_new[2].ToString());
            to_date = Convert.ToDateTime(date_increment_splt_new[1].ToString() + "/" + date_increment_splt_new[0].ToString() + "/" + date_increment_splt_new[2].ToString());
            t_date = to_date;
            f_date = from_date;
            dt1 = from_date;
            dt2 = to_date;
            //========find holiday
            has.Clear();
            has.Add("from_date", f_date);
            has.Add("to_date", t_date);
            has.Add("degree_code", ddlbranch.SelectedValue.ToString());
            has.Add("sem", ddlduration.SelectedValue.ToString());
            has.Add("coll_code", Session["collegecode"].ToString());
            int iscount = 0;
            //Modiofied by srinath 9/10/2013
            //holidaycon.Close();
            //holidaycon.Open();
            //Modified by subburaj 28/08/2014**************//
            string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + dt1.ToString() + "' and '" + dt2.ToString() + "' and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedItem.ToString() + "";
            //************End***********************//
            //SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, holidaycon);
            //SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);
            DataSet dsholiday = new DataSet();
            dsholiday = da.select_method(sqlstr_holiday, hat, "Text");
            //daholiday.Fill(dsholiday);
            if (dsholiday.Tables[0].Rows.Count > 0)
            {
                iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
            }
            has.Add("iscount", iscount);
            ds_holi = dacc.select_method("HOLIDATE_DETAILS_FINE", has, "sp");
            if (ds_holi.Tables[0].Rows.Count > 0)
            {
                for (int holi = 0; holi < ds_holi.Tables[0].Rows.Count; holi++)
                {
                    if (ds_holi.Tables[0].Rows[holi]["halforfull"].ToString() == "False")
                    {
                        halforfull = "0";
                    }
                    else
                    {
                        halforfull = "1";
                    }
                    if (ds_holi.Tables[0].Rows[holi]["morning"].ToString() == "False")
                    {
                        mng = "0";
                    }
                    else
                    {
                        mng = "1";
                    }
                    if (ds_holi.Tables[0].Rows[holi]["evening"].ToString() == "False")
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
            //============================section
            if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1")
            {
                strsec = "";
            }
            else
            {
                strsec = " and r.sections='" + ddlsec.SelectedItem.ToString() + "'";
            }
            //==================================
            //Added By Srinath 11/8/2013
            string strorder = filerfunction();
            //con.Close();
            //con.Open();
            //  cmd = new SqlCommand(" select distinct r.roll_no as 'ROLL NO', r.stud_name as 'STUD NAME', r.reg_no as 'REG NO',p.No_of_hrs_per_day as 'PER DAY',schorder as 'order',start_date,no_of_hrs_I_half_day,no_of_hrs_II_half_day,len(a.roll_no), convert(varchar(15),adm_date,103) as adm_date  FROM attendance a , registration r , Department d ,  PeriodAttndSchedule p  ,seminfo s,applyn app WHERE a.roll_no=r.roll_no and   r.degree_code=p.degree_code and  r.Batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and  s.batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and r.degree_code= " + ddlbranch.SelectedValue.ToString() + " and s.degree_code= " + ddlbranch.SelectedValue.ToString() + " and  s.semester=" + ddlduration.SelectedValue.ToString() + " and p.semester=" + ddlduration.SelectedValue.ToString() + "  and (r.CC = 0)  AND (r.DelFlag = 0)  AND (r.Exam_Flag <> 'debar') AND (r.Current_Semester IS NOT NULL) and  r.app_no=app.app_no " + strsec + "  " + Session["strvar"] + " order by r.roll_no  ", con);
            //cmd = new SqlCommand(" select distinct r.roll_no as 'ROLL NO', r.stud_name as 'STUD NAME', r.reg_no as 'REG NO',p.No_of_hrs_per_day as 'PER DAY',schorder as 'order',start_date,no_of_hrs_I_half_day,no_of_hrs_II_half_day,len(a.roll_no), convert(varchar(15),adm_date,103) as adm_date,r.serialno  FROM attendance a , registration r , Department d ,  PeriodAttndSchedule p  ,seminfo s,applyn app WHERE a.roll_no=r.roll_no and   r.degree_code=p.degree_code and  r.Batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and  s.batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and r.degree_code= " + ddlbranch.SelectedValue.ToString() + " and s.degree_code= " + ddlbranch.SelectedValue.ToString() + " and  s.semester=" + ddlduration.SelectedValue.ToString() + " and p.semester=" + ddlduration.SelectedValue.ToString() + "  and (r.CC = 0)  AND (r.DelFlag = 0)  AND (r.Exam_Flag <> 'debar') AND (r.Current_Semester IS NOT NULL) and  r.app_no=app.app_no " + strsec + "  " + Session["strvar"] + " " + strorder + "  ", con);
            //cmd = new SqlCommand(" select distinct r.roll_no as 'ROLL NO', r.stud_name as 'STUD NAME', r.reg_no as 'REG NO',p.No_of_hrs_per_day as 'PER DAY',schorder as 'order',start_date,no_of_hrs_I_half_day,no_of_hrs_II_half_day,len(a.roll_no), convert(varchar(15),adm_date,103) as adm_date  FROM attendance a , registration r , Department d ,  PeriodAttndSchedule p  ,seminfo s,applyn app WHERE a.roll_no=r.roll_no and   r.degree_code=p.degree_code and  r.Batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and  s.batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and r.degree_code= " + ddlbranch.SelectedValue.ToString() + " and s.degree_code= " + ddlbranch.SelectedValue.ToString() + " and  s.semester=" + ddlduration.SelectedValue.ToString() + " and p.semester=" + ddlduration.SelectedValue.ToString() + "  and (r.CC = 0)  AND (r.DelFlag = 0)  AND (r.Exam_Flag <> 'debar') AND (r.Current_Semester IS NOT NULL) and  r.app_no=app.app_no " + strsec + "  " + Session["strvar"] + " "+strorder+"  ", con);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(ds_student);




            querystring = "select distinct r.roll_no as 'ROLL NO', r.stud_name as 'STUD NAME', r.reg_no as 'REG NO',p.No_of_hrs_per_day as 'PER DAY',schorder as 'order',start_date,no_of_hrs_I_half_day,no_of_hrs_II_half_day,len(r.roll_no), convert(varchar(15),adm_date,103) as adm_date,r.serialno,r.delflag,r.Exam_Flag  FROM attendance a , registration r , Department d ,  PeriodAttndSchedule p  ,seminfo s,applyn app WHERE a.roll_no=r.roll_no and   r.degree_code=p.degree_code  and   r.Batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and  s.batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and r.degree_code= " + ddlbranch.SelectedValue.ToString() + " and s.degree_code= " + ddlbranch.SelectedValue.ToString() + " and  s.semester=" + ddlduration.SelectedValue.ToString() + " and p.semester=" + ddlduration.SelectedValue.ToString() + "  and (r.CC = 0) " + includedisco + includedeba + " AND (r.Current_Semester IS NOT NULL) and  r.app_no=app.app_no " + strsec + "  " + Session["strvar"] + " " + strorder + " ";
            ds_student = da.select_method(querystring, hat, "Text");//and r.roll_no='18EE001'
            stud_count = ds_student.Tables[0].Rows.Count;
            if (stud_count > 0)
            {
                Showgrid.Visible = true;
                btnxl.Visible = true;
                btnprintmaster.Visible = true;
                btnPrint.Visible = true;
                Printcontrol.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = true;
                // lblrptname.Visible = true;
                int temp_count_temp = 0;
                //hat.Clear();
                //hat.Add("college_code", Session["collegecode"].ToString());
                //hat.Add("form_name", "consolidate_subjwise_attndreport.aspx");
                //dsprint = d2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");            
                //if (dsprint.Tables[0].Rows.Count > 0)
                //{
                //    isonumber = dsprint.Tables[0].Rows[0]["ISOCode"].ToString();
                //    if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
                //    {
                //        subject_report.Sheets[0].ColumnHeader.Cells[7, 0].Border.BorderColorBottom = Color.White;
                //        new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                //        new_header_string_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString();
                //        new_header_string_split = new_header_string.Split(',');
                //        subject_report.Sheets[0].SheetCorner.RowCount = subject_report.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
                //    }
                //}
                //  subject_report.Sheets[0].ColumnHeader.RowCount = subject_report.Sheets[0].ColumnHeader.RowCount + 2;//Hidden By SRinath 15/5/2013

                //subject_report.Sheets[0].ColumnCount = 0;
                ArrayList arrColHdrNames1 = new ArrayList();
                ArrayList arrColHdrNames2 = new ArrayList();
                arrColHdrNames1.Add("S.No");
                arrColHdrNames2.Add("S.No");
                data.Columns.Add("SNo", typeof(string));
                if (Session["Rollflag"].ToString() == "1")
                {
                    arrColHdrNames1.Add("RollNo");
                    arrColHdrNames2.Add("RollNo");
                    data.Columns.Add("Roll No", typeof(string));
                    colcnt++;
                    columncnt++;
                }
                else
                {
                    arrColHdrNames1.Add("RollNo");
                    arrColHdrNames2.Add("RollNo");
                    data.Columns.Add("Roll No", typeof(string));
                    colcnt++;
                    columncnt++;

                }
                if (Session["Regflag"].ToString() == "1")
                {
                    arrColHdrNames1.Add("Reg No");
                    arrColHdrNames2.Add("Reg No");
                    data.Columns.Add("Reg No", typeof(string));
                    colcnt++;
                    columncnt++;
                }
                arrColHdrNames1.Add("Name of the Student");
                arrColHdrNames2.Add("Name of the Student");
                data.Columns.Add("Name of the Student", typeof(string));

                colcnt = colcnt + 2;
                columncnt = columncnt + 2;
                no_of_hrs = int.Parse(ds_student.Tables[0].Rows[0]["PER DAY"].ToString());
                mng_hrs = int.Parse(ds_student.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString());
                evng_hrs = int.Parse(ds_student.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString());
                order = ds_student.Tables[0].Rows[0]["order"].ToString();
                sem_start_date = ds_student.Tables[0].Rows[0]["start_date"].ToString();


                if (no_of_hrs > 0)
                {

                    ///////////////
                    //Aruna 06oct2012 Load subject==============================================================================
                    //getcon.Close();
                    //getcon.Open();
                    subj_count = 0;
                    string sqlstr = "Select S.Subject_Code,s.acronym, S.Subject_no, S.max_int_marks,SS.Subject_Type,s.acronym,s.subject_name  from Subject as s,Sub_Sem as ss ,Syllabus_Master as SMas where SMas.Syll_Code = S.Syll_Code and SMas.Syll_Code = SS.Syll_Code and SS.Syll_Code = S.Syll_Code and S.SubType_no = SS.Subtype_no and SS.Promote_Count = 1 and SMas.Degree_Code =" + ddlbranch.SelectedValue.ToString() + " and SMas.Batch_Year =" + ddlbatch.SelectedValue.ToString() + " and SMas.Semester = " + ddlduration.SelectedValue.ToString() + " order by S.Subject_no, SS.SubType_No ";
                    //SqlCommand sqlcmd = new SqlCommand(sqlstr, getcon);
                    //SqlDataAdapter sqldba = new SqlDataAdapter(sqlcmd);
                    //sqldba.Fill(subds);

                    //Modified by Mullai
                    subds = da.select_method(sqlstr, hat, "Text");
                    if (subds.Tables[0].Rows.Count != 0)
                    {

                        for (int i = 0; i < (subds.Tables[0].Rows.Count); i++)
                        {
                            subj_count++;
                            colcnt++;


                            arrColHdrNames1.Add(subds.Tables[0].Rows[i]["Subject_Code"].ToString() + " " + subds.Tables[0].Rows[i]["subject_name"].ToString());
                            arrColHdrNames2.Add("Conducted Periods");

                            arrColHdrNames1.Add(subds.Tables[0].Rows[i]["Subject_Code"].ToString() + " " + subds.Tables[0].Rows[i]["subject_name"].ToString());
                            arrColHdrNames2.Add("Attended Periods");


                            arrColHdrNames1.Add(subds.Tables[0].Rows[i]["Subject_Code"].ToString() + " " + subds.Tables[0].Rows[i]["subject_name"].ToString());
                            arrColHdrNames2.Add("On Duty Periods");


                            arrColHdrNames1.Add(subds.Tables[0].Rows[i]["Subject_Code"].ToString() + " " + subds.Tables[0].Rows[i]["subject_name"].ToString());
                            arrColHdrNames2.Add("Absent Periods");

                            arrColHdrNames1.Add(subds.Tables[0].Rows[i]["Subject_Code"].ToString() + " " + subds.Tables[0].Rows[i]["subject_name"].ToString());
                            arrColHdrNames2.Add("% Att");

                            conhr = new System.Text.StringBuilder("Conducted Periods");

                            AddTableColumn(data, conhr);
                            conhr = new System.Text.StringBuilder("Attended Periods");

                            AddTableColumn(data, conhr);

                            //if (chkondutyvisble.Checked == true)
                            //{
                            //    conhr = new System.Text.StringBuilder("On Duty Periods");

                            //    AddTableColumn(data, conhr);
                            //}
                            //if (chkabsentvisble.Checked == true)
                            //{
                            //    conhr = new System.Text.StringBuilder("Absent Periods");

                            //    AddTableColumn(data, conhr);
                            //}

                            conhr = new System.Text.StringBuilder("On Duty Periods");

                            AddTableColumn(data, conhr);
                            conhr = new System.Text.StringBuilder("Absent Periods");

                            AddTableColumn(data, conhr);

                            conhr = new System.Text.StringBuilder("% Att");

                            AddTableColumn(data, conhr);

                            //saran
                            dicstdsubj.Add(colcnt, subds.Tables[0].Rows[i]["Subject_Code"].ToString() + " " + subds.Tables[0].Rows[i]["subject_name"].ToString() + "$" + subds.Tables[0].Rows[i]["Subject_no"].ToString());

                            colcnt = colcnt + 4;

                        }


                        //subject_report.Sheets[0].ColumnHeaderSpanModel.Add((subject_report.Sheets[0].SheetCorner.RowCount - 2), 5, 1, subj_count);
                        //subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].SheetCorner.RowCount - 2), 5].Text = "ATTENDANCE  IN PERCENTAGE ";
                        //subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].SheetCorner.RowCount - 2), 5].HorizontalAlign = HorizontalAlign.Center;
                        // subject_report.Sheets[0].ColumnHeader.Rows[(subject_report.Sheets[0].SheetCorner.RowCount - 2)].Visible = false;//Hidden By SRinath 15/5/2013

                        arrColHdrNames1.Add("Total Conducted Periods");
                        arrColHdrNames2.Add("Total Conducted Periods");
                        arrColHdrNames1.Add("Total Attended Periods");
                        arrColHdrNames2.Add("Total Attended Periods");
                        arrColHdrNames1.Add("Total Percentage");
                        arrColHdrNames2.Add("Total Percentage");
                        arrColHdrNames1.Add("Number of Courses in which Student has less than 70 % Attendance");
                        arrColHdrNames2.Add("Number of Courses in which Student has less than 70 % Attendance");

                        data.Columns.Add("Total Conducted Periods", typeof(string));
                        data.Columns.Add("Total Attended Periods", typeof(string));
                        data.Columns.Add("Total Percentage", typeof(string));
                        data.Columns.Add("Number of Courses in which Student has less than 70 % Attendance", typeof(string));

                    }
                    //==========================================================================================================
                    //string s = "5959";

                    DataRow drHdr1 = data.NewRow();
                    DataRow drHdr2 = data.NewRow();

                    for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    {
                        drHdr1[grCol] = arrColHdrNames1[grCol];
                        drHdr2[grCol] = arrColHdrNames2[grCol];

                    }

                    data.Rows.Add(drHdr1);
                    data.Rows.Add(drHdr2);
                    for (int temp_stud_count = 0; temp_stud_count < stud_count; temp_stud_count++)
                    {
                        drow = data.NewRow();

                        row_count = temp_stud_count;
                        string admdate = ds_student.Tables[0].Rows[row_count]["adm_date"].ToString();
                        string[] admdatesp = admdate.Split(new Char[] { '/' });
                        admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
                        //subject_report.Sheets[0].Rows[subject_report.Sheets[0].RowCount-1].Visible = false;



                        drow["SNo"] = (temp_stud_count + 1).ToString();

                        if (Session["Rollflag"].ToString() == "1")
                            drow["Roll No"] = ds_student.Tables[0].Rows[row_count]["ROLL NO"].ToString();
                        else
                            drow["Roll No"] = ds_student.Tables[0].Rows[row_count]["ROLL NO"].ToString();
                        if (Session["Regflag"].ToString() == "1")
                            drow["Reg No"] = ds_student.Tables[0].Rows[row_count]["REG NO"].ToString();
                        drow["Name of The Student"] = ds_student.Tables[0].Rows[row_count]["STUD NAME"].ToString();

                        data.Rows.Add(drow);
                        string del = ds_student.Tables[0].Rows[row_count]["delflag"].ToString();
                        string examf = ds_student.Tables[0].Rows[row_count]["Exam_Flag"].ToString();
                        if (del == "1" || examf.ToUpper() == "DEBAR")
                        {
                            dicdicont.Add(data.Rows.Count - 1, ds_student.Tables[0].Rows[row_count]["ROLL NO"].ToString());
                        }




                        dicstdroll.Add((temp_stud_count + 2), ds_student.Tables[0].Rows[row_count]["ROLL NO"].ToString() + '-' + admdate.ToString());


                        has_load_rollno.Add(ds_student.Tables[0].Rows[row_count]["ROLL NO"].ToString(), 0);
                        has_total_attnd_hour.Add(ds_student.Tables[0].Rows[row_count]["ROLL NO"].ToString(), 0);

                        if (!hatsplhrattendance.Contains(ds_student.Tables[0].Rows[row_count]["ROLL NO"].ToString()))
                        {
                            hatsplhrattendance.Add(ds_student.Tables[0].Rows[row_count]["ROLL NO"].ToString(), row_count);
                        }

                    }


                    load_attendance();
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Update Master Setting";
                }
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "No Student(s) Available";
            }
        }
        catch
        {
        }
    }

    public void load_attendance()
    {
        try
        {

            includediscon = " and delflag=0";
            includedebar = " and exam_flag <> 'DEBAR'";
            includedisco = " and r.delflag=0";
            includedeba = " and r.exam_flag <> 'DEBAR'";
            string getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Discount' " + grouporusercode1 + "");
            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
            {
                includediscon = string.Empty;
                includedisco = string.Empty;
            }
            getshedulockva = d2.GetFunctionv("select value from Master_Settings where settings='Attendance Debar' " + grouporusercode1 + " ");
            if (getshedulockva.Trim() == "1" || getshedulockva.Trim().ToLower() == "true")
            {
                includedebar = string.Empty;
                includedeba = string.Empty;
            }


            Hashtable hatattendance = new Hashtable();//Added by srinath 9/10/2013
            //added By srinath 18/2/2013 ==STart
            string[] fromdatespit = txtFromDate.Text.Split('/');
            Dictionary<DateTime, byte> dicAlternateDayOrder = new Dictionary<DateTime, byte>();//magesh 3.9.18
            string[] todatespit = txtToDate.Text.Split('/');
            DateTime spfromdate = Convert.ToDateTime(fromdatespit[1] + '/' + fromdatespit[0] + '/' + fromdatespit[2]);
            DateTime sptodate = Convert.ToDateTime(todatespit[1] + '/' + todatespit[0] + '/' + todatespit[2]);
            ht_sphr.Clear();
            string splhrsec = "";
            if (ddlsec.SelectedValue.ToString() != "" && ddlsec.SelectedValue.ToString() != "-1")
            {
                splhrsec = "and sections='" + ddlsec.SelectedItem.ToString() + "'";
            }
            string hrdetno = "";
            string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " " + splhrsec + " and date between '" + spfromdate.ToString() + "' and '" + sptodate.ToString() + "'";
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
            //== End
            //============Added By srinath 30/5/2014
            string currlabsub = "select distinct s.subject_no,s.subject_name,s.subject_code,sy.Batch_Year,sy.degree_code,sy.semester from syllabus_master sy,sub_sem sm,subject s where sy.syll_code=sm.syll_code and sy.syll_code=s.syll_code and sm.syll_code=s.syll_code and sm.subType_no=s.subType_no and sm.Lab=1 and sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and sy.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and sy.semester='" + ddlduration.SelectedValue.ToString() + "'   order by sy.Batch_Year,sy.degree_code,sy.semester";
            DataSet dscurrlab = dacc.select_method_wo_parameter(currlabsub, "Text");
            Hashtable hatlab = new Hashtable();
            for (int l = 0; l < dscurrlab.Tables[0].Rows.Count; l++)
            {
                string strsub = dscurrlab.Tables[0].Rows[l]["subject_no"].ToString();
                if (!hatlab.Contains(strsub))
                {
                    hatlab.Add(strsub, strsub);
                }
            }
            //==================================================
            //Added by Srinath 5/9/2014 For Day Order Change=========Start==========================
            string getdeteails = "select convert(nvarchar(15),s.start_date,101) as start_date,nodays,s.starting_dayorder from seminfo s,PeriodAttndSchedule p where s.degree_code=p.degree_code and p.semester=s.semester and s.semester='" + ddlduration.SelectedItem.ToString() + "' and s.batch_year='" + ddlbatch.Text.ToString() + "'  and s.degree_code='" + ddlbranch.SelectedValue.ToString() + "'";
            getdeteails = getdeteails + " ; select * from tbl_consider_day_order where semester='" + ddlduration.SelectedItem.ToString() + "' and batch_year='" + ddlbatch.Text.ToString() + "'  and degree_code='" + ddlbranch.SelectedValue.ToString() + "'";
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
            Hashtable hatdc = new Hashtable();
            try
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
            catch
            {
            }
            //====================End=====================================================
            string subno_val = "";
            int less70perc = 0;
            double sub_prc = 0;
            int rcnt = 0;
            string strorder = filerfunction();
            if (data.Columns.Count > 4)
            {
                for (int colcnt = 5; colcnt <= data.Columns.Count - 1; colcnt++)
                {
                    rcnt = 0;
                    sub_prc = 0;
                    less70perc = 0;
                    roll_count = 0;
                    present_count = 0;
                    temp_hr_field = "";
                    has_load_rollno.Clear();
                    has_total_attnd_hour.Clear();
                    has_total_onduty_hour.Clear();
                    onduty = 0;
                    if (dicstdsubj.ContainsKey(colcnt))
                    {

                        string subcode = dicstdsubj[colcnt];
                        string[] split = subcode.Split('$');

                        subno_val = split[1];
                        if (subno_val.Trim().ToString() != "")
                        {
                            string rstrsec = "";
                            try
                            {
                                temp_date = dt1;
                                subject_no = subno_val;
                                splhrsec = "";
                                if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1")
                                {
                                    strsec = "";
                                    rstrsec = "";
                                    splhrsec = "";
                                }
                                else
                                {
                                    strsec = " and sections='" + ddlsec.SelectedItem.ToString() + "'";
                                    rstrsec = " and r.sections='" + ddlsec.SelectedItem.ToString() + "'";
                                    splhrsec = " and l.sections='" + ddlsec.SelectedItem.ToString() + "'";
                                }
                                //==================================
                                //modified By Srinath 22/2/2013 
                                if (chkflag == false)
                                {
                                    chkflag = true;
                                    ds_attndmaster.Dispose();
                                    ds_attndmaster.Reset();
                                    has.Clear();
                                    has.Add("colege_code", Session["collegecode"].ToString());
                                    ds_attndmaster = dacc.select_method("ATT_MASTER_SETTING", has, "sp");
                                    count_master = (ds_attndmaster.Tables[0].Rows.Count);
                                    if (count_master > 0)
                                    {
                                        for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                                        {
                                            if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() != "2")
                                            {
                                                if (!has_attnd_masterset.ContainsKey(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                                                {
                                                    has_attnd_masterset.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString());
                                                }
                                            }
                                            if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "2")//==31/5/12 pRABHA
                                            {
                                                if (!has_attnd_masterset_notconsider.ContainsKey(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                                                {
                                                    has_attnd_masterset_notconsider.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString());
                                                }
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
                                    querystring = "select rights from  special_hr_rights where " + grouporusercode + "";
                                    DataSet dsrights = da.select_method(querystring, hat, "Text");
                                    if (dsrights.Tables[0].Rows.Count > 0)
                                    {
                                        if (dsrights.Tables[0].Rows[0]["rights"].ToString().ToLower().Trim() == "true")
                                        {
                                            splhr_flag = true;
                                        }
                                    }
                                }
                                string subj_type = da.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subject_no + "'");
                                Dictionary<string, string> dicDate = new Dictionary<string, string>();
                                dicDate.Clear();
                                while (temp_date <= dt2)
                                {
                                    if (!hatdc.Contains(temp_date))//Added by Srinath 5/9/2014 For Day Order Change
                                    {
                                        if (splhr_flag == true)
                                        {
                                            if (ht_sphr.Contains(Convert.ToString(temp_date)))
                                            {
                                                getspecial_hr();
                                            }
                                        }
                                        span_count = 0;
                                        if (!hat_holy.ContainsKey(temp_date))
                                        {
                                            if (!hat_holy.ContainsKey(temp_date))
                                            {
                                                hat_holy.Add(temp_date, "3*0*0");
                                            }
                                        }
                                        value_holi_status = GetCorrespondingKey(temp_date, hat_holy).ToString();
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
                                                split_holiday_status_1 = mng_hrs + 1;
                                                split_holiday_status_2 = no_of_hrs;
                                            }
                                            if (split_holiday_status[2].ToString() == "1")//==============evng holiday//mng working day
                                            {
                                                split_holiday_status_1 = 1;
                                                split_holiday_status_2 = mng_hrs;
                                            }
                                        }
                                        else if (split_holiday_status[0].ToString() == "0")//=================fulday holiday
                                        {
                                            split_holiday_status_1 = 0;
                                            split_holiday_status_2 = 0;
                                        }
                                        if (split_holiday_status_1 == 0 && split_holiday_status_2 == 0)
                                        {
                                            //temp_date = temp_date.AddDays(1); //Hidden by srinath 11/9/2014
                                        }
                                        else
                                        {
                                            holiflag = true;
                                            ds_alter.Clear();
                                            //---------------alternate schedule
                                            DataSet dsalterdet = new DataSet();
                                            ds_alter.Dispose();
                                            ds_alter.Reset();
                                            string alterquery = "select  * from alternate_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedItem.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and FromDate ='" + temp_date + "' " + strsec + " order by FromDate Desc";
                                            ds_alter = da.select_method(alterquery, hat, "Text");

                                            string alterquery1 = "select  * from AlternateDetails where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedItem.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and AlternateDate ='" + temp_date + "' " + strsec + " order by AlternateDate Desc";
                                            dsalterdet = da.select_method(alterquery1, hat, "Text");

                                            //---------------------------------------------
                                            ds.Clear();
                                            ds.Dispose();
                                            ds.Reset();
                                            string query = "select top 1 * from semester_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedItem.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and FromDate <='" + temp_date + "' " + strsec + " order by FromDate Desc";
                                            ds = da.select_method(query, hat, "Text");
                                            hatattendance.Clear();
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                if (no_of_hrs > 0)
                                                {
                                                    dummy_date = temp_date.ToString();
                                                    string[] dummy_date_split = dummy_date.Split(' ');
                                                    string[] final_date_string = dummy_date_split[0].Split('/');
                                                    dummy_date = final_date_string[1].ToString() + "/" + final_date_string[0].ToString() + "/" + final_date_string[2].ToString();
                                                    month_year = ((Convert.ToInt16(final_date_string[2].ToString()) * 12) + (Convert.ToInt16(final_date_string[0].ToString()))).ToString();
                                                    if (order != "0")
                                                    {
                                                        strDay = temp_date.ToString("ddd");
                                                    }
                                                    else
                                                    {
                                                        //Modified by srinath 5/9/2014
                                                        //strDay = findday(no_of_hrs, sem_start_date.ToString(), dummy_date);
                                                        string[] sp = dummy_date.Split('/');
                                                        string curdate = sp[1] + '/' + sp[0] + '/' + sp[2];
                                                        strDay = d2.findday(curdate, ddlbranch.SelectedValue.ToString(), ddlduration.SelectedItem.ToString(), ddlbatch.Text.ToString(), semstartdate, noofdays, startday);
                                                        //magesh 3.9.18
                                                        if (dicAlternateDayOrder.ContainsKey(temp_date))
                                                        {
                                                            strDay = d2.findDayName(dicAlternateDayOrder[temp_date]);
                                                            string Day_Order = Convert.ToString(dicAlternateDayOrder[temp_date]).Trim();
                                                        } //magesh 3.9.18
                                                    }

                                                    for (int temp_hr = split_holiday_status_1; temp_hr <= split_holiday_status_2; temp_hr++)
                                                    {
                                                        CurrentDate = dummy_date;
                                                        Boolean samehr_flag = false;
                                                        roll_count = 0;
                                                        present_count = 0;
                                                        temp_hr_field = strDay + temp_hr;
                                                        date_temp_field = "d" + final_date_string[1].ToString() + "d" + temp_hr;
                                                        hatattendance.Clear();
                                                        //string strAltDet = "select * from AlternateDetails where AlternateDate='" + f_date.ToString("MM/dd/yyyy") + "' and AlterHour='" + temp_hr_field + "' order by Noalter desc";
                                                        //DataTable dtAlt = dir.selectDataTable(strAltDet);

                                                        string hour = ds.Tables[0].Rows[0][temp_hr_field].ToString();
                                                        Boolean checalter = false;
                                                        string single_hour2 = "";
                                                        if (hour != "")
                                                        {
                                                            string[] split_full_hour1 = hour.Split(';');
                                                            for (int g1 = 0; g1 <= split_full_hour1.GetUpperBound(0); g1++)
                                                            {

                                                                string single_hour1 = split_full_hour1[g1].ToString();
                                                                string[] split_single_hour1 = single_hour1.Split('-');

                                                                if (split_single_hour1.GetUpperBound(0) >= 1)
                                                                {
                                                                    dsalterdet.Tables[0].DefaultView.RowFilter = "ActSubNo='" + split_single_hour1[0].ToString() + "' and AlterHour='" + temp_hr + "' ";
                                                                    DataView dvstaffdetails = dsalterdet.Tables[0].DefaultView;

                                                                    //ds_alter.Tables[0].DefaultView.RowFilter=
                                                                    if (!checalter)
                                                                    {
                                                                        if (dvstaffdetails.Count > 0)
                                                                        {
                                                                            if (single_hour2 == "")
                                                                                single_hour2 = ds_alter.Tables[0].Rows[0][temp_hr_field].ToString();
                                                                            else
                                                                                single_hour2 = single_hour2 + ";" + ds_alter.Tables[0].Rows[0][temp_hr_field].ToString();

                                                                            //single_hour2 = ds_alter.Tables[0].Rows[0][temp_hr_field].ToString();
                                                                            check_alter = false;
                                                                            checalter = true;
                                                                        }
                                                                        else
                                                                        {
                                                                            check_alter = false;
                                                                            if (single_hour2 == "")
                                                                                single_hour2 = single_hour1;
                                                                            else
                                                                                single_hour2 = single_hour2 + ";" + single_hour1;
                                                                        }
                                                                    }

                                                                }
                                                            }
                                                        }
                                                        if (check_alter)
                                                        {
                                                            if (ds_alter.Tables[0].Rows.Count > 0)
                                                            {
                                                                for (int hasrow = 0; hasrow < ds_alter.Tables[0].Rows.Count; hasrow++)
                                                                {
                                                                    full_hour = ds_alter.Tables[0].Rows[hasrow][temp_hr_field].ToString();
                                                                    if (full_hour.Trim() != "")
                                                                    {
                                                                        temp_has_subj_code.Clear();
                                                                        string[] split_full_hour = full_hour.Split(';');
                                                                        //=======================Srinath 30/5/2014========================
                                                                        Boolean batchflag = false;
                                                                        for (int g = 0; g <= split_full_hour.GetUpperBound(0); g++)
                                                                        {
                                                                            string[] valhr = split_full_hour[g].ToString().Split('-');
                                                                            if (valhr.GetUpperBound(0) > 1)
                                                                            {
                                                                                string lsub = valhr[0].ToString();

                                                                                if (hatlab.Contains(lsub))
                                                                                {
                                                                                    batchflag = true;
                                                                                }
                                                                            }
                                                                        }
                                                                        //===============================================
                                                                        for (int semi_colon = 0; semi_colon <= split_full_hour.GetUpperBound(0); semi_colon++)
                                                                        {
                                                                            roll_count = 0;
                                                                            single_hour = split_full_hour[semi_colon].ToString();
                                                                            string[] split_single_hour = single_hour.Split('-');

                                                                            if (split_single_hour.GetUpperBound(0) >= 1)
                                                                            {

                                                                                check_alter = true;
                                                                                if (split_single_hour[0].ToString() == subject_no)
                                                                                {
                                                                                    if (!temp_has_subj_code.ContainsKey(subject_no))
                                                                                    {
                                                                                        temp_has_subj_code.Add(subject_no, subject_no);
                                                                                        recflag = true;
                                                                                        roll_count = 0;
                                                                                        if (samehr_flag == false)
                                                                                        {
                                                                                            samehr_flag = true;
                                                                                        }
                                                                                        //================Srinath 30/5/2014===============
                                                                                        if (batchflag == false)
                                                                                        {
                                                                                            subj_type = "0";
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            subj_type = "1";
                                                                                        }
                                                                                        //================================================
                                                                                        Hashtable has_stud_list = new Hashtable();
                                                                                        if (subj_type != "1" && subj_type != "True" && subj_type != "TRUE" && subj_type != "true")
                                                                                        {
                                                                                            string strquery = "select r.roll_no,a." + date_temp_field + " as attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from registration r ,subjectchooser s,attendance a  where r.roll_no=a.roll_no and s.roll_no=a.roll_no and a.month_year='" + month_year + "' and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and cc=0 " + includedisco + includedeba + " and r.roll_no=s.roll_no and s.semester= " + ddlduration.SelectedItem.ToString() + " " + strsec + " and  subject_no=" + subject_no + " " + strorder + "";
                                                                                            DataSet dsquery = d2.select_method(strquery, hat, "Text");
                                                                                            //if (dsquery.Tables.Count > 0)
                                                                                            //{
                                                                                            for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                                            {
                                                                                                string rollno = dsquery.Tables[0].Rows[i]["roll_no"].ToString().Trim().ToLower();
                                                                                                if (!hatattendance.Contains(rollno))
                                                                                                {
                                                                                                    hatattendance.Add(rollno, dsquery.Tables[0].Rows[i]["attvalue"].ToString());
                                                                                                }
                                                                                                string val = dsquery.Tables[0].Rows[i]["attvalue"].ToString();
                                                                                                //if (val=="0" || val=="" || val==null )
                                                                                                //{
                                                                                                //    if (!dicDate.ContainsKey(CurrentDate) && !dicDate.ContainsValue(temp_hr.ToString()))
                                                                                                //    {
                                                                                                //        dicDate.Add(CurrentDate, temp_hr.ToString());
                                                                                                //    }
                                                                                                //}
                                                                                            }
                                                                                            //}
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            string strquery = "select r.roll_no,a." + date_temp_field + " as attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from  registration r,subjectchooser_New s,laballoc_new l,attendance a  where a.roll_no=s.roll_no and r.roll_no=a.roll_no and a.month_year='" + month_year + "' and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedItem.ToString() + " and cc=0 " + includedisco + includedeba + "  " + rstrsec + " and r.roll_no=s.roll_no and s.subject_no=" + subject_no + " and r.degree_code=l.degree_code and r.batch_year=l.batch_year and l.Semester=r.Current_Semester and s.subject_no =l.subject_no " + splhrsec + " and hour_value=" + temp_hr + "  and day_value='" + strDay + "' and l.subject_no=" + subject_no + " and s.batch=l.stu_batch " + section_lab + " and FromDate ='" + temp_date + "' and l.fdate=s.fromdate " + strorder + "";
                                                                                            DataSet dsquery = d2.select_method(strquery, hat, "Text");
                                                                                            //if (dsquery.Tables.Count > 0)
                                                                                            //{
                                                                                            for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                                            {
                                                                                                string rollno = dsquery.Tables[0].Rows[i]["roll_no"].ToString().Trim().ToLower();
                                                                                                if (!hatattendance.Contains(rollno))
                                                                                                {
                                                                                                    hatattendance.Add(rollno, dsquery.Tables[0].Rows[i]["attvalue"].ToString());
                                                                                                }
                                                                                                string val = dsquery.Tables[0].Rows[i]["attvalue"].ToString();
                                                                                                //if (val == "0" || val == "" || val == null)
                                                                                                //{
                                                                                                //    if (!dicDate.ContainsKey(CurrentDate) && !dicDate.ContainsValue(temp_hr.ToString()))
                                                                                                //    {
                                                                                                //        dicDate.Add(CurrentDate, temp_hr.ToString());
                                                                                                //    }
                                                                                                //}
                                                                                            }
                                                                                            //}
                                                                                        }
                                                                                        //Saran /////
                                                                                        if (hatattendance.Count > 0)
                                                                                        {
                                                                                            for (int i = 2; i < data.Rows.Count; i++)
                                                                                            {
                                                                                                string rollno = data.Rows[i][1].ToString().Trim().ToLower();
                                                                                                if (hatattendance.Contains(rollno.ToString()))
                                                                                                {
                                                                                                    no_stud_flag = true;
                                                                                                    string date = dicstdroll[i];
                                                                                                    string[] spilt = date.Split('-');
                                                                                                    Admission_date = Convert.ToDateTime(spilt[1]);
                                                                                                    string attvalue = GetCorrespondingKey(rollno, hatattendance).ToString();
                                                                                                    string value = Attmark(attvalue.ToString());
                                                                                                    if (temp_date >= Admission_date)
                                                                                                    {
                                                                                                        data.Rows[i][data.Columns.Count - 1] = value;

                                                                                                        if (data.Rows[i][data.Columns.Count - 1] == "HS")//====9/6/12 PRABHA
                                                                                                        {
                                                                                                            if (!has_hs.ContainsKey((data.Columns.Count - 1)))
                                                                                                            {
                                                                                                                has_hs.Add((data.Columns.Count - 1), (data.Columns.Count - 1));
                                                                                                            }
                                                                                                        }
                                                                                                        if ((attvalue.ToString()) != "8")
                                                                                                        {
                                                                                                            if (value != "HS") //'Aruna 21may20123/7/12 PRABHA
                                                                                                            {
                                                                                                                if (!has_attnd_masterset_notconsider.ContainsKey(attvalue.ToString()))//==31/5/12 PRABHA
                                                                                                                {
                                                                                                                    if (has_attnd_masterset.ContainsKey(attvalue.ToString()))
                                                                                                                    {
                                                                                                                        string getval = Convert.ToString(GetCorrespondingKey(attvalue, has_attnd_masterset));
                                                                                                                        if (getval.ToString() == "0")
                                                                                                                        {
                                                                                                                            if (chkonduty.Checked == false)
                                                                                                                            {
                                                                                                                                if (attvalue != "3")
                                                                                                                                {
                                                                                                                                    present_count = Convert.ToInt16(GetCorrespondingKey(data.Rows[i][1], has_load_rollno));
                                                                                                                                    present_count++;
                                                                                                                                    has_load_rollno[data.Rows[i][1]] = present_count;
                                                                                                                                }
                                                                                                                            }
                                                                                                                            else
                                                                                                                            {
                                                                                                                                present_count = Convert.ToInt16(GetCorrespondingKey(data.Rows[i][1], has_load_rollno));
                                                                                                                                present_count++;
                                                                                                                                has_load_rollno[data.Rows[i][1]] = present_count;
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                    if (value != "NE")
                                                                                                                    {
                                                                                                                        present_count = Convert.ToInt16(GetCorrespondingKey(data.Rows[i][1], has_total_attnd_hour));
                                                                                                                        present_count++;
                                                                                                                        has_total_attnd_hour[data.Rows[i][1]] = present_count;
                                                                                                                    }
                                                                                                                    if (attvalue == "3")
                                                                                                                    {
                                                                                                                        onduty = Convert.ToInt16(GetCorrespondingKey(data.Rows[i][1], has_total_onduty_hour));
                                                                                                                        onduty++;
                                                                                                                        has_total_onduty_hour[data.Rows[i][1]] = onduty;
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
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



                                                        samehr_flag = false;
                                                        roll_count = 0;
                                                        present_count = 0;
                                                        if (check_alter == false)
                                                        {
                                                            string split_full_hour_sem1 = single_hour2;
                                                            if (split_full_hour_sem1.Trim() != "")
                                                            {
                                                                temp_has_subj_code.Clear();
                                                                string[] split_full_hour_sem = split_full_hour_sem1.Split(';');
                                                                //==================Srinath 30/5/2014=============================
                                                                Boolean batchflag = false;
                                                                for (int g = 0; g <= split_full_hour_sem.GetUpperBound(0); g++)
                                                                {
                                                                    string[] valhr = split_full_hour_sem[g].ToString().Split('-');
                                                                    if (valhr.GetUpperBound(0) > 1)
                                                                    {
                                                                        string lsub = valhr[0].ToString();
                                                                        if (hatlab.Contains(lsub))
                                                                        {
                                                                            batchflag = true;
                                                                        }
                                                                    }
                                                                }
                                                                //===============================================
                                                                for (int semi_colon = 0; semi_colon <= split_full_hour_sem.GetUpperBound(0); semi_colon++)
                                                                {
                                                                    roll_count = 0;
                                                                    single_hour = split_full_hour_sem[semi_colon].ToString();
                                                                    string[] split_single_hour = single_hour.Split('-');
                                                                    if (split_single_hour.GetUpperBound(0) >= 1)
                                                                    {
                                                                        if (split_single_hour[0].ToString() == subject_no)
                                                                        {
                                                                            if (!temp_has_subj_code.ContainsKey(subject_no))
                                                                            {
                                                                                temp_has_subj_code.Add(subject_no, subject_no);
                                                                                recflag = true;
                                                                                if (samehr_flag == false)
                                                                                {
                                                                                    samehr_flag = true;
                                                                                }
                                                                                Hashtable has_stud_list = new Hashtable();
                                                                                //============Srinath 30/5/2014=================
                                                                                if (batchflag == true)
                                                                                {
                                                                                    subj_type = "1";
                                                                                }
                                                                                else
                                                                                {
                                                                                    subj_type = "0";
                                                                                }
                                                                                //=======================================
                                                                                if (subj_type != "1" && subj_type != "True" && subj_type != "TRUE" && subj_type != "true")
                                                                                {
                                                                                    string strquery = "select r.roll_no,a." + date_temp_field + " as attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from registration r ,subjectchooser s,attendance a where r.roll_no=a.roll_no and a.roll_no=s.roll_no and a.month_year='" + month_year + "' and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and cc=0 " + includedisco + includedeba + " and r.roll_no=s.roll_no and s.semester= " + ddlduration.SelectedItem.ToString() + " " + strsec + " and  subject_no=" + subject_no + " " + strorder + "";
                                                                                    DataSet dsquery = d2.select_method(strquery, hat, "Text");
                                                                                    //if (dsquery.Tables.Count > 0)
                                                                                    //{
                                                                                    for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                                    {
                                                                                        string rollno = dsquery.Tables[0].Rows[i]["Roll_no"].ToString().Trim().ToLower();
                                                                                        if (!hatattendance.Contains(rollno))
                                                                                        {
                                                                                            hatattendance.Add(rollno, dsquery.Tables[0].Rows[i]["attvalue"].ToString());
                                                                                        }
                                                                                        string val = dsquery.Tables[0].Rows[i]["attvalue"].ToString();
                                                                                        //if (val == "0" || val == "" || val == null)
                                                                                        //{
                                                                                        //    if (!dicDate.ContainsKey(CurrentDate) && !dicDate.ContainsValue(temp_hr.ToString()))
                                                                                        //    {
                                                                                        //        dicDate.Add(CurrentDate, temp_hr.ToString());
                                                                                        //    }
                                                                                        //}
                                                                                    }
                                                                                    //}
                                                                                }
                                                                                else
                                                                                {
                                                                                    string strquery = "select r.roll_no,a." + date_temp_field + " AS attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from  registration r,subjectchooser s,laballoc l,attendance a  where r.roll_no=a.roll_no and a.roll_no=s.roll_no and a.month_year='" + month_year + "' and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedItem.ToString() + " and cc=0 " + includedisco + includedeba + "  " + rstrsec + " and r.roll_no=s.roll_no and s.subject_no=" + subject_no + " and l.Semester=r.Current_Semester and r.degree_code=l.degree_code and r.batch_year=l.batch_year  and s.subject_no =l.subject_no " + splhrsec + " and hour_value=" + temp_hr + "  and day_value='" + strDay + "' and l.subject_no=" + subject_no + " " + section_lab + " and s.batch=l.stu_batch " + strorder + "";
                                                                                    DataSet dsquery = d2.select_method(strquery, hat, "Text");
                                                                                    //if (dsquery.Tables.Count > 0)
                                                                                    //{
                                                                                    for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                                    {
                                                                                        string rollno = dsquery.Tables[0].Rows[i]["Roll_no"].ToString().Trim().ToLower();
                                                                                        if (!hatattendance.Contains(rollno.ToString()))
                                                                                        {
                                                                                            hatattendance.Add(rollno.ToString(), dsquery.Tables[0].Rows[i]["attvalue"].ToString());
                                                                                        }
                                                                                        string val = dsquery.Tables[0].Rows[i]["attvalue"].ToString();
                                                                                        //if (val == "0" || val == "" || val == null)
                                                                                        //{
                                                                                        //    if (!dicDate.ContainsKey(CurrentDate) && !dicDate.ContainsValue(temp_hr.ToString()))
                                                                                        //    {
                                                                                        //        dicDate.Add(CurrentDate, temp_hr.ToString());
                                                                                        //    }
                                                                                        //}
                                                                                    }
                                                                                    //}
                                                                                }
                                                                                if (hatattendance.Count > 0)
                                                                                {
                                                                                    for (int i = 2; i < data.Rows.Count; i++)
                                                                                    {
                                                                                        string rollno = data.Rows[i][1].ToString().ToLower();
                                                                                        if (hatattendance.Contains(rollno.ToString()))
                                                                                        {
                                                                                            no_stud_flag = true;
                                                                                            string date = dicstdroll[i];
                                                                                            string[] spilt = date.Split('-');
                                                                                            Admission_date = Convert.ToDateTime(spilt[1]);


                                                                                            string attvalue = GetCorrespondingKey(rollno, hatattendance).ToString();
                                                                                            string value = Attmark(attvalue.ToString());
                                                                                            if (temp_date >= Admission_date)
                                                                                            {
                                                                                                data.Rows[i][data.Columns.Count - 1] = value;

                                                                                                if (data.Rows[i][data.Columns.Count - 1] == "HS")//====9/6/12 PRABHA
                                                                                                {
                                                                                                    if (!has_hs.ContainsKey((data.Columns.Count - 1)))
                                                                                                    {
                                                                                                        has_hs.Add((data.Columns.Count - 1), (data.Columns.Count - 1));
                                                                                                    }
                                                                                                }
                                                                                                if ((attvalue.ToString()) != "8")
                                                                                                {
                                                                                                    if (value != "HS") //'Aruna 21may20123/7/12 PRABHA
                                                                                                    {
                                                                                                        if (!has_attnd_masterset_notconsider.ContainsKey(attvalue.ToString()))//==31/5/12 PRABHA
                                                                                                        {
                                                                                                            if (has_attnd_masterset.ContainsKey(attvalue.ToString()))
                                                                                                            {
                                                                                                                string getval = Convert.ToString(GetCorrespondingKey(attvalue, has_attnd_masterset));
                                                                                                                if (getval.ToString() == "0")
                                                                                                                {
                                                                                                                    if (chkonduty.Checked == false)
                                                                                                                    {
                                                                                                                        if (attvalue.ToString().Trim() != "3")
                                                                                                                        {
                                                                                                                            present_count = Convert.ToInt16(GetCorrespondingKey(data.Rows[i][1], has_load_rollno));
                                                                                                                            present_count++;
                                                                                                                            has_load_rollno[data.Rows[i][1]] = present_count;
                                                                                                                        }
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        present_count = Convert.ToInt16(GetCorrespondingKey(data.Rows[i][1], has_load_rollno));
                                                                                                                        present_count++;
                                                                                                                        has_load_rollno[data.Rows[i][1]] = present_count;
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                            if (value != "NE")
                                                                                                            {
                                                                                                                present_count = Convert.ToInt16(GetCorrespondingKey(data.Rows[i][1], has_total_attnd_hour));
                                                                                                                present_count++;
                                                                                                                has_total_attnd_hour[data.Rows[i][1]] = present_count;
                                                                                                            }
                                                                                                            if (attvalue == "3")
                                                                                                            {
                                                                                                                onduty = Convert.ToInt16(GetCorrespondingKey(data.Rows[i][1], has_total_onduty_hour));
                                                                                                                onduty++;
                                                                                                                has_total_onduty_hour[data.Rows[i][1]] = onduty;
                                                                                                            }
                                                                                                            //modified by raj kumar
                                                                                                            if (value == "NE")
                                                                                                            {
                                                                                                                if (!dicDate.ContainsKey(CurrentDate) && !dicDate.ContainsValue(temp_hr.ToString()))
                                                                                                                {
                                                                                                                    dicDate.Add(CurrentDate, temp_hr.ToString());
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        check_alter = false;
                                                    }
                                                }
                                            }
                                        }
                                    }//Added by srinath 5/9/2014 for day order change
                                    temp_date = temp_date.AddDays(1);
                                }
                                if (dicDate.Count > 0)
                                {
                                    string valDate = string.Empty;
                                    string valTime = string.Empty;
                                    string Noresult = string.Empty;
                                    foreach (KeyValuePair<string, string> dt in dicDate)
                                    {
                                        valDate = dt.Key;
                                        valTime = dt.Value;
                                        Noresult = Noresult + "Date: " + " " + valDate + " " + "Hour: " + "" + valTime + " ";
                                    }
                                    //added by Prabha on Dec 12 2017  Rajkumar 12/22/2017
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
                                    //usercode = Session["usercode"].ToString().Trim();
                                    string alertRights = dirAcc.selectScalarString("select value from Master_Settings where settings='AlertMessageForAttendance' " + qryUserCodeOrGroupCode + "");
                                    if (alertRights == "1" && !string.IsNullOrEmpty(Noresult))
                                    {
                                        Showgrid.Visible = false;
                                        //string Noresult = UnmarkHours;
                                        lblAlertMsg.Visible = true;
                                        lblAlertMsg.Text = Noresult + " " + " Attendance not Found";
                                        divPopAlert.Visible = true;

                                        btnprintmaster.Visible = false;
                                        btnPrint.Visible = false;
                                        txtexcelname.Visible = false;
                                        lblrptname.Visible = false;
                                        btnxl.Visible = false;
                                    }
                                    //commented by prabha on jan 18 2018
                                    //return;
                                }
                                colcnt = colcnt + 4;
                                if (recflag == true || spl_hr_flag == true)
                                {
                                    double attnd_hr = 0, tot_hr = 0, ondutyvalue = 0;
                                    for (int row_cnt = 2; row_cnt < data.Rows.Count; row_cnt++)
                                    {
                                        less70perc = 0;
                                        attnd_hr = 0;
                                        tot_hr = 0;
                                        sub_prc = 0;
                                        ondutyvalue = 0;
                                        string roll_number = data.Rows[row_cnt][1].ToString();
                                        if (has_load_rollno.Contains(roll_number))
                                        {
                                            attnd_hr = Convert.ToDouble(GetCorrespondingKey(roll_number, has_load_rollno));
                                        }
                                        if (has_total_attnd_hour.Contains(roll_number))
                                        {
                                            tot_hr = Convert.ToDouble(GetCorrespondingKey(roll_number, has_total_attnd_hour));
                                        }
                                        if (has_total_onduty_hour.Contains(roll_number))
                                        {
                                            ondutyvalue = Convert.ToDouble(GetCorrespondingKey(roll_number, has_total_onduty_hour));
                                        }
                                        if (chkonduty.Checked == true)
                                        {
                                            sub_prc = Math.Round(((attnd_hr / tot_hr) * 100), 2);
                                        }
                                        else
                                        {
                                            double tot = attnd_hr + ondutyvalue;
                                            sub_prc = Math.Round(((tot / tot_hr) * 100), 2);
                                        }
                                        less70perc = 0;
                                        if (sub_prc < 70)
                                        {
                                            if (stud_perccnt.ContainsKey(data.Rows[row_cnt][1].ToString()))
                                            {
                                                less70perc = Convert.ToInt16(GetCorrespondingKey(data.Rows[row_cnt][1].ToString(), stud_perccnt));
                                                less70perc++;
                                                stud_perccnt[data.Rows[row_cnt][1].ToString()] = less70perc;
                                            }
                                            else
                                            {
                                                stud_perccnt.Add(data.Rows[row_cnt][1].ToString(), 1);
                                            }
                                        }
                                        if (attnd_hr == 0 && tot_hr == 0)
                                        {
                                            data.Rows[row_cnt][colcnt - 1] = "-";

                                        }
                                        else
                                        {
                                            no_stud_flag = true;
                                            int isub = 0;
                                            if (roll_number.ToString() != "")
                                            {
                                                isub = Convert.ToInt16(da.GetFunction("select isnull(count(*),0) from subjectchooser where roll_no='" + roll_number.ToString() + "' and subject_no=" + subno_val.Trim().ToString() + ""));
                                                if (isub > 0)
                                                {
                                                    data.Rows[row_cnt][colcnt - 5] = tot_hr.ToString();
                                                    data.Rows[row_cnt][colcnt - 4] = attnd_hr.ToString();

                                                    Double absent = 0;
                                                    if (chkonduty.Checked == true)
                                                    {
                                                        absent = tot_hr - attnd_hr;
                                                    }
                                                    else
                                                    {
                                                        absent = tot_hr - (attnd_hr + ondutyvalue);
                                                    }
                                                    data.Rows[row_cnt][colcnt - 2] = absent.ToString();
                                                    data.Rows[row_cnt][colcnt - 3] = ondutyvalue.ToString();
                                                    data.Rows[row_cnt][colcnt - 1] = sub_prc.ToString();


                                                }
                                                else
                                                {
                                                    data.Rows[row_cnt][colcnt - 1] = "-";


                                                }
                                            }
                                            else
                                            {
                                                data.Rows[row_cnt][colcnt - 1] = "-";
                                            }
                                        }
                                        data.Rows[row_cnt][data.Columns.Count - 1] = less70perc.ToString();

                                    }
                                    int roll_no_cnt = 0;
                                    for (int roll_no_set = 0; roll_no_set < data.Rows.Count; roll_no_set++)
                                    {

                                        //if (subject_report.Sheets[0].Rows[roll_no_set].Visible == true)
                                        //{
                                        //    roll_no_cnt++;
                                        //    //  subject_report.Sheets[0].Cells[roll_no_set, 0].Text = roll_no_cnt.ToString();
                                        //}
                                    }
                                }
                                else
                                {
                                    Showgrid.Visible = false;
                                    btnxl.Visible = false;
                                    Printcontrol.Visible = false;
                                    btnprintmaster.Visible = false;
                                    btnPrint.Visible = false;
                                    txtexcelname.Visible = false;
                                    lblrptname.Visible = false;
                                    errmsg.Visible = true;
                                    errmsg.Text = "No Record(s) Found";
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                //Added by Venkat 4/9/2014============================Start
                for (int row_cnt = 2; row_cnt < data.Rows.Count; row_cnt++)
                {
                    Double tothr = 0;
                    Double totat = 0;
                    for (int c = 5; c < data.Columns.Count - 4; c++)
                    {
                        if (dicstdsubj.ContainsKey(c))
                        {
                            string subjcode = dicstdsubj[c];
                            string[] split = subjcode.Split('$');
                            string sub = Convert.ToString(split[1]);
                            if (sub != "")
                            {

                                string val = data.Rows[row_cnt][c - 1].ToString();
                                if (val != "-" && val != "")
                                {
                                    tothr = tothr + Convert.ToDouble(val);
                                }
                                if (chkonduty.Checked == true)
                                {
                                    string val1 = data.Rows[row_cnt][c].ToString();
                                    if (val1 != "-" && val1 != "")
                                    {
                                        totat = totat + Convert.ToDouble(val1);
                                    }
                                }
                                else
                                {
                                    string val1 = data.Rows[row_cnt][c].ToString();
                                    string val3 = data.Rows[row_cnt][c + 1].ToString();
                                    if (val1 == "-" || val1 == "")
                                    {
                                        val1 = "0";
                                    }
                                    if (val3 == "-" || val3 == "")
                                    {
                                        val3 = "0";
                                    }
                                    double val4 = Convert.ToDouble(val1) + Convert.ToDouble(val3);
                                    string finval = val4.ToString();
                                    if (finval != "-" && finval != "")
                                    {
                                        totat = totat + Convert.ToDouble(val4);
                                    }
                                }
                            }
                        }
                    }
                    data.Rows[row_cnt][data.Columns.Count - 4] = tothr.ToString();
                    data.Rows[row_cnt][data.Columns.Count - 3] = totat.ToString();

                    Double percentage = totat / tothr * 100;
                    percentage = Math.Round(percentage, 2);
                    if (percentage > 100)
                    {
                        percentage = 100;
                    }
                    data.Rows[row_cnt][data.Columns.Count - 2] = percentage.ToString();


                }
                //=============================================End
                if (holiflag == true)
                {
                    if (no_stud_flag == false)
                    {
                        Showgrid.Visible = false;
                        btnxl.Visible = false;
                        Printcontrol.Visible = false;
                        btnprintmaster.Visible = false;
                        btnPrint.Visible = false;
                        txtexcelname.Visible = false;
                        lblrptname.Visible = false;
                        errmsg.Visible = true;
                        errmsg.Text = "Student(s) Not Available Or Attendance Cant Be Marked";
                    }
                    else
                    {
                        errmsg.Visible = false;
                    }
                }
                else
                {
                    Showgrid.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    btnPrint.Visible = false;
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    errmsg.Visible = true;
                    errmsg.Text = "Holiday";
                }
            }

        }
        catch (Exception ex)
        {
        }
    }

    public string Attmark(string Attstr_mark)
    {
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
        else if (Attstr_mark == "6")
        {
            Att_mark = "NSS";
        }
        else if (Attstr_mark == "7")
        {
            Att_mark = "H";
        }
        else if (Attstr_mark == "8")
        {
            Att_mark = "NJ";
        }
        else if (Attstr_mark == "9")
        {
            Att_mark = "S";
        }
        else if (Attstr_mark == "10")
        {
            Att_mark = "L";
        }
        else if (Attstr_mark == "11")
        {
            Att_mark = "NCC";
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
        else
        {
            Att_mark = "NE";
        }
        //return Convert.ToInt32(Att_mark);
        return Att_mark;
    }

    public void getspecial_hr()
    {

        try
        {
            //added By Srinath =========Start
            string splsec = "";
            if (ddlsec.SelectedValue.ToString() != "" && ddlsec.SelectedValue.ToString() != "-1")
            {
                splsec = " and sm.sections='" + ddlsec.SelectedItem.ToString() + "'";
            }
            string hrdetno = "";
            if (ht_sphr.Contains(Convert.ToString(temp_date)))
            {
                hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(temp_date), ht_sphr));
            }
            if (hrdetno != "")
            {
                //====End
                //con_splhr_query_master.Close();
                //con_splhr_query_master.Open();
                DataSet ds_splhr_query_master = new DataSet();
                //string splhr_query_master = "select r.roll_no , attendance from specialhr_attendance sa,registration r where hrdet_no in(select hrdet_no from specialhr_details where hrentry_no=(select hrentry_no from  specialhr_master where batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and date='" + temp_date + "') and subject_no='" + subject_no + "') and r.roll_no=sa.roll_no and batch_year=" + ddlbatch.SelectedValue.ToString() + " and current_semester=" + ddlduration.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + "  and (CC = 0)  AND (DelFlag = 0)  AND (Exam_Flag <> 'debar')  order by r.roll_no asc";
                //string splhr_query_master = "select spa.roll_no,spa.attendance from  specialhr_attendance spa,specialhr_details spd where spa.hrdet_no=spd.hrdet_no and spd.subject_no='" + subject_no + "'  and spd.hrdet_no in(" + hrdetno + ") order by spa.roll_no asc";
                string splhr_query_master = "select spa.roll_no,spa.attendance from  specialhr_attendance spa,specialhr_details spd,specialhr_master sm where spd.hrentry_no=sm.hrentry_no  and spa.hrdet_no=spd.hrdet_no  and spd.subject_no='" + subject_no + "'  and spd.hrdet_no in(" + hrdetno + ") " + splsec + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + "  order by spa.roll_no asc";
                ds_splhr_query_master = da.select_method(splhr_query_master, hat, "Text");
                //SqlDataReader dr_splhr_query_master;
                //cmd = new SqlCommand(splhr_query_master, con_splhr_query_master);
                //dr_splhr_query_master = cmd.ExecuteReader();
                for (int i = 0; i < ds_splhr_query_master.Tables[0].Rows.Count; i++)
                {
                    // while (dr_splhr_query_master.Read())
                    //{
                    // if (dr_splhr_query_master.HasRows)
                    {
                        spl_hr_flag = true;
                        no_stud_flag = true;
                        //{
                        //gotolable:
                        //if (roll_count < subject_report.Sheets[0].RowCount)
                        //{
                        recflag = true;
                        //if (subject_report.Sheets[0].Cells[roll_count, 1].Text.Trim() == dr_splhr_query_master[0].ToString().Trim())
                        //{
                        if (hatsplhrattendance.Contains(ds_splhr_query_master.Tables[0].Rows[i][0].ToString()))
                        {
                            string rollNo = ds_splhr_query_master.Tables[0].Rows[i][0].ToString();
                            roll_count = Convert.ToInt32(GetCorrespondingKey(ds_splhr_query_master.Tables[0].Rows[i][0].ToString(), hatsplhrattendance));
                            roll_count += 2;
                            if ((ds_splhr_query_master.Tables[0].Rows[i][1].ToString()) != "8")
                            {
                                if (Attmark(ds_splhr_query_master.Tables[0].Rows[i][1].ToString()) != "HS")
                                {
                                    if (has_attnd_masterset.ContainsKey((ds_splhr_query_master.Tables[0].Rows[i][1].ToString())))
                                    {
                                        int att = Convert.ToInt32(GetCorrespondingKey(ds_splhr_query_master.Tables[0].Rows[i][1].ToString(), has_attnd_masterset));
                                        if (att == 0)
                                        {
                                            if (has_load_rollno.Contains(ds_splhr_query_master.Tables[0].Rows[i][0].ToString()))
                                            {
                                                present_count = Convert.ToInt16(has_load_rollno[ds_splhr_query_master.Tables[0].Rows[i][0].ToString()].ToString());
                                                present_count++;
                                                has_load_rollno[data.Rows[roll_count][1]] = present_count;
                                            }
                                            else
                                            {

                                                if (chkonduty.Checked == false)
                                                {
                                                    if (Convert.ToString(ds_splhr_query_master.Tables[0].Rows[i][1]).Trim() != "3")
                                                    {
                                                        present_count = Convert.ToInt16(GetCorrespondingKey(data.Rows[roll_count][1], has_load_rollno));
                                                        present_count++;
                                                        has_load_rollno[data.Rows[roll_count][1]] = present_count;
                                                    }
                                                }
                                                else
                                                {
                                                    present_count = Convert.ToInt16(GetCorrespondingKey(data.Rows[roll_count][1], has_load_rollno));
                                                    present_count++;
                                                    has_load_rollno[data.Rows[roll_count][1]] = present_count;
                                                }


                                                //has_load_rollno.Add(data.Rows[roll_count][1], ds_splhr_query_master.Tables[0].Rows[i][1].ToString());
                                            }
                                        }
                                    }
                                    if (Attmark(ds_splhr_query_master.Tables[0].Rows[i][1].ToString()) != "NE")
                                    {
                                        if (has_total_attnd_hour.Contains(ds_splhr_query_master.Tables[0].Rows[i][0].ToString()))
                                        {
                                            present_count = Convert.ToInt16(has_total_attnd_hour[ds_splhr_query_master.Tables[0].Rows[i][0].ToString()].ToString());
                                            present_count++;
                                            has_total_attnd_hour[data.Rows[roll_count][1]] = present_count;
                                        }
                                        else
                                        {
                                            has_total_attnd_hour.Add(data.Rows[roll_count][1], "1");
                                        }
                                    }
                                }
                            }
                        }
                        //Modified By srinath 8/6/2013=======
                        //else if (subject_report.Sheets[0].Cells[roll_count, 1].Text.Trim() == dr_splhr_query_master[0].ToString().Trim())
                        ////else if (subject_report.Sheets[0].Cells[roll_count - 1, 1].Text.Trim() == dr_splhr_query_master[0].ToString().Trim())
                        ////===============
                        //{
                        //    //subject_report.Sheets[0].Cells[roll_count, (subject_report.Sheets[0].ColumnCount - 1)].Text = Attmark(dr_splhr_query_master[1].ToString());
                        //    // subject_report.Sheets[0].Rows[roll_count].Visible = true;
                        //    if ((dr_splhr_query_master[1].ToString()) != "8")
                        //    {
                        //        if (Attmark(dr_splhr_query_master[1].ToString()) != "HS")
                        //        {
                        //            if (has_attnd_masterset.ContainsKey((dr_splhr_query_master[1].ToString())))
                        //            {
                        //                 int att=Convert.ToInt32(GetCorrespondingKey(dr_splhr_query_master[1].ToString(),has_attnd_masterset));
                        //                 if (att == 0)
                        //                 {
                        //                     if (has_load_rollno.Contains(dr_splhr_query_master[0].ToString()))
                        //                     {
                        //                         //   present_count = Convert.ToInt16(GetCorrespondingKey(Attmark(dr_splhr_query_master[0].ToString()).ToString(), has_load_rollno));
                        //                         present_count = Convert.ToInt16(has_load_rollno[dr_splhr_query_master[0].ToString()].ToString());
                        //                         present_count++;
                        //                         \\[subject_report.Sheets[0].Cells[roll_count - 1, 1].Text] = present_count;
                        //                     }
                        //                     else
                        //                     {
                        //                         has_load_rollno.Add(subject_report.Sheets[0].Cells[roll_count, 1].Text, dr_splhr_query_master[1].ToString());
                        //                     }
                        //                 }
                        //            }
                        //            if (Attmark(dr_splhr_query_master[1].ToString()) != "NE")
                        //            {
                        //                if (has_total_attnd_hour.Contains(dr_splhr_query_master[0].ToString()))
                        //                {
                        //                    present_count = Convert.ToInt16(has_total_attnd_hour[dr_splhr_query_master[0].ToString()].ToString());
                        //                    present_count++;
                        //                    has_total_attnd_hour[subject_report.Sheets[0].Cells[roll_count - 1, 1].Text] = present_count;
                        //                }
                        //                else
                        //                {
                        //                    has_total_attnd_hour.Add(subject_report.Sheets[0].Cells[roll_count, 1].Text, "1");
                        //                }
                        //            }
                        //        }
                        //    }
                        //    roll_count = roll_count - 1;
                        //}
                        //else
                        //{
                        //    // subject_report.Sheets[0].Cells[roll_count, (subject_report.Sheets[0].ColumnCount - 1)].Text = "-";
                        //    roll_count++;
                        //    if (roll_count < subject_report.Sheets[0].RowCount)
                        //    {
                        //        goto gotolable;
                        //    }
                        //    else
                        //    {
                        //        break;
                        //    }
                        //}
                        //}
                        //else
                        //{
                        //    break;
                        //}
                        //}
                        // roll_count++;
                    }
                }
            }//added By Srinath 22/2/2013
        }
        catch
        {
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        // Session["column_header_row_count"] = Convert.ToString(subject_report.ColumnHeader.RowCount);
        string sections = ddlsec.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
        {
            sections = "";
        }
        else
        {
            sections = "- Sec-" + sections;
        }
        string ss = null;
        string degreedetails = "Consolidate Attendance Details- Subject Wise Report" + '@' + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '-' + ddlbranch.SelectedItem.ToString() + '-' + "Sem-" + ddlduration.SelectedItem.ToString() + sections + '@' + "Period :" + txtFromDate.Text.ToString() + " To " + txtToDate.Text.ToString();
        string pagename = "consolidate_subjwise_attndreport.aspx";
        Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
        Printcontrol.Visible = true;
    }

    protected void chksorthead_CheckedChanged(object sender, EventArgs e)
    {
        if (Showgrid.Visible == true)
        {
            if (chksorthead.Checked == true)
            {
                string sqlstr = "Select S.Subject_Code,s.acronym, S.Subject_no, S.max_int_marks,SS.Subject_Type,s.acronym,s.subject_name  from Subject as s,Sub_Sem as ss ,Syllabus_Master as SMas where SMas.Syll_Code = S.Syll_Code and SMas.Syll_Code = SS.Syll_Code and SS.Syll_Code = S.Syll_Code and S.SubType_no = SS.Subtype_no and SS.Promote_Count = 1 and SMas.Degree_Code =" + ddlbranch.SelectedValue.ToString() + " and SMas.Batch_Year =" + ddlbatch.SelectedValue.ToString() + " and SMas.Semester = " + ddlduration.SelectedValue.ToString() + " order by S.Subject_no, SS.SubType_No ";
                subds.Reset();
                subds.Dispose();
                subds = da.select_method(sqlstr, hat, "Text");
                if (subds.Tables[0].Rows.Count != 0)
                {
                    int noofsubjcet = data.Columns.Count;
                    int sub = 0;
                    for (int i = 9; i < noofsubjcet; i = i + 5)
                    {
                        if (i > 9)
                            sub++;

                        System.Text.StringBuilder conhr = new System.Text.StringBuilder();

                        conhr = new System.Text.StringBuilder("CON");
                        ResetTableColumn(data, conhr, i - 4);

                        conhr = new System.Text.StringBuilder("ATT");
                        ResetTableColumn(data, conhr, i - 3);

                        conhr = new System.Text.StringBuilder("OD");
                        ResetTableColumn(data, conhr, i - 2);

                        conhr = new System.Text.StringBuilder("AB");
                        ResetTableColumn(data, conhr, i - 1);

                        //subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].SheetCorner.RowCount - 2), (i - 4)].Text = subds.Tables[0].Rows[sub]["acronym"].ToString();
                        //subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].SheetCorner.RowCount - 1), (i - 4)].Text = "CON";
                        //subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].SheetCorner.RowCount - 1), (i - 3)].Text = "ATT";
                        //subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].SheetCorner.RowCount - 1), (i - 2)].Text = "OD";
                        //subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].SheetCorner.RowCount - 1), (i - 1)].Text = "AB";
                        //subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].SheetCorner.RowCount - 1), i].Text = "% Att";
                        //subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].SheetCorner.RowCount - 2), (i - 4)].Note = subds.Tables[0].Rows[sub]["Subject_no"].ToString();
                    }

                }
            }
            else
            {
                string sqlstr = "Select S.Subject_Code,s.acronym, S.Subject_no, S.max_int_marks,SS.Subject_Type,s.acronym,s.subject_name  from Subject as s,Sub_Sem as ss ,Syllabus_Master as SMas where SMas.Syll_Code = S.Syll_Code and SMas.Syll_Code = SS.Syll_Code and SS.Syll_Code = S.Syll_Code and S.SubType_no = SS.Subtype_no and SS.Promote_Count = 1 and SMas.Degree_Code =" + ddlbranch.SelectedValue.ToString() + " and SMas.Batch_Year =" + ddlbatch.SelectedValue.ToString() + " and SMas.Semester = " + ddlduration.SelectedValue.ToString() + " order by S.Subject_no, SS.SubType_No ";
                subds.Reset();
                subds.Dispose();
                subds = da.select_method(sqlstr, hat, "Text");
                if (subds.Tables[0].Rows.Count != 0)
                {
                    int noofsubjcet = data.Columns.Count;
                    int sub = 0;
                    for (int i = 9; i < noofsubjcet; i = i + 5)
                    {
                        if (i > 9)
                            sub++;

                        //    subject_report.Sheets[0].Columns[subject_report.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        //    subject_report.Sheets[0].ColumnHeaderSpanModel.Add((subject_report.Sheets[0].SheetCorner.RowCount - 2), (i - 4), 1, 5);
                        //    subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].SheetCorner.RowCount - 2), (i - 4)].Text = subds.Tables[0].Rows[sub]["Subject_Code"].ToString() + '-' + subds.Tables[0].Rows[sub]["subject_name"].ToString();
                        //    subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].SheetCorner.RowCount - 1), (i - 4)].Text = "Conducted Periods";
                        //    subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].SheetCorner.RowCount - 1), (i - 3)].Text = "Attended Periods";
                        //    subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].SheetCorner.RowCount - 1), (i - 2)].Text = "On Duty Periods";
                        //    subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].SheetCorner.RowCount - 1), (i - 1)].Text = "Absent Periods";
                        //    subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].SheetCorner.RowCount - 1), i].Text = "% Att";
                        //    subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].SheetCorner.RowCount - 2), (i - 4)].Note = subds.Tables[0].Rows[sub]["Subject_no"].ToString();
                    }
                    //subject_report.Sheets[0].ColumnHeaderSpanModel.Add((subject_report.Sheets[0].SheetCorner.RowCount - 2), (subject_report.Sheets[0].ColumnCount - 1), 2, 1);
                    //subject_report.Sheets[0].ColumnHeader.Cells[(subject_report.Sheets[0].SheetCorner.RowCount - 2), (subject_report.Sheets[0].ColumnCount - 1)].Text = "Number of Courses in which Student has less than 70 % Attendance";
                }
            }
        }

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

    private static void ResetTableColumn(DataTable resultsTable, StringBuilder ColumnName, int colindex)
    {
        try
        {

            DataColumn tableCol = new DataColumn(ColumnName.ToString());
            resultsTable.Columns[colindex].ColumnName = ColumnName.ToString();
            resultsTable.AcceptChanges();

        }
        catch (System.Data.DuplicateNameException)
        {

            ColumnName.Append(" ");
            resultsTable.Columns[colindex].ColumnName = ColumnName.ToString();
            resultsTable.AcceptChanges();
            //AddTableColumn(resultsTable, ColumnName);
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
                for (int j = columncnt; j < data.Columns.Count; j++)
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
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
        spReportName.InnerHtml = "Consolidate Attendance Details- Subject Wise Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }
}

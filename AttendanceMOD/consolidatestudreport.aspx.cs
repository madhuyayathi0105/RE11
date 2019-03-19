
//==========MANIPRABHA A.
using System;//=====================================modified on 10/1/12, 13/2/12, 16/2/12-textcelltype, 29/2/12(border width,XL)
//----------21/3/12(pnl visible),23/3/12(wrong var usage for to_month_year, month on dates,header span ),23/3/12(complete consoli halfday holiday)
//============30/3/12(len(r_no)), 2/4/12(header setting), 2/6/12(if ihof and IIhof=4 condition),
//==============12/6/12(include spl hr, p_m_s_n, try in p_l, ISO code),15/6/12(hide column),11/7/12(05->06 in header),20/7/12(txt cell type, no logo->celltype)
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

public partial class NewAttendance : System.Web.UI.Page
{


    [Serializable()]
    public class MyImg : ImageCellType
    {

        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  

            img.Width = Unit.Percentage(80);
            return img;
        }
    }

    #region var declaration

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

    SqlCommand cmd;
    SqlCommand cmd1a;
    SqlCommand cmd3a;
    SqlCommand cmd4a;

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
    int setfp;
    double dif_date1 = 0;
    double dif_date = 0;
    double Ihof, IIhof;
    string pp;
    string regularflag = "";
    string genderflag = "";
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    string value_holi_status = "";
    string[] split_holiday_status = new string[1000];
    int conduct_hour_new = 0;
    string new_header_string_index;
    string isonumber = "";
    DataSet ds_attnd_pts = new DataSet();
    Hashtable hath = new Hashtable();
    Hashtable hat = new Hashtable();

    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();

    int countofp = 0;
    int countofa = 0;
    int conduct_hour_new_fal = 0, conduct_hour_new_true = 0;
    //=================12/6/12 PRABHA
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int notconsider_value = 0;
    bool splhr_flag = false;
    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0;
    double per_leave_fals = 0;
    int per_abshrs_spl_true = 0, tot_per_hrs_spl_true = 0, tot_conduct_hr_spl_true = 0, tot_ondu_spl_true = 0;
    double per_leave_true = 0;
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
    //---------------------------
    int ddiff;
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
    int count;
    int next = 0;
    int minpresII = 0;
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
    int tot_per_hrs, per_tot_per_hrs, cum_per_tot_per, tot_wok_hrs;
    double per_con_hrs, cum_con_hrs;
    double njhr, njdate, per_njhr, per_njdate, cum_njhr, cum_njdate;
    double per_per_hrs, cum_per_perhrs;
    double tot_ondu, per_tot_ondu, cum_tot_ondu;
    string usercode = "", collegecode = "", singleuser = "", group_user = "";
    int holiday;
    string split_holiday_status_1 = "", split_holiday_status_2 = "";
    string mon1 = "", mon2 = "", mon3 = "";
    int mon_cnt = 0, start_col = 5;


    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0, cum_per_workingdays1 = 0;

    #endregion

    //end
    //public DataSet Bind_Degree(string college_code, string user_code)
    //{
    //    SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    //    dcon.Close();
    //    dcon.Open();
    //    SqlCommand cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
    //    SqlDataAdapter da = new SqlDataAdapter(cmd);
    //    DataSet ds = new DataSet();
    //    da.Fill(ds);
    //    return ds;
    //}
    //public DataSet Bind_Dept(string degree_code, string college_code, string user_code)
    //{
    //    SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    //    dcon.Close();
    //    dcon.Open();
    //    SqlCommand cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
    //    SqlDataAdapter da = new SqlDataAdapter(cmd);
    //    DataSet ds = new DataSet();
    //    da.Fill(ds);
    //    return ds;
    //}
    static string grouporusercode = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        norecordlbl.Visible = false;
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
            FarPoint.Web.Spread.NamedStyle fontblue = new FarPoint.Web.Spread.NamedStyle("blue");
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread1.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;

            FpSpread1.ActiveSheetView.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].SheetName = " ";

            FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
            style1.Font.Size = 12;
            style1.Font.Bold = true;
            style1.HorizontalAlign = HorizontalAlign.Center;
            style1.ForeColor = Color.Black;
            style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpSpread1.Sheets[0].SheetCorner.Columns[0].Visible = false;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].AllowTableCorner = true;
            FpSpread1.Sheets[0].DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.CommandBar.Visible = false;
            // FpSpread1.Sheets[0].DefaultColumnWidth = 80;

            FpSpread1.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            //  Panel3.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            cumfromlbl.Visible = false;
            cumfromtxt.Visible = false;
            cumtolbl.Visible = false;
            cumtotxt.Visible = false;
            norecordlbl.Visible = false;
            ne.Visible = false;
            pointchk.Visible = false;
            //  pagesetpanel.Visible = false;
            //  pageddltxt.Visible = false;
            //   errmsg.Visible = false;
            tolbl.Visible = false;
            frmlbl.Visible = false;
            tofromlbl.Visible = false;
            lblpages.Visible = false;
            ddlpage.Visible = false;
            //'----------------------------------
            Buttontotal.Visible = false;
            //   lblrecord.Visible = false;
            DropDownListpage.Visible = false;
            ddlpagelbl.Visible = false;
            pageddltxt.Visible = false;
            pgsearch_lbl.Visible = false;
            pagesearch_txt.Visible = false;
            errmsg.Visible = false;

            //------------initial date picker value
            string dt = DateTime.Today.ToShortDateString();
            string[] dsplit = dt.Split(new Char[] { '/' });
            txtToDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            txtFromDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            cumfromtxt.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            cumtotxt.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
            Session["curr_year"] = dsplit[2].ToString();
            //----------------------------------------------0n 6/4/12 PRABHA
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
                    txtToDate.Enabled = true;
                    txtFromDate.Enabled = true;
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
                    txtToDate.Enabled = false;
                    txtFromDate.Enabled = false;
                    cumcheck.Enabled = false;
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

                        //ddlbatch.SelectedIndex + "," + ddldegree.SelectedIndex + "," + ddlbranch.SelectedIndex + "," + sem_index + "," + sec_index + "," + txtFromDate.Text + "," + txtToDate.Text + "," + cumcheck.Checked + "," + cumfromtxt.Text + "," + cumtotxt.Text + "," + pointchk.Checked;
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
                            txtToDate.Enabled = true;
                            txtFromDate.Enabled = true;
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

                            cumcheck.Checked = Convert.ToBoolean(string_session_values[7].ToString());
                            cumfromtxt.Text = string_session_values[8].ToString();

                            cumtotxt.Text = string_session_values[9].ToString();

                            pointchk.Checked = Convert.ToBoolean(string_session_values[10].ToString());
                            //   btnGo_Click(sender, e);
                            print_btngo();
                            if (check_col_count_flag == true)
                            {
                                view_header_setting();
                            }
                            load_ddlpage();
                        }
                        else
                        {
                            ddldegree.Enabled = false;
                            ddlbranch.Enabled = false;
                            ddlduration.Enabled = false;
                            ddlsec.Enabled = false;
                            btnGo.Enabled = false;
                            txtToDate.Enabled = false;
                            txtFromDate.Enabled = false;
                        }
                    }
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
        }
    }

    public void bindbatch()
    {
        ////batch
        ddlbatch.Items.Clear();
        string sqlstr = "";
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

        //----------------display max year value 
        //sqlstr = "select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' ";
        //max_bat = Convert.ToInt32(GetFunction(sqlstr));
        //ddlbatch.SelectedValue = max_bat.ToString();
        //con.Close();
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

    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  pagesetpanel.Visible = false;
        FpSpread1.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        ne.Visible = false;
        Panel3.Visible = false;
        norecordlbl.Visible = false;
        bindsec();
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


    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //pagesetpanel.Visible = false;
        FpSpread1.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
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
    }


    //----------------------------GO button

    protected void btnGo_Click(object sender, EventArgs e)
    {
        btnclick_or_print = true;
        try
        {
            if ((txtFromDate.Text == string.Empty) || (cumfromtxt.Text == string.Empty))
            {
                frmlbl.Visible = true;
            }
            else
            {
                frmlbl.Visible = false;
            }

            if ((txtToDate.Text == string.Empty) || (cumtotxt.Text == string.Empty))
            {
                tolbl.Visible = true;
            }
            else
            {
                tolbl.Visible = false;
            }

            if (((txtFromDate.Text != string.Empty) && (cumfromtxt.Text != string.Empty)) && ((txtToDate.Text != string.Empty) && (cumtotxt.Text != string.Empty)))
            {
                if (ddlsec.Enabled == true && ddlsec.Text != "-1" && txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
                {
                    gobutton();

                }
                if (ddlsec.Enabled == false && txtFromDate.Text != string.Empty && txtToDate.Text != string.Empty)
                {
                    gobutton();
                }
                FarPoint.Web.Spread.SheetView svsort = new FarPoint.Web.Spread.SheetView();
                svsort = FpSpread1.ActiveSheetView;
                svsort.AllowSort = true;

                //  load_ddlpage();
                //view_header_setting();
            }


        }
        catch
        {
        }
    }

    public void gobutton()
    {
        //'----------------------------------------------------------------------
        //'---------------------------------------------date validate-------------
        //FpSpread1.Sheets[0].ColumnHeader.RowCount = 8;//Hidden By Srinath 14/5/2013
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
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
            tofromlbl.Text = "From Date Should Be Less Than To Date";
            tofromlbl.Visible = true;

            Buttontotal.Visible = false;

            DropDownListpage.Visible = false;
            //   pagesetpanel.Visible = false;
            Panel3.Visible = false;
            FpSpread1.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;

        }
        else
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

            tofromlbl.Text = "";
            tofromlbl.Visible = false;
            //   pagesetpanel.Visible = true;
            Buttontotal.Visible = true;

            DropDownListpage.Visible = true;

            FpSpread1.Visible = true;
            btnxl.Visible = true;
            btnprintmaster.Visible = true;
            Printcontrol.Visible = false;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;

            Panel3.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FarPoint.Web.Spread.SheetView svsort = new FarPoint.Web.Spread.SheetView();
            svsort = FpSpread1.ActiveSheetView;
            svsort.AllowSort = true;
            //'----------------------------------------------------------------------
            if (cumcheck.Checked == true)
            {
                //'---------------------------------------------date validate-------------
                string valfromdate1 = "";
                string valtodate1 = "";
                string frmconcat1 = "";

                valfromdate1 = cumfromtxt.Text.ToString();
                string[] split3 = valfromdate1.Split(new char[] { '/' });
                frmconcat1 = split3[1].ToString() + '/' + split3[0].ToString() + '/' + split3[2].ToString();
                DateTime dtfromdate1 = Convert.ToDateTime(frmconcat1.ToString());

                valtodate1 = cumtotxt.Text.ToString();
                string[] split4 = valtodate1.Split(new char[] { '/' });
                frmconcat1 = split4[1].ToString() + '/' + split4[0].ToString() + '/' + split4[2].ToString();
                DateTime dttodate1 = Convert.ToDateTime(frmconcat1.ToString());

                if (valfromdate1 == string.Empty)
                {
                    frmlbl.Visible = true;
                }
                else
                {
                    tolbl.Visible = false;
                }
                TimeSpan ts1 = dttodate1.Subtract(dtfromdate1);
                int days1 = ts1.Days;
                if (days1 < 0)
                {
                    tofromlbl.Text = "From Date Should Be Less Than To Date";
                    tofromlbl.Visible = true;

                    Buttontotal.Visible = false;

                    DropDownListpage.Visible = false;

                    Panel3.Visible = false;
                    FpSpread1.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    Printcontrol.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    FpSpread1.Sheets[0].RowCount = 0;

                }
                else
                {
                    tofromlbl.Text = "";
                    tofromlbl.Visible = false;

                    Buttontotal.Visible = true;

                    DropDownListpage.Visible = true;

                    FpSpread1.Visible = true;
                    btnxl.Visible = true;
                    btnprintmaster.Visible = true;
                    Printcontrol.Visible = false;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    Panel3.Visible = false;

                    spsizeforcum();

                    //  func_load();
                }
            }
            else
            {

                spsize();

            }
            if (tofromlbl.Visible == false)
            {
                func_load();
            }

        }
        //'-------------------------------------------------------------------------------------
        if (Convert.ToInt32(FpSpread1.Sheets[0].RowCount) == 0)
        {

            Buttontotal.Visible = false;

            DropDownListpage.Visible = false;

        }
        else
        {
            final_print_col_cnt = 0;


            //--------------------------------------------------- defn for settings
            if (Session["Rollflag"].ToString() == "0")
            {
                FpSpread1.Sheets[0].ColumnHeader.Columns[1].Visible = false;
            }
            else
            {
                FpSpread1.Sheets[0].ColumnHeader.Columns[1].Visible = true;
            }
            if (Session["Regflag"].ToString() == "0")
            {
                FpSpread1.Sheets[0].ColumnHeader.Columns[2].Visible = false;
            }
            else
            {
                FpSpread1.Sheets[0].ColumnHeader.Columns[1].Visible = true;
            }
            if (Session["studflag"].ToString() == "0")
            {
                FpSpread1.Sheets[0].ColumnHeader.Columns[4].Visible = false;
            }
            else
            {
                FpSpread1.Sheets[0].ColumnHeader.Columns[1].Visible = true;
            }

            for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
            {
                if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
                {
                    final_print_col_cnt++;
                }
            }

            //setheader_print();//Hidden By Srinath 14/5/2013
            Buttontotal.Visible = true;
            //   lblrecord.Visible = false;
            DropDownListpage.Visible = true;
            ddlpagelbl.Visible = true;

            pgsearch_lbl.Visible = true;
            pagesearch_txt.Visible = true;


            FpSpread1.Visible = true;
            btnxl.Visible = true;
            btnprintmaster.Visible = true;
            Printcontrol.Visible = false;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;

            Double totalRows = 0;
            totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
            DropDownListpage.Items.Clear();
            if (totalRows >= 10)
            {
                FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                {
                    DropDownListpage.Items.Add((k + 10).ToString());
                }
                DropDownListpage.Items.Add("Others");
                DropDownListpage.SelectedIndex = DropDownListpage.Items.Count - 2;
                FpSpread1.Height = 350;

            }
            else if (totalRows == 0)
            {
                DropDownListpage.Items.Add("0");
                FpSpread1.Height = 200;
            }
            else
            {
                FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                DropDownListpage.Items.Add(FpSpread1.Sheets[0].PageSize.ToString());
                FpSpread1.Height = 200 + (10 * Convert.ToInt32(totalRows));
            }
            totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
            Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);
            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + 1;
        }
    }

    public void func_load()
    {
        string sec;

        if (ddlsec.Enabled == true)
        {
            if (ddlsec.SelectedItem.ToString() == string.Empty)
            {
                sec = "";
            }
            else
            {
                sec = ddlsec.SelectedItem.ToString();

            }
        }
        else
        {
            sec = "";
        }


        //hat.Clear();
        //hat.Add("bath", int.Parse(ddlbatch.SelectedItem.ToString()));
        //hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
        //hat.Add("sec", sec.ToString());
        //ds4 = d2.select_method("ALL_STUDENT_DETAILS", hat, "sp");
        string sqlStr = "";
        string sections = "";
        string strsec = "";
        sections = ddlsec.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
        {
            strsec = "";
        }
        else
        {
            strsec = " and sections='" + sections.ToString() + "'";
        }
        //added By Srinath 11/8/2013
        string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
        string strorder = "";
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
        //modified by annyutha and subburaj*** 27/08/14**********//
        sqlStr = "select distinct registration.Roll_No as RollNumber, registration.Reg_No as RegistrationNumber,registration.stud_name as Student_Name,registration.stud_type as StudentType,len(registration.Roll_No) from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + " and  registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0  " + strsec + " " + Session["strvar"] + " " + strorder + ""; //Registration.current_semester=" + ddlduration.SelectedItem.Value + " and
        //***end****//
        //sqlStr = "select distinct registration.Roll_No as RollNumber, registration.Reg_No as RegistrationNumber,registration.stud_name as Student_Name,registration.stud_type as StudentType,len(registration.Roll_No) from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + "   and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0  " + strsec + " " + Session["strvar"] + " order by len(registration.Roll_No)";//Hidden By Srinatgh
        //  sqlStr = " select distinct r.roll_no as 'ROLL_NO',r.Reg_No as 'REG_NO',r.Stud_Name as 'STUD_NAME',r.Roll_Admit as 'ADMIT_NO' from registration r,applyn a where a.app_no=r.app_no and cc=0 and exam_flag<>'debar' and delflag=0 and r.batch_year=" + ddlbatch.SelectedValue.ToString() + " and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " " + strsec + "  " + Session["strvar"] + "";
        con.Close();
        con.Open();
        SqlCommand cmd = new SqlCommand(sqlStr, con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds4);

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
        count = ds1.Tables[0].Rows.Count;

        ////'===============================settings====================================
        ////if (Session["Rollflag"].ToString() == "0")
        ////{
        ////    FpSpread1.Sheets[0].ColumnHeader.Columns[1].Visible = false;
        ////}
        ////if (Session["Regflag"].ToString() == "0")
        ////{
        ////    FpSpread1.Sheets[0].ColumnHeader.Columns[2].Visible = false;
        ////}
        ////if (Session["Studflag"].ToString() == "0")
        ////{
        ////    FpSpread1.Sheets[0].ColumnHeader.Columns[4].Visible = false;
        ////}


        ////'===========================================================================


        int stu_count = ds4.Tables[0].Rows.Count;
        for (rows_count = 0; rows_count < stu_count; rows_count++)
        {
            per_abshrs_spl_fals = 0;
            tot_per_hrs_spl_fals = 0;
            tot_conduct_hr_spl_fals = 0;
            tot_ondu_spl_fals = 0;
            per_leave_fals = 0;
            per_abshrs_spl_true = 0;
            tot_per_hrs_spl_true = 0;
            tot_conduct_hr_spl_true = 0;
            tot_ondu_spl_true = 0;
            per_leave_true = 0;

            conduct_hour_new_fal = 0;
            conduct_hour_new_true = 0;
            print();
            check = 1;
            presentdays();
            print1();

            if (cumcheck.Checked == true)
            {
                //print();
                check = 2;
                cumpresentdays();
                print2();
            }
        }
        if (ds4.Tables[0].Rows.Count == 0)
        {
            FpSpread1.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            Panel3.Visible = false;
            //  pagesetpanel.Visible = false;
            norecordlbl.Text = "No Record(s) Found";
            norecordlbl.Visible = true;
        }
        else if (ds4.Tables[0].Rows.Count != 0)
        {
            norecordlbl.Visible = false;
            norecordlbl.Text = "";
            print3();
        }
    }
    private void print3()
    {

        frdate = txtFromDate.Text;
        //todate = txtToDate.Text;
        //ts = DateTime.Parse(todate.ToString()).Subtract(DateTime.Parse(frdate.ToString()));
        //diff_date = Convert.ToString(ts.Days+1);
        //difdate = int.Parse(diff_date.ToString());
        string dt = frdate;
        string[] dsplit = dt.Split(new Char[] { '/' });
        frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
        dumm_from_date = DateTime.Parse(frdate.ToString());
        int headercount = 4;
        int row_count = 1;
        string fnp = "FNP";
        string anp = "ANP";
        string fna = "FNA";
        string ana = "ANA";
        string p = "P";
        string a = "A";
        //added
        int spanstart = 5;//added by srinath 21/8/2013
        if (Session["studflag"].ToString() == "0")
        {
            if (optionbtn.SelectedValue != "pa")
            {
                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 4;


                for (ddiff = 5; ddiff <= difdate; ddiff++)
                {

                    spanstart = ddiff;
                    //  head = FpSpread1.Sheets[0].ColumnHeader.Cells[0, headercount].Text;
                    head = dumm_from_date.Day.ToString() + dumm_from_date.Month.ToString() + "FNP";
                    // head = head + "FNP";
                    int value = Convert.ToInt32(GetCorrespondingKey(head, hath));
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 4, 0].Border.BorderColorLeft = Color.Black;
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 4, 0].Text = " ";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, ddiff].Text = value.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, 0].Text = fnp.ToString();


                    head = "";
                    //head = FpSpread1.Sheets[0].ColumnHeader.Cells[0, headercount].Text;
                    head = dumm_from_date.Day.ToString() + dumm_from_date.Month.ToString() + "ANP";
                    //  head = head + "ANP";
                    value = Convert.ToInt32(GetCorrespondingKey(head, hath));
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 3, 0].Border.BorderColorLeft = Color.Black;
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 3, 0].Text = " ";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, ddiff].Text = value.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 0].Text = anp.ToString();


                    head = "";
                    //head = FpSpread1.Sheets[0].ColumnHeader.Cells[0, headercount].Text;
                    head = dumm_from_date.Day.ToString() + dumm_from_date.Month.ToString() + "FNA";
                    // head = head + "FNA"; 
                    value = Convert.ToInt32(GetCorrespondingKey(head, hath));
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Border.BorderColorLeft = Color.Black;
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = " ";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, ddiff].Text = value.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = fna.ToString();

                    head = "";
                    //head = FpSpread1.Sheets[0].ColumnHeader.Cells[0, headercount].Text;
                    head = dumm_from_date.Day.ToString() + dumm_from_date.Month.ToString() + "ANA";
                    //head = head + "ANA";
                    value = Convert.ToInt32(GetCorrespondingKey(head, hath));
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColorLeft = Color.Black;
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = " ";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ddiff].Text = value.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ana.ToString();
                    //headercount++;

                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 4, 0, 1, 5);
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 3, 0, 1, 5);
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 2, 0, 1, 5);
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);

                    dumm_from_date = dumm_from_date.AddDays(1);


                }
            }
            else
            {
                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 2;


                for (ddiff = 5; ddiff <= difdate; ddiff++)
                {

                    spanstart = ddiff;
                    //  head = FpSpread1.Sheets[0].ColumnHeader.Cells[0, headercount].Text;
                    head = dumm_from_date.Day.ToString() + dumm_from_date.Month.ToString() + "P";
                    // head = head + "FNP";
                    int value = Convert.ToInt32(GetCorrespondingKey(head, hath));
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Border.BorderColorLeft = Color.Black;
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = " ";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, ddiff].Text = value.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = p.ToString();


                    head = "";
                    //head = FpSpread1.Sheets[0].ColumnHeader.Cells[0, headercount].Text;
                    head = dumm_from_date.Day.ToString() + dumm_from_date.Month.ToString() + "A";
                    //  head = head + "ANP";
                    value = Convert.ToInt32(GetCorrespondingKey(head, hath));
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColorLeft = Color.Black;
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = " ";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ddiff].Text = value.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = a.ToString();




                    
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 2, 0, 1, 5);
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);

                    dumm_from_date = dumm_from_date.AddDays(1);


                }
            }
        }
        else
        {

            if (optionbtn.SelectedValue != "pa")
            {
                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 4;


                for (ddiff = 5; ddiff <= difdate; ddiff++)
                {
                    spanstart = ddiff;
                    //  head = FpSpread1.Sheets[0].ColumnHeader.Cells[0, headercount].Text;
                    head = dumm_from_date.Day.ToString() + dumm_from_date.Month.ToString() + "FNP";
                    // head = head + "FNP";
                    int value = Convert.ToInt32(GetCorrespondingKey(head, hath));
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 4, 0].Border.BorderColorLeft = Color.Black;
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 4, 0].Text = " ";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, ddiff].Text = value.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 4, 0].Text = fnp.ToString();


                    head = "";
                    //head = FpSpread1.Sheets[0].ColumnHeader.Cells[0, headercount].Text;
                    head = dumm_from_date.Day.ToString() + dumm_from_date.Month.ToString() + "ANP";
                    //  head = head + "ANP";
                    value = Convert.ToInt32(GetCorrespondingKey(head, hath));
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 3, 0].Border.BorderColorLeft = Color.Black;
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 3, 0].Text = " ";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, ddiff].Text = value.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 3, 0].Text = anp.ToString();


                    head = "";
                    //head = FpSpread1.Sheets[0].ColumnHeader.Cells[0, headercount].Text;
                    head = dumm_from_date.Day.ToString() + dumm_from_date.Month.ToString() + "FNA";
                    // head = head + "FNA"; 
                    value = Convert.ToInt32(GetCorrespondingKey(head, hath));
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Border.BorderColorLeft = Color.Black;
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = " ";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, ddiff].Text = value.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = fna.ToString();

                    head = "";
                    //head = FpSpread1.Sheets[0].ColumnHeader.Cells[0, headercount].Text;
                    head = dumm_from_date.Day.ToString() + dumm_from_date.Month.ToString() + "ANA";
                    //head = head + "ANA";
                    value = Convert.ToInt32(GetCorrespondingKey(head, hath));
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColorLeft = Color.Black;
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = " ";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ddiff].Text = value.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ana.ToString();
                    //headercount++;

                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 4, 0, 1, 5);
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 3, 0, 1, 5);
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 2, 0, 1, 5);
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);

                    dumm_from_date = dumm_from_date.AddDays(1);


                }
            }
            else
            {
                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 2;


                for (ddiff = 5; ddiff <= difdate; ddiff++)
                {
                    spanstart = ddiff;
                    //  head = FpSpread1.Sheets[0].ColumnHeader.Cells[0, headercount].Text;
                    head = dumm_from_date.Day.ToString() + dumm_from_date.Month.ToString() + "P";
                    // head = head + "FNP";
                    int value = Convert.ToInt32(GetCorrespondingKey(head, hath));
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Border.BorderColorLeft = Color.Black;
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = " ";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, ddiff].Text = value.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = p.ToString();


                    head = "";
                    //head = FpSpread1.Sheets[0].ColumnHeader.Cells[0, headercount].Text;
                    head = dumm_from_date.Day.ToString() + dumm_from_date.Month.ToString() + "A";
                    //  head = head + "ANP";
                    value = Convert.ToInt32(GetCorrespondingKey(head, hath));
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColorLeft = Color.Black;
                    FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = " ";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ddiff].Text = value.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = a.ToString();


                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 2, 0, 1, 5);
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);

                    dumm_from_date = dumm_from_date.AddDays(1);


                }
            }

        }
        if (optionbtn.SelectedValue != "pa")
        {
            //added by srinath 21/8/2013
            spanstart = spanstart + 1;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 4, spanstart, 1, FpSpread1.Sheets[0].ColumnCount);
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 3, spanstart, 1, FpSpread1.Sheets[0].ColumnCount);
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 2, spanstart, 1, FpSpread1.Sheets[0].ColumnCount);
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, spanstart, 1, FpSpread1.Sheets[0].ColumnCount);
            //FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount);
        }
        else
        {
            spanstart = spanstart + 1;
            
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 2, spanstart, 1, FpSpread1.Sheets[0].ColumnCount);
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, spanstart, 1, FpSpread1.Sheets[0].ColumnCount);
        }
    }

    private void print2()
    {
        string dum_tage_date, dum_tage_hrs;
        string dum_cum_tage_date, dum_cum_tage_hrs;
        per_tage_date = ((pre_present_date / per_workingdays) * 100);
        if (per_tage_date > 100)
        {
            per_tage_date = 100;
        }
        //  per_con_hrs = ((per_workingdays * NoHrs) - per_dum_unmark);
        per_con_hrs = conduct_hour_new_fal - per_dum_unmark + tot_conduct_hr_spl_fals;
        per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl_fals) / per_con_hrs) * 100);

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
        cum_con_hrs = ((cum_workingdays * NoHrs) - cum_dum_unmark + tot_conduct_hr_spl_true);
        cum_tage_hrs = (((cum_per_perhrs + tot_per_hrs_spl_true) / cum_con_hrs) * 100);
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
        //    per_con_hrs = ((per_workingdays * NoHrs) - per_dum_unmark);
        //  per_con_hrs = conduct_hour_new_fal - per_dum_unmark;

        setfp += 1;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = per_workingdays.ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;
        setfp += 1;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = pre_present_date.ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;
        setfp += 1;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = dum_tage_date.ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;

        setfp += 1;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = per_con_hrs.ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;
        setfp += 1;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = (per_per_hrs + tot_per_hrs_spl_fals).ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;
        setfp += 1;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = dum_tage_hrs.ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;
        setfp += 1;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = (per_tot_ondu + tot_ondu_spl_fals).ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;

        setfp += 1;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = cum_workingdays.ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;
        setfp += 1;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = cum_present_date.ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;
        setfp += 1;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = dum_cum_tage_date.ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;

        setfp += 1;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = cum_con_hrs.ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;
        setfp += 1;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = (cum_per_perhrs + tot_per_hrs_spl_true).ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;
        setfp += 1;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = dum_cum_tage_hrs.ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;
        setfp += 1;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = (cum_tot_ondu + tot_ondu_spl_true).ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;

        setfp += 1;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = (per_absent_date + per_abshrs_spl_true).ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;
        setfp += 1;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = (pre_leave_date + per_leave_true).ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;
        setfp += 1;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = (pre_ondu_date + tot_ondu_spl_true).ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;
        if (pointchk.Checked == true)
        {
            setfp += 1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = cum_tot_point.ToString();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;
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
            string dum_tage_date, dum_tage_hrs;
            string dum_cum_tage_date, dum_cum_tage_hrs;
            per_tage_date = ((pre_present_date / per_workingdays) * 100);
            if (per_tage_date > 100)
            {
                per_tage_date = 100;
            }
            //    per_con_hrs = ((per_workingdays * NoHrs) - per_dum_unmark);
            per_con_hrs = conduct_hour_new_fal;
          //  per_con_hrs = conduct_hour_new_fal - per_dum_unmark + tot_conduct_hr_spl_fals;
            per_tage_hrs = (((per_per_hrs + tot_per_hrs_spl_fals) / per_con_hrs) * 100);

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

            //   per_con_hrs = ((per_workingdays * NoHrs) - per_dum_unmark);
            //   per_con_hrs = conduct_hour_new_fal - per_dum_unmark;//Hidden By Srinath 24/8/2013

            setfp += 1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = per_workingdays.ToString();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;

            setfp += 1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = pre_present_date.ToString();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;

            setfp += 1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = dum_tage_date.ToString();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;



            setfp += 1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = per_con_hrs.ToString();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;

            setfp += 1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = (per_per_hrs + tot_per_hrs_spl_fals).ToString();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;

            setfp += 1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = dum_tage_hrs.ToString();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;

            setfp += 1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = (per_tot_ondu + tot_ondu_spl_fals).ToString();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;



            setfp += 1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = (per_absent_date + per_abshrs_spl_fals).ToString();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;
            setfp += 1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = (pre_leave_date + per_leave_fals).ToString();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;

            setfp += 1;
            //Modified by srinath 17/10/2013
            //if(FpSpread1.Sheets[0].RowCount> setfp)
            //{
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = (pre_ondu_date + tot_ondu_spl_fals).ToString();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;
            //}

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
    }

    private void print()
    {
        FarPoint.Web.Spread.SheetView svsort = new FarPoint.Web.Spread.SheetView();
        svsort = FpSpread1.ActiveSheetView;
        svsort.AllowSort = true;
        ++FpSpread1.Sheets[0].RowCount;
        FpSpread1.Sheets[0].RowHeader.Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColorLeft = Color.Black;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = FpSpread1.Sheets[0].RowCount.ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds4.Tables[0].Rows[rows_count]["RollNumber"].ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds4.Tables[0].Rows[rows_count]["RegistrationNumber"].ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds4.Tables[0].Rows[rows_count]["Student_Name"].ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds4.Tables[0].Rows[rows_count]["StudentType"].ToString();
        setfp = 4;
    }
    private void perdats()
    {
        setfp += 1;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].Text = pp.ToString();
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, setfp].HorizontalAlign = HorizontalAlign.Center;
        pp = "";
    }

    private void spsizeforcum()
    {
        frdate = txtFromDate.Text;
        todate = txtToDate.Text;
        year_fromat();

        ts = DateTime.Parse(todate.ToString()).Subtract(DateTime.Parse(frdate.ToString()));
        diff_date = Convert.ToString(ts.Days + 5);
        difdate = int.Parse(diff_date.ToString());

        FpSpread1.Sheets[0].RowCount = 0;
        if (pointchk.Checked == true)
        {
            FpSpread1.Sheets[0].ColumnCount = difdate + 19;
        }
        else
        {
            FpSpread1.Sheets[0].ColumnCount = difdate + 18;
        }
        //  FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;


        FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();

        FpSpread1.Sheets[0].Columns[1].CellType = textcel_type;
        FpSpread1.Sheets[0].Columns[0].CellType = textcel_type;

        //============================================================0n 6/4/12 PRABHA
        hat.Clear();
        hat.Add("college_code", Session["collegecode"].ToString());
        hat.Add("form_name", "consolidatestudreport.aspx");
        dsprint = d2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        if (dsprint.Tables[0].Rows.Count > 0)
        {

            lblpages.Visible = true;
            ddlpage.Visible = true;
            isonumber = dsprint.Tables[0].Rows[0]["ISOCode"].ToString();
            //3. header add
            if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
            {

                new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                new_header_string_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString();
                string[] new_header_string_split = new_header_string.Split(',');
                FpSpread1.Sheets[0].SheetCorner.RowCount = FpSpread1.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
            }
            //3. end header add

        }
        //==========================================================
        //  FpSpread1.Sheets[0].ColumnHeader.RowCount = FpSpread1.Sheets[0].ColumnHeader.RowCount+2;//Hidden By SRinath 14/5/2013

        //FpSpread1.Sheets[0].SheetCornerSpanModel.Add(0, 0, 8, 1);
        //FpSpread1.Sheets[0].SheetCornerSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 0, 2, 1);
        //FpSpread1.Sheets[0].SheetCorner.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 0].Text = "S.No";
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 0, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 0].Text = "S.No";
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1].Text = "Roll No";
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 2, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 2].Text = "Register No";
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 3, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 3].Text = "Name";
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 4, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 4].Text = "Student Type";

        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;

        FarPoint.Web.Spread.SheetView svsort = new FarPoint.Web.Spread.SheetView();
        svsort = FpSpread1.ActiveSheetView;
        svsort.AllowSort = true;

        //for (ddiff = 4; ddiff <= difdate; ddiff++)
        //{
        //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(8, ddiff, 2, 1);
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[8, ddiff].Text = dumm_from_date.Day.ToString();

        //    dumm_from_date = dumm_from_date.AddDays(1);
        //}

        for (ddiff = 5; ddiff <= difdate; ddiff++)
        {
            FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = dumm_from_date.Day.ToString();
            mon2 = dumm_from_date.ToString("MMMM");
            if (mon1 == mon2 || mon3 == "")
            {
                mon1 = mon2;
                mon_cnt++;
                mon3 = mon2;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), start_col, 1, mon_cnt);
                FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), start_col].Text = mon2;
                FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), start_col].HorizontalAlign = HorizontalAlign.Center;
            }
            else
            {
                start_col = ddiff;
                mon_cnt = 1;
                mon1 = mon2;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), start_col, 1, 1);
                FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), start_col].Text = mon2;
                FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), start_col].HorizontalAlign = HorizontalAlign.Center;

            }
            dumm_from_date = dumm_from_date.AddDays(1);
        }


        // if (Convert.ToInt32(Session["daywise"]) == 1)
        {
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff, 1, 3);
            FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff].Text = "Day Percentage From " + txtFromDate.Text + " To " + txtToDate.Text;

        }
        if (Convert.ToInt32(Session["daywise"]) == 1)
        {

            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Conducted days";
        if (Convert.ToInt32(Session["daywise"]) == 1)
        {

            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }

        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Attended days";
        if (Convert.ToInt32(Session["daywise"]) == 1)
        {

            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }


        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Percentage";

        if (Convert.ToInt32(Session["daywise"]) == 1)
        {

            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;

        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;

        }


        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff, 1, 4);
        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff, 1, 4);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff].Text = "Hour Percentage From " + txtFromDate.Text + " To " + txtToDate.Text;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Conducted Hours";
        if (Convert.ToInt32(Session["hourwise"]) == 1)
        {

            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;

        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }


        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Attended Hours";
        if (Convert.ToInt32(Session["hourwise"]) == 1)
        {

            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }


        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Percentage";
        if (Convert.ToInt32(Session["hourwise"]) == 1)
        {

            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }

        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Onduty Hours";
        if (Convert.ToInt32(Session["hourwise"]) == 1)
        {

            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }

        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff, 1, 3);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff].Text = "Cumulative Percentage From " + cumfromtxt.Text + " To " + cumtotxt.Text;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Conducted Days";
        if (Convert.ToInt32(Session["daywise"]) == 1)
        {

            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }


        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Attended Days";
        if (Convert.ToInt32(Session["daywise"]) == 1)
        {

            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }


        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Percentage";
        if (Convert.ToInt32(Session["daywise"]) == 1)
        {

            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }


        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff, 1, 4);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff].Text = "Cumulative Percentage From " + cumfromtxt.Text + " To " + cumtotxt.Text;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Conducted Hours";
        if (Convert.ToInt32(Session["hourwise"]) == 1)
        {

            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }


        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Attended Hours";
        if (Convert.ToInt32(Session["hourwise"]) == 1)
        {

            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }


        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Percentage";
        if (Convert.ToInt32(Session["hourwise"]) == 1)
        {

            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }


        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Onduty Hours ";
        if (Convert.ToInt32(Session["hourwise"]) == 1)
        {

            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }


        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff].Text = "No of Days Absent ";

        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff].Text = "No of Days Leave ";

        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff].Text = "No of Days OD ";
        if (pointchk.Checked == true)
        {
            ddiff = ddiff + 1;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff, 2, 1);
            FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff].Text = "PTS";
        }

    }

    private void spsize()
    {

        frdate = txtFromDate.Text;
        todate = txtToDate.Text;
        year_fromat();

        ts = DateTime.Parse(todate.ToString()).Subtract(DateTime.Parse(frdate.ToString()));
        // ts = DateTime.Parse(frdate.ToString()).Subtract(DateTime.Parse(todate.ToString()));
        diff_date = Convert.ToString(ts.Days + 5);
        difdate = int.Parse(diff_date.ToString());
        per_dif_dates = difdate - 3;
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = difdate + 11;
        //FpSpread1.Sheets[0].ColumnHeader.RowCount = 8; //Hidden By Srinath 14/5/2013
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;

        FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();

        FpSpread1.Sheets[0].Columns[1].CellType = textcel_type;
        FpSpread1.Sheets[0].Columns[0].CellType = textcel_type;
        FpSpread1.Sheets[0].Columns[2].CellType = textcel_type;


        //============================================================0n 6/4/12 PRABHA
        hat.Clear();
        hat.Add("college_code", Session["collegecode"].ToString());
        hat.Add("form_name", "consolidatestudreport.aspx");
        dsprint = d2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        if (dsprint.Tables[0].Rows.Count > 0)
        {

            lblpages.Visible = true;
            ddlpage.Visible = true;
            isonumber = dsprint.Tables[0].Rows[0]["ISOCode"].ToString();
            //3. header add
            if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
            {

                new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                new_header_string_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString();
                string[] new_header_string_split = new_header_string.Split(',');
                FpSpread1.Sheets[0].SheetCorner.RowCount = FpSpread1.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
            }
            //3. end header add

        }
        //==========================================================
        //FpSpread1.Sheets[0].ColumnHeader.RowCount = FpSpread1.Sheets[0].ColumnHeader.RowCount + 2;//Hidden By Srinath 14/5/2013


        //FpSpread1.Sheets[0].SheetCornerSpanModel.Add(0, 0, 8, 1);
        //FpSpread1.Sheets[0].SheetCornerSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount-2), 0, 2, 1);
        //FpSpread1.Sheets[0].SheetCorner.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 0].Text = "S.No";

        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 0, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 0].Text = "S.No";
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1].Text = "Roll No";
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 2, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 2].Text = "Register No";
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 3, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 3].Text = "Name";
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 4, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 4].Text = "Student Type";

        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;

        //for (ddiff =4; ddiff <= difdate; ddiff++)
        //{
        //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(8, ddiff, 2, 1);
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[8, ddiff].Text = dumm_from_date.Day.ToString();
        //    dumm_from_date = dumm_from_date.AddDays(1);
        //}
        for (ddiff = 5; ddiff <= difdate; ddiff++)
        {

            FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = dumm_from_date.Day.ToString();
            //   FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), start_col, 1, mon_cnt);
            mon2 = dumm_from_date.ToString("MMMM");
            if (mon1 == mon2 || mon3 == "")
            {
                mon1 = mon2;
                mon_cnt++;
                mon3 = mon2;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), start_col, 1, mon_cnt);
                FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), start_col].Text = mon2;
                FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), start_col].HorizontalAlign = HorizontalAlign.Center;
            }
            else
            {
                start_col = ddiff;
                mon_cnt = 1;
                mon1 = mon2;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), start_col, 1, 1);
                FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), start_col].Text = mon2;
                FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), start_col].HorizontalAlign = HorizontalAlign.Center;

            }
            dumm_from_date = dumm_from_date.AddDays(1);
        }


        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff, 1, 3);

        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff].Text = "Day Percentage From " + txtFromDate.Text + " To " + txtToDate.Text;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Conducted days";
        if (Convert.ToInt32(Session["daywise"]) == 1)
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }



        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Attended days";
        if (Convert.ToInt32(Session["daywise"]) == 1)
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }



        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Percentage";
        if (Convert.ToInt32(Session["daywise"]) == 1)
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }


        ddiff = ddiff + 1;

        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff, 1, 4);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff].Text = "Hour Percentage From " + txtFromDate.Text + " To " + txtToDate.Text;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Conducted Hours";
        if (Convert.ToInt32(Session["hourwise"]) == 1)
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }


        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Attended Hours";
        if (Convert.ToInt32(Session["hourwise"]) == 1)
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }


        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Percentage";
        if (Convert.ToInt32(Session["hourwise"]) == 1)
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }


        ddiff = ddiff + 1;
        // FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, ddiff, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), ddiff].Text = "Onduty Hours";
        if (Convert.ToInt32(Session["hourwise"]) == 1)
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = true;
        }
        else
        {
            FpSpread1.Sheets[0].Columns[ddiff].Visible = false;
        }

        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff].Text = "No of Days Absent ";
        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff].Text = "No of Days Leave ";
        ddiff = ddiff + 1;
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff, 2, 1);
        FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), ddiff].Text = "No of Days OD ";
    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.CurrentPage = 0;
        pagesearch_txt.Text = "";
        errmsg.Visible = false;
        pagesearch_txt.Text = "";
        pageddltxt.Text = "";
        pageddltxt.Text = "";
        if (DropDownListpage.Text == "Others")
        {
            pageddltxt.Visible = true;
            pageddltxt.Focus();
        }
        else
        {
            pageddltxt.Visible = false;
            FpSpread1.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            CalculateTotalPages();
        }
    }

    protected void pageddltxt_TextChanged(object sender, EventArgs e)
    {
        FpSpread1.CurrentPage = 0;
        pagesearch_txt.Text = "";
        try
        {
            if (pageddltxt.Text != string.Empty)
            {
                if (FpSpread1.Sheets[0].RowCount >= Convert.ToInt16(pageddltxt.Text.ToString()) && Convert.ToInt16(pageddltxt.Text.ToString()) != 0)
                {
                    FpSpread1.Sheets[0].PageSize = Convert.ToInt16(pageddltxt.Text.ToString());
                    errmsg.Visible = false;
                    CalculateTotalPages();
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Enter valid Record count";
                    pageddltxt.Text = "";
                }
            }
        }
        catch
        {
            errmsg.Visible = true;
            errmsg.Text = "Please Enter valid Record count";
            pageddltxt.Text = "";
        }
    }
    protected void pagesearch_txt_TextChanged(object sender, EventArgs e)
    {
        if (pagesearch_txt.Text.Trim() != "")
        {
            if (Convert.ToInt64(pagesearch_txt.Text) > Convert.ToInt64(Session["totalPages"]))
            {
                errmsg.Visible = true;
                errmsg.Text = "Exceed The Page Limit";
                FpSpread1.Visible = true;
                btnxl.Visible = true;
                btnprintmaster.Visible = true;
                Printcontrol.Visible = false;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                pagesearch_txt.Text = "";
            }
            else if (Convert.ToInt64(pagesearch_txt.Text) == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = "Page search should be more than 0";
                FpSpread1.Visible = true;
                btnxl.Visible = true;
                btnprintmaster.Visible = true;
                Printcontrol.Visible = false;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                pagesearch_txt.Text = "";
            }

            else
            {
                errmsg.Visible = false;
                FpSpread1.CurrentPage = Convert.ToInt16(pagesearch_txt.Text) - 1;
                FpSpread1.Visible = true;
                btnxl.Visible = true;
                btnprintmaster.Visible = true;
                Printcontrol.Visible = false;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
            }
        }
    }
    void CalculateTotalPages()
    {
        Double totalRows = 0;
        totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);
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
        int my_un_mark = 0;
        per_abshrs_spl = 0;
        tot_per_hrs_spl = 0;
        tot_ondu_spl = 0;
        per_hhday_spl = 0;
        unmark_spl = 0;
        tot_conduct_hr_spl = 0;



        year_fromat();
        conduct_hour_new = 0;
        hat.Clear();
        hat.Add("std_rollno", ds4.Tables[0].Rows[rows_count]["RollNumber"].ToString());
        hat.Add("from_month", cal_from_date);
        hat.Add("to_month", cal_to_date);
        ds2 = d2.select_method("STUD_ATTENDANCE", hat, "sp");

        hat.Clear();
        hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
        hat.Add("sem", int.Parse(ddlduration.SelectedItem.ToString()));
        hat.Add("from_date", frdate.ToString());
        hat.Add("to_date", todate.ToString());
        hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));
        //ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");

        //------------------------------------------------------------------
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
        // ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");


        //------------------------------------------------------------------

        //------------------------------------------------------------------
        //int iscount1 = 0;
        //holidaycon.Close();
        //holidaycon.Open();
        //string sqlstr_holiday1 = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date >= '" + frdate.ToString() + "' and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedItem.ToString() + "";
        //SqlCommand cmdholiday1 = new SqlCommand(sqlstr_holiday1, holidaycon);
        //SqlDataAdapter daholiday1 = new SqlDataAdapter(cmdholiday1);
        //DataSet dsholiday1 = new DataSet();
        //daholiday1.Fill(dsholiday1);
        //if (dsholiday1.Tables[0].Rows.Count > 0)
        //{
        //    iscount1 = Convert.ToInt16(dsholiday1.Tables[0].Rows[0]["cnt"].ToString());
        //}
        //hat.Add("iscount1", iscount1);
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
                //modified
                if (!holiday_table11.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                {
                    holiday_table11.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], holiday_sched_details);
                }
                //   holiday_table1.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], holiday_sched_details);
            }
        }

        if (ds3.Tables[1].Rows.Count != 0)
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

                if (!holiday_table11.ContainsKey((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString()))
                {
                    holiday_table11.Add((Convert.ToInt16(dummy_split[2])).ToString() + "/" + (Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString(), holiday_sched_details);
                }

                //modified
                if (!holiday_table21.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                {
                    holiday_table21.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
                }

                if (!holiday_table2.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                {
                    holiday_table2.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
                }
            }
        }

        if (ds3.Tables[2].Rows.Count != 0)
        {
            for (int k = 0; k < ds3.Tables[2].Rows.Count; k++)
            {
                string[] split_date_time1 = ds3.Tables[2].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                string[] dummy_split = split_date_time1[0].Split('/');

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

                //modified
                if (!holiday_table31.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                {
                    holiday_table31.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
                }
                if (!holiday_table3.ContainsKey(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
                {
                    holiday_table3.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], k);
                }
            }
        }

        //------------------------------------------------------------------

        if (rows_count == 0)
        {
            //=====================================12/6/12 PRABHA
            //added by srinath 21/8/2013
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
        }
        holiday = 0;
        mmyycount = ds2.Tables[0].Rows.Count;
        moncount = mmyycount - 1;
        int ds3count = ds3.Tables[1].Rows.Count;
        ds3count = ds3count - 1;
        if (ds3.Tables[0].Rows.Count != 0)
        {
            ts = DateTime.Parse(ds3.Tables[0].Rows[0]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
            diff_date = Convert.ToString(ts.Days);
            dif_date1 = double.Parse(diff_date.ToString());
            next = 0;
        }
        if (ds2.Tables[0].Rows.Count != 0)
        {
            int rrowcount = 0;
            int rowcount = 0;
            int rcount = 0;
            int ccount;

            ccount = ds3.Tables[1].Rows.Count;
            ccount = ccount - 1;

            while (dumm_from_date <= (per_to_date))
            {

                if (splhr_flag == true)
                {
                    getspecial_hr();
                }
                int temp_unmark = 0;
                for (int i = rcount; i <= mmyycount; i++)
                {
                    if (cal_from_date == int.Parse(ds2.Tables[0].Rows[next]["month_year"].ToString()))
                    {
                        string[] split_date_time1 = dumm_from_date.ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        if (!holiday_table11.Contains((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()))
                        {
                            holiday_table11.Add((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString(), "3*0*0");
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
                                split_holiday_status_1 = "0";
                                split_holiday_status_2 = "0";
                                //dumm_from_date = dumm_from_date.AddDays(1);
                                //break;
                            }


                            if (ds3.Tables[1].Rows.Count != 0)
                            {
                                ts = DateTime.Parse(ds3.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
                                //ts = DateTime.Parse(ds3.Tables[1].Rows[rowcount]["HOLI_DATE"].ToString()).Subtract(DateTime.Parse(dumm_from_date.ToString()));
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
                            if (ds3.Tables[1].Rows.Count != 0)
                            {

                                if (ds3count >= rrowcount)
                                {

                                    //if (dumm_from_date == DateTime.Parse(ds3.Tables[1].Rows[rrowcount]["HOLI_DATE"].ToString()))
                                    //{
                                    if (!holiday_table11.Contains((Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()))
                                    {
                                        if (split_holiday_status_1 == "0" && split_holiday_status_2 == "0")
                                        {
                                            holiday = 0;
                                            holiday = 1;
                                            IIhof = 2;
                                            Ihof = 2;
                                            rrowcount++;
                                        }
                                    }
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
                                    }
                                    else if (value == "7")
                                    {
                                        per_hhday += 1;
                                    }
                                    else
                                    {
                                        unmark += 1;
                                        temp_unmark++;
                                        my_un_mark++;
                                    }
                                }

                                if (per_perhrs >= minpresI)
                                {
                                    Present += 0.5;
                                    Ihof = 0.5;
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
                                    Ihof = 1;
                                }
                                if (per_ondu >= 1)
                                {
                                    Onduty += 0.5;
                                }
                                conduct_hour_new += fnhrs;
                                //modified by srinath 29/4/2014
                                if (fnhrs - temp_unmark >= minpresI)
                                {
                                    workingdays += 0.5;
                                }
                                else
                                {
                                    Ihof = 10;
                                }
                                mng_conducted_half_days += 1;
                                // workingdays += 0.5;
                            }
                            else
                            {
                                Ihof = 4;
                            }
                            per_perhrs = 0;
                            per_ondu = 0;
                            per_leave = 0;
                            per_abshrs = 0;

                            njhr = 0;
                            temp_unmark = 0;//added by srinath 29/4/2014
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

                                            per_leave += 1;
                                    }
                                    else if (value == "7")
                                    {
                                        per_hhday += 1;
                                    }
                                    else
                                    {
                                        unmark += 1;
                                        temp_unmark++;
                                        my_un_mark++;
                                    }
                                }
                                if (per_perhrs >= minpresII)
                                {
                                    Present += 0.5;
                                    IIhof = 0.5;
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
                                else if (njhr >= minpresII)
                                {

                                    IIhof = 1;
                                    njdate += 0.5;
                                }

                                if (unmark == NoHrs)
                                {
                                    Ihof = 3;
                                    IIhof = 3;
                                    //per_holidate += 1;//Modified by srinath 28/4/2014
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
                                conduct_hour_new += NoHrs - fnhrs;
                                //modified by srinath 29/4/2014
                                //workingdays += 0.5;
                                if ((NoHrs - fnhrs) - temp_unmark >= minpresII)
                                {
                                    workingdays += 0.5;
                                }
                                else
                                {
                                    if (Ihof == 10)
                                    {
                                        checknull = "1";
                                    }
                                    else
                                    {
                                        IIhof = 10;
                                    }
                                }
                                evng_conducted_half_days += 1;

                            }
                            else
                            {
                                IIhof = 4;

                            }

                            //checknull

                            per_perhrs = 0;
                            per_ondu = 0;
                            per_leave = 0;
                            per_abshrs = 0;
                            unmark = 0;
                            njhr = 0;
                            if (check == 1)
                            {
                                present_mark();
                            }
                            dumm_from_date = dumm_from_date.AddDays(1);
                            if (dumm_from_date.Day == 1)
                            {
                                cal_from_date++;
                                rcount++;
                                if (moncount > next)
                                {
                                    next++;

                                }
                            }
                            //  workingdays += 1;
                            per_perhrs = 0;
                        }
                        else
                        {

                            workingdays += 1;
                            per_holidate += 1;
                            holiday = 1;
                            dumm_from_date = dumm_from_date.AddDays(1);
                            if (dumm_from_date.Day == 1)
                            {
                                cal_from_date++;
                                rcount++;
                                if (moncount > next)
                                {
                                    next++;

                                }
                            }
                            rrowcount++;
                            if (check == 1)
                            {
                                Ihof = 2;
                                IIhof = 2;
                                present_mark();
                            }
                        }
                    }
                    else
                    {
                        // if (cal_from_date == int.Parse(ds2.Tables[0].Rows[next]["month_year"].ToString()))
                        if (check == 1)
                        {
                            string ddii = ds2.Tables[0].Rows[next]["month_year"].ToString();
                            int ddiii = int.Parse(ddii.ToString());
                            ddiii = ddiii - cal_from_date;
                            if (ddiii != -1)
                            {
                                DateTime dumm_fdate = dumm_from_date.AddDays(1 - Convert.ToInt16(dumm_from_date.Day.ToString()));
                                dumm_fdate = dumm_fdate.AddMonths(1);

                                // DateTime dumm_fdate = dumm_from_date.AddMonths(1);
                                while (dumm_from_date < (dumm_fdate))
                                {
                                    if (dumm_from_date <= (per_to_date))
                                    {
                                        pp = "NE";
                                        perdats();
                                        dumm_from_date = dumm_from_date.AddDays(1);
                                        workingdays += 1;
                                    }

                                }
                                cal_from_date++;
                            }
                            else if (ddiii == -1)
                            {
                                while (dumm_from_date <= (per_to_date))
                                {
                                    pp = "NE";
                                    perdats();
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
                            workingdays += 1;
                            dumm_from_date = dumm_from_date.AddMonths(1);
                        }
                        //if (moncount > next)
                        //{
                        //    i--;
                        //}
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
                    pp = "NE";
                    perdats();
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
            pre_present_date = Present;
            per_per_hrs = tot_per_hrs;
            per_absent_date = Absent;
            pre_ondu_date = Onduty;
            pre_leave_date = Leave;
            per_workingdays = workingdays - per_holidate - per_njdate;
            per_dum_unmark = dum_unmark;
           // conduct_hour_new_fal = conduct_hour_new;
            conduct_hour_new_fal = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value; //added by Mullai
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
            cum_workingdays = workingdays - per_holidate - cum_njdate;
            cum_dum_unmark = dum_unmark;
            cum_tot_point = absent_point + leave_point;
           // conduct_hour_new_true = conduct_hour_new;
            conduct_hour_new_true = (((mng_conducted_half_days * fnhrs) + (evng_conducted_half_days * (NoHrs - fnhrs))) - my_un_mark) - notconsider_value;
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
        next = 0;
        mng_conducted_half_days = 0;
        evng_conducted_half_days = 0;
        my_un_mark = 0;
        notconsider_value = 0;
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
    private void present_mark()
    {
        string date_FNP = dumm_from_date.Day.ToString() + dumm_from_date.Month.ToString() + "FNP";
        string date_ANP = dumm_from_date.Day.ToString() + dumm_from_date.Month.ToString() + "ANP";
        string date_FNA = dumm_from_date.Day.ToString() + dumm_from_date.Month.ToString() + "FNA";
        string date_ANA = dumm_from_date.Day.ToString() + dumm_from_date.Month.ToString() + "ANA";
        string date_P = dumm_from_date.Day.ToString() + dumm_from_date.Month.ToString() + "P";
        string date_A = dumm_from_date.Day.ToString() + dumm_from_date.Month.ToString() + "A";

        if (optionbtn.SelectedValue == "pp" || optionbtn.SelectedValue == "pa")
        {
            if (Ihof == 0.5)
            {
                pp = "P";
                if (hath.Contains(date_FNP))
                {
                    int value = Convert.ToInt32(GetCorrespondingKey(date_FNP, hath));
                    value++;
                    hath[date_FNP] = value;
                }
                else
                {
                    hath.Add(date_FNP, count_has);

                }
            }
            else if (Ihof == 0.0)
            {
                pp = "A";
                if (hath.Contains(date_FNA))
                {
                    int value = Convert.ToInt32(GetCorrespondingKey(date_FNA, hath));
                    value++;
                    hath[date_FNA] = value;
                }
                else
                {
                    hath.Add(date_FNA, count_has);
                }
            }
            else if (Ihof == 4)
            {
                pp = "H";
                if (hath.Contains(date_FNA))
                {
                    int value = Convert.ToInt32(GetCorrespondingKey(date_FNA, hath));
                    value++;
                    hath[date_FNA] = value;
                }
                else
                {
                    hath.Add(date_FNA, count_has);
                }
            }
            else if (Ihof == 10)
            {
                pp = "NE";
                if (hath.Contains(date_FNA))
                {
                    int value = Convert.ToInt32(GetCorrespondingKey(date_FNA, hath));
                    value++;
                    hath[date_FNA] = value;
                }
                else
                {
                    hath.Add(date_FNA, count_has);
                }
            }
            if (IIhof == 0.5)
            {
                pp += "/P";
                if (hath.Contains(date_ANP))
                {
                    int value = Convert.ToInt32(GetCorrespondingKey(date_ANP, hath));
                    value++;
                    hath[date_ANP] = value;
                }
                else
                {
                    hath.Add(date_ANP, count_has);
                }
            }
            else if (IIhof == 0.0)
            {
                pp += "/A";
                if (hath.Contains(date_ANA))
                {
                    int value = Convert.ToInt32(GetCorrespondingKey(date_ANA, hath));
                    value++;
                    hath[date_ANA] = value;

                }
                else
                {
                    hath.Add(date_ANA, count_has);

                }
            }
            else if (IIhof == 4)
            {
                pp += "/H";
                if (hath.Contains(date_ANA))
                {
                    int value = Convert.ToInt32(GetCorrespondingKey(date_ANA, hath));
                    value++;
                    hath[date_ANA] = value;

                }
                else
                {
                    hath.Add(date_ANA, count_has);

                }
            }
            else if (IIhof == 10)
            {
                pp += "/NE";
                if (hath.Contains(date_ANA))
                {
                    int value = Convert.ToInt32(GetCorrespondingKey(date_ANA, hath));
                    value++;
                    hath[date_ANA] = value;

                }
                else
                {
                    hath.Add(date_ANA, count_has);

                }
            }
            if (holiday == 1)
            {
                pp = "H/H";
                holiday = 0;
            }

            if (checknull == "1")
            {
                pp = "NE";

            }
            if (Ihof == 1)
            {
                pp = "NJ";
            }
            else if (IIhof == 1)
            {
                pp = "NJ";
            }
        }
        else
        {
            if (Ihof == 0.5)
            {
                pp = "0.5";
                if (hath.Contains(date_FNP))
                {
                    int value = Convert.ToInt32(GetCorrespondingKey(date_FNP, hath));
                    value++;
                    hath[date_FNP] = value;
                }
                else
                {
                    hath.Add(date_FNP, count_has);

                }
            }
            else if (Ihof == 0.0)
            {
                pp = "0";
                if (hath.Contains(date_FNA))
                {
                    int value = Convert.ToInt32(GetCorrespondingKey(date_FNA, hath));
                    value++;
                    hath[date_FNA] = value;
                }
                else
                {
                    hath.Add(date_FNA, count_has);
                }
            }
            //else if (Ihof == 4)
            //{
            //    pp = "0";
            //    if (hath.Contains(date_FNA))
            //    {
            //        int value = Convert.ToInt32(GetCorrespondingKey(date_FNA, hath));
            //        value++;
            //        hath[date_FNA] = value;
            //    }
            //    else
            //    {
            //        hath.Add(date_FNA, count_has);
            //    }
            //}
            if (IIhof == 10)
            {
                pp = "/NE";
                if (hath.Contains(date_ANP))
                {
                    int value = Convert.ToInt32(GetCorrespondingKey(date_ANP, hath));
                    value++;
                    hath[date_ANP] = value;
                }
                else
                {
                    hath.Add(date_ANP, count_has);
                }
            }
            else if (IIhof == 0.0)
            {

                pp = "/0";
                if (hath.Contains(date_ANA))
                {
                    int value = Convert.ToInt32(GetCorrespondingKey(date_ANA, hath));
                    value++;
                    hath[date_ANA] = value;

                }
                else
                {
                    hath.Add(date_ANA, count_has);

                }

            }

            if (Ihof == 0.5 && IIhof == 0.5)
            {

                pp = "1";
            }
            else if (Ihof == 0.0 && IIhof == 0.0)
            {
                pp = "0";
            }
            else if (Ihof == 0.5 || IIhof == 0.5)
            {
                pp = "0.5";
            }
            if (Ihof == 4 && IIhof == 4)
            {
                pp = "0";
            }
        }

        if (holiday == 1)
        {
            pp = "H";
        }
        if (checknull == "1")
        {
            pp = "NE";
        }


        if (Ihof == 3.0 || IIhof == 3.0)
        {
            pp = "NE";
        }
        Ihof = 0;
        IIhof = 0;
        holiday = 0;
        checknull = "";

        if (optionbtn.SelectedValue == "pa")//added by rajasekar 06_12_2018
        {
            if (pp == "P/P")
                pp = "P";
            else if (pp == "P/A")
                pp = "A";
            else if (pp == "A/P")
                pp = "A";
            else if (pp == "A/A")
                pp = "A";
            else if (pp == "H/H")
                pp = "H";
            else if (pp == "H/A")
                pp = "A";
            else if (pp == "H/P")
                pp = "P";
            else if (pp == "A/H")
                pp = "A";
            else if (pp == "P/H")
                pp = "P";
            else if (pp == "NE/NE")
                pp = "NE";
            else if (pp == "NE/A")
                pp = "A";
            else if (pp == "NE/P")
                pp = "P";
            else if (pp == "A/NE")
                pp = "A";
            else if (pp == "P/NE")
                pp = "P";

            if (pp == "P")
            {
                if (hath.Contains(date_P))
                {
                    int value = Convert.ToInt32(GetCorrespondingKey(date_P, hath));
                    value++;
                    hath[date_P] = value;
                }
                else
                {
                    hath.Add(date_P, count_has);

                }
            }
            else if (pp == "A")
            {
                if (hath.Contains(date_A))
                {
                    int value = Convert.ToInt32(GetCorrespondingKey(date_A, hath));
                    value++;
                    hath[date_A] = value;
                }
                else
                {
                    hath.Add(date_A, count_has);

                }
            }
        }

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

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  pagesetpanel.Visible = false;
        ne.Visible = false;
        FpSpread1.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Panel3.Visible = false;
        //   pagesetpanel.Visible = false;
        norecordlbl.Visible = false;
        //bindsem();
        //bindbranch();
        //bindsem();
        //bindsec();
    }
    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  pagesetpanel.Visible = false;
        ne.Visible = false;
        FpSpread1.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Panel3.Visible = false;
        norecordlbl.Visible = false;
        frmlbl.Visible = false;
        tolbl.Visible = false;
    }
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        // pagesetpanel.Visible = false;
        ne.Visible = false;
        FpSpread1.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        Panel3.Visible = false;
        norecordlbl.Visible = false;
        frmlbl.Visible = false;
        tolbl.Visible = false;
    }
    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        //  pagesetpanel.Visible = false;
        ne.Visible = false;
        FpSpread1.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
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
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        // pagesetpanel.Visible = false;
        ne.Visible = false;
        FpSpread1.Visible = false;
        Panel3.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        norecordlbl.Visible = false;
        bindbranch();
        bindsem();
        bindsec();
    }
    protected void cumfromtxt_TextChanged(object sender, EventArgs e)
    {

        // pagesetpanel.Visible = false; 
        ne.Visible = false;
    }
    protected void cumtotxt_TextChanged(object sender, EventArgs e)
    {
        //   pagesetpanel.Visible = false;
        ne.Visible = false;
    }
    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        Control cntUpdateBtn = FpSpread1.FindControl("Update");
        Control cntCancelBtn = FpSpread1.FindControl("Cancel");
        Control cntCopyBtn = FpSpread1.FindControl("Copy");
        Control cntCutBtn = FpSpread1.FindControl("Clear");
        Control cntPasteBtn = FpSpread1.FindControl("Paste");
        Control cntPageNextBtn = FpSpread1.FindControl("Next");
        Control cntPagePreviousBtn = FpSpread1.FindControl("Prev");
        Control cntEditBtn = FpSpread1.FindControl("Edit");
        // Control cntPagePrintBtn = FpSpread1.FindControl("Print");

        if ((cntPagePreviousBtn != null))
        {

            TableCell tc = (TableCell)cntPagePreviousBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            //tc = (TableCell)cntCancelBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntEditBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntUpdateBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntCopyBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntCutBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPasteBtn.Parent;
            //tr.Cells.Remove(tc);

            tc = (TableCell)cntPageNextBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPagePreviousBtn.Parent;
            tr.Cells.Remove(tc);

            ////tc = (TableCell)cntPagePrintBtn.Parent;
            ////tr.Cells.Remove(tc);

        }

        base.Render(writer);
    }
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
    //    cmd = new SqlCommand("select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website from collinfo  where college_code=" + Session["collegecode"] + "", con);
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




    //        if (FpSpread1.Sheets[0].Columns[0].Visible == true)
    //        {
    //              FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 8, 2);               
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;
    //        }
    //         else if (FpSpread1.Sheets[0].Columns[0].Visible == false && FpSpread1.Sheets[0].Columns[1].Visible == true && FpSpread1.Sheets[0].Columns[2].Visible == true)
    //        {
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 8, 2);               
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].CellType = mi;
    //         }
    //        else if (FpSpread1.Sheets[0].Columns[1].Visible == false && FpSpread1.Sheets[0].Columns[0].Visible == false)
    //        {
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 8, 2);
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].CellType = mi;
    //        }
    //         FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, (FpSpread1.Sheets[0].ColumnCount - 5));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = coll_name;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5,2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[7, 2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[7, 2].Border.BorderColorTop = Color.White;

    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 0].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 0].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 0].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 0].Border.BorderColorLeft = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[7, 0].Border.BorderColorLeft = Color.White;

    //      //      FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Border.BorderColorRight = Color.White;

    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 0].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 0].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 0].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 0].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[7, 0].Border.BorderColorRight = Color.White;

    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, ((FpSpread1.Sheets[0].ColumnCount - 3)), 1, 1);
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, ((FpSpread1.Sheets[0].ColumnCount - 3)), 1, 1);
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, ((FpSpread1.Sheets[0].ColumnCount - 3)), 1, 1);
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, ((FpSpread1.Sheets[0].ColumnCount - 3)), 1, 1);
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(4, ((FpSpread1.Sheets[0].ColumnCount - 3)), 1, 1);
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(5, ((FpSpread1.Sheets[0].ColumnCount - 3)), 1, 1);
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, ((FpSpread1.Sheets[0].ColumnCount - 3)), 1, 1);
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(7, ((FpSpread1.Sheets[0].ColumnCount - 3)), 1, 1);

    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorRight = Color.White;
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[7, 2].Border.BorderColorRight = Color.White;


    //           // FpSpread1.Sheets[0].ColumnHeader.Cells[0, (FpSpread1.Sheets[0].ColumnCount - 3)].Border.BorderColorLeft = Color.White;


    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 1, (FpSpread1.Sheets[0].ColumnCount - 5));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Text = address1 + "-" + address2 + "-" + address3;
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, 2, 1, (FpSpread1.Sheets[0].ColumnCount - 5));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 2].Text = "Phone:" + phoneno + "  Fax:" + faxno;
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, 2, 1, (FpSpread1.Sheets[0].ColumnCount - 5));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[3, 2].Text = "Email:" + email + "  Web Site:" + website;
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(4, 2, 1, (FpSpread1.Sheets[0].ColumnCount - 5));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[4, 2].Text = "Consolidate Student Attendance Report";
    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(5, 2, 1, (FpSpread1.Sheets[0].ColumnCount - 5));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[5, 2].Text = "--------------------------------------------------------------";

    //            string sec_val = "";

    //            if (ddlsec.SelectedValue.ToString() != string.Empty && ddlsec.SelectedValue.ToString() != null)
    //            {
    //                sec_val = "Section: " + ddlsec.SelectedItem.ToString();
    //            }
    //            else
    //            {
    //                sec_val = "";
    //            }

    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, 2, 1, (FpSpread1.Sheets[0].ColumnCount - 5));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[6, 2].Text = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";


    //            string dt = DateTime.Today.ToShortDateString();
    //            string[] dsplit = dt.Split(new Char[] { '/' });


    //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(7, 2, 1, (FpSpread1.Sheets[0].ColumnCount - 5));
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[7, 2].Text = "From: " + txtFromDate.Text + "      To: " + txtToDate.Text + "       Date: " + dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();


    //         FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, ((FpSpread1.Sheets[0].ColumnCount - 3)), 8, 3);
    //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, (FpSpread1.Sheets[0].ColumnCount - 3)].CellType = mi2;


    //            }

    //    int overall_colcount = 0;
    //    overall_colcount = FpSpread1.Sheets[0].ColumnCount;
    //    FpSpread1.Width = overall_colcount * 100;


    //}
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

    protected void RadioHeader_CheckedChanged(object sender, EventArgs e)
    {
        FpSpread1.Sheets[0].ColumnHeader.Rows[0].Visible = true;
        FpSpread1.Sheets[0].ColumnHeader.Rows[1].Visible = true;
        FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = true;
        FpSpread1.Sheets[0].ColumnHeader.Rows[3].Visible = true;
        FpSpread1.Sheets[0].ColumnHeader.Rows[4].Visible = true;
        FpSpread1.Sheets[0].ColumnHeader.Rows[5].Visible = true;
        FpSpread1.Sheets[0].ColumnHeader.Rows[6].Visible = true;
        FpSpread1.Sheets[0].ColumnHeader.Rows[7].Visible = true;

        int i = 0;
        ddlpage.Items.Clear();
        int totrowcount = FpSpread1.Sheets[0].RowCount;
        int pages = totrowcount / 25;
        int intialrow = 1;
        int remainrows = totrowcount % 25;
        if (FpSpread1.Sheets[0].RowCount > 0)
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
            for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread1.Sheets[0].Rows[i].Visible = true;
            }
            Double totalRows = 0;
            totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
            Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);
            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
            DropDownListpage.Items.Clear();
            if (totalRows >= 10)
            {
                FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                {
                    DropDownListpage.Items.Add((k + 10).ToString());
                }
                DropDownListpage.Items.Add("Others");
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                FpSpread1.Height = 335;

            }
            else if (totalRows == 0)
            {
                DropDownListpage.Items.Add("0");
                FpSpread1.Height = 100;
            }
            else
            {
                FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                DropDownListpage.Items.Add(FpSpread1.Sheets[0].PageSize.ToString());
                FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
            }
            if (Convert.ToInt32(FpSpread1.Sheets[0].RowCount) > 10)
            {
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                FpSpread1.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                //   FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
                CalculateTotalPages();
            }


            Panel3.Visible = false;


        }
        else
        {

            norecordlbl.Visible = false;
            Panel3.Visible = false;
        }
    }

    public void load_ddlpage()
    {
        int totrowcount = FpSpread1.Sheets[0].RowCount;
        int pages = totrowcount / 25;
        int intialrow = 1;
        int remainrows = totrowcount % 25;

        if (FpSpread1.Sheets[0].RowCount > 0)
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
    }
    //protected void Radiowithoutheader_CheckedChanged(object sender, EventArgs e)
    //{
    //    int i = 0;
    //    norecordlbl.Visible = false;
    //    ddlpage.Items.Clear();
    //    int totrowcount = FpSpread1.Sheets[0].RowCount;
    //    int pages = totrowcount / 25;
    //    int intialrow = 1;
    //    int remainrows = totrowcount % 25;
    //    if (FpSpread1.Sheets[0].RowCount > 0)
    //    {
    //        int i5 = 0;
    //        ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
    //        for (i = 1; i <= pages; i++)
    //        {
    //            i5 = i;

    //            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //            intialrow = intialrow + 25;
    //        }
    //        if (remainrows > 0)
    //        {
    //            i = i5 + 1;
    //            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //        }
    //    }
    //    if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
    //    {
    //        for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
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
    //}
    //protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //loadvalues_pagesetting();

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
    //            FpSpread1.Sheets[0].ColumnHeader.Rows[7].Visible = false ;
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
    //}
    protected void optionbtn_SelectedIndexChanged(object sender, EventArgs e)
    {
        //pagesetpanel.Visible = false;
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            //Modified By Srinath 
            norecordlbl.Visible = false;
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
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

        Session["page_redirect_value"] = ddlbatch.SelectedIndex + "," + ddldegree.SelectedIndex + "," + ddlbranch.SelectedIndex + "," + sem_index + "," + sec_index + "," + txtFromDate.Text + "," + txtToDate.Text + "," + cumcheck.Checked + "," + cumfromtxt.Text + "," + cumtotxt.Text + "," + pointchk.Checked;

        // first_btngo();
        btnGo_Click(sender, e);

        lblpages.Visible = true;
        ddlpage.Visible = true;
        string clmnheadrname = "";
        int total_clmn_count = FpSpread1.Sheets[0].ColumnCount;

        for (int srtcnt = 0; srtcnt < total_clmn_count; srtcnt++)
        {
            if (FpSpread1.Sheets[0].Columns[srtcnt].Visible == true)
            {
                if (FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text != "")
                {
                    subcolumntext = "";
                    if (clmnheadrname == "")
                    {
                        clmnheadrname = FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
                    }
                    else
                    {
                        if (child_flag == false)
                        {
                            clmnheadrname = clmnheadrname + "," + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
                        }
                        else
                        {
                            clmnheadrname = clmnheadrname + "$)," + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
                        }

                    }
                    child_flag = false;
                }
                else
                {
                    child_flag = true;
                    if (subcolumntext == "")
                    {
                        for (int te = srtcnt - 1; te <= srtcnt; te++)
                        {
                            if (te == srtcnt - 1)
                            {
                                clmnheadrname = clmnheadrname + "* ($" + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
                                subcolumntext = clmnheadrname + "* ($" + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
                            }
                            else
                            {
                                clmnheadrname = clmnheadrname + "$" + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
                                subcolumntext = clmnheadrname + "$" + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, te].Text;

                            }
                        }
                    }
                    else
                    {
                        subcolumntext = subcolumntext + "$" + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
                        clmnheadrname = clmnheadrname + "$" + FpSpread1.Sheets[0].ColumnHeader.Cells[FpSpread1.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
                    }
                }
            }
        }
        Response.Redirect("Print_Master_Setting_new.aspx?ID=" + clmnheadrname.ToString() + ":" + "consolidatestudreport.aspx" + ":" + ddlbatch.SelectedItem.ToString() + " Batch - " + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] " + strsec + " :" + "Consolidate Student Attendance Report");

    }
    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        norecordlbl.Visible = false;

        view_header = dsprint.Tables[0].Rows[0]["header_flag_value"].ToString();
        if (view_header == "0")
        {
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread1.Sheets[0].Rows[i].Visible = false;
            }
            int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
            int end = start + 24;
            if (end >= FpSpread1.Sheets[0].RowCount)
            {
                end = FpSpread1.Sheets[0].RowCount;
            }
            int rowstart = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(start);
            int rowend = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(end);
            for (int i = start - 1; i < end; i++)
            {
                FpSpread1.Sheets[0].Rows[i].Visible = true;
            }
            for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
            {
                FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
            }

        }
        else if (view_header == "1")
        {

            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread1.Sheets[0].Rows[i].Visible = false;
            }
            int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
            int end = start + 24;
            if (end >= FpSpread1.Sheets[0].RowCount)
            {
                end = FpSpread1.Sheets[0].RowCount;
            }
            int rowstart = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(start);
            int rowend = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(end);
            for (int i = start - 1; i < end; i++)
            {
                FpSpread1.Sheets[0].Rows[i].Visible = true;
            }
            if (Convert.ToInt32(ddlpage.SelectedValue.ToString()) == 1)
            {
                for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
                }
            }
            else
            {
                for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread1.Sheets[0].Rows[i].Visible = false;
            }
            int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
            int end = start + 24;
            if (end >= FpSpread1.Sheets[0].RowCount)
            {
                end = FpSpread1.Sheets[0].RowCount;
            }
            int rowstart = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(start);
            int rowend = FpSpread1.Sheets[0].RowCount - Convert.ToInt32(end);
            for (int i = start - 1; i < end; i++)
            {
                FpSpread1.Sheets[0].Rows[i].Visible = true;
            }

            {
                for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
                }
            }
        }
        if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
        {

            if (view_header == "1" || view_header == "0")
            {
                for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
                }
            }
            else
            {
                for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
                }
            }

            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread1.Sheets[0].Rows[i].Visible = true;
            }
            Double totalRows = 0;
            totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
            Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);
            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
            DropDownListpage.Items.Clear();
            if (totalRows >= 10)
            {
                FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                {
                    DropDownListpage.Items.Add((k + 10).ToString());
                }
                DropDownListpage.Items.Add("Others");
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                FpSpread1.Height = 335;
            }
            else if (totalRows == 0)
            {
                DropDownListpage.Items.Add("0");
                FpSpread1.Height = 100;
            }
            else
            {
                FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                DropDownListpage.Items.Add(FpSpread1.Sheets[0].PageSize.ToString());
                FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
            }
            if (Convert.ToInt32(FpSpread1.Sheets[0].RowCount) > 10)
            {
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                FpSpread1.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                //  FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
                CalculateTotalPages();
            }

            Panel3.Visible = false;
        }
        else
        {
            Panel3.Visible = false;
        }

        if (view_footer_text != "")
        {
            if (view_footer == "0")
            {
                FpSpread1.Sheets[0].Rows[(FpSpread1.Sheets[0].RowCount - 1)].Visible = true;
                FpSpread1.Sheets[0].Rows[(FpSpread1.Sheets[0].RowCount - 2)].Visible = true;
                FpSpread1.Sheets[0].Rows[(FpSpread1.Sheets[0].RowCount - 3)].Visible = true;
            }
            else
            {
                if (ddlpage.Text != "")
                {
                    if (ddlpage.SelectedIndex != ddlpage.Items.Count - 1)
                    {
                        FpSpread1.Sheets[0].Rows[(FpSpread1.Sheets[0].RowCount - 1)].Visible = false;
                        FpSpread1.Sheets[0].Rows[(FpSpread1.Sheets[0].RowCount - 2)].Visible = false;
                        FpSpread1.Sheets[0].Rows[(FpSpread1.Sheets[0].RowCount - 3)].Visible = false;
                    }
                }
            }
        }
    }

    public void print_btngo()
    {
        btnclick_or_print = false;

        FpSpread1.Sheets[0].RowHeader.Columns[0].Visible = false;
        final_print_col_cnt = 0;
        norecordlbl.Visible = false;
        check_col_count_flag = false;


        FpSpread1.Sheets[0].SheetCorner.RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 0;
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].SheetCorner.RowCount = 8;
        FpSpread1.Sheets[0].ColumnCount = 6;


        hat.Clear();
        hat.Add("college_code", Session["collegecode"].ToString());
        hat.Add("form_name", "consolidatestudreport.aspx");
        dsprint = d2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        if (dsprint.Tables[0].Rows.Count > 0)
        {
            lblpages.Visible = true;
            ddlpage.Visible = true;

            gobutton();

            final_print_col_cnt = 0;
            //3. header add
            if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
            {

                new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                string[] new_header_string_split = new_header_string.Split(',');
                // FpSpread1.Sheets[0].SheetCorner.RowCount = FpSpread1.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
            }
            //3. end header add

            //1.set visible columns
            column_field = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------------get column field



            if (column_field != "" && column_field != null)
            {
                check_col_count_flag = true;


                if (btnclick_or_print == false)
                {

                    for (col_count_all = 0; col_count_all < FpSpread1.Sheets[0].ColumnCount; col_count_all++)
                    {
                        FpSpread1.Sheets[0].Columns[col_count_all].Visible = false;//------------invisible all column                                
                    }


                    printvar = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------visible setting columns
                    string[] split_printvar = printvar.Split(',');
                    for (int splval = 0; splval <= split_printvar.GetUpperBound(0); splval++)
                    {
                        span_cnt = 0;
                        string[] split_star = split_printvar[splval].Split('*');
                        if (split_star.GetUpperBound(0) > 0)
                        {
                            for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount - 1; col_count++)
                            {
                                if (FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), col_count].Text == split_star[0])
                                {
                                    child_span_count = 0;

                                    string[] split_star_doller = split_star[1].Split('$');
                                    for (int doller_count = 1; doller_count < (split_star_doller.GetUpperBound(0)); doller_count++)
                                    {
                                        for (int child_node = col_count; child_node <= (col_count + split_star_doller.GetUpperBound(0)) - 1; child_node++)
                                        {
                                            if (FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 1), child_node].Text == split_star_doller[doller_count])
                                            {
                                                span_cnt++;
                                                if (span_cnt == 1 && child_node == col_count + 1)
                                                {
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), col_count + 1].Text = split_star[0].ToString();
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


                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add((FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), col_count, 1, span_cnt);


                                                FpSpread1.Sheets[0].Columns[child_node].Visible = true;

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
                            for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
                            {
                                if (FpSpread1.Sheets[0].ColumnHeader.Cells[(FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), col_count].Text == split_printvar[splval])
                                {
                                    FpSpread1.Sheets[0].Columns[col_count].Visible = true;



                                    final_print_col_cnt++;
                                    break;
                                }
                            }
                        }
                    }
                }


                else
                {
                    for (col_count_all = 0; col_count_all < FpSpread1.Sheets[0].ColumnCount; col_count_all++)
                    {
                        FpSpread1.Sheets[0].Columns[col_count_all].Visible = true;//------------invisible all column                                
                    }
                    final_print_col_cnt = FpSpread1.Sheets[0].ColumnCount;
                    //'===============================settings====================================
                    if (Session["Rollflag"].ToString() == "0")
                    {
                        final_print_col_cnt--;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[1].Visible = false;
                    }
                    if (Session["Regflag"].ToString() == "0")
                    {
                        final_print_col_cnt--;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[2].Visible = false;
                    }
                    if (Session["Studflag"].ToString() == "0")
                    {
                        final_print_col_cnt--;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[4].Visible = false;
                    }

                    //'===========================================================================


                }
                //1 end.set visible columns









                //4.college information setting

                //setheader_print();//Hidden By Srinath 14/5/2013

                //4 end.college information setting

            }
            else
            {
                FpSpread1.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                Printcontrol.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                Panel3.Visible = false;
                lblpages.Visible = false;
                ddlpage.Visible = false;
                norecordlbl.Visible = true;
                norecordlbl.Text = "Select Atleast One Column Field From The Treeview";
            }
        }
        //FpSpread1.Sheets[0].Columns[0].Width = 100;
        //  FpSpread1.Width = final_print_col_cnt * 100;
    }

    public void view_header_setting()
    {


        if (dsprint.Tables[0].Rows.Count > 0)
        {

            view_footer = dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString();
            view_header = dsprint.Tables[0].Rows[0]["header_flag_value"].ToString();
            view_footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
            if (view_header == "0" || view_header == "1")
            {
                norecordlbl.Visible = false;

                for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
                }

                int i = 0;
                ddlpage.Items.Clear();
                int totrowcount = FpSpread1.Sheets[0].RowCount;
                int pages = totrowcount / 25;
                int intialrow = 1;
                int remainrows = totrowcount % 25;
                if (FpSpread1.Sheets[0].RowCount > 0)
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
                    for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                    {
                        FpSpread1.Sheets[0].Rows[i].Visible = true;
                    }
                    Double totalRows = 0;
                    totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
                    Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);
                    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                    DropDownListpage.Items.Clear();
                    if (totalRows >= 10)
                    {
                        FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                        {
                            DropDownListpage.Items.Add((k + 10).ToString());
                        }
                        DropDownListpage.Items.Add("Others");
                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        FpSpread1.Height = 335;

                    }
                    else if (totalRows == 0)
                    {
                        DropDownListpage.Items.Add("0");
                        FpSpread1.Height = 100;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                        DropDownListpage.Items.Add(FpSpread1.Sheets[0].PageSize.ToString());
                        FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
                    }
                    if (Convert.ToInt32(FpSpread1.Sheets[0].RowCount) > 10)
                    {
                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        FpSpread1.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                        FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
                        CalculateTotalPages();
                    }


                    Panel3.Visible = false;


                }
                else
                {
                    norecordlbl.Visible = false;
                    Panel3.Visible = false;
                }
            }
            else if (view_header == "2")
            {

                for (int row_cnt = 0; row_cnt < FpSpread1.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
                }

                norecordlbl.Visible = false;
                int i = 0;
                ddlpage.Items.Clear();
                int totrowcount = FpSpread1.Sheets[0].RowCount;
                int pages = totrowcount / 25;
                int intialrow = 1;
                int remainrows = totrowcount % 25;
                if (FpSpread1.Sheets[0].RowCount > 0)
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
                    for (i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                    {
                        FpSpread1.Sheets[0].Rows[i].Visible = true;
                    }
                    Double totalRows = 0;
                    totalRows = Convert.ToInt32(FpSpread1.Sheets[0].RowCount);
                    Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread1.Sheets[0].PageSize);
                    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                    DropDownListpage.Items.Clear();
                    if (totalRows >= 10)
                    {
                        FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                        {
                            DropDownListpage.Items.Add((k + 10).ToString());
                        }
                        DropDownListpage.Items.Add("Others");
                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        FpSpread1.Height = 335;

                    }
                    else if (totalRows == 0)
                    {
                        DropDownListpage.Items.Add("0");
                        FpSpread1.Height = 100;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                        DropDownListpage.Items.Add(FpSpread1.Sheets[0].PageSize.ToString());
                        FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
                    }
                    if (Convert.ToInt32(FpSpread1.Sheets[0].RowCount) > 10)
                    {
                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        FpSpread1.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                        //  FpSpread1.Height = 100 + (10 * Convert.ToInt32(totalRows));
                        CalculateTotalPages();
                    }
                    Panel3.Visible = false;
                }
                else
                {
                    Panel3.Visible = false;
                }
            }
            else
            {

            }
        }
    }

    //public void setheader_print()
    //{
    //    if (dsprint.Tables[0].Rows.Count > 0)
    //    {
    //        //2.Footer setting
    //        if (dsprint.Tables[0].Rows[0]["footer"].ToString() != null && dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
    //        {
    //            footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());
    //            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 3;

    //            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 3), 0].ColumnSpan = FpSpread1.Sheets[0].ColumnCount;
    //            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 2), 0].ColumnSpan = FpSpread1.Sheets[0].ColumnCount;

    //            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 3), 0].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 2), 0].Border.BorderColorTop = Color.White;
    //            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 2), 0].Border.BorderColorBottom = Color.White;
    //            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), 0].Border.BorderColorTop = Color.White;


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

    //                for (col_count = 0; col_count < FpSpread1.Sheets[0].ColumnCount; col_count++)
    //                {
    //                    if (FpSpread1.Sheets[0].Columns[col_count].Visible == true)
    //                    {
    //                        FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, final_print_col_cnt);
    //                        FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text;
    //                        FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
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
    //                        FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                        FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                        if (col_count != FpSpread1.Sheets[0].ColumnCount - 1)
    //                        {
    //                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                        }
    //                        FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
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
    //                            FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer + footer_balanc_col);
    //                        }
    //                        else
    //                        {

    //                            FpSpread1.Sheets[0].SpanModel.Add((FpSpread1.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer);

    //                        }
    //                        FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                        FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                        if (col_count - 1 >= 0)
    //                        {
    //                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorLeft = Color.White;
    //                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorRight = Color.White;
    //                        }
    //                        FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                        FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                        if (col_count + 1 < FpSpread1.Sheets[0].ColumnCount)
    //                        {
    //                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorLeft = Color.White;
    //                            FpSpread1.Sheets[0].Cells[(FpSpread1.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorRight = Color.White;
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



    //    // dsprint.Tables[0].Rows[0]["column_fields"].ToString();

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

    //                    for (int x_head_row = 0; x_head_row < (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2); x_head_row++)
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(x_head_row, col_count, 1, 1);
    //                    }
    //                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                      if (logo_length_left > 0 && logo_length_left.ToString() != "")
    //                    {
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                      }
    //                    FpSpread1.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
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

    //                    for (int x_head_row = 0; x_head_row < (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2); x_head_row++)
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(x_head_row, col_count, 1, 1);
    //                    }
    //                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                    if (logo_length_left > 0 && logo_length_left.ToString() != "")
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                    }
    //                    FpSpread1.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
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
    //                    //FpSpread1.Sheets[0].ColumnHeader.Columns[col_count].Width = 100;//

    //                    for (int x_head_row = 0; x_head_row < (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2); x_head_row++)
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(x_head_row, col_count, 1, 1);
    //                    }
    //                    if (isonumber != string.Empty)
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Text = "ISO CODE:" + isonumber;
    //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount - 3), 1);
    //                        if (logo_length > 0 && logo_length.ToString() != "")
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].CellType = mi2;
    //                        }
    //                        FpSpread1.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                    else
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                        if (logo_length > 0 && logo_length.ToString() != "")
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi2;
    //                        }
    //                        FpSpread1.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                    }
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
    //                    //   FpSpread1.Sheets[0].ColumnHeader.Columns[col_count].Width = 100;//

    //                    for (int x_head_row = 0; x_head_row < (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2); x_head_row++)
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(x_head_row, col_count, 1, 1);
    //                    }

    //                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                    if (logo_length_left > 0 && logo_length_left.ToString() != "")
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                    }
    //                    FpSpread1.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                }


    //                if (final_print_col_cnt == temp_count + 1)
    //                {
    //                    end_column = col_count;


    //                    for (int x_head_row = 0; x_head_row < (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2); x_head_row++)
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(x_head_row, col_count, 1, 1);
    //                    }
    //                    if (isonumber != string.Empty)
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count - 1].Text = "ISO CODE:";// +isonumber;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Text = isonumber;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].HorizontalAlign = HorizontalAlign.Left;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount - 3), 1);
    //                        if (logo_length > 0 && logo_length.ToString() != "")
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].CellType = mi2;
    //                        }
    //                        FpSpread1.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorTop = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count - 1].Border.BorderColorBottom = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count - 1].Border.BorderColorTop = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count - 1].Border.BorderColorRight = Color.White;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count ].Border.BorderColorLeft= Color.White;
    //                    }
    //                    else
    //                    {
    //                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (FpSpread1.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                        if (logo_length > 0 && logo_length.ToString() != "")
    //                        {
    //                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi2;
    //                        }
    //                        FpSpread1.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
    //                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                    }

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

    //    FpSpread1.Width = final_print_col_cnt * 100;
    //}

    public void one_column()
    {


        header_text();

        // if (final_print_col_cnt == 3)
        //{
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Text = coll_name;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;

        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Text = "ISO CODE:";
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorLeft = Color.White;
        //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
        //}
        //else
        //{
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Text = coll_name;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
        // }

        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Text = address1 + "-" + address2 + "-" + address3;
        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorTop = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorBottom = Color.White;

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

        FpSpread1.Sheets[0].ColumnHeader.Cells[2, col_count].Text = phone + fax;
        FpSpread1.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorTop = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorBottom = Color.White;

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

        FpSpread1.Sheets[0].ColumnHeader.Cells[3, col_count].Text = email_id + web_add;
        FpSpread1.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorTop = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorBottom = Color.White;

        if (form_name != "" && form_name != null)
        {
            FpSpread1.Sheets[0].ColumnHeader.Cells[4, col_count].Text = form_name;
            FpSpread1.Sheets[0].ColumnHeader.Cells[5, col_count].Text = "----------------------------------------------------";
        }
        FpSpread1.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorTop = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorBottom = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorTop = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorBottom = Color.White;



        FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Text = degree_deatil;
        FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorTop = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorBottom = Color.White;

        FpSpread1.Sheets[0].ColumnHeader.Cells[7, col_count].Text = "From: " + txtFromDate.Text + "      To: " + txtToDate.Text + "       Date: " + DateTime.Now.ToString("dd/MM/yyyy");
        FpSpread1.Sheets[0].ColumnHeader.Cells[7, col_count].Border.BorderColorTop = Color.White;


        int temp_count_temp = 0;

        if (dsprint.Tables[0].Rows.Count > 0)
        {
            if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
            {
                FpSpread1.Sheets[0].ColumnHeader.Cells[7, col_count].Border.BorderColorBottom = Color.White;
                string[] new_header_string_index_split = new_header_string_index.Split(',');
                for (int row_head_count = 8; row_head_count < (8 + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Text = new_header_string_split[temp_count_temp].ToString();
                    FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorTop = Color.White;
                    if (row_head_count != (8 + new_header_string_split.GetUpperBound(0)))
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorBottom = Color.White;
                    }

                    if (temp_count_temp <= new_header_string_index_split.GetUpperBound(0))
                    {
                        header_alignment = new_header_string_index_split[temp_count_temp].ToString();
                        if (header_alignment != string.Empty)
                        {
                            if (header_alignment == "2")
                            {
                                FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else if (header_alignment == "1")
                            {
                                FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Left;
                            }
                            else
                            {
                                FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Right;
                            }
                        }
                    }

                    temp_count_temp++;
                }
            }


        }
    }

    public void more_column()
    {


        header_text();

        //=========iso 12/6/12 PRABHA
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Text = coll_name;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
        if (isonumber != string.Empty)
        {
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count - 1));
        }
        else
        {
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count));
        }

        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Text = address1 + "-" + address2 + "-" + address3;
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, 1, (end_column - col_count));
        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorTop = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorBottom = Color.White;

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

        FpSpread1.Sheets[0].ColumnHeader.Cells[2, col_count].Text = phone + fax;
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, col_count, 1, (end_column - col_count));
        FpSpread1.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorTop = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorBottom = Color.White;

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

        FpSpread1.Sheets[0].ColumnHeader.Cells[3, col_count].Text = email_id + web_add;
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(3, col_count, 1, (end_column - col_count));
        FpSpread1.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorTop = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorBottom = Color.White;

        if (form_name != "" && form_name != null)
        {
            FpSpread1.Sheets[0].ColumnHeader.Cells[4, col_count].Text = form_name;
            FpSpread1.Sheets[0].ColumnHeader.Cells[5, col_count].Text = "----------------------------------------------------";

        }
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(4, col_count, 1, (end_column - col_count));
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(5, col_count, 1, (end_column - col_count));
        FpSpread1.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorTop = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorBottom = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorTop = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorBottom = Color.White;


        FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Text = degree_deatil;
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(6, col_count, 1, (end_column - col_count));
        FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorTop = Color.White;
        FpSpread1.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorBottom = Color.White;

        FpSpread1.Sheets[0].ColumnHeader.Cells[7, col_count].Text = "From: " + txtFromDate.Text + "      To: " + txtToDate.Text + "       Date: " + DateTime.Now.ToString("dd/MM/yyyy");
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(7, col_count, 1, (end_column - col_count));
        FpSpread1.Sheets[0].ColumnHeader.Cells[7, col_count].Border.BorderColorTop = Color.White;


        int temp_count_temp = 0;
        if (dsprint.Tables[0].Rows.Count > 0)
        {
            if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
            {
                new_header_string_split = (dsprint.Tables[0].Rows[0]["new_header_name"].ToString()).Split(',');
                string[] new_header_string_index_split = new_header_string_index.Split(',');
                FpSpread1.Sheets[0].ColumnHeader.Cells[7, col_count].Border.BorderColorBottom = Color.White;
                for (int row_head_count = 8; row_head_count < (8 + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Text = new_header_string_split[temp_count_temp].ToString();
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(row_head_count, col_count, 1, (end_column - col_count));
                    FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorTop = Color.White;
                    if (row_head_count != (8 + new_header_string_split.GetUpperBound(0)))
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].Border.BorderColorBottom = Color.White;
                    }

                    if (temp_count_temp <= new_header_string_index_split.GetUpperBound(0))
                    {
                        header_alignment = new_header_string_index_split[temp_count_temp].ToString();
                        if (header_alignment != string.Empty)
                        {
                            if (header_alignment == "2")
                            {
                                FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else if (header_alignment == "1")
                            {
                                FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Left;
                            }
                            else
                            {
                                FpSpread1.Sheets[0].ColumnHeader.Cells[row_head_count, col_count].HorizontalAlign = HorizontalAlign.Right;
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
        cmd = new SqlCommand("select isnull(college_name,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website,isnull(form_heading_name,'') as form_name,isnull(batch_degree_branch,'') as degree_deatil,isnull(header_align,'') as header_alignment,isnull(header_flag_value,'') as view_header from print_master_setting  where form_name='consolidatestudreport.aspx'", con);
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
                    form_name = "Consolidate Student Attendance Report";
                    degree_deatil = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
                    // header_alignment = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
                    // view_header = dr_collinfo["view_header"].ToString();
                }

            }
        }
    }
    public void getspecial_hr()
    {


        con_splhr_query_master.Close();
        con_splhr_query_master.Open();
        DataSet ds_splhr_query_master = new DataSet();
        //  no_stud_flag = false;
        string strsplhrsec = "";
        if (ddlsec.SelectedItem.ToString() == "All" || ddlsec.SelectedItem.ToString() == string.Empty || ddlsec.SelectedItem.ToString() == "-1")
        {
            strsplhrsec = "";
        }
        else
        {
            strsplhrsec = " and sections='" + ddlsec.SelectedItem.ToString() + "'";
        }
        //string splhr_query_master = "select attendance from specialhr_attendance sa,registration r where hrdet_no in(select hrdet_no from specialhr_details where hrentry_no=(select hrentry_no from  specialhr_master where batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and date='" + dumm_from_date + "')  ) and r.roll_no=sa.roll_no and batch_year=" + ddlbatch.SelectedValue.ToString() + " and current_semester=" + ddlduration.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + "  and (CC = 0)  AND (DelFlag = 0)  AND (Exam_Flag <> 'debar') and sa.roll_no='" + ds4.Tables[0].Rows[rows_count]["RollNumber"].ToString() + "'  order by r.roll_no asc";
        string splhr_query_master = "select attendance from specialhr_attendance sa,registration r where hrdet_no in(select hrdet_no from specialhr_details where hrentry_no in (select hrentry_no from  specialhr_master where batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and date='" + dumm_from_date + "' " + strsplhrsec + ")  ) and r.roll_no=sa.roll_no and batch_year=" + ddlbatch.SelectedValue.ToString() + " and current_semester=" + ddlduration.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + "  and (CC = 0)  AND (DelFlag = 0)  AND (Exam_Flag <> 'debar') and sa.roll_no='" + ds4.Tables[0].Rows[rows_count]["RollNumber"].ToString() + "'  order by r.roll_no asc";
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
        }
        else if (check == 2)
        {
            per_abshrs_spl_true = per_abshrs_spl;
            tot_per_hrs_spl_true = tot_per_hrs_spl;
            per_leave_true = per_leave;
            tot_conduct_hr_spl_true = tot_conduct_hr_spl;
            tot_ondu_spl_true = tot_ondu_spl;
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
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Session["column_header_row_count"] = Convert.ToString(FpSpread1.ColumnHeader.RowCount);
        string sections = ddlsec.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
        {
            sections = "";
        }
        else
        {
            sections = "- Sec-" + sections;
        }
        string degreedetails = "Consolidate Student Attendance Report" + '@' + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '[' + ddlbranch.SelectedItem.ToString() + ']' + '-' + "Sem-" + ddlduration.SelectedItem.ToString() + sections + '@' + "Date :" + txtFromDate.Text.ToString() + " To " + txtToDate.Text.ToString();
        string pagename = "consolidatestudreport.aspx";
        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
}
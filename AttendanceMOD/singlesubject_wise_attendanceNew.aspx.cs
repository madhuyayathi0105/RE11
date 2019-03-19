using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using FarPoint.Web.Spread;
using InsproDataAccess;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

public partial class singlesubject_wise_attendance : System.Web.UI.Page
{
    [Serializable()]
    public class MyImg : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl;
            img.Width = Unit.Percentage(80);
            return img;
        }
    }
    FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();

    SqlConnection con_splhr_query_master = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection holidaycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_sem = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_date = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);

    SqlCommand cmd1;
    SqlCommand cmd;
    SqlCommand cmd_attnd;
    SqlCommand cmd_sem_shed;
    SqlCommand cmd_alt_shed;

    DAccess2 dacc = new DAccess2();
    DAccess2 d2 = new DAccess2();

    DataSet ds_sphr = new DataSet();
    DataSet dsprint = new DataSet();
    DataSet ds_getvalues = new DataSet();
    DataSet ds = new DataSet();
    DataSet ds_subject = new DataSet();
    DataSet ds_holi = new DataSet();
    DataSet ds_student = new DataSet();
    DataSet ds_date = new DataSet();
    DataSet ds_attndmaster = new DataSet();

    static Hashtable ht_sphr = new Hashtable();
    static Hashtable has_subtype = new Hashtable();
    Hashtable has_attnd_masterset_notconsider = new Hashtable();
    Hashtable hatabsentvalues = new Hashtable();
    Hashtable has_hs = new Hashtable();
    Hashtable has = new Hashtable();
    Hashtable has_load_rollno = new Hashtable();
    Hashtable has_total_attnd_hour = new Hashtable();
    Hashtable has_total_absent_hour = new Hashtable();
    Hashtable result_has = new Hashtable();
    Hashtable hat_holy = new Hashtable();
    Hashtable has_attnd_masterset = new Hashtable();
    Hashtable temp_has_subj_code = new Hashtable();
    Hashtable hat = new Hashtable();

    static string from_date_sem = string.Empty;
    string to_date_sem = string.Empty;
    string coll_name = string.Empty;
    string address1 = string.Empty;
    string address2 = string.Empty;
    string address3 = string.Empty;
    string phoneno = string.Empty;
    string phone = string.Empty;
    string faxno = string.Empty;
    string fax = string.Empty;
    string email = string.Empty;
    string email_id = string.Empty;
    string website = string.Empty;
    string web_add = string.Empty;
    string form_name = string.Empty;
    string header_alignment = string.Empty;
    string degree_deatil = string.Empty;
    string new_header_string_index = string.Empty;
    string isonumber = string.Empty;
    string markorder = string.Empty;
    string[] new_header_string_split = new string[100];

    double tot_hr = 0;

    Boolean spl_hr_flag = false;
    Boolean no_stud_flag = false;
    Boolean recflag = false;
    Boolean holiflag = false;
    Boolean check_alter = false;
    Boolean payflag = false;
    Boolean check_col_count_flag = false;
    Boolean samehr_flag = false;
    Boolean chkflag = false;
    Boolean splhr_flag = false;
    static Boolean forschoolsetting = false;

    int count_master = 0;
    int col_count_all = 0;
    int span_cnt = 0;
    int col_count = 0;
    int child_span_count = 0;
    int footer_count = 0;
    int split_col_for_footer = 0;
    int footer_balanc_col = 0;
    int tf = 0;
    int final_print_col_cnt = 0;
    int span_count = 0;
    int present_count = 0;
    int roll_count = 0;
    int temp_count = 0;
    int mng_hrs = 0, evng_hrs = 0;
    int temp_stud_count = 0;
    int row_count = 0;
    int no_of_hrs = 0;
    int stud_count = 0;
    int between_visible_col_cnt = 0, between_visible_col_cnt_bal = 0;
    int start_column = 0, x = 0, visi_col_second = 0, visi_col_first = 0;
    int end_column = 0;
    int split_holiday_status_1 = 0, split_holiday_status_2 = 0;

    string[] string_session_values;
    string[] split_holiday_status = new string[1000];
    string Att_mark = string.Empty;
    string printvar = string.Empty;
    string footer_text = string.Empty;
    string view_footer = string.Empty;
    string view_header = string.Empty;
    string view_footer_text = string.Empty;
    string section_lab = string.Empty;
    string regularflag = string.Empty;
    string new_header_string = string.Empty;
    string order = string.Empty;
    string roll_no = string.Empty;
    string sem_start_date = string.Empty;
    string strDay = string.Empty;
    string dummy_date = string.Empty;
    string temp_hr_field = string.Empty;
    string subject_no = string.Empty;
    string full_hour = string.Empty;
    string single_hour = string.Empty;
    string column_field = string.Empty;
    string date_temp_field = string.Empty;
    string month_year = string.Empty;
    string present_calcflag = string.Empty;
    string halforfull = string.Empty;
    string mng = string.Empty;
    string evng = string.Empty;
    string holiday_sched_details = string.Empty;
    string subj_type = string.Empty;
    string group_user = string.Empty;
    string singleuser = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string date1 = string.Empty;
    string datefrom = string.Empty;
    string date2 = string.Empty;
    string dateto = string.Empty;
    string strsec = string.Empty;
    string value_holi_status = string.Empty;
    static string grouporusercode = string.Empty;

    DateTime Admission_date = new DateTime();
    DateTime dt1 = new DateTime();
    DateTime dt2 = new DateTime();
    DateTime temp_date = new DateTime();
    static DateTime from_date = new DateTime();
    static DateTime to_date = new DateTime();

    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    protected void Page_PreRender(object sender, EventArgs e)
    {
        try
        {
        }
        catch
        {
        }
    }

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
            //lblpages.Visible = false;
            //ddlpage.Visible = false;
            subject_spread.Sheets[0].SheetName = " ";
            subject_spread.Sheets[0].AutoPostBack = true;
            subject_spread.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
            subject_spread.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            subject_spread.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
            subject_spread.ActiveSheetView.DefaultStyle.Font.Name = "Book Antiqua";
            subject_spread.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
            subject_spread.ActiveSheetView.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            //---------------page number
            subject_spread.Sheets[0].PageSize = 10;
            subject_spread.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
            subject_spread.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
            subject_spread.Pager.Align = HorizontalAlign.Right;
            subject_spread.Pager.Font.Bold = true;
            subject_spread.Pager.Font.Name = "Book Antiqua";
            subject_spread.Pager.ForeColor = Color.DarkGreen;
            subject_spread.Pager.BackColor = Color.Beige;
            subject_spread.Pager.BackColor = Color.AliceBlue;
            subject_spread.Pager.PageCount = 5;
            //---------------------------
            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.HorizontalAlign = HorizontalAlign.Center;
            style.ForeColor = Color.Black;
            style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            subject_spread.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            subject_spread.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
            subject_spread.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            subject_spread.Sheets[0].AllowTableCorner = true;
            subject_spread.Sheets[0].SheetCorner.Columns[0].Width = 50;
            subject_spread.CommandBar.Visible = false;
            //==================visibility
            subject_spread.Visible = false;
            btnxl.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            // pnl_head_pageset.Visible = false;
            //pnl_pagesetting.Visible = false;
            errlbl.Visible = false;
            frmlbl.Visible = false;
            tolbl.Visible = false;
            tofromlbl.Visible = false;
            // pageddltxt.Visible = false;
            //   binddate();
            string date = Convert.ToString(DateTime.Now.ToString("MM/d/yyyy"));
            string[] split = date.Split(new Char[] { '/' });
            string date_disp = split[1] + "/" + split[0] + "/" + split[2];
            txtFromDate.Text = DateTime.Now.ToString("d/MM/yyyy");
            Session["curr_year"] = split[2].ToString();
            //------------initial date picker value
            string dt = DateTime.Now.ToString("MM/d/yyyy");
            string[] dsplit = dt.Split(new Char[] { '/' });
            txtToDate.Text = DateTime.Now.ToString("d/MM/yyyy");
            txtFromDate.Text = DateTime.Now.ToString("d/MM/yyyy");
            txtexcelname.Text = string.Empty;
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            Session["Sex"] = "0";
            Session["flag"] = "-1";
            string Master = string.Empty;
            string strdayflag = string.Empty;
            //  string regularflag = string.Empty;
            string genderflag = string.Empty;
            Master = "select * from Master_Settings where " + grouporusercode + "";
            mysql.Open();
            SqlDataReader mtrdr;
            SqlCommand mtcmd = new SqlCommand(Master, mysql);
            string regularflag = string.Empty;
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
                    if (mtrdr["settings"].ToString() == "Days Scholor" && mtrdr["value"].ToString() == "1")
                    {
                        strdayflag = " and (registration.Stud_Type='Day Scholar'";
                    }
                    if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                    {
                        if (strdayflag != null && strdayflag != "\0")
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
                        // Session["strvar"] = Session["strvar"] + " and (mode=1)";
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
                        //Session["strvar"] = Session["strvar"] + " and (mode=3)";
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
            Session["StaffSelector"] = "0";
            string minimumabsentsms = dacc.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString() + "'").Split('-')[0];
            if (minimumabsentsms.Trim() == "1")
            {
                Session["StaffSelector"] = "1";
            }
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
                    load_subject();
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
                    if (string_session_values.GetUpperBound(0) == 8)
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
                            load_subject();
                            if (Convert.ToBoolean(string_session_values[7].ToString() == "True"))
                            {
                                ddlsubject.SelectedIndex = Convert.ToInt16(string_session_values[8].ToString());
                            }
                            else
                            {
                                ddlsubject.Enabled = false;
                            }
                            //print_btngo();
                            //  setheader_print();//Hidden By Srinath 15/5/2013
                            //view_header_setting();
                            //   subject_spread.Width = final_print_col_cnt * 100;
                        }
                        subject_spread.Sheets[0].ColumnHeader.Rows[5].Visible = false;
                        subject_spread.Sheets[0].ColumnHeader.Rows[7].Visible = false;
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
                //===================================
            }
            GetTesttype();
            loadonduty();
            chkonduty.Checked = true;
            chkondutyspit.Checked = true;
            txtonduty.Visible = true;
            ponduty.Visible = true;
            // Added By Sridharan 12 Mar 2015
            //{
            string grouporusercodeschool = string.Empty;
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

    public void bindbatch()
    {
        ddlbatch.Items.Clear();
        ds = dacc.select_method_wo_parameter("bind_batch", "sp");
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
        has.Clear();
        has.Add("single_user", singleuser);
        has.Add("group_code", group_user);
        has.Add("college_code", collegecode);
        has.Add("user_code", usercode);
        ds = dacc.select_method("bind_degree", has, "sp");
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
        has.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        has.Add("single_user", singleuser);
        has.Add("group_code", group_user);
        has.Add("course_id", ddldegree.SelectedValue);
        has.Add("college_code", collegecode);
        has.Add("user_code", usercode);
        ds = dacc.select_method("bind_branch", has, "sp");
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
        string duration = string.Empty;
        Boolean first_year = false;
        has.Clear();
        collegecode = Session["collegecode"].ToString();
        has.Add("degree_code", ddlbranch.SelectedValue.ToString());
        has.Add("batch_year", ddlbatch.SelectedValue.ToString());
        has.Add("college_code", collegecode);
        ds = dacc.select_method("bind_sem", has, "sp");
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
        has.Clear();
        has.Add("batch_year", ddlbatch.SelectedValue.ToString());
        has.Add("degree_code", ddlbranch.SelectedValue);
        ds = dacc.select_method("bind_sec", has, "sp");
        int count5 = ds.Tables[0].Rows.Count;
        if (count5 > 0)
        {
            ddlsec.DataSource = ds;
            ddlsec.DataTextField = "sections";
            ddlsec.DataValueField = "sections";
            ddlsec.DataBind();
            ddlsec.Enabled = true;
        }
        else
        {
            ddlsec.Enabled = false;
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtexcelname.Text = string.Empty;
        subject_spread.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        //  pnl_head_pageset.Visible = false;
        //pnl_pagesetting.Visible = false;
        errlbl.Visible = false;
        //bindbranch();
        //bindsem();
        //bindsec();
        load_subject();
        // binddate();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtexcelname.Text = string.Empty;
        subject_spread.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        // pnl_head_pageset.Visible = false;
        //pnl_pagesetting.Visible = false;
        errlbl.Visible = false;
        bindbranch();
        bindsem();
        bindsec();
        load_subject();
        //  binddate();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtexcelname.Text = string.Empty;
        subject_spread.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        //   pnl_head_pageset.Visible = false;
        // pnl_pagesetting.Visible = false;
        errlbl.Visible = false;
        bindsem();
        bindsec();
        load_subject();
        //  binddate();
    }

    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtexcelname.Text = string.Empty;
        subject_spread.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        //  pnl_head_pageset.Visible = false;
        // pnl_pagesetting.Visible = false;
        errlbl.Visible = false;
        bindsec();
        load_subject();
        // binddate();
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtexcelname.Text = string.Empty;
        subject_spread.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        // pnl_head_pageset.Visible = false;
        // pnl_pagesetting.Visible = false;
        errlbl.Visible = false;
        load_subject();
        // binddate();
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtexcelname.Text = string.Empty;
            errmsg.Visible = false;
            subject_spread.Visible = false;
            btnxl.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            //pnl_pagesetting.Visible = false;
            frmlbl.Visible = false;
            tolbl.Visible = false;
            tofromlbl.Visible = false;
            errlbl.Visible = false;
            if (txtFromDate.Text != "")
            {
                string[] spitfrom = txtFromDate.Text.Split('/');
                DateTime dtfrom = Convert.ToDateTime(spitfrom[1] + '/' + spitfrom[0] + '/' + spitfrom[2]);
                string[] spilttodate = txtToDate.Text.Split('/');
                DateTime dtto = Convert.ToDateTime(spilttodate[1] + '/' + spilttodate[0] + '/' + spilttodate[2]);
                if (dtto < dtfrom)
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    errlbl.Visible = true;
                    errlbl.Text = "To Date Must Be Greater Than From Date";
                }
            }
            else
            {
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        catch (Exception ex)
        {
            txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            errlbl.Visible = true;
            errlbl.Text = "Please Enter Valid From Date";
        }
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtexcelname.Text = string.Empty;
            errmsg.Visible = false;
            subject_spread.Visible = false;
            btnxl.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            //  pnl_pagesetting.Visible = false;
            frmlbl.Visible = false;
            tolbl.Visible = false;
            tofromlbl.Visible = false;
            errlbl.Visible = false;
            if (txtToDate.Text != "")
            {
                string[] spitfrom = txtFromDate.Text.Split('/');
                DateTime dtfrom = Convert.ToDateTime(spitfrom[1] + '/' + spitfrom[0] + '/' + spitfrom[2]);
                string[] spilttodate = txtToDate.Text.Split('/');
                DateTime dtto = Convert.ToDateTime(spilttodate[1] + '/' + spilttodate[0] + '/' + spilttodate[2]);
                if (dtto < dtfrom)
                {
                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    errlbl.Visible = true;
                    errlbl.Text = "To Date Must Be Greater Than From Date";
                }
            }
            else
            {
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        catch (Exception ex)
        {
            txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            errlbl.Visible = true;
            errlbl.Text = "Please Enter Valid From Date";
        }
    }

    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        subject_spread.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        //  pnl_head_pageset.Visible = false;
        // pnl_pagesetting.Visible = false;
        errlbl.Visible = false;
        txtexcelname.Text = string.Empty;
        GetTesttype();
    }

    public void GetTesttype()
    {
        try
        {
            con.Close();
            con.Open();
            string SyllabusYr;
            string SyllabusQry;
            SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester =" + ddlduration.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + "";
            SyllabusYr = GetFunction(SyllabusQry.ToString());
            string Sqlstr;
            Sqlstr = string.Empty;
            if (SyllabusQry != "" && SyllabusQry != null)
            {
                Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester=" + ddlduration.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " order by criteria";
                SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(Sqlstr, con);
                DataSet titles = new DataSet();
                con.Close();
                con.Open();
                sqlAdapter1.Fill(titles);
                ddltest.DataSource = titles;
                ddltest.DataValueField = "Criteria_No";
                ddltest.DataTextField = "Criteria";
                ddltest.DataBind();
            }
        }
        catch
        {
        }
    }

    public void load_subject()
    {
        //  string staff_code="";
        subject_spread.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        //   pnl_head_pageset.Visible = false;
        //  pnl_pagesetting.Visible = false;
        frmlbl.Visible = false;
        tolbl.Visible = false;
        tofromlbl.Visible = false;
        errlbl.Visible = false;
        int count_subject = 0;
        txtexcelname.Text = string.Empty;
        has.Clear();
        has.Add("Batch_Year", ddlbatch.SelectedValue.ToString());
        has.Add("DegCode", ddlbranch.SelectedValue.ToString());
        has.Add("Sems", ddlduration.SelectedItem.ToString());
        has.Add("staffcode", Session["Staff_Code"].ToString());
        if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1" || ddlsec.Enabled == false)
        {
            has.Add("sec", "");
        }
        else
        {
            has.Add("sec", ddlsec.SelectedValue.ToString());
        }
        ds_subject = dacc.select_method("single_subjectwise_attnd", has, "sp");
        count_subject = (ds_subject.Tables[0].Rows.Count);
        if (count_subject > 0)
        {
            ddlsubject.Enabled = true;
            ddlsubject.DataSource = ds_subject;
            ddlsubject.DataTextField = "subject_name";
            ddlsubject.DataValueField = "subject_no";
            ddlsubject.DataBind();
            ddlsubject.Items.Insert(0, "--Select--");
            //   ddlsubject.Items.Insert(0, " ");
            has_subtype.Clear();
            for (int i = 0; i < ds_subject.Tables[0].Rows.Count; i++)
            {
                if (!has_subtype.ContainsKey(ds_subject.Tables[0].Rows[i]["subject_no"].ToString()))
                {
                    has_subtype.Add(ds_subject.Tables[0].Rows[i]["subject_no"].ToString(), ds_subject.Tables[0].Rows[i]["subject_type"].ToString());
                }
            }
        }
        else
        {
            ddlsubject.Enabled = false;
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            txtexcelname.Text = string.Empty;
            if (ddlsec.Items.Count > 0)
            {
                if (Convert.ToString(ddlsec.SelectedValue).Trim() == "" || Convert.ToString(ddlsec.SelectedValue).Trim() == "-1" || Convert.ToString(ddlsec.SelectedValue).Trim().ToLower() == "all")
                {
                    section_lab = string.Empty;
                }
                else
                {
                    section_lab = " and l.sections='" + Convert.ToString(ddlsec.SelectedItem).Trim() + "'";
                }
            }
            btn_click();
            int temp_col = 0;
            if (subject_spread.Sheets[0].ColumnCount > 0 && subject_spread.Sheets[0].RowCount > 0)
            {
                //for (temp_col = 0; temp_col < subject_spread.Sheets[0].ColumnCount; temp_col++)
                //{
                //    subject_spread.Sheets[0].Columns[temp_col].Visible = true;
                //}
                subject_spread.Sheets[0].Columns[1].Visible = false;
                if (Session["Rollflag"] != null && Session["Rollflag"].ToString() == "0")
                {
                    subject_spread.Sheets[0].ColumnHeader.Columns[1].Visible = false;
                }
                else
                {
                    subject_spread.Sheets[0].ColumnHeader.Columns[1].Visible = true;
                }
                if (Session["Regflag"] != null && Session["Regflag"].ToString() == "0")
                {
                    subject_spread.Sheets[0].ColumnHeader.Columns[2].Visible = false;
                }
                else
                {
                    subject_spread.Sheets[0].ColumnHeader.Columns[2].Visible = true;
                }
                //final_print_col_cnt = 0;
                //for (temp_col = 0; temp_col < subject_spread.Sheets[0].ColumnCount; temp_col++)
                //{
                //    if (subject_spread.Sheets[0].Columns[temp_col].Visible == true)
                //    {
                //        final_print_col_cnt++;
                //    }
                //}
                //  setheader_print();//Hidden By Srinath 15/5/2013
                //view_header_setting();
                //subject_spread.Sheets[0].ColumnHeader.Rows[5].Visible = false;
                //subject_spread.Sheets[0].ColumnHeader.Rows[7].Visible = false;
            }
            else
            {
                //pnl_head_pageset.Visible = false;
                //pnl_pagesetting.Visible = false;
                btnxl.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                subject_spread.Visible = false;
                errlbl.Visible = true;
                errlbl.Text = "No Record(s) Found";
            }
            //subject_spread.Sheets[0].ColumnHeader.Rows[5].Visible = false;
            //subject_spread.Sheets[0].ColumnHeader.Rows[7].Visible = false;
            subject_spread.Sheets[0].PageSize = subject_spread.Sheets[0].RowCount;
        }
        catch
        {
        }
    }

    public void btn_click()
    {
        try
        {
            subject_spread.Visible = false;
            btnxl.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            //     pnl_head_pageset.Visible = false;
            //pnl_pagesetting.Visible = false;
            frmlbl.Visible = false;
            tolbl.Visible = false;
            tofromlbl.Visible = false;
            errlbl.Visible = false;
            date1 = txtFromDate.Text;
            if (date1.Trim() != "")
            {
                string[] split = date1.Split(new Char[] { '/' });
                if (split.GetUpperBound(0) == 2)//-------date valid
                {
                    if (Convert.ToInt16(split[0].ToString()) <= 31 && Convert.ToInt16(split[1].ToString()) <= 12 && Convert.ToInt16(split[0].ToString()) <= Convert.ToInt16(DateTime.Now.Year))
                    {
                        datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                        date2 = txtToDate.Text.ToString();
                        if (date2.Trim() != "")
                        {
                            string[] split1 = date2.Split(new Char[] { '/' });
                            if (split1.GetUpperBound(0) == 2)//--date valid
                            {
                                if (Convert.ToInt16(split1[0].ToString()) <= 31 && Convert.ToInt16(split1[1].ToString()) <= 12 && Convert.ToInt16(split1[0].ToString()) <= Convert.ToInt16(DateTime.Now.Year))
                                {
                                    dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                                    dt1 = Convert.ToDateTime(datefrom.ToString());
                                    Session["from_date_time"] = dt1;
                                    dt2 = Convert.ToDateTime(dateto.ToString());
                                    Session["to_date_time"] = dt2;
                                    TimeSpan t = dt2.Subtract(dt1);
                                    long days = t.Days;
                                    Session["days"] = days;
                                    if (days >= 0)//-----check date difference
                                    {
                                        if (ddlsubject.Text != "" && ddlsubject.Text != " " && ddlsubject.Text != "--Select--")
                                        {
                                            load_student();
                                        }
                                        else
                                        {
                                            errlbl.Visible = true;
                                            errlbl.Text = "Select Subject And Then Click Go";
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
                            tolbl.Visible = true;
                            tolbl.Text = "Select To Date";
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
            else
            {
                frmlbl.Visible = true;
                frmlbl.Text = "Select From Date";
            }
        }
        catch
        {
        }
    }

    public void load_student()
    {
        try
        {
            FarPoint.Web.Spread.SheetView svsort = new FarPoint.Web.Spread.SheetView();
            svsort = subject_spread.ActiveSheetView;
            svsort.AllowSort = true;
            subject_spread.Sheets[0].RowCount = 0;
            subject_spread.SheetCorner.RowCount = 0;
            //hidden by Srinath 15/5/2013
            // subject_spread.Sheets[0].ColumnHeader.RowCount = 9;
            subject_spread.Sheets[0].ColumnHeader.RowCount = 2;
            //========find holiday
            has.Clear();
            has.Add("from_date", dt1);
            has.Add("to_date", dt2);
            has.Add("degree_code", ddlbranch.SelectedValue.ToString());
            has.Add("sem", ddlduration.SelectedValue.ToString());
            has.Add("coll_code", Session["collegecode"].ToString());
            int iscount = 0;
            holidaycon.Close();
            holidaycon.Open();
            string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + dt1.ToString() + "' and '" + dt2.ToString() + "' and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedItem.ToString() + "";
            SqlCommand cmdholiday = new SqlCommand(sqlstr_holiday, holidaycon);
            SqlDataAdapter daholiday = new SqlDataAdapter(cmdholiday);
            DataSet dsholiday = new DataSet();
            daholiday.Fill(dsholiday);
            if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
            {
                iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
            }
            has.Add("iscount", iscount);
            ds_holi = dacc.select_method("HOLIDATE_DETAILS_FINE", has, "sp");
            if (ds_holi.Tables.Count > 0 && ds_holi.Tables[0].Rows.Count > 0)
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
            if (ddlsec.Items.Count > 0)
            {
                if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1")
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " and r.sections='" + ddlsec.SelectedItem.ToString() + "'";
                }
            }
            string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
            string strorder = "ORDER BY r.roll_no";
            string srialno = dacc.GetFunction("select LinkValue from inssettings where college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "' and linkname='Student Attendance'");
            if (srialno == "1")
            {
                strorder = "ORDER BY r.serialno";
            }
            if (orderby_Setting == "1")
            {
                strorder = "ORDER BY r.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strorder = "ORDER BY r.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "ORDER BY r.roll_no,r.Reg_No,r.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "ORDER BY r.roll_no,r.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "ORDER BY r.Reg_No,r.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "ORDER BY r.roll_no,r.Stud_Name";
            }
            con.Close();
            con.Open();
            string strstaffselector = string.Empty;
            if (Convert.ToString(Session["StaffSelector"]).Trim() == "1")
            {
                if (Session["Staff_Code"] != null)
                {
                    if (Convert.ToString(Session["Staff_Code"]).Trim() != "" && Convert.ToString(Session["Staff_Code"]).Trim() != "0")
                    {
                        // strstaffselector = " and sc.staffcode like '" + Session["Staff_Code"].ToString() + "'";
                        strstaffselector = " and sc.staffcode like '%" + Session["Staff_Code"].ToString() + "%'"; // Added by jairam 07-03-2015
                    }
                }
            }

            string sex = "0";
            string flag = "-1";
            string Master = string.Empty;
            string strdayflag = string.Empty;
            string genderflag = string.Empty;
            string regularflag = string.Empty;
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0] + "'";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string qryfilter = string.Empty;
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            if (!string.IsNullOrEmpty(grouporusercode))
            {
                Master = "select * from Master_Settings  where " + grouporusercode + "";
                ds = dirAcc.selectDataSet(Master);
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow mtrdr in ds.Tables[0].Rows)
                {
                    if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "sex" && Convert.ToString(mtrdr["value"]) == "1")
                    {
                        sex = "1";
                    }
                    if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "general" && Convert.ToString(mtrdr["value"]) == "1")
                    {
                        flag = "0";
                    }
                    if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "as per lesson" && Convert.ToString(mtrdr["value"]) == "1")
                    {
                        flag = "1";
                    }
                    if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "male" && Convert.ToString(mtrdr["value"]).Trim() == "1")
                    {
                        genderflag = " and (app.sex='0'";
                    }
                    if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "female" && Convert.ToString(mtrdr["value"]).Trim() == "1")
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
                    if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "days scholor" && Convert.ToString(mtrdr["value"]).Trim() == "1")
                    {
                        strdayflag = " and (r.Stud_Type='Day Scholar'";
                    }
                    if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "hostel" && Convert.ToString(mtrdr["value"]).Trim() == "1")
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
                    if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "regular")
                    {
                        regularflag = "and ((r.mode=1)";
                    }
                    if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "lateral")
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
                    if (Convert.ToString(mtrdr["settings"]).Trim().ToLower() == "transfer")
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
                }
            }
            if (strdayflag != null && strdayflag != "")
            {
                strdayflag = strdayflag + ")";
            }
            qryfilter = strdayflag;
            if (regularflag != "")
            {
                regularflag = regularflag + ")";
            }
            if (genderflag != "")
            {
                genderflag = genderflag + ")";
            }
            qryfilter += regularflag + genderflag;

            //cmd = new SqlCommand(" select distinct registration.roll_no as 'ROLL NO', registration.stud_name as 'STUD NAME', registration.reg_no as 'REG NO',p.No_of_hrs_per_day as 'PER DAY',schorder as 'order',start_date,no_of_hrs_I_half_day,no_of_hrs_II_half_day,len(att.roll_no), convert(varchar(15),adm_date,103) as adm_date,registration.serialno FROM attendance att , registration ,   PeriodAttndSchedule p  ,seminfo s,applyn a,subjectChooser sc WHERE att.roll_no=registration.roll_no and   registration.degree_code=p.degree_code and  registration.Batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and  s.batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and registration.degree_code= " + ddlbranch.SelectedValue.ToString() + " and s.degree_code= " + ddlbranch.SelectedValue.ToString() + " and  s.semester=" + ddlduration.SelectedValue.ToString() + " and p.semester=" + ddlduration.SelectedValue.ToString() + "  and (registration.CC = 0)  AND (registration.DelFlag = 0)  AND (registration.Exam_Flag <> 'debar') AND (registration.Current_Semester IS NOT NULL) and  registration.app_no=a.app_no " + strsec + " and sc.roll_no=Registration.Roll_No  and sc.subject_no='" + ddlsubject.SelectedValue.ToString() + "' " + strstaffselector + " " + Session["strvar"] + " " + strorder + " ", con);


            string studQ = "select distinct r.roll_no as 'ROLL NO', r.stud_name as 'STUD NAME', r.reg_no as 'REG NO',p.No_of_hrs_per_day as 'PER DAY',schorder as 'order',start_date,no_of_hrs_I_half_day,no_of_hrs_II_half_day,len(att.roll_no), convert(varchar(15),adm_date,103) as adm_date,r.serialno FROM attendance att , registration r ,   PeriodAttndSchedule p  ,seminfo s,applyn a,subjectChooser sc WHERE att.roll_no=r.roll_no and   r.degree_code=p.degree_code and  r.Batch_Year=" + ddlbatch.SelectedValue.ToString() + "  and  s.batch_Year='" + ddlbatch.SelectedValue.ToString() + "'  and r.degree_code= '" + ddlbranch.SelectedValue.ToString() + "' and s.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and  s.semester='" + ddlduration.SelectedValue.ToString() + "' and p.semester='" + ddlduration.SelectedValue.ToString() + "' and (r.CC = 0)  AND (r.DelFlag = 0)  AND (r.Exam_Flag <> 'debar') AND (r.Current_Semester IS NOT NULL) and  r.app_no=a.app_no " + strsec + " and sc.roll_no=r.Roll_No  and sc.subject_no='" + ddlsubject.SelectedValue.ToString() + "' " + strstaffselector + " " + qryfilter + " " + strorder + " ";



            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(ds_student);
            ds_student = dirAcc.selectDataSet(studQ);

            stud_count = (ds_student.Tables.Count > 0) ? ds_student.Tables[0].Rows.Count : 0;
            if (stud_count > 0)
            {
                subject_spread.Visible = true;
                btnxl.Visible = true;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                //   pnl_head_pageset.Visible = true;
                //pnl_pagesetting.Visible = false;
                int temp_count_temp = 0;
                //Hidden By Srinath 15/5/2013
                //hat.Clear();
                //hat.Add("college_code", Session["collegecode"].ToString());
                //hat.Add("form_name", "singlesubject_wise_attendance.aspx");
                //dsprint = d2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
                ////======================0n 3/5/12 PRABHA
                //if (dsprint.Tables[0].Rows.Count > 0)
                //{
                //    isonumber = dsprint.Tables[0].Rows[0]["ISOCode"].ToString();
                //    if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
                //    {
                //        subject_spread.Sheets[0].ColumnHeader.Cells[7, 0].Border.BorderColorBottom = Color.White;
                //        new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                //        new_header_string_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString();
                //        new_header_string_split = new_header_string.Split(',');
                //        subject_spread.Sheets[0].SheetCorner.RowCount = subject_spread.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
                //    }
                //}
                //=====================================
                // subject_spread.Sheets[0].ColumnHeader.RowCount = subject_spread.Sheets[0].ColumnHeader.RowCount + 2;
                subject_spread.Sheets[0].SheetCorner.Columns[0].Visible = false;
                subject_spread.Sheets[0].ColumnCount = 4;
                subject_spread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Left;
                subject_spread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 2, 0].Text = "S.No";
                subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 2, 1].Text = "Roll No";
                subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 2, 2].Text = "Reg No";
                subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 2, 3].Text = "Student Name";
                subject_spread.Sheets[0].SheetCornerSpanModel.Add(subject_spread.Sheets[0].ColumnHeader.RowCount - 2, 0, 2, 1);
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(subject_spread.Sheets[0].ColumnHeader.RowCount - 2, 0, 2, 1);
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(subject_spread.Sheets[0].ColumnHeader.RowCount - 2, 1, 2, 1);
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(subject_spread.Sheets[0].ColumnHeader.RowCount - 2, 2, 2, 1);
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(subject_spread.Sheets[0].ColumnHeader.RowCount - 2, 3, 2, 1);
                no_of_hrs = int.Parse(ds_student.Tables[0].Rows[0]["PER DAY"].ToString());
                mng_hrs = int.Parse(ds_student.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString());
                evng_hrs = int.Parse(ds_student.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString());
                order = ds_student.Tables[0].Rows[0]["order"].ToString();
                sem_start_date = ds_student.Tables[0].Rows[0]["start_date"].ToString();
                FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                subject_spread.Sheets[0].Columns[1].CellType = textcel_type;
                subject_spread.Sheets[0].Columns[2].CellType = textcel_type;
                if (no_of_hrs > 0)
                {
                    for (temp_stud_count = 0; temp_stud_count < stud_count; temp_stud_count++)
                    {
                        subject_spread.Sheets[0].RowCount++;
                        row_count = subject_spread.Sheets[0].RowCount - 1;
                        string admdate = ds_student.Tables[0].Rows[row_count]["adm_date"].ToString();
                        string[] admdatesp = admdate.Split(new Char[] { '/' });
                        admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
                        // Admission_date = Convert.ToDateTime(admdate);
                        subject_spread.Sheets[0].Rows[subject_spread.Sheets[0].RowCount - 1].Visible = false;
                        subject_spread.Sheets[0].Cells[row_count, 0].Text = (temp_stud_count + 1).ToString();
                        subject_spread.Sheets[0].Cells[row_count, 1].Text = ds_student.Tables[0].Rows[row_count]["ROLL NO"].ToString();
                        subject_spread.Sheets[0].Cells[row_count, 1].Tag = ds_student.Tables[0].Rows[row_count]["ROLL NO"].ToString();
                        subject_spread.Sheets[0].Cells[row_count, 1].Note = admdate.ToString();
                        subject_spread.Sheets[0].Cells[row_count, 2].Text = ds_student.Tables[0].Rows[row_count]["REG NO"].ToString();
                        subject_spread.Sheets[0].Cells[row_count, 0].HorizontalAlign = HorizontalAlign.Center;
                        subject_spread.Sheets[0].Cells[row_count, 1].HorizontalAlign = HorizontalAlign.Left;
                        subject_spread.Sheets[0].Cells[row_count, 2].HorizontalAlign = HorizontalAlign.Left;
                        subject_spread.Sheets[0].Cells[row_count, 3].HorizontalAlign = HorizontalAlign.Left;
                        has_load_rollno.Add(ds_student.Tables[0].Rows[row_count]["ROLL NO"].ToString().ToLower(), 0);
                        has_total_attnd_hour.Add(ds_student.Tables[0].Rows[row_count]["ROLL NO"].ToString().ToLower(), 0);
                        has_total_absent_hour.Add(ds_student.Tables[0].Rows[row_count]["ROLL NO"].ToString().ToLower(), 0);
                        string stud_name_value_new = Convert.ToString(ds_student.Tables[0].Rows[row_count]["STUD NAME"]);
                        subject_spread.Sheets[0].Cells[row_count, 3].Text = Convert.ToString(stud_name_value_new).Trim();
                    }
                    load_attendance();
                }
                else
                {
                    errlbl.Visible = true;
                    errlbl.Text = "Update Master Setting";
                }
            }
            else
            {
                errlbl.Visible = true;
                errlbl.Text = "No Student(s) Available";
            }
        }
        catch
        {
        }
    }

    public string filterfunction()
    {
        string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
        string strorder = "ORDER BY r.roll_no";
        markorder = "ORDER BY registration.roll_no";
        string serialno = dacc.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
        if (serialno == "1")
        {
            strorder = "ORDER BY r.serialno";
            markorder = "ORDER BY registration.serialno";
        }
        else
        {
            if (orderby_Setting == "0")
            {
                strorder = "ORDER BY r.roll_no";
                markorder = "ORDER BY registration.roll_no";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "ORDER BY r.Reg_No";
                markorder = "ORDER BY registration.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strorder = "ORDER BY r.Stud_Name";
                markorder = "ORDER BY registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "ORDER BY r.roll_no,r.Reg_No,r.Stud_Name";
                markorder = "ORDER BY registration.roll_no,registration.Reg_No,registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "ORDER BY r.roll_no,r.Reg_No";
                markorder = "ORDER BY registration.roll_no,registration.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "ORDER BY r.Reg_No,r.Stud_Name";
                markorder = "ORDER BY registration.Reg_No,registration.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "ORDER BY r.roll_no,r.Stud_Name";
                markorder = "ORDER BY registration.roll_no,registration.Stud_Name";
            }
        }
        return strorder;
    }

    public void load_attendance()
    {
        try
        {
            string sections = string.Empty;
            Hashtable hatonduty = new Hashtable();
            DataSet dsonduty = new DataSet();
            Hashtable hatodtot = new Hashtable();
            string strondutyquery = string.Empty;
            //sections = ddlsec.SelectedItem.ToString();
            if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1")
            {
                sections = string.Empty;
            }
            else
            {
                sections = ddlsec.SelectedItem.ToString();
            }
            DataSet dsmark = new DataSet();
            DataView dvmark = new DataView();
            //ddlsubject.Items.Insert(0, "--Select--");
            filterfunction();
            if (ddltest.Items.Count > 0)
            {
                string filterwithsection = "e.criteria_no ='" + ddltest.SelectedItem.Value.ToString() + "' and e.sections='" + sections.ToString() + "' and e.subject_no='" + ddlsubject.SelectedValue.ToString() + "' and e.exam_code = r.exam_code And registration.roll_no = r.roll_no And registration.RollNo_Flag <> 0 and registration.cc=0 and registration.delflag=0 and registration.exam_flag <> 'DEBAR' " + markorder + " ";
                string filterwithoutsection = "e.criteria_no ='" + ddltest.SelectedItem.Value.ToString() + "' and e.subject_no='" + ddlsubject.SelectedValue.ToString() + "' and e.exam_code = r.exam_code And registration.roll_no = r.roll_no And registration.RollNo_Flag <> 0 and registration.cc=0 and registration.delflag=0 and registration.exam_flag <> 'DEBAR' " + markorder + "";
                int stud_count = 0;
                hat.Clear();
                hat.Add("criteria_no", ddltest.SelectedItem.Value.ToString());
                hat.Add("subjectno", ddlsubject.SelectedValue.ToString());
                hat.Add("strsec", sections.ToString());
                hat.Add("filterwithsection", filterwithsection.ToString());
                hat.Add("filterwithoutsection", filterwithoutsection.ToString());
                dsmark = d2.select_method("SELECT_ALL_STUDENT_ONE_TEST", hat, "sp");
            }
            Hashtable hatattendance = new Hashtable();
            //added By Srinath 14/8/2013
            string strorder = filterfunction(); ;
            string rstrsec = string.Empty;
            // string[] final_date_string=new string[2];

            temp_date = dt1;
            subject_no = ddlsubject.SelectedValue.ToString();
            string splhrsec = string.Empty;
            string sectionName = string.Empty;
            if (ddlsec.SelectedValue.ToString() == "" || ddlsec.SelectedValue.ToString() == "-1")
            {
                strsec = string.Empty;
                rstrsec = string.Empty;
                splhrsec = string.Empty;
                sectionName = string.Empty;
            }
            else
            {
                strsec = " and sections='" + ddlsec.SelectedItem.ToString() + "'";
                rstrsec = " and r.sections='" + ddlsec.SelectedItem.ToString() + "'";
                splhrsec = "and sections='" + ddlsec.SelectedItem.ToString() + "'";
                sectionName = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
            }
            string stralldetaisquery = "select r.roll_no,s.subject_no,s.batch,r.adm_date from registration r,subjectchooser s where s.roll_no=r.roll_no and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and s.subject_no='" + ddlsubject.SelectedValue.ToString() + "' " + rstrsec + "";
            stralldetaisquery = stralldetaisquery + " ;select r.roll_no,s.subject_no,s.batch,r.adm_date,s.fromdate from registration r,subjectchooser_new s where s.roll_no=r.roll_no and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and s.subject_no='" + ddlsubject.SelectedValue.ToString() + "' " + rstrsec + "";
            stralldetaisquery = stralldetaisquery + " ;select day_value,hour_value,stu_batch,subject_no,timetablename from laballoc where batch_year='" + ddlbatch.SelectedValue.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and subject_no='" + ddlsubject.SelectedValue.ToString() + "' " + strsec + "";
            stralldetaisquery = stralldetaisquery + " ;select day_value,hour_value,stu_batch,subject_no,fdate from laballoc_new where batch_year='" + ddlbatch.SelectedValue.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and subject_no='" + ddlsubject.SelectedValue.ToString() + "' " + strsec + "";
            stralldetaisquery = stralldetaisquery + " ;select a.* from attendance a,registration r where a.roll_no=r.roll_no and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' " + rstrsec + "";
            stralldetaisquery = stralldetaisquery + " ;select a.* from attendance_withreason a,registration r where a.roll_no=r.roll_no and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' " + rstrsec + "";
            stralldetaisquery = stralldetaisquery + " ;--select * from Semester_Schedule where batch_year='" + ddlbatch.SelectedValue.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester='" + ddlduration.SelectedItem.ToString() + "'  " + strsec + " order by FromDate desc";
            stralldetaisquery = stralldetaisquery + " ; select * from Alternate_Schedule where batch_year='" + ddlbatch.SelectedValue.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester='" + ddlduration.SelectedItem.ToString() + "'  " + strsec + "  order by FromDate desc";
            DataSet dsalldetails = d2.select_method_wo_parameter(stralldetaisquery, "Text");

            DataTable dtAlterSchedule = new DataTable();
            DataTable dtSchedule = new DataTable();
            DateTime dtFromDate1 = new DateTime();
            DateTime dtToDate1 = new DateTime();
            DateTime.TryParseExact(Convert.ToString(txtFromDate.Text).Trim(), "d/M/yyyy", null, DateTimeStyles.None, out dtFromDate1);
            DateTime.TryParseExact(Convert.ToString(txtToDate.Text).Trim(), "d/M/yyyy", null, DateTimeStyles.None, out dtToDate1);
            int intNHrs = 0;
            SemesterandAlternateSchedule(Convert.ToString(Session["collegecode"]).Trim(), Convert.ToString(ddlbatch.SelectedValue).Trim(), Convert.ToString(ddlbranch.SelectedValue).Trim(), Convert.ToString(ddlduration.SelectedValue).Trim(), sectionName, dtFromDate1.ToString("MM/dd/yyyy"), dtToDate1.ToString("MM/dd/yyyy"), no_of_hrs, ref  dtSchedule, ref  dtAlterSchedule);

            dsalldetails.Tables.Add(dtSchedule);
            dsalldetails.Tables.Add(dtAlterSchedule);
            //==================================
            //modified By Srinath 22/2/2013 ====Start 
            if (chkflag == false)
            {
                chkflag = true;
                //Hashtable has_attnd_masterset_notconsider = new Hashtable(); //Hiden By Srinath 22/2/2013
                //---------------get calcflag
                has.Clear();
                has.Add("colege_code", Session["collegecode"].ToString());
                ds_attndmaster = dacc.select_method("ATT_MASTER_SETTING", has, "sp");
                count_master = (ds_attndmaster.Tables[0].Rows.Count);
                if (count_master > 0)
                {
                    for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                    {
                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
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
                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                        {
                            if (!hatabsentvalues.ContainsKey(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                            {
                                hatabsentvalues.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString());
                            }
                        }
                    }
                }
                //=====================================12/5/12 PRABHA
                //added By srinath 18/2/2013 ==start
                string[] fromdatespit = txtFromDate.Text.Split('/');
                string[] todatespit = txtToDate.Text.Split('/');
                DateTime spfromdate = Convert.ToDateTime(fromdatespit[1] + '/' + fromdatespit[0] + '/' + fromdatespit[2]);
                DateTime sptodate = Convert.ToDateTime(todatespit[1] + '/' + todatespit[0] + '/' + todatespit[2]);
                ht_sphr.Clear();
                string hrdetno = string.Empty;
                string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " " + splhrsec + " and date between '" + spfromdate.ToString() + "' and '" + sptodate.ToString() + "' and sd.subject_no='" + ddlsubject.SelectedValue.ToString() + "' ";
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
                // End 
                //added by srinath 21/8/2013
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim().Split(';')[0] + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                //Boolean splhr_flag = false;//Hidden By Srinath
                //con.Close();
                //cmd.CommandText = "select rights from  special_hr_rights where " + grouporusercode + "";
                //cmd.Connection = con;
                //con.Open();
                //SqlDataReader dr_rights_spl_hr = cmd.ExecuteReader();
                DataTable dtSplHr = new DataTable();
                string splQ = "select rights from  special_hr_rights where " + grouporusercode + "";
                dtSplHr = dirAcc.selectDataTable(splQ);

                if (dtSplHr.Rows.Count > 0)
                {
                    //while (dr_rights_spl_hr.Read())
                    foreach (DataRow dr_rights_spl_hr in dtSplHr.Rows)
                    {
                        string spl_hr_rights = string.Empty;
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
            //===================================subject type
            //Added by Srinath 5/9/2014=========Start==========================
            string getdeteails = "select convert(nvarchar(15),s.start_date,101) as start_date,nodays,s.starting_dayorder from seminfo s,PeriodAttndSchedule p where s.degree_code=p.degree_code and p.semester=s.semester and s.semester='" + ddlduration.SelectedItem.ToString() + "' and s.batch_year='" + ddlbatch.Text.ToString() + "'  and s.degree_code='" + ddlbranch.SelectedValue.ToString() + "'";
            getdeteails = getdeteails + " ; select * from tbl_consider_day_order where semester='" + ddlduration.SelectedItem.ToString() + "' and batch_year='" + ddlbatch.Text.ToString() + "'  and degree_code='" + ddlbranch.SelectedValue.ToString() + "'";
            getdeteails = getdeteails + " ; select subject_type,LAB From sub_sem where subtype_no=(select subtype_no from subject where  subject_no='" + subject_no + "')";
            DataSet dssem = d2.select_method_wo_parameter(getdeteails, "Text");
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
                for (int dc = 0; dc < dssem.Tables[1].Rows.Count; dc++)
                {
                    DateTime dtdcf = Convert.ToDateTime(dssem.Tables[1].Rows[dc]["from_date"].ToString());
                    DateTime dtdct = Convert.ToDateTime(dssem.Tables[1].Rows[dc]["to_date"].ToString());
                    for (DateTime dtc = dtdcf; dtc <= dtdct; dtc = dtc.AddDays(1))
                    {
                        if (!hatdc.Contains(dtc))
                        {
                            hatdc.Add(dtc, dtc);
                        }
                    }
                }
            }
            catch
            {
            }
            //====================End=====================================================
            // string subj_type = GetFunction("select subject_type From sub_sem where subtype_no=(select subtype_no from subject where  subject_no='" + subject_no + "')");
            string subj_type = dssem.Tables[2].Rows[0]["LAB"].ToString();
            //       //===================================
            while (temp_date <= dt2)
            {
                //string strTempDate = temp_date.ToString("MM/dd/yyyy");
                if (!hatdc.Contains(temp_date))//Added by Srinath for day order change
                {
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
                        //temp_date = temp_date.AddDays(1); aruna 30oct2012
                    }
                    else
                    {
                        //DateTime dtTempDate = new DateTime();
                        //dtTempDate = Convert.ToDateTime(temp_date.ToString("MM/dd/yyyy"));
                        holiflag = true;
                        //ds_alter.Clear();
                        //---------------alternate schedule
                        //con.Close();
                        //con.Open();
                        //cmd_alt_shed = new SqlCommand("select  * from alternate_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedItem.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and FromDate ='" + temp_date + "' " + strsec + " order by FromDate Desc", con);
                        //SqlDataAdapter da_alter = new SqlDataAdapter(cmd_alt_shed);
                        //ds_alter.Clear();
                        //da_alter.Fill(ds_alter);
                        dsalldetails.Tables[7].DefaultView.RowFilter = "degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedItem.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and  FromDate=#" + temp_date.ToString("MM/dd/yyyy") + "# " + strsec + "";
                        DataView dvaltersech = dsalldetails.Tables[7].DefaultView;// changed by rajkumar 5/2/2018 FromDate='" + temp_date.ToString("MM/dd/yyyy") + "'
                        //---------------------------------------------
                        //ds.Clear();
                        //con.Close();
                        //con.Open();
                        //cmd_sem_shed = new SqlCommand("select top 1 * from semester_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedItem.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and FromDate <='" + temp_date + "' " + strsec + " order by FromDate Desc", con);
                        //SqlDataAdapter da = new SqlDataAdapter(cmd_sem_shed);
                        //da.Fill(ds);
                        dsalldetails.Tables[6].DefaultView.RowFilter = "degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedItem.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and  FromDate<=#" + temp_date.ToString("MM/dd/yyyy") + "# " + strsec + "";//// changed by rajkumar 5/2/2018 FromDate='" + temp_date.ToString("MM/dd/yyyy") + "'
                        DataView dvsemsech = dsalldetails.Tables[6].DefaultView;
                        hatattendance.Clear();
                        //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        if (dvsemsech.Count > 0)
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
                                    //modified by srinath 5/9/2014
                                    //strDay = findday(no_of_hrs, sem_start_date.ToString(), dummy_date);
                                    string[] sp = dummy_date.Split('/');
                                    string curdate = sp[1] + '/' + sp[0] + '/' + sp[2];
                                    strDay = d2.findday(curdate, ddlbranch.SelectedValue.ToString(), ddlduration.SelectedItem.ToString(), ddlbatch.Text.ToString(), semstartdate, noofdays, startday);
                                }
                                for (int temp_hr = split_holiday_status_1; temp_hr <= split_holiday_status_2; temp_hr++)
                                {
                                    samehr_flag = false;
                                    roll_count = 0;
                                    present_count = 0;
                                    temp_hr_field = strDay + temp_hr;
                                    date_temp_field = "d" + final_date_string[1].ToString() + "d" + temp_hr;
                                    hatattendance.Clear();
                                    //if (ds_alter.Tables[0].Rows.Count > 0)
                                    if (dvaltersech.Count > 0)
                                    {
                                        //for (int hasrow = 0; hasrow < ds_alter.Tables[0].Rows.Count; hasrow++)
                                        for (int hasrow = 0; hasrow < dvaltersech.Count; hasrow++)
                                        {
                                            full_hour = dvaltersech[hasrow][temp_hr_field].ToString();
                                            hatattendance.Clear();
                                            if (full_hour.Trim() != "")
                                            {
                                                temp_has_subj_code.Clear();
                                                string[] split_full_hour = full_hour.Split(';');
                                                for (int semi_colon = 0; semi_colon <= split_full_hour.GetUpperBound(0); semi_colon++)
                                                {
                                                    roll_count = 0;
                                                    single_hour = split_full_hour[semi_colon].ToString();
                                                    string[] split_single_hour = single_hour.Split('-');
                                                    //if (split_single_hour.GetUpperBound(0) == 2 || split_single_hour.GetUpperBound(0) == 3)//Hidden By Srinath 1/6/2013
                                                    if (split_single_hour.GetUpperBound(0) >= 1)
                                                    {
                                                        check_alter = true;
                                                        if (split_single_hour[0].ToString() == subject_no)
                                                        {
                                                            if (!temp_has_subj_code.ContainsKey(subject_no))
                                                            {
                                                                temp_has_subj_code.Add(subject_no, subject_no);
                                                                //----------------------check lab allocation
                                                                recflag = true;
                                                                roll_count = 0;
                                                                if (samehr_flag == false)
                                                                {
                                                                    span_count++;
                                                                    subject_spread.Sheets[0].ColumnCount++;
                                                                    samehr_flag = true;
                                                                }
                                                                subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 1)].Text = temp_hr.ToString();
                                                                Hashtable has_stud_list = new Hashtable();
                                                                //------------------find subject type
                                                                //subj_type = GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subject_no + "'");
                                                                subj_type = dssem.Tables[2].Rows[0]["LAB"].ToString();
                                                                //====================
                                                                if (subj_type != "1" && subj_type != "True" && subj_type != "TRUE" && subj_type.Trim().ToLower() != "true")
                                                                {
                                                                    dsalldetails.Tables[0].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subject_no + "'";
                                                                    DataView dvlabhr = dsalldetails.Tables[0].DefaultView;
                                                                    for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                                    {
                                                                        string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                        dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                        DataView dvattva = dsalldetails.Tables[4].DefaultView;
                                                                        if (dvattva.Count > 0)
                                                                        {
                                                                            string attval = dvattva[0][date_temp_field].ToString();
                                                                            if (!hatattendance.Contains(rollno.ToString()))
                                                                            {
                                                                                hatattendance.Add(rollno.ToString(), attval);
                                                                            }
                                                                        }
                                                                    }
                                                                    //string strquery = "select r.roll_no,a." + date_temp_field + " as attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from registration r ,subjectchooser s,attendance a  where r.roll_no=a.roll_no and s.roll_no=a.roll_no and a.month_year='" + month_year + "' and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=s.roll_no and s.semester= " + ddlduration.SelectedItem.ToString() + " " + strsec + " and  subject_no=" + subject_no + " " + strorder + "";
                                                                    //strondutyquery = "select r.roll_no,a." + date_temp_field + " as attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from registration r ,subjectchooser s,attendance_withreason a  where r.roll_no=a.roll_no and s.roll_no=a.roll_no and a.month_year='" + month_year + "' and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=s.roll_no and s.semester= " + ddlduration.SelectedItem.ToString() + " " + strsec + " and  subject_no=" + subject_no + " " + strorder + "";
                                                                    //DataSet dsquery = d2.select_method(strquery, hat, "Text");
                                                                    //for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                    //{
                                                                    //    string rollno = dsquery.Tables[0].Rows[i]["roll_no"].ToString().Trim().ToLower();
                                                                    //    if (!hatattendance.Contains(rollno))
                                                                    //    {
                                                                    //        hatattendance.Add(rollno, dsquery.Tables[0].Rows[i]["attvalue"].ToString());
                                                                    //    }
                                                                    //}
                                                                }
                                                                else
                                                                {
                                                                    //string strquery = "select r.roll_no,a." + date_temp_field + " as attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from  registration r,subjectchooser_New s,laballoc_new l,attendance a  where a.roll_no=s.roll_no and r.roll_no=a.roll_no and a.month_year='" + month_year + "' and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedItem.ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar'  " + rstrsec + " and r.roll_no=s.roll_no and s.subject_no=" + subject_no + " and r.degree_code=l.degree_code and r.batch_year=l.batch_year and l.Semester=r.Current_Semester  and s.subject_no =l.subject_no and hour_value=" + temp_hr + "  and day_value='" + strDay + "' and l.subject_no=" + subject_no + " and s.batch=l.stu_batch " + section_lab + " and FromDate ='" + temp_date + "' and l.fdate=s.fromdate " + strorder + "";
                                                                    //strondutyquery = "select r.roll_no,a." + date_temp_field + " as attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from  registration r,subjectchooser_New s,laballoc_new l,attendance_withreason a  where a.roll_no=s.roll_no and r.roll_no=a.roll_no and a.month_year='" + month_year + "' and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedItem.ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar'  " + rstrsec + " and r.roll_no=s.roll_no and s.subject_no=" + subject_no + " and r.degree_code=l.degree_code and r.batch_year=l.batch_year and l.Semester=r.Current_Semester  and s.subject_no =l.subject_no and hour_value=" + temp_hr + "  and day_value='" + strDay + "' and l.subject_no=" + subject_no + " and s.batch=l.stu_batch " + section_lab + " and FromDate ='" + temp_date + "' and l.fdate=s.fromdate " + strorder + "";
                                                                    //DataSet dsquery = d2.select_method(strquery, hat, "Text");
                                                                    //for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                    //{
                                                                    //    string rollno = dsquery.Tables[0].Rows[i]["roll_no"].ToString().Trim().ToLower();
                                                                    //    if (!hatattendance.Contains(rollno))
                                                                    //    {
                                                                    //        hatattendance.Add(rollno, dsquery.Tables[0].Rows[i]["attvalue"].ToString());
                                                                    //    }
                                                                    //}
                                                                    dsalldetails.Tables[3].DefaultView.RowFilter = "hour_value=" + temp_hr + "  and day_value='" + strDay + "' and subject_no='" + subject_no + "' and fdate='" + temp_date.ToString("MM/dd/yyyy").ToString() + "'";
                                                                    DataView dvlabbatch = dsalldetails.Tables[3].DefaultView;
                                                                    for (int lb = 0; lb < dvlabbatch.Count; lb++)
                                                                    {
                                                                        string batch = dvlabbatch[lb]["stu_batch"].ToString();
                                                                        if (batch != null && batch.Trim() != "")
                                                                        {
                                                                            dsalldetails.Tables[1].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subject_no + "' and batch='" + batch + "' and fromdate='" + temp_date.ToString("MM/dd/yyyy") + "'";
                                                                            DataView dvlabhr = dsalldetails.Tables[1].DefaultView;
                                                                            for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                                            {
                                                                                string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                                dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                                DataView dvattva = dsalldetails.Tables[4].DefaultView;
                                                                                if (dvattva.Count > 0)
                                                                                {
                                                                                    string attval = dvattva[0][date_temp_field].ToString();
                                                                                    if (!hatattendance.Contains(rollno.ToString()))
                                                                                    {
                                                                                        hatattendance.Add(rollno.ToString(), attval);
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (chkondutyspit.Checked == true)
                                                                {
                                                                    //dsonduty.Reset();
                                                                    //dsonduty.Dispose();
                                                                    //dsonduty = d2.select_method_wo_parameter(strondutyquery, "Text");
                                                                    //for (int ro = 0; ro < dsonduty.Tables[0].Rows.Count; ro++)
                                                                    //{
                                                                    //    string odroll = dsonduty.Tables[0].Rows[ro]["roll_no"].ToString().ToLower().Trim();
                                                                    //    string odrea = dsonduty.Tables[0].Rows[ro]["attvalue"].ToString();
                                                                    //    string odkey = odroll + '-' + odrea;
                                                                    //    int odval = 1;
                                                                    //    if (odrea.Trim() != "")
                                                                    //    {
                                                                    //        if (hatonduty.Contains(odkey))
                                                                    //        {
                                                                    //            odval = Convert.ToInt32(GetCorrespondingKey(odkey, hatonduty));
                                                                    //            odval = odval + 1;
                                                                    //            hatonduty[odkey] = odval;
                                                                    //        }
                                                                    //        else
                                                                    //        {
                                                                    //            hatonduty.Add(odkey, 1);
                                                                    //        }
                                                                    //    }
                                                                    //}
                                                                    dsalldetails.Tables[0].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subject_no + "'";
                                                                    DataView dvlabhr = dsalldetails.Tables[0].DefaultView;
                                                                    for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                                    {
                                                                        string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                        dsalldetails.Tables[5].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                        DataView dvattva = dsalldetails.Tables[5].DefaultView;
                                                                        if (dvattva.Count > 0)
                                                                        {
                                                                            string odrea = dvattva[0][date_temp_field].ToString();
                                                                            string odkey = rollno + '-' + odrea;
                                                                            int odval = 1;
                                                                            if (odrea.Trim() != "")
                                                                            {
                                                                                if (hatonduty.Contains(odkey))
                                                                                {
                                                                                    odval = Convert.ToInt32(GetCorrespondingKey(odkey, hatonduty));
                                                                                    odval = odval + 1;
                                                                                    hatonduty[odkey] = odval;
                                                                                }
                                                                                else
                                                                                {
                                                                                    hatonduty.Add(odkey, 1);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (hatattendance.Count > 0)
                                                                {
                                                                    for (int i = 0; i < subject_spread.Sheets[0].RowCount; i++)
                                                                    {
                                                                        string rollno = subject_spread.Sheets[0].Cells[i, 1].Text.ToString().Trim().ToLower();
                                                                        if (hatattendance.Contains(rollno.ToString()))
                                                                        {
                                                                            no_stud_flag = true;
                                                                            Admission_date = Convert.ToDateTime(subject_spread.Sheets[0].Cells[i, 1].Note.Trim());
                                                                            string attvalue = Convert.ToString(GetCorrespondingKey(rollno, hatattendance));
                                                                            string value = Attmark(attvalue.ToString());
                                                                            if (temp_date >= Admission_date)
                                                                            {
                                                                                subject_spread.Sheets[0].Rows[i].Visible = true;
                                                                                subject_spread.Sheets[0].Cells[i, (subject_spread.Sheets[0].ColumnCount - 1)].Text = value;
                                                                                if (value.Trim().ToLower() == "a")
                                                                                {
                                                                                    subject_spread.Sheets[0].Cells[i, (subject_spread.Sheets[0].ColumnCount - 1)].ForeColor = Color.Red;
                                                                                }
                                                                                if (subject_spread.Sheets[0].Cells[i, (subject_spread.Sheets[0].ColumnCount - 1)].Text == "HS")//====9/6/12 PRABHA
                                                                                {
                                                                                    if (!has_hs.ContainsKey((subject_spread.Sheets[0].ColumnCount - 1)))
                                                                                    {
                                                                                        has_hs.Add((subject_spread.Sheets[0].ColumnCount - 1), (subject_spread.Sheets[0].ColumnCount - 1));
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
                                                                                                if (chkonduty.Checked == true)
                                                                                                {
                                                                                                    if (getval.ToString() == "0")
                                                                                                    {
                                                                                                        present_count = Convert.ToInt16(GetCorrespondingKey(subject_spread.Sheets[0].Cells[i, 1].Text.ToString().Trim().ToLower(), has_load_rollno));
                                                                                                        present_count++;
                                                                                                        has_load_rollno[subject_spread.Sheets[0].Cells[i, 1].Text.ToString().ToLower().Trim()] = present_count;
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    if (attvalue.ToString() != "3")
                                                                                                    {
                                                                                                        if (getval.ToString() == "0")
                                                                                                        {
                                                                                                            present_count = Convert.ToInt16(GetCorrespondingKey(subject_spread.Sheets[0].Cells[i, 1].Text.ToString().Trim().ToLower(), has_load_rollno));
                                                                                                            present_count++;
                                                                                                            has_load_rollno[subject_spread.Sheets[0].Cells[i, 1].Text.ToString().ToLower().Trim()] = present_count;
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            if (value != "NE")
                                                                                            {
                                                                                                present_count = Convert.ToInt16(GetCorrespondingKey(subject_spread.Sheets[0].Cells[i, 1].Text.ToString().ToLower().Trim(), has_total_attnd_hour));
                                                                                                present_count++;
                                                                                                has_total_attnd_hour[subject_spread.Sheets[0].Cells[i, 1].Text.ToString().ToLower().Trim()] = present_count;
                                                                                            }
                                                                                            if (attvalue.ToString() == "3")
                                                                                            {
                                                                                                if (!hatodtot.Contains(rollno.ToLower()))
                                                                                                {
                                                                                                    hatodtot.Add(rollno.ToLower(), "1");
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    double getods = Convert.ToDouble(GetCorrespondingKey(rollno, hatodtot));
                                                                                                    getods = getods + 1;
                                                                                                    hatodtot[rollno.ToLower()] = getods;
                                                                                                }
                                                                                            }
                                                                                            //Added By SRinath 8/2/2014
                                                                                            if (hatabsentvalues.ContainsKey(attvalue.ToString()))
                                                                                            {
                                                                                                double getads = Convert.ToDouble(GetCorrespondingKey(rollno.ToLower(), has_total_absent_hour));
                                                                                                getads = getads + 1;
                                                                                                has_total_absent_hour[rollno.ToLower()] = getads;
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
                                    samehr_flag = false;
                                    roll_count = 0;
                                    present_count = 0;
                                    if (check_alter == false)
                                    {
                                        //full_hour = ds.Tables[0].Rows[0][temp_hr_field].ToString();
                                        full_hour = dvsemsech[0][temp_hr_field].ToString();
                                        if (full_hour.Trim() != "")
                                        {
                                            temp_has_subj_code.Clear();
                                            string[] split_full_hour_sem = full_hour.Split(';');
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
                                                                span_count++;
                                                                subject_spread.Sheets[0].ColumnCount++;
                                                                samehr_flag = true;
                                                            }
                                                            subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 1)].Text = temp_hr.ToString();
                                                            Hashtable has_stud_list = new Hashtable();
                                                            //------------------find subject type
                                                            //subj_type = GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subject_no + "'");
                                                            subj_type = dssem.Tables[2].Rows[0]["LAB"].ToString();
                                                            if (subj_type != "1" && subj_type != "True" && subj_type != "TRUE" && subj_type.Trim().ToLower() != "true")
                                                            {
                                                                dsalldetails.Tables[0].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subject_no + "'";
                                                                DataView dvlabhr = dsalldetails.Tables[0].DefaultView;
                                                                for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                                {
                                                                    string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                    dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                    DataView dvattva = dsalldetails.Tables[4].DefaultView;
                                                                    if (dvattva.Count > 0)
                                                                    {
                                                                        string attval = dvattva[0][date_temp_field].ToString();
                                                                        if (!hatattendance.Contains(rollno.ToString()))
                                                                        {
                                                                            hatattendance.Add(rollno.ToString(), attval);
                                                                        }
                                                                    }
                                                                }
                                                                //string strquery = "select r.roll_no,a." + date_temp_field + " as attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from registration r ,subjectchooser s,attendance a where r.roll_no=a.roll_no and a.roll_no=s.roll_no and a.month_year='" + month_year + "' and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=s.roll_no and s.semester= " + ddlduration.SelectedItem.ToString() + " " + strsec + " and  subject_no=" + subject_no + " " + strorder + "";
                                                                //strondutyquery = "select r.roll_no,a." + date_temp_field + " as attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from registration r ,subjectchooser s,attendance_withreason a where r.roll_no=a.roll_no and a.roll_no=s.roll_no and a.month_year='" + month_year + "' and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=s.roll_no and s.semester= " + ddlduration.SelectedItem.ToString() + " " + strsec + " and  subject_no=" + subject_no + " " + strorder + "";
                                                                //DataSet dsquery = d2.select_method(strquery, hat, "Text");
                                                                //for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                //{
                                                                //    string rollno = dsquery.Tables[0].Rows[i]["Roll_no"].ToString().Trim().ToLower();
                                                                //    if (!hatattendance.Contains(rollno))
                                                                //    {
                                                                //        hatattendance.Add(rollno, dsquery.Tables[0].Rows[i]["attvalue"].ToString());
                                                                //    }
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                dsalldetails.Tables[2].DefaultView.RowFilter = "hour_value=" + temp_hr + " and subject_no='" + subject_no + "'  and day_value='" + strDay + "' and timetablename='" + dvsemsech[0]["ttname"].ToString() + "'";
                                                                DataView dvlabbatch = dsalldetails.Tables[2].DefaultView;
                                                                for (int lb = 0; lb < dvlabbatch.Count; lb++)
                                                                {
                                                                    string batch = dvlabbatch[lb]["stu_batch"].ToString();
                                                                    if (batch != null && batch.Trim() != "")
                                                                    {
                                                                        dsalldetails.Tables[0].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subject_no + "' and batch='" + batch + "' ";
                                                                        DataView dvlabhr = dsalldetails.Tables[0].DefaultView;
                                                                        for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                                        {
                                                                            string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                            dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                            DataView dvattva = dsalldetails.Tables[4].DefaultView;
                                                                            if (dvattva.Count > 0)
                                                                            {
                                                                                string attval = dvattva[0][date_temp_field].ToString();
                                                                                if (!hatattendance.Contains(rollno.ToString()))
                                                                                {
                                                                                    hatattendance.Add(rollno.ToString(), attval);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                //string strquery = "select r.roll_no,a." + date_temp_field + " AS attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from  registration r,subjectchooser s,laballoc l,attendance a  where r.roll_no=a.roll_no and a.roll_no=s.roll_no and a.month_year='" + month_year + "' and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedItem.ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar'  " + rstrsec + " and r.roll_no=s.roll_no and s.subject_no=" + subject_no + " and r.degree_code=l.degree_code and r.batch_year=l.batch_year and l.Semester=r.Current_Semester  and s.subject_no =l.subject_no and hour_value=" + temp_hr + "  and day_value='" + strDay + "' and l.subject_no=" + subject_no + " " + section_lab + " and s.batch=l.stu_batch " + strorder + "";
                                                                //strondutyquery = "select r.roll_no,a." + date_temp_field + " AS attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from  registration r,subjectchooser s,laballoc l,attendance_withreason a  where r.roll_no=a.roll_no and a.roll_no=s.roll_no and a.month_year='" + month_year + "' and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedItem.ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar'  " + rstrsec + " and r.roll_no=s.roll_no and s.subject_no=" + subject_no + " and r.degree_code=l.degree_code and r.batch_year=l.batch_year and l.Semester=r.Current_Semester  and s.subject_no =l.subject_no and hour_value=" + temp_hr + "  and day_value='" + strDay + "' and l.subject_no=" + subject_no + " " + section_lab + " and s.batch=l.stu_batch " + strorder + "";
                                                                //DataSet dsquery = d2.select_method(strquery, hat, "Text");
                                                                //for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                //{
                                                                //    string rollno = dsquery.Tables[0].Rows[i]["Roll_no"].ToString().Trim().ToLower();
                                                                //    if (!hatattendance.Contains(rollno.ToString()))
                                                                //    {
                                                                //        hatattendance.Add(rollno.ToString(), dsquery.Tables[0].Rows[i]["attvalue"].ToString());
                                                                //    }
                                                                //}
                                                            }
                                                            if (chkondutyspit.Checked == true)
                                                            {
                                                                dsalldetails.Tables[0].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subject_no + "'";
                                                                DataView dvlabhr = dsalldetails.Tables[0].DefaultView;
                                                                for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                                {
                                                                    string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                    dsalldetails.Tables[5].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                    DataView dvattva = dsalldetails.Tables[5].DefaultView;
                                                                    if (dvattva.Count > 0)
                                                                    {
                                                                        string odrea = dvattva[0][date_temp_field].ToString();
                                                                        string odkey = rollno + '-' + odrea;
                                                                        int odval = 1;
                                                                        if (odrea.Trim() != "")
                                                                        {
                                                                            if (hatonduty.Contains(odkey))
                                                                            {
                                                                                odval = Convert.ToInt32(GetCorrespondingKey(odkey, hatonduty));
                                                                                odval = odval + 1;
                                                                                hatonduty[odkey] = odval;
                                                                            }
                                                                            else
                                                                            {
                                                                                hatonduty.Add(odkey, 1);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                //dsonduty.Reset();
                                                                //dsonduty.Dispose();
                                                                //dsonduty = d2.select_method_wo_parameter(strondutyquery, "Text");
                                                                //for (int ro = 0; ro < dsonduty.Tables[0].Rows.Count; ro++)
                                                                //{
                                                                //    string odroll = dsonduty.Tables[0].Rows[ro]["roll_no"].ToString().ToLower().Trim();
                                                                //    string odrea = dsonduty.Tables[0].Rows[ro]["attvalue"].ToString();
                                                                //    string odkey = odroll + '-' + odrea;
                                                                //    int odval = 1;
                                                                //    if (odrea.Trim() != "")
                                                                //    {
                                                                //        if (hatonduty.Contains(odkey))
                                                                //        {
                                                                //            odval = Convert.ToInt32(GetCorrespondingKey(odkey, hatonduty));
                                                                //            odval = odval + 1;
                                                                //            hatonduty[odkey] = odval;
                                                                //        }
                                                                //        else
                                                                //        {
                                                                //            hatonduty.Add(odkey, 1);
                                                                //        }
                                                                //    }
                                                                //}
                                                            }
                                                            if (hatattendance.Count > 0)
                                                            {
                                                                for (int i = 0; i < subject_spread.Sheets[0].RowCount; i++)
                                                                {
                                                                    string rollno = subject_spread.Sheets[0].Cells[i, 1].Text.ToString().ToLower().Trim();
                                                                    if (hatattendance.Contains(rollno.ToString()))
                                                                    {
                                                                        no_stud_flag = true;
                                                                        Admission_date = Convert.ToDateTime(subject_spread.Sheets[0].Cells[i, 1].Note.Trim());
                                                                        string attvalue = GetCorrespondingKey(rollno, hatattendance).ToString();
                                                                        string value = Attmark(attvalue.ToString()).Trim();
                                                                        if (temp_date >= Admission_date)
                                                                        {
                                                                            subject_spread.Sheets[0].Rows[i].Visible = true;
                                                                            subject_spread.Sheets[0].Cells[i, (subject_spread.Sheets[0].ColumnCount - 1)].Text = value;
                                                                            if (value.Trim().ToLower() == "a")
                                                                            {
                                                                                subject_spread.Sheets[0].Cells[i, (subject_spread.Sheets[0].ColumnCount - 1)].ForeColor = Color.Red;
                                                                            }
                                                                            if (subject_spread.Sheets[0].Cells[i, (subject_spread.Sheets[0].ColumnCount - 1)].Text == "HS")
                                                                            {
                                                                                if (!has_hs.ContainsKey((subject_spread.Sheets[0].ColumnCount - 1)))
                                                                                {
                                                                                    has_hs.Add((subject_spread.Sheets[0].ColumnCount - 1), (subject_spread.Sheets[0].ColumnCount - 1));
                                                                                }
                                                                            }
                                                                            if ((attvalue.ToString()) != "8")
                                                                            {
                                                                                if (value != "HS")
                                                                                {
                                                                                    if (!has_attnd_masterset_notconsider.ContainsKey(attvalue.ToString()))//==31/5/12 PRABHA
                                                                                    {
                                                                                        if (has_attnd_masterset.ContainsKey(attvalue.ToString()))
                                                                                        {
                                                                                            string getval = Convert.ToString(GetCorrespondingKey(attvalue, has_attnd_masterset));
                                                                                            if (chkonduty.Checked == true)
                                                                                            {
                                                                                                if (getval.ToString() == "0")
                                                                                                {
                                                                                                    present_count = Convert.ToInt16(GetCorrespondingKey(subject_spread.Sheets[0].Cells[i, 1].Text.ToString().Trim().ToLower().Trim(), has_load_rollno));
                                                                                                    present_count++;
                                                                                                    has_load_rollno[subject_spread.Sheets[0].Cells[i, 1].Text.ToString().ToLower().Trim()] = present_count;
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                if (attvalue.ToString() != "3")
                                                                                                {
                                                                                                    if (getval.ToString() == "0")
                                                                                                    {
                                                                                                        present_count = Convert.ToInt16(GetCorrespondingKey(subject_spread.Sheets[0].Cells[i, 1].Text.ToString().ToLower().Trim(), has_load_rollno));
                                                                                                        present_count++;
                                                                                                        has_load_rollno[subject_spread.Sheets[0].Cells[i, 1].Text.ToString().ToLower().Trim()] = present_count;
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        if (value != "NE")
                                                                                        {
                                                                                            present_count = Convert.ToInt16(GetCorrespondingKey(subject_spread.Sheets[0].Cells[i, 1].Text.ToString().Trim().ToLower(), has_total_attnd_hour));
                                                                                            present_count++;
                                                                                            has_total_attnd_hour[subject_spread.Sheets[0].Cells[i, 1].Text.ToString().ToLower().Trim()] = present_count;
                                                                                        }
                                                                                        if (attvalue.ToString() == "3")
                                                                                        {
                                                                                            if (!hatodtot.Contains(rollno.ToLower()))
                                                                                            {
                                                                                                hatodtot.Add(rollno.ToLower(), "1");
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                double getods = Convert.ToDouble(GetCorrespondingKey(rollno, hatodtot));
                                                                                                getods = getods + 1;
                                                                                                hatodtot[rollno.ToLower()] = getods;
                                                                                            }
                                                                                        }
                                                                                        if (hatabsentvalues.ContainsKey(attvalue.ToString()))
                                                                                        {
                                                                                            double getads = Convert.ToDouble(GetCorrespondingKey(rollno.ToLower(), has_total_absent_hour));
                                                                                            getads = getads + 1;
                                                                                            has_total_absent_hour[rollno.ToLower()] = getads;
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
                    if (splhr_flag == true)
                    {
                        //added By srinath 13/2/2013 ===start
                        if (ht_sphr.Contains(Convert.ToString(temp_date)))
                        {
                            getspecial_hr();
                        }
                    }
                    if (span_count != 0)
                    {
                        subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - (span_count)), 1, span_count);
                        subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - (span_count))].Text = temp_date.ToString("dd") + "/" + temp_date.ToString("MM") + "/" + temp_date.ToString("yyyy");
                    }
                }//Add by Srinath 5/9/2014 for day order change==
                temp_date = temp_date.AddDays(1);
            }
            if (recflag == true || spl_hr_flag == true)
            {
                double attnd_hr = 0, tot_hr = 0, onduty = 0;
                subject_spread.Sheets[0].ColumnCount = subject_spread.Sheets[0].ColumnCount + 3;
                subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - 3)].Text = "Conducted Period(s)";
                subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - 2)].Text = "Present Period(s)";
                subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - 1)].Text = "Absent Period(s)";
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - 3), 2, 1);
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - 2), 2, 1);
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - 1), 2, 1);
                int ondutcount = 0;
                Boolean odchflag = false;
                if (chkondutyspit.Checked == true)
                {
                    for (int spon = 0; spon < chklsonduty.Items.Count; spon++)
                    {
                        if (chklsonduty.Items[spon].Selected == true)
                        {
                            ondutcount++;
                            odchflag = true;
                            subject_spread.Sheets[0].ColumnCount++;
                            if (ondutcount == 1)
                            {
                                subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - 1)].Text = "Onduty Period(s)";
                            }
                            string onname = chklsonduty.Items[spon].Text.ToString();
                            subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 1)].Text = onname;
                        }
                    }
                    if (ondutcount > 0)
                    {
                        subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - (ondutcount)), 1, ondutcount);
                    }
                    else
                    {
                        subject_spread.Sheets[0].ColumnCount++;
                        subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - 1)].Text = "Onduty Period(s)";
                        subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - 1), 2, 1);
                    }
                }
                else
                {
                    subject_spread.Sheets[0].ColumnCount++;
                    subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - 1)].Text = "Onduty Period(s)";
                    subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - 1), 2, 1);
                }
                subject_spread.Sheets[0].ColumnCount++;
                subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - 1)].Text = "Total Present Period(s)";
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - 1), 2, 1);
                subject_spread.Sheets[0].ColumnCount++;
                subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - 1)].Text = "Percentage";
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - 1), 2, 1);
                subject_spread.Sheets[0].ColumnCount++;
                subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - 1)].Text = "Mark";
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(subject_spread.Sheets[0].ColumnHeader.RowCount - 2, (subject_spread.Sheets[0].ColumnCount - 1), 2, 1);
                attnd_hr = 0;
                tot_hr = 0;
                onduty = 0;
                //if(dsmark.Tables.Count>0)
                //    {
                for (int row_cnt = 0; row_cnt < subject_spread.Sheets[0].RowCount; row_cnt++)
                {
                    roll_no = subject_spread.Sheets[0].Cells[row_cnt, 1].Text.ToString().ToLower().Trim();
                    //added by gowtham 
                    string mark = string.Empty;
                    int fail = 0;
                    if (dsmark.Tables.Count > 0)
                    {
                        if (dsmark.Tables[0].Rows.Count > 0)
                        {
                            dsmark.Tables[0].DefaultView.RowFilter = "roll_no='" + roll_no + "'";
                            dvmark = dsmark.Tables[0].DefaultView;
                            if (dvmark.Count > 0)
                            {
                                mark = Convert.ToString(dvmark[0]["marks_obtained"]);
                                if (double.Parse(mark) < 0)
                                {
                                    switch (mark)
                                    {
                                        case "-1":
                                            mark = "AAA";
                                            break;
                                        case "-2":
                                            mark = "EL";
                                            break;
                                        case "-3":
                                            mark = "EOD";
                                            break;
                                        case "-4":
                                            mark = "ML";
                                            break;
                                        case "-5":
                                            mark = "SOD";
                                            break;
                                        case "-6":
                                            mark = "NSS";
                                            break;
                                        case "-7":
                                            mark = "NJ";
                                            break;
                                        case "-8":
                                            mark = "S";
                                            break;
                                        case "-9":
                                            mark = "L";
                                            break;
                                        case "-10":
                                            mark = "NCC";
                                            break;
                                        case "-11":
                                            mark = "HS";
                                            break;
                                        case "-12":
                                            mark = "PP";
                                            break;
                                        case "-13":
                                            mark = "SYOD";
                                            break;
                                        case "-14":
                                            mark = "COD";
                                            break;
                                        case "-15":
                                            mark = "OOD";
                                            break;
                                        case "-16":
                                            mark = "OD";
                                            break;
                                        case "-17":
                                            mark = "LA";
                                            break;
                                    }
                                    fail = 1;
                                }
                                else
                                {
                                    double mimark = Convert.ToDouble(Convert.ToString(dvmark[0]["min_mark"]));
                                    if (mimark > Convert.ToDouble(mark))
                                    {
                                        fail = 1;
                                    }
                                    else
                                    {
                                        fail = 0;
                                    }
                                }
                            }
                        }
                        if (mark == "")
                        {
                            mark = "-";
                        }
                        subject_spread.Sheets[0].Cells[row_cnt, (subject_spread.Sheets[0].ColumnCount - 1)].Text = mark;
                        subject_spread.Sheets[0].Cells[row_cnt, (subject_spread.Sheets[0].ColumnCount - 1)].HorizontalAlign = HorizontalAlign.Center;
                        if (fail == 1)
                        {
                            subject_spread.Sheets[0].Cells[row_cnt, (subject_spread.Sheets[0].ColumnCount - 1)].ForeColor = Color.Red;
                        }
                    }
                    onduty = 0;
                    if (ondutcount == 0)
                    {
                        // ondutcount = 1;
                        if (hatodtot.Contains(roll_no.ToLower()))
                        {
                            onduty = Convert.ToDouble(GetCorrespondingKey(roll_no.ToLower(), hatodtot));
                        }
                        else
                        {
                            onduty = 0;
                        }
                        subject_spread.Sheets[0].Cells[row_cnt, (subject_spread.Sheets[0].ColumnCount - 4)].Text = onduty.ToString();
                        subject_spread.Sheets[0].Cells[row_cnt, (subject_spread.Sheets[0].ColumnCount - 4)].HorizontalAlign = HorizontalAlign.Center;
                    }
                    else
                    {
                        for (int odcol = subject_spread.Sheets[0].ColumnCount - (4 + ondutcount); odcol < subject_spread.Sheets[0].ColumnCount - 2; odcol++)
                        {
                            string odcolvsl = subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 1, odcol].Text.ToString();
                            string keyval = roll_no + '-' + odcolvsl;
                            if (odcolvsl.Trim() != "")
                            {
                                if (hatonduty.Contains(keyval))
                                {
                                    Double ondutyvalue = Convert.ToDouble(GetCorrespondingKey(keyval, hatonduty));
                                    onduty = onduty + ondutyvalue;
                                    subject_spread.Sheets[0].Cells[row_cnt, odcol].Text = ondutyvalue.ToString();
                                }
                                else
                                {
                                    subject_spread.Sheets[0].Cells[row_cnt, odcol].Text = "0";
                                }
                            }
                        }
                    }
                    int columnva = ondutcount;
                    if (odchflag == false)
                    {
                        columnva = ondutcount + 1;
                    }
                    if (has_load_rollno.Contains(roll_no))
                    {
                        attnd_hr = Convert.ToDouble(GetCorrespondingKey(roll_no, has_load_rollno));
                        subject_spread.Sheets[0].Cells[row_cnt, (subject_spread.Sheets[0].ColumnCount - (5 + columnva))].Text = attnd_hr.ToString();
                        subject_spread.Sheets[0].Cells[row_cnt, (subject_spread.Sheets[0].ColumnCount - (5 + columnva))].HorizontalAlign = HorizontalAlign.Center;
                    }
                    if (has_total_attnd_hour.Contains(roll_no))
                    {
                        tot_hr = Convert.ToDouble(GetCorrespondingKey(roll_no, has_total_attnd_hour));
                        subject_spread.Sheets[0].Cells[row_cnt, (subject_spread.Sheets[0].ColumnCount - (6 + columnva))].Text = tot_hr.ToString();
                        subject_spread.Sheets[0].Cells[row_cnt, (subject_spread.Sheets[0].ColumnCount - (6 + columnva))].HorizontalAlign = HorizontalAlign.Center;
                    }
                    double absent = 0;
                    Double percentage = 0;
                    if (has_total_absent_hour.Contains(roll_no.ToLower()))
                    {
                        absent = Convert.ToDouble(GetCorrespondingKey(roll_no.ToLower(), has_total_absent_hour));
                    }
                    if (tot_hr > 0 && (tot_hr - absent) > 0)
                        percentage = Math.Round(((tot_hr - absent) / tot_hr) * 100, 2);
                    subject_spread.Sheets[0].Cells[row_cnt, (subject_spread.Sheets[0].ColumnCount - (4 + columnva))].Text = absent.ToString();
                    subject_spread.Sheets[0].Cells[row_cnt, (subject_spread.Sheets[0].ColumnCount - (4 + columnva))].HorizontalAlign = HorizontalAlign.Center;
                    if (attnd_hr == 0 && tot_hr == 0)
                    {
                        subject_spread.Sheets[0].Cells[row_cnt, (subject_spread.Sheets[0].ColumnCount - 2)].Text = "-";
                    }
                    else
                    {
                        subject_spread.Sheets[0].Cells[row_cnt, (subject_spread.Sheets[0].ColumnCount - 2)].Text = percentage.ToString();//(Math.Round(((attnd_hr / tot_hr) * 100), 2)).ToString();
                        subject_spread.Sheets[0].Cells[row_cnt, (subject_spread.Sheets[0].ColumnCount - 2)].HorizontalAlign = HorizontalAlign.Center;
                    }
                    Double maxpresent = tot_hr - absent;
                    subject_spread.Sheets[0].Cells[row_cnt, (subject_spread.Sheets[0].ColumnCount - 3)].Text = maxpresent.ToString();
                    subject_spread.Sheets[0].Cells[row_cnt, (subject_spread.Sheets[0].ColumnCount - 3)].HorizontalAlign = HorizontalAlign.Center;
                }
                int roll_no_cnt = 0;
                for (int roll_no_set = 0; roll_no_set < subject_spread.Sheets[0].RowCount; roll_no_set++)
                {
                    subject_spread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    if (subject_spread.Sheets[0].Rows[roll_no_set].Visible == true)
                    {
                        roll_no_cnt++;
                        subject_spread.Sheets[0].Cells[roll_no_set, 0].Text = roll_no_cnt.ToString();
                    }
                }
                if (Convert.ToInt32(roll_no_cnt) != 0)
                {
                    Double totalRows = 0;
                    totalRows = Convert.ToInt32(roll_no_cnt);
                    //DropDownListpage.Items.Clear();
                    if (totalRows >= 10)
                    {
                        subject_spread.Sheets[0].PageSize = 10;
                        //for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                        //{
                        //   // DropDownListpage.Items.Add((k + 10).ToString());
                        //}
                        //DropDownListpage.Items.Add("Others");
                        subject_spread.Height = 410;
                        subject_spread.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                        subject_spread.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    }
                    else if (totalRows == 0)
                    {
                        //   DropDownListpage.Items.Add("0");
                        subject_spread.Height = 200;
                    }
                    else
                    {
                        subject_spread.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                        //   DropDownListpage.Items.Add(subject_spread.Sheets[0].PageSize.ToString());
                        subject_spread.Height = 30 + (38 * Convert.ToInt32(totalRows));
                    }
                    if (Convert.ToInt32(roll_no_cnt) > 10)
                    {
                        // DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        // subject_spread.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                        //  CalculateTotalPages();
                    }
                    Session["totalPages"] = (int)Math.Ceiling(totalRows / subject_spread.Sheets[0].PageSize);
                    //  Buttontotal.Text = "Records : " + roll_no_cnt + "          Pages : " + Session["totalPages"];
                }
            }
            else
            {
                // pnl_pagesetting.Visible = false;
                subject_spread.Visible = false;
                btnxl.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                errlbl.Visible = true;
                errlbl.Text = "No Record(s) Found";
                return;
            }
            //  if (no_stud_flag == false && recflag == true)
            if (holiflag == true)
            {
                if (no_stud_flag == false)
                {
                    //   pnl_head_pageset.Visible = false;
                    //  pnl_pagesetting.Visible = false;
                    subject_spread.Visible = false;
                    btnxl.Visible = false;
                    Printcontrol.Visible = false;
                    btnprintmaster.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    errlbl.Visible = true;
                    //errlbl.Text = "Attendance Can't Be Mark";
                    errlbl.Text = "Student(s) Not Available Or Attendance Cant Be Marked";
                }
            }
            else
            {
                //pnl_pagesetting.Visible = false;
                subject_spread.Visible = false;
                btnxl.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                errlbl.Visible = true;
                //errlbl.Text = "Attendance Can't Be Mark";
                errlbl.Text = "Holiday";
            }
        }
        catch
        {
        }
    }

    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        try
        {
            IDictionaryEnumerator e = hashTable.GetEnumerator();
            while (e.MoveNext())
            {
                if (e.Key.ToString() == key.ToString())
                {
                    return e.Value;
                }
            }
        }
        catch
        {

        }
        return null;
    }

    public string Attmark(string Attstr_mark)
    {
        Att_mark = string.Empty;
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
        else
        {
            Att_mark = "NE";
        }
        //return Convert.ToInt32(Att_mark);
        return Att_mark;
    }

    public string Attvalues(string Att_str1)
    {
        string Attvalue;
        Attvalue = string.Empty;
        if (Att_str1 == "P")
        {
            Attvalue = "1";
        }
        else if (Att_str1 == "A")
        {
            Attvalue = "2";
        }
        else if (Att_str1 == "OD")
        {
            Attvalue = "3";
        }
        else if (Att_str1 == "ML")
        {
            Attvalue = "4";
        }
        else if (Att_str1 == "SOD")
        {
            Attvalue = "5";
        }
        else if (Att_str1 == "NSS")
        {
            Attvalue = "6";
        }
        else if (Att_str1 == "H")
        {
            Attvalue = "7";
        }
        else if (Att_str1 == "NJ")
        {
            Attvalue = "8";
        }
        else if (Att_str1 == "S")
        {
            Attvalue = "9";
        }
        else if (Att_str1 == "L")
        {
            Attvalue = "10";
        }
        else if (Att_str1 == "NCC")
        {
            Attvalue = "11";
        }
        else if (Att_str1 == "HS")
        {
            Attvalue = "12";
        }
        else if (Att_str1 == "PP")
        {
            Attvalue = "13";
        }
        else if (Att_str1 == "SYOD")
        {
            Attvalue = "14";
        }
        else if (Att_str1 == "COD")
        {
            Attvalue = "15";
        }
        else if (Att_str1 == "OOD")
        {
            Attvalue = "16";
        }
        else
        {
            Attvalue = "NE";
        }
        return Attvalue;
    }

    public string sem_roman(int sem)
    {
        string sql = string.Empty;
        string sem_roman = string.Empty;
        SqlDataReader rsChkSet;
        con_sem.Close();
        con_sem.Open();
        sql = "select * from inssettings where college_code=" + Session["collegecode"] + " and LinkName ='Semester Display'";
        SqlCommand cmd1 = new SqlCommand(sql, con_sem);
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

    private string findday(int no, string sdate, string todate)
    {
        int order, holino;
        holino = 0;
        string day_order = string.Empty;
        string from_date = "", tmpdate = string.Empty;
        string fdate = "", smdate = string.Empty;
        int diff_work_day = 0;
        tmpdate = sdate.ToString();
        string[] semstart_date = tmpdate.Split(new Char[] { ' ' });
        string[] sm_date = semstart_date[0].Split(new Char[] { '/' });
        smdate = sm_date[0].ToString() + "/" + sm_date[1].ToString() + "/" + sm_date[2].ToString();
        from_date = todate.ToString();
        string[] fm_date = from_date.Split(new Char[] { '/' });
        fdate = fm_date[1].ToString() + "/" + fm_date[0].ToString() + "/" + fm_date[2].ToString();
        SqlDataReader dr;
        con.Close();
        con.Open();
        cmd = new SqlCommand("select count(*) from holidaystudents where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and holiday_date between '" + sdate.ToString() + "' and  '" + fdate.ToString() + "' and halforfull='0'", con);
        dr = cmd.ExecuteReader();
        dr.Read();
        if (dr.HasRows == true)
        {
            holino = Convert.ToInt16(dr[0].ToString());
        }
        string quer = "select nodays from PeriodAttndSchedule where degree_code='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and semester='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "'";
        string nodays = GetFunction(quer);
        int no_days = Convert.ToInt32(nodays);
        DateTime dt1 = Convert.ToDateTime(smdate);
        DateTime dt2 = Convert.ToDateTime(fdate);
        TimeSpan t = dt2 - dt1;
        int days = t.Days;
        diff_work_day = days - holino;
        order = Convert.ToInt16(diff_work_day.ToString()) % no_days;
        //-----------------------------------------------------------
        order = order + 1;
        string stastdayorder = string.Empty;
        stastdayorder = GetFunction("select starting_dayorder from seminfo where degree_code='" + Convert.ToString(ddlbranch.SelectedValue).Trim() + "' and semester='" + Convert.ToString(ddlduration.SelectedValue).Trim() + "' and batch_year='" + Convert.ToString(ddlbatch.SelectedValue).Trim() + "'");
        if (Convert.ToString(stastdayorder).Trim() != "")
        {
            if ((Convert.ToString(stastdayorder).Trim() != "1") && (Convert.ToString(stastdayorder).Trim() != "0"))
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
            order = no_days;
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
        return (day_order);
        con.Close();
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        errlbl.Visible = false;
        string reoprtname = txtexcelname.Text;
        if (reoprtname.ToString().Trim() != "")
        {
            d2.printexcelreport(subject_spread, reoprtname.ToString().Trim());
            txtexcelname.Text = string.Empty;
        }
        else
        {
            errlbl.Visible = true;
            errlbl.Text = "Please Enter Your Report Name";
        }
    }

    public void binddate()
    {
        con_date.Close();
        con_date.Open();
        string str_query = "select start_date,end_date from seminfo where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "' and semester='" + ddlduration.SelectedValue.ToString() + "'";
        ds_date.Clear();
        ds_date = dirAcc.selectDataSet(str_query);
        if (ds_date.Tables.Count > 0 && ds_date.Tables[0].Rows.Count > 0)
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
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            txtFromDate.Enabled = false;
            txtToDate.Enabled = false;
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
            return string.Empty;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string batch = "", sections = "", semester = "", degreecode = "", subcolumntext = "", strsec = string.Empty;
        Boolean child_flag = false;
        int sec_index = 0, sem_index = 0;
        batch = ddlbatch.SelectedValue.ToString();
        sections = ddlsec.SelectedValue.ToString();
        semester = ddlduration.SelectedValue.ToString();
        degreecode = ddlbranch.SelectedValue.ToString();
        if (ddlsec.Items.Count > 0)
        {
            if (ddlsec.Text == "")
            {
                strsec = string.Empty;
            }
            else
            {
                if (Convert.ToString(ddlsec.SelectedItem).Trim() == "")
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " - " + Convert.ToString(ddlsec.SelectedItem).Trim();
                }
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
        Session["page_redirect_value"] = ddlbatch.SelectedIndex + "," + ddldegree.SelectedIndex + "," + ddlbranch.SelectedIndex + "," + sem_index + "," + sec_index + "," + txtFromDate.Text + "," + txtToDate.Text + "," + ddlsubject.Enabled + "," + ddlsubject.SelectedIndex;
        // first_btngo();
        btnGo_Click(sender, e);
        string clmnheadrname = string.Empty;
        int total_clmn_count = subject_spread.Sheets[0].ColumnCount;
        for (int srtcnt = 0; srtcnt < total_clmn_count; srtcnt++)
        {
            if (subject_spread.Sheets[0].Columns[srtcnt].Visible == true)
            {
                if (subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text != "")
                {
                    subcolumntext = string.Empty;
                    if (clmnheadrname == "")
                    {
                        clmnheadrname = subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
                    }
                    else
                    {
                        if (child_flag == false)
                        {
                            clmnheadrname = clmnheadrname + "," + subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
                        }
                        else
                        {
                            clmnheadrname = clmnheadrname + "$)," + subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
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
                                clmnheadrname = clmnheadrname + "* ($" + subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
                                subcolumntext = clmnheadrname + "* ($" + subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
                            }
                            else
                            {
                                clmnheadrname = clmnheadrname + "$" + subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
                                subcolumntext = clmnheadrname + "$" + subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
                            }
                        }
                    }
                    else
                    {
                        subcolumntext = subcolumntext + "$" + subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
                        clmnheadrname = clmnheadrname + "$" + subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
                    }
                }
            }
        }
        //Response.Redirect("Print_Master_Setting_new.aspx?ID=" + clmnheadrname.ToString() + ":" + "singlesubject_wise_attendance.aspx" + ":" + ddlbatch.SelectedItem.ToString() + " Batch - " + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] " + strsec + " :" + "Individual SubjectWise Attendance Report ");
        Session["redirect_query_string"] = clmnheadrname.ToString() + ":" + "singlesubject_wise_attendance.aspx" + ":" + ddlbatch.SelectedItem.ToString() + " Batch - " + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] " + strsec + " :" + "Individual SubjectWise Attendance Report";
        Response.Redirect("Print_Master_Setting_new.aspx");
    }

    public void more_column()
    {
        header_text();
        int tot_col_first = 0;
        double max_tot = 0;
        foreach (DictionaryEntry parameter2 in has_total_attnd_hour)
        {
            max_tot = Convert.ToDouble((parameter2.Value).ToString());
            if (tot_hr < max_tot)
            {
                tot_hr = max_tot;
            }
        }
        subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count].Text = coll_name;
        subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
        if (final_print_col_cnt > 3)
        {
            int dd = end_column - col_count;
            int span_col = 0;
            if (dd >= 100)
            {
                int span_col_count = 0, span_balanc = 0;
                span_col_count = dd / 100;
                span_balanc = dd % 100;
                for (span_col = 0; span_col <= dd - 100; span_col += 100)
                {
                    subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].ColumnSpan = 100;
                    subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].ColumnSpan = 100;
                    subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].ColumnSpan = 100;
                    subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].ColumnSpan = 100;
                    subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].ColumnSpan = 100;
                    subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].ColumnSpan = 100;
                    subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].ColumnSpan = 100;
                    subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].ColumnSpan = 100;
                    subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].Border.BorderColorBottom = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorTop = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorBottom = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorTop = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorBottom = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorTop = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorBottom = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorTop = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorBottom = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorTop = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorBottom = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorTop = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorBottom = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].Border.BorderColorTop = Color.White;
                }
                subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].ColumnSpan = span_balanc;
                subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].Border.BorderColorRight = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].Border.BorderColorLeft = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].ColumnSpan = span_balanc;
                subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorRight = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorLeft = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].ColumnSpan = span_balanc;
                subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorRight = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorLeft = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].ColumnSpan = span_balanc;
                subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorRight = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorLeft = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].ColumnSpan = span_balanc;
                subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorRight = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorLeft = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].ColumnSpan = span_balanc;
                subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorRight = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorLeft = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].ColumnSpan = span_balanc;
                subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorRight = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorLeft = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].ColumnSpan = span_balanc;
                subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].Border.BorderColorRight = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].Border.BorderColorLeft = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[0, col_count + span_col].Border.BorderColorBottom = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorTop = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count + span_col].Border.BorderColorBottom = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorTop = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count + span_col].Border.BorderColorBottom = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorTop = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count + span_col].Border.BorderColorBottom = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorTop = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count + span_col].Border.BorderColorBottom = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorTop = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count + span_col].Border.BorderColorBottom = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorTop = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count + span_col].Border.BorderColorBottom = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count + span_col].Border.BorderColorTop = Color.White;
            }
            else
            {
                if (isonumber != string.Empty)
                {
                    subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count - 1));
                }
                else
                {
                    subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count));
                }
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, 1, (end_column - col_count));
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(2, col_count, 1, (end_column - col_count));
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(3, col_count, 1, (end_column - col_count));
                //aruna subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, col_count, 1, (end_column - col_count));
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(7, col_count, 1, (end_column - col_count));
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(4, col_count, 1, (end_column - col_count));
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(5, col_count, 1, (end_column - col_count));
                //aruna subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, col_count, 1, (end_column - col_count));
            }
        }
        subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count].Text = address1 + "-" + address2 + "-" + address3;
        subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorTop = Color.White;
        subject_spread.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorBottom = Color.White;
        if (phoneno != "" && phoneno != null)
        {
            phone = "Phone:" + phoneno;
        }
        else
        {
            phone = string.Empty;
        }
        if (faxno != "" && faxno != null)
        {
            fax = "  Fax:" + faxno;
        }
        else
        {
            fax = string.Empty;
        }
        subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count].Text = phone + fax;
        subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorTop = Color.White;
        subject_spread.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorBottom = Color.White;
        if (email != "" && faxno != null)
        {
            email_id = "Email:" + email;
        }
        else
        {
            email_id = string.Empty;
        }
        if (website != "" && website != null)
        {
            web_add = "  Web Site:" + website;
        }
        else
        {
            web_add = string.Empty;
        }
        subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count].Text = email_id + web_add;
        subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorTop = Color.White;
        subject_spread.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorBottom = Color.White;
        if (form_name != "" && form_name != null)
        {
            subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count].Text = form_name;
            subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count].Text = "----------------------------------------------------";
        }
        if (final_print_col_cnt <= 3)
        {
            //aruna=============
            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, col_count, 1, subject_spread.Sheets[0].ColumnCount - 1);
            //==================
            subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count].Text = degree_deatil.ToString(); //"Name of the Degree & Branch:" + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "     Regulation:" + GetFunction(" select regulation from degree  where degree_code=" + ddlbranch.SelectedValue.ToString() + "");
            subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count].Text = "Academic Year:" + DateTime.Now.Year.ToString() + "Semester Number:" + ddlduration.SelectedValue.ToString();
            subject_spread.Sheets[0].ColumnHeader.Cells[8, col_count].Text = "Subject:" + GetFunction("select subject_code From subject where subject_no='" + ddlsubject.SelectedValue.ToString() + "'") + "-" + ddlsubject.SelectedItem.ToString() + "   Total Number Of Hour(s) Conducted:" + tot_hr;
            subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorTop = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorBottom = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count].Border.BorderColorTop = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, col_count].Border.BorderColorBottom = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[8, col_count].Border.BorderColorTop = Color.White;
        }
        else if (final_print_col_cnt > 3 && final_print_col_cnt <= 8)
        {
            between_visible_col_cnt = (final_print_col_cnt) / 2;
            between_visible_col_cnt_bal = (final_print_col_cnt) % 2;
            visi_col_first = 0;
            for (tot_col_first = start_column; tot_col_first < subject_spread.Sheets[0].ColumnCount - 1; tot_col_first++)//==============find first half column count
            {
                if (subject_spread.Sheets[0].Columns[tot_col_first].Visible == true)
                {
                    visi_col_first++;
                    if (visi_col_first == between_visible_col_cnt)
                    {
                        visi_col_first = tot_col_first;
                        break;
                    }
                }
            }
            //aruna=============
            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, start_column, 1, subject_spread.Sheets[0].ColumnCount - 1);
            //==================
            subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column].Text = degree_deatil.ToString(); //"Name of the Degree & Branch:" + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString();
            subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column].HorizontalAlign = HorizontalAlign.Left;
            subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column].Border.BorderColorBottom = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column].Border.BorderColorRight = Color.White;
            int tot_col_second = 0;
            for (tot_col_second = tot_col_first + 1; tot_col_second < subject_spread.Sheets[0].ColumnCount; tot_col_second++)
            {
                if (subject_spread.Sheets[0].Columns[tot_col_second].Visible == true)
                {
                    visi_col_second = tot_col_second;
                    //break;
                }
            }
            subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first].Border.BorderColorLeft = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first].Border.BorderColorRight = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first].Border.BorderColorBottom = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first].HorizontalAlign = HorizontalAlign.Left;
            subject_spread.Sheets[0].ColumnHeader.Cells[6, end_column].HorizontalAlign = HorizontalAlign.Left;
            //   subject_spread.Sheets[0].ColumnHeader.Cells[6, end_column].Text = GetFunction(" select regulation from degree  where degree_code=" + ddlbranch.SelectedValue.ToString() + "");
            subject_spread.Sheets[0].ColumnHeader.Cells[6, end_column].Border.BorderColorBottom = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column].Text = "Academic Year:" + DateTime.Now.Year.ToString();
            subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column].HorizontalAlign = HorizontalAlign.Left;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column].Border.BorderColorRight = Color.White;
            //subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first].Text = "Regulation:";
            //subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first].Text = "Semester Number:";
            subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first].Border.BorderColorTop = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first].Border.BorderColorLeft = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first].Border.BorderColorRight = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first].HorizontalAlign = HorizontalAlign.Left;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, end_column].HorizontalAlign = HorizontalAlign.Left;
            //  subject_spread.Sheets[0].ColumnHeader.Cells[7, end_column].Text = ddlduration.SelectedValue.ToString();
            subject_spread.Sheets[0].ColumnHeader.Cells[7, end_column].Border.BorderColorTop = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, end_column].Border.BorderColorBottom = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first].Border.BorderColorBottom = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column].Border.BorderColorBottom = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column].Text = "Subject:" + GetFunction("select subject_code From subject where subject_no='" + ddlsubject.SelectedValue.ToString() + "'") + "-" + ddlsubject.SelectedItem;
            subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column].HorizontalAlign = HorizontalAlign.Left;
            subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column].Border.BorderColorRight = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_second].Text = string.Empty; //"Regulation:" + GetFunction(" select regulation from degree  where degree_code=" + ddlbranch.SelectedValue.ToString() + "");
            subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_second].Text = "Semester Number:" + ddlduration.SelectedValue.ToString();
            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, start_column, 1, visi_col_second - 1); //aruna
            subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_second].Text = "Conducted Hrs:" + tot_hr.ToString();
            subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_second].HorizontalAlign = HorizontalAlign.Left;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_second].HorizontalAlign = HorizontalAlign.Left;
            subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_second].HorizontalAlign = HorizontalAlign.Left;
            subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_second].Border.BorderColorTop = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_second].Border.BorderColorBottom = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_second].Border.BorderColorTop = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_second].Border.BorderColorBottom = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_second].Border.BorderColorTop = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_second].Border.BorderColorLeft = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_second - 1].Border.BorderColorRight = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column].Border.BorderColorRight = Color.White;
            //aruna subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, start_column, 1, tot_col_first - start_column);
            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(7, start_column, 1, tot_col_first - start_column);
            //aruna subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, start_column, 1, tot_col_first - start_column);
            //aruna subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, visi_col_second, 1, (subject_spread.Sheets[0].ColumnCount - visi_col_second));
            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(7, visi_col_second, 1, (subject_spread.Sheets[0].ColumnCount - visi_col_second));
            //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, visi_col_second, 1, (subject_spread.Sheets[0].ColumnCount - visi_col_second));
        }
        else if (final_print_col_cnt > 8)
        {
            // between_visible_col_cnt = (end_column - col_count)/2;
            between_visible_col_cnt = (final_print_col_cnt) / 2;
            between_visible_col_cnt_bal = (final_print_col_cnt) % 2;
            visi_col_first = 0;
            for (tot_col_first = start_column; tot_col_first < subject_spread.Sheets[0].ColumnCount - 1; tot_col_first++)//==============find first half column count
            {
                if (subject_spread.Sheets[0].Columns[tot_col_first].Visible == true)
                {
                    visi_col_first++;
                    //   if (visi_col_first == between_visible_col_cnt)
                    if (visi_col_first == final_print_col_cnt - 4)
                    {
                        visi_col_first = tot_col_first;
                        break;
                    }
                }
            }
            //aruna=============
            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, start_column, 1, subject_spread.Sheets[0].ColumnCount - 1);
            //==================
            subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column].Text = degree_deatil.ToString(); //"Name of the Degree & Branch:" + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString();
            subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column].HorizontalAlign = HorizontalAlign.Left;
            subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column].Border.BorderColorBottom = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column].Border.BorderColorRight = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column].Border.BorderColorRight = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column].Border.BorderColorRight = Color.White;
            int tot_col_second = 0;
            for (tot_col_second = tot_col_first + 1; tot_col_second < subject_spread.Sheets[0].ColumnCount; tot_col_second++)
            {
                if (subject_spread.Sheets[0].Columns[tot_col_second].Visible == true)
                {
                    visi_col_second = tot_col_second;
                    //break;
                }
            }
            subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first].Border.BorderColorLeft = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first].Border.BorderColorRight = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first].Border.BorderColorBottom = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first].HorizontalAlign = HorizontalAlign.Left;
            subject_spread.Sheets[0].ColumnHeader.Cells[6, end_column].HorizontalAlign = HorizontalAlign.Left;
            //   subject_spread.Sheets[0].ColumnHeader.Cells[6, end_column].Text = GetFunction(" select regulation from degree  where degree_code=" + ddlbranch.SelectedValue.ToString() + "");
            subject_spread.Sheets[0].ColumnHeader.Cells[6, end_column].Border.BorderColorBottom = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column].Text = "Academic Year:" + DateTime.Now.Year.ToString();
            subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column].HorizontalAlign = HorizontalAlign.Left;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column].Border.BorderColorRight = Color.White;
            //subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first].Text = "Regulation:";
            //subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first].Text = "Semester Number:";
            subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first].Border.BorderColorTop = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first].Border.BorderColorLeft = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first].Border.BorderColorRight = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first].HorizontalAlign = HorizontalAlign.Left;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, end_column].HorizontalAlign = HorizontalAlign.Left;
            //  subject_spread.Sheets[0].ColumnHeader.Cells[7, end_column].Text = ddlduration.SelectedValue.ToString();
            subject_spread.Sheets[0].ColumnHeader.Cells[7, end_column].Border.BorderColorTop = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, end_column].Border.BorderColorBottom = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first].Border.BorderColorBottom = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column].Border.BorderColorBottom = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column].Text = "Subject:" + GetFunction("select subject_code From subject where subject_no='" + ddlsubject.SelectedValue.ToString() + "'") + "-" + ddlsubject.SelectedItem;
            subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column].HorizontalAlign = HorizontalAlign.Left;
            subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column].Border.BorderColorRight = Color.White;
            if (visi_col_second < 100)
            {
                subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_second].Text = string.Empty; // "Regulation:" + GetFunction(" select regulation from degree  where degree_code=" + ddlbranch.SelectedValue.ToString() + "");
                subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_second].Text = "Semester Number:" + ddlduration.SelectedValue.ToString();
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, start_column, 1, visi_col_second - 1); //aruna
                subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_second].Text = "Conducted Hrs:" + tot_hr.ToString();
                subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_second].HorizontalAlign = HorizontalAlign.Left;
                subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_second].HorizontalAlign = HorizontalAlign.Left;
                subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_second].HorizontalAlign = HorizontalAlign.Left;
            }
            subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_second].Border.BorderColorTop = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_second].Border.BorderColorBottom = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_second].Border.BorderColorTop = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_second].Border.BorderColorBottom = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_second].Border.BorderColorTop = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_second].Border.BorderColorLeft = Color.White;
            //subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_second].Border.BorderColorRight = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_second - 1].Border.BorderColorRight = Color.White;
            subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_second].HorizontalAlign = HorizontalAlign.Left;
            subject_spread.Sheets[0].ColumnHeader.Cells[8, end_column].HorizontalAlign = HorizontalAlign.Left;
            // subject_spread.Sheets[0].ColumnHeader.Cells[8, end_column].Text = tot_hr.ToString();
            subject_spread.Sheets[0].ColumnHeader.Cells[8, end_column].Border.BorderColorTop = Color.White;
            //=================================================3/5/12
            if (visi_col_second < 100)
            {
                //aruna subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, start_column, 1, tot_col_first - start_column);
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(7, start_column, 1, tot_col_first - start_column);
                //aruna subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, start_column, 1, tot_col_first - start_column);
                //aruna  subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, visi_col_second, 1, (subject_spread.Sheets[0].ColumnCount - visi_col_second));
                subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(7, visi_col_second, 1, (subject_spread.Sheets[0].ColumnCount - visi_col_second));
                //aruna subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, visi_col_second, 1, (subject_spread.Sheets[0].ColumnCount - visi_col_second));
            }
            {
                int dd = visi_col_first;
                int span_col = 0;
                if (dd >= 100)
                {
                    int span_col_count = 0, span_balanc = 0;
                    span_col_count = dd / 100;
                    span_balanc = dd % 100;
                    for (span_col = 0; span_col <= dd - 100; span_col += 100)
                    {
                        subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].ColumnSpan = 100;
                        subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorRight = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorLeft = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column + span_col].ColumnSpan = 100;
                        subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column + span_col].Border.BorderColorRight = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column + span_col].Border.BorderColorLeft = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].ColumnSpan = 100;
                        subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorRight = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorLeft = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorRight = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column + span_col].Border.BorderColorRight = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorRight = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorLeft = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column + span_col].Border.BorderColorLeft = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorLeft = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorTop = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorBottom = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column + span_col].Border.BorderColorTop = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column + span_col].Border.BorderColorBottom = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorTop = Color.White;
                    }
                    subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].ColumnSpan = span_balanc;
                    subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].ColumnSpan = span_balanc;
                    subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].ColumnSpan = span_balanc;
                    subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column + span_col].Border.BorderColorTop = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorBottom = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column + span_col].Border.BorderColorTop = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[6, start_column + span_col].Border.BorderColorBottom = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[7, start_column + span_col].Border.BorderColorTop = Color.White;
                }
                else
                {
                    //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, start_column, 1, visi_col_second + 1);
                    //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(7, start_column, 1, visi_col_second + 1);
                    //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, start_column, 1, visi_col_second + 1);
                }
            }
            {
                int dd = (subject_spread.Sheets[0].ColumnCount - 2 - visi_col_second);
                int span_col = 0;
                if (dd >= 100)
                {
                    int span_col_count = 0, span_balanc = 0;
                    span_col_count = dd / 100;
                    span_balanc = dd % 100;
                    for (span_col = 0; span_col <= dd - 100; span_col += 100)
                    {
                        subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_first + span_col].ColumnSpan = 100;
                        subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_first + span_col].Border.BorderColorRight = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_first + span_col].Border.BorderColorLeft = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first + span_col].ColumnSpan = 100;
                        subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first + span_col].Border.BorderColorRight = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first + span_col].Border.BorderColorLeft = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first + span_col].ColumnSpan = 100;
                        subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first + span_col].Border.BorderColorRight = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first + span_col].Border.BorderColorLeft = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_first + span_col].Border.BorderColorRight = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first + span_col].Border.BorderColorRight = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first + span_col].Border.BorderColorRight = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_first + span_col].Border.BorderColorLeft = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first + span_col].Border.BorderColorLeft = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first + span_col].Border.BorderColorLeft = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_first + span_col].Border.BorderColorTop = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first + span_col].Border.BorderColorBottom = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first + span_col].Border.BorderColorTop = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first + span_col].Border.BorderColorBottom = Color.White;
                        subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first + span_col].Border.BorderColorTop = Color.White;
                    }
                    if (span_balanc == 0)
                    {
                        subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first + span_col - 100].Text = string.Empty; // "Regulation:" + GetFunction(" select regulation from degree  where degree_code=" + ddlbranch.SelectedValue.ToString() + "");
                        subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first + span_col - 100].Text = "Semester Number:" + ddlduration.SelectedValue.ToString();
                        subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, start_column, 1, visi_col_first + span_col - 99); //aruna
                        subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_first + span_col - 100].Text = "Conducted Hrs:" + tot_hr.ToString();
                        subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first + span_col - 100].HorizontalAlign = HorizontalAlign.Left;
                        subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first + span_col - 100].HorizontalAlign = HorizontalAlign.Left;
                        subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_first + span_col - 100].HorizontalAlign = HorizontalAlign.Left;
                    }
                    else
                    {
                        subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first + span_col].Text = string.Empty; //"Regulation:" + GetFunction(" select regulation from degree  where degree_code=" + ddlbranch.SelectedValue.ToString() + "");
                        subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first + span_col].Text = "Semester Number:" + ddlduration.SelectedValue.ToString();
                        subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, start_column, 1, visi_col_first + span_col - 1); //aruna
                        subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_first + span_col].Text = "Conducted Hrs:" + tot_hr.ToString();
                        subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first + span_col].HorizontalAlign = HorizontalAlign.Left;
                        subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first + span_col].HorizontalAlign = HorizontalAlign.Left;
                        subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_first + span_col].HorizontalAlign = HorizontalAlign.Left;
                    }
                    subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_first + span_col].ColumnSpan = span_balanc;
                    subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_first + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_first + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first + span_col].ColumnSpan = span_balanc;
                    subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first + span_col].ColumnSpan = span_balanc;
                    subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first + span_col].Border.BorderColorRight = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first + span_col].Border.BorderColorLeft = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_first + span_col].Border.BorderColorTop = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first + span_col].Border.BorderColorBottom = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first + span_col].Border.BorderColorTop = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first + span_col].Border.BorderColorBottom = Color.White;
                    subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first + span_col].Border.BorderColorTop = Color.White;
                }
                else
                {
                    //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(8, visi_col_first, 1, visi_col3-1);
                    //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(7, visi_col_first, 1, visi_col3-1);
                    //subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(6, visi_col_first, 1, visi_col3-1);
                    //subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first + visi_col3-1].Text = "Regulation:";
                    //subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first + visi_col3-1].Text = "Semester Number:";
                    //subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_first + visi_col3-1].Text = "Total Conducted Hours:";
                    //subject_spread.Sheets[0].ColumnHeader.Cells[6, visi_col_first + visi_col3].HorizontalAlign = HorizontalAlign.Right;
                    //subject_spread.Sheets[0].ColumnHeader.Cells[7, visi_col_first + visi_col3].HorizontalAlign = HorizontalAlign.Right;
                    //subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_first + visi_col3].HorizontalAlign = HorizontalAlign.Right;
                }
            }
            //=====================================================
        }
        subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorTop = Color.White;
        subject_spread.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorBottom = Color.White;
        subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorTop = Color.White;
        subject_spread.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorBottom = Color.White;
        int temp_count_temp = 0;
        string[] header_align_index;
        if (dsprint.Tables[0].Rows.Count > 0)
        {
            if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
            {
                header_align_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString().Split(',');
                string[] new_header_string_index_split = new_header_string_index.Split(',');
                subject_spread.Sheets[0].ColumnHeader.Cells[8, start_column].Border.BorderColorBottom = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[8, end_column].Border.BorderColorBottom = Color.White;
                subject_spread.Sheets[0].ColumnHeader.Cells[8, visi_col_first].Border.BorderColorBottom = Color.White;
                for (int row_head_count = 9; row_head_count < (9 + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
                {
                    subject_spread.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Text = new_header_string_split[temp_count_temp].ToString();
                    if (final_print_col_cnt > 3)
                    {
                        //  subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(row_head_count, start_column, 1, (subject_spread.Sheets[0].ColumnCount - start_column + 1));
                        int dd = (subject_spread.Sheets[0].ColumnCount - start_column + 1);
                        int span_col = 0;
                        if (dd >= 100)
                        {
                            int span_col_count = 0, span_balanc = 0;
                            span_col_count = dd / 100;
                            span_balanc = dd % 100;
                            for (span_col = 0; span_col <= dd - 100; span_col += 100)
                            {
                                subject_spread.Sheets[0].ColumnHeader.Cells[row_head_count, start_column + span_col].ColumnSpan = 100;
                            }
                            subject_spread.Sheets[0].ColumnHeader.Cells[row_head_count, start_column + span_col].ColumnSpan = span_balanc;
                        }
                        else
                        {
                            subject_spread.Sheets[0].ColumnHeaderSpanModel.Add(row_head_count, start_column, 1, (subject_spread.Sheets[0].ColumnCount - start_column + 1));
                        }
                    }
                    subject_spread.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorTop = Color.White;
                    if (row_head_count != (9 + new_header_string_split.GetUpperBound(0)))
                    {
                        subject_spread.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorBottom = Color.White;
                    }
                    if (temp_count_temp <= new_header_string_index_split.GetUpperBound(0))
                    {
                        header_alignment = new_header_string_index_split[temp_count_temp].ToString();
                        if (header_alignment != string.Empty)
                        {
                            if (header_alignment == "2")
                            {
                                subject_spread.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else if (header_alignment == "1")
                            {
                                subject_spread.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Left;
                            }
                            else
                            {
                                subject_spread.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Right;
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
        string sec_val = string.Empty;
        if (ddlsec.SelectedValue.ToString() != string.Empty && ddlsec.SelectedValue.ToString() != null)
        {
            sec_val = "Section: " + ddlsec.SelectedItem.ToString();
        }
        else
        {
            sec_val = string.Empty;
        }
        Boolean check_print_row = false;
        SqlDataReader dr_collinfo;
        con.Close();
        con.Open();
        cmd = new SqlCommand("select isnull(college_name,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website,isnull(form_heading_name,'') as form_name,isnull(batch_degree_branch,'') as degree_deatil,isnull(header_align,'') as header_alignment,isnull(header_flag_value,'') as view_header from print_master_setting  where form_name='singlesubject_wise_attendance.aspx'", con);
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
                    degree_deatil = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
                }
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
                    check_print_row = true;
                    coll_name = dr_collinfo["collname"].ToString();
                    address1 = dr_collinfo["address1"].ToString();
                    address2 = dr_collinfo["address2"].ToString();
                    address3 = dr_collinfo["address3"].ToString();
                    phoneno = dr_collinfo["phoneno"].ToString();
                    faxno = dr_collinfo["faxno"].ToString();
                    email = dr_collinfo["email"].ToString();
                    website = dr_collinfo["website"].ToString();
                    form_name = "Subject Wise Attendance Report ";
                    degree_deatil = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
                }
            }
        }
    }

    public void getspecial_hr()
    {
        try
        {
            string hrdetno = string.Empty;
            if (ht_sphr.Contains(Convert.ToString(temp_date)))
            {
                hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(temp_date), ht_sphr));
            }
            if (hrdetno != "")
            {
                Hashtable hatsphco = new Hashtable();
                string[] strval = hrdetno.Split(',');
                for (int cou = 0; cou <= strval.GetUpperBound(0); cou++)
                {
                    span_count++;
                    subject_spread.Sheets[0].ColumnCount++;
                    samehr_flag = true;
                    subject_spread.Sheets[0].ColumnHeader.Cells[subject_spread.Sheets[0].ColumnHeader.RowCount - 1, (subject_spread.Sheets[0].ColumnCount - 1)].Text = "SH " + (cou + 1);
                    if (!hatsphco.Contains(strval[cou].ToString()))
                    {
                        hatsphco.Add(strval[cou].ToString(), subject_spread.Sheets[0].ColumnCount - 1);
                    }
                }
                string splhr_query_master = "select spa.roll_no,spa.attendance,spa.hrdet_no from  specialhr_attendance spa,specialhr_details spd where spa.hrdet_no=spd.hrdet_no and spd.subject_no='" + subject_no + "' and spd.hrdet_no in(" + hrdetno + ") order by spa.hrdet_no";
                DataSet dsval = d2.select_method_wo_parameter(splhr_query_master, "Text");
                DataView dvsphratt = new DataView();
                for (int roll_count = 0; roll_count < subject_spread.Sheets[0].RowCount; roll_count++)
                {
                    string rollno = subject_spread.Sheets[0].Cells[roll_count, 1].Text.ToString().Trim().ToLower();
                    dsval.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                    dvsphratt = dsval.Tables[0].DefaultView;
                    recflag = true;
                    spl_hr_flag = true;
                    for (int spo = 0; spo < dvsphratt.Count; spo++)
                    {
                        subject_spread.Sheets[0].Rows[roll_count].Visible = true;
                        spl_hr_flag = true;
                        no_stud_flag = true;
                        string value = Attmark(dvsphratt[spo][1].ToString());
                        int columno = 0;
                        if (hatsphco.Contains(dvsphratt[spo][2].ToString()))
                        {
                            columno = Convert.ToInt32(hatsphco[dvsphratt[spo][2].ToString()]);
                        }
                        subject_spread.Sheets[0].Cells[roll_count, columno].Text = value;
                        if ((dvsphratt[spo][1].ToString()) != "8")
                        {
                            if (Attmark(dvsphratt[spo][1].ToString()) != "HS")
                            {
                                string vg = dvsphratt[spo][1].ToString();
                                if (has_attnd_masterset.ContainsKey((dvsphratt[spo][1].ToString().Trim().ToLower())))
                                {
                                    present_count = Convert.ToInt16(has_load_rollno[dvsphratt[spo][0].ToString().Trim().ToLower()]);
                                    present_count++;
                                    has_load_rollno[subject_spread.Sheets[0].Cells[roll_count, 1].Text.Trim().ToLower()] = present_count;
                                }
                                else if (hatabsentvalues.ContainsKey((dvsphratt[spo][1].ToString())))
                                {
                                    present_count = Convert.ToInt16(has_total_absent_hour[dvsphratt[spo][0].ToString().Trim().ToLower()]);
                                    present_count++;
                                    has_total_absent_hour[subject_spread.Sheets[0].Cells[roll_count, 1].Text.Trim().ToLower()] = present_count;
                                }
                                if (Attmark(dvsphratt[spo][1].ToString()) != "NE")
                                {
                                    present_count = Convert.ToInt16(has_total_attnd_hour[dvsphratt[spo][0].ToString().Trim().ToLower()]);
                                    present_count++;
                                    has_total_attnd_hour[subject_spread.Sheets[0].Cells[roll_count, 1].Text.Trim().ToLower()] = present_count;
                                }
                            }
                        }
                    }
                }
            }//Added By Srinath 22/2/2013
        }
        catch
        {
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string sections = ddlsec.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
        {
            sections = string.Empty;
        }
        string Testname = string.Empty;
        if (ddltest.Items.Count > 0)
        {
            Testname = "@Test Name : " + ddltest.SelectedItem.ToString() + "";
        }
        string degreedetails = "Individual Subject Wise Attendance Report" + '@' + "Degree :" + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '-' + ddlbranch.SelectedItem.ToString() + '-' + "Sem-" + ddlduration.SelectedItem.ToString() + '-' + sections + '@' + "Subject Name : " + ddlsubject.SelectedItem.ToString() + " " + Testname + "" + '@' + "Date :" + txtFromDate.Text.ToString() + " To " + txtToDate.Text.ToString() + " ";
        string pagename = "singlesubject_wise_attendance.aspx";
        Printcontrol.loadspreaddetails(subject_spread, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    protected void chklsonduty_SelectedIndexChanged(object sender, EventArgs e)
    {
        int commcount = 0;
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
            txtonduty.Text = "On Duty (" + commcount.ToString() + ")";
        }
        else
        {
            chksonduty.Checked = false;
            txtonduty.Text = "On Duty (" + commcount.ToString() + ")";
        }
    }

    protected void chksonduty_ChekedChange(object sender, EventArgs e)
    {
        if (chksonduty.Checked == true)
        {
            for (int i = 0; i < chklsonduty.Items.Count; i++)
            {
                chklsonduty.Items[i].Selected = true;
                txtonduty.Text = "On Duty(" + chklsonduty.Items.Count + ")";
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
        chksonduty.Checked = false;
        txtonduty.Text = "--Select--";
        string collegecode = Session["collegecode"].ToString();
        string query = "select TextCode,Textval from textvaltable where TextCriteria='Attrs' and college_code='" + collegecode + "'";
        DataSet ds = new DataSet();
        ds.Dispose(); ds.Reset();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            chklsonduty.DataSource = ds;
            chklsonduty.DataTextField = "Textval";
            chklsonduty.DataValueField = "TextCode";
            chklsonduty.DataBind();
        }
        //for (int i = 0; i < chklsonduty.Items.Count; i++)
        //{
        //    chklsonduty.Items[i].Selected = false;
        //}
        //chksonduty.Checked = true;
        //txtonduty.Text = " On Duty (" + chklsonduty.Items.Count + ")";
    }

    private void GetAttendanceTypeAll(out Dictionary<string, ArrayList> dicAttType)
    {
        dicAttType = new Dictionary<string, ArrayList>();
        try
        {
            Dictionary<string, string> dicA = new Dictionary<string, string>();
            dicA.Add("colege_code", Session["collegecode"].ToString());
            DataTable dtAttType = storeAcc.selectDataTable("ATT_MASTER_SETTING", dicA);

            for (count_master = 0; count_master < dtAttType.Rows.Count; count_master++)
            {
                ArrayList arrList = new ArrayList();
                if (dtAttType.Rows[count_master]["calcflag"].ToString() == "0")
                {

                    if (!dicAttType.ContainsKey(dtAttType.Rows[count_master]["leavecode"].ToString()))
                    {
                        arrList = new ArrayList();
                        arrList.Add(Convert.ToString(dtAttType.Rows[count_master]["calcflag"]).Trim());
                        dicAttType.Add(Convert.ToString(dtAttType.Rows[count_master]["leavecode"]).Trim(), arrList);
                    }
                    else
                    {
                        arrList = new ArrayList();
                        arrList = dicAttType[dtAttType.Rows[count_master]["leavecode"].ToString()];
                        if (!arrList.Contains(Convert.ToString(dtAttType.Rows[count_master]["calcflag"]).Trim()))
                        {
                            arrList.Add(Convert.ToString(dtAttType.Rows[count_master]["calcflag"]).Trim());
                        }
                        dicAttType[dtAttType.Rows[count_master]["leavecode"].ToString()] = arrList;
                    }
                }
                if (dtAttType.Rows[count_master]["calcflag"].ToString() == "2")
                {
                    if (!dicAttType.ContainsKey(dtAttType.Rows[count_master]["leavecode"].ToString()))
                    {
                        arrList = new ArrayList();
                        arrList.Add(Convert.ToString(dtAttType.Rows[count_master]["calcflag"]).Trim());
                        dicAttType.Add(Convert.ToString(dtAttType.Rows[count_master]["leavecode"]).Trim(), arrList);
                    }
                    else
                    {
                        arrList = new ArrayList();
                        arrList = dicAttType[dtAttType.Rows[count_master]["leavecode"].ToString()];
                        if (!arrList.Contains(Convert.ToString(dtAttType.Rows[count_master]["calcflag"]).Trim()))
                        {
                            arrList.Add(Convert.ToString(dtAttType.Rows[count_master]["calcflag"]).Trim());
                        }
                        dicAttType[dtAttType.Rows[count_master]["leavecode"].ToString()] = arrList;
                    }
                }
                if (dtAttType.Rows[count_master]["calcflag"].ToString() == "1")
                {
                    if (!dicAttType.ContainsKey(dtAttType.Rows[count_master]["leavecode"].ToString()))
                    {
                        arrList = new ArrayList();
                        arrList.Add(Convert.ToString(dtAttType.Rows[count_master]["calcflag"]).Trim());
                        dicAttType.Add(Convert.ToString(dtAttType.Rows[count_master]["leavecode"]).Trim(), arrList);
                    }
                    else
                    {
                        arrList = new ArrayList();
                        arrList = dicAttType[dtAttType.Rows[count_master]["leavecode"].ToString()];
                        if (!arrList.Contains(Convert.ToString(dtAttType.Rows[count_master]["calcflag"]).Trim()))
                        {
                            arrList.Add(Convert.ToString(dtAttType.Rows[count_master]["calcflag"]).Trim());
                        }
                        dicAttType[dtAttType.Rows[count_master]["leavecode"].ToString()] = arrList;
                    }
                }
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// To Get Semester Schedule and Aleternate Schedule From New To Old Table Structue
    /// Developed By Malang Raja T
    /// </summary> 
    /// <param name="Collegecode">CollegeCode</param>
    /// <param name="Batchyear">BatchYear</param>
    /// <param name="Degreecode">DegreeCode</param>
    /// <param name="Semester">Semester</param>
    /// <param name="Section">Section Name</param>
    /// <param name="SemStartdate">From Date in the Format MM/dd/yyyy</param>
    /// <param name="SemEnddate">To Date in the Format MM/dd/yyyy</param>
    /// <param name="intNHrs">Total No Of Hours for this Class</param>
    /// <param name="dtSchedule">Semester Schedule </param>
    /// <param name="dtAlterSchedule">Aleternate Schedule</param>
    protected void SemesterandAlternateSchedule(string Collegecode, string Batchyear, string Degreecode, string Semester, string Section, string SemStartdate, string SemEnddate, int intNHrs, ref DataTable dtSchedule, ref DataTable dtAlterSchedule)
    {
        try
        {
            string qrySection = string.Empty;
            if (Section.Trim() != "")
                qrySection = " and ltrim(rtrim(isnull(ct.TT_sec,'')))='" + Section + "'";
            DataTable dtSelect = dirAcc.selectDataTable("select CONVERT(varchar(20),ct.TT_date,103) as TT_date,TT_date as NewDate,ct.TT_degCode as degree_code,ct.TT_classAdvisor as class_advisor,ct.TT_sem as semester,ct.TT_batchyear as batch_year,ltrim(rtrim(isnull(ct.TT_sec,''))) as Sections,ct.TT_lastRec as LastRec,ct.TT_name as ttname,TT_subno,TT_staffcode,TT_Day,TT_Hour,isnull(Lab,'0') as Lab,isnull(Elective,'0') as Elective,isnull(s.practicalPair,0) as practicalpair,do.Daydiscription,(select room_name from Room_Detail rd where rd.roompk = ctd.TT_Room ) as Room,ct.TT_ClassPK from TT_ClassTimetable ct, TT_ClassTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + Collegecode.Trim() + "' and ct.TT_degCode='" + Convert.ToString(Degreecode).Trim() + "' and ct.TT_batchyear='" + Batchyear + "' and ct.TT_sem='" + Convert.ToString(Semester).Trim() + "' " + qrySection + " and ct.TT_date<='" + SemEnddate + "' order by NewDate desc");  // Change this Place 
            dtSchedule = new DataTable();
            dtSchedule.Columns.Add("TT_ClassPK");
            dtSchedule.Columns.Add("FromDate");
            dtSchedule.Columns.Add("TTDate", typeof(DateTime));
            dtSchedule.Columns.Add("degree_code");
            dtSchedule.Columns.Add("class_advisor");
            dtSchedule.Columns.Add("semester");
            dtSchedule.Columns.Add("batch_year");
            dtSchedule.Columns.Add("Sections");
            dtSchedule.Columns.Add("LastRec");
            dtSchedule.Columns.Add("ttname");
            DataTable dtMinMaxSchedule = new DataTable();
            dtMinMaxSchedule = dirAcc.selectDataTable("select distinct CONVERT(varchar(20),ct.TT_date,103) as TT_date,ct.TT_date TTDate from TT_ClassTimetable ct, TT_ClassTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_ClassFk and ct.TT_colCode='" + Collegecode + "' and ct.TT_degCode='" + Degreecode + "' and ct.TT_batchyear='" + Batchyear + "' and ct.TT_sem='" + Semester + "' " + qrySection + " and ct.TT_date<='" + SemEnddate + "' order by  TTDate desc");
            if (dtMinMaxSchedule.Rows.Count > 0)  // Change this Place 
            {
                DataRow drSchedule;//= dtSchedule.NewRow();
                //DateTime dtMinDate = new DateTime();
                //DateTime dtMaxDate = new DateTime();
                //if (Convert.ToString(dtMinMaxSchedule.Rows[0]["FromDate"]).Trim() != "" && Convert.ToString(dtMinMaxSchedule.Rows[0]["ToDate"]).Trim() != "")
                //{
                //DateTime.TryParseExact(Convert.ToString(dtMinMaxSchedule.Rows[0]["FromDate"]), "dd/MM/yyyy", null, DateTimeStyles.None, out dtMinDate);
                //DateTime.TryParseExact(Convert.ToString(dtMinMaxSchedule.Rows[0]["ToDate"]), "dd/MM/yyyy", null, DateTimeStyles.None, out dtMaxDate);
                //for (DateTime dtTemp = dtMinDate; dtTemp <= dtMaxDate; dtTemp = dtTemp.AddDays(1))
                //{
                DateTime dtTTDate = new DateTime();
                for (int ttRow = 0; ttRow < dtMinMaxSchedule.Rows.Count; ttRow++)
                {
                    if (Convert.ToString(dtMinMaxSchedule.Rows[0]["TT_date"]).Trim() != "" && Convert.ToString(dtMinMaxSchedule.Rows[0]["TTDate"]).Trim() != "")
                    {
                        DateTime.TryParseExact(Convert.ToString(dtMinMaxSchedule.Rows[ttRow]["TT_date"]), "dd/MM/yyyy", null, DateTimeStyles.None, out dtTTDate);
                        drSchedule = dtSchedule.NewRow();
                        drSchedule["FromDate"] = Convert.ToString(dtTTDate.ToString("MM/dd/yyyy"));
                        drSchedule["TTDate"] = dtTTDate;
                        DateTime DnewCheck = Convert.ToDateTime(Convert.ToString(dtMinMaxSchedule.Rows[ttRow]["TTDate"]));
                        for (int i = 0; i < 7; i++)
                        {
                            string curday = string.Empty;
                            string curdayFull = string.Empty;
                            switch (i)
                            {
                                case 0:
                                    curdayFull = "Monday";
                                    curday = "mon";
                                    break;
                                case 1:
                                    curdayFull = "Tuesday";
                                    curday = "tue";
                                    break;
                                case 2:
                                    curdayFull = "Wednesday";
                                    curday = "wed";
                                    break;
                                case 3:
                                    curdayFull = "Thursday";
                                    curday = "thu";
                                    break;
                                case 4:
                                    curdayFull = "Friday";
                                    curday = "fri";
                                    break;
                                case 5:
                                    curdayFull = "Saturday";
                                    curday = "sat";
                                    break;
                                case 6:
                                    curdayFull = "Sunday";
                                    curday = "sun";
                                    break;
                            }
                            for (int hrsI = 1; hrsI <= intNHrs; hrsI++)
                            {
                                if (!dtSchedule.Columns.Contains(curday + hrsI))
                                    dtSchedule.Columns.Add(curday + hrsI);

                                dtSelect.DefaultView.RowFilter = "Daydiscription='" + curdayFull + "' and TT_Hour='" + hrsI + "' and NewDate ='" + DnewCheck.ToString("MM/dd/yyy") + "'";
                                //DataView dvAttCrit = dtSelect.DefaultView;
                                //dvAttCrit.Sort = "desc TT_date";
                                DataTable dtFilter = dtSelect.DefaultView.ToTable();
                                if (dtFilter.Rows.Count > 0)
                                {
                                    drSchedule["TT_ClassPK"] = Convert.ToString(dtFilter.Rows[0]["TT_ClassPK"]);
                                    drSchedule["ttname"] = Convert.ToString(dtFilter.Rows[0]["ttname"]);
                                    drSchedule["degree_code"] = Convert.ToString(dtFilter.Rows[0]["degree_code"]);
                                    drSchedule["class_advisor"] = Convert.ToString(dtFilter.Rows[0]["class_advisor"]);
                                    drSchedule["semester"] = Convert.ToString(dtFilter.Rows[0]["semester"]);
                                    drSchedule["batch_year"] = Convert.ToString(dtFilter.Rows[0]["batch_year"]);
                                    drSchedule["Sections"] = Convert.ToString(dtFilter.Rows[0]["Sections"]);
                                    drSchedule["LastRec"] = Convert.ToString(dtFilter.Rows[0]["LastRec"]);
                                    StringBuilder sbNew = new StringBuilder();
                                    for (int dtI = 0; dtI < dtFilter.Rows.Count; dtI++)
                                    {
                                        string subno = Convert.ToString(dtFilter.Rows[dtI]["TT_subno"]);
                                        string stfcode = Convert.ToString(dtFilter.Rows[dtI]["TT_staffcode"]);
                                        string elect = Convert.ToString(dtFilter.Rows[dtI]["Elective"]);
                                        string Lab = Convert.ToString(dtFilter.Rows[dtI]["Lab"]);
                                        string practicalpair = Convert.ToString(dtFilter.Rows[dtI]["practicalpair"]);
                                        string differenciator = "S";
                                        string room = Convert.ToString(dtFilter.Rows[dtI]["room"]);
                                        if (elect.ToLower() == "true")
                                        {
                                            differenciator = "E";
                                        }
                                        else if (Lab.ToLower() == "true")
                                        {
                                            differenciator = "L";
                                        }
                                        else if (practicalpair != "0")
                                        {
                                            differenciator = "C";
                                        }
                                        sbNew.Append(subno + "-" + stfcode + "-" + differenciator + "-" + room + ";");
                                    }
                                    drSchedule[curday + hrsI] = sbNew.ToString();
                                }
                            }
                        }
                        dtSchedule.Rows.Add(drSchedule);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    string curday = string.Empty;
                    string curdayFull = string.Empty;
                    switch (i)
                    {
                        case 0:
                            curdayFull = "Monday";
                            curday = "mon";
                            break;
                        case 1:
                            curdayFull = "Tuesday";
                            curday = "tue";
                            break;
                        case 2:
                            curdayFull = "Wednesday";
                            curday = "wed";
                            break;
                        case 3:
                            curdayFull = "Thursday";
                            curday = "thu";
                            break;
                        case 4:
                            curdayFull = "Friday";
                            curday = "fri";
                            break;
                        case 5:
                            curdayFull = "Saturday";
                            curday = "sat";
                            break;
                        case 6:
                            curdayFull = "Sunday";
                            curday = "sun";
                            break;
                    }
                    for (int hrsI = 1; hrsI <= intNHrs; hrsI++)
                    {
                        if (!dtSchedule.Columns.Contains(curday + hrsI))
                            dtSchedule.Columns.Add(curday + hrsI);
                    }
                }
            }
            DataTable dtAlterSelect = dirAcc.selectDataTable("select ct.TT_degCode as degree_code,ct.TT_classAdvisor as class_advisor,ct.TT_sem as semester,ct.TT_batchyear as batch_year,ltrim(rtrim(isnull(ct.TT_sec,''))) as Sections,ct.TT_lastRec as LastRec,ctd.TT_ClassFK,ct.TT_name as ttname,TT_subno,TT_staffcode,TT_Day,TT_Hour,isnull(Lab,'0') as Lab,isnull(Elective,'0') as Elective,isnull(s.practicalPair,0) as practicalpair,do.Daydiscription,ctd.TT_AlterDate,(select room_name from Room_Detail rd where rd.roompk = ctd.TT_Room ) as Room,ctd.TT_AlterDate from TT_ClassTimetable ct, TT_AlterTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_CLassFK and ct.TT_colCode='" + Convert.ToString(Collegecode).Trim() + "' and ct.TT_degCode='" + Convert.ToString(Degreecode).Trim() + "' and ct.TT_batchyear='" + Batchyear + "' and ct.TT_sem='" + Convert.ToString(Semester).Trim() + "' " + qrySection + "  and ctd.TT_AlterDate>='" + SemStartdate + "' and ctd.TT_AlterDate<='" + SemEnddate + "'  order by TT_AlterDate");
            dtAlterSchedule = new DataTable();
            dtAlterSchedule.Columns.Add("FromDate");
            dtAlterSchedule.Columns.Add("TTAlterDate", typeof(DateTime));
            dtAlterSchedule.Columns.Add("TT_ClassFK");
            dtAlterSchedule.Columns.Add("degree_code");
            dtAlterSchedule.Columns.Add("class_advisor");
            dtAlterSchedule.Columns.Add("semester");
            dtAlterSchedule.Columns.Add("batch_year");
            dtAlterSchedule.Columns.Add("Sections");
            dtAlterSchedule.Columns.Add("LastRec");//
            dtAlterSchedule.Columns.Add("ttname");//ttname
            DataTable dtMinMaxDate = dirAcc.selectDataTable("select distinct CONVERT(varchar(20),ctd.TT_AlterDate,103) as TT_date,ctd.TT_AlterDate TTDate from TT_ClassTimetable ct, TT_AlterTimetableDet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_CLassFK and ct.TT_colCode='" + Convert.ToString(Collegecode).Trim() + "' and ct.TT_degCode='" + Convert.ToString(Degreecode).Trim() + "' and ct.TT_batchyear='" + Batchyear + "' and ct.TT_sem='" + Convert.ToString(Semester).Trim() + "' " + qrySection + "  and ctd.TT_AlterDate>='" + SemStartdate + "' and ctd.TT_AlterDate<='" + SemEnddate + "' order by TTDate");
            if (dtMinMaxDate.Rows.Count > 0)
            {
                DataRow drAltert;// = dtAlterSchedule.NewRow();
                DateTime dtMinDate = new DateTime();
                DateTime dtMaxDate = new DateTime();
                DateTime dtTTDate = new DateTime();
                //if (Convert.ToString(dtMinMaxDate.Rows[0]["FromDate"]).Trim() != "" && Convert.ToString(dtMinMaxDate.Rows[0]["ToDate"]).Trim() != "")
                //{
                //DateTime.TryParseExact(Convert.ToString(dtMinMaxDate.Rows[0]["FromDate"]), "dd/MM/yyyy", null, DateTimeStyles.None, out dtMinDate);
                //DateTime.TryParseExact(Convert.ToString(dtMinMaxDate.Rows[0]["ToDate"]), "dd/MM/yyyy", null, DateTimeStyles.None, out dtMaxDate);
                //for (DateTime dtTemp = dtMinDate; dtTemp <= dtMaxDate; dtTemp = dtTemp.AddDays(1))
                //{
                for (int ttRow = 0; ttRow < dtMinMaxDate.Rows.Count; ttRow++)
                {
                    if (Convert.ToString(dtMinMaxDate.Rows[0]["TT_date"]).Trim() != "" && Convert.ToString(dtMinMaxDate.Rows[0]["TTDate"]).Trim() != "")
                    {
                        DateTime.TryParseExact(Convert.ToString(dtMinMaxDate.Rows[ttRow]["TT_date"]), "dd/MM/yyyy", null, DateTimeStyles.None, out dtTTDate);
                        drAltert = dtAlterSchedule.NewRow();
                        drAltert["FromDate"] = Convert.ToString(dtTTDate.ToString("MM/dd/yyyy"));
                        drAltert["TTAlterDate"] = dtTTDate;
                        for (int i = 0; i < 7; i++)
                        {
                            string curday = string.Empty;
                            string curdayFull = string.Empty;
                            switch (i)
                            {
                                case 0:
                                    curdayFull = "Monday";
                                    curday = "mon";
                                    break;
                                case 1:
                                    curdayFull = "Tuesday";
                                    curday = "tue";
                                    break;
                                case 2:
                                    curdayFull = "Wednesday";
                                    curday = "wed";
                                    break;
                                case 3:
                                    curdayFull = "Thursday";
                                    curday = "thu";
                                    break;
                                case 4:
                                    curdayFull = "Friday";
                                    curday = "fri";
                                    break;
                                case 5:
                                    curdayFull = "Saturday";
                                    curday = "sat";
                                    break;
                                case 6:
                                    curdayFull = "Sunday";
                                    curday = "sun";
                                    break;
                            }
                            for (int hrsI = 1; hrsI <= intNHrs; hrsI++)
                            {
                                if (!dtAlterSchedule.Columns.Contains(curday + hrsI))
                                    dtAlterSchedule.Columns.Add(curday + hrsI);

                                dtAlterSelect.DefaultView.RowFilter = "Daydiscription='" + curdayFull + "' and TT_Hour='" + hrsI + "' and TT_AlterDate='" + dtTTDate.ToString("MM/dd/yyyy") + "'";
                                DataTable dtFilter = dtAlterSelect.DefaultView.ToTable();
                                if (dtFilter.Rows.Count > 0)
                                {
                                    StringBuilder sbNew = new StringBuilder();
                                    drAltert["TT_ClassFK"] = Convert.ToString(dtFilter.Rows[0]["TT_ClassFK"]);
                                    drAltert["ttname"] = Convert.ToString(dtFilter.Rows[0]["ttname"]);
                                    drAltert["degree_code"] = Convert.ToString(dtFilter.Rows[0]["degree_code"]);
                                    drAltert["class_advisor"] = Convert.ToString(dtFilter.Rows[0]["class_advisor"]);
                                    drAltert["semester"] = Convert.ToString(dtFilter.Rows[0]["semester"]);
                                    drAltert["batch_year"] = Convert.ToString(dtFilter.Rows[0]["batch_year"]);
                                    drAltert["Sections"] = Convert.ToString(dtFilter.Rows[0]["Sections"]);
                                    drAltert["LastRec"] = Convert.ToString(dtFilter.Rows[0]["LastRec"]);
                                    for (int dtI = 0; dtI < dtFilter.Rows.Count; dtI++)
                                    {
                                        string subno = Convert.ToString(dtFilter.Rows[dtI]["TT_subno"]);
                                        string stfcode = Convert.ToString(dtFilter.Rows[dtI]["TT_staffcode"]);
                                        string elect = Convert.ToString(dtFilter.Rows[dtI]["Elective"]);
                                        string Lab = Convert.ToString(dtFilter.Rows[dtI]["Lab"]);
                                        string practicalpair = Convert.ToString(dtFilter.Rows[dtI]["practicalpair"]);
                                        string differenciator = "S";
                                        string room = Convert.ToString(dtFilter.Rows[dtI]["room"]);
                                        if (elect.ToLower() == "true")
                                        {
                                            differenciator = "E";
                                        }
                                        else if (Lab.ToLower() == "true")
                                        {
                                            differenciator = "L";
                                        }
                                        else if (practicalpair != "0")
                                        {
                                            differenciator = "C";
                                        }
                                        sbNew.Append(subno + "-" + stfcode + "-" + differenciator + "-" + room + ";");
                                    }
                                    drAltert[curday + hrsI] = sbNew.ToString();
                                }
                            }
                        }
                        dtAlterSchedule.Rows.Add(drAltert);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    string curday = string.Empty;
                    string curdayFull = string.Empty;
                    switch (i)
                    {
                        case 0:
                            curdayFull = "Monday";
                            curday = "mon";
                            break;
                        case 1:
                            curdayFull = "Tuesday";
                            curday = "tue";
                            break;
                        case 2:
                            curdayFull = "Wednesday";
                            curday = "wed";
                            break;
                        case 3:
                            curdayFull = "Thursday";
                            curday = "thu";
                            break;
                        case 4:
                            curdayFull = "Friday";
                            curday = "fri";
                            break;
                        case 5:
                            curdayFull = "Saturday";
                            curday = "sat";
                            break;
                        case 6:
                            curdayFull = "Sunday";
                            curday = "sun";
                            break;
                    }
                    for (int hrsI = 1; hrsI <= intNHrs; hrsI++)
                    {
                        if (!dtAlterSchedule.Columns.Contains(curday + hrsI))
                            dtAlterSchedule.Columns.Add(curday + hrsI);
                    }
                }
            }
        }
        catch
        {
        }
    }

}

using System;//=============modified on 28/2/12(remov "select all"), 28/2/12(tot P&A), 29/2/12(SlipList),(spread width)
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
//--------------07/3/12(select all for NE values ly), 12/4/12(update attnd parameter msg), 11/05/12( halforfull='0')
//==============26/5/12(holiday date err msg issue),2/6/12(condition),20/6/12(notes open in a download window and save in a temp folder)
//--------------6/7/12(strsec into query and isNull),19/7/12(order by,check has contain)
//=============09.08.12(added suspend condition when load to the spread,and condn in select and deselect button) mythili 
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BalAccess;
using System.Globalization;

public partial class newadmin : System.Web.UI.Page
{
    // SqlConnection dc_con = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    // SqlConnection dc_con1 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    // SqlConnection mysql1 = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlCommand cmd = new SqlCommand();

    Hashtable hat = new Hashtable();
    DataSet ds_attndmaster = new DataSet();
    DAccess2 dacces2 = new DAccess2();
    DAccess2 d2 = new DAccess2();
    int count_master = 0;
    Hashtable present_calcflag = new Hashtable();
    Hashtable absent_calcflag = new Hashtable();

    SqlCommand cmd_sem_shed;
    DataSet ds = new DataSet();
    string strsec = string.Empty;
    string no_of_hrs = string.Empty; string sch_order = string.Empty; string srt_day = string.Empty; string startdate = string.Empty; string no_days = string.Empty; string starting_dayorder = string.Empty;
    int no_hrs = 0, nodays = 0, temp_hr = 0, strdate = 0;
    string date_txt = string.Empty; string sem_sched = string.Empty; string subject_no = string.Empty; string Att_dcolumn = string.Empty; string Att_strqueryst = string.Empty;
    int present_count = 0, absent_count = 0, colcnt = 0;
    string staffcode = string.Empty;
    string roll_indiv = string.Empty;
    bool flag_true = false;
    int flag;
    bool cellclick = false;
    bool cellclick1 = false;
    bool colhead = false;
    bool serialflag = false;
    bool update_flag = false;
    string strdayflag;
    string regularflag = string.Empty;
    string genderflag = string.Empty;
    bool nullflag = false;
    string Att_mark = string.Empty;
    int Att_mark_row;
    int Att_mark_column;
    string strorder;
    string groupcode = string.Empty;
    string Day_Order = string.Empty;
    string SenderID = string.Empty;
    string Password = string.Empty;
    static string collegename = string.Empty;
    static string collacronym = string.Empty;
    static string coursename = string.Empty;
    static string minimum_day = string.Empty;
    static ArrayList countarray = new ArrayList();
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
    string grouporusercode = string.Empty;
    // static string max_perday =string.Empty;
    string grouporusercode1 = string.Empty;

    public bool daycheck(DateTime seldate)//modify by sirnath 26.09.16
    {
        string collegecode = Session["collegecode"].ToString();
        bool daycheck = false;
        DateTime curdate;//, prevdate;
        long total, k, s;
        string[] ddate = new string[1000];
        string c_date = DateTime.Today.ToString();
        DateTime todate_day = Convert.ToDateTime(DateTime.Today.ToString());
        curdate = DateTime.Today;
        if (seldate.ToString() == c_date)
        {
            daycheck = true;
            return daycheck;
        }
        else
        {
         
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
                grouporusercode1 = " and group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
                grouporusercode1 = " and usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            string lockdayvalue = "select * from Master_Settings where settings='Attendance Lock Days' " + grouporusercode + "";
            DataSet ds = new DataSet();
            ds = d2.select_method_wo_parameter(lockdayvalue, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // if (ds.Tables[0].Rows[i][0].ToString() != null && ds.Tables[0].Rows[i][0].ToString() != "")
                    if (ds.Tables[0].Rows[0]["value"].ToString() == "0")
                    {
                        total = int.Parse(ds.Tables[0].Rows[i]["template"].ToString());
                        total = total + 1;
                        String strholidasquery = "select holiday_date from holidaystudents where degree_code='" + ddlbranch.SelectedValue.ToString() + "'  and semester='" + ddlsem.SelectedValue.ToString() + "'";
                        DataSet ds1 = new DataSet();
                        ds1 = dacces2.select_method(strholidasquery, hat, "Text");
                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count <= 0)
                        {
                            for (int i1 = 1; i1 < total; i1++)
                            {
                                string temp_date = todate_day.AddDays(-i1).ToString();
                                string temp2 = todate_day.AddDays(i1).ToString();
                                if (temp_date == seldate.ToString())
                                {
                                    daycheck = true;
                                    return daycheck;
                                }
                                if (temp2 == seldate.ToString())
                                {
                                    daycheck = true;
                                    return daycheck;
                                }
                            }
                        }
                        else
                        {
                            k = 0;
                            for (int i1 = 1; i1 < ds1.Tables[0].Rows.Count; i1++)
                            {
                                ddate[k] = ds1.Tables[0].Rows[i1][0].ToString();
                                k++;
                            }
                            i = 0;
                            while (i <= total - 1)
                            {
                                string temp_date = curdate.AddDays(-i).ToString();
                                for (s = 0; s < k - 1; s++)
                                {
                                    if (temp_date == ddate[s].ToString())
                                    {
                                        total = total + 1;
                                        goto lab;
                                    }
                                }
                            lab:
                                i = i + 1;
                                if (temp_date == seldate.ToString())
                                {
                                    daycheck = true;
                                    return daycheck;
                                }
                            }
                        }
                    }
                    else
                    {
                        daycheck = true;
                    }
                }
            }
        }
        return daycheck;
    }

    public DataSet Bind_Degree(string college_code, string user_code, string group_code)
    {
        string query = string.Empty;
        if ((group_code.ToString().Trim() != "") && (group_code.Trim() != "0") && (group_code.ToString().Trim() != "-1"))
        {
            query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_code + "";
        }
        else
        {
            query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "";
        }
        DataSet ds = new DataSet();
        ds = dacces2.select_method(query, hat, "Text");
        return ds;
    }

    public DataSet Bind_Dept(string degree_code, string college_code, string user_code, string group_code)
    {
        string query = string.Empty;
        if ((group_code.ToString().Trim() != "") && (group_code.Trim() != "0") && (group_code.ToString().Trim() != "-1"))
        {
            query = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_code + "";
        }
        else
        {
            query = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "";
        }
        DataSet ds = new DataSet();
        ds = dacces2.select_method(query, hat, "Text");
        return ds;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (!IsPostBack)
        {
            string grouporusercode = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                grouporusercode1 = "  and group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                grouporusercode1 = "  and usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode + "";
            schoolds.Clear();
            schoolds.Dispose();
            schoolds = dacces2.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                if (schoolvalue.Trim() == "0")
                {
                    lblbatch.Text = "Year";
                    lbldegree.Text = "School Type";
                    lblbranch.Text = "Standard";
                    lblsem.Text = "Term";
                }
            }
            txtFromDate.Attributes.Add("Readonly", "Readonly");
            TxtToDate.Attributes.Add("Readonly", "Readonly");
            ddlmark.Items.Insert(0, new ListItem("--Select--", "-1"));
            ddlmark.Items.Add("P");
            ddlmark.Items.Add("A");
            ddlmark.Items.Add("OD");
            ddlmark.Items.Add("SOD");
            ddlmark.Items.Add("ML");
            ddlmark.Items.Add("NSS");
            ddlmark.Items.Add("L");

            FpSpread2.Sheets[0].AutoPostBack = false;
            update_flag = false;

            ddlmarkothers.Items.Insert(0, new ListItem("--Select--", "-1"));
            ddlmarkothers.Items.Add("P");
            ddlmarkothers.Items.Add("A");
            ddlmarkothers.Items.Add("OD");
            ddlmarkothers.Items.Add("SOD");
            ddlmarkothers.Items.Add("ML");
            ddlmarkothers.Items.Add("NSS");
            ddlmarkothers.Items.Add("L");
            txtFromDate.Text = DateTime.Today.ToString("d-MM-yyyy");
            TxtToDate.Text = DateTime.Today.ToString("d-MM-yyyy");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            Session["Sex"] = "0";
            Session["flag"] = "-1";
            FpSpread2.Sheets[0].FrozenRowCount = 1;
            FpSpread2.Sheets[0].Columns.Default.Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Columns.Default.Font.Size = FontUnit.Medium;

            FpSpread2.Sheets[0].Rows.Default.Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Rows.Default.Font.Size = FontUnit.Medium;

            MyStyle.Font.Bold = true;
            MyStyle.Font.Size = FontUnit.Medium;
            MyStyle.HorizontalAlign = HorizontalAlign.Center;
            MyStyle.ForeColor = Color.Black;
            MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            spread_sliplist.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            spread_sliplist.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            spread_sliplist.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.Font.Name = "Book Antiqua";
            style.BackColor = FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.BackColor;
            FpSpread2.SheetCorner.Rows.Default.Font.Size = FontUnit.Medium;
            FpSpread2.SheetCorner.Rows.Default.Font.Name = "Book Antiqua";
            FpSpread2.SheetCorner.Rows.Default.Font.Bold = true;
            FpSpread2.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            FpSpread2.Sheets[0].AllowTableCorner = true;
            FpSpread2.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns.Default.HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            FpSpread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            FpSpread2.Sheets[0].Columns.Default.HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread2.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpSpread2.ActiveSheetView.SheetCorner.DefaultStyle.BackColor = FpSpread2.Sheets[0].ColumnHeader.DefaultStyle.BackColor;
            FpSpread2.RowHeader.Width = 50;
            pnl_sliplist.Visible = false;
            string Master1 = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                Master1 = "select * from Master_Settings where group_code=" + Session["group_code"] + "";
            }
            else
            {
                Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            }
            DataSet dsmaseter = dacces2.select_method(Master1, hat, "Text");
            string regularflag = string.Empty;
            if (dsmaseter.Tables.Count > 0 && dsmaseter.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsmaseter.Tables[0].Rows.Count; i++)
                {
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Register No" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "sex" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Sex"] = "1";
                    }
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "General attend" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        option.SelectedValue = "1";
                    }
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Absentees" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        option.SelectedValue = "2";
                    }
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "RollNo" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        RadioButtonList1.SelectedValue = "1";
                    }
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "RegisterNo" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        RadioButtonList1.SelectedValue = "2";
                    }
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Admission No" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        RadioButtonList1.SelectedValue = "3";
                    }
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "General" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["flag"] = 0;
                    }
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "As Per Lesson" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["flag"] = 1;
                    }
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Male" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        genderflag = " and (a.sex='0'";
                    }
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Female" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
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
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Days Scholor" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        if (strdayflag != null && strdayflag != "\0")
                        {
                            strdayflag = strdayflag + " or registration.Stud_Type='Day Scholar'";
                        }
                        else
                        {
                            strdayflag = " and (registration.Stud_Type='Day Scholar'";
                        }
                    }
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Hostel" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
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
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Regular" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        regularflag = "and ((registration.mode=1)";
                    }
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Lateral" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        if (regularflag != "")
                        {
                            regularflag = regularflag + " or (registration.mode=3)";
                        }
                        else
                        {
                            regularflag = regularflag + " and ((registration.mode=3)";
                        }
                    }
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Transfer" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        if (regularflag != "")
                        {
                            regularflag = regularflag + " or (registration.mode=2)";
                        }
                        else
                        {
                            regularflag = regularflag + " and ((registration.mode=2)";
                        }
                    }
                }
            }

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
            FpSpread2.Sheets[0].FrozenRowCount = 1;
            BindBatch();
            bool lockflag = false;
            if (ddlbatch.Items.Count > 0)
            {
                lockflag = true;
            }
            string collegecode = Session["collegecode"].ToString();
            string usercode = Session["usercode"].ToString();
            groupcode = Session["group_code"].ToString();
            DataSet ds = Bind_Degree(collegecode.ToString(), usercode, groupcode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && lockflag == true)
            {
                ddldegree.Enabled = true;
                ddlbranch.Enabled = true;
                ddlsem.Enabled = true;
                txtFromDate.Enabled = true;
                TxtToDate.Enabled = true;
                ddlsec.Enabled = true;
                Btngo.Enabled = true;

                ddldegree.DataSource = ds;
                ddldegree.DataValueField = "course_id";

                ddldegree.DataTextField = "course_name";
                ddldegree.DataBind();
                string course_id = ddldegree.SelectedValue.ToString();
                groupcode = Session["group_code"].ToString();
                DataSet dsbranch = Bind_Dept(course_id, collegecode, usercode, groupcode);
                ddlbranch.DataSource = dsbranch;
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataBind();
                bindsem();

            }
            else
            {
                ddldegree.Enabled = false;
                ddlbranch.Enabled = false;
                ddlsem.Enabled = false;
                txtFromDate.Enabled = false;
                TxtToDate.Enabled = false;
                ddlsec.Enabled = false;
                Btngo.Enabled = false;
            }
            btnsliplist.Visible = false;
        }
    }

    protected void Btngo_Click(object sender, EventArgs e)
    {
        try
        {
            lblset.Text = string.Empty;
            datelbl.Text = string.Empty;
            FpSpread2.Visible = false;
            pHeaderatendence.Visible = false;
            pBodyatendence.Visible = false;
            lblfromdate.Visible = false;
            lbltodate.Visible = false;
            lblset.Visible = false;
            lblother.Visible = false;
            LabelE.Visible = false;
            serialflag = false;
            DataSet dsholiday = new DataSet();
            loadcollegename();
            Dictionary<string, DateTime[]> dicFeeOfRollStudents = new Dictionary<string, DateTime[]>();
            Dictionary<string, byte> dicFeeOnRollStudents = new Dictionary<string, byte>();
            GetFeeOfRollStudent(ref dicFeeOfRollStudents, ref dicFeeOnRollStudents);
            if (staffcode == "" || staffcode == null)
            {
                FpSpread2.Visible = true;
                btnsliplist.Visible = true;
                if (txtFromDate.Text != "")
                {
                    if (TxtToDate.Text != "")
                    {
                        lblset.Visible = false;
                        datelbl.Visible = false;
                        Buttonsave.Visible = true;
                        Buttonupdate.Visible = false;
                        string strsec = string.Empty;
                        FpSpread2.Sheets[0].Visible = true;
                        string sec = string.Empty;
                        string secrights = string.Empty;
                        if (ddlsec.Enabled == true)
                        {
                            if (ddlsec.Text.ToString().Trim().ToLower() == "all" || ddlsec.Text.ToString() == "")
                            {
                                strsec = string.Empty;
                            }
                            else
                            {
                                strsec = " and registration.sections='" + ddlsec.SelectedValue.ToString() + "'";
                                sec = " and r.sections='" + ddlsec.SelectedValue.ToString() + "'";
                                secrights = ddlsec.SelectedValue.ToString();
                            }
                        }
                        bool secrightsflag = false;
                        string collegecode = Session["collegecode"].ToString();
                        string ucode = string.Empty;
                        string code = string.Empty;
                        string group_code = Session["group_code"].ToString();
                        if (group_code.Contains(';'))
                        {
                            string[] group_semi = group_code.Split(';');
                            group_code = group_semi[0].ToString();
                        }
                        if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
                        {
                            ucode = group_code;
                            code = "group_code=" + ucode + "";
                            grouporusercode1 = "  and group_code=" + Session["group_code"].ToString().Trim() + "";
                        }
                        else
                        {
                            ucode = Session["usercode"].ToString();
                            code = "usercode=" + ucode + "";
                            grouporusercode1 = "  and usercode=" + Session["usercode"].ToString().Trim() + "";
                        }
                        string rightscopy = "select * from Master_Settings where settings='Copy Attendance'  and " + code + "";
                        DataSet ds = new DataSet();
                        ds = dacces2.select_method_wo_parameter(rightscopy, "text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["value"].ToString() == "1")
                            {
                                btncopy.Enabled = true;
                            }
                            else if (ds.Tables[0].Rows[0]["value"].ToString() == "0")
                            {
                                btncopy.Enabled = false;
                            }
                        }
                        else
                        {
                            btncopy.Enabled = false;
                        }
                        string strgetsec = dacces2.GetFunction("select sections from tbl_attendance_rights where batch_year='" + ddlbatch.SelectedItem.ToString() + "' and user_id='" + ucode + "' ");
                        if (strgetsec.Trim() != null && strgetsec.Trim() != "0")
                        {
                            string[] spsec = strgetsec.Split(',');
                            for (int sp = 0; sp <= spsec.GetUpperBound(0); sp++)
                            {
                                string valu = spsec[sp].ToString();
                                if (secrights.Trim().ToLower() == valu.Trim().ToLower())
                                {
                                    secrightsflag = true;
                                }
                            }
                        }
                        if (secrightsflag == false)
                        {
                            lblset.Visible = true;
                            lblset.Text = "Please Set Rights For The User";
                            btnsliplist.Visible = false;
                            return;
                        }

                        ds.Reset();
                        ds.Dispose();
                        string strquery = "select start_date,isnull(starting_dayorder,1) as starting_dayorder,schorder,nodays,No_of_hrs_per_day,min_hrs_per_day from seminfo s,periodattndschedule p where s.degree_code=p.degree_code and s.semester=p.semester and batch_year=" + ddlbatch.Text.ToString() + " and s.degree_code=" + ddlbranch.SelectedValue.ToString() + " and s.semester=" + ddlsem.SelectedValue.ToString() + "";
                        ds = dacces2.select_method(strquery, hat, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            sch_order = ds.Tables[0].Rows[0]["schorder"].ToString();
                            no_days = ds.Tables[0].Rows[0]["nodays"].ToString();
                            startdate = ds.Tables[0].Rows[0]["start_date"].ToString();
                            starting_dayorder = ds.Tables[0].Rows[0]["starting_dayorder"].ToString();
                            no_of_hrs = ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                        }

                        string date1 = string.Empty;
                        string date2 = string.Empty;
                        string datefrom;
                        string dateto = string.Empty;

                        date1 = txtFromDate.Text.ToString();
                        string[] split = date1.Split(new Char[] { '-' });
                        datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();

                        date2 = TxtToDate.Text.ToString();
                        string[] split1 = date2.Split(new Char[] { '-' });
                        dateto = split1[1].ToString() + "-" + split1[0].ToString() + "-" + split1[2].ToString();

                        DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
                        DateTime dt2 = Convert.ToDateTime(dateto.ToString());
                        TimeSpan t = dt2.Subtract(dt1);

                        bool daylock = false;
                        daylock = daycheck(Convert.ToDateTime(datefrom.ToString()));
                        if (daylock == false)
                        {
                            lblset.Visible = true;
                            lblset.Text = "From Date is Locked Please Contact Administrator";
                            return;
                        }
                        daylock = daycheck(Convert.ToDateTime(dateto.ToString()));
                        if (daylock == false)
                        {
                            lblset.Visible = true;
                            lblset.Text = "To Date is Locked Please Contact Administrator";
                            return;
                        }
                        long days = t.Days;
                        if (days < 0)
                        {
                            lblset.Visible = true;
                            lblset.Text = "From date should be less than To date";
                            return;
                        }
                        if (no_days != "7") //Deepali on 16.4.18
                        {
                            if (days == 0 && dt1.ToString("dddd") == "Sunday")
                            {
                                lblset.Visible = true;
                                lblset.Text = "Selected Day is Sunday";
                                return;
                            }
                        }
                        if (dt1 > DateTime.Today)
                        {
                            lblset.Visible = true;
                            lblset.Text = "You can not mark attendance for the date greater than today";
                            txtFromDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                            return;
                        }
                        else
                        {
                            datelbl.Visible = false;
                        }
                        if (dt2 > DateTime.Today)
                        {
                            lblset.Visible = true;
                            lblset.Text = "You can not mark attendance for the date greater than today";
                            TxtToDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                            return;
                        }
                        else
                        {
                            datelbl.Visible = false;
                        }
                        //================================commented by Deepali on 18.4.16
                        //ds.Reset();  
                        //ds.Dispose();
                        //string strquery = "select start_date,isnull(starting_dayorder,1) as starting_dayorder,schorder,nodays,No_of_hrs_per_day,min_hrs_per_day from seminfo s,periodattndschedule p where s.degree_code=p.degree_code and s.semester=p.semester and batch_year=" + ddlbatch.Text.ToString() + " and s.degree_code=" + ddlbranch.SelectedValue.ToString() + " and s.semester=" + ddlsem.SelectedValue.ToString() + "";
                        //ds = dacces2.select_method(strquery, hat, "Text");
                        //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        //{
                        //    sch_order = ds.Tables[0].Rows[0]["schorder"].ToString();
                        //    no_days = ds.Tables[0].Rows[0]["nodays"].ToString();
                        //    startdate = ds.Tables[0].Rows[0]["start_date"].ToString();
                        //    starting_dayorder = ds.Tables[0].Rows[0]["starting_dayorder"].ToString();
                        //    no_of_hrs = ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                        //}
                        if (no_of_hrs.Trim() != "")
                        {
                            no_hrs = Convert.ToInt16(no_of_hrs);
                        }
                        else
                        {
                            no_hrs = 0;
                        }
                        //sch_order = GetFunction("Select schorder from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + "");
                        //no_days = GetFunction("Select nodays from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + "");
                        nodays = Convert.ToInt16(nodays);
                        // startdate = GetFunction("select start_date from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " ");
                        //starting_dayorder = GetFunction("select isnull(starting_dayorder,1) as starting_dayorder from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " ");

                        if (starting_dayorder == "")
                        {
                            starting_dayorder = "1";
                        }
                        datelbl.Visible = false;
                        lblset.Visible = false;
                        FpSpread2.Sheets[0].ColumnCount = 5;
                        FpSpread2.Sheets[0].Columns.Default.Width = 0;
                        FpSpread2.Sheets[0].Rows.Default.Height = 25;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = " ";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";

                        //start==========code added by Manikandan
                        FpSpread2.Sheets[0].Cells[0, 0].Locked = true;
                        FpSpread2.Sheets[0].Columns[1].Locked = true;
                        FpSpread2.Sheets[0].Columns[2].Locked = true;
                        FpSpread2.Sheets[0].Columns[3].Locked = true;
                        FpSpread2.Sheets[0].Columns[4].Locked = true;
                        //end============

                        FpSpread2.Sheets[0].FrozenColumnCount = 5;
                        FpSpread2.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].ColumnHeader.Columns[2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].ColumnHeader.Columns[4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Columns[0].Width = 50;
                        FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                        FpSpread2.Sheets[0].Columns[1].CellType = textcel_type;
                        FpSpread2.Sheets[0].Columns[2].CellType = textcel_type;
                        FpSpread2.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread2.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;

                        FarPoint.Web.Spread.ComboBoxCellType objintcell1 = new FarPoint.Web.Spread.ComboBoxCellType();

                        string[] strcomo = new string[20];
                        string[] strcomo1 = new string[20];
                        string[] strcomo1a = new string[20];
                        string[] strcomo2 = new string[20];
                        FarPoint.Web.Spread.ComboBoxCellType objintcell2 = new FarPoint.Web.Spread.ComboBoxCellType();
                        int i = 0, j = 0, z = 0, x = 0;
                        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                        {
                            strquery = "select rights from  OD_Master_Setting where group_code=" + Session["group_code"].ToString() + "";
                        }
                        else
                        {
                            strquery = "select rights from  OD_Master_Setting where usercode=" + Session["UserCode"].ToString() + "";
                        }
                        if (strquery.Trim() != "")
                        {
                            Hashtable od_has = new Hashtable();
                            string od_rights = dacces2.GetFunction(strquery);

                            if (od_rights != null && od_rights.Trim() != "" && od_rights.Trim() != "0")
                            {
                                string[] split_od_rights = od_rights.Split(',');
                                strcomo = new string[split_od_rights.GetUpperBound(0) + 2];
                                strcomo1 = new string[split_od_rights.GetUpperBound(0) + 2];
                                strcomo1a = new string[split_od_rights.GetUpperBound(0) + 3];
                                strcomo2 = new string[split_od_rights.GetUpperBound(0) + 2];
                                strcomo1a[j++] = "Select for All";
                                strcomo[i++] = string.Empty;
                                strcomo1[z++] = string.Empty;
                                strcomo2[x++] = string.Empty;
                                strcomo1a[j++] = string.Empty;
                                for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                                {
                                    strcomo[i++] = split_od_rights[od_temp].ToString();
                                    strcomo1[z++] = split_od_rights[od_temp].ToString();
                                    strcomo1a[j++] = split_od_rights[od_temp].ToString();
                                    strcomo2[x++] = split_od_rights[od_temp].ToString();
                                }
                            }
                            else
                            {
                                strcomo[0] = string.Empty;
                                strcomo[1] = "P";
                                strcomo[2] = "A";
                                strcomo[3] = "OD";
                                strcomo[4] = "SOD";
                                strcomo[5] = "ML";
                                strcomo[6] = "NSS";
                                strcomo[7] = "L";
                                strcomo[8] = "NCC";
                                strcomo[9] = "HS";
                                strcomo[10] = "PP";
                                strcomo[11] = "SYOD";
                                strcomo[12] = "COD";
                                strcomo[13] = "OOD";
                                strcomo[14] = "LA";

                                strcomo1[0] = string.Empty;
                                strcomo1[1] = "P";
                                strcomo1[2] = "A";
                                strcomo1[3] = "OD";
                                strcomo1[4] = "SOD";
                                strcomo1[5] = "ML";
                                strcomo1[6] = "NSS";
                                strcomo1[7] = "L";
                                strcomo1[8] = "NCC";
                                strcomo1[9] = "HS";
                                strcomo1[10] = "PP";
                                strcomo1[11] = "SYOD";
                                strcomo1[12] = "COD";
                                strcomo1[13] = "OOD";
                                strcomo1[14] = "LA";

                                strcomo1a[0] = "Select for All";
                                strcomo1a[1] = string.Empty;
                                strcomo1a[2] = "P";
                                strcomo1a[3] = "A";
                                strcomo1a[4] = "OD";
                                strcomo1a[5] = "SOD";
                                strcomo1a[6] = "ML";
                                strcomo1a[7] = "NSS";
                                strcomo1a[8] = "L";
                                strcomo1a[9] = "NCC";
                                strcomo1a[10] = "HS";
                                strcomo1a[11] = "PP";
                                strcomo1a[12] = "SYOD";
                                strcomo1a[13] = "COD";
                                strcomo1a[14] = "OOD";
                                strcomo1a[15] = "LA";

                                strcomo2[0] = string.Empty;
                                strcomo2[1] = "P";
                                strcomo2[2] = "A";
                                strcomo2[3] = "OD";
                                strcomo2[4] = "SOD";
                                strcomo2[5] = "ML";
                                strcomo2[6] = "NSS";
                                strcomo2[7] = "L";
                                strcomo2[8] = "NCC";
                                strcomo2[9] = "HS";
                                strcomo2[10] = "PP";
                                strcomo2[11] = "SYOD";
                                strcomo2[12] = "COD";
                                strcomo2[13] = "OOD";
                                strcomo2[14] = "LA";
                            }
                        }
                        else
                        {
                            strcomo[0] = string.Empty;
                            strcomo[1] = "P";
                            strcomo[2] = "A";
                            strcomo[3] = "OD";
                            strcomo[4] = "SOD";
                            strcomo[5] = "ML";
                            strcomo[6] = "NSS";
                            strcomo[7] = "L";
                            strcomo[8] = "NCC";
                            strcomo[9] = "HS";
                            strcomo[10] = "PP";
                            strcomo[11] = "SYOD";
                            strcomo[12] = "COD";
                            strcomo[13] = "OOD";
                            strcomo[14] = "LA";

                            strcomo1[0] = string.Empty;
                            strcomo1[1] = "P";
                            strcomo1[2] = "A";
                            strcomo1[3] = "OD";
                            strcomo1[4] = "SOD";
                            strcomo1[5] = "ML";
                            strcomo1[6] = "NSS";
                            strcomo1[7] = "L";
                            strcomo1[8] = "NCC";
                            strcomo1[9] = "HS";
                            strcomo1[10] = "PP";
                            strcomo1[11] = "SYOD";
                            strcomo1[12] = "COD";
                            strcomo1[13] = "OOD";
                            strcomo1[14] = "LA";

                            strcomo1a[0] = "Select for All";
                            strcomo1a[1] = string.Empty;
                            strcomo1a[2] = "P";
                            strcomo1a[3] = "A";
                            strcomo1a[4] = "OD";
                            strcomo1a[5] = "SOD";
                            strcomo1a[6] = "ML";
                            strcomo1a[7] = "NSS";
                            strcomo1a[8] = "L";
                            strcomo1a[9] = "NCC";
                            strcomo1a[10] = "HS";
                            strcomo1a[11] = "PP";
                            strcomo1a[12] = "SYOD";
                            strcomo1a[13] = "COD";
                            strcomo1a[14] = "OOD";
                            strcomo1a[15] = "LA";

                            strcomo2[0] = string.Empty;
                            strcomo2[1] = "P";
                            strcomo2[2] = "A";
                            strcomo2[3] = "OD";
                            strcomo2[4] = "SOD";
                            strcomo2[5] = "ML";
                            strcomo2[6] = "NSS";
                            strcomo2[7] = "L";
                            strcomo2[8] = "NCC";
                            strcomo2[9] = "HS";
                            strcomo2[10] = "PP";
                            strcomo2[11] = "SYOD";
                            strcomo2[12] = "COD";
                            strcomo2[13] = "OOD";
                            strcomo2[14] = "LA";
                        }

                        objintcell1 = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
                        objintcell1.ShowButton = true;
                        objintcell1.AutoPostBack = true;
                        objintcell1.UseValue = true;
                        FpSpread2.Sheets[0].Columns[0].CellType = objintcell1;
                        FpSpread2.SaveChanges();
                        FpSpread2.Sheets[0].Columns[0].BackColor = Color.MistyRose;

                        FpSpread2.Sheets[0].ColumnHeader.Columns[1].Visible = false;
                        FpSpread2.Sheets[0].ColumnHeader.Columns[2].Visible = false;
                        FpSpread2.Sheets[0].ColumnHeader.Columns[4].Visible = false;
                        if (Session["Rollflag"].ToString() != "0")
                        {
                            FpSpread2.Sheets[0].ColumnHeader.Columns[1].Visible = true;
                            FpSpread2.Sheets[0].Columns[1].Width = 100;
                        }
                        if (Session["Regflag"].ToString() != "0")
                        {
                            FpSpread2.Sheets[0].ColumnHeader.Columns[2].Visible = true;
                            FpSpread2.Sheets[0].Columns[2].Width = 100;
                        }
                        if (Session["Studflag"].ToString() != "0")
                        {
                            FpSpread2.Sheets[0].ColumnHeader.Columns[4].Visible = true;
                            FpSpread2.Sheets[0].Columns[4].Width = 100;
                        }
                        FpSpread2.Sheets[0].Columns[3].Width = 200;
                        FpSpread2.SaveChanges();
                        FpSpread2.Sheets[0].AutoPostBack = false;
                        FpSpread2.Sheets[0].RowCount = 1;
                        FpSpread2.Sheets[0].Cells[0, 0].CellType = textcel_type;
                        FpSpread2.Sheets[0].Cells[0, 0].BackColor = Color.White;
                        FarPoint.Web.Spread.ComboBoxCellType objintcell = new FarPoint.Web.Spread.ComboBoxCellType();
                        objintcell = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1a);
                        objintcell.ShowButton = true;
                        objintcell.AutoPostBack = true;
                        objintcell.UseValue = true;
                        FpSpread2.SaveChanges();

                        FarPoint.Web.Spread.ComboBoxCellType objcom = new FarPoint.Web.Spread.ComboBoxCellType();
                        objcom = new FarPoint.Web.Spread.ComboBoxCellType(strcomo);
                        objcom.UseValue = true;
                        objintcell.AutoPostBack = true;

                        strorder = "ORDER BY registration.Roll_No";
                        string serialno = dacces2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
                        if (serialno.Trim() == "1")
                        {
                            serialflag = true;
                            strorder = "ORDER BY registration.serialno";
                        }
                        else
                        {
                            serialflag = false;
                            string orderby_Setting = GetFunction("select value from master_Settings where settings='order_by'");
                            if (orderby_Setting == "0")
                            {
                                strorder = "ORDER BY Registration.Roll_No";
                            }
                            else if (orderby_Setting == "1")
                            {
                                strorder = "ORDER BY Registration.Reg_No";
                            }
                            else if (orderby_Setting == "2")
                            {
                                strorder = "ORDER BY Registration.Stud_Name";
                            }
                            else if (orderby_Setting == "0,1,2")
                            {
                                strorder = "ORDER BY Registration.Roll_No,Registration.Reg_No,Registration.Stud_Name";
                            }
                            else if (orderby_Setting == "0,1")
                            {
                                strorder = "ORDER BY Registration.Roll_No,Registration.Reg_No";
                            }
                            else if (orderby_Setting == "1,2")
                            {
                                strorder = "ORDER BY Registration.Reg_No,Registration.Stud_Name";
                            }
                            else if (orderby_Setting == "0,2")
                            {
                                strorder = "ORDER BY Registration.Roll_No,Registration.Stud_Name";
                            }
                        }
                        string Discon = dacces2.GetFunction("select value from Master_Settings where settings='Attendance Discount'  " + grouporusercode1 + " ");
                        string debar = dacces2.GetFunction("select value from Master_Settings where settings='Attendance Debar'  " +grouporusercode1+"");
                        string dis = string.Empty;
                        string deba = string.Empty;
                        if (Discon == "1" || Discon.Trim().ToLower() == "true")
                            dis = string.Empty;
                        else
                            dis = "  and delflag=0";

                        if (debar == "1" || debar.Trim().ToLower() == "true")
                            deba = string.Empty;
                        else
                            deba = "  and exam_flag <> 'DEBAR'";

                        if (days >= 0)
                        {
                            string sqlstrq;
                            string sqlstr;
                            sqlstr = "select Registration.app_no,Registration.roll_no,Registration.reg_no, Registration.stud_name,Registration.stud_type,registration.serialno,Registration.Adm_Date,Registration.delflag,Registration.exam_flag from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + " and registration.current_semester=" + ddlsem.SelectedValue.ToString() + " and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 " + dis + " " + deba + " " + strsec + " " + Session["strvar"] + " and adm_date<='" + dateto + "' " + strorder + "";
                            //if (serialflag == false)
                            //{
                            //    //----check the condn for order by rollno and regno-------added by mythili 10.08.12
                            //    ////string orderby_Setting = GetFunction("select value from master_Settings where settings='order_by'");// and usercode=" + Session["usercode"] + "

                            //    ////if (orderby_Setting != "") 
                            //    ////{
                            //    ////    if (orderby_Setting == "1")//if 1 means roll no
                            //    ////    {
                            //    ////        sqlstr = "select roll_no,reg_no, registration.stud_name,registration.stud_type from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + " and registration.current_semester=" + ddlsem.SelectedValue.ToString() + " and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and a.degree_code = registration.degree_code " + strsec + " " + Session["strvar"] + " and adm_date<='" + dateto + "' order by  roll_no ";
                            //    ////    }
                            //    ////    else //if 0 means reg no
                            //    ////    {
                            //    ////        sqlstr = "select roll_no,reg_no, registration.stud_name,registration.stud_type from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + " and registration.current_semester=" + ddlsem.SelectedValue.ToString() + " and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and a.degree_code = registration.degree_code " + strsec + " " + Session["strvar"] + " and adm_date<='" + dateto + "' order by  reg_no ";
                            //    ////    }
                            //    ////}
                            //    if (strorder != "")
                            //    {
                            //        sqlstr = "select registration.roll_no,registration.reg_no, registration.stud_name,registration.stud_type from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + " and registration.current_semester=" + ddlsem.SelectedValue.ToString() + " and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and a.degree_code = registration.degree_code " + strsec + " " + Session["strvar"] + " and adm_date<='" + dateto + "' " + strorder;
                            //    }

                            //    else//this is for order by not selected in settings
                            //    {
                            //        sqlstr = "select registration.roll_no,registration.reg_no, registration.stud_name,registration.stud_type from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + " and registration.current_semester=" + ddlsem.SelectedValue.ToString() + " and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and a.degree_code = registration.degree_code " + strsec + " " + Session["strvar"] + " and adm_date<='" + dateto + "' order by  roll_no ";
                            //    }

                            //}
                            //else
                            //{
                            //    sqlstr = "select registration.roll_no,registration.reg_no, registration.stud_name,registration.stud_type from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlbranch.SelectedValue.ToString() + " and registration.current_semester=" + ddlsem.SelectedValue.ToString() + " and registration.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 and a.degree_code = registration.degree_code " + strsec + " " + Session["strvar"] + " and adm_date<='" + dateto + "' order by  roll_no ";
                            //}

                            DataSet dsstudent = dacces2.select_method(sqlstr, hat, "Text");
                            if (dsstudent.Tables.Count > 0 && dsstudent.Tables[0].Rows.Count > 0)
                            {
                                FpSpread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                                FpSpread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                                for (int stu = 0; stu < dsstudent.Tables[0].Rows.Count; stu++)
                                {
                                    FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 1;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = dsstudent.Tables[0].Rows[stu]["Roll_no"].ToString();
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Tag = dsstudent.Tables[0].Rows[stu]["app_no"].ToString();
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = dsstudent.Tables[0].Rows[stu]["reg_no"].ToString();
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = dsstudent.Tables[0].Rows[stu]["stud_name"].ToString();
                                    FpSpread2.Sheets[0].RowHeader.Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread2.Sheets[0].RowCount - 1);
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = dsstudent.Tables[0].Rows[stu]["stud_type"].ToString();
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Tag = dsstudent.Tables[0].Rows[stu]["Adm_Date"].ToString();
                                    if (dsstudent.Tables[0].Rows[stu]["stud_type"].ToString() == "Hostler")
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].BackColor = Color.LightYellow;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].BackColor = Color.LightYellow;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].BackColor = Color.LightYellow;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].BackColor = Color.LightYellow;
                                    }
                                    else if (Convert.ToString(dsstudent.Tables[0].Rows[stu]["exam_flag"]).Trim().ToLower() == "debar" || Convert.ToString(dsstudent.Tables[0].Rows[stu]["delflag"]).Trim().ToLower() == "1" || Convert.ToString(dsstudent.Tables[0].Rows[stu]["delflag"]).Trim().ToLower()=="true")//delflag
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].BackColor = Color.Red;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].BackColor = Color.Red;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].BackColor = Color.Red;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].BackColor = Color.Red;
                                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Locked = true;
                                    }
                                    else
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].BackColor = Color.MediumSeaGreen;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].BackColor = Color.MediumSeaGreen;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].BackColor = Color.MediumSeaGreen;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].BackColor = Color.MediumSeaGreen;
                                    }
                                }
                            }
                            else
                            {
                                lblset.Visible = true;
                                lblset.Text = "There are no students available";
                                btnsliplist.Visible = false;
                                return;
                            }
                            //da.Close();
                            // mysql.Close();
                            //sqlstrq = "select * from PeriodAttndSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + "";

                            //string noofhours = GetFunction(sqlstrq);
                            //Modified by Muthu..................................................................................................................................................................................................................................................

                            //con.Close();
                            //con.Open();

                            //SqlCommand cmd_get_hours = new SqlCommand("select * from PeriodAttndSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + "", con);
                            //SqlDataAdapter ad_get_hours = new SqlDataAdapter(cmd_get_hours);
                            //DataTable dt_get_hours = new DataTable();
                            //ad_get_hours.Fill(dt_get_hours);
                            ds.Reset();
                            ds.Dispose();
                            strquery = "select * from PeriodAttndSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + "";
                            ds = dacces2.select_method(strquery, hat, "Text");
                            //string Queryvalue = "select * from PeriodAttndSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + "";
                            //DataSet dt_get_hours = dacces2.select_method(Queryvalue, hat, "Text");
                            string noofhours = no_hrs.ToString();
                            //if (dt_get_hours.Tables[0].Rows.Count > 0)//Added By Srinath 17/8/2013
                            //{
                            //    noofhours = dt_get_hours.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                            //}
                            //else
                            //{
                            //    noofhours = "0";
                            //}

                            //string numberhor =string.Empty;
                            //numberhor = noofhours;
                            string str = string.Empty;
                            str = txtFromDate.Text;
                            lblset.Text = string.Empty;
                        HOLDAY:
                            if (no_days != "7")//Deepali on 16.4.18
                            {
                                if (dt1.ToString("dddd") == "Sunday")
                                {
                                    lblset.Visible = true;
                                    lblset.Text = lblset.Text + dt1.ToString("d-MM-yyyy") + "-holiday" + " Sunday  ";
                                    if (dt2 != dt1)
                                    {
                                        dt1 = dt1.AddDays(1);
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                            }

                            int spancolumn = Convert.ToInt32(noofhours);
                            string half_full = string.Empty;
                            string morning_h = string.Empty;
                            string evening_h = string.Empty;
                            int starthour = 1;
                            //strquery = "select * from holidaystudents where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and holiday_date='" + dt1.ToString("yyyy-MM-d") + "'";
                            //DataSet datest = new DataSet();
                            //datest = dacces2.select_method(strquery, hat, "Text");                            
                            //if (datest.Tables[0].Rows.Count > 0)
                            //{
                            //    //half_full = dt_get_holiday.Rows[0]["halforfull"].ToString();
                            //    //morning_h = dt_get_holiday.Rows[0]["morning"].ToString();
                            //    //evening_h = dt_get_holiday.Rows[0]["evening"].ToString();
                            //    half_full = datest.Tables[0].Rows[0]["halforfull"].ToString();
                            //    morning_h = datest.Tables[0].Rows[0]["morning"].ToString();
                            //    evening_h = datest.Tables[0].Rows[0]["evening"].ToString();
                            //}

                            //if (half_full == "False")
                            //{
                            //    lblset.Visible = true;
                            //    lblset.Text = lblset.Text + dt1.ToString("d-MM-yyyy") + "-holiday";
                            //    if (dt2 != dt1)
                            //    {
                            //        dt1 = dt1.AddDays(1);
                            //        goto HOLDAY;
                            //    }
                            //    else
                            //    {
                            //        return;
                            //    }
                            //}
                            //else if (half_full == "True" && morning_h == "True")
                            //{
                            //    starthour = Convert.ToInt32(noofhours) - Convert.ToInt32(ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString());
                            //    starthour = starthour + 1;
                            //    spancolumn = Convert.ToInt32(noofhours) - Convert.ToInt32(ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString()); ;
                            //    // noofhours = dt_get_hours.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString();
                            //    //holiday_flag = "E";
                            //}
                            //else if (half_full == "True" && evening_h == "True")
                            //{
                            //    noofhours = ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
                            //    spancolumn = Convert.ToInt32(noofhours) - Convert.ToInt32(ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString()); ;
                            //    //noofhours = dt_get_hours.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
                            //    //holiday_flag = "M";
                            //}

                            //Day_Order =string.Empty;


                            if (noofhours.ToString().Trim() != "" && noofhours != "0" && noofhours.ToString() != null)
                            {
                                //for (int ini_loop = starthour; ini_loop <= Convert.ToInt32(noofhours.ToString()); ini_loop++)
                                //{
                                //    if (dt1 > dt2)
                                //        break;
                                //    FpSpread2.Sheets[0].SheetCorner.RowCount = 2; //Set Date
                                //    FpSpread2.Sheets[0].ColumnCount = FpSpread2.Sheets[0].ColumnCount + 1;

                                FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 80;
                                if (Convert.ToString(Day_Order).Trim() != "")
                                {
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = dt1.ToString("d-MM-yyyy") + " [Day Order " + Day_Order.ToString() + "]";
                                }
                                else
                                {
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = dt1.ToString("d-MM-yyyy");
                                }

                                //    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ini_loop);
                                //    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Tag = dt1;
                                //    // FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].CellType = objintcell;   //Set Hour
                                //    //FpSpread2.ActiveSheetView.Columns[FpSpread2.Sheets[0].ColumnCount - 1].CellType = objcom;

                                //    FpSpread2.ActiveSheetView.Columns[FpSpread2.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                //}
                                //FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 1, spancolumn);

                                FpSpread2.Sheets[0].SheetCorner.RowCount = 2;
                                string[] differdays = new string[500];
                                DateTime temp_date = dt1;
                                int date_loop = 0;
                                //  for (int date_loop = 1; date_loop <= days; date_loop++) //Next Next Date
                                while (temp_date <= dt2)
                                {
                                    temp_date = dt1.AddDays(date_loop);
                                    starthour = 1;
                                    spancolumn = 1;
                                    Day_Order = string.Empty;
                                    if (temp_date <= dt2)
                                    {
                                        //if (no_hrs > 0)
                                        //{
                                        //    if (sch_order != "0")
                                        //    {
                                        //        srt_day = temp_date.ToString("ddd");
                                        //    }
                                        //    else
                                        //    {
                                        //        string[] tmpdate = temp_date.ToString().Split(new char[] { ' ' });
                                        //        string currdate = tmpdate[0].ToString();
                                        //        string[] tmpdate1 = startdate.ToString().Split(new char[] { ' ' });
                                        //        string startdate1 = tmpdate1[0].ToString();
                                        //        //modified by srinnath
                                        //        //srt_day = findday(currdate.ToString(), startdate1.ToString(), no_days.ToString(), starting_dayorder.ToString());
                                        //        srt_day = dacces2.findday(currdate.ToString(), ddlbranch.SelectedValue.ToString(), ddlsem.SelectedItem.ToString(), ddlbatch.Text, startdate1.ToString(), no_days.ToString(), starting_dayorder.ToString());
                                        //    }
                                        //}

                                        if (no_hrs > 0)
                                        {
                                            if (sch_order != "0")
                                            {
                                                srt_day = dt1.ToString("ddd");
                                                Day_Order = string.Empty;
                                            }
                                            else
                                            {
                                                string[] tmpdate = dt1.ToString().Split(new char[] { ' ' });
                                                string currdate = tmpdate[0].ToString();
                                                string[] tmpdate1 = startdate.ToString().Split(new char[] { ' ' });
                                                string startdate1 = tmpdate1[0].ToString();
                                                string cur1 = temp_date.ToString().Split(new char[] { ' ' })[0].ToString();//Modified by Rajkumar on 7-1-2018
                                                //   srt_day = findday(currdate.ToString(), startdate1.ToString(), no_days.ToString(), starting_dayorder.ToString());
                                                srt_day = dacces2.findday(cur1.ToString(), ddlbranch.SelectedValue.ToString(), ddlsem.SelectedValue.ToString(), ddlbatch.SelectedValue.ToString(), startdate1.ToString(), no_days.ToString(), starting_dayorder);
                                                if (srt_day.Trim().ToLower() == "mon")
                                                    Day_Order = "1";
                                                else if (srt_day.Trim().ToLower() == "tue")
                                                    Day_Order = "2";
                                                else if (srt_day.Trim().ToLower() == "wed")
                                                    Day_Order = "3";
                                                else if (srt_day.Trim().ToLower() == "thu")
                                                    Day_Order = "4";
                                                else if (srt_day.Trim().ToLower() == "fri")
                                                    Day_Order = "5";
                                                else if (srt_day.Trim().ToLower() == "sat")
                                                    Day_Order = "6";
                                                else if (srt_day.Trim().ToLower() == "sun")
                                                    Day_Order = "7";
                                                Day_Order = Day_Order + "-" + Convert.ToString(srt_day);
                                            }
                                        }

                                        strquery = "select * from holidaystudents where holiday_date='" + temp_date + "'and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + "";
                                        dsholiday.Reset();
                                        dsholiday.Dispose();
                                        dsholiday = dacces2.select_method_wo_parameter(strquery, "Text");

                                        string holi_des = string.Empty;
                                        string holi_value = string.Empty;
                                        DateTime holidate;

                                        if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
                                        {
                                            holi_des = dsholiday.Tables[0].Rows[0][2].ToString();
                                            holi_value = dsholiday.Tables[0].Rows[0][4].ToString();
                                        }

                                        if (holi_des != "" && holi_value.Trim().ToLower() == "false")
                                        {
                                            lblset.Visible = true;
                                            holidate = Convert.ToDateTime(dsholiday.Tables[0].Rows[0][1].ToString());//Added by Manikandan 30/07/2013
                                            if (no_days != "7") //Deepali on 16.4.18
                                            {
                                                if (temp_date.ToString("dddd").Trim().ToLower() == "sunday")
                                                {
                                                    lblset.Text = "    " + lblset.Text + holidate.ToString("d/MM/yyyy") + "-holiday" + " Sunday  ";//Modified by Manikandan 30/07/2013
                                                }
                                                else
                                                {
                                                    lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "-holiday";
                                                }
                                            }
                                            //==== added by Deepali on 16.4.18
                                            if (no_days == "7") 
                                            {
                                                lblset.Text = "Selected date (" + holidate.ToString("d/MM/yyyy") + ") marked as Holiday (Sunday)";
                                               
                                            }
                                        }
                                        else
                                        {
                                            if (temp_date > dt2) break;
                                            if (no_days != "7") //Deepali on 16.4.18
                                            {
                                                if (temp_date.ToString("dddd").Trim().ToLower() == "sunday")
                                                {
                                                    lblset.Visible = true;
                                                    lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "-holiday" + " Sunday  ";
                                                    date_loop++;
                                                    continue;
                                                }
                                            }
                                            if (half_full.Trim().ToLower() == "false")
                                            {
                                                lblset.Visible = true;
                                                lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "-holiday";
                                                date_loop++;
                                                continue;
                                            }
                                            else
                                            {
                                                if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
                                                {
                                                    lblset.Visible = true;
                                                    bool morse = Convert.ToBoolean(dsholiday.Tables[0].Rows[0]["morning"]);
                                                    bool evese = Convert.ToBoolean(dsholiday.Tables[0].Rows[0]["evening"]);
                                                    if (morse == true)
                                                    {
                                                        lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "- is Morning holiday";
                                                    }
                                                    else
                                                    {
                                                        lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "- is Evening holiday";
                                                    }
                                                }
                                            }
                                            differdays[date_loop] = temp_date.ToString("d-MM-yyyy");
                                            i = 0;
                                            string dateformat;

                                            half_full = string.Empty;
                                            morning_h = string.Empty;
                                            evening_h = string.Empty;

                                            if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
                                            {
                                                half_full = dsholiday.Tables[0].Rows[0]["halforfull"].ToString();
                                                morning_h = dsholiday.Tables[0].Rows[0]["morning"].ToString();
                                                evening_h = dsholiday.Tables[0].Rows[0]["evening"].ToString();
                                            }

                                            spancolumn = Convert.ToInt32(noofhours);
                                            if (half_full == "True" && morning_h == "True")
                                            {
                                                // noofhours = dt_get_hours.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString();
                                                starthour = Convert.ToInt32(noofhours) - Convert.ToInt32(ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString());
                                                starthour = starthour + 1;
                                                //spancolumn = Convert.ToInt32(noofhours) - Convert.ToInt32(ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString());
                                                spancolumn = Convert.ToInt32(ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString()); //aruna 25feb2013
                                                //holiday_flag = "M";
                                            }
                                            else if (half_full == "True" && evening_h == "True")
                                            {
                                                noofhours = ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
                                                if (noofhours == "" && noofhours == null)
                                                {
                                                    noofhours = "0";
                                                }
                                                //spancolumn = Convert.ToInt32(noofhours) - Convert.ToInt32(ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString());
                                                spancolumn = Convert.ToInt32(ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString());  //aruna 25feb2013
                                            }
                                            else
                                            {
                                                noofhours = ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                                                //noofhours = dacces2.GetFunction("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + "");
                                                if (noofhours == "" && noofhours == null)
                                                {
                                                    noofhours = "0";
                                                }
                                            }

                                            for (i = starthour; i <= Convert.ToInt32(noofhours.ToString()); i++)
                                            {
                                                FpSpread2.Sheets[0].ColumnCount = FpSpread2.Sheets[0].ColumnCount + 1;
                                                dateformat = FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(differdays[date_loop]);
                                                if (Convert.ToString(Day_Order).Trim() != "")
                                                {
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = dateformat.ToString() + " [Day Order " + Day_Order.ToString() + "]";
                                                }
                                                else
                                                {
                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = dateformat.ToString();
                                                }
                                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Tag = dateformat.ToString();

                                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(i);
                                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Tag = temp_date;
                                                FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].BackColor = Color.MistyRose;
                                                FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 80;
                                            }
                                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - spancolumn, 1, spancolumn);
                                        }
                                        // temp_date = dt1.AddDays(date_loop);
                                        date_loop++;
                                    }
                                }
                                int temp = 0;//1
                                for (temp = 5; temp < FpSpread2.Sheets[0].ColumnCount; temp++)
                                {
                                    FpSpread2.Sheets[0].Cells[0, temp].CellType = objintcell;
                                }
                                FpSpread2.Sheets[0].RowHeader.Cells[0, 0].Text = " ";
                                for (temp = 5; temp < FpSpread2.Sheets[0].ColumnCount; temp++)
                                {
                                    FpSpread2.Sheets[0].Columns[temp].CellType = objcom;
                                }

                                string str_Date;
                                string str_day;
                                string Atmonth;
                                string Atyear;
                                long strdate;
                                string rollno_Att = string.Empty;
                                string Att_dcolumn = string.Empty;
                                string Att_Markvalue;
                                string Att_Mark1;
                                //string Discon = dacces2.GetFunction("select value from Master_Settings where settings='Attendance Discount'");
                                //string debar = dacces2.GetFunction("select value from Master_Settings where settings='Attendance Debar'");
                                //string dis = string.Empty;
                                //string deba = string.Empty;
                                if (Discon == "1" || Discon.Trim().ToLower() == "true")
                                    dis = string.Empty;
                                else
                                    dis = "  and delflag=0";

                                if (debar == "1" || debar.Trim().ToLower() == "true")
                                    deba = string.Empty;
                                else
                                    deba = "  and exam_flag <> 'DEBAR'";

                                temp = 0;
                                Hashtable hatsuspent = new Hashtable();
                                string strsuspen = "select s.roll_no from stucon s,Registration r where r.Roll_No=s.roll_no and ack_susp=1 and tot_days>0 and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.current_semester=" + ddlsem.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 " + dis + " " + deba + " " + sec + "";//and exam_flag <> 'DEBAR' and delflag=0
                                ds_attndmaster.Reset();
                                ds_attndmaster.Dispose();
                                ds_attndmaster = dacces2.select_method(strsuspen, hatsuspent, "Text");
                                if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
                                {
                                    for (int su = 0; su < ds_attndmaster.Tables[0].Rows.Count; su++)
                                    {
                                        rollno_Att = ds_attndmaster.Tables[0].Rows[su]["ROll_no"].ToString();
                                        if (!hatsuspent.Contains(rollno_Att))
                                        {
                                            hatsuspent.Add(rollno_Att.Trim().ToLower(), rollno_Att);
                                        }
                                    }
                                }
                               
                                if (Discon == "1" || Discon.Trim().ToLower() == "true")
                                    dis = string.Empty;
                                else
                                    dis = "  and delflag=0";

                                if (debar == "1" || debar.Trim().ToLower() == "true")
                                    deba = string.Empty;
                                else
                                    deba = "  and exam_flag <> 'DEBAR'";

                                sqlstr = "select a.* from attendance a,Registration r where r.Roll_No=a.roll_no and r.degree_code=" + ddlbranch.SelectedValue.ToString() + " and r.current_semester=" + ddlsem.SelectedValue.ToString() + " and r.batch_year=" + ddlbatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 " + dis + " " + deba + " " + sec + " and adm_date<='" + dateto + "'";
                                ds_attndmaster = dacces2.select_method(sqlstr, hat, "Text");
                                DataView dvatt = new DataView();
                                string monthyear = string.Empty;

                                for (Att_mark_row = 1; Att_mark_row < FpSpread2.Sheets[0].RowCount; Att_mark_row++)
                                {
                                    rollno_Att = FpSpread2.Sheets[0].Cells[Att_mark_row, 1].Text.ToString();
                                    for (Att_mark_column = 5; Att_mark_column < FpSpread2.Sheets[0].ColumnCount; Att_mark_column++)
                                    {
                                        str_Date = FpSpread2.Sheets[0].ColumnHeader.Cells[0, Att_mark_column].Text;
                                        string[] tmpdate = str_Date.ToString().Split(new char[] { ' ' });
                                        str_Date = tmpdate[0].ToString();

                                        rollno_Att = FpSpread2.Sheets[0].Cells[Att_mark_row, 1].Text.ToString();
                                        FpSpread2.Sheets[0].RowHeader.Cells[Att_mark_row, 0].Text = Att_mark_row.ToString();
                                        string[] sp = str_Date.Split(new Char[] { '-' });
                                        str_day = sp[0].ToString();
                                        Atmonth = sp[1].ToString();
                                        Atyear = sp[2].ToString();
                                        strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                                        int Att_hour;
                                        Att_hour = Convert.ToInt32(FpSpread2.Sheets[0].ColumnHeader.Cells[1, Att_mark_column].Text);
                                        Att_dcolumn = "d" + Convert.ToInt16(str_day) + "d" + Att_hour;
                                        DateTime dtcurdate = Convert.ToDateTime(sp[1] + '/' + sp[0] + '/' + sp[2]);
                                        string rollNo = string.Empty;
                                        bool checkedFeeOfRoll = false;
                                        rollNo = Convert.ToString(rollno_Att).Trim().ToLower();
                                        if (dicFeeOfRollStudents.ContainsKey(rollNo.Trim()) && dicFeeOnRollStudents.ContainsKey(rollNo.Trim()))
                                        {
                                            DateTime[] dtFeeOfRoll = dicFeeOfRollStudents[rollNo.Trim()];
                                            DateTime dtSelDate = new DateTime();
                                            dtSelDate = Convert.ToDateTime(dtcurdate);
                                            string dtadntdate = dacces2.GetFunction("select adm_date from registration where Roll_No ='" + rollNo + "'");
                                            DateTime dtadm = Convert.ToDateTime(dtadntdate);
                                            if (dtadm <= dtSelDate)
                                            {
                                                if (dtSelDate >= dtFeeOfRoll[0])
                                                {
                                                    DateTime dtDefaultDate = new DateTime(1900, 1, 1);//SqlServer Default Date
                                                    if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtSelDate < dtFeeOfRoll[1])
                                                    {
                                                        checkedFeeOfRoll = true;
                                                    }
                                                    else if (dicFeeOnRollStudents[rollNo.Trim()] == 1)
                                                    {
                                                        checkedFeeOfRoll = true;
                                                    }
                                                    else if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtFeeOfRoll[1] == dtDefaultDate)
                                                    {
                                                        checkedFeeOfRoll = true;
                                                    }
                                                    //else if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtSelDate >= dtFeeOfRoll[1])
                                                    //{
                                                    //    checkedFeeOfRoll = false;
                                                    //}
                                                    else
                                                    {
                                                        checkedFeeOfRoll = false;
                                                    }
                                                }
                                                else
                                                {
                                                    checkedFeeOfRoll = false;
                                                }
                                            }
                                            else
                                            {
                                                checkedFeeOfRoll = false;
                                            }
                                        }
                                        if (checkedFeeOfRoll)
                                        {
                                            FarPoint.Web.Spread.TextCellType tc = new FarPoint.Web.Spread.TextCellType();
                                            FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].CellType = tc;
                                            FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text = "A";
                                            FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Locked = true;
                                            FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Tag = "2";
                                            FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].BackColor = Color.Red;
                                        }
                                        else
                                        {
                                            if (hatsuspent.Contains(rollno_Att.Trim().ToLower()))
                                            {
                                                int splitackdate = Convert.ToInt32(sp[0]);
                                                int splitackmonth = Convert.ToInt32(sp[1]);
                                                int splitackyear = Convert.ToInt32(sp[2]);

                                                string concat_susdate = splitackmonth + "/" + splitackdate + "/" + splitackyear;
                                                string suspend_qry = "select  convert(varchar(15),dateadd(day,tot_days-1,ack_date),1) as action_days,ack_date,tot_days from stucon where ack_susp=1 and tot_days>0 and roll_no='" + rollno_Att.ToString() + "' and Convert(date,ack_date)<= '" + concat_susdate.ToString() + "'";    //modified by prabha on feb 14 2018
                                                DataSet ds_suspend = new DataSet();
                                                ds_suspend = dacces2.select_method(suspend_qry, hat, "Text");
                                                if (ds_suspend.Tables.Count > 0 && ds_suspend.Tables[0].Rows.Count > 0)
                                                {
                                                    DateTime dt_curr = Convert.ToDateTime(concat_susdate.ToString());
                                                    DateTime dt_act = Convert.ToDateTime(ds_suspend.Tables[0].Rows[0]["action_days"].ToString());
                                                    TimeSpan t_con = dt_act.Subtract(dt_curr);
                                                    long daycon = t_con.Days;

                                                    DateTime dt_curr1 = Convert.ToDateTime(ds_suspend.Tables[0].Rows[0]["ack_date"].ToString());
                                                    DateTime dt_act1 = Convert.ToDateTime(concat_susdate.ToString());
                                                    TimeSpan t_con1 = dt_act1.Subtract(dt_curr1);
                                                    long daycon1 = t_con1.Days;
                                                    long totalactdays = Convert.ToInt32(ds_suspend.Tables[0].Rows[0]["tot_days"]);
                                                    if ((Convert.ToInt32(daycon + daycon1) == totalactdays - 1) && (daycon >= 0))
                                                    {
                                                        FarPoint.Web.Spread.TextCellType tc = new FarPoint.Web.Spread.TextCellType();
                                                        FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].CellType = tc;
                                                        FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text = "S";
                                                        FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Locked = true;
                                                        FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Tag = "9";
                                                    }
                                                    else
                                                    {
                                                        DateTime dtadm = Convert.ToDateTime(FpSpread2.Sheets[0].Cells[Att_mark_row, 4].Tag.ToString());
                                                        if (dtadm <= dtcurdate)
                                                        {
                                                            ds_attndmaster.Tables[0].DefaultView.RowFilter = " Roll_no='" + rollno_Att + "' and month_year='" + strdate + "'";
                                                            dvatt = ds_attndmaster.Tables[0].DefaultView;
                                                            if (dvatt.Count > 0)
                                                            {
                                                                Att_Markvalue = dvatt[0]["" + Att_dcolumn + ""].ToString();
                                                                Att_Mark1 = Attmark(Att_Markvalue);
                                                                FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Tag = Att_Markvalue.ToString();
                                                                FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text = Att_Mark1.ToString();
                                                                FpSpread2.Sheets[0].AutoPostBack = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Tag = "8";
                                                            FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text = "NJ";
                                                            FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Locked = true;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                DateTime dtadm = Convert.ToDateTime(FpSpread2.Sheets[0].Cells[Att_mark_row, 4].Tag.ToString());
                                                if (dtadm <= dtcurdate)
                                                {
                                                    if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
                                                    {
                                                        ds_attndmaster.Tables[0].DefaultView.RowFilter = " Roll_no='" + rollno_Att + "' and month_year='" + strdate + "'";
                                                        dvatt = ds_attndmaster.Tables[0].DefaultView;
                                                    }
                                                    if (dvatt.Count > 0)
                                                    {
                                                        Att_Markvalue = dvatt[0]["" + Att_dcolumn + ""].ToString();
                                                        Att_Mark1 = Attmark(Att_Markvalue);
                                                        FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Tag = Att_Markvalue.ToString();
                                                        FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text = Att_Mark1.ToString();
                                                        FpSpread2.Sheets[0].AutoPostBack = false;
                                                    }
                                                }
                                                else
                                                {
                                                    FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Tag = "8";
                                                    FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text = "NJ";
                                                    FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Locked = true;
                                                }
                                                //for (int att = 0; att < ds_attndmaster.Tables[0].Rows.Count; att++)
                                                //{
                                                //    if (rollno_Att == ds_attndmaster.Tables[0].Rows[att]["Roll_no"].ToString())
                                                //    {
                                                //        Att_Markvalue = ds_attndmaster.Tables[0].Rows[att]["" + Att_dcolumn + ""].ToString();
                                                //        //Att_strqueryst= "select " + Att_dcolumn + " from Attendance where Roll_no='" + rollno_Att.ToString() + "' and month_year=" + strdate.ToString() + "";

                                                //        // Att_Markvalue = GetFunction(Att_strqueryst);
                                                //        Att_Mark1 = Attmark(Att_Markvalue);
                                                //        FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Tag = Att_Markvalue.ToString();
                                                //        FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text = Att_Mark1.ToString();

                                                //        FpSpread2.Sheets[0].AutoPostBack = false;
                                                //        if (Att_Mark1 != "" && Att_Mark1 != "NE") //Added by srinath 23/8/2013
                                                //        {
                                                //            temp = temp + 1;
                                                //        }
                                                //        att = ds_attndmaster.Tables[0].Rows.Count;
                                                //    }
                                                //}
                                            }
                                        }
                                    }
                                }
                                if (temp > 0)
                                {
                                    Buttonsave.Visible = false;
                                    Buttonupdate.Visible = true;
                                }
                            }
                            else
                            {
                                FpSpread2.Visible = false;
                                btnsliplist.Visible = false;//Added By Srinath 12/8/2013
                                lblset.Visible = true;
                                lblset.Text = "Please Update Attendance Parameters!!!";
                                return;
                            }
                            FpSpread2.SaveChanges();
                        }

                        if (Convert.ToInt32(FpSpread2.Sheets[0].RowCount) == 1)
                        {

                            pHeaderatendence.Visible = false;
                            pBodyatendence.Visible = false;
                            Panelpage.Visible = false;
                            Panel3.Visible = false;
                        }
                        else
                        {
                            FpSpread2.Visible = true;
                            FpSpread2.ActiveSheetView.SheetCornerSpanModel.Add(0, 0, 2, 1);
                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

                            FpSpread2.ActiveSheetView.SheetCorner.Cells[0, 0].Text = "S.No";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
                            FpSpread2.Sheets[0].Columns[0].Width = 50;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                            FarPoint.Web.Spread.TextCellType textcel_type1 = new FarPoint.Web.Spread.TextCellType();
                            FpSpread2.Sheets[0].Columns[1].CellType = textcel_type1;
                            FpSpread2.Sheets[0].Columns[2].CellType = textcel_type1;
                            FpSpread2.Sheets[0].Columns[3].CellType = textcel_type1;
                            FpSpread2.Visible = true;
                            pHeaderatendence.Visible = true;
                            pBodyatendence.Visible = true;
                            Panelpage.Visible = false;


                            //strcomo2 = new string[] {  " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" };//-----21/6/12 PRABHA
                            //---------------------------------load rights                      

                            i = 0;
                            //string strquery =string.Empty;
                            //if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                            //{
                            //    strquery = "select rights from  OD_Master_Setting where group_code=" + Session["group_code"].ToString() + "";
                            //   // cmd.CommandText = "select rights from  OD_Master_Setting where group_code=" + Session["group_code"].ToString() + "";
                            //}
                            //else
                            //{
                            //    strquery = "select rights from  OD_Master_Setting where usercode=" + Session["UserCode"].ToString() + "";
                            //   // cmd.CommandText = "select rights from  OD_Master_Setting where usercode=" + Session["UserCode"].ToString() + "";
                            //}

                            ////cmd.Connection = con;
                            ////con.Close();
                            ////con.Open();
                            ////dr_rights_od = cmd.ExecuteReader();
                            ////if (dr_rights_od.HasRows)
                            //if(strquery.Trim()!="")
                            //{
                            //    //while (dr_rights_od.Read())
                            //    //{

                            //        string od_rights =string.Empty;
                            //        Hashtable od_has = new Hashtable();
                            //        od_rights = dacces2.GetFunction(strquery);
                            //        //od_rights = dr_rights_od["rights"].ToString();

                            //        if (od_rights != null && od_rights.Trim()!="" && od_rights.Trim()!="0")
                            //        {
                            //            string[] split_od_rights = od_rights.Split(',');

                            //            strcomo2 = new string[split_od_rights.GetUpperBound(0) + 2];
                            //            strcomo2[i++] =string.Empty;
                            //            for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                            //            {

                            //                strcomo2[i++] = split_od_rights[od_temp].ToString();
                            //            }

                            //        }
                            //        else
                            //        {
                            //            strcomo2[0] =string.Empty;
                            //            strcomo2[1] = "P";
                            //            strcomo2[2] = "A";
                            //            strcomo2[3] = "OD";
                            //            strcomo2[4] = "SOD";
                            //            strcomo2[5] = "ML";
                            //            strcomo2[6] = "NSS";
                            //            strcomo2[7] = "L";
                            //            strcomo2[8] = "NCC";
                            //            strcomo2[9] = "HS";
                            //            strcomo2[10] = "PP";
                            //            strcomo2[11] = "SYOD";
                            //            strcomo2[12] = "COD";
                            //            strcomo2[13] = "OOD";
                            //            strcomo2[14] = "LA";

                            //        }
                            //    //}
                            //}
                            //else
                            //{
                            //    strcomo2[0] =string.Empty;
                            //    strcomo2[1] = "P";
                            //    strcomo2[2] = "A";
                            //    strcomo2[3] = "OD";
                            //    strcomo2[4] = "SOD";
                            //    strcomo2[5] = "ML";
                            //    strcomo2[6] = "NSS";
                            //    strcomo2[7] = "L";
                            //    strcomo2[8] = "NCC";
                            //    strcomo2[9] = "HS";
                            //    strcomo2[10] = "PP";
                            //    strcomo2[11] = "SYOD";
                            //    strcomo2[12] = "COD";
                            //    strcomo2[13] = "OOD";
                            //    strcomo2[14] = "LA";

                            //}

                            //---------------------------
                            objintcell2 = new FarPoint.Web.Spread.ComboBoxCellType(strcomo2);
                            objintcell2.ShowButton = true;
                            objintcell2.AutoPostBack = true;
                            objintcell2.UseValue = true;
                            FpSpread2.Sheets[0].Columns[0].CellType = objintcell2;
                            FpSpread2.SaveChanges();
                            FpSpread2.Sheets[0].Columns[0].BackColor = Color.MistyRose;


                            ///-------------present and absent count
                            FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 2;
                            FpSpread2.Sheets[0].SpanModel.Add((FpSpread2.Sheets[0].RowCount - 2), 0, 1, 5);
                            FpSpread2.Sheets[0].SpanModel.Add((FpSpread2.Sheets[0].RowCount - 1), 0, 1, 5);

                            FpSpread2.Sheets[0].Rows[(FpSpread2.Sheets[0].RowCount - 2)].CellType = textcel_type1;
                            FpSpread2.Sheets[0].Rows[(FpSpread2.Sheets[0].RowCount - 1)].CellType = textcel_type1;

                            FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 2), 0].Text = "No Of Student(s) Present:";
                            FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 1), 0].Text = "No Of Student(s) Absent:";
                            FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 2), 0].Locked = true;
                            FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 1), 0].Locked = true;

                            //Added By Srinath 13/8/2013
                            FpSpread2.Sheets[0].RowHeader.Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = " ";
                            FpSpread2.Sheets[0].RowHeader.Cells[FpSpread2.Sheets[0].RowCount - 2, 0].Text = " ";

                            //---------------get calcflag
                            present_calcflag.Clear();
                            absent_calcflag.Clear();
                            hat.Clear();
                            hat.Add("colege_code", Session["collegecode"].ToString());
                            ds_attndmaster = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
                            if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
                            {
                                count_master = (ds_attndmaster.Tables[0].Rows.Count);
                            }
                            if (count_master > 0)
                            {
                                for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                                {
                                    if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                                    {
                                        present_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                                    }
                                    if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                                    {
                                        absent_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                                    }
                                }
                            }

                            for (Att_mark_column = 5; Att_mark_column < FpSpread2.Sheets[0].ColumnCount; Att_mark_column++)
                            {
                                absent_count = 0;
                                present_count = 0;
                                for (Att_mark_row = 1; Att_mark_row < FpSpread2.Sheets[0].RowCount - 2; Att_mark_row++)
                                {
                                    if (FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text.ToString() != "")
                                    {
                                        if (present_calcflag.ContainsKey(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Tag.ToString()))
                                        {
                                            present_count++;
                                        }
                                        if (absent_calcflag.ContainsKey(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Tag.ToString()))
                                        {
                                            absent_count++;
                                        }
                                    }
                                }
                                FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 2), Att_mark_column].Text = present_count.ToString();
                                FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 1), Att_mark_column].Text = absent_count.ToString();
                            }

                            //----------------------------
                            Panel3.Visible = true;
                            Double totalRows = 0;
                            totalRows = Convert.ToInt32(FpSpread2.Sheets[0].RowCount);
                            Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread2.Sheets[0].PageSize);
                            //Buttontotal.Text = "Records: " + totalRows + "  Pages:1 ";
                            //  DropDownListpage.Items.Clear();
                            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                            FpSpread2.Height = 300;
                            FpSpread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                            FpSpread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                            //if (totalRows >= 10)
                            //{
                            //    FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                            //    //for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                            //    //{
                            //    //    DropDownListpage.Items.Add((k + 10).ToString());
                            //    //}
                            //    //  DropDownListpage.Items.Add("Others");
                            //    FpSpread2.Height = 300;
                            //    FpSpread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                            //    FpSpread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                            //}

                            //else if (totalRows == 0)
                            //{
                            //    // DropDownListpage.Items.Add("0");
                            //    // FpSpread2.Height = 200;
                            //}
                            //else
                            //{
                            //    FpSpread2.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                            //    // DropDownListpage.Items.Add(FpSpread2.Sheets[0].PageSize.ToString());
                            //    FpSpread2.Height = 60 + (25 * Convert.ToInt32(totalRows));
                            //}
                            //  DropDownListpage.SelectedIndex = DropDownListpage.Items.Count - 2;
                            FpSpread2.Sheets[0].AutoPostBack = false;
                            int widt = 0;
                            for (int col = 0; col < FpSpread2.Sheets[0].ColumnCount; col++)
                            {
                                widt = widt + FpSpread2.Sheets[0].Columns[col].Width;
                            }
                            widt = widt + FpSpread2.Sheets[0].RowHeader.Width + 15;
                            if (widt > 900)
                            {
                                //FpSpread2.Width = 950;
                                FpSpread2.Width = widt-80;
                            }
                            else
                            {
                                FpSpread2.Width = widt;
                            }
                        }

                        if (FpSpread2.Sheets[0].Rows.Count < 20)
                        {
                            int height = 0;
                            for (int hig = 0; hig < FpSpread2.Sheets[0].RowCount; hig++)
                            {
                                height = height + FpSpread2.Sheets[0].Rows[hig].Height;
                            }

                            FpSpread2.Height = height;
                        }
                        else
                        {
                            FpSpread2.Height = 600;
                        }
                    }
                    else
                    {
                        lbltodate.Visible = true;
                        lbltodate.Text = "Select From Date";                           
                    }
                }
                else
                {
                    lblfromdate.Visible = true;
                    lblfromdate.Text = "Select From Date";
                }
            }
            FpSpread2.Sheets[0].AutoPostBack = false;
            if (FpSpread2.Sheets[0].RowCount > 0)
            {
                Double heighva = FpSpread2.Sheets[0].Rows[0].Height;
                heighva = FpSpread2.Sheets[0].RowCount * heighva + 350;
                FpSpread2.Height = Convert.ToInt32(heighva);
                FpSpread2.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;                              
            lblset.Text = ex.ToString();
            FpSpread2.Visible = false;
        }
    }

    protected void Buttonsave_Click(object sender, EventArgs e)
    {
        try
        {
            string monthandyear = string.Empty;
            string updatevalues = string.Empty;
            string savehoursqlstrq;
            int totalhor;
            string noofhours_save = string.Empty;
            string no_firsthalf = string.Empty;
            string no_secondhalf = string.Empty;
            string no_minpresent_firsthalf = string.Empty;
            string no_minpresent_secondhalf = string.Empty;
            string min_per_day = string.Empty;
            savehoursqlstrq = "select No_of_hrs_per_day,no_of_hrs_I_half_day,no_of_hrs_II_half_day,min_pres_II_half_day ,min_pres_I_half_day,min_hrs_per_day  from PeriodAttndSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + "";
            ds.Clear();
            ds = dacces2.select_method_wo_parameter(savehoursqlstrq, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                noofhours_save = ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                no_firsthalf = ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
                no_secondhalf = ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString();
                no_minpresent_firsthalf = ds.Tables[0].Rows[0]["min_pres_I_half_day"].ToString();
                no_minpresent_secondhalf = ds.Tables[0].Rows[0]["min_pres_II_half_day"].ToString();
                min_per_day = ds.Tables[0].Rows[0]["min_hrs_per_day"].ToString();
            }

            totalhor = Convert.ToInt32(noofhours_save);
            string str_Date;

            string str_rollno;
            string str_hour;
            string Atyear;
            string Atmonth;
            long strdate;
            string str_day;
            string Att_mark;
            string Att_value;
            string dcolumn;
            string Splitmondate;

            str_Date = string.Empty;
            str_rollno = string.Empty;
            str_hour = string.Empty;
            str_day = string.Empty;
            Att_mark = string.Empty;
            Att_value = string.Empty;
            dcolumn = string.Empty;
            Splitmondate = string.Empty;


            string settingquery = string.Empty;
            string addSection = string.Empty;
            if (ddlsec.SelectedIndex > 0)
            {
                addSection = "  and r.sections='" + ddlsec.SelectedValue.Trim() + "' ";
            }
            settingquery = "select a.roll_no,a.month_year,d1d1,d1d2,d1d3,d1d4,d1d5,d1d6,d1d7,d1d8,d1d9,d1d10,d2d1,d2d2,d2d3,d2d4,d2d5,d2d6,d2d7,d2d8,d2d9,d2d10,d3d1,d3d2,d3d3,d3d4,d3d5,d3d6,d3d7,d3d8,d3d9,d3d10,d4d1,d4d2,d4d3,d4d4,d4d5,d4d6,d4d7,d4d8,d4d9,d4d10,d5d1,d5d2,d5d3,d5d4,d5d5,d5d6,d5d7,d5d8,d5d9,d5d10,d6d1,d6d2,d6d3,d6d4,d6d5,d6d6,d6d7,d6d8,d6d9,d6d10,d7d1,d7d2,d7d3,d7d4,d7d5,d7d6,d7d7,d7d8,d7d9,d7d10,d8d1,d8d2,d8d3,d8d4,d8d5,d8d6,d8d7,d8d8,d8d9,d8d10,d9d1,d9d2,d9d3,d9d4,d9d5,d9d6,d9d7,d9d8,d9d9,d9d10,d10d1,d10d2,d10d3,d10d4,d10d5,d10d6,d10d7,d10d8,d10d9,d10d10,d11d1,d11d2,d11d3,d11d4,d11d5,d11d6,d11d7,d11d8,d11d9,d11d10,d12d1,d12d2,d12d3,d12d4,d12d5,d12d6,d12d7,d12d8,d12d9,d12d10,d13d1,d13d2,d13d3,d13d4,d13d5,d13d6,d13d7,d13d8,d13d9,d13d10,d14d1,d14d2,d14d3,d14d4,d14d5,d14d6,d14d7,d14d8,d14d9,d14d10,d15d1,d15d2,d15d3,d15d4,d15d5,d15d6,d15d7,d15d8,d15d9,d15d10,d16d1,d16d2,d16d3,d16d4,d16d5,d16d6,d16d7,d16d8,d16d9,d16d10,d17d1,d17d2,d17d3,d17d4,d17d5,d17d6,d17d7,d17d8,d17d9,d17d10,d18d1,d18d2,d18d3,d18d4,d18d5,d18d6,d18d7,d18d8,d18d9,d18d10,d19d1,d19d2,d19d3,d19d4,d19d5,d19d6,d19d7,d19d8,d19d9,d19d10,d20d1,d20d2,d20d3,d20d4,d20d5,d20d6,d20d7,d20d8,d20d9,d20d10,d21d1,d21d2,d21d3,d21d4,d21d5,d21d6,d21d7,d21d8,d21d9,d21d10,d22d1,d22d2,d22d3,d22d4,d22d5,d22d6,d22d7,d22d8,d22d9,d22d10,d23d1,d23d2,d23d3,d23d4,d23d5,d23d6,d23d7,d23d8,d23d9,d23d10,d24d1,d24d2,d24d3,d24d4,d24d5,d24d6,d24d7,d24d8,d24d9,d24d10,d25d1,d25d2,d25d3,d25d4,d25d5,d25d6,d25d7,d25d8,d25d9,d25d10,d26d1,d26d2,d26d3,d26d4,d26d5,d26d6,d26d7,d26d8,d26d9,d26d10,d27d1,d27d2,d27d3,d27d4,d27d5,d27d6,d27d7,d27d8,d27d9,d27d10,d28d1,d28d2,d28d3,d28d4,d28d5,d28d6,d28d7,d28d8,d28d9,d28d10,d29d1,d29d2,d29d3,d29d4,d29d5,d29d6,d29d7,d29d8,d29d9,d29d10,d30d1,d30d2,d30d3,d30d4,d30d5,d30d6,d30d7,d30d8,d30d9,d30d10,d31d1,d31d2,d31d3,d31d4,d31d5,d31d6,d31d7,d31d8,d31d9,d31d10,a.Att_App_no,a.Att_CollegeCode from attendance a,Registration r where Att_App_no = r.App_No and r.degree_code  in ( '" + ddlbranch.SelectedValue.ToString() + "') " + addSection;

            ds.Clear();
            ds = dacces2.select_method_wo_parameter(settingquery, "Text");
            bool noentryflag = false;
            int notattmarkcount = 0;

            for (int Att_row = 1; Att_row <= FpSpread2.Sheets[0].RowCount - 3; Att_row++)
            {
                updatevalues = string.Empty;
                monthandyear = string.Empty;
                string existattndval = string.Empty;
                int colcount1 = 0;
                string getvalue = string.Empty;
                str_rollno = FpSpread2.Sheets[0].GetText(Att_row, 1).ToString();
                bool rollnoentry = false;
                for (int Att_column = 5; Att_column <= FpSpread2.Sheets[0].ColumnCount - 1; Att_column++)
                {
                    colcount1++;
                    str_Date = FpSpread2.Sheets[0].ColumnHeader.GetClipValue(0, Att_column, 1, 1);
                    string[] tmpdate = str_Date.ToString().Split(new char[] { ' ' });
                    str_Date = tmpdate[0].ToString();

                    Splitmondate = str_Date.ToString();
                    string[] split = Splitmondate.Split(new Char[] { '-' });
                    str_day = split[0].ToString();
                    Atmonth = split[1].ToString();
                    Atyear = split[2].ToString();
                    strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                    str_hour = FpSpread2.Sheets[0].ColumnHeader.GetClipValue(1, Att_column, 1, 1);
                    string[] split_hr = str_hour.Split(new Char[] { '-' });
                    str_hour = str_hour[0].ToString();
                    Att_mark = Convert.ToString(FpSpread2.GetEditValue(Att_row, Att_column));
                    if (Att_mark == "System.Object")
                    {
                        Att_mark = FpSpread2.Sheets[0].Cells[Att_row, Att_column].Text.ToString();
                        getvalue = Attvalues(FpSpread2.Sheets[0].Cells[Att_row, Att_column].Text.ToString());
                    }

                    dcolumn = "d" + str_day + "d" + str_hour;
                    Att_value = Attvalues(Att_mark);
                    getvalue = Att_value;
                    if (Att_value == "")
                    {
                        Att_value = "0";
                    }
                    if (Att_value != "0")
                    {
                        nullflag = true;
                    }

                    if (updatevalues == "")
                    {
                        updatevalues = dcolumn + "=" + Att_value;
                    }
                    else
                    {
                        updatevalues = updatevalues + ',' + dcolumn + "=" + Att_value;
                    }

                    ds.Tables[0].DefaultView.RowFilter = " roll_no='" + str_rollno + "' and month_year='" + strdate + "'";
                    DataView dvstuattmon = ds.Tables[0].DefaultView;
                    if (dvstuattmon.Count > 0)
                    {
                        string setval = dvstuattmon[0][dcolumn].ToString();
                        if (existattndval == "")
                        {
                            existattndval = dcolumn + "=" + setval;
                        }
                        else
                        {
                            existattndval = existattndval + ',' + dcolumn + "=" + setval;
                        }
                    }
                    else
                    {
                        string setval = "0";
                        if (existattndval == "")
                        {
                            existattndval = dcolumn + "=" + setval;
                        }
                        else
                        {
                            existattndval = existattndval + ',' + dcolumn + "=" + setval;
                        }
                    }

                    if (monthandyear == "")
                    {
                        monthandyear = strdate.ToString();
                    }
                    if (existattndval != updatevalues)
                    {
                        noentryflag = true;
                        rollnoentry = true;
                    }
                }
                if (rollnoentry == false)
                {
                    notattmarkcount = notattmarkcount + 1;
                }
            }

            if (noentryflag == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Please Update Attendance And Save')", true);
                return;
            }
            else
            {
                if (notattmarkcount > 0)
                {
                    lblMessage.Text = notattmarkcount + " Student(s) Attendance Are Not Update. Do You Want to Save  the Attendance ?";
                }
                else
                {
                    lblMessage.Text = "Do You Want to Save the Attendance ?";
                }
                mpemsgboxsave.Show();
            }
            Buttonexit.Visible = false;
        }
            
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    public void parentsmeet(string regno01, DateTime datectr, string reson)//added by sridhar 02 aug 2014
    {
        DataSet dstemp = new DataSet();
        string srisql = "if not exists (select * from parents_meet where roll_no='" + regno01 + "' and send_date='" + datectr + "')  begin  insert into parents_meet  values ('" + regno01 + "','" + datectr + "','" + reson + "','','','','') end";
        dstemp.Clear();
        dstemp = dacces2.select_method_wo_parameter(srisql, "Text");
    }

    protected void Buttonupdate_Click(object sender, EventArgs e)
    {
        Buttonsave_Click(sender, e);
    }

    protected void Buttonexit_Click(object sender, EventArgs e)
    {
        string intime = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
        int a = dacces2.update_method_wo_parameter("update UserEELog  set Out_Time='" + intime + "',LogOff='1' where entry_code='" + Session["Entry_Code"] + "'", "Text");
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected void Buttonselectall_Click(object sender, EventArgs e)
    {
        try
        {
            lblset.Visible = false;
            if (FpSpread2.Sheets[0].RowCount > 1)
            {
                for (int row = 1; row < FpSpread2.Sheets[0].RowCount - 2; row++)
                {
                    for (int col = 5; col < FpSpread2.Sheets[0].ColumnCount; col++)
                    {
                        if (FpSpread2.Sheets[0].Cells[row, col].Text != "S" && FpSpread2.Sheets[0].Cells[row, col].Text.ToUpper() != "OD")//condn added 09.08.12
                        {
                            if (FpSpread2.Sheets[0].Cells[row, col].Locked == false)
                            {
                                FpSpread2.Sheets[0].Cells[row, col].Text = "P";
                            }
                            FpSpread2.SaveChanges();
                        }
                    }
                }

                //---------------get calcflag
                present_calcflag.Clear();
                absent_calcflag.Clear();
                hat.Clear();
                hat.Add("colege_code", Session["collegecode"].ToString());
                ds_attndmaster = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
                count_master = (ds_attndmaster.Tables[0].Rows.Count);
                if (count_master > 0)
                {
                    for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                    {

                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                        {
                            present_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());

                        }
                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                        {
                            absent_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                        }
                    }
                }
                for (Att_mark_column = 5; Att_mark_column < FpSpread2.Sheets[0].ColumnCount; Att_mark_column++)
                {
                    absent_count = 0;
                    present_count = 0;

                    for (Att_mark_row = 1; Att_mark_row < FpSpread2.Sheets[0].RowCount - 2; Att_mark_row++)
                    {
                        if (FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text != "" && FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text != " " && FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text != null) //condn 09.08.12 mythili
                        {
                            string getvalue = Attvalues(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text.ToString());
                            if (present_calcflag.ContainsKey(getvalue))
                            {
                                present_count++;
                            }
                            if (absent_calcflag.ContainsKey(getvalue))
                            {
                                absent_count++;
                            }
                        }
                    }
                    FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 2), Att_mark_column].Text = present_count.ToString();
                    FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 1), Att_mark_column].Text = absent_count.ToString();
                }

                //----------------------------
                //Added by srinath 24/8/2013
                string entrycode = Session["Entry_Code"].ToString();
                string formname = "Student Attendance Entry";
                string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                string doa = DateTime.Now.ToString("MM/dd/yyy");
                string section = string.Empty;
                if (ddlsec.SelectedValue.ToString() != "" && ddlsec.SelectedValue.ToString() != "All" && ddlsec.SelectedValue.ToString() != null && ddlsec.SelectedValue.ToString() != "0")
                {
                    section = ":Sections -" + ddlsec.SelectedValue.ToString();
                }
                string details = "" + ddlbranch.SelectedValue.ToString() + ":Sem - " + ddlsem.SelectedValue.ToString() + ":Batch Year -" + ddlbatch.SelectedValue.ToString() + " " + section + "";
                string modules = "0";
                string act_diff = " ";
                string ctsname = "Changed the Attendance Information";
                string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','7','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
                int a = dacces2.update_method_wo_parameter(strlogdetails, "Text");
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    protected void Buttondeselect_Click(object sender, EventArgs e)
    {
        try
        {
            lblset.Visible = false;
            if (FpSpread2.Sheets[0].RowCount > 1)
            {
                for (int row = 1; row < FpSpread2.Sheets[0].RowCount - 2; row++)
                {
                    for (int col = 5; col < FpSpread2.Sheets[0].ColumnCount; col++)
                    {
                        if (FpSpread2.Sheets[0].Cells[row, col].Text != "S" && FpSpread2.Sheets[0].Cells[row, col].Text.ToUpper() != "OD")//condn added on 09.08.12 mythili
                        {
                            if (FpSpread2.Sheets[0].Cells[row, col].Locked == false)
                            {
                                FpSpread2.Sheets[0].Cells[row, col].Text = string.Empty;
                            }
                            FpSpread2.SaveChanges();
                        }
                    }
                }

                //---------------get calcflag
                hat.Clear();
                hat.Add("colege_code", Session["collegecode"].ToString());
                ds_attndmaster = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
                count_master = (ds_attndmaster.Tables[0].Rows.Count);
                if (count_master > 0)
                {
                    for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                    {
                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                        {
                            if (!present_calcflag.ContainsKey(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                            {
                                present_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                            }

                        }
                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                        {
                            if (!absent_calcflag.ContainsKey(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                            {
                                absent_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                            }
                        }
                    }
                }

                for (Att_mark_column = 5; Att_mark_column < FpSpread2.Sheets[0].ColumnCount; Att_mark_column++)
                {
                    absent_count = 0;
                    present_count = 0;

                    for (Att_mark_row = 1; Att_mark_row < FpSpread2.Sheets[0].RowCount - 2; Att_mark_row++)
                    {
                        if (FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text.Trim() != "" && FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text != null) //condn added on 09.08.12 mythili
                        {
                            string getvalue = Attvalues(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text.ToString());
                            if (present_calcflag.ContainsKey(getvalue))
                            {
                                present_count++;
                            }
                            if (absent_calcflag.ContainsKey(getvalue))
                            {
                                absent_count++;
                            }
                        }
                    }
                    FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 2), Att_mark_column].Text = present_count.ToString();
                    FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 1), Att_mark_column].Text = absent_count.ToString();
                }

                //----------------------------
                //Added by srinath 24/8/2013
                string entrycode = Session["Entry_Code"].ToString();
                string formname = "Student Attendance Entry";
                string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                string doa = DateTime.Now.ToString("MM/dd/yyy");
                string section = string.Empty;
                if (ddlsec.SelectedValue.ToString() != "" && ddlsec.SelectedValue.ToString() != "All" && ddlsec.SelectedValue.ToString() != null && ddlsec.SelectedValue.ToString() != "0")
                {
                    section = ":Sections -" + ddlsec.SelectedValue.ToString();
                }
                string details = "" + ddlbranch.SelectedValue.ToString() + ":Sem - " + ddlsem.SelectedValue.ToString() + ":Batch Year -" + ddlbatch.SelectedValue.ToString() + " " + section + "";
                string modules = "0";
                string act_diff = " ";
                string ctsname = "Change Attendance Information";
                string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','8','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
                int a = dacces2.update_method_wo_parameter(strlogdetails, "Text");
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBoxother.Text = string.Empty;
        lblother.Visible = false;
        LabelE.Visible = false;

        if (DropDownListpage.Text == "Others")
        {
            TextBoxother.Visible = true;
            TextBoxother.Focus();
        }
        else
        {
            TextBoxother.Visible = false;
            FpSpread2.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            CalculateTotalPages();
        }
    }

    void CalculateTotalPages()
    {
        Double totalRows = 0;
        totalRows = Convert.ToInt32(FpSpread2.Sheets[0].RowCount);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread2.Sheets[0].PageSize);
        Buttontotal.Text = "Records: " + totalRows + "  Pages: " + Session["totalPages"];
        Buttontotal.Visible = true;
    }

    protected void TextBoxpage_TextChanged(object sender, EventArgs e)
    {
        lblother.Visible = false;
        try
        {
            if (FpSpread2.Sheets[0].RowCount > 0)
            {
                if (TextBoxpage.Text.Trim() != "")
                {
                    if (Convert.ToInt32(TextBoxpage.Text) > Convert.ToInt16(Session["totalPages"]))
                    {
                        LabelE.Visible = true;
                        LabelE.Text = "Exceed The Page Limit";
                        TextBoxpage.Text = string.Empty;
                        FpSpread2.Visible = true;
                    }
                    else if ((Convert.ToInt32(TextBoxpage.Text) == 0))
                    {
                        LabelE.Text = "Should be Greater than Zero";
                        LabelE.Visible = true;
                        TextBoxpage.Text = string.Empty;
                        FpSpread2.Visible = true;
                    }
                    else
                    {
                        LabelE.Visible = false;
                        FpSpread2.CurrentPage = Convert.ToInt32(TextBoxpage.Text) - 1;
                        FpSpread2.Visible = true;
                    }
                }
            }
        }
        catch
        {
            LabelE.Text = "Exceed The Page Limit";
            TextBoxpage.Text = string.Empty;
            LabelE.Visible = true;
        }
    }

    protected void TextBoxother_TextChanged(object sender, EventArgs e)
    {
        LabelE.Visible = false;
        try
        {
            if (FpSpread2.Sheets[0].RowCount > 0)
            {
                if (TextBoxother.Text != "")
                {
                    FpSpread2.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
                    CalculateTotalPages();
                    lblother.Visible = false;
                }
            }
        }
        catch
        {
            lblother.Text = "Enter the Valid Page";
            TextBoxother.Text = string.Empty;
            lblother.Visible = true;
        }

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
        else if (Attstr_mark == "17")
        {
            Att_mark = "LA";
        }
        else
        {
            //aruna 20mar2014 Att_mark = "NE";
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
        else if (Att_str1 == "LA")
        {
            Attvalue = "17";
        }
        else
        {
            Attvalue = string.Empty;
        }
        return Attvalue;
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

    protected void FpSpread2_SelectedIndexChanged(Object sender, EventArgs e)
    {
        FpSpread2.SaveChanges();
    }

    protected void FpSpread2_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string controlatt = string.Empty;
            bool dateflag = false, chk_flag = false, savefalg = false;
            string buttonok = String.Empty;
            string spread = string.Empty;
            Control control = null;
            string ctrlname = Page.Request.Params["__EVENTTARGET"];
            if (ctrlname != null && ctrlname != String.Empty)
            {
                control = Page.FindControl(ctrlname);
                spread = ctrlname.ToString();
            }
            else
            {
                string ctrlStr = String.Empty;
                Control c = null;
                foreach (string ctl in Page.Request.Form)
                {
                    if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                    {
                        ctrlStr = ctl.Substring(0, ctl.Length - 2);
                        c = Page.FindControl(ctrlStr);
                    }
                    else
                    {
                        c = Page.FindControl(ctl);
                        buttonok = ctl;
                    }
                    if (c is System.Web.UI.WebControls.Button ||
                             c is System.Web.UI.WebControls.ImageButton)
                    {
                        control = c;
                        break;
                    }
                }
            }
            string spreadname = string.Empty;
            if (spread != "")
            {
                string[] spiltspreadname = spread.Split('$');
                spreadname = spiltspreadname[2].ToString().Trim();
                controlatt = spreadname;
            }
            if (spreadname.ToString().Trim().ToLower() == "fpspread2")
            {
                string actrow = FpSpread2.Sheets[0].ActiveRow.ToString();  //e.SheetView.ActiveRow.ToString();
                string actcol = FpSpread2.Sheets[0].ActiveColumn.ToString();  //e.SheetView.ActiveColumn.ToString();
                string last = e.CommandArgument.ToString();
                if (actrow == "0")
                {
                    if (last == "0")
                    {
                        flag_true = false;
                    }
                    else
                    {
                        flag_true = true;
                    }
                }
                if (actcol == "0")
                {
                    if (actrow == last)
                    {
                        flag_true = false;
                    }
                    else
                    {
                        flag_true = true;
                    }
                }
                if (flag_true == false && actrow == "0")
                {
                    string seltext = string.Empty;
                    for (int j = 1; j < Convert.ToInt16(FpSpread2.Sheets[0].RowCount); j++)
                    {
                        actcol = e.SheetView.ActiveColumn.ToString();
                        string value = e.EditValues[0].ToString();
                        e.Handled = true;
                        seltext = e.EditValues[Convert.ToInt32(actcol)].ToString();

                        if (seltext != "System.Object" && seltext.Trim() != "Select for All")
                        {
                            if ((FpSpread2.Sheets[0].GetText(j, Convert.ToInt16(actcol)) != "OD") && (FpSpread2.Sheets[0].GetText(j, Convert.ToInt16(actcol)) != "S"))
                            {
                                if (FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Locked == false)
                                {
                                    FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
                                }
                            }
                        }
                        else
                        {
                            if ((FpSpread2.Sheets[0].GetText(j, Convert.ToInt16(actcol)) != "OD") && (FpSpread2.Sheets[0].GetText(j, Convert.ToInt16(actcol)) != "S"))
                            {
                                if (FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Locked == false)
                                {
                                    FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = string.Empty;
                                }
                            }
                        }
                        //string rollNo = FpSpread2.Sheets[0].Cells[j, 2].Text;
                        if((FpSpread2.Sheets[0].Cells[j,4].BackColor==Color.Red))
                            FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = string.Empty;
                    }
                    flag_true = true;
                }

                if (flag_true == false && actcol == "0")
                {
                    int colcnt;
                    int i;
                    string strvalue;
                    int r = (int)e.CommandArgument;
                    colcnt = e.EditValues.Count - 1;

                    //for (i = 0; i <= colcnt; i++)
                    //{
                    //    if (i >= 5)
                    //    {
                    //        if (!object.ReferenceEquals(e.EditValues[i], FarPoint.Web.Spread.FpSpread.Unchanged))
                    //        {
                    //            strvalue = e.EditValues[0].ToString();
                    //            if (strvalue != "System.Object")
                    //            {
                    //                FpSpread2.Sheets[0].Cells[r, i].Value = strvalue;

                    //            }
                    //        }
                    //    }
                    //} 
                    actcol = FpSpread2.Sheets[0].ActiveColumn.ToString(); //e.SheetView.ActiveColumn.ToString();
                    string value = e.EditValues[0].ToString();
                    e.Handled = true;
                    string seltext = e.EditValues[0].ToString();
                    for (int j = 5; j <= Convert.ToInt16(FpSpread2.Sheets[0].ColumnCount - 1); j++)
                    {
                        //e.EditValues[Convert.ToInt16(actcol)].ToString();
                        if (seltext != "Select for All")
                        {
                            if (seltext != "System.Object" && seltext.Trim() != "Select for All")
                            {
                                if ((FpSpread2.Sheets[0].Cells[Convert.ToInt16(actrow), j].Text != "OD") && (FpSpread2.Sheets[0].Cells[Convert.ToInt16(actrow), j].Text != "S") && (FpSpread2.Sheets[0].Cells[Convert.ToInt16(actrow), 1].BackColor != Color.Red))
                                {
                                    if (FpSpread2.Sheets[0].Cells[Convert.ToInt16(actrow), j].Locked == false)
                                    {
                                        FpSpread2.Sheets[0].Cells[Convert.ToInt16(actrow), j].Text = seltext.ToString();
                                        // FpSpread2.Sheets[0].Cells[Convert.ToInt16(r), j].Text = seltext.ToString();
                                    }
                                }
                                if ((FpSpread2.Sheets[0].Cells[Convert.ToInt16(actrow), 4].BackColor == Color.Red))
                                    FpSpread2.Sheets[0].Cells[Convert.ToInt16(actrow), j].Text = string.Empty;
                            }
                            //else
                            //{
                            //    for (i = 0; i <= colcnt; i++)
                            //    {
                            //        if (i >= 5)
                            //        {
                            //            if (!object.ReferenceEquals(e.EditValues[i], FarPoint.Web.Spread.FpSpread.Unchanged))
                            //            {
                            //                strvalue = e.EditValues[i].ToString();
                            //                if (strvalue != "System.Object")
                            //                {
                            //                    FpSpread2.Sheets[0].Cells[r, i].Value = strvalue;

                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                        }
                    }




                    flag_true = true;
                }

                //Calculate no.of present==============================================================
                present_calcflag.Clear();
                absent_calcflag.Clear();
                hat.Clear();
                hat.Add("colege_code", Session["collegecode"].ToString());
                ds_attndmaster = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
                count_master = (ds_attndmaster.Tables[0].Rows.Count);
                if (count_master > 0)
                {
                    for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                    {

                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                        {
                            present_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());

                        }
                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                        {
                            absent_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                        }
                    }
                }

                for (Att_mark_column = 5; Att_mark_column < FpSpread2.Sheets[0].ColumnCount; Att_mark_column++)
                {
                    absent_count = 0;
                    present_count = 0;

                    for (Att_mark_row = 1; Att_mark_row < FpSpread2.Sheets[0].RowCount - 2; Att_mark_row++)
                    {
                        if (FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text.Trim() != "" && FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text != null) //condn 09.08.12 m ythili
                        {
                            string getvalue = Attvalues(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text.ToString());
                            if (present_calcflag.ContainsKey(getvalue))
                            {
                                present_count++;
                            }
                            if (absent_calcflag.ContainsKey(getvalue))
                            {
                                absent_count++;
                            }
                        }
                    }
                    FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 2), Att_mark_column].Text = present_count.ToString();
                    FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 1), Att_mark_column].Text = absent_count.ToString();
                }
            }
        }
        catch
        {
        }
        //=============================================================================

    }

    public void BindBatch()
    {
        try
        {
            string Master1 = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {

                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
                else
                {
                    Master1 = Session["group_code"].ToString();
                }
            }
            else
            {
                Master1 = Session["usercode"].ToString();
            }
            string collegecode = Session["collegecode"].ToString();
            string strbinddegree = "select distinct batch_year from tbl_attendance_rights where user_id='" + Master1 + "'  order by batch_year desc";

            DataSet ds = dacces2.select_method_wo_parameter(strbinddegree, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
                ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
            }
        }
        catch
        {
        }
    }

    public void BindDegree()
    {
        string collegecode = Session["collegecode"].ToString();
        DataSet ds = ClsAttendanceAccess.GetDegreeDetail(collegecode.ToString());
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddldegree.DataSource = ds;
            ddldegree.DataValueField = "Course_Id";
            ddldegree.DataTextField = "Course_Name";
            ddldegree.DataBind();
            ddldegree.Items.Insert(0, new ListItem("--Select--", "-1"));
        }
    }

    public int Get_Semester()
    {
        string batch_calcode_degree;
        string batch = ddlbatch.SelectedValue.ToString();
        string collegecode = Session["collegecode"].ToString();
        string degree = ddlbranch.SelectedValue.ToString();
        batch_calcode_degree = batch.ToString() + "/" + collegecode.ToString() + "/" + degree.ToString();
        DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            return Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
        }
        else
        {
            return 0;
        }
    }

    public void SemfunDetails()
    {
        if (ddlbranch.SelectedIndex != 0)
        {
            int durationval;
            durationval = Convert.ToInt32(Get_Semester());

            for (int durcnt = 1; durcnt <= durationval; durcnt++)
            {
                ddlsem.Items.Add(durcnt.ToString());
            }
        }
        ddlsem.Items.Insert(0, new ListItem("--Select--", "-1"));
    }

    public void BindSectionDetail()
    {
        //string branch = ddlbranch.SelectedValue.ToString();
        //string batch = ddlbatch.SelectedValue.ToString();

        //DataSet ds = ClsAttendanceAccess.GetsectionDetail(batch.ToString(), branch.ToString());
        //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //{
        //    ddlsec.DataSource = ds;
        //    ddlsec.DataTextField = "sections";
        //    ddlsec.DataValueField = "sections";
        //    ddlsec.DataBind();

        //    if (ddlsec.Items.Count > 0)
        //    {
        //        if (ddlsec.Items[0].Text != "")
        //        {
        //            ddlsec.Items.Insert(0, new ListItem("--Select--", "-1"));
        //            ddlsec.Enabled = true;
        //        }
        //        else
        //            ddlsec.Enabled = false;
        //    }
        //}
        string branch = ddlbranch.SelectedValue.ToString();
        string batch = ddlbatch.SelectedValue.ToString();
        //Modified by srinath 13/9/2013

        con.Close();
        con.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        //string secquery = "select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'";
        //ds = dacces2.select_method(secquery, hat, "Text");
        da.Fill(ds);
        ddlsec.DataSource = ds;
        ddlsec.DataTextField = "sections";
        ddlsec.DataValueField = "sections";
        ddlsec.DataBind();
        ddlsec.Items.Insert(0, "All");

        SqlDataReader dr_sec;
        dr_sec = cmd.ExecuteReader();
        dr_sec.Read();
        if (dr_sec.HasRows == true)
        {
            if (dr_sec["sections"].ToString().Trim() == "")
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
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        // lblnorec.Visible = false;
        load_spread();
        string course_id = ddldegree.SelectedValue.ToString();
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["UserCode"].ToString();
        //if (ddldegree.SelectedIndex > 0)
        //{

        //    DataSet ds = ClsAttendanceAccess.GetBranchDetail(course_id.ToString(), collegecode.ToString());
        //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        ddlbranch.DataSource = ds;
        //        ddlbranch.DataTextField = "Dept_Name";
        //        ddlbranch.DataValueField = "degree_code";
        //        ddlbranch.DataBind();
        //       // ddlbranch.Items.Insert(0, new ListItem("--Select--", "-1"));
        //    }
        //}
        con.Open();
        //cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + ddldegree.SelectedValue.ToString() + " and degree.college_code= " + Session["collegecode"] + " ", con);
        //SqlDataAdapter daBRANCH = new SqlDataAdapter(cmd);
        //DataSet dsbranch = new DataSet();
        //daBRANCH.Fill(dsbranch);
        //string course_id = ddlDegree.SelectedValue.ToString();
        groupcode = Session["group_code"].ToString();
        DataSet dsbranch = Bind_Dept(course_id, collegecode, usercode, groupcode);
        ddlbranch.DataSource = dsbranch;
        ddlbranch.DataValueField = "degree_code";
        ddlbranch.DataTextField = "dept_name";
        ddlbranch.DataBind();
        con.Close();
        //bind semester
        bindsem();
        //bind section
        BindSectionDetail();
        btnsliplist.Visible = false;//Added By Srinath 12/8/2013
    }

    public void bindsem()
    {
        ddlsem.Items.Clear();
        bool first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Close();
        con.Open();
        SqlDataReader dr;
        cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.Text.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
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
                    ddlsem.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlsem.Items.Add(i.ToString());
                }
            }
        }
        else
        {
            dr.Close();
            SqlDataReader dr1;
            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
            ddlsem.Items.Clear();
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
                        ddlsem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                }
            }
            dr1.Close();
        }
        if (ddlsem.Items.Count > 0)
        {
            ddlsem.SelectedIndex = 0;
            BindSectionDetail();
        }
        con.Close();
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        //lblnorec.Visible = false;
        btnsliplist.Visible = false;//Added By Srinath 12/8/2013
        load_spread();
        if (!Page.IsPostBack == false)
        {
            ddlsec.Items.Clear();
        }
        Btngo.Visible = true;
        btnok.Visible = false;
        BindSectionDetail();
        string collegecode = (string)Session["collegecode"];

        // if (option.SelectedValue == "1")
        {
            mysql.Open();
            SqlCommand cmd;
            SqlDataReader rs;
            cmd = new SqlCommand("select linkvalue from inssettings where college_code= '" + collegecode + "' and linkname='Manual Attendance'", mysql);
            rs = cmd.ExecuteReader();
            if (rs.HasRows == true)
            {
                while (rs.Read())
                {
                    if (rs["linkvalue"].ToString() == "1")
                    {
                        cksubjectwise.Enabled = true;
                        cksubjectwise.Visible = true;
                        ckmanual.Visible = true;
                        ckmanual.Enabled = false;
                        Btngo.Visible = false;
                        btnok.Visible = true;
                        btnok.Enabled = false;
                        Panelhour.Visible = false;
                    }
                    else
                    {
                        cksubjectwise.Enabled = false;
                        cksubjectwise.Visible = false;
                        ckmanual.Visible = false;
                        ckmanual.Enabled = false;
                        Panelhour.Visible = false;
                    }
                }
            }
            else
            {
                cksubjectwise.Enabled = false;
            }
            mysql.Close();
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnsliplist.Visible = false;
        load_spread();
        bindsem();
        if (!Page.IsPostBack == false)
        {
            ddlsem.Items.Clear();
        }
        try
        {
            //if ((ddlBranch.SelectedIndex != 0) && (ddlBranch.SelectedIndex > 0))
            //{
            bindsem();
            //}
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
        btnsliplist.Visible = false;//Added By Srinath 12/8/2013
    }

    protected void ddlmark_SelectedIndexChanged(object sender, EventArgs e)
    {
        markdiff.Visible = false;
        lblmarkabs.Visible = false;
        string mark = string.Empty;
        string markothers = string.Empty;
        mark = ddlmark.SelectedItem.ToString();
        markothers = ddlmarkothers.SelectedItem.ToString();
        if (mark == markothers && ddlmark.SelectedIndex != 0 && ddlmarkothers.SelectedIndex != 0)
        {
            Label10.Visible = true;
            markdiff.Visible = false;
            //ddlmarkothers.Enabled = false;
        }
        else
        {
            Label10.Visible = false;
        }
        ddlmarkothers.Focus();
    }

    public void load_spread()
    {
        lblset.Visible = false;
        ddlsubject.Visible = false;
        //ckhr2.Visible = false;
        ckmanual.Enabled = false;
        cksubjectwise.Visible = false;
        pHeaderatendence.Visible = false;
        pBodyatendence.Visible = false;
        Panelpage.Visible = false;
        Panel3.Visible = false;
        Panelhour.Visible = false;
        ckmanual.Checked = false;
        cksubjectwise.Checked = false;
        //     Panelhour.Visible = false;
        Panelind.Visible = false;
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnsliplist.Visible = false;//Added By Srinath 12/8/2013
        //load_spread();
        //con.Open();
        //string collegecode = Session["collegecode"].ToString();
        //string usercode = Session["usercode"].ToString();
        //groupcode = Session["group_code"].ToString();
        //DataSet ds = Bind_Degree(collegecode.ToString(), usercode, groupcode);
        //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //{
        //    ddldegree.DataSource = ds;
        //    ddldegree.DataValueField = "course_id";
        //    ddldegree.DataTextField = "course_name";
        //    ddldegree.DataBind();           
        //    con.Close();
        //}
        //con.Open();
        //string course_id = ddldegree.SelectedValue.ToString();
        //groupcode = Session["group_code"].ToString();
        //DataSet dsbranch = Bind_Dept(course_id, collegecode, usercode, groupcode);
        //ddlbranch.DataSource = dsbranch;
        //ddlbranch.DataValueField = "degree_code";
        //ddlbranch.DataTextField = "dept_name";
        //ddlbranch.DataBind();
        //con.Close();
        //bindsem();
        //BindSectionDetail();
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnsliplist.Visible = false;//Added By Srinath 12/8/2013
        // lblnorec.Visible = false;
        //int collegecode = 13;
        ////string collegecode = (string)Session["collegecode"];
        ////Label6.Visible = false;
        ////if (option.SelectedValue == "1")
        ////{
        ////    mysql.Open();
        ////    SqlCommand cmd;
        ////    SqlDataReader rs;
        ////    cmd = new SqlCommand("select linkvalue from inssettings where college_code= '" + collegecode + "' and linkname='Manual Attendance'", mysql);
        ////    rs = cmd.ExecuteReader();
        ////    if (rs.HasRows == true)
        ////    {
        ////        while (rs.Read())
        ////        {
        ////            if (rs["linkvalue"].ToString() == "1")
        ////            {
        ////                cksubjectwise.Enabled = true;
        ////                cksubjectwise.Visible = true;
        ////                ckmanual.Visible = true;
        ////            }
        ////            else
        ////            {
        ////                cksubjectwise.Enabled = false;
        ////                cksubjectwise.Visible = false;
        ////                ckmanual.Visible = false;
        ////            }
        ////        }
        ////    }
        ////    else
        ////    {
        ////        cksubjectwise.Enabled = false;
        ////    }
        ////    mysql.Close();
        ////}
    }

    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnsliplist.Visible = false;//Added By Srinath 12/8/2013
        if (cksubjectwise.Checked == false)
        {
            datelbl.Visible = false;
            lblset.Visible = true;
            lblset.Text = "Select subjectwise and then proceed";
            ddlsubject.Visible = false;
            ckmanual.Enabled = false;
            btnok.Enabled = false;
            Panelhour.Visible = false;
            FpSpread2.Visible = false;
            pHeaderatendence.Visible = false; ;
            pBodyatendence.Visible = false;
            return;
        }
        if (ddlsubject.SelectedIndex == 0)
        {
            lblset.Visible = true;
            lblset.Text = "Select the subject and then proceed";

            ckmanual.Enabled = false;
            btnok.Enabled = false;
            Panelhour.Visible = false;
            FpSpread2.Visible = false;
            pHeaderatendence.Visible = false; ;
            pBodyatendence.Visible = false;
            return;
        }
        if (txtFromDate.Text == "")
        {
            datelbl.Text = "Select From Date";
            datelbl.Visible = true;
            return;
        }
        if (TxtToDate.Text == "")
        {
            datelbl.Text = "Select To Date";
            datelbl.Visible = true;
            return;
        }
        // Panelhour.Visible = true;
        FpSpread2.ActiveSheetView.AutoPostBack = false;
        //ckmanual.Visible = true;
        ckmanual.Enabled = true;

        // FpSpread2.Height = 320;
        // FpSpread2.Width = 700;
        Btngo.Visible = false;
        btnok.Visible = true;
        FpSpread2.Sheets[0].ColumnCount = 0;
        FpSpread2.Sheets[0].RowCount = 0;
        Panelpage.Visible = false;
        Panel3.Visible = false;
        lblset.Visible = false;
        Panelhour.Visible = true;

        FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 1;
        FarPoint.Web.Spread.ComboBoxCellType objintcell6 = new FarPoint.Web.Spread.ComboBoxCellType();
        //  string[] strcomo1 = new string[] { "Select for All ", " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" };
        //string[]  strcomo = new string[] { " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" };
        //---------------------------------load rights                      
        string[] strcomo1 = new string[20];
        string[] strcomo = new string[20];
        int i = 0, j = 0;
        SqlCommand cmd = new SqlCommand();
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            cmd.CommandText = "select rights from  OD_Master_Setting where group_code=" + Session["group_code"].ToString() + "";
        }
        else
        {
            cmd.CommandText = "select rights from  OD_Master_Setting where usercode=" + Session["UserCode"].ToString() + "";
        }

        cmd.Connection = con;
        con.Close();
        con.Open();
        SqlDataReader dr_rights_od = cmd.ExecuteReader();
        if (dr_rights_od.HasRows)
        {
            while (dr_rights_od.Read())
            {
                string od_rights = string.Empty;
                Hashtable od_has = new Hashtable();
                od_rights = dr_rights_od["rights"].ToString();
                if (od_rights != string.Empty)
                {
                    string[] split_od_rights = od_rights.Split(',');
                    strcomo = new string[split_od_rights.GetUpperBound(0) + 1];
                    strcomo1 = new string[split_od_rights.GetUpperBound(0) + 2];
                    strcomo1[j++] = "Select for All ";
                    for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                    {
                        strcomo[i++] = split_od_rights[od_temp].ToString();
                        strcomo1[j++] = split_od_rights[od_temp].ToString();
                    }
                }
                else
                {
                    strcomo[0] = string.Empty;
                    strcomo[1] = "P";
                    strcomo[2] = "A";
                    strcomo[3] = "OD";
                    strcomo[4] = "SOD";
                    strcomo[5] = "ML";
                    strcomo[6] = "NSS";
                    strcomo[7] = "L";
                    strcomo[8] = "NCC";
                    strcomo[9] = "HS";
                    strcomo[10] = "PP";
                    strcomo[11] = "SYOD";
                    strcomo[12] = "COD";
                    strcomo[13] = "OOD";
                    strcomo[14] = "LA";

                    strcomo1[0] = "Select for All";
                    strcomo1[1] = string.Empty;
                    strcomo1[2] = "P";
                    strcomo1[3] = "A";
                    strcomo1[4] = "OD";
                    strcomo1[5] = "SOD";
                    strcomo1[6] = "ML";
                    strcomo1[7] = "NSS";
                    strcomo1[8] = "L";
                    strcomo1[9] = "NCC";
                    strcomo1[10] = "HS";
                    strcomo1[11] = "PP";
                    strcomo1[12] = "SYOD";
                    strcomo1[13] = "COD";
                    strcomo1[14] = "OOD";
                    strcomo1[15] = "LA";
                }
            }
        }
        else
        {
            strcomo[0] = string.Empty;
            strcomo[1] = "P";
            strcomo[2] = "A";
            strcomo[3] = "OD";
            strcomo[4] = "SOD";
            strcomo[5] = "ML";
            strcomo[6] = "NSS";
            strcomo[7] = "L";
            strcomo[8] = "NCC";
            strcomo[9] = "HS";
            strcomo[10] = "PP";
            strcomo[11] = "SYOD";
            strcomo[12] = "COD";
            strcomo[13] = "OOD";
            strcomo[14] = "LA";

            strcomo1[0] = "Select for All";
            strcomo1[1] = string.Empty;
            strcomo1[2] = "P";
            strcomo1[3] = "A";
            strcomo1[4] = "OD";
            strcomo1[5] = "SOD";
            strcomo1[6] = "ML";
            strcomo1[7] = "NSS";
            strcomo1[8] = "L";
            strcomo1[9] = "NCC";
            strcomo1[10] = "HS";
            strcomo1[11] = "PP";
            strcomo1[12] = "SYOD";
            strcomo1[13] = "COD";
            strcomo1[14] = "OOD";
            strcomo1[15] = "LA";
        }

        //---------------------------
        objintcell6 = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
        objintcell6.ShowButton = true;
        objintcell6.AutoPostBack = true;
        objintcell6.UseValue = true;
        // FpSpread2.ActiveSheetView.Cells[0, 5].CellType = objintcell;
        FpSpread2.SaveChanges();

        FarPoint.Web.Spread.ComboBoxCellType objcom = new FarPoint.Web.Spread.ComboBoxCellType();
        objcom = new FarPoint.Web.Spread.ComboBoxCellType(strcomo);

        objcom.AutoPostBack = true;
        //  objcom.UseValue = true;

        string sub_no = string.Empty;
        sub_no = ddlsubject.SelectedValue;

        //string deg_code = GetFunction("select distinct degree_code from degree where course_id=" + ddldegree.SelectedItem.Value.ToString() + " and dept_code=" + ddlbranch.SelectedItem.Value.ToString() + "");

        string sqlstr = GetFunction("select No_of_hrs_per_day  from PeriodAttndSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedItem.Value.ToString() + " " + strorder + "");
        int noofhours = 0;
        if (sqlstr != "")
            noofhours = Convert.ToInt32(sqlstr);
        ckhr2.Items.Clear();
        for (int p = 1; p <= noofhours; p++)
            ckhr2.Items.Add(p.ToString());

        string strsec = string.Empty;
        if (ddlsec.Text.ToString() == "All" || ddlsec.Text.ToString() == "")
        {
            strsec = string.Empty;
        }
        else
        {
            strsec = " and registration.sections='" + ddlsec.SelectedValue.ToString() + "'";
        }
        FpSpread2.Sheets[0].ColumnCount = 5;
        FpSpread2.Sheets[0].Columns.Default.Width = 0;
        FpSpread2.Sheets[0].Rows.Default.Height = 25;
        FpSpread2.Sheets[0].ColumnHeader.Columns[0].Visible = false;
        FpSpread2.Sheets[0].ColumnHeader.Columns[1].Visible = false;
        FpSpread2.Sheets[0].ColumnHeader.Columns[3].Visible = false;
        if (Session["Rollflag"].ToString() != "0")
        {
            FpSpread2.Sheets[0].ColumnHeader.Columns[0].Visible = true;
            FpSpread2.Sheets[0].Columns[0].Width = 100;
        }
        if (Session["Regflag"].ToString() != "0")
        {
            FpSpread2.Sheets[0].ColumnHeader.Columns[1].Visible = true;
            FpSpread2.Sheets[0].Columns[1].Width = 100;
        }
        if (Session["Studflag"].ToString() != "0")
        {
            FpSpread2.Sheets[0].ColumnHeader.Columns[3].Visible = true;
            FpSpread2.Sheets[0].Columns[3].Width = 100;
        }
        FpSpread2.Sheets[0].Columns[2].Width = 230;
        //Added By Srinath 14/3/2013 ========Start
        string orderby_Setting = GetFunction("select value from master_Settings where settings='order_by'");

        if (orderby_Setting == "")
        {
            strorder = string.Empty;
        }
        else
        {
            if (orderby_Setting == "0")
            {
                strorder = "ORDER BY registration.Roll_No";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "ORDER BY Registration.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strorder = "ORDER BY Registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "ORDER BY Registration.Roll_No,Registration.Reg_No,Registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "ORDER BY Registration.Roll_No,Registration.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "ORDER BY Registration.Reg.No,Registration.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "ORDER BY Registration.Roll_No,Registration.Stud_Name";
            }
        }

        string ucode = string.Empty;
        string code = string.Empty;
        string group_code = Session["group_code"].ToString();
        if (group_code.Contains(';'))
        {
            string[] group_semi = group_code.Split(';');
            group_code = group_semi[0].ToString();
        }
        if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
        {
            ucode = group_code;
            code = "group_code=" + ucode + "";
            grouporusercode1 = "  and group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            ucode = Session["usercode"].ToString();
            code = "usercode=" + ucode + "";
            grouporusercode1 = "  and usercode=" + Session["usercode"].ToString().Trim() + "";
        }

        string Discon = dacces2.GetFunction("select value from Master_Settings where settings='Attendance Discount'  " + grouporusercode1 + "");
        string debar = dacces2.GetFunction("select value from Master_Settings where settings='Attendance Debar'  " + grouporusercode1 + "");
        string dis=string.Empty;
        string deba=string.Empty;
        if (Discon == "1" || Discon.Trim().ToLower() == "true")
            dis = string.Empty;
        else
            dis = "  and delflag=0";

        if (debar == "1" || debar.Trim().ToLower() == "true")
            deba = string.Empty;
        else
            deba = "  and exam_flag <> 'DEBAR'";

        string str = "Select distinct registration.roll_no,registration.reg_no,registration.stud_name,registration.stud_type from registration,SubjectChooser,applyn a where a.app_no=registration.app_no and  registration.roll_no = subjectchooser.roll_no and registration.Degree_Code =" + ddlbranch.SelectedValue + " and Semester =" + ddlsem.SelectedValue.ToString() + " and registration.Batch_Year = " + ddlbatch.SelectedValue.ToString() + " and Subject_No = '" + sub_no + "' " + strsec + Session["strvar"].ToString() + " and RollNo_Flag<>0 and cc=0 " + dis + " " + deba + " " + strorder + "";
        cmd = new SqlCommand(str, mysql);
        mysql.Open();
        SqlDataReader da = cmd.ExecuteReader();
        if (da.HasRows)
            while (da.Read())
            {
                FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 1;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = da.GetValue(0).ToString();
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = da.GetValue(1).ToString();
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = da.GetValue(2).ToString();

                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = da.GetValue(3).ToString();
                if (da["stud_type"].ToString() == "Hostler")
                {
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].BackColor = Color.LightYellow;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].BackColor = Color.LightYellow;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].BackColor = Color.LightYellow;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].BackColor = Color.LightYellow;
                }
                else
                {
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].BackColor = Color.MediumSeaGreen;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].BackColor = Color.MediumSeaGreen;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].BackColor = Color.MediumSeaGreen;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].BackColor = Color.MediumSeaGreen;
                }
            }
        da.Close();
        mysql.Close();
        FpSpread2.Sheets[0].ColumnHeader.RowCount = 2;
        FpSpread2.ActiveSheetView.SheetCornerSpanModel.Add(0, 0, 2, 1);
        // FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 2);//Hidden By Srinath 17/8/2013
        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
        FpSpread2.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Roll No";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Type";
        FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
        FpSpread2.Sheets[0].Columns[1].CellType = textcel_type;
        FpSpread2.Sheets[0].Columns[2].CellType = textcel_type;
        FpSpread2.Sheets[0].Columns[0].CellType = textcel_type;

        FpSpread2.Sheets[0].Columns[0].Width = 50;
        //  string[] strcomo3;
        FarPoint.Web.Spread.ComboBoxCellType objintcell3 = new FarPoint.Web.Spread.ComboBoxCellType();
        //  strcomo3 = new string[] { " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" };
        //---------------------------------load rights                      

        string[] strcomo3 = new string[20];
        i = 0;
        ; cmd = new SqlCommand();
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            cmd.CommandText = "select rights from  OD_Master_Setting where group_code=" + Session["group_code"].ToString() + "";
        }
        else
        {
            cmd.CommandText = "select rights from  OD_Master_Setting where usercode=" + Session["UserCode"].ToString() + "";
        }
        cmd.Connection = con;
        con.Close();
        con.Open();
        dr_rights_od = cmd.ExecuteReader();
        if (dr_rights_od.HasRows)
        {
            while (dr_rights_od.Read())
            {
                string od_rights = string.Empty;
                Hashtable od_has = new Hashtable();
                od_rights = dr_rights_od["rights"].ToString();
                if (od_rights != string.Empty)
                {
                    string[] split_od_rights = od_rights.Split(',');
                    strcomo3 = new string[split_od_rights.GetUpperBound(0) + 1];
                    for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                    {
                        strcomo3[i++] = split_od_rights[od_temp].ToString();
                    }
                }
                else
                {
                    strcomo3[0] = string.Empty;
                    strcomo3[1] = "P";
                    strcomo3[2] = "A";
                    strcomo3[3] = "OD";
                    strcomo3[4] = "SOD";
                    strcomo3[5] = "ML";
                    strcomo3[6] = "NSS";
                    strcomo3[7] = "L";
                    strcomo3[8] = "NCC";
                    strcomo3[9] = "HS";
                    strcomo3[10] = "PP";
                    strcomo3[11] = "SYOD";
                    strcomo3[12] = "COD";
                    strcomo3[13] = "OOD";
                    strcomo3[14] = "LA";
                }
            }
        }
        else
        {
            strcomo3[0] = string.Empty;
            strcomo3[1] = "P";
            strcomo3[2] = "A";
            strcomo3[3] = "OD";
            strcomo3[4] = "SOD";
            strcomo3[5] = "ML";
            strcomo3[6] = "NSS";
            strcomo3[7] = "L";
            strcomo3[8] = "NCC";
            strcomo3[9] = "HS";
            strcomo3[10] = "PP";
            strcomo3[11] = "SYOD";
            strcomo3[12] = "COD";
            strcomo3[13] = "OOD";
            strcomo3[14] = "LA";
        }
        objintcell3 = new FarPoint.Web.Spread.ComboBoxCellType(strcomo3);
        objintcell3.ShowButton = true;
        objintcell3.AutoPostBack = true;
        objintcell3.UseValue = true;
        FpSpread2.ActiveSheetView.Columns[0].CellType = objintcell3;
        FpSpread2.SaveChanges();
        FpSpread2.Sheets[0].Columns[0].BackColor = Color.MistyRose;
        string strsec1 = string.Empty;
        if (ddlsec.Text.ToString() == "All" || ddlsec.Text.ToString() == "")
        {
            strsec1 = string.Empty;
        }
        else
        {
            strsec1 = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
        }
        string getval_var = GetFunction("select isflag from Manual_schedule where college_code=13");
        if (getval_var != "" && getval_var == "1")
        {
            Panel3.Visible = true;
            Panelhour.Visible = true;
            // ckhr2.Visible = true;
            btnok.Visible = true;
            string str_Date = txtFromDate.Text;
            string[] split_d = str_Date.Split(new Char[] { '-' });
            string str_day = split_d[0].ToString();
            string Atmonth = split_d[1].ToString();
            string Atyear = split_d[2].ToString();
            long strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
            string date1 = txtFromDate.Text.ToString();
            string[] split = date1.Split(new Char[] { '-' });
            string datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
            string date2 = TxtToDate.Text.ToString();
            string[] split1 = date2.Split(new Char[] { '-' });
            string dateto = split1[1].ToString() + "-" + split1[0].ToString() + "-" + split1[2].ToString();
            DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
            DateTime dt2 = Convert.ToDateTime(dateto.ToString());
            TimeSpan t = dt2.Subtract(dt1);
            long days = t.Days;
            if (days >= 0)
            {
                datelbl.Text = string.Empty;
                string[] differdays = new string[days];
                string sqlstr1 = "select distinct * from Direct_Schedule where degree_code=" + ddlbranch.SelectedValue + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and month_year = " + strdate + " " + strsec1 + "";
                mysql.Open();
                SqlCommand cmd_indi = new SqlCommand(sqlstr1, mysql);
                SqlDataReader reader_indi;
                reader_indi = cmd_indi.ExecuteReader();
                int count = 0;
                if (reader_indi.HasRows == true)
                {
                    Panel3.Visible = true;
                    ddlsubject.Enabled = true;
                    ckmanual.Checked = true;
                    while (reader_indi.Read())
                    {
                    HOLDAY:
                        if (dt1.ToString("dddd") == "Sunday")
                        {
                            lblset.Visible = true;
                            lblset.Text = "    " + lblset.Text + dt1.ToString("d-MM-yyyy") + "-holiday" + " Sunday  ";
                            dt1 = dt1.AddDays(1);
                        }
                        string holday = GetFunction("select holiday_desc from holidaystudents where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and holiday_date=" + dt1.ToString("yyyy-MM-d"));
                        if (holday != "")
                        {
                            lblset.Visible = true;
                            lblset.Text = "    " + lblset.Text + dt1.ToString("d-MM-yyyy") + "-holiday";
                            dt1 = dt1.AddDays(1);
                            goto HOLDAY;
                        }
                        if (Convert.ToInt32(noofhours.ToString()) != 0)  // First Date
                        {
                            for (int ini_loop = 1; ini_loop <= Convert.ToInt32(noofhours.ToString()); ini_loop++)
                            {
                                if (dt1 > dt2) break;
                                string str_dateck = "d" + dt1.Day + "d" + ini_loop.ToString();
                                if (reader_indi[str_dateck].ToString() == sub_no.ToString())
                                {
                                    count = count + 1;
                                    ckhr2.Items[ini_loop - 1].Selected = true;
                                    //FpSpread2.Sheets[0].ColumnCount = FpSpread2.Sheets[0].ColumnCount + 1;
                                    //FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = ini_loop - 1;
                                    FpSpread2.Sheets[0].SheetCorner.RowCount = 2; //Set Date
                                    FpSpread2.Sheets[0].ColumnCount = FpSpread2.Sheets[0].ColumnCount + 1;
                                    FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 80;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = dt1.ToString("d-MM-yyyy");
                                    //FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, Convert.ToInt32(noofhours.ToString()));
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ini_loop);
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Tag = dt1;
                                    //FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].CellType = objintcell;   //Set Hour
                                    //FpSpread2.ActiveSheetView.Columns[FpSpread2.Sheets[0].ColumnCount - 1].CellType = objcom;
                                }
                            }
                            if (count != 0)
                                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, count);
                            //-----------Next date
                            //string Att_changedate1;
                            //string Att_dateformate1;
                            //string Att_changedate2;
                            //string Att_dateformate2;
                            //Att_changedate1 =string.Empty;
                            //Att_dateformate1 =string.Empty;
                            //Att_changedate2 =string.Empty;
                            //Att_dateformate2 =string.Empty;
                            for (int date_loop = 1; date_loop <= days; date_loop++) //Next Next Date
                            {
                                DateTime temp_date = dt1.AddDays(date_loop);
                                if (temp_date > dt2) break;
                                if (temp_date.ToString("dddd") == "Sunday")
                                {
                                    lblset.Visible = true;
                                    lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "-holiday" + " Sunday  ";
                                    //dt1 = dt1.AddDays(1);
                                    continue;
                                }
                                string holiday = GetFunction("select holiday_desc from holidaystudents where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and holiday_date=" + temp_date.ToString("yyyy-MM-d"));
                                if (holday != "")
                                {
                                    lblset.Visible = true;
                                    lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "-holiday";
                                    //dt1 = dt1.AddDays(1);
                                    continue;
                                }
                                differdays[date_loop - 1] = temp_date.ToString("d-MM-yyyy");
                                i = 0;
                                string dateformat;
                                //string Att_changedate;
                                //string Att_changryear;
                                //string Att_changesplit;
                                //string Att_dateformate;
                                for (i = 1; i <= Convert.ToInt32(noofhours.ToString()); i++)
                                {
                                    string ck_hr1 = string.Empty;
                                    ck_hr1 = i.ToString();
                                    int ini1;
                                    string ini_str1 = string.Empty;
                                    ini1 = i;
                                    ini_str1 = Convert.ToString(ini1);
                                    string str_dateck = "d" + dt1.Day + "d" + i.ToString();
                                    if (reader_indi[str_dateck].ToString() == sub_no.ToString())
                                    {
                                        ckhr2.Items[i - 1].Selected = true;
                                        FpSpread2.Sheets[0].ColumnCount = FpSpread2.Sheets[0].ColumnCount + 1;
                                        dateformat = FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(differdays[date_loop - 1]);
                                        //string[] split2 = dateformat.Split(new Char[] { '-' });
                                        ////Att_changedate = split2[0].ToString() + "/" + split2[1].ToString();
                                        //Att_changedate = split2[1].ToString() + "-" + split2[0].ToString() + "-" + split2[2].ToString();
                                        //Att_changryear = split2[2].ToString();
                                        //string[] split5 = Att_changryear.Split(new Char[] { ' ' });
                                        //Att_changesplit = split5[0].ToString();
                                        //Att_dateformate = split2[1].ToString() + "/" + split2[0].ToString() + "/" + split5[0].ToString();
                                        //// Att_changedate = AttSpread.Sheets[0].ColumnHeader.Cells[0, AttSpread.Sheets[0].ColumnCount - 1].Text = Convert.ToString(differdays[date_loop - 1]);

                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = dateformat.ToString();
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Tag = dateformat.ToString();
                                        //FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - ), 1, Convert.ToInt32(noofhours.ToString()));
                                        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, (FpSpread2.Sheets[0].ColumnCount - count), 1, count);
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(i);
                                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(i);
                                        //FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].CellType = objintcell;
                                        //FpSpread2.ActiveSheetView.Columns[FpSpread2.Sheets[0].ColumnCount - 1].CellType = objcom;
                                        FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 80;
                                    }
                                }
                            }
                        }
                    }
                }
                mysql.Close();
                int temp = 0;
                for (temp = 4; temp < FpSpread2.Sheets[0].ColumnCount; temp++)
                    FpSpread2.ActiveSheetView.Cells[0, temp].CellType = objintcell3;
                for (temp = 4; temp < FpSpread2.Sheets[0].ColumnCount; temp++)
                    FpSpread2.ActiveSheetView.Columns[temp].CellType = objcom;
                //----------------------------Reterive the saved values
                int Att_mark_row1;
                int Att_mark_column1;
                string str_Date1;
                string str_day1;
                string Atmonth1;
                string Atyear1;
                long str_date1;
                // string Att_str_hour1;
                string rollno_Att1 = string.Empty;
                string Att_dcolumn1 = string.Empty;
                string Att_strqueryst1 = string.Empty;
                string Att_Markvalue1;
                string Att_Mark11;
                temp = 0;
                for (Att_mark_row1 = 1; Att_mark_row1 <= FpSpread2.Sheets[0].RowCount - 1; Att_mark_row1++)
                {
                    for (Att_mark_column1 = 4; Att_mark_column1 <= FpSpread2.Sheets[0].ColumnCount - 1; Att_mark_column1++)
                    {
                        str_Date1 = FpSpread2.Sheets[0].ColumnHeader.Cells[0, Att_mark_column1].Text;
                        rollno_Att1 = FpSpread2.Sheets[0].Cells[Att_mark_row1, 0].Text.ToString();
                        string[] split_d1 = str_Date1.Split(new Char[] { '-' });
                        str_day1 = split_d1[0].ToString();
                        Atmonth1 = split_d1[1].ToString();
                        Atyear1 = split_d1[2].ToString();
                        str_date1 = (Convert.ToInt32(Atmonth1) + Convert.ToInt32(Atyear1) * 12);
                        int Att_hour1;
                        Att_hour1 = Convert.ToInt32(FpSpread2.Sheets[0].ColumnHeader.Cells[1, Att_mark_column1].Text);
                        Att_dcolumn1 = "d" + Convert.ToInt16(str_day1) + "d" + Att_hour1;
                        Att_strqueryst1 = "select " + Att_dcolumn1 + " from Attendance where Roll_no='" + rollno_Att1.ToString() + "' and month_year=" + str_date1.ToString() + "";
                        Att_Markvalue1 = GetFunction(Att_strqueryst1);
                        Att_Mark11 = Attmark(Att_Markvalue1);
                        FpSpread2.Sheets[0].SetValue(Att_mark_row1, Att_mark_column1, Att_Markvalue1.ToString());
                        FpSpread2.Sheets[0].SetText(Att_mark_row1, Att_mark_column1, Att_Mark11.ToString());
                        if (Att_Mark11 != "")
                        {
                            temp = temp + 1;
                        }
                    }
                }
                if (temp > 0)
                {
                    Buttonsave.Visible = false;
                    Buttonupdate.Visible = true;
                    Buttonexit.Visible = false;
                }
            }
        }
        else
        {
            ckmanual.Checked = false;
            Panelhour.Visible = false;
            //ckhr2.Visible = false;
            btnok.Enabled = false;
        }
        if (FpSpread2.Sheets[0].RowCount > 1)
        {
            // FpSpread2.Sheets[0].AutoPostBack = true;
            FpSpread2.SaveChanges();
            pHeaderatendence.Visible = true;
            pBodyatendence.Visible = true;
            FpSpread2.Visible = true;
            Buttontotal.Visible = true;
            // lblrecord.Visible = true;
            DropDownListpage.Visible = true;
            TextBoxother.Visible = false;
            //lblpage.Visible = true;
            TextBoxpage.Visible = true;
            FpSpread2.Visible = true;
            Panelpage.Visible = false;
            Double totalRows = 0;
            totalRows = Convert.ToInt32(FpSpread2.Sheets[0].RowCount);
            Session["totalPages"] = (int)Math.Ceiling(totalRows / FpSpread2.Sheets[0].PageSize);
            Buttontotal.Text = "Records: " + totalRows + "  Pages: 1";
            DropDownListpage.Items.Clear();
            if (totalRows >= 10)
            {
                FpSpread2.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                {
                    DropDownListpage.Items.Add((k + 10).ToString());
                }
                DropDownListpage.Items.Add("Others");
                FpSpread2.Height = 250 + FpSpread2.Sheets[0].ColumnHeader.Height;
                DropDownListpage.SelectedIndex = DropDownListpage.Items.Count - 2;
            }
            else
            {
                FpSpread2.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                DropDownListpage.Items.Add(FpSpread2.Sheets[0].PageSize.ToString());
                FpSpread2.Height = FpSpread2.Sheets[0].ColumnHeader.Height + (25 * Convert.ToInt32(totalRows));
            }
            FpSpread2.Columns.Default.Font.Name = "Book Antiqua";
            FpSpread2.Columns.Default.Font.Size = FontUnit.Medium;
            //FpSpread2.Sheets[0].Columns[0].Width = len * 11;
            //FpSpread2.Sheets[0].Columns[1].Width = len1 * 11;
            //FpSpread2.Sheets[0].Columns[2].Width = len2 * 11;
            //FpSpread2.Sheets[0].Columns[3].Width = len3 * 12;
            //FpSpread2.Sheets[0].Columns[4].Width = len4 * 11;
            //FpSpread2.Sheets[0].Columns[5].Width = 100;
            //FpSpread2.Sheets[0].Columns[6].Width = 100;
            int widt = 0;
            for (int col = 0; col < FpSpread2.Sheets[0].ColumnCount; col++)
                widt = widt + FpSpread2.Sheets[0].Columns[col].Width;
            widt = widt + FpSpread2.Sheets[0].RowHeader.Width + 15;

            if (widt > 900)
            {
                FpSpread2.Width = 900;
            }
            else
                FpSpread2.Width = widt;
            // FpSpread2.Sheets[0].DefaultRowHeight = 20;
            FpSpread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            FpSpread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            FpSpread2.SaveChanges();
            pHeaderatendence.Visible = true;
            pBodyatendence.Visible = true;
        }
        else
        {
            load_spread();
            lblset.Visible = true;
            lblset.Text = "There are no Students Available";
            Btngo.Visible = true;
            btnok.Visible = false;
            ddlsubject.Visible = false;
            cksubjectwise.Visible = true;
        }
        Panel3.Visible = false;
        if (FpSpread2.Sheets[0].ColumnCount > 5)
        {
            Panel3.Visible = true;
            Buttonsave.Visible = false;
            Buttonupdate.Visible = true;
            Buttonexit.Visible = false;
            Buttonselectall.Visible = true;
            Buttondeselect.Visible = true;
            // ckhr2_SelectedIndexChanged(sender,e);
        }
        //string sub_no="";
        //sub_no=ddlsubject.SelectedValue;
        FpSpread2.Height = 300;
        FpSpread2.Sheets[0].AutoPostBack = false;
        if (ckmanual.Checked == true)
        {
            //  Panelhour.Visible = true;
            Panelhour.Visible = true;
            btnok.Enabled = true;
        }
        else
        {
            //  Panelhour.Visible = false;
            Panelhour.Visible = false;
            btnok.Enabled = false;
        }
    }

    protected void cksubjectwise_CheckedChanged(object sender, EventArgs e)
    {
        lblset.Text = string.Empty;
        lblfromdate.Text = string.Empty;
        FpSpread2.Visible = false;
        pHeaderatendence.Visible = false;
        pBodyatendence.Visible = false;
        lbltodate.Text = string.Empty;
        lblset.Visible = false;
        lblother.Visible = false;
        LabelE.Visible = false;
        if (txtFromDate.Text == "")
        {
            lblfromdate.Text = "Select From Date";
            lblfromdate.Visible = true;
            return;
        }
        if (TxtToDate.Text == "")
        {
            lbltodate.Text = "Select To Date";
            lbltodate.Visible = true;
            return;
        }
        if (cksubjectwise.Checked == true)
        {
            datelbl.Visible = false;
            string date1 = string.Empty;
            string date2 = string.Empty;
            string datefrom;
            string dateto = string.Empty;
            date1 = txtFromDate.Text.ToString();
            string[] split = date1.Split(new Char[] { '-' });
            datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
            date2 = TxtToDate.Text.ToString();
            string[] split1 = date2.Split(new Char[] { '-' });
            dateto = split1[1].ToString() + "-" + split1[0].ToString() + "-" + split1[2].ToString();
            DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
            DateTime dt2 = Convert.ToDateTime(dateto.ToString());
            TimeSpan t = dt2.Subtract(dt1);
            long days = t.Days;
            if (days < 0)
            {
                datelbl.Visible = true;
                datelbl.Text = "From date should be less than To date";
                cksubjectwise.Checked = false;
                return;
            }
            if (days == 0 && dt1.ToString("dddd") == "Sunday")
            {
                datelbl.Visible = true;
                datelbl.Text = "Selected Day is Sunday";
                cksubjectwise.Checked = false;
                return;
            }
            if (dt1 > DateTime.Today)
            {
                lblset.Visible = true;
                lblset.Text = "You can not mark attendance for the date greater than today";
                txtFromDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                cksubjectwise.Checked = false;
                return;
            }
            else
            {
                lblset.Visible = false;
            }
            if (dt2 > DateTime.Today)
            {
                lblset.Visible = true;
                lblset.Text = "You can not mark attendance for the date greater than today";
                TxtToDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                return;
            }
            else
            {
                lblset.Visible = false;
            }
            lblset.Visible = false;
            Btngo.Visible = false;
            //  if (option.SelectedValue == "1")
            {
                ddlsubject.Visible = true;
                string strsec = string.Empty;
                string section = ddlsec.SelectedValue.ToString();
                if (!Page.IsPostBack == false)
                {
                    if (ddlsec.Enabled == true)
                        if (ddlsec.Text.ToString().Trim().ToLower() == "all" || ddlsec.Text.ToString() == "")
                        {
                            strsec = string.Empty;
                        }
                        else
                        {
                            strsec = " and Sections='" + section + "'";
                        }
                }

                mysql.Open();
                //string query = "select distinct staff_selector.subject_no,subject_name,subject_code from syllabus_master,subject,staff_selector,sub_sem where syllabus_master.degree_code =" + int.Parse(ddlbranch.SelectedValue) + " And syllabus_master.Semester = " + int.Parse(ddlsem.SelectedValue) + " And syllabus_master.batch_year = " + int.Parse(ddlbatch.SelectedValue) + " and staff_selector.batch_year=syllabus_master.batch_year and syllabus_master.syll_code=subject.syll_code and sub_sem.syll_code=syllabus_master.syll_code and sub_sem.subtype_no=subject.subtype_no and subject.subject_no= staff_selector.subject_no  " + strsec + " and work_load is null group by  staff_selector.subject_no having count(*) > 1";
                string query = "select distinct staff_selector.subject_no,subject_name,subject_code from syllabus_master,subject,staff_selector,sub_sem where syllabus_master.degree_code =" + int.Parse(ddlbranch.SelectedValue) + " And syllabus_master.Semester = " + int.Parse(ddlsem.SelectedValue) + " And syllabus_master.batch_year = " + int.Parse(ddlbatch.SelectedValue) + " and staff_selector.batch_year=syllabus_master.batch_year and syllabus_master.syll_code=subject.syll_code and sub_sem.syll_code=syllabus_master.syll_code and sub_sem.subtype_no=subject.subtype_no and subject.subject_no= staff_selector.subject_no  " + strsec + " and work_load is null";
                SqlCommand com = new SqlCommand(query, mysql);
                SqlDataReader dr;
                dr = com.ExecuteReader();
                ddlsubject.Items.Clear();
                if (dr.HasRows == true)
                {
                    lblset.Visible = false;
                    while (dr.Read())
                    {
                        ListItem li = new ListItem();
                        li.Value = dr["subject_no"].ToString();
                        li.Text = (dr["subject_name"].ToString());
                        ddlsubject.Items.Add(li);
                    }
                    ddlsubject.Items.Insert(0, "---Select---");
                }
                else
                {
                    lblset.Text = "There are no subjects available for the given semester";
                    ddlsubject.Visible = false;
                    lblset.Visible = true;
                    ckmanual.Enabled = false;
                    btnok.Enabled = false;
                    pHeaderatendence.Visible = false;
                    pBodyatendence.Visible = false;
                }
                dr.Close();
                mysql.Close();
            }
            //else
            //{
            //    ddlsubject.Visible = false;
            //}
        }
        else
        {
            Panelhour.Visible = false;
            ddlsubject.Visible = false;
            ckmanual.Enabled = false;
            btnok.Enabled = false;
            lblset.Text = "Select Subjectwise and then proceed";
            lblset.Visible = true;
        }
        //if (ckmanual.Checked == true)
        //{
        //   // Panelhour.Visible = true;
        //    tbhour.Visible = true;
        //}
        //else
        //{
        //    //Panelhour.Visible = false;
        //    tbhour.Visible = false;
        //}

    }

    protected void btnok_Click(object sender, EventArgs e)
    {
        lblset.Visible = false;
        lblother.Visible = false;
        LabelE.Visible = false;
        lblset.Text = string.Empty;
        // if (option.SelectedValue == "1")
        {
            datelbl.Visible = false;
            FpSpread2.Sheets[0].ColumnCount = 5;
            Buttonsave.Visible = true;
            pHeaderatendence.Visible = true;
            pBodyatendence.Visible = true;
            string date1 = string.Empty;
            string datefrom = string.Empty;
            string date2 = string.Empty;
            string dateto = string.Empty;
            //int noofhours;
            FpSpread2.Visible = true;
            //string deg_code = GetFunction("select distinct degree_code from degree where course_id=" + ddldegree.SelectedItem.Value.ToString() + " and dept_code=" + ddlbranch.SelectedItem.Value.ToString() + "");

            string sqlstr = GetFunction("select No_of_hrs_per_day  from PeriodAttndSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedItem.Value.ToString() + "");
            int noofhours = 0;
            if (sqlstr != "")
                noofhours = Convert.ToInt32(sqlstr);
            date1 = txtFromDate.Text.ToString();
            string[] split = date1.Split(new Char[] { '-' });
            datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
            date2 = TxtToDate.Text.ToString();
            string[] split1 = date2.Split(new Char[] { '-' });
            dateto = split1[1].ToString() + "-" + split1[0].ToString() + "-" + split1[2].ToString();
            DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
            DateTime dt2 = Convert.ToDateTime(dateto.ToString());
            TimeSpan t = dt2.Subtract(dt1);
            long days = t.Days;
            if (days < 0)
            {
                datelbl.Visible = true;
                datelbl.Text = "From date should  be less than To date";
            }
            if (days == 0 && dt1.ToString("dddd") == "Sunday")
            {
                lblset.Visible = true;
                lblset.Text = "Selected Day is Sunday";
                return;
            }
            if (dt1 > DateTime.Today)
            {
                lblset.Visible = true;
                lblset.Text = "You can not mark attendance for the date greater than today";
                txtFromDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                return;
            }
            else
            {
                lblset.Visible = false;
            }
            if (dt2 > DateTime.Today)
            {
                lblset.Visible = true;
                lblset.Text = "You can not mark attendance for the date greater than today";
                TxtToDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                return;
            }
            else
            {
                lblset.Visible = false;
            }

            int cnt = 0;
            for (int item = 0; item < ckhr2.Items.Count; item++)
            {
                if (ckhr2.Items[item].Selected == true)
                {
                    ++cnt;
                    //tbhour.Text = tbhour.Text + " " + ckhr2.Items[item].Text;
                }
            }
            if (cnt == 0)
            {
                lblset.Text = "Select the hour and then proceed";
                lblset.Visible = true;
                // tbhour.Text = "Select the hour";
                return;
            }

            lblset.Visible = false;

            lblset.Text = string.Empty;
        HOLDAY:
            if (dt1.ToString("dddd") == "Sunday")
            {
                lblset.Visible = true;
                lblset.Text = "    " + lblset.Text + dt1.ToString("d-MM-yyyy") + "-holiday" + " Sunday  ";
                dt1 = dt1.AddDays(1);

            }

            string holday = GetFunction("select holiday_desc from holidaystudents where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and holiday_date=" + dt1.ToString("yyyy-MM-d"));
            if (holday != "")
            {
                lblset.Visible = true;
                lblset.Text = "    " + lblset.Text + dt1.ToString("d-MM-yyyy") + "-holiday";
                dt1 = dt1.AddDays(1);
                goto HOLDAY;
            }

            if (days >= 0)
            {
                string[] differdays = new string[days];


                if (noofhours != 0)  // First Date
                {
                    int count = 0;
                    for (int ini_loop = 1; ini_loop <= noofhours; ini_loop++)
                    {
                        if (dt1 > dt2) break;
                        string ck_hr = string.Empty;
                        ck_hr = ckhr2.Items[ini_loop - 1].Text;
                        int ini;
                        string ini_str = string.Empty;

                        ini = ini_loop;
                        ini_str = Convert.ToString(ini);
                        if (ck_hr == ini_str && ckhr2.Items[ini_loop - 1].Selected == true)
                        {

                            count = count + 1;
                            FpSpread2.Sheets[0].SheetCorner.RowCount = 2; //Set Date
                            FpSpread2.Sheets[0].ColumnCount = FpSpread2.Sheets[0].ColumnCount + 1;
                            FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 80;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = dt1.ToString("d-MM-yyyy");

                            //FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, Convert.ToInt32(noofhours.ToString()));

                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ini_loop);

                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Tag = dt1;


                        }
                    }
                    if (count != 0)
                        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, count);

                    //-----------Next date
                    //string Att_changedate1;
                    //string Att_dateformate1;
                    //string Att_changedate2;
                    //string Att_dateformate2;
                    //Att_changedate1 =string.Empty;
                    //Att_dateformate1 =string.Empty;
                    //Att_changedate2 =string.Empty;
                    //Att_dateformate2 =string.Empty;

                    for (int date_loop = 1; date_loop <= days; date_loop++) //Next Next Date
                    {


                        DateTime temp_date = dt1.AddDays(date_loop);
                        if (temp_date > dt2) break;
                        if (temp_date.ToString("dddd") == "Sunday")
                        {
                            lblset.Visible = true;
                            lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "-holiday" + " Sunday  ";
                            //dt1 = dt1.AddDays(1);
                            continue;

                        }
                        string holiday = GetFunction("select holiday_desc from holidaystudents where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and holiday_date=" + temp_date.ToString("yyyy-MM-d"));
                        if (holday != "")
                        {
                            lblset.Visible = true;
                            lblset.Text = "    " + lblset.Text + temp_date.ToString("d-MM-yyyy") + "-holiday";
                            //dt1 = dt1.AddDays(1);
                            continue;
                        }
                        differdays[date_loop - 1] = temp_date.ToString("d-MM-yyyy");

                        int i;
                        string dateformat;
                        //string Att_changedate;
                        //string Att_changryear;
                        //string Att_changesplit;
                        //string Att_dateformate;
                        for (i = 1; i <= noofhours; i++)
                        {

                            string ck_hr1 = string.Empty;
                            ck_hr1 = ckhr2.Items[i - 1].Text;
                            int ini1;
                            string ini_str1 = string.Empty;

                            ini1 = i;
                            ini_str1 = Convert.ToString(ini1);
                            if (ck_hr1 == ini_str1 && ckhr2.Items[i - 1].Selected == true)
                            {

                                FpSpread2.Sheets[0].ColumnCount = FpSpread2.Sheets[0].ColumnCount + 1;

                                dateformat = FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(differdays[date_loop - 1]);

                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = dateformat.ToString();
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Tag = dateformat.ToString();
                                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, (FpSpread2.Sheets[0].ColumnCount - count), 1, count);
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(i);
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(i);

                                FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 80;
                            }

                        }


                    }
                }

                FarPoint.Web.Spread.ComboBoxCellType objintcell7 = new FarPoint.Web.Spread.ComboBoxCellType();
                //string[] strcomo1 = new string[] { "Select for All ", " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" };
                //string[] strcomo = new string[] { " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" };//------21/6/12 PRABHA
                //---------------------------------load rights                      
                string[] strcomo1 = new string[20];
                string[] strcomo = new string[20];
                int ii = 0, j = 0;
                SqlCommand cmd = new SqlCommand();
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    cmd.CommandText = "select rights from  OD_Master_Setting where group_code=" + Session["group_code"].ToString() + "";
                }
                else
                {
                    cmd.CommandText = "select rights from  OD_Master_Setting where usercode=" + Session["UserCode"].ToString() + "";
                }

                cmd.Connection = con;
                con.Close();
                con.Open();
                SqlDataReader dr_rights_od = cmd.ExecuteReader();
                if (dr_rights_od.HasRows)
                {
                    while (dr_rights_od.Read())
                    {

                        string od_rights = string.Empty;
                        Hashtable od_has = new Hashtable();

                        od_rights = dr_rights_od["rights"].ToString();

                        if (od_rights != string.Empty)
                        {
                            string[] split_od_rights = od_rights.Split(',');

                            strcomo = new string[split_od_rights.GetUpperBound(0) + 1];
                            strcomo1 = new string[split_od_rights.GetUpperBound(0) + 2];
                            strcomo1[j++] = "Select for All ";
                            for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                            {

                                strcomo[ii++] = split_od_rights[od_temp].ToString();
                                strcomo1[j++] = split_od_rights[od_temp].ToString();
                            }

                        }
                        else
                        {
                            strcomo[0] = string.Empty;
                            strcomo[1] = "P";
                            strcomo[2] = "A";
                            strcomo[3] = "OD";
                            strcomo[4] = "SOD";
                            strcomo[5] = "ML";
                            strcomo[6] = "NSS";
                            strcomo[7] = "L";
                            strcomo[8] = "NCC";
                            strcomo[9] = "HS";
                            strcomo[10] = "PP";
                            strcomo[11] = "SYOD";
                            strcomo[12] = "COD";
                            strcomo[13] = "OOD";
                            strcomo[14] = "LA";

                            strcomo1[0] = "Select for All";
                            strcomo1[1] = string.Empty;
                            strcomo1[2] = "P";
                            strcomo1[3] = "A";
                            strcomo1[4] = "OD";
                            strcomo1[5] = "SOD";
                            strcomo1[6] = "ML";
                            strcomo1[7] = "NSS";
                            strcomo1[8] = "L";
                            strcomo1[9] = "NCC";
                            strcomo1[10] = "HS";
                            strcomo1[11] = "PP";
                            strcomo1[12] = "SYOD";
                            strcomo1[13] = "COD";
                            strcomo1[14] = "OOD";
                            strcomo1[15] = "LA";
                        }
                    }
                }
                else
                {
                    strcomo[0] = string.Empty;
                    strcomo[1] = "P";
                    strcomo[2] = "A";
                    strcomo[3] = "OD";
                    strcomo[4] = "SOD";
                    strcomo[5] = "ML";
                    strcomo[6] = "NSS";
                    strcomo[7] = "L";
                    strcomo[8] = "NCC";
                    strcomo[9] = "HS";
                    strcomo[10] = "PP";
                    strcomo[11] = "SYOD";
                    strcomo[12] = "COD";
                    strcomo[13] = "OOD";
                    strcomo[14] = "LA";


                    strcomo1[0] = "Select for All";
                    strcomo1[1] = string.Empty;
                    strcomo1[2] = "P";
                    strcomo1[3] = "A";
                    strcomo1[4] = "OD";
                    strcomo1[5] = "SOD";
                    strcomo1[6] = "ML";
                    strcomo1[7] = "NSS";
                    strcomo1[8] = "L";
                    strcomo1[9] = "NCC";
                    strcomo1[10] = "HS";
                    strcomo1[11] = "PP";
                    strcomo1[12] = "SYOD";
                    strcomo1[13] = "COD";
                    strcomo1[14] = "OOD";
                    strcomo1[15] = "LA";
                }

                //---------------------------
                objintcell7 = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
                objintcell7.ShowButton = true;
                objintcell7.AutoPostBack = true;
                objintcell7.UseValue = true;
                FpSpread2.SaveChanges();

                FarPoint.Web.Spread.ComboBoxCellType objcom = new FarPoint.Web.Spread.ComboBoxCellType();
                objcom = new FarPoint.Web.Spread.ComboBoxCellType(strcomo);

                objcom.AutoPostBack = true;
                //  objcom.UseValue = true;

                int temp = 0;
                for (temp = 4; temp < FpSpread2.Sheets[0].ColumnCount; temp++)
                    FpSpread2.ActiveSheetView.Cells[0, temp].CellType = objintcell7;
                for (temp = 4; temp < FpSpread2.Sheets[0].ColumnCount; temp++)
                    FpSpread2.ActiveSheetView.Columns[temp].CellType = objcom;

                //----------------------------Reterive the saved values

                int Att_mark_row;
                int Att_mark_column;
                string str_Date;
                string str_day;
                string Atmonth;
                string Atyear;
                long str_date;
                //string Att_str_hour;
                string rollno_Att = string.Empty;
                string Att_dcolumn = string.Empty;
                string Att_strqueryst = string.Empty;
                string Att_Markvalue;
                string Att_Mark1;

                temp = 0;

                for (Att_mark_row = 1; Att_mark_row <= FpSpread2.Sheets[0].RowCount - 1; Att_mark_row++)
                {
                    for (Att_mark_column = 4; Att_mark_column <= FpSpread2.Sheets[0].ColumnCount - 1; Att_mark_column++)
                    {
                        str_Date = FpSpread2.Sheets[0].ColumnHeader.Cells[0, Att_mark_column].Text;
                        rollno_Att = FpSpread2.Sheets[0].Cells[Att_mark_row, 0].Text.ToString();
                        string[] split_d = str_Date.Split(new Char[] { '-' });
                        str_day = split_d[0].ToString();
                        Atmonth = split_d[1].ToString();
                        Atyear = split_d[2].ToString();
                        str_date = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                        int Att_hour;
                        Att_hour = Convert.ToInt32(FpSpread2.Sheets[0].ColumnHeader.Cells[1, Att_mark_column].Text);
                        Att_dcolumn = "d" + str_day + "d" + Att_hour;

                        Att_strqueryst = "select " + Att_dcolumn + " from Attendance where Roll_no='" + rollno_Att.ToString() + "' and month_year=" + str_date.ToString() + "";

                        Att_Markvalue = GetFunction(Att_strqueryst);
                        Att_Mark1 = Attmark(Att_Markvalue);
                        FpSpread2.Sheets[0].SetValue(Att_mark_row, Att_mark_column, Att_Markvalue.ToString());
                        FpSpread2.Sheets[0].SetText(Att_mark_row, Att_mark_column, Att_Mark1.ToString());

                        if (Att_Mark1 != "")
                        {
                            temp = temp + 1;
                        }

                    }
                }

                if (temp > 0)
                {
                    Buttonsave.Visible = true;
                    Buttonupdate.Visible = false;
                    Buttonexit.Visible = false;
                }
            }

            if (FpSpread2.Sheets[0].ColumnCount > 5)
            {
                Buttonselectall.Visible = true;
                Buttondeselect.Visible = true;
                Buttonsave.Visible = true;
                Panel3.Visible = true;
                int widt = 0;
                for (int col = 0; col < FpSpread2.Sheets[0].ColumnCount; col++)
                    widt = widt + FpSpread2.Sheets[0].Columns[col].Width;
                widt = widt + FpSpread2.Sheets[0].RowHeader.Width + 15;
                if (widt > 900)
                {
                    FpSpread2.Width = 900;
                }
                else
                    FpSpread2.Width = widt;
            }
            else
                Panel3.Visible = false;
            FpSpread2.SaveChanges();
            Buttonupdate.Visible = false;
            Buttonexit.Visible = false;
        }

    }

    protected void ckmanual_CheckedChanged(object sender, EventArgs e)
    {
        //  Panelhour.Visible = true;]
        lbltodate.Visible = false;
        lblfromdate.Visible = false;
        lblset.Text = string.Empty;
        Panelhour.Visible = true;
        if (ckmanual.Checked == true)
        {
            if (cksubjectwise.Checked == false)
            {
                lblset.Visible = true;
                lblset.Text = "Select subjectwise and then proceed";
                Panelhour.Visible = false;
                ckmanual.Enabled = false;
                btnok.Enabled = false;
                FpSpread2.Visible = false;
                pHeaderatendence.Visible = false;
                pBodyatendence.Visible = false;
                ddlsubject.Visible = false;
                return;
            }
            if (ddlsubject.SelectedIndex == 0)
            {
                lblset.Visible = true;
                lblset.Text = "Select the subject and then proceed";
                Panelhour.Visible = false;
                ckmanual.Enabled = false;
                btnok.Enabled = false;
                FpSpread2.Visible = false;
                pHeaderatendence.Visible = false;
                pBodyatendence.Visible = false;
                return;
            }
            if (txtFromDate.Text == "")
            {
                lblfromdate.Text = "Select From Date";
                lblfromdate.Visible = true;
                return;
            }
            if (TxtToDate.Text == "")
            {
                lbltodate.Text = "Select To Date";
                lbltodate.Visible = true;
                return;
            }
            lblset.Visible = false;
            Panelhour.Visible = true;
            // ckhr2.Visible = true;
            btnok.Visible = true;
            btnok.Enabled = true;
            //string deg_code = GetFunction("select distinct degree_code from degree where course_id=" + ddldegree.SelectedItem.Value.ToString() + " and dept_code=" + ddlbranch.SelectedItem.Value.ToString() + "");

            string sqlstr = GetFunction("select No_of_hrs_per_day  from PeriodAttndSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedItem.Value.ToString() + "");
            int noofhours = 0;
            if (sqlstr != "")
                noofhours = Convert.ToInt32(sqlstr);
            ckhr2.Items.Clear();
            for (int p = 1; p <= noofhours; p++)
                ckhr2.Items.Add(p.ToString());

        }
        else
        {
            FpSpread2.Visible = false;
            Panelhour.Visible = false;
            pHeaderatendence.Visible = false;
            pBodyatendence.Visible = false;
            lblset.Text = "Select Manual Attendance and then proceed";
            lblset.Visible = true;
            //ckhr2.Visible = false;
            btnok.Enabled = false;

        }
    }

    protected void ckhr2_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblset.Text = string.Empty;
        if (cksubjectwise.Checked == false)
        {
            lblset.Visible = true;
            lblset.Text = "Select subjectwise and then proceed";
            Panelhour.Visible = false;
            ckmanual.Enabled = false;
            ddlsubject.Visible = false;
            FpSpread2.Visible = false;
            pBodyatendence.Visible = false;
            pHeaderatendence.Visible = false;
            return;
        }
        if (ddlsubject.SelectedIndex == 0)
        {
            lblset.Visible = true;
            lblset.Text = "Select the subject and then proceed";
            Panelhour.Visible = false;
            ckmanual.Enabled = false;
            btnok.Enabled = false;
            FpSpread2.Visible = false;
            pBodyatendence.Visible = false;
            pHeaderatendence.Visible = false;
            return;
        }
        if (ckmanual.Checked == false)
        {
            lblset.Visible = false;
            lblset.Text = "Select Manual Schedule and then proceed";
            Panelhour.Visible = false;
            btnok.Enabled = false;
            FpSpread2.Visible = false;
            pBodyatendence.Visible = false;
            pHeaderatendence.Visible = false;
            return;
        }
        if (txtFromDate.Text == "")
        {
            lblfromdate.Text = "Select From Date";
            lblfromdate.Visible = true;
            return;
        }
        if (TxtToDate.Text == "")
        {
            lbltodate.Text = "Select To Date";
            lbltodate.Visible = true;
            return;
        }
        int count = 0;
        //  tbhour.Text="Hours";
        for (int item = 0; item < ckhr2.Items.Count; item++)
        {
            if (ckhr2.Items[item].Selected == true)
            {
                ++count;
                // tbhour.Text=tbhour.Text+" "+ckhr2.Items[item].Text;
            }
        }
        // if (count == 0)
        // tbhour.Text = "Select the hour";
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        btnsliplist.Visible = false;//Added By Srinath 12/8/2013
        string date1 = string.Empty;
        string datefrom = string.Empty;
        lblfromdate.Visible = false;
        lbltodate.Visible = false;
        if (txtFromDate.Text == "")
        {
            lblfromdate.Text = "Select From Date";
            lblfromdate.Visible = true;
            return;
        }

        date1 = txtFromDate.Text.ToString();
        string[] split = date1.Split(new Char[] { '-' });
        datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();

        DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
        if (dt1 > DateTime.Today)
        {
            lblset.Visible = true;
            lblset.Text = "You can not mark attendance for the date greater than today";
            //txtFromDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
            return;
        }
        else
        {
            lblset.Visible = false;
        }
    }

    protected void TxtToDate_TextChanged(object sender, EventArgs e)
    {
        btnsliplist.Visible = false;//Added By Srinath 12/8/2013
        lbltodate.Visible = false;
        if (TxtToDate.Text == "")
        {
            lbltodate.Text = "Select To Date";
            lbltodate.Visible = true;
            return;
        }
        string date2 = string.Empty;
        string dateto = string.Empty;
        //int noofhours;
        lblset.Visible = false;
        date2 = TxtToDate.Text.ToString();
        string[] split1 = date2.Split(new Char[] { '-' });
        dateto = split1[1].ToString() + "-" + split1[0].ToString() + "-" + split1[2].ToString();

        DateTime dt2 = Convert.ToDateTime(dateto.ToString());
        if (dt2 > DateTime.Today)
        {
            lblset.Visible = true;
            lblset.Text = "You can not mark attendance for the date greater than today";
            // TxtToDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
            return;
        }
    }

    protected void btngoindividual_Click(object sender, EventArgs e)
    {
        try
        {
            string mark_ddl = string.Empty;
            string markothers_ddl = string.Empty;
            long strdate = 0;
            string[] strcomo;
            string date1 = string.Empty;
            string date2 = string.Empty;
            string datefrom;
            string dateto = string.Empty;
            mark_ddl = ddlmark.SelectedItem.ToString();
            date1 = txtFromDate.Text.ToString();
            string[] split = date1.Split(new Char[] { '-' });
            datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
            date2 = TxtToDate.Text.ToString();
            string[] split1 = date2.Split(new Char[] { '-' });
            dateto = split1[1].ToString() + "-" + split1[0].ToString() + "-" + split1[2].ToString();
            DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
            DateTime dt2 = Convert.ToDateTime(dateto.ToString());
            TimeSpan t = dt2.Subtract(dt1);
            markothers_ddl = ddlmarkothers.SelectedItem.ToString();
            long days = t.Days;
            if (days < 0)
            {
                lblset.Visible = true;
                lblset.Text = "To date should be greater than from date";
                return;
            }
            if (mark_ddl == markothers_ddl && ddlmark.SelectedIndex != 0 && ddlmarkothers.SelectedIndex != 0)
            {
                markdiff.Visible = true;
                return;
            }
            else
            {

                markdiff.Visible = false;
                Label10.Visible = false;
                lblinvalidreg.Visible = false;
                markdiff.Visible = false;
                //lbltreesave.Visible = false;
                lblregno.Visible = false;
                lblrunerror.Visible = false;
                lblinvalidreg.Visible = false;
                lblhrselect.Visible = false;
                lblmarkabs.Visible = false;
                lblother.Visible = false;
                string markabs = string.Empty;
                string markother = string.Empty;
                string mark = string.Empty;
                string markothers = string.Empty;
                mark = ddlmark.SelectedItem.ToString();
                markother = ddlmarkothers.SelectedItem.ToString();

                markothers = ddlmarkothers.SelectedIndex.ToString();
                markabs = ddlmark.SelectedIndex.ToString();
                string reg = string.Empty;
                reg = txtregno.Text;
                string running = string.Empty;
                running = txtrunning.Text;
                string hr_select = string.Empty;
                hr_select = Ckhour.SelectedValue;
                if (staffcode == null || staffcode == "")
                {

                    FpSpread2.Sheets[0].Visible = false;
                    if (reg != "")
                    {
                        if (running != "")
                        {
                            if (hr_select != "")
                            {
                                if (markabs != "0")
                                {
                                    if (markothers != "0")
                                    {
                                        if (RadioButtonList1.SelectedValue == "1" || RadioButtonList1.SelectedValue == "2" || RadioButtonList1.SelectedValue == "3")
                                        {
                                            Panelind.Visible = true;
                                            txtregno.Visible = true;
                                            txtrunning.Visible = true;

                                            lblset.Visible = false;
                                            Buttonsave.Visible = true;
                                            Buttonexit.Visible = false;
                                            FpSpread2.Sheets[0].Visible = true;
                                            // FpSpread2.Width = 850;
                                            FpSpread2.Sheets[0].RowCount = 0;
                                            FpSpread2.Sheets[0].ColumnCount = 0;
                                            FpSpread2.Sheets[0].ColumnCount = 5;
                                            FpSpread2.Sheets[0].Columns[0].Visible = true;
                                            FpSpread2.Sheets[0].Columns[1].Visible = true;
                                            FpSpread2.Sheets[0].Columns[2].Visible = true;
                                            FpSpread2.Sheets[0].Columns[3].Visible = true;

                                            if (Session["Rollflag"].ToString() != "0")
                                            {
                                                FpSpread2.Sheets[0].ColumnHeader.Columns[0].Visible = true;
                                            }
                                            if (Session["Regflag"].ToString() != "0")
                                            {
                                                FpSpread2.Sheets[0].ColumnHeader.Columns[1].Visible = true;
                                            }

                                            if (Session["Studflag"].ToString() != "0")
                                            {
                                                FpSpread2.Sheets[0].ColumnHeader.Columns[3].Visible = true;
                                            }



                                            //FpSpread2.Sheets[0].ColumnHeader.Rows[1].Font.Bold = true;
                                            string strsec;
                                            if (ddlsec.Text.ToString() == "All" || ddlsec.Text.ToString() == "")
                                            {
                                                strsec = string.Empty;
                                            }
                                            else
                                            {
                                                strsec = " and registration.sections='" + ddlsec.SelectedValue.ToString() + "'";

                                            }
                                            int roll_count = 0;
                                            string roll_no = string.Empty;
                                            string reg_running = string.Empty;
                                            reg_running = txtrunning.Text;
                                            string[] split_reg_running = reg_running.Split(new Char[] { ',' });
                                            for (int i_reg = 0; i_reg <= split_reg_running.GetUpperBound(0); i_reg++)
                                            {
                                                if (split_reg_running[i_reg] == "")
                                                    continue;
                                                roll_no = txtregno.Text + split_reg_running[i_reg];

                                                string absent_mark = string.Empty;
                                                if (RadioButtonList1.SelectedValue == "1")
                                                {
                                                    absent_mark = "  and roll_no like '%" + roll_no + "' ";
                                                    FpSpread2.Sheets[0].Columns[0].Visible = true;
                                                }
                                                if (RadioButtonList1.SelectedValue == "2")
                                                {
                                                    absent_mark = "  and reg_no like '%" + roll_no + "' ";
                                                    FpSpread2.Sheets[0].Columns[1].Visible = true;
                                                }
                                                if (RadioButtonList1.SelectedValue == "3")
                                                {
                                                    absent_mark = "  and roll_admit like '%" + roll_no + "' ";
                                                    FpSpread2.Sheets[0].Columns[2].Visible = true;
                                                }

                                                string sqlstr;
                                                mysql.Open();
                                                sqlstr = "select distinct registration.roll_no,registration.reg_no,registration.roll_admit,stud_name,stud_type from registration where degree_code=" + ddlbranch.SelectedValue.ToString() + absent_mark + strsec + "";

                                                SqlCommand cmd_indi = new SqlCommand(sqlstr, mysql);
                                                SqlDataReader reader_indi;
                                                reader_indi = cmd_indi.ExecuteReader();
                                                if (reader_indi.Read())
                                                {
                                                    if (roll_no.ToUpper() == reader_indi["roll_no"].ToString() || roll_no.ToLower() == reader_indi["roll_no"].ToString() || roll_no == reader_indi["roll_no"].ToString())
                                                    {

                                                        FpSpread2.Sheets[0].RowCount += 1;
                                                        roll_count = roll_count + 1;

                                                        string Rollno_rd = string.Empty;
                                                        string regno_rd = string.Empty;
                                                        string stud_name = string.Empty;
                                                        string stud_type = string.Empty;
                                                        string adm_no = string.Empty;

                                                        Rollno_rd = reader_indi[0].ToString();
                                                        regno_rd = reader_indi[1].ToString();
                                                        adm_no = reader_indi[2].ToString();
                                                        stud_name = reader_indi[3].ToString();
                                                        stud_type = reader_indi[4].ToString();

                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Rollno_rd.ToString();
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = regno_rd.ToString();
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = adm_no.ToString();
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = stud_name.ToString();
                                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = stud_type.ToString();
                                                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Height = 30;

                                                    }
                                                    else
                                                    {

                                                        lblinvalidreg.Visible = true;
                                                        lblinvalidreg.Text = "The Roll Number" + " '" + roll_no + "' " + "is invalid";
                                                        // FpSpread2.Width = 0;
                                                        //FpSpread2.Height = 0;
                                                        if (roll_count == 0)
                                                        {
                                                            Buttonsave.Visible = false;
                                                        }
                                                        else
                                                            Buttonsave.Visible = true;
                                                    }
                                                }
                                                string sqlstrq = "select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + "";
                                                string noofhours = GetFunction(sqlstrq);
                                                string numberhor = string.Empty;
                                                numberhor = noofhours;
                                                string str = string.Empty;
                                                str = txtFromDate.Text;


                                                if (noofhours.ToString() != "" && noofhours != "0")
                                                {


                                                    FarPoint.Web.Spread.ComboBoxCellType objintcell8 = new FarPoint.Web.Spread.ComboBoxCellType();
                                                    //string[] strcomo1 = new string[] { "Select for All ", " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" };
                                                    //strcomo = new string[] { " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" };//---------21/6/12 PRABHA
                                                    //---------------------------------load rights                      
                                                    string[] strcomo1 = new string[20];
                                                    strcomo = new string[20];
                                                    int i = 0, j = 0;
                                                    SqlCommand cmd = new SqlCommand();
                                                    if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                                                    {
                                                        cmd.CommandText = "select rights from  OD_Master_Setting where group_code=" + Session["group_code"].ToString() + "";
                                                    }
                                                    else
                                                    {
                                                        cmd.CommandText = "select rights from  OD_Master_Setting where usercode=" + Session["UserCode"].ToString() + "";
                                                    }


                                                    cmd.Connection = con;
                                                    con.Close();
                                                    con.Open();
                                                    SqlDataReader dr_rights_od = cmd.ExecuteReader();
                                                    if (dr_rights_od.HasRows)
                                                    {
                                                        while (dr_rights_od.Read())
                                                        {

                                                            string od_rights = string.Empty;
                                                            Hashtable od_has = new Hashtable();

                                                            od_rights = dr_rights_od["rights"].ToString();

                                                            if (od_rights != string.Empty)
                                                            {
                                                                string[] split_od_rights = od_rights.Split(',');

                                                                strcomo = new string[split_od_rights.GetUpperBound(0) + 1];
                                                                strcomo1 = new string[split_od_rights.GetUpperBound(0) + 2];
                                                                strcomo1[j++] = "Select for All ";
                                                                for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                                                                {

                                                                    strcomo[i++] = split_od_rights[od_temp].ToString();
                                                                    strcomo1[j++] = split_od_rights[od_temp].ToString();
                                                                }

                                                            }
                                                            else
                                                            {
                                                                strcomo[0] = string.Empty;
                                                                strcomo[1] = "P";
                                                                strcomo[2] = "A";
                                                                strcomo[3] = "OD";
                                                                strcomo[4] = "SOD";
                                                                strcomo[5] = "ML";
                                                                strcomo[6] = "NSS";
                                                                strcomo[7] = "L";
                                                                strcomo[8] = "NCC";
                                                                strcomo[9] = "HS";
                                                                strcomo[10] = "PP";
                                                                strcomo[11] = "SYOD";
                                                                strcomo[12] = "COD";
                                                                strcomo[13] = "OOD";
                                                                strcomo[14] = "LA";


                                                                strcomo1[0] = "Select for All";
                                                                strcomo1[1] = string.Empty;
                                                                strcomo1[2] = "P";
                                                                strcomo1[3] = "A";
                                                                strcomo1[4] = "OD";
                                                                strcomo1[5] = "SOD";
                                                                strcomo1[6] = "ML";
                                                                strcomo1[7] = "NSS";
                                                                strcomo1[8] = "L";
                                                                strcomo1[9] = "NCC";
                                                                strcomo1[10] = "HS";
                                                                strcomo1[11] = "PP";
                                                                strcomo1[12] = "SYOD";
                                                                strcomo1[13] = "COD";
                                                                strcomo1[14] = "OOD";
                                                                strcomo1[15] = "LA";
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        strcomo[0] = string.Empty;
                                                        strcomo[1] = "P";
                                                        strcomo[2] = "A";
                                                        strcomo[3] = "OD";
                                                        strcomo[4] = "SOD";
                                                        strcomo[5] = "ML";
                                                        strcomo[6] = "NSS";
                                                        strcomo[7] = "L";
                                                        strcomo[8] = "NCC";
                                                        strcomo[9] = "HS";
                                                        strcomo[10] = "PP";
                                                        strcomo[11] = "SYOD";
                                                        strcomo[12] = "COD";
                                                        strcomo[13] = "OOD";
                                                        strcomo[14] = "LA";


                                                        strcomo1[0] = "Select for All";
                                                        strcomo1[1] = string.Empty;
                                                        strcomo1[2] = "P";
                                                        strcomo1[3] = "A";
                                                        strcomo1[4] = "OD";
                                                        strcomo1[5] = "SOD";
                                                        strcomo1[6] = "ML";
                                                        strcomo1[7] = "NSS";
                                                        strcomo1[8] = "L";
                                                        strcomo1[9] = "NCC";
                                                        strcomo1[10] = "HS";
                                                        strcomo1[11] = "PP";
                                                        strcomo1[12] = "SYOD";
                                                        strcomo1[13] = "COD";
                                                        strcomo1[14] = "OOD";
                                                        strcomo1[15] = "LA";
                                                    }

                                                    //---------------------------
                                                    objintcell8 = new FarPoint.Web.Spread.ComboBoxCellType(strcomo1);
                                                    objintcell8.ShowButton = true;
                                                    objintcell8.AutoPostBack = true;
                                                    objintcell8.UseValue = true;
                                                    // FpSpread2.ActiveSheetView.Cells[0, 5].CellType = objintcell;
                                                    FpSpread2.SaveChanges();

                                                    FarPoint.Web.Spread.ComboBoxCellType objcom = new FarPoint.Web.Spread.ComboBoxCellType();
                                                    objcom = new FarPoint.Web.Spread.ComboBoxCellType(strcomo);

                                                    objcom.AutoPostBack = true;

                                                    //       objcom.UseValue = true;

                                                    if (days >= 0)
                                                    {
                                                        string[] differdays = new string[days];


                                                        if (Convert.ToInt32(noofhours.ToString()) != 0)  // First Date
                                                        {
                                                            int count = 0;
                                                            for (int ini_loop = 1; ini_loop <= Convert.ToInt32(noofhours.ToString()); ini_loop++)
                                                            {
                                                                string ck_hr = string.Empty;
                                                                ck_hr = Ckhour.Items[ini_loop - 1].Text;
                                                                int ini;
                                                                string ini_str = string.Empty;

                                                                ini = ini_loop;
                                                                ini_str = Convert.ToString(ini);
                                                                if (ck_hr == ini_str && Ckhour.Items[ini_loop - 1].Selected == true)
                                                                {

                                                                    count = count + 1;
                                                                    FpSpread2.Sheets[0].SheetCorner.RowCount = 2; //Set Date
                                                                    FpSpread2.Sheets[0].ColumnCount = FpSpread2.Sheets[0].ColumnCount + 1;
                                                                    // FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].Width = 40;
                                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = txtFromDate.Text.ToString();

                                                                    //FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, Convert.ToInt32(noofhours.ToString()));

                                                                    FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ini_loop);
                                                                    //FpSpread2.Sheets[0].Columns[FpSpread2.Sheets[0].ColumnCount - 1].CellType = objintcell;   //Set Hour
                                                                    //FpSpread2.ActiveSheetView.Columns[FpSpread2.Sheets[0].ColumnCount - 1].CellType = objcom;


                                                                }
                                                            }
                                                            if (count != 0)
                                                                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, count);
                                                        }

                                                    }


                                                }



                                                //----------------------------Reterive the saved values

                                                int Att_mark_row;
                                                int Att_mark_column;
                                                string str_Date;
                                                string str_day;
                                                string Atmonth;
                                                string Atyear;
                                                //long str_date;
                                                //string Att_str_hour;
                                                string rollno_Att = string.Empty;
                                                string Att_dcolumn = string.Empty;
                                                string Att_strqueryst = string.Empty;
                                                string Att_Markvalue;
                                                string Att_Mark1;

                                                int temp = 0;

                                                for (Att_mark_row = 1; Att_mark_row <= FpSpread2.Sheets[0].RowCount - 1; Att_mark_row++)
                                                {


                                                    for (Att_mark_column = 5; Att_mark_column <= FpSpread2.Sheets[0].ColumnCount - 1; Att_mark_column++)
                                                    {
                                                        str_Date = FpSpread2.Sheets[0].ColumnHeader.Cells[0, Att_mark_column].Text;
                                                        rollno_Att = FpSpread2.Sheets[0].Cells[Att_mark_row, 0].Text.ToString();
                                                        string[] split_d = str_Date.Split(new Char[] { '-' });
                                                        str_day = split_d[0].ToString();
                                                        Atmonth = split_d[1].ToString();
                                                        Atyear = split_d[2].ToString();
                                                        strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                                                        int Att_hour;
                                                        Att_hour = Convert.ToInt32(FpSpread2.Sheets[0].ColumnHeader.Cells[1, Att_mark_column].Text);
                                                        Att_dcolumn = "d" + str_day + "d" + Att_hour;

                                                        Att_strqueryst = "select " + Att_dcolumn + " from Attendance where Roll_no='" + rollno_Att.ToString() + "' and month_year=" + strdate.ToString() + "";

                                                        Att_Markvalue = GetFunction(Att_strqueryst);
                                                        if (Att_Markvalue != null)
                                                            temp = temp + 1;
                                                        //Att_Mark1 = Attmark(Att_Markvalue);
                                                        Att_Mark1 = Attmark(ddlmark.SelectedItem.Text.ToString());
                                                        //FpSpread2.Sheets[0].SetValue(Att_mark_row, Att_mark_column, Att_Markvalue.ToString());
                                                        //FpSpread2.Sheets[0].SetText(Att_mark_row, Att_mark_column, Att_Mark1.ToString());
                                                        FpSpread2.Sheets[0].SetValue(Att_mark_row, Att_mark_column, Att_Mark1);
                                                        FpSpread2.Sheets[0].SetText(Att_mark_row, Att_mark_column, ddlmark.SelectedItem.Text.ToString());



                                                    }
                                                }


                                                // FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 2);//Hidden by Srinath 17/8/2013
                                                //  FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                                                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                                                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                                                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                                                FpSpread2.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Register No";
                                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
                                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
                                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student Type";
                                                FpSpread2.Sheets[0].Columns[0].Width = 50;
                                                FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                                                FpSpread2.Sheets[0].Columns[1].CellType = textcel_type;
                                                FpSpread2.Sheets[0].Columns[2].CellType = textcel_type;
                                                FpSpread2.Sheets[0].Columns[0].CellType = textcel_type;

                                                string[] strcomo4 = new string[20];
                                                FarPoint.Web.Spread.ComboBoxCellType objintcell4 = new FarPoint.Web.Spread.ComboBoxCellType();
                                                // strcomo4 = new string[] {  " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD" };
                                                //---------------------------------load rights                      

                                                int ii = 0;
                                                SqlCommand cmdd = new SqlCommand();
                                                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                                                {
                                                    cmdd.CommandText = "select rights from  OD_Master_Setting where group_code=" + Session["group_code"].ToString() + "";
                                                }
                                                else
                                                {
                                                    cmdd.CommandText = "select rights from  OD_Master_Setting where usercode=" + Session["UserCode"].ToString() + "";
                                                }

                                                cmdd.Connection = con;
                                                con.Close();
                                                con.Open();
                                                SqlDataReader dr_rights_od_od = cmdd.ExecuteReader();
                                                if (dr_rights_od_od.HasRows)
                                                {
                                                    while (dr_rights_od_od.Read())
                                                    {

                                                        string od_rights = string.Empty;
                                                        Hashtable od_has = new Hashtable();

                                                        od_rights = dr_rights_od_od["rights"].ToString();

                                                        if (od_rights != string.Empty)
                                                        {
                                                            string[] split_od_rights = od_rights.Split(',');

                                                            strcomo4 = new string[split_od_rights.GetUpperBound(0) + 1];
                                                            for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                                                            {

                                                                strcomo4[ii++] = split_od_rights[od_temp].ToString();
                                                            }

                                                        }
                                                        else
                                                        {

                                                            strcomo4[0] = string.Empty;
                                                            strcomo4[1] = "P";
                                                            strcomo4[2] = "A";
                                                            strcomo4[3] = "OD";
                                                            strcomo4[4] = "SOD";
                                                            strcomo4[5] = "ML";
                                                            strcomo4[6] = "NSS";
                                                            strcomo4[7] = "L";
                                                            strcomo4[8] = "NCC";
                                                            strcomo4[9] = "HS";
                                                            strcomo4[10] = "PP";
                                                            strcomo4[11] = "SYOD";
                                                            strcomo4[12] = "COD";
                                                            strcomo4[13] = "OOD";
                                                            strcomo4[14] = "LA";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    strcomo4[0] = string.Empty;
                                                    strcomo4[1] = "P";
                                                    strcomo4[2] = "A";
                                                    strcomo4[3] = "OD";
                                                    strcomo4[4] = "SOD";
                                                    strcomo4[5] = "ML";
                                                    strcomo4[6] = "NSS";
                                                    strcomo4[7] = "L";
                                                    strcomo4[8] = "NCC";
                                                    strcomo4[9] = "HS";
                                                    strcomo4[10] = "PP";
                                                    strcomo4[11] = "SYOD";
                                                    strcomo4[12] = "COD";
                                                    strcomo4[13] = "OOD";
                                                    strcomo4[14] = "LA";
                                                }

                                                //---------------------------
                                                objintcell4 = new FarPoint.Web.Spread.ComboBoxCellType(strcomo4);
                                                objintcell4.ShowButton = true;
                                                objintcell4.AutoPostBack = true;
                                                objintcell4.UseValue = true;
                                                FpSpread2.ActiveSheetView.Columns[0].CellType = objintcell4;
                                                FpSpread2.SaveChanges();
                                                FpSpread2.Sheets[0].Columns[0].BackColor = Color.MistyRose;



                                            }
                                        }
                                        else
                                        {
                                            lblset.Text = "Select roll number or registration number  or admission number from settings";
                                            lblset.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        lblother.Visible = true;
                                    }

                                }
                                else
                                {
                                    lblmarkabs.Visible = true;
                                }
                            }
                            else
                            {
                                lblhrselect.Visible = true;
                            }

                        }
                        else
                        {
                            lblrunerror.Visible = true;


                        }
                    }
                    else
                    {
                        lblregno.Visible = true;
                        lblregno.Text = "Enter Static Part";
                    }
                }
            }
            Panelind.Visible = true;
        }
        catch (Exception ex)
        {
            lblregno.Visible = true;
            lblregno.Text = ex.ToString();
        }
    }

    protected void ddlmarkothers_SelectedIndexChanged(object sender, EventArgs e)
    {
        Label10.Visible = false;
        lblother.Visible = false;

        string mark = string.Empty;
        string markothers = string.Empty;
        mark = ddlmark.SelectedItem.ToString();
        markothers = ddlmarkothers.SelectedItem.ToString();
        if (mark == markothers && ddlmark.SelectedIndex != 0 && ddlmarkothers.SelectedIndex != 0)
        {
            markdiff.Visible = true;
            Label10.Visible = false;
        }
        else
        {
            markdiff.Visible = false;

        }

        ddlmarkothers.Focus();

    }

    protected void txtregno_TextChanged(object sender, EventArgs e)
    {
        lblregno.Visible = false;
    }

    protected void txtrunning_TextChanged(object sender, EventArgs e)
    {
        lblrunerror.Visible = false;
    }

    protected void Ckhour_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void Labelatend_Click(object sender, EventArgs e)
    {
        //FpSpread2.Sheets[0].AutoPostBack = false;
        // Session["height"] = 500;

        ////string h = Session["height"].ToString();
        ////string w = Session["width"].ToString();
        ////FpSpread2.Height = 
        //FpSpread2.Width = Convert.ToInt32(w);

    }

    protected void btnsliplist_Click(object sender, EventArgs e)
    {
        try
        {
            lblnorecpending.Visible = false;
            Hashtable hatvalue = new Hashtable();
            lblset.Visible = false;
            bool altschedule = false;
            bool check_unmark = false;
            pnl_sliplist.Visible = false;
            spread_sliplist.SheetCorner.Columns[0].Visible = false;
            spread_sliplist.Sheets[0].RowCount = 0;
            spread_sliplist.Sheets[0].ColumnCount = 0;
            spread_sliplist.Sheets[0].ColumnCount = 5;
            spread_sliplist.Sheets[0].RowHeader.Visible = false;
            spread_sliplist.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spread_sliplist.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
            spread_sliplist.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Hour";
            spread_sliplist.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Name";
            spread_sliplist.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject";

            spread_sliplist.Sheets[0].ColumnHeader.Columns[0].Width = 50;
            spread_sliplist.Sheets[0].ColumnHeader.Columns[1].Width = 100;
            spread_sliplist.Sheets[0].ColumnHeader.Columns[2].Width = 50;
            spread_sliplist.Sheets[0].ColumnHeader.Columns[3].Width = 200;
            spread_sliplist.Sheets[0].ColumnHeader.Columns[4].Width = 300;

            //int ac = 0, ar = 0;
            //string active_tag =string.Empty;
            //int rowcnt = 0;
            con.Close();
            con.Open();
            string sectionsvalue = string.Empty;
            if (ddlsec.Text == "")
            {
                strsec = string.Empty;
            }
            else
            {

                if (ddlsec.SelectedItem.ToString() == "All" || ddlsec.SelectedItem.ToString() == null || ddlsec.SelectedItem.ToString() == " " || ddlsec.SelectedItem.ToString() == "")//modifiefd by annyutha dec18
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " and sections='" + ddlsec.SelectedItem.ToString() + "'";
                    sectionsvalue = ddlsec.SelectedItem.ToString();
                }
            }
            DataSet dsstuatt = new DataSet();
            no_of_hrs = GetFunction("Select No_of_hrs_per_day from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + "");
            no_hrs = Convert.ToInt16(no_of_hrs);
            sch_order = GetFunction("Select schorder from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + "");
            no_days = GetFunction("Select nodays from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + "");
            nodays = Convert.ToInt16(no_days);
            startdate = GetFunction("select start_date from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " ");
            starting_dayorder = GetFunction("select isnull(starting_dayorder,1) as starting_dayorder from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " ");
            string enddate = GetFunction("select end_date from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " ");

            DateTime dt = DateTime.Now;
            if (enddate != null && enddate != "0" && enddate.Trim() != "0")
            {
                dt = Convert.ToDateTime(enddate);
            }

            DateTime dtstart = DateTime.Now;
            if (startdate != null && startdate != "0" && startdate.Trim() != "0")
            {
                dtstart = Convert.ToDateTime(startdate);
            }
            if (starting_dayorder == "")
            {
                starting_dayorder = "1";
            }
            bool valuset = false;
            bool semflag = false;

            for (colcnt = 5; colcnt < FpSpread2.Sheets[0].ColumnCount; colcnt++)
            {
                DateTime temp_date = new DateTime();
                date_txt = FpSpread2.Sheets[0].ColumnHeader.Cells[1, colcnt].Tag.ToString();
                temp_hr = Convert.ToInt16(FpSpread2.Sheets[0].ColumnHeader.Cells[1, colcnt].Text.ToString());

                temp_date = Convert.ToDateTime(date_txt);
                if (dt >= temp_date && dtstart <= temp_date)
                {
                    ds.Clear();
                    string strqu = "select top 1 * from semester_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlsem.SelectedItem.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and FromDate <='" + temp_date + "' " + strsec + " order by FromDate Desc";
                    ds = dacces2.select_method_wo_parameter(strqu, "Text");

                    DataSet ds_alter = new DataSet();
                    string sqlalter = "select * from alternate_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlsem.SelectedItem.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and FromDate ='" + temp_date + "' " + strsec + "";
                    ds_alter = dacces2.select_method(sqlalter, hat, "txt");

                    if (no_hrs > 0)
                    {
                        if (sch_order != "0")
                        {
                            srt_day = temp_date.ToString("ddd");
                        }
                        else
                        {
                            string[] tmpdate = temp_date.ToString().Split(new char[] { ' ' });
                            string currdate = tmpdate[0].ToString();
                            string[] tmpdate1 = startdate.ToString().Split(new char[] { ' ' });
                            string startdate1 = tmpdate1[0].ToString();

                            // srt_day = findday(currdate.ToString(), startdate.ToString(), no_days.ToString(), starting_dayorder.ToString()); srt_day = findday(currdate.ToString(), startdate.ToString(), no_days.ToString(), starting_dayorder.ToString());
                            srt_day = dacces2.findday(currdate.ToString(), ddlbranch.SelectedValue.ToString(), ddlsem.SelectedItem.ToString(), ddlbatch.Text.ToString(), startdate.ToString(), no_days.ToString(), starting_dayorder.ToString());

                        }
                    }
                    //-------------------------------

                    Att_dcolumn = "d" + Convert.ToInt16(temp_date.ToString("dd")) + "d" + temp_hr;
                    strdate = ((Convert.ToInt16(temp_date.ToString("yyyy"))) * 12) + (Convert.ToInt16(temp_date.ToString("MM")));

                    //Added by Srinath 17/8/2013
                    //bool checkattendance = false;
                    //string strsturecordavaila = GetFunction("select count(*) from registration,attendance where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and current_semester=" + ddlsem.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' " + strsec + " and registration.roll_no=attendance.roll_no ");


                    //string stravilablestu = GetFunction("select count(*) from registration where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and current_semester=" + ddlsem.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar'  " + strsec + "");
                    //if (strsturecordavaila != stravilablestu)
                    //{
                    //    checkattendance = true;
                    //    Att_strqueryst = "1";
                    //}

                    //if (checkattendance == false)
                    //{
                    //    if (strsturecordavaila != "" && strsturecordavaila != "0" && strsturecordavaila != null)
                    //    {
                    //        Att_strqueryst = GetFunction("select count(*) from registration,attendance where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and current_semester=" + ddlsem.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' " + strsec + " and registration.roll_no=attendance.roll_no and (" + Att_dcolumn + " is null or " + Att_dcolumn + "=0 or " + Att_dcolumn + "='' or " + Att_dcolumn + " is NULL )");
                    //    }
                    //    else
                    //    {
                    //        Att_strqueryst = "1";
                    //    }
                    //}


                    bool alterhourflag = false;
                    if (ds_alter.Tables[0].Rows.Count > 0) //Alternate schedule
                    {
                        if (ds_alter.Tables[0].Rows[0][srt_day + temp_hr].ToString() != "" || ds_alter.Tables[0].Rows[0][srt_day + temp_hr].ToString() != null)
                        {
                            string sem_sched1 = ds_alter.Tables[0].Rows[0][srt_day + temp_hr].ToString();
                            if (sem_sched1 != "")
                            {
                                alterhourflag = true;
                                string[] sem_sched_split1 = sem_sched1.Split(';');
                                for (int temp_sem_split1 = 0; temp_sem_split1 <= sem_sched_split1.GetUpperBound(0); temp_sem_split1++)
                                {
                                    string[] sem_sched_split_split1 = sem_sched_split1[temp_sem_split1].Split('-');
                                    if (sem_sched_split_split1.GetUpperBound(0) >= 2)
                                    {
                                        subject_no = sem_sched_split_split1[0].ToString();
                                        altschedule = false;
                                        string check_lab1 = GetFunction("select lab From sub_sem where subtype_no=(select subtype_no from subject where  subject_no='" + subject_no + "')");
                                        if (check_lab1.Trim().ToLower() == "true" || check_lab1.Trim() == "1")
                                        {
                                            hatvalue.Clear();
                                            hatvalue.Add("batch_year", ddlbatch.SelectedValue.ToString());
                                            hatvalue.Add("degree_code", ddlbranch.SelectedValue.ToString());
                                            hatvalue.Add("sem", ddlsem.SelectedItem.ToString());
                                            hatvalue.Add("sections", sectionsvalue);
                                            hatvalue.Add("month_year", strdate);
                                            hatvalue.Add("date", temp_date);
                                            hatvalue.Add("subject_no", subject_no);
                                            hatvalue.Add("day", srt_day);
                                            hatvalue.Add("hour", temp_hr);
                                            dsstuatt.Reset();
                                            dsstuatt.Dispose();
                                            dsstuatt = dacces2.select_method("sp_stu_atten_month_check_lab_alter", hatvalue, "sp");
                                            Att_strqueryst = dsstuatt.Tables[0].Rows[0]["stucount"].ToString();
                                            if (int.Parse(Att_strqueryst) > 0)
                                            {
                                                hatvalue.Clear();
                                                hatvalue.Add("columnname", Att_dcolumn);
                                                hatvalue.Add("batch_year", ddlbatch.SelectedValue.ToString());
                                                hatvalue.Add("degree_code", ddlbranch.SelectedValue.ToString());
                                                hatvalue.Add("sem", ddlsem.SelectedItem.ToString());
                                                hatvalue.Add("sections", sectionsvalue);
                                                hatvalue.Add("month_year", strdate);
                                                hatvalue.Add("date", temp_date);
                                                hatvalue.Add("subject_no", subject_no);
                                                hatvalue.Add("day", srt_day);
                                                hatvalue.Add("hour", temp_hr);
                                                dsstuatt.Reset();
                                                dsstuatt.Dispose();
                                                dsstuatt = dacces2.select_method("sp_stu_atten_day_check_lab_alter", hatvalue, "sp");
                                                if (Att_strqueryst == dsstuatt.Tables[0].Rows[0]["stucount"].ToString())
                                                {
                                                    altschedule = true;
                                                }
                                                else
                                                {
                                                    altschedule = false;
                                                }
                                            }
                                            else
                                            {
                                                altschedule = false;
                                            }
                                        }
                                        else
                                        {
                                            hatvalue.Clear();
                                            hatvalue.Add("batch_year", ddlbatch.SelectedValue.ToString());
                                            hatvalue.Add("degree_code", ddlbranch.SelectedValue.ToString());
                                            hatvalue.Add("sem", ddlsem.SelectedItem.ToString());
                                            hatvalue.Add("sections", sectionsvalue);
                                            hatvalue.Add("month_year", strdate);
                                            hatvalue.Add("date", temp_date);
                                            hatvalue.Add("subject_no", subject_no);
                                            dsstuatt.Reset();
                                            dsstuatt.Dispose();
                                            dsstuatt = dacces2.select_method("sp_stu_atten_month_check", hatvalue, "sp");
                                            Att_strqueryst = dsstuatt.Tables[0].Rows[0]["stucount"].ToString();
                                            if (int.Parse(Att_strqueryst) > 0)
                                            {
                                                hatvalue.Clear();
                                                hatvalue.Add("columnname", Att_dcolumn);
                                                hatvalue.Add("batch_year", ddlbatch.SelectedValue.ToString());
                                                hatvalue.Add("degree_code", ddlbranch.SelectedValue.ToString());
                                                hatvalue.Add("sem", ddlsem.SelectedItem.ToString());
                                                hatvalue.Add("sections", sectionsvalue);
                                                hatvalue.Add("month_year", strdate);
                                                hatvalue.Add("date", temp_date);
                                                hatvalue.Add("subject_no", subject_no);
                                                dsstuatt.Reset();
                                                dsstuatt.Dispose();
                                                dsstuatt = dacces2.select_method("sp_stu_atten_day_check", hatvalue, "sp");
                                                if (Att_strqueryst == dsstuatt.Tables[0].Rows[0]["stucount"].ToString())
                                                {
                                                    altschedule = true;
                                                }
                                                else
                                                {
                                                    altschedule = false;
                                                }

                                            }
                                            else
                                            {
                                                altschedule = false;
                                            }


                                        }
                                    }
                                    if (altschedule == false)
                                    {
                                        valuset = true;
                                        string subname = GetFunction("select subject_name from subject where subject_no='" + subject_no + "'");
                                        for (int st = 1; st < sem_sched_split_split1.GetUpperBound(0); st++)
                                        {
                                            if (sem_sched_split_split1[st].ToString().Trim() != "")
                                            {
                                                spread_sliplist.Sheets[0].RowCount++;
                                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 0].Text = spread_sliplist.Sheets[0].RowCount.ToString();
                                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 1].Text = Convert.ToDateTime(date_txt).ToString("dd/MM/yyyy");
                                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 2].Text = FpSpread2.Sheets[0].ColumnHeader.Cells[1, colcnt].Text;
                                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 3].Text = GetFunction("select staff_name from staffmaster where staff_code='" + sem_sched_split_split1[st].ToString() + "'");
                                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 4].Text = subname;
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                    if (alterhourflag == false)
                    {
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            string timetable = ds.Tables[0].Rows[0]["ttname"].ToString(); // Added by jairam 10-12-2014

                            sem_sched = ds.Tables[0].Rows[0][srt_day + temp_hr].ToString();
                            if (sem_sched != "")
                            {
                                semflag = true;
                                string[] sem_sched_split = sem_sched.Split(';');
                                for (int temp_sem_split = 0; temp_sem_split <= sem_sched_split.GetUpperBound(0); temp_sem_split++)
                                {
                                    string[] sem_sched_split_split = sem_sched_split[temp_sem_split].Split('-');
                                    if (sem_sched_split_split.GetUpperBound(0) >= 2)
                                    {
                                        subject_no = sem_sched_split_split[0].ToString();
                                        altschedule = false;
                                        string check_lab1 = GetFunction("select lab From sub_sem where subtype_no=(select subtype_no from subject where  subject_no='" + sem_sched_split_split[0] + "')");
                                        if (check_lab1.Trim().ToLower() == "true" || check_lab1.Trim() == "1")
                                        {
                                            hatvalue.Clear();
                                            hatvalue.Add("batch_year", ddlbatch.SelectedValue.ToString());
                                            hatvalue.Add("degree_code", ddlbranch.SelectedValue.ToString());
                                            hatvalue.Add("sem", ddlsem.SelectedItem.ToString());
                                            hatvalue.Add("sections", sectionsvalue);
                                            hatvalue.Add("month_year", strdate);
                                            hatvalue.Add("date", temp_date);
                                            hatvalue.Add("subject_no", subject_no);
                                            hatvalue.Add("day", srt_day);
                                            hatvalue.Add("hour", temp_hr);
                                            hatvalue.Add("ttmane", timetable);
                                            dsstuatt.Dispose();
                                            dsstuatt.Reset();
                                            dsstuatt = dacces2.select_method("sp_stu_atten_month_check_lab", hatvalue, "sp");
                                            Att_strqueryst = dsstuatt.Tables[0].Rows[0]["stucount"].ToString();
                                            if (Convert.ToInt32(Att_strqueryst) > 0)
                                            {
                                                hatvalue.Clear();
                                                hatvalue.Add("columnname", Att_dcolumn);
                                                hatvalue.Add("batch_year", ddlbatch.SelectedValue.ToString());
                                                hatvalue.Add("degree_code", ddlbranch.SelectedValue.ToString());
                                                hatvalue.Add("sem", ddlsem.SelectedItem.ToString());
                                                hatvalue.Add("sections", sectionsvalue);
                                                hatvalue.Add("month_year", strdate);
                                                hatvalue.Add("date", temp_date);
                                                hatvalue.Add("subject_no", subject_no);
                                                hatvalue.Add("day", srt_day);
                                                hatvalue.Add("hour", temp_hr);
                                                hatvalue.Add("ttmane", timetable);
                                                dsstuatt.Reset();
                                                dsstuatt.Dispose();
                                                dsstuatt = dacces2.select_method("sp_stu_atten_day_check_lab", hatvalue, "sp");
                                                if (Att_strqueryst == dsstuatt.Tables[0].Rows[0]["stucount"].ToString())
                                                {
                                                    altschedule = true;
                                                }
                                                else
                                                {
                                                    altschedule = false;
                                                }
                                            }
                                            else
                                            {
                                                altschedule = false;
                                            }

                                        }
                                        else
                                        {

                                            hatvalue.Clear();
                                            hatvalue.Add("batch_year", ddlbatch.SelectedValue.ToString());
                                            hatvalue.Add("degree_code", ddlbranch.SelectedValue.ToString());
                                            hatvalue.Add("sem", ddlsem.SelectedItem.ToString());
                                            hatvalue.Add("sections", sectionsvalue);
                                            hatvalue.Add("month_year", strdate);
                                            hatvalue.Add("date", temp_date);
                                            hatvalue.Add("subject_no", subject_no);
                                            dsstuatt.Reset();
                                            dsstuatt.Dispose();
                                            dsstuatt = dacces2.select_method("sp_stu_atten_month_check", hatvalue, "sp");
                                            Att_strqueryst = dsstuatt.Tables[0].Rows[0]["stucount"].ToString();
                                            if (int.Parse(Att_strqueryst) > 0)
                                            {
                                                hatvalue.Clear();
                                                hatvalue.Add("columnname", Att_dcolumn);
                                                hatvalue.Add("batch_year", ddlbatch.SelectedValue.ToString());
                                                hatvalue.Add("degree_code", ddlbranch.SelectedValue.ToString());
                                                hatvalue.Add("sem", ddlsem.SelectedItem.ToString());
                                                hatvalue.Add("sections", sectionsvalue);
                                                hatvalue.Add("month_year", strdate);
                                                hatvalue.Add("date", temp_date);
                                                hatvalue.Add("subject_no", subject_no);
                                                dsstuatt.Reset();
                                                dsstuatt.Dispose();
                                                dsstuatt = dacces2.select_method("sp_stu_atten_day_check", hatvalue, "sp");
                                                if (Att_strqueryst == dsstuatt.Tables[0].Rows[0]["stucount"].ToString())
                                                {
                                                    altschedule = true;
                                                }
                                                else
                                                {
                                                    altschedule = false;
                                                }

                                            }
                                            else
                                            {
                                                altschedule = false;
                                            }


                                        }
                                    }
                                    if (altschedule == false)
                                    {
                                        valuset = true;
                                        string subname = GetFunction("select subject_name from subject where subject_no='" + subject_no + "'");
                                        for (int st = 1; st < sem_sched_split_split.GetUpperBound(0); st++)
                                        {
                                            if (sem_sched_split_split[st].ToString().Trim() != "")
                                            {
                                                spread_sliplist.Sheets[0].RowCount++;
                                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 0].Text = spread_sliplist.Sheets[0].RowCount.ToString();
                                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 1].Text = Convert.ToDateTime(date_txt).ToString("dd/MM/yyyy");
                                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 2].Text = FpSpread2.Sheets[0].ColumnHeader.Cells[1, colcnt].Text;
                                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 3].Text = GetFunction("select staff_name from staffmaster where staff_code='" + sem_sched_split_split[st].ToString() + "'");
                                                spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 4].Text = subname;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //  }
                    //            if (int.Parse(Att_strqueryst) > 0)
                    //{
                    //altschedule = false;
                    // ds.Clear();
                    //cmd_sem_shed = new SqlCommand("select top 1 * from semester_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlsem.SelectedItem.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and FromDate <='" + temp_date + "' " + strsec + " order by FromDate Desc", con);
                    // SqlDataAdapter da = new SqlDataAdapter(cmd_sem_shed);
                    // da.Fill(ds);
                    //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    //{
                    //    sem_sched = ds.Tables[0].Rows[0][srt_day + temp_hr].ToString();
                    //    if (sem_sched != "")
                    //    {
                    //        string[] sem_sched_split = sem_sched.Split(';');
                    //        for (int temp_sem_split = 0; temp_sem_split <= sem_sched_split.GetUpperBound(0); temp_sem_split++)
                    //        {

                    //            string[] sem_sched_split_split = sem_sched_split[temp_sem_split].Split('-');
                    //            if (sem_sched_split_split.GetUpperBound(0) >= 2)
                    //            {
                    //                //Check alternate schedule=======================================
                    //              //  DataSet ds_alter = new DataSet();
                    //                ds_alter.Clear();
                    //                ds_alter.Dispose();
                    //                ds_alter.Reset();
                    //                 sqlalter = "select " + srt_day + temp_hr + " from alternate_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlsem.SelectedItem.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and FromDate ='" + temp_date + "' " + strsec + "";
                    //                ds_alter = dacces2.select_method(sqlalter, hat, "txt");
                    //                if (ds_alter.Tables[0].Rows.Count > 0) //Alternate schedule
                    //                {
                    //                    if (ds_alter.Tables[0].Rows[0][srt_day + temp_hr].ToString() != "" || ds_alter.Tables[0].Rows[0][srt_day + temp_hr].ToString() != null)
                    //                    {
                    //                        string sem_sched1 = ds_alter.Tables[0].Rows[0][srt_day + temp_hr].ToString();
                    //                        if (sem_sched1 != "")
                    //                        {
                    //                            altschedule = true;
                    //                            string[] sem_sched_split1 = sem_sched1.Split(';');
                    //                            for (int temp_sem_split1 = 0; temp_sem_split1 <= sem_sched_split1.GetUpperBound(0); temp_sem_split1++)
                    //                            {

                    //                                string[] sem_sched_split_split1 = sem_sched_split1[temp_sem_split1].Split('-');
                    //                                if (sem_sched_split_split1.GetUpperBound(0) >= 2)
                    //                                {
                    //                                    string subject_no1 = sem_sched_split_split1[0].ToString();
                    //                                    string check_lab1 = GetFunction("select subject_type From sub_sem where subtype_no=(select subtype_no from subject where  subject_no='" + subject_no1 + "')");
                    //                                    if (check_lab1 == "Practicals" || check_lab1 == "Practical")
                    //                                    {
                    //                                        practical_load(sem_sched_split_split1[1].ToString());
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        spread_sliplist.Sheets[0].RowCount++;
                    //                                        spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 0].Text = spread_sliplist.Sheets[0].RowCount.ToString();
                    //                                        spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 1].Text = Convert.ToDateTime(date_txt).ToString("dd/MM/yyyy");
                    //                                        spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 2].Text = FpSpread2.Sheets[0].ColumnHeader.Cells[1, colcnt].Text;
                    //                                        spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 3].Text = GetFunction("select staff_name from staffmaster where staff_code='" + sem_sched_split_split1[1].ToString() + "'");
                    //                                        spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 4].Text = GetFunction("select subject_name from subject where subject_no='" + subject_no1 + "'");
                    //                                    }
                    //                                }
                    //                            }
                    //                        }
                    //                        else
                    //                        {
                    //                            subject_no = sem_sched_split_split[0].ToString();
                    //                            hatvalue.Clear();
                    //                            hatvalue.Add("batch_year", ddlbatch.SelectedValue.ToString());
                    //                            hatvalue.Add("degree_code", ddlbranch.SelectedValue.ToString());
                    //                            hatvalue.Add("sem", ddlsem.SelectedItem.ToString());
                    //                            hatvalue.Add("sections", sectionsvalue);
                    //                            hatvalue.Add("month_year", strdate);
                    //                            hatvalue.Add("date", temp_date);
                    //                            hatvalue.Add("subject_no", subject_no);
                    //                            dsstuatt.Reset();
                    //                            dsstuatt.Dispose();
                    //                            dsstuatt = dacces2.select_method("sp_stu_atten_month_check", hatvalue, "Text");
                    //                            if (dsstuatt.Tables[0].Rows.Count > 0)
                    //                            {
                    //                                Att_strqueryst = dsstuatt.Tables[0].Rows[0]["stucount"].ToString();
                    //                                if (int.Parse(Att_strqueryst) > 0)
                    //                                {
                    //                                    hatvalue.Clear();
                    //                                    hatvalue.Add("batch_year", ddlbatch.SelectedValue.ToString());
                    //                                    hatvalue.Add("degree_code", ddlbranch.SelectedValue.ToString());
                    //                                    hatvalue.Add("sem", ddlsem.SelectedItem.ToString());
                    //                                    hatvalue.Add("sections", sectionsvalue);
                    //                                    hatvalue.Add("month_year", strdate);
                    //                                    hatvalue.Add("date", temp_date);
                    //                                    hatvalue.Add("subject_no", subject_no);
                    //                                    dsstuatt.Reset();
                    //                                    dsstuatt.Dispose();
                    //                                    dsstuatt = dacces2.select_method("sp_stu_atten_day_check", hatvalue, "sp");
                    //                                    if (dsstuatt.Tables[0].Rows.Count > 0)
                    //                                    {
                    //                                        if (Att_strqueryst == dsstuatt.Tables[0].Rows[0]["stucount"].ToString())
                    //                                        {
                    //                                            altschedule = true;
                    //                                        }
                    //                                        else
                    //                                        {
                    //                                            altschedule = false;
                    //                                        }
                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        altschedule = false;
                    //                                    }
                    //                                }
                    //                                else
                    //                                {
                    //                                    altschedule = false;
                    //                                }
                    //                            }
                    //                            else
                    //                            {
                    //                                altschedule = false;
                    //                            }
                    //                        }
                    //                    }
                    //                }

                    //                if (altschedule == false) //Normal Schedule
                    //                {
                    //                    subject_no = sem_sched_split_split[0].ToString();
                    //                    string check_lab = GetFunction("select subject_type From sub_sem where subtype_no=(select subtype_no from subject where  subject_no='" + subject_no + "')");
                    //                    if (check_lab == "Practicals" || check_lab == "Practical")
                    //                    {
                    //                        practical_load(sem_sched_split_split[1].ToString());
                    //                    }
                    //                    else
                    //                    {
                    //                        spread_sliplist.Sheets[0].RowCount++;
                    //                        spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 0].Text = spread_sliplist.Sheets[0].RowCount.ToString();
                    //                        spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 1].Text = Convert.ToDateTime(date_txt).ToString("dd/MM/yyyy");
                    //                        spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 2].Text = FpSpread2.Sheets[0].ColumnHeader.Cells[1, colcnt].Text;
                    //                        spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 3].Text = GetFunction("select staff_name from staffmaster where staff_code='" + sem_sched_split_split[1].ToString() + "'");
                    //                        spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 4].Text = GetFunction("select subject_name from subject where subject_no='" + subject_no + "'");
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //}
                }
            }

            pnl_sliplist.Height = (spread_sliplist.Sheets[0].RowCount * 200) + 500;
            pnl_sliplist.Width = 670;
            pnl_sliplist.Visible = true;
            Buttonupdate.Enabled = true;
            spread_sliplist.Visible = true;
            if (valuset == false)
            {
                if (semflag == false)
                {
                    lblnorecpending.Text = "Please Update Semester Information";
                }
                else
                {
                    lblnorecpending.Text = "Attendance Completed";
                }
                lblnorecpending.Visible = true;
                spread_sliplist.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblset.Visible = true;
            lblset.Text = ex.ToString();
        }
    }

    protected void exit_sliplist_Click(object sender, EventArgs e)
    {
        pnl_sliplist.Visible = false;
        btnsliplist.Enabled = true;
    }

    public void practical_load(string staff_code)
    {
        string lab_absent_count = string.Empty;
        lab_absent_count = GetFunction("select count(*) From attendance ,registration,subjectchooser,sub_sem,subject where month_year= " + strdate + " and degree_code =" + ddlbranch.SelectedValue.ToString() + " and current_semester = " + ddlsem.SelectedItem.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and registration.roll_no=attendance.roll_no " + strsec + " and batch in(select top 1 stu_batch from laballoc where subject_no=" + subject_no + "  and batch_year=" + ddlbatch.SelectedItem.ToString() + " and hour_value=" + temp_hr + " " + strsec + " and degree_code=" + ddlbranch.SelectedValue.ToString() + ")  and sub_sem.lab=1 and subjectchooser.subtype_no=sub_sem.subtype_no and  subjectchooser.semester = " + ddlsem.SelectedValue.ToString() + "  and subjectchooser.subject_no=subject.subject_no and  registration.roll_no=subjectchooser.roll_no and  registration.current_semester=subjectchooser.semester and subjectchooser.subject_no=" + subject_no + " " + strsec + "");
        if (int.Parse(lab_absent_count) > 0)
        {
            spread_sliplist.Sheets[0].RowCount++;
            spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 0].Text = spread_sliplist.Sheets[0].RowCount.ToString();
            spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 1].Text = Convert.ToDateTime(date_txt).ToString("dd/MM/yyyy");
            spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 2].Text = FpSpread2.Sheets[0].ColumnHeader.Cells[1, colcnt].Text;
            spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 3].Text = GetFunction("select staff_name from staffmaster where staff_code='" + staff_code.ToString() + "'");
            spread_sliplist.Sheets[0].Cells[(spread_sliplist.Sheets[0].RowCount - 1), 4].Text = GetFunction("select subject_name from subject where subject_no='" + subject_no + "'");

        }
    }

    //public string findday(string curday, string sdate, string no_days, string stastdayorder)
    //{
    //    int holiday = 0;
    //    if (no_days == "")
    //        return "";
    //    if (sdate != "")
    //    {
    //        //string[] sp_date = curday.Split(new Char[] { '/' });
    //        //string cur_date = sp_date[1].ToString() + "-" + sp_date[0].ToString() + "-" + sp_date[2].ToString();
    //        string cur_date = curday;
    //        DateTime dt1 = Convert.ToDateTime(sdate);
    //        DateTime dt2 = Convert.ToDateTime(cur_date);
    //        TimeSpan ts = dt2 - dt1;
    //        string query1 = "select count(*)as count from holidaystudents  where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "'";
    //        string holday = GetFunction(query1);
    //        if (holday != "")
    //            holiday = Convert.ToInt32(holday);
    //        int dif_days = ts.Days;
    //        int nodays = Convert.ToInt32(no_days);
    //        int order = (dif_days - holiday) % nodays;
    //        order = order + 1;

    //        //-----------------------------------------------------------     

    //        if (stastdayorder.ToString().Trim() != "")
    //        {
    //            if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
    //            {
    //                order = order + (Convert.ToInt16(stastdayorder) - 1);
    //                if (order == (nodays + 1))
    //                    order = 1;
    //                else if (order > nodays)
    //                    order = order % nodays;
    //            }
    //        }
    //        //-----------------------------------------------------------

    //        Day_Order = Convert.ToString(order);
    //        string findday =string.Empty;
    //        if (order == 1)
    //            findday = "mon";
    //        else if (order == 2) findday = "tue";
    //        else if (order == 3) findday = "wed";
    //        else if (order == 4) findday = "thu";
    //        else if (order == 5) findday = "fri";
    //        else if (order == 6) findday = "sat";
    //        else if (order == 7) findday = "sun";
    //        return findday;
    //    }
    //    else
    //        return "";

    //}
    //private string findday(int no, string sdate, string todate)
    //{
    //    int order, holino;
    //    holino = 0;
    //    string day_order =string.Empty;
    //    string from_date =string.Empty;
    //    string fdate =string.Empty;
    //    int diff_work_day = 0;

    //    from_date = todate.ToString();
    //    string[] fm_date = from_date.Split(new Char[] { '/' });
    //    fdate = fm_date[1].ToString() + "/" + fm_date[0].ToString() + "/" + fm_date[2].ToString();
    //    SqlDataReader dr;
    //    con.Close();
    //    con.Open();
    //    cmd = new SqlCommand("select count(*) from holidaystudents where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and holiday_date between '" + sdate.ToString() + "' and  '" + fdate.ToString() + "' and halforfull='0'", con);
    //    dr = cmd.ExecuteReader();
    //    dr.Read();
    //    if (dr.HasRows == true)
    //    {
    //        holino = Convert.ToInt16(dr[0].ToString());
    //    }
    //    DateTime dt1 = Convert.ToDateTime(fdate.ToString());
    //    DateTime dt2 = Convert.ToDateTime(sdate.ToString());
    //    TimeSpan t = dt1.Subtract(dt2);
    //    int days = t.Days;

    //    diff_work_day = days - holino;
    //    order = Convert.ToInt16(diff_work_day.ToString()) % no;
    //    if (order.ToString() == "0")
    //    {
    //        order = no;
    //    }
    //    if (order.ToString() == "1")
    //    {
    //        day_order = "mon";
    //    }
    //    else if (order.ToString() == "2")
    //    {
    //        day_order = "tue";
    //    }
    //    else if (order.ToString() == "3")
    //    {
    //        day_order = "wed";
    //    }
    //    else if (order.ToString() == "4")
    //    {
    //        day_order = "thu";
    //    }
    //    else if (order.ToString() == "5")
    //    {
    //        day_order = "fri";
    //    }
    //    else if (order.ToString() == "6")
    //    {
    //        day_order = "sat";
    //    }
    //    else if (order.ToString() == "7")
    //    {
    //        day_order = "sun";
    //    }
    //    return (day_order);
    //    con.Close();
    //}

    public void SendingSms(string rollno, string date, string hour, string college, string course, string setting, int total, int absent)
    {
        try
        {
            string Gender = string.Empty;
            string collegename1 = string.Empty;
            string Hour = hour;
            string hour_check = string.Empty;
            string admno = string.Empty;
            string app_no = string.Empty;
            string regno = string.Empty;

            string MsgText = string.Empty;
            string RecepientNo = string.Empty;
            int check = 0;
            string user_id = string.Empty;

            collegename1 = college;
            string coursename1 = course;

            string[] split = date.Split(new Char[] { '-' });
            string datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
            date = datefrom;

            if (Convert.ToInt16(hour) == 1)
            {
                Hour = hour + "st ";
                hour_check = hour;
            }

            else if (Convert.ToInt16(hour) == 2)
            {

                Hour = hour + "nd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 3)
            {

                Hour = hour + "rd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) > 3)
            {

                Hour = hour + "th ";
                hour_check = hour;
            }

            string str1 = string.Empty;
            string group_code = Session["group_code"].ToString();
            if (group_code.Contains(";"))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + group_code + "'";
                str1 = str1 + "  select template from master_Settings where settings='SmsAttendanceTepmlate' and group_code='" + group_code + "'and value='1'";
                str1 = str1 + "  select Sections,Roll_Admit,Reg_No,App_No from Registration where Roll_No='" + rollno + "'";

                str1 = str1 + "   select d.PhoneNo from Department d,Degree de,staffmaster s,staff_appl_master sa where d.Dept_Code=de.Dept_Code and s.appl_no=sa.appl_no and d.Head_Of_Dept=s.staff_code and resign='0' and settled='0' and de.Dept_Code ='" + ddlbranch.SelectedItem.Value.ToString() + "'";

            }
            else
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + Session["usercode"].ToString() + "'";
                str1 = str1 + "  select template from master_Settings where settings='SmsAttendanceTepmlate' and usercode='" + Session["usercode"].ToString() + "'and value='1'";
                str1 = str1 + "  select Sections,Roll_Admit,Reg_No,App_No from Registration where Roll_No='" + rollno + "'";

                str1 = str1 + "   select d.PhoneNo from Department d,Degree de,staffmaster s,staff_appl_master sa where d.Dept_Code=de.Dept_Code and s.appl_no=sa.appl_no and d.Head_Of_Dept=s.staff_code and resign='0' and settled='0' and de.Dept_Code ='" + ddlbranch.SelectedItem.Value.ToString() + "'";
            }
            bool flage = false;
            DataSet ds1;
            ds1 = dacces2.select_method_wo_parameter(str1, "txt");

            DataSet dsSMSSendDetails = new DataSet();
            dsSMSSendDetails = dacces2.select_method_wo_parameter("select * from smsdeliverytrackmaster where Convert(varchar(20),date,103)='" + DateTime.Now.ToString("dd/MM/yyyy") + "' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "' and smsFor='absentees'", "text");
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                for (int jj = 0; jj < ds1.Tables[0].Rows.Count; jj++)
                {
                    if (ds1.Tables[0].Rows[jj]["TextName"].ToString() == "Attendance Sms for Absent" && ds1.Tables[0].Rows[jj]["Taxtval"].ToString() == "1")
                    {
                        flage = true;
                    }
                }

                if (flage == true)
                {

                    for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                    {
                        if (ds1.Tables[0].Rows[k]["TextName"].ToString() == "Period" && ds1.Tables[0].Rows[k]["Taxtval"].ToString() != "")
                        {
                            string splihours = ds1.Tables[0].Rows[k]["Taxtval"].ToString();
                            string[] fin_split = splihours.Split(',');
                            int count = fin_split.Length;
                            for (int i = 0; i < count; i++)
                            {
                                string final_Hours = fin_split[i];
                                if (hour_check == final_Hours)
                                {
                                    check = check + 1;
                                }
                            }

                        }
                    }
                }
            }
            if (ds1.Tables.Count > 2 && ds1.Tables[2].Rows.Count > 0)
            {
                regno = Convert.ToString(ds1.Tables[2].Rows[0]["Reg_No"]);
                admno = Convert.ToString(ds1.Tables[2].Rows[0]["Roll_Admit"]);
                app_no = Convert.ToString(ds1.Tables[2].Rows[0]["App_No"]);
            }
            if (check > 0)
            {
                check = 0;
                string ssr = "select * from Track_Value where college_code='" + Session["collegecode"].ToString() + "'";
                DataSet dstrack;
                dstrack = dacces2.select_method_wo_parameter(ssr, "txt");
                if (dstrack.Tables.Count > 0 && dstrack.Tables[0].Rows.Count > 0)
                {
                    user_id = Convert.ToString(dstrack.Tables[0].Rows[0]["SMS_User_ID"]);

                    string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,r.app_no from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                    DataSet dsMobile;
                    dsMobile = dacces2.select_method_wo_parameter(Phone, "txt");

                    DataSet ds;

                    if (ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                    {
                        string studentAppNo = Convert.ToString(dsMobile.Tables[0].Rows[0]["app_no"]).Trim();
                        if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                        {
                            Gender = "Your Son ";
                        }
                        else
                        {
                            Gender = "Your Daughter ";

                        }

                        DateTime dt = Convert.ToDateTime(date);
                        //string section =string.Empty;
                        //if (ddlsec.Enabled == true)
                        //{
                        //    section = Convert.ToString(ddlsec.SelectedItem.Text);
                        //}

                        string section = string.Empty;
                        if (ddlsec.Enabled == true)
                        {
                            section = Convert.ToString(ddlsec.SelectedItem.Text);
                            if (section.Trim().ToLower() == "all")
                            {
                                if (ds1.Tables[1].Rows.Count > 0)
                                {
                                    string sectvalue = ds1.Tables[2].Rows[0][0].ToString();
                                    if (sectvalue.Trim() != "" && sectvalue.Trim() != null)
                                    {
                                        section = sectvalue.ToString();
                                    }
                                }
                            }
                        }
                        if (ds1.Tables.Count > 1 && ds1.Tables[1].Rows.Count > 0)
                        {
                            if (setting == "Hour")
                            {
                                string templatevlaue = Convert.ToString(ds1.Tables[1].Rows[0]["template"]);
                                if (templatevlaue.Trim() != "")
                                {
                                    string[] splittemplate = templatevlaue.Split('$');
                                    if (splittemplate.Length > 0)
                                    {
                                        for (int j = 0; j <= splittemplate.GetUpperBound(0); j++)
                                        {
                                            if (splittemplate[j].ToString() != "")
                                            {
                                                if (splittemplate[j].ToString() == "College Name")
                                                {
                                                    MsgText = MsgText + " " + collegename1;
                                                }

                                                else if (splittemplate[j].ToString() == "Student Name")
                                                {
                                                    MsgText = MsgText + " " + dsMobile.Tables[0].Rows[0]["StudName"].ToString();
                                                }
                                                else if (splittemplate[j].ToString() == "Degree")
                                                {
                                                    MsgText = MsgText + " " + coursename1;
                                                }
                                                else if (splittemplate[j].ToString() == "Section")
                                                {
                                                    if (section.Trim() != "")
                                                    {
                                                        MsgText = MsgText + " " + "" + section + " Section";
                                                    }
                                                }
                                                else if (splittemplate[j].ToString() == "Thank You")
                                                {
                                                    MsgText = MsgText + " " + splittemplate[j].ToString();
                                                }
                                                else if (splittemplate[j].ToString() == "Absent")
                                                {
                                                    MsgText = MsgText + " " + Hour + " hour Absent";
                                                }
                                                else if (splittemplate[j].ToString() == "Conducted Hours")
                                                {
                                                    MsgText = MsgText + " Conducted hours:" + total + "";
                                                }
                                                else if (splittemplate[j].ToString() == "Absent hours")
                                                {
                                                    MsgText = MsgText + " Absent hours:" + absent + "";
                                                }
                                                // 22.09.16
                                                else if (splittemplate[j].ToString() == "Date")
                                                {
                                                    MsgText = MsgText + " Date: " + dt.ToString("dd/MM/yyyy") + "";
                                                }
                                                else if (splittemplate[j].ToString() == "HOD")
                                                {
                                                    if (ds1.Tables[3].Rows.Count > 0)
                                                    {
                                                        MsgText = MsgText + " - " + Convert.ToString(ds1.Tables[3].Rows[0][0]);
                                                    }
                                                    else
                                                    {
                                                        MsgText = MsgText + " ";
                                                    }
                                                }
                                                else if (splittemplate[j].ToString() == "Roll No")
                                                {
                                                    MsgText = MsgText + " " + rollno;
                                                }
                                                else if (splittemplate[j].ToString() == "Register No")
                                                {
                                                    MsgText = MsgText + " " + regno;
                                                }
                                                else if (splittemplate[j].ToString() == "Application No")
                                                {
                                                    MsgText = MsgText + " " + app_no;
                                                }
                                                else if (splittemplate[j].ToString() == "Admission No")
                                                {
                                                    MsgText = MsgText + " " + admno;
                                                }
                                                else
                                                {
                                                    if (MsgText == "")
                                                    {
                                                        MsgText = splittemplate[j].ToString();
                                                    }
                                                    else
                                                    {
                                                        MsgText = MsgText + " " + splittemplate[j].ToString();
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
                            if (setting == "Hour")
                            {
                                MsgText = "Dear Parent, Good Morning. This Message from" + " " + collegename1 + ". Your ward " + dsMobile.Tables[0].Rows[0]["StudName"].ToString() + " of " + coursename1 + "-" + section + " is found absent  " + Hour + " hour. Conducted hour:" + total + " Absent hour:" + absent + ". Thank you";
                            }
                        }
                        for (int jj1 = 0; jj1 < ds1.Tables[0].Rows.Count; jj1++)
                        {
                            bool checkHourAbsentees = false;
                            bool isSentAbsentees = true;
                            if (setting == "Hour")
                            {
                                checkHourAbsentees = true;
                            }
                            DataView dvSendSMSDetails = new DataView();
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                    string getval = dacces2.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[1].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    string strpath = string.Empty;
                                    dvSendSMSDetails = new DataView();
                                    isSentAbsentees = true;
                                    if (dsSMSSendDetails.Tables.Count > 0 && dsSMSSendDetails.Tables[0].Rows.Count > 0)
                                    {
                                        dsSMSSendDetails.Tables[0].DefaultView.RowFilter = "mobilenos='" + RecepientNo + "' and studentAppNo='" + studentAppNo + "'";
                                        dvSendSMSDetails = dsSMSSendDetails.Tables[0].DefaultView;
                                    }
                                    if (checkHourAbsentees && dvSendSMSDetails.Count > 0)
                                    {
                                        isSentAbsentees = false;
                                    }
                                    //if (SenderID != "eSNCET" && Password != "yahoo10")
                                    //{
                                    //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    //}
                                    //else
                                    //{
                                    //    strpath = "http://49.50.69.90/api/smsapi.aspx?username=" + user_id + "&password=" + Password + "&to=" + RecepientNo + "&from=" + SenderID + "&message=" + MsgText;
                                    //}
                                    // string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;

                                    //string isst = "0";

                                    //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                    int nofosmssend = 0;
                                    if (isSentAbsentees)
                                        nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0", "absentees", studentAppNo);
                                }
                            }
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                    string getval = dacces2.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {

                                        SenderID = spret[0].ToString();
                                        Password = spret[1].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    string strpath = string.Empty;
                                    dvSendSMSDetails = new DataView();
                                    isSentAbsentees = true;
                                    if (dsSMSSendDetails.Tables.Count > 0 && dsSMSSendDetails.Tables[0].Rows.Count > 0)
                                    {
                                        dsSMSSendDetails.Tables[0].DefaultView.RowFilter = "mobilenos='" + RecepientNo + "'";
                                        dvSendSMSDetails = dsSMSSendDetails.Tables[0].DefaultView;
                                    }
                                    if (checkHourAbsentees && dvSendSMSDetails.Count > 0)
                                    {
                                        isSentAbsentees = false;
                                    }
                                    //if (SenderID != "eSNCET" && Password != "yahoo10")
                                    //{
                                    //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    //}
                                    //else
                                    //{
                                    //    strpath = "http://49.50.69.90/api/smsapi.aspx?username=" + user_id + "&password=" + Password + "&to=" + RecepientNo + "&from=" + SenderID + "&message=" + MsgText;
                                    //}
                                    //string isst = "0";
                                    //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                    int nofosmssend = 0;
                                    if (isSentAbsentees)
                                        nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0", "absentees", studentAppNo);
                                }
                            }
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                    string getval = dacces2.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[1].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    string strpath = string.Empty;
                                    dvSendSMSDetails = new DataView();
                                    isSentAbsentees = true;
                                    if (dsSMSSendDetails.Tables.Count > 0 && dsSMSSendDetails.Tables[0].Rows.Count > 0)
                                    {
                                        dsSMSSendDetails.Tables[0].DefaultView.RowFilter = "mobilenos='" + RecepientNo + "'";
                                        dvSendSMSDetails = dsSMSSendDetails.Tables[0].DefaultView;
                                    }
                                    if (checkHourAbsentees && dvSendSMSDetails.Count > 0)
                                    {
                                        isSentAbsentees = false;
                                    }
                                    //if (SenderID != "eSNCET" && Password != "yahoo10")
                                    //{
                                    // strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    //}
                                    //else
                                    //{
                                    //    strpath = "http://49.50.69.90/api/smsapi.aspx?username=" + user_id + "&password=" + Password + "&to=" + RecepientNo + "&from=" + SenderID + "&message=" + MsgText;
                                    //}
                                    //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    //string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;

                                    //string isst = "0";

                                    //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                    int nofosmssend = 0;
                                    if (isSentAbsentees)
                                        nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0", "absentees", studentAppNo);
                                }
                            }
                        }

                    }
                }
            }
            else
            {
                if (flage == true)
                {
                    if (setting == "Day")
                    {

                        string ssr = "select * from Track_Value where college_code='" + Session["collegecode"].ToString() + "'";
                        DataSet dstrack;
                        dstrack = dacces2.select_method_wo_parameter(ssr, "txt");
                        if (dstrack.Tables[0].Rows.Count > 0)
                        {
                            user_id = Convert.ToString(dstrack.Tables[0].Rows[0]["SMS_User_ID"]);

                            string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                            DataSet dsMobile;
                            dsMobile = dacces2.select_method_wo_parameter(Phone, "txt");


                            DataSet ds;

                            if (ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                            {
                                if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                                {
                                    Gender = "Your Son ";
                                }
                                else
                                {
                                    Gender = "Your Daughter ";

                                }

                                DateTime dt = Convert.ToDateTime(date);
                                string section = string.Empty;
                                if (ddlsec.Enabled == true)
                                {
                                    section = Convert.ToString(ddlsec.SelectedItem.Text);
                                }
                                if (ds1.Tables[1].Rows.Count > 0)
                                {
                                    if (setting == "Day")
                                    {
                                        string templatevlaue = Convert.ToString(ds1.Tables[1].Rows[0]["template"]);
                                        if (templatevlaue.Trim() != "")
                                        {
                                            string[] splittemplate = templatevlaue.Split('$');
                                            if (splittemplate.Length > 0)
                                            {
                                                for (int j = 0; j <= splittemplate.GetUpperBound(0); j++)
                                                {
                                                    if (splittemplate[j].ToString() != "")
                                                    {
                                                        if (splittemplate[j].ToString() == "College Name")
                                                        {
                                                            MsgText = MsgText + " " + collegename1;
                                                        }

                                                        else if (splittemplate[j].ToString() == "Student Name")
                                                        {
                                                            MsgText = MsgText + " " + dsMobile.Tables[0].Rows[0]["StudName"].ToString();
                                                        }
                                                        else if (splittemplate[j].ToString() == "Degree")
                                                        {
                                                            MsgText = MsgText + " " + coursename1;
                                                        }
                                                        else if (splittemplate[j].ToString() == "Section")
                                                        {
                                                            MsgText = MsgText + " " + "" + section + " Section";
                                                        }
                                                        else if (splittemplate[j].ToString() == "Thank You")
                                                        {
                                                            MsgText = MsgText + " " + splittemplate[j].ToString();
                                                        }
                                                        else if (splittemplate[j].ToString() == "Absent")
                                                        {
                                                            MsgText = MsgText + " " + "absent";
                                                        }
                                                        else if (splittemplate[j].ToString() == "Conducted Days")
                                                        {
                                                            MsgText = MsgText + " Conducted Days: " + total + "";
                                                        }
                                                        else if (splittemplate[j].ToString() == "Absent Days")
                                                        {
                                                            MsgText = MsgText + " Absent Days: " + absent + "";
                                                        }
                                                        //22/09/16
                                                        else if (splittemplate[j].ToString() == "Date")
                                                        {
                                                            MsgText = MsgText + " Date: " + dt.ToString("dd/MM/yyyy") + "";
                                                        }
                                                        else if (splittemplate[j].ToString() == "HOD")
                                                        {
                                                            if (ds1.Tables[3].Rows.Count > 0)
                                                            {
                                                                MsgText = MsgText + " - " + Convert.ToString(ds1.Tables[3].Rows[0][0]);
                                                            }
                                                            else
                                                            {
                                                                MsgText = MsgText + " ";
                                                            }
                                                        }
                                                        else if (splittemplate[j].ToString() == "Roll No")
                                                        {
                                                            MsgText = MsgText + " " + rollno;
                                                        }
                                                        else if (splittemplate[j].ToString() == "Register No")
                                                        {
                                                            MsgText = MsgText + " " + regno;
                                                        }
                                                        else if (splittemplate[j].ToString() == "Application No")
                                                        {
                                                            MsgText = MsgText + " " + app_no;
                                                        }
                                                        else if (splittemplate[j].ToString() == "Admission No")
                                                        {
                                                            MsgText = MsgText + " " + admno;
                                                        }
                                                        else
                                                        {
                                                            if (MsgText == "")
                                                            {
                                                                MsgText = splittemplate[j].ToString();
                                                            }
                                                            else
                                                            {
                                                                MsgText = MsgText + " " + splittemplate[j].ToString();
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
                                    if (setting == "Day")
                                    {
                                        MsgText = "Dear Parent, Good Morning. This Message from " + " " + collegename1 + ". Your ward " + dsMobile.Tables[0].Rows[0]["StudName"].ToString() + " of " + coursename1 + "-" + section + " is found absent today. Conducted Days:" + total + " Absent Days:" + absent + ". Thank you";
                                    }
                                }

                                for (int jj1 = 0; jj1 < ds1.Tables[0].Rows.Count; jj1++)
                                {
                                    if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                    {
                                        if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                                        {
                                            RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                            string getval = dacces2.GetUserapi(user_id);
                                            string[] spret = getval.Split('-');
                                            if (spret.GetUpperBound(0) == 1)
                                            {

                                                SenderID = spret[0].ToString();
                                                Password = spret[1].ToString();
                                                Session["api"] = user_id;
                                                Session["senderid"] = SenderID;
                                            }
                                            string strpath = string.Empty;
                                            //if (SenderID != "eSNCET" && Password != "yahoo10")
                                            //{
                                            //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                            //}
                                            //else
                                            //{
                                            //    strpath = "http://49.50.69.90/api/smsapi.aspx?username=" + user_id + "&password=" + Password + "&to=" + RecepientNo + "&from=" + SenderID + "&message=" + MsgText;
                                            //}

                                            //string isst = "0";

                                            //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                            int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");

                                        }

                                    }
                                    if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                    {
                                        if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                                        {
                                            RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();

                                            string getval = dacces2.GetUserapi(user_id);
                                            string[] spret = getval.Split('-');
                                            if (spret.GetUpperBound(0) == 1)
                                            {

                                                SenderID = spret[0].ToString();
                                                Password = spret[1].ToString();
                                                Session["api"] = user_id;
                                                Session["senderid"] = SenderID;
                                            }
                                            string strpath = string.Empty;
                                            //if (SenderID != "eSNCET" && Password != "yahoo10")
                                            //{
                                            // strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                            //}
                                            //else
                                            //{
                                            //    strpath = "http://49.50.69.90/api/smsapi.aspx?username=" + user_id + "&password=" + Password + "&to=" + RecepientNo + "&from=" + SenderID + "&message=" + MsgText;
                                            //}

                                            //string isst = "0";

                                            //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                            int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");
                                        }

                                    }
                                    if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                    {
                                        if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                                        {

                                            RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();

                                            string getval = dacces2.GetUserapi(user_id);
                                            string[] spret = getval.Split('-');
                                            if (spret.GetUpperBound(0) == 1)
                                            {

                                                SenderID = spret[0].ToString();
                                                Password = spret[1].ToString();
                                                Session["api"] = user_id;
                                                Session["senderid"] = SenderID;
                                            }
                                            string strpath = string.Empty;
                                            //if (SenderID != "eSNCET" && Password != "yahoo10")
                                            //{
                                            //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                            //}
                                            //else
                                            //{
                                            //    strpath = "http://49.50.69.90/api/smsapi.aspx?username=" + user_id + "&password=" + Password + "&to=" + RecepientNo + "&from=" + SenderID + "&message=" + MsgText;
                                            //}

                                            //string isst = "0";

                                            //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                            int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");
                                        }

                                    }
                                }
                            }
                        }
                    }
                    else if (setting == "Minimun Absent Day")
                    {
                        string ssr = "select * from Track_Value where college_code='" + Session["collegecode"].ToString() + "'";
                        DataSet dstrack;
                        dstrack = dacces2.select_method_wo_parameter(ssr, "txt");
                        if (dstrack.Tables[0].Rows.Count > 0)
                        {
                            user_id = Convert.ToString(dstrack.Tables[0].Rows[0]["SMS_User_ID"]);

                            string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                            DataSet dsMobile;
                            dsMobile = dacces2.select_method_wo_parameter(Phone, "txt");


                            string days = string.Empty;

                            if (ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                            {

                                if (minimum_day == "1")
                                {
                                    days = "Day";
                                }
                                if (minimum_day != "1")
                                {
                                    days = "Days";
                                }
                                DateTime dt = Convert.ToDateTime(date);
                                string date1 = TxtToDate.Text;
                                string[] splitdate = date1.Split('-');

                                DateTime statrtdate = Convert.ToDateTime(splitdate[1].ToString() + "/" + splitdate[0].ToString() + "/" + splitdate[2].ToString());
                                statrtdate = statrtdate.AddDays(7);

                                string seconddate = TxtToDate.Text;
                                string finalseconddate = seconddate.Replace("-", "/");

                                MsgText = "Dear Parent, your ward is absent for " + minimum_day + " " + days + " (" + countarray[countarray.Count - 1].ToString() + " to " + finalseconddate.ToString() + "). You are requested to meet Principal/Hod on or before a week " + statrtdate.ToString("dd/MM/yyyy") + ". Thank you!.";

                                for (int jj1 = 0; jj1 < ds1.Tables[0].Rows.Count; jj1++)
                                {
                                    if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                    {

                                        if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                                        {


                                            RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();


                                            string getval = dacces2.GetUserapi(user_id);
                                            string[] spret = getval.Split('-');
                                            if (spret.GetUpperBound(0) == 1)
                                            {

                                                SenderID = spret[0].ToString();
                                                Password = spret[1].ToString();
                                                Session["api"] = user_id;
                                                Session["senderid"] = SenderID;
                                            }
                                            string strpath = string.Empty;
                                            //if (SenderID != "eSNCET" && Password != "yahoo10")
                                            //{
                                            //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                            //}
                                            //else
                                            //{
                                            //    strpath = "http://49.50.69.90/api/smsapi.aspx?username=" + user_id + "&password=" + Password + "&to=" + RecepientNo + "&from=" + SenderID + "&message=" + MsgText;
                                            //}

                                            //string isst = "0";

                                            //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                            int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");


                                        }

                                    }
                                    if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                    {
                                        if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                                        {

                                            RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();

                                            string getval = dacces2.GetUserapi(user_id);
                                            string[] spret = getval.Split('-');
                                            if (spret.GetUpperBound(0) == 1)
                                            {

                                                SenderID = spret[0].ToString();
                                                Password = spret[1].ToString();
                                                Session["api"] = user_id;
                                                Session["senderid"] = SenderID;
                                            }
                                            string strpath = string.Empty;
                                            //if (SenderID != "eSNCET" && Password != "yahoo10")
                                            //{
                                            //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                            //}
                                            //else
                                            //{
                                            //    strpath = "http://49.50.69.90/api/smsapi.aspx?username=" + user_id + "&password=" + Password + "&to=" + RecepientNo + "&from=" + SenderID + "&message=" + MsgText;
                                            //}

                                            //string isst = "0";

                                            //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                            int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");
                                        }

                                    }
                                    if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                    {
                                        if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                                        {
                                            RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();

                                            string getval = dacces2.GetUserapi(user_id);
                                            string[] spret = getval.Split('-');
                                            if (spret.GetUpperBound(0) == 1)
                                            {

                                                SenderID = spret[0].ToString();
                                                Password = spret[1].ToString();
                                                Session["api"] = user_id;
                                                Session["senderid"] = SenderID;
                                            }
                                            string strpath = string.Empty;
                                            //if (SenderID != "eSNCET" && Password != "yahoo10")
                                            //{
                                            //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                            //}
                                            //else
                                            //{
                                            //    strpath = "http://49.50.69.90/api/smsapi.aspx?username=" + user_id + "&password=" + Password + "&to=" + RecepientNo + "&from=" + SenderID + "&message=" + MsgText;
                                            //}

                                            //string isst = "0";

                                            //smsreport(strpath, isst, dt, RecepientNo, MsgText);
                                            int nofosmssend = d2.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0");
                                        }

                                    }
                                }

                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    //public void GetUserapi(string user_id)
    //{
    //    try
    //    {
    //        if (user_id == "AAACET")
    //        {
    //            SenderID = "AAACET";
    //            Password = "AAACET";
    //        }
    //        else if (user_id == "AALIME")
    //        {
    //            SenderID = "AALIME";
    //            Password = "AALIME";
    //        }
    //        else if (user_id == "ACETVM")
    //        {
    //            SenderID = "ACETVM";
    //            Password = "ACETVM";
    //        }
    //        else if (user_id == "AGNICT")
    //        {
    //            SenderID = "AGNICT";
    //            Password = "AGNICT";
    //        }
    //        else if (user_id == "AMSPTC")
    //        {
    //            SenderID = "AMSPTC";
    //            Password = "AMSPTC";
    //        }
    //        else if (user_id == "ANGE")
    //        {
    //            SenderID = "ANGE";
    //            Password = "ANGE";
    //        }
    //        else if (user_id == "ARASUU")
    //        {
    //            SenderID = "ARASUU";
    //            Password = "ARASUU";
    //        }
    //        else if (user_id == "DAVINC")
    //        {
    //            SenderID = "DAVINC";
    //            Password = "DAVINC";
    //        }
    //        else if (user_id == "EASACG")
    //        {
    //            SenderID = "EASACG";
    //            Password = "EASACG";
    //        }
    //        else if (user_id == "ECESMS")
    //        {
    //            SenderID = "ECESMS";
    //            Password = "ECESMS";
    //        }
    //        else if (user_id == "ESECED")
    //        {
    //            SenderID = "ESECED";
    //            Password = "ESECED";
    //        }
    //        else if (user_id == "ESENGG")
    //        {
    //            SenderID = "ESENGG";
    //            Password = "ESENGG";
    //        }
    //        else if (user_id == "ESEPTC")
    //        {
    //            SenderID = "ESEPTC";
    //            Password = "ESEPTC";
    //        }
    //        else if (user_id == "ESMSCH")
    //        {
    //            SenderID = "ESMSCH";
    //            Password = "ESMSCH";
    //        }
    //        else if (user_id == "GKMCET")
    //        {
    //            SenderID = "GKMCET";
    //            Password = "GKMCET";
    //        }
    //        else if (user_id == "IJAYAM")
    //        {
    //            SenderID = "IJAYAM";
    //            Password = "IJAYAM";
    //        }
    //        else if (user_id == "JJAAMC")
    //        {
    //            SenderID = "JJAAMC";
    //            Password = "JJAAMC";
    //        }

    //        else if (user_id == "KINGSE")
    //        {
    //            SenderID = "KINGSE";
    //            Password = "KINGSE";
    //        }
    //        else if (user_id == "KNMHSS")
    //        {
    //            SenderID = "KNMHSS";
    //            Password = "KNMHSS";
    //        }
    //        else if (user_id == "KSRIET")
    //        {
    //            SenderID = "KSRIET";
    //            Password = "KSRIET";
    //        }
    //        else if (user_id == "KTVRKP")
    //        {
    //            SenderID = "KTVRKP";
    //            Password = "KTVRKP";
    //        }
    //        else if (user_id == "MPNMJS")
    //        {
    //            SenderID = "MPNMJS";
    //            Password = "MPNMJS";
    //        }
    //        else if (user_id == "NANDHA")
    //        {
    //            SenderID = "NANDHA";
    //            Password = "NANDHA";
    //        }
    //        else if (user_id == "NECARE")
    //        {
    //            SenderID = "NECARE";
    //            Password = "NECARE";
    //        }
    //        else if (user_id == "NSNCET")
    //        {
    //            SenderID = "NSNCET";
    //            Password = "NSNCET";
    //        }
    //        else if (user_id == "PETENG")
    //        {
    //            SenderID = "PETENG";
    //            Password = "PETENG";
    //        }
    //        else if (user_id == "PMCTEC")
    //        {
    //            SenderID = "PMCTEC";
    //            Password = "PMCTEC";
    //        }
    //        else if (user_id == "PPGITS")
    //        {
    //            SenderID = "PPGITS";
    //            Password = "PPGITS";
    //        }
    //        else if (user_id == "PROFCL")
    //        {
    //            SenderID = "PROFCL";
    //            Password = "PROFCL";
    //        }
    //        else if (user_id == "PSVCET")
    //        {
    //            SenderID = "PSVCET";
    //            Password = "PSVCET";
    //        }
    //        else if (user_id == "SASTH")
    //        {
    //            SenderID = "SASTH";
    //            Password = "SASTH";
    //        }
    //        else if (user_id == "SCTSBS")
    //        {
    //            SenderID = "SCTSBS";
    //            Password = "SCTSBS";
    //        }
    //        else if (user_id == "SCTSCE")
    //        {
    //            SenderID = "SCTSCE";
    //            Password = "SCTSCE";
    //        }
    //        else if (user_id == "SCTSEC")
    //        {
    //            SenderID = "SCTSEC";
    //            Password = "SCTSEC";
    //        }
    //        else if (user_id == "SKCETC")
    //        {
    //            SenderID = "SKCETC";
    //            Password = "SKCETC";
    //        }
    //        else if (user_id == "SRECCG")
    //        {
    //            SenderID = "SRECCG";
    //            Password = "SRECCG";
    //        }
    //        else if (user_id == "SLAECT")
    //        {
    //            SenderID = "SLAECT";
    //            Password = "SLAECT";
    //        }
    //        else if (user_id == "SSCENG")
    //        {
    //            SenderID = "SSCENG";
    //            Password = "SSCENG";
    //        }
    //        else if (user_id == "SSMCEE")
    //        {
    //            SenderID = "SSMCEE";
    //            Password = "SSMCEE";
    //        }
    //        else if (user_id == "SVICET")
    //        {
    //            SenderID = "SVICET";
    //            Password = "SVICET";
    //        }
    //        else if (user_id == "SVCTCG")
    //        {
    //            SenderID = "SVCTCG";
    //            Password = "SVCTCG";
    //        }
    //        else if (user_id == "SVSCBE")
    //        {
    //            SenderID = "SVSCBE";
    //            Password = "SVSCBE";
    //        }
    //        else if (user_id == "TECENG")
    //        {
    //            SenderID = "TECENG";
    //            Password = "TECENG";
    //        }
    //        else if (user_id == "TJENGG")
    //        {
    //            SenderID = "TJENGG";
    //            Password = "TJENGG";
    //        }
    //        else if (user_id == "TSMJCT")
    //        {
    //            SenderID = "TSMJCT";
    //            Password = "TSMJCT";
    //        }
    //        else if (user_id == "VCWSMS")
    //        {
    //            SenderID = "VCWSMS";
    //            Password = "VCWSMS";
    //        }
    //        else if (user_id == "VRSCET")
    //        {
    //            SenderID = "VRSCET";
    //            Password = "VRSCET";
    //        }
    //        else if (user_id == "AUDIIT")
    //        {
    //            SenderID = "AUDIIT";
    //            Password = "AUDIIT";
    //        }
    //        else if (user_id == "SAENGG")
    //        {
    //            SenderID = "SAENGG";
    //            Password = "SAENGG";
    //        }

    //        else if (user_id == "STANE")
    //        {
    //            SenderID = "STANES";
    //            Password = "STANES";
    //        }

    //        else if (user_id == "MBCBSE")
    //        {
    //            SenderID = "MBCBSE";
    //            Password = "MBCBSE";
    //        }

    //        else if (user_id == "HIETPT")
    //        {
    //            SenderID = "HIETPT";
    //            Password = "HIETPT";
    //        }

    //        else if (user_id == "SVPITM")
    //        {
    //            SenderID = "SVPITM";
    //            Password = "SVPITM";
    //        }

    //        else if (user_id == "AUDCET")
    //        {
    //            SenderID = "AUDCET";
    //            Password = "AUDCET";
    //        }
    //        else if (user_id == "AUDWOM")
    //        {
    //            SenderID = "AUDWOM";
    //            Password = "AUDWOM";
    //        }

    //        else if (user_id == "AUDIPG")
    //        {
    //            SenderID = "AUDIPG";
    //            Password = "AUDIPG";
    //        }

    //        else if (user_id == "MCCDAY")
    //        {
    //            SenderID = "MCCDAY";
    //            Password = "MCCDAY";
    //        }

    //        else if (user_id == "MCCSFS")
    //        {
    //            SenderID = "MCCSFS";
    //            Password = "MCCSFS";
    //        }
    //        else if (user_id == "JMHRSS")
    //        {
    //            SenderID = "JMHRSS";
    //            Password = "JMHRSS";
    //        }
    //        else if (user_id == "JHSSCB")
    //        {
    //            SenderID = "JHSSCB";
    //            Password = "JHSSCB";
    //        }
    //        else if (user_id == "JMHRSS")
    //        {
    //            SenderID = "JMHRSS";
    //            Password = "JMHRSS";
    //        }
    //        else if (user_id == "JHSSCB")
    //        {
    //            SenderID = "JHSSCB";
    //            Password = "JHSSCB";
    //        } 
    //        Session["api"] = user_id;
    //        Session["senderid"] = SenderID;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    public void smsreport(string uril, string isstaff, DateTime dt, string phone, string msg)
    {
        try
        {
            string phoneno = phone;
            string message = msg;
            string date = dt.ToString("MM/dd/yyyy") + ' ' + DateTime.Now.ToString("hh:mm:ss");
            WebRequest request = WebRequest.Create(uril);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();
            string groupmsgid = string.Empty;
            groupmsgid = strvel;
            string[] splitvalue = groupmsgid.Split(' ');
            int sms = 0;
            string smsreportinsert = string.Empty;
            string[] split_mobileno = phoneno.Split(new Char[] { ',' });
            for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
            {
                smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date,sender_id)values( '" + split_mobileno[icount] + "','" + splitvalue[0] + "','" + message + "','" + Session["collegecode"].ToString() + "','" + isstaff + "','" + date + "','" + Session["UserCode"].ToString() + "')";// Added by jairam 21-11-2014
                sms = dacces2.insert_method(smsreportinsert, hat, "Text");
            }
        }
        catch (Exception ex)
        {

        }

    }

    public void loadcollegename() // Added by Jayaraman 30.07.2014
    {
        try
        {
            string collquery = "Select collname,Coll_acronymn from collinfo where college_code=" + Session["collegecode"].ToString() + "";
            DataSet datacol = new DataSet();
            datacol.Clear();
            datacol = dacces2.select_method_wo_parameter(collquery, "Text");
            if (datacol.Tables.Count > 0 && datacol.Tables[0].Rows.Count > 0)
            {
                collacronym = datacol.Tables[0].Rows[0]["Coll_acronymn"].ToString();
                collegename = datacol.Tables[0].Rows[0]["collname"].ToString();
            }
            string degreequery = "select distinct Course_Name,Dept_Name from Department dep, Degree deg, course c where dep.Dept_Code=deg.Dept_Code and c.Course_Id=deg.Course_Id and deg.college_code =" + Session["collegecode"].ToString() + " and Degree_Code=" + ddlbranch.SelectedItem.Value + "";
            DataSet dscode = new DataSet();
            dscode = dacces2.select_method_wo_parameter(degreequery, "Text");
            if (dscode.Tables.Count > 0 && dscode.Tables[0].Rows.Count > 0)
            {
                string course = dscode.Tables[0].Rows[0]["Course_Name"].ToString();
                string deptname = dscode.Tables[0].Rows[0]["Dept_Name"].ToString();
                coursename = course + "-" + deptname;
            }
        }
        catch
        {

        }
    }

    //Added by  jayaraman  08/08/2014 
    public void sendvoicecall(string rollno, string date, string hour, string batch, string degree, string college, string course, string setting)
    {
        try
        {
            string Hour = hour;
            string hour_check = string.Empty;
            string roll = rollno;
            string batchyear = batch;
            string coursename = course;
            string voicelanguage = string.Empty;
            string collegename = college;

            string MsgText = string.Empty;
            string RecepientNo = string.Empty;
            int check = 0;

            string[] split = date.Split(new Char[] { '-' });
            string datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
            date = datefrom;

            if (Convert.ToInt16(hour) == 1)
            {
                Hour = hour + "st ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 2)
            {
                Hour = hour + "nd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 3)
            {
                Hour = hour + "rd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) > 3)
            {
                Hour = hour + "th ";
                hour_check = hour;
            }

            string str1 = string.Empty;
            string group_code = Session["group_code"].ToString();
            if (group_code.Contains(";"))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + group_code + "'";
                str1 = str1 + "  select Sections  from Registration where Roll_No='" + rollno + "'";
            }
            else
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + Session["usercode"].ToString() + "'";
                str1 = str1 + "  select Sections  from Registration where Roll_No='" + rollno + "'";
            }

            bool flage = false;
            DataSet ds1 = new DataSet();
            ds1 = dacces2.select_method_wo_parameter(str1, "txt");
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                for (int jj = 0; jj < ds1.Tables[0].Rows.Count; jj++)
                {
                    if (ds1.Tables[0].Rows[jj]["TextName"].ToString() == "Voice Call for Absent" && ds1.Tables[0].Rows[jj]["Taxtval"].ToString() == "1")
                    {
                        flage = true;
                    }
                }
                if (flage == true)
                {
                    for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                    {
                        if (ds1.Tables[0].Rows[k]["TextName"].ToString() == "Period" && ds1.Tables[0].Rows[k]["Taxtval"].ToString() != "")
                        {
                            string splihours = ds1.Tables[0].Rows[k]["Taxtval"].ToString();
                            string[] fin_split = splihours.Split(',');
                            int count = fin_split.Length;
                            for (int i = 0; i < count; i++)
                            {
                                string final_Hours = fin_split[i];
                                if (hour_check == final_Hours)
                                {
                                    check = check + 1;
                                }
                            }
                        }
                    }
                }
            }

            string section_voice = string.Empty;
            if (ddlsec.Enabled == true)
            {
                section_voice = Convert.ToString(ddlsec.SelectedItem.Text);
                if (section_voice == "All")
                {
                    if (ds1.Tables[1].Rows.Count > 0)
                    {
                        string sectvalue = ds1.Tables[2].Rows[0][0].ToString();
                        if (sectvalue.Trim() != "" && sectvalue.Trim() != null)
                        {
                            section_voice = sectvalue.ToString();
                        }
                    }
                }
            }

            if (check > 0)
            {
                check = 0;
                string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,VoiceLang from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                DataSet dsMobile;
                dsMobile = dacces2.select_method_wo_parameter(Phone, "txt");
                string voicelang = string.Empty;
                if (dsMobile.Tables.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                {
                    voicelang = Convert.ToString(dsMobile.Tables[0].Rows[0]["VoiceLang"]);
                    if (voicelang != "")
                    {
                        string langquery = string.Empty;
                        langquery = "select TextVal from textvaltable where TextCode  ='" + voicelang + "' and TextCriteria='PLang' and college_code=" + Session["collegecode"] + "";
                        DataSet datalang = new DataSet();
                        datalang = dacces2.select_method_wo_parameter(langquery, "Text");
                        if (datalang.Tables[0].Rows.Count > 0)
                        {
                            voicelanguage = datalang.Tables[0].Rows[0]["TextVal"].ToString();
                        }
                    }
                }
                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                {
                    string gender = string.Empty;
                    if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                    {
                        gender = "MALE";
                    }
                    else
                    {
                        gender = "FEMALE";
                    }
                    string orginalname = string.Empty;
                    string student_name = Convert.ToString(dsMobile.Tables[0].Rows[0]["StudName"]);
                    if (student_name.Contains(".") == true)
                    {
                        string[] splitname = student_name.Split('.');

                        for (int i = 0; i <= splitname.GetUpperBound(0); i++)
                        {
                            string lengthname = splitname[i].ToString();
                            if (lengthname.Trim().Length > 2)
                            {
                                orginalname = splitname[i].ToString();
                            }
                        }
                    }
                    else
                    {
                        string[] split2ndname = student_name.Split(' ');
                        if (split2ndname.Length > 0)
                        {
                            for (int k = 0; k <= split2ndname.GetUpperBound(0); k++)
                            {
                                string firstname = split2ndname[k].ToString();
                                if (firstname.Trim().Length > 2)
                                {
                                    if (orginalname == "")
                                    {
                                        orginalname = firstname.ToString();
                                    }
                                    else
                                    {
                                        orginalname = orginalname + " " + firstname.ToString();
                                    }
                                }
                            }
                        }
                    }
                    DateTime dt = Convert.ToDateTime(date);
                    biz.lbinfotech.www.Data h = new biz.lbinfotech.www.Data();
                    MsgText = "ABSETN AT ";
                    for (int jj1 = 0; jj1 < ds.Tables[0].Rows.Count; jj1++)
                    {
                        if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                            {
                                RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");

                            }

                        }
                        if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                            {

                                RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                            }
                        }
                        if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                            {
                                RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                            }
                        }
                    }
                }
            }
            else
            {
                if (flage == true)
                {
                    if (setting == "Day")
                    {
                        string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,VoiceLang from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                        DataSet dsMobile;
                        dsMobile = dacces2.select_method_wo_parameter(Phone, "txt");

                        string voicelang = Convert.ToString(dsMobile.Tables[0].Rows[0]["VoiceLang"]);
                        if (voicelang != "")
                        {
                            string langquery = string.Empty;
                            langquery = "select TextVal from textvaltable where TextCode  ='" + voicelang + "' and TextCriteria='PLang' and college_code=" + Session["collegecode"] + "";
                            DataSet datalang = new DataSet();
                            datalang = dacces2.select_method_wo_parameter(langquery, "Text");
                            if (datalang.Tables[0].Rows.Count > 0)
                            {
                                voicelanguage = datalang.Tables[0].Rows[0]["TextVal"].ToString();
                            }
                        }
                        // voicelanguage = "English";
                        if (ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                        {
                            string gender = string.Empty;
                            if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                            {
                                gender = "MALE";
                            }
                            else
                            {
                                gender = "FEMALE";
                            }
                            string orginalname = string.Empty;
                            string student_name = Convert.ToString(dsMobile.Tables[0].Rows[0]["StudName"]);
                            if (student_name.Contains(".") == true)
                            {
                                string[] splitname = student_name.Split('.');
                                for (int i = 0; i <= splitname.GetUpperBound(0); i++)
                                {
                                    string lengthname = splitname[i].ToString();
                                    if (lengthname.Trim().Length > 2)
                                    {
                                        orginalname = splitname[i].ToString();
                                    }
                                }
                            }
                            else
                            {
                                string[] split2ndname = student_name.Split(' ');
                                if (split2ndname.Length > 0)
                                {
                                    for (int k = 0; k <= split2ndname.GetUpperBound(0); k++)
                                    {
                                        string firstname = split2ndname[k].ToString();
                                        if (firstname.Trim().Length > 2)
                                        {
                                            if (orginalname == "")
                                            {
                                                orginalname = firstname.ToString();
                                            }
                                            else
                                            {
                                                orginalname = orginalname + " " + firstname.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            DateTime dt = Convert.ToDateTime(date);
                            biz.lbinfotech.www.Data h = new biz.lbinfotech.www.Data();
                            MsgText = "ABSETN AT ";
                            for (int jj1 = 0; jj1 < ds1.Tables[0].Rows.Count; jj1++)
                            {
                                if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                {
                                    if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                                    {
                                        RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                        string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILY", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    }
                                }
                                if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                {
                                    if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                                    {
                                        RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                        string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILY", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");

                                    }
                                }
                                if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                {
                                    if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                                    {
                                        RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                        string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILY", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    }
                                }
                            }
                        }
                    }
                    else if (setting == "Minimun Absent Day")
                    {
                        string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,VoiceLang from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                        DataSet dsMobile;
                        dsMobile = dacces2.select_method_wo_parameter(Phone, "txt");

                        string voicelang = Convert.ToString(dsMobile.Tables[0].Rows[0]["VoiceLang"]);
                        if (voicelang != "")
                        {
                            string langquery = string.Empty;
                            langquery = "select TextVal from textvaltable where TextCode  ='" + voicelang + "' and TextCriteria='PLang' and college_code=" + Session["collegecode"] + "";
                            DataSet datalang = new DataSet();
                            datalang = dacces2.select_method_wo_parameter(langquery, "Text");
                            if (datalang.Tables[0].Rows.Count > 0)
                            {
                                voicelanguage = datalang.Tables[0].Rows[0]["TextVal"].ToString();
                            }
                        }
                        //voicelanguage = "English";
                        if (ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                        {
                            string gender = string.Empty;
                            if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                            {
                                gender = "MALE";
                            }
                            else
                            {
                                gender = "FEMALE";
                            }

                            string orginalname = string.Empty;
                            string student_name = Convert.ToString(dsMobile.Tables[0].Rows[0]["StudName"]);
                            if (student_name.Contains(".") == true)
                            {
                                string[] splitname = student_name.Split('.');

                                for (int i = 0; i <= splitname.GetUpperBound(0); i++)
                                {
                                    string lengthname = splitname[i].ToString();
                                    if (lengthname.Trim().Length > 2)
                                    {
                                        orginalname = splitname[i].ToString();
                                    }
                                }
                            }
                            else
                            {
                                string[] split2ndname = student_name.Split(' ');
                                if (split2ndname.Length > 0)
                                {
                                    for (int k = 0; k <= split2ndname.GetUpperBound(0); k++)
                                    {
                                        string firstname = split2ndname[k].ToString();
                                        if (firstname.Trim().Length > 2)
                                        {
                                            if (orginalname == "")
                                            {
                                                orginalname = firstname.ToString();
                                            }
                                            else
                                            {
                                                orginalname = orginalname + " " + firstname.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                            DateTime dt = Convert.ToDateTime(date);
                            biz.lbinfotech.www.Data h = new biz.lbinfotech.www.Data();
                            MsgText = "ABSETN AT ";
                            for (int jj1 = 0; jj1 < ds.Tables[0].Rows.Count; jj1++)
                            {
                                if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                {
                                    if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                                    {
                                        RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                        string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILY", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    }
                                }
                                if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                {
                                    if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                                    {
                                        RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                        string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILY", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    }
                                }
                                if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                                {
                                    if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                                    {
                                        RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                        string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILY", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "-" + section_voice + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString().Trim() + "");
                                    }
                                }
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

    protected void btnattOk_Click(object sender, EventArgs e)
    {
        try
        {
            attendacesavefunction();
            mpemsgboxsave.Hide();
        }
        catch
        {
        }
    }

    protected void btnattCancel_Click(object sender, EventArgs e)
    {
        try
        {
            mpemsgboxsave.Hide();
        }
        catch
        {
        }
    }

    public void attendacesavefunction()
    {
        try
        {
            bool savefalg = false;//Added by Srinath 23/8/2013
            int savevalue = 0;
            string strquery = string.Empty;
            int insert = 0;
            string insertvalues = string.Empty;
            string updatevalues = string.Empty;
            string monthandyear = string.Empty;
            loadcollegename();
            DataSet data1 = new DataSet();
            ArrayList notarray = new ArrayList();
            Hashtable holiday = new Hashtable();
            WebService web = new WebService();
            if (txtFromDate.Text != "" && TxtToDate.Text != "")
            {
                if (FpSpread2.Sheets[0].ColumnCount > 1)
                {
                    if (ckmanual.Checked == true)
                    {
                        string sub_no = ddlsubject.SelectedValue;
                        string str_Date1;

                        string str_rollno1;
                        string str_hour1;
                        string Atyear1;
                        string Atmonth1;
                        long strdate1;
                        string str_day1;
                        string Att_mark1;
                        string Att_value1;
                        string Insertquery1;
                        string updatequery1;
                        string dcolumn1;
                        string Splitmondate1;


                        str_Date1 = string.Empty;
                        str_rollno1 = string.Empty;
                        str_hour1 = string.Empty;
                        str_day1 = string.Empty;
                        Att_mark1 = string.Empty;
                        Att_value1 = string.Empty;
                        Insertquery1 = string.Empty;
                        updatequery1 = string.Empty;
                        dcolumn1 = string.Empty;
                        Splitmondate1 = string.Empty;

                        for (int Att_row = 1; Att_row <= FpSpread2.Sheets[0].RowCount - 3; Att_row++)
                        {
                            str_rollno1 = FpSpread2.Sheets[0].Cells[Att_row, 1].Text.ToString();
                            for (int Att_column = 5; Att_column <= FpSpread2.Sheets[0].ColumnCount - 1; Att_column++)
                            {
                                str_Date1 = FpSpread2.Sheets[0].ColumnHeader.GetClipValue(0, Att_column, 1, 1);
                                Splitmondate1 = str_Date1.ToString();
                                string[] split = Splitmondate1.Split(new Char[] { '-' });
                                str_day1 = split[0].ToString();
                                Atmonth1 = split[1].ToString();
                                Atyear1 = split[2].ToString();
                                strdate1 = (Convert.ToInt32(Atmonth1) + Convert.ToInt32(Atyear1) * 12);
                                str_hour1 = FpSpread2.Sheets[0].ColumnHeader.GetClipValue(1, Att_column, 1, 1);
                                string[] split_hr = str_hour1.Split(new Char[] { '-' });
                                str_hour1 = str_hour1[0].ToString();
                                Att_mark1 = Convert.ToString(FpSpread2.GetEditValue(Att_row, Att_column).ToString());
                                if (Att_mark1 == "System.Object")
                                {
                                    Att_mark1 = FpSpread2.Sheets[0].Cells[Att_row, Att_column].Text.ToString();
                                }

                                if (Att_mark1 == "A")
                                {
                                }

                                string sect = "", sec_sql = string.Empty;
                                if (ddlsec.SelectedItem.Text.Trim().ToLower() != "all" && ddlsec.SelectedItem.Text.Trim() != "")
                                {
                                    sec_sql = " and sections='" + ddlsec.SelectedItem.Text + "'";
                                    sect = ddlsec.SelectedItem.Text;
                                }
                                Att_value1 = Attvalues(Att_mark1);
                                if (Att_value1 != "")
                                {
                                    nullflag = true;
                                }
                                string query = "select * from Direct_Schedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and month_year = " + strdate1 + "  and sections='" + ddlsec.Text + "'";
                                DataSet ds_save2 = new DataSet();
                                ds_save2 = dacces2.select_method(query, hat, "Text");
                                if (ds_save2.Tables.Count > 0)
                                {
                                    if (ds_save2.Tables[0].Rows.Count > 0)//Save Attendance
                                    {
                                        dcolumn1 = "d" + str_day1 + "d" + str_hour1 + "=" + sub_no;
                                        updatequery1 = "update Direct_Schedule set " + dcolumn1 + "  where  degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and month_year = " + strdate1 + sec_sql;

                                        int a = dacces2.update_method_wo_parameter(updatequery1, "Text");

                                        savefalg = true;//Added by srinath 23/8/2013
                                        savevalue = 2;
                                    }
                                    else
                                    {
                                        dcolumn1 = "d" + str_day1 + "d" + str_hour1;
                                        Insertquery1 = "insert into Direct_Schedule(batch_year,degree_code,sections,month_year," + dcolumn1 + ") values (" + ddlbatch.SelectedValue.ToString() + "," + ddlbranch.SelectedValue.ToString() + ",'" + sect.ToString() + "'," + strdate1 + "," + sub_no + ")";
                                        int a = dacces2.update_method_wo_parameter(Insertquery1, "Text");
                                        savefalg = true;//Added by srinath 23/8/2013
                                        savevalue = 1;
                                    }
                                }
                            }
                        }

                        string college_code = (string)Session["collegecode"];
                        string querymauel = "select * from Manual_schedule where college_code='" + college_code + "'";
                        DataSet ds_save_flag = new DataSet();
                        ds_save_flag = dacces2.select_method(querymauel, hat, "Text");
                        if (ds_save_flag.Tables.Count > 0)
                        {
                            if (ds_save_flag.Tables[0].Rows.Count > 0)//Save Attendance
                            {
                                updatequery1 = "update Manual_schedule set isflag = 1,college_code='" + college_code + "'";
                                int a = dacces2.update_method_wo_parameter(updatequery1, "Text");
                                savefalg = true;//Added by srinath 23/8/2013
                                savevalue = 2;
                            }
                            else
                            {
                                // dcolumn = "d" + str_day + "d" + str_hour + "=" + Att_value;
                                dcolumn1 = "d" + str_day1 + "d" + str_hour1;
                                Insertquery1 = "insert into Manual_schedule(isflag,college_code) values (1,'" + college_code + "')";
                                int d = dacces2.update_method_wo_parameter(Insertquery1, "Text");
                                savefalg = true;//Added by srinath 23/8/2013
                                savevalue = 1;
                            }
                        }
                        string str_Date;
                        string str_rollno;
                        string str_hour;
                        string Atyear;
                        string Atmonth;
                        long strdate;
                        string str_day;
                        string Att_mark;
                        string Att_value;
                        string dcolumn;
                        string Splitmondate;
                        string appNo = string.Empty;
                        str_Date = string.Empty;
                        str_rollno = string.Empty;
                        str_hour = string.Empty;
                        str_day = string.Empty;
                        Att_mark = string.Empty;
                        Att_value = string.Empty;
                        dcolumn = string.Empty;
                        Splitmondate = string.Empty;
                        for (int Att_row = 1; Att_row <= FpSpread2.Sheets[0].RowCount - 3; Att_row++)
                        {
                            insertvalues = string.Empty;
                            updatevalues = string.Empty;
                            monthandyear = string.Empty;
                            string values = string.Empty;
                            bool isDis = false;
                            for (int Att_column = 4; Att_column <= FpSpread2.Sheets[0].ColumnCount - 1; Att_column++)
                            {
                                str_rollno = FpSpread2.Sheets[0].GetText(Att_row, 0).ToString();
                                if (FpSpread2.Sheets[0].Rows[Att_row].Locked)
                                {
                                    isDis = true;
                                }
                                if (!isDis)
                                {
                                    appNo = FpSpread2.Sheets[0].GetTag(Att_row, 1).ToString();
                                    str_Date = FpSpread2.Sheets[0].ColumnHeader.GetClipValue(0, Att_column, 1, 1);
                                    Splitmondate = str_Date.ToString();
                                    string[] split = Splitmondate.Split(new Char[] { '-' });
                                    str_day = split[0].ToString();
                                    Atmonth = split[1].ToString();
                                    Atyear = split[2].ToString();
                                    strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                                    str_hour = FpSpread2.Sheets[0].ColumnHeader.GetClipValue(1, Att_column, 1, 1);
                                    string[] split_hr = str_hour.Split(new Char[] { '-' });
                                    str_hour = str_hour[0].ToString();
                                    Att_mark = Convert.ToString(FpSpread2.GetEditValue(Att_row, Att_column).ToString());
                                    if (Att_mark == "System.Object")
                                    {
                                        Att_mark = FpSpread2.Sheets[0].Cells[Att_row, Att_column].Text.ToString();
                                    }
                                    Att_value = Attvalues(Att_mark);
                                    if (Att_value == "")
                                        Att_value = "0";
                                    if (Att_value != "0")
                                    {
                                        nullflag = true;
                                    }
                                    dcolumn = "d" + str_day + "d" + str_hour;

                                    if (monthandyear == "")
                                    {
                                        monthandyear = strdate.ToString();
                                    }
                                    if (insertvalues == "")
                                    {
                                        insertvalues = dcolumn;
                                        values = Att_value;
                                        updatevalues = dcolumn + "=" + Att_value;
                                    }
                                    else
                                    {
                                        insertvalues = insertvalues + ',' + dcolumn;
                                        values = values + ',' + Att_value;
                                        updatevalues = updatevalues + ',' + dcolumn + "=" + Att_value;
                                    }

                                    if (monthandyear != strdate.ToString() || Att_column == FpSpread2.Sheets[0].ColumnCount - 1)
                                    {
                                        hat.Clear();
                                        hat.Add("Att_App_no", appNo);
                                        hat.Add("Att_CollegeCode", Session["collegecode"].ToString());
                                        hat.Add("rollno", str_rollno);
                                        hat.Add("monthyear", monthandyear);
                                        hat.Add("columnname", insertvalues);
                                        hat.Add("colvalues", values);
                                        hat.Add("coulmnvalue", updatevalues);
                                        insert = dacces2.insert_method("sp_ins_upd_student_attendance_Dead", hat, "sp");
                                        insertvalues = string.Empty;
                                        updatevalues = string.Empty;
                                        monthandyear = string.Empty;
                                        values = string.Empty;
                                        if (monthandyear != strdate.ToString())
                                        {
                                            monthandyear = strdate.ToString();
                                            insertvalues = dcolumn;
                                            values = Att_value;
                                            updatevalues = dcolumn + "=" + Att_value;
                                        }
                                        savefalg = true;
                                        if (Buttonsave.Visible == true)
                                        {
                                            savevalue = 1;
                                        }
                                        else
                                        {
                                            savevalue = 2;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else // start last modify jairam  // 03-09-2014
                    {
                        string savehoursqlstrq;
                        int totalhor;
                        string noofhours_save = string.Empty;
                        string no_firsthalf = string.Empty;
                        string no_secondhalf = string.Empty;
                        string no_minpresent_firsthalf = string.Empty;
                        string no_minpresent_secondhalf = string.Empty;
                        string min_per_day = string.Empty;
                        savehoursqlstrq = "select No_of_hrs_per_day,no_of_hrs_I_half_day,no_of_hrs_II_half_day,min_pres_II_half_day ,min_pres_I_half_day,min_hrs_per_day  from PeriodAttndSchedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + "";
                        ds.Clear();
                        ds = dacces2.select_method_wo_parameter(savehoursqlstrq, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            noofhours_save = ds.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                            no_firsthalf = ds.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
                            no_secondhalf = ds.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString();
                            no_minpresent_firsthalf = ds.Tables[0].Rows[0]["min_pres_I_half_day"].ToString();
                            no_minpresent_secondhalf = ds.Tables[0].Rows[0]["min_pres_II_half_day"].ToString();
                            min_per_day = ds.Tables[0].Rows[0]["min_hrs_per_day"].ToString();
                        }

                        totalhor = Convert.ToInt32(noofhours_save);
                        string str_Date;

                        string str_rollno;
                        string str_hour;
                        string Atyear;
                        string Atmonth;
                        long strdate;
                        string str_day;
                        string Att_mark;
                        string Att_value;
                        string dcolumn;
                        string Splitmondate;
                        string appNo = string.Empty;
                        str_Date = string.Empty;
                        str_rollno = string.Empty;
                        str_hour = string.Empty;
                        str_day = string.Empty;
                        Att_mark = string.Empty;
                        Att_value = string.Empty;
                        dcolumn = string.Empty;
                        Splitmondate = string.Empty;

                        string hourwise = string.Empty;
                        string daywise = string.Empty;
                        string hourwisedata = string.Empty;
                        string daywisedata = string.Empty;

                        string minimum = string.Empty;
                        string minimun_data = string.Empty;

                        string settingquery = string.Empty;
                        settingquery = "select TextName,Taxtval from Attendance_Settings where  College_Code ='" + Session["collegecode"].ToString() + "'and user_id='" + Session["usercode"].ToString() + "'";
                        settingquery = settingquery + " ;";//select * from attendance

                        //Added by Idhris
                        string addSection = string.Empty;
                        if (ddlsec.SelectedIndex > 0)
                        {
                            addSection = "  and r.sections='" + ddlsec.SelectedValue.Trim() + "' ";
                        }
                        settingquery += "select a.roll_no,a.month_year,d1d1,d1d2,d1d3,d1d4,d1d5,d1d6,d1d7,d1d8,d1d9,d1d10,d2d1,d2d2,d2d3,d2d4,d2d5,d2d6,d2d7,d2d8,d2d9,d2d10,d3d1,d3d2,d3d3,d3d4,d3d5,d3d6,d3d7,d3d8,d3d9,d3d10,d4d1,d4d2,d4d3,d4d4,d4d5,d4d6,d4d7,d4d8,d4d9,d4d10,d5d1,d5d2,d5d3,d5d4,d5d5,d5d6,d5d7,d5d8,d5d9,d5d10,d6d1,d6d2,d6d3,d6d4,d6d5,d6d6,d6d7,d6d8,d6d9,d6d10,d7d1,d7d2,d7d3,d7d4,d7d5,d7d6,d7d7,d7d8,d7d9,d7d10,d8d1,d8d2,d8d3,d8d4,d8d5,d8d6,d8d7,d8d8,d8d9,d8d10,d9d1,d9d2,d9d3,d9d4,d9d5,d9d6,d9d7,d9d8,d9d9,d9d10,d10d1,d10d2,d10d3,d10d4,d10d5,d10d6,d10d7,d10d8,d10d9,d10d10,d11d1,d11d2,d11d3,d11d4,d11d5,d11d6,d11d7,d11d8,d11d9,d11d10,d12d1,d12d2,d12d3,d12d4,d12d5,d12d6,d12d7,d12d8,d12d9,d12d10,d13d1,d13d2,d13d3,d13d4,d13d5,d13d6,d13d7,d13d8,d13d9,d13d10,d14d1,d14d2,d14d3,d14d4,d14d5,d14d6,d14d7,d14d8,d14d9,d14d10,d15d1,d15d2,d15d3,d15d4,d15d5,d15d6,d15d7,d15d8,d15d9,d15d10,d16d1,d16d2,d16d3,d16d4,d16d5,d16d6,d16d7,d16d8,d16d9,d16d10,d17d1,d17d2,d17d3,d17d4,d17d5,d17d6,d17d7,d17d8,d17d9,d17d10,d18d1,d18d2,d18d3,d18d4,d18d5,d18d6,d18d7,d18d8,d18d9,d18d10,d19d1,d19d2,d19d3,d19d4,d19d5,d19d6,d19d7,d19d8,d19d9,d19d10,d20d1,d20d2,d20d3,d20d4,d20d5,d20d6,d20d7,d20d8,d20d9,d20d10,d21d1,d21d2,d21d3,d21d4,d21d5,d21d6,d21d7,d21d8,d21d9,d21d10,d22d1,d22d2,d22d3,d22d4,d22d5,d22d6,d22d7,d22d8,d22d9,d22d10,d23d1,d23d2,d23d3,d23d4,d23d5,d23d6,d23d7,d23d8,d23d9,d23d10,d24d1,d24d2,d24d3,d24d4,d24d5,d24d6,d24d7,d24d8,d24d9,d24d10,d25d1,d25d2,d25d3,d25d4,d25d5,d25d6,d25d7,d25d8,d25d9,d25d10,d26d1,d26d2,d26d3,d26d4,d26d5,d26d6,d26d7,d26d8,d26d9,d26d10,d27d1,d27d2,d27d3,d27d4,d27d5,d27d6,d27d7,d27d8,d27d9,d27d10,d28d1,d28d2,d28d3,d28d4,d28d5,d28d6,d28d7,d28d8,d28d9,d28d10,d29d1,d29d2,d29d3,d29d4,d29d5,d29d6,d29d7,d29d8,d29d9,d29d10,d30d1,d30d2,d30d3,d30d4,d30d5,d30d6,d30d7,d30d8,d30d9,d30d10,d31d1,d31d2,d31d3,d31d4,d31d5,d31d6,d31d7,d31d8,d31d9,d31d10,a.Att_App_no,a.Att_CollegeCode from attendance a,Registration r where Att_App_no = r.App_No and r.degree_code  in ( '" + ddlbranch.SelectedValue.ToString() + "') " + addSection;
                        //End
                        ds.Clear();
                        ds = dacces2.select_method_wo_parameter(settingquery, "Text");
                        DataTable dtattenda = ds.Tables[1];
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            ArrayList addarray = new ArrayList();
                            DataView dv_demand_data = new DataView();
                            ds.Tables[0].DefaultView.RowFilter = "TextName in ('Hour','Day','Minimun Absent Day','Minimun Days')";
                            dv_demand_data = ds.Tables[0].DefaultView;
                            if (dv_demand_data.Count > 0)
                            {
                                for (int i = 0; i < dv_demand_data.Count; i++)
                                {
                                    if (dv_demand_data[i]["TextName"].ToString() == "Hour" && Convert.ToInt32(dv_demand_data[i]["Taxtval"]) == 1)
                                    {
                                        hourwise = "1";
                                        hourwisedata = "Hour";
                                    }
                                    else if (dv_demand_data[i]["TextName"].ToString() == "Hour" && Convert.ToInt32(dv_demand_data[i]["Taxtval"]) == 0)
                                    {
                                        hourwise = "0";
                                    }
                                    if (dv_demand_data[i]["TextName"].ToString() == "Day" && Convert.ToInt32(dv_demand_data[i]["Taxtval"]) == 1)
                                    {
                                        daywise = "1";
                                        daywisedata = "Day";
                                    }
                                    else if (dv_demand_data[i]["TextName"].ToString() == "Day" && Convert.ToInt32(dv_demand_data[i]["Taxtval"]) == 0)
                                    {
                                        daywise = "0";
                                    }
                                    if (dv_demand_data[i]["TextName"].ToString() == "Minimun Absent Day" && Convert.ToInt32(dv_demand_data[i]["Taxtval"]) == 1)
                                    {
                                        minimum = "1";
                                        minimun_data = "Minimun Absent Day";
                                    }
                                    else if (dv_demand_data[i]["TextName"].ToString() == "Minimun Absent Day" && Convert.ToInt32(dv_demand_data[i]["Taxtval"]) == 0)
                                    {
                                        minimum = "0";
                                    }
                                    if (dv_demand_data[i]["TextName"].ToString() == "Minimun Days" && Convert.ToString(dv_demand_data[i]["Taxtval"]) != "")
                                    {
                                        minimum_day = Convert.ToString(dv_demand_data[i]["Taxtval"]);
                                    }
                                    else if (dv_demand_data[i]["TextName"].ToString() == "Minimun Days" && Convert.ToString(dv_demand_data[i]["Taxtval"]) == "")
                                    {
                                        minimum_day = string.Empty;
                                    }
                                }
                            }
                        }
                        present_calcflag.Clear();
                        absent_calcflag.Clear();
                        hat.Clear();
                        hat.Add("colege_code", Session["collegecode"].ToString());
                        ds_attndmaster = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
                        count_master = (ds_attndmaster.Tables[0].Rows.Count);
                        if (count_master > 0)
                        {
                            for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                            {

                                if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                                {
                                    present_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());

                                }
                                if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                                {
                                    absent_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                                }
                            }
                        }

                        int startsem_date = 0;
                        string start_Date = string.Empty;

                        string startdatequery = string.Empty;
                        startdatequery = "select leavecode from AttMasterSetting where calcflag='2' and collegecode=" + Session["collegecode"].ToString() + "";
                        startdatequery = startdatequery + " select convert(varchar(10),start_date,103) as start_date from seminfo where  degree_code='" + ddlbranch.SelectedValue.ToString() + "' and semester='" + ddlsem.SelectedValue.ToString() + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "'";
                        startdatequery = startdatequery + " select convert(varchar(10),holiday_date,103)as holiday_date ,halforfull,morning,evening from holidayStudents where degree_code ='" + ddlbranch.SelectedValue.ToString() + "' and semester ='" + ddlsem.SelectedValue.ToString() + "'";
                        data1.Clear();
                        data1 = dacces2.select_method_wo_parameter(startdatequery, "Text");
                        if (data1.Tables.Count > 0 && data1.Tables[0].Rows.Count > 0)
                        {
                            for (int val = 0; val < data1.Tables[0].Rows.Count; val++)
                            {
                                notarray.Add(data1.Tables[0].Rows[val]["leavecode"].ToString());
                            }
                        }
                        if (data1.Tables.Count > 1 && data1.Tables[1].Rows.Count > 0)
                        {
                            start_Date = data1.Tables[1].Rows[0]["start_date"].ToString();
                            string[] split = start_Date.Split(new Char[] { '/' });
                            string str_day1 = split[0].ToString();
                            string Atmonth1 = split[1].ToString();
                            string Atyear1 = split[2].ToString();
                            startsem_date = (Convert.ToInt32(Atmonth1) + Convert.ToInt32(Atyear1) * 12);
                        }

                        if (data1.Tables.Count > 2 && data1.Tables[2].Rows.Count > 0)
                        {
                            for (int hol = 0; hol < data1.Tables[2].Rows.Count; hol++)
                            {
                                string date = data1.Tables[2].Rows[hol]["holiday_date"].ToString();
                                string conncetion = data1.Tables[2].Rows[hol]["holiday_date"].ToString() + "*" + data1.Tables[2].Rows[hol]["halforfull"].ToString() + "*" + data1.Tables[2].Rows[hol]["morning"].ToString() + "*" + data1.Tables[2].Rows[hol]["evening"].ToString();
                                if (!holiday.ContainsKey(date))
                                {
                                    holiday.Add(date, conncetion);
                                }
                            }
                        }


                        int total_conduct_hour = 0;
                        int absent_hour = 0;
                        bool noentryflag = false;
                        for (int Att_row = 1; Att_row <= FpSpread2.Sheets[0].RowCount - 3; Att_row++)
                        {
                            insertvalues = string.Empty;
                            updatevalues = string.Empty;
                            monthandyear = string.Empty;
                            string values = string.Empty;
                            string existattndval = string.Empty;

                            int colcount1 = 0;
                            string getvalue = string.Empty;
                            bool isdis = false;
                            if (FpSpread2.Sheets[0].Rows[Att_row].Locked)
                            {
                                isdis = true;
                            }
                            if (!isdis)
                            {
                                str_rollno = FpSpread2.Sheets[0].GetText(Att_row, 1).ToString();
                                appNo = FpSpread2.Sheets[0].GetTag(Att_row, 1).ToString();
                                for (int Att_column = 5; Att_column <= FpSpread2.Sheets[0].ColumnCount - 1; Att_column++)
                                {
                                    colcount1++;
                                    str_Date = FpSpread2.Sheets[0].ColumnHeader.GetClipValue(0, Att_column, 1, 1);
                                    string[] tmpdate = str_Date.ToString().Split(new char[] { ' ' });
                                    str_Date = tmpdate[0].ToString();

                                    Splitmondate = str_Date.ToString();
                                    string[] split = Splitmondate.Split(new Char[] { '-' });
                                    str_day = split[0].ToString();
                                    Atmonth = split[1].ToString();
                                    Atyear = split[2].ToString();
                                    strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                                    str_hour = FpSpread2.Sheets[0].ColumnHeader.GetClipValue(1, Att_column, 1, 1);
                                    string[] split_hr = str_hour.Split(new Char[] { '-' });
                                    str_hour = str_hour[0].ToString();
                                    Att_mark = Convert.ToString(FpSpread2.GetEditValue(Att_row, Att_column));
                                    if (Att_mark == "System.Object")
                                    {
                                        Att_mark = FpSpread2.Sheets[0].Cells[Att_row, Att_column].Text.ToString();
                                        getvalue = Attvalues(FpSpread2.Sheets[0].Cells[Att_row, Att_column].Text.ToString());
                                    }
                                    dcolumn = "d" + str_day + "d" + str_hour;
                                    Att_value = Attvalues(Att_mark);
                                    getvalue = Att_value;
                                    if (Att_value == "")
                                    {
                                        Att_value = "0";
                                    }
                                    if (Att_value != "0")
                                    {
                                        nullflag = true;
                                    }
                                    if (minimum != "1")
                                    {
                                        if (hourwise == "1")
                                        {
                                            if (absent_calcflag.Count > 0)
                                            {
                                                if (absent_calcflag.Contains(getvalue) == true)
                                                {
                                                    string value_return = web.coundected_hour(strdate, startsem_date, str_rollno, absent_calcflag, notarray);
                                                    if (value_return == "Empty")
                                                    {
                                                        total_conduct_hour = 1;
                                                        absent_hour = 1;
                                                    }
                                                    else
                                                    {
                                                        string[] splitvalue = value_return.Split('-');
                                                        if (splitvalue.Length > 0)
                                                        {
                                                            if (splitvalue[0].ToString() != "")
                                                            {
                                                                total_conduct_hour = Convert.ToInt32(splitvalue[0]);
                                                                total_conduct_hour++;
                                                            }
                                                            else
                                                            {
                                                                total_conduct_hour++;
                                                            }
                                                            if (splitvalue[1].ToString() != "")
                                                            {
                                                                absent_hour = Convert.ToInt32(splitvalue[1]);
                                                                absent_hour++;
                                                            }
                                                            else
                                                            {
                                                                absent_hour++;
                                                            }
                                                        }
                                                    }
                                                    SendingSms(str_rollno, Splitmondate, str_hour, collacronym, coursename, hourwisedata, total_conduct_hour, absent_hour);
                                                    sendvoicecall(str_rollno, Splitmondate, str_hour, ddlbatch.SelectedValue.ToString(), ddlbranch.SelectedItem.Value.ToString(), collegename, coursename, hourwisedata);
                                                }
                                            }
                                        }
                                    }

                                    if (insertvalues == "")
                                    {
                                        insertvalues = dcolumn;
                                        values = Att_value;
                                        updatevalues = dcolumn + "=" + Att_value;
                                    }
                                    else
                                    {
                                        insertvalues = insertvalues + ',' + dcolumn;
                                        values = values + ',' + Att_value;
                                        updatevalues = updatevalues + ',' + dcolumn + "=" + Att_value;
                                    }
                                    dtattenda.DefaultView.RowFilter = " roll_no='" + str_rollno + "' and month_year='" + strdate + "'";
                                    DataView dvstuattmon = dtattenda.DefaultView;
                                    if (dvstuattmon.Count > 0)
                                    {
                                        string setval = dvstuattmon[0][dcolumn].ToString();
                                        if (existattndval == "")
                                        {
                                            existattndval = dcolumn + "=" + setval;
                                        }
                                        else
                                        {
                                            existattndval = existattndval + ',' + dcolumn + "=" + setval;
                                        }
                                    }
                                    else
                                    {
                                        string setval = "0";
                                        if (existattndval == "")
                                        {
                                            existattndval = dcolumn + "=" + setval;
                                        }
                                        else
                                        {
                                            existattndval = existattndval + ',' + dcolumn + "=" + setval;
                                        }
                                    }
                                    if (monthandyear == "")
                                    {
                                        monthandyear = strdate.ToString();
                                    }
                                    if (monthandyear != strdate.ToString() || Att_column == FpSpread2.Sheets[0].ColumnCount - 1)
                                    {
                                        if (existattndval != updatevalues)
                                        {
                                            hat.Clear();
                                            hat.Add("Att_App_no", appNo);
                                            hat.Add("Att_CollegeCode", Session["collegecode"].ToString());
                                            hat.Add("rollno", str_rollno);
                                            hat.Add("monthyear", monthandyear);
                                            hat.Add("columnname", insertvalues);
                                            hat.Add("colvalues", values);
                                            hat.Add("coulmnvalue", updatevalues);
                                            insert = dacces2.insert_method("sp_ins_upd_student_attendance_Dead", hat, "sp");
                                            noentryflag = true;
                                        }
                                        insertvalues = string.Empty;
                                        updatevalues = string.Empty;
                                        monthandyear = string.Empty;
                                        values = string.Empty;
                                        existattndval = string.Empty;
                                        if (monthandyear != strdate.ToString())
                                        {
                                            monthandyear = strdate.ToString();
                                            insertvalues = dcolumn;
                                            values = Att_value;
                                            updatevalues = dcolumn + "=" + Att_value;
                                        }
                                        savefalg = true;
                                        if (Buttonsave.Visible == true)
                                        {
                                            savevalue = 1;
                                        }
                                        else
                                        {
                                            savevalue = 2;
                                        }
                                    }

                                }


                                #region Added by Idhris Attendance Insert New Table 29-12-2016

                                string[] frDateArr = txtFromDate.Text.Split('-');
                                string[] toDateArr = TxtToDate.Text.Split('-');
                                DateTime frmDte = Convert.ToDateTime(frDateArr[1] + "/" + frDateArr[0] + "/" + frDateArr[2]);
                                DateTime toDte = Convert.ToDateTime(toDateArr[1] + "/" + toDateArr[0] + "/" + toDateArr[2]);

                                for (DateTime dtFrom = frmDte; dtFrom <= toDte; dtFrom = dtFrom.AddDays(1))
                                {
                                    str_Date = dtFrom.ToString("dd-MM-yyyy");
                                    string[] tmpdate = str_Date.ToString().Split(new char[] { ' ' });
                                    str_Date = tmpdate[0].ToString();

                                    Splitmondate = str_Date.ToString();
                                    string[] split = Splitmondate.Split(new Char[] { '-' });
                                    str_day = split[0].ToString();
                                    Atmonth = split[1].ToString();
                                    Atyear = split[2].ToString();
                                    strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);

                                    StringBuilder sb_aattddaayy = new StringBuilder();
                                    for (int hrsI = 1; hrsI <= Convert.ToInt32(noofhours_save); hrsI++)
                                    {
                                        sb_aattddaayy.Append("d" + str_day + "d" + hrsI + ",");
                                    }
                                    if (sb_aattddaayy.Length > 1)
                                    {
                                        sb_aattddaayy.Remove(sb_aattddaayy.Length - 1, 1);
                                    }
                                    attendanceMark(appNo, (int)strdate, sb_aattddaayy.ToString(), Convert.ToInt32(noofhours_save), Convert.ToInt32(no_firsthalf), Convert.ToInt32(no_secondhalf), Convert.ToInt32(no_minpresent_firsthalf), Convert.ToInt32(no_minpresent_secondhalf), dtFrom.ToString("MM/dd/yyyy").ToString(), Session["collegecode"].ToString());
                                }

                                #endregion

                                if (minimum != "1")
                                {
                                    if (daywise == "1")
                                    {
                                        string fromdate = txtFromDate.Text;
                                        string todate = TxtToDate.Text;
                                        string[] fromdatesplit = fromdate.Split('-');
                                        string[] todatesplit = todate.Split('-');
                                        DateTime newfromdate = Convert.ToDateTime(fromdatesplit[1].ToString() + "/" + fromdatesplit[0].ToString() + "/" + fromdatesplit[2].ToString());
                                        DateTime newtodate = Convert.ToDateTime(todatesplit[1].ToString() + "/" + todatesplit[0].ToString() + "/" + todatesplit[2].ToString());
                                        TimeSpan dt = new TimeSpan();
                                        dt = newtodate.Subtract(newfromdate);
                                        int days = dt.Days;
                                        if (days == 0)
                                        {
                                            string newdate = newtodate.ToString("dd/MM/yyyy");
                                            string[] newdatesplit = newdate.Split('/');
                                            string date_value = newdatesplit[0].ToString();
                                            date_value = date_value.TrimStart('0');
                                            string date_value_table = "d" + date_value;
                                            string month_value = newdatesplit[1].ToString();
                                            string year_value = newdatesplit[2].ToString();
                                            string monty_year_value = Convert.ToString((Convert.ToInt32(year_value) * 12 + Convert.ToInt32(month_value)));
                                            string date_value_table_day = string.Empty;
                                            for (int k = 1; k <= totalhor; k++)
                                            {
                                                if (date_value_table_day == "")
                                                {
                                                    date_value_table_day = date_value_table + "d" + k;
                                                }
                                                else
                                                {
                                                    date_value_table_day = date_value_table_day + "," + date_value_table + "d" + k;
                                                }
                                            }

                                            int split_day_hour = 0;
                                            int first_split_present = 0;
                                            int second_split_absent = 0;
                                            int notconsider = 0;
                                            int first_split_absent = 0;
                                            int second_split_present = 0;
                                            int firstempty_count = 0;
                                            int secondempty_count = 0;
                                            bool attendflag = false;
                                            string absent_count_query = string.Empty;
                                            absent_count_query = "Select " + date_value_table_day + " from attendance where roll_no ='" + str_rollno + "'and month_year in ('" + monty_year_value + "')";
                                            ds.Clear();
                                            ds = dacces2.select_method_wo_parameter(absent_count_query, "Text");
                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                            {
                                                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                                                {
                                                    split_day_hour++;
                                                    string attendvalue = Convert.ToString(ds.Tables[0].Rows[0][j]);
                                                    if (attendvalue != "")
                                                    {
                                                        if (present_calcflag.Count > 0)
                                                        {
                                                            if (split_day_hour <= Convert.ToInt32(no_firsthalf))
                                                            {

                                                                if (present_calcflag.Contains(attendvalue) == true)
                                                                {
                                                                    first_split_present++;

                                                                }
                                                                else if (absent_calcflag.Contains(attendvalue) == true)
                                                                {
                                                                    first_split_absent++;
                                                                }
                                                                else if (attendvalue == "" || attendvalue == "0" || attendvalue == null || attendvalue == "H")
                                                                {
                                                                    firstempty_count++;
                                                                }
                                                                else
                                                                {
                                                                    notconsider++;
                                                                }
                                                            }
                                                            else
                                                            {

                                                                if (present_calcflag.Contains(attendvalue) == true)
                                                                {
                                                                    second_split_present++;
                                                                }
                                                                else if (absent_calcflag.Contains(attendvalue) == true)
                                                                {
                                                                    second_split_absent++;
                                                                }
                                                                else if (attendvalue == "" || attendvalue == null || attendvalue == "0" || attendvalue == "H")
                                                                {
                                                                    secondempty_count++;
                                                                }
                                                                else
                                                                {
                                                                    notconsider++;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            attendflag = true;
                                                        }
                                                    }
                                                }

                                                if (attendflag == false)
                                                {
                                                    if (firstempty_count < Convert.ToInt32(no_minpresent_firsthalf))
                                                    {
                                                        if (secondempty_count < Convert.ToInt32(no_minpresent_secondhalf))
                                                        {

                                                            if (first_split_present < Convert.ToInt32(no_minpresent_firsthalf) && second_split_present < Convert.ToInt32(no_minpresent_secondhalf))
                                                            {
                                                                if (first_split_absent != 0 && second_split_absent != 0)
                                                                {
                                                                    string days_count = web.condected_days(start_Date, fromdate, str_rollno, absent_calcflag, notarray, totalhor, no_firsthalf, no_secondhalf, present_calcflag, no_minpresent_firsthalf, no_minpresent_secondhalf, holiday);
                                                                    if (days_count == "Empty")
                                                                    {
                                                                        total_conduct_hour = 1;
                                                                        absent_hour = 1;
                                                                    }
                                                                    else
                                                                    {
                                                                        string[] splitvalue = days_count.Split('-');
                                                                        if (splitvalue.Length > 0)
                                                                        {
                                                                            if (splitvalue[0].ToString() != "")
                                                                            {
                                                                                total_conduct_hour = Convert.ToInt32(splitvalue[0]);

                                                                            }
                                                                            else
                                                                            {
                                                                                total_conduct_hour++;
                                                                            }
                                                                            if (splitvalue[1].ToString() != "")
                                                                            {
                                                                                absent_hour = Convert.ToInt32(splitvalue[1]);

                                                                            }
                                                                            else
                                                                            {
                                                                                absent_hour++;
                                                                            }
                                                                        }

                                                                    }
                                                                    SendingSms(str_rollno, Splitmondate, str_hour, collacronym, coursename, daywisedata, total_conduct_hour, absent_hour);
                                                                    sendvoicecall(str_rollno, Splitmondate, str_hour, ddlbatch.SelectedValue.ToString(), ddlbranch.SelectedItem.Value.ToString(), collegename, coursename, daywisedata);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }


                                            }

                                        }
                                        if (days > 0)
                                        {
                                            while (newfromdate <= newtodate)
                                            {
                                                if (newfromdate.ToString("dddd") != "Sunday")
                                                {
                                                    string new_date = newfromdate.ToString("dd/MM/yyyy");
                                                    string[] new_split_date = new_date.Split('/');
                                                    string new_date_from = new_split_date[0].ToString();
                                                    new_date_from = new_date_from.TrimStart('0');
                                                    string new_date_from_table = "d" + new_date_from;
                                                    string new_date_month = new_split_date[1].ToString();
                                                    string new_date_year = new_split_date[2].ToString();
                                                    string new_date_monthyear = Convert.ToString((Convert.ToInt32(new_date_year) * 12 + Convert.ToInt32(new_date_month)));
                                                    string date_value_table_day = string.Empty;
                                                    for (int k = 1; k <= totalhor; k++)
                                                    {
                                                        if (date_value_table_day == "")
                                                        {
                                                            date_value_table_day = new_date_from_table + "d" + k;
                                                        }
                                                        else
                                                        {
                                                            date_value_table_day = date_value_table_day + "," + new_date_from_table + "d" + k;
                                                        }

                                                    }
                                                    int split_day_hour = 0;
                                                    int first_split_present = 0;
                                                    int second_split_present = 0;
                                                    int first_split_absent = 0;
                                                    int second_split_absent = 0;
                                                    int firstempty_count = 0;
                                                    int secondempty_count = 0;
                                                    string absent_count_query = string.Empty;
                                                    bool attendflag = false;
                                                    absent_count_query = "Select " + date_value_table_day + " from attendance where roll_no ='" + str_rollno + "'and month_year in ('" + new_date_monthyear + "')";
                                                    ds.Clear();
                                                    ds = dacces2.select_method_wo_parameter(absent_count_query, "Text");
                                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                                                        {
                                                            split_day_hour++;
                                                            string attendvalue = Convert.ToString(ds.Tables[0].Rows[0][j]);
                                                            if (attendvalue != "")
                                                            {
                                                                if (present_calcflag.Count > 0)
                                                                {
                                                                    if (split_day_hour <= Convert.ToInt32(no_firsthalf))
                                                                    {
                                                                        if (present_calcflag.Contains(attendvalue) == true)
                                                                        {
                                                                            first_split_present++;
                                                                        }
                                                                        else if (absent_calcflag.Contains(attendvalue) == true)
                                                                        {
                                                                            first_split_absent++;
                                                                        }
                                                                        else if (attendvalue == "" || attendvalue == "0" || attendvalue == null || attendvalue == "H")
                                                                        {
                                                                            firstempty_count++;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (present_calcflag.Contains(attendvalue) == true)
                                                                        {
                                                                            second_split_present++;
                                                                        }
                                                                        else if (absent_calcflag.Contains(attendvalue) == true)
                                                                        {
                                                                            second_split_absent++;
                                                                        }
                                                                        if (attendvalue == "" || attendvalue == null || attendvalue == "0" || attendvalue == "H")
                                                                        {
                                                                            secondempty_count++;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    attendflag = true;
                                                                }
                                                            }
                                                        }
                                                        if (attendflag == false)
                                                        {
                                                            if (firstempty_count < Convert.ToInt32(no_minpresent_firsthalf) && secondempty_count < Convert.ToInt32(no_minpresent_secondhalf))
                                                            {
                                                                if (first_split_present < Convert.ToInt32(no_minpresent_firsthalf) && second_split_present < Convert.ToInt32(no_minpresent_secondhalf))
                                                                {
                                                                    if (first_split_absent != 0 && second_split_absent != 0)
                                                                    {
                                                                        string days_count = web.condected_days(start_Date, fromdate, str_rollno, absent_calcflag, notarray, totalhor, no_firsthalf, no_secondhalf, present_calcflag, no_minpresent_firsthalf, no_minpresent_secondhalf, holiday);
                                                                        if (days_count == "Empty")
                                                                        {
                                                                            total_conduct_hour = 1;
                                                                            absent_hour = 1;
                                                                        }
                                                                        else
                                                                        {
                                                                            string[] splitvalue = days_count.Split('-');
                                                                            if (splitvalue.Length > 0)
                                                                            {
                                                                                if (splitvalue[0].ToString() != "")
                                                                                {
                                                                                    total_conduct_hour = Convert.ToInt32(splitvalue[0]);
                                                                                }
                                                                                else
                                                                                {
                                                                                    total_conduct_hour++;
                                                                                }
                                                                                if (splitvalue[1].ToString() != "")
                                                                                {
                                                                                    absent_hour = Convert.ToInt32(splitvalue[1]);
                                                                                }
                                                                                else
                                                                                {
                                                                                    absent_hour++;
                                                                                }
                                                                            }
                                                                        }
                                                                        SendingSms(str_rollno, Splitmondate, str_hour, collacronym, coursename, daywisedata, total_conduct_hour, absent_hour);
                                                                        sendvoicecall(str_rollno, Splitmondate, str_hour, ddlbatch.SelectedValue.ToString(), ddlbranch.SelectedItem.Value.ToString(), collegename, coursename, daywisedata);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    newfromdate = newfromdate.AddDays(1);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (minimum == "1")
                                {
                                    if (minimum_day != "")
                                    {
                                        int parentsmeat = 0;
                                        int firsthalf = 0;
                                        int secondhalf = 0;
                                        int firsthalfabsent = 0;
                                        int secondhalfabent = 0;
                                        int firstemptycount = 0;
                                        int secondemptycount = 0;
                                        int emptycount = 0;
                                        string mini_absent_days = minimum_day.ToString();
                                        string date = TxtToDate.Text;
                                        string[] splitdate = date.Split('-');
                                        DateTime statrtdate = Convert.ToDateTime(splitdate[1].ToString() + "/" + splitdate[0].ToString() + "/" + splitdate[2].ToString());
                                        ArrayList addmonthyear = new ArrayList();
                                        ArrayList adddate = new ArrayList();
                                        Hashtable addmonthdate = new Hashtable();
                                        DataSet dsholidy = new DataSet();
                                        DataView dv = new DataView();
                                        string hollydayquery = string.Empty;
                                        hollydayquery = "select * from holidayStudents where degree_code=" + ddldegree.SelectedItem.Value + " and semester=" + ddlsem.SelectedItem.Value + "";
                                        dsholidy.Clear();
                                        dsholidy = dacces2.select_method_wo_parameter(hollydayquery, "Text");

                                        for (int i = 0; i < Convert.ToInt32(mini_absent_days); i++)
                                        {
                                            if (dsholidy.Tables[0].Rows.Count > 0)
                                            {
                                                dv.Table.DefaultView.RowFilter = "holiday_date='" + statrtdate.ToString("MM/dd/yyyy") + "'";
                                                if (dv.Count > 0)
                                                {
                                                    string holidayvalue = Convert.ToString(dv[0]["halforfull"]);
                                                    if (holidayvalue != "")
                                                    {
                                                        if (holidayvalue == "0" || holidayvalue == "1")
                                                        {
                                                            i--;
                                                        }
                                                    }
                                                }
                                                if (statrtdate.ToString("dddd") != "Sunday")
                                                {
                                                    string date1 = statrtdate.ToString("dd/MM/yyyy");
                                                    countarray.Add(date1);
                                                    string[] splitdate1 = date1.Split('/');
                                                    string firstdate = splitdate1[0].ToString();
                                                    firstdate = firstdate.TrimStart('0');
                                                    string finaldate = "d" + firstdate;
                                                    adddate.Add(finaldate);
                                                    string firstmonth = splitdate1[1].ToString();
                                                    string firstyear = splitdate1[2].ToString();
                                                    string datevalue = Convert.ToString((Convert.ToInt32(firstyear) * 12 + Convert.ToInt32(firstmonth)));

                                                    addmonthyear.Add((datevalue));
                                                }
                                                else
                                                {
                                                    i--;
                                                }

                                            }
                                            else
                                            {
                                                if (statrtdate.ToString("dddd") != "Sunday")
                                                {
                                                    string date1 = statrtdate.ToString("dd/MM/yyyy");
                                                    countarray.Add(date1);
                                                    string[] splitdate1 = date1.Split('/');
                                                    string firstdate = splitdate1[0].ToString();
                                                    firstdate = firstdate.TrimStart('0');
                                                    string finaldate = "d" + firstdate;
                                                    adddate.Add(finaldate);
                                                    string firstmonth = splitdate1[1].ToString();
                                                    string firstyear = splitdate1[2].ToString();
                                                    string datevalue = Convert.ToString((Convert.ToInt32(firstyear) * 12 + Convert.ToInt32(firstmonth)));

                                                    addmonthyear.Add((datevalue));
                                                }
                                                else
                                                {
                                                    i--;
                                                }
                                            }
                                            statrtdate = statrtdate.AddDays(-1);
                                        }
                                        if (adddate.Count > 0)
                                        {
                                            for (int i = 0; i < adddate.Count; i++)
                                            {
                                                string value = adddate[i].ToString();
                                                string dayvalue = string.Empty;
                                                if (totalhor > 0)
                                                {
                                                    for (int k = 1; k <= totalhor; k++)
                                                    {
                                                        if (dayvalue == "")
                                                        {
                                                            dayvalue = value + "d" + k;
                                                        }
                                                        else
                                                        {
                                                            dayvalue = dayvalue + "," + value + "d" + k;
                                                        }
                                                    }
                                                    bool attendflag = false;
                                                    int dscolcount = 0;
                                                    string attendancequery = string.Empty;
                                                    attendancequery = "Select " + dayvalue + " from attendance where roll_no ='" + str_rollno + "'and month_year in ('" + addmonthyear[i].ToString() + "')";
                                                    ds.Clear();
                                                    ds = dacces2.select_method_wo_parameter(attendancequery, "Text");
                                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                                                        {
                                                            dscolcount++;
                                                            string attendvalue = Convert.ToString(ds.Tables[0].Rows[0][j]);
                                                            if (attendvalue != "")
                                                            {
                                                                if (present_calcflag.Count > 0)
                                                                {

                                                                    if (dscolcount <= Convert.ToInt32(no_firsthalf))
                                                                    {
                                                                        if (present_calcflag.Contains(attendvalue) == true)
                                                                        {
                                                                            firsthalf++;

                                                                        }
                                                                        else if (absent_calcflag.Contains(attendvalue) == true)
                                                                        {
                                                                            firsthalfabsent++;

                                                                        }
                                                                        else if (attendvalue == "" || attendvalue == "0" || attendvalue == null || attendvalue == "H")
                                                                        {
                                                                            firstemptycount++;
                                                                        }
                                                                    }
                                                                    else
                                                                    {

                                                                        if (present_calcflag.Contains(attendvalue) == true)
                                                                        {
                                                                            secondhalf++;
                                                                        }
                                                                        else if (absent_calcflag.Contains(attendvalue) == true)
                                                                        {
                                                                            secondhalfabent++;

                                                                        }
                                                                        else if (attendvalue == "" || attendvalue == null || attendvalue == "0" || attendvalue == "H")
                                                                        {
                                                                            secondemptycount++;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    attendflag = true;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                emptycount++;
                                                            }
                                                        }
                                                        if (attendflag == false)
                                                        {
                                                            if (emptycount < Convert.ToInt32(min_per_day)) ;
                                                            if (firstemptycount < Convert.ToInt32(no_minpresent_firsthalf) && secondemptycount < Convert.ToInt32(no_minpresent_secondhalf))
                                                            {
                                                                if (firsthalf < Convert.ToInt32(no_minpresent_firsthalf) && secondhalf < Convert.ToInt32(no_minpresent_secondhalf))
                                                                {
                                                                    if (firsthalfabsent != 0 && secondhalfabent != 0)
                                                                    {
                                                                        parentsmeat++;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (Convert.ToInt32(mini_absent_days) == parentsmeat)
                                        {
                                            DateTime datesnowcc = DateTime.Now;
                                            string purpose01 = "" + mini_absent_days + "  Days Absent Parents Meet ";
                                            string days_count = web.condected_days(start_Date, txtFromDate.Text, str_rollno, absent_calcflag, notarray, totalhor, no_firsthalf, no_secondhalf, present_calcflag, no_minpresent_firsthalf, no_minpresent_secondhalf, holiday);
                                            if (days_count == "Empty")
                                            {
                                                total_conduct_hour = 0;
                                                absent_hour = 0;
                                            }
                                            else
                                            {
                                                string[] splitvalue = days_count.Split('-');
                                                if (splitvalue.Length > 0)
                                                {
                                                    if (splitvalue[0].ToString() != "")
                                                    {
                                                        total_conduct_hour = Convert.ToInt32(splitvalue[0]);

                                                    }
                                                    else
                                                    {
                                                        total_conduct_hour++;
                                                    }
                                                    if (splitvalue[1].ToString() != "")
                                                    {
                                                        absent_hour = Convert.ToInt32(splitvalue[1]);

                                                    }
                                                    else
                                                    {
                                                        absent_hour++;
                                                    }
                                                }

                                            }
                                            SendingSms(str_rollno, Splitmondate, str_hour, collacronym, coursename, daywisedata, total_conduct_hour, absent_hour);
                                            parentsmeet(str_rollno, datesnowcc, purpose01);//added by sridhar 13 aug 2014
                                        }

                                    }


                                }
                            }
                        }
                        if (nullflag == true)
                        {
                            lblset.Visible = false;

                        }
                        else
                        {
                            lblset.Visible = true;
                            lblset.Text = "Mark Attendance For Save";
                        }

                        for (Att_mark_column = 5; Att_mark_column < FpSpread2.Sheets[0].ColumnCount; Att_mark_column++)
                        {
                            absent_count = 0;
                            present_count = 0;

                            for (Att_mark_row = 1; Att_mark_row < FpSpread2.Sheets[0].RowCount - 2; Att_mark_row++)
                            {
                                if (FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text.Trim() != "" && FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text != null) //condn 09.08.12 mythili
                                {
                                    string getvalue = Attvalues(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text.ToString());
                                    if (present_calcflag.ContainsKey(getvalue))
                                    {
                                        present_count++;
                                    }
                                    if (absent_calcflag.ContainsKey(getvalue))
                                    {
                                        absent_count++;
                                    }
                                }
                            }
                            FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 2), Att_mark_column].Text = present_count.ToString();
                            FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 1), Att_mark_column].Text = absent_count.ToString();
                        }
                        if (savefalg == true)
                        {

                            string entrycode = Session["Entry_Code"].ToString();
                            string formname = "Student Attendance Entry";
                            string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                            string doa = DateTime.Now.ToString("MM/dd/yyy");
                            string section = string.Empty;
                            if (ddlsec.SelectedValue.ToString() != "" && ddlsec.SelectedValue.ToString() != "All" && ddlsec.SelectedValue.ToString() != null && ddlsec.SelectedValue.ToString() != "0")
                            {
                                section = ":Sections -" + ddlsec.SelectedValue.ToString();
                            }
                            string details = "" + ddlbranch.SelectedValue.ToString() + ":Sem - " + ddlsem.SelectedValue.ToString() + ":Batch Year -" + ddlbatch.SelectedValue.ToString() + " " + section + "";
                            string modules = "0";
                            string act_diff = " ";
                            string ctsname = "Update Attendance Information";
                            if (noentryflag == true)
                            {
                                if (savevalue == 1)
                                {
                                    ctsname = "Save the Attendance Inforamtion";
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Saved successfully')", true);
                                }
                                else
                                {
                                    ctsname = "Update the Attendance Information";
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Updated successfully')", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Please Update Attendance And Save')", true);
                            }
                            string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','" + savevalue + "','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
                            int a = dacces2.update_method_wo_parameter(strlogdetails, "Text");
                            Buttonsave.Visible = false;
                            Buttonupdate.Visible = true;
                        }
                    }
                }
            }
            mpemsgboxsave.Show();
        }
        catch
        {
        }
    }

    /// <summary>
    ///  ************** Added by jairam 04-10-2014 **************
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btncopy_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread2.SaveChanges();
            string attend_value = string.Empty;
            for (int Att_row = 1; Att_row <= FpSpread2.Sheets[0].RowCount - 3; Att_row++)
            {
                attend_value = FpSpread2.Sheets[0].Cells[Att_row, 5].Text;
                if (attend_value.Trim() != "")
                {
                    for (int Att_column = 6; Att_column <= FpSpread2.Sheets[0].ColumnCount - 1; Att_column++)
                    {
                        FpSpread2.Sheets[0].Cells[Att_row, Att_column].Text = attend_value.ToString();
                    }
                }
            }
            FpSpread2.SaveChanges();
            present_calcflag.Clear();
            absent_calcflag.Clear();
            hat.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            ds_attndmaster = dacces2.select_method("ATT_MASTER_SETTING", hat, "sp");
            count_master = (ds_attndmaster.Tables[0].Rows.Count);
            if (count_master > 0)
            {
                for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                {
                    if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                    {
                        present_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());

                    }
                    if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                    {
                        absent_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                    }
                }
            }
            for (Att_mark_column = 5; Att_mark_column < FpSpread2.Sheets[0].ColumnCount; Att_mark_column++)
            {
                absent_count = 0;
                present_count = 0;
                for (Att_mark_row = 1; Att_mark_row < FpSpread2.Sheets[0].RowCount - 2; Att_mark_row++)
                {
                    if (FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text != "" && FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text != " " && FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text != null) //condn 09.08.12 mythili
                    {
                        string getvalue = Attvalues(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text.ToString());
                        if (present_calcflag.ContainsKey(getvalue))
                        {
                            present_count++;
                        }
                        if (absent_calcflag.ContainsKey(getvalue))
                        {
                            absent_count++;
                        }
                    }
                }
                FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 2), Att_mark_column].Text = present_count.ToString();
                FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 1), Att_mark_column].Text = absent_count.ToString();
            }
        }
        catch
        {

        }
    }

    //************************* End **************************************

    //Added by Idhris 29-12-2016

    #region allstudentattendancereport new table

    protected void attendanceMark(string appno, int mnthYear, string attDay, int noMaxHrsDay, int noFstHrsDay, int noSndHrsDay, int noMinFstHrsDay, int noMinSndHrsDay, string DateVal, string collegecode)
    {
        try
        {
            DataSet dsload = new DataSet();
            Dictionary<int, int> AttValueMrng = new Dictionary<int, int>();
            Dictionary<int, int> AttvalueEve = new Dictionary<int, int>();
            double attVal = 0;
            int MPCnt = 0;
            int EPCnt = 0;
            int MnullCnt = 0;
            int EnullCnt = 0;
            string SelQ = " select " + attDay + ",A.ROLL_NO,r.app_no from attendance a,registration r where r.roll_no =a.roll_no  and r.college_code=Att_CollegeCode and  r.college_code='" + collegecode + "' AND month_year='" + mnthYear + "' and Att_App_no='" + appno + "' ";
            //d1d1,d1d2,d1d3,d1d4,d1d5,d1d6,d1d7,d1d8,d1d9,d1d10,d2d1,d2d2,d2d3,d2d4,d2d5,d2d6,d2d7,d2d8,d2d9,d2d10,d3d1,d3d2,d3d3,d3d4,d3d5,d3d6,d3d7,d3d8,d3d9,d3d10,d4d1,d4d2,d4d3,d4d4,d4d5,d4d6,d4d7,d4d8,d4d9,d4d10,d5d1,d5d2,d5d3,d5d4,d5d5,d5d6,d5d7,d5d8,d5d9,d5d10,d6d1,d6d2,d6d3,d6d4,d6d5,d6d6,d6d7,d6d8,d6d9,d6d10,d7d1,d7d2,d7d3,d7d4,d7d5,d7d6,d7d7,d7d8,d7d9,d7d10,d8d1,d8d2,d8d3,d8d4,d8d5,d8d6,d8d7,d8d8,d8d9,d8d10,d9d1,d9d2,d9d3,d9d4,d9d5,d9d6,d9d7,d9d8,d9d9,d9d10,d10d1,d10d2,d10d3,d10d4,d10d5,d10d6,d10d7,d10d8,d10d9,d10d10,d11d1,d11d2,d11d3,d11d4,d11d5,d11d6,d11d7,d11d8,d11d9,d11d10,d12d1,d12d2,d12d3,d12d4,d12d5,d12d6,d12d7,d12d8,d12d9,d12d10,d13d1,d13d2,d13d3,d13d4,d13d5,d13d6,d13d7,d13d8,d13d9,d13d10,d14d1,d14d2,d14d3,d14d4,d14d5,d14d6,d14d7,d14d8,d14d9,d14d10,d15d1,d15d2,d15d3,d15d4,d15d5,d15d6,d15d7,d15d8,d15d9,d15d10,d16d1,d16d2,d16d3,d16d4,d16d5,d16d6,d16d7,d16d8,d16d9,d16d10,d17d1,d17d2,d17d3,d17d4,d17d5,d17d6,d17d7,d17d8,d17d9,d17d10,d18d1,d18d2,d18d3,d18d4,d18d5,d18d6,d18d7,d18d8,d18d9,d18d10,d19d1,d19d2,d19d3,d19d4,d19d5,d19d6,d19d7,d19d8,d19d9,d19d10,d20d1,d20d2,d20d3,d20d4,d20d5,d20d6,d20d7,d20d8,d20d9,d20d10,d21d1,d21d2,d21d3,d21d4,d21d5,d21d6,d21d7,d21d8,d21d9,d21d10,d22d1,d22d2,d22d3,d22d4,d22d5,d22d6,d22d7,d22d8,d22d9,d22d10,d23d1,d23d2,d23d3,d23d4,d23d5,d23d6,d23d7,d23d8,d23d9,d23d10,d24d1,d24d2,d24d3,d24d4,d24d5,d24d6,d24d7,d24d8,d24d9,d24d10,d25d1,d25d2,d25d3,d25d4,d25d5,d25d6,d25d7,d25d8,d25d9,d25d10,d26d1,d26d2,d26d3,d26d4,d26d5,d26d6,d26d7,d26d8,d26d9,d26d10,d27d1,d27d2,d27d3,d27d4,d27d5,d27d6,d27d7,d27d8,d27d9,d27d10,d28d1,d28d2,d28d3,d28d4,d28d5,d28d6,d28d7,d28d8,d28d9,d28d10,d29d1,d29d2,d29d3,d29d4,d29d5,d29d6,d29d7,d29d8,d29d9,d29d10,d30d1,d30d2,d30d3,d30d4,d30d5,d30d6,d30d7,d30d8,d30d9,d30d10,d31d1,d31d2,d31d3,d31d4,d31d5,d31d6,d31d7,d31d8,d31d9,d31d10
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQ, "Text");
            if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
            {
                for (int sel = 0; sel < noMaxHrsDay; sel++)
                {
                    if (sel < noFstHrsDay)
                    {
                        double.TryParse(Convert.ToString(dsload.Tables[0].Rows[0][sel]), out attVal);
                        if (attVal != 0 || attVal != 0.0)
                        {
                            if (attVal == 1)
                                MPCnt++;
                            else
                            {
                                //  MOCnt = attVal;
                                if (!AttValueMrng.ContainsKey(Convert.ToInt32(attVal)))
                                    AttValueMrng.Add(Convert.ToInt32(attVal), 1);
                                else
                                {
                                    int Cnt = 0;
                                    int.TryParse(Convert.ToString(AttValueMrng[Convert.ToInt32(attVal)]), out Cnt);
                                    Cnt += 1;
                                    AttValueMrng.Remove(Convert.ToInt32(attVal));
                                    AttValueMrng.Add(Convert.ToInt32(attVal), Cnt);
                                }
                            }

                        }
                        else
                            MnullCnt++;
                    }
                    else if (sel >= noSndHrsDay)
                    {
                        double.TryParse(Convert.ToString(dsload.Tables[0].Rows[0][sel]), out attVal);
                        if (attVal != 0 || attVal != 0.0)
                        {
                            if (attVal == 1)
                                EPCnt++;
                            else
                            {
                                // EOCnt = attVal;
                                if (!AttvalueEve.ContainsKey(Convert.ToInt32(attVal)))
                                    AttvalueEve.Add(Convert.ToInt32(attVal), 1);
                                else
                                {
                                    int Cnt = 0;
                                    int.TryParse(Convert.ToString(AttvalueEve[Convert.ToInt32(attVal)]), out Cnt);
                                    Cnt += 1;
                                    AttvalueEve.Remove(Convert.ToInt32(attVal));
                                    AttvalueEve.Add(Convert.ToInt32(attVal), Cnt);
                                }
                            }

                        }
                        else
                            EnullCnt++;
                    }
                }

                int matt = attendanceSet(MPCnt, MnullCnt, noMinFstHrsDay, AttValueMrng);
                int eatt = attendanceSet(EPCnt, EnullCnt, noMinSndHrsDay, AttvalueEve);
                if (matt != null && eatt != null)
                {
                    string InsQ = " if exists (select AppNo from AllStudentAttendanceReport where dateofattendance='" + DateVal + "' and appno='" + dsload.Tables[0].Rows[0]["app_no"] + "')update AllStudentAttendanceReport set mleavecode='" + matt + "',eleavecode='" + eatt + "' where  dateofattendance='" + DateVal + "' and appno='" + dsload.Tables[0].Rows[0]["app_no"] + "' else insert into AllStudentAttendanceReport(AppNo, DateofAttendance,MLeaveCode,ELeaveCode) values('" + dsload.Tables[0].Rows[0]["app_no"] + "','" + DateVal + "','" + matt + "','" + eatt + "')";
                    int save = d2.update_method_wo_parameter(InsQ, "Text");
                }
            }
        }
        catch { }
    }

    protected int attendanceSet(int attCnt, int nullCnt, int hrCntCheck, Dictionary<int, int> val)
    {
        int attVal = 0;
        try
        {
            //if (attCnt >= hrCntCheck)
            //    attVal = Convert.ToInt32(leave);
            //else if (nullCnt > 0)
            //    attVal = 0;
            //else
            //    attVal = Convert.ToInt32(leave);
            if (attCnt >= hrCntCheck)
                attVal = 1;
            else if (nullCnt > 0)
                attVal = 0;
            else
            {
                val = val.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                foreach (KeyValuePair<int, int> txt in val)
                {
                    attVal = Convert.ToInt32(txt.Key);
                    break;
                }
            }
        }
        catch { }
        return attVal;
    }

    #endregion

    /// <summary>
    /// This Is Used to Get All Fee of Students 
    /// Developed By Malang Raja T
    /// </summary>
    /// <param name="dicFeeOfRoll">referenced type Dictionary To Hold Fee of Roll Student</param>
    /// <param name="dtFromDate">dd-MM-yyyy</param>
    /// <param name="dtToDate">dd-MM-yyyy</param>
    private void GetFeeOfRollStudent(ref Dictionary<string, DateTime[]> dicFeeOfRollStudents, ref Dictionary<string, byte> dicFeeOnRoll, string fromDate = null, string toDate = null)
    {
        try
        {
            DataSet dsFeeOfRollDate = new DataSet();
            DateTime dtFromDate = new DateTime();
            DateTime dtToDate = new DateTime();
            bool isFromSuccess = false;
            bool isToSuccess = false;
            if (!string.IsNullOrEmpty(fromDate))
            {
                isFromSuccess = DateTime.TryParseExact(fromDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtFromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                isToSuccess = DateTime.TryParseExact(toDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtToDate);
            }
            string qryFeeOfRollDate = string.Empty;
            if (isFromSuccess && isToSuccess)
            {
                qryFeeOfRollDate = " and curr_date between '" + dtFromDate.ToString("mm/dd/yyyy") + "' and '" + dtToDate.ToString("mm/dd/yyyy") + "'";
            }
            else if (isFromSuccess)
            {
                qryFeeOfRollDate = " and curr_date='" + dtFromDate.ToString("mm/dd/yyyy") + "'";
            }
            else if (isToSuccess)
            {
                qryFeeOfRollDate = " and curr_date='" + dtToDate.ToString("mm/dd/yyyy") + "'";
            }
            else
            {
                qryFeeOfRollDate = string.Empty;
            }
            string qry = "select roll_no,Convert(varchar(50),curr_date,103) as curr_date,Convert(varchar(50),CAST(ack_date as DateTime),103) as ack_date,Convert(varchar(50),feeOnRollDate,103) as feeOnRollDate,semester,ack_fee_of_roll from stucon where (ack_fee_of_roll=1 or feeOnRollDate is not null) " + qryFeeOfRollDate;
            dsFeeOfRollDate = dacces2.select_method_wo_parameter(qry, "text");

            if (dsFeeOfRollDate.Tables.Count > 0 && dsFeeOfRollDate.Tables[0].Rows.Count > 0)
            {
                dicFeeOfRollStudents.Clear();
                foreach (DataRow drFeeOfRoll in dsFeeOfRollDate.Tables[0].Rows)
                {
                    string rollNo = Convert.ToString(drFeeOfRoll["roll_no"]).Trim().ToLower();
                    string feeOffRollDate = Convert.ToString(drFeeOfRoll["curr_date"]).Trim();
                    string feeOffRollDate1 = Convert.ToString(drFeeOfRoll["ack_date"]).Trim();
                    string feeOnRollDate = Convert.ToString(drFeeOfRoll["feeOnRollDate"]).Trim();
                    string isFeeOfRoll = Convert.ToString(drFeeOfRoll["ack_fee_of_roll"]).Trim();
                    byte FeeOnRoll = 0;
                    byte.TryParse(isFeeOfRoll.Trim(), out FeeOnRoll);
                    DateTime dtFeeOffRollDate = new DateTime();
                    DateTime dtFeeOnRollDate = new DateTime();
                    bool isFeeOff = DateTime.TryParseExact(feeOffRollDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFeeOffRollDate);
                    bool isFeeOn = DateTime.TryParseExact(feeOnRollDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFeeOnRollDate);
                    DateTime[] dtFeeRoll = new DateTime[2];
                    dtFeeRoll[0] = dtFeeOffRollDate;
                    dtFeeRoll[1] = dtFeeOnRollDate;
                    if (!isFeeOn)
                    {
                        //dtFeeOnRollDate = ;
                    }
                    if (!dicFeeOfRollStudents.ContainsKey(rollNo.Trim()))
                    {
                        dicFeeOfRollStudents.Add(rollNo.Trim(), dtFeeRoll);
                    }
                    if (!dicFeeOnRoll.ContainsKey(rollNo.Trim()))
                    {
                        dicFeeOnRoll.Add(rollNo.Trim(), FeeOnRoll);
                    }
                }
            }
        }
        catch
        {

        }
    }

}

//26.09.16 old function
//public bool daycheck(DateTime seldate)
//   {
//       string collegecode = Session["collegecode"].ToString();
//       bool daycheck;
//       //DateTime curdate, prevdate;
//       long total, k, s;
//       string[] ddate = new string[500];
//       //DateTime[] ddate = new DateTime[500];
//       //curdate == DateTime.Today.ToString() ;
//       string c_date = DateTime.Today.ToString();

//       if (seldate.ToString() == c_date)
//       {
//           daycheck = true;
//           return daycheck;
//       }
//       else
//       {
//           //dc_con.Close();
//           //dc_con.Open();
//           //SqlCommand cmd = new SqlCommand("select lockdays,lflag from collinfo where college_code=" + collegecode + "", dc_con);
//           //SqlDataAdapter da = new SqlDataAdapter(cmd);
//           string strlockquery = "select lockdays,lflag from collinfo where college_code=" + collegecode + "";
//           DataSet ds = new DataSet();
//           ds = dacces2.select_method(strlockquery, hat, "Text");
//           //  da.Fill(ds);
//           if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
//           {
//               for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
//               {
//                   //If StrComp(ChkRs(1), "true", vbTextCompare) = 0 Then
//                   if (ds.Tables[0].Rows[i][1].ToString() == "true")
//                   {
//                       //If IsNull(ChkRs(0)) = False And val(ChkRs(0)) >= 0 Then
//                       if (ds.Tables[0].Rows[i][0].ToString() != null && int.Parse(ds.Tables[0].Rows[i][0].ToString()) >= 0)
//                       {
//                           total = int.Parse(ds.Tables[0].Rows[i][0].ToString());
//                           //Modified by srinath 13/9/2013

//                           //dc_con1.Close();
//                           //dc_con1.Open();
//                           //SqlCommand cmd1 = new SqlCommand("select holiday_date from holidaystudents where degree_code=" + ddldegree.SelectedValue + "  and semester=" + ddlsem.SelectedItem.ToString() + "", dc_con1);
//                           //SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
//                           string holidayquery = "select holiday_date from holidaystudents where degree_code=" + ddldegree.SelectedValue + "  and semester=" + ddlsem.SelectedItem.ToString() + "";
//                           DataSet ds1 = new DataSet();
//                           ds1 = dacces2.select_method(holidayquery, hat, "Text");
//                           //da1.Fill(ds1);
//                           if (ds1.Tables[0].Rows.Count <= 0)
//                           {
//                               for (int i1 = 1; i1 < total; i1++)
//                               {
//                                   string temp_date = seldate.AddDays(-1).ToString();
//                                   if (temp_date == seldate.ToString())
//                                   {
//                                       daycheck = true;
//                                       return daycheck;
//                                   }
//                               }
//                           }
//                           else
//                           {
//                               k = 0;
//                               for (int i1 = 1; i1 < ds1.Tables[0].Rows.Count; i1++)
//                               {
//                                   ddate[k] = ds1.Tables[0].Rows[i1][0].ToString();
//                                   k++;
//                               }

//                               i = 1;
//                               while (i <= total)
//                               {
//                                   string temp_date = seldate.AddDays(-1).ToString();
//                                   for (s = 0; s < k - 1; s++)
//                                   {
//                                       if (temp_date == ddate[s].ToString())
//                                       {
//                                           total = total + 1;
//                                           goto lab;
//                                       }

//                                   }
//                               lab:
//                                   i = i + 1;
//                                   if (temp_date == seldate.ToString())
//                                   {
//                                       daycheck = true;
//                                       return daycheck;
//                                   }
//                               }
//                           }
//                       }
//                       else
//                       {
//                           daycheck = true;
//                       }
//                   }
//                   else
//                   {
//                       daycheck = true;
//                   }
//               }
//           }
//       }
//       return true;
//   }


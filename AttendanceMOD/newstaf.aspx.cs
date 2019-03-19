using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;

public partial class newstaf : System.Web.UI.Page
{
    string batchsetting = string.Empty;
    SqlConnection ssql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());

    bool isDoubleDay = false;
    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
    TableCell HeaderCell = new TableCell();
    SqlCommand cmd = new SqlCommand();
    DataTable dtTTDisp = new DataTable();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    bool isvisible = true;
    NotificationSend ns = new NotificationSend();//abarna
    static string path1 = string.Empty;
    string staff_code = string.Empty;
    string loadunitssubj_no = string.Empty;

    int Att_mark_column = 0, Att_mark_row = 0, absent_count = 0, present_count = 0;

    string strdayflag;
    string regularflag = string.Empty;
    string genderflag = string.Empty;
    string str = string.Empty;
    string degree_code = string.Empty;
    string semester = string.Empty;
    bool chk = false;
    string strday = string.Empty;
    TreeNode cnode;
    TreeNode ynode;
    string subject_no = string.Empty;
    string sections = string.Empty;
    string hr = string.Empty;
    static string sel_date1 = string.Empty;
    static string sel_date = string.Empty;
    static string getcelltag = string.Empty;
    string getcolheader = string.Empty;
    string getdate = string.Empty;
    string getdate_new = string.Empty;
    string strsec = string.Empty;
    string Att_strqueryst = string.Empty;
    string subj_count_in_onehr = string.Empty;
    static string selectedpath = string.Empty;
    static string storepath = string.Empty;
    DataTable dthmwwrk = new DataTable();
    DataRow drhmewrk = null;
    Hashtable hat1 = new Hashtable();
    static int ar;
    static int ac;
    string subj_type = string.Empty;
    string MsgText = string.Empty;
    string RecepientNo = string.Empty;
    string AttDate = string.Empty;
    string AttHour = string.Empty;
    string tmp_camprevar = string.Empty;
    string cur_camprevar = string.Empty;
    DAccess2 da = new DAccess2();
    DataSet ds_iscount = new DataSet();
    DataSet ds_attndmaster = new DataSet();
    Hashtable present_calcflag = new Hashtable();
    Hashtable absent_calcflag = new Hashtable();
    Hashtable hat = new Hashtable();
    static Hashtable ht_sch = new Hashtable();
    static Hashtable ht_sdate = new Hashtable();
    static Hashtable ht_bell = new Hashtable();
    static Hashtable ht_period = new Hashtable();

    int count_master = 0;
    static string grouporusercode = string.Empty;
    static bool hr_lock = false;
    string noofdays = string.Empty;
    string start_datesem = string.Empty;
    string start_dayorder = string.Empty;
    string degree_var = string.Empty;
    string tmp_datevalue = string.Empty;
    string strsction = string.Empty;
    string Day_Order = string.Empty;
    string Day_Var = string.Empty;
    bool singlesubject = false;
    string singlesubjectno = string.Empty;
    static int inicolcount = 0;
    string SenderID = string.Empty;
    string Password = string.Empty;
    string user_id = string.Empty;
    bool slipfalg = false;
    string strquerytext = string.Empty;
    DataSet ds = new DataSet();
    Hashtable hatroll = new Hashtable();
    string strinvalidroll = string.Empty;
    bool dailyentryflag = false;
    bool attendanceentryflag = false;
    Hashtable has_attnd_masterset = new Hashtable();
    Hashtable has_attnd_masterset_notconsider = new Hashtable();
    Hashtable hatabsentvalues = new Hashtable();
    Hashtable has_total_attnd_hour = new Hashtable();
    Hashtable ht_sphr = new Hashtable();
    Hashtable has_hs = new Hashtable();
    Hashtable has_load_rollno = new Hashtable();
    static ArrayList arrayst;//abarna
    static ArrayList arr;
    bool saveflag = false;
    Dictionary<string, DateTime[]> dicFeeOfRollStudents = new Dictionary<string, DateTime[]>();
    Dictionary<string, byte> dicFeeOnRollStudents = new Dictionary<string, byte>();

    Dictionary<int, string> dicalter = new Dictionary<int, string>();
    
    public bool daycheck(DateTime seldate)
    {
        string collegecode = Session["collegecode"].ToString();
        bool daycheck = false;
        DateTime curdate;//, prevdate;
        long total, k, s;
        string[] ddate;
        //DateTime[] ddate = new DateTime[500];
        //curdate == DateTime.Today.ToString() ;
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
            //Modified by srinath 12/8/2013
            string lockdayvalue = "select lockdays,lflag from collinfo where college_code=" + collegecode + "";
            DataSet ds = new DataSet();
            ds = da.select_method(lockdayvalue, hat, "Text");
            // da.Fill(ds);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i][1].ToString() == "True")
                    {
                        if (ds.Tables[0].Rows[i][0].ToString() != null && int.Parse(ds.Tables[0].Rows[i][0].ToString()) >= 0)
                        {
                            total = int.Parse(ds.Tables[0].Rows[i][0].ToString());
                            total = total + 1;
                            //Modified by srinath 12/8/2013
                            String strholidasquery = "select holiday_date from holidaystudents where degree_code=" + Session["deg_code"].ToString() + "  and semester=" + Session["semester"].ToString() + "";
                            DataSet ds1 = new DataSet();
                            ds1 = da.select_method(strholidasquery, hat, "Text");
                            //if (ds1.Tables[0].Rows.Count <= 0)
                            if (ds1.Tables[0].Rows.Count <= 0)
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
                                ddate = new string[ds1.Tables[0].Rows.Count];
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
                    else
                    {
                        daycheck = true;
                    }
                }
            }
        }
        return daycheck;
    }
    public bool DayLockForUser(DateTime seldate)
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
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim().Split(';')[0] + "";
            }
            else
            {
                grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            //modified by Mullai
            string batyr = Session["batch_year"].ToString();
            string batchyr1 = string.Empty;
            string lockdayvalue = "select * from Master_Settings where settings='Attendance Lock Days' " + grouporusercode + "";
            DataSet ds = new DataSet();
            ds = da.select_method(lockdayvalue, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string batchvalue = string.Empty;
                batchvalue = "0";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (Convert.ToString(ds.Tables[0].Rows[0]["value"]) == "1")
                    {
                        string batyr3 = Convert.ToString(ds.Tables[0].Rows[0]["template"]);
                        string[] sptbat = batyr3.Split(',');
                        if (sptbat.Length > 0)
                        {
                            for (int j1 = 0; j1 < sptbat.Length; j1++)
                            {
                                string yr2 = sptbat[j1].ToString();
                                string[] batyrspt = yr2.Split('-');

                                string batchyer = batyrspt[0].ToString();
                                if (batyr.Contains(batchyer))
                                {
                                    batchvalue = batyrspt[1].ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        batchvalue = Convert.ToString(ds.Tables[0].Rows[0]["template"]);
                    }
                    if (!string.IsNullOrEmpty(batchvalue))
                    {
                        // if (ds.Tables[0].Rows[i][0].ToString() != null && ds.Tables[0].Rows[i][0].ToString() != "")
                        //{
                        //total = int.Parse(ds.Tables[0].Rows[i][0].ToString());
                        //total = total + 1;
                        total = int.Parse(batchvalue);
                        total = total + 1;
                        String strholidasquery = "select holiday_date from holidaystudents where degree_code='" + Convert.ToString(Session["deg_code"]).Trim() + "'  and semester='" + Convert.ToString(Session["semester"]).Trim() + "'";
                        DataSet ds1 = new DataSet();
                        ds1 = da.select_method(strholidasquery, hat, "Text");
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

    //protected override void Render(System.Web.UI.HtmlTextWriter writer)
    //{

    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null) //Aruna For Back Button
            {
                Response.Redirect("~/Default.aspx");
            }

            // Buttonsavelesson.Enabled = false;
            pnl_sliplist.Visible = false;
            btnsliplist.Enabled = false;
            staff_code = (string)Session["Staff_Code"];
            btnsliplist.Visible = false;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            chkis_studavailable.Checked = false;
            if (!IsPostBack)
            {
                pBodyhomework.Visible = false;
                headerpanelhomework.Visible = false;
                //btnaddhme.Visible = false;
                GridView1.DataBind();
                tbfdate.Attributes.Add("readonly", "readonly");
                tbtodate.Attributes.Add("readonly", "readonly");
                othertopicadd();
                btnaddquestion.Enabled = true;
                btnqtnupdate.Enabled = false;
                string dateFormat = DateTime.Now.GetDateTimeFormats('d')[0];
                if (staff_code == "" || staff_code == null)
                {
                    Response.Write("You Are not a Valid Staff");
                    //return;//Hided by Manikandan 14/08/2013
                }
                //Start==============added by Manikandan 14/08/2013================
                tbfdate.Text = DateTime.Now.AddDays(0).ToString("d-MM-yyyy");
                // =datefrom.ToString();
                tbtodate.Text = DateTime.Now.AddDays(0).ToString("d-MM-yyyy");
                string staffOrAdmin = Convert.ToString(Session["StafforAdmin"]).Trim();
                if ((staffOrAdmin.ToLower().Trim() == "staff"))
                {
                    //scheduleorattnd = 1;                
                    loadstafspread();
                    Label1.Text = "Individual Staff Report";
                    scodelbl.Visible = false;
                    scodetxt.Visible = false;
                    snamelbl.Visible = false;
                    snamelbl1.Visible = false;
                    lblstaffname.Visible = false;
                    ddlstaffname.Visible = false;
                }
                else if ((staffOrAdmin.ToLower().Trim() == "admin"))
                {
                    //scheduleorattnd = 1;
                    bindstaff();
                    loadstafspread();
                    scodelbl.Visible = true;
                    scodetxt.Visible = true;
                    snamelbl.Visible = true;
                    snamelbl1.Visible = false;
                    gridTimeTable.Visible = false;
                    lblstaffname.Visible = true;
                    ddlstaffname.Visible = true;
                    Label1.Text = "Individual Staff Report";
                    clearfield();
                }
                else
                {
                    if (string.IsNullOrEmpty(Convert.ToString(Session["Staff_Code"]).Trim()))
                    {
                        bindstaff();
                        loadstafspread();

                        scodelbl.Visible = true;
                        scodetxt.Visible = true;
                        snamelbl.Visible = true;
                        snamelbl1.Visible = false;
                        gridTimeTable.Visible = false;
                        lblstaffname.Visible = true;
                        ddlstaffname.Visible = true;
                        Label1.Text = "Individual Staff Report";
                        clearfield();
                    }
                    else
                    {
                        //Label1.Text = "Individual Staff Report";
                        scodelbl.Visible = false;
                        scodetxt.Visible = false;
                        snamelbl.Visible = false;
                        snamelbl1.Visible = false;
                        lblstaffname.Visible = false;
                        ddlstaffname.Visible = false;
                    }
                }
                tvyet.Attributes.Add("onclick", "postBackByObject()");
                tvcomplete.Attributes.Add("onclick", "postBackByObject()");
                Session["curr_year"] = DateTime.Now.ToString("yyyy");
                Session["Rollflag"] = "0";//26.01.17
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                Session["Sex"] = "0";
                Session["flag"] = "-1";
                string Master = "select * from Master_Settings where " + grouporusercode + "";
                DataSet ds = da.select_method(Master, hat, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "sex" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Sex"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "General attend" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            option.SelectedValue = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Absentees" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            option.SelectedValue = "2";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "RollNo" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            RadioButtonList1.SelectedValue = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "RegisterNo" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            RadioButtonList1.SelectedValue = "2";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Admission No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            RadioButtonList1.SelectedValue = "3";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "General" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["flag"] = 0;
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "As Per Lesson" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["flag"] = 1;
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Male" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            if (genderflag != "" && genderflag != "\0")  //added By Mullai
                            {
                                genderflag = genderflag + " applyn.sex='0'";
                            }
                            else
                            {
                                genderflag = " and (applyn.sex='0' ";
                            }


                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Female" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            if (genderflag != "" && genderflag != "\0")
                            {
                                genderflag = genderflag + " or applyn.sex='1'";
                            }
                            else
                            {
                                genderflag = " and (applyn.sex='1' or";
                            }
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Days Scholor" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            strdayflag = " and (registration.Stud_Type='Day Scholar'";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Hostel" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
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
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Regular" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            regularflag = "and ((registration.mode=1)";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Lateral" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
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
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Transfer" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
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
                Session["StaffSelector"] = "0";
                Session["Copy Attendance"] = "0";
                string rightscopy = da.GetFunction("select value from Master_Settings where settings='Copy Attendance'  and " + grouporusercode + "");
                if (rightscopy == "1")
                {
                    Session["Copy Attendance"] = "1";
                }
                //********************************************
                if (Session["StafforAdmin"] == "")//Added by Manikandan 17/08/2013
                {
                    loadstafspread();//=========================================================function for load spread
                }
                loadreason();
                ddlreason.Attributes.Add("onfocus", "reason()");
                pnotesuploadadd.Visible = false;
            }

        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void Btnadd_Click(object sender, EventArgs e)
    {
        div5.Visible = true;
    }
    protected void btnnexit(object sender, EventArgs e)
    {
        div5.Visible = false;
    }
    protected void btnadd(object sender, EventArgs e)
    {
        string othertopic = "select subpk from dailyEntryother where college_code=" + Session["collegecode"] + " and topic_name='" + TextBox3.Text + " ' ";
        DataSet dsdailyother = da.select_method(othertopic, hat, "Text");
        if (dsdailyother.Tables.Count > 0 && dsdailyother.Tables[0].Rows.Count == 0)
        {
            string insother = "insert into dailyEntryother (college_code,topic_name) values(" + Session["collegecode"] + ",'" + TextBox3.Text + "' )";
            int a = da.update_method_wo_parameter(insother, "Text");
            if (a == 1)
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
            div5.Visible = false;
        }
        othertopicadd();
    }
    protected void Btndel_Click(object sender, EventArgs e)
    {

        string othertopic = "select subpk from dailyEntryother where college_code=" + Session["collegecode"] + " and subpk='" + ddlother.SelectedValue + " '";
        DataSet dsdailyother = da.select_method(othertopic, hat, "Text");
        if (dsdailyother.Tables.Count > 0 && dsdailyother.Tables[0].Rows.Count > 0)
        {
            string insother = "delete from  dailyEntryother where  college_code=" + Session["collegecode"] + " and subpk='" + dsdailyother.Tables[0].Rows[0]["subpk"] + " ' ";
            int a = da.update_method_wo_parameter(insother, "Text");
        }
        othertopicadd();
    }
    protected void ddlother_selected(object sender, EventArgs e)
    {
        Buttonsavelesson.Enabled = true;
    }
    protected void btnsaves(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(ddlother.SelectedItem.Text) != "Select")
            {
                string sch_order = string.Empty;

                string order_day = string.Empty;
                string query = "select schorder from PeriodAttndSchedule where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString();
                string order = da.GetFunction(query);
                if (order == "")
                    return;
                string curday = Session["sch_date"].ToString();
                DateTime day_lesson = Convert.ToDateTime(curday);
                if (order != "0")
                    order_day = day_lesson.ToString("ddd");
                else
                {
                    order_day = find_day_order();
                    if (order_day == "")
                        return;
                }
                if (order_day == "mon")
                    sch_order = "1";
                else if (order_day == "tue")
                    sch_order = "2";
                else if (order_day == "wed")
                    sch_order = "3";
                else if (order_day == "thu")
                    sch_order = "4";
                else if (order_day == "fri")
                    sch_order = "5";
                else if (order_day == "sat")
                    sch_order = "6";
                else if (order_day == "sun")
                    sch_order = "7";
                if (order_day == "Mon")
                    sch_order = "1";
                else if (order_day == "Tue")
                    sch_order = "2";
                else if (order_day == "Wed")
                    sch_order = "3";
                else if (order_day == "Thu")
                    sch_order = "4";
                else if (order_day == "Fri")
                    sch_order = "5";
                else if (order_day == "Sat")
                    sch_order = "6";
                else if (order_day == "Sun")
                    sch_order = "7";

                string othertopic = "select subpk from dailyEntryother where college_code=" + Session["collegecode"] + " and subpk='" + Convert.ToString(ddlother.SelectedValue) + " ' ";
                DataSet dsdailyother = da.select_method(othertopic, hat, "Text");
                if (dsdailyother.Tables.Count > 0 && dsdailyother.Tables[0].Rows.Count > 0)
                {
                    string strdailtquery = "select * from dailyStaffEntry where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "'";
                    DataSet dsdaily = da.select_method(strdailtquery, hat, "Text");
                    if (dsdaily.Tables.Count > 0 && dsdaily.Tables[0].Rows.Count > 0)
                    {

                        strdailtquery = "update dailyStaffEntry set sch_order=" + sch_order + ",othersub='" + dsdailyother.Tables[0].Rows[0]["subpk"] + "' where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "'";
                        int a = da.update_method_wo_parameter(strdailtquery, "Text");
                    }
                    else
                    {

                        string sec = (string)Session["sections"].ToString();
                        if (sec != "")
                            strdailtquery = "insert into dailyStaffEntry (degree_code,semester,batch_year,sch_date,sch_order,sections,othersub) values(" + Session["deg_code"].ToString() + "," + Session["semester"].ToString() + "," + Session["batch_year"].ToString() + ",'" + Session["sch_date"].ToString() + "'," + sch_order + ",'" + Session["sections"].ToString() + "','" + dsdailyother.Tables[0].Rows[0]["subpk"] + "')";
                        else
                            strdailtquery = "insert into dailyStaffEntry (degree_code,semester,batch_year,sch_date,sch_order,othersub) values(" + Session["deg_code"].ToString() + "," + Session["semester"].ToString() + "," + Session["batch_year"].ToString() + ",'" + Session["sch_date"].ToString() + "'," + sch_order + ",'" + dsdailyother.Tables[0].Rows[0]["subpk"] + "')";
                        int a = da.update_method_wo_parameter(strdailtquery, "Text");
                    }
                }
                else
                {

                    string othertopics = "select subpk from dailyEntryother where college_code=" + Session["collegecode"] + " and subpk='" + Convert.ToString(ddlother.SelectedValue) + "'  ";
                    DataSet dsdailyothers = da.select_method(othertopics, hat, "Text");
                    if (dsdailyothers.Tables.Count > 0 && dsdailyothers.Tables[0].Rows.Count > 0)
                    {
                        string strdailtquery = "select * from dailyStaffEntry where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "'";
                        DataSet dsdaily = da.select_method(strdailtquery, hat, "Text");
                        if (dsdaily.Tables.Count > 0 && dsdaily.Tables[0].Rows.Count > 0)
                        {

                            strdailtquery = "update dailyStaffEntry set sch_order=" + sch_order + ",othersub='" + dsdailyothers.Tables[0].Rows[0]["subpk"] + "' where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "'";
                            int ins = da.update_method_wo_parameter(strdailtquery, "Text");
                        }
                        else
                        {

                            string sec = (string)Session["sections"].ToString();
                            if (sec != "")
                                strdailtquery = "insert into dailyStaffEntry (degree_code,semester,batch_year,sch_date,sch_order,sections,othersub) values(" + Session["deg_code"].ToString() + "," + Session["semester"].ToString() + "," + Session["batch_year"].ToString() + ",'" + Session["sch_date"].ToString() + "'," + sch_order + ",'" + Session["sections"].ToString() + "','" + dsdailyothers.Tables[0].Rows[0]["subpk"] + "')";
                            else
                                strdailtquery = "insert into dailyStaffEntry (degree_code,semester,batch_year,sch_date,sch_order,othersub) values(" + Session["deg_code"].ToString() + "," + Session["semester"].ToString() + "," + Session["batch_year"].ToString() + ",'" + Session["sch_date"].ToString() + "'," + sch_order + ",'" + dsdailyothers.Tables[0].Rows[0]["subpk"] + "')";
                            int insert = da.update_method_wo_parameter(strdailtquery, "Text");
                        }
                    }
                }
                div5.Visible = false;
                filltree();
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void tbfdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Labelstaf.Visible = false;
            string[] spiltfrom = tbfdate.Text.ToString().Split(new Char[] { '-' });
            string[] spilto = tbtodate.Text.ToString().Split('-');
            DateTime dtto = Convert.ToDateTime(spilto[1].ToString() + '-' + spilto[0].ToString() + '-' + spilto[2].ToString());
            DateTime dtfrom = Convert.ToDateTime(spiltfrom[1].ToString() + '-' + spiltfrom[0].ToString() + '-' + spiltfrom[2].ToString());
            if (dtfrom > DateTime.Today)
            {
                if (Session["StafforAdmin"] == "")
                {
                    Labelstaf.Visible = true;
                    Labelstaf.Text = "You can not mark attendance for the date greater than today";
                    tbfdate.Text = DateTime.Today.ToString("d-MM-yyyy");
                }
            }
            if (dtfrom > dtto)
            {
                Labelstaf.Visible = true;
                Labelstaf.Text = "To Date Must be Greater than From Date";
                tbfdate.Text = tbtodate.Text;
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void tbtodate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Labelstaf.Visible = false;
            string[] spiltfrom = tbfdate.Text.ToString().Split(new Char[] { '-' });
            string[] spilto = tbtodate.Text.ToString().Split('-');
            DateTime dtto = Convert.ToDateTime(spilto[1].ToString() + '-' + spilto[0].ToString() + '-' + spilto[2].ToString());
            DateTime dtfrom = Convert.ToDateTime(spiltfrom[1].ToString() + '-' + spiltfrom[0].ToString() + '-' + spiltfrom[2].ToString());
            if (dtto > DateTime.Today)
            {
                if (Session["StafforAdmin"] == "")
                {
                    Labelstaf.Visible = true;
                    Labelstaf.Text = "You can not mark attendance for the date greater than today";
                    tbtodate.Text = DateTime.Today.ToString("d-MM-yyyy");
                }
            }
            if (dtfrom > dtto)
            {
                Labelstaf.Visible = true;
                Labelstaf.Text = "To Date Must be Greater than From Date";
                tbfdate.Text = tbtodate.Text;
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void option_SelectedIndexChanged(object sender, EventArgs e)
    {
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
    public string Attmark(string Attstr_mark)
    {
        string Att_mark = string.Empty;
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
            Att_mark = " ";
        }
        //return Convert.ToInt32(Att_mark);
        return Att_mark;
    }
    public void loadunits(string subject_no)
    {
        if (subject_no.Trim() != "")
        {
            string sqlunitsquery = "select * from sub_unit_details where subject_no='" + subject_no + "' and parent_code='0'";
            DataSet unitsds = new DataSet();
            unitsds.Clear();
            unitsds = da.select_method_wo_parameter(sqlunitsquery, "Text");
            if (unitsds.Tables.Count > 0 && unitsds.Tables[0].Rows.Count > 0)
            {
                ddlunits.DataTextField = "unit_name";
                ddlunits.DataValueField = "topic_no";
                ddlunits.DataSource = unitsds;
                ddlunits.DataBind();
                //ddlunitsobj.DataTextField = "unit_name";
                //ddlunitsobj.DataValueField = "topic_no";
                //ddlunitsobj.DataSource = unitsds;
                //ddlunitsobj.DataBind();
                ddlunits.Enabled = true;
                //ddlunitsobj.Enabled = true;
            }
            else
            {
                ddlunits.Enabled = false;
                //ddlunitsobj.Enabled = false;
            }
        }
    }
    protected void Buttonexit_Click(object sender, EventArgs e)
    {
    }
    protected void ddlmarkothers_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void btngoindividual_Click(object sender, EventArgs e)
    {
    }
    protected void ddlmark_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void txtregno_TextChanged(object sender, EventArgs e)
    {
    }
    protected void txtrunning_TextChanged(object sender, EventArgs e)
    {
    }
    protected void Buttonselectall_Click(object sender, EventArgs e)
    {
        try
        {
            //Added by srinath 24/8/2013
            //String rollnovalue = string.Empty;
            //bool savefalg = false;
            //if (FpSpread2.Sheets[0].RowCount > 1)
            //    for (int temp_col = 7; temp_col <= FpSpread2.Sheets[0].ColumnCount - 1; temp_col = temp_col + 2)
            //    {
            //        for (int row = 0; row < FpSpread2.Sheets[0].RowCount; row++)
            //        {
            //            if (FpSpread2.Sheets[0].Cells[row, temp_col].Text != "S" && FpSpread2.Sheets[0].Cells[row, temp_col].Locked != true && FpSpread2.Sheets[0].Cells[row, temp_col].Text.ToLower() != "od" && (FpSpread2.Sheets[0].Rows[row].BackColor != Color.Red))// condn added on 09.08.12 mythli
            //            {
            //                FpSpread2.Sheets[0].Cells[row, temp_col].Text = "P";
            //                savefalg = true;
            //                rollnovalue = FpSpread2.Sheets[0].Cells[1, 1].Text;
            //            }
            //        }
            //    }
            //---------------get calcflag
            //present_calcflag.Clear();
            //absent_calcflag.Clear();
            //hat.Clear();
            //hat.Add("colege_code", Session["collegecode"].ToString());
            //ds_attndmaster = da.select_method("ATT_MASTER_SETTING", hat, "sp");
            //if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
            //{
            //    count_master = (ds_attndmaster.Tables[0].Rows.Count);
            //    if (count_master > 0)
            //    {
            //        for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
            //        {
            //            if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
            //            {
            //                present_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
            //            }
            //            if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
            //            {
            //                absent_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
            //            }
            //        }
            //    }
            //}
            //for (Att_mark_column = 7; Att_mark_column < FpSpread2.Sheets[0].ColumnCount; Att_mark_column++)
            //{
            //    absent_count = 0;
            //    present_count = 0;
            //    for (Att_mark_row = 1; Att_mark_row < FpSpread2.Sheets[0].RowCount - 2; Att_mark_row++)
            //    {
            //        if (FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text.ToString() != "")
            //        {
            //            if (present_calcflag.ContainsKey(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Value.ToString()))
            //            {
            //                present_count++;
            //            }
            //            if (absent_calcflag.ContainsKey(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Value.ToString()))
            //            {
            //                absent_count++;
            //            }
            //        }
            //    }
            //    FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 2), Att_mark_column].Text = present_count.ToString();
            //    FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 1), Att_mark_column].Text = absent_count.ToString();
            //    Att_mark_column++;
            //}
            //FpSpread2.SaveChanges();
            //----------------------------
            //Added by srinath 24/8/2013
            //if (savefalg == true)
            //{
            //    string entrycode = Session["Entry_Code"].ToString();
            //    string formname = "Student Attendance Entry";
            //    string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
            //    string doa = DateTime.Now.ToString("MM/dd/yyy");
            //    string details = string.Empty;
            //    DataSet dsroll = da.select_method("Select Batch_year,Degree_code,Current_semester,Sections from registration where roll_no='" + rollnovalue + "'", hat, "Text");
            //    if (dsroll.Tables.Count > 0 && dsroll.Tables[0].Rows.Count > 0)
            //    {
            //        details = "" + dsroll.Tables[0].Rows[0]["Degree_code"].ToString() + ": Sem - " + dsroll.Tables[0].Rows[0]["Current_semester"].ToString() + ": Batch Year - " + dsroll.Tables[0].Rows[0]["Batch_year"].ToString();
            //        if (dsroll.Tables[0].Rows[0]["Sections"].ToString() != "" && dsroll.Tables[0].Rows[0]["Sections"].ToString() != "0" && dsroll.Tables[0].Rows[0]["Sections"].ToString() != null)
            //        {
            //            details = details + ": Sections - " + dsroll.Tables[0].Rows[0]["Sections"].ToString();
            //        }
            //    }
            //    string modules = "0";
            //    string act_diff = " ";
            //    string ctsname = "Change Attendance Information";
            //    string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','7','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
            //    int a = da.update_method_wo_parameter(strlogdetails, "Text");
            //}
            Buttonselectall.Focus();
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void Buttondeselect_Click(object sender, EventArgs e)
    {
        try
        {
            //String rollnovalue = string.Empty;
            //bool savefalg = false;
            //if (FpSpread2.Sheets[0].RowCount > 1)
            //    for (int temp_col = 7; temp_col <= FpSpread2.Sheets[0].ColumnCount - 1; temp_col++)
            //    {
            //        for (int row = 0; row < FpSpread2.Sheets[0].RowCount; row++)
            //        {
            //            if (FpSpread2.Sheets[0].Cells[row, temp_col].Text != "S" && FpSpread2.Sheets[0].Cells[row, temp_col].Locked != true && FpSpread2.Sheets[0].Cells[row, temp_col].Text.ToLower() != "od" && (FpSpread2.Sheets[0].Rows[row].BackColor != Color.Red))// condn added on 09.08.12 mythli
            //            {
            //                FpSpread2.Sheets[0].Cells[row, temp_col].Text = string.Empty;
            //                savefalg = true;
            //                rollnovalue = FpSpread2.Sheets[0].Cells[1, 1].Text;
            //            }
            //            FpSpread2.SaveChanges();
            //        }
            //    }
            ////---------------get calcflag
            //present_calcflag.Clear();
            //absent_calcflag.Clear();
            //hat.Clear();
            //hat.Add("colege_code", Session["collegecode"].ToString());
            //ds_attndmaster = da.select_method("ATT_MASTER_SETTING", hat, "sp");
            //if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
            //{
            //    count_master = (ds_attndmaster.Tables[0].Rows.Count);
            //    if (count_master > 0)
            //    {
            //        for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
            //        {
            //            if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
            //            {
            //                present_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
            //            }
            //            if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
            //            {
            //                absent_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
            //            }
            //        }
            //    }
            //}
            //for (Att_mark_column = 7; Att_mark_column < FpSpread2.Sheets[0].ColumnCount; Att_mark_column++)
            //{
            //    absent_count = 0;
            //    present_count = 0;
            //    for (Att_mark_row = 1; Att_mark_row < FpSpread2.Sheets[0].RowCount - 2; Att_mark_row++)
            //    {
            //        if (FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Text.ToString() != "")
            //        {
            //            if (present_calcflag.ContainsKey(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Value.ToString()))
            //            {
            //                present_count++;
            //            }
            //            if (absent_calcflag.ContainsKey(FpSpread2.Sheets[0].Cells[Att_mark_row, Att_mark_column].Value.ToString()))
            //            {
            //                absent_count++;
            //            }
            //        }
            //    }
            //    FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 2), Att_mark_column].Text = present_count.ToString();
            //    FpSpread2.Sheets[0].Cells[(FpSpread2.Sheets[0].RowCount - 1), Att_mark_column].Text = absent_count.ToString();
            //    Att_mark_column++;
            //}
            ////----------------------------
            ////Added by srinath 24/8/2013
            //if (savefalg == true)
            //{
            //    string entrycode = Session["Entry_Code"].ToString();
            //    string formname = "Student Attendance Entry";
            //    string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
            //    string doa = DateTime.Now.ToString("MM/dd/yyy");
            //    string details = string.Empty;
            //    DataSet dsroll = da.select_method("Select Batch_year,Degree_code,Current_semester,Sections from registration where roll_no='" + rollnovalue + "'", hat, "Text");
            //    if (dsroll.Tables.Count > 0 && dsroll.Tables[0].Rows.Count > 0)
            //    {
            //        details = "" + dsroll.Tables[0].Rows[0]["Degree_code"].ToString() + ": Sem - " + dsroll.Tables[0].Rows[0]["Current_semester"].ToString() + ": Batch Year - " + dsroll.Tables[0].Rows[0]["Batch_year"].ToString();
            //        if (dsroll.Tables[0].Rows[0]["Sections"].ToString() != "" && dsroll.Tables[0].Rows[0]["Sections"].ToString() != "0" && dsroll.Tables[0].Rows[0]["Sections"].ToString() != null)
            //        {
            //            details = details + ": Sections - " + dsroll.Tables[0].Rows[0]["Sections"].ToString();
            //        }
            //    }
            //    string modules = "0";
            //    string act_diff = " ";
            //    string ctsname = "Change Attendance Information";
            //    string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','8','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
            //    int a = da.update_method_wo_parameter(strlogdetails, "Text");
            //}
            //Buttonselectall.Focus();
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void OnTreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {
        Buttonsavelesson.Enabled = true;
        bool t = e.Node.Checked;
        if (ddlselectmanysub.Items.Count > 0)
        {
            string valsp = ddlselectmanysub.SelectedValue.ToString();
            string[] sp1 = valsp.Split(new Char[] { '-' });
            if (sp1.GetUpperBound(0) > 2)
            {
                string subcode = sp1[2].ToString();
                string subname = da.GetFunction("Select Subject_Name from subject where subject_no='" + subcode + "'");
                storepath = subname + " " + "/";
            }
            selectedpath = storepath;
        }
    }
    public string find_day_order()
    {
        int holiday = 0;
        string query = "select CONVERT(VARCHAR(10),start_date,23) from seminfo where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + "  and batch_year=" + Session["batch_year"].ToString();
        string sdate = da.GetFunction(query);
        string quer = "select nodays from PeriodAttndSchedule where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString();
        string no_days = da.GetFunction(quer);
        if (sdate != "")
        {
            string curday = Session["sch_date"].ToString();
            string[] sp_date = sdate.Split(new Char[] { '-' });
            string start_date = sp_date[1].ToString() + "-" + sp_date[2].ToString() + "-" + sp_date[0].ToString();
            DateTime dt1 = Convert.ToDateTime(start_date);
            DateTime dt2 = Convert.ToDateTime(curday);
            TimeSpan ts = dt2 - dt1;
            string query1 = "select count(*) from holidaystudents  where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "' and halforfull='0'";
            string holday = da.GetFunction(query1);
            if (holday != "")
                holiday = Convert.ToInt32(holday);
            int dif_days = ts.Days;
            int nodays = Convert.ToInt32(no_days);
            int order = (dif_days - holiday) % nodays;
            order = order + 1;
            string findday = string.Empty;
            if (order == 1)
                findday = "mon";
            else if (order == 2) findday = "tue";
            else if (order == 3) findday = "wed";
            else if (order == 4) findday = "thu";
            else if (order == 5) findday = "fri";
            else if (order == 6) findday = "sat";
            else if (order == 7) findday = "sun";
            return findday;
        }
        else
            return "";
    }
    protected void tvyet_SelectedNodeChanged1(object sender, EventArgs e)
    {
        Buttonsavelesson.Enabled = true;
        //bool t = e.Node.Checked;
    }
    protected void OnTreeNodecompleteCheckChanged(object sender, TreeNodeEventArgs e)
    {
        Buttonsavelesson.Enabled = true;
        bool t = e.Node.Checked;
    }
    public void SendingSms(string rollno, string appno, string regno, string admno, string date, string hour, string degree, int total, int absent)
    {
        try
        {
            string Gender = string.Empty;
            string Hour = hour;
            string hour_check = string.Empty;
            //UserEmailID =string.Empty;
            MsgText = string.Empty;
            RecepientNo = string.Empty;
            int check = 0;
            string coursename = string.Empty;
            string coursename1 = string.Empty;
            string collegename = string.Empty;
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
            string collquery = "Select collname,Coll_acronymn from collinfo where college_code=" + Session["collegecode"].ToString() + "";
            DataSet datacol = new DataSet();
            datacol.Clear();
            datacol = da.select_method_wo_parameter(collquery, "Text");
            if (datacol.Tables.Count > 0 && datacol.Tables[0].Rows.Count > 0)
            {
                collegename = datacol.Tables[0].Rows[0]["Coll_acronymn"].ToString();
            }
            //string degreequery = "select distinct Course_Name,Dept_Name from Department dep, Degree deg, course c where dep.Dept_Code=deg.Dept_Code and c.Course_Id=deg.Course_Id and deg.college_code =" + Session["collegecode"].ToString() + " and Degree_Code=" + degree + "";
            string degreequery = "select distinct Course_Name,Dept_Name,r.degree_code,deg.Acronym from Department dep, Degree deg, course c,Registration r where dep.Dept_Code=deg.Dept_Code and c.Course_Id=deg.Course_Id and r.degree_code=deg.Degree_Code and r.Roll_No='" + rollno + "'";
            DataSet dscode = new DataSet(); string degreecode = string.Empty;
            dscode = da.select_method_wo_parameter(degreequery, "Text");
            if (dscode.Tables.Count > 0 && dscode.Tables[0].Rows.Count > 0)
            {
                string course = dscode.Tables[0].Rows[0]["Course_Name"].ToString();
                string deptname = dscode.Tables[0].Rows[0]["Dept_Name"].ToString();
                string deptacrn = dscode.Tables[0].Rows[0]["Acronym"].ToString();
                degreecode = dscode.Tables[0].Rows[0]["degree_code"].ToString();
                coursename = course + "-" + deptname;
                coursename1 = course + "-" + deptacrn;
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
                //str1 = str1 + "select distinct textname,taxtval from Attendance_Settings where  textname='Day' and college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + group_code + "'";
            }
            else
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + Session["usercode"].ToString() + "'";
                str1 = str1 + "  select template from master_Settings where settings='SmsAttendanceTepmlate' and usercode='" + Session["usercode"].ToString() + "'and value='1'";

                //str1 = str1 + "select distinct textname,taxtval from Attendance_Settings where textname='Day' and college_code='" + Session["collegecode"].ToString() + "' and USER_ID='" + Session["usercode"].ToString() + "'";
            }
            bool flage = false;
            DataSet ds1;
            ds1 = da.select_method_wo_parameter(str1, "txt");


            string hodphone = da.GetFunction("  select d.PhoneNo from Department d,Degree de,staffmaster s,staff_appl_master sa where d.Dept_Code=de.Dept_Code and s.appl_no=sa.appl_no and d.Head_Of_Dept=s.staff_code and resign='0' and settled='0' and de.Dept_Code ='" + degreecode + "'");
            DataSet dsSMSSendDetails = new DataSet();
            bool hourWiseAbsent = false;//Convert.ToDateTime(date).ToString("dd/MM/yyyy")
            dsSMSSendDetails = da.select_method_wo_parameter("select * from smsdeliverytrackmaster where Convert(varchar(20),date,103)='" + DateTime.Now.ToString("dd/MM/yyyy") + "' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "' and smsFor='absentees'", "text");


            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                for (int jj = 0; jj < ds1.Tables[0].Rows.Count; jj++)
                {
                    if (ds1.Tables[0].Rows[jj]["TextName"].ToString() == "Attendance Sms for Absent" && ds1.Tables[0].Rows[jj]["Taxtval"].ToString() == "1")
                    {
                        flage = true;
                    }
                    else if (ds1.Tables[0].Rows[jj]["TextName"].ToString() == "Hour" && Convert.ToInt32(ds1.Tables[0].Rows[jj]["Taxtval"]) == 1)
                    {
                        hourWiseAbsent = true;
                    }
                    else if (ds1.Tables[0].Rows[jj]["TextName"].ToString() == "Hour" && Convert.ToInt32(ds1.Tables[0].Rows[jj]["Taxtval"]) == 0)
                    {
                        hourWiseAbsent = false;
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
                if (!hourWiseAbsent)
                {
                    check = 1;
                    Hour = date;
                    if (dsSMSSendDetails.Tables.Count > 0 && dsSMSSendDetails.Tables[0].Rows.Count > 0)
                    {
                        dsSMSSendDetails.Tables[0].DefaultView.RowFilter = "studentAppNo='" + appno + "'";
                        DataTable dtchk = dsSMSSendDetails.Tables[0].DefaultView.ToTable();
                        if (dtchk.Rows.Count > 0)
                            check = 0;
                    }
                }
            }
            if (check > 0)
            {
                check = 0;
                string ssr = "select * from Track_Value where college_code='" + Session["collegecode"].ToString() + "'";
                DataSet dstrack;
                dstrack = da.select_method_wo_parameter(ssr, "txt");
                if (dstrack.Tables.Count > 0 && dstrack.Tables[0].Rows.Count > 0)
                {
                    user_id = Convert.ToString(dstrack.Tables[0].Rows[0]["SMS_User_ID"]).Trim();
                    string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,r.app_no from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                    DataSet dsMobile;
                    dsMobile = da.select_method_wo_parameter(Phone, "txt");
                    if (ds1.Tables.Count > 1 && ds1.Tables[1].Rows.Count > 0) //************************ added by jairam****************************** 10-10-2014
                    {
                        DateTime dtatnddate = new DateTime();
                        DateTime currdt = new DateTime();
                        string currdate = DateTime.Now.ToString("dd/MM/yyyy");
                        string[] atnddtsplit = date.Split('-');
                        string attnddt = atnddtsplit[0] + "/" + atnddtsplit[1] + "/" + atnddtsplit[2];
                        bool atnddate = DateTime.TryParseExact(attnddt.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtatnddate);
                        bool currentdt = DateTime.TryParseExact(currdate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out currdt);

                        DateTime dt = Convert.ToDateTime(date);
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
                                            MsgText = MsgText + " " + collegename;
                                        }
                                        else if (splittemplate[j].ToString() == "Student Name")
                                        {
                                            MsgText = MsgText + " " + dsMobile.Tables[0].Rows[0]["StudName"].ToString();
                                        }
                                        else if (splittemplate[j].ToString().ToLower() == "degree")
                                        {
                                            MsgText = MsgText + " " + coursename1;
                                        }
                                        else if (splittemplate[j].ToString() == "Section")
                                        {
                                            if (sections != "")
                                            {
                                                MsgText = MsgText + " " + "" + sections + " Section";
                                            }
                                        }
                                        else if (splittemplate[j].ToString() == "Thank You")
                                        {
                                            MsgText = MsgText + " " + splittemplate[j].ToString();
                                        }
                                        else if (splittemplate[j].ToString() == "Absent")
                                        {
                                            if (!hourWiseAbsent)
                                            {
                                                if (atnddate == currentdt)
                                                {
                                                    MsgText = MsgText + "Absent today";
                                                }
                                                else
                                                {
                                                    MsgText = MsgText + "Absent on " + attnddt + "";
                                                }
                                            }
                                            else
                                            {
                                                MsgText = MsgText + " " + Hour + " hour Absent";
                                            }
                                        }
                                        else if (splittemplate[j].ToString() == "Absent hours")
                                        {

                                            MsgText = MsgText + " Absent hours: " + absent;
                                        }
                                        //22/09/16
                                        else if (splittemplate[j].ToString() == "Date")
                                        {
                                            MsgText = MsgText + " Date: " + dt.ToString("dd/MM/yyyy") + "";
                                        }
                                        else if (splittemplate[j].ToString() == "HOD")
                                        {
                                            if (hodphone.Trim() != "")
                                            {
                                                MsgText = MsgText + " - " + hodphone;
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
                                            MsgText = MsgText + " " + appno;
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
                    else
                    {
                        string hr = string.Empty;
                        if (!hourWiseAbsent)
                        {
                            hr = " today";
                        }
                        else
                        {
                            string[] split1 = date.Split(new Char[] { '-' });
                            string datefrom1 = split[0].ToString() + "-" + split[1].ToString() + "-" + split[2].ToString();

                            hr = " on " + datefrom1 +" "+ Hour + "hour";
                        }

                        MsgText = "Dear Parent, Good Morning. This Message from" + " " + collegename + ". Your ward " + dsMobile.Tables[0].Rows[0]["StudName"].ToString() + " of " + coursename + " is found absent " + hr + ".  Conducted Hours:" + total + " Absent Hours:" + absent + ". Thank you";
                    }
                    if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                    {
                        string studentAppNo = Convert.ToString(dsMobile.Tables[0].Rows[0]["app_no"]).Trim();
                        for (int jj1 = 0; jj1 < ds1.Tables[0].Rows.Count; jj1++)
                        {
                            bool checkHourAbsentees = false;
                            bool isSentAbsentees = true;
                            if (hourWiseAbsent)
                            {
                                checkHourAbsentees = true;
                            }
                            DataView dvSendSMSDetails = new DataView();
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {

                                if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                                {
                                    RecepientNo = Convert.ToString(dsMobile.Tables[0].Rows[0]["FatherMobile"]).Trim();
                                    string getval = da.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[1].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //Modified By Srinath
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
                                        //isSentAbsentees = false;
                                        isSentAbsentees = true;  //added by Mullai
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
                                    //smsreport(strpath, isst, dt);
                                    int nofosmssend = 0;
                                    if (isSentAbsentees)
                                        nofosmssend = da.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0", "absentees", studentAppNo);
                                }
                            }
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                    string getval = da.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[1].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //Modified By SRinath /2/2014
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
                                        isSentAbsentees = true;
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
                                    //smsreport(strpath, isst, dt);
                                    int nofosmssend = 0;
                                    if (isSentAbsentees)
                                        nofosmssend = da.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0", "absentees", studentAppNo);
                                }
                            }
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                    string getval = da.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[1].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //Modified By Srinatrh 8/2/2014
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
                                        isSentAbsentees = true;
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
                                    //smsreport(strpath, isst, dt);
                                    int nofosmssend = 0;
                                    if (isSentAbsentees)
                                        nofosmssend = da.send_sms(user_id, Session["collegecode"].ToString(), Session["usercode"].ToString(), RecepientNo, MsgText, "0", "absentees", studentAppNo);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        //Response.Write("<script>window.open('" + openpath + "')</script>");
        //Response.Redirect(openpath);
        //Response.RedirectToRoute(openpath);
    }
    protected void HyperLink1_PreRender(object sender, EventArgs e)
    {
        //HyperLink1.NavigateUrl = openpath;
    }
    protected void openfile(object sender, EventArgs e)
    {
        //hpl.GetRouteUrl(openpath);
    }
    protected void RadioSubject_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            ddlnoofanswers.SelectedIndex = 0;
            txtqtnname.Text = string.Empty;

            radiotough1.Checked = true;
            btnSave.Enabled = true;
            btnqtnupdate.Enabled = false;
            sprdretrivedate();
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void RadioGeneral_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            ddlnoofanswers.SelectedIndex = 0;
            txtquestion1.Text = string.Empty;
            radiotough1.Checked = true;
            btnSave.Enabled = true;
            btnqtnupdate.Enabled = false;
            sprdretrivedate();
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        //modify jairam  **************** 04-09-2014 *********//
        ddlnoofanswers.SelectedIndex = 0;
        txtqtnname.Text = string.Empty;
        GridView4.Visible = false;
        btnqtnsave.Enabled = true;
        btnqtnupdate.Enabled = false;
        btndeleteatndqtn.Enabled = false;

        Session["qtn_no"] = string.Empty;
    }
    protected void btnqtndelete_Click(object sender, EventArgs e)
    {
        try
        {
            lblnorec.Visible = false;
            if (Session["qtn_no"].ToString() != "")
            {
                string deletemodquery = "delete from questionaddition where collegecode=" + Session["collegecode"].ToString() + " and question_no=" + Session["qtn_no"].ToString() + "";
                //Modified by srinath 12/8/2013
                //  SqlCommand deletemodquerycmd = new SqlCommand(deletemodquery, con1);
                //con1.Close();
                //con1.Open();
                // deletemodquerycmd.ExecuteNonQuery();
                int insert = da.update_method_wo_parameter(deletemodquery, "Text");
                Session["qtn_no"] = string.Empty;
                string ctsname = "Update the Objective Type Information";//saranya
                string entrycode = Session["Entry_Code"].ToString();
                string PageName = "Student Attendance";
                string batchyear = Session["batch_year"].ToString();
                string TimeOfAttendance = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                string DateOfAttendence = DateTime.Now.ToString("MM/dd/yyy");
                da.insertUserActionLog(entrycode, batchyear, degree_code, semester, sections, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 3);//saranya

                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
                ddlnoofanswers.SelectedIndex = 0;
                txtqtnname.Text = string.Empty;

                radiotough1.Checked = true;
                btnqtnupdate.Enabled = false;
                btnqtndelete.Enabled = false;
                btnqtnsave.Enabled = true;
                sprdretrivedate();
                txtquestion1.Text = string.Empty;

            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void exit_sliplist_Click(object sender, EventArgs e)
    {
        pnl_sliplist.Visible = false;
        btnsliplist.Enabled = true;
    }
    public string findday(string curday, string deg_code, string semester, string batch_year, string sdate, string no_days, string stastdayorder)
    {
        int holiday = 0;
        if (no_days == "")
            return "";
        if (sdate != "")
        {
            string[] sp_date = sdate.Split(new Char[] { '-' });
            string start_date = sp_date[1].ToString() + "-" + sp_date[2].ToString() + "-" + sp_date[0].ToString();
            DateTime dt1 = Convert.ToDateTime(start_date);
            DateTime dt2 = Convert.ToDateTime(curday);
            TimeSpan ts = dt2 - dt1;
            string query1 = "select count(*)as count from holidaystudents  where degree_code=" + deg_code.ToString() + " and semester=" + semester.ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "' and isnull(Not_include_dayorder,0)<>'1'";//01.03.17 barath";";
            string holday = da.GetFunction(query1);
            if (holday != "")
                holiday = Convert.ToInt32(holday);
            int dif_days = ts.Days;
            //Added by Srinath 10/9/2013
            string leave = da.GetFunction(" select Holiday_desc from holidaystudents  where degree_code=" + deg_code.ToString() + "  and semester=" + semester.ToString() + " and  holiday_date='" + dt2.ToString("yyyy-MM-dd") + "' ");
            if (leave != null && leave != "0")
            {
                dif_days = dif_days + 1;
            }
            //=================Added by srinath 4/9/2014==============================================================================
            int dayorderchangedate = 0;
            try
            {
                string strdayorder = "select * from tbl_consider_day_order where Degree_code='" + deg_code.ToString() + "' and Batch_year='" + batch_year + "' and Semester='" + semester + "' and ((From_Date between '" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "') or (To_Date between '" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "'))";
                DataSet dsdayorderchange = da.select_method_wo_parameter(strdayorder, "Text");
                if (dsdayorderchange.Tables.Count > 0 && dsdayorderchange.Tables[0].Rows.Count > 0)
                {
                    for (int doc = 0; doc < dsdayorderchange.Tables[0].Rows.Count; doc++)
                    {
                        DateTime dtdcf = Convert.ToDateTime(dsdayorderchange.Tables[0].Rows[doc]["From_Date"].ToString());
                        DateTime dtdct = Convert.ToDateTime(dsdayorderchange.Tables[0].Rows[doc]["To_Date"].ToString());
                        for (DateTime dtdcst = dtdcf; dtdcst <= dtdct; dtdcst = dtdcst.AddDays(1))
                        {
                            if (dtdcst <= dt2)
                            {
                                dayorderchangedate = dayorderchangedate + 1;
                            }
                        }
                    }
                }
                holiday = holiday + dayorderchangedate;
            }
            catch (Exception ex)
            {
                da.sendErrorMail(ex, "13", "NewStaff.aspx");
            }
            //=================End==================================================================================================
            int nodays = Convert.ToInt32(no_days);
            int order = (dif_days - holiday) % nodays;
            order = order + 1;
            if (stastdayorder.ToString().Trim() != "")
            {
                if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
                {
                    order = order + (Convert.ToInt16(stastdayorder) - 1);
                    if (order == (nodays + 1))
                        order = 1;
                    else if (order > nodays)
                        order = order % nodays;
                }
            }
            //-----------------------------------------------------------
            if (order.ToString() == "0")
            {
                order = Convert.ToInt32(no_days);
            }
            string findday = string.Empty;
            if (order == 1)
                findday = "mon";
            else if (order == 2) findday = "tue";
            else if (order == 3) findday = "wed";
            else if (order == 4) findday = "thu";
            else if (order == 5) findday = "fri";
            else if (order == 6) findday = "sat";
            else if (order == 7) findday = "sun";
            if (order >= 1)
            {
                Day_Order = Convert.ToString(order) + "-" + Convert.ToString(findday);
            }
            else
            {
                Day_Order = string.Empty;
            }
            return findday;
        }
        else
            return "";
    }
    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value.ToString();
            }
        }
        return null;
    }
    //Modified By Srinath 8/2/2014
    //public void GetUserapi(string user_id)
    //{
    //    try
    //    {
    //        if (user_id == "DEANSEC")
    //        {
    //            SenderID = "DEANSE";
    //            Password = "DEANSEC";
    //        }
    //        else if (user_id == "PROFCL")
    //        {
    //            SenderID = "PROFCL";
    //            Password = "PROFCL";
    //        }
    //        else if (user_id == "SASTHA")
    //        {
    //            SenderID = "SASTHA";
    //            Password = "SASTHA";
    //        }
    //        else if (user_id == "SSMCE")
    //        {
    //            SenderID = "SSMCE";
    //            Password = "SSMCE";
    //        }
    //        else if (user_id == "NECARE")
    //        {
    //            SenderID = "NECARE";
    //            Password = "NECARE";
    //        }
    //        else if (user_id == "SVCTCG")
    //        {
    //            SenderID = "SVCTCG";
    //            Password = "SVCTCG";
    //        }
    //        else if (user_id == "AGNICT")
    //        {
    //            SenderID = "AGNICT";
    //            Password = "AGNICT";
    //        }
    //        else if (user_id == "NANDHA")
    //        {
    //            SenderID = "NANDHA";
    //            Password = "NANDHA";
    //        }
    //        else if (user_id == "DHIRA")
    //        {
    //            SenderID = "DHIRAJ";
    //            Password = "DHIRA";
    //        }
    //        else if (user_id == "ANGEL123")
    //        {
    //            SenderID = "ANGELS";
    //            Password = "ANGEL123";
    //        }
    //        else if (user_id == "BALAJI12")
    //        {
    //            SenderID = "BALAJI";
    //            Password = "BALAJI12";
    //        }
    //        else if (user_id == "AKSHYA123")
    //        {
    //            SenderID = "AKSHYA";
    //            Password = "AKSHYA";
    //        }
    //        else if (user_id == "PPGITS")
    //        {
    //            SenderID = "PPGITS";
    //            Password = "PPGITS";
    //        }
    //        else if (user_id == "PETENG")
    //        {
    //            SenderID = "PETENG";
    //            Password = "PETENG";
    //        }
    //        else if (user_id == "JJCET")
    //        {
    //            SenderID = "JJCET";
    //            Password = "JJCET";
    //        }
    //        else if (user_id == "PSVCET")
    //        {
    //            SenderID = "PSVCET";
    //            Password = "PSVCET";
    //        }
    //        else if (user_id == "AMSECE")
    //        {
    //            SenderID = "AMSECE";
    //            Password = "AMSECE";
    //        }
    //        else if (user_id == "GKMCET")
    //        {
    //            SenderID = "GKMCET";
    //            Password = "GKMCET";
    //        }
    //        else if (user_id == "SLAECT")
    //        {
    //            SenderID = "SLAECT";
    //            Password = "SLAECT";
    //        }
    //        else if (user_id == "DCTSCE")
    //        {
    //            SenderID = "DCTSCE";
    //            Password = "DCTSCE";
    //        }
    //        else if (user_id == "DCTSCE")
    //        {
    //            SenderID = "DCTSCE";
    //            Password = "DCTSCE";
    //        }
    //        else if (user_id == "DCTSEC")
    //        {
    //            SenderID = "DCTSEC";
    //            Password = "DCTSEC";
    //        }
    //        else if (user_id == "DCTSBS")
    //        {
    //            SenderID = "DCTSBS";
    //            Password = "DCTSBS";
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
    //        else if (user_id == "SCTSBS")
    //        {
    //            SenderID = "SCTSBS";
    //            Password = "SCTSBS";
    //        }
    //        else if (user_id == "ESECED")
    //        {
    //            SenderID = "ESECED";
    //            Password = "ESECED";
    //        }
    //        else if (user_id == "IJAYAM")
    //        {
    //            SenderID = "IJAYAM";
    //            Password = "IJAYAM";
    //        }
    //        else if (user_id == "MPNMJS")
    //        {
    //            SenderID = "MPNMJS";
    //            Password = "MPNMJS";
    //        }
    //        else if (user_id == "EASACG")
    //        {
    //            SenderID = "EASACG";
    //            Password = "EASACG";
    //        }
    //        else if (user_id == "KTVRKP")
    //        {
    //            SenderID = "KTVRKP";
    //            Password = "KTVRKP";
    //        }
    //        else if (user_id == "SVSCBE")
    //        {
    //            SenderID = "SVSCBE";
    //            Password = "SVSCBE";
    //        }
    //        else if (user_id == "AIHTCH")
    //        {
    //            SenderID = "AIHTCH";
    //            Password = "AIHTCH";
    //        }
    //        else if (user_id == "NSNCET")
    //        {
    //            SenderID = "NSNCET";
    //            Password = "NSNCET";
    //        }
    //        else if (user_id == "SVICET")
    //        {
    //            SenderID = "SVICET";
    //            Password = "SVICET";
    //        }
    //        else if (user_id == "SSCENG")
    //        {
    //            SenderID = "SSCENG";
    //            Password = "SSCENG";
    //        }
    //        else if (user_id == "ECESMS")
    //        {
    //            SenderID = "ECESMS";
    //            Password = "ECESMS";
    //        }
    //        else if (user_id == "NGPTEC")
    //        {
    //            SenderID = "NGPTEC";
    //            Password = "NGPTEC";
    //        }
    //        else if (user_id == "NGPTEC")
    //        {
    //            SenderID = "NGPTEC";
    //            Password = "NGPTEC";
    //        }
    //        else if (user_id == "KSRIET")
    //        {
    //            SenderID = "KSRIET";
    //            Password = "KSRIET";
    //        }
    //        else if (user_id == "VCWSMS")
    //        {
    //            SenderID = "VCWSMS";
    //            Password = "VCWSMS";
    //        }
    //        else if (user_id == "PMCTEC")
    //        {
    //            SenderID = "PMCTEC";
    //            Password = "PMCTEC";
    //        }
    //        else if (user_id == "SRECCG")
    //        {
    //            SenderID = "SRECCG";
    //            Password = "SRECCG";
    //        }
    //        else if (user_id == "SCHCLG")
    //        {
    //            SenderID = "SCHCLG";
    //            Password = "SCHCLG";
    //        }
    //        else if (user_id == "TSMJCT")
    //        {
    //            SenderID = "TSMJCT";
    //            Password = "TSMJCT";
    //        }
    //        else if (user_id == "SRECTD")
    //        {
    //            SenderID = "SRECTD";
    //            Password = "SRECTD";
    //        }
    //        else if (user_id == "EICTPC")
    //        {
    //            SenderID = "EICTPC";
    //            Password = "EICTPC";
    //        }
    //        else if (user_id == "SHACLG")
    //        {
    //            SenderID = "SHACLG";
    //            Password = "SHACLG";
    //        }
    //        else if (user_id == "ARASUU")
    //        {
    //            SenderID = "ARASUU";
    //            Password = "ARASUU";
    //        }
    //        else if (user_id == "TECAAA")
    //        {
    //            SenderID = "TECAAA";
    //            Password = "TECAAA";
    //        }
    //        else if (user_id == "AAACET")
    //        {
    //            SenderID = "AAACET";
    //            Password = "AAACET";
    //        }
    //        else if (user_id == "SVISTE")
    //        {
    //            SenderID = "SVISTE";
    //            Password = "SVISTE";
    //        }
    //        else if (user_id == "AALIME")
    //        {
    //            SenderID = "AALIME";
    //            Password = "AALIME";
    //        }
    //        else if (user_id == "VRSCET")
    //        {
    //            SenderID = "VRSCET";
    //            Password = "VRSCET";
    //        }
    //        Session["api"] = user_id;
    //        Session["senderid"] = SenderID;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
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
    //        else if (user_id == "SVschl")
    //        {
    //            SenderID = "SVschl";
    //            Password = "SVschl";
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
    //        Session["api"] = user_id;
    //        Session["senderid"] = SenderID;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    public void smsreport(string uril, string isstaff, DateTime dt)
    {
        try
        {
            string date = dt.ToString("MM/dd/yyyy") + ' ' + DateTime.Now.ToString("hh:mm:ss");
            WebRequest request = WebRequest.Create(uril);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();
            string groupmsgid = string.Empty;
            groupmsgid = strvel;
            int sms = 0;
            string smsreportinsert = string.Empty;
            string[] split_mobileno = RecepientNo.Split(new Char[] { ',' });
            for (int icount = 0; icount <= split_mobileno.GetUpperBound(0); icount++)
            {
                smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date,sender_id)values( '" + split_mobileno[icount] + "','" + groupmsgid + "','" + MsgText + "','" + Session["collegecode"].ToString() + "','" + isstaff + "','" + date + "','" + Session["UserCode"].ToString() + "')";// Added by jairam 21-11-2014
                sms = da.insert_method(smsreportinsert, hat, "Text");
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    public bool Hour_lock(string degree_code, string batch_year, string semester, string prd, string secval)
    {
        string degree_var = string.Empty;
        string starttime = string.Empty;
        string endtime = string.Empty;
        string startperiod = string.Empty;
        string endperiod = string.Empty;
        string actualtime = string.Empty;
        string period = string.Empty;
        string[] sp = prd.Split(' ');
        DateTime current_time;
        DateTime start_time;
        DateTime end_time;
        bool lock_flag = false;
        if (sp.GetUpperBound(0) >= 1)
        {
            period = Convert.ToString(sp[1]);
        }
        hr_lock = false;
        string getlock = string.Empty;
        if (secval.Trim() != "")
        {
            getlock = da.GetFunction("select lockstatus from attendance_hrlock where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and semester='" + semester + "' and section='" + secval + "' and locktype=2");
        }
        else
        {
            getlock = da.GetFunction("select lockstatus from attendance_hrlock where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and semester='" + semester + "' and locktype=2 ");
        }
        if (getlock.Trim().ToLower() == "true" || getlock.Trim() == "1")
        {
            hr_lock = true;
        }
        if (hr_lock == true)
        {
            if (ht_period.Count > 0)
            {
                if (ht_period.Contains(Convert.ToString(period)))
                {
                    string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(period), ht_period));
                    string[] sp_rd_semi = contvar.Split(',');
                    if (sp_rd_semi.GetUpperBound(0) >= 1) //Get Mark attendance Hrs for lock
                    {
                        startperiod = Convert.ToString(sp_rd_semi[0]);
                        endperiod = Convert.ToString(sp_rd_semi[1]);
                        if (ht_bell.Count > 0)
                        {
                            degree_var = Convert.ToString(batch_year) + "-" + Convert.ToString(degree_code) + "-" + Convert.ToString(semester) + "-" + Convert.ToString(startperiod);
                            if (ht_bell.Contains(Convert.ToString(degree_var))) //Get period start time for lock
                            {
                                string contvar1 = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_bell));
                                string[] sp_rd_semi1 = contvar1.Split(',');
                                if (sp_rd_semi1.GetUpperBound(0) >= 1)
                                {
                                    starttime = Convert.ToString(sp_rd_semi1[0]);
                                }
                            }
                            degree_var = Convert.ToString(batch_year) + "-" + Convert.ToString(degree_code) + "-" + Convert.ToString(semester) + "-" + Convert.ToString(endperiod);
                            if (ht_bell.Contains(Convert.ToString(degree_var))) //Get period end time for lock
                            {
                                string contvar1 = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_bell));
                                string[] sp_rd_semi1 = contvar1.Split(',');
                                if (sp_rd_semi1.GetUpperBound(0) >= 1)
                                {
                                    endtime = Convert.ToString(sp_rd_semi1[1]);
                                }
                            }
                            string sql_stringvar = "SELECT LTRIM(RIGHT(CONVERT(VARCHAR(20), GETDATE(), 100), 7))as time";
                            ds_attndmaster = da.select_method(sql_stringvar, hat, "Text");
                            if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
                            {
                                actualtime = Convert.ToString(ds_attndmaster.Tables[0].Rows[0]["time"]);
                            }
                            if (starttime.ToString().Trim() != "" && endtime.ToString().Trim() != "" && actualtime.ToString().Trim() != "")
                            {
                                current_time = Convert.ToDateTime(actualtime);
                                start_time = Convert.ToDateTime(starttime);
                                end_time = Convert.ToDateTime(endtime);
                                if (current_time >= start_time && current_time <= end_time)
                                {
                                    lock_flag = false;
                                }
                                else
                                {
                                    lock_flag = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        return lock_flag;
    }
    private void PopulateTreeview_General(string subno)
    {
        try
        {
            this.tvyet.Nodes.Clear();
            HierarchyTrees hierarchyTrees = new HierarchyTrees();
            HierarchyTrees.HTree objHTree = null;
            strquerytext = "select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + subno + "'";
            ds.Reset();
            ds.Dispose();
            ds = da.select_method(strquerytext, hat, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objHTree = new HierarchyTrees.HTree();
                    objHTree.topic_no = int.Parse(ds.Tables[0].Rows[i]["Topic_no"].ToString());
                    objHTree.parent_code = int.Parse(ds.Tables[0].Rows[i]["parent_code"].ToString());
                    objHTree.unit_name = ds.Tables[0].Rows[i]["unit_name"].ToString();
                    hierarchyTrees.Add(objHTree);
                }
            }
            foreach (HierarchyTrees.HTree hTree in hierarchyTrees)
            {
                HierarchyTrees.HTree parentNode = hierarchyTrees.Find(delegate(HierarchyTrees.HTree emp) { return emp.topic_no == hTree.parent_code; });
                if (parentNode != null)
                {
                    foreach (TreeNode tn in tvyet.Nodes)
                    {
                        if (tn.Value == parentNode.topic_no.ToString())
                        {
                            tn.ChildNodes.Add(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
                        }
                        if (tn.ChildNodes.Count > 0)
                        {
                            foreach (TreeNode ctn in tn.ChildNodes)
                            {
                                RecursiveChild(ctn, parentNode.topic_no.ToString(), hTree);
                            }
                        }
                    }
                }
                else
                {
                    tvyet.Nodes.Add(new TreeNode(hTree.unit_name, hTree.topic_no.ToString()));
                }
                tvyet.ExpandAll();
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    public void RecursiveChild(TreeNode tn, string searchValue, HierarchyTrees.HTree hTree)
    {
        if (tn.Value == searchValue)
        {
            tn.ChildNodes.Add(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
        }
        if (tn.ChildNodes.Count > 0)
        {
            foreach (TreeNode ctn in tn.ChildNodes)
            {
                RecursiveChild(ctn, searchValue, hTree);
            }
        }
    }
    public class HierarchyTrees : List<HierarchyTrees.HTree>
    {
        public class HTree
        {
            private int m_topic_no;
            private int m_parent_code;
            private string m_unit_name;
            public int topic_no
            {
                get { return m_topic_no; }
                set { m_topic_no = value; }
            }
            public int parent_code
            {
                get { return m_parent_code; }
                set { m_parent_code = value; }
            }
            public string unit_name
            {
                get { return m_unit_name; }
                set { m_unit_name = value; }
            }
        }
    }

    //Start===================Added by Manikandan 16/08/2013=========================
    protected void scodetxt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            strquerytext = da.GetFunction("select staff_name from staffmaster where staffmaster.staff_code='" + scodetxt.SelectedValue.ToString() + "'");
            if (strquerytext.Trim() != "" && strquerytext != null && strquerytext.Trim() != "0")
            {
                snamelbl.Text = strquerytext + "!!!!!";
                snamelbl.ForeColor = Color.Green;
                ddlstaffname.SelectedValue = scodetxt.SelectedValue.ToString();
            }
            else
            {
                snamelbl.Text = "No Staff Available in this Code";
                snamelbl.ForeColor = Color.Red;
            }
            if (Session["Staff_Code_val"] != "")
            {
                snamelbl.Visible = true;
                snamelbl1.Visible = true;
            }
            string name_code = string.Empty;
            name_code = scodetxt.SelectedValue.ToString();
            Session["Staff_Code_val"] = name_code.ToString();
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void ddlstaffname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            strquerytext = da.GetFunction("select staff_name from staffmaster where staffmaster.staff_code='" + ddlstaffname.SelectedValue.ToString() + "'");
            if (strquerytext.Trim() != "" && strquerytext != null && strquerytext.Trim() != "0")
            {
                snamelbl.Text = strquerytext + "!!!!!";
                snamelbl.ForeColor = Color.Green;
                scodetxt.SelectedValue = ddlstaffname.SelectedValue.ToString();
            }
            else
            {
                snamelbl.Text = "No Staff Available in this Code";
                snamelbl.ForeColor = Color.Red;
            }
            if (Session["Staff_Code_val"] != "")
            {
                snamelbl.Visible = true;
                snamelbl1.Visible = true;
            }
            string name_code = string.Empty;
            name_code = scodetxt.SelectedValue.ToString();
            Session["Staff_Code_val"] = name_code.ToString();
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    public void bindstaff()
    {
        try
        {
            scodetxt.Items.Clear();
            string staff_name = string.Empty;
            string staff_code = string.Empty;
            strquerytext = "select distinct staff_name,m.staff_code from staffmaster m,stafftrans t,hrdept_master h,desig_master d,staff_selector st where m.resign<>1 and m.settled<>1 and m.staff_code = t.staff_code and t.dept_code = h.dept_code and t.desig_code = d.desig_code and latestrec = 1 and st.staff_code=m.staff_code and m.college_code = " + Session["collegecode"] + " order by staff_name";
            ds.Dispose();
            ds.Reset();
            ds = da.select_method(strquerytext, hat, "Text");
            //Hided by gowtham////////////////
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    staff_name = ds.Tables[0].Rows[i]["staff_name"].ToString();
            //    staff_code = ds.Tables[0].Rows[i]["staff_code"].ToString();
            //    ListItem acclist = new ListItem();
            //    acclist.Value = (staff_code.ToString());
            //    acclist.Text = (staff_name.ToString()) + "-" + (staff_code.ToString());
            //    scodetxt.Items.Add(staff_code);
            //    ddlstaffname.Items.Add(staff_name);
            //}
            /////////////////////////////
            //Added by gowtham ------------------------
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    scodetxt.DataSource = ds;
                    scodetxt.DataTextField = "staff_code";
                    scodetxt.DataValueField = "staff_code";
                    scodetxt.DataBind();
                    ddlstaffname.DataSource = ds;
                    ddlstaffname.DataTextField = "staff_name";
                    ddlstaffname.DataValueField = "staff_code";
                    ddlstaffname.DataBind();
                }
            }
            //------------------End--------------------
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        try
        {
            if ((Session["StafforAdmin"] == "Staff") || (Session["StafforAdmin"] == "Admin"))
            {
                //clearfield();
                Session["StafforAdmin"] = string.Empty;
                Session["clearschedulesession"] = "clear";
                Response.Redirect("adminschedulegrid.aspx");
            }
            else
            {
                Response.Redirect("Default_login.aspx");
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    public void clearfield()
    {
        try
        {
            if ((Session["StafforAdmin"] == "Staff") || (Session["StafforAdmin"] == "Admin"))
            {
                headerpanelhomework.Visible = false;
                //btnaddhme.Visible = false;
                pHeaderatendence.Visible = false;
                pBodyatendence.Visible = false;
                pHeaderlesson.Visible = false;
                pBodylesson.Visible = false;
                headerpanelnotes.Visible = false;
                pBodynotes.Visible = false;
                headerADDQuestion.Visible = false;
                pBodyaddquestion.Visible = false;
                headerquestionaddition.Visible = false;
                pBodyquestionaddition.Visible = false;
                ck_append.Visible = false;
                btnsliplist.Visible = false;
            }
            else
            {
                headerpanelhomework.Visible = true;
                //btnaddhme.Visible = true;
                pHeaderatendence.Visible = true;
                pBodyatendence.Visible = true;
                pHeaderlesson.Visible = true;
                pBodylesson.Visible = true;
                headerpanelnotes.Visible = true;
                pBodynotes.Visible = true;
                headerADDQuestion.Visible = true;
                pBodyaddquestion.Visible = true;
                headerquestionaddition.Visible = true;
                pBodyquestionaddition.Visible = true;
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }


    public string filterfunction()
    {
        string strorder = "ORDER BY Registration.Roll_No";
        string serialno = da.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
        if (serialno == "1")
        {
            strorder = "order by registration.serialno";
        }
        else
        {
            string orderby_Setting = da.GetFunction("select value from master_Settings where settings='order_by'");
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
        return strorder;
    }
    protected void OnTreeNodecompleteCheckChanged(object sender, EventArgs e)
    {
    }
    protected void tvcomplete_SelectedNodeChanged1(object sender, EventArgs e)
    {
    }

    protected void btnaddrow_Click(object sender, EventArgs e)
    {

    }
    protected void rbgraphics_checkchange(object sender, EventArgs e)
    {
        Slipentry.Visible = false;
        loadgraphics();
    }
    public void btnremovereason_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlreason.Items.Count > 0)
            {
                string collegecode = Session["collegecode"].ToString();
                string reason = ddlreason.SelectedItem.ToString();
                if (reason.Trim().ToLower() != "all" && reason.Trim() != "")
                {
                    string strquery = "delete textvaltable where TextVal='" + reason + "' and TextCriteria='Attrs' and college_code='" + collegecode + "'";
                    int a = da.update_method_wo_parameter(strquery, "Text");
                    loadreason();
                }
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    public void btnaddreason_Click(object sender, EventArgs e)
    {
        panel1.Visible = true;
    }
    public void btnreasonnew_Click(object sender, EventArgs e)
    {
        panel1.Visible = true;
        string collegecode = Session["collegecode"].ToString();
        string reason = txtreason.Text.ToString();
        if (reason.Trim() != "")
        {
            string strquery = "insert into textvaltable (TextVal,TextCriteria,college_code) values('" + reason + "','Attrs','" + collegecode + "')";
            int a = da.update_method_wo_parameter(strquery, "Text");
            txtreason.Text = string.Empty;
            loadreason();
        }
    }
    public void btnreasonexit_Click(object sender, EventArgs e)
    {
        panel1.Visible = false;
    }
    public void sendvoicecall(string rollno, string date, string hour, string batch, string degree)
    {
        try
        {
            string Gender = string.Empty;
            string Hour = hour;
            string hour_check = string.Empty;
            //UserEmailID =string.Empty;
            string roll = rollno;
            string batchyear = batch;
            string coursename = string.Empty;
            string collegename = string.Empty;
            string collaccronymn = string.Empty;
            string voicelanguage = string.Empty;
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
            string collquery = "Select collname from collinfo where college_code=" + Session["collegecode"].ToString() + "";
            DataSet datacol = new DataSet();
            datacol.Clear();
            datacol = da.select_method_wo_parameter(collquery, "Text");
            if (datacol.Tables.Count > 0 && datacol.Tables[0].Rows.Count > 0)
            {
                collegename = datacol.Tables[0].Rows[0]["collname"].ToString();
            }
            string degreequery = "select distinct Course_Name,Dept_Name from Department dep, Degree deg, course c where dep.Dept_Code=deg.Dept_Code and c.Course_Id=deg.Course_Id and deg.college_code =" + Session["collegecode"].ToString() + " and Degree_Code=" + degree + "";
            DataSet dscode = new DataSet();
            dscode = da.select_method_wo_parameter(degreequery, "Text");
            if (dscode.Tables.Count > 0 && dscode.Tables[0].Rows.Count > 0)
            {
                string course = dscode.Tables[0].Rows[0]["Course_Name"].ToString();
                string deptname = dscode.Tables[0].Rows[0]["Dept_Name"].ToString();
                coursename = course + "-" + deptname;
            }
            string str1 = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + Session["collegecode"].ToString() + "'";
            }
            else
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where college_code='" + Session["collegecode"].ToString() + "'";
            }
            bool flage = false;
            DataSet ds1;
            ds1 = da.select_method_wo_parameter(str1, "txt");
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
            if (check > 0)
            {
                check = 0;
                string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,VoiceLang from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + Session["collegecode"].ToString() + "'";
                DataSet dsMobile;
                dsMobile = da.select_method_wo_parameter(Phone, "txt");
                string str = string.Empty;
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    str = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + Session["collegecode"].ToString() + "'";
                }
                else
                {
                    str = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + Session["collegecode"].ToString() + "'";
                }
                string voicelang = Convert.ToString(dsMobile.Tables[0].Rows[0]["VoiceLang"]);
                if (voicelang != "")
                {
                    string langquery = string.Empty;
                    langquery = "select TextVal from textvaltable where TextCode  ='" + voicelang + "' and TextCriteria='PLang' and college_code=" + Session["collegecode"] + "";
                    DataSet datalang = new DataSet();
                    datalang = da.select_method_wo_parameter(langquery, "Text");
                    if (datalang.Tables[0].Rows.Count > 0)
                    {
                        voicelanguage = datalang.Tables[0].Rows[0]["TextVal"].ToString();
                    }
                }
                // voicelanguage = "English";
                DataSet ds;
                ds = da.select_method_wo_parameter(str, "txt");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && dsMobile.Tables.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                {
                    //    if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                    //    {
                    //        Gender = "Your Son ";
                    //    }
                    //    else
                    //    {
                    //        Gender = "Your daughter";
                    //    }
                    //    string studentname = dsMobile.Tables[0].Rows[0]["stud_name"].ToString();
                    //    string[] splitname = studentname.Split('.');
                    //    string finalstudentname = splitname[0].ToString();
                    //    if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\dear parents.wav") == true)
                    //    {
                    //        if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\good morning.wav") == true)
                    //        {
                    //            if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\this call from.wav") == true)
                    //            {
                    //                if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\javagar school.wav") == true)
                    //                {
                    //                    if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\" + Gender + ".wav") == true)
                    //                    {
                    //                        if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\" + finalstudentname + ".wav") == true)
                    //                        {
                    //                            if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\" + coursename.ToString() + ".wav") == true)
                    //                            {
                    //                                if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\is absent today.wav") == true)
                    //                                {
                    //                                    if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\at 7th.wav") == true)
                    //                                    {
                    //                                        if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\thank you.wav") == true)
                    //                                        {
                    //                                            string[] files = new string[10] { "C:\\Documents and Settings\\Admin\\Desktop\\cutter\\dear parents.wav", "C:\\Documents and Settings\\Admin\\Desktop\\cutter\\good morning.wav", "C:\\Documents and Settings\\Admin\\Desktop\\cutter\\this call from.wav", "C:\\Documents and Settings\\Admin\\Desktop\\cutter\\javagar school.wav",
                    //                                          "C:\\Documents and Settings\\Admin\\Desktop\\cutter\\" + Gender + ".wav" ,"C:\\Documents and Settings\\Admin\\Desktop\\cutter\\" + studentname + ".wav","C:\\Documents and Settings\\Admin\\Desktop\\cutter\\" + coursename.ToString() + ".wav","C:\\Documents and Settings\\Admin\\Desktop\\cutter\\is absent today.wav","C:\\Documents and Settings\\Admin\\Desktop\\cutter\\at 7th.wav","C:\\Documents and Settings\\Admin\\Desktop\\cutter\\thank you.wav"};
                    //                                            // WaveIO wa = new WaveIO();
                    //                                            Concatenate(Server.MapPath("~/UploadFiles/chinnamaili.wav"), files);
                    //                                            filepath = Server.MapPath("~/UploadFiles/chinnamaili.wav");
                    //                                            insertmethod(filepath);
                    //                                        }
                    //                                    }
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    FileInfo fileinfo = new FileInfo(filepath);
                    //    string filename = fileinfo.Name;
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
                    for (int jj1 = 0; jj1 < ds.Tables[0].Rows.Count; jj1++)
                    {
                        if (ds.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                            {
                                //  DateTime dt = Convert.ToDateTime(date);
                                MsgText = "ABSETN AT ";
                                RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                //Modified By Srinath
                                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                // string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                biz.lbinfotech.www.Data h = new biz.lbinfotech.www.Data();
                                string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString() + "");
                                string isst = "0";
                                //smsreport(strpath, isst, dt);
                            }
                        }
                        if (ds.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                            {
                                // DateTime dt = Convert.ToDateTime(date);
                                MsgText = " ABSETN AT ";
                                RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                //Modified By SRinath /2/2014
                                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                //  string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                biz.lbinfotech.www.Data h = new biz.lbinfotech.www.Data();
                                string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString() + "");
                                string isst = "0";
                                //smsreport(strpath, isst, dt);
                            }
                        }
                        if (ds.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                            {
                                MsgText = " ABSENT AT";
                                RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                //Modified By Srinatrh 8/2/2014
                                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                //string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                biz.lbinfotech.www.Data h = new biz.lbinfotech.www.Data();
                                string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString() + "");
                                string isst = "0";
                                //smsreport(strpath, isst, dt);
                            }
                        }
                    }
                    //}
                    //}
                }
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    public void btnclosenotes_Click(object sender, EventArgs e)
    {
        pnotesuploadadd.Visible = false;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            pnotesuploadadd.Visible = true;
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void chkalterlession_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkalterlession.Checked == true)
            {
                Panelyet.Width = 312;
                Panelcomplete.Width = 312;
                Plessionalter.Width = 312;
                loadalternode();
                Plessionalter.Visible = true;
            }
            else
            {
                Panelyet.Width = 460;
                Panelcomplete.Width = 460;
                Plessionalter.Visible = false;
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    public void getspecial_hr(string temp_date, string subject_no, DataSet dsalldetails)
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
                string splhr_query_master = "select spa.roll_no,spa.attendance,spa.hrdet_no from  specialhr_attendance spa,specialhr_details spd where spa.hrdet_no=spd.hrdet_no and spd.subject_no='" + subject_no + "' and spd.hrdet_no in(" + hrdetno + ") order by spa.hrdet_no";
                DataSet dsval = da.select_method_wo_parameter(splhr_query_master, "Text");
                DataView dvsphratt = new DataView();
                dsalldetails.Tables[0].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString() + "' and subject_no='" + subject_no + "'";
                DataView dvlabhr = dsalldetails.Tables[0].DefaultView;
                for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                {
                    string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                    dsval.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                    dvsphratt = dsval.Tables[0].DefaultView;
                    for (int spo = 0; spo < dvsphratt.Count; spo++)
                    {
                        string attval = dvsphratt[spo][1].ToString();
                        string value = Attmark(attval);
                        int columno = 0;
                        if (hatsphco.Contains(dvsphratt[spo][2].ToString()))
                        {
                            columno = Convert.ToInt32(hatsphco[dvsphratt[spo][2].ToString()]);
                        }
                        if ((dvsphratt[spo][1].ToString()) != "8")
                        {
                            if (Attmark(dvsphratt[spo][1].ToString()) != "HS")
                            {
                                bool checkedFeeOfRoll = false;
                                if (dicFeeOfRollStudents.ContainsKey(rollno.Trim().ToLower()) && dicFeeOnRollStudents.ContainsKey(rollno.Trim().ToLower()))
                                {
                                    DateTime[] dtFeeOfRoll = dicFeeOfRollStudents[rollno.Trim().ToLower()];
                                    DateTime dtTempDate = new DateTime();
                                    DateTime.TryParseExact(temp_date, "MM/dd/yyyy", null, DateTimeStyles.None, out dtTempDate);

                                    if (dtTempDate >= dtFeeOfRoll[0])
                                    {
                                        bool hasRollOff = false;
                                        DateTime dtDefaultDate = new DateTime(1900, 1, 1);//SqlServer Default Date
                                        if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && dtTempDate < dtFeeOfRoll[1])
                                        {
                                            hasRollOff = true;
                                            checkedFeeOfRoll = true;
                                        }
                                        else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 1)
                                        {
                                            hasRollOff = true;
                                            checkedFeeOfRoll = true;
                                        }
                                        else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && dtFeeOfRoll[1] == dtDefaultDate)
                                        {
                                            hasRollOff = true;
                                            checkedFeeOfRoll = true;
                                        }
                                        else
                                        {
                                            hasRollOff = false;
                                            checkedFeeOfRoll = false;
                                        }
                                        if (hasRollOff)
                                        {
                                            //if (has_attnd_masterset.ContainsKey("2"))
                                            //{
                                            //    if (has_load_rollno.Contains(rollno + '-' + subjectno))
                                            //    {
                                            //        present_count = Convert.ToInt16(has_load_rollno[rollno + '-' + subjectno]);
                                            //        present_count++;
                                            //        has_load_rollno[rollno + '-' + subjectno] = present_count;
                                            //    }
                                            //    else
                                            //    {
                                            //        has_load_rollno.Add(rollno + '-' + subjectno, 1);
                                            //    }
                                            //}
                                            if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subject_no.Trim()))
                                            {
                                                present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subject_no.Trim()]);
                                                present_count++;
                                                has_total_attnd_hour[rollno.Trim() + '-' + subject_no.Trim()] = present_count;
                                            }
                                            else
                                            {
                                                has_total_attnd_hour.Add(rollno.Trim() + '-' + subject_no.Trim(), 1);
                                            }
                                        }
                                    }
                                }
                                if (!checkedFeeOfRoll)
                                {
                                    if (has_attnd_masterset.ContainsKey(attval))
                                    {
                                        if (has_load_rollno.Contains(rollno.Trim() + '-' + subject_no.Trim()))
                                        {
                                            present_count = Convert.ToInt16(has_load_rollno[rollno.Trim() + '-' + subject_no.Trim()]);
                                            present_count++;
                                            has_load_rollno[rollno.Trim() + '-' + subject_no.Trim()] = present_count;
                                        }
                                        else
                                        {
                                            has_load_rollno.Add(rollno.Trim() + '-' + subject_no.Trim(), 1);
                                        }
                                    }
                                    if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subject_no.Trim()))
                                    {
                                        present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subject_no.Trim()]);
                                        present_count++;
                                        has_total_attnd_hour[rollno.Trim() + '-' + subject_no.Trim()] = present_count;
                                    }
                                    else
                                    {
                                        has_total_attnd_hour.Add(rollno.Trim() + '-' + subject_no.Trim(), 1);
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
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
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
            dsload = da.select_method_wo_parameter(SelQ, "Text");
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
                    int save = da.update_method_wo_parameter(InsQ, "Text");
                }
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
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
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
        return attVal;
    }

    #endregion
    private string findDayName(byte dayOrder)
    {
        string dayName = string.Empty;
        switch (dayOrder)
        {
            case 0:
                dayName = string.Empty;
                break;
            case 1:
                dayName = "mon";
                break;
            case 2:
                dayName = "tue";
                break;
            case 3:
                dayName = "wed";
                break;
            case 4:
                dayName = "thu";
                break;
            case 5:
                dayName = "fri";
                break;
            case 6:
                dayName = "sat";
                break;
            case 7:
                dayName = "sun";
                break;
            default:
                break;
        }
        return dayName;
    }
    private void GetFeeOfRollStudent(ref Dictionary<string, DateTime[]> dicFeeOffRollStudents, ref Dictionary<string, byte> dicFeeOnRoll, string fromDate, string toDate = null)
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
            //qry = "select roll_no, Convert(varchar(50),curr_date,103) as curr_date,infr_type,Convert(varchar(50),CAST(ack_date as DateTime),103) as ack_date,Convert(varchar(50),feeOnRollDate,103) as feeOnRollDate,ack_diss,ack_fine,ack_remarks,ack_susp,ack_warn,tot_days,fine_amo,semester,ack_fee_of_roll,Remark,Convert(varchar(50),suspendFromDate,103) as suspendFromDate,Convert(varchar(50),suspendToDate,103) as suspendToDate from stucon where (ack_fee_of_roll=1 or feeOnRollDate is not null) " + qryFeeOfRollDate;
            string qry = "select roll_no,Convert(varchar(50),curr_date,103) as curr_date,Convert(varchar(50),CAST(ack_date as DateTime),103) as ack_date,Convert(varchar(50),feeOnRollDate,103) as feeOnRollDate,semester,ack_fee_of_roll from stucon where (ack_fee_of_roll=1 or feeOnRollDate is not null) and  CAST(ack_date as DateTime) <='" + dtFromDate.ToString("MM/dd/yyyy") + "' order by CAST(ack_date as DateTime) desc";
            dsFeeOfRollDate = da.select_method_wo_parameter(qry, "text");
            if (dsFeeOfRollDate.Tables.Count > 0 && dsFeeOfRollDate.Tables[0].Rows.Count > 0)
            {
                dicFeeOffRollStudents.Clear();
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
                    if (!dicFeeOffRollStudents.ContainsKey(rollNo.Trim().ToLower()))
                    {
                        dicFeeOffRollStudents.Add(rollNo.Trim().ToLower(), dtFeeRoll);
                    }
                    if (!dicFeeOnRoll.ContainsKey(rollNo.Trim().ToLower().ToLower()))
                    {
                        dicFeeOnRoll.Add(rollNo.Trim().ToLower(), FeeOnRoll);
                    }
                    //string rollNo = Convert.ToString(drFeeOfRoll[""]).Trim();
                }
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    private bool CheckSchoolOrCollege(string collegeCode)
    {
        bool isSchoolOrCollege = false;
        try
        {
            if (!string.IsNullOrEmpty(collegeCode))
            {
                //qry = "select ISNULL(InstType,'0') as InstType,case when ISNULL(InstType,'0')='0' then 'College' when ISNULL(InstType,'0')='1' then 'School' end as CollegeOrSchool from collinfo where college_code='" + collegeCode + "'";
                string qry = "select ISNULL(InstType,'0') as InstType from collinfo where college_code='" + collegeCode + "'";
                string insType = da.GetFunction(qry);
                if (!string.IsNullOrEmpty(insType) && insType.Trim() != "0")
                {
                    isSchoolOrCollege = true;
                }
                else
                {
                    isSchoolOrCollege = false;
                }

            }
            return isSchoolOrCollege;
        }
        catch
        {
            return false;
        }
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirmBox.Visible = false;
            divConfirm.Visible = false;
        }
        catch
        {
        }
    }
    private bool isChoiceBasedSystem(string batchYear)
    {
        bool staffSelector = false;
        try
        {

            string qryStudeStaffSelector = string.Empty;   //Session["collegecode"].ToString()
            string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString() + "'");
            string[] splitminimumabsentsms = minimumabsentsms.Split('-');
            if (splitminimumabsentsms.Length == 2)
            {
                int batchyearsetting = 0;
                int.TryParse(Convert.ToString(splitminimumabsentsms[1]).Trim(), out batchyearsetting);
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    if (Convert.ToInt32(batchYear.ToString()) >= batchyearsetting)
                    {
                        staffSelector = true;
                    }
                }
            }
            else if (splitminimumabsentsms.Length > 0)
            {
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    staffSelector = true;
                }
            }
            //if (staffSelector)
            //{
            //    qryStudeStaffSelector = " and sc.staffcode like '%" + staffcode + "%'";
            //}
        }
        catch
        {
        }
        return staffSelector;
    }

    //Start===================Added by Rajkumar=========================
    public void loadstafspread()
    {
        try
        {
            dtTTDisp.Columns.Add("DateDisp");
            dtTTDisp.Columns.Add("DateVal");
            dtTTDisp.Columns.Add("DayOrder");
            dtTTDisp.Columns.Add("P1ValDisp");
            dtTTDisp.Columns.Add("P1Val");
            dtTTDisp.Columns.Add("TT_1");
            dtTTDisp.Columns.Add("P2ValDisp");
            dtTTDisp.Columns.Add("P2Val");
            dtTTDisp.Columns.Add("TT_2");
            dtTTDisp.Columns.Add("P3ValDisp");
            dtTTDisp.Columns.Add("P3Val");
            dtTTDisp.Columns.Add("TT_3");
            dtTTDisp.Columns.Add("P4ValDisp");
            dtTTDisp.Columns.Add("P4Val");
            dtTTDisp.Columns.Add("TT_4");
            dtTTDisp.Columns.Add("P5ValDisp");
            dtTTDisp.Columns.Add("P5Val");
            dtTTDisp.Columns.Add("TT_5");
            dtTTDisp.Columns.Add("P6ValDisp");
            dtTTDisp.Columns.Add("P6Val");
            dtTTDisp.Columns.Add("TT_6");
            dtTTDisp.Columns.Add("P7ValDisp");
            dtTTDisp.Columns.Add("P7Val");
            dtTTDisp.Columns.Add("TT_7");
            dtTTDisp.Columns.Add("P8ValDisp");
            dtTTDisp.Columns.Add("P8Val");
            dtTTDisp.Columns.Add("TT_8");
            dtTTDisp.Columns.Add("P9ValDisp");
            dtTTDisp.Columns.Add("P9Val");
            dtTTDisp.Columns.Add("TT_9");
            dtTTDisp.Columns.Add("P10ValDisp");
            dtTTDisp.Columns.Add("P10Val");
            dtTTDisp.Columns.Add("TT_10");


            Session["chkdoubledayno"] = "";
            Labelstaf.Visible = false;
            string sql_s = string.Empty;
            string Strsql = string.Empty;
            string SchOrder1 = string.Empty;
            string SqlBatchYear = string.Empty;
            string SqlPrefinal1 = string.Empty;
            string SqlPrefinal2 = string.Empty;
            string SqlPrefinal3 = string.Empty;
            string SqlPrefinal4 = string.Empty;
            DataSet dsgetvalue = new DataSet();
            string getquery = string.Empty;
            string SqlFinal = string.Empty;
            string SqlFinal1 = string.Empty;
            string sql1 = string.Empty;
            string tmp_varstr = string.Empty;
            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };
            lbl_alert.Visible = false;
            lblmanysubject.Visible = false;
            ddlselectmanysub.Visible = false;
            DataSet dsalterperiod = new DataSet();
            Hashtable hatsublab = new Hashtable();
            string check_lab = string.Empty;
            DataSet dsstuatt = new DataSet();
            Hashtable hatvalue = new Hashtable();
            Hashtable ha = new Hashtable();
            Hashtable h3 = new Hashtable();
            string sectionsvalue = string.Empty;
            string sectionvar = string.Empty;
            string date1;
            string date2;
            string datefrom;
            string dateto;
            string sqlstr = string.Empty;
            int noofhrs = 0;
            Slipentry.Visible = false;
            //=============================================================================================================
            string vari = string.Empty;
            ht_sch.Clear();

            hat.Clear();
            hat.Add("college_code", Session["collegecode"].ToString());
            string sql_stringvar = "sp_select_details_staff";
            ds_attndmaster.Dispose();
            ds_attndmaster.Reset();
            ds_attndmaster = da.select_method(sql_stringvar, hat, "sp");
            if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[0].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[0].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[0].Rows[pcont]["semester"]);
                    if (!ht_sch.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[0].Rows[pcont]["SchOrder"] + "," + ds_attndmaster.Tables[0].Rows[pcont]["nodays"];
                        ht_sch.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }
            ht_sdate.Clear();

            if (ds_attndmaster.Tables.Count > 1 && ds_attndmaster.Tables[1].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[1].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["batch_year"]) + "-" + Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[1].Rows[pcont]["semester"]);
                    if (!ht_sdate.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[1].Rows[pcont]["sdate"] + "," + ds_attndmaster.Tables[1].Rows[pcont]["starting_dayorder"];
                        ht_sdate.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }
            ht_bell.Clear();

            if (ds_attndmaster.Tables.Count > 2 && ds_attndmaster.Tables[2].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[2].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["batch_year"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["semester"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["period1"]);
                    if (!ht_bell.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[2].Rows[pcont]["start_time"] + "," + ds_attndmaster.Tables[2].Rows[pcont]["end_time"];
                        ht_bell.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }
            ht_period.Clear();
            //sql_stringvar = "select * from attendance_hrlock where college_code=" + Session["collegecode"].ToString() + " order by lock_hr";
            //ds_attndmaster = da.select_method(sql_stringvar, hat, "Text");
            if (ds_attndmaster.Tables.Count > 3 && ds_attndmaster.Tables[3].Rows.Count > 0)
            {
                for (int pcont = 0; pcont < ds_attndmaster.Tables[3].Rows.Count; pcont++)
                {
                    degree_var = Convert.ToString(ds_attndmaster.Tables[3].Rows[pcont]["lock_hr"]);
                    if (!ht_period.Contains(Convert.ToString(degree_var)))
                    {
                        vari = ds_attndmaster.Tables[3].Rows[pcont]["markatt_from"] + "," + ds_attndmaster.Tables[3].Rows[pcont]["markatt_to"];
                        ht_period.Add(degree_var, Convert.ToString(vari));
                    }
                }
            }
            hr_lock = false;
            if (ds_attndmaster.Tables.Count > 4 && ds_attndmaster.Tables[4].Rows.Count > 0)
            {
                string locktrue = ds_attndmaster.Tables[4].Rows[0]["hrlock"].ToString();
                if (locktrue == "1")
                {
                    hr_lock = true;
                }
            }

            string degreename = string.Empty;

            Hashtable hatdegreename = new Hashtable();
            if (ds_attndmaster.Tables.Count > 5 && ds_attndmaster.Tables[5].Rows.Count > 0)
            {
                for (int i = 0; i < ds_attndmaster.Tables[5].Rows.Count; i++)
                {
                    if (!hatdegreename.Contains(ds_attndmaster.Tables[5].Rows[i]["Degree_Code"].ToString()))
                    {
                        hatdegreename.Add(ds_attndmaster.Tables[5].Rows[i]["Degree_Code"].ToString(), ds_attndmaster.Tables[5].Rows[i]["course"].ToString() + '-' + ds_attndmaster.Tables[5].Rows[i]["dept_acronym"].ToString());
                    }
                }
            }
            //string datefrom;
            //string dateto;
            date1 = tbfdate.Text.ToString();
            string[] split = date1.Split(new Char[] { '-' });
            datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
            date2 = tbtodate.Text.ToString();
            string[] split1 = date2.Split(new Char[] { '-' });
            dateto = split1[1].ToString() + "-" + split1[0].ToString() + "-" + split1[2].ToString();

            string ddf = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
            string ddt = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
            if (Convert.ToInt16(split[0].ToString()) <= 31 && Convert.ToInt16(split[1].ToString()) <= 12 && Convert.ToInt16(split[2].ToString()) <= Convert.ToInt16(Session["curr_year"]))
            {
                if (Convert.ToInt16(split1[0].ToString()) <= 31 && Convert.ToInt16(split1[1].ToString()) <= 12 && Convert.ToInt16(split1[2].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                {
                    long days = -1;
                    DateTime dt1 = DateTime.Now.AddDays(-6);
                    DateTime dt2 = DateTime.Now;
                    try
                    {
                        dt1 = Convert.ToDateTime(ddf);
                        dt2 = Convert.ToDateTime(ddt);
                        TimeSpan t = dt2.Subtract(dt1);
                        days = t.Days;
                    }
                    catch
                    {
                        try
                        {
                            dt1 = Convert.ToDateTime(date1);
                            dt2 = Convert.ToDateTime(date2);
                            TimeSpan t = dt2.Subtract(dt1);
                            days = t.Days;
                        }
                        catch
                        {
                            Labelstaf.Text = ddf + ddt;
                        }
                    }
                    if (days < 0)
                    {

                        headerpanelhomework.Visible = false;
                        //btnaddhme.Visible = false;

                        Labelstaf.Visible = true;
                        gridTimeTable.Visible = false;
                        GridView1.Visible = false;
                        pHeaderatendence.Visible = false;
                        pHeaderlesson.Visible = false;
                        headerpanelnotes.Visible = false;
                        headerADDQuestion.Visible = false;
                        headerquestionaddition.Visible = false;
                        pBodyatendence.Visible = false;
                        pBodylesson.Visible = false;
                        headerpanelnotes.Visible = false;
                        pBodynotes.Visible = false;
                        pBodyquestionaddition.Visible = false;
                        return;
                    }
                    if (days >= 0)
                    {
                        headerpanelhomework.Visible = false;
                        //btnaddhme.Visible = false;
                        //load_attnd_spread();
                        Labelstaf.Visible = false;
                        gridTimeTable.Visible = false;
                        GridView1.Visible = false;
                        pHeaderatendence.Visible = false;
                        pHeaderlesson.Visible = false;
                        headerpanelnotes.Visible = false;
                        pBodyatendence.Visible = false;
                        pBodylesson.Visible = false;
                        pBodynotes.Visible = false;
                        headerADDQuestion.Visible = false;
                        headerquestionaddition.Visible = false;
                        pBodyquestionaddition.Visible = false;
                        string[] differdays = new string[days];
                        //sqlstr = da.GetFunction("select max(No_of_hrs_per_day) from PeriodAttndSchedule");
                        noofhrs = 0;
                        if (ds_attndmaster.Tables.Count > 6 && ds_attndmaster.Tables[6].Rows.Count > 0)
                        {
                            if (ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString().Trim() != "" && ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString().Trim() != null && ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString().Trim() != "0")
                            {
                                #region magesh 16.8.18
                                string chkdoubleday = da.GetFunction("select * from doubledayorder where doubleDate between '" + datefrom + " ' and '" + dateto + "'");
                                if (chkdoubleday != "0")
                                {
                                    noofhrs = Convert.ToInt32(ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString()) * 2;
                                    Session["doublehour"] = noofhrs;
                                    isDoubleDay = true;

                                }
                                else
                                {
                                    Session["chkdoubledayno"] = "1";
                                    noofhrs = Convert.ToInt32(ds_attndmaster.Tables[6].Rows[0]["noofhours"].ToString());
                                    Session["doublehour"] = noofhrs;
                                }
                                #endregion magesh 16.8.18
                            }
                        }
                        if (noofhrs != 0)
                        {
                            DataRow drTT = null;

                            sql1 = string.Empty;
                            Strsql = string.Empty;
                            SqlFinal = string.Empty;
                            //Start===========Added by Manikandan 14/08/2013===============
                            string stafcode = string.Empty;
                            if ((Session["StafforAdmin"] == "Staff") || (Session["StafforAdmin"] == "Admin"))
                            {
                                ck_append.Visible = false;
                                btnsliplist.Visible = false;
                            }
                            else
                            {
                                ck_append.Visible = true;
                                btnsliplist.Visible = true;
                                //scheduleorattnd = 2;
                            }
                            if (Session["StafforAdmin"] == "Admin")
                            {
                                string stafnamecode = scodetxt.SelectedItem.ToString();
                                //string[] splitcode = stafnamecode.Split(new char[] { '-' });
                                // stafcode = splitcode[1];
                                stafcode = scodetxt.SelectedValue.ToString();
                            }
                            else
                            {
                                stafcode = Session["Staff_Code"].ToString();
                            }
                            //============================End==============================
                            for (int day_lp = 0; day_lp < 7; day_lp++)
                            {
                                strday = Days[day_lp].ToString();
                                sql1 = sql1 + "(";
                                tmp_varstr = string.Empty;
                                for (int i_loop = 1; i_loop <= noofhrs; i_loop++)
                                {
                                    Strsql = Strsql + strday + Convert.ToString(i_loop) + ",";
                                    if (tmp_varstr == "")
                                    {
                                        tmp_varstr = tmp_varstr + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line
                                    }
                                    else
                                    {
                                        tmp_varstr = tmp_varstr + " or " + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line
                                    }
                                }
                                if (day_lp != 6)
                                    tmp_varstr = tmp_varstr + ") or ";
                                else
                                    tmp_varstr = tmp_varstr + ")";
                                sql1 = sql1 + tmp_varstr.ToString();
                            }

                            ha.Clear();
                            ha.Add("StaffCode", stafcode);

                            DataSet dsperiod = da.select_method("GetStaffSchedule", ha, "sp");


                            //==========================End====================
                            DataView dvalternaet = new DataView();
                            DataView dvsemster = new DataView();
                            DataView dvholiday = new DataView();
                            DataView dvdaily = new DataView();
                            DataView dvsubject = new DataView();
                            DataView dvsublab = new DataView();
                            // remove collegecode by srinath // 02-09-2014
                            h3.Clear();
                            h3.Add("StaffCode", stafcode);
                            h3.Add("FromDate", ddf);
                            h3.Add("ToDate", ddt);
                            DataSet dsall = da.select_method("GetStaffTimeTableInfo", h3, "sp");
                            //**************added By Srinath 29Jan2015
                            string strstaffselector = string.Empty;
                            Hashtable hatholiday = new Hashtable();
                            int countholiday = 0;
                            int countholiday1 = 0;
                            int alt = 0;
                            dicalter.Clear();
                            if (dsperiod.Tables.Count > 0 && dsperiod.Tables[0].Rows.Count > 0)
                            {
                                for (int pre = 0; pre < dsperiod.Tables[0].Rows.Count; pre++)
                                {
                                    countholiday++;//added by Mullai
                                    //magesh 14.8.18
                                    string hoursdoubleday = Convert.ToString(Session["doublehour"]);
                                    int.TryParse(hoursdoubleday, out noofhrs);//magesh 14.8.18
                                    cur_camprevar = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]);
                                    string getdate = string.Empty;
                                    if (Convert.ToString(tmp_camprevar.Trim()) != Convert.ToString(cur_camprevar.Trim()))
                                    {
                                        strsction = string.Empty;
                                        if ((Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "") && (Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "-1"))
                                        {
                                            strsction = " and isnull(sections,'')='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                        }
                                        Hashtable h2 = new Hashtable();
                                        h2.Add("batchYear", dsperiod.Tables[0].Rows[pre]["batch_year"].ToString());
                                        h2.Add("DegCode", dsperiod.Tables[0].Rows[pre]["degree_code"].ToString());
                                        h2.Add("Sem", dsperiod.Tables[0].Rows[pre]["semester"].ToString());
                                        DataSet dsgetsub = da.select_method("GetStudentSubject", h2, "sp");

                                        DataView dtcurlab = new DataView();
                                        if (dsgetsub.Tables.Count > 0)
                                        {
                                            dsgetsub.Tables[0].DefaultView.RowFilter = " degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"] + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                            dtcurlab = dsgetsub.Tables[0].DefaultView;
                                        }
                                        Hashtable hatcurlab = new Hashtable();
                                        for (int cula = 0; cula < dtcurlab.Count; cula++)
                                        {
                                            string lasubno = dtcurlab[cula]["subject_no"].ToString();
                                            string labhour = dtcurlab[cula]["lab"].ToString();
                                            if (labhour.Trim() == "1" || labhour.Trim().ToLower() == "true")
                                            {
                                                if (!hatcurlab.Contains(lasubno))
                                                {
                                                    hatcurlab.Add(lasubno, lasubno);
                                                }
                                            }
                                        }
                                        Hashtable h1 = new Hashtable();
                                        Session["StaffSelector"] = "0";
                                        strstaffselector = string.Empty;   //Session["collegecode"].ToString()
                                        string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString() + "'");
                                        string[] splitminimumabsentsms = minimumabsentsms.Split('-');
                                        if (splitminimumabsentsms.Length == 2)
                                        {
                                            int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                                            if (splitminimumabsentsms[0].ToString() == "1")
                                            {
                                                if (Convert.ToInt32(dsperiod.Tables[0].Rows[pre]["batch_year"].ToString()) >= batchyearsetting)
                                                {
                                                    Session["StaffSelector"] = "1";
                                                }
                                            }
                                        }
                                        if (Session["StaffSelector"].ToString() == "1")
                                        {
                                            strstaffselector = " and s.staffcode like '%" + Session["Staff_Code"].ToString() + "%'";
                                        }
                                        h1.Clear();
                                        h1.Add("StaffCode", strstaffselector);
                                        h1.Add("section", strsction);
                                        h1.Add("batchYear", dsperiod.Tables[0].Rows[pre]["batch_year"]);
                                        h1.Add("DegCode", dsperiod.Tables[0].Rows[pre]["degree_code"].ToString());
                                        h1.Add("Sem", dsperiod.Tables[0].Rows[pre]["semester"].ToString());

                                        DataSet dssubstucount = da.select_method("GetStudentCount", h1, "sp");
                                        DataView dvsubstucount = new DataView();
                                        hatholiday.Clear();
                                        DataView duholiday = new DataView();
                                        if (dsall.Tables.Count > 2)
                                        {
                                            dsall.Tables[2].DefaultView.RowFilter = " degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " ";
                                            duholiday = dsall.Tables[2].DefaultView;
                                        }

                                        for (int i = 0; i < duholiday.Count; i++)
                                        {
                                            if (!hatholiday.Contains(duholiday[i]["holiday_date"].ToString()))
                                            {
                                                countholiday1++;  //added by Mullai
                                                hatholiday.Add(duholiday[i]["holiday_date"].ToString(), duholiday[i]["holiday_desc"].ToString());
                                            }
                                        }
                                        int frshlf = 0;
                                        int schlf = 0;
                                        DataView dvperiod = new DataView();
                                        if (dsall.Tables.Count > 6)
                                        {
                                            dsall.Tables[6].DefaultView.RowFilter = " degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and  semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                            dvperiod = dsall.Tables[6].DefaultView;
                                        }
                                        if (dvperiod.Count > 0)
                                        {
                                            string morhr = dvperiod[0]["mor"].ToString();
                                            string evehr = dvperiod[0]["mor"].ToString();
                                            if (morhr != null && morhr.Trim() != "")
                                            {
                                                frshlf = Convert.ToInt32(morhr);
                                            }
                                            if (evehr != null && evehr.Trim() != "")
                                            {
                                                schlf = Convert.ToInt32(evehr);
                                            }
                                        }
                                        string getcurrent_sem = string.Empty;
                                        DataView dvcurrsem = new DataView();
                                        if (dsall.Tables.Count > 5)
                                        {
                                            dsall.Tables[5].DefaultView.RowFilter = "degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and batch_year = '" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and isnull(sections,'')='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]).Trim() + "'";
                                            dvcurrsem = dsall.Tables[5].DefaultView;
                                        }
                                        if (dvcurrsem.Count > 0)
                                        {
                                            getcurrent_sem = dvcurrsem[0]["current_semester"].ToString();
                                        }
                                        if (Convert.ToString(getcurrent_sem) == Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]))
                                        {
                                            string semenddate = dsperiod.Tables[0].Rows[pre]["end_date"].ToString();
                                            string altersetion = string.Empty;
                                            if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() != "-1" && dsperiod.Tables[0].Rows[pre]["sections"].ToString() != null && dsperiod.Tables[0].Rows[pre]["sections"].ToString().Trim() != "")
                                            {
                                                altersetion = "  and isnull(Sections,'')='" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "'";
                                            }
                                            //===============================Start==============================================================
                                            Hashtable hatdc = new Hashtable();
                                            Dictionary<DateTime, byte> dicAlternateDayOrder = new Dictionary<DateTime, byte>();
                                            DataView dvdayorderchanged = new DataView();
                                            if (dsall.Tables.Count > 7)
                                            {
                                                dsall.Tables[7].DefaultView.RowFilter = "degree_code ='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "'  and batch_year = '" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and semester=" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "  ";
                                                dvdayorderchanged = dsall.Tables[7].DefaultView;
                                            }
                                            for (int dc = 0; dc < dvdayorderchanged.Count; dc++)
                                            {
                                                DateTime dtdcf = Convert.ToDateTime(dvdayorderchanged[dc]["from_date"].ToString());
                                                DateTime dtdct = Convert.ToDateTime(dvdayorderchanged[dc]["to_date"].ToString());
                                                string alternateDayOrder = Convert.ToString(dvdayorderchanged[dc]["DayOrder"]).Trim();
                                                byte alternateDay = 0;
                                                byte.TryParse(alternateDayOrder, out alternateDay);
                                                for (DateTime dtc = dtdcf; dtc <= dtdct; dtc = dtc.AddDays(1))
                                                {
                                                    if (!hatdc.Contains(dtc))
                                                    {
                                                        hatdc.Add(dtc, dtc);
                                                    }
                                                    if (!dicAlternateDayOrder.ContainsKey(dtc))
                                                    {
                                                        dicAlternateDayOrder.Add(dtc, alternateDay);
                                                    }
                                                }
                                            }
                                            //=================================End==============================================================

                                            Session["StaffSelector"] = "0";
                                            strstaffselector = string.Empty;  //Session["collegecode"].ToString()
                                            minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString() + "'");
                                            splitminimumabsentsms = minimumabsentsms.Split('-');
                                            if (splitminimumabsentsms.Length == 2)
                                            {
                                                int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                                                if (splitminimumabsentsms[0].ToString() == "1")
                                                {
                                                    if (Convert.ToInt32(dsperiod.Tables[0].Rows[pre]["batch_year"].ToString()) >= batchyearsetting)
                                                    {
                                                        Session["StaffSelector"] = "1";
                                                    }
                                                }
                                            }
                                            if (Session["StaffSelector"].ToString() == "1")
                                            {
                                                strstaffselector = " and s.staffcode like '%" + Session["Staff_Code"].ToString() + "%'";
                                            }
                                            //magesh 16.8.18
                                            Boolean doubleday1 = false;
                                            int noofhours = noofhrs;//magesh 16.8.18

                                            for (int row_inc = 0; row_inc <= days; row_inc++) //Date Loop
                                            {

                                                drTT = dtTTDisp.NewRow();
                                                if (hatdegreename.Contains(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString()))
                                                {
                                                    degreename = GetCorrespondingKey(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString(), hatdegreename).ToString();
                                                }
                                                DateTime cur_day = new DateTime();
                                                //magesh 10.8.18
                                                if (doubleday1 == true)
                                                {
                                                    cur_day = dt2.AddDays(-row_inc + 1);
                                                }
                                                else
                                                    cur_day = dt2.AddDays(-row_inc);


                                                //  cur_day = dt2.AddDays(-row_inc);//magesh 10.8.18
                                                string strAlt = "select * from AlternateDetails where batch_year='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "' and semester='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]) + "' and Degree_code='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "' " + altersetion + " and AlternateDate='" + cur_day.ToString("MM/dd/yyyy") + "'";
                                                DataTable dtAlterDet = dirAcc.selectDataTable(strAlt);


                                                if (!hatdc.Contains(cur_day) || (dicAlternateDayOrder.ContainsKey(cur_day) && dicAlternateDayOrder[cur_day] != 0))
                                                {
                                                    tmp_datevalue = Convert.ToString(cur_day);
                                                    degree_var = Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]);
                                                    string SchOrder = string.Empty;
                                                    string day_from = cur_day.ToString("yyyy-MM-dd");
                                                    DateTime schfromdate = cur_day;


                                                    if (dsall.Tables.Count > 1)
                                                    {
                                                        dsall.Tables[1].DefaultView.RowFilter = "batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and FromDate<='" + cur_day.ToString() + "'";
                                                        dvsemster = dsall.Tables[1].DefaultView;
                                                    }
                                                    if (dvsemster.Count > 0)
                                                    {
                                                        getdate = dvsemster[0]["FromDate"].ToString();
                                                    }
                                                    else
                                                    {
                                                        getdate = string.Empty;
                                                    }
                                                    if (Convert.ToString(getdate) != "" && Convert.ToString(getdate).Trim() != "0" && Convert.ToString(getdate).Trim() != null)
                                                    {
                                                        DateTime getsche = Convert.ToDateTime(getdate);
                                                        if (Convert.ToDateTime(schfromdate) == Convert.ToDateTime(getsche) || Convert.ToDateTime(schfromdate) != Convert.ToDateTime(getsche))
                                                        {
                                                            if (ht_sch.Contains(Convert.ToString(degree_var)))
                                                            {
                                                                string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_sch));
                                                                string[] sp_rd_semi = contvar.Split(',');
                                                                if (sp_rd_semi.GetUpperBound(0) >= 1)
                                                                {
                                                                    SchOrder = sp_rd_semi[0].ToString();
                                                                    SchOrder1 = sp_rd_semi[0].ToString();//magesh 14.8.18
                                                                    noofdays = sp_rd_semi[1].ToString();
                                                                }
                                                            }
                                                            Dictionary<string, string> dicautoswitch = new Dictionary<string, string>();
                                                            DataView dvautoswitch = new DataView();
                                                            if (dsall.Tables.Count > 8)
                                                            {
                                                                dsall.Tables[8].DefaultView.RowFilter = " batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and Current_Semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and TTName='" + dvsemster[0]["ttname"].ToString() + "'";
                                                                dvautoswitch = dsall.Tables[8].DefaultView;
                                                            }
                                                            for (int au = 0; au < dvautoswitch.Count; au++)
                                                            {
                                                                string autoswi = dvautoswitch[au]["Day_Value"].ToString() + dvautoswitch[au]["Hour_Value"].ToString();
                                                                if (!dicautoswitch.ContainsKey(autoswi))
                                                                {
                                                                    dicautoswitch.Add(autoswi, dvautoswitch[au]["auto_switch"].ToString() + '-' + dvautoswitch[au]["no_of_batch"].ToString());
                                                                }
                                                            }
                                                            degree_var = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]);
                                                            if (ht_sdate.Contains(Convert.ToString(degree_var)))
                                                            {
                                                                string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_sdate));
                                                                string[] sp_rd_semi = contvar.Split(',');
                                                                if (sp_rd_semi.GetUpperBound(0) >= 1)
                                                                {
                                                                    start_datesem = sp_rd_semi[0].ToString();
                                                                    start_dayorder = sp_rd_semi[1].ToString();
                                                                }
                                                            }
                                                            if (noofdays.ToString().Trim() == "")
                                                            {
                                                                goto lb1;
                                                            }
                                                            Day_Order = string.Empty;


                                                            if (SchOrder == "1")
                                                            {
                                                                strday = cur_day.ToString("ddd"); //Week Dayorder
                                                                Day_Order = "0-" + Convert.ToString(strday);
                                                                drTT["DateDisp"] = cur_day.ToString("d-MM-yyyy");
                                                                drTT["DateVal"] = cur_day.ToString("MM/dd/yyyy");
                                                                drTT["DayOrder"] = strday;
                                                            }
                                                            else
                                                            {
                                                                string[] sps = dt2.ToString().Split('/');
                                                                string curdate = sps[0] + '/' + sps[1] + '/' + sps[2]; 
                                                                Session["SchOrderdouble"] = SchOrder;
                                                                strday = da.findday(cur_day.ToString(), dsperiod.Tables[0].Rows[pre]["degree_code"].ToString(), dsperiod.Tables[0].Rows[pre]["semester"].ToString(), dsperiod.Tables[0].Rows[pre]["batch_year"].ToString(), start_datesem.ToString(), noofdays.ToString(), start_dayorder);
                                                                if (doubleday1 == true)//magesh 18.8.18
                                                                    cur_day = dt2.AddDays(-row_inc);
                                                                if (dicAlternateDayOrder.ContainsKey(cur_day))
                                                                {
                                                                    strday = findDayName(dicAlternateDayOrder[cur_day]);
                                                                    Day_Order = Convert.ToString(dicAlternateDayOrder[cur_day]).Trim();
                                                                    //drTT["DateDisp"] = cur_day.ToString("d-MM-yyyy") + "(" + strday + ")";
                                                                    //drTT["DateVal"] = cur_day.ToString("MM/dd/yyyy");
                                                                    //drTT["DayOrder"] = strday;
                                                                }
                                                                else
                                                                {
                                                                    if (strday.Trim().ToLower() == "mon")
                                                                        Day_Order = "1";
                                                                    else if (strday.Trim().ToLower() == "tue")
                                                                        Day_Order = "2";
                                                                    else if (strday.Trim().ToLower() == "wed")
                                                                        Day_Order = "3";
                                                                    else if (strday.Trim().ToLower() == "thu")
                                                                        Day_Order = "4";
                                                                    else if (strday.Trim().ToLower() == "fri")
                                                                        Day_Order = "5";
                                                                    else if (strday.Trim().ToLower() == "sat")
                                                                        Day_Order = "6";
                                                                    else if (strday.Trim().ToLower() == "sun")
                                                                        Day_Order = "7";
                                                                }

                                                                Day_Order = Day_Order + "-" + Convert.ToString(strday);
                                                                drTT["DateDisp"] = cur_day.ToString("d-MM-yyyy");
                                                                drTT["DateVal"] = cur_day.ToString("MM/dd/yyyy");
                                                                drTT["DayOrder"] = strday;
                                                            }
                                                            if (strday.ToString().Trim() == "")
                                                            {
                                                                goto lb1;
                                                            }
                                                            //==check holiday
                                                            string reasonsun = string.Empty;
                                                            if (countholiday == countholiday1) //added by Mullai
                                                            {
                                                                if (hatholiday.Contains(cur_day.ToString()))
                                                                {
                                                                    reasonsun = GetCorrespondingKey(cur_day.ToString(), hatholiday).ToString();
                                                                    if (noofdays != "7") // Deepali on 16.4.18
                                                                    {
                                                                        if (reasonsun.Trim().ToLower() == "sunday")
                                                                        {
                                                                            DataRow drTTholi = dtTTDisp.NewRow();
                                                                            drTTholi["DateDisp"] = cur_day.ToString("d-MM-yyyy");//dtFrom.DayOfWeek
                                                                            drTTholi["DateVal"] = cur_day.ToString("MM/dd/yyyy");
                                                                            drTTholi["DayOrder"] = strday;
                                                                            for (byte curPeriod = 1; curPeriod <= noofhrs; curPeriod++)
                                                                            {
                                                                                drTTholi["P" + curPeriod + "ValDisp"] = Convert.ToString("Sunday Holiday") + "##MISD";
                                                                            }
                                                                            dtTTDisp.Rows.Add(drTTholi);
                                                                        }
                                                                        else if (reasonsun.Trim().ToLower() == "saturday")  //modified by prabha on Feb 15 2018
                                                                        {
                                                                            DataRow drTTholi = dtTTDisp.NewRow();
                                                                            drTTholi["DateDisp"] = cur_day.ToString("d-MM-yyyy");//dtFrom.DayOfWeek
                                                                            drTTholi["DateVal"] = cur_day.ToString("MM/dd/yyyy");
                                                                            drTTholi["DayOrder"] = strday;
                                                                            for (byte curPeriod = 1; curPeriod <= noofhrs; curPeriod++)
                                                                            {
                                                                                drTTholi["P" + curPeriod + "ValDisp"] = Convert.ToString("Saturday Holiday") + "##MISD";
                                                                            }
                                                                            dtTTDisp.Rows.Add(drTTholi);
                                                                        }
                                                                    }
                                                                }
                                                            }


                                                            if (!hatholiday.Contains(cur_day.ToString()) || reasonsun.Trim().ToLower() != "sunday")
                                                            {
                                                                string str_day = strday;
                                                                string Atmonth = cur_day.Month.ToString();
                                                                string Atyear = cur_day.Year.ToString();
                                                                long strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                                                                sql1 = string.Empty;
                                                                Strsql = string.Empty;

                                                                for (int i_loop = 1; i_loop <= noofhrs; i_loop++)
                                                                {
                                                                    Strsql = Strsql + strday + Convert.ToString(i_loop) + ",";
                                                                    if (sql1 == "")
                                                                    {
                                                                        sql1 = sql1 + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line
                                                                    }
                                                                    else
                                                                    {
                                                                        sql1 = sql1 + " or " + strday + Convert.ToString(i_loop) + " like '%" + stafcode + "%'";//Modified by Manikandan 14/08/2013 from above comment line
                                                                    }
                                                                }
                                                                string day_aten = cur_day.Day.ToString();
                                                                bool check_hour = false;
                                                                string strsectionvar = string.Empty;
                                                                string labsection = string.Empty;
                                                                if (Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "" && Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) != "-1")
                                                                {
                                                                    strsectionvar = " and isnull(sections,'')='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                                                    labsection = " and isnull(sections,'')='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]) + "'";
                                                                }
                                                                sql1 = " and (" + sql1 + ")";
                                                                if (dsall.Tables.Count > 0)
                                                                {
                                                                    dsall.Tables[0].DefaultView.RowFilter = "degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and batch_year=" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and fromdate='" + day_from + "'";
                                                                    dvalternaet = dsall.Tables[0].DefaultView;
                                                                }
                                                                string text_temp = string.Empty;
                                                                int temp = 0;
                                                                text_temp = string.Empty;
                                                                string getcolumnfield = string.Empty;
                                                                string getcolumnfield_alter = string.Empty;
                                                                bool moringleav = false;
                                                                bool evenleave = false;
                                                                if (dsall.Tables.Count > 2)
                                                                {
                                                                    dsall.Tables[2].DefaultView.RowFilter = "holiday_date='" + cur_day.ToString("MM/dd/yyyy") + "' and degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + " and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "'";
                                                                    dvholiday = dsall.Tables[2].DefaultView;
                                                                }
                                                                if (dvholiday.Count > 0)
                                                                {
                                                                    if (!hatholiday.Contains(cur_day.ToString()))
                                                                    {
                                                                        hatholiday.Add(cur_day.ToString(), dvholiday[0]["holiday_desc"].ToString());
                                                                    }
                                                                    if (dvholiday[0]["morning"].ToString() == "1" || dvholiday[0]["morning"].ToString().Trim().ToLower() == "true")
                                                                    {
                                                                        moringleav = true;
                                                                    }
                                                                    if (dvholiday[0]["evening"].ToString() == "1" || dvholiday[0]["evening"].ToString().Trim().ToLower() == "true")
                                                                    {
                                                                        evenleave = true;
                                                                    }
                                                                    if (dvholiday[0]["halforfull"].ToString() == "0" || dvholiday[0]["halforfull"].ToString().Trim().ToLower() == "false")
                                                                    {
                                                                        evenleave = true;
                                                                        moringleav = true;
                                                                    }
                                                                }

                                                                //magesh 6.8.18

                                                                int doublecon = 1;
                                                                if (Session["chkdoubledayno"] != "1")
                                                                {
                                                                    if (doubleday1 == false)
                                                                    {
                                                                        doublecon = 1;
                                                                        noofhrs = noofhours / 2;
                                                                    }
                                                                    else
                                                                    {
                                                                        doublecon = noofhrs + 1;
                                                                        // noofhrs = noofhrs * 2;
                                                                    }
                                                                }

                                                                string Discon = da.GetFunction("select value from Master_Settings where settings='Attendance Discount'  and   " + grouporusercode + "");
                                                                string debar = da.GetFunction("select value from Master_Settings where settings='Attendance Debar'  and   " + grouporusercode + "");
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


                                                                for (temp = 1; temp <= noofhrs; temp++)//Hour loop
                                                                {
                                                                    try
                                                                    {
                                                                        if (dicautoswitch.ContainsKey(strday + temp))
                                                                        {
                                                                            bool altflag = false;
                                                                            if (dvalternaet.Count > 0)
                                                                            {
                                                                                string getva = dvalternaet[0]["" + strday.Trim() + temp + ""].ToString();
                                                                                if (getva.Trim() != "" && getva != null)
                                                                                {
                                                                                    altflag = true;
                                                                                }
                                                                            }
                                                                            if (altflag == false)
                                                                            {
                                                                                string[] autobatch = dicautoswitch[strday + temp].Split('-');
                                                                                if (autobatch.GetUpperBound(0) == 1)
                                                                                {
                                                                                    int batch = Convert.ToInt32(autobatch[1]);
                                                                                    DateTime dts = Convert.ToDateTime(getdate);
                                                                                    DateTime dtnow = cur_day;
                                                                                    TimeSpan ts = dtnow - dts;
                                                                                    int dif_days = ts.Days;
                                                                                    int weekcoun = dif_days / 7;
                                                                                    string[] spsubva = autobatch[0].Split(',');
                                                                                    int counsubj = spsubva.GetUpperBound(0) + 1;
                                                                                    int order = weekcoun % counsubj;
                                                                                    string rsec = string.Empty;
                                                                                    if (altersetion.Trim() != "" && altersetion != null)
                                                                                    {
                                                                                        rsec = dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                    }
                                                                                    string getstaffcode = string.Empty;
                                                                                    string setalte = string.Empty;
                                                                                    DataSet dsstaff = new DataSet();
                                                                                    if (batch >= 1)
                                                                                    {
                                                                                        for (int b = 0; b < batch; b++)
                                                                                        {
                                                                                            int val = order + b;
                                                                                            int su = val % counsubj;
                                                                                            string subno = spsubva[su].ToString();
                                                                                            string batchset = "B" + (b + 1).ToString();
                                                                                            string getstaffquery = "select distinct staff_code from staff_selector where subject_no='" + subno + "' and isnull(sections,'')='" + rsec + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' ";
                                                                                            dsstaff.Reset();
                                                                                            dsstaff = da.select_method_wo_parameter(getstaffquery, "Text");
                                                                                            getstaffcode = subno;
                                                                                            if (dsstaff.Tables.Count > 0 && dsstaff.Tables[0].Rows.Count > 0)
                                                                                            {
                                                                                                for (int sh = 0; sh < dsstaff.Tables[0].Rows.Count; sh++)
                                                                                                {
                                                                                                    getstaffcode = getstaffcode + '-' + dsstaff.Tables[0].Rows[sh]["staff_code"].ToString();
                                                                                                }
                                                                                            }
                                                                                            getstaffcode = getstaffcode + "-L";
                                                                                            if (setalte == "")
                                                                                            {
                                                                                                setalte = getstaffcode;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                setalte = setalte + ";" + getstaffcode;
                                                                                            }
                                                                                            string strquery = "delete from subjectChooser_New where subject_no='" + subno + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' and fromdate='" + cur_day.ToString() + "' and roll_no in( select roll_no from Registration where  batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and cc=0 " + dis + " " + deba + " )";
                                                                                            int insert = da.update_method_wo_parameter(strquery, "Text");
                                                                                            strquery = "delete from laballoc_new where  batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and day_value='" + strday + "' and hour_value='" + temp + "' and fdate='" + cur_day.ToString() + "' and subject_no='" + subno + "'";
                                                                                            insert = da.update_method_wo_parameter(strquery, "Text");
                                                                                            strquery = "insert into subjectChooser_New (semester,roll_no,subject_no,subtype_no,Batch,fromdate,todate) ";
                                                                                            strquery = strquery + "(select s.semester,s.roll_no,s.subject_no,s.subtype_no,s.Batch,'" + cur_day.ToString() + "' as fromdate ,'" + cur_day.ToString() + "' as todate from Registration r ,subjectChooser s where r.Roll_No=s.roll_no and r.Current_Semester=s.semester and r.CC=0 " + dis + " " + deba + " and s.subject_no='" + subno + "' and batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and r.degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and s.semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and s.Batch='" + batchset + "')";
                                                                                            insert = da.update_method_wo_parameter(strquery, "Text");
                                                                                            strquery = "insert into laballoc_new (Batch_Year,Degree_Code,Semester,Sections,Subject_No,Stu_Batch,Day_Value,Hour_Value,fdate,tdate) ";
                                                                                            strquery = strquery + "values('" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "','" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "','" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "','" + rsec + "','" + subno + "','" + batchset + "','" + strday + "','" + temp + "','" + cur_day.ToString() + "','" + cur_day.ToString() + "')";
                                                                                            insert = da.update_method_wo_parameter(strquery, "Text");
                                                                                        }
                                                                                        string strquery1 = "if exists(select * from Alternate_Schedule where batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and FromDate= '" + cur_day.ToString() + "')";
                                                                                        strquery1 = strquery1 + " Update Alternate_Schedule set " + strday + temp + "='" + setalte + "' where batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and FromDate= '" + cur_day.ToString() + "'";
                                                                                        strquery1 = strquery1 + " ELse insert into Alternate_Schedule(batch_year,degree_code,semester,Sections,FromDate," + strday + temp + ") values('" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "','" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "','" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "','" + rsec + "','" + cur_day.ToString() + "','" + setalte + "')";
                                                                                        int insert1 = da.update_method_wo_parameter(strquery1, "Text");
                                                                                    }

                                                                                    getstaffcode = "select * from Alternate_Schedule where batch_year='" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "' and degree_code='" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "' and semester='" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "' " + altersetion + " and FromDate= '" + cur_day.ToString() + "'";
                                                                                    dsstaff.Reset();
                                                                                    dsstaff = da.select_method_wo_parameter(getstaffcode, "Text");
                                                                                    dvalternaet = dsstaff.Tables[0].DefaultView;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    catch
                                                                    {
                                                                    }
                                                                    string sp_rd = string.Empty;
                                                                    bool altfalg = false;
                                                                    bool newAlter = false;
                                                                    string semsch = Convert.ToString(dvsemster[0][strday.Trim() + temp]);
                                                                    //Modified by rajkumar on 8-12-2018
                                                                    DataView dvaltStaff = new DataView();
                                                                    if (dtAlterDet.Rows.Count > 0)
                                                                    {
                                                                        dtAlterDet.DefaultView.RowFilter = "(ActstaffCode='" + stafcode + "' or alterStaffCode='" + stafcode + "') and AlterHour='" + temp + "'";
                                                                        dvaltStaff = dtAlterDet.DefaultView;


                                                                        dtAlterDet.DefaultView.RowFilter = "(ActstaffCode='" + stafcode + "') and AlterHour='" + temp + "'";
                                                                        DataView dvaltStaffNew = dtAlterDet.DefaultView;
                                                                        if (dvaltStaffNew.Count > 0)
                                                                            newAlter = true;
                                                                    }

                                                                    if (dvaltStaff.Count > 0)
                                                                    {
                                                                        if (dvalternaet.Count > 0)
                                                                        {
                                                                            sp_rd = dvalternaet[0]["" + strday.Trim() + temp + ""].ToString();
                                                                            if (hatdegreename.Contains(dvalternaet[0]["degree_code"].ToString()))
                                                                            {
                                                                                degreename = GetCorrespondingKey(dvalternaet[0]["degree_code"].ToString(), hatdegreename).ToString();
                                                                            }
                                                                        }
                                                                    }
                                                                    else if (dvalternaet.Count > 0)
                                                                    {
                                                                        string altStr = Convert.ToString(dvalternaet[0]["" + strday.Trim() + temp + ""]);
                                                                        if (semsch.Contains(stafcode) && !string.IsNullOrEmpty(altStr))
                                                                            newAlter = true;

                                                                        if (Convert.ToString(dvalternaet[0]["" + strday.Trim() + temp + ""]).Trim().ToLower().Contains(stafcode.Trim().ToLower()))
                                                                        {
                                                                            sp_rd = dvalternaet[0]["" + strday.Trim() + temp + ""].ToString();
                                                                            if (hatdegreename.Contains(dvalternaet[0]["degree_code"].ToString()))
                                                                            {
                                                                                degreename = GetCorrespondingKey(dvalternaet[0]["degree_code"].ToString(), hatdegreename).ToString();
                                                                            }
                                                                        }
                                                                        else
                                                                            sp_rd = string.Empty;
                                                                    }
                                                                    else
                                                                    {
                                                                        sp_rd = string.Empty;
                                                                    }
                                                                    if (sp_rd.Trim() != "" && sp_rd.Trim() != "0" && sp_rd != null)
                                                                    {
                                                                        altfalg = true;
                                                                        string[] sp_rd_split = sp_rd.Split(';');
                                                                        for (int index = 0; index <= sp_rd_split.GetUpperBound(0); index++)
                                                                        {
                                                                            string[] sp2 = sp_rd_split[index].Split(new Char[] { '-' });
                                                                            if (sp2.GetUpperBound(0) >= 1)
                                                                            {
                                                                                int upperbound = sp2.GetUpperBound(0);
                                                                                for (int multi_staff = 1; multi_staff < sp2.GetUpperBound(0); multi_staff++)
                                                                                {
                                                                                    if (sp2[multi_staff].Trim().ToLower() == stafcode.Trim().ToLower())
                                                                                    {
                                                                                        //==============================theroy batch=======================================
                                                                                        bool checklabhr = false;
                                                                                        for (int sr = 0; sr <= sp_rd_split.GetUpperBound(0); sr++)
                                                                                        {
                                                                                            string[] getlasub = sp_rd_split[sr].ToString().Split('-');
                                                                                            if (getlasub.GetUpperBound(0) > 1)
                                                                                            {
                                                                                                string srllab = getlasub[0].ToString();
                                                                                                if (hatcurlab.Contains(srllab))
                                                                                                {
                                                                                                    checklabhr = true;
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        //======================================================================
                                                                                        string sect = dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                        if (sect != "-1" && sect != null && sect.Trim() != "")
                                                                                        {
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            sect = string.Empty;
                                                                                        }
                                                                                        if (semenddate.Trim() != "" && semenddate.Trim() != null && semenddate.Trim() != "0")
                                                                                        {
                                                                                            if (cur_day <= (Convert.ToDateTime(semenddate)))
                                                                                            {
                                                                                                check_hour = true;
                                                                                                double Num;
                                                                                                bool isNum = double.TryParse(sp2[0].ToString(), out Num);
                                                                                                if (isNum)
                                                                                                {
                                                                                                    if (checklabhr == false)
                                                                                                    {
                                                                                                        dsall.Tables[4].DefaultView.RowFilter = " subject_no=" + sp2[0] + "";
                                                                                                        dvsubject = dsall.Tables[4].DefaultView;
                                                                                                        if (dvsubject.Count > 0)
                                                                                                        {
                                                                                                            text_temp = dvsubject[0]["subject_name"].ToString() + "-S";
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        dsall.Tables[4].DefaultView.RowFilter = " subject_no=" + sp2[0] + "";
                                                                                                        dvsubject = dsall.Tables[4].DefaultView;
                                                                                                        if (dvsubject.Count > 0)
                                                                                                        {
                                                                                                            text_temp = dvsubject[0]["subject_name"].ToString() + "-L";
                                                                                                        }
                                                                                                    }
                                                                                                    string Schedule_string = string.Empty;
                                                                                                    if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "-1" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == null)
                                                                                                    {
                                                                                                        if (checklabhr == false)
                                                                                                        {
                                                                                                            Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-S-0"; //+ sp_rd_semi.GetUpperBound(0);
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-L-0"; //+ sp_rd_semi.GetUpperBound(0);
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        if (checklabhr == false)
                                                                                                        {
                                                                                                            Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-S-0";// +sp_rd_semi.GetUpperBound(0);
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-L-0";// +sp_rd_semi.GetUpperBound(0);
                                                                                                        }
                                                                                                    }
                                                                                                    bool allowleave = false;
                                                                                                    if (hatholiday.Contains(cur_day.ToString()))
                                                                                                    {
                                                                                                        if (moringleav == true)
                                                                                                        {
                                                                                                            if (frshlf >= temp)
                                                                                                            {
                                                                                                                allowleave = true;
                                                                                                            }
                                                                                                        }
                                                                                                        if (evenleave == true)
                                                                                                        {
                                                                                                            if (temp > frshlf)
                                                                                                            {
                                                                                                                allowleave = true;
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                    if (allowleave == true)
                                                                                                    {
                                                                                                        if (hatholiday.Contains(cur_day.ToString()))
                                                                                                        {
                                                                                                            string holidayreason = GetCorrespondingKey(cur_day.ToString(), hatholiday).ToString();
                                                                                                            if (Convert.ToString(drTT["P" + temp + "ValDisp"]).Trim() == "")
                                                                                                            {

                                                                                                                drTT["P" + temp + "ValDisp"] = Convert.ToString("Selected day is Holiday- Reason-" + holidayreason + "-" + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "");
                                                                                                                drTT["P" + temp + "Val"] = "Selected day is Holiday- Reason-" + holidayreason + "-" + Schedule_string.ToString() + "-alter";
                                                                                                                drTT["TT_" + temp] = Day_Order;

                                                                                                                //FpSpread1.Sheets[0].Cells[(row_inc), temp - 1].ForeColor = Color.Blue;
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                drTT["P" + temp + "ValDisp"] = Convert.ToString(drTT["P" + temp + "ValDisp"]) + " * " + "Selected day is Holiday- Reason-" + holidayreason + "-" + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                drTT["P" + temp + "Val"] = Convert.ToString(drTT["P" + temp + "Val"]) + " * " + "Selected day is Holiday- Reason-" + holidayreason + "-" + Schedule_string.ToString() + "-alter";
                                                                                                                drTT["TT_" + temp] = Day_Order;

                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {

                                                                                                        if (Convert.ToString(drTT["P" + temp + "ValDisp"]).Trim() == "")
                                                                                                        {
                                                                                                            drTT["P" + temp + "ValDisp"] = text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                            drTT["P" + temp + "Val"] = Schedule_string.ToString() + "-alter";

                                                                                                            drTT["TT_" + temp] = Day_Order;
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            string tmpvar = string.Empty;
                                                                                                            string istemp = string.Empty;
                                                                                                            istemp = Convert.ToString(drTT["P" + temp + "ValDisp"]);
                                                                                                            tmpvar = text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                            if (Convert.ToString(istemp) != Convert.ToString(tmpvar))
                                                                                                            {
                                                                                                                drTT["P" + temp + "ValDisp"] = Convert.ToString(drTT["P" + temp + "ValDisp"]) + " * " + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + da.GetFunction("select distinct case course.Course_Name when '-1' then ' ' else course.Course_Name end from degree,course,registration where course.Course_Id=degree.Course_Id and degree.Degree_Code=registration.degree_code and registration.degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "") + "-" + da.GetFunction("select distinct department.dept_acronym from degree,department,registration where department.Dept_Code=degree.Dept_Code and degree.Degree_Code=registration.degree_code and registration.degree_code=" + dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "") + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                drTT["P" + temp + "Val"] = Convert.ToString(drTT["P" + temp + "Val"]) + " * " + Schedule_string.ToString() + "-alter";
                                                                                                                drTT["TT_" + temp] = Day_Order;
                                                                                                            }
                                                                                                        }
                                                                                                        dailyentryflag = false;
                                                                                                        attendanceentryflag = false;
                                                                                                        //FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Font.Bold = true;
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }

                                                                    if (altfalg == false && !newAlter)
                                                                    {
                                                                        getcolumnfield = Convert.ToString(strday + temp);
                                                                        attendanceentryflag = false;
                                                                        dailyentryflag = false;
                                                                        // if (dsperiod.Tables[0].Rows[pre][getcolumnfield].ToString() != "" && dsperiod.Tables[0].Rows[pre][getcolumnfield].ToString() != null && dsperiod.Tables[0].Rows[pre][getcolumnfield].ToString() != "\0")
                                                                        if (dvsemster.Count > 0)
                                                                        {
                                                                            if (dvsemster[0][getcolumnfield].ToString() != "" && dvsemster[0][getcolumnfield].ToString() != null && dvsemster[0][getcolumnfield].ToString() != "\0")
                                                                            {
                                                                                string timetable = string.Empty;
                                                                                string name = dvsemster[0]["ttname"].ToString();
                                                                                if (name != null && name.Trim() != "")
                                                                                {
                                                                                    timetable = name;
                                                                                }
                                                                                sp_rd = dvsemster[0][getcolumnfield].ToString();
                                                                                string[] sp_rd_semi = sp_rd.Split(';');
                                                                                for (int semi = 0; semi <= sp_rd_semi.GetUpperBound(0); semi++)
                                                                                {
                                                                                    string[] sp2 = sp_rd_semi[semi].Split(new Char[] { '-' });
                                                                                    if (sp2.GetUpperBound(0) >= 1)
                                                                                    {
                                                                                        int upperbound = sp2.GetUpperBound(0);
                                                                                        for (int multi_staff = 1; multi_staff < sp2.GetUpperBound(0); multi_staff++)
                                                                                        {
                                                                                            if (sp2[multi_staff].Trim().ToLower() == stafcode.Trim().ToLower())
                                                                                            {
                                                                                                //==============================theroy batch=======================================
                                                                                                bool checklabhr = false;
                                                                                                for (int sr = 0; sr <= sp_rd_semi.GetUpperBound(0); sr++)
                                                                                                {
                                                                                                    string[] getlasub = sp_rd_semi[sr].ToString().Split('-');
                                                                                                    for (int sp = 1; sp <= getlasub.Count(); sp++) //added by Mullai 
                                                                                                    {
                                                                                                        if (stafcode == getlasub[sp - 1])
                                                                                                        {
                                                                                                            if (getlasub.GetUpperBound(0) > 1)
                                                                                                            {
                                                                                                                string srllab = getlasub[0].ToString();
                                                                                                                if (hatcurlab.Contains(srllab))
                                                                                                                {
                                                                                                                    checklabhr = true;
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                //======================================================================
                                                                                                string sect = dsperiod.Tables[0].Rows[pre]["sections"].ToString();
                                                                                                if (sect == "-1" || sect == null || sect.Trim() == "")
                                                                                                {
                                                                                                    sect = string.Empty;
                                                                                                }
                                                                                                if (semenddate.Trim() != "" && semenddate.Trim() != null && semenddate.Trim() != "0")
                                                                                                {
                                                                                                    if (cur_day <= (Convert.ToDateTime(semenddate)))
                                                                                                    {
                                                                                                        check_hour = true;
                                                                                                        double Num;
                                                                                                        bool isNum = double.TryParse(sp2[0].ToString(), out Num);
                                                                                                        if (isNum)
                                                                                                        {
                                                                                                            // text_temp = da.GetFunction("select subject_name from subject where subject_no=" + sp2[0] + "") + "-" + sp2[upperbound];
                                                                                                            if (checklabhr == false)
                                                                                                            {
                                                                                                                dsall.Tables[4].DefaultView.RowFilter = " subject_no=" + sp2[0] + "";
                                                                                                                dvsubject = dsall.Tables[4].DefaultView;
                                                                                                                if (dvsubject.Count > 0)
                                                                                                                {
                                                                                                                    text_temp = dvsubject[0]["subject_name"].ToString() + "-S";
                                                                                                                }
                                                                                                                //text_temp = da.GetFunction("select subject_name from subject where subject_no=" + sp2[0] + "") + "-S";
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                dsall.Tables[4].DefaultView.RowFilter = " subject_no=" + sp2[0] + "";
                                                                                                                dvsubject = dsall.Tables[4].DefaultView;
                                                                                                                if (dvsubject.Count > 0)
                                                                                                                {
                                                                                                                    text_temp = dvsubject[0]["subject_name"].ToString() + "-S";
                                                                                                                }
                                                                                                                //text_temp = da.GetFunction("select subject_name from subject where subject_no=" + sp2[0] + "") + "-L";
                                                                                                            }
                                                                                                            string Schedule_string = string.Empty;
                                                                                                            if (dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "-1" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == "" || dsperiod.Tables[0].Rows[pre]["sections"].ToString() == null)
                                                                                                            {
                                                                                                                if (checklabhr == false)
                                                                                                                {
                                                                                                                    Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-S-" + sp_rd_semi.GetUpperBound(0);
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-L-" + sp_rd_semi.GetUpperBound(0);
                                                                                                                }
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                if (checklabhr == false)
                                                                                                                {
                                                                                                                    Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-S-" + sp_rd_semi.GetUpperBound(0);
                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    Schedule_string = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + "-" + sp2[0].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["sections"].ToString() + "-" + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-L-" + sp_rd_semi.GetUpperBound(0);
                                                                                                                }
                                                                                                            }
                                                                                                            bool allowleave = false;
                                                                                                            if (hatholiday.Contains(cur_day.ToString()))
                                                                                                            {
                                                                                                                if (moringleav == true)
                                                                                                                {
                                                                                                                    if (frshlf >= temp)
                                                                                                                    {
                                                                                                                        allowleave = true;
                                                                                                                    }
                                                                                                                }
                                                                                                                if (evenleave == true)
                                                                                                                {
                                                                                                                    if (temp > frshlf)
                                                                                                                    {
                                                                                                                        allowleave = true;
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                            if (allowleave == true)
                                                                                                            {
                                                                                                                if (hatholiday.Contains(cur_day.ToString()))
                                                                                                                {
                                                                                                                    string holidayreason = GetCorrespondingKey(cur_day.ToString(), hatholiday).ToString();
                                                                                                                    if (Convert.ToString(drTT["P" + doublecon + "ValDisp"]).Trim() == "")
                                                                                                                    {

                                                                                                                        drTT["P" + doublecon + "ValDisp"] = "Selected day is Holiday- Reason-" + holidayreason + "-" + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                        drTT["P" + doublecon + "Val"] = "Selected day is Holiday- Reason-" + holidayreason + "-" + Schedule_string.ToString() + "-sem";

                                                                                                                        drTT["TT_" + doublecon] = Day_Order;

                                                                                                                    }
                                                                                                                    else
                                                                                                                    {

                                                                                                                        drTT["P" + doublecon + "ValDisp"] = Convert.ToString(drTT["P" + doublecon + "ValDisp"]) + '*' + "Selected day is Holiday- Reason-" + holidayreason + "-" + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                        drTT["P" + doublecon + "Val"] = Convert.ToString(drTT["P" + doublecon + "Val"]) + '*' + "Selected day is Holiday- Reason-" + holidayreason + "-" + Schedule_string.ToString() + "-sem";

                                                                                                                        drTT["TT_" + doublecon] = Day_Order;

                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                if (Convert.ToString(drTT["P" + doublecon + "ValDisp"]).Trim() == "")
                                                                                                                {
                                                                                                                    drTT["P" + doublecon + "ValDisp"] = text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                    drTT["P" + doublecon + "Val"] = Schedule_string.ToString() + "-sem";
                                                                                                                    drTT["TT_" + doublecon] = Day_Order;

                                                                                                                }
                                                                                                                else
                                                                                                                {
                                                                                                                    if (Convert.ToString(drTT["P" + doublecon + "ValDisp"]).Trim() != "")
                                                                                                                    {
                                                                                                                        string tmpvar = string.Empty;
                                                                                                                        string istemp = string.Empty;
                                                                                                                        istemp = Convert.ToString(drTT["P" + doublecon + "ValDisp"]);
                                                                                                                        tmpvar = text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                        if (Convert.ToString(istemp) != Convert.ToString(tmpvar))
                                                                                                                        {
                                                                                                                            drTT["P" + doublecon + "ValDisp"] = Convert.ToString(drTT["P" + doublecon + "ValDisp"]) + " * " + text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                            drTT["P" + doublecon + "Val"] = Convert.ToString(drTT["P" + doublecon + "Val"]) + " * " + Schedule_string.ToString() + "-sem";

                                                                                                                            drTT["TT_" + doublecon] = Day_Order;
                                                                                                                        }
                                                                                                                    }
                                                                                                                    else
                                                                                                                    {
                                                                                                                        drTT["P" + doublecon + "ValDisp"] = text_temp + "  " + dsperiod.Tables[0].Rows[pre]["batch_year"].ToString() + "-" + degreename + "-Sem" + dsperiod.Tables[0].Rows[pre]["semester"].ToString() + " " + sect + "";
                                                                                                                        drTT["P" + doublecon + "Val"] = Schedule_string.ToString() + "-sem";
                                                                                                                        drTT["TT_" + doublecon] = Day_Order;
                                                                                                                    }
                                                                                                                }
                                                                                                                //----------------set color
                                                                                                                dailyentryflag = false;
                                                                                                                attendanceentryflag = false;
                                                                                                                //FpSpread1.Sheets[0].Cells[row_inc, temp - 1].Font.Bold = true;
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
                                                                    doublecon++;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                dtTTDisp.Rows.Add(drTT);
                                                if (SchOrder1 == "0")
                                                {
                                                    string chkdoubleday = da.GetFunction("select * from doubledayorder where doubleDate='" + cur_day + "' and batchYear='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"].ToString()) + "' and degreecode='" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"].ToString()) + "'");
                                                    if (chkdoubleday != "" && chkdoubleday != "0")
                                                    {
                                                        if (doubleday1 == false)
                                                        {
                                                            doubleday1 = true;
                                                            row_inc--;
                                                            Session["doubledayshk"] = "true";
                                                        }
                                                        else
                                                        {
                                                            doubleday1 = false;
                                                            Session["doubledayshk"] = "false";

                                                        }
                                                    }
                                                    else
                                                    {
                                                        doubleday1 = false;
                                                        Session["doubledayshk"] = "false";
                                                    }
                                                }
                                            }
                                            //Added By Srinath Day Order Change 4Sep2014
                                        }
                                        else
                                        {
                                            alt++;
                                          
                                            string degc = dsperiod.Tables[0].Rows[pre]["degree_code"].ToString();
                                            string bat = dsperiod.Tables[0].Rows[pre]["batch_year"].ToString();
                                            string degn = da.GetFunction("select acronym from degree where degree_code='" + degc + "' ");
                                            string nam=bat+"-"+degn;
                                            dicalter.Add(alt, nam);

                                            //divPopAlertContent.Visible = true;
                                            //divPopAlert.Visible = true;
                                            //lblAlertMsg.Visible = true;
                                            //lblAlertMsg.Text = "Students Are In Different Semester";
                                        }
                                    }
                                lb1: tmp_camprevar = Convert.ToString(dsperiod.Tables[0].Rows[pre]["batch_year"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["degree_code"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["semester"]) + "-" + Convert.ToString(dsperiod.Tables[0].Rows[pre]["sections"]);
                                }
                            }

                            if (dtTTDisp.Rows.Count > 0)
                            {
                                DataTable dtNewTimeTable = new DataTable();
                                dtNewTimeTable.Columns.Add("DateDisp");
                                dtNewTimeTable.Columns.Add("DateVal");
                                dtNewTimeTable.Columns.Add("DayOrder");
                                dtNewTimeTable.Columns.Add("P1ValDisp");
                                dtNewTimeTable.Columns.Add("P1Val");
                                dtNewTimeTable.Columns.Add("TT_1");
                                dtNewTimeTable.Columns.Add("P2ValDisp");
                                dtNewTimeTable.Columns.Add("P2Val");
                                dtNewTimeTable.Columns.Add("TT_2");
                                dtNewTimeTable.Columns.Add("P3ValDisp");
                                dtNewTimeTable.Columns.Add("P3Val");
                                dtNewTimeTable.Columns.Add("TT_3");
                                dtNewTimeTable.Columns.Add("P4ValDisp");
                                dtNewTimeTable.Columns.Add("P4Val");
                                dtNewTimeTable.Columns.Add("TT_4");
                                dtNewTimeTable.Columns.Add("P5ValDisp");
                                dtNewTimeTable.Columns.Add("P5Val");
                                dtNewTimeTable.Columns.Add("TT_5");
                                dtNewTimeTable.Columns.Add("P6ValDisp");
                                dtNewTimeTable.Columns.Add("P6Val");
                                dtNewTimeTable.Columns.Add("TT_6");
                                dtNewTimeTable.Columns.Add("P7ValDisp");
                                dtNewTimeTable.Columns.Add("P7Val");
                                dtNewTimeTable.Columns.Add("TT_7");
                                dtNewTimeTable.Columns.Add("P8ValDisp");
                                dtNewTimeTable.Columns.Add("P8Val");
                                dtNewTimeTable.Columns.Add("TT_8");
                                dtNewTimeTable.Columns.Add("P9ValDisp");
                                dtNewTimeTable.Columns.Add("P9Val");
                                dtNewTimeTable.Columns.Add("TT_9");
                                dtNewTimeTable.Columns.Add("P10ValDisp");
                                dtNewTimeTable.Columns.Add("P10Val");
                                dtNewTimeTable.Columns.Add("TT_10");
                                DataRow dtRow = null;
                                DataTable dicdate = dtTTDisp.DefaultView.ToTable(true, "DateVal");
                                foreach (DataRow dc in dicdate.Rows)
                                {
                                    string dt = Convert.ToString(dc["DateVal"]);
                                    dtTTDisp.DefaultView.RowFilter = "DateVal='" + dt + "'";
                                    DataTable dtFinal = dtTTDisp.DefaultView.ToTable();
                                    if (dtFinal.Rows.Count > 0)
                                    {
                                        dtRow = dtNewTimeTable.NewRow();
                                        foreach (DataRow dr in dtFinal.Rows)
                                        {
                                            dtRow["DateDisp"] = Convert.ToString(dr["DateDisp"]);
                                            dtRow["DateVal"] = Convert.ToString(dr["DateVal"]);
                                            dtRow["DayOrder"] = Convert.ToString(dr["DayOrder"]);

                                            if (!string.IsNullOrEmpty(Convert.ToString(dtRow["P1ValDisp"])) && !string.IsNullOrEmpty(Convert.ToString(dr["P1ValDisp"])))
                                            {
                                                dtRow["P1ValDisp"] = Convert.ToString(dtRow["P1ValDisp"]) + "*<br/>" + Convert.ToString(dr["P1ValDisp"]);
                                                dtRow["P1Val"] = Convert.ToString(dtRow["P1Val"]) + "*" + Convert.ToString(dr["P1Val"]);
                                                //dtRow["TT_1"] = Convert.ToString(dtRow["TT_1"]) + "*" + Convert.ToString(dr["TT_1"]);
                                                dtRow["TT_1"] = Convert.ToString(dr["TT_1"]);
                                            }
                                            else if (!string.IsNullOrEmpty(Convert.ToString(dr["P1ValDisp"])))
                                            {
                                                dtRow["P1ValDisp"] = Convert.ToString(dr["P1ValDisp"]);
                                                dtRow["P1Val"] = Convert.ToString(dr["P1Val"]);
                                                dtRow["TT_1"] = Convert.ToString(dr["TT_1"]);
                                            }
                                            if (!string.IsNullOrEmpty(Convert.ToString(dtRow["P2ValDisp"])) && !string.IsNullOrEmpty(Convert.ToString(dr["P2ValDisp"])))
                                            {
                                                dtRow["P2ValDisp"] = Convert.ToString(dtRow["P2ValDisp"]) + "*<br/>" + Convert.ToString(dr["P2ValDisp"]);
                                                dtRow["P2Val"] = Convert.ToString(dtRow["P2Val"]) + "*" + Convert.ToString(dr["P2Val"]);
                                                //dtRow["TT_2"] = Convert.ToString(dtRow["TT_2"]) + "*" + Convert.ToString(dr["TT_2"]);
                                                dtRow["TT_2"] = Convert.ToString(dr["TT_2"]);
                                                //dtRow["TT_2"] = Convert.ToString(dr["TT_2"]);
                                            }
                                            else if (!string.IsNullOrEmpty(Convert.ToString(dr["P2ValDisp"])))
                                            {
                                                dtRow["P2ValDisp"] = Convert.ToString(dr["P2ValDisp"]);
                                                dtRow["P2Val"] = Convert.ToString(dr["P2Val"]);
                                                dtRow["TT_2"] = Convert.ToString(dr["TT_2"]);
                                            }
                                            if (!string.IsNullOrEmpty(Convert.ToString(dtRow["P3ValDisp"])) && !string.IsNullOrEmpty(Convert.ToString(dr["P3ValDisp"])))
                                            {
                                                dtRow["P3ValDisp"] = Convert.ToString(dtRow["P3ValDisp"]) + "*<br/>" + Convert.ToString(dr["P3ValDisp"]);
                                                dtRow["P3Val"] = Convert.ToString(dtRow["P3Val"]) + "*" + Convert.ToString(dr["P3Val"]);
                                                //dtRow["TT_3"] = Convert.ToString(dtRow["TT_3"]) + "*" + Convert.ToString(dr["TT_3"]);
                                                dtRow["TT_3"] = Convert.ToString(dr["TT_3"]);
                                            }
                                            else if (!string.IsNullOrEmpty(Convert.ToString(dr["P3ValDisp"])))
                                            {
                                                dtRow["P3ValDisp"] = Convert.ToString(dr["P3ValDisp"]);
                                                dtRow["P3Val"] = Convert.ToString(dr["P3Val"]);
                                                dtRow["TT_3"] = Convert.ToString(dr["TT_3"]);
                                            }
                                            if (!string.IsNullOrEmpty(Convert.ToString(dtRow["P4ValDisp"])) && !string.IsNullOrEmpty(Convert.ToString(dr["P4ValDisp"])))
                                            {
                                                dtRow["P4ValDisp"] = Convert.ToString(dtRow["P4ValDisp"]) + "*<br/>" + Convert.ToString(dr["P4ValDisp"]);
                                                dtRow["P4Val"] = Convert.ToString(dtRow["P4Val"]) + "*" + Convert.ToString(dr["P4Val"]);
                                                //dtRow["TT_4"] = Convert.ToString(dtRow["TT_4"]) + "*" + Convert.ToString(dr["TT_4"]);
                                                dtRow["TT_4"] = Convert.ToString(dr["TT_4"]);
                                            }
                                            else if (!string.IsNullOrEmpty(Convert.ToString(dr["P4ValDisp"])))
                                            {
                                                dtRow["P4ValDisp"] = Convert.ToString(dr["P4ValDisp"]);
                                                dtRow["P4Val"] = Convert.ToString(dr["P4Val"]);
                                                dtRow["TT_4"] = Convert.ToString(dr["TT_4"]);
                                            }
                                            if (!string.IsNullOrEmpty(Convert.ToString(dtRow["P5ValDisp"])) && !string.IsNullOrEmpty(Convert.ToString(dr["P5ValDisp"])))
                                            {
                                                dtRow["P5ValDisp"] = Convert.ToString(dtRow["P5ValDisp"]) + "*<br/>" + Convert.ToString(dr["P5ValDisp"]);
                                                dtRow["P5Val"] = Convert.ToString(dtRow["P5Val"]) + "*" + Convert.ToString(dr["P5Val"]);
                                                //dtRow["TT_5"] = Convert.ToString(dtRow["TT_5"]) + "*" + Convert.ToString(dr["TT_5"]);
                                                dtRow["TT_5"] = Convert.ToString(dr["TT_5"]);
                                            }
                                            else if (!string.IsNullOrEmpty(Convert.ToString(dr["P5ValDisp"])))
                                            {
                                                dtRow["P5ValDisp"] = Convert.ToString(dr["P5ValDisp"]);
                                                dtRow["P5Val"] = Convert.ToString(dr["P5Val"]);
                                                dtRow["TT_5"] = Convert.ToString(dr["TT_5"]);
                                            }
                                            if (!string.IsNullOrEmpty(Convert.ToString(dtRow["P6ValDisp"])) && !string.IsNullOrEmpty(Convert.ToString(dr["P6ValDisp"])))
                                            {

                                                dtRow["P6ValDisp"] = Convert.ToString(dtRow["P6ValDisp"]) + "*<br/>" + Convert.ToString(dr["P6ValDisp"]);
                                                dtRow["P6Val"] = Convert.ToString(dtRow["P6Val"]) + "*" + Convert.ToString(dr["P6Val"]);
                                                // dtRow["TT_6"] = Convert.ToString(dtRow["TT_6"]) + "*" + Convert.ToString(dr["TT_6"]);
                                                dtRow["TT_6"] = Convert.ToString(dr["TT_6"]);
                                            }
                                            else if (!string.IsNullOrEmpty(Convert.ToString(dr["P6ValDisp"])))
                                            {
                                                dtRow["P6ValDisp"] = Convert.ToString(dr["P6ValDisp"]);
                                                dtRow["P6Val"] = Convert.ToString(dr["P6Val"]);
                                                dtRow["TT_6"] = Convert.ToString(dr["TT_6"]);
                                            }
                                            if (!string.IsNullOrEmpty(Convert.ToString(dtRow["P7ValDisp"])) && !string.IsNullOrEmpty(Convert.ToString(dr["P7ValDisp"])))
                                            {
                                                dtRow["P7ValDisp"] = Convert.ToString(dtRow["P7ValDisp"]) + "*<br/>" + Convert.ToString(dr["P7ValDisp"]);
                                                dtRow["P7Val"] = Convert.ToString(dtRow["P7Val"]) + "*" + Convert.ToString(dr["P7Val"]);
                                                //dtRow["TT_7"] = Convert.ToString(dtRow["TT_7"]) + "*" + Convert.ToString(dr["TT_7"]);
                                                dtRow["TT_7"] = Convert.ToString(dr["TT_7"]);
                                            }
                                            else if (!string.IsNullOrEmpty(Convert.ToString(dr["P7ValDisp"])))
                                            {
                                                dtRow["P7ValDisp"] = Convert.ToString(dr["P7ValDisp"]);
                                                dtRow["P7Val"] = Convert.ToString(dr["P7Val"]);
                                                dtRow["TT_7"] = Convert.ToString(dr["TT_7"]);
                                            }
                                            if (!string.IsNullOrEmpty(Convert.ToString(dtRow["P8ValDisp"])) && !string.IsNullOrEmpty(Convert.ToString(dr["P8ValDisp"])))
                                            {
                                                dtRow["P8ValDisp"] = Convert.ToString(dtRow["P8ValDisp"]) + "*<br/>" + Convert.ToString(dr["P8ValDisp"]);
                                                dtRow["P8Val"] = Convert.ToString(dtRow["P8Val"]) + "*" + Convert.ToString(dr["P8Val"]);
                                                //dtRow["TT_8"] = Convert.ToString(dtRow["TT_8"]) + "*" + Convert.ToString(dr["TT_7"]);
                                                dtRow["TT_8"] = Convert.ToString(dr["TT_8"]);
                                            }
                                            else if (!string.IsNullOrEmpty(Convert.ToString(dr["P8ValDisp"])))
                                            {
                                                dtRow["P8ValDisp"] = Convert.ToString(dr["P8ValDisp"]);
                                                dtRow["P8Val"] = Convert.ToString(dr["P8Val"]);
                                                dtRow["TT_8"] = Convert.ToString(dr["TT_8"]);
                                            }
                                            if (!string.IsNullOrEmpty(Convert.ToString(dtRow["P9ValDisp"])) && !string.IsNullOrEmpty(Convert.ToString(dr["P9ValDisp"])))
                                            {

                                                dtRow["P9ValDisp"] = Convert.ToString(dtRow["P9ValDisp"]) + "* <br/>" + Convert.ToString(dr["P9ValDisp"]);
                                                dtRow["P9Val"] = Convert.ToString(dtRow["P9Val"]) + "*" + Convert.ToString(dr["P9Val"]);
                                                //dtRow["TT_9"] = Convert.ToString(dtRow["TT_9"]) + "*" + Convert.ToString(dr["TT_9"]);
                                                dtRow["TT_9"] = Convert.ToString(dr["TT_9"]);

                                            }
                                            else if (!string.IsNullOrEmpty(Convert.ToString(dr["P9ValDisp"])))
                                            {

                                                dtRow["P9ValDisp"] = Convert.ToString(dr["P9ValDisp"]);
                                                dtRow["P9Val"] = Convert.ToString(dr["P9Val"]);
                                                dtRow["TT_9"] = Convert.ToString(dr["TT_9"]);
                                            }
                                            if (!string.IsNullOrEmpty(Convert.ToString(dtRow["P10ValDisp"])) && !string.IsNullOrEmpty(Convert.ToString(dr["P10ValDisp"])))
                                            {

                                                dtRow["P10ValDisp"] = Convert.ToString(dtRow["P10ValDisp"]) + "* <br/>" + Convert.ToString(dr["P10ValDisp"]);
                                                dtRow["P10Val"] = Convert.ToString(dtRow["P10Val"]) + "*" + Convert.ToString(dr["P10Val"]);
                                                //dtRow["TT_10"] = Convert.ToString(dtRow["TT_10"]) + "*" + Convert.ToString(dr["TT_10"]);
                                                dtRow["TT_10"] = Convert.ToString(dr["TT_10"]);
                                            }
                                            else if (!string.IsNullOrEmpty(Convert.ToString(dr["P10ValDisp"])))
                                            {
                                                dtRow["P10ValDisp"] = Convert.ToString(dr["P10ValDisp"]);
                                                dtRow["P10Val"] = Convert.ToString(dr["P10Val"]);
                                                dtRow["TT_10"] = Convert.ToString(dr["TT_10"]);
                                            }

                                        }
                                        dtNewTimeTable.Rows.Add(dtRow);
                                    }
                                }

                                if (dtNewTimeTable.Rows.Count > 0)
                                {
                                    gridTimeTable.Columns[0].Visible = true;
                                    gridTimeTable.Columns[1].Visible = false;
                                    gridTimeTable.Columns[2].Visible = false;
                                    gridTimeTable.Columns[3].Visible = false;
                                    gridTimeTable.Columns[4].Visible = false;
                                    gridTimeTable.Columns[5].Visible = false;
                                    gridTimeTable.Columns[6].Visible = false;
                                    gridTimeTable.Columns[7].Visible = false;
                                    gridTimeTable.Columns[8].Visible = false;
                                    gridTimeTable.Columns[9].Visible = false;
                                    gridTimeTable.Columns[10].Visible = false;
                                    divTimeTable.Visible = true;


                                    gridTimeTable.DataSource = dtNewTimeTable;
                                    gridTimeTable.DataBind();

                                    gridTimeTable.Visible = true;
                                    divTimeTable.Visible = true;
                                    if (isDoubleDay)
                                        noofhrs = noofhrs * 2;
                                    for (int i = 0; i <= noofhrs; i++)
                                    {
                                        gridTimeTable.Columns[i].Visible = true;
                                    }
                                    seColor();
                                }
                            }

                            //else
                            //{
                            //    gridTimeTable.Visible = false;
                            //}

                        }

                    }
                }
            }
            if (dicalter.Count > 0)
            {
                string val2 = string.Empty;
                foreach (KeyValuePair<int, string> val in dicalter)
                {
                    if(string.IsNullOrEmpty(val2))
                    {
                         val2 = val.Value;
                    }
                    else
                    {
                         val2 = ","+ val.Value;
                    }
                   
                }
                divPopAlertContent.Visible = true;
                divPopAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Students Are In Different Semester for "+val2+"";
            }
        }
        catch (Exception ex)
        {
            //da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    private DataTable getAbReasons()
    {
        DataTable dtReas = new DataTable();
        try
        {
            string query = "select ltrim(rtrim(isnull(TextCode,' '))) as TextCode,ltrim(rtrim(isnull(Textval,' '))) as Textval from textvaltable where TextCriteria='Attrs' and college_code=" + Session["collegecode"].ToString() + "";
            dtReas = dirAcc.selectDataTable(query);
        }
        catch { dtReas.Clear(); }
        return dtReas;
    }
    protected void ddlSelect_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlLabTest = (DropDownList)sender;
            var row = ddlLabTest.NamingContainer;
            //DataGridItem row = (DataGridItem)ddlLabTest.NamingContainer;
            DropDownList ddlAddLabTestShortName = (DropDownList)row.FindControl("ddlSelect");
            DropDownList ddlPer = (DropDownList)row.FindControl("ddlLeavetype");
            string appddltext = ddlAddLabTestShortName.SelectedItem.Text;
            ddlPer.ClearSelection();
            ddlPer.Items.FindByText(appddltext).Selected = true;
            GridView1.Rows[0].Visible = false;
            int gridCount = GridView1.Rows.Count;
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }

    }
    protected void ddlSelectAll_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList chckheader = (DropDownList)GridView1.HeaderRow.FindControl("ddlSelectAll");
            string selAll = Convert.ToString(chckheader.SelectedItem.Text);
            GridView1.Rows[0].Visible = false;
            if (!string.IsNullOrEmpty(selAll))
            {
                foreach (GridViewRow dr in GridView1.Rows)
                {
                    DropDownList selsubject = (DropDownList)dr.FindControl("ddlSelect");
                    DropDownList leavetype = (DropDownList)dr.FindControl("ddlLeavetype");
                    selsubject.ClearSelection();
                    selsubject.Items.FindByText(selAll).Selected = true;
                    leavetype.ClearSelection();
                    leavetype.Items.FindByText(selAll).Selected = true;
                }
            }
            GridView1.Rows[0].Visible = false;
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    int subTotalRowIndex = 0;
    protected void Buttongo_Click(object sender, EventArgs e)
    {
        try
        {
            Tablenote.Visible = false;
            Tablegview.Visible = false;
            //tablediv.Visible = false;
            loadstafspread();

        }
        catch { }
    }
    protected void lnkAttMark(object sender, EventArgs e)
    {
        LinkButton lnkSelected = (LinkButton)sender;
        string rowIndxS = lnkSelected.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
        int rowIndx = Convert.ToInt32(rowIndxS) - 2;
        string colIndxS = lnkSelected.UniqueID.ToString().Split('$')[4].Replace("lnkPeriod_", string.Empty);
        int colIndx = Convert.ToInt32(colIndxS);
        Session["Row"] = rowIndx;
        Session["Col"] = colIndx;
        loadStudentGrid(colIndx, rowIndx);
        //seColor();

    }
    public void loadStudentGrid(int colIndex, int rowIndex)
    {
        try
        {
            // old_spread2_select();
            //pBodyhomework.Visible = true;
            bool checkleave = false;
            Panelyet.Width = 460;
            Slipentry.Visible = false;
            Panelcomplete.Width = 460;
            GridView1.Visible = false;
            if (Session["flag"].ToString().Trim() == "1")
            {
                chkalterlession.Visible = true;
            }
            else
            {
                chkalterlession.Visible = false;
            }
            chkalterlession.Checked = false;
            Plessionalter.Visible = false;
            ddlclassnotes.Items.Clear();//Added by srinath 22/3/2015
            Labelstaf.Visible = false;
            lbl_alert.Visible = false;

            headerpanelhomework.Visible = true;
            //btnaddhme.Visible = true;
            Tablenote.Visible = true;
            //Tablegview.Visible = true;

            //Buttonupdate.Enabled = false;
            Buttonsavelesson.Enabled = false;
            txtquestion1.Text = string.Empty;//Added by Srinath 21/8/2013
            btnupdatequetion.Enabled = false;
            btndeleteatndqtn.Enabled = false;
            btnaddquestion.Enabled = true;//Added By Srinath 21/8/2013

            singlesubject = false;
            lbldayorder.Visible = false;
            ddlselectmanysub.Visible = false;
            lblmanysubject.Visible = false;
            string sub_name = string.Empty;
            selectedpath = string.Empty;
            Buttonsave.Enabled = true;

            clearfield();
            dicFeeOfRollStudents = new Dictionary<string, DateTime[]>();
            dicFeeOnRollStudents = new Dictionary<string, byte>();

            if (staff_code.ToString() != "")
            {
                //LabelE.Visible = false;
                btnaddquestion.Enabled = true;
                btnqtnupdate.Enabled = false;
                int c = gridTimeTable.Rows.Count;
                int r = gridTimeTable.Columns.Count;

                ar = Convert.ToInt32(rowIndex);
                ac = Convert.ToInt32(colIndex);
                //Added by Mullai 
                string lnktxt = "lnkPeriod_" + ac;
                string text_val11 = Convert.ToString((gridTimeTable.Rows[ar].FindControl(lnktxt.Trim()) as LinkButton).Text);
                //string getdayorder = Convert.ToString(FpSpread1.Sheets[0].Cells[ar, ac].Note);
                string getdayorder = Convert.ToString((gridTimeTable.Rows[ar].FindControl("lblTT_" + ac) as Label).Text);
                if (Convert.ToString(getdayorder).Trim() != "")
                {
                    string[] dayorderval = getdayorder.Split(new Char[] { '-' });
                    if (Convert.ToString(dayorderval[0]).Trim() != "0")
                    {
                        lbldayorder.Visible = true;
                        lbldayorder.Text = "Day Order " + Convert.ToString(dayorderval[0]).Trim();
                    }
                    if (dayorderval.Length > 2)
                    {
                        Day_Var = Convert.ToString(dayorderval[2]).Trim();
                    }
                    else
                    {
                        Day_Var = Convert.ToString(dayorderval[1]).Trim();
                    }
                }
                if (gridTimeTable.Rows[ar].Cells[ac].Enabled == true)
                {
                    if (ar != -1)
                    {
                        ArrayList arrBatchYear = new ArrayList();
                        ArrayList arrDegreeCode = new ArrayList();
                        ArrayList arrSemester = new ArrayList();
                        DataSet dsBellTime = new DataSet();
                        //Modified by srinath 29/8/2013 ==Start
                        string spread_text = Convert.ToString((gridTimeTable.Rows[ar].FindControl(lnktxt.Trim()) as LinkButton).Text);
                        string text_val = Convert.ToString((gridTimeTable.Rows[ar].FindControl(lnktxt.Trim()) as LinkButton).Text);

                        if (spread_text != "" && spread_text != "Sunday Holiday" && spread_text != "Saturday Holiday")
                        {
                            getcelltag = Convert.ToString((gridTimeTable.Rows[ar].FindControl("lblPeriod_" + ac) as Label).Text);
                            //getcelltag = Convert.ToString(FpSpread1.Sheets[0].Cells[ar, ac].Tag);//lblPeriod_1

                            string avoidholiday = string.Empty;
                            string avoidholidaytext = string.Empty;
                            string[] spiltgetceltag = getcelltag.Split('*');
                            string[] spilttext = text_val.Split('*');

                            for (int k = 0; k <= spiltgetceltag.GetUpperBound(0); k++)
                            {
                                string[] spitvalue = spiltgetceltag[k].Split('-');  //Session["collegecode"].ToString()
                                string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString() + "'");
                                string[] splitminimumabsentsms = minimumabsentsms.Split('-');
                                if (spitvalue.Length > 0)
                                {
                                    if (spitvalue.Length == 7)
                                        if (!arrBatchYear.Contains(Convert.ToString(spitvalue[3])))
                                            arrBatchYear.Add(Convert.ToString(spitvalue[3]));
                                    if (spitvalue.Length == 8)
                                        if (!arrBatchYear.Contains(Convert.ToString(spitvalue[4])))
                                            arrBatchYear.Add(Convert.ToString(spitvalue[4]));

                                    if (!arrDegreeCode.Contains(Convert.ToString(spitvalue[0])))
                                        arrDegreeCode.Add(Convert.ToString(spitvalue[0]));
                                    if (spitvalue.Length > 1)
                                    {
                                        if (!arrSemester.Contains(Convert.ToString(spitvalue[1])))
                                            arrSemester.Add(Convert.ToString(spitvalue[1]));
                                    }
                                }
                                if (splitminimumabsentsms.Length == 2)
                                {
                                    int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                                    int checkbatchyear = 0;
                                    if (spitvalue.Length == 7)
                                    {
                                        checkbatchyear = Convert.ToInt32(spitvalue[3].ToString());
                                    }
                                    if (spitvalue.Length == 8)
                                    {
                                        checkbatchyear = Convert.ToInt32(spitvalue[4].ToString());
                                    }
                                    if (splitminimumabsentsms[0].ToString() == "1" && checkbatchyear >= batchyearsetting)
                                    {
                                        Session["StaffSelector"] = "1";
                                    }
                                    else
                                    {
                                        Session["StaffSelector"] = "0";
                                    }
                                }
                                else
                                {
                                    Session["StaffSelector"] = "0";
                                }
                                if (spitvalue[0].ToLower().Trim() == "selected day is holiday")
                                {
                                }
                                else
                                {
                                    if (avoidholiday == "")
                                    {
                                        avoidholiday = spiltgetceltag[k].ToString();
                                        avoidholidaytext = spilttext[k].ToString();
                                    }
                                    else
                                    {
                                        avoidholiday = avoidholiday + '*' + spiltgetceltag[k].ToString();
                                        avoidholidaytext = avoidholidaytext + '*' + spilttext[k].ToString();
                                    }
                                }
                            }
                            hr = ac.ToString();
                            string qryBatchYear = string.Join("','", arrBatchYear.ToArray());
                            string qryDegreeCode = string.Join("','", arrDegreeCode.ToArray());
                            string qrySemester = string.Join("','", arrSemester.ToArray());
                            if (!string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qrySemester))
                            {
                                string qry = "select distinct Min(bs.start_time) as start_time,Max(bs.end_time) as end_time,bs.Period1,CONVERT(varchar(5),Min(bs.start_time),108) as startTime,CONVERT(varchar(5),MAx(bs.end_time),108) as endTime,CONVERT(varchar(5),Min(bs.start_time),108)+' - '+CONVERT(varchar(5),MAx(bs.end_time),108) as PeriodDuration from BellSchedule bs where bs.Period1 not like '%break%' and bs.batch_year in('" + qryBatchYear + "') and bs.Degree_Code in('" + qryDegreeCode + "') and bs.semester in('" + qrySemester + "') and bs.Period1='" + hr + "' group by bs.Period1  ; select distinct bs.batch_year,bs.Degree_Code,bs.semester,bs.Period1,Min(bs.start_time) as start_time,MAx(bs.end_time) as end_time,CONVERT(varchar(5),Min(bs.start_time),108) as startTime,CONVERT(varchar(5),MAx(bs.end_time),108) as endTime,CONVERT(varchar(5),Min(bs.start_time),108)+' - '+CONVERT(varchar(5),MAx(bs.end_time),108) as PeriodDuration from BellSchedule bs where bs.Period1 not like '%break%' and bs.batch_year in('" + qryBatchYear + "') and bs.Degree_Code in('" + qryDegreeCode + "') and bs.semester in('" + qrySemester + "') and bs.Period1='" + hr + "' group by bs.batch_year,bs.Degree_Code,bs.semester,bs.Period1 order by bs.batch_year desc,bs.Degree_Code,bs.semester desc,bs.Period1";
                                dsBellTime = dirAcc.selectDataSet(qry);
                            }

                            DateTime utcTime = DateTime.UtcNow;
                            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi); // convert from utc to local

                            int hour = 0, minute = 0, second = 0;
                            DateTime dtPeriodTime = new DateTime();//2000, 1, 1, hour, minute, second
                            DateTime dtActualPeriodStartTime = new DateTime();
                            DateTime dtActualPeriodEndTime = new DateTime();
                            DateTime dtAttendanceDate = new DateTime();

                            string curDate = Convert.ToString((gridTimeTable.Rows[ar].FindControl("lblDateDisp") as Label).Text);
                            lbldate.Text = curDate;
                            DateTime.TryParseExact(Convert.ToString(curDate), "d-MM-yyyy", null, DateTimeStyles.None, out dtAttendanceDate);
                            if (dsBellTime.Tables.Count > 0 && dsBellTime.Tables[0].Rows.Count > 0)
                            {
                                string startTime = Convert.ToString(dsBellTime.Tables[0].Rows[0]["startTime"]).Trim();
                                string endTime = Convert.ToString(dsBellTime.Tables[0].Rows[0]["endTime"]).Trim();

                                DateTime.TryParseExact(startTime, "HH:mm", null, DateTimeStyles.None, out dtActualPeriodStartTime);
                                DateTime.TryParseExact(endTime, "HH:mm", null, DateTimeStyles.None, out dtActualPeriodEndTime);
                                dtPeriodTime = new DateTime(dtAttendanceDate.Year, dtAttendanceDate.Month, dtAttendanceDate.Day, dtActualPeriodStartTime.Hour, dtActualPeriodStartTime.Minute, 0);
                            }
                            else
                            {
                                dtPeriodTime = new DateTime(dtAttendanceDate.Year, dtAttendanceDate.Month, dtAttendanceDate.Day, localTime.Hour, localTime.Minute, localTime.Second);
                            }
                            if (dtPeriodTime > localTime)
                            {
                                headerpanelhomework.Visible = false;
                                //btnaddhme.Visible = false;
                                Buttonsave.Visible = false;
                                //Buttonupdate.Visible = false;
                                pHeaderatendence.Visible = false;
                                pHeaderlesson.Visible = false;
                                headerpanelnotes.Visible = false;
                                pBodyatendence.Visible = false;
                                pBodylesson.Visible = false;
                                pBodynotes.Visible = false;
                                pBodyquestionaddition.Visible = false;
                                pBodyhomework.Visible = false;
                                headerquestionaddition.Visible = false;
                                headerADDQuestion.Visible = false;
                                lbl_alert.Visible = true;
                                //Added by srinath 7/9/2013
                                //  lbl_alert.Text = "You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator";
                                //Added by srinath 25/8/2016 JPR
                                lbl_alert.Text = " You cannot edit this day/Hour attendance due to security reasons. Because the period is still not started";
                                GridView1.Visible = false;
                                Buttondeselect.Visible = false;
                                Buttonselectall.Visible = false;
                                lblmanysubject.Visible = false;
                                ddlselectmanysub.Visible = false;
                                return;
                            }

                            getcelltag = avoidholiday;
                            if (avoidholidaytext.Contains("<br/>"))
                                avoidholidaytext = avoidholidaytext.Replace("<br/>", "");

                            text_val = avoidholidaytext;
                            // string[] spitdate = FpSpread1.Sheets[0].RowHeader.Cells[ar, 0].Text.Split('(');
                            sel_date1 = curDate.ToString();
                            getcolheader = Convert.ToString(gridTimeTable.HeaderRow.Cells[ac].Text);
                            string[] sel_date_split = sel_date1.Split(new Char[] { '-' });
                            getdate_new = sel_date_split[0] + "-" + sel_date_split[1] + "-" + sel_date_split[2];
                            if (sel_date_split[0].Length == 1)
                            {
                                sel_date_split[0] = "0" + sel_date_split[0];
                            }
                            if (sel_date_split[1].Length == 1)
                            {
                                sel_date_split[1] = "0" + sel_date_split[1];
                            }
                            sel_date1 = sel_date_split[0] + "-" + sel_date_split[1] + "-" + sel_date_split[2];
                            sel_date = sel_date_split[1] + "-" + sel_date_split[0] + "-" + sel_date_split[2];
                            getdate = sel_date_split[2] + "-" + sel_date_split[1] + "-" + sel_date_split[0];
                            string[] subject1 = spread_text.Split(new char[] { '2' });
                            sub_name = subject1[0].ToString();
                            string[] treepath1 = sub_name.Split(new char[] { '-' });
                            if (treepath1.GetUpperBound(0) == 1)
                            {
                                sub_name = treepath1[0].ToString();
                            }
                            else if (treepath1.GetUpperBound(0) > 1)
                            {
                                sub_name = treepath1[0] + "-" + treepath1[1];
                            }
                            selectedpath = sub_name + " " + "/";
                            storepath = selectedpath;
                            DateTime tem = Convert.ToDateTime(sel_date);
                            strday = tem.ToString("ddd");
                            GetFeeOfRollStudent(ref dicFeeOfRollStudents, ref dicFeeOnRollStudents, Convert.ToString(Convert.ToDateTime(sel_date).ToString("dd-MM-yyyy")));
                            // string text_val = FpSpread1.Sheets[0].Cells[ar, ac].Text;
                            string[] splittext = text_val.Split(new char[] { '*' });
                            string[] splitvalue = getcelltag.Split('*');
                            string split_val = string.Empty;
                            ddlselectmanysub.Items.Clear();
                            for (int splitvalue_star = 0; splitvalue_star <= splittext.GetUpperBound(0); splitvalue_star++)
                            {
                                int max_val = ddlselectmanysub.Items.Count;
                                split_val = splittext[splitvalue_star].ToString();
                                ddlselectmanysub.Items.Add(split_val);
                                ddlselectmanysub.Items[max_val].Value = splitvalue[splitvalue_star].ToString();
                                ddlclassnotes.Items.Add(split_val);//Added by srinath 22/3/2015
                                ddlclassnotes.Items[max_val].Value = splitvalue[splitvalue_star].ToString();//Added by srinath 22/3/2015
                            }
                            ddlselectmanysub.Items.Insert(0, " ");
                            if (ddlselectmanysub.Items.Count >= 3)
                            {
                                ddlselectmanysub.Visible = true;
                                lblmanysubject.Visible = true;
                                ddlselectmanysub.SelectedIndex = 0;
                                ddlclassnotes.Items.Insert(0, "All");//Added by srinath 22/3/2015
                            }
                            else
                            {
                                if (ddlselectmanysub.Items.Count == 2)
                                {
                                    ddlselectmanysub.SelectedIndex = 1;
                                }
                                ddlselectmanysub.Visible = false;
                                lblmanysubject.Visible = false;
                            }
                            splitvalue = ddlselectmanysub.SelectedValue.ToString().Split('-');
                            if (splitvalue.GetUpperBound(0) > 0)
                            {
                                if (splitvalue.GetUpperBound(0) == 7)
                                {
                                    string degree_code = splitvalue[0].ToString();
                                    string semester = splitvalue[1].ToString();
                                    string subject_no = splitvalue[2].ToString();
                                    string batch_year = splitvalue[4].ToString();
                                    string secval = string.Empty;
                                    if (splitvalue.GetUpperBound(0) == 7)
                                    {
                                        secval = splitvalue[3];
                                        batch_year = splitvalue[4];
                                    }
                                    else
                                    {
                                        batch_year = splitvalue[3];
                                    }
                                    loadunitssubj_no = subject_no;
                                    bool hrlock = Hour_lock(degree_code, batch_year, semester, getcolheader, secval);
                                    if (hrlock == true)
                                    {
                                        headerpanelhomework.Visible = false;
                                        //btnaddhme.Visible = false;
                                        Buttonsave.Visible = false;
                                        //Buttonupdate.Visible = false;
                                        pHeaderatendence.Visible = false;
                                        pHeaderlesson.Visible = false;
                                        headerpanelnotes.Visible = false;
                                        pBodyatendence.Visible = false;
                                        pBodylesson.Visible = false;
                                        pBodynotes.Visible = false;
                                        pBodyquestionaddition.Visible = false;
                                        headerquestionaddition.Visible = false;
                                        headerADDQuestion.Visible = false;
                                        lbl_alert.Visible = true;
                                        pBodyhomework.Visible = false;
                                        //Added by srinath 7/9/2013
                                        //  lbl_alert.Text = "You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator";
                                        //Added by srinath 25/8/2016 JPR
                                        lbl_alert.Text = " You cannot edit this day/Hour attendance due to security reasons. Get permission from PRINCIPAL to update the attendance";
                                        GridView1.Visible = false;
                                        Buttondeselect.Visible = false;
                                        Buttonselectall.Visible = false;
                                        lblmanysubject.Visible = false;
                                        ddlselectmanysub.Visible = false;
                                        return;
                                    }


                                    retrievespreadattendancequestion(batch_year, degree_code, semester, subject_no, sel_date1);
                                    retrivespreadfornotes(batch_year, degree_code, semester, subject_no, sel_date1);
                                    sprdretrivedate();
                                    btnSave.Visible = true;
                                }
                                else
                                {
                                    string degree_code = splitvalue[0].ToString();
                                    string semester = splitvalue[1].ToString();
                                    string subject_no = splitvalue[2].ToString();
                                    loadunitssubj_no = subject_no;
                                    string batch_year = splitvalue[3].ToString();
                                    string secval = string.Empty;
                                    if (splitvalue.GetUpperBound(0) == 7)
                                    {
                                        secval = splitvalue[3];
                                        batch_year = splitvalue[4];
                                    }
                                    else
                                    {
                                        batch_year = splitvalue[3];
                                    }
                                    loadunitssubj_no = subject_no;
                                    bool hrlock = Hour_lock(degree_code, batch_year, semester, getcolheader, secval);
                                    if (hrlock == true)
                                    {
                                        Buttonsave.Visible = false;
                                        //Buttonupdate.Visible = false;
                                        pHeaderatendence.Visible = false;
                                        pHeaderlesson.Visible = false;
                                        headerpanelnotes.Visible = false;
                                        pBodyatendence.Visible = false;
                                        pBodylesson.Visible = false;
                                        pBodynotes.Visible = false;
                                        pBodyquestionaddition.Visible = false;
                                        headerquestionaddition.Visible = false;
                                        headerADDQuestion.Visible = false;
                                        pBodyhomework.Visible = false;
                                        GridView1.Visible = false;
                                        lbl_alert.Visible = true;
                                        //Added by srinath 25/8/2016 JPR
                                        lbl_alert.Text = " You cannot edit this day/Hour attendance due to security reasons. Get permission from PRINCIPAL to update the attendance";
                                        //Added by srinath 7/9/2013
                                        //  lbl_alert.Text = "You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator";
                                        GridView1.Visible = false;
                                        Buttondeselect.Visible = false;
                                        Buttonselectall.Visible = false;
                                        lblmanysubject.Visible = false;
                                        ddlselectmanysub.Visible = false;
                                        return;
                                    }
                                    retrievespreadattendancequestion(batch_year, degree_code, semester, subject_no, sel_date1);
                                    retrivespreadfornotes(batch_year, degree_code, semester, subject_no, sel_date1);
                                    loadunitssubj_no = subject_no;
                                    sprdretrivedate();
                                    btnSave.Visible = true;
                                }
                            }
                            else
                            {


                            }
                            if (getcelltag != "")
                            {
                                {
                                    if (ck_append.Checked == false)
                                    {
                                        load_attnd_spread();
                                        mark_attendance();
                                        checkleave = false;
                                    }
                                    else
                                    {
                                        if (GridView1.Columns.Count != 8)
                                        {
                                            mark_attendance2();
                                            checkleave = false;
                                        }
                                        else
                                        {
                                            // load_attnd_spread();
                                            mark_attendance();
                                            checkleave = false;
                                        }
                                    }
                                }
                                rbgraphics.Checked = true;
                                loadgraphics();
                                if (slipfalg == true)
                                {
                                    btnsliplist.Enabled = true;
                                }
                                else
                                {
                                    btnsliplist.Enabled = false;
                                }
                                btnSave.Visible = true;
                            }
                            else
                            {
                                headerpanelhomework.Visible = false;
                                //btnaddhme.Visible = false;
                                checkleave = true;
                                GridView1.Visible = false;
                                pHeaderlesson.Visible = false;
                                headerpanelnotes.Visible = false;
                                headerADDQuestion.Visible = false;
                                headerquestionaddition.Visible = false;
                                pBodylesson.Visible = false;
                                pHeaderatendence.Visible = false;
                                pBodyatendence.Visible = false;
                                pBodynotes.Visible = false;
                                pBodyquestionaddition.Visible = false;
                                pBodyhomework.Visible = false;
                            }
                        }
                        else
                        {
                            if (ck_append.Checked == false && GridView1.Visible == false)
                            {
                                headerpanelhomework.Visible = false;
                                //btnaddhme.Visible = false;
                                Buttonsave.Visible = false;
                                // Buttonupdate.Visible = false;
                                pHeaderatendence.Visible = false;
                                pHeaderlesson.Visible = false;
                                headerpanelnotes.Visible = false;
                                pBodyatendence.Visible = false;
                                pBodylesson.Visible = false;
                                pBodynotes.Visible = false;
                                pBodyquestionaddition.Visible = false;
                                headerquestionaddition.Visible = false;
                                headerADDQuestion.Visible = false;
                                pBodyaddquestion.Visible = false;
                                pBodyhomework.Visible = false;
                                lbl_alert.Visible = true;
                                lbl_alert.Text = "Selected Day is Sunday Holiday ";

                            }
                        }
                    }

                    else
                    {
                        headerpanelhomework.Visible = false;
                        //btnaddhme.Visible = false;
                        Labelstaf.Visible = true;
                        Labelstaf.Text = "Select The Subject";
                        GridView1.Visible = false;
                        pHeaderlesson.Visible = false;
                        headerpanelnotes.Visible = false;
                        headerADDQuestion.Visible = false;
                        headerquestionaddition.Visible = false;
                        pBodylesson.Visible = false;
                        pHeaderatendence.Visible = false;
                        pBodyatendence.Visible = false;
                        pBodynotes.Visible = false;
                        pBodyquestionaddition.Visible = false;
                        pBodyhomework.Visible = false;
                    }
                    load_presen_absent_count();
                    if (ddlselectmanysub.Visible == false)
                    {
                        divatt.Style.Value = "margin-left: 450px";
                    }
                    else
                    {
                        divatt.Style.Value = "margin-left: 150px";
                    }
                }
            }
            if ((Session["StafforAdmin"] == "Staff") || (Session["StafforAdmin"] == "Admin"))
            {
                lbl_alert.Visible = false;
            }
            //  clearfield();
            if (GridView1.Rows.Count > 0 && isvisible)
            {
                GridView1.Visible = true;
            }
            else
            {
                Buttonsave.Visible = false;
                //Buttonupdate.Visible = false;

                headerpanelhomework.Visible = false;
                //btnaddhme.Visible = false;

                pHeaderatendence.Visible = false;
                pHeaderlesson.Visible = false;
                headerpanelnotes.Visible = false;
                pBodyatendence.Visible = false;
                pBodylesson.Visible = false;
                pBodynotes.Visible = false;
                pBodyquestionaddition.Visible = false;
                headerquestionaddition.Visible = false;
                headerADDQuestion.Visible = false;
                pBodyhomework.Visible = false;
                pBodyaddquestion.Visible = false;
                GridView1.Visible = false;
            }
            if (ar >= 0 && ac >= 0)
            {
                string text_val1 = Convert.ToString((gridTimeTable.Rows[ar].FindControl("lblPeriod_" + ac) as Label).Text);


                if (text_val1.Trim() == "")
                {
                    if (ck_append.Checked == false)
                    {
                        Buttonsave.Visible = false;
                        //Buttonupdate.Visible = false;
                        pHeaderatendence.Visible = false;
                        pHeaderlesson.Visible = false;
                        headerpanelnotes.Visible = false;
                        pBodyatendence.Visible = false;
                        pBodylesson.Visible = false;
                        pBodynotes.Visible = false;
                        pBodyquestionaddition.Visible = false;
                        headerquestionaddition.Visible = false;
                        headerADDQuestion.Visible = false;
                        pBodyaddquestion.Visible = false;
                        pBodyhomework.Visible = false;

                        //Modified by subburaj 19/8/2014******//
                        lbl_alert.Visible = true;
                        lbl_alert.Text = "Please Select the Scheduled Hour Properly";
                        //***********End*********//

                        // GridView1.Rows.Count = 0;
                        GridView1.Visible = false;
                    }
                    else if (ck_append.Checked == true && GridView1.Visible == false)
                    {
                        Buttonsave.Visible = false;
                        //Buttonupdate.Visible = false;
                        headerpanelhomework.Visible = false;
                        //btnaddhme.Visible = false;
                        pHeaderatendence.Visible = false;
                        pHeaderlesson.Visible = false;
                        headerpanelnotes.Visible = false;
                        pBodyatendence.Visible = false;
                        pBodylesson.Visible = false;
                        pBodynotes.Visible = false;
                        pBodyquestionaddition.Visible = false;
                        headerquestionaddition.Visible = false;
                        headerADDQuestion.Visible = false;
                        pBodyaddquestion.Visible = false;
                        pBodyhomework.Visible = false;
                        //Modified by subburaj 19/8/2014******//
                        lbl_alert.Visible = true;
                        lbl_alert.Text = "Please Select the Scheduled Hour Properly";
                        //***********End*********//

                        //GridView1.Sheets[0].RowCount = 0;
                        GridView1.Visible = false;
                    }
                    else
                    {
                        //Modified by subburaj 19/8/2014******//
                        lbl_alert.Visible = true;
                        lbl_alert.Text = "No  Student(s) were found!";
                        //***********End*********//
                    }
                }
                else
                {
                    if (checkleave == true)
                    {
                        headerpanelhomework.Visible = false;
                        //btnaddhme.Visible = false;
                        Buttonsave.Visible = false;
                        //Buttonupdate.Visible = false;
                        pHeaderatendence.Visible = false;
                        pHeaderlesson.Visible = false;
                        headerpanelnotes.Visible = false;
                        pBodyatendence.Visible = false;
                        pBodylesson.Visible = false;
                        pBodynotes.Visible = false;
                        pBodyquestionaddition.Visible = false;
                        headerquestionaddition.Visible = false;
                        headerADDQuestion.Visible = false;
                        pBodyaddquestion.Visible = false;
                        lbl_alert.Visible = true;
                        GridView1.Visible = false;
                        pBodyhomework.Visible = false;
                        lbl_alert.Text = "Selected Hour is Holiday";
                    }
                }
            }

            if (Session["Copy Attendance"].ToString() == "1")
                check_attendance.Visible = true;
            else
                check_attendance.Visible = false;
            pnotesuploadadd.Visible = false;//Added By Srinath
            loadunits(loadunitssubj_no);

            //tablediv.Visible = false;
            //btnsavewrk.Visible = false;
            btnaddhme_Click();
            loadgview();

        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    public void load_attnd_spread()
    {
        //---------------------------------load rights
        string[] strcomo = new string[20];
        string[] attnd_rights1 = new string[100];
        int i = 0;
        string odrights = da.GetFunction("select rights from  OD_Master_Setting where " + grouporusercode + "");
        if (odrights.Trim() != null && odrights.Trim() != "" && odrights.Trim() != "0")
        {
            string od_rights = string.Empty;
            od_rights = odrights;
            string[] split_od_rights = od_rights.Split(',');
            strcomo = new string[split_od_rights.GetUpperBound(0) + 2];
            strcomo[i++] = string.Empty;

            for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
            {
                strcomo[i++] = split_od_rights[od_temp].ToString();
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
        }

        GridView1.Columns[0].Visible = true;
        GridView1.Columns[1].Visible = true;
        GridView1.Columns[2].Visible = true;
        GridView1.Columns[3].Visible = true;
        GridView1.Columns[4].Visible = false;
        GridView1.Columns[5].Visible = true;
        GridView1.Columns[6].Visible = false;

    }
    protected void GridView1_OnPreRender(object sender, EventArgs e)
    {

    }
    private void AddTotalRow(string labelText, string value)
    {
        //GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
        //row.BackColor = ColorTranslator.FromHtml("#F9F9F9");
        //row.Cells.AddRange(new TableCell[3] { new TableCell (), //Empty Cell
        //                                new TableCell { Text = labelText, HorizontalAlign = HorizontalAlign.Right},
        //                                new TableCell { Text = value, HorizontalAlign = HorizontalAlign.Right } });

        //GridView1.Controls[0].Controls.Add(row);
    }
    protected void Buttonsave_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirmBox.Visible = false;
            divConfirm.Visible = false;
            divPopAlert.Visible = false;
            divPopAlertContent.Visible = false;
            int Cou = GridView1.Rows.Count;
            string msgValue = string.Empty;
            DateTime dtAttDate = new DateTime();
            //string hour = Convert.ToString((GridView1.Rows[1].FindControl("lblHR") as Label).Text);
            string attendanceDate = (GridView1.Rows[0].FindControl("lblDate") as Label).Text;
            string[] dt = attendanceDate.Split('-');
            string date = dt[1].ToString();
            string mnt = dt[0].ToString();
            string yr = dt[2].ToString();
            string curdate = date + "-" + mnt + "-" + yr;

            //Dictionary<DateTime, Dictionary<string, int>> dicStudentAttendance = new Dictionary<DateTime, Dictionary<string, int>>();
            //GridView1.Rows[0].Visible = false;
            int colS = 1;
            string column = string.Empty;
            if (DateTime.TryParseExact(curdate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtAttDate))
            {
                for (int colV = 8; colV < GridView1.Columns.Count; colV += 2)
                {
                    if (GridView1.Columns[colV].Visible == true)
                    {
                        Dictionary<DateTime, Dictionary<string, int>> dicStudentAttendance = new Dictionary<DateTime, Dictionary<string, int>>();
                        string hr = (GridView1.HeaderRow.FindControl("lbl" + colS) as Label).Text;
                        string hour = Convert.ToString(hr.Split(' ')[1]);

                        for (int row = 0; row < GridView1.Rows.Count; row++)
                        {
                            Dictionary<string, int> dicAttCount = new Dictionary<string, int>();
                            string regNo = (GridView1.Rows[row].FindControl("lblRegNo") as Label).Text;
                            string Ho = string.Empty;
                            if (colS == 1)
                                Ho = string.Empty;
                            else
                                Ho = colS.ToString();

                            string attendanceMark = Convert.ToString((GridView1.Rows[row].FindControl("ddlLeavetype" + Ho) as DropDownList).SelectedItem.Text).Trim().ToLower();

                            if (!dicStudentAttendance.ContainsKey(dtAttDate))
                            {
                                if (!dicAttCount.ContainsKey(attendanceMark))
                                {

                                    dicAttCount.Add(attendanceMark, 1);
                                }
                                else
                                {
                                    dicAttCount[attendanceMark] += 1;
                                }
                                dicStudentAttendance.Add(dtAttDate, dicAttCount);
                            }
                            else
                            {
                                dicAttCount = dicStudentAttendance[dtAttDate];
                                if (!dicAttCount.ContainsKey(attendanceMark))
                                {

                                    dicAttCount.Add(attendanceMark, 1);
                                }
                                else
                                {
                                    dicAttCount[attendanceMark] += 1;
                                }
                                dicStudentAttendance[dtAttDate] = dicAttCount;
                            }
                        }
                        int cu = GridView1.Rows.Count;
                        if (dicStudentAttendance.ContainsKey(dtAttDate))
                        {
                            string msgAttCount = string.Empty;
                            Dictionary<string, int> dicStudAttCount = dicStudentAttendance[dtAttDate];
                            foreach (KeyValuePair<string, int> keyItem in dicStudAttCount)
                            {
                                msgAttCount += ((!string.IsNullOrEmpty(keyItem.Key.ToUpper().Trim())) ? keyItem.Key.ToUpper().Trim() : "Unmarked") + "\t:\t" + keyItem.Value + "\t\t";
                            }
                            msgValue += "Attendance Date : " + dtAttDate.ToString("dd/MM/yyyy") + " Hour : " + hour + "\t\t" + msgAttCount;
                        }
                    }
                    colS++;
                }
            }
            lblConfirmMsg.Text = "Do You Want To Save Attendance ? " + ((!string.IsNullOrEmpty(msgValue)) ? msgValue : "");
            divConfirm.Visible = true;
            divConfirmBox.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void Buttonupdate_Click(object sender, EventArgs e)
    {
        Buttonsave_Click(sender, e);
    }
    protected void btnYes_Click(object sender, EventArgs e)
    {
        try
        {
            divPopAlert.Visible = false;
            divPopAlertContent.Visible = false;
            Labelstaf.Visible = false;
            bool nullflag = false;
            bool savefalg = true;//Added by Srinath 23/8/2013
            int savevalue = 0;
            int insert = 0;
            string getroll = string.Empty;
            string batch = string.Empty;
            string section = string.Empty;
            string attendancequery = string.Empty;
            string val = string.Empty;
            string strorder = filterfunction();
            DataSet dsattendance = new DataSet();
            Hashtable hatattendance = new Hashtable();
            Hashtable hatreason = new Hashtable();
            Hashtable hatattvalue = new Hashtable();
            ArrayList notarray = new ArrayList();
            DataSet data1 = new DataSet();
            WebService web = new WebService();
            dailyentryflag = false;
            attendanceentryflag = false;
            bool isSchoolAttendance = false;
            int startsem_date = 0;
            int total_conduct_hour = 0;
            int absent_hour = 0;
            staff_code = (string)Session["Staff_Code"];
            string[] split_tag_val;
            string savehoursqlstrq;
            int totalhor;
            string noofhours_save = string.Empty;
            string no_firsthalf = string.Empty;
            string no_secondhalf = string.Empty;
            string no_minpresent_firsthalf = string.Empty;
            string no_minpresent_secondhalf = string.Empty;
            string min_per_day = string.Empty;
            bool daysGot = false;
            string sb_aattddaayy = string.Empty;
            string str_day = string.Empty;
            string strstaffselector = string.Empty;

            string currdatesmsstngs = da.GetFunctionv("select distinct value from Master_Settings where settings='SMS For Current Date Only'  and " + grouporusercode + "");


            dsattendance.Dispose();
            dsattendance.Reset();
            attendancequery = "select distinct LeaveCode,CalcFlag from AttMasterSetting";
            dsattendance = da.select_method(attendancequery, hat, "Text");
            present_calcflag.Clear();
            absent_calcflag.Clear();
            hatattendance.Clear();
            string minimumabsentsms = da.GetFunction("select value from Master_Settings where settings='Minimum_days_Absent_Sms'");
            if (dsattendance.Tables.Count > 0 && dsattendance.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsattendance.Tables[0].Rows.Count; i++)
                {
                    if (!hatattendance.Contains(dsattendance.Tables[0].Rows[i]["LeaveCode"].ToString()))
                    {
                        hatattendance.Add(dsattendance.Tables[0].Rows[i]["LeaveCode"].ToString(), dsattendance.Tables[0].Rows[i]["CalcFlag"].ToString());
                    }
                    //for Present Absent Count=
                    if (dsattendance.Tables[0].Rows[i]["calcflag"].ToString() == "0")
                    {
                        present_calcflag.Add(dsattendance.Tables[0].Rows[i]["leavecode"].ToString(), dsattendance.Tables[0].Rows[i]["leavecode"].ToString());
                    }
                    if (dsattendance.Tables[0].Rows[i]["calcflag"].ToString() == "1")
                    {
                        absent_calcflag.Add(dsattendance.Tables[0].Rows[i]["leavecode"].ToString(), dsattendance.Tables[0].Rows[i]["leavecode"].ToString());
                    }
                }
            }


            int colIndex = Convert.ToInt32(Session["Col"].ToString());
            int rowIndex = Convert.ToInt32(Session["Row"].ToString());
            getcelltag = Convert.ToString((gridTimeTable.Rows[rowIndex].FindControl("lblPeriod_" + colIndex) as Label).Text);
            string[] spilttext = getcelltag.Split('*');
            for (int j = 0; j <= spilttext.GetUpperBound(0); j++)
            {
                split_tag_val = spilttext[j].Split('-');
                if (split_tag_val.GetUpperBound(0) >= 7)
                {
                    batch = split_tag_val[4].ToString();
                    degree_code = split_tag_val[0].ToString();
                    semester = split_tag_val[1].ToString();
                    subject_no = split_tag_val[2].ToString();
                    section = "and r.isnull(Sections,'')='" + split_tag_val[3].ToString() + "'";
                }
                else
                {
                    batch = split_tag_val[3].ToString();
                    degree_code = split_tag_val[0].ToString();
                    semester = split_tag_val[1].ToString();
                    subject_no = split_tag_val[2].ToString();
                    section = string.Empty;
                }

                #region Added by Idhris 29-12-2016
                if (!degree_code.ToLower().Contains("holiday") && !semester.ToLower().Contains("holiday"))
                {
                    if (!daysGot)
                    {
                        //daysGot = true;
                        savehoursqlstrq = "select No_of_hrs_per_day,no_of_hrs_I_half_day,no_of_hrs_II_half_day,min_pres_II_half_day ,min_pres_I_half_day,min_hrs_per_day  from PeriodAttndSchedule where degree_code=" + degree_code + " and semester=" + semester + "";
                        DataSet dsDa = da.select_method_wo_parameter(savehoursqlstrq, "Text");
                        if (dsDa.Tables.Count > 0 && dsDa.Tables[0].Rows.Count > 0)
                        {
                            noofhours_save = dsDa.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                            no_firsthalf = dsDa.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
                            no_secondhalf = dsDa.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString();
                            no_minpresent_firsthalf = dsDa.Tables[0].Rows[0]["min_pres_I_half_day"].ToString();
                            no_minpresent_secondhalf = dsDa.Tables[0].Rows[0]["min_pres_II_half_day"].ToString();
                            min_per_day = dsDa.Tables[0].Rows[0]["min_hrs_per_day"].ToString();
                        }
                        sb_aattddaayy = "d" + str_day + "d1," + "d" + str_day + "d2," + "d" + str_day + "d3," + "d" + str_day + "d4," + "d" + str_day + "d5," + "d" + str_day + "d6," + "d" + str_day + "d7," + "d" + str_day + "d8," + "d" + str_day + "d9," + "d" + str_day + "d10";
                    }

                #endregion

                    Session["StaffSelector"] = "0";
                    strstaffselector = string.Empty;  //Session["collegecode"].ToString()
                    string staffbatchyear = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString() + "'");
                    string[] splitminimumabsentsms = staffbatchyear.Split('-');
                    if (splitminimumabsentsms.Length == 2)
                    {
                        int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                        if (splitminimumabsentsms[0].ToString() == "1")
                        {
                            if (Convert.ToInt32(batch) >= batchyearsetting)
                            {
                                Session["StaffSelector"] = "1";
                            }
                        }
                    }
                    if (Session["StaffSelector"].ToString() == "1")
                    {
                        strstaffselector = " and s.staffcode like '%" + Session["Staff_Code"].ToString() + "%'";
                    }
                    string startdatequery = string.Empty;
                    startdatequery = "select leavecode from AttMasterSetting where calcflag='2' and collegecode=" + Session["collegecode"].ToString() + "";
                    startdatequery = startdatequery + " select convert(varchar(10),start_date,103) as start_date from seminfo where  degree_code='" + degree_code + "' and semester='" + semester + "' and batch_year='" + batch + "'";
                    startdatequery = startdatequery + " select convert(varchar(10),holiday_date,103)as holiday_date ,halforfull,morning,evening from holidayStudents where degree_code ='" + degree_code + "' and semester ='" + semester + "'";
                    data1.Clear();
                    data1 = da.select_method_wo_parameter(startdatequery, "Text");
                    if (data1.Tables.Count > 0 && data1.Tables[0].Rows.Count > 0)
                    {
                        for (int val1 = 0; val1 < data1.Tables[0].Rows.Count; val1++)
                        {
                            notarray.Add(data1.Tables[0].Rows[val1]["leavecode"].ToString());
                        }
                    }
                }
            }
            string hourwise = string.Empty;
            string hourwisedata = string.Empty;
            DataSet ds1 = new DataSet();
            string settingquery = string.Empty;
            settingquery = "select TextName,Taxtval from Attendance_Settings where  College_Code ='" + Session["collegecode"].ToString() + "'and user_id='" + Session["usercode"].ToString() + "'";

            ds1.Clear();
            ds1 = da.select_method_wo_parameter(settingquery, "Text");
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                DataView dv_demand_data = new DataView();
                ds1.Tables[0].DefaultView.RowFilter = "TextName in ('Hour')";
                dv_demand_data = ds1.Tables[0].DefaultView;
                if (dv_demand_data.Count > 0)
                {
                    if (dv_demand_data[0]["TextName"].ToString() == "Hour" && Convert.ToInt32(dv_demand_data[0]["Taxtval"]) == 1)
                    {
                        hourwise = "1";
                        hourwisedata = "Hour";
                    }
                    else if (dv_demand_data[0]["TextName"].ToString() == "Hour" && Convert.ToInt32(dv_demand_data[0]["Taxtval"]) == 0)
                    {
                        hourwise = "0";
                    }
                }
            }


            if (check_attendance.Checked == false)
            {
                int i = GridView1.Rows.Count;
                //foreach (GridViewRow row in GridView1.Rows)
                //{
                for (int gr = 0; gr < GridView1.Rows.Count; gr++)
                {
                    string appNo = (GridView1.Rows[gr].FindControl("lblAppNo") as Label).Text;
                    string regNo = (GridView1.Rows[gr].FindControl("lblRegNo") as Label).Text;
                    string RollAdmt = (GridView1.Rows[gr].FindControl("lblAdmNo") as Label).Text;
                    string rollNo = (GridView1.Rows[gr].FindControl("lblrollNo") as Label).Text;
                    string collCode = (GridView1.Rows[gr].FindControl("lblCollCode") as Label).Text;
                    string Batch = (GridView1.Rows[gr].FindControl("lblBatch") as Label).Text;
                    string degCode = (GridView1.Rows[gr].FindControl("lbldegCode") as Label).Text;
                    string Sem = (GridView1.Rows[gr].FindControl("lblCurSems") as Label).Text;
                    string Sec = (GridView1.Rows[gr].FindControl("Label6") as Label).Text;
                    string Date = (GridView1.Rows[gr].FindControl("lblDate") as Label).Text;
                    string Hr = (GridView1.Rows[gr].FindControl("lblHR") as Label).Text;


                    string[] split = Date.Split(new Char[] { '-' });
                    str_day = (Convert.ToInt16(split[1].ToString())).ToString();

                    string Atmonth = (Convert.ToInt16(split[0].ToString())).ToString();
                    string Atyear = split[2].ToString();
                    int strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                    int cellVal = 1;

                    for (int ColV = 8; ColV < GridView1.Columns.Count; ColV += 2)
                    {
                        if (GridView1.Columns[ColV].Visible == true && GridView1.Rows[gr].Enabled == true)
                        {
                            string str_hour = Convert.ToString(Hr);
                            string dcolumn = string.Empty;
                            dcolumn = "d" + str_day + "d" + cellVal.ToString();
                            string leavetxt = string.Empty;
                            string reson = string.Empty;

                            if (cellVal == 1)
                            {
                                DropDownList ddlreson = GridView1.Rows[gr].FindControl("ddlReson") as DropDownList;
                                leavetxt = Convert.ToString((GridView1.Rows[gr].FindControl("ddlLeavetype") as DropDownList).SelectedItem.Text);
                                if (ddlreson.Items.Count > 0)
                                    reson = Convert.ToString((GridView1.Rows[gr].FindControl("ddlReson") as DropDownList).SelectedItem.Text);
                            }
                            else
                            {
                                DropDownList ddlreson = GridView1.Rows[gr].FindControl("ddlReson" + cellVal) as DropDownList;
                                leavetxt = Convert.ToString((GridView1.Rows[gr].FindControl("ddlLeavetype" + cellVal) as DropDownList).SelectedItem.Text);
                                if (ddlreson.Items.Count > 0)
                                    reson = Convert.ToString((GridView1.Rows[gr].FindControl("ddlReson" + cellVal) as DropDownList).SelectedItem.Text);
                            }


                            string Att_value1 = Attvalues(leavetxt);
                            string strquery = string.Empty;

                            if (!string.IsNullOrEmpty(rollNo))
                            {
                                if (!string.IsNullOrEmpty(reson))
                                {
                                    hat.Clear();
                                    hat.Add("AttWr_CollegeCode", collCode);// Session["collegecode"].ToString());
                                    hat.Add("AtWr_App_no", appNo);
                                    hat.Add("columnname", dcolumn);
                                    hat.Add("roll_no", rollNo);
                                    hat.Add("month_year", strdate);
                                    hat.Add("values", reson);
                                    strquery = "sp_ins_upd_student_attendance_reason";
                                    insert = da.insert_method(strquery, hat, "sp");
                                }

                                hat.Clear();
                                hat.Add("Att_App_no", appNo);
                                hat.Add("Att_CollegeCode", collCode);// Session["collegecode"].ToString());
                                hat.Add("columnname", dcolumn);
                                hat.Add("roll_no", rollNo);
                                hat.Add("month_year", strdate);
                                hat.Add("values", Att_value1);
                                strquery = "sp_ins_upd_student_attendance";
                                insert = da.insert_method(strquery, hat, "sp");
                                isSchoolAttendance = false;
                                // string minimumabsentsms = da.GetFunction("select value from Master_Settings where settings='Minimum_days_Absent_Sms'");
                                isSchoolAttendance = CheckSchoolOrCollege(collCode);
                                if (isSchoolAttendance)
                                {
                                    attendanceMark(appNo, (int)strdate, sb_aattddaayy, Convert.ToInt32(noofhours_save), Convert.ToInt32(no_firsthalf), Convert.ToInt32(no_secondhalf), Convert.ToInt32(no_minpresent_firsthalf), Convert.ToInt32(no_minpresent_secondhalf), (split[1] + "/" + split[0] + "/" + split[2]), collCode);// Session["collegecode"].ToString());
                                }
                                savefalg = true;
                                getroll = rollNo.ToString();

                                if (hatattendance.Contains(Att_value1.ToString()))
                                {
                                    string att = GetCorrespondingKey(Att_value1.ToString(), hatattendance).ToString();
                                    if (att.Trim() == "1")
                                    {
                                        if (minimumabsentsms.Trim() != "" && minimumabsentsms != null && minimumabsentsms.Trim() != "0")
                                        {
                                            bool abshrs = true;
                                            string[] curedate = Date.Split('-');
                                            DateTime dtstart = Convert.ToDateTime(curedate[1] + '/' + curedate[0] + '/' + curedate[2]);
                                            string strgetval = "select r.degree_code,r.Current_Semester,No_of_hrs_per_day,p.min_pres_I_half_day,no_of_hrs_II_half_day,p.min_pres_I_half_day,p.min_pres_II_half_day,s.start_date from seminfo s,PeriodAttndSchedule p,Registration r where p.degree_code=s.degree_code and r.degree_code=p.degree_code and p.semester=r.Current_Semester and p.semester=s.semester and r.Batch_Year=s.batch_year  and r.Roll_No='" + rollNo + "'";
                                            DataSet dsgetval = da.select_method_wo_parameter(strgetval, "Text");
                                            if (dsgetval.Tables.Count > 0 && dsgetval.Tables[0].Rows.Count > 0)
                                            {
                                                string degree_code = dsgetval.Tables[0].Rows[0]["degree_code"].ToString();
                                                string semester = dsgetval.Tables[0].Rows[0]["Current_Semester"].ToString();
                                                string noofhrs = dsgetval.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
                                                string fhrs = dsgetval.Tables[0].Rows[0]["min_pres_I_half_day"].ToString();
                                                string shrs = dsgetval.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString();
                                                string minfhpre = dsgetval.Tables[0].Rows[0]["min_pres_I_half_day"].ToString();
                                                string minshpre = dsgetval.Tables[0].Rows[0]["min_pres_II_half_day"].ToString();
                                                DateTime st = Convert.ToDateTime(dsgetval.Tables[0].Rows[0]["start_date"].ToString());
                                                string holidayquery = "select CONVERT(nvarchar(15),holiday_date,101) as holidate,halforfull,morning,evening,holiday_desc from holidayStudents where degree_code='" + degree_code + "' and semester='" + semester + "' and holiday_date between '" + st.ToString() + "' and '" + dtstart.ToString() + "'";
                                                DataSet dsholiday = da.select_method_wo_parameter(holidayquery, "Text");
                                                Dictionary<DateTime, string> dtholid = new Dictionary<DateTime, string>();
                                                if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int hd = 0; hd < dsholiday.Tables[0].Rows.Count; hd++)
                                                    {
                                                        DateTime holday = Convert.ToDateTime(dsholiday.Tables[0].Rows[0]["holidate"].ToString());
                                                        string halorfulvalue = dsholiday.Tables[0].Rows[0]["halforfull"].ToString() + '/' + dsholiday.Tables[0].Rows[0]["morning"].ToString() + '/' + dsholiday.Tables[0].Rows[0]["evening"].ToString();
                                                        if (!dtholid.ContainsKey(holday))
                                                        {
                                                            dtholid.Add(holday, halorfulvalue);
                                                        }
                                                    }
                                                }


                                                int endday = int.Parse(minimumabsentsms);
                                                int totpre = 0, totabs = 0, totcount = 0, tothrs = 0;
                                                string periodval = string.Empty;

                                                for (int std = 0; std < endday; std++)
                                                {
                                                    periodval = string.Empty;
                                                    DateTime dtcheda = dtstart.AddDays(-std);
                                                    if (!dtholid.ContainsKey(dtcheda))
                                                    {
                                                        string[] spdate = dtcheda.ToString().Split(' ');
                                                        string str_Date1 = spdate[0].ToString();
                                                        string[] split1 = str_Date1.Split(new Char[] { '/' });
                                                        string str_day1 = (Convert.ToInt16(split1[1].ToString())).ToString();
                                                        string Atmonth1 = (Convert.ToInt16(split1[0].ToString())).ToString();
                                                        string Atyear1 = split1[2].ToString();
                                                        int getmotnyear = (Convert.ToInt32(Atmonth1) + Convert.ToInt32(Atyear1) * 12);
                                                        for (int abhr = 1; abhr <= int.Parse(noofhrs); abhr++)
                                                        {
                                                            if (periodval == "")
                                                            {
                                                                periodval = "d" + str_day1 + "d" + abhr + "";
                                                            }
                                                            else
                                                            {
                                                                periodval = periodval + ',' + "d" + str_day1 + "d" + abhr + "";
                                                            }
                                                        }
                                                        tothrs = 0;
                                                        strgetval = "select " + periodval + " from attendance where roll_no='" + rollNo + "' and month_year='" + getmotnyear + "'";
                                                        dsgetval = da.select_method_wo_parameter(strgetval, "Text");
                                                        if (dsgetval.Tables.Count > 0 && dsgetval.Tables[0].Rows.Count > 0)
                                                        {
                                                            int valhrs = 1;
                                                            for (valhrs = 1; valhrs < int.Parse(fhrs) + 1; valhrs++)
                                                            {
                                                                string atthrval = dsgetval.Tables[0].Rows[0][valhrs - 1].ToString();
                                                                if (atthrval.Trim() != "" && atthrval != null)
                                                                {
                                                                    if (hatattendance.Contains(atthrval))
                                                                    {
                                                                        tothrs++;
                                                                        string absorpre = GetCorrespondingKey(atthrval.ToString(), hatattendance).ToString();
                                                                        if (absorpre == "0")
                                                                        {
                                                                            totpre++;
                                                                        }
                                                                        if (absorpre == "1")
                                                                        {
                                                                            totabs++;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (tothrs >= int.Parse(minfhpre) && totpre < int.Parse(minfhpre) && totabs > 0)
                                                            {
                                                                abshrs = false;
                                                                valhrs--;
                                                            }
                                                            else
                                                            {
                                                                abshrs = true;
                                                                std = endday + 20;
                                                                valhrs = int.Parse(noofhrs) * 10;
                                                            }
                                                            totpre = 0; totabs = 0; tothrs = 0;
                                                            if (abshrs == false)
                                                            {
                                                                for (valhrs = valhrs + 1; valhrs <= int.Parse(noofhrs); valhrs++)
                                                                {
                                                                    string atthrval = dsgetval.Tables[0].Rows[0][valhrs - 1].ToString();
                                                                    if (atthrval.Trim() != "" && atthrval != null)
                                                                    {
                                                                        if (hatattendance.Contains(atthrval))
                                                                        {
                                                                            tothrs++;
                                                                            string absorpre = GetCorrespondingKey(atthrval.ToString(), hatattendance).ToString();
                                                                            if (absorpre == "0")
                                                                            {
                                                                                totpre++;
                                                                            }
                                                                            if (absorpre == "1")
                                                                            {
                                                                                totabs++;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (tothrs >= int.Parse(minshpre) && totpre < int.Parse(minshpre) && totabs > 0)
                                                                {
                                                                    abshrs = false;
                                                                }
                                                                else
                                                                {
                                                                    abshrs = true;
                                                                    std = endday + 20;
                                                                    valhrs = int.Parse(noofhrs) * 10;
                                                                }
                                                                totpre = 0; totabs = 0; tothrs = 0;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        totpre = 0; totabs = 0; tothrs = 0;
                                                        string[] get = dtholid[dtcheda].Split('/');
                                                        string fulldau = get[0].ToString();
                                                        string morin = get[1].ToString();
                                                        string eneveing = get[2].ToString();
                                                        if (fulldau == "1")
                                                        {
                                                            endday++;
                                                        }
                                                        else
                                                        {
                                                            string[] spdate = dtcheda.ToString().Split(' ');
                                                            string str_Date1 = spdate[0].ToString();
                                                            string[] split1 = str_Date1.Split(new Char[] { '/' });
                                                            string str_day1 = (Convert.ToInt16(split1[1].ToString())).ToString();
                                                            string Atmonth1 = (Convert.ToInt16(split1[0].ToString())).ToString();
                                                            string Atyear1 = split1[2].ToString();
                                                            int getmotnyear = (Convert.ToInt32(Atmonth1) + Convert.ToInt32(Atyear1) * 12);
                                                            int finalhrs = 0;
                                                            int minprehours = 0;
                                                            int abhr1 = 1;
                                                            int minprehrs = 0;
                                                            if (morin == "1" || morin.Trim().ToLower() == "true")
                                                            {
                                                                finalhrs = int.Parse(fhrs);
                                                                abhr1 = 1;
                                                                minprehrs = int.Parse(minfhpre);
                                                            }
                                                            if (morin == "1" || morin.Trim().ToLower() == "true")
                                                            {
                                                                finalhrs = int.Parse(noofhrs);
                                                                abhr1 = int.Parse(fhrs) + 1;
                                                                minprehrs = int.Parse(minshpre);
                                                            }
                                                            for (int abhr = abhr1; abhr <= finalhrs; abhr++)
                                                            {
                                                                if (periodval == "")
                                                                {
                                                                    periodval = "d" + str_day1 + "d" + abhr + "";
                                                                }
                                                                else
                                                                {
                                                                    periodval = periodval + ',' + "d" + str_day1 + "d" + abhr + "";
                                                                }
                                                            }
                                                            strgetval = "select " + periodval + " from attendance where roll_no='" + rollNo + "' and month_year='" + getmotnyear + "'";
                                                            dsgetval = da.select_method_wo_parameter(strgetval, "Text");
                                                            if (dsgetval.Tables.Count > 0 && dsgetval.Tables[0].Rows.Count > 0)
                                                            {
                                                                for (int valhrs = abhr1; valhrs < int.Parse(fhrs) + 1; valhrs++)
                                                                {
                                                                    string atthrval = dsgetval.Tables[0].Rows[0][valhrs - 1].ToString();
                                                                    if (atthrval.Trim() != "" && atthrval != null)
                                                                    {
                                                                        if (hatattendance.Contains(atthrval))
                                                                        {
                                                                            tothrs++;
                                                                            string absorpre = GetCorrespondingKey(atthrval.ToString(), hatattendance).ToString();
                                                                            if (absorpre == "0")
                                                                            {
                                                                                totpre++;
                                                                            }
                                                                            if (absorpre == "1")
                                                                            {
                                                                                totabs++;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (tothrs >= int.Parse(minshpre) && totpre < int.Parse(minshpre) && totabs > 0)
                                                                {
                                                                    abshrs = false;
                                                                }
                                                                else
                                                                {
                                                                    abshrs = true;
                                                                    std = endday + 20;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                abshrs = true;
                                                                std = endday + 20;
                                                            }
                                                        }
                                                    }
                                                }
                                                if (abshrs == false)
                                                {
                                                    AttDate = sel_date1.Trim();
                                                    AttHour = str_hour;
                                                    if (AttHour != "")
                                                    {
                                                        string value_return = web.coundected_hour(strdate, startsem_date, rollNo, absent_calcflag, notarray);
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

                                                        //added by Mullai
                                                        DateTime dtatnddate = new DateTime();
                                                        DateTime currdt = new DateTime();
                                                        string currdate = DateTime.Now.ToString("dd/MM/yyyy");
                                                        string[] atnddtsplit = AttDate.Split('-');
                                                        string attnddt = atnddtsplit[0] + "/" + atnddtsplit[1] + "/" + atnddtsplit[2];
                                                        bool atnddate = DateTime.TryParseExact(attnddt.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtatnddate);
                                                        bool currentdt = DateTime.TryParseExact(currdate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out currdt);
                                                        if (currdatesmsstngs == "1")
                                                        {
                                                            if (dtatnddate == currdt)
                                                            {
                                                                SendingSms(rollNo.ToString(), appNo, regNo, RollAdmt, AttDate, AttHour, degree_code, total_conduct_hour, absent_hour);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            SendingSms(rollNo.ToString(), appNo, regNo, RollAdmt, AttDate, AttHour, degree_code, total_conduct_hour, absent_hour);
                                                        }



                                                       // SendingSms(rollNo.ToString(), appNo, regNo, RollAdmt, AttDate, AttHour, degree_code, total_conduct_hour, absent_hour);
                                                        sendvoicecall(rollNo.ToString(), AttDate, AttHour, batch, degree_code);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                AttDate = sel_date1.Trim();
                                                AttHour = str_hour;
                                                if (AttHour != "")
                                                {
                                                    string value_return = web.coundected_hour(strdate, startsem_date, rollNo, absent_calcflag, notarray);
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

                                                    DateTime dtatnddate = new DateTime();
                                                    DateTime currdt = new DateTime();
                                                    string currdate = DateTime.Now.ToString("dd/MM/yyyy");
                                                    string[] atnddtsplit = AttDate.Split('-');
                                                    string attnddt = atnddtsplit[0] + "/" + atnddtsplit[1] + "/" + atnddtsplit[2];
                                                    bool atnddate = DateTime.TryParseExact(attnddt.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtatnddate);
                                                    bool currentdt = DateTime.TryParseExact(currdate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out currdt);
                                                    if (currdatesmsstngs == "1")
                                                    {
                                                        if (dtatnddate == currdt)
                                                        {
                                                            SendingSms(rollNo.ToString(), appNo, regNo, RollAdmt, AttDate, AttHour, degree_code, total_conduct_hour, absent_hour);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        SendingSms(rollNo.ToString(), appNo, regNo, RollAdmt, AttDate, AttHour, degree_code, total_conduct_hour, absent_hour);
                                                    }

                                                   // SendingSms(rollNo.ToString(), appNo, regNo, RollAdmt, AttDate, AttHour, degree_code, total_conduct_hour, absent_hour);
                                                    sendvoicecall(rollNo.ToString(), AttDate, AttHour, batch, degree_code);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            AttDate = sel_date1.Trim();
                                            AttHour = str_hour;
                                            if (AttHour != "")
                                            {
                                                string value_return = web.coundected_hour(strdate, startsem_date, rollNo, absent_calcflag, notarray);
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

                                                DateTime dtatnddate = new DateTime();
                                                DateTime currdt = new DateTime();
                                                string currdate = DateTime.Now.ToString("dd/MM/yyyy");
                                                string[] atnddtsplit = AttDate.Split('-');
                                                string attnddt = atnddtsplit[0] + "/" + atnddtsplit[1] + "/" + atnddtsplit[2];
                                                bool atnddate = DateTime.TryParseExact(attnddt.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtatnddate);
                                                bool currentdt = DateTime.TryParseExact(currdate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out currdt);
                                                if (currdatesmsstngs == "1")
                                                {
                                                    if (dtatnddate == currdt)
                                                    {
                                                        SendingSms(rollNo.ToString(), appNo, regNo, RollAdmt, AttDate, AttHour, degree_code, total_conduct_hour, absent_hour);
                                                    }
                                                }
                                                else
                                                {
                                                    SendingSms(rollNo.ToString(), appNo, regNo, RollAdmt, AttDate, AttHour, degree_code, total_conduct_hour, absent_hour);
                                                }

                                               // SendingSms(rollNo.ToString(), appNo, regNo, RollAdmt, AttDate, AttHour, degree_code, total_conduct_hour, absent_hour);
                                                sendvoicecall(rollNo.ToString(), AttDate, AttHour, batch, degree_code);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        cellVal++;
                    }

                    //}
                }
            }

            #region Else
            //else
            //{

            //    #region Added by Idhris 29-12-2016

            //    #endregion
            //    for (int att_col = 7; att_col <= FpSpread2.Sheets[0].ColumnCount - 2; att_col = att_col + 2)
            //    {
            //        hatreason.Clear();
            //        hatattvalue.Clear();
            //        hr = FpSpread2.Sheets[0].ColumnHeader.Cells[1, att_col].Tag.ToString();
            //        str_Date = FpSpread2.Sheets[0].ColumnHeader.Cells[0, att_col].Text;
            //        string[] split = str_Date.Split(new Char[] { '-' });
            //        str_day = (Convert.ToInt16(split[0].ToString())).ToString();
            //        Atmonth = (Convert.ToInt16(split[1].ToString())).ToString();
            //        Atyear = split[2].ToString();
            //        strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
            //        str_hour = Convert.ToString(hr);
            //        dcolumn = "d" + str_day + "d" + str_hour;
            //        string[] spilttext = getcelltag.Split('*');
            //        for (int j = 0; j <= spilttext.GetUpperBound(0); j++)
            //        {
            //            split_tag_val = spilttext[j].Split('-');
            //            if (split_tag_val.GetUpperBound(0) >= 7)
            //            {
            //                batch = split_tag_val[4].ToString();
            //                degree_code = split_tag_val[0].ToString();
            //                semester = split_tag_val[1].ToString();
            //                subject_no = split_tag_val[2].ToString();
            //                section = "and r.Sections='" + split_tag_val[3].ToString() + "'";
            //            }
            //            else
            //            {
            //                batch = split_tag_val[3].ToString();
            //                degree_code = split_tag_val[0].ToString();
            //                semester = split_tag_val[1].ToString();
            //                subject_no = split_tag_val[2].ToString();
            //                section = string.Empty;
            //            }
            //            #region Added by Idhris 29-12-2016
            //            if (!daysGot)
            //            {
            //                //daysGot = true;
            //                savehoursqlstrq = "select No_of_hrs_per_day,no_of_hrs_I_half_day,no_of_hrs_II_half_day,min_pres_II_half_day ,min_pres_I_half_day,min_hrs_per_day  from PeriodAttndSchedule where degree_code=" + degree_code + " and semester=" + semester + "";
            //                DataSet dsDa = da.select_method_wo_parameter(savehoursqlstrq, "Text");
            //                if (dsDa.Tables.Count > 0 && dsDa.Tables[0].Rows.Count > 0)
            //                {
            //                    noofhours_save = dsDa.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
            //                    no_firsthalf = dsDa.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
            //                    no_secondhalf = dsDa.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString();
            //                    no_minpresent_firsthalf = dsDa.Tables[0].Rows[0]["min_pres_I_half_day"].ToString();
            //                    no_minpresent_secondhalf = dsDa.Tables[0].Rows[0]["min_pres_II_half_day"].ToString();
            //                    min_per_day = dsDa.Tables[0].Rows[0]["min_hrs_per_day"].ToString();
            //                }
            //                sb_aattddaayy = "d" + str_day + "d1," + "d" + str_day + "d2," + "d" + str_day + "d3," + "d" + str_day + "d4," + "d" + str_day + "d5," + "d" + str_day + "d6," + "d" + str_day + "d7," + "d" + str_day + "d8," + "d" + str_day + "d9," + "d" + str_day + "d10";
            //            }
            //            #endregion
            //            Session["StaffSelector"] = "0";
            //            strstaffselector = string.Empty;  //Session["collegecode"].ToString()
            //            string staffbatchyear = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString() + "'");
            //            string[] splitminimumabsentsms = staffbatchyear.Split('-');
            //            if (splitminimumabsentsms.Length == 2)
            //            {
            //                int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
            //                if (splitminimumabsentsms[0].ToString() == "1")
            //                {
            //                    if (Convert.ToInt32(batch) >= batchyearsetting)
            //                    {
            //                        Session["StaffSelector"] = "1";
            //                    }
            //                }
            //            }
            //            if (Session["StaffSelector"].ToString() == "1")
            //            {
            //                strstaffselector = " and s.staffcode like '%" + Session["Staff_Code"].ToString() + "%'";
            //            }
            //            string startdatequery = string.Empty;
            //            startdatequery = "select leavecode from AttMasterSetting where calcflag='2' and collegecode=" + Session["collegecode"].ToString() + "";
            //            startdatequery = startdatequery + " select convert(varchar(10),start_date,103) as start_date from seminfo where  degree_code='" + degree_code + "' and semester='" + semester + "' and batch_year='" + batch + "'";
            //            startdatequery = startdatequery + " select convert(varchar(10),holiday_date,103)as holiday_date ,halforfull,morning,evening from holidayStudents where degree_code ='" + degree_code + "' and semester ='" + semester + "'";
            //            data1.Clear();
            //            data1 = da.select_method_wo_parameter(startdatequery, "Text");
            //            if (data1.Tables.Count > 0 && data1.Tables[0].Rows.Count > 0)
            //            {
            //                for (int val1 = 0; val1 < data1.Tables[0].Rows.Count; val1++)
            //                {
            //                    notarray.Add(data1.Tables[0].Rows[val1]["leavecode"].ToString());
            //                }
            //            }
            //            if (data1.Tables.Count > 1 && data1.Tables[1].Rows.Count > 0)
            //            {
            //                strt_date = data1.Tables[1].Rows[0]["start_date"].ToString();
            //                string[] split1 = strt_date.Split(new Char[] { '/' });
            //                string str_day1 = split1[0].ToString();
            //                string Atmonth1 = split1[1].ToString();
            //                string Atyear1 = split1[2].ToString();
            //                startsem_date = (Convert.ToInt32(Atmonth1) + Convert.ToInt32(Atyear1) * 12);
            //            }
            //            //Check Attendance With Reason
            //            strquery = "select r.roll_no," + dcolumn + " from attendance_withreason a,Registration r where a.roll_no=r.Roll_No and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'Debar' and r.Batch_Year=" + batch + " ";
            //            strquery = strquery + " and r.degree_code=" + degree_code + " and r.Current_Semester=" + semester + " " + section + " and month_year='" + strdate + "' and (" + dcolumn + "<>'' and " + dcolumn + " is not null)";
            //            dsattendance.Reset();
            //            dsattendance.Dispose();
            //            dsattendance = da.select_method(strquery, hat, "Text");
            //            if (dsattendance.Tables.Count > 0 && dsattendance.Tables[0].Rows.Count > 0)
            //            {
            //                for (int i = 0; i < dsattendance.Tables[0].Rows.Count; i++)
            //                {
            //                    str_rollno = dsattendance.Tables[0].Rows[i]["Roll_no"].ToString().Trim().ToLower();
            //                    reason_var = dsattendance.Tables[0].Rows[i]["" + dcolumn + ""].ToString();
            //                    if (!hatreason.Contains(str_rollno))
            //                    {
            //                        hatreason.Add(str_rollno, reason_var);
            //                    }
            //                }
            //            }
            //            //Check Attendance
            //            strquery = "select r.roll_no," + dcolumn + " from attendance a,Registration r,subjectChooser s where r.Roll_No=s.roll_no and a.roll_no=r.Roll_No and s.roll_no=a.roll_no and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'Debar'  and r.Batch_Year=" + batch + " and r.degree_code=" + degree_code + " ";
            //            strquery = strquery + "and r.Current_Semester=" + semester + " " + section + " and month_year='" + strdate + "' and s.subject_no=" + subject_no + " " + strstaffselector + " and (" + dcolumn + "<>''and " + dcolumn + "<>'0' and " + dcolumn + " is not null )";//barath 26.01.17
            //            //and a.Att_CollegeCode=r.college_code 
            //            dsattendance.Reset();
            //            dsattendance.Dispose();
            //            dsattendance = da.select_method(strquery, hat, "Text");
            //            if (dsattendance.Tables.Count > 0 && dsattendance.Tables[0].Rows.Count > 0)
            //            {
            //                for (int i = 0; i < dsattendance.Tables[0].Rows.Count; i++)
            //                {
            //                    str_rollno = dsattendance.Tables[0].Rows[i]["Roll_no"].ToString().Trim().ToLower();
            //                    reason_var = dsattendance.Tables[0].Rows[i]["" + dcolumn + ""].ToString();
            //                    if (!hatattvalue.Contains(str_rollno.Trim().ToLower()))
            //                    {
            //                        hatattvalue.Add(str_rollno.Trim().ToLower(), reason_var);
            //                    }
            //                }
            //            }
            //        }
            //        for (int Att_row = 1; Att_row < FpSpread2.Sheets[0].RowCount - 2; Att_row++)
            //        {
            //            if (FpSpread2.Sheets[0].Cells[Att_row, 1].Text.ToString().Trim() != "" && FpSpread2.Sheets[0].Cells[Att_row, 1].Text.ToString() != null)
            //            {
            //                str_rollno = FpSpread2.Sheets[0].Cells[Att_row, 1].Text.ToString();
            //                AppNo = FpSpread2.Sheets[0].Cells[Att_row, 1].Note.ToString();
            //                regno = FpSpread2.Sheets[0].Cells[Att_row, 2].Text.ToString();
            //                admno = FpSpread2.Sheets[0].Cells[Att_row, 3].Text.ToString();
            //                //barath
            //                string clgcode = FpSpread2.Sheets[0].Cells[Att_row, 1].Tag.ToString();
            //                reason_var = Convert.ToString(FpSpread2.GetEditValue(Att_row, 7));
            //                if (reason_var == "System.Object")
            //                {
            //                    reason_var = FpSpread2.Sheets[0].Cells[Att_row, att_col + 1].Text.ToString();
            //                }
            //                if (reason_var == null)
            //                {
            //                    reason_var = "Null";
            //                }
            //                Att_mark1 = Convert.ToString(FpSpread2.GetEditValue(Att_row, att_col).ToString());
            //                FpSpread2.SaveChanges();
            //                if (Att_mark1 == "System.Object")
            //                {
            //                    Att_mark1 = FpSpread2.Sheets[0].Cells[Att_row, att_col].Text.ToString();
            //                }
            //                if (Att_mark1 != "")
            //                {
            //                    nullflag = true;
            //                }
            //                Att_value1 = Attvalues(Att_mark1);
            //                {
            //                    //Insert Attendance With Reason Value
            //                    if (reason_var.Trim() != "" || hatreason.Contains(str_rollno.Trim().ToLower()))
            //                    {
            //                        val = string.Empty;
            //                        if (hatreason.Contains(str_rollno.Trim().ToLower()))
            //                        {
            //                            val = GetCorrespondingKey(str_rollno.Trim().ToLower(), hatreason).ToString();
            //                        }
            //                        if (val != reason_var)
            //                        {
            //                            hat.Clear();
            //                            hat.Add("AttWr_CollegeCode", clgcode);// Session["collegecode"].ToString());
            //                            hat.Add("AtWr_App_no", AppNo);
            //                            hat.Add("columnname", dcolumn);
            //                            hat.Add("roll_no", str_rollno);
            //                            hat.Add("month_year", strdate);
            //                            hat.Add("values", reason_var);
            //                            strquery = "sp_ins_upd_student_attendance_reason";
            //                            insert = da.insert_method(strquery, hat, "sp");
            //                        }
            //                    }
            //                    //Insert Attendance  Value
            //                    if (Att_value1 != "" || hatattvalue.Contains(str_rollno.Trim().ToLower()))
            //                    {
            //                        val = string.Empty;
            //                        if (hatattvalue.Contains(str_rollno.Trim().ToLower()))
            //                        {
            //                            val = GetCorrespondingKey(str_rollno.Trim().ToLower(), hatattvalue).ToString();
            //                        }
            //                        if (val != Att_value1)
            //                        {
            //                            hat.Clear();
            //                            hat.Add("Att_App_no", AppNo);
            //                            hat.Add("Att_CollegeCode", clgcode);// Session["collegecode"].ToString());
            //                            hat.Add("columnname", dcolumn);
            //                            hat.Add("roll_no", str_rollno);
            //                            hat.Add("month_year", strdate);
            //                            hat.Add("values", Att_value1);
            //                            strquery = "sp_ins_upd_student_attendance";
            //                            insert = da.insert_method(strquery, hat, "sp");
            //                        }
            //                    }
            //                    isSchoolAttendance = false;
            //                    isSchoolAttendance = CheckSchoolOrCollege(clgcode);
            //                    if (isSchoolAttendance)
            //                    {
            //                        attendanceMark(AppNo, (int)strdate, sb_aattddaayy, Convert.ToInt32(noofhours_save), Convert.ToInt32(no_firsthalf), Convert.ToInt32(no_secondhalf), Convert.ToInt32(no_minpresent_firsthalf), Convert.ToInt32(no_minpresent_secondhalf), (split[1] + "/" + split[0] + "/" + split[2]), clgcode);// Session["collegecode"].ToString());
            //                    }
            //                    savefalg = true;
            //                    getroll = str_rollno.ToString();
            //                    if (hatattendance.Contains(Att_value1.ToString()))
            //                    {
            //                        string att = GetCorrespondingKey(Att_value1.ToString(), hatattendance).ToString();
            //                        if (att.Trim() == "1")
            //                        {
            //                            if (minimumabsentsms.Trim() != "" && minimumabsentsms != null && minimumabsentsms.Trim() != "0")
            //                            {
            //                                bool abshrs = true;
            //                                string[] curedate = FpSpread2.Sheets[0].ColumnHeader.Cells[0, att_col].Text.Split('-');
            //                                DateTime dtstart = Convert.ToDateTime(curedate[1] + '/' + curedate[0] + '/' + curedate[2]);
            //                                string strgetval = "select r.degree_code,r.Current_Semester,No_of_hrs_per_day,p.min_pres_I_half_day,no_of_hrs_II_half_day,p.min_pres_I_half_day,p.min_pres_II_half_day,s.start_date from seminfo s,PeriodAttndSchedule p,Registration r where p.degree_code=s.degree_code and r.degree_code=p.degree_code and p.semester=r.Current_Semester and p.semester=s.semester and r.Batch_Year=s.batch_year  and r.Roll_No='" + str_rollno + "'";
            //                                DataSet dsgetval = da.select_method_wo_parameter(strgetval, "Text");
            //                                if (dsgetval.Tables.Count > 0 && dsgetval.Tables[0].Rows.Count > 0)
            //                                {
            //                                    string degree_code = dsgetval.Tables[0].Rows[0]["degree_code"].ToString();
            //                                    string semester = dsgetval.Tables[0].Rows[0]["Current_Semester"].ToString();
            //                                    string noofhrs = dsgetval.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString();
            //                                    string fhrs = dsgetval.Tables[0].Rows[0]["min_pres_I_half_day"].ToString();
            //                                    string shrs = dsgetval.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString();
            //                                    string minfhpre = dsgetval.Tables[0].Rows[0]["min_pres_I_half_day"].ToString();
            //                                    string minshpre = dsgetval.Tables[0].Rows[0]["min_pres_II_half_day"].ToString();
            //                                    DateTime st = Convert.ToDateTime(dsgetval.Tables[0].Rows[0]["start_date"].ToString());
            //                                    string holidayquery = "select CONVERT(nvarchar(15),holiday_date,101) as holidate,halforfull,morning,evening,holiday_desc from holidayStudents where degree_code='" + degree_code + "' and semester='" + semester + "' and holiday_date between '" + st.ToString() + "' and '" + dtstart.ToString() + "'";
            //                                    DataSet dsholiday = da.select_method_wo_parameter(holidayquery, "Text");
            //                                    Dictionary<DateTime, string> dtholid = new Dictionary<DateTime, string>();
            //                                    if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
            //                                    {
            //                                        for (int hd = 0; hd < dsholiday.Tables[0].Rows.Count; hd++)
            //                                        {
            //                                            DateTime holday = Convert.ToDateTime(dsholiday.Tables[0].Rows[0]["holidate"].ToString());
            //                                            string halorfulvalue = dsholiday.Tables[0].Rows[0]["halforfull"].ToString() + '/' + dsholiday.Tables[0].Rows[0]["morning"].ToString() + '/' + dsholiday.Tables[0].Rows[0]["evening"].ToString();
            //                                            if (!dtholid.ContainsKey(holday))
            //                                            {
            //                                                dtholid.Add(holday, halorfulvalue);
            //                                            }
            //                                        }
            //                                    }
            //                                    int endday = int.Parse(minimumabsentsms);
            //                                    int totpre = 0, totabs = 0, totcount = 0, tothrs = 0;
            //                                    string periodval = string.Empty;
            //                                    for (int std = 0; std < endday; std++)
            //                                    {
            //                                        periodval = string.Empty;
            //                                        DateTime dtcheda = dtstart.AddDays(-std);
            //                                        if (!dtholid.ContainsKey(dtcheda))
            //                                        {
            //                                            string[] spdate = dtcheda.ToString().Split(' ');
            //                                            string str_Date1 = spdate[0].ToString();
            //                                            string[] split1 = str_Date1.Split(new Char[] { '/' });
            //                                            string str_day1 = (Convert.ToInt16(split1[1].ToString())).ToString();
            //                                            string Atmonth1 = (Convert.ToInt16(split1[0].ToString())).ToString();
            //                                            string Atyear1 = split1[2].ToString();
            //                                            int getmotnyear = (Convert.ToInt32(Atmonth1) + Convert.ToInt32(Atyear1) * 12);
            //                                            for (int abhr = 1; abhr <= int.Parse(noofhrs); abhr++)
            //                                            {
            //                                                if (periodval == "")
            //                                                {
            //                                                    periodval = "d" + str_day1 + "d" + abhr + "";
            //                                                }
            //                                                else
            //                                                {
            //                                                    periodval = periodval + ',' + "d" + str_day1 + "d" + abhr + "";
            //                                                }
            //                                            }
            //                                            tothrs = 0;
            //                                            strgetval = "select " + periodval + " from attendance where roll_no='" + str_rollno + "' and month_year='" + getmotnyear + "'";
            //                                            dsgetval = da.select_method_wo_parameter(strgetval, "Text");
            //                                            if (dsgetval.Tables.Count > 0 && dsgetval.Tables[0].Rows.Count > 0)
            //                                            {
            //                                                int valhrs = 1;
            //                                                for (valhrs = 1; valhrs < int.Parse(fhrs) + 1; valhrs++)
            //                                                {
            //                                                    string atthrval = dsgetval.Tables[0].Rows[0][valhrs - 1].ToString();
            //                                                    if (atthrval.Trim() != "" && atthrval != null)
            //                                                    {
            //                                                        if (hatattendance.Contains(atthrval))
            //                                                        {
            //                                                            tothrs++;
            //                                                            string absorpre = GetCorrespondingKey(atthrval.ToString(), hatattendance).ToString();
            //                                                            if (absorpre == "0")
            //                                                            {
            //                                                                totpre++;
            //                                                            }
            //                                                            if (absorpre == "1")
            //                                                            {
            //                                                                totabs++;
            //                                                            }
            //                                                        }
            //                                                    }
            //                                                }
            //                                                if (tothrs >= int.Parse(minfhpre) && totpre < int.Parse(minfhpre) && totabs > 0)
            //                                                {
            //                                                    abshrs = false;
            //                                                    valhrs--;
            //                                                }
            //                                                else
            //                                                {
            //                                                    abshrs = true;
            //                                                    std = endday + 20;
            //                                                    valhrs = int.Parse(noofhrs) * 10;
            //                                                }
            //                                                totpre = 0; totabs = 0; tothrs = 0;
            //                                                if (abshrs == false)
            //                                                {
            //                                                    for (valhrs = valhrs + 1; valhrs <= int.Parse(noofhrs); valhrs++)
            //                                                    {
            //                                                        string atthrval = dsgetval.Tables[0].Rows[0][valhrs - 1].ToString();
            //                                                        if (atthrval.Trim() != "" && atthrval != null)
            //                                                        {
            //                                                            if (hatattendance.Contains(atthrval))
            //                                                            {
            //                                                                tothrs++;
            //                                                                string absorpre = GetCorrespondingKey(atthrval.ToString(), hatattendance).ToString();
            //                                                                if (absorpre == "0")
            //                                                                {
            //                                                                    totpre++;
            //                                                                }
            //                                                                if (absorpre == "1")
            //                                                                {
            //                                                                    totabs++;
            //                                                                }
            //                                                            }
            //                                                        }
            //                                                    }
            //                                                    if (tothrs >= int.Parse(minshpre) && totpre < int.Parse(minshpre) && totabs > 0)
            //                                                    {
            //                                                        abshrs = false;
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        abshrs = true;
            //                                                        std = endday + 20;
            //                                                        valhrs = int.Parse(noofhrs) * 10;
            //                                                    }
            //                                                    totpre = 0; totabs = 0; tothrs = 0;
            //                                                }
            //                                            }
            //                                        }
            //                                        else
            //                                        {
            //                                            totpre = 0; totabs = 0; tothrs = 0;
            //                                            string[] get = dtholid[dtcheda].Split('/');
            //                                            string fulldau = get[0].ToString();
            //                                            string morin = get[1].ToString();
            //                                            string eneveing = get[2].ToString();
            //                                            if (fulldau == "1")
            //                                            {
            //                                                endday++;
            //                                            }
            //                                            else
            //                                            {
            //                                                string[] spdate = dtcheda.ToString().Split(' ');
            //                                                string str_Date1 = spdate[0].ToString();
            //                                                string[] split1 = str_Date1.Split(new Char[] { '/' });
            //                                                string str_day1 = (Convert.ToInt16(split1[1].ToString())).ToString();
            //                                                string Atmonth1 = (Convert.ToInt16(split1[0].ToString())).ToString();
            //                                                string Atyear1 = split1[2].ToString();
            //                                                int getmotnyear = (Convert.ToInt32(Atmonth1) + Convert.ToInt32(Atyear1) * 12);
            //                                                int finalhrs = 0;
            //                                                int minprehours = 0;
            //                                                int abhr1 = 1;
            //                                                int minprehrs = 0;
            //                                                if (morin == "1" || morin.Trim().ToLower() == "true")
            //                                                {
            //                                                    finalhrs = int.Parse(fhrs);
            //                                                    abhr1 = 1;
            //                                                    minprehrs = int.Parse(minfhpre);
            //                                                }
            //                                                if (morin == "1" || morin.Trim().ToLower() == "true")
            //                                                {
            //                                                    finalhrs = int.Parse(noofhrs);
            //                                                    abhr1 = int.Parse(fhrs) + 1;
            //                                                    minprehrs = int.Parse(minshpre);
            //                                                }
            //                                                for (int abhr = abhr1; abhr <= finalhrs; abhr++)
            //                                                {
            //                                                    if (periodval == "")
            //                                                    {
            //                                                        periodval = "d" + str_day1 + "d" + abhr + "";
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        periodval = periodval + ',' + "d" + str_day1 + "d" + abhr + "";
            //                                                    }
            //                                                }
            //                                                strgetval = "select " + periodval + " from attendance where roll_no='" + str_rollno + "' and month_year='" + getmotnyear + "'";
            //                                                dsgetval = da.select_method_wo_parameter(strgetval, "Text");
            //                                                if (dsgetval.Tables.Count > 0 && dsgetval.Tables[0].Rows.Count > 0)
            //                                                {
            //                                                    for (int valhrs = abhr1; valhrs < int.Parse(fhrs) + 1; valhrs++)
            //                                                    {
            //                                                        string atthrval = dsgetval.Tables[0].Rows[0][valhrs - 1].ToString();
            //                                                        if (atthrval.Trim() != "" && atthrval != null)
            //                                                        {
            //                                                            if (hatattendance.Contains(atthrval))
            //                                                            {
            //                                                                tothrs++;
            //                                                                string absorpre = GetCorrespondingKey(atthrval.ToString(), hatattendance).ToString();
            //                                                                if (absorpre == "0")
            //                                                                {
            //                                                                    totpre++;
            //                                                                }
            //                                                                if (absorpre == "1")
            //                                                                {
            //                                                                    totabs++;
            //                                                                }
            //                                                            }
            //                                                        }
            //                                                    }
            //                                                    if (tothrs >= int.Parse(minshpre) && totpre < int.Parse(minshpre) && totabs > 0)
            //                                                    {
            //                                                        abshrs = false;
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        abshrs = true;
            //                                                        std = endday + 20;
            //                                                    }
            //                                                }
            //                                                else
            //                                                {
            //                                                    abshrs = true;
            //                                                    std = endday + 20;
            //                                                }
            //                                            }
            //                                        }
            //                                    }
            //                                    if (abshrs == false)
            //                                    {
            //                                        AttDate = str_Date.Trim();
            //                                        AttHour = str_hour;
            //                                        if (AttHour != "")
            //                                        {
            //                                            string value_return = web.coundected_hour(strdate, startsem_date, str_rollno, absent_calcflag, notarray);
            //                                            if (value_return == "Empty")
            //                                            {
            //                                                total_conduct_hour = 1;
            //                                                absent_hour = 1;
            //                                            }
            //                                            else
            //                                            {
            //                                                string[] splitvalue = value_return.Split('-');
            //                                                if (splitvalue.Length > 0)
            //                                                {
            //                                                    if (splitvalue[0].ToString() != "")
            //                                                    {
            //                                                        total_conduct_hour = Convert.ToInt32(splitvalue[0]);
            //                                                        total_conduct_hour++;
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        total_conduct_hour++;
            //                                                    }
            //                                                    if (splitvalue[1].ToString() != "")
            //                                                    {
            //                                                        absent_hour = Convert.ToInt32(splitvalue[1]);
            //                                                        absent_hour++;
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        absent_hour++;
            //                                                    }
            //                                                }
            //                                            }
            //                                            //added by Mullai
            //                                            DateTime dtatnddate = new DateTime();
            //                                            DateTime currdt = new DateTime();
            //                                            string currdate = DateTime.Now.ToString("dd/MM/yyyy");
            //                                            string[] atnddtsplit = AttDate.Split('-');
            //                                            string attnddt = atnddtsplit[0] + "/" + atnddtsplit[1] + "/" + atnddtsplit[2];
            //                                            bool atnddate = DateTime.TryParseExact(AttDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtatnddate);
            //                                            bool currentdt = DateTime.TryParseExact(currdate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out currdt);
            //                                            if (currdatesmsstngs == "1")
            //                                            {
            //                                                if (dtatnddate == currdt)
            //                                                {
            //                                                    SendingSms(str_rollno.ToString(), AppNo, regno, admno, AttDate, AttHour, degree_code, total_conduct_hour, absent_hour);
            //                                                }
            //                                            }
            //                                            else
            //                                            {
            //                                                SendingSms(str_rollno.ToString(), AppNo, regno, admno, AttDate, AttHour, degree_code, total_conduct_hour, absent_hour);
            //                                            }
            //                                            sendvoicecall(str_rollno.ToString(), AttDate, AttHour, batch, degree_code);
            //                                        }
            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    AttDate = str_Date.Trim();
            //                                    AttHour = str_hour;
            //                                    if (AttHour != "")
            //                                    {
            //                                        string value_return = web.coundected_hour(strdate, startsem_date, str_rollno, absent_calcflag, notarray);
            //                                        if (value_return == "Empty")
            //                                        {
            //                                            total_conduct_hour = 1;
            //                                            absent_hour = 1;
            //                                        }
            //                                        else
            //                                        {
            //                                            string[] splitvalue = value_return.Split('-');
            //                                            if (splitvalue.Length > 0)
            //                                            {
            //                                                if (splitvalue[0].ToString() != "")
            //                                                {
            //                                                    total_conduct_hour = Convert.ToInt32(splitvalue[0]);
            //                                                    total_conduct_hour++;
            //                                                }
            //                                                else
            //                                                {
            //                                                    total_conduct_hour++;
            //                                                }
            //                                                if (splitvalue[1].ToString() != "")
            //                                                {
            //                                                    absent_hour = Convert.ToInt32(splitvalue[1]);
            //                                                    absent_hour++;
            //                                                }
            //                                                else
            //                                                {
            //                                                    absent_hour++;
            //                                                }
            //                                            }
            //                                        }
            //                                        //added by Mullai
            //                                        DateTime dtatnddate = new DateTime();
            //                                        DateTime currdt = new DateTime();
            //                                        string currdate = DateTime.Now.ToString("dd/MM/yyyy");
            //                                        string[] atnddtsplit = AttDate.Split('-');
            //                                        string attnddt = atnddtsplit[0] + "/" + atnddtsplit[1] + "/" + atnddtsplit[2];
            //                                        bool atnddate = DateTime.TryParseExact(AttDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtatnddate);
            //                                        bool currentdt = DateTime.TryParseExact(currdate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out currdt);
            //                                        if (currdatesmsstngs == "1")
            //                                        {
            //                                            if (dtatnddate == currdt)
            //                                            {
            //                                                SendingSms(str_rollno.ToString(), AppNo, regno, admno, AttDate, AttHour, degree_code, total_conduct_hour, absent_hour);
            //                                            }
            //                                        }
            //                                        else
            //                                        {
            //                                            SendingSms(str_rollno.ToString(), AppNo, regno, admno, AttDate, AttHour, degree_code, total_conduct_hour, absent_hour);
            //                                        }

            //                                        sendvoicecall(str_rollno.ToString(), AttDate, AttHour, batch, degree_code);
            //                                    }
            //                                }
            //                            }
            //                            else
            //                            {
            //                                AttDate = str_Date.Trim();
            //                                AttHour = str_hour;
            //                                if (AttHour != "")
            //                                {
            //                                    string value_return = web.coundected_hour(strdate, startsem_date, str_rollno, absent_calcflag, notarray);
            //                                    if (value_return == "Empty")
            //                                    {
            //                                        total_conduct_hour = 1;
            //                                        absent_hour = 1;
            //                                    }
            //                                    else
            //                                    {
            //                                        string[] splitvalue = value_return.Split('-');
            //                                        if (splitvalue.Length > 0)
            //                                        {
            //                                            if (splitvalue[0].ToString() != "")
            //                                            {
            //                                                total_conduct_hour = Convert.ToInt32(splitvalue[0]);
            //                                                total_conduct_hour++;
            //                                            }
            //                                            else
            //                                            {
            //                                                total_conduct_hour++;
            //                                            }
            //                                            if (splitvalue[1].ToString() != "")
            //                                            {
            //                                                absent_hour = Convert.ToInt32(splitvalue[1]);
            //                                                absent_hour++;
            //                                            }
            //                                            else
            //                                            {
            //                                                absent_hour++;
            //                                            }
            //                                        }
            //                                    }
            //                                    //added by Mullai
            //                                    DateTime dtatnddate = new DateTime();
            //                                    DateTime currdt = new DateTime();
            //                                    string currdate = DateTime.Now.ToString("dd/MM/yyyy");
            //                                    string[] atnddtsplit = AttDate.Split('-');
            //                                    string attnddt = atnddtsplit[0] + "/" + atnddtsplit[1] + "/" + atnddtsplit[2];
            //                                    bool atnddate = DateTime.TryParseExact(attnddt.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtatnddate);
            //                                    bool currentdt = DateTime.TryParseExact(currdate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out currdt);
            //                                    if (currdatesmsstngs == "1")
            //                                    {
            //                                        if (dtatnddate == currdt)
            //                                        {
            //                                            SendingSms(str_rollno.ToString(), AppNo, regno, admno, AttDate, AttHour, degree_code, total_conduct_hour, absent_hour);
            //                                        }
            //                                    }
            //                                    else
            //                                    {
            //                                        SendingSms(str_rollno.ToString(), AppNo, regno, admno, AttDate, AttHour, degree_code, total_conduct_hour, absent_hour);
            //                                    }
            //                                    sendvoicecall(str_rollno.ToString(), AttDate, AttHour, batch, degree_code);
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        hr = (int.Parse(hr) + 1).ToString();
            //    }
            //}
            #endregion

            string formname = "Student Attendance";
            string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
            string doa = DateTime.Now.ToString("MM/dd/yyy");
            string details = string.Empty;
            if (getroll != "")
            {
                DataSet dsroll = da.select_method("Select Batch_year,Degree_code,Current_semester,Sections from registration where roll_no='" + getroll + "'", hat, "Text");
                if (dsroll.Tables.Count > 0 && dsroll.Tables[0].Rows.Count > 0)
                {
                    details = "" + dsroll.Tables[0].Rows[0]["Degree_code"].ToString() + ": Sem - " + dsroll.Tables[0].Rows[0]["Current_semester"].ToString() + ": Batch Year - " + dsroll.Tables[0].Rows[0]["Batch_year"].ToString();
                    if (dsroll.Tables[0].Rows[0]["Sections"].ToString() != "" && dsroll.Tables[0].Rows[0]["Sections"].ToString() != "0" && dsroll.Tables[0].Rows[0]["Sections"].ToString() != null)
                    {
                        details = details + ": Sections - " + dsroll.Tables[0].Rows[0]["Sections"].ToString();
                    }
                }
            }
            string modules = "0";
            string act_diff = " ";
            string ctsname = "Update the Attendance Entry Details";
            if (savevalue == 1)
            {
                ctsname = "Save the Attendance Entry Details";
            }
            string entrycode = string.Empty;
            if (Session["Entry_Code"] != null)
            {
                entrycode = Session["Entry_Code"].ToString();
            }
            string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','" + savevalue + "','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
            int a = da.update_method_wo_parameter(strlogdetails, "Text");
            ////Set Color
            divConfirmBox.Visible = false;
            divConfirm.Visible = false;

            if (strinvalidroll == "")
            {
                Buttonsavelesson.Enabled = true;
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                lblAlertMsg.Text = "Saved successfully";
                divPopAlert.Visible = true;
                divPopAlertContent.Visible = true;
                return;
                //return;
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully ......But Following Roll No Are Invalid : " + strinvalidroll + "')", true);
                lblAlertMsg.Text = "Saved successfully ......But Following Roll No Are Invalid : " + strinvalidroll + "";
                divPopAlert.Visible = true;
                divPopAlertContent.Visible = true;
                return;
                //return;
            }
        }
        catch
        {

            divConfirmBox.Visible = false;
            divConfirm.Visible = false;
            divPopAlert.Visible = false;
            divPopAlertContent.Visible = false;
        }
    }
    public void load_attendance()
    {
        bool splhr_flag = false;
        string[] split_holiday_status = new string[1000];
        string sections = string.Empty;
        Hashtable hatonduty = new Hashtable();
        DataSet dsonduty = new DataSet();
        Hashtable hatodtot = new Hashtable();
        DataSet dsmark = new DataSet();
        DataView dvmark = new DataView();
        Hashtable has = new Hashtable();
        DateTime temp_date, dt2;
        Hashtable hat_holy = new Hashtable();
        Hashtable temp_has_subj_code = new Hashtable();
        int split_holiday_status_1 = 0, split_holiday_status_2 = 0;
        bool holiflag = false;
        try
        {
            has.Clear();
            has.Add("colege_code", Session["collegecode"].ToString());
            ds_attndmaster = da.select_method("ATT_MASTER_SETTING", has, "sp");
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
                        if (!has_attnd_masterset.ContainsKey(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                        {
                            has_attnd_masterset.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString());
                        }
                    }
                    if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "2")
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
            string get_alter_or_sem = string.Empty;
            string[] split_tag_val = getcelltag.Split('*');
            string Discon = da.GetFunction("select value from Master_Settings where settings='Attendance Discount' and  " + grouporusercode + "");
            string debar = da.GetFunction("select value from Master_Settings where settings='Attendance Debar' and  " + grouporusercode + "");
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


            for (int tag_for = 0; tag_for <= split_tag_val.GetUpperBound(0); tag_for++)
            {
                str = split_tag_val[tag_for].ToString();
                string tempdegree = split_tag_val[tag_for].ToString();
                if (str != "")
                {
                    string[] sp1 = str.Split(new Char[] { '-' });
                    if (sp1[0].ToString().Trim() != "Selected day is Holiday" && sp1[0].ToString().Trim() != "")
                    {
                        string byear = string.Empty;
                        degree_code = sp1[0];
                        semester = sp1[1];
                        string subject_no = sp1[2].Trim();
                        string batch_year = sp1[4].ToString();
                        string subj_type = string.Empty;
                        if (sp1.GetUpperBound(0) == 7)
                        {
                            sections = sp1[3];
                            byear = sp1[4];
                            subj_type = sp1[5];
                            subj_count_in_onehr = sp1[6];
                            get_alter_or_sem = sp1[7];
                        }
                        else
                        {
                            sections = string.Empty;
                            byear = sp1[3];
                            subj_type = sp1[4];
                            subj_count_in_onehr = sp1[5];
                            get_alter_or_sem = sp1[6];
                        }
                        count_master = 0;
                        string splhrsec = string.Empty;
                        string rstrsec = string.Empty;
                        if (sections.Trim() == "" || sections.Trim() == "-1")
                        {
                            strsec = string.Empty;
                            rstrsec = string.Empty;
                            splhrsec = string.Empty;
                        }
                        else
                        {
                            strsec = " and isnull(sections,'')='" + sections + "'";
                            rstrsec = " and isnull(r.sections,'')='" + sections + "'";
                            splhrsec = "and isnull(sections,'')='" + sections + "'";
                        }
                        DataSet ds_student = da.select_method_wo_parameter(" select distinct r.roll_no as 'ROLL NO', r.stud_name as 'STUD NAME', r.reg_no as 'REG NO',r.Adm_Date,p.No_of_hrs_per_day as 'PER DAY',schorder as 'order',start_date,no_of_hrs_I_half_day,no_of_hrs_II_half_day,len(r.roll_no),convert(varchar(15),adm_date,103) as adm_date,r.serialno FROM registration r, Department d ,PeriodAttndSchedule p  ,seminfo s WHERE r.degree_code=p.degree_code and r.Batch_Year=" + byear + "  and  s.batch_Year=" + byear + "  and r.degree_code= " + degree_code + " and s.degree_code= " + degree_code + " and  s.semester=" + semester + " and p.semester=" + semester + "  and (r.CC = 0)  " + dis + "  " + deba + " AND (r.Current_Semester IS NOT NULL)  " + strsec + " ", "Text");
                        int stud_count = ds_student.Tables[0].Rows.Count;
                        int no_of_hrs = int.Parse(ds_student.Tables[0].Rows[0]["PER DAY"].ToString());
                        int mng_hrs = int.Parse(ds_student.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString());
                        int evng_hrs = int.Parse(ds_student.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString());
                        string order = ds_student.Tables[0].Rows[0]["order"].ToString();

                        string sem_start_date = ds_student.Tables[0].Rows[0]["start_date"].ToString();

                        string temp_date1 = Convert.ToString((gridTimeTable.Rows[ar].FindControl("lblDateDisp") as Label).Text);

                        //string temp_date1 = FpSpread1.Sheets[0].RowHeader.Cells[ar, 0].Text;

                        //string[] spitdate = temp_date1;
                        string[] date_split = temp_date1.Split('-');
                        getdate = date_split[0] + "-" + date_split[1] + "-" + date_split[2];
                        string datefrom = sem_start_date;
                        DateTime dt1 = Convert.ToDateTime(sem_start_date);
                        string date2 = getdate;
                        string[] split1 = date2.Split(new Char[] { '-' });
                        string dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                        dt2 = Convert.ToDateTime(dateto.ToString());
                        DateTime spfromdate = dt1;
                        DateTime sptodate = dt2;
                        string dummy_date = string.Empty;
                        string month_year = string.Empty;
                        string strDay = string.Empty;
                        string full_hour = string.Empty;
                        string single_hour = string.Empty;
                        string temp_hr_field = string.Empty;
                        string date_temp_field = string.Empty;
                        ht_sphr.Clear();
                        string hrdetno = string.Empty;
                        string getsphr = "select distinct  date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + degree_code + " and batch_year=" + byear + " and semester=" + semester + " " + splhrsec + " and date between '" + spfromdate.ToString() + "' and '" + sptodate.ToString() + "'";
                        DataSet ds_sphr = da.select_method(getsphr, hat, "Text");
                        if (ds_sphr.Tables.Count > 0 && ds_sphr.Tables[0].Rows.Count > 0)
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
                        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                        {
                            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                        }
                        else
                        {
                            grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                        }
                        string spl_hr_rights = da.GetFunction("select rights from  special_hr_rights where " + grouporusercode + "");
                        if (spl_hr_rights.Trim().ToLower() == "true" || spl_hr_rights.Trim().ToLower() == "1")
                        {
                            splhr_flag = true;
                        }
                        int present_count = 0;
                        temp_date = dt1;
                        string stralldetaisquery = "select distinct r.roll_no,s.subject_no,s.batch,r.adm_date from registration r,subjectchooser s where s.roll_no=r.roll_no and r.batch_year='" + byear + "' and r.degree_code='" + degree_code + "' and s.subject_no='" + subject_no + "' " + rstrsec + "";
                        stralldetaisquery = stralldetaisquery + " ;select distinct r.roll_no,s.subject_no,s.batch,r.adm_date from registration r,subjectchooser_new s where s.roll_no=r.roll_no and r.batch_year='" + byear + "' and r.degree_code='" + degree_code + "' and s.subject_no='" + subject_no + "' " + rstrsec + "";
                        stralldetaisquery = stralldetaisquery + " ;select distinct day_value,hour_value,stu_batch,subject_no,timetablename from laballoc where batch_year='" + byear + "' and degree_code='" + degree_code + "' and subject_no='" + subject_no + "' " + strsec + "";
                        stralldetaisquery = stralldetaisquery + " ;select distinct day_value,hour_value,stu_batch,subject_no,fdate from laballoc_new where batch_year='" + byear + "' and degree_code='" + degree_code + "' and subject_no='" + subject_no + "' " + strsec + "";
                        stralldetaisquery = stralldetaisquery + " ;select a.* from attendance a,registration r where a.roll_no=r.roll_no and r.batch_year='" + byear + "' and r.degree_code='" + degree_code + "' " + rstrsec + "";
                        stralldetaisquery = stralldetaisquery + " ;select a.* from attendance_withreason a,registration r where a.roll_no=r.roll_no and r.batch_year='" + byear + "' and r.degree_code='" + degree_code + "' " + rstrsec + "";
                        stralldetaisquery = stralldetaisquery + " ;select * from Semester_Schedule where batch_year='" + byear + "' and degree_code='" + degree_code + "' and semester='" + semester + "'  " + strsec + " order by FromDate desc";
                        stralldetaisquery = stralldetaisquery + " ;select * from Alternate_Schedule where batch_year='" + byear + "' and degree_code='" + degree_code + "' and semester='" + semester + "'  " + strsec + "  order by FromDate desc";
                        DataSet dsalldetails = da.select_method_wo_parameter(stralldetaisquery, "Text");

                        string getdeteails = "select convert(nvarchar(15),s.start_date,101) as start_date,nodays,s.starting_dayorder from seminfo s,PeriodAttndSchedule p where s.degree_code=p.degree_code and p.semester=s.semester and s.semester='" + semester + "' and s.batch_year='" + byear + "'  and s.degree_code='" + degree_code + "'";
                        getdeteails = getdeteails + " ; select * from tbl_consider_day_order where semester='" + semester + "' and batch_year='" + byear + "'  and degree_code='" + degree_code + "'";
                        getdeteails = getdeteails + " ; select subject_type,LAB From sub_sem where subtype_no=(select subtype_no from subject where  subject_no='" + subject_no + "')";
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
                                    for (DateTime dtc = dtdcf; dtc <= dtdct; dtc = dtc.AddDays(1))
                                    {
                                        if (!hatdc.Contains(dtc))
                                        {
                                            hatdc.Add(dtc, dtc);
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                        has.Clear();
                        has.Add("from_date", dt1);
                        has.Add("to_date", dt2);
                        has.Add("degree_code", degree_code);
                        has.Add("sem", semester);
                        has.Add("coll_code", Session["collegecode"].ToString());
                        int iscount = 0;
                        string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + dt1.ToString() + "' and '" + dt2.ToString() + "' and degree_code=" + degree_code + " and semester=" + semester + "";
                        DataSet dsholiday = da.select_method_wo_parameter(sqlstr_holiday, "Text");
                        if (dsholiday.Tables.Count > 0 && dsholiday.Tables[0].Rows.Count > 0)
                        {
                            iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                        }
                        has.Add("iscount", iscount);
                        DataSet ds_holi = da.select_method("HOLIDATE_DETAILS_FINE", has, "sp");
                        string halforfull = "", mng = "", evng = "", holiday_sched_details = string.Empty;
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
                        subj_type = dssem.Tables[2].Rows[0]["LAB"].ToString();
                        while (temp_date <= dt2)
                        {
                            bool check_alter = false;
                            if (!hatdc.Contains(temp_date))
                            {
                                if (splhr_flag == true)
                                {
                                    if (ht_sphr.Contains(Convert.ToString(temp_date)))
                                    {
                                        getspecial_hr(Convert.ToString(temp_date.ToString("MM/dd/yyyy")), subject_no.Trim(), dsalldetails);
                                    }
                                }
                                if (!hat_holy.ContainsKey(temp_date))
                                {
                                    if (!hat_holy.ContainsKey(temp_date))
                                    {
                                        hat_holy.Add(temp_date, "3*0*0");
                                    }
                                }
                                string value_holi_status = GetCorrespondingKey(temp_date, hat_holy).ToString();
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
                                }
                                else
                                {
                                    holiflag = true;
                                    DataView dvaltersech = new DataView();
                                    DataView dvsemsech = new DataView();
                                    if (dsalldetails.Tables.Count > 7 && dsalldetails.Tables[7].Rows.Count > 0)
                                    {
                                        dsalldetails.Tables[7].DefaultView.RowFilter = "degree_code = " + degree_code + " and semester = " + semester + " and batch_year = " + byear + " and FromDate ='" + temp_date + "' " + strsec + "";
                                        dvaltersech = dsalldetails.Tables[7].DefaultView;
                                    }
                                    if (dsalldetails.Tables.Count > 6 && dsalldetails.Tables[6].Rows.Count > 0)
                                    {
                                        dsalldetails.Tables[6].DefaultView.RowFilter = "degree_code = " + degree_code + " and semester = " + semester + " and batch_year = " + byear + " and FromDate <='" + temp_date + "' " + strsec + "";
                                        dvsemsech = dsalldetails.Tables[6].DefaultView;
                                    }
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
                                                string[] sp = dummy_date.Split('/');
                                                string curdate = sp[1] + '/' + sp[0] + '/' + sp[2];
                                                strDay = da.findday(curdate, degree_code, semester, byear, semstartdate, noofdays, startday);
                                            }

                                            for (int temp_hr = split_holiday_status_1; temp_hr <= split_holiday_status_2; temp_hr++)
                                            {
                                                check_alter = false;
                                                present_count = 0;
                                                temp_hr_field = strDay + temp_hr;
                                                date_temp_field = "d" + final_date_string[1].ToString() + "d" + temp_hr;
                                                if (dvaltersech.Count > 0)
                                                {
                                                    for (int hasrow = 0; hasrow < dvaltersech.Count; hasrow++)
                                                    {
                                                        full_hour = dvaltersech[hasrow][temp_hr_field].ToString();
                                                        if (full_hour.Trim() != "")
                                                        {
                                                            check_alter = true;
                                                            temp_has_subj_code.Clear();
                                                            string[] split_full_hour = full_hour.Split(';');
                                                            for (int semi_colon = 0; semi_colon <= split_full_hour.GetUpperBound(0); semi_colon++)
                                                            {
                                                                single_hour = split_full_hour[semi_colon].ToString();
                                                                string[] split_single_hour = single_hour.Split('-');
                                                                if (split_single_hour.GetUpperBound(0) >= 1)
                                                                {
                                                                    string subjectno = Convert.ToString(split_single_hour[0]).Trim();
                                                                    staff_code = Convert.ToString(Session["Staff_Code"]);
                                                                    if (subject_no.Trim() == subjectno.Trim())
                                                                    {
                                                                        if (!temp_has_subj_code.ContainsKey(subject_no.Trim()))
                                                                        {
                                                                            temp_has_subj_code.Add(subject_no.Trim(), subject_no.Trim());
                                                                            Hashtable has_stud_list = new Hashtable();
                                                                            subj_type = dssem.Tables[2].Rows[0]["LAB"].ToString();
                                                                            if (subj_type != "1" && subj_type.Trim().ToLower() != "true")
                                                                            {
                                                                                DataView dvlabhr = new DataView();
                                                                                if (dsalldetails.Tables.Count > 0 && dsalldetails.Tables[0].Rows.Count > 0)
                                                                                {
                                                                                    dsalldetails.Tables[0].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subject_no + "'";
                                                                                    dvlabhr = dsalldetails.Tables[0].DefaultView;
                                                                                }
                                                                                for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                                                {
                                                                                    string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                                    DataView dvattva = new DataView();
                                                                                    if (dsalldetails.Tables.Count > 4 && dsalldetails.Tables[4].Rows.Count > 0)
                                                                                    {
                                                                                        dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                                        dvattva = dsalldetails.Tables[4].DefaultView;
                                                                                    }
                                                                                    bool checkedFeeOfRoll = false;
                                                                                    if (dicFeeOfRollStudents.ContainsKey(rollno.Trim().ToLower()) && dicFeeOnRollStudents.ContainsKey(rollno.Trim().ToLower()))
                                                                                    {
                                                                                        DateTime[] dtFeeOfRoll = dicFeeOfRollStudents[rollno.Trim().ToLower()];
                                                                                        if (temp_date >= dtFeeOfRoll[0])
                                                                                        {
                                                                                            bool hasRollOff = false;
                                                                                            DateTime dtDefaultDate = new DateTime(1900, 1, 1);//SqlServer Default Date
                                                                                            if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && temp_date < dtFeeOfRoll[1])
                                                                                            {
                                                                                                hasRollOff = true;
                                                                                                checkedFeeOfRoll = true;
                                                                                            }
                                                                                            else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 1)
                                                                                            {
                                                                                                hasRollOff = true;
                                                                                                checkedFeeOfRoll = true;
                                                                                            }
                                                                                            else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && dtFeeOfRoll[1] == dtDefaultDate)
                                                                                            {
                                                                                                hasRollOff = true;
                                                                                                checkedFeeOfRoll = true;
                                                                                            }
                                                                                            //else if (dicFeeOnRollStudents[rollNo.Trim()] == 0 && dtSelDate >= dtFeeOfRoll[1])
                                                                                            //{
                                                                                            //    hasRollOff = false;
                                                                                            //    checkedFeeOfRoll = false;
                                                                                            //}
                                                                                            else
                                                                                            {
                                                                                                hasRollOff = false;
                                                                                                checkedFeeOfRoll = false;
                                                                                            }
                                                                                            if (hasRollOff)
                                                                                            {

                                                                                                if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                                {
                                                                                                    present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                    present_count++;
                                                                                                    has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    has_total_attnd_hour.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    if (dvattva.Count > 0)
                                                                                    {
                                                                                        string attval = Convert.ToString(dvattva[0][date_temp_field]).Trim();
                                                                                        if (!string.IsNullOrEmpty(attval) && attval.Trim() != "12" && attval.Trim() != "8" && attval.Trim() != "0")
                                                                                        {
                                                                                            if (has_attnd_masterset.ContainsKey(attval.Trim()))
                                                                                            {
                                                                                                if (has_load_rollno.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                                {
                                                                                                    present_count = Convert.ToInt16(has_load_rollno[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                    present_count++;
                                                                                                    has_load_rollno[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    has_load_rollno.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                                }
                                                                                            }
                                                                                            if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                            {
                                                                                                present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                present_count++;
                                                                                                has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                has_total_attnd_hour.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                dsalldetails.Tables[3].DefaultView.RowFilter = "hour_value=" + temp_hr + "  and day_value='" + strDay + "' and subject_no='" + subject_no + "' and fdate='" + temp_date.ToString("MM/dd/yyyy").ToString() + "'";
                                                                                DataView dvlabbatch = dsalldetails.Tables[3].DefaultView;
                                                                                for (int lb = 0; lb < dvlabbatch.Count; lb++)
                                                                                {
                                                                                    string batch = dvlabbatch[lb]["stu_batch"].ToString();
                                                                                    if (batch != null && batch.Trim() != "")
                                                                                    {
                                                                                        DataView dvlabhr = new DataView();
                                                                                        if (dsalldetails.Tables.Count > 1 && dsalldetails.Tables[1].Rows.Count > 0)
                                                                                        {
                                                                                            dsalldetails.Tables[1].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subject_no + "' and batch='" + batch + "' ";
                                                                                            dvlabhr = dsalldetails.Tables[1].DefaultView;
                                                                                        }
                                                                                        for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                                                        {
                                                                                            string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                                            DataView dvattva = new DataView();
                                                                                            if (dsalldetails.Tables.Count > 4 && dsalldetails.Tables[4].Rows.Count > 0)
                                                                                            {
                                                                                                dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                                                dvattva = dsalldetails.Tables[4].DefaultView;
                                                                                            }
                                                                                            bool checkedFeeOfRoll = false;
                                                                                            if (dicFeeOfRollStudents.ContainsKey(rollno.Trim().ToLower()) && dicFeeOnRollStudents.ContainsKey(rollno.Trim().ToLower()))
                                                                                            {
                                                                                                DateTime[] dtFeeOfRoll = dicFeeOfRollStudents[rollno.Trim().ToLower()];
                                                                                                if (temp_date >= dtFeeOfRoll[0])
                                                                                                {
                                                                                                    bool hasRollOff = false;
                                                                                                    DateTime dtDefaultDate = new DateTime(1900, 1, 1);//SqlServer Default Date
                                                                                                    if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && temp_date < dtFeeOfRoll[1])
                                                                                                    {
                                                                                                        hasRollOff = true;
                                                                                                        checkedFeeOfRoll = true;
                                                                                                    }
                                                                                                    else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 1)
                                                                                                    {
                                                                                                        hasRollOff = true;
                                                                                                        checkedFeeOfRoll = true;
                                                                                                    }
                                                                                                    else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && dtFeeOfRoll[1] == dtDefaultDate)
                                                                                                    {
                                                                                                        hasRollOff = true;
                                                                                                        checkedFeeOfRoll = true;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        hasRollOff = false;
                                                                                                        checkedFeeOfRoll = false;
                                                                                                    }
                                                                                                    if (hasRollOff)
                                                                                                    {

                                                                                                        if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                                        {
                                                                                                            present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                            present_count++;
                                                                                                            has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            has_total_attnd_hour.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            if (dvattva.Count > 0 && !checkedFeeOfRoll)
                                                                                            {
                                                                                                string attval = dvattva[0][date_temp_field].ToString();
                                                                                                //if (attval != "12" && attval != "8" && attval.Trim() != "")
                                                                                                if (!string.IsNullOrEmpty(attval) && attval.Trim() != "12" && attval.Trim() != "8" && attval.Trim() != "0")
                                                                                                {
                                                                                                    if (has_attnd_masterset.ContainsKey(attval))
                                                                                                    {
                                                                                                        if (has_load_rollno.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                                        {
                                                                                                            present_count = Convert.ToInt16(has_load_rollno[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                            present_count++;
                                                                                                            has_load_rollno[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            has_load_rollno.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                                        }
                                                                                                    }
                                                                                                    if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                                    {
                                                                                                        present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                        present_count++;
                                                                                                        has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        has_total_attnd_hour.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
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
                                                        }
                                                    }
                                                }
                                                present_count = 0;
                                                if (check_alter == false)
                                                {
                                                    full_hour = dvsemsech[0][temp_hr_field].ToString();
                                                    if (full_hour.Trim() != "")
                                                    {
                                                        temp_has_subj_code.Clear();
                                                        string[] split_full_hour_sem = full_hour.Split(';');
                                                        for (int semi_colon = 0; semi_colon <= split_full_hour_sem.GetUpperBound(0); semi_colon++)
                                                        {
                                                            single_hour = split_full_hour_sem[semi_colon].ToString();
                                                            string[] split_single_hour = single_hour.Split('-');
                                                            if (split_single_hour.GetUpperBound(0) >= 1)
                                                            {
                                                                string subjectno = split_single_hour[0].ToString().Trim();
                                                                //if (subject_no == subjectno)
                                                                //{
                                                                staff_code = Convert.ToString(Session["Staff_Code"]);
                                                                if (subject_no.Trim() == subjectno.Trim())
                                                                {
                                                                    if (!temp_has_subj_code.ContainsKey(subject_no.Trim()))
                                                                    {
                                                                        temp_has_subj_code.Add(subject_no.Trim(), subject_no.Trim());
                                                                        Hashtable has_stud_list = new Hashtable();
                                                                        subj_type = dssem.Tables[2].Rows[0]["LAB"].ToString();
                                                                        if (subj_type.Trim() != "1" && subj_type != "True" && subj_type != "TRUE" && subj_type.Trim().ToLower() != "true")
                                                                        {
                                                                            dsalldetails.Tables[0].DefaultView.RowFilter = " adm_date<='" + temp_date.ToString("MM/dd/yyyy") + "' and subject_no='" + subject_no + "'";
                                                                            DataView dvlabhr = dsalldetails.Tables[0].DefaultView;
                                                                            for (int sstu = 0; sstu < dvlabhr.Count; sstu++)
                                                                            {
                                                                                string rollno = dvlabhr[sstu]["roll_no"].ToString().Trim().ToLower();
                                                                                dsalldetails.Tables[4].DefaultView.RowFilter = " month_year='" + month_year.ToString() + "' and roll_no='" + rollno + "'";
                                                                                DataView dvattva = dsalldetails.Tables[4].DefaultView;
                                                                                bool checkedFeeOfRoll = false;
                                                                                if (dicFeeOfRollStudents.ContainsKey(rollno.Trim().ToLower()) && dicFeeOnRollStudents.ContainsKey(rollno.Trim().ToLower()))
                                                                                {
                                                                                    DateTime[] dtFeeOfRoll = dicFeeOfRollStudents[rollno.Trim().ToLower()];
                                                                                    if (temp_date >= dtFeeOfRoll[0])
                                                                                    {
                                                                                        bool hasRollOff = false;
                                                                                        DateTime dtDefaultDate = new DateTime(1900, 1, 1);//SqlServer Default Date
                                                                                        if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && temp_date < dtFeeOfRoll[1])
                                                                                        {
                                                                                            hasRollOff = true;
                                                                                            checkedFeeOfRoll = true;
                                                                                        }
                                                                                        else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 1)
                                                                                        {
                                                                                            hasRollOff = true;
                                                                                            checkedFeeOfRoll = true;
                                                                                        }
                                                                                        else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && dtFeeOfRoll[1] == dtDefaultDate)
                                                                                        {
                                                                                            hasRollOff = true;
                                                                                            checkedFeeOfRoll = true;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            hasRollOff = false;
                                                                                            checkedFeeOfRoll = false;
                                                                                        }
                                                                                        if (hasRollOff)
                                                                                        {

                                                                                            if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                            {
                                                                                                present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                present_count++;
                                                                                                has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                has_total_attnd_hour.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                if (dvattva.Count > 0 && !checkedFeeOfRoll)
                                                                                {
                                                                                    string attval = dvattva[0][date_temp_field].ToString();
                                                                                    //if (attval != "12" && attval != "8" && attval.Trim() != "")
                                                                                    if (!string.IsNullOrEmpty(attval) && attval.Trim() != "12" && attval.Trim() != "8" && attval.Trim() != "0")
                                                                                    {
                                                                                        if (has_attnd_masterset.ContainsKey(attval))
                                                                                        {
                                                                                            if (has_load_rollno.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                            {
                                                                                                present_count = Convert.ToInt16(has_load_rollno[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                present_count++;
                                                                                                has_load_rollno[rollno + '-' + subjectno] = present_count;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                has_load_rollno.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                            }
                                                                                        }
                                                                                        if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                        {
                                                                                            present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                            present_count++;
                                                                                            has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            has_total_attnd_hour.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
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
                                                                                        bool checkedFeeOfRoll = false;
                                                                                        if (dicFeeOfRollStudents.ContainsKey(rollno.Trim().ToLower()) && dicFeeOnRollStudents.ContainsKey(rollno.Trim().ToLower()))
                                                                                        {
                                                                                            DateTime[] dtFeeOfRoll = dicFeeOfRollStudents[rollno.Trim().ToLower()];
                                                                                            if (temp_date >= dtFeeOfRoll[0])
                                                                                            {
                                                                                                bool hasRollOff = false;
                                                                                                DateTime dtDefaultDate = new DateTime(1900, 1, 1);//SqlServer Default Date
                                                                                                if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && temp_date < dtFeeOfRoll[1])
                                                                                                {
                                                                                                    hasRollOff = true;
                                                                                                    checkedFeeOfRoll = true;
                                                                                                }
                                                                                                else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 1)
                                                                                                {
                                                                                                    hasRollOff = true;
                                                                                                    checkedFeeOfRoll = true;
                                                                                                }
                                                                                                else if (dicFeeOnRollStudents[rollno.Trim().ToLower()] == 0 && dtFeeOfRoll[1] == dtDefaultDate)
                                                                                                {
                                                                                                    hasRollOff = true;
                                                                                                    checkedFeeOfRoll = true;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    hasRollOff = false;
                                                                                                    checkedFeeOfRoll = false;
                                                                                                }
                                                                                                if (hasRollOff)
                                                                                                {

                                                                                                    if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                                    {
                                                                                                        present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                        present_count++;
                                                                                                        has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        has_total_attnd_hour.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        if (dvattva.Count > 0 && !checkedFeeOfRoll)
                                                                                        {
                                                                                            string attval = dvattva[0][date_temp_field].ToString();
                                                                                            //if (attval != "12" && attval != "8" && attval.Trim() != "")
                                                                                            if (!string.IsNullOrEmpty(attval) && attval.Trim() != "12" && attval.Trim() != "8" && attval.Trim() != "0")
                                                                                            {
                                                                                                if (has_attnd_masterset.ContainsKey(attval))
                                                                                                {
                                                                                                    if (has_load_rollno.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                                    {
                                                                                                        present_count = Convert.ToInt16(has_load_rollno[rollno + '-' + subjectno]);
                                                                                                        present_count++;
                                                                                                        has_load_rollno[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        has_load_rollno.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
                                                                                                    }
                                                                                                }
                                                                                                if (has_total_attnd_hour.Contains(rollno.Trim() + '-' + subjectno.Trim()))
                                                                                                {
                                                                                                    present_count = Convert.ToInt16(has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()]);
                                                                                                    present_count++;
                                                                                                    has_total_attnd_hour[rollno.Trim() + '-' + subjectno.Trim()] = present_count;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    has_total_attnd_hour.Add(rollno.Trim() + '-' + subjectno.Trim(), 1);
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
                                                    }
                                                }
                                            }
                                            check_alter = false;
                                        }
                                    }
                                }
                            }
                            temp_date = temp_date.AddDays(1);
                        }
                        foreach (GridViewRow row in GridView1.Rows)
                        {
                            string roll = (row.FindControl("lblrollNo") as Label).Text.ToLower();
                            double attnd_hr = 0, tot_hr = 0;
                            if (has_total_attnd_hour.Contains(roll.Trim() + '-' + subject_no.Trim()))
                            {
                                tot_hr = Convert.ToDouble(has_total_attnd_hour[roll.Trim() + '-' + subject_no.Trim()]);
                                if (has_load_rollno.Contains(roll.Trim() + '-' + subject_no.Trim()))
                                {
                                    attnd_hr = Convert.ToDouble(has_load_rollno[roll.Trim() + '-' + subject_no.Trim()]);
                                }
                                Label lblTotHR = (row.FindControl("lblStaff") as Label);
                                lblTotHR.Text = tot_hr.ToString() + " ( " + attnd_hr.ToString() + " )";
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void gridTimeTable_OnDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
        }
        catch
        {

        }
    }
    public void seColor()
    {
        try
        {
            string checkalter = string.Empty;
            string Discon = da.GetFunction("select value from Master_Settings where settings='Attendance Discount' and  " + grouporusercode + "");
            string debar = da.GetFunction("select value from Master_Settings where settings='Attendance Debar' and  " + grouporusercode + "");
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


            foreach (GridViewRow dr1 in gridTimeTable.Rows)
            {

                for (int i = 1; i <= 10; i++)
                {
                    string str_Date = (dr1.FindControl("lblDate") as Label).Text;
                    string linktext = "lblPeriod_" + i;
                    string linkVal = "lnkPeriod_" + i;
                    string getcelltag = (dr1.FindControl(linktext) as Label).Text;
                    string getText = (dr1.FindControl(linkVal) as LinkButton).Text;
                    if (!string.IsNullOrEmpty(getcelltag))
                    {
                        string[] split = str_Date.Split(new Char[] { '/' });
                        //string linkVal = "lnkPeriod_" + i;
                        LinkButton lnkbtn = (dr1.FindControl(linkVal) as LinkButton);
                        string str_day = (Convert.ToInt16(split[1].ToString())).ToString();
                        string Atmonth = (Convert.ToInt16(split[0].ToString())).ToString();
                        string Atyear = split[2].ToString();
                        int strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                        string str_hour = Convert.ToString(i);
                        string dcolumn = "d" + str_day + "d" + str_hour;
                        DateTime date = Convert.ToDateTime(split[0].ToString() + '-' + split[1].ToString() + '-' + split[2].ToString());
                        string day = date.ToString("ddd");
                        string[] spilttext = getcelltag.Split('*');
                        if (!getcelltag.ToLower().Contains("holiday") && !getText.ToLower().Contains("holiday"))
                        {
                            for (int j = 0; j <= spilttext.GetUpperBound(0); j++)
                            {
                                string batch = string.Empty;
                                string section = string.Empty;
                                bool colorflag = false;
                                if (j > 0)
                                {
                                    if (lnkbtn.ForeColor == Color.Blue)
                                    {
                                        colorflag = true;
                                    }
                                    else
                                    {
                                        colorflag = false;
                                    }
                                }
                                if (colorflag == false)
                                {
                                    string check_lab = string.Empty;
                                    dailyentryflag = false;
                                    attendanceentryflag = false;
                                    string[] split_tag_val = spilttext[j].Split('-');
                                    if (split_tag_val.GetUpperBound(0) >= 7)
                                    {
                                        batch = split_tag_val[4].ToString();
                                        degree_code = split_tag_val[0].ToString();
                                        semester = split_tag_val[1].ToString();
                                        subject_no = split_tag_val[2].ToString();
                                        //section = "and Registration.Sections='" + split_tag_val[3].ToString() + "'";
                                        section = split_tag_val[3].ToString();
                                        checkalter = split_tag_val[7].ToString();
                                        check_lab = split_tag_val[5].ToString();
                                    }
                                    else
                                    {
                                        batch = split_tag_val[3].ToString();
                                        degree_code = split_tag_val[0].ToString();
                                        semester = split_tag_val[1].ToString();
                                        subject_no = split_tag_val[2].ToString();
                                        section = string.Empty;
                                        checkalter = split_tag_val[6].ToString();
                                        check_lab = split_tag_val[4].ToString();
                                    }

                                    string sectionvar = string.Empty;
                                    if (section.Trim() != "" && section != null && section != "-1")
                                    {
                                        sectionvar = " and isnull(sections,'')='" + section + "'";
                                    }
                                    Session["StaffSelector"] = "0";
                                    string strstaffselector = string.Empty;   //Session["collegecode"].ToString()
                                    string staffbatchyear = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString() + "'");
                                    string[] splitminimumabsentsms = staffbatchyear.Split('-');
                                    if (splitminimumabsentsms.Length == 2)
                                    {
                                        int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                                        if (splitminimumabsentsms[0].ToString() == "1")
                                        {
                                            if (Convert.ToInt32(batch) >= batchyearsetting)
                                            {
                                                Session["StaffSelector"] = "1";
                                            }
                                        }
                                    }
                                    if (Session["StaffSelector"].ToString() == "1")
                                    {
                                        strstaffselector = " and s.staffcode like '%" + Session["Staff_Code"].ToString() + "%'";
                                    }
                                    string stuCount = string.Empty;
                                    if (check_lab == "L" || check_lab.Trim().ToLower() == "l")
                                    {
                                        string strquery = "  select p.schOrder,p.nodays,Convert(nvarchar(15),s.start_date,23) as start,s.starting_dayorder from PeriodAttndSchedule p, seminfo s where s.degree_code=p.degree_code and s.semester=p.semester and s.degree_code=" + degree_code + " and s.semester=" + semester + " AND s.batch_year=" + batch + "";
                                        Day_Order = "0";
                                        DataSet dsattendance = da.select_method(strquery, hat, "Text");
                                        if (dsattendance.Tables.Count > 0 && dsattendance.Tables[0].Rows.Count > 0)
                                        {
                                            Day_Order = dsattendance.Tables[0].Rows[0]["schOrder"].ToString();
                                            noofdays = dsattendance.Tables[0].Rows[0]["nodays"].ToString();
                                            start_datesem = dsattendance.Tables[0].Rows[0]["start"].ToString();
                                            start_dayorder = dsattendance.Tables[0].Rows[0]["starting_dayorder"].ToString();
                                        }
                                        //Week / Day order
                                        if (Day_Order == "1")
                                        {
                                            day = date.ToString("ddd");
                                        }
                                        else
                                        {
                                            day = da.findday(date.ToString(), degree_code, semester, batch, start_datesem.ToString(), noofdays.ToString(), start_dayorder);//Modifeied By Srianth add comman Daccess 5/9/2014
                                        }





                                        if (checkalter.ToLower().Trim() == "alter")
                                        {
                                            hat.Clear();
                                            hat.Add("batch_year", batch);
                                            hat.Add("degree_code", degree_code);
                                            hat.Add("sem", semester);
                                            hat.Add("sections", section);
                                            hat.Add("month_year", strdate);
                                            hat.Add("date", date);
                                            hat.Add("subject_no", subject_no);
                                            hat.Add("day", day);
                                            hat.Add("hour", str_hour);
                                            ds.Reset();
                                            ds.Dispose();
                                            ds = da.select_method("sp_stu_atten_month_check_lab_alter", hat, "sp");
                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                            {
                                                Att_strqueryst = ds.Tables[0].Rows[0]["stucount"].ToString();
                                                if (int.Parse(Att_strqueryst) > 0)
                                                {
                                                    hat.Clear();
                                                    ds.Reset();
                                                    ds.Dispose();
                                                    string strgetatt = "select count(distinct r.Roll_No) as stucount from registration r,attendance a,subjectchooser_new s where degree_code='" + degree_code + "' and current_semester='" + semester + "' and batch_year='" + batch + "' and cc=0 " + dis + " " + deba + " and month_year=" + strdate + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and";

                                                    strgetatt = strgetatt + " r.current_semester=s.semester and subject_no='" + subject_no + "' " + sectionvar + " and(" + dcolumn + " is not null and " + dcolumn + "<>'0' and " + dcolumn + "<>'') and fromdate='" + date + "' and batch    in(select stu_batch from laballoc_new where subject_no='" + subject_no + "'  and batch_year='" + batch + "'  and hour_value='" + str_hour + "'  and    degree_code='" + degree_code + "' ";
                                                    strgetatt = strgetatt + " and day_value='" + day + "' and semester='" + semester + "' " + sectionvar + " and fdate='" + date + "') and adm_date<='" + date + "'";
                                                    ds = da.select_method_wo_parameter(strgetatt, "Text");
                                                    //ds = da.select_method("sp_stu_atten_day_check_lab_alter", hat, "sp");
                                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        if (Att_strqueryst == ds.Tables[0].Rows[0]["stucount"].ToString())
                                                        {
                                                            Att_strqueryst = "0";
                                                        }
                                                        else
                                                        {
                                                            Att_strqueryst = "1";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Att_strqueryst = "1";
                                                    }
                                                }
                                                else
                                                {
                                                    Att_strqueryst = "1";
                                                }
                                            }
                                            else
                                            {
                                                Att_strqueryst = "1";
                                            }
                                        }


                                        else
                                        {
                                            string timetable = da.GetFunction("select top 1 TTName,  FromDate from  Semester_Schedule where degree_code='" + degree_code + "' and semester='" + semester + "' and batch_year='" + batch + "' " + sectionvar + " and FromDate<='" + date.ToString("MM/dd/yyyy") + "' order by FromDate Desc");
                                            hat.Clear();
                                            string strstt = "select count(distinct r.Roll_No) as stucount from registration r,subjectchooser s where degree_code='" + degree_code + "' and ";
                                            strstt = strstt + " current_semester='" + semester + "' and batch_year='" + batch.ToString() + "' and cc=0 " + dis + " " + deba + " and r.roll_no=s.roll_no ";
                                            strstt = strstt + " and r.current_semester=s.semester and subject_no='" + subject_no + "'  " + sectionvar + "  and batch in(select stu_batch from ";
                                            strstt = strstt + " laballoc where subject_no='" + subject_no + "'  and batch_year='" + batch.ToString() + "'  and hour_value='" + str_hour + "' and degree_code='" + degree_code + "' ";
                                            strstt = strstt + " and day_value='" + day + "' and semester='" + semester + "'  " + sectionvar + " and Timetablename='" + timetable + "') and adm_date<='" + date.ToString("MM/dd/yyyy") + "'  " + strstaffselector + "";
                                            ds.Reset();
                                            ds.Dispose();
                                            //ds = da.select_method("sp_stu_atten_month_check_lab", hat, "sp");
                                            ds = da.select_method_wo_parameter(strstt, "Text");
                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                            {
                                                Att_strqueryst = ds.Tables[0].Rows[0]["stucount"].ToString();
                                                if (int.Parse(Att_strqueryst) > 0)
                                                {
                                                    hat.Clear();
                                                    ds.Reset();
                                                    ds.Dispose();
                                                    string strgetatt = "select count( r.Roll_No) as stucount from registration r,attendance a,subjectchooser s where degree_code='" + degree_code + "' and current_semester='" + semester + "' and batch_year='" + batch + "' and cc=0 " + dis + " " + deba + " and month_year=" + strdate + "";
                                                    strgetatt = strgetatt + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and r.current_semester=s.semester " + dis + " " + deba + " and subject_no='" + subject_no + "' " + sectionvar + " and(" + dcolumn + " is not null and " + dcolumn + "<>'0' and " + dcolumn + "<>'') and batch in(select stu_batch from laballoc ";
                                                    strgetatt = strgetatt + " where subject_no='" + subject_no + "' and Timetablename='" + timetable + "' and batch_year='" + batch + "'  and hour_value='" + str_hour + "'  and    degree_code='" + degree_code + "' and day_value='" + day + "' and semester='" + semester + "' " + sectionvar + ") and adm_date<='" + date + "' " + strstaffselector + "";
                                                    ds = da.select_method_wo_parameter(strgetatt, "Text");
                                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        if (Att_strqueryst == ds.Tables[0].Rows[0]["stucount"].ToString())
                                                        {
                                                            Att_strqueryst = "0";
                                                        }
                                                        else
                                                        {
                                                            Att_strqueryst = "1";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Att_strqueryst = "1";
                                                    }
                                                }
                                                else
                                                {
                                                    Att_strqueryst = "1";
                                                }
                                            }
                                            else
                                            {
                                                Att_strqueryst = "1";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (checkalter.ToLower().Trim() == "alter")
                                            strstaffselector = string.Empty;
                                        hat.Clear();
                                        string strgetatt1 = "select count(distinct r.Roll_No) as stucount from registration r,subjectchooser s where  r.roll_no=s.roll_no " + dis + " " + deba + " and ";
                                        strgetatt1 = strgetatt1 + " r.current_semester=s.semester and batch_year='" + batch + "' and degree_code='" + degree_code + "'  and current_semester='" + semester + "' " + sectionvar + " ";
                                        strgetatt1 = strgetatt1 + "  and subject_no='" + subject_no + "'  and adm_date<='" + date + "' and cc=0  " + dis + " " + deba + "  " + strstaffselector + "";
                                        ds = da.select_method_wo_parameter(strgetatt1, "Text");
                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            stuCount = Convert.ToString(ds.Tables[0].Rows[0]["stucount"]);
                                            Att_strqueryst = ds.Tables[0].Rows[0]["stucount"].ToString();
                                            if (int.Parse(Att_strqueryst) > 0)
                                            {
                                                hat.Clear();
                                                hat.Add("columnname", dcolumn);
                                                hat.Add("batch_year", batch);
                                                hat.Add("degree_code", degree_code);
                                                hat.Add("sem", semester);
                                                hat.Add("sections", section);
                                                hat.Add("month_year", strdate);
                                                hat.Add("date", date);
                                                hat.Add("subject_no", subject_no);
                                                ds.Reset();
                                                ds.Dispose();
                                                string strgetatt = "select count(registration.roll_no) as stucount  from registration,attendance,subjectchooser s where degree_code='" + degree_code + "' and current_semester='" + semester + "' and batch_year='" + batch + "' and cc=0 " + dis + " " + deba + " and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and  registration.roll_no=s.roll_no ";
                                                strgetatt = strgetatt + " and registration.current_semester=s.semester and subject_no='" + subject_no + "' " + sectionvar + "";
                                                strgetatt = strgetatt + " and (" + dcolumn + " is not null and " + dcolumn + "<>'0' and " + dcolumn + "<>'') and adm_date<='" + date + "'  " + strstaffselector + "";
                                                ds = da.select_method_wo_parameter(strgetatt, "Text");
                                                //    ds = da.select_method("sp_stu_atten_day_check", hat, "sp");
                                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                {
                                                    if (Att_strqueryst == ds.Tables[0].Rows[0]["stucount"].ToString())
                                                    {
                                                        Att_strqueryst = "0";
                                                    }
                                                    else
                                                    {
                                                        Att_strqueryst = "1";
                                                    }
                                                }
                                                else
                                                {
                                                    Att_strqueryst = "1";
                                                }
                                            }
                                            else
                                            {
                                                Att_strqueryst = "1";
                                            }
                                        }
                                        else
                                        {
                                            Att_strqueryst = "1";
                                        }
                                    }
                                    if (int.Parse(Att_strqueryst) > 0 && stuCount!="0")
                                    {
                                        //FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.Blue;
                                        attendanceentryflag = false;
                                        // lnkbtn.ForeColor = Color.DarkTurquoise;
                                    }
                                    else
                                    {
                                        attendanceentryflag = true;
                                        //FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.ForestGreen;
                                    }
                                    if (section.Trim() == "" || section == null || section == "-1")
                                    {
                                        section = string.Empty;
                                    }
                                    else
                                    {
                                        section = " and isnull(Sections,'')='" + section + "'";
                                    }
                                    strquerytext = "select de.lp_code from dailyentdet de,dailystaffentry ds where de.lp_code=ds.lp_code  and batch_year=" + batch + " and degree_code='" + degree_code + "' and semester=" + semester + " " + section + " and subject_no='" + subject_no + "' and  staff_code='" + staff_code + "' and sch_date='" + date + "' and hr=" + str_hour + "";
                                    ds.Reset();
                                    ds.Dispose();
                                    ds = da.select_method(strquerytext, hat, "Text");
                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        dailyentryflag = true;
                                    }
                                    if (dailyentryflag == false && attendanceentryflag == false)
                                    {
                                        lnkbtn.ForeColor = Color.Blue;
                                        j = spilttext.GetUpperBound(0) + 1;
                                    }
                                    else if (dailyentryflag == true && attendanceentryflag == false)
                                    {
                                        if (lnkbtn.ForeColor == Color.DarkOrchid)
                                        {
                                            lnkbtn.ForeColor = Color.Blue;
                                        }
                                        else
                                        {
                                            lnkbtn.ForeColor = Color.DarkTurquoise;
                                        }
                                    }
                                    else if (dailyentryflag == false && attendanceentryflag == true)
                                    {
                                        if (lnkbtn.ForeColor == Color.DarkTurquoise)
                                        {
                                            lnkbtn.ForeColor = Color.Blue;
                                        }
                                        else
                                        {
                                            lnkbtn.ForeColor = Color.DarkOrchid;
                                        }
                                    }
                                    else
                                    {
                                        if (j == 0)
                                        {
                                            lnkbtn.ForeColor = Color.ForestGreen;
                                        }
                                        else
                                        {
                                            if (lnkbtn.ForeColor == Color.ForestGreen)
                                            {
                                                lnkbtn.ForeColor = Color.ForestGreen;
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
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    public void load_presen_absent_count()
    {
    }
    protected void btndailyentrydelete_Click(object sender, EventArgs e)
    {
        try
        {
            string topics = string.Empty;
            string sch_order = string.Empty;
            Hashtable hatdelnode = new Hashtable();
            foreach (TreeNode node in tvcomplete.CheckedNodes)
            {
                if (topics == "")
                {
                    topics = topics + node.Value;
                    hatdelnode.Add(node.Value, node.Value);
                    selectedpath = selectedpath + node.ValuePath;
                }
                else
                {
                    topics = topics + "/" + node.Value;
                    hatdelnode.Add(node.Value, node.Value);
                    selectedpath = selectedpath + "=" + node.ValuePath;
                }
            }
            if (topics == "")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('There is nothing to Delete from Topics Completed')", true);
                return;
            }
            string order_day = string.Empty;
            string query = "select schorder from PeriodAttndSchedule where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString();
            string order = da.GetFunction(query);
            if (order == "")
                return;
            string curday = Session["sch_date"].ToString();
            DateTime day_lesson = Convert.ToDateTime(curday);
            if (order != "0")
                order_day = day_lesson.ToString("ddd");
            else
            {
                order_day = find_day_order();
                if (order_day == "")
                    return;
            }
            if (order_day == "mon")
                sch_order = "1";
            else if (order_day == "tue")
                sch_order = "2";
            else if (order_day == "wed")
                sch_order = "3";
            else if (order_day == "thu")
                sch_order = "4";
            else if (order_day == "fri")
                sch_order = "5";
            else if (order_day == "sat")
                sch_order = "6";
            else if (order_day == "sun")
                sch_order = "7";
            if (order_day == "Mon")
                sch_order = "1";
            else if (order_day == "Tue")
                sch_order = "2";
            else if (order_day == "Wed")
                sch_order = "3";
            else if (order_day == "Thu")
                sch_order = "4";
            else if (order_day == "Fri")
                sch_order = "5";
            else if (order_day == "Sat")
                sch_order = "6";
            else if (order_day == "Sun")
                sch_order = "7";
            string subj_no = (string)Session["sub_no"].ToString();
            string hour_hr = (string)Session["hr"].ToString();

            string lp_code = string.Empty;
            int a = 0;
            string updatenode = string.Empty;
            string strdailtquery = "select * from dailyStaffEntry where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "'";
            DataSet dsdaily = da.select_method(strdailtquery, hat, "Text");
            if (dsdaily.Tables.Count > 0 && dsdaily.Tables[0].Rows.Count > 0)
            {
                string lp_query = "select lp_code from dailyStaffEntry where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "' and sch_order=" + sch_order;
                lp_code = da.GetFunction(lp_query);
                if (lp_code != "")
                {
                    string del = "select topics from dailyEntdet where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "";
                    strdailtquery = da.GetFunction(del);
                    string[] strb = strdailtquery.Split('/');
                    for (int st = 0; st <= strb.GetUpperBound(0); st++)
                    {
                        string getva = strb[st].ToString();
                        if (!hatdelnode.Contains(getva))
                        {
                            if (updatenode == "")
                            {
                                updatenode = getva;
                            }
                            else
                            {
                                updatenode = updatenode + '/' + getva;
                            }
                        }
                    }
                }
            }

            string entrycode = Session["Entry_Code"].ToString();//saranya
            string PageName = "Student Attendance";
            string batch = Session["batch_year"].ToString();
            string TimeOfAttendance = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
            string DateOfAttendence = DateTime.Now.ToString("MM/dd/yyy");
            if (updatenode.Trim() == "" || updatenode == "0")//Update Here//
            {
                //string struopdatye = "update  dailyEntdet set topics='" + updatenode + "' where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "";
                string struopdatye = "delete  dailyEntdet  where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "";

                int upqu = da.update_method_wo_parameter(struopdatye, "text");

                string ctsname = "Update the Daily Entry Information";

                da.insertUserActionLog(entrycode, batch, degree_code, semester, sections, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 2);//saranya
            }
            else//Delete Here
            {
                string struopdatye = "delete dailyEntdet where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "";
                int upqu = da.update_method_wo_parameter(struopdatye, "text");
                struopdatye = "delete dailyStaffEntry where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "' and sch_order=" + sch_order;
                upqu = da.update_method_wo_parameter(struopdatye, "text");

                string ctsname = "Delete the Daily Entry Information";
                da.insertUserActionLog(entrycode, batch, degree_code, semester, sections, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 3);//saranya
            }

            int.TryParse(Convert.ToString(Session["Row"]), out ar);
            int.TryParse(Convert.ToString(Session["Col"]), out ac);

            //ar = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
            //ac = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
            string col = "lblPeriod_" + ac;


            if (ar != -1)
            {
                string spread_text = (gridTimeTable.Rows[ar].FindControl(col) as Label).Text;
                //string spread_text = FpSpread1.Sheets[0].Cells[ar, ac].Text;
                getcelltag = ddlselectmanysub.SelectedValue.ToString();
                //int.TryParse(Convert.ToString(Session["Row"]), out ar);
                //int.TryParse(Convert.ToString(Session["Col"]), out ac);

                string getdate = (gridTimeTable.Rows[ar].FindControl("lblDateDisp") as Label).Text;
                //string getdate = FpSpread1.Sheets[0].Cells[ar, 0].Text;
                string[] splitvalue = getcelltag.Split(new char[] { '-' });
                if (splitvalue.GetUpperBound(0) > 0)
                {
                    string degree_code = splitvalue[0].ToString();
                    string semester = splitvalue[1].ToString();
                    string subject_no = splitvalue[2].ToString();
                    string batch_year = string.Empty;
                    if (splitvalue.GetUpperBound(0) == 7)
                    {
                        batch_year = splitvalue[4].ToString();
                    }
                    else
                    {
                        batch_year = splitvalue[3].ToString();
                    }

                    filltree();
                }

            }
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted successfully')", true);
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void Buttonsavelesson_Click(object sender, EventArgs e)
    {
        try
        {
            bool isInsert = false;

            // ddlother.Items.FindByText("Select");

            string Discon = da.GetFunction("select value from Master_Settings where settings='Attendance Discount' and  " + grouporusercode + "");
            string debar = da.GetFunction("select value from Master_Settings where settings='Attendance Debar' and  " + grouporusercode + "");
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



            if (chkalterlession.Checked == true)
            {
                savelessionalter();
            }
            else
            {
                string topics = string.Empty;
                string sch_order = string.Empty;
                foreach (TreeNode node in tvyet.CheckedNodes)
                {
                    if (topics == "")
                    {
                        topics = topics + node.Value;
                        selectedpath = selectedpath + node.ValuePath;
                    }
                    else
                    {
                        topics = topics + "/" + node.Value;
                        selectedpath = selectedpath + "=" + node.ValuePath;
                    }
                }
                btnsaves(sender, e);
                ddlother.SelectedIndex = ddlother.Items.IndexOf(ddlother.Items.FindByText("Select"));
                foreach (TreeNode node in tvyet.CheckedNodes)
                {
                    if (topics == "")
                    {
                        topics = topics + node.Value;
                        selectedpath = selectedpath + node.ValuePath;
                    }
                    else
                    {
                        topics = topics + "/" + node.Value;
                        selectedpath = selectedpath + "=" + node.ValuePath;
                    }
                }
                if (topics == "")
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('There is nothing to save')", true);
                    return;
                }
                string order_day = string.Empty;
                string query = "select schorder from PeriodAttndSchedule where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString();
                string order = da.GetFunction(query);
                if (order == "")
                    return;
                string curday = Session["sch_date"].ToString();
                DateTime day_lesson = Convert.ToDateTime(curday);
                if (order != "0")
                    order_day = day_lesson.ToString("ddd");
                else
                {
                    order_day = find_day_order();
                    if (order_day == "")
                        return;
                }
                if (order_day == "mon")
                    sch_order = "1";
                else if (order_day == "tue")
                    sch_order = "2";
                else if (order_day == "wed")
                    sch_order = "3";
                else if (order_day == "thu")
                    sch_order = "4";
                else if (order_day == "fri")
                    sch_order = "5";
                else if (order_day == "sat")
                    sch_order = "6";
                else if (order_day == "sun")
                    sch_order = "7";
                if (order_day == "Mon")
                    sch_order = "1";
                else if (order_day == "Tue")
                    sch_order = "2";
                else if (order_day == "Wed")
                    sch_order = "3";
                else if (order_day == "Thu")
                    sch_order = "4";
                else if (order_day == "Fri")
                    sch_order = "5";
                else if (order_day == "Sat")
                    sch_order = "6";
                else if (order_day == "Sun")
                    sch_order = "7";
                string subj_no = (string)Session["sub_no"].ToString();
                string hour_hr = (string)Session["hr"].ToString();

                int a = 0;
                string strdailtquery = "select * from dailyStaffEntry where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "'";
                DataSet dsdaily = da.select_method(strdailtquery, hat, "Text");
                if (dsdaily.Tables.Count > 0 && dsdaily.Tables[0].Rows.Count > 0)
                {
                    isInsert = false;
                    strdailtquery = "update dailyStaffEntry set sch_order=" + sch_order + " where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "'";
                    a = da.update_method_wo_parameter(strdailtquery, "Text");
                }
                else
                {
                    isInsert = true;
                    string sec = (string)Session["sections"].ToString();
                    if (sec != "")
                        strdailtquery = "insert into dailyStaffEntry (degree_code,semester,batch_year,sch_date,sch_order,sections) values(" + Session["deg_code"].ToString() + "," + Session["semester"].ToString() + "," + Session["batch_year"].ToString() + ",'" + Session["sch_date"].ToString() + "'," + sch_order + ",'" + Session["sections"].ToString() + "')";
                    else
                        strdailtquery = "insert into dailyStaffEntry (degree_code,semester,batch_year,sch_date,sch_order) values(" + Session["deg_code"].ToString() + "," + Session["semester"].ToString() + "," + Session["batch_year"].ToString() + ",'" + Session["sch_date"].ToString() + "'," + sch_order + ")";
                    a = da.update_method_wo_parameter(strdailtquery, "Text");
                }
                string lp_query = "select lp_code from dailyStaffEntry where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "' and sch_order=" + sch_order;
                string lp_code = da.GetFunction(lp_query);
                if (lp_code != "")
                {
                    strdailtquery = "select * from dailyEntdet where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "";
                    dsdaily.Dispose();
                    dsdaily.Reset();
                    dsdaily = da.select_method(strdailtquery, hat, "Text");
                    if (dsdaily.Tables.Count > 0 && dsdaily.Tables[0].Rows.Count > 0)
                    {
                        string strgettopic = da.GetFunction("select topics from dailyEntdet where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "");
                        if (strgettopic != "" && strgettopic != null && strgettopic != "0")
                        {
                            topics = topics + "/" + strgettopic;
                        }
                        //cmd.CommandText = "update dailyEntdet set topics='" + topics + "' where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "";                  
                        strdailtquery = "update dailyEntdet set topics='" + topics + "' where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "";
                        a = da.insert_method(strdailtquery, hat, "Text");
                    }
                    else
                    {
                        //cmd.CommandText = "insert into dailyEntdet (lp_code,subject_no,topics,hr,staff_code) values(" + lp_code + "," + subj_no + ",'" + topics + "'," + hour_hr + ",'" + staff_code + "')";
                        strdailtquery = "insert into dailyEntdet (lp_code,subject_no,topics,hr,staff_code) values(" + lp_code + "," + subj_no + ",'" + topics + "'," + hour_hr + ",'" + staff_code + "')";
                        a = da.update_method_wo_parameter(strdailtquery, "Text");
                    }
                }
                int.TryParse(Convert.ToString(Session["Row"]), out ar);
                int.TryParse(Convert.ToString(Session["Col"]), out ac);
                //ar = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
                //ac = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
                if (ar != -1)
                {
                    string col = "lblPeriod_" + ac;
                    string spread_text = (gridTimeTable.Rows[ar].FindControl(col) as Label).Text;
                    //string spread_text = FpSpread1.Sheets[0].Cells[ar, ac].Text;
                    getcelltag = ddlselectmanysub.SelectedValue.ToString();
                    string getdate = (gridTimeTable.Rows[ar].FindControl("lblDate") as Label).Text;
                    string[] splitvalue = getcelltag.Split(new char[] { '-' });
                    if (splitvalue.GetUpperBound(0) > 0)
                    {
                        string degree_code = splitvalue[0].ToString();
                        string semester = splitvalue[1].ToString();
                        string subject_no = splitvalue[2].ToString();
                        string batch_year = string.Empty;
                        if (splitvalue.GetUpperBound(0) == 7)
                        {
                            batch_year = splitvalue[4].ToString();
                        }
                        else
                        {
                            batch_year = splitvalue[3].ToString();
                        }
                        filltree();
                    }
                    ////Set Color
                    //**************added By Srinath 29Jan2015
                    string strstaffselector = string.Empty;
                    string checkalter = string.Empty;


                    string spread_text1 = Convert.ToString((gridTimeTable.Rows[ar].FindControl("lnkPeriod_" + ac) as LinkButton).Text);
                    string text_val = Convert.ToString((gridTimeTable.Rows[ar].FindControl("lnkPeriod_" + ac) as LinkButton).Text);

                    if (spread_text1 != "" && spread_text1 != "Sunday Holiday" && spread_text1 != "Saturday Holiday")
                    {

                        getcelltag = Convert.ToString((gridTimeTable.Rows[ar].FindControl("lblPeriod_" + ac) as Label).Text);
                        string avoidholiday = string.Empty;
                        string avoidholidaytext = string.Empty;
                        string[] spiltgetceltag = getcelltag.Split('*');
                        string[] spilttext = text_val.Split('*');
                        for (int k = 0; k <= spiltgetceltag.GetUpperBound(0); k++)
                        {
                            string[] spitvalue = spiltgetceltag[k].Split('-');
                            if (spitvalue[0].ToLower().Trim() == "selected day is holiday")
                            {
                            }
                            else
                            {
                                if (avoidholiday == "")
                                {
                                    avoidholiday = spiltgetceltag[k].ToString();
                                    avoidholidaytext = spilttext[k].ToString();
                                }
                                else
                                {
                                    avoidholiday = avoidholiday + '*' + spiltgetceltag[k].ToString();
                                    avoidholidaytext = avoidholidaytext + '*' + spilttext[k].ToString();
                                }
                            }
                        }
                        getcelltag = avoidholiday;
                        text_val = avoidholidaytext;
                    }

                    hr = (GridView1.Rows[1].FindControl("lblHR") as Label).Text;

                    //hr = FpSpread1.Sheets[0].ColumnHeader.Cells[0, ac].Tag.ToString();
                    string temp_date = (gridTimeTable.Rows[ar].FindControl("lblDateDisp") as Label).Text;
                    //string temp_date = FpSpread1.Sheets[0].RowHeader.Cells[ar, 0].Text;
                    //string[] spitdate = temp_date.Split('(');
                    string str_Date = temp_date.ToString();
                    //string str_Date = FpSpread2.Sheets[0].ColumnHeader.Cells[0, att_col].Text;
                    string[] split = str_Date.Split(new Char[] { '-' });
                    string str_day = (Convert.ToInt16(split[0].ToString())).ToString();
                    string Atmonth = (Convert.ToInt16(split[1].ToString())).ToString();
                    string Atyear = split[2].ToString();
                    int strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                    string str_hour = Convert.ToString(hr);
                    string dcolumn = "d" + str_day + "d" + str_hour;
                    string batch = "", section = "", strquery = string.Empty;
                    DataSet dsattendance = new DataSet();
                    DateTime date = Convert.ToDateTime(split[1].ToString() + '-' + split[0].ToString() + '-' + split[2].ToString());
                    string day = date.ToString("ddd");
                    string[] spilttext2 = getcelltag.Split('*');
                    string linkVal = "lnkPeriod_" + ac;
                    LinkButton lnkbtn = (gridTimeTable.Rows[ar].FindControl(linkVal) as LinkButton);
                    for (int j = 0; j <= spilttext2.GetUpperBound(0); j++)
                    {
                        bool colorflag = false;
                        if (j > 0)
                        {
                            if (lnkbtn.ForeColor == Color.Blue)
                            {
                                colorflag = true;
                            }
                            else
                            {
                                colorflag = false;
                            }
                        }
                        if (colorflag == false)
                        {
                            dailyentryflag = false;
                            attendanceentryflag = false;
                            string[] split_tag_val = spilttext2[j].Split('-');
                            string check_lab = string.Empty;
                            if (split_tag_val.GetUpperBound(0) >= 7)
                            {
                                batch = split_tag_val[4].ToString();
                                degree_code = split_tag_val[0].ToString();
                                semester = split_tag_val[1].ToString();
                                subject_no = split_tag_val[2].ToString();
                                //section = "and Registration.Sections='" + split_tag_val[3].ToString() + "'";
                                section = split_tag_val[3].ToString();
                                checkalter = split_tag_val[7].ToString();
                                check_lab = split_tag_val[5].ToString();
                            }
                            else
                            {
                                batch = split_tag_val[3].ToString();
                                degree_code = split_tag_val[0].ToString();
                                semester = split_tag_val[1].ToString();
                                subject_no = split_tag_val[2].ToString();
                                section = string.Empty;
                                checkalter = split_tag_val[6].ToString();
                                check_lab = split_tag_val[4].ToString();
                            }
                            Session["StaffSelector"] = "0";
                            strstaffselector = string.Empty;   //Session["collegecode"].ToString()
                            string staffbatchyear = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString() + "'");
                            string[] splitminimumabsentsms = staffbatchyear.Split('-');
                            if (splitminimumabsentsms.Length == 2)
                            {
                                int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                                if (splitminimumabsentsms[0].ToString() == "1")
                                {
                                    if (Convert.ToInt32(batch) >= batchyearsetting)
                                    {
                                        Session["StaffSelector"] = "1";
                                    }
                                }
                            }
                            if (Session["StaffSelector"].ToString() == "1")
                            {
                                strstaffselector = " and s.staffcode like '%" + Session["Staff_Code"].ToString() + "%'";
                            }
                            string labsection = string.Empty;
                            if (section.Trim() == "" && section != null && section.Trim() != "-1")
                            {
                                labsection = " and isnull(sections,'')='" + section + "'";
                            }
                            //  string check_lab = da.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subject_no + "'");
                            if (check_lab == "L" || check_lab.Trim().ToLower() == "l")
                            {
                                strquery = "  select p.schOrder,p.nodays,Convert(nvarchar(15),s.start_date,23) as start,s.starting_dayorder from PeriodAttndSchedule p, seminfo s where s.degree_code=p.degree_code and s.semester=p.semester and s.degree_code=" + degree_code + " and s.semester=" + semester + " AND s.batch_year=" + batch + "";
                                Day_Order = "0";
                                dsattendance = da.select_method(strquery, hat, "Text");
                                if (dsattendance.Tables.Count > 0 && dsattendance.Tables[0].Rows.Count > 0)
                                {
                                    Day_Order = dsattendance.Tables[0].Rows[0]["schOrder"].ToString();
                                    noofdays = dsattendance.Tables[0].Rows[0]["nodays"].ToString();
                                    start_datesem = dsattendance.Tables[0].Rows[0]["start"].ToString();
                                    start_dayorder = dsattendance.Tables[0].Rows[0]["starting_dayorder"].ToString();
                                }
                                //Week / Day order
                                if (Day_Order == "1")
                                {
                                    day = date.ToString("ddd");
                                }
                                else
                                {
                                    day = da.findday(date.ToString(), degree_code, semester, batch, start_datesem.ToString(), noofdays.ToString(), start_dayorder);//Modifeied By Srianth add comman Daccess 5/9/2014
                                }
                                if (checkalter.ToLower().Trim() == "alter")
                                {
                                    hat.Clear();
                                    hat.Add("batch_year", batch);
                                    hat.Add("degree_code", degree_code);
                                    hat.Add("sem", semester);
                                    hat.Add("sections", section);
                                    hat.Add("month_year", strdate);
                                    hat.Add("date", date);
                                    hat.Add("subject_no", subject_no);
                                    hat.Add("day", day);
                                    hat.Add("hour", str_hour);
                                    ds.Reset();
                                    ds.Dispose();
                                    ds = da.select_method("sp_stu_atten_month_check_lab_alter", hat, "sp");
                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        Att_strqueryst = ds.Tables[0].Rows[0]["stucount"].ToString();
                                        if (int.Parse(Att_strqueryst) > 0)
                                        {
                                            hat.Clear();
                                            hat.Add("columnname", dcolumn);
                                            hat.Add("batch_year", batch);
                                            hat.Add("degree_code", degree_code);
                                            hat.Add("sem", semester);
                                            hat.Add("sections", section);
                                            hat.Add("month_year", strdate);
                                            hat.Add("date", date);
                                            hat.Add("subject_no", subject_no);
                                            hat.Add("day", day);
                                            hat.Add("hour", str_hour);
                                            ds.Reset();
                                            ds.Dispose();
                                            // ds = da.select_method("sp_stu_atten_day_check_lab_alter", hat, "sp");
                                            string strgetatt = "select count(distinct r.Roll_No) as stucount from registration r,attendance a,subjectchooser_new s where degree_code='" + degree_code + "' and current_semester='" + semester + "' and batch_year='" + batch + "' and cc=0  " + dis + " " + deba + " and month_year=" + strdate + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and";
                                            strgetatt = strgetatt + " r.current_semester=s.semester and subject_no='" + subject_no + "' " + labsection + " and(" + dcolumn + " is not null and " + dcolumn + "<>'0' and " + dcolumn + "<>'') and fromdate='" + date + "' and batch    in(select stu_batch from laballoc_new where subject_no='" + subject_no + "'  and batch_year='" + batch + "'  and hour_value='" + str_hour + "'  and    degree_code='" + degree_code + "' ";
                                            strgetatt = strgetatt + " and day_value='" + day + "' and semester='" + semester + "' " + labsection + " and fdate='" + date + "') and adm_date<='" + date + "' " + strstaffselector + "";
                                            ds = da.select_method_wo_parameter(strgetatt, "Text");
                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                            {
                                                if (Att_strqueryst == ds.Tables[0].Rows[0]["stucount"].ToString())
                                                {
                                                    Att_strqueryst = "0";
                                                }
                                                else
                                                {
                                                    Att_strqueryst = "1";
                                                }
                                            }
                                            else
                                            {
                                                Att_strqueryst = "1";
                                            }
                                        }
                                        else
                                        {
                                            Att_strqueryst = "1";
                                        }
                                    }
                                    else
                                    {
                                        Att_strqueryst = "1";
                                    }
                                }
                                else
                                {
                                    string timetable = da.GetFunction("select top 1 TTName,  FromDate from  Semester_Schedule where degree_code='" + degree_code + "' and semester='" + semester + "' and batch_year='" + batch + "' " + labsection + " and FromDate<='" + date.ToString("MM/dd/yyyy") + "'  order by FromDate Desc");
                                    hat.Clear();

                                    string strstt = "select count(distinct r.Roll_No) as stucount from registration r,subjectchooser s where degree_code='" + degree_code + "' and ";
                                    strstt = strstt + " current_semester='" + semester + "' and batch_year='" + batch.ToString() + "' and cc=0 " + dis + " " + deba + " and r.roll_no=s.roll_no ";
                                    strstt = strstt + " and r.current_semester=s.semester and subject_no='" + subject_no + "' " + labsection + " and batch in(select stu_batch from ";
                                    strstt = strstt + " laballoc where subject_no='" + subject_no + "'  and batch_year='" + batch.ToString() + "'  and hour_value='" + str_hour + "' and degree_code='" + degree_code + "' ";
                                    strstt = strstt + " and day_value='" + day + "' and semester='" + semester + "' " + labsection + " and Timetablename='" + timetable + "') and adm_date<='" + date.ToString("MM/dd/yyyy") + "'  " + strstaffselector + "";
                                    ds.Reset();
                                    ds.Dispose();
                                    // ds = da.select_method("sp_stu_atten_month_check_lab", hat, "sp");
                                    ds = da.select_method_wo_parameter(strstt, "Text");
                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        Att_strqueryst = ds.Tables[0].Rows[0]["stucount"].ToString();
                                        if (int.Parse(Att_strqueryst) > 0)
                                        {
                                            hat.Clear();
                                            hat.Add("columnname", dcolumn);
                                            hat.Add("batch_year", batch);
                                            hat.Add("degree_code", degree_code);
                                            hat.Add("sem", semester);
                                            hat.Add("sections", section);
                                            hat.Add("month_year", strdate);
                                            hat.Add("date", date);
                                            hat.Add("subject_no", subject_no);
                                            hat.Add("day", day);
                                            hat.Add("hour", str_hour);
                                            hat.Add("ttmane", timetable);
                                            ds.Reset();
                                            ds.Dispose();
                                            //  ds = da.select_method("sp_stu_atten_day_check_lab", hat, "sp");
                                            string strgetatt = "select count( r.Roll_No) as stucount from registration r,attendance a,subjectchooser s where degree_code='" + degree_code + "' and current_semester='" + semester + "' and batch_year='" + batch + "' and cc=0 " + dis + " " + deba + " and month_year=" + strdate + "";
                                            strgetatt = strgetatt + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and r.current_semester=s.semester and subject_no='" + subject_no + "' " + labsection + " and(" + dcolumn + " is not null and " + dcolumn + "<>'0' and " + dcolumn + "<>'') and batch in(select stu_batch from laballoc ";
                                            strgetatt = strgetatt + " where subject_no='" + subject_no + "' and Timetablename='" + timetable + "' and batch_year='" + batch + "'  and hour_value='" + str_hour + "'  and    degree_code='" + degree_code + "' and day_value='" + day + "' and semester='" + semester + "' " + labsection + ") and adm_date<='" + date + "' " + strstaffselector + "";
                                            ds = da.select_method_wo_parameter(strgetatt, "Text");
                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                            {
                                                if (Att_strqueryst == ds.Tables[0].Rows[0]["stucount"].ToString())
                                                {
                                                    Att_strqueryst = "0";
                                                }
                                                else
                                                {
                                                    Att_strqueryst = "1";
                                                }
                                            }
                                            else
                                            {
                                                Att_strqueryst = "1";
                                            }
                                        }
                                        else
                                        {
                                            Att_strqueryst = "1";
                                        }
                                    }
                                    else
                                    {
                                        Att_strqueryst = "1";
                                    }
                                }
                            }
                            else
                            {

                                ds.Reset();
                                ds.Dispose();
                                //ds = da.select_method("sp_stu_atten_month_check", hat, "sp");
                                string strgetatt1 = "select count(distinct r.Roll_No) as stucount from registration r,subjectchooser s where  r.roll_no=s.roll_no and ";
                                strgetatt1 = strgetatt1 + " r.current_semester=s.semester and batch_year='" + batch + "' and degree_code='" + degree_code + "'  and current_semester='" + semester + "' " + labsection + " ";
                                strgetatt1 = strgetatt1 + "  and subject_no='" + subject_no + "'  and adm_date<='" + date + "' and cc=0 " + dis + " " + deba + "  " + strstaffselector + "";
                                ds = da.select_method_wo_parameter(strgetatt1, "Text");
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    Att_strqueryst = ds.Tables[0].Rows[0]["stucount"].ToString();
                                    if (int.Parse(Att_strqueryst) > 0)
                                    {
                                        hat.Clear();
                                        hat.Add("columnname", dcolumn);
                                        hat.Add("batch_year", batch);
                                        hat.Add("degree_code", degree_code);
                                        hat.Add("sem", semester);
                                        hat.Add("sections", section);
                                        hat.Add("month_year", strdate);
                                        hat.Add("date", date);
                                        hat.Add("subject_no", subject_no);
                                        ds.Reset();
                                        ds.Dispose();
                                        //ds = da.select_method("sp_stu_atten_day_check", hat, "sp");
                                        string strgetatt = "select count(registration.roll_no) as stucount  from registration,attendance,subjectchooser s where degree_code='" + degree_code + "' and current_semester='" + semester + "' and batch_year='" + batch + "' and cc=0 " + dis + " " + deba + " and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and  registration.roll_no=s.roll_no ";
                                        strgetatt = strgetatt + " and registration.current_semester=s.semester and subject_no='" + subject_no + "' " + labsection + "";
                                        strgetatt = strgetatt + " and (" + dcolumn + " is not null and " + dcolumn + "<>'0' and " + dcolumn + "<>'') and adm_date<='" + date + "' " + strstaffselector + " ";
                                        ds = da.select_method_wo_parameter(strgetatt, "Text");
                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            if (Att_strqueryst == ds.Tables[0].Rows[0]["stucount"].ToString())
                                            {
                                                Att_strqueryst = "0";
                                            }
                                            else
                                            {
                                                Att_strqueryst = "1";
                                            }
                                        }
                                        else
                                        {
                                            Att_strqueryst = "1";
                                        }
                                    }
                                    else
                                    {
                                        Att_strqueryst = "1";
                                    }
                                }
                                else
                                {
                                    Att_strqueryst = "1";
                                }
                            }
                            if (int.Parse(Att_strqueryst) > 0)
                            {
                                //FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.Blue;
                                attendanceentryflag = false;
                            }
                            else
                            {
                                attendanceentryflag = true;
                                //FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.ForestGreen;
                            }
                            if (section.Trim() == "" || section == null || section.Trim() == "-1")
                            {
                                section = string.Empty;
                            }
                            else
                            {
                                section = " and isnull(Sections,'')='" + section + "'";
                            }

                            strquerytext = "select de.lp_code from dailyentdet de,dailystaffentry ds where de.lp_code=ds.lp_code  and batch_year=" + batch + " and degree_code='" + degree_code + "' and semester=" + semester + " " + section + " and subject_no='" + subject_no + "' and  staff_code='" + staff_code + "' and sch_date='" + date + "' and hr=" + hr + "";
                            ds.Reset();
                            ds.Dispose();
                            ds = da.select_method(strquerytext, hat, "Text");
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                dailyentryflag = true;
                            }



                            if (dailyentryflag == false && attendanceentryflag == false)
                            {
                                lnkbtn.ForeColor = Color.Blue;
                                j = spilttext2.GetUpperBound(0) + 1;
                            }
                            else if (dailyentryflag == true && attendanceentryflag == false)
                            {
                                if (lnkbtn.ForeColor == Color.DarkOrchid)
                                {
                                    lnkbtn.ForeColor = Color.Blue;
                                }
                                else
                                {
                                    lnkbtn.ForeColor = Color.DarkTurquoise;
                                }
                            }
                            else if (dailyentryflag == false && attendanceentryflag == true)
                            {
                                if (lnkbtn.ForeColor == Color.DarkTurquoise)
                                {
                                    lnkbtn.ForeColor = Color.Blue;
                                }
                                else
                                {
                                    lnkbtn.ForeColor = Color.DarkOrchid;
                                }
                            }
                            else
                            {
                                if (j == 0)
                                {
                                    lnkbtn.ForeColor = Color.ForestGreen;
                                }
                                else
                                {
                                    if (lnkbtn.ForeColor == Color.ForestGreen)
                                    {
                                        lnkbtn.ForeColor = Color.ForestGreen;
                                    }
                                }
                            }
                        }
                    }
                    string ctsname = "Save the Daily Entry Information";
                    if (!isInsert)
                        ctsname = "Update the Daily Entry Information";
                    string entrycode = Session["Entry_Code"].ToString();//saranya
                    string PageName = "Student Attendance";

                    string TimeOfAttendance = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                    string DateOfAttendence = DateTime.Now.ToString("MM/dd/yyy");
                    da.insertUserActionLog(entrycode, batch, degree_code, semester, sections, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 1);//saranya
                }
            }
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);

        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void Buttonexitlesson_Click(object sender, EventArgs e)
    {
        // Response.Redirect("Default2.aspx");
    }
    protected void ddlselectmanysub_SelectedIndexChanged(object sender, EventArgs e)//----13/6/12 PRABHA
    {
        try
        {
            storepath = string.Empty;
            selectedpath = string.Empty;
            //FpSpread3.Sheets[0].RowCount = 0;
            int.TryParse(Convert.ToString(Session["Row"]), out ar);
            int.TryParse(Convert.ToString(Session["Col"]), out ac);
            if (!string.IsNullOrEmpty(ddlselectmanysub.SelectedItem.Text))
            {
                string subName = ddlselectmanysub.SelectedItem.Text.Split(new char[] { '-' })[0];
                lblsubtext.Text = subName;
            }
            string[] splitvalue = ddlselectmanysub.SelectedValue.ToString().Split(new char[] { '-' });
            //Added by srinath 30/8/2013

            //string getdayorder = Convert.ToString(FpSpread1.Sheets[0].Cells[ar, ac].Note);
            string getdayorder = (gridTimeTable.Rows[ar].FindControl("lblTT_" + ac) as Label).Text;
            sel_date1 = (gridTimeTable.Rows[ar].FindControl("lblDateDisp") as Label).Text;

            if (Convert.ToString(getdayorder).Trim() != "")
            {
                string[] dayorderval = getdayorder.Split(new Char[] { '-' });
                if (Convert.ToString(dayorderval[0]).Trim() != "0")
                {
                    lbldayorder.Visible = true;
                    lbldayorder.Text = "Day Order " + Convert.ToString(dayorderval[0]).Trim();
                }
                if (dayorderval.Length > 2)
                {
                    Day_Var = Convert.ToString(dayorderval[2]).Trim();
                }
                else
                {
                    Day_Var = Convert.ToString(dayorderval[1]).Trim();
                }
            }
            if (splitvalue.GetUpperBound(0) > 0)
            {
                filltree();
                if (splitvalue.GetUpperBound(0) == 7)
                {
                    string degree_code = splitvalue[0].ToString();
                    string semester = splitvalue[1].ToString();
                    string subject_no = splitvalue[2].ToString();
                    string batch_year = splitvalue[4].ToString();
                    retrievespreadattendancequestion(batch_year, degree_code, semester, subject_no, sel_date1);
                    retrivespreadfornotes(batch_year, degree_code, semester, subject_no, sel_date1);
                    sprdretrivedate();
                }
                else
                {
                    string degree_code = splitvalue[0].ToString();
                    string semester = splitvalue[1].ToString();
                    string subject_no = splitvalue[2].ToString();
                    string batch_year = splitvalue[3].ToString();
                    retrievespreadattendancequestion(batch_year, degree_code, semester, subject_no, sel_date1);
                    retrivespreadfornotes(batch_year, degree_code, semester, subject_no, sel_date1);
                    sprdretrivedate();
                }

                string[] sel_date_split = sel_date1.Split(new Char[] { '-' });
                getdate_new = sel_date_split[2] + "-" + sel_date_split[1] + "-" + sel_date_split[0];
                if (sel_date_split[0].Length == 1)
                {
                    sel_date_split[0] = "0" + sel_date_split[0];
                }
                if (sel_date_split[1].Length == 1)
                {
                    sel_date_split[1] = "0" + sel_date_split[1];
                }
                sel_date1 = sel_date_split[0] + "-" + sel_date_split[1] + "-" + sel_date_split[2];
                sel_date = sel_date_split[1] + "-" + sel_date_split[0] + "-" + sel_date_split[2];
                getdate = sel_date_split[2] + "-" + sel_date_split[1] + "-" + sel_date_split[0];
                //added  by aruna
                if (ddlselectmanysub.Items.Count >= 3)
                {
                    if (ddlselectmanysub.SelectedItem.Text.ToString().Trim() != "")
                    {
                        singlesubject = true;
                        singlesubjectno = Convert.ToString(ddlselectmanysub.SelectedValue);
                        load_attnd_spread();//Added by srinath 13/8/2013
                        mark_attendance();
                        load_presen_absent_count();
                    }
                    else
                    {
                        singlesubject = false;
                        load_attnd_spread();//Added by srinath 13/8/2013
                        mark_attendance();
                        load_presen_absent_count();
                    }
                }
            }
            else //added  by aruna
            {
                if (ddlselectmanysub.Items.Count >= 3)
                {
                    singlesubject = false;
                    load_attnd_spread();//Added by srinath 13/8/2013
                    mark_attendance();
                    load_presen_absent_count();
                }
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    public void savelessionalter()
    {
        try
        {
            bool isInsert = false;
            string topics = string.Empty;
            string sch_order = string.Empty;
            foreach (TreeNode node in tvalterlession.CheckedNodes)
            {
                if (topics == "")
                {
                    topics = topics + node.Value;
                    selectedpath = selectedpath + node.ValuePath;
                }
                else
                {
                    topics = topics + "/" + node.Value;
                    selectedpath = selectedpath + "=" + node.ValuePath;
                }
            }
            if (topics == "")
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('There is nothing to save')", true);
                return;
            }
            string order_day = string.Empty;
            string query = "select schorder from PeriodAttndSchedule where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString();
            string order = da.GetFunction(query);
            if (order == "")
                return;
            string curday = Session["sch_date"].ToString();
            DateTime day_lesson = Convert.ToDateTime(curday);
            if (order != "0")
                order_day = day_lesson.ToString("ddd");
            else
            {
                order_day = find_day_order();
                if (order_day == "")
                    return;
            }
            if (order_day == "mon")
                sch_order = "1";
            else if (order_day == "tue")
                sch_order = "2";
            else if (order_day == "wed")
                sch_order = "3";
            else if (order_day == "thu")
                sch_order = "4";
            else if (order_day == "fri")
                sch_order = "5";
            else if (order_day == "sat")
                sch_order = "6";
            else if (order_day == "sun")
                sch_order = "7";
            if (order_day == "Mon")
                sch_order = "1";
            else if (order_day == "Tue")
                sch_order = "2";
            else if (order_day == "Wed")
                sch_order = "3";
            else if (order_day == "Thu")
                sch_order = "4";
            else if (order_day == "Fri")
                sch_order = "5";
            else if (order_day == "Sat")
                sch_order = "6";
            else if (order_day == "Sun")
                sch_order = "7";
            string subj_no = (string)Session["sub_no"].ToString();
            string hour_hr = (string)Session["hr"].ToString();

            int a = 0;
            string strdailtquery = "select * from dailyStaffEntry where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "'";
            DataSet dsdaily = da.select_method(strdailtquery, hat, "Text");
            if (dsdaily.Tables.Count > 0 && dsdaily.Tables[0].Rows.Count > 0)
            {
                isInsert = false;
                strdailtquery = "update dailyStaffEntry set sch_order=" + sch_order + " where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "'";
                a = da.update_method_wo_parameter(strdailtquery, "Text");
            }
            else
            {
                isInsert = true;
                string sec = (string)Session["sections"].ToString();
                if (sec != "")
                    strdailtquery = "insert into dailyStaffEntry (degree_code,semester,batch_year,sch_date,sch_order,sections) values(" + Session["deg_code"].ToString() + "," + Session["semester"].ToString() + "," + Session["batch_year"].ToString() + ",'" + Session["sch_date"].ToString() + "'," + sch_order + ",'" + Session["sections"].ToString() + "')";
                else
                    strdailtquery = "insert into dailyStaffEntry (degree_code,semester,batch_year,sch_date,sch_order) values(" + Session["deg_code"].ToString() + "," + Session["semester"].ToString() + "," + Session["batch_year"].ToString() + ",'" + Session["sch_date"].ToString() + "'," + sch_order + ")";
                a = da.update_method_wo_parameter(strdailtquery, "Text");
            }
            string lp_query = "select lp_code from dailyStaffEntry where degree_code=" + Session["deg_code"].ToString() + " and semester=" + Session["semester"].ToString() + Session["str_section"].ToString() + "  and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + Session["sch_date"].ToString() + "' and sch_order=" + sch_order;
            string lp_code = da.GetFunction(lp_query);
            if (lp_code != "")
            {
                strdailtquery = "select * from dailyEntdet where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "";
                dsdaily.Dispose();
                dsdaily.Reset();
                dsdaily = da.select_method(strdailtquery, hat, "Text");
                if (dsdaily.Tables.Count > 0 && dsdaily.Tables[0].Rows.Count > 0)
                {
                    string strgettopic = da.GetFunction("select topics from dailyEntdet where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "");
                    if (strgettopic != "" && strgettopic != null && strgettopic != "0")
                    {
                        topics = topics + "/" + strgettopic;
                    }
                    //cmd.CommandText = "update dailyEntdet set topics='" + topics + "' where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "";                  
                    strdailtquery = "update dailyEntdet set topics='" + topics + "' where lp_code=" + lp_code + " and subject_no=" + Session["sub_no"].ToString() + "  and staff_code='" + staff_code + "' and hr=" + Session["hr"].ToString() + "";
                    a = da.insert_method(strdailtquery, hat, "Text");
                }
                else
                {
                    //cmd.CommandText = "insert into dailyEntdet (lp_code,subject_no,topics,hr,staff_code) values(" + lp_code + "," + subj_no + ",'" + topics + "'," + hour_hr + ",'" + staff_code + "')";
                    strdailtquery = "insert into dailyEntdet (lp_code,subject_no,topics,hr,staff_code) values(" + lp_code + "," + subj_no + ",'" + topics + "'," + hour_hr + ",'" + staff_code + "')";
                    a = da.update_method_wo_parameter(strdailtquery, "Text");
                }
            }

            //ar = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveRow.ToString());
            //ac = Convert.ToInt32(FpSpread1.ActiveSheetView.ActiveColumn.ToString());
            int.TryParse(Convert.ToString(Session["Row"]), out ar);
            int.TryParse(Convert.ToString(Session["Col"]), out ac);


            if (ar != -1)
            {
                //string spread_text = FpSpread1.Sheets[0].Cells[ar, ac].Text;
                //  getcelltag = FpSpread1.Sheets[0].GetTag(ar, ac).ToString();
                getcelltag = ddlselectmanysub.SelectedValue.ToString();

                //string getdate = FpSpread1.Sheets[0].Cells[ar, 0].Text;
                string getdate = (gridTimeTable.Rows[ar].FindControl("lblDate") as Label).Text;
                string[] splitvalue = getcelltag.Split(new char[] { '-' });
                if (splitvalue.GetUpperBound(0) > 0)
                {
                    string degree_code = splitvalue[0].ToString();
                    string semester = splitvalue[1].ToString();
                    string subject_no = splitvalue[2].ToString();
                    string batch_year = string.Empty;
                    if (splitvalue.GetUpperBound(0) == 7)
                    {
                        batch_year = splitvalue[4].ToString();
                    }
                    else
                    {
                        batch_year = splitvalue[3].ToString();
                    }
                    string ctsname = "Save the Lession Alter";
                    if (!isInsert)
                        ctsname = "Update the Lession Alter";
                    string entrycode = Session["Entry_Code"].ToString();//saranya
                    string PageName = "Student Attendance";

                    string TimeOfAttendance = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                    string DateOfAttendence = DateTime.Now.ToString("MM/dd/yyy");
                    da.insertUserActionLog(entrycode, batch_year, degree_code, semester, Convert.ToString(Session["sections"]), TimeOfAttendance, DateOfAttendence, PageName, ctsname, 1);//saranya
                }
                filltree();
                loadalternode();
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    public void filltree()
    {
        try
        {
            tvcomplete.Nodes.Clear();
            tvyet.Nodes.Clear();
            string sqlstr = string.Empty;
            int actrow1;
            int actcol1;
            string getcelltag;
            // string str =string.Empty;
            string degree_code = string.Empty;
            string semester = string.Empty;
            string subject_no = string.Empty;
            string sections = string.Empty;
            string strsec = string.Empty;
            string topics = string.Empty;
            int hr = 0;
            string sch_date = string.Empty;
            string[] sp_date;
            string sch_dt = string.Empty;
            ArrayList topics_com = new ArrayList();
            ArrayList topics_yet = new ArrayList();
            ArrayList topics_today = new ArrayList();
            string topics_plan = string.Empty;
            string topics_Entry = string.Empty;
            string topics_Entryall = string.Empty;


            //actrow1 = FpSpread1.ActiveSheetView.ActiveRow;
            //actcol1 = FpSpread1.ActiveSheetView.ActiveColumn;
            int.TryParse(Convert.ToString(Session["Row"]), out actrow1);
            int.TryParse(Convert.ToString(Session["Col"]), out actcol1);


            string sub_name = ddlselectmanysub.SelectedItem.ToString();

            //sch_dt = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text;
            //string getdate = FpSpread1.Sheets[0].Cells[ar, 0].Text;
            //string getdate = (gridTimeTable.Rows[ar].FindControl("lblDate") as Label).Text;
            string spdatesp = (gridTimeTable.Rows[ar].FindControl("lblDateDisp") as Label).Text;

            sch_dt = spdatesp.ToString();
            DataSet dsipcode = new DataSet();
            DataSet dstopic = new DataSet();
            string stripcode = string.Empty;
            string strtopic = string.Empty;
            if (sch_dt != "")
            {
                sp_date = sch_dt.Split(new Char[] { '-' });
                sch_date = sp_date[2].ToString() + "-" + sp_date[1].ToString() + "-" + sp_date[0].ToString();
                Session["sch_date"] = sch_date;
            }
            if (sub_name.Trim() == "")
                return;
            else
            {
                Buttonsavelesson.Visible = true;
                Labellvalid.Visible = false;
                Panelcomplete.Visible = true;
                Panelyet.Visible = true;
                string flag = (string)Session["flag"].ToString();
                tvcomplete.Nodes.Clear();
                tvyet.Nodes.Clear();
                Label2.Text = "Lesson Plan Topics";
                string tagval = "lblPeriod_" + ac;

                //getcelltag = FpSpread1.Sheets[0].GetTag(actrow1, actcol1).ToString();
                getcelltag = (gridTimeTable.Rows[ar].FindControl(tagval) as Label).Text;


                string[] spitvalue = getcelltag.Split('*');
                getcelltag = string.Empty;
                for (int l = 0; l <= spitvalue.GetUpperBound(0); l++)
                {
                    string[] spitvalue1 = spitvalue[l].Split('-');
                    if (spitvalue1[0].ToLower().Trim() != "selected day is holiday")
                    {
                        if (getcelltag == "")
                        {
                            getcelltag = spitvalue[l].ToString();
                        }
                        else
                        {
                            getcelltag = getcelltag + spitvalue[l].ToString();
                        }
                    }
                }
                if (ddlselectmanysub.Items.Count > 0)
                {
                    string getddlsub = ddlselectmanysub.SelectedValue.ToString();
                    if (getddlsub.Trim() != "")
                    {
                        string[] sp1 = getddlsub.Split(new Char[] { '-' });
                        if (sp1.GetUpperBound(0) >= 7)
                        {
                            degree_code = sp1[0];
                            semester = sp1[1];
                            subject_no = sp1[2];
                            sections = sp1[3];
                            hr = actcol1;
                        }
                        else
                        {
                            degree_code = sp1[0];
                            semester = sp1[1];
                            subject_no = sp1[2];
                            sections = string.Empty;
                            hr = actcol1;
                        }
                    }
                }
                else
                {
                    if (getcelltag != "")
                    {
                        string[] sp1 = getcelltag.Split(new Char[] { '-' });
                        if (sp1.GetUpperBound(0) >= 7)
                        {
                            degree_code = sp1[0];
                            semester = sp1[1];
                            subject_no = sp1[2];
                            sections = sp1[3];
                            hr = actcol1;
                        }
                        else
                        {
                            degree_code = sp1[0];
                            semester = sp1[1];
                            subject_no = sp1[2];
                            sections = string.Empty;
                            hr = actcol1;
                        }
                    }
                }
                cnode = new TreeNode(sub_name, subject_no);
                ynode = new TreeNode(sub_name, subject_no);
                if (sections.ToString() != "" && sections.ToString() != "-1")
                {
                    strsec = " and sections='" + sections.ToString() + "' ";
                }
                else
                {
                    strsec = string.Empty;
                }

                topics_Entry = string.Empty;
                stripcode = "select topics from dailyStaffEntry d,dailyEntdet de where d.lp_code=de.lp_code and degree_code='" + degree_code + "' and semester= '" + semester + "' " + strsec + " and batch_year='" + Session["batch_year"].ToString() + "' and sch_date<='" + sch_date + "' order by sch_date";
                dsipcode = da.select_method(stripcode, hat, "Text");
                if (dsipcode.Tables.Count > 0 && dsipcode.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsipcode.Tables[0].Rows.Count; i++)
                    {
                        topics = dsipcode.Tables[0].Rows[i]["topics"].ToString();
                        if (topics.Trim() != "")
                        {
                            string[] sptopic = topics.Split('/');
                            for (int stp = 0; stp <= sptopic.GetUpperBound(0); stp++)
                            {
                                if (sptopic[stp].ToString().Trim() != "")
                                {
                                    if (topics_Entry.ToString().Trim() == "")
                                    {
                                        topics_Entry = sptopic[stp].ToString();
                                    }
                                    else
                                    {
                                        topics_Entry = topics_Entry + "," + sptopic[stp].ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                this.tvcomplete.Nodes.Clear();
                HierarchyTrees hierarchyTrees1 = new HierarchyTrees();
                HierarchyTrees.HTree objHTree1 = null;
                sqlstr = string.Empty;
                if (topics_Entry.ToString().Trim() != "")
                {
                    //con.Close();
                    //con.Open();
                    sqlstr = "select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + subject_no + "' ";
                    sqlstr = sqlstr + " and (topic_no  in(select parent_code from sub_unit_details where topic_no in(" + topics_Entry + "))";
                    sqlstr = sqlstr + " or topic_no  in( select parent_code from sub_unit_details where topic_no in(select parent_code from sub_unit_details where topic_no in(" + topics_Entry + ")))";
                    sqlstr = sqlstr + " or topic_no in(" + topics_Entry + ")) order by parent_code,topic_no";
                    dstopic.Dispose();
                    dstopic.Reset();
                    dstopic = da.select_method(sqlstr, hat, "Text");
                    this.tvcomplete.Nodes.Clear();
                    hierarchyTrees1.Clear();
                    if (dstopic.Tables.Count > 0 && dstopic.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dstopic.Tables[0].Rows.Count; i++)
                        {
                            objHTree1 = new HierarchyTrees.HTree();
                            objHTree1.topic_no = int.Parse(dstopic.Tables[0].Rows[i]["Topic_no"].ToString());
                            objHTree1.parent_code = int.Parse(dstopic.Tables[0].Rows[i]["parent_code"].ToString());
                            objHTree1.unit_name = dstopic.Tables[0].Rows[i]["unit_name"].ToString();
                            hierarchyTrees1.Add(objHTree1);
                        }
                        //objHTree1.topic_no = 0;
                        //objHTree1.unit_name = "Others";
                        //objHTree1.parent_code = 0;
                        //hierarchyTrees1.Add(objHTree1);
                    }

                    foreach (HierarchyTrees.HTree hTree in hierarchyTrees1)
                    {
                        HierarchyTrees.HTree parentNode = hierarchyTrees1.Find(delegate(HierarchyTrees.HTree emp) { return emp.topic_no == hTree.parent_code; });
                        if (parentNode != null)
                        {
                            foreach (TreeNode tn in tvcomplete.Nodes)
                            {
                                if (tn.Value == parentNode.topic_no.ToString())
                                {
                                    tn.ChildNodes.Add(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
                                }
                                if (tn.ChildNodes.Count > 0)
                                {
                                    foreach (TreeNode ctn in tn.ChildNodes)
                                    {
                                        RecursiveChild(ctn, parentNode.topic_no.ToString(), hTree);
                                    }
                                }
                            }
                        }
                        else
                        {
                            tvcomplete.Nodes.Add(new TreeNode(hTree.unit_name, hTree.topic_no.ToString()));
                        }
                        //  tvcomplete.ExpandAll();
                    }


                    string sub = "select * from dailyStaffEntry d,dailyEntryother,dailyEntdet de where isnull(othersub,'')<>'' and  degree_code=" + degree_code + " and semester= " + semester + strsec + " and batch_year=" + Session["batch_year"].ToString() + " and othersub=subpk  and college_code=" + Session["collegecode"] + " and d.lp_code=de.lp_code  and sch_date='" + sch_date + "'  and hr='" + hr + "' and  staff_code='" + Session["Staff_Code"].ToString() + "'";
                    dsipcode.Reset();
                    dsipcode = da.select_method(sub, hat, "Text");
                    if (dsipcode.Tables.Count > 0 && dsipcode.Tables[0].Rows.Count > 0)
                    {
                        tvcomplete.Nodes.Add(new TreeNode("others", "0"));
                        for (int i = 0; i < dsipcode.Tables[0].Rows.Count; i++)
                        {
                            //foreach(HierarchyTrees.HTree hTree in hierarchyTrees1)
                            //{
                            //HierarchyTrees.HTree parentNode = hierarchyTrees1.Find(delegate(HierarchyTrees.HTree emp) { return emp.topic_no == hTree.parent_code; });
                            //if (parentNode != null)
                            // {
                            foreach (TreeNode tn in tvcomplete.Nodes)
                            {
                                if (tn.Value == "0")
                                {

                                    {
                                        tn.ChildNodes.Add(new TreeNode(Convert.ToString(dsipcode.Tables[0].Rows[i]["topic_name"]), Convert.ToString(dsipcode.Tables[0].Rows[i]["othersub"])));
                                    }

                                    //}
                                    //}
                                    //if (tn.ChildNodes.Count > 0)
                                    //{
                                    //    foreach (TreeNode ctn in tn.ChildNodes)
                                    //    {
                                    //        RecursiveChild(ctn, "0", hTree);
                                    //    }
                                    //}

                                }
                            }
                        }
                    }
                }



                //End =================================================================================================================================
                //Start Topics Completed ===================================================================================================
                //mysql.Close();
                //mysql.Open();
                topics_Entryall = string.Empty;
                string striplcode = "select lp_code from dailyStaffEntry where degree_code=" + degree_code + " and semester= " + semester + strsec + " and batch_year=" + Session["batch_year"].ToString() + " ";
                dsipcode.Dispose();
                dsipcode.Reset();
                dsipcode = da.select_method(striplcode, hat, "Text");
                if (dsipcode.Tables.Count > 0 && dsipcode.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsipcode.Tables[0].Rows.Count; i++)
                    {

                        Session["lp_code"] = dsipcode.Tables[0].Rows[i]["lp_code"].ToString();
                        strtopic = "select distinct topics from dailyEntdet where lp_code=" + dsipcode.Tables[0].Rows[i]["lp_code"].ToString() + " and subject_no = " + subject_no + " and staff_code='" + staff_code + "' ";
                        dstopic.Dispose();
                        dstopic.Reset();
                        dstopic = da.select_method(strtopic, hat, "Text");
                        if (dstopic.Tables.Count > 0 && dstopic.Tables[0].Rows.Count > 0)
                        {

                            topics = dstopic.Tables[0].Rows[0]["topics"].ToString();
                            if (topics.Contains("/"))
                            {
                                string[] split = topics.Split(new Char[] { '/' });
                                int ln = split.Length;
                                for (int t = 0; t < ln; t++)
                                {
                                    topics_com.Add(split[t].ToString());
                                    if (topics_Entryall.ToString().Trim() == "")
                                    {
                                        topics_Entryall = split[t].ToString();
                                    }
                                    else
                                    {
                                        topics_Entryall = topics_Entryall + "," + split[t].ToString();
                                    }
                                }
                            }
                            else
                            {
                                topics_com.Add(topics.ToString());
                                if (topics_Entryall.ToString().Trim() == "")
                                {
                                    topics_Entryall = topics.ToString();
                                }
                                else
                                {
                                    topics_Entryall = topics_Entryall + "," + topics.ToString();
                                }
                            }
                        }
                        //read_top.Close();
                    }
                    if (topics_Entryall.ToString().Trim() != "")
                    {
                        topics_Entryall = " and topic_no not in(" + topics_Entryall + ")";
                    }
                }
                //==================================================================================================================================
                //Start Lesson Plan Topics Datewise and Hourwise===============================================================================================================
                //mysql.Close();
                //mysql.Open();
                topics_plan = string.Empty;
                stripcode = "select lp_code from lesson_plan where degree_code=" + degree_code + " and semester= " + semester + strsec + " and batch_year=" + Session["batch_year"].ToString() + " and sch_date='" + sch_date + "'";
                dsipcode.Dispose();
                dsipcode.Reset();
                dsipcode = da.select_method(stripcode, hat, "Text");
                if (dsipcode.Tables.Count > 0 && dsipcode.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsipcode.Tables[0].Rows.Count; i++)
                    {

                        strtopic = "select distinct topics from lessonplantopics where lp_code=" + dsipcode.Tables[0].Rows[i]["lp_code"].ToString() + " and subject_no = " + subject_no + " and staff_code='" + staff_code + "' and hr=" + hr;
                        dstopic.Dispose();
                        dstopic.Reset();
                        dstopic = da.select_method(strtopic, hat, "Text");
                        if (dstopic.Tables.Count > 0 && dstopic.Tables[0].Rows.Count > 0)
                        {
                            topics = dstopic.Tables[0].Rows[0]["topics"].ToString();
                            if (topics.Contains("/"))
                            {
                                string[] split = topics.Split(new Char[] { '/' });
                                int ln = split.Length;
                                for (int t = 0; t < ln; t++)
                                {
                                    topics_today.Add(split[t].ToString());
                                    if (topics_plan.ToString().Trim() == "")
                                    {
                                        topics_plan = split[t].ToString();
                                    }
                                    else
                                    {
                                        topics_plan = topics_plan + "," + split[t].ToString();
                                    }
                                }
                            }
                            else
                            {
                                topics_today.Add(topics.ToString());
                                if (topics_plan.ToString().Trim() == "")
                                {
                                    topics_plan = topics.ToString();
                                }
                                else
                                {
                                    topics_plan = topics_plan + "," + topics.ToString();
                                }
                            }
                        }
                        //read_top.Close();
                    }
                }
                //End ========================================================================================================================================================
                //Yet To Be Complete=============================================================================================================================================================
                this.tvyet.Nodes.Clear();
                HierarchyTrees hierarchyTrees = new HierarchyTrees();
                HierarchyTrees.HTree objHTree = null;
                //con.Close();
                //con.Open();
                sqlstr = string.Empty;
                if (flag == "1") //As Per Lesson Plan
                {
                    if (topics_plan.ToString().Trim() != "")
                    {
                        sqlstr = "select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + subject_no + "' ";
                        sqlstr = sqlstr + " and (topic_no  in(select parent_code from sub_unit_details where topic_no in(" + topics_plan + "))";
                        sqlstr = sqlstr + " or topic_no  in( select parent_code from sub_unit_details where topic_no in(select parent_code from sub_unit_details where topic_no in(" + topics_plan + ")))";
                        sqlstr = sqlstr + " or topic_no in(" + topics_plan + ")) order by parent_code,topic_no";
                    }
                }
                else //General
                {
                    // order added by Srinath   //01-09-2014
                    // sqlstr = "select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + subject_no + "' "+ topics_Entryall +""; //Exclude Daily Entry Topic
                    sqlstr = "select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + subject_no + "' order by parent_code,topic_no "; //Include Daily Entry Topic
                }
                if (sqlstr.ToString().Trim() != "")
                {
                    dstopic.Dispose();
                    dstopic.Reset();
                    dstopic = da.select_method(sqlstr, hat, "Text");
                    this.tvyet.Nodes.Clear();
                    hierarchyTrees.Clear();
                    if (dstopic.Tables.Count > 0 && dstopic.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dstopic.Tables[0].Rows.Count; i++)
                        {
                            objHTree = new HierarchyTrees.HTree();
                            objHTree.topic_no = int.Parse(dstopic.Tables[0].Rows[i]["Topic_no"].ToString());
                            objHTree.parent_code = int.Parse(dstopic.Tables[0].Rows[i]["parent_code"].ToString());
                            objHTree.unit_name = dstopic.Tables[0].Rows[i]["unit_name"].ToString();
                            hierarchyTrees.Add(objHTree);
                        }
                        objHTree.unit_name = "Others";
                        hierarchyTrees.Add(objHTree);
                    }

                    foreach (HierarchyTrees.HTree hTree in hierarchyTrees)
                    {
                        HierarchyTrees.HTree parentNode = hierarchyTrees.Find(delegate(HierarchyTrees.HTree emp) { return emp.topic_no == hTree.parent_code; });
                        if (parentNode != null)
                        {
                            foreach (TreeNode tn in tvyet.Nodes)
                            {
                                if (tn.Value == parentNode.topic_no.ToString())
                                {
                                    tn.ChildNodes.Add(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
                                }
                                if (tn.ChildNodes.Count > 0)
                                {
                                    foreach (TreeNode ctn in tn.ChildNodes)
                                    {
                                        RecursiveChild(ctn, parentNode.topic_no.ToString(), hTree);
                                    }
                                }
                            }
                        }
                        else
                        {
                            tvyet.Nodes.Add(new TreeNode(hTree.unit_name, hTree.topic_no.ToString()));
                        }
                        // tvyet.ExpandAll();
                    }
                    //tn.ChildNodes.Add(new TreeNode("others", "0"));

                    string sub = "select * from dailyStaffEntry,dailyEntryother where isnull(othersub,'')<>'' and  degree_code=" + degree_code + " and semester= " + semester + strsec + " and batch_year=" + Session["batch_year"].ToString() + " and othersub=subpk    and college_code=" + Session["collegecode"] + " and sch_date='" + sch_date + "' ";
                    dsipcode.Reset();
                    dsipcode = da.select_method(sub, hat, "Text");
                    if (dsipcode.Tables.Count > 0 && dsipcode.Tables[0].Rows.Count > 0)
                    {
                        tvyet.Nodes.Add(new TreeNode("others", "0"));
                        for (int i = 0; i < dsipcode.Tables[0].Rows.Count; i++)
                        {
                            //foreach(HierarchyTrees.HTree hTree in hierarchyTrees1)
                            //{
                            //HierarchyTrees.HTree parentNode = hierarchyTrees1.Find(delegate(HierarchyTrees.HTree emp) { return emp.topic_no == hTree.parent_code; });
                            //if (parentNode != null)
                            // {
                            foreach (TreeNode tn in tvyet.Nodes)
                            {

                                if (tn.Value == "0")
                                {

                                    {

                                        tn.ChildNodes.Add(new TreeNode(Convert.ToString(dsipcode.Tables[0].Rows[i]["topic_name"]), Convert.ToString(dsipcode.Tables[0].Rows[i]["othersub"])));
                                        if (Convert.ToString(ddlother.SelectedItem.Text) != "Select")
                                        {
                                            tvyet.CheckedNodes.Add(new TreeNode(Convert.ToString(dsipcode.Tables[0].Rows[i]["topic_name"]), Convert.ToString(dsipcode.Tables[0].Rows[i]["othersub"])));
                                        }
                                    }

                                    //}
                                    //}
                                    //if (tn.ChildNodes.Count > 0)
                                    //{
                                    //    foreach (TreeNode ctn in tn.ChildNodes)
                                    //    {
                                    //        RecursiveChild(ctn, "0", hTree);
                                    //    }
                                    //}

                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void btnaddnotes_Click(object sender, EventArgs e)
    {
        try
        {
            if (!fileupload.HasFile)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select The File And Then Proceed";
                return;
            }
            string strval = string.Empty;
            if (ddlclassnotes.SelectedItem.ToString().Trim().ToLower() == "all")
            {
                for (int dlt = 0; dlt < ddlclassnotes.Items.Count; dlt++)
                {
                    string setval = ddlclassnotes.Items[dlt].Value.ToString();
                    string[] spset = setval.Split('-');
                    if (spset.GetUpperBound(0) >= 5)
                    {
                        if (strval == "")
                        {
                            strval = ddlclassnotes.Items[dlt].Value.ToString();
                        }
                        else
                        {
                            strval = strval + '*' + ddlclassnotes.Items[dlt].Value.ToString();
                        }
                    }
                }
            }
            else
            {
                strval = ddlclassnotes.SelectedValue.ToString();
            }
            string[] sptree = strval.Split('*');
            bool savnotsflag = false;

            int actrow1 = 0;
            int actcol1 = 0;
            int.TryParse(Convert.ToString(Session["Row"]), out actrow1);
            int.TryParse(Convert.ToString(Session["Col"]), out actcol1);


            string sch_dt = string.Empty;
            string getdate = (gridTimeTable.Rows[ar].FindControl("lblDateDisp") as Label).Text;
            //string[] spdatesp = getdate.Split('(');



            sch_dt = getdate.ToString();
            string degree_code = string.Empty;
            string semester = string.Empty;
            string subject_no = string.Empty;
            string batchyear = string.Empty;
            string entrycode = Session["Entry_Code"].ToString();//saranya
            string PageName = "Student Attendance";

            string TimeOfAttendance = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
            string DateOfAttendence = DateTime.Now.ToString("MM/dd/yyy");
            for (int trs = 0; trs <= sptree.GetUpperBound(0); trs++)
            {
                //int actrow1 = 0;
                //int actcol1 = 0;
                //string sub_name = FpSpread1.Sheets[0].Cells[actrow1, actcol1].Text;
                //string[] subj_name_split = sub_name.Split('-');
                //string subj_name = subj_name_split[0].ToString();
                //  string subcode = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow1, actcol1].Tag);
                // string subcode = ddlclassnotes.SelectedValue.ToString();
                degree_code = string.Empty;
                semester = string.Empty;
                subject_no = string.Empty;
                batchyear = string.Empty;
                string subcode = sptree[trs].ToString();
                string subj_name = string.Empty;
                string treepath = string.Empty;
                if (ddlclassnotes.Items.Count > 0)
                {
                    string valsp = sptree[trs].ToString();
                    string[] sp1 = valsp.Split(new Char[] { '-' });
                    if (sp1.GetUpperBound(0) > 2)
                    {
                        string subcode1 = sp1[2].ToString();
                        subj_name = da.GetFunction("Select Subject_Name from subject where subject_no='" + subcode1 + "'");
                        treepath = subj_name + " " + "/";
                    }
                }
                lblerror.Visible = false;
                if (fileupload.HasFile)
                {
                    if (fileupload.FileName.EndsWith(".jpg") || fileupload.FileName.EndsWith(".gif") || fileupload.FileName.EndsWith(".png") || fileupload.FileName.EndsWith(".txt") || fileupload.FileName.EndsWith(".doc") || fileupload.FileName.EndsWith(".xls") || fileupload.FileName.EndsWith(".docx") || fileupload.FileName.EndsWith(".txt") || fileupload.FileName.EndsWith(".document") || fileupload.FileName.EndsWith(".xls") || fileupload.FileName.EndsWith(".xlsx") || fileupload.FileName.EndsWith(".pdf") || fileupload.FileName.EndsWith(".ppt") || fileupload.FileName.EndsWith(".pptx"))
                    {
                        string fileName = Path.GetFileName(fileupload.PostedFile.FileName);
                        string fileExtension = Path.GetExtension(fileupload.PostedFile.FileName);
                        string documentType = string.Empty;
                        switch (fileExtension)
                        {
                            case ".pdf":
                                documentType = "application/pdf";
                                break;
                            case ".xls":
                                documentType = "application/vnd.ms-excel";
                                break;
                            case ".xlsx":
                                documentType = "application/vnd.ms-excel";
                                break;
                            case ".doc":
                                documentType = "application/vnd.ms-word";
                                break;
                            case ".docx":
                                documentType = "application/vnd.ms-word";
                                break;
                            case ".gif":
                                documentType = "image/gif";
                                break;
                            case ".png":
                                documentType = "image/png";
                                break;
                            case ".jpg":
                                documentType = "image/jpg";
                                break;
                            case ".ppt":
                                documentType = "application/vnd.ms-ppt";
                                break;
                            case ".pptx":
                                documentType = "application/vnd.ms-pptx";
                                break;
                            case ".txt":
                                documentType = "application/txt";
                                break;
                        }
                        int fileSize = fileupload.PostedFile.ContentLength;
                        //Create array and read the file into it
                        byte[] documentBinary = new byte[fileSize];
                        fileupload.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                        if (subcode.ToString().Trim() != "")
                        {
                            string[] sp1 = subcode.Split(new Char[] { '-' });
                            degree_code = sp1[0];
                            semester = sp1[1];
                            subject_no = sp1[2];
                            if (sp1.GetUpperBound(0) == 7)
                            {
                                batchyear = sp1[4];
                            }
                            else
                            {
                                batchyear = sp1[3];
                            }
                            string[] spdate = sch_dt.Split(new Char[] { '-' });
                            if (spdate[0].Length == 1)
                            {
                                spdate[0] = "0" + spdate[0];
                            }
                            if (spdate[1].Length == 1)
                            {
                                spdate[1] = "0" + spdate[1];
                            }
                            sch_dt = spdate[0] + "-" + spdate[1] + "-" + spdate[2];
                            string date1 = spdate[1] + "-" + spdate[0] + "-" + spdate[2];
                            sch_dt = spdate[0] + "-" + spdate[1] + "-" + spdate[2];
                            SqlCommand cmdnotes = new SqlCommand();
                            string fileid = batchyear + "@" + degree_code + "@" + semester + "@" + subject_no;
                            cmdnotes.CommandText = "INSERT INTO notestbl(filename,filetype,filedata,date,subject_no,subject_name,degree_code,sem,batch,treeview,fileid)" + " VALUES (@DocName,@Type,@DocData,@date,@subject_no,@subject_name,@degree_code,@semester,@batch,@treepath,@fileid)";
                            cmdnotes.CommandType = CommandType.Text;
                            cmdnotes.Connection = ssql;
                            SqlParameter DocName = new SqlParameter("@DocName", SqlDbType.VarChar, 50);
                            DocName.Value = fileName.ToString();
                            cmdnotes.Parameters.Add(DocName);
                            SqlParameter Type = new SqlParameter("@Type", SqlDbType.VarChar, 50);
                            Type.Value = documentType.ToString();
                            cmdnotes.Parameters.Add(Type);
                            SqlParameter uploadedDocument = new SqlParameter("@DocData", SqlDbType.Binary, fileSize);
                            uploadedDocument.Value = documentBinary;
                            cmdnotes.Parameters.Add(uploadedDocument);
                            SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 100);
                            uploadedDate.Value = date1;
                            cmdnotes.Parameters.Add(uploadedDate);
                            SqlParameter uploadedsubject_no = new SqlParameter("@subject_no", SqlDbType.VarChar, 50);
                            uploadedsubject_no.Value = subject_no;
                            cmdnotes.Parameters.Add(uploadedsubject_no);
                            SqlParameter uploadedsubject_name = new SqlParameter("@subject_name", SqlDbType.VarChar, 500);
                            uploadedsubject_name.Value = subj_name;
                            cmdnotes.Parameters.Add(uploadedsubject_name);
                            SqlParameter uploaded_deg_code = new SqlParameter("@degree_code", SqlDbType.VarChar, 50);
                            uploaded_deg_code.Value = degree_code;
                            cmdnotes.Parameters.Add(uploaded_deg_code);
                            SqlParameter uploaded_sem = new SqlParameter("@semester", SqlDbType.Int, 50);
                            uploaded_sem.Value = semester;
                            cmdnotes.Parameters.Add(uploaded_sem);
                            SqlParameter uploaded_batch_yr = new SqlParameter("@batch", SqlDbType.Int, 50);
                            uploaded_batch_yr.Value = batchyear;
                            cmdnotes.Parameters.Add(uploaded_batch_yr);
                            SqlParameter uploaded_treepath = new SqlParameter("@treepath", SqlDbType.VarChar, 500);
                            uploaded_treepath.Value = treepath;
                            cmdnotes.Parameters.Add(uploaded_treepath);
                            SqlParameter uploaded_id = new SqlParameter("@fileid", SqlDbType.VarChar, 500);
                            uploaded_id.Value = fileid;
                            cmdnotes.Parameters.Add(uploaded_id);
                            //string insertquery = " INSERT INTO notestbl(filename,filetype,filedata,date,subject_no,subject_name,degree_code,sem,batch,treeview,fileid)";
                            //insertquery = insertquery + " values ('" + fileName + "','" + documentType + "'," + documentBinary + ",'" + date1 + "','" + subject_no + "','" + subj_name + "','" + degree_code + "','" + semester + "','" + batchyear + "','" + treepath + "','" + fileid + "')";
                            ssql.Close();
                            ssql.Open();
                            int result = cmdnotes.ExecuteNonQuery();
                            savnotsflag = true;
                        }
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Selected file format is Not allowed";
                    }
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Selected file format is Not allowed";
                }
            }
            if (savnotsflag == true)
            {
                retrivespreadfornotes(batchyear, degree_code, semester, subject_no, sch_dt);
                string ctsname = "Save the Notes Upload Information";
                da.insertUserActionLog(entrycode, batchyear, degree_code, semester, sections, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 1);//saranya
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Saved Successfully')", true);
            }
            //else
            //{
            //    lblerror.Visible = true;
            //    lblerror.Text = "Select the Topic And Proceed";
            //}
            fileupload.Focus();
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void btnaddquestion_Click(object sender, EventArgs e)
    {
        try
        {
            string unitselected = string.Empty;
            string treepath = selectedpath;
            if (treepath != "")
            {
                if (txtquestion1.Text != "")
                {
                    if (ddlgivemarks.Text != "")
                    {
                        if (ddlunits.Items.Count > 0)
                        {
                            if (ddlunits.SelectedItem.Text.Trim() != "")
                            {
                                unitselected = ddlunits.SelectedItem.Value.ToString();
                            }
                        }
                        int actrow1 = 0;
                        int actcol1 = 0;
                        string sch_dt = string.Empty;
                        string degree_code = string.Empty;
                        string semester = string.Empty;
                        string subject_no = string.Empty;
                        string batchyear = string.Empty;

                        int.TryParse(Convert.ToString(Session["Row"]), out actrow1);
                        int.TryParse(Convert.ToString(Session["Col"]), out actcol1);


                        string sub_name = ddlselectmanysub.SelectedItem.ToString();
                        string subcode = ddlselectmanysub.SelectedValue.ToString();

                        // string[] spdatesp = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text.Split(' ');
                        string spdatesp = (gridTimeTable.Rows[ar].FindControl("lblDateDisp") as Label).Text;

                        sch_dt = spdatesp.ToString();

                        string entrycode = Session["Entry_Code"].ToString();//saranya
                        string PageName = "Student Attendance";

                        string TimeOfAttendance = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                        string DateOfAttendence = DateTime.Now.ToString("MM/dd/yyy");
                        // sch_dt = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text;
                        if (subcode != "")
                        {
                            string[] sp1 = subcode.Split(new Char[] { '-' });
                            if (sp1.GetUpperBound(0) == 7)
                            {
                                batchyear = sp1[4];
                            }
                            else
                            {
                                batchyear = sp1[3];
                            }
                            degree_code = sp1[0];
                            semester = sp1[1];
                            subject_no = sp1[2];
                            string[] spdate = sch_dt.Split(new Char[] { '-' });
                            if (spdate[0].Length == 1)
                            {
                                spdate[0] = "0" + spdate[0];
                            }
                            if (spdate[1].Length == 1)
                            {
                                spdate[1] = "0" + spdate[1];
                            }
                            sch_dt = spdate[0] + "-" + spdate[1] + "-" + spdate[2];
                            string date1 = spdate[1] + "-" + spdate[0] + "-" + spdate[2];
                            string question = txtquestion1.Text;
                            question = Regex.Replace(question, "'", ".");
                            string insertquestions = "insert into attendance_question_addition (batch_year,degree_code,semester,subject_no,date,question,marks,treeviewpath,subj_unit) values(" + batchyear + "," + degree_code + "," + semester + "," + subject_no + ",'" + date1 + "','" + question + "','" + ddlgivemarks.SelectedItem.Text + "','" + treepath + "','" + unitselected + "')";
                            int a = da.insert_method(insertquestions, hat, "Text");
                            string ctsname = "Save the Notes Upload Information";
                            da.insertUserActionLog(entrycode, batchyear, degree_code, semester, sections, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 1);//saranya
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Added Successfully')", true);
                            retrievespreadattendancequestion(batchyear, degree_code, semester, subject_no, sch_dt);
                            txtquestion1.Text = string.Empty;
                            ddlgivemarks.SelectedValue = "0";
                        }
                    }
                    else
                    {
                        lblerrorquestionadd_att.Visible = true;
                        lblerrorquestionadd_att.Text = "Enter the Marks";
                    }
                }
                else
                {
                    lblerrorquestionadd_att.Visible = true;
                    lblerrorquestionadd_att.Text = "Enter the Question";
                }
            }
            else
            {
                lblerrorquestionadd_att.Visible = true;
                lblerrorquestionadd_att.Text = "Select the Topic And Proceed";
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    public void loadgraphics()
    {
        try
        {
            rbgraphics.Checked = true;
            rbappenses.Checked = false;
            GridView1.Visible = true;
            Buttonselectall.Visible = true;
            Buttondeselect.Visible = true;
            Buttonsave.Visible = true;
            //Buttonupdate.Visible = true;



            int acr = 0;
            int acc = 0;
            int.TryParse(Convert.ToString(Session["Row"]), out acr);
            int.TryParse(Convert.ToString(Session["Col"]), out acc);
            if (acr > -1 && acc > -1)
            {
                string tagval = "lnkPeriod_" + acc;
                //string degreeclass = FpSpread1.Sheets[0].Cells[acr, acc].Text.ToString();
                string degreeclass = (gridTimeTable.Rows[ar].FindControl(tagval) as LinkButton).Text;
                string[] splitclass = degreeclass.Split('*');
                if (splitclass.GetUpperBound(0) > 0)
                {
                    lblmanysubject.Visible = true;
                    ddlselectmanysub.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    public void loadreason()
    {
        ddlreason.Items.Clear();
        string collegecode = Session["collegecode"].ToString();
        string query = "select TextCode,Textval from textvaltable where TextCriteria='Attrs' and college_code=" + collegecode + "";
        ds.Dispose(); ds.Reset();
        ds = da.select_method_wo_parameter(query, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlreason.DataSource = ds;
            ddlreason.DataTextField = "Textval";
            ddlreason.DataValueField = "TextCode";
            ddlreason.DataBind();
        }
    }
    public void loadalternode()
    {
        tvalterlession.Nodes.Clear();
        int hr = 0;
        string batch_year = string.Empty;
        string secval = string.Empty;
        string sch_dt = string.Empty;
        string dateval = string.Empty;

        int actrow1 = 0;
        int acc = 0;
        int.TryParse(Convert.ToString(Session["Row"]), out actrow1);
        int.TryParse(Convert.ToString(Session["Col"]), out acc);

        sch_dt = (gridTimeTable.Rows[ar].FindControl("lblDate") as Label).Text;

        string spdatesp = (gridTimeTable.Rows[ar].FindControl("lblDateDisp") as Label).Text;

        sch_dt = spdatesp.ToString();
        if (sch_dt != "")
        {
            string[] sp_date = sch_dt.Split(new Char[] { '-' });
            dateval = sp_date[2].ToString() + "-" + sp_date[1].ToString() + "-" + sp_date[0].ToString();
        }
        Plessionalter.Visible = true;
        if (ddlselectmanysub.Items.Count > 0)
        {
            string getddlsub = ddlselectmanysub.SelectedValue.ToString();
            if (getddlsub.Trim() != "")
            {
                string[] sp1 = getddlsub.Split(new Char[] { '-' });
                if (sp1.GetUpperBound(0) >= 7)
                {
                    degree_code = sp1[0];
                    semester = sp1[1];
                    subject_no = sp1[2];
                    sections = sp1[3];
                    batch_year = sp1[4];
                    secval = " and isnull(Sections,'')='" + sections + "'";
                }
                else
                {
                    degree_code = sp1[0];
                    semester = sp1[1];
                    subject_no = sp1[2];
                    sections = string.Empty;
                    batch_year = sp1[4];
                }
            }
        }
        else
        {
            if (getcelltag != "")
            {
                string[] sp1 = getcelltag.Split(new Char[] { '-' });
                if (sp1.GetUpperBound(0) >= 7)
                {
                    degree_code = sp1[0];
                    semester = sp1[1];
                    subject_no = sp1[2];
                    sections = sp1[3];
                    batch_year = sp1[4];
                    secval = " and isnull(Sections,'')='" + sections + "'";
                }
                else
                {
                    degree_code = sp1[0];
                    semester = sp1[1];
                    subject_no = sp1[2];
                    sections = string.Empty;
                    batch_year = sp1[4];
                }
            }
        }
        string topics_Entry = string.Empty;
        string strquerylession = "select * from lesson_plan p,lessonplantopics l where l.lp_code=p.lp_code and p.degree_code='" + degree_code + "' and p.Batch_Year='" + batch_year + "' and p.semester='" + semester + "' and sch_date<='" + dateval + "' and subject_no='" + subject_no + "'  order by sch_date,hr";
        DataSet dsalter = da.select_method_wo_parameter(strquerylession, "Text");
        if (dsalter.Tables.Count > 0 && dsalter.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsalter.Tables[0].Rows.Count; i++)
            {
                string strlession = dsalter.Tables[0].Rows[i]["topics"].ToString();
                string[] spless = strlession.Split('/');
                for (int s = 0; s <= spless.GetUpperBound(0); s++)
                {
                    string lessionval = spless[s].ToString();
                    if (lessionval.Trim() != "" && lessionval != null)
                    {
                        if (topics_Entry == "")
                        {
                            topics_Entry = lessionval;
                        }
                        else
                        {
                            topics_Entry = topics_Entry + "," + lessionval;
                        }
                    }
                }
            }
        }
        HierarchyTrees hierarchyTrees1 = new HierarchyTrees();
        HierarchyTrees.HTree objHTree1 = null;
        hierarchyTrees1.Clear();
        tvalterlession.Nodes.Clear();
        string sqlstr = "select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + subject_no + "' ";
        sqlstr = sqlstr + " and (topic_no  in(select parent_code from sub_unit_details where topic_no in(" + topics_Entry + "))";
        sqlstr = sqlstr + " or topic_no  in( select parent_code from sub_unit_details where topic_no in(select parent_code from sub_unit_details where topic_no in(" + topics_Entry + ")))";
        sqlstr = sqlstr + " or topic_no in(" + topics_Entry + ")) order by parent_code,topic_no";
        DataSet dstopic = da.select_method(sqlstr, hat, "Text");
        if (dstopic.Tables.Count > 0 && dstopic.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dstopic.Tables[0].Rows.Count; i++)
            {
                objHTree1 = new HierarchyTrees.HTree();
                objHTree1.topic_no = int.Parse(dstopic.Tables[0].Rows[i]["Topic_no"].ToString());
                objHTree1.parent_code = int.Parse(dstopic.Tables[0].Rows[i]["parent_code"].ToString());
                objHTree1.unit_name = dstopic.Tables[0].Rows[i]["unit_name"].ToString();
                hierarchyTrees1.Add(objHTree1);
            }
        }
        foreach (HierarchyTrees.HTree hTree in hierarchyTrees1)
        {
            HierarchyTrees.HTree parentNode = hierarchyTrees1.Find(delegate(HierarchyTrees.HTree emp) { return emp.topic_no == hTree.parent_code; });
            if (parentNode != null)
            {
                foreach (TreeNode tn in tvalterlession.Nodes)
                {
                    if (tn.Value == parentNode.topic_no.ToString())
                    {
                        tn.ChildNodes.Add(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
                    }
                    if (tn.ChildNodes.Count > 0)
                    {
                        foreach (TreeNode ctn in tn.ChildNodes)
                        {
                            RecursiveChild(ctn, parentNode.topic_no.ToString(), hTree);
                        }
                    }
                }
            }
            else
            {
                tvalterlession.Nodes.Add(new TreeNode(hTree.unit_name, hTree.topic_no.ToString()));
            }
            tvalterlession.ExpandAll();
        }
    }
    public void retrivespreadfornotes(string batchyear, string degree_code, string semester, string subject_no, string datenew)
    {
        try
        {
            if (ddlselectmanysub.Items.Count > 0)
            {
                string valsp = ddlselectmanysub.SelectedValue.ToString();
                string[] sp1 = valsp.Split(new Char[] { '-' });
                if (sp1.GetUpperBound(0) > 2)
                {
                    string subcode = sp1[2].ToString();
                    string subname = da.GetFunction("Select Subject_Name from subject where subject_no='" + subcode + "'");
                    selectedpath = subname + " " + "/";
                }
            }
            // string getdate = "select date,path,subject_name,subject.subject_no,treeview from attendance_document_save,subject where subject.subject_no=attendance_document_save.subject_no and batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + semester + " and treeview like'" + selectedpath + "%' and attendance_document_save.subject_no=" + subject_no + " and convert(varchar(20),attendance_document_save.date,105)='" + datenew + "'";
            string getdate = "select filename,date,subject_name,subject_no,treeview from notestbl where subject_no=" + subject_no + " and batch=" + batchyear + " and degree_code=" + degree_code + " and sem=" + semester + " and subject_no=" + subject_no + " and treeview like'" + selectedpath + "%' ";//and date='"+datetime+"'";//and convert(varchar(20),attendance_document_save.date,105)='" + datenew + "'";
            //Modified by srinath 12/9/2013
            //  SqlDataAdapter dagetdate = new SqlDataAdapter(getdate, ssql);
            DataSet dsgetdate = new DataSet();
            dsgetdate = da.select_method(getdate, hat, "Text");
            DataTable dtDate = new DataTable();///date1
            dtDate.Columns.Add("date1");//Batch
            dtDate.Columns.Add("Batch");
            dtDate.Columns.Add("degCode");
            dtDate.Columns.Add("subName");
            dtDate.Columns.Add("sem");
            dtDate.Columns.Add("topic");
            dtDate.Columns.Add("subNo");
            dtDate.Columns.Add("path");
            dtDate.Columns.Add("pathtag");
            DataRow dr = null;
            if (dsgetdate.Tables.Count > 0 && dsgetdate.Tables[0].Rows.Count > 0)
            {
                //FpSpread3.Visible = true;
                string date = string.Empty;
                string subject = string.Empty;
                string path = string.Empty;
                string subjectno = string.Empty;
                string treepath = string.Empty;
                int sno = 0;
                for (int i = 0; i < dsgetdate.Tables[0].Rows.Count; i++)
                {
                    dr = dtDate.NewRow();
                    string selecttopic = string.Empty;
                    string date1 = string.Empty;
                    string[] treepath1 = new string[10];
                    string getpathname = string.Empty;
                    string topic = string.Empty;
                    int maxpath = 0;
                    sno++;
                    date = dsgetdate.Tables[0].Rows[i]["date"].ToString();
                    string[] spdate = date.Split(new Char[] { '/' });
                    string[] spyear = spdate[2].Split(new char[] { ' ' });
                    if (spdate[0].Length == 1)
                    {
                        spdate[0] = "0" + spdate[0];
                    }
                    if (spdate[1].Length == 1)
                    {
                        spdate[1] = "0" + spdate[1];
                    }
                    date1 = spdate[1] + "-" + spdate[0] + "-" + spyear[0];
                    subject = dsgetdate.Tables[0].Rows[i]["subject_name"].ToString();
                    subjectno = dsgetdate.Tables[0].Rows[i]["subject_no"].ToString();
                    path = dsgetdate.Tables[0].Rows[i]["filename"].ToString();
                    treepath = dsgetdate.Tables[0].Rows[i]["treeview"].ToString();
                    string[] treepath2 = treepath.Split(new char[] { '=' });
                    if (treepath2.GetUpperBound(0) > 0)
                    {
                        for (int i1 = 0; i1 <= treepath2.GetUpperBound(0); i1++)
                        {
                            treepath1 = treepath2[i1].Split(new char[] { '/' });
                            maxpath = treepath1.GetUpperBound(0);
                            if (treepath1.GetUpperBound(0) > 1)
                            {
                                topic = treepath1[maxpath];
                                getpathname = "select * from sub_unit_details where subject_no = " + subjectno + " and topic_no=" + topic + " order by topic_no";
                            }
                            else
                            {
                                topic = treepath1[0];
                                getpathname = "select * from sub_unit_details where subject_no = " + subjectno + "  order by topic_no";
                            }
                            lblerrorquestionadd_att.Visible = false;
                            //Modified by srinath 12/9/2013
                            DataSet dsgetpathname = new DataSet();
                            dsgetpathname = da.select_method(getpathname, hat, "Text");
                            if (dsgetpathname.Tables.Count > 0 && dsgetpathname.Tables[0].Rows.Count > 0)
                            {
                                if (selecttopic == "")
                                {
                                    selecttopic = dsgetpathname.Tables[0].Rows[0]["unit_name"].ToString();
                                }
                                else
                                {
                                    selecttopic = selecttopic + "," + dsgetpathname.Tables[0].Rows[0]["unit_name"].ToString();
                                }
                            }
                            if (selecttopic == "")
                            {
                                selecttopic = treepath1[0];
                            }
                        }
                    }
                    else
                    {
                        treepath1 = treepath.Split(new char[] { '/' });
                        maxpath = treepath1.GetUpperBound(0);
                        if (treepath1.GetUpperBound(0) > 1)
                        {
                            topic = treepath1[maxpath];
                            getpathname = "select * from sub_unit_details where subject_no = " + subjectno + " and topic_no=" + topic + " order by topic_no";
                        }
                        else
                        {
                            topic = treepath1[0];
                            getpathname = "select * from sub_unit_details where subject_no = " + subjectno + "  order by topic_no";
                        }
                        lblerrorquestionadd_att.Visible = false;
                        //Modified by srinath 12/9/2013
                        DataSet dsgetpathname = new DataSet();
                        dsgetpathname = da.select_method(getpathname, hat, "Text");
                        if (dsgetpathname.Tables.Count > 0 && dsgetpathname.Tables[0].Rows.Count > 0)
                        {
                            if (selecttopic == "")
                            {
                                selecttopic = dsgetpathname.Tables[0].Rows[0]["unit_name"].ToString();
                            }
                            else
                            {
                                selecttopic = selecttopic + "," + dsgetpathname.Tables[0].Rows[0]["unit_name"].ToString();
                            }
                        }
                        if (selecttopic == "")
                        {
                            selecttopic = treepath1[0];
                        }
                    }

                    if (treepath != "" && selecttopic != "")
                    {
                        dr["date1"] = date1;//Batch
                        dr["Batch"] = batchyear;
                        dr["degCode"] = degree_code;
                        dr["subName"] = subject;
                        dr["sem"] = semester;
                        dr["topic"] = selecttopic;
                        dr["subNo"] = subjectno;
                        dr["path"] = path;
                        dr["pathtag"] = batchyear + "@" + degree_code + "@" + semester + "@" + subject_no;
                        dtDate.Rows.Add(dr);
                    }
                }

                if (dtDate.Rows.Count > 0)
                {
                    GridView3.DataSource = dtDate;
                    GridView3.DataBind();
                    GridView3.Visible = true;
                }
            }
            else
            {
                GridView3.Visible = false;
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void lnkDownload_click(Object sender, EventArgs e)
    {
        try
        {
            //bool x = FpSpread3.Sheets[0].AutoPostBack;
            btnSave.Enabled = false;
            btnnotesdelete.Enabled = true;
            string activerow = string.Empty;
            string activecol = string.Empty;


            LinkButton lnkSelected = (LinkButton)sender;
            string rowIndxS = lnkSelected.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            //string colIndxS = lnkSelected.UniqueID.ToString().Split('$')[4].Replace("lnkPeriod_", string.Empty);
            int colIndx = 4;


            activerow = rowIndx.ToString();
            activecol = colIndx.ToString();

            if (Convert.ToInt32(activecol) == 4)
            {
                string fileName = string.Empty;



                Label lblpath = (GridView3.Rows[rowIndx].FindControl("lblPathTag") as Label);
                Label lblFile = (GridView3.Rows[rowIndx].FindControl("lblPath") as Label);

                path1 = lblpath.Text;
                string fileid = lblFile.Text;

                string strquer = "SELECT filename,filedata,filetype FROM notestbl WHERE fileid='" + path1 + "' and filename='" + fileid + "'";
                DataSet dsquery = da.select_method(strquer, hat, "Text");
                if (dsquery.Tables.Count > 0 && dsquery.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                    {
                        Response.ContentType = dsquery.Tables[0].Rows[i]["filetype"].ToString();
                        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + dsquery.Tables[0].Rows[i]["filename"] + "\"");
                        Response.BinaryWrite((byte[])dsquery.Tables[0].Rows[i]["filedata"]);
                        Response.End();
                    }
                }
            }
            fileupload.Focus();
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void lnkDelete_click(object sender, EventArgs e)
    {
        try
        {
            string actrow1 = string.Empty;
            int actcol1 = 0;
            LinkButton lnkSelected = (LinkButton)sender;
            string rowIndxS = lnkSelected.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            //string colIndxS = lnkSelected.UniqueID.ToString().Split('$')[4].Replace("lnkPeriod_", string.Empty);
            int colIndx = 5;

            actrow1 = Convert.ToString(rowIndx);
            actcol1 = colIndx;

            if (Convert.ToInt32(actrow1) >= 0 && actrow1 != "")
            {


                string date = (GridView3.Rows[rowIndx].FindControl("lblDate") as Label).Text;
                string path = (GridView3.Rows[rowIndx].FindControl("lblPathdel") as Label).Text;
                string batvhyear = (GridView3.Rows[rowIndx].FindControl("lblBatchYear") as Label).Text;
                string degreecode = (GridView3.Rows[rowIndx].FindControl("lblDegCode") as Label).Text;
                string semester = (GridView3.Rows[rowIndx].FindControl("lblSem") as Label).Text;
                string subno = (GridView3.Rows[rowIndx].FindControl("lblSubjectNo") as Label).Text;


                string deletenotes = "delete from notestbl where batch=" + batvhyear + " and degree_code=" + degreecode + " and sem=" + semester + " and convert(varchar(20),date,105)='" + date + "' and filename='" + path + "'";
                int a = da.update_method_wo_parameter(deletenotes, "Text");
                string ctsname = "Delete the Notes Entry Information";

                string entrycode = Session["Entry_Code"].ToString();//saranya
                string PageName = "Student Attendance";

                string TimeOfAttendance = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                string DateOfAttendence = DateTime.Now.ToString("MM/dd/yyy");
                da.insertUserActionLog(entrycode, batvhyear, degree_code, semester, sections, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 3);//saranya

                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert(' Deleted Successfully')", true);
                retrivespreadfornotes(batvhyear, degreecode, semester, subno, date);
                btndeleteatndqtn.Enabled = false;
                btnSave.Enabled = true;
                btnnotesdelete.Enabled = false;
            }
            fileupload.Focus();
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void ddlnoofanswers_SelectedIndexchanged(object sender, EventArgs e)
    {
        try
        {
            txtqtnname.Text = string.Empty;
            btnqtnsave.Enabled = true;
            DataTable dtCho = new DataTable();
            dtCho.Columns.Add("option");
            dtCho.Columns.Add("Val");
            dtCho.Columns.Add("ischecked");
            DataRow drRow = null;
            if (ddlnoofanswers.SelectedValue.ToString() != "A")
            {
                int noofchc = Convert.ToInt32(ddlnoofanswers.SelectedItem.Text);

                for (int i = 1; i <= noofchc; i++)
                {
                    drRow = dtCho.NewRow();
                    string sno = ddlnoofanswers.Items[i - 1].Value;
                    drRow["option"] = sno;
                    drRow["Val"] = "";
                    drRow["ischecked"] = "0";
                    dtCho.Rows.Add(drRow);
                }
            }
            if (dtCho.Rows.Count > 0)
            {
                GridView4.DataSource = dtCho;
                GridView4.DataBind();
                GridView4.Visible = true;
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void btnqtnsave_Click(object sender, EventArgs e)
    {
        try
        {
            string treepath = selectedpath;
            if (ddlselectmanysub.Items.Count > 0)
            {
                string valsp = ddlselectmanysub.SelectedValue.ToString();
                string[] sp1 = valsp.Split(new Char[] { '-' });
                if (sp1.GetUpperBound(0) > 2)
                {
                    string subcode = sp1[2].ToString();
                    string subname = da.GetFunction("Select Subject_Name from subject where subject_no='" + subcode + "'");
                    treepath = subname + " " + "/";
                }
            }
            if (treepath != "")
            {
                lblnorec.Visible = false;
                int ansflag = 0;
                int ansfillflag = 0;
                int qtnflag = 0;

                //if (TreeView1.SelectedNode != null)
                //{
                int actrow1 = 0;
                int actcol1 = 0;
                string sch_dt = string.Empty;
                string degree_code = string.Empty;
                string semester = string.Empty;
                string subject_no = string.Empty;
                string batchyear = string.Empty;
                int.TryParse(Convert.ToString(Session["Row"]), out actrow1);
                int.TryParse(Convert.ToString(Session["Col"]), out actcol1);

                string sub_name = (gridTimeTable.Rows[actrow1].FindControl("lnkPeriod_" + actcol1) as LinkButton).Text;
                string subcode = Convert.ToString((gridTimeTable.Rows[actrow1].FindControl("lblPeriod_" + actcol1) as Label).Text);
                string spdatesp = (gridTimeTable.Rows[actrow1].FindControl("lblDateDisp") as Label).Text;
                sch_dt = spdatesp.ToString();
                //sch_dt = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text;
                if (subcode != "")
                {
                    if (txtqtnname.Text != "")
                    {
                        string nofans = string.Empty;
                        for (int row = 0; row < Convert.ToInt32(GridView4.Rows.Count); row++)
                        {
                            string answers = Convert.ToString((GridView4.Rows[row].FindControl("opttxt") as TextBox).Text);
                            if (nofans == "")
                            {
                                nofans = answers;
                            }
                            else
                            {
                                nofans = nofans + "?" + answers;
                            }
                            nofans = Regex.Replace(nofans, "'", ".");
                            if (answers == "")
                            {
                                ansfillflag = 1;
                                lblnorec.Visible = true;
                                lblnorec.Text = "Fill all the Answers and Proceed";
                            }
                        }
                        string corretans = string.Empty;


                        for (int res = 0; res < Convert.ToInt32(GridView4.Rows.Count); res++)
                        {
                            bool isval = false;

                            string s = Convert.ToString((GridView4.Rows[res].FindControl("lbloption") as Label).Text);

                            isval = (GridView4.Rows[res].FindControl("Option") as CheckBox).Checked;

                            CheckBox chk = GridView4.Rows[res].FindControl("Option") as CheckBox;
                            if (isval && ansflag == 0)
                            {
                                ansflag = 1;
                                corretans = Convert.ToString((GridView4.Rows[res].FindControl("opttxt") as TextBox).Text);
                                corretans = Regex.Replace(corretans, "'", ".");
                                chk.Checked = false;
                                //sprdnoofchoices.Sheets[0].Cells[res, 1].Value = false;
                            }
                            else if (ansflag == 1 && isval)
                            {
                                chk.Checked = false;
                                //sprdnoofchoices.Sheets[0].Cells[res, 1].Value = false;
                                ansflag = 2;
                                lblnorec.Visible = true;
                                lblnorec.Text = "Select Any one Answer as correct Answer";
                            }
                        }
                        string tough = string.Empty;
                        if (radiotough1.Checked == true)
                        {
                            tough = "1";
                        }
                        else if (radiotough2.Checked == true)
                        {
                            tough = "2";
                        }
                        else if (radiotough3.Checked == true)
                        {
                            tough = "3";
                        }
                        else if (radiotough4.Checked == true)
                        {
                            tough = "4";
                        }
                        string[] sp1 = subcode.Split(new Char[] { '-' });
                        if (sp1.GetUpperBound(0) > 0)
                        {
                            degree_code = sp1[0];
                            semester = sp1[1];
                            subject_no = sp1[2];
                            if (sp1.GetUpperBound(0) == 7)
                            {
                                batchyear = sp1[4];
                            }
                            else
                            {
                                batchyear = sp1[4];//3
                                if (batchyear.ToUpper() == "S" || batchyear.ToUpper() == "L" || batchyear.ToUpper() == "L")
                                    batchyear = sp1[3];//3

                            }
                        }
                        string[] spdate = sch_dt.Split(new Char[] { '-' });
                        if (spdate[0].Length == 1)
                        {
                            spdate[0] = "0" + spdate[0];
                        }
                        if (spdate[1].Length == 1)
                        {
                            spdate[1] = "0" + spdate[1];
                        }
                        string date1 = spdate[1] + "-" + spdate[0] + "-" + spdate[2];
                        sch_dt = spdate[0] + "-" + spdate[1] + "-" + spdate[2];
                        string[] subject1 = sub_name.Split(new char[] { '2' });
                        sub_name = subject1[0].ToString();
                        string path = treepath;
                        string qtnname = txtqtnname.Text;
                        qtnname = Regex.Replace(qtnname, "'", "''");
                        string checkquestion = "select question from questionaddition where batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + semester + " and treepath='" + path + "' and collegecode=" + Session["collegecode"].ToString() + "  and question='" + qtnname + "'";
                        DataSet dscheckquestion = new DataSet();
                        dscheckquestion = da.select_method(checkquestion, hat, "Text");
                        if (dscheckquestion.Tables.Count > 0 && dscheckquestion.Tables[0].Rows.Count > 0)
                        {
                            qtnflag = 1;
                        }
                        if (ansflag == 1 && ansfillflag == 0 && qtnflag == 0)
                        {
                            //Technical English (12)/38218/38219
                            //sno++;
                            string insertquery = string.Empty;

                            if (RadioSubject.Checked == true)
                            {
                                insertquery = "insert into questionaddition (batch_year,degree_code,semester,subject_no,treepath,question,choices,correct_ans,toughness,collegecode) values(" + batchyear + "," + degree_code + "," + semester + "," + subject_no + ",'" + path + "','" + qtnname + "','" + nofans + "','" + corretans + "','" + tough + "'," + Session["collegecode"].ToString() + ")";
                            }
                            else if (RadioGeneral.Checked == true)
                            {
                                insertquery = "insert into questionaddition (treepath,question,choices,correct_ans,toughness,collegecode) values('General','" + qtnname + "','" + nofans + "','" + corretans + "','" + tough + "'," + Session["collegecode"].ToString() + ")";
                            }
                            //Modified by srinath12/9/2013
                            int insert = da.update_method_wo_parameter(insertquery, "Text");
                            string ctsname = "Save the Objective Type Information";
                            string entrycode = Session["Entry_Code"].ToString();//saranya
                            string PageName = "Student Attendance";

                            string TimeOfAttendance = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                            string DateOfAttendence = DateTime.Now.ToString("MM/dd/yyy");
                            da.insertUserActionLog(entrycode, batchyear, degree_code, semester, sections, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 1);//saranya
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                            ddlnoofanswers.SelectedIndex = 0;
                            txtqtnname.Text = string.Empty;
                            radiotough1.Checked = true;
                            sprdretrivedate();
                            //bindtree();
                        }
                        else if (qtnflag == 1)
                        {
                            lblnorec.Visible = true;
                            lblnorec.Text = "Question already exist";
                        }
                        else if (ansfillflag != 0)
                        {
                            lblnorec.Visible = true;
                            lblnorec.Text = "Fill all the Answers and Proceed";
                        }
                        else if (ansflag != 1)
                        {
                            lblnorec.Visible = true;
                            lblnorec.Text = "Check any one as correct answer";

                            if (GridView4.Rows.Count < 1)
                            {
                                lblnorec.Visible = true;
                                lblnorec.Text = "Select No of Choices";
                            }
                        }
                    }
                    else
                    {
                        lblnorec.Visible = true;
                        lblnorec.Text = "Enter the question";
                    }
                }
            }
            else
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Select the Topic And Proceed";
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void btnqtnupdate_Click(object sender, EventArgs e)
    {
        try
        {
            lblnorec.Visible = false;
            int ansflag = 0;
            int ansfillflag = 0;
            int qtnflag = 0;
            int actrow1 = 0;
            int actcol1 = 0;
            string sch_dt = string.Empty;
            string degree_code = string.Empty;
            string semester = string.Empty;
            string subject_no = string.Empty;
            string batchyear = string.Empty;
            int.TryParse(Convert.ToString(Session["Row"]), out actrow1);
            int.TryParse(Convert.ToString(Session["Col"]), out actcol1);

            string sub_name = (gridTimeTable.Rows[actrow1].FindControl("lnkPeriod_" + actcol1) as LinkButton).Text;
            string subcode = Convert.ToString((gridTimeTable.Rows[actrow1].FindControl("lblPeriod_" + actcol1) as Label).Text);
            string spdatesp = (gridTimeTable.Rows[actrow1].FindControl("lblDateDisp") as Label).Text;
            sch_dt = spdatesp.ToString();
            if (subcode != "")
            {
                string[] sp1 = subcode.Split(new Char[] { '-' });
                if (sp1.GetUpperBound(0) > 0)
                {
                    degree_code = sp1[0];
                    semester = sp1[1];
                    subject_no = sp1[2];
                    batchyear = sp1[3];
                }
                string[] spdate = sch_dt.Split(new Char[] { '-' });
                if (spdate[0].Length == 1)
                {
                    spdate[0] = "0" + spdate[0];
                }
                if (spdate[1].Length == 1)
                {
                    spdate[1] = "0" + spdate[1];
                }
                string date1 = spdate[1] + "-" + spdate[0] + "-" + spdate[2];
                sch_dt = spdate[0] + "-" + spdate[1] + "-" + spdate[2];


                if (txtqtnname.Text != "")
                {
                    string corretans = string.Empty;
                    for (int res = 1; res < Convert.ToInt32(GridView4.Rows.Count); res++)
                    {
                        bool isval = false;

                        string s = Convert.ToString((GridView4.Rows[res].FindControl("lbloption") as TextBox).Text);
                        isval = (GridView4.Rows[res].FindControl("lbloption") as CheckBox).Checked;
                        CheckBox chk = GridView4.Rows[res].FindControl("lbloption") as CheckBox;
                        if (isval && ansflag == 0)
                        {
                            ansflag = 1;
                            corretans = Convert.ToString((GridView4.Rows[res].FindControl("opttxt") as TextBox).Text);
                            corretans = Regex.Replace(corretans, "'", ".");
                            chk.Checked = false;
                            //sprdnoofchoices.Sheets[0].Cells[res, 1].Value = false;
                        }
                        else if (ansflag == 1 && isval)
                        {
                            chk.Checked = false;
                            //sprdnoofchoices.Sheets[0].Cells[res, 1].Value = false;
                            ansflag = 2;
                            lblnorec.Visible = true;
                            lblnorec.Text = "Select Any one Answer as correct Answer";
                        }
                    }

                    string nofans = string.Empty;
                    for (int row = 0; row < Convert.ToInt32(GridView4.Rows.Count); row++)
                    {
                        string answers = Convert.ToString((GridView4.Rows[row].FindControl("opttxt") as TextBox).Text);
                        if (nofans == "")
                        {
                            nofans = answers;
                        }
                        else
                        {
                            nofans = nofans + "?" + answers;
                        }
                        nofans = Regex.Replace(nofans, "'", ".");
                        if (answers == "")
                        {
                            ansfillflag = 1;
                            lblnorec.Visible = true;
                            lblnorec.Text = "Fill all the Answers and Proceed";
                        }
                    }

                    string tough = string.Empty;
                    if (radiotough1.Checked == true)
                    {
                        tough = "1";
                    }
                    else if (radiotough2.Checked == true)
                    {
                        tough = "2";
                    }
                    else if (radiotough3.Checked == true)
                    {
                        tough = "3";
                    }
                    else if (radiotough4.Checked == true)
                    {
                        tough = "4";
                    }
                    //string criteria = TreeView1.SelectedNode.Text;
                    //string parentcode = TreeView1.SelectedNode.Value;
                    string path = Session["path"].ToString();
                    string qtnname = txtqtnname.Text;
                    qtnname = Regex.Replace(qtnname, "'", "''");
                    string checkquestion = "select question from questionaddition where batch_year=" + batchyear + "and degree_code=" + degree_code + " and semester=" + semester + "and treepath='" + path + "' and collegecode=" + Session["collegecode"].ToString() + "  and question='" + qtnname + "'";
                    //Modified by srinath 12/9/2013
                    // SqlDataAdapter dacheckquestion = new SqlDataAdapter(checkquestion, con1);
                    DataSet dscheckquestion = new DataSet();
                    dscheckquestion = da.select_method(checkquestion, hat, "Text");
                    // con1.Close();
                    //  con1.Open();
                    //dacheckquestion.Fill(dscheckquestion);
                    if (dscheckquestion.Tables.Count > 0 && dscheckquestion.Tables[0].Rows.Count > 0)
                    {
                        qtnflag = 1;
                    }
                    if (ansflag == 1 && ansfillflag == 0 && qtnflag == 0)
                    {
                        if (Session["qtn_no"].ToString() != "")
                        {
                            string updatequery = "update questionaddition set question='" + qtnname + "',choices='" + nofans + "',correct_ans='" + corretans + "',toughness='" + tough + "',collegecode=" + Session["collegecode"].ToString() + " where collegecode=" + Session["collegecode"].ToString() + " and question_no=" + Session["qtn_no"].ToString() + "";
                            //Modified by srinath 12/8/2013
                            int insert = da.update_method_wo_parameter(updatequery, "Text");
                            string ctsname = "Update the Objective Type Information";//saranya
                            string entrycode = Session["Entry_Code"].ToString();
                            string PageName = "Student Attendance";
                            string TimeOfAttendance = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                            string DateOfAttendence = DateTime.Now.ToString("MM/dd/yyy");
                            da.insertUserActionLog(entrycode, batchyear, degree_code, semester, sections, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 2);//saranya


                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);
                            sprdretrivedate();
                            ddlnoofanswers.SelectedIndex = 0;
                            txtqtnname.Text = string.Empty;
                            radiotough1.Checked = true;
                            Session["qtn_no"] = string.Empty;
                            btnqtnupdate.Enabled = false;
                            //btnqtndelete.Enabled = false;
                            btnqtnsave.Enabled = true;

                        }
                    }
                    else if (qtnflag == 1)
                    {
                        lblnorec.Visible = true;
                        lblnorec.Text = "Question already exist";
                    }
                    else if (ansfillflag != 0)
                    {
                        lblnorec.Visible = true;
                        lblnorec.Text = "Fill all the Answers and Proceed";
                    }
                    else if (ansflag != 1)
                    {
                        lblnorec.Visible = true;
                        lblnorec.Text = "Check any one as correct answer";
                        if (GridView4.Rows.Count < 1)
                        {
                            lblnorec.Visible = true;
                            lblnorec.Text = "Select No of Choices";
                        }
                    }
                }
                else
                {
                    lblnorec.Visible = true;
                    lblnorec.Text = "Enter the question";
                }
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    public void sprdretrivedate()
    {
        try
        {
            if (ddlselectmanysub.Items.Count > 0)
            {
                string valsp = ddlselectmanysub.SelectedValue.ToString();
                string[] sp1 = valsp.Split(new Char[] { '-' });
                if (sp1.GetUpperBound(0) > 2)
                {
                    string subcode = sp1[2].ToString();
                    string subname = da.GetFunction("Select Subject_Name from subject where subject_no='" + subcode + "'");
                    selectedpath = subname + " " + "/";
                }
            }
            string treepath = selectedpath;
            if (treepath != "")
            {
                string path = string.Empty;
                int actrow1 = 0;
                int actcol1 = 0;
                string sch_dt = string.Empty;
                string degree_code = string.Empty;
                string semester = string.Empty;
                string subject_no = string.Empty;
                string batchyear = string.Empty;

                int.TryParse(Convert.ToString(Session["Row"]), out actrow1);
                int.TryParse(Convert.ToString(Session["Col"]), out actcol1);
                string tagval = "lblPeriod_" + ac;
                string sub_name = (gridTimeTable.Rows[ar].FindControl(tagval) as Label).Text;
                string subcode = ddlselectmanysub.SelectedValue.ToString();
                string spdatesp = (gridTimeTable.Rows[ar].FindControl("lblDateDisp") as Label).Text;
                //string[] spdatesp = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text.Split(''); 
                sch_dt = spdatesp.ToString();
                // sch_dt = FpSpread1.Sheets[0].RowHeader.Cells[actrow1, 0].Text;
                string[] subject1 = sub_name.Split(new char[] { '2' });
                sub_name = subject1[0].ToString();
                path = treepath + "%";
                if (subcode != "")
                {
                    string[] sp1 = subcode.Split(new Char[] { '-' });
                    if (sp1.GetUpperBound(0) == 7)
                    {
                        degree_code = sp1[0];
                        semester = sp1[1];
                        subject_no = sp1[2];
                        batchyear = sp1[4];
                    }
                    else
                    {
                        degree_code = sp1[0];
                        semester = sp1[1];
                        subject_no = sp1[2];
                        batchyear = sp1[3];
                    }
                    //========================
                    string[] spdate = sch_dt.Split(new Char[] { '-' });
                    if (spdate[0].Length == 1)
                    {
                        spdate[0] = "0" + spdate[0];
                    }
                    if (spdate[1].Length == 1)
                    {
                        spdate[1] = "0" + spdate[1];
                    }
                    string date1 = spdate[1] + "-" + spdate[0] + "-" + spdate[2];
                    sch_dt = spdate[0] + "-" + spdate[1] + "-" + spdate[2];
                    string getdegreecode = "select dept_name,course_name from course,department,degree where degree.dept_code=department.dept_code and degree.course_id=course.course_id and degree.degree_code=" + degree_code + " and degree.college_code=" + Session["collegecode"].ToString() + "";
                    //Modified by srinath 12/9/2013
                    // SqlDataAdapter dagetdegreecode = new SqlDataAdapter(getdegreecode, con1);
                    DataSet dsgetdegreecode = new DataSet();
                    dsgetdegreecode = da.select_method(getdegreecode, hat, "Text");
                    // dagetdegreecode.Fill(dsgetdegreecode);
                    string deptname = string.Empty;
                    string coursename = string.Empty;
                    if (dsgetdegreecode.Tables.Count > 0 && dsgetdegreecode.Tables[0].Rows.Count > 0)
                    {
                        deptname = dsgetdegreecode.Tables[0].Rows[0]["dept_name"].ToString();
                        coursename = dsgetdegreecode.Tables[0].Rows[0]["course_name"].ToString();
                    }
                    int querytype = 3;
                    if (RadioGeneral.Checked == true)
                    {
                        querytype = 0;
                        //selectquery = "select question_no,question,choices,correct_ans from questionaddition where  treepath='General'";
                    }
                    if (RadioSubject.Checked == true)
                    {
                        if (path != "")
                        {
                            querytype = 1;
                        }
                        else
                        {
                            querytype = 2;
                        }
                    }
                    hat.Clear();
                    DataSet dsselectquery = new DataSet();
                    hat.Add("batch_year", Convert.ToInt32(batchyear));
                    hat.Add("degreecode", Convert.ToInt32(degree_code));
                    hat.Add("semester", Convert.ToInt32(semester));
                    hat.Add("subjectno", Convert.ToInt32(subject_no));
                    hat.Add("querytype", querytype);
                    hat.Add("collegecode", Convert.ToInt32(Session["collegecode"].ToString()));
                    hat.Add("path", path.ToString());
                    dsselectquery = da.select_method("questionadditonretrive", hat, "sp");
                    //daselectquery.Fill(dsselectquery);
                    DataTable dtreport = new DataTable();
                    dtreport.Columns.Add("qno");
                    dtreport.Columns.Add("qun");
                    dtreport.Columns.Add("ans");
                    dtreport.Columns.Add("cAns");
                    DataRow dr = null;
                    if (dsselectquery.Tables.Count > 0 && dsselectquery.Tables[0].Rows.Count > 0)
                    {
                        int sno = 0;
                        for (int row = 0; row < dsselectquery.Tables[0].Rows.Count; row++)
                        {
                            dr = dtreport.NewRow();

                            sno++;
                            string qtnno = dsselectquery.Tables[0].Rows[row]["question_no"].ToString();
                            string question = dsselectquery.Tables[0].Rows[row]["question"].ToString();
                            string choices = dsselectquery.Tables[0].Rows[row]["choices"].ToString();
                            string correctans = dsselectquery.Tables[0].Rows[row]["correct_ans"].ToString();
                            string[] choices1 = choices.Split(new char[] { '?' });
                            int choices2 = choices1.GetUpperBound(0) + 1;
                            int sno1 = 0;
                            for (int i = 0; i < choices2; i++)
                            {
                                sno1++;
                                choices = choices1[i].ToString();
                            }

                            dr["qno"] = qtnno;
                            dr["qun"] = question;
                            dr["ans"] = choices;
                            dr["cAns"] = correctans;
                            dtreport.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        GridView5.Visible = false;
                    }
                    if (dtreport.Rows.Count > 0)
                    {
                        GridView5.DataSource = dtreport;
                        GridView5.DataBind();
                        GridView5.Visible = true;
                    }
                    else
                    {
                        GridView5.Visible = true;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void btnsliplist_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
        }
    }
    public void retrievespreadattendancequestion(string batchyear, string degree_code, string semester, string subject_no, string datenotes)
    {
        try
        {
            if (ddlselectmanysub.Items.Count > 0)
            {
                string valsp = ddlselectmanysub.SelectedValue.ToString();
                string[] sp1 = valsp.Split(new Char[] { '-' });
                if (sp1.GetUpperBound(0) > 2)
                {
                    string subcode = sp1[2].ToString();
                    string subname = da.GetFunction("Select Subject_Name from subject where subject_no='" + subcode + "'");
                    selectedpath = subname + " " + "/";
                }
            }
            string getdate = "select date,question,marks,subject_name,subject.subject_no,qtn,treeviewpath from attendance_question_addition,subject where subject.subject_no=attendance_question_addition.subject_no and batch_year=" + batchyear + " and degree_code=" + degree_code + " and semester=" + semester + "and attendance_question_addition.subject_no=" + subject_no + " and treeviewpath like '" + selectedpath + "%' and convert(varchar(20),attendance_question_addition.date,105)='" + datenotes + "' order by qtn asc";
            DataSet dsgetdate = new DataSet();
            dsgetdate = da.select_method(getdate, hat, "Text");
            if (dsgetdate.Tables.Count > 0 && dsgetdate.Tables[0].Rows.Count > 0)
            {
                btnupdatequetion.Enabled = false;
                btnaddquestion.Enabled = true;
                btndeleteatndqtn.Enabled = false;//Added by Srinath 21/8/2013

                string date = string.Empty;
                string subject = string.Empty;
                //string path =string.Empty;
                string question = string.Empty;
                string qtn_no = string.Empty;
                string mark = string.Empty;
                string treepath = string.Empty;
                string subjectno = string.Empty;
                int sno = 0;
                DataTable dtQen = new DataTable();
                dtQen.Columns.Add("Sno");
                dtQen.Columns.Add("bacth");
                dtQen.Columns.Add("Date1");
                dtQen.Columns.Add("degree");
                dtQen.Columns.Add("subject");
                dtQen.Columns.Add("subNo");
                dtQen.Columns.Add("sem");
                dtQen.Columns.Add("unit");
                dtQen.Columns.Add("unitNo");
                dtQen.Columns.Add("qen");
                dtQen.Columns.Add("qno");
                dtQen.Columns.Add("mark");
                DataRow drq = null;

                if (dsgetdate.Tables.Count > 0 && dsgetdate.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsgetdate.Tables[0].Rows.Count; i++)
                    {
                        drq = dtQen.NewRow();
                        sno++;
                        DateTime sa = Convert.ToDateTime(dsgetdate.Tables[0].Rows[i]["date"].ToString());
                        date = dsgetdate.Tables[0].Rows[i]["date"].ToString();
                        string[] spdate = date.Split(new Char[] { '/' });
                        if (spdate[0].Length == 1)
                        {
                            spdate[0] = "0" + spdate[0];
                        }
                        if (spdate[1].Length == 1)
                        {
                            spdate[1] = "0" + spdate[1];
                        }
                        string[] spyear = spdate[2].Split(new char[] { ' ' });
                        string date1 = spdate[1] + "-" + spdate[0] + "-" + spyear[0];
                        subject = dsgetdate.Tables[0].Rows[i]["subject_name"].ToString();
                        question = dsgetdate.Tables[0].Rows[i]["question"].ToString();
                        mark = dsgetdate.Tables[0].Rows[i]["marks"].ToString();
                        qtn_no = dsgetdate.Tables[0].Rows[i]["qtn"].ToString();
                        treepath = dsgetdate.Tables[0].Rows[i]["treeviewpath"].ToString();
                        subjectno = dsgetdate.Tables[0].Rows[i]["subject_no"].ToString();
                        string selecttopic = string.Empty;
                        string[] treepath2 = treepath.Split(new char[] { '=' });
                        if (treepath2.GetUpperBound(0) > 0)
                        {
                            for (int i1 = 0; i1 <= treepath2.GetUpperBound(0); i1++)
                            {
                                string[] treepath1 = treepath2[i1].Split(new char[] { '/' });
                                int maxpath = treepath1.GetUpperBound(0);
                                string topic = string.Empty;
                                string getpathname = string.Empty;
                                if (treepath1.GetUpperBound(0) > 1)
                                {
                                    topic = treepath1[maxpath];
                                    getpathname = "select * from sub_unit_details where subject_no = " + subjectno + " and topic_no=" + topic + " order by topic_no";
                                }
                                else
                                {
                                    topic = treepath1[0];
                                    getpathname = "select * from sub_unit_details where subject_no = " + subjectno + "  order by topic_no";
                                }
                                lblerrorquestionadd_att.Visible = false;
                                //Modified by srinath 12/9/2013
                                //  SqlDataAdapter dagetpathname = new SqlDataAdapter(getpathname, con2);
                                DataSet dsgetpathname = new DataSet();
                                dsgetpathname = da.select_method(getpathname, hat, "Text");
                                if (dsgetpathname.Tables.Count > 0 && dsgetpathname.Tables[0].Rows.Count > 0)
                                {
                                    if (selecttopic == "")
                                    {
                                        selecttopic = dsgetpathname.Tables[0].Rows[0]["unit_name"].ToString();
                                    }
                                    else
                                    {
                                        selecttopic = selecttopic + "," + dsgetpathname.Tables[0].Rows[0]["unit_name"].ToString();
                                    }
                                }
                                if (selecttopic == "")
                                {
                                    selecttopic = treepath1[0];
                                }
                            }
                        }
                        else
                        {
                            string[] treepath1 = treepath.Split(new char[] { '/' });
                            int maxpath = treepath1.GetUpperBound(0);
                            string topic = string.Empty;
                            string getpathname = string.Empty;
                            if (treepath1.GetUpperBound(0) > 1)
                            {
                                topic = treepath1[maxpath];
                                getpathname = "select * from sub_unit_details where subject_no = " + subjectno + " and topic_no=" + topic + " order by topic_no";
                            }
                            else
                            {
                                topic = treepath1[0];
                                getpathname = "select * from sub_unit_details where subject_no = " + subjectno + "  order by topic_no";
                            }
                            lblerrorquestionadd_att.Visible = false;
                            DataSet dsgetpathname = new DataSet();
                            dsgetpathname = da.select_method(getpathname, hat, "Text");
                            if (dsgetpathname.Tables.Count > 0 && dsgetpathname.Tables[0].Rows.Count > 0)
                            {
                                if (selecttopic == "")
                                {
                                    selecttopic = dsgetpathname.Tables[0].Rows[0]["unit_name"].ToString();
                                }
                                else
                                {
                                    selecttopic = selecttopic + "," + dsgetpathname.Tables[0].Rows[0]["unit_name"].ToString();
                                }
                            }
                            if (selecttopic == "")
                            {
                                selecttopic = treepath1[0];
                            }
                        }
                        if (treepath != "" && selecttopic != "")
                        {
                            string strunitsno = da.GetFunction("select subj_unit from attendance_question_addition where qtn='" + qtn_no + "' and date='" + date + "' and subject_no='" + subjectno + "' and degree_code='" + degree_code + "' and batch_year='" + batchyear + "' and semester='" + semester + "'");
                            string strunits = da.GetFunction("select unit_name from sub_unit_details where topic_no='" + strunitsno + "'");

                            drq["Sno"] = Convert.ToString(sno);
                            drq["bacth"] = Convert.ToString(batchyear);
                            drq["Date1"] = Convert.ToString(date1);
                            drq["degree"] = Convert.ToString(degree_code);
                            drq["subject"] = Convert.ToString(subject);
                            drq["subNo"] = Convert.ToString(subject_no);
                            drq["sem"] = Convert.ToString(semester);
                            drq["unit"] = Convert.ToString(strunits);
                            drq["unitNo"] = Convert.ToString(strunitsno);
                            drq["qen"] = Convert.ToString(question);
                            drq["qno"] = Convert.ToString(qtn_no);
                            drq["mark"] = Convert.ToString(mark);
                            dtQen.Rows.Add(drq);

                        }
                        else
                        {
                            lblerrorquestionadd_att.Visible = true;
                            lblerrorquestionadd_att.Text = "Select the Topic and Proceed";
                        }
                    }
                }
                if (dtQen.Rows.Count > 0)
                {
                    GridView6.DataSource = dtQen;
                    GridView6.DataBind();
                    GridView6.Visible = true;
                }

            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void btnupdatequetion_Click(object sender, EventArgs e)
    {
        try
        {
            string unitselected = string.Empty;
            if (txtquestion1.Text != "")
            {
                if (ddlgivemarks.SelectedItem.Text != "")
                {
                    lblerrorquestionadd_att.Visible = false;
                    string activerow = string.Empty;
                    string activecol = string.Empty;

                    if (Convert.ToInt32(activerow) >= 0 && activerow != "")
                    {
                        if (ddlunits.Items.Count > 0)
                        {
                            if (ddlunits.SelectedItem.Text.Trim() != "")
                            {
                                unitselected = ddlunits.SelectedItem.Value.ToString();
                            }
                        }
                        string qtn = "";
                        string Mark = "";
                        string qtn_no = "";
                        string batchyear = "";
                        string degreecode = "";
                        string semester = "";
                        string subject_no = "";
                        string date2 = "";
                        string updatequery = "update attendance_question_addition set question='" + txtquestion1.Text + "', marks=" + ddlgivemarks.SelectedItem.Text + ", subj_unit='" + unitselected + "' where qtn=" + qtn_no + "";
                        //Modified by SRinath 12/9/2013
                        int insert = da.update_method_wo_parameter(updatequery, "Text");


                        string ctsname = "Update the Question";//saranya
                        string entrycode = Session["Entry_Code"].ToString();
                        string PageName = "Student Attendance";

                        string TimeOfAttendance = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                        string DateOfAttendence = DateTime.Now.ToString("MM/dd/yyy");
                        da.insertUserActionLog(entrycode, batchyear, degree_code, semester, sections, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 2);//saranya
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);
                        retrievespreadattendancequestion(batchyear, degreecode, semester, subject_no, date2);
                        txtquestion1.Text = string.Empty;
                        ddlgivemarks.SelectedValue = "0";
                        btnqtnupdate.Enabled = false;
                        btnaddquestion.Enabled = true;
                        btndeleteatndqtn.Enabled = false;
                    }
                }
                else
                {
                    lblerrorquestionadd_att.Visible = true;
                    lblerrorquestionadd_att.Text = "Enter the mark";
                }
            }
            else
            {
                lblerrorquestionadd_att.Visible = true;
                lblerrorquestionadd_att.Text = "Enter the Question";
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void lnkGDelete_click(object sender, EventArgs e)
    {
        try
        {
            string actrow1 = string.Empty;
            int actcol1 = 0;
            LinkButton lnkSelected = (LinkButton)sender;
            string rowIndxS = lnkSelected.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;

            int colIndx = 5;

            actrow1 = Convert.ToString(rowIndx);
            actcol1 = colIndx;

            if (Convert.ToInt32(actrow1) >= 0 && actrow1 != "")
            {
                string qtn = (GridView6.Rows[rowIndx].FindControl("lblQen") as TextBox).Text;
                string Mark = (GridView6.Rows[rowIndx].FindControl("lblMark") as TextBox).Text;
                string batchyear = (GridView6.Rows[rowIndx].FindControl("lblBatch") as Label).Text;
                string degreecode = (GridView6.Rows[rowIndx].FindControl("lblDeg") as Label).Text;
                string semester = (GridView6.Rows[rowIndx].FindControl("lblSem") as Label).Text;
                string subject_no = (GridView6.Rows[rowIndx].FindControl("lblSuNo") as Label).Text;
                string date2 = (GridView6.Rows[rowIndx].FindControl("lblDate") as Label).Text;
                string qtn_no = (GridView6.Rows[rowIndx].FindControl("lblQno") as Label).Text;
                string deletequery = "delete from attendance_question_addition where qtn=" + qtn_no + "";
                int insert = da.update_method_wo_parameter(deletequery, "Text");

                string ctsname = "Delete the Question";

                string entrycode = Session["Entry_Code"].ToString();//saranya
                string PageName = "Student Attendance";

                string TimeOfAttendance = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                string DateOfAttendence = DateTime.Now.ToString("MM/dd/yyy");
                da.insertUserActionLog(entrycode, batchyear, degree_code, semester, sections, TimeOfAttendance, DateOfAttendence, PageName, ctsname, 3);//saranya
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
                retrievespreadattendancequestion(batchyear, degreecode, semester, subject_no, date2);
                btnqtnupdate.Enabled = false;
                btnaddquestion.Enabled = true;
                btndeleteatndqtn.Enabled = false;

            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void rbappenses_checkchange(object sender, EventArgs e)
    {
        Slipentry.Visible = false;
        appentiesentry();
    }
    public void appentiesentry()
    {
        try
        {
            GridView1.Visible = false;
            Buttonselectall.Visible = false;
            Buttondeselect.Visible = false;
            Buttonsave.Visible = false;
            //Buttonupdate.Visible = false;
            lblmanysubject.Visible = false;
            ddlselectmanysub.Visible = false;
            lblatdate.Visible = true;
            lblcurdate.Visible = true;
            lblhour.Visible = true;
            lblhrvalue.Visible = true;
            lblattend.Visible = true;
            ddlattend.Visible = true;
            Slipentry.Visible = true;
            //btnaddrow.Visible = true;
            //fpattendanceentry.Visible = true;
            Gridview2.Visible = true;
            lblreststudent.Visible = true;
            ddlreststudent.Visible = true;
            lblerrmsg.Visible = true;
            btnaddattendance.Visible = true;
            fieldat.Visible = true;
            lblerrmsg.Visible = false;
            string hour = string.Empty;
            int atar = Convert.ToInt32(Session["Row"].ToString());
            int atac = Convert.ToInt32(Session["Col"].ToString());

            if (atar > -1 && atar > -1)
            {
                lblcurdate.Text = (gridTimeTable.Rows[atar].FindControl("lblDateDisp") as Label).Text;
                lblhrvalue.Text = atac.ToString();
                ddlattend.Items.Clear();
                ddlreststudent.Items.Clear();
                string odrights = da.GetFunction("select rights from OD_Master_Setting where " + grouporusercode + "");
                if (odrights.Trim() != null && odrights.Trim() != "" && odrights.Trim() != "0")
                {
                    string od_rights = string.Empty;
                    od_rights = odrights;
                    string[] split_od_rights = od_rights.Split(',');
                    ddlattend.Items.Add(" ");
                    ddlreststudent.Items.Add(" ");
                    for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                    {
                        string value = split_od_rights[od_temp];
                        ddlattend.Items.Add("" + value + " ");
                        ddlreststudent.Items.Add("" + value + " ");
                    }
                }
                else
                {
                    ddlreststudent.Items.Add(" ");
                    ddlreststudent.Items.Add("P");
                    ddlreststudent.Items.Add("A");
                    ddlreststudent.Items.Add("OD ");
                    ddlreststudent.Items.Add("SOD");
                    ddlreststudent.Items.Add("ML");
                    ddlreststudent.Items.Add("NSS");
                    ddlreststudent.Items.Add("L");
                    ddlreststudent.Items.Add("NCC");
                    ddlreststudent.Items.Add("HS");
                    ddlreststudent.Items.Add("PP");
                    ddlreststudent.Items.Add("SYOD");
                    ddlreststudent.Items.Add("COD");
                    ddlreststudent.Items.Add("OOD");
                    ddlreststudent.Items.Add("LA");

                    ddlattend.Items.Add(" ");
                    ddlattend.Items.Add("P");
                    ddlattend.Items.Add("A");
                    ddlattend.Items.Add("OD ");
                    ddlattend.Items.Add("SOD");
                    ddlattend.Items.Add("ML");
                    ddlattend.Items.Add("NSS");
                    ddlattend.Items.Add("L");
                    ddlattend.Items.Add("NCC");
                    ddlattend.Items.Add("HS");
                    ddlattend.Items.Add("PP");
                    ddlattend.Items.Add("SYOD");
                    ddlattend.Items.Add("COD");
                    ddlattend.Items.Add("OOD");
                    ddlattend.Items.Add("LA");
                }
                Gridview2.Visible = true;
                DataTable dt = new DataTable();
                DataRow dr = null;

                dt.Columns.Add(new DataColumn("Column1", typeof(string)));
                dt.Columns.Add(new DataColumn("Column2", typeof(string)));
                dr = dt.NewRow();
                dr["Column1"] = string.Empty;
                dr["Column2"] = string.Empty;
                dt.Rows.Add(dr);
                //dr = dt.NewRow();
                //Store the DataTable in ViewState
                ViewState["CurrentTable"] = dt;
                Gridview2.DataSource = dt;

                Gridview2.DataBind();
                //fpattendanceentry.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void btnaddattendance_Click(object sender, EventArgs e)
    {
        try
        {
            rbgraphics.Checked = false;
            rbappenses.Checked = true;
            lblerrmsg.Visible = false;
            string colU = Convert.ToString(Session["Col"]);
            string studroll = string.Empty;
            string rollprefix = string.Empty;
            string setattandance = ddlattend.SelectedItem.ToString().Trim();
            string setrestattendance = ddlreststudent.SelectedItem.ToString().Trim();
            Hashtable hatrestroll = new Hashtable();
            Hashtable hatinvalidroll = new Hashtable();
            bool entryfalag = false;
            hatinvalidroll.Clear();
            hatrestroll.Clear();
            //string strinvalidroll =string.Empty;
            if (setattandance.Trim() != "" && setattandance.Trim() != null && setattandance.Trim() != "-1")
            {
                for (int j = 0; j < GridView1.Rows.Count; j++)
                {
                    studroll = (GridView1.Rows[j].FindControl("lblrollNo") as Label).Text;
                    if (!hatroll.Contains(studroll.Trim().ToLower()))
                    {
                        hatroll.Add(studroll.Trim().ToLower(), j);
                    }
                }

                if (Gridview2.Rows.Count > 0)
                {
                    for (int i = 0; i < Gridview2.Rows.Count; i++)
                    {
                        rollprefix = (Gridview2.Rows[i].FindControl("TextBox1") as TextBox).Text;
                        string prefixrollno = (Gridview2.Rows[i].FindControl("TextBox2") as TextBox).Text;
                        if (rollprefix.Trim() != null && rollprefix.Trim() != "" && prefixrollno != null && prefixrollno.Trim() != "")
                        {
                            string[] prerollno = prefixrollno.Split(',');

                            for (int j = 0; j <= prerollno.GetUpperBound(0); j++)
                            {
                                //for (int col = 7; col < FpSpread2.Sheets[0].ColumnCount; col = col + 2)
                                //{
                                studroll = rollprefix + prerollno[j].ToString().Trim().ToLower();
                                if (hatroll.Contains(studroll.Trim().ToLower()))
                                {
                                    entryfalag = true;
                                    string rowvalue = GetCorrespondingKey(studroll.Trim().ToLower(), hatroll).ToString();
                                    if (colU == "1")
                                    {

                                        if (rowvalue != "Entered" && (GridView1.Rows[Convert.ToInt32(rowvalue)].FindControl("ddlLeavetype") as DropDownList).SelectedItem.Text != "S" && (GridView1.Rows[Convert.ToInt32(rowvalue)].FindControl("ddlLeavetype") as DropDownList).SelectedItem.Text.ToLower().Trim() != "od")
                                        {
                                            int row = Convert.ToInt32(rowvalue);
                                            DropDownList ddlle = (GridView1.Rows[Convert.ToInt32(rowvalue)].FindControl("ddlLeavetype") as DropDownList);
                                            ddlle.ClearSelection();
                                            ddlle.Items.FindByText(setattandance.Trim()).Selected = true;
                                            hatroll[studroll] = "Entered";
                                        }
                                    }
                                    else
                                    {

                                        if (rowvalue != "Entered" && (GridView1.Rows[Convert.ToInt32(rowvalue)].FindControl("ddlLeavetype" + colU) as DropDownList).SelectedItem.Text != "S" && (GridView1.Rows[Convert.ToInt32(rowvalue)].FindControl("ddlLeavetype" + colU) as DropDownList).SelectedItem.Text.ToLower().Trim() != "od")
                                        {
                                            int row = Convert.ToInt32(rowvalue);
                                            DropDownList ddlle = (GridView1.Rows[Convert.ToInt32(rowvalue)].FindControl("ddlLeavetype" + colU) as DropDownList);
                                            ddlle.ClearSelection();
                                            ddlle.Items.FindByText(setattandance.Trim()).Selected = true;
                                            hatroll[studroll] = "Entered";
                                        }
                                    }
                                }
                                else
                                {
                                    if (!hatinvalidroll.Contains(studroll.ToLower()))
                                    {
                                        hatinvalidroll.Add(studroll, studroll);
                                    }
                                }
                                //}
                            }
                        }
                    }

                    if (setrestattendance.Trim() != "" && setrestattendance.Trim() != null && setrestattendance.Trim() != "-1")
                    {
                        for (int j = 0; j < GridView1.Rows.Count; j++)
                        {
                            studroll = (GridView1.Rows[j].FindControl("lblrollNo") as Label).Text;
                            if (hatroll.Contains(studroll.Trim().ToLower()))
                            {
                                //for (int col = 7; col < FpSpread2.Sheets[0].ColumnCount; col = col + 2)
                                //{
                                string restroll = GetCorrespondingKey(studroll.Trim().ToLower(), hatroll).ToString();
                                if (colU == "1")
                                {
                                    if (restroll != "Entered" && (GridView1.Rows[Convert.ToInt32(restroll)].FindControl("ddlLeavetype") as DropDownList).SelectedItem.Text != "S" && (GridView1.Rows[Convert.ToInt32(restroll)].FindControl("ddlLeavetype") as DropDownList).SelectedItem.Text.ToLower().Trim() != "od")
                                    {
                                        DropDownList ddlle = (GridView1.Rows[Convert.ToInt32(restroll)].FindControl("ddlLeavetype") as DropDownList);
                                        ddlle.ClearSelection();
                                        ddlle.Items.FindByText(setrestattendance.Trim()).Selected = true;
                                    }
                                }
                                else
                                {
                                    if (restroll != "Entered" && (GridView1.Rows[Convert.ToInt32(restroll)].FindControl("ddlLeavetype" + colU) as DropDownList).SelectedItem.Text != "S" && (GridView1.Rows[Convert.ToInt32(restroll)].FindControl("ddlLeavetype" + colU) as DropDownList).SelectedItem.Text.ToLower().Trim() != "od")
                                    {
                                        DropDownList ddlle = (GridView1.Rows[Convert.ToInt32(restroll)].FindControl("ddlLeavetype" + colU) as DropDownList);
                                        ddlle.ClearSelection();
                                        ddlle.Items.FindByText(setrestattendance.Trim()).Selected = true;
                                    }
                                }
                                //}
                            }
                        }
                    }
                    if (hatinvalidroll.Count > 0)
                    {
                        foreach (DictionaryEntry parameter1 in hatinvalidroll)
                        {
                            if (strinvalidroll == "")
                            {
                                strinvalidroll = (parameter1.Key).ToString();
                            }
                            else
                            {
                                strinvalidroll = strinvalidroll + " , " + (parameter1.Key).ToString();
                            }
                        }
                    }
                    if (entryfalag == true)
                    {
                        Buttonsave_Click(sender, e);
                        appentiesentry();
                    }
                    else
                    {
                        lblerrmsg.Visible = true;
                        lblerrmsg.Text = "No Student Match";
                    }
                }
                else
                {
                    lblerrmsg.Visible = true;
                    lblerrmsg.Text = "Please Add Row";
                }
            }
            else
            {
                lblerrmsg.Visible = true;
                lblerrmsg.Text = "Please Enter Selected Students Attendance";
            }

        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void addnewrow(object sender, EventArgs e)
    {
        AddNewRowToGrid();
    }
    private void AddNewRowToGrid()
    {
        try
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {

                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];

                DataRow drCurrentRow = null;

                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {

                        //extract the TextBox values

                        TextBox box1 = (TextBox)Gridview2.Rows[rowIndex].Cells[0].FindControl("TextBox1");
                        TextBox box2 = (TextBox)Gridview2.Rows[rowIndex].Cells[1].FindControl("TextBox2");

                        drCurrentRow = dtCurrentTable.NewRow();
                        dtCurrentTable.Rows[i - 1]["Column1"] = box1.Text;
                        dtCurrentTable.Rows[i - 1]["Column2"] = box2.Text;
                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;
                    Gridview2.DataSource = dtCurrentTable;
                    Gridview2.DataBind();

                }

            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    public void mark_attendance2()
    {
        try
        {
            DataTable dtreson = getAbReasons();
            string Discon = da.GetFunction("select value from Master_Settings where settings='Attendance Discount' and  " + grouporusercode + "");
            string debar = da.GetFunction("select value from Master_Settings where settings='Attendance Debar' and  " + grouporusercode + "");
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

            int col = Convert.ToInt32(Session["Col"].ToString());
            int row = Convert.ToInt32(Session["Row"].ToString());
            string hr = string.Empty;
            string Date1 = string.Empty;
            int findHR = 1;
            for (int cell = 8; cell < GridView1.Columns.Count; cell += 2)
            {
                if (GridView1.Columns[cell].Visible == true)
                {
                    string hour = (GridView1.HeaderRow.FindControl("lbl" + findHR) as Label).Text;
                    hr = Convert.ToString(hour.Split(' ')[1]);
                    Date1 = (gridTimeTable.Rows[0].FindControl("lblDate") as Label).Text;
                    break;
                }
                findHR++;
            }

            string PerTag = (gridTimeTable.Rows[row].FindControl("lblPeriod_" + hr) as Label).Text;//lblDateDisp
            string Curtag = (gridTimeTable.Rows[row].FindControl("lblPeriod_" + col) as Label).Text;
            string Date2 = (gridTimeTable.Rows[row].FindControl("lblDateDisp") as Label).Text;

            if (PerTag.Contains("alter"))
                PerTag = PerTag.Replace("alter", "sem");
            if (Curtag.Contains("alter"))
                Curtag = Curtag.Replace("alter", "sem");

            if (PerTag.Trim() == Curtag.Trim())//&& Date1 == Date2
            {

                for (int jj = 0; jj < GridView1.Rows.Count; jj++)
                {
                    DropDownList attMark;
                    DropDownList ddlReson;
                    DropDownList ddlHeader;

                    if (col == 1)
                    {
                        ddlReson = (GridView1.Rows[jj].FindControl("ddlReson") as DropDownList);
                        attMark = (GridView1.Rows[jj].FindControl("ddlLeavetype") as DropDownList);
                        attMark.Items.FindByText("A").Selected = true;
                    }
                    else
                    {
                        ddlReson = (GridView1.Rows[jj].FindControl("ddlReson" + col) as DropDownList);
                        attMark = (GridView1.Rows[jj].FindControl("ddlLeavetype" + col) as DropDownList);
                        //ddlHeader = (GridView1.HeaderRow.FindControl("ddlLeavetype" + col) as DropDownList);
                    }
                    attMark.Items.Clear();
                    ddlReson.Items.Clear();
                    if (dtreson.Rows.Count > 0)
                    {
                        ddlReson.DataSource = dtreson;
                        ddlReson.Width = 120;
                        ddlReson.DataValueField = "TextCode";
                        ddlReson.DataTextField = "Textval";
                        ddlReson.DataBind();
                        ddlReson.Items.Insert(0, " ");
                    }
                    //ddlHeader.Items.Clear();
                    string[] strcomo = new string[20];
                    string[] attnd_rights1 = new string[100];
                    int i = 0;
                    string odrights = da.GetFunction("select rights from  OD_Master_Setting where " + grouporusercode + "");

                    if (odrights.Trim() != null && odrights.Trim() != "" && odrights.Trim() != "0")
                    {
                        string od_rights = string.Empty;
                        od_rights = odrights;
                        string[] split_od_rights = od_rights.Split(',');
                        strcomo = new string[split_od_rights.GetUpperBound(0) + 2];
                        strcomo[i++] = string.Empty;

                        for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                        {
                            attMark.Items.Add(Convert.ToString(split_od_rights[od_temp]));
                            strcomo[i++] = split_od_rights[od_temp].ToString();
                        }
                        attMark.Items.Insert(0, " ");
                    }
                    else
                    {
                        string[] value = { " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD", "LA" };

                        strcomo[0] = string.Empty;

                        for (int od_temp = 0; od_temp < value.Length; od_temp++)
                        {
                            attMark.Items.Add(Convert.ToString(value[od_temp]));
                        }
                    }
                    if (dtreson.Rows.Count > 0)
                    {
                        ddlReson.Width = 120;
                        ddlReson.DataSource = dtreson;
                        //ddlReson.DataSource = getAbReasons();
                        ddlReson.DataTextField = "Textval";
                        ddlReson.DataValueField = "TextCode";
                        ddlReson.DataBind();
                        ddlReson.Items.Insert(0, " ");
                    }
                    //attMark.Items.FindByText("A").Selected = true;
                }


                GetFeeOfRollStudent(ref dicFeeOfRollStudents, ref dicFeeOnRollStudents, Convert.ToString(Convert.ToDateTime(sel_date).ToString("dd-MM-yyyy")));
                chk = daycheck(Convert.ToDateTime(sel_date));
                if (chk == false)
                {
                    //FpSpread1.Visible = false;
                    Buttonsave.Visible = false;
                    headerpanelhomework.Visible = false;
                    //btnaddhme.Visible = false;
                    //Buttonupdate.Visible = false;
                    pHeaderatendence.Visible = false;
                    pHeaderlesson.Visible = false;
                    pBodyatendence.Visible = false;
                    pBodylesson.Visible = false;
                    pBodynotes.Visible = false;
                    pBodyquestionaddition.Visible = false;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = " You cannot edit this day/Hour attendance due to security reasons. Get permission from PRINCIPAL to update the attendance";
                }
                else
                {
                    string temp_date = Convert.ToString((gridTimeTable.Rows[ar].FindControl("lblDateDisp") as Label).Text);
                    sel_date = temp_date;
                    string querytext;
                    int Att_mark_row;
                    string str_Date;
                    string str_day;
                    string Atmonth;
                    string Atyear;
                    long strdate;
                    string Att_str_hour;
                    string rollno_Att = string.Empty;
                    string Att_dcolumn = string.Empty;
                    string Att_strqueryst = string.Empty;
                    string Att_Markvalue;
                    string Att_Mark1;
                    int temp = 0;
                    string[] split_tag_val;
                    //Increase Speed for load Attendance Modified by srinath 23/9/2013
                    if (singlesubject == true)
                    {
                        split_tag_val = Convert.ToString(singlesubjectno).Split('*');
                    }
                    else
                    {
                        split_tag_val = getcelltag.Split('*');
                    }

                    string get_alter_or_sem = string.Empty;
                    bool tag_flag = false;
                    string byear = string.Empty;
                    string strorder = filterfunction();

                    for (int tag_for = 0; tag_for <= split_tag_val.GetUpperBound(0); tag_for++)
                    {
                        string tempdegree =
                            str = split_tag_val[tag_for].ToString();
                        if (str != "")
                        {
                            string[] sp1 = str.Split(new Char[] { '-' });
                            if (sp1[0].ToString().Trim() != "Selected day is Holiday" && sp1[0].ToString().Trim() != "")
                            {
                                if (sp1.GetUpperBound(0) == 7)
                                {
                                    sections = sp1[3];
                                    byear = sp1[4];
                                    subj_count_in_onehr = sp1[6];
                                    get_alter_or_sem = sp1[7];
                                    degree_code = sp1[0];
                                    subject_no = sp1[2];
                                    semester = sp1[1];
                                }
                                else
                                {
                                    degree_code = sp1[0];
                                    sections = string.Empty;
                                    byear = sp1[3];
                                    subj_count_in_onehr = sp1[5];
                                    get_alter_or_sem = sp1[6];
                                    subject_no = sp1[2];
                                    semester = sp1[1];
                                }
                                bool hrlock = Hour_lock(degree_code, byear, semester, getcolheader, sections);  //aruna 23july2013
                                if (hrlock == true)
                                {
                                    headerpanelhomework.Visible = false;
                                    //btnaddhme.Visible = false;
                                    Buttonsave.Visible = false;
                                    //Buttonupdate.Visible = false;
                                    pHeaderatendence.Visible = false;
                                    pHeaderlesson.Visible = false;
                                    headerpanelnotes.Visible = false;
                                    pBodyatendence.Visible = false;
                                    pBodylesson.Visible = false;
                                    pBodynotes.Visible = false;
                                    pBodyquestionaddition.Visible = false;
                                    headerquestionaddition.Visible = false;
                                    headerADDQuestion.Visible = false;
                                    lbl_alert.Visible = true;
                                    lbl_alert.Text = "You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator";
                                    GridView1.Visible = false;
                                    Buttondeselect.Visible = false;
                                    Buttonselectall.Visible = false;
                                    lblmanysubject.Visible = false;
                                    ddlselectmanysub.Visible = false;
                                    return;
                                }
                                string check_lab = da.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subject_no + "'");
                                if (check_lab == "1" || check_lab == "True" || check_lab == "TRUE" || check_lab == "true")
                                {
                                    subj_type = "L";
                                }
                                else
                                {
                                    subj_type = "S";
                                }

                                Att_str_hour = col.ToString();
                                string getdateact = temp_date;
                                str_Date = temp_date;
                                string[] split = str_Date.Split(new Char[] { '-' });
                                str_day = split[0].ToString();
                                Atmonth = split[1].ToString();
                                Atyear = split[2].ToString();
                                strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                                Att_dcolumn = "d" + str_day + "d" + Att_str_hour;
                                string concat_susdate = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                                Session["StaffSelector"] = "0";  //Session["collegecode"].ToString()
                                string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString() + "'");
                                string[] splitminimumabsentsms = minimumabsentsms.Split('-');
                                if (splitminimumabsentsms.Length == 2)
                                {
                                    int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                                    if (splitminimumabsentsms[0].ToString() == "1")
                                    {
                                        if (Convert.ToInt32(byear) >= batchyearsetting)
                                        {
                                            Session["StaffSelector"] = "1";
                                        }
                                    }
                                }
                                string strstaffselector = string.Empty;
                                if (Session["StaffSelector"].ToString() == "1")
                                {
                                    strstaffselector = " and subjectchooser.staffcode like '%" + Session["Staff_Code"].ToString() + "%'";
                                }
                                string attendancequery = "Select a." + Att_dcolumn + " as Attendance, registration.roll_no,registration.reg_no ,registration.stud_name,registration.serialno from registration,SubjectChooser s,applyn,Attendance a  where registration.roll_no = s.roll_no and  s.Semester=registration.current_semester and registration.app_no=applyn.app_no and s.roll_no=a.roll_no and a.roll_no=registration.Roll_No and registration.RollNo_Flag<>0 and registration.cc=0 " + dis + "" + deba + "";
                                attendancequery = attendancequery + " and registration.Batch_year='" + byear + "' and registration.Degree_Code = " + degree_code + " and registration.current_semester = '" + semester + "' and s.Subject_No = '" + subject_no.ToString() + "'and s.Semester = '" + semester + "'  " + strsec + " " + Session["strvar"].ToString() + "and adm_date<='" + concat_susdate + "'  and  a.month_year=" + strdate + " " + strorder + "";
                                DataSet dsattendance = da.select_method(attendancequery, hat, "Text");
                                Hashtable hatroll = new Hashtable();
                                if (dsattendance.Tables.Count > 0 && dsattendance.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < dsattendance.Tables[0].Rows.Count; i++)
                                    {
                                        if (dsattendance.Tables[0].Rows[i]["attendance"].ToString().Trim() != null && dsattendance.Tables[0].Rows[i]["attendance"].ToString().Trim() != "")
                                        {
                                            if (!hatroll.Contains(dsattendance.Tables[0].Rows[i]["Roll_no"].ToString().Trim().ToLower()))
                                            {
                                                hatroll.Add(dsattendance.Tables[0].Rows[i]["Roll_no"].ToString().Trim().ToLower(), dsattendance.Tables[0].Rows[i]["Attendance"].ToString());
                                            }
                                        }
                                    }
                                }
                                Hashtable hatreason = new Hashtable();
                                string attendacereason = "Select a." + Att_dcolumn + " as Reason, registration.roll_no,registration.reg_no ,registration.stud_name,registration.serialno from registration,SubjectChooser s,applyn,Attendance_withreason a  where registration.roll_no = s.roll_no and  s.Semester=registration.current_semester and registration.app_no=applyn.app_no and s.roll_no=a.roll_no and a.roll_no=registration.Roll_No and registration.RollNo_Flag<>0 and registration.cc=0 " + dis + "" + deba + " and a." + Att_dcolumn + "<>'' and a." + Att_dcolumn + " is not null";
                                attendacereason = attendacereason + " and registration.Batch_year='" + byear + "' and registration.Degree_Code = " + degree_code + " and registration.current_semester = '" + semester + "' and s.Subject_No = '" + subject_no.ToString() + "'and s.Semester = '" + semester + "'  " + strsec + " " + Session["strvar"].ToString() + "and adm_date<='" + concat_susdate + "'  and  a.month_year=" + strdate + " " + strorder + "";
                                DataSet daattreason = da.select_method(attendacereason, hat, "Text");

                                DataTable dtOnduty = dirAcc.selectDataTable("select * from Onduty_Stud od where   (convert(datetime,od.fromdate,105) >= '" + concat_susdate + "' or  convert(datetime,od.Todate,105)>='" + concat_susdate + "') and  (convert(datetime,od.fromdate,105) <='" + concat_susdate + "' or convert(datetime,od.Todate,105)<= '" + concat_susdate + "')");

                                if (daattreason.Tables.Count > 0 && daattreason.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < daattreason.Tables[0].Rows.Count; i++)
                                    {
                                        if (daattreason.Tables[0].Rows[i]["Reason"].ToString().Trim() != null && daattreason.Tables[0].Rows[i]["Reason"].ToString().Trim() != "")
                                        {
                                            if (!hatreason.Contains(daattreason.Tables[0].Rows[i]["Roll_no"].ToString().Trim().ToLower()))
                                            {
                                                hatreason.Add(daattreason.Tables[0].Rows[i]["Roll_no"].ToString().Trim().ToLower(), daattreason.Tables[0].Rows[i]["Reason"].ToString());
                                            }
                                        }
                                    }
                                }
                                string strsuspend = "select  convert(varchar(15),dateadd(day,tot_days-1,ack_date),1) as action_days,ack_date,tot_days,s.roll_no from stucon s,Registration where Registration.Roll_No=s.roll_no and registration.Batch_year='" + byear + "' and Registration.Degree_Code = " + degree_code + " and Registration.current_semester = '" + semester + "'  and ack_susp=1 and tot_days>0 and Registration.CC=0 " + dis + "" + deba + " and ack_date<='" + concat_susdate + "'";
                                DataSet dssuspend = da.select_method(strsuspend, hat, "Text");
                                Hashtable hatsuspend = new Hashtable();
                                if (dssuspend.Tables.Count > 0 && dssuspend.Tables[0].Rows.Count > 0)
                                {
                                    for (int s = 0; s < dssuspend.Tables[0].Rows.Count; s++)
                                    {
                                        if (!hatsuspend.Contains(dssuspend.Tables[0].Rows[s]["Roll_no"].ToString().Trim().ToLower()))
                                        {
                                            hatsuspend.Add(dssuspend.Tables[0].Rows[s]["Roll_no"].ToString().Trim().ToLower(), dssuspend.Tables[0].Rows[s]["action_days"].ToString() + '^' + dssuspend.Tables[0].Rows[s]["ack_date"].ToString() + '^' + dssuspend.Tables[0].Rows[s]["tot_days"].ToString());
                                        }
                                    }
                                }

                                for (Att_mark_row = 0; Att_mark_row < GridView1.Rows.Count; Att_mark_row++)
                                {
                                    {
                                        //ddlReson = (GridView1.Rows[Att_mark_row].FindControl("ddlReson" + col) as DropDownList);
                                        rollno_Att = Convert.ToString((GridView1.Rows[Att_mark_row].FindControl("lblrollNo") as Label).Text);
                                        DropDownList ddl;
                                        DropDownList ddlReson;
                                        if ((GridView1.Rows[Att_mark_row].BackColor == Color.Red)) // 30-12-2016 (Jayaraman)
                                        {

                                            if (col == 1)
                                            {
                                                ddl = (GridView1.Rows[Att_mark_row].FindControl("ddlLeavetype") as DropDownList);
                                                ddlReson = (GridView1.Rows[Att_mark_row].FindControl("ddlReson") as DropDownList);
                                            }
                                            else
                                            {
                                                ddl = (GridView1.Rows[Att_mark_row].FindControl("ddlLeavetype" + col) as DropDownList);
                                                ddlReson = (GridView1.Rows[Att_mark_row].FindControl("ddlReson" + col) as DropDownList);
                                            }

                                            ListItem item = ddl.Items.FindByText("A");
                                            if (item == null)
                                            {
                                                ddl.Items.Insert(0, "A");
                                                ddl.Enabled = false;
                                            }
                                            else
                                                ddl.Items.FindByText("A").Selected = true;

                                            GridView1.Rows[Att_mark_row].Enabled = false;


                                        }
                                        else
                                        {
                                            if (hatsuspend.Contains(rollno_Att.Trim().ToLower()))
                                            {
                                                DateTime dt_curr = Convert.ToDateTime(concat_susdate.ToString());
                                                string values = GetCorrespondingKey(rollno_Att.Trim().ToLower(), hatsuspend).ToString();
                                                if (values.Trim() != null && values.Trim() != "")
                                                {
                                                    string[] strspiltvalues = values.Split('^');
                                                    if (strspiltvalues.GetUpperBound(0) > 1)
                                                    {
                                                        string actiondate = strspiltvalues[0].ToString();
                                                        string ackdate = strspiltvalues[1].ToString();
                                                        long totalactdays = Convert.ToInt32(strspiltvalues[2].ToString());
                                                        DateTime dt_act = Convert.ToDateTime(actiondate);
                                                        DateTime dt_curr1 = Convert.ToDateTime(ackdate);
                                                        // DateTime dt_act = Convert.ToDateTime(ds_suspend.Tables[0].Rows[0]["action_days"].ToString());
                                                        TimeSpan t_con = dt_act.Subtract(dt_curr);
                                                        long daycon = t_con.Days;
                                                        // DateTime dt_curr1 = Convert.ToDateTime(ds_suspend.Tables[0].Rows[0]["ack_date"].ToString());
                                                        DateTime dt_act1 = Convert.ToDateTime(concat_susdate.ToString());
                                                        TimeSpan t_con1 = dt_act1.Subtract(dt_curr1);
                                                        long daycon1 = t_con1.Days;

                                                        if ((Convert.ToInt32(daycon + daycon1) == totalactdays - 1) && (daycon > 0))
                                                        {

                                                            if (col == 1)
                                                            {
                                                                ddl = (GridView1.Rows[Att_mark_row].FindControl("ddlLeavetype") as DropDownList);
                                                                ddlReson = (GridView1.Rows[Att_mark_row].FindControl("ddlReson") as DropDownList);
                                                            }
                                                            else
                                                            {
                                                                ddl = (GridView1.Rows[Att_mark_row].FindControl("ddlLeavetype" + col) as DropDownList);
                                                                ddlReson = (GridView1.Rows[Att_mark_row].FindControl("ddlReson" + col) as DropDownList);
                                                            }
                                                            //ddl.Items.FindByText("S").Selected = true;
                                                            GridView1.Rows[Att_mark_row].Enabled = false;

                                                        }
                                                        else
                                                        {

                                                            if (hatroll.Contains(rollno_Att.Trim().ToLower()))
                                                            {
                                                                Att_Markvalue = GetCorrespondingKey(rollno_Att.Trim().ToLower(), hatroll).ToString();
                                                                Att_Markvalue = da.GetFunction(Att_strqueryst);
                                                                Att_Mark1 = Attmark(Att_Markvalue);

                                                                if (col == 1)
                                                                {
                                                                    ddl = (GridView1.Rows[Att_mark_row].FindControl("ddlLeavetype") as DropDownList);
                                                                    ddlReson = (GridView1.Rows[Att_mark_row].FindControl("ddlReson") as DropDownList);
                                                                }
                                                                else
                                                                {
                                                                    ddl = (GridView1.Rows[Att_mark_row].FindControl("ddlLeavetype" + col) as DropDownList);
                                                                    ddlReson = (GridView1.Rows[Att_mark_row].FindControl("ddlReson" + col) as DropDownList);
                                                                }
                                                                ListItem list = ddl.Items.FindByText(Att_Mark1);
                                                                if (list == null)
                                                                {
                                                                    ddl.Items.Insert(0, Att_Mark1);
                                                                    ddl.Enabled = false;
                                                                }
                                                                else
                                                                    ddl.Items.FindByText(Att_Mark1.ToString()).Selected = true;

                                                                GridView1.Rows[Att_mark_row].Enabled = false;


                                                                if (Att_Mark1 != "")
                                                                {
                                                                    temp = temp + 1;
                                                                }
                                                            }
                                                            if (hatreason.Contains(rollno_Att.Trim().ToLower()))
                                                            {
                                                                Att_Markvalue = GetCorrespondingKey(rollno_Att.Trim().ToLower(), hatreason).ToString();
                                                                Att_Mark1 = Attmark(Att_Markvalue);
                                                                if (col == 1)
                                                                {
                                                                    ddl = (GridView1.Rows[Att_mark_row].FindControl("ddlLeavetype") as DropDownList);
                                                                    ddlReson = (GridView1.Rows[Att_mark_row].FindControl("ddlReson") as DropDownList);
                                                                }
                                                                else
                                                                {
                                                                    ddl = (GridView1.Rows[Att_mark_row].FindControl("ddlLeavetype" + col) as DropDownList);
                                                                    ddlReson = (GridView1.Rows[Att_mark_row].FindControl("ddlReson" + col) as DropDownList);
                                                                }
                                                                ListItem item = ddlReson.Items.FindByText(Att_Mark1.ToString());
                                                                if (item == null)
                                                                {
                                                                    ddlReson.Items.Insert(0, Att_Mark1.ToString());
                                                                    ddlReson.Enabled = false;
                                                                }
                                                                else
                                                                    ddlReson.Items.FindByText(Att_Mark1.ToString()).Selected = true;
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                            //End ===========================
                                            else //if the student does not have suspension this part will work
                                            {

                                                if (hatroll.Contains(rollno_Att.Trim().ToLower()))
                                                {
                                                    Att_Markvalue = GetCorrespondingKey(rollno_Att.Trim().ToLower(), hatroll).ToString();
                                                    Att_Mark1 = Attmark(Att_Markvalue);
                                                    if (col == 1)
                                                    {
                                                        ddl = (GridView1.Rows[Att_mark_row].FindControl("ddlLeavetype") as DropDownList);
                                                        //ddlReson = (GridView1.Rows[Att_mark_row].FindControl("ddlReson") as DropDownList);
                                                    }
                                                    else
                                                    {
                                                        ddl = (GridView1.Rows[Att_mark_row].FindControl("ddlLeavetype" + col) as DropDownList);
                                                        //ddlReson = (GridView1.Rows[Att_mark_row].FindControl("ddlReson" + col) as DropDownList);
                                                    }
                                                    ListItem list = ddl.Items.FindByText(Att_Mark1);
                                                    if (list == null)
                                                    {
                                                        ddl.Items.Insert(0, Att_Mark1);
                                                        ddl.Enabled = false;
                                                    }
                                                    else
                                                        ddl.Items.FindByText(Att_Mark1.ToString()).Selected = true;

                                                    if (Att_Mark1 != "")
                                                    {
                                                        temp = temp + 1;
                                                    }
                                                }
                                                if (hatreason.Contains(rollno_Att.Trim().ToLower()))
                                                {
                                                    Att_Markvalue = GetCorrespondingKey(rollno_Att.Trim().ToLower(), hatreason).ToString();
                                                    Att_Mark1 = Attmark(Att_Markvalue);
                                                    if (col == 1)
                                                        ddlReson = (GridView1.Rows[Att_mark_row].FindControl("ddlReson") as DropDownList);
                                                    else
                                                        ddlReson = (GridView1.Rows[Att_mark_row].FindControl("ddlReson" + col) as DropDownList);

                                                    ListItem list = ddlReson.Items.FindByText(Att_Markvalue);
                                                    if (list == null)
                                                    {
                                                        ddlReson.Items.Insert(0, Att_Markvalue);
                                                        ddlReson.Enabled = false;
                                                    }
                                                    else
                                                        ddlReson.Items.FindByText(Att_Markvalue.ToString()).Selected = true;
                                                }

                                            }
                                        }

                                        if (dtOnduty.Rows.Count > 0)
                                        {
                                            if (col == 1)
                                            {
                                                ddl = (GridView1.Rows[Att_mark_row].FindControl("ddlLeavetype") as DropDownList);
                                                ddlReson = (GridView1.Rows[Att_mark_row].FindControl("ddlReson") as DropDownList);
                                            }
                                            else
                                            {
                                                ddl = (GridView1.Rows[Att_mark_row].FindControl("ddlLeavetype" + col) as DropDownList);
                                                ddlReson = (GridView1.Rows[Att_mark_row].FindControl("ddlReson" + col) as DropDownList);
                                            }

                                            dtOnduty.DefaultView.RowFilter = "roll_no='" + rollno_Att + "'";
                                            DataView dvOD = dtOnduty.DefaultView;
                                            if (dvOD.Count > 0)
                                            {
                                                string Hours = Convert.ToString(dvOD[0]["hourse"]);
                                                if (!string.IsNullOrEmpty(Hours))
                                                {
                                                    if (Hours.Contains(col.ToString()))
                                                    {

                                                        ddl.Enabled = false;
                                                        ddlReson.Enabled = false;
                                                        //grd.Enabled = false;
                                                    }
                                                }
                                                else
                                                {
                                                    ddl.Enabled = false;
                                                    ddlReson.Enabled = false;
                                                    //grd.Enabled = false;
                                                }

                                            }
                                        }

                                    }


                                }
                            }
                            if (temp > 0)
                            {
                                Buttonsave.Visible = false;
                                //Buttonupdate.Visible = true;
                            }
                            Labelstaf.Visible = false;
                            GridView1.Visible = true;
                            headerpanelhomework.Visible = true;
                            //btnaddhme.Visible = true;
                            Buttonselectall.Visible = true;
                            Buttondeselect.Visible = true;
                            Panelcomplete.Visible = true;
                            Panelyet.Visible = true;
                            filltree();
                            headerpanelnotes.Visible = true;
                            pHeaderlesson.Visible = true;
                            pBodylesson.Visible = true;
                            pBodynotes.Visible = true;
                            pBodyquestionaddition.Visible = true;
                            pHeaderatendence.Visible = true;
                            pBodyatendence.Visible = true;
                            pHeaderatendence.Visible = true;
                            pHeaderlesson.Visible = true;
                            pBodyatendence.Visible = true;
                            pBodylesson.Visible = true;
                            lbl_alert.Visible = false;
                            Buttonsave.Visible = true;
                            //Buttonupdate.Visible = true;
                            pHeaderatendence.Visible = true;
                            pHeaderlesson.Visible = true;
                            Buttonsave.Visible = true;
                        }
                    }
                }

                present_calcflag.Clear();
                absent_calcflag.Clear();
                hat.Clear();
                hat.Add("colege_code", Session["collegecode"].ToString());
                ds_attndmaster = da.select_method("ATT_MASTER_SETTING", hat, "sp");
                if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
                {
                    count_master = (ds_attndmaster.Tables[0].Rows.Count);
                    if (count_master > 0)
                    {
                        for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                        {
                            if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                            {
                                if (!present_calcflag.ContainsKey(ds_attndmaster.Tables[0].Rows[count_master]["DispText"].ToString()))
                                {
                                    present_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["DispText"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["DispText"].ToString
        ());
                                }
                            }
                            if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                            {
                                if (!absent_calcflag.ContainsKey(ds_attndmaster.Tables[0].Rows[count_master]["DispText"].ToString()))
                                {
                                    absent_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["DispText"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["DispText"].ToString());
                                }
                            }
                        }
                    }
                }
                int coli = 0;

                if (col == 1)
                {
                    //GridView1.FooterRow.Cells[8].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                    GridView1.Columns[8].Visible = true;
                    GridView1.Columns[9].Visible = true;
                }
                if (col == 2)
                {
                    //GridView1.FooterRow.Cells[10].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                    GridView1.Columns[10].Visible = true;
                    GridView1.Columns[11].Visible = true;
                }
                if (col == 3)
                {
                    //GridView1.FooterRow.Cells[12].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                    GridView1.Columns[12].Visible = true;
                    GridView1.Columns[13].Visible = true;
                }
                if (col == 4)
                {
                    //GridView1.FooterRow.Cells[14].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                    GridView1.Columns[14].Visible = true;
                    GridView1.Columns[15].Visible = true;
                }
                if (col == 5)
                {
                    //GridView1.FooterRow.Cells[16].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                    GridView1.Columns[16].Visible = true;
                    GridView1.Columns[17].Visible = true;
                }
                if (col == 6)
                {
                    //GridView1.FooterRow.Cells[18].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                    GridView1.Columns[18].Visible = true;
                    GridView1.Columns[19].Visible = true;
                }
                if (col == 7)
                {
                    //GridView1.FooterRow.Cells[20].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                    GridView1.Columns[20].Visible = true;
                    GridView1.Columns[21].Visible = true;
                }
                if (col == 8)
                {
                    //GridView1.FooterRow.Cells[22].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                    GridView1.Columns[22].Visible = true;
                    GridView1.Columns[23].Visible = true;
                }
                if (col == 9)
                {
                    //GridView1.FooterRow.Cells[24].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                    GridView1.Columns[24].Visible = true;
                    GridView1.Columns[25].Visible = true;
                }
                if (col == 10)
                {
                    //GridView1.FooterRow.Cells[26].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                    GridView1.Columns[26].Visible = true;
                    GridView1.Columns[27].Visible = true;
                }
                for (int cellS = 8; cellS < GridView1.Columns.Count; cellS += 2)
                {
                    absent_count = 0;
                    present_count = 0;

                    foreach (GridViewRow GR in GridView1.Rows)
                    {
                        if (GridView1.Columns[cellS].Visible)
                        {

                            string LeaveType = string.Empty;
                            if (cellS == 8)
                                coli = 1;
                            if (cellS == 10)
                                coli = 2;
                            if (cellS == 12)
                                coli = 3;
                            if (cellS == 14)
                                coli = 4;
                            if (cellS == 16)
                                coli = 5;
                            if (cellS == 18)
                                coli = 6;
                            if (cellS == 20)
                                coli = 7;
                            if (cellS == 22)
                                coli = 8;
                            if (cellS == 24)
                                coli = 9;
                            if (cellS == 26)
                                coli = 10;

                            if (coli == 1)
                                LeaveType = Convert.ToString((GR.FindControl("ddlLeavetype") as DropDownList).SelectedItem.Text);
                            else
                                LeaveType = Convert.ToString((GR.FindControl("ddlLeavetype" + coli) as DropDownList).SelectedItem.Text);
                            if (!string.IsNullOrEmpty(LeaveType))
                            {
                                if (present_calcflag.ContainsValue(LeaveType.ToString()))
                                {
                                    present_count++;
                                }
                                if (absent_calcflag.ContainsValue(LeaveType.ToString()))
                                {
                                    absent_count++;
                                }
                            }
                            GridView1.FooterRow.Cells[5].Text = "No Of Student(s) Present: <br> No Of Student(s) Absent:";
                            if (coli == 1)
                                GridView1.FooterRow.Cells[8].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                            if (coli == 2)
                                GridView1.FooterRow.Cells[10].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                            if (coli == 3)
                                GridView1.FooterRow.Cells[12].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                            if (coli == 4)
                                GridView1.FooterRow.Cells[14].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                            if (coli == 5)
                                GridView1.FooterRow.Cells[16].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                            if (coli == 6)
                                GridView1.FooterRow.Cells[18].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                            if (coli == 7)
                                GridView1.FooterRow.Cells[20].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                            if (coli == 8)
                                GridView1.FooterRow.Cells[22].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                            if (coli == 9)
                                GridView1.FooterRow.Cells[24].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                            if (coli == 10)
                                GridView1.FooterRow.Cells[26].Text = present_count.ToString() + "<br>" + absent_count.ToString();
                        }
                    }
                }



            }
            else
            {
                lbl_alert.Visible = true;
                lbl_alert.Text = "Append Can't be Allow Different (Type Of) Subjects";
            }

        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    public void mark_attendance()
    {
        try
        {
            string Discon = da.GetFunction("select value from Master_Settings where settings='Attendance Discount' and  " + grouporusercode + "");
            string debar = da.GetFunction("select value from Master_Settings where settings='Attendance Debar' and  " + grouporusercode + "");
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

            dicFeeOfRollStudents = new Dictionary<string, DateTime[]>();
            dicFeeOnRollStudents = new Dictionary<string, byte>();
            //GetFeeOfRollStudent(ref dicFeeOfRollStudents, ref dicFeeOnRollStudents);

            bool attavailable = false;
            chkis_studavailable.Checked = false;
            string get_alter_or_sem = string.Empty;
            DataSet dsstudentquery = new DataSet();
            Hashtable hatstudegree = new Hashtable();
            string[] split_tag_val;
            if (singlesubject == true)
            {
                split_tag_val = Convert.ToString(singlesubjectno).Split('*');

            }
            else
            {
                split_tag_val = getcelltag.Split('*');
                inicolcount = Convert.ToInt16(GridView1.Columns.Count);
            }



            #region Redo Attendance
            bool incRedo = false;
            string stvfa = da.GetFunctionv("select value from Master_Settings where settings = 'Include Redo student in Attendance'");
            if (stvfa.Trim() == "1")
            {
                incRedo = true;
            }
            #endregion

            for (int tag_for = 0; tag_for <= split_tag_val.GetUpperBound(0); tag_for++)
            {
                str = split_tag_val[tag_for].ToString();
                string tempdegree = split_tag_val[tag_for].ToString();

                if (str != "")
                {
                    string[] sp1 = str.Split(new Char[] { '-' });
                    if (sp1[0].ToString().Trim() != "Selected day is Holiday" && sp1[0].ToString().Trim() != "")
                    {
                        string byear = string.Empty;
                        degree_code = sp1[0];
                        semester = sp1[1];
                        subject_no = sp1[2];
                        string batch_year = sp1[4].ToString();
                        //==============================================================================================
                        //  string check_lab = da.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subject_no + "'");
                        if (sp1.GetUpperBound(0) == 7)
                        {
                            sections = sp1[3];
                            byear = sp1[4];
                            subj_type = sp1[5];
                            subj_count_in_onehr = sp1[6];
                            get_alter_or_sem = sp1[7];
                        }
                        else
                        {
                            sections = string.Empty;
                            byear = sp1[3];
                            subj_type = sp1[4];
                            subj_count_in_onehr = sp1[5];
                            get_alter_or_sem = sp1[6];
                        }
                        Session["deg_code"] = degree_code;
                        Session["semester"] = semester;
                        Session["sub_no"] = subject_no;
                        Session["sections"] = sections;
                        Session["batch_year"] = byear;
                        bool hrlock = Hour_lock(degree_code, batch_year, semester, getcolheader, sections);  //aruna 23july2013
                        if (hrlock == true)
                        {
                            headerpanelhomework.Visible = false;
                            //btnaddhme.Visible = false;
                            Buttonsave.Visible = false;
                            // Buttonupdate.Visible = false;
                            pHeaderatendence.Visible = false;
                            pHeaderlesson.Visible = false;
                            headerpanelnotes.Visible = false;
                            pBodyatendence.Visible = false;
                            pBodylesson.Visible = false;
                            pBodynotes.Visible = false;
                            pBodyquestionaddition.Visible = false;
                            headerquestionaddition.Visible = false;
                            headerADDQuestion.Visible = false;
                            lbl_alert.Visible = true;
                            lbl_alert.Text = " You cannot edit this day/Hour attendance due to security reasons. Get permission from PRINCIPAL to update the attendance";
                            GridView1.Visible = false;
                            Buttondeselect.Visible = false;
                            Buttonselectall.Visible = false;
                            lblmanysubject.Visible = false;
                            ddlselectmanysub.Visible = false;
                            return;
                        }
                        chk = daycheck(Convert.ToDateTime(sel_date));
                        GetFeeOfRollStudent(ref dicFeeOfRollStudents, ref dicFeeOnRollStudents, Convert.ToString(Convert.ToDateTime(sel_date).ToString("dd-MM-yyyy")));
                        bool userDayLock = DayLockForUser(Convert.ToDateTime(sel_date));
                        if (!userDayLock)
                        {
                            headerpanelhomework.Visible = false;
                            //btnaddhme.Visible = false;
                            Buttonsave.Visible = false;
                            //Buttonupdate.Visible = false;
                            pHeaderatendence.Visible = false;
                            pHeaderlesson.Visible = false;
                            headerpanelnotes.Visible = false;
                            pBodyatendence.Visible = false;
                            pBodylesson.Visible = false;
                            pBodynotes.Visible = false;
                            pBodyquestionaddition.Visible = false;
                            lbl_alert.Visible = true;
                            //lbl_alert.Text = "You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator";
                            //Added by srinath 25/8/2016 JPR
                            lbl_alert.Text = " You cannot edit this day attendance due to security reasons. Get permission from PRINCIPAL to update the attendance";
                            GridView1.Visible = false;
                            Buttondeselect.Visible = false;
                            Buttonselectall.Visible = false;
                            lblmanysubject.Visible = false;
                            ddlselectmanysub.Visible = false;
                            headerquestionaddition.Visible = false;
                            headerADDQuestion.Visible = false;
                            return;
                        }
                        else if (chk == false)
                        {
                            headerpanelhomework.Visible = false;
                            //btnaddhme.Visible = false;
                            Buttonsave.Visible = false;
                            //Buttonupdate.Visible = false;
                            pHeaderatendence.Visible = false;
                            pHeaderlesson.Visible = false;
                            headerpanelnotes.Visible = false;
                            pBodyatendence.Visible = false;
                            pBodylesson.Visible = false;
                            pBodynotes.Visible = false;
                            pBodyquestionaddition.Visible = false;
                            lbl_alert.Visible = true;
                            //lbl_alert.Text = "You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator";
                            //Added by srinath 25/8/2016 JPR
                            lbl_alert.Text = " You cannot edit this day/Hour attendance due to security reasons. Get permission from PRINCIPAL to update the attendance";
                            GridView1.Visible = false;
                            Buttondeselect.Visible = false;
                            Buttonselectall.Visible = false;
                            lblmanysubject.Visible = false;
                            ddlselectmanysub.Visible = false;
                            headerquestionaddition.Visible = false;
                            headerADDQuestion.Visible = false;
                        }
                        else
                        {
                            //getcolheader = getcolheader;
                            string temp_date = Convert.ToString((gridTimeTable.Rows[ar].FindControl("lblDateDisp") as Label).Text);
                            //string temp_date = FpSpread1.Sheets[0].RowHeader.Cells[ar, 0].Text;
                            //string[] spitdate = temp_date;
                            string[] date_split = temp_date.Split('-');
                            getdate = date_split[2] + "-" + date_split[1] + "-" + date_split[0];
                            hr = ac.ToString();
                            string altersetion = string.Empty;
                            if (sections.ToString() != "" && sections.ToString() != "-1" && sections != null)
                            {
                                strsec = " and isnull(sections,'')='" + sections.ToString() + "' ";
                                altersetion = "and isnull(sections,'')='" + sections.ToString() + "'"; 
                            }
                            else
                            {
                                strsec = string.Empty;
                                altersetion = string.Empty;
                            }

                            string qry = "select asd.actualStaffCode from alternateStaffDetails asd ,subject s where asd.subjectNo=s.subject_no and alternateDate='" + getdate + "' and alterHour='" + hr + "' and alterStaffCode='" + Convert.ToString(Session["Staff_Code"]).Trim() + "' and asd.subjectNo='" + subject_no + "'";

                            DataTable dtAlterStaffDetails = new DataTable();
                            dtAlterStaffDetails = da.select_method_wop_table(qry, "text");

                            //string strAlt = "select * from AlternateDetails where batch_year='" + Convert.ToString(byear) + "' and semester='" + Convert.ToString(semester) + "' and Degree_code='" + Convert.ToString(degree_code) + "' " + altersetion + " and AlternateDate='" + getdate + "' and AlterHour='" + hr + "'";
                            //DataTable dtAlterDet = dirAcc.selectDataTable(strAlt);

                            string actualStaffCode = Convert.ToString(Session["Staff_Code"]).Trim();
                            List<string> alterStaffDetails = dtAlterStaffDetails.AsEnumerable().Select(r => r.Field<string>("actualStaffCode")).ToList<string>();
                            //if (alterStaffDetails.Count == 0)
                            alterStaffDetails.Add(actualStaffCode);
                            //-------------------------------serial number check
                            Session["str_section"] = strsec;
                            //Session["hr"] = ac + 1;
                            Session["hr"] = ac;
                            Session["StaffSelector"] = "0";  //Session["collegecode"].ToString()
                            string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString() + "'");
                            string[] splitminimumabsentsms = minimumabsentsms.Split('-');
                            if (splitminimumabsentsms.Length == 2)
                            {
                                int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                                if (splitminimumabsentsms[0].ToString() == "1")
                                {
                                    if (Convert.ToInt32(byear) >= batchyearsetting)
                                    {
                                        Session["StaffSelector"] = "1";
                                    }
                                }
                            }
                            string strstaffselector = string.Empty;
                            if (Session["StaffSelector"].ToString() == "1")
                            {
                                foreach (string staff in alterStaffDetails)
                                {
                                    if (!string.IsNullOrEmpty(strstaffselector))
                                    {
                                        strstaffselector += " or subjectchooser.staffcode like '%" + staff + "%'";
                                    }
                                    else
                                    {
                                        strstaffselector = " and (subjectchooser.staffcode like '%" + staff + "%'";
                                        //strstaffselector += " or subjectchooser.staffcode like '%" + Convert.ToString(Session["Staff_Code"]).Trim() + "%'";//for alter same subject diffrent staff
                                    }
                                }
                                if (!string.IsNullOrEmpty(strstaffselector))
                                {
                                    strstaffselector += ")";
                                }
                            }
                            string staffCodeList = string.Empty;
                            staffCodeList = string.Join("','", alterStaffDetails.ToArray());
                            string batchvalue = da.GetFunction("select Stud_batch from staff_selector where subject_no='" + subject_no + "' and staff_code in('" + staffCodeList + "') " + strsec + "");
                            if (batchvalue.Trim() != "" && batchvalue.Trim() != "0" && batchvalue.Trim() != "-1" && batchvalue != null)
                            {
                                batchvalue = " and batch='" + batchvalue + "'";
                            }
                            else
                            {
                                batchvalue = string.Empty;
                            }
                            Hashtable hatfeeroll = new Hashtable();
                            string strfeeofrollquery = "select r.Roll_No from stucon s,Registration r where s.roll_no=r.Roll_No and r.Current_Semester=s.semester and s.ack_fee_of_roll=1  and r.Batch_Year='" + byear + "' and r.degree_code=" + degree_code + " and r.Current_Semester='" + semester + "' and r.cc=0 " + dis + "" + deba + " " + strsec + " ";
                            DataSet dsfeerol = da.select_method_wo_parameter(strfeeofrollquery, "text");
                            if (dsfeerol.Tables.Count > 0 && dsfeerol.Tables[0].Rows.Count > 0)
                            {
                                for (int fs = 0; fs < dsfeerol.Tables[0].Rows.Count; fs++)
                                {
                                    string feeofrolls = dsfeerol.Tables[0].Rows[fs]["Roll_No"].ToString().Trim().ToLower();
                                    if (!hatfeeroll.Contains(feeofrolls))
                                    {
                                        hatfeeroll.Add(feeofrolls, feeofrolls);
                                    }
                                }
                            }

                            string strorder = filterfunction();
                            string strstudentquery = string.Empty;
                            string timetable = string.Empty;
                            string gettimetable = da.GetFunction("Select top 1 ttname from semester_schedule where batch_year=" + byear + "  and degree_code=" + degree_code + " and semester='" + semester + "'  " + strsec + " and FromDate<='" + sel_date + "' order by FromDate Desc");                            //DataSet dsttname = da.select_method_wo_parameter(gettimetable, "text");
                            if (gettimetable.Trim() != null && gettimetable.Trim() != "" && gettimetable.Trim() != "0")
                            {
                                timetable = " and Timetablename='" + gettimetable + "'";
                            }
                           // if (get_alter_or_sem.Trim() == "alter")
                                //strstaffselector = string.Empty;

                            if (subj_type == "L")
                            {
                                if (get_alter_or_sem.Trim() == "alter")
                                {
                                    strstudentquery = "select distinct registration.roll_no,registration.app_no,registration.reg_no,registration.roll_admit ,registration.stud_name,registration.stud_type,registration.serialno,registration.degree_code,Registration.college_code,Registration.batch_year,Registration.current_semester,Registration.Sections,Registration.delflag,Registration.exam_flag  from subjectchooser_New,sub_sem,subject,Registration where fromdate='" + getdate + "' and  todate='" + getdate + "' and batch in(select distinct stu_batch from laballoc_new where subject_no='" + subject_no + "'  and batch_year=" + byear + "  and hour_value='" + hr + "'   " + strsec + "  and degree_code=" + degree_code + " and fdate='" + getdate + "' and  tdate='" + getdate + "' and day_value='" + Day_Var + "' ) and subjectchooser_New.subtype_no=sub_sem.subtype_no and  semester =  " + semester.ToString() + " and subjectchooser_New.subject_no=subject.subject_no and  registration.roll_no=subjectchooser_New.roll_no and  registration.current_semester=subjectchooser_New.semester and subjectchooser_New.subject_no='" + subject_no + "' and degree_code in ('" + degree_code + "')   and adm_date<='" + sel_date + "'  and SubjectChooser_new.Semester=registration.current_semester " + strsec + " and RollNo_Flag<>0 and cc=0 " + dis + "" + deba + " " + strorder + "";

                                }
                                else
                                {

                                    strstudentquery = "select distinct registration.roll_no,registration.app_no,registration.reg_no,registration.roll_admit ,registration.stud_name,registration.stud_type,registration.serialno,registration.degree_code,Registration.college_code,Registration.batch_year,Registration.current_semester,Registration.Sections,Registration.delflag,Registration.exam_flag from subjectchooser,sub_sem,subject,registration where batch in(select stu_batch from laballoc where subject_no='" + subject_no + "'  and batch_year=" + byear + "  and hour_value=" + hr + "   " + strsec + "  and degree_code=" + degree_code + " and day_value='" + Day_Var + "' " + timetable + " ) and subjectchooser.subtype_no=sub_sem.subtype_no and  semester =  " + semester.ToString() + " and subjectchooser.subject_no=subject.subject_no and  registration.roll_no=subjectchooser.roll_no and  registration.current_semester=subjectchooser.semester and subjectchooser.subject_no='" + subject_no + "'  and  registration.roll_no= subjectchooser.roll_no and registration.batch_year=" + byear + "  and registration.degree_code=" + degree_code + " " + strsec + "     and adm_date<='" + sel_date + "' and SubjectChooser.Semester=registration.current_semester and RollNo_Flag<>0 and cc=0 " + dis + "" + deba + "  " + strstaffselector + " " + batchvalue + " " + strorder + "";
                                }
                            }
                            else
                            {
                                strstudentquery = "Select distinct registration.roll_no,registration.app_no,registration.reg_no,registration.roll_admit ,registration.stud_name,registration.stud_type,registration.serialno,registration.degree_code,Registration.college_code,Registration.batch_year,Registration.current_semester,Registration.Sections,Registration.delflag,Registration.exam_flag from registration,SubjectChooser,applyn where registration.roll_no = subjectchooser.roll_no and registration.Degree_Code =" + degree_code + " and Semester = '" + semester + "' and Subject_No = '" + subject_no.ToString() + "' and RollNo_Flag<>0 and cc=0 " + dis + "" + deba + " and Semester = '" + semester + "' " + strsec + Session["strvar"].ToString() + "and registration.app_no=applyn.app_no" + "    and adm_date<='" + sel_date + "'  and SubjectChooser.Semester=registration.current_semester " + strstaffselector + " " + batchvalue + " " + strorder + "";


                            }
                            strstudentquery = strstudentquery + " ;select Distinct  Textval from textvaltable where textcriteria ='Attrs'";
                            strstudentquery = strstudentquery + " ; select rights from  OD_Master_Setting where " + grouporusercode.Split(';')[0] + "";
                            strstudentquery = strstudentquery + " ; select c.course_name,de.dept_acronym,d.degree_code from degree d,department de,course c where d.dept_code=de.dept_code and c.course_id=d.course_id";

                            string isredo = "select  distinct registration.roll_no,registration.app_no,registration.reg_no,registration.roll_admit ,registration.stud_name,registration.stud_type,registration.serialno,registration.degree_code,Registration.college_code,Registration.batch_year,Registration.current_semester,Registration.Sections,Registration.delflag,Registration.exam_flag from registration where registration.Batch_year='" + byear + "' and  degree_code='" + degree_code + "' and Current_Semester='" + semester.ToString() + "' and  adm_date<='" + sel_date + "'  and  cc=0  and ISNULL(isRedo,0)=1 " + strorder + "";
                            DataTable dtredo = dirAcc.selectDataTable(isredo);


                            if (dsstudentquery.Tables.Count > 0)
                            {
                                DataSet dsStudent = da.select_method_wo_parameter(strstudentquery, "Text");
                                dsstudentquery.Tables[0].Merge(dsStudent.Tables[0]);
                            }
                            else
                                dsstudentquery = da.select_method_wo_parameter(strstudentquery, "Text");

                            if (incRedo && dtredo.Rows.Count > 0)
                                dsstudentquery.Tables[0].Merge(dtredo);

                        }
                    }
                }
            }
            string[] date = sel_date.Split('-');
            byte day = Convert.ToByte(date[1]);
            byte month = Convert.ToByte(date[0]);
            int year = Convert.ToInt32(date[2]);
            string monthyear = ((year * 12) + month).ToString();
            string hour = Convert.ToString(ac);
            string column = "d" + day + "d" + hour;
            if (dsstudentquery.Tables.Count > 0 && dsstudentquery.Tables[0].Rows.Count > 0)
            {
                GridView1.DataSource = dsstudentquery.Tables[0];
                GridView1.DataBind();
                DataTable dt = dsstudentquery.Tables[0];
                arrayst = new ArrayList();
                arr = new ArrayList();
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    string appno = Convert.ToString(dt.Rows[k]["app_no"]);
                    arr.Add(appno);
                    string token = da.GetFunction("select fcm_token from Registration where app_no='" + appno + "'");
                    if (token != "")
                    {
                        arrayst.Add(token);
                    }

                }
                DataSet dsAttendance = new DataSet();
                Hashtable hatA = new Hashtable();
                //Here Mark Attendance------------------
                for (int tag_for = 0; tag_for <= split_tag_val.GetUpperBound(0); tag_for++)
                {
                    str = split_tag_val[tag_for].ToString();
                    string tempdegree = split_tag_val[tag_for].ToString();

                    if (str != "")
                    {
                        string[] sp1 = str.Split(new Char[] { '-' });
                        if (sp1[0].ToString().Trim() != "Selected day is Holiday" && sp1[0].ToString().Trim() != "")
                        {
                            string byear = string.Empty;
                            degree_code = sp1[0];
                            semester = sp1[1];
                            subject_no = sp1[2];
                            string batch_year = sp1[4].ToString();
                            //==============================================================================================
                            //  string check_lab = da.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subject_no + "'");
                            if (sp1.GetUpperBound(0) == 7)
                            {
                                sections = sp1[3];
                                byear = sp1[4];
                                subj_type = sp1[5];
                                subj_count_in_onehr = sp1[6];
                                get_alter_or_sem = sp1[7];
                            }
                            else
                            {
                                sections = string.Empty;
                                byear = sp1[3];
                                subj_type = sp1[4];
                                subj_count_in_onehr = sp1[5];
                                get_alter_or_sem = sp1[6];
                            }


                            //hatA.Clear();
                            //hat.Clear();
                            //hat.Add("mb_year", byear);// Session["collegecode"].ToString());
                            //hat.Add("md_code", degree_code);
                            //hat.Add("msem", semester);
                            //hat.Add("msec", sections);
                            //hat.Add("@mColumn", monthyear);
                            //hat.Add("@ColumnName", column);
                            string College = Convert.ToString(Session["collegecode"]);
                            string SlectQ = "select roll_no,Att_App_no,Att_CollegeCode,month_year,(select am.DispText from AttMasterSetting am where am.LeaveCode=a." + column + " and am.collegecode='" + College + "') as LeaveType,a." + column + " from attendance a where a.roll_no in(select roll_no from Registration where Batch_Year='" + byear + "' and degree_code='" + degree_code + "' and Current_Semester='" + semester + "' and isnull(Sections,'')='" + sections + "') and month_year=" + monthyear + " and ISNULL(a." + column + ",'')<>''";
                            DataTable dtTatt = new DataTable();
                            if (dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0].Rows.Count > 0)
                            {
                                dtTatt = dirAcc.selectDataTable(SlectQ);
                                dsAttendance.Tables[0].Merge(dtTatt);
                            }
                            else
                                dsAttendance = da.select_method_wo_parameter(SlectQ, "text");
                        }
                    }
                }

                int col = Convert.ToInt32(Session["Col"].ToString());//sel_date
                string temp_date = Convert.ToString((gridTimeTable.Rows[ar].FindControl("lblDateDisp") as Label).Text);
                DataTable dtOnduty = dirAcc.selectDataTable("select * from Onduty_Stud od where   (convert(datetime,od.fromdate,105) >= '" + sel_date + "' or  convert(datetime,od.Todate,105)>='" + sel_date + "') and  (convert(datetime,od.fromdate,105) <='" + sel_date + "' or convert(datetime,od.Todate,105)<= '" + sel_date + "')");


                if (dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0].Rows.Count > 0)
                {
                    foreach (GridViewRow grd in GridView1.Rows)
                    {
                        DropDownList AttMark;
                        DropDownList ddlReson;
                        if (col == 1)
                        {
                            AttMark = (grd.FindControl("ddlLeavetype") as DropDownList);
                            ddlReson = (grd.FindControl("ddlReson") as DropDownList);
                        }
                        else
                        {
                            AttMark = (grd.FindControl("ddlLeavetype" + col) as DropDownList);
                            ddlReson = (grd.FindControl("ddlReson" + col) as DropDownList);
                        }
                        string rollNo = (grd.FindControl("lblrollNo") as Label).Text;//156045
                        dsAttendance.Tables[0].DefaultView.RowFilter = "roll_no='" + rollNo + "'";

                        DataTable dtAttSaved = dsAttendance.Tables[0].DefaultView.ToTable();
                        if (dtAttSaved.Rows.Count > 0)
                        {
                            string attval = Convert.ToString(dtAttSaved.Rows[0][column]);
                            string MarkAtt = Attmark(attval);
                            ListItem item = AttMark.Items.FindByText(MarkAtt);
                            if (item == null)
                            {
                               //AttMark.Text = MarkAtt;
                                AttMark.Items.Insert(0, MarkAtt);
                                AttMark.Enabled = false;
                            }
                            else
                                AttMark.Items.FindByText(MarkAtt).Selected = true;
                        }
                        string attWithReson = "Select a." + column + " as Reason  from Attendance_withreason a  where  a.roll_no='" + rollNo + "' and month_year='" + monthyear + "'  and   a." + column + "<>''  and a." + column + " is not null ";
                        DataTable dtReason = dirAcc.selectDataTable(attWithReson);
                        if (dtReason.Rows.Count > 0)
                        {
                            string reason = Convert.ToString(dtReason.Rows[0]["Reason"]);
                            ListItem item = ddlReson.Items.FindByText(reason);
                            if (item==null)
                            {
                                //AttMark.Text = MarkAtt;
                                ddlReson.Items.Insert(0, reason);
                                ddlReson.Enabled = false;
                            }
                            else
                                ddlReson.Items.FindByText(reason).Selected = true;
                        }

                        if (dtOnduty.Rows.Count > 0)
                        {
                            dtOnduty.DefaultView.RowFilter = "roll_no='" + rollNo + "'";
                            DataView dvOD = dtOnduty.DefaultView;
                            if (dvOD.Count > 0)
                            {
                                string Hours = Convert.ToString(dvOD[0]["hourse"]);
                                if (!string.IsNullOrEmpty(Hours))
                                {
                                    if (Hours.Contains(col.ToString()))
                                    {
                                        AttMark.Enabled = false;
                                        ddlReson.Enabled = false;
                                        //grd.Enabled = false;
                                    }
                                }
                                else
                                {
                                    AttMark.Enabled = false;
                                    ddlReson.Enabled = false;
                                    //grd.Enabled = false;
                                }

                            }
                        }

                        string stuCon = " select  convert(varchar(15),dateadd(day,tot_days-1,ack_date),1) as action_days,ack_date,tot_days,s.roll_no from stucon s where s.roll_no='" + rollNo + "'  and ack_date<='" + sel_date + "'";
                        DataTable dtStuCon = dirAcc.selectDataTable(stuCon);
                        if (dtStuCon.Rows.Count > 0)
                        {
                            DateTime dt_curr = Convert.ToDateTime(sel_date.ToString());
                            DateTime dt_act = Convert.ToDateTime(Convert.ToString(dtStuCon.Rows[0]["action_days"]));
                            TimeSpan t_con = dt_act.Subtract(dt_curr);
                            long daycon = t_con.Days;

                            DateTime dt_curr1 = Convert.ToDateTime(Convert.ToString(dtStuCon.Rows[0]["ack_date"]));
                            DateTime dt_act1 = Convert.ToDateTime(sel_date.ToString());
                            TimeSpan t_con1 = dt_act1.Subtract(dt_curr1);
                            long daycon1 = t_con1.Days;
                            long totalactdays = Convert.ToInt32(Convert.ToInt32(dtStuCon.Rows[0]["tot_days"]));
                            if ((Convert.ToInt32(daycon + daycon1) == totalactdays - 1) && totalactdays > 0)// && (daycon >= 0)
                            {
                                //AttMark.Items.FindByText("S").Selected = true;
                                //AttMark.Enabled = false;
                                //ddlReson.Enabled = false;
                                grd.BackColor = Color.Red;
                                grd.Enabled = false;
                            }


                        }
                    }
                }

                //-----------------------------------------
                GridView1.Visible = true;
                attavailable = true;
                GridView1.Columns[0].Visible = true;
                GridView1.Columns[1].Visible = true;
                GridView1.Columns[2].Visible = false;
                GridView1.Columns[3].Visible = false;
                GridView1.Columns[4].Visible = false;
                GridView1.Columns[5].Visible = true;
                GridView1.Columns[6].Visible = false;
                GridView1.Columns[7].Visible = true;


                //int col = Convert.ToInt32(Session["Col"].ToString());
                //------

                GridView1.Columns[8].Visible = false;
                GridView1.Columns[9].Visible = false;
                GridView1.Columns[10].Visible = false;
                GridView1.Columns[11].Visible = false;
                GridView1.Columns[12].Visible = false;
                GridView1.Columns[13].Visible = false;

                GridView1.Columns[14].Visible = false;
                GridView1.Columns[15].Visible = false;

                GridView1.Columns[16].Visible = false;
                GridView1.Columns[17].Visible = false;

                GridView1.Columns[18].Visible = false;
                GridView1.Columns[19].Visible = false;

                GridView1.Columns[20].Visible = false;
                GridView1.Columns[21].Visible = false;

                GridView1.Columns[22].Visible = false;
                GridView1.Columns[23].Visible = false;

                GridView1.Columns[24].Visible = false;
                GridView1.Columns[25].Visible = false;

                GridView1.Columns[26].Visible = false;
                GridView1.Columns[27].Visible = false;

                if (col == 1)
                {
                    GridView1.Columns[8].Visible = true;
                    GridView1.Columns[9].Visible = true;
                }
                if (col == 2)
                {
                    GridView1.Columns[10].Visible = true;
                    GridView1.Columns[11].Visible = true;
                }
                if (col == 3)
                {
                    GridView1.Columns[12].Visible = true;
                    GridView1.Columns[13].Visible = true;
                }
                if (col == 4)
                {
                    GridView1.Columns[14].Visible = true;
                    GridView1.Columns[15].Visible = true;
                }
                if (col == 5)
                {
                    GridView1.Columns[16].Visible = true;
                    GridView1.Columns[17].Visible = true;
                }
                if (col == 6)
                {
                    GridView1.Columns[18].Visible = true;
                    GridView1.Columns[19].Visible = true;
                }
                if (col == 7)
                {
                    GridView1.Columns[20].Visible = true;
                    GridView1.Columns[21].Visible = true;
                }
                if (col == 8)
                {
                    GridView1.Columns[22].Visible = true;
                    GridView1.Columns[23].Visible = true;
                }
                if (col == 9)
                {
                    GridView1.Columns[24].Visible = true;
                    GridView1.Columns[25].Visible = true;
                }
                if (col == 10)
                {
                    GridView1.Columns[26].Visible = true;
                    GridView1.Columns[27].Visible = true;
                }


                //------------------------------------



                int colu = GridView1.Columns.Count;

                if (Session["Rollflag"].ToString() == "1")
                {
                    GridView1.Columns[2].Visible = true;
                    GridView1.Columns[2].HeaderStyle.Width = 100;
                    //FpSpread2.Sheets[0].ColumnHeader.Columns[1].Visible = true;
                    // FpSpread2.Sheets[0].Columns[1].Width = 100;
                }
                if (Session["Regflag"].ToString() == "1")
                {
                    GridView1.Columns[3].Visible = true;
                    GridView1.Columns[3].HeaderStyle.Width = 100;
                    //FpSpread2.Sheets[0].ColumnHeader.Columns[2].Visible = true;
                    //FpSpread2.Sheets[0].Columns[2].Width = 100;
                }
                if (Session["Studflag"].ToString() == "1")
                {
                    GridView1.Columns[4].Visible = true;
                    GridView1.Columns[4].HeaderStyle.Width = 200;
                    //FpSpread2.Sheets[0].ColumnHeader.Columns[5].Visible = true;
                    //FpSpread2.Sheets[0].Columns[5].Width = 100;
                }
                if (CheckBox1.Checked == false)
                {
                    GridView1.Columns[7].Visible = false;
                    GridView1.Columns[7].HeaderStyle.Width = 100;
                    GridView1.HeaderRow.Cells[7].Visible = false;
                }
                filltree();
            }
            else
            {
                if (ddlselectmanysub.Visible = true && ddlselectmanysub.Items.Count > 0)
                {
                    if (get_alter_or_sem.Trim() == "alter" || subj_type == "L")
                    {
                        lbl_alert.Visible = true;
                        lbl_alert.Text = "Please Allocate Batch For The Selected Class Students";
                        GridView1.Visible = false;
                        isvisible = false;
                    }
                    else
                    {
                        lbl_alert.Visible = true;
                        lbl_alert.Text = "No Students Found";
                        GridView1.Visible = false;
                        isvisible = false;
                    }
                }
                else
                {
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Please Allocate Batch For The Selected Class Students";
                    pBodyatendence.Visible = false;
                    pHeaderatendence.Visible = false;
                    GridView1.Visible = false;
                    isvisible = false;
                }
            }
            if (attavailable == true && CheckBox1.Checked)
            {
                load_attendance();
            }

        }
        catch (Exception ex)
        {
           // da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {

                DropDownList selectAll = (e.Row.FindControl("ddlSelectAll") as DropDownList);
                DropDownList selectAll2 = (e.Row.FindControl("ddlSelectAll2") as DropDownList);
                DropDownList selectAll3 = (e.Row.FindControl("ddlSelectAll3") as DropDownList);
                DropDownList selectAll4 = (e.Row.FindControl("ddlSelectAll4") as DropDownList);
                DropDownList selectAll5 = (e.Row.FindControl("ddlSelectAll5") as DropDownList);
                DropDownList selectAll6 = (e.Row.FindControl("ddlSelectAll6") as DropDownList);
                DropDownList selectAll7 = (e.Row.FindControl("ddlSelectAll7") as DropDownList);
                DropDownList selectAll8 = (e.Row.FindControl("ddlSelectAll8") as DropDownList);
                DropDownList selectAll9 = (e.Row.FindControl("ddlSelectAll9") as DropDownList);
                DropDownList selectAll10 = (e.Row.FindControl("ddlSelectAll10") as DropDownList);

                string[] strcomo = new string[20];
                string[] attnd_rights1 = new string[100];
                int i = 0;
                string odrights = da.GetFunction("select rights from  OD_Master_Setting where " + grouporusercode + "");
                if (odrights.Trim() != null && odrights.Trim() != "" && odrights.Trim() != "0")
                {
                    string od_rights = string.Empty;
                    od_rights = odrights;
                    string[] split_od_rights = od_rights.Split(',');
                    strcomo = new string[split_od_rights.GetUpperBound(0) + 2];
                    strcomo[i++] = string.Empty;

                    for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                    {
                        selectAll.Items.Add(Convert.ToString(split_od_rights[od_temp]));
                        selectAll2.Items.Add(Convert.ToString(split_od_rights[od_temp]));
                        selectAll3.Items.Add(Convert.ToString(split_od_rights[od_temp]));
                        selectAll4.Items.Add(Convert.ToString(split_od_rights[od_temp]));
                        selectAll5.Items.Add(Convert.ToString(split_od_rights[od_temp]));
                        selectAll6.Items.Add(Convert.ToString(split_od_rights[od_temp]));
                        selectAll7.Items.Add(Convert.ToString(split_od_rights[od_temp]));
                        selectAll8.Items.Add(Convert.ToString(split_od_rights[od_temp]));
                        selectAll9.Items.Add(Convert.ToString(split_od_rights[od_temp]));
                        selectAll10.Items.Add(Convert.ToString(split_od_rights[od_temp]));
                        strcomo[i++] = split_od_rights[od_temp].ToString();
                    }
                    selectAll.Items.Insert(0, " ");
                    selectAll2.Items.Insert(0, " ");
                    selectAll3.Items.Insert(0, " ");
                    selectAll4.Items.Insert(0, " ");
                    selectAll5.Items.Insert(0, " ");
                    selectAll6.Items.Insert(0, " ");
                    selectAll7.Items.Insert(0, " ");
                    selectAll8.Items.Insert(0, " ");
                    selectAll9.Items.Insert(0, " ");
                    selectAll10.Items.Insert(0, " ");
                }
                else
                {
                    string[] value = { " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD", "LA" };
                    strcomo[0] = string.Empty;
                    for (int od_temp = 0; od_temp < value.Length; od_temp++)
                    {
                        selectAll.Items.Add(Convert.ToString(value[od_temp]));
                        selectAll.Items.Add(Convert.ToString(value[od_temp]));
                        selectAll2.Items.Add(Convert.ToString(value[od_temp]));
                        selectAll3.Items.Add(Convert.ToString(value[od_temp]));
                        selectAll4.Items.Add(Convert.ToString(value[od_temp]));
                        selectAll5.Items.Add(Convert.ToString(value[od_temp]));
                        selectAll6.Items.Add(Convert.ToString(value[od_temp]));
                        selectAll7.Items.Add(Convert.ToString(value[od_temp]));
                        selectAll8.Items.Add(Convert.ToString(value[od_temp]));
                        selectAll9.Items.Add(Convert.ToString(value[od_temp]));
                        selectAll10.Items.Add(Convert.ToString(value[od_temp]));
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int col = Convert.ToInt32(Session["Col"].ToString());
                Label stutype = (e.Row.FindControl("lblStuType") as Label);
                Label lblRollNo = (e.Row.FindControl("lblrollNo") as Label);
                Label lblCollCode = (e.Row.FindControl("lblCollCode") as Label);
                string del = Convert.ToString((e.Row.FindControl("lblDis") as Label).Text);
                string debar = Convert.ToString((e.Row.FindControl("lblDebar") as Label).Text);
                string ColCode = lblCollCode.Text;
                string rollNo = lblRollNo.Text;
                if (stutype.Text.Contains("Hostler"))
                {
                    e.Row.BackColor = Color.LightYellow;
                }
                else if (del.Trim() == "1" || del.Trim().ToLower() == "true" || debar.ToLower().Trim() == "debar")
                {
                    e.Row.BackColor = Color.Red;
                    e.Row.Enabled = false;
                }
                else
                {
                    e.Row.BackColor = Color.MediumSeaGreen;
                }
                DropDownList atttype = (e.Row.FindControl("ddlSelect") as DropDownList);
                DropDownList AttMark;
                DropDownList ddlReson;
                //if (del.Trim() != "1" && del.Trim().ToLower() != "true" && debar.ToLower().Trim() != "debar")
                //    {
                //int col = Convert.ToInt32(Session["Col"].ToString());
                if (col == 1)
                {
                    AttMark = (e.Row.FindControl("ddlLeavetype") as DropDownList);
                    ddlReson = (e.Row.FindControl("ddlReson") as DropDownList);
                }
                else
                {
                    AttMark = (e.Row.FindControl("ddlLeavetype" + col) as DropDownList);
                    ddlReson = (e.Row.FindControl("ddlReson" + col) as DropDownList);
                }

                string[] strcomo = new string[20];
                string[] attnd_rights1 = new string[100];
                int i = 0;
                string odrights = da.GetFunction("select rights from  OD_Master_Setting where " + grouporusercode + "");
                if (odrights.Trim() != null && odrights.Trim() != "" && odrights.Trim() != "0")
                {
                    string od_rights = string.Empty;
                    od_rights = odrights;
                    string[] split_od_rights = od_rights.Split(',');
                    strcomo = new string[split_od_rights.GetUpperBound(0) + 2];
                    strcomo[i++] = string.Empty;

                    for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                    {
                        atttype.Items.Add(Convert.ToString(split_od_rights[od_temp]));
                        AttMark.Items.Add(Convert.ToString(split_od_rights[od_temp]));
                        strcomo[i++] = split_od_rights[od_temp].ToString();
                    }
                    atttype.Items.Insert(0, " ");
                    AttMark.Items.Insert(0, " ");

                }
                else
                {
                    string[] value = { " ", "P", "A", "OD", "SOD", "ML", "NSS", "L", "NCC", "HS", "PP", "SYOD", "COD", "OOD", "LA" };

                    strcomo[0] = string.Empty;

                    for (int od_temp = 0; od_temp < value.Length; od_temp++)
                    {
                        AttMark.Items.Add(Convert.ToString(value[od_temp]));
                        atttype.Items.Add(Convert.ToString(value[od_temp]));
                    }
                }

                ddlReson.DataSource = getAbReasons();
                ddlReson.Width = 120;
                ddlReson.DataTextField = "Textval";
                ddlReson.DataValueField = "TextCode";
                ddlReson.DataBind();
                ddlReson.Items.Insert(0, string.Empty);


                bool checkedFeeOfRoll = false;
                Label lblRoll = (e.Row.FindControl("lblrollNo") as Label);
                string rollNoNEw = Convert.ToString(lblRoll.Text);
                Label lblDate = (e.Row.FindControl("lblDate") as Label);
                Label lblHR = (e.Row.FindControl("lblHR") as Label);

                lblDate.Text = sel_date.ToString();
                lblHR.Text = Convert.ToString(ac);//ac+1

                if (dicFeeOfRollStudents.ContainsKey(rollNo.Trim()) && dicFeeOnRollStudents.ContainsKey(rollNo.Trim()))
                {
                    DateTime[] dtFeeOfRoll = dicFeeOfRollStudents[rollNo.Trim()];
                    DateTime dtSelDate = new DateTime();
                    dtSelDate = Convert.ToDateTime(getdate);
                    string dtadntdate = da.GetFunction("select adm_date from registration where Roll_No ='" + rollNo + "'");
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
                    e.Row.BackColor = Color.Red;
                    AttMark.Enabled = false;
                    atttype.Enabled = false;
                }
                //else
                //{
                //    string[] date = sel_date.Split('-');
                //    byte day = Convert.ToByte(date[1]);
                //    byte month = Convert.ToByte(date[0]);
                //    int year = Convert.ToInt32(date[2]);
                //    string monthyear = ((year * 12) + month).ToString();
                //    string hour = Convert.ToString(ac);
                //    string column = "d" + day + "d" + hour;

                //    DataTable dtAttSaved = dirAcc.selectDataTable("select roll_no,Att_App_no,Att_CollegeCode,month_year,(select am.DispText from AttMasterSetting am where am.LeaveCode=a." + column + ") as LeaveType,a." + column + " from attendance a where Roll_no in ('" + rollNoNEw + "') and Att_CollegeCode = " + ColCode + " and month_year='" + monthyear + "' and ISNULL(a." + column + ",'')<>''");

                //    if (dtAttSaved.Rows.Count > 0)
                //    {
                //        string attval = Convert.ToString(dtAttSaved.Rows[0][column]);
                //        string MarkAtt = Attmark(attval);
                //        AttMark.Items.FindByText(MarkAtt).Selected = true;
                //    }

                //    string attWithReson = "Select a." + column + " as Reason  from Attendance_withreason a  where  a.roll_no='" + rollNoNEw + "' and month_year='" + monthyear + "'  and   a." + column + "<>''  and a." + column + " is not null ";
                //    DataTable dtReason = dirAcc.selectDataTable(attWithReson);
                //    if (dtReason.Rows.Count > 0)
                //    {
                //        string reason = Convert.ToString(dtReason.Rows[0]["Reason"]);
                //        ddlReson.Items.FindByText(reason).Selected = true;
                //    }
                //    string stuCon = " select  convert(varchar(15),dateadd(day,tot_days-1,ack_date),1) as action_days,ack_date,tot_days,s.roll_no from stucon s where s.roll_no='" + rollNoNEw + "'  and ack_date<='" + sel_date + "'";
                //    DataTable dtStuCon = dirAcc.selectDataTable(stuCon);
                //    if (dtStuCon.Rows.Count > 0)
                //    {
                //        string actiondate = Convert.ToString(dtStuCon.Rows[0]["action_days"]);
                //        string ackdate = Convert.ToString(dtStuCon.Rows[0]["ack_date"]);
                //        long totalactdays = Convert.ToInt32(dtStuCon.Rows[0]["tot_days"]);
                //        DateTime dt_curr = Convert.ToDateTime(sel_date.ToString());

                //        DateTime dt_act = Convert.ToDateTime(actiondate);
                //        DateTime dt_curr1 = Convert.ToDateTime(ackdate);
                //        TimeSpan t_con = dt_act.Subtract(dt_curr);
                //        long daycon = t_con.Days;
                //        DateTime dt_act1 = Convert.ToDateTime(sel_date.ToString());

                //        TimeSpan t_con1 = dt_act1.Subtract(dt_curr1);
                //        long daycon1 = t_con1.Days;

                //        if ((Convert.ToInt32(daycon + daycon1) == totalactdays - 1) && (daycon > 0))
                //        {
                //            AttMark.Items.FindByText("S").Selected = true;
                //            AttMark.Enabled = false;
                //        }
                //    }
                //}
            }
        }

        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void GridView1_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            present_calcflag.Clear();
            absent_calcflag.Clear();
            hat.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            ds_attndmaster = da.select_method("ATT_MASTER_SETTING", hat, "sp");
            if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
            {
                count_master = (ds_attndmaster.Tables[0].Rows.Count);
                if (count_master > 0)
                {
                    for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                    {
                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "0")
                        {
                            if (!present_calcflag.ContainsKey(ds_attndmaster.Tables[0].Rows[count_master]["DispText"].ToString()))
                            {
                                present_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["DispText"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["DispText"].ToString
    ());
                            }
                        }
                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                        {
                            if (!absent_calcflag.ContainsKey(ds_attndmaster.Tables[0].Rows[count_master]["DispText"].ToString()))
                            {
                                absent_calcflag.Add(ds_attndmaster.Tables[0].Rows[count_master]["DispText"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["DispText"].ToString());
                            }
                        }
                    }
                }
            }
            string get_alter_or_sem = string.Empty;
            DataSet dsstudentquery = new DataSet();
            Hashtable hatstudegree = new Hashtable();
            string[] split_tag_val;
            if (singlesubject == true)
            {
                split_tag_val = Convert.ToString(singlesubjectno).Split('*');

            }
            else
            {
                split_tag_val = getcelltag.Split('*');
                inicolcount = Convert.ToInt16(GridView1.Columns.Count);
            }
            string[] date = sel_date.Split('-');
            byte day = Convert.ToByte(date[1]);
            byte month = Convert.ToByte(date[0]);
            int year = Convert.ToInt32(date[2]);
            string monthyear = ((year * 12) + month).ToString();
            string hour = Convert.ToString(ac);
            string column = "d" + day + "d" + hour;

            DataSet dsAttendance = new DataSet();
            DataSet dsReason = new DataSet();

            Hashtable hatA = new Hashtable();
            //Here Mark Attendance------------------
            for (int tag_for = 0; tag_for <= split_tag_val.GetUpperBound(0); tag_for++)
            {
                str = split_tag_val[tag_for].ToString();
                string tempdegree = split_tag_val[tag_for].ToString();

                if (str != "")
                {
                    string[] sp1 = str.Split(new Char[] { '-' });
                    if (sp1[0].ToString().Trim() != "Selected day is Holiday" && sp1[0].ToString().Trim() != "")
                    {
                        string byear = string.Empty;
                        degree_code = sp1[0];
                        semester = sp1[1];
                        subject_no = sp1[2];
                        string batch_year = sp1[4].ToString();
                        //==============================================================================================
                        //  string check_lab = da.GetFunction("select lab from sub_sem,Subject where Subject.subtype_no=sub_sem.subtype_no and subject_no='" + subject_no + "'");
                        if (sp1.GetUpperBound(0) == 7)
                        {
                            sections = sp1[3];
                            byear = sp1[4];
                            subj_type = sp1[5];
                            subj_count_in_onehr = sp1[6];
                            get_alter_or_sem = sp1[7];
                        }
                        else
                        {
                            sections = string.Empty;
                            byear = sp1[3];
                            subj_type = sp1[4];
                            subj_count_in_onehr = sp1[5];
                            get_alter_or_sem = sp1[6];
                        }
                        string sect = string.Empty;
                        if (!string.IsNullOrEmpty(sections))
                            sect = "  and isnull(Sections,'')='" + sections + "'";

                        string College = Convert.ToString(Session["collegecode"]);

                        string SlectQ = "select roll_no,Att_App_no,Att_CollegeCode,month_year,(select am.DispText from AttMasterSetting am where convert(nvarchar(max),am.LeaveCode)=convert(nvarchar(max),a." + column + ") and am.collegecode='" + College + "') as LeaveType,a." + column + " from attendance a where a.roll_no in(select roll_no from Registration where Batch_Year='" + byear + "' and degree_code='" + degree_code + "' and Current_Semester='" + semester + "' " + sect + ") and month_year=" + monthyear + " and ISNULL(a." + column + ",'')<>''";

                        string SlectRQ = "select roll_no,Atwr_App_no,Attwr_CollegeCode,month_year,(select am.DispText from AttMasterSetting am where convert(nvarchar(max),am.LeaveCode)=convert(nvarchar(max),a." + column + ") and am.collegecode='" + College + "') as LeaveType,a." + column + " as reason from Attendance_withreason a where a.roll_no in(select roll_no from Registration where Batch_Year='" + byear + "' and degree_code='" + degree_code + "' and Current_Semester='" + semester + "' " + sect + ") and month_year=" + monthyear + " and ISNULL(a." + column + ",'')<>''";

                        DataTable dtTatt = new DataTable();
                        DataTable dicResons = new DataTable();
                        if (dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0].Rows.Count > 0)
                        {
                            dtTatt = dirAcc.selectDataTable(SlectQ);
                            dsAttendance.Tables[0].Merge(dtTatt);
                        }
                        else
                            dsAttendance = da.select_method_wo_parameter(SlectQ, "text");

                        if (dsReason.Tables.Count > 0 && dsReason.Tables[0].Rows.Count > 0)
                        {
                            dicResons = dirAcc.selectDataTable(SlectRQ);
                            dsReason.Tables[0].Merge(dicResons);
                        }
                        else
                            dsReason = da.select_method_wo_parameter(SlectRQ, "text");
                    }
                }
            }
            int col = Convert.ToInt32(Session["Col"].ToString());
            if (dsAttendance.Tables.Count > 0 && dsAttendance.Tables[0].Rows.Count > 0)
            {
                foreach (GridViewRow grd in GridView1.Rows)
                {
                    DropDownList AttMark;
                    DropDownList ddlReson;
                    if (col == 1)
                    {
                        AttMark = (grd.FindControl("ddlLeavetype") as DropDownList);
                        ddlReson = (grd.FindControl("ddlReson") as DropDownList);
                    }
                    else
                    {
                        AttMark = (grd.FindControl("ddlLeavetype" + col) as DropDownList);
                        ddlReson = (grd.FindControl("ddlReson" + col) as DropDownList);
                    }
                    string rollNo = (grd.FindControl("lblrollNo") as Label).Text;
                    dsAttendance.Tables[0].DefaultView.RowFilter = "roll_no='" + rollNo + "'";
                    DataTable dtAttSaved = dsAttendance.Tables[0].DefaultView.ToTable();
                    if (dtAttSaved.Rows.Count > 0)
                    {
                        string attval = Convert.ToString(dtAttSaved.Rows[0][column]);
                        string MarkAtt = Attmark(attval);
                        ListItem list= AttMark.Items.FindByText(MarkAtt);
                        if (list == null)
                        {
                            AttMark.Items.Insert(0, MarkAtt);
                            AttMark.Enabled = false;
                        }
                        else
                            AttMark.Items.FindByText(MarkAtt).Selected = true;
                        if (!string.IsNullOrEmpty(MarkAtt))
                        {
                            if (present_calcflag.ContainsValue(MarkAtt.ToString()))
                            {
                                present_count++;
                            }
                            if (absent_calcflag.ContainsValue(MarkAtt.ToString()))
                            {
                                absent_count++;
                            }
                        }
                    }
                    dsReason.Tables[0].DefaultView.RowFilter = "roll_no='" + rollNo + "'";
                    DataTable dtReason = dsReason.Tables[0].DefaultView.ToTable();
                    if (dtReason.Rows.Count > 0)
                    {
                        string reason = Convert.ToString(dtReason.Rows[0]["Reason"]);
                        //ddlReson.Items.FindByText(reason.Trim()).Selected = true;
                    }

                    string stuCon = " select  convert(varchar(15),dateadd(day,tot_days-1,ack_date),1) as action_days,ack_date,tot_days,s.roll_no from stucon s where s.roll_no='" + rollNo + "'  and ack_date<='" + sel_date + "'";
                    DataTable dtStuCon = dirAcc.selectDataTable(stuCon);
                    if (dtStuCon.Rows.Count > 0)
                    {
                        string actiondate = Convert.ToString(dtStuCon.Rows[0]["action_days"]);
                        string ackdate = Convert.ToString(dtStuCon.Rows[0]["ack_date"]);
                        long totalactdays = Convert.ToInt32(dtStuCon.Rows[0]["tot_days"]);
                        DateTime dt_curr = Convert.ToDateTime(sel_date.ToString());

                        DateTime dt_act = Convert.ToDateTime(actiondate);
                        DateTime dt_curr1 = Convert.ToDateTime(ackdate);
                        TimeSpan t_con = dt_act.Subtract(dt_curr);
                        long daycon = t_con.Days;
                        DateTime dt_act1 = Convert.ToDateTime(sel_date.ToString());

                        TimeSpan t_con1 = dt_act1.Subtract(dt_curr1);
                        long daycon1 = t_con1.Days;

                        if ((Convert.ToInt32(daycon + daycon1) == totalactdays - 1) && (daycon > 0))
                        {
                            AttMark.Text = "S";
                            //AttMark.Items.FindByText("S").Selected = true;
                            AttMark.Enabled = false;
                            if (!string.IsNullOrEmpty("S"))
                            {
                                if (present_calcflag.ContainsValue("S".ToString()))
                                {
                                    present_count++;
                                }
                                if (absent_calcflag.ContainsValue("S".ToString()))
                                {
                                    absent_count++;
                                }
                            }
                        }
                    }
                }
            }

            GridView1.FooterRow.Cells[5].Text = "No Of Student(s) Present: <br> No Of Student(s) Absent:";
            if (col == 1)
                GridView1.FooterRow.Cells[8].Text = present_count.ToString() + "<br>" + absent_count.ToString();
            if (col == 2)
                GridView1.FooterRow.Cells[10].Text = present_count.ToString() + "<br>" + absent_count.ToString();
            if (col == 3)
                GridView1.FooterRow.Cells[12].Text = present_count.ToString() + "<br>" + absent_count.ToString();
            if (col == 4)
                GridView1.FooterRow.Cells[14].Text = present_count.ToString() + "<br>" + absent_count.ToString();
            if (col == 5)
                GridView1.FooterRow.Cells[16].Text = present_count.ToString() + "<br>" + absent_count.ToString();
            if (col == 6)
                GridView1.FooterRow.Cells[18].Text = present_count.ToString() + "<br>" + absent_count.ToString();
            if (col == 7)
                GridView1.FooterRow.Cells[20].Text = present_count.ToString() + "<br>" + absent_count.ToString();
            if (col == 8)
                GridView1.FooterRow.Cells[22].Text = present_count.ToString() + "<br>" + absent_count.ToString();
            if (col == 9)
                GridView1.FooterRow.Cells[24].Text = present_count.ToString() + "<br>" + absent_count.ToString();
            if (col == 10)
                GridView1.FooterRow.Cells[26].Text = present_count.ToString() + "<br>" + absent_count.ToString();

        }
        catch (Exception ex)
        {
            //da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlertContent.Visible = false;
            divPopAlert.Visible = false;
            seColorforHour();
            int colIndex = Convert.ToInt32(Session["Col"].ToString());
            int rowIndex = Convert.ToInt32(Session["Row"].ToString());
            loadStudentGrid(colIndex, rowIndex);
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    public void seColorforHour()
    {
        try
        {
            string Discon = da.GetFunction("select value from Master_Settings where settings='Attendance Discount' and  " + grouporusercode + "");
            string debar = da.GetFunction("select value from Master_Settings where settings='Attendance Debar' and " + grouporusercode + "");
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


            int coli = 0;
            for (int cellS = 8; cellS < GridView1.Columns.Count; cellS += 2)
            {
                if (GridView1.Columns[cellS].Visible)
                {

                    string LeaveType = string.Empty;
                    if (cellS == 8)
                        coli = 1;
                    if (cellS == 19)
                        coli = 2;
                    if (cellS == 12)
                        coli = 3;
                    if (cellS == 14)
                        coli = 4;
                    if (cellS == 16)
                        coli = 5;
                    if (cellS == 18)
                        coli = 6;
                    if (cellS == 20)
                        coli = 7;
                    if (cellS == 22)
                        coli = 7;
                    if (cellS == 24)
                        coli = 8;
                    if (cellS == 26)
                        coli = 9;

                    string checkalter = string.Empty;
                    int colIndex = Convert.ToInt32(coli);
                    int rowIndex = Convert.ToInt32(Session["Row"].ToString());
                    string str_Date = (gridTimeTable.Rows[rowIndex].FindControl("lblDate") as Label).Text;
                    string linktext = "lblPeriod_" + colIndex;
                    string linkVal = "lnkPeriod_" + colIndex;
                    string getcelltag = (gridTimeTable.Rows[rowIndex].FindControl(linktext) as Label).Text;
                    if (!string.IsNullOrEmpty(getcelltag))
                    {
                        string[] split = str_Date.Split(new Char[] { '/' });
                        //string linkVal = "lnkPeriod_" + i;
                        LinkButton lnkbtn = (gridTimeTable.Rows[rowIndex].FindControl(linkVal) as LinkButton);
                        string str_day = (Convert.ToInt16(split[1].ToString())).ToString();
                        string Atmonth = (Convert.ToInt16(split[0].ToString())).ToString();
                        string Atyear = split[2].ToString();
                        int strdate = (Convert.ToInt32(Atmonth) + Convert.ToInt32(Atyear) * 12);
                        string str_hour = Convert.ToString(colIndex);
                        string dcolumn = "d" + str_day + "d" + str_hour;
                        DateTime date = Convert.ToDateTime(split[0].ToString() + '-' + split[1].ToString() + '-' + split[2].ToString());
                        string day = date.ToString("ddd");
                        string[] spilttext = getcelltag.Split('*');
                        for (int j = 0; j <= spilttext.GetUpperBound(0); j++)
                        {
                            string batch = string.Empty;
                            string section = string.Empty;
                            bool colorflag = false;
                            if (j > 0)
                            {
                                if (lnkbtn.ForeColor == Color.Blue)
                                {
                                    colorflag = true;
                                }
                                else
                                {
                                    colorflag = false;
                                }
                            }
                            if (colorflag == false)
                            {
                                string check_lab = string.Empty;
                                dailyentryflag = false;
                                attendanceentryflag = false;
                                string[] split_tag_val = spilttext[j].Split('-');
                                if (split_tag_val.GetUpperBound(0) >= 7)
                                {
                                    batch = split_tag_val[4].ToString();
                                    degree_code = split_tag_val[0].ToString();
                                    semester = split_tag_val[1].ToString();
                                    subject_no = split_tag_val[2].ToString();
                                    //section = "and Registration.Sections='" + split_tag_val[3].ToString() + "'";
                                    section = split_tag_val[3].ToString();
                                    checkalter = split_tag_val[7].ToString();
                                    check_lab = split_tag_val[5].ToString();
                                }
                                else
                                {
                                    batch = split_tag_val[3].ToString();
                                    degree_code = split_tag_val[0].ToString();
                                    semester = split_tag_val[1].ToString();
                                    subject_no = split_tag_val[2].ToString();
                                    section = string.Empty;
                                    checkalter = split_tag_val[6].ToString();
                                    check_lab = split_tag_val[4].ToString();
                                }

                                string sectionvar = string.Empty;
                                if (section.Trim() != "" && section != null && section != "-1")
                                {
                                    sectionvar = " and isnull(sections,'')='" + section + "'";
                                }
                                Session["StaffSelector"] = "0";
                                string strstaffselector = string.Empty;   //Session["collegecode"].ToString()
                                string staffbatchyear = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString() + "'");
                                string[] splitminimumabsentsms = staffbatchyear.Split('-');
                                if (splitminimumabsentsms.Length == 2)
                                {
                                    int batchyearsetting = Convert.ToInt32(splitminimumabsentsms[1].ToString());
                                    if (splitminimumabsentsms[0].ToString() == "1")
                                    {
                                        if (Convert.ToInt32(batch) >= batchyearsetting)
                                        {
                                            Session["StaffSelector"] = "1";
                                        }
                                    }
                                }
                                if (Session["StaffSelector"].ToString() == "1")
                                {
                                    strstaffselector = " and s.staffcode like '%" + Session["Staff_Code"].ToString() + "%'";
                                }
                                if (check_lab == "L" || check_lab.Trim().ToLower() == "l")
                                {
                                    string strquery = "  select p.schOrder,p.nodays,Convert(nvarchar(15),s.start_date,23) as start,s.starting_dayorder from PeriodAttndSchedule p, seminfo s where s.degree_code=p.degree_code and s.semester=p.semester and s.degree_code=" + degree_code + " and s.semester=" + semester + " AND s.batch_year=" + batch + "";
                                    Day_Order = "0";
                                    DataSet dsattendance = da.select_method(strquery, hat, "Text");
                                    if (dsattendance.Tables.Count > 0 && dsattendance.Tables[0].Rows.Count > 0)
                                    {
                                        Day_Order = dsattendance.Tables[0].Rows[0]["schOrder"].ToString();
                                        noofdays = dsattendance.Tables[0].Rows[0]["nodays"].ToString();
                                        start_datesem = dsattendance.Tables[0].Rows[0]["start"].ToString();
                                        start_dayorder = dsattendance.Tables[0].Rows[0]["starting_dayorder"].ToString();
                                    }
                                    //Week / Day order
                                    if (Day_Order == "1")
                                    {
                                        day = date.ToString("ddd");
                                    }
                                    else
                                    {
                                        day = da.findday(date.ToString(), degree_code, semester, batch, start_datesem.ToString(), noofdays.ToString(), start_dayorder);//Modifeied By Srianth add comman Daccess 5/9/2014
                                    }
                                    if (checkalter.ToLower().Trim() == "alter")
                                    {
                                        hat.Clear();
                                        hat.Add("batch_year", batch);
                                        hat.Add("degree_code", degree_code);
                                        hat.Add("sem", semester);
                                        hat.Add("sections", section);
                                        hat.Add("month_year", strdate);
                                        hat.Add("date", date);
                                        hat.Add("subject_no", subject_no);
                                        hat.Add("day", day);
                                        hat.Add("hour", str_hour);
                                        ds.Reset();
                                        ds.Dispose();
                                        ds = da.select_method("sp_stu_atten_month_check_lab_alter", hat, "sp");
                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            Att_strqueryst = ds.Tables[0].Rows[0]["stucount"].ToString();
                                            if (int.Parse(Att_strqueryst) > 0)
                                            {
                                                hat.Clear();
                                                ds.Reset();
                                                ds.Dispose();
                                                string strgetatt = "select count(distinct r.Roll_No) as stucount from registration r,attendance a,subjectchooser_new s where degree_code='" + degree_code + "' and current_semester='" + semester + "' and batch_year='" + batch + "' and cc=0 " + dis + "" + deba + " and month_year=" + strdate + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and";
                                                strgetatt = strgetatt + " r.current_semester=s.semester and subject_no='" + subject_no + "' " + sectionvar + " and(" + dcolumn + " is not null and " + dcolumn + "<>'0' and " + dcolumn + "<>'') and fromdate='" + date + "' and batch    in(select stu_batch from laballoc_new where subject_no='" + subject_no + "'  and batch_year='" + batch + "'  and hour_value='" + str_hour + "'  and    degree_code='" + degree_code + "' ";
                                                strgetatt = strgetatt + " and day_value='" + day + "' and semester='" + semester + "' " + sectionvar + " and fdate='" + date + "') and adm_date<='" + date + "'";
                                                ds = da.select_method_wo_parameter(strgetatt, "Text");
                                                //ds = da.select_method("sp_stu_atten_day_check_lab_alter", hat, "sp");
                                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                {
                                                    if (Att_strqueryst == ds.Tables[0].Rows[0]["stucount"].ToString())
                                                    {
                                                        Att_strqueryst = "0";
                                                    }
                                                    else
                                                    {
                                                        Att_strqueryst = "1";
                                                    }
                                                }
                                                else
                                                {
                                                    Att_strqueryst = "1";
                                                }
                                            }
                                            else
                                            {
                                                Att_strqueryst = "1";
                                            }
                                        }
                                        else
                                        {
                                            Att_strqueryst = "1";
                                        }
                                    }
                                    else
                                    {
                                        string timetable = da.GetFunction("select top 1 TTName,  FromDate from  Semester_Schedule where degree_code='" + degree_code + "' and semester='" + semester + "' and batch_year='" + batch + "' " + sectionvar + " and FromDate<='" + date.ToString("MM/dd/yyyy") + "' order by FromDate Desc");
                                        hat.Clear();
                                        string strstt = "select count(distinct r.Roll_No) as stucount from registration r,subjectchooser s where degree_code='" + degree_code + "' and ";
                                        strstt = strstt + " current_semester='" + semester + "' and batch_year='" + batch.ToString() + "' and cc=0 " + dis + " " + deba + " and r.roll_no=s.roll_no ";
                                        strstt = strstt + " and r.current_semester=s.semester and subject_no='" + subject_no + "'  " + sectionvar + "  and batch in(select stu_batch from ";
                                        strstt = strstt + " laballoc where subject_no='" + subject_no + "'  and batch_year='" + batch.ToString() + "'  and hour_value='" + str_hour + "' and degree_code='" + degree_code + "' ";
                                        strstt = strstt + " and day_value='" + day + "' and semester='" + semester + "'  " + sectionvar + " and Timetablename='" + timetable + "') and adm_date<='" + date.ToString("MM/dd/yyyy") + "'  " + strstaffselector + "";
                                        ds.Reset();
                                        ds.Dispose();
                                        //ds = da.select_method("sp_stu_atten_month_check_lab", hat, "sp");
                                        ds = da.select_method_wo_parameter(strstt, "Text");
                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            Att_strqueryst = ds.Tables[0].Rows[0]["stucount"].ToString();
                                            if (int.Parse(Att_strqueryst) > 0)
                                            {
                                                hat.Clear();
                                                ds.Reset();
                                                ds.Dispose();
                                                string strgetatt = "select count( r.Roll_No) as stucount from registration r,attendance a,subjectchooser s where degree_code='" + degree_code + "' and current_semester='" + semester + "' and batch_year='" + batch + "' and cc=0 " + dis + " " + deba + " and month_year=" + strdate + "";
                                                strgetatt = strgetatt + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and r.current_semester=s.semester and subject_no='" + subject_no + "' " + sectionvar + " and(" + dcolumn + " is not null and " + dcolumn + "<>'0' and " + dcolumn + "<>'') and batch in(select stu_batch from laballoc ";
                                                strgetatt = strgetatt + " where subject_no='" + subject_no + "' and Timetablename='" + timetable + "' and batch_year='" + batch + "'  and hour_value='" + str_hour + "'  and    degree_code='" + degree_code + "' and day_value='" + day + "' and semester='" + semester + "' " + sectionvar + ") and adm_date<='" + date + "' " + strstaffselector + "";
                                                ds = da.select_method_wo_parameter(strgetatt, "Text");
                                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                                {
                                                    if (Att_strqueryst == ds.Tables[0].Rows[0]["stucount"].ToString())
                                                    {
                                                        Att_strqueryst = "0";
                                                    }
                                                    else
                                                    {
                                                        Att_strqueryst = "1";
                                                    }
                                                }
                                                else
                                                {
                                                    Att_strqueryst = "1";
                                                }
                                            }
                                            else
                                            {
                                                Att_strqueryst = "1";
                                            }
                                        }
                                        else
                                        {
                                            Att_strqueryst = "1";
                                        }
                                    }
                                }
                                else
                                {
                                    hat.Clear();
                                    string strgetatt1 = "select count(distinct r.Roll_No) as stucount from registration r,subjectchooser s where  r.roll_no=s.roll_no and ";
                                    strgetatt1 = strgetatt1 + " r.current_semester=s.semester and batch_year='" + batch + "' and degree_code='" + degree_code + "'  and current_semester='" + semester + "' " + sectionvar + " ";
                                    strgetatt1 = strgetatt1 + "  and subject_no='" + subject_no + "'  and adm_date<='" + date + "' and cc=0 " + dis + " " + deba + "  " + strstaffselector + "";
                                    ds = da.select_method_wo_parameter(strgetatt1, "Text");
                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        Att_strqueryst = ds.Tables[0].Rows[0]["stucount"].ToString();
                                        if (int.Parse(Att_strqueryst) > 0)
                                        {
                                            hat.Clear();
                                            hat.Add("columnname", dcolumn);
                                            hat.Add("batch_year", batch);
                                            hat.Add("degree_code", degree_code);
                                            hat.Add("sem", semester);
                                            hat.Add("sections", section);
                                            hat.Add("month_year", strdate);
                                            hat.Add("date", date);
                                            hat.Add("subject_no", subject_no);
                                            ds.Reset();
                                            ds.Dispose();
                                            string strgetatt = "select count(registration.roll_no) as stucount  from registration,attendance,subjectchooser s where degree_code='" + degree_code + "' and current_semester='" + semester + "' and batch_year='" + batch + "' and cc=0 " + dis + " " + deba + " and month_year='" + strdate + "' and registration.roll_no=attendance.roll_no and  registration.roll_no=s.roll_no ";
                                            strgetatt = strgetatt + " and registration.current_semester=s.semester and subject_no='" + subject_no + "' " + sectionvar + "";
                                            strgetatt = strgetatt + " and (" + dcolumn + " is not null and " + dcolumn + "<>'0' and " + dcolumn + "<>'') and adm_date<='" + date + "'  " + strstaffselector + "";
                                            ds = da.select_method_wo_parameter(strgetatt, "Text");
                                            //    ds = da.select_method("sp_stu_atten_day_check", hat, "sp");
                                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                            {
                                                if (Att_strqueryst == ds.Tables[0].Rows[0]["stucount"].ToString())
                                                {
                                                    Att_strqueryst = "0";
                                                }
                                                else
                                                {
                                                    Att_strqueryst = "1";
                                                }
                                            }
                                            else
                                            {
                                                Att_strqueryst = "1";
                                            }
                                        }
                                        else
                                        {
                                            Att_strqueryst = "1";
                                        }
                                    }
                                    else
                                    {
                                        Att_strqueryst = "1";
                                    }
                                }
                                if (int.Parse(Att_strqueryst) > 0)
                                {
                                    //FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.Blue;
                                    attendanceentryflag = false;
                                    // lnkbtn.ForeColor = Color.DarkTurquoise;
                                }
                                else
                                {
                                    attendanceentryflag = true;
                                    //FpSpread1.Sheets[0].Cells[ar, Convert.ToInt32(hr) - 1].ForeColor = Color.ForestGreen;
                                }
                                if (section.Trim() == "" || section == null || section == "-1")
                                {
                                    section = string.Empty;
                                }
                                else
                                {
                                    section = " and isnull(Sections,'')='" + section + "'";
                                }
                                strquerytext = "select de.lp_code from dailyentdet de,dailystaffentry ds where de.lp_code=ds.lp_code  and batch_year=" + batch + " and degree_code='" + degree_code + "' and semester=" + semester + " " + section + " and subject_no='" + subject_no + "' and  staff_code='" + staff_code + "' and sch_date='" + date + "' and hr=" + str_hour + "";
                                ds.Reset();
                                ds.Dispose();
                                ds = da.select_method(strquerytext, hat, "Text");
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    dailyentryflag = true;
                                }
                                if (dailyentryflag == false && attendanceentryflag == false)
                                {
                                    lnkbtn.ForeColor = Color.Blue;
                                    j = spilttext.GetUpperBound(0) + 1;
                                }
                                else if (dailyentryflag == true && attendanceentryflag == false)
                                {
                                    if (lnkbtn.ForeColor == Color.DarkOrchid)
                                    {
                                        lnkbtn.ForeColor = Color.Blue;
                                    }
                                    else
                                    {
                                        lnkbtn.ForeColor = Color.DarkTurquoise;
                                    }
                                }
                                else if (dailyentryflag == false && attendanceentryflag == true)
                                {
                                    if (lnkbtn.ForeColor == Color.DarkTurquoise)
                                    {
                                        lnkbtn.ForeColor = Color.Blue;
                                    }
                                    else
                                    {
                                        lnkbtn.ForeColor = Color.DarkOrchid;
                                    }
                                }
                                else
                                {
                                    if (j == 0)
                                    {
                                        lnkbtn.ForeColor = Color.ForestGreen;
                                    }
                                    else
                                    {
                                        if (lnkbtn.ForeColor == Color.ForestGreen)
                                        {
                                            lnkbtn.ForeColor = Color.ForestGreen;
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
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }
    public void othertopicadd()
    {
        try
        {

            string other = "select * from dailyEntryother where college_code='" + Session["collegecode"] + "'";
            DataSet dsother = new DataSet();
            dsother = da.select_method(other, hat, "Text");
            if (dsother.Tables.Count > 0 && dsother.Tables[0].Rows.Count > 0)
            {
                ddlother.DataSource = dsother;
                ddlother.DataTextField = "topic_name";
                ddlother.DataValueField = "subpk";
                ddlother.DataBind();
                ddlother.Items.Insert(0, "Select");
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }

    #region HOME_WORK

    protected void btnsavewrk_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean pic = false;
            Boolean file = false;

            if (string.IsNullOrEmpty(txtheading.Text))
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Give some heading for Home Work')", true);
                return;
            }
            if (string.IsNullOrEmpty(txthomework.Text))
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Write some work for students')", true);
                return;
            }
            if (fudfile.HasFile)
            {
                if (fudfile.FileName.EndsWith(".jpg") || fudfile.FileName.EndsWith(".gif") || fudfile.FileName.EndsWith(".png") || fudfile.FileName.EndsWith(".jpeg"))
                { pic = true; }
                else
                {
                    pic = false;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The file you selected is not a valid image file. Please select another file')", true);
                    return;
                }
            }
            if (fudattachemntss.HasFile)
            {
                if (fudattachemntss.FileName.EndsWith(".txt") || fudattachemntss.FileName.EndsWith(".doc") || fudattachemntss.FileName.EndsWith(".xls") || fudattachemntss.FileName.EndsWith(".docx") || fudattachemntss.FileName.EndsWith(".txt") || fudattachemntss.FileName.EndsWith(".document") || fudattachemntss.FileName.EndsWith(".xls") || fudattachemntss.FileName.EndsWith(".xlsx") || fudattachemntss.FileName.EndsWith(".pdf") || fudattachemntss.FileName.EndsWith(".ppt") || fudattachemntss.FileName.EndsWith(".pptx"))
                { file = true; }
                else
                {
                    file = false;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('The file you selected is not a valid file. Please select another file')", true);
                    return;
                }
            }
            //if (pic == true && file == true)
            //{
            //    if (fudfile.HasFile && fudattachemntss.HasFile)
            //    {
            //abarna



            homeworksave();

            //ns.SendMessage(a);
            //    }
            //}

            #region hide
            //if (chkboxsms.Checked == true)
            //{
            //    if (txtmessage.Text != "")
            //    {
            //        sendsms();
            //    }
            //    else
            //    {
            //        lblsendmail.Text = "txtmessage is empty";
            //    }
            //}
            //else
            //{
            //    txtmessage.Text = "";
            //}
            //if (chkboxmail.Checked == true)
            //{
            //    if (txtbody.Text != "")
            //    {
            //        emailsend();
            //    }
            //    else
            //    {
            //        lblsendmail.Text = "txtbody is empty";
            //    }
            //}
            //else
            //{
            //    txtbody.Text = "";
            //}
            //if (chknotification.Checked == true)
            //{
            //    if (txtnotification.Text != "")
            //    {
            //        notificationsend();
            //    }
            //    else
            //    {
            //        lblsendmail.Text = "txtnotification is empty";
            //    }
            //}
            //else
            //{
            //    txtnotification.Text = "";
            //}
            //if (chkvoicecall.Checked == true)
            //{
            //    if (FileUpload1.FileName != "")
            //    {
            //        sendvoicemsg();
            //    }
            //    else
            //    {
            //        lblerrorvoice.Text = "upload voice file ";
            //    }
            //}
            #endregion
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }

    protected void homeworksave()
    {
        try
        {

            byte[] picbinary = new byte[0];
            string fileName = string.Empty;
            string fileExtension = string.Empty;
            string documentType = string.Empty;
            int picsize = 0;

            byte[] filebinary = new byte[0];
            string fileName1 = string.Empty;
            string fileExtension1 = string.Empty;
            string documentType1 = string.Empty;
            int filesize = 0;

            string ddlsub = string.Empty;
            string subno = string.Empty;
            string section = string.Empty;
            string heading = string.Empty;

            string hmewrk = txthomework.Text;
            heading = txtheading.Text;
            string update = lblsubtext.Text;
            string idno = lbldel.Text;

            string dte = lbldate.Text;

            DateTime dtAttendanceDate1 = new DateTime();
            DateTime.TryParseExact(Convert.ToString(dte), "d-MM-yyyy", null, DateTimeStyles.None, out dtAttendanceDate1);
            if (fudfile.HasFile)
            {
                bool FileFromat = false;
                FileFromat = FileTypeCheck(fudfile, ref fileName, ref fileExtension, ref documentType);
                picsize = fudfile.PostedFile.ContentLength;
                picbinary = new byte[picsize];
                fudfile.PostedFile.InputStream.Read(picbinary, 0, picsize);//string datetime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            }
            if (fudattachemntss.HasFile)
            {
                bool FileFromat1 = false;
                FileFromat1 = FileTypeCheck1(fudattachemntss, ref fileName1, ref fileExtension1, ref documentType1);
                filesize = fudattachemntss.PostedFile.ContentLength;
                filebinary = new byte[filesize];
                fudattachemntss.PostedFile.InputStream.Read(filebinary, 0, filesize);
            }

            if (ddlselectmanysub.SelectedValue != "" && ddlselectmanysub.SelectedValue != " ")
            {
                ddlsub = ddlselectmanysub.SelectedValue;
                string[] subsplit = ddlsub.Split('-');
                subno = subsplit[2];
                if (subsplit.GetUpperBound(0) == 7)
                {
                    section = subsplit[3];
                }
                else
                {
                    section = "";
                }
                SqlCommand cmdnotes = new SqlCommand();

                if (picsize == 0 && filesize == 0)
                {
                    cmdnotes.CommandText = "if exists(select * from Home_Work where idno='" + idno + "') update Home_Work set Homework=@hmewrk,Heading=@head where idno='" + idno + "'  else insert into Home_Work(subjectno,Date,Homework,Section,PhotoAttachment,PhotoContentType,PhotoData,FileAttachment,FileContentType,FileData,Heading)" + " VALUES (@subjno,@date,@hmewrk,@section,@picname,@pictype,@picdata,@filename,@filetype,@filedata,@head)";
                }
                else if (picsize != 0 && filesize != 0)
                {
                    cmdnotes.CommandText = "if exists(select * from Home_Work where idno='" + idno + "') update Home_Work set Homework=@hmewrk,Heading=@head,PhotoAttachment=@picname,PhotoContentType=@pictype,PhotoData=@picdata,FileAttachment=@filename,FileContentType=@filetype,FileData=@filedata where idno='" + idno + "'  else insert into Home_Work(subjectno,Date,Homework,Section,PhotoAttachment,PhotoContentType,PhotoData,FileAttachment,FileContentType,FileData,Heading)" + " VALUES (@subjno,@date,@hmewrk,@section,@picname,@pictype,@picdata,@filename,@filetype,@filedata,@head)";
                }
                else if (picsize != 0 && filesize == 0)
                {
                    cmdnotes.CommandText = "if exists(select * from Home_Work where idno='" + idno + "') update Home_Work set Homework=@hmewrk,Heading=@head,PhotoAttachment=@picname,PhotoContentType=@pictype,PhotoData=@picdata where idno='" + idno + "'  else insert into Home_Work(subjectno,Date,Homework,Section,PhotoAttachment,PhotoContentType,PhotoData,FileAttachment,FileContentType,FileData,Heading)" + " VALUES (@subjno,@date,@hmewrk,@section,@picname,@pictype,@picdata,@filename,@filetype,@filedata,@head)";
                }
                else if (picsize == 0 && filesize != 0)
                {
                    cmdnotes.CommandText = "if exists(select * from Home_Work where idno='" + idno + "') update Home_Work set Homework=@hmewrk,Heading=@head,FileAttachment=@filename,FileContentType=@filetype,FileData=@filedata where idno='" + idno + "'  else insert into Home_Work(subjectno,Date,Homework,Section,PhotoAttachment,PhotoContentType,PhotoData,FileAttachment,FileContentType,FileData,Heading)" + " VALUES (@subjno,@date,@hmewrk,@section,@picname,@pictype,@picdata,@filename,@filetype,@filedata,@head)";
                }

                cmdnotes.CommandType = CommandType.Text;
                cmdnotes.Connection = ssql;

                SqlParameter subjno = new SqlParameter("@subjno", SqlDbType.Int, 100);
                subjno.Value = Convert.ToInt32(subno);
                cmdnotes.Parameters.Add(subjno);

                SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 50);
                uploadedDate.Value = dtAttendanceDate1;
                cmdnotes.Parameters.Add(uploadedDate);

                SqlParameter homewrk = new SqlParameter("@hmewrk", SqlDbType.NVarChar, 1500);
                homewrk.Value = hmewrk.ToString();
                cmdnotes.Parameters.Add(homewrk);

                SqlParameter sec = new SqlParameter("@section", SqlDbType.NVarChar, 100);
                sec.Value = section.ToString();
                cmdnotes.Parameters.Add(sec);

                SqlParameter picnme = new SqlParameter("@picname", SqlDbType.NVarChar, 100);
                picnme.Value = fileName.ToString();
                cmdnotes.Parameters.Add(picnme);

                SqlParameter pictype = new SqlParameter("@pictype", SqlDbType.NVarChar, 100);
                pictype.Value = documentType.ToString();
                cmdnotes.Parameters.Add(pictype);

                SqlParameter picdata = new SqlParameter("@picdata", SqlDbType.Binary, picsize);
                picdata.Value = picbinary;
                cmdnotes.Parameters.Add(picdata);

                SqlParameter filenme = new SqlParameter("@filename", SqlDbType.NVarChar, 100);
                filenme.Value = fileName1.ToString();
                cmdnotes.Parameters.Add(filenme);

                SqlParameter filetype = new SqlParameter("@filetype", SqlDbType.NVarChar, 100);
                filetype.Value = documentType1.ToString();
                cmdnotes.Parameters.Add(filetype);

                SqlParameter filedata = new SqlParameter("@filedata", SqlDbType.Binary, filesize);
                filedata.Value = filebinary;
                cmdnotes.Parameters.Add(filedata);

                SqlParameter head = new SqlParameter("@head", SqlDbType.NVarChar, 100);
                head.Value = heading.ToString();
                cmdnotes.Parameters.Add(head);

                ssql.Close();
                ssql.Open();
                int result = cmdnotes.ExecuteNonQuery();
                string idnos = da.GetFunction("select idno from home_work where subjectno=" + subno + " and Date='" + dtAttendanceDate1.ToString("MM/dd/yyyy") + "' order by idno desc");
                for (int j = 0; j < arr.Count; j++)
                {
                    string app_no = Convert.ToString(arr[j]);
                    String qry = "if exists(select * from  stud_homework_status where app_no='" + app_no + "' and homework_id=" + idnos + " and date='" + dtAttendanceDate1.ToString("MM/dd/yyyy") + "')update stud_homework_status set app_no='" + app_no + "' , homework_id=" + idnos + " , date='" + dtAttendanceDate1.ToString("MM/dd/yyyy") + "' where app_no='" + app_no + "' and homework_id='" + idnos + "' and date='" + dtAttendanceDate1.ToString("MM/dd/yyyy") + "' else insert into stud_homework_status values('" + app_no + "'," + idnos + ",'" + dtAttendanceDate1.ToString("MM/dd/yyyy") + "',0,0)";
                    int x = da.update_method_wo_parameter(qry, "Text");
                    string token = da.GetFunction("select fcm_token from Registration where app_no='" + app_no + "'");
                    if (token != "")
                    {
                        if (x == 1)
                        {
                            // heading = "Student Login:" + txthomework.Text + " " + "~" + app_no + "~" + idnos + "~" + dtAttendanceDate1.ToString("MM/dd/yyyy");
                            heading = "Student Login" + "~" + app_no + "~" + idnos + "~" + dtAttendanceDate1.ToString("MM/dd/yyyy");
                            ns.SendMessage(token, heading, txthomework.Text);//abarna

                        }

                    }
                }
                if (result > 0)
                {
                    loadgview();
                    lbldel.Text = "";
                    lbldel.Text = "";
                    txtheading.Text = "";
                    txthomework.Text = "";
                    lblshowpic.Visible = false;
                    lnkdelpic.Visible = false;
                    lblshowdoc.Visible = false;
                    lnkdeldoc.Visible = false;
                    fudfile.Visible = true;
                    fudattachemntss.Visible = true;
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                    saveflag = true;
                }
            }
            else
            {
                //string subnmee = ddlclassnotes.SelectedItem.Text;
                //if (subnmee == "All")
                //{
                int cont = 0;
                for (int ji = 1; ji < ddlselectmanysub.Items.Count; ji++)
                {
                    string[] subsplit = ddlselectmanysub.Items[ji].Value.Split('-');// globsubno.Split('-');
                    subno = subsplit[2];
                    if (subsplit.GetUpperBound(0) == 7)
                    {
                        section = subsplit[3];
                    }
                    else
                    {
                        section = "";
                    }

                    SqlCommand cmdnotes = new SqlCommand();

                    if (picsize == 0 && filesize == 0)
                    {
                        cmdnotes.CommandText = "if exists(select * from Home_Work where idno='" + idno + "') update Home_Work set Homework=@hmewrk,Heading=@head where idno='" + idno + "'  else insert into Home_Work(subjectno,Date,Homework,Section,PhotoAttachment,PhotoContentType,PhotoData,FileAttachment,FileContentType,FileData,Heading)" + " VALUES (@subjno,@date,@hmewrk,@section,@picname,@pictype,@picdata,@filename,@filetype,@filedata,@head)";
                    }
                    else if (picsize != 0 && filesize != 0)
                    {
                        cmdnotes.CommandText = "if exists(select * from Home_Work where idno='" + idno + "') update Home_Work set Homework=@hmewrk,Heading=@head,PhotoAttachment=@picname,PhotoContentType=@pictype,PhotoData=@picdata,FileAttachment=@filename,FileContentType=@filetype,FileData=@filedata where idno='" + idno + "'  else insert into Home_Work(subjectno,Date,Homework,Section,PhotoAttachment,PhotoContentType,PhotoData,FileAttachment,FileContentType,FileData,Heading)" + " VALUES (@subjno,@date,@hmewrk,@section,@picname,@pictype,@picdata,@filename,@filetype,@filedata,@head)";
                    }
                    else if (picsize != 0 && filesize == 0)
                    {
                        cmdnotes.CommandText = "if exists(select * from Home_Work where idno='" + idno + "') update Home_Work set Homework=@hmewrk,Heading=@head,PhotoAttachment=@picname,PhotoContentType=@pictype,PhotoData=@picdata where idno='" + idno + "'  else insert into Home_Work(subjectno,Date,Homework,Section,PhotoAttachment,PhotoContentType,PhotoData,FileAttachment,FileContentType,FileData,Heading)" + " VALUES (@subjno,@date,@hmewrk,@section,@picname,@pictype,@picdata,@filename,@filetype,@filedata,@head)";
                    }
                    else if (picsize == 0 && filesize != 0)
                    {
                        cmdnotes.CommandText = "if exists(select * from Home_Work where idno='" + idno + "') update Home_Work set Homework=@hmewrk,Heading=@head,FileAttachment=@filename,FileContentType=@filetype,FileData=@filedata where idno='" + idno + "'  else insert into Home_Work(subjectno,Date,Homework,Section,PhotoAttachment,PhotoContentType,PhotoData,FileAttachment,FileContentType,FileData,Heading)" + " VALUES (@subjno,@date,@hmewrk,@section,@picname,@pictype,@picdata,@filename,@filetype,@filedata,@head)";
                    }

                    cmdnotes.CommandType = CommandType.Text;
                    cmdnotes.Connection = ssql;

                    SqlParameter subjno = new SqlParameter("@subjno", SqlDbType.Int, 100);
                    subjno.Value = Convert.ToInt32(subno);
                    cmdnotes.Parameters.Add(subjno);

                    SqlParameter uploadedDate = new SqlParameter("@date", SqlDbType.DateTime, 50);
                    uploadedDate.Value = dtAttendanceDate1;
                    cmdnotes.Parameters.Add(uploadedDate);

                    SqlParameter homewrk = new SqlParameter("@hmewrk", SqlDbType.NVarChar, 1500);
                    homewrk.Value = hmewrk.ToString();
                    cmdnotes.Parameters.Add(homewrk);

                    SqlParameter sec = new SqlParameter("@section", SqlDbType.NVarChar, 100);
                    sec.Value = section.ToString();
                    cmdnotes.Parameters.Add(sec);

                    SqlParameter picnme = new SqlParameter("@picname", SqlDbType.NVarChar, 100);
                    picnme.Value = fileName.ToString();
                    cmdnotes.Parameters.Add(picnme);

                    SqlParameter pictype = new SqlParameter("@pictype", SqlDbType.NVarChar, 100);
                    pictype.Value = documentType.ToString();
                    cmdnotes.Parameters.Add(pictype);

                    SqlParameter picdata = new SqlParameter("@picdata", SqlDbType.Binary, picsize);
                    picdata.Value = picbinary;
                    cmdnotes.Parameters.Add(picdata);

                    SqlParameter filenme = new SqlParameter("@filename", SqlDbType.NVarChar, 100);
                    filenme.Value = fileName1.ToString();
                    cmdnotes.Parameters.Add(filenme);

                    SqlParameter filetype = new SqlParameter("@filetype", SqlDbType.NVarChar, 100);
                    filetype.Value = documentType1.ToString();
                    cmdnotes.Parameters.Add(filetype);

                    SqlParameter filedata = new SqlParameter("@filedata", SqlDbType.Binary, filesize);
                    filedata.Value = filebinary;
                    cmdnotes.Parameters.Add(filedata);

                    SqlParameter head = new SqlParameter("@head", SqlDbType.NVarChar, 100);
                    head.Value = heading.ToString();
                    cmdnotes.Parameters.Add(head);

                    ssql.Close();
                    ssql.Open();
                    int result = cmdnotes.ExecuteNonQuery();
                    if (result > 0)
                    {
                        cont++;
                    }
                    string idnos = da.GetFunction("select idno from home_work where subjectno=" + subno + " and Date='" + dtAttendanceDate1.ToString("MM/dd/yyyy") + "' order by idno desc");
                    for (int j = 0; j < arr.Count; j++)
                    {
                        string app_no = Convert.ToString(arr[j]);
                        String qry = "if exists(select * from  stud_homework_status where app_no='" + app_no + "' and homework_id=" + idnos + " and date='" + dtAttendanceDate1.ToString("MM/dd/yyyy") + "')update stud_homework_status set app_no='" + app_no + "' , homework_id=" + idnos + " , date='" + dtAttendanceDate1.ToString("MM/dd/yyyy") + "' where app_no='" + app_no + "' and homework_id='" + idnos + "' and date='" + dtAttendanceDate1.ToString("MM/dd/yyyy") + "' else insert into stud_homework_status values('" + app_no + "'," + idnos + ",'" + dtAttendanceDate1.ToString("MM/dd/yyyy") + "',0,0)";
                        int x = da.update_method_wo_parameter(qry, "Text");
                        string token = da.GetFunction("select fcm_token from Registration where app_no='" + app_no + "'");
                        if (token != "")
                        {
                            if (x == 1)
                            {




                                heading = "Student Login" + "~" + app_no + "~" + idnos + "~" + dtAttendanceDate1.ToString("MM/dd/yyyy");

                                ns.SendMessage(token, txtheading.Text, txthomework.Text);//abarna

                            }

                        }
                    }
                }
                if (cont > 0)
                {
                    loadgview();
                    lbldel.Text = "";
                    lbldel.Text = "";
                    txtheading.Text = "";
                    txthomework.Text = "";
                    lblshowpic.Visible = false;
                    lnkdelpic.Visible = false;
                    lblshowdoc.Visible = false;
                    lnkdeldoc.Visible = false;
                    fudfile.Visible = true;
                    fudattachemntss.Visible = true;
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                    saveflag = true;
                }
                //}
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }

    protected bool FileTypeCheck(FileUpload UploadFile, ref string fileName, ref string fileExtension, ref string documentType)
    {
        bool fileBool = false;
        try
        {
            if (UploadFile.FileName.EndsWith(".jpg") || UploadFile.FileName.EndsWith(".gif") || UploadFile.FileName.EndsWith(".png") || UploadFile.FileName.EndsWith(".txt") || UploadFile.FileName.EndsWith(".doc") || UploadFile.FileName.EndsWith(".xls") || UploadFile.FileName.EndsWith(".docx") || UploadFile.FileName.EndsWith(".txt") || UploadFile.FileName.EndsWith(".document") || UploadFile.FileName.EndsWith(".xls") || UploadFile.FileName.EndsWith(".xlsx") || UploadFile.FileName.EndsWith(".pdf") || UploadFile.FileName.EndsWith(".ppt") || UploadFile.FileName.EndsWith(".pptx"))
            {
                fileName = Path.GetFileName(UploadFile.PostedFile.FileName);
                fileExtension = Path.GetExtension(UploadFile.PostedFile.FileName);
                documentType = string.Empty;
                switch (fileExtension)
                {
                    case ".pdf":
                        documentType = "application/pdf";
                        break;
                    case ".xls":
                        documentType = "application/vnd.ms-excel";
                        break;
                    case ".xlsx":
                        documentType = "application/vnd.ms-excel";
                        break;
                    case ".doc":
                        documentType = "application/vnd.ms-word";
                        break;
                    case ".docx":
                        documentType = "application/vnd.ms-word";
                        break;
                    case ".gif":
                        documentType = "image/gif";
                        break;
                    case ".png":
                        documentType = "image/png";
                        break;
                    case ".jpg":
                        documentType = "image/jpg";
                        break;
                    case ".ppt":
                        documentType = "application/vnd.ms-ppt";
                        break;
                    case ".pptx":
                        documentType = "application/vnd.ms-pptx";
                        break;
                    case ".txt":
                        documentType = "application/txt";
                        break;
                }
                if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(fileExtension) && !string.IsNullOrEmpty(documentType))
                    fileBool = true;
            }
        }
        catch { return fileBool; }
        return fileBool;
    }

    protected bool FileTypeCheck1(FileUpload UploadFile1, ref string fileName1, ref string fileExtension1, ref string documentType1)
    {
        bool fileBool = false;
        try
        {
            if (UploadFile1.FileName.EndsWith(".jpg") || UploadFile1.FileName.EndsWith(".gif") || UploadFile1.FileName.EndsWith(".png") || UploadFile1.FileName.EndsWith(".txt") || UploadFile1.FileName.EndsWith(".doc") || UploadFile1.FileName.EndsWith(".xls") || UploadFile1.FileName.EndsWith(".docx") || UploadFile1.FileName.EndsWith(".txt") || UploadFile1.FileName.EndsWith(".document") || UploadFile1.FileName.EndsWith(".xls") || UploadFile1.FileName.EndsWith(".xlsx") || UploadFile1.FileName.EndsWith(".pdf") || UploadFile1.FileName.EndsWith(".ppt") || UploadFile1.FileName.EndsWith(".pptx"))
            {
                fileName1 = Path.GetFileName(UploadFile1.PostedFile.FileName);
                fileExtension1 = Path.GetExtension(UploadFile1.PostedFile.FileName);
                documentType1 = string.Empty;
                switch (fileExtension1)
                {
                    case ".pdf":
                        documentType1 = "application/pdf";
                        break;
                    case ".xls":
                        documentType1 = "application/vnd.ms-excel";
                        break;
                    case ".xlsx":
                        documentType1 = "application/vnd.ms-excel";
                        break;
                    case ".doc":
                        documentType1 = "application/vnd.ms-word";
                        break;
                    case ".docx":
                        documentType1 = "application/vnd.ms-word";
                        break;
                    case ".gif":
                        documentType1 = "image/gif";
                        break;
                    case ".png":
                        documentType1 = "image/png";
                        break;
                    case ".jpg":
                        documentType1 = "image/jpg";
                        break;
                    case ".ppt":
                        documentType1 = "application/vnd.ms-ppt";
                        break;
                    case ".pptx":
                        documentType1 = "application/vnd.ms-pptx";
                        break;
                    case ".txt":
                        documentType1 = "application/txt";
                        break;
                }
                if (!string.IsNullOrEmpty(fileName1) && !string.IsNullOrEmpty(fileExtension1) && !string.IsNullOrEmpty(documentType1))
                    fileBool = true;
            }
        }
        catch { return fileBool; }
        return fileBool;
    }

    protected void btnaddhme_Click()
    {
        try
        {
            //divcontrol.Visible = true;
            //tablediv.Visible = true;
            lblsubtext.Text = "";
            pBodyhomework.Visible = true;
            Tablenote.Visible = true;
            txtheading.Text = "";
            txthomework.Text = "";
            lbldel.Text = "";
            fudfile.Visible = true;
            fudattachemntss.Visible = true;
            lblshowpic.Visible = false;
            lnkdelpic.Visible = false;
            lblshowdoc.Visible = false;
            lnkdeldoc.Visible = false;
            if (ddlselectmanysub.SelectedValue != "" && ddlselectmanysub.SelectedValue != " ")
            {
                string sub = ddlselectmanysub.SelectedItem.Text;
                string[] spl = sub.Split('-');
                lblsubtext.Text = spl[0];
            }
            else
            {
                lblsubtext.Text = "All";
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }

    protected void gviewhme_onRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[4];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void gviewhme_selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);

            lblshowpic.Visible = false;
            lnkdelpic.Visible = false;
            lblshowdoc.Visible = false;
            lnkdeldoc.Visible = false;
            fudfile.Visible = true;
            fudattachemntss.Visible = true;
            lblattachements.Visible = true;
            string unid = (gviewhomewrk.Rows[rowIndex].FindControl("lbluniq") as Label).Text;
            string topic = (gviewhomewrk.Rows[rowIndex].FindControl("lbltopic") as Label).Text;
            string subject = (gviewhomewrk.Rows[rowIndex].FindControl("lblsubject") as Label).Text;
            string picattach = (gviewhomewrk.Rows[rowIndex].FindControl("lnkDownloadpic") as LinkButton).Text;
            string fileattach = (gviewhomewrk.Rows[rowIndex].FindControl("lnkDownloadfile") as LinkButton).Text;
            string head = (gviewhomewrk.Rows[rowIndex].FindControl("lblhead") as Label).Text;

            lbldel.Text = unid;

            txtheading.Text = head;
            txthomework.Text = topic;
            lblsubtext.Text = subject;
            if (!string.IsNullOrEmpty(picattach))
            { lblshowpic.Text = picattach; lblshowpic.Visible = true; lnkdelpic.Visible = true; fudfile.Visible = false; }
            if (!string.IsNullOrEmpty(fileattach))
            { lblshowdoc.Text = fileattach; lblshowdoc.Visible = true; lnkdeldoc.Visible = true; fudattachemntss.Visible = false; }

            Tablenote.Visible = true;
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }

    protected void lnkDownloadpic_click(object sender, EventArgs e)
    {
        try
        {
            string activerow = string.Empty;
            string activecol = string.Empty;


            LinkButton lnkSelected = (LinkButton)sender;
            string rowIndxS = lnkSelected.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            string iduni = (gviewhomewrk.Rows[rowIndx].FindControl("lbluniq") as Label).Text;
            int colIndx = 5;
            DataSet dspicture = new DataSet();

            activerow = rowIndx.ToString();
            activecol = colIndx.ToString();

            if (Convert.ToInt32(activecol) == 5)
            {
                string qrys = "select PhotoAttachment,PhotoContentType,PhotoData from Home_Work where idno='" + iduni + "'";
                dspicture.Clear();
                dspicture = da.select_method_wo_parameter(qrys, "Text");
                if (dspicture.Tables.Count > 0 && dspicture.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dspicture.Tables[0].Rows.Count; i++)
                    {
                        Response.ContentType = dspicture.Tables[0].Rows[i]["PhotoContentType"].ToString();
                        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + dspicture.Tables[0].Rows[i]["PhotoAttachment"] + "\"");
                        Response.BinaryWrite((byte[])dspicture.Tables[0].Rows[i]["PhotoData"]);
                        Response.End();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }

    protected void lnkDownloadfile_click(object sender, EventArgs e)
    {
        try
        {
            string activerow = string.Empty;
            string activecol = string.Empty;


            LinkButton lnkSelected = (LinkButton)sender;
            string rowIndxS = lnkSelected.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            string iduni = (gviewhomewrk.Rows[rowIndx].FindControl("lbluniq") as Label).Text;
            int colIndx = 6;
            DataSet dspicture1 = new DataSet();

            activerow = rowIndx.ToString();
            activecol = colIndx.ToString();

            if (Convert.ToInt32(activecol) == 6)
            {
                string qrys = "select FileAttachment,FileContentType,FileData from Home_Work where idno='" + iduni + "'";
                dspicture1.Clear();
                dspicture1 = da.select_method_wo_parameter(qrys, "Text");
                if (dspicture1.Tables.Count > 0 && dspicture1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dspicture1.Tables[0].Rows.Count; i++)
                    {
                        Response.ContentType = dspicture1.Tables[0].Rows[i]["FileContentType"].ToString();
                        Response.AddHeader("Content-Disposition", "attachment;filename=\"" + dspicture1.Tables[0].Rows[i]["FileAttachment"] + "\"");
                        Response.BinaryWrite((byte[])dspicture1.Tables[0].Rows[i]["FileData"]);
                        Response.End();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }

    protected void loadgview()
    {
        try
        {
            DataSet dsset = new DataSet();
            string sect = string.Empty;
            string sunum = string.Empty;
            dthmwwrk.Rows.Clear();
            dthmwwrk.Columns.Clear();

            dthmwwrk.Columns.Add("sno");
            dthmwwrk.Columns.Add("uniqid");
            dthmwwrk.Columns.Add("Date1");
            dthmwwrk.Columns.Add("Subject1");
            dthmwwrk.Columns.Add("Subjectno");
            dthmwwrk.Columns.Add("Heading1");
            dthmwwrk.Columns.Add("Topic1");
            dthmwwrk.Columns.Add("Photo1");
            dthmwwrk.Columns.Add("Attachment1");

            string ddlvalue = ddlselectmanysub.SelectedValue;
            string[] splt = ddlvalue.Split('-');

            if (ddlvalue != "" && ddlvalue != " ")
            {
                sunum = splt[2];
            }
            if (splt.GetUpperBound(0) == 7)
            {
                sect = splt[3];
            }

            if (string.IsNullOrEmpty(sunum))
            {
                dsset.Clear();
                dsset = da.select_method_wo_parameter("select * from Home_Work order by idno", "Text");
            }
            else
            {
                if (string.IsNullOrEmpty(sect))
                {
                    dsset.Clear();
                    dsset = da.select_method_wo_parameter("select * from Home_Work where subjectno=" + sunum + " order by idno", "Text");
                }
                else
                {
                    dsset.Clear();
                    dsset = da.select_method_wo_parameter("select * from Home_Work where subjectno=" + sunum + " and section='" + sect + "' order by idno", "Text");
                }
            }

            if (dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
            {
                int sno = 0;
                for (int i = 0; i < dsset.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    drhmewrk = dthmwwrk.NewRow();

                    string uniq = Convert.ToString(dsset.Tables[0].Rows[i]["idno"]);
                    string date1 = Convert.ToString(dsset.Tables[0].Rows[i]["Date"]);
                    string[] datesplit = date1.Split(' ');
                    string date = datesplit[0];
                    string subjectno = Convert.ToString(dsset.Tables[0].Rows[i]["subjectno"]);
                    string subjectname = da.GetFunction("Select Subject_Name from subject where subject_no='" + subjectno + "'");
                    string headng = Convert.ToString(dsset.Tables[0].Rows[i]["Heading"]);
                    string topic = Convert.ToString(dsset.Tables[0].Rows[i]["Homework"]);
                    string photo = Convert.ToString(dsset.Tables[0].Rows[i]["PhotoAttachment"]);
                    string file = Convert.ToString(dsset.Tables[0].Rows[i]["FileAttachment"]);

                    drhmewrk["sno"] = sno.ToString();
                    drhmewrk["uniqid"] = uniq;
                    drhmewrk["Date1"] = date;
                    drhmewrk["Subject1"] = subjectname;
                    drhmewrk["Subjectno"] = subjectno;
                    drhmewrk["Heading1"] = headng;
                    drhmewrk["Topic1"] = topic;
                    drhmewrk["Photo1"] = photo;
                    drhmewrk["Attachment1"] = file;

                    dthmwwrk.Rows.Add(drhmewrk);
                }
                gviewhomewrk.DataSource = dthmwwrk;
                gviewhomewrk.DataBind();
                gviewhomewrk.Visible = true;
            }
            else
            {
                gviewhomewrk.Visible = false;
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, "13", "NewStaff.aspx");
        }
    }

    protected void btndeletewrk_Click(object sender, EventArgs e)
    {
        hat1.Clear();
        string unique = lbldel.Text;
        string qurys = "delete from Home_Work where idno='" + unique + "'";
        int delet = da.insert_method(qurys, hat1, "Text");
        if (delet > 0)
        {
            lbldel.Text = "";
            txtheading.Text = "";
            txthomework.Text = "";
            fudfile.Visible = true;
            fudattachemntss.Visible = true;
            lblshowpic.Visible = false;
            lnkdelpic.Visible = false;
            lblshowdoc.Visible = false;
            lnkdeldoc.Visible = false;
            loadgview();
        }
    }

    protected void lnlremovepic(object sender, EventArgs e)
    {
        hat1.Clear();
        string idnum = lbldel.Text;
        string delpic = "update Home_Work set PhotoAttachment='',PhotoContentType='',PhotoData=0 where idno='" + idnum + "'";
        int k = da.insert_method(delpic, hat1, "Text");
        if (k > 0)
        {
            lblshowpic.Visible = false;
            lnkdelpic.Visible = false;
            lblfile.Visible = true;
            fudfile.Visible = true;
            loadgview();
        }
    }

    protected void lnlremovedoc(object sender, EventArgs e)
    {
        hat1.Clear();
        string idnum = lbldel.Text;
        string delpic = "update Home_Work set FileAttachment='',FileContentType='',FileData=0 where idno='" + idnum + "'";
        int k = da.insert_method(delpic, hat1, "Text");
        if (k > 0)
        {
            lblshowdoc.Visible = false;
            lnkdeldoc.Visible = false;
            lblattachements.Visible = true;
            fudattachemntss.Visible = true;
            loadgview();
        }
    }

    #endregion

}


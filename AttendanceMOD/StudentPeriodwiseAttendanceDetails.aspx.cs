using System;
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

using System.Globalization;

public partial class Student_Absenties_Report : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbranch = string.Empty;
    static string grouporusercode = "";


    string pp = "";
    int sk = 0;
    Hashtable hat = new Hashtable();
    Hashtable hat_days_first = new Hashtable();
    Hashtable hat_days_end = new Hashtable();
    Hashtable hat_holy = new Hashtable();
    Hashtable hatsetrights = new Hashtable();
    DataSet newds = new DataSet();

    DataTable dtNew = new DataTable();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet testds = new DataSet();
    DataSet ds_holi = new DataSet();
    DataSet ds_attndmaster = new DataSet();

    int count_master = 0;
    string absent_calcflag = "";
    Hashtable absent_hash = new Hashtable();
    int sqlstrq = 0;
    DataSet dsprint = new DataSet();
    Hashtable has_visible_column = new Hashtable();
    int col_count = 0;
    int count = 0;
    string new_header_string = "";
    string[] new_header_string_split;
    int rc = 0;

    string date1 = "";
    int endk = 0;
    string datefrom = "";
    string dateto = "";
    string date2 = "";
    int day_val = 0;
    int day_diff = 0;
    int sno = 0;
    Boolean sflag = false;
    Boolean rowflag = false;
    Boolean dayflag = false;
    DateTime dt1, dt2;
    DateTime date_today;
    string abshrs_temp = "";
    string abshrs_list = "";
    double totpresentday;
    double perprest, perpresthrs, perabsent, perabsenthrs, perondu, peronduhrs, perleave, perleavehrs;
    double pertothrs, pertotondu, pertotleavehrs, pertotabsenthrs, onduday, cumcontotpresentday, percontotpresentday, hollyhrs, condhrs, condhrs_2, balamonday, att_points;
    int i = 0, minI, minII, perdayhrs, wk1, wk2, wk3, wk4, wk5, wk6, wk7, wk8, wk9, Ihof, IIhof, fullday, cumfullday, cc = 0;
    double cumperprest, cumperpresthrs, cumperabsent, cumperabsenthrs, cumperondu, cumperonduhrs, cumperleave, cumperleavehrs, checkpre, baldate, totmonth, cummcc, cumcondhrs, percondhrs = 0, cumatt_points;
    string m7, m2, m3, m4, m5, m6, m1, m8, m9;
    Double totalRows = 0;

    int hour1, hour2, hour3, hour4, hour5, hour6, hour7, hour8, hour9, condhrs1, condhrs2, condhrs3, condhrs4, condhrs5, condhrs6, condhrs7, condhrs8, condhrs9;
    int ondu1, ondu2, ondu3, ondu4, ondu5, ondu6, ondu7, ondu8, ondu9, leave1, leave2, leave3, leave4, leave5, leave6, leave7, leave8, leave9;
    string holi_month;
    string fmLength;

    int holi_days, abse_point, leave_point;

    Boolean unmark_flag = false;
    double par = 0, abse = 0;
    double present, absent, hollydats, leaves, ondu;
    double presenthrs, absenthrs, hollydatshrs, leaveshrs, onduhrs;
    int perhr, abshr, rcc = 0;
    int ond, le, fyyy, mm = 1, att;
    int daycount, betdays;
    int dat, dumm;
    double onhr, lehr;
    int fm, fyy, fd, tm, tyy, td, fcal, tcal, k;
    double wkhr, wkhd, dumwkhr, dumwkhd, dumper, per;
    int kk = 0, cumdays, printcheck;
    string roll_no, reg_no, roll_ad, studname;
    double dumprest, dumpresthrs, dumpresenthrs, dumleaveshrs, dumonduhrs, dumabsenthrs, dumabsent, dumondu, dumleavehrs, dumleave, attday, dumattday;
    int diff = 1, att2, lea1, lea2, on_1, on_2, hdate = 0;
    double holldays, totworkday, dumtotworkday, dumperhrs, dumtoterhrs, perhrs, totperhrs;
    string frdate, todate;
    string regularflag = "";
    string genderflag = "";
    string strdayflag = "";
    static string[] string_session_values;
    DataSet ds_sphr = new DataSet();
    static Hashtable ht_sphr = new Hashtable();


    //added by annyutha//
    string strsec;
    string rstrsec1;
    string splhrsec;
    Boolean chkflag = false;
    Hashtable has = new Hashtable();
    Hashtable has_attnd_masterset = new Hashtable();
    Hashtable has_attnd_masterset_notconsider = new Hashtable();
    Hashtable hatabsentvalues = new Hashtable();
    DateTime temp_date = new DateTime();
    string full_hour = "";
    string single_hour = "";

    Boolean recflag = false;
    int mng_hrs = 0, evng_hrs = 0;
    int no_of_hrs = 0;
    Boolean holiflag = false;
    string strDay = "", dummy_date = "", temp_hr_field = "", subject_no = "";
    string order = "";
    string date_temp_field = "", month_year = "";
    Hashtable temp_has_subj_code = new Hashtable();
    Boolean check_alter = false;
    DataSet ds_subject = new DataSet();
    DataSet stabsteen = new DataSet();
    DataSet dsalldetails = new DataSet();
    Boolean sunday_holiday = false;
    List<DateTime> li = new List<DateTime>();
    Hashtable checking = new Hashtable();
    Boolean setcheck = false;
    Boolean norecord = false;
    string errordate = "";

    string rollno = "";
    string regno = "";
    string sname = "";
    string pp1 = "";
    int pct, act, odct, mlct, sodct, nssct, hct, njct, sct, lct, nccct, hsct, ppct, syodct, codct, oodct, lact, nect = 0;
    int pct1, act1, odct1, mlct1, sodct1, nssct1, hct1, njct1, sct1, lct1, nccct1, hsct1, ppct1, syodct1, codct1, oodct1, lact1, nect1 = 0;

    DAccess2 da = new DAccess2();

    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();

    DateTime per_from_date;
    DateTime per_to_date;
    DateTime per_from_gendate;
    DateTime per_to_gendate;
    DateTime dumm_from_date;
    DateTime Admission_date;

    TimeSpan ts;
    Boolean deptflag = false;

    string batch = "";
    string degree = "";
    string sem = "";
    string sections = "";
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    string value, date;
    string tempvalue = "-1";
    string value_holi_status = "";
    string split_holiday_status_1 = "", split_holiday_status_2 = "";
    string dd = "", diff_date = "";

    string[] split_holiday_status = new string[1000];

    double dif_date = 0;
    double dif_date1 = 0;
    double per_perhrs, per_abshrs;
    double per_hhday;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double workingdays = 0;
    double per_workingdays = 0;
    double leave_pointer, absent_pointer;
    double absent_point;
    double per_holidate;
    double njhr, njdate, per_njdate;
    double per_per_hrs;

    Double minpresday = 0, nohrsprsentperday = 0, noofdaypresen = 0;
    Double moringabsentfine = 0, eveingabsentfine = 0, studentabsentfine = 0;


    int mmyycount = 0;
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0, tot_ml_spl = 0;
    int tot_conduct_hr_spl = 0;
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0;
    int notconsider_value = 0;
    int moncount;
    int unmark;
    int NoHrs = 0;
    int fnhrs = 0;
    int minpresI = 0;
    int next = 0;
    int minpresII = 0;
    int rows_count;
    int ObtValue = -1;
    int cal_from_date, cal_from_date_tmp;
    int cal_to_date, cal_to_date_tmp;
    int per_dum_unmark, dum_unmark;
    int tot_per_hrs;
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;

    //added by rajasekar 22/08/2018
    DataTable dtt = new DataTable();
    DataRow dtrow = null;
    string colfitroll = "";
    string colfitreg = "";
    string colfitadm = "";
    //============================//

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();

        lblnorec.Visible = false;
        errmsg.Visible = false;

        if (!IsPostBack)
        {
            if (ddl_rtype.SelectedItem.Value == "0")
            {
                lblfrom.Text = "Date";
                lblto.Visible = false;
                txtto.Visible = false;
            }
            else
            {
                lblfrom.Text = "From Date";
                lblto.Visible = true;
                txtto.Visible = true;
            }
            

            

            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            Session["attdaywisecla"] = "0";
            string daywisecal = da.GetFunction("select value from Master_Settings where settings='Attendance Day Wise Calculation'");
            if (daywisecal.Trim() == "1")
            {
                Session["attdaywisecla"] = "1";
            }

            if (Session["usercode"] != "")
            {
                string Master = "select * from Master_Settings where " + grouporusercode + "";
                DataSet ds = da.select_method(Master, hat, "Text");
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
                }
            }
            txtfrom.Attributes.Add("readonly", "readonly");
            txtto.Attributes.Add("readonly", "readonly");
            lblnorec.Visible = false;
            errmsg.Visible = false;
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            if (txtdegree.Enabled == true)
            {
                txtdegree.Enabled = true;
                txtbranch.Enabled = true;
                btngo.Enabled = true;
                txtfrom.Enabled = true;
                BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                BindSectransport(strbatch, strbranch);
                BindSectionDetail(strbatch, strbranch);
                txtfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtto.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                txtdegree.Enabled = false;
                txtbranch.Enabled = false;
                btngo.Enabled = false;
                txtfrom.Enabled = false;
            }
        }
    }
    public void BindBatch()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            //ds2 = da.BindBatch();

            string Master1 = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {

                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
            }
            else
            {
                Master1 = Session["usercode"].ToString();
            }
            string collegecode = Session["collegecode"].ToString();
            string strbinddegree = "select distinct batch_year from tbl_attendance_rights where user_id='" + Master1 + "' order by batch_year desc";
            ds2 = da.select_method_wo_parameter(strbinddegree, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklsbatch.DataSource = ds2;
                chklsbatch.DataTextField = "Batch_year";
                chklsbatch.DataValueField = "Batch_year";
                chklsbatch.DataBind();
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    if (chklsbatch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklsbatch.Items.Count == count)
                    {
                        chkbatch.Checked = true;
                    }

                }
            }


        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            count = 0;

            chklstdegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = da.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstdegree.DataSource = ds2;
                chklstdegree.DataTextField = "course_name";
                chklstdegree.DataValueField = "course_id";
                chklstdegree.DataBind();
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    if (chklstdegree.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstdegree.Items.Count == count)
                    {
                        chkdegree.Checked = true;
                    }
                }
                txtdegree.Enabled = true;
            }
            else
            {
                txtdegree.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }

    }

    public void BindBranch(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            count = 0;

            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = da.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstbranch.DataSource = ds2;
                chklstbranch.DataTextField = "dept_name";
                chklstbranch.DataValueField = "degree_code";
                chklstbranch.DataBind();
                chklstbranch.Items[0].Selected = true;
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    if (chklstbranch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstbranch.Items.Count == count)
                    {
                        chkbranch.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = "Please Select the Degree";
        }

    }

    public void BindSectransport(string strbatch, string strbranch)
    {
        try
        {
            count = 0;

            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    if (strbatch == "")
                    {
                        strbatch = "'" + chklsbatch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbatch = strbatch + "," + "'" + chklsbatch.Items[i].Value.ToString() + "'";
                    }
                }
            }

            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    if (strbranch == "")
                    {
                        strbranch = "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbranch = strbranch + "," + "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = " Please Select the Branch";
        }
    }

    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            count = 0;

            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = da.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstbranch.DataSource = ds2;
                chklstbranch.DataTextField = "dept_name";
                chklstbranch.DataValueField = "degree_code";
                chklstbranch.DataBind();
                chklstbranch.Items[0].Selected = true;
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    if (chklstbranch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstbranch.Items.Count == count)
                    {
                        chkbranch.Checked = true;
                    }
                }
            }
            BindSectionDetail(strbatch, strbranch);
        }

        catch (Exception ex)
        {
            errmsg.Text = "Please Select the Degree";
        }
    }
    public void BindSectionDetail(string strbatch, string strbranch)
    {
        try
        {
            count = 0;

            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    if (strbatch == "")
                    {
                        strbatch = "'" + chklsbatch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbatch = strbatch + "," + "'" + chklsbatch.Items[i].Value.ToString() + "'";
                    }
                }
            }
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    if (strbranch == "")
                    {
                        strbranch = "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbranch = strbranch + "," + "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                }
            }

            chklssec.Items.Clear();
            ds2.Dispose();
            ds2.Reset();
            ds2 = da.BindSectionDetail(strbatch, strbranch);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklssec.DataSource = ds2;
                chklssec.DataTextField = "sections";
                chklssec.DataBind();
                if (Convert.ToString(ds2.Tables[0].Columns["sections"]) == string.Empty)
                {
                    chklssec.Enabled = false;
                }
                else
                {
                    txtsec.Enabled = true;
                    chklssec.Enabled = true;
                    chklssec.SelectedIndex = chklssec.Items.Count - 2;
                    chklssec.Items[0].Selected = true;
                    for (int i = 0; i < chklssec.Items.Count; i++)
                    {
                        chklssec.Items[i].Selected = true;
                        if (chklssec.Items[i].Selected == true)
                        {
                            count += 1;
                        }
                        if (chklssec.Items.Count == count)
                        {
                            chksec.Checked = true;
                        }
                    }
                }
                chklssec.Items.Insert(0, "Empty Section");
            }
            else
            {
                chklssec.Enabled = false;
                txtsec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = " Please Select the Branch";
        }

    }


    # region Batch CheckChange -Events
    protected void chkbatch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (chkbatch.Checked == true)
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                    txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = false;
                    txtbatch.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            string clg = "";
            int commcount = 0;
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtbatch.Text = "Batch(" + commcount.ToString() + ")";
                    if (clg == "")
                    {
                        clg = chklsbatch.Items[i].Value.ToString();
                    }
                    else
                    {
                        clg = clg + "','" + chklsbatch.Items[i].Value;
                    }
                }
            }

            if (commcount == 0)
            {
                txtbatch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }

    }
    #endregion

    # region Degree CheckChange -Events
    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkdegree.Checked == true)
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                    txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = false;
                    txtdegree.Text = "---Select---";
                }
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clg = "";
            int commcount = 0;
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtdegree.Text = "Degree(" + commcount.ToString() + ")";
                    if (clg == "")
                    {
                        clg = chklstdegree.Items[i].Value.ToString();
                    }
                    else
                    {
                        clg = clg + "','" + chklstdegree.Items[i].Value;
                    }
                }
            }
            if (commcount == 0)
            {
                txtbatch.Text = "--Select--";
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            chklstbranch_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    #endregion

    # region Branch CheckChange -Events
    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = false;
                    txtbranch.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clg = "";
            int commcount = 0;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtbranch.Text = "Branch(" + commcount.ToString() + ")";
                    if (clg == "")
                    {
                        clg = chklstbranch.Items[i].Value.ToString();
                    }
                    else
                    {
                        clg = clg + "','" + chklstbranch.Items[i].Value;
                    }
                }
            }

            if (commcount == 0)
            {
                txtbatch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    #endregion

    #region Section CheckChange -Events
    protected void chksec_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chksec.Checked == true)
            {
                for (int i = 0; i < chklssec.Items.Count; i++)
                {
                    chklssec.Items[i].Selected = true;
                    txtsec.Text = "Section(" + (chklssec.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklssec.Items.Count; i++)
                {
                    chklssec.Items[i].Selected = false;
                    txtsec.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    protected void chklstsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            for (int i = 0; i < chklssec.Items.Count; i++)
            {
                if (chklssec.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtsec.Text = "Section(" + commcount.ToString() + ")";

                }
            }

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    #endregion

    protected void ddl_rtype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        

        grdover.Visible = false;
        exceldiv.Visible = false;
        Printcontrol.Visible = false;
        if (ddl_rtype.SelectedItem.Value == "0")
        {
            lblfrom.Text = "Date";
            lblto.Visible = false;
            txtto.Visible = false;
            
            grdover.Visible = false;
        }
        else
        {
            lblfrom.Text = "From Date";
            lblto.Visible = true;
            txtto.Visible = true;
        }
    }

    # region Excel Generate Event
    protected void btnxl_Click(object sender, EventArgs e)
    {

        try
        {
            errmsg.Visible = true;
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                
                da.printexcelreportgrid(grdover, reportname);
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
        }
    }
    # endregion

    #region Print Control Event
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string fdt = txtfrom.Text;
        string tdt = txtto.Text;
        string date = "";
        string collacname = da.GetFunction("Select acr from collinfo where college_code='" + Session["collegecode"].ToString() + "'");
        string batch = da.GetFunction("select replace(value,',',' - ') from Master_Settings where settings like 'Academic year'");
        string degreedetails = "";
        if (ddl_rtype.SelectedItem.Value == "0")
        {
            date = "@" + " Date : " + fdt;
            degreedetails = "Daily Absentees Report";
        }
        else
        {
            date = "@" + " From Date : " + fdt + "  To :" + tdt;
            degreedetails = "Monthly Absentees Report";
        }
        degreedetails = collacname + "$" + degreedetails + "$" + batch + date;
        string pagename = "MonthlyStudentPeriodwiseAttendanceDetails.aspx";
        if (ddl_rtype.SelectedItem.Value == "0")
        {
            pagename = "DailyStudentPeriodwiseAttendanceDetails.aspx";
        }
        string ss = null;
        Printcontrol.loadspreaddetails(grdover, pagename, degreedetails, 0, ss);
        Printcontrol.Visible = true;
    }
    #endregion

    protected void btngo_Click(object sender, EventArgs e)
    {

        btnPrint11();
        exceldiv.Visible = false;

        string strbatchsectionrights = "";
        if (Session["Single_User"].ToString() == "True")
        {
            strbatchsectionrights = "and user_id='" + Session["UserCode"].ToString() + "'";
        }
        else
        {
            string groupcode = Session["group_code"].ToString();
            string[] from_split = groupcode.Split(';');
            if (from_split[0].ToString() != "")
            {
                strbatchsectionrights = "and user_id='" + from_split[0].ToString() + "'";
            }
        }
        string batchyear = "";
        if (txtbatch.Text != "---Select---" && txtdegree.Text != "---Select---" && txtbranch.Text != "---Select---")
        {
            if (txtbatch.Text != "--Select--")
            {
                for (int j = 0; j < chklsbatch.Items.Count; j++)
                {
                    if (chklsbatch.Items[j].Selected == true)
                    {
                        if (batchyear.Trim() == "")
                            batchyear = "'" + chklsbatch.Items[j].Value.ToString() + "'";
                        else
                            batchyear = batchyear + "," + "'" + chklsbatch.Items[j].Value.ToString() + "'";
                    }
                }
            }
        }
        if (batchyear.Trim() != "")
        {
            batchyear = " batch_year in(" + batchyear + ")";
        }

        hatsetrights.Clear();
        string strbatchsectionsrights = "select sections,batch_year from tbl_attendance_rights where " + batchyear + " " + strbatchsectionrights + "";
        DataSet dssections = da.select_method_wo_parameter(strbatchsectionsrights, "Text");
        if (dssections.Tables[0].Rows.Count > 0)
        {
            for (int se = 0; se < dssections.Tables[0].Rows.Count; se++)
            {
                string strval = dssections.Tables[0].Rows[se]["sections"].ToString();
                string bathrights = dssections.Tables[0].Rows[se]["batch_year"].ToString();
                string[] spsec = strval.Split(',');
                for (int sp = 0; sp <= spsec.GetUpperBound(0); sp++)
                {
                    string valu = spsec[sp].ToString().Trim();
                    if (!hatsetrights.Contains(bathrights + ',' + valu))
                    {
                        hatsetrights.Add(bathrights + ',' + valu, bathrights + ',' + valu);
                    }
                }
            }
        }
        else
        {
            
            grdover.Visible = false;
            exceldiv.Visible = false;
            lblnorec.Visible = true;
            lblnorec.Text = "Please Update The Batch Year and Sections Rights";
        }

        if (ddl_rtype.SelectedItem.Value == "1")
        {
            lblfrom.Text = "From Date";
            lblto.Visible = true;
            txtto.Visible = true;

            

            if (Session["Rollflag"].ToString() == "0")
            {
                
                colfitroll = "Roll";
                

            }
            if (Session["Regflag"].ToString() == "0")
            {
                
                colfitreg = "Reg";
            }
            
            colfitadm = "Adm";
            first_btngo();
        }
        else
        {
            lblto.Visible = false;
            txtto.Visible = false;
            lblfrom.Text = "Date";
            Bindingspread();
        }
    }
    private void present_mark(string Attstr_mark)
    {
        //int pct = 0, act = 0, odct = 0, mlct = 0, sodct = 0, nssct = 0, hct = 0, njct = 0, sct = 0, lct = 0, nccct = 0, hsct = 0, ppct = 0, syodct = 0, codct = 0, oodct = 0, lact = 0, nect = 0;
        switch (Attstr_mark)
        {

            case "1":
                pp = "P";
                pct++;
                break;
            case "2":
                pp = "A";
                act++;
                break;
            case "3":
                pp = "OD";
                odct++;
                break;
            case "4":
                pp = "ML";
                mlct++;
                break;
            case "5":
                pp = "SOD";
                sodct++;
                break;
            case "6":
                pp = "NSS";
                nssct++;
                break;
            case "7":
                pp = "H";
                hct++;
                break;

            case "8":
                pp = "NJ";
                njct++;
                break;
            case "9":
                pp = "S";
                sct++;
                break;
            case "10":
                pp = "L";
                lct++;
                break;
            case "11":
                pp = "NCC";
                nccct++;
                break;
            case "12":
                pp = "HS";
                hsct++;
                break;
            case "13":
                pp = "PP";
                ppct++;
                break;
            case "14":
                pp = "SYOD";
                syodct++;
                break;
            case "15":
                pp = "COD";
                codct++;
                break;
            case "16":

                pp = "OOD";
                oodct++;
                break;
            case "17":
                pp = "LA";
                lact++;
                break;
            default:
                pp = "NE";
                nect++;
                break;
        }
    }
    private void present_mark2(string Attstr_mark)
    {
        //int pct1 = 0, act1 = 0, odct1 = 0, mlct1 = 0, sodct1 = 0, nssct1 = 0, hct1 = 0, njct1 = 0, sct1 = 0, lct1 = 0, nccct1 = 0, hsct1 = 0, ppct1 = 0, syodct1 = 0, codct1 = 0, oodct1 = 0, lact1 = 0, nect1 = 0;
        switch (Attstr_mark)
        {

            case "1":
                pp1 = "P";
                pct1++;
                break;
            case "2":
                pp1 = "A";
                act1++;
                break;
            case "3":
                pp1 = "OD";
                odct1++;
                break;
            case "4":
                pp1 = "ML";
                mlct1++;
                break;
            case "5":
                pp1 = "SOD";
                sodct1++;
                break;
            case "6":
                pp1 = "NSS";
                nssct1++;
                break;
            case "7":
                pp1 = "H";
                hct1++;
                break;

            case "8":
                pp1 = "NJ";
                njct1++;
                break;
            case "9":
                pp1 = "S";
                sct1++;
                break;
            case "10":
                pp1 = "L";
                lct1++;
                break;
            case "11":
                pp1 = "NCC";
                nccct1++;
                break;
            case "12":
                pp1 = "HS";
                hsct1++;
                break;
            case "13":
                pp1 = "PP";
                ppct1++;
                break;
            case "14":
                pp1 = "SYOD";
                syodct1++;
                break;
            case "15":
                pp1 = "COD";
                codct1++;
                break;
            case "16":

                pp1 = "OOD";
                oodct1++;
                break;
            case "17":
                pp1 = "LA";
                lact1++;
                break;
            default:
                pp1 = "NE";
                nect1++;
                break;
        }
    }
    //protected void Bindingspread()
    //{
    //    try
    //    {
    //        Hashtable hat = new Hashtable();
    //        string batchyear = "";
    //        if (txtbatch.Text != "--Select--")
    //        {
    //            for (int j = 0; j < chklsbatch.Items.Count; j++)
    //            {
    //                if (chklsbatch.Items[j].Selected == true)
    //                {
    //                    if (batchyear == "")
    //                        batchyear = "'" + chklsbatch.Items[j].Value.ToString() + "'";
    //                    else
    //                        batchyear = batchyear + "," + "'" + chklsbatch.Items[j].Value.ToString() + "'";
    //                }
    //            }
    //        }
    //        string degree = "";
    //        if (txtdegree.Text != "--Select--")
    //        {
    //            for (int s = 0; s < chklstdegree.Items.Count; s++)
    //            {
    //                if (chklstdegree.Items[s].Selected == true)
    //                {
    //                    if (degree == "")
    //                        degree = "'" + chklstdegree.Items[s].Value.ToString() + "'";
    //                    else
    //                        degree = degree + "," + "'" + chklstdegree.Items[s].Value.ToString() + "'";
    //                }
    //            }
    //        }

    //        string branch = "";
    //        if (txtbranch.Text != "--Select--")
    //        {

    //            for (int k = 0; k < chklstbranch.Items.Count; k++)
    //            {
    //                if (chklstbranch.Items[k].Selected == true)
    //                {
    //                    if (branch == "")
    //                        branch = "'" + chklstbranch.Items[k].Value.ToString() + "'";
    //                    else
    //                        branch = branch + "," + "'" + chklstbranch.Items[k].Value.ToString() + "'";
    //                }
    //            }
    //        }
    //        string fdate = txtfrom.Text;
    //        string tdate = txtto.Text;
    //        string[] split = fdate.Split('/');
    //        DateTime dt = new DateTime();
    //        DateTime dt1 = new DateTime();
    //        if (batchyear != "" && degree != "" && branch != "")
    //        {
    //            bool check = false;
    //            check = DateTime.TryParseExact(fdate, "dd/MM/yyyy", null, DateTimeStyles.None, out dt);
    //            check = DateTime.TryParseExact(tdate, "dd/MM/yyyy", null, DateTimeStyles.None, out dt1);
    //            if (dt > dt1)
    //            {
    //                lblnorec.Visible = true;
    //                lblnorec.Text = "From Date Should Be Lesserthan Or Equals to To Date !!!";
    //                FpStudentAttendance.Visible = false;
    //                exceldiv.Visible = false;
    //                return;
    //            }
    //            if (check == true)
    //            {
    //                int dd = dt.Day;
    //                int dd1 = dt1.Day;

    //                TimeSpan diff = dt1 - dt;
    //                int days = diff.Days;

    //                int mm = dt.Month;
    //                int mm1 = dt1.Month;
    //                int yy = dt.Year;
    //                int yy1 = dt1.Year;
    //                int monthyear = (yy * 12) + mm;

    //                FarPoint.Web.Spread.NamedStyle fontblue = new FarPoint.Web.Spread.NamedStyle("blue");
    //                FpStudentAttendance.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
    //                FpStudentAttendance.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
    //                FpStudentAttendance.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
    //                FpStudentAttendance.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
    //                FpStudentAttendance.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
    //                FpStudentAttendance.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;

    //                FpStudentAttendance.ActiveSheetView.DefaultStyle.Font.Name = "Book Antiqua";
    //                FpStudentAttendance.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
    //                FpStudentAttendance.Sheets[0].SheetCorner.Columns[0].Visible = false;
    //                FpStudentAttendance.Sheets[0].SheetCorner.RowCount = 10;

    //                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
    //                style.Font.Size = 12;
    //                style.Font.Bold = true;
    //                style.HorizontalAlign = HorizontalAlign.Center;
    //                style.ForeColor = Color.Black;
    //                FpStudentAttendance.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
    //                FpStudentAttendance.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
    //                FpStudentAttendance.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
    //                FpStudentAttendance.Sheets[0].AllowTableCorner = true;

    //                FpStudentAttendance.Sheets[0].AutoPostBack = true;
    //                FpStudentAttendance.Sheets[0].ColumnHeader.RowCount = 0;
    //                FpStudentAttendance.Sheets[0].RowCount = 0;
    //                FpStudentAttendance.Sheets[0].ColumnCount = 0;
    //                FpStudentAttendance.Sheets[0].ColumnHeader.RowCount = 1;
    //                FpStudentAttendance.Sheets[0].ColumnCount = 18;
    //                FpStudentAttendance.Sheets[0].RowHeader.Visible = false;

    //                FpStudentAttendance.CommandBar.Visible = false;
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Period Details";
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Status";
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Cells[0, 6].Text = "S.No";
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Roll No";
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Reg No";
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Student Name";
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Period Details";
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Status";
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Cells[0, 12].Text = "S.No";
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Roll No";
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Reg No";
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Cells[0, 15].Text = "Student Name";
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Cells[0, 16].Text = "Period Details";
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Cells[0, 17].Text = "Status";

    //                FpStudentAttendance.Sheets[0].Columns[0].Locked = true;
    //                FpStudentAttendance.Sheets[0].Columns[1].Locked = true;
    //                FpStudentAttendance.Sheets[0].Columns[2].Locked = true;
    //                FpStudentAttendance.Sheets[0].Columns[3].Locked = true;
    //                FpStudentAttendance.Sheets[0].Columns[4].Locked = true;
    //                FpStudentAttendance.Sheets[0].Columns[5].Locked = true;
    //                FpStudentAttendance.Sheets[0].Columns[6].Locked = true;
    //                FpStudentAttendance.Sheets[0].Columns[7].Locked = true;
    //                FpStudentAttendance.Sheets[0].Columns[8].Locked = true;
    //                FpStudentAttendance.Sheets[0].Columns[9].Locked = true;
    //                FpStudentAttendance.Sheets[0].Columns[10].Locked = true;
    //                FpStudentAttendance.Sheets[0].Columns[11].Locked = true;
    //                FpStudentAttendance.Sheets[0].Columns[12].Locked = true;
    //                FpStudentAttendance.Sheets[0].Columns[13].Locked = true;
    //                FpStudentAttendance.Sheets[0].Columns[14].Locked = true;
    //                FpStudentAttendance.Sheets[0].Columns[15].Locked = true;
    //                FpStudentAttendance.Sheets[0].Columns[16].Locked = true;
    //                FpStudentAttendance.Sheets[0].Columns[17].Locked = true;

    //                FpStudentAttendance.Sheets[0].ColumnHeader.Columns[1].Visible = false;
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Columns[2].Visible = false;
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Columns[3].Visible = false;
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Columns[7].Visible = false;
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Columns[8].Visible = false;
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Columns[9].Visible = false;
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Columns[13].Visible = false;
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Columns[14].Visible = false;
    //                FpStudentAttendance.Sheets[0].ColumnHeader.Columns[15].Visible = false;
    //                FpStudentAttendance.Visible = false;

    //                rollno = Session["Rollflag"].ToString();
    //                regno = Session["Regflag"].ToString();
    //                sname = Session["Studflag"].ToString();
    //                if (rollno != "0")
    //                {
    //                    FpStudentAttendance.Sheets[0].ColumnHeader.Columns[1].Visible = true;
    //                    FpStudentAttendance.Sheets[0].ColumnHeader.Columns[7].Visible = true;
    //                    FpStudentAttendance.Sheets[0].ColumnHeader.Columns[13].Visible = true;
    //                    FpStudentAttendance.Sheets[0].Columns[1].Width = 100;
    //                }
    //                if (regno != "0")
    //                {
    //                    FpStudentAttendance.Sheets[0].ColumnHeader.Columns[2].Visible = true;
    //                    FpStudentAttendance.Sheets[0].ColumnHeader.Columns[8].Visible = true;
    //                    FpStudentAttendance.Sheets[0].ColumnHeader.Columns[15].Visible = true;
    //                    FpStudentAttendance.Sheets[0].Columns[2].Width = 100;
    //                }
    //                if (sname != "0")
    //                {
    //                    FpStudentAttendance.Sheets[0].ColumnHeader.Columns[3].Visible = true;
    //                    FpStudentAttendance.Sheets[0].ColumnHeader.Columns[9].Visible = true;
    //                    FpStudentAttendance.Sheets[0].ColumnHeader.Columns[16].Visible = true;
    //                    FpStudentAttendance.Sheets[0].Columns[3].Width = 100;
    //                }

    //                string getleavecode = "select LeaveCode  from AttMasterSetting where CollegeCode='" + collegecode + "' and CalcFlag=1";
    //                ds1 = da.select_method_wo_parameter(getleavecode, "Text");
    //                string leavecode = "";
    //                if (ds1.Tables[0].Rows.Count > 0)
    //                {
    //                    for (int sk = 0; sk < ds1.Tables[0].Rows.Count; sk++)
    //                    {
    //                        if (leavecode == "")
    //                        {
    //                            leavecode = ds1.Tables[0].Rows[sk]["LeaveCode"].ToString();
    //                        }
    //                        else
    //                        {
    //                            leavecode = leavecode + "," + ds1.Tables[0].Rows[sk]["LeaveCode"].ToString();
    //                        }
    //                    }
    //                }
    //                if (leavecode == "")
    //                {
    //                    leavecode = "2";
    //                }
    //                string strorder = "ORDER BY r.Roll_No";
    //                string serialno = da.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
    //                if (serialno.Trim() == "1")
    //                {
    //                    strorder = "ORDER BY r.serialno";
    //                }
    //                else
    //                {
    //                    string orderby_Setting = da.GetFunction("select value from master_Settings where settings='order_by'");
    //                    if (orderby_Setting == "0")
    //                    {
    //                        strorder = "ORDER BY r.Roll_No";
    //                    }
    //                    else if (orderby_Setting == "1")
    //                    {
    //                        strorder = "ORDER BY r.Reg_No";
    //                    }
    //                    else if (orderby_Setting == "2")
    //                    {
    //                        strorder = "ORDER BY r.Stud_Name";
    //                    }
    //                    else if (orderby_Setting == "0,1,2")
    //                    {
    //                        strorder = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
    //                    }
    //                    else if (orderby_Setting == "0,1")
    //                    {
    //                        strorder = "ORDER BY r.Roll_No,r.Reg_No";
    //                    }
    //                    else if (orderby_Setting == "1,2")
    //                    {
    //                        strorder = "ORDER BY r.Reg_No,r.Stud_Name";
    //                    }
    //                    else if (orderby_Setting == "0,2")
    //                    {
    //                        strorder = "ORDER BY r.Roll_No,r.Stud_Name";
    //                    }
    //                }
    //                ds.Clear();
    //                for (int k = 0; k < days + 1; k++)
    //                {
    //                    int d1 = 0;
    //                    dt = dt.AddDays(k);
    //                    d1 = dt.Day;
    //                    string getquery = " select r.sections,r.Current_Semester,r.Batch_Year,c.Course_Name,de.dept_acronym,d.Degree_Code,r.roll_no,Reg_No,r.serialno,r.Stud_Name,Roll_Admit,Adm_Date,a.d" + d1 + "d1,a.d" + d1 + "d2,a.d" + d1 + "d3,a.d" + d1 + "d4,a.d" + d1 + "d5,a.d" + d1 + "d6,a.d" + d1 + "d7,a.d" + d1 + "d8,a.d" + d1 + "d9  from Registration r,attendance a,Degree d,Department de,course c  where a.roll_no=r.Roll_No and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id  and d.Dept_Code=de.Dept_Code and a.month_year='" + monthyear + "' and Batch_Year in (" + batchyear + ") and r.degree_code in (" + branch + ") and cc=0  and exam_flag <> 'DEBAR'  and delflag=0 and (a.d" + d1 + "d1 in(" + leavecode + ") or a.d" + d1 + "d2 in(" + leavecode + ") or a.d" + d1 + "d3 in(" + leavecode + ") or a.d" + d1 + "d4 in(" + leavecode + ") or a.d" + d1 + "d5 in(" + leavecode + ") or a.d" + d1 + "d6 in(" + leavecode + ") or a.d" + d1 + "d7 in(" + leavecode + ") or a.d" + d1 + "d8 in(" + leavecode + ") or a.d" + d1 + "d9 in(" + leavecode + ") ) and r.Adm_Date<='" + dt.ToString("MM/dd/yyyy") + "' " + strorder + "";
    //                    getquery += "  select c.Course_Name,d.Degree_Code,c.Course_Id from Course c,Degree d where c.Course_Id=d.Course_Id and d.degree_code in (" + branch + ")";
    //                    ds = da.select_method_wo_parameter(getquery, "Text");
    //                    if (ds.Tables[0].Rows.Count > 0)
    //                    {
    //                        DataView dv = new DataView();
    //                        int sno = 1;
    //                        int abscount = 0;
    //                        int abscount1 = 0;
    //                        for (int sel = 0; sel < ds.Tables[1].Rows.Count; sel++)
    //                        {
    //                            ds.Tables[0].DefaultView.RowFilter = "degree_code='" + Convert.ToString(ds.Tables[1].Rows[sel]["degree_code"]) + "'";
    //                            dv = ds.Tables[0].DefaultView;
    //                            DataSet newds = new DataSet();
    //                            DataTable dtNew = new DataTable();
    //                            dtNew = dv.ToTable();
    //                            newds.Tables.Add(dtNew);
    //                            if (dv.Count > 0)
    //                            {
    //                                string checkcourse = "";
    //                                int finalstartrow = FpStudentAttendance.Sheets[0].RowCount;
    //                                for (int sk = 0; sk < dv.Count; )
    //                                {
    //                                    string coursedetails = "";
    //                                    if (checkcourse != newds.Tables[0].Rows[sk]["Course_Name"].ToString())
    //                                    {
    //                                        FpStudentAttendance.Sheets[0].RowCount++;
    //                                        checkcourse = newds.Tables[0].Rows[sk]["Course_Name"].ToString();
    //                                        string date = dt.ToString("dd/MM/yyyy");
    //                                        string batch = newds.Tables[0].Rows[sk]["Batch_Year"].ToString();
    //                                        string dept = newds.Tables[0].Rows[sk]["dept_acronym"].ToString();
    //                                        string section = newds.Tables[0].Rows[sk]["sections"].ToString();
    //                                        coursedetails = date + " " + batch + " " + checkcourse + "-" + dept + " " + "SEC-" + section;
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 0].Text = coursedetails;
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 0].BackColor = ColorTranslator.FromHtml("#ffccff");
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 0].Font.Bold = true;
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
    //                                        FpStudentAttendance.Sheets[0].SpanModel.Add(FpStudentAttendance.Sheets[0].RowCount - 1, 0, 1, FpStudentAttendance.Sheets[0].ColumnCount);
    //                                    }
    //                                    FpStudentAttendance.Sheets[0].RowCount++;
    //                                    string degcode = newds.Tables[0].Rows[sk]["Degree_Code"].ToString();
    //                                    string semester = newds.Tables[0].Rows[sk]["Current_Semester"].ToString();
    //                                    string noofperiods = da.GetFunction("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code='" + degcode + "' and semester='" + semester + "' ");
    //                                    if (dv.Count > sk)
    //                                    {
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 2].CellType = new FarPoint.Web.Spread.TextCellType();
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 1].Text = newds.Tables[0].Rows[sk]["roll_no"].ToString();
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 2].Text = newds.Tables[0].Rows[sk]["Reg_No"].ToString();
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 3].Text = newds.Tables[0].Rows[sk]["Stud_Name"].ToString();
    //                                        string perioddetails = "";
    //                                        string status = "";
    //                                        int nofperiods = Convert.ToInt32(noofperiods);

    //                                        hat.Clear();
    //                                        hat.Add("degree_code", degcode);
    //                                        hat.Add("sem_ester", int.Parse(semester));
    //                                        ds2 = da.select_method("period_attnd_schedule", hat, "sp");
    //                                        int NoHrs = 0;
    //                                        int fnhrs = 0;
    //                                        int anhrs = 0;
    //                                        int minpresI = 0;
    //                                        int minpresII = 0;
    //                                        if (ds2.Tables[0].Rows.Count != 0)
    //                                        {
    //                                            NoHrs = int.Parse(ds2.Tables[0].Rows[0]["PER DAY"].ToString());
    //                                            fnhrs = int.Parse(ds2.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
    //                                            anhrs = int.Parse(ds2.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
    //                                            minpresI = int.Parse(ds2.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
    //                                            minpresII = int.Parse(ds2.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
    //                                        }
    //                                        int test1 = fnhrs - minpresI;
    //                                        int test2 = anhrs - minpresII;
    //                                        int forct = 1;
    //                                        int ct = 0;
    //                                        for (int hr = 1; hr <= fnhrs; hr++)
    //                                        {
    //                                            string h1 = "";
    //                                            string h1A = "";
    //                                            h1 = newds.Tables[0].Rows[sk]["d" + d1 + "d" + hr].ToString();
    //                                            if (h1 == "2")
    //                                            {
    //                                                ct++;
    //                                            }
    //                                            present_mark(h1);
    //                                            h1A = hr + pp;

    //                                            if (perioddetails == "")
    //                                            {
    //                                                perioddetails = h1A;
    //                                            }
    //                                            else
    //                                            {
    //                                                perioddetails = perioddetails + "," + h1A;
    //                                            }
    //                                            forct++;
    //                                        }
    //                                        abscount = ct;
    //                                        int ct1 = 0;
    //                                        for (int hr = 1; hr <= anhrs; hr++)
    //                                        {
    //                                            string h1 = "";
    //                                            string h1A = "";
    //                                            h1 = newds.Tables[0].Rows[sk]["d" + d1 + "d" + forct].ToString();
    //                                            present_mark2(h1);
    //                                            h1A = forct + pp1;
    //                                            if (h1 == "2")
    //                                            {
    //                                                ct1++;
    //                                            }
    //                                            if (perioddetails == "")
    //                                            {
    //                                                perioddetails = h1A;
    //                                            }
    //                                            else
    //                                            {
    //                                                perioddetails = perioddetails + "," + h1A;
    //                                            }
    //                                            forct++;
    //                                        }
    //                                        abscount1 = ct1;
    //                                        int ttct = abscount + abscount1;
    //                                        if (test1 < abscount && test2 < abscount1)
    //                                        {
    //                                            status = "FL";
    //                                        }
    //                                        else if (test1 < abscount)
    //                                        {
    //                                            status = "HA";
    //                                        }
    //                                        else if (test2 < abscount1)
    //                                        {
    //                                            status = "HA";
    //                                        }
    //                                        else if (NoHrs == ttct)
    //                                        {
    //                                            status = "FL";
    //                                        }
    //                                        sk++;
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 4].Text = perioddetails;
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 5].Text = status;
    //                                    }
    //                                    if (dv.Count > sk)
    //                                    {
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 8].CellType = new FarPoint.Web.Spread.TextCellType();
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(sno++);
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 7].Text = newds.Tables[0].Rows[sk]["roll_no"].ToString();
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 8].Text = newds.Tables[0].Rows[sk]["Reg_No"].ToString();
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 9].Text = newds.Tables[0].Rows[sk]["Stud_Name"].ToString();
    //                                        string perioddetails = "";
    //                                        string status = "";
    //                                        int nofperiods = Convert.ToInt32(noofperiods);
    //                                        hat.Clear();
    //                                        hat.Add("degree_code", degcode);
    //                                        hat.Add("sem_ester", int.Parse(semester));
    //                                        ds2 = da.select_method("period_attnd_schedule", hat, "sp");
    //                                        int NoHrs = 0;
    //                                        int fnhrs = 0;
    //                                        int anhrs = 0;
    //                                        int minpresI = 0;
    //                                        int minpresII = 0;
    //                                        if (ds2.Tables[0].Rows.Count != 0)
    //                                        {
    //                                            NoHrs = int.Parse(ds2.Tables[0].Rows[0]["PER DAY"].ToString());
    //                                            fnhrs = int.Parse(ds2.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
    //                                            anhrs = int.Parse(ds2.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
    //                                            minpresI = int.Parse(ds2.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
    //                                            minpresII = int.Parse(ds2.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
    //                                        }
    //                                        int test1 = fnhrs - minpresI;
    //                                        int test2 = anhrs - minpresII;
    //                                        int forct = 1;
    //                                        int ct = 0;
    //                                        for (int hr = 1; hr <= fnhrs; hr++)
    //                                        {
    //                                            string h1 = "";
    //                                            string h1A = "";
    //                                            h1 = newds.Tables[0].Rows[sk]["d" + d1 + "d" + hr].ToString();
    //                                            if (h1 == "2")
    //                                            {
    //                                                ct++;
    //                                            }
    //                                            present_mark(h1);
    //                                            h1A = hr + pp;

    //                                            if (perioddetails == "")
    //                                            {
    //                                                perioddetails = h1A;
    //                                            }
    //                                            else
    //                                            {
    //                                                perioddetails = perioddetails + "," + h1A;
    //                                            }
    //                                            forct++;
    //                                        }
    //                                        abscount = ct;
    //                                        int ct1 = 0;
    //                                        for (int hr = 1; hr <= anhrs; hr++)
    //                                        {
    //                                            string h1 = "";
    //                                            string h1A = "";

    //                                            h1 = newds.Tables[0].Rows[sk]["d" + d1 + "d" + forct].ToString();
    //                                            present_mark2(h1);
    //                                            h1A = forct + pp1;
    //                                            if (h1 == "2")
    //                                            {
    //                                                ct1++;
    //                                            }

    //                                            if (perioddetails == "")
    //                                            {
    //                                                perioddetails = h1A;
    //                                            }
    //                                            else
    //                                            {
    //                                                perioddetails = perioddetails + "," + h1A;
    //                                            }
    //                                            forct++;
    //                                        }
    //                                        abscount1 = ct1;
    //                                        int ttct = abscount + abscount1;
    //                                        if (test1 < abscount && test2 < abscount1)
    //                                        {
    //                                            status = "FL";
    //                                        }
    //                                        else if (test1 < abscount)
    //                                        {
    //                                            status = "HA";
    //                                        }
    //                                        else if (test2 < abscount1)
    //                                        {
    //                                            status = "HA";
    //                                        }
    //                                        else if (NoHrs == ttct)
    //                                        {
    //                                            status = "FL";
    //                                        }
    //                                        sk++;
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 10].Text = perioddetails;
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 11].Text = status;
    //                                    }

    //                                    if (dv.Count > sk)
    //                                    {
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 14].CellType = new FarPoint.Web.Spread.TextCellType();
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(sno++);
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 13].Text = newds.Tables[0].Rows[sk]["roll_no"].ToString();
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 14].Text = newds.Tables[0].Rows[sk]["Reg_No"].ToString();
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 15].Text = newds.Tables[0].Rows[sk]["Stud_Name"].ToString();
    //                                        string perioddetails = "";
    //                                        string status = "";
    //                                        int nofperiods = Convert.ToInt32(noofperiods);

    //                                        hat.Clear();
    //                                        hat.Add("degree_code", degcode);
    //                                        hat.Add("sem_ester", int.Parse(semester));
    //                                        ds2 = da.select_method("period_attnd_schedule", hat, "sp");
    //                                        int NoHrs = 0;
    //                                        int fnhrs = 0;
    //                                        int anhrs = 0;
    //                                        int minpresI = 0;
    //                                        int minpresII = 0;
    //                                        if (ds2.Tables[0].Rows.Count != 0)
    //                                        {
    //                                            NoHrs = int.Parse(ds2.Tables[0].Rows[0]["PER DAY"].ToString());
    //                                            fnhrs = int.Parse(ds2.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
    //                                            anhrs = int.Parse(ds2.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
    //                                            minpresI = int.Parse(ds2.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
    //                                            minpresII = int.Parse(ds2.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
    //                                        }
    //                                        int test1 = fnhrs - minpresI;
    //                                        int test2 = anhrs - minpresII;
    //                                        int forct = 1;
    //                                        int ct = 0;
    //                                        for (int hr = 1; hr <= fnhrs; hr++)
    //                                        {
    //                                            string h1 = "";
    //                                            string h1A = "";
    //                                            h1 = newds.Tables[0].Rows[sk]["d" + d1 + "d" + hr].ToString();
    //                                            if (h1 == "2")
    //                                            {
    //                                                ct++;
    //                                            }
    //                                            present_mark(h1);
    //                                            h1A = hr + pp;

    //                                            if (perioddetails == "")
    //                                            {
    //                                                perioddetails = h1A;
    //                                            }
    //                                            else
    //                                            {
    //                                                perioddetails = perioddetails + "," + h1A;
    //                                            }
    //                                            forct++;
    //                                        }
    //                                        abscount = ct;
    //                                        int ct1 = 0;
    //                                        for (int hr = 1; hr <= anhrs; hr++)
    //                                        {
    //                                            string h1 = "";
    //                                            string h1A = "";

    //                                            h1 = newds.Tables[0].Rows[sk]["d" + d1 + "d" + forct].ToString();
    //                                            present_mark2(h1);
    //                                            h1A = forct + pp1;
    //                                            if (h1 == "2")
    //                                            {
    //                                                ct1++;
    //                                            }

    //                                            if (perioddetails == "")
    //                                            {
    //                                                perioddetails = h1A;
    //                                            }
    //                                            else
    //                                            {
    //                                                perioddetails = perioddetails + "," + h1A;
    //                                            }
    //                                            forct++;
    //                                        }
    //                                        abscount1 = ct1;
    //                                        int ttct = abscount + abscount1;
    //                                        if (test1 < abscount && test2 < abscount1)
    //                                        {
    //                                            status = "FL";
    //                                        }
    //                                        else if (test1 < abscount)
    //                                        {
    //                                            status = "HA";
    //                                        }
    //                                        else if (test2 < abscount1)
    //                                        {
    //                                            status = "HA";
    //                                        }
    //                                        else if (NoHrs == ttct)
    //                                        {
    //                                            status = "FL";
    //                                        }
    //                                        sk++;
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 16].Text = perioddetails;
    //                                        FpStudentAttendance.Sheets[0].Cells[FpStudentAttendance.Sheets[0].RowCount - 1, 17].Text = status;
    //                                    }
    //                                }
    //                            }
    //                        }
    //                        FpStudentAttendance.Sheets[0].PageSize = FpStudentAttendance.Sheets[0].RowCount;
    //                        FpStudentAttendance.Visible = true;
    //                        exceldiv.Visible = true;
    //                    }
    //                    else
    //                    {
    //                        lblnorec.Visible = true;
    //                        lblnorec.Text = "No Records Found";
    //                        FpStudentAttendance.Visible = false;
    //                        exceldiv.Visible = false;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                lblnorec.Visible = true;
    //                lblnorec.Text = "Date is not in correct format";
    //                FpStudentAttendance.Visible = false;
    //                exceldiv.Visible = false;
    //            }
    //        }
    //        else
    //        {
    //            lblnorec.Visible = true;
    //            lblnorec.Text = "Please Input Valid Data !!!";
    //            FpStudentAttendance.Visible = false;
    //            exceldiv.Visible = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblnorec.Text = ex.ToString();
    //    }
    //}
    protected void Bindingspread()
    {
        try
        {
            //added by rajasekar 22/08/2018
            DataRow dtrow = null;
            int col = 0;

            //==========================//

            if (txtbatch.Text != "---Select---" && txtdegree.Text != "---Select---" && txtbranch.Text != "---Select---")
            {
                Hashtable hat = new Hashtable();
                string batchyear = "";
                if (txtbatch.Text != "--Select--")
                {
                    for (int j = 0; j < chklsbatch.Items.Count; j++)
                    {
                        if (chklsbatch.Items[j].Selected == true)
                        {
                            if (batchyear == "")
                                batchyear = "'" + chklsbatch.Items[j].Value.ToString() + "'";
                            else
                                batchyear = batchyear + "," + "'" + chklsbatch.Items[j].Value.ToString() + "'";
                        }
                    }
                }
                string degree = "";
                if (txtdegree.Text != "--Select--")
                {
                    for (int s = 0; s < chklstdegree.Items.Count; s++)
                    {
                        if (chklstdegree.Items[s].Selected == true)
                        {
                            if (degree == "")
                                degree = "'" + chklstdegree.Items[s].Value.ToString() + "'";
                            else
                                degree = degree + "," + "'" + chklstdegree.Items[s].Value.ToString() + "'";
                        }
                    }
                }

                string branch = "";
                if (txtbranch.Text != "--Select--")
                {

                    for (int k = 0; k < chklstbranch.Items.Count; k++)
                    {
                        if (chklstbranch.Items[k].Selected == true)
                        {
                            if (branch == "")
                                branch = "'" + chklstbranch.Items[k].Value.ToString() + "'";
                            else
                                branch = branch + "," + "'" + chklstbranch.Items[k].Value.ToString() + "'";
                        }
                    }
                }

                string sectionval = "";
                if (txtsec.Text != "--Select--")
                {

                    for (int k = 0; k < chklssec.Items.Count; k++)
                    {
                        if (chklssec.Items[k].Selected == true)
                        {
                            if (sectionval == "")
                                sectionval =  "'"+ chklssec.Items[k].Value.ToString() +"'" ;
                            else
                                sectionval = sectionval + "," + "'" + chklssec.Items[k].Value.ToString() +" '";
                        }
                    }
                }
                //if (sectionval.Trim() != "")
                //{
                //    sectionval = " and r.Sections in( " + sectionval +" ) ";
                //}

                string date = txtfrom.Text;
                string[] split = date.Split('/');
                DateTime dt = new DateTime();

                bool check = false;
                check = DateTime.TryParseExact(date, "dd/MM/yyyy", null, DateTimeStyles.None, out dt);
                if (check == true)
                {
                    int d1 = dt.Day;
                    int mm = dt.Month;
                    int yy = dt.Year;
                    int monthyear = (yy * 12) + mm;

                   

                    

                    

                    

                    //added by rajasekar 01/09/2018
                    colfitroll = "roll";
                    colfitreg = "reg";
                    colfitadm = "sname";
                    //===========================//


                    rollno = Session["Rollflag"].ToString();
                    regno = Session["Regflag"].ToString();
                    sname = Session["Studflag"].ToString();
                    if (rollno != "0")
                    {
                        
                        colfitroll = "";
                    }
                    if (regno != "0")
                    {
                       
                        colfitreg = "";
                    }
                    if (sname != "0")
                    {
                        
                        colfitadm = "";
                    }

                    dtrow = dtt.NewRow();
                    dtt.Rows.Add(dtrow);
                    int colu = 0;

                    //added by rajasekar 01/09/2018
                    dtt.Columns.Add("S.No", typeof(string));
                    dtt.Rows[0][colu] = "S.No";
                    colu++;
                    if (colfitroll == "")
                    {
                        dtt.Columns.Add("Roll No", typeof(string));
                        dtt.Rows[0][colu] = "Roll No";
                        colu++;
                    }
                    if (colfitreg == "")
                    {
                        dtt.Columns.Add("Reg No", typeof(string));
                        dtt.Rows[0][colu] = "Reg No";
                        colu++;
                    }
                    if (colfitadm == "")
                    {
                        dtt.Columns.Add("Student Name", typeof(string));
                        dtt.Rows[0][colu] = "Student Name";
                        colu++;
                    }
                    dtt.Columns.Add("Period Details", typeof(string));
                    dtt.Rows[0][colu] = "Period Details";
                    colu++;
                    dtt.Columns.Add("Status", typeof(string));
                    dtt.Rows[0][colu] = "Status";
                    colu++;
                    dtt.Columns.Add("S.No.", typeof(string));
                    dtt.Rows[0][colu] = "S.No";
                    colu++;
                    if (colfitroll == "")
                    {
                        dtt.Columns.Add("Roll No.", typeof(string));
                        dtt.Rows[0][colu] = "Roll No";
                        colu++;
                    }
                    if (colfitreg == "")
                    {
                        dtt.Columns.Add("Reg No.", typeof(string));
                        dtt.Rows[0][colu] = "Reg No";
                        colu++;
                    }
                    if (colfitadm == "")
                    {
                        dtt.Columns.Add("Student Name.", typeof(string));
                        dtt.Rows[0][colu] = "Student Name";
                        colu++;
                    }
                    dtt.Columns.Add("Period Details.", typeof(string));
                    dtt.Rows[0][colu] = "Period Details";
                    colu++;
                    dtt.Columns.Add("Status.", typeof(string));
                    dtt.Rows[0][colu] = "Status";
                    colu++;
                    dtt.Columns.Add("S.No:", typeof(string));
                    dtt.Rows[0][colu] = "S.No";
                    colu++;
                    if (colfitroll == "")
                    {
                        dtt.Columns.Add("Roll No:", typeof(string));
                        dtt.Rows[0][colu] = "Roll No";
                        colu++;
                    }
                    if (colfitreg == "")
                    {
                        dtt.Columns.Add("Reg No:", typeof(string));
                        dtt.Rows[0][colu] = "Reg No";
                        colu++;
                    }
                    if (colfitadm == "")
                    {
                        dtt.Columns.Add("Student Name:", typeof(string));
                        dtt.Rows[0][colu] = "Student Name";
                        colu++;
                    }
                    dtt.Columns.Add("Period Details:", typeof(string));
                    dtt.Rows[0][colu] = "Period Details";
                    colu++;
                    dtt.Columns.Add("Status:", typeof(string));
                    dtt.Rows[0][colu] = "Status";
                    colu++;


                    //===================================//

                    Dictionary<string, string> dicattvalue = new Dictionary<string, string>();
                    string getleavecode = "select LeaveCode,CalcFlag  from AttMasterSetting where CollegeCode='" + collegecode + "'";
                    ds1 = da.select_method_wo_parameter(getleavecode, "Text");
                    string leavecode = "";
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        for (int sk = 0; sk < ds1.Tables[0].Rows.Count; sk++)
                        {
                            string calcflag = ds1.Tables[0].Rows[sk]["CalcFlag"].ToString();
                            string attcode = ds1.Tables[0].Rows[sk]["LeaveCode"].ToString();
                            if (!dicattvalue.ContainsKey(attcode))
                            {
                                dicattvalue.Add(attcode, calcflag);
                            }
                            if (calcflag.Trim() == "1")
                            {
                                if (leavecode == "")
                                {
                                    leavecode = attcode;
                                }
                                else
                                {
                                    leavecode = leavecode + "," + attcode;
                                }
                            }
                        }
                    }
                    if (leavecode == "")
                    {
                        leavecode = "2";
                    }
                    //string strorder = ", r.Roll_No";
                    //string serialno = da.GetFunction("select LinkValue from inssettings where college_code='" + Session["collegecode"].ToString() + "' and linkname='Student Attendance'");
                    //if (serialno.Trim() == "1")
                    //{
                    //    strorder = ", r.serialno";
                    //}
                    //else
                    //{
                    //    string orderby_Setting = da.GetFunction("select value from master_Settings where settings='order_by'");
                    //    if (orderby_Setting == "0")
                    //    {
                    //        strorder = ", r.Roll_No";
                    //    }
                    //    else if (orderby_Setting == "1")
                    //    {
                    //        strorder = ", r.Reg_No";
                    //    }
                    //    else if (orderby_Setting == "2")
                    //    {
                    //        strorder = ", r.Stud_Name";
                    //    }
                    //    else if (orderby_Setting == "0,1,2")
                    //    {
                    //        strorder = ", r.Roll_No,r.Reg_No,r.Stud_Name";
                    //    }
                    //    else if (orderby_Setting == "0,1")
                    //    {
                    //        strorder = "ORDER BY r.Roll_No,r.Reg_No";
                    //    }
                    //    else if (orderby_Setting == "1,2")
                    //    {
                    //        strorder = ", r.Reg_No,r.Stud_Name";
                    //    }
                    //    else if (orderby_Setting == "0,2")
                    //    {
                    //        strorder = ", r.Roll_No,r.Stud_Name";
                    //    }
                    //}

                    string serialno = da.GetFunction("select value from master_Settings where settings='order_by'");
                    serialno = serialno.Trim();
                    string strorder = "ORDER BY r.roll_no";
                    switch (serialno)
                    {
                        case "0":
                            strorder = "ORDER BY r.roll_no";
                            break;
                        case "1":
                            strorder = "ORDER BY r.Reg_No";
                            break;
                        case "2":
                            strorder = "ORDER BY r.Stud_Name";
                            break;
                        case "0,1,2":
                            strorder = "ORDER BY r.roll_no,r.Reg_No,r.stud_name";
                            break;
                        case "0,1":
                            strorder = "ORDER BY r.roll_no,r.Reg_No";
                            break;
                        case "1,2":
                            strorder = "ORDER BY r.Reg_No,r.Stud_Name";
                            break;
                        case "0,2":
                            strorder = "ORDER BY r.roll_no,r.Stud_Name";
                            break;
                        default:
                            strorder = "ORDER BY r.roll_no";
                            break;
                    }
                   




                    ds.Clear();
                    string getquery = " select r.sections,r.Current_Semester,r.Batch_Year,c.Course_Name,de.dept_acronym,d.Degree_Code,r.roll_no,Reg_No,r.serialno,r.Stud_Name,Roll_Admit,Adm_Date,a.d" + d1 + "d1,a.d" + d1 + "d2,a.d" + d1 + "d3,a.d" + d1 + "d4,a.d" + d1 + "d5,a.d" + d1 + "d6,a.d" + d1 + "d7,a.d" + d1 + "d8,a.d" + d1 + "d9  from Registration r,attendance a,Degree d,Department de,course c  where a.roll_no=r.Roll_No and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id  and d.Dept_Code=de.Dept_Code and a.month_year='" + monthyear + "' and Batch_Year in (" + batchyear + " ) and r.sections in(  " + sectionval + " ) and r.degree_code in (" + branch + ") and cc=0  and exam_flag <> 'DEBAR'  and delflag=0 and (a.d" + d1 + "d1 in(" + leavecode + ") or a.d" + d1 + "d2 in(" + leavecode + ") or a.d" + d1 + "d3 in(" + leavecode + ") or a.d" + d1 + "d4 in(" + leavecode + ") or a.d" + d1 + "d5 in(" + leavecode + ") or a.d" + d1 + "d6 in(" + leavecode + ") or a.d" + d1 + "d7 in(" + leavecode + ") or a.d" + d1 + "d8 in(" + leavecode + ") or a.d" + d1 + "d9 in(" + leavecode + ") ) and r.Adm_Date<='" + dt.ToString("MM/dd/yyyy") + "' " + strorder + "";
                    //order by r.Batch_Year desc,c.Course_Name,de.dept_acronym,d.Degree_Code,r.Current_Semester,r.Sections
                    ds = da.select_method_wo_parameter(getquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataView dv = new DataView();
                        int sno = 1;
                        string tempdegree = "";
                        string checkcourse = "";
                        int NoHrs = 0;
                        int fnhrs = 0;
                        int anhrs = 0;
                        int minpresI = 0;
                        int minpresII = 0;
                        int newrow = 0;
                        int columnval = 0;
                        col = 0;
                        for (int sk = 0; sk < ds.Tables[0].Rows.Count; sk++)
                        {
                            string batch = ds.Tables[0].Rows[sk]["Batch_Year"].ToString();
                            string dept = ds.Tables[0].Rows[sk]["dept_acronym"].ToString();
                            checkcourse = ds.Tables[0].Rows[sk]["Course_Name"].ToString();
                            string section = ds.Tables[0].Rows[sk]["sections"].ToString();
                            string coursedetails = batch + " " + checkcourse + "-" + dept;
                            string degcode = ds.Tables[0].Rows[sk]["degree_code"].ToString();
                            string semester = ds.Tables[0].Rows[sk]["Current_Semester"].ToString();
                            if (hatsetrights.Contains(batch + ',' + section.Trim()))
                            {
                                if (section != "")
                                {
                                    coursedetails = batch + " " + checkcourse + "-" + dept + " " + "SEC-" + section;
                                }
                                if (coursedetails != tempdegree)
                                {
                                    hat.Clear();
                                    hat.Add("degree_code", degcode);
                                    hat.Add("sem_ester", int.Parse(semester));
                                    ds2 = da.select_method("period_attnd_schedule", hat, "sp");
                                    if (ds2.Tables[0].Rows.Count != 0)
                                    {
                                        NoHrs = int.Parse(ds2.Tables[0].Rows[0]["PER DAY"].ToString());
                                        fnhrs = int.Parse(ds2.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                                        anhrs = int.Parse(ds2.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                                        minpresI = int.Parse(ds2.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                                        minpresII = int.Parse(ds2.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                                    }
                                }
                                string perioddetails = "";
                                string status = "";
                                double Present = 0;
                                int nofopreshours = 0, noofabsenthours = 0, noofnj = 0;
                                string prseorabsen = "";
                                for (int hr = 1; hr <= fnhrs; hr++)
                                {
                                    string h1 = ds.Tables[0].Rows[sk]["d" + d1 + "d" + hr].ToString();
                                    present_mark2(h1);
                                    if (perioddetails == "")
                                    {
                                        perioddetails = pp1;
                                    }
                                    else
                                    {
                                        perioddetails = perioddetails + "," + pp1;
                                    }
                                    if (dicattvalue.ContainsKey(h1))
                                    {
                                        prseorabsen = dicattvalue[h1].ToString();
                                        if (prseorabsen == "0")
                                        {
                                            nofopreshours++;
                                        }
                                        else if (prseorabsen == "1")
                                        {
                                            noofabsenthours++;
                                        }
                                        else if (prseorabsen == "2")
                                        {
                                            noofnj++;
                                        }
                                    }
                                }
                                if (noofnj + nofopreshours >= minpresI)
                                {
                                    Present = 0.5;
                                }
                                nofopreshours = 0;
                                noofabsenthours = 0;
                                noofnj = 0;
                                for (int hr = fnhrs + 1; hr <= NoHrs; hr++)
                                {
                                    string h1 = ds.Tables[0].Rows[sk]["d" + d1 + "d" + hr].ToString();
                                    present_mark2(h1);
                                    if (perioddetails == "")
                                    {
                                        perioddetails = pp1;
                                    }
                                    else
                                    {
                                        perioddetails = perioddetails + "," + pp1;
                                    }
                                    if (dicattvalue.ContainsKey(h1))
                                    {
                                        prseorabsen = dicattvalue[h1].ToString();
                                        if (prseorabsen == "0")
                                        {
                                            nofopreshours++;
                                        }
                                        else if (prseorabsen == "1")
                                        {
                                            noofabsenthours++;
                                        }
                                        else if (prseorabsen == "2")
                                        {
                                            noofnj++;
                                        }
                                    }
                                }
                                if (noofnj + nofopreshours >= minpresII)
                                {
                                    Present = Present + 0.5;
                                }
                                status = "";
                                if (Present == 0)
                                {
                                    status = "FA";
                                }
                                else if (Present == 0.5)
                                {
                                    status = "HA";
                                }
                                if (status != "")
                                {
                                    newrow++;
                                    if ((newrow % 4) == 0)
                                    {
                                        newrow = 1;
                                    }
                                    if (coursedetails != tempdegree)
                                    {
                                        newrow = 1;
                                        tempdegree = coursedetails;
                                       

                                        //Added by rajasekar 22/08/2018


                                        if (col != 0)
                                        {
                                            col = 0;
                                            dtt.Rows.Add(dtrow);
                                        }
                                        dtrow = dtt.NewRow();
                                        dtrow[col] = coursedetails;
                                        //dtt.Rows.Add(dtrow);
                                        //====================//
                                    }
                                    columnval = newrow * 6 - 6;
                                    if (newrow == 1)
                                    {
                                        
                                        columnval = 0;

                                        //Added by rajasekar 22/08/2018
                                        col = 0;
                                        dtt.Rows.Add(dtrow);
                                        dtrow = dtt.NewRow();
                                        //Added by rajasekar 22/08/2018
                                    }
                                    


                                    //Added by rajasekar 22/08/2018


                                    dtrow[col] = Convert.ToString(sno++);
                                    col++;
                                    if (colfitroll == "")
                                    {
                                        dtrow[col] = ds.Tables[0].Rows[sk]["roll_no"].ToString();
                                        col++;
                                    }
                                    if (colfitreg == "")
                                    {
                                        dtrow[col] = ds.Tables[0].Rows[sk]["Reg_No"].ToString();
                                        col++;
                                    }

                                    if (colfitadm == "")
                                    {
                                        dtrow[col] = ds.Tables[0].Rows[sk]["Stud_Name"].ToString();
                                        col++;
                                    }

                                    dtrow[col] = perioddetails;
                                    col++;

                                    dtrow[col] = status;
                                    col++;



                                    //====================//


                                }

                            }
                        }
                        //added by rajasekar
                        dtt.Rows.Add(dtrow);
                        grdover.DataSource = dtt;
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

                                    if (grdover.HeaderRow.Cells[j].Text == "S.No" || grdover.HeaderRow.Cells[j].Text == "S.No." || grdover.HeaderRow.Cells[j].Text == "S.No:" || grdover.HeaderRow.Cells[j].Text == "Status" || grdover.HeaderRow.Cells[j].Text == "Status." || grdover.HeaderRow.Cells[j].Text == "Status:" || grdover.Rows[i].Cells[j].Text == "&nbsp;")
                                    {
                                        grdover.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                        if (grdover.Rows[i].Cells[j].Text == "&nbsp;" && j == 1)
                                        {
                                            grdover.Rows[i].Cells[j - 1].HorizontalAlign = HorizontalAlign.Center;
                                            grdover.Rows[i].Cells[j - 1].BackColor = ColorTranslator.FromHtml("#2E8B57");
                                            grdover.Rows[i].Cells[j - 1].Font.Bold = true;
                                            grdover.Rows[i].Cells[j - 1].ColumnSpan = grdover.Rows[i].Cells.Count;
                                            for (int a = 1; a < grdover.Rows[i].Cells.Count; a++)
                                                grdover.Rows[i].Cells[a].Visible = false;


                                        }
                                    }

                                    else
                                        grdover.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;

                                }
                            }

                        }


                        //========================//
                       
                        grdover.Visible = true;
                        exceldiv.Visible = true;

                        if (grdover.Rows.Count == 0)
                        {
                            lblnorec.Visible = true;
                            lblnorec.Text = "No Records Found";
                            
                            grdover.Visible = false;
                            exceldiv.Visible = false;
                        }
                    }
                    else
                    {
                        lblnorec.Visible = true;
                        lblnorec.Text = "No Records Found";
                        
                        grdover.Visible = false;
                        exceldiv.Visible = false;
                    }
                }
                else
                {
                    lblnorec.Visible = true;
                    lblnorec.Text = "Date is not in correct format";
                    exceldiv.Visible = false;
                }
            }
            else
            {
                
                grdover.Visible = false;
                exceldiv.Visible = false;
                lblnorec.Visible = true;
                lblnorec.Text = "Please Select The Required Fields And Then Proceed !!!";
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
        }
    }

    public void first_btngo()
    {
        if (txtfrom.Text != string.Empty && txtto.Text != string.Empty)
        {
            gobutton();
        }
    }

    public void gobutton()
    {
        try
        {
            //added by rajasekar 22/08/2018
            
            int col = 0;
            
            //==========================//

            hat.Clear();
            ds_attndmaster.Clear();
            count_master = 0;
            absent_calcflag = "";
            absent_hash.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            ds_attndmaster = da.select_method("ATT_MASTER_SETTING", hat, "sp");
            count_master = (ds_attndmaster.Tables[0].Rows.Count);
            string batchyear = "";
            if (txtbatch.Text != "---Select---" && txtdegree.Text != "---Select---" && txtbranch.Text != "---Select---")
            {
                if (txtbatch.Text != "--Select--")
                {
                    for (int j = 0; j < chklsbatch.Items.Count; j++)
                    {
                        if (chklsbatch.Items[j].Selected == true)
                        {
                            if (batchyear == "")
                                batchyear = "'" + chklsbatch.Items[j].Value.ToString() + "'";
                            else
                                batchyear = batchyear + "," + "'" + chklsbatch.Items[j].Value.ToString() + "'";
                        }
                    }
                }

                string branch = "";
                if (txtbranch.Text != "--Select--")
                {

                    for (int k = 0; k < chklstbranch.Items.Count; k++)
                    {
                        if (chklstbranch.Items[k].Selected == true)
                        {
                            if (branch == "")
                                branch = "'" + chklstbranch.Items[k].Value.ToString() + "'";
                            else
                                branch = branch + "," + "'" + chklstbranch.Items[k].Value.ToString() + "'";
                        }
                    }
                }
                string section = "";
                if (txtsec.Text != "--Select--")
                {

                    for (int k = 0; k < chklssec.Items.Count; k++)
                    {
                        if (chklssec.Items[k].Selected == true)
                        {
                            if (section == "")
                                section = "'" + chklssec.Items[k].Value.ToString() + "'";
                            else
                                section = section + "," + "'" + chklssec.Items[k].Value.ToString() + "'";
                        }
                    }
                }
                if (section.Trim() != "")
                {
                    section = " and r.Sections in(" + section + ") ";
                }
                if (count_master > 0)
                {
                    for (count_master = 0; count_master < ds_attndmaster.Tables[0].Rows.Count; count_master++)
                    {
                        if (ds_attndmaster.Tables[0].Rows[count_master]["calcflag"].ToString() == "1")
                        {
                            if (absent_calcflag == "")
                            {
                                absent_calcflag = ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString();
                                if (!absent_hash.Contains(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                                {
                                    absent_hash.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                                }
                            }
                            else
                            {
                                absent_calcflag = absent_calcflag + "," + ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString();
                                if (!absent_hash.Contains(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString()))
                                {
                                    absent_hash.Add(ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString(), ds_attndmaster.Tables[0].Rows[count_master]["leavecode"].ToString());
                                }
                            }
                        }
                    }
                }

                hat_days_end.Clear();
                hat_days_first.Clear();
                

                
                grdover.Visible = false;
                

                


                hat.Clear();
                hat.Add("college_code", Session["collegecode"].ToString());
                hat.Add("form_name", "AbsenteeRt.aspx");
                dsprint = da.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
                if (dsprint.Tables[0].Rows.Count > 0)
                {
                    if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
                    {
                        new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                        new_header_string_split = new_header_string.Split(',');
                       
                    }
                }

                dtrow = dtt.NewRow();
                dtt.Rows.Add(dtrow);
                int colu = 0;
               
                //added by rajasekar 01/09/2018
                dtt.Columns.Add("S.No", typeof(string));
                dtt.Rows[0][colu] = "S.No";
                colu++;
                if (colfitadm == "")
                {
                    dtt.Columns.Add("Admission No", typeof(string));
                    dtt.Rows[0][colu] = "Admission No";
                    colu++;

                }
                if (colfitroll == "")
                {
                    dtt.Columns.Add("Roll No", typeof(string));
                    dtt.Rows[0][colu] = "Roll No";
                    colu++;
                }
                if (colfitreg == "")
                {
                    dtt.Columns.Add("Register No", typeof(string));
                    dtt.Rows[0][colu] = "Register No";
                    colu++;
                }
                dtt.Columns.Add("Student Name", typeof(string));
                dtt.Rows[0][colu] = "Student Name";
                colu++;
                dtt.Columns.Add("Total Absent Days", typeof(string));
                dtt.Rows[0][colu] = "Total Absent Days";
                colu++;
                dtt.Columns.Add("S.No.", typeof(string));
                dtt.Rows[0][colu] = "S.No";
                colu++;
                if (colfitadm == "")
                {
                    dtt.Columns.Add("Admission No.", typeof(string));
                    dtt.Rows[0][colu] = "Admission No";
                    colu++;
                }
                if (colfitroll == "")
                {
                    dtt.Columns.Add("Roll No.", typeof(string));
                    dtt.Rows[0][colu] = "Roll No";
                    colu++;
                }
                if (colfitreg == "")
                {
                    dtt.Columns.Add("Register No.", typeof(string));
                    dtt.Rows[0][colu] = "Register No";
                    colu++;
                }
                dtt.Columns.Add("Student Name.", typeof(string));
                dtt.Rows[0][colu] = "Student Name";
                colu++;
                dtt.Columns.Add("Total Absent Days.", typeof(string));
                dtt.Rows[0][colu] = "Total Absent Days";
                colu++;
                dtt.Columns.Add("S.No:", typeof(string));
                dtt.Rows[0][colu] = "S.No";
                colu++;
                if (colfitadm == "")
                {
                    dtt.Columns.Add("Admission No:", typeof(string));
                    dtt.Rows[0][colu] = "Admission No";
                    colu++;
                }
                if (colfitroll == "")
                {
                    dtt.Columns.Add("Roll No:", typeof(string));
                    dtt.Rows[0][colu] = "Roll No";
                    colu++;
                }
                if (colfitreg == "")
                {
                    dtt.Columns.Add("Register No:", typeof(string));
                    dtt.Rows[0][colu] = "Register No";
                    colu++;
                }
                dtt.Columns.Add("Student Name:", typeof(string));
                dtt.Rows[0][colu] = "Student Name";
                colu++;
                dtt.Columns.Add("Total Absent Days:", typeof(string));
                dtt.Rows[0][colu] = "Total Absent Days";
                colu++;


                //===================================//

                date1 = txtfrom.Text.ToString();
                string[] split = date1.Split(new Char[] { '/' });
                if (split.GetUpperBound(0) == 2)
                {
                    datefrom = split[1].ToString() + "/" + split[0].ToString() + "/" + split[2].ToString();
                    date2 = txtto.Text.ToString();
                    string[] split1 = date2.Split(new Char[] { '/' });
                    if (split1.GetUpperBound(0) == 2)
                    {
                        dateto = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
                        dt1 = Convert.ToDateTime(datefrom.ToString());
                        dt2 = Convert.ToDateTime(dateto.ToString());
                        TimeSpan t = dt2.Subtract(dt1);
                        long days = t.Days;
                        ht_sphr.Clear();

                        if (days >= 0)
                        {
                            string orderby_Setting = da.GetFunction(" select value from master_Settings where settings='order_by'");
                            orderby_Setting = orderby_Setting.Trim();
                            string strorder = "ORDER BY r.roll_no";
                            switch (orderby_Setting)
                            {
                                case "0":
                                    strorder = ",r.roll_no";
                                    break;
                                case "1":
                                    strorder = ",r.Reg_No";
                                    break;
                                case "2":
                                    strorder = ",r.Stud_Name";
                                    break;
                                case "0,1,2":
                                    strorder = " ,r.roll_no,r.Reg_No,r.stud_name";
                                    break;
                                case "0,1":
                                    strorder = " ,r.roll_no,r.Reg_No";
                                    break;
                                case "1,2":
                                    strorder = " ,r.Reg_No,r.Stud_Name";
                                    break;
                                case "0,2":
                                    strorder = ",r.roll_no,r.Stud_Name";
                                    break;
                                default:
                                    strorder = ",r.roll_no";
                                    break;
                            }
                           


                            //string orderby_Setting = da.GetFunction("select value from master_Settings where settings='order_by'");
                            //string strorder = ", r.roll_no";
                            //if (orderby_Setting == "0")
                            //{
                            //    strorder = ",r.roll_no";
                            //}
                            //else if (orderby_Setting == "1")
                            //{
                            //    strorder = ",r.reg_no";
                            //}
                            //else if (orderby_Setting == "2")
                            //{
                            //    strorder = ",r.stud_name";
                            //}
                            //else if (orderby_Setting == "0,1,2")
                            //{
                            //    strorder = ", r.roll_no,r.reg_no,r.stud_name";
                            //}
                            //else if (orderby_Setting == "0,1")
                            //{
                            //    strorder = ",r.roll_no,r.reg_no";
                            //}
                            //else if (orderby_Setting == "1,2")
                            //{
                            //    strorder = ",r.reg_no,r.stud_name";
                            //}
                            //else if (orderby_Setting == "0,2")
                            //{
                            //    strorder = ",r.roll_no,r.stud_name";
                            //}

                            int newrow = 0;
                            int columnval = 0;
                            string tempdegree = "";
                            col = 0;

                            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                            hat.Clear();
                            hat.Add("colege_code", Session["collegecode"].ToString());
                            ds1 = da.select_method("ATT_MASTER_SETTING", hat, "sp");
                            count = ds1.Tables[0].Rows.Count;
                            ds4.Clear();
                            string strquery = "select r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,r.serialno,c.college_code,r.batch_year,r.degree_code,r.current_semester,c.Course_Name,de.dept_acronym,r.sections,r.Adm_Date,r.Roll_Admit from Registration r,Degree d,Course c,Department de where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.degree_code in(" + branch + ") and r.batch_year in(" + batchyear + ") and r.CC = 0  AND r.DelFlag = 0 aND r.Exam_Flag <> 'debar' order by r.Batch_Year desc,de.dept_acronym,r.Degree_Code,r.Current_Semester,r.Sections  " + strorder + "";
                            ds4 = da.select_method_wo_parameter(strquery, "Text");
                            DataView dv = new DataView();
                            for (rows_count = 0; rows_count < ds4.Tables[0].Rows.Count; rows_count++)
                            {
                                batch = ds4.Tables[0].Rows[rows_count]["Batch_Year"].ToString();
                                degree = ds4.Tables[0].Rows[rows_count]["degree_code"].ToString();
                                sem = ds4.Tables[0].Rows[rows_count]["Current_Semester"].ToString();
                                sections = ds4.Tables[0].Rows[rows_count]["sections"].ToString();
                                if (hatsetrights.Contains(batch + ',' + sections.Trim()))
                                {
                                    string course = ds4.Tables[0].Rows[rows_count]["Course_Name"].ToString();
                                    string department = ds4.Tables[0].Rows[rows_count]["dept_acronym"].ToString();
                                    string roll = ds4.Tables[0].Rows[rows_count]["roll_no"].ToString();
                                    string reg = ds4.Tables[0].Rows[rows_count]["reg_no"].ToString();
                                    string name = ds4.Tables[0].Rows[rows_count]["stud_name"].ToString();
                                    string studtype = ds4.Tables[0].Rows[rows_count]["Stud_Type"].ToString();
                                    string coursedetails = batch + " -" + course + " -" + department + " -" + sem;
                                    if (sections.Trim() != "")
                                    {
                                        coursedetails = coursedetails + " - " + sections;
                                    }
                                    if (tempdegree != coursedetails)
                                    {
                                        deptflag = false;
                                        hat.Clear();
                                        hat.Add("degree_code", degree);
                                        hat.Add("sem_ester", int.Parse(sem.ToString()));
                                        ds = da.select_method("period_attnd_schedule", hat, "sp");
                                        if (ds.Tables[0].Rows.Count != 0)
                                        {
                                            NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                                            fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                                            minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                                            minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                                            minpresday = int.Parse(ds.Tables[0].Rows[0]["MIN PREE PER DAY"].ToString());
                                        }
                                    }
                                    frdate = txtfrom.Text;
                                    todate = txtto.Text;
                                    string dt = frdate;
                                    string[] dsplit = dt.Split(new Char[] { '/' });
                                    frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                                    int demfcal = int.Parse(dsplit[2].ToString());
                                    demfcal = demfcal * 12;
                                    cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                                    cal_from_date_tmp = demfcal + int.Parse(dsplit[1].ToString());

                                    string monthcal = cal_from_date.ToString();
                                    dt = todate;
                                    dsplit = dt.Split(new Char[] { '/' });
                                    todate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                                    int demtcal = int.Parse(dsplit[2].ToString());
                                    demtcal = demtcal * 12;
                                    cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
                                    cal_to_date_tmp = demtcal + int.Parse(dsplit[1].ToString());

                                    per_from_gendate = Convert.ToDateTime(frdate);
                                    per_to_gendate = Convert.ToDateTime(todate);

                                    per_abshrs_spl = 0;
                                    tot_per_hrs_spl = 0;
                                    tot_ondu_spl = 0;
                                    tot_ml_spl = 0;
                                    tot_conduct_hr_spl = 0;
                                    per_workingdays1 = 0;
                                    persentmonthcal();

                                    if (per_absent_date > 0)
                                    {
                                        newrow++;
                                        if ((newrow % 4) == 0)
                                        {
                                            newrow = 1;
                                        }
                                        if (coursedetails != tempdegree)
                                        {
                                            newrow = 1;
                                            tempdegree = coursedetails;
                                            

                                            //Added by rajasekar 22/08/2018
                                           
                                            
                                            if (col != 0)
                                            {
                                                col = 0;
                                                dtt.Rows.Add(dtrow);
                                            }
                                            dtrow = dtt.NewRow();
                                            dtrow[col] = coursedetails;
                                            //dtt.Rows.Add(dtrow);
                                            //====================//
                                        }
                                        columnval = newrow * 6 - 6;
                                        if (newrow == 1)
                                        {
                                            
                                            columnval = 0;
                                            //Added by rajasekar 22/08/2018
                                            col = 0;
                                            dtt.Rows.Add(dtrow);
                                            dtrow = dtt.NewRow();
                                            //Added by rajasekar 22/08/2018
                                        }
                                        sno++;

                                       


                                        //Added by rajasekar 22/08/2018

                                        
                                        dtrow[col] = Convert.ToString(sno);
                                        col++;
                                        if (colfitadm == "")
                                        {
                                            dtrow[col] = ds4.Tables[0].Rows[rows_count]["Roll_Admit"].ToString();
                                            col++;
                                        }
                                        if (colfitroll == "")
                                        {
                                            dtrow[col] = ds4.Tables[0].Rows[rows_count]["roll_no"].ToString();
                                            col++;
                                        }

                                        if (colfitreg == "")
                                        {
                                            dtrow[col] = ds4.Tables[0].Rows[rows_count]["Reg_No"].ToString();
                                            col++;
                                        }

                                        dtrow[col] = ds4.Tables[0].Rows[rows_count]["Stud_Name"].ToString();
                                        col++;

                                        dtrow[col] = per_absent_date.ToString();
                                        col++;


                                        
                                        //====================//


                                    }
                                }
                            }
                            //added by rajasekar
                            dtt.Rows.Add(dtrow);
                            grdover.DataSource = dtt;
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

                                        if (grdover.HeaderRow.Cells[j].Text == "S.No" || grdover.HeaderRow.Cells[j].Text == "S.No." || grdover.HeaderRow.Cells[j].Text == "S.No:" || grdover.HeaderRow.Cells[j].Text == "Total Absent Days" || grdover.HeaderRow.Cells[j].Text == "Total Absent Days." || grdover.HeaderRow.Cells[j].Text == "Total Absent Days:" || grdover.Rows[i].Cells[j].Text == "&nbsp;")
                                        {
                                            grdover.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                            if (grdover.Rows[i].Cells[j].Text == "&nbsp;" && j == 1)
                                            {
                                                grdover.Rows[i].Cells[j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                grdover.Rows[i].Cells[j - 1].BackColor = ColorTranslator.FromHtml("#2E8B57");
                                                grdover.Rows[i].Cells[j - 1].Font.Bold = true;
                                                grdover.Rows[i].Cells[j - 1].ColumnSpan = grdover.Rows[i].Cells.Count;
                                                for (int a = 1; a < grdover.Rows[i].Cells.Count; a++)
                                                    grdover.Rows[i].Cells[a].Visible = false;


                                            }
                                        }

                                        else
                                            grdover.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;

                                    }
                                }

                            }


                            //========================//
                            if (grdover.Rows.Count == 0)
                            {
                                lblnorec.Visible = true;
                                lblnorec.Text = "From Date Should Be Lessthan The To Date";
                                
                                grdover.Visible = false;
                                exceldiv.Visible = false;
                                return;
                            }
                        }
                        else
                        {
                            lblnorec.Visible = true;
                            lblnorec.Text = "From Date Should Be Lessthan The To Date";
                           
                            grdover.Visible = false;
                            exceldiv.Visible = false;
                            return;
                        }
                    }
                    else
                    {
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Choose Valid Date";
                        
                        grdover.Visible = false;
                        exceldiv.Visible = false;
                        return;
                    }
                }
                else
                {
                    lblnorec.Visible = true;
                    lblnorec.Text = "Please Choose Valid Date";
                    
                    grdover.Visible = false;
                    exceldiv.Visible = false;
                    return;
                }
                if (count < 1)
                {
                    lblnorec.Visible = true;
                    lblnorec.Text = "No Records Found";
                    
                    grdover.Visible = false;
                    exceldiv.Visible = false;
                    return;
                }
            }
            else
            {
                
                grdover.Visible = false;
                exceldiv.Visible = false;
                lblnorec.Visible = true;
                lblnorec.Text = "Please Select The Required Fields And Then Proceed !!!";
            }
            
            grdover.Visible = true;
            exceldiv.Visible = true;
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
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
    public void from_today()
    {
        hat.Clear();
        hat.Add("f_date", int.Parse(fcal.ToString()));
        hat.Add("t_date", int.Parse(tcal.ToString()));
        hat.Add("roll_no", newds.Tables[0].Rows[sk]["ROLL NO"].ToString());
        ds1 = da.select_method("ATT_REPORTS_DETAILS", hat, "sp");
        dat = fd;
    }

    //public void findday()
    //{
    //    int tno = 0;
    //    double tk = 0;
    //    double t1k = 0;
    //    //hat_days_first.Clear();
    //    //hat_days_end.Clear();
    //    perabsenthrs = 0;
    //    from_today();
    //    int i = 0;
    //    double k = 1;
    //    dat = fd;
    //    date_today = dt1;

    //    for (int cumd = fcal; cumd <= tcal; cumd++)
    //    {
    //    nextmonth:

    //        totpresentday = 0;
    //        if (count == 0)
    //        {
    //            if (cumd == tcal)
    //            {
    //                cal_date(cumd);

    //                if (fd == td)
    //                {
    //                    totpresentday += 1;
    //                }
    //                else if (td == daycount)
    //                {
    //                    totpresentday += daycount;
    //                }

    //                else
    //                {
    //                    totpresentday += td - (fd - 1);
    //                }

    //            }

    //            if (cumd != tcal)
    //            {

    //                cal_date(cumd);

    //                totpresentday += daycount;
    //            }

    //            //------------find start date
    //            if (cumd == fcal)
    //            {
    //                k = fd;
    //            }
    //            else
    //            {
    //                k = 1;
    //            }

    //            if (cumd == tcal)
    //            {
    //                endk = td;
    //            }
    //            else
    //            {
    //                endk = int.Parse(totpresentday.ToString());
    //            }
    //            //if (cumd)

    //            //if (tno != cumd)
    //            //{
    //            //    tno = cumd;
    //            //    tk = k;
    //            if (!hat_days_first.ContainsKey(cumd))
    //            {
    //                hat_days_first.Add(cumd, k);
    //            }
    //            //}
    //            //if (t1k != endk)
    //            //{
    //            //    tno = cumd;
    //            //    t1k = endk;
    //            if (!hat_days_end.ContainsKey(cumd))
    //            {
    //                hat_days_end.Add(cumd, endk);
    //            }

    //            //}

    //        }

    //        else
    //        {
    //            k = int.Parse(GetCorrespondingKey(cumd, hat_days_first).ToString());
    //            endk = int.Parse(GetCorrespondingKey(cumd, hat_days_end).ToString());
    //        }

    //        for (k = k; k <= endk; k++)
    //        {
    //        nextday:
    //            absenthrs = 0;
    //            if (spl_hr_flag == true)
    //            {
    //                if (ht_sphr.Contains(Convert.ToString(date_today)))
    //                {
    //                    getspecial_hr();
    //                }
    //            }

    //            if (count == 0)
    //            {
    //                findholy();
    //                if (ds_holi.Tables[0].Rows.Count > 0)
    //                {
    //                    if (ds_holi.Tables[0].Rows[0]["halforfull"].ToString() == "False")
    //                    {
    //                        halforfull = "0";
    //                    }
    //                    else
    //                    {
    //                        halforfull = "1";
    //                    }
    //                    if (ds_holi.Tables[0].Rows[0]["morning"].ToString() == "False")
    //                    {
    //                        mng = "0";
    //                    }
    //                    else
    //                    {
    //                        mng = "1";
    //                    }
    //                    if (ds_holi.Tables[0].Rows[0]["evening"].ToString() == "False")
    //                    {
    //                        evng = "0";
    //                    }
    //                    else
    //                    {
    //                        evng = "1";
    //                    }

    //                    holiday_sched_details = halforfull + "*" + mng + "*" + evng;
    //                    if (!hat_holy.ContainsKey(date_today.ToString("dd") + "/" + date_today.ToString("MM") + "/" + date_today.ToString("yyy")))
    //                    {
    //                        hat_holy.Add(date_today.ToString("dd") + "/" + date_today.ToString("MM") + "/" + date_today.ToString("yyy"), holiday_sched_details);
    //                    }
    //                    //hat_holy.Add(date_today, date_today);

    //                }

    //                else
    //                {
    //                    holiday_sched_details = "3*0*0";
    //                    if (!hat_holy.ContainsKey(date_today.ToString("dd") + "/" + date_today.ToString("MM") + "/" + date_today.ToString("yyy")))
    //                    {
    //                        hat_holy.Add(date_today.ToString("dd") + "/" + date_today.ToString("MM") + "/" + date_today.ToString("yyy"), holiday_sched_details);
    //                    }
    //                }

    //            }
    //            if (hat_holy.ContainsKey(date_today.ToString("dd") + "/" + date_today.ToString("MM") + "/" + date_today.ToString("yyy")))
    //            {
    //                value_holi_status = GetCorrespondingKey(date_today.ToString("dd") + "/" + date_today.ToString("MM") + "/" + date_today.ToString("yyy"), hat_holy).ToString();
    //                split_holiday_status = value_holi_status.Split('*');

    //                if (split_holiday_status[0].ToString() == "3")//=========ful day working day
    //                {
    //                    split_holiday_status_1 = "1";
    //                    split_holiday_status_2 = "1";
    //                }
    //                else if (split_holiday_status[0].ToString() == "1")//=============half day working day
    //                {
    //                    if (split_holiday_status[1].ToString() == "1")//==============mng holiday//evng working day
    //                    {
    //                        split_holiday_status_1 = "0";
    //                        split_holiday_status_2 = "1";
    //                    }

    //                    if (split_holiday_status[2].ToString() == "1")//==============evng holiday//mng working day
    //                    {
    //                        split_holiday_status_1 = "1";
    //                        split_holiday_status_2 = "0";
    //                    }
    //                }
    //                else if (split_holiday_status[0].ToString() == "0")
    //                {
    //                    date_today = date_today.AddDays(1);
    //                    k++;
    //                    if (k <= endk)
    //                    {
    //                        goto nextday;
    //                    }
    //                    else
    //                    {
    //                        i++;
    //                        cumd++;
    //                        goto nextmonth;
    //                    }
    //                    // break;
    //                }


    //                //=============================================

    //                {
    //                    m1 = "d" + k + "d1";
    //                    m2 = "d" + k + "d2";
    //                    m3 = "d" + k + "d3";
    //                    m4 = "d" + k + "d4";
    //                    m5 = "d" + k + "d5";
    //                    m6 = "d" + k + "d6";
    //                    m7 = "d" + k + "d7";
    //                    m8 = "d" + k + "d8";
    //                    m9 = "d" + k + "d9";

    //                    i = 0;

    //                    int count1 = ds1.Tables[0].Rows.Count;
    //                    {
    //                        if (count1 > 0)
    //                        {
    //                            if (i < count1)
    //                            //  if(Convert.ToInt16( ds1.Tables[0].Rows[i]["month_year"].ToString())==cumd)
    //                            {
    //                                if (ds1.Tables[0].Rows[i]["month_year"].ToString() == cumd.ToString())
    //                                {
    //                                    //i++;
    //                                    if ((split_holiday_status_1 == "1" && Ihof >= 1) || (split_holiday_status_2 == "1" && IIhof <= 1 && Ihof < 1))
    //                                    {
    //                                        if (ds1.Tables[0].Rows[i][m1].ToString() != string.Empty)
    //                                        {
    //                                            unmark_flag = true;
    //                                            hour1 = int.Parse(ds1.Tables[0].Rows[i][m1].ToString());
    //                                        }
    //                                    }
    //                                    if ((split_holiday_status_1 == "1" && Ihof >= 2) || (split_holiday_status_2 == "1" && IIhof <= 2 && Ihof < 2))
    //                                    {
    //                                        if (ds1.Tables[0].Rows[i][m2].ToString() != string.Empty)
    //                                        {
    //                                            unmark_flag = true;
    //                                            hour2 = int.Parse(ds1.Tables[0].Rows[i][m2].ToString());
    //                                        }
    //                                    }
    //                                    if ((split_holiday_status_1 == "1" && Ihof >= 3) || (split_holiday_status_2 == "1" && IIhof <= 3 && Ihof < 3))
    //                                    {
    //                                        if (ds1.Tables[0].Rows[i][m3].ToString() != string.Empty)
    //                                        {
    //                                            unmark_flag = true;
    //                                            hour3 = int.Parse(ds1.Tables[0].Rows[i][m3].ToString());
    //                                        }
    //                                    }
    //                                    if ((split_holiday_status_1 == "1" && Ihof >= 4) || (split_holiday_status_2 == "1" && IIhof <= 4 && Ihof < 4))
    //                                    {
    //                                        if (ds1.Tables[0].Rows[i][m4].ToString() != string.Empty)
    //                                        {
    //                                            unmark_flag = true;
    //                                            hour4 = int.Parse(ds1.Tables[0].Rows[i][m4].ToString());
    //                                        }
    //                                    }
    //                                    if ((split_holiday_status_1 == "1" && Ihof >= 5) || (split_holiday_status_2 == "1" && IIhof <= 5 && Ihof < 5))
    //                                    {
    //                                        if (ds1.Tables[0].Rows[i][m5].ToString() != string.Empty)
    //                                        {
    //                                            unmark_flag = true;
    //                                            hour5 = int.Parse(ds1.Tables[0].Rows[i][m5].ToString());
    //                                        }
    //                                    }
    //                                    if ((split_holiday_status_1 == "1" && Ihof >= 6) || (split_holiday_status_2 == "1" && IIhof <= 6 && Ihof < 6))
    //                                    {
    //                                        if (ds1.Tables[0].Rows[i][m6].ToString() != string.Empty)
    //                                        {
    //                                            unmark_flag = true;
    //                                            hour6 = int.Parse(ds1.Tables[0].Rows[i][m6].ToString());
    //                                        }
    //                                    }
    //                                    if ((split_holiday_status_1 == "1" && Ihof <= 7) || (split_holiday_status_2 == "1" && IIhof <= 7 && Ihof < 7))
    //                                    {
    //                                        if (ds1.Tables[0].Rows[i][m7].ToString() != string.Empty)
    //                                        {
    //                                            unmark_flag = true;
    //                                            hour7 = int.Parse(ds1.Tables[0].Rows[i][m7].ToString());
    //                                        }
    //                                    }
    //                                    if ((split_holiday_status_1 == "1" && Ihof >= 8) || (split_holiday_status_2 == "1" && IIhof <= 8 && Ihof < 8))
    //                                    {
    //                                        if (ds1.Tables[0].Rows[i][m8].ToString() != string.Empty)
    //                                        {
    //                                            unmark_flag = true;
    //                                            hour8 = int.Parse(ds1.Tables[0].Rows[i][m8].ToString());
    //                                        }
    //                                    }
    //                                    if ((split_holiday_status_1 == "1" && Ihof >= 9) || (split_holiday_status_2 == "1" && IIhof <= 9 && Ihof < 9))
    //                                    {
    //                                        if (ds1.Tables[0].Rows[i][m9].ToString() != string.Empty)
    //                                        {
    //                                            unmark_flag = true;
    //                                            hour9 = int.Parse(ds1.Tables[0].Rows[i][m9].ToString());
    //                                        }
    //                                    }

    //                                    hat.Clear();
    //                                    hat.Add("m1", hour1.ToString());
    //                                    hat.Add("m2", hour2.ToString());
    //                                    hat.Add("m3", hour3.ToString());
    //                                    hat.Add("m4", hour4.ToString());
    //                                    hat.Add("m5", hour5.ToString());
    //                                    hat.Add("m6", hour6.ToString());
    //                                    hat.Add("m7", hour7.ToString());
    //                                    hat.Add("m8", hour8.ToString());
    //                                    hat.Add("m9", hour9.ToString());

    //                                    ds2 = da.select_method("CAL_DAYS", hat, "sp");

    //                                    if ((split_holiday_status_1 == "1" && Ihof >= 1) || (split_holiday_status_2 == "1" && IIhof <= 1 && Ihof < 1))
    //                                    {
    //                                        if (ds2.Tables[0].Rows.Count != 0)
    //                                        {
    //                                            if (ds2.Tables[0].Rows[0]["FLAG"].ToString() == "0")
    //                                            {
    //                                                wk1 = 1;
    //                                            }
    //                                            else
    //                                            {
    //                                                if (ds2.Tables[0].Rows[0]["FLAG"].ToString() != "2")//==========15/5/12 PRABHA
    //                                                {
    //                                                    dayflag = true;
    //                                                    absenthrs = absenthrs + 1;
    //                                                    abshrs_temp = "1";

    //                                                }
    //                                                else//==========26/5/12 PRABHA
    //                                                {
    //                                                    wk1 = 1;//==========26/5/12 PRABHA
    //                                                }
    //                                            }

    //                                        }
    //                                        else
    //                                        {
    //                                            condhrs1 = 1;
    //                                        }
    //                                    }
    //                                    if ((split_holiday_status_1 == "1" && Ihof >= 2) || (split_holiday_status_2 == "1" && IIhof <= 2 && Ihof < 2))
    //                                    {
    //                                        if (ds2.Tables[1].Rows.Count != 0)
    //                                        {
    //                                            if (ds2.Tables[1].Rows[0]["FLAG"].ToString() == "0")
    //                                            {
    //                                                wk2 = 1;
    //                                            }
    //                                            else
    //                                            {
    //                                                if (ds2.Tables[1].Rows[0]["FLAG"].ToString() != "2")//==========15/5/12 PRABHA
    //                                                {
    //                                                    absenthrs = absenthrs + 1;
    //                                                    if (abshrs_temp == string.Empty)
    //                                                    {
    //                                                        dayflag = true;
    //                                                        abshrs_temp = "2";
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        dayflag = true;
    //                                                        abshrs_temp = abshrs_temp + ",2";
    //                                                    }


    //                                                }
    //                                                else//==========26/5/12 PRABHA
    //                                                {
    //                                                    wk2 = 1;//==========26/5/12 PRABHA
    //                                                }
    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            condhrs2 = 1;
    //                                        }
    //                                    }
    //                                    if ((split_holiday_status_1 == "1" && Ihof >= 3) || (split_holiday_status_2 == "1" && IIhof <= 3 && Ihof < 3))
    //                                    {
    //                                        if (ds2.Tables[2].Rows.Count != 0)
    //                                        {
    //                                            if (ds2.Tables[2].Rows[0]["FLAG"].ToString() == "0")
    //                                            {
    //                                                wk3 = 1;
    //                                            }
    //                                            else
    //                                            {
    //                                                if (ds2.Tables[2].Rows[0]["FLAG"].ToString() != "2")//==========15/5/12 PRABHA
    //                                                {
    //                                                    absenthrs = absenthrs + 1;
    //                                                    if (abshrs_temp == string.Empty)
    //                                                    {
    //                                                        dayflag = true;
    //                                                        abshrs_temp = "3";
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        dayflag = true;
    //                                                        abshrs_temp = abshrs_temp + ",3";
    //                                                    }



    //                                                }
    //                                                else//==========26/5/12 PRABHA
    //                                                {
    //                                                    wk3 = 1;//==========26/5/12 PRABHA
    //                                                }
    //                                            }

    //                                        }

    //                                        else
    //                                        {


    //                                            condhrs3 = 1;

    //                                        }
    //                                    }

    //                                    if ((split_holiday_status_1 == "1" && Ihof >= 4) || (split_holiday_status_2 == "1" && IIhof <= 4 && Ihof < 4))
    //                                    {
    //                                        if (ds2.Tables[3].Rows.Count != 0)
    //                                        {
    //                                            if (ds2.Tables[3].Rows[0]["FLAG"].ToString() == "0")
    //                                            {
    //                                                wk4 = 1;
    //                                            }
    //                                            else
    //                                            {
    //                                                if (ds2.Tables[3].Rows[0]["FLAG"].ToString() != "2")//==========15/5/12 PRABHA
    //                                                {
    //                                                    absenthrs = absenthrs + 1;
    //                                                    if (abshrs_temp == string.Empty)
    //                                                    {
    //                                                        dayflag = true;
    //                                                        abshrs_temp = "4";
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        dayflag = true;
    //                                                        abshrs_temp = abshrs_temp + ",4";
    //                                                    }


    //                                                }
    //                                                else//==========26/5/12 PRABHA
    //                                                {
    //                                                    wk4 = 1;//==========26/5/12 PRABHA
    //                                                }
    //                                            }

    //                                        }
    //                                        else
    //                                        {

    //                                            condhrs4 = 1;

    //                                        }
    //                                    }

    //                                    if ((split_holiday_status_1 == "1" && Ihof >= 5) || (split_holiday_status_2 == "1" && IIhof <= 5 && Ihof < 5))
    //                                    {
    //                                        if (ds2.Tables[4].Rows.Count != 0)
    //                                        {
    //                                            if (ds2.Tables[4].Rows[0]["FLAG"].ToString() == "0")
    //                                            {
    //                                                wk5 = 1;
    //                                            }


    //                                            else
    //                                            {
    //                                                if (ds2.Tables[4].Rows[0]["FLAG"].ToString() != "2")//==========15/5/12 PRABHA
    //                                                {
    //                                                    absenthrs = absenthrs + 1;
    //                                                    if (abshrs_temp == string.Empty)
    //                                                    {
    //                                                        dayflag = true;
    //                                                        abshrs_temp = "5";
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        dayflag = true;
    //                                                        abshrs_temp = abshrs_temp + ",5";
    //                                                    }


    //                                                }
    //                                                else//==========26/5/12 PRABHA
    //                                                {
    //                                                    wk5 = 1;//==========26/5/12 PRABHA
    //                                                }
    //                                            }

    //                                        }
    //                                        else
    //                                        {

    //                                            condhrs5 = 1;
    //                                        }
    //                                    }

    //                                    if ((split_holiday_status_1 == "1" && Ihof >= 6) || (split_holiday_status_2 == "1" && IIhof <= 6 && Ihof < 6))
    //                                    {
    //                                        if (ds2.Tables[5].Rows.Count != 0)
    //                                        {
    //                                            if (ds2.Tables[5].Rows[0]["FLAG"].ToString() == "0")
    //                                            {
    //                                                wk6 = 1;
    //                                            }
    //                                            else
    //                                            {
    //                                                if (ds2.Tables[5].Rows[0]["FLAG"].ToString() != "2")//==========15/5/12 PRABHA
    //                                                {
    //                                                    absenthrs = absenthrs + 1;
    //                                                    if (abshrs_temp == string.Empty)
    //                                                    {
    //                                                        dayflag = true;
    //                                                        abshrs_temp = "6";
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        dayflag = true;
    //                                                        abshrs_temp = abshrs_temp + ",6";
    //                                                    }


    //                                                }
    //                                                else//==========26/5/12 PRABHA
    //                                                {
    //                                                    wk6 = 1;//==========26/5/12 PRABHA
    //                                                }
    //                                            }

    //                                        }
    //                                        else
    //                                        {

    //                                            condhrs6 = 1;
    //                                        }
    //                                    }

    //                                    if ((split_holiday_status_1 == "1" && Ihof >= 7) || (split_holiday_status_2 == "1" && IIhof <= 7 && Ihof < 7))
    //                                    {
    //                                        if (ds2.Tables[6].Rows.Count != 0)
    //                                        {
    //                                            if (ds2.Tables[6].Rows[0]["FLAG"].ToString() == "0")
    //                                            {
    //                                                wk7 = 1;
    //                                            }
    //                                            else
    //                                            {

    //                                                if (ds2.Tables[6].Rows[0]["FLAG"].ToString() != "2")//==========15/5/12 PRABHA
    //                                                {
    //                                                    absenthrs = absenthrs + 1;
    //                                                    if (abshrs_temp == string.Empty)
    //                                                    {
    //                                                        dayflag = true;
    //                                                        abshrs_temp = "7";
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        dayflag = true;
    //                                                        abshrs_temp = abshrs_temp + ",7";
    //                                                    }


    //                                                }
    //                                                else//==========26/5/12 PRABHA
    //                                                {
    //                                                    wk7 = 1;//==========26/5/12 PRABHA
    //                                                }
    //                                            }

    //                                        }
    //                                        else
    //                                        {
    //                                            condhrs7 = 1;
    //                                        }

    //                                    }
    //                                    if ((split_holiday_status_1 == "1" && Ihof >= 8) || (split_holiday_status_2 == "1" && IIhof <= 8 && Ihof < 8))
    //                                    {
    //                                        if (ds2.Tables[7].Rows.Count != 0)
    //                                        {
    //                                            if (ds2.Tables[7].Rows[0]["FLAG"].ToString() == "0")
    //                                            {
    //                                                wk8 = 1;
    //                                            }
    //                                            else
    //                                            {
    //                                                if (ds2.Tables[7].Rows[0]["FLAG"].ToString() != "2")//==========15/5/12 PRABHA
    //                                                {
    //                                                    absenthrs = absenthrs + 1;
    //                                                    if (abshrs_temp == string.Empty)
    //                                                    {
    //                                                        dayflag = true;
    //                                                        abshrs_temp = "8";
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        dayflag = true;
    //                                                        abshrs_temp = abshrs_temp + ",8";
    //                                                    }


    //                                                }
    //                                                else//==========26/5/12 PRABHA
    //                                                {
    //                                                    wk8 = 1;//==========26/5/12 PRABHA
    //                                                }
    //                                            }

    //                                        }
    //                                        else
    //                                        {

    //                                            condhrs8 = 1;
    //                                        }

    //                                    }
    //                                    if ((split_holiday_status_1 == "1" && Ihof >= 9) || (split_holiday_status_2 == "1" && IIhof <= 9 && Ihof < 9))
    //                                    {
    //                                        if (ds2.Tables[8].Rows.Count != 0)
    //                                        {
    //                                            if (ds2.Tables[8].Rows[0]["FLAG"].ToString() == "0")
    //                                            {
    //                                                wk9 = 1;
    //                                            }
    //                                            else
    //                                            {

    //                                                if (ds2.Tables[8].Rows[0]["FLAG"].ToString() != "2")//==========15/5/12 PRABHA
    //                                                {
    //                                                    absenthrs = absenthrs + 1;
    //                                                    if (abshrs_temp == string.Empty)
    //                                                    {
    //                                                        dayflag = true;
    //                                                        abshrs_temp = "9";
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        dayflag = true;
    //                                                        abshrs_temp = abshrs_temp + ",9";
    //                                                    }


    //                                                }
    //                                                else//==========26/5/12 PRABHA
    //                                                {
    //                                                    wk9 = 1;//==========26/5/12 PRABHA
    //                                                }
    //                                            }


    //                                        }
    //                                        else
    //                                        {
    //                                            condhrs9 = 1;
    //                                        }
    //                                    }

    //                                    if (fullday == 9)
    //                                    {
    //                                        // condhrs = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8 + condhrs9;
    //                                        //  if (hour1 != 0 && hour2 != 0 && hour3 != 0 && hour4 != 0 && hour5 != 0 && hour6 != 0 && hour7 != 0 && hour8 != 0 && hour9 != 0)
    //                                        {

    //                                            if (Ihof == 0 && IIhof == 9)
    //                                            {
    //                                                condhrs = 0;
    //                                                condhrs_2 = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8 + condhrs9;
    //                                                att = 0;
    //                                                att2 = wk1 + wk2 + wk3 + wk4 + wk5 + wk6 + wk7 + wk8 + wk9;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                            }
    //                                            else if (Ihof == 1 && IIhof == 8)
    //                                            {
    //                                                condhrs = condhrs1;
    //                                                condhrs_2 = condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8 + condhrs9;
    //                                                att = wk1;
    //                                                att2 = wk2 + wk3 + wk4 + wk5 + wk6 + wk7 + wk8 + wk9;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 2 && IIhof == 7)
    //                                            {
    //                                                condhrs = condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8 + condhrs9;
    //                                                att = wk1 + wk2;
    //                                                att2 = wk3 + wk4 + wk5 + wk6 + wk7 + wk8 + wk9;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }
    //                                            else if (Ihof == 3 && IIhof == 6)
    //                                            {
    //                                                condhrs = condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8 + condhrs9;
    //                                                att = wk1 + wk2 + wk3;
    //                                                att2 = wk4 + wk5 + wk6 + wk7 + wk8 + wk9;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }


    //                                            else if (Ihof == 4 && IIhof == 5)
    //                                            {
    //                                                condhrs = condhrs4 + condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs5 + condhrs6 + condhrs7 + condhrs8 + condhrs9;
    //                                                att = wk1 + wk2 + wk3 + wk4;
    //                                                att2 = wk5 + wk6 + wk7 + wk8 + wk9;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }


    //                                            else if (Ihof == 5 && IIhof == 4)
    //                                            {
    //                                                condhrs = condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs6 + condhrs7 + condhrs8 + condhrs9;
    //                                                att = wk1 + wk2 + wk3 + wk4 + wk5;
    //                                                att2 = wk6 + wk7 + wk8 + wk9;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 6 && IIhof == 3)
    //                                            {
    //                                                condhrs = condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs7 + condhrs8 + condhrs9;
    //                                                att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6;
    //                                                att2 = wk7 + wk8 + wk9;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                            }

    //                                            else if (Ihof == 7 && IIhof == 2)
    //                                            {
    //                                                condhrs = condhrs7 + condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs8 + condhrs9;
    //                                                att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6 + wk7;
    //                                                att2 = wk8 + wk9;


    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 8 && IIhof == 1)
    //                                            {

    //                                                condhrs = condhrs8 + condhrs7 + condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs9;
    //                                                att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6 + wk7 + wk8;
    //                                                att2 = wk9;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 9 && IIhof == 0)
    //                                            {
    //                                                condhrs = condhrs9 + condhrs8 + condhrs7 + condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = 0;
    //                                                att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6 + wk7 + wk8 + wk9;
    //                                                att2 = 0;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                        }


    //                                    }
    //                                    else if (fullday == 8)
    //                                    {
    //                                        //condhrs = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8;
    //                                        //     if (hour1 != 0 && hour2 != 0 && hour3 != 0 && hour4 != 0 && hour5 != 0 && hour6 != 0 && hour7 != 0 && hour8 != 0)
    //                                        {
    //                                            if (Ihof == 0 && IIhof == 8)
    //                                            {
    //                                                condhrs = 0;
    //                                                condhrs_2 = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8;
    //                                                att = 0;
    //                                                att2 = wk1 + wk2 + wk3 + wk4 + wk5 + wk6 + wk7 + wk8;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 1 && IIhof == 7)
    //                                            {

    //                                                condhrs = condhrs1;
    //                                                condhrs_2 = condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8;
    //                                                att = wk1;
    //                                                att2 = wk2 + wk3 + wk4 + wk5 + wk6 + wk7 + wk8;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }


    //                                            else if (Ihof == 2 && IIhof == 6)
    //                                            {

    //                                                condhrs = condhrs1 + condhrs2;
    //                                                condhrs_2 = condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8;
    //                                                att = wk1 + wk2;
    //                                                att2 = wk3 + wk4 + wk5 + wk6 + wk7 + wk8;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 3 && IIhof == 5)
    //                                            {
    //                                                condhrs = condhrs3 + condhrs1 + condhrs2;
    //                                                condhrs_2 = condhrs4 + condhrs5 + condhrs6 + condhrs7 + condhrs8;
    //                                                att = wk1 + wk2 + wk3;
    //                                                att2 = wk4 + wk5 + wk6 + wk7 + wk8;


    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 4 && IIhof == 4)
    //                                            {
    //                                                condhrs = condhrs4 + condhrs3 + condhrs1 + condhrs2;
    //                                                condhrs_2 = condhrs5 + condhrs6 + condhrs7 + condhrs8;
    //                                                att = wk1 + wk2 + wk3 + wk4;
    //                                                att2 = wk5 + wk6 + wk7 + wk8;


    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 5 && IIhof == 3)
    //                                            {
    //                                                condhrs = condhrs5 + condhrs4 + condhrs3 + condhrs1 + condhrs2;
    //                                                condhrs_2 = condhrs6 + condhrs7 + condhrs8;
    //                                                att = wk1 + wk2 + wk3 + wk4 + wk5;
    //                                                att2 = wk6 + wk7 + wk8;


    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }


    //                                            else if (Ihof == 6 && IIhof == 2)
    //                                            {
    //                                                condhrs = condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs1 + condhrs2;
    //                                                condhrs_2 = condhrs7 + condhrs8;
    //                                                att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6;
    //                                                att2 = wk7 + wk8;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {

    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;

    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }


    //                                            else if (Ihof == 7 && IIhof == 1)
    //                                            {
    //                                                condhrs = condhrs7 + condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs1 + condhrs2;
    //                                                condhrs_2 = condhrs8;
    //                                                att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6 + wk7;
    //                                                att2 = wk8;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                        wk8 = 0;

    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }


    //                                            else if (Ihof == 8 && IIhof == 0)
    //                                            {
    //                                                condhrs = condhrs7 + condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs1 + condhrs2 + condhrs8;
    //                                                att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6 + wk7 + wk8;
    //                                                att2 = 0;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                        }



    //                                    }

    //                                    else if (fullday == 7)
    //                                    {
    //                                        //condhrs = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7;
    //                                        //  if (hour1 != 0 && hour2 != 0 && hour3 != 0 && hour4 != 0 && hour5 != 0 && hour6 != 0 && hour7 != 0)
    //                                        {

    //                                            if (Ihof == 0 && IIhof == 7)
    //                                            {
    //                                                condhrs = 0;
    //                                                condhrs_2 = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7;
    //                                                att = 0;
    //                                                att2 = wk1 + wk2 + wk3 + wk4 + wk5 + wk6 + wk7;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {

    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 1 && IIhof == 6)
    //                                            {
    //                                                condhrs = condhrs1;
    //                                                condhrs_2 = condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7;
    //                                                att = wk1;
    //                                                att2 = wk2 + wk3 + wk4 + wk5 + wk6 + wk7;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        att = 0;
    //                                                        present += 0.5;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }


    //                                            else if (Ihof == 2 && IIhof == 5)
    //                                            {
    //                                                condhrs = condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs3 + condhrs4 + condhrs5 + condhrs6 + condhrs7;
    //                                                att = wk1 + wk2;
    //                                                att2 = wk3 + wk4 + wk5 + wk6 + wk7;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {

    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }


    //                                            else if (Ihof == 3 && IIhof == 4)
    //                                            {
    //                                                condhrs = condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs4 + condhrs5 + condhrs6 + condhrs7;
    //                                                att = wk1 + wk2 + wk3;
    //                                                att2 = wk4 + wk5 + wk6 + wk7;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 4 && IIhof == 3)
    //                                            {
    //                                                condhrs = condhrs4 + condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs5 + condhrs6 + condhrs7;
    //                                                att = wk1 + wk2 + wk3 + wk4;
    //                                                att2 = wk5 + wk6 + wk7;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 5 && IIhof == 2)
    //                                            {
    //                                                condhrs = condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs6 + condhrs7;
    //                                                att = wk1 + wk2 + wk3 + wk4 + wk5;
    //                                                att2 = wk6 + wk7;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {

    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }

    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }


    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 6 && IIhof == 1)
    //                                            {
    //                                                condhrs = condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs7;
    //                                                att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6;
    //                                                att2 = wk7;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 7 && IIhof == 0)
    //                                            {
    //                                                condhrs = condhrs7 + condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = 0;
    //                                                att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6 + wk7;
    //                                                att2 = 0;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }
    //                                        }
    //                                    }


    //                                    else if (fullday == 6)
    //                                    {
    //                                        //  condhrs = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6;
    //                                        //   if (hour1 != 0 && hour2 != 0 && hour3 != 0 && hour4 != 0 && hour5 != 0 && hour6 != 0)
    //                                        {

    //                                            if (Ihof == 0 && IIhof == 6)
    //                                            {
    //                                                condhrs = 0;
    //                                                condhrs_2 = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6;

    //                                                att = 0;
    //                                                att2 = wk1 + wk2 + wk3 + wk4 + wk5 + wk6;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 1 && IIhof == 5)
    //                                            {

    //                                                condhrs = condhrs1;
    //                                                condhrs_2 = condhrs2 + condhrs3 + condhrs4 + condhrs5 + condhrs6;
    //                                                att = wk1;
    //                                                att2 = wk2 + wk3 + wk4 + wk5 + wk6;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        att = 0;
    //                                                        present += 0.5;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {

    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }


    //                                            else if (Ihof == 2 && IIhof == 4)
    //                                            {

    //                                                condhrs = condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs3 + condhrs4 + condhrs5 + condhrs6;
    //                                                att = wk1 + wk2;
    //                                                att2 = wk3 + wk4 + wk5 + wk6;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                                if (split_holiday_status_2 == "1")
    //                                                {

    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }


    //                                            else if (Ihof == 3 && IIhof == 3)
    //                                            {
    //                                                condhrs = condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs4 + condhrs5 + condhrs6;

    //                                                att = wk1 + wk2 + wk3;
    //                                                att2 = wk4 + wk5 + wk6;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 4 && IIhof == 2)
    //                                            {

    //                                                condhrs = condhrs4 + condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs5 + condhrs6;
    //                                                att = wk1 + wk2 + wk3 + wk4;
    //                                                att2 = wk5 + wk6;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 5 && IIhof == 1)
    //                                            {

    //                                                condhrs = condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs6;
    //                                                att = wk1 + wk2 + wk3 + wk4 + wk5;
    //                                                att2 = wk6;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }


    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 6 && IIhof == 0)
    //                                            {


    //                                                condhrs = condhrs6 + condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = 0;
    //                                                att = wk1 + wk2 + wk3 + wk4 + wk5 + wk6;
    //                                                att2 = 0;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }

    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                        }


    //                                    }
    //                                    else if (fullday == 5)
    //                                    {
    //                                        //condhrs = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5;
    //                                        //  if (hour1 != 0 && hour2 != 0 && hour3 != 0 && hour4 != 0 && hour5 != 0)


    //                                        if (Ihof == 0 && IIhof == 5)
    //                                        {
    //                                            condhrs = 0;
    //                                            condhrs_2 = condhrs1 + condhrs2 + condhrs3 + condhrs4 + condhrs5;
    //                                            att = 0;
    //                                            att2 = wk1 + wk2 + wk3 + wk4 + wk5;
    //                                            lea2 = leave1 + leave2 + leave3 + leave4 + leave5;
    //                                            on_2 = ondu1 + ondu2 + ondu3 + ondu4 + ondu5;
    //                                            lea1 = 0;
    //                                            on_1 = 0;


    //                                            if (split_holiday_status_1 == "1")
    //                                            {
    //                                                if (minI - condhrs <= att)
    //                                                {
    //                                                    present += 0.5;
    //                                                }
    //                                                else if (minI <= lea1)
    //                                                {
    //                                                    leaves += 0.5;
    //                                                }
    //                                                else if (minI <= on_1)
    //                                                {

    //                                                    pertotondu += 0.5;

    //                                                }
    //                                                else
    //                                                {
    //                                                    absent += 0.5;
    //                                                }
    //                                            }

    //                                            if (split_holiday_status_2 == "1")
    //                                            {
    //                                                if (minII - condhrs_2 <= att2)
    //                                                {
    //                                                    present += 0.5;
    //                                                    att2 = 0;

    //                                                }
    //                                                else if (minII <= lea1)
    //                                                {
    //                                                    leaves += 0.5;
    //                                                }
    //                                                else if (minII <= on_1)
    //                                                {

    //                                                    pertotondu += 0.5;

    //                                                }
    //                                                else
    //                                                {
    //                                                    absent += 0.5;
    //                                                }
    //                                            }
    //                                        }

    //                                        else if (Ihof == 1 && IIhof == 4)
    //                                        {
    //                                            condhrs = condhrs1;
    //                                            condhrs_2 = condhrs2 + condhrs3 + condhrs4 + condhrs5;
    //                                            att = wk1;
    //                                            att2 = wk2 + wk3 + wk4 + wk5;
    //                                            lea2 = leave2 + leave3 + leave4 + leave5;
    //                                            on_2 = ondu2 + ondu3 + ondu4 + ondu5;
    //                                            lea1 = leave1;
    //                                            on_1 = ondu1;

    //                                            if (split_holiday_status_1 == "1")
    //                                            {
    //                                                if (minI - condhrs <= att)
    //                                                {
    //                                                    att = 0;
    //                                                    present += 0.5;
    //                                                }
    //                                                else if (minI <= lea1)
    //                                                {
    //                                                    leaves += 0.5;
    //                                                }
    //                                                else if (minI <= on_1)
    //                                                {

    //                                                    pertotondu += 0.5;

    //                                                }
    //                                                else
    //                                                {
    //                                                    absent += 0.5;
    //                                                }
    //                                            }

    //                                            if (split_holiday_status_2 == "1")
    //                                            {
    //                                                if (minII - condhrs_2 <= att2)
    //                                                {
    //                                                    present += 0.5;
    //                                                    att2 = 0;
    //                                                }
    //                                                else if (minII <= lea1)
    //                                                {
    //                                                    leaves += 0.5;
    //                                                }
    //                                                else if (minII <= on_1)
    //                                                {

    //                                                    pertotondu += 0.5;

    //                                                }
    //                                                else
    //                                                {
    //                                                    absent += 0.5;
    //                                                }
    //                                            }
    //                                        }


    //                                        else if (Ihof == 2 && IIhof == 3)
    //                                        {

    //                                            condhrs = condhrs2 + condhrs1;
    //                                            condhrs_2 = condhrs3 + condhrs4 + condhrs5;
    //                                            att = wk1 + wk2;
    //                                            att2 = wk3 + wk4 + wk5;
    //                                            lea2 = leave3 + leave4 + leave5;
    //                                            on_2 = ondu3 + ondu4 + ondu5;
    //                                            lea1 = leave1 + leave2;
    //                                            on_1 = ondu1 + ondu2;

    //                                            if (split_holiday_status_1 == "1")
    //                                            {
    //                                                if (minI - condhrs <= att)
    //                                                {
    //                                                    present += 0.5;
    //                                                }
    //                                                else if (minI <= lea1)
    //                                                {
    //                                                    leaves += 0.5;
    //                                                }
    //                                                else if (minI <= on_1)
    //                                                {

    //                                                    pertotondu += 0.5;

    //                                                }
    //                                                else
    //                                                {
    //                                                    absent += 0.5;
    //                                                }
    //                                            }

    //                                            if (split_holiday_status_2 == "1")
    //                                            {
    //                                                if (minII - condhrs_2 <= att2)
    //                                                {
    //                                                    present += 0.5;
    //                                                    att2 = 0;

    //                                                }
    //                                                else if (minII <= lea1)
    //                                                {
    //                                                    leaves += 0.5;
    //                                                }
    //                                                else if (minII <= on_1)
    //                                                {

    //                                                    pertotondu += 0.5;

    //                                                }
    //                                                else
    //                                                {
    //                                                    absent += 0.5;
    //                                                }
    //                                            }
    //                                        }


    //                                        else if (Ihof == 3 && IIhof == 2)
    //                                        {

    //                                            condhrs = condhrs3 + condhrs2 + condhrs1;
    //                                            condhrs_2 = condhrs4 + condhrs5;
    //                                            att = wk1 + wk2 + wk3;
    //                                            att2 = wk4 + wk5;
    //                                            lea2 = leave4 + leave5;
    //                                            on_2 = ondu4 + ondu5;
    //                                            lea1 = leave1 + leave2 + leave3;
    //                                            on_1 = ondu1 + ondu2 + ondu3;

    //                                            if (split_holiday_status_1 == "1")
    //                                            {
    //                                                if (minI - condhrs <= att)
    //                                                {
    //                                                    present += 0.5;
    //                                                    att = 0;
    //                                                }
    //                                                else if (minI <= lea1)
    //                                                {
    //                                                    leaves += 0.5;
    //                                                }
    //                                                else if (minI <= on_1)
    //                                                {

    //                                                    pertotondu += 0.5;

    //                                                }
    //                                                else
    //                                                {
    //                                                    absent += 0.5;
    //                                                }
    //                                            }

    //                                            if (split_holiday_status_2 == "1")
    //                                            {
    //                                                if (minII - condhrs_2 <= att2)
    //                                                {
    //                                                    present += 0.5;
    //                                                    att2 = 0;

    //                                                }
    //                                                else if (minII <= lea1)
    //                                                {
    //                                                    leaves += 0.5;
    //                                                }
    //                                                else if (minII <= on_1)
    //                                                {

    //                                                    pertotondu += 0.5;

    //                                                }
    //                                                else
    //                                                {
    //                                                    absent += 0.5;
    //                                                }
    //                                            }
    //                                        }

    //                                        else if (Ihof == 4 && IIhof == 1)
    //                                        {
    //                                            condhrs = condhrs4 + condhrs3 + condhrs2 + condhrs1;
    //                                            condhrs_2 = condhrs5;
    //                                            att = wk1 + wk2 + wk3 + wk4;
    //                                            att2 = wk5;
    //                                            lea2 = leave5;
    //                                            on_2 = ondu5;
    //                                            lea1 = leave1 + leave2 + leave3 + leave4;
    //                                            on_1 = ondu1 + ondu2 + ondu3 + ondu4;

    //                                            if (split_holiday_status_1 == "1")
    //                                            {
    //                                                if (minI - condhrs <= att)
    //                                                {
    //                                                    present += 0.5;
    //                                                    att = 0;
    //                                                }
    //                                                else if (minI <= lea1)
    //                                                {
    //                                                    leaves += 0.5;
    //                                                }
    //                                                else if (minI <= on_1)
    //                                                {

    //                                                    pertotondu += 0.5;

    //                                                }
    //                                                else
    //                                                {
    //                                                    absent += 0.5;
    //                                                }
    //                                            }

    //                                            if (split_holiday_status_2 == "1")
    //                                            {
    //                                                if (minII - condhrs_2 <= att2)
    //                                                {
    //                                                    present += 0.5;
    //                                                    att2 = 0;

    //                                                }
    //                                                else if (minII <= lea1)
    //                                                {
    //                                                    leaves += 0.5;
    //                                                }
    //                                                else if (minII <= on_1)
    //                                                {

    //                                                    pertotondu += 0.5;

    //                                                }
    //                                                else
    //                                                {
    //                                                    absent += 0.5;
    //                                                }
    //                                            }
    //                                        }

    //                                        else if (Ihof == 5 && IIhof == 0)
    //                                        {

    //                                            condhrs = condhrs5 + condhrs4 + condhrs3 + condhrs2 + condhrs1;
    //                                            condhrs_2 = 0;
    //                                            att = wk1 + wk2 + wk3 + wk4 + wk5;
    //                                            att2 = 0;
    //                                            lea2 = 0;
    //                                            on_2 = 0;
    //                                            lea1 = leave1 + leave2 + leave3 + leave4 + leave5;
    //                                            on_1 = ondu1 + ondu2 + ondu3 + ondu4 + ondu5;

    //                                            if (split_holiday_status_1 == "1")
    //                                            {
    //                                                if (minI - condhrs <= att)
    //                                                {
    //                                                    present += 0.5;
    //                                                    att = 0;
    //                                                }
    //                                                else if (minI <= lea1)
    //                                                {
    //                                                    leaves += 0.5;
    //                                                }
    //                                                else if (minI <= on_1)
    //                                                {

    //                                                    pertotondu += 0.5;

    //                                                }
    //                                                else
    //                                                {
    //                                                    absent += 0.5;
    //                                                }
    //                                            }

    //                                            if (split_holiday_status_2 == "1")
    //                                            {
    //                                                if (minI - condhrs_2 <= att2)
    //                                                {
    //                                                    present += 0.5;
    //                                                    att2 = 0;

    //                                                }
    //                                                else if (minII <= lea1)
    //                                                {
    //                                                    leaves += 0.5;
    //                                                }
    //                                                else if (minII <= on_1)
    //                                                {

    //                                                    pertotondu += 0.5;

    //                                                }
    //                                                else
    //                                                {
    //                                                    absent += 0.5;
    //                                                }
    //                                            }
    //                                        }


    //                                    }

    //                                    else if (fullday == 4)
    //                                    {
    //                                        // condhrs = condhrs1 + condhrs2 + condhrs3 + condhrs4;
    //                                        // if (hour1 != 0 && hour2 != 0 && hour3 != 0 && hour4 != 0)
    //                                        {


    //                                            if (Ihof == 0 && IIhof == 4)
    //                                            {
    //                                                condhrs = 0;
    //                                                condhrs_2 = condhrs1 + condhrs2 + condhrs3 + condhrs4;
    //                                                att = 0;
    //                                                att2 = wk1 + wk2 + wk3 + wk4;
    //                                                lea1 = 0;
    //                                                on_1 = 0;
    //                                                lea2 = leave1 + leave2 + leave3 + leave4;
    //                                                on_2 = ondu1 + ondu2 + ondu3 + ondu4;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                    }
    //                                                    else if (minI <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minI <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }
    //                                                    else if (minII <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minII <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 1 && IIhof == 3)
    //                                            {
    //                                                condhrs = condhrs1;
    //                                                condhrs_2 = condhrs2 + condhrs3 + condhrs4;
    //                                                att = wk1;
    //                                                att2 = wk2 + wk3 + wk4;
    //                                                lea1 = leave1;
    //                                                on_1 = ondu1;
    //                                                lea2 = leave2 + leave3 + leave4;
    //                                                on_2 = ondu2 + ondu3 + ondu4;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        att = 0;
    //                                                        present += 0.5;
    //                                                    }
    //                                                    else if (minI <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minI <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }
    //                                                    else if (minII <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minII <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }


    //                                            else if (Ihof == 2 && IIhof == 2)
    //                                            {
    //                                                condhrs = condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs3 + condhrs4;
    //                                                att = wk1 + wk2;
    //                                                att2 = wk3 + wk4;
    //                                                lea1 = leave1 + leave2;
    //                                                on_1 = ondu1 + ondu2;
    //                                                lea2 = leave3 + leave4;
    //                                                on_2 = ondu3 + ondu4;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                    }
    //                                                    else if (minI <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minI <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }
    //                                                    else if (minII <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minII <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }


    //                                            else if (Ihof == 3 && IIhof == 1)
    //                                            {

    //                                                condhrs = condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs4;
    //                                                att = wk1 + wk2 + wk3;
    //                                                att2 = wk4;
    //                                                lea1 = leave1 + leave2 + leave3;
    //                                                on_1 = ondu1 + ondu2 + ondu3;
    //                                                lea2 = leave4;
    //                                                on_2 = ondu4;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }
    //                                                    else if (minI <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minI <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }
    //                                                    else if (minII <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minII <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 4 && IIhof == 0)
    //                                            {
    //                                                condhrs = condhrs4 + condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = 0;
    //                                                att = wk1 + wk2 + wk3 + wk4;
    //                                                att2 = 0;
    //                                                lea1 = leave1 + leave2 + leave3 + leave4;
    //                                                on_1 = ondu1 + ondu2 + ondu3 + ondu4;
    //                                                lea2 = 0;
    //                                                on_2 = 0;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }
    //                                                    else if (minI <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minI <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }
    //                                                    else if (minII <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minII <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                        }
    //                                    }

    //                                    else if (fullday == 3)
    //                                    {
    //                                        //  condhrs = condhrs1 + condhrs2 + condhrs3;
    //                                        //   if (hour1 != 0 && hour2 != 0 && hour3 != 0)
    //                                        {

    //                                            if (Ihof == 0 && IIhof == 3)
    //                                            {
    //                                                condhrs = 0;

    //                                                condhrs_2 = condhrs1 + condhrs2 + condhrs3;
    //                                                att = 0;
    //                                                att2 = wk1 + wk2 + wk3;
    //                                                lea2 = leave1 + leave2 + leave3;
    //                                                on_2 = ondu1 + ondu2 + ondu3;
    //                                                lea1 = 0;
    //                                                on_1 = 0;

    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                    }
    //                                                    else if (minI <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minI <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;
    //                                                    }
    //                                                    else if (minII <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minII <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 1 && IIhof == 2)
    //                                            {
    //                                                condhrs = condhrs1;
    //                                                condhrs_2 = condhrs2 + condhrs3;
    //                                                att = wk1;
    //                                                att2 = wk2 + wk3;
    //                                                lea2 = leave2 + leave3;
    //                                                on_2 = ondu2 + ondu3;
    //                                                lea1 = leave1;
    //                                                on_1 = ondu1;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        att = 0;
    //                                                        present += 0.5;
    //                                                    }
    //                                                    else if (minI <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minI <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }
    //                                                    else if (minII <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minII <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }

    //                                                }
    //                                            }


    //                                            else if (Ihof == 2 && IIhof == 1)
    //                                            {

    //                                                condhrs = condhrs2 + condhrs1;
    //                                                condhrs_2 = condhrs3;
    //                                                att = wk1 + wk2;
    //                                                att2 = wk3;
    //                                                lea2 = leave3;
    //                                                on_2 = ondu3;
    //                                                lea1 = leave1 + leave2;
    //                                                on_1 = ondu1 + ondu2;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                    }
    //                                                    else if (minI <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minI <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }
    //                                                    else if (minII <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minII <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }


    //                                            else if (Ihof == 3 && IIhof == 0)
    //                                            {

    //                                                condhrs = condhrs3 + condhrs2 + condhrs1;
    //                                                condhrs_2 = 0;
    //                                                att = wk1 + wk2 + wk3;
    //                                                att2 = 0;
    //                                                lea2 = 0;
    //                                                on_2 = 0;
    //                                                lea1 = leave1 + leave2 + leave3;
    //                                                on_1 = ondu1 + ondu2 + ondu3;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att = 0;
    //                                                    }
    //                                                    else if (minI <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minI <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }

    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }
    //                                                    else if (minII <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minII <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                        }


    //                                    }

    //                                    else if (fullday == 2)
    //                                    {
    //                                        //  condhrs = condhrs1 + condhrs2;
    //                                        //  if (hour1 != 0 && hour2 != 0)
    //                                        {
    //                                            if (Ihof == 0 && IIhof == 2)
    //                                            {
    //                                                condhrs = 0;
    //                                                condhrs_2 = condhrs1 + condhrs2;

    //                                                att = 0;
    //                                                att2 = wk1 + wk2;
    //                                                lea1 = 0;
    //                                                on_1 = 0;
    //                                                lea2 = leave1 + leave2;
    //                                                on_2 = ondu1 + ondu2;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                    }
    //                                                    else if (minI <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minI <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }
    //                                                    else if (minII <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minII <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 1 && IIhof == 1)
    //                                            {

    //                                                condhrs = condhrs1;
    //                                                condhrs_2 = condhrs2;
    //                                                att = wk1;
    //                                                att2 = wk2;
    //                                                lea1 = leave1;
    //                                                on_1 = ondu1;
    //                                                lea2 = leave2;
    //                                                on_2 = ondu2;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        att = 0;
    //                                                        present += 0.5;
    //                                                    }
    //                                                    else if (minI <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minI <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }
    //                                                    else if (minII <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minII <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }


    //                                            else if (Ihof == 2 && IIhof == 0)
    //                                            {

    //                                                condhrs = condhrs1 + condhrs2;
    //                                                condhrs_2 = 0;
    //                                                att = wk1 + wk2;
    //                                                att2 = 0;
    //                                                lea1 = leave1 + leave2;
    //                                                on_1 = ondu1 + ondu2;
    //                                                lea2 = 0;
    //                                                on_2 = 0;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                    }
    //                                                    else if (minI <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minI <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {

    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }
    //                                                    else if (minII <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minII <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;
    //                                                    }
    //                                                }
    //                                            }


    //                                        }

    //                                    }

    //                                    else if (fullday == 1)
    //                                    {
    //                                        //  condhrs = condhrs1;

    //                                        //    if (hour1 != 0)
    //                                        {
    //                                            if (Ihof == 0 && IIhof == 1)
    //                                            {
    //                                                condhrs = 0;
    //                                                condhrs_2 = condhrs1;
    //                                                att = 0;
    //                                                att2 = wk1;
    //                                                lea2 = leave1;
    //                                                on_2 = ondu1;
    //                                                lea1 = 0;
    //                                                on_1 = 0;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        present += 0.5;
    //                                                    }
    //                                                    else if (minI <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minI <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;

    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {
    //                                                    if (minII <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }
    //                                                    else if (minII - condhrs_2 <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minII <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;

    //                                                    }
    //                                                }
    //                                            }

    //                                            else if (Ihof == 1 && IIhof == 0)
    //                                            {
    //                                                condhrs = condhrs1;
    //                                                condhrs_2 = 0;
    //                                                att = wk1;
    //                                                att2 = 0;
    //                                                lea1 = leave1;
    //                                                on_1 = ondu1;
    //                                                lea2 = 0;
    //                                                on_2 = 0;
    //                                                if (split_holiday_status_1 == "1")
    //                                                {
    //                                                    if (minI - condhrs <= att)
    //                                                    {
    //                                                        att = 0;
    //                                                        present += 0.5;
    //                                                    }
    //                                                    else if (minI <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minI <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;

    //                                                    }
    //                                                }

    //                                                if (split_holiday_status_2 == "1")
    //                                                {

    //                                                    if (minII - condhrs_2 <= att2)
    //                                                    {
    //                                                        present += 0.5;
    //                                                        att2 = 0;

    //                                                    }
    //                                                    else if (minII <= lea1)
    //                                                    {
    //                                                        leaves += 0.5;
    //                                                    }
    //                                                    else if (minII <= on_1)
    //                                                    {

    //                                                        pertotondu += 0.5;

    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        absent += 0.5;

    //                                                    }
    //                                                }
    //                                            }
    //                                        }

    //                                    }
    //                                    att = 0;
    //                                    att2 = 0;
    //                                    wk1 = 0;
    //                                    wk2 = 0;
    //                                    wk3 = 0;
    //                                    wk4 = 0;
    //                                    wk5 = 0;
    //                                    wk6 = 0;
    //                                    wk7 = 0;
    //                                    wk8 = 0;
    //                                    condhrs1 = 0;
    //                                    condhrs2 = 0;
    //                                    condhrs2 = 0;
    //                                    condhrs3 = 0;
    //                                    condhrs4 = 0;
    //                                    condhrs5 = 0;
    //                                    condhrs6 = 0;
    //                                    condhrs7 = 0;
    //                                    condhrs8 = 0;
    //                                    condhrs9 = 0;
    //                                    hour1 = 0;
    //                                    hour2 = 0;
    //                                    hour3 = 0;
    //                                    hour4 = 0;
    //                                    hour5 = 0;
    //                                    hour6 = 0;
    //                                    hour7 = 0;
    //                                    hour8 = 0;
    //                                    hour9 = 0;

    //                                }
    //                                cc++;
    //                                perabsenthrs = perabsenthrs + Convert.ToInt16(absenthrs);
    //                            }
    //                        }

    //                    }
    //                    abshrs_temp = "";
    //                    date_today = date_today.AddDays(1);
    //                }
    //            }
    //        }
    //        dat = 1;
    //        i++;
    //    }
    //    perabsent = absent;
    //    present = 0;
    //    presenthrs = 0;
    //    absent = 0;
    //    absenthrs = 0;
    //    totpresentday = 0;

    //}
    //public void cal_date(double cumd)
    //{

    //    int calm1 = fyy * 12 + 1;
    //    int calm2 = fyy * 12 + 2;
    //    int calm3 = fyy * 12 + 3;
    //    int calm4 = fyy * 12 + 4;
    //    int calm5 = fyy * 12 + 5;
    //    int calm6 = fyy * 12 + 6;
    //    int calm7 = fyy * 12 + 7;
    //    int calm8 = fyy * 12 + 8;
    //    int calm9 = fyy * 12 + 9;
    //    int calm10 = fyy * 12 + 10;
    //    int calm11 = fyy * 12 + 11;
    //    int calm12 = fyy * 12 + 12;
    //    if (calm1 == cumd || calm3 == cumd || calm5 == cumd || calm7 == cumd || calm8 == cumd || calm10 == cumd || calm12 == cumd)
    //    {
    //        daycount = 31;
    //    }
    //    if (calm4 == cumd || calm6 == cumd || calm9 == cumd || calm11 == cumd)
    //    {
    //        daycount = 30;
    //    }


    //    if (calm2 == cumd)
    //    {

    //        int lyear = 2000;
    //        int ly;
    //        if (lyear <= fyy)
    //        {
    //            ly = lyear - fyy;
    //        }
    //        else
    //        {
    //            ly = fyy - lyear;
    //        }
    //        if (ly == 4)
    //        {
    //            daycount = 29;
    //        }
    //        else
    //        {
    //            daycount = 28;

    //        }
    //    }

    //}
    //public void getspecial_hr()
    //{

    //    string hrdetno = "";
    //    if (ht_sphr.Contains(Convert.ToString(date_today)))
    //    {
    //        hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(date_today), ht_sphr));

    //    }
    //    if (hrdetno != "")
    //    {
    //        DataSet ds_splhr_query_master = new DataSet();
    //        string splhr_query_master = "select attendance from specialhr_attendance where roll_no='" + ds.Tables[0].Rows[count]["ROLL NO"].ToString() + "'  and hrdet_no in(" + hrdetno + ")";

    //        ds_splhr_query_master = da.select_method_wo_parameter(splhr_query_master, "text");
    //        if (ds_splhr_query_master.Tables[0].Rows.Count > 0)
    //        {
    //            if (absent_hash.Contains(ds_splhr_query_master.Tables[0].Rows[0]["attendance"].ToString()))
    //            {
    //                dayflag = true;
    //                perabsenthrs = perabsenthrs + 1;
    //            }
    //        }
    //    }
    //}
    //public void findholy()
    //{
    //    hat.Clear();
    //    hat.Add("date_val", date_today);
    //    hat.Add("sem_val", Session["sem"].ToString());
    //    for (int s = 0; s < chklstbranch.Items.Count; s++)
    //    {
    //        if (chklstbranch.Items[s].Selected == true)
    //        {
    //            hat.Add("degree_code", chklstbranch.Items[s].Value.ToString());
    //            s = chklstbranch.Items.Count;
    //        }
    //    }
    //    ds_holi = da.select_method("holiday_sp", hat, "sp");
    //}

    public void persentmonthcal()
    {
        try
        {
            Boolean isadm = false;
            per_abshrs_spl = 0;
            tot_per_hrs_spl = 0;
            tot_conduct_hr_spl = 0;
            tot_ondu_spl = 0;
            tot_ml_spl = 0;
            int my_un_mark = 0;
            int njdate_mng = 0, njdate_evng = 0;
            int per_holidate_mng = 0, per_holidate_evng = 0;
            mng_conducted_half_days = 0;
            evng_conducted_half_days = 0;
            notconsider_value = 0;
            cal_from_date = cal_from_date_tmp;
            cal_to_date = cal_to_date_tmp;
            per_from_date = per_from_gendate;
            per_to_date = per_to_gendate;
            dumm_from_date = per_from_date;

            string admdate = ds4.Tables[0].Rows[rows_count]["adm_date"].ToString();
            Admission_date = Convert.ToDateTime(admdate);

            dd = ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString();
            hat.Clear();
            hat.Add("std_rollno", ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString());
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds2 = da.select_method("STUD_ATTENDANCE", hat, "sp");
            mmyycount = ds2.Tables[0].Rows.Count;
            moncount = mmyycount - 1;
            if (deptflag == false)
            {
                deptflag = true;
                hat.Clear();
                hat.Add("degree_code", int.Parse(degree));
                hat.Add("sem", int.Parse(sem));
                hat.Add("from_date", frdate.ToString());
                hat.Add("to_date", todate.ToString());
                hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));
                int iscount = 0;
                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + degree + " and semester=" + sem;
                DataSet dsholiday = da.select_method_wo_parameter(sqlstr_holiday, "Text");
                if (dsholiday.Tables[0].Rows.Count > 0)
                {
                    iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                }
                hat.Add("iscount", iscount);
                ds3 = da.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");

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
                    }
                }

                if (ds3.Tables[1].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[1].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
                        if(!holiday_table21.Contains(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0]))
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
                while (dumm_from_date <= (per_to_date))
                {
                    nohrsprsentperday = 0;
                    noofdaypresen = 0;
                    isadm = false;
                    if (dumm_from_date >= Admission_date)
                    {
                        isadm = true;
                        int temp_unmark = 0;

                        for (int i = 1; i <= mmyycount; i++)
                        {
                            ds2.Tables[0].DefaultView.RowFilter = "month_year='" + cal_from_date + "' and roll_no='" + ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString() + "'";
                            DataView dvattvalue = ds2.Tables[0].DefaultView;
                            if (dvattvalue.Count > 0)
                            {
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
                                        value_holi_status = holiday_table11[dummy_split[1].ToString() + "/" + dummy_split[0].ToString() + "/" + dummy_split[2].ToString()].ToString();
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
                                            nohrsprsentperday = per_perhrs + njhr;
                                            if (per_perhrs + njhr >= minpresI)
                                            {
                                                Present += 0.5;
                                                noofdaypresen = 0.5;
                                            }
                                            else if (per_abshrs >= 1)
                                            {
                                                Absent += 0.5;
                                                absent_point += absent_pointer / 2;
                                                studentabsentfine = studentabsentfine + moringabsentfine;
                                            }
                                            if (njhr >= minpresI)
                                            {
                                                njdate += 0.5;
                                                njdate_mng += 1;
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
                                        per_abshrs = 0;
                                        temp_unmark = 0;
                                        njhr = 0;
                                        int k = fnhrs + 1;
                                        if (split_holiday_status_2 == "1")
                                        {
                                            for (i = k; i <= NoHrs; i++)
                                            {
                                                date = "d" + dumm_from_date.Day.ToString("") + "d" + i.ToString();
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
                                            nohrsprsentperday = nohrsprsentperday + per_perhrs + njhr;
                                            if (per_perhrs + njhr >= minpresII)
                                            {
                                                Present += 0.5;
                                                noofdaypresen = noofdaypresen + 0.5;
                                            }
                                            else if (per_abshrs >= 1)
                                            {
                                                Absent += 0.5;
                                                absent_point += absent_pointer / 2;
                                                studentabsentfine = studentabsentfine + eveingabsentfine;
                                            }
                                            if (njhr >= minpresII)
                                            {
                                                njdate_evng += 1;
                                                njdate += 0.5;
                                            }
                                            if (Session["attdaywisecla"].ToString() == "1")
                                            {
                                                if (nohrsprsentperday < minpresday)
                                                {
                                                    Present = Present - noofdaypresen;
                                                    Absent = Absent + noofdaypresen;
                                                }
                                            }
                                            nohrsprsentperday = 0;
                                            noofdaypresen = 0;
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
                                i = mmyycount + 1;
                            }
                            else
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
                    nohrsprsentperday = 0;
                    noofdaypresen = 0;
                }
                int diff_Date = per_from_date.Day - dumm_from_date.Day;
            }
            per_njdate = njdate;
            pre_present_date = Present - njdate;
            per_per_hrs = tot_per_hrs;
            per_absent_date = Absent;
            pre_ondu_date = Onduty;
            pre_leave_date = Leave;
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
        }
        catch
        {
        }
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
        spReportName.InnerHtml = "Student Periodwise Absentees Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }


    public override void VerifyRenderingInServerForm(Control control)
    { }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;
using InsproDataAccess;
using System.Text;

public partial class Condonation : System.Web.UI.Page
{
    #region Variable Declaration

    double leavfinaeamount = 0;
    double medicalLeaveDays = 0;
    double moringabsentfine = 0, eveingabsentfine = 0, studentabsentfine = 0;

    static Hashtable hasdaywise = new Hashtable();
    static Hashtable hashrwise = new Hashtable();
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();

    InsproDirectAccess dir = new InsproDirectAccess();

    double per_leavehrs;
    string regularflag = string.Empty; string new_header_string = string.Empty; string new_header_string_index = string.Empty;
    string genderflag = string.Empty;
    bool deptflag = false;
    int mmyycount;
    string dd = string.Empty;
    Hashtable hat = new Hashtable();

    static bool splhr_flag = false;
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();

    int days1 = 0;

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet dsprint = new DataSet();

    bool yesflag = false;
    double minpresday = 0, nohrsprsentperday = 0, noofdaypresen = 0;
    string leftlogo = "", rightlogo = "", leftlength = "", rightlength = "", multi_iso = string.Empty;
    int per_abshrs_spl_fals = 0, tot_per_hrs_spl_fals = 0, tot_conduct_hr_spl_fals = 0, tot_ondu_spl_fals = 0, tot_ml_spl_fals = 0;
    double per_leave_fals = 0;
    int per_abshrs_spl_true = 0, tot_per_hrs_spl_true = 0, tot_conduct_hr_spl_true = 0, tot_ondu_spl_true = 0, tot_ml_spl_true = 0;
    double per_leave_true = 0;
    int per_abshrs_spl = 0, tot_per_hrs_spl = 0, tot_ondu_spl = 0, tot_ml_spl = 0;
    int per_hhday_spl = 0, unmark_spl = 0, tot_conduct_hr_spl = 0;
    int mng_conducted_half_days = 0, evng_conducted_half_days = 0, per_workingdays1 = 0, cum_per_workingdays1 = 0;
    int notconsider_value = 0;
    double conduct_hour_new = 0;
    static string view_footer = "", view_header = "", view_footer_text = string.Empty;
    int moncount;
    double dif_date = 0;
    double dif_date1 = 0;
    int abs = 0, att = 0;
    int dum_diff_date, unmark;
    string roll_no, reg_no, roll_ad, studname;
    int check;
    string temp_reg_no = string.Empty;

    DateTime per_from_date;
    DateTime per_to_date;
    DateTime per_from_gendate;
    DateTime per_to_gendate;
    DateTime per_from_cumdate;
    DateTime per_to_cumdate;
    DateTime dumm_from_date;
    DateTime Admission_date;

    string frdate, todate, new_header_name = string.Empty;
    string halforfull = "", mng = "", evng = "", holiday_sched_details = string.Empty;
    int final_print_col_cnt = 0;
    bool check_col_count_flag = false;
    string column_field = "", printvar = string.Empty;
    int col_count_all = 0, span_cnt = 0, child_span_count = 0;
    int footer_count = 0, temp_count = 0, split_col_for_footer = 0, footer_balanc_col = 0;
    string footer_text = string.Empty;
    TimeSpan ts;
    string coll_name = "", address1 = "", degree_deatil = "", header_alignment = "", address2 = "", phoneno = "", faxno = "", email = "", address3 = "", website = "", form_name = "", pincode = string.Empty;
    string[] new_header_string_split;
    int end_column = 0;
    int temp_count_temp = 0;
    string phone = "", fax = "", email_id = "", web_add = string.Empty;
    string diff_date;
    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;
    int col_count = 0;
    int count;
    int next = 0;
    int minpresII = 0;
    string value, date;
    int i, rows_count;
    string tempvalue = "-1";
    int ObtValue = -1;
    int cal_from_date, cal_from_date_tmp, cal_from_cumdate_tmp;
    int cal_to_date, start_column = 0, cal_to_date_tmp, cal_to_cumdate_tmp;

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
    double tot_ondu, per_tot_ondu, cum_tot_ondu, cum_tot_ml, tot_ml, per_tot_ml;
    string usercode = "", collegecode = "", singleuser = "", group_user = string.Empty;
    string[] string_session_values;
    DataSet ds_attnd_pts = new DataSet();
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    string value_holi_status = string.Empty;
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = "", split_holiday_status_2 = string.Empty;
    string isonumber = string.Empty;
    int inirow_count = 0;
    static string grouporusercode = string.Empty;
    int demfcal, demtcal;
    string monthcal;
    DataSet ds_sphr = new DataSet();
    static Hashtable ht_sphr = new Hashtable();
    Hashtable hatonduty = new Hashtable();
    Hashtable hatcumonduty = new Hashtable();
    string strondutyvalue = string.Empty;
    int ondutycount = 0;
    string stronduquery = string.Empty;
    bool cumlaflag = false;
    int medicalLeaveHours = 0;
    int medicalLeaveCountPerSession = 0;
    int selDegree = 0;
    int selBranch = 0;
    int selSec = 0;
    string newDegreeCode = string.Empty;
    string newBranchCode = string.Empty;
    string newsections = string.Empty;
    string newBatchYear = string.Empty;
    string newsemester = string.Empty;
    string qryDegree = string.Empty;
    string qrySec = string.Empty;
    string qryBranch = string.Empty;
    DataTable dtAddFine = new DataTable();
    DataTable dtRenewDays = new DataTable();
    DataRow drCurrentRow;
    DataTable dtfine = new DataTable();
    DataRow drow;
    DataTable dt2 = new DataTable();

    DataTable datastudentwriteexam = new DataTable();
    DataRow datarow;
    static Dictionary<int, string> dicstudetails = new Dictionary<int, string>();
    static Dictionary<int, string> dicstucolspan = new Dictionary<int, string>();
    static int colcnt = 0;

    DataTable dataApplyCondonation = new DataTable();
    DataRow datarowApplyCondonation;
    static Dictionary<int, string> dicstudApplyCondonation = new Dictionary<int, string>();
    static Dictionary<int, string> dicCondonationappcnt = new Dictionary<int, string>();
    static Dictionary<int, string> dicstuApplyCondonationcolspan = new Dictionary<int, string>();
    static Dictionary<int, string> dicstuApplyCondonationedit = new Dictionary<int, string>();
    static int colcnt1 = 0;
    static int rowindex = 0;

    DataTable datanotelg = new DataTable();
    DataRow dtrownotelg;
    static Dictionary<int, string> dicstudnotelg = new Dictionary<int, string>();
    static Dictionary<int, string> dicstunotelgcolspan = new Dictionary<int, string>();
    static int colcnt3 = 0;
    #endregion



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
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
        }
        panelerrmsg.Visible = false;
        errmsg.Visible = false;
        if (!IsPostBack)
        {
            txtfdate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");
            txtCondonationDate.Attributes.Add("readonly", "readonly");
            txtfdate.Text = DateTime.Today.ToString("d/MM/yyyy");
            txttodate.Text = DateTime.Today.ToString("d/MM/yyyy");
            lblErrCondo.Text = string.Empty;
            lblErrCondo.Visible = false;
            bindstram();
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            BindSection();
            clear();
            chkShowDetails.Checked = false;
            rblDayOrPercerntage.SelectedIndex = 0;
            txtOrder.Text = string.Empty;
            ItemList.Clear();
            Itemindex.Clear();
            txtOrder.Visible = false;
            chkColumnOrderAll.Checked = false;
            string value = string.Empty;
            int index;
            value = string.Empty;
            for (int i = 0; i < cblColumnOrder.Items.Count; i++)
            {
                if (cblColumnOrder.Items[i].Selected == false)
                {
                    ItemList.Remove(cblColumnOrder.Items[i].Text.ToString());
                    Itemindex.Remove(Convert.ToString(i));
                }
                else
                {
                    if (!Itemindex.Contains(i))
                    {
                        ItemList.Add(cblColumnOrder.Items[i].Text.ToString());
                        Itemindex.Add(i);
                    }
                }
            }
            txtOrder.Visible = true;
            txtOrder.Text = string.Empty;
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (txtOrder.Text == "")
                {
                    txtOrder.Text = ItemList[i].ToString();
                }
                else
                {
                    txtOrder.Text = txtOrder.Text + "," + ItemList[i].ToString();
                }
            }
            if (ItemList.Count == cblColumnOrder.Items.Count)
            {
                chkColumnOrderAll.Checked = true;
            }
            if (ItemList.Count > 0)
            {
                txtOrder.Visible = false;
                lbtnRemoveAll.Visible = true;
            }
            else
            {
                txtOrder.Visible = false;
                lbtnRemoveAll.Visible = false;
            }
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            Session["attdaywisecla"] = "0";
            string daywisecal = d2.GetFunction("select value from Master_Settings where settings='Attendance Day Wise Calculation'");
            if (daywisecal.Trim() == "1")
            {
                Session["attdaywisecla"] = "1";
            }
            string grouporusercode = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            string Master = "select * from Master_Settings where " + grouporusercode + "";
            DataSet ds = d2.select_method(Master, hat, "Text");
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
    }

    public void bindstram()
    {
        try
        {
            ddlstream.Items.Clear();
            ddlstream.Enabled = false;
            #region Added By Malang Raja And Commented By Malang Raja
            //group_user = Session["group_code"].ToString();
            //if (group_user.Contains(';'))
            //{
            //    string[] group_semi = group_user.Split(';');
            //    group_user = group_semi[0].ToString();
            //}
            //usercode = Session["usercode"].ToString();
            //collegecode = Convert.ToString(Session["collegecode"]).Trim();
            //singleuser = Session["single_user"].ToString();
            //string strquery = "select distinct isnull(Ltrim(rtrim(course.type)),'') as type from degree,course, deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and isnull(Ltrim(rtrim(course.type)),'')<>'' and course.college_code ='" + Convert.ToString(Session["collegecode"]).Trim() + "' and user_code='" + usercode + "' ";
            //if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            //{
            //    strquery = "select distinct isnull(Ltrim(rtrim(course.type)),'') as type from degree,course, deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and isnull(Ltrim(rtrim(course.type)),'')<>''  and course.college_code ='" + Convert.ToString(Session["collegecode"]).Trim() + "' and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "' ";
            //}
            #endregion Added By Malang Raja And Commented By Malang Raja
            DataSet ds = d2.select_method_wo_parameter("select distinct isnull(Ltrim(rtrim(type)),'') as type from Course where isnull(Ltrim(rtrim(type)),'')<>'' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstream.DataSource = ds;
                ddlstream.DataTextField = "type";
                ddlstream.DataValueField = "type";
                ddlstream.DataBind();
                ddlstream.Enabled = true;
            }
            else
            {
                ddlstream.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    //public void BindBatch()
    //{
    //    try
    //    {
    //        ddlbatch.Items.Clear();
    //        DataSet ds = d2.BindBatch();
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddlbatch.DataSource = ds;
    //            ddlbatch.DataTextField = "Batch_year";
    //            ddlbatch.DataValueField = "Batch_year";
    //            ddlbatch.DataBind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errmsg.Visible = true;
    //        errmsg.Text = ex.ToString();
    //    }
    //}
    public void BindBatch()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();

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
            ds2 = d2.select_method_wo_parameter(strbinddegree, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklsbatch.DataSource = ds2;
                chklsbatch.DataTextField = "Batch_year";
                chklsbatch.DataValueField = "Batch_year";
                chklsbatch.DataBind();
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;

                }
                txtbatch.Text = Label1.Text + "(" + chklsbatch.Items.Count + ")";
                chkbatch.Checked = true;
            }


        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    protected void chkbatch_ChekedChange(object sender, EventArgs e)
    {
        CallCheckboxChange(chkbatch, chklsbatch, txtbatch, Label1.Text, "--Select--");
        BindDegree();
        bindbranch();
        bindsem();
        BindSection();
    }
    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chkbatch, chklsbatch, txtbatch, Label1.Text, "--Select--");
        BindDegree();
        bindbranch();
        bindsem();
        BindSection();
    }


    public void BindDegree()
    {
        try
        {
            ddldegree.Items.Clear();
            cblDegree.Items.Clear();
            chkDegree.Checked = false;
            txtDegree.Text = "-- Select --";
            //group_user = Session["group_code"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            usercode = Session["usercode"].ToString();
            collegecode = Convert.ToString(Session["collegecode"]).Trim();
            singleuser = Session["single_user"].ToString();
            //if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            //{
            //    grouporusercode = " and group_code='" + Session["group_code"].ToString().Trim() + "'";
            //}
            //else
            //{
            //    grouporusercode = " and user_code='" + Session["usercode"].ToString().Trim() + "'";
            //}
            string typeval = string.Empty;
            if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
            {
                typeval = " and type='" + Convert.ToString(ddlstream.SelectedItem).Trim() + "'";
            }
            string strquery = "select distinct degree.course_id,course.course_name from degree,course, deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and course.college_code ='" + Convert.ToString(Session["collegecode"]).Trim() + "' and user_code='" + usercode + "' " + typeval + "";
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                strquery = "select distinct degree.course_id,course.course_name from degree,course, deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and course.college_code ='" + Convert.ToString(Session["collegecode"]).Trim() + "' and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "' " + typeval + " ";
            }
            ds = d2.select_method_wo_parameter(strquery, "Text");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
                cblDegree.DataSource = ds;
                cblDegree.DataTextField = "course_name";
                cblDegree.DataValueField = "course_id";
                cblDegree.DataBind();
                foreach (ListItem li in cblDegree.Items)
                {
                    li.Selected = true;
                }
                txtDegree.Text = "Degree" + "(" + cblDegree.Items.Count + ")";
                chkDegree.Checked = true;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void bindbranch()
    {
        try
        {
            ddlbranch.Items.Clear();
            chkBranch.Checked = false;
            cblBranch.Items.Clear();
            txtBranch.Text = "-- Select --";
            hat.Clear();
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
            string typeval = string.Empty;
            if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
            {
                typeval = " and type='" + ddlstream.SelectedItem.ToString() + "'";
            }
            selDegree = 0;
            newDegreeCode = string.Empty;
            qryDegree = string.Empty;
            string coursecode = string.Empty;
            foreach (ListItem li in cblDegree.Items)
            {
                if (li.Selected)
                {
                    selDegree++;
                    if (string.IsNullOrEmpty(newDegreeCode.Trim()))
                    {
                        newDegreeCode = "'" + li.Value + "'";
                    }
                    else
                    {
                        newDegreeCode += ",'" + li.Value + "'";
                    }
                }
            }
            if (selDegree > 0)
            {
                coursecode = " and degree.course_id in(" + newDegreeCode + ")";
                string strquery = "select distinct degree.degree_code,de.dept_name from degree,course, deptprivilages,department de where course.course_id=degree.course_id and de.dept_code=degree.dept_code and course.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and course.college_code ='" + Convert.ToString(Session["collegecode"]).Trim() + "' and user_code='" + usercode + "' " + typeval + " " + coursecode + "";
                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    strquery = "select distinct degree.degree_code,de.dept_name from degree,course, deptprivilages,department de where course.course_id=degree.course_id and de.dept_code=degree.dept_code and course.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "' and course.college_code ='" + Convert.ToString(Session["collegecode"]).Trim() + "' " + typeval + "  " + coursecode + "";
                }
                ds = d2.select_method_wo_parameter(strquery, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count2 = ds.Tables[0].Rows.Count;
                if (count2 > 0)
                {
                    ddlbranch.DataSource = ds;
                    ddlbranch.DataTextField = "dept_name";
                    ddlbranch.DataValueField = "degree_code";
                    ddlbranch.DataBind();
                    cblBranch.DataSource = ds;
                    cblBranch.DataTextField = "dept_name";
                    cblBranch.DataValueField = "degree_code";
                    cblBranch.DataBind();
                    foreach (ListItem li in cblBranch.Items)
                    {
                        li.Selected = true;
                    }
                    txtBranch.Text = "Branch" + "(" + cblBranch.Items.Count + ")";
                    chkBranch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void bindsem()
    {
        try
        {
            ddlsemester.Items.Clear();
            bool first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            selBranch = 0;
            qryBranch = string.Empty;
            newBranchCode = string.Empty;
            foreach (ListItem li in cblBranch.Items)
            {
                if (li.Selected)
                {
                    selBranch++;
                    if (string.IsNullOrEmpty(newBranchCode.Trim()))
                    {
                        newBranchCode = "'" + li.Value + "'";
                    }
                    else
                    {
                        newBranchCode += ",'" + li.Value + "'";
                    }
                }
            }
            string selectedBatchYears = Convert.ToString(getCblSelectedValue(chklsbatch));
            if (selBranch > 0)
            {
                qryBranch = " and degree_code in(" + newBranchCode + ")";
            }
            string strgetsem = "select distinct ndurations,first_year_nonsemester from ndegree where batch_year in ('" + selectedBatchYears.ToString() + "') and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "' " + qryBranch + " order by NDurations desc ";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strgetsem, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsemester.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsemester.Items.Add(i.ToString());
                    }
                }
            }
            else
            {
                strgetsem = "select distinct duration,first_year_nonsemester  from degree where college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "' " + qryBranch + " order by  duration desc";
                ddlsemester.Items.Clear();
                ds.Dispose();
                ds.Reset();
                ds = d2.select_method_wo_parameter(strgetsem, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlsemester.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlsemester.Items.Add(i.ToString());
                        }
                    }
                }
            }
            //string minattexam = d2.GetFunction("select percent_eligible_for_exam from PeriodAttndSchedule where semester='" + ddlsemester.SelectedItem.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "'");
            //txtminattpercentage.Text = string.Empty;
            //if (minattexam.Trim() != "")
            //{
            //    txtminattpercentage.Text = minattexam;
            //}
            string minattexam = d2.GetFunction("select distinct percent_eligible_for_exam from PeriodAttndSchedule where semester='" + ddlsemester.SelectedItem.ToString() + "' " + qryBranch + " order by percent_eligible_for_exam desc");
            txtminattpercentage.Text = string.Empty;
            if (minattexam.Trim() != "")
            {
                txtminattpercentage.Text = minattexam;
            }
        }
        catch (Exception ex)
        {
            //errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void BindSection()
    {
        try
        {
            ddlsection.Items.Clear();
            chkSec.Checked = false;
            cblSec.Items.Clear();
            txtSec.Text = "-- Select --";
            txtSec.Enabled = false;
            selBranch = 0;
            qryBranch = string.Empty;
            newBranchCode = string.Empty;
            foreach (ListItem li in cblBranch.Items)
            {
                if (li.Selected)
                {
                    selBranch++;
                    if (string.IsNullOrEmpty(newBranchCode.Trim()))
                    {
                        newBranchCode = "'" + li.Value + "'";
                    }
                    else
                    {
                        newBranchCode += ",'" + li.Value + "'";
                    }
                }
            }
            string selectedBatchYears = Convert.ToString(getCblSelectedValue(chklsbatch));
            if (selBranch > 0)
            {
                qryBranch = " and degree_code in(" + newBranchCode + ")";
            }
            string strect = "select distinct case when (isnull(Rtrim(Ltrim(sections)),'') ='') then 'Empty' else isnull(Rtrim(Ltrim(sections)),'') end  as sections,isnull(Rtrim(Ltrim(sections)),'') as SecVal from registration where batch_year in ('" + selectedBatchYears.ToString() + "') " + qryBranch + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' union select'Empty' as sections,'' as SecVal  order by SecVal";//union select'Empty' as sections,'' as SecVal
            DataSet ds = d2.select_method_wo_parameter(strect, "Text");
            ddlsection.DataSource = ds;
            ddlsection.DataTextField = "sections";
            ddlsection.DataBind();
            ddlsection.Items.Insert(0, "All");
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["sections"].ToString() == string.Empty)
                {
                    ddlsection.Enabled = false;
                }
                else
                {
                    ddlsection.Enabled = true;
                }
                cblSec.DataSource = ds;
                cblSec.DataTextField = "sections";
                cblSec.DataValueField = "SecVal";
                cblSec.DataBind();
                foreach (ListItem li in cblSec.Items)
                {
                    li.Selected = true;
                }
                txtSec.Text = "Section" + "(" + cblSec.Items.Count + ")";
                chkSec.Checked = true;
                txtSec.Enabled = true;
            }
            else
            {
                txtSec.Enabled = false;
                ddlsection.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void clear()
    {
        errmsg.Visible = false;
        PWrite.Visible = false;
        Showgrid.Visible = false;
        PCondonation.Visible = false;
        Showgrid1.Visible = false;
        PNotEligible.Visible = false;
        Showgrid2.Visible = false;
        panelrollnopop.Visible = false;
        btnsave.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        txtexcelname.Text = string.Empty;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        Printcontrol.Visible = false;
        lblconrptname.Visible = false;
        txtconexcel.Visible = false;
        txtconexcel.Text = string.Empty;
        btnconxl.Visible = false;
        btnconprint.Visible = false;
        btnCondonationReport.Visible = false;
        PRINTPDF1.Visible = false;
        lblnotelexclname.Visible = false;
        txtnoteliexcel.Visible = false;
        txtnoteliexcel.Text = string.Empty;
        btnnoteliexcel.Visible = false;
        btnnoteliprint.Visible = false;
        PRINTPDF2.Visible = false;
        ddlCondonationReport.Visible = false;
        dicstudetails.Clear();
        dicstucolspan.Clear();
        dicstudApplyCondonation.Clear();
        dicCondonationappcnt.Clear();
        dicstuApplyCondonationcolspan.Clear();
        dicstudnotelg.Clear();
        dicstunotelgcolspan.Clear();
        dicstuApplyCondonationedit.Clear();
        colcnt1 = 0;
        colcnt = 0;
        colcnt3 = 0;
    }

    protected void ddlstream_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBatch();
        BindDegree();
        bindbranch();
        bindsem();
        BindSection();
        clear();
    }

    //protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindDegree();
    //    bindbranch();
    //    bindsem();
    //    BindSection();
    //    clear();
    //}

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
        BindSection();
        clear();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        BindSection();
        clear();
    }

    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        selBranch = 0;
        qryBranch = string.Empty;
        newBranchCode = string.Empty;
        foreach (ListItem li in cblBranch.Items)
        {
            if (li.Selected)
            {
                selBranch++;
                if (string.IsNullOrEmpty(newBranchCode.Trim()))
                {
                    newBranchCode = "'" + li.Value + "'";
                }
                else
                {
                    newBranchCode += ",'" + li.Value + "'";
                }
            }
        }
        if (selBranch > 0)
        {
            qryBranch = " and degree_code in(" + newBranchCode + ")";
        }
        string minattexam = d2.GetFunction("select distinct percent_eligible_for_exam from PeriodAttndSchedule where semester='" + ddlsemester.SelectedItem.ToString() + "' " + qryBranch + " order by percent_eligible_for_exam desc");
        txtminattpercentage.Text = string.Empty;
        if (minattexam.Trim() != "")
        {
            txtminattpercentage.Text = minattexam;
        }
        BindSection();
        clear();
    }

    protected void ddlsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    #region Added By Malang Raja On Oct 17 2016

    protected void chkDegree_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        int count = 0;
        if (chkDegree.Checked == true)
        {
            count++;
            for (int i = 0; i < cblDegree.Items.Count; i++)
            {
                cblDegree.Items[i].Selected = true;
            }
            txtDegree.Text = "Degree (" + (cblDegree.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblDegree.Items.Count; i++)
            {
                cblDegree.Items[i].Selected = false;
            }
            txtDegree.Text = "-- Select --";
        }
        bindbranch();
        bindsem();
        BindSection();
    }

    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        int commcount = 0;
        txtDegree.Text = "-- Select --";
        chkDegree.Checked = false;
        for (int i = 0; i < cblDegree.Items.Count; i++)
        {
            if (cblDegree.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cblDegree.Items.Count)
            {
                chkDegree.Checked = true;
            }
            txtDegree.Text = "Degree (" + Convert.ToString(commcount) + ")";
        }
        bindbranch();
        bindsem();
        BindSection();
    }

    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        if (chkBranch.Checked == true)
        {
            count++;
            for (int i = 0; i < cblBranch.Items.Count; i++)
            {
                cblBranch.Items[i].Selected = true;
            }
            txtBranch.Text = "Branch (" + (cblBranch.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblBranch.Items.Count; i++)
            {
                cblBranch.Items[i].Selected = false;
            }
            txtBranch.Text = "-- Select --";
        }
        bindsem();
        BindSection();
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        int commcount = 0;
        txtBranch.Text = "-- Select --";
        chkBranch.Checked = false;
        for (int i = 0; i < cblBranch.Items.Count; i++)
        {
            if (cblBranch.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cblBranch.Items.Count)
            {
                chkBranch.Checked = true;
            }
            txtBranch.Text = "Branch (" + Convert.ToString(commcount) + ")";
        }
        bindsem();
        BindSection();
    }

    protected void chkSec_CheckedChanged(object sender, EventArgs e)
    {
        clear();
        if (chkSec.Checked == true)
        {
            count++;
            for (int i = 0; i < cblSec.Items.Count; i++)
            {
                cblSec.Items[i].Selected = true;
            }
            txtSec.Text = "Section (" + (cblSec.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblSec.Items.Count; i++)
            {
                cblSec.Items[i].Selected = false;
            }
            txtSec.Text = "-- Select --";
        }
    }

    protected void cblSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        int commcount = 0;
        txtSec.Text = "-- Select --";
        chkSec.Checked = false;
        for (int i = 0; i < cblSec.Items.Count; i++)
        {
            if (cblSec.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cblSec.Items.Count)
            {
                chkSec.Checked = true;
            }
            txtSec.Text = "Section (" + Convert.ToString(commcount) + ")";
        }
    }

    #endregion

    protected void txtfdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            string fdate = txtfdate.Text.ToString();
            string[] spf = fdate.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            string tdate = txttodate.Text.ToString();
            string[] spt = tdate.Split('/');
            DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
            if (dtt < dtf)
            {
                txtfdate.Text = DateTime.Today.ToString("d/MM/yyyy");
                txttodate.Text = DateTime.Today.ToString("d/MM/yyyy");
                errmsg.Visible = true;
                errmsg.Text = "Please Enter The From Date Must Be Lesser Than Or Equl To Date";
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            string fdate = txtfdate.Text.ToString();
            string[] spf = fdate.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            string tdate = txttodate.Text.ToString();
            string[] spt = tdate.Split('/');
            DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
            if (dtt < dtf)
            {
                txtfdate.Text = DateTime.Today.ToString("d/MM/yyyy");
                txttodate.Text = DateTime.Today.ToString("d/MM/yyyy");
                errmsg.Visible = true;
                errmsg.Text = "Please Enter The From Date Must Be Lesser Than Or Equl To Date";
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            string stream = string.Empty;
            string qryedudegree = string.Empty;
            string strsec = string.Empty;
            string secval = string.Empty;
            lblErrCondo.Text = string.Empty;
            lblErrCondo.Visible = false;
            if (ddlstream.Items.Count > 0)
            {
                stream = Convert.ToString(ddlstream.SelectedItem.Text).Trim();
            }

            if (Convert.ToString(getCblSelectedValue(chklsbatch)) == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "No Batch Year Were Found";
                return;
            }
            else
            {
                newBatchYear = Convert.ToString(getCblSelectedValue(chklsbatch));
            }
            if (cblDegree.Items.Count == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = "No Degree Were Found";
                return;
            }
            selBranch = 0;
            qryBranch = string.Empty;
            newBranchCode = string.Empty;
            if (cblBranch.Items.Count == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = "No Branch Were Found";
                return;
            }
            else
            {
                foreach (ListItem li in cblBranch.Items)
                {
                    if (li.Selected)
                    {
                        selBranch++;
                        if (string.IsNullOrEmpty(newBranchCode.Trim()))
                        {
                            newBranchCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            newBranchCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (selBranch > 0)
                {
                    qryBranch = " and r.degree_code in(" + newBranchCode + ")";
                    qryedudegree = " and d.Degree_Code in(" + newBranchCode + ")";
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select Any One Branch And Then Proceed";
                    return;
                }
            }
            if (ddlsemester.Items.Count == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = "No Semester Were Found";
                return;
            }
            else
            {
                newsemester = Convert.ToString(ddlsemester.SelectedValue).Trim();
            }
            selSec = 0;
            newsections = string.Empty;
            qrySec = string.Empty;
            if (cblSec.Items.Count > 0)
            {
                foreach (ListItem li in cblSec.Items)
                {
                    if (li.Selected)
                    {
                        selBranch++;
                        if (string.IsNullOrEmpty(newsections.Trim()))
                        {
                            newsections = "'" + li.Value + "'";
                        }
                        else
                        {
                            newsections += ",'" + li.Value + "'";
                        }
                    }
                }
                if (selBranch > 0)
                {
                    qrySec = " and isnull(ltrim(rtrim(Sections)),'') in (" + newsections + ")";
                    strsec = " and isnull(ltrim(rtrim(Sections)),'') in (" + newsections + ")";
                    secval = " and isnull(ltrim(rtrim(r.Sections)),'') in (" + newsections + ")";
                }
            }
            string fdate = txtfdate.Text.ToString();
            string[] spf = fdate.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            string tdate = txttodate.Text.ToString();
            string[] spt = tdate.Split('/');
            DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
            if (dtt < dtf)
            {
                txtfdate.Text = DateTime.Today.ToString("d/MM/yyyy");
                txttodate.Text = DateTime.Today.ToString("d/MM/yyyy");
                errmsg.Visible = true;
                errmsg.Text = "Please Enter The From Date Must Be Lesser Than Or Equl To Date";
            }
            string exameligper = Convert.ToString(txtminattpercentage.Text).Trim();
            if (exameligper.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter The Min.Attendance % For Eligibility To Write Exam";
                return;
            }
            double eleigminattper = 0;// Convert.ToDouble(exameligper);
            double eligibleToWriteExam = 0;
            double.TryParse(exameligper.Trim(), out eligibleToWriteExam);
            if (eligibleToWriteExam > 100)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter The Min.Attendance % For Eligibility To Write Exam Lesser Than Or Equal 100";
                return;
            }
            bool isPercentage = false;
            if (rblHrDaywise.SelectedIndex == 0)
            {
                if (rblPercDays.SelectedIndex == 0)
                {
                    isPercentage = false;
                }
                else
                {
                    isPercentage = true;
                }
            }
            DataTable dtCondonation = new DataTable();
            //string degrecode = ddlbranch.SelectedValue.ToString();
            string getcoursetype = "select distinct isnull(Ltrim(rtrim(c.type)),'') as type,c.Edu_Level from Degree d,Course c where d.Course_Id=c.Course_Id " + qryedudegree + " and isnull(Ltrim(rtrim(c.type)),'')='" + stream + "'";
            DataSet dscourtype = d2.select_method_wo_parameter(getcoursetype, "Text");
            string typval = string.Empty;
            string qryType = string.Empty;
            string qryEdulevel = string.Empty;
            string eduLevelVal = string.Empty;
            //ArrayList arrEduLevel = new ArrayList();
            if (dscourtype.Tables.Count > 0 && dscourtype.Tables[0].Rows.Count > 0)
            {
                string edulevel = string.Empty;
                string type = string.Empty;
                for (int edu = 0; edu < dscourtype.Tables[0].Rows.Count; edu++)
                {
                    edulevel = Convert.ToString(dscourtype.Tables[0].Rows[edu]["Edu_Level"]).Trim();
                    type = Convert.ToString(dscourtype.Tables[0].Rows[edu]["type"]).Trim();
                    if (!string.IsNullOrEmpty(edulevel.Trim()))
                    {
                        string strattramquery1 = "select * from Condonation_Fee where college_code='" + collegecode + "' and edu_level='" + edulevel + "' and isnull(Ltrim(rtrim(Type)),'')='" + type + "'   and isnull(isDays,'0')='" + isPercentage + "'";
                        DataSet dsrange1 = d2.select_method_wo_parameter(strattramquery1, "Text");
                        if (dsrange1.Tables[0].Rows.Count == 0)
                        {
                            errmsg.Visible = true;
                            errmsg.Text = "Please Set Condonation Fee Settings to (" + type + " - " + edulevel + " ) " + ((isPercentage) ? "Days" : "Perentages") + " And Then Proceed";
                            return;
                        }
                        if (string.IsNullOrEmpty(typval.Trim()))
                        {
                            eduLevelVal = "'" + edulevel.Trim() + "'";
                        }
                        else
                        {
                            eduLevelVal += ",'" + edulevel.Trim() + "'";
                        }
                    }
                    if (!string.IsNullOrEmpty(type.Trim()))
                    {
                        if (string.IsNullOrEmpty(typval.Trim()))
                        {
                            typval = "'" + type.Trim() + "'";
                        }
                        else
                        {
                            typval += ",'" + type.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(eduLevelVal.Trim()))
                {
                    qryEdulevel = " and edu_level in(" + eduLevelVal.Trim() + ")";
                }
                if (!string.IsNullOrEmpty(typval.Trim()))
                {
                    qryType = " and isnull(Ltrim(rtrim(Type)),'') in(" + typval.Trim() + ")";
                }
                //if (type.Trim() != "" && type != null)
                //{
                //    typval = " and Type='" + type + "'";
                //}
            }
            string strattramquery = "select ROW_NUMBER() OVER(partition by edu_level order by Att_from desc) as Category,* from Condonation_Fee where college_code='" + collegecode + "' " + qryEdulevel + qryType + " and isnull(isDays,'0')='" + isPercentage + "' order by Att_from desc";
            DataSet dsrange = d2.select_method_wo_parameter(strattramquery, "Text");
            if (dsrange.Tables[0].Rows.Count == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Set Condonation Fee Settings to " + ((isPercentage) ? "Days" : "Perentages") + " And Then Proceed";
                return;
            }
            string sec = string.Empty;
            string strorder = "ORDER BY r.Roll_No";
            string serialno = d2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
            if (serialno.Trim() == "1")
            {
                strorder = "ORDER BY r.college_code,r.Batch_Year,c.type,c.Edu_Level desc,d.Degree_Code,r.Current_Semester,r.Sections,r.serialno";
            }
            else
            {
                string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                if (orderby_Setting == "0")
                {
                    strorder = "ORDER BY r.college_code,r.Batch_Year,c.type,c.Edu_Level desc,d.Degree_Code,r.Current_Semester,r.Sections,r.Roll_No";
                }
                else if (orderby_Setting == "1")
                {
                    strorder = "ORDER BY r.college_code,r.Batch_Year,c.type,c.Edu_Level desc,d.Degree_Code,r.Current_Semester,r.Sections,r.Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = "ORDER BY r.college_code,r.Batch_Year,c.type,c.Edu_Level desc,d.Degree_Code,r.Current_Semester,r.Sections,r.Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "ORDER BY r.college_code,r.Batch_Year,c.type,c.Edu_Level desc,d.Degree_Code,r.Current_Semester,r.Sections,r.Roll_No,r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "ORDER BY r.college_code,r.Batch_Year,c.type,c.Edu_Level desc,d.Degree_Code,r.Current_Semester,r.Sections,r.Roll_No,r.Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "ORDER BY r.college_code,r.Batch_Year,c.type,c.Edu_Level desc,d.Degree_Code,r.Current_Semester,r.Sections,r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "ORDER BY r.college_code,r.Batch_Year,c.type,c.Edu_Level desc,d.Degree_Code,r.Current_Semester,r.Sections,r.Roll_No,r.Stud_Name";
                }
            }
            //if (ddlsection.Items.Count > 0)
            //{
            //    sections = ddlsection.SelectedValue.ToString().Trim();
            //    if (sections.ToString().ToLower().Trim() == "all" || sections.ToString().ToLower().Trim() == string.Empty || sections.ToString().ToLower().Trim() == "-1")
            //    {
            //        strsec = string.Empty;
            //    }
            //    else
            //    {
            //        strsec = " and sections='" + sections.ToString() + "'";
            //        secval = " and r.sections='" + sections.ToString() + "'";
            //    }
            //}
            string strquery = " select isnull(ltrim(rtrim(c.type)),'') as type,c.Edu_Level,c.Course_Name,dt.Dept_Name,r.degree_code,r.Roll_No,r.Reg_No,r.App_No,r.Stud_Name,Convert(nvarchar(15),r.Adm_Date,103) as Adm_Date,r.Stud_Type,r.college_code,r.current_semester,isnull(ltrim(rtrim(r.Sections)),'') as Sections,case when isnull(ltrim(rtrim(r.Sections)),'')<>'' then (c.Edu_Level+' - '+Convert(varchar(50),r.Batch_Year)+' - ' +c.Course_Name+' - '+dt.Dept_Name+' - '+isnull(ltrim(rtrim(r.Sections)),'')) when isnull(ltrim(rtrim(r.Sections)),'')='' then (c.Edu_Level+' - '+Convert(varchar(50),r.Batch_Year)+' - '+c.Course_Name+' - '+dt.Dept_Name)end as Department_Details from Registration r,Course c,Degree d,Department dt where c.college_code=r.college_code  and r.college_code=d.college_code and dt.college_code=d.college_code and r.college_code=dt.college_code and d.college_code=c.college_code and dt.college_code=c.college_code and c.Course_Id=d.Course_Id and dt.Dept_Code=d.Dept_Code and r.degree_code=d.Degree_Code and Batch_Year in ('" + newBatchYear + "') " + qryBranch + " and Current_Semester='" + newsemester + "' " + secval + " and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' " + strorder + "";
            strquery += " select app_no,Remarks,Semester,batch_year,degree_code,is_eligible,ChallanDate,ChallanNo from Eligibility_list where batch_year in ('" + newBatchYear + "')  and degree_code in(" + newBranchCode + ")";//and Semester='" + newsemester + "'
            ds4 = d2.select_method_wo_parameter(strquery, "Text");
            splhr_flag = false;
            string strgetspaval = d2.GetFunction("select rights from  special_hr_rights where " + grouporusercode + "");
            if (strgetspaval.Trim() == "" || strgetspaval.Trim().ToLower() == "true")
            {
                splhr_flag = true;
            }

            #region 1st_Grid_StdWriteExam_Columns
            ArrayList headlist = new ArrayList();
            datastudentwriteexam.Columns.Add("S.No", typeof(string));
            headlist.Add("S.No");
            if (Session["Rollflag"].ToString() == "1")
            {
                datastudentwriteexam.Columns.Add("Roll.No", typeof(string));
                headlist.Add("Roll.No");
                colcnt++;
            }
            if (Session["Regflag"].ToString() == "1")
            {
                datastudentwriteexam.Columns.Add("Reg.No", typeof(string));
                headlist.Add("Reg.No");
                colcnt++;
            }
            if (Session["Studflag"].ToString() == "1")
            {
                datastudentwriteexam.Columns.Add("Student Type", typeof(string));
                headlist.Add("Student Type");
                colcnt++;
            }
            colcnt = colcnt + 2;
            datastudentwriteexam.Columns.Add("Student Name", typeof(string));
            datastudentwriteexam.Columns.Add("Present Percentage", typeof(string));
            datastudentwriteexam.Columns.Add("Absent Percentage", typeof(string));
            headlist.Add("Student Name");
            headlist.Add("Present Percentage");
            headlist.Add("Absent Percentage");

            if (rblHrDaywise.SelectedIndex == 1)
            {
                datastudentwriteexam.Columns.Add("Conducted Hours", typeof(string));
                datastudentwriteexam.Columns.Add("Present Hours", typeof(string));
                datastudentwriteexam.Columns.Add("Absent Hours", typeof(string));

                headlist.Add("Conducted Hours");
                headlist.Add("Present Hours");
                headlist.Add("Absent Hours");
            }
            else
            {
                datastudentwriteexam.Columns.Add("Conducted Days", typeof(string));
                datastudentwriteexam.Columns.Add("Present Days", typeof(string));
                datastudentwriteexam.Columns.Add("Absent Days", typeof(string));

                headlist.Add("Conducted Days");
                headlist.Add("Present Days");
                headlist.Add("Absent Days");
            }
            datarow = datastudentwriteexam.NewRow();
            datastudentwriteexam.Rows.Add();

            #endregion



            #region 2st_Grid_StdApply_Condonation
            ArrayList headlist2 = new ArrayList();
            dataApplyCondonation.Columns.Add("SNo", typeof(string));
            headlist2.Add("S.No");
            headlist2.Add("Select");
            headlist2.Add("Edit");

            if (Session["Rollflag"].ToString() == "1")
            {
                dataApplyCondonation.Columns.Add("Roll.No", typeof(string));
                headlist2.Add("Roll.No");
                colcnt1++;
            }
            if (Session["Regflag"].ToString() == "1")
            {
                dataApplyCondonation.Columns.Add("Reg.No", typeof(string));
                headlist2.Add("Reg.No");
                colcnt1++;
            }
            if (Session["Studflag"].ToString() == "1")
            {
                dataApplyCondonation.Columns.Add("Student Type", typeof(string));
                headlist2.Add("Student Type");
                colcnt1++;
            }
            colcnt1 = colcnt1 + 4;
            dataApplyCondonation.Columns.Add("Student Name", typeof(string));
            dataApplyCondonation.Columns.Add("Present Percentage", typeof(string));
            dataApplyCondonation.Columns.Add("Absent Percentage", typeof(string));

            headlist2.Add("Student Name");
            headlist2.Add("Present Percentage");
            headlist2.Add("Absent Percentage");
            #region Added by Idhris 15-10-2016
            if (rblCondType.SelectedIndex == 0)
            {
                dataApplyCondonation.Columns.Add("Fine Amount", typeof(string));
                headlist2.Add("Fine Amount");
            }

            #endregion
            if (rblHrDaywise.SelectedIndex == 1)
            {
                dataApplyCondonation.Columns.Add("Conducted Hours", typeof(string));
                dataApplyCondonation.Columns.Add("Present Hours", typeof(string));
                dataApplyCondonation.Columns.Add("Absent Hours", typeof(string));

                headlist2.Add("Conducted Hours");
                headlist2.Add("Present Hours");
                headlist2.Add("Absent Hours");

            }
            else
            {
                dataApplyCondonation.Columns.Add("Conducted Days", typeof(string));
                dataApplyCondonation.Columns.Add("Present Days", typeof(string));
                dataApplyCondonation.Columns.Add("Absent Days", typeof(string));

                headlist2.Add("Conducted Days");
                headlist2.Add("Present Days");
                headlist2.Add("Absent Days");

            }
            dataApplyCondonation.Columns.Add("Remarks", typeof(string));
            headlist2.Add("Remarks");

            datarowApplyCondonation = dataApplyCondonation.NewRow();
            dataApplyCondonation.Rows.Add(datarowApplyCondonation);
            #endregion


            dtCondonation.Columns.Clear();
            dtCondonation.Rows.Clear();
            dtCondonation.Columns.Add("App_No");
            dtCondonation.Columns.Add("RollNo");
            dtCondonation.Columns.Add("RegNo");
            dtCondonation.Columns.Add("Degree_Code");
            dtCondonation.Columns.Add("Degree_Details");
            dtCondonation.Columns.Add("Batch_Year");
            dtCondonation.Columns.Add("Edu_Level");
            dtCondonation.Columns.Add("Semester");
            dtCondonation.Columns.Add("Student_Type");
            dtCondonation.Columns.Add("Student_Name");
            dtCondonation.Columns.Add("Present_Percentage");
            dtCondonation.Columns.Add("Absent_Percentage");
            //"att_from", "att_from"
            dtCondonation.Columns.Add("att_from", typeof(double));
            dtCondonation.Columns.Add("Att_To", typeof(double));
            dtCondonation.Columns.Add("Fine_Amount");
            dtCondonation.Columns.Add("Conducted_Hours");
            dtCondonation.Columns.Add("Present_Hours");
            dtCondonation.Columns.Add("Absent_Hours");
            dtCondonation.Columns.Add("Conducted_Days");
            dtCondonation.Columns.Add("Present_Days");
            dtCondonation.Columns.Add("Absent_Days");
            dtCondonation.Columns.Add("HeaderID");
            dtCondonation.Columns.Add("FeeCode");
            dtCondonation.Columns.Add("Category", typeof(int));
            dtCondonation.Columns.Add("ChallanDate");
            dtCondonation.Columns.Add("ChallanNo");
            dtCondonation.Columns.Add("absentHoursPercentage1");
            dtCondonation.Columns.Add("absentDaysPercentage1");
            dtCondonation.Columns.Add("presentHoursPercentage1");
            dtCondonation.Columns.Add("presentDaysPercentage1");

            //to be Modified

            #region 3_rdGrid_For_NotEligible_Student

            ArrayList headlist3 = new ArrayList();
            datanotelg.Columns.Add("SNo", typeof(string));
            datanotelg.Columns.Add("Eligible For Next Semester", typeof(string));
            headlist3.Add("S.No");
            headlist3.Add("Eligible For Next Semester");

            if (Session["Rollflag"].ToString() == "1")
            {
                datanotelg.Columns.Add("Roll.No", typeof(string));
                headlist3.Add("Roll.No");
                colcnt3++;
            }
            if (Session["Regflag"].ToString() == "1")
            {
                datanotelg.Columns.Add("Reg.No", typeof(string));
                headlist3.Add("Reg.No");
                colcnt3++;
            }
            if (Session["Studflag"].ToString() == "1")
            {
                datanotelg.Columns.Add("Student Type", typeof(string));
                headlist3.Add("Student Type");
                colcnt3++;
            }
            colcnt3 = colcnt3 + 3;
            datanotelg.Columns.Add("Student Name", typeof(string));
            datanotelg.Columns.Add("Present Percentage", typeof(string));
            datanotelg.Columns.Add("Absent Percentage", typeof(string));
            headlist3.Add("Student Name");
            headlist3.Add("Present Percentage");
            headlist3.Add("Absent Percentage");

            if (rblHrDaywise.SelectedIndex == 1)
            {
                datanotelg.Columns.Add("Conducted Hours", typeof(string));
                datanotelg.Columns.Add("Present Hours", typeof(string));
                datanotelg.Columns.Add("Absent Hours", typeof(string));
                headlist3.Add("Conducted Hours");
                headlist3.Add("Present Hours");
                headlist3.Add("Absent Hours");
            }
            else
            {
                datanotelg.Columns.Add("Conducted Days", typeof(string));
                datanotelg.Columns.Add("Present Days", typeof(string));
                datanotelg.Columns.Add("Absent Days", typeof(string));

                headlist3.Add("Conducted Days");
                headlist3.Add("Present Days");
                headlist3.Add("Absent Days");
            }

            datanotelg.Columns.Add("Remarks", typeof(string));
            headlist3.Add("Remarks");


            dtrownotelg = datanotelg.NewRow();
            datanotelg.Rows.Add(dtrownotelg);
            #endregion

            frdate = Convert.ToString(txtfdate.Text);
            todate = Convert.ToString(txttodate.Text);
            string dt = frdate;
            string[] dsplit = dt.Split(new Char[] { '/' });
            frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
            demfcal = int.Parse(dsplit[2].ToString());
            demfcal = demfcal * 12;
            cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
            cal_from_date_tmp = demfcal + int.Parse(dsplit[1].ToString());
            monthcal = cal_from_date.ToString();
            dt = todate;
            dsplit = dt.Split(new Char[] { '/' });
            todate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
            demtcal = int.Parse(dsplit[2].ToString());
            demtcal = demtcal * 12;
            cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
            cal_to_date_tmp = demtcal + int.Parse(dsplit[1].ToString());
            per_from_gendate = Convert.ToDateTime(frdate);
            per_to_gendate = Convert.ToDateTime(todate);


            string[] al = new string[2];
            al[1] = "Eligible For Next Semester";
            al[0] = "Not Eligible";

            string[] aVal = new string[2];
            aVal[1] = "4";
            aVal[0] = "3";

            string eligiblitytype = string.Empty;
            int srno = 0, srcon = 0, nrsrno = 0;
            if (ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count > 0)
            {
                //FpSpread2.Sheets[0].RowCount++;
                //FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 7);
                //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].CellType = chkall;
                //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;
                //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                DataRow drCondo;
                PWrite.Visible = true;
                Showgrid.Visible = true;
                PCondonation.Visible = true;
                Showgrid1.Visible = true;
                PNotEligible.Visible = true;
                Showgrid2.Visible = true;
                btnsave.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                txtexcelname.Text = string.Empty;
                btnxl.Visible = true;
                btnprintmaster.Visible = true;
                lblconrptname.Visible = true;
                txtconexcel.Visible = true;
                txtconexcel.Text = string.Empty;
                btnconxl.Visible = true;
                btnconprint.Visible = true;
                btnCondonationReport.Visible = true;
                lblnotelexclname.Visible = true;
                txtnoteliexcel.Visible = true;
                txtnoteliexcel.Text = string.Empty;
                btnnoteliexcel.Visible = true;
                btnnoteliprint.Visible = true;
                ddlCondonationReport.Visible = Showgrid.Visible;
                ArrayList arrDegree = new ArrayList();
                ArrayList arrToWrite = new ArrayList();
                ArrayList arrToCondonation = new ArrayList();
                ArrayList arrToNotEligible = new ArrayList();
                for (rows_count = 0; rows_count < ds4.Tables[0].Rows.Count; rows_count++)
                {
                    string rollno = ds4.Tables[0].Rows[rows_count]["Roll_No"].ToString();
                    string regno = ds4.Tables[0].Rows[rows_count]["Reg_No"].ToString();
                    string studtype = ds4.Tables[0].Rows[rows_count]["Stud_Type"].ToString();
                    string studnmae = ds4.Tables[0].Rows[rows_count]["Stud_Name"].ToString();
                    string appno = ds4.Tables[0].Rows[rows_count]["app_no"].ToString();
                    string degcode = Convert.ToString(ds4.Tables[0].Rows[rows_count]["degree_code"]).Trim();
                    string semester = Convert.ToString(ds4.Tables[0].Rows[rows_count]["current_semester"]).Trim();
                    string departmentDetails = Convert.ToString(ds4.Tables[0].Rows[rows_count]["Department_Details"]).Trim();
                    string typeName = Convert.ToString(ds4.Tables[0].Rows[rows_count]["type"]).Trim();
                    string education = Convert.ToString(ds4.Tables[0].Rows[rows_count]["Edu_Level"]).Trim();
                    //persentmonthcal(); // Hidden By  Malang Raja on Oct 17 2016 Due to mismatch in Attendance Percentage For MCC
                    if (!arrDegree.Contains(degcode))
                    {
                        hat.Clear();
                        hat.Add("degree_code", degcode);
                        hat.Add("sem_ester", int.Parse(semester));
                        ds = d2.select_method("period_attnd_schedule", hat, "sp");
                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                            fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                            anhrs = int.Parse(ds.Tables[0].Rows[0]["II_HALF_DAY"].ToString());
                            minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                            minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                            minpresday = int.Parse(ds.Tables[0].Rows[0]["MIN PREE PER DAY"].ToString());
                        }
                        hat.Clear();
                        hat.Add("colege_code", Session["collegecode"].ToString());
                        ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
                        count = ds1.Tables[0].Rows.Count;
                        arrDegree.Add(degcode);
                    }
                    string collegeCodeP = ds4.Tables[0].Rows[rows_count]["college_code"].ToString(), degreeP = degcode, semP = semester, rollnoP = rollno, admDateP = ds4.Tables[0].Rows[rows_count]["adm_date"].ToString();
                    persentmonthcal(collegeCodeP, degreeP, semP, rollnoP, admDateP);
                    double conductedDays = 0;
                    double presentDays = 0;
                    double absentDays = 0;
                    double conductedHours = 0;
                    double presentHours = 0;
                    double absentHours = 0;
                    double absentDaysPercentage = 0;
                    double absentHoursPercentage = 0;
                    string absentDaysPercentage1 = string.Empty;
                    string absentHoursPercentage1 = string.Empty;
                    string dum_tage_date = "", dum_tage_hrs;
                    conductedDays = per_workingdays;
                    presentDays = pre_present_date;
                    if (per_workingdays > 0)
                    {
                        absentDays = per_workingdays - pre_present_date;
                    }
                    if (per_workingdays > 0)
                    {
                        per_tage_date = ((pre_present_date / per_workingdays) * 100);
                        absentDaysPercentage = ((absentDays / per_workingdays) * 100);
                    }
                    else
                    {
                        per_tage_date = 0;
                        absentDaysPercentage = 0;
                    }
                    conductedHours = per_workingdays1;
                    presentHours = per_per_hrs;
                    if (per_workingdays1 > 0)
                    {
                        absentHours = per_workingdays1 - per_per_hrs;
                    }
                    if (per_workingdays1 > 0)
                    {
                        per_tage_hrs = (((per_per_hrs) / (per_workingdays1)) * 100);
                        absentHoursPercentage = ((absentHours / per_workingdays1) * 100);
                    }
                    else
                    {
                        per_tage_hrs = 0;
                        absentHoursPercentage = 0;
                    }
                    if (per_tage_date.ToString() == "NaN")
                    {
                        per_tage_date = 0;
                    }
                    else if (per_tage_date.ToString() == "Infinity")
                    {
                        per_tage_date = 0;
                    }
                    if (per_tage_date > 100)
                    {
                        per_tage_date = 100;
                    }
                    per_tage_date = Math.Round(per_tage_date, 2);
                    absentDaysPercentage = Math.Round(absentDaysPercentage, 2);
                    per_tage_hrs = Math.Round(per_tage_hrs, 2);
                    absentHoursPercentage = Math.Round(absentHoursPercentage, 2);
                    //dum_tage_hrs = per_tage_hrs.ToString();
                    dum_tage_date = String.Format("{0:0,0.00}", float.Parse(Convert.ToString(per_tage_date).Trim()));
                    dum_tage_hrs = String.Format("{0:0,0.00}", float.Parse(Convert.ToString(per_tage_hrs).Trim()));
                    absentDaysPercentage1 = String.Format("{0:0,0.00}", float.Parse(Convert.ToString(absentDaysPercentage).Trim()));
                    absentHoursPercentage1 = String.Format("{0:0,0.00}", float.Parse(Convert.ToString(absentHoursPercentage).Trim()));

                    //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(absentHoursPercentage1);
                    //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Note = Convert.ToString(absentDaysPercentage1);

                    //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(dum_tage_hrs);
                    //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Note = Convert.ToString(dum_tage_date);

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
                    double compareValue = 0;
                    if (rblHrDaywise.SelectedIndex == 0)
                    {
                        if (rblPercDays.SelectedIndex == 0)
                        {
                            compareValue = per_tage_date;
                        }
                        else
                        {
                            compareValue = presentDays;
                        }
                    }
                    else
                    {
                        compareValue = per_tage_hrs;
                    }
                    if (eligibleToWriteExam <= compareValue)//==============Eligible To Write Exam===================
                    {
                        srno++;

                        if (!arrToWrite.Contains(departmentDetails))
                        {

                            datarow = datastudentwriteexam.NewRow();
                            datarow["S.No"] = departmentDetails.ToString();
                            datastudentwriteexam.Rows.Add(datarow);
                            arrToWrite.Add(departmentDetails);
                            dicstucolspan.Add(datastudentwriteexam.Rows.Count - 1, departmentDetails);
                        }

                        datarow = datastudentwriteexam.NewRow();
                        datarow["S.No"] = srno.ToString();
                        if (Session["Rollflag"].ToString() == "1")
                            datarow["Roll.No"] = rollno;
                        if (Session["Regflag"].ToString() == "1")
                            datarow["Reg.No"] = regno;
                        if (Session["Studflag"].ToString() == "1")
                            datarow["Student Type"] = studtype;
                        datarow["Student Name"] = studnmae;

                        //added by sudhagar
                        double roundPer = 0;

                        if (rblHrDaywise.SelectedIndex == 0)
                        {
                            if (cbincround.Checked)
                                double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(dum_tage_date))), out roundPer);
                            else
                                double.TryParse(Convert.ToString(dum_tage_date), out roundPer);
                            datarow["Present Percentage"] = Convert.ToString(roundPer).Trim();

                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dum_tage_date).Trim();
                        }
                        else
                        {
                            if (cbincround.Checked)
                                double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(dum_tage_hrs))), out roundPer);
                            else
                                double.TryParse(Convert.ToString(dum_tage_hrs), out roundPer);
                            datarow["Present Percentage"] = Convert.ToString(roundPer).Trim();

                            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dum_tage_hrs).Trim();
                        }

                        double roundPers = 0;

                        if (rblHrDaywise.SelectedIndex == 0)
                        {
                            if (cbincround.Checked)
                                double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(absentDaysPercentage1))), out roundPers);
                            else
                                double.TryParse(Convert.ToString(absentDaysPercentage1), out roundPers);
                            datarow["Absent Percentage"] = Convert.ToString(roundPers).Trim();

                            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(absentDaysPercentage1).Trim();
                        }
                        else
                        {
                            if (cbincround.Checked)
                                double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(dum_tage_hrs))), out roundPers);
                            else
                                double.TryParse(Convert.ToString(dum_tage_hrs), out roundPers);

                            datarow["Absent Percentage"] = Convert.ToString(roundPers).Trim();

                            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dum_tage_hrs).Trim();
                        }

                        if (rblHrDaywise.SelectedIndex == 1)
                        {

                            datarow["Conducted Hours"] = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(conductedHours) : Convert.ToString(conductedDays));
                            datarow["Present Hours"] = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(presentHours) : Convert.ToString(presentDays));
                            datarow["Absent Hours"] = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(absentHours) : Convert.ToString(absentDays));
                        }
                        else
                        {
                            datarow["Conducted Days"] = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(conductedHours) : Convert.ToString(conductedDays));
                            datarow["Present Days"] = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(presentHours) : Convert.ToString(presentDays));
                            datarow["Absent Days"] = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(absentHours) : Convert.ToString(absentDays));
                        }

                        datastudentwriteexam.Rows.Add(datarow);
                        string stdtails = rollno + "-" + appno + "-" + degcode + "-" + newBatchYear + "-" + semester + "-" + Convert.ToString(absentHoursPercentage1) + "-" + Convert.ToString(absentDaysPercentage1) + "-" + Convert.ToString(dum_tage_hrs) + "-" + Convert.ToString(dum_tage_date) + "-" + Convert.ToString(conductedHours) + "-" + Convert.ToString(conductedDays) + "-" + Convert.ToString(presentHours) + "-" + Convert.ToString(presentDays) + "-" + Convert.ToString(absentHours) + "-" + Convert.ToString(absentDays);
                        dicstudetails.Add(srno, stdtails);


                    }
                    else
                    {
                        dsrange.Tables[0].DefaultView.RowFilter = "att_from<= '" + compareValue + "' and att_to>= '" + compareValue + "' and Type='" + typeName + "' and edu_level='" + education + "'";
                        DataView dvfein = dsrange.Tables[0].DefaultView;
                        dvfein.Sort = "att_from desc";
                        #region Barath Remark added 25.07.17
                        ds4.Tables[1].DefaultView.RowFilter = " app_no='" + appno + "' and batch_year in ('" + newBatchYear + "') and Semester='" + semester + "' and degree_code='" + degcode + "'";
                        DataView RemarkDv = ds4.Tables[1].DefaultView;
                        string Remarks = string.Empty;
                        if (RemarkDv.Count > 0)
                            Remarks = Convert.ToString(RemarkDv[0]["Remarks"]) == "CAG" ? Convert.ToString(RemarkDv[0]["Remarks"]) + ",NE" : Convert.ToString(RemarkDv[0]["Remarks"]);
                        //Remarks = Convert.ToString(RemarkDv[0]["Remarks"]);
                        if (string.IsNullOrEmpty(Remarks))
                        {
                            ds4.Tables[1].DefaultView.RowFilter = " app_no='" + appno + "' and batch_year in ('" + newBatchYear + "') and Semester='" + (Convert.ToInt32(semester) - 1) + "' and degree_code='" + degcode + "'";
                            RemarkDv = ds4.Tables[1].DefaultView;
                            if (RemarkDv.Count > 0)
                                Remarks = Convert.ToString(RemarkDv[0]["Remarks"]).ToUpper() == "CAG" ? Convert.ToString(RemarkDv[0]["Remarks"]) + ",NE" : Convert.ToString(RemarkDv[0]["Remarks"]); //modified
                            //Remarks = Convert.ToString(RemarkDv[0]["Remarks"]);
                        }

                        #endregion
                        if (dvfein.Count > 0)//==============Eligible To Condonation Apply===================
                        {
                            srcon++;
                            drCondo = dtCondonation.NewRow();
                            drCondo["App_No"] = appno;
                            drCondo["RollNo"] = rollno;
                            drCondo["RegNo"] = regno;
                            drCondo["Degree_Code"] = degcode;
                            drCondo["Degree_Details"] = Convert.ToString(departmentDetails).Trim();
                            drCondo["Batch_Year"] = newBatchYear;
                            drCondo["Semester"] = semester;
                            drCondo["Student_Type"] = studtype;
                            drCondo["Student_Name"] = studnmae;
                            drCondo["Edu_Level"] = education;
                            if (rblHrDaywise.SelectedIndex == 0)
                            {
                                //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dum_tage_date).Trim();
                                drCondo["Present_Percentage"] = Convert.ToString(dum_tage_date).Trim();
                            }
                            else
                            {
                                //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dum_tage_hrs).Trim();
                                drCondo["Present_Percentage"] = Convert.ToString(dum_tage_hrs).Trim();
                            }
                            drCondo["Absent_Percentage"] = string.Empty;
                            if (rblHrDaywise.SelectedIndex == 0)
                            {
                                // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(absentDaysPercentage1).Trim();
                                drCondo["Absent_Percentage"] = Convert.ToString(absentDaysPercentage1).Trim();
                            }
                            else
                            {
                                // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(absentHoursPercentage1).Trim();
                                drCondo["Absent_Percentage"] = Convert.ToString(absentHoursPercentage1).Trim();
                            }

                            drCondo["absentHoursPercentage1"] = Convert.ToString(absentHoursPercentage1).Trim();
                            drCondo["absentDaysPercentage1"] = Convert.ToString(absentDaysPercentage1).Trim();
                            drCondo["presentHoursPercentage1"] = Convert.ToString(dum_tage_hrs).Trim();
                            drCondo["presentDaysPercentage1"] = Convert.ToString(dum_tage_date).Trim();

                            drCondo["att_from"] = Convert.ToString(dvfein[0]["att_from"]).Trim();
                            drCondo["att_to"] = Convert.ToString(dvfein[0]["Att_To"]).Trim();
                            drCondo["Fine_Amount"] = Convert.ToString(dvfein[0]["Fine_Amt"]).Trim();
                            drCondo["Conducted_Hours"] = Convert.ToString(conductedHours).Trim();
                            drCondo["Present_Hours"] = Convert.ToString(presentHours).Trim();
                            drCondo["Absent_Hours"] = Convert.ToString(absentHours).Trim();
                            drCondo["Conducted_Days"] = Convert.ToString(conductedDays).Trim();
                            drCondo["Present_Days"] = Convert.ToString(presentDays).Trim();
                            drCondo["Absent_Days"] = Convert.ToString(absentDays).Trim();
                            drCondo["HeaderID"] = Convert.ToString(dvfein[0]["Header_id"]).Trim();
                            drCondo["FeeCode"] = Convert.ToString(dvfein[0]["Fee_code"]).Trim();
                            drCondo["Category"] = Convert.ToString(dvfein[0]["Category"]).Trim();
                            dtCondonation.Rows.Add(drCondo);

                            #region Cmd_Line
                            //FpSpread2.Sheets[0].RowCount++;
                            //if (!arrToCondonation.Contains(Convert.ToString(departmentDetails).Trim()))
                            //{
                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].CellType = txt;
                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(departmentDetails).Trim();
                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].BackColor = System.Drawing.Color.DarkGray;
                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Tag = "-1";
                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;
                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            //    FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 13);
                            //    arrToCondonation.Add(Convert.ToString(departmentDetails).Trim());
                            //    FpSpread2.Sheets[0].RowCount++;
                            //}
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].CellType = txt;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = srcon.ToString();
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dvfein[0]["Header_id"]).Trim();
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(dvfein[0]["Fee_code"]).Trim();
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = txt;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = rollno;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Tag = appno;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Note = degcode;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = true;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = regno;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = newBatchYear;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Note = semester;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = txt;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Locked = true;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = txt;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = studtype;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(departmentDetails).Trim();
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Locked = true;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].CellType = txt;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = studnmae;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Locked = true;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = txt;
                            //if (rblHrDaywise.SelectedIndex == 0)
                            //{
                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text =  Convert.ToString(dum_tage_date).Trim();
                            //}
                            //else
                            //{
                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString( dum_tage_hrs).Trim();
                            //}
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Locked = true;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].CellType = txt;
                            //if (rblHrDaywise.SelectedIndex == 0)
                            //{
                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(absentDaysPercentage1).Trim();
                            //}
                            //else
                            //{
                            //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(absentHoursPercentage1).Trim();
                            //}
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Locked = true;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].CellType = txt;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = dvfein[0]["Fine_Amt"].ToString();
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Locked = true;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;

                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].CellType = txt;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(conductedHours) : Convert.ToString(conductedDays));
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Locked = true;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].CellType = txt;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(presentHours) : Convert.ToString(presentDays));
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Locked = true;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].VerticalAlign = VerticalAlign.Middle;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].CellType = txt;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Text = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(absentHours) : Convert.ToString(absentDays));
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Locked = true;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].VerticalAlign = VerticalAlign.Middle;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].CellType = txt;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Text = string.Empty;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Locked = true;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Left;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].VerticalAlign = VerticalAlign.Middle;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                            ////FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].CellType = chk;
                            ////FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 12].CellType = cbEach;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Center;
                            //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 12].VerticalAlign = VerticalAlign.Middle;
                            #endregion
                        }
                        else//==============Not Eligible To Write Exam===================
                        {

                            nrsrno++;


                            if (!arrToNotEligible.Contains(Convert.ToString(departmentDetails).Trim()))
                            {
                                dtrownotelg = datanotelg.NewRow();
                                dtrownotelg["SNo"] = departmentDetails.ToString();
                                datanotelg.Rows.Add(dtrownotelg);
                                arrToNotEligible.Add(Convert.ToString(departmentDetails).Trim());
                                dicstunotelgcolspan.Add(datanotelg.Rows.Count - 1, departmentDetails);
                            }
                            dtrownotelg = datanotelg.NewRow();
                            dtrownotelg["SNo"] = nrsrno.ToString();
                            if (Session["Rollflag"].ToString() == "1")
                                dtrownotelg["Roll.No"] = rollno;
                            if (Session["Regflag"].ToString() == "1")
                                dtrownotelg["Reg.No"] = regno;
                            if (Session["Studflag"].ToString() == "1")
                                dtrownotelg["Student Type"] = studtype;
                            dtrownotelg["Student Name"] = studnmae;

                            double roundPers = 0;

                            if (rblHrDaywise.SelectedIndex == 0)
                            {
                                if (cbincround.Checked)
                                    double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(dum_tage_date))), out roundPers);
                                else
                                    double.TryParse(Convert.ToString(dum_tage_date), out roundPers);

                                dtrownotelg["Present Percentage"] = Convert.ToString(roundPers).Trim();
                            }
                            else
                            {
                                if (cbincround.Checked)
                                    double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(dum_tage_hrs))), out roundPers);
                                else
                                    double.TryParse(Convert.ToString(dum_tage_hrs), out roundPers);
                                dtrownotelg["Present Percentage"] = Convert.ToString(roundPers).Trim();

                            }

                            double roundPer = 0;

                            if (rblHrDaywise.SelectedIndex == 0)
                            {
                                if (cbincround.Checked)
                                    double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(absentDaysPercentage1))), out roundPer);
                                else
                                    double.TryParse(Convert.ToString(absentDaysPercentage1), out roundPer);
                                dtrownotelg["Absent Percentage"] = Convert.ToString(roundPer).Trim();

                            }
                            else
                            {
                                if (cbincround.Checked)
                                    double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(absentHoursPercentage1))), out roundPer);
                                else
                                    double.TryParse(Convert.ToString(absentHoursPercentage1), out roundPer);
                                dtrownotelg["Absent Percentage"] = Convert.ToString(roundPer).Trim();

                            }

                            if (rblHrDaywise.SelectedIndex == 1)
                            {
                                dtrownotelg["Conducted Hours"] = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(conductedHours) : Convert.ToString(conductedDays));
                                dtrownotelg["Present Hours"] = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(presentHours) : Convert.ToString(presentDays));
                                dtrownotelg["Absent Hours"] = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(absentHours) : Convert.ToString(absentDays));
                            }
                            else
                            {
                                dtrownotelg["Conducted Days"] = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(conductedHours) : Convert.ToString(conductedDays));
                                dtrownotelg["Present Days"] = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(presentHours) : Convert.ToString(presentDays));
                                dtrownotelg["Absent Days"] = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(absentHours) : Convert.ToString(absentDays));

                            }
                            dtrownotelg["Remarks"] = Remarks;
                            string eligiblevalue = dir.selectScalarString("select is_eligible from Eligibility_list where app_no='" + appno + "' and Semester='" + semester + "' and batch_year in ('" + newBatchYear + "')");
                            if (eligiblevalue != "")
                                dtrownotelg["Eligible For Next Semester"] = Convert.ToString(eligiblevalue) == "3" ? "Not Eligible" : "Eligible For Next Semester";


                            datanotelg.Rows.Add(dtrownotelg);

                            //(Showgrid2.Rows[nrsrno].FindControl("ddl_Eligible") as DropDownList).Text = Convert.ToString(eligiblevalue) == "3" ? "Not Eligible" : "Eligible For Next Semester";//
                            // (Showgrid2.Rows[nrsrno].FindControl("ddl_Eligible") as DropDownList).Items.Clear();


                            //(Showgrid2.Rows[nrsrno].FindControl("ddlmethod") as DropDownList).DataTextField = "textval";
                            //(Showgrid2.Rows[nrsrno].FindControl("ddl_Eligible") as DropDownList).DataBind();                            

                            //(Showgrid2.Rows[nrsrno].FindControl("lbl_remark") as Label).Text = Convert.ToString(eligiblevalue);
                            string studtails = rollno + "-" + appno + "-" + degcode + "-" + newBatchYear + "-" + semester + "-" + Convert.ToString(absentHoursPercentage1) + "-" + Convert.ToString(absentDaysPercentage1) + "-" + Convert.ToString(dum_tage_hrs) + "-" + Convert.ToString(dum_tage_date) + "-" + Convert.ToString(conductedHours) + "-" + Convert.ToString(conductedDays) + "-" + Convert.ToString(presentHours) + "-" + Convert.ToString(presentDays) + "-" + Convert.ToString(absentHours) + "-" + Convert.ToString(absentDays);
                            dicstudnotelg.Add(nrsrno, studtails);

                        }
                    }
                }
                if (dtCondonation.Rows.Count > 0)
                {

                    DataTable dtCategory = new DataTable();
                    DataTable dtEduLevel = new DataTable();
                    DataView dvCondonation = new DataView();
                    dtEduLevel = dtCondonation.DefaultView.ToTable(true, "edu_level");
                    foreach (DataRow dr in dtEduLevel.Rows)
                    {
                        string eduLevel = Convert.ToString(dr["edu_level"]).Trim();
                        dtCondonation.DefaultView.RowFilter = "edu_level='" + Convert.ToString(dr["edu_level"]).Trim() + "'";
                        DataTable dtNewCondo = dtCondonation.DefaultView.ToTable();
                        if (dtNewCondo.Rows.Count > 0)
                        {
                            dtCategory.Columns.Add("Rows", typeof(int));
                            dtCategory.Columns["Rows"].AutoIncrement = true;
                            dtCategory.Columns["Rows"].AutoIncrementSeed = 1;
                            dtCategory.Columns["Rows"].AutoIncrementStep = 1;
                            dtCategory = dtNewCondo.DefaultView.ToTable(true, "Category", "edu_level", "att_from", "att_to");
                            DataView dvN = dtCategory.DefaultView;
                            dvN.Sort = "Category asc";
                            int catogory = 1;
                            //dsrange.Tables[0].DefaultView.RowFilter = "att_from<= '" + compareValue + "' and att_to>= '" + compareValue + "' and Type='" + typeName + "' and edu_level='" + education + "'";
                            //DataView dvfein = dsrange.Tables[0].DefaultView;
                            //dvfein.Sort = "att_from desc";
                            foreach (DataRowView dredu in dvN)
                            {

                                catogory++;
                                arrToCondonation.Clear();
                                int serialNo = 0;
                                string attFrom = Convert.ToString(dredu["att_from"]).Trim();
                                string attTo = Convert.ToString(dredu["att_to"]).Trim();
                                dtNewCondo.DefaultView.RowFilter = "Present_Percentage>='" + attFrom + "' and Present_Percentage<='" + attTo + "'";
                                dvCondonation = new DataView();
                                dvCondonation = dtNewCondo.DefaultView;
                                if (dvCondonation.Count > 0)
                                {
                                    datarowApplyCondonation = dataApplyCondonation.NewRow();
                                    datarowApplyCondonation["SNo"] = "Category - " + Convert.ToString(catogory).Trim();
                                    dataApplyCondonation.Rows.Add(datarowApplyCondonation);


                                    foreach (DataRowView dvCondo in dvCondonation)
                                    {
                                        string appNo = Convert.ToString(dvCondo["App_No"]).Trim();
                                        string rollNo = Convert.ToString(dvCondo["RollNo"]).Trim();
                                        string regNo = Convert.ToString(dvCondo["RegNo"]).Trim();
                                        string degreeCode = Convert.ToString(dvCondo["Degree_Code"]).Trim();
                                        string degreeDetails = Convert.ToString(dvCondo["Degree_Details"]).Trim();
                                        string batchYear = Convert.ToString(dvCondo["Batch_Year"]).Trim();
                                        string semester = Convert.ToString(dvCondo["Semester"]).Trim();
                                        string studentType = Convert.ToString(dvCondo["Student_Type"]).Trim();
                                        string studentName = Convert.ToString(dvCondo["Student_Name"]).Trim();
                                        string presentPercentage = Convert.ToString(dvCondo["Present_Percentage"]).Trim();
                                        string absendPercentage = Convert.ToString(dvCondo["Absent_Percentage"]).Trim();
                                        string fineAmount = Convert.ToString(dvCondo["Fine_Amount"]).Trim();
                                        string conductedHours = Convert.ToString(dvCondo["Conducted_Hours"]).Trim();
                                        string presentHours = Convert.ToString(dvCondo["Present_Hours"]).Trim();
                                        string absentHours = Convert.ToString(dvCondo["Absent_Hours"]).Trim();
                                        string conductedDays = Convert.ToString(dvCondo["Conducted_Days"]).Trim();
                                        string presentDays = Convert.ToString(dvCondo["Present_Days"]).Trim();
                                        string absentDays = Convert.ToString(dvCondo["Absent_Days"]).Trim();
                                        string HeaderID = Convert.ToString(dvCondo["HeaderID"]).Trim();
                                        string FeeCode = Convert.ToString(dvCondo["FeeCode"]).Trim();
                                        string Category = Convert.ToString(dvCondo["Category"]).Trim();

                                        string absentHoursPercentage1 = Convert.ToString(dvCondo["absentHoursPercentage1"]).Trim();
                                        string absentDaysPercentage1 = Convert.ToString(dvCondo["absentDaysPercentage1"]).Trim();
                                        string presentHoursPercentage1 = Convert.ToString(dvCondo["presentHoursPercentage1"]).Trim();
                                        string presentDaysPercentage1 = Convert.ToString(dvCondo["presentDaysPercentage1"]).Trim();

                                        string condqry = "select convert(varchar(20), ChallanDate ,103 ) as ChallanDate ,ChallanNo from Eligibility_list where app_no='" + appNo + "' and Semester='" + semester + "' and batch_year in ('" + batchYear + "') and degree_code='" + degreeCode + "' and is_eligible='2'";
                                        DataTable dtCondonationApplied = dir.selectDataTable(condqry);

                                        #region Barath Remark Added 25.07.17
                                        ds4.Tables[1].DefaultView.RowFilter = " app_no='" + appNo + "' and batch_year in ('" + batchYear + "') and Semester='" + semester + "' and degree_code='" + degreeCode + "'";
                                        DataView RemarkDv = ds4.Tables[1].DefaultView;
                                        string Remarks = string.Empty;
                                        if (RemarkDv.Count > 0)
                                            Remarks = Convert.ToString(RemarkDv[0]["Remarks"]).ToUpper() == "CAG" ? Convert.ToString(RemarkDv[0]["Remarks"]) + ",NE" : Convert.ToString(RemarkDv[0]["Remarks"]); //modified
                                        //Remarks = Convert.ToString(RemarkDv[0]["Remarks"]);
                                        if (string.IsNullOrEmpty(Remarks))
                                        {
                                            ds4.Tables[1].DefaultView.RowFilter = " app_no='" + appNo + "' and batch_year in ('" + batchYear + "')  and degree_code='" + degreeCode + "'";//and Semester='" + (Convert.ToInt32(semester) - 1) + "'
                                            RemarkDv = ds4.Tables[1].DefaultView;
                                            //if (RemarkDv.Count > 0)//13.10.17
                                            //    Remarks = Convert.ToString(RemarkDv[0]["Remarks"]);
                                            DataTable RemarkTD = new DataTable();
                                            RemarkTD = RemarkDv.ToTable();
                                            foreach (DataRow Rm in RemarkTD.Rows)
                                            {
                                                string PrevString = Convert.ToString(Rm["Remarks"]);
                                                if (!string.IsNullOrEmpty(PrevString) && PrevString.ToUpper() != "NE")
                                                    Remarks = Convert.ToString(RemarkDv[0]["Remarks"]).ToUpper() == "CAG" ? Convert.ToString(RemarkDv[0]["Remarks"]) + ",NE" : Convert.ToString(RemarkDv[0]["Remarks"]);//modified
                                                //Remarks = Convert.ToString(RemarkDv[0]["Remarks"]);
                                            }
                                        }
                                        #endregion

                                        serialNo++;
                                        if (!arrToCondonation.Contains(Convert.ToString(degreeDetails).Trim()))
                                        {
                                            datarowApplyCondonation = dataApplyCondonation.NewRow();
                                            datarowApplyCondonation["SNo"] = Convert.ToString(degreeDetails).Trim();
                                            dataApplyCondonation.Rows.Add(datarowApplyCondonation);

                                            arrToCondonation.Add(Convert.ToString(degreeDetails).Trim());
                                            dicstuApplyCondonationcolspan.Add(dataApplyCondonation.Rows.Count - 1, degreeDetails);
                                        }

                                        datarowApplyCondonation = dataApplyCondonation.NewRow();

                                        datarowApplyCondonation["SNo"] = serialNo;
                                        if (Session["Rollflag"].ToString() == "1")
                                            datarowApplyCondonation["Roll.No"] = rollNo;
                                        if (Session["Regflag"].ToString() == "1")
                                            datarowApplyCondonation["Reg.No"] = regNo;
                                        if (Session["Studflag"].ToString() == "1")
                                            datarowApplyCondonation["Student Type"] = studentType;
                                        datarowApplyCondonation["Student Name"] = studentName;

                                        double roundPer = 0;
                                        if (cbincround.Checked)
                                            double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(presentPercentage))), out roundPer);
                                        else
                                            double.TryParse(Convert.ToString(presentPercentage), out roundPer);
                                        datarowApplyCondonation["Present Percentage"] = Convert.ToString(roundPer).Trim();

                                        double roundPers = 0;
                                        if (cbincround.Checked)
                                            double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(absendPercentage))), out roundPers);
                                        else
                                            double.TryParse(Convert.ToString(absendPercentage), out roundPers);
                                        datarowApplyCondonation["Absent Percentage"] = Convert.ToString(roundPers).Trim();



                                        if (rblCondType.SelectedIndex == 0)
                                            datarowApplyCondonation["Fine Amount"] = Convert.ToString(fineAmount).Trim();

                                        if (rblHrDaywise.SelectedIndex == 1)
                                        {
                                            datarowApplyCondonation["Conducted Hours"] = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(conductedHours) : Convert.ToString(conductedDays));
                                            datarowApplyCondonation["Present Hours"] = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(presentHours) : Convert.ToString(presentDays));
                                            datarowApplyCondonation["Absent Hours"] = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(absentHours) : Convert.ToString(absentDays));


                                        }
                                        else
                                        {
                                            datarowApplyCondonation["Conducted Days"] = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(conductedHours) : Convert.ToString(conductedDays));
                                            datarowApplyCondonation["Present Days"] = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(presentHours) : Convert.ToString(presentDays));
                                            datarowApplyCondonation["Absent Days"] = ((rblHrDaywise.SelectedIndex == 1) ? Convert.ToString(absentHours) : Convert.ToString(absentDays));


                                        }

                                        dataApplyCondonation.Rows.Add(datarowApplyCondonation);

                                        string stdtail = rollNo + "$" + Convert.ToString(HeaderID).Trim() + "$" + Convert.ToString(FeeCode).Trim() + "$" + appNo + "$" + degreeCode + "$" + batchYear + "$" + semester + "$" + Convert.ToString(degreeDetails).Trim() + "$" + Convert.ToString(absentHoursPercentage1) + "$" + Convert.ToString(absentDaysPercentage1) + "$" + Convert.ToString(presentHoursPercentage1) + "$" + Convert.ToString(presentDaysPercentage1) + "$" + Convert.ToString(conductedHours) + "$" + Convert.ToString(conductedDays) + "$" + Convert.ToString(presentHours) + "$" + Convert.ToString(presentDays) + "$" + Convert.ToString(absentHours) + "$" + Convert.ToString(absentDays) + "$" + regNo;
                                        dicstudApplyCondonation.Add(dataApplyCondonation.Rows.Count - 1, stdtail);

                                        if (dtCondonationApplied.Rows.Count > 0)
                                        {
                                            dicCondonationappcnt.Add(dataApplyCondonation.Rows.Count - 1, Convert.ToString(dtCondonationApplied.Rows[0]["ChallanDate"]) + "$" + Convert.ToString(dtCondonationApplied.Rows[0]["ChallanNo"]));


                                        }
                                        else
                                        {
                                            dicstuApplyCondonationedit.Add(dataApplyCondonation.Rows.Count - 1, "2");


                                        }
                                    }

                                }
                            }
                        }
                    }
                }

                if (datastudentwriteexam.Columns.Count > 0)
                {

                    int col = datastudentwriteexam.Columns.Count;
                    if (datastudentwriteexam.Columns.Count > 0 && datastudentwriteexam.Rows.Count > 1)
                    {
                        Showgrid.DataSource = datastudentwriteexam;
                        Showgrid.DataBind();
                        Showgrid.Visible = true;

                        for (int grCol = 0; grCol < Showgrid.HeaderRow.Cells.Count; grCol++)
                            Showgrid.HeaderRow.Cells[grCol].Visible = false;
                        for (int i = 0; i < Showgrid.HeaderRow.Cells.Count; i++)
                        {
                            Showgrid.Rows[0].Cells[i].Text = Convert.ToString(headlist[i]);
                        }
                        Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                        Showgrid.Rows[0].Font.Name = "Book Anitique";
                        Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        Showgrid.Rows[0].Font.Bold = true;
                       


                        foreach (KeyValuePair<int, string> dr in dicstucolspan)
                        {
                            int r = dr.Key;
                            Showgrid.Rows[r].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            Showgrid.Rows[r].Cells[0].ColumnSpan = col;
                            Showgrid.Rows[r].Cells[0].BackColor = Color.LightGray;
                            Showgrid.Rows[r].Cells[0].BorderColor = Color.Black;
                            for (int a = 1; a < col; a++)
                                Showgrid.Rows[r].Cells[a].Visible = false;
                        }
                    }
                    else
                    {
                        Label_error_Showgrid.Visible = true;
                        Label_error_Showgrid.Text = "No Records Found";


                    }

                    if (dataApplyCondonation.Columns.Count > 0 && dataApplyCondonation.Rows.Count > 1)
                    {
                        Showgrid1.DataSource = dataApplyCondonation;
                        Showgrid1.DataBind();
                        Showgrid1.Visible = true;

                        for (int grCol = 0; grCol < Showgrid1.HeaderRow.Cells.Count; grCol++)
                            Showgrid1.HeaderRow.Cells[grCol].Visible = false;

                        for (int i = 0; i < dataApplyCondonation.Columns.Count + 2; i++)
                        {
                            if (i >= 3)
                            {
                                Showgrid1.Rows[0].Cells[i + 1].Text = Convert.ToString(headlist2[i]);
                            }
                            else
                                Showgrid1.Rows[0].Cells[i].Text = Convert.ToString(headlist2[i]);
                        }
                        Showgrid1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                        Showgrid1.Rows[0].Font.Name = "Book Anitique";
                        Showgrid1.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        Showgrid1.Rows[0].Font.Bold = true;
                        
                        chkselecall.Visible = true;


                        int col1 = dataApplyCondonation.Columns.Count;
                        foreach (KeyValuePair<int, string> dr in dicstuApplyCondonationcolspan)
                        {
                            int r = dr.Key;
                            Showgrid1.Rows[r - 1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            Showgrid1.Rows[r - 1].Cells[0].ColumnSpan = col1 + 2;
                            Showgrid1.Rows[r - 1].Cells[0].BackColor = Color.LightGray;
                            Showgrid1.Rows[r - 1].Cells[0].BorderColor = Color.Black;
                            Showgrid1.Rows[r].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            Showgrid1.Rows[r].Cells[0].ColumnSpan = col1 + 2;
                            Showgrid1.Rows[r].Cells[0].BackColor = Color.LightGray;
                            Showgrid1.Rows[r].Cells[0].BorderColor = Color.Black;
                            for (int a = 1; a < col1 + 3; a++)
                            {
                                Showgrid1.Rows[r - 1].Cells[a].Visible = false;
                                Showgrid1.Rows[r].Cells[a].Visible = false;
                            }
                        }


                    }
                    else
                    {
                        Label_error_Showgrid1.Visible = true;
                        Label_error_Showgrid1.Text = "No Students Eligible To Condonation Apply";
                    }




                    if (datanotelg.Columns.Count > 0 && datanotelg.Rows.Count > 1)
                    {
                        Showgrid2.DataSource = datanotelg;
                        Showgrid2.DataBind();
                        Showgrid2.Visible = true;

                        for (int grCol = 0; grCol < Showgrid2.HeaderRow.Cells.Count; grCol++)
                            Showgrid2.HeaderRow.Cells[grCol].Visible = false;
                        for (int i = 0; i < Showgrid2.HeaderRow.Cells.Count - 2; i++)
                        {
                            
                            if (i >= 2)
                            {
                                Showgrid2.Rows[0].Cells[i + 2].Text = Convert.ToString(headlist3[i]);
                            }
                            else
                                Showgrid2.Rows[0].Cells[i].Text = Convert.ToString(headlist3[i]);

                        }
                        Showgrid2.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                        Showgrid2.Rows[0].Font.Name = "Book Anitique";
                        Showgrid2.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        Showgrid2.Rows[0].Font.Bold = true;
                        Showgrid2.Visible = true;


                        int col2 = datanotelg.Columns.Count;
                        foreach (KeyValuePair<int, string> dr in dicstunotelgcolspan)
                        {
                            int r = dr.Key;
                            Showgrid2.Rows[r].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            Showgrid2.Rows[r].Cells[0].ColumnSpan = col2;
                            Showgrid2.Rows[r].Cells[0].BackColor = Color.LightGray;
                            Showgrid2.Rows[r].Cells[0].BorderColor = Color.Black;
                            for (int a = 1; a < col + 4; a++)
                                Showgrid2.Rows[r].Cells[a].Visible = false;
                        }
                    }
                    else
                    {
                        Label_error_Showgrid2.Visible = true;
                        Label_error_Showgrid2.Text = "Students Are Not Eligible To Write Exam";
                    }


                    //Column Order 
                    string gridcoltext = "";
                    for (int c = 0; c < cblColumnOrder.Items.Count; c++)
                    {
                        if (cblColumnOrder.Items[c].Selected == false)
                        {
                            string coltext = cblColumnOrder.Items[c].Text.Trim();
                            for (int i = 0; i < Showgrid.HeaderRow.Cells.Count; i++)
                            {
                                gridcoltext = Showgrid.HeaderRow.Cells[i].Text.Trim();
                                if (coltext == gridcoltext)
                                {
                                    Showgrid.HeaderRow.Cells[i].Visible = false;
                                    for (int r = 0; r < Showgrid.Rows.Count; r++)
                                        Showgrid.Rows[r].Cells[i].Visible = false;
                                }
                            }
                            for (int i = 0; i < Showgrid1.HeaderRow.Cells.Count; i++)
                            {
                                gridcoltext = Showgrid1.HeaderRow.Cells[i].Text.Trim();
                                if (coltext == gridcoltext)
                                {
                                    Showgrid1.HeaderRow.Cells[i].Visible = false;
                                    for (int r = 0; r < Showgrid1.Rows.Count; r++)
                                        Showgrid1.Rows[r].Cells[i].Visible = false;
                                }
                            }
                            for (int i = 0; i < Showgrid2.HeaderRow.Cells.Count; i++)
                            {
                                gridcoltext = Showgrid2.HeaderRow.Cells[i].Text.Trim();
                                if (coltext == gridcoltext)
                                {
                                    Showgrid2.HeaderRow.Cells[i].Visible = false;
                                    for (int r = 0; r < Showgrid2.Rows.Count; r++)
                                        Showgrid2.Rows[r].Cells[i].Visible = false;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }

            


           
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    #region Grid1_Event
    protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                for (int j = colcnt; j < datastudentwriteexam.Columns.Count; j++)
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }

        }
        catch
        {


        }

    }
    #endregion

    #region Grid2_Event_For_ApplyCondonation
    protected void Showgrid1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {


          
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Visible = false;

                int row = e.Row.RowIndex;
                if (dicstuApplyCondonationedit.ContainsKey(row))
                {
                    Button btnedit = (Button)e.Row.Cells[2].FindControl("editbtn");
                    btnedit.Enabled = false;
                }


                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                for (int j = colcnt1 + 1; j < Showgrid1.HeaderRow.Cells.Count; j++)
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;

            }
        }
        catch
        {

        }

    }

    protected void btnedit_Click(object sender, EventArgs e)
    {
        try
        {
            rowindex = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;


            if (rowindex > -1)
            {
                conddate = txtCondonationDate.Text;
                condchallan = txtChallanAmount.Text;
                string stdapplycon = dicstudApplyCondonation[rowindex];
                string[] split1 = stdapplycon.Split('$');


                string Cond_app_no = Convert.ToString(split1[3]).Trim();
                string Cond_semester = Convert.ToString(split1[6]).Trim();
                string Cond_batchyr = Convert.ToString(split1[5]).Trim();
                string Cond_degreecode = Convert.ToString(split1[4]).Trim();
                string condqry = "select convert(varchar(20), ChallanDate , 103) as ChallanDate,ChallanNo from Eligibility_list where app_no='" + Cond_app_no + "' and Semester='" + Cond_semester + "' and batch_year='" + Cond_batchyr + "' and degree_code='" + Cond_degreecode + "' and is_eligible='2'";
                DataTable dtCondonationApplied = dir.selectDataTable(condqry);
                if (dtCondonationApplied.Rows.Count > 0)
                {

                    if (dicCondonationappcnt.ContainsKey(rowindex))
                    {
                        dicCondonationappcnt.Remove(rowindex);
                        dicCondonationappcnt.Add(rowindex, Convert.ToString(dtCondonationApplied.Rows[0]["ChallanDate"]) + "$" + Convert.ToString(dtCondonationApplied.Rows[0]["ChallanNo"]));
                    }
                    else
                    {
                        dicCondonationappcnt.Add(rowindex, Convert.ToString(dtCondonationApplied.Rows[0]["ChallanDate"]) + "$" + Convert.ToString(dtCondonationApplied.Rows[0]["ChallanNo"]));

                    }

                    txtCondonationDate.Text = Convert.ToString(dtCondonationApplied.Rows[0]["ChallanDate"]);
                    txtChallanAmount.Text = Convert.ToString(dtCondonationApplied.Rows[0]["ChallanNo"]);
                    divPopCond.Visible = true;
                }
            }


        }
        catch
        {

        }



    }

    #endregion

    #region Grid3_Event_For_Notelg
    protected void Showgrid2_DataBound(object sender, EventArgs e)
    {
        try
        {

            if (Showgrid2.Rows.Count > 0)
            {
                string strelg = string.Empty;
                for (int a = 0; a < Showgrid2.Rows.Count; a++)
                {
                    (Showgrid2.Rows[a].FindControl("ddl_Eligible") as DropDownList).Items.Clear();



                    (Showgrid2.Rows[a].FindControl("ddl_Eligible") as DropDownList).Items.Add("Not Eligible");
                    (Showgrid2.Rows[a].FindControl("ddl_Eligible") as DropDownList).Items.Add("Eligible For Next Semester");
                    Label elg = (Label)Showgrid2.Rows[a].FindControl("lbl_Eligible");

                    strelg = elg.Text;
                    if (!string.IsNullOrEmpty(strelg) && strelg != "1")
                    {
                        (Showgrid2.Rows[a].FindControl("ddl_Eligible") as DropDownList).SelectedIndex = (Showgrid2.Rows[a].FindControl("ddl_Eligible") as DropDownList).Items.IndexOf((Showgrid2.Rows[a].FindControl("ddl_Eligible") as DropDownList).Items.FindByText(strelg));
                    }
                    else
                    {
                        (Showgrid2.Rows[a].FindControl("ddl_Eligible") as DropDownList).SelectedIndex = (Showgrid2.Rows[a].FindControl("ddl_Eligible") as DropDownList).Items.IndexOf((Showgrid2.Rows[a].FindControl("ddl_Eligible") as DropDownList).Items.FindByText(""));
                    }

                }
            }
        }
        catch
        {

        }
    }

    protected void Showgrid2_onRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
          
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[3].Visible = false;
                Label elg = (Label)e.Row.Cells[1].FindControl("lbl_Eligible");

                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                for (int j = colcnt3 + 2; j < datanotelg.Columns.Count; j++)
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;

            }
        }

        catch
        {

        }

    }

    #endregion




    public void persentmonthcal()
    {
        DataSet dsondutyval = new DataSet();
        bool isadm = false;
        hatonduty.Clear();
        try
        {
            per_abshrs_spl = 0;
            tot_per_hrs_spl = 0;
            per_leave = 0;
            tot_conduct_hr_spl = 0;
            tot_ondu_spl = 0;
            tot_ml_spl = 0;
            int my_un_mark = 0;
            int njdate_mng = 0, njdate_evng = 0;
            int per_holidate_mng = 0, per_holidate_evng = 0;
            mng_conducted_half_days = 0;
            evng_conducted_half_days = 0;
            notconsider_value = 0;
            conduct_hour_new = 0;
            cal_from_date = cal_from_date_tmp;
            cal_to_date = cal_to_date_tmp;
            per_from_date = per_from_gendate;
            per_to_date = per_to_gendate;
            dumm_from_date = per_from_date;
            string admdate = ds4.Tables[0].Rows[rows_count]["adm_date"].ToString();
            string[] admdatesp = admdate.Split(new Char[] { '/' });
            admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
            Admission_date = Convert.ToDateTime(admdate);
            dd = ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString();
            strondutyvalue = "select * from attendance_withreason where roll_no='" + dd + "' and month_year ='" + cal_from_date + "'";
            dsondutyval = d2.select_method_wo_parameter(strondutyvalue, "text");
            hat.Clear();
            hat.Add("std_rollno", ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString());
            hat.Add("from_month", cal_from_date);
            hat.Add("to_month", cal_to_date);
            ds2 = d2.select_method("STUD_ATTENDANCE", hat, "sp");
            mmyycount = ds2.Tables[0].Rows.Count;
            moncount = mmyycount - 1;
            if (rows_count == 0)
            {
                hat.Clear();
                hat.Add("degree_code", int.Parse(ddlbranch.SelectedValue.ToString()));
                hat.Add("sem", int.Parse(ddlsemester.SelectedItem.ToString()));
                hat.Add("from_date", frdate.ToString());
                hat.Add("to_date", todate.ToString());
                hat.Add("coll_code", int.Parse(Session["collegecode"].ToString()));
                int iscount = 0;
                string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsemester.SelectedItem.ToString() + "";
                DataSet dsholiday = d2.select_method_wo_parameter(sqlstr_holiday, "Text");
                if (dsholiday.Tables[0].Rows.Count > 0)
                {
                    iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
                }
                hat.Add("iscount", iscount);
                ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
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
                        // holiday_table1.Add(dummy_split[2] + "/" + dummy_split[1] + "/" + dummy_split[0], holiday_sched_details);
                    }
                }
                if (ds3.Tables[1].Rows.Count != 0)
                {
                    for (int k = 0; k < ds3.Tables[1].Rows.Count; k++)
                    {
                        string[] split_date_time1 = ds3.Tables[1].Rows[k]["HOLI_DATE"].ToString().Split(' ');
                        string[] dummy_split = split_date_time1[0].Split('/');
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
                            if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
                            {
                                getspecial_hr();
                            }
                        }
                        for (int i = 1; i <= mmyycount; i++)
                        {
                            ds2.Tables[0].DefaultView.RowFilter = "month_year='" + cal_from_date + "' and roll_no='" + ds4.Tables[0].Rows[rows_count]["ROLL_NO"].ToString() + "'";
                            DataView dvattvalue = ds2.Tables[0].DefaultView;
                            if (dvattvalue.Count > 0)//Added by srinath 13/10/2014
                            {
                                // if (cal_from_date == int.Parse(ds2.Tables[0].Rows[next]["month_year"].ToString()))
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
                                        if (split_holiday_status[0].ToString() == "3")
                                        {
                                            split_holiday_status_1 = "1";
                                            split_holiday_status_2 = "1";
                                        }
                                        else if (split_holiday_status[0].ToString() == "1")
                                        {
                                            if (split_holiday_status[1].ToString() == "1")
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
                                                //value = ds2.Tables[0].Rows[next][date].ToString();
                                                value = dvattvalue[0][date].ToString();
                                                //Added by srinath 31/1/2014=========Start
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
                                                    my_un_mark++;
                                                }
                                            }
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
                                                    my_un_mark++;
                                                }
                                            }
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
            tot_ml = 0;
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void persentmonthcal(string collegeCode, string degree, string sem, string rollno, string admitDate)
    {
        medicalLeaveCountPerSession = 0;
        bool isadm = false;
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
        string admdate = admitDate;// ds4.Tables[0].Rows[rows_count]["adm_date"].ToString();
        //Admission_date = Convert.ToDateTime(admdate);
        DateTime.TryParseExact(admdate, "dd/MM/yyyy", null, DateTimeStyles.None, out Admission_date);
        dd = rollno.Trim();
        hat.Clear();
        hat.Add("std_rollno", rollno.Trim());
        hat.Add("from_month", cal_from_date);
        hat.Add("to_month", cal_to_date);
        ds2 = d2.select_method("STUD_ATTENDANCE", hat, "sp");
        mmyycount = ds2.Tables[0].Rows.Count;
        moncount = mmyycount - 1;
        if (deptflag == false)
        {
            deptflag = true;
            hat.Clear();
            hat.Add("degree_code", int.Parse(Convert.ToString(degree).Trim()));
            hat.Add("sem", int.Parse(Convert.ToString(sem).Trim()));
            hat.Add("from_date", Convert.ToString(frdate));
            hat.Add("to_date", Convert.ToString(todate));
            hat.Add("coll_code", int.Parse(Convert.ToString(collegeCode)));
            int iscount = 0;
            string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + todate.ToString() + "' and degree_code=" + degree + " and semester=" + sem;
            DataSet dsholiday = d2.select_method_wo_parameter(sqlstr_holiday, "Text");
            if (dsholiday.Tables[0].Rows.Count > 0)
            {
                iscount = Convert.ToInt16(dsholiday.Tables[0].Rows[0]["cnt"].ToString());
            }
            hat.Add("iscount", iscount);
            ds3 = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
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
                medicalLeaveCountPerSession = 0;
                nohrsprsentperday = 0;
                noofdaypresen = 0;
                isadm = false;
                if (dumm_from_date >= Admission_date)
                {
                    isadm = true;
                    int temp_unmark = 0;
                    for (int i = 1; i <= mmyycount; i++)
                    {
                        ds2.Tables[0].DefaultView.RowFilter = "month_year='" + cal_from_date + "' and roll_no='" + rollno + "'";
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
                                    value_holi_status = holiday_table11[(Convert.ToInt16(dummy_split[1])).ToString() + "/" + (Convert.ToInt16(dummy_split[0])).ToString() + "/" + (Convert.ToInt16(dummy_split[2])).ToString()].ToString();//dummy_split[1].ToString() + "/" + dummy_split[0].ToString() + "/" + dummy_split[2].ToString()
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
                                    per_leavehrs = 0;
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
                                                if (value == "4")
                                                {
                                                    medicalLeaveCountPerSession++;
                                                    medicalLeaveHours++;
                                                }
                                                if (value == "10")
                                                {
                                                    per_leavehrs++;
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
                                            if (per_leavehrs > 0)
                                            {
                                                Leave += 0.5;
                                                leavfinaeamount = leavfinaeamount + moringabsentfine;
                                            }
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
                                        if (medicalLeaveCountPerSession + njhr >= minpresI)
                                        {
                                            medicalLeaveDays = medicalLeaveDays + 0.5;
                                        }
                                    }
                                    medicalLeaveCountPerSession = 0;
                                    per_perhrs = 0;
                                    per_abshrs = 0;
                                    temp_unmark = 0;
                                    per_leavehrs = 0;
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
                                                if (value == "10")
                                                {
                                                    per_leavehrs++;
                                                }
                                                if (value == "4")
                                                {
                                                    medicalLeaveCountPerSession++;
                                                    medicalLeaveHours++;
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
                                            if (per_leavehrs > 0)
                                            {
                                                Leave += 0.5;
                                                leavfinaeamount = leavfinaeamount + eveingabsentfine;
                                            }
                                        }
                                        if (njhr >= minpresII)
                                        {
                                            njdate_evng += 1;
                                            njdate += 0.5;
                                        }
                                        if (medicalLeaveCountPerSession + njhr >= minpresII)
                                        {
                                            medicalLeaveDays = medicalLeaveDays + 0.5;
                                        }
                                        if (Session["attdaywisecla"] != null && Session["attdaywisecla"].ToString() == "1")
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

    public void getspecial_hr()
    {
        try
        {
            string hrdetno = string.Empty;
            if (ht_sphr.Contains(Convert.ToString(dumm_from_date)))
            {
                hrdetno = Convert.ToString(ht_sphr[Convert.ToString(dumm_from_date)]);
            }
            if (hrdetno != "")
            {
                DataSet ds_splhr_query_master = new DataSet();
                string splhr_query_master = "select attendance from specialhr_attendance where roll_no='" + dd + "'  and hrdet_no in(" + hrdetno + ")";
                DataSet dssplatt = d2.select_method_wo_parameter(splhr_query_master, "Text");
                if (dssplatt.Tables[0].Rows.Count > 0)
                {
                    for (int hp = 0; hp < dssplatt.Tables[0].Rows.Count; hp++)
                    {
                        value = dssplatt.Tables[0].Rows[hp][0].ToString();
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
                    tot_ml_spl_fals = tot_ml_spl;
                }
                else if (check == 2)
                {
                    per_abshrs_spl_true = per_abshrs_spl;
                    tot_per_hrs_spl_true = tot_per_hrs_spl;
                    per_leave_true = per_leave;
                    tot_conduct_hr_spl_true = tot_conduct_hr_spl;
                    tot_ondu_spl_true = tot_ondu_spl;
                    tot_ml_spl_true = tot_ml_spl;
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }


    //Saran
    //protected void FpSpread2_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{
    //    try
    //    {
    //        string ctrlname = Page.Request.Params["__EVENTTARGET"];
    //        if (ctrlname != null && ctrlname != String.Empty)
    //        {
    //            string[] spiltspreadname = ctrlname.Split('$');
    //            if (spiltspreadname.GetUpperBound(0) > 1)
    //            {
    //                string getrowxol = spiltspreadname[3].ToString().Trim();
    //                string[] spr = getrowxol.Split(',');
    //                if (spr.GetUpperBound(0) == 1)
    //                {
    //                    int arow = Convert.ToInt32(spr[0]);
    //                    int acol = Convert.ToInt32(spr[1]);
    //                    if (arow == 0 && acol > 4)
    //                    {
    //                        string setval = e.EditValues[acol].ToString();
    //                        int setvalcel = 0;
    //                        if (setval.Trim().ToLower() == "true" || setval.Trim() == "1")
    //                        {
    //                            setvalcel = 1;
    //                        }
    //                        for (int r = 1; r < FpSpread2.Sheets[0].RowCount; r++)
    //                        {
    //                            int value = 0;
    //                            if (int.TryParse(FpSpread2.Sheets[0].Cells[r, 0].Text, out value))
    //                            {
    //                                FpSpread2.Sheets[0].Cells[r, acol].Value = setvalcel;
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errmsg.Visible = true;
    //        errmsg.Text = ex.ToString();
    //    }
    //}

    public void clear2()
    {
        DivAddcondonationfees.Visible = false;
        grdAddcondonationfees.Visible = false;
        panelrollnopop.Visible = true;
        btnaddrow.Visible = false;
        btnfeesave.Visible = false;
        btnfeedelete.Visible = false;
    }

    protected void btncondonationfee_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            clear2();
            rblDayOrPercerntage.SelectedIndex = 0;
            bindpstram();
            bindeducation();
            loadheadre();
            loadledger();

            //if (rblCOndonationType.SelectedIndex == 1)
            //{
            //    phRow1.Controls.Add(btnpgo);
            //}
            //else
            //{
            //    phRow2.Controls.Add(btnpgo);
            //}
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void bindpstram()
    {
        try
        {
            ddlpstream.Items.Clear();
            ddlpstream.Enabled = false;
            DataSet ds = d2.select_method_wo_parameter("select distinct isnull(Ltrim(rtrim(type)),'') as type from Course where isnull(Ltrim(rtrim(type)),'')<>'' and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "' ", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlpstream.DataSource = ds;
                ddlpstream.DataTextField = "type";
                ddlpstream.DataValueField = "type";
                ddlpstream.DataBind();
                ddlpstream.Enabled = true;
            }
            else
            {
                ddlpstream.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            panelerrmsg.Visible = true;
            panelerrmsg.Text = ex.ToString();
        }
    }

    public void bindeducation()
    {
        try
        {
            ddlpcourse.Items.Clear();
            string collegecode = Convert.ToString(Session["collegecode"]).Trim();
            string usercode = Convert.ToString(Session["usercode"]).Trim();
            string group_code = Convert.ToString(Session["group_code"]).Trim();
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = Convert.ToString(group_semi[0]).Trim();
            }
            string typeval = string.Empty;
            if (ddlpstream.Items.Count > 0 && ddlpstream.Enabled == true)
            {
                typeval = " and course.type='" + Convert.ToString(ddlpstream.SelectedItem).Trim() + "'";
            }
            string query = string.Empty;
            if ((Convert.ToString(group_code).Trim() != "") && (Convert.ToString(group_code).Trim() != "0") && (Convert.ToString(group_code).Trim() != "-1"))
            {
                query = "select distinct course.Edu_Level from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_code + "' " + typeval + "";
            }
            else
            {
                query = "select distinct course.Edu_Level from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' " + typeval + "";
            }
            DataSet ds = new DataSet();
            ds = d2.select_method(query, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlpcourse.DataSource = ds;
                ddlpcourse.DataValueField = "Edu_Level";
                ddlpcourse.DataTextField = "Edu_Level";
                ddlpcourse.DataBind();
            }
        }
        catch (Exception ex)
        {
            panelerrmsg.Visible = true;
            panelerrmsg.Text = ex.ToString();
        }
    }

    public void loadheadre()
    {
        try
        {
            ddlheader.Items.Clear();
            string collegecode = Session["collegecode"].ToString();
            //string straccheadquery = "select header_name,header_id from Acctheader where header_name not in ('arrear')";
            string straccheadquery = "select headername,headerpk from FM_HeaderMaster where collegecode='" + collegecode + "'";
            DataSet ds = d2.select_method_wo_parameter(straccheadquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlheader.DataSource = ds;
                ddlheader.DataTextField = "headername";
                ddlheader.DataValueField = "headerpk";
                ddlheader.DataBind();
            }
        }
        catch (Exception ex)
        {
            panelerrmsg.Visible = true;
            panelerrmsg.Text = ex.ToString();
        }
    }

    public void loadledger()
    {
        try
        {
            ddlledger.Items.Clear();
            if (ddlheader.Items.Count > 0)
            {
                //string strquer = "select fee_type,fee_code from fee_info where fee_type not in ('Cash','Income & Expenditure','Misc') and fee_type not in (select bankname from bank_master1) and header_id = " + ddlheader.SelectedValue.ToString() + " order by fee_code";
                string strquer = "select ledgerpk,ledgername from FM_LedgerMaster where collegecode='" + collegecode + "' and headerfk = " + ddlheader.SelectedValue.ToString() + " order by ledgerpk";
                DataSet ds1 = d2.select_method_wo_parameter(strquer, "text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    ddlledger.DataSource = ds1;
                    ddlledger.DataTextField = "ledgername";
                    ddlledger.DataValueField = "ledgerpk";
                    ddlledger.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            panelerrmsg.Visible = true;
            panelerrmsg.Text = ex.ToString();
        }
    }

    protected void ddlpstream_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindeducation();
        clear2();
    }

    protected void ddlpcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear2();
    }

    protected void ddlheader_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadledger();
        clear2();
    }

    protected void ddlledger_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear2();
    }



    #region CondonationFeesSetting

    protected void btnpgo_Click(object sender, EventArgs e)
    {
        try
        {
            btnfeesave.Visible = false;
            btnfeedelete.Visible = false;
            btnaddrow.Visible = false;
            panelerrmsg.Visible = false;
            panelerrmsg.Text = string.Empty;

            //  Fpconfees.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Attendance Percentage";
            bool isPercentage = false;
            if (rblDayOrPercerntage.SelectedIndex == 0)
                isPercentage = false;
            else
                isPercentage = true;


            dtfine.Columns.Add("FromPer", typeof(string));
            dtfine.Columns.Add("ToPer", typeof(string));
            dtfine.Columns.Add("DayFrom", typeof(string));
            dtfine.Columns.Add("DayTo", typeof(string));
            dtfine.Columns.Add("FineAmount", typeof(string));

            DivAddcondonationfees.Visible = false;
            string typeval = string.Empty;
            string gettyoeval = string.Empty;
            if (ddlpstream.Items.Count > 0 && ddlpstream.Enabled == true)
            {
                typeval = " and type='" + ddlpstream.SelectedItem.ToString() + "'";
                gettyoeval = ddlpstream.SelectedItem.ToString();
            }
            string courseval = string.Empty;// ddlpcourse.SelectedItem.ToString();
            string headerid = ddlheader.SelectedValue.ToString();
            string ledger = ddlledger.SelectedValue.ToString();
            if (ddlpcourse.Items.Count > 0)
            {
                courseval = Convert.ToString(ddlpcourse.SelectedItem.Text).Trim();
            }
            else
            {
                panelerrmsg.Visible = true;
                panelerrmsg.Text = "No Education Level Were Found. Please Check The User Degree Rights And Then Proceed";
                return;
            }
            if (ddlheader.Items.Count > 0)
            {
                headerid = Convert.ToString(ddlheader.SelectedValue).Trim();
            }
            else
            {
                panelerrmsg.Visible = true;
                panelerrmsg.Text = "No Header Were Found.";
                return;
            }
            if (ddlledger.Items.Count > 0)
            {
                ledger = Convert.ToString(ddlledger.SelectedValue).Trim();
            }
            else
            {
                panelerrmsg.Visible = true;
                panelerrmsg.Text = "No Ledger Were Found.";
                return;
            }
            collegecode = Convert.ToString(Session["collegecode"]).Trim();
            string strquery = "select Att_from,Att_To,Fine_Amt,isnull(isDays,'0') as isDays from Condonation_Fee where college_code='" + collegecode + "' " + typeval + " and Edu_Level='" + courseval + "' and isnull(isDays,'0')='" + isPercentage + "' order by Att_from";
            DataSet dsconlevel = d2.select_method_wo_parameter(strquery, "Text");
            DivAddcondonationfees.Visible = true;
            btnfeesave.Visible = true;
            btnfeedelete.Visible = true;
            btnaddrow.Visible = true;
            if (dsconlevel.Tables.Count > 0 && dsconlevel.Tables[0].Rows.Count > 0)
            {
                for (int r = 0; r < dsconlevel.Tables[0].Rows.Count; r++)
                {
                    drow = dtfine.NewRow();

                    string DayOrPercentage = Convert.ToString(dsconlevel.Tables[0].Rows[r]["isDays"]).Trim();
                    bool Day = false;
                    bool.TryParse(DayOrPercentage.Trim(), out Day);
                    if (!Day)
                    {

                    }
                    string attfrom = Convert.ToString(dsconlevel.Tables[0].Rows[r]["Att_from"]).Trim();
                    string attto = Convert.ToString(dsconlevel.Tables[0].Rows[r]["Att_To"]).Trim();
                    string fineamount = Convert.ToString(dsconlevel.Tables[0].Rows[r]["Fine_Amt"]).Trim();

                    if (rblDayOrPercerntage.SelectedIndex == 0)
                    {
                        drow["FromPer"] = attfrom;
                        drow["ToPer"] = attto;
                        drow["DayFrom"] = "";
                        drow["DayTo"] = "";

                    }
                    else
                    {
                        drow["FromPer"] = "";
                        drow["ToPer"] = "";
                        drow["DayFrom"] = attfrom;
                        drow["DayTo"] = attto;

                    }

                    drow["FineAmount"] = fineamount;
                    dtfine.Rows.Add(drow);

                }
            }
            if ((dtfine.Rows.Count > 0 && dtfine.Columns.Count > 0) || dtfine.Rows.Count == 0)
            {
                if (dtfine.Rows.Count == 0)
                {
                    drow = dtfine.NewRow();
                    drow["FromPer"] = "";
                    drow["ToPer"] = "";
                    drow["DayFrom"] = "";
                    drow["DayTo"] = "";
                    drow["FineAmount"] = "";
                    dtfine.Rows.Add(drow);
                }
                grdAddcondonationfees.DataSource = dtfine;
                grdAddcondonationfees.DataBind();
                grdAddcondonationfees.Visible = true;
                DivAddcondonationfees.Visible = true;
                ViewState["CurrentTable"] = dtfine;



            }

        }
        catch (Exception ex)
        {
            panelerrmsg.Visible = true;
            panelerrmsg.Text = ex.ToString();
        }
    }



    protected void btnaddrow_Click(object sender, EventArgs e)
    {
        try
        {
            if (grdAddcondonationfees.Visible == true)
            {
                int row_cnt = grdAddcondonationfees.Rows.Count;
                if (row_cnt != 0)
                {
                    dtfine = (DataTable)ViewState["CurrentTable"];
                    dtfine.Rows.RemoveAt(dtfine.Rows.Count - 1);

                    int r = grdAddcondonationfees.Rows.Count - 1;

                    drow = dtfine.NewRow();
                    TextBox frper = (TextBox)grdAddcondonationfees.Rows[r].FindControl("txt_FromPer");
                    TextBox toper = (TextBox)grdAddcondonationfees.Rows[r].FindControl("txt_ToPer");
                    TextBox dayfr = (TextBox)grdAddcondonationfees.Rows[r].FindControl("txt_DayFrom");
                    TextBox dayto = (TextBox)grdAddcondonationfees.Rows[r].FindControl("txt_DayTo");
                    TextBox fine = (TextBox)grdAddcondonationfees.Rows[r].FindControl("txt_FineAmount");

                    drow["FromPer"] = frper.Text;
                    drow["ToPer"] = toper.Text;
                    drow["DayFrom"] = dayfr.Text;
                    drow["DayTo"] = dayto.Text;
                    drow["FineAmount"] = fine.Text;
                    dtfine.Rows.Add(drow);

                    ViewState["CurrentTable"] = dtfine;
                }
                if (ViewState["CurrentTable"] != null)
                {
                    dtfine = (DataTable)ViewState["CurrentTable"];
                    drCurrentRow = null;
                    int TotRowCnt = Convert.ToInt32(dtfine.Rows.Count);
                    if (dtfine.Rows.Count > 0)
                    {
                        DataRow drow1;

                        drow1 = dtfine.NewRow();
                        drow1["FromPer"] = "";
                        drow1["ToPer"] = "";
                        drow1["DayFrom"] = "";
                        drow1["DayTo"] = "";
                        drow1["FineAmount"] = "";
                        dtfine.Rows.Add(drow1);

                        ViewState["CurrentTable"] = dtfine;
                    }

                }


                grdAddcondonationfees.DataSource = dtfine;
                grdAddcondonationfees.DataBind();
                grdAddcondonationfees.Visible = true;
                DivAddcondonationfees.Visible = true;
                Session["dtCondonationfees"] = dtfine;



            }
            else
            {
                panelerrmsg.Visible = true;
                panelerrmsg.Text = "Please Click Go Before Add Row";
            }
        }
        catch
        {

        }
    }

    protected void bbtnfeesave_Click(object sender, EventArgs e)
    {
        try
        {

            if (grdAddcondonationfees.Rows.Count == 0)
            {
                panelerrmsg.Visible = true;
                panelerrmsg.Text = "Please Enter The Value And Then Proceed";
                return;
            }
            if (ddlheader.Items.Count == 0)
            {
                panelerrmsg.Visible = true;
                panelerrmsg.Text = "Please Select The Header And Then Proceed";
                return;
            }
            if (ddlledger.Items.Count == 0)
            {
                panelerrmsg.Visible = true;
                panelerrmsg.Text = "Please Select The Ledger And Then Proceed";
                return;
            }
            string getval = "";
            string getval2 = "";
            string getval3 = "";
            for (int r = 0; r < grdAddcondonationfees.Rows.Count; r++)
            {

                TextBox frper = (TextBox)grdAddcondonationfees.Rows[r].FindControl("txt_FromPer");
                TextBox toper = (TextBox)grdAddcondonationfees.Rows[r].FindControl("txt_ToPer");
                TextBox dayfr = (TextBox)grdAddcondonationfees.Rows[r].FindControl("txt_DayFrom");
                TextBox dayto = (TextBox)grdAddcondonationfees.Rows[r].FindControl("txt_DayTo");
                TextBox fine = (TextBox)grdAddcondonationfees.Rows[r].FindControl("txt_FineAmount");


                if (rblDayOrPercerntage.SelectedIndex == 0)
                {
                    getval = frper.Text.ToString();
                    getval2 = toper.Text.ToString();
                }
                else
                {
                    getval = dayfr.Text.ToString();
                    getval2 = dayto.Text.ToString();
                }


                getval3 = fine.Text.ToString();
                if (getval == "")
                {
                    panelerrmsg.Visible = true;
                    panelerrmsg.Text = "Please Enter The From Attendance % Value At " + (r + 1) + " Row And Then Proceed";
                    return;
                }
                if (getval2.Trim() == "")
                {
                    panelerrmsg.Visible = true;
                    panelerrmsg.Text = "Please Enter The To Attendance % Value At " + (r + 1) + " Row And Then Proceed";
                    return;
                }
                #region Added by Idhris 15-10-2016
                if (getval3.Trim() == "" && rblCOndonationType.SelectedIndex == 1)
                {
                    getval3 = "0";
                }
                #endregion
                if (getval3.Trim() == "")
                {
                    panelerrmsg.Visible = true;
                    panelerrmsg.Text = "Please Enter The Fine Amount At " + (r + 1) + " Row And Then Proceed";
                    return;
                }
                double attfrom = Convert.ToDouble(getval);
                double attto = Convert.ToDouble(getval2);
                if (attfrom > attto)
                {
                    panelerrmsg.Visible = true;
                    panelerrmsg.Text = "To Range Must Be Greater Than Or Equal From Range Value  Value At " + (r + 1) + " Row And Then Proceed";
                    return;
                }
            }
            string typeval = string.Empty;
            string gettyoeval = string.Empty;
            if (ddlpstream.Items.Count > 0 && ddlpstream.Enabled == true)
            {
                typeval = " and type='" + Convert.ToString(ddlpstream.SelectedItem).Trim() + "'";
                gettyoeval = Convert.ToString(ddlpstream.SelectedItem).Trim();
            }
            string courseval = Convert.ToString(ddlpcourse.SelectedItem).Trim();
            string headerid = Convert.ToString(ddlheader.SelectedValue).Trim();
            string ledger = Convert.ToString(ddlledger.SelectedValue).Trim();
            collegecode = Convert.ToString(Session["collegecode"]).Trim();
            bool isPercentage = false;
            if (rblDayOrPercerntage.SelectedIndex == 0)
            {
                isPercentage = false;
            }
            else
            {
                isPercentage = true;
            }
            string strsavevalue = "Delete from Condonation_Fee where college_code='" + collegecode + "' and Edu_Level='" + courseval + "' " + typeval + " and isnull(isDays,'0')='" + isPercentage + "'";
            int insupdval = d2.update_method_wo_parameter(strsavevalue, "Text");
            for (int r = 0; r < grdAddcondonationfees.Rows.Count; r++)
            {
                TextBox frper = (TextBox)grdAddcondonationfees.Rows[r].FindControl("txt_FromPer");
                TextBox toper = (TextBox)grdAddcondonationfees.Rows[r].FindControl("txt_ToPer");
                TextBox dayfr = (TextBox)grdAddcondonationfees.Rows[r].FindControl("txt_DayFrom");
                TextBox dayto = (TextBox)grdAddcondonationfees.Rows[r].FindControl("txt_DayTo");
                TextBox fine = (TextBox)grdAddcondonationfees.Rows[r].FindControl("txt_FineAmount");

                if (rblDayOrPercerntage.SelectedIndex == 0)
                {
                    getval = frper.Text.ToString();
                    getval2 = toper.Text.ToString();
                }
                else
                {
                    getval = dayfr.Text.ToString();
                    getval2 = dayto.Text.ToString();
                }


                getval3 = Convert.ToString(fine.Text).Trim();
                #region Added by Idhris 15-10-2016
                if (getval3.Trim() == "" && rblCOndonationType.SelectedIndex == 1)
                {
                    getval3 = "0";
                }
                #endregion
                double attfrom = Convert.ToDouble(getval);
                double attto = Convert.ToDouble(getval2);
                double feeval = Convert.ToDouble(getval3);
                strsavevalue = "insert into Condonation_Fee(college_code,Type,Edu_Level,Att_from,Att_To,Fine_Amt,Header_id,Fee_code,isDays)";
                strsavevalue = strsavevalue + "  values('" + collegecode + "','" + gettyoeval + "','" + courseval + "','" + attfrom + "','" + attto + "','" + feeval + "','" + headerid + "','" + ledger + "','" + isPercentage + "')";
                insupdval = d2.update_method_wo_parameter(strsavevalue, "Text");
            }
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
        }
        catch (Exception ex)
        {
            panelerrmsg.Visible = true;
            panelerrmsg.Text = ex.ToString();
        }
    }

    protected void btnfeedelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (grdAddcondonationfees.Rows.Count == 0)
            {
                panelerrmsg.Visible = true;
                panelerrmsg.Text = "Please Enter The Value And Then Proceed";
                return;
            }
            string typeval = string.Empty;
            string gettyoeval = string.Empty;
            if (ddlpstream.Items.Count > 0 && ddlpstream.Enabled == true)
            {
                typeval = " and type='" + Convert.ToString(ddlpstream.SelectedItem).Trim() + "'";
                gettyoeval = Convert.ToString(ddlpstream.SelectedItem).Trim();
            }
            string courseval = Convert.ToString(ddlpcourse.SelectedItem).Trim();
            string headerid = Convert.ToString(ddlheader.SelectedValue).Trim();
            string ledger = Convert.ToString(ddlledger.SelectedValue).Trim();
            collegecode = Convert.ToString(Session["collegecode"]).Trim();
            bool isPercentage = false;
            if (rblDayOrPercerntage.SelectedIndex == 0)
            {
                isPercentage = false;
            }
            else
            {
                isPercentage = true;
            }
            string strsavevalue = "Delete from Condonation_Fee where college_code='" + collegecode + "' and Edu_Level='" + courseval + "' " + typeval + " and isnull(isDays,'0')='" + isPercentage + "'";
            int insupdval = d2.update_method_wo_parameter(strsavevalue, "Text");
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted successfully')", true);
            btnpgo_Click(sender, e);
        }
        catch (Exception ex)
        {
            panelerrmsg.Visible = true;
            panelerrmsg.Text = ex.ToString();
        }
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        panelrollnopop.Visible = false;
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            string strinsupdaval = string.Empty;
            int insupdval = 0;
            string batchyear = string.Empty;  //ddlbatch.SelectedValue.ToString();
            string degreecode = string.Empty; // ddlbranch.SelectedValue.ToString();
            string sem = string.Empty;       // ddlsemester.SelectedValue.ToString();
            string sec = string.Empty;
            string secval = string.Empty;
            string stream = string.Empty;
            string qryedudegree = string.Empty;
            string sections = string.Empty;
            string strsec = string.Empty;
            //string secval = string.Empty;
            if (ddlstream.Items.Count > 0)
            {
                stream = Convert.ToString(ddlstream.SelectedItem.Text).Trim();
            }
            if (Convert.ToString(getCblSelectedValue(chklsbatch)) == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "No Batch Year Were Found";
                return;
            }
            else
            {
                batchyear = Convert.ToString(getCblSelectedValue(chklsbatch));
            }
            if (cblDegree.Items.Count == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = "No Degree Were Found";
                return;
            }
            selBranch = 0;
            qryBranch = string.Empty;
            degreecode = string.Empty;
            if (cblBranch.Items.Count == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = "No Branch Were Found";
                return;
            }
            else
            {
                foreach (ListItem li in cblBranch.Items)
                {
                    if (li.Selected)
                    {
                        selBranch++;
                        if (string.IsNullOrEmpty(degreecode.Trim()))
                        {
                            degreecode = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreecode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (selBranch > 0)
                {
                    qryBranch = " and r.degree_code in(" + degreecode + ")";
                    qryedudegree = " and d.Degree_Code in(" + degreecode + ")";
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select Any One Branch And Then Proceed";
                    return;
                }
            }
            if (ddlsemester.Items.Count == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = "No Semester Were Found";
                return;
            }
            else
            {
                sem = Convert.ToString(ddlsemester.SelectedValue).Trim();
            }
            selSec = 0;
            newsections = string.Empty;
            qrySec = string.Empty;
            if (cblSec.Items.Count > 0)
            {
                foreach (ListItem li in cblSec.Items)
                {
                    if (li.Selected)
                    {
                        selSec++;
                        if (string.IsNullOrEmpty(newsections.Trim()))
                        {
                            newsections = "'" + li.Value + "'";
                        }
                        else
                        {
                            newsections += ",'" + li.Value + "'";
                        }
                    }
                }
                if (selSec > 0)
                {
                    qrySec = " and isnull(Ltrim(rtrim(sections)),'') in (" + newsections + ")";
                    strsec = " and isnull(Ltrim(rtrim(sections)),'') in (" + newsections + ")";
                    secval = " and isnull(Ltrim(rtrim(r.sections)),'') in (" + newsections + ")";
                }
            }
            //if (ddlsection.Enabled == true && ddlsection.Items.Count > 0)
            //{
            //    if (ddlsection.SelectedItem.ToString() != string.Empty && ddlsection.Text != "All")
            //    {
            //        sec = ddlsection.SelectedItem.ToString();
            //        secval = " and r.Sections='" + sec + "'";
            //    }
            //}
            collegecode = Convert.ToString(Session["collegecode"]).Trim();
            //string currfinacialyear = d2.GetFunction("select LinkValue from InsSettings where LinkName='Current Financial Year' and college_code='" + collegecode + "'");
            //if (currfinacialyear.Trim() == "" || currfinacialyear.Trim() == "0")
            //{
            //    errmsg.Visible = true;
            //    errmsg.Text = "Please Select The financial Year";
            //    return;
            //}
            string typeval = string.Empty;
            string gettyoeval = string.Empty;
            if (ddlpstream.Items.Count > 0 && ddlpstream.Enabled == true)
            {
                typeval = " and type='" + Convert.ToString(ddlpstream.SelectedItem).Trim() + "'";
                gettyoeval = Convert.ToString(ddlpstream.SelectedItem).Trim();
            }
            //string courseval = d2.GetFunction("select c.Edu_Level from Degree d,Course c where d.Course_Id=c.Course_Id and d.Degree_Code='" + degreecode + "'");
            string feecetsemyear = string.Empty;
            string strtype = d2.GetFunction("select LinkValue from New_InsSettings where college_code='" + collegecode + "' and LinkName='Fee Yearwise' And user_code = '" + usercode + "'");
            if (strtype == "1")
            {
                if (Convert.ToString(sem).Trim() == "1" || Convert.ToString(sem).Trim() == "2")
                {
                    feecetsemyear = "1 Year";
                }
                else if (Convert.ToString(sem).Trim() == "3" || Convert.ToString(sem).Trim() == "4")
                {
                    feecetsemyear = "2 Year";
                }
                else if (Convert.ToString(sem).Trim() == "5" || Convert.ToString(sem).Trim() == "6")
                {
                    feecetsemyear = "3 Year";
                }
                else if (Convert.ToString(sem).Trim() == "7" || Convert.ToString(sem).Trim() == "8")
                {
                    feecetsemyear = "7 Year";
                }
                else if (Convert.ToString(sem).Trim() == "9" || Convert.ToString(sem).Trim() == "10")
                {
                    feecetsemyear = "5 Year";
                }
            }
            else
            {
                feecetsemyear = "" + sem.Trim() + " Semester";
            }
            string feecategory = d2.GetFunction("SELECT * FROM TEXTVALTABLE WHERE textcriteria='FEECA' and college_code=" + collegecode + " and textval='" + feecetsemyear + "'");
            if (feecategory.Trim() == "" || feecategory.Trim() == "0")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Update The Parameter In Fee Category";
                return;
            }
            bool isPercentage = false;
            if (rblHrDaywise.SelectedIndex == 0)
            {
                if (rblPercDays.SelectedIndex == 0)
                {
                    isPercentage = false;
                }
                else
                {
                    isPercentage = true;
                }
            }
            string headerid = string.Empty;
            string ledgerid = string.Empty;
            string getcoursetype = "select distinct isnull(Ltrim(rtrim(c.type)),'') as type,c.Edu_Level from Degree d,Course c where d.Course_Id=c.Course_Id " + qryedudegree + " and isnull(Ltrim(rtrim(c.type)),'')='" + stream + "'";
            DataSet dscourtype = d2.select_method_wo_parameter(getcoursetype, "Text");
            string typval = string.Empty;
            string qryType = string.Empty;
            string qryEdulevel = string.Empty;
            string eduLevelVal = string.Empty;
            ArrayList arrType = new ArrayList();
            if (dscourtype.Tables.Count > 0 && dscourtype.Tables[0].Rows.Count > 0)
            {
                string edulevel = string.Empty;
                string type = string.Empty;
                for (int edu = 0; edu < dscourtype.Tables[0].Rows.Count; edu++)
                {
                    edulevel = Convert.ToString(dscourtype.Tables[0].Rows[edu]["Edu_Level"]).Trim();
                    type = Convert.ToString(dscourtype.Tables[0].Rows[edu]["type"]).Trim();
                    if (!string.IsNullOrEmpty(edulevel.Trim()))
                    {
                        string strattramquery1 = "select * from Condonation_Fee where college_code='" + collegecode + "' and edu_level='" + edulevel + "' and Type='" + type + "'   and isnull(isDays,'0')='" + isPercentage + "'";
                        DataSet dsrange1 = d2.select_method_wo_parameter(strattramquery1, "Text");
                        if (dsrange1.Tables[0].Rows.Count == 0)
                        {
                            errmsg.Visible = true;
                            errmsg.Text = "Please Set Condonation Fee Settings to (" + type + " - " + edulevel + " ) " + ((isPercentage) ? "Days" : "Perentages") + " And Then Proceed";
                            return;
                        }
                        if (string.IsNullOrEmpty(typval.Trim()))
                        {
                            eduLevelVal = "'" + edulevel.Trim() + "'";
                        }
                        else
                        {
                            eduLevelVal += ",'" + edulevel.Trim() + "'";
                        }
                    }
                    if (!string.IsNullOrEmpty(type.Trim()))
                    {
                        if (!arrType.Contains(type.Trim()))
                        {
                            if (string.IsNullOrEmpty(typval.Trim()))
                            {
                                typval = "'" + type.Trim() + "'";
                            }
                            else
                            {
                                typval += ",'" + type.Trim() + "'";
                            }
                            arrType.Add(type.Trim());
                        }
                    }
                }
                if (!string.IsNullOrEmpty(eduLevelVal.Trim()))
                {
                    qryEdulevel = " and edu_level in(" + eduLevelVal.Trim() + ")";
                }
                if (!string.IsNullOrEmpty(typval.Trim()))
                {
                    qryType = " and isnull(Ltrim(rtrim(Type)),'') in(" + typval.Trim() + ")";
                }
            }
            string strattramquery = "select * from Condonation_Fee where college_code='" + collegecode + "' " + qryEdulevel + qryType + " and isnull(isDays,'0')='" + isPercentage + "'";
            DataSet dsrange = d2.select_method_wo_parameter(strattramquery, "Text");
            if (dsrange.Tables[0].Rows.Count == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Set Condonation Fee Settings to " + ((isPercentage) ? "Days" : "Perentages") + " And Then Proceed";
                return;
            }
            strinsupdaval = "delete e from Eligibility_list e,Registration r where e.Roll_no=r.Roll_No and e.batch_year=r.Batch_Year and e.degree_code=r.degree_code and e.Semester=r.Current_Semester " + secval + " and r.batch_year='" + batchyear + "' " + qryBranch + " and r.current_semester='" + sem + "'";
            insupdval = d2.update_method_wo_parameter(strinsupdaval, "Text");
            for (int r = 1; r < Showgrid.Rows.Count; r++)
            {
                if (dicstudetails.ContainsKey(r))
                {
                    string details = dicstudetails[r];
                    string[] split = details.Split('-');
                    string rollno = Convert.ToString(split[0]).Trim();

                    if (!string.IsNullOrEmpty(rollno))
                    {
                        string appno = Convert.ToString(split[1]).Trim();
                        string stuname = Showgrid.Rows[r].Cells[colcnt].Text.ToString();
                        string degcode = Convert.ToString(split[2]).Trim();
                        string batch = Convert.ToString(split[3]).Trim();
                        string sem1 = Convert.ToString(split[4]).Trim();

                        string presentDays = Convert.ToString(split[12]).Trim();
                        string absentDays = Convert.ToString(split[14]).Trim();
                        string workingDays = Convert.ToString(split[10]).Trim();
                        string presentHours = Convert.ToString(split[11]).Trim();
                        string absentHours = Convert.ToString(split[13]).Trim();
                        string workingHours = Convert.ToString(split[9]).Trim();


                        string dayWisePresentPercentage = Convert.ToString(split[8]).Trim();
                        string dayWiseAbsentPercentage = Convert.ToString(split[6]).Trim();
                        string HourWisePresentPercentage = Convert.ToString(split[7]).Trim();
                        string HourWiseAbsentPercentage = Convert.ToString(split[5]).Trim();


                        strinsupdaval = " if not exists(select * from Eligibility_list where Semester='" + sem1 + "' and app_no='" + appno + "' and degree_code='" + degcode + "' and batch_year='" + batch + "') insert into Eligibility_list(batch_year,degree_code,Semester,Roll_no,app_no,stud_name,is_eligible,presentDays,absentDays,workingDays,dayWisePresentPercentage,dayWiseAbsentPercentage,presentHours,absentHours,workingHours,HourWisePresentPercentage,HourWiseAbsentPercentage) values('" + batch + "','" + degcode + "','" + sem1 + "','" + rollno + "','" + appno + "','" + stuname + "','1','" + presentDays + "','" + absentDays + "','" + workingDays + "','" + dayWisePresentPercentage + "','" + dayWiseAbsentPercentage + "','" + presentHours + "','" + absentHours + "','" + workingHours + "','" + HourWisePresentPercentage + "','" + HourWiseAbsentPercentage + "')";
                        insupdval = d2.update_method_wo_parameter(strinsupdaval, "Text");
                    }
                }
            }
            for (int r = 1; r < Showgrid1.Rows.Count; r++)
            {
                if (dicstudApplyCondonation.ContainsKey(r))
                {
                    string stdapplycon = dicstudApplyCondonation[r];
                    string[] split1 = stdapplycon.Split('$');
                    string rollno = Convert.ToString(split1[0]).Trim();
                    if (!string.IsNullOrEmpty(rollno))
                    {
                        string feeamnt = "";
                        string app_no = Convert.ToString(split1[3]).Trim();


                        string stuname = Convert.ToString(Showgrid1.Rows[r].Cells[colcnt1].Text).Trim();

                        if (rblCondType.SelectedIndex == 0)
                            feeamnt = Convert.ToString(Showgrid1.Rows[r].Cells[colcnt1 + 3].Text).Trim();


                        string degcode = Convert.ToString(split1[4]).Trim();
                        string batch = Convert.ToString(split1[5]).Trim();
                        string sem1 = Convert.ToString(split1[6]).Trim();
                        string headerID = Convert.ToString(split1[1]).Trim();
                        string ledgerID = Convert.ToString(split1[2]).Trim();
                        string Remarks = Convert.ToString(dataApplyCondonation.Rows[r][dataApplyCondonation.Columns.Count - 1]).Trim();

                        string presentDays = Convert.ToString(split1[15]).Trim();
                        string absentDays = Convert.ToString(split1[17]).Trim();
                        string workingDays = Convert.ToString(split1[13]).Trim();

                        string presentHours = Convert.ToString(split1[14]).Trim();
                        string absentHours = Convert.ToString(split1[16]).Trim();
                        string workingHours = Convert.ToString(split1[12]).Trim();


                        string dayWisePresentPercentage = Convert.ToString(split1[11]).Trim();
                        string dayWiseAbsentPercentage = Convert.ToString(split1[9]).Trim();
                        string HourWisePresentPercentage = Convert.ToString(split1[10]).Trim();
                        string HourWiseAbsentPercentage = Convert.ToString(split1[8]).Trim();

                        strinsupdaval = " if not exists(select * from Eligibility_list where Semester='" + sem1 + "' and app_no='" + app_no + "' and degree_code='" + degcode + "' and batch_year='" + batch + "') insert into Eligibility_list(batch_year,degree_code,Semester,Roll_no,app_no,stud_name,is_eligible,fine_amt,Remarks,presentDays,absentDays,workingDays,dayWisePresentPercentage,dayWiseAbsentPercentage,presentHours,absentHours,workingHours,HourWisePresentPercentage,HourWiseAbsentPercentage) values('" + batch + "','" + degcode + "','" + sem1 + "','" + rollno + "','" + app_no + "','" + stuname + "','2','" + feeamnt + "','" + Remarks + "','" + presentDays + "','" + absentDays + "','" + workingDays + "','" + dayWisePresentPercentage + "','" + dayWiseAbsentPercentage + "','" + presentHours + "','" + absentHours + "','" + workingHours + "','" + HourWisePresentPercentage + "','" + HourWiseAbsentPercentage + "')";
                        insupdval = d2.update_method_wo_parameter(strinsupdaval, "Text");
                        //strinsupdaval = "if exists (select * from FT_FeeAllot where LedgerFK ='" + ledgerID + "' and HeaderFK ='" + headerID + "' and FeeCategory='" + feecategory + "' and  FinYearFK='" + currfinacialyear + "' and App_No='" + app_no + "') update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',FeeAmount='" + feeamnt + "',TotalAmount='" + feeamnt + "',PayMode='0',PayStartDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',PaidStatus='0',BalAmount='" + feeamnt + "' where LedgerFK='" + ledgerID + "' and HeaderFK='" + headerID + "' and FeeCategory='" + feecategory + "' and  FinYearFK='" + currfinacialyear + "' and App_No='" + app_no + "' ";
                        //strinsupdaval = strinsupdaval + " else  INSERT INTO FT_FeeAllot(AllotDate,App_No,LedgerFK,HeaderFK,FeeAmount,TotalAmount,PayMode,FeeCategory,PayStartDate,PaidStatus,paidamount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "'," + app_no + ",'" + ledgerID + "','" + headerID + "','" + feeamnt + "','" + feeamnt + "','0','" + feecategory + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','0','0','" + feeamnt + "','" + currfinacialyear + "')";
                        //insupdval = d2.update_method_wo_parameter(strinsupdaval, "Text");
                    }
                }
            }
            for (int r = 1; r < Showgrid2.Rows.Count; r++)
            {
                if (dicstudnotelg.ContainsKey(r))
                {
                    string stdnotelg = dicstudnotelg[r];
                    string[] split2 = stdnotelg.Split('-');

                    string rollno = Convert.ToString(split2[0]).Trim();


                    if (!string.IsNullOrEmpty(rollno))
                    {
                        string appno = Convert.ToString(split2[1]).Trim();
                        string stuname = Convert.ToString(Showgrid2.Rows[r].Cells[colcnt3 + 1].Text).Trim(); ;
                        string degcode = Convert.ToString(split2[2]).Trim();
                        string batch = Convert.ToString(split2[3]).Trim();
                        string sem1 = Convert.ToString(split2[4]).Trim();
                        string Remarks = Convert.ToString(Showgrid2.Rows[r].Cells[Showgrid2.Columns.Count - 1].Text).Trim();
                        DropDownList elg = (DropDownList)Showgrid2.Rows[r].Cells[1].FindControl("ddl_Eligible");
                        //  Showgrid2.Rows[r].FindControl("ddl_Eligible")
                        string g = elg.Text;
                        string stucatogery = Convert.ToString(Showgrid2.Rows[r].Cells[1].Text).Trim();

                        int stucatogeryVal = 3;
                        if (!string.IsNullOrEmpty(stucatogery) && stucatogery.Trim() != "")
                        {
                            stucatogeryVal = Convert.ToString(Showgrid2.Rows[r - 1].Cells[1].Text).Trim().ToUpper() == "NOT ELIGIBLE" ? 3 : 4;
                        }

                        string presentDays = Convert.ToString(split2[12]).Trim();
                        string absentDays = Convert.ToString(split2[14]).Trim();
                        string workingDays = Convert.ToString(split2[10]).Trim();
                        string presentHours = Convert.ToString(split2[11]).Trim();
                        string absentHours = Convert.ToString(split2[13]).Trim();
                        string workingHours = Convert.ToString(split2[9]).Trim();


                        string dayWisePresentPercentage = Convert.ToString(split2[8]).Trim();
                        string dayWiseAbsentPercentage = Convert.ToString(split2[6]).Trim();
                        string HourWisePresentPercentage = Convert.ToString(split2[7]).Trim();
                        string HourWiseAbsentPercentage = Convert.ToString(split2[5]).Trim();

                        strinsupdaval = " if not exists(select * from Eligibility_list where Semester='" + sem1 + "' and app_no='" + appno + "' and degree_code='" + degcode + "' and batch_year='" + batch + "') insert into Eligibility_list(batch_year,degree_code,Semester,Roll_no,app_no,stud_name,is_eligible,Remarks,presentDays,absentDays,workingDays,dayWisePresentPercentage,dayWiseAbsentPercentage,presentHours,absentHours,workingHours,HourWisePresentPercentage,HourWiseAbsentPercentage) values('" + batch + "','" + degcode + "','" + sem1 + "','" + rollno + "','" + appno + "','" + stuname + "','" + stucatogeryVal + "','" + Remarks + "','" + presentDays + "','" + absentDays + "','" + workingDays + "','" + dayWisePresentPercentage + "','" + dayWiseAbsentPercentage + "','" + presentHours + "','" + absentHours + "','" + workingHours + "','" + HourWisePresentPercentage + "','" + HourWiseAbsentPercentage + "')";
                        insupdval = d2.update_method_wo_parameter(strinsupdaval, "Text");
                    }
                }
            }
            btngo_Click(sender, e);
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = Convert.ToString(ex);
        }
    }

    protected void GvUsersRowDataBound(object sender, GridViewRowEventArgs eventArgs)
    {
        try
        {


            if (eventArgs.Row.RowType == DataControlRowType.Header)
            {

                if (rblCOndonationType.SelectedIndex == 0)
                {
                    if (rblDayOrPercerntage.SelectedIndex == 0)
                    {
                        eventArgs.Row.Cells[3].Visible = false;
                        eventArgs.Row.Cells[4].Visible = false;
                    }
                    else
                    {
                        eventArgs.Row.Cells[1].Visible = false;
                        eventArgs.Row.Cells[2].Visible = false;

                    }

                }
                else
                {
                    if (rblDayOrPercerntage.SelectedIndex == 0)
                    {
                        eventArgs.Row.Cells[3].Visible = false;
                        eventArgs.Row.Cells[4].Visible = false;
                        eventArgs.Row.Cells[5].Visible = false;
                    }
                    else
                    {

                        eventArgs.Row.Cells[1].Visible = false;
                        eventArgs.Row.Cells[2].Visible = false;
                        eventArgs.Row.Cells[5].Visible = false;

                    }

                }
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                Table table = (Table)grdAddcondonationfees.Controls[0];
                TableRow headerow = table.Rows[0];
                for (int i = 0; i < 1; i++)
                {
                    TableCell headerCell = headerow.Cells[0];
                    headerow.Cells.RemoveAt(0);
                    HeaderGridRow.Cells.Add(headerCell);
                    headerCell.RowSpan = 2;
                }
                TableCell HeaderCell1 = new TableCell();

                HeaderCell1.Text = "Attendance Percentage";
                HeaderCell1.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell1.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell1);


                grdAddcondonationfees.Controls[0].Controls.AddAt(0, HeaderGridRow);
            }

            if (eventArgs.Row.RowType == DataControlRowType.DataRow)
            {
                if (rblCOndonationType.SelectedIndex == 0)
                {
                    if (rblDayOrPercerntage.SelectedIndex == 0)
                    {
                        eventArgs.Row.Cells[3].Visible = false;
                        eventArgs.Row.Cells[4].Visible = false;
                    }
                    else
                    {
                        eventArgs.Row.Cells[1].Visible = false;
                        eventArgs.Row.Cells[2].Visible = false;

                    }

                }
                else
                {
                    if (rblDayOrPercerntage.SelectedIndex == 0)
                    {
                        eventArgs.Row.Cells[3].Visible = false;
                        eventArgs.Row.Cells[4].Visible = false;
                        eventArgs.Row.Cells[5].Visible = false;
                    }
                    else
                    {
                        eventArgs.Row.Cells[1].Visible = false;
                        eventArgs.Row.Cells[2].Visible = false;
                        eventArgs.Row.Cells[5].Visible = false;

                    }

                }
            }

        }
        catch
        {

        }

    }

    #endregion

    #region Print Excel

    #region Print To Write Exam

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text.Trim().Replace(" ", "_");
            if (Convert.ToString(reportname).Trim() != "")
            {
                d2.printexcelreportgrid(Showgrid, reportname);
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name For Write Exam";
                errmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = Convert.ToString(ex);
        }
    }

    #endregion  Print To Write Exam

    #region Print To Condonation

    protected void btnconxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtconexcel.Text.Trim().Replace(" ", "_");
            //  FpSpread2.Sheets[0].Columns[12].Visible = false;
            if (Convert.ToString(reportname).Trim() != "")
            {
                d2.printexcelreportgrid(Showgrid1, reportname);
             
                // FpSpread2.Sheets[0].Columns[12].Visible = true;
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name For Condonation";
                errmsg.Visible = true;
            }
            // FpSpread2.Sheets[0].Columns[12].Visible = true;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = Convert.ToString(ex);
        }
    }

    #endregion Print To Condonation

    #region Print To Not Eligible

    protected void btnnoteliexcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtnoteliexcel.Text.Trim().Replace(" ", "_");
            if (Convert.ToString(reportname).Trim() != "")
            {
                d2.printexcelreportgrid(Showgrid2, reportname);
               
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name For Not Eligible";
                errmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = Convert.ToString(ex);
        }
    }

    #endregion  Print To Not Eligible

    #endregion Print Excel

    #region Print PDF

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string ss=null;
            PRINTPDF1.Visible = false;
            PRINTPDF2.Visible = false;
            string Details = BindName();
            string degreedetails = "Condonation Report To Write Exam " + '@' + Details;
            string pagename = "condonation.aspx";
            Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public string BindName()
    {
        string Details = string.Empty;
        try
        {
            string DeptName = string.Empty;
            string DegreeName = string.Empty;
            string sections = string.Empty;
            int Baranchcount = 0;
            int DegreeCount = 0;
            int SectionCount = 0;

            if (cblBranch.Items.Count > 0)
            {
                for (int intch = 0; intch < cblBranch.Items.Count; intch++)
                {
                    if (cblBranch.Items[intch].Selected == true)
                    {
                        Baranchcount++;
                        DeptName = Convert.ToString(cblBranch.Items[intch].Text);
                        if (Baranchcount > 1)
                        {
                            break;
                        }
                    }
                }
            }
            if (cblDegree.Items.Count > 0)
            {
                for (int intch = 0; intch < cblDegree.Items.Count; intch++)
                {
                    if (cblDegree.Items[intch].Selected == true)
                    {
                        DegreeCount++;
                        DegreeName = Convert.ToString(cblDegree.Items[intch].Text);
                        if (DegreeCount > 1)
                        {
                            break;
                        }
                    }
                }
            }
            if (cblSec.Items.Count > 0)
            {
                for (int intch = 0; intch < cblSec.Items.Count; intch++)
                {
                    if (cblSec.Items[intch].Selected == true)
                    {
                        SectionCount++;
                        sections = Convert.ToString(cblSec.Items[intch].Text);
                        if (SectionCount > 1)
                        {
                            break;
                        }
                    }
                }
                if (sections.Trim() == "Empty")
                {
                    sections = "";
                }
                else
                {
                    sections = "- Sec-" + sections;
                }
            }
            string selectedBatchYears = Convert.ToString(getCblSelectedText(chklsbatch));
            if (DegreeCount == 1 && SectionCount == 1 && Baranchcount == 1)
            {
                Details = "Class & Group : " + selectedBatchYears.ToString() + '-' + DegreeName.ToString() + '-' + DeptName.ToString() + '-' + "Sem-" + ddlsemester.SelectedItem.ToString() + sections + '@' + "Period               : " + txtfdate.Text.ToString() + " to " + txttodate.Text.ToString() + " ";
            }


        }
        catch
        {

        }
        return Details;
    }

    protected void btnconprint_Click(object sender, EventArgs e)
    {
        try
        {
            string ss = null;
            Printcontrol.Visible = false;
            PRINTPDF2.Visible = false;
            // FpSpread2.Sheets[0].Columns[12].Visible = false;
            string Details = BindName();
            string degreedetails = "Condonation Student Report " + '@' + Details;
            string pagename = "condonation.aspx";
            PRINTPDF1.loadspreaddetails(Showgrid1, pagename, degreedetails, 0, ss);
            //  FpSpread2.Sheets[0].Columns[12].Visible = true;
            PRINTPDF1.Visible = true;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void btnnoteliprint_Click(object sender, EventArgs e)
    {
        try
        {
            string ss = null;
            string Details = BindName();
            string degreedetails = "Not Eligible Student Report " + '@' + Details;
            string pagename = "condonation.aspx";
            PRINTPDF2.loadspreaddetails(Showgrid2, pagename, degreedetails, 0, ss);
            PRINTPDF2.Visible = true;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    #endregion Print PDF

    //Added by Idhris 15-10-2016 for Condonation Fees and Daywise - Hourwise percentage
    protected void rblHrDaywise_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        if (rblHrDaywise.SelectedIndex == 0)
        {
            rblPercDays.Visible = true;
            lblPercDays.Visible = true;
        }
        else
        {
            rblPercDays.Visible = false;
            lblPercDays.Visible = false;
        }
    }

    protected void rblPercDays_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void rblCondType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void rblCOndonationType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clear2();
        if (rblCOndonationType.SelectedIndex == 0)
        {
            lblheader.Visible = true;
            ddlheader.Visible = true;
            lblledger.Visible = true;
            ddlledger.Visible = true;
            //phRow2.Controls.Add(btnpgo);
        }
        else
        {
            lblheader.Visible = false;
            ddlheader.Visible = false;
            lblledger.Visible = false;
            ddlledger.Visible = false;
            //phRow1.Controls.Add(btnpgo);
        }
    }

    // Added By Malang Raja On Oct 17 2016 for Show Present,Absent Details
    protected void chkShowDetails_CheckedChanged(object sender, EventArgs e)
    {
        clear();
    }

    //  Added By Malang Raja On Oct 17 2016 for Condonation Fee Settings Day or Percentage
    protected void rblDayOrPercerntage_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clear2();
    }

    #region Column Order

    #region Added By Malang Raja on Oct 20 2016

    protected void chkColumnOrderAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkColumnOrderAll.Checked == true)
            {
                txtOrder.Text = string.Empty;
                ItemList.Clear();
                Itemindex.Clear();
                for (int i = 0; i < cblColumnOrder.Items.Count; i++)
                {
                    string si = Convert.ToString(i).Trim();
                    cblColumnOrder.Items[i].Selected = true;
                    lbtnRemoveAll.Visible = true;
                    ItemList.Add(Convert.ToString(cblColumnOrder.Items[i].Text).Trim());
                    Itemindex.Add(si);
                }
                lbtnRemoveAll.Visible = true;
                txtOrder.Visible = true;
                txtOrder.Text = string.Empty;
                int j = 0;
                string colname12 = string.Empty;
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                    {
                        colname12 = Convert.ToString(ItemList[i]).Trim() + "(" + Convert.ToString(j).Trim() + ")";
                    }
                    else
                    {
                        colname12 = colname12 + "," + Convert.ToString(ItemList[i]).Trim() + "(" + Convert.ToString(j).Trim() + ")";
                    }
                }
                txtOrder.Text = colname12;
            }
            else
            {
                ItemList.Clear();
                Itemindex.Clear();
                for (int i = 0; i < cblColumnOrder.Items.Count; i++)
                {
                    cblColumnOrder.Items[i].Selected = false;
                }
                lbtnRemoveAll.Visible = false;
                txtOrder.Text = string.Empty;
                txtOrder.Visible = false;
            }
            txtOrder.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void lbtnRemoveAll_Click(object sender, EventArgs e)
    {
        try
        {
            cblColumnOrder.ClearSelection();
            chkColumnOrderAll.Checked = false;
            lbtnRemoveAll.Visible = false;
            ItemList.Clear();
            Itemindex.Clear();
            txtOrder.Text = string.Empty;
            txtOrder.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void cblColumnOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            chkColumnOrderAll.Checked = false;
            string value = string.Empty;
            int index;
            //cblColumnOrder.Items[0].Selected = true;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index).Trim();
            if (cblColumnOrder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    ItemList.Add(Convert.ToString(cblColumnOrder.Items[index].Text).Trim());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(Convert.ToString(cblColumnOrder.Items[index].Text).Trim());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblColumnOrder.Items.Count; i++)
            {
                if (cblColumnOrder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i).Trim();
                    ItemList.Remove(Convert.ToString(cblColumnOrder.Items[i].Text).Trim());
                    Itemindex.Remove(sindex);
                }
            }
            lbtnRemoveAll.Visible = true;
            txtOrder.Visible = false;
            txtOrder.Text = string.Empty;
            string colname12 = string.Empty;
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = Convert.ToString(ItemList[i]).Trim() + "(" + Convert.ToString((i + 1)).Trim() + ")";
                }
                else
                {
                    colname12 = colname12 + "," + Convert.ToString(ItemList[i]).Trim() + "(" + Convert.ToString((i + 1)).Trim() + ")";
                }
            }
            txtOrder.Text = colname12;
            if (ItemList.Count == 14)
            {
                chkColumnOrderAll.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                txtOrder.Visible = false;
                lbtnRemoveAll.Visible = false;
            }
            txtOrder.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #endregion

    #region Condonation Report Added By Malang Raja on Oct 21 2016

    protected void btnCondonationReport_Click(object sender, EventArgs e)
    {
        lblErrCondo.Text = string.Empty;
        lblErrCondo.Visible = false;

        bool status = false;
        contentDiv.InnerHtml = "";
        StringBuilder html = new StringBuilder();
        if (ddlCondonationReport.SelectedValue == "0")
        {
            try
            {


                if (Showgrid1.Rows.Count > 1)
                {
                    int selected = 0;
                    int val = 0;
                    foreach (GridViewRow gvrow in Showgrid1.Rows)
                    {
                        System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");

                        if (chk != null && chk.Checked == true)
                        {
                            selected++;
                        }
                    }
                    if (selected == 0)
                    {
                        lblErrCondo.Visible = true;
                        lblErrCondo.Text = "Please Select Atleast One Student And Then Proceed";
                        return;
                    }
                    else
                    {
                        string strquery = "select collname+' ('+category+')' as collegeName,university,affliatedby,acr,address3,pincode,district,district+' - '+pincode  as districtpin from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                        DataSet dsCollegeDetail = d2.select_method_wo_parameter(strquery, "Text");
                        string Collegename = string.Empty;
                        string aff = string.Empty;
                        string collacr = string.Empty;
                        string dispin = string.Empty;
                        string clgaddress = string.Empty;
                        string univ = string.Empty;
                        string pincode = string.Empty;
                        if (dsCollegeDetail.Tables.Count > 0 && dsCollegeDetail.Tables[0].Rows.Count > 0)
                        {
                            Collegename = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["collegeName"]).Trim();
                            aff = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["affliatedby"]).Trim();
                            univ = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["university"]).Trim();
                            string[] strpa = aff.Split(',');
                            aff = "( " + univ + " " + strpa[0] + " )";
                            collacr = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["acr"]).Trim();
                            pincode = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["pincode"]).Trim();
                            pincode = pincode.Substring(pincode.Length - 3);
                            int pin = 0;
                            int.TryParse(pincode, out pin);
                            clgaddress = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["address3"]).Trim() + " , " + Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["district"]).Trim() + ((pin != 0) ? (" - " + Convert.ToString(pin).Trim()) : " - " + pincode);
                            //clgaddress = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["address3"]);
                            dispin = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["districtpin"]).Trim();
                        }
                        for (int re = 1; re < Showgrid1.Rows.Count; re++)
                        {
                            System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)Showgrid1.Rows[re].FindControl("selectchk");
                            if (chk != null && chk.Checked == true)
                            {

                                string stdapplycon = dicstudApplyCondonation[re];
                                string[] split1 = stdapplycon.Split('$');
                                string rollno = Convert.ToString(split1[0]).Trim();

                                string rollNo = Convert.ToString(split1[0]).Trim();
                                string regNo = Convert.ToString(split1[17]).Trim();
                                string studentName = Convert.ToString(Showgrid1.Rows[re].Cells[colcnt1].Text).Trim();
                                string degreeDeatils = Convert.ToString(split1[7]).Trim();
                                string semester = Convert.ToString(split1[6]).Trim();
                                status = true;

                                html.Append("<center> <div style='height: 990px; width: 100%; border: 0px solid black; margin-left: 5px; margin: 0px; page-break-after: always;'> <center><div style='border: 0px solid black'>  <center>");

                                html.Append("<table cellspacing='0' cellpadding='0' style='width: 95%; ' border='0'>");

                                html.Append("<tr><td style='width: 50px;'></td><td style='text-align: left;' > <img src=~/college/Left_Logo(" + Convert.ToString(Session["collegecode"]) + ").jpeg alt='' style='height: 100px; width: 120px;' /></td><td style='font-size: 12px; font-family: Book Antiqua;  border: 0px solid black; text-align: center;'><span style='font-size: 14px;font-weight:bold;'>" + Collegename + "</span> <br><span style='font-size: 14px;font-weight:bold;'>APPLICATION FOR CONDONATIOIN OF SHORTAGE OF ATTENDANCE</span></td><td style='text-align: right;' ></td></tr><tr><td style='width: 50px;'></td><td></td><td ></td><td style='text-align: right;' ><span style='font-size: 14px;font-weight:bold;'> </span></td></tr> ");

                                html.Append(" </table></center>");
                                html.Append("<table style='width: 95%; margin-top: 1px; margin-bottom: 2px; font-size: medium;'cellpadding='5' cellspacing='0'> <tr><td  style='text-align: left; width: auto;'>MONTH & YEAR : </td><td></td><td></td><td style='text-align: right; width: auto;'>SEMESTER :" + ToRoman(semester) + " </td> </tr></table>");
                                html.Append("<center> <table style='width: 95%; margin-top: 1px; margin-bottom: 2px; font-size: medium;'cellpadding='5' cellspacing='0'> <tr><td  style='text-align: left; width: auto;'>NAME : </td><td></td><td></td><td style='text-align: right; width: auto;'>CLASS & GROUP :<br />" + degreeDeatils + " </td> </tr></table></center>");

                                html.Append(" <br /><table style='width: 95%; margin-top: 1px; margin-bottom: 2px; font-size: medium;'cellpadding='5' cellspacing='0'> <tr><td  style='text-align: left; width: auto;'>TOTAL NUMBER OF WORKING DAYS</td><td>:</td><td></td><td style='text-align: right; width: auto;'>90 DAYS</td> </tr><tr><td  style='text-align: left; width: auto;'>MAX.NO OF DAYS OF ABSENCE PERMITTED</td><td>:</td><td></td><td style='text-align: right; width: auto;'>22.5 DAYS</td> </tr></tr><tr><td  style='text-align: left; width: auto;'>DETAILS OF ATTENDANCE</td><td>:</td><td></td><td style='text-align: right; width: auto;'></td> </tr><tr><td  style='text-align: left; width: auto;'>ABSENT WITH LEAVE </td><td>:</td><td></td><td style='text-align: right; width: auto;'>-------------------- DAYS</td> </tr><tr><td  style='text-align: left; width: auto;'>ABSENT WITHOUT LEAVE  </td><td>:</td><td></td><td style='text-align: right; width: auto;'>-------------------- DAYS</td> </tr><tr><td  style='text-align: left; width: auto;'>TOTAL ABSENT  </td><td>:</td><td></td><td style='text-align: right; width: auto;'>-------------------- DAYS</td> </tr></table>");



                                html.Append("<br /><table style='width: 95%; margin-top: 1px; margin-bottom: 2px; font-size: medium;'cellpadding='5' cellspacing='0'> <tr><td  style='text-align: left; width: auto;'>DATE OF APPLICATON :SINGNATURE OF STUDENT </td></tr><tr><td></td></tr><tr><td style='border-bottom: thin solid #000000; ' ></td></tr><tr><td  style='text-align: left; width: auto;'>ELIGIBLE FOR CONDONATION </td></tr><tr><td></td></tr><tr><td  style='text-align: left; width: auto;'>DEAN OF STUDENT AFFAIRS </td></tr><tr><td style='border-bottom: thin solid #000000; ' ></td></tr><tr><td  style='text-align: left; width: auto;'>RECOMMENDED FOR CONDONATION </td></tr><tr><td></td></tr><tr><td  style='text-align: left; width: auto;'>HEAD OF THE DEPARTMENT </td></tr><tr><td style='border-bottom: thin solid #000000; ' ></td></tr><tr><td  style='text-align: left; width: auto;'><b>CONDONATION GRANTED /NOT GRANTED </b></td></tr><tr><td></td></tr><tr><td  style='text-align: left; width: auto;'><b>PRINCIPAL</b> </td></tr><tr><td style='border-bottom: thin solid #000000; ' ></td></tr><tr><td  style='text-align: center; width: auto;'><b>(FOR RECORDS OFFICE USE ONLY) </b></td></tr></table>");

                                html.Append("<table style='width: 95%; margin-top: 1px; margin-bottom: 2px; font-size: medium;'cellpadding='5' cellspacing='0'> <tr><td  style='text-align: left; width: auto;'><b>DETAILS OF FEES PAID  : </b></td></tr><tr><td  style='text-align: left; width: auto;'><b>RECEIPT NO. :</b></td></tr><tr><td  style='text-align: left; width: auto;'><b>AMOUNT PAID : </b></td></tr><tr><td  style='text-align: left; width: auto;'><b>DATE  :</b></td></tr></table>");



                                html.Append("</div></center></div></center>");

                            }
                        }
                    }
                }
                else
                {
                    lblErrCondo.Visible = true;
                    lblErrCondo.Text = "No Record(s) Found";
                    return;
                }
                if (status)
                {
                    contentDiv.InnerHtml = html.ToString();
                    contentDiv.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "btnprint", "PrintDiv();", true);
                    Showgrid1.Visible = true;
                }
            }
            catch (Exception ex)
            {
            }
        }
        else
        {
            if (Showgrid1.Rows.Count > 1)
            {


                int selected = 0;
                int val = 0;
                foreach (GridViewRow gvrow in Showgrid1.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("selectchk");
                    if (chk != null && chk.Checked == true)
                    {
                        selected++;
                    }
                }
                if (selected == 0)
                {
                    lblErrCondo.Visible = true;
                    lblErrCondo.Text = "Please Select Atleast One Student And Then Proceed";
                    return;
                }
                else
                {
                    string strquery = "select collname,college_code,category,university,affliatedby,acr,address3,pincode,district,district+' - '+pincode  as districtpin,logo1 from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                    DataSet dsCollegeDetail = d2.select_method_wo_parameter(strquery, "Text");
                    string Collegename = string.Empty;
                    string aff = string.Empty;
                    string collacr = string.Empty;
                    string dispin = string.Empty;
                    string clgaddress = string.Empty;
                    string univ = string.Empty;
                    string pincode = string.Empty;
                    string catogery = string.Empty;
                    string Accredited = string.Empty;
                    string affiliated = string.Empty;

                    if (dsCollegeDetail.Tables.Count > 0 && dsCollegeDetail.Tables[0].Rows.Count > 0)
                    {
                        Collegename = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["collname"]).Trim();
                        catogery = "(" + Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["category"]).Trim() + ")";
                        aff = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["affliatedby"]).Trim();
                        univ = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["university"]).Trim();
                        string[] strpa = aff.Split(',');
                        aff = strpa[0];
                        Accredited = strpa[1];
                        affiliated = strpa[2];
                        collacr = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["acr"]).Trim();
                        pincode = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["pincode"]).Trim();
                        pincode = pincode.Substring(pincode.Length - 3);
                        int pin = 0;
                        int.TryParse(pincode, out pin);
                        clgaddress = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["address3"]).Trim() + " , " + Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["district"]).Trim() + ((pin != 0) ? (" - " + Convert.ToString(pin).Trim()) : " - " + pincode);
                        //clgaddress = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["address3"]);
                        dispin = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["districtpin"]).Trim();


                    }



                    for (int re = 1; re < Showgrid1.Rows.Count; re++)
                    {
                        val = 0;
                        System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)Showgrid1.Rows[re].FindControl("selectchk");
                        if (chk != null && chk.Checked == true)
                        {
                            string stdapplycon = dicstudApplyCondonation[re];
                            string[] split1 = stdapplycon.Split('$');
                            string rollno = Convert.ToString(split1[0]).Trim();

                            string rollNo = Convert.ToString(split1[0]).Trim();
                            string regNo = Convert.ToString(split1[17]).Trim();
                            string studentName = Convert.ToString(Showgrid1.Rows[re].Cells[colcnt1].Text).Trim();
                            string degreeDeatils = Convert.ToString(split1[7]).Trim();
                            string semester = Convert.ToString(split1[6]).Trim();
                            string presentpercent = Convert.ToString(Showgrid1.Rows[re].Cells[colcnt1 + 1].Text).Trim() + " %";

                            string absentpercent = Convert.ToString(Showgrid1.Rows[re].Cells[colcnt1 + 2].Text).Trim();
                            string conducteddays = "";
                            string daysattended = "";
                            string absentdays = "";

                            string batchYear = Convert.ToString(split1[5]).Trim();
                            string degreeCode = Convert.ToString(split1[4]).Trim();
                            int colcount = colcnt1 + 3;
                            Boolean fine = false;
                            if (rblCondType.SelectedIndex == 0)
                            {
                                fine = true;
                                condamount = Convert.ToString(Showgrid1.Rows[re].Cells[colcount].Text).Trim();
                            }
                            if (fine)
                                colcount = colcount + 1;


                            conducteddays = Convert.ToString(Showgrid1.Rows[re].Cells[colcount].Text).Trim() + " Days";
                            daysattended = Convert.ToString(Showgrid1.Rows[re].Cells[colcount + 1].Text).Trim() + " Days";
                            absentdays = Convert.ToString(Showgrid1.Rows[re].Cells[colcount + 2].Text).Trim() + " Days";



                            if (dicCondonationappcnt.ContainsKey(re))
                            {

                                string stdapplycont = dicCondonationappcnt[re];
                                string[] split = stdapplycont.Split('$');

                                conddate = Convert.ToString(split[0]);
                                condchallan = Convert.ToString(split[1]);
                            }
                            string duration = string.Empty;
                            int max_sem1 = 0;
                            if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode))
                            {
                                string max_sem = d2.GetFunctionv("select NDurations from ndegree where batch_year='" + batchYear + "'  and Degree_code='" + degreeCode + "'");
                                if (max_sem == "" || max_sem == null)
                                {
                                    max_sem = d2.GetFunctionv("SELECT Duration FROM Degree where  Degree_Code='" + degreeCode + "'");
                                }
                                int.TryParse(max_sem, out max_sem1);
                            }

                            switch (max_sem1)
                            {
                                case 2:
                                    duration = "1 Year";
                                    break;
                                case 4:
                                    duration = "2 Years";
                                    break;
                                case 6:
                                    duration = "3 Years";
                                    break;
                            }

                            string currentsem = dir.selectScalarString("select Current_Semester from Registration where Reg_No='" + regNo + "'");
                            string currentyr = string.Empty;
                            int studentSemester = 0;

                            //int.TryParse(currentsem, out studentSemester);
                            //currentyr = Convert.ToString((studentSemester % 2) + ((studentSemester % 2) == 0) ? 0 : 1);
                            if (!(currentsem == "" || currentsem == null))
                            {
                                if (currentsem.Trim() == "1" || currentsem.Trim() == "2")
                                    currentyr = "1st Year";
                                if (currentsem.Trim() == "3" || currentsem.Trim() == "4")
                                    currentyr = "2nd Year";
                                if (currentsem.Trim() == "5" || currentsem.Trim() == "6")
                                    currentyr = "3rd Year";
                                if (currentsem.Trim() == "7" || currentsem.Trim() == "8")
                                    currentyr = "4th Year";
                                if (currentsem.Trim() == "9" || currentsem.Trim() == "10")
                                    currentyr = "5th Year";
                            }

                            switch (currentsem.Trim())
                            {
                                case "1":
                                    currentsem = currentsem + "st";
                                    break;
                                case "2":
                                    currentsem = currentsem + "nd";
                                    break;
                                case "3":
                                    currentsem = currentsem + "rd";
                                    break;
                                default:
                                    currentsem = currentsem + "th";
                                    break;
                            }




                            html.Append("<center> <div style='height: 990px; width: 100%; border: 0px solid black; margin-left: 5px; margin: 0px; page-break-after: always;'> <center><div style='border: 0px solid black'>  <center>");

                            html.Append("<table cellspacing='0' cellpadding='0' style='width: 95%; ' border='0'>");
                            html.Append("<tr><td><img src=~/college/Left_Logo(" + Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["college_code"]).Trim() + ").jpeg alt='' style='height: 100px; width: 120px;' /></td><td style='font-size: 12px; font-family: Book Antiqua;  border: 0px solid black; text-align: center;'><span style='font-size: 14px;font-weight:bold;'>" + Collegename + "<br />" + catogery + "<br />" + aff + "<br />" + Convert.ToString(Accredited.Split('\\').Last()) + "<br />" + Convert.ToString(affiliated.Split('\\').Last()) + "<br />" + dispin + "</span></td><td  style='text-align: left; width: auto;'><td><img src=~/college/Left_Logo(" + Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["college_code"]).Trim() + ").jpeg alt='' style='height: 100px; width: 120px;' /></td><td style='font-size: 12px; font-family: Book Antiqua;  border: 0px solid black; text-align: center;'><span style='font-size: 14px;font-weight:bold;'>" + Collegename + "<br />" + catogery + "<br />" + aff + "<br />" + Convert.ToString(Accredited.Split('\\').Last()) + "<br />" + Convert.ToString(affiliated.Split('\\').Last()) + "<br />" + dispin + "</span></td></tr> ");
                            html.Append("</table></center> ");

                            html.Append("<center> <table style='width: 95%; margin-top: 1px; margin-bottom: 2px; font-size: medium;'cellpadding='5' cellspacing='0'> <tr><td colspan='2'  style='text-align: center; width: auto;'>ATTENDANCE CERTIFICATE<hr /></td><td  style='text-align: left; width: 2%;'></td><td colspan='2'  style='text-align: center; width: auto;'>APPLICATION FOR GRANT OF CONDONATION<hr /></td></tr><tr><td  colspan='2'   style='text-align: left; width: auto;'>(Should be sent to the Controller of Examinatons, Jamal Mohamed College atleast \n by 10  days prior to the date of commencement of the Examinations )</td><td style='text-align: left; width: 2%;'></td><td colspan='2' style='text-align: left; width: auto;'>(Only those candidates who fall short of attendance from 26% to 40% of the working days\n need to use this form)</td></tr><tr><td></td></tr><tr><td  style='text-align: left; width: auto;'><b>Name of the Candidate:</b> </td><td>" + studentName + "</td><td  style='text-align: left; width: 2%;'></td><td  style='text-align: left; width: auto;'><b>Name of the Candidate</b>: " + studentName + "</td><td><b>Register No:</b> " + regNo + "</td></tr>  <tr><td  style='text-align: left; width: auto;'><b>Register No:</b>  </td><td>" + regNo + "</td><td  style='text-align: left; width: 2%;'></td><td  style='text-align: left; width: auto;'><b>Course and Subject:</b>" + Convert.ToString(degreeDeatils.Split('-').ElementAt(2)) + " -" + Convert.ToString(degreeDeatils.Split('-').ElementAt(3)) + "-" + Convert.ToString(degreeDeatils.Split('-').ElementAt(4)) + " </td><td><b>Year/Semester:</b> " + currentyr + " / " + currentsem + " Sem  </td></tr><tr><td  style='text-align: left; width: auto;'><b>Class & Main: </b> </td><td>" + Convert.ToString(degreeDeatils.Split('-').ElementAt(2)) + " -" + Convert.ToString(degreeDeatils.Split('-').ElementAt(3)) + "-" + Convert.ToString(degreeDeatils.Split('-').ElementAt(4)) + "</td><td  style='text-align: left; width: 2%;'></td><td  style='text-align: left; width: auto;'><b>Total No. of days/hours the college Worked:</b> </td><td>" + conducteddays + "</td></tr><tr><td  style='text-align: left; width: auto;'><b>Year/Semester: </b>  </td><td>" + currentyr + " / " + currentsem + " Sem </td><td  style='text-align: left; width: 2%;'></td><td  style='text-align: left; width: auto;'><b>No. of days/hours the Candidate attended:</b> </td><td>" + daysattended + "</td></tr><tr><td  style='text-align: left; width: auto;'><b>Period Of The Course: </b></td><td>" + duration + " </td><td  style='text-align: left; width: 2%;'></td><td  style='text-align: left; width: auto;'><b>Actual shortage of attendance: </b></td><td>" + absentpercent + " % </td></tr><tr><td  style='text-align: left; width: auto;'><b>1. Total No. of working days:</b> </td><td>" + conducteddays + " </td><td  style='text-align: left; width: 2%;'></td><td  style='text-align: left; width: auto;'><b>Actual shortage of attendance:</b> </td><td>" + absentpercent + " % </td></tr><tr><td  style='text-align: left; width: auto;'><b>2. No of days attend: </b></td><td>" + daysattended + " </td><td  style='text-align: left; width: 2%;'></td><td  style='text-align: left; width: auto;'><b>Category:</b> </td><td></td></tr><tr><td  style='text-align: left; width: auto;'><b>3.Present Percentage: </b></td><td>" + presentpercent + " </td><td  style='text-align: left; width: 2%;'></td>");

                            if (currentyr.Trim() == "3rd Year")
                            {
                                html.Append("<td  style='text-align: left; width: auto;'><span><b>(i) 26% to 30%    <br/>(ii) 31% to 40% </span></td><td><span>Condonation Fee : 600<br/>Condonation Fee : (600+50)</b></span> </td></tr>");

                            }
                            else
                            {
                                html.Append("<td  style='text-align: left; width: auto;'><span><b>(i) 26% to 35%     <br/>(ii) 36% to 50%</span></td><td><span>Condonation Fee : 600<br/>Condonation Fee : (600+50)</b></span> </td></tr>");

                            }


                            html.Append("<tr><td colspan='2'  style='text-align: left; width: auto;'><b>Signature of Candidate:</b></td><td  style='text-align: left; width: 2%;'></td><td  style='text-align: left; width: auto;'><span><b>Reason for shortage attendance: <br/>(Relevant authentic evidence should be enclosed)</b></span></td><td></td></tr><tr><td colspan='2'  style='text-align: left; width: auto;'><b>Certified that the candidate has earned the required percentage of attendance.</b></td><td  style='text-align: left; width: 2%;'></td><td  style='text-align: left; width: auto;'><b>Condonation Fee :" + condamount + "</b></td><td> </td></tr><tr><td colspan='2'  style='text-align: left; width: auto;'><b>The candidate requires Condonation of attendance.</b></td><td  style='text-align: left; width: 2%;'></td><td></td><td></td></tr><tr><td colspan='2' style='text-align: left; width: auto;'><span><b> The Candidate has not earned the required attenedance and<br/>hence He/She is not permitted to sit for the examination.</b></span></td><td  style='text-align: left; width: 2%;'></td><td  style='text-align: left; width: auto;'><b>Challan :" + condchallan + "</b></td><td><b>Signature of the Candidate </b> </td></tr><tr><td></td><td></td><td  style='text-align: left; width: 2%;'></td><td  style='text-align: left; width: auto;'><b>Sanction by the principal in the case of category</b> </td><td></td></tr><tr><td  style='text-align: left; width: auto;'><b>Whether the attendance is regular: </b></td><td>Satisfaction</td><td  style='text-align: left; width: 2%;'></td><td  style='text-align: left; width: auto;'> <span><b>(i) SPECIFIC RECOMMENDATION of the Registration of \nattendance in the case of category </b> </span></td><td><b>Recommended with Medical Certificate</b> </td></tr><tr><td  style='text-align: left; width: auto;'><b>Conduct & Character:</b> </td><td>Good</td><td  style='text-align: left; width: 2%;'></td><td><span><b>(ii) (The Principal should certify to the genuinity  of the \n reason for absence)</b></span></td><td></td></tr><tr><td colspan='2'  style='text-align: left; width: auto;'><b>NOTE:Strike off whichever is not applicable.</b></td><td  style='text-align: left; width: 2%;'></td><td></td><td></td></tr><tr><td></td><td></td><td  style='text-align: left; width: 2%;'></td><td  style='text-align: left; width: auto; border-bottom: thin solid #000000;'>Date:" + conddate + "</td><td style='width: auto; border-bottom: thin solid #000000; '>Signature of the Principal</td></tr></tr><tr><td  style='text-align: left; width: auto;'><b>Condonation  :</b></td><td>Sanctioned /NonSanctioned </td><td  style='text-align: left; width: 2%;'></td><td colspan='2' style='text-align: left; width: auto;'><b>FOR THE CONTROLLER OF EXAMINATIONS JAMAL MOHAMED COLLEGE\nOFFICE USE ONLY</b></td></tr><tr><td  style='text-align: left; width: auto;'><b>Date:</b>" + conddate + "</td><td><b>Challan :</b>" + condchallan + "</td><td  style='text-align: left; width: 2%;'></td><td  style='text-align: left; width: auto;'><b>Remarks of the section</b></td><td></td></tr></table><br/><br/>");

                            html.Append("<table style='width: 95%; margin-top: 1px; margin-bottom: 2px; font-size: medium;'cellpadding='5' cellspacing='0'> <tr><td  style='text-align: left; width: auto;'><b>Signature of the Registrar</b></td><td><b>Signature of the Principal</b></td><td  style='text-align: left; width: 2%;'></td><td  style='text-align: left; width: auto;'><b>Section Head</b></td><td><b>Order of the Controller \n of Examinations</b></td></tr></table></center>   ");



                            html.Append("</div></center>");
                            status = true;
                        }

                    }
                    if (status)
                    {
                        contentDiv.InnerHtml = html.ToString();
                        contentDiv.Visible = true;
                        ScriptManager.RegisterStartupScript(this, GetType(), "btnprint", "PrintDiv();", true);
                        Showgrid1.Visible = true;
                    }
                }
            }
        }
    }

    public string ToRoman(string part)
    {
        string roman = string.Empty;
        try
        {
            switch (part)
            {
                case "1":
                    roman = "I";
                    break;
                case "2":
                    roman = "II";
                    break;
                case "3":
                    roman = "III";
                    break;
                case "4":
                    roman = "IV";
                    break;
                case "5":
                    roman = "V";
                    break;
                case "6":
                    roman = "VI";
                    break;
                case "7":
                    roman = "VII";
                    break;
                case "8":
                    roman = "VIII";
                    break;
                case "9":
                    roman = "IX";
                    break;
                case "10":
                    roman = "X";
                    break;
                case "11":
                    roman = "XI";
                    break;
                case "12":
                    roman = "XII";
                    break;
            }
        }
        catch (Exception ex)
        {
        }
        return roman;
    }

    #endregion

    #region Condonation Report Format 2  Added By Prabha

    string conddate = string.Empty;
    string condchallan = string.Empty;
    string condamount = string.Empty;

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime date = new DateTime();
            conddate = txtCondonationDate.Text;
            condchallan = txtChallanAmount.Text;
            DateTime.TryParseExact(conddate, "d/MM/yyyy", null, DateTimeStyles.None, out date);

            string stdapplycon = dicstudApplyCondonation[rowindex];
            string[] split1 = stdapplycon.Split('$');

            string Cond_fineamnt = "";
            string Cond_app_no = Convert.ToString(split1[3]).Trim();
            string Cond_roll_no = Convert.ToString(split1[0]).Trim();
            string Cond_semester = Convert.ToString(split1[6]).Trim();
            string Cond_batchyr = Convert.ToString(split1[5]).Trim();
            string Cond_name = Convert.ToString(Showgrid1.Rows[rowindex].Cells[colcnt1].Text).Trim();
            if (rblCondType.SelectedIndex == 0)
                Cond_fineamnt = Convert.ToString(Showgrid1.Rows[rowindex].Cells[colcnt1 + 3].Text).Trim();
            string Cond_degreecode = Convert.ToString(split1[4]).Trim();




            string qry = "if exists(select ChallanDate,ChallanNo  from Eligibility_list where app_no= '" + Cond_app_no + "' and Semester= '" + Cond_semester + "' and degree_code= '" + Cond_degreecode + "' and batch_year='" + Cond_batchyr + "' ) update  Eligibility_list set Roll_no= '" + Cond_roll_no + "',stud_name= '" + Cond_name + "',is_eligible='2',batch_year= '" + Cond_batchyr + "' ,Semester= '" + Cond_semester + "',degree_code= '" + Cond_degreecode + "',fine_amt= '" + Cond_fineamnt + "',app_no= '" + Cond_app_no + "',isCondonationFee='1',isCompleteRedo='',Remarks='',ChallanDate='" + date.ToString("MM/dd/yyyy") + "',ChallanNo='" + condchallan + "'  where app_no= '" + Cond_app_no + "' and Semester= '" + Cond_semester + "' and degree_code= '" + Cond_degreecode + "' and batch_year='" + Cond_batchyr + "'";

            int res = dir.insertData(qry);
            btngo_Click(sender, e);
            if (res > 0)
            {
                txtCondonationDate.Text = conddate;
                txtChallanAmount.Text = condchallan;
                divPopUpAlert.Visible = false;
                lblAlertMsg.Text = "Saved Successfully";
            }
            divPopCond.Visible = false;

        }
        catch (Exception ex)
        {

        }
    }

    protected void btnCondExit_Click(object sender, EventArgs e)
    {
        divPopCond.Visible = false;
        txtCondonationDate.Text = string.Empty;
        txtChallanAmount.Text = string.Empty;
        //btngo_Click(sender, e);
    }

    protected void btnPopUpAlertClose_Click(object sender, EventArgs e)
    {
        divPopUpAlert.Visible = false;
        lblAlertMsg.Text = string.Empty;
    }

    #endregion

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = "";
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }

    #endregion

}

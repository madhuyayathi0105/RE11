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
using InsproDataAccess;
public partial class AttendanceMOD_CondonationReport : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    InsproDirectAccess da = new InsproDirectAccess();
    ReuasableMethods rs = new ReuasableMethods();
    DataTable data = new DataTable();
    DataRow drow;
    DataSet ds = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string grouporusercode = string.Empty;
    Hashtable hat = new Hashtable();
    Hashtable htcol = new Hashtable();

    int d = 0;
    #region Attendance variable
    Dictionary<string, string> SemInfoDet = new Dictionary<string, string>();
    Dictionary<string, int> HolidayInfoDet = new Dictionary<string, int>();
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
    double leavfinaeamount = 0;
    double medicalLeaveDays = 0;
    double moringabsentfine = 0, eveingabsentfine = 0, studentabsentfine = 0;
    int medicalLeaveHours = 0;
    int medicalLeaveCountPerSession = 0;
    double absentdaynew_val = 0;//modified by prabha feb 08 2018
    double absenthournew_val = 0;


    DateTime per_from_date;
    DateTime per_to_date;
    DateTime per_from_gendate;
    DateTime per_to_gendate;
    DateTime per_from_cumdate;
    DateTime per_to_cumdate;
    DateTime dumm_from_date;
    DateTime Admission_date;

    int NoHrs = 0;
    int fnhrs = 0;
    int anhrs = 0;
    int minpresI = 0;
    int col_count = 0;
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

    string[] string_session_values;
    DataSet ds_attnd_pts = new DataSet();
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    string value_holi_status = string.Empty;
    string[] split_holiday_status = new string[1000];
    string split_holiday_status_1 = "", split_holiday_status_2 = string.Empty;
    string isonumber = string.Empty;
    int inirow_count = 0;
    int demfcal, demtcal;
    string monthcal;
    Hashtable hatonduty = new Hashtable();
    Hashtable hatcumonduty = new Hashtable();
    DataSet ds1 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    bool yesflag = false;
    int dum_diff_date, unmark;
    double per_leavehrs;
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
    int mmyycount;
    string dd = string.Empty;
    int moncount;
    double dif_date = 0;
    double dif_date1 = 0;
    bool deptflag = false;
    static bool splhr_flag = false;
    Hashtable holiday_table11 = new Hashtable();
    Hashtable holiday_table21 = new Hashtable();
    Hashtable holiday_table31 = new Hashtable();
    string halforfull = "", mng = "", evng = "", holiday_sched_details = string.Empty;
    TimeSpan ts;
    string diff_date;

    int cellval = 0;
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
            grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
        }
        errmsg.Visible = false;
        lbl_norec.Visible = false;
        if (!IsPostBack)
        {
            bindstram();
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            BindSection();
            bindhostel();
            cb_reporttype.Checked = true;
            cb_reporttype_CheckedChanged(sender, e);
            string Master = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type') " + grouporusercode + "";
            DataSet ds = d2.select_method(Master, hat, "Text");
            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        ViewState["Rollflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        ViewState["Regflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        ViewState["Studflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[i]["settings"].ToString() == "Admission No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        ViewState["Admissionflag"] = "1";
                    }
                }
            }
        }
    }

    #region Bind Methods
    public void bindstram()
    {
        try
        {
            ddlstream.Items.Clear();
            ddlstream.Enabled = false;
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
    public void BindBatch()
    {
        try
        {
            cbl_batchyear.Items.Clear();
            DataSet ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batchyear.DataSource = ds;
                cbl_batchyear.DataTextField = "Batch_year";
                cbl_batchyear.DataValueField = "Batch_year";
                cbl_batchyear.DataBind();
            }
            foreach (ListItem item in cbl_batchyear.Items)
            {
                item.Selected = true;
                cb_batchyear.Checked = true;
                object sender = new object();
                EventArgs e = new EventArgs();
                cb_batchyear_CheckedChanged(sender, e);
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void BindDegree()
    {
        try
        {
            cblDegree.Items.Clear();
            chkDegree.Checked = false;
            txtDegree.Text = "-- Select --";
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            usercode = Session["usercode"].ToString();
            collegecode = Convert.ToString(Session["collegecode"]).Trim();
            singleuser = Session["single_user"].ToString();

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
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
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
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            string typeval = string.Empty;
            if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
                typeval = " and type='" + ddlstream.SelectedItem.ToString() + "'";
            string coursecode = rs.GetSelectedItemsValueAsString(cblDegree);
            if (!string.IsNullOrEmpty(coursecode))
            {
                string strquery = "select distinct degree.degree_code,de.dept_name from degree,course, deptprivilages,department de where course.course_id=degree.course_id and de.dept_code=degree.dept_code and course.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and course.college_code ='" + Convert.ToString(Session["collegecode"]).Trim() + "' and user_code='" + usercode + "' and degree.course_id in('" + coursecode + "') " + typeval + " ";
                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    strquery = "select distinct degree.degree_code,de.dept_name from degree,course, deptprivilages,department de where course.course_id=degree.course_id and de.dept_code=degree.dept_code and course.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "' and course.college_code ='" + Convert.ToString(Session["collegecode"]).Trim() + "' and degree.course_id in('" + coursecode + "') " + typeval + " ";
                }
                ds = d2.select_method_wo_parameter(strquery, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
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

            chkSem.Checked = false;
            cblSem.Items.Clear();
            txtSem.Text = "-- Select --";

            ddlsemester.Items.Clear();
            bool first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            string BranchCode = rs.GetSelectedItemsValueAsString(cblBranch);
            string qryBranch = string.Empty;
            if (!string.IsNullOrEmpty(BranchCode))
            {
                qryBranch = " and degree_code in('" + BranchCode + "')";
            }
            string strgetsem = "select distinct ndurations,first_year_nonsemester from ndegree where batch_year in ('" + rs.GetSelectedItemsValueAsString(cbl_batchyear) + "' ) and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'  " + qryBranch + " order by NDurations desc ";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strgetsem, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    ddlsemester.Items.Add(i.ToString());
                    cblSem.Items.Add(i.ToString());
                }
            }
            else
            {
                strgetsem = "select distinct duration,first_year_nonsemester  from degree where college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'  " + qryBranch + " order by  duration desc";
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
                        ddlsemester.Items.Add(i.ToString());
                        cblSem.Items.Add(i.ToString());
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
    public void BindSection()
    {
        try
        {
            chkSec.Checked = false;
            cblSec.Items.Clear();
            txtSec.Text = "-- Select --";
            txtSec.Enabled = false;
            string BranchCode = rs.GetSelectedItemsValueAsString(cblBranch);
            string qryBranch = string.Empty;
            if (!string.IsNullOrEmpty(BranchCode))
            {
                qryBranch = " and degree_code in('" + BranchCode + "')";
            }
            string strect = "select distinct case when (isnull(Rtrim(Ltrim(sections)),'') ='') then 'Empty' else isnull(Rtrim(Ltrim(sections)),'') end  as sections,isnull(Rtrim(Ltrim(sections)),'') as SecVal from registration where batch_year  in ('" + rs.GetSelectedItemsValueAsString(cbl_batchyear) + "' )" + qryBranch + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' union select'Empty' as sections,'' as SecVal  order by SecVal";//union select'Empty' as sections,'' as SecVal
            DataSet ds = d2.select_method_wo_parameter(strect, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
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
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void bindhostel()
    {
        try
        {
            ds.Clear();
            string itemname = "  select HostelMasterPK,HostelName  from HM_HostelMaster  union select '' HostelMasterPK,'Empty'HostelName  order by HostelMasterPK,HostelName  ";
            ds = d2.select_method_wo_parameter(itemname, "Text");
            cbl_hostelname.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();
                //if (cbl_hostelname.Items.Count > 0)
                //{
                //    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                //    {
                //        cbl_hostelname.Items[i].Selected = true;
                //    }
                //    txt_hostelname.Text = "Hostel Name(" + cbl_hostelname.Items.Count + ")";
                //}
            }
            else
            {
                txt_hostelname.Text = "--Select--";
            }
        }
        catch
        {

        }
    }
    #endregion

    #region Filter Events
    protected void ddlstream_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBatch();
        BindDegree();
        bindbranch();
        bindsem();
        BindSection();
    }
    //protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindDegree();
    //    bindbranch();
    //    bindsem();
    //    BindSection();
    //}
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
        BindSection();
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        BindSection();
    }
    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {

        string BranchCode = rs.GetSelectedItemsValueAsString(cblBranch);
        string qryBranch = string.Empty;
        if (!string.IsNullOrEmpty(BranchCode))
        {
            qryBranch = " and degree_code in(" + BranchCode + ")";
        }
        string minattexam = d2.GetFunction("select distinct percent_eligible_for_exam from PeriodAttndSchedule where semester='" + ddlsemester.SelectedItem.ToString() + "' " + qryBranch + " order by percent_eligible_for_exam desc");
    }
    protected void chkDegree_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cblDegree, chkDegree, txtDegree, "Degree");
        bindbranch();
        bindsem();
        BindSection();
    }
    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cblDegree, chkDegree, txtDegree, "Degree");
        bindbranch();
        bindsem();
        BindSection();
    }
    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cblBranch, chkBranch, txtBranch, "Branch");
        bindsem();
        BindSection();
    }
    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cblBranch, chkBranch, txtBranch, "Branch");
        bindsem();
        BindSection();
    }
    protected void chkSem_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cblSem, chkSem, txtSem, "Semester");
    }

    protected void cblSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cblSem, chkSem, txtSem, "Semester");
    }

    protected void chkSec_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cblSec, chkSec, txtSec, "Section");
    }
    protected void cblSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cblSec, chkSec, txtSec, "Section");
    }
    protected void cbl_reporttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_reporttype, cb_reporttype, txt_reporttype, "Type");
    }
    protected void cb_reporttype_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_reporttype, cb_reporttype, txt_reporttype, "Type");
    }
    protected void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_hostelname, cb_hostelname, txt_hostelname, "Hostel Name");
    }
    protected void cb_hostelname_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_hostelname, cb_hostelname, txt_hostelname, "Hostel Name");
    }
    protected void cb_batchyear_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_batchyear, cb_batchyear, txt_batchyear, "Batch Year");
    }
    protected void cbl_batchyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_batchyear, cb_batchyear, txt_batchyear, "Batch Year");
    }
    public void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.Trim() != "")
            {
                lbl_norec.Visible = false;
                //d2.printexcelreport(FpCondonation, report);
                d2.printexcelreportgrid(Showgrid, report);
                txt_excelname.Text = "";
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();
        }
        catch (Exception ex)
        {
            lbl_norec.Text = ex.ToString();
        }
    }
    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            //Printcontrol.loadspreaddetails(FpCondonation, "CondonationReport.aspx", "Student Condonation Report");
            string ss = null;
            string pagename = "CondonationReport.aspx";
            string degreedetails = string.Empty;
            Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
            ////Printcontrol.loadspreaddetails(attnd_report, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }
    #endregion

    protected void btngo_Click(object sender, EventArgs e)
    {

        try
        {
            btnPrint11();
            Printcontrol.Visible = false;
            string selectedSem = string.Empty;
            string Batchyear = rs.GetSelectedItemsValueAsString(cbl_batchyear);
            string BranchCode = rs.GetSelectedItemsValueAsString(cblBranch);
            string Semester = string.Empty;// Convert.ToString(ddlsemester.SelectedItem.Value);

            if (ddlsemester.Items.Count > 0 && ddlsemester.Visible)
            {
                Semester = Convert.ToString(ddlsemester.SelectedItem.Value);
            }
            else if (cblSem.Items.Count > 0 && txtSem.Visible)
            {
                Semester = rs.GetSelectedItemsValueAsString(cblSem);
                selectedSem = "/Sem " + Semester.Replace("'", "").Replace(",", "/Sem ");
            }
            int semCount = Semester.Split(',').Length;

            
            int rowcount = 0;
            string Section = rs.GetSelectedItemsValueAsString(cblSec);
            string Type = rs.GetSelectedItemsValueAsString(cbl_reporttype);
            string HostelmasterPK = rs.GetSelectedItemsValueAsString(cbl_hostelname);
            string HostelQry = string.Empty;
            string DegreeQry = string.Empty;
            string SemesterQry = string.Empty;
            string sectionQry = string.Empty;
            if (!string.IsNullOrEmpty(Type))
            {
                Type = " and is_eligible in('" + Type + "') ";
            }
            if (!string.IsNullOrEmpty(HostelmasterPK))
            {
                HostelQry = " and isnull(hm.HostelMasterPK,'0') in('" + HostelmasterPK + "') ";
            }
            if (!string.IsNullOrEmpty(BranchCode))
            {
                DegreeQry = " and r.degree_code in('" + BranchCode + "') ";
            }
            if (!string.IsNullOrEmpty(Semester))
            {
                SemesterQry = " and Semester in('" + Semester + "')";
            }
            if (!string.IsNullOrEmpty(Section))
            {
                sectionQry = " and ISNULL(r.Sections,'') in('" + Section + "') ";
            }
            if (!string.IsNullOrEmpty(Batchyear) && !string.IsNullOrEmpty(Semester))
            {
                string EligibilityQry = "  select distinct r.Roll_no,r.Reg_No,r.Roll_Admit,r.stud_name,r.Stud_Type,e.is_eligible, r.batch_year, e.Semester, e.degree_code,e.fine_amt, e.app_no,e.isCondonationFee, e.isCompleteRedo,r.Sections,CONVERT(varchar(100),c.Course_Name)+'-'+CONVERT(varchar(max),dt.dept_name)Degree,e.remarks,hm.HostelName,h.HostelMasterFK,c.Course_Id,CONVERT(varchar(10), r.Adm_Date,103) Adm_Date from Eligibility_list e left join HT_HostelRegistration h on e.app_no=h.APP_No left join HM_HostelMaster hm on h.HostelMasterFK=hm.HostelMasterPK ,Registration r ,Degree d,course c,Department dt where d.Dept_Code=dt.Dept_Code and d.Course_Id=c.Course_Id and r.degree_code=d.Degree_Code and d.college_code=r.college_code and r.App_No=e.app_no and r.Roll_No=e.Roll_no and r.Batch_Year=e.batch_year and r.degree_code=e.degree_code and r.batch_year in ('" + Batchyear + "')  " + DegreeQry + SemesterQry + sectionQry + HostelQry + Type + "   order by c.Course_Id,e.degree_code  ";
                DataSet StudentDet = da.selectDataSet(EligibilityQry);


                DataTable dtDistinctStudent = new DataTable();
                if (StudentDet.Tables.Count > 0 && StudentDet.Tables[0].Rows.Count > 0)
                {
                    dtDistinctStudent = StudentDet.Tables[0].DefaultView.ToTable(true, "app_no", "Roll_no", "Reg_No", "Roll_Admit", "stud_name", "Stud_Type", "batch_year", "degree_code", "Sections", "Degree");
                    if (!string.IsNullOrEmpty(Type))
                    {
                        if (semCount == 1)
                        {

                            drow = data.NewRow();
                            data.Rows.Add(drow);
                            int colu = 0;
                            data.Columns.Add("SNo", typeof(string));
                            data.Rows[0][colu] = "SNo";
                            colu++;
                            if (Convert.ToString(ViewState["Rollflag"]) == "1")
                            {
                                data.Columns.Add("Roll No", typeof(string));
                                data.Rows[0][colu] = "Roll No";
                                colu++;
                            }
                            if (Convert.ToString(ViewState["Regflag"]) == "1")
                            {
                                data.Columns.Add("Reg No", typeof(string));
                                data.Rows[0][colu] = "Reg No";
                                colu++;
                            }
                            if (Convert.ToString(ViewState["Admissionflag"]) == "1")
                            {
                                data.Columns.Add("Admission No", typeof(string));
                                data.Rows[0][colu] = "Admission No";
                                colu++;
                            }
                            data.Columns.Add("Student Name", typeof(string));
                            data.Rows[0][colu] = "Student Name";
                            colu++;
                            if (Convert.ToString(ViewState["Studflag"]) == "1")
                            {
                                data.Columns.Add("Student Type", typeof(string));
                                data.Rows[0][colu] = "Student Type";
                                colu++;
                            }
                            data.Columns.Add("Degree", typeof(string));
                            data.Rows[0][colu] = "Degree";
                            colu++;
                            data.Columns.Add("Remarks", typeof(string));
                            data.Rows[0][colu] = "Remarks";
                            colu++;
                            data.Columns.Add("Hostel Name", typeof(string));
                            data.Rows[0][colu] = "Hostel Name";
                            colu++;
                            data.Columns.Add("Attendance Percentage", typeof(string));
                            data.Rows[0][colu] = "Attendance Percentage";
                            colu++;
                            if (rblHrDaywise.SelectedIndex == 0)
                            {
                                data.Columns.Add("Absent Days", typeof(string));
                                data.Rows[0][colu] = "Absent Days";
                                colu++;
                            }
                            else if (cbl_reporttype.Items[1].Selected == true)
                            {
                                data.Columns.Add("Absent Hours", typeof(string));
                                data.Rows[0][colu] = "Absent Hours";
                                colu++;

                            }
                            if (cbl_reporttype.Items[2].Selected == true)
                            {
                                if (cbl_reporttype.Items[2].Selected == true && cbl_reporttype.Items[0].Selected == false && cbl_reporttype.Items[1].Selected == false)
                                {

                                    data.Columns.Add("Eligible For Next Semester", System.Type.GetType("System.Boolean"));
                                    data.Rows[0][colu] = "Eligible For Next Semester";
                                    colu++;
                                }
                                else
                                {
                                    data.Columns.Add("Eligible For Next Semester", typeof(string));
                                    data.Rows[0][colu] = "Eligible For Next Semester";
                                    colu++;
                                }
                            }
                            int row = 0;
                            if (cbl_reporttype.Items[0].Selected == true)
                            {
                                //modified by prabha feb 08 2018
                                #region Eligibility
                                DataView EligibilityDv = new DataView();
                                StudentDet.Tables[0].DefaultView.RowFilter = " is_eligible='1'";
                                EligibilityDv = StudentDet.Tables[0].DefaultView;

                                foreach (DataRowView dr in EligibilityDv)
                                {
                                    rowcount++;
                                    if (rowcount == 1)
                                    {
                                        drow = data.NewRow();
                                        drow["SNo"] = "Eligibility to write exam";
                                        data.Rows.Add(drow);

                                    }

                                    AttendancePercentage(collegecode, rs.GetSelectedItemsValueAsString(cbl_batchyear), Convert.ToString(dr["degree_code"]), Convert.ToString(dr["Semester"]), Convert.ToString(dr["Roll_no"]), Convert.ToString(dr["Adm_Date"]));
                                    double presentPercentage = 0;
                                    if (rblHrDaywise.SelectedIndex == 0)
                                        double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(dum_tage_date), 2)), out presentPercentage);
                                    else
                                        double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(dum_tage_hrs), 2)), out presentPercentage);

                                    double fromrange = 0;
                                    double torange = 0;
                                    bool visibleflag = false;
                                    if (txtfrom_range.Text.Trim() != "" && !string.IsNullOrEmpty(txtfrom_range.Text.Trim()) && txtto_range.Text.Trim() != "" && !string.IsNullOrEmpty(txtto_range.Text.Trim()))
                                    {
                                        double.TryParse(txtfrom_range.Text.Trim(), out fromrange);
                                        double.TryParse(txtto_range.Text.Trim(), out torange);
                                        if (presentPercentage >= fromrange && presentPercentage <= torange)
                                        {
                                            visibleflag = true;
                                            row++;
                                        }

                                    }
                                    if (visibleflag)
                                    {
                                        //row++;
                                        drow = data.NewRow();
                                        drow["SNo"] = Convert.ToString(row);
                                        drow["Student Name"] = Convert.ToString(dr["stud_name"]);
                                        drow["Degree"] = Convert.ToString(dr["Degree"]);
                                        drow["Remarks"] = Convert.ToString(dr["remarks"]).ToUpper() == "CAG" ? Convert.ToString(dr["remarks"]) + ",NE" : Convert.ToString(dr["remarks"]);
                                        drow["Hostel Name"] = Convert.ToString(dr["HostelName"]);
                                        drow["Attendance Percentage"] = String.Format("{0:0.00}", presentPercentage);

                                        if (Convert.ToString(ViewState["Rollflag"]) == "1")
                                            drow["Roll No"] = Convert.ToString(dr["Roll_no"]);
                                        if (Convert.ToString(ViewState["Regflag"]) == "1")
                                            drow["Reg No"] = Convert.ToString(dr["Reg_No"]);
                                        if (Convert.ToString(ViewState["Admissionflag"]) == "1")
                                            drow["Admission No"] = Convert.ToString(dr["Roll_Admit"]);
                                        if (Convert.ToString(ViewState["Studflag"]) == "1")
                                            drow["Student Type"] = Convert.ToString(dr["Stud_Type"]);
                                        int col2 = data.Columns.Count -2;
                                        if (rblHrDaywise.SelectedIndex == 0)
                                        {

                                            if (Convert.ToString(absentdaynew_val).Contains(".5"))
                                            {
                                                drow[col2] = String.Format("{0:0.0}", absentdaynew_val);
                                                col2++;
                                            }
                                            else
                                            {
                                                drow[col2] = String.Format("{0:0}", absentdaynew_val);
                                                col2++;
                                            }
                                        }
                                        else
                                        {

                                            if (Convert.ToString(absentdaynew_val).Contains(".5"))
                                            {
                                                drow[col2] = String.Format("{0:0.0}", absenthournew_val);
                                                col2++;
                                            }

                                            else
                                            {
                                                drow[col2] = String.Format("{0:0}", absenthournew_val);
                                                col2++;
                                            }

                                        }
                                        data.Rows.Add(drow);

                                    }

                                }
                                #endregion
                            }
                            if (cbl_reporttype.Items[1].Selected == true)
                            {
                                //modified by prabha feb 08 2018
                                #region Eligible To Condonation Apply
                                DataView CondonationDv = new DataView();
                                StudentDet.Tables[0].DefaultView.RowFilter = " is_eligible='2'";
                                CondonationDv = StudentDet.Tables[0].DefaultView;
                                row = 0;
                                rowcount = 0;
                                foreach (DataRowView dr in CondonationDv)
                                {
                                    rowcount++;
                                    if (rowcount == 1)
                                    {
                                        drow = data.NewRow();
                                        drow["SNo"] = "Condonation";
                                        data.Rows.Add(drow);

                                    }
                                    AttendancePercentage(collegecode, rs.GetSelectedItemsValueAsString(cbl_batchyear), Convert.ToString(dr["degree_code"]), Convert.ToString(dr["Semester"]), Convert.ToString(dr["Roll_no"]), Convert.ToString(dr["Adm_Date"]));
                                    double presentPercentage = 0;
                                    if (rblHrDaywise.SelectedIndex == 0)
                                        double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(dum_tage_date), 2)), out presentPercentage);
                                    else
                                        double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(dum_tage_hrs), 2)), out presentPercentage);

                                    double fromrange = 0;
                                    double torange = 0;
                                    bool visibleflag = false;
                                    if (txtfrom_range.Text.Trim() != "" && !string.IsNullOrEmpty(txtfrom_range.Text.Trim()) && txtto_range.Text.Trim() != "" && !string.IsNullOrEmpty(txtto_range.Text.Trim()))
                                    {
                                        double.TryParse(txtfrom_range.Text.Trim(), out fromrange);
                                        double.TryParse(txtto_range.Text.Trim(), out torange);
                                        if (presentPercentage >= fromrange && presentPercentage <= torange)
                                        {
                                            visibleflag = true;
                                            row++;
                                        }

                                    }
                                    if (visibleflag)
                                    {

                                        drow = data.NewRow();
                                        drow["SNo"] = Convert.ToString(row);
                                        drow["Student Name"] = Convert.ToString(dr["stud_name"]);
                                        drow["Degree"] = Convert.ToString(dr["Degree"]);
                                        drow["Remarks"] = Convert.ToString(dr["remarks"]).ToUpper() == "CAG" ? Convert.ToString(dr["remarks"]) + ",NE" : Convert.ToString(dr["remarks"]);
                                        drow["Hostel Name"] = Convert.ToString(dr["HostelName"]);
                                        drow["Attendance Percentage"] = String.Format("{0:0.00}", presentPercentage);

                                        if (Convert.ToString(ViewState["Rollflag"]) == "1")
                                            drow["Roll No"] = Convert.ToString(dr["Roll_no"]);
                                        if (Convert.ToString(ViewState["Regflag"]) == "1")
                                            drow["Reg No"] = Convert.ToString(dr["Reg_No"]);
                                        if (Convert.ToString(ViewState["Admissionflag"]) == "1")
                                            drow["Admission No"] = Convert.ToString(dr["Roll_Admit"]);
                                        if (Convert.ToString(ViewState["Studflag"]) == "1")
                                            drow["Student Type"] = Convert.ToString(dr["Stud_Type"]);


                                        if (rblHrDaywise.SelectedIndex == 0)
                                        {
                                            if (Convert.ToString(absentdaynew_val).Contains(".5"))
                                                drow["Absent Hours"] = String.Format("{0:0.0}", absentdaynew_val);
                                            else
                                                drow["Absent Hours"] = String.Format("{0:0}", absentdaynew_val);

                                        }
                                        else
                                        {
                                            if (Convert.ToString(absentdaynew_val).Contains(".5"))
                                                drow["Absent Hours"] = String.Format("{0:0.0}", absenthournew_val);
                                            else
                                                drow["Absent Hours"] = String.Format("{0:0}", absenthournew_val);
                                        }

                                        data.Rows.Add(drow);
                                    }

                                }
                                #endregion
                            }
                            if (cbl_reporttype.Items[2].Selected == true)
                            {
                                //modified by prabha feb 08 2018
                                #region Not eligible

                                DataView notEligibilityDv = new DataView();
                                StudentDet.Tables[0].DefaultView.RowFilter = " is_eligible='3'";
                                notEligibilityDv = StudentDet.Tables[0].DefaultView;
                                row = 0;
                                rowcount = 0;
                                foreach (DataRowView dr in notEligibilityDv)
                                {
                                    rowcount++;
                                    if (rowcount == 1)
                                    {
                                        drow = data.NewRow();
                                        drow["SNo"] = "Not Eligibility";
                                        data.Rows.Add(drow);

                                    }
                                    double presentPercentage = 0;
                                    if (rblHrDaywise.SelectedIndex == 0)
                                        if (dum_tage_date != "")
                                            double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(dum_tage_date), 2)), out presentPercentage);
                                        else
                                            double.TryParse(Convert.ToString(Math.Round(Convert.ToDecimal(dum_tage_hrs), 2)), out presentPercentage);
                                    double fromrange = 0;
                                    double torange = 0;
                                    bool visibleflag = false;
                                    if (txtfrom_range.Text.Trim() != "" && !string.IsNullOrEmpty(txtfrom_range.Text.Trim()) && txtto_range.Text.Trim() != "" && !string.IsNullOrEmpty(txtto_range.Text.Trim()))
                                    {
                                        double.TryParse(txtfrom_range.Text.Trim(), out fromrange);
                                        double.TryParse(txtto_range.Text.Trim(), out torange);
                                        if (presentPercentage >= fromrange && presentPercentage <= torange)
                                        {
                                            visibleflag = true;
                                            row++;
                                        }
                                    }
                                    if (visibleflag)
                                    {

                                        drow = data.NewRow();
                                        drow["SNo"] = Convert.ToString(row);
                                        drow["Student Name"] = Convert.ToString(dr["stud_name"]);
                                        drow["Degree"] = Convert.ToString(dr["Degree"]);
                                        drow["Remarks"] = Convert.ToString(dr["remarks"]).ToUpper() == "CAG" ? Convert.ToString(dr["remarks"]) + ",NE" : Convert.ToString(dr["remarks"]);
                                        drow["Hostel Name"] = Convert.ToString(dr["HostelName"]);
                                        drow["Attendance Percentage"] = String.Format("{0:0.00}", presentPercentage);

                                        if (Convert.ToString(ViewState["Rollflag"]) == "1")
                                            drow["Roll No"] = Convert.ToString(dr["Roll_no"]);
                                        if (Convert.ToString(ViewState["Regflag"]) == "1")
                                            drow["Reg No"] = Convert.ToString(dr["Reg_No"]);
                                        if (Convert.ToString(ViewState["Admissionflag"]) == "1")
                                            drow["Admission No"] = Convert.ToString(dr["Roll_Admit"]);
                                        if (Convert.ToString(ViewState["Studflag"]) == "1")
                                            drow["Student Type"] = Convert.ToString(dr["Stud_Type"]);
                                        AttendancePercentage(collegecode, rs.GetSelectedItemsValueAsString(cbl_batchyear), Convert.ToString(dr["degree_code"]), Convert.ToString(dr["Semester"]), Convert.ToString(dr["Roll_no"]), Convert.ToString(dr["Adm_Date"]));


                                       
                                        //System.Web.UI.WebControls.CheckBox ch = new System.Web.UI.WebControls.CheckBox();

                                        //drow["Eligible For Next Semester"] =ch;
                                        data.Rows.Add(drow);

                                    }

                                }

                                #endregion
                            }


                        }
                        else
                        {
                            drow = data.NewRow();
                            data.Rows.Add(drow);
                            int colu = 0;
                            

                            data.Columns.Add("SNo", typeof(string));
                            
                            data.Rows[0][colu] = "SNo";
                            colu++;
                            if (Convert.ToString(ViewState["Rollflag"]) == "1")
                            {
                                data.Columns.Add("Roll No", typeof(string));
                                data.Rows[0][colu] = "Roll No";
                                colu++;
                            }
                            if (Convert.ToString(ViewState["Regflag"]) == "1")
                            {
                                data.Columns.Add("Reg No", typeof(string));
                                data.Rows[0][colu] = "Reg No";
                                colu++;
                            }
                            if (Convert.ToString(ViewState["Admissionflag"]) == "1")
                            {
                                data.Columns.Add("Admission No", typeof(string));
                                data.Rows[0][colu] = "Admission No";
                                colu++;
                            }
                            data.Columns.Add("Student Name", typeof(string));
                            data.Rows[0][colu] = "Student Name";
                            colu++;
                            if (Convert.ToString(ViewState["Studflag"]) == "1")
                            {
                                data.Columns.Add("Student Type", typeof(string));
                                data.Rows[0][colu] = "Student Type";
                                colu++;
                            }
                            data.Columns.Add("Degree", typeof(string));
                            data.Rows[0][colu] = "Degree";
                            colu++;
                            if (selectedSem != "")
                            {
                                string[] SplitSem = selectedSem.Split('/');

                                if (SplitSem.Length > 1)
                                {
                                    for (int s = 0; s < SplitSem.Length; s++)
                                    {

                                        string sem = Convert.ToString(SplitSem[s]);
                                        if (sem != "")
                                        {
                                            data.Columns.Add(sem, typeof(string));
                                            data.Rows[0][colu] = sem;
                                            colu++;
                                        }

                                    }

                                }
                            }

                            if (dtDistinctStudent.Rows.Count > 0)
                            {
                                int row = 0;
                                foreach (DataRow dr in dtDistinctStudent.Rows)
                                {

                                    row++;
                                    drow = data.NewRow();
                                    drow["SNo"] = Convert.ToString(row);
                                    drow["Student Name"] = Convert.ToString(dr["stud_name"]);
                                    drow["Degree"] = Convert.ToString(dr["Degree"]);

                                    if (Convert.ToString(ViewState["Rollflag"]) == "1")
                                        drow["Roll No"] = Convert.ToString(dr["Roll_no"]);
                                    if (Convert.ToString(ViewState["Regflag"]) == "1")
                                        drow["Reg No"] = Convert.ToString(dr["Reg_No"]);
                                    if (Convert.ToString(ViewState["Admissionflag"]) == "1")
                                        drow["Admission No"] = Convert.ToString(dr["Roll_Admit"]);
                                    if (Convert.ToString(ViewState["Studflag"]) == "1")
                                        drow["Student Type"] = Convert.ToString(dr["Stud_Type"]);

                                    string appNo = Convert.ToString(dr["app_no"]);


                                    ArrayList arrSelectedReport = new ArrayList();
                                    foreach (ListItem li in cbl_reporttype.Items)
                                    {
                                        if (li.Selected)
                                            if (!arrSelectedReport.Contains(li.Value))
                                                arrSelectedReport.Add(li.Value);
                                    }
                                    if (selectedSem != "")
                                    {
                                        string[] SplitSem = selectedSem.Split('/');
                                        if (SplitSem.Length > 1)
                                        {
                                            for (int s = 0; s < SplitSem.Length; s++)
                                            {
                                                string sem1 = Convert.ToString(SplitSem[s]);
                                                if (sem1 != "")
                                                {
                                                    string sem = Convert.ToString(sem1).Replace("Sem ", "");

                                                    DataView dv = new DataView();
                                                    StudentDet.Tables[0].DefaultView.RowFilter = " app_no='" + appNo + "' and semester='" + sem + "'";
                                                    dv = StudentDet.Tables[0].DefaultView;
                                                    if (dv.Count > 0)
                                                    {
                                                        string categoryCode = Convert.ToString(dv[0]["is_eligible"]);
                                                        string categoryName = string.Empty;
                                                        if (arrSelectedReport.Contains(categoryCode))
                                                        {
                                                            switch (categoryCode)
                                                            {
                                                                case "1":
                                                                    categoryName = "E";
                                                                    break;
                                                                case "2":
                                                                    categoryName = "C";
                                                                    break;
                                                                case "3":
                                                                    categoryName = "NE";
                                                                    break;
                                                                case "4":
                                                                    categoryName = "EN";
                                                                    break;
                                                            }
                                                        }
                                                        drow[sem1] = Convert.ToString(categoryName);
                                                        //FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, col].Text = categoryName;
                                                    }
                                                    else
                                                    {
                                                        drow[sem1] = Convert.ToString("--");
                                                        //FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, col].Text = "--";
                                                    }

                                                }
                                            }

                                        }
                                    }


                                    data.Rows.Add(drow);
                                }


                            }
                        }
                        if (data.Rows.Count > 0)
                        {
                            cellval = data.Columns.Count - 1;
                            Showgrid.DataSource = data;
                            Showgrid.DataBind();
                            Showgrid.Visible = true;
                            divSpread.Visible = true;
                            Showgrid.HeaderRow.Visible = false;
                            //BoundField bf = new BoundField();
                            //bf.HeaderText = "Types of bot available";

                            //// Add my column to my gridview
                            //Showgrid.Columns.Add(bf);
                            //  CheckBox ch = new CheckBox();
                            //            ch.ID = "cb1";
                            //            Showgrid.Rows[1].Cells[9].Controls.Add(ch);

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
                                        if (Showgrid.HeaderRow.Cells[j].Text == "Roll No" || Showgrid.HeaderRow.Cells[j].Text == "Reg No" || Showgrid.HeaderRow.Cells[j].Text == "Admission No" || Showgrid.HeaderRow.Cells[j].Text == "Student Name" || Showgrid.HeaderRow.Cells[j].Text == "Degree" || Showgrid.Rows[i].Cells[j].Text == "&nbsp;")
                                        {
                                            Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;
                                            if (Showgrid.Rows[i].Cells[j].Text == "&nbsp;" && j == 1)
                                            {
                                                Showgrid.Rows[i].Cells[j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                Showgrid.Rows[i].Cells[j - 1].HorizontalAlign = HorizontalAlign.Center;
                                                Showgrid.Rows[i].Cells[j - 1].BackColor = Color.LightGreen;
                                                Showgrid.Rows[i].Cells[j - 1].ColumnSpan = Showgrid.Rows[i].Cells.Count;
                                                for (int a = 1; a < Showgrid.Rows[i].Cells.Count; a++)
                                                    Showgrid.Rows[i].Cells[a].Visible = false;


                                            }
                                        }

                                        else
                                            Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;

                                    }
                                }

                            }


                            if (Showgrid.Rows.Count > 0)
                            {

                                if (cbl_reporttype.Items[2].Selected == true && cbl_reporttype.Items[0].Selected == false && cbl_reporttype.Items[1].Selected == false)
                                {

                                    d = Convert.ToInt32(data.Columns.Count);
                                    //d = d - 1;
                                    Showgrid.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                    Showgrid.Rows[0].Cells[0].BackColor = Color.LightGreen;
                                    Showgrid.Rows[0].Cells[0].ColumnSpan = d;
                                    for (int a = 1; a < d; a++)
                                    {
                                        if (a != d - 1)
                                            Showgrid.Rows[0].Cells[a].Visible = false;
                                        if (a == d - 1)
                                        {
                                            htcol.Add("0", a);
                                            int j = 0;
                                            for (int a1 = 1; a1 < Showgrid.Rows.Count; a1++)
                                            {
                                                
                                                
                                                //Showgrid.Rows[a1].Cells[a].HorizontalAlign = HorizontalAlign.Center;
                                                

                                                //CheckBox ch = new CheckBox();
                                                //ch.ID = "chkb" + j + "";
                                                //Showgrid.Rows[a1].Cells[a].Controls.Add(ch);
                                                //j++;
                                            }
                                        }

                                    }
                                    //addchk();
                                }
                                else
                                {

                                }
                            }

                        }
                        else
                        {
                            divSpread.Visible = false;
                            Showgrid.Visible = false;
                            //btn_pdf.Visible = false;
                            //btnexcel.Visible = false;
                        }
                    }
                    else
                    {
                        divSpread.Visible = false;
                        errmsg.Visible = true;
                        errmsg.Text = "Please select all fields";
                    }
                }
                else
                {
                    divSpread.Visible = false;
                    errmsg.Visible = true;
                    errmsg.Text = "No Records Founds";
                }
            }
            else
            {
                divSpread.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "Please select all fields";
            }
        }
        catch
        {

        }
    }

    protected void AttendancePercentage(string collegeCodeP, string BatchYear, string degreeP, string semP, string rollnoP, string admDateP)
    {
        string SemStartDate = string.Empty;
        string SemEndDate = string.Empty;
        if (!SemInfoDet.ContainsKey(degreeP + "$" + semP + "$" + BatchYear))
        {
            string SemInfoQry = "select semester,CONVERT(varchar(10), start_date,103)start_date,CONVERT(varchar(10), end_date,103)end_date,no_of_working_days from seminfo where degree_code=" + degreeP + " and semester =" + semP + " and batch_year in (' " + BatchYear + "')  order by semester ";
            DataSet semdetailsDs = da.selectDataSet(SemInfoQry);
            if (semdetailsDs.Tables[0].Rows.Count > 0)
            {
                SemStartDate = Convert.ToString(semdetailsDs.Tables[0].Rows[0]["start_date"]);   //existing
                SemEndDate = Convert.ToString(semdetailsDs.Tables[0].Rows[0]["end_date"]);
                if (txtfdate.Text.Trim() != "" && !string.IsNullOrEmpty(txtfdate.Text.Trim()) && txttodate.Text.Trim() != "" && !string.IsNullOrEmpty(txttodate.Text.Trim()))
                {
                    SemStartDate = txtfdate.Text.Trim();   //modified by prabha for date filter
                    SemEndDate = txttodate.Text.Trim();
                }
            }
            SemInfoDet.Add(degreeP + "$" + semP + "$" + BatchYear, SemStartDate + "*" + SemEndDate);
        }
        else
        {
            string[] semDate = Convert.ToString(SemInfoDet[degreeP + "$" + semP + "$" + BatchYear]).Split('*');
            if (semDate.Length == 2)
            {
                SemStartDate = Convert.ToString(semDate[0]);
                SemEndDate = Convert.ToString(semDate[1]);
            }
        }
        string dt = SemStartDate;
        string[] dsplit = dt.Split(new Char[] { '/' });
        SemStartDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
        demfcal = int.Parse(dsplit[2].ToString());
        demfcal = demfcal * 12;
        cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
        cal_from_date_tmp = demfcal + int.Parse(dsplit[1].ToString());
        monthcal = cal_from_date.ToString();
        dt = SemEndDate;
        dsplit = dt.Split(new Char[] { '/' });
        SemEndDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
        demtcal = int.Parse(dsplit[2].ToString());
        demtcal = demtcal * 12;
        cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
        cal_to_date_tmp = demtcal + int.Parse(dsplit[1].ToString());
        per_from_gendate = Convert.ToDateTime(SemStartDate);
        per_to_gendate = Convert.ToDateTime(SemEndDate);

        ArrayList arrDegree = new ArrayList();
        if (!arrDegree.Contains(degreeP))
        {
            hat.Clear();
            hat.Add("degree_code", degreeP);
            hat.Add("sem_ester", int.Parse(semP));
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
            int count = ds1.Tables[0].Rows.Count;
            arrDegree.Add(degreeP);
        }
        persentmonthcal(collegeCodeP, degreeP, semP, rollnoP, admDateP, SemStartDate, SemEndDate);

        conductedDays = per_workingdays;
        presentDays = pre_present_date;

        double ondutyval_new = pre_ondu_date;


        if (per_workingdays > 0)
        {
            absentDays = per_workingdays - pre_present_date;
        }
        if (per_workingdays > 0)
        {
            per_tage_date = ((pre_present_date / per_workingdays) * 100);
            absentDaysPercentage = ((absentDays / per_workingdays) * 100);
            absentdaynew_val = absentDays;
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
            absenthournew_val = absentHours;
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
        dum_tage_date = String.Format("{0:0,0.00}", float.Parse(Convert.ToString(per_tage_date).Trim()));
        dum_tage_hrs = String.Format("{0:0,0.00}", float.Parse(Convert.ToString(per_tage_hrs).Trim()));
        absentDaysPercentage1 = String.Format("{0:0,0.00}", float.Parse(Convert.ToString(absentDaysPercentage).Trim()));
        absentHoursPercentage1 = String.Format("{0:0,0.00}", float.Parse(Convert.ToString(absentHoursPercentage).Trim()));
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
            compareValue = presentDays;
        }
        else
        {
            compareValue = per_tage_hrs;
        }
    }

    public void persentmonthcal(string collegeCode, string degree, string sem, string rollno, string admitDate, string SemStartDate, string SemEndDate)
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
        string admdate = admitDate;
        DateTime.TryParseExact(admdate, "dd/MM/yyyy", null, DateTimeStyles.None, out Admission_date);
        dd = rollno.Trim();
        hat.Clear();
        hat.Add("std_rollno", rollno.Trim());
        hat.Add("from_month", cal_from_date);
        hat.Add("to_month", cal_to_date);
        DataSet ds2 = d2.select_method("STUD_ATTENDANCE", hat, "sp");
        mmyycount = ds2.Tables[0].Rows.Count;
        moncount = mmyycount - 1;
        int count = ds1.Tables[0].Rows.Count;
        if (deptflag == false)
        {
            deptflag = true;
            hat.Clear();
            hat.Add("degree_code", int.Parse(Convert.ToString(degree).Trim()));
            hat.Add("sem", int.Parse(Convert.ToString(sem).Trim()));
            hat.Add("from_date", Convert.ToString(SemStartDate));
            hat.Add("to_date", Convert.ToString(SemEndDate));
            hat.Add("coll_code", int.Parse(Convert.ToString(collegeCode)));
            int iscount = 0;
            string sqlstr_holiday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + SemStartDate.ToString() + "' and '" + SemEndDate.ToString() + "' and degree_code=" + degree + " and semester=" + sem;
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

    protected void btn_CondonationEligibleSave_Click(object sender, EventArgs e)
    {
        string Roll_No = string.Empty;
        string Reg_No = string.Empty;
        string App_No = string.Empty;
        string Stud_Name = string.Empty;
        string Current_Semester = string.Empty;
        string Batch_Year = string.Empty;
        string Degree_Code = string.Empty;
        string studqry = string.Empty;
        string selectqry = string.Empty;
        string updateqry = string.Empty;
        string RedoSem = string.Empty;
        int result = 0;
        DataTable dtstudedetails = new DataTable();
        //Saran
        int j = 0;
       

        for (int i = 1; i < Showgrid.Rows.Count; i++)
        {
            
            int checkedd = 0;
            cellval=Showgrid.Rows[1].Cells.Count-1;
            string val = "0" + 0 + "";
            string chkname = "ctl" + val + "";
            CheckBox chkRow = (Showgrid.Rows[i].Cells[cellval].FindControl(chkname) as CheckBox);
            Boolean sm = chkRow.Checked;
            if (chkRow.Checked == true)
                checkedd = 1;
            else
                checkedd = 0;
            
            if (checkedd == 1)
            {
                Roll_No = Convert.ToString(Showgrid.Rows[i].Cells[1].Text);
                Reg_No = Convert.ToString(Showgrid.Rows[i].Cells[2].Text);

                studqry = "select r.App_No,r.Roll_No,r.Reg_No,r.Stud_Name,r.Current_Semester,r.Batch_Year,r.degree_code,r.Branch_code,el.Semester as RedoSem from Registration r,applyn a,Eligibility_list el where el.app_no=r.App_No and el.app_no=a.App_No and r.App_No=a.app_no and r.DelFlag=0 and r.CC=0 and r.Exam_Flag<>'DEBAR' and r.Roll_No='" + Roll_No + "'  and el.is_eligible='3'";
                dtstudedetails = da.selectDataTable(studqry);
                if (dtstudedetails.Rows.Count > 0)
                {
                    App_No = Convert.ToString(dtstudedetails.Rows[0]["App_No"]);
                    Stud_Name = Convert.ToString(dtstudedetails.Rows[0]["Stud_Name"]);
                    Current_Semester = Convert.ToString(dtstudedetails.Rows[0]["Current_Semester"]);
                    Batch_Year = Convert.ToString(dtstudedetails.Rows[0]["Batch_Year"]);
                    Degree_Code = Convert.ToString(dtstudedetails.Rows[0]["degree_code"]);
                    RedoSem = Convert.ToString(dtstudedetails.Rows[0]["RedoSem"]);

                    selectqry = "select * from Eligibility_list where Roll_no='" + Roll_No + "' and batch_year='" + Batch_Year + "' and degree_code='" + Degree_Code + "' and app_no='" + App_No + "' and is_eligible='3' and Semester='" + RedoSem + "'";

                    updateqry = "update Eligibility_list  set  Roll_no ='" + Roll_No + "' , batch_year='" + Batch_Year + "' , degree_code='" + Degree_Code + "' ,is_eligible='4'  where Roll_no='" + Roll_No + "' and batch_year='" + Batch_Year + "' and degree_code='" + Degree_Code + "' and app_no='" + App_No + "' and is_eligible='3'";

                    result = da.insertData("if exists (" + selectqry + ") " + updateqry);

                }
            }
        }
        if (result > 0)
        {
            btngo_Click(sender, e);
        }
    }


    //protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
           
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            if (cbl_reporttype.Items[2].Selected == true)
    //            {
                 
    //                CheckBox cbsel = (CheckBox)e.Row.Cells[8].FindControl("lbl_cb");
    //                cbsel.Checked = false;
    //                //if (strrm.Text != "")
    //                //{
    //                //    cbsel.Checked = true;
                     
    //                //}
    //            }
    //        }
    //    }
    //    catch
    //    {

    //    }

    //}

    protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {

        try
        {
            int m = 0;
            
            
            foreach (GridViewRow row in Showgrid.Rows)
            {
                int asms = Showgrid.Rows.Count;
                if (m == 141)
                {
                    int asm = Showgrid.Rows.Count;
                }
                m++;
                string val = string.Empty;
                // if (m <= dscount.Rows.Count)
                //{
                
                    
                        int a = 0;
                        val = "0" + a + "";
                    
                    string chkname = "ctl" + val + "";
                    CheckBox stud_rollno = (CheckBox)row.FindControl(chkname);
                    //(GridView1.Rows[m].FindControl(chkname) as CheckBox);

                    stud_rollno.Enabled = true;
                    //stud_rollno = (GridView1.Rows[m].Cells[4].FindControl(chkname) as CheckBox);
                    //stud_rollno = (GridView1.Rows[m].FindControl(chkname) as CheckBox);
                    //CheckBox chkRow = (row.Cells[4].FindControl(chkname) as CheckBox);
                    //Boolean sm = chkRow.Checked;
                    //if ((GridView1.Rows[m].Cells[4].FindControl(chkname) as CheckBox).Checked == false)
                    //{
                    //}
                
                // }
                // }

                // e.Row.Cells[i].Controls.Add(chk);

                #region com
                //GridView HeaderGrid = (GridView)sender;
                //GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                //TableCell HeaderCell = new TableCell();
                //HeaderCell.Text = "";
                //HeaderCell.ColumnSpan = 5;
                //TableCell HeaderCell5 = new TableCell();
                //HeaderCell5.Text = "Absent Hours";
                //HeaderCell5.HorizontalAlign = HorizontalAlign.Center;

                //DateTime datfr = new DateTime();
                //DateTime datto = new DateTime();
                //datfr = Convert.ToDateTime(datefrom);
                //datto = Convert.ToDateTime(dateto);
                //int count = 0;
                //while (datfr <= datto)
                //{
                //    count++;
                //    datfr = datfr.AddDays(1);
                //}

                //HeaderCell5.ColumnSpan = count;
                //TableCell HeaderCell6 = new TableCell();
                //HeaderCell6.Text = "";
                //HeaderCell6.ColumnSpan = 2;
                //HeaderGridRow.Cells.Add(HeaderCell);
                //HeaderGridRow.Cells.Add(HeaderCell5);
                //HeaderGridRow.Cells.Add(HeaderCell6);
                //GridView1.Controls[0].Controls.AddAt(0, HeaderGridRow);
                #endregion


                //  }
                  
            }

            // }
        }
        catch
        {
        }
    }

    public void addchk()
    {

        try
        {


            // if (optradio.Items[1].Selected == true)
            // {
            //foreach (GridViewRow row in Showgrid.Rows)
            //{
            //    if (row.RowType == DataControlRowType.DataRow)
            //    {
            //        {
            //            for (int i = 0; i < Showgrid.Rows.Count; i++)
            //            {
            //                CheckBox chk = new CheckBox();
            //                chk.EnableViewState = true;
            //                chk.Enabled = true;
            //                chk.ID = "chkb" + i + "";
            //                row.Cells[i].Controls.Add(chk);
                            
            //            }
            //        }





            //    }
            //}

            // }
        }
        catch
        {
        }
    }

    #region modified by prabha on feb  08 2018 //report modification based on requirement of mcc

    protected void txtfdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string fdate = txtfdate.Text.ToString();
            string[] spf = fdate.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            if (!string.IsNullOrEmpty(txttodate.Text))
            {
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
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Condonation Report (AT-40)");
        }
    }

    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        try
        {
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
            d2.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Condonation Report (AT-40)");
        }
    }

    protected void txtfrom_range_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            int fromrange = 0;
            int torange = 0;
            Int32.TryParse(txtfrom_range.Text.Trim(), out fromrange);
            if (txtto_range.Text.Trim() != "" || (!string.IsNullOrEmpty(txtto_range.Text.Trim())))
            {
                Int32.TryParse(txtto_range.Text.Trim(), out torange);
                if (fromrange > torange)
                {
                    txtfrom_range.Text = string.Empty;

                    errmsg.Visible = true;
                    errmsg.Text = "Please Enter The From Range That Must Be Lesser Than Or Equal To Range";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Condonation Report (AT-40)");
        }
    }

    protected void txttorange_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            int fromrange = 0;
            int torange = 0;
            Int32.TryParse(txtto_range.Text.Trim(), out torange);
            if (txtfrom_range.Text.Trim() != "" || (!string.IsNullOrEmpty(txtfrom_range.Text.Trim())))
            {
                Int32.TryParse(txtfrom_range.Text.Trim(), out fromrange);
                if (fromrange > torange)
                {
                    txtto_range.Text = string.Empty;
                    errmsg.Visible = true;
                    errmsg.Text = "Please Enter The To Range That Must Be Greater Than Or Equal To From Range";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), "Condonation Report (AT-40)");
        }
    }


    #endregion

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
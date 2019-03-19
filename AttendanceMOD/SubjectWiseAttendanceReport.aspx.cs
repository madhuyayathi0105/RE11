using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class SubjectWiseAttendanceReport : System.Web.UI.Page
{
    bool check_col_count_flag = false;
    bool btnclick_or_print = false;
    bool recflag = false;
    bool check_alter = false;
    bool chkflag = false;
    bool splhr_flag = false;
    bool isSchool = false;

    double max_tot = 0;
    double attnd_hr = 0, tot_hr = 0;

    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();

    DataSet dsHolidayList = new DataSet();
    DataSet dsHolidays = new DataSet();
    DataSet dsSplHr = new DataSet();
    DataSet dsAttndanceMaster = new DataSet();
    DataSet dsAlterSchedule = new DataSet();
    DataSet dsSemesterSchedule = new DataSet();
    static DataSet dsprint = new DataSet();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();

    DateTime dtFromDate = new DateTime();
    DateTime dtToDate = new DateTime();
    DateTime dtSemStartDate = new DateTime();
    DateTime dtSemEndDate = new DateTime();
    DateTime dtTempDate = new DateTime();
    DateTime dtAdmissionDate = new DateTime();
    DateTime temp_date = new DateTime();
    DateTime Admission_date;

    Dictionary<string, DateTime> dicStudentAdmDate = new Dictionary<string, DateTime>();

    Hashtable hashmark = new Hashtable();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    Hashtable htSubjectType = new Hashtable();
    Hashtable hatattendance = new Hashtable();
    Hashtable hatdc = new Hashtable();
    Hashtable has_od = new Hashtable();
    Hashtable hat_holy = new Hashtable();
    Hashtable htHoliday = new Hashtable();
    Hashtable hatsplhrattendance = new Hashtable();
    Hashtable has_total_attnd_hour = new Hashtable();
    Hashtable has_load_rollno = new Hashtable();
    Hashtable htSplHr = new Hashtable();
    Hashtable has_attnd_masterset_notconsider = new Hashtable();
    Hashtable has_attnd_masterset = new Hashtable();
    Hashtable holiday = new Hashtable();
    Hashtable temp_has_subj_code = new Hashtable();
    static Hashtable has_subtype = new Hashtable();

    int noOfHours = 0;
    int firstHalf = 0;
    int secondHalf = 0;
    int mng_hrs = 0, evng_hrs = 0;
    int od_count = 0;
    int between_visible_col_cnt = 0, between_visible_col_cnt_bal = 0;
    int x = 0;
    int visi_col = 0, visi_col1 = 0;
    int count_master = 0;
    int start_column = 0, end_column = 0;
    int new_header_count = 0;
    int temp_count = 0, final_print_col_cnt = 0, split_col_for_footer = 0, col_count = 0, footer_balanc_col = 0, footer_count = 0;
    int col_count_all = 0, span_cnt = 0, child_span_count = 0;
    int split_holiday_status_1 = 0, split_holiday_status_2 = 0;
    int temp_stud_count = 0;
    int row_count = 0;
    int stud_count = 0;
    int span_count = 0;
    int present_count = 0;
    int roll_count = 0;
    int selDegree = 0;
    int selBranch = 0;
    int selSec = 0;
    int selSubject = 0;
    static int isHeaderwise = 0;

    string group_user = string.Empty;
    string singleuser = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string collegecode1 = string.Empty;
    string user_code = string.Empty;
    string collegeCode = string.Empty;
    string newCollegeCode = string.Empty;
    string newBatchYear = string.Empty;
    string newDegreeCode = string.Empty;
    string newBranchCode = string.Empty;
    string newsemester = string.Empty;
    string newsections = string.Empty;
    string subjectNo = string.Empty;
    string qryCollege = string.Empty;
    string qryBatch = string.Empty;
    string qryDegree = string.Empty;
    string qryBranch = string.Empty;
    string qrySem = string.Empty;
    string qrySec = string.Empty;
    string qrySubjectNo = string.Empty;
    string qry = string.Empty;
    string roll_no = string.Empty;
    string strDay = string.Empty;
    string dummy_date = string.Empty;
    string temp_hr_field = string.Empty;
    string subject_no = string.Empty;
    string full_hour = string.Empty;
    string single_hour = string.Empty;
    string order = string.Empty;
    string halforFull = string.Empty;
    string mng = string.Empty;
    string evng = string.Empty;
    string semStartDate = string.Empty;
    string holiday_sched_details = string.Empty;
    string new_header_string_index = string.Empty;
    string isonumber = string.Empty;
    string new_header_string = string.Empty;
    string column_field = string.Empty;
    string printvar = string.Empty;
    string view_footer = string.Empty;
    string view_header = string.Empty;
    string view_footer_text = string.Empty;
    string coll_name = string.Empty;
    string address1 = string.Empty;
    string address2 = string.Empty;
    string address3 = string.Empty;
    string form_name = string.Empty;
    string phoneno = string.Empty;
    string faxno = string.Empty;
    string email = string.Empty;
    string website = string.Empty;
    string degree_val = string.Empty;
    string footer_text = string.Empty;
    string header_alignment = string.Empty;
    string degree_deatil = string.Empty;
    string phone = string.Empty;
    string fax = string.Empty;
    string email_id = string.Empty;
    string web_add = string.Empty;
    string get_date_holiday = string.Empty;
    string subj_type = string.Empty;
    string date1 = string.Empty;
    string datefrom = string.Empty;
    string date2 = string.Empty;
    string dateto = string.Empty;
    string halforfull = string.Empty;
    string value_holi_status = string.Empty;
    string strsec = string.Empty;
    string date_temp_field = string.Empty;
    string month_year = string.Empty;
    string Att_mark;
    string section_lab = string.Empty;
    string present_calcflag = string.Empty;
    static string grouporusercode = string.Empty;

    string[] string_session_values = new string[100];
    string[] new_header_string_split;
    string[] split_holiday_status = new string[1000];

    //added by rajasekar 20/09/2018
    DataTable dtl = new DataTable();
    DataRow dtrow = null;

    string [] admissiondate;
    string[] subcode;
    int coln = 0;
    System.Text.StringBuilder ConHrs = new System.Text.StringBuilder();
    System.Text.StringBuilder Present = new System.Text.StringBuilder();
    System.Text.StringBuilder OD = new System.Text.StringBuilder();
    System.Text.StringBuilder TotHrs = new System.Text.StringBuilder();
    System.Text.StringBuilder Absent = new System.Text.StringBuilder();
    System.Text.StringBuilder per = new System.Text.StringBuilder();
    
    //=================================//

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.Header.DataBind();
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            usercode = Convert.ToString(Session["usercode"]).Trim();
            collegecode = Convert.ToString(Session["collegecode"]).Trim();
            singleuser = Convert.ToString(Session["single_user"]).Trim();
            group_user = Convert.ToString(Session["group_code"]).Trim();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            string grouporusercode1 = string.Empty;
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode1 = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
            }
            else
            {
                grouporusercode1 = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode1 + "";
            schoolds.Clear();
            schoolds.Dispose();
            schoolds = d2.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = Convert.ToString(schoolds.Tables[0].Rows[0]["value"]).Trim();
                if (schoolvalue.Trim() == "0")
                {
                    isSchool = true;
                }
            }
            if (!IsPostBack)
            {
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                #region LoadHeader
                BindCollege();
                BindBatch();
                BindDegree();
                BindBranch();
                BindSem();
                BindSectionDetail();
                BindSubjects();
                #endregion LoadHeader
                lbl_norec1.Visible = false;
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                Session["AdmissionNo"] = "0";
                string grouporusercode = string.Empty;
                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                }
                user_code = Convert.ToString(Session["usercode"]).Trim();
                string Master = "select * from Master_Settings where " + grouporusercode + "";
                DataSet ds = d2.select_method(Master, hat, "Text");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "roll no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "register no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "student_type" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "admission no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                    {
                        Session["AdmissionNo"] = "1";
                    }
                    //Admission No
                }
                ChangeHeaderName(isSchool);
            }
            collegecode1 = ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13");
            if (Session["usercode"] != null)
            {
                user_code = Convert.ToString(Session["usercode"]).Trim();
                usercode = user_code;
            }
            if (Session["single_user"] != null)
            {
                singleuser = Convert.ToString(Session["single_user"]).Trim();
            }
        }
        catch (ThreadAbortException tt)
        {
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Page Load

    #region Bind Header

    public void ChangeHeaderName(bool isschool)
    {
        try
        {
            lblErrmsg.Visible = false;
            lblErrmsg.Text = string.Empty;
            lblCollege.Text = ((!isschool) ? "College" : "School");
            lblBatch.Text = ((!isschool) ? "Batch" : "Year");
            lblDegree.Text = ((!isschool) ? "Degree" : "School Type");
            lblBranch.Text = ((!isschool) ? "Branch" : "Standard");
            lblSem.Text = ((!isschool) ? "Semester" : "Term");
            lblSec.Text = ((!isschool) ? "Section" : "Section");
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindCollege()
    {
        try
        {
            string group_code = Convert.ToString(Session["group_code"]).Trim();
            string columnfield = string.Empty;
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = Convert.ToString(group_semi[0]).Trim();
            }
            if ((Convert.ToString(group_code).Trim() != "") && (Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true" && Convert.ToString(Session["single_user"]).Trim() != "TRUE" && Convert.ToString(Session["single_user"]).Trim() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            hat.Clear();
            hat.Add("column_field", Convert.ToString(columnfield));
            ds = da.select_method("bind_college", hat, "sp");
            ddlCollege.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.Enabled = true;
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindBatch()
    {
        try
        {
            ddlBatch.Items.Clear();
            ds = da.BindBatch();
            if (ds.Tables.Count > 0)
            {
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    ddlBatch.DataSource = ds;
                    ddlBatch.DataTextField = "batch_year";
                    ddlBatch.DataValueField = "batch_year";
                    ddlBatch.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindDegree()
    {
        try
        {
            ddlDegree.Items.Clear();
            cblDegree.Items.Clear();
            chkDegree.Checked = false;
            txtDegree.Text = "-- Select --";
            usercode = Convert.ToString(Session["usercode"]).Trim();
            singleuser = Convert.ToString(Session["single_user"]).Trim();
            group_user = Convert.ToString(Session["group_code"]).Trim();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }
            has.Clear();
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("college_code", Convert.ToString(ddlCollege.SelectedValue).Trim());
            has.Add("user_code", usercode);
            ds = da.select_method("bind_degree", has, "sp");
            if (ds.Tables.Count > 0)
            {
                int count1 = ds.Tables[0].Rows.Count;
                if (count1 > 0)
                {
                    ddlDegree.DataSource = ds;
                    ddlDegree.DataTextField = "course_name";
                    ddlDegree.DataValueField = "course_id";
                    ddlDegree.DataBind();
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
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindBranch()
    {
        try
        {
            ddlBranch.Items.Clear();
            chkBranch.Checked = false;
            cblBranch.Items.Clear();
            txtBranch.Text = "-- Select --";
            hat.Clear();
            group_user = Convert.ToString(Session["group_code"]).Trim();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddlDegree.SelectedValue);
            hat.Add("college_code", Convert.ToString(ddlCollege.SelectedValue).Trim());
            hat.Add("user_code", usercode);
            //string typeval =string.Empty;
            //if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
            //{
            //    typeval = " and type='" + ddlstream.SelectedItem.ToString() + "'";
            //}
            selDegree = 0;
            newDegreeCode = string.Empty;
            qryDegree = string.Empty;
            string coursecode = string.Empty;
            foreach (ListItem li in ddlDegree.Items)
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
                string strquery = "select distinct degree.degree_code,de.dept_name from degree,course, deptprivilages,department de where course.course_id=degree.course_id and de.dept_code=degree.dept_code and course.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and course.college_code ='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "' and user_code='" + usercode + "' " + " " + coursecode + "";
                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    strquery = "select distinct degree.degree_code,de.dept_name from degree,course, deptprivilages,department de where course.course_id=degree.course_id and de.dept_code=degree.dept_code and course.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "' and course.college_code ='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "' " + "  " + coursecode + "";
                }
                ds = d2.select_method_wo_parameter(strquery, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count2 = ds.Tables[0].Rows.Count;
                if (count2 > 0)
                {
                    ddlBranch.DataSource = ds;
                    ddlBranch.DataTextField = "dept_name";
                    ddlBranch.DataValueField = "degree_code";
                    ddlBranch.DataBind();
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
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindSem()
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            string strbatchyear = string.Empty;
            string strbranch = string.Empty;
            ddlSem.Items.Clear();
            bool first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            if (ddlBatch.Items.Count > 0)
            {
                strbatchyear = Convert.ToString(ddlBatch.SelectedValue).Trim();
            }
            newDegreeCode = string.Empty;
            selBranch = 0;
            foreach (ListItem li in cblBranch.Items)
            {
                if (li.Selected)
                {
                    selBranch++;
                    if (string.IsNullOrEmpty(newDegreeCode))
                    {
                        newDegreeCode = "'" + li.Value + "'";
                    }
                    else
                    {
                        newDegreeCode += ",'" + li.Value + "'";
                    }
                }
            }
            ds.Dispose();
            ds.Reset();
            ddlSem.Items.Clear();
            string qry = string.Empty;
            //ddlSem.Items.Count = 0;
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(newDegreeCode) && !string.IsNullOrEmpty(strbatchyear))
            {
                qry = "select distinct max(ndurations) as ndurations,first_year_nonsemester from ndegree where degree_code in (" + newDegreeCode + ") and batch_year in (" + strbatchyear + ") and college_code='" + collegeCode + "' group by first_year_nonsemester order by ndurations desc; select distinct max(duration) duration,first_year_nonsemester from degree where degree_code in (" + newDegreeCode + ") and college_code='" + collegeCode + "' group by first_year_nonsemester order by duration desc";
                ds = da.select_method_wo_parameter(qry, "Text");
                //ds = d2.BindSem(newDegreeCode, strbatchyear, collegeCode);
            }
            if (ds.Tables.Count > 0)
            {
                //first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]));
                //duration = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0][0]));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                    int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlSem.Items.Add(Convert.ToString(i));
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlSem.Items.Add(Convert.ToString(i));
                        }
                    }
                }
                else if (ds.Tables[1].Rows.Count > 0)
                {
                    bool.TryParse(Convert.ToString(ds.Tables[1].Rows[0][1]).Trim(), out first_year);
                    int.TryParse(Convert.ToString(ds.Tables[1].Rows[0][0]).Trim(), out duration);
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlSem.Items.Add(Convert.ToString(i));
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlSem.Items.Add(Convert.ToString(i));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindSectionDetail()
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            string strbatch = Convert.ToString(ddlBatch.SelectedValue).Trim();
            string strbranch = Convert.ToString(ddlBranch.SelectedValue).Trim();
            ddlSec.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSectionDetail(strbatch, strbranch);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSec.DataSource = ds;
                ddlSec.DataTextField = "sections";
                ddlSec.DataValueField = "sections";
                ddlSec.DataBind();
                if (Convert.ToString(ds.Tables[0].Columns["sections"]).Trim() == string.Empty)
                {
                    ddlSec.Enabled = false;
                }
                else
                {
                    ddlSec.Enabled = true;
                }
            }
            else
            {
                ddlSec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindSubjects()
    {
        try
        {
            string staff_code = string.Empty;
            chkSubject.Checked = false;
            cblSubject.Items.Clear();
            txtSubject.Text = "-- Select --";
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divAttendanceReport.Visible = false;
            int subjectCount = 0;
            has.Clear();
            has.Add("Batch_Year", Convert.ToString(ddlBatch.SelectedValue).Trim());
            has.Add("DegCode", Convert.ToString(ddlBranch.SelectedValue).Trim());
            has.Add("Sems", Convert.ToString(ddlSem.SelectedItem).Trim());
            has.Add("staffcode", Convert.ToString(Session["Staff_Code"]).Trim());
            if (ddlSec.Items.Count > 0)
            {
                if (Convert.ToString(ddlSec.SelectedValue).Trim() == "" || Convert.ToString(ddlSec.SelectedValue).Trim() == "-1" || ddlSec.Enabled == false)
                {
                    has.Add("sec", "");
                }
                else
                {
                    has.Add("sec", Convert.ToString(ddlSec.SelectedValue).Trim());
                }
            }
            else
            {
                has.Add("sec", "");
            }
            ds = da.select_method("single_subjectwise_attnd", has, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                subjectCount = (ds.Tables[0].Rows.Count);
            }
            if (subjectCount > 0)
            {
                txtSubject.Enabled = true;
                cblSubject.DataSource = ds;
                cblSubject.DataTextField = "subject_name";
                cblSubject.DataValueField = "subject_no";
                cblSubject.DataBind();
                foreach (ListItem li in cblSubject.Items)
                {
                    li.Selected = true;
                }
                txtSubject.Text = "Subject" + "(" + cblSubject.Items.Count + ")";
                chkSubject.Checked = true;
                htSubjectType.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (!htSubjectType.ContainsKey(Convert.ToString(ds.Tables[0].Rows[i]["subject_no"]).Trim()))
                    {
                        htSubjectType.Add(Convert.ToString(ds.Tables[0].Rows[i]["subject_no"]).Trim(), Convert.ToString(ds.Tables[0].Rows[i]["subject_type"]).Trim());
                    }
                }
            }
            else
            {
                txtSubject.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

    #region Header Changed Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divAttendanceReport.Visible = false;
            BindBatch();
            BindDegree();
            BindBranch();
            BindSem();
            BindSectionDetail();
            BindSubjects();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divAttendanceReport.Visible = false;
            BindDegree();
            BindBranch();
            BindSem();
            BindSectionDetail();
            BindSubjects();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divAttendanceReport.Visible = false;
            BindBranch();
            BindSem();
            BindSectionDetail();
            BindSubjects();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divAttendanceReport.Visible = false;
            BindSem();
            BindSectionDetail();
            BindSubjects();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divAttendanceReport.Visible = false;
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
            BindBranch();
            BindSem();
            BindSectionDetail();
            BindSubjects();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divAttendanceReport.Visible = false;
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
            BindBranch();
            BindSem();
            BindSectionDetail();
            BindSubjects();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divAttendanceReport.Visible = false;
            int count = 0;
            if (chkBranch.Checked == true)
            {
                for (int i = 0; i < cblBranch.Items.Count; i++)
                {
                    count++;
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
            BindSem();
            BindSectionDetail();
            BindSubjects();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divAttendanceReport.Visible = false;
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
            BindSem();
            BindSectionDetail();
            BindSubjects();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divAttendanceReport.Visible = false;
            BindSectionDetail();
            BindSubjects();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divAttendanceReport.Visible = false;
            BindSubjects();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkSubject_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divAttendanceReport.Visible = false;
            int count = 0;
            txtSubject.Text = "-- Select --";
            cblSubject.ClearSelection();
            if (chkSubject.Checked == true)
            {
                foreach (ListItem li in cblSubject.Items)
                {
                    count++;
                    li.Selected = true;
                }
                txtSubject.Text = "Subject (" + (cblSubject.Items.Count) + ")";
            }
            else
            {
                foreach (ListItem li in cblSubject.Items)
                {
                    count++;
                    li.Selected = false;
                }
                txtSubject.Text = "-- Select --";
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divAttendanceReport.Visible = false;
            int commcount = 0;
            txtSubject.Text = "-- Select --";
            chkSubject.Checked = false;
            foreach (ListItem li in cblSubject.Items)
            {
                if (li.Selected)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblSubject.Items.Count)
                {
                    chkSubject.Checked = true;
                }
                txtSubject.Text = "Subject (" + Convert.ToString(commcount) + ")";
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divAttendanceReport.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divAttendanceReport.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

    #region Generate Excel

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            
            string reportname = txtexcelname1.Text.Trim().Replace(" ", "_");
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (Showgrid.Visible == true)
                {
                    
                    da.printexcelreportgrid(Showgrid, reportname);
                }

                
                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Generate Excel

    #region Print PDF

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string sections = string.Empty;
            if (ddlSec.Items.Count > 0)
            {
                sections = Convert.ToString(ddlSec.SelectedItem.Text).Trim();
                if (Convert.ToString(sections).Trim().ToLower() == "all" || Convert.ToString(sections).Trim().ToLower() == string.Empty || Convert.ToString(sections).Trim().ToLower() == "-1")
                {
                    sections = string.Empty;
                }
                else
                {
                    sections = "- Sec-" + sections;
                }
            }
            string rptheadname = "Subject Wise Attendance With Percentage Report" + "@ Degree :" + ddlBatch.SelectedItem.ToString() + '-' + ddlDegree.SelectedItem.ToString() + '-' + ddlBranch.SelectedItem.ToString() + '-' + "Sem-" + ddlSem.SelectedItem.ToString() + sections + '@' + "Date :" + txtFromDate.Text.ToString() + " To " + txtToDate.Text.ToString();
            string pagename = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString();
            string degreedetails = "";
            //if (FpAttendanceReport.Visible == true)
            //{
            //    Printcontrol1.loadspreaddetails(FpAttendanceReport, pagename, rptheadname);
            //}
            string ss = null;
            Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
            Printcontrol.Visible = true;
            lbl_norec1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Print PDF

    #region Popup Close

    protected void btnPopupClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            lblPopupAlert.Text = string.Empty;
            divPopupAlert.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Popup Close

    #region Go Click

    public void Init_Spread()
    {
        try
        {
            #region FpSpread Style

            Showgrid.Visible = false;

            
            #endregion FpSpread Style

            

            #region SpreadStyles

            
            

            
            

            #endregion SpreadStyles

            

            //FpAttendanceReport.Sheets[0].AutoPostBack = false;
            //FpAttendanceReport.Sheets[0].AutoPostBack = true;

            

            //added by rajasekar 18/09/2018

            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);

            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
            int colu = 0;

            dtl.Columns.Add("S.No", typeof(string));
            dtl.Rows[0][colu] = "S.No";
            colu++;

            if (Session["AdmissionNo"] != null && Convert.ToString(Session["AdmissionNo"]).Trim() == "1")
            {
                
                dtl.Columns.Add("Admission No", typeof(string));
                dtl.Rows[0][colu] = "Admission No";
                colu++;
            }
            
            if (Session["Rollflag"] != null && Convert.ToString(Session["Rollflag"]).Trim() == "1")
            {
                
                dtl.Columns.Add("Roll No", typeof(string));
                dtl.Rows[0][colu] = "Roll No";
                colu++;
            }
            
            if (Session["Regflag"] != null && Convert.ToString(Session["Regflag"]).Trim() == "1")
            {
                
                dtl.Columns.Add("Reg No", typeof(string));
                dtl.Rows[0][colu] = "Reg No";
                colu++;
            }
           
            if (Session["Studflag"] != null && Convert.ToString(Session["Studflag"]).Trim() == "1")
            {
                
                dtl.Columns.Add("Student Type", typeof(string));
                dtl.Rows[0][colu] = "Student Type";
                colu++;
            }
            
            

            dtl.Columns.Add("Student Name", typeof(string));
            dtl.Rows[0][colu] = "Student Name";
            colu++;

            ViewState["temp_table"] = dtl.Columns.Count;
            //================================//
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            string qrysplHr = string.Empty;
            string splHr = string.Empty;
            lbl_norec1.Visible = false;
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divAttendanceReport.Visible = false;
            selBranch = 0;
            selDegree = 0;
            selSubject = 0;
            string[] subjectName = new string[1];
            newCollegeCode = string.Empty;
            newBatchYear = string.Empty;
            newBranchCode = string.Empty;
            newDegreeCode = string.Empty;
            newsemester = string.Empty;
            newsections = string.Empty;
            collegeCode = string.Empty;
            qryCollege = string.Empty;
            qryBatch = string.Empty;
            qryDegree = string.Empty;
            qryBranch = string.Empty;
            qrySem = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                newCollegeCode = collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else
            {
                lblPopupAlert.Text = "No College were Found";
                lblPopupAlert.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (ddlBatch.Items.Count > 0)
            {
                newBatchYear = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
            }
            else
            {
                lblPopupAlert.Text = "No Batch were Found";
                lblPopupAlert.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (ddlDegree.Items.Count > 0)
            {
                newDegreeCode = string.Empty;
                selDegree = 0;
                qryDegree = string.Empty;
                foreach (ListItem li in ddlDegree.Items)
                {
                    if (li.Selected)
                    {
                        selDegree++;
                        if (string.IsNullOrEmpty(newDegreeCode))
                        {
                            newDegreeCode = "'" + Convert.ToString(li.Value).Trim() + "'";
                        }
                        else
                        {
                            newDegreeCode += ",'" + Convert.ToString(li.Value).Trim() + "'";
                        }
                    }
                }
                if (selDegree == 0)
                {
                    lblPopupAlert.Text = "Please Select Any One Degree";
                    lblPopupAlert.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblPopupAlert.Text = "No Degree were Found";
                lblPopupAlert.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (ddlBranch.Items.Count > 0)
            {
                newBranchCode = string.Empty;
                selBranch = 0;
                qryBranch = string.Empty;
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        selBranch++;
                        if (string.IsNullOrEmpty(newBranchCode))
                        {
                            newBranchCode = "'" + Convert.ToString(li.Value).Trim() + "'";
                        }
                        else
                        {
                            newBranchCode += ",'" + Convert.ToString(li.Value).Trim() + "'";
                        }
                    }
                }
                if (selBranch == 0)
                {
                    lblPopupAlert.Text = "Please Select Any One Branch";
                    lblPopupAlert.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblPopupAlert.Text = "No Branch were Found";
                lblPopupAlert.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (ddlSem.Items.Count > 0)
            {
                newsemester = Convert.ToString(ddlSem.SelectedItem.Text).Trim();
                qrySem = string.Empty;
            }
            else
            {
                lblPopupAlert.Text = "No Semester were Found";
                lblPopupAlert.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            newsections = string.Empty;
            qrySec = string.Empty;
            string strsec = string.Empty;
            string splhrsec = string.Empty;
            selSec = 0;
            if (ddlSec.Items.Count > 0)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(ddlSec.SelectedItem.Text)) && Convert.ToString(ddlSec.SelectedItem.Text).Trim() != "-1" && Convert.ToString(ddlSec.SelectedItem.Text).Trim() != "" && ddlSec.Enabled != false)
                {
                    newsections = Convert.ToString(ddlSec.SelectedItem.Text).Trim();
                }
                else
                {
                    newsections = string.Empty;
                }
                if (Convert.ToString(ddlSec.SelectedItem.Text).Trim() == "-1" || Convert.ToString(ddlSec.SelectedItem.Text).Trim() == "")
                {
                    strsec = string.Empty;
                    section_lab = string.Empty;
                    splhrsec = string.Empty;
                }
                else
                {
                    strsec = " and r.sections='" + Convert.ToString(ddlSec.SelectedItem.Text).Trim() + "'";
                    splhrsec = " and sections='" + Convert.ToString(ddlSec.SelectedItem.Text).Trim() + "'";
                    section_lab = " and l.sections='" + Convert.ToString(ddlSec.SelectedItem.Text).Trim() + "'";
                }
            }
            else
            {
                newsections = string.Empty;
            }
            if (cblSubject.Items.Count > 0)
            {
                selSubject = 0;
                subjectNo = string.Empty;
                foreach (ListItem li in cblSubject.Items)
                {
                    if (li.Selected)
                    {
                        selSubject++;
                        Array.Resize(ref subjectName, selSubject);
                        subjectName[selSubject - 1] = li.Text.Trim();
                        if (string.IsNullOrEmpty(subjectNo))
                        {
                            subjectNo = "'" + Convert.ToString(li.Value).Trim() + "'";
                        }
                        else
                        {
                            subjectNo += ",'" + Convert.ToString(li.Value).Trim() + "'";
                        }
                    }
                }
                if (selSubject == 0)
                {
                    lblPopupAlert.Text = "Please Select Any One Report Type";
                    lblPopupAlert.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblPopupAlert.Text = "No Report were Found";
                lblPopupAlert.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            dtFromDate = new DateTime();
            string fromDate = txtFromDate.Text.Trim();
            string toDate = txtToDate.Text.Trim();
            if (!string.IsNullOrEmpty(fromDate.Trim()))
            {
                if (!DateTime.TryParseExact(fromDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate))
                {
                    lblPopupAlert.Text = "From Date Must Be in the Format dd/MM/yyyy";
                    lblPopupAlert.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblPopupAlert.Text = "Please Select From Date";
                lblPopupAlert.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (!string.IsNullOrEmpty(toDate.Trim()))
            {
                if (!DateTime.TryParseExact(toDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate))
                {
                    lblPopupAlert.Text = "To Date Must Be in the Format dd/MM/yyyy";
                    lblPopupAlert.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblPopupAlert.Text = "Please Select To Date";
                lblPopupAlert.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (dtToDate < dtFromDate)
            {
                lblPopupAlert.Text = "From Date Must Be Lesser Than To Date";
                lblPopupAlert.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            string strOrderBy = string.Empty;
            string qry = string.Empty;
            DataSet dsStudentsList = new DataSet();
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(newBatchYear) && !string.IsNullOrEmpty(newBranchCode) && !string.IsNullOrEmpty(newsemester) && !string.IsNullOrEmpty(subjectNo))
            {
                string[] subjectNoSelected = subjectNo.Replace("'", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string sex = "0";
                string flag = "-1";
                string Master = string.Empty;
                string strdayflag = string.Empty;
                string genderflag = string.Empty;
                string regularflag = string.Empty;
                string grouporusercode = string.Empty;
                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0] + "'";
                }
                else
                {
                    grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                }
                string qryfilter = string.Empty;
                Master = "select * from Master_Settings where " + grouporusercode + "";
                ds.Clear();
                ds.Dispose();
                ds.Reset();
                ds = d2.select_method_wo_parameter(Master, "Text");
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
                string orderBySetting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                strOrderBy = " ORDER BY r.roll_no";
                string serialno = d2.GetFunction("select LinkValue from inssettings where college_code='" + collegeCode + "' and linkname='Student Attendance'");
                if (serialno == "1")
                {
                    strOrderBy = "ORDER BY r.serialno";
                }
                else
                {
                    if (orderBySetting == "0")
                    {
                        strOrderBy = "ORDER BY r.roll_no";
                    }
                    else if (orderBySetting == "1")
                    {
                        strOrderBy = "ORDER BY r.Reg_No";
                    }
                    else if (orderBySetting == "2")
                    {
                        strOrderBy = "ORDER BY r.Stud_Name";
                    }
                    else if (orderBySetting == "0,1,2")
                    {
                        strOrderBy = "ORDER BY r.roll_no,r.Reg_No,r.Stud_Name";
                    }
                    else if (orderBySetting == "0,1")
                    {
                        strOrderBy = "ORDER BY r.roll_no,r.Reg_No";
                    }
                    else if (orderBySetting == "1,2")
                    {
                        strOrderBy = "ORDER BY r.Reg_No,r.Stud_Name";
                    }
                    else if (orderBySetting == "0,2")
                    {
                        strOrderBy = "ORDER BY r.roll_no,r.Stud_Name";
                    }
                }
                has.Clear();
                has.Add("from_date", dtFromDate);
                has.Add("to_date", dtToDate);
                has.Add("degree_code", newBranchCode.Replace("'", ""));
                has.Add("sem", newsemester);
                has.Add("coll_code", collegeCode);
                int iscount = 0;
                string qryHoliday = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + dtFromDate.ToString() + "' and '" + dtToDate.ToString() + "' and degree_code='" + newBranchCode.Replace("'", "") + "' and semester='" + newsemester + "'";
                dsHolidays = d2.select_method_wo_parameter(qryHoliday, "Text");
                if (dsHolidays.Tables.Count > 0 && dsHolidays.Tables[0].Rows.Count > 0)
                {
                    int.TryParse(Convert.ToString(dsHolidays.Tables[0].Rows[0]["cnt"]).Trim(), out iscount);
                }
                has.Add("iscount", iscount);
                dsHolidayList = d2.select_method("HOLIDATE_DETAILS_FINE", has, "sp");
                if (dsHolidayList.Tables.Count > 0 && dsHolidayList.Tables[0].Rows.Count > 0)
                {
                    for (int holi = 0; holi < dsHolidayList.Tables[0].Rows.Count; holi++)
                    {
                        if (Convert.ToString(dsHolidayList.Tables[0].Rows[holi]["halforfull"]).Trim().ToLower() == "false" || Convert.ToString(dsHolidayList.Tables[0].Rows[holi]["halforfull"]).Trim().ToLower() == "0")
                        {
                            halforFull = "0";
                        }
                        else
                        {
                            halforFull = "1";
                        }
                        if (Convert.ToString(dsHolidayList.Tables[0].Rows[holi]["morning"]).Trim().ToLower() == "false" || Convert.ToString(dsHolidayList.Tables[0].Rows[holi]["morning"]).Trim().ToLower() == "0")
                        {
                            mng = "0";
                        }
                        else
                        {
                            mng = "1";
                        }
                        if (Convert.ToString(dsHolidayList.Tables[0].Rows[holi]["evening"]).Trim().ToLower() == "false" || Convert.ToString(dsHolidayList.Tables[0].Rows[holi]["evening"]).Trim().ToLower() == "0")
                        {
                            evng = "0";
                        }
                        else
                        {
                            evng = "1";
                        }
                        holiday_sched_details = halforFull + "*" + mng + "*" + evng;
                        if (!htHoliday.ContainsKey(Convert.ToDateTime(Convert.ToString(dsHolidayList.Tables[0].Rows[holi]["HOLI_DATE"]).Trim())))
                        {
                            htHoliday.Add(Convert.ToDateTime(Convert.ToString(dsHolidayList.Tables[0].Rows[holi]["HOLI_DATE"]).Trim()), holiday_sched_details);
                        }
                    }
                }
                qry = " select distinct r.app_no,r.Roll_Admit,r.Stud_Type,r.roll_no as 'ROLL NO', r.stud_name as 'STUD NAME', r.reg_no as 'REG NO',p.No_of_hrs_per_day as 'PER DAY',schorder as 'order',start_date,no_of_hrs_I_half_day,no_of_hrs_II_half_day,len(a.roll_no), convert(varchar(15),adm_date,103) as adm_date,r.serialno FROM attendance a , registration r , Department d ,  PeriodAttndSchedule p  ,seminfo s,applyn app,subjectchooser sc WHERE a.roll_no=r.roll_no and sc.roll_no=r.roll_no and sc.subject_no in(" + subjectNo + ") and r.degree_code=p.degree_code and  r.Batch_Year=" + newBatchYear + "  and  s.batch_Year=" + newBatchYear + "  and r.degree_code= " + newBranchCode + " and s.degree_code= " + newBranchCode + " and  s.semester=" + newsemester + " and p.semester=" + newsemester + "  and (r.CC = 0)  AND (r.DelFlag = 0)  AND (r.Exam_Flag <> 'debar') AND (r.Current_Semester IS NOT NULL) and  r.app_no=app.app_no " + strsec + qryfilter + " " + strOrderBy + "  ";
                dsStudentsList = d2.select_method_wo_parameter(qry, "text");
                int sno = 1;
                if (dsStudentsList.Tables.Count > 0 && dsStudentsList.Tables[0].Rows.Count > 0)
                {
                    Init_Spread();
                    semStartDate = Convert.ToString(dsStudentsList.Tables[0].Rows[0]["start_date"]).Trim();
                    order = Convert.ToString(dsStudentsList.Tables[0].Rows[0]["order"]).Trim();
                    int.TryParse(Convert.ToString(dsStudentsList.Tables[0].Rows[0]["PER DAY"]).Trim(), out noOfHours);
                    int.TryParse(Convert.ToString(dsStudentsList.Tables[0].Rows[0]["no_of_hrs_I_half_day"]).Trim(), out firstHalf);
                    int.TryParse(Convert.ToString(dsStudentsList.Tables[0].Rows[0]["no_of_hrs_II_half_day"]).Trim(), out secondHalf);
                    if (noOfHours > 0)
                    {
                        int rowco=Convert.ToInt32(dsStudentsList.Tables[0].Rows.Count);
                        admissiondate = new string[rowco];
                        foreach (DataRow drStudent in dsStudentsList.Tables[0].Rows)
                        {
                            int rowCount = 0;
                            string serialNo = Convert.ToString(drStudent["serialno"]).Trim();
                            string rollNo = Convert.ToString(drStudent["ROLL NO"]).Trim();
                            string regNo = Convert.ToString(drStudent["REG NO"]).Trim();
                            string appNo = Convert.ToString(drStudent["app_no"]).Trim();
                            string admissionNo = Convert.ToString(drStudent["Roll_Admit"]).Trim();
                            string admissionDate = Convert.ToString(drStudent["adm_date"]).Trim();
                            string studentName = Convert.ToString(drStudent["STUD NAME"]).Trim();
                            string studentType = Convert.ToString(drStudent["Stud_Type"]).Trim();
                            dtAdmissionDate = new DateTime();
                            if (DateTime.TryParseExact(admissionDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtAdmissionDate))
                            {
                                if (!dicStudentAdmDate.ContainsKey(rollNo.Trim().ToLower()))
                                {
                                    dicStudentAdmDate.Add(rollNo.Trim().ToLower(), dtAdmissionDate);
                                }
                            }
                            else
                            {
                                if (!dicStudentAdmDate.ContainsKey(rollNo.Trim().ToLower()))
                                {
                                    dicStudentAdmDate.Add(rollNo.Trim().ToLower(), dtAdmissionDate);
                                }
                            }
                            if (!has_load_rollno.ContainsKey(rollNo.Trim().ToLower()))
                            {
                                has_load_rollno.Add(rollNo.Trim().ToLower(), 0);
                            }
                            if (!has_total_attnd_hour.ContainsKey(rollNo.Trim().Trim().ToLower()))
                            {
                                has_total_attnd_hour.Add(rollNo.Trim().ToLower(), 0);
                            }
                           
                            rowCount = (dtl.Rows.Count+1) - 3;
                            if (!hatsplhrattendance.Contains(rollNo.Trim().ToLower()))
                            {
                                hatsplhrattendance.Add(rollNo.Trim().ToLower(), rowCount);
                            }
                            
                            //string admdate = admissionDate;// ds_student.Tables[0].Rows[row_count]["adm_date"].ToString();
                            //string[] admdatesp = admdate.Split(new Char[] { '/' });
                            //admdate = admdatesp[2].ToString() + "/" + admdatesp[1].ToString() + "/" + admdatesp[0].ToString();
                           



                            dtrow = dtl.NewRow();
                            coln = 0;

                            dtrow[coln] = Convert.ToString(sno).Trim();
                            coln++;
                            admissiondate[sno-1] = Convert.ToString(dtAdmissionDate.ToString("yyyy/MM/dd")).Trim();
                            if (Session["AdmissionNo"] != null && Convert.ToString(Session["AdmissionNo"]).Trim() == "1")
                            {
                                dtrow[coln] = Convert.ToString(admissionNo).Trim();
                                coln++;
                            }
                            
                            if (Session["Rollflag"] != null && Convert.ToString(Session["Rollflag"]).Trim() == "1")
                            {
                                dtrow[coln] = Convert.ToString(rollNo).Trim();
                                coln++;
                            }
                            
                            if (Session["Regflag"] != null && Convert.ToString(Session["Regflag"]).Trim() == "1")
                            {
                                dtrow[coln] = Convert.ToString(regNo).Trim();
                                coln++;
                            }
                            
                            if (Session["Studflag"] != null && Convert.ToString(Session["Studflag"]).Trim() == "1")
                            {
                                dtrow[coln] = Convert.ToString(studentType).Trim();
                                coln++;
                            }



                            dtrow[coln] = Convert.ToString(studentName).Trim();
                            coln++;

                            dtl.Rows.Add(dtrow);
                            //================================================//

                            sno++;
                        }
                        htSplHr.Clear();
                        string hrdetno = string.Empty;
                        string getsphr = "select distinct date,hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and degree_code=" + newBranchCode + " and batch_year=" + newBatchYear + " and semester=" + newsemester + " and date between '" + dtFromDate.ToString() + "' and '" + dtToDate.ToString() + "' " + splhrsec + "";
                        dsSplHr = d2.select_method(getsphr, hat, "Text");
                        if (dsSplHr.Tables.Count > 0 && dsSplHr.Tables[0].Rows.Count > 0)
                        {
                            for (int sphr = 0; sphr < dsSplHr.Tables[0].Rows.Count; sphr++)
                            {
                                if (htSplHr.Contains(Convert.ToString(dsSplHr.Tables[0].Rows[sphr]["date"]).Trim()))
                                {
                                    hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(dsSplHr.Tables[0].Rows[sphr]["date"]).Trim(), htSplHr));
                                    hrdetno = hrdetno + "," + Convert.ToString(dsSplHr.Tables[0].Rows[sphr]["hrdet_no"]).Trim();
                                    htSplHr[Convert.ToString(dsSplHr.Tables[0].Rows[sphr]["date"])] = hrdetno.Trim();
                                }
                                else
                                {
                                    htSplHr.Add(Convert.ToString(dsSplHr.Tables[0].Rows[sphr]["date"]).Trim(), Convert.ToString(dsSplHr.Tables[0].Rows[sphr]["hrdet_no"]).Trim());
                                }
                            }
                        }
                        if (subjectNoSelected.Length > 0)
                        {
                            string rstrsec = string.Empty;
                            if (ddlSec.Items.Count > 0)
                            {
                                if (Convert.ToString(ddlSec.SelectedItem.Text).Trim() == "-1" || Convert.ToString(ddlSec.SelectedItem.Text).Trim() == "")
                                {
                                    strsec = string.Empty;
                                    rstrsec = string.Empty;
                                }
                                else
                                {
                                    strsec = " and sections='" + Convert.ToString(ddlSec.SelectedItem.Text).Trim() + "'";
                                    rstrsec = " and r.sections='" + Convert.ToString(ddlSec.SelectedItem.Text).Trim() + "'";
                                    section_lab = " and l.sections='" + Convert.ToString(ddlSec.SelectedItem.Text).Trim() + "'";
                                }
                            }
                            DataSet dsAlterSchedule1 = new DataSet();
                            dsAlterSchedule1.Clear();
                            qry = "select  * from alternate_schedule where degree_code = '" + newBranchCode.Replace("'", "").Trim() + "' and semester = '" + newsemester + "' and batch_year = '" + newBatchYear + "' and FromDate between '" + dtFromDate + "' and '" + dtToDate + "' " + strsec + " order by FromDate Desc";
                            dsAlterSchedule1 = d2.select_method_wo_parameter(qry, "Text");
                            //---------------------------------------------
                            DataSet dsSemesterSchedule1 = new DataSet();
                            dsSemesterSchedule1.Clear();
                            qry = "select  * from semester_schedule where degree_code = '" + newBranchCode.Replace("'", "").Trim() + "' and semester = '" + newsemester + "' and batch_year = '" + newBatchYear + "'" + strsec + " order by FromDate Desc";
                            dsSemesterSchedule1 = d2.select_method_wo_parameter(qry, "Text");
                            string currlabsub = "select distinct s.subject_no,s.subject_name,s.subject_code,sy.Batch_Year,sy.degree_code,sy.semester from syllabus_master sy,sub_sem sm,subject s where sy.syll_code=sm.syll_code and sy.syll_code=s.syll_code and sm.syll_code=s.syll_code and sm.subType_no=s.subType_no and sm.Lab=1 and sy.Batch_Year='" + newBatchYear + "' and sy.degree_code='" + newBranchCode.Replace("'", "").Trim() + "' and sy.semester='" + newsemester + "' order by sy.Batch_Year,sy.degree_code,sy.semester";
                            DataSet dsCurrentLab = d2.select_method_wo_parameter(currlabsub, "Text");
                            string getdeteails = "select convert(nvarchar(15),s.start_date,101) as start_date,nodays,s.starting_dayorder from seminfo s,PeriodAttndSchedule p where s.degree_code=p.degree_code and p.semester=s.semester and s.semester='" + newsemester + "' and s.batch_year='" + newBatchYear + "'  and s.degree_code='" + newBranchCode.Replace("'", "").Trim() + "'";
                            getdeteails = getdeteails + " ; select * from tbl_consider_day_order where semester='" + newsemester + "' and batch_year='" + newBatchYear + "'  and degree_code='" + newBranchCode.Replace("'", "").Trim() + "'";
                            DataSet dsSem = d2.select_method_wo_parameter(getdeteails, "Text");
                            has.Clear();
                            has.Add("colege_code", collegeCode);
                            dsAttndanceMaster = d2.select_method("ATT_MASTER_SETTING", has, "sp");
                            grouporusercode = string.Empty;
                            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                            {
                                grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim() + "'";
                            }
                            else
                            {
                                grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                            }
                            string qryNew = "select rights from special_hr_rights where " + grouporusercode + "";
                            DataSet dsSplHrRights = new DataSet();
                            dsSplHrRights = d2.select_method_wo_parameter(qryNew, "Text");
                            //and a.month_year='" + month_year + "'
                            string qryTheryAlter = "select distinct s.subject_no,r.roll_no,a.*, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from registration r ,subjectchooser s,attendance a  where r.roll_no=a.roll_no and s.roll_no=a.roll_no  and r.degree_code='" + newBranchCode.Replace("'", "").Trim() + "' and batch_year='" + newBatchYear + "' and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=s.roll_no and s.semester= '" + newsemester + "' " + strsec + " and  subject_no in(" + subjectNo + ") " + strOrderBy + "";
                            DataSet dsTheoryAlter = new DataSet();
                            dsTheoryAlter = d2.select_method_wo_parameter(qryTheryAlter, "Text");
                            //and hour_value='" + temp_hr + "' and a.month_year='" + month_year + "'  and day_value='" + strDay + "'
                            string qryLabAlter = "select distinct s.subject_no,FromDate,hour_value,day_value,r.roll_no,a.*, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from  registration r,subjectchooser_New s,laballoc_new l,attendance a  where a.roll_no=s.roll_no and r.roll_no=a.roll_no  and r.degree_code='" + newBranchCode.Replace("'", "").Trim() + "' and r.batch_year='" + newBatchYear + "' and cc=0 and delflag=0 and exam_flag<>'debar'  " + rstrsec + " and r.roll_no=s.roll_no and s.subject_no in(" + subjectNo + ") and r.degree_code=l.degree_code and r.batch_year=l.batch_year and l.Semester=r.Current_Semester and s.subject_no =l.subject_no and s.Batch=l.Stu_Batch and l.subject_no in(" + subjectNo + ")  " + section_lab + " and FromDate between '" + dtFromDate + "' and '" + dtToDate + "' and l.fdate=s.fromdate " + strOrderBy + "";
                            DataSet dsPracticalAlter = new DataSet();
                            dsPracticalAlter = d2.select_method_wo_parameter(qryLabAlter, "Text");
                            //and a.month_year='" + month_year + "'
                            string qryTherySchedule = "select distinct s.subject_no,r.roll_no,a.*, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from registration r ,subjectchooser s,attendance a where r.roll_no=a.roll_no and a.roll_no=s.roll_no  and r.degree_code='" + newBranchCode.Replace("'", "").Trim() + "' and batch_year='" + newBatchYear + "' and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=s.roll_no and s.semester= '" + newsemester + "' " + strsec + " and  subject_no in(" + subjectNo + ") " + strOrderBy + "";
                            DataSet dsTheorySchedule = new DataSet();
                            dsTheorySchedule = d2.select_method_wo_parameter(qryTherySchedule, "Text");
                            //and hour_value='" + temp_hr + "'  and day_value='" + strDay + "' and a.month_year='" + month_year + "'
                            string qryLabSchedule = "select distinct s.subject_no,hour_value,day_value,r.roll_no,a.*, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from  registration r,subjectchooser s,laballoc l,attendance a  where r.roll_no=a.roll_no and a.roll_no=s.roll_no  and r.degree_code='" + newBranchCode.Replace("'", "").Trim() + "' and r.batch_year='" + newBatchYear + "' and cc=0 and delflag=0 and exam_flag<>'debar' and s.Batch=l.Stu_Batch  " + rstrsec + " and r.roll_no=s.roll_no and s.subject_no in(" + subjectNo + ")  and r.degree_code=l.degree_code and r.batch_year=l.batch_year and l.Semester=r.Current_Semester and s.subject_no =l.subject_no   and l.subject_no in(" + subjectNo + ")  " + section_lab + " " + strOrderBy + "";
                            DataSet dsPracticalSchedule = new DataSet();
                            dsPracticalSchedule = d2.select_method_wo_parameter(qryLabSchedule, "Text");
                            int subCount = 0;
                            int[] totalConductedHrs = new int[subjectNoSelected.Length];
                            Hashtable htTotalConductedHrs = new Hashtable();
                            Hashtable httotalStudPresentHrs = new Hashtable();
                            Hashtable httotalStudPercentage = new Hashtable();
                            int len = subjectNoSelected.Length;
                            subcode =new string[len];
                            int ss = 0;
                            foreach (string subjects in subjectNoSelected)
                            {
                                //subjects = subjects.Trim();
                                int[] totalStudConductedHrs = new int[subjectNoSelected.Length];
                                int[] totalStudPresentHrs = new int[subjectNoSelected.Length];
                                double[] totalStudPercentage = new double[subjectNoSelected.Length];
                                CalculateAttendance(dsTheoryAlter, dsPracticalAlter, dsTheorySchedule, dsPracticalSchedule, strOrderBy, dsAttndanceMaster, dsSplHrRights, dsSem, dsSemesterSchedule1, dsAlterSchedule1, dsCurrentLab, dtFromDate, dtToDate, subjects, ref has_load_rollno, ref has_total_attnd_hour, ref has_od);
                                string subjectNames = subjectName[subCount];
                                string subjectAcronymn = d2.GetFunctionv("select acronym from subject where subject_no='" + subjects.Trim() + "'");
                                subCount++;
                                max_tot = 0;
                                attnd_hr = 0;
                                tot_hr = 0;
                                
                                
                                


                            
                                subcode[ss] = subjectAcronymn;
                                ss++;
                        
                                
                                
                                ConHrs = new System.Text.StringBuilder("Con. Hrs");
                                AddTableColumn(dtl, ConHrs);
                                dtl.Rows[0][dtl.Columns.Count - 1] = subjectAcronymn;
                                dtl.Rows[1][dtl.Columns.Count - 1] = "Con. Hrs";
                                

                                Present = new System.Text.StringBuilder("Present");
                                AddTableColumn(dtl, Present);
                                dtl.Rows[1][dtl.Columns.Count - 1] = "Present";

                                OD = new System.Text.StringBuilder("OD");
                                AddTableColumn(dtl, OD);
                                dtl.Rows[1][dtl.Columns.Count - 1] = "OD";

                                TotHrs = new System.Text.StringBuilder("Tot.Hrs");
                                AddTableColumn(dtl, TotHrs);
                                dtl.Rows[1][dtl.Columns.Count - 1] = "Tot.Hrs";

                                Absent = new System.Text.StringBuilder("Absent");
                                AddTableColumn(dtl, Absent);
                                dtl.Rows[1][dtl.Columns.Count - 1] = "Absent";

                                per = new System.Text.StringBuilder("  \t\t%\t\t  ");
                                AddTableColumn(dtl, per);
                                dtl.Rows[1][dtl.Columns.Count - 1] = "  \t\t%\t\t  ";
                                


                                sno = 0;
                                int countInvisible = 0;
                                for (int row_cnt = 2; row_cnt < dtl.Rows.Count; row_cnt++)
                                {
                                    bool check_flag = false;
                                    attnd_hr = 0;
                                    roll_no = Convert.ToString(dtl.Rows[row_cnt]["Roll No"].ToString()).Trim();
                                    if (has_load_rollno.Contains(roll_no.Trim().ToLower()))
                                    {
                                        attnd_hr = Convert.ToDouble(GetCorrespondingKey(roll_no.Trim().ToLower(), has_load_rollno));
                                        
                                            sno++;
                                            dtl.Rows[row_cnt][0] = sno.ToString();
                                            
                                        double hrs = 0;
                                        if (!httotalStudPresentHrs.Contains(roll_no.Trim().ToLower()))
                                        {
                                            httotalStudPresentHrs.Add(roll_no.Trim().ToLower(), attnd_hr);
                                        }
                                        else
                                        {
                                            hrs = Convert.ToDouble(GetCorrespondingKey(roll_no.Trim().ToLower(), httotalStudPresentHrs));
                                            hrs += attnd_hr;
                                            httotalStudPresentHrs[roll_no.Trim().ToLower()] = hrs;
                                        }
                                        

                                        dtl.Rows[row_cnt][dtl.Columns.Count - 5] = attnd_hr.ToString();
                                    }
                                    tot_hr = 0;
                                    if (has_total_attnd_hour.Contains(roll_no.Trim().ToLower()))
                                    {
                                        tot_hr = Convert.ToDouble(GetCorrespondingKey(roll_no.Trim().ToLower(), has_total_attnd_hour));
                                        if (row_cnt == 2)
                                        {
                                            if (max_tot < tot_hr)
                                            {
                                                max_tot = tot_hr;
                                            }
                                            Session["max_tot_hour"] = max_tot.ToString();
                                        }
                                    }
                                    od_count = 0;
                                    if (has_od.Contains(roll_no.Trim().ToLower()))
                                    {
                                        od_count = Convert.ToInt16(GetCorrespondingKey(roll_no.Trim().ToLower(), has_od));
                                        check_flag = true;
                                       

                                        dtl.Rows[row_cnt][dtl.Columns.Count - 4] = od_count.ToString();
                                    }
                                    if (check_flag == false)
                                    {
                                        
                                        dtl.Rows[row_cnt][dtl.Columns.Count - 4] = "0";
                                    }
                                    if (attnd_hr == 0 && od_count == 0)
                                    {
                                        
                                        dtl.Rows[row_cnt][dtl.Columns.Count - 3] = "-";
                                    }
                                    else
                                    {
                                        
                                        dtl.Rows[row_cnt][dtl.Columns.Count - 3] = (attnd_hr + od_count).ToString();
                                    }
                                    if (attnd_hr == 0 && tot_hr == 0)
                                    {
                                        

                                        dtl.Rows[row_cnt][dtl.Columns.Count - 2] = "-";
                                    }
                                    else
                                    {
                                        

                                        dtl.Rows[row_cnt][dtl.Columns.Count - 2] = (tot_hr - (attnd_hr + od_count)).ToString(); 
                                    }
                                    
                                    dtl.Rows[row_cnt][dtl.Columns.Count - 6] = tot_hr.ToString(); 
                                    double hrsCon = 0;
                                    if (!htTotalConductedHrs.Contains(roll_no.Trim().ToLower()))
                                    {
                                        htTotalConductedHrs.Add(roll_no.Trim().ToLower(), tot_hr);
                                    }
                                    else
                                    {
                                        hrsCon = Convert.ToDouble(GetCorrespondingKey(roll_no.ToLower().Trim(), htTotalConductedHrs));
                                        hrsCon += tot_hr;
                                        //htTotalConductedHrs.Add(roll_no.Trim(), hrsCon);
                                        htTotalConductedHrs[roll_no.Trim().ToLower()] = hrsCon;
                                    }
                                    
                                    

                                    if (dtl.Rows[row_cnt][dtl.Columns.Count - 3].ToString() == "0" && tot_hr == 0)
                                    {
                                        dtl.Rows[row_cnt][dtl.Columns.Count - 1] = "-";
                                        
                                    }
                                    else
                                    {
                                        double avg_val = 0, avgstudent3 = 0;
                                        decimal avgstudent1 = 0, avgstudent2 = 0;
                                        double attnd_perc = 0;
                                        avg_val = (((attnd_hr + od_count) / tot_hr) * 100);
                                        if (avg_val.ToString() != "NaN")
                                        {
                                            //avgstudent1 = Convert.ToDecimal(avg_val);
                                            //avgstudent2 = Math.Round(avgstudent1);
                                            //avgstudent3 = Convert.ToDouble(avgstudent2);
                                            //attnd_perc = Convert.ToString(avgstudent3);
                                            attnd_perc = Math.Round(avg_val, 2);
                                        }
                                        else
                                        {
                                            attnd_perc = 0;
                                        }
                                        dtl.Rows[row_cnt][dtl.Columns.Count - 1] = attnd_perc.ToString();
                                       
                                        double hrsPer = 0;
                                        if (!httotalStudPercentage.Contains(roll_no.Trim().ToLower()))
                                        {
                                            httotalStudPercentage.Add(roll_no.Trim().ToLower(), attnd_perc);
                                        }
                                        else
                                        {
                                            hrsPer = Convert.ToDouble(GetCorrespondingKey(roll_no.Trim().ToLower(), httotalStudPercentage));
                                            hrsPer += attnd_perc;
                                            httotalStudPercentage[roll_no.Trim().ToLower()] = hrsPer;
                                        }
                                        
                                        //if (attMinShorage < attnd_perc)
                                        //{
                                        //    countInvisible++;
                                        //    FpAttendanceReport.Sheets[0].Rows[row_cnt].Visible = false;
                                        //    //.Cells[row_cnt, (FpAttendanceReport.Sheets[0].ColumnCount - 1)]
                                        //}
                                    }
                                }
                                bool check_row_visible = false;
                                int rowValue = 0;
                                for (int row_visible = 2; row_visible < dtl.Rows.Count; row_visible++)
                                {
                                    
                                        rowValue++;
                                        check_row_visible = true;
                                        
                                        dtl.Rows[row_visible][0] = rowValue.ToString();
                                    
                                }
                                if (!check_row_visible || countInvisible == dtl.Rows.Count)
                                {
                                    lblErrmsg.Text = string.Empty;
                                    lblErrmsg.Visible = false;
                                    rptprint1.Visible = false;
                                    divAttendanceReport.Visible = false;
                                    lblPopupAlert.Text = "No Record(s) Found";
                                    lblPopupAlert.Visible = true;
                                    divPopupAlert.Visible = true;
                                    return;
                                }
                            }
                            for (int roll = 2; roll < dtl.Rows.Count; roll++)
                            {
                                if (roll == 2)
                                {
                                    
                                   

                                    dtl.Columns.Add("Tot Con", typeof(string));
                                    dtl.Rows[0][dtl.Columns.Count - 1] = "Tot Con";


                                    dtl.Columns.Add("Tot. Present", typeof(string));
                                    dtl.Rows[0][dtl.Columns.Count - 1] = "Tot. Present";


                                    dtl.Columns.Add("\t\t%\t\t", typeof(string));
                                    dtl.Rows[0][dtl.Columns.Count - 1] = "\t\t%\t\t";


                                    dtl.Columns.Add("Remarks", typeof(string));
                                    dtl.Rows[0][dtl.Columns.Count - 1] = "Remarks";
                                }
                                
                                roll_no = dtl.Rows[roll]["Roll No"].ToString().Trim();
                                double totConducted = 0;
                                double overallPresent = 0;
                                double overallPercentage = 0;
                                if (htTotalConductedHrs.Contains(roll_no.Trim().ToLower()))
                                {
                                    totConducted = Convert.ToDouble(GetCorrespondingKey(roll_no.Trim().ToLower(), htTotalConductedHrs));
                                    

                                    dtl.Rows[roll][dtl.Columns.Count - 4] = Convert.ToString(totConducted);
                                }
                                else
                                {
                                    
                                    dtl.Rows[roll][dtl.Columns.Count - 4] = Convert.ToString("--");
                                }
                                
                                if (httotalStudPresentHrs.Contains(roll_no.Trim().ToLower()))
                                {
                                    overallPresent = Convert.ToDouble(GetCorrespondingKey(roll_no.Trim().ToLower(), httotalStudPresentHrs));
                                    
                                    dtl.Rows[roll][dtl.Columns.Count - 3] = Convert.ToString(overallPresent);
                                }
                                else
                                {
                                    
                                    dtl.Rows[roll][dtl.Columns.Count - 3] = Convert.ToString("--");
                                }
                                
                                double avg = 0;
                                if (overallPresent > 0 && totConducted > 0)
                                {
                                    avg = (overallPresent / totConducted) * 100;
                                    avg = Math.Round(avg, 2, MidpointRounding.AwayFromZero);
                                }
                                

                                dtl.Rows[roll][dtl.Columns.Count - 2] = Convert.ToString(avg);
                            }
                        }

                        if (dtl.Rows.Count > 0)
                        {
                            Showgrid.DataSource = dtl;
                            Showgrid.DataBind();
                            Showgrid.Visible = true;
                            Showgrid.HeaderRow.Visible = false;
                            int tempt = Convert.ToInt32(ViewState["temp_table"]);
                            int ccc = tempt;

                            for (int i = 0; i < Showgrid.Rows.Count; i++)
                            {

                                for (int j = 0; j < Showgrid.HeaderRow.Cells.Count; j++)
                                {

                                    if (i == 0 || i == 1)
                                    {
                                        Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                        Showgrid.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                        Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                        Showgrid.Rows[i].Cells[j].Font.Bold = true;

                                        if (i == 0)
                                        {
                                            if (j < tempt || j >= Showgrid.HeaderRow.Cells.Count - 4)
                                            {
                                                Showgrid.Rows[i].Cells[j].RowSpan = 2;
                                                for (int a = i; a < 1; a++)
                                                    Showgrid.Rows[a + 1].Cells[j].Visible = false;
                                            }
                                            else if (ccc == j)
                                            {
                                                Showgrid.Rows[i].Cells[j].ColumnSpan = 6;
                                                for (int a = j + 1; a < j + 6; a++)
                                                    Showgrid.Rows[i].Cells[a].Visible = false;

                                                ccc += 6;
                                            }

                                        }
                                    }
                                    else
                                    {


                                        if (Showgrid.HeaderRow.Cells[j].Text == "Admission No" || Showgrid.HeaderRow.Cells[j].Text == "Roll No" || Showgrid.HeaderRow.Cells[j].Text == "Reg No" || Showgrid.HeaderRow.Cells[j].Text == "Student Type" || Showgrid.HeaderRow.Cells[j].Text == "Student Name")
                                        {
                                            Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;


                                        }

                                        else
                                        {
                                            Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;




                                        }


                                    }
                                }

                            }
                        }

                        
                        divAttendanceReport.Visible = true;
                        rptprint1.Visible = true;
                    }
                    else
                    {
                        lblErrmsg.Text = string.Empty;
                        lblErrmsg.Visible = false;
                        rptprint1.Visible = false;
                        divAttendanceReport.Visible = false;
                        lblPopupAlert.Text = "Update Master Setting";
                        lblPopupAlert.Visible = true;
                        divPopupAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblErrmsg.Text = string.Empty;
                    lblErrmsg.Visible = false;
                    rptprint1.Visible = false;
                    divAttendanceReport.Visible = false;
                    lblPopupAlert.Text = "No Record(s) were Found";
                    lblPopupAlert.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void CalculateAttendance(DataSet dsTheoryAlter, DataSet dsPracticalAlter, DataSet dsTheorySchedule, DataSet dsPracticalSchedule, string strOrderBy, DataSet dsAttndanceMaster, DataSet dsSplHrRights1, DataSet dsSem, DataSet dsSemesterSchedule1, DataSet dsAlterSchedule1, DataSet dsCurrentLab, DateTime dtFromDate, DateTime dtToDate, string subjectNo, ref Hashtable has_load_rollno, ref Hashtable has_total_attnd_hour, ref Hashtable has_od)
    {
        try
        {
            HashValueToZero(has_load_rollno);
            HashValueToZero(has_total_attnd_hour);
            HashValueToZero(has_od);
            Dictionary<DateTime, byte> dicAlternateDayOrder = new Dictionary<DateTime, byte>();//magesh 3.9.18
            //string orderBySetting = d2.GetFunction("select value from master_Settings where settings='order_by'");
            //string strOrderBy = " ORDER BY r.roll_no";
            //string serialno = d2.GetFunction("select LinkValue from inssettings where college_code='" + collegeCode + "' and linkname='Student Attendance'");
            //if (serialno == "1")
            //{
            //    strOrderBy = "ORDER BY r.serialno";
            //}
            //else
            //{
            //    if (orderBySetting == "0")
            //    {
            //        strOrderBy = "ORDER BY r.roll_no";
            //    }
            //    else if (orderBySetting == "1")
            //    {
            //        strOrderBy = "ORDER BY r.Reg_No";
            //    }
            //    else if (orderBySetting == "2")
            //    {
            //        strOrderBy = "ORDER BY r.Stud_Name";
            //    }
            //    else if (orderBySetting == "0,1,2")
            //    {
            //        strOrderBy = "ORDER BY r.roll_no,r.Reg_No,r.Stud_Name";
            //    }
            //    else if (orderBySetting == "0,1")
            //    {
            //        strOrderBy = "ORDER BY r.roll_no,r.Reg_No";
            //    }
            //    else if (orderBySetting == "1,2")
            //    {
            //        strOrderBy = "ORDER BY r.Reg_No,r.Stud_Name";
            //    }
            //    else if (orderBySetting == "0,2")
            //    {
            //        strOrderBy = "ORDER BY r.roll_no,r.Stud_Name";
            //    }
            //}
            string temp_tag = "", rstrsec = string.Empty;
            dtTempDate = dtFromDate;
            string subject_no = subjectNo;
            if (ddlSec.Items.Count > 0)
            {
                if (Convert.ToString(ddlSec.SelectedItem.Text).Trim() == "-1" || Convert.ToString(ddlSec.SelectedItem.Text).Trim() == "")
                {
                    strsec = string.Empty;
                    rstrsec = string.Empty;
                }
                else
                {
                    strsec = " and sections='" + Convert.ToString(ddlSec.SelectedItem.Text).Trim() + "'";
                    rstrsec = " and r.sections='" + Convert.ToString(ddlSec.SelectedItem.Text).Trim() + "'";
                    section_lab = " and l.sections='" + Convert.ToString(ddlSec.SelectedItem.Text).Trim() + "'";
                }
            }
            Hashtable hatlab = new Hashtable();
            if (dsCurrentLab.Tables.Count > 0 && dsCurrentLab.Tables[0].Rows.Count > 0)
            {
                for (int l = 0; l < dsCurrentLab.Tables[0].Rows.Count; l++)
                {
                    string strSubNo = Convert.ToString(dsCurrentLab.Tables[0].Rows[l]["subject_no"]).Trim();
                    if (!hatlab.Contains(strSubNo))
                    {
                        hatlab.Add(strSubNo, strSubNo);
                    }
                }
            }
            string semstartdate = string.Empty;
            string noofdays = string.Empty;
            string startday = string.Empty;
            if (dsSem.Tables.Count > 0)
            {
                if (dsSem.Tables.Count > 0 && dsSem.Tables[0].Rows.Count > 0)
                {
                    semstartdate = Convert.ToString(dsSem.Tables[0].Rows[0]["start_date"]).Trim();
                    noofdays = Convert.ToString(dsSem.Tables[0].Rows[0]["nodays"]).Trim();
                    startday = Convert.ToString(dsSem.Tables[0].Rows[0]["starting_dayorder"]).Trim();
                }
                try
                {
                    if (dsSem.Tables.Count > 1 && dsSem.Tables[1].Rows.Count > 0)
                    {
                        for (int dc = 0; dc < dsSem.Tables[1].Rows.Count; dc++)
                        {
                            DateTime dtdcf = Convert.ToDateTime(Convert.ToString(dsSem.Tables[1].Rows[dc]["from_date"]).Trim());
                            DateTime dtdct = Convert.ToDateTime(Convert.ToString(dsSem.Tables[1].Rows[dc]["to_date"]).Trim());
                            string asperday = Convert.ToString(dsSem.Tables[1].Rows[dc]["include_attendance"].ToString());
                            string alternateDayOrder = Convert.ToString(dsSem.Tables[1].Rows[dc]["DayOrder"]).Trim();
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
            }
            if (chkflag == false)
            {
                chkflag = true;
                int count_master = 0;
                if (dsAttndanceMaster.Tables.Count > 0)
                {
                    count_master = (dsAttndanceMaster.Tables[0].Rows.Count);
                    if (count_master > 0)
                    {
                        for (count_master = 0; count_master < dsAttndanceMaster.Tables[0].Rows.Count; count_master++)
                        {
                            if (Convert.ToString(dsAttndanceMaster.Tables[0].Rows[count_master]["calcflag"]).Trim() == "0")
                            {
                                has_attnd_masterset.Add(Convert.ToString(dsAttndanceMaster.Tables[0].Rows[count_master]["leavecode"]).Trim(), Convert.ToString(dsAttndanceMaster.Tables[0].Rows[count_master]["leavecode"]).Trim());
                            }
                            if (Convert.ToString(dsAttndanceMaster.Tables[0].Rows[count_master]["calcflag"]).Trim() == "2")
                            {
                                if (!has_attnd_masterset_notconsider.ContainsKey(Convert.ToString(dsAttndanceMaster.Tables[0].Rows[count_master]["leavecode"]).Trim()))
                                {
                                    has_attnd_masterset_notconsider.Add(Convert.ToString(dsAttndanceMaster.Tables[0].Rows[count_master]["leavecode"]).Trim(), Convert.ToString(dsAttndanceMaster.Tables[0].Rows[count_master]["leavecode"]).Trim());
                                }
                            }
                        }
                    }
                }
                //string grouporusercode =string.Empty;
                //if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                //{
                //    grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
                //}
                //else
                //{
                //    grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                //}
                //string qryNew = "select rights from  special_hr_rights where " + grouporusercode + "";
                DataSet dsSplHrRights = new DataSet();
                dsSplHrRights = dsSplHrRights1;
                //dsSplHrRights = d2.select_method_wo_parameter(qryNew, "Text");
                if (dsSplHrRights.Tables.Count > 0 && dsSplHrRights.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr_rights_spl_hr in dsSplHrRights.Tables[0].Rows)
                    {
                        string spl_hr_rights = string.Empty;
                        Hashtable od_has = new Hashtable();
                        spl_hr_rights = Convert.ToString(dr_rights_spl_hr["rights"]).Trim();
                        if (spl_hr_rights.Trim().ToLower() == "true" || spl_hr_rights.Trim().ToLower() == "1")
                        {
                            splhr_flag = true;
                        }
                    }
                }
            }
            while (dtTempDate <= dtToDate)
            {
                if (!hatdc.Contains(dtTempDate))
                {
                    if (splhr_flag == true)
                    {
                        if (htSplHr.Contains(Convert.ToString(dtTempDate).Trim()))
                        {
                            getspecial_hr(htSplHr);
                        }
                    }
                    span_count = 0;
                    if (!hat_holy.ContainsKey(dtTempDate))
                    {
                        if (!hat_holy.ContainsKey(dtTempDate))
                        {
                            hat_holy.Add(dtTempDate, "3*0*0");
                        }
                    }
                    value_holi_status = GetCorrespondingKey(dtTempDate, hat_holy).ToString();
                    split_holiday_status = value_holi_status.Split('*');
                    if (split_holiday_status[0].ToString() == "3")//=========ful day working day
                    {
                        split_holiday_status_1 = 1;
                        split_holiday_status_2 = noOfHours;
                    }
                    else if (split_holiday_status[0].ToString() == "1")//=============half day working day
                    {
                        if (split_holiday_status[1].ToString() == "1")//==============mng holiday//evng working day
                        {
                            split_holiday_status_1 = firstHalf + 1;
                            split_holiday_status_2 = noOfHours;
                        }
                        if (split_holiday_status[2].ToString() == "1")//==============evng holiday//mng working day
                        {
                            split_holiday_status_1 = 1;
                            split_holiday_status_2 = firstHalf;
                        }
                    }
                    else if (split_holiday_status[0].ToString() == "0")//=================fulday holiday
                    {
                        split_holiday_status_1 = 0;
                        split_holiday_status_2 = 0;
                    }
                    //------------------------------
                    if (split_holiday_status_1 == 0 && split_holiday_status_2 == 0)
                    {
                        //  temp_date = temp_date.AddDays(1);//Hidden by srinath 11/9/2014
                    }
                    else
                    {
                        //---------------alternate schedule
                        if (dsSemesterSchedule1.Tables.Count > 0 && dsSemesterSchedule1.Tables[0].Rows.Count > 0)
                        {
                            dsSemesterSchedule1.Tables[0].DefaultView.RowFilter = "FromDate <='" + dtTempDate + "'";
                            DataView dv = dsSemesterSchedule1.Tables[0].DefaultView;
                            dsSemesterSchedule.Clear();
                            dsSemesterSchedule = new DataSet();
                            dsSemesterSchedule.Tables.Add(dv.ToTable());
                        }
                        if (dsAlterSchedule1.Tables.Count > 0 && dsAlterSchedule1.Tables[0].Rows.Count > 0)
                        {
                            dsAlterSchedule1.Tables[0].DefaultView.RowFilter = "FromDate ='" + dtTempDate + "'";
                            DataView dv1 = dsAlterSchedule1.Tables[0].DefaultView;
                            dsAlterSchedule.Clear();
                            dsAlterSchedule = new DataSet();
                            dsAlterSchedule.Tables.Add(dv1.ToTable());
                        }
                        if (dsSemesterSchedule.Tables.Count > 0 && dsSemesterSchedule.Tables[0].Rows.Count > 0)
                        {
                            if (noOfHours > 0)
                            {
                                dummy_date = dtTempDate.ToString();
                                string[] dummy_date_split = dummy_date.Split(' ');
                                string[] final_date_string = dummy_date_split[0].Split('/');
                                dummy_date = final_date_string[1].ToString() + "/" + final_date_string[0].ToString() + "/" + final_date_string[2].ToString();
                                month_year = ((Convert.ToInt16(final_date_string[2].ToString()) * 12) + (Convert.ToInt16(final_date_string[0].ToString()))).ToString();
                                if (order != "0")
                                {
                                    strDay = dtTempDate.ToString("ddd");
                                }
                                else
                                {
                                    string[] sp = dummy_date.Split('/');
                                    string curdate = sp[1] + '/' + sp[0] + '/' + sp[2];
                                    strDay = d2.findday(curdate, newBranchCode, newsemester, newBatchYear, semstartdate, noofdays, startday);
                                    //magesh 3.9.18
                                    if (dicAlternateDayOrder.ContainsKey(dtTempDate))
                                    {
                                        strDay =d2.findDayName(dicAlternateDayOrder[dtTempDate]);
                                        string Day_Order = Convert.ToString(dicAlternateDayOrder[dtTempDate]).Trim();
                                    } //magesh 3.9.18
                                }
                                for (int temp_hr = split_holiday_status_1; temp_hr <= split_holiday_status_2; temp_hr++)
                                {
                                    bool samehr_flag = false;
                                    roll_count = 0;
                                    present_count = 0;
                                    temp_hr_field = strDay + temp_hr;
                                    date_temp_field = "d" + final_date_string[1].ToString() + "d" + temp_hr;
                                    hatattendance.Clear();
                                    if (dsAlterSchedule.Tables.Count > 0 && dsAlterSchedule.Tables[0].Rows.Count > 0)
                                    {
                                        for (int hasrow = 0; hasrow < dsAlterSchedule.Tables[0].Rows.Count; hasrow++)
                                        {
                                            full_hour = dsAlterSchedule.Tables[0].Rows[hasrow][temp_hr_field].ToString();
                                            if (full_hour.Trim() != "")
                                            {
                                                temp_has_subj_code.Clear();
                                                string[] split_full_hour = full_hour.Split(';');
                                                bool batchflag = false;
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
                                                for (int semi_colon = 0; semi_colon <= split_full_hour.GetUpperBound(0); semi_colon++)
                                                {
                                                    roll_count = 0;
                                                    single_hour = split_full_hour[semi_colon].ToString();
                                                    string[] split_single_hour = single_hour.Split('-');
                                                    if (split_single_hour.GetUpperBound(0) >= 1)
                                                    {
                                                        check_alter = true;
                                                        if (split_single_hour[0].ToString().Trim() == subject_no.Trim())
                                                        {
                                                            if (!temp_has_subj_code.ContainsKey(subject_no.Trim()))
                                                            {
                                                                temp_has_subj_code.Add(subject_no.Trim(), subject_no.Trim());
                                                                //----------------------check lab allocation
                                                                recflag = true;
                                                                roll_count = 0;
                                                                if (samehr_flag == false)
                                                                {
                                                                    span_count++;
                                                                    samehr_flag = true;
                                                                }
                                                                //------------------------attendance
                                                                Hashtable has_stud_list = new Hashtable();
                                                                //------------------find subject type
                                                                if (batchflag == false)
                                                                {
                                                                    subj_type = "0";
                                                                }
                                                                else
                                                                {
                                                                    subj_type = "1";
                                                                }
                                                                //====================
                                                                if (subj_type.Trim().ToLower() != "1" && subj_type.Trim() != "True" && subj_type.Trim().ToUpper() != "TRUE" && subj_type.Trim().ToLower() != "true")
                                                                {
                                                                    //===Raja 1
                                                                    string strquery = "select distinct r.roll_no,a." + date_temp_field + " as attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from registration r ,subjectchooser s,attendance a  where r.roll_no=a.roll_no and s.roll_no=a.roll_no and a.month_year='" + month_year + "' and r.degree_code='" + newBranchCode.Replace("'", "").Trim() + "' and batch_year='" + newBatchYear + "' and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=s.roll_no and s.semester= '" + newsemester + "' " + strsec + " and  subject_no='" + subject_no + "' " + strOrderBy + "";
                                                                    DataSet dsquery = new DataSet();
                                                                    if (dsTheoryAlter.Tables.Count > 0 && dsTheoryAlter.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        dsTheoryAlter.Tables[0].DefaultView.RowFilter = "month_year='" + month_year + "' and  subject_no='" + subject_no + "'";
                                                                        DataTable dtTemp = new DataTable();
                                                                        dtTemp = dsTheoryAlter.Tables[0].DefaultView.ToTable();
                                                                        dsquery.Tables.Add(dtTemp);
                                                                    }
                                                                    //dsquery = d2.select_method(strquery, hat, "Text");
                                                                    if (dsquery.Tables.Count > 0 && dsquery.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                        {
                                                                            string rollno = Convert.ToString(dsquery.Tables[0].Rows[i]["roll_no"]).Trim();
                                                                            if (!hatattendance.Contains(rollno.Trim().ToLower()))
                                                                            {
                                                                                //hatattendance.Add(rollno.Trim(), Convert.ToString(dsquery.Tables[0].Rows[i]["attvalue"]).Trim());
                                                                                hatattendance.Add(rollno.Trim().ToLower(), Convert.ToString(dsquery.Tables[0].Rows[i][date_temp_field]).Trim());
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    string strquery = "select distinct  r.roll_no,a." + date_temp_field + " as attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from  registration r,subjectchooser_New s,laballoc_new l,attendance a  where a.roll_no=s.roll_no and r.roll_no=a.roll_no and a.month_year='" + month_year + "' and r.degree_code='" + newBranchCode.Replace("'", "").Trim() + "' and r.batch_year='" + newBatchYear + "' and cc=0 and delflag=0 and exam_flag<>'debar'  " + rstrsec + " and r.roll_no=s.roll_no and s.subject_no='" + subject_no + "' and r.degree_code=l.degree_code and r.batch_year=l.batch_year and l.Semester=r.Current_Semester and s.subject_no =l.subject_no and hour_value='" + temp_hr + "' and day_value='" + strDay + "' and l.subject_no=" + subject_no + "  " + section_lab + " and FromDate ='" + dtTempDate + "' and l.fdate=s.fromdate " + strOrderBy + "";
                                                                    //strquery = "select distinct registration.roll_no,registration.app_no,registration.reg_no,registration.roll_admit ,registration.stud_name,registration.stud_type,registration.serialno,registration.degree_code,Registration.college_code from subjectchooser_New,sub_sem,subject,registration where fromdate='" + getdate + "' and  todate='" + getdate + "' and batch in(select   distinct stu_batch from laballoc_new where subject_no='" + subject_no + "'  and batch_year=" + byear + "  and hour_value=" + hr + "   " + strsec + "  and degree_code=" + degree_code + " and fdate='" + getdate + "' and  tdate='" + getdate + "' and day_value='" + Day_Var + "' ) and subjectchooser_New.subtype_no=sub_sem.subtype_no and  semester =  " + semester.ToString() + " and subjectchooser_New.subject_no=subject.subject_no and  registration.roll_no=subjectchooser_New.roll_no and  registration.current_semester=subjectchooser_New.semester and subjectchooser_New.subject_no='" + subject_no + "'   and adm_date<='" + sel_date + "'  and SubjectChooser_new.Semester=registration.current_semester " + strsec + " and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR'";
                                                                    //and s.batch=l.stu_batch
                                                                    //DataSet dsquery = d2.select_method(strquery, hat, "Text");
                                                                    DataSet dsquery = new DataSet();
                                                                    if (dsPracticalAlter.Tables.Count > 0 && dsPracticalAlter.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        dsPracticalAlter.Tables[0].DefaultView.RowFilter = "month_year='" + month_year + "' and  subject_no='" + subject_no + "' and FromDate ='" + dtTempDate + "' and hour_value='" + temp_hr + "'  and day_value='" + strDay + "'";
                                                                        DataTable dtTemp = new DataTable();
                                                                        dtTemp = dsPracticalAlter.Tables[0].DefaultView.ToTable();
                                                                        dsquery.Tables.Add(dtTemp);
                                                                    }
                                                                    //dsquery = d2.select_method(strquery, hat, "Text");
                                                                    if (dsquery.Tables.Count > 0 && dsquery.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                        {
                                                                            string rollno = Convert.ToString(dsquery.Tables[0].Rows[i]["roll_no"]).Trim();
                                                                            if (!hatattendance.Contains(rollno.Trim().ToLower()))
                                                                            {
                                                                                //hatattendance.Add(rollno.Trim(), Convert.ToString(dsquery.Tables[0].Rows[i]["attvalue"]).Trim());
                                                                                hatattendance.Add(rollno.Trim().ToLower(), Convert.ToString(dsquery.Tables[0].Rows[i][date_temp_field]).Trim());
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (hatattendance.Count > 0)
                                                                {
                                                                    for (int i = 0; i < dtl.Rows.Count-2; i++)
                                                                    {
                                                                        string rollno = Convert.ToString(dtl.Rows[i+2]["Roll No"].ToString()).Trim();
                                                                        if (hatattendance.Contains(rollno.Trim().ToLower()))
                                                                        {
                                                                            Admission_date = Convert.ToDateTime(admissiondate[i].Trim());
                                                                            string attvalue = Convert.ToString(GetCorrespondingKey(rollno.Trim().ToLower(), hatattendance)).Trim();
                                                                            string value = Attmark(attvalue.Trim());
                                                                            if (dtTempDate >= Admission_date)
                                                                            {
                                                                               
                                                                                if (attvalue == "3")
                                                                                {
                                                                                    temp_tag = "3";
                                                                                }
                                                                                else
                                                                                {
                                                                                    temp_tag = "0";
                                                                                }
                                                                                if ((attvalue.ToString()) != "8" && (attvalue.ToString()) != "7" )  //modified by prabha on 20 dec 2017 --  && (attvalue.ToString()) != "8" has been added
                                                                                {
                                                                                    if (value != "HS")
                                                                                    {
                                                                                        if (!has_attnd_masterset_notconsider.ContainsKey(attvalue.ToString().Trim()))
                                                                                        {
                                                                                            if (temp_tag == "0")
                                                                                            {
                                                                                                if (has_attnd_masterset.ContainsKey(attvalue.Trim()))
                                                                                                {
                                                                                                    present_count = Convert.ToInt16(GetCorrespondingKey(rollno.Trim().ToLower(), has_load_rollno));
                                                                                                    present_count++;
                                                                                                    has_load_rollno[rollno.ToLower().Trim()] = present_count;
                                                                                                }
                                                                                                if (value != "NE")
                                                                                                {
                                                                                                    present_count = Convert.ToInt16(GetCorrespondingKey(rollno.Trim().ToLower(), has_total_attnd_hour));
                                                                                                    present_count++;
                                                                                                    has_total_attnd_hour[rollno.Trim().ToLower()] = present_count;
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                if (!has_od.ContainsKey(dtl.Rows[i+2]["Roll No"].ToString().ToLower().Trim()))
                                                                                                {
                                                                                                    has_od.Add(rollno.Trim().ToLower(), 1);
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    od_count = Convert.ToInt16(GetCorrespondingKey(rollno.Trim().ToLower(), has_od));
                                                                                                    od_count++;
                                                                                                    has_od[rollno.Trim().ToLower()] = od_count;
                                                                                                }
                                                                                                if (value != "NE")
                                                                                                {
                                                                                                    {
                                                                                                        present_count = Convert.ToInt16(GetCorrespondingKey(rollno.Trim().ToLower(), has_total_attnd_hour));
                                                                                                        present_count++;
                                                                                                        has_total_attnd_hour[rollno.Trim().ToLower()] = present_count;
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
                                        full_hour = Convert.ToString(dsSemesterSchedule.Tables[0].Rows[0][temp_hr_field]).Trim();
                                        if (full_hour.Trim() != "")
                                        {
                                            temp_has_subj_code.Clear();
                                            string[] split_full_hour_sem = full_hour.Split(';');
                                            bool batchflag = false;
                                            for (int g = 0; g <= split_full_hour_sem.GetUpperBound(0); g++)
                                            {
                                                string[] valhr = split_full_hour_sem[g].ToString().Split('-');
                                                if (valhr.GetUpperBound(0) > 1)
                                                {
                                                    string lsub = Convert.ToString(valhr[0]).Trim();
                                                    if (hatlab.Contains(lsub))
                                                    {
                                                        batchflag = true;
                                                    }
                                                }
                                            }
                                            for (int semi_colon = 0; semi_colon <= split_full_hour_sem.GetUpperBound(0); semi_colon++)
                                            {
                                                roll_count = 0;
                                                single_hour = Convert.ToString(split_full_hour_sem[semi_colon]).Trim();
                                                string[] split_single_hour = single_hour.Split('-');
                                                if (split_single_hour.GetUpperBound(0) >= 1)
                                                {
                                                    if (Convert.ToString(split_single_hour[0]).Trim() == subject_no.Trim())
                                                    {
                                                        if (!temp_has_subj_code.ContainsKey(subject_no.Trim()))
                                                        {
                                                            temp_has_subj_code.Add(subject_no.Trim(), subject_no.Trim());
                                                            recflag = true;
                                                            if (samehr_flag == false)
                                                            {
                                                                span_count++;
                                                                samehr_flag = true;
                                                            }
                                                            Hashtable has_stud_list = new Hashtable();
                                                            if (batchflag == true)
                                                            {
                                                                subj_type = "1";
                                                            }
                                                            else
                                                            {
                                                                subj_type = "0";
                                                            }
                                                            if (subj_type != "1" && subj_type != "True" && subj_type != "TRUE" && subj_type != "true")
                                                            {
                                                                string strquery = "select distinct r.roll_no,a." + date_temp_field + " as attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from registration r ,subjectchooser s,attendance a where r.roll_no=a.roll_no and a.roll_no=s.roll_no and a.month_year='" + month_year + "' and r.degree_code='" + newBranchCode.Replace("'", "").Trim() + "' and batch_year='" + newBatchYear + "' and cc=0 and delflag=0 and exam_flag<>'debar' and r.roll_no=s.roll_no and s.semester= '" + newsemester + "' " + strsec + " and  subject_no='" + subject_no + "' " + strOrderBy + "";
                                                                //DataSet dsquery = d2.select_method(strquery, hat, "Text");
                                                                DataSet dsquery = new DataSet();
                                                                if (dsTheorySchedule.Tables.Count > 0 && dsTheorySchedule.Tables[0].Rows.Count > 0)
                                                                {
                                                                    dsTheorySchedule.Tables[0].DefaultView.RowFilter = "month_year='" + month_year + "' and  subject_no='" + subject_no + "'";
                                                                    DataTable dtTemp = new DataTable();
                                                                    dtTemp = dsTheorySchedule.Tables[0].DefaultView.ToTable();
                                                                    dsquery.Tables.Add(dtTemp);
                                                                }
                                                                //dsquery = d2.select_method(strquery, hat, "Text");
                                                                if (dsquery.Tables.Count > 0 && dsquery.Tables[0].Rows.Count > 0)
                                                                {
                                                                    for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                    {
                                                                        string rollno = Convert.ToString(dsquery.Tables[0].Rows[i]["Roll_no"]).Trim();
                                                                        if (!hatattendance.Contains(rollno.Trim().ToLower()))
                                                                        {
                                                                            //hatattendance.Add(rollno.Trim(), Convert.ToString(dsquery.Tables[0].Rows[i]["attvalue"]).Trim());
                                                                            hatattendance.Add(rollno.Trim().ToLower(), Convert.ToString(dsquery.Tables[0].Rows[i][date_temp_field]).Trim());
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                string strquery = "select distinct r.roll_no,a." + date_temp_field + " AS attvalue, convert(varchar(15),adm_date,103) as adm_date,r.Reg_No,r.Stud_Name,r.serialno from  registration r,subjectchooser s,laballoc l,attendance a  where r.roll_no=a.roll_no and a.roll_no=s.roll_no and a.month_year='" + month_year + "' and r.degree_code='" + newBranchCode.Replace("'", "").Trim() + "' and r.batch_year='" + newBatchYear + "' and cc=0 and delflag=0 and exam_flag<>'debar'  " + rstrsec + " and r.roll_no=s.roll_no and s.subject_no='" + subject_no + "' and r.degree_code=l.degree_code and r.batch_year=l.batch_year and l.Semester=r.Current_Semester and s.subject_no =l.subject_no and s.batch=l.stu_batch  and hour_value='" + temp_hr + "'  and day_value='" + strDay + "' and l.subject_no='" + subject_no + "' " + section_lab + " " + strOrderBy + "";
                                                                //and s.batch=l.stu_batch 
                                                                //DataSet dsquery = d2.select_method(strquery, hat, "Text");
                                                                //DataSet dsquery = d2.select_method(strquery, hat, "Text");
                                                                DataSet dsquery = new DataSet();
                                                                if (dsPracticalSchedule.Tables.Count > 0 && dsPracticalSchedule.Tables[0].Rows.Count > 0)
                                                                {
                                                                    dsPracticalSchedule.Tables[0].DefaultView.RowFilter = "month_year='" + month_year + "' and  subject_no='" + subject_no + "'  and hour_value='" + temp_hr + "'  and day_value='" + strDay + "' ";//and section='A'
                                                                    DataTable dtTemp = new DataTable();
                                                                    dtTemp = dsPracticalSchedule.Tables[0].DefaultView.ToTable();
                                                                    dsquery.Tables.Add(dtTemp);
                                                                }
                                                                //dsquery = d2.select_method(strquery, hat, "Text");
                                                                if (dsquery.Tables.Count > 0 && dsquery.Tables[0].Rows.Count > 0)
                                                                {
                                                                    for (int i = 0; i < dsquery.Tables[0].Rows.Count; i++)
                                                                    {
                                                                        string rollno = Convert.ToString(dsquery.Tables[0].Rows[i]["Roll_no"]).Trim();
                                                                        if (!hatattendance.Contains(rollno.Trim().ToLower()))
                                                                        {
                                                                            //hatattendance.Add(rollno.Trim(), Convert.ToString(dsquery.Tables[0].Rows[i]["attvalue"]).Trim());
                                                                            hatattendance.Add(rollno.Trim().ToLower(), Convert.ToString(dsquery.Tables[0].Rows[i][date_temp_field]).Trim());
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (hatattendance.Count > 0)
                                                            {
                                                                for (int i = 0; i < dtl.Rows.Count-2; i++)
                                                                {//pppp
                                                                    string rollno = Convert.ToString(dtl.Rows[i+2]["Roll No"].ToString()).Trim();
                                                                    if (hatattendance.Contains(rollno.Trim().ToLower()))
                                                                    {
                                                                        Admission_date = Convert.ToDateTime(admissiondate[i].Trim());
                                                                        string attvalue = Convert.ToString(GetCorrespondingKey(rollno.Trim().ToLower(), hatattendance)).Trim();
                                                                        string value = Attmark(Convert.ToString(attvalue).Trim());
                                                                        if (dtTempDate >= Admission_date)
                                                                        {
                                                                            
                                                                            if (attvalue == "3")
                                                                            {
                                                                                temp_tag = "3";
                                                                            }
                                                                            else
                                                                            {
                                                                                temp_tag = "0";
                                                                            }
                                                                            if ((attvalue.ToString()) != "8" && (attvalue.ToString()) != "7")//modified by prabha on 20 dec 2017 --  && (attvalue.ToString()) != "8" has been added
                                                                            {
                                                                                if (value != "HS")
                                                                                {
                                                                                    if (temp_tag == "0")
                                                                                    {
                                                                                        if (has_attnd_masterset.ContainsKey(attvalue.Trim()))
                                                                                        {
                                                                                            if (temp_tag == "0")
                                                                                            {
                                                                                                present_count = Convert.ToInt16(GetCorrespondingKey(rollno.Trim().ToLower(), has_load_rollno));
                                                                                                present_count++;
                                                                                                has_load_rollno[rollno.ToLower().Trim()] = present_count;
                                                                                            }
                                                                                        }
                                                                                        if (value != "NE")
                                                                                        {
                                                                                            {
                                                                                                present_count = Convert.ToInt16(GetCorrespondingKey(rollno.Trim().ToLower(), has_total_attnd_hour));
                                                                                                present_count++;
                                                                                                has_total_attnd_hour[rollno.Trim().ToLower()] = present_count;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (!has_od.ContainsKey(rollno.Trim().ToLower()))
                                                                                        {
                                                                                            has_od.Add(rollno.ToLower().Trim(), 1);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            od_count = Convert.ToInt16(GetCorrespondingKey(rollno.Trim().ToLower(), has_od));
                                                                                            od_count++;
                                                                                            has_od[rollno.Trim().ToLower()] = od_count;
                                                                                        }
                                                                                        if (value != "NE")
                                                                                        {
                                                                                            present_count = Convert.ToInt16(GetCorrespondingKey(rollno.Trim().ToLower(), has_total_attnd_hour));
                                                                                            present_count++;
                                                                                            has_total_attnd_hour[rollno.Trim().ToLower()] = present_count;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                    }
                                                                }//pppp
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
                }
                dtTempDate = dtTempDate.AddDays(1);
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private Hashtable HashValueToZero(Hashtable theHash)
    {
        object[] keys = new object[theHash.Keys.Count];
        theHash.Keys.CopyTo(keys, 0);
        foreach (object key in keys)
        {
            theHash[key] = 0;
        }
        return theHash;
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

    public void getspecial_hr(Hashtable htSplHr)
    {
        try
        {
            string hrdetno = string.Empty;
            if (htSplHr.Contains(Convert.ToString(dtTempDate).Trim()))
            {
                hrdetno = Convert.ToString(GetCorrespondingKey(Convert.ToString(dtTempDate).Trim(), htSplHr));
            }
            if (hrdetno != "")
            {
                DataSet ds_splhr_query_master = new DataSet();
                string splhr_query_master = "select spa.roll_no,spa.attendance from  specialhr_attendance spa,specialhr_details spd where spa.hrdet_no=spd.hrdet_no and spd.subject_no='" + subject_no + "'  and spd.hrdet_no in(" + hrdetno + ") order by spa.roll_no asc";
                ds_splhr_query_master = d2.select_method_wo_parameter(splhr_query_master, "text");
                //SqlDataReader dr_splhr_query_master;
                //cmd = new SqlCommand(splhr_query_master, con_splhr_query_master);
                //dr_splhr_query_master = cmd.ExecuteReader();
                if (ds_splhr_query_master.Tables.Count > 0 && ds_splhr_query_master.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr_splhr_query_master in ds_splhr_query_master.Tables[0].Rows)
                    {
                        if (hatsplhrattendance.Contains(Convert.ToString(dr_splhr_query_master[0]).Trim().ToLower()))
                        {
                            roll_count = Convert.ToInt32(GetCorrespondingKey(Convert.ToString(dr_splhr_query_master[0]).Trim().ToLower(), hatsplhrattendance));
                            recflag = true;
                            if (dtl.Rows[roll_count]["Roll No"].ToString().Trim().ToLower() == Convert.ToString(dr_splhr_query_master[0]).Trim().ToLower())
                            {
                                
                                if ((Convert.ToString(dr_splhr_query_master[1]).Trim()) != "8")
                                {
                                    if (Attmark(Convert.ToString(dr_splhr_query_master[1]).Trim()) != "HS")
                                    {
                                        if (has_attnd_masterset.ContainsKey((Convert.ToString(dr_splhr_query_master[1]).Trim())))
                                        {
                                            present_count = Convert.ToInt16(has_load_rollno[Convert.ToString(dr_splhr_query_master[0]).Trim().ToLower()].ToString());
                                            present_count++;
                                            has_load_rollno[dtl.Rows[roll_count]["Roll No"].ToString().Trim().ToLower()] = present_count;
                                        }
                                        if (Attmark(dr_splhr_query_master[1].ToString()) != "NE")
                                        {
                                            present_count = Convert.ToInt16(has_total_attnd_hour[Convert.ToString(dr_splhr_query_master[0]).Trim().ToLower()].ToString());
                                            present_count++;
                                            has_total_attnd_hour[dtl.Rows[roll_count]["Roll No"].ToString().Trim().ToLower()] = present_count;
                                        }
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

    private void GetAutoSwitchLab(string batchYear, string degreeCode, string semester, string section, string timeTableName, DateTime dtCurrentLabDate)
    {
        try
        {
        }
        catch (Exception ex)
        {
        }
    }

    private void LoadAutoSwitchLab(string batchYear, string degreeCode, string semester, string section, string timeTableName, ref Dictionary<string, string> dicAutoSwitchLab)
    {
        try
        {
            DataSet dsAutoSwitch = new DataSet();
            if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(timeTableName))
            {
                string qry = "SELECT sm.Batch_Year,sm.degree_code,sm.semester,ltrim(rtrim(isnull(l.Sections,''))) Sections,l.Timetablename,sch.FromDate,l.Day_Value,case when l.Day_Value='mon' then '1' when l.Day_Value='tue' then '2' when l.Day_Value='wed' then '3' when l.Day_Value='thu' then '4' when l.Day_Value='fri' then '5' when l.Day_Value='sat' then '6' when l.Day_Value='sun' then '7' end as Day_Code,l.Hour_Value,ltrim(rtrim(isnull(l.Auto_Switch,''))) as Auto_Switch,l.Stu_Batch from LabAlloc l,syllabus_master sm,Semester_Schedule sch where l.Degree_Code=sm.degree_code and l.Batch_Year=sm.Batch_Year and sm.semester=l.Semester and sm.degree_code=sch.degree_code and l.Degree_Code=sch.degree_code and sch.batch_year=l.Batch_Year and sch.batch_year=l.Batch_Year and sch.semester=sm.semester and sch.semester=l.Semester and ltrim(rtrim(isnull(sch.Sections,'')))=ltrim(rtrim(isnull(l.Sections,''))) and TTName=l.Timetablename and  sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and ltrim(rtrim(isnull(l.Sections,''))) ='" + section + "' and Timetablename='" + timeTableName + "'  order by sm.Batch_Year desc,sm.degree_code,sm.semester asc,l.Sections,l.Timetablename,l.Day_Value,l.Hour_Value,l.Stu_Batch,sch.FromDate,Day_Code; SELECT distinct sm.Batch_Year,sm.degree_code,sm.semester,ltrim(rtrim(isnull(l.Sections,''))) Sections,l.Timetablename,sch.FromDate,l.Day_Value,case when l.Day_Value='mon' then '1' when l.Day_Value='tue' then '2' when l.Day_Value='wed' then '3' when l.Day_Value='thu' then '4' when l.Day_Value='fri' then '5' when l.Day_Value='sat' then '6' when l.Day_Value='sun' then '7' end as Day_Code,l.Hour_Value,ltrim(rtrim(isnull(l.Auto_Switch,''))) as Auto_Switch,Count(l.Stu_Batch) as noOfBatch from LabAlloc l,syllabus_master sm,Semester_Schedule sch where l.Degree_Code=sm.degree_code and l.Batch_Year=sm.Batch_Year and sm.semester=l.Semester and sm.degree_code=sch.degree_code and l.Degree_Code=sch.degree_code and sch.batch_year=l.Batch_Year and sch.batch_year=l.Batch_Year and sch.semester=sm.semester and sch.semester=l.Semester and ltrim(rtrim(isnull(sch.Sections,'')))=ltrim(rtrim(isnull(l.Sections,''))) and TTName=l.Timetablename and sm.Batch_Year='" + batchYear + "' and sm.degree_code='" + degreeCode + "' and sm.semester='" + semester + "' and ltrim(rtrim(isnull(l.Sections,''))) ='" + section + "' and Timetablename='" + timeTableName + "' group by sm.Batch_Year,sm.degree_code,sm.semester,l.Sections,l.Timetablename,sch.FromDate,l.Day_Value,l.Hour_Value,Auto_Switch order by sm.Batch_Year desc,sm.degree_code,sm.semester asc,l.Sections,l.Timetablename,l.Day_Value,l.Hour_Value,l.Stu_Batch,sch.FromDate,Day_Code";
                dsAutoSwitch = d2.select_method_wo_parameter(qry, "Text");
            }
            DataSet dsSemInfo = new DataSet();
            string qrySemInfo = "select r.college_code,si.batch_year,si.degree_code,si.semester,ltrim(rtrim(isnull(r.sections,''))) as sections,si.start_date,si.end_date,si.starting_dayorder,p.nodays,p.No_of_hrs_per_day,p.min_hrs_per_day,p.no_of_hrs_I_half_day,p.min_pres_I_half_day,p.no_of_hrs_II_half_day,p.min_pres_II_half_day,p.schOrder from seminfo si,registration r,PeriodAttndSchedule p where p.degree_code=si.degree_code and p.degree_code=r.degree_code and si.semester=p.semester and r.Batch_year=si.batch_year and si.degree_code=r.degree_code  and r.cc=0 and r.delflag=0 and exam_falg<>'debar' and r.Batch_Year='" + batchYear + "' and r.degree_code='" + degreeCode + "' and si.semester='" + semester + "' and ltrim(rtrim(isnull(r.Sections,''))) ='" + section + "' order by si.Batch_year desc,si.degree_code,si.semester,r.Sections asc";
            //dsSemInfo = d2.select_method_wo_parameter(qrySemInfo, "text");
            // key : batch@DegreeCode@Semster@Section Value : Key- Date Value  B1[0] B2[1]
            Dictionary<string, Dictionary<DateTime, string[]>> dicAutoSwitch = new Dictionary<string, Dictionary<DateTime, string[]>>();
            Dictionary<string, Dictionary<DateTime, AutoSwitchLab>> dicAutoSwitch1 = new Dictionary<string, Dictionary<DateTime, AutoSwitchLab>>();
            dicAutoSwitchLab = new Dictionary<string, string>();
            if (dsAutoSwitch.Tables.Count > 1 && dsAutoSwitch.Tables[1].Rows.Count > 0)
            {
                dsAutoSwitch.Tables[1].DefaultView.RowFilter = "Batch_Year='" + batchYear + "' and degree_code='" + degreeCode + "' and semester='" + semester + "' and Sections='" + section + "' and Timetablename='" + timeTableName + "' and auto_switch<>''";
                DataTable dtDate = new DataTable();
                DataTable dtLabBatch = new DataTable();
                dtDate = dsAutoSwitch.Tables[1].DefaultView.ToTable();
                if (dtDate.Rows.Count > 0)
                {
                    for (int au = 0; au < dtDate.Rows.Count; au++)
                    {
                        string autoSwitch = Convert.ToString(dtDate.Rows[au]["Day_Value"]).Trim() + Convert.ToString(dtDate.Rows[au]["Hour_Value"]).Trim();
                        if (!dicAutoSwitchLab.ContainsKey(autoSwitch.Trim().ToLower()))
                        {
                            dicAutoSwitchLab.Add(autoSwitch.Trim().ToLower(), Convert.ToString(dtDate.Rows[au]["auto_switch"]).Trim() + '-' + Convert.ToString(dtDate.Rows[au]["no_of_batch"]).Trim());
                        }
                    }
                }
            }
            AutoSwitchLab autoSwitchLab;
            if (false)
            {
                if (dsAutoSwitch.Tables.Count > 0 && dsAutoSwitch.Tables[0].Rows.Count > 0)
                {
                    if (dsSemInfo.Tables.Count > 0 && dsSemInfo.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drSemInfo in dsSemInfo.Tables[0].Rows)
                        {
                            string startDate = Convert.ToString(drSemInfo["start_date"]).Trim();
                            string endDate = Convert.ToString(drSemInfo["end_date"]).Trim();
                            string collegeCode = Convert.ToString(drSemInfo["college_code"]).Trim();
                            string batchYear1 = Convert.ToString(drSemInfo["batch_year"]).Trim();
                            string degreeCode1 = Convert.ToString(drSemInfo["degree_code"]).Trim();
                            string semester1 = Convert.ToString(drSemInfo["semester"]).Trim();
                            string section1 = Convert.ToString(drSemInfo["sections"]).Trim();
                            string startingDayOrder = Convert.ToString(drSemInfo["starting_dayorder"]).Trim();
                            string noOfDays = Convert.ToString(drSemInfo["nodays"]).Trim();
                            string noOfHrsPerDay = Convert.ToString(drSemInfo["No_of_hrs_per_day"]).Trim();
                            string minHrsPerDay = Convert.ToString(drSemInfo["min_hrs_per_day"]).Trim();
                            string noOfFirstHalfPerDay = Convert.ToString(drSemInfo["no_of_hrs_I_half_day"]).Trim();
                            string minFirstHalfPerDay = Convert.ToString(drSemInfo["min_pres_I_half_day"]).Trim();
                            string noOfSecondHalfPerDay = Convert.ToString(drSemInfo["no_of_hrs_II_half_day"]).Trim();
                            string minSecondHalfPerDay = Convert.ToString(drSemInfo["min_pres_II_half_day"]).Trim();
                            string scheduleOrder = Convert.ToString(drSemInfo["schOrder"]).Trim();

                            int noOfDaysPerSem = 0;
                            int totalHrsPerDay = 0;
                            int minPresentPerDay = 0;
                            int totalFirstHalf = 0;
                            int minFirstHalf = 0;
                            int totalSecondHalf = 0;
                            int minSecondHalf = 0;

                            string keyValue = batchYear1 + "@" + degreeCode1 + "@" + semester1 + "@" + section1;
                            DateTime dtStartDate = new DateTime();
                            DateTime dtEndDate = new DateTime();
                            DateTime.TryParseExact(startDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtStartDate);
                            DateTime.TryParseExact(endDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtEndDate);
                            string[] prevBatch = new string[0];
                            for (DateTime dtDummyDate = dtStartDate; dtDummyDate <= dtEndDate; dtDummyDate = dtDummyDate.AddDays(1))
                            {
                                int.TryParse(noOfDays, out noOfDaysPerSem);
                                int.TryParse(noOfHrsPerDay, out totalHrsPerDay);
                                int.TryParse(minHrsPerDay, out minPresentPerDay);
                                int.TryParse(noOfFirstHalfPerDay, out totalFirstHalf);
                                int.TryParse(minFirstHalfPerDay, out minFirstHalf);
                                int.TryParse(noOfSecondHalfPerDay, out totalSecondHalf);
                                int.TryParse(minSecondHalfPerDay, out minSecondHalf);
                                int startHr = 1;
                                int endHr = totalHrsPerDay;
                                string dayName = string.Empty;
                                string dayCode = string.Empty;
                                bool ishoilday = false;
                                bool isholimorn = false;
                                bool isholieven = false;
                                int fhrs = 0;
                                isholidayCheck(collegeCode, degreeCode1, semester1, dtDummyDate.ToString("dd/MM/yyyy"), out ishoilday, out isholimorn, out isholieven, out fhrs);
                                bool isWorkingDay = false;
                                bool isHoliday = false;

                                if (scheduleOrder.Trim().ToLower() != "0")
                                {
                                    dayName = dtDummyDate.ToString("ddd");
                                }
                                else
                                {
                                    string[] sp = dtDummyDate.ToString("dd/MM/yyyy").Split('/');
                                    string curdate = sp[1] + '/' + sp[0] + '/' + sp[2];
                                    dayName = d2.findday(curdate, degreeCode1, semester1, batchYear1, startDate, noOfDays, startingDayOrder);
                                }
                                dayCode = GetDayCode(dayName).ToString();
                                if (ishoilday && isholimorn && isholieven)
                                {
                                    isWorkingDay = false;
                                    isHoliday = true;
                                    startHr = 0;
                                    endHr = 0;
                                }
                                else if (!ishoilday && !isholimorn && !isholieven)
                                {
                                    isWorkingDay = true;
                                    isHoliday = false;
                                    startHr = 1;
                                    endHr = totalHrsPerDay;
                                }
                                else if (!ishoilday && !isholimorn && isholieven)
                                {
                                    isWorkingDay = true;
                                    isHoliday = false;
                                    startHr = 1;
                                    endHr = totalFirstHalf;
                                }
                                else if (!ishoilday && isholimorn && !isholieven)
                                {
                                    isWorkingDay = true;
                                    isHoliday = false;
                                    startHr = totalFirstHalf + 1;
                                    endHr = totalHrsPerDay;
                                }
                                //else
                                //{
                                //    isHoliday = true;
                                //    isWorkingDay = false;
                                //}
                                if (isWorkingDay)
                                {
                                    if (dsAutoSwitch.Tables.Count > 0 && dsAutoSwitch.Tables[0].Rows.Count > 0)
                                    {
                                        dsAutoSwitch.Tables[0].DefaultView.RowFilter = "Batch_Year='" + batchYear1 + "' and degree_code='" + degreeCode1 + "' and semester='" + semester1 + "' and Sections='" + section1 + "' and Timetablename='" + timeTableName + "' and Day_Value='" + dayName + "' and Hour_Value>='" + startHr + "' and Hour_Value<='" + endHr + "'";
                                        DataTable dtDate = new DataTable();
                                        DataTable dtLabBatch = new DataTable();
                                        dtDate = dsAutoSwitch.Tables[0].DefaultView.ToTable();
                                        if (dtDate.Rows.Count > 0)
                                        {
                                            dtDate.DefaultView.RowFilter = "Auto_Switch<>''";
                                            dtLabBatch = dtDate.DefaultView.ToTable();
                                            if (dtLabBatch.Rows.Count == 0)
                                            {
                                                dtDate.DefaultView.RowFilter = "Auto_Switch=''";
                                                dtLabBatch = dtDate.DefaultView.ToTable();
                                                if (dtLabBatch.Rows.Count > 0)
                                                {
                                                    List<string> list = dtLabBatch.AsEnumerable()
                                                           .Select(r => r.Field<string>("Stu_Batch"))
                                                           .ToList();
                                                    string[] labBatch = list.Distinct().ToArray();
                                                    string batchList = string.Join(",", list.ToArray().Distinct());
                                                    foreach (DataRow drLabBatch in dtLabBatch.Rows)
                                                    {
                                                        Dictionary<DateTime, string[]> dicTemp = new Dictionary<DateTime, string[]>();
                                                        if (!dicAutoSwitch.ContainsKey(keyValue.Trim().ToLower()))
                                                        {
                                                            dicTemp = new Dictionary<DateTime, string[]>();
                                                        }
                                                        else
                                                        {
                                                        }
                                                        if (!dicAutoSwitch1.ContainsKey(keyValue.Trim().ToLower()))
                                                        {
                                                        }
                                                        else
                                                        {
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                List<string> list = dtLabBatch.AsEnumerable()
                                                           .Select(r => r.Field<string>("Stu_Batch"))
                                                           .ToList();
                                                string[] labBatch = list.Distinct().ToArray();
                                                string batchList = string.Join(",", list.ToArray().Distinct());
                                                foreach (DataRow drLabBatch in dtLabBatch.Rows)
                                                {
                                                    string autoSwitch = Convert.ToString(drLabBatch["Day_Value"]).Trim() + Convert.ToString(drLabBatch["Hour_Value"]).Trim();
                                                    if (dicAutoSwitchLab.ContainsKey(autoSwitch.Trim().ToLower()))
                                                    {
                                                        Dictionary<DateTime, string[]> dicTemp = new Dictionary<DateTime, string[]>();
                                                        if (!dicAutoSwitch.ContainsKey(keyValue.Trim().ToLower()))
                                                        {
                                                            dicTemp.Add(dtDummyDate, labBatch);
                                                            prevBatch = labBatch;
                                                        }
                                                        else
                                                        {
                                                        }
                                                        if (!dicAutoSwitch1.ContainsKey(keyValue.Trim().ToLower()))
                                                        {
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

    /// <summary>
    /// Developed By Malang Raja T 
    /// </summary>
    /// <param name="college_code"></param>
    /// <param name="degree_code"></param>
    /// <param name="semester"></param>
    /// <param name="frdate">dd/MM/yyyy</param>
    /// <param name="ishoilday"></param>
    /// <param name="isholimorn"></param>
    /// <param name="isholieven"></param>
    /// <param name="fhrs"></param>
    public void isholidayCheck(string college_code, string degree_code, string semester, string frdate, out bool ishoilday, out bool isholimorn, out bool isholieven, out int fhrs)
    {
        Hashtable holiday_table = new Hashtable();
        DataSet ds2 = new DataSet();
        DataSet ds_holi = new DataSet();
        DateTime dumm_from_date = new DateTime();
        string[] dsplit = frdate.Split(new Char[] { '/' });
        frdate = Convert.ToString(dsplit[2]) + "/" + Convert.ToString(dsplit[1]) + "/" + Convert.ToString(dsplit[0]);
        dumm_from_date = Convert.ToDateTime(frdate);
        ishoilday = false;
        isholimorn = false;
        isholieven = false;
        fhrs = 0;
        try
        {
            hat.Clear();
            hat.Add("degree_code", degree_code);
            hat.Add("sem", semester);
            hat.Add("from_date", frdate);
            hat.Add("to_date", frdate);
            hat.Add("coll_code", college_code);
            int iscount = 0;
            string strquery = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate + "' and '" + frdate + "' and degree_code=" + degree_code + " and semester=" + semester + "";
            ds2.Reset();
            ds2.Dispose();
            ds2 = d2.select_method(strquery, hat, "Text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                iscount = 0;
                int.TryParse(Convert.ToString(ds2.Tables[0].Rows[0]["cnt"]), out iscount);
            }
            hat.Add("iscount", iscount);
            ds_holi = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
            holiday_table.Clear();
            if (ds_holi.Tables.Count > 0 && ds_holi.Tables[0].Rows.Count != 0)
            {
                for (int k = 0; k < ds_holi.Tables[0].Rows.Count; k++)
                {
                    if (!holiday_table.ContainsKey(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[0].Rows[k]["HOLI_DATE"]).Trim())))
                    {
                        holiday_table.Add(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[0].Rows[k]["HOLI_DATE"]).Trim()), k);
                    }
                }
            }
            if (ds_holi.Tables.Count > 1 && ds_holi.Tables[1].Rows.Count != 0)
            {
                for (int k = 0; k < ds_holi.Tables[1].Rows.Count; k++)
                {
                    if (!holiday_table.ContainsKey(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[1].Rows[k]["HOLI_DATE"]).Trim())))
                    {
                        holiday_table.Add(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[1].Rows[k]["HOLI_DATE"]).Trim()), k);
                    }
                }
            }
            if (ds_holi.Tables.Count > 2 && ds_holi.Tables[2].Rows.Count != 0)
            {
                for (int k = 0; k < ds_holi.Tables[2].Rows.Count; k++)
                {
                    if (!holiday_table.ContainsKey(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[2].Rows[k]["HOLI_DATE"]).Trim())))
                    {
                        holiday_table.Add(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[2].Rows[k]["HOLI_DATE"]).Trim()), k);
                    }
                }
            }
            fhrs = 0;
            string hrs = d2.GetFunction("select no_of_hrs_I_half_day from periodattndschedule where degree_code='" + degree_code + "' and semester='" + semester + "'");
            if (hrs.Trim() != "" && hrs != null && hrs.Trim() != "0")
            {
                int.TryParse(hrs, out fhrs);
            }
            if (!holiday_table.ContainsKey(dumm_from_date))
            {
                ishoilday = false;
                isholimorn = false;
                isholieven = false;
            }
            else
            {
                ishoilday = true;
                isholimorn = false;
                isholieven = false;
                int starthout = 0;
                string strholyquery = "select halforfull,morning,evening from holidaystudents where halforfull=1 and holiday_date='" + dumm_from_date.ToString("MM/dd/yyyy") + "'";
                DataSet dsholidayval = d2.select_method_wo_parameter(strholyquery, "Text");
                if (dsholidayval.Tables.Count > 0 && dsholidayval.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToString(dsholidayval.Tables[0].Rows[0]["morning"]) == "1" || Convert.ToString(dsholidayval.Tables[0].Rows[0]["morning"]).Trim().ToLower() == "true")
                    {
                        ishoilday = false;
                        isholimorn = true;
                        isholieven = false;
                    }
                    if (Convert.ToString(dsholidayval.Tables[0].Rows[0]["evening"]) == "1" || Convert.ToString(dsholidayval.Tables[0].Rows[0]["evening"]).Trim().ToLower() == "true")
                    {
                        isholimorn = false;
                        ishoilday = false;
                        isholieven = true;
                    }
                }
                else
                {
                    ishoilday = true;
                    isholimorn = true;
                    isholieven = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public class AutoSwitchLab
    {
        private string[] dayName;
        private int[] dayCode;
        private int[] hourList;
        private string[] labBatch;
        private bool autoSwitch = false;
        private DateTime dtLabDate;
        private Dictionary<string[], string[]> dicAutoSwitchLabBatch = new Dictionary<string[], string[]>();
        public DateTime LabDate
        {
            get
            {
                return dtLabDate;
            }
            set
            {
                dtLabDate = value;
            }
        }
        public string[] DayName
        {
            get
            {
                return dayName;
            }
            set
            {
                dayName = value;
            }
        }
        public string[] LabBatch
        {
            get
            {
                return labBatch;
            }
            set
            {
                labBatch = value;
            }
        }
        public int[] DayCode
        {
            get
            {
                return dayCode;
            }
            set
            {
                dayCode = value;
            }
        }
        public int[] HourList
        {
            get
            {
                return hourList;
            }
            set
            {
                hourList = value;
            }
        }
        public bool IsAutoSwitch
        {
            get
            {
                return autoSwitch;
            }
            set
            {
                autoSwitch = value;
            }
        }
        public Dictionary<string[], string[]> DicAutoSwitchLabBatch
        {
            get
            {
                return dicAutoSwitchLabBatch;
            }
            set
            {
                dicAutoSwitchLabBatch = value;
            }
        }
        AutoSwitchLab()
        {
            dayName = new string[0];
            dayCode = new int[0];
            hourList = new int[0];
            labBatch = new string[0];
            dicAutoSwitchLabBatch = new Dictionary<string[], string[]>();
        }
    }

    private string GetDayName(string dayCode)
    {
        string dayName = string.Empty;
        try
        {
            dayCode = dayCode.Trim();
            switch (dayCode)
            {
                case "0":
                    dayName = Convert.ToString(DayOfWeek.Sunday).Substring(0, 3);
                    break;
                case "1":
                    dayName = Convert.ToString(DayOfWeek.Monday).Substring(0, 3);
                    break;
                case "2":
                    dayName = Convert.ToString(DayOfWeek.Tuesday).Substring(0, 3);
                    break;
                case "3":
                    dayName = Convert.ToString(DayOfWeek.Wednesday).Substring(0, 3);
                    break;
                case "4":
                    dayName = Convert.ToString(DayOfWeek.Thursday).Substring(0, 3);
                    break;
                case "5":
                    dayName = Convert.ToString(DayOfWeek.Friday).Substring(0, 3);
                    break;
                case "6":
                    dayName = Convert.ToString(DayOfWeek.Saturday).Substring(0, 3);
                    break;
            }
            return dayName;
        }
        catch (Exception ex)
        {
            return dayName;
        }
    }

    private int GetDayCode(string dayName)
    {
        string dayCode1 = string.Empty;
        int dayCode = 0;
        try
        {
            dayName = dayName.Trim().ToLower();
            switch (dayName)
            {
                case "sun":
                    dayCode1 = Convert.ToString(DayOfWeek.Sunday).Substring(0, 3);
                    int.TryParse(Convert.ToString(DayOfWeek.Sunday).Trim(), out dayCode);
                    break;
                case "mon":
                    dayCode1 = Convert.ToString(DayOfWeek.Monday).Substring(0, 3);
                    int.TryParse(Convert.ToString(DayOfWeek.Monday).Trim(), out dayCode);
                    break;
                case "tue":
                    dayCode1 = Convert.ToString(DayOfWeek.Tuesday).Substring(0, 3);
                    int.TryParse(Convert.ToString(DayOfWeek.Tuesday).Trim(), out dayCode);
                    break;
                case "wed":
                    dayCode1 = Convert.ToString(DayOfWeek.Wednesday).Substring(0, 3);
                    int.TryParse(Convert.ToString(DayOfWeek.Wednesday).Trim(), out dayCode);
                    break;
                case "thu":
                    dayCode1 = Convert.ToString(DayOfWeek.Thursday).Substring(0, 3);
                    int.TryParse(Convert.ToString(DayOfWeek.Thursday).Trim(), out dayCode);
                    break;
                case "fri":
                    dayCode1 = Convert.ToString(DayOfWeek.Friday).Substring(0, 3);
                    int.TryParse(Convert.ToString(DayOfWeek.Friday).Trim(), out dayCode);
                    break;
                case "sat":
                    dayCode1 = Convert.ToString(DayOfWeek.Saturday).Substring(0, 3);
                    int.TryParse(Convert.ToString(DayOfWeek.Saturday).Trim(), out dayCode);
                    break;
            }
            return dayCode;
        }
        catch (Exception ex)
        {
            return dayCode;
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
    #endregion Go Click
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
        spReportName.InnerHtml = "Subject Wise Attendance Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }


    public override void VerifyRenderingInServerForm(Control control)
    { }
}
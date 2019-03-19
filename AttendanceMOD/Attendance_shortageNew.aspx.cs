#region Namespace Declaration

using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using System.Collections;
using Farpoint = FarPoint.Web.Spread;
using System.Collections.Generic;
using System.Globalization;
using InsproDataAccess;

#endregion Namespace Declaration

public partial class Attendance_shortageNew : System.Web.UI.Page
{
    #region Field Declaration

    Hashtable hat = new Hashtable();

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    string batch_year = string.Empty;
    string degree_code = string.Empty;
    string semester = string.Empty;
    string section = string.Empty;

    string test_name = string.Empty;
    string test_no = string.Empty;
    string subject_no = string.Empty;

    string exam_type = string.Empty;
    string exam_code = string.Empty;

    string qry = string.Empty;
    string qrysec = string.Empty;

    bool isSchool = false;

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    InsproDirectAccess dirAcc = new InsproDirectAccess();

    string fromDate = string.Empty;
    string toDate = string.Empty;

    DateTime dtFromDate = new DateTime();
    DateTime dtToDate = new DateTime();
    bool isValidDate = false;

    TimeSpan tsFromToDiff = new TimeSpan();

    string UnmarkHours = string.Empty;
    string CurrentDate = string.Empty;
    Dictionary<string, string> dicDate = new Dictionary<string, string>();
    List<Dictionary<string, string>> allResults = new List<Dictionary<string, string>>();
    DataTable data = new DataTable();
    int colcnt = 0;
    ArrayList arrColHdrNames1 = new ArrayList();
    ArrayList arrColHdrNames2 = new ArrayList();
    ArrayList arrColHdrNames3 = new ArrayList();


    #region Attendance

    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds4 = new DataSet();
    DataSet ds1 = new DataSet();

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

    string frdate, todate;
    string halforfull = "", mng = "", evng = "", holiday_sched_details = "";
    string dd = "";
    string diff_date;
    string value, date;
    string tempvalue = "-1";


    string value_holi_status = "";
    string split_holiday_status_1 = "", split_holiday_status_2 = "";

    string[] split_holiday_status = new string[1000];

    double dif_date = 0;
    double dif_date1 = 0;
    double per_perhrs, per_abshrs, per_leavehrs;
    double per_hhday;
    double Present = 0;
    double Absent = 0;
    double Onduty = 0;
    double Leave = 0;
    double pre_present_date, pre_ondu_date, pre_leave_date, per_absent_date;
    double workingdays = 0;
    double per_workingdays = 0;
    double leave_pointer, absent_pointer;
    double leave_point, absent_point;
    double per_holidate;
    double njhr, njdate, per_njdate;
    double per_per_hrs;
    Double leavfinaeamount = 0;

    Double minpresday = 0, nohrsprsentperday = 0, noofdaypresen = 0;
    Double moringabsentfine = 0, eveingabsentfine = 0, studentabsentfine = 0;
    Double medicalLeaveDays = 0;


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
    int count;
    int next = 0;
    int minpresII = 0;
    int rows_count;
    int ObtValue = -1;
    int cal_from_date, cal_from_date_tmp;
    int cal_to_date, cal_to_date_tmp;
    int per_dum_unmark, dum_unmark;
    int tot_per_hrs;
    int holi_leav = 0, holi_absent = 0, leav_pt = 0, absent_pt = 0;
    int medicalLeaveHours = 0;
    int medicalLeaveCountPerSession = 0;

    #endregion Attendance


    #endregion Field Declaration

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
            usercode = Convert.ToString(Session["usercode"]);
            collegecode = Convert.ToString(Session["collegecode"]);
            singleuser = Convert.ToString(Session["single_user"]);
            group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }

            string grouporusercode1 = "";
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
                string schoolvalue = Convert.ToString(schoolds.Tables[0].Rows[0]["value"]);
                if (schoolvalue.Trim() == "0")
                {
                    isSchool = true;
                }
            }
            if (!IsPostBack)
            {
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");

                txtPerFrom.Attributes.Add("autocomplete", "off");
                txtPerTo.Attributes.Add("autocomplete", "off");

                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtMinPresentML.Text = "70";
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;

                popupdiv.Visible = false;
                lblpopuperr.Text = string.Empty;

                divMainContents.Visible = false;
                rptprint1.Visible = false;

                txtPerFrom.Text = string.Empty;
                txtPerTo.Text = string.Empty;
                lbl_norec1.Visible = false;

                #region LoadHeader

                Bindcollege();
                BindBatch();
                BindDegree();
                bindbranch();
                bindsem();
                BindSectionDetail();

                #endregion LoadHeader

                Session["daywise"] = "0";
                Session["hourwise"] = "0";
                Session["attdaywisecla"] = "0";
                string daywisecal = d2.GetFunction("select value from Master_Settings where settings='Attendance Day Wise Calculation'");
                if (daywisecal.Trim() == "1")
                {
                    Session["attdaywisecla"] = "1";
                }

                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                string grouporusercode = string.Empty;


                //string grouporusercode1 = "";
                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                }

                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                Session["Sex"] = "0";
                Session["flag"] = "-1";
                string Master1 = "";
                string strdayflag = "";
                //  string regularflag = "";
                string genderflag = "";
                Master1 = "select * from Master_Settings where " + grouporusercode + "";

                DataSet dsSettings = d2.select_method_wo_parameter(Master1, "text");
                string regularflag = "";
                if (dsSettings.Tables.Count > 0 && dsSettings.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsSettings.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToString(dsSettings.Tables[0].Rows[i]["settings"]).Trim() == "Roll No" && Convert.ToString(dsSettings.Tables[0].Rows[i]["value"]).Trim() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (Convert.ToString(dsSettings.Tables[0].Rows[i]["settings"]).Trim() == "Register No" && Convert.ToString(dsSettings.Tables[0].Rows[i]["value"]).Trim() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (Convert.ToString(dsSettings.Tables[0].Rows[i]["settings"]).Trim() == "Student_Type" && Convert.ToString(dsSettings.Tables[0].Rows[i]["value"]).Trim() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (Convert.ToString(dsSettings.Tables[0].Rows[i]["settings"]).Trim() == "sex" && Convert.ToString(dsSettings.Tables[0].Rows[i]["value"]).Trim() == "1")
                        {
                            Session["Sex"] = "1";
                        }

                        if (Convert.ToString(dsSettings.Tables[0].Rows[i]["settings"]).Trim() == "General" && Convert.ToString(dsSettings.Tables[0].Rows[i]["value"]).Trim() == "1")
                        {

                            Session["flag"] = 0;

                        }
                        if (Convert.ToString(dsSettings.Tables[0].Rows[i]["settings"]).Trim() == "As Per Lesson" && Convert.ToString(dsSettings.Tables[0].Rows[i]["value"]).Trim() == "1")
                        {

                            Session["flag"] = 1;

                        }

                        if (Convert.ToString(dsSettings.Tables[0].Rows[i]["settings"]).Trim() == "Male" && Convert.ToString(dsSettings.Tables[0].Rows[i]["value"]).Trim() == "1")
                        {

                            genderflag = " and (app.sex='0'";
                        }
                        if (Convert.ToString(dsSettings.Tables[0].Rows[i]["settings"]).Trim() == "Female" && Convert.ToString(dsSettings.Tables[0].Rows[i]["value"]).Trim() == "1")
                        {
                            if (genderflag.Trim() != "" && genderflag.Trim() != "\0")
                            {
                                genderflag = genderflag + " or app.sex='1'";
                            }
                            else
                            {
                                genderflag = " and (app.sex='1'";
                            }

                        }

                        if (Convert.ToString(dsSettings.Tables[0].Rows[i]["settings"]).Trim() == "Days Scholor" && Convert.ToString(dsSettings.Tables[0].Rows[i]["value"]).Trim() == "1")
                        {
                            strdayflag = " and (r.Stud_Type='Day Scholar'";
                        }

                        if (Convert.ToString(dsSettings.Tables[0].Rows[i]["settings"]).Trim() == "Hostel" && Convert.ToString(dsSettings.Tables[0].Rows[i]["value"]).Trim() == "1")
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
                        if (Convert.ToString(dsSettings.Tables[0].Rows[i]["settings"]).Trim() == "Regular")
                        {
                            regularflag = "and ((r.mode=1)";
                        }
                        if (Convert.ToString(dsSettings.Tables[0].Rows[i]["settings"]).Trim() == "Lateral")
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
                        if (Convert.ToString(dsSettings.Tables[0].Rows[i]["settings"]).Trim() == "Transfer")
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
                        if (Convert.ToString(dsSettings.Tables[0].Rows[i]["settings"]).Trim() == "Day Wise" && Convert.ToString(dsSettings.Tables[0].Rows[i]["value"]).Trim() == "1")
                        {
                            Session["daywise"] = "1";
                        }
                        if (Convert.ToString(dsSettings.Tables[0].Rows[i]["settings"]).Trim() == "Hour Wise" && Convert.ToString(dsSettings.Tables[0].Rows[i]["value"]).Trim() == "1")
                        {
                            Session["hourwise"] = "1";
                        }
                    }
                }

                if (strdayflag != null && strdayflag != "")
                {
                    strdayflag = strdayflag + ")";
                }
                Session["strvar"] = strdayflag;

                if (regularflag.Trim() != "")
                {
                    regularflag = regularflag + ")";
                }
                if (genderflag.Trim() != "")
                {
                    genderflag = genderflag + ")";
                }
                Session["strvar"] = Session["strvar"] + regularflag + genderflag;

                ChangeHeaderName(isSchool);

                if (CheckSettings())
                {
                    divMainContents.Visible = false;
                    rptprint1.Visible = false;
                    Showgrid.Visible = false;
                    lblpopuperr.Text = "Plaese Select Day Wise Or Hour Wise!!! You Must Choose Any One of These Settings!!!";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Page Load

    #region Bind Header

    public void Bindcollege()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            string columnfield = "";
            group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", Convert.ToString(columnfield));
            ds.Dispose();
            ds.Clear();
            ds.Reset();
            ds = d2.select_method("bind_college", hat, "sp");
            ddlCollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
            else
            {
                lblErrSearch.Text = "Set college rights to the staff";
                lblErrSearch.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void BindBatch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ddlbatch.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter("bind_batch", "sp");
            if (ds.Tables.Count > 0)
            {
                int count = ds.Tables[0].Rows.Count;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlbatch.DataSource = ds;
                    ddlbatch.DataTextField = "batch_year";
                    ddlbatch.DataValueField = "batch_year";
                    ddlbatch.DataBind();
                    ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
                }
                int count1 = ds.Tables[1].Rows.Count;
                if (count > 0)
                {
                    int max_bat = 0;
                    int.TryParse(Convert.ToString(ds.Tables[1].Rows[0][0]), out max_bat);
                    ddlbatch.SelectedValue = Convert.ToString(max_bat);
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void BindDegree()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ddldegree.Items.Clear();
            //if (group_user.Contains(';'))
            //{
            //    string[] group_semi = group_user.Split(';');
            //    group_user = Convert.ToString(group_semi[0]);
            //}

            usercode = Convert.ToString(Session["usercode"]);
            collegecode = Convert.ToString(Session["collegecode"]);
            singleuser = Convert.ToString(Session["single_user"]);
            group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }

            ds.Dispose();
            ds.Reset();
            //ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = d2.select_method("bind_degree", hat, "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void bindbranch()
    {
        try
        {

            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string course_id = Convert.ToString(ddldegree.SelectedValue);
            ddlbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void BindSectionDetail()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            string strbatch = Convert.ToString(ddlbatch.SelectedValue);
            string strbranch = Convert.ToString(ddlbranch.SelectedValue);

            ddlsec.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSectionDetail(strbatch, strbranch);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataBind();
                if (Convert.ToString(ds.Tables[0].Columns["sections"]) == string.Empty)
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
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void bindsem()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            string strbatchyear = Convert.ToString(ddlbatch.Text);
            string strbranch = Convert.ToString(ddlbranch.SelectedValue);

            ddlsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSem(strbranch, strbatchyear, collegecode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(Convert.ToString(i));
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsem.Items.Add(Convert.ToString(i));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void ChangeHeaderName(bool isschool)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = "";
            //lblCollege.Text = ((!isschool) ? "College" : "School");
            lbl_Batchyear.Text = ((!isschool) ? "Batch" : "Year");
            lbldegree.Text = ((!isschool) ? "Degree" : "School Type");
            lblbranch.Text = ((!isschool) ? "Department" : "Standard");
            lblsem.Text = ((!isschool) ? "Semester" : "Term");
            lblsec.Text = ((!isschool) ? "Section" : "Section");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }


    //Added By Saranyaevi 17.9.2018

    public void Load_Grid_Header()
    {
        try
        {
            colcnt = 0;

            arrColHdrNames1.Add("S.No");
            arrColHdrNames2.Add("S.No");
            arrColHdrNames3.Add("S.No");
            data.Columns.Add("col0");
            if (Convert.ToString(Session["Rollflag"]) == "1")
            {
                arrColHdrNames1.Add("RollNo");
                arrColHdrNames2.Add("RollNo");
                arrColHdrNames3.Add("RollNo");
                colcnt++;
                data.Columns.Add("col" + colcnt);
            }
            if (Convert.ToString(Session["Regflag"]) == "1")
            {
                arrColHdrNames1.Add("Reg No");
                arrColHdrNames2.Add("Reg No");
                arrColHdrNames3.Add("Reg No");
                colcnt++;
                data.Columns.Add("col" + colcnt);
            }

            arrColHdrNames1.Add("Name of The Students");
            arrColHdrNames2.Add("Name of The Students");
            arrColHdrNames3.Add("Name of The Students");
            colcnt++;
            data.Columns.Add("col" + colcnt);
            if (Convert.ToString(Session["Studflag"]) == "1")
            {
                arrColHdrNames1.Add("Student Type");
                arrColHdrNames2.Add("Student Type");
                arrColHdrNames3.Add("Student Type");
                colcnt++;
                data.Columns.Add("col" + colcnt);
            }
            colcnt = colcnt + 1;
            int clcnt = colcnt;
            bool hour = false;
            if (Convert.ToString(Session["hourwise"]).Trim() == "1")
            {
                hour = true;
                arrColHdrNames1.Add("For Hour Wise Attendance");
                arrColHdrNames2.Add("Hour Wise Attendance (Without Considering ML)");
                arrColHdrNames3.Add("Conducted Hours");
                data.Columns.Add("col" + clcnt);


                clcnt++;
                arrColHdrNames1.Add("For Hour Wise Attendance");
                arrColHdrNames2.Add("Hour Wise Attendance (Without Considering ML)");
                arrColHdrNames3.Add("No. of Hours Present");
                data.Columns.Add("col" + clcnt);

                clcnt++;
                arrColHdrNames1.Add("For Hour Wise Attendance");
                arrColHdrNames2.Add("Hour Wise Attendance (Without Considering ML)");
                arrColHdrNames3.Add("Attendance(Hour Wise) %");
                data.Columns.Add("col" + clcnt);


                clcnt++;
                arrColHdrNames1.Add("For Hour Wise Attendance");
                arrColHdrNames2.Add("Attendance % Considering ML For Hour Wise (MC Enclosed)");
                arrColHdrNames3.Add("No. of Hours Applied");
                data.Columns.Add("col" + clcnt);


                clcnt++;
                arrColHdrNames1.Add("For Hour Wise Attendance");
                arrColHdrNames2.Add("Attendance % Considering ML For Hour Wise (MC Enclosed)");
                arrColHdrNames3.Add("No. of Hours Considered");
                data.Columns.Add("col" + clcnt);


                clcnt++;
                arrColHdrNames1.Add("For Hour Wise Attendance");
                arrColHdrNames2.Add("Attendance % Considering ML For Hour Wise (MC Enclosed)");
                arrColHdrNames3.Add("Attendance% Considering ML");
                data.Columns.Add("col" + clcnt);


            }
            if (Convert.ToString(Session["daywise"]).Trim() == "1")
            {
                if (hour)
                    clcnt++;

                arrColHdrNames1.Add("For Day Wise Attendance");
                arrColHdrNames2.Add("Day Wise Attendance (Without Considering ML)");
                arrColHdrNames3.Add("Working Days");
                data.Columns.Add("col" + clcnt);

                clcnt++;
                arrColHdrNames1.Add("For Day Wise Attendance");
                arrColHdrNames2.Add("Day Wise Attendance (Without Considering ML)");
                arrColHdrNames3.Add("No. of Days Present");
                data.Columns.Add("col" + clcnt);

                clcnt++;
                arrColHdrNames1.Add("For Day Wise Attendance");
                arrColHdrNames2.Add("Day Wise Attendance (Without Considering ML)");
                arrColHdrNames3.Add("Attendance(Day Wise) %");
                data.Columns.Add("col" + clcnt);


                clcnt++;
                arrColHdrNames1.Add("For Day Wise Attendance");
                arrColHdrNames2.Add("Attendance % Considering ML For Day Wise (MC Enclosed)");
                arrColHdrNames3.Add("No. of Day Applied");
                data.Columns.Add("col" + clcnt);

                clcnt++;
                arrColHdrNames1.Add("For Day Wise Attendance");
                arrColHdrNames2.Add("Attendance % Considering ML For Day Wise (MC Enclosed)");
                arrColHdrNames3.Add("No. of Day Considered");
                data.Columns.Add("col" + clcnt);


                clcnt++;
                arrColHdrNames1.Add("For Day Wise Attendance");
                arrColHdrNames2.Add("Attendance % Considering ML For Day Wise (MC Enclosed)");
                arrColHdrNames3.Add("Attendance % Considering ML");
                data.Columns.Add("col" + clcnt);



            }

            clcnt++;
            arrColHdrNames1.Add("No of Times Permitted With Condonation");
            arrColHdrNames2.Add("No of Times Permitted With Condonation");
            arrColHdrNames3.Add("No of Times Permitted With Condonation");
            data.Columns.Add("col" + clcnt);


            clcnt++;
            arrColHdrNames1.Add("Principal Approval A-Approved NA-Not Approved");
            arrColHdrNames2.Add("Principal Approval A-Approved NA-Not Approved");
            arrColHdrNames3.Add("Principal Approval A-Approved NA-Not Approved");
            data.Columns.Add("col" + clcnt);




            DataRow drHdr1 = data.NewRow();
            DataRow drHdr2 = data.NewRow();
            DataRow drHdr3 = data.NewRow();


            for (int grCol = 0; grCol < data.Columns.Count; grCol++)
            {

                drHdr1["col" + grCol] = arrColHdrNames1[grCol];
                drHdr2["col" + grCol] = arrColHdrNames2[grCol];
                drHdr3["col" + grCol] = arrColHdrNames3[grCol];


            }

            data.Rows.Add(drHdr1);
            data.Rows.Add(drHdr2);
            data.Rows.Add(drHdr3);


        }

        catch
        {


        }



    }



    #endregion Bind Header

    #region DropDown Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContents.Visible = false;
            rptprint1.Visible = false;
            //txtMinPresentML.Text = "70";
            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();

            if (CheckSettings())
            {
                divMainContents.Visible = false;
                rptprint1.Visible = false;
                Showgrid.Visible = false;
                lblpopuperr.Text = "Plaese Select Day Wise Or Hour Wise!!! You Must Choose Any One of These Settings!!!";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContents.Visible = false;
            rptprint1.Visible = false;
            //txtMinPresentML.Text = "70";
            ////btnSave.Visible = false;
            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();

            if (CheckSettings())
            {
                divMainContents.Visible = false;
                rptprint1.Visible = false;
                Showgrid.Visible = false;
                lblpopuperr.Text = "Plaese Select Day Wise Or Hour Wise!!! You Must Choose Any One of These Settings!!!";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContents.Visible = false;
            //txtMinPresentML.Text = "70";
            rptprint1.Visible = false;
            bindbranch();
            bindsem();
            BindSectionDetail();

            if (CheckSettings())
            {
                divMainContents.Visible = false;
                rptprint1.Visible = false;
                Showgrid.Visible = false;
                lblpopuperr.Text = "Plaese Select Day Wise Or Hour Wise!!! You Must Choose Any One of These Settings!!!";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContents.Visible = false;
            rptprint1.Visible = false;
            //txtMinPresentML.Text = "70";
            bindsem();
            BindSectionDetail();

            if (CheckSettings())
            {
                divMainContents.Visible = false;
                rptprint1.Visible = false;
                Showgrid.Visible = false;
                lblpopuperr.Text = "Plaese Select Day Wise Or Hour Wise!!! You Must Choose Any One of These Settings!!!";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContents.Visible = false;
            rptprint1.Visible = false;
            //txtMinPresentML.Text = "70";
            BindSectionDetail();

            if (CheckSettings())
            {
                divMainContents.Visible = false;
                rptprint1.Visible = false;
                Showgrid.Visible = false;
                lblpopuperr.Text = "Plaese Select Day Wise Or Hour Wise!!! You Must Choose Any One of These Settings!!!";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContents.Visible = false;
            rptprint1.Visible = false;
            //txtMinPresentML.Text = "70";
            if (CheckSettings())
            {
                divMainContents.Visible = false;
                rptprint1.Visible = false;
                Showgrid.Visible = false;
                lblpopuperr.Text = "Plaese Select Day Wise Or Hour Wise!!! You Must Choose Any One of These Settings!!!";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion DropDown Events

    #region TextBox Changed

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContents.Visible = false;
            rptprint1.Visible = false;
            DateTime dtToday = new DateTime();
            dtToday = DateTime.Now;
            fromDate = txtFromDate.Text.Trim();
            toDate = txtToDate.Text.Trim();

            if (CheckSettings())
            {
                divMainContents.Visible = false;
                rptprint1.Visible = false;
                Showgrid.Visible = false;
                lblpopuperr.Text = "Plaese Select Day Wise Or Hour Wise!!! You Must Choose Any One of These Settings!!!";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (fromDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
                if (!isValidDate)
                {
                    txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
                    lblpopuperr.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
            else
            {
                txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblpopuperr.Text = "Please Choose From Date";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (toDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
                if (!isValidDate)
                {
                    txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
                    lblpopuperr.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
            else
            {
                txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblpopuperr.Text = "Please Choose To Date";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (dtFromDate > dtToday)
            {
                txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblpopuperr.Text = "From Date Must Be Lesser Than or Equal to Today Date";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (dtToDate > dtToday)
            {
                txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblpopuperr.Text = "To Date Must Be Lesser Than or Equal to Today Date";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (dtFromDate > dtToDate)
            {
                txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
                txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblpopuperr.Text = "From Date Must Be Lesser Than or Equal to To Date";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContents.Visible = false;
            rptprint1.Visible = false;

            DateTime dtToday = new DateTime();
            dtToday = DateTime.Now;
            fromDate = txtFromDate.Text.Trim();
            toDate = txtToDate.Text.Trim();

            if (CheckSettings())
            {
                divMainContents.Visible = false;
                rptprint1.Visible = false;
                Showgrid.Visible = false;
                lblpopuperr.Text = "Plaese Select Day Wise Or Hour Wise!!! You Must Choose Any One of These Settings!!!";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (fromDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
                if (!isValidDate)
                {
                    txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
                    lblpopuperr.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
            else
            {
                txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblpopuperr.Text = "Please Choose From Date";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (toDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
                if (!isValidDate)
                {
                    txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
                    lblpopuperr.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
            else
            {
                txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblpopuperr.Text = "Please Choose To Date";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (dtFromDate > dtToday)
            {
                txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblpopuperr.Text = "From Date Must Be Lesser Than or Equal to Today Date";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (dtToDate > dtToday)
            {
                txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblpopuperr.Text = "To Date Must Be Lesser Than or Equal to Today Date";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (dtFromDate > dtToDate)
            {
                txtFromDate.Text = dtToday.ToString("dd/MM/yyyy");
                txtToDate.Text = dtToday.ToString("dd/MM/yyyy");
                lblpopuperr.Text = "To Date Must Be Greater Than or Equal to From Date";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void txtPerFrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContents.Visible = false;
            rptprint1.Visible = false;
            string FromPercentage = txtPerFrom.Text.Trim();
            string ToPercentage = txtPerTo.Text.Trim();
            int fromPer = 0;
            int toPer = 0;

            if (CheckSettings())
            {
                divMainContents.Visible = false;
                rptprint1.Visible = false;
                Showgrid.Visible = false;
                lblpopuperr.Text = "Plaese Select Day Wise Or Hour Wise!!! You Must Choose Any One of These Settings!!!";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (FromPercentage != "")
            {
                isValidDate = int.TryParse(FromPercentage, out fromPer);
                if (!(fromPer >= 0 && fromPer <= 100))
                {
                    txtPerFrom.Text = string.Empty;
                    lblpopuperr.Text = "From Percentage Must Be Between 0 And 100";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
                if (!isValidDate)
                {
                    txtPerFrom.Text = string.Empty;
                    lblpopuperr.Text = "From Percentage Must Be In Numeric";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
            else
            {
                txtPerFrom.Text = string.Empty;
                lblpopuperr.Text = "Please Enter Any From Value";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            if (ToPercentage != "")
            {
                isValidDate = int.TryParse(ToPercentage, out toPer);
                if (!(toPer >= 0 && toPer <= 100))
                {
                    txtPerTo.Text = string.Empty;
                    lblpopuperr.Text = "To Percentage Must Be Between 0 And 100";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
                if (!isValidDate)
                {
                    txtPerTo.Text = string.Empty;
                    lblpopuperr.Text = "To Percentage Must Be In Numeric";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
            //else
            //{
            //    txtPerTo.Text = string.Empty;
            //    lblpopuperr.Text = "Please Enter Any To Value";
            //    lblpopuperr.Visible = true;
            //    popupdiv.Visible = true;
            //    return;
            //}
            //if (fromPer > toPer)
            //{
            //    txtPerTo.Text = string.Empty;
            //    txtPerFrom.Text = string.Empty;
            //    lblpopuperr.Text = "From Percentage Must Be Lesser Than or Equal To To Percentage";
            //    lblpopuperr.Visible = true;
            //    popupdiv.Visible = true;
            //    return;
            //}
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void txtPerTo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContents.Visible = false;
            rptprint1.Visible = false;
            string FromPercentage = txtPerFrom.Text.Trim();
            string ToPercentage = txtPerTo.Text.Trim();
            int toPer = 0;
            int fromPer = 0;

            if (CheckSettings())
            {
                divMainContents.Visible = false;
                rptprint1.Visible = false;
                Showgrid.Visible = false;
                lblpopuperr.Text = "Plaese Select Day Wise Or Hour Wise!!! You Must Choose Any One of These Settings!!!";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (FromPercentage != "")
            {
                isValidDate = int.TryParse(FromPercentage, out fromPer);
                if (!(fromPer >= 0 && fromPer <= 100))
                {
                    txtPerFrom.Text = string.Empty;
                    lblpopuperr.Text = "From Percentage Must Be Between 0 And 100";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
            else
            {
                txtPerFrom.Text = string.Empty;
                lblpopuperr.Text = "Please Enter Any From Value";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ToPercentage != "")
            {
                isValidDate = int.TryParse(ToPercentage, out toPer);
                if (!(toPer >= 0 && toPer <= 100))
                {
                    txtPerTo.Text = string.Empty;
                    lblpopuperr.Text = "To Percentage Must Be Between 0 And 100";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
            else
            {
                txtPerTo.Text = string.Empty;
                lblpopuperr.Text = "Please Enter Any To Value";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (fromPer > toPer)
            {
                txtPerTo.Text = string.Empty;
                txtPerFrom.Text = string.Empty;
                lblpopuperr.Text = "To Percentage Must Be Greater Than or Equal To From Percentage";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion TextBox Changed

    #region Button Events

    #region Go Click

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {

            btnPrint11();
            dicDate.Clear();
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            divMainContents.Visible = false;
            rptprint1.Visible = false;

            if (CheckSettings())
            {
                divMainContents.Visible = false;
                rptprint1.Visible = false;
                Showgrid.Visible = false;
                lblpopuperr.Text = "Plaese Select Day Wise Or Hour Wise!!! You Must Choose Any One of These Settings!!!";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            int startColumn = 5;

            int hourColumns = 0;
            int daysColumn = 0;


            bool isDayWise = false;
            bool isHourWise = false;
            bool isRowVisible = false;

            if (Convert.ToString(Session["hourwise"]).Trim() == "1")
            {
                isHourWise = true;
                hourColumns += startColumn;
                startColumn += 6;
            }

            if (Convert.ToString(Session["daywise"]).Trim() == "1")
            {
                isDayWise = true;
                daysColumn += startColumn;
                startColumn += 6;
            }

            DateTime dtFromDate = new DateTime();
            DateTime dtToDate = new DateTime();

            string fromDate = txtFromDate.Text.Trim();
            string toDate = txtToDate.Text.Trim();

            string fromPercentage = txtPerFrom.Text.Trim();// string.Empty;
            string toPecentage = txtPerTo.Text.Trim();//string.Empty;

            int fromPer = 0;
            int toPer = 0;

            int spreadHeight = 0;

            bool isValidDate = false;

            if (ddlCollege.Items.Count == 0)
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "School" : "College") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            else
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            if (ddlbatch.Items.Count == 0)
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Year" : "Batch") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            else
            {
                batch_year = Convert.ToString(ddlbatch.SelectedItem.Text).Trim();
            }
            if (ddldegree.Items.Count == 0)
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "School Type" : "Degree") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }

            if (ddlbranch.Items.Count == 0)
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Standard" : "Department") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            else
            {
                degree_code = Convert.ToString(ddlbranch.SelectedValue).Trim();
            }

            if (ddlsem.Items.Count == 0)
            {
                lblpopuperr.Text = "No " + ((isSchool) ? "Term" : " Semester") + " were Found";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            else
            {
                semester = Convert.ToString(ddlsem.SelectedItem.Text).Trim();
            }

            if (ddlsec.Items.Count > 0)
            {
                section = Convert.ToString(ddlsec.SelectedItem.Text).Trim();
                if (ddlsec.Enabled == false)
                {
                    qrysec = string.Empty;
                }
                else if (ddlsec.Items.Count > 0 && ddlsec.SelectedItem.Text.ToLower().Trim() != "all")
                {
                    qrysec = " and r.sections='" + section + "'";
                }
                else
                {
                    qrysec = string.Empty;
                }
            }
            else
            {
                qrysec = string.Empty;
            }
            if (fromDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
                if (!isValidDate)
                {
                    lblpopuperr.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
            else
            {
                lblpopuperr.Text = "Please Choose From Date";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            if (toDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
                if (!isValidDate)
                {
                    lblpopuperr.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
            else
            {
                lblpopuperr.Text = "Please Choose To Date";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            if (fromPercentage.Trim() != "")
            {
                isValidDate = false;
                isValidDate = int.TryParse(fromPercentage.Trim(), out fromPer);
                if (!isValidDate)
                {
                    lblpopuperr.Text = "From Percentage Is Invalid.Must Be Allows Numeric Only";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
                if (!(fromPer >= 0 && fromPer <= 100))
                {
                    lblpopuperr.Text = "From Percentage Must Be Between 0 And 100";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
            else
            {
                lblpopuperr.Text = "Please Enter From Percentage";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            if (toPecentage.Trim() != "")
            {
                isValidDate = false;
                isValidDate = int.TryParse(toPecentage.Trim(), out toPer);
                if (!isValidDate)
                {
                    lblpopuperr.Text = "To Percentage Is Invalid.Must Be Allows Numeric Only";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
                if (!(toPer >= 0 && toPer <= 100))
                {
                    lblpopuperr.Text = "To Percentage Must Be Between 0 And 100";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
            else
            {
                lblpopuperr.Text = "Please Enter To Percentage";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            if (fromPer <= toPer)
            {

            }
            else
            {
                lblpopuperr.Text = "From Percentage Is Lesser Than or Equal To The To Percentage";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            double mlconsider = 0;
            bool isMlValid = double.TryParse(txtMinPresentML.Text.Trim(), out mlconsider);
            if (string.IsNullOrEmpty(txtMinPresentML.Text.Trim()))
            {
                lblpopuperr.Text = "Please Enter Minimum Percentage For ML Consideration";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            if (!isMlValid)
            {
                lblpopuperr.Text = "Minimum Percentage For ML Consideration Must Be Numeric";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }
            string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
            string strorder = "";
            if (orderby_Setting == "0")
            {
                strorder = "ORDER BY roll_no";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "ORDER BY Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strorder = "ORDER BY Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "ORDER BY roll_no,Reg_No,Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "ORDER BY roll_no,Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "ORDER BY Reg_No,Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "ORDER BY roll_no,Stud_Name";
            }

            hat.Clear();
            hat.Add("degree_code", degree_code);
            hat.Add("sem_ester", int.Parse(semester));
            ds = d2.select_method("period_attnd_schedule", hat, "sp");
            if (ds.Tables[0].Rows.Count != 0)
            {
                NoHrs = int.Parse(ds.Tables[0].Rows[0]["PER DAY"].ToString());
                fnhrs = int.Parse(ds.Tables[0].Rows[0]["I_HALF_DAY"].ToString());
                minpresI = int.Parse(ds.Tables[0].Rows[0]["MIN PREE I DAY"].ToString());
                minpresII = int.Parse(ds.Tables[0].Rows[0]["MIN PREE II DAY"].ToString());
                minpresday = int.Parse(ds.Tables[0].Rows[0]["MIN PREE PER DAY"].ToString());
            }

            hat.Clear();
            hat.Add("colege_code", Session["collegecode"].ToString());
            ds1 = d2.select_method("ATT_MASTER_SETTING", hat, "sp");
            count = ds1.Tables[0].Rows.Count;
            qry = "select roll_no,reg_no,stud_name,stud_type,len(roll_no), convert(varchar(15),adm_date,103) as adm_date from registration r where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and cc='0' and delflag='0' and exam_flag<>'debar' and college_code='" + collegecode + "' " + qrysec + " " + strorder + "";

            ds.Reset();
            ds.Clear();
            ds.Dispose();
            ds = d2.select_method_wo_parameter(qry, "Text");
            bool gridVis = false;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Load_Grid_Header();
                DataRow drow;
                int sno = 0;
                for (int stud = 0; stud < ds.Tables[0].Rows.Count; stud++)
                {
                    string studentName = Convert.ToString(ds.Tables[0].Rows[stud]["stud_name"]).Trim();
                    string studentRollNo = Convert.ToString(ds.Tables[0].Rows[stud]["roll_no"]).Trim();
                    string studentRegNo = Convert.ToString(ds.Tables[0].Rows[stud]["reg_no"]).Trim();
                    string studentType = Convert.ToString(ds.Tables[0].Rows[stud]["stud_type"]).Trim();
                    string studentAdmitDate = Convert.ToString(ds.Tables[0].Rows[stud]["adm_date"]).Trim();

                    frdate = txtFromDate.Text;
                    todate = txtToDate.Text;
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
                    leavfinaeamount = 0;
                    medicalLeaveDays = 0;
                    medicalLeaveHours = 0;

                    persentmonthcal(collegecode, degree_code, semester, studentRollNo, studentAdmitDate);

                    Double absenthours = per_workingdays1 - per_per_hrs;

                    string dum_tage_date, dum_tage_hrs;
                    double per_tage_date = 0;// ((pre_present_date / per_workingdays) * 100);

                    if (per_workingdays > 0)
                    {
                        per_tage_date = ((pre_present_date / per_workingdays) * 100);
                    }
                    if (per_tage_date > 100)
                    {
                        per_tage_date = 100;
                    }

                    double per_tage_hrs = 0;// (((per_per_hrs) / (per_workingdays1)) * 100);

                    if (per_workingdays1 > 0)
                    {
                        per_tage_hrs = (((per_per_hrs) / (per_workingdays1)) * 100);
                    }

                    if (per_tage_hrs > 100)
                    {
                        per_tage_hrs = 100;
                    }

                    dum_tage_date = String.Format("{0:0,0.00}", float.Parse(per_tage_date.ToString()));

                    per_tage_hrs = Math.Round(per_tage_hrs, 2);
                    dum_tage_hrs = per_tage_hrs.ToString();
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


                    Double needmldays = per_workingdays * Convert.ToDouble(toPer) / 100;
                    Double acacldfays = needmldays;
                    needmldays = Math.Round(needmldays, 2, MidpointRounding.AwayFromZero);
                    //string[] stv = needmldays.ToString().Split('.');
                    //if (stv.GetUpperBound(0) == 1)
                    //{
                    //    int checj = Convert.ToInt32(stv[1].ToString());
                    //    if (checj != 50 && checj != 00)
                    //    {
                    //        int check = Convert.ToInt32(stv[1][0].ToString());
                    //        int valc = 50;
                    //        if (checj < 5)
                    //        {
                    //            valc = 100;
                    //        }
                    //        int neddpointdays = valc - checj;
                    //        needmldays = needmldays + neddpointdays;
                    //    }
                    //}
                    double avg = Math.Round((Convert.ToDouble(per_workingdays1) * Convert.ToDouble(toPer) / 100), 2, MidpointRounding.AwayFromZero);
                    double requiredHours = avg; //Math.Ceiling((per_workingdays1 * toPer / 100), 2, MidpointRounding.AwayFromZero);

                    double requireDays = Math.Round((per_workingdays * toPer / 100), 2, MidpointRounding.AwayFromZero);
                    //requireDays = Math.Ceiling(requireDays);
                    string requireValues = Convert.ToString(requireDays).Trim();
                    int dotIndex = requireValues.IndexOf('.');
                    string beforeDot = "";
                    string afterDot = "";
                    double beforeDecimal = 0;
                    double afterDecimal = 0;
                    double newRequiredDays = 0;
                    //if (dotIndex > 0)
                    //{
                    //    beforeDot = requireValues.Substring(0, dotIndex);
                    //    afterDot = requireValues.Substring(dotIndex + 1, requireValues.Length - (dotIndex + 1)).PadRight(2, '0');
                    //    double.TryParse(beforeDot, out beforeDecimal);
                    //    double.TryParse(afterDot, out afterDecimal);
                    //    newRequiredDays = beforeDecimal;
                    //    if (afterDecimal <= 50 && afterDecimal > 0)
                    //    {
                    //        newRequiredDays += 0.5;
                    //    }
                    //    else
                    //    {
                    //        newRequiredDays += 1;
                    //    }
                    //    requireDays = newRequiredDays;
                    //}
                    isRowVisible = false;
                    if (isDayWise)
                    {
                        if ((per_tage_date <= toPer && per_tage_date >= fromPer))
                        {
                            isRowVisible = true;
                        }
                    }
                    if (isHourWise)
                    {
                        if ((per_tage_hrs <= toPer && per_tage_hrs >= fromPer))
                        {
                            isRowVisible = true;
                        }
                    }

                    if (isRowVisible)
                    {
                        sno++;
                        drow = data.NewRow();
                        data.Rows.Add(drow);
                        gridVis = true;
                        int col = 0;

                        data.Rows[data.Rows.Count - 1][col] = Convert.ToString(sno);


                        if (Convert.ToString(Session["Rollflag"]) == "1")
                        {
                            col++;
                            data.Rows[data.Rows.Count - 1][col] = Convert.ToString(studentRollNo);
                        }

                        if (Convert.ToString(Session["Regflag"]) == "1")
                        {
                            col++;
                            data.Rows[data.Rows.Count - 1][col] = Convert.ToString(studentRegNo);
                        }
                        col++;
                        data.Rows[data.Rows.Count - 1][col] = Convert.ToString(studentName);
                        if (Convert.ToString(Session["Studflag"]) == "1")
                        {
                            col++;
                            data.Rows[data.Rows.Count - 1][col] = Convert.ToString(studentType);
                        }
                        if (isHourWise)
                        {
                           
                            col++;
                            data.Rows[data.Rows.Count - 1][col] = Convert.ToString(per_workingdays1);
                            col++;
                            data.Rows[data.Rows.Count - 1][col] = Convert.ToString(per_per_hrs);
                            col++;
                            data.Rows[data.Rows.Count - 1][col] = Convert.ToString(dum_tage_hrs);
                            col++;
                            data.Rows[data.Rows.Count - 1][col] = Convert.ToString(medicalLeaveHours);
                            Double needHours = requiredHours - per_per_hrs;
                            needHours = Math.Ceiling(needHours);
                            double medicalLeaveAverage = 0;
                            double.TryParse(Convert.ToString(dum_tage_hrs).Trim(), out medicalLeaveAverage);
                            if (needHours <= medicalLeaveHours && needHours > 0 && mlconsider <= medicalLeaveAverage)
                            {
                                col++;
                                data.Rows[data.Rows.Count - 1][col] = needHours.ToString();

                                if (per_workingdays1 > 0)
                                {
                                    medicalLeaveAverage = Math.Round((((per_per_hrs + needHours) / per_workingdays1) * 100), 2, MidpointRounding.AwayFromZero);

                                }
                            }
                            else
                            {
                                col++;
                                data.Rows[data.Rows.Count - 1][col] = "0";
                            }

                            col++;
                            string newMLPercentage = String.Format("{0:0,0.00}", float.Parse(Convert.ToString(medicalLeaveAverage).Trim()));
                            data.Rows[data.Rows.Count - 1][col] = Convert.ToString(newMLPercentage).Trim();


                        }

                        if (isDayWise)
                        {
                            col++;
                            data.Rows[data.Rows.Count - 1][col] = Convert.ToString(per_workingdays).Trim();
                            col++;
                            data.Rows[data.Rows.Count - 1][col] = Convert.ToString(pre_present_date).Trim();
                            col++;
                            data.Rows[data.Rows.Count - 1][col] = Convert.ToString(dum_tage_date).Trim();
                            col++;
                            data.Rows[data.Rows.Count - 1][col] = Convert.ToString(medicalLeaveDays).Trim();
                            Double needdays = requireDays - pre_present_date;
                            needdays = Math.Ceiling(needdays);
                            double medicalLeaveAverage = 0;
                            double.TryParse(Convert.ToString(dum_tage_date).Trim(), out medicalLeaveAverage);
                            if (needdays <= medicalLeaveDays && needdays > 0 && mlconsider <= medicalLeaveAverage)
                            {
                                col++;
                                data.Rows[data.Rows.Count - 1][col] = Convert.ToString(needdays).Trim();

                                if (per_workingdays > 0)
                                {
                                    medicalLeaveAverage = Math.Round((((pre_present_date + needdays) / per_workingdays) * 100), 2, MidpointRounding.AwayFromZero);
                                }
                            }
                            else
                            {
                                col++;
                                data.Rows[data.Rows.Count - 1][col] = "0";
                            }


                            string newMLPercentage = String.Format("{0:0,0.00}", float.Parse(Convert.ToString(medicalLeaveAverage).Trim()));
                            col++;
                            data.Rows[data.Rows.Count - 1][col] = Convert.ToString(newMLPercentage).Trim();

                        }
                        col++;
                        data.Rows[data.Rows.Count - 1][col] = Convert.ToString(0).Trim();
                        col++;
                        data.Rows[data.Rows.Count - 1][col] = Convert.ToString("A/NA").Trim();

                    }
                }

                if (data.Rows.Count > 0 && gridVis)
                {

                    Showgrid.DataSource = data;
                    Showgrid.DataBind();
                    Showgrid.Visible = true;

                    int rowcnt = Showgrid.Rows.Count - 3;
                    //Rowspan
                    for (int rowIndex = Showgrid.Rows.Count - rowcnt - 1; rowIndex >= 0; rowIndex--)
                    {
                        GridViewRow row = Showgrid.Rows[rowIndex];
                        GridViewRow previousRow = Showgrid.Rows[rowIndex + 1];
                        Showgrid.Rows[rowIndex].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        Showgrid.Rows[rowIndex].Font.Bold = true;
                        Showgrid.Rows[rowIndex].HorizontalAlign = HorizontalAlign.Center;

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
                    //ColumnSpan
                    for (int rowIndex = Showgrid.Rows.Count - rowcnt - 1; rowIndex >= 0; rowIndex--)
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


                divMainContents.Visible = true;
                rptprint1.Visible = true;
                if (Showgrid.Rows.Count == 0 || gridVis==false)
                {
                    Showgrid.Visible = false;
                    divMainContents.Visible = false;
                    rptprint1.Visible = false;
                    lblpopuperr.Text = "No Record(s) Were Found.";
                    lblpopuperr.Visible = true;
                    popupdiv.Visible = true;
                    return;
                }
            }
            else
            {
                lblpopuperr.Text = "No Record(s) Were Found.";
                lblpopuperr.Visible = true;
                popupdiv.Visible = true;
                return;
            }


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
            string alertRights = d2.GetFunction("select  value from Master_Settings where settings='AlertMessageForAttendance' " + qryUserCodeOrGroupCode + "");
            // string Noresult = UnmarkHours;

            if (alertRights == "1")
            {
                if (dicDate.Count > 0)
                {
                    allResults.Add(dicDate);
                    string valDate = string.Empty;
                    string valTime = string.Empty;
                    string Noresult = string.Empty;
                    foreach (KeyValuePair<string, string> dt in dicDate)
                    {
                        valDate = dt.Key;
                        valTime = dt.Value;
                        Noresult = Noresult + "Date: " + " " + valDate + " " + "Hour: " + "" + valTime + " ";
                    }
                    //Noresult = +Noresult;
                    //string Noresult = UnmarkHours;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = Noresult + " " + " Attendance not Found";
                    divPopAlert.Visible = true;
                    return;
                }

            }
        }

        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Go Click

    #region Print PDF

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            //Session["column_header_row_count"] = Convert.ToString(FpEntry.ColumnHeader.RowCount);
            string sections = string.Empty;// ddlsec.SelectedValue.ToString();
            if (ddlsec.Items.Count > 0)
            {
                sections = Convert.ToString(ddlsec.SelectedValue).Trim();
                if (sections.Trim().ToLower() == "all" || sections.Trim() == string.Empty || sections.Trim() == "-1")
                {
                    sections = "";
                }
                else
                {
                    sections = " - Sec-" + sections;
                }
            }
            string ss = null;
            string degreedetails = "Attendance Shortage Details - Report" + '@' + "Degree: " + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '-' + ddlbranch.SelectedItem.ToString() + '-' + "Sem-" + ddlsem.SelectedItem.ToString() + sections + '@' + "Period :" + txtFromDate.Text.ToString() + " To " + txtToDate.Text.ToString();
            string pagename = "Attendance_shortageNew.aspx";
            Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
            Printcontrol.Visible = true;

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Print PDF

    #region Print Excel

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            lblpopuperr.Text = string.Empty;
            Printcontrol.Visible = false;
            string reportname = txtexcelname1.Text;
            if (Convert.ToString(reportname).Trim() != "")
            {
                d2.printexcelreportgrid(Showgrid, reportname);

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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }


    #endregion Print PDF

    #region Popup Close

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblpopuperr.Text = string.Empty;
            popupdiv.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Popup Close

    public void persentmonthcal(string collegeCode, string degree, string sem, string rollno, string admitDate)
    {
        medicalLeaveCountPerSession = 0;
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
                    if (holiday_table21.ContainsKey(dummy_split))   //added by Mullai
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
                                    per_leavehrs = 0;
                                    if (split_holiday_status_1 == "1")
                                    {
                                        for (i = 1; i <= fnhrs; i++)
                                        {
                                            CurrentDate = dumm_from_date.ToString("dd/MM/yyyy");
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
                                            if (value == "0" || value == "" || value == null)
                                            {
                                                if (!dicDate.ContainsKey(CurrentDate) && !dicDate.ContainsValue(i.ToString()))
                                                {
                                                    dicDate.Add(CurrentDate, i.ToString());
                                                }

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
                                        if (medicalLeaveCountPerSession > 0)
                                        {
                                            if (medicalLeaveCountPerSession >= minpresI)
                                            {
                                                medicalLeaveDays = medicalLeaveDays + 0.5;
                                            }
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

                                            if (value == "0" || value == "" || value == null)
                                            {
                                                if (!dicDate.ContainsKey(CurrentDate) && !dicDate.ContainsValue(i.ToString()))
                                                {
                                                    dicDate.Add(CurrentDate, i.ToString());
                                                }
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
                                        if (medicalLeaveCountPerSession > 0)//Rajkumar 23/12/2017
                                        {
                                            if (medicalLeaveCountPerSession + njhr >= minpresII)
                                            {
                                                medicalLeaveDays = medicalLeaveDays + 0.5;
                                            }
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
        //if (dicDate.Count > 0)
        //{

        //    allResults.Add(dicDate);
        //    string valDate = string.Empty;
        //    string valTime = string.Empty;
        //    string Noresult = string.Empty;
        //    foreach (KeyValuePair<string, string> dt in dicDate)
        //    {
        //        valDate = dt.Key;
        //        valTime = dt.Value;
        //        Noresult = Noresult + "Date: " + " " + valDate + " " + "Hour: " + "" + valTime + " ";
        //    }
        //    //Noresult = +Noresult;
        //    //string Noresult = UnmarkHours;
        //    lblAlertMsg.Visible = true;
        //    lblAlertMsg.Text = Noresult + " " + " Attendance not Found";
        //    divPopAlert.Visible = true;
        //    return;

        //}

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

    #endregion Button Events

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
        spReportName.InnerHtml = "Attendance Shortage Details - Regulation Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


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
                for (int j = colcnt; j < data.Columns.Count; j++)
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }


        }
        catch
        {


        }

    }

    public bool CheckSettings()
    {
        try
        {
            bool isValid = false;
            if (Session["daywise"] != null && Session["hourwise"] != null)
            {
                if (Session["daywise"] == "0" && Session["hourwise"] == "0")
                {
                    isValid = true;
                    return isValid;
                }
            }
            else
            {
                Response.Redirect("~/Attendance_shortageNew.aspx");
            }
            return isValid;
        }
        catch (Exception ex)
        {
            return false;
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

}
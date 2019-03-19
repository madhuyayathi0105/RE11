using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Text;
public partial class AttendanceSubSiteMaster : System.Web.UI.MasterPage
{
    DAccess2 da = new DAccess2();
    static string grouporusercode = string.Empty;
    string sql = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //string strPreviousPage =string.Empty;
        //if (Request.UrlReferrer != null)
        //{
        //    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
        //}
        //if (strPreviousPage == "")
        //{
        //    Session["IsLogin"] = "0";
        //    Response.Redirect("~/Default.aspx");
        //}
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        string group_code = Convert.ToString(Session["group_code"]);
        if (group_code.Contains(";"))
        {
            string[] group_semi = group_code.Split(';');
            group_code = group_semi[0].ToString();
        }
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            grouporusercode = " group_code=" + group_code + "";
        else
            grouporusercode = " user_code=" + Session["usercode"].ToString().Trim() + "";
        string collegecode = Session["Collegecode"].ToString();
        string collegeName = da.GetFunction("select collname from collinfo where  college_code='" + collegecode + "'");
        if (da.GetFunction("select LinkValue from New_InsSettings where LinkName='UseCommonCollegeCode' and user_code ='" + Session["UserCode"].ToString() + "'") == "1")
        {
            string comCOde = da.GetFunction("select com_name from collinfo where  college_code='" + collegecode + "'").Trim();
            collegeName = (comCOde.Length > 1) ? comCOde : collegeName;
        }
        lblcolname.Text = collegeName;
        //lblcolname.Text = da.GetFunction("select collname from collinfo where  college_code='" + collegecode + "'");
        string color = da.GetFunction("select Farvour_color from user_color where user_code='" + Session["UserCode"].ToString() + "' and college_code='" + collegecode + "'");
        string colornew = string.Empty;
        if (color.Trim() == "0")
        {
            colornew = "#06d995";
        }
        else
        {
            colornew = color;
            //prewcolor.Attributes.Add("style", "background-color:" + colornew + ";");
        }
        if (!IsPostBack)
        {
            MainDivIdValue.Attributes.Add("style", "background-color:" + colornew + ";border-bottom: 6px solid lightyellow; box-shadow: 0 0 11px -4px; height: 58px; left: 0; position: fixed; z-index: 2; top: 0; width: 100%;");
            Session["StafforAdmin"] = string.Empty;
            if (Convert.ToString(Session["Staff_Code"]) != "")
            {
                Session["StafforAdmin"] = "Staff";
                img_stfphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + Session["Staff_Code"];
                imgstdphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + Session["Staff_Code"];
                string stfdescode = string.Empty;
                sql = "select desig_code from stafftrans where staff_code='" + Convert.ToString(Session["Staff_Code"]) + "' and latestrec=1";
                stfdescode = da.GetFunction(sql);
                if (stfdescode != "" && stfdescode != null)
                {
                    string stfdesigname = string.Empty;
                    sql = "select dm.desig_name from desig_master dm where dm.desig_code='" + stfdescode.ToString() + "' and collegecode=" + Session["collegecode"].ToString();
                    stfdesigname = da.GetFunction(sql);
                    string staffname = string.Empty;
                    sql = "select staff_name from staffmaster where staff_code='" + Session["staff_code"] + "'";
                    staffname = da.GetFunction(sql);
                    string deptname = string.Empty;
                    sql = "select dt.dept_acronym from Department dt,stafftrans st where dt.Dept_code=st.dept_code and staff_code='" + Session["staff_code"] + "' and latestrec=1";
                    deptname = da.GetFunction(sql);
                    lbslstaffname.Text = Convert.ToString(staffname);
                    lbldesignation.Text = Convert.ToString(stfdesigname);
                    lbldept.Text = Convert.ToString(deptname);
                }
            }
            else
            {
                Session["StafforAdmin"] = "Admin";
                string staffname = string.Empty;
                sql = "select full_name from usermaster where user_code='" + Session["UserCode"] + "'";
                staffname = da.GetFunction(sql);
                lbslstaffname.Text = Convert.ToString(staffname);
            }
        }
        try
        {
            EntryCheck();
            DataSet dsRights = new DataSet();
            DataTable dtOutput = new DataTable();
            DataView dvnew = new DataView();
            string SelQ = string.Empty;
            SelQ = "  select distinct HeaderName from Security_Rights_Details where Rights_Code in(select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Attendance'";
            SelQ = SelQ + " select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Attendance'  order by HeaderPriority, PagePriority asc";
            dsRights = da.select_method_wo_parameter(SelQ, "Text");
            if (dsRights.Tables.Count > 0 && dsRights.Tables[0].Rows.Count > 0 && dsRights.Tables[1].Rows.Count > 0)
            {
                dsRights.Tables[1].DefaultView.RowFilter = " HeaderName='Master'";
                dvnew = dsRights.Tables[1].DefaultView;
                if (dvnew.Count > 0)
                {
                    MasterList.Visible = true;
                    for (int tab1 = 0; tab1 < dvnew.Count; tab1++)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        tabs1.Controls.Add(li);
                        tabs1.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 0px 15px 0px 15px;");
                        HtmlGenericControl anchor = new HtmlGenericControl("a");
                        anchor.Attributes.Add("target", "_blank");
                        anchor.Attributes.Add("href", Convert.ToString(dvnew[tab1]["PageName"]));
                        anchor.InnerText = Convert.ToString(dvnew[tab1]["ReportName"]);
                        li.Controls.Add(anchor);
                    }
                }
                else
                    MasterList.Visible = false;
                dsRights.Tables[1].DefaultView.RowFilter = " HeaderName='Operation'";
                dvnew = dsRights.Tables[1].DefaultView;
                if (dvnew.Count > 0)
                {
                    OperationList.Visible = true;
                    for (int tab2 = 0; tab2 < dvnew.Count; tab2++)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        tabs2.Controls.Add(li);
                        tabs2.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 0px 15px 0px 15px;");
                        HtmlGenericControl anchor = new HtmlGenericControl("a");
                        anchor.Attributes.Add("target", "_blank");
                        anchor.Attributes.Add("href", Convert.ToString(dvnew[tab2]["PageName"]));
                        anchor.InnerText = Convert.ToString(dvnew[tab2]["ReportName"]);
                        li.Controls.Add(anchor);
                    }
                }
                else
                    OperationList.Visible = false;
                dsRights.Tables[1].DefaultView.RowFilter = " HeaderName='Report'";
                dvnew = dsRights.Tables[1].DefaultView;
                if (dvnew.Count > 0)
                {
                    ReportList.Visible = true;
                    for (int tab3 = 0; tab3 < dvnew.Count; tab3++)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        tabs3.Controls.Add(li);
                        if (dvnew.Count <= 10)
                            tabs3.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 0px 15px 0px 15px;");
                        else if (dvnew.Count > 10)
                            tabs3.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 0px 15px 0px 15px; height:450px;");
                        HtmlGenericControl anchor = new HtmlGenericControl("a");
                        anchor.Attributes.Add("target", "_blank");
                        anchor.Attributes.Add("href", Convert.ToString(dvnew[tab3]["PageName"]));
                        anchor.InnerText = Convert.ToString(dvnew[tab3]["ReportName"]);
                        li.Controls.Add(anchor);
                    }
                }
                else
                    ReportList.Visible = false;
                dsRights.Tables[1].DefaultView.RowFilter = " HeaderName='Chart'";
                dvnew = dsRights.Tables[1].DefaultView;
                if (dvnew.Count > 0)
                {
                    ChartList.Visible = true;
                    for (int tab4 = 0; tab4 < dvnew.Count; tab4++)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        tabs4.Controls.Add(li);
                        HtmlGenericControl anchor = new HtmlGenericControl("a");
                        anchor.Attributes.Add("target", "_blank");
                        anchor.Attributes.Add("href", Convert.ToString(dvnew[tab4]["PageName"]));
                        anchor.InnerText = Convert.ToString(dvnew[tab4]["ReportName"]);
                        li.Controls.Add(anchor);
                    }
                }
                else
                    ChartList.Visible = false;
            }
        }
        catch { }
        LiteralControl ltr = new LiteralControl();
        ltr.Text = "<style type=\"text/css\" rel=\"stylesheet\">" +
                    @"#showmenupages .has-sub ul li:hover a
                                                {
color:lightyellow;
                                                    background-color:" + colornew + @";
                                                }
#showmenupages .has-sub ul li a
        {
border-bottom: 1px dotted " + colornew + @";
}
ul li
{
  border-bottom: 1px dotted " + colornew + @";
            border-right: 1px dotted " + colornew + @";
}
ul li:hover
        {
color:lightyellow;
 background-color:" + colornew + @";
border-radius:5px;
}
a:hover
        {
color:lightyellow;
}
                                                </style>
                                                ";
        this.Page.Header.Controls.Add(ltr);
    }

    private void EntryCheck()
    {
        DataTable dtRights = BuildTable();
        try
        {
            for (int row = 0; row < dtRights.Rows.Count; row++)
            {
                StringBuilder sbQuery = new StringBuilder();
                string rightsCode = Convert.ToString(dtRights.Rows[row]["RightsCode"]);
                sbQuery.Append("IF Exists (select Rights_Code from Security_Rights_Details where  Rights_Code ='" + rightsCode + "' AND ModuleName='" + Convert.ToString(dtRights.Rows[row]["Module"]) + "') Update Security_Rights_Details set ModuleName ='" + Convert.ToString(dtRights.Rows[row]["Module"]) + "',HeaderName='" + Convert.ToString(dtRights.Rows[row]["Header"]) + "' ,ReportId='" + Convert.ToString(dtRights.Rows[row]["ReportId"]) + "' ,ReportName='" + Convert.ToString(dtRights.Rows[row]["ReportName"]) + "' ,PageName='" + Convert.ToString(dtRights.Rows[row]["PageName"]) + "' ,HelpURL='" + Convert.ToString(dtRights.Rows[row]["HelpPage"]) + "' ,PagePriority='" + Convert.ToString(dtRights.Rows[row]["PagePriority"]) + "' ,HeaderPriority='" + Convert.ToString(dtRights.Rows[row]["HeaderPriority"]) + "' where Rights_Code ='" + rightsCode + "' AND ModuleName='" + Convert.ToString(dtRights.Rows[row]["Module"]) + "' ELSE insert into Security_Rights_Details (ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL ,PagePriority ,HeaderPriority ) values ('" + Convert.ToString(dtRights.Rows[row]["Module"]) + "','" + Convert.ToString(dtRights.Rows[row]["Header"]) + "','" + rightsCode + "','" + Convert.ToString(dtRights.Rows[row]["ReportId"]) + "','" + Convert.ToString(dtRights.Rows[row]["ReportName"]) + "','" + Convert.ToString(dtRights.Rows[row]["PageName"]) + "','" + Convert.ToString(dtRights.Rows[row]["HelpPage"]) + "','" + Convert.ToString(dtRights.Rows[row]["PagePriority"]) + "','" + Convert.ToString(dtRights.Rows[row]["HeaderPriority"]) + "')");
                da.update_method_wo_parameter(sbQuery.ToString(), "Text");
            }
        }
        catch { }
    }

    private DataTable BuildTable()
    {
        DataTable dtRights = new DataTable();
        dtRights.Columns.Add("RightsCode");
        dtRights.Columns.Add("Module");
        dtRights.Columns.Add("Header");
        dtRights.Columns.Add("ReportId");
        dtRights.Columns.Add("ReportName");
        dtRights.Columns.Add("PageName");
        dtRights.Columns.Add("HelpPage");
        dtRights.Columns.Add("PagePriority");
        dtRights.Columns.Add("HeaderPriority");
        string saveCOl = da.GetFunction("select LinkValue from inssettings where College_code ='" + Session["collegecode"].ToString() + "' and LinkName ='Individual Staff Login Attendance New'").Trim();
        //Master
        Session["StafforAdmin"] = string.Empty;
        Session["clearschedulesession"] = "clear";
        string linkpath = string.Empty;
        string TimeTablepath = string.Empty;
        string BatchAllocationPath = string.Empty;
        string SubjectWiseAttendReport11 = string.Empty;
        string SubjectWiseAttendReport36 = string.Empty;
        if (Convert.ToString(Session["Staff_Code"]) != "")
        {
            if (saveCOl == "1")
            {
                linkpath = "NewStaffAttendance.aspx";
                TimeTablepath = "NewStudentTimeTable.aspx";
                BatchAllocationPath = "NewBatchschedular.aspx";
                SubjectWiseAttendReport11 = "singlesubject_wise_attendanceNew.aspx";
                SubjectWiseAttendReport36 = "SubjectWiseAttendanceNewReport.aspx";
            }
            else
            {
                linkpath = "newstaf.aspx";
                TimeTablepath = "StudentTimeTable.aspx";
                BatchAllocationPath = "Batchschedular.aspx";
                SubjectWiseAttendReport11 = "singlesubject_wise_attendance.aspx";
                SubjectWiseAttendReport36 = "SubjectWiseAttendanceReport.aspx";
            }
        }
        else
        {
            linkpath = "newadmin.aspx";
            if (saveCOl == "1")
            {
                TimeTablepath = "NewStudentTimeTable.aspx";
                BatchAllocationPath = "NewBatchschedular.aspx";
                SubjectWiseAttendReport11 = "singlesubject_wise_attendanceNew.aspx";
                SubjectWiseAttendReport36 = "SubjectWiseAttendanceNewReport.aspx";
            }
            else
            {
                TimeTablepath = "StudentTimeTable.aspx";
                BatchAllocationPath = "Batchschedular.aspx";
                SubjectWiseAttendReport11 = "singlesubject_wise_attendance.aspx";
                SubjectWiseAttendReport36 = "SubjectWiseAttendanceReport.aspx";
            }
        }

        #region Master

        dtRights.Rows.Add(719, "Attendance", "Master", "AM101", "Syallabus Entry", "Syllabus_Entry.aspx", "HelpUrl.html", 1, 1);
        dtRights.Rows.Add(727, "Attendance", "Master", "AM102", "Master Subject Allotments", "MatserSubjectChooser.aspx", "HelpUrl.html", 2, 1);
        dtRights.Rows.Add(729, "Attendance", "Master", "AM111", "Subject Allotments - A", "SubjectAllotment.aspx", "HelpUrl.html", 11, 1);
        dtRights.Rows.Add(713, "Attendance", "Master", "AM103", "Staff Selector", "Subjectschedularpage.aspx", "HelpUrl.html", 3, 1);
        dtRights.Rows.Add(717, "Attendance", "Master", "AM104", "Semester Time Table", TimeTablepath, "HelpUrl.html", 4, 1);
        dtRights.Rows.Add(714, "Attendance", "Master", "AM105", "Batch Allocation", BatchAllocationPath, "HelpUrl.html", 5, 1);
        dtRights.Rows.Add(715, "Attendance", "Master", "AM106", "Lesson Planner", "Lesson_Planner.aspx", "HelpUrl.html", 6, 1);
        dtRights.Rows.Add(723, "Attendance", "Master", "AM107", "Online Elective Subject Type Settings", "Foundationsubjectselection.aspx", "HelpUrl.html", 7, 1);
        dtRights.Rows.Add(724, "Attendance", "Master", "AM108", "Special Hour Batch Allocation", "SplhourBatchAllocation.aspx", "HelpUrl.html", 8, 1);
        dtRights.Rows.Add(721, "Attendance", "Master", "AM109", "Master Time Table", "MasterTimeTable.aspx", "HelpUrl.html", 9, 1);
        dtRights.Rows.Add(728, "Attendance", "Master", "AM110", "Subject wise Batch Allocation", "SubjectWiseBatchAllocation.aspx", "HelpUrl.html", 10, 1);

        #endregion Master

        //Operation
        dtRights.Rows.Add("1001", "Attendance", "Operation", "AO01", "Attendance Entry", linkpath, "HelpPage.Html", "1", "2");
        dtRights.Rows.Add("100001", "Attendance", "Operation", "AO02", "OD Entry", "StudentOndutydetails.aspx", "HelpPage.Html", "2", "2");
        dtRights.Rows.Add("100002", "Attendance", "Operation", "AO03", "Special Hour Attendance", "student_special_hours_attendance.aspx", "HelpPage.Html", "3", "2");
        dtRights.Rows.Add("100003", "Attendance", "Operation", "AO04", "Student Conduct", "StudentConduct.aspx", "HelpPage.Html", "4", "2");
        dtRights.Rows.Add("100004", "Attendance", "Operation", "AO05", "Staff Attendance Entry With Out Timetable", "staffattendanceentry.aspx", "HelpPage.Html", "5", "2");
        dtRights.Rows.Add("100005", "Attendance", "Operation", "AO06", "All Student Attendance", "AllStudentAttendance1.aspx", "HelpPage.Html", "6", "2");
        dtRights.Rows.Add("100009", "Attendance", "Operation", "AO07", "Student Attendance New", "SchoolStudentAttendance.aspx", "HelpPage.Html", "7", "2");
        dtRights.Rows.Add("100006", "Attendance", "Operation", "AO08", "Attendance Entry New", "newadminnew.aspx", "HelpPage.Html", "8", "2");
        dtRights.Rows.Add("100007", "Attendance", "Operation", "AO09", "Student Condonation", "condonation.aspx", "HelpPage.Html", "9", "2");
        dtRights.Rows.Add("100008", "Attendance", "Operation", "AO10", "Special Day/Free Hour Entry", "AttendanceSplDayFreeHrEntry.aspx", "HelpPage.Html", "10", "2");
        dtRights.Rows.Add(722, "Attendance", "Operation", "AO11", "Considered Dayorder Change", "ConsiderdDayorderchanged.aspx", "HelpUrl.html", 11, 2);
        dtRights.Rows.Add("100010", "Attendance", "Operation", "AO12", "Special Hour Allotment", "Student_special_Hour_Entry.aspx", "HelpUrl.html", "12", "2");
        dtRights.Rows.Add("100011", "Attendance", "Operation", "AO13", "Special Hour Student Selection", "StudentEntryForSpecialClass.aspx", "HelpUrl.html", "13", "2");
        dtRights.Rows.Add("100012", "Attendance", "Operation", "AO14", "Student Subject Allotment Conversion", "StudentSubjectAllotmentConversion.aspx", "HelpUrl.html", "14", "2");
        //kowshika 07.04.2018
        dtRights.Rows.Add("100015", "Attendance", "Operation", "AO15", "Class Section Wise Master Settings", "ClassSectionWiseMasterSettings.aspx", "HelpUrl.html", "15", "2");
        dtRights.Rows.Add("100016", "Attendance", "Operation", "AO16", "Elective Subject Student Count", "Elective Subject Student Count.aspx", "HelpUrl.html", "16", "2");
        dtRights.Rows.Add("100017", "Attendance", "Operation", "AO17", "TTSelection Settings", "TTSelectionSettings.aspx", "HelpUrl.html", "17", "2");
        //magesh 19.7.18
        dtRights.Rows.Add("100018", "Attendance", "Operation", "AO18", "Late Entry", "Late Attendance.aspx", "HelpUrl.html", "18", "2");
        dtRights.Rows.Add("100020", "Attendance", "Operation", "AO19", "Subject Room Allotment", "Subject Room Allotement.aspx", "HelpUrl.html", "19", "2");
        //Reports
        dtRights.Rows.Add("1002", "Attendance", "Report", "AT01", "Hourwise / Daywise Absentees Report", "AbsenteeRt.aspx", "HelpPage.Html", "1", "3");
        dtRights.Rows.Add("1003", "Attendance", "Report", "AT02", "Cumulative Attendance Report", "cumreport.aspx", "HelpPage.Html", "2", "3");
        dtRights.Rows.Add("1004", "Attendance", "Report", "AT03", "Monthly Student Attendance Report", "monthattndreport.aspx", "HelpPage.Html", "3", "3");
        dtRights.Rows.Add("1006", "Attendance", "Report", "AT04", "Overall Attendance Report Per Day", "Ovrall_Attreport_perday.aspx", "HelpPage.Html", "4", "3");
        dtRights.Rows.Add("1007", "Attendance", "Report", "AT05", "Consolidated Student Attendance Report.", "consolidatestudreport.aspx", "HelpPage.Html", "5", "3");
        dtRights.Rows.Add("1008", "Attendance", "Report", "AT06", "Overall Daily Attendance Report", "overalldailyattndreport.aspx", "HelpPage.Html", "6", "3");
        dtRights.Rows.Add("1009", "Attendance", "Report", "AT07", "Subjectwise Attendance With Percentage Report", "subjwiseattndreport.aspx", "HelpPage.Html", "7", "3");
        dtRights.Rows.Add("1010", "Attendance", "Report", "AT08", "Attendance Fine Report", "attnd_fine_report.aspx", "HelpPage.Html", "8", "3");
        dtRights.Rows.Add("1011", "Attendance", "Report", "AT09", "Individual Student Attendance", "dailystudentattndreport.aspx", "HelpPage.Html", "9", "3");
        dtRights.Rows.Add("1012", "Attendance", "Report", "AT10", "Congratulations Report", "congrats.aspx", "HelpPage.Html", "10", "3");
        dtRights.Rows.Add("1013", "Attendance", "Report", "AT11", "Individual Subject Wise Attendance Report", SubjectWiseAttendReport11, "HelpPage.Html", "11", "3");
        dtRights.Rows.Add("1014", "Attendance", "Report", "AT12", "Consolidate SubjectWise Attendance Report", "consolidate_subjwise_attndreport.aspx", "HelpPage.Html", "12", "3");
        dtRights.Rows.Add("1015", "Attendance", "Report", "AT13", "Subject Wise Attendance Details – Splitup Report", "singlesubjectwise_splitup_attnd_report.aspx", "HelpPage.Html", "13", "3");
        dtRights.Rows.Add("1016", "Attendance", "Report", "AT14", "Overall Attendance Details -Splitup Report", "Attendance_overall.aspx", "HelpPage.Html", "14", "3");
        dtRights.Rows.Add("1017", "Attendance", "Report", "AT15", "Attendance Shortage Details - Regulation Report", "Attendance_shortageNew.aspx", "HelpPage.Html", "15", "3");
        dtRights.Rows.Add("1018", "Attendance", "Report", "AT16", "Overall Attendance Percentage Report", "attendance.aspx", "HelpPage.Html", "16", "3");
        dtRights.Rows.Add("1019", "Attendance", "Report", "AT17", "Overall Percentagewise Attendance Report", "Overall_PercentageWise_Attnd.aspx", "HelpPage.Html", "17", "3");
        dtRights.Rows.Add("1020", "Attendance", "Report", "AT18", "Special Hour Report", "Specialhourreport.aspx", "HelpPage.Html", "18", "3");
        dtRights.Rows.Add("1021", "Attendance", "Report", "AT19", "Student Weekly Report", "StudentWeeklyAttendance.aspx", "HelpPage.Html", "19", "3");
        dtRights.Rows.Add("1022", "Attendance", "Report", "AT20", "Daily Entry and Lesson Planner Report", "DailyEntry LessonPlanner Report.aspx", "HelpPage.Html", "20", "3");
        dtRights.Rows.Add("1023", "Attendance", "Report", "AT21", "Attendance Report", "AttendanceReport_New.aspx", "HelpPage.Html", "21", "3");
        dtRights.Rows.Add("1024", "Attendance", "Report", "AT22", "Attendance With Reason", "AttendanceReason.aspx", "HelpPage.Html", "22", "3");
        dtRights.Rows.Add("1025", "Attendance", "Report", "AT23", "Subject Notes", "notes.aspx", "HelpPage.Html", "23", "3");
        dtRights.Rows.Add("1026", "Attendance", "Report", "AT24", "Day Wise Staff Task Performance Report", "dailystaffreport.aspx", "HelpPage.Html", "24", "3");
        dtRights.Rows.Add("1027", "Attendance", "Report", "AT25", "Student Letter Report", "attandanceletterfmt.aspx", "HelpPage.Html", "25", "3");
        dtRights.Rows.Add("1028", "Attendance", "Report", "AT26", "Absentees Report", "Periodwiseattendancereport.aspx", "HelpPage.Html", "26", "3");
        dtRights.Rows.Add("1029", "Attendance", "Report", "AT27", "Department & Period Wise Attendance Report", "DepartmentWiseAttendanceReport.aspx", "HelpPage.Html", "27", "3");
        dtRights.Rows.Add("1030", "Attendance", "Report", "AT28", "Consolidated Cumulative Attendance Report", "ConsoliatedCumulative_AttnReport.aspx", "HelpPage.Html", "28", "3");
        dtRights.Rows.Add("1031", "Attendance", "Report", "AT29", "Consolidated Absentees Report", "SubjectWiseAbsenteesReport.aspx", "HelpPage.Html", "29", "3");
        dtRights.Rows.Add("1032", "Attendance", "Report", "AT30", "Attendance Period Master Settings", "AttendancePeriod_Master_Settings_New.aspx", "HelpPage.Html", "30", "3");
        dtRights.Rows.Add("1033", "Attendance", "Report", "AT31", "Absentees Report of Board", "StudentPeriodwiseAttendanceDetails.aspx", "HelpPage.Html", "31", "3");
        dtRights.Rows.Add("1034", "Attendance", "Report", "AT32", "Day Wise Abseentees SMS", "Day_Wise_Absentees_sms.aspx", "HelpPage.Html", "32", "3");
        dtRights.Rows.Add("1035", "Attendance", "Report", "AT33", "Attendance Report PerDay", "attreport_perday.aspx", "HelpPage.Html", "33", "3");
        dtRights.Rows.Add("1036", "Attendance", "Report", "AT34", "School Attendance Report", "SchoolAttendanceReport.aspx", "HelpPage.Html", "34", "3");
        dtRights.Rows.Add("1037", "Attendance", "Report", "AT35", "School Compartmentwise Report", "SchoolCompartmentwiseReport.aspx", "HelpPage.Html", "35", "3");
        dtRights.Rows.Add("1038", "Attendance", "Report", "AT36", "Subject Wise Attendance Report", SubjectWiseAttendReport36, "HelpPage.Html", "36", "3");
        dtRights.Rows.Add("720", "Attendance", "Report", "AT37", "Subject Selected Student Report", "SubjectSelectedStudentreport.aspx", "HelpUrl.html", "37", "3");
        dtRights.Rows.Add("1039", "Attendance", "Report", "AT38", "Staff Key List", "StaffKeyListDetails.aspx", "HelpUrl.html", "38", "3");
        dtRights.Rows.Add("1040", "Attendance", "Report", "AT39", "Student Previous Attendance Report", "StudentsAttendancePrevousHistory.aspx", "HelpUrl.html", "39", "3");
        dtRights.Rows.Add("1041", "Attendance", "Report", "AT40", "Student Condonation Report", "CondonationReport.aspx", "HelpUrl.html", "40", "3");
        dtRights.Rows.Add("1042", "Attendance", "Report", "AT41", "Individual Student Condonation Report", "CondonationNewReport.aspx", "HelpUrl.html", "41", "3");
        //magesh 23/2/18
        dtRights.Rows.Add("100013", "Attendance", "Report", "AT42", "Overall College Attendance Percentage", "OverallcollegeAttendancepercentage.aspx", "HelpUrl.html", "42", "3");
        //kowshika 07.04.2018
        dtRights.Rows.Add("100014", "Attendance", "Report", "AT43", "Elective Subject Count Report", "ElectiveSubjectCountReport.aspx", "HelpUrl.html", "43", "3");
        dtRights.Rows.Add("100019", "Attendance", "Report", "AT44", "Late Entry Report", "Late Attendance Report.aspx", "HelpUrl.html", "44", "3");
        return dtRights;
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        if(Session["Entry_Code"]!=null)
        {
            string entryCode = Session["Entry_Code"].ToString();
            da.userTimeOut(entryCode);
        }
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

}

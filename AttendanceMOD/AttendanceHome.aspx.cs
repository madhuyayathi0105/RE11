using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class AttendanceHome : System.Web.UI.Page
{
    DAccess2 DA = new DAccess2();
    static string grouporusercode = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
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
        {
            grouporusercode = " group_code=" + group_code + "";
        }
        else
        {
            grouporusercode = " user_code=" + Session["usercode"].ToString().Trim() + "";
        }
        try
        {
            EntryCheck();
            DataSet dsRights = new DataSet();
            dsRights = DA.select_method_wo_parameter("select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Attendance' order by HeaderPriority, PagePriority asc", "Text");//select rights_code from security_user_right where " + grouporusercode + " 
            if (dsRights.Tables.Count > 0 && dsRights.Tables[0].Rows.Count > 0)
            {
                BindMenuGrid(dsRights.Tables[0]);
            }
            else
            {
                gridMenu.DataSource = null;
                gridMenu.DataBind();
            }
        }
        catch
        {
            gridMenu.DataSource = null;
            gridMenu.DataBind();
        }
    }

    private void BindMenuGrid(DataTable dtMenu)
    {
        gridMenu.DataSource = dtMenu;
        gridMenu.DataBind();
    }

    protected void gridMenu_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";
    }

    protected void gridMenu_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            for (int i = gridMenu.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = gridMenu.Rows[i];
                GridViewRow previousRow = gridMenu.Rows[i - 1];
                for (int j = 1; j <= 1; j++)
                {
                    Label lnlname = (Label)row.FindControl("lblHdrName");
                    Label lnlname1 = (Label)previousRow.FindControl("lblHdrName");
                    if (lnlname.Text == lnlname1.Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                            {
                                previousRow.Cells[j].RowSpan = 2;
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            }
                            row.Cells[j].Visible = false;
                        }
                    }
                }
            }
        }
        catch
        {
        }
        loadcolor();
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
        string saveCOl = DA.GetFunction("select LinkValue from inssettings where College_code ='" + Session["collegecode"].ToString() + "' and LinkName ='Individual Staff Login Attendance New'").Trim();
        //Master
        Session["StafforAdmin"] = "";
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
        dtRights.Rows.Add("100018", "Attendance", "Operation", "AO18", "Late Entry", "Late Attendance.aspx", "HelpUrl.html", "18", "2");//magesh
        //dtRights.Rows.Add("1042", "Attendance", "Report", "AT41", "Special Hour Master Setting", "Student_special_Hour_Entry.aspx", "HelpUrl.html", "41", "3");
        //dtRights.Rows.Add("1043", "Attendance", "Report", "AT42", "Special Hour Student Selection", "StudentEntryForSpecialClass.aspx", "HelpUrl.html", "42", "3");

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

        dtRights.Rows.Add("100019", "Attendance", "Report", "AT44", "Late Entry  Report", "Late Attendance Report.aspx", "HelpUrl.html", "44", "3");

        
        return dtRights;
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
                DA.update_method_wo_parameter(sbQuery.ToString(), "Text");
            }
        }
        catch { }
    }

    protected void loadcolor()
    {
        for (int ik = 0; ik < gridMenu.Rows.Count; ik++)
        {
            Label sno = (Label)gridMenu.Rows[ik].Cells[0].FindControl("lblSno");
            Label hdrname = (Label)gridMenu.Rows[ik].Cells[1].FindControl("lblHdrName");
            Label hdrid = (Label)gridMenu.Rows[ik].Cells[2].FindControl("lblReportId");
            LinkButton menu = (LinkButton)gridMenu.Rows[ik].Cells[3].FindControl("lbPagelink");
            HtmlAnchor help = (HtmlAnchor)gridMenu.Rows[ik].Cells[4].FindControl("lbHelplink");
            if (hdrname.Text == "Master")
            {
                sno.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                hdrname.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                hdrid.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                menu.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                help.Style.Add("color", "#ff00ff");
            }
            if (hdrname.Text == "Operation")
            {
                sno.ForeColor = Color.Black;
                hdrname.ForeColor = Color.Black;
                hdrid.ForeColor = Color.Black;
                menu.ForeColor = Color.Black;
                help.Style.Add("color", "Black");
            }
            if (hdrname.Text == "Report")
            {
                sno.ForeColor = Color.Green;
                hdrname.ForeColor = Color.Green;
                hdrid.ForeColor = Color.Green;
                menu.ForeColor = Color.Green;
                help.Style.Add("color", "Green");
            }
            if (hdrname.Text == "Charts")
            {
                sno.ForeColor = ColorTranslator.FromHtml("#3869fa");
                hdrname.ForeColor = ColorTranslator.FromHtml("#3869fa");
                hdrid.ForeColor = ColorTranslator.FromHtml("#3869fa");
                menu.ForeColor = ColorTranslator.FromHtml("#3869fa");
                help.Style.Add("color", "#3869fa");
            }
            if (hdrname.Text == "Others")
            {
                sno.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                hdrname.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                hdrid.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                menu.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                help.Style.Add("color", "#ff00ff");
            }
        }
    }

}
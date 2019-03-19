using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Text;


public partial class AdmissionHome : System.Web.UI.Page
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
            dsRights = DA.select_method_wo_parameter("select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Admission' order by HeaderPriority, PagePriority asc", "Text");//select rights_code from security_user_right where " + grouporusercode + " 
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

        //Master
        Session["StafforAdmin"] = "";
        Session["clearschedulesession"] = "clear";
        string linkpath = string.Empty;
        if (Convert.ToString(Session["Staff_Code"]) != "")
        {
            linkpath = "newstaf.aspx";
        }
        else
        {
            linkpath = "newadmin.aspx";
        }

        //------------Master
        dtRights.Rows.Add(88901, "Admission", "Master", "AM101", "Rank List Generation", "RankListGeneration.aspx", "HelpUrl.html", 1, 1);
        dtRights.Rows.Add(88902, "Admission", "Master", "AM102", "Date and Time Slot Settings", "DateAndTimeSlotSettings.aspx", "HelpUrl.html", 2, 1);
        dtRights.Rows.Add(88903, "Admission", "Master", "AM103", "Counselling Rank Range Settings - Slot Wise", "SlotwiseRankListSettings.aspx", "HelpUrl.html", 3, 1);
        dtRights.Rows.Add(88904, "Admission", "Master", "AM104", "Admission Settings", "AdmissionStreamSettings.aspx", "HelpUrl.html", 4, 1);

        dtRights.Rows.Add(88905, "Admission", "Master", "AM105", "Hostel Master Settings", "HostelMasterSettings.aspx", "HelpUrl.html", 5, 1);

        //-------------Operation

        dtRights.Rows.Add(88906, "Admission", "Operation", "AO101", "Degree Wise Seat Allotment - Sheet Matrix", "DegreewiseSeatAllotment.aspx", "HelpUrl.html", 1, 2);
        dtRights.Rows.Add(88907, "Admission", "Operation", "AO102", "Counselling Student Registration", "Student_Selection.aspx", "HelpUrl.html", 2, 2);
        dtRights.Rows.Add(88908, "Admission", "Operation", "AO103", "Counselling Student Registration - Special Permission", "StudentRegistrationNew.aspx", "HelpUrl.html", 3, 2);
        dtRights.Rows.Add(88909, "Admission", "Operation", "AO104", "Slot Time Extention for Registration", "StudentExtentedTime.aspx", "HelpUrl.html", 4, 2);
        dtRights.Rows.Add(88910, "Admission", "Operation", "AO105", "Counselling Student Certificate Verification", "Student_verification.aspx", "HelpUrl.html", 5, 2);
        dtRights.Rows.Add(88911, "Admission", "Operation", "AO106", "Student Course Selection", "StudentCourseSelection.aspx", "HelpUrl.html", 6, 2);
        dtRights.Rows.Add(88912, "Admission", "Operation", "AO107", "Student Selected Course Rejection", "StudentCourseRejection.aspx", "HelpUrl.html", 7, 2);
        dtRights.Rows.Add(88913, "Admission", "Operation", "AO108", "Seat Availability", "SeatStatus.aspx", "HelpUrl.html", 8, 2);
        dtRights.Rows.Add(88914, "Admission", "Operation", "AO109", "Hostel Seat Availability", "HostelStatus.aspx", "HelpUrl.html", 9, 2);
        dtRights.Rows.Add(88915, "Admission", "Operation", "AO110", "Transport Seat Availability", "Transport_availability.aspx", "HelpUrl.html", 10, 2);
        dtRights.Rows.Add(88916, "Admission", "Operation", "AO111", "Student Hostel and Transport Selection Process", "Student_Hostelandtransport_request.aspx", "HelpUrl.html", 11, 2);
        dtRights.Rows.Add(88928, "Admission", "Operation", "AO112", "Student Course Selection - Special Permission", "StudentCourseSelectionSplPermission.aspx", "HelpUrl.html", 12, 2);

        //-------------Report

        dtRights.Rows.Add(88917, "Admission", "Report", "AR101", "Admission Status Report Detail", "StudentsAdmissionSelectionStatusReportDetail.aspx", "HelpUrl.html", 1, 3);
        dtRights.Rows.Add(88918, "Admission", "Report", "AR102", "Admission Status Report Count", "StudentsAdmissionSelectionStatusReport.aspx", "HelpUrl.html", 2, 3);
        dtRights.Rows.Add(88919, "Admission", "Report", "AR103", "Admission Status Report Chart", "Admission_chart.aspx", "HelpUrl.html", 3, 3);
        dtRights.Rows.Add(88920, "Admission", "Report", "AR104", "Counselling Student - SMS Alert", "CounsellingRankListSMS.aspx", "HelpUrl.html", 4, 3);
        //dtRights.Rows.Add(88921, "Admission", "Report", "AR105", "Elective Subject Student Strength Count", "Elective Subject Student Count.aspx", "HelpUrl.html", 5, 3);
        //dtRights.Rows.Add(88922, "Admission", "Report", "AR106", "Class Wise and Section Wise Strength", "ClassSectionWiseMasterSettings.aspx", "HelpUrl.html", 6, 3);
        dtRights.Rows.Add(88923, "Admission", "Report", "AR105", "Subject Code and Subject Name Edit", "SubjectNoSubjectNameEdit.aspx", "HelpUrl.html", 7, 3);
        dtRights.Rows.Add(88924, "Admission", "Report", "AR106", "Branch Sliding / Change", "StudentTransferSteamWise.aspx", "HelpUrl.html", 7, 3);
        dtRights.Rows.Add(88925, "Admission", "Report", "AR107", "CBCS Registration", "CBSCRegistration.aspx", "HelpUrl.html", 9, 3);
        //dtRights.Rows.Add(88926, "Admission", "Report", "AR110", "CBCS Report", "ElectiveSubjectCountReport.aspx", "HelpUrl.html", 10, 3);
        dtRights.Rows.Add(88927, "Admission", "Report", "AR108", "Admission Status Report", "AdmissionStatusReport.aspx", "HelpUrl.html", 10, 3);

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
using System;
using System.Data;
using System.Text;
using System.Drawing;
using System.Web.UI.WebControls;

public partial class Feedbackhome : System.Web.UI.Page
{
    DAccess2 DA = new DAccess2();
    static string grouporusercode = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }

        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grouporusercode = " user_code=" + Session["usercode"].ToString().Trim() + "";
        }
        try
        {
            EntryCheck();
            DataSet dsRights = new DataSet();
            dsRights = DA.select_method_wo_parameter("select ModuleName,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and Modulename='Feedback' order by HeaderPriority, PagePriority asc", "Text");//select rights_code from security_user_right where " + grouporusercode + " 
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

        dtRights.Rows.Add("2501", "Feedback", "Master", "FB001", "Type", "Type_Master.aspx", "HelpPage.Html", "1", "1");
        dtRights.Rows.Add("2502", "Feedback", "Master", "FB002", "Question", "Question_Master.aspx", "HelpPage.Html", "2", "1");
        dtRights.Rows.Add("2503", "Feedback", "Master", "FB003", "Feed Back", "FeedBack_Master.aspx", "HelpPage.Html", "3", "1");
        dtRights.Rows.Add("2504", "Feedback", "Master", "FB004", "FeedBack Question", "FeedBack_Question_Master.aspx", "HelpPage.Html", "4", "1");
        dtRights.Rows.Add("2505", "Feedback", "Master", "FB005", "FeedBack Question Type", "FeedBackquestion_type.aspx", "HelpPage.Html", "5", "1");
        dtRights.Rows.Add("2510", "Feedback", "Operation", "FBO001", "Online FeedbackTest", "Online_FeedBack.aspx", "HelpPage.Html", "1", "2");
        dtRights.Rows.Add("2514", "Feedback", "Operation", "FBO002", "Grievance Mail", "Grievance Mail.aspx", "HelpPage.Html", "2", "2");
        dtRights.Rows.Add("2506", "Feedback", "Report", "FBR001", "Feed Back Report", "Feedback_report.aspx", "HelpPage.Html", "1", "3");
        dtRights.Rows.Add("2507", "Feedback", "Report", "FBR002", "Uniquecode Generation", "uniquecode_generation_Report.aspx", "HelpPage.Html", "2", "3");
        dtRights.Rows.Add("2508", "Feedback", "Report", "FBR003", "Anonymous Feedback Report", "Feedback_anonymousisgender.aspx", "HelpPage.Html", "3", "3");
        dtRights.Rows.Add("2509", "Feedback", "Report", "FBR004", "General Feedback Report", "Generalfeedback.aspx", "HelpPage.Html", "4", "3");
        dtRights.Rows.Add("2511", "Feedback", "Report", "FBR005", "Feedback Report Consolidated", "Feedbackreport_consolidation.aspx", "HelpPage.Html", "5", "3");
        dtRights.Rows.Add("2512", "Feedback", "Report", "FBR006", "Anonymous Department Staff Report", "AnonymousDepartmentwiseReport.aspx", "HelpPage.Html", "6", "3");
        dtRights.Rows.Add("2513", "Feedback", "Report", "FBR007", "General Descriptive Feedback Report", "generaldescriptivefeedbackreport.aspx", "HelpPage.Html", "7", "3");
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
                sbQuery.Append("IF Exists (select Rights_Code from Security_Rights_Details where  Rights_Code ='" + rightsCode + "') Update Security_Rights_Details set ModuleName ='" + Convert.ToString(dtRights.Rows[row]["Module"]) + "',HeaderName='" + Convert.ToString(dtRights.Rows[row]["Header"]) + "' ,ReportId='" + Convert.ToString(dtRights.Rows[row]["ReportId"]) + "' ,ReportName='" + Convert.ToString(dtRights.Rows[row]["ReportName"]) + "' ,PageName='" + Convert.ToString(dtRights.Rows[row]["PageName"]) + "' ,HelpURL='" + Convert.ToString(dtRights.Rows[row]["HelpPage"]) + "' ,PagePriority='" + Convert.ToString(dtRights.Rows[row]["PagePriority"]) + "' ,HeaderPriority='" + Convert.ToString(dtRights.Rows[row]["HeaderPriority"]) + "' where Rights_Code ='" + rightsCode + "' ELSE insert into Security_Rights_Details (ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL ,PagePriority ,HeaderPriority ) values ('" + Convert.ToString(dtRights.Rows[row]["Module"]) + "','" + Convert.ToString(dtRights.Rows[row]["Header"]) + "','" + rightsCode + "','" + Convert.ToString(dtRights.Rows[row]["ReportId"]) + "','" + Convert.ToString(dtRights.Rows[row]["ReportName"]) + "','" + Convert.ToString(dtRights.Rows[row]["PageName"]) + "','" + Convert.ToString(dtRights.Rows[row]["HelpPage"]) + "','" + Convert.ToString(dtRights.Rows[row]["PagePriority"]) + "','" + Convert.ToString(dtRights.Rows[row]["HeaderPriority"]) + "')");

                int sampu = DA.update_method_wo_parameter(sbQuery.ToString(), "Text");
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
            LinkButton help = (LinkButton)gridMenu.Rows[ik].Cells[4].FindControl("lbHelplink");
            if (hdrname.Text == "Master")
            {
                sno.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                hdrname.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                hdrid.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                menu.ForeColor = ColorTranslator.FromHtml("#ff00ff");
                help.ForeColor = ColorTranslator.FromHtml("#ff00ff");
            }
            //if (hdrname.Text == "Operation")
            //{
            //    sno.ForeColor = Color.Black;
            //    hdrname.ForeColor = Color.Black;
            //    hdrid.ForeColor = Color.Black;
            //    menu.ForeColor = Color.Black;
            //    help.ForeColor = Color.Black;
            //}
            if (hdrname.Text == "Report")
            {
                sno.ForeColor = Color.Green;
                hdrname.ForeColor = Color.Green;
                hdrid.ForeColor = Color.Green;
                menu.ForeColor = Color.Green;
                help.ForeColor = Color.Green;
            }
        }
    }


    protected void gridMenu_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //  e.Row.BackColor = ColorTranslator.FromHtml("#FF5600");
            if (e.Row.RowType == DataControlRowType.Header)
                e.Row.CssClass = "header";
        }
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
}
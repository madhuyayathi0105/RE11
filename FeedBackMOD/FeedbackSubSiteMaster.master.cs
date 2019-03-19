using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class FeedbackSubSiteMaster : System.Web.UI.MasterPage
{
    DAccess2 da = new DAccess2();
    static string grouporusercode = string.Empty;
    string sql = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //string strPreviousPage = "";
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
        string colornew = "";
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
            if (Convert.ToString(Session["Staff_Code"]) != "")
            {
                img_stfphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + Session["Staff_Code"];
                imgstdphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + Session["Staff_Code"];
                string stfdescode = "";
                sql = "select desig_code from stafftrans where staff_code='" + Convert.ToString(Session["Staff_Code"]) + "' and latestrec=1";
                stfdescode = da.GetFunction(sql);


                if (stfdescode != "" && stfdescode != null)
                {
                    string stfdesigname = "";
                    sql = "select dm.desig_name from desig_master dm where dm.desig_code='" + stfdescode.ToString() + "' and collegecode=" + Session["collegecode"].ToString();
                    stfdesigname = da.GetFunction(sql);



                    string staffname = "";
                    sql = "select staff_name from staffmaster where staff_code='" + Session["staff_code"] + "'";
                    staffname = da.GetFunction(sql);

                    string deptname = "";
                    sql = "select dt.dept_acronym from Department dt,stafftrans st where dt.Dept_code=st.dept_code and staff_code='" + Session["staff_code"] + "' and latestrec=1";
                    deptname = da.GetFunction(sql);
                    lbslstaffname.Text = Convert.ToString(staffname);
                    lbldesignation.Text = Convert.ToString(stfdesigname);
                    lbldept.Text = Convert.ToString(deptname);

                }
            }
            else
            {


                string staffname = "";
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
            SelQ = "  select distinct HeaderName from Security_Rights_Details where Rights_Code in(select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Feedback'";
            SelQ = SelQ + " select ModuleName ,HeaderName ,Rights_Code ,ReportId ,ReportName ,PageName ,HelpURL  from Security_Rights_Details where Rights_Code in (select rights_code from security_user_right where " + grouporusercode + " ) and ModuleName='Feedback' order by HeaderPriority, PagePriority asc";
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
                        tabs1.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 15px;");
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
                        tabs2.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 15px;");
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
                        tabs3.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 15px;");
                        HtmlGenericControl anchor = new HtmlGenericControl("a");
                        anchor.Attributes.Add("target", "_blank");
                        anchor.Attributes.Add("href", Convert.ToString(dvnew[tab3]["PageName"]));
                        anchor.InnerText = Convert.ToString(dvnew[tab3]["ReportName"]);
                        li.Controls.Add(anchor);
                    }
                }
                else
                    ReportList.Visible = false;
                dsRights.Tables[1].DefaultView.RowFilter = " HeaderName='Charts'";
                dvnew = dsRights.Tables[1].DefaultView;
                if (dvnew.Count > 0)
                {
                    ChartList.Visible = true;
                    for (int tab4 = 0; tab4 < dvnew.Count; tab4++)
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        tabs4.Controls.Add(li);
                        tabs4.Attributes.Add("style", "border: 1px solid #999999;background-color: #F0F0F0;box-shadow: 0px 0px 8px #999999; -moz-box-shadow: 0px 0px 10px #999999;-webkit-box-shadow: 0px 0px 10px #999999;border: 1px solid #D9D9D9;border-radius: 15px;");
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
}
a:hover
        {
color:lightyellow;
}
                                                </style>
                                                ";
        this.Page.Header.Controls.Add(ltr);
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

                int sampu = da.update_method_wo_parameter(sbQuery.ToString(), "Text");
            }
        }
        catch { }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        if (Session["Entry_Code"] != null)
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

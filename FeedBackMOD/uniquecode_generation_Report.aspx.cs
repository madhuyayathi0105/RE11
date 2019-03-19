using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class uniquecode_generation_Report : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DateTime dt = new DateTime();
    DateTime dt1 = new DateTime();
    string q1 = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        lblvalidation1.Text = "";
        if (!IsPostBack)
        {
            bindcollege(); bindfeedback();
            FpSpread1.Visible = false;
        }
    }
    protected void lnk_btnlogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    protected void bindcollege()
    {
        try
        {
            ddl_collegename.Items.Clear();
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
            else
            {
                ddl_collegename.Items.Clear();
                ddl_collegename.Items.Insert(0, "Select");
            }
        }
        catch { }
    }
    protected void bindfeedback()
    {
        try
        {
            if (ddl_collegename.Items.Count > 0)
            {
                ddl_feedbackname.Items.Clear();
                ds.Clear();
                ds = d2.select_method_wo_parameter("select  distinct  FeedBackName  from CO_FeedBackMaster where  CollegeCode in ('" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "') and FeedBackName<>''", "text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_feedbackname.DataSource = ds;
                    ddl_feedbackname.DataTextField = "FeedBackName";
                    ddl_feedbackname.DataValueField = "FeedBackName";
                    ddl_feedbackname.DataBind();
                }
                else
                {
                    ddl_feedbackname.Items.Clear();
                    ddl_feedbackname.Items.Insert(0, "Select");
                }
            }
            else
            {
                lbl_error.Visible = false;
                lbl_error.Text = "Please Select College Name ";
            }
        }
        catch { }
    }

    protected void ddl_collegename_selectedindex(object sender, EventArgs e)
    {
        try
        {
            bindfeedback();
        }
        catch { }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            if (ddl_feedbackname.Items.Count > 0)
            {
                if (ddl_feedbackname.SelectedItem.Text.Trim() != "--Select--")
                {
                    q1 = " select COUNT(FeedbackUnicode),m.Batch_Year,m.DegreeCode,m.semester,m.Section,FeedBackMasterPK,(cr.Course_Name +' - '+dt.Dept_Name) as Dept,cr.Course_Id ,d.Degree_Code from CO_FeedbackUniCode c,CO_FeedBackMaster M,Degree d,Department dt,Course cr where cr.Course_Id =d.Course_Id and d.Dept_Code =dt.Dept_Code and d.Degree_Code =m.DegreeCode and c.FeedbackMasterFK =m.FeedBackMasterPK and FeedBackName ='" + Convert.ToString(ddl_feedbackname.SelectedItem.Text) + "' group by m.DegreeCode,m.Batch_Year,m.semester,m.Section,FeedBackMasterPK,(cr.Course_Name +' - '+dt.Dept_Name) ,cr.Course_Id,d.Degree_Code order by d.Degree_Code,m.batch_year asc";//cr.Course_Id asc ";

                    q1 = q1 + " select c.FeedbackUnicode ,FeedBackMasterPK  from CO_FeedbackUniCode c,CO_FeedBackMaster M where c.FeedbackMasterFK =m.FeedBackMasterPK ";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FpSpread1.Sheets[0].RowCount = 1;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 2;
                        FpSpread1.Sheets[0].AutoPostBack = false;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread1.Columns[0].Width = 150;
                        FpSpread1.Columns[0].Locked = true;
                        FpSpread1.Columns[1].Locked = true;
                        FpSpread1.Columns[1].Width = 500;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.NO";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Unique Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ddl_feedbackname.SelectedItem.Text);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
                            string section = Convert.ToString(ds.Tables[0].Rows[j]["Section"]);
                            string semester = Convert.ToString(ds.Tables[0].Rows[j]["semester"]);
                            string Batch_Year = Convert.ToString(ds.Tables[0].Rows[j]["Batch_Year"]);
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Batch_Year + " - " + Convert.ToString(ds.Tables[0].Rows[j]["Dept"]) + " - " + semester + " - " + section;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            FpSpread1.Sheets[0].AddSpanCell(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);

                            ds.Tables[1].DefaultView.RowFilter = "FeedBackMasterPK='" + Convert.ToString(ds.Tables[0].Rows[j]["FeedBackMasterPK"]) + "'";
                            DataView dv1 = ds.Tables[1].DefaultView;
                            int co = 0;
                            if (dv1.Count > 0)
                            {
                                for (int k = 0; k < dv1.Count; k++)
                                {
                                    co++;
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(k + 1);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv1[k]["FeedbackUnicode"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                }
                            }
                            FpSpread1.Sheets[0].RowCount++;
                        }
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        FpSpread1.Visible = true;
                        lbl_error.Visible = false;
                        rptprint.Visible = true;
                    }
                    else
                    {
                        FpSpread1.Visible = false;
                        rptprint.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "No Records Founds";
                    }
                }
                else
                {
                    FpSpread1.Visible = false;
                    rptprint.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Select Feedback Name";
                }
            }
        }
        catch
        { }
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {

        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Unique Code Generation Report";
            string pagename = "uniquecode_generation_Report.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
}
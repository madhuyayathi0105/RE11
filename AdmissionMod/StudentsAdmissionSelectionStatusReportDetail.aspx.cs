using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Threading;
using System.Globalization;
using InsproDataAccess;
using System.Data;

public partial class StudentsAdmissionSelectionStatusReportDetail : System.Web.UI.Page
{
    #region Field Declaration

    Hashtable ht = new Hashtable();

    string userCode = string.Empty;
    string collegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;

    string qry = string.Empty;
    string qryCollegeCode = string.Empty;
    string qryGraduate = string.Empty;
    string qryBatch = string.Empty;
    string qryCourse = string.Empty;
    string qryDate = string.Empty;
    string qrySession = string.Empty;
    string qryStream = string.Empty;

    string batchYear = string.Empty;
    string graduate = string.Empty;
    string courseId = string.Empty;
    string courseName = string.Empty;
    string streamName = string.Empty;
    string streamCode = string.Empty;
    string fromDate = string.Empty;
    string toDate = string.Empty;

    DateTime dtFromDate = new DateTime();
    DateTime dtToDate = new DateTime();

    bool isSchool = false;
    int selected = 0;
    byte reportValue = 0;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    InsproDirectAccess dirAcc = new InsproDirectAccess();

    #endregion

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            userCode = Convert.ToString(Session["usercode"]).Trim();
            collegeCode = Convert.ToString(Session["collegecode"]).Trim();
            singleUser = Convert.ToString(Session["single_user"]).Trim();
            groupUserCode = Convert.ToString(Session["group_code"]).Trim();
            if (!IsPostBack)
            {
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                divPopupAlert.Visible = false;
                lblAlertMsg.Text = string.Empty;

                if (ddlReportType.Items.Count > 0)
                    ddlReportType.SelectedIndex = 0;
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Attributes.Add("readonly", "readonly");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                BindCollege();
                BindBatch();
                BindGraduate();
                BindCourse();
                BindStream();
                BindSession();
                ddlgender.Items.Clear();
                ddlgender.Items.Add("All");
                ddlgender.Items.Add("Male");
                ddlgender.Items.Add("Female");
            }
        }
        catch (ThreadAbortException tt)
        {

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

    #region Bind Header

    private void BindCollege()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            string columnfield = string.Empty;
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(groupUserCode).Trim() != "") && Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                columnfield = " and group_code='" + groupUserCode + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            ht.Clear();
            ht.Add("column_field", Convert.ToString(columnfield));
            ds.Dispose();
            ds.Clear();
            ds.Reset();
            ds = d2.select_method("bind_college", ht, "sp");
            ddlCollege.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
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
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void BindBatch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            ds.Dispose();
            ds.Reset();
            ds.Clear();
            ddlBatch.Items.Clear();
            ddlBatch.Enabled = false;
            if (ddlCollege.Items.Count > 0)
            {
                selected = 0;
                qryCollegeCode = string.Empty;
                string collegeCodeNew = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(collegeCodeNew.Trim()))
                        {
                            collegeCodeNew = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            collegeCodeNew += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCodeNew) && selected > 0)
                {
                    qryCollegeCode = " and r.college_code in(" + collegeCodeNew + ")";
                }
                ds = d2.select_method_wo_parameter("select distinct r.Batch_Year from applyn r where r.batch_year<>'-1' and r.batch_year<>'' " + qryCollegeCode + " order by r.Batch_Year desc", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlBatch.DataSource = ds;
                    ddlBatch.DataTextField = "Batch_Year";
                    ddlBatch.DataValueField = "Batch_Year";
                    ddlBatch.DataBind();
                    ddlBatch.Enabled = true;
                    ddlBatch.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void BindGraduate()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            ds.Dispose();
            ds.Reset();
            ds.Clear();
            ddlEduLevel.Items.Clear();
            ddlEduLevel.Enabled = false;
            if (ddlCollege.Items.Count > 0)
            {
                selected = 0;
                qryCollegeCode = string.Empty;
                string collegeCodeNew = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(collegeCodeNew.Trim()))
                        {
                            collegeCodeNew = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            collegeCodeNew += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCodeNew) && selected > 0)
                {
                    qryCollegeCode = " and c.college_code in(" + collegeCodeNew + ")";
                }
            }
            qry = "select distinct c.Edu_Level from Course c where 1=1 " + qryCollegeCode + "  order by c.Edu_Level desc";
            ds = d2.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlEduLevel.DataSource = ds;
                ddlEduLevel.DataTextField = "Edu_Level";
                ddlEduLevel.DataValueField = "Edu_Level";
                ddlEduLevel.DataBind();
                ddlEduLevel.Enabled = true;
                ddlEduLevel.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void BindCourse()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            ds.Dispose();
            ds.Reset();
            ds.Clear();
            ddlCourse.Items.Clear();
            ddlCourse.Enabled = false;
            qryCollegeCode = string.Empty;
            string graduate = string.Empty;
            qryGraduate = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                selected = 0;
                string collegeCodeNew = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(collegeCodeNew.Trim()))
                        {
                            collegeCodeNew = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            collegeCodeNew += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCodeNew) && selected > 0)
                {
                    qryCollegeCode = " and c.college_code in(" + collegeCodeNew + ")";
                }
            }
            if (ddlEduLevel.Items.Count > 0)
            {
                selected = 0;
                graduate = string.Empty;
                foreach (ListItem li in ddlEduLevel.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(graduate.Trim()))
                        {
                            graduate = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            graduate += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(graduate) && selected > 0)
                {
                    qryGraduate = " and c.edu_level in(" + graduate + ")";
                }
            }
            qry = "select distinct c.Course_Id,c.Course_Name,c.Priority from Course c where 1=1 " + qryCollegeCode + qryGraduate + " order by c.Priority,c.Course_Id";
            ds = d2.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlCourse.DataSource = ds;
                ddlCourse.DataTextField = "Course_Name";
                ddlCourse.DataValueField = "Course_Id";
                ddlCourse.DataBind();
                ddlCourse.Enabled = true;
                ddlCourse.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void BindStream()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            ds.Dispose();
            ds.Reset();
            ds.Clear();
            ddlStream.Items.Clear();
            ddlStream.Enabled = false;
            if (ddlCollege.Items.Count > 0)
            {
                selected = 0;
                qryCollegeCode = string.Empty;
                string collegeCodeNew = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(collegeCodeNew.Trim()))
                        {
                            collegeCodeNew = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            collegeCodeNew += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCodeNew) && selected > 0)
                {
                    qryCollegeCode = " and tv.college_code in(" + collegeCodeNew + ")";
                }
            }
            qry = "select TextCode,TextVal from TextValTable tv where TextCriteria='ADMst' " + qryCollegeCode + " order by TextVal";
            ds = d2.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlStream.DataSource = ds;
                ddlStream.DataTextField = "TextVal";
                ddlStream.DataValueField = "TextCode";
                ddlStream.DataBind();
                ddlStream.Enabled = true;
                ddlStream.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void BindSession()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            ds.Dispose();
            ds.Reset();
            ds.Clear();
            ddlSession.Items.Clear();
            ddlSession.Enabled = false;
            qryBatch = string.Empty;
            qryCollegeCode = string.Empty;
            qryCourse = string.Empty;
            qryGraduate = string.Empty;
            qryDate = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                selected = 0;
                string collegeCodeNew = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(collegeCodeNew.Trim()))
                        {
                            collegeCodeNew = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            collegeCodeNew += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCodeNew) && selected > 0)
                {
                    qryCollegeCode = " and ds.CollegeCode in(" + collegeCodeNew + ")";
                }
            }

            if (ddlBatch.Items.Count > 0)
            {
                selected = 0;

                batchYear = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(batchYear.Trim()))
                        {
                            batchYear = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYear) && selected > 0)
                {
                    qryBatch = " and ds.BatchYear in(" + batchYear + ")";
                }
            }

            if (ddlEduLevel.Items.Count > 0)
            {
                selected = 0;
                qryGraduate = string.Empty;
                graduate = string.Empty;
                foreach (ListItem li in ddlEduLevel.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(graduate.Trim()))
                        {
                            graduate = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            graduate += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(graduate) && selected > 0)
                {
                    qryGraduate = " and ds.EduLevel in(" + graduate + ")";
                }
            }

            if (ddlCourse.Items.Count > 0)
            {
                selected = 0;
                qryCourse = string.Empty;
                courseId = string.Empty;
                foreach (ListItem li in ddlCourse.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(courseId.Trim()))
                        {
                            courseId = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            courseId += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(courseId) && selected > 0)
                {
                    qryCourse = " and ds.CourseID in(" + courseId + ")";
                }
            }
            fromDate = Convert.ToString(txtFromDate.Text).Trim();
            toDate = Convert.ToString(txtToDate.Text).Trim();
            dtFromDate = new DateTime();
            dtToDate = new DateTime();
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime.TryParseExact(fromDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
                qryDate = " and SlotDate>='" + dtFromDate.ToShortDateString() + "'";
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime.TryParseExact(toDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
                qryDate += "and SlotDate<='" + dtToDate.ToShortDateString() + "'";
            }
            qry = "select distinct SlotTime from ST_DaySlot ds where SlotTime is not null and SlotTime<>'' " + qryCollegeCode + qryBatch + qryGraduate + qryCourse + qryDate + " ";//and SlotDate between '05/20/2017' and '05/31/2017' and ds.BatchYear='2016' and ds.CollegeCode='13' and ds.CourseID='24' and ds.EduLevel='UG'
            ds = d2.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSession.DataSource = ds;
                ddlSession.DataTextField = "SlotTime";
                ddlSession.DataValueField = "SlotTime";
                ddlSession.DataBind();
                ddlSession.Enabled = true;
                ddlSession.Items.Insert(0, "All");
                ddlSession.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

    #region Index ChangeEvent

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;

            BindBatch();
            BindGraduate();
            BindCourse();
            BindStream();
            BindSession();
            clearReportGrid();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            BindGraduate();
            BindCourse();
            BindStream();
            BindSession();
            clearReportGrid();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlEduLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;


            BindCourse();
            BindStream();
            BindSession();
            clearReportGrid();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;


            BindStream();
            BindSession();
            clearReportGrid();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlStream_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;


            BindSession();
            clearReportGrid();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            ddlgender.Visible = false;
            clearReportGrid();
            Showdiv.Visible = false;
            if (ddlReportType.SelectedItem.Text == "Hostel registered")
            {
                ddlgender.Visible = true;
                Showdiv.Visible = true;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            clearReportGrid();

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        try
        {

            DateTime fromdate = Convert.ToDateTime(txtFromDate.Text.Split('/')[1] + "/" + txtFromDate.Text.Split('/')[0] + "/" + txtFromDate.Text.Split('/')[2]);
            DateTime todate = Convert.ToDateTime(txtToDate.Text.Split('/')[1] + "/" + txtToDate.Text.Split('/')[0] + "/" + txtToDate.Text.Split('/')[2]);

            if (fromdate <= todate)
            {
            }
            else
            {
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }

            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;

            clearReportGrid();

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

    #region Click

    #region Close Popup

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Close Popup

    #region Show Report

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            clearReportGrid();

            string collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            string batchYear = Convert.ToString(ddlBatch.SelectedValue);
            string eduLevel = Convert.ToString(ddlEduLevel.SelectedValue);
            string courseCode = Convert.ToString(ddlCourse.SelectedValue);
            string streamCode = Convert.ToString(ddlStream.SelectedValue);
            string criteriaCode = string.Empty;

            string[] resVal = dirAcc.selectScalarString("SELECT LinkValue FROM New_InsSettings WHERE LinkName='ADMISSIONCOURSESELECTIONSETTINGS' AND college_code='" + collegeCode + "'").Split('$');

            if (resVal.Length == 6)
            {
                criteriaCode = resVal[5];
            }

            string sessionVal = ddlSession.Items.Count > 0 ? ((ddlSession.SelectedItem.Value != "All") ? ddlSession.SelectedItem.Value : string.Empty) : string.Empty;
            string session = string.Empty;
            session = sessionVal;
            sessionVal = string.IsNullOrEmpty(sessionVal) ? string.Empty : " and SlotTime in ('" + sessionVal + "') ";
            string[] fromDate = txtFromDate.Text.Split('/');
            string[] toDate = txtToDate.Text.Split('/');

            string fromDt = fromDate[1] + "/" + fromDate[0] + "/" + fromDate[2];
            string toDt = toDate[1] + "/" + toDate[0] + "/" + toDate[2];

            byte reportType = Convert.ToByte(ddlReportType.SelectedIndex);
            reportValue = reportType;
            bool boolCheck = false;
            string selQ = string.Empty;
            selQ = "select distinct sr.criteriaCode  from ST_DaySlot sd,ST_RankListSlot sr where sd.ST_DaySlotPK =sr.ST_DaySlotFk and sd.SlotDate  between '" + fromDt + "' and '" + toDt + "'";
            if (ddlSession.SelectedItem.Text.Trim() != "All")
            {
                selQ += " and sd.SlotTime ='" + session + "'";
            }

            DataTable dtdata = dirAcc.selectDataTable(selQ);
            selQ = string.Empty;
            if (dtdata.Rows.Count > 0)
            {
                criteriaCode = string.Empty;
                for (int introw = 0; introw < dtdata.Rows.Count; introw++)
                {
                    if (criteriaCode.Trim() == "")
                    {
                        criteriaCode = Convert.ToString(dtdata.Rows[introw]["criteriaCode"]);
                    }
                    else
                    {
                        criteriaCode += "','" + Convert.ToString(dtdata.Rows[introw]["criteriaCode"]);
                    }
                }
            }
            string Gender = string.Empty;
            if (ddlgender.SelectedItem.Text != "All")
            {
                string Text = ddlgender.SelectedItem.Text;
                if (Text.Trim() == "Male")
                {
                    Gender = "and sex ='0'";
                }
                else
                {
                    Gender = "and sex ='1'";
                }
            }
            switch (reportType)
            {
                case 0://Counselling Called Report
                    //selQ = "select a.app_formno as [Application Number],a.stud_name as [Student Name],a.Student_Mobile as [Mobile No],a.StuPer_Id as [Email ID],rt.ST_Rank as [Rank],convert(varchar(10),a.enrollment_card_date ,103) as [Date],a.enrollment_session as [Session Time] from applyn a ,ST_RankTable rt where a.app_no=rt.ST_AppNo " + sessionVal + " and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria='" + criteriaCode + "'  and (a.enrollment_card_date between '" + fromDt + "' and '" + toDt + "') order by ST_Rank ";

                    selQ = " select a.app_formno as [Application Number],a.stud_name as [Student Name],a.Student_Mobile as [Mobile No],a.StuPer_Id as [Email ID],rt.ST_Rank as [Rank],convert(varchar(10),d.SlotDate ,103) as [Date],d.SlotTime as [Session Time] from applyn a ,ST_RankTable rt,ST_DaySlot D,ST_RankListSlot SR,ST_StudentSession ss where a.app_no=rt.ST_AppNo AND SS.ST_App_No =a.app_no and ss.ST_RankListFk =sr.ST_RankListPk and sr.ST_DaySlotFk =d.ST_DaySlotPK and SR.criteriaCode=rt.ST_RankCriteria and rt.ST_Stream =sr.streamCode  and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria in ('" + criteriaCode + "')  and (d.SlotDate between '" + fromDt + "' and '" + toDt + "')  " + sessionVal + " order by ST_Rank ";

                    break;
                case 1://Registered
                    //selQ = "select a.app_formno as [Application Number],a.stud_name as [Student Name],a.Student_Mobile as [Mobile No],a.StuPer_Id as [Email ID],convert(varchar(10),a.enrollment_card_date ,103) as [Date],a.enrollment_session as [Session Time] from applyn a ,ST_RankTable rt where a.app_no=rt.ST_AppNo " + sessionVal + " and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria='" + criteriaCode + "' and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and (a.enrollment_card_date between '" + fromDt + "' and '" + toDt + "') order by ST_Rank ";
                    //a.Student_Mobile as [Mobile No],a.StuPer_Id as [Email ID],convert(varchar(10),d.SlotDate ,103) as [Date],d.SlotTime as [Session Time]
                    selQ = " select a.app_no, a.app_formno as [Application Number],a.stud_name as [Student Name],rt.ST_Rank as [Rank] from applyn a ,ST_RankTable rt,ST_DaySlot D,ST_RankListSlot SR,ST_StudentSession ss where a.app_no=rt.ST_AppNo AND SS.ST_App_No =a.app_no and ss.ST_RankListFk =sr.ST_RankListPk and sr.ST_DaySlotFk =d.ST_DaySlotPK and SR.criteriaCode=rt.ST_RankCriteria and rt.ST_Stream =sr.streamCode  and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria in ('" + criteriaCode + "') and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1'  and (d.SlotDate between '" + fromDt + "' and '" + toDt + "')  " + sessionVal + " and (CONVERT(varchar(10), a.enrollment_card_date,101)) between '" + fromDt + "' and '" + toDt + "' order by ST_Rank ";

                    if (ddlStream.SelectedItem.Text == "Stream I")
                    {
                        selQ += "  select r.ST_AppNo,ST_RankCriteria,ST_Rank,ST_Stream,(select masterValue from co_mastervalues mv where mv.mastercode=ST_RankCriteria and mastercriteria='StudRankCriteria') as Criteria,HSCMarkSec,jeeMarkSec,cast (CombinedScore as decimal(10,4)) as CombinedScore from ST_RankTable r,ST_Student_Mark_Detail st where st.ST_AppNo =r.ST_AppNo and ST_Stream ='" + streamCode + "' order by ST_RankCriteria";
                    }
                    else
                    {
                        selQ += "  select r.ST_AppNo,ST_RankCriteria,ST_Rank,ST_Stream,(select masterValue from co_mastervalues mv where mv.mastercode=ST_RankCriteria and mastercriteria='StudRankCriteria') as Criteria,HSCMarkSec,jeeMarkSec,cast (CombinedScoreSII as decimal(10,4)) as CombinedScore from ST_RankTable r,ST_Student_Mark_Detail st where st.ST_AppNo =r.ST_AppNo and ST_Stream ='" + streamCode + "' order by ST_RankCriteria";
                    }


                    break;
                case 2://Not Registered
                    //selQ = "select a.app_formno as [Application Number],a.stud_name as [Student Name],a.Student_Mobile as [Mobile No],a.StuPer_Id as [Email ID],rt.ST_Rank as [Rank],convert(varchar(10),a.enrollment_card_date ,103) as [Date],a.enrollment_session as [Session Time] from applyn a ,ST_RankTable rt where a.app_no=rt.ST_AppNo " + sessionVal + " and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria='" + criteriaCode + "' and isnull(a.selection_status,'0') ='0' and ISNULL(IsConfirm,'0')='1' and (a.enrollment_card_date between '" + fromDt + "' and '" + toDt + "') order by ST_Rank ";

                    selQ = " select a.app_formno as [Application Number],a.stud_name as [Student Name],a.Student_Mobile as [Mobile No],a.StuPer_Id as [Email ID],convert(varchar(10),d.SlotDate ,103) as [Date],d.SlotTime as [Session Time],rt.ST_Rank as [Rank] from applyn a ,ST_RankTable rt,ST_DaySlot D,ST_RankListSlot SR,ST_StudentSession ss where a.app_no=rt.ST_AppNo AND SS.ST_App_No =a.app_no and ss.ST_RankListFk =sr.ST_RankListPk and sr.ST_DaySlotFk =d.ST_DaySlotPK and SR.criteriaCode=rt.ST_RankCriteria and rt.ST_Stream =sr.streamCode  and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria in ('" + criteriaCode + "') and isnull(a.selection_status,'0') ='0' and ISNULL(IsConfirm,'0')='1'  and (d.SlotDate between '" + fromDt + "' and '" + toDt + "')  " + sessionVal + " order by ST_Rank ";

                    break;
                case 3://Verified
                    //selQ = "select a.app_formno as [Application Number],a.stud_name as [Student Name],a.Student_Mobile as [Mobile No],a.StuPer_Id as [Email ID],rt.ST_Rank as [Rank],convert(varchar(10),a.enrollment_card_date ,103) as [Date],a.enrollment_session as [Session Time] from applyn a ,ST_RankTable rt where a.app_no=rt.ST_AppNo " + sessionVal + " and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria='" + criteriaCode + "' and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and ISNULL(a.admission_status,'0')='1' and (a.enrollment_card_date between '" + fromDt + "' and '" + toDt + "') order by ST_Rank ";
                    selQ = " select a.app_no, a.app_formno as [Application Number],a.stud_name as [Student Name],a.Student_Mobile as [Mobile No],a.StuPer_Id as [Email ID],convert(varchar(10),d.SlotDate ,103) as [Date],d.SlotTime as [Session Time],rt.ST_Rank as [Rank] from applyn a ,ST_RankTable rt,ST_DaySlot D,ST_RankListSlot SR,ST_StudentSession ss where a.app_no=rt.ST_AppNo AND SS.ST_App_No =a.app_no and ss.ST_RankListFk =sr.ST_RankListPk and sr.ST_DaySlotFk =d.ST_DaySlotPK and SR.criteriaCode=rt.ST_RankCriteria and rt.ST_Stream =sr.streamCode  and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria in ('" + criteriaCode + "') and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and ISNULL(a.admission_status,'0')='1'  and (d.SlotDate between '" + fromDt + "' and '" + toDt + "')  " + sessionVal + " order by ST_Rank ";

                    break;
                case 4://Not Verified
                    //selQ = "select a.app_formno as [Application Number],a.stud_name as [Student Name],a.Student_Mobile as [Mobile No],a.StuPer_Id as [Email ID],rt.ST_Rank as [Rank],convert(varchar(10),a.enrollment_card_date ,103) as [Date],a.enrollment_session as [Session Time] from applyn a ,ST_RankTable rt where a.app_no=rt.ST_AppNo " + sessionVal + " and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria='" + criteriaCode + "' and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and ISNULL(a.admission_status,'0')='0' and (a.enrollment_card_date between '" + fromDt + "' and '" + toDt + "') order by ST_Rank ";
                    selQ = " select a.app_no, a.app_formno as [Application Number],a.stud_name as [Student Name],a.Student_Mobile as [Mobile No],a.StuPer_Id as [Email ID],convert(varchar(10),d.SlotDate ,103) as [Date],d.SlotTime as [Session Time],rt.ST_Rank as [Rank] from applyn a ,ST_RankTable rt,ST_DaySlot D,ST_RankListSlot SR,ST_StudentSession ss where a.app_no=rt.ST_AppNo AND SS.ST_App_No =a.app_no and ss.ST_RankListFk =sr.ST_RankListPk and sr.ST_DaySlotFk =d.ST_DaySlotPK and SR.criteriaCode=rt.ST_RankCriteria and rt.ST_Stream =sr.streamCode  and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria in ('" + criteriaCode + "') and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and ISNULL(a.admission_status,'0')='0'  and (d.SlotDate between '" + fromDt + "' and '" + toDt + "')  " + sessionVal + " order by ST_Rank ";

                    break;
                case 5://Admitted
                    //selQ = "select a.app_formno as [Application Number],a.stud_name as [Student Name],(select (c.course_Name+'-'+dt.dept_acronym) from degree d,course c,department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code and a.degree_code=d.degree_code and r.degree_code=d.degree_code and d.college_code='" + collegeCode + "') as [Department],(select masterValue from co_mastervalues mv where mv.mastercode=a.quota and mastercriteria='StudRankCriteria' and quota is not null and mv.collegecode='" + collegeCode + "') as [Quota],a.Student_Mobile as [Mobile No],a.StuPer_Id as [Email ID],rt.ST_Rank as [Rank] from applyn a ,ST_RankTable rt,Registration r where a.app_no=rt.ST_AppNo  and r.App_No=a.app_no " + sessionVal + " and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria='" + criteriaCode + "' and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and ISNULL(a.admission_status,'0')='1' and (a.enrollment_card_date between '" + fromDt + "' and '" + toDt + "') order by ST_Rank ";

                    selQ = " select a.app_formno as [Application Number],a.stud_name as [Student Name],(select (c.course_Name+'-'+dt.dept_acronym) from degree d,course c,department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code and a.degree_code=d.degree_code and r.degree_code=d.degree_code and d.college_code='" + collegeCode + "') as [Department],(select masterValue from co_mastervalues mv where mv.mastercode=a.quota and mastercriteria='StudRankCriteria' and quota is not null and mv.collegecode='" + collegeCode + "') as [Quota],a.Student_Mobile as [Mobile No],a.StuPer_Id as [Email ID],rt.ST_Rank as [Rank] from applyn a ,ST_RankTable rt,Registration r,ST_DaySlot d,ST_RankListSlot sr,ST_StudentSession ss where a.app_no=rt.ST_AppNo and ss.ST_App_No =a.app_no and ss.ST_App_No =r.App_No and ss.ST_App_No =rt.ST_AppNo and d.ST_DaySlotPK =sr.ST_DaySlotFk and sr.ST_RankListPk =ss.ST_RankListFk and rt.ST_RankCriteria =sr.criteriaCode and sr.streamCode =rt.ST_Stream  and r.App_No=a.app_no  and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria in ('" + criteriaCode + "') and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and ISNULL(a.admission_status,'0')='1' and SlotDate between '" + fromDt + "' and '" + toDt + "'  " + sessionVal + "  and Adm_Date between '" + fromDt + "' and '" + toDt + "'   order by ST_Rank";


                    //,convert(varchar(10),a.enrollment_card_date ,103) as [Date],a.enrollment_session as [Session Time]
                    break;
                case 6://Admitted count
                    //selQ = "select dt.Dept_Name,d.degree_code,Count(a.app_no) as Total from applyn a ,ST_RankTable rt,Registration r,Degree d,Department dt where r.degree_code=d.Degree_Code and d.Dept_Code=dt.Dept_Code and a.app_no=rt.ST_AppNo  and r.App_No=a.app_no " + sessionVal + "  and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria='" + criteriaCode + "' and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and ISNULL(a.admission_status,'0')='1' and (a.enrollment_card_date between '" + fromDt + "' and '" + toDt + "') group by dt.Dept_Name,d.degree_code order by dt.Dept_Name";

                    selQ = "select dt.Dept_Name,d.degree_code,Count(a.app_no) as Total from applyn a ,ST_RankTable rt,Registration r,Degree d,Department dt,ST_DaySlot sd,ST_RankListSlot st,ST_StudentSession ss where r.degree_code=d.Degree_Code and d.Dept_Code=dt.Dept_Code and a.app_no=rt.ST_AppNo  and r.App_No=a.app_no and sd.ST_DaySlotPK = st.ST_DaySlotFk and st.ST_RankListPk =ss.ST_RankListFk and ss.ST_App_No =r.App_No and ss.ST_App_No =a.app_no and ss.ST_App_No =rt.ST_AppNo and st.streamCode=rt.ST_Stream and rt.ST_RankCriteria =st.criteriaCode and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria in ('" + criteriaCode + "') and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and ISNULL(a.admission_status,'0')='1' and sd.SlotDate between '" + fromDt + "' and '" + toDt + "'  " + sessionVal + "  group by dt.Dept_Name,d.degree_code order by dt.Dept_Name";

                    selQ += "  select dt.Dept_Name,d.degree_code,Count(a.app_no) as Total,(select masterValue from co_mastervalues mv where mv.mastercode=a.quota and mastercriteria='StudRankCriteria' and quota is not null and mv.collegecode='13') as [Quota] from applyn a ,ST_RankTable rt,Registration r,Degree d,Department dt,ST_DaySlot sd,ST_RankListSlot st,ST_StudentSession ss where r.degree_code=d.Degree_Code and d.Dept_Code=dt.Dept_Code and a.app_no=rt.ST_AppNo  and r.App_No=a.app_no and sd.ST_DaySlotPK = st.ST_DaySlotFk and st.ST_RankListPk =ss.ST_RankListFk and ss.ST_App_No =r.App_No and ss.ST_App_No =a.app_no and ss.ST_App_No =rt.ST_AppNo and st.streamCode=rt.ST_Stream and rt.ST_RankCriteria =st.criteriaCode  and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria in ('" + criteriaCode + "') and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and ISNULL(a.admission_status,'0')='1' and sd.SlotDate between '" + fromDt + "' and '" + toDt + "'  " + sessionVal + " group by dt.Dept_Name,d.degree_code,a.quota order by dt.Dept_Name";
                    break;
                case 7://Admitted Count with session
                    selQ = "select dt.Dept_Name,d.degree_code,Count(a.app_no) as Total,SlotDate as dbDate ,SlotTime as [Session Time],CONVERT(varchar(10),slotdate,103) as [Date] from applyn a ,ST_RankTable rt,Registration r,Degree d,Department dt,ST_DaySlot sd,ST_RankListSlot st,ST_StudentSession ss where r.degree_code=d.Degree_Code and d.Dept_Code=dt.Dept_Code and a.app_no=rt.ST_AppNo  and r.App_No=a.app_no and sd.ST_DaySlotPK = st.ST_DaySlotFk and st.ST_RankListPk =ss.ST_RankListFk and ss.ST_App_No =r.App_No and ss.ST_App_No =a.app_no and ss.ST_App_No =rt.ST_AppNo and st.streamCode=rt.ST_Stream and rt.ST_RankCriteria =st.criteriaCode and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria in ('" + criteriaCode + "') and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and ISNULL(a.admission_status,'0')='1' and sd.SlotDate between '" + fromDt + "' and '" + toDt + "'  " + sessionVal + "  group by dt.Dept_Name,d.degree_code,SlotDate,SlotTime order by dt.Dept_Name";

                    selQ += "  select dt.Dept_Name,d.degree_code,Count(a.app_no) as Total,(select masterValue from co_mastervalues mv where mv.mastercode=a.quota and mastercriteria='StudRankCriteria' and quota is not null and mv.collegecode='" + collegeCode + "') as [Quota],SlotDate as dbDate ,SlotTime,CONVERT(varchar(10),slotdate,103) as [Date] from applyn a ,ST_RankTable rt,Registration r,Degree d,Department dt,ST_DaySlot sd,ST_RankListSlot st,ST_StudentSession ss where r.degree_code=d.Degree_Code and d.Dept_Code=dt.Dept_Code and a.app_no=rt.ST_AppNo  and r.App_No=a.app_no and sd.ST_DaySlotPK = st.ST_DaySlotFk and st.ST_RankListPk =ss.ST_RankListFk and ss.ST_App_No =r.App_No and ss.ST_App_No =a.app_no and ss.ST_App_No =rt.ST_AppNo and st.streamCode=rt.ST_Stream and rt.ST_RankCriteria =st.criteriaCode  and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria in ('" + criteriaCode + "') and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and ISNULL(a.admission_status,'0')='1' and sd.SlotDate between '" + fromDt + "' and '" + toDt + "'  " + sessionVal + " group by dt.Dept_Name,d.degree_code,a.quota,SlotDate,SlotTime order by dt.Dept_Name";
                    break;
                case 8://Hostel Registered
                    selQ = "  select a.app_formno as [Application Number],a.stud_name as [Student Name],dt.Dept_Name as Branch,a.Student_Mobile as [Mobile No],a.StuPer_Id as [Email ID],rt.ST_Rank as [Rank],r.Stud_Type as [Student Type] from applyn a ,ST_RankTable rt,Registration r,Degree d, Department dt,ST_DaySlot sd,ST_RankListSlot st,ST_StudentSession ss  where d.Dept_Code=dt.Dept_Code and r.degree_code=d.Degree_Code and a.app_no=rt.ST_AppNo  and r.App_No=a.app_no and sd.ST_DaySlotPK = st.ST_DaySlotFk and st.ST_RankListPk =ss.ST_RankListFk and ss.ST_App_No =r.App_No and ss.ST_App_No =a.app_no and ss.ST_App_No =rt.ST_AppNo and st.streamCode=rt.ST_Stream and rt.ST_RankCriteria =st.criteriaCode  and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria in ('" + criteriaCode + "') and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and ISNULL(a.admission_status,'0')='1' and ISNULL(r.stud_type,'')='Hostler' " + Gender + " and sd.SlotDate between '" + fromDt + "' and '" + toDt + "'  " + sessionVal + "  order by ST_Rank ";
                    break;
                case 9://Transport Registered
                    selQ = "select a.app_formno as [Application Number],a.stud_name as [Student Name],dt.Dept_Name as Branch,a.Student_Mobile as [Mobile No],a.StuPer_Id as [Email ID],rt.ST_Rank as [Rank],s.Stage_Name as Boarding from applyn a ,ST_RankTable rt,Registration r,Stage_Master s,Degree d, Department dt,ST_DaySlot sd,ST_RankListSlot st,ST_StudentSession ss  where d.Dept_Code=dt.Dept_Code and r.degree_code=d.Degree_Code and s.Stage_id=r.Boarding and  a.app_no=rt.ST_AppNo  and r.App_No=a.app_no and sd.ST_DaySlotPK = st.ST_DaySlotFk and st.ST_RankListPk =ss.ST_RankListFk and ss.ST_App_No =r.App_No and ss.ST_App_No =a.app_no and ss.ST_App_No =rt.ST_AppNo and st.streamCode=rt.ST_Stream and rt.ST_RankCriteria =st.criteriaCode  and a.college_code = '" + collegeCode + "' and a.batch_year='" + batchYear + "' and a.courseID='" + courseCode + "'  and rt.ST_RankCriteria in ('" + criteriaCode + "') and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and ISNULL(a.admission_status,'0')='1' and isnull(r.Boarding,'')<>'' and sd.SlotDate between '" + fromDt + "' and '" + toDt + "'  " + sessionVal + "  order by ST_Rank  ";
                    break;
            }
            if (reportType == 7)
            {
                DataSet dsReport = new DataSet();
                dsReport = dirAcc.selectDataSet(selQ);
                if (dsReport.Tables.Count == 2 && dsReport.Tables[0].Rows.Count > 0 && dsReport.Tables[1].Rows.Count > 0)
                {
                    DataTable dtReport = new DataTable();
                    dtReport = getAdmittedCountWithSession(dsReport);

                    gridReport.DataSource = dtReport;
                    gridReport.DataBind();
                    gridReport.Visible = true;

                    btnBasePrint.Visible = true;
                }
                else
                {
                    boolCheck = true;
                }
            }
            else if (reportType != 6 && reportType != 1)
            {
                DataTable dtReport = dirAcc.selectDataTable(selQ);
                if (dtReport.Rows.Count > 0)
                {
                    gridReport.DataSource = dtReport;
                    gridReport.DataBind();
                    gridReport.Visible = true;

                    btnBasePrint.Visible = true;
                }
                else
                    boolCheck = true;
            }
            else
            {
                DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
                if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0 && dsVal.Tables[1].Rows.Count > 0)
                {
                    if (reportType == 6)
                    {
                        getAdmitCntDet(dsVal);
                    }
                    else if (reportType == 1)
                    {
                        getAdmitCntDetNew(dsVal);
                    }
                }
                else
                    boolCheck = true;
            }
            if (boolCheck)
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Text = "No records found";
            }

        }
        catch
        {
            divPopupAlert.Visible = true;
            lblAlertMsg.Text = "Please try later";
        }
    }
    protected void getAdmitCntDet(DataSet dsVal)
    {
        try
        {
            DataTable dtStud = new DataTable();
            dtStud.Columns.Add("Dept Name");
            dtStud.Columns.Add("Total");
            DataRow drStud;
            bool boolCol = false;
            for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
            {
                drStud = dtStud.NewRow();
                drStud["Dept Name"] = Convert.ToString(dsVal.Tables[0].Rows[row]["Dept_Name"]);
                drStud["Total"] = Convert.ToString(dsVal.Tables[0].Rows[row]["Total"]);
                if (dsVal.Tables[1].Rows.Count > 0)
                {
                    dsVal.Tables[1].DefaultView.RowFilter = "degree_code='" + Convert.ToString(dsVal.Tables[0].Rows[row]["degree_code"]) + "'";
                    DataTable dtQuota = dsVal.Tables[1].DefaultView.ToTable();
                    if (dtQuota.Rows.Count > 0)
                    {
                        for (int col = 0; col < dtQuota.Rows.Count; col++)
                        {
                            if (!boolCol)
                                dtStud.Columns.Add(Convert.ToString(dtQuota.Rows[col][3]));
                            drStud[Convert.ToString(dtQuota.Rows[col][3])] = Convert.ToString(dtQuota.Rows[col][2]);
                        }
                        boolCol = true;
                    }
                }
                dtStud.Rows.Add(drStud);
            }
            if (dtStud.Rows.Count > 0)
            {
                gridReport.DataSource = dtStud;
                gridReport.DataBind();
                gridReport.Visible = true;

                btnBasePrint.Visible = true;
            }
        }
        catch { }
    }

    protected void getAdmitCntDetNew(DataSet dsVal)
    {
        try
        {
            DataTable dtStud = new DataTable();

            dtStud.Columns.Add("Application No");
            dtStud.Columns.Add("Student Name");
            dtStud.Columns.Add("HSC Mark");
            if (ddlStream.SelectedItem.Text == "Stream I")
            {
                if (ddlCourse.SelectedItem.Text == "LAW")
                {
                    dtStud.Columns.Add("CLAT Score");
                }
                else
                {
                    dtStud.Columns.Add("JEE Score");
                }
                dtStud.Columns.Add("Combined Score");
            }
            else if (ddlStream.SelectedItem.Text == "Stream II")
            {
                //dtStud.Columns.Add("JEE Score");
                dtStud.Columns.Add("Normalized Percentile");
            }
            DataRow drStud;
            bool boolCol = false;

            DataTable dv = dsVal.Tables[1].DefaultView.ToTable(true, "ST_RankCriteria", "Criteria");
            if (dv.Rows.Count > 0)
            {
                for (int indt = 0; indt < dv.Rows.Count; indt++)
                {
                    dtStud.Columns.Add(Convert.ToString(dv.Rows[indt]["Criteria"]));
                }
            }
            // dtStud.Columns.Add(Convert.ToString(dtQuota.Rows[col][3]));
            for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
            {
                drStud = dtStud.NewRow();
                drStud["Application No"] = Convert.ToString(dsVal.Tables[0].Rows[row]["Application Number"]);
                drStud["Student Name"] = Convert.ToString(dsVal.Tables[0].Rows[row]["Student Name"]);
                if (dsVal.Tables[1].Rows.Count > 0)
                {
                    dsVal.Tables[1].DefaultView.RowFilter = "ST_AppNo='" + Convert.ToString(dsVal.Tables[0].Rows[row]["app_no"]) + "'";
                    DataTable dtQuota = dsVal.Tables[1].DefaultView.ToTable();
                    if (dtQuota.Rows.Count > 0)
                    {
                        for (int col = 0; col < 1; col++)
                        {
                            drStud["HSC Mark"] = Convert.ToString(dtQuota.Rows[col]["HSCMarkSec"]);
                            int colIndex = 0;
                            if (ddlStream.SelectedItem.Text.Trim() != "Stream II")
                            {
                                drStud[3] = Convert.ToString(dtQuota.Rows[col]["jeeMarkSec"]);
                                drStud[4] = Convert.ToString(dtQuota.Rows[col]["CombinedScore"]);
                                colIndex = 5;
                            }
                            else
                            {
                                // drStud[3] = Convert.ToString(dtQuota.Rows[col]["jeeMarkSec"]);
                                drStud["Normalized Percentile"] = Convert.ToString(dtQuota.Rows[col]["CombinedScore"]);
                                colIndex = 4;
                            }
                            for (int indt = 0; indt < dv.Rows.Count; indt++)
                            {
                                dsVal.Tables[1].DefaultView.RowFilter = "ST_AppNo='" + Convert.ToString(dsVal.Tables[0].Rows[row]["app_no"]) + "' and ST_RankCriteria='" + Convert.ToString(dv.Rows[indt]["ST_RankCriteria"]) + "'";
                                DataView dsview = dsVal.Tables[1].DefaultView;
                                if (dsview.Count > 0)
                                {
                                    drStud[(indt + colIndex)] = Convert.ToString(dsview[0]["ST_Rank"]);
                                }
                                else
                                {
                                    drStud[(indt + colIndex)] = "-";
                                }
                            }
                        }
                        boolCol = true;
                    }
                }
                dtStud.Rows.Add(drStud);
            }
            if (dtStud.Rows.Count > 0)
            {
                gridReport.DataSource = dtStud;
                gridReport.DataBind();
                gridReport.Visible = true;
                if (gridReport.Columns.Count > 0)
                {
                    //if (ddlStream.SelectedItem.Text.Trim() != "Steam I")
                    //{
                    //    gridReport.Columns[4].Visible = false;
                    //}
                }

                btnBasePrint.Visible = true;
            }
        }
        catch { }
    }

    protected void gridReport_OnDataBound(object sender, EventArgs e)
    {

    }
    protected void gridReport_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (reportValue == 6)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int Count = e.Row.Cells.Count;
                for (int row = 2; row < Count; row++)
                {
                    e.Row.Cells[row].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        else if (ddlReportType.SelectedIndex == 7)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int coli = 4; coli < e.Row.Cells.Count; coli++)
                {
                    e.Row.Cells[coli].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        else if (ddlReportType.SelectedIndex == 1)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int coli = 3; coli < e.Row.Cells.Count; coli++)
                {
                    e.Row.Cells[coli].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
    }
    private void clearReportGrid()
    {
        gridReport.DataSource = null;
        gridReport.DataBind();
        gridReport.Visible = false;

        btnBasePrint.Visible = false;
    }
    //Added by Idhris 30-05-2017
    private DataTable getAdmittedCountWithSession(DataSet dsReport)
    {
        DataTable dtReport = new DataTable();
        try
        {
            DataTable dtCriteria = dsReport.Tables[1].DefaultView.ToTable(true, "Quota");
            if (dtCriteria.Rows.Count > 0)
            {
                dtReport.Columns.Add("Branch");
                dtReport.Columns.Add("Date");
                dtReport.Columns.Add("Session Time");
                dtReport.Columns.Add("Total");
                foreach (DataRow drCrit in dtCriteria.Rows)
                {
                    dtReport.Columns.Add(drCrit["Quota"].ToString());
                }

                foreach (DataRow drReport in dsReport.Tables[0].Rows)
                {
                    string dbDate = Convert.ToString(drReport["dbDate"]);
                    string dispDate = Convert.ToString(drReport["Date"]);
                    string session = Convert.ToString(drReport["Session Time"]);
                    string branch = Convert.ToString(drReport["Dept_Name"]);
                    string degree_code = Convert.ToString(drReport["degree_code"]);
                    string total = Convert.ToString(drReport["Total"]);

                    DataRow drRep = dtReport.NewRow();
                    drRep["Branch"] = branch;
                    drRep["Date"] = dispDate;
                    drRep["Session Time"] = session;
                    drRep["Total"] = total;

                    for (int colI = 4; colI < dtReport.Columns.Count; colI++)
                    {
                        string critName = dtReport.Columns[colI].ColumnName;
                        dsReport.Tables[1].DefaultView.RowFilter = "degree_code='" + degree_code + "' and [Quota]='" + critName + "' and dbDate='" + dbDate + "' and SlotTime='" + session + "'";
                        DataTable dtCritTot = dsReport.Tables[1].DefaultView.ToTable();
                        if (dtCritTot.Rows.Count > 0)
                        {
                            drRep[critName] = Convert.ToString(dtCritTot.Rows[0]["Total"]);
                        }
                        else
                        {
                            drRep[critName] = 0;
                        }
                    }

                    dtReport.Rows.Add(drRep);
                }
            }
        }
        catch { dtReport.Clear(); }
        return dtReport;
    }
    #endregion Show Report

    #endregion

}
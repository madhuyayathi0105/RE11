using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Threading;
using System.Globalization;

public partial class AdmissionMod_StudentsAdmissionSelectionStatusReport : System.Web.UI.Page
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
    string sessionName = string.Empty;
    string fromDate = string.Empty;
    string toDate = string.Empty;

    DateTime dtFromDate = new DateTime();
    DateTime dtToDate = new DateTime();

    bool isSchool = false;
    int selected = 0;

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

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
                divMainContent.Visible = false;
                btnPrint.Visible = false;
                divSession.Visible = false;
                if (ddlReportType.Items.Count > 0)
                    ddlReportType.SelectedIndex = 1;
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
            }
        }
        catch (ThreadAbortException tt)
        {

        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            divMainContent.Visible = false;
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
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void BindBatch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContent.Visible = false;
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
                Control c = ddlCollege;
                if (c is CheckBoxList)
                {
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
                }
                else if (c is DropDownList)
                {
                    selected++;
                    collegeCodeNew = "'" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'";
                }
                if (!string.IsNullOrEmpty(collegeCodeNew) && selected > 0)
                {
                    qryCollegeCode = " and r.college_code in(" + collegeCodeNew + ")";
                }
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
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void BindGraduate()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContent.Visible = false;
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
                Control c = ddlCollege;
                if (c is CheckBoxList)
                {
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
                }
                else if (c is DropDownList)
                {
                    selected++;
                    collegeCodeNew = "'" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'";
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
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void BindCourse()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContent.Visible = false;
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
                Control c = ddlCollege;
                if (c is CheckBoxList)
                {
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
                }
                else if (c is DropDownList)
                {
                    selected++;
                    collegeCodeNew = "'" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'";
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
                Control c = ddlEduLevel;
                if (c is CheckBoxList)
                {
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
                }
                else if (c is DropDownList)
                {
                    selected++;
                    graduate = "'" + Convert.ToString(ddlEduLevel.SelectedValue).Trim() + "'";
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
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void BindStream()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContent.Visible = false;
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
                Control c = ddlCollege;
                if (c is CheckBoxList)
                {
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
                }
                else if (c is DropDownList)
                {
                    selected++;
                    collegeCodeNew = "'" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'";
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
                ddlStream.Items.Insert(0, new ListItem("ALL", "ALL"));
                ddlStream.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void BindSession()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContent.Visible = false;
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
                Control c = ddlCollege;
                if (c is CheckBoxList)
                {
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
                }
                else if (c is DropDownList)
                {
                    selected++;
                    collegeCodeNew = "'" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'";
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
                Control c = ddlBatch;
                if (c is CheckBoxList)
                {
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
                }
                else if (c is DropDownList)
                {
                    selected++;
                    batchYear = "'" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "'";
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
                Control c = ddlEduLevel;
                if (c is CheckBoxList)
                {
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
                }
                else if (c is DropDownList)
                {
                    selected++;
                    graduate = "'" + Convert.ToString(ddlEduLevel.SelectedValue).Trim() + "'";
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
                Control c = ddlCollege;
                if (c is CheckBoxList)
                {
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
                }
                else if (c is DropDownList)
                {
                    selected++;
                    courseId = "'" + Convert.ToString(ddlCourse.SelectedValue).Trim() + "'";
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
                ddlSession.Items.Insert(0, new ListItem("ALL", "ALL"));
                ddlSession.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            divMainContent.Visible = false;
            btnPrint.Visible = false;
            BindBatch();
            BindGraduate();
            BindCourse();
            BindStream();
            BindSession();
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            divMainContent.Visible = false;
            btnPrint.Visible = false;
            BindCourse();
            BindStream();
            BindSession();
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            divMainContent.Visible = false;
            btnPrint.Visible = false;
            BindStream();
            BindSession();
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            divMainContent.Visible = false;
            btnPrint.Visible = false;
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            divMainContent.Visible = false;
            btnPrint.Visible = false;
            divSession.Visible = false;
            if (ddlReportType.Items.Count > 0)
            {
                //if (ddlReportType.SelectedIndex == 0)
                //{
                //    divSession.Visible = false;
                //}
                //else if (ddlReportType.SelectedIndex == 2)
                //{
                //    divSession.Visible = true;
                //}
                int index = ddlReportType.SelectedIndex;
                switch (index)
                {
                    case 0: break;
                    case 1: break;
                    case 2: divSession.Visible = true; break;
                }
            }
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            divMainContent.Visible = false;
            btnPrint.Visible = false;
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContent.Visible = false;
            btnPrint.Visible = false;
            BindSession();
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContent.Visible = false;
            btnPrint.Visible = false;
            BindSession();
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Close Popup

    #region GO

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContent.Visible = false;
            btnPrint.Visible = false;
            qry = string.Empty;
            qryCollegeCode = string.Empty;
            qryGraduate = string.Empty;
            qryBatch = string.Empty;
            qryCourse = string.Empty;
            qryDate = string.Empty;
            qrySession = string.Empty;
            qryStream = string.Empty;

            collegeCode = string.Empty;
            batchYear = string.Empty;
            graduate = string.Empty;
            courseId = string.Empty;
            courseName = string.Empty;
            streamName = string.Empty;
            streamCode = string.Empty;
            sessionName = string.Empty;
            fromDate = string.Empty;
            toDate = string.Empty;
            bool isDateOrSessionWise = true;
            string filterStream = string.Empty;
            if (ddlReportType.Items.Count > 0)
            {
                if (ddlReportType.SelectedIndex != 0)
                    isDateOrSessionWise = false;
            }
            if (ddlCollege.Items.Count > 0)
            {
                selected = 0;
                collegeCode = string.Empty;
                Control c = ddlCollege;
                if (c is CheckBoxList)
                {
                    foreach (ListItem li in ddlCollege.Items)
                    {
                        if (li.Selected)
                        {
                            selected++;
                            if (string.IsNullOrEmpty(collegeCode.Trim()))
                            {
                                collegeCode = "'" + li.Value.Trim() + "'";
                            }
                            else
                            {
                                collegeCode += ",'" + li.Value.Trim() + "'";
                            }
                        }
                    }
                }
                else if (c is DropDownList)
                {
                    selected++;
                    collegeCode = "'" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'";
                }
                if (!string.IsNullOrEmpty(collegeCode) && selected > 0)
                {
                    qryCollegeCode = " and a.College_Code in(" + collegeCode + ")";
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblCollege.Text + " were Found";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }

            if (ddlBatch.Items.Count > 0)
            {
                selected = 0;
                batchYear = string.Empty;
                Control c = ddlBatch;
                if (c is CheckBoxList)
                {
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
                }
                else if (c is DropDownList)
                {
                    selected++;
                    batchYear = "'" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "'";
                }
                if (!string.IsNullOrEmpty(batchYear) && selected > 0)
                {
                    qryBatch = " and a.Batch_Year in(" + batchYear + ")";
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblBatch.Text + " were Found";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }

            if (ddlEduLevel.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblEduLevel.Text + " were Found";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }

            if (ddlCourse.Items.Count > 0)
            {
                selected = 0;
                qryCourse = string.Empty;
                courseId = string.Empty;
                Control c = ddlCourse;
                if (c is CheckBoxList)
                {
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
                }
                else if (c is DropDownList)
                {
                    selected++;
                    courseId = "'" + Convert.ToString(ddlCourse.SelectedValue).Trim() + "'";
                }
                if (!string.IsNullOrEmpty(courseId) && selected > 0)
                {
                    qryCourse = " and a.courseId in(" + courseId + ")";
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblCourse.Text + " were Found";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }

            if (ddlStream.Items.Count > 0)
            {
                selected = 0;
                qryStream = string.Empty;
                streamCode = string.Empty;
                streamName = string.Empty;
                Control c = ddlStream;
                if (c is CheckBoxList)
                {
                    foreach (ListItem li in ddlStream.Items)
                    {
                        if (li.Selected)
                        {
                            selected++;
                            if (Convert.ToString(li.Value).Trim().ToLower() != "all" && Convert.ToString(li.Value).Trim().ToLower() != "")
                            {
                                if (string.IsNullOrEmpty(streamCode.Trim()))
                                {
                                    streamCode = "'" + li.Value.Trim() + "'";
                                }
                                else
                                {
                                    streamCode += ",'" + li.Value.Trim() + "'";
                                }
                            }
                        }
                    }
                }
                else if (c is DropDownList)
                {
                    if (Convert.ToString(ddlStream.SelectedValue).Trim().ToLower() != "all" && Convert.ToString(ddlStream.SelectedValue).Trim().ToLower() != "")
                    {
                        selected++;
                        streamCode = "'" + Convert.ToString(ddlStream.SelectedValue).Trim() + "'";
                    }
                }
                if (!string.IsNullOrEmpty(streamCode) && selected > 0)
                {
                    qryStream = " and sc.Category_Code in(" + streamCode + ")";
                    filterStream = " and Category_Code in(" + streamCode + ")";
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblStream.Text + " were Found";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            fromDate = Convert.ToString(txtFromDate.Text).Trim();
            toDate = Convert.ToString(txtToDate.Text).Trim();
            dtFromDate = new DateTime();
            dtToDate = new DateTime();
            if (!string.IsNullOrEmpty(fromDate))
            {
                bool isFValid = DateTime.TryParseExact(fromDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
                qryDate = " and a.enrollment_card_date>='" + dtFromDate.ToShortDateString() + "'";
                if (!isFValid)
                {
                    lblAlertMsg.Text = "Please Select Valid " + lblFromDate.Text;
                    lblAlertMsg.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "Please Select " + lblFromDate.Text;
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                bool isTValid = DateTime.TryParseExact(toDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
                qryDate += "and a.enrollment_card_date<='" + dtToDate.ToShortDateString() + "'";
                if (!isTValid)
                {
                    lblAlertMsg.Text = "Please Select Valid " + lblToDate.Text;
                    lblAlertMsg.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "Please Select " + lblToDate.Text;
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (dtFromDate > dtToDate)
            {
                lblAlertMsg.Text = "From Date Must Be Less Than To Date";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (!isDateOrSessionWise && ddlReportType.SelectedIndex == 2)
            {
                if (ddlSession.Items.Count > 0)
                {
                    selected = 0;
                    qrySession = string.Empty;
                    sessionName = string.Empty;
                    Control c = ddlSession;
                    if (c is CheckBoxList)
                    {
                        foreach (ListItem li in ddlSession.Items)
                        {
                            if (li.Selected)
                            {
                                selected++;
                                if (Convert.ToString(li.Value).Trim().ToLower() != "all" && Convert.ToString(li.Value).Trim().ToLower() != "")
                                {
                                    if (string.IsNullOrEmpty(sessionName.Trim()))
                                    {
                                        sessionName = "'" + li.Value.Trim() + "'";
                                    }
                                    else
                                    {
                                        sessionName += ",'" + li.Value.Trim() + "'";
                                    }
                                }
                            }
                        }
                    }
                    else if (c is DropDownList)
                    {
                        if (Convert.ToString(ddlSession.SelectedValue).Trim().ToLower() != "all" && Convert.ToString(ddlSession.SelectedValue).Trim().ToLower() != "")
                        {
                            selected++;
                            sessionName = "'" + Convert.ToString(ddlSession.SelectedValue).Trim() + "'";
                        }
                    }
                    if (!string.IsNullOrEmpty(sessionName) && selected > 0)
                    {
                        qrySession = " and a.enrollment_session in(" + sessionName + ")";
                    }
                }
                else
                {
                    lblAlertMsg.Text = "No " + lblSession.Text + " were Found";
                    lblAlertMsg.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
            }

            DataTable dtCourseDet = new DataTable();
            dtCourseDet.Columns.Add("courseID", typeof(int));
            dtCourseDet.Columns.Add("CourseName", typeof(string));
            dtCourseDet.Columns.Add("DegreeName", typeof(string));
            dtCourseDet.Columns.Add("DegreeCode", typeof(string));
            dtCourseDet.Columns.Add("EnrollDate", typeof(string));
            dtCourseDet.Columns.Add("enrollSession", typeof(string));
            dtCourseDet.Columns.Add("Priority", typeof(string));
            dtCourseDet.Columns.Add("totSeats", typeof(int));
            dtCourseDet.Columns.Add("calledStudent", typeof(int));
            dtCourseDet.Columns.Add("registered", typeof(int));
            dtCourseDet.Columns.Add("verified", typeof(int));
            dtCourseDet.Columns.Add("admited", typeof(int));
            dtCourseDet.Columns.Add("hostel", typeof(int));
            dtCourseDet.Columns.Add("transport", typeof(int));

            DataSet dsCourseDet = new DataSet();
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(qryCourse) && !string.IsNullOrEmpty(qryDate) && !string.IsNullOrEmpty(collegeCode))
            {
                qry = "select distinct c.Course_Id,c.Course_Name,dt.Dept_Name,dt.dept_acronym,dg.Degree_Code,Convert(varchar(20),a.enrollment_card_date,103) as enrollment_card_date,a.enrollment_session,Isnull(c.Priority,'0') Priority,sc.Category_Code,sc.Category_Name,isnull(sc.NoOfSeats,'0') NoOfSeats from Degree dg,Course c,Department dt,applyn a,seattype_cat sc where sc.Degree_Code=dg.Degree_Code and sc.Batch_Year=a.batch_year and sc.collegeCode=a.college_code and  a.courseID=c.Course_Id and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and c.Course_Id in(" + courseId + ") and a.batch_year in(" + batchYear + ") and c.college_code in(" + collegeCode + ") " + qryDate + qrySession + qryStream + " order by Priority,c.Course_Id,enrollment_card_date,a.enrollment_session,dg.Degree_Code,sc.Category_Code";
                dsCourseDet.Clear();
                dsCourseDet = d2.select_method_wo_parameter(qry, "text");

                DataSet dsAdmissionDetails = new DataSet();
                qry = " select distinct a.courseID,Convert(varchar(20),a.enrollment_card_date,103) as enrollment_card_date,a.enrollment_session,sc.Category_Code,sc.Category_Name,Count(Distinct a.app_no) as CALLED from applyn a,seattype_cat sc where sc.Batch_Year=a.batch_year and sc.collegeCode=a.college_code and a.college_code in(" + collegeCode + ") and a.batch_year in(" + batchYear + ") and a.courseID in(" + courseId + ") " + qryDate + qrySession + qryStream + " group by a.courseID,a.enrollment_card_date,a.enrollment_session,sc.Category_Code,sc.Category_Name order by a.courseID ;";
                qry += " select distinct a.courseID,Convert(varchar(20),a.enrollment_card_date,103) as enrollment_card_date,a.enrollment_session,sc.Category_Code,sc.Category_Name,Count(Distinct a.app_no) as Registered from applyn a ,seattype_cat sc where sc.Batch_Year=a.batch_year and sc.collegeCode=a.college_code and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and a.college_code in(" + collegeCode + ") and a.batch_year in(" + batchYear + ") and a.courseID in(" + courseId + ") " + qryDate + qrySession + qryStream + " group by a.courseID,a.enrollment_card_date,a.enrollment_session,sc.Category_Code,sc.Category_Name order by a.courseID  ; ";

                qry += "select distinct a.courseID,Convert(varchar(20),a.enrollment_card_date,103) as enrollment_card_date,a.enrollment_session,sc.Category_Code,sc.Category_Name,Count(Distinct a.app_no) as Verified from applyn a,seattype_cat sc where sc.Batch_Year=a.batch_year and sc.collegeCode=a.college_code and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and ISNULL(a.admission_status,'0')='1' and a.college_code in(" + collegeCode + ") and a.batch_year in(" + batchYear + ") and a.courseID in(" + courseId + ") " + qryDate + qrySession + qryStream + " group by a.courseID,a.enrollment_card_date,a.enrollment_session,sc.Category_Code,sc.Category_Name order by a.courseID ; ";

                qry += "select distinct sc.degree_code,a.courseID,Convert(varchar(20),a.enrollment_card_date,103) as enrollment_card_date,a.enrollment_session,sc.Category_Code,sc.Category_Name,Count(Distinct a.app_no) as Admitted from applyn a ,Registration r,seattype_cat sc where sc.Degree_Code=a.degree_code and r.degree_code=sc.Degree_Code and  sc.Batch_Year=a.batch_year and sc.collegeCode=a.college_code and r.App_No=a.app_no and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and ISNULL(a.admission_status,'0')='1' and a.college_code in(" + collegeCode + ") and a.batch_year in(" + batchYear + ") and a.courseID in(" + courseId + ") " + qryDate + qrySession + qryStream + " group by sc.degree_code,a.courseID,a.enrollment_card_date,a.enrollment_session,sc.Category_Code,sc.Category_Name order by a.courseID";

                qry += " select distinct sc.degree_code,a.courseID,Convert(varchar(20),a.enrollment_card_date,103) as enrollment_card_date,a.enrollment_session,sc.Category_Code,sc.Category_Name,Count(Distinct a.app_no) as Hostel from applyn a ,Registration r,HT_HostelRegistration ht,seattype_cat sc where sc.Degree_Code=a.degree_code and r.degree_code=sc.Degree_Code and  sc.Batch_Year=a.batch_year and sc.collegeCode=a.college_code and r.App_No=a.app_no  and ht.APP_No=a.app_no and r.App_no=ht.APP_No and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and ISNULL(a.admission_status,'0')='1' and a.college_code in(" + collegeCode + ") and a.batch_year in(" + batchYear + ") and a.courseID in(" + courseId + ") " + qryDate + qrySession + qryStream + " group by sc.degree_code,a.courseID,a.enrollment_card_date,a.enrollment_session,sc.Category_Code,sc.Category_Name order by sc.degree_code";

                qry += " select distinct sc.degree_code,a.courseID,Convert(varchar(20),a.enrollment_card_date,103) as enrollment_card_date,a.enrollment_session,sc.Category_Code,sc.Category_Name,Count(Distinct a.app_no) as Transport from applyn a ,Registration r,seattype_cat sc where sc.Degree_Code=a.degree_code and r.degree_code=sc.Degree_Code and sc.Batch_Year=a.batch_year and sc.collegeCode=a.college_code and r.App_No=a.app_no and isnull(a.selection_status,'0') ='1' and ISNULL(IsConfirm,'0')='1' and ISNULL(a.admission_status,'0')='1' and isnull(r.Boarding,'')<>'' and isnull(r.Bus_RouteID,'')<>'' and isnull(r.VehID,'')<>'' and a.college_code in(" + collegeCode + ") and a.batch_year in(" + batchYear + ") and a.courseID in(" + courseId + ") " + qryDate + qrySession + qryStream + " group by sc.degree_code,a.courseID,a.enrollment_card_date,a.enrollment_session,sc.Category_Code,sc.Category_Name order by sc.degree_code";
                dsAdmissionDetails = d2.select_method_wo_parameter(qry, "text");

                if (dsCourseDet.Tables.Count > 0 && dsCourseDet.Tables[0].Rows.Count > 0)
                {
                    DataTable dtNew = new DataTable();
                    DataRow drNew;
                    dsCourseDet.Tables[0].DefaultView.Sort = "Priority,Course_Id,enrollment_card_date,enrollment_session,Degree_Code";
                    string qryFilter1 = string.Empty;
                    string[] filter1 = new string[5] { "Course_Id", "Course_Name", "Dept_Name", "Degree_Code", "Priority" };//, "enrollment_card_date"
                    if (!isDateOrSessionWise)
                    {
                        if (ddlReportType.SelectedIndex == 2)
                        {
                            Array.Resize(ref filter1, filter1.Length + 2);
                            filter1[filter1.Length - 1] = "enrollment_session";
                            filter1[filter1.Length - 2] = "enrollment_card_date";
                        }
                    }
                    else
                    {
                        Array.Resize(ref filter1, filter1.Length + 2);
                        filter1[filter1.Length - 1] = "enrollment_session";
                        filter1[filter1.Length - 2] = "enrollment_card_date";
                    }
                    dtNew = dsCourseDet.Tables[0].DefaultView.ToTable(true, filter1);
                    foreach (DataRow dr in dtNew.Rows)
                    {
                        DataView dv = new DataView();
                        string courseIdVal = Convert.ToString(dr["Course_Id"]).Trim();
                        string courseNames = Convert.ToString(dr["Course_Name"]).Trim();
                        string enrollment_session = string.Empty;
                        string enrollmentDate = string.Empty;
                        if (!isDateOrSessionWise)
                        {
                            if (ddlReportType.SelectedIndex == 2)
                            {
                                enrollment_session = Convert.ToString(dr["enrollment_session"]).Trim();
                                enrollmentDate = Convert.ToString(dr["enrollment_card_date"]).Trim();
                            }
                        }
                        else
                        {
                            enrollmentDate = Convert.ToString(dr["enrollment_card_date"]).Trim();
                        }

                        string degCode = Convert.ToString(dr["Degree_Code"]).Trim();
                        string qryFilter = string.Empty;
                        string[] filter = new string[6] { "Course_Id", "Course_Name", "Priority", "Category_Code", "Category_Name", "NoOfSeats" };//"enrollment_card_date"
                        if (!isDateOrSessionWise)
                        {
                            if (ddlReportType.SelectedIndex == 2)
                            {
                                qryFilter = " and enrollment_card_date='" + Convert.ToString(dr["enrollment_card_date"]).Trim() + "' and enrollment_session='" + Convert.ToString(dr["enrollment_session"]).Trim() + "'";
                                Array.Resize(ref filter, filter.Length + 2);
                                filter[filter.Length - 1] = "enrollment_session";
                                filter[filter.Length - 2] = "enrollment_card_date";
                            }
                        }
                        else
                        {
                            qryFilter = " and enrollment_card_date='" + Convert.ToString(dr["enrollment_card_date"]).Trim() + "'";
                            Array.Resize(ref filter, filter.Length + 1);
                            filter[filter.Length - 1] = "enrollment_card_date";
                        }

                        dsCourseDet.Tables[0].DefaultView.RowFilter = "Course_Id='" + Convert.ToString(dr["Course_Id"]).Trim() + "' and Course_Name='" + Convert.ToString(dr["Course_Name"]).Trim() + "'  and Priority='" + Convert.ToString(dr["Priority"]).Trim() + "' and Degree_Code='" + degCode + "'  " + qryFilter;
                        dv = dsCourseDet.Tables[0].DefaultView;

                        DataTable dtCount = dv.ToTable(true, filter);
                        object noofseats = dv.ToTable(true, filter).Compute("sum(NoOfSeats)", "Course_Id='" + Convert.ToString(dr["Course_Id"]).Trim() + "' and Course_Name='" + Convert.ToString(dr["Course_Name"]).Trim() + "' and Priority='" + Convert.ToString(dr["Priority"]).Trim() + "'  " + qryFilter);//and enrollment_card_date='" + Convert.ToString(dr["enrollment_card_date"]).Trim() + "'
                        drNew = dtCourseDet.NewRow();
                        drNew["courseID"] = Convert.ToString(dr["Course_Id"]).Trim();
                        drNew["CourseName"] = Convert.ToString(dr["Course_Name"]).Trim();
                        drNew["DegreeCode"] = Convert.ToString(dr["Degree_Code"]).Trim();
                        drNew["DegreeName"] = Convert.ToString(dr["Dept_Name"]).Trim();
                        drNew["enrollSession"] = enrollment_session;
                        drNew["EnrollDate"] = enrollmentDate;
                        drNew["Priority"] = Convert.ToString(dr["Priority"]).Trim();
                        drNew["totSeats"] = Convert.ToString(noofseats).Trim();

                        int called = 0;
                        int registered = 0;
                        int verified = 0;
                        int admited = 0;
                        int hostel = 0;
                        int transport = 0;
                        DataView dvAD = new DataView();
                        if (dsAdmissionDetails.Tables.Count > 0 && dsAdmissionDetails.Tables[0].Rows.Count > 0)
                        {
                            dvAD = new DataView();
                            dsAdmissionDetails.Tables[0].DefaultView.RowFilter = "courseId='" + courseIdVal + "'  " + qryFilter + filterStream;//and enrollment_card_date='" + enrollmentDate + "'
                            dvAD = dsAdmissionDetails.Tables[0].DefaultView;
                            object cnt = dvAD.ToTable().Compute("sum(CALLED)", string.Empty);
                            int.TryParse(Convert.ToString(cnt).Trim(), out called);
                        }
                        if (dsAdmissionDetails.Tables.Count > 1 && dsAdmissionDetails.Tables[1].Rows.Count > 0)
                        {
                            dvAD = new DataView();
                            dsAdmissionDetails.Tables[1].DefaultView.RowFilter = "courseId='" + courseIdVal + "'  " + qryFilter + filterStream;//and enrollment_card_date='" + enrollmentDate + "'
                            dvAD = dsAdmissionDetails.Tables[1].DefaultView;
                            object cnt = dvAD.ToTable().Compute("sum(Registered)", string.Empty);
                            int.TryParse(Convert.ToString(cnt).Trim(), out registered);
                        }
                        if (dsAdmissionDetails.Tables.Count > 2 && dsAdmissionDetails.Tables[2].Rows.Count > 0)
                        {
                            dvAD = new DataView();
                            dsAdmissionDetails.Tables[2].DefaultView.RowFilter = "courseId='" + courseIdVal + "'  " + qryFilter + filterStream;//and enrollment_card_date='" + enrollmentDate + "'
                            dvAD = dsAdmissionDetails.Tables[2].DefaultView;
                            object cnt = dvAD.ToTable().Compute("sum(Verified)", string.Empty);
                            int.TryParse(Convert.ToString(cnt).Trim(), out verified);
                        }
                        if (dsAdmissionDetails.Tables.Count > 3 && dsAdmissionDetails.Tables[3].Rows.Count > 0)
                        {
                            dvAD = new DataView();
                            dsAdmissionDetails.Tables[3].DefaultView.RowFilter = "courseId='" + courseIdVal + "' and degree_code='" + degCode + "'  " + qryFilter + filterStream;//and enrollment_card_date='" + enrollmentDate + "'
                            dvAD = dsAdmissionDetails.Tables[3].DefaultView;
                            object cnt = dvAD.ToTable().Compute("sum(Admitted)", string.Empty);
                            int.TryParse(Convert.ToString(cnt).Trim(), out admited);
                        }
                        if (dsAdmissionDetails.Tables.Count > 4 && dsAdmissionDetails.Tables[4].Rows.Count > 0)
                        {
                            dvAD = new DataView();
                            dsAdmissionDetails.Tables[4].DefaultView.RowFilter = "courseId='" + courseIdVal + "' and degree_code='" + degCode + "' " + qryFilter + filterStream;// and enrollment_card_date='" + enrollmentDate + "' 
                            dvAD = dsAdmissionDetails.Tables[4].DefaultView;
                            object cnt = dvAD.ToTable().Compute("sum(Hostel)", string.Empty);
                            int.TryParse(Convert.ToString(cnt).Trim(), out hostel);
                        }
                        if (dsAdmissionDetails.Tables.Count > 5 && dsAdmissionDetails.Tables[5].Rows.Count > 0)
                        {
                            dvAD = new DataView();
                            dsAdmissionDetails.Tables[5].DefaultView.RowFilter = "courseId='" + courseIdVal + "' and degree_code='" + degCode + "' " + qryFilter + filterStream;//and enrollment_card_date='" + enrollmentDate + "'
                            dvAD = dsAdmissionDetails.Tables[5].DefaultView;
                            object cnt = dvAD.ToTable().Compute("sum(Transport)", string.Empty);
                            int.TryParse(Convert.ToString(cnt).Trim(), out transport);
                        }
                        drNew["calledStudent"] = Convert.ToString(called).Trim();
                        drNew["registered"] = Convert.ToString(registered).Trim();
                        drNew["verified"] = Convert.ToString(verified).Trim();
                        drNew["admited"] = Convert.ToString(admited).Trim();
                        drNew["hostel"] = Convert.ToString(hostel).Trim();
                        drNew["transport"] = Convert.ToString(transport).Trim();
                        dtCourseDet.Rows.Add(drNew);
                    }
                }
            }
            if (dtCourseDet.Rows.Count > 0)
            {
                gvAdmissionStatus.DataSource = dtCourseDet;
                gvAdmissionStatus.DataBind();
                btnPrint.Visible = true;
                divMainContent.Visible = true;
            }
            else
            {
                lblAlertMsg.Text = "No Record(s) were Found";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            ////d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

    #region Close Popup

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            ////d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Close Popup

    #endregion

    protected void gvAdmissionStatus_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gvAdmissionStatus.HeaderRow.Cells.Count > 0)
            {
                gvAdmissionStatus.HeaderRow.Cells[1].Visible = (ddlReportType.SelectedIndex != 1) ? true : false;
                gvAdmissionStatus.HeaderRow.Cells[2].Visible = (ddlReportType.SelectedIndex == 2) ? true : false;
                foreach (GridViewRow gvRow in gvAdmissionStatus.Rows)
                {
                    if (gvRow.Cells[1].RowSpan == 0)
                    {
                        gvRow.Cells[1].Visible = (ddlReportType.SelectedIndex != 1) ? true : false;
                        gvRow.Cells[2].Visible = (ddlReportType.SelectedIndex == 2) ? true : false;
                    }
                }
            }
            int countSpanRows = 0;
            for (int i = gvAdmissionStatus.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = gvAdmissionStatus.Rows[i];
                GridViewRow previousRow = gvAdmissionStatus.Rows[i - 1];
                for (int j = 1; j <= 4; j++)
                {
                    bool validation = false;
                    Label lblCurrent = new Label();
                    Label lblPrevious = new Label();
                    string columnName = string.Empty;
                    switch (j)
                    {
                        case 1:
                            columnName = "lblDate";
                            break;
                        case 2:
                            columnName = "lblgvSession";
                            break;
                        case 3:
                            columnName = "lblCourseName";
                            break;
                        case 4:
                            columnName = "lblDegreeName";
                            break;
                    }
                    lblCurrent = (Label)row.FindControl(columnName);
                    lblPrevious = (Label)previousRow.FindControl(columnName);
                    if (lblCurrent.Text == lblPrevious.Text)
                    {
                        validation = true;
                    }
                    if (validation)
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
    }

}

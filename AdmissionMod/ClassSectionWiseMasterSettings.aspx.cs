using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdmissionMod_ClassSectionWiseMasterSettings : System.Web.UI.Page
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

    string batchYear = string.Empty;
    string graduate = string.Empty;
    string courseId = string.Empty;
    string courseName = string.Empty;

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
                btnSave.Visible = false;

                BindCollege();
                BindBatch();
                BindGraduate();
                BindCourse();
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
            btnSave.Visible = false;
            BindBatch();
            BindGraduate();
            BindCourse();
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
            btnSave.Visible = false;
            BindCourse();
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
            btnSave.Visible = false;
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
            btnSave.Visible = false;
            qry = string.Empty;
            qryCollegeCode = string.Empty;
            qryGraduate = string.Empty;
            qryBatch = string.Empty;
            qryCourse = string.Empty;

            collegeCode = string.Empty;
            batchYear = string.Empty;
            graduate = string.Empty;
            courseId = string.Empty;
            courseName = string.Empty;

            string filterStream = string.Empty;
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

            DataTable dtCourseDet = new DataTable();
            dtCourseDet.Columns.Add("courseID", typeof(string));
            dtCourseDet.Columns.Add("CourseName", typeof(string));
            dtCourseDet.Columns.Add("DegreeName", typeof(string));
            dtCourseDet.Columns.Add("DegreeCode", typeof(string));
            dtCourseDet.Columns.Add("DeptCode", typeof(string));
            dtCourseDet.Columns.Add("eduLevel", typeof(string));
            dtCourseDet.Columns.Add("Priority", typeof(string));
            dtCourseDet.Columns.Add("NoOfseats", typeof(string));
            dtCourseDet.Columns.Add("NoofSections", typeof(string));
            dtCourseDet.Columns.Add("sectionName", typeof(string));
            dtCourseDet.Columns.Add("sectionNo", typeof(string));
            dtCourseDet.Columns.Add("studentCount", typeof(string));

            DataView dv = new DataView();
            DataSet dsCourseDet = new DataSet();
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(qryCourse) && !string.IsNullOrEmpty(collegeCode))
            {
                if (cb_Check.Checked == false)
                {
                    qry = "select c.Course_Id,dt.Dept_Code,dg.Degree_Code,c.Edu_Level,c.Course_Name,dt.Dept_Name,iSNULL(dg.No_Of_seats,'0') as No_Of_seats,ISnull(dg.NoofSections,'1') as NoofSections,Isnull(c.Priority,'0') Priority,sd.batchYear,LTRIM(RTRIM(ISNULL(sd.sectionName,''))) as sectionName,ISNULL(sd.studentCount,'0') as studentCount,LTRIM(RTRIM(ISNULL(sd.sectionNo,'1'))) as sectionNo from Degree dg join  Course c on c.Course_Id=dg.Course_Id and c.college_code=dg.college_code join Department dt on dt.Dept_Code=dg.Dept_Code and dg.college_code=dt.college_code and dt.college_code=c.college_code left join sectionDetails sd on dg.Degree_Code=sd.degreeCode where c.Course_Id in(" + courseId + ") and c.college_code in(" + collegeCode + ")  order by dg.Degree_Code";
                    qry += " select Nsections as  NoofSections,degree_code,batch_year  from NDegree where  batch_year ='" + ddlBatch.SelectedValue + "'";
                    dsCourseDet.Clear();
                    dsCourseDet = d2.select_method_wo_parameter(qry, "text");

                    if (dsCourseDet.Tables.Count > 0 && dsCourseDet.Tables[0].Rows.Count > 0)
                    {
                        DataRow drCourseDet;
                        DataTable dtDistintCourses = new DataTable();
                        dtDistintCourses = dsCourseDet.Tables[0].DefaultView.ToTable(true, "Course_Name", "Course_Id", "Dept_Code", "Degree_Code", "Edu_Level", "Dept_Name", "No_Of_seats", "NoofSections", "Priority");
                        foreach (DataRow drCourse in dtDistintCourses.Rows)
                        {
                            string Course_Name = Convert.ToString(drCourse["Course_Name"]).Trim();
                            string Course_Id = Convert.ToString(drCourse["Course_Id"]).Trim();
                            string Dept_Code = Convert.ToString(drCourse["Dept_Code"]).Trim();
                            string Degree_Code = Convert.ToString(drCourse["Degree_Code"]).Trim();
                            string Edu_Level = Convert.ToString(drCourse["Edu_Level"]).Trim();
                            string Dept_Name = Convert.ToString(drCourse["Dept_Name"]).Trim();
                            string No_Of_seats = Convert.ToString(drCourse["No_Of_seats"]).Trim();
                            string NoofSections = Convert.ToString(drCourse["NoofSections"]).Trim();
                            string Priority = Convert.ToString(drCourse["Priority"]).Trim();
                            int nofSeats = 0;
                            int noOfSections = 0;
                            int autochar = 65;
                            int.TryParse(No_Of_seats, out nofSeats);
                            int.TryParse(NoofSections, out noOfSections);
                            dsCourseDet.Tables[1].DefaultView.RowFilter = " Degree_Code='" + Degree_Code + "' and batch_year='" + ddlBatch.SelectedItem.Text + "'";
                            dv = dsCourseDet.Tables[1].DefaultView;
                            if (dv.Count > 0)
                            {
                                int.TryParse(Convert.ToString(dv[0]["NoofSections"]), out noOfSections);
                            }

                            for (int startSection = 1; startSection <= noOfSections; startSection++)
                            {
                                DataTable dtSectionsDetails = new DataTable();
                                if (dsCourseDet.Tables[0].Rows.Count > 0)
                                {
                                    dsCourseDet.Tables[0].DefaultView.RowFilter = "Course_Id='" + Course_Id + "' and Degree_Code='" + Degree_Code + "' and Edu_Level='" + Edu_Level + "' and sectionName='" + (char)(autochar) + "' and batchYear='" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "'";
                                    dtSectionsDetails = dsCourseDet.Tables[0].DefaultView.ToTable();
                                }
                                drCourseDet = dtCourseDet.NewRow();
                                drCourseDet["courseID"] = (dtSectionsDetails.Rows.Count > 0) ? Convert.ToString(dtSectionsDetails.Rows[0]["Course_Id"]).Trim() : Convert.ToString(Course_Id).Trim();
                                drCourseDet["CourseName"] = (dtSectionsDetails.Rows.Count > 0) ? Convert.ToString(dtSectionsDetails.Rows[0]["Course_Name"]).Trim() : Convert.ToString(Course_Name).Trim();
                                drCourseDet["DegreeName"] = (dtSectionsDetails.Rows.Count > 0) ? Convert.ToString(dtSectionsDetails.Rows[0]["Dept_Name"]).Trim() : Convert.ToString(Dept_Name).Trim();
                                drCourseDet["DeptCode"] = (dtSectionsDetails.Rows.Count > 0) ? Convert.ToString(dtSectionsDetails.Rows[0]["Dept_Code"]).Trim() : Convert.ToString(Dept_Code).Trim();
                                drCourseDet["DegreeCode"] = (dtSectionsDetails.Rows.Count > 0) ? Convert.ToString(dtSectionsDetails.Rows[0]["Degree_Code"]).Trim() : Convert.ToString(Degree_Code).Trim();
                                drCourseDet["eduLevel"] = (dtSectionsDetails.Rows.Count > 0) ? Convert.ToString(dtSectionsDetails.Rows[0]["Edu_Level"]).Trim() : Convert.ToString(Edu_Level).Trim();
                                drCourseDet["NoOfseats"] = (dtSectionsDetails.Rows.Count > 0) ? Convert.ToString(dtSectionsDetails.Rows[0]["No_Of_seats"]).Trim() : Convert.ToString(No_Of_seats).Trim();
                                drCourseDet["NoofSections"] = (dtSectionsDetails.Rows.Count > 0) ? Convert.ToString(dtSectionsDetails.Rows[0]["NoofSections"]).Trim() : Convert.ToString(NoofSections).Trim();
                                drCourseDet["Priority"] = (dtSectionsDetails.Rows.Count > 0) ? Convert.ToString(dtSectionsDetails.Rows[0]["Priority"]).Trim() : Convert.ToString(Priority).Trim();
                                drCourseDet["sectionName"] = (dtSectionsDetails.Rows.Count > 0) ? Convert.ToString(dtSectionsDetails.Rows[0]["sectionName"]).Trim() : Convert.ToString((char)(autochar)).Trim();
                                drCourseDet["sectionNo"] = (dtSectionsDetails.Rows.Count > 0) ? Convert.ToString(dtSectionsDetails.Rows[0]["sectionNo"]).Trim() : Convert.ToString(startSection).Trim();
                                drCourseDet["studentCount"] = (dtSectionsDetails.Rows.Count > 0) ? Convert.ToString(dtSectionsDetails.Rows[0]["studentCount"]).Trim() : Convert.ToString("").Trim();
                                dtCourseDet.Rows.Add(drCourseDet);
                                autochar++;
                            }
                        }
                    }
                    //if (dtCourseDet.Rows.Count > 0)
                    //{
                    //    gvSectionWiseCount.DataSource = dtCourseDet;
                    //    gvSectionWiseCount.DataBind();
                    //    gvSectionWiseCount.Visible = true;
                    //    divMainContent.Visible = true;
                    //}
                    //else
                    //{

                    //}


                    if (dtCourseDet.Rows.Count > 0)
                    {
                        gvSectionWiseCount.DataSource = dtCourseDet;
                        gvSectionWiseCount.DataBind();
                        btnPrint.Visible = true;
                        btnSave.Visible = true;
                        divMainContent.Visible = true;
                        divShowcontant.Visible = false;
                    }
                    else
                    {
                        lblAlertMsg.Text = "No Record(s) were Found";
                        lblAlertMsg.Visible = true;
                        divPopupAlert.Visible = true;
                        divShowcontant.Visible = false;
                        return;
                    }
                }
                if (cb_Check.Checked == true)
                {
                    qry = "select c.Course_Id,dt.Dept_Code,d.Degree_Code,c.Edu_Level,c.Course_Name,dt.Dept_Name,isnull(ElectiveCount,0) as ElectiveCount from Degree d,Ndegree N,Department dt,Course c where d.Degree_Code =N.Degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code =" + collegeCode + " and c.Course_Id =" + courseId + " and N.batch_year ='" + ddlBatch.SelectedItem.Text + "'";
                    dsCourseDet.Clear();
                    dsCourseDet = d2.select_method_wo_parameter(qry, "text");
                    if (dsCourseDet.Tables.Count > 0 && dsCourseDet.Tables[0].Rows.Count > 0)
                    {
                        GridView1.DataSource = dsCourseDet.Tables[0];
                        GridView1.DataBind();
                        GridView1.Visible = true;
                        btnPrint.Visible = true;
                        btnSave.Visible = true;
                        divShowcontant.Visible = true;
                        //divMainContent.Visible = true;
                    }
                    else
                    {
                        lblAlertMsg.Text = "No Record(s) were Found";
                        lblAlertMsg.Visible = true;
                        divPopupAlert.Visible = true;
                        divShowcontant.Visible = false;
                        return;
                    }
                }
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

    #region Save Details

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            bool isSave = false;
            if (cb_Check.Checked == false)
            {
                if (gvSectionWiseCount.Rows.Count > 0)
                {
                    foreach (GridViewRow gvSecRow in gvSectionWiseCount.Rows)
                    {
                        string batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
                        string degreeCode = Convert.ToString(ddlBatch.SelectedValue).Trim();
                        Label lbldegreeCode = (Label)gvSecRow.FindControl("lblDegreeCode");
                        degreeCode = Convert.ToString(lbldegreeCode.Text).Trim();
                        TextBox txtStudentCount = (TextBox)gvSecRow.FindControl("txtStudentCount");
                        Label lblSectionName = (Label)gvSecRow.FindControl("lblSectionName");
                        Label lblTotSeats = (Label)gvSecRow.FindControl("lblTotSeats");
                        Label lblSectionNo = (Label)gvSecRow.FindControl("lblSectionNo");
                        //lblTotSeats
                        string studentCount = txtStudentCount.Text;
                        int studCount = 0;
                        int.TryParse(studentCount, out studCount);
                        int totalCount = 0;
                        int.TryParse(lblTotSeats.Text.Trim(), out totalCount);
                        if (!string.IsNullOrEmpty(lblSectionName.Text.Trim()) && studCount > 0 && totalCount > 0)
                        {
                            qry = "if exists (select * from sectionDetails where batchYear='" + batchYear + "' and degreeCode='" + degreeCode + "' and sectionName='" + lblSectionName.Text.Trim() + "') update sectionDetails set studentCount='" + studCount + "',sectionNo='" + lblSectionNo.Text.Trim() + "' where batchYear='" + batchYear + "' and degreeCode='" + degreeCode + "' and sectionName='" + lblSectionName.Text.Trim() + "' else insert into sectionDetails (degreeCode,batchYear,sectionName,sectionNo,studentCount) values('" + degreeCode + "','" + batchYear + "','" + lblSectionName.Text.Trim() + "','" + lblSectionNo.Text.Trim() + "','" + studCount + "')";
                            int inserted = d2.update_method_wo_parameter(qry, "text");
                            if (inserted > 0)
                            {
                                isSave = true;
                            }
                        }
                    }
                }
                btnGo_Click(sender, e);
                if (isSave)
                {
                    lblAlertMsg.Text = "Saved Successfully";
                    lblAlertMsg.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
                else
                {
                    lblAlertMsg.Text = "Not Saved";
                    lblAlertMsg.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
            }
            if (cb_Check.Checked == true)
            {
                if (GridView1.Rows.Count > 0)
                {
                    foreach (GridViewRow gvSecRow in GridView1.Rows)
                    {
                        string batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
                        string degreeCode = Convert.ToString(ddlBatch.SelectedValue).Trim();
                        Label lbldegreeCode = (Label)gvSecRow.FindControl("lblDegreeCode");
                        degreeCode = Convert.ToString(lbldegreeCode.Text).Trim();
                        TextBox txtStudentCount = (TextBox)gvSecRow.FindControl("txtStudentCount");
                        //Label lblSectionName = (Label)gvSecRow.FindControl("lblSectionName");
                        //Label lblTotSeats = (Label)gvSecRow.FindControl("lblTotSeats");
                        //Label lblSectionNo = (Label)gvSecRow.FindControl("lblSectionNo");
                        //lblTotSeats
                        string studentCount = txtStudentCount.Text;
                        int studCount = 0;
                        int.TryParse(studentCount, out studCount);
                        //int totalCount = 0;
                        //int.TryParse(lblTotSeats.Text.Trim(), out totalCount);
                        if (studCount > 0)
                        {
                            qry = "  if exists (select Degree_code from Ndegree where Degree_code ='" + lbldegreeCode.Text + "' and batch_year ='" + batchYear + "') update Ndegree set ElectiveCount='" + studCount + "' where Degree_code ='" + lbldegreeCode.Text + "' and batch_year ='" + batchYear + "' ";
                            int inserted = d2.update_method_wo_parameter(qry, "text");
                            if (inserted > 0)
                            {
                                isSave = true;
                            }
                        }
                    }
                }
                btnGo_Click(sender, e);
                if (isSave)
                {
                    lblAlertMsg.Text = "Saved Successfully";
                    lblAlertMsg.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
                else
                {
                    lblAlertMsg.Text = "Not Saved";
                    lblAlertMsg.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
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

    protected void gvSectionWiseCount_DataBound(object sender, EventArgs e)
    {
        try
        {
            int countSpanRows = 0;
            for (int i = gvSectionWiseCount.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = gvSectionWiseCount.Rows[i];
                GridViewRow previousRow = gvSectionWiseCount.Rows[i - 1];
                for (int j = 1; j <= 1; j++)
                {
                    bool validation = false;
                    Label lblCurrent = new Label();
                    Label lblPrevious = new Label();
                    string columnName = string.Empty;
                    switch (j)
                    {
                        case 1:
                            columnName = "lblDegreeName";
                            break;
                    }
                    lblCurrent = (Label)row.FindControl(columnName);
                    lblPrevious = (Label)previousRow.FindControl(columnName);
                    TextBox txtStudentCount = (TextBox)row.FindControl("txtStudentCount");
                    txtStudentCount.Attributes.Add("onchange", "return validateCount()");
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
                                previousRow.Cells[j + 1].RowSpan = 2;
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                previousRow.Cells[j + 1].RowSpan = row.Cells[j + 1].RowSpan + 1;
                            }
                            row.Cells[j].Visible = false;
                            row.Cells[j + 1].Visible = false;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void gvSectionWiseCount_PreRender(object sender, EventArgs e)
    {

        ClientScriptManager cs = Page.ClientScript;
        foreach (GridViewRow grdrow in gvSectionWiseCount.Rows)
        {
            Label lblDegreeCode = (Label)grdrow.FindControl("lblDegreeCode");

            Label lblDegreeName = (Label)grdrow.FindControl("lblDegreeName");

            Label lblTotSeats = (Label)grdrow.FindControl("lblTotSeats");

            Label lblSectionName = (Label)grdrow.FindControl("lblSectionName");

            Label lblStudentCount = (Label)grdrow.FindControl("lblStudentCount");

            Label lblNoofSeats = (Label)grdrow.FindControl("lblNoofSeats");
            TextBox txtStudentCount = (TextBox)grdrow.FindControl("txtStudentCount");
            txtStudentCount.Attributes.Add("onchange", "return validateCount()");
            cs.RegisterArrayDeclaration("gvDegreeCode", String.Concat("'", lblDegreeCode.ClientID, "'"));

            cs.RegisterArrayDeclaration("gvDegreeName", String.Concat("'", lblDegreeName.ClientID, "'"));

            cs.RegisterArrayDeclaration("gvTotSeats", String.Concat("'", lblTotSeats.ClientID, "'"));
            cs.RegisterArrayDeclaration("gvNoofSeats", String.Concat("'", lblNoofSeats.ClientID, "'"));

            cs.RegisterArrayDeclaration("gvSectionName", String.Concat("'", lblSectionName.ClientID, "'"));

            cs.RegisterArrayDeclaration("gvStudentCount", String.Concat("'", txtStudentCount.ClientID, "'"));
            cs.RegisterArrayDeclaration("gvStudentCount_lbl", String.Concat("'", lblStudentCount.ClientID, "'"));

        }
    }

}
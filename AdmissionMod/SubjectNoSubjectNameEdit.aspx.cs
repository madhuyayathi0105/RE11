using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using wc = System.Web.UI.WebControls;

public partial class AdmissionMod_SubjectNoSubjectNameEdit : System.Web.UI.Page
{
    #region Field Declaration

    Hashtable ht = new Hashtable();

    string userCode = string.Empty;
    string collegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;

    string qry = string.Empty;
    string qryCollegeCode = string.Empty;
    string qryGraduate = string.Empty;
    string qryBatch = string.Empty;
    string qryDegreeCode = string.Empty;
    string qryCourse = string.Empty;

    string batchYear = string.Empty;
    string degreeCode = string.Empty;
    string graduate = string.Empty;
    string courseId = string.Empty;
    string courseName = string.Empty;

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
                BindRightsBaseBatch();
                BindGraduate();
                BindCourse();
                BindBranch();
                BindSem();
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

    public void BindRightsBaseBatch()
    {
        try
        {
            userCode = string.Empty;
            groupUserCode = string.Empty;
            qryUserOrGroupCode = string.Empty;
            collegeCode = string.Empty;
            ds.Clear();
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    groupUserCode = Convert.ToString(group_semi[0]);
                }
                if (!string.IsNullOrEmpty(groupUserCode))
                {
                    qryUserOrGroupCode = " and user_id='" + groupUserCode + "'";
                }
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                if (!string.IsNullOrEmpty(userCode))
                {
                    qryUserOrGroupCode = " and user_id='" + userCode + "'";
                }
            }
            if (ddlCollege.Items.Count > 0 && ddlCollege.Visible)
            {
                collegeCode = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCode))
                        {
                            collegeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and r.college_code in(" + collegeCode + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qryCollegeCode = " and r.college_code in(" + collegeCode + ")";
            }
            string qryEduLevel = string.Empty;
            if (ddlEduLevel.Items.Count > 0)
            {
                string eduLevels = "'" + Convert.ToString(ddlEduLevel.SelectedItem.Text).Trim() + "'";
                if (!string.IsNullOrEmpty(eduLevels))
                {
                    qryEduLevel = " and ltrim(rtrim(isnull(c.edu_level,''))) in(" + eduLevels + ")";
                }
            }
            DataSet dsBatch = new DataSet();
            if (!string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                string qry = "select distinct batch_year from tbl_attendance_rights r where batch_year<>'' " + qryCollegeCode + qryUserOrGroupCode + " order by batch_year desc";
                dsBatch = d2.select_method_wo_parameter(qry, "Text");
            }
            if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = dsBatch;
                ddlBatch.DataTextField = "Batch_year";
                ddlBatch.DataValueField = "Batch_year";
                ddlBatch.DataBind();
                ddlBatch.SelectedIndex = 0;
            }
            else
            {
                if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryCollegeCode))
                {
                    qry = "select distinct r.Batch_Year from Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and r.degree_code=dg.Degree_Code  and r.Batch_Year<>'0' and r.Batch_Year<>-1 " + qryCollegeCode + qryEduLevel + " order by r.Batch_Year desc";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(qry, "Text");
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlBatch.DataSource = ds;
                    ddlBatch.DataTextField = "Batch_Year";
                    ddlBatch.DataValueField = "Batch_Year";
                    ddlBatch.DataBind();
                    ddlBatch.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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

    private void BindDepartment()
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

    public void BindBranch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            cblBranch.Items.Clear();
            ddlBranch.Items.Clear();
            chkBranch.Checked = false;
            txtBranch.Text = "--Select--";
            txtBranch.Enabled = false;
            ds.Clear();
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            ds.Dispose();
            ds.Reset();
            string columnfield = string.Empty;
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(groupUserCode).Trim() != "") && Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                columnfield = " and dp.group_code='" + groupUserCode.Trim() + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and dp.user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            selected = 0;
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
                    qryCourse = " and c.course_id in(" + courseId + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryCourse) && !string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(columnfield))
            {
                //ds = da.select_method_wo_parameter("select distinct dg.Degree_Code,dt.Dept_Name from Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r where  r.degree_code=dg.Degree_Code and dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and r.college_code=c.college_code and r.college_code=dg.college_code and dt.college_code=r.college_code " + qryCourseId + qryCollege + columnfield + qryStream + qryEduLevel + qryBatch + "order by dg.Degree_Code", "text");//and r.CC='1' and ISNULL(r.isRedo,'0')='0' 
                ds = d2.select_method_wo_parameter("select distinct dg.degree_code,dt.dept_name as degree,dg.Acronym,dg.course_id,dt.dept_name,c.priority from Degree dg,Course c,Department dt,DeptPrivilages dp where dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and c.college_code=dt.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code " + qryCollegeCode + columnfield + qryCourse + " order by c.priority,dt.dept_name ", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblBranch.DataSource = ds;
                    cblBranch.DataTextField = "degree";
                    cblBranch.DataValueField = "degree_code";
                    cblBranch.DataBind();
                    checkBoxListselectOrDeselect(cblBranch, true);
                    txtBranch.Enabled = true;
                    CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");

                    ddlBranch.DataSource = ds;
                    ddlBranch.DataTextField = "degree";
                    ddlBranch.DataValueField = "degree_code";
                    ddlBranch.DataBind();
                    ddlBranch.SelectedIndex = 0;
                }
                else
                {
                    txtBranch.Enabled = false;
                }
            }
            else
            {
                txtBranch.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindSem()
    {
        try
        {
            ds.Clear();
            ddlSem.Items.Clear();
            ddlSem.Enabled = false;
            bool first_year = false;
            int duration = 0;
            int i = 0;
            selected = 0;
            qryCollegeCode = string.Empty;
            collegeCode = string.Empty;
            qryBatch = string.Empty;
            batchYear = string.Empty;
            qryDegreeCode = string.Empty;
            degreeCode = string.Empty;
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
                    qryCollegeCode = " and c.college_code in(" + collegeCode + ")";
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
                    qryCourse = " and c.course_id in(" + courseId + ")";
                }
            }
            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                batchYear = string.Empty;
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Text + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatch = " and Batch_year in(" + batchYear + ")";
                }
            }
            if (ddlBranch.Items.Count > 0 && ddlBranch.Visible)
            {
                degreeCode = string.Empty;
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCode))
                        {
                            degreeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and degree_code in(" + degreeCode + ")";
                }
            }
            else if (cblBranch.Items.Count > 0 && txtBranch.Enabled)
            {
                degreeCode = string.Empty;
                foreach (ListItem li in cblBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCode))
                        {
                            degreeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and degree_code in(" + degreeCode + ")";
                }
            }
            string sqlnew = string.Empty;
            if (!string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryBatch) && !string.IsNullOrEmpty(collegeCode))
            {
                sqlnew = "select distinct max(ndurations) ndurations,first_year_nonsemester from ndegree dg where ndurations<>'0' and college_code in(" + collegeCode + ")" + qryDegreeCode + qryBatch + " group by first_year_nonsemester";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sqlnew, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                }
                ddlSem.SelectedIndex = 0;
                ddlSem.Enabled = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(collegeCode))
                {
                    sqlnew = "select distinct max(duration) duration,first_year_nonsemester from degree dg where duration<>'0' and college_code in(" + collegeCode + ") " + qryDegreeCode + " group by first_year_nonsemester";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sqlnew, "Text");
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                    int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                    }
                    ddlSem.SelectedIndex = 0;
                    ddlSem.Enabled = true;
                }
                else
                {
                    ddlSem.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            BindRightsBaseBatch();
            BindGraduate();
            BindCourse();
            BindBranch();
            BindSem();
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
            BindBranch();
            BindSem();
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
            BindBranch();
            BindSem();
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
            BindBranch();
            BindSem();
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
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
            CallCheckboxChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            BindSem();
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
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
            CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            BindSem();
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
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
            BindSem();
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlSem_SelectedIndexChanged(object sender, EventArgs e)
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

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        StringBuilder selectedvalue = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append("'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                    else
                    {
                        selectedvalue.Append(",'" + Convert.ToString(cblSelected.Items[sel].Value) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedvalue.ToString();
    }

    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        StringBuilder selectedText = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append("'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                    else
                    {
                        selectedText.Append(",'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedText.ToString();
    }

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = string.Empty;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }

    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = string.Empty;
            cb.Checked = false;
            txt.Text = deft;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }

    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true, int selCount = 0)
    {
        try
        {
            int count = 0;
            foreach (wc.ListItem li in cbl.Items)
            {
                if (selCount != 0 && count == selCount)
                {
                    break;
                }
                count++;
                li.Selected = selected;
            }
        }
        catch
        {
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
            string Degree = string.Empty;

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
                    qryCollegeCode = " and sfs.College_Code in(" + collegeCode + ")";
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
                    qryBatch = " and sm.Batch_Year in(" + batchYear + ")";
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
                    qryCourse = " and c.course_Id in(" + courseId + ")";
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblCourse.Text + " were Found";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }

            if (ddlBranch.Items.Count > 0 && ddlBranch.Visible)
            {
                degreeCode = string.Empty;
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCode))
                        {
                            degreeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and sm.degree_code in(" + degreeCode + ")";
                    Degree = degreeCode;
                }
            }
            else if (cblBranch.Items.Count > 0 && txtBranch.Enabled)
            {
                degreeCode = string.Empty;
                foreach (ListItem li in cblBranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCode))
                        {
                            degreeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and sm.degree_code in(" + degreeCode + ")";
                    Degree = degreeCode;
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblBranch.Text + " were Found";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            string semester = string.Empty;
            string qrySemester = string.Empty;
            if (ddlSem.Items.Count > 0)
            {
                semester = string.Empty;
                foreach (ListItem li in ddlSem.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(semester))
                        {
                            semester = "'" + li.Value + "'";
                        }
                        else
                        {
                            semester += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(semester))
                {
                    qrySemester = " and sm.semester in(" + semester + ")";
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblSem.Text + " were Found";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            DataTable dtCourseDet = new DataTable();

            DataSet dsCourseDet = new DataSet();
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(qryCourse) && !string.IsNullOrEmpty(collegeCode))
            {
                string Qury = "select subject_code,subject_name as subject_name,subject_no from subject s,syllabus_master sy,sub_sem ss where s.subType_no =ss.subType_no and sy.syll_code =s.syll_code and ss.syll_code =sy.syll_code and sy.degree_code in (" + degreeCode + ") and sy.Batch_Year ='" + ddlBatch.SelectedItem.Text + "' and sy.semester ='" + ddlSem.SelectedItem.Text + "'";
                dsCourseDet = d2.select_method_wo_parameter(Qury, "Text");
            }
            if (dsCourseDet.Tables.Count > 0 && dsCourseDet.Tables[0].Rows.Count > 0)
            {
                gvSectionWiseCount.DataSource = dsCourseDet.Tables[0];
                gvSectionWiseCount.DataBind();
                divMainContent.Visible = true;
                btnSave.Visible = true;
            }
            else
            {
                lblAlertMsg.Text = "No Record(s) Found";
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

    #region Save Details

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            bool isSave = false;

            if (gvSectionWiseCount.Rows.Count > 0)
            {
                foreach (GridViewRow gvSecRow in gvSectionWiseCount.Rows)
                {

                    TextBox txtsubjectcode = (TextBox)gvSecRow.FindControl("txtStudentCount");
                    TextBox txtsubjectName = (TextBox)gvSecRow.FindControl("txtSubjectName");
                    Label lblStaffApplId = (Label)gvSecRow.FindControl("lblDegreeName");
                    Label lblSubjectNo = (Label)gvSecRow.FindControl("lblSubjectNo");

                    if (!string.IsNullOrEmpty(lblSubjectNo.Text.Trim()) && !string.IsNullOrEmpty(txtsubjectcode.Text.Trim()) && !string.IsNullOrEmpty(txtsubjectName.Text.Trim()))
                    {
                        qry = "  if exists (select subject_Code from subject where subject_code ='" + lblStaffApplId.Text + "' and subject_no ='" + lblSubjectNo.Text + "')  update subject set subject_code ='" + txtsubjectcode.Text + "',subject_name='" + txtsubjectName.Text + "' where subject_code ='" + lblStaffApplId.Text + "' and subject_no ='" + lblSubjectNo.Text + "'";
                        int inserted = d2.update_method_wo_parameter(qry, "text");
                        if (inserted > 0)
                        {
                            isSave = true;
                        }
                    }
                }
            }

            if (isSave)
            {
                btnGo_Click(sender, e);
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
                for (int j = 1; j <= 2; j++)
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
                        case 2:
                            columnName = "lblSubjectDet";
                            break;
                    }
                    lblCurrent = (Label)row.FindControl(columnName);
                    lblPrevious = (Label)previousRow.FindControl(columnName);
                    //TextBox txtStudentCount = (TextBox)row.FindControl("txtStudentCount");
                    //txtStudentCount.Attributes.Add("onchange", "return validateCount()");
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
                                //previousRow.Cells[j + 1].RowSpan = 2;
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                //previousRow.Cells[j + 1].RowSpan = row.Cells[j + 1].RowSpan + 1;
                            }
                            row.Cells[j].Visible = false;
                            //row.Cells[j + 1].Visible = false;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void gvSection_DataBound(object sender, EventArgs e)
    {
        try
        {
            int countSpanRows = 0;
            for (int i = DivgvSectionWiseCount.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = DivgvSectionWiseCount.Rows[i];
                GridViewRow previousRow = DivgvSectionWiseCount.Rows[i - 1];
                for (int j = 1; j <= 3; j++)
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
                        case 2:
                            columnName = "lblSubjectDet";
                            break;
                        case 3:
                            columnName = "lblStaffName";
                            break;
                    }
                    lblCurrent = (Label)row.FindControl(columnName);
                    lblPrevious = (Label)previousRow.FindControl(columnName);
                    //TextBox txtStudentCount = (TextBox)row.FindControl("txtStudentCount");
                    //txtStudentCount.Attributes.Add("onchange", "return validateCount()");
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
                                //previousRow.Cells[j + 1].RowSpan = 2;
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                //previousRow.Cells[j + 1].RowSpan = row.Cells[j + 1].RowSpan + 1;
                            }
                            row.Cells[j].Visible = false;
                            //row.Cells[j + 1].Visible = false;
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
            grdrow.Cells[4].Attributes.Add("ondblclick", "return clickCells(" + grdrow.RowIndex + ")");
            Label lblDegreeCode = (Label)grdrow.FindControl("lblDegreeCode");
            Label lblDegreeName = (Label)grdrow.FindControl("lblDegreeName");

            Label lblSubjectDet = (Label)grdrow.FindControl("lblSubjectDet");
            Label lblSubjectCode = (Label)grdrow.FindControl("lblSubjectCode");
            Label lblSubjectNo = (Label)grdrow.FindControl("lblSubjectNo");

            Label lblStaffName = (Label)grdrow.FindControl("lblStaffName");
            Label lblStaffCode = (Label)grdrow.FindControl("lblStaffCode");
            Label lblStaffApplId = (Label)grdrow.FindControl("lblStaffApplId");

            Label lblStudentCount = (Label)grdrow.FindControl("lblStudentCount");
            TextBox txtStudentCount = (TextBox)grdrow.FindControl("txtStudentCount");
            txtStudentCount.Enabled = true;
            txtStudentCount.Attributes.Add("style", "display:block;");
            lblStudentCount.Attributes.Add("style", "display:none;");

            if (!string.IsNullOrEmpty(txtStudentCount.Text.Trim()) && txtStudentCount.Text.Trim() != "0")
            {
                txtStudentCount.Attributes.Add("style", "display:none;");
                lblStudentCount.Attributes.Add("style", "display:block;");
            }

            txtStudentCount.Attributes.Add("onfocusout", "return focusOut(" + grdrow.RowIndex + ")");
            txtStudentCount.Attributes.Add("onclick", "return textBoxClick(" + grdrow.RowIndex + ")");
            cs.RegisterArrayDeclaration("gvDegreeCode", String.Concat("'", lblDegreeCode.ClientID, "'"));
            cs.RegisterArrayDeclaration("gvDegreeName", String.Concat("'", lblDegreeName.ClientID, "'"));

            cs.RegisterArrayDeclaration("gvSubjectDet", String.Concat("'", lblSubjectDet.ClientID, "'"));
            cs.RegisterArrayDeclaration("gvSubjectCode", String.Concat("'", lblSubjectCode.ClientID, "'"));
            cs.RegisterArrayDeclaration("gvSubjectNo", String.Concat("'", lblSubjectNo.ClientID, "'"));

            cs.RegisterArrayDeclaration("gvStaffDet", String.Concat("'", lblStaffName.ClientID, "'"));
            cs.RegisterArrayDeclaration("gvStaffCode", String.Concat("'", lblStaffCode.ClientID, "'"));
            cs.RegisterArrayDeclaration("gvStaffApplId", String.Concat("'", lblStaffApplId.ClientID, "'"));

            cs.RegisterArrayDeclaration("gvStudentCount", String.Concat("'", txtStudentCount.ClientID, "'"));
            cs.RegisterArrayDeclaration("gvStudentCount_lbl", String.Concat("'", lblStudentCount.ClientID, "'"));
        }
    }


    public string GetSection(int Value)
    {
        string NewSection = "";
        switch (Value)
        {
            case 1:
                NewSection = "A";
                break;
            case 2:
                NewSection = "B";
                break;
            case 3:
                NewSection = "C";
                break;
            case 4:
                NewSection = "D";
                break;
            case 5:
                NewSection = "E";
                break;
            case 6:
                NewSection = "F";
                break;
            case 7:
                NewSection = "G";
                break;
            case 8:
                NewSection = "H";
                break;
            case 9:
                NewSection = "I";
                break;
            case 10:
                NewSection = "J";
                break;
            case 11:
                NewSection = "K";
                break;
            case 12:
                NewSection = "L";
                break;
            case 13:
                NewSection = "M";
                break;
            case 14:
                NewSection = "N";
                break;
            case 15:
                NewSection = "O";
                break;
            case 16:
                NewSection = "P";
                break;
            case 17:
                NewSection = "Q";
                break;
            case 18:
                NewSection = "R";
                break;
            case 19:
                NewSection = "S";
                break;

        }
        return NewSection;
    }

}
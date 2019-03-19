using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using wc = System.Web.UI.WebControls;

public partial class AdmissionMod_Elective_Subject_Student_Count : System.Web.UI.Page
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
    GridViewRow gvSecRow;

    bool isSchool = false;
    int selected = 0;



    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet papcount = new DataSet();
    ReuasableMethods rs = new ReuasableMethods();

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
            eqpapmat();
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
            //cblBranch.Items.Clear();
            ddlBranch.Items.Clear();
            //chkBranch.Checked = false;
            //txtBranch.Text = "--Select--";
            //txtBranch.Enabled = false;
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
                    //cblBranch.DataSource = ds;
                    //cblBranch.DataTextField = "degree";
                    //cblBranch.DataValueField = "degree_code";
                    //cblBranch.DataBind();
                    //checkBoxListselectOrDeselect(cblBranch, true);
                    //txtBranch.Enabled = true;
                    //CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");

                    ddlBranch.DataSource = ds;
                    ddlBranch.DataTextField = "degree";
                    ddlBranch.DataValueField = "degree_code";
                    ddlBranch.DataBind();
                    ddlBranch.SelectedIndex = 0;
                }
                else
                {
                    //txtBranch.Enabled = false;
                }
            }
            else
            {
                //txtBranch.Enabled = false;
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
            //else if (cblBranch.Items.Count > 0 && txtBranch.Enabled)
            //{
            //    degreeCode = string.Empty;
            //    foreach (ListItem li in cblBranch.Items)
            //    {
            //        if (li.Selected)
            //        {
            //            if (string.IsNullOrEmpty(degreeCode))
            //            {
            //                degreeCode = "'" + li.Value + "'";
            //            }
            //            else
            //            {
            //                degreeCode += ",'" + li.Value + "'";
            //            }
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(degreeCode))
            //    {
            //        qryDegreeCode = " and degree_code in(" + degreeCode + ")";
            //    }
            //}
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

    public void eqpapmat()
    {
        if (cbequalpapcount.Checked == true)
        {
            ddlBatch.Enabled = false;
            ddlBranch.Enabled = false;
            ddlCourse.Enabled = false;
            ddlEduLevel.Enabled = false;
            ddlSem.Enabled = false;

          
        }
        else
        {
            ddlBatch.Enabled = true;
            ddlBranch.Enabled = true;
            ddlCourse.Enabled = true;
            ddlEduLevel.Enabled = true;
            ddlSem.Enabled = true;
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

    //protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblErrSearch.Text = string.Empty;
    //        lblErrSearch.Visible = false;
    //        divPopupAlert.Visible = false;
    //        lblAlertMsg.Text = string.Empty;
    //        divMainContent.Visible = false;
    //        btnPrint.Visible = false;
    //        btnSave.Visible = false;
    //        CallCheckboxChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
    //        BindSem();
    //    }
    //    catch (Exception ex)
    //    {
    //        //lblErrSearch.Text = Convert.ToString(ex);
    //        //lblErrSearch.Visible = true;
    //        //d2.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
    //    }
    //}

    //protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblErrSearch.Text = string.Empty;
    //        lblErrSearch.Visible = false;
    //        divPopupAlert.Visible = false;
    //        lblAlertMsg.Text = string.Empty;
    //        divMainContent.Visible = false;
    //        btnPrint.Visible = false;
    //        btnSave.Visible = false;
    //        CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
    //        BindSem();
    //    }
    //    catch (Exception ex)
    //    {
    //        //lblErrSearch.Text = Convert.ToString(ex);
    //        //lblErrSearch.Visible = true;
    //        //d2.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0 && ddlCollege.Visible) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
    //    }
    //}

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

    protected void cb_eqpapmat_checkedchange(object sender, EventArgs e)
    {
       

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
                //else if (cblBranch.Items.Count > 0 && txtBranch.Enabled)
                //{
                //    degreeCode = string.Empty;
                //    foreach (ListItem li in cblBranch.Items)
                //    {
                //        if (li.Selected)
                //        {
                //            if (string.IsNullOrEmpty(degreeCode))
                //            {
                //                degreeCode = "'" + li.Value + "'";
                //            }
                //            else
                //            {
                //                degreeCode += ",'" + li.Value + "'";
                //            }
                //        }
                //    }
                //    if (!string.IsNullOrEmpty(degreeCode))
                //    {
                //        qryDegreeCode = " and sm.degree_code in(" + degreeCode + ")";
                //        Degree = degreeCode;
                //    }
                //}
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
                        //string semval = string.Empty;
                        string setting = "CBCSsem" + batchYear;
                        string setsem = d2.GetFunction("select template from master_settings where settings='" + setting + "' and value='" + degreeCode + "'");
                        if (!string.IsNullOrEmpty(setsem) && setsem != "0")
                            semester = setsem;
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

                string CheckValue = "and isnull(IsSectionWise,'0')<>'1'";
                if (cbSectionWise.Checked == false)
                {
                    CheckValue = "and isnull(IsSectionWise,'0')<>'1'";
                }
                if (cbSectionWise.Checked == true)
                {
                    CheckValue = "and isnull(IsSectionWise,'0')='1'";
                }

            //added by Mullai

             if (cbequalpapcount.Checked == true)
            {
                

                string equal_subcode = string.Empty;
                string eqpapcount = string.Empty;
                string eq_subjectcode = string.Empty;
                 string sub_no=string.Empty;
                 string eqpapcount1 = string.Empty;
                 string stafcode=string.Empty;

                 string eqpapscode = "select distinct s.subject_code,Equal_Subject_Code from subject s,syllabus_master sy,sub_sem ss,Registration r,tbl_Subject_paper_Matching ep where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and r.Current_Semester=sy.semester and s.syll_code=sy.syll_code and ss.syll_code=s.syll_code and (Com_Subject_Code=s.subject_code or Equal_Subject_Code=s.subject_code) and Com_Subject_Code=s.subject_code and CC=0 and DelFlag=0";
                papcount.Clear();
                papcount = d2.select_method_wo_parameter(eqpapscode, "text");
                if (papcount.Tables.Count > 0 && papcount.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < papcount.Tables[0].Rows.Count; i++)
                    {
                        if (string.IsNullOrEmpty(equal_subcode))
                            equal_subcode = "'" + Convert.ToString(papcount.Tables[0].Rows[i]["Equal_Subject_code"]) + "'";
                        else if (!equal_subcode.Contains(Convert.ToString(papcount.Tables[0].Rows[i]["Equal_Subject_code"])))
                            equal_subcode += "," + "'" + Convert.ToString(papcount.Tables[0].Rows[i]["Equal_Subject_code"]) + "'";                                   
                    }
                    eqpapcount1 = "select distinct s.subject_name, s.subject_code,LTRIM(RTRIM(ISNULL(s.subject_name,'')))+' ( '+s.subject_code+' )' as subjectDetails ,sse.staff_code,sm.staff_name,sam.appl_id,LTRIM(RTRIM(ISNULL(sm.staff_name,'')))+' ( '+sse.staff_code+' )' as staffDetails from subject s,staffmaster sm,staff_selector sse,staff_appl_master sam where s.subject_no=sse.subject_no and sm.staff_code=sse.staff_code  and s.subject_code in(" + equal_subcode + ")  and sam.appl_no=sm.appl_no ";

                    DataSet satfcod = new DataSet();
                    satfcod = d2.select_method_wo_parameter(eqpapcount1, "text");
                    if (satfcod.Tables.Count > 0 && satfcod.Tables[0].Rows.Count > 0)
                    {
                        for (int i1 = 0; i1 < satfcod.Tables[0].Rows.Count; i1++)
                        {
                            if (string.IsNullOrEmpty(stafcode))
                                stafcode = "'" + Convert.ToString(satfcod.Tables[0].Rows[i1]["staff_code"]) + "'";
                            else if (!stafcode.Contains(Convert.ToString(satfcod.Tables[0].Rows[i1]["staff_code"])))
                                stafcode += "," + "'" + Convert.ToString(satfcod.Tables[0].Rows[i1]["staff_code"]) + "'";
                           
                        }
                    }
                    eqpapcount = "select distinct s.subject_name, s.subject_code,LTRIM(RTRIM(ISNULL(s.subject_name,'')))+' ( '+s.subject_code+' )' as subjectDetails ,sse.staff_code,sm.staff_name,sam.appl_id ,LTRIM(RTRIM(ISNULL(sm.staff_name,'')))+' ( '+sse.staff_code+' )' as staffDetails,case when ISNULL('','0')='0' then '' else Convert(varchar(20),ISNULL('','0')) end as studentCount,case when ISNULL('','0')='0' then '' else Convert(varchar(20),ISNULL('','0')) end as studentminCount    from staff_appl_master sam , subject s,staffmaster sm,staff_selector sse where s.subject_no=sse.subject_no and sm.staff_code=sse.staff_code   and s.subject_code in(" + equal_subcode + ") and sse.staff_code in(" + stafcode + ") and sam.appl_no=sm.appl_no order by s.subject_code";
                    papcount = d2.select_method_wo_parameter(eqpapcount, "text");

                    if (papcount.Tables.Count > 0 && papcount.Tables[0].Rows.Count > 0)
                    {                       
                        DataTable counteq = new DataTable();
                        DataTable papmat = new DataTable();
                        DataRow eq;
                        papmat = papcount.Tables[0].DefaultView.ToTable(true, "subject_name", "subject_code", "subjectDetails", "staff_code", "staff_name", "appl_id", "staffDetails", "studentCount", "studentminCount");
                       
                        if (papmat.Rows.Count > 0)
                        {
                                counteq.Columns.Add("subject Details");
                                counteq.Columns.Add("subject_no");
                                counteq.Columns.Add("subject_code");
                                counteq.Columns.Add("staff Details");
                                counteq.Columns.Add("appl_id");
                                counteq.Columns.Add("staff_code");
                                counteq.Columns.Add("MaxCount");
                                counteq.Columns.Add("MinCount");

                                for (int j = 0; j < papmat.Rows.Count; j++)
                                {
                                    string subcode = Convert.ToString(papmat.Rows[j]["subject_code"]);
                                    string staffcod = Convert.ToString(papmat.Rows[j]["staff_code"]);

                                    string qry1 = "select distinct s.subject_code,s.subject_no,sse.staff_code from subject s,staffmaster sm,staff_selector sse where s.subject_no=sse.subject_no and sm.staff_code=sse.staff_code  and s.subject_code='" + subcode + "' and sse.staff_code ='" + staffcod + "' and sse.staff_code=sm.staff_code   order by s.subject_no";
                                  // string qry1 = "select distinct s.subject_code,s.subject_no,sse.staff_code from staff_appl_master sam , subject s,staffmaster sm,staff_selector sse,electiveSubjectDetails ed where s.subject_no=sse.subject_no and sm.staff_code=sse.staff_code  and s.subject_code='" + subcode + "' and sse.staff_code  ='" + staffcod + "' and sse.staff_code=sm.staff_code   and  sam.appl_no=sm.appl_no and ed.staffCode=sse.staff_code and ed.staffCode=sm.staff_code and ed.subjectNo=sse.subject_no and sam.appl_id=ed.staffApplId   order by s.subject_no ";

                                    DataSet subn = new DataSet();
                                    subn.Clear();
                                    subn = d2.select_method_wo_parameter(qry1, "text");
                                    if (subn.Tables.Count > 0 && subn.Tables[0].Rows.Count > 0)
                                    {
                                        for (int k = 0; k < subn.Tables[0].Rows.Count; k++)
                                        {

                                            if (string.IsNullOrEmpty(sub_no))
                                                sub_no = Convert.ToString(subn.Tables[0].Rows[k]["subject_no"]);
                                            else if (!sub_no.Contains(Convert.ToString(subn.Tables[0].Rows[k]["subject_no"])))
                                                sub_no += "," + Convert.ToString(subn.Tables[0].Rows[k]["subject_no"]);
                                        }
                                    }

                                    string qry2 = " select distinct s.subject_name, s.subject_code,LTRIM(RTRIM(ISNULL(s.subject_name,'')))+' ( '+s.subject_code+' )' as subjectDetails ,sse.staff_code,sm.staff_name,sam.appl_id ,LTRIM(RTRIM(ISNULL(sm.staff_name,'')))+' ( '+sse.staff_code+' )' as staffDetails,case when ISNULL(studentCount,'0')='0' then '' else Convert(varchar(20),ISNULL(studentCount,'0')) end as studentCount,case when ISNULL(studmincount,'0')='0' then '' else Convert(varchar(20),ISNULL(studmincount,'0')) end as studentminCount    from electiveSubjectDetails ed,staff_appl_master sam , subject s,staffmaster sm,staff_selector sse where s.subject_no=sse.subject_no and sm.staff_code=sse.staff_code and ed.staffCode=sse.staff_code and ed.staffCode=sm.staff_code and ed.subjectNo=sse.subject_no   and s.subject_code ='" + subcode + "' and sam.appl_id=ed.staffApplId and sse.staff_code ='"+staffcod +"' and sam.appl_no=sm.appl_no order by s.subject_code";
                                    //string qry2 = "select distinct  ed.subjectNo, ed.staffCode,case when ISNULL(studentCount,'0')='0' then '' else Convert(varchar(20),ISNULL(studentCount,'0')) end as studentCount,case when ISNULL(studmincount,'0')='0' then '' else Convert(varchar(20),ISNULL(studmincount,'0')) end as studentminCount  from electiveSubjectDetails ed,subject s,staff_appl_master sam ,staffmaster sm,staff_selector sse where sm.staff_code=sse.staff_code and ed.staffCode=sse.staff_code and ed.staffCode=sm.staff_code and ed.subjectNo=sse.subject_no and s.subject_no=sse.subject_no and s.subject_code in(" + equal_subcode + ") and sam.appl_id=ed.staffApplId and sse.staff_code in (" + stafcode + ") and  sam.appl_no=sm.appl_no";
                                    DataSet minmax = new DataSet();
                                    minmax.Clear();
                                    minmax = d2.select_method_wo_parameter(qry2, "text");                                                                    
                                    if (papcount.Tables.Count > 0 && papcount.Tables[0].Rows.Count > 0)
                                    {
                                       
                                        eq = counteq.NewRow(); 
                                        eq[0] = Convert.ToString(papmat.Rows[j]["subjectDetails"]);
                                        eq[1] = sub_no;
                                        eq[2] = Convert.ToString(papmat.Rows[j]["subject_code"]);

                                        eq[3] = Convert.ToString(papmat.Rows[j]["staffDetails"]);
                                        eq[4] = Convert.ToString(papmat.Rows[j]["appl_id"]);
                                        eq[5] = Convert.ToString(papmat.Rows[j]["staff_code"]);

                                        if (minmax.Tables.Count > 0 && minmax.Tables[0].Rows.Count > 0)
                                        {
                                            DataTable dt1 = new DataTable();
                                            dt1 = minmax.Tables[0].DefaultView.ToTable(true, "subject_name", "subject_code", "subjectDetails", "staff_code", "staff_name", "appl_id", "staffDetails", "studentCount", "studentminCount");

                                           // minmax.Tables[0].DefaultView.RowFilter = "studentCount='" + Convert.ToString(papmat.Rows[j]["studentCount"]) + "' and studentminCount='" + Convert.ToString(papmat.Rows[j]["studentminCount"]) + "' and staff_code='" + Convert.ToString(papmat.Rows[j]["staff_code"]) + "' ";
                                            eq[6] = Convert.ToString(dt1.Rows[0]["studentCount"]);
                                            eq[7] = Convert.ToString(dt1.Rows[0]["studentminCount"]);
                                        }
                                        else
                                        {
                                            string maxcount = "0";
                                            string mincount = "0";
                                            eq[6] = maxcount;
                                            eq[7] = mincount;
                                        }
                                        //eq[6] = Convert.ToString(papmat.Rows[j]["studentCount"]);
                                        //eq[7] = Convert.ToString(papmat.Rows[j]["studentminCount"]);
                                        counteq.Rows.Add(eq);
                                        sub_no = string.Empty;
                                    }
                                }
                                
                            gveqpapmatch.DataSource = counteq;
                            gveqpapmatch.DataBind();
                            btnPrint.Visible = true;
                            btnSave.Visible = true;
                            divMainContent.Visible = false;
                            divmaincontentSectionwise.Visible = false;
                            divmaincontenteqlpapmatch.Visible = true;
                        }
                    }
                }

                else
                {
                    lblAlertMsg.Text = "No Records Found";
                    lblAlertMsg.Visible = true;
                    divPopupAlert.Visible = true;
                    return;

                }
        }
              //***********
            else
             {

                DataTable dtCourseDet = new DataTable();

                DataSet dsCourseDet = new DataSet();
                if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(qryCourse) && !string.IsNullOrEmpty(collegeCode))
                {
                    qry = "select sm.Batch_Year,sm.degree_code,dt.Dept_Name as DegreeName,sm.semester,ISNULL(ss.ElectivePap,'0') as ElectivePap,s.subject_no,s.subject_code,s.subject_name,LTRIM(RTRIM(ISNULL(s.subject_name,'')))+' ( '+s.subject_code+' )' as subjectDetails,sa.appl_id,sfm.staff_code,sfm.staff_name,LTRIM(RTRIM(ISNULL(sfm.staff_name,'')))+' ( '+sfm.staff_code+' )' as staffDetails,sfs.facultyChoice,sfs.staffPriority,sfs.Stud_batch,sfs.Sections,case when ISNULL(ed.studentCount,'0')='0' then '' else Convert(varchar(20),ISNULL(ed.studentCount,'0')) end as studentCount,case when ISNULL(ed.studmincount,'0')='0' then '' else Convert(varchar(20),ISNULL(ed.studmincount,'0')) end as studentminCount from  syllabus_master sm join  sub_sem ss  on  ss.syll_code=sm.syll_code join subject s on sm.syll_code=s.syll_code and ss.subType_no=s.subType_no and s.syll_code=ss.syll_code join staff_selector sfs on s.subject_no=sfs.subject_no join staffmaster sfm on sfm.staff_code=sfs.staff_code join staff_appl_master sa on sa.appl_no=sfm.appl_no join Degree dg on sm.degree_code=dg.Degree_Code join Course c on c.Course_Id=dg.Course_Id join Department dt on dt.Dept_Code=dg.Dept_Code LEFT join electiveSubjectDetails ed on ed.subjectNo=s.subject_no and ed.subjectNo=sfs.subject_no and sa.appl_id=ed.staffApplId and sfm.staff_code=ed.staffCode and sfs.staff_code=ed.staffCode where ISNULL(ss.ElectivePap,'0')='1' " + qryBatch + qryDegreeCode + qrySemester + " order by sm.Batch_Year desc,sm.degree_code,sm.semester,s.subject_code,sfs.staffPriority";

                    qry += "  select Nsections as  NoofSections,degree_code,batch_year  from NDegree where Degree_Code in (" + Degree + ") and batch_year ='" + ddlBatch.SelectedValue + "'";
                    qry += " select e.sectionName,e.subjectNo,e.staffApplId,e.staffCode,e.studentCount,sy.degree_code,ISNULL(e.studmincount,'0') as studentminCount  from electiveSubjectDetails e,syllabus_master sy,subject s where sy.syll_code =s.syll_code and s.subject_no =e.subjectNo and sy.degree_code in (" + Degree + ") and sy.Batch_Year in (" + ddlBatch.SelectedValue + ") and sy.semester ='" + ddlSem.SelectedValue + "' " + CheckValue + "";
                    dsCourseDet.Clear();
                    dsCourseDet = d2.select_method_wo_parameter(qry, "text");

                }
                if (dsCourseDet.Tables.Count > 0 && dsCourseDet.Tables[0].Rows.Count > 0)
                {
                    if (cbSectionWise.Checked == false)
                    {
                        dtCourseDet = dsCourseDet.Tables[0].DefaultView.ToTable(true, "Batch_Year", "degree_code", "DegreeName", "semester", "ElectivePap", "subject_no", "subject_code", "subject_name", "subjectDetails", "appl_id", "staff_code", "staff_name", "staffDetails");
                    }
                    if (cbSectionWise.Checked == true)
                    {
                        dtCourseDet = dsCourseDet.Tables[0].DefaultView.ToTable(true, "Batch_Year", "degree_code", "DegreeName", "semester", "ElectivePap", "subject_no", "subject_code", "subject_name", "subjectDetails", "appl_id", "staff_code", "staff_name", "staffDetails");
                    }
                }
                if (cbSectionWise.Checked == false)
                {
                    if (dtCourseDet.Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        DataRow dr;
                        dt.Columns.Add("DegreeName");
                        dt.Columns.Add("subjectDetails");
                        dt.Columns.Add("subject_no");
                        dt.Columns.Add("subject_code");
                        dt.Columns.Add("staffDetails");
                        dt.Columns.Add("appl_id");
                        dt.Columns.Add("staff_code");
                        dt.Columns.Add("studentCount");
                        dt.Columns.Add("Degree_Code");
                        dt.Columns.Add("studentminCount");

                        if (dtCourseDet.Rows.Count > 0)
                        {
                            for (int intdt = 0; intdt < dtCourseDet.Rows.Count; intdt++)
                            {

                                int studentCount = 0;
                                int minstudentCount = 0;

                                dsCourseDet.Tables[2].DefaultView.RowFilter = "degree_code='" + Convert.ToString(dtCourseDet.Rows[intdt]["Degree_Code"]) + "' and staffApplId='" + Convert.ToString(dtCourseDet.Rows[intdt]["appl_id"]) + "' and subjectNo='" + Convert.ToString(dtCourseDet.Rows[intdt]["subject_no"]) + "'";
                                DataView dvnewsection = dsCourseDet.Tables[2].DefaultView;
                                if (dvnewsection.Count > 0)
                                {
                                    int.TryParse(Convert.ToString(dvnewsection[0]["studentCount"]), out studentCount);
                                    int.TryParse(Convert.ToString(dvnewsection[0]["studentminCount"]), out minstudentCount);
                                }
                                dr = dt.NewRow();
                                dr[0] = Convert.ToString(dtCourseDet.Rows[intdt]["DegreeName"]);
                                dr[1] = Convert.ToString(dtCourseDet.Rows[intdt]["subjectDetails"]);
                                dr[2] = Convert.ToString(dtCourseDet.Rows[intdt]["subject_no"]);
                                dr[3] = Convert.ToString(dtCourseDet.Rows[intdt]["subject_code"]);
                                dr[4] = Convert.ToString(dtCourseDet.Rows[intdt]["staffDetails"]);
                                dr[5] = Convert.ToString(dtCourseDet.Rows[intdt]["appl_id"]);
                                dr[6] = Convert.ToString(dtCourseDet.Rows[intdt]["staff_code"]);
                                dr[7] = Convert.ToString(studentCount);
                                dr[8] = Convert.ToString(dtCourseDet.Rows[intdt]["Degree_Code"]);
                                dr[9] = Convert.ToString(minstudentCount);
                                // dr[9] = Convert.ToString(dtCourseDet.Rows[intdt]["Section"]);
                                dt.Rows.Add(dr);
                            }

                            if (dt.Rows.Count > 0)
                            {
                                gvSectionWiseCount.DataSource = dt;
                                gvSectionWiseCount.DataBind();
                                btnPrint.Visible = true;
                                btnSave.Visible = true;
                                divMainContent.Visible = true;
                                divmaincontentSectionwise.Visible = false;

                            }
                            else
                            {
                                lblAlertMsg.Text = "No Record(s) Found";
                                lblAlertMsg.Visible = true;
                                divPopupAlert.Visible = true;
                                return;
                            }
                        }
                    }
                    else
                    {
                        lblAlertMsg.Text = "No Record(s) Found";
                        lblAlertMsg.Visible = true;
                        divPopupAlert.Visible = true;
                        return;
                    }
                }
                if (cbSectionWise.Checked == true)
                {
                    DataTable dt = new DataTable();
                    DataRow dr;
                    dt.Columns.Add("DegreeName");
                    dt.Columns.Add("subjectDetails");
                    dt.Columns.Add("subject_no");
                    dt.Columns.Add("subject_code");
                    dt.Columns.Add("staffDetails");
                    dt.Columns.Add("appl_id");
                    dt.Columns.Add("staff_code");
                    dt.Columns.Add("Section");
                    dt.Columns.Add("studentCount");
                    dt.Columns.Add("Degree_Code");
                    dt.Columns.Add("studentminCount");

                    if (dtCourseDet.Rows.Count > 0)
                    {
                        for (int intdt = 0; intdt < dtCourseDet.Rows.Count; intdt++)
                        {
                            string degree_code = Convert.ToString(dtCourseDet.Rows[intdt]["Degree_Code"]);
                            dsCourseDet.Tables[1].DefaultView.RowFilter = "Degree_Code='" + degree_code + "'";
                            DataView dvnew = dsCourseDet.Tables[1].DefaultView;
                            if (dvnew.Count > 0)
                            {
                                int Sectioncount = Convert.ToInt32(dvnew[0]["NoofSections"]);
                                for (int intNo = 0; intNo < Sectioncount; intNo++)
                                {
                                    int studentCount = 0;
                                    int minstudentCount = 0;
                                    string Section = GetSection(intNo + 1);
                                    dsCourseDet.Tables[2].DefaultView.RowFilter = "degree_code='" + degree_code + "' and sectionName='" + Section + "' and staffApplId='" + Convert.ToString(dtCourseDet.Rows[intdt]["appl_id"]) + "' and subjectNo='" + Convert.ToString(dtCourseDet.Rows[intdt]["subject_no"]) + "'";
                                    DataView dvnewsection = dsCourseDet.Tables[2].DefaultView;
                                    if (dvnewsection.Count > 0)
                                    {
                                        int.TryParse(Convert.ToString(dvnewsection[0]["studentCount"]), out studentCount);
                                        int.TryParse(Convert.ToString(dvnewsection[0]["studentminCount"]), out minstudentCount);
                                    }
                                    dr = dt.NewRow();
                                    dr[0] = Convert.ToString(dtCourseDet.Rows[intdt]["DegreeName"]);
                                    dr[1] = Convert.ToString(dtCourseDet.Rows[intdt]["subjectDetails"]);
                                    dr[2] = Convert.ToString(dtCourseDet.Rows[intdt]["subject_no"]);
                                    dr[3] = Convert.ToString(dtCourseDet.Rows[intdt]["subject_code"]);
                                    dr[4] = Convert.ToString(dtCourseDet.Rows[intdt]["staffDetails"]);
                                    dr[5] = Convert.ToString(dtCourseDet.Rows[intdt]["appl_id"]);
                                    dr[6] = Convert.ToString(dtCourseDet.Rows[intdt]["staff_code"]);
                                    dr[7] = Convert.ToString(Section);
                                    dr[8] = Convert.ToString(studentCount);
                                    dr[9] = Convert.ToString(dtCourseDet.Rows[intdt]["Degree_Code"]);
                                    dr[10] = Convert.ToString(minstudentCount);
                                    // dr[9] = Convert.ToString(dtCourseDet.Rows[intdt]["Section"]);
                                    dt.Rows.Add(dr);
                                }
                            }
                        }

                        if (dt.Rows.Count > 0)
                        {
                            DivgvSectionWiseCount.DataSource = dt;
                            DivgvSectionWiseCount.DataBind();
                            divmaincontentSectionwise.Visible = true;
                            btnPrint.Visible = true;
                            btnSave.Visible = true;
                            divMainContent.Visible = false;

                        }
                        else
                        {
                            lblAlertMsg.Text = "No Record(s) Found";
                            lblAlertMsg.Visible = true;
                            divPopupAlert.Visible = true;
                            return;
                        }
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
            //added by Mullai
            if (cbequalpapcount.Checked == true)
            {
                bool isSav = false;
                bool isvaid = true;
                foreach (GridViewRow gveqmat in gveqpapmatch.Rows)
                {
                    TextBox txtStudentcount = (TextBox)gveqmat.FindControl("txtStudentCount");
                    TextBox txtStudentCount_min = (TextBox)gveqmat.FindControl("txtStudentCount_min");
                    string studentCount = txtStudentcount.Text;
                    int studCount = 0;
                    int.TryParse(studentCount, out studCount);
                    String student_cnt = txtStudentCount_min.Text;
                    int stud_mcount = 0;
                    int.TryParse(student_cnt, out stud_mcount);
                    if (studCount < stud_mcount)
                    {
                        isvaid = false;
                    }

                }
                if (isvaid)
                {
                    int inserted = 0;
                    foreach (GridViewRow gveqmat in gveqpapmatch.Rows)
                    {
                        TextBox txtStudcount = (TextBox)gveqmat.FindControl("txtStudentCount");
                        TextBox txtminStudentCount = (TextBox)gveqmat.FindControl("txtStudentCount_min");
                        Label lblSubjectCode = (Label)gveqmat.FindControl("lblSubjectCode");
                       
                        Label lblSubjectNo = (Label)gveqmat.FindControl("lblSubjectNo");

                        string sunum=Convert.ToString(lblSubjectNo.Text);
                        //string[] subnum=sunum.Split(';');
                        //string newsubno=string.Empty;

                        Label lblStaffApplId = (Label)gveqmat.FindControl("lblStaffApplId");
                        Label lblStaffCode = (Label)gveqmat.FindControl("lblStaffCode");
                        string studentCount = txtStudcount.Text;
                        int studCount = 0;
                        int.TryParse(studentCount, out studCount);

                        //min student Count
                        String student_cnt = txtminStudentCount.Text;
                        int stud_mcount = 0;
                        int.TryParse(student_cnt, out stud_mcount);
                        if (!string.IsNullOrEmpty(lblSubjectNo.Text.Trim()) && studCount > 0 && !string.IsNullOrEmpty(lblStaffApplId.Text.Trim()) && !string.IsNullOrEmpty(lblStaffCode.Text.Trim()))
                        {
                            string[] subnum = sunum.Split(',');
                            for (int s = 0; s < subnum.Length; s++)
                            {
                                string newsubno = Convert.ToString(subnum[s]).Trim();

                                qry = "if exists (select * from electiveSubjectDetails where subjectNo='" + newsubno + "' and staffApplId='" + lblStaffApplId.Text.Trim() + "' and staffCode='" + lblStaffCode.Text.Trim() + "' ) update electiveSubjectDetails set studentCount='" + studCount + "' , studmincount = '" + stud_mcount + "' where subjectNo='" + newsubno.Trim() + "' and staffApplId='" + lblStaffApplId.Text.Trim() + "' and staffCode='" + lblStaffCode.Text.Trim() + "'  else insert into electiveSubjectDetails (subjectNo,staffApplId,staffCode,studentCount,studmincount) values('" + newsubno + "','" + lblStaffApplId.Text.Trim() + "','" + lblStaffCode.Text.Trim() + "','" + studCount + "','" + stud_mcount + "')";
                               inserted = d2.update_method_wo_parameter(qry, "text");
                            }
                         
                            if (inserted > 0)
                            {
                                isSav = true;
                            }
                        }
                    }
                
                 if (isSav)
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
                else
                {
                    lblAlertMsg.Text = "Not Saved!Pls Check MIN MAX Count";
                    lblAlertMsg.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }





            }
                //***********


            else
            {
                bool isSave = false;
                bool isvalid = true;
                foreach (GridViewRow gvSecRow in gvSectionWiseCount.Rows)
                {
                    TextBox txtStudentCount = (TextBox)gvSecRow.FindControl("txtStudentCount");
                    TextBox txtStudentCount_min = (TextBox)gvSecRow.FindControl("txtStudentCount_min");
                    //lblTotSeats
                    string studentCount = txtStudentCount.Text;
                    int studCount = 0;
                    int.TryParse(studentCount, out studCount);

                    //min student Count
                    String student_cnt = txtStudentCount_min.Text;
                    int stud_mcount = 0;
                    int.TryParse(student_cnt, out stud_mcount);
                    if (studCount < stud_mcount)
                    {
                        isvalid = false;
                    }
                }
                if (isvalid)
                {
                    if (cbSectionWise.Checked == false)
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
                                TextBox txtStudentCount_min = (TextBox)gvSecRow.FindControl("txtStudentCount_min");
                                Label lblStaffApplId = (Label)gvSecRow.FindControl("lblStaffApplId");
                                Label lblStaffCode = (Label)gvSecRow.FindControl("lblStaffCode");
                                Label lblSubjectNo = (Label)gvSecRow.FindControl("lblSubjectNo");

                                //lblTotSeats
                                string studentCount = txtStudentCount.Text;
                                int studCount = 0;
                                int.TryParse(studentCount, out studCount);

                                //min student Count
                                String student_cnt = txtStudentCount_min.Text;
                                int stud_mcount = 0;
                                int.TryParse(student_cnt, out stud_mcount);


                                if (!string.IsNullOrEmpty(lblSubjectNo.Text.Trim()) && studCount > 0 && !string.IsNullOrEmpty(lblStaffApplId.Text.Trim()) && !string.IsNullOrEmpty(lblStaffCode.Text.Trim()))
                                {
                                    qry = "if exists (select * from electiveSubjectDetails where subjectNo='" + lblSubjectNo.Text.Trim() + "' and staffApplId='" + lblStaffApplId.Text.Trim() + "' and staffCode='" + lblStaffCode.Text.Trim() + "' and isnull(issectionwise,'0')=0) update electiveSubjectDetails set studentCount='" + studCount + "' , studmincount = '" + stud_mcount + "' where subjectNo='" + lblSubjectNo.Text.Trim() + "' and staffApplId='" + lblStaffApplId.Text.Trim() + "' and staffCode='" + lblStaffCode.Text.Trim() + "' else insert into electiveSubjectDetails (subjectNo,staffApplId,staffCode,studentCount,issectionwise,studmincount) values('" + lblSubjectNo.Text.Trim() + "','" + lblStaffApplId.Text.Trim() + "','" + lblStaffCode.Text.Trim() + "','" + studCount + "',0,'" + stud_mcount + "')";
                                    qry += " update Ndegree set ElectiveSelection='0' where Degree_code ='" + lbldegreeCode.Text + "' and batch_year ='" + batchYear + "' and college_code ='" + ddlCollege.SelectedValue + "'";
                                    int inserted = d2.update_method_wo_parameter(qry, "text");
                                    if (inserted > 0)
                                    {
                                        isSave = true;
                                    }
                                }
                            }
                        }
                    }
                    if (cbSectionWise.Checked == true)
                    {
                        if (DivgvSectionWiseCount.Rows.Count > 0)
                        {
                            foreach (GridViewRow gvSecRow in DivgvSectionWiseCount.Rows)
                            {
                                string batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
                                string degreeCode = Convert.ToString(ddlBatch.SelectedValue).Trim();
                                Label lbldegreeCode = (Label)gvSecRow.FindControl("lblDegreeCode");
                                degreeCode = Convert.ToString(lbldegreeCode.Text).Trim();
                                TextBox txtStudentCount = (TextBox)gvSecRow.FindControl("txtStudentCount");
                                TextBox txtStudentCount_min = (TextBox)gvSecRow.FindControl("txtStudentCount_min");//
                                Label lblStaffApplId = (Label)gvSecRow.FindControl("lblStaffApplId");
                                Label lblStaffCode = (Label)gvSecRow.FindControl("lblStaffCode");
                                Label lblSubjectNo = (Label)gvSecRow.FindControl("lblSubjectNo");
                                Label lblsection = (Label)gvSecRow.FindControl("lblSection");
                                string studentCount = txtStudentCount.Text;
                                int studCount = 0;
                                int.TryParse(studentCount, out studCount);

                                //min student Count
                                String student_cnt = txtStudentCount_min.Text;
                                int stud_mcount = 0;
                                int.TryParse(student_cnt, out stud_mcount);

                                if (!string.IsNullOrEmpty(lblSubjectNo.Text.Trim()) && studCount > 0 && !string.IsNullOrEmpty(lblStaffApplId.Text.Trim()) && !string.IsNullOrEmpty(lblStaffCode.Text.Trim()))
                                {
                                    qry = "if exists (select * from electiveSubjectDetails where subjectNo='" + lblSubjectNo.Text.Trim() + "' and staffApplId='" + lblStaffApplId.Text.Trim() + "' and staffCode='" + lblStaffCode.Text.Trim() + "' and SectionName='" + lblsection.Text + "') update electiveSubjectDetails set studentCount='" + studCount + "' , studmincount = '" + stud_mcount + "' where subjectNo='" + lblSubjectNo.Text.Trim() + "' and staffApplId='" + lblStaffApplId.Text.Trim() + "' and staffCode='" + lblStaffCode.Text.Trim() + "' and SectionName='" + lblsection.Text + "' else insert into electiveSubjectDetails (subjectNo,staffApplId,staffCode,studentCount,SectionName,IsSectionWise,studmincount) values('" + lblSubjectNo.Text.Trim() + "','" + lblStaffApplId.Text.Trim() + "','" + lblStaffCode.Text.Trim() + "','" + studCount + "','" + lblsection.Text + "',1,'" + stud_mcount + "')";
                                    qry += " update Ndegree set ElectiveSelection='1' where Degree_code ='" + lbldegreeCode.Text + "' and batch_year ='" + batchYear + "' and college_code ='" + ddlCollege.SelectedValue + "'";
                                    int inserted = d2.update_method_wo_parameter(qry, "text");
                                    if (inserted > 0)
                                    {
                                        isSave = true;
                                    }
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
                else
                {
                    lblAlertMsg.Text = "Not Saved!Pls Check MIN MAX Count";
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

            // Label lblDegreeCode_min = (Label)grdrow.FindControl("lblDegreeCode_min");
            txtStudentCount.Enabled = true;
            txtStudentCount.Attributes.Add("style", "display:block;");
            lblStudentCount.Attributes.Add("style", "display:none;");



            if (!string.IsNullOrEmpty(txtStudentCount.Text.Trim()) && txtStudentCount.Text.Trim() != "0")
            {
                txtStudentCount.Attributes.Add("style", "display:none;");
                lblStudentCount.Attributes.Add("style", "display:block;");
            }

            //min student count
            Label lblStudentCount_min = (Label)grdrow.FindControl("lblStudentCount_min");
            TextBox txtStudentCount_min = (TextBox)grdrow.FindControl("txtStudentCount_min");
            txtStudentCount_min.Enabled = true;
            if (!string.IsNullOrEmpty(txtStudentCount_min.Text.Trim()) && txtStudentCount_min.Text.Trim() != "0")
            {
                txtStudentCount_min.Attributes.Add("style", "display:block;");
                lblStudentCount_min.Attributes.Add("style", "display:none;");
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

            cs.RegisterArrayDeclaration("gvStudentMinCount", String.Concat("'", txtStudentCount_min.ClientID, "'"));
            cs.RegisterArrayDeclaration("gvStudentMinCount_lbl", String.Concat("'", lblStudentCount_min.ClientID, "'"));
        }
    }

    // count check

    protected void gveqpapmatch_DataBound(object sender, EventArgs e)
    {
      
        for (int i = gveqpapmatch.Rows.Count - 1; i > 0; i--)
        {
            GridViewRow row = gveqpapmatch.Rows[i];
            GridViewRow previousRow = gveqpapmatch.Rows[i - 1];
            for (int j = 1; j <= 2; j++)
            {
                bool validation = false;
                Label lblCurrent = new Label();
                Label lblPrevious = new Label();
                string columnName = string.Empty;
                switch (j)
                {
                    case 1:
                        columnName = "lblSubjectDet";
                        break;
                    case 2:
                        columnName = "lblStaffName";
                        break;
                }
                lblCurrent = (Label)row.FindControl(columnName);
                lblCurrent = (Label)previousRow.FindControl(columnName);
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
    
    protected void gveqpapmatch_PreRender(object sender, EventArgs e)
    {
        ClientScriptManager cs = Page.ClientScript;
        foreach (GridViewRow grdrow in gveqpapmatch.Rows)
        {
            grdrow.Cells[4].Attributes.Add("ondblclick", "return clickCells(" + grdrow.RowIndex + ")");
            Label lblSubjectDet = (Label)grdrow.FindControl("lblSubjectDet");
            Label lblSubjectCode = (Label)grdrow.FindControl("lblSubjectCode");
            Label lblSubjectNo = (Label)grdrow.FindControl("lblSubjectNo");

            Label lblStaffName = (Label)grdrow.FindControl("lblStaffName");
            Label lblStaffCode = (Label)grdrow.FindControl("lblStaffCode");
            Label lblStaffApplId = (Label)grdrow.FindControl("lblStaffApplId");

           // Label lblStudentCount = (Label)grdrow.FindControl("lblStudentCount");
            TextBox txtStudentCount = (TextBox)grdrow.FindControl("txtStudentCount");
           // Label lblStudentCount_min = (Label)grdrow.FindControl("lblStudentCount_min");
            TextBox txtStudentCount_min = (TextBox)grdrow.FindControl("txtStudentCount_min");

            txtStudentCount.Enabled = true;
            txtStudentCount.Attributes.Add("style", "display:block;");
           // lblStudentCount.Attributes.Add("style", "display:none;");

            if (!string.IsNullOrEmpty(txtStudentCount.Text.Trim()) && txtStudentCount.Text.Trim() != "0")
            {
                txtStudentCount.Attributes.Add("style", "display:block;");
               // lblStudentCount.Attributes.Add("style", "display:block;");
            }

            txtStudentCount_min.Enabled = true;
            if (!string.IsNullOrEmpty(txtStudentCount_min.Text.Trim()) && txtStudentCount_min.Text.Trim() != "0")
            {
                txtStudentCount_min.Attributes.Add("style", "display:block;");
               // lblStudentCount_min.Attributes.Add("style", "display:none;");
            }
            txtStudentCount.Attributes.Add("onfocusout", "return focusOut(" + grdrow.RowIndex + ")");
            txtStudentCount.Attributes.Add("onclick", "return textBoxClick(" + grdrow.RowIndex + ")");
            cs.RegisterArrayDeclaration("gvSubjectDet", String.Concat("'", lblSubjectDet.ClientID, "'"));
            cs.RegisterArrayDeclaration("gvSubjectCode", String.Concat("'", lblSubjectCode.ClientID, "'"));
            cs.RegisterArrayDeclaration("gvSubjectNo", String.Concat("'", lblSubjectNo.ClientID, "'"));

            cs.RegisterArrayDeclaration("gvStaffDet", String.Concat("'", lblStaffName.ClientID, "'"));
            cs.RegisterArrayDeclaration("gvStaffCode", String.Concat("'", lblStaffCode.ClientID, "'"));
            cs.RegisterArrayDeclaration("gvStaffApplId", String.Concat("'", lblStaffApplId.ClientID, "'"));
            cs.RegisterArrayDeclaration("gvStudentCount", String.Concat("'", txtStudentCount.ClientID, "'"));
           // cs.RegisterArrayDeclaration("gvStudentCount_lbl", String.Concat("'", lblStudentCount.ClientID, "'"));

            cs.RegisterArrayDeclaration("gvStudentMinCount", String.Concat("'", txtStudentCount_min.ClientID, "'"));
           // cs.RegisterArrayDeclaration("gvStudentMinCount_lbl", String.Concat("'", lblStudentCount_min.ClientID, "'"));

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

    public void txtStudentCount_textChange(object sender, EventArgs e)
    {
        CountCheck();
    }

    public void txtStudentCount_min_textChange(object sender, EventArgs e)
    {
        CountCheck();

    }

    public void CountCheck()
    {
        try
        {
        foreach (GridViewRow gvSecRow in gvSectionWiseCount.Rows)
        {
            Label err_lable1 = new Label();
            TextBox txtStudentCount = (TextBox)gvSecRow.FindControl("txtStudentCount");
            TextBox txtStudentCount_min = (TextBox)gvSecRow.FindControl("txtStudentCount_min");
            err_lable1 = (Label)gvSecRow.FindControl("err_lable");
            String mxcount = txtStudentCount.Text;
            String mincount = txtStudentCount_min.Text;

            int min_count = 0;
            int.TryParse(mincount, out min_count);
            int max_count=0;
            int.TryParse(mxcount, out max_count);
            if ((min_count > max_count) || (max_count < min_count))
            {
                lblAlertMsg.Text = "please Enter valid minimum maximum count";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }

        }
        }
        catch
        {
        }
    }

}
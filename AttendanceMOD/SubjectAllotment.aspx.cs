using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Farpoint = FarPoint.Web.Spread;
using wc = System.Web.UI.WebControls;
using InsproDataAccess;

public partial class AttendanceMOD_SubjectAllotment : System.Web.UI.Page
{

    #region Field Declaration

    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable ht = new Hashtable();

    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;

    string collegeCode = string.Empty;
    string batchYear = string.Empty;
    string courseId = string.Empty;
    string degreeCode = string.Empty;
    string semester = string.Empty;
    string section = string.Empty;
    string subjectType = string.Empty;
    string hours = string.Empty;
    string fromDate = string.Empty;
    string toDate = string.Empty;
    string orderBy = string.Empty;
    string orderBySetting = string.Empty;

    string qry = string.Empty;
    string qryCollegeCode = string.Empty;
    string qryBatchYear = string.Empty;
    string qryDegreeCode = string.Empty;
    string qrySemester = string.Empty;
    string qrySection = string.Empty;
    string qrySubjectType = string.Empty;
    string qryCourseId = string.Empty;
    string qryHours = string.Empty;

    byte dayType = 0;
    byte totalHours = 0;
    int selected = 0;

    Institution institute;
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();

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
            else
            {
                userCollegeCode = Convert.ToString(Session["collegecode"]).Trim();
                userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            }
            if (!IsPostBack)
            {
                lblAlertMsg.Text = string.Empty;
                divPopAlert.Visible = false;
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                divMainContents.Visible = false;

                setLabelText();
                Bindcollege();
                BindBatch();
                BindDegree();
                BindBranch();
                BindSem();
                BindSectionDetail();
                BindSubjectType();

                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Admissionno"] = "0";
                string grouporusercode = string.Empty;
                if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " group_code='" + Convert.ToString(Session["group_code"]).Trim().Split(',')[0] + "' ";
                }
                else
                {
                    grouporusercode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "' ";
                }
                ds.Clear();
                ht.Clear();
                if (!string.IsNullOrEmpty(grouporusercode))
                {
                    string Master1 = "select * from Master_Settings where " + grouporusercode + "";
                    ds = da.select_method(Master1, ht, "Text");
                }
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "roll no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim().ToLower() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "register no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim().ToLower() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "student_type" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim().ToLower() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "admission no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim().ToLower() == "1")
                        {
                            Session["Admissionno"] = "1";
                        }
                    }
                }
            }
        }
        catch (ThreadAbortException tt)
        {
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion Page Load

    #region Bind Header

    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            ht.Clear();
            ht.Add("column_field", Convert.ToString(columnfield));
            DataSet dsprint = da.select_method("bind_college", ht, "sp");
            ddlCollege.Items.Clear();
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = dsprint;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindBatch()
    {
        try
        {
            qryUserOrGroupCode = string.Empty;
            groupUserCode = string.Empty;
            collegeCode = string.Empty;
            userCode = string.Empty;
            ddlBatch.Items.Clear();
            ds.Clear();
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    groupUserCode = Convert.ToString(group_semi[0]);
                }
                else
                {
                    groupUserCode = group;
                }
                if (!string.IsNullOrEmpty(groupUserCode))
                {
                    qryUserOrGroupCode = " and user_id='" + groupUserCode + "'";
                }
            }
            else
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                if (!string.IsNullOrEmpty(userCode))
                {
                    qryUserOrGroupCode = " and user_id='" + userCode + "'";
                }
            }
            if (ddlCollege.Items.Count > 0)
            {
                //collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                collegeCode = string.Empty;
                Control c = ddlCollege;
                if (c is DropDownList)
                {
                    collegeCode = "'" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                //if (!string.IsNullOrEmpty(collegeCode))
                //{
                //    qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
                //}
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                string strbinddegree = "select distinct batch_year from tbl_attendance_rights where college_code in(" + collegeCode + ") and batch_year<>'' and batch_year<>'0' and batch_year<>'-1'  " + qryUserOrGroupCode + " order by batch_year desc";
                ds = da.select_method_wo_parameter(strbinddegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "Batch_year";
                ddlBatch.DataValueField = "Batch_year";
                ddlBatch.DataBind();
                ddlBatch.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindDegree()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ddlDegree.Items.Clear();
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            collegeCode = string.Empty;
            qryCollegeCode = string.Empty;
            batchYear = string.Empty;
            qryUserOrGroupCode = string.Empty;
            groupUserCode = string.Empty;
            userCode = string.Empty;
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            if (!string.IsNullOrEmpty(groupUserCode) && Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                qryUserOrGroupCode = " and dp.group_code='" + groupUserCode.Trim() + "'";
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                qryUserOrGroupCode = " and dp.user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = string.Empty;
                Control c = ddlCollege;
                if (c is DropDownList)
                {
                    collegeCode = "'" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
                }
            }
            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                batchYear = string.Empty;
                Control c = ddlBatch;
                if (c is DropDownList)
                {
                    batchYear = "'" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and r.Batch_year in(" + batchYear + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                ds = da.select_method_wo_parameter("select distinct dg.course_id,c.course_name,c.Priority from Degree dg,Course c,Department dt,DeptPrivilages dp where  dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code  " + qryCollegeCode + qryUserOrGroupCode + "  order by c.Priority", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlDegree.DataSource = ds;
                    ddlDegree.DataTextField = "course_name";
                    ddlDegree.DataValueField = "course_id";
                    ddlDegree.DataBind();
                    ddlDegree.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindBranch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ddlBranch.Items.Clear();
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            collegeCode = string.Empty;
            qryCollegeCode = string.Empty;
            batchYear = string.Empty;
            courseId = string.Empty;
            qryCourseId = string.Empty;
            qryBatchYear = string.Empty;
            qryUserOrGroupCode = string.Empty;
            groupUserCode = string.Empty;
            userCode = string.Empty;
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            if (!string.IsNullOrEmpty(groupUserCode) && Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                qryUserOrGroupCode = " and dp.group_code='" + groupUserCode.Trim() + "'";
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                qryUserOrGroupCode = " and dp.user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            selected = 0;
            collegeCode = string.Empty;
            qryCollegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = string.Empty;
                Control c = ddlCollege;
                if (c is DropDownList)
                {
                    collegeCode = "'" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
                }
            }
            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                batchYear = string.Empty;
                Control c = ddlBatch;
                if (c is DropDownList)
                {
                    batchYear = "'" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and r.Batch_year in(" + batchYear + ")";
                }
            }
            if (ddlDegree.Items.Count > 0)
            {
                courseId = string.Empty;
                Control c = ddlDegree;
                if (c is DropDownList)
                {
                    courseId = "'" + Convert.ToString(ddlDegree.SelectedValue).Trim() + "'"; ;
                }
                else
                {
                    foreach (ListItem li in ddlDegree.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(courseId))
                            {
                                courseId = "'" + li.Value + "'";
                            }
                            else
                            {
                                courseId += ",'" + li.Value + "'";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(courseId))
                {
                    qryCourseId = " and c.Course_Id in(" + courseId + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryCourseId) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                ds = da.select_method_wo_parameter("select distinct dg.Degree_Code,dt.Dept_Name from Degree dg,Course c,Department dt,DeptPrivilages dp where  dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code  " + qryCourseId + qryUserOrGroupCode + "order by dg.Degree_Code", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlBranch.DataSource = ds;
                    ddlBranch.DataTextField = "dept_name";
                    ddlBranch.DataValueField = "degree_code";
                    ddlBranch.DataBind();
                    ddlBranch.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindSem()
    {
        try
        {
            ds.Clear();
            ddlSem.Items.Clear();
            bool first_year = false;
            int duration = 0;
            int i = 0;
            selected = 0;
            qryCollegeCode = string.Empty;
            collegeCode = string.Empty;
            qryBatchYear = string.Empty;
            batchYear = string.Empty;
            courseId = string.Empty;
            qryCourseId = string.Empty;
            qryDegreeCode = string.Empty;
            degreeCode = string.Empty;

            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = string.Empty;
                Control c = ddlCollege;
                if (c is DropDownList)
                {
                    collegeCode = "'" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and college_code in(" + collegeCode + ")";
                }
            }
            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                batchYear = string.Empty;
                Control c = ddlBatch;
                if (c is DropDownList)
                {
                    batchYear = "'" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and Batch_year in(" + batchYear + ")";
                }
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = string.Empty;
                Control c = ddlBranch;
                if (c is DropDownList)
                {
                    degreeCode = "'" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and degree_code in(" + degreeCode + ")";
                }
            }
            string sqlnew = string.Empty;
            if (!string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryBatchYear))
            {
                sqlnew = "select distinct max(ndurations) ndurations,first_year_nonsemester from ndegree dg where ndurations<>'0'" + qryDegreeCode + qryCollegeCode + qryBatchYear + " group by first_year_nonsemester";
                ds.Clear();
                ds = da.select_method_wo_parameter(sqlnew, "Text");
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
            }
            else
            {
                if (!string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollegeCode))
                {
                    sqlnew = "select distinct max(duration) duration,first_year_nonsemester from degree where duration<>'0' " + qryDegreeCode + qryCollegeCode + " group by first_year_nonsemester";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(sqlnew, "Text");
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
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindSectionDetail()
    {
        try
        {
            qryUserOrGroupCode = string.Empty;
            groupUserCode = string.Empty;
            userCode = string.Empty;
            collegeCode = string.Empty;
            batchYear = string.Empty;
            degreeCode = string.Empty;
            ds = new DataSet();
            ddlSec.Items.Clear();

            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = string.Empty;
                Control c = ddlCollege;
                if (c is DropDownList)
                {
                    collegeCode = "'" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                //if (!string.IsNullOrEmpty(collegeCode))
                //{
                //    qryCollegeCode = " and college_code in(" + collegeCode + ")";
                //}
            }
            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
            {
                batchYear = string.Empty;
                Control c = ddlBatch;
                if (c is DropDownList)
                {
                    batchYear = "'" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                //if (!string.IsNullOrEmpty(batchYear))
                //{
                //    qryBatchYear = " and Batch_year in(" + batchYear + ")";
                //}
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = string.Empty;
                Control c = ddlBranch;
                if (c is DropDownList)
                {
                    degreeCode = "'" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                //if (!string.IsNullOrEmpty(degreeCode))
                //{
                //    qryDegreeCode = " and degree_code in(" + degreeCode + ")";
                //}
            }
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    groupUserCode = Convert.ToString(group_semi[0]);
                }
                else
                {
                    groupUserCode = group;
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
            string qrysections = string.Empty;
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                qrysections = da.GetFunctionv("select distinct sections from tbl_attendance_rights where college_code in(" + collegeCode + ") and batch_year in(" + batchYear + ")  " + qryUserOrGroupCode).Trim();
            }
            if (!string.IsNullOrEmpty(qrysections.Trim()))
            {
                string[] sectionsAll = qrysections.Trim().Split(new char[] { ',' });
                string sections = string.Empty;
                bool hasEmpty = false;
                if (sectionsAll.Length > 0)
                {
                    for (int sec = 0; sec < sectionsAll.Length; sec++)
                    {
                        if (!string.IsNullOrEmpty(sectionsAll[sec].Trim()))
                        {
                            if (sections.Trim() == "")
                            {
                                sections = "'" + sectionsAll[sec] + "'";
                            }
                            else
                            {
                                sections += ",'" + sectionsAll[sec] + "'";
                            }
                        }
                        else if (!hasEmpty)
                        {
                            hasEmpty = true;
                            if (sections.Trim() == "")
                            {
                                sections = "'" + sectionsAll[sec] + "'";
                            }
                            else
                            {
                                sections += ",'" + sectionsAll[sec] + "'";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(sections.Trim()))
                {
                    string sqlnew = "select distinct sections from registration where batch_year in(" + Convert.ToString(batchYear).Trim() + ") and degree_code in(" + Convert.ToString(degreeCode).Trim() + ") and sections<>'-1' and sections<>' ' and college_code in(" + Convert.ToString(collegeCode).Trim() + ") and sections in(" + sections + ") and delflag='0' and cc='0' and exam_flag<>'Debar' order by sections";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(sqlnew, "Text");
                }
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSec.DataSource = ds;
                ddlSec.DataTextField = "sections";
                ddlSec.DataValueField = "sections";
                ddlSec.DataBind();
                ddlSec.Enabled = true;

            }
            else
            {
                ddlSec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindSubjectType()
    {
        try
        {
            qryUserOrGroupCode = string.Empty;
            groupUserCode = string.Empty;
            userCode = string.Empty;
            collegeCode = string.Empty;
            batchYear = string.Empty;
            degreeCode = string.Empty;
            ds = new DataSet();
            ddlSubejctType.Items.Clear();
            chkSubjectType.Checked = false;
            cblSubjectType.Items.Clear();
            txtSubjectType.Text = "-- Select --";
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = string.Empty;
                Control c = ddlCollege;
                if (c is DropDownList)
                {
                    collegeCode = "'" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
            }
            if (ddlBatch.Items.Count > 0)
            {
                batchYear = string.Empty;
                Control c = ddlBatch;
                if (c is DropDownList)
                {
                    batchYear = "'" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                //if (!string.IsNullOrEmpty(batchYear))
                //{
                //    qryBatchYear = " and Batch_year in(" + batchYear + ")";
                //}
            }
            if (ddlBranch.Items.Count > 0)
            {
                degreeCode = string.Empty;
                Control c = ddlBranch;
                if (c is DropDownList)
                {
                    degreeCode = "'" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                //if (!string.IsNullOrEmpty(degreeCode))
                //{
                //    qryDegreeCode = " and degree_code in(" + degreeCode + ")";
                //}
            }

            if (ddlSem.Items.Count > 0)
            {
                semester = string.Empty;
                Control c = ddlSem;
                if (c is DropDownList)
                {
                    semester = "'" + Convert.ToString(ddlSem.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                //if (!string.IsNullOrEmpty(degreeCode))
                //{
                //    qryDegreeCode = " and semester in(" + degreeCode + ")";
                //}
            }


            if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester))
            {
                string sqlnew = "select distinct ss.subType_no,ss.subject_type from subject s,Syllabus_Master sm,sub_sem ss where s.subType_no=ss.subType_no and ss.syll_code=s.syll_code and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ISNULL(ss.ElectivePap,'0')='0' and sm.Batch_Year in(" + batchYear + ") and sm.degree_code in(" + degreeCode + ") and sm.semester in(" + semester + ")  order by ss.subType_no";

                //sqlnew = "select distinct ss.subType_no,ss.subject_type from TT_ClassTimetable ct,TT_ClassTimetableDet ctd,subject s,Syllabus_Master sm,sub_sem ss where s.subType_no=ss.subType_no and ss.syll_code=s.syll_code and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and s.subject_no=ctd.TT_subno and ct.TT_ClassPK=ctd.TT_ClassFK and sm.Batch_Year=ct.TT_batchyear and sm.degree_code=ct.TT_degCode and sm.semester=ct.TT_Sem and ct.TT_lastRec=1 and ISNULL(ss.ElectivePap,'0')='0' and ct.TT_colCode in(" + collegeCode + ")  and sm.Batch_Year in(" + batchYear + ") and sm.degree_code in(" + degreeCode + ") and sm.semester in(" + semester + ")  order by ss.subType_no"; //and ct.TT_sec in(" + section + ")
                ds.Clear();
                ds = da.select_method_wo_parameter(sqlnew, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSubejctType.DataSource = ds;
                ddlSubejctType.DataTextField = "subject_type";
                ddlSubejctType.DataValueField = "subType_no";
                ddlSubejctType.DataBind();
                ddlSubejctType.Enabled = true;
                txtSubjectType.Enabled = true;
                cblSubjectType.DataSource = ds;
                cblSubjectType.DataTextField = "subject_type";
                cblSubjectType.DataValueField = "subType_no";
                cblSubjectType.DataBind();
                foreach (ListItem li in cblSubjectType.Items)
                {
                    li.Selected = true;
                }
                txtSubjectType.Text = "Subject" + "(" + cblSubjectType.Items.Count + ")";
                chkSubjectType.Checked = true;
            }
            else
            {
                txtSubjectType.Enabled = false;
                ddlSubejctType.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    private void setLabelText()
    {
        try
        {
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim().Split(',')[0] + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            institute = new Institution(grouporusercode);
            List<Label> lbl = new List<Label>();
            List<byte> fields = new List<byte>();
            lbl.Add(lblCollege);
            lbl.Add(lblDegree);
            lbl.Add(lblBranch);
            lbl.Add(lblSem);
            fields.Add(0);
            fields.Add(2);
            fields.Add(3);
            fields.Add(4);
            if (institute != null && institute.TypeInstitute == 1)
            {
                lblBatch.Text = "Year";
            }
            else
            {
                lblBatch.Text = "Batch";
            }
            new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void Init_Spread(Farpoint.FpSpread FpSpread1, int type = 0)
    {
        try
        {
            #region FpSpread Style

            //FpSpread1.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;

            #endregion FpSpread Style

            #region SpreadStyles

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = System.Drawing.Color.Black;
            FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
            sheetstyle.Font.Name = "Book Antiqua";
            sheetstyle.Font.Size = FontUnit.Medium;
            sheetstyle.Font.Bold = true;
            sheetstyle.HorizontalAlign = HorizontalAlign.Center;
            sheetstyle.VerticalAlign = VerticalAlign.Middle;
            sheetstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Border.BorderSize = 1;
            sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

            #endregion SpreadStyles

            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.ForeColor = Color.Black;
            FpSpread1.Sheets[0].DefaultStyle = sheetstyle;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.HorizontalScrollBarPolicy = Farpoint.ScrollBarPolicy.AsNeeded;
            FpSpread1.VerticalScrollBarPolicy = Farpoint.ScrollBarPolicy.AsNeeded;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].DefaultStyle.HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.RowHeader.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.ShowHeaderSelection = false;
            DataSet dsSettings = new DataSet();
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " and  group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            if (!string.IsNullOrEmpty(grouporusercode))
            {
                string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type') and value='1' " + grouporusercode + "";
                dsSettings = da.select_method_wo_parameter(Master1, "Text");
            }
            bool isRollVisible = ColumnHeaderVisiblity(0, dsSettings);
            bool isRegVisible = ColumnHeaderVisiblity(1, dsSettings);
            bool isAdmitNoVisible = ColumnHeaderVisiblity(2, dsSettings);
            bool isStudentTypeVisible = ColumnHeaderVisiblity(3, dsSettings);
            if (type == 0)
            {
                FpSpread1.Sheets[0].ColumnCount = 11;
                FpSpread1.Sheets[0].Columns[0].Width = 80;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                FpSpread1.Sheets[0].Columns[1].Width = 100;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Visible = isRollVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                FpSpread1.Sheets[0].Columns[2].Width = 150;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Visible = isRegVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

                FpSpread1.Sheets[0].Columns[3].Width = 150;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Visible = isAdmitNoVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

                FpSpread1.Sheets[0].Columns[4].Width = 200;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Visible = isStudentTypeVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

                FpSpread1.Sheets[0].Columns[5].Width = 200;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Mode of Admission";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

                FpSpread1.Sheets[0].Columns[6].Width = 280;
                FpSpread1.Sheets[0].Columns[6].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Student Name";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);

                FpSpread1.Sheets[0].Columns[7].Width = 150;
                FpSpread1.Sheets[0].Columns[7].Locked = true;
                FpSpread1.Sheets[0].Columns[7].Resizable = false;
                FpSpread1.Sheets[0].Columns[7].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = lblDegree.Text;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);

                FpSpread1.Sheets[0].Columns[8].Width = 100;
                FpSpread1.Sheets[0].Columns[8].Locked = true;
                FpSpread1.Sheets[0].Columns[8].Resizable = false;
                FpSpread1.Sheets[0].Columns[8].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = lblBranch.Text;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);

                FpSpread1.Sheets[0].Columns[9].Width = 70;
                FpSpread1.Sheets[0].Columns[9].Locked = true;
                FpSpread1.Sheets[0].Columns[9].Resizable = false;
                FpSpread1.Sheets[0].Columns[9].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = lblSec.Text;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);

                FpSpread1.Sheets[0].Columns[10].Width = 45;
                FpSpread1.Sheets[0].Columns[10].Locked = false;
                FpSpread1.Sheets[0].Columns[10].Resizable = false;
                FpSpread1.Sheets[0].Columns[10].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);

            }
            else
            {
                FpSpread1.Sheets[0].ColumnCount = 7;

                FpSpread1.Sheets[0].Columns[0].Width = 40;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                FpSpread1.Sheets[0].Columns[1].Width = 100;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Visible = isRollVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                FpSpread1.Sheets[0].Columns[2].Width = 120;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Visible = isRegVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

                FpSpread1.Sheets[0].Columns[3].Width = 120;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Visible = isAdmitNoVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

                FpSpread1.Sheets[0].Columns[4].Width = 150;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

                FpSpread1.Sheets[0].Columns[5].Width = 100;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = lblSem.Text;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

                FpSpread1.Sheets[0].Columns[6].Width = 60;
                FpSpread1.Sheets[0].Columns[6].Locked = false;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);

            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    /// <summary>
    /// Developed By Malang Raja on Dec 7 2016
    /// </summary>
    /// <param name="type">0 For Roll No,1 For Register No,2 For Admission No, 3 For Student Type</param>
    /// <param name="dsSettingsOptional">it is Optional Parameter</param>
    /// <returns>true or false</returns>
    private bool ColumnHeaderVisiblity(int type, DataSet dsSettingsOptional = null)
    {
        bool hasValues = false;
        try
        {
            DataSet dsSettings = new DataSet();
            if (dsSettingsOptional == null)
            {
                string grouporusercode = string.Empty;
                if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    string groupCode = Convert.ToString(Session["group_code"]).Trim();
                    string[] groupUser = Convert.ToString(groupCode).Trim().Split(';');
                    if (groupUser.Length > 0)
                    {
                        groupCode = groupUser[0].Trim();
                    }
                    if (!string.IsNullOrEmpty(groupCode.Trim()))
                    {
                        grouporusercode = " and  group_code=" + Convert.ToString(groupCode).Trim() + "";
                    }
                }
                else if (Session["usercode"] != null)
                {
                    grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                }
                if (!string.IsNullOrEmpty(grouporusercode))
                {
                    string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type') and value='1' " + grouporusercode + "";
                    dsSettings = da.select_method_wo_parameter(Master1, "Text");
                }
            }
            else
            {
                dsSettings = dsSettingsOptional;
            }
            if (dsSettings.Tables.Count > 0 && dsSettings.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drSettings in dsSettings.Tables[0].Rows)
                {
                    switch (type)
                    {
                        case 0:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "roll no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 1:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "register no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 2:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "admission no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 3:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "student_type")
                            {
                                hasValues = true;
                            }
                            break;
                    }
                    if (hasValues)
                        break;
                }
            }
            return hasValues;
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
            return false;
        }
    }

    #endregion

    #region Index Changed Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindBatch();
            BindDegree();
            BindBranch();
            BindSem();
            BindSectionDetail();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindDegree();
            BindBranch();
            BindSem();
            BindSectionDetail();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlDegree_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindBranch();
            BindSem();
            BindSectionDetail();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlBranch_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindSem();
            BindSectionDetail();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlSem_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindSectionDetail();
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlSec_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindSubjectType();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void chkSubjectType_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkSubjectType, cblSubjectType, txtSubjectType, lblSem.Text, "--Select--");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblSubjectType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkSubjectType, cblSubjectType, txtSubjectType, lblSem.Text, "--Select--");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlSubejctType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void FpStudentList_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            FpStudentList.SaveChanges();
            int r = FpStudentList.Sheets[0].ActiveRow;
            int j = FpStudentList.Sheets[0].ActiveColumn;
            if (r == 0 && j == 10)
            {
                int val = 0;
                int.TryParse(Convert.ToString(FpStudentList.Sheets[0].Cells[r, j].Value).Trim(), out val);
                for (int row = 1; row < FpStudentList.Sheets[0].RowCount; row++)
                {
                    if (val == 1)
                        FpStudentList.Sheets[0].Cells[row, j].Value = 1;
                    else
                        FpStudentList.Sheets[0].Cells[row, j].Value = 0;
                }
            }
        }
        catch
        {
        }
    }

    #endregion Index Changed Events

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

    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true)
    {
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                li.Selected = selected;
            }
        }
        catch
        {
        }
    }

    #endregion

    #region Button Events

    #region Button Go Click

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;

            collegeCode = string.Empty;
            degreeCode = string.Empty;
            batchYear = string.Empty;
            semester = string.Empty;
            section = string.Empty;
            fromDate = string.Empty;
            toDate = string.Empty;
            orderBy = string.Empty;
            orderBySetting = string.Empty;
            qry = string.Empty;
            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
            qrySection = string.Empty;
            qrySemester = string.Empty;

            DataSet dsODStudentDetails = new DataSet();
            DataSet dsDegreeDetails = new DataSet();
            if (ddlCollege.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblCollege.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                collegeCode = string.Empty;
                qryCollegeCode = string.Empty;
                Control c = ddlCollege;
                if (c is DropDownList)
                {
                    collegeCode = "'" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and r.college_code in(" + collegeCode + ")";
                }
            }
            if (ddlBatch.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblBatch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                batchYear = string.Empty;
                qryBatchYear = string.Empty;
                Control c = ddlBatch;
                if (c is DropDownList)
                {
                    batchYear = "'" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and r.Batch_year in(" + batchYear + ")";
                }
            }
            if (ddlDegree.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblDegree.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                courseId = string.Empty;
                qryCourseId = string.Empty;
                foreach (ListItem li in ddlDegree.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(courseId))
                        {
                            courseId = "'" + li.Value + "'";
                        }
                        else
                        {
                            courseId += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(courseId))
                {
                    qryCourseId = " and c.Course_Id in(" + courseId + ")";
                }
            }
            if (ddlBranch.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblBranch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                degreeCode = string.Empty;
                qryDegreeCode = string.Empty;
                Control c = ddlBranch;
                if (c is DropDownList)
                {
                    degreeCode = "'" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and r.degree_code in(" + degreeCode + ")";
                }
            }
            if (ddlSem.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblSem.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                semester = string.Empty;
                qrySemester = string.Empty;
                Control c = ddlSem;
                if (c is DropDownList)
                {
                    semester = "'" + Convert.ToString(ddlSem.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                if (!string.IsNullOrEmpty(semester))
                {
                    qrySemester = " and r.current_semester in(" + semester + ")";
                }
            }
            if (ddlSec.Items.Count > 0)
            {
                section = string.Empty;
                qrySection = string.Empty;
                Control c = ddlSec;
                if (c is DropDownList)
                {
                    section = "'" + Convert.ToString(ddlSec.SelectedValue).Trim() + "'"; ;
                }
                else
                {
                    foreach (ListItem li in ddlSec.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(section))
                            {
                                section = "'" + li.Value + "'";
                            }
                            else
                            {
                                section += ",'" + li.Value + "'";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(section))
                {
                    qrySection = " and LTRIM(RTRIM(ISNULL(r.sections,''))) in(" + section + ")";
                }
            }
            else
            {
                section = string.Empty;
                qrySection = string.Empty;
            }

            orderBySetting = da.GetFunction("select value from master_Settings where settings='order_by'");
            orderBySetting = orderBySetting.Trim();
            orderBy = "ORDER BY r.roll_no";
            switch (orderBySetting)
            {
                case "0":
                    orderBy = "ORDER BY r.roll_no";
                    break;
                case "1":
                    orderBy = "ORDER BY r.Reg_No";
                    break;
                case "2":
                    orderBy = "ORDER BY r.Stud_Name";
                    break;
                case "0,1,2":
                    orderBy = "ORDER BY r.roll_no,r.Reg_No,r.stud_name";
                    break;
                case "0,1":
                    orderBy = "ORDER BY r.roll_no,r.Reg_No";
                    break;
                case "1,2":
                    orderBy = "ORDER BY r.Reg_No,r.Stud_Name";
                    break;
                case "0,2":
                    orderBy = "ORDER BY r.roll_no,r.Stud_Name";
                    break;
                default:
                    orderBy = "ORDER BY r.roll_no";
                    break;
            }
            Farpoint.SheetView svsort = new FarPoint.Web.Spread.SheetView();
            Farpoint.CheckBoxCellType chkAll = new Farpoint.CheckBoxCellType();
            chkAll.AutoPostBack = true;
            Farpoint.CheckBoxCellType chkSingleCell = new Farpoint.CheckBoxCellType();
            if (!string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qrySemester))
            {
                qry = "select r.Roll_No,r.app_no,r.Reg_No,r.Roll_Admit,r.Stud_Type,case when r.mode=1 then 'REGULAR' when r.mode='2' then 'TRANSFER' when r.mode='3' then 'LATERAL' end as mode,r.Stud_Name,r.college_code,r.Batch_year,r.degree_code,r.current_semester,LTRIM(RTRIM(ISNULL(r.sections,''))) as sections  from Registration r,applyn a where a.app_no =r.App_No and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' " + qryCollegeCode + qryBatchYear + qrySemester + qryDegreeCode + qrySection + " " + orderBy;
                dsODStudentDetails.Clear();
                dsODStudentDetails = da.select_method_wo_parameter(qry, "text");

                qry = "select d.Degree_Code,(c.Course_Name ) as degreename,(dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id ;";
                dsDegreeDetails = da.select_method_wo_parameter(qry, "text");

                if (dsODStudentDetails.Tables.Count > 0 && dsODStudentDetails.Tables[0].Rows.Count > 0)
                {
                    Init_Spread(FpStudentList, 0);
                    FpStudentList.Sheets[0].RowCount = 1;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 10].CellType = chkAll;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 10].Locked = false;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 10].VerticalAlign = VerticalAlign.Middle;
                    FpStudentList.Sheets[0].SpanModel.Add(FpStudentList.Sheets[0].RowCount - 1, 0, 1, 10);
                    svsort = FpStudentList.ActiveSheetView;
                    svsort.AllowSort = true;

                    int serialNo = 0;
                    foreach (DataRow drODStudent in dsODStudentDetails.Tables[0].Rows)
                    {
                        serialNo++;
                        FpStudentList.Sheets[0].RowCount++;
                        string appNo = string.Empty;
                        string rollNo = string.Empty;
                        string regNo = string.Empty;
                        string admitNo = string.Empty;
                        string studentName = string.Empty;
                        string studentType = string.Empty;

                        string degreeName = string.Empty;
                        string departmentName = string.Empty;
                        string collegeCodeNew = string.Empty;
                        string batchYearNew = string.Empty;
                        string degreeCodeNew = string.Empty;
                        string currentSemester = string.Empty;
                        string sectionNew = string.Empty;
                        string modeofAdmission = string.Empty;

                        appNo = Convert.ToString(drODStudent["app_no"]).Trim();
                        rollNo = Convert.ToString(drODStudent["roll_no"]).Trim();
                        regNo = Convert.ToString(drODStudent["reg_no"]).Trim();
                        admitNo = Convert.ToString(drODStudent["Roll_Admit"]).Trim();
                        studentName = Convert.ToString(drODStudent["stud_name"]).Trim();
                        studentType = Convert.ToString(drODStudent["Stud_Type"]).Trim();
                        modeofAdmission = Convert.ToString(drODStudent["mode"]).Trim();
                        collegeCodeNew = Convert.ToString(drODStudent["college_code"]).Trim();
                        batchYearNew = Convert.ToString(drODStudent["Batch_Year"]).Trim();
                        degreeCodeNew = Convert.ToString(drODStudent["degree_code"]).Trim();
                        currentSemester = Convert.ToString(drODStudent["Current_Semester"]).Trim();
                        sectionNew = Convert.ToString(drODStudent["sections"]).Trim();

                        DataView dvDegreeName = new DataView();
                        DataView dvPeriodDetails = new DataView();
                        if (dsDegreeDetails.Tables.Count > 0 && dsDegreeDetails.Tables[0].Rows.Count > 0)
                        {
                            dsDegreeDetails.Tables[0].DefaultView.RowFilter = "Degree_code='" + degreeCodeNew + "'";
                            dvDegreeName = dsDegreeDetails.Tables[0].DefaultView;
                            if (dvDegreeName.Count > 0)
                            {
                                degreeName = Convert.ToString(dvDegreeName[0]["degreename"]);
                                departmentName = Convert.ToString(dvDegreeName[0]["dept_acronym"]);
                            }
                        }

                        Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialNo).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(appNo).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(degreeCodeNew).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 0].Locked = true;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(rollNo).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(collegeCodeNew).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(currentSemester).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 1].Locked = true;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(regNo).Trim();
                        //FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(collegeCode).Trim();
                        //FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(courseName).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(admitNo).Trim();
                        //FpStudentList.Sheets[3].Cells[FpStudentList.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(collegeCode).Trim();
                        //FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(courseName).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;


                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(studentType).Trim();
                        //FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(collegeCode).Trim();
                        //FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 4].Note = Convert.ToString(courseName).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 4].Locked = true;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(modeofAdmission).Trim();
                        //FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(collegeCode).Trim();
                        //FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 5].Note = Convert.ToString(courseName).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 5].CellType = txtCell;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 5].Locked = true;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(studentName).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 6].CellType = txtCell;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 6].Locked = true;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;

                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(degreeName).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 7].CellType = txtCell;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 7].Locked = true;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;

                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(departmentName).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 8].CellType = txtCell;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 8].Locked = true;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;

                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(section).Trim();
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 9].CellType = txtCell;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 9].Locked = true;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 9].VerticalAlign = VerticalAlign.Middle;

                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 10].Value = 0;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 10].CellType = chkSingleCell;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 10].Locked = false;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, 10].VerticalAlign = VerticalAlign.Middle;
                    }
                    divMainContents.Visible = true;
                    FpStudentList.Sheets[0].PageSize = FpStudentList.Sheets[0].RowCount;
                    FpStudentList.Width = 980;
                    FpStudentList.Height = 500;
                    FpStudentList.SaveChanges();
                    FpStudentList.Visible = true;
                }
                else
                {
                    divMainContents.Visible = false;
                    lblAlertMsg.Text = "No Record(s) Found";
                    divPopAlert.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion Button Go Click

    #region Popup Close

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13"))), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion  Popup Close

    #region Save Subject Allotment

    protected void btnSaveSubject_Click(object sender, EventArgs e)
    {
        try
        {
            FpStudentList.SaveChanges();
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            collegeCode = string.Empty;
            degreeCode = string.Empty;
            batchYear = string.Empty;
            semester = string.Empty;
            section = string.Empty;
            fromDate = string.Empty;
            toDate = string.Empty;
            orderBy = string.Empty;
            orderBySetting = string.Empty;
            qry = string.Empty;
            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
            qrySection = string.Empty;
            qrySemester = string.Empty;

            DataSet dsSubjectList = new DataSet();
            DataSet dsDegreeDetails = new DataSet();
            if (ddlCollege.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblCollege.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                collegeCode = string.Empty;
                qryCollegeCode = string.Empty;
                Control c = ddlCollege;
                if (c is DropDownList)
                {
                    collegeCode = "'" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and ct.TT_colCode in(" + collegeCode + ")";
                }
            }
            if (ddlBatch.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblBatch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                batchYear = string.Empty;
                qryBatchYear = string.Empty;
                Control c = ddlBatch;
                if (c is DropDownList)
                {
                    batchYear = "'" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and sm.Batch_year in(" + batchYear + ")";
                }
            }
            if (ddlDegree.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblDegree.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                courseId = string.Empty;
                qryCourseId = string.Empty;
                foreach (ListItem li in ddlDegree.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(courseId))
                        {
                            courseId = "'" + li.Value + "'";
                        }
                        else
                        {
                            courseId += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(courseId))
                {
                    qryCourseId = " and c.Course_Id in(" + courseId + ")";
                }
            }
            if (ddlBranch.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblBranch.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                degreeCode = string.Empty;
                qryDegreeCode = string.Empty;
                Control c = ddlBranch;
                if (c is DropDownList)
                {
                    degreeCode = "'" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and sm.degree_code in(" + degreeCode + ")";
                }
            }
            if (ddlSem.Items.Count == 0)
            {
                lblAlertMsg.Text = "No " + lblSem.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                semester = string.Empty;
                qrySemester = string.Empty;
                Control c = ddlSem;
                if (c is DropDownList)
                {
                    semester = "'" + Convert.ToString(ddlSem.SelectedValue).Trim() + "'"; ;
                }
                else
                {
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
                }
                if (!string.IsNullOrEmpty(semester))
                {
                    qrySemester = " and sm.semester in(" + semester + ")";
                }
            }
            if (ddlSec.Items.Count > 0)
            {
                section = string.Empty;
                qrySection = string.Empty;
                Control c = ddlSec;
                if (c is DropDownList)
                {
                    section = "'" + Convert.ToString(ddlSec.SelectedValue).Trim() + "'"; ;
                }
                else
                {
                    foreach (ListItem li in ddlSec.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(section))
                            {
                                section = "'" + li.Value + "'";
                            }
                            else
                            {
                                section += ",'" + li.Value + "'";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(section))
                {
                    qrySection = " and LTRIM(RTRIM(ISNULL(ct.TT_sec,''))) in(" + section + ")";
                }
            }
            else
            {
                section = string.Empty;
                qrySection = string.Empty;
            }
            if (cblSubjectType.Items.Count > 0 && cblSubjectType.Visible)
            {

            }
            else if (ddlSubejctType.Items.Count > 0 && ddlSubejctType.Visible)
            {
                subjectType = string.Empty;
                qrySubjectType = string.Empty;
                Control c = ddlBatch;
                if (c is DropDownList)
                {
                    subjectType = "'" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "'"; ;
                }
                else
                {
                    foreach (ListItem li in ddlBatch.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(subjectType))
                            {
                                subjectType = "'" + li.Text + "'";
                            }
                            else
                            {
                                subjectType += ",'" + li.Text + "'";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(subjectType))
                {
                    qrySubjectType = " and ss.subType_no in(" + subjectType + ")";
                }
            }
            else
            {
                lblAlertMsg.Text = "No " + lblSubjectType.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            bool isSaved = false;
            bool isSelectStudent = false;
            for (int rows = 0; rows < FpStudentList.Sheets[0].Rows.Count; rows++)
            {
                int selected = 0;
                int.TryParse(Convert.ToString(FpStudentList.Sheets[0].Cells[rows, 10].Value).Trim(), out selected);
                if (selected == 1)
                {
                    isSelectStudent = true;
                }
            }
            if (!isSelectStudent)
            {
                lblAlertMsg.Text = "Please Select Atleast One Student And Then Proceed";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                DataTable dtSubjectDetails = new DataTable();
                DataTable dtSubjectStaffDetails = new DataTable();
                if (!string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qrySemester))
                {
                    qry = "select distinct sm.semester,ss.subType_no,ss.subject_type,s.subject_name,s.subject_no,ISNULL(ss.Lab,'0') as Lab,sfs.staff_code from TT_ClassTimetable ct,TT_ClassTimetableDet ctd,subject s,Syllabus_Master sm,sub_sem ss,staff_selector sfs where s.subType_no=ss.subType_no and ss.syll_code=s.syll_code and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and s.subject_no=ctd.TT_subno and ct.TT_ClassPK=ctd.TT_ClassFK and sm.Batch_Year=ct.TT_batchyear and sm.degree_code=ct.TT_degCode and sm.semester=ct.TT_Sem and sfs.subject_no=s.subject_no and sfs.batch_year=sm.Batch_Year and sfs.batch_year=ct.TT_batchyear and sfs.staff_code=ctd.TT_staffcode and sfs.subject_no=ctd.TT_subno and ISNULL(ss.ElectivePap,'0')='0' and ct.TT_lastRec=1 " + qryCollegeCode + qryBatchYear + qrySubjectType + qryDegreeCode + qrySemester + qrySection + " order by Lab,ss.subType_no,s.subject_no";//,LTRIM(RTRIM(ISNULL(sfs.Sections,''))) as Sections
                    dtSubjectStaffDetails = dirAcc.selectDataTable(qry);

                    if (dtSubjectStaffDetails.Rows.Count > 0)
                    {
                        dtSubjectDetails = dtSubjectStaffDetails.DefaultView.ToTable(true, "subType_no", "subject_type", "subject_name", "subject_no", "Lab", "semester");

                        DataTable dtSubjectType = new DataTable();
                        dtSubjectType = dtSubjectStaffDetails.DefaultView.ToTable(true, "subType_no", "subject_type", "semester");
                        for (int row = 1; row < FpStudentList.Sheets[0].RowCount; row++)
                        {
                            string rollNo = Convert.ToString(FpStudentList.Sheets[0].Cells[row, 1].Text).Trim();
                            int selected = 0;
                            int.TryParse(Convert.ToString(FpStudentList.Sheets[0].Cells[row, 10].Value).Trim(), out selected);
                            if (selected == 1)
                            {
                                foreach (DataRow drSubjectType in dtSubjectType.Rows)
                                {
                                    string subjectTypeNo = Convert.ToString(drSubjectType["subType_no"]).Trim();
                                    string subjectTypetName = Convert.ToString(drSubjectType["subject_type"]).Trim();
                                    string subjectSem = Convert.ToString(drSubjectType["semester"]).Trim();
                                    Dictionary<string, string> dicSubTypeQ = new Dictionary<string, string>();
                                    dicSubTypeQ.Add("rollNo", rollNo);
                                    dicSubTypeQ.Add("semester", subjectSem.ToString());
                                    dicSubTypeQ.Add("subjectTypeNo", subjectTypeNo.ToString());
                                    int delST = storeAcc.deleteData("uspDeleteSubjectTypeSubjectChooser", dicSubTypeQ);

                                    dtSubjectDetails.DefaultView.RowFilter = "subType_no='" + subjectTypeNo + "' and semester='" + subjectSem + "'";
                                    DataTable dtSubjectSubType = new DataTable();
                                    dtSubjectSubType = dtSubjectDetails.DefaultView.ToTable();
                                    int paperOrder = 0;
                                    foreach (DataRow drSubject in dtSubjectSubType.Rows)
                                    {
                                        paperOrder++;
                                        string subjectNo = Convert.ToString(drSubject["subject_no"]).Trim();
                                        string subType = Convert.ToString(drSubject["subType_no"]).Trim();
                                        string SubSem = Convert.ToString(drSubject["semester"]).Trim();
                                        //@staffCode
                                        DataTable dtStaffCode = new DataTable();
                                        dtSubjectStaffDetails.DefaultView.RowFilter = "subject_no='" + subjectNo + "' and subType_no='" + subType + "' and semester='" + SubSem + "'";
                                        dtStaffCode = dtSubjectStaffDetails.DefaultView.ToTable(true, "staff_code");
                                        List<string> list = dtStaffCode.AsEnumerable().Select(r => r.Field<string>("staff_code")).ToList();
                                        string staffCode = string.Join(";", list.ToArray());

                                        Dictionary<string, string> dicSubjectQ = new Dictionary<string, string>();
                                        dicSubjectQ.Add("rollNo", rollNo);
                                        dicSubjectQ.Add("semester", SubSem.ToString());
                                        dicSubjectQ.Add("subjectNo", subjectNo.ToString());
                                        dicSubjectQ.Add("subjectTypeNo", subType.ToString());
                                        dicSubjectQ.Add("paperOrder", paperOrder.ToString());
                                        dicSubjectQ.Add("studbatch", "");
                                        dicSubjectQ.Add("grpCell", "0");
                                        dicSubjectQ.Add("staffCode", staffCode);

                                        int InsUSC = storeAcc.deleteData("uspInsUpSubjectChooser", dicSubjectQ);
                                        if (InsUSC > 0)
                                        {
                                            isSaved = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        lblAlertMsg.Text = "No Subject or Time Table Were Found";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
            }
            if (isSaved)
            {
                lblAlertMsg.Text = "Saved Successfully";
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                lblAlertMsg.Text = "No Saved";
                divPopAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

    #region Generate Excel

    protected void btnPrintExcel_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            printCommon.Visible = false;
            string reportname = txtExcelName.Text.Trim().Replace(" ", "_").Trim();
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (FpStudentList.Visible == true)
                {
                    da.printexcelreport(FpStudentList, reportname);
                }
                lblExcelError.Visible = false;
            }
            else
            {
                lblExcelError.Text = "Please Enter Your Report Name";
                lblExcelError.Visible = true;
                txtExcelName.Focus();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Generate Excel

    #region Print PDF

    protected void btnPrintPDF_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string rptheadname = string.Empty;
            rptheadname = "Over All GPA And CGPA Calculation Report";
            string pagename = "GPA_CGPA_CalculationProcess.aspx";
            //string Course_Name = Convert.ToString(ddldegree.SelectedItem).Trim();
            //rptheadname += "@ " + Course_Name + " - " + Convert.ToString(ddlbranch.SelectedItem).Trim() + "@ " + " Year of Admission : " + Convert.ToString(ddlbatch.SelectedItem).Trim() + "@ " + " Semester : " + Convert.ToString(ddlsem.SelectedItem).Trim();
            if (FpStudentList.Visible == true)
            {
                printCommon.loadspreaddetails(FpStudentList, pagename, rptheadname);
            }
            printCommon.Visible = true;
            lblExcelError.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Print PDF

    private DataTable GetStudentDetails(string collegeCode, string batchYear, string degreeCode, string semester, string section)
    {
        DataTable dtStudentDetails = new DataTable();

        string qrySec = string.Empty;
        if (!string.IsNullOrEmpty(section) && section.Trim().ToLower() != "all" && section.Trim().ToLower() != "-1" && section.Trim().ToLower() != "empty")
        {
            qrySec = "  and LTRIM(RTRIM(ISNULL(r.sections,''))) in(" + section + ")";
        }
        if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(semester))
        {
            string qry = "select r.Roll_No,r.Reg_No,r.Roll_Admit,r.Stud_Type,case when r.mode=1 then 'REGULAR' when r.mode='2' then 'TRANSFER' when r.mode='3' then 'LATERAL' end as mode,r.Stud_Name from Registration r,applyn a where a.app_no =r.App_No and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and r.college_code in(" + collegeCode + ") and r.Batch_Year in(" + batchYear + ") and r.degree_code in(" + degreeCode + ")  and r.Current_Semester in(" + semester + ")  " + qrySec + " " + orderByStudents();
            dtStudentDetails = dirAcc.selectDataTable(qry);
        }
        return dtStudentDetails;
    }

    private string orderByStudents()
    {
        string orderBySetting = da.GetFunction("select value from master_Settings where settings='order_by'");
        orderBySetting = orderBySetting.Trim();
        string orderBy = "ORDER BY r.roll_no";
        switch (orderBySetting)
        {
            case "0":
                orderBy = "ORDER BY r.roll_no";
                break;
            case "1":
                orderBy = "ORDER BY r.Reg_No";
                break;
            case "2":
                orderBy = "ORDER BY r.Stud_Name";
                break;
            case "0,1,2":
                orderBy = "ORDER BY r.roll_no,r.Reg_No,r.stud_name";
                break;
            case "0,1":
                orderBy = "ORDER BY r.roll_no,r.Reg_No";
                break;
            case "1,2":
                orderBy = "ORDER BY r.Reg_No,r.Stud_Name";
                break;
            case "0,2":
                orderBy = "ORDER BY r.roll_no,r.Stud_Name";
                break;
            default:
                orderBy = "ORDER BY r.roll_no";
                break;
        }
        return orderBy;
    }

    #endregion Button Events

}
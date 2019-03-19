using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using Farpoint = FarPoint.Web.Spread;
using wc = System.Web.UI.WebControls;

public partial class AttendanceMOD_StaffKeyListDetails : System.Web.UI.Page
{
    #region Field Declaration

    string staffCode = string.Empty;
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    string qry = string.Empty;
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string grouporusercode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string groupUserCode = string.Empty;
    DAccess2 da = new DAccess2();

    string collegeCodes = string.Empty;
    string batchYears = string.Empty;
    string courseId = string.Empty;
    string qryCourseId = string.Empty;
    string degreeCodes = string.Empty;
    string semesters = string.Empty;
    string sections = string.Empty;
    string subjectCodes = string.Empty;
    string subjectNames = string.Empty;
    string staffNames = string.Empty;
    string staffCodes = string.Empty;

    string qryCollegeCode = string.Empty;
    string qryBatchYear = string.Empty;
    string qrySemester = string.Empty;
    string qrySection = string.Empty;
    string qryDegreeCode = string.Empty;
    string qryStaffCode = string.Empty;
    string qryStaffName = string.Empty;
    string qrySubjectCode = string.Empty;
    int selected = 0;

    DataSet ds = new DataSet();

    Dictionary<string, string> dicParemeterQ = new Dictionary<string, string>();

    #endregion  Field Declaration

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
                collegeCode = Convert.ToString(Session["collegecode"]).Trim();
            userCode = (Session["Staff_Code"] == null) ? "" : Convert.ToString(Session["usercode"]).Trim();
            if (Session["group_code"] != null && (Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code='" + Session["group_code"].ToString().Trim() + "'";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " usercode='" + Session["usercode"].ToString().Trim() + "'";
            }
            if (Session["Staff_Code"] != null)
            {
                staffCode = Convert.ToString(Session["Staff_Code"]).Trim();
            }
            if (!IsPostBack)
            {
                divMainContents.Visible = false;
                lblAlertMsg.Text = string.Empty;
                divPopAlert.Visible = false;
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                BindCollege();
                BindBatch();
                BindDegree();
                BindBranch();
                BindSem();
                BindSectionDetail();
                BindStaffDetails();
                bindSubjectsList();
                divOther.Visible = false;
                divStaffHeader.Visible = false;
                if (IsAdminOrStaff())
                {
                    divOther.Visible = true;
                }
                else
                {
                    divStaffHeader.Visible = true;
                }
            }
        }
        catch
        {
        }
    }

    #endregion Page Load

    #region Bind Header

    public void BindCollege()
    {
        try
        {
            cblCollege.Items.Clear();
            chkCollege.Checked = false;
            txtCollege.Text = "--Select--";
            txtCollege.Enabled = false;
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
            dicParemeterQ.Clear();
            dicParemeterQ.Add("column_field", Convert.ToString(columnfield));
            DataSet dsprint = storeAcc.selectDataSet("bind_college", dicParemeterQ);
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                //ddlCollege.DataSource = dsprint;
                //ddlCollege.DataTextField = "collname";
                //ddlCollege.DataValueField = "college_code";
                //ddlCollege.DataBind();
                //ddlCollege.Visible = false;

                cblCollege.DataSource = dsprint;
                cblCollege.DataTextField = "collname";
                cblCollege.DataValueField = "college_code";
                cblCollege.DataBind();
                txtCollege.Enabled = true;
                checkBoxListselectOrDeselect(cblCollege, true, 1);
                CallCheckboxListChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");

            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindBatch()
    {
        try
        {
            cblBatch.Items.Clear();
            chkBatch.Checked = false;
            txtBatch.Text = "--Select--";
            txtBatch.Enabled = false;
            qryUserOrGroupCode = string.Empty;
            groupUserCode = string.Empty;
            collegeCode = string.Empty;
            userCode = string.Empty;
            //ddlBatch.Items.Clear();
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
            if (cblCollege.Items.Count > 0)
            {
                collegeCode = string.Empty;
                Control c = cblCollege;
                if (c is DropDownList)
                {
                    collegeCode = "'" + Convert.ToString(cblCollege.SelectedValue).Trim() + "'"; ;
                }
                else if (c is CheckBoxList)
                {
                    collegeCode = getCblSelectedValue(cblCollege);
                }
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                string strbinddegree = "select distinct batch_year from tbl_attendance_rights where college_code in(" + collegeCode + ") and batch_year<>'' and batch_year<>'0' and batch_year<>'-1'  " + qryUserOrGroupCode + " order by batch_year desc";
                ds = da.select_method_wo_parameter(strbinddegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //ddlBatch.DataSource = ds;
                //ddlBatch.DataTextField = "Batch_year";
                //ddlBatch.DataValueField = "Batch_year";
                //ddlBatch.DataBind();
                //ddlBatch.SelectedIndex = 0;

                cblBatch.DataSource = ds;
                cblBatch.DataTextField = "Batch_year";
                cblBatch.DataValueField = "Batch_year";
                cblBatch.DataBind();
                txtBatch.Enabled = true;
                checkBoxListselectOrDeselect(cblBatch, true, 1);
                CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindDegree()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            cblDegree.Items.Clear();
            chkDegree.Checked = false;
            txtDegree.Text = "--Select--";
            txtDegree.Enabled = false;
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            collegeCode = string.Empty;
            qryCollegeCode = string.Empty;
            batchYears = string.Empty;
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
            if (cblCollege.Items.Count > 0)
            {
                collegeCode = string.Empty;
                Control c = cblCollege;
                if (c is DropDownList)
                {
                    collegeCode = "'" + Convert.ToString(cblCollege.SelectedValue).Trim() + "'"; ;
                }
                else if (c is CheckBoxList)
                {
                    collegeCode = getCblSelectedValue(cblCollege);
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
                }
            }
            if (cblBatch.Items.Count > 0 && cblBatch.Visible == true)
            {
                batchYears = string.Empty;
                Control c = cblBatch;
                if (c is DropDownList)
                {
                    batchYears = "'" + Convert.ToString(cblBatch.SelectedValue).Trim() + "'"; ;
                }
                else if (c is CheckBoxList)
                {
                    batchYears = getCblSelectedValue(cblBatch);
                }
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatchYear = " and r.Batch_year in(" + batchYears + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                ds = da.select_method_wo_parameter("select distinct dg.course_id,c.course_name,c.Priority from Degree dg,Course c,Department dt,DeptPrivilages dp where  dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code  " + qryCollegeCode + qryUserOrGroupCode + "  order by c.Priority", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblDegree.DataSource = ds;
                    cblDegree.DataTextField = "course_name";
                    cblDegree.DataValueField = "course_id";
                    cblDegree.DataBind();
                    txtDegree.Enabled = true;
                    checkBoxListselectOrDeselect(cblDegree, true, 1);
                    CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindBranch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            cblBranch.Items.Clear();
            chkBranch.Checked = false;
            txtBranch.Text = "--Select--";
            txtBranch.Enabled = false;
            ds.Clear();
            ds.Dispose();
            ds.Reset();
            collegeCode = string.Empty;
            qryCollegeCode = string.Empty;
            batchYears = string.Empty;
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
            if (cblCollege.Items.Count > 0)
            {
                collegeCode = string.Empty;
                Control c = cblCollege;
                if (c is DropDownList)
                {
                    collegeCode = "'" + Convert.ToString(cblCollege.SelectedValue).Trim() + "'"; ;
                }
                else if (c is CheckBoxList)
                {
                    collegeCode = getCblSelectedValue(cblCollege);
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
                }
            }
            if (cblBatch.Items.Count > 0 && cblBatch.Visible == true)
            {
                batchYears = string.Empty;
                Control c = cblBatch;
                if (c is DropDownList)
                {
                    batchYears = "'" + Convert.ToString(cblBatch.SelectedValue).Trim() + "'"; ;
                }
                else if (c is CheckBoxList)
                {
                    batchYears = getCblSelectedValue(cblBatch);
                }
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatchYear = " and r.Batch_year in(" + batchYears + ")";
                }
            }
            if (cblDegree.Items.Count > 0)
            {
                courseId = string.Empty;
                Control c = cblDegree;
                if (c is DropDownList)
                {
                    courseId = "'" + Convert.ToString(cblDegree.SelectedValue).Trim() + "'"; ;
                }
                else if (c is CheckBoxList)
                {
                    courseId = getCblSelectedValue(cblDegree);
                }
                if (!string.IsNullOrEmpty(courseId))
                {
                    qryCourseId = " and c.Course_Id in(" + courseId + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryCourseId) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                ds = da.select_method_wo_parameter("select distinct dg.Degree_Code,dt.Dept_Name from Degree dg,Course c,Department dt,DeptPrivilages dp where  dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code  " + qryCollegeCode + qryCourseId + qryUserOrGroupCode + "order by dg.Degree_Code", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cblBranch.DataSource = ds;
                    cblBranch.DataTextField = "dept_name";
                    cblBranch.DataValueField = "degree_code";
                    cblBranch.DataBind();
                    txtBranch.Enabled = true;
                    checkBoxListselectOrDeselect(cblBranch, true, 1);
                    CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindSem()
    {
        try
        {
            ds.Clear();
            //ddlSem.Items.Clear();
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            cblSem.Items.Clear();
            chkSem.Checked = false;
            txtSem.Text = "--Select--";
            txtSem.Enabled = false;
            bool first_year = false;
            int duration = 0;
            int i = 0;
            selected = 0;
            qryCollegeCode = string.Empty;
            collegeCode = string.Empty;
            qryBatchYear = string.Empty;
            batchYears = string.Empty;
            courseId = string.Empty;
            qryCourseId = string.Empty;
            qryDegreeCode = string.Empty;
            degreeCodes = string.Empty;

            if (cblCollege.Items.Count > 0)
            {
                collegeCode = string.Empty;
                Control c = cblCollege;
                if (c is DropDownList)
                {
                    collegeCode = "'" + Convert.ToString(cblCollege.SelectedValue).Trim() + "'"; ;
                }
                else if (c is CheckBoxList)
                {
                    collegeCode = getCblSelectedValue(cblCollege);
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryCollegeCode = " and college_code in(" + collegeCode + ")";
                }
            }
            if (cblBatch.Items.Count > 0 && cblBatch.Visible == true)
            {
                batchYears = string.Empty;
                Control c = cblBatch;
                if (c is DropDownList)
                {
                    batchYears = "'" + Convert.ToString(cblBatch.SelectedValue).Trim() + "'"; ;
                }
                else if (c is CheckBoxList)
                {
                    batchYears = getCblSelectedValue(cblBatch);
                }
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatchYear = " and Batch_year in(" + batchYears + ")";
                }
            }
            if (cblBranch.Items.Count > 0)
            {
                degreeCodes = string.Empty;
                Control c = cblBranch;
                if (c is DropDownList)
                {
                    degreeCodes = "'" + Convert.ToString(cblBranch.SelectedValue).Trim() + "'"; ;
                }
                else if (c is CheckBoxList)
                {
                    degreeCodes = getCblSelectedValue(cblBranch);
                }
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and degree_code in(" + degreeCodes + ")";
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
                        cblSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                    else if (first_year == true && i != 2)
                    {
                        cblSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                }
                checkBoxListselectOrDeselect(cblSem, true);
                CallCheckboxListChange(chkSem, cblSem, txtSem, lblSem.Text, "--Select--");
                txtSem.Enabled = true;
                //ddlSem.SelectedIndex = 0;
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
                            cblSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                        else if (first_year == true && i != 2)
                        {
                            cblSem.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                    }
                    //ddlSem.SelectedIndex = 0;
                    checkBoxListselectOrDeselect(cblSem, true);
                    CallCheckboxListChange(chkSem, cblSem, txtSem, lblSem.Text, "--Select--");
                    txtSem.Enabled = true;
                }

            }
            if (cblSem.Items.Count > 0)
            {
                txtSem.Enabled = true;
            }
            else
            {
                txtSem.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
            batchYears = string.Empty;
            degreeCodes = string.Empty;
            ds = new DataSet();
            cblSec.Items.Clear();
            chkSec.Checked = false;
            txtSec.Text = "--Select--";
            txtSec.Enabled = false;
            if (cblCollege.Items.Count > 0)
            {
                collegeCode = string.Empty;
                Control c = cblCollege;
                if (c is DropDownList)
                {
                    collegeCode = "'" + Convert.ToString(cblCollege.SelectedValue).Trim() + "'"; ;
                }
                else if (c is CheckBoxList)
                {
                    collegeCode = getCblSelectedValue(cblCollege);
                }
            }
            if (cblBatch.Items.Count > 0 && cblBatch.Visible == true)
            {
                batchYears = string.Empty;
                Control c = cblBatch;
                if (c is DropDownList)
                {
                    batchYears = "'" + Convert.ToString(cblBatch.SelectedValue).Trim() + "'"; ;
                }
                else if (c is CheckBoxList)
                {
                    batchYears = getCblSelectedValue(cblBatch);
                }
            }
            if (cblBranch.Items.Count > 0)
            {
                degreeCodes = string.Empty;
                Control c = cblBranch;
                if (c is DropDownList)
                {
                    degreeCodes = "'" + Convert.ToString(cblBranch.SelectedValue).Trim() + "'"; ;
                }
                else if (c is CheckBoxList)
                {
                    degreeCodes = getCblSelectedValue(cblBranch);
                }
                //if (!string.IsNullOrEmpty(degreeCode))
                //{
                //    qryDegreeCode = " and degree_code in(" + degreeCode + ")";
                //}
            }
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
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
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYears) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                qrysections = da.GetFunctionv("select distinct sections from tbl_attendance_rights where college_code in(" + collegeCode + ") and batch_year in(" + batchYears + ")  " + qryUserOrGroupCode).Trim();
            }
            string sectionFilter = string.Empty;
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

                if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYears) && !string.IsNullOrEmpty(degreeCodes) && !string.IsNullOrEmpty(sections.Trim()))
                {
                    //string sqlnew = "select distinct LTRIM(RTRIM(ISNULL(sections,'')))  as sections from registration where batch_year in(" + Convert.ToString(batchYears).Trim() + ") and degree_code in(" + Convert.ToString(degreeCodes).Trim() + ") and LTRIM(RTRIM(ISNULL(sections,'')))<>'-1' and LTRIM(RTRIM(ISNULL(sections,'')))<>'' and college_code in(" + Convert.ToString(collegeCode).Trim() + ") and LTRIM(RTRIM(ISNULL(sections,''))) in(" + sections + ") and delflag='0' and cc='0' and exam_flag<>'Debar' order by sections";
                    //ds.Clear();
                    //ds = da.select_method_wo_parameter(sqlnew, "Text");
                    sectionFilter = " and LTRIM(RTRIM(ISNULL(sections,''))) in(" + sections + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(batchYears) && !string.IsNullOrEmpty(degreeCodes))
            {
                string sqlnew = "select distinct LTRIM(RTRIM(ISNULL(sections,'')))  as sections from registration where batch_year in(" + Convert.ToString(batchYears).Trim() + ") and degree_code in(" + Convert.ToString(degreeCodes).Trim() + ") and LTRIM(RTRIM(ISNULL(sections,'')))<>'-1' and LTRIM(RTRIM(ISNULL(sections,'')))<>'' and college_code in(" + Convert.ToString(collegeCode).Trim() + ") " + sectionFilter + " and delflag='0' and cc='0' and exam_flag<>'Debar' order by sections";
                ds.Clear();
                ds = da.select_method_wo_parameter(sqlnew, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblSec.DataSource = ds;
                cblSec.DataTextField = "sections";
                cblSec.DataValueField = "sections";
                cblSec.DataBind();
                checkBoxListselectOrDeselect(cblSec, true);
                CallCheckboxListChange(chkSec, cblSec, txtSec, lblSec.Text, "--Select--");
                txtSec.Enabled = true;
            }
            else
            {
                txtSec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    public void BindStaffDetails()
    {
        try
        {
            ddlStaff.Items.Clear();
            ddlStaff.Enabled = false;
            degreeCodes = string.Empty;
            collegeCode = string.Empty;
            if (cblCollege.Items.Count > 0)
            {
                collegeCode = string.Empty;
                Control c = cblCollege;
                if (c is DropDownList)
                {
                    collegeCode = "'" + Convert.ToString(cblCollege.SelectedValue).Trim() + "'"; ;
                }
                else if (c is CheckBoxList)
                {
                    collegeCode = getCblSelectedValue(cblCollege);
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    qryBatchYear = " and college_code in(" + collegeCode + ")";
                }
            }
            if (cblBatch.Items.Count > 0 && cblBatch.Visible == true)
            {
                batchYears = string.Empty;
                Control c = cblBatch;
                if (c is DropDownList)
                {
                    batchYears = "'" + Convert.ToString(cblBatch.SelectedValue).Trim() + "'"; ;
                }
                else if (c is CheckBoxList)
                {
                    batchYears = getCblSelectedValue(cblBatch);
                }
                if (!string.IsNullOrEmpty(batchYears))
                {
                    qryBatchYear = " and sm.Batch_year in(" + batchYears + ")";
                }
            }
            if (cblBranch.Items.Count > 0)
            {
                degreeCodes = string.Empty;
                Control c = cblBranch;
                if (c is DropDownList)
                {
                    degreeCodes = "'" + Convert.ToString(cblBranch.SelectedValue).Trim() + "'"; ;
                }
                else if (c is CheckBoxList)
                {
                    degreeCodes = getCblSelectedValue(cblBranch);
                }
                if (!string.IsNullOrEmpty(degreeCodes))
                {
                    qryDegreeCode = " and degree_code in(" + degreeCodes + ")";
                }
            }
            if (cblSem.Items.Count > 0)
            {
                semesters = string.Empty;
                Control c = cblSem;
                if (c is DropDownList)
                {
                    semesters = "'" + Convert.ToString(cblSem.SelectedValue).Trim() + "'"; ;
                }
                else if (c is CheckBoxList)
                {
                    semesters = getCblSelectedValue(cblSem);
                }
                if (!string.IsNullOrEmpty(semesters))
                {
                    qrySemester = " and sm.semester in(" + semesters + ")";
                }
            }
            if (cblSec.Items.Count > 0)
            {
                sections = string.Empty;
                Control c = cblSec;
                if (c is DropDownList)
                {
                    sections = "'" + Convert.ToString(cblSec.SelectedValue).Trim() + "'"; ;
                }
                else if (c is CheckBoxList)
                {
                    sections = getCblSelectedValue(cblSec);
                }
                if (!string.IsNullOrEmpty(sections))
                {
                    qrySection = " and ss.Sections in (" + sections + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(degreeCodes) && !string.IsNullOrEmpty(batchYears) && !string.IsNullOrEmpty(semesters))
            {
                //qry = "select distinct sm.staff_code,sm.staff_name,sm.staff_name+' ('+sm.staff_code+')' as StaffDisp from staffmaster sm,stafftrans st where st.staff_code=sm.staff_code and st.dept_code in(select dt.Dept_Code from Department dt,Degree dg,Course c where c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and dg.Degree_Code in(" + degreeCodes + ")) and ISNULL(st.latestrec,'1')='1' and sm.college_code in(" + collegeCode + ") order by sm.staff_name,sm.staff_code";and ss.Sections in('A')
                qry = "select distinct sfm.staff_code,sfm.staff_name,sfm.staff_name+' ('+sfm.staff_code+')' as StaffDisp from staffmaster sfm,stafftrans st,syllabus_master sm,staff_selector ss where st.staff_code=sfm.staff_code and st.dept_code in(select dt.Dept_Code from Department dt,Degree dg,Course c where c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and dg.Degree_Code in(" + degreeCodes + ")) and ss.staff_code=sfm.staff_code and ss.batch_year=sm.Batch_Year and ISNULL(st.latestrec,'1')='1' and sfm.college_code in(" + collegeCode + ") and ss.batch_year in(" + batchYears + ") and sm.degree_code in (" + degreeCodes + ") and sm.semester in(" + semesters + ") " + qrySection + " order by sfm.staff_name,sfm.staff_code";
                ds = da.select_method_wo_parameter(qry, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlStaff.DataSource = ds;
                ddlStaff.DataTextField = "StaffDisp";
                ddlStaff.DataValueField = "staff_code";
                ddlStaff.DataBind();
                ddlStaff.Enabled = true;
                ddlStaff.SelectedIndex = 0;
            }
            else
            {
                ddlStaff.Enabled = false;
            }
        }
        catch
        {
        }
    }

    private void bindSubjectsList()
    {
        try
        {
            bool staffSelector = false;
            cblStaffSubject.Items.Clear();
            chkStaffSubject.Checked = false;
            txtStaffSubject.Text = "--Select--";
            txtStaffSubject.Enabled = false;

            cblSubject.Items.Clear();
            chkSubject.Checked = false;
            txtSubject.Text = "--Select--";
            txtSubject.Enabled = false;

            string qryStudeStaffSelector = string.Empty;
            string minimumabsentsms = dirAcc.selectScalarString("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(cblCollege.SelectedValue).Trim() + "'");
            string[] splitminimumabsentsms = minimumabsentsms.Split('-');
            if (splitminimumabsentsms.Length == 2)
            {
                int batchyearsetting = 0;
                int.TryParse(Convert.ToString(splitminimumabsentsms[1]).Trim(), out batchyearsetting);
                if (Convert.ToString(splitminimumabsentsms[0]).Trim() == "1")
                {
                    staffSelector = true;
                }
            }
            DataTable dtSubject = new DataTable();

            bool isStaffUser = false;
            degreeCodes = string.Empty;
            collegeCode = string.Empty;
            semesters = string.Empty;
            sections = string.Empty;
            batchYears = string.Empty;
            if (IsAdminOrStaff())
            {
                staffCode = string.Empty;
                isStaffUser = false;
                if (ddlStaff.Items.Count > 0)
                {
                    staffCode = Convert.ToString(ddlStaff.SelectedValue).Trim();
                }

                if (cblCollege.Items.Count > 0)
                {
                    collegeCode = string.Empty;
                    Control c = cblCollege;
                    if (c is DropDownList)
                    {
                        collegeCode = "'" + Convert.ToString(cblCollege.SelectedValue).Trim() + "'"; ;
                    }
                    else if (c is CheckBoxList)
                    {
                        collegeCode = getCblSelectedValue(cblCollege);
                    }
                    if (!string.IsNullOrEmpty(collegeCode))
                    {
                        qryBatchYear = " and college_code in(" + collegeCode + ")";
                    }
                }
                if (cblBatch.Items.Count > 0 && cblBatch.Visible == true)
                {
                    batchYears = string.Empty;
                    Control c = cblBatch;
                    if (c is DropDownList)
                    {
                        batchYears = "'" + Convert.ToString(cblBatch.SelectedValue).Trim() + "'"; ;
                    }
                    else if (c is CheckBoxList)
                    {
                        batchYears = getCblSelectedValue(cblBatch);
                    }
                    if (!string.IsNullOrEmpty(batchYears))
                    {
                        qryBatchYear = " and sm.Batch_year in(" + batchYears + ")";
                    }
                }
                if (cblBranch.Items.Count > 0)
                {
                    degreeCodes = string.Empty;
                    Control c = cblBranch;
                    if (c is DropDownList)
                    {
                        degreeCodes = "'" + Convert.ToString(cblBranch.SelectedValue).Trim() + "'"; ;
                    }
                    else if (c is CheckBoxList)
                    {
                        degreeCodes = getCblSelectedValue(cblBranch);
                    }
                    if (!string.IsNullOrEmpty(degreeCodes))
                    {
                        qryDegreeCode = " and degree_code in(" + degreeCodes + ")";
                    }
                }
                if (cblSem.Items.Count > 0)
                {
                    semesters = string.Empty;
                    Control c = cblSem;
                    if (c is DropDownList)
                    {
                        semesters = "'" + Convert.ToString(cblSem.SelectedValue).Trim() + "'"; ;
                    }
                    else if (c is CheckBoxList)
                    {
                        semesters = getCblSelectedValue(cblSem);
                    }
                    if (!string.IsNullOrEmpty(semesters))
                    {
                        qrySemester = " and sm.semester in(" + semesters + ")";
                    }
                }
                if (cblSec.Items.Count > 0)
                {
                    sections = string.Empty;
                    Control c = cblSec;
                    if (c is DropDownList)
                    {
                        sections = "'" + Convert.ToString(cblSec.SelectedValue).Trim() + "'"; ;
                    }
                    else if (c is CheckBoxList)
                    {
                        sections = getCblSelectedValue(cblSec);
                    }
                    if (!string.IsNullOrEmpty(sections))
                    {
                        qrySection = " and ss.Sections in (" + sections + ")";
                    }
                }
            }
            else
            {
                isStaffUser = true;
                staffCode = Convert.ToString(Session["Staff_Code"]).Trim();
            }
            if (staffSelector)
            {
                qryStudeStaffSelector = " and sc.staffcode like '%" + staffCode + "%'";
            }
            if (!string.IsNullOrEmpty(staffCode))
            {
                if (isStaffUser)
                {
                    qry = "select distinct s.subject_name+' ['+subject_code+'] ' as subject_name ,s.subject_code from TT_ClassTimeTable T,Subject S,StaffMaster SM,TT_ClassTimeTabledet TT left join Room_detail R on TT_room=R.RoomPk Where T.TT_ClassPK=TT.TT_ClassFK and S.subject_no=TT_subno and SM.staff_code=TT_staffCode and sm.staff_code='" + staffCode + "' order by s.subject_code";
                    dtSubject = dirAcc.selectDataTable(qry);
                }
                else if (!string.IsNullOrEmpty(batchYears) && !string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(degreeCodes) && !string.IsNullOrEmpty(semesters) && !string.IsNullOrEmpty(staffCode))
                {
                    qry = "select distinct s.subject_name+' ['+subject_code+'] ' as subject_name ,s.subject_code from TT_ClassTimeTable T,Subject S,staffmaster sm,TT_ClassTimeTabledet TT left join Room_detail R on TT_room=R.RoomPk  Where T.TT_ClassPK=TT.TT_ClassFK and S.subject_no=TT_subno and SM.staff_code=TT_staffCode and TT_room=R.RoomPk and sm.staff_code='" + staffCode + "' and T.TT_batchyear in(" + batchYears + ") and T.TT_colCode in(" + collegeCode + ") and T.TT_degCode in (" + degreeCodes + ") and T.TT_sem in(" + semesters + ") and T.TT_sec in (" + sections + ") order by s.subject_code";
                    dtSubject = dirAcc.selectDataTable(qry);
                }
            }
            if (dtSubject.Rows.Count > 0)
            {
                //ddlSubject.DataSource = dtSubject;
                //ddlSubject.DataTextField = "subject_name";
                //ddlSubject.DataValueField = "subject_code";
                //ddlSubject.DataBind();
                //ddlSubject.Enabled = true;
                if (!isStaffUser)
                {
                    cblSubject.DataSource = dtSubject;
                    cblSubject.DataTextField = "subject_name";
                    cblSubject.DataValueField = "subject_code";
                    cblSubject.DataBind();
                    txtSubject.Enabled = true;
                    checkBoxListselectOrDeselect(cblSubject, true, 1);
                    CallCheckboxListChange(chkSubject, cblSubject, txtSubject, lblSubjectAD.Text, "--Select--");

                }
                else
                {
                    cblStaffSubject.DataSource = dtSubject;
                    cblStaffSubject.DataTextField = "subject_name";
                    cblStaffSubject.DataValueField = "subject_code";
                    cblStaffSubject.DataBind();
                    checkBoxListselectOrDeselect(cblStaffSubject, true, 1);
                    CallCheckboxListChange(chkStaffSubject, cblStaffSubject, txtStaffSubject, lblSubject.Text, "--Select--");
                    txtStaffSubject.Enabled = true;
                }
            }
            else
            {
                txtSubject.Enabled = false;
                txtStaffSubject.Enabled = false;
            }
        }
        catch
        {
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
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.ShowHeaderSelection = false;
            DataSet dsSettings = new DataSet();
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode = " and  group_code='" + Convert.ToString(Session["group_code"]).Trim() + "'";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " and usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(grouporusercode))
            {
                string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type','Application No') and value='1' " + grouporusercode + "";
                dsSettings = dirAcc.selectDataSet(Master1);
            }
            bool isRollVisible = ColumnHeaderVisiblity(0, dsSettings);
            bool isRegVisible = ColumnHeaderVisiblity(1, dsSettings);
            bool isAdmitNoVisible = ColumnHeaderVisiblity(2, dsSettings);
            bool isStudentTypeVisible = ColumnHeaderVisiblity(3, dsSettings);
            bool isApplicationNo = ColumnHeaderVisiblity(4, dsSettings);
            if (type == 0)
            {
                FpSpread1.Sheets[0].ColumnCount = 16;
                int count = 0;
                FpSpread1.Sheets[0].Columns[count].Width = 80;
                FpSpread1.Sheets[0].Columns[count].Locked = true;
                FpSpread1.Sheets[0].Columns[count].Resizable = false;
                FpSpread1.Sheets[0].Columns[count].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, count].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, count, 2, 1);

                count++;
                FpSpread1.Sheets[0].Columns[count].Width = 360;
                FpSpread1.Sheets[0].Columns[count].Locked = true;
                FpSpread1.Sheets[0].Columns[count].Resizable = false;
                FpSpread1.Sheets[0].Columns[count].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, count].Text = "Subject Code";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, count, 2, 1);

                count++;
                FpSpread1.Sheets[0].Columns[count].Width = 360;
                FpSpread1.Sheets[0].Columns[count].Locked = true;
                FpSpread1.Sheets[0].Columns[count].Resizable = false;
                FpSpread1.Sheets[0].Columns[count].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, count].Text = "Subject Name";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, count, 2, 1);

                count++;
                FpSpread1.Sheets[0].Columns[count].Width = 360;
                FpSpread1.Sheets[0].Columns[count].Locked = true;
                FpSpread1.Sheets[0].Columns[count].Resizable = false;
                FpSpread1.Sheets[0].Columns[count].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, count].Text = "Subject Details";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, count, 2, 1);
                //FpSpread1.Sheets[0].SetColumnMerge(count, Farpoint.Model.MergePolicy.Always);

                count++;
                FpSpread1.Sheets[0].Columns[count].Width = 90;
                FpSpread1.Sheets[0].Columns[count].Locked = true;
                FpSpread1.Sheets[0].Columns[count].Resizable = false;
                FpSpread1.Sheets[0].Columns[count].Visible = isApplicationNo;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, count].Text = "Application No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, count, 2, 1);

                count++;
                FpSpread1.Sheets[0].Columns[count].Width = 100;
                FpSpread1.Sheets[0].Columns[count].Locked = true;
                FpSpread1.Sheets[0].Columns[count].Resizable = false;
                FpSpread1.Sheets[0].Columns[count].Visible = isRollVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, count].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, count, 2, 1);

                count++;
                FpSpread1.Sheets[0].Columns[count].Width = 100;
                FpSpread1.Sheets[0].Columns[count].Locked = true;
                FpSpread1.Sheets[0].Columns[count].Resizable = false;
                FpSpread1.Sheets[0].Columns[count].Visible = isRegVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, count].Text = "Reg No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, count, 2, 1);

                count++;
                FpSpread1.Sheets[0].Columns[count].Width = 100;
                FpSpread1.Sheets[0].Columns[count].Locked = true;
                FpSpread1.Sheets[0].Columns[count].Resizable = false;
                FpSpread1.Sheets[0].Columns[count].Visible = isAdmitNoVisible;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, count].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, count, 2, 1);

                count++;
                FpSpread1.Sheets[0].Columns[count].Width = 70;
                FpSpread1.Sheets[0].Columns[count].Locked = true;
                FpSpread1.Sheets[0].Columns[count].Resizable = false;
                FpSpread1.Sheets[0].Columns[count].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, count].Text = "Resident";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, count, 2, 1);

                count++;
                FpSpread1.Sheets[0].Columns[count].Width = 230;
                FpSpread1.Sheets[0].Columns[count].Locked = true;
                FpSpread1.Sheets[0].Columns[count].Resizable = false;
                FpSpread1.Sheets[0].Columns[count].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, count].Text = "Student Name";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, count, 2, 1);
                FpSpread1.Sheets[0].SetColumnMerge(count, Farpoint.Model.MergePolicy.Always);

                count++;
                FpSpread1.Sheets[0].Columns[count].Width = 90;
                FpSpread1.Sheets[0].Columns[count].Locked = true;
                FpSpread1.Sheets[0].Columns[count].Resizable = false;
                FpSpread1.Sheets[0].Columns[count].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, count].Text = "Batch Year";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, count, 2, 1);
                FpSpread1.Sheets[0].SetColumnMerge(count, Farpoint.Model.MergePolicy.Always);

                count++;
                FpSpread1.Sheets[0].Columns[count].Width = 180;
                FpSpread1.Sheets[0].Columns[count].Locked = true;
                FpSpread1.Sheets[0].Columns[count].Resizable = false;
                FpSpread1.Sheets[0].Columns[count].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, count].Text = "Degree Details";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, count, 2, 1);
                FpSpread1.Sheets[0].SetColumnMerge(count, Farpoint.Model.MergePolicy.Always);

                count++;
                FpSpread1.Sheets[0].Columns[count].Width = 80;
                FpSpread1.Sheets[0].Columns[count].Locked = true;
                FpSpread1.Sheets[0].Columns[count].Resizable = false;
                FpSpread1.Sheets[0].Columns[count].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, count].Text = "Degree Code";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(count, 3, 2, 1);

                count++;
                FpSpread1.Sheets[0].Columns[count].Width = 70;
                FpSpread1.Sheets[0].Columns[count].Locked = true;
                FpSpread1.Sheets[0].Columns[count].Resizable = false;
                FpSpread1.Sheets[0].Columns[count].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, count].Text = "Semester";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, count, 2, 1);

                count++;
                FpSpread1.Sheets[0].Columns[count].Width = 50;
                FpSpread1.Sheets[0].Columns[count].Locked = true;
                FpSpread1.Sheets[0].Columns[count].Resizable = false;
                FpSpread1.Sheets[0].Columns[count].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, count].Text = "Section";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, count, 2, 1);



                count++;
                FpSpread1.Sheets[0].Columns[count].Width = 60;
                FpSpread1.Sheets[0].Columns[count].Locked = true;
                FpSpread1.Sheets[0].Columns[count].Resizable = false;
                FpSpread1.Sheets[0].Columns[count].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, count].Text = "Room Name";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, count, 2, 1);

            }
        }
        catch (Exception ex)
        {

        }
    }

    #endregion Bind Header

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
            BindStaffDetails();
            bindSubjectsList();
        }
        catch
        {
        }
    }

    protected void chkCollege_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");
            BindBatch();
            BindDegree();
            BindBranch();
            BindSem();
            BindSectionDetail();
            BindStaffDetails();
            bindSubjectsList();
        }
        catch
        {
        }
    }

    protected void cblCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");
            BindBatch();
            BindDegree();
            BindBranch();
            BindSem();
            BindSectionDetail();
            BindStaffDetails();
            bindSubjectsList();
        }
        catch
        {
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
            BindStaffDetails();
            bindSubjectsList();
        }
        catch
        {
        }
    }

    protected void chkBatch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
            BindDegree();
            BindBranch();
            BindSem();
            BindSectionDetail();
            BindStaffDetails();
            bindSubjectsList();
        }
        catch
        {
        }
    }

    protected void cblBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkBatch, cblBatch, txtBatch, lblBatch.Text, "--Select--");
            BindDegree();
            BindBranch();
            BindSem();
            BindSectionDetail();
            BindStaffDetails();
            bindSubjectsList();
        }
        catch
        {
        }
    }

    protected void chkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            BindBranch();
            BindSem();
            BindSectionDetail();
            BindStaffDetails();
            bindSubjectsList();
        }
        catch
        {
        }
    }

    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkDegree, cblDegree, txtDegree, lblDegree.Text, "--Select--");
            BindBranch();
            BindSem();
            BindSectionDetail();
            BindStaffDetails();
            bindSubjectsList();
        }
        catch
        {
        }
    }

    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            BindSem();
            BindSectionDetail();
            BindStaffDetails();
            bindSubjectsList();
        }
        catch
        {
        }
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkBranch, cblBranch, txtBranch, lblBranch.Text, "--Select--");
            BindSem();
            BindSectionDetail();
            BindStaffDetails();
            bindSubjectsList();
        }
        catch
        {
        }
    }

    protected void ddlSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindSectionDetail();
            BindStaffDetails();
            bindSubjectsList();
        }
        catch
        {
        }
    }

    protected void chkSem_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkSem, cblSem, txtSem, lblSem.Text, "--Select--");
            BindSectionDetail();
            BindStaffDetails();
            bindSubjectsList();
        }
        catch
        {
        }
    }

    protected void cblSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkSem, cblSem, txtSem, lblSem.Text, "--Select--");
            BindSectionDetail();
            BindStaffDetails();
            bindSubjectsList();
        }
        catch
        {
        }
    }

    protected void chkSec_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkSec, cblSec, txtSec, lblSec.Text, "--Select--");
            BindStaffDetails();
            bindSubjectsList();
        }
        catch
        {
        }
    }

    protected void cblSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkSec, cblSec, txtSec, lblSec.Text, "--Select--");
            BindStaffDetails();
            bindSubjectsList();
        }
        catch
        {
        }
    }

    protected void ddlStaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            bindSubjectsList();
        }
        catch
        {
        }
    }

    protected void chkSubject_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkSubject, cblSubject, txtSubject, lblSubjectAD.Text, "--Select--");
        }
        catch
        {
        }
    }

    protected void cblSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkSubject, cblSubject, txtSubject, lblSubjectAD.Text, "--Select--");
        }
        catch
        {
        }
    }

    protected void chkStaffSubject_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxChange(chkStaffSubject, cblStaffSubject, txtStaffSubject, lblSubject.Text, "--Select--");
        }
        catch
        {
        }
    }

    protected void cblStaffSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            CallCheckboxListChange(chkStaffSubject, cblStaffSubject, txtStaffSubject, lblSubject.Text, "--Select--");
        }
        catch
        {
        }
    }

    protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
        }
        catch
        {
        }
    }

    #endregion Index Changed Events

    #region GO Click

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            divMainContents.Visible = false;
            bool staffSelector = false;
            string subjectCode = string.Empty;

            //if (IsAdminOrStaff())
            //{
            //    lblAlertMsg.Text = "Please Choose Staff User";
            //    divPopAlert.Visible = true;
            //    return;
            //}
            string qryStudeStaffSelector = string.Empty;
            string minimumabsentsms = dirAcc.selectScalarString("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(cblCollege.SelectedValue).Trim() + "'");
            string[] splitminimumabsentsms = minimumabsentsms.Split('-');
            if (splitminimumabsentsms.Length == 2)
            {
                int batchyearsetting = 0;
                int.TryParse(Convert.ToString(splitminimumabsentsms[1]).Trim(), out batchyearsetting);
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    staffSelector = true;
                }
            }
            DataTable dtSubject = new DataTable();
            bool isStaffUser = false;
            if (IsAdminOrStaff())
            {
                string[] college = new string[0];
                string[] batch = new string[0];
                string[] degree = new string[0];
                string[] semester = new string[0];
                string[] section = new string[0];
                staffCode = string.Empty;
                isStaffUser = false;
                if (cblCollege.Items.Count > 0)
                {
                    collegeCode = string.Empty;
                    Control c = cblCollege;
                    if (c is DropDownList)
                    {
                        collegeCode = "'" + Convert.ToString(cblCollege.SelectedValue).Trim() + "'";
                        Array.Resize(ref college, college.Length + 1);
                        college[college.Length - 1] = Convert.ToString(cblCollege.SelectedValue).Trim();
                    }
                    else if (c is CheckBoxList)
                    {
                        collegeCode = getCblSelectedValue(cblCollege, out college);
                    }
                    if (!string.IsNullOrEmpty(collegeCode))
                    {
                        qryCollegeCode = " and college_code in(" + collegeCode + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select Atleast One " + lblCollege.Text + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblAlertMsg.Text = "No " + lblCollege.Text + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                if (cblBatch.Items.Count > 0 && cblBatch.Visible == true)
                {
                    batchYears = string.Empty;
                    Control c = cblBatch;
                    if (c is DropDownList)
                    {
                        batchYears = "'" + Convert.ToString(cblBatch.SelectedValue).Trim() + "'";
                        Array.Resize(ref batch, batch.Length + 1);
                        batch[batch.Length - 1] = Convert.ToString(cblBatch.SelectedValue).Trim();
                    }
                    else if (c is CheckBoxList)
                    {
                        batchYears = getCblSelectedValue(cblBatch, out batch);
                    }
                    if (!string.IsNullOrEmpty(batchYears))
                    {
                        qryBatchYear = " and sm.Batch_year in(" + batchYears + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select Atleast One " + lblBatch.Text + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblAlertMsg.Text = "No " + lblBatch.Text + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                if (cblBranch.Items.Count > 0)
                {
                    degreeCodes = string.Empty;
                    Control c = cblBranch;
                    if (c is DropDownList)
                    {
                        degreeCodes = "'" + Convert.ToString(cblBranch.SelectedValue).Trim() + "'";
                        Array.Resize(ref degree, batch.Length + 1);
                        degree[degree.Length - 1] = Convert.ToString(cblBranch.SelectedValue).Trim();
                    }
                    else if (c is CheckBoxList)
                    {
                        degreeCodes = getCblSelectedValue(cblBranch, out degree);
                    }
                    if (!string.IsNullOrEmpty(degreeCodes))
                    {
                        qryDegreeCode = " and degree_code in(" + degreeCodes + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select Atleast One " + lblBranch.Text + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblAlertMsg.Text = "No " + lblBranch.Text + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                if (cblSem.Items.Count > 0)
                {
                    semesters = string.Empty;
                    Control c = cblSem;
                    if (c is DropDownList)
                    {
                        semesters = "'" + Convert.ToString(cblSem.SelectedValue).Trim() + "'";
                        Array.Resize(ref semester, semester.Length + 1);
                        semester[semester.Length - 1] = Convert.ToString(cblSem.SelectedValue).Trim();
                    }
                    else if (c is CheckBoxList)
                    {
                        semesters = getCblSelectedValue(cblSem, out semester);
                    }
                    if (!string.IsNullOrEmpty(semesters))
                    {
                        qrySemester = " and sm.semester in(" + semesters + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select Atleast One " + lblSem.Text + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblAlertMsg.Text = "No " + lblSem.Text + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                if (cblSec.Items.Count > 0)
                {
                    sections = string.Empty;
                    Control c = cblSec;
                    if (c is DropDownList)
                    {
                        sections = "'" + Convert.ToString(cblSec.SelectedValue).Trim() + "'";
                        Array.Resize(ref section, section.Length + 1);
                        section[section.Length - 1] = Convert.ToString(cblSec.SelectedValue).Trim();
                    }
                    else if (c is CheckBoxList)
                    {
                        sections = getCblSelectedValue(cblSec, out section);
                    }
                    if (!string.IsNullOrEmpty(sections))
                    {
                        qrySection = " and ss.Sections in (" + sections + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select Atleast One " + lblSec.Text + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                if (ddlStaff.Items.Count > 0)
                {
                    staffCode = Convert.ToString(ddlStaff.SelectedValue).Trim();
                }
                else
                {
                    lblAlertMsg.Text = "No " + lblStaff.Text + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                if (cblSubject.Items.Count == 0)
                {
                    lblAlertMsg.Text = "No " + lblSubjectAD + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                else
                {
                    subjectCode = getCblSelectedValue(cblSubject);
                    if (string.IsNullOrEmpty(subjectCode))
                    {
                        lblAlertMsg.Text = "Please Select Atleast One " + lblSubjectAD.Text + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
            }
            else
            {
                if (cblStaffSubject.Items.Count == 0)
                {
                    lblAlertMsg.Text = "No " + lblSubject.Text + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                else
                {
                    subjectCode = getCblSelectedValue(cblStaffSubject);
                }
                isStaffUser = true;
                staffCode = Convert.ToString(Session["Staff_Code"]).Trim();
                lblStaffNameDisp.Text = string.Empty;
                if (string.IsNullOrEmpty(staffCode))
                {
                    lblAlertMsg.Text = "No Staff Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                else
                {
                    lblStaffNameDisp.Text = staffCode + " - " + dirAcc.selectScalarString("select sm.staff_name from staffmaster sm where sm.staff_code='" + staffCode + "'");
                }
            }
            if (staffSelector)
            {
                qryStudeStaffSelector = " and sc.staffcode like '%" + staffCode + "%'";
            }
            DataTable dtStudentList = new DataTable();
            if (!string.IsNullOrEmpty(staffCode) && !string.IsNullOrEmpty(subjectCode))
            {
                dtStudentList = getStudentData(staffCode, subjectCode);
            }
            if (dtStudentList.Rows.Count > 0)
            {
                Init_Spread(FpStudentList);
                int serialNo = 0;
                foreach (DataRow drStudent in dtStudentList.Rows)
                {
                    serialNo++;
                    string appNo = Convert.ToString(drStudent["app_no"]).Trim();
                    string studName = Convert.ToString(drStudent["stud_name"]).Trim();
                    string studType = Convert.ToString(drStudent["Resident"]).Trim();
                    string rollNo = Convert.ToString(drStudent["Roll_No"]).Trim();
                    string regNo = Convert.ToString(drStudent["Reg_No"]).Trim();
                    string batchYear = Convert.ToString(drStudent["Batch_Year"]).Trim();
                    string degreeCode = Convert.ToString(drStudent["degree_code"]).Trim();
                    string degreeDetail = Convert.ToString(drStudent["DegreeDetails"]).Trim();
                    string section = Convert.ToString(drStudent["Section"]).Trim();
                    string semester = Convert.ToString(drStudent["Current_Semester"]).Trim();
                    string roomNo = Convert.ToString(drStudent["RoomNo"]).Trim();
                    string admitNo = Convert.ToString(drStudent["Roll_Admit"]).Trim();
                    string applicationNo = Convert.ToString(drStudent["app_formno"]).Trim();
                    string subjectCodeValue = Convert.ToString(drStudent["subject_code"]).Trim();
                    string subjectNameValue = Convert.ToString(drStudent["subject_name"]).Trim();
                    string subjectDetailsValue = Convert.ToString(drStudent["SubjectDetails"]).Trim();
                    int column = 0;
                    Farpoint.TextCellType txtCell = new Farpoint.TextCellType();

                    FpStudentList.Sheets[0].RowCount++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(serialNo).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Tag = Convert.ToString(appNo).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Note = Convert.ToString(degreeCode).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(subjectCodeValue).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Tag = Convert.ToString(subjectCodeValue).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Note = Convert.ToString(subjectNameValue).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(subjectNameValue).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Tag = Convert.ToString(subjectCodeValue).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Note = Convert.ToString(subjectNameValue).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(subjectDetailsValue).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Tag = Convert.ToString(subjectCodeValue).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Note = Convert.ToString(subjectNameValue).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(applicationNo).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(rollNo).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(regNo).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(admitNo).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(studType).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(studName).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(batchYear).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(degreeDetail).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(degreeCode).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(semester).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(section).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(roomNo).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

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
                lblAlertMsg.Text = "No Record(s) Were Found";
                divPopAlert.Visible = true;
                return;
            }
        }
        catch
        {
        }
    }

    protected void btnAdminGo_Click(object sender, EventArgs e)
    {
        try
        {
            divMainContents.Visible = false;
            bool staffSelector = false;
            string subjectCode = string.Empty;
            string qryStudeStaffSelector = string.Empty;
            string minimumabsentsms = dirAcc.selectScalarString("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Convert.ToString(cblCollege.SelectedValue).Trim() + "'");
            string[] splitminimumabsentsms = minimumabsentsms.Split('-');
            if (splitminimumabsentsms.Length == 2)
            {
                int batchyearsetting = 0;
                int.TryParse(Convert.ToString(splitminimumabsentsms[1]).Trim(), out batchyearsetting);
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    staffSelector = true;
                }
            }
            DataTable dtSubject = new DataTable();
            bool isStaffUser = false;
            if (IsAdminOrStaff())
            {
                string[] college = new string[0];
                string[] batch = new string[0];
                string[] degree = new string[0];
                string[] semester = new string[0];
                string[] section = new string[0];
                staffCode = string.Empty;
                isStaffUser = false;
                if (cblCollege.Items.Count > 0)
                {
                    collegeCode = string.Empty;
                    Control c = cblCollege;
                    if (c is DropDownList)
                    {
                        collegeCode = "'" + Convert.ToString(cblCollege.SelectedValue).Trim() + "'";
                        Array.Resize(ref college, college.Length + 1);
                        college[college.Length - 1] = Convert.ToString(cblCollege.SelectedValue).Trim();
                    }
                    else if (c is CheckBoxList)
                    {
                        collegeCode = getCblSelectedValue(cblCollege, out college);
                    }
                    if (!string.IsNullOrEmpty(collegeCode))
                    {
                        qryCollegeCode = " and college_code in(" + collegeCode + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select Atleast One " + lblCollege.Text + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblAlertMsg.Text = "No " + lblCollege.Text + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                if (cblBatch.Items.Count > 0 && cblBatch.Visible == true)
                {
                    batchYears = string.Empty;
                    Control c = cblBatch;
                    if (c is DropDownList)
                    {
                        batchYears = "'" + Convert.ToString(cblBatch.SelectedValue).Trim() + "'";
                        Array.Resize(ref batch, batch.Length + 1);
                        batch[batch.Length - 1] = Convert.ToString(cblBatch.SelectedValue).Trim();
                    }
                    else if (c is CheckBoxList)
                    {
                        batchYears = getCblSelectedValue(cblBatch, out batch);
                    }
                    if (!string.IsNullOrEmpty(batchYears))
                    {
                        qryBatchYear = " and sm.Batch_year in(" + batchYears + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select Atleast One " + lblBatch.Text + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblAlertMsg.Text = "No " + lblBatch.Text + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                if (cblBranch.Items.Count > 0)
                {
                    degreeCodes = string.Empty;
                    Control c = cblBranch;
                    if (c is DropDownList)
                    {
                        degreeCodes = "'" + Convert.ToString(cblBranch.SelectedValue).Trim() + "'";
                        Array.Resize(ref degree, batch.Length + 1);
                        degree[degree.Length - 1] = Convert.ToString(cblBranch.SelectedValue).Trim();
                    }
                    else if (c is CheckBoxList)
                    {
                        degreeCodes = getCblSelectedValue(cblBranch, out degree);
                    }
                    if (!string.IsNullOrEmpty(degreeCodes))
                    {
                        qryDegreeCode = " and degree_code in(" + degreeCodes + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select Atleast One " + lblBranch.Text + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblAlertMsg.Text = "No " + lblBranch.Text + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                if (cblSem.Items.Count > 0)
                {
                    semesters = string.Empty;
                    Control c = cblSem;
                    if (c is DropDownList)
                    {
                        semesters = "'" + Convert.ToString(cblSem.SelectedValue).Trim() + "'";
                        Array.Resize(ref semester, semester.Length + 1);
                        semester[semester.Length - 1] = Convert.ToString(cblSem.SelectedValue).Trim();
                    }
                    else if (c is CheckBoxList)
                    {
                        semesters = getCblSelectedValue(cblSem, out semester);
                    }
                    if (!string.IsNullOrEmpty(semesters))
                    {
                        qrySemester = " and sm.semester in(" + semesters + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select Atleast One " + lblSem.Text + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblAlertMsg.Text = "No " + lblSem.Text + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                if (cblSec.Items.Count > 0)
                {
                    sections = string.Empty;
                    Control c = cblSec;
                    if (c is DropDownList)
                    {
                        sections = "'" + Convert.ToString(cblSec.SelectedValue).Trim() + "'";
                        Array.Resize(ref section, section.Length + 1);
                        section[section.Length - 1] = Convert.ToString(cblSec.SelectedValue).Trim();
                    }
                    else if (c is CheckBoxList)
                    {
                        sections = getCblSelectedValue(cblSec, out section);
                    }
                    if (!string.IsNullOrEmpty(sections))
                    {
                        qrySection = " and ss.Sections in (" + sections + ")";
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Select Atleast One " + lblSec.Text + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
                if (ddlStaff.Items.Count > 0)
                {
                    staffCode = Convert.ToString(ddlStaff.SelectedValue).Trim();
                    lblStaffNameDisp.Text = staffCode + " - " + dirAcc.selectScalarString("select sm.staff_name from staffmaster sm where sm.staff_code='" + staffCode + "'");
                }
                else
                {
                    lblAlertMsg.Text = "No " + lblStaff.Text + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                if (cblSubject.Items.Count == 0)
                {
                    lblAlertMsg.Text = "No " + lblSubjectAD + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                else
                {
                    subjectCode = getCblSelectedValue(cblSubject);
                    if (string.IsNullOrEmpty(subjectCode))
                    {
                        lblAlertMsg.Text = "Please Select Atleast One " + lblSubjectAD.Text + " And Then Proceed";
                        divPopAlert.Visible = true;
                        return;
                    }
                }
            }
            else
            {
                if (cblStaffSubject.Items.Count == 0)
                {
                    lblAlertMsg.Text = "No " + lblSubject.Text + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                else
                {
                    subjectCode = getCblSelectedValue(cblStaffSubject);
                }
                isStaffUser = true;
                staffCode = Convert.ToString(Session["Staff_Code"]).Trim();
                lblStaffNameDisp.Text = string.Empty;
                if (string.IsNullOrEmpty(staffCode))
                {
                    lblAlertMsg.Text = "No Staff Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                else
                {
                    lblStaffNameDisp.Text = staffCode + " - " + dirAcc.selectScalarString("select sm.staff_name from staffmaster sm where sm.staff_code='" + staffCode + "'");
                }
            }
            if (staffSelector)
            {
                qryStudeStaffSelector = " and sc.staffcode like '%" + staffCode + "%'";
            }
            DataTable dtStudentList = new DataTable();
            if (!string.IsNullOrEmpty(staffCode) && !string.IsNullOrEmpty(subjectCode))
            {
                if (isStaffUser)
                    dtStudentList = getStudentData(staffCode, subjectCode);
                else
                {
                    if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(degreeCodes) && !string.IsNullOrEmpty(batchYears) && !string.IsNullOrEmpty(semesters))
                    {
                        dtStudentList = getStudentData(staffCode, subjectCode);
                    }
                }
            }
            if (dtStudentList.Rows.Count > 0)
            {
                Init_Spread(FpStudentList);
                int serialNo = 0;
                foreach (DataRow drStudent in dtStudentList.Rows)
                {
                    serialNo++;
                    Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
                    string appNo = Convert.ToString(drStudent["app_no"]).Trim();
                    string studName = Convert.ToString(drStudent["stud_name"]).Trim();
                    string studType = Convert.ToString(drStudent["Resident"]).Trim();
                    string rollNo = Convert.ToString(drStudent["Roll_No"]).Trim();
                    string regNo = Convert.ToString(drStudent["Reg_No"]).Trim();
                    string batchYear = Convert.ToString(drStudent["Batch_Year"]).Trim();
                    string degreeCode = Convert.ToString(drStudent["degree_code"]).Trim();
                    string degreeDetail = Convert.ToString(drStudent["DegreeDetails"]).Trim();
                    string section = Convert.ToString(drStudent["Section"]).Trim();
                    string semester = Convert.ToString(drStudent["Current_Semester"]).Trim();
                    string roomNo = Convert.ToString(drStudent["RoomNo"]).Trim();
                    string admitNo = Convert.ToString(drStudent["Roll_Admit"]).Trim();
                    string applicationNo = Convert.ToString(drStudent["app_formno"]).Trim();
                    string subjectCodeValue = Convert.ToString(drStudent["subject_code"]).Trim();
                    string subjectNameValue = Convert.ToString(drStudent["subject_name"]).Trim();
                    string subjectDetailsValue = Convert.ToString(drStudent["SubjectDetails"]).Trim();

                    int column = 0;
                    FpStudentList.Sheets[0].RowCount++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(serialNo).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Tag = Convert.ToString(appNo).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Note = Convert.ToString(degreeCode).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(subjectCodeValue).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Tag = Convert.ToString(subjectCodeValue).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Note = Convert.ToString(subjectNameValue).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(subjectNameValue).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Tag = Convert.ToString(subjectCodeValue).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Note = Convert.ToString(subjectNameValue).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(subjectDetailsValue).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Tag = Convert.ToString(subjectCodeValue).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Note = Convert.ToString(subjectNameValue).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(applicationNo).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(rollNo).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(regNo).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(admitNo).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(studType).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(studName).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(batchYear).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(degreeDetail).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(degreeCode).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(semester).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(section).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;

                    column++;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Text = Convert.ToString(roomNo).Trim();
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].CellType = txtCell;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Font.Name = "Book Antiqua";
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].Locked = true;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentList.Sheets[0].Cells[FpStudentList.Sheets[0].RowCount - 1, column].VerticalAlign = VerticalAlign.Middle;
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
                lblAlertMsg.Text = "No Record(s) Were Found";
                divPopAlert.Visible = true;
                return;
            }
        }
        catch
        {
        }
    }

    #endregion GO Click

    #region Alert Close

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    #endregion Alert Close

    #region Generate Excel

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            printMaster1.Visible = false;
            string reportname = txtExcelFileName.Text.Trim().Replace(" ", "_").Trim();
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
                txtExcelFileName.Focus();
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
            rptheadname = "Staff Key List Report@ Staff Code & Name\t:\t" + lblStaffNameDisp.Text;
            string pagename = "StaffKeyListDetails.aspx";
            if (FpStudentList.Visible == true)
            {
                printMaster1.loadspreaddetails(FpStudentList, pagename, rptheadname);
            }
            printMaster1.Visible = true;
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

    #region Reusable Method

    private bool IsAdminOrStaff()
    {
        bool isAdmin = false;
        try
        {
            string admin = Convert.ToString(Session["StafforAdmin"]).Trim().ToLower();
            if (Convert.ToString(Session["StafforAdmin"]).Trim().ToLower() == "admin")
            {
                isAdmin = true;
            }
            else
            {
                staffCode = Convert.ToString(Session["Staff_Code"]).Trim();
            }
            if (string.IsNullOrEmpty(staffCode))
            {
                isAdmin = true;
            }
        }
        catch
        {
        }
        return isAdmin;
    }

    /// <summary>
    /// Developed By Malang Raja on Dec 7 2016
    /// </summary>
    /// <param name="type">0 For Roll No,1 For Register No,2 For Admission No, 3 For Student Type , 4 For Application No</param>
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
                    string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type','Application No') and value='1' " + grouporusercode + "";
                    dsSettings = dirAcc.selectDataSet(Master1);
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
                        case 4:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "application no")
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
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
            return false;
        }
    }

    private string orderByStudents()
    {
        string orderBySetting = dirAcc.selectScalarString("select value from master_Settings where settings='order_by'");
        orderBySetting = orderBySetting.Trim();
        string orderBy = "ORDER BY s.subject_code,r.Batch_Year,r.degree_code,Section,r.roll_no";
        switch (orderBySetting)
        {
            case "0":
                orderBy = "ORDER BY s.subject_code,r.Batch_Year,r.degree_code,Section,r.roll_no";
                break;
            case "1":
                orderBy = "ORDER BY s.subject_code,r.Batch_Year,r.degree_code,Section,r.Reg_No";
                break;
            case "2":
                orderBy = "ORDER BY s.subject_code,r.Batch_Year,r.degree_code,Section,r.Stud_Name";
                break;
            case "0,1,2":
                orderBy = "ORDER BY s.subject_code,r.Batch_Year,r.degree_code,Section,r.roll_no,r.Reg_No,r.stud_name";
                break;
            case "0,1":
                orderBy = "ORDER BY s.subject_code,r.Batch_Year,r.degree_code,Section,r.roll_no,r.Reg_No";
                break;
            case "1,2":
                orderBy = "ORDER BY s.subject_code,r.Batch_Year,r.degree_code,Section, r.Reg_No,r.Stud_Name";
                break;
            case "0,2":
                orderBy = "ORDER BY s.subject_code,r.Batch_Year,r.degree_code,Section,r.roll_no,r.Stud_Name";
                break;
            default:
                orderBy = "ORDER BY s.subject_code,r.Batch_Year,r.degree_code,Section,r.roll_no";
                break;
        }
        return orderBy;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="subjectCode"></param>
    /// <param name="staffcode"></param>
    /// <returns></returns>
    private DataTable getStudentData(string staffcode, string subjectCode, string[] collegeCode = null, string[] batchYear = null, string[] degreeCode = null, string[] semester = null, string[] section = null)
    {
        DataTable dtStud = new DataTable();
        try
        {
            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
            qrySemester = string.Empty;
            qrySection = string.Empty;
            string qryStaffSection = string.Empty;
            if (collegeCode != null)
            {
                string value = string.Empty;
                value = string.Join("','", collegeCode);
                if (!string.IsNullOrEmpty(value))
                {
                    qryCollegeCode = " and r.college_code in ('" + value + "')";
                }
            }
            if (batchYear != null)
            {
                string value = string.Empty;
                value = string.Join("','", batchYear);
                if (!string.IsNullOrEmpty(value))
                {
                    qryBatchYear = " and r.batch_year in ('" + value + "')";
                }
            }
            if (degreeCode != null)
            {
                string value = string.Empty;
                value = string.Join("','", degreeCode);
                if (!string.IsNullOrEmpty(value))
                {
                    qryBatchYear = " and r.degree_code in ('" + value + "')";
                }
            }
            if (semester != null)
            {
                string value = string.Empty;
                value = string.Join("','", semester);
                if (!string.IsNullOrEmpty(value))
                {
                    qryBatchYear = " and r.degree_code in ('" + value + "')";
                }
            }
            if (section != null)
            {
                string value = string.Empty;
                value = string.Join("','", section);
                if (!string.IsNullOrEmpty(value))
                {
                    qrySection = " and LTRIM(RTRIM(isnull(r.Sections,''))) in ('" + value + "')";
                    qryStaffSection = " and LTRIM(RTRIM(isnull(ss.Sections,''))) in ('" + value + "')";
                }
            }
            string orderBy = orderByStudents();
            bool staffSelector = false;
            string qryStudeStaffSelector = string.Empty;
            string minimumabsentsms = dirAcc.selectScalarString("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" +  Convert.ToString(cblCollege.SelectedValue).Trim() +  "'");
            string[] splitminimumabsentsms = minimumabsentsms.Split('-');
            if (splitminimumabsentsms.Length == 2)
            {
                int batchyearsetting = 0;
                int.TryParse(Convert.ToString(splitminimumabsentsms[1]).Trim(), out batchyearsetting);
                if (Convert.ToString(splitminimumabsentsms[0]).Trim() == "1")
                {
                    staffSelector = true;
                }
            }
            if (staffSelector)
            {
                qryStudeStaffSelector = " and sc.staffcode like '%" + staffcode + "%'";
            }

            //string selQ = " select sc.staffcode,a.app_no,r.stud_name,a.app_formno,r.Roll_No,r.Roll_Admit,r.Reg_No,case when isnull(r.Stud_Type,'Day Scholar')='Day Scholar' then 'D' else case when ISNULL(rd.Room_Name,'')='' then 'H' else 'H - '+ISNULL(rd.Room_Name,'') end  end  as Resident from applyn a, subjectChooser sc ,staff_selector ss,Registration r left join HT_HostelRegistration hr on hr.APP_No=r.App_No left  join Room_Detail rd on RoomPK=hr.RoomFK where sc.subject_no=ss.subject_no and r.App_No = a.app_no and r.Current_Semester=sc.semester and r.Roll_No = sc.roll_no and sc.subject_no='" + subjectCode + "' and ss.staff_code  like '%" + staffcode + "%'  " + qryStudeStaffSelector + orderBy;
            string selQ = "select distinct a.app_no,r.Current_Semester,r.stud_name,r.Batch_Year,r.degree_code,LTRIM(RTRIM(ISNULL(r.Sections,''))) as Section,a.app_formno,r.Roll_No,r.Roll_Admit,r.Reg_No,case when isnull(r.Stud_Type,'Day Scholar')='Day Scholar' then 'D' else case when ISNULL(rd.Room_Name,'')='' then 'H' else 'H - '+ISNULL(rd.Room_Name,'') end  end  as Resident,ISNULL(rd.Room_Name,'') as RoomNo,case when(ltrim(rtrim(isnull(c.type,'')))<>'') then ltrim(rtrim(isnull(c.type,'')))+case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then ' '+c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym+case when(ltrim(rtrim(isnull(r.Sections,'')))<>'') then ' '+ltrim(rtrim(isnull(r.Sections,''))) else '' end else c.Course_Name+' '+dt.dept_acronym+case when(ltrim(rtrim(isnull(r.Sections,'')))<>'') then ' '+ltrim(rtrim(isnull(r.Sections,''))) else '' end end  else case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym+case when(ltrim(rtrim(isnull(r.Sections,'')))<>'') then ' '+ltrim(rtrim(isnull(r.Sections,''))) else '' end else c.Course_Name+' '+dt.dept_acronym+case when(ltrim(rtrim(isnull(r.Sections,'')))<>'') then ' '+ltrim(rtrim(isnull(r.Sections,''))) else '' end end end as DegreeDetails,s.subject_code,s.subject_name,s.subject_name+' ['+subject_code+'] ' as SubjectDetails from applyn a,subject s, subjectChooser sc ,staff_selector ss,Course c,Degree dg,Department dt,Registration r left join HT_HostelRegistration hr on hr.APP_No=r.App_No left join Room_Detail rd on RoomPK=hr.RoomFK where sc.subject_no=ss.subject_no and r.App_No = a.app_no and r.Current_Semester=sc.semester and r.Roll_No = sc.roll_no and s.subject_no=sc.subject_no and ss.subject_no=s.subject_no and r.Batch_Year=ss.batch_year and dg.Degree_Code=r.degree_code and r.cc='0' and r.delFlag='0' and r.Exam_Flag<>'debar' and dg.Dept_Code=dt.Dept_Code and c.Course_Id=dg.Course_Id and ss.staff_code like '%" + staffcode + "%' and s.subject_code in(" + subjectCode + ") " + qryStudeStaffSelector + qryCollegeCode + qryBatchYear + qryDegreeCode + qrySemester + qrySection + qryStaffSection + orderBy;
            dtStud = dirAcc.selectDataTable(selQ);
        }
        catch { dtStud.Clear(); }
        return dtStud;
    }

    #endregion Reusable Method

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

    private string getCblSelectedValue(CheckBoxList cblSelected, out string[] values)
    {
        StringBuilder selectedvalue = new StringBuilder();
        values = new string[0];
        try
        {
            foreach (wc.ListItem li in cblSelected.Items)
            {
                if (li.Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append("'" + Convert.ToString(li.Value).Trim() + "'");
                    }
                    else
                    {
                        selectedvalue.Append(",'" + Convert.ToString(li.Value).Trim() + "'");
                    }
                    Array.Resize(ref values, values.Length + 1);
                    values[values.Length - 1] = Convert.ToString(li.Value).Trim();
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

    /// <summary>
    /// Select or Deselect CheckboxList
    /// </summary>
    /// <param name="cbl">CheckBoxList Control</param>
    /// <param name="selected">true or false ; Default : true</param>
    /// <param name="top">Default : Select All Items</param>
    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true, int? top = null)
    {
        try
        {
            int topVal = top ?? cbl.Items.Count;
            int count = 0;
            foreach (wc.ListItem li in cbl.Items)
            {
                li.Selected = selected;
                count++;
                if (top == count)
                    break;
            }
        }
        catch
        {
        }
    }

    #endregion

}
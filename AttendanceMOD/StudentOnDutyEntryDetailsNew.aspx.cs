using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI.WebControls;
using Farpoint = FarPoint.Web.Spread;

public partial class AttendanceMOD_StudentOnDutyEntryDetailsNew : System.Web.UI.Page
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
    string qryCourseId = string.Empty;
    string qryHours = string.Empty;

    byte dayType = 0;
    byte totalHours = 0;
    int selected = 0;

    bool isSchool = false;
    bool cellclick = false;
    bool flag_true = false;
    bool isValidDate = false;
    bool isValidFromDate = false;
    bool isValidToDate = false;

    DateTime dtFromDate = new DateTime();
    DateTime dtToDate = new DateTime();
    DateTime dtDummyDate = new DateTime();

    static Hashtable htHoursPerDay = new Hashtable();

    Institution institute;

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
                txtStudent.Text = string.Empty;
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtToDate.Attributes.Add("readonly", "readonly");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtFromDateOD.Attributes.Add("readonly", "readonly");
                txtFromDateOD.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtToDateOD.Attributes.Add("readonly", "readonly");
                txtToDateOD.Text = DateTime.Now.ToString("dd/MM/yyyy");

                chkStudentWise.Checked = false;
                divODEntryDetails.Visible = false;
                divAddStudents.Visible = false;
                divSearchAllStudents.Visible = true;
                divHalfHr.Visible = false;

                lblAlertMsg.Text = string.Empty;
                divPopAlert.Visible = false;
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                divMainContents.Visible = false;
                divUpdateOD.Visible = false;
                divDeleteOD.Visible = false;
                setLabelText();
                Bindcollege();
                BindBatch();
                BindDegree();
                BindBranch();
                BindSem();
                BindSectionDetail();
                BindAttendanceRights();

                BindCollegeOD();
                BindBatchOD();
                BindDegreeOD();
                BindBranchOD();
                BindSemOD();
                BindSectionDetailOD();
                BindReason();
                BindHour();
                BindMinute();
                SetStudentWiseSettings();

                Init_Spread(FpShowODDetails, 0);
                Init_Spread(FpStudentDetails, 1);
                this.txtStudent.Attributes.Add("onkeypress", "btnAddStudent_Click(this,'" + this.btnAddStudent.ClientID + "')");
                ddlPurpose.Attributes.Add("onfocus", "frelig()");
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Admissionno"] = "0";
                string grouporusercode = string.Empty;
                if (Session["group_code"] != null && (Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim().Split(',')[0] + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                string Master1 = "select * from Master_Settings where " + grouporusercode + "";
                ds.Clear();
                ht.Clear();
                ds = da.select_method(Master1, ht, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Admission No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
                    qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
                }
            }
            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
                    qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
                }
            }
            if (ddlBatch.Items.Count > 0 && ddlBatch.Visible == true)
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
                if (!string.IsNullOrEmpty(batchYear))
                {
                    qryBatchYear = " and r.Batch_year in(" + batchYear + ")";
                }
            }
            if (ddlDegree.Items.Count > 0)
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
                //collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
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
                    qryCollegeCode = " and college_code in(" + collegeCode + ")";
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
                    qryBatchYear = " and Batch_year in(" + batchYear + ")";
                }
            }
            if (ddlBranch.Items.Count > 0)
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
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
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
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            cblSec.Items.Clear();
            chkSec.Checked = false;
            txtSec.Enabled = false;
            txtSec.Text = "-- Select --";
            if (ddlCollege.Items.Count > 0)
            {
                //collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
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
                //if (!string.IsNullOrEmpty(collegeCode))
                //{
                //    qryCollegeCode = " and college_code in(" + collegeCode + ")";
                //}
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
                //if (!string.IsNullOrEmpty(batchYear))
                //{
                //    qryBatchYear = " and Batch_year in(" + batchYear + ")";
                //}
            }
            if (ddlBranch.Items.Count > 0)
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
                cblSec.DataSource = ds;
                cblSec.DataTextField = "sections";
                cblSec.DataValueField = "sections";
                cblSec.DataBind();
                for (int h = 0; h < cblSec.Items.Count; h++)
                {
                    cblSec.Items[h].Selected = true;
                }
                txtSec.Text = "Section" + "(" + cblSec.Items.Count + ")";
                chkSec.Checked = true;
                txtSec.Enabled = true;
            }
            else
            {
                ddlSec.Enabled = false;
                txtSec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
                FpSpread1.Sheets[0].ColumnCount = 17;
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
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

                FpSpread1.Sheets[0].Columns[5].Width = 150;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = lblDegree.Text;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

                FpSpread1.Sheets[0].Columns[6].Width = 100;
                FpSpread1.Sheets[0].Columns[6].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = lblBranch.Text;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);

                FpSpread1.Sheets[0].Columns[7].Width = 70;
                FpSpread1.Sheets[0].Columns[7].Locked = true;
                FpSpread1.Sheets[0].Columns[7].Resizable = false;
                FpSpread1.Sheets[0].Columns[7].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = lblSec.Text;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);

                FpSpread1.Sheets[0].Columns[8].Width = 80;
                FpSpread1.Sheets[0].Columns[8].Locked = true;
                FpSpread1.Sheets[0].Columns[8].Resizable = false;
                FpSpread1.Sheets[0].Columns[8].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Purpose";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);

                FpSpread1.Sheets[0].Columns[9].Width = 100;
                FpSpread1.Sheets[0].Columns[9].Locked = true;
                FpSpread1.Sheets[0].Columns[9].Resizable = false;
                FpSpread1.Sheets[0].Columns[9].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "From Date";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);

                FpSpread1.Sheets[0].Columns[10].Width = 100;
                FpSpread1.Sheets[0].Columns[10].Locked = true;
                FpSpread1.Sheets[0].Columns[10].Resizable = false;
                FpSpread1.Sheets[0].Columns[10].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "To Date";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);

                FpSpread1.Sheets[0].Columns[11].Width = 50;
                FpSpread1.Sheets[0].Columns[11].Locked = true;
                FpSpread1.Sheets[0].Columns[11].Resizable = false;
                FpSpread1.Sheets[0].Columns[11].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Out Time";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 11, 2, 1);

                FpSpread1.Sheets[0].Columns[12].Width = 100;
                FpSpread1.Sheets[0].Columns[12].Locked = true;
                FpSpread1.Sheets[0].Columns[12].Resizable = false;
                FpSpread1.Sheets[0].Columns[12].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "In Time";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 12, 2, 1);

                FpSpread1.Sheets[0].Columns[13].Width = 25;
                FpSpread1.Sheets[0].Columns[13].Locked = true;
                FpSpread1.Sheets[0].Columns[13].Resizable = false;
                FpSpread1.Sheets[0].Columns[13].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Type";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 13, 2, 1);

                FpSpread1.Sheets[0].Columns[14].Width = 100;
                FpSpread1.Sheets[0].Columns[14].Locked = true;
                FpSpread1.Sheets[0].Columns[14].Resizable = false;
                FpSpread1.Sheets[0].Columns[14].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Total No.Of Hrs";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 14, 2, 1);

                FpSpread1.Sheets[0].Columns[15].Width = 100;
                FpSpread1.Sheets[0].Columns[15].Locked = true;
                FpSpread1.Sheets[0].Columns[15].Resizable = false;
                FpSpread1.Sheets[0].Columns[15].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 15].Text = "Total No.Of Days";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 15, 2, 1);

                FpSpread1.Sheets[0].Columns[16].Width = 45;
                FpSpread1.Sheets[0].Columns[16].Locked = false;
                FpSpread1.Sheets[0].Columns[16].Resizable = false;
                FpSpread1.Sheets[0].Columns[16].Visible = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 16].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 16, 2, 1);

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
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = lblSemOD.Text;
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            int count = 0;
            if (chkSec.Checked == true)
            {
                count++;
                for (int i = 0; i < cblSec.Items.Count; i++)
                {
                    cblSec.Items[i].Selected = true;
                }
                txtSec.Text = "Section(" + (cblSec.Items.Count) + ")";
                txtSec.Enabled = true;
            }
            else
            {
                for (int i = 0; i < cblSec.Items.Count; i++)
                {
                    cblSec.Items[i].Selected = false;
                }
                txtSec.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            int commcount = 0;
            chkSec.Checked = false;
            for (int i = 0; i < cblSec.Items.Count; i++)
            {
                if (cblSec.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblSec.Items.Count)
                {
                    chkSec.Checked = true;
                }
                txtSec.Text = "Section(" + Convert.ToString(commcount) + ")";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Index Changed Events

    #region Show ON Duty Spread Events

    protected void FpShowODDetails_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            cellclick = true;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void FpShowODDetails_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string ar = e.CommandArgument.ToString();
            string[] spitval = ar.Split(',');
            string[] spitrow = spitval[0].Split('=');
            string actrow = spitrow[1].ToString();
            string[] spiticol = spitval[1].Split('=');
            string[] spitvn = spiticol[1].Split('}');
            string actcol = spitvn[0].ToString();
            btnOnDutyDelete.Visible = false;
            btnOnDutyUpdate.Visible = false;
            divUpdateOD.Visible = false;
            divDeleteOD.Visible = false;
            if (flag_true == false && actrow == "0")
            {
                string seltext = FpShowODDetails.Sheets[0].Cells[0, 16].Value.ToString();
                int setval = 0;
                if (seltext.Trim() == "1")
                {
                    btnOnDutyDelete.Visible = true;
                    divDeleteOD.Visible = true;
                    if (FpShowODDetails.Sheets[0].RowCount == 2)
                    {
                        btnOnDutyUpdate.Visible = true;
                        divUpdateOD.Visible = true;
                    }
                    else
                    {
                        btnOnDutyUpdate.Visible = false;
                        divUpdateOD.Visible = false;
                    }
                }
                else
                {
                    btnOnDutyDelete.Visible = false;
                    btnOnDutyUpdate.Visible = false;
                    divUpdateOD.Visible = false;
                    divDeleteOD.Visible = false;
                }
                for (int j = 1; j < Convert.ToInt16(FpShowODDetails.Sheets[0].RowCount); j++)
                {
                    if (seltext != "System.Object" && seltext.Trim() != "-1")
                    {
                        FpShowODDetails.Sheets[0].Cells[j, 16].Value = seltext.ToString();
                    }
                }
                flag_true = true;
            }
            else if (flag_true == false && actrow != "0")
            {
                int setval = 0;
                for (int j = 1; j < Convert.ToInt16(FpShowODDetails.Sheets[0].RowCount); j++)
                {
                    int isval = Convert.ToInt32(FpShowODDetails.Sheets[0].Cells[j, 16].Value);
                    if (isval == 1)
                    {
                        setval++;
                    }
                }
                if (setval == 1)
                {
                    btnOnDutyDelete.Visible = true;
                    btnOnDutyUpdate.Visible = true;
                    divUpdateOD.Visible = true;
                    divDeleteOD.Visible = true;
                }
                else if (setval > 1)
                {
                    btnOnDutyDelete.Visible = true;
                    btnOnDutyUpdate.Visible = false;
                    divUpdateOD.Visible = false;
                    divDeleteOD.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void FpShowODDetails_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (cellclick == true)
            {
                //string activerow = string.Empty;
                //string activecol = string.Empty;
                //activerow = sprdretrivedate.ActiveSheetView.ActiveRow.ToString();
                //activecol = sprdretrivedate.ActiveSheetView.ActiveColumn.ToString();
                //string retroll = sprdretrivedate.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                //string appNo = sprdretrivedate.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag.ToString();
                //string purpose = sprdretrivedate.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Text;
                //FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                //FpStudentDetails.Sheets[0].Columns[6].CellType = chkcell;
                //string studinfo = "select Stud_name,Reg_no,current_semester,degree_code from registration where app_no='" + appNo + "'";
                ////SqlDataAdapter dastudinfo = new SqlDataAdapter(studinfo, con1);
                //DataSet dsstudinfo = d2.select_method(studinfo, hat, "Text");
                ////dastudinfo.Fill(dsstudinfo);
                ////con1.Close();
                ////con1.Open();
                //int sno = 0;
                //FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                //if (dsstudinfo.Tables.Count > 0 && dsstudinfo.Tables[0].Rows.Count > 0)
                //{
                //    //addmenudeletepopup1.Visible = true;
                //    FpStudentDetails.Sheets[0].RowCount = 0;
                //    panelrollnopop.Visible = true;
                //    sno++;
                //    string studname = dsstudinfo.Tables[0].Rows[0]["stud_name"].ToString();
                //    string regno = dsstudinfo.Tables[0].Rows[0]["Reg_no"].ToString();
                //    string sem = dsstudinfo.Tables[0].Rows[0]["current_semester"].ToString();
                //    string degreecode = dsstudinfo.Tables[0].Rows[0]["degree_code"].ToString();
                //    attendenace(Convert.ToInt32(degreecode), Convert.ToInt32(sem));
                //    FpStudentDetails.Sheets[0].RowCount = FpStudentDetails.Sheets[0].RowCount + 1;
                //    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                //    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Tag = appNo;
                //    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Text = retroll;
                //    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Text = regno;
                //    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].CellType = textcel_type;
                //    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].CellType = textcel_type;
                //    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].Text = studname;
                //    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].Text = sem;
                //    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Value = true;
                //    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                //    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                //    txtselecthours.Text = Convert.ToString(sprdretrivedate.Sheets[0].Cells[Convert.ToInt32(activerow), 11].Tag);
                //    txtfrmdate.Text = Convert.ToString(sprdretrivedate.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text);
                //    txttodate.Text = Convert.ToString(sprdretrivedate.Sheets[0].Cells[Convert.ToInt32(activerow), 10].Text);
                //    addmenusavepopup1.Text = "Update";
                //    //strat=====added by Manikandan
                //    string getouttime = sprdretrivedate.Sheets[0].Cells[Convert.ToInt32(activerow), 11].Text;
                //    string getintime = sprdretrivedate.Sheets[0].Cells[Convert.ToInt32(activerow), 12].Text;
                //    string[] splitouttime = getouttime.Split(new char[] { ' ' });
                //    string[] splitintime = getintime.Split(new char[] { ' ' });
                //    string splitedouttime = splitouttime[0].ToString();
                //    string splitedoutmeridian = splitouttime[1].ToString();
                //    string splitedintime = splitintime[0].ToString();
                //    string splitedinmeridian = splitintime[1].ToString();
                //    //Modified By Srinathss 21/8/2013
                //    //string[] outtime = splitedouttime.Split(new char[] { '-' });
                //    string[] outtime = splitedouttime.Split(new char[] { ':' });
                //    string hour = outtime[0];
                //    string min = outtime[1];
                //    if (outtime[0].Length == 1)
                //    {
                //        hour = "0" + outtime[0];
                //    }
                //    if (min.Length == 1)
                //    {
                //        min = "0" + outtime[1];
                //    }
                //    // string[] intime = splitedintime.Split(new char[] { '-' });
                //    string[] intime = splitedintime.Split(new char[] { ':' });
                //    string outhr = intime[0].ToString();
                //    string outmm = intime[1].ToString();
                //    if (outhr.Length == 1)
                //    {
                //        outhr = "0" + outhr;
                //    }
                //    if (outmm.Length == 1)
                //    {
                //        outmm = "0" + outmm;
                //    }
                //    ddlouttimehh.Text = hour;
                //    ddlouttimemm.Text = min;
                //    ddlouttimeses.Text = splitedoutmeridian;
                //    ddlintimehh.Text = outhr;
                //    ddlintimemm.Text = outmm;
                //    ddlintimeses.Text = splitedinmeridian;
                //    purpose = d2.GetFunction("select textcode from TextValTable where TextCriteria='Attrs' and textval='" + purpose + "' and college_code='" + collegecode + "'");
                //    if (purpose.Trim() != "" && purpose.Trim() != "0")
                //    {
                //        ddlpurpose.Text = purpose;
                //    }
                //    addmenusavepopup1.Enabled = true;
                //end=========
                //}
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void FpCalculateCGPA_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            FpCalculateCGPA.SaveChanges();
            int r = FpCalculateCGPA.Sheets[0].ActiveRow;
            int j = FpCalculateCGPA.Sheets[0].ActiveColumn;
            if (r == 0 && j == 0)
            {
                int val = 0;
                int.TryParse(Convert.ToString(FpCalculateCGPA.Sheets[0].Cells[r, j].Value).Trim(), out val);
                for (int row = 1; row < FpCalculateCGPA.Sheets[0].RowCount; row++)
                {
                    if (val == 1)
                        FpCalculateCGPA.Sheets[0].Cells[row, j].Value = 1;
                    else
                        FpCalculateCGPA.Sheets[0].Cells[row, j].Value = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Show ON Duty Spread Events

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
            divUpdateOD.Visible = false;
            divDeleteOD.Visible = false;

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
            isValidDate = false;
            isValidFromDate = false;
            isValidToDate = false;

            fromDate = Convert.ToString(txtFromDate.Text).Trim();
            toDate = Convert.ToString(txtToDate.Text).Trim();
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
                    qrySemester = " and r.current_semester in(" + semester + ")";
                }
            }
            if (ddlSec.Items.Count > 0)
            {
                section = string.Empty;
                qrySection = string.Empty;
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
                if (!string.IsNullOrEmpty(section))
                {
                    qrySection = " and sections in(" + section + ")";
                }
            }
            else
            {
                section = string.Empty;
                qrySection = string.Empty;
            }
            if (fromDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
                isValidFromDate = isValidDate;
                if (!isValidDate)
                {
                    lblAlertMsg.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblAlertMsg.Visible = true;
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "Please Choose From Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            if (toDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
                isValidToDate = isValidDate;
                if (!isValidDate)
                {
                    lblAlertMsg.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblAlertMsg.Visible = true;
                    divPopAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblAlertMsg.Text = "Please Choose To Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            string qryDate = string.Empty;
            if (dtFromDate > dtToDate)
            {
                lblAlertMsg.Text = "From Date Must Be Lesser Than Or Equal To To Date";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                qryDate = " and convert(datetime,od.fromdate,105) between '" + dtFromDate.ToString("MM/dd/yyyy") + "' and '" + dtToDate.ToString("MM/dd/yyyy") + "'";
            }
            orderBySetting = da.GetFunction("select value from master_Settings where settings='order_by'");
            orderBySetting = orderBySetting.Trim();
            orderBy = "ORDER BY fromdate,rollNoLen,r.roll_no";
            switch (orderBySetting)
            {
                case "0":
                    orderBy = "ORDER BY fromdate,rollNoLen,r.roll_no";
                    break;
                case "1":
                    orderBy = "ORDER BY fromdate,regNoLen,r.Reg_No";
                    break;
                case "2":
                    orderBy = "ORDER BY fromdate,r.Stud_Name";
                    break;
                case "0,1,2":
                    orderBy = "ORDER BY fromdate,rollNoLen,r.roll_no,regNoLen,r.Reg_No,r.stud_name";
                    break;
                case "0,1":
                    orderBy = "ORDER BY rollNoLen,r.roll_no,regNoLen,r.Reg_No";
                    break;
                case "1,2":
                    orderBy = "ORDER BY fromdate,regNoLen,r.Reg_No,r.Stud_Name";
                    break;
                case "0,2":
                    orderBy = "ORDER BY fromdate,rollNoLen,r.roll_no,r.Stud_Name";
                    break;
                default:
                    orderBy = "ORDER BY fromdate,rollNoLen,r.roll_no";
                    break;
            }
            Farpoint.SheetView svsort = new FarPoint.Web.Spread.SheetView();
            Farpoint.CheckBoxCellType chkAll = new Farpoint.CheckBoxCellType();
            chkAll.AutoPostBack = true;
            Farpoint.CheckBoxCellType chkSingleCell = new Farpoint.CheckBoxCellType();
            if (!string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qrySemester) && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate) && isValidFromDate && isValidToDate)
            {
                //qry = "select distinct r.college_code,r.roll_no,r.reg_no,r.Roll_Admit,stud_name,purpose,fromdate,todate,outtime,intime,attnd_type,len(r.Reg_No),len(r.roll_no),no_of_hourse,hourse ,r.degree_code,r.sections,r.app_no from registration r,onduty_stud od where od.roll_no=r.roll_no and convert(datetime,fromdate,105) between '" + dtFromDate.ToString("MM/dd/yyyy") + "' and '" + dtToDate.ToString("MM/dd/yyyy") + "' and r.roll_no in(select roll_no from registration where degree_code='" + ddlBranch.SelectedValue + "' and batch_year='" + ddlBatch.SelectedValue + "' and current_semester='" + ddlSem.SelectedItem.Value + "' " + qrySection + ")  " + orderBy;
                qry = "select distinct r.college_code,r.Current_Semester,r.Batch_Year,r.roll_no,r.reg_no,r.Roll_Admit,r.stud_name,od.purpose,convert(varchar, od.fromdate, 103) as fromdate,convert(varchar, od.todate, 103)  as todate,convert(varchar, od.outtime, 108) as outtime,convert(varchar, od.intime, 108) as intime,od.attnd_type,len(r.Reg_No) as regNoLen,len(r.roll_no) rollNoLen,od.no_of_hourse,od.hourse,r.degree_code,r.sections,r.app_no from registration r,onduty_stud od where od.roll_no=r.roll_no " + qryCollegeCode + qryBatchYear + qryDegreeCode + qrySemester + qrySection + qryDate + " " + orderBy;
                dsODStudentDetails.Clear();
                dsODStudentDetails = da.select_method_wo_parameter(qry, "text");
                qry = "select d.Degree_Code,(c.Course_Name ) as degreename,(dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id ; select No_of_hrs_per_day,degree_code,semester from periodattndschedule where degree_code in(" + degreeCode + ") and semester in(" + semester + ")";
                dsDegreeDetails = da.select_method_wo_parameter(qry, "text");
                if (dsODStudentDetails.Tables.Count > 0 && dsODStudentDetails.Tables[0].Rows.Count > 0)
                {
                    Init_Spread(FpShowODDetails, 0);
                    FpShowODDetails.Sheets[0].RowCount = 1;
                    FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 16].CellType = chkAll;
                    FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 16].Font.Name = "Book Antiqua";
                    FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 16].Locked = false;
                    FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 16].HorizontalAlign = HorizontalAlign.Center;
                    FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 16].VerticalAlign = VerticalAlign.Middle;
                    FpShowODDetails.Sheets[0].SpanModel.Add(FpShowODDetails.Sheets[0].RowCount - 1, 0, 1, 16);
                    svsort = FpShowODDetails.ActiveSheetView;
                    svsort.AllowSort = true;
                    int serialNo = 0;
                    foreach (DataRow drODStudent in dsODStudentDetails.Tables[0].Rows)
                    {
                        serialNo++;
                        FpShowODDetails.Sheets[0].RowCount++;
                        string appNo = string.Empty;
                        string rollNo = string.Empty;
                        string regNo = string.Empty;
                        string admitNo = string.Empty;
                        string studentName = string.Empty;
                        string fromDateNew = string.Empty;
                        string toDateNew = string.Empty;
                        string outTime = string.Empty;
                        string inTime = string.Empty;
                        string purpose = string.Empty;
                        string degreeName = string.Empty;
                        string departmentName = string.Empty;
                        string collegeCodeNew = string.Empty;
                        string batchYearNew = string.Empty;
                        string degreeCodeNew = string.Empty;
                        string currentSemester = string.Empty;
                        string sectionNew = string.Empty;
                        string attendanceTypeVal = string.Empty;
                        string attendanceType = string.Empty;
                        string noOfHrs = string.Empty;
                        double noOfHrsCount = 0;
                        double noOfDays = 0;
                        double totalHoursNew = 0;
                        double remainderDay = 0;
                        string hours = string.Empty;
                        DateTime dtFromDateNew = new DateTime();
                        DateTime dtToDateNew = new DateTime();
                        DateTime dtOutTimeNew = new DateTime();
                        DateTime dtInTimeNew = new DateTime();

                        appNo = Convert.ToString(drODStudent["app_no"]).Trim();
                        rollNo = Convert.ToString(drODStudent["roll_no"]).Trim();
                        regNo = Convert.ToString(drODStudent["reg_no"]).Trim();
                        admitNo = Convert.ToString(drODStudent["Roll_Admit"]).Trim();
                        studentName = Convert.ToString(drODStudent["stud_name"]).Trim();
                        fromDateNew = Convert.ToString(drODStudent["fromdate"]).Trim();
                        toDateNew = Convert.ToString(drODStudent["todate"]).Trim();
                        outTime = Convert.ToString(drODStudent["outtime"]).Trim();
                        inTime = Convert.ToString(drODStudent["intime"]).Trim();
                        purpose = Convert.ToString(drODStudent["purpose"]).Trim();
                        collegeCodeNew = Convert.ToString(drODStudent["college_code"]).Trim();
                        batchYearNew = Convert.ToString(drODStudent["Batch_Year"]).Trim();
                        degreeCodeNew = Convert.ToString(drODStudent["degree_code"]).Trim();
                        currentSemester = Convert.ToString(drODStudent["Current_Semester"]).Trim();
                        sectionNew = Convert.ToString(drODStudent["sections"]).Trim();
                        attendanceTypeVal = Convert.ToString(drODStudent["attnd_type"]).Trim();
                        attendanceType = Convert.ToString(drODStudent["attnd_type"]).Trim();
                        noOfHrs = Convert.ToString(drODStudent["no_of_hourse"]).Trim();
                        hours = Convert.ToString(drODStudent["hourse"]).Trim();
                        double.TryParse(noOfHrs, out noOfHrsCount);

                        DateTime.TryParseExact(fromDateNew, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDateNew);
                        DateTime.TryParseExact(toDateNew, "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDateNew);
                        DateTime.TryParseExact(outTime, "HH:mm:ss", null, DateTimeStyles.None, out dtOutTimeNew);
                        DateTime.TryParseExact(inTime, "HH:mm:ss", null, DateTimeStyles.None, out dtInTimeNew);
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
                        if (dsDegreeDetails.Tables.Count > 1 && dsDegreeDetails.Tables[1].Rows.Count > 0)
                        {
                            dsDegreeDetails.Tables[1].DefaultView.RowFilter = "Degree_code='" + degreeCodeNew + "' and semester='" + currentSemester + "'";
                            dvDegreeName = dsDegreeDetails.Tables[1].DefaultView;
                            if (dvDegreeName.Count > 0)
                            {
                                string totalHours = Convert.ToString(dvDegreeName[0]["No_of_hrs_per_day"]);
                                double.TryParse(totalHours, out totalHoursNew);
                            }
                        }
                        noOfHrsCount = 0;
                        noOfDays = 0;
                        string noofODday = string.Empty;
                        double.TryParse(Convert.ToString(noOfHrs), out noOfHrsCount);
                        noOfDays = noOfHrsCount / totalHoursNew;
                        string noofday = Convert.ToString(noOfDays);
                        remainderDay = 0;
                        remainderDay = noOfHrsCount % totalHoursNew;
                        string[] dayandhours = noofday.Split('.');
                        if (dayandhours.Length > 1)
                        {
                            if (dayandhours[0] == "0")
                                noofODday = Convert.ToString(remainderDay) + " Hours";
                            else
                                noofODday = Convert.ToString(dayandhours[0]) + " Days " + Convert.ToString(remainderDay) + " Hours";
                        }
                        else
                        {
                            noofODday = Convert.ToString(dayandhours[0]);
                        }
                        Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialNo).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(appNo).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(degreeCodeNew).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 0].Locked = true;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(rollNo).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(collegeCodeNew).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(currentSemester).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 1].Locked = true;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(regNo).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(collegeCode).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(courseName).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(admitNo).Trim();
                        //FpShowODDetails.Sheets[3].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(collegeCode).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(courseName).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(studentName).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(collegeCode).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 4].Note = Convert.ToString(courseName).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 4].Locked = true;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(degreeName).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(collegeCode).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 5].Note = Convert.ToString(courseName).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 5].CellType = txtCell;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 5].Locked = true;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(departmentName).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(collegeCode).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 6].Note = Convert.ToString(courseName).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 6].CellType = txtCell;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 6].Locked = true;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;

                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(sectionNew).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(collegeCode).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 7].Note = Convert.ToString(courseName).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 7].CellType = txtCell;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 7].Locked = true;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;

                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(purpose).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 8].Tag = Convert.ToString(collegeCode).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 8].Note = Convert.ToString(courseName).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 8].CellType = txtCell;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 8].Locked = true;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;

                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 9].Text = dtFromDateNew.ToString("dd/MM/yyyy");
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 9].Tag = Convert.ToString(collegeCode).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 9].Note = Convert.ToString(courseName).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 9].CellType = txtCell;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 9].Locked = true;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 9].VerticalAlign = VerticalAlign.Middle;

                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 10].Text = dtToDateNew.ToString("dd/MM/yyyy");
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 10].Tag = Convert.ToString(collegeCode).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 10].Note = Convert.ToString(courseName).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 10].CellType = txtCell;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 10].Locked = true;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 10].VerticalAlign = VerticalAlign.Middle;

                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 11].Text = dtOutTimeNew.ToString("hh:mm:ss tt");
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 11].Tag = Convert.ToString(hours).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 11].Note = Convert.ToString(courseName).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 11].CellType = txtCell;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 11].Locked = true;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 11].VerticalAlign = VerticalAlign.Middle;

                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 12].Text = dtInTimeNew.ToString("hh:mm:ss tt");
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 12].Tag = Convert.ToString(collegeCode).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 12].Note = Convert.ToString(courseName).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 12].CellType = txtCell;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 12].Font.Name = "Book Antiqua";
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 12].Locked = true;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Center;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 12].VerticalAlign = VerticalAlign.Middle;

                        if (!string.IsNullOrEmpty(attendanceTypeVal))
                        {
                            attendanceType = GetAttendanceStatusName(attendanceTypeVal);
                        }
                        else
                        {
                            attendanceType = "--";
                        }
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 13].Text = Convert.ToString(attendanceType).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 13].Tag = Convert.ToString(collegeCode).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 13].Note = Convert.ToString(courseName).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 13].CellType = txtCell;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 13].Font.Name = "Book Antiqua";
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 13].Locked = true;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 13].HorizontalAlign = HorizontalAlign.Center;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 13].VerticalAlign = VerticalAlign.Middle;

                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 14].Text = Convert.ToString(noOfHrs).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 14].Tag = Convert.ToString(collegeCode).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 14].Note = Convert.ToString(courseName).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 14].CellType = txtCell;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 14].Font.Name = "Book Antiqua";
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 14].Locked = true;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 14].HorizontalAlign = HorizontalAlign.Center;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 14].VerticalAlign = VerticalAlign.Middle;

                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 15].Text = Convert.ToString(noofODday).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 15].Tag = Convert.ToString(collegeCode).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 15].Note = Convert.ToString(courseName).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 15].CellType = txtCell;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 15].Font.Name = "Book Antiqua";
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 15].Locked = true;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 15].HorizontalAlign = HorizontalAlign.Center;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 15].VerticalAlign = VerticalAlign.Middle;

                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 16].Tag = Convert.ToString(collegeCode).Trim();
                        //FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 16].Note = Convert.ToString(courseName).Trim();
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 16].CellType = chkSingleCell;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 16].Font.Name = "Book Antiqua";
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 16].Locked = false;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 16].HorizontalAlign = HorizontalAlign.Center;
                        FpShowODDetails.Sheets[0].Cells[FpShowODDetails.Sheets[0].RowCount - 1, 16].VerticalAlign = VerticalAlign.Middle;

                    }
                    divMainContents.Visible = true;
                    FpShowODDetails.Sheets[0].PageSize = FpShowODDetails.Sheets[0].RowCount;
                    FpShowODDetails.Width = 980;
                    FpShowODDetails.Height = 500;
                    FpShowODDetails.SaveChanges();
                    FpShowODDetails.Visible = true;
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Button Go Click

    #region Button Add Click

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            divODEntryDetails.Visible = true;
            chkStudentWise.Checked = false;
            divAddStudents.Visible = false;
            divHalfHr.Visible = false;
            divSearchAllStudents.Visible = true;
            btnRemoveOdStudents.Visible = false;
            txtStudent.Text = string.Empty;
            btnPopSaveOD.Text = "Save";
            txtFromDateOD.Attributes.Add("readonly", "readonly");
            txtFromDateOD.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDateOD.Attributes.Add("readonly", "readonly");
            txtToDateOD.Text = DateTime.Now.ToString("dd/MM/yyyy");
            BindCollegeOD();
            BindBatchOD();
            BindDegreeOD();
            BindBranchOD();
            BindSemOD();
            BindSectionDetailOD();
            BindReason();
            BindHour();
            BindMinute();
            if (ddlCollegeOD.Items.Count > 0)
            {
                ddlCollegeOD.Enabled = true;
            }
            if (ddlBatchOD.Items.Count > 0)
            {
                ddlBatchOD.Enabled = true;
            }
            if (ddlDegreeOD.Items.Count > 0)
            {
                ddlDegreeOD.Enabled = true;
            }
            if (ddlBranchOD.Items.Count > 0)
            {
                ddlBranchOD.Enabled = true;
            }
            if (ddlSemOD.Items.Count > 0)
            {
                ddlSemOD.Enabled = true;
            }
            if (ddlSecOD.Items.Count > 0)
            {
                ddlSecOD.Enabled = true;
            }
            if (ddlPurpose.Items.Count > 0)
            {
                ddlPurpose.Enabled = true;
            }
            if (ddlAttendanceOption.Items.Count > 0)
            {
                ddlAttendanceOption.Enabled = true;
            }
            if (ddlOutTimeHr.Items.Count > 0)
            {
                ddlOutTimeHr.Enabled = true;
            }
            if (ddlOutTimeMM.Items.Count > 0)
            {
                ddlOutTimeMM.Enabled = true;
            }
            if (ddlOutTimeSess.Items.Count > 0)
            {
                ddlOutTimeSess.Enabled = true;
            }
            if (ddlInTimeHr.Items.Count > 0)
            {
                ddlInTimeHr.Enabled = true;
            }
            if (ddlInTimeMM.Items.Count > 0)
            {
                ddlInTimeMM.Enabled = true;
            }
            if (ddlInTimeSess.Items.Count > 0)
            {
                ddlInTimeSess.Enabled = true;
            }
            Init_Spread(FpStudentDetails, 1);
            Farpoint.CheckBoxCellType chkAll = new Farpoint.CheckBoxCellType();
            chkAll.AutoPostBack = true;
            FpStudentDetails.Sheets[0].RowCount = 1;
            FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].CellType = chkAll;
            FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].Locked = false;
            FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
            ShowStudentsList(0);
            SetDefaultODEntry();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Button Add Click

    #region Generate Excel

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            Printcontrol1.Visible = false;
            string reportname = txtexcelname1.Text.Trim().Replace(" ", "_").Trim();
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (FpShowODDetails.Visible == true)
                {
                    da.printexcelreport(FpShowODDetails, reportname);
                }
                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() :((ddlCollege.Items.Count>0)?Convert.ToString(ddlCollege.SelectedValue).Trim():"13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            if (FpShowODDetails.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpShowODDetails, pagename, rptheadname);
            }
            Printcontrol1.Visible = true;
            lbl_norec1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Print PDF

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
            da.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13"))), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void btnODPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            txtStudent.Text = string.Empty;
            txtStudent.Focus();
            lblODAlertMsg.Text = string.Empty;
            divPopODAlert.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, (((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13"))), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion  Popup Close

    #region Popup Confimation

    protected void btnConfirmYes_Click(object sender, EventArgs e)
    {
        try
        {
            divPopAlert.Visible = false;
            divPopODAlert.Visible = false;
            lblODAlertMsg.Text = string.Empty;
            divConfirm.Visible = false;
            bool isSaveSucc = false;
            if (lblSaveorDelete.Text.Trim() == "1")
            {
                save();
            }
            else if (lblSaveorDelete.Text.Trim() == "2") //delete
            {
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void btnConfirmNo_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirm.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void btnConfirmShowYes_Click(object sender, EventArgs e)
    {
        try
        {
            divPopAlert.Visible = false;
            divPopODAlert.Visible = false;
            lblODAlertMsg.Text = string.Empty;
            divConfirmShow.Visible = false;
            bool isSaveSucc = false;
            if (lblSaveorDeleteShow.Text.Trim() == "2")
            {
                deleteODDetails();
            }
            else if (lblSaveorDeleteShow.Text.Trim() == "1")
            {
                updateODDetails();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void btnConfirmShowNo_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirmShow.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion  Popup Confimation

    #region Update OnDuty Details

    private void updateODDetails()
    {
        try
        {
            lblPopODErr.Text = string.Empty;
            lblPopODErr.Visible = false;
            divPopODAlert.Visible = false;
            chkStudentWise.Checked = false;
            divAddStudents.Visible = false;
            divSearchAllStudents.Visible = true;
            btnRemoveOdStudents.Visible = false;
            FpShowODDetails.SaveChanges();
            for (int res = 1; res < FpShowODDetails.Sheets[0].RowCount; res++)
            {
                int isval = 0;
                isval = Convert.ToInt32(FpShowODDetails.Sheets[0].Cells[res, 16].Value);
                if (isval == 1)
                {
                    //loadreason();
                    BindReason();
                    string activerow = Convert.ToString(res).Trim();
                    string retAppNo = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag).Trim();
                    string retroll = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text).Trim();
                    string purpose = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Text).Trim();
                    FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                    FpStudentDetails.Sheets[0].Columns[6].CellType = chkcell;
                    string attedancetype = FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(activerow), 13].Text;
                    string studinfo = "select r.Stud_name,r.Roll_Admit,r.Reg_no,r.current_semester,r.degree_code,r.batch_year,r.sections,r.app_no,r.roll_no,r.college_code,c.Course_Id from registration r,course c,Degree dg,Department dt where dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code=r.degree_code and r.college_code=dt.college_code and r.college_code=c.college_code and c.college_code=dg.college_code AND r.app_no='" + retAppNo + "'";
                    DataSet dsstudinfo = da.select_method_wo_parameter(studinfo, "Text");
                    int sno = 0;
                    FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                    if (dsstudinfo.Tables.Count > 0 && dsstudinfo.Tables[0].Rows.Count > 0)
                    {
                        divODEntryDetails.Visible = true;
                        sno++;
                        string studname = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["stud_name"]).Trim();
                        string app_No = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["app_No"]).Trim();
                        string rollno = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["roll_no"]).Trim();
                        string regno = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["Reg_no"]).Trim();
                        string sem = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["current_semester"]).Trim();
                        string degreecode = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["degree_code"]).Trim();
                        string batchval = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["batch_year"]).Trim();
                        string section = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["sections"]).Trim();
                        string collegeCode = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["college_code"]).Trim();
                        string courseId = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["Course_Id"]).Trim();
                        string admissionno = Convert.ToString(dsstudinfo.Tables[0].Rows[0]["Roll_Admit"]).Trim();
                        attendenace(degreecode, sem);
                        ddlCollegeOD.Enabled = false;
                        ddlBatchOD.Enabled = false;
                        ddlBranchOD.Enabled = false;
                        ddlDegreeOD.Enabled = false;
                        ddlSemOD.Enabled = false;
                        ddlSecOD.Enabled = false;
                        if (ddlCollegeOD.Items.Count > 0)
                        {
                            ddlCollegeOD.SelectedValue = collegeCode;
                            ddlCollegeOD_SelectedIndexChanged(new object(), new EventArgs());
                        }
                        if (ddlBatchOD.Items.Count > 0)
                        {
                            ddlBatchOD.SelectedValue = batchval;
                            ddlBatchOD_SelectedIndexChanged(new object(), new EventArgs());
                        }
                        if (ddlDegreeOD.Items.Count > 0)
                        {
                            ddlDegreeOD.SelectedValue = courseId;
                            ddlDegreeOD_SelectedIndexChanged(new object(), new EventArgs());
                        }
                        if (ddlBranchOD.Items.Count > 0)
                        {
                            ddlBranchOD.SelectedValue = degreecode;
                            ddlBranchOD_SelectedIndexChanged(new object(), new EventArgs());
                        }
                        if (ddlSemOD.Items.Count > 0)
                        {
                            ddlSemOD.SelectedValue = sem;
                            ddlSemOD_SelectedIndexChanged(new object(), new EventArgs());
                        }
                        if (ddlSecOD.Items.Count > 0)
                        {
                            ddlSecOD.Enabled = false;
                            ddlSecOD.SelectedIndex = 0;
                            if (section.Trim().ToLower() != "")
                            {
                                ddlSecOD.SelectedValue = section;
                            }
                        }
                        divPopODAlert.Visible = false;
                        FpStudentDetails.Sheets[0].RowCount = 0;
                        FpStudentDetails.Sheets[0].RowCount = FpStudentDetails.Sheets[0].RowCount + 1;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Tag = batchval;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Note = collegeCode;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].CellType = textcel_type;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Tag = degreecode;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Text = rollno;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Value = rollno;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Text = regno;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Tag = section;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Note = courseId;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].CellType = textcel_type;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].Text = admissionno;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].Text = studname;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Text = sem;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Tag = app_No;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].CellType = new Farpoint.CheckBoxCellType();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].Locked = true;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].Value = 1;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Text = sem;
                        txtNoOfHours.Text = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(activerow), 11].Tag).Trim();
                        txtFromDateOD.Text = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text).Trim();
                        txtToDate.Text = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(activerow), 10].Text).Trim();
                        btnPopSaveOD.Text = "Update";
                        string getouttime = FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(activerow), 11].Text;
                        string getintime = FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(activerow), 12].Text;
                        string[] splitouttime = getouttime.Split(new char[] { ' ' });
                        string[] splitintime = getintime.Split(new char[] { ' ' });
                        string splitedouttime = splitouttime[0].ToString();
                        string splitedoutmeridian = splitouttime[1].ToString();
                        string splitedintime = splitintime[0].ToString();
                        string splitedinmeridian = splitintime[1].ToString();
                        string[] hourList = txtNoOfHours.Text.Split(',');
                        if (hourList.Length > 0)
                        {
                            cblHours.Items.Clear();
                            int item = 0;
                            foreach (string hrslst in hourList)
                            {
                                cblHours.Items.Add(new ListItem(hrslst, hrslst));
                                cblHours.Items[item].Selected = true;
                                item++;
                            }
                        }
                        string[] outtime = splitedouttime.Split(new char[] { ':' });
                        string hour = outtime[0];
                        string min = outtime[1];
                        if (outtime[0].Length == 1)
                        {
                            hour = "0" + outtime[0];
                        }
                        if (min.Length == 1)
                        {
                            min = "0" + outtime[1];
                        }
                        string[] intime = splitedintime.Split(new char[] { ':' });
                        string outhr = intime[0].ToString();
                        string outmm = intime[1].ToString();
                        if (outhr.Length == 1)
                        {
                            outhr = "0" + outhr;
                        }
                        if (outmm.Length == 1)
                        {
                            outmm = "0" + outmm;
                        }
                        ddlOutTimeHr.Enabled = false;
                        ddlOutTimeMM.Enabled = false;
                        ddlOutTimeSess.Enabled = false;
                        ddlOutTimeHr.Text = hour;
                        ddlOutTimeMM.Text = min;
                        ddlOutTimeSess.Text = splitedoutmeridian;
                        ddlInTimeHr.Enabled = false;
                        ddlInTimeMM.Enabled = false;
                        ddlInTimeSess.Enabled = false;
                        ddlInTimeHr.Text = outhr;
                        ddlInTimeMM.Text = outmm;
                        ddlInTimeSess.Text = splitedinmeridian;
                        purpose = da.GetFunction("select textcode from TextValTable where TextCriteria='Attrs' and textval='" + purpose + "' and college_code='" + collegeCode + "'");
                        if (purpose.Trim() != "" && purpose.Trim() != "0")
                        {
                            if (ddlPurpose.Items.Count > 0)
                            {
                                ddlPurpose.SelectedValue = purpose;
                            }
                        }
                        BindAttendanceRights();
                        ddlAttendanceOption.Enabled = false;
                        if (ddlAttendanceOption.Items.Count > 0)
                        {
                            ListItem list = new ListItem(attedancetype.Trim().ToUpper(), attedancetype.Trim().ToUpper());
                            if (ddlAttendanceOption.Items.Contains(list))
                            {
                                ddlAttendanceOption.Text = attedancetype;
                            }
                        }
                        btnPopSaveOD.Enabled = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnOnDutyUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            lblPopODErr.Text = string.Empty;
            lblPopODErr.Visible = false;
            divPopODAlert.Visible = false;
            chkStudentWise.Checked = false;
            divAddStudents.Visible = false;
            divSearchAllStudents.Visible = true;
            btnRemoveOdStudents.Visible = false;
            FpShowODDetails.SaveChanges();

            bool isSelected = false;
            int count = 0;
            for (int res = 1; res < FpShowODDetails.Sheets[0].RowCount; res++)
            {
                int isval = 0;
                isval = Convert.ToInt32(FpShowODDetails.Sheets[0].Cells[res, 16].Value);
                if (isval == 1)
                {
                    count++;
                    isSelected = true;
                }
            }
            if (count > 1)
            {
                lblAlertMsg.Text = "Please Select Only One Record To Update.";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            if (isSelected)
            {
                lblSaveorDeleteShow.Text = "1";
                lblConfirmMsgShow.Text = "Do You Want To Update OD Details?";
                divConfirmShow.Visible = true;
                return;
            }
            else
            {
                lblAlertMsg.Text = "Please Select Any One Record To Update";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Update OnDuty Details

    #region Delete OnDuty Details

    protected void btnOnDutyDelete_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirmShow.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblPopODErr.Text = string.Empty;
            lblPopODErr.Visible = false;
            bool isSelected = false;
            for (int res = 1; res < FpShowODDetails.Sheets[0].RowCount; res++)
            {
                int isval = 0;
                isval = Convert.ToInt32(FpShowODDetails.Sheets[0].Cells[res, 16].Value);
                if (isval == 1)
                {
                    isSelected = true;
                }
            }
            if (isSelected)
            {
                lblSaveorDeleteShow.Text = "2";
                lblConfirmMsgShow.Text = "Do You Want To Delete OD Details?";
                divConfirmShow.Visible = true;
                return;
            }
            else
            {
                //lblAlertMsg.Text = "Deleted Successfully";
                lblAlertMsg.Text = "Please Select Atleast One Record To Delete";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void deleteODDetails()
    {
        try
        {
            int savevalue = 0;
            bool isDeleteSucc = false;
            string dt = string.Empty;
            FpShowODDetails.SaveChanges();
            for (int res = 1; res < FpShowODDetails.Sheets[0].RowCount; res++)
            {
                int isval = 0;
                isval = Convert.ToInt32(FpShowODDetails.Sheets[0].Cells[res, 16].Value);
                if (isval == 1)
                {
                    string ar = Convert.ToString(res).Trim();
                    string appNo = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(ar), 0].Tag).Trim();
                    string degreecode = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(ar), 0].Note).Trim();
                    string stdRollno = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(ar), 1].Text).Trim();
                    string hours = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(ar), 11].Tag).Trim();
                    string getdate = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(ar), 9].Text).Trim();
                    string collegeCode = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(ar), 1].Tag).Trim();
                    string semesterNew = Convert.ToString(FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(ar), 1].Note).Trim();
                    string[] spdate = getdate.Split('/');
                    string frdate = spdate[1] + '/' + spdate[0] + '/' + spdate[2];
                    string deleteentry = "delete from onduty_stud where roll_no='" + stdRollno + "' and fromdate='" + frdate + "' and hourse='" + hours + "'";
                    savevalue = da.update_method_wo_parameter(deleteentry, "Text");
                    if (savevalue != 0)
                    {
                        isDeleteSucc = true;
                    }
                    fromDate = getdate;
                    toDate = FpShowODDetails.Sheets[0].Cells[Convert.ToInt32(ar), 10].Text.ToString();
                    dt = fromDate;
                    string[] dsplit = dt.Split(new Char[] { '/' });
                    frdate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                    int demfcal = int.Parse(dsplit[2].ToString());
                    demfcal = demfcal * 12;
                    int cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                    string monthcal = cal_from_date.ToString();
                    dt = toDate;
                    dsplit = dt.Split(new Char[] { '/' });
                    toDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                    int demtcal = int.Parse(dsplit[2].ToString());
                    demtcal = demfcal * 12;
                    int cal_to_date = demfcal + int.Parse(dsplit[1].ToString());
                    DateTime per_from_date = Convert.ToDateTime(frdate);
                    DateTime per_to_date = Convert.ToDateTime(toDate);
                    DateTime dumm_from_date = per_from_date;
                    string reason = string.Empty;
                    ht.Clear();
                    ht.Add("degree_code", int.Parse(degreecode));
                    ht.Add("sem", int.Parse(semesterNew));
                    ht.Add("from_date", frdate.ToString());
                    ht.Add("to_date", toDate.ToString());
                    ht.Add("coll_code", int.Parse(collegeCode));
                    int iscount = 0;
                    string strquery = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate.ToString() + "' and '" + toDate.ToString() + "' and degree_code='" + degreecode + "' and semester='" + semesterNew + "'";
                    DataSet ds_holi = new DataSet();
                    ds_holi.Reset();
                    ds_holi.Dispose();
                    ds_holi = da.select_method(strquery, ht, "Text");
                    if (ds_holi.Tables.Count > 0 && ds_holi.Tables[0].Rows.Count > 0)
                    {
                        iscount = Convert.ToInt16(ds_holi.Tables[0].Rows[0]["cnt"].ToString());
                    }
                    ht.Add("iscount", iscount);
                    ds_holi = da.select_method("ALL_HOLIDATE_DETAILS", ht, "sp");
                    Hashtable holiday_table = new Hashtable();
                    holiday_table.Clear();
                    if (ds_holi.Tables.Count > 0 && ds_holi.Tables[0].Rows.Count != 0)
                    {
                        for (int k = 0; k < ds_holi.Tables[0].Rows.Count; k++)
                        {
                            if (!holiday_table.ContainsKey(Convert.ToDateTime(ds_holi.Tables[0].Rows[k]["HOLI_DATE"].ToString())))
                            {
                                holiday_table.Add(Convert.ToDateTime(ds_holi.Tables[0].Rows[k]["HOLI_DATE"].ToString()), k);
                            }
                        }
                    }
                    if (ds_holi.Tables.Count > 1 && ds_holi.Tables[1].Rows.Count != 0)
                    {
                        for (int k = 0; k < ds_holi.Tables[1].Rows.Count; k++)
                        {
                            if (!holiday_table.ContainsKey(Convert.ToDateTime(ds_holi.Tables[1].Rows[k]["HOLI_DATE"].ToString())))
                            {
                                holiday_table.Add(Convert.ToDateTime(ds_holi.Tables[1].Rows[k]["HOLI_DATE"].ToString()), k);
                            }
                        }
                    }
                    if (ds_holi.Tables.Count > 2 && ds_holi.Tables[2].Rows.Count != 0)
                    {
                        for (int k = 0; k < ds_holi.Tables[2].Rows.Count; k++)
                        {
                            if (!holiday_table.ContainsKey(Convert.ToDateTime(ds_holi.Tables[2].Rows[k]["HOLI_DATE"].ToString())))
                            {
                                holiday_table.Add(Convert.ToDateTime(ds_holi.Tables[2].Rows[k]["HOLI_DATE"].ToString()), k);
                            }
                        }
                    }
                    Dictionary<string, StringBuilder[]> dicQueryValue = new Dictionary<string, StringBuilder[]>();
                    if (dumm_from_date <= per_to_date)
                    {
                        while (dumm_from_date <= per_to_date)
                        {
                            StringBuilder sbQueryUpdate = new StringBuilder();
                            StringBuilder sbQUeryInsertValue = new StringBuilder();
                            StringBuilder sbQueryColumnName = new StringBuilder();
                            int monthyear = 0;
                            if (!holiday_table.ContainsKey(dumm_from_date))
                            {
                                string dummfromdate = Convert.ToString(dumm_from_date);
                                string[] fromdate1 = dummfromdate.Split(new char[] { ' ' });
                                string fromdate2 = fromdate1[0].ToString();
                                string[] fromdate = fromdate2.Split(new char[] { '/' });
                                string fromdatedate = fromdate[1].ToString();
                                string fromdatemonth = fromdate[0].ToString();
                                string fromdateyear = fromdate[2].ToString();
                                monthyear = Convert.ToInt32(fromdatemonth) + Convert.ToInt32(fromdateyear) * 12;
                                string valueupdate = string.Empty;
                                string insertvalue = string.Empty;
                                string odvalue = string.Empty;
                                int totnoofhours = 0;
                                string[] hourslimit = hours.Split(new char[] { ',' });
                                totnoofhours = Convert.ToInt32(hourslimit.GetUpperBound(0).ToString()) + 1;
                                int taken_hourse = 0;
                                taken_hourse = taken_hourse + totnoofhours;
                                for (int i = 0; i < Convert.ToInt32(totnoofhours); i++)
                                {
                                    string particularhrs = hourslimit[i].ToString();
                                    string value = ("d" + fromdatedate + "d" + particularhrs);
                                    string reval = value;
                                    string attValue = "''";
                                    value = value + "=''";
                                    if (valueupdate == "")
                                    {
                                        valueupdate = reval + "=" + attValue;
                                    }
                                    else
                                    {
                                        valueupdate = valueupdate + "," + reval + "=" + attValue;
                                    }
                                    if (insertvalue == "")
                                    {
                                        insertvalue = reval;
                                    }
                                    else
                                    {
                                        insertvalue = insertvalue + "," + reval;
                                    }
                                    if (odvalue == "")
                                    {
                                        odvalue = attValue;
                                    }
                                    else
                                    {
                                        odvalue = odvalue + "," + attValue;
                                    }
                                    //string updateattend = "update Attendance set " + value + " where  Roll_no='" + stdRollno + "' and month_year=" + monthyear + "";
                                    //int save = da.update_method_wo_parameter(updateattend, "Text");
                                    //ht.Clear();
                                    //ht.Add("AtWr_App_no", appNo);
                                    //ht.Add("AttWr_CollegeCode", collegeCode);
                                    //ht.Add("columnname", reval);
                                    //ht.Add("roll_no", stdRollno);
                                    //ht.Add("month_year", monthyear);
                                    //ht.Add("values", reason);
                                    //strquery = "sp_ins_upd_student_attendance_reason";
                                    //int insert = da.insert_method(strquery, ht, "sp");
                                    //if (insert != 0)
                                    //{
                                    //    isDeleteSucc = true;
                                    //}
                                }
                                if (!string.IsNullOrEmpty(insertvalue))
                                {
                                    sbQueryColumnName.Append(insertvalue + ",");
                                }
                                if (!string.IsNullOrEmpty(odvalue))
                                {
                                    sbQUeryInsertValue.Append(odvalue + ",");
                                }
                                if (!string.IsNullOrEmpty(valueupdate))
                                {
                                    sbQueryUpdate.Append(valueupdate + ",");
                                }
                            }
                            StringBuilder[] sbAll = new StringBuilder[3];
                            if (!string.IsNullOrEmpty(sbQueryColumnName.ToString().Trim()) && !string.IsNullOrEmpty(sbQUeryInsertValue.ToString().Trim()) && !string.IsNullOrEmpty(sbQueryUpdate.ToString().Trim()))
                            {
                                if (dicQueryValue.ContainsKey(monthyear.ToString().Trim()))
                                {
                                    sbAll = dicQueryValue[monthyear.ToString().Trim()];
                                    sbAll[0].Append(sbQueryColumnName);
                                    sbAll[1].Append(sbQUeryInsertValue);
                                    sbAll[2].Append(sbQueryUpdate);
                                    dicQueryValue[monthyear.ToString().Trim()] = sbAll;
                                }
                                else if (monthyear != 0)
                                {
                                    sbAll[0] = new StringBuilder();
                                    sbAll[1] = new StringBuilder();
                                    sbAll[2] = new StringBuilder();
                                    sbAll[0].Append(Convert.ToString(sbQueryColumnName));
                                    sbAll[1].Append(Convert.ToString(sbQUeryInsertValue));
                                    sbAll[2].Append(Convert.ToString(sbQueryUpdate));
                                    dicQueryValue.Add(monthyear.ToString().Trim(), sbAll);
                                }
                            }
                            dumm_from_date = dumm_from_date.AddDays(1);
                        }
                        if (dicQueryValue.Count > 0)
                        {
                            StringBuilder[] spAll = new StringBuilder[3];
                            foreach (KeyValuePair<string, StringBuilder[]> dicQueery in dicQueryValue)
                            {
                                spAll = new StringBuilder[3];
                                string monthValue = dicQueery.Key;
                                spAll = dicQueery.Value;
                                string insertColumnName = spAll[0].ToString().Trim(',');
                                string insertColumnValue = spAll[1].ToString().Trim(',');
                                string updateColumnNameValue = spAll[2].ToString().Trim(',');
                                string[] splitColumn = insertColumnName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string sp in splitColumn)
                                {
                                    ht.Clear();
                                    ht.Add("AtWr_App_no", appNo);
                                    ht.Add("AttWr_CollegeCode", collegeCode);
                                    ht.Add("columnname", sp);
                                    ht.Add("roll_no", stdRollno);
                                    ht.Add("month_year", monthValue);
                                    ht.Add("values", reason);
                                    strquery = "sp_ins_upd_student_attendance_reason";
                                    int insert = da.insert_method(strquery, ht, "sp");
                                    if (savevalue != 0)
                                    {
                                        isDeleteSucc = true;
                                    }
                                }
                                ht.Clear();
                                ht.Add("Att_App_no", appNo);
                                ht.Add("Att_CollegeCode", collegeCode);
                                ht.Add("rollno", stdRollno);
                                ht.Add("monthyear", monthValue);
                                ht.Add("columnname", insertColumnName);
                                ht.Add("colvalues", insertColumnValue);
                                ht.Add("coulmnvalue", updateColumnNameValue);
                                savevalue = da.insert_method("sp_ins_upd_student_attendance_Dead", ht, "sp");
                                if (savevalue != 0)
                                {
                                    isDeleteSucc = true;
                                }
                            }
                        }
                    }
                }
            }
            btnOnDutyDelete.Visible = false;
            btnOnDutyUpdate.Visible = false;
            btnGo_Click(new Object(), new EventArgs());
            if (isDeleteSucc)
            {
                lblAlertMsg.Text = "Deleted Successfully";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            else
            {
                lblAlertMsg.Text = "Not Deleted";
                lblAlertMsg.Visible = true;
                divPopAlert.Visible = true;
                return;
            }
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
        }
        catch
        {

        }
    }

    #endregion Delete OnDuty Details

    #endregion Button Events

    #region OnDuty Details

    #region Bind Header

    public void BindCollegeOD()
    {
        try
        {
            ddlCollegeOD.Items.Clear();
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
            ddlCollegeOD.Items.Clear();
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                ddlCollegeOD.DataSource = dsprint;
                ddlCollegeOD.DataTextField = "collname";
                ddlCollegeOD.DataValueField = "college_code";
                ddlCollegeOD.DataBind();
                ddlCollegeOD.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindBatchOD()
    {
        try
        {
            qryUserOrGroupCode = string.Empty;
            groupUserCode = string.Empty;
            collegeCode = string.Empty;
            userCode = string.Empty;
            ddlBatchOD.Items.Clear();
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
            if (ddlCollegeOD.Items.Count > 0)
            {
                //collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                collegeCode = string.Empty;
                foreach (ListItem li in ddlCollegeOD.Items)
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
                ddlBatchOD.DataSource = ds;
                ddlBatchOD.DataTextField = "Batch_year";
                ddlBatchOD.DataValueField = "Batch_year";
                ddlBatchOD.DataBind();
                ddlBatchOD.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindDegreeOD()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ddlDegreeOD.Items.Clear();
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
            if (ddlCollegeOD.Items.Count > 0)
            {
                collegeCode = string.Empty;
                foreach (ListItem li in ddlCollegeOD.Items)
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
                    qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
                }
            }
            if (ddlBatchOD.Items.Count > 0 && ddlBatchOD.Visible == true)
            {
                foreach (ListItem li in ddlBatchOD.Items)
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
                    qryBatchYear = " and r.Batch_year in(" + batchYear + ")";
                }
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                ds = da.select_method_wo_parameter("select distinct dg.course_id,c.course_name,c.Priority from Degree dg,Course c,Department dt,DeptPrivilages dp where  dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code " + qryCollegeCode + qryUserOrGroupCode + "  order by c.Priority", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlDegreeOD.DataSource = ds;
                    ddlDegreeOD.DataTextField = "course_name";
                    ddlDegreeOD.DataValueField = "course_id";
                    ddlDegreeOD.DataBind();
                    ddlDegreeOD.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindBranchOD()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ddlBranchOD.Items.Clear();
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
            if (ddlCollegeOD.Items.Count > 0)
            {
                collegeCode = string.Empty;
                foreach (ListItem li in ddlCollegeOD.Items)
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
                    qryCollegeCode = " and dg.college_code in(" + collegeCode + ")";
                }
            }
            if (ddlBatchOD.Items.Count > 0 && ddlBatchOD.Visible == true)
            {
                foreach (ListItem li in ddlBatchOD.Items)
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
                    qryBatchYear = " and r.Batch_year in(" + batchYear + ")";
                }
            }
            if (ddlDegreeOD.Items.Count > 0)
            {
                foreach (ListItem li in ddlDegreeOD.Items)
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
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(qryCourseId) && !string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                ds = da.select_method_wo_parameter("select distinct dg.Degree_Code,dt.Dept_Name from Degree dg,Course c,Department dt,DeptPrivilages dp where  dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code " + qryCourseId + qryCollegeCode + qryUserOrGroupCode + "order by dg.Degree_Code", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlBranchOD.DataSource = ds;
                    ddlBranchOD.DataTextField = "dept_name";
                    ddlBranchOD.DataValueField = "degree_code";
                    ddlBranchOD.DataBind();
                    ddlBranchOD.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindSemOD()
    {
        try
        {
            ds.Clear();
            ddlSemOD.Items.Clear();
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
            if (ddlCollegeOD.Items.Count > 0)
            {
                //collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                collegeCode = string.Empty;
                foreach (ListItem li in ddlCollegeOD.Items)
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
                    qryCollegeCode = " and college_code in(" + collegeCode + ")";
                }
            }
            if (ddlBatchOD.Items.Count > 0 && ddlBatchOD.Visible == true)
            {
                batchYear = string.Empty;
                foreach (ListItem li in ddlBatchOD.Items)
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
                    qryBatchYear = " and Batch_year in(" + batchYear + ")";
                }
            }
            if (ddlBranchOD.Items.Count > 0)
            {
                foreach (ListItem li in ddlBranchOD.Items)
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
            if (!string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryBatchYear))
            {
                sqlnew = "select distinct max(ndurations) ndurations,first_year_nonsemester from ndegree dg where ndurations<>'0'" + qryDegreeCode + qryCollegeCode + qryBatchYear + " group by first_year_nonsemester";
                ds.Clear();
                ds = da.select_method_wo_parameter(sqlnew, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSemOD.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSemOD.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                    }
                }
                ddlSemOD.SelectedIndex = 0;
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
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlSemOD.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlSemOD.Items.Add(new ListItem(Convert.ToString(i).Trim(), Convert.ToString(i).Trim()));
                        }
                    }
                    ddlSemOD.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindSectionDetailOD()
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
            ddlSecOD.Items.Clear();
            ddlSecOD.Enabled = false;
            if (ddlCollegeOD.Items.Count > 0)
            {                //collegeCode = Convert.ToString(ddlCollegeOD.SelectedValue).Trim();
                collegeCode = string.Empty;
                foreach (ListItem li in ddlCollegeOD.Items)
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
                //if (!string.IsNullOrEmpty(collegeCode))
                //{
                //    qryCollegeCode = " and college_code in(" + collegeCode + ")";
                //}
            }
            if (ddlBatchOD.Items.Count > 0)
            {
                //batchYear = Convert.ToString(ddlBatchOD.SelectedValue).Trim();
                batchYear = string.Empty;
                foreach (ListItem li in ddlBatchOD.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Value + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Value + "'";
                        }
                    }
                }
                //if (!string.IsNullOrEmpty(batchYear))
                //{
                //    qryBatchYear = " and batch_year in(" + batchYear + ")";
                //}
            }
            if (ddlBranchOD.Items.Count > 0)
            {
                //degreeCode = Convert.ToString(ddlBranchOD.SelectedValue).Trim();
                degreeCode = string.Empty;
                foreach (ListItem li in ddlBranchOD.Items)
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
                    string sqlnew = "select distinct sections from registration where batch_year in(" + Convert.ToString(batchYear).Trim() + ") and degree_code in (" + Convert.ToString(degreeCode).Trim() + ") and sections<>'-1' and sections<>' ' and college_code in(" + Convert.ToString(collegeCode).Trim() + ") and sections in(" + sections + ") and delflag='0' and cc='0' and exam_flag<>'Debar' order by sections";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(sqlnew, "Text");
                }
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSecOD.DataSource = ds;
                ddlSecOD.DataTextField = "sections";
                ddlSecOD.DataValueField = "sections";
                ddlSecOD.DataBind();
                //ddlSec.Items.Insert(0, "All");
                ddlSecOD.Enabled = true;
            }
            else
            {
                ddlSecOD.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void BindHour()
    {
        try
        {
            ddlInTimeHr.Items.Clear();
            ddlOutTimeHr.Items.Clear();
            for (int hr = 0; hr <= 12; hr++)
            {
                ddlInTimeHr.Items.Insert(hr, new ListItem(Convert.ToString(hr).Trim().PadLeft(2, '0'), Convert.ToString(hr).Trim().PadLeft(2, '0')));
                ddlOutTimeHr.Items.Insert(hr, new ListItem(Convert.ToString(hr).Trim().PadLeft(2, '0'), Convert.ToString(hr).Trim().PadLeft(2, '0')));
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void BindMinute()
    {
        try
        {
            ddlInTimeMM.Items.Clear();
            ddlOutTimeMM.Items.Clear();
            int s = 0;
            for (int mm = 0; mm <= 60; mm++)
            {
                ddlInTimeMM.Items.Insert(s, new ListItem(Convert.ToString(mm).Trim().PadLeft(2, '0'), Convert.ToString(mm).Trim().PadLeft(2, '0')));
                ddlOutTimeMM.Items.Insert(s, new ListItem(Convert.ToString(mm).Trim().PadLeft(2, '0'), Convert.ToString(mm).Trim().PadLeft(2, '0')));
                s++;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindReason()
    {
        try
        {
            ddlPurpose.Items.Clear();
            collegeCode = Convert.ToString(ddlCollegeOD.SelectedValue).Trim();
            ds.Dispose();
            ds.Reset();
            ds.Clear();
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qry = "select TextCode,Textval from textvaltable where TextCriteria='Attrs' and college_code='" + collegeCode + "' order by Textval";
                ds = da.select_method_wo_parameter(qry, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlPurpose.DataSource = ds;
                ddlPurpose.DataTextField = "Textval";
                ddlPurpose.DataValueField = "TextCode";
                ddlPurpose.DataBind();
                btnPopSaveOD.Enabled = true;
            }
            else
            {
                btnPopSaveOD.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindAttendanceRights()
    {
        try
        {
            qryUserOrGroupCode = string.Empty;
            groupUserCode = string.Empty;
            userCode = string.Empty;
            ddlAttendanceOption.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds.Clear();
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            if (!string.IsNullOrEmpty(groupUserCode) && Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                qryUserOrGroupCode = " group_code='" + groupUserCode.Trim() + "'";
            }
            else if (Session["usercode"] != null)
            {
                userCode = Convert.ToString(Session["usercode"]).Trim();
                qryUserOrGroupCode = " usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(qryUserOrGroupCode))
            {
                qry = "select distinct rights from OD_Master_Setting where " + qryUserOrGroupCode + "";
                ds = da.select_method_wo_parameter(qry, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Dictionary<string, int> dicAttRights = new Dictionary<string, int>();
                int itemCount = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlAttendanceOption.Enabled = true;
                    string odRights = string.Empty;
                    string temp1 = string.Empty;
                    Hashtable htOD = new Hashtable();
                    odRights = Convert.ToString(ds.Tables[0].Rows[i]["rights"]).Trim();
                    if (odRights != string.Empty)
                    {
                        string[] splitODRights = odRights.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        for (int odTemp = 0; odTemp < splitODRights.Length; odTemp++)
                        {
                            if (!string.IsNullOrEmpty(Convert.ToString(splitODRights[odTemp]).Trim()))
                            {
                                if (!dicAttRights.ContainsKey(Convert.ToString(splitODRights[odTemp]).Trim().ToLower()))
                                {
                                    ddlAttendanceOption.Items.Add(Convert.ToString(splitODRights[odTemp]).Trim());
                                    dicAttRights.Add(Convert.ToString(splitODRights[odTemp]).Trim().ToLower(), 1);
                                    itemCount++;
                                }
                            }
                        }
                    }
                }
                if (ddlAttendanceOption.Items.Count == 0)
                {
                    ddlAttendanceOption.Enabled = false;
                }
                else
                {
                    ddlAttendanceOption.Enabled = true;
                }
            }
            else
            {
                ddlAttendanceOption.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Bind Header

    #region Index Changed Events

    protected void ddlCollegeOD_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindBatchOD();
            BindDegreeOD();
            BindBranchOD();
            BindSemOD();
            BindSectionDetailOD();
            BindReason();
            ShowStudentsList(0);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlBatchOD_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindDegreeOD();
            BindBranchOD();
            BindSemOD();
            BindSectionDetailOD();
            ShowStudentsList(0);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlDegreeOD_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindBranchOD();
            BindSemOD();
            BindSectionDetailOD();
            ShowStudentsList(0);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlBranchOD_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindSemOD();
            BindSectionDetailOD();
            ShowStudentsList(0);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlSemOD_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            BindSectionDetailOD();
            ShowStudentsList(0);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlSecOD_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContents.Visible = false;
            ShowStudentsList(0);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void rbFullDay_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblPopODErr.Visible = false;
            txtNoOfHours.Text = string.Empty;
            divHalfHr.Visible = false;
            rbPM.Checked = false;
            rbAM.Checked = false;
            cblHours.Items.Clear();
            if (htHoursPerDay.Count == 0)
            {
                attendenace("", "", 1);
            }
            if (htHoursPerDay.Contains("full"))
            {
                string get_ful_hr = GetCorrespondingKey("full", htHoursPerDay).ToString();
                for (int x = 1; x <= Convert.ToInt16(get_ful_hr); x++)
                {
                    cblHours.Items.Add("" + x + "");
                    cblHours.Items[x - 1].Selected = true;
                    if (txtNoOfHours.Text == "")
                    {
                        txtNoOfHours.Text = x.ToString();
                    }
                    else
                    {
                        txtNoOfHours.Text = txtNoOfHours.Text + "," + x.ToString();
                    }
                }
                lblNoOfHours.Visible = false;
            }
            else
            {
                lblPopODErr.Visible = true;
                lblPopODErr.Text = "Please Add " + lblSemOD.Text + " Information";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void rbHourWise_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblPopODErr.Visible = false;
            txtNoOfHours.Text = string.Empty;
            cblHours.Items.Clear();
            divHalfHr.Visible = false;
            rbAM.Visible = false;
            rbPM.Visible = false;
            txtNoOfHours.Visible = true;
            lblNoOfHours.Visible = true;
            string tothrs = string.Empty;
            string fsthalfhrs = string.Empty;
            string scndhalfhrs = string.Empty;
            if (htHoursPerDay.Count == 0)
            {
                attendenace("", "", 1);
            }
            foreach (DictionaryEntry parameter1 in htHoursPerDay)
            {
                string daytext = Convert.ToString(parameter1.Key);
                string noofhours = Convert.ToString(parameter1.Value);
                if (daytext == "full")
                {
                    tothrs = noofhours;
                }
                if (daytext == "fn")
                {
                    fsthalfhrs = noofhours;
                }
                if (daytext == "an")
                {
                    scndhalfhrs = noofhours;
                }
            }
            if (tothrs != "" && tothrs != null)
            {
                if (rbHourWise.Checked == true)
                {
                    string temp = string.Empty;
                    for (int fulhrs = 1; fulhrs <= Convert.ToInt32(tothrs); fulhrs++)
                    {
                        cblHours.Items.Add("" + fulhrs + "");
                        //Chkselecthours.Items[fulhrs-1].Selected = true;
                        //if (temp == "")
                        //{
                        //    txtselecthours.Text = (fulhrs).ToString();
                        //    temp = (fulhrs).ToString();
                        //}
                        //else
                        //{
                        //    txtselecthours.Text = txtselecthours.Text + "," + (fulhrs).ToString();
                        //}
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void rbHalfDay_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblPopODErr.Visible = false;
            txtNoOfHours.Text = string.Empty;
            cblHours.Items.Clear();
            divHalfHr.Visible = true;
            rbAM.Visible = true;
            rbPM.Visible = true;
            lblNoOfHours.Visible = true;
            int tothrs = 0;
            int fsthalfhrs = 0;
            int scndhalfhrs = 0;
            txtNoOfHours.Visible = true;
            if (htHoursPerDay.Count == 0)
            {
                attendenace("", "", 1);
            }
            foreach (DictionaryEntry parameter1 in htHoursPerDay)
            {
                string daytext = Convert.ToString(parameter1.Key);
                int noofhours = Convert.ToInt32(parameter1.Value);
                if (daytext == "full")
                {
                    tothrs = noofhours;
                }
                if (daytext == "fn")
                {
                    fsthalfhrs = noofhours;
                }
                if (daytext == "an")
                {
                    scndhalfhrs = noofhours;
                }
            }
            if (rbHalfDay.Checked == true)
            {
                if (rbAM.Checked == true)
                {
                    string temp = string.Empty;
                    for (int fsthrs = 1; fsthrs <= fsthalfhrs; fsthrs++)
                    {
                        cblHours.Items.Add("" + fsthrs + "");
                        cblHours.Items[fsthrs - 1].Selected = true;
                        if (temp == "")
                        {
                            txtNoOfHours.Text = (fsthrs).ToString();
                            temp = (fsthrs).ToString();
                        }
                        else
                        {
                            txtNoOfHours.Text = txtNoOfHours.Text + "," + (fsthrs).ToString();
                        }
                    }
                }
                else if (rbPM.Checked == true)
                {
                    string temp = string.Empty;
                    for (int scdhrs = fsthalfhrs + 1; scdhrs <= Convert.ToInt32(fsthalfhrs + scndhalfhrs); scdhrs++)
                    {
                        cblHours.Items.Add("" + scdhrs + "");
                        if (temp == "")
                        {
                            txtNoOfHours.Text = (scdhrs).ToString();
                            temp = (scdhrs).ToString();
                        }
                        else
                        {
                            txtNoOfHours.Text = txtNoOfHours.Text + "," + (scdhrs).ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void rbAM_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblPopODErr.Visible = false;
            cblHours.Items.Clear();
            txtNoOfHours.Text = string.Empty;
            int tothrs = 0;
            int fsthalfhrs = 0;
            int scndhalfhrs = 0;
            if (htHoursPerDay.Count == 0)
            {
                attendenace("", "", 1);
            }
            foreach (DictionaryEntry parameter1 in htHoursPerDay)
            {
                string daytext = Convert.ToString(parameter1.Key);
                int noofhours = Convert.ToInt32(parameter1.Value);
                if (daytext == "full")
                {
                    tothrs = noofhours;
                }
                if (daytext == "fn")
                {
                    fsthalfhrs = noofhours;
                }
                if (daytext == "an")
                {
                    scndhalfhrs = noofhours;
                }
            }
            if (rbAM.Checked == true)
            {
                string temp = string.Empty;
                for (int fsthrs = 1; fsthrs <= Convert.ToInt32(fsthalfhrs); fsthrs++)
                {
                    cblHours.Items.Add("" + fsthrs + "");
                    cblHours.Items[fsthrs - 1].Selected = true;
                    if (temp == "")
                    {
                        txtNoOfHours.Text = (fsthrs).ToString();
                        temp = (fsthrs).ToString();
                    }
                    else
                    {
                        txtNoOfHours.Text = txtNoOfHours.Text + "," + (fsthrs).ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void rbPM_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblPopODErr.Visible = false;
            cblHours.Items.Clear();
            txtNoOfHours.Text = string.Empty;
            int tothrs = 0;
            int fsthalfhrs = 0;
            int scndhalfhrs = 0;
            if (htHoursPerDay.Count == 0)
            {
                attendenace("", "", 1);
            }
            foreach (DictionaryEntry parameter1 in htHoursPerDay)
            {
                string daytext = Convert.ToString(parameter1.Key);
                int noofhours = Convert.ToInt32(parameter1.Value);
                if (daytext == "full")
                {
                    tothrs = noofhours;
                }
                if (daytext == "fn")
                {
                    fsthalfhrs = noofhours;
                }
                if (daytext == "an")
                {
                    scndhalfhrs = noofhours;
                }
            }// panelselecthours.Visible = true;
            if (rbPM.Checked == true)
            {
                string temp = string.Empty;
                for (int scdhrs = fsthalfhrs + 1; scdhrs <= Convert.ToInt32(scndhalfhrs + fsthalfhrs); scdhrs++)
                {
                    cblHours.Items.Add("" + scdhrs + "");
                    cblHours.Items[scdhrs - (fsthalfhrs + 1)].Selected = true;
                    if (temp == "")
                    {
                        txtNoOfHours.Text = (scdhrs).ToString();
                        temp = scdhrs.ToString();
                    }
                    else
                    {
                        txtNoOfHours.Text = txtNoOfHours.Text + "," + (scdhrs).ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void txtNoOfHours_TextChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblHours_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblPopODErr.Visible = false;
            txtNoOfHours.Text = string.Empty;
            string value = string.Empty;
            for (int i = 0; i < cblHours.Items.Count; i++)
            {
                if (cblHours.Items[i].Selected == true)
                {
                    value = cblHours.Items[i].Text;
                    if (txtNoOfHours.Text == "")
                    {
                        txtNoOfHours.Text = value;
                    }
                    else
                    {
                        txtNoOfHours.Text = txtNoOfHours.Text + "," + value;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Index Changed Events

    protected void chkStudentWise_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txtStudent.Text = string.Empty;
            divAddStudents.Visible = false;
            divSearchAllStudents.Visible = true;
            btnRemoveOdStudents.Visible = false;
            txtFromDateOD.Attributes.Add("readonly", "readonly");
            txtFromDateOD.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDateOD.Attributes.Add("readonly", "readonly");
            txtToDateOD.Text = DateTime.Now.ToString("dd/MM/yyyy");
            BindHour();
            BindMinute();
            if (chkStudentWise.Checked)
            {
                divSearchAllStudents.Visible = false;
                divAddStudents.Visible = true;
                btnRemoveOdStudents.Visible = true;
                txtStudent.Focus();
                SetStudentWiseSettings();
                Init_Spread(FpStudentDetails, 1);
                //ShowStudentsList(1);
            }
            else
            {
                divSearchAllStudents.Visible = true;
                divAddStudents.Visible = false;
                btnRemoveOdStudents.Visible = false;
                BindCollegeOD();
                BindBatchOD();
                BindDegreeOD();
                BindBranchOD();
                BindSemOD();
                BindSectionDetailOD();
                Init_Spread(FpStudentDetails, 1);
                ShowStudentsList(0);
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void btnAddStudent_Click(object sender, EventArgs e)
    {
        try
        {
            ShowStudentsList(1);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void FpStudentDetails_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = e.SheetView.ActiveRow.ToString();
            if (flag_true == false && actrow == "0")
            {
                for (int j = 1; j < Convert.ToInt16(FpStudentDetails.Sheets[0].RowCount); j++)
                {
                    string actcol = e.SheetView.ActiveColumn.ToString();
                    string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                    if (seltext != "System.Object")
                        FpStudentDetails.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
                }
                flag_true = true;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlSearchBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtStudent.Text = string.Empty;
            if (ddlSearchBy.Items.Count > 0)
            {
                lblStudentOptions.Text = ddlSearchBy.SelectedItem.Text;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void btnRemoveOdStudents_Click(object sender, EventArgs e)
    {
        try
        {
            lblPopODErr.Text = string.Empty;
            divPopODAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            FpStudentDetails.SaveChanges();
            Farpoint.CheckBoxCellType chkAll = new Farpoint.CheckBoxCellType();
            chkAll.AutoPostBack = true;
            Farpoint.CheckBoxCellType chkSingleCell = new Farpoint.CheckBoxCellType();
            Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("Batch");
            dt.Columns.Add("RollNo");
            dt.Columns.Add("DegreeCode");
            dt.Columns.Add("RegNo");
            dt.Columns.Add("Section");
            dt.Columns.Add("StudentName");
            dt.Columns.Add("Semester");
            dt.Columns.Add("AdmissionNo");
            dt.Columns.Add("AppNo");
            dt.Columns.Add("CollegeCode");
            dt.Columns.Add("courseId");
            if (FpStudentDetails.Sheets[0].RowCount > 1)
            {
                int count = 0;
                bool isremove = false;
                for (int row = 1; row < FpStudentDetails.Sheets[0].RowCount; row++)
                {
                    if (Convert.ToInt16(FpStudentDetails.Sheets[0].Cells[row, 6].Value) == 1)
                    {
                        count++;
                        isremove = true;
                    }
                    else
                    {
                        string batchYearNew = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 0].Tag).Trim();
                        string collegeCodeNew = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 0].Note).Trim();
                        string rollno = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 1].Text).Trim();
                        string degreeCodeNew = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 1].Tag).Trim();
                        string regno = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 2].Text).Trim();
                        string sectionNew = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 2].Tag).Trim();
                        string courseID = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 2].Note).Trim();
                        string admissionno = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 3].Text).Trim();
                        string studname = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 4].Text).Trim();
                        string semesterNew = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 5].Text).Trim();
                        string appNo = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 5].Tag).Trim();
                        dr = dt.NewRow();
                        dr["Batch"] = batchYearNew;
                        dr["RollNo"] = rollno;
                        dr["RegNo"] = regno;
                        dr["DegreeCode"] = degreeCodeNew;
                        dr["Section"] = sectionNew;
                        dr["StudentName"] = studname;
                        dr["Semester"] = semesterNew;
                        dr["AdmissionNo"] = admissionno;
                        dr["AppNo"] = appNo;
                        dr["CollegeCode"] = collegeCodeNew;
                        dr["courseId"] = courseID;
                        dt.Rows.Add(dr);
                    }
                }
                if (isremove)
                {
                    Init_Spread(FpStudentDetails, 1);
                    int serialNo = 0;
                    FpStudentDetails.Sheets[0].RowCount = 1;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].CellType = chkAll;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].Locked = false;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    FpStudentDetails.Sheets[0].AutoPostBack = false;
                    foreach (DataRow drStudents in dt.Rows)
                    {
                        serialNo++;
                        string batchYearNew = Convert.ToString(drStudents["Batch"]).Trim();
                        string rollno = Convert.ToString(drStudents["RollNo"]).Trim();
                        string degreeCodeNew = Convert.ToString(drStudents["DegreeCode"]).Trim();
                        string regno = Convert.ToString(drStudents["RegNo"]).Trim();
                        string sectionNew = Convert.ToString(drStudents["Section"]).Trim();
                        string studname = Convert.ToString(drStudents["StudentName"]).Trim();
                        string semesterNew = Convert.ToString(drStudents["Semester"]).Trim();
                        string admissionno = Convert.ToString(drStudents["AdmissionNo"]);
                        string appNo = Convert.ToString(drStudents["appNo"]).Trim();
                        string collegeCodeNew = Convert.ToString(drStudents["CollegeCode"]).Trim();
                        string courseID = Convert.ToString(drStudents["courseId"]).Trim();
                        FpStudentDetails.Sheets[0].RowCount++;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialNo).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(batchYearNew).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(collegeCodeNew).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Locked = true;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(rollno).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(degreeCodeNew).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Value = Convert.ToString(rollno).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Locked = true;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(regno).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(sectionNew).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(courseID).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(admissionno).Trim();
                        //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(sectionNew).Trim();
                        //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(courseId).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(studname).Trim();
                        //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(sectionNew).Trim();
                        //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].Note = Convert.ToString(courseId).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].Locked = true;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(semesterNew).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(appNo).Trim();
                        //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Note = Convert.ToString(courseId).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].CellType = txtCell;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Locked = true;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].CellType = chkSingleCell;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].Locked = false;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                    }
                    txtStudent.Text = string.Empty;
                    txtStudent.Focus();
                    FpStudentDetails.Sheets[0].PageSize = FpStudentDetails.Sheets[0].RowCount;
                    //FpStudentDetails.Width = 880;
                    FpStudentDetails.Height = 300;
                    FpStudentDetails.SaveChanges();
                }
                if (count == 0)
                {
                    divPopODAlert.Visible = true;
                    lblPopODErr.Text = "Please Select Atleast One Student";
                    lblPopODErr.Visible = true;
                    return;
                }
            }
            else
            {
                Init_Spread(FpStudentDetails, 1);
                FpStudentDetails.Sheets[0].RowCount = 1;
                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].CellType = chkAll;
                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].Locked = false;
                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                FpStudentDetails.SaveChanges();
                divPopODAlert.Visible = true;
                lblPopODErr.Text = "No Student Were Found";
                lblPopODErr.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void SetStudentWiseSettings()
    {
        try
        {
            DataSet dsSearchBy = new DataSet();
            dsSearchBy = GetSettings();
            divSearchBy.Visible = false;
            if (dsSearchBy.Tables.Count > 0 && dsSearchBy.Tables[0].Rows.Count > 0)
            {
                ddlSearchBy.DataSource = dsSearchBy;
                ddlSearchBy.DataTextField = "settings";
                ddlSearchBy.DataValueField = "SetValue";
                ddlSearchBy.DataBind();
                divSearchBy.Visible = true;
                ddlSearchBy.SelectedIndex = 0;
                if (lbl_clgT.Text.Trim().ToUpper() == "SCHOOL")
                {
                    foreach (System.Web.UI.WebControls.ListItem li in ddlSearchBy.Items)
                    {
                        if (li.Text.Trim().ToLower().Contains("admission no"))
                        {
                            ddlSearchBy.SelectedValue = li.Value;
                        }
                    }
                }
                else
                {
                    foreach (System.Web.UI.WebControls.ListItem li in ddlSearchBy.Items)
                    {
                        if (li.Text.Trim().ToLower().Contains("roll no"))
                        {
                            ddlSearchBy.SelectedValue = li.Value;
                        }
                    }
                }
                lblStudentOptions.Text = ddlSearchBy.SelectedItem.Text;
                if (dsSearchBy.Tables[0].Rows.Count == 1)
                {
                    ddlSearchBy.Enabled = false;
                }
            }
            else
            {
                divSearchBy.Visible = false;
                if (lbl_clgT.Text.Trim().ToUpper() == "SCHOOL")
                {
                    lblStudentOptions.Text = "Admission No";
                }
                else
                {
                    lblStudentOptions.Text = "Roll_no";
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void SetDefaultODEntry()
    {
        try
        {
            lblPopODErr.Text = string.Empty;
            lblPopODErr.Visible = false;
            divAddStudents.Visible = false;
            divSearchAllStudents.Visible = true;
            btnRemoveOdStudents.Visible = false;
            chkStudentWise.Checked = false;

            divHalfHr.Visible = false;
            rbFullDay.Checked = false;
            rbHalfDay.Checked = false;
            rbHourWise.Checked = false;
            txtNoOfHours.Text = string.Empty;
            txtStudent.Text = string.Empty;
            chkIncludeSplHrs.Checked = false;

            ddlInTimeHr.SelectedValue = DateTime.Now.ToString("hh");
            ddlInTimeMM.SelectedValue = DateTime.Now.ToString("mm");
            ddlInTimeSess.SelectedValue = DateTime.Now.ToString("tt");

            ddlOutTimeHr.SelectedValue = DateTime.Now.ToString("hh");
            ddlOutTimeMM.SelectedValue = DateTime.Now.ToString("mm");
            ddlOutTimeSess.SelectedValue = DateTime.Now.ToString("tt");

            txtFromDateOD.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtToDateOD.Text = DateTime.Now.ToString("dd/MM/yyyy");

            if (cblHours.Items.Count > 0)
            {
                foreach (ListItem liHr in cblHours.Items)
                {
                    liHr.Selected = false;
                }
            }
            if (ddlPurpose.Items.Count > 0)
            {
                ddlPurpose.SelectedIndex = 0;
            }
            if (ddlAttendanceOption.Items.Count > 0)
            {
                ddlAttendanceOption.SelectedIndex = 0;
            }
            ddlSecOD_SelectedIndexChanged(new object(), new EventArgs());
            if (FpStudentDetails.Sheets[0].RowCount > 1)
            {
                for (int row = 0; row < FpStudentDetails.Sheets[0].RowCount; row++)
                {
                    FpStudentDetails.Sheets[0].Cells[row, 6].Value = 0;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void attendenace(string degreecode, string semester, int type = 0)
    {
        //rbFullDay.Checked = true;
        //rbHalfDay.Checked = false;
        //rbHourWise.Checked = false;
        //divHalfHr.Visible = false;
        //rbAM.Visible = false;
        //rbPM.Visible = false;
        cblHours.Items.Clear();
        htHoursPerDay.Clear();
        Dictionary<string, int> dicHrsPerDay = new Dictionary<string, int>();
        DataSet dsPeriodDetails = new DataSet();
        ArrayList arrDegree = new ArrayList();
        ArrayList arrSem = new ArrayList();
        if (type != 0)
        {
            //degreecode = string.Empty;
            //semester = string.Empty;
            if (chkStudentWise.Checked)
            {
                if (FpStudentDetails.Sheets[0].RowCount > 1)
                {
                    for (int row = 0; row < FpStudentDetails.Sheets[0].RowCount; row++)
                    {
                        string degree = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 1].Tag).Trim();
                        string sem = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 5].Text).Trim();
                        if (!string.IsNullOrEmpty(sem))
                        {
                            if (!arrSem.Contains(sem.Trim()))
                            {
                                arrSem.Add(sem.Trim());
                                if (string.IsNullOrEmpty(semester))
                                {
                                    semester = "'" + sem + "'";
                                }
                                else
                                {
                                    semester += ",'" + sem + "'";
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(degree))
                        {
                            if (!arrDegree.Contains(degree.Trim()))
                            {
                                arrDegree.Add(degree.Trim());
                                if (string.IsNullOrEmpty(degreecode))
                                {
                                    degreecode = "'" + degree + "'";
                                }
                                else
                                {
                                    degreecode += ",'" + degree + "'";
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                degreecode = Convert.ToString(ddlBranchOD.SelectedValue).Trim();
                semester = Convert.ToString(ddlSemOD.SelectedValue).Trim();
            }
        }
        if (!string.IsNullOrEmpty(degreecode) && !string.IsNullOrEmpty(semester))
        {
            string gerhours = "select no_of_hrs_per_day,no_of_hrs_I_half_day,no_of_hrs_II_half_day from periodattndschedule where degree_code in(" + degreecode + ") and semester in(" + semester + ")";
            dsPeriodDetails = da.select_method_wo_parameter(gerhours, "Text");
        }
        string tothrs = string.Empty;
        string fsthalfhrs = string.Empty;
        string scndhalfhrs = string.Empty;
        if (dsPeriodDetails.Tables.Count > 0 && dsPeriodDetails.Tables[0].Rows.Count > 0)
        {
            tothrs = dsPeriodDetails.Tables[0].Rows[0]["no_of_hrs_per_day"].ToString();
            if (tothrs != "")
            {
                htHoursPerDay.Add("full", tothrs);
            }
            fsthalfhrs = dsPeriodDetails.Tables[0].Rows[0]["no_of_hrs_I_half_day"].ToString();
            if (fsthalfhrs != "")
            {
                htHoursPerDay.Add("fn", fsthalfhrs);
            }
            scndhalfhrs = dsPeriodDetails.Tables[0].Rows[0]["no_of_hrs_II_half_day"].ToString();
            if (scndhalfhrs != "")
            {
                htHoursPerDay.Add("an", scndhalfhrs);
            }
        }
        string temp = string.Empty;
        if (tothrs != "" && tothrs != null)
        {
            if (rbFullDay.Checked == true)
            {
                for (int fulhrs = 1; fulhrs <= Convert.ToInt32(tothrs); fulhrs++)
                {
                    cblHours.Items.Add("" + fulhrs + "");
                }
            }
        }
    }

    private void save()
    {
        try
        {
            divConfirm.Visible = false;
            bool save_flag = false;
            bool isSaveAttendance = false;
            int savevalue = 0;
            string Attvalue = string.Empty;
            //fromDate = txtFromDateOD.Text;
            //toDate = txtToDateOD.Text;
            fromDate = Convert.ToString(txtFromDateOD.Text).Trim();
            toDate = Convert.ToString(txtToDateOD.Text).Trim();
            if (fromDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
                isValidFromDate = isValidDate;
                if (!isValidDate)
                {
                    lblODAlertMsg.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblODAlertMsg.Text = "Please Choose From Date";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
            if (toDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
                isValidToDate = isValidDate;
                if (!isValidDate)
                {
                    lblODAlertMsg.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblODAlertMsg.Text = "Please Choose To Date";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
            string qryDate = string.Empty;
            if (dtFromDate > dtToDate)
            {
                lblODAlertMsg.Text = "From Date Must Be Lesser Than Or Equal To To Date";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
            string dt = fromDate;
            string strholiday = string.Empty;
            string reason = string.Empty;
            string purpose = string.Empty;
            if (ddlPurpose.Text == "")
            {
                reason = string.Empty;
                purpose = string.Empty;
            }
            else
            {
                purpose = Convert.ToString(ddlPurpose.SelectedItem).Trim();
                reason = Convert.ToString(ddlPurpose.SelectedItem).Trim();
            }
            bool isSchoolAttendance = false;
            string[] dsplit = dt.Split(new Char[] { '/' });
            fromDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
            int demfcal = int.Parse(dsplit[2].ToString());
            demfcal = demfcal * 12;
            int cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
            string monthcal = cal_from_date.ToString();
            dt = toDate;
            dsplit = dt.Split(new Char[] { '/' });
            toDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
            int demtcal = int.Parse(dsplit[2].ToString());
            demtcal = demfcal * 12;
            int cal_to_date = demfcal + int.Parse(dsplit[1].ToString());
            DateTime per_from_date = Convert.ToDateTime(fromDate);
            DateTime per_to_date = Convert.ToDateTime(toDate);
            DateTime dumm_from_date = per_from_date;
            lblPopODErr.Visible = false;
            FpStudentDetails.SaveChanges();
            int flag = 0;
            int reval = 0;
            string leavhlf = string.Empty;
            bool setDefault = false;
            if (btnPopSaveOD.Text.Trim().ToLower() == "save")
            {
                reval = 1;
                setDefault = true;
            }
            else
            {
                reval = 0;
                setDefault = false;
            }
            for (int res = reval; res <= Convert.ToInt32(FpStudentDetails.Sheets[0].RowCount) - 1; res++)
            {
                int isval = 0;
                isval = Convert.ToInt32(FpStudentDetails.Sheets[0].Cells[res, 6].Value);
                if (isval == 1)
                {
                    string batchval = Convert.ToString(FpStudentDetails.Sheets[0].Cells[res, 0].Tag).Trim();
                    string degree = Convert.ToString(FpStudentDetails.Sheets[0].Cells[res, 1].Tag).Trim();
                    string semester = Convert.ToString(FpStudentDetails.Sheets[0].Cells[res, 5].Text).Trim();
                    string section = Convert.ToString(FpStudentDetails.Sheets[0].Cells[res, 2].Tag).Trim();
                    string collegeCode = Convert.ToString(FpStudentDetails.Sheets[0].Cells[res, 0].Note).Trim();
                    ht.Clear();
                    ht.Add("degree_code", int.Parse(degree.ToString()));
                    ht.Add("sem", int.Parse(semester));
                    ht.Add("from_date", fromDate.ToString());
                    ht.Add("to_date", toDate.ToString());
                    ht.Add("coll_code", int.Parse(collegeCode));
                    int iscount = 0;
                    DataSet ds2 = new DataSet();
                    qry = "select isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + fromDate.ToString() + "' and '" + toDate.ToString() + "' and degree_code=" + degree + " and semester=" + semester.ToString() + "";
                    ds2.Reset();
                    ds2.Dispose();
                    ds2 = da.select_method(qry, ht, "Text");
                    if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    {
                        iscount = Convert.ToInt16(ds2.Tables[0].Rows[0]["cnt"].ToString());
                    }
                    ht.Add("iscount", iscount);
                    DataSet ds_holi = da.select_method("ALL_HOLIDATE_DETAILS", ht, "sp");
                    isSchoolAttendance = false;
                    isSchoolAttendance = CheckSchoolOrCollege(collegeCode);
                    Hashtable holiday_table = new Hashtable();
                    holiday_table.Clear();
                    if (ds_holi.Tables.Count > 0 && ds_holi.Tables[0].Rows.Count != 0)
                    {
                        for (int k = 0; k < ds_holi.Tables[0].Rows.Count; k++)
                        {
                            if (!holiday_table.ContainsKey(Convert.ToDateTime(ds_holi.Tables[0].Rows[k]["HOLI_DATE"].ToString())))
                            {
                                holiday_table.Add(Convert.ToDateTime(ds_holi.Tables[0].Rows[k]["HOLI_DATE"].ToString()), k);
                            }
                        }
                    }
                    if (ds_holi.Tables.Count > 1 && ds_holi.Tables[1].Rows.Count != 0)
                    {
                        for (int k = 0; k < ds_holi.Tables[1].Rows.Count; k++)
                        {
                            if (!holiday_table.ContainsKey(Convert.ToDateTime(ds_holi.Tables[1].Rows[k]["HOLI_DATE"].ToString())))
                            {
                                holiday_table.Add(Convert.ToDateTime(ds_holi.Tables[1].Rows[k]["HOLI_DATE"].ToString()), k);
                            }
                        }
                    }
                    if (ds_holi.Tables.Count > 2 && ds_holi.Tables[2].Rows.Count != 0)
                    {
                        for (int k = 0; k < ds_holi.Tables[2].Rows.Count; k++)
                        {
                            if (!holiday_table.ContainsKey(Convert.ToDateTime(ds_holi.Tables[2].Rows[k]["HOLI_DATE"].ToString())))
                            {
                                holiday_table.Add(Convert.ToDateTime(ds_holi.Tables[2].Rows[k]["HOLI_DATE"].ToString()), k);
                            }
                        }
                    }
                    int fhrs = 0;
                    string hrs = da.GetFunction("select no_of_hrs_I_half_day from periodattndschedule where degree_code=" + degree.ToString() + " and semester='" + semester.ToString() + "'");
                    DataSet dsval = new DataSet();
                    DataView dvval = new DataView();
                    int noMaxHrsDay = 0;
                    int noFstHrsDay = 0;
                    int noSndHrsDay = 0;
                    int noMinFstHrsDay = 0;
                    int noMinSndHrsDay = 0;
                    string selQ = " select CONVERT(varchar(50), start_date,105) as start_date,isnull(starting_dayorder,1) as starting_dayorder,schorder,nodays,No_of_hrs_per_day,min_hrs_per_day,p.no_of_hrs_I_half_day,p.no_of_hrs_II_half_day,s.batch_year,s.degree_code,s.semester,p.min_pres_I_half_day,p.min_pres_II_half_day from seminfo s,periodattndschedule p where s.degree_code=p.degree_code and s.semester=p.semester and p.degree_code=" + degree.ToString() + " and p.semester='" + semester.ToString() + "'";
                    dsval.Clear();
                    dsval = da.select_method_wo_parameter(selQ, "Text");
                    if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                    {
                        //sch_order = dv1[0]["schorder"].ToString();
                        //no_days = dv1[0]["nodays"].ToString();
                        //startdate = dv1[0]["start_date"].ToString();
                        //starting_dayorder = dv1[0]["starting_dayorder"].ToString();
                        //no_of_hrs = dv1[0]["No_of_hrs_per_day"].ToString();
                        //frst_half_day = dv1[0]["no_of_hrs_I_half_day"].ToString();
                        //secd_half_day = dv1[0]["no_of_hrs_II_half_day"].ToString();
                        //min_frst_half_day = dv1[0]["min_pres_I_half_day"].ToString();
                        //min_secd_half_day = dv1[0]["min_pres_II_half_day"].ToString();
                        int.TryParse(Convert.ToString(dsval.Tables[0].Rows[0]["No_of_hrs_per_day"]), out noMaxHrsDay);
                        int.TryParse(Convert.ToString(dsval.Tables[0].Rows[0]["no_of_hrs_I_half_day"]), out noFstHrsDay);
                        int.TryParse(Convert.ToString(dsval.Tables[0].Rows[0]["no_of_hrs_II_half_day"]), out noSndHrsDay);
                        int.TryParse(Convert.ToString(dsval.Tables[0].Rows[0]["min_pres_I_half_day"]), out noMinFstHrsDay);
                        int.TryParse(Convert.ToString(dsval.Tables[0].Rows[0]["min_pres_II_half_day"]), out noMinSndHrsDay);
                    }
                    //no_of_hrs = dv1[0]["No_of_hrs_per_day"].ToString();
                    //frst_half_day = dv1[0]["no_of_hrs_I_half_day"].ToString();
                    //secd_half_day = dv1[0]["no_of_hrs_II_half_day"].ToString();
                    //min_frst_half_day = dv1[0]["min_pres_I_half_day"].ToString();
                    //min_secd_half_day = dv1[0]["min_pres_II_half_day"].ToString();
                    if (hrs.Trim() != "" && hrs != null && hrs.Trim() != "0")
                    {
                        fhrs = Convert.ToInt32(hrs);
                    }
                    bool leaveflag = false;
                    string strsec = string.Empty;
                    if (ddlSecOD.Items.Count > 0)
                    {
                        if (ddlSecOD.SelectedValue.ToString().Trim().ToLower() != "all" && ddlSecOD.SelectedValue.ToString().Trim().ToLower() != "" && ddlSecOD.SelectedValue.ToString().Trim().ToLower() != "-1")
                        {
                            strsec = " and sm.sections='" + ddlSecOD.SelectedValue.ToString() + "'";
                        }
                        else
                        {
                            strsec = string.Empty;
                        }
                    }
                    string strspecdeiatlqury = "select sm.date,sd.hrdet_no from specialhr_details sd,specialhr_master sm where sd.hrentry_no=sm.hrentry_no and sm.batch_year='" + batchval.ToString() + "' and sm.date between '" + fromDate.ToString() + "' and '" + toDate.ToString() + "' and sm.degree_code=" + degree.ToString() + " and sm.semester='" + semester.ToString() + "' " + strsec + "";
                    DataSet dsspecial = da.select_method_wo_parameter(strspecdeiatlqury, "Text");
                    flag = 1;
                    string appNo = FpStudentDetails.Sheets[0].Cells[res, 5].Tag.ToString();
                    string stdRollno = FpStudentDetails.Sheets[0].Cells[res, 1].Value.ToString();
                    string stdregno = FpStudentDetails.Sheets[0].Cells[res, 2].Text;
                    string stdname = FpStudentDetails.Sheets[0].Cells[res, 4].Text;
                    string stdsem = FpStudentDetails.Sheets[0].Cells[res, 5].Text;
                    int taken_hourse = 0;
                    int monthyear = 0;
                    if (txtFromDateOD.Text != "")//&& txttodate.Text != "")
                    {
                        if (txtNoOfHours.Text != "" || rbFullDay.Checked == true)
                        {
                            fromDate = txtFromDateOD.Text;
                            toDate = txtToDateOD.Text;
                            dt = fromDate;
                            dsplit = dt.Split(new Char[] { '/' });
                            fromDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                            demfcal = int.Parse(dsplit[2].ToString());
                            demfcal = demfcal * 12;
                            cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                            monthcal = cal_from_date.ToString();
                            dt = toDate;
                            dsplit = dt.Split(new Char[] { '/' });
                            toDate = dsplit[2].ToString() + "/" + dsplit[1].ToString() + "/" + dsplit[0].ToString();
                            demtcal = int.Parse(dsplit[2].ToString());
                            demtcal = demfcal * 12;
                            cal_to_date = demfcal + int.Parse(dsplit[1].ToString());
                            per_from_date = Convert.ToDateTime(fromDate);
                            per_to_date = Convert.ToDateTime(toDate);
                            dumm_from_date = per_from_date;
                            Dictionary<string, StringBuilder[]> dicQueryValue = new Dictionary<string, StringBuilder[]>();
                            if (dumm_from_date <= per_to_date)
                            {
                                while (dumm_from_date <= per_to_date)
                                {
                                    StringBuilder sbQueryUpdate = new StringBuilder();
                                    StringBuilder sbQUeryInsertValue = new StringBuilder();
                                    StringBuilder sbQueryColumnName = new StringBuilder();
                                    if (!holiday_table.ContainsKey(dumm_from_date))
                                    {
                                        string dummfromdate = Convert.ToString(dumm_from_date);
                                        string[] fromdate1 = dummfromdate.Split(new char[] { ' ' });
                                        string fromdate2 = fromdate1[0].ToString();
                                        string[] fromdate = fromdate2.Split(new char[] { '/' });
                                        string fromdatedate = fromdate[1].ToString();
                                        string fromdatemonth = fromdate[0].ToString();
                                        string fromdateyear = fromdate[2].ToString();
                                        monthyear = Convert.ToInt32(fromdatemonth) + Convert.ToInt32(fromdateyear) * 12;
                                        string valueupdate = string.Empty;
                                        string insertvalue = string.Empty;
                                        string odvalue = string.Empty;
                                        int totnoofhours = 0;
                                        string[] hourslimit = txtNoOfHours.Text.Split(new char[] { ',' });
                                        totnoofhours = Convert.ToInt32(hourslimit.GetUpperBound(0).ToString()) + 1;
                                        taken_hourse = taken_hourse + totnoofhours;
                                        for (int i = 0; i < Convert.ToInt32(totnoofhours); i++)
                                        {
                                            string particularhrs = hourslimit[i].ToString();
                                            string value = ("d" + fromdatedate + "d" + particularhrs);
                                            string selectddl_value = string.Empty;
                                            if (ddlAttendanceOption.Items.Count > 0)
                                            {
                                                selectddl_value = ddlAttendanceOption.SelectedItem.ToString();
                                                Attvalue = GetAttendanceStatusCode(selectddl_value);
                                                if (valueupdate == "")
                                                {
                                                    valueupdate = value + "=" + Attvalue;
                                                }
                                                else
                                                {
                                                    valueupdate = valueupdate + "," + value + "=" + Attvalue;
                                                }
                                                if (insertvalue == "")
                                                {
                                                    insertvalue = value;
                                                }
                                                else
                                                {
                                                    insertvalue = insertvalue + "," + value;
                                                }
                                                if (odvalue == "")
                                                {
                                                    odvalue = Attvalue;
                                                }
                                                else
                                                {
                                                    odvalue = odvalue + "," + Attvalue;
                                                }
                                            }
                                            else
                                            {
                                                lblPopODErr.Visible = true;
                                                lblPopODErr.Text = "There No Right(s) To This User";
                                                return;
                                            }
                                            //Added by srinath 29/01/2014
                                            //if (Attvalue.Trim() == "3")
                                            // {
                                            //ht.Clear();
                                            //ht.Add("AtWr_App_no", appNo);
                                            //ht.Add("AttWr_CollegeCode", collegeCode);
                                            //ht.Add("columnname", value);
                                            //ht.Add("roll_no", stdRollno);
                                            //ht.Add("month_year", monthyear);
                                            //ht.Add("values", reason);
                                            //string strquery = "sp_ins_upd_student_attendance_reason";
                                            //int insert = da.insert_method(strquery, ht, "sp");
                                            //  }
                                            //*********** modified by annyutha 27/8/14*******//
                                            //else
                                            //{
                                            //    reason=string.Empty;
                                            //    hat.Clear();
                                            //    hat.Add("columnname", value);
                                            //    hat.Add("roll_no", stdRollno);
                                            //    hat.Add("month_year", monthyear);
                                            //    hat.Add("values", reason);
                                            //    strquery = "sp_ins_upd_student_attendance_reason";
                                            //    int insert = d2.insert_method(strquery, hat, "sp");
                                            //}
                                            //***********end***************//
                                        }
                                        if (!string.IsNullOrEmpty(insertvalue))
                                        {
                                            sbQueryColumnName.Append(insertvalue + ",");
                                        }
                                        if (!string.IsNullOrEmpty(odvalue))
                                        {
                                            sbQUeryInsertValue.Append(odvalue + ",");
                                        }
                                        if (!string.IsNullOrEmpty(valueupdate))
                                        {
                                            sbQueryUpdate.Append(valueupdate + ",");
                                        }
                                        //ht.Clear();
                                        //ht.Add("Att_App_no", appNo);
                                        //ht.Add("Att_CollegeCode", collegeCode);
                                        //ht.Add("rollno", stdRollno);
                                        //ht.Add("monthyear", monthyear);
                                        //ht.Add("columnname", insertvalue);
                                        //ht.Add("colvalues", odvalue);
                                        //ht.Add("coulmnvalue", valueupdate);
                                        //savevalue = da.insert_method("sp_ins_upd_student_attendance_Dead", ht, "sp");
                                        save_flag = true;
                                        //days
                                        //StringBuilder strPerDay = new StringBuilder();
                                        //string strPerDays = string.Empty;
                                        //bool hrcheck = false;
                                        //for (int hrcnt = 1; hrcnt <= noMaxHrsDay; hrcnt++)
                                        //{
                                        //    strPerDay.Append("d" + fromdate[1].ToString().TrimStart('0') + "d" + hrcnt + ",");
                                        //}
                                        //if (strPerDay.Length > 0)
                                        //{
                                        //    strPerDay.Remove(strPerDay.Length - 1, 1);
                                        //    strPerDays = Convert.ToString(strPerDay);
                                        //}
                                        //if (save_flag == true)
                                        //{
                                        //    int attval = 0;
                                        //    if (Attvalue == "")
                                        //        attval = 0;
                                        //    else
                                        //        attval = Convert.ToInt32(Attvalue);
                                        //    if (isSchoolAttendance)
                                        //    {
                                        //        attendanceMark(Convert.ToString(appNo), Convert.ToInt32(monthyear), strPerDays, noMaxHrsDay, noFstHrsDay, noSndHrsDay, noMinFstHrsDay, noMinSndHrsDay, Convert.ToString(fromDate), Convert.ToString(collegeCode), attval);
                                        //    }
                                        //}
                                    }
                                    else
                                    {
                                        int starthout = 0;
                                        //taken_hourse = 0;
                                        string strholyquery = "select halforfull,morning,evening from holidaystudents where halforfull=1 and holiday_date='" + dumm_from_date.ToString("MM/dd/yyyy") + "'";
                                        DataSet dsholidayval = da.select_method_wo_parameter(strholyquery, "Text");
                                        if (dsholidayval.Tables.Count > 0 && dsholidayval.Tables[0].Rows.Count > 0)
                                        {
                                            string sethours = string.Empty;
                                            string[] sphrsp = txtNoOfHours.Text.Split(',');
                                            for (int sph = 0; sph <= sphrsp.GetUpperBound(0); sph++)
                                            {
                                                int sehrou = Convert.ToInt32(sphrsp[sph]);
                                                if (sehrou <= fhrs)
                                                {
                                                    if (dsholidayval.Tables[0].Rows[0]["morning"].ToString() == "1" || dsholidayval.Tables[0].Rows[0]["morning"].ToString().Trim().ToLower() == "true")
                                                    {
                                                    }
                                                    else
                                                    {
                                                        taken_hourse = taken_hourse + 1;
                                                        if (sethours == "")
                                                        {
                                                            sethours = sehrou.ToString();
                                                        }
                                                        else
                                                        {
                                                            sethours = sethours + ',' + sehrou.ToString();
                                                        }
                                                    }
                                                }
                                                if (sehrou > fhrs)
                                                {
                                                    if (dsholidayval.Tables[0].Rows[0]["evening"].ToString() == "1" || dsholidayval.Tables[0].Rows[0]["evening"].ToString().Trim().ToLower() == "true")
                                                    {
                                                    }
                                                    else
                                                    {
                                                        taken_hourse = taken_hourse + 1;
                                                        if (sethours == "")
                                                        {
                                                            sethours = sehrou.ToString();
                                                        }
                                                        else
                                                        {
                                                            sethours = sethours + ',' + sehrou.ToString();
                                                        }
                                                    }
                                                }
                                            }
                                            if (sethours != "")
                                            {
                                                int totnoofhours = 0;
                                                string[] hourslimit = sethours.Split(new char[] { ',' });
                                                totnoofhours = Convert.ToInt32(hourslimit.GetUpperBound(0).ToString()) + 1;
                                                string dummfromdate = Convert.ToString(dumm_from_date);
                                                string[] fromdate1 = dummfromdate.Split(new char[] { ' ' });
                                                string fromdate2 = fromdate1[0].ToString();
                                                string[] fromdate = fromdate2.Split(new char[] { '/' });
                                                string fromdatedate = fromdate[1].ToString();
                                                string fromdatemonth = fromdate[0].ToString();
                                                string fromdateyear = fromdate[2].ToString();
                                                monthyear = Convert.ToInt32(fromdatemonth) + Convert.ToInt32(fromdateyear) * 12;
                                                string valueupdate = string.Empty;
                                                string insertvalue = string.Empty;
                                                string odvalue = string.Empty;
                                                for (int i = starthout; i < Convert.ToInt32(totnoofhours); i++)
                                                {
                                                    string particularhrs = hourslimit[i].ToString();
                                                    string value = ("d" + fromdatedate + "d" + particularhrs);
                                                    string selectddl_value = string.Empty;
                                                    if (ddlAttendanceOption.Items.Count > 0)
                                                    {
                                                        selectddl_value = ddlAttendanceOption.SelectedItem.ToString();
                                                        Attvalue = GetAttendanceStatusCode(selectddl_value);
                                                        if (valueupdate == "")
                                                        {
                                                            valueupdate = value + "=" + Attvalue;
                                                        }
                                                        else
                                                        {
                                                            valueupdate = valueupdate + "," + value + "=" + Attvalue;
                                                        }
                                                        if (insertvalue == "")
                                                        {
                                                            insertvalue = value;
                                                        }
                                                        else
                                                        {
                                                            insertvalue = insertvalue + "," + value;
                                                        }
                                                        if (odvalue == "")
                                                        {
                                                            odvalue = Attvalue;
                                                        }
                                                        else
                                                        {
                                                            odvalue = odvalue + "," + Attvalue;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lblPopODErr.Visible = true;
                                                        lblPopODErr.Text = "There No Right(s) To This User";
                                                        return;
                                                    }
                                                    //Added by srinath 29/01/2014
                                                    //if (Attvalue.Trim() == "3")
                                                    //{
                                                    //    ht.Clear();
                                                    //    ht.Add("AtWr_App_no", appNo);
                                                    //    ht.Add("AttWr_CollegeCode", collegeCode);
                                                    //    ht.Add("columnname", value);
                                                    //    ht.Add("roll_no", stdRollno);
                                                    //    ht.Add("month_year", monthyear);
                                                    //    ht.Add("values", reason);
                                                    //    string strquery = "sp_ins_upd_student_attendance_reason";
                                                    //    int insert = da.insert_method(strquery, ht, "sp");
                                                    //}
                                                }
                                                if (!string.IsNullOrEmpty(insertvalue))
                                                {
                                                    sbQueryColumnName.Append(insertvalue + ",");
                                                }
                                                if (!string.IsNullOrEmpty(odvalue))
                                                {
                                                    sbQUeryInsertValue.Append(odvalue + ",");
                                                }
                                                if (!string.IsNullOrEmpty(valueupdate))
                                                {
                                                    sbQueryUpdate.Append(valueupdate + ",");
                                                }
                                                //ht.Clear();
                                                //ht.Add("Att_App_no", appNo);
                                                //ht.Add("Att_CollegeCode", collegeCode);
                                                //ht.Add("rollno", stdRollno);
                                                //ht.Add("monthyear", monthyear);
                                                //ht.Add("columnname", insertvalue);
                                                //ht.Add("colvalues", odvalue);
                                                //ht.Add("coulmnvalue", valueupdate);
                                                //savevalue = da.insert_method("sp_ins_upd_student_attendance_Dead", ht, "sp");
                                                //string txtintime = ddlInTimeHr.SelectedValue.ToString() + ":" + ddlInTimeMM.SelectedValue.ToString() + ":00" + " " + ddlInTimeSess.SelectedValue.ToString();
                                                //string txtouttime = ddlOutTimeHr.SelectedValue.ToString() + ":" + ddlOutTimeMM.SelectedValue.ToString() + ":00" + " " + ddlOutTimeSess.SelectedValue.ToString();
                                                //ht.Clear();
                                                //ht.Add("rollno", stdRollno);
                                                //ht.Add("semester", stdsem);
                                                //ht.Add("purpose", purpose.ToString());
                                                //ht.Add("fromdate", Convert.ToDateTime(fromDate));
                                                //ht.Add("todate", Convert.ToDateTime(toDate));
                                                //ht.Add("outtime", Convert.ToDateTime(txtouttime));
                                                //ht.Add("intime", Convert.ToDateTime(txtintime));
                                                //ht.Add("college_code", collegeCode);
                                                //ht.Add("attnd_type", Attvalue);
                                                //ht.Add("no_of_hourse", taken_hourse);
                                                //ht.Add("hourse", txtNoOfHours.Text.ToString());
                                                //savevalue = da.insert_method("sp_ins_upd_student_OdEntry", ht, "sp");
                                                save_flag = true;
                                            }
                                            if (leaveflag == false && sethours == "")
                                            {
                                                if (strholiday == "")
                                                {
                                                    strholiday = "Holiday(s) are : " + dumm_from_date.ToString("dd/MM/yyyy") + "(Half day Holiday)";
                                                }
                                                else
                                                {
                                                    strholiday = strholiday + "," + dumm_from_date.ToString("dd/MM/yyyy") + "(Half day Holiday)";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (leaveflag == false)
                                            {
                                                if (strholiday == "")
                                                {
                                                    strholiday = "Holiday(s) are : " + dumm_from_date.ToString("dd/MM/yyyy");
                                                }
                                                else
                                                {
                                                    strholiday = strholiday + "," + dumm_from_date.ToString("dd/MM/yyyy");
                                                }
                                            }
                                        }
                                    }
                                    StringBuilder[] sbAll = new StringBuilder[3];
                                    if (!string.IsNullOrEmpty(sbQueryColumnName.ToString().Trim()) && !string.IsNullOrEmpty(sbQUeryInsertValue.ToString().Trim()) && !string.IsNullOrEmpty(sbQueryUpdate.ToString().Trim()))
                                    {
                                        if (dicQueryValue.ContainsKey(monthyear.ToString().Trim()))
                                        {
                                            sbAll = dicQueryValue[monthyear.ToString().Trim()];
                                            sbAll[0].Append(sbQueryColumnName);
                                            sbAll[1].Append(sbQUeryInsertValue);
                                            sbAll[2].Append(sbQueryUpdate);
                                            dicQueryValue[monthyear.ToString().Trim()] = sbAll;
                                        }
                                        else if (monthyear != 0)
                                        {
                                            sbAll[0] = new StringBuilder();
                                            sbAll[1] = new StringBuilder();
                                            sbAll[2] = new StringBuilder();
                                            sbAll[0].Append(Convert.ToString(sbQueryColumnName));
                                            sbAll[1].Append(Convert.ToString(sbQUeryInsertValue));
                                            sbAll[2].Append(Convert.ToString(sbQueryUpdate));
                                            dicQueryValue.Add(monthyear.ToString().Trim(), sbAll);
                                        }
                                    }
                                    //====================Special Hour Attendance =======================
                                    dsspecial.Tables[0].DefaultView.RowFilter = "date='" + dumm_from_date.ToString("MM/dd/yyyy") + "'";
                                    DataView dvspecy = dsspecial.Tables[0].DefaultView;
                                    for (int sph = 0; sph < dvspecy.Count; sph++)
                                    {
                                        string hrdepno = dvspecy[sph]["hrdet_no"].ToString();
                                        string strspattval = " if exists (select * from specialhr_attendance where hrdet_no='" + hrdepno + "' and month_year='" + monthyear + "' and roll_no='" + stdRollno + "' and SpHr_App_no='" + appNo + "')";
                                        strspattval = strspattval + " update specialhr_attendance set attendance='" + Attvalue + "',SpHr_CollegeCode='" + collegeCode + "'  where hrdet_no='" + hrdepno + "' and month_year='" + monthyear + "' and roll_no='" + stdRollno + "'  and SpHr_App_no='" + appNo + "'";
                                        strspattval = strspattval + " else";
                                        strspattval = strspattval + " insert into specialhr_attendance(SpHr_App_no,SpHr_CollegeCode,roll_no,hrdet_no,attendance,month_year) values('" + appNo + "','" + collegeCode + "','" + stdRollno + "','" + hrdepno + "','" + Attvalue + "','" + monthyear + "')";
                                        savevalue = da.update_method_wo_parameter(strspattval, "Text");
                                    }
                                    dumm_from_date = dumm_from_date.AddDays(1);
                                }
                                string txtintime = ddlInTimeHr.SelectedValue.ToString() + ":" + ddlInTimeMM.SelectedValue.ToString() + ":00" + " " + ddlInTimeSess.SelectedValue.ToString();
                                string txtouttime = ddlOutTimeHr.SelectedValue.ToString() + ":" + ddlOutTimeMM.SelectedValue.ToString() + ":00" + " " + ddlOutTimeSess.SelectedValue.ToString();
                                ht.Clear();
                                ht.Add("rollno", stdRollno);
                                ht.Add("semester", stdsem);
                                ht.Add("purpose", purpose.ToString());
                                ht.Add("fromdate", Convert.ToDateTime(fromDate));
                                ht.Add("todate", Convert.ToDateTime(toDate));
                                ht.Add("outtime", Convert.ToDateTime(txtouttime));
                                ht.Add("intime", Convert.ToDateTime(txtintime));
                                ht.Add("college_code", collegeCode);
                                ht.Add("attnd_type", Attvalue);
                                ht.Add("no_of_hourse", taken_hourse);
                                ht.Add("hourse", txtNoOfHours.Text.ToString());
                                string colgcode = collegeCode;
                                savevalue = da.insert_method("sp_ins_upd_student_OdEntry", ht, "sp");
                                if (savevalue != 0)
                                {
                                    isSaveAttendance = true;
                                }
                                if (dicQueryValue.Count > 0)
                                {
                                    StringBuilder[] spAll = new StringBuilder[3];
                                    foreach (KeyValuePair<string, StringBuilder[]> dicQueery in dicQueryValue)
                                    {
                                        spAll = new StringBuilder[3];
                                        string monthValue = dicQueery.Key;
                                        spAll = dicQueery.Value;
                                        string insertColumnName = spAll[0].ToString().Trim(',');
                                        string insertColumnValue = spAll[1].ToString().Trim(',');
                                        string updateColumnNameValue = spAll[2].ToString().Trim(',');
                                        if (Attvalue.Trim() == "3")
                                        {
                                            string[] splitColumn = insertColumnName.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (string sp in splitColumn)
                                            {
                                                ht.Clear();
                                                ht.Add("AtWr_App_no", appNo);
                                                ht.Add("AttWr_CollegeCode", collegeCode);
                                                ht.Add("columnname", sp);
                                                ht.Add("roll_no", stdRollno);
                                                ht.Add("month_year", monthValue);
                                                ht.Add("values", reason);
                                                string strquery = "sp_ins_upd_student_attendance_reason";
                                                int insert = da.insert_method(strquery, ht, "sp");
                                                if (savevalue != 0)
                                                {
                                                    isSaveAttendance = true;
                                                }
                                            }
                                        }
                                        ht.Clear();
                                        ht.Add("Att_App_no", appNo);
                                        ht.Add("Att_CollegeCode", collegeCode);
                                        ht.Add("rollno", stdRollno);
                                        ht.Add("monthyear", monthValue);
                                        ht.Add("columnname", insertColumnName);
                                        ht.Add("colvalues", insertColumnValue);
                                        ht.Add("coulmnvalue", updateColumnNameValue);
                                        savevalue = da.insert_method("sp_ins_upd_student_attendance_Dead", ht, "sp");
                                        if (savevalue != 0)
                                        {
                                            isSaveAttendance = true;
                                        }
                                    }
                                    dumm_from_date = per_from_date;
                                    while (dumm_from_date <= per_to_date)
                                    {
                                        StringBuilder sbQueryUpdate = new StringBuilder();
                                        StringBuilder sbQUeryInsertValue = new StringBuilder();
                                        StringBuilder sbQueryColumnName = new StringBuilder();
                                        if (!holiday_table.ContainsKey(dumm_from_date))
                                        {
                                            int monthValue = Convert.ToInt32(dumm_from_date.Month.ToString().TrimStart('0')) + Convert.ToInt32(dumm_from_date.Year) * 12;
                                            StringBuilder strPerDay = new StringBuilder();
                                            string strPerDays = string.Empty;
                                            bool hrcheck = false;
                                            for (int hrcnt = 1; hrcnt <= noMaxHrsDay; hrcnt++)
                                            {
                                                strPerDay.Append("d" + dumm_from_date.Day.ToString().TrimStart('0') + "d" + hrcnt + ",");
                                            }
                                            if (strPerDay.Length > 0)
                                            {
                                                strPerDay.Remove(strPerDay.Length - 1, 1);
                                                strPerDays = Convert.ToString(strPerDay);
                                            }
                                            if (save_flag == true && isSaveAttendance)
                                            {
                                                int attval = 0;
                                                if (Attvalue == "")
                                                    attval = 0;
                                                else
                                                    attval = Convert.ToInt32(Attvalue);
                                                if (isSchoolAttendance)
                                                {
                                                    attendanceMark(Convert.ToString(appNo), Convert.ToInt32(monthValue), strPerDays, noMaxHrsDay, noFstHrsDay, noSndHrsDay, noMinFstHrsDay, noMinSndHrsDay, Convert.ToString(dtFromDate.ToString("yyyy/MM/dd")), Convert.ToString(collegeCode), attval);
                                                }
                                            }
                                        }
                                        dumm_from_date = dumm_from_date.AddDays(1);
                                    }
                                }
                                if (setDefault)
                                {
                                    FpStudentDetails.Sheets[0].Cells[res, 6].Value = 0;
                                }
                            }
                            else
                            {
                                lblPopODErr.Visible = true;
                                lblPopODErr.Text = "From date should be less than Todate";
                            }
                        }
                        else
                        {
                            lblPopODErr.Visible = true;
                            lblPopODErr.Text = "Select Hours";
                        }
                    }
                    leaveflag = true;
                }
            }
            if (flag == 0)
            {
                lblPopODErr.Visible = true;
                lblPopODErr.Text = "Select Students and Proceed";
            }
            string successMessage = string.Empty;
            if (save_flag == true)
            {
                if (strholiday != "")
                {
                    lblPopODErr.Visible = true;
                    lblPopODErr.Text = strholiday + " " + leavhlf;
                }
                if (setDefault)
                {
                    successMessage = "Saved ";
                    SetDefaultODEntry();
                }
                else
                {
                    successMessage = "Updated ";
                }
                if (isSaveAttendance)
                {
                    lblODAlertMsg.Text = successMessage + " Successfully";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
                else
                {
                    lblODAlertMsg.Text = "Not " + successMessage;
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
            }
            else
            {
                if (setDefault)
                {
                    successMessage = "Saved ";
                    SetDefaultODEntry();
                }
                else
                {
                    successMessage = "Updated ";
                }
                if (strholiday != "")
                {
                    lblPopODErr.Visible = true;
                    lblPopODErr.Text = strholiday;
                }
                lblODAlertMsg.Text = "Not " + successMessage;
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlPurpose_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void txtFromDateOD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //lblnorec.Visible = false;
            //btnoddelete.Visible = false;
            //btnodupdate.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void txtToDateOD_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //lblnorec.Visible = false;
            //btnoddelete.Visible = false;
            //btnodupdate.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void btnPopDeleteOD_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirm.Visible = true;
            lblSaveorDelete.Text = "2";
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void btnPopSaveOD_Click(object sender, EventArgs e)
    {
        try
        {
            fromDate = Convert.ToString(txtFromDateOD.Text).Trim();
            toDate = Convert.ToString(txtToDateOD.Text).Trim();
            FpStudentDetails.SaveChanges();
            if (fromDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
                isValidFromDate = isValidDate;
                if (!isValidDate)
                {
                    lblODAlertMsg.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblODAlertMsg.Text = "Please Choose From Date";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
            if (toDate.Trim() != "")
            {
                isValidDate = false;
                isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
                isValidToDate = isValidDate;
                if (!isValidDate)
                {
                    lblODAlertMsg.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblODAlertMsg.Text = "Please Choose To Date";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
            string qryDate = string.Empty;
            if (dtFromDate > dtToDate)
            {
                lblODAlertMsg.Text = "From Date Must Be Lesser Than Or Equal To To Date";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
            int reval = 0;
            bool isSelected = false;
            if (btnPopSaveOD.Text.Trim().ToLower() == "save")
            {
                reval = 1;
            }
            else
            {
                reval = 0;
            }
            if (ddlAttendanceOption.Items.Count == 0)
            {
                lblODAlertMsg.Text = "There is No Attendance Right(s) To This User";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
            if (FpStudentDetails.Sheets[0].RowCount == reval)
            {
                lblODAlertMsg.Text = "No Students Were Found";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
            if (cblHours.Items.Count == 0)
            {
                lblODAlertMsg.Text = "There is No Hour(s) Were Found";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
            else
            {
                bool hourSelect = false;
                foreach (ListItem liHr in cblHours.Items)
                {
                    if (liHr.Selected)
                    {
                        hourSelect = true;
                    }
                }
                if (!hourSelect)
                {
                    lblODAlertMsg.Text = "Please Select Any One Hour And Then Proceed";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
            }
            for (int res = reval; res <= Convert.ToInt32(FpStudentDetails.Sheets[0].RowCount) - 1; res++)
            {
                int isval = 0;
                int.TryParse(Convert.ToString(FpStudentDetails.Sheets[0].Cells[res, 6].Value).Trim(), out isval);
                //isval = Convert.ToInt32(FpStudentDetails.Sheets[0].Cells[res, 6].Value);
                if (isval == 1)
                {
                    isSelected = true;
                }
            }
            if (isSelected)
            {
                bool daychek = AttendanceDayLock(dtFromDate, ((chkStudentWise.Checked) ? 1 : 0));
                if (Session["UserName"].ToString().Trim() == "admin")
                {
                    daychek = true;
                }
                if (daychek == true)
                {
                    if (btnPopSaveOD.Text.Trim().ToLower() == "save" || btnPopSaveOD.Text.Trim().ToLower() == "update")
                    {
                        lblSaveorDelete.Text = "1";
                    }
                    else
                    {
                        lblSaveorDelete.Text = "2";
                    }
                    divConfirm.Visible = true;
                    lblConfirmMsg.Text = "Do you want mark attendance from " + dtFromDate.ToString("dd/MM/yyyy") + " to " + dtToDate.ToString("dd/MM/yyyy");
                }
                else
                {
                    lblODAlertMsg.Text = "You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblODAlertMsg.Text = "Please Select Any Atleast One Student and Then Proceed";
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public bool AttendanceDayLock(DateTime seldate, int type = 0)
    {
        string collegecode = Session["collegecode"].ToString();
        bool daycheck = true;
        string cudate = DateTime.Now.ToString("MM/dd/yyyy");
        DateTime curdate = Convert.ToDateTime(cudate);
        //string batch = ddlbatch.SelectedValue.ToString();
        string degree = ddlBranchOD.SelectedValue.ToString();
        string sem = ddlSemOD.SelectedValue.ToString();
        ArrayList arrDegree = new ArrayList();
        ArrayList arrSem = new ArrayList();
        string degreecode = string.Empty;
        string semester = string.Empty;
        if (type != 0)
        {
            //degreecode = string.Empty;
            //semester = string.Empty;
            if (chkStudentWise.Checked)
            {
                if (FpStudentDetails.Sheets[0].RowCount > 1)
                {
                    for (int row = 0; row < FpStudentDetails.Sheets[0].RowCount; row++)
                    {
                        string degree1 = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 1].Tag).Trim();
                        string sem1 = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 5].Text).Trim();
                        if (!string.IsNullOrEmpty(sem1))
                        {
                            if (!arrSem.Contains(sem1.Trim()))
                            {
                                arrSem.Add(sem1.Trim());
                                if (string.IsNullOrEmpty(semester))
                                {
                                    semester = "'" + sem1 + "'";
                                }
                                else
                                {
                                    semester += ",'" + sem1 + "'";
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(degree1))
                        {
                            if (!arrDegree.Contains(degree1.Trim()))
                            {
                                arrDegree.Add(degree1.Trim());
                                if (string.IsNullOrEmpty(degreecode))
                                {
                                    degreecode = "'" + degree1 + "'";
                                }
                                else
                                {
                                    degreecode += ",'" + degree1 + "'";
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                degreecode = Convert.ToString(ddlBranchOD.SelectedValue).Trim();
                semester = Convert.ToString(ddlSemOD.SelectedValue).Trim();
                collegecode = Convert.ToString(ddlCollegeOD.SelectedValue).Trim();
            }
        }
        else
        {
            degreecode = Convert.ToString(ddlBranchOD.SelectedValue).Trim();
            semester = Convert.ToString(ddlSemOD.SelectedValue).Trim();
            collegecode = Convert.ToString(ddlCollegeOD.SelectedValue).Trim();
        }
        if (seldate.ToString("MM/dd/yyyy") == curdate.ToString("MM/dd/yyyy"))
        {
            return daycheck;
        }
        else
        {
            string lockdayvalue = "select LockODDays,LOD_Flag from collinfo where college_code='" + collegecode + "'";
            DataSet ds = new DataSet();
            ds = da.select_method_wo_parameter(lockdayvalue, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["LOD_Flag"].ToString().Trim().ToLower() == "true" || ds.Tables[0].Rows[0]["LOD_Flag"].ToString().Trim() == "1")
                {
                    if (ds.Tables[0].Rows[0][0].ToString() != null && int.Parse(ds.Tables[0].Rows[0][0].ToString()) >= 0)
                    {
                        int total = int.Parse(ds.Tables[0].Rows[0]["LockODDays"].ToString());
                        String strholidasquery = "select distinct holiday_date from holidaystudents where degree_code in(" + degreecode + ")  and semester in(" + semester + ") and holiday_date between '" + seldate.ToString("MM/dd/yyyy") + "' and '" + curdate.ToString("MM/dd/yyyy") + "'";
                        DataSet ds1 = new DataSet();
                        ds1 = da.select_method_wo_parameter(strholidasquery, "Text");
                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            total = total + ds1.Tables[0].Rows.Count;
                        }
                        DateTime dt1 = seldate;
                        DateTime dt2 = curdate;
                        TimeSpan ts = dt2 - dt1;
                        int dif_days = ts.Days;
                        if (dif_days > total)
                        {
                            daycheck = false;
                        }
                    }
                }
            }
        }
        return daycheck;
    }

    protected void btnPopExitOD_Click(object sender, EventArgs e)
    {
        try
        {
            divODEntryDetails.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void ShowStudentsList(byte type = 0, string studentRollNo = null)
    {
        try
        {
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
            isValidDate = false;
            isValidFromDate = false;
            isValidToDate = false;
            fromDate = Convert.ToString(txtFromDateOD.Text).Trim();
            toDate = Convert.ToString(txtToDateOD.Text).Trim();
            if (type != 0)
            {
                FpStudentDetails.SaveChanges();
            }
            orderBySetting = da.GetFunction("select value from master_Settings where settings='order_by'");
            orderBySetting = orderBySetting.Trim();
            orderBy = "ORDER BY rollNoLen,r.roll_no";
            switch (orderBySetting)
            {
                case "0":
                    orderBy = "ORDER BY rollNoLen,r.roll_no";
                    break;
                case "1":
                    orderBy = "ORDER BY regNoLen,r.Reg_No";
                    break;
                case "2":
                    orderBy = "ORDER BY r.Stud_Name";
                    break;
                case "0,1,2":
                    orderBy = "ORDER BY rollNoLen,r.roll_no,regNoLen,r.Reg_No,r.stud_name";
                    break;
                case "0,1":
                    orderBy = "ORDER BY rollNoLen,r.roll_no,regNoLen,r.Reg_No";
                    break;
                case "1,2":
                    orderBy = "ORDER BY regNoLen,r.Reg_No,r.Stud_Name";
                    break;
                case "0,2":
                    orderBy = "ORDER BY rollNoLen,r.roll_no,r.Stud_Name";
                    break;
                default:
                    orderBy = "ORDER BY rollNoLen,r.roll_no";
                    break;
            }
            Farpoint.SheetView svsort = new FarPoint.Web.Spread.SheetView();
            Farpoint.CheckBoxCellType chkAll = new Farpoint.CheckBoxCellType();
            chkAll.AutoPostBack = true;
            Farpoint.CheckBoxCellType chkSingleCell = new Farpoint.CheckBoxCellType();
            Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
            DataSet dsStudentDetails = new DataSet();
            DataSet dsDegreeDetails = new DataSet();
            if (type == 0)
            {
                if (ddlCollegeOD.Items.Count == 0)
                {
                    lblAlertMsg.Text = "No " + lblCollegeOD.Text.Trim() + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                else
                {
                    collegeCode = string.Empty;
                    qryCollegeCode = string.Empty;
                    foreach (ListItem li in ddlCollegeOD.Items)
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
                if (ddlBatchOD.Items.Count == 0)
                {
                    lblAlertMsg.Text = "No " + lblBatchOD.Text.Trim() + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                else
                {
                    batchYear = string.Empty;
                    qryBatchYear = string.Empty;
                    foreach (ListItem li in ddlBatchOD.Items)
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
                        qryBatchYear = " and r.Batch_year in(" + batchYear + ")";
                    }
                }
                if (ddlDegreeOD.Items.Count == 0)
                {
                    lblAlertMsg.Text = "No " + lblDegreeOD.Text.Trim() + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                else
                {
                    courseId = string.Empty;
                    qryCourseId = string.Empty;
                    foreach (ListItem li in ddlDegreeOD.Items)
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
                if (ddlBranchOD.Items.Count == 0)
                {
                    lblAlertMsg.Text = "No " + lblBranchOD.Text.Trim() + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                else
                {
                    degreeCode = string.Empty;
                    qryDegreeCode = string.Empty;
                    foreach (ListItem li in ddlBranchOD.Items)
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
                        qryDegreeCode = " and r.degree_code in(" + degreeCode + ")";
                    }
                }
                if (ddlSemOD.Items.Count == 0)
                {
                    lblAlertMsg.Text = "No " + lblSemOD.Text.Trim() + " Were Found";
                    divPopAlert.Visible = true;
                    return;
                }
                else
                {
                    semester = string.Empty;
                    qrySemester = string.Empty;
                    foreach (ListItem li in ddlSemOD.Items)
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
                        qrySemester = " and r.current_semester in(" + semester + ")";
                    }
                }
                if (ddlSecOD.Items.Count > 0)
                {
                    section = string.Empty;
                    qrySection = string.Empty;
                    foreach (ListItem li in ddlSecOD.Items)
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
                    if (!string.IsNullOrEmpty(section))
                    {
                        qrySection = " and sections in(" + section + ")";
                    }
                }
                else
                {
                    section = string.Empty;
                    qrySection = string.Empty;
                }
                if (!string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qrySemester))
                {
                    //qry = "select r.roll_no,r.college_code,r.reg_no,r.stud_name,r.current_semester,r.sections,r.Roll_Admit,r.app_no,c.Course_Id from registration r,course c,Degree dg,Department dt where dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code=r.degree_code and r.college_code=dt.college_code and r.college_code=c.college_code and c.college_code=dg.college_code  and r.cc=0 and r.delflag=0 and r.exam_flag<>'Debar' " + qryCollegeCode + qryBatchYear + qryDegreeCode + qrySemester + qrySection + " and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1) " + orderBy + "";
                    qry = "select r.roll_no,r.college_code,len(r.Reg_No) as regNoLen,len(r.roll_no) rollNoLen,r.reg_no,r.stud_name,r.current_semester,ltrim(rtrim(isnull(r.sections,''))) as sections,r.Roll_Admit,r.batch_year,r.degree_code,r.app_no,c.Course_Id from registration r,course c,Degree dg,Department dt where dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code=r.degree_code and r.college_code=dt.college_code and r.college_code=c.college_code and c.college_code=dg.college_code  and r.cc=0 and r.delflag=0 and r.exam_flag<>'Debar' " + qryCollegeCode + qryBatchYear + qryDegreeCode + qrySemester + qrySection + " and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1) " + orderBy + "";
                    dsStudentDetails = da.select_method_wo_parameter(qry, "text");
                }
            }
            else if (type == 2)
            {
                if (!string.IsNullOrEmpty(studentRollNo))
                {
                    qry = "select r.roll_no,r.college_code,len(r.Reg_No) as regNoLen,len(r.roll_no) rollNoLen,r.reg_no,r.stud_name,r.current_semester,ltrim(rtrim(isnull(r.sections,''))) as sections,r.Roll_Admit,r.batch_year,r.degree_code,r.app_no,c.Course_Id from registration r,course c,Degree dg,Department dt where dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code=r.degree_code and r.college_code=dt.college_code and r.college_code=c.college_code and c.college_code=dg.college_code  and r.cc=0 and r.delflag=0 and r.exam_flag<>'Debar' and r.Roll_No='" + studentRollNo + "' and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1)" + orderBy;
                    dsStudentDetails = da.select_method_wo_parameter(qry, "text");
                }
            }
            else if (type == 1)
            {
                string commonSelection = string.Empty;
                string rollNo = string.Empty;
                string appNo = string.Empty;
                string regNo = string.Empty;
                string admitNo = string.Empty;
                fromDate = Convert.ToString(txtFromDateOD.Text).Trim();
                toDate = Convert.ToString(txtToDate.Text).Trim();
                commonSelection = Convert.ToString(txtStudent.Text).Trim();
                if (string.IsNullOrEmpty(commonSelection))
                {
                    lblPopODErr.Text = "Please Enter The " + lblStudentOptions.Text.Trim();
                    divPopODAlert.Visible = true;
                    btnODPopAlertClose.Focus();
                    return;
                }
                if (ddlSearchBy.Items.Count > 0)
                {
                    string selectedItems = Convert.ToString(ddlSearchBy.SelectedItem.Text).Trim().ToLower();
                    string selectedValue = Convert.ToString(ddlSearchBy.SelectedValue).Trim();
                    //switch (selectedItems)
                    //{
                    //    case "roll no":
                    //        rollNo = commonSelection.Trim();
                    //        break;
                    //    case "register no":
                    //        rollNo = da.GetFunction("select Roll_No from Registration where Reg_no='" + rollNo + "'");
                    //        break;
                    //    case "admission no":
                    //        rollNo = da.GetFunction("select Roll_No from Registration where Roll_Admit='" + commonSelection + "'");
                    //        break;
                    //}
                    switch (selectedValue)
                    {
                        case "3":
                            rollNo = commonSelection.Trim();
                            break;
                        case "2":
                            rollNo = da.GetFunction("select Roll_No from Registration where Reg_no='" + rollNo + "'");
                            break;
                        case "1":
                            rollNo = da.GetFunction("select Roll_No from Registration where Roll_Admit='" + commonSelection + "'");
                            break;
                    }
                }
                else
                {
                    if (lbl_clgT.Text.Trim().ToUpper() == "SCHOOL") //jai
                    {
                        rollNo = da.GetFunction("select Roll_No from Registration where Roll_Admit='" + commonSelection + "'");
                    }
                    else
                    {
                        if (lblStudentOptions.Text.Trim().ToLower() == "register no")
                        {
                            rollNo = da.GetFunction("select Roll_No from Registration where Reg_no='" + commonSelection + "'");
                        }
                        else if (lblStudentOptions.Text.Trim().ToLower() == "admission no")
                        {
                            rollNo = da.GetFunction("select Roll_No from Registration where Roll_Admit='" + commonSelection + "'");
                        }
                        else if (lblStudentOptions.Text.Trim().ToLower().Contains("Student Roll_no") || lblStudentOptions.Text.Trim().ToLower().Contains("roll no"))
                        {
                            rollNo = commonSelection;
                        }
                    }
                }
                for (int r = 0; r < FpStudentDetails.Sheets[0].RowCount; r++)
                {
                    if (FpStudentDetails.Sheets[0].Cells[r, 1].Text.ToString().Trim().ToLower() == rollNo.Trim().ToLower())
                    {
                        txtStudent.Text = string.Empty;
                        divPopODAlert.Visible = true;
                        lblODAlertMsg.Text = "Already Exist the Roll No: " + rollNo;
                        btnODPopAlertClose.Focus();
                        return;
                    }
                }
                if (fromDate.Trim() != "")
                {
                    isValidDate = false;
                    isValidDate = DateTime.TryParseExact(fromDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtFromDate);
                    isValidFromDate = isValidDate;
                    if (!isValidDate)
                    {
                        txtStudent.Text = string.Empty;
                        btnODPopAlertClose.Focus();
                        lblODAlertMsg.Text = "From Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                        lblODAlertMsg.Visible = true;
                        divPopODAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    txtStudent.Text = string.Empty;
                    btnODPopAlertClose.Focus();
                    lblODAlertMsg.Text = "Please Choose From Date";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
                if (toDate.Trim() != "")
                {
                    isValidDate = false;
                    isValidDate = DateTime.TryParseExact(toDate.Trim(), "dd/MM/yyyy", null, DateTimeStyles.None, out dtToDate);
                    isValidToDate = isValidDate;
                    if (!isValidDate)
                    {
                        txtStudent.Text = string.Empty;
                        btnODPopAlertClose.Focus();
                        lblODAlertMsg.Text = "To Date Is In Invalid Format.Must Be In The Format dd/MM/yyyy";
                        lblODAlertMsg.Visible = true;
                        divPopODAlert.Visible = true;
                        return;
                    }
                }
                else
                {
                    txtStudent.Text = string.Empty;
                    btnODPopAlertClose.Focus();
                    lblODAlertMsg.Text = "Please Choose To Date";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
                string qryDate = string.Empty;
                if (dtFromDate > dtToDate)
                {
                    txtStudent.Text = string.Empty;
                    txtStudent.Focus();
                    btnODPopAlertClose.Focus();
                    lblODAlertMsg.Text = "From Date Must Be Lesser Than Or Equal To To Date";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    return;
                }
                else
                {
                    qryDate = " and convert(datetime,od.fromdate,105) between '" + dtFromDate.ToString("MM/dd/yyyy") + "' and '" + dtToDate.ToString("MM/dd/yyyy") + "'";
                }
                if (string.IsNullOrEmpty(rollNo) || rollNo == "0")
                {
                    lblODAlertMsg.Text = lblStudentOptions.Text.Trim() + " " + txtStudent.Text.Trim() + " is Not Available.";
                    lblODAlertMsg.Visible = true;
                    divPopODAlert.Visible = true;
                    txtStudent.Text = string.Empty;
                    txtStudent.Focus();
                    btnODPopAlertClose.Focus();
                    return;
                }
                string existroll = da.GetFunction("select Convert(nvarchar(15),od.fromdate,103)+' To '+ Convert(nvarchar(15),od.Todate,103) from Onduty_Stud od where Roll_no='" + rollNo + "' and (convert(datetime,od.fromdate,105) between '" + dtFromDate.ToString("MM/dd/yyyy") + "' and '" + dtToDate.ToString("MM/dd/yyyy") + "' or convert(datetime,od.Todate,105)  between '" + dtFromDate.ToString("MM/dd/yyyy") + "' and '" + dtToDate.ToString("MM/dd/yyyy") + "')");
                if (existroll.Trim() != "" && existroll != "0")
                {
                    txtStudent.Text = string.Empty;
                    txtStudent.Focus();
                    btnODPopAlertClose.Focus();
                    lblAlertMsg.Text = "Already Student Exist in Od Entry at " + existroll;
                    divPopAlert.Visible = true;
                    return;
                }
                //qry = "select r.roll_no,r.college_code,r.reg_no,r.stud_name,r.current_semester,r.sections,r.Roll_Admit,r.app_no,c.Course_Id from registration r,course c,Degree dg,Department dt where dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code=r.degree_code and r.college_code=dt.college_code and r.college_code=c.college_code and c.college_code=dg.college_code  and r.cc=0 and r.delflag=0 and r.exam_flag<>'Debar' " + qryCollegeCode + qryBatchYear + qryDegreeCode + qrySemester + qrySection + " and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1) " + orderBy + "";
                qry = "select r.roll_no,r.college_code,len(r.Reg_No) as regNoLen,len(r.roll_no) rollNoLen,r.reg_no,r.stud_name,r.current_semester,ltrim(rtrim(isnull(r.sections,''))) as sections,r.Roll_Admit,r.batch_year,r.degree_code,r.app_no,c.Course_Id from registration r,course c,Degree dg,Department dt where dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code=r.degree_code and r.college_code=dt.college_code and r.college_code=c.college_code and c.college_code=dg.college_code  and r.cc=0 and r.delflag=0 and r.exam_flag<>'Debar' and r.Roll_No='" + rollNo + "' and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1)" + orderBy;
                dsStudentDetails = da.select_method_wo_parameter(qry, "text");
            }
            if (dsStudentDetails.Tables.Count > 0 && dsStudentDetails.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                DataRow dr;
                dt.Columns.Add("Batch");
                dt.Columns.Add("RollNo");
                dt.Columns.Add("DegreeCode");
                dt.Columns.Add("RegNo");
                dt.Columns.Add("Section");
                dt.Columns.Add("StudentName");
                dt.Columns.Add("Semester");
                dt.Columns.Add("AdmissionNo");
                dt.Columns.Add("AppNo");
                dt.Columns.Add("CollegeCode");
                dt.Columns.Add("courseId");
                int serialNo = 0;
                if (type == 0)
                {
                    Init_Spread(FpStudentDetails, 1);
                    FpStudentDetails.Sheets[0].RowCount = 1;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].CellType = chkAll;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].Locked = false;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    serialNo = 0;
                    FpStudentDetails.SaveChanges();
                    attendenace(Convert.ToString(ddlBranchOD.SelectedValue).Trim(), Convert.ToString(ddlSemOD.SelectedValue).Trim());
                    if (rbFullDay.Checked == true)
                    {
                        rbFullDay_CheckedChanged(new Object(), new EventArgs());
                    }
                    else if (rbHalfDay.Checked == true)
                    {
                        rbHalfDay_CheckedChanged(new Object(), new EventArgs());
                    }
                    else if (rbHourWise.Checked == true)
                    {
                        rbHourWise_CheckedChanged(new Object(), new EventArgs());
                    }
                }
                else if (type == 1)
                {
                    FpStudentDetails.SaveChanges();
                    if (FpStudentDetails.Sheets[0].RowCount > 1)
                    {
                        for (int row = 1; row < FpStudentDetails.Sheets[0].RowCount; row++)
                        {
                            string batchYearNew = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 0].Tag).Trim();
                            string collegeCodeNew = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 0].Note).Trim();
                            string rollno = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 1].Text).Trim();
                            string degreeCodeNew = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 1].Tag).Trim();
                            string regno = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 2].Text).Trim();
                            string sectionNew = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 2].Tag).Trim();
                            string courseID = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 2].Note).Trim();
                            string admissionno = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 3].Text).Trim();
                            string studname = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 4].Text).Trim();
                            string semesterNew = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 5].Text).Trim();
                            string appNo = Convert.ToString(FpStudentDetails.Sheets[0].Cells[row, 5].Tag).Trim();
                            dr = dt.NewRow();
                            dr["Batch"] = batchYearNew;
                            dr["RollNo"] = rollno;
                            dr["RegNo"] = regno;
                            dr["DegreeCode"] = degreeCodeNew;
                            dr["Section"] = sectionNew;
                            dr["StudentName"] = studname;
                            dr["Semester"] = semesterNew;
                            dr["AdmissionNo"] = admissionno;
                            dr["AppNo"] = appNo;
                            dr["CollegeCode"] = collegeCodeNew;
                            dr["courseId"] = courseID;
                            dt.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        Init_Spread(FpStudentDetails, 1);
                        FpStudentDetails.Sheets[0].RowCount = 1;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].CellType = chkAll;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].Locked = false;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                        serialNo = 0;
                        FpStudentDetails.SaveChanges();
                    }
                    Init_Spread(FpStudentDetails, 1);
                    serialNo = 0;
                    FpStudentDetails.Sheets[0].RowCount = 1;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].CellType = chkAll;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].Locked = false;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, FpStudentDetails.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                    //for (int row = 0; row < dt.Rows.Count; row++)
                    foreach (DataRow drStudents in dt.Rows)
                    {
                        serialNo++;
                        string batchYearNew = Convert.ToString(drStudents["Batch"]).Trim();
                        string rollno = Convert.ToString(drStudents["RollNo"]).Trim();
                        string degreeCodeNew = Convert.ToString(drStudents["DegreeCode"]).Trim();
                        string regno = Convert.ToString(drStudents["RegNo"]).Trim();
                        string sectionNew = Convert.ToString(drStudents["Section"]).Trim();
                        string studname = Convert.ToString(drStudents["StudentName"]).Trim();
                        string semesterNew = Convert.ToString(drStudents["Semester"]).Trim();
                        string admissionno = Convert.ToString(drStudents["AdmissionNo"]);
                        string appNo = Convert.ToString(drStudents["appNo"]).Trim();
                        string collegeCodeNew = Convert.ToString(drStudents["CollegeCode"]).Trim();
                        string courseID = Convert.ToString(drStudents["courseId"]).Trim();
                        attendenace(Convert.ToString(degreeCodeNew).Trim(), Convert.ToString(semesterNew).Trim());
                        if (rbFullDay.Checked == true)
                        {
                            rbFullDay_CheckedChanged(new Object(), new EventArgs());
                        }
                        else if (rbHalfDay.Checked == true)
                        {
                            rbHalfDay_CheckedChanged(new Object(), new EventArgs());
                        }
                        else if (rbHourWise.Checked == true)
                        {
                            rbHourWise_CheckedChanged(new Object(), new EventArgs());
                        }
                        FpStudentDetails.Sheets[0].RowCount++;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialNo).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(batchYearNew).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(collegeCodeNew).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Locked = true;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(rollno).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(degreeCodeNew).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Value = Convert.ToString(rollno).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Locked = true;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(regno).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(sectionNew).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(courseID).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(admissionno).Trim();
                        //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(sectionNew).Trim();
                        //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(courseId).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(studname).Trim();
                        //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(sectionNew).Trim();
                        //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].Note = Convert.ToString(courseId).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].Locked = true;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(semesterNew).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(appNo).Trim();
                        //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Note = Convert.ToString(courseId).Trim();
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].CellType = txtCell;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Locked = true;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].CellType = chkSingleCell;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].Locked = false;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;

                    }
                    txtStudent.Text = string.Empty;
                    txtStudent.Focus();
                    FpStudentDetails.Sheets[0].PageSize = FpStudentDetails.Sheets[0].RowCount;
                    //FpStudentDetails.Width = 880;
                    FpStudentDetails.Height = 300;
                    FpStudentDetails.SaveChanges();
                }
                foreach (DataRow drStudents in dsStudentDetails.Tables[0].Rows)
                {
                    string rollNo = string.Empty;
                    string regNo = string.Empty;
                    string admitNo = string.Empty;
                    string appNo = string.Empty;
                    string studentName = string.Empty;
                    string collegeCodeNew = string.Empty;
                    string batchYearNew = string.Empty;
                    string degreeCodeNew = string.Empty;
                    string currentSemester = string.Empty;
                    string sectionNew = string.Empty;
                    string courseId = string.Empty;
                    serialNo++;
                    FpStudentDetails.Sheets[0].RowCount++;
                    rollNo = Convert.ToString(drStudents["roll_no"]).Trim();
                    regNo = Convert.ToString(drStudents["reg_no"]).Trim();
                    admitNo = Convert.ToString(drStudents["Roll_Admit"]).Trim();
                    appNo = Convert.ToString(drStudents["app_no"]).Trim();
                    studentName = Convert.ToString(drStudents["stud_name"]).Trim();
                    collegeCodeNew = Convert.ToString(drStudents["college_code"]).Trim();
                    batchYearNew = Convert.ToString(drStudents["batch_year"]).Trim();
                    degreeCodeNew = Convert.ToString(drStudents["degree_code"]).Trim();
                    currentSemester = Convert.ToString(drStudents["current_semester"]).Trim();
                    sectionNew = Convert.ToString(drStudents["sections"]).Trim();
                    courseId = Convert.ToString(drStudents["Course_Id"]).Trim();
                    attendenace(Convert.ToString(degreeCodeNew).Trim(), Convert.ToString(currentSemester).Trim());
                    if (rbFullDay.Checked == true)
                    {
                        rbFullDay_CheckedChanged(new Object(), new EventArgs());
                    }
                    else if (rbHalfDay.Checked == true)
                    {
                        rbHalfDay_CheckedChanged(new Object(), new EventArgs());
                    }
                    else if (rbHourWise.Checked == true)
                    {
                        rbHourWise_CheckedChanged(new Object(), new EventArgs());
                    }

                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialNo).Trim();
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(batchYearNew).Trim();
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(collegeCodeNew).Trim();
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].Locked = true;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(rollNo).Trim();
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(degreeCodeNew).Trim();
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Value = Convert.ToString(rollNo).Trim();
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].Locked = true;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(regNo).Trim();
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(sectionNew).Trim();
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(courseId).Trim();
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].Locked = true;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(admitNo).Trim();
                    //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(sectionNew).Trim();
                    //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(courseId).Trim();
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].Locked = true;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(studentName).Trim();
                    //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(sectionNew).Trim();
                    //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].Note = Convert.ToString(courseId).Trim();
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].Locked = true;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(currentSemester).Trim();
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(appNo).Trim();
                    //FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Note = Convert.ToString(courseId).Trim();
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].CellType = txtCell;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].Locked = true;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].CellType = chkSingleCell;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].Locked = false;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpStudentDetails.Sheets[0].Cells[FpStudentDetails.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                }
                FpStudentDetails.Sheets[0].PageSize = FpStudentDetails.Sheets[0].RowCount;
                //FpStudentDetails.Width = 880;
                FpStudentDetails.Height = 300;
                FpStudentDetails.SaveChanges();
            }
            else
            {
                FpStudentDetails.Sheets[0].RowCount = 0;
                FpStudentDetails.Sheets[0].PageSize = FpStudentDetails.Sheets[0].RowCount;
                //FpStudentDetails.Width = 880;
                FpStudentDetails.Height = 300;
                FpStudentDetails.SaveChanges();
                if (type == 0)
                {
                    lblODAlertMsg.Text = "No Record(s) Were Found.";
                }
                else
                {
                    lblODAlertMsg.Text = lblStudentOptions.Text.Trim() + " " + txtStudent.Text.Trim() + " is Not Available.";
                    txtStudent.Text = string.Empty;
                    txtStudent.Focus();
                    btnODPopAlertClose.Focus();
                }
                lblODAlertMsg.Visible = true;
                divPopODAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
        }
    }

    #region allstudentattendancereport new table

    protected void attendanceMark(string appNo, int mnthYear, string attDay, int noMaxHrsDay, int noFstHrsDay, int noSndHrsDay, int noMinFstHrsDay, int noMinSndHrsDay, string DateVal, string collegecode, int Attvalue)
    {
        try
        {
            DataSet dsload = new DataSet();
            Dictionary<int, int> AttValueMrng = new Dictionary<int, int>();
            Dictionary<int, int> AttvalueEve = new Dictionary<int, int>();
            double attVal = 0;
            int MPCnt = 0;
            int EPCnt = 0;
            int MnullCnt = 0;
            int EnullCnt = 0;
            string SelQ = " select " + attDay + ",A.ROLL_NO,r.app_no from attendance a,registration r where r.roll_no =a.roll_no and r.college_code='" + collegecode + "' AND month_year='" + mnthYear + "' and Att_App_no='" + appNo + "' ";
            dsload.Clear();
            dsload = da.select_method_wo_parameter(SelQ, "Text");
            if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
            {
                for (int sel = 0; sel < noMaxHrsDay; sel++)
                {
                    if (sel < noFstHrsDay)
                    {
                        double.TryParse(Convert.ToString(dsload.Tables[0].Rows[0][sel]), out attVal);
                        if (attVal != 0 || attVal != 0.0)
                        {
                            if (attVal == 1)
                                MPCnt++;
                            else
                            {
                                //  MOCnt = attVal;
                                if (!AttValueMrng.ContainsKey(Convert.ToInt32(attVal)))
                                    AttValueMrng.Add(Convert.ToInt32(attVal), 1);
                                else
                                {
                                    int Cnt = 0;
                                    int.TryParse(Convert.ToString(AttValueMrng[Convert.ToInt32(attVal)]), out Cnt);
                                    Cnt += 1;
                                    AttValueMrng.Remove(Convert.ToInt32(attVal));
                                    AttValueMrng.Add(Convert.ToInt32(attVal), Cnt);
                                }
                            }
                        }
                        else
                            MnullCnt++;
                    }
                    else if (sel >= noSndHrsDay)
                    {
                        double.TryParse(Convert.ToString(dsload.Tables[0].Rows[0][sel]), out attVal);
                        if (attVal != 0 || attVal != 0.0)
                        {
                            if (attVal == 1)
                                EPCnt++;
                            else
                            {
                                // EOCnt = attVal;
                                if (!AttvalueEve.ContainsKey(Convert.ToInt32(attVal)))
                                    AttvalueEve.Add(Convert.ToInt32(attVal), 1);
                                else
                                {
                                    int Cnt = 0;
                                    int.TryParse(Convert.ToString(AttvalueEve[Convert.ToInt32(attVal)]), out Cnt);
                                    Cnt += 1;
                                    AttvalueEve.Remove(Convert.ToInt32(attVal));
                                    AttvalueEve.Add(Convert.ToInt32(attVal), Cnt);
                                }
                            }
                        }
                        else
                            EnullCnt++;
                    }
                }
                int matt = attendanceSet(MPCnt, MnullCnt, noMinFstHrsDay, Attvalue, AttValueMrng);
                int eatt = attendanceSet(EPCnt, EnullCnt, noMinSndHrsDay, Attvalue, AttvalueEve);
                if (matt != null && eatt != null)
                {
                    string InsQ = " if exists (select * from AllStudentAttendanceReport where dateofattendance='" + DateVal + "' and appno='" + dsload.Tables[0].Rows[0]["app_no"] + "')update AllStudentAttendanceReport set mleavecode='" + matt + "',eleavecode='" + eatt + "' where  dateofattendance='" + DateVal + "' and appno='" + dsload.Tables[0].Rows[0]["app_no"] + "' else insert into AllStudentAttendanceReport(AppNo, DateofAttendance,MLeaveCode,ELeaveCode) values('" + dsload.Tables[0].Rows[0]["app_no"] + "','" + DateVal + "','" + matt + "','" + eatt + "')";
                    int save = da.update_method_wo_parameter(InsQ, "Text");
                }
            }
        }
        catch { }
    }

    protected int attendanceSet(int attCnt, int nullCnt, int hrCntCheck, int Attvalue, Dictionary<int, int> val)
    {
        int attVal = 0;
        try
        {
            //if (attCnt >= hrCntCheck)
            //    attVal = 1;
            //else if (nullCnt > 0)
            //    attVal = 0;
            //else
            //    attVal = Attvalue;
            if (attCnt >= hrCntCheck)
                attVal = 1;
            else if (nullCnt > 0)
                attVal = 0;
            else
            {
                val = val.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                foreach (KeyValuePair<int, int> txt in val)
                {
                    attVal = Convert.ToInt32(txt.Key);
                    break;
                }
            }
        }
        catch { }
        return attVal;
    }

    #endregion

    #endregion

    #region Reason

    protected void btnReasonDel_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlPurpose.Items.Count > 0)
            {
                string collegecode = Convert.ToString(ddlCollegeOD.SelectedValue).Trim();
                string reason = ddlPurpose.SelectedItem.ToString();
                if (reason.Trim().ToLower() != "all" && reason.Trim() != "")
                {
                    string strquery = "delete textvaltable where TextVal='" + reason + "' and TextCriteria='Attrs' and college_code='" + collegecode + "'";
                    int a = da.update_method_wo_parameter(strquery, "Text");
                    BindReason();
                }
            }
            divShowInFraction.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void btnReasonSet_Click(object sender, EventArgs e)
    {
        try
        {
            divShowInFraction.Visible = true;
            txtReason.Text = string.Empty;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void btnAddReason_Click(object sender, EventArgs e)
    {
        try
        {
            divShowInFraction.Visible = true;
            string collegecode = Convert.ToString(ddlCollegeOD.SelectedValue).Trim();
            string reason = txtReason.Text.ToString();
            if (reason.Trim() != "" && collegecode != "")
            {
                string strquery = "if not exists (select TextCode,Textval from textvaltable where TextCriteria='Attrs' and college_code='" + collegecode + "' and TextVal='" + reason + "' ) insert into textvaltable (TextVal,TextCriteria,college_code) values('" + reason + "','Attrs','" + collegecode + "')";
                int a = da.update_method_wo_parameter(strquery, "Text");
                txtReason.Text = string.Empty;
                BindReason();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void btnExitReason_Click(object sender, EventArgs e)
    {
        try
        {
            divShowInFraction.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

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
            lbl.Add(lblCollegeOD);
            lbl.Add(lbl_clgT);
            lbl.Add(lblDegree);
            lbl.Add(lblDegreeOD);
            lbl.Add(lblBranch);
            lbl.Add(lblBranchOD);
            lbl.Add(lblSem);
            lbl.Add(lblSemOD);
            fields.Add(0);
            fields.Add(0);
            fields.Add(0);
            fields.Add(2);
            fields.Add(2);
            fields.Add(3);
            fields.Add(3);
            fields.Add(4);
            fields.Add(4);
            if (institute != null && institute.TypeInstitute == 1)
            {
                lblBatch.Text = "Year";
                lblBatchOD.Text = "Year";
            }
            else
            {
                lblBatch.Text = "Batch";
                lblBatchOD.Text = "Batch";
            }
            new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
            return false;
        }
    }

    private DataSet GetSettings()
    {
        DataSet dsSettings = new DataSet();
        try
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
                string Master1 = "select distinct settings,value,ROW_NUMBER() over (ORDER BY settings DESC) as SetValue1,Case when settings='Admission No' then '1' when settings='Register No' then '2' when settings='Roll No' then '3' end as SetValue from Master_Settings where settings in('Roll No','Register No','Admission No') and value='1' " + grouporusercode + "";
                dsSettings = da.select_method(Master1, ht, "Text");
            }
            else
            {
                dsSettings.Clear();
                DataTable dt = new DataTable();
                dt.Columns.Clear();
                dt.Rows.Clear();
                dt.Columns.Add("settings");
                dt.Columns.Add("SetValue");
                dt.Rows.Add("Admission No", "1");
                dt.Rows.Add("Register No", "2");
                dt.Rows.Add("Roll No", "3");
                dsSettings.Tables.Add(dt);
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
        return dsSettings;
    }

    private bool CheckSchoolOrCollege(string collegeCode)
    {
        bool isSchoolOrCollege = false;
        try
        {
            if (!string.IsNullOrEmpty(collegeCode))
            {
                //qry = "select ISNULL(InstType,'0') as InstType,case when ISNULL(InstType,'0')='0' then 'College' when ISNULL(InstType,'0')='1' then 'School' end as CollegeOrSchool from collinfo where college_code='" + collegeCode + "'";
                string qry = "select ISNULL(InstType,'0') as InstType from collinfo where college_code='" + collegeCode + "'";
                string insType = da.GetFunction(qry).Trim();
                if (string.IsNullOrEmpty(insType) || insType.Trim() == "0")
                {
                    isSchoolOrCollege = false;
                }
                else if (!string.IsNullOrEmpty(insType) && insType.Trim() == "1")
                {
                    isSchoolOrCollege = true;
                }
                else
                {
                    isSchoolOrCollege = false;
                }
            }
            return isSchoolOrCollege;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
            return false;
        }
    }

    public string GetAttendanceStatusName(string attStatusCode)
    {
        string attendanceStatus = string.Empty;
        attStatusCode = attStatusCode.Trim();
        switch (attStatusCode)
        {
            case "1":
                attendanceStatus = "P";
                break;
            case "2":
                attendanceStatus = "A";
                break;
            case "3":
                attendanceStatus = "OD";
                break;
            case "4":
                attendanceStatus = "ML";
                break;
            case "5":
                attendanceStatus = "SOD";
                break;
            case "6":
                attendanceStatus = "NSS";
                break;
            case "7":
                attendanceStatus = "H";
                break;
            case "8":
                attendanceStatus = "NJ";
                break;
            case "9":
                attendanceStatus = "S";
                break;
            case "10":
                attendanceStatus = "L";
                break;
            case "11":
                attendanceStatus = "NCC";
                break;
            case "12":
                attendanceStatus = "HS";
                break;
            case "13":
                attendanceStatus = "PP";
                break;
            case "14":
                attendanceStatus = "SYOD";
                break;
            case "15":
                attendanceStatus = "COD";
                break;
            case "16":
                attendanceStatus = "OOD";
                break;
            case "17":
                attendanceStatus = "LA";
                break;
            default:
                attendanceStatus = string.Empty;
                break;
        }
        return attendanceStatus;
    }

    public string GetAttendanceStatusCode(string attStatusCode)
    {
        string attendanceStatus = string.Empty;
        attStatusCode = attStatusCode.Trim().ToUpper();
        switch (attStatusCode)
        {
            case "P":
                attendanceStatus = "1";
                break;
            case "A":
                attendanceStatus = "2";
                break;
            case "OD":
                attendanceStatus = "3";
                break;
            case "ML":
                attendanceStatus = "4";
                break;
            case "SOD":
                attendanceStatus = "5";
                break;
            case "NSS":
                attendanceStatus = "6";
                break;
            case "H":
                attendanceStatus = "7";
                break;
            case "NJ":
                attendanceStatus = "8";
                break;
            case "S":
                attendanceStatus = "9";
                break;
            case "L":
                attendanceStatus = "10";
                break;
            case "NCC":
                attendanceStatus = "11";
                break;
            case "HS":
                attendanceStatus = "12";
                break;
            case "PP":
                attendanceStatus = "13";
                break;
            case "SYOD":
                attendanceStatus = "14";
                break;
            case "COD":
                attendanceStatus = "15";
                break;
            case "OOD":
                attendanceStatus = "16";
                break;
            case "LA":
                attendanceStatus = "17";
                break;
            default:
                attendanceStatus = string.Empty;
                break;
        }
        return attendanceStatus;
    }

    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        try
        {
            IDictionaryEnumerator e = hashTable.GetEnumerator();
            while (e.MoveNext())
            {
                if (e.Key.ToString() == key.ToString())
                {
                    return e.Value;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
        return null;
    }

}
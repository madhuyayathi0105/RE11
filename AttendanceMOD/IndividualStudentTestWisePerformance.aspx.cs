using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BalAccess;
using InsproDataAccess;
using Farpoint = FarPoint.Web.Spread;
using wc = System.Web.UI.WebControls;

public partial class MarkMod_IndividualStudentTestWisePerformance : System.Web.UI.Page
{
    #region Field Declaration

    DAccess2 da = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DataSet ds = new DataSet();
    Hashtable ht = new Hashtable();

    DataTable dtCommon = new DataTable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();

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
    string testName = string.Empty;
    string testNo = string.Empty;
    string subjectName = string.Empty;
    string subjectNo = string.Empty;
    string subjectCode = string.Empty;
    string sections = string.Empty;

    string orderBy = string.Empty;
    string orderBySetting = string.Empty;

    string qry = string.Empty;
    string qryCollegeCode = string.Empty;
    string qryCollegeCode1 = string.Empty;
    string qryBatchYear = string.Empty;
    string qryDegreeCode = string.Empty;
    string qrySemester = string.Empty;
    string qrySection = string.Empty;
    string qryCourseId = string.Empty;
    string qrytestNo = string.Empty;
    string qrytestName = string.Empty;
    string qrySubjectNo = string.Empty;
    string qrySubjectName = string.Empty;
    string qrySubjectCode = string.Empty;

    int selectedCount = 0;

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
                setLabelText();
                Bindcollege();
                BindBatch();
                BindDegree();
                bindbranch();
                bindsem();
                BindSectionDetail();
                //divStudentDetail.Visible = false;


            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region bindMethod

    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            dtCommon.Clear();
            ddlCollege.Enabled = false;
            DataSet dsprint = new DataSet();
            dsprint.Clear();
            string qryUserCodeOrGroupCode = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
            {
                qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
            {
                qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(qryUserCodeOrGroupCode))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("column_field", Convert.ToString(qryUserCodeOrGroupCode));
                dtCommon = storeAcc.selectDataTable("bind_college", dicQueryParameter);
            }
            if (dtCommon.Rows.Count > 0)
            {
                ddlCollege.DataSource = dtCommon;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
                ddlCollege.Enabled = true;
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
            ddlBatch.Items.Clear();
            string Master1 = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                Master1 = group.Split(';')[0];
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = Convert.ToString(group_semi[0]).Trim();
                }
            }
            else
            {
                Master1 = Convert.ToString(Session["usercode"]).Trim();
            }
            string collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(Master1.Trim()) && !string.IsNullOrEmpty(collegecode))
            {
                string strbinddegree = "select distinct batch_year from tbl_attendance_rights where college_code='" + collegecode + "'";
                //user_id='" + Master1 + "' and 
                ds = da.select_method_wo_parameter(strbinddegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "Batch_year";
                ddlBatch.DataValueField = "Batch_year";
                ddlBatch.DataBind();
                ddlBatch.SelectedIndex = ddlBatch.Items.Count - 1;
            }
        }
        catch
        {
        }
    }

    public void BindDegree()
    {
        string college_code = Convert.ToString(ddlCollege.SelectedValue).Trim();
        string query = string.Empty;
        ddlDegree.Items.Clear();
        string usercode = Convert.ToString(Session["usercode"]).Trim();
        string singleuser = Convert.ToString(Session["single_user"]).Trim();
        string group_user = Convert.ToString(Session["group_code"]).Trim();
        if (group_user.Contains(";"))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(group_user).Trim() != "0") && (Convert.ToString(group_user).Trim() != "-1"))
        {
            query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "' ";
        }
        else
        {
            query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code='" + college_code + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' ";
        }
        DataSet ds = new DataSet();
        ds.Clear();
        ds = da.select_method_wo_parameter(query, "Text");
        // DataSet ds = ClsAttendanceAccess.GetDegreeDetail(collegecode.ToString());
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlDegree.DataSource = ds;
            ddlDegree.DataValueField = "Course_Id";
            ddlDegree.DataTextField = "Course_Name";
            ddlDegree.DataBind();
            // ddlDegree.Items.Insert(0, new ListItem("--Select--", "-1"));
        }
    }

    public void bindbranch()
    {
        try
        {
            DataSet ds = new DataSet();
            ds.Clear();
            ddlBranch.Items.Clear();
            ht.Clear();
            string usercode = Convert.ToString(Session["usercode"]).Trim();
            string collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            string singleuser = Convert.ToString(Session["single_user"]).Trim();
            string group_user = Convert.ToString(Session["group_code"]).Trim();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }
            string course_id = string.Empty;// ddlDegree.SelectedValue.ToString();
            if (ddlDegree.Items.Count > 0)
            {
                course_id = Convert.ToString(ddlDegree.SelectedValue).Trim();
                string query = string.Empty;
                if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(group_user).Trim() != "0") && (Convert.ToString(group_user).Trim() != "-1"))
                {
                    query = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id='" + course_id + "' and degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "'";
                }
                else
                {
                    query = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id='" + course_id + "' and degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' ";
                }
                ds = da.select_method_wo_parameter(query, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count2 = ds.Tables[0].Rows.Count;
                if (count2 > 0)
                {
                    ddlBranch.DataSource = ds;
                    ddlBranch.DataTextField = "dept_name";
                    ddlBranch.DataValueField = "degree_code";
                    ddlBranch.DataBind();
                }
            }
        }
        catch
        {
        }
    }

    public void bindsem()
    {
        ddlSem.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        string query = string.Empty;
        DataSet ds = new DataSet();
        if (ddlBatch.Items.Count > 0 && ddlBranch.Items.Count > 0 && Session["collegecode"] != null)
        {
            query = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and batch_year='" + Convert.ToString(ddlBatch.SelectedItem.Text).Trim() + "' and college_code='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
            int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlSem.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlSem.Items.Add(i.ToString());
                }
            }
        }
        else
        {
            if (ddlBranch.Items.Count > 0 && Session["collegecode"] != null)
            {
                query = "select distinct duration,first_year_nonsemester  from degree where degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and college_code='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'";
                ddlSem.Items.Clear();
                ds = new DataSet();
                ds.Clear();
                ds = da.select_method_wo_parameter(query, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSem.Items.Add(i.ToString());
                    }
                }
            }
        }
        if (ddlSem.Items.Count > 0)
        {
            ddlSem.SelectedIndex = 0;
            BindSectionDetail();
        }
    }

    public void BindSectionDetail()
    {
        DataSet ds = new DataSet();
        ddlSec.Items.Clear();
        if (ddlBranch.Items.Count > 0 && ddlBatch.Items.Count > 0)
        {
            string branch = Convert.ToString(ddlBranch.SelectedValue).Trim();
            string batch = Convert.ToString(ddlBatch.SelectedValue).Trim();
            string query = "select distinct sections from registration where batch_year='" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "' and degree_code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlSec.DataSource = ds;
            ddlSec.DataTextField = "sections";
            ddlSec.DataValueField = "sections";
            ddlSec.DataBind();
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToString(ds.Tables[0].Rows[0]["sections"]).Trim() == "")
            {
                ddlSec.Enabled = false;

            }
            else
            {
                ddlSec.Enabled = true;
            }
        }
        else
        {
            ddlSec.Enabled = false;

        }
    }

    public void Get_Semester()
    {
        bool first_year;
        first_year = false;
        int duration = 0;
        string batch_calcode_degree;
        ddlSem.Items.Clear();
        //int typeval = 4;
        if (ddlBatch.Items.Count > 0 && ddlBranch.Items.Count > 0 && Session["collegecode"] != null)
        {
            string batch = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
            string collegecode = Convert.ToString(Session["collegecode"]).Trim();
            string degree = Convert.ToString(ddlBranch.SelectedValue).Trim();
            batch_calcode_degree = batch.ToString() + "/" + collegecode.ToString() + "/" + degree.ToString();
            //Session["collegecode"].ToString();
            DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                //duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                for (int i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSem.Items.Add(i.ToString());
                    }
                }
                //ddlSemYr.Items.Insert(0, new ListItem("- -Select- -", "-1"));
            }
        }
    }

    public void bindTest()
    {
        try
        {
            string Query = " select distinct Criteria,criteria_no from CriteriaForInternal C,syllabus_Master sy where sy.syll_code=c.syll_code and sy.degree_code ='" + ddlBranch.SelectedValue + "' and sy.batch_year='" + ddlBatch.SelectedItem.Text + "' and sy.semester ='" + ddlSem.SelectedItem.Text + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlTest.DataSource = ds;
                ddlTest.DataTextField = "Criteria";
                ddlTest.DataValueField = "criteria_no";
                ddlTest.DataBind();
            }
        }
        catch
        {

        }
    }

    public void HeaderBind()
    {
        try
        {
            attnd_report.CommandBar.Visible = false;
            attnd_report.Sheets[0].RowHeader.Visible = false;
            attnd_report.Sheets[0].RowCount = 0;
            attnd_report.Sheets[0].ColumnCount = 0;
            attnd_report.Sheets[0].AutoPostBack = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            attnd_report.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            int Wid = 0;
            attnd_report.Sheets[0].ColumnCount++;
            attnd_report.Sheets[0].ColumnHeader.Cells[0, attnd_report.Sheets[0].ColumnCount - 1].Text = "S.No";
            attnd_report.Sheets[0].ColumnHeader.Cells[0, attnd_report.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            attnd_report.Columns[0].Width = 50;
            attnd_report.Columns[0].Locked = true;
            Wid += 50;

            attnd_report.Sheets[0].ColumnCount++;
            attnd_report.Sheets[0].ColumnHeader.Cells[0, attnd_report.Sheets[0].ColumnCount - 1].Text = "Roll No";
            attnd_report.Sheets[0].ColumnHeader.Cells[0, attnd_report.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            attnd_report.Columns[attnd_report.Sheets[0].ColumnCount - 1].Visible = false;
            attnd_report.Columns[1].Width = 100;
            attnd_report.Columns[1].Locked = true;

            attnd_report.Sheets[0].ColumnCount++;
            attnd_report.Sheets[0].ColumnHeader.Cells[0, attnd_report.Sheets[0].ColumnCount - 1].Text = "Register No";
            attnd_report.Sheets[0].ColumnHeader.Cells[0, attnd_report.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            attnd_report.Columns[attnd_report.Sheets[0].ColumnCount - 1].Visible = false;
            attnd_report.Columns[2].Width = 100;
            attnd_report.Columns[2].Locked = true;

            attnd_report.Sheets[0].ColumnCount++;
            attnd_report.Sheets[0].ColumnHeader.Cells[0, attnd_report.Sheets[0].ColumnCount - 1].Text = "Admission No";
            attnd_report.Sheets[0].ColumnHeader.Cells[0, attnd_report.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            attnd_report.Columns[attnd_report.Sheets[0].ColumnCount - 1].Visible = false;
            attnd_report.Columns[3].Width = 100;
            attnd_report.Columns[3].Locked = true;

            attnd_report.Sheets[0].ColumnCount++;
            attnd_report.Sheets[0].ColumnHeader.Cells[0, attnd_report.Sheets[0].ColumnCount - 1].Text = "Student Name";
            attnd_report.Sheets[0].ColumnHeader.Cells[0, attnd_report.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            attnd_report.Columns[4].Width = 200;
            attnd_report.Columns[4].Locked = true;
            Wid += 200;

            attnd_report.Sheets[0].ColumnCount++;
            attnd_report.Sheets[0].ColumnHeader.Cells[0, attnd_report.Sheets[0].ColumnCount - 1].Text = "Select";
            attnd_report.Sheets[0].ColumnHeader.Cells[0, attnd_report.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            attnd_report.Columns[5].Width = 100;
            attnd_report.Columns[5].Locked = false;
            Wid += 100;
            ds.Clear();
            ds = GetSettings();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int intds = 0; intds < ds.Tables[0].Rows.Count; intds++)
                {
                    string Value = Convert.ToString(ds.Tables[0].Rows[intds]["SetValue"]);
                    if (Value.Trim() == "1")
                    {
                        attnd_report.Columns[3].Visible = true;
                        Wid += 100;
                    }
                    else if (Value.Trim() == "2")
                    {
                        attnd_report.Columns[2].Visible = true;
                        Wid += 100;
                    }
                    else if (Value.Trim() == "3")
                    {
                        attnd_report.Columns[1].Visible = true;
                        Wid += 100;
                    }
                }
            }
            attnd_report.Width = Wid + 50;
        }
        catch
        {

        }
    }

    #endregion

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
                dsSettings = dirAcc.selectDataSet(Master1);
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
        return dsSettings;
    }

    #region Index Changed Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string studentApplicationNo = string.Empty;
            ShowReport.Visible = false;
            btnPrint.Visible = false;
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            bindTest();

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
            ShowReport.Visible = false;
            btnPrint.Visible = false;
            string studentApplicationNo = string.Empty;

            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
            bindTest();

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
            ShowReport.Visible = false;
            btnPrint.Visible = false;
            string studentApplicationNo = string.Empty;

            bindbranch();
            bindsem();
            BindSectionDetail();
            bindTest();

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
            ShowReport.Visible = false;
            btnPrint.Visible = false;
            string studentApplicationNo = string.Empty;

            bindsem();
            BindSectionDetail();
            bindTest();

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
            ShowReport.Visible = false;
            btnPrint.Visible = false;
            string studentApplicationNo = string.Empty;
            BindSectionDetail();
            bindTest();
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
            ShowReport.Visible = false;
            btnPrint.Visible = false;
            string studentApplicationNo = string.Empty;
            bindTest();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlTest_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ShowReport.Visible = false;
            btnPrint.Visible = false;
            string studentApplicationNo = string.Empty;

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void chkTest_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            CallCheckboxChange(chkTest, cblTest, txtTest, lblTest.Text, "--Select--");
            string studentApplicationNo = string.Empty;

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void cblTest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            CallCheckboxListChange(chkTest, cblTest, txtTest, lblTest.Text, "--Select--");
            string studentApplicationNo = string.Empty;

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void attnd_report_Command_Click(object sender, EventArgs e)
    {
        try
        {
            if (attnd_report.Sheets[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(attnd_report.Sheets[0].Cells[0, 5].Value) == 1)
                {
                    for (int intF = 1; intF < attnd_report.Sheets[0].Rows.Count; intF++)
                    {
                        attnd_report.Sheets[0].Cells[intF, 5].Value = 1;
                    }
                }
                else
                {
                    for (int intF = 1; intF < attnd_report.Sheets[0].Rows.Count; intF++)
                    {
                        attnd_report.Sheets[0].Cells[intF, 5].Value = 0;
                    }
                }
            }
        }
        catch
        {

        }
    }

    #endregion Index Changed Events

    #region Button Events

    #region Get Students Details

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            string CollegeCode = ddlCollege.SelectedValue;
            string batchYear = ddlBatch.SelectedItem.Text;
            string degreeCode = ddlBranch.SelectedValue;
            string semester = ddlSem.SelectedItem.Text;
            string sections = string.Empty;
            string TestName = string.Empty;
            HeaderBind();
            if (ddlSec.Items.Count > 0)
            {
                sections = ddlSec.SelectedItem.Text;
            }
            if (ddlTest.Items.Count > 0)
            {
                TestName = ddlTest.SelectedItem.Text;
            }
            string Query = "select r.roll_no,r.reg_no,r.stud_name,r.roll_admit,r.app_no from registration r where cc=0 and delflag=0 and exam_flag<>'DEBAR' and  degree_code ='" + degreeCode + "' and college_code ='" + CollegeCode + "' and batch_year='" + batchYear + "' and current_semester='" + semester + "'";
            if (sections.Trim() != "")
            {
                Query += " and sections='" + sections + "'";
            }
            //Query += " order by roll_no";// By Malang Raja to Order By Settings
            Query += " " + orderByStudents(CollegeCode, "r");
            ds.Clear();
            ds = da.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int Sno = 0;
                FarPoint.Web.Spread.CheckBoxCellType cb = new Farpoint.CheckBoxCellType();
                cb.AutoPostBack = true;

                FarPoint.Web.Spread.CheckBoxCellType cbsub = new Farpoint.CheckBoxCellType();
                FarPoint.Web.Spread.TextCellType txt = new Farpoint.TextCellType();
                cbsub.AutoPostBack = false;

                attnd_report.Sheets[0].RowCount++;
                attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 5].CellType = cb;
                attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;

                for (int intds = 0; intds < ds.Tables[0].Rows.Count; intds++)
                {
                    Sno++;
                    attnd_report.Sheets[0].RowCount++;
                    attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 0].Text = Sno.ToString();
                    attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                    attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[intds]["roll_no"]);
                    attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 1].CellType = txt;
                    attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[intds]["app_no"]);
                    attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[intds]["reg_no"]);
                    attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 2].CellType = txt;
                    attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[intds]["roll_admit"]);
                    attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 3].CellType = txt;
                    attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[intds]["stud_Name"]);
                    attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;

                    attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 5].CellType = cbsub;
                    attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                }
                if (attnd_report.Sheets[0].RowCount > 0)
                {
                    ShowReport.Visible = true;
                    btnPrint.Visible = true;
                    attnd_report.Sheets[0].PageSize = attnd_report.Sheets[0].RowCount;
                    attnd_report.Height = 700;
                }
            }
            else
            {
                divPopAlert.Visible = true;
                ShowReport.Visible = false;
                btnPrint.Visible = false;
                lblAlertMsg.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    #endregion

    #region Alert Popup Close

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

    #endregion

    #region Confirmation Yes/No Click

    protected void btnYes_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirmBox.Visible = false;
        }
        catch
        {
        }
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        try
        {
            divConfirmBox.Visible = false;
        }
        catch
        {
        }
    }

    #endregion

    #region PDF Generation

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtSubjectStrength = new DataTable();
            DataTable dtSubjectTotal = new DataTable();
            DataTable dtSubjectMaxMark = new DataTable();
            string TextCode = Convert.ToString(ddlTest.SelectedValue);
            string GetValueQuery = "SELECT Count(re.roll_no) as Count,e.subject_no FROM CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code  and c.criteria_no='" + TextCode + "' and marks_obtained >=0";
            if (ddlSec.Items.Count > 0)
            {
                GetValueQuery += "  and e.sections ='" + ddlSec.SelectedItem.Text + "' ";
            }
            GetValueQuery += " group by e.subject_no";

            GetValueQuery += " SELECT Max(re.marks_obtained) as Count,e.subject_no FROM CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and c.criteria_no='" + TextCode + "' and marks_obtained >=0";
            if (ddlSec.Items.Count > 0)
            {
                GetValueQuery += "  and e.sections ='" + ddlSec.SelectedItem.Text + "' ";
            }
            GetValueQuery += " group by e.subject_no";
            GetValueQuery += " SELECT Sum(re.marks_obtained) as Count,e.subject_no FROM CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and c.criteria_no='" + TextCode + "' and marks_obtained >=0";
            if (ddlSec.Items.Count > 0)
            {
                GetValueQuery += "  and e.sections ='" + ddlSec.SelectedItem.Text + "' ";
            }
            GetValueQuery += " group by e.subject_no";
            ds.Clear();
            ds = da.select_method_wo_parameter(GetValueQuery, "Text");
            batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
            collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            degreeCode = Convert.ToString(ddlBranch.SelectedValue).Trim();
            semester = Convert.ToString(ddlSem.SelectedValue).Trim();
            testName = Convert.ToString(ddlTest.SelectedItem.Text).Trim();
            qry = "select gm.College_Code,gm.batch_year,gm.Degree_Code,ISNULL(gm.Semester,'0') as Semester,gm.Credit_Points,gm.Mark_Grade,gm.Frange,gm.Trange,LTRIM(RTRIM(ISNULL(gm.Criteria,''))) as Criteria,gm.classify,CONVERT(Varchar(50),gm.Frange)+' - '+CONVERT(Varchar(50),gm.Trange) as Ranges from Grade_Master gm where gm.batch_year='" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "' and gm.College_Code='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "' and gm.Degree_Code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and ISNULL(gm.Semester,'0')='" + Convert.ToString(ddlSem.SelectedValue).Trim() + "' union select gm.College_Code,gm.batch_year,gm.Degree_Code,ISNULL(gm.Semester,'0') as Semester,gm.Credit_Points,gm.Mark_Grade,gm.Frange,gm.Trange,LTRIM(RTRIM(ISNULL(gm.Criteria,''))) as Criteria,gm.classify,CONVERT(Varchar(50),gm.Frange)+' - '+CONVERT(Varchar(50),gm.Trange) as Ranges from Grade_Master gm where gm.batch_year='" + Convert.ToString(ddlBatch.SelectedValue).Trim() + "' and gm.College_Code='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "' and gm.Degree_Code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "' and ISNULL(gm.Semester,'0')='0' order by gm.College_Code,gm.batch_year,gm.Degree_Code,gm.Semester,gm.Criteria,gm.Trange desc,gm.Frange desc";
            DataTable dtGradeDetails = dirAcc.selectDataTable(qry);
            DataTable dtGeneralGrade = new DataTable();
            if (dtGradeDetails.Rows.Count > 0)
            {
                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + semester + "' and Criteria='General'";
                dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
            }
            if (dtGeneralGrade.Rows.Count == 0)
            {
                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Criteria='General'";
                dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
            }
            if (dtGeneralGrade.Rows.Count == 0)
            {
                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='" + semester + "' and Criteria=''";
                dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
            }
            if (dtGeneralGrade.Rows.Count == 0)
            {
                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batchYear + "' and College_Code='" + collegeCode + "' and Degree_Code='" + degreeCode + "' and Semester='0' and Criteria=''";
                dtGeneralGrade = dtGradeDetails.DefaultView.ToTable(true, "Mark_Grade", "Frange", "Trange", "Ranges");
            }

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dtSubjectStrength = ds.Tables[0].DefaultView.ToTable();
                dtSubjectTotal = ds.Tables[2].DefaultView.ToTable();
                dtSubjectMaxMark = ds.Tables[1].DefaultView.ToTable();

                StringBuilder SbHtml = new StringBuilder();
                string App_no = string.Empty;
                string StudName = string.Empty;
                string RollNo = string.Empty;
                string Section = string.Empty;
                bool check = false;
                bool checkNewCheck = false;
                bool SelectFalg = false;
                if (ddlSec.Items.Count > 0)
                {
                    Section = ddlSec.SelectedItem.Text;
                }
                attnd_report.SaveChanges();
                DataTable DtRank = RankCalculation(TextCode);
                for (int intFp = 1; intFp < attnd_report.Sheets[0].RowCount; intFp++)
                {
                    int Val = Convert.ToInt32(attnd_report.Sheets[0].Cells[intFp, 5].Value);
                    if (Val == 1)
                    {
                        SelectFalg = true;
                        App_no = Convert.ToString(attnd_report.Sheets[0].Cells[intFp, 1].Tag);
                        StudName = Convert.ToString(attnd_report.Sheets[0].Cells[intFp, 4].Text);
                        RollNo = Convert.ToString(attnd_report.Sheets[0].Cells[intFp, 1].Text);
                        checkNewCheck = StudentMarkDetails(App_no, SbHtml, TextCode, StudName, dtSubjectMaxMark, dtSubjectStrength, dtSubjectTotal, dtGradeDetails, DtRank, Section, RollNo.Trim());
                        if (checkNewCheck == true)
                        {
                            check = true;
                        }
                    }
                }
                if (SelectFalg && !check)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Test Mark Not Entered";
                }
                else if (!SelectFalg)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Please Select Any One Students";
                }
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Test Mark Not Entered";
            }

        }
        catch
        {

        }
    }

    public bool StudentMarkDetails(string App_no, StringBuilder SbHtml, string TestCode, string Studname, DataTable dtSubjectMaxMark, DataTable dtSubjectStrength, DataTable dtSubjectTotal, DataTable dtGradeDetails, DataTable DtRank, string Section, string RollNo)
    {
        bool ConditionFlag = false;
        try
        {
            DataView dvsubstrength = new DataView();
            DataView dvsubMax = new DataView();
            DataView dvsubTotal = new DataView();
            DataTable dtSingleSubject = new DataTable();
            DataTable dtMultiSubject = new DataTable();
            DataTable dtSubjectPriority = new DataTable();
            double subStrenth = 0;
            double SubMax = 0;
            double subTotal = 0;
            double subjectMark = 0;
            double ConvertionOutofMark = 0;
            double SubjectMaxMark = 0;
            double convertionSubjectMaxMArk = 0;

            double ConvertMark = 0;
            double convertAvarageMark = 0;
            double ConverhighestMark = 0;

            double TotalSecureMark = 0;
            double TotalMaxMark = 0;
            string Acrdemicyear = da.GetFunction("select value from master_settings where settings='Academic year'");
            string[] split = Acrdemicyear.Split(',');
            string Acr = da.GetFunction("select acr from collinfo where college_code ='" + ddlCollege.SelectedValue + "'");

            string Query = "SELECT r.App_no,r.Roll_no,r.college_Code,r.Reg_No,r.Batch_Year,r.degree_Code,r.current_semester,sm.semester,c.Criteria_no as TestNo,c.criteria as TestName,c.min_mark as TestMinMark,c.max_mark as TestMaxMark,s.subject_code,s.subject_name,s.subjectpriority,s.subject_no,s.min_int_marks as SubjectMinINT,s.max_int_marks as SubjectMaxINT,s.min_ext_marks as SubjectMinEXT,s.max_ext_marks as SubjectMaxEXT,s.mintotal as SubjectMinTotal,s.maxtotal as SubjectMaxTotal,e.exam_code,e.min_mark as ConductedMinMark,e.max_mark as ConductedMaxMark,ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'') as TestMark,ISNULL(CONVERT(VARCHAR(100),re.Retest_Marks_obtained),'') as RetestMark,ISNULL(ss.isSingleSubject,'0') as isSingleSubject,ss.subject_type,ss.subType_no FROM CriteriaForInternal c,Exam_type e,Result re,registration r,syllabus_master sm,subject s,sub_sem ss where ss.subType_no=s.subType_no and s.subject_no=e.subject_no and s.syll_code=sm.syll_code and s.syll_code=c.syll_code and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and r.Batch_Year=sm.Batch_Year and r.degree_Code=sm.degree_code and r.current_semester=sm.semester and r.Roll_no=re.roll_no and LTRIM(RTRIM(ISNULL(e.sections,'')))=LTRIM(RTRIM(ISNULL(r.sections,''))) and r.App_no='" + App_no + "' and c.criteria_no='" + TestCode + "'  order by r.App_no,s.subjectpriority,s.subject_code";
            ds.Clear();
            ds = da.select_method_wo_parameter(Query, "Text");
            if (ddlSec.Items.Count > 0)//saran
            {
                if (!string.IsNullOrEmpty(Convert.ToString(ddlSec.SelectedValue).Trim()) && Convert.ToString(ddlSec.SelectedValue).Trim().ToLower() != "all" && Convert.ToString(ddlSec.SelectedValue).Trim().ToLower() != "-1")
                {
                    qrySection = "  and LTRIM(RTRIM(ISNULL(e.sections,'')))='" + Convert.ToString(ddlSec.SelectedValue).Trim() + "'";
                }
            }

            DataTable dtSubSubjectMarkList = new DataTable();
            DataTable dtSubSubjectMarkDetails = new DataTable();
            string qry2 = "select distinct s.subjectId, s.subSubjectName,su.subject_no,ss.subType_no,ss.isSingleSubject,ss.subject_type from subsubjectTestDetails s,subSubjectWiseMarkEntry sm,Exam_type e,subject su,sub_sem ss  where s.subjectId=sm.subjectId and s.examCode=e.exam_code and su.syll_code=ss.syll_code and ss.subType_no=su.subType_no and su.subject_no=e.subject_no and criteria_no='" + TestCode + "' " + qrySection;
            dtSubSubjectMarkList = dirAcc.selectDataTable(qry2);

            qry2 = "select distinct s.subjectId,s.subSubjectName,s.maxMark,s.minMark,ss.subType_no,ss.isSingleSubject,ss.subject_type,su.subject_no,e.criteria_no,sm.appNo,sm.testMark,ISNULL(sm.ReTestMark,'0') as ReTestMark,sm.remarks from subsubjectTestDetails s,subSubjectWiseMarkEntry sm,Exam_type e,subject su,sub_sem ss  where s.subjectId=sm.subjectId and s.examCode=e.exam_code and su.syll_code=ss.syll_code and ss.subType_no=su.subType_no and su.subject_no=e.subject_no and sm.appNo='" + App_no + "' and e.criteria_no='" + TestCode + "'" + qrySection;
            dtSubSubjectMarkDetails = dirAcc.selectDataTable(qry2);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string batch = Convert.ToString(ds.Tables[0].Rows[0]["Batch_Year"]).Trim();
                string college = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]).Trim();
                string degree = Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]).Trim();
                string sems = Convert.ToString(ds.Tables[0].Rows[0]["semester"]).Trim();
                string testNames = Convert.ToString(ds.Tables[0].Rows[0]["TestName"]).Trim();
                string testNos = Convert.ToString(ds.Tables[0].Rows[0]["TestNo"]).Trim();

                ds.Tables[0].DefaultView.RowFilter = "isSingleSubject='False'";
                dtSingleSubject = ds.Tables[0].DefaultView.ToTable();
                dtSubjectPriority = ds.Tables[0].DefaultView.ToTable(true, "subjectpriority", "subject_no");

                ds.Tables[0].DefaultView.RowFilter = "isSingleSubject='True'";
                dtMultiSubject = ds.Tables[0].DefaultView.ToTable();

                DataRow dr;
                DataView DvSubTypeSubject = new DataView();
                DataTable dtSubjectType = dtMultiSubject.DefaultView.ToTable(true, "subject_Type", "SubType_no");
                if (dtSubjectType.Rows.Count > 0)
                {
                    for (int intST = 0; intST < dtSubjectType.Rows.Count; intST++)
                    {
                        dtMultiSubject.DefaultView.RowFilter = "SubType_no='" + Convert.ToString(dtSubjectType.Rows[intST]["SubType_no"]) + "'";
                        DvSubTypeSubject = dtMultiSubject.DefaultView;
                        DvSubTypeSubject.Sort = "subjectpriority asc";
                        if (DvSubTypeSubject.Count > 0)
                        {
                            dr = dtSubjectPriority.NewRow();
                            dr[0] = Convert.ToString(DvSubTypeSubject[0]["subjectpriority"]);
                            dr[1] = Convert.ToString(DvSubTypeSubject[0]["SubType_no"]);
                            dtSubjectPriority.Rows.Add(dr);
                        }
                    }
                }
                DvSubTypeSubject = dtSubjectPriority.DefaultView;
                DvSubTypeSubject.Sort = "subjectpriority asc";
                dtSubjectPriority = DvSubTypeSubject.ToTable();
                ConditionFlag = true;

                #region I Page

                SbHtml.Append("<div style='height:845px; width: 655px; border:1px solid black; margin:0px; margin-left: 5px;page-break-after: always;'>");

                #region Header

                SbHtml.Append("<div style='width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
                SbHtml.Append("<table cellspacing='0' cellpadding='5' style='width: 645px; font-weight: bold;'>");
                SbHtml.Append("<tr style='text-align:center;'>");
                SbHtml.Append("<td>");
                SbHtml.Append("<span>" + ddlCollege.SelectedItem.Text.Trim().ToUpper() + "</span>");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("<tr style='text-align:center;'>");
                SbHtml.Append("<td>");
                SbHtml.Append("<span>(Affiliated to " + Acr + ")</span>");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("<tr style='text-align:right;'>");
                SbHtml.Append("<td>");
                SbHtml.Append("<span>DATE: " + DateTime.Now.ToString("dd/MM/yyyy") + "</span>");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("<tr style='text-align:center;'>");
                SbHtml.Append("<td>");
                SbHtml.Append("<span>ACADEMIC PERFORMANCE</span>");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("</tr>");
                SbHtml.Append("<tr style='text-align:center;'>");
                SbHtml.Append("<td>");

                SbHtml.Append("<span>" + ddlTest.SelectedItem.Text.Trim().ToUpper() + "</span>");

                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("<tr style='text-align:center;'>");
                SbHtml.Append("<td>");
                SbHtml.Append("<span>" + split[0] + " - " + split[1] + "</span>");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("</table>");
                SbHtml.Append("</div>");

                #endregion

                #region Student Details

                SbHtml.Append("<div style='width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
                SbHtml.Append("<table cellspacing='0' cellpadding='5' style='width: 645px; font-weight: bold;'>");
                SbHtml.Append("<tr>");

                SbHtml.Append("<td>");
                SbHtml.Append("<span>Name of the Student:</span>");
                SbHtml.Append("&nbsp;&nbsp;<span>" + Studname + "</span>");
                SbHtml.Append("</td>");

                SbHtml.Append("<td>");
                SbHtml.Append("<span>Class & Section:</span>");
                if (Section.Trim() != "")
                {
                    SbHtml.Append("&nbsp;&nbsp;<span>" + ddlBranch.SelectedItem.Text + " - " + Section + "</span>");
                }
                else
                {
                    SbHtml.Append("&nbsp;&nbsp;<span>" + ddlBranch.SelectedItem.Text + "</span>");
                }

                SbHtml.Append("</td>");

                SbHtml.Append("</tr>");
                SbHtml.Append("</table>");
                SbHtml.Append("</div>");

                #endregion

                #region Subject Details

                string OutofMark = Convert.ToString(txt_Convertion.Text);
                double.TryParse(OutofMark, out ConvertionOutofMark);
                SbHtml.Append("<br>");
                SbHtml.Append("<div style='width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
                SbHtml.Append("<table cellspacing='0' cellpadding='5' style='width: 645px;' border='1px'>");
                SbHtml.Append("<tr style='text-align:center;'>");

                SbHtml.Append("<td colspan='2'>");
                SbHtml.Append("<span>Subject</span>");
                SbHtml.Append("</td>");

                if (OutofMark.Trim() != "" && OutofMark.Trim() != "0")
                {
                    SbHtml.Append("<td>");
                    SbHtml.Append("<span>Mark (Out of " + OutofMark + ")</span>");
                    SbHtml.Append("</td>");
                }
                else
                {
                    SbHtml.Append("<td>");
                    SbHtml.Append("<span>Mark</span>");
                    SbHtml.Append("</td>");
                }

                SbHtml.Append("<td>");
                SbHtml.Append("<span>Grade</span>");
                SbHtml.Append("</td>");

                SbHtml.Append("<td>");
                SbHtml.Append("<span>Subject Average</span>");
                SbHtml.Append("</td>");

                SbHtml.Append("<td>");
                SbHtml.Append("<span>Highest Mark</span>");
                SbHtml.Append("</td>");

                SbHtml.Append("</tr>");

                DataView DvSubjectOrder = new DataView();
                if (dtSubjectPriority.Rows.Count > 0)
                {
                    for (int intPri = 0; intPri < dtSubjectPriority.Rows.Count; intPri++)
                    {
                        string Priority = Convert.ToString(dtSubjectPriority.Rows[intPri]["subjectpriority"]);
                        string subjectNo = Convert.ToString(dtSubjectPriority.Rows[intPri]["subject_no"]);
                        dtSingleSubject.DefaultView.RowFilter = "subject_no='" + subjectNo.ToString() + "' and subjectpriority='" + Priority.ToString() + "'";
                        DvSubjectOrder = dtSingleSubject.DefaultView;
                        string displayGrade = string.Empty;
                        if (DvSubjectOrder.Count > 0)
                        {
                            string SubjectNo = Convert.ToString(DvSubjectOrder[0]["subject_no"]);
                            string Mark = Convert.ToString(DvSubjectOrder[0]["TestMark"]);
                            double.TryParse(Mark, out subjectMark);
                            string SubTestMaxMark = Convert.ToString(DvSubjectOrder[0]["ConductedMaxMark"]);
                            double.TryParse(SubTestMaxMark, out SubjectMaxMark);
                            dtSubjectStrength.DefaultView.RowFilter = "subject_no ='" + SubjectNo + "'";
                            dvsubstrength = dtSubjectStrength.DefaultView;
                            if (dvsubstrength.Count > 0)
                            {
                                double.TryParse(Convert.ToString(dvsubstrength[0]["count"]), out subStrenth);
                            }
                            dtSubjectMaxMark.DefaultView.RowFilter = "subject_no ='" + SubjectNo + "'";
                            dvsubMax = dtSubjectMaxMark.DefaultView;
                            if (dvsubMax.Count > 0)
                            {
                                double.TryParse(Convert.ToString(dvsubMax[0]["count"]), out SubMax);
                            }
                            dtSubjectTotal.DefaultView.RowFilter = "subject_no ='" + SubjectNo + "'";
                            dvsubTotal = dtSubjectTotal.DefaultView;
                            if (dvsubstrength.Count > 0)
                            {
                                double.TryParse(Convert.ToString(dvsubTotal[0]["count"]), out subTotal);
                            }
                            double outof100 = subjectMark;
                            if (subjectMark >= 0 && SubjectMaxMark > 0)
                                outof100 = Math.Round((subjectMark / SubjectMaxMark) * 100, 0, MidpointRounding.AwayFromZero);
                            if (ConvertionOutofMark != 0)
                            {
                                ConvertMark = Math.Round((subjectMark / SubjectMaxMark) * ConvertionOutofMark, 0, MidpointRounding.AwayFromZero);
                                //ConvertMark = (subjectMark / SubjectMaxMark) * ConvertionOutofMark;
                                ConverhighestMark = (SubMax / SubjectMaxMark) * ConvertionOutofMark;
                                convertAvarageMark = ((subTotal / subStrenth) / SubjectMaxMark) * ConvertionOutofMark;
                                convertionSubjectMaxMArk = ConvertionOutofMark;
                            }
                            else
                            {
                                ConverhighestMark = SubMax;
                                convertAvarageMark = (subTotal / subStrenth);
                                ConvertMark = Math.Round(subjectMark, 0, MidpointRounding.AwayFromZero);
                                convertionSubjectMaxMArk = SubjectMaxMark;
                            }
                            string displayMark = Convert.ToString(Math.Round(ConvertMark)).Trim();
                            if (subjectMark < 0)
                            {
                                displayMark = getMarkText(Convert.ToString(subjectMark).Trim());
                            }
                            else
                                TotalSecureMark += ConvertMark;
                            TotalMaxMark += convertionSubjectMaxMArk;

                            DataView dvGrade = new DataView();
                            if (dtGradeDetails.Rows.Count > 0)
                            {
                                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='" + sems + "' and Criteria='" + testNames.Trim() + "' and Frange<='" + ConvertMark + "' and Trange>='" + ConvertMark + "'";
                                dvGrade = dtGradeDetails.DefaultView;
                                if (dvGrade.Count == 0)
                                {
                                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='0' and Criteria='" + testNames.Trim() + "' and Frange<='" + ConvertMark + "' and Trange>='" + ConvertMark + "'";
                                    dvGrade = dtGradeDetails.DefaultView;
                                }
                                if (dvGrade.Count == 0)
                                {
                                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='" + sems + "' and Frange<='" + outof100 + "' and Trange>='" + outof100 + "'";
                                    dvGrade = dtGradeDetails.DefaultView;
                                }
                                if (dvGrade.Count == 0)
                                {
                                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='0' and Frange<='" + outof100 + "' and Trange>='" + outof100 + "'";
                                    dvGrade = dtGradeDetails.DefaultView;
                                }
                            }
                            if (dvGrade.Count > 0)
                            {
                                displayGrade = Convert.ToString(dvGrade[0]["Mark_Grade"]).Trim();
                            }
                            //saran

                            dtSubSubjectMarkList.DefaultView.RowFilter = "subject_no='" + SubjectNo + "' and isSingleSubject='false'";

                            DataTable dtSubSubjectName = dtSubSubjectMarkList.DefaultView.ToTable(true, "subSubjectName", "subjectId");
                            if (dtSubSubjectName.Rows.Count > 0)
                            {
                                SbHtml.Append("<tr>");
                                SbHtml.Append("<td rowspan='" + dtSubSubjectName.Rows.Count + "'>");
                                SbHtml.Append("<span>" + Convert.ToString(DvSubjectOrder[0]["Subject_name"]) + "</span>");

                                SbHtml.Append("</td>");
                                int rowSub = 0;
                                foreach (DataRow drSubSubject in dtSubSubjectName.Rows)
                                {
                                    string subSubjectName = Convert.ToString(drSubSubject["subSubjectName"]).Trim();
                                    string subSubjectId = Convert.ToString(drSubSubject["subjectId"]).Trim();

                                    dtSubSubjectMarkDetails.DefaultView.RowFilter = "subjectId='" + subSubjectId + "' and isSingleSubject='false'";
                                    DataView dvSubSubjectMark = new DataView();
                                    dvSubSubjectMark = dtSubSubjectMarkDetails.DefaultView;
                                    if (rowSub != 0)
                                    {
                                        SbHtml.Append("<tr>");
                                    }
                                    SbHtml.Append("<td>");
                                    SbHtml.Append("<span>" + subSubjectName + "</span>");

                                    SbHtml.Append("</td>");
                                    if (dvSubSubjectMark.Count > 0)
                                    {
                                        //s.subjectId, s.subSubjectName,s.maxMark,s.minMark,subject_no,e.criteria_no,sm.appNo,sm.testMark,sm.ReTestMark,sm.remarks
                                        string testMark = Convert.ToString(dvSubSubjectMark[0]["testMark"]).Trim();
                                        string testMinMark = Convert.ToString(dvSubSubjectMark[0]["minMark"]).Trim();
                                        string testMaxMark = Convert.ToString(dvSubSubjectMark[0]["maxMark"]).Trim();
                                        string reTestMark = Convert.ToString(dvSubSubjectMark[0]["ReTestMark"]).Trim();

                                        double testMarks = 0;
                                        double testMinMarks = 0;
                                        double testMaxMarks = 0;
                                        double reTestMarks = 0;

                                        double.TryParse(testMark, out testMarks);
                                        double.TryParse(testMinMark, out testMinMarks);
                                        double.TryParse(testMaxMark, out testMaxMarks);
                                        double.TryParse(reTestMark, out reTestMarks);

                                        double subSubjectOutOf100 = 0;
                                        displayMark = testMark;
                                        if (testMarks < 0)
                                        {
                                            displayMark = getMarkText(Convert.ToString(testMarks).Trim());
                                        }
                                        if (testMaxMarks > 0 && testMarks > 0)
                                        {
                                            subSubjectOutOf100 = (testMarks / testMaxMarks) * 100;
                                        }
                                        subSubjectOutOf100 = Math.Round(subSubjectOutOf100, 0, MidpointRounding.AwayFromZero);

                                        dvGrade = new DataView();
                                        if (dtGradeDetails.Rows.Count > 0)
                                        {
                                            dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='" + sems + "' and Criteria='" + testNames.Trim() + "' and Frange<='" + subSubjectOutOf100 + "' and Trange>='" + ConvertMark + "'";
                                            dvGrade = dtGradeDetails.DefaultView;
                                            if (dvGrade.Count == 0)
                                            {
                                                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='0' and Criteria='" + testNames.Trim() + "' and Frange<='" + subSubjectOutOf100 + "' and Trange>='" + ConvertMark + "'";
                                                dvGrade = dtGradeDetails.DefaultView;
                                            }
                                            if (dvGrade.Count == 0)
                                            {
                                                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='" + sems + "' and Frange<='" + subSubjectOutOf100 + "' and Trange>='" + subSubjectOutOf100 + "'";
                                                dvGrade = dtGradeDetails.DefaultView;
                                            }
                                            if (dvGrade.Count == 0)
                                            {
                                                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='0' and Frange<='" + subSubjectOutOf100 + "' and Trange>='" + subSubjectOutOf100 + "'";
                                                dvGrade = dtGradeDetails.DefaultView;
                                            }
                                        }
                                        if (dvGrade.Count > 0)
                                        {
                                            displayGrade = Convert.ToString(dvGrade[0]["Mark_Grade"]).Trim();
                                        }
                                        SbHtml.Append("<td style='text-align:center;'>");
                                        SbHtml.Append("<span>" + displayMark + "</span>");
                                        SbHtml.Append("</td>");

                                        SbHtml.Append("<td style='text-align:center;'>");
                                        SbHtml.Append("<span>" + displayGrade + "</span>");
                                        SbHtml.Append("</td>");
                                    }
                                    if (rowSub == 0)
                                    {
                                        SbHtml.Append("<td rowspan='" + dtSubSubjectName.Rows.Count + "' style='text-align:center;'>");
                                        SbHtml.Append("<span>" + Math.Round(convertAvarageMark) + "</span>");
                                        SbHtml.Append("</td>");

                                        SbHtml.Append("<td rowspan='" + dtSubSubjectName.Rows.Count + "' style='text-align:center;'>");
                                        SbHtml.Append("<span>" + Math.Round(ConverhighestMark) + "</span>");
                                        SbHtml.Append("</td>");
                                        SbHtml.Append("</tr>");
                                    }
                                    else
                                    {
                                        SbHtml.Append("</tr>");
                                    }
                                    rowSub++;
                                }
                            }
                            else
                            {
                                SbHtml.Append("<tr>");
                                SbHtml.Append("<td colspan='2'>");
                                SbHtml.Append("<span>" + Convert.ToString(DvSubjectOrder[0]["Subject_name"]) + "</span>");
                                SbHtml.Append("</td>");

                                SbHtml.Append("<td style='text-align:center;'>");
                                SbHtml.Append("<span>" + displayMark + "</span>");
                                SbHtml.Append("</td>");

                                SbHtml.Append("<td style='text-align:center;'>");
                                SbHtml.Append("<span>" + displayGrade + "</span>");
                                SbHtml.Append("</td>");

                                SbHtml.Append("<td style='text-align:center;'>");
                                SbHtml.Append("<span>" + Math.Round(convertAvarageMark) + "</span>");
                                SbHtml.Append("</td>");

                                SbHtml.Append("<td style='text-align:center;'>");
                                SbHtml.Append("<span>" + Math.Round(ConverhighestMark) + "</span>");
                                SbHtml.Append("</td>");
                                SbHtml.Append("</tr>");
                            }
                        }
                        else
                        {
                            dtMultiSubject.DefaultView.RowFilter = "SubType_no='" + Convert.ToString(subjectNo) + "'";
                            DvSubTypeSubject = dtMultiSubject.DefaultView;
                            StringBuilder SbSubject = new StringBuilder();
                            if (DvSubTypeSubject.Count > 0)
                            {
                                for (int intDvSub = 0; intDvSub < DvSubTypeSubject.Count; intDvSub++)
                                {
                                    SbSubject.Append(Convert.ToString(DvSubTypeSubject[intDvSub]["subject_no"]) + ",");
                                }
                                if (SbSubject.Length > 0)
                                {
                                    SbSubject.Remove(SbSubject.Length - 1, 1);
                                }
                                string GetQuery = " SELECT sum(e.Max_Mark) as MaxMark FROM CriteriaForInternal c,Exam_type e where c.Criteria_no=e.criteria_no and c.criteria_no='" + TestCode + "' and e.subject_no in (" + SbSubject + ")";
                                if (Section.Trim() != "")
                                {
                                    GetQuery += "  and e.sections ='" + Section + "' ";
                                }
                                string MaxMark = da.GetFunction(GetQuery);
                                double Max = 0;
                                double.TryParse(MaxMark, out  Max);
                                double convert = 0;
                                if (txt_Convertion.Text.Trim() != "" && txt_Convertion.Text.Trim() != "0")
                                {
                                    double.TryParse(Convert.ToString(txt_Convertion.Text), out convert);
                                }
                                if (convert == 0)
                                {
                                    convert = Max;
                                }
                                string GetValueQuery = " SELECT round((sum(re.marks_obtained)/" + Max + " )*" + convert + ",0) as Count,re.roll_no FROM CriteriaForInternal c,Exam_type e,Result re where c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and c.criteria_no='" + TestCode + "' and marks_obtained >=0 and e.subject_no in (" + SbSubject + ")";
                                if (Section.Trim() != "")
                                {
                                    GetValueQuery += "  and e.sections ='" + Section + "' ";
                                }
                                GetValueQuery += " group by re.roll_no order by sum(re.marks_obtained) desc";
                                DataSet dsmulti = da.select_method_wo_parameter(GetValueQuery, "Text");

                                qry = "SELECT SUM(e.max_mark) as MaxMark FROM CriteriaForInternal c,Exam_type e,subject s where s.subject_no=e.subject_no and s.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and c.criteria_no='" + TestCode + "' and s.subType_no='" + Convert.ToString(subjectNo) + "'" + ((Section.Trim() != "") ? "  and e.sections ='" + Section + "' " : "") + " group by s.subType_no";
                                double subjectTypeMaxMark = dirAcc.selectScalarDouble(qry);
                                double MaxSubjectMark = 0;
                                double MinSubjectMark = 0;
                                double SumSubjectMark = 0;

                                double AvgMark = 0;
                                ConvertMark = 0;
                                if (dsmulti.Tables.Count > 0 && dsmulti.Tables[0].Rows.Count > 0)
                                {
                                    dsmulti.Tables[0].DefaultView.RowFilter = "roll_no='" + RollNo + "'";
                                    DataView dvSubMark = dsmulti.Tables[0].DefaultView;
                                    if (dvSubMark.Count > 0)
                                    {
                                        double.TryParse(Convert.ToString(dvSubMark[0]["Count"]), out  ConvertMark);
                                    }
                                    double.TryParse(Convert.ToString(dsmulti.Tables[0].Rows[0][0]), out MaxSubjectMark);
                                    double.TryParse(Convert.ToString(dsmulti.Tables[0].Rows[dsmulti.Tables[0].Rows.Count - 1][0]), out MinSubjectMark);
                                    double.TryParse(Convert.ToString(dsmulti.Tables[0].Compute("sum(Count)", "")), out SumSubjectMark);
                                    AvgMark = (SumSubjectMark / dsmulti.Tables[0].Rows.Count);
                                }
                                double outof100 = ConvertMark;
                                if (convert == 0)
                                {
                                    if (ConvertMark >= 0 && subjectTypeMaxMark > 0)
                                        outof100 = Math.Round((ConvertMark / subjectTypeMaxMark) * 100, 0, MidpointRounding.AwayFromZero);
                                }

                                DataView dvGrade = new DataView();
                                if (dtGradeDetails.Rows.Count > 0)
                                {
                                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='" + sems + "' and Criteria='" + testNames.Trim() + "' and Frange<='" + ConvertMark + "' and Trange>='" + ConvertMark + "'";
                                    dvGrade = dtGradeDetails.DefaultView;
                                    if (dvGrade.Count == 0)
                                    {
                                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='0' and Criteria='" + testNames.Trim() + "' and Frange<='" + ConvertMark + "' and Trange>='" + ConvertMark + "'";
                                        dvGrade = dtGradeDetails.DefaultView;
                                    }
                                    if (dvGrade.Count == 0)
                                    {
                                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='" + sems + "' and Frange<='" + outof100 + "' and Trange>='" + outof100 + "'";
                                        dvGrade = dtGradeDetails.DefaultView;
                                    }
                                    if (dvGrade.Count == 0)
                                    {
                                        dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='0' and Frange<='" + outof100 + "' and Trange>='" + outof100 + "'";
                                        dvGrade = dtGradeDetails.DefaultView;
                                    }
                                }
                                if (dvGrade.Count > 0)
                                {
                                    displayGrade = Convert.ToString(dvGrade[0]["Mark_Grade"]).Trim();
                                }
                                string displayMark = Convert.ToString(ConvertMark).Trim();
                                if (ConvertMark < 0)
                                {
                                    displayMark = getMarkText(Convert.ToString(ConvertMark).Trim());
                                }
                                dtSubSubjectMarkList.DefaultView.RowFilter = "SubType_no='" + Convert.ToString(subjectNo) + "' and isSingleSubject='true'";

                                DataTable dtSubSubjectName = dtSubSubjectMarkList.DefaultView.ToTable(true, "subSubjectName");
                                if (dtSubSubjectName.Rows.Count > 0)
                                {
                                    SbHtml.Append("<tr>");
                                    SbHtml.Append("<td rowspan='" + dtSubSubjectName.Rows.Count + "'>");
                                    SbHtml.Append("<span>" + Convert.ToString(DvSubTypeSubject[0]["subject_Type"]) + "</span>");
                                    SbHtml.Append("</td>");
                                    int rowSub = 0;
                                    foreach (DataRow drSubSubject in dtSubSubjectName.Rows)
                                    {
                                        string subSubjectName = Convert.ToString(drSubSubject["subSubjectName"]).Trim();
                                        //string subSubjectId = Convert.ToString(drSubSubject["subjectId"]).Trim();

                                        dtSubSubjectMarkDetails.DefaultView.RowFilter = "subSubjectName='" + Convert.ToString(drSubSubject["subSubjectName"]).Trim() + "' and SubType_no='" + Convert.ToString(subjectNo).Trim() + "' and isSingleSubject='true'";
                                        DataView dvSubSubjectMark = new DataView();
                                        dvSubSubjectMark = dtSubSubjectMarkDetails.DefaultView;
                                        if (rowSub != 0)
                                        {
                                            SbHtml.Append("<tr>");
                                        }
                                        SbHtml.Append("<td>");
                                        SbHtml.Append("<span>" + subSubjectName + "</span>");

                                        SbHtml.Append("</td>");
                                        if (dvSubSubjectMark.Count > 0)
                                        {
                                            //s.subjectId, s.subSubjectName,s.maxMark,s.minMark,subject_no,e.criteria_no,sm.appNo,sm.testMark,sm.ReTestMark,sm.remarks
                                            object testMark = dvSubSubjectMark.ToTable().Compute("SUM(testMark)", "testMark>=0 and subSubjectName='" + Convert.ToString(drSubSubject["subSubjectName"]).Trim() + "' and SubType_no='" + Convert.ToString(subjectNo).Trim() + "' and isSingleSubject='true'"); //Convert.ToString(dvSubSubjectMark[0]["testMark"]).Trim();
                                            object testMinMark = dvSubSubjectMark.ToTable().Compute("SUM(minMark)", "minMark>=0 and subSubjectName='" + Convert.ToString(drSubSubject["subSubjectName"]).Trim() + "' and SubType_no='" + Convert.ToString(subjectNo).Trim() + "' and isSingleSubject='true'"); //Convert.ToString(dvSubSubjectMark[0]["minMark"]).Trim();
                                            object testMaxMark = dvSubSubjectMark.ToTable().Compute("SUM(maxMark)", "maxMark>=0 and subSubjectName='" + Convert.ToString(drSubSubject["subSubjectName"]).Trim() + "' and SubType_no='" + Convert.ToString(subjectNo).Trim() + "' and isSingleSubject='true'"); //Convert.ToString(dvSubSubjectMark[0]["maxMark"]).Trim();
                                            object reTestMark = dvSubSubjectMark.ToTable().Compute("SUM(ReTestMark)", "ReTestMark>=0 and subSubjectName='" + Convert.ToString(drSubSubject["subSubjectName"]).Trim() + "' and SubType_no='" + Convert.ToString(subjectNo).Trim() + "' and isSingleSubject='true'"); //Convert.ToString(dvSubSubjectMark[0]["ReTestMark"]).Trim();

                                            double testMarks = 0;
                                            double testMinMarks = 0;
                                            double testMaxMarks = 0;
                                            double reTestMarks = 0;

                                            double.TryParse(Convert.ToString(testMark).Trim(), out testMarks);
                                            double.TryParse(Convert.ToString(testMinMark).Trim(), out testMinMarks);
                                            double.TryParse(Convert.ToString(testMaxMark).Trim(), out testMaxMarks);
                                            double.TryParse(Convert.ToString(reTestMark).Trim(), out reTestMarks);

                                            double subSubjectOutOf100 = 0;
                                            displayMark = Convert.ToString(testMark).Trim();
                                            if (testMarks < 0)
                                            {
                                                displayMark = getMarkText(Convert.ToString(testMarks).Trim());
                                            }
                                            if (testMaxMarks > 0 && testMarks > 0)
                                            {
                                                subSubjectOutOf100 = (testMarks / testMaxMarks) * 100;
                                            }
                                            subSubjectOutOf100 = Math.Round(subSubjectOutOf100, 0, MidpointRounding.AwayFromZero);

                                            dvGrade = new DataView();
                                            if (dtGradeDetails.Rows.Count > 0)
                                            {
                                                dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='" + sems + "' and Criteria='" + testNames.Trim() + "' and Frange<='" + subSubjectOutOf100 + "' and Trange>='" + ConvertMark + "'";
                                                dvGrade = dtGradeDetails.DefaultView;
                                                if (dvGrade.Count == 0)
                                                {
                                                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='0' and Criteria='" + testNames.Trim() + "' and Frange<='" + subSubjectOutOf100 + "' and Trange>='" + ConvertMark + "'";
                                                    dvGrade = dtGradeDetails.DefaultView;
                                                }
                                                if (dvGrade.Count == 0)
                                                {
                                                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='" + sems + "' and Frange<='" + subSubjectOutOf100 + "' and Trange>='" + subSubjectOutOf100 + "'";
                                                    dvGrade = dtGradeDetails.DefaultView;
                                                }
                                                if (dvGrade.Count == 0)
                                                {
                                                    dtGradeDetails.DefaultView.RowFilter = "batch_year='" + batch + "' and College_Code='" + college + "' and Degree_Code='" + degree + "' and Semester='0' and Frange<='" + subSubjectOutOf100 + "' and Trange>='" + subSubjectOutOf100 + "'";
                                                    dvGrade = dtGradeDetails.DefaultView;
                                                }
                                            }
                                            if (dvGrade.Count > 0)
                                            {
                                                displayGrade = Convert.ToString(dvGrade[0]["Mark_Grade"]).Trim();
                                            }
                                            SbHtml.Append("<td style='text-align:center;'>");
                                            SbHtml.Append("<span>" + displayMark + "</span>");
                                            SbHtml.Append("</td>");

                                            SbHtml.Append("<td style='text-align:center;'>");
                                            SbHtml.Append("<span>" + displayGrade + "</span>");
                                            SbHtml.Append("</td>");
                                        }
                                        else
                                        {
                                            SbHtml.Append("<td style='text-align:center;'>");
                                            SbHtml.Append("<span>" + displayMark + "</span>");
                                            SbHtml.Append("</td>");

                                            SbHtml.Append("<td style='text-align:center;'>");
                                            SbHtml.Append("<span>" + displayGrade + "</span>");
                                            SbHtml.Append("</td>");
                                        }
                                        if (rowSub == 0)
                                        {
                                            SbHtml.Append("<td rowspan='" + dtSubSubjectName.Rows.Count + "' style='text-align:center;'>");
                                            SbHtml.Append("<span>" + Math.Round(convertAvarageMark) + "</span>");
                                            SbHtml.Append("</td>");

                                            SbHtml.Append("<td rowspan='" + dtSubSubjectName.Rows.Count + "' style='text-align:center;'>");
                                            SbHtml.Append("<span>" + Math.Round(ConverhighestMark) + "</span>");
                                            SbHtml.Append("</td>");
                                            SbHtml.Append("</tr>");
                                        }
                                        else
                                        {
                                            SbHtml.Append("</tr>");
                                        }
                                        rowSub++;
                                    }
                                }
                                else
                                {

                                    SbHtml.Append("<tr>");
                                    SbHtml.Append("<td colspan='2'>");
                                    SbHtml.Append("<span>" + Convert.ToString(DvSubTypeSubject[0]["subject_Type"]) + "</span>");
                                    SbHtml.Append("</td>");

                                    SbHtml.Append("<td style='text-align:center;'>");
                                    SbHtml.Append("<span>" + displayMark + "</span>");
                                    SbHtml.Append("</td>");

                                    SbHtml.Append("<td style='text-align:center;'>");
                                    SbHtml.Append("<span>" + displayGrade + "</span>");
                                    SbHtml.Append("</td>");

                                    SbHtml.Append("<td style='text-align:center;'>");
                                    SbHtml.Append("<span>" + Math.Round(AvgMark) + "</span>");
                                    SbHtml.Append("</td>");

                                    SbHtml.Append("<td style='text-align:center;'>");
                                    SbHtml.Append("<span>" + Math.Round(MaxSubjectMark) + "</span>");
                                    SbHtml.Append("</td>");

                                    SbHtml.Append("</tr>");
                                }
                                if (ConvertMark >= 0)
                                    TotalSecureMark += ConvertMark;
                                TotalMaxMark += convert;
                            }
                        }
                    }
                }
                SbHtml.Append("</table>");
                SbHtml.Append("</div>");
                #endregion

                #region FooterDetails

                SbHtml.Append("<br>");
                SbHtml.Append("<br>");
                SbHtml.Append("<div style='width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
                SbHtml.Append("<table cellspacing='0' cellpadding='5' style='width: 645px;'>");
                SbHtml.Append("<tr style='text-align:left;'>");
                SbHtml.Append("<td>");
                SbHtml.Append("<span>Total: " + Math.Round(TotalSecureMark) + " / " + TotalMaxMark + " </span>");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("<tr style='text-align:left;'>");

                int RankNo = 0;
                int STdCount = attnd_report.Sheets[0].RowCount - 1;
                if (DtRank.Rows.Count > 0)
                {
                    DtRank.DefaultView.RowFilter = "AppNo ='" + App_no + "'";
                    DataView dvR = DtRank.DefaultView;
                    if (dvR.Count > 0)
                    {
                        int.TryParse(Convert.ToString(dvR[0]["RankOnePlus"]), out RankNo);
                    }
                }
                SbHtml.Append("<td>");
                if (RankNo != 0)
                {
                    SbHtml.Append("<span>Rank: " + RankNo + " / " + DtRank.Rows.Count + " / " + STdCount + "</span>");
                }
                else
                {
                    SbHtml.Append("<span>Rank: - / " + DtRank.Rows.Count + " / " + STdCount + "</span>");
                }
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("<tr style='text-align:left;'>");
                SbHtml.Append("<td>");
                SbHtml.Append("<span>Remarks: __________________________________________________</span>");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("</table>");
                SbHtml.Append("</div>");

                #endregion

                #region Footer StaffSignature

                SbHtml.Append("<br>");
                SbHtml.Append("<br>");
                SbHtml.Append("<div style='width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
                SbHtml.Append("<table cellspacing='0' cellpadding='0' style='width: 645px; font-weight: bold;'>");
                SbHtml.Append("<tr style='text-align:left;'>");

                SbHtml.Append("<td>");
                SbHtml.Append("<span>Signature of the Class Teacher:</span>");
                SbHtml.Append("</td>");

                SbHtml.Append("</tr>");
                SbHtml.Append("</table>");
                SbHtml.Append("</div>");

                #endregion

                #region Footer Signature

                SbHtml.Append("<br>");
                SbHtml.Append("<br>");
                SbHtml.Append("<br>");
                SbHtml.Append("<div style='width: 645px; border: 0px solid black; margin:0px; margin-left: 5px;'>");
                SbHtml.Append("<table cellspacing='0' cellpadding='0' style='width: 645px; font-weight: bold;'>");
                SbHtml.Append("<tr style='text-align:left;'>");
                SbHtml.Append("<td>");
                SbHtml.Append("<span>DR. C.SATISH</span>");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("<tr style='text-align:left;'>");
                SbHtml.Append("<td>");
                SbHtml.Append("<span>DIRECTOR</span>");
                SbHtml.Append("</td>");
                SbHtml.Append("</tr>");
                SbHtml.Append("</table>");
                SbHtml.Append("</div>");
                SbHtml.Append("</div>");

                #endregion

                #endregion

                contentDiv.InnerHtml = SbHtml.ToString();
                contentDiv.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "btnPrint", "PrintDiv();", true);
            }
        }
        catch
        {

        }
        return ConditionFlag;

    }

    public DataTable RankCalculation(string TestCode)
    {
        DataTable DtRank = new DataTable();
        try
        {
            DataTable dtStudent = new DataTable();
            DataView dvSubject = new DataView();
            StringBuilder AppNoAdd = new StringBuilder();
            DataTable dtSum = new DataTable();
            Dictionary<string, double> AddMarkDetails = new Dictionary<string, double>();
            Dictionary<string, double> AddMarkDetailsNEw = new Dictionary<string, double>();
            string TotalSTudentQuery = string.Empty;
            string Sections = string.Empty;
            if (ddlSec.Items.Count > 0)
            {
                Sections += "  and r.sections ='" + ddlSec.SelectedItem.Text + "' ";
            }
            TotalSTudentQuery = "  select case when ISNULL(c.App_no,'')<>'' then c.App_no when ISNULL(s.App_no,'')<>'' then s.App_no end as App_no,ISNULL(c.totalSubject,'0')+ISNULL(s.totalSubject,'0') as totalSubject from (select  LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.App_No),''))) App_no,Count(distinct sc.subject_no) totalSubject from subject s,sub_sem ss,syllabus_master sm,subjectChooser sc,Registration r  ,CriteriaForInternal c,Exam_type e  where e.subject_no=s.subject_no and c.criteria_no=c.criteria_no and c.syll_code=sm.syll_code and r.Roll_No=sc.roll_no and r.Batch_Year=sm.Batch_Year and sm.degree_code=r.degree_code and sm.semester=r.Current_Semester and s.syll_code=ss.syll_code and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=sc.subject_no and sc.semester=sm.semester and ISNULL(ss.isSingleSubject,'0')=0 and sm.Batch_year='" + ddlBatch.SelectedItem.Text + "' and sm.semester='" + ddlSem.SelectedItem.Text + "' and sm.degree_code='" + ddlBranch.SelectedValue + "' and c.criteria_no='" + TestCode + "' " + Sections + " /*and ss.promote_count=1*/ group by r.App_No) as c  full join (select LTRIM(RTRIM(ISNULL(Convert(varchar(500),r.App_No),''))) App_no,Count(distinct ss.subType_no) totalSubject from subject s,sub_sem ss,syllabus_master sm,subjectChooser sc,Registration r ,CriteriaForInternal c,Exam_type e  where e.subject_no=s.subject_no and c.criteria_no=c.criteria_no and c.syll_code=sm.syll_code and r.Roll_No=sc.roll_no and r.Batch_Year=sm.Batch_Year and sm.degree_code=r.degree_code and sm.semester=r.Current_Semester and s.syll_code=ss.syll_code and s.syll_code=sm.syll_code and ss.syll_code=sm.syll_code and ss.subType_no=s.subType_no and s.subject_no=sc.subject_no and sc.semester=sm.semester and ISNULL(ss.isSingleSubject,'0')=1 and sm.Batch_year='" + ddlBatch.SelectedItem.Text + "' and sm.semester='" + ddlSem.SelectedItem.Text + "' and sm.degree_code='" + ddlBranch.SelectedValue + "' and c.criteria_no='" + TestCode + "' " + Sections + "  /*and ss.promote_count=1*/ group by r.App_No) as s on c.App_no=s.App_no order by s.App_No";

            TotalSTudentQuery += " SELECT r.App_no,r.Roll_no,r.college_Code,r.Reg_No,r.Batch_Year,r.degree_Code,r.current_semester,c.Criteria_no as TestNo,c.criteria as TestName,c.min_mark as TestMinMark,c.max_mark as TestMaxMark,s.subject_code,s.subject_name,s.subjectpriority,s.subject_no,s.min_int_marks as SubjectMinINT,s.max_int_marks as SubjectMaxINT,s.min_ext_marks as SubjectMinEXT,s.max_ext_marks as SubjectMaxEXT,s.mintotal as SubjectMinTotal,s.maxtotal as SubjectMaxTotal,e.exam_code,e.min_mark as ConductedMinMark,e.max_mark as ConductedMaxMark,CAST(ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'0') as float) as TestMark,CAST(ISNULL(CONVERT(VARCHAR(100),re.Retest_Marks_obtained),'0') as float) as RetestMark,marks_obtained,ss.subject_type,ss.subtype_no FROM CriteriaForInternal c,Exam_type e,Result re,registration r,syllabus_master sm,subject s ,sub_sem ss where ss.subType_no=s.subType_no and s.subject_no=e.subject_no and s.syll_code=sm.syll_code and s.syll_code=c.syll_code and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and r.Batch_Year=sm.Batch_Year and r.degree_Code=sm.degree_code and r.current_semester=sm.semester and r.Roll_no=re.roll_no and LTRIM(RTRIM(ISNULL(e.sections,'')))=LTRIM(RTRIM(ISNULL(r.sections,'')))  and c.criteria_no='" + TestCode + "' and re.marks_obtained>= e.min_mark and ISNULL(ss.isSingleSubject,'0')='0'";
            //and re.marks_obtained>= e.min_mark
            if (ddlSec.Items.Count > 0)
            {
                TotalSTudentQuery += "  and e.sections ='" + ddlSec.SelectedItem.Text + "' ";
            }
            TotalSTudentQuery += " order by r.App_no,s.subject_code";
            TotalSTudentQuery += " SELECT r.App_no,r.Roll_no,r.college_Code,r.Reg_No,r.Batch_Year,r.degree_Code,r.current_semester,c.Criteria_no as TestNo,c.criteria as TestName,c.min_mark as TestMinMark,c.max_mark as TestMaxMark,s.subject_code,s.subject_name,s.subjectpriority,s.subject_no,s.min_int_marks as SubjectMinINT,s.max_int_marks as SubjectMaxINT,s.min_ext_marks as SubjectMinEXT,s.max_ext_marks as SubjectMaxEXT,s.mintotal as SubjectMinTotal,s.maxtotal as SubjectMaxTotal,e.exam_code,e.min_mark as ConductedMinMark,e.max_mark as ConductedMaxMark,CAST(ISNULL(CONVERT(VARCHAR(100),re.marks_obtained),'0') as float) as TestMark,CAST(ISNULL(CONVERT(VARCHAR(100),re.Retest_Marks_obtained),'0')  as float) as RetestMark,marks_obtained,ss.subject_type,ss.subtype_no FROM CriteriaForInternal c,Exam_type e,Result re,registration r,syllabus_master sm,subject s,sub_sem ss where ss.subType_no=s.subType_no and s.subject_no=e.subject_no and s.syll_code=sm.syll_code and s.syll_code=c.syll_code and sm.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and r.Batch_Year=sm.Batch_Year and r.degree_Code=sm.degree_code and r.current_semester=sm.semester and r.Roll_no=re.roll_no and LTRIM(RTRIM(ISNULL(e.sections,'')))=LTRIM(RTRIM(ISNULL(r.sections,'')))  and c.criteria_no='" + TestCode + "' and ISNULL(ss.isSingleSubject,'0')='1'";
            if (ddlSec.Items.Count > 0)
            {
                TotalSTudentQuery += "  and e.sections ='" + ddlSec.SelectedItem.Text + "'";
            }
            TotalSTudentQuery += " order by r.App_no,s.subject_code";
            ds.Clear();
            ds = da.select_method_wo_parameter(TotalSTudentQuery, "Text");
            DataView DvSubcount = new DataView();
            int TotalSubCunt = 0;
            int PassSubCount = 0;
            double TotalSubjectMark = 0;
            double TotalMaxSubjectMark = 0;
            double AvgMarkSubjectMark = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int intSTno = 0; intSTno < ds.Tables[0].Rows.Count; intSTno++)
                {
                    string AppNo = Convert.ToString(ds.Tables[0].Rows[intSTno]["App_no"]);
                    string SubCount = Convert.ToString(ds.Tables[0].Rows[intSTno]["totalSubject"]);
                    PassSubCount = 0;
                    int.TryParse(SubCount, out TotalSubCunt);
                    if (TotalSubCunt > 0)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            ds.Tables[1].DefaultView.RowFilter = "App_no='" + AppNo + "'";
                            DvSubcount = ds.Tables[1].DefaultView;
                            if (DvSubcount.Count > 0)
                            {
                                DataTable sumSubjectMark = DvSubcount.ToTable();
                                PassSubCount += DvSubcount.Count;
                                double.TryParse(Convert.ToString(sumSubjectMark.Compute("sum(TestMark)", "")), out TotalSubjectMark);
                                double.TryParse(Convert.ToString(sumSubjectMark.Compute("sum(ConductedMaxMark)", "")), out TotalMaxSubjectMark);
                            }
                        }
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            ds.Tables[2].DefaultView.RowFilter = "App_no='" + AppNo + "'";
                            DvSubcount = ds.Tables[2].DefaultView;
                            if (DvSubcount.Count > 0)
                            {
                                DataTable dtMultiSubject = DvSubcount.ToTable();
                                DataTable dtMultSubType = DvSubcount.ToTable(true, "subType_no", "subject_type");
                                if (dtMultSubType.Rows.Count > 0)
                                {
                                    for (int intM = 0; intM < dtMultSubType.Rows.Count; intM++)
                                    {
                                        dtMultiSubject.DefaultView.RowFilter = "subType_no='" + Convert.ToString(dtMultSubType.Rows[intM]["subType_no"]) + "'";
                                        DataTable dtSubjectSum = dtMultiSubject.DefaultView.ToTable();
                                        if (dtSubjectSum.Rows.Count > 0)
                                        {
                                            double subTypeMark = 0;
                                            double subTypeMax = 0;
                                            double subTypeMin = 0;
                                            double.TryParse(Convert.ToString(dtSubjectSum.Compute("sum(TestMark)", "TestMark>=0")), out subTypeMark);
                                            double.TryParse(Convert.ToString(dtSubjectSum.Compute("sum(ConductedMaxMark)", "")), out subTypeMax);
                                            double.TryParse(Convert.ToString(dtSubjectSum.Compute("sum(ConductedMinMark)", "")), out subTypeMin);
                                            if (subTypeMin <= subTypeMark)
                                            {
                                                PassSubCount += 1;
                                                TotalSubjectMark += subTypeMark;
                                                TotalMaxSubjectMark += subTypeMax;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (TotalSubCunt == PassSubCount)
                        {
                            double Percent = ((TotalSubjectMark / TotalSubCunt) * 100);
                            AddMarkDetails.Add(Convert.ToString(AppNo), TotalSubjectMark);
                            AddMarkDetailsNEw.Add(Convert.ToString(AppNo), Math.Round(Percent));
                        }
                    }
                }
                if (AddMarkDetails.Count > 0 && AddMarkDetailsNEw.Count > 0)
                {
                    CalculateRankByPercentage(AddMarkDetails, AddMarkDetailsNEw, ref DtRank, true);
                }
            }
        }
        catch
        {

        }
        return DtRank;
    }

    public void CalculateRankByPercentage(Dictionary<string, double> dicTotalMarks, Dictionary<string, double> dicTotalPercentage, ref DataTable dtRankList, bool rankOnePlus = false, byte forPercentageOrTotal = 0)
    {
        try
        {
            dicTotalPercentage = dicTotalPercentage.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            dicTotalMarks = dicTotalMarks.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            dtRankList = new DataTable();
            dtRankList.Clear();
            dtRankList.Columns.Add("AppNo");
            dtRankList.Columns.Add("Total");
            dtRankList.Columns.Add("Percentage");
            dtRankList.Columns.Add("Rank");
            dtRankList.Columns.Add("RankOnePlus");
            DataRow drRankList;
            int rank = 1;
            int rankOnePlusBy = 1;
            int actualRank = 0;
            double previousPercentage = 0;
            double previousTotal = 0;

            if (forPercentageOrTotal == 0)
            {
                foreach (KeyValuePair<string, double> keyPercentage in dicTotalPercentage)
                {
                    string keyAppNo = keyPercentage.Key.Trim();
                    double currentPercentage = keyPercentage.Value;
                    double totalMark = 0;
                    if (dicTotalMarks.ContainsKey(keyAppNo))
                    {
                        totalMark = dicTotalMarks[keyAppNo];
                    }
                    bool equalToPrevious = true;
                    if (previousPercentage != 0 && previousPercentage != currentPercentage)
                    {
                        if (rankOnePlus && actualRank != 0)
                        {
                            rankOnePlusBy = actualRank;
                        }
                        rank++;
                        rankOnePlusBy++;
                        equalToPrevious = false;
                    }
                    actualRank++;
                    previousPercentage = currentPercentage;
                    drRankList = dtRankList.NewRow();
                    drRankList["AppNo"] = keyAppNo;
                    drRankList["Total"] = totalMark;
                    drRankList["Percentage"] = currentPercentage;
                    drRankList["Rank"] = rank;
                    drRankList["RankOnePlus"] = rankOnePlusBy;
                    dtRankList.Rows.Add(drRankList);
                }
            }
            else
            {
                foreach (KeyValuePair<string, double> keyTotal in dicTotalMarks)
                {
                    string keyAppNo = keyTotal.Key.Trim();
                    double currentPercentage = 0;
                    double totalMark = keyTotal.Value;
                    if (dicTotalPercentage.ContainsKey(keyAppNo))
                    {
                        currentPercentage = dicTotalPercentage[keyAppNo];

                        bool equalToPrevious = true;
                        if (previousTotal != 0 && previousTotal != totalMark)
                        {
                            if (rankOnePlus && actualRank != 0)
                            {
                                rankOnePlusBy = actualRank;
                            }
                            rank++;
                            rankOnePlusBy++;
                            equalToPrevious = false;
                        }
                        actualRank++;
                        previousTotal = totalMark;
                        drRankList = dtRankList.NewRow();
                        drRankList["AppNo"] = keyAppNo;
                        drRankList["Total"] = totalMark;
                        drRankList["Percentage"] = currentPercentage;
                        drRankList["Rank"] = rank;
                        drRankList["RankOnePlus"] = rankOnePlusBy;
                        dtRankList.Rows.Add(drRankList);
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #endregion

    #region Reusable Method

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

    private bool getSelectedCheckBoxListCount(CheckBoxList cbl, out int selectedCount)
    {
        selectedCount = 0;
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                if (li.Selected)
                {
                    selectedCount++;
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Developed By Malang Raja T
    /// </summary>
    /// <param name="c">Only Data Bound Controls eg.DropDownList,RadioButtonList,CheckBoxList </param>
    /// <param name="selectedValue"></param>
    /// <param name="selectedText"></param>
    /// <param name="type">0 - Index; 1 - Text; 2 - Value;</param>
    private void SelectDataBound(Control c, string selectedValue, string selectedText)
    {
        try
        {
            bool isDataBoundControl = false;
            if (c is DataBoundControl)
            {
                if (c is CheckBoxList || c is DropDownList || c is RadioButtonList)
                {
                    isDataBoundControl = true;
                }
                if (isDataBoundControl)
                {
                    ListControl lstControls = (ListControl)c;
                    if (lstControls.Items.Count > 0)
                    {
                        ListItem[] listItem = new ListItem[lstControls.Items.Count];
                        lstControls.Items.CopyTo(listItem, 0);
                        if (listItem.Contains(new ListItem(selectedText, selectedValue)))
                        {
                            lstControls.SelectedValue = selectedValue;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
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
                //spPageHeading.InnerHtml = "Student's Previous Test Report";
                //Page.Title = "Student's Previous Test Report";
            }
            else
            {
                lblBatch.Text = "Batch";
                //spPageHeading.InnerHtml = "Student's Previous CAM Report";
                //Page.Title = "Student's Previous CAM Report";
            }
            new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
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
                string insType = dirAcc.selectScalarString(qry).Trim();
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
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
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
        return attendanceStatus.ToUpper().Trim();
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
                if (Convert.ToString(e.Key).Trim() == Convert.ToString(key).Trim())
                {
                    return e.Value;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex).Trim();
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
        return null;
    }

    private string orderByStudents(string collegeCode, string aliasName = null, string tableName = null)
    {
        string orderBy = string.Empty;
        try
        {
            string orderBySetting = dirAcc.selectScalarString("select value from master_Settings where settings='order_by' ");//and value<>''
            orderBySetting = orderBySetting.Trim();

            string serialNo = dirAcc.selectScalarString("select LinkValue from inssettings where college_code='" + collegeCode + "' and linkname='Student Attendance'");

            string aliasOrTableName = ((string.IsNullOrEmpty(aliasName) && string.IsNullOrEmpty(tableName)) ? "" : ((!string.IsNullOrEmpty(tableName)) ? tableName.Trim() + "." : ((!string.IsNullOrEmpty(aliasName)) ? aliasName.Trim() + "." : "")));

            orderBy = "ORDER BY " + aliasOrTableName + "roll_no";
            if (serialNo.Trim().ToLower() == "1" || serialNo.ToLower().Trim() == "true")
                orderBy = "ORDER BY " + aliasOrTableName + "serialno";
            else
                switch (orderBySetting)
                {
                    case "0":
                        orderBy = "ORDER BY " + aliasOrTableName + "roll_no";
                        break;
                    case "1":
                        orderBy = "ORDER BY " + aliasOrTableName + "Reg_No";
                        break;
                    case "2":
                        orderBy = "ORDER BY " + aliasOrTableName + "Stud_Name";
                        break;
                    case "0,1,2":
                        orderBy = "ORDER BY " + aliasOrTableName + "roll_no," + aliasOrTableName + "Reg_No," + aliasOrTableName + "stud_name";
                        break;
                    case "0,1":
                        orderBy = "ORDER BY " + aliasOrTableName + "roll_no," + aliasOrTableName + "Reg_No";
                        break;
                    case "1,2":
                        orderBy = "ORDER BY " + aliasOrTableName + "Reg_No," + aliasOrTableName + "Stud_Name";
                        break;
                    case "0,2":
                        orderBy = "ORDER BY " + aliasOrTableName + "roll_no," + aliasOrTableName + "Stud_Name";
                        break;
                    default:
                        orderBy = "ORDER BY " + aliasOrTableName + "roll_no";
                        break;
                }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex).Trim();
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
        return orderBy;
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
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
            return false;
        }
    }

    private string getMarkText(string mark)
    {
        try
        {
            mark = mark.Trim().ToLower();
            switch (mark)
            {
                case "-1":
                    mark = "AAA";
                    break;
                case "-2":
                    mark = "EL";
                    break;
                case "-3":
                    mark = "EOD";
                    break;
                case "-4":
                    mark = "ML";
                    break;
                case "-5":
                    mark = "SOD";
                    break;
                case "-6":
                    mark = "NSS";
                    break;
                case "-7":
                    mark = "NJ";
                    break;
                case "-8":
                    mark = "S";
                    break;
                case "-9":
                    mark = "L";
                    break;
                case "-10":
                    mark = "NCC";
                    break;
                case "-11":
                    mark = "HS";
                    break;
                case "-12":
                    mark = "PP";
                    break;
                case "-13":
                    mark = "SYOD";
                    break;
                case "-14":
                    mark = "COD";
                    break;
                case "-15":
                    mark = "OOD";
                    break;
                case "-16":
                    mark = "OD";
                    break;
                case "-17":
                    mark = "LA";
                    break;
                case "-18":
                    mark = "RAA";
                    break;
            }
        }
        catch
        {
        }
        return mark;
    }

    private void GetSubjectGrade()
    {
        try
        {

        }
        catch
        {
        }
    }

    #endregion

    public void Check()
    {
        //dtStudent = ds.Tables[0].DefaultView.ToTable(true, "App_no");
        //int NoofSubjectCount = 0;
        //int.TryParse(Convert.ToString(ds.Tables[1].Rows[0]["Count"]), out NoofSubjectCount);
        //if (dtStudent.Rows.Count > 0)
        //{
        //    for (int intdt = 0; intdt < dtStudent.Rows.Count; intdt++)
        //    {
        //        ds.Tables[0].DefaultView.RowFilter = "App_no='" + Convert.ToString(dtStudent.Rows[intdt]["App_no"]) + "'";
        //        dvSubject = ds.Tables[0].DefaultView;
        //        if (dvSubject.Count > 0 && dvSubject.Count == NoofSubjectCount)
        //        {
        //            dtSum = dvSubject.ToTable();
        //            int SumofSecureMark = Convert.ToInt32(dtSum.Compute("Sum(marks_obtained)", ""));
        //            int sumofMaxMark = Convert.ToInt32(dtSum.Compute("Sum(ConductedMaxMark)", ""));
        //            AppNoAdd.Append(Convert.ToString(dtStudent.Rows[intdt]["App_no"]) + ",");
        //            AddMarkDetails.Add(Convert.ToInt32(dtStudent.Rows[intdt]["App_no"]), SumofSecureMark);
        //        }
        //    }
        //    AddMarkDetails = AddMarkDetails.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        //    if (AppNoAdd.Length > 1)
        //    {
        //        AppNoAdd.Remove(AppNoAdd.Length - 1, 1);
        //        if (AppNoAdd.Length > 1)
        //        {
        //            string RankQuery = " SELECT DENSE_RANK() OVER (ORDER BY Total DESC) AS Rank, app_no, Total FROM (SELECT sum(re.marks_obtained)as Total,r.app_no FROM CriteriaForInternal c,Exam_type e,Result re,registration r where r.roll_no=re.roll_no and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code  and c.criteria_no='" + TestCode + "' and marks_obtained >=0  ";
        //            if (ddlSec.Items.Count > 0)
        //            {
        //                RankQuery += "  and e.sections ='" + ddlSec.SelectedItem.Text + "' ";
        //            }
        //            RankQuery += "  and r.app_no in (" + AppNoAdd + ") group by r.app_no) AS RankTable ";
        //            ds.Clear();
        //            ds = da.select_method_wo_parameter(RankQuery, "Text");
        //            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //            {
        //                DtRank = ds.Tables[0].DefaultView.ToTable();
        //            }
        //        }
        //    }
        //}
    }

}
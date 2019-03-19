using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Drawing;

public partial class AttendanceMOD_StudentsAttendancePrevousHistory : System.Web.UI.Page
{
    #region Field Declaration

    DAccess2 da = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();
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

    string orderBy = string.Empty;
    string orderBySetting = string.Empty;

    string qry = string.Empty;
    string qryCollegeCode = string.Empty;
    string qryBatchYear = string.Empty;
    string qryDegreeCode = string.Empty;
    string qrySemester = string.Empty;
    string qrySection = string.Empty;
    string qryCourseId = string.Empty;

    int selectedCount = 0;

    Institution institute;

    DataTable data = new DataTable();
    DataRow drow;
    Dictionary<int, string> dichrdet = new Dictionary<int, string>();
    ArrayList arrColHdrNames1 = new ArrayList();
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
                divStudentDetail.Visible = false;
                Bindcollege();
                SetStudentWiseSettings();
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    #region Bind Header

    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            ddlCollege.Enabled = false;
            DataSet dsprint = new DataSet();
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
                ht.Clear();
                dsprint.Clear();
                ht.Add("column_field", Convert.ToString(qryUserCodeOrGroupCode));
                dsprint = da.select_method("bind_college", ht, "sp");
            }
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = dsprint;
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
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
        return dsSettings;
    }

    private void SetStudentWiseSettings()
    {
        try
        {
            ddlSearchBy.Items.Clear();
            ddlSearchBy.Enabled = false;
            DataSet dsSearchBy = new DataSet();
            dsSearchBy = GetSettings();
            if (ddlCollege.Items.Count > 0)
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            if (dsSearchBy.Tables.Count > 0 && dsSearchBy.Tables[0].Rows.Count > 0)
            {
                ddlSearchBy.DataSource = dsSearchBy;
                ddlSearchBy.DataTextField = "settings";
                ddlSearchBy.DataValueField = "SetValue";
                ddlSearchBy.DataBind();
                ddlSearchBy.SelectedIndex = 0;
                if (CheckSchoolOrCollege(collegeCode))
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
                lblSearchStudent.Text = ddlSearchBy.SelectedItem.Text;

            }
            else
            {
                if (lblCollege.Text.Trim().ToUpper() == "SCHOOL")
                {
                    lblSearchStudent.Text = "Admission No";
                    ddlSearchBy.Items.Insert(0, new ListItem("Admission No", "1"));
                }
                else
                {
                    lblSearchStudent.Text = "Roll No";
                    ddlSearchBy.Items.Insert(0, new ListItem("Roll No", "3"));
                }
            }
            if (ddlSearchBy.Items.Count <= 1)
            {
                ddlSearchBy.Enabled = false;
            }
            else
            {
                ddlSearchBy.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }


    public void bindAlldetails(string AppNo)
    {
        try
        {
            string Query = "select distinct dg.degree_Code,(C.course_Name +' - '+dt.dept_Name) as Dept_Name from StudentRegisterHistory srh,Course c,Degree dg,Department dt where srh.degreeCode=dg.Degree_Code and srh.collegeCode=dg.college_code and c.college_code=dt.college_code and dg.college_code=c.college_code and c.college_code=dt.college_code and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and RedoType='2' and  srh.App_no='" + AppNo + "'";

            Query += " select distinct srh.batchYear from StudentRegisterHistory srh,Course c,Degree dg,Department dt where srh.degreeCode=dg.Degree_Code and srh.collegeCode=dg.college_code and c.college_code=dt.college_code and dg.college_code=c.college_code and c.college_code=dt.college_code and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and RedoType='2' and srh.App_no='" + AppNo + "'";
            Query += " select distinct srh.semester from StudentRegisterHistory srh,Course c,Degree dg,Department dt where srh.degreeCode=dg.Degree_Code and srh.collegeCode=dg.college_code and c.college_code=dt.college_code and dg.college_code=c.college_code and c.college_code=dt.college_code and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and RedoType='2' and srh.App_no='" + AppNo + "'";

            ds.Clear();
            ds = da.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds.Tables[0];
                ddldegree.DataTextField = "Dept_Name";
                ddldegree.DataValueField = "degree_Code";
                ddldegree.DataBind();
            }
            if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                ddlYear.DataSource = ds.Tables[1];
                ddlYear.DataTextField = "batchYear";
                ddlYear.DataValueField = "batchYear";
                ddlYear.DataBind();
            }
            if (ds.Tables.Count > 0 && ds.Tables[2].Rows.Count > 0)
            {
                ddlsemester.DataSource = ds.Tables[2];
                ddlsemester.DataTextField = "semester";
                ddlsemester.DataValueField = "semester";
                ddlsemester.DataBind();
            }

            txt_from.Attributes.Add("readonly", "readonly");
            txt_to.Attributes.Add("readonly", "readonly");
            if (ddldegree.Items.Count > 0 && ddlsemester.Items.Count > 0 && ddlYear.Items.Count > 0)
            {
                string NewQuery = "select convert(varchar(10),start_Date,103) as StratDate,convert(varchar(10),End_date,103) as EndDate from seminfo where degree_code ='" + ddldegree.SelectedValue + "' and semester='" + ddlsemester.SelectedValue + "' and batch_year='" + ddlYear.SelectedValue + "'";
                NewQuery += " select distinct srh.sections from StudentRegisterHistory srh,Course c,Degree dg,Department dt where srh.degreeCode=dg.Degree_Code and srh.collegeCode=dg.college_code and c.college_code=dt.college_code and dg.college_code=c.college_code and c.college_code=dt.college_code and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and RedoType='2' and srh.App_no='" + AppNo + "' and sections<>'' and  batchYear='" + ddlYear.SelectedValue + "' and degree_Code='" + ddldegree.SelectedValue + "' and semester='" + ddlsemester.SelectedValue + "'";
                ds.Clear();
                ds = da.select_method_wo_parameter(NewQuery, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    txt_from.Text = Convert.ToString(ds.Tables[0].Rows[0]["StratDate"]);
                    txt_to.Text = Convert.ToString(ds.Tables[0].Rows[0]["EndDate"]);
                }
                else
                {
                    txt_from.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txt_to.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    ddlsection.DataSource = ds.Tables[1];
                    ddlsection.DataTextField = "sections";
                    ddlsection.DataValueField = "sections";
                    ddlsection.DataBind();
                }
            }

        }
        catch
        {

        }
    }
    public void bindDegree()
    {
        try
        {
            string Query = "select distinct dg.degree_Code,(C.course_Name +' - '+dt.dept_Name) as Dept_Name from StudentRegisterHistory srh,Course c,Degree dg,Department dt where srh.degreeCode=dg.Degree_Code and srh.collegeCode=dg.college_code and c.college_code=dt.college_code and dg.college_code=c.college_code and c.college_code=dt.college_code and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and RedoType='2' and batchYear='" + ddlYear.SelectedValue + "' and srh.App_no='" + lblAppNo.Text.Trim() + "'";
            //
            ds.Clear();
            ds = da.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds.Tables[0];
                ddldegree.DataTextField = "Dept_Name";
                ddldegree.DataValueField = "degree_Code";
                ddldegree.DataBind();
            }
        }
        catch
        {

        }
    }
    public void bindSemester()
    {
        try
        {
            string Query = "select distinct Semester from StudentRegisterHistory srh,Course c,Degree dg,Department dt where srh.degreeCode=dg.Degree_Code and srh.collegeCode=dg.college_code and c.college_code=dt.college_code and dg.college_code=c.college_code and c.college_code=dt.college_code and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and RedoType='2' and batchYear='" + ddlYear.SelectedValue + "' and degree_code ='" + ddldegree.SelectedValue + "' and srh.App_no='" + lblAppNo.Text.Trim() + "'";
            //and srh.App_no='" + AppNo + "'
            ds.Clear();
            ds = da.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlsemester.DataSource = ds.Tables[0];
                ddlsemester.DataTextField = "Semester";
                ddlsemester.DataValueField = "Semester";
                ddlsemester.DataBind();
            }
        }
        catch
        {

        }
    }
    public void bindsemdetails()
    {
        try
        {
            string Query = "select distinct Sections from StudentRegisterHistory srh,Course c,Degree dg,Department dt where srh.degreeCode=dg.Degree_Code and srh.collegeCode=dg.college_code and c.college_code=dt.college_code and dg.college_code=c.college_code and c.college_code=dt.college_code and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and RedoType='2' and sections<>'' and batchYear='" + ddlYear.SelectedValue + "' and degree_code ='" + ddldegree.SelectedValue + "' and semester ='" + ddlsemester.SelectedValue + "' and srh.App_no='" + lblAppNo.Text.Trim() + "'";
            Query += "select convert(varchar(10),start_Date,103) as StratDate,convert(varchar(10),End_date,103) as EndDate from seminfo where degree_code ='" + ddldegree.SelectedValue + "' and semester='" + ddlsemester.SelectedValue + "' and batch_year='" + ddlYear.SelectedValue + "'";
            //and srh.App_no='" + AppNo + "'
            ds.Clear();
            ds = da.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlsection.DataSource = ds.Tables[0];
                ddlsection.DataTextField = "Sections";
                ddlsection.DataValueField = "Sections";
                ddlsection.DataBind();
            }
            if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                txt_from.Text = Convert.ToString(ds.Tables[1].Rows[0]["StratDate"]);
                txt_to.Text = Convert.ToString(ds.Tables[1].Rows[0]["EndDate"]);
            }
            else
            {
                txt_from.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_to.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        catch
        {

        }
    }
    public void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();
        lbl.Add(lblCollege);
        fields.Add(0);
        //lbl.Add(lbl_Stream);
        //fields.Add(1);
        //lbl.Add(lbldegree);
        //fields.Add(2);
        lbl.Add(lbldegree);
        fields.Add(3);
        lbl.Add(lblsemester);
        fields.Add(4);
        //Name -0, Stream - 1 ,Degree - 2, Branch - 3, Term - 4
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
        if (lbldegree.Text.Trim().ToLower() == "standard")
        {
            lblyear.Text = "Year";
        }
    }
    #endregion Bind Header

    #region Index Changed Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divStudentDetail.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            //divMainContents.Visible = false;
            //BindBatch();
            //BindDegree();
            //BindBranch();
            //BindSem();
            //BindSectionDetail();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlSearchBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divStudentDetail.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            //divMainContents.Visible = false;
            txtSearchStudent.Text = string.Empty;
            if (ddlSearchBy.Items.Count > 0)
            {
                lblSearchStudent.Text = ddlSearchBy.SelectedItem.Text;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : (ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13"), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
    }

    protected void ddlsemester_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            bindsemdetails();
        }
        catch
        {

        }
    }

    protected void ddldegree_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {

            bindSemester();
            bindsemdetails();
        }
        catch
        {

        }
    }



    protected void ddlYear_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            bindDegree();
            bindSemester();
            bindsemdetails();
        }
        catch
        {

        }
    }

    #endregion

    #region Button Events

    #region Search Student Click

    protected void btnSearchStudent_Click(object sender, EventArgs e)
    {
        try
        {
            divStudentDetail.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            string studentRegNo = string.Empty;
            string studentRollNo = string.Empty;
            string searchedStudent = string.Empty;
            string studentAdmissionNo = string.Empty;
            string studentApplicationNo = string.Empty;

            collegeCode = string.Empty;
            degreeCode = string.Empty;
            batchYear = string.Empty;
            semester = string.Empty;
            section = string.Empty;

            orderBy = string.Empty;
            orderBySetting = string.Empty;

            qry = string.Empty;
            qryCollegeCode = string.Empty;
            qryBatchYear = string.Empty;
            qryDegreeCode = string.Empty;
            qrySection = string.Empty;
            qrySemester = string.Empty;

            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblCollege.Text.Trim() + " Were Found";
                divPopAlert.Visible = true;
                return;
            }
            searchedStudent = txtSearchStudent.Text.Trim();
            if (!string.IsNullOrEmpty(searchedStudent))
            {
                if (ddlSearchBy.Items.Count > 0)
                {
                    string selectedItems = Convert.ToString(ddlSearchBy.SelectedItem.Text).Trim().ToLower();
                    string selectedValue = Convert.ToString(ddlSearchBy.SelectedValue).Trim();

                    switch (selectedValue)
                    {
                        case "1":
                            studentRollNo = dirAcc.selectScalarString("select Roll_No from Registration where Roll_Admit='" + searchedStudent + "'");
                            break;
                        case "2":
                            studentRollNo = dirAcc.selectScalarString("select Roll_No from Registration where Reg_no='" + searchedStudent + "'");
                            break;
                        case "3":
                            studentRollNo = searchedStudent.Trim();
                            break;
                    }

                }
                else
                {
                    if (CheckSchoolOrCollege(collegeCode))
                    {
                        studentRollNo = da.GetFunction("select Roll_No from Registration where Roll_Admit='" + searchedStudent + "'");
                    }
                    else
                    {
                        if (lblSearchStudent.Text.Trim().ToLower() == "register no" || lblSearchStudent.Text.Trim().ToLower() == "reg no")
                        {
                            studentRegNo = searchedStudent;
                            studentRollNo = da.GetFunction("select Roll_No from Registration where Reg_no='" + searchedStudent + "'");
                        }
                        else if (lblSearchStudent.Text.Trim().ToLower() == "admission no")
                        {
                            studentAdmissionNo = searchedStudent;
                            studentRollNo = da.GetFunction("select Roll_No from Registration where Roll_Admit='" + searchedStudent + "'");
                        }
                        else if (lblSearchStudent.Text.Trim().ToLower().Contains("student roll_no") || lblSearchStudent.Text.Trim().ToLower().Contains("roll no"))
                        {
                            studentRollNo = searchedStudent;
                        }
                    }
                }
                if (studentRollNo.Trim() != "")
                {
                    string QuerySelect = "select case when(ltrim(rtrim(isnull(c.type,'')))<>'') then ltrim(rtrim(isnull(c.type,'')))+case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then ' '+c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym+case when(ltrim(rtrim(isnull(r.Sections,'')))<>'') then ' '+ltrim(rtrim(isnull(r.Sections,'')))+' Semester : '+Convert(Varchar(20), r.Current_Semester) else ''+' Semester : '+Convert(Varchar(20), r.Current_Semester) end else c.Course_Name+' '+dt.dept_acronym+case when(ltrim(rtrim(isnull(r.Sections,'')))<>'') then ' '+ltrim(rtrim(isnull(r.Sections,'')))+' Semester : '+Convert(Varchar(20), r.Current_Semester) else ''+' Semester : '+Convert(Varchar(20), r.Current_Semester) end end  else case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym+case when(ltrim(rtrim(isnull(r.Sections,'')))<>'') then ' - '+ltrim(rtrim(isnull(r.Sections,'')))+' Semester : '+Convert(Varchar(20), r.Current_Semester) else ''+' Semester : '+Convert(Varchar(20), r.Current_Semester) end else c.Course_Name+' '+dt.dept_acronym+case when(ltrim(rtrim(isnull(r.Sections,'')))<>'') then ' - '+ltrim(rtrim(isnull(r.Sections,'')))+' Semester : '+Convert(Varchar(20), r.Current_Semester) else '' end end end as DegreeDetails,dt.dept_acronym + CASE WHEN (LTRIM(RTRIM(ISNULL(r.Sections, ''))) <> '') THEN ' - ' + LTRIM(RTRIM(ISNULL(r.Sections, '')))+' Semester : '+Convert(Varchar(20), r.Current_Semester) ELSE ''+' Semester : '+Convert(Varchar(20), r.Current_Semester) END AS ClassDetails,r.Batch_year,r.degree_code,r.current_semester,LTRIM(RTRIM(ISNULL(r.Sections, ''))) as Sections,r.college_code,r.app_no,ISNULL(convert(varchar(200),r.serialno),'') as serialNo,r.reg_no,r.roll_no,r.Roll_Admit,r.Stud_Type,r.Stud_Name,Convert(varchar(20),r.Adm_Date,103) as AdmissionDate from Registration r,Course c,Degree dg,Department dt,collinfo clg where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=c.college_code and dt.college_code=clg.college_code and clg.college_code=r.college_code and r.college_code=dg.college_code and r.college_code=dt.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and dg.Degree_Code=r.degree_code and r.roll_no='" + studentRollNo + "' and r.college_code ='" + ddlCollege.SelectedValue + "'";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(QuerySelect, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        lblStudentName.Text = Convert.ToString(ds.Tables[0].Rows[0]["stud_Name"]);
                        lblStudentRollNo.Text = Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]);
                        lblAdmissionNo.Text = Convert.ToString(ds.Tables[0].Rows[0]["roll_admit"]);
                        lblRegNo.Text = Convert.ToString(ds.Tables[0].Rows[0]["reg_no"]);
                        if (lblCollege.Text.Trim().ToLower() != "school")
                        {
                            lblClassName.Text = Convert.ToString(ds.Tables[0].Rows[0]["degreedetails"]);
                        }
                        else
                        {
                            lblClassName.Text = Convert.ToString(ds.Tables[0].Rows[0]["classdetails"]);
                        }
                        lblAppNo.Text = Convert.ToString(ds.Tables[0].Rows[0]["app_no"]);
                        bindAlldetails(Convert.ToString(ds.Tables[0].Rows[0]["app_no"]));
                        divStudentDetail.Visible = true;
                        divShowPrivious.Visible = true;
                        setLabelText();
                    }
                    else
                    {
                        divPopAlert.Visible = true;
                        Showgrid.Visible = false;
                        divStudentDetail.Visible = false;
                        divShowPrivious.Visible = false;
                        print.Visible = false;
                        lblAlertMsg.Text = "No Records Found";
                    }
                }


            }
            else
            {
                lblAlertMsg.Text = "Please Enter " + lblSearchStudent.Text.Trim();
                divPopAlert.Visible = true;
                Showgrid.Visible = false;
                divStudentDetail.Visible = false;
                divShowPrivious.Visible = false;
                print.Visible = false;
                return;
            }
        }
        catch (Exception ex)
        {

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

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {

            btnPrint11();
            string FromDate = txt_from.Text;
            string ToDate = txt_to.Text;
            string[] FromSplit = FromDate.Split('/');
            string[] ToSplit = ToDate.Split('/');
            DateTime FromDt = Convert.ToDateTime(FromSplit[1] + "/" + FromSplit[0] + "/" + FromSplit[2]);
            DateTime ToDt = Convert.ToDateTime(ToSplit[1] + "/" + ToSplit[0] + "/" + ToSplit[2]);
            Session["attdaywisecla"] = "0";
            string daywisecal = da.GetFunction("select value from Master_Settings where settings='Attendance Day Wise Calculation'");
            if (daywisecal.Trim() == "1")
            {
                Session["attdaywisecla"] = "1";
            }

            string NewQuery = "select convert(varchar(10),start_Date,103) as StratDate,convert(varchar(10),End_date,103) as EndDate from seminfo where degree_code ='" + ddldegree.SelectedValue + "' and semester='" + ddlsemester.SelectedValue + "' and batch_year='" + ddlYear.SelectedValue + "' and start_Date<='" + FromDt.ToString("MM/dd/yyyy") + "' and End_date>='" + ToDt.ToString("MM/dd/yyyy") + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(NewQuery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                getdata(FromDt, ToDt);
            }
            else
            {
                divPopAlert.Visible = true;
                Showgrid.Visible = false;
                print.Visible = false;
                lblAlertMsg.Text = "Please Select Correct From and To Date";
            }
        }
        catch
        {

        }
    }
    public void getdata(DateTime FromDate, DateTime Todate)
    {

        string AppNo = lblAppNo.Text.Trim();
        string batchYear = ddlYear.SelectedItem.Text;
        string degreecode = ddldegree.SelectedValue;
        string semester = ddlsemester.SelectedItem.Text;
        string section = string.Empty;
        if (ddlsection.Items.Count > 0)
        {
            section = ddlsection.SelectedItem.Text;
        }
        string SelectQuery = "select * from StudentRegisterHistory srh,Course c,Degree dg,Department dt where srh.degreeCode=dg.Degree_Code and srh.collegeCode=dg.college_code and c.college_code=dt.college_code and dg.college_code=c.college_code and c.college_code=dt.college_code and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and RedoType='2' and batchYear='" + batchYear + "' and degree_code ='" + degreecode + "' and srh.App_no='" + AppNo + "' and semester ='" + semester + "'";
        if (section.Trim() != "")
        {
            SelectQuery += " and sections ='" + section + "'";
        }
        SelectQuery += " select No_of_hrs_per_day,degree_code,no_of_hrs_I_half_day,no_of_hrs_II_half_day,min_pres_I_half_day,min_pres_II_half_day,semester,min_hrs_per_day from PeriodAttndSchedule where degree_code ='" + degreecode + "' and semester='" + semester + "'";
        ds.Clear();
        ds = da.select_method_wo_parameter(SelectQuery, "Text");
        string Roll_No = string.Empty;
        if (ds.Tables.Count > 1 && ds.Tables[0].Rows.Count > 0)
        {
            Roll_No = Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]);
            loadattendance(Roll_No, AppNo, ddlReportType.SelectedIndex, FromDate, Todate, ds);
        }
        else
        {
            divPopAlert.Visible = true;
            Showgrid.Visible = false;
            print.Visible = false;
            lblAlertMsg.Text = "No Records Found";
        }
    }
    public void loadattendance(string Roll_No, string AppNo, int Index, DateTime FromDate, DateTime Todate, DataSet dnew)
    {
        try
        {
            if (Index == 0) // Detailed
            {
                #region Details Report
                #region headerBind
                HeaderBind(Index, dnew);
                #endregion
                #region Attendance Parameter Value
                DataView dvattnd = new DataView();
                hat.Clear();
                hat.Add("colege_code", Convert.ToString(ddlCollege.SelectedValue));
                DataSet dsattva = da.select_method("ATT_MASTER_SETTING", hat, "sp");
                Dictionary<string, string> dicattval = new Dictionary<string, string>();
                Dictionary<string, string> dicDispval = new Dictionary<string, string>();
                Dictionary<string, string> discHoliday = new Dictionary<string, string>();
                if (dsattva.Tables.Count > 0 && dsattva.Tables[0].Rows.Count > 0)
                {
                    for (int at = 0; at < dsattva.Tables[0].Rows.Count; at++)
                    {
                        string leavcode = Convert.ToString(dsattva.Tables[0].Rows[at]["leavecode"]).Trim();
                        string calc = Convert.ToString(dsattva.Tables[0].Rows[at]["calcflag"]).Trim();
                        string DispText = Convert.ToString(dsattva.Tables[0].Rows[at]["dispText"]).Trim();
                        if (!dicattval.ContainsKey(leavcode.Trim()))
                        {
                            dicattval.Add(leavcode.Trim(), calc);
                            dicDispval.Add(leavcode.Trim(), DispText);
                        }
                    }
                    #region PeriodAttend Schedulue
                    int MaxNohourPerDay = 0;
                    int MaxNoHourFisrtHalf = 0;
                    int MaxNohourSecondHalf = 0;
                    int MinNoHourFisrtHalf = 0;
                    int MinNohourSecondHalf = 0;
                    int MinHourPresentDay = 0;
                    int PresentHour = 0;
                    int MinPresentFirstHalf = 0;
                    int MinPresentSecondHalf = 0;
                    int NotEnterCount = 0;
                    int FirstHalfNotEnter = 0;
                    int SecondHalfNotEnter = 0;
                    int ConHour = 0;
                    int AttnHour = 0;
                    double ConDays = 0;
                    double AttnDays = 0;
                    if (dnew.Tables.Count > 0 && dnew.Tables[1].Rows.Count > 0)
                    {
                        int.TryParse(Convert.ToString(ds.Tables[1].Rows[0]["No_of_hrs_per_day"]), out MaxNohourPerDay);
                        int.TryParse(Convert.ToString(ds.Tables[1].Rows[0]["no_of_hrs_I_half_day"]), out MaxNoHourFisrtHalf);
                        int.TryParse(Convert.ToString(ds.Tables[1].Rows[0]["no_of_hrs_II_half_day"]), out MaxNohourSecondHalf);
                        int.TryParse(Convert.ToString(ds.Tables[1].Rows[0]["min_pres_I_half_day"]), out MinNoHourFisrtHalf);
                        int.TryParse(Convert.ToString(ds.Tables[1].Rows[0]["min_pres_II_half_day"]), out MinNohourSecondHalf);
                        int.TryParse(Convert.ToString(ds.Tables[1].Rows[0]["min_hrs_per_day"]), out MinHourPresentDay);
                    }
                    #endregion
                    #region HolidaySelection
                    string qry = "select convert(varchar(10), holiday_date,101) as holiday_date,halforfull,morning,evening,holiday_desc FROM holidayStudents where holiday_date between '" + FromDate.ToString("MM/dd/yyyy") + "' and '" + Todate.ToString("MM/dd/yyyy") + "' and degree_code=" + ddldegree.SelectedValue + " and semester=" + ddlsemester.SelectedItem.Text + "";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(qry, "text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int intholy = 0; intholy < ds.Tables[0].Rows.Count; intholy++)
                        {
                            if (!discHoliday.ContainsKey(Convert.ToString(ds.Tables[0].Rows[intholy]["holiday_date"])))
                            {
                                discHoliday.Add(Convert.ToString(ds.Tables[0].Rows[intholy]["holiday_date"]), Convert.ToString(ds.Tables[0].Rows[intholy]["holiday_desc"]) + "$" + Convert.ToString(ds.Tables[0].Rows[intholy]["halforfull"]) + "$" + Convert.ToString(ds.Tables[0].Rows[intholy]["morning"]) + "$" + Convert.ToString(ds.Tables[0].Rows[intholy]["evening"]));
                            }
                        }
                    }
                    #endregion
                    #region  Attendance Details
                    string AttendQuery = "select * from Attendance where Roll_no ='" + Roll_No + "' and Att_App_no='" + AppNo + "'";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(AttendQuery, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string frdate_datetime = string.Empty;
                        string todate_datetime = string.Empty;
                        string[] from_split2;
                        int Day = 0;
                        int Month = 0;
                        int Year = 0;
                        int MonthYear = 0;
                        int Sno = 0;
                        string FilterIndexValue = string.Empty;
                        string StudentDetails = string.Empty;
                        if (lblCollege.Text.Trim().ToLower() == "college")
                        {
                            StudentDetails = Convert.ToString(lblStudentName.Text + " - " + ddlYear.SelectedItem.Text + " - " + ddldegree.SelectedItem.Text + " - Sem " + ddlsemester.SelectedItem.Text);
                            if (ddlsection.Items.Count > 0)
                            {
                                StudentDetails += " - Sec " + ddlsection.SelectedItem.Text;
                            }
                        }
                        else
                        {
                            StudentDetails = Convert.ToString(lblStudentName.Text + " - " + ddlYear.SelectedItem.Text + " - " + ddldegree.SelectedItem.Text + " - Term " + ddlsemester.SelectedItem.Text);
                            if (ddlsection.Items.Count > 0)
                            {
                                StudentDetails += " - Sec " + ddlsection.SelectedItem.Text;
                            }
                        }
                        drow = data.NewRow();
                        drow["SNo"] = Convert.ToString(StudentDetails);
                        data.Rows.Add(drow);

                        #region DateWise Calculation
                        Dictionary<int, string> dicforecolhol = new Dictionary<int, string>();
                        Dictionary<string, string> dicforeatt = new Dictionary<string, string>();
                        while (FromDate <= Todate)
                        {
                            PresentHour = 0;
                            MinPresentFirstHalf = 0;
                            MinPresentSecondHalf = 0;
                            NotEnterCount = 0;
                            FirstHalfNotEnter = 0;
                            SecondHalfNotEnter = 0;
                            from_split2 = Convert.ToString(FromDate.ToString("MM/dd/yyyy")).Split('/');
                            if (from_split2.Length > 0)
                            {
                                Day = Convert.ToInt16(Convert.ToString(from_split2[1]).Trim());
                                Month = Convert.ToInt16(Convert.ToString(from_split2[0]).Trim());
                                Year = Convert.ToInt16(Convert.ToString(from_split2[2]).Trim());
                            }
                            MonthYear = (12 * Year) + Month;
                            ds.Tables[0].DefaultView.RowFilter = "month_year='" + MonthYear + "'";
                            dvattnd = ds.Tables[0].DefaultView;
                            if (dvattnd.Count > 0)
                            {
                                Sno++;

                                drow = data.NewRow();
                                drow["SNo"] = Convert.ToString(Sno);
                                drow["Date"] = Convert.ToString(FromDate.ToString("dd/MM/yyyy"));


                                if (data.Columns.Count > 0 && FromDate.ToString("dddd") != "Sunday" && !discHoliday.ContainsKey(Convert.ToString(FromDate.ToString("MM/dd/yyyy"))))
                                {
                                    int Loopcount = 0;
                                    string STatus = string.Empty;
                                    if (dichrdet.Count > 0)
                                    {
                                        foreach (KeyValuePair<int, string> dr in dichrdet)
                                        {
                                            int k = dr.Key;
                                            string hrhead = dr.Value;
                                            string CindexValue = k.ToString();
                                            Loopcount += 1;
                                            if (CindexValue.Trim() != "")
                                            {
                                                FilterIndexValue = "d" + Day + "d" + CindexValue + "";
                                                string AttndValue = Convert.ToString(dvattnd[0][FilterIndexValue]);
                                                if (AttndValue.Trim() != "")
                                                {
                                                    drow[hrhead] = Convert.ToString(dicDispval[AttndValue]);

                                                    string CalValue = Convert.ToString(dicattval[AttndValue]);

                                                    if (!dicforeatt.ContainsKey(Convert.ToString(dicDispval[AttndValue])))
                                                        dicforeatt.Add(Convert.ToString(dicDispval[AttndValue]), Convert.ToString(CalValue));
                                                    ConHour += 1;
                                                    if (CalValue.Trim() == "0")
                                                    {
                                                        PresentHour += 1;
                                                        if (Loopcount <= MaxNoHourFisrtHalf)
                                                        {
                                                            MinPresentFirstHalf += 1;
                                                        }
                                                        else
                                                        {
                                                            MinPresentSecondHalf += 1;
                                                        }
                                                        //attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, intFC].ForeColor = Color.Green;

                                                        AttnHour += 1;
                                                    }
                                                    else if (CalValue.Trim() == "1")
                                                    {
                                                        // attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, intFC].ForeColor = Color.Red;
                                                    }
                                                    else if (CalValue.Trim() == "2")
                                                    {
                                                        //attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, intFC].ForeColor = Color.Blue;
                                                    }
                                                }
                                                else
                                                {
                                                    NotEnterCount += 1;
                                                    if (Loopcount <= MaxNoHourFisrtHalf)
                                                    {
                                                        FirstHalfNotEnter += 1;
                                                    }
                                                    else
                                                    {
                                                        SecondHalfNotEnter += 1;
                                                    }
                                                    drow[hrhead] = Convert.ToString("-");

                                                }
                                            }
                                        }
                                    }
                                    if (MinHourPresentDay != 0 && Session["attdaywisecla"] != null && Session["attdaywisecla"] == "1")
                                    {

                                        if (MinHourPresentDay <= PresentHour)
                                        {
                                            STatus = "FP";
                                            ConDays += 1;
                                            AttnDays += 1;
                                        }
                                        else if (MinHourPresentDay <= (PresentHour + NotEnterCount))
                                        {
                                            STatus = "";
                                        }
                                        else
                                        {
                                            STatus = "FA";
                                            ConDays += 1;
                                        }
                                    }
                                    else
                                    {

                                        if (MinNoHourFisrtHalf <= MinPresentFirstHalf && MinNohourSecondHalf <= MinPresentSecondHalf)
                                        {
                                            STatus = "FP";
                                            ConDays += 1;
                                            AttnDays += 1;
                                        }
                                        else if (MinNoHourFisrtHalf <= (MinPresentFirstHalf + FirstHalfNotEnter) && MinNohourSecondHalf <= (MinPresentSecondHalf + SecondHalfNotEnter))
                                        {
                                            STatus = "";
                                            if (NotEnterCount != MaxNohourPerDay)
                                            {
                                                ConDays += 1;
                                            }
                                        }
                                        else if (MinNoHourFisrtHalf <= MinPresentFirstHalf || MinNohourSecondHalf <= MinPresentSecondHalf)
                                        {
                                            STatus = "HP";
                                            ConDays += 1;
                                            AttnDays += 0.5;
                                        }
                                        else if (MinNoHourFisrtHalf > (MinPresentFirstHalf) && (MinNohourSecondHalf > (MinPresentSecondHalf)))
                                        {
                                            STatus = "FA";
                                            ConDays += 1;
                                        }
                                        else if (MinNoHourFisrtHalf > (MinPresentFirstHalf) || (MinNohourSecondHalf > (MinPresentSecondHalf)))
                                        {
                                            STatus = "HA";
                                            ConDays += 0.5;
                                        }

                                    }
                                    drow["Attendance Status"] = Convert.ToString(STatus);

                                }
                                else
                                {
                                    string Discription = string.Empty;
                                    if (Convert.ToString(FromDate.ToString("dddd")) == "Sunday")
                                    {
                                        Discription = "Sunday";
                                    }
                                    else if (discHoliday.ContainsKey(Convert.ToString(FromDate.ToString("MM/dd/yyyy"))))
                                    {

                                        Discription = Convert.ToString(discHoliday[Convert.ToString(FromDate.ToString("MM/dd/yyyy"))]).Split('$')[0];
                                    }

                                    string value = dichrdet[1];
                                    drow[value] = Discription.ToString();
                                    int nocnt = Sno;
                                    dicforecolhol.Add(nocnt + 1, Discription);
                                    //attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 2].ForeColor = ColorTranslator.FromHtml("#003366");

                                }
                                data.Rows.Add(drow);
                            }
                            FromDate = FromDate.AddDays(1);
                        }
                        #endregion
                        #region Cumulative Calucualtion
                        if (data.Rows.Count > 1)
                        {
                            double HourPer = 0;
                            HourPer = (Convert.ToDouble(AttnHour) / Convert.ToDouble(ConHour)) * 100;
                            double DaysPer = 0;
                            DaysPer = (AttnDays / ConDays) * 100;

                            string value = value = dichrdet[1];
                            drow = data.NewRow();
                            drow["SNo"] = "Con Hours";
                            drow[value] = ConHour.ToString();
                            data.Rows.Add(drow);
                            drow = data.NewRow();
                            drow["SNo"] = "Attn Hours";
                            drow[value] = AttnHour.ToString();
                            data.Rows.Add(drow);
                            drow = data.NewRow();
                            drow["SNo"] = "Hours %";
                            drow[value] = Convert.ToString(Math.Round(HourPer, 2));
                            data.Rows.Add(drow);
                            drow = data.NewRow();
                            drow["SNo"] = "Con Days";
                            drow[value] = ConDays.ToString();
                            data.Rows.Add(drow);
                            drow = data.NewRow();
                            drow["SNo"] = "Attn Days";
                            drow[value] = AttnDays.ToString();
                            data.Rows.Add(drow);
                            drow = data.NewRow();
                            drow["SNo"] = "Days %";
                            drow[value] = Convert.ToString(Math.Round(DaysPer, 2));
                            data.Rows.Add(drow);


                            if (data.Columns.Count > 0 && data.Rows.Count > 0)
                            {
                                Showgrid.DataSource = data;
                                Showgrid.DataBind();
                                Showgrid.Visible = true;
                                print.Visible = true;

                                Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                Showgrid.Rows[0].Font.Bold = true;
                                Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                                int d = Convert.ToInt32(data.Columns.Count);
                                int colspan = Convert.ToInt32(data.Columns.Count - 3);
                                Showgrid.Rows[1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                Showgrid.Rows[1].Cells[0].ColumnSpan = d;
                                for (int a = 1; a < d; a++)
                                    Showgrid.Rows[1].Cells[a].Visible = false;

                                int j = 0;
                                int g = 0;
                                int colcnt = data.Columns.Count;
                                int rowcnt = Showgrid.Rows.Count - 6;
                                for (g = 0; g < data.Columns.Count; g++)
                                {
                                    for (j = 1; j < Showgrid.Rows.Count; j++)
                                        Showgrid.Rows[j].Cells[g].HorizontalAlign = HorizontalAlign.Center;
                                    if (g == 0)
                                        for (int k = rowcnt; k < Showgrid.Rows.Count; k++)
                                        {
                                            Showgrid.Rows[k].Cells[g].ColumnSpan = 2;
                                            Showgrid.Rows[k].Cells[1].Visible = false;
                                        }
                                }
                                for (g = 2; g < data.Columns.Count - 1; g++)
                                {
                                    for (j = 2; j < Showgrid.Rows.Count - 6; j++)
                                    {
                                        Showgrid.Rows[j].Cells[g].BorderColor = Color.Black;

                                        if (!dicforecolhol.ContainsKey(j))
                                        {

                                            string hrvalue = Showgrid.Rows[j].Cells[g].Text;
                                            if (!dicforeatt.ContainsKey(hrvalue))
                                                Showgrid.Rows[j].Cells[g].ForeColor = Color.Black;
                                            else
                                            {

                                                //int myKey = dicforeatt.FirstOrDefault(x => x.Value == hrvalue).Key;
                                                string keyvalue = dicforeatt[hrvalue];
                                                if (keyvalue == "0")
                                                    Showgrid.Rows[j].Cells[g].ForeColor = Color.Green;
                                                else if (keyvalue == "1")
                                                    Showgrid.Rows[j].Cells[g].ForeColor = Color.Red;
                                                else if (keyvalue == "2")
                                                    Showgrid.Rows[j].Cells[g].ForeColor = Color.Blue;
                                            }

                                        }
                                        else
                                        {
                                            if (g == 2)
                                            {
                                                Showgrid.Rows[j].Cells[g].ForeColor = Color.DarkBlue;
                                                Showgrid.Rows[j].Cells[g].HorizontalAlign = HorizontalAlign.Center;
                                                Showgrid.Rows[j].Cells[g].ColumnSpan = colspan;
                                                for (int a = 3; a < data.Columns.Count - 1; a++)
                                                    Showgrid.Rows[j].Cells[a].Visible = false;
                                            }
                                        }

                                    }

                                }


                            }
                        }
                        else
                        {
                            divPopAlert.Visible = true;
                            Showgrid.Visible = false;
                            print.Visible = false;
                            lblAlertMsg.Text = "No Records Found";
                        }
                        #endregion
                    }
                    else
                    {
                        divPopAlert.Visible = true;
                        Showgrid.Visible = false;
                        print.Visible = false;
                        lblAlertMsg.Text = "No Records Found";
                    }
                    #endregion
                }
                else
                {
                    divPopAlert.Visible = true;
                    Showgrid.Visible = false;
                    print.Visible = false;
                    lblAlertMsg.Text = "Please Update Attendnce Parameter";
                }
                #endregion
                #endregion
            }
            else if (Index == 1) // Cumulative
            {
                #region headerbind
                HeaderBind(Index, dnew);
                #endregion
                #region Cumulative Report
                #region Attendance Parameter Value
                DataView dvattnd = new DataView();
                hat.Clear();
                hat.Add("colege_code", Convert.ToString(ddlCollege.SelectedValue));
                DataSet dsattva = da.select_method("ATT_MASTER_SETTING", hat, "sp");
                Dictionary<string, string> dicattval = new Dictionary<string, string>();
                Dictionary<string, string> dicDispval = new Dictionary<string, string>();
                Dictionary<string, string> discHoliday = new Dictionary<string, string>();
                if (dsattva.Tables.Count > 0 && dsattva.Tables[0].Rows.Count > 0)
                {
                    for (int at = 0; at < dsattva.Tables[0].Rows.Count; at++)
                    {
                        string leavcode = Convert.ToString(dsattva.Tables[0].Rows[at]["leavecode"]).Trim();
                        string calc = Convert.ToString(dsattva.Tables[0].Rows[at]["calcflag"]).Trim();
                        string DispText = Convert.ToString(dsattva.Tables[0].Rows[at]["dispText"]).Trim();
                        if (!dicattval.ContainsKey(leavcode.Trim()))
                        {
                            dicattval.Add(leavcode.Trim(), calc);
                            dicDispval.Add(leavcode.Trim(), DispText);
                        }
                    }
                    #region PeriodAttend Schedulue
                    int MaxNohourPerDay = 0;
                    int MaxNoHourFisrtHalf = 0;
                    int MaxNohourSecondHalf = 0;
                    int MinNoHourFisrtHalf = 0;
                    int MinNohourSecondHalf = 0;
                    int MinHourPresentDay = 0;
                    int PresentHour = 0;
                    int MinPresentFirstHalf = 0;
                    int MinPresentSecondHalf = 0;
                    int NotEnterCount = 0;
                    int FirstHalfNotEnter = 0;
                    int SecondHalfNotEnter = 0;
                    int ConHour = 0;
                    int AttnHour = 0;
                    double ConDays = 0;
                    double AttnDays = 0;
                    if (dnew.Tables.Count > 0 && dnew.Tables[1].Rows.Count > 0)
                    {
                        int.TryParse(Convert.ToString(ds.Tables[1].Rows[0]["No_of_hrs_per_day"]), out MaxNohourPerDay);
                        int.TryParse(Convert.ToString(ds.Tables[1].Rows[0]["no_of_hrs_I_half_day"]), out MaxNoHourFisrtHalf);
                        int.TryParse(Convert.ToString(ds.Tables[1].Rows[0]["no_of_hrs_II_half_day"]), out MaxNohourSecondHalf);
                        int.TryParse(Convert.ToString(ds.Tables[1].Rows[0]["min_pres_I_half_day"]), out MinNoHourFisrtHalf);
                        int.TryParse(Convert.ToString(ds.Tables[1].Rows[0]["min_pres_II_half_day"]), out MinNohourSecondHalf);
                        int.TryParse(Convert.ToString(ds.Tables[1].Rows[0]["min_hrs_per_day"]), out MinHourPresentDay);
                    }
                    #endregion
                    #region HolidaySelection
                    string qry = "select convert(varchar(10), holiday_date,101) as holiday_date,halforfull,morning,evening,holiday_desc FROM holidayStudents where holiday_date between '" + FromDate.ToString("MM/dd/yyyy") + "' and '" + Todate.ToString("MM/dd/yyyy") + "' and degree_code=" + ddldegree.SelectedValue + " and semester=" + ddlsemester.SelectedItem.Text + "";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(qry, "text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int intholy = 0; intholy < ds.Tables[0].Rows.Count; intholy++)
                        {
                            if (!discHoliday.ContainsKey(Convert.ToString(ds.Tables[0].Rows[intholy]["holiday_date"])))
                            {
                                discHoliday.Add(Convert.ToString(ds.Tables[0].Rows[intholy]["holiday_date"]), Convert.ToString(ds.Tables[0].Rows[intholy]["holiday_desc"]) + "$" + Convert.ToString(ds.Tables[0].Rows[intholy]["halforfull"]) + "$" + Convert.ToString(ds.Tables[0].Rows[intholy]["morning"]) + "$" + Convert.ToString(ds.Tables[0].Rows[intholy]["evening"]));
                            }
                        }
                    }
                    #endregion
                    #region  Attendance Details
                    string AttendQuery = "select * from Attendance where Roll_no ='" + Roll_No + "' and Att_App_no='" + AppNo + "'";
                    ds.Clear();
                    ds = da.select_method_wo_parameter(AttendQuery, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string frdate_datetime = string.Empty;
                        string todate_datetime = string.Empty;
                        string[] from_split2;
                        int Day = 0;
                        int Month = 0;
                        int Year = 0;
                        int MonthYear = 0;
                        int Sno = 0;
                        string FilterIndexValue = string.Empty;
                        #region DateWise Calculation
                        while (FromDate <= Todate)
                        {
                            PresentHour = 0;
                            MinPresentFirstHalf = 0;
                            MinPresentSecondHalf = 0;
                            NotEnterCount = 0;
                            FirstHalfNotEnter = 0;
                            SecondHalfNotEnter = 0;
                            from_split2 = Convert.ToString(FromDate.ToString("MM/dd/yyyy")).Split('/');
                            if (from_split2.Length > 0)
                            {
                                Day = Convert.ToInt16(Convert.ToString(from_split2[1]).Trim());
                                Month = Convert.ToInt16(Convert.ToString(from_split2[0]).Trim());
                                Year = Convert.ToInt16(Convert.ToString(from_split2[2]).Trim());
                            }
                            MonthYear = (12 * Year) + Month;
                            ds.Tables[0].DefaultView.RowFilter = "month_year='" + MonthYear + "'";
                            dvattnd = ds.Tables[0].DefaultView;
                            if (dvattnd.Count > 0)
                            {

                                if (data.Columns.Count > 0 && FromDate.ToString("dddd") != "Sunday" && !discHoliday.ContainsKey(Convert.ToString(FromDate.ToString("MM/dd/yyyy"))))
                                {
                                    int Loopcount = 0;
                                    string STatus = string.Empty;
                                    for (int intFC = 0; intFC < MaxNohourPerDay; intFC++)
                                    {
                                        string CindexValue = Convert.ToString(intFC + 1);
                                        Loopcount += 1;
                                        if (CindexValue.Trim() != "")
                                        {
                                            FilterIndexValue = "d" + Day + "d" + CindexValue + "";
                                            string AttndValue = Convert.ToString(dvattnd[0][FilterIndexValue]);
                                            if (AttndValue.Trim() != "")
                                            {

                                                string CalValue = Convert.ToString(dicattval[AttndValue]);
                                                ConHour += 1;
                                                if (CalValue.Trim() == "0")
                                                {
                                                    PresentHour += 1;
                                                    if (Loopcount <= MaxNoHourFisrtHalf)
                                                    {
                                                        MinPresentFirstHalf += 1;
                                                    }
                                                    else
                                                    {
                                                        MinPresentSecondHalf += 1;
                                                    }

                                                    AttnHour += 1;
                                                }
                                            }
                                            else
                                            {
                                                NotEnterCount += 1;
                                                if (Loopcount <= MaxNoHourFisrtHalf)
                                                {
                                                    FirstHalfNotEnter += 1;
                                                }
                                                else
                                                {
                                                    SecondHalfNotEnter += 1;
                                                }
                                            }
                                        }
                                    }
                                    if (MinHourPresentDay != 0 && Session["attdaywisecla"] != null && Session["attdaywisecla"] == "1")
                                    {

                                        if (MinHourPresentDay <= PresentHour)
                                        {
                                            STatus = "FP";
                                            ConDays += 1;
                                            AttnDays += 1;
                                        }
                                        else if (MinHourPresentDay <= (PresentHour + NotEnterCount))
                                        {
                                            STatus = "";
                                        }
                                        else
                                        {
                                            STatus = "FA";
                                            ConDays += 1;
                                        }
                                    }
                                    else
                                    {


                                        if (MinNoHourFisrtHalf <= MinPresentFirstHalf && MinNohourSecondHalf <= MinPresentSecondHalf)
                                        {
                                            STatus = "FP";
                                            ConDays += 1;
                                            AttnDays += 1;
                                        }
                                        else if (MinNoHourFisrtHalf <= (MinPresentFirstHalf + FirstHalfNotEnter) && MinNohourSecondHalf <= (MinPresentSecondHalf + SecondHalfNotEnter))
                                        {
                                            STatus = "";
                                            if (NotEnterCount != MaxNohourPerDay)
                                            {
                                                ConDays += 1;
                                            }
                                        }
                                        else if (MinNoHourFisrtHalf <= MinPresentFirstHalf || MinNohourSecondHalf <= MinPresentSecondHalf)
                                        {
                                            STatus = "HP";
                                            ConDays += 1;
                                            AttnDays += 0.5;
                                        }
                                        else if (MinNoHourFisrtHalf > (MinPresentFirstHalf) && (MinNohourSecondHalf > (MinPresentSecondHalf)))
                                        {
                                            STatus = "FA";
                                            ConDays += 1;
                                        }
                                        else if (MinNoHourFisrtHalf > (MinPresentFirstHalf) || (MinNohourSecondHalf > (MinPresentSecondHalf)))
                                        {
                                            STatus = "HA";
                                            ConDays += 0.5;
                                        }
                                    }
                                }
                                else
                                {
                                    string Discription = string.Empty;
                                    if (Convert.ToString(FromDate.ToString("dddd")) == "Sunday")
                                    {
                                        Discription = "Sunday";
                                    }
                                    else if (discHoliday.ContainsKey(Convert.ToString(FromDate.ToString("MM/dd/yyyy"))))
                                    {
                                        Discription = Convert.ToString(discHoliday[Convert.ToString(FromDate.ToString("MM/dd/yyyy"))]).Split('$')[0];
                                    }
                                }
                            }
                            FromDate = FromDate.AddDays(1);
                        }
                        #endregion
                        #region Cumulative Calucualtion
                        if (ConHour != 0 && ConDays != 0)
                        {
                            string StudentDetails = string.Empty;
                            if (lblCollege.Text.Trim().ToLower() == "college")
                            {
                                StudentDetails = Convert.ToString(lblStudentName.Text + " - " + ddlYear.SelectedItem.Text + " - " + ddldegree.SelectedItem.Text + " - Sem " + ddlsemester.SelectedItem.Text);
                                if (ddlsection.Items.Count > 0)
                                {
                                    StudentDetails += " - Sec " + ddlsection.SelectedItem.Text;
                                }
                            }
                            else
                            {
                                StudentDetails = Convert.ToString(lblStudentName.Text + " - " + ddlYear.SelectedItem.Text + " - " + ddldegree.SelectedItem.Text + " - Term " + ddlsemester.SelectedItem.Text);
                                if (ddlsection.Items.Count > 0)
                                {
                                    StudentDetails += " - Sec " + ddlsection.SelectedItem.Text;
                                }
                            }
                            drow = data.NewRow();
                            drow["SNo"] = StudentDetails.ToString();
                            data.Rows.Add(drow);

                            double HourPer = 0;
                            HourPer = (Convert.ToDouble(AttnHour) / Convert.ToDouble(ConHour)) * 100;
                            double DaysPer = 0;
                            DaysPer = (AttnDays / ConDays) * 100;
                            Sno++;
                            drow = data.NewRow();
                            drow["SNo"] = Convert.ToString(Sno);

                            drow["Con Hour"] = Convert.ToString(ConHour);
                            drow["Attn Hour"] = Convert.ToString(AttnHour);
                            drow["Hour %"] = Convert.ToString(Math.Round(HourPer, 2));
                            drow["Con Days"] = Convert.ToString(ConDays);
                            drow["Attn Days"] = Convert.ToString(AttnDays);
                            drow["Days %"] = Convert.ToString(Math.Round(DaysPer, 2));
                            data.Rows.Add(drow);

                            if (data.Columns.Count > 0 && data.Rows.Count > 0)
                            {
                                Showgrid.DataSource = data;
                                Showgrid.DataBind();
                                Showgrid.Visible = true;
                                print.Visible = true;

                                Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                Showgrid.Rows[0].Font.Bold = true;
                                Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                                int d = Convert.ToInt32(data.Columns.Count);
                                Showgrid.Rows[1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                Showgrid.Rows[1].Cells[0].ColumnSpan = d;
                                for (int a = 1; a < d; a++)
                                    Showgrid.Rows[1].Cells[a].Visible = false;
                                for (int g = 0; g < data.Columns.Count; g++)
                                {
                                    for (int j = 0; j < Showgrid.Rows.Count; j++)
                                        Showgrid.Rows[j].Cells[g].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }

                        }
                        else
                        {
                            divPopAlert.Visible = true;
                            Showgrid.Visible = false;
                            print.Visible = false;
                            lblAlertMsg.Text = "No Records Found";
                        }

                        #endregion
                    }
                    else
                    {
                        divPopAlert.Visible = true;
                        Showgrid.Visible = false;
                        print.Visible = false;
                        lblAlertMsg.Text = "No Records Found";
                    }
                    #endregion
                }
                else
                {
                    divPopAlert.Visible = true;
                    Showgrid.Visible = false;
                    print.Visible = false;
                    lblAlertMsg.Text = "Please Update Attendnce Parameter";
                }
                #endregion
                #endregion
            }
        }
        catch
        {
        }
    }
    public void HeaderBind(int Index, DataSet dnew)
    {
        try
        {

            dichrdet.Clear();
            //data.Clear();
            if (Index == 0)
            {
                arrColHdrNames1.Add("S.No");
                arrColHdrNames1.Add("Date");
                data.Columns.Add("SNo", typeof(string));
                data.Columns.Add("Date", typeof(string));
                if (dnew.Tables.Count > 1 && dnew.Tables[1].Rows.Count > 0)
                {
                    int MaxHour = 0;
                    int.TryParse(Convert.ToString(dnew.Tables[1].Rows[0]["No_of_hrs_per_day"]), out MaxHour);
                    if (MaxHour != 0)
                    {
                        for (int intMax = 0; intMax < MaxHour; intMax++)
                        {
                            arrColHdrNames1.Add(Convert.ToString(intMax + 1) + " Hour");
                            data.Columns.Add(Convert.ToString(intMax + 1) + " Hour", typeof(string));
                            dichrdet.Add(intMax + 1, Convert.ToString(intMax + 1) + " Hour");


                        }
                    }
                }
                arrColHdrNames1.Add("Attendance Status");
                data.Columns.Add("Attendance Status", typeof(string));

            }
            else if (Index == 1)
            {
                arrColHdrNames1.Add("S.No");
                arrColHdrNames1.Add("Con Hour");
                arrColHdrNames1.Add("Attn Hour");
                arrColHdrNames1.Add("Hour %");
                arrColHdrNames1.Add("Con Days");
                arrColHdrNames1.Add("Attn Days");
                arrColHdrNames1.Add("Days %");

                data.Columns.Add("SNo", typeof(string));
                data.Columns.Add("Con Hour", typeof(string));
                data.Columns.Add("Attn Hour", typeof(string));
                data.Columns.Add("Hour %", typeof(string));
                data.Columns.Add("Con Days", typeof(string));
                data.Columns.Add("Attn Days", typeof(string));
                data.Columns.Add("Days %", typeof(string));

            }
            DataRow drHdr1 = data.NewRow();
            for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                drHdr1[grCol] = arrColHdrNames1[grCol];
            data.Rows.Add(drHdr1);
        }
        catch
        {

        }
    }

    protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    e.Row.Cells[grCol].Visible = false;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                for (int j = 0; j < data.Columns.Count; j++)
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }


        }
        catch
        {
        }
    }
    #endregion

    #region Reusable Method

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
            //lblErrSearch.Text = Convert.ToString(ex).Trim();
            //lblErrSearch.Visible = true;
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollegeOD.Items.Count > 0) ? Convert.ToString(ddlCollegeOD.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
        }
        return null;
    }

    private string orderByStudents(string collegeCode, string aliasName = null, string tableName = null)
    {
        string orderBySetting = dirAcc.selectScalarString("select value from master_Settings where settings='order_by' ");//and value<>''
        orderBySetting = orderBySetting.Trim();

        string serialNo = dirAcc.selectScalarString("select LinkValue from inssettings where college_code='" + collegeCode + "' and linkname='Student Attendance'");

        string aliasOrTableName = ((string.IsNullOrEmpty(aliasName) && string.IsNullOrEmpty(tableName)) ? "" : ((!string.IsNullOrEmpty(tableName)) ? tableName.Trim() + "." : ((!string.IsNullOrEmpty(aliasName)) ? aliasName.Trim() + "." : "")));

        string orderBy = "ORDER BY " + aliasOrTableName + "roll_no";
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
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
            return false;
        }
    }

    #endregion

    #region Print
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                da.printexcelreportgrid(Showgrid, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your  Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        { }
    }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string ss = null;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Student Attendance Report " + '@' + " Date   : " + txt_from.Text + " To " + txt_to.Text + "";
            pagename = "StudentsAttendancePrevousHistory.aspx";
            Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
            Printcontrol.Visible = true;
        }
        catch { }
    }

    public void btnPrint11()
    {
        DAccess2 ddd2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = ddd2.select_method_wo_parameter(colQ, "Text");
        string collegeName = string.Empty;
        string collegeCateg = string.Empty;
        string collegeAff = string.Empty;
        string collegeAdd = string.Empty;
        string collegePhone = string.Empty;
        string collegeFax = string.Empty;
        string collegeWeb = string.Empty;
        string collegeEmai = string.Empty;
        string collegePin = string.Empty;
        string acr = string.Empty;
        string City = string.Empty;
        if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
        {
            collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
            City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
            collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
            collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
            collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
            collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
            collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
            collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
        }
        DateTime dt = DateTime.Now;
        int year = dt.Year;
        spCollegeName.InnerHtml = collegeName;
        spAddr.InnerHtml = collegeAdd;
        spDegreeName.InnerHtml = acr;
        spReportName.InnerHtml = "Student's Previous Attendance Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }
    #endregion


}
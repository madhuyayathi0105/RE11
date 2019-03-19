using System;
using System.Collections.Generic;
using System.Data;
using InsproDataAccess;
using System.Text;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI;


public partial class AdmissionMod_StudentTransferSteamWise : System.Web.UI.Page
{
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string UserCode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        UserCode = Session["usercode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (!IsPostBack)
        {
            bindCollege();
            bindBatch();
            bindEdulevel();
            bindCourse();
            bindBranch();

            txt_date.Attributes.Add("readonly", "readonly");
            txt_date.Text = System.DateTime.Now.ToString("dd/MM/yyyy");

            rdbtype_SelectedIndexChanged(sender, e);
        }
    }
    public void bindCollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            ds.Clear();
            ds = d2.BindCollegebaseonrights(UserCode, 1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
        }
        catch
        {

        }
    }
    public void bindBatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
        }
        catch
        {

        }
    }
    public void bindEdulevel()
    {
        try
        {
            ddlEduLev.Items.Clear();
            ds.Clear();
            ds = d2.select_method_wo_parameter("select distinct Edu_level from Course where college_code=" + ddlCollege.SelectedValue + " order by Edu_level desc", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlEduLev.DataSource = ds;
                ddlEduLev.DataTextField = "Edu_level";
                ddlEduLev.DataValueField = "Edu_level";
                ddlEduLev.DataBind();
            }
        }
        catch
        {

        }
    }
    public void bindCourse()
    {
        try
        {
            ddlcourse.Items.Clear();
            ds.Clear();
            ds = d2.select_method_wo_parameter("select distinct course_id,Course_Name from Course where college_code=" + ddlCollege.SelectedValue + " and Edu_Level='" + ddlEduLev.SelectedValue + "' order by course_id", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcourse.DataSource = ds;
                ddlcourse.DataTextField = "Course_Name";
                ddlcourse.DataValueField = "course_id";
                ddlcourse.DataBind();
            }
        }
        catch
        {

        }
    }
    public void bindBranch()
    {
        try
        {
            string streamValue = streamVal();

            ddlBranch.Items.Clear();
            DataSet dsBran = d2.select_method_wo_parameter("select d.Degree_Code,(c.course_Name+' '+ dt.dept_name) as dept_name from Degree d, Department dt,course c where dt.Dept_Code=d.Dept_Code and c.Course_Id=d.Course_Id and d.college_code='" + ddlCollege.SelectedValue + "' and c.Edu_Level='" + ddlEduLev.SelectedValue + "' and d.degree_code in (select Degree_Code Remaining from seattype_cat where collegeCode='" + ddlCollege.SelectedValue + "' and Batch_Year='" + ddlbatch.SelectedValue + "' and Quota='" + ddlCategory.SelectedValue + "' and Category_Code='" + streamValue + "' and (ISNULL(Tot_Seat,0)-allotedSeats)>0) order by Dept_Name asc ", "Text");
            //and d.Course_Id='" + ddlcourse.SelectedValue + "'
            if (dsBran.Tables.Count > 0 && dsBran.Tables[0].Rows.Count > 0)
            {
                ddlBranch.DataSource = dsBran;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "Degree_Code";
                ddlBranch.DataBind();
            }
        }
        catch
        {

        }
    }
    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindBatch();
        bindEdulevel();
        bindCourse();
        bindBranch();

    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEdulevel();
        bindCourse();
        bindBranch();

    }
    protected void ddlEdulevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindCourse();
        bindBranch();

    }
    protected void ddlcourse_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindBranch();

    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindBranch();
    }
    private void bindCategory(string critCode)
    {
        try
        {
            ddlCategory.Items.Clear();
            DataSet dsStudRankCrit = d2.select_method_wo_parameter("select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='StudRankCriteria' and collegeCode ='" + ddlCollege.SelectedValue + "' and MasterCode in (" + critCode + ")", "Text");
            if (dsStudRankCrit.Tables.Count > 0 && dsStudRankCrit.Tables[0].Rows.Count > 0)
            {
                ddlCategory.DataSource = dsStudRankCrit;
                ddlCategory.DataTextField = "MasterValue";
                ddlCategory.DataValueField = "MasterCode";
                ddlCategory.DataBind();
            }
        }
        catch { }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText, string contextKey)
    {
        string Flitervalues = contextKey;
        string[] Flitervalue = Flitervalues.Split('$');
        string collegecode = Convert.ToString(Flitervalue[0]);
        string batchyear = Convert.ToString(Flitervalue[1]);
        string edulevel = Convert.ToString(Flitervalue[2]);
        string courseid = Convert.ToString(Flitervalue[3]);
        string tapselect = Convert.ToString(Flitervalue[4]);
        WebService ws = new WebService();
        List<string> name = new List<string>();
        if (prefixText.Trim() != "")
        {
            string regquery = "";
            if (tapselect == "0")
                regquery = " and IsConfirm='1' and ISNULL(selection_status,0)=0 and ISNULL(admission_status,0)=0";
            else if (tapselect == "1")
                regquery = " and IsConfirm='1' and ISNULL(selection_status,0)=1 and ISNULL(admission_status,0)=0";

            string query = "  select app_formno from applyn a,course c,Degree d,Department dt where a.college_code=d.college_code and c.Course_Id=d.Course_Id and dt.Dept_Code=d.Dept_Code and a.degree_code=d.Degree_Code and  a.college_code='" + collegecode + "' and batch_year='" + batchyear + "' and c.Edu_Level='" + edulevel + "' and c.Course_Id='" + courseid + "' " + regquery + " and app_formno like '" + prefixText + "%' ";
            name = ws.Getname(query);
        }
        return name;
    }
    public void rdbtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbtype.SelectedItem.Value == "0")
        {
            btn_register.Text = "Register";

            verification_div.Visible = false;
        }
        if (rdbtype.SelectedItem.Value == "1")
        {
            btn_register.Text = "Submit";

            verification_div.Visible = false;
        }
    }
    protected void btn_register_click(object sender, EventArgs e)
    {
        try
        {
            clearDetails(false);
            ddlBranch.Items.Clear();
            ddlCategory.Items.Clear();
            verification_div.Visible = false;
            rankdet_span.InnerHtml = string.Empty;
            rankdet_spanRes.InnerHtml = string.Empty;
            string streamValue = streamVal();
            BindStream();
            bindCategory();
            bindBranchNew();
            if (rdbtype.SelectedItem.Value == "0")
            {
                verification_div.Visible = false;
                if (txt_applicationno.Text.Trim() != "")
                {
                    int selection = d2.update_method_wo_parameter(" update applyn set selection_status='1' where app_formno='" + txt_applicationno.Text + "'", "Text");
                    if (selection != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Student Registered Successfully')", true);
                        txt_applicationno.Text = "";
                    }
                }
            }
            else if (rdbtype.SelectedItem.Value == "1")
            {
                string certificatequery = "  SELECT (select MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CertName=MasterCode)CertificateName,CertName FROM CertMasterDet where CourseID='" + ddlcourse.SelectedItem.Value + "'";
                certificatequery += " select a.quota,a.app_no,app_formno,stud_name,CONVERT(varchar(10),dob,103)dob,case when sex='0' then 'Male' when sex='1' then 'Female' when sex='2' then 'Transgender' end sex,Student_Mobile,parent_name,cast(sm.jeeMarkSec as decimal(10,2))jeeMarkSec,sm.jeeStateRank,(select textval from textvaltable where TextCriteria='unive' and TextCode=sm.board)board,cast(sm.HSCMarkSec as decimal(10,2))HSCMarkSec,convert(varchar,sm.yearPassing)+'-'+convert(varchar, DateName( month , DateAdd( month , sm.monthpassing , 0 ) - 1 )) as yearofpassing  from applyn a left join ST_Student_Mark_Detail sm on a.app_no=sm.ST_AppNo  where  app_formno='" + txt_applicationno.Text + "' and a.college_code='" + ddlCollege.SelectedValue + "' and isnull(isconfirm,'0')='1' and isnull(selection_status,'0')='1' and isnull(admission_Status,'0')='1'";

                certificatequery += "select a.app_no,ST_Rank,ST_RankCriteria,(select MasterValue from CO_MasterValues where MasterCriteria='StudRankCriteria' and MasterCode = ST_RankCriteria ) as Criteria from applyn a,ST_RankTable r where a.app_no=r.ST_AppNo and a.app_formno='" + txt_applicationno.Text + "' and a.college_code='" + ddlCollege.SelectedValue + "' and r.ST_Stream ='" + streamValue + "'";

                ds = d2.select_method_wo_parameter(certificatequery, "text");
                if (ds.Tables != null)
                {
                    #region student details
                    if (ds.Tables.Count > 2 && ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            applicantno_span.InnerHtml = ": " + Convert.ToString(dr["app_formno"]);
                            applicantname_span.InnerHtml = ": " + Convert.ToString(dr["stud_name"]);
                            dob_span.InnerHtml = ": " + Convert.ToString(dr["dob"]);
                            gender_span.InnerHtml = ": " + Convert.ToString(dr["sex"]);
                            studmobileno_span.InnerHtml = ": " + Convert.ToString(dr["Student_Mobile"]);
                            fathername_span.InnerHtml = ": " + Convert.ToString(dr["parent_name"]);
                            personaldet_div.Visible = true;

                            jeemark_span.InnerHtml = ": " + Convert.ToString(dr["jeeMarkSec"]);
                            jeestaterank.InnerHtml = ": " + Convert.ToString(dr["jeeStateRank"]);
                            board.InnerHtml = ": " + Convert.ToString(dr["board"]);
                            hscmark_span.InnerHtml = ": " + Convert.ToString(dr["HSCMarkSec"]);
                            yearofpassing_span.InnerHtml = ": " + Convert.ToString(dr["yearofpassing"]);
                        }

                        if (ds.Tables[2].Rows.Count > 0)
                        {

                            rankdet_div.Visible = true;
                            verification_div.Visible = true;
                            DataTable dtRegDet = dirAcc.selectDataTable("select a.quota,r.degree_code,(c.course_Name +' '+dt.dept_name) as dept_name,StreamAdmission from registration r,applyn a,degree d,department dt,Course c where a.app_no=r.app_no and d.degree_code=r.degree_code and dt.dept_code=d.dept_code and c.course_id=d.course_id and a.app_formno='" + txt_applicationno.Text.Trim() + "' and a.college_code='" + ddlCollege.SelectedValue + "'");
                            if (dtRegDet.Rows.Count > 0)
                            {
                                for (int crI = 0; crI < ds.Tables[2].Rows.Count; crI++)
                                {
                                    string criteria = Convert.ToString(ds.Tables[2].Rows[crI]["Criteria"]);
                                    string criteriaVal = Convert.ToString(ds.Tables[2].Rows[crI]["ST_RankCriteria"]);

                                    rankdet_span.InnerHtml += criteria + "<br>";
                                    rankdet_spanRes.InnerHtml += ": " + Convert.ToString(ds.Tables[2].Rows[crI]["ST_Rank"]) + "<br>";
                                }

                                string critCode = Convert.ToString(dtRegDet.Rows[0]["quota"]);
                                string StreamCode = Convert.ToString(dtRegDet.Rows[0]["StreamAdmission"]);
                                bindCategory(critCode);
                                BindStreamNew(StreamCode);
                                ListItem lst = new ListItem(Convert.ToString(dtRegDet.Rows[0]["dept_name"]), Convert.ToString(dtRegDet.Rows[0]["degree_code"]));
                                ddlBranch.Items.Add(lst);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Student not enrolled')", true);
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Student not enrolled')", true);
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "StudentCourseSelection");
        }
    }
    protected void clear()
    {
        applicantno_span.InnerHtml = ": ";
        applicantname_span.InnerHtml = ": ";
        dob_span.InnerHtml = ": ";
        gender_span.InnerHtml = ": ";
        studmobileno_span.InnerHtml = ": ";
        fathername_span.InnerHtml = ": ";

        jeemark_span.InnerHtml = ": ";
        jeestaterank.InnerHtml = ": ";
        board.InnerHtml = ": ";
        hscmark_span.InnerHtml = ": ";
        yearofpassing_span.InnerHtml = ": ";
    }
    //Added by Idhris 24-05-2017
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string appFormNo = txt_applicationno.Text.Trim();
            string collegeCode = ddlCollege.Items.Count > 0 ? ddlCollege.SelectedValue : string.Empty;
            string appNo = getAppNoAndValidateInputs(appFormNo, collegeCode);
            string streamValue = streamVal();

            if (!string.IsNullOrEmpty(appNo) && streamValue != string.Empty && ddlCategory.Items.Count > 0 && ddlBranch.Items.Count > 0)
            {
                divPrintReject.Visible = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please check inputs')", true);
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please try later')", true);
        }
    }
    private string getAppNoAndValidateInputs(string appFormNo, string collegeCode)
    {
        string appNo = string.Empty;
        try
        {
            if (!string.IsNullOrEmpty(appFormNo) && !string.IsNullOrEmpty(collegeCode))
            {
                appNo = dirAcc.selectScalarString("select a.app_No from applyn a,registration r where a.app_no=r.app_no and  app_formno='" + appFormNo + "' and a.college_code='" + collegeCode + "' and isnull(isconfirm,'0')='1' and isnull(selection_status,'0')='1' and isnull(admission_Status,'0')='1'");
            }
        }
        catch { appNo = string.Empty; }
        return appNo;
    }
    //Clear details after saving
    private void clearDetails(bool clearText)
    {
        verification_div.Visible = false;
        //Personal Details
        applicantno_span.InnerHtml = string.Empty;
        applicantname_span.InnerHtml = string.Empty;
        dob_span.InnerHtml = string.Empty;
        gender_span.InnerHtml = string.Empty;
        studmobileno_span.InnerHtml = string.Empty;
        fathername_span.InnerHtml = string.Empty;

        //Academic Details
        jeemark_span.InnerHtml = string.Empty;
        hscmark_span.InnerHtml = string.Empty;
        board.InnerHtml = string.Empty;
        yearofpassing_span.InnerHtml = string.Empty;

        //Rank Details
        rankdet_span.InnerHtml = string.Empty;
        rankdet_spanRes.InnerHtml = string.Empty;

        //Branch and Category
        ddlCategory.Items.Clear();
        ddlBranch.Items.Clear();

        if (clearText)
        {
            //Finalize
            txt_applicationno.Text = string.Empty;
            txt_applicationno.Focus();
        }
    }
    //Get Stream value saved
    private string streamVal()
    {
        string streamValue = string.Empty;
        try
        {
            string[] resVal = dirAcc.selectScalarString("SELECT LinkValue FROM New_InsSettings WHERE LinkName='ADMISSIONCOURSESELECTIONSETTINGS' AND college_code='" + ddlCollege.SelectedValue + "'").Split('$');

            if (resVal.Length == 6)
            {
                string collegeCode = resVal[0];
                string batchYear = resVal[1];
                string eduLevel = resVal[2];
                string courseCode = resVal[3];
                streamValue = resVal[4];
                string criteriaCode = resVal[5];
            }
        }
        catch { streamValue = string.Empty; }
        return streamValue;
    }
    //Rejection COnfirmation
    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {
            divPrintReject.Visible = false;
            string appFormNo = txt_applicationno.Text.Trim();
            string collegeCode = ddlCollege.Items.Count > 0 ? ddlCollege.SelectedValue : string.Empty;
            string appNo = getAppNoAndValidateInputs(appFormNo, collegeCode);
            string streamValue = ddlfirstStream.SelectedValue;
            if (!string.IsNullOrEmpty(appNo) && streamValue != string.Empty)
            {
                if (isSeatAvailable(collegeCode, ddlbatch.SelectedItem.Text, ddlStream.SelectedValue, ddlcate.SelectedValue, ddlTansferCourse.SelectedValue))
                {
                    dirAcc.deleteData("update seattype_cat set allotedSeats=(allotedSeats-1)  where collegeCode='" + ddlCollege.SelectedValue + "' and Batch_Year='" + ddlbatch.SelectedValue + "' and Degree_Code='" + ddlBranch.SelectedValue + "' and Quota='" + ddlCategory.SelectedValue + "' and Category_Code='" + streamValue + "'");

                    dirAcc.deleteData("update seattype_cat set allotedSeats=(allotedSeats+1)  where collegeCode='" + ddlCollege.SelectedValue + "' and Batch_Year='" + ddlbatch.SelectedValue + "' and Degree_Code='" + ddlTansferCourse.SelectedValue + "' and Quota='" + ddlcate.SelectedValue + "' and Category_Code='" + ddlStream.SelectedValue + "'");

                    string Query = "update Registration set degree_code ='" + ddlTansferCourse.SelectedValue + "',Adm_Date ='" + DateTime.Now.ToString("MM/dd/yyyy") + "' where app_no='" + appNo + "'  and college_code='" + collegeCode + "'";
                    string InstQuery = " insert into ST_Student_Transfer (AppNo,TransferDate,TransferTime,FromDegree,Todegree,FromCollege,Tocollege,FromSeatType,ToSeatType)  values ('" + appNo + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now + "','" + ddlBranch.SelectedValue + "','" + ddlTansferCourse.SelectedValue + "','" + ddlCollege.SelectedValue + "','" + ddlCollege.SelectedValue + "','" + streamValue + "','" + ddlStream.SelectedValue + "')";
                    d2.select_method_wo_parameter(InstQuery, "Text");

                    Query += "  update applyn set is_enroll='2',quota='" + ddlcate.SelectedValue + "',degree_code='" + ddlTansferCourse.SelectedValue + "',StreamAdmission='" + ddlStream.SelectedValue + "'  where app_no='" + appNo + "'  and college_code='" + collegeCode + "'";
                    d2.select_method_wo_parameter(Query, "Text");

                    clearDetails(false);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Transfered successfully')", true);

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please check inputs')", true);
                }
                // dirAcc.deleteData("update applyn set is_enroll='0',seattype='0',quota='0',degree_code=null where app_no='" + appNo + "'");


            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please check inputs')", true);
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please try later')", true);
        }
    }
    protected void btnRejectClose_Click(object sender, EventArgs e)
    {
        divPrintReject.Visible = false;
    }
    protected void btn_clear_click(object sender, EventArgs e)
    {
        clearDetails(true);
    }

    private void BindStream()
    {
        try
        {
            string qry = string.Empty;
            qry = "select TextCode,TextVal from TextValTable tv where TextCriteria='ADMst' and college_code ='" + ddlCollege.SelectedValue + "' order by TextVal";
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

        }
    }
    private void bindCategory()
    {
        try
        {
            ddlCategory.Items.Clear();
            DataSet dsStudRankCrit = d2.select_method_wo_parameter("select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='StudRankCriteria' and collegeCode ='" + ddlCollege.SelectedValue + "'", "Text");//and MasterCode in (" + critCode + ")
            if (dsStudRankCrit.Tables.Count > 0 && dsStudRankCrit.Tables[0].Rows.Count > 0)
            {
                ddlcate.DataSource = dsStudRankCrit;
                ddlcate.DataTextField = "MasterValue";
                ddlcate.DataValueField = "MasterCode";
                ddlcate.DataBind();
            }
        }
        catch { }
    }
    public void bindBranchNew()
    {
        try
        {
            string streamValue = ddlStream.SelectedValue;

            ddlTansferCourse.Items.Clear();
            DataSet dsBran = d2.select_method_wo_parameter("select d.Degree_Code,(c.course_Name+' '+ dt.dept_name) as dept_name from Degree d, Department dt,course c where dt.Dept_Code=d.Dept_Code and c.Course_Id=d.Course_Id and d.college_code='" + ddlCollege.SelectedValue + "' and c.Edu_Level='" + ddlEduLev.SelectedValue + "'  and d.degree_code in (select Degree_Code Remaining from seattype_cat where collegeCode='" + ddlCollege.SelectedValue + "' and Batch_Year='" + ddlbatch.SelectedValue + "' and Quota='" + ddlcate.SelectedValue + "' and Category_Code='" + streamValue + "' and (ISNULL(Tot_Seat,0)-allotedSeats)>0) order by Dept_Name asc ", "Text");
            //and d.Course_Id='" + ddlcourse.SelectedValue + "'
            if (dsBran.Tables.Count > 0 && dsBran.Tables[0].Rows.Count > 0)
            {
                ddlTansferCourse.DataSource = dsBran;
                ddlTansferCourse.DataTextField = "dept_name";
                ddlTansferCourse.DataValueField = "Degree_Code";
                ddlTansferCourse.DataBind();
            }
        }
        catch
        {

        }
    }
    private void BindStreamNew(string StreamValue)
    {
        try
        {
            string qry = string.Empty;
            qry = "select TextCode,TextVal from TextValTable tv where TextCriteria='ADMst' and college_code ='" + ddlCollege.SelectedValue + "' and TextCode ='" + StreamValue + "' order by TextVal";
            ds = d2.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlfirstStream.DataSource = ds;
                ddlfirstStream.DataTextField = "TextVal";
                ddlfirstStream.DataValueField = "TextCode";
                ddlfirstStream.DataBind();
                ddlfirstStream.Enabled = true;
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void ddlStream_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindBranchNew();
        }
        catch
        {

        }
    }
    protected void ddlcate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindBranchNew();
        }
        catch
        {

        }
    }
    private bool isSeatAvailable(string collegeCode, string batchYr, string streamValue, string categCode, string degCode)
    {
        bool available = false;
        int seatRemains = dirAcc.selectScalarInt("select ISNULL(Tot_seat,0)-ISNULL(allotedSeats,0) as Remaining from seattype_cat where collegeCode='" + collegeCode + "' and Category_Code='" + streamValue + "' and Degree_Code='" + degCode + "' and Batch_Year='" + batchYr + "' and Quota='" + categCode + "'");
        if (seatRemains > 0)
            available = true;
        return available;
    }

}
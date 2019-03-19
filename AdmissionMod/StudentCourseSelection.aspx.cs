using System;
using System.Collections.Generic;
using System.Data;
using InsproDataAccess;
using System.Text;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;

public partial class AdmissionMod_StudentCourseSelection : System.Web.UI.Page
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
            getRegistrarSign();
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
            string courseID = streamValNew();
            ddlBranch.Items.Clear();
            DataSet dsBran = d2.select_method_wo_parameter("select d.Degree_Code,(c.course_Name+' '+ dt.dept_name) as dept_name from Degree d, Department dt,course c where dt.Dept_Code=d.Dept_Code and c.Course_Id=d.Course_Id and d.college_code='" + ddlCollege.SelectedValue + "' and c.Edu_Level='" + ddlEduLev.SelectedValue + "' and d.degree_code in (select Degree_Code Remaining from seattype_cat where collegeCode='" + ddlCollege.SelectedValue + "' and Batch_Year='" + ddlbatch.SelectedValue + "' and Quota='" + ddlCategory.SelectedValue + "' and Category_Code='" + streamValue + "' and d.Course_Id in (" + courseID + ") and (ISNULL(Tot_Seat,0)-allotedSeats)>0) order by Dept_Name asc ", "Text");

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
        getRegistrarSign();
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
            DataSet dsStudRankCrit = d2.select_method_wo_parameter("select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='StudRankCriteria' and collegeCode ='" + ddlCollege.SelectedValue + "'", "Text");//and MasterCode in (" + critCode + ")
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
            contentDiv.InnerHtml = string.Empty;
            if (txt_applicationno.Text.Trim() != string.Empty)
            {
                if (printAdmitCardPrevious(txt_applicationno.Text.Trim()))
                {
                    return;
                }
            }
            ddlCategory.Items.Clear();
            verification_div.Visible = false;
            rankdet_span.InnerHtml = string.Empty;
            string streamValue = streamVal();
            //rankdet_spanRes.InnerHtml = string.Empty;
            if (rdbtype.SelectedItem.Value == "0")
            {
                verification_div.Visible = false;
                if (txt_applicationno.Text.Trim() != "")
                {
                    int selection = d2.update_method_wo_parameter(" update applyn set selection_status='1' where app_formno='" + txt_applicationno.Text.Trim() + "'", "Text");
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
                certificatequery += " select a.app_no,app_formno,stud_name,CONVERT(varchar(10),dob,103)dob,case when sex='0' then 'Male' when sex='1' then 'Female' when sex='2' then 'Transgender' end sex,Student_Mobile,parent_name,cast(sm.jeeMarkSec as decimal(10,2))jeeMarkSec,sm.jeeStateRank,(select textval from textvaltable where TextCriteria='unive' and TextCode=sm.board)board,cast(sm.HSCMarkSec as decimal(10,2))HSCMarkSec,convert(varchar,sm.yearPassing)+'-'+convert(varchar, DateName( month , DateAdd( month , sm.monthpassing , 0 ) - 1 )) as yearofpassing  from applyn a left join ST_Student_Mark_Detail sm on a.app_no=sm.ST_AppNo  where  app_formno='" + txt_applicationno.Text.Trim() + "' and a.college_code='" + ddlCollege.SelectedValue + "' and isnull(isconfirm,'0')='1' and isnull(selection_status,'0')='1' and isnull(admission_Status,'0')='1'";

                certificatequery += "select a.app_no,ST_Rank,ST_RankCriteria,(select MasterValue from CO_MasterValues where MasterCriteria='StudRankCriteria' and MasterCode = ST_RankCriteria ) as Criteria from applyn a,ST_RankTable r where a.app_no=r.ST_AppNo and a.app_formno='" + txt_applicationno.Text.Trim() + "' and a.college_code='" + ddlCollege.SelectedValue + "' and ST_stream='" + streamValue + "'";

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
                            StringBuilder critCode = new StringBuilder();
                            StringBuilder resRank = new StringBuilder();
                            resRank.Append("<table rules='all' style='border:1px solid black; width:300px;text-align:center;'>");
                            for (int crI = 0; crI < ds.Tables[2].Rows.Count; crI++)
                            {
                                string criteria = Convert.ToString(ds.Tables[2].Rows[crI]["Criteria"]);
                                string criteriaVal = Convert.ToString(ds.Tables[2].Rows[crI]["ST_RankCriteria"]);

                                resRank.Append("<tr><td>" + criteria + "</td><td>" + " " + Convert.ToString(ds.Tables[2].Rows[crI]["ST_Rank"]) + "</td></tr>");
                                //rankdet_span.InnerHtml += criteria + "<br><hr>";
                                //rankdet_spanRes.InnerHtml += ": " + Convert.ToString(ds.Tables[2].Rows[crI]["ST_Rank"]) + "<br><hr>";
                                critCode.Append(criteriaVal + ",");
                            }
                            resRank.Append("</table>");
                            rankdet_span.InnerHtml = resRank.ToString();
                            verification_div.Visible = true;
                            bindCategory(critCode.ToString().TrimEnd(','));
                            bindBranch();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Student not registered')", true);
                    }
                    #endregion
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Student not registered')", true);
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
            contentDiv.InnerHtml = string.Empty;
            string categCode = ddlCategory.Items.Count > 0 ? ddlCategory.SelectedValue : string.Empty;
            string degCode = ddlBranch.Items.Count > 0 ? ddlBranch.SelectedValue : string.Empty;
            string appFormNo = txt_applicationno.Text.Trim();
            string collegeCode = ddlCollege.Items.Count > 0 ? ddlCollege.SelectedValue : string.Empty;
            string appNo = getAppNoAndValidateInputs(categCode, degCode, appFormNo, collegeCode);

            string streamValue = streamVal();

            if (!string.IsNullOrEmpty(appNo))
            {
                //&& streamValue != string.Empty
                //**********************Main*****************************************
                //DataTable dtStudRank = dirAcc.selectDataTable("select a.app_no,ST_Rank,ST_RankCriteria,(select MasterValue from CO_MasterValues where MasterCriteria='StudRankCriteria' and MasterCode = ST_RankCriteria ) as Criteria from applyn a,ST_RankTable r where a.app_no=r.ST_AppNo and a.app_no='" + appNo + "' and r.ST_Stream='" + streamValue + "'");
                //if (dtStudRank.Rows.Count > 0)
                //{
                //dtStudRank.DefaultView.RowFilter = "ST_RankCriteria='" + categCode + "'";
                //DataView dvCatCheck = dtStudRank.DefaultView;
                //if (dvCatCheck.Count > 0)
                //{
                //**********************Main*****************************************
                string retMessage = string.Empty;
                bool checkCategAvailability = true;
                //**********************Main*****************************************
                //categoryPriorityCheck(collegeCode, ddlbatch.SelectedValue, streamValue, categCode, degCode, out retMessage);
                //**********************Main*****************************************
                if (checkCategAvailability)
                {
                    #region Admission Number Generation While Save and insert into Registration
                    //bool isAdmNoGenSettingsOn = false;
                    string admNoGenbatch = ddlbatch.SelectedItem.Text;
                    // isAdmNoGenSettingsOn = getAdmGenOnRcpt(collegeCode, UserCode, ref  admNoGenbatch);

                    //If Admission generation On
                    //if (isAdmNoGenSettingsOn)
                    //{
                    string app_formNo = string.Empty;
                    if (isAdmNoNotGenerated(appNo, admNoGenbatch, collegeCode, ref app_formNo))
                    {
                        string queryRollApp = "select stud_name,app_formno as Roll_No,app_formno,a.app_no,app_formno as Reg_No, Current_Semester,'' sections ,mode,a.batch_year,seattype,Edu_Level  from applyn a, course c   where  a.courseID=c.course_id  and  a.app_no='" + appNo + "'  and a.college_code='" + collegeCode + "' ";
                        DataSet dsRollApp = new DataSet();
                        dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                        string StudMode = string.Empty;
                        string studname = string.Empty;
                        string eduleve = string.Empty;
                        string Mode = string.Empty;
                        string seattype = string.Empty;
                        string batchYr = string.Empty;
                        string cursem = string.Empty;

                        if (dsRollApp.Tables.Count > 0 && dsRollApp.Tables[0].Rows.Count > 0)
                        {
                            studname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["stud_name"]);
                            StudMode = Convert.ToString(dsRollApp.Tables[0].Rows[0]["mode"]);

                            eduleve = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Edu_Level"]);
                            Mode = Convert.ToString(dsRollApp.Tables[0].Rows[0]["mode"]);
                            seattype = Convert.ToString(dsRollApp.Tables[0].Rows[0]["seattype"]);
                            batchYr = Convert.ToString(dsRollApp.Tables[0].Rows[0]["batch_year"]);
                            cursem = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Current_Semester"]);
                            if (seattype.Trim() == "" || seattype.Trim() == "0")
                            {
                                seattype = dirAcc.selectScalarString("select TextCode from TextValtable where TextCriteria='seat' and college_code=" + collegeCode + " and TextVal like 'M%'");
                            }
                        }

                        int format = 0;
                        string generatedAdmNo = string.Empty;
                        bool UpdateFlag = false;
                        if (admissionNoGeneration())
                        {
                            generatedAdmNo = generateApplNo(collegeCode, Convert.ToInt32(degCode), eduleve, Mode, seattype, batchYr, out format);
                            UpdateFlag = true;
                        }
                        if (generatedAdmNo == string.Empty)
                        {
                            generatedAdmNo = app_formNo;
                        }


                        if (isSeatAvailable(collegeCode, batchYr, streamValue, categCode, degCode))
                        {
                            d2.select_method_wo_parameter("if exists(select App_No from Registration where App_No='" + appNo + "' and college_code='" + collegeCode + "') update registration set roll_admit='" + generatedAdmNo + "' where app_no='" + appNo + "'  and college_code='" + collegeCode + "' else insert into Registration (App_No,Adm_Date,Roll_Admit,Roll_No,RollNo_Flag,Reg_No,Stud_Name,Batch_Year,degree_code,college_code,CC,DelFlag,Exam_Flag,Current_Semester,mode)values('" + appNo + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + generatedAdmNo + "','" + app_formNo + "','1','" + app_formNo + "','" + studname + "','" + admNoGenbatch + "','" + degCode + "','" + collegeCode + "','0','0','OK','1','" + StudMode + "')  ", "Text");
                            d2.select_method_wo_parameter("update applyn set is_enroll='2',seattype='" + seattype + "',quota='" + categCode + "',degree_code='" + degCode + "',StreamAdmission='" + streamValue + "'  where app_no='" + appNo + "'  and college_code='" + collegeCode + "'", "Text");
                            if (UpdateFlag == true)
                            {
                                UpdateApplNo(collegeCode, Convert.ToInt32(degCode), eduleve, Mode, seattype, batchYr, format);
                            }
                            //Allot number for category save
                            dirAcc.updateData("update seattype_cat set allotedSeats=(allotedSeats+1)  where collegeCode='" + ddlCollege.SelectedValue + "' and Batch_Year='" + ddlbatch.SelectedValue + "' and Degree_Code='" + degCode + "' and Quota='" + ddlCategory.SelectedValue + "' and Category_Code='" + streamValue + "'");

                            //Fee allot on Save
                            FeeAllotOnSave(collegeCode, degCode, appNo, batchYr, seattype, cursem);



                            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);

                            string selCertQ = "select M.MasterValue from StudCertDetails_New c,CO_MasterValues m where c.CertificateId=m.MasterCode and ISNULL(c.isOrginal,'0')='1' and c.App_no='" + appNo + "' and CertificateId <>'946'";
                            DataTable dtCert = dirAcc.selectDataTable(selCertQ);
                            string certDet = string.Empty;
                            if (dtCert.Rows.Count > 0)
                            {
                                certDet = "The following original documents have been received: <br /><br />";
                                foreach (DataRow drCert in dtCert.Rows)
                                {
                                    certDet += Convert.ToString(drCert["MasterValue"]) + ", ";
                                }
                                certDet = certDet.Trim().TrimEnd(',');
                                certDet += "<br /><br />";
                            }

                            int curBatch = Convert.ToInt32(ddlbatch.SelectedValue);
                            string acadYear = curBatch + "-" + (curBatch + 1);
                            //<tr><td  style='width:170px;'>Campus</td><td>: Kumbakonam</td></tr>
                            StringBuilder sbAdmLetter = new StringBuilder();
                            sbAdmLetter.Append("<div style='height: 850px; width: 730px; font-family: Times New Roman; font-size: 16px; padding: 10px; padding-left:50px;'> <center><div style='height: 220px;'></div><div style='width: 730px; text-align: center; text-decoration: underline; padding-bottom:25px;'>ADMISSION LETTER</div><div style='width: 730px; padding-bottom:25px;'><span  style='padding-left: 530px;'>" + DateTime.Now.ToString("dd-MM-yyyy") + "</span></div><table style='width: 730px; text-align: left;' cellpadding=5><tr><td style='width:170px;'>Name</td><td>: " + studname.ToUpper() + "</td></tr><tr><td  style='width:170px;'>Application No</td><td>: " + generatedAdmNo + "</td></tr><tr><td  style='width:170px;'>Programme Admitted to</td><td>: " + ddlBranch.SelectedItem.Text + "</td></tr><tr><td  style='width:170px;'>Academic Year</td><td>: 2017-18</td></tr><tr><td  style='width:170px;'>Campus</td><td>: Kumbakonam</td></tr></table><div style='width:730px; padding-top:25px; text-align:left;'><p>Dear <b>" + studname.ToUpper() + "</b><br /><br />Congratulations!<br /><br />Welcome to the SASTRA student fraternity.<br /><br />" + certDet + "Classes for First year will commence on <b>" + getCommenceDateTime(collegeCode) + ".</b><br /><br />Wishing you a fruitful stay at SASTRA UNIVERSITY.<br /><br />Yours faithfully,      <br /><br /><img src='../image/registrarsign" + collegeCode + ".jpeg' style='height:40px; width:120px;'/><br /><br /><b>REGISTRAR</b></p></div></center></div>");
                            contentDiv.InnerHtml = sbAdmLetter.ToString();
                            divPrintAdmLetter.Visible = true;

                            clearDetails(true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Seat not available for this Branch and Category')", true);
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Student already enrolled')", true);
                    }
                    //}
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Admission restricted')", true);
                    //}
                    #endregion
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + retMessage + "')", true);
                }
                //**********************Main*****************************************
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please check category')", true);
                //}
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please check student rank')", true);
                //}
                //**********************Main*****************************************
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
    private string getAppNoAndValidateInputs(string categCode, string degCode, string appFormNo, string collegeCode)
    {
        string appNo = string.Empty;
        try
        {
            if (!string.IsNullOrEmpty(categCode) && !string.IsNullOrEmpty(degCode) && !string.IsNullOrEmpty(appFormNo) && !string.IsNullOrEmpty(collegeCode))
            {
                appNo = dirAcc.selectScalarString("select app_No from applyn where app_formno='" + appFormNo + "' and college_code='" + collegeCode + "' and isnull(isconfirm,'0')='1' and isnull(selection_status,'0')='1' and isnull(admission_Status,'0')='1'");
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
        //rankdet_spanRes.InnerHtml = string.Empty;

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
    //Priority Check for Category
    private bool categoryPriorityCheck(string collegeCode, string batchYr, string streamValue, string categCode, string degCode, out string retMessage)
    {
        retMessage = string.Empty;
        bool catOk = false;
        try
        {
            string selQ = "select MasterCode,MasterValue,case when Masterpriority=0 then 1000 else Masterpriority end as Priority from CO_MasterValues where MasterCriteria='StudRankCriteria' and CollegeCode='" + collegeCode + "' order by Priority";
            DataTable dtPriority = dirAcc.selectDataTable(selQ);
            if (dtPriority.Rows.Count > 0)
            {
                foreach (DataRow drPriority in dtPriority.Rows)
                {
                    string curCategCode = Convert.ToString(drPriority["MasterCode"]).Trim();
                    if (curCategCode == categCode)
                    {
                        catOk = true;
                        break;
                    }
                    else
                    {
                        bool isAvailable = isSeatAvailable(collegeCode, batchYr, streamValue, curCategCode, degCode);
                        if (isAvailable)
                        {
                            retMessage = "Please allot on " + Convert.ToString(drPriority["MasterValue"]).Trim() + " category";
                            break;
                        }
                    }
                }
            }
        }
        catch { catOk = false; }
        return catOk;
    }
    //Check for Current Degree seat availability
    private bool isSeatAvailable(string collegeCode, string batchYr, string streamValue, string categCode, string degCode)
    {
        bool available = false;
        int seatRemains = dirAcc.selectScalarInt("select ISNULL(Tot_seat,0)-ISNULL(allotedSeats,0) as Remaining from seattype_cat where collegeCode='" + collegeCode + "' and Category_Code='" + streamValue + "' and Degree_Code='" + degCode + "' and Batch_Year='" + batchYr + "' and Quota='" + categCode + "'");
        if (seatRemains > 0)
            available = true;
        return available;
    }
    //Admission Number Generation Based on Settings & while saving receipt
    private bool getAdmGenOnRcpt(string collegeCode, string userCode, ref string batch)
    {
        bool isAdmGenOnRcpt = false;
        try
        {
            string[] prevVal = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='AdmissionNoGenerateOnReceipt' and college_code ='" + collegeCode + "' and user_code='" + userCode + "' ").Trim().Split(',');
            if (prevVal.Length == 2)
            {
                if (prevVal[0] == "1")
                {
                    isAdmGenOnRcpt = true;
                    batch = prevVal[1];
                }
            }
        }
        catch { }

        return isAdmGenOnRcpt;
    }
    private bool isAdmNoNotGenerated(string appNo, string batch, string collegeCode, ref string app_formNo)
    {
        bool isNotGenerated = false;
        try
        {
            app_formNo = d2.GetFunction("select a.app_formno,a.app_no,a.batch_year from applyn a where isnull(a.is_enroll,0) <> '2' and a.app_no = '" + appNo + "'  and a.batch_year='" + batch + "'  and a.college_code='" + collegeCode + "'").Trim();
            if (app_formNo != string.Empty && app_formNo != "0")
            {
                isNotGenerated = true;
            }
        }
        catch { }
        return isNotGenerated;
    }
    protected double collegewiseapplicationRights(string collegeCode)
    {
        double RightsCode = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select linkvalue from New_InsSettings where LinkName='CollegewiseAdmissionNoRights' and user_code ='" + UserCode + "' and college_code ='" + collegeCode + "'")), out RightsCode);
        return RightsCode;
    }
    public bool admissionNoGeneration()
    {
        bool SetFlag = false;
        try
        {
            string value = d2.GetFunction("select value from Master_Settings where settings ='Admission No Rights' and usercode ='" + UserCode + "'");
            if (value.Trim() == "1")
            {
                SetFlag = true;
            }
        }
        catch { }
        return SetFlag;
    }
    //Admission no generation
    private string generateApplNo(string collegecode, int degreecode, string edulevel, string mode, string seattype, string batchyear, out int format)
    {
        string applNo = string.Empty;
        format = 0;
        try
        {
            ApplicationNumberGeneration appGen = new ApplicationNumberGeneration();
            int codeCheck = 0;
            string query = "select LinkValue from New_InsSettings where LinkName='CollegewiseAdmissionNoRights' and user_code ='" + UserCode + "' "; //and college_code ='" + collegecode + "'
            codeCheck = dirAcc.selectScalarInt(query);
            if (codeCheck > 0)
            {
                applNo = appGen.getApplicationNumber(collegecode, batchyear, 1);
                format = 1;
            }
            else
            {
                query = "select LinkValue from New_InsSettings where LinkName='EdulevelAdmissionNoRights' and user_code ='" + UserCode + "' ";//and college_code ='" + collegecode + "'
                codeCheck = dirAcc.selectScalarInt(query);

                if (codeCheck > 0)
                {
                    applNo = appGen.getApplicationNumber(collegecode, edulevel, batchyear, 1);
                    format = 2;
                }
                else
                {
                    query = "select LinkValue from New_InsSettings where LinkName='DegreeSeatModewiseAdmissionNoRights' and user_code ='" + UserCode + "' ";//and college_code ='" + collegecode + "'
                    codeCheck = dirAcc.selectScalarInt(query);
                    if (codeCheck > 0)
                    {
                        applNo = appGen.getApplicationNumber(collegecode, batchyear, degreecode.ToString(), mode, seattype, 1);
                        format = 3;
                    }
                    else
                    {
                        applNo = appGen.getApplicationNumber(collegecode, batchyear, degreecode, 1);
                        format = 0;
                    }
                }
            }
        }
        catch { applNo = string.Empty; }
        return applNo;
    }
    private bool UpdateApplNo(string collegecode, int degreecode, string edulevel, string mode, string seattype, string batchyear, int format)
    {
        bool update = false;

        try
        {
            ApplicationNumberGeneration appGen = new ApplicationNumberGeneration();
            int codeCheck = 0;
            string query = "select LinkValue from New_InsSettings where LinkName='CollegewiseAdmissionNoRights' and user_code ='" + UserCode + "' "; //and college_code ='" + collegecode + "'
            codeCheck = dirAcc.selectScalarInt(query);
            if (codeCheck > 0)
            {
                update = appGen.updateApplicationNumber(collegecode, batchyear, 1);

            }
            else
            {
                query = "select LinkValue from New_InsSettings where LinkName='EdulevelAdmissionNoRights' and user_code ='" + UserCode + "'"; // and college_code ='" + collegecode + "'
                codeCheck = dirAcc.selectScalarInt(query);

                if (codeCheck > 0)
                {
                    update = appGen.updateApplicationNumber(collegecode, edulevel, batchyear, 1);

                }
                else
                {
                    query = "select LinkValue from New_InsSettings where LinkName='DegreeSeatModewiseAdmissionNoRights' and user_code ='" + UserCode + "' "; //and college_code ='" + collegecode + "'
                    codeCheck = dirAcc.selectScalarInt(query);
                    if (codeCheck > 0)
                    {
                        update = appGen.updateApplicationNumber(collegecode, batchyear, degreecode.ToString(), mode, seattype, 1);

                    }
                    else
                    {
                        update = appGen.updateApplicationNumber(collegecode, batchyear, degreecode, 1);

                    }
                }
            }
        }
        catch { update = false; }
        return update;
    }
    //TextVal code creation
    public string getTextCodeOrInsert(string textCriteria, string textName, string collegeCode)
    {
        string textCode = string.Empty;
        textName = textName.Trim();
        textCriteria = textCriteria.Trim();
        try
        {
            string select_subno = "select TextCode from textvaltable where TextCriteria='" + textCriteria + "' and college_code ='" + Convert.ToString(collegeCode).Trim() + "' and TextVal='" + textName + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                textCode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]).Trim();
            }
            else
            {
                string insertquery = "insert into textvaltable(TextCriteria,TextVal,college_code) values('" + textCriteria + "','" + textName + "','" + Convert.ToString(collegeCode).Trim() + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select TextCode from textvaltable where TextCriteria='" + textCriteria + "' and college_code =" + Convert.ToString(collegeCode).Trim() + " and TextVal='" + textName + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        textCode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]).Trim();
                    }
                }
            }
        }
        catch
        {
        }
        return textCode;
    }
    //Get Stream value saved
    private string streamVal()
    {
        string streamValue = string.Empty;
        try
        {
            string[] resVal = dirAcc.selectScalarString("SELECT LinkValue FROM New_InsSettings WHERE LinkName='ADMISSIONCOURSESELECTIONSETTINGS' AND college_code='" + ddlCollege.SelectedValue + "'").Split('$');

            //string time = Convert.ToString(System.DateTime.Now.ToString("HH:mm"));
            //string date = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
            //string studentchk = "  select * from (select (left(slotTime,CHARINDEX('-',slotTime)-1))fromtime,substring(slotTime, charindex('-', slotTime) + 1, len(slotTime)) as totime, St_App_no,selection_status,StreamCode from ST_DaySlot SD,ST_RankListSlot SR,ST_StudentSession SS,Applyn a where SD.ST_DAySlotPK=SR.ST_DAySlotFK and SS.ST_RanklistFk= SR.ST_RanklistPK and SS.ST_App_No=a.app_no  and slotDate='" + date + "'  and app_formno='" + txt_applicationno.Text.Trim() + "' ) as Temp where   CONVERT(datetime, fromtime) <= '" + time + "' and CONVERT(datetime, totime) >=  '" + time + "'";
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

    private string streamValNew()
    {
        string streamValue = string.Empty;
        try
        {
            string[] resVal = dirAcc.selectScalarString("SELECT LinkValue FROM New_InsSettings WHERE LinkName='ADMISSIONCOURSESELECTIONSETTINGS' AND college_code='" + ddlCollege.SelectedValue + "'").Split('$');

            //string time = Convert.ToString(System.DateTime.Now.ToString("HH:mm"));
            //string date = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
            //string studentchk = "  select * from (select (left(slotTime,CHARINDEX('-',slotTime)-1))fromtime,substring(slotTime, charindex('-', slotTime) + 1, len(slotTime)) as totime, St_App_no,selection_status,StreamCode from ST_DaySlot SD,ST_RankListSlot SR,ST_StudentSession SS,Applyn a where SD.ST_DAySlotPK=SR.ST_DAySlotFK and SS.ST_RanklistFk= SR.ST_RanklistPK and SS.ST_App_No=a.app_no  and slotDate='" + date + "'  and app_formno='" + txt_applicationno.Text.Trim() + "' ) as Temp where   CONVERT(datetime, fromtime) <= '" + time + "' and CONVERT(datetime, totime) >=  '" + time + "'";
            if (resVal.Length == 6)
            {
                string collegeCode = resVal[0];
                string batchYear = resVal[1];
                string eduLevel = resVal[2];
                string courseCode = resVal[3];
                streamValue = resVal[3];
                string criteriaCode = resVal[5];
            }
        }
        catch { streamValue = string.Empty; }
        return streamValue;
    }
    //Fee allocation on course selection
    public void FeeAllotOnSave(string collegeCode, string degreecode, string app_no, string batchyear, string seattype, string cursem)
    {
        try
        {
            string textcode = string.Empty;
            ListItem feecat = new ListItem();
            string getfinid = d2.getCurrentFinanceYear(UserCode, Convert.ToString(collegeCode));
            string Generalfeeallot = d2.GetFunction("select value from Master_Settings where settings ='GeneralFeeAllot' and usercode ='" + UserCode + "'");

            string includeMulsem = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='IncludeMultipleTermSettings'  and college_code ='" + collegeCode + "'");
            if (includeMulsem == "1")
            {
                string MulsemCode = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='SelectedMultipleFeecategoryCode' and college_code ='" + collegeCode + "'");
                textcode = MulsemCode != "0" ? MulsemCode : "0";
            }
            if (textcode == "0" || string.IsNullOrEmpty(textcode))
            {
                feecat = getFeecategoryNEW(cursem, collegeCode);
                textcode = feecat.Value;
            }
            if (textcode != "0" && getfinid != "" && getfinid != "0")
            {
                string[] splcode = textcode.Split(',');
                for (int row = 0; row < splcode.Length; row++)
                {
                    textcode = Convert.ToString(splcode[row]);
                    string checkfee = "select LedgerFK,HeaderFK,PayMode,FeeAmount,deductAmout,DeductReason,TotalAmount,RefundAmount,FeeCategory,FineAmount from FT_FeeAllotDegree where DegreeCode='" + degreecode + "' and BatchYear ='" + batchyear + "' and SeatType ='" + seattype + "' and FeeCategory ='" + textcode + "' and FinYearFK ='" + getfinid + "'";
                    DataSet ds = d2.select_method_wo_parameter(checkfee, "text");
                    if (Generalfeeallot == "1" && ds.Tables[0].Rows.Count == 0 && textcode != "-1")
                    {
                    }
                    else
                    {
                        string IsGeneralFeeAllot = d2.GetFunction("select value from Master_Settings where settings='GeneralFeeAllot' and usercode='" + UserCode + "'");

                        if (getfinid.Trim() != "" && getfinid.Trim() != "0")
                        {
                            if (IsGeneralFeeAllot.Trim() == "1")
                            {
                                generalFeeallot(degreecode, seattype, batchyear, getfinid, app_no, textcode);
                            }
                        }
                    }
                }
            }
        }
        catch { }
    }
    protected void generalFeeallot(string degreecode, string seattype, string batchyear, string getfinid, string app_no, string textcode)
    {
        string headerfk = "";
        string leadgerfk = "";
        double feeamount = 0;
        double deduct = 0;
        string deductrea = "";
        double totalamount = 0;
        string refund = "";
        string feecatg = "";
        double finamount = 0;
        string paymode = "";

        string qur = "select LedgerFK,HeaderFK,PayMode,FeeAmount,deductAmout,DeductReason,TotalAmount,RefundAmount,FeeCategory,FineAmount from FT_FeeAllotDegree where DegreeCode='" + degreecode + "' and BatchYear ='" + batchyear + "' and SeatType ='" + seattype + "' and FeeCategory ='" + textcode + "' and FinYearFK ='" + getfinid + "'";
        DataSet ds = d2.select_method_wo_parameter(qur, "text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
            {
                headerfk = Convert.ToString(ds.Tables[0].Rows[k]["HeaderFK"]);
                leadgerfk = Convert.ToString(ds.Tables[0].Rows[k]["LedgerFK"]).Trim();
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[k]["FeeAmount"]), out feeamount);
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[k]["deductAmout"]), out deduct);
                deductrea = Convert.ToString(ds.Tables[0].Rows[k]["DeductReason"]);
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[k]["TotalAmount"]), out totalamount);
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[k]["FineAmount"]), out finamount);
                refund = Convert.ToString(ds.Tables[0].Rows[k]["RefundAmount"]);
                feecatg = Convert.ToString(ds.Tables[0].Rows[k]["FeeCategory"]);
                paymode = Convert.ToString(ds.Tables[0].Rows[k]["PayMode"]);

                string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + textcode + "')  and App_No in('" + app_no + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + feeamount + "',PaidAmount='0' ,DeductAmout='" + deduct + "',DeductReason='" + deductrea + "',FromGovtAmt='0',TotalAmount='" + totalamount + "',RefundAmount='" + refund + "',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='" + paymode + "',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='" + finamount + "',BalAmount='" + totalamount + "' where LedgerFK in('" + leadgerfk + "') and HeaderFK in('" + headerfk + "') and FeeCategory in('" + feecatg + "') and App_No in('" + app_no + "') else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + app_no + ",'" + leadgerfk + "','" + headerfk + "','" + feeamount + "','" + deduct + "'," + deductrea + ",'0','" + totalamount + "','" + refund + "','0','','" + paymode + "','" + feecatg + "','','0','','0','" + finamount + "','" + totalamount + "','" + getfinid + "')";
                int a = d2.update_method_wo_parameter(insupdquery, "text");
            }
        }
    }
    protected void getInsertValues(int type, ref double totalamount, ref double consAmt, ref double deduct)
    {
        try
        {
            if (type == 0)
            {
                totalamount = totalamount - consAmt;
                deduct += consAmt;
            }
            else
            {
                double percent = 0;
                percent = Math.Round((totalamount / 100) * consAmt);
                totalamount = totalamount - percent;
                deduct += percent;
            }
        }
        catch { }
    }
    private ListItem getFeecategoryNEW(string Sem, string collegeCode)
    {
        ListItem feeCategory = new ListItem();
        string linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + UserCode + "' and college_code ='" + collegeCode + "'");
        DataSet dsFeecat = new DataSet();
        if (linkvalue == "0")
        {
            dsFeecat = d2.select_method_wo_parameter("selECT TextCode,textval from textvaltable where TextCriteria ='FEECA' and textval = '" + Sem + " Semester' and college_code=" + collegeCode + "", "Text");
        }
        else if (linkvalue == "1")
        {
            string year = newfunction(Sem);
            dsFeecat = d2.select_method_wo_parameter("selECT TextCode,textval from textvaltable where TextCriteria ='FEECA' and textval = '" + year + " Year' and college_code=" + collegeCode + "", "Text");
        }
        else if (linkvalue == "2")
        {
            string term = newfunction(Sem);
            dsFeecat = d2.select_method_wo_parameter("selECT TextCode,textval from textvaltable where TextCriteria ='FEECA' and textval = 'Term " + term + "' and college_code=" + collegeCode + "", "Text");
        }
        if (dsFeecat.Tables.Count > 0 && dsFeecat.Tables[0].Rows.Count > 0)
        {
            feeCategory.Text = Convert.ToString(dsFeecat.Tables[0].Rows[0]["textval"]);
            feeCategory.Value = Convert.ToString(dsFeecat.Tables[0].Rows[0]["TextCode"]);
        }
        else
        {
            feeCategory.Text = " ";
            feeCategory.Value = "-1";
        }
        return feeCategory;
    }
    public string newfunction(string val)
    {
        string value = "";
        if (val.Trim() == "1" || val.Trim() == "2")
        {
            value = "1";
        }
        if (val.Trim() == "3" || val.Trim() == "4")
        {
            value = "2";
        }
        if (val.Trim() == "5" || val.Trim() == "6")
        {
            value = "3";
        }
        if (val.Trim() == "7" || val.Trim() == "8")
        {
            value = "4";
        }
        if (val.Trim() == "9" || val.Trim() == "10")
        {
            value = "5";
        }
        return value;
    }
    //Print Option for Admission Letter
    protected void btn_printDiv_Click(object sender, EventArgs e)
    {
        divPrintAdmLetter.Visible = false;
        contentDiv.Visible = true;
        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "PrintDiv()", true);

    }
    protected void btn_printclose_Click(object sender, EventArgs e)
    {
        divPrintAdmLetter.Visible = false;
    }
    //Print option for already enrolled student
    protected void btnPrintDup_Click(object sender, EventArgs e)
    {
        divPrintDuplicate.Visible = false;
        contentDiv.Visible = true;
        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "PrintDiv()", true);

    }
    protected void btnPrintDupClose_Click(object sender, EventArgs e)
    {
        divPrintDuplicate.Visible = false;
    }
    private bool printAdmitCardPrevious(string app_FormNo)
    {
        bool enrolled = false;
        try
        {
            contentDiv.InnerHtml = string.Empty;

            string appFormNo = txt_applicationno.Text.Trim();
            string collegeCode = ddlCollege.Items.Count > 0 ? ddlCollege.SelectedValue : string.Empty;
            string appNo = getAppNoAndValidate(appFormNo, collegeCode);

            string streamValue = streamVal();

            if (!string.IsNullOrEmpty(appNo))
            {
                string app_formNo = string.Empty;
                bool isAdmNoGenSettingsOn = false;
                string admNoGenbatch = string.Empty;
                isAdmNoGenSettingsOn = getAdmGenOnRcpt(collegeCode, UserCode, ref  admNoGenbatch);
                if (!isAdmNoNotGenerated(appNo, admNoGenbatch, collegeCode, ref app_formNo))
                {
                    string queryRollApp = "select a.stud_name,a.app_formno,a.app_no,a.current_semester,a.mode,a.batch_year,a.seattype,c.Edu_Level,c.Course_Name,dt.Dept_Name from applyn a,Registration r,Degree d, Department dt, course c where a.app_no= r.App_No and d.Degree_Code=r.degree_code and d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id and  a.app_no='" + appNo + "'  and a.college_code='" + collegeCode + "' ";
                    DataSet dsRollApp = new DataSet();
                    dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");

                    string studname = string.Empty;
                    string eduleve = string.Empty;
                    string Mode = string.Empty;
                    string seattype = string.Empty;
                    string batchYr = string.Empty;
                    string cursem = string.Empty;
                    string course = string.Empty;
                    string branch = string.Empty;

                    if (dsRollApp.Tables.Count > 0 && dsRollApp.Tables[0].Rows.Count > 0)
                    {
                        app_formNo = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formNo"]);
                        studname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["stud_name"]);
                        eduleve = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Edu_Level"]);
                        Mode = Convert.ToString(dsRollApp.Tables[0].Rows[0]["mode"]);
                        seattype = Convert.ToString(dsRollApp.Tables[0].Rows[0]["seattype"]);
                        batchYr = Convert.ToString(dsRollApp.Tables[0].Rows[0]["batch_year"]);
                        cursem = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Current_Semester"]);
                        course = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Course_Name"]);
                        branch = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Dept_Name"]);
                        //if (seattype.Trim() == "")
                        //{
                        //    seattype = dirAcc.selectScalarString("select TextCode from TextValtable where TextCriteria='seat' and college_code=" + collegeCode + " and TextVal like 'M%'");
                        //}
                    }

                    string selCertQ = "select M.MasterValue from StudCertDetails_New c,CO_MasterValues m where c.CertificateId=m.MasterCode and ISNULL(c.isOrginal,'0')='1' and c.App_no='" + appNo + "' and CertificateId <>'946'";
                    DataTable dtCert = dirAcc.selectDataTable(selCertQ);
                    string certDet = string.Empty;
                    if (dtCert.Rows.Count > 0)
                    {
                        certDet = "The following original documents have been received. <br /><br />";
                        foreach (DataRow drCert in dtCert.Rows)
                        {
                            certDet += Convert.ToString(drCert["MasterValue"]) + ", ";
                        }
                        certDet = certDet.Trim().TrimEnd(',');
                        certDet += "<br /><br />";
                    }

                    int curBatch = Convert.ToInt32(ddlbatch.SelectedValue);
                    string acadYear = curBatch + "-" + (curBatch + 1);
                    //<tr><td  style='width:170px;'>Campus</td><td>: Kumbakonam</td></tr>
                    StringBuilder sbAdmLetter = new StringBuilder();
                    sbAdmLetter.Append("<div style='height: 850px; width: 730px; font-family: Times New Roman; font-size: 16px; padding: 10px; padding-left:50px;'> <center><div style='height: 220px;'></div><div style='width: 730px; text-align: center; text-decoration: underline; padding-bottom:25px;'>ADMISSION LETTER</div><div style='width: 730px; padding-bottom:25px;'><span  style='padding-left: 530px;'>" + DateTime.Now.ToString("dd-MM-yyyy") + "</span></div><table style='width: 730px; text-align: left;' cellpadding=5><tr><td style='width:170px;'>Name</td><td>: " + studname.ToUpper() + "</td></tr><tr><td  style='width:170px;'>Application No</td><td>: " + app_formNo + "</td></tr><tr><td  style='width:170px;'>Programme Admitted to</td><td>: " + (course + " " + branch) + "</td></tr></tr><tr><td  style='width:170px;'>Academic Year</td><td>: 2017-18</td></tr><tr><td  style='width:170px;'>Campus</td><td>: Kumbakonam</td></tr></table><div style='width:730px; padding-top:25px; text-align:left;'><p>Dear <b>" + studname.ToUpper() + "</b><br /><br />Congratulations!<br /><br />Welcome to the SASTRA student fraternity.<br /><br />" + certDet + "Classes for First year will commence on <b>" + getCommenceDateTime(collegeCode) + ".</b><br /><br />Wishing you a fruitful stay at SASTRA UNIVERSITY.<br /><br />Yours faithfully,      <br /><br /><img src='../image/registrarsign" + collegeCode + ".jpeg' style='height:40px; width:120px;'/><br /><br /><b>REGISTRAR</b></p></div></center></div>");
                    contentDiv.InnerHtml = sbAdmLetter.ToString();
                    divPrintDuplicate.Visible = true;
                    enrolled = true;
                }
            }
        }
        catch
        {
            enrolled = false;
        }
        return enrolled;
    }
    private string getAppNoAndValidate(string appFormNo, string collegeCode)
    {
        string appNo = string.Empty;
        try
        {
            if (!string.IsNullOrEmpty(appFormNo) && !string.IsNullOrEmpty(collegeCode))
            {
                appNo = dirAcc.selectScalarString("select a.app_No from applyn a,registration r  where a.app_no=r.app_no and app_formno='" + appFormNo + "' and a.college_code='" + collegeCode + "' and isnull(isconfirm,'0')='1' and isnull(selection_status,'0')='1' and isnull(admission_Status,'0')='1'");
            }
        }
        catch { appNo = string.Empty; }
        return appNo;
    }
    //Get Commence Date and Time
    private string getCommenceDateTime(string collegeCode)
    {
        string commDateVal = string.Empty;
        try
        {
            string commDateTime = dirAcc.selectScalarString("select LinkValue from New_InsSettings where college_code='" + collegeCode + "' and LinkName='CommenceDateAndTime'");
            string[] commDateTimes = commDateTime.Split(',');
            if (commDateTimes.Length == 2)
            {
                string commDate = commDateTimes[0];
                string[] commDateDt = commDate.Split('/');
                DateTime dtcommDate = Convert.ToDateTime(commDateDt[1] + "/" + commDateDt[0] + "/" + commDateDt[2]);
                commDateVal = commDate + " (" + dtcommDate.DayOfWeek + ") at " + commDateTimes[1].ToLower();
            }
        }
        catch
        {
            commDateVal = string.Empty;
        }
        return commDateVal;
    }
    //Get Registrar signature
    private void getRegistrarSign()
    {
        try
        {
            string logoQ = "select registrarSign from collinfo where college_code=" + ddlCollege.SelectedValue + "";
            DataTable dtLogo = dirAcc.selectDataTable(logoQ);
            if (dtLogo.Rows.Count > 0)
            {
                string logoname = Server.MapPath("~/image/registrarsign" + ddlCollege.SelectedValue + ".jpeg");
                if (File.Exists(logoname))
                {
                    File.Delete(logoname);
                }
                if (!File.Exists(logoname))
                {
                    MemoryStream memoryStream = new MemoryStream();
                    byte[] file = (byte[])dtLogo.Rows[0]["registrarSign"];
                    memoryStream.Write(file, 0, file.Length);
                    if (file.Length > 0)
                    {
                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                        thumb.Save(logoname, System.Drawing.Imaging.ImageFormat.Jpeg);

                    }
                    memoryStream.Dispose();
                    memoryStream.Close();
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_clear_click(object sender, EventArgs e)
    {
        clearDetails(true);
    }
}
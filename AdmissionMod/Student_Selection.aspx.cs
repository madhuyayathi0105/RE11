using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using InsproDataAccess;

public partial class AdmissionMod_Student_Selection : System.Web.UI.Page
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
            txt_date.Attributes.Add("readonly", "readonly");
            txt_date.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            string value = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ShowListRegistration' and college_code='" + ddlCollege.SelectedValue + "'");
            if (value.Trim() == "1")
            {
                registration_div.Visible = false;
                btnGo.Visible = true;
            }
            else
            {
                registration_div.Visible = true;
                btnGo.Visible = false;
            }

            BindSession();
            functionShowcount();
        }
        functionShowcount();
    }

    public void bindCollege()
    {
        try
        {
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
            if (ddlEduLev.Items.Count > 0)
            {
                ds.Clear();
                ds = d2.select_method_wo_parameter("select distinct course_id,Course_Name from Course where Edu_Level='" + ddlEduLev.SelectedItem.Value + "' and college_code=" + ddlCollege.SelectedValue + " order by course_id", "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlcourse.DataSource = ds;
                    ddlcourse.DataTextField = "Course_Name";
                    ddlcourse.DataValueField = "course_id";
                    ddlcourse.DataBind();
                }
            }
        }
        catch
        {
        }
    }
    protected void ddlEduLev_selectedindexchanged(object sender, EventArgs e)
    {
        bindCourse();
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

            string time = Convert.ToString(System.DateTime.Now.ToString("HH:mm"));
            string date = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));

            //string query = " select app_formno from (select (left(Enrollment_session,CHARINDEX('-',enrollment_session)-1))fromtime,substring(Enrollment_session, charindex('-', Enrollment_session) + 1, len(Enrollment_session)) as totime,app_formno,Enrollment_session from applyn a,course c where a.college_code='" + collegecode + "' and batch_year='" + batchyear + "' and a.courseID=c.Course_Id  and c.Edu_Level='" + edulevel + "' and c.Course_Id='" + courseid + "' and isnull(EnrollmentCard,0)=1 and Enrollment_card_date='" + date + "' and isnull(Enrollment_card_date,'')<>'' and isnull(EnrollmentCard,0)<>0  " + regquery + ") checkscheduletime  where CONVERT(datetime, fromtime) <= '" + time + "' and CONVERT(datetime, totime) >=  '" + time + "' and app_formno like '" + prefixText + "%'";

            string query = "  select app_formno from (select (left(Enrollment_session,CHARINDEX('-',enrollment_session)-1))fromtime,substring(Enrollment_session, charindex('-', Enrollment_session) + 1, len(Enrollment_session)) as totime,app_formno,Enrollment_session from applyn a,course c where a.college_code='" + collegecode + "' and batch_year='" + batchyear + "' and a.courseID=c.Course_Id  and c.Edu_Level='" + edulevel + "' and c.Course_Id='" + courseid + "' and isnull(EnrollmentCard,0)=1  and IsConfirm='1' and ISNULL(selection_status,0)=0 and ISNULL(admission_status,0)=0) checkscheduletime  where  app_formno like '" + prefixText + "%'";
            name = ws.Getname(query);
        }
        return name;
    }
    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        alert_pop.Visible = false;
        txt_applicationno.Focus();
    }
    public void rdbtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbtype.SelectedItem.Value == "0")
        {
            btn_register.Text = "Register";
            btn_shortlist.Visible = false;
            verification_div.Visible = false;
        }
        if (rdbtype.SelectedItem.Value == "1")
        {
            btn_register.Text = "Submit";
            btn_shortlist.Visible = true;
            verification_div.Visible = false;
        }
    }
    protected void btn_register_click(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            if (rdbtype.SelectedItem.Value == "0")
            {
                verification_div.Visible = false;
                if (txt_applicationno.Text.Trim() != "")
                {
                    string time = Convert.ToString(System.DateTime.Now.ToString("HH:mm"));
                    string date = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                    ds = d2.select_method_wo_parameter("  select * from (select (left(slotTime,CHARINDEX('-',slotTime)-1))fromtime,substring(slotTime, charindex('-', slotTime) + 1, len(slotTime)) as totime, St_App_no,selection_status from ST_DaySlot SD,ST_RankListSlot SR,ST_StudentSession SS,Applyn a where SD.ST_DAySlotPK=SR.ST_DAySlotFK and SS.ST_RanklistFk= SR.ST_RanklistPK and SS.ST_App_No=a.app_no  and slotDate='" + date + "'  and app_formno='" + txt_applicationno.Text + "' ) as Temp where   CONVERT(datetime, fromtime) <= '" + time + "' and CONVERT(datetime, totime) >=  '" + time + "'", "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToString(ds.Tables[0].Rows[0]["selection_status"]).Trim() == "False" || Convert.ToString(ds.Tables[0].Rows[0]["selection_status"]).Trim() == "0")
                        {
                            int selection = d2.update_method_wo_parameter(" update applyn set selection_status='1' where app_formno='" + txt_applicationno.Text + "'", "Text");
                            if (selection != 0)
                            {
                                lbl_alert.Text = "Student Registered Successfully";
                                alert_pop.Visible = true;
                                txt_applicationno.Text = "";
                            }
                        }
                        else
                        {
                            lbl_alert.Text = "Student Already Registered";
                            alert_pop.Visible = true;
                            txt_applicationno.Text = "";
                        }
                    }
                    else
                    {
                        lbl_alert.Text = "Please check student schedule time";
                        alert_pop.Visible = true;
                    }
                }
            }
            else if (rdbtype.SelectedItem.Value == "1")
            {
                string certificatequery = "  SELECT (select MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CertName=MasterCode)CertificateName,CertName FROM CertMasterDet where CourseID='" + ddlcourse.SelectedItem.Value + "'";
                certificatequery += " select a.app_no,app_formno,stud_name,CONVERT(varchar(10),dob,103)dob,case when sex='0' then 'Male' when sex='1' then 'Female' when sex='2' then 'Transgender' end sex,Student_Mobile,parent_name,cast(sm.jeeMarkSec as decimal(10,2))jeeMarkSec,sm.jeeStateRank,(select textval from textvaltable where TextCriteria='unive' and TextCode=sm.board)board,cast(sm.HSCMarkSec as decimal(10,2))HSCMarkSec,convert(varchar,sm.yearPassing)+'-'+convert(varchar, DateName( month , DateAdd( month , sm.monthpassing , 0 ) - 1 )) as yearofpassing  from applyn a left join ST_Student_Mark_Detail sm on a.app_no=sm.ST_AppNo  where isnull(a.EnrollmentCard,0)=1 and ISNULL(a.Enrollment_session,'')<>'' and ISNULL(a.Enrollment_card_date,'')<>'' and isnull(a.EnrollmentCard,0)<>0 and a.IsConfirm='1' and isnull(Admission_Status,'0')<>'1' and app_formno='" + txt_applicationno.Text + "'";
                ds = d2.select_method_wo_parameter(certificatequery, "text");
                if (ds.Tables != null)
                {
                    #region certificate details
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        certificate_grid.DataSource = ds.Tables[0];
                        certificate_grid.DataBind();
                        certificate_grid_div.Visible = true;
                        if (certificate_grid.Rows.Count > 0)
                        {
                            if (certificate_grid.HeaderRow.Cells.Count > 0)
                                certificate_grid.HeaderRow.Cells[certificate_grid.Columns.Count + 1].Visible = false;
                            for (int i = 0; i < certificate_grid.Rows.Count; i++)
                            {
                                certificate_grid.Rows[i].Cells[certificate_grid.Columns.Count + 1].Visible = false;
                            }
                        }
                    }
                    else
                    {
                        certificate_grid.DataBind();
                        certificate_grid_div.Visible = false;
                    }
                    #endregion

                    #region student details
                    if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
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
                    }
                    #endregion

                    verification_div.Visible = true;
                    cb_selectall.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "Student_Selection");
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
        cb_selectall.Checked = false;
    }
    protected void btn_shortlist_click(object sender, EventArgs e)
    {
        try
        {
            bool certchk = false;
            if (txt_applicationno.Text.Trim() != "")
            {
                if (certificate_grid.Rows.Count > 0)
                {
                    for (int i = 0; i < certificate_grid.Rows.Count; i++)
                    {
                        CheckBox chkorginal = (CheckBox)certificate_grid.Rows[i].FindControl("cb_select");
                        if (!chkorginal.Checked)
                        {
                            lbl_alert.Text = "Submit All Certificates";
                            alert_pop.Visible = true;
                            return;
                        }
                    }
                    string app_no = d2.GetFunction(" select app_no from applyn where app_formno='" + txt_applicationno.Text + "'");
                    for (int i = 0; i < certificate_grid.Rows.Count; i++)
                    {
                        CheckBox chkorginal = (CheckBox)certificate_grid.Rows[i].FindControl("cb_select");
                        if (chkorginal.Checked)
                        {
                            string CertName = certificate_grid.Rows[i].Cells[certificate_grid.Rows[i].Cells.Count - 1].Text;
                            int certins = d2.update_method_wo_parameter("  if not exists(select app_no from StudCertDetails_New where App_no='" + app_no + "' and CertificateId='" + CertName + "') insert into StudCertDetails_New (App_no,CertificateId,isDuplicate,isOrginal)values('" + app_no + "','" + CertName + "','1','1')", "text");
                            if (certins != 0)
                            {
                                certchk = true;
                            }
                        }
                    }
                }
                int selection = d2.update_method_wo_parameter(" update applyn set Admission_Status='1' where app_formno='" + txt_applicationno.Text + "'", "Text");
                if (selection != 0)
                {
                    lbl_alert.Text = "Student Verified Successfully";
                    alert_pop.Visible = true;
                    txt_applicationno.Text = "";
                    clear();
                    txt_applicationno.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Session["collegecode"].ToString(), "Student_Selection");
        }
    }
    protected void cb_selectall_click(object sender, EventArgs e)
    {
        if (certificate_grid.Rows.Count > 0)
        {
            for (int i = 0; i < certificate_grid.Rows.Count; i++)
            {
                CheckBox chkorginal = (CheckBox)certificate_grid.Rows[i].FindControl("cb_select");
                if (cb_selectall.Checked)
                    chkorginal.Checked = true;
                else
                    chkorginal.Checked = false;
            }
            btn_shortlist.Focus();
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            string time = Convert.ToString(System.DateTime.Now.ToString("HH:mm"));
            string date = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
            string Stream = streamVal();

            if (ddlSession.Items.Count > 0)
            {
                if (Stream.Trim() != "0" && Stream.Trim() != "Stream II")
                {
                    ds = d2.select_method_wo_parameter(" select * from (select (left(slotTime,CHARINDEX('-',slotTime)-1))fromtime,substring(slotTime, charindex('-', slotTime) + 1, len(slotTime)) as totime, St_App_no,selection_status,app_formno,a.stud_name,CONVERT(varchar(10),dob,103) as dob,HSCMarkSec,jeeMarkSec,cast(CombinedScore as decimal(10,4)) as CombinedScore,cast(CombinedScoreSII as decimal(10,4)) as CombinedScoreII,sst.ST_Rank ,(select Mastervalue from co_Mastervalues where mastercode=sst.ST_RankCriteria) as  ST_RankCriteria  from ST_DaySlot SD,ST_RankListSlot SR,ST_StudentSession SS,Applyn a,ST_Student_Mark_Detail St,ST_RankTable sst where SD.ST_DAySlotPK=SR.ST_DAySlotFK and SS.ST_RanklistFk= SR.ST_RanklistPK and SS.ST_App_No=a.app_no and st.ST_AppNo=a.app_no and sst.ST_AppNo =st.ST_AppNo and sst.ST_AppNo =a.app_no and sr.streamCode =sst.ST_Stream and sst.ST_RankCriteria =sr.criteriaCode  and slotDate='" + date + "' and isnull(selection_status,'0') ='0' and SlotTime ='" + ddlSession.SelectedItem.Text + "' ) as Temp order by ST_Rank asc", "Text");
                }
                else
                {
                    ds = d2.select_method_wo_parameter(" select * from (select (left(slotTime,CHARINDEX('-',slotTime)-1))fromtime,substring(slotTime, charindex('-', slotTime) + 1, len(slotTime)) as totime, St_App_no,selection_status,app_formno,a.stud_name,CONVERT(varchar(10),dob,103) as dob,HSCMarkSec,jeeMarkSec,cast(CombinedScoreSII as decimal(10,4)) as CombinedScore,sst.ST_Rank ,(select Mastervalue from co_Mastervalues where mastercode=sst.ST_RankCriteria) as  ST_RankCriteria  from ST_DaySlot SD,ST_RankListSlot SR,ST_StudentSession SS,Applyn a,ST_Student_Mark_Detail St,ST_RankTable sst where SD.ST_DAySlotPK=SR.ST_DAySlotFK and SS.ST_RanklistFk= SR.ST_RanklistPK and SS.ST_App_No=a.app_no and st.ST_AppNo=a.app_no and sst.ST_AppNo =st.ST_AppNo and sst.ST_AppNo =a.app_no and sr.streamCode =sst.ST_Stream and sst.ST_RankCriteria =sr.criteriaCode  and slotDate='" + date + "' and isnull(selection_status,'0') ='0' and SlotTime ='" + ddlSession.SelectedItem.Text + "' ) as Temp order by ST_Rank asc", "Text");
                }
                //where   CONVERT(datetime, fromtime) <= '" + time + "' and CONVERT(datetime, totime) >=  '" + time + "'
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    gridShowDetails.DataSource = ds.Tables[0];
                    gridShowDetails.DataBind();
                    listRegisterDiv.Visible = true;
                    if (gridShowDetails.Rows.Count > 0)
                    {
                        if (Stream.Trim() != "0" && Stream.Trim() != "Stream I")
                        {

                            gridShowDetails.Columns[7].Visible = false;
                            gridShowDetails.Columns[8].Visible = false;

                        }
                        else
                        {                           
                            gridShowDetails.Columns[9].Visible = false;
                        }
                    }
                }
                else
                {
                    lbl_alert.Text = "No Recards Found";
                    alert_pop.Visible = true;
                }
            }
            else
            {
                lbl_alert.Text = "Please Allot Session";
                alert_pop.Visible = true;
            }
        }
        catch
        {

        }
    }

    protected void btnSaveTT_OnClick(object sender, EventArgs e)
    {
        try
        {
            Button btnSaveTT = (Button)sender;
            string rowIndxS = btnSaveTT.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int selIndx = Convert.ToInt32(rowIndxS) - 2;

            Label lblTTSec = (Label)gridShowDetails.Rows[selIndx].FindControl("lblapplno");
            if (lblTTSec.Text.Trim() != "")
            {
                int selection = d2.update_method_wo_parameter(" update applyn set selection_status='1' ,enrollment_card_date ='" + DateTime.Now + "',Enrolled_User='" + UserCode + "' where app_no='" + lblTTSec.Text + "' and batch_year='" + ddlbatch.SelectedItem.Text + "' and college_code ='" + ddlCollege.SelectedValue + "'", "Text");
                if (selection != 0)
                {
                    btnGo_Click(sender, e);
                    // btnGo.Focus();
                    //lbl_alert.Text = "Student Registered Successfully";
                    //alert_pop.Visible = true;
                    //txt_applicationno.Text = "";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Student Registered Successfully')", true);

                }
                else
                {
                    lbl_alert.Text = "Not Registered";
                    alert_pop.Visible = true;
                }
            }
        }
        catch
        {

        }
    }

    public void functionShowcount()
    {
        try
        {
            string time = Convert.ToString(System.DateTime.Now.ToString("HH:mm"));
            string date = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
            string Query = string.Empty;
            if (ddlSession.Items.Count > 0)
            {
                Query = "select * from (select (left(slotTime,CHARINDEX('-',slotTime)-1))fromtime,substring(slotTime, charindex('-', slotTime) + 1, len(slotTime)) as totime from ST_DaySlot SD,ST_RankListSlot SR,ST_StudentSession SS,Applyn a,ST_Student_Mark_Detail St,ST_RankTable sst where SD.ST_DAySlotPK=SR.ST_DAySlotFK and SS.ST_RanklistFk= SR.ST_RanklistPK and SS.ST_App_No=a.app_no and st.ST_AppNo=a.app_no and sst.ST_AppNo =st.ST_AppNo and sst.ST_AppNo =a.app_no and sr.streamCode =sst.ST_Stream and sst.ST_RankCriteria =sr.criteriaCode  and slotDate='" + date + "' and SlotTime ='" + ddlSession.SelectedItem.Text + "' ) as Temp ";
                //and isnull(selection_status,'0') ='0' 
                Query += " select * from (select (left(slotTime,CHARINDEX('-',slotTime)-1))fromtime,substring(slotTime, charindex('-', slotTime) + 1, len(slotTime)) as totime from ST_DaySlot SD,ST_RankListSlot SR,ST_StudentSession SS,Applyn a,ST_Student_Mark_Detail St,ST_RankTable sst where SD.ST_DAySlotPK=SR.ST_DAySlotFK and SS.ST_RanklistFk= SR.ST_RanklistPK and SS.ST_App_No=a.app_no and st.ST_AppNo=a.app_no and sst.ST_AppNo =st.ST_AppNo and sst.ST_AppNo =a.app_no and sr.streamCode =sst.ST_Stream and sst.ST_RankCriteria =sr.criteriaCode  and slotDate='" + date + "' and SlotTime ='" + ddlSession.SelectedItem.Text + "' and isnull(selection_status,'0') ='1' ) as Temp  ";
                ds = d2.select_method_wo_parameter(Query, "Text");

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    CalledSpan.InnerHtml = ": " + Convert.ToString(ds.Tables[0].Rows.Count);
                    RegisterdSpan.InnerHtml = ": " + Convert.ToString(ds.Tables[1].Rows.Count);
                }
            }

        }
        catch
        {

        }
    }

    protected void tmrTTStat_OnTick(object sender, EventArgs e)
    {
        functionShowcount();
    }

    private void BindSession()
    {
        try
        {
            string qry = string.Empty;
            qry = "select distinct SlotTime from ST_DaySlot ds where SlotTime is not null and SlotTime<>'' and SlotDate ='" + DateTime.Now.ToString("MM/dd/yyyy") + "'";//and SlotDate between '05/20/2017' and '05/31/2017' and ds.BatchYear='2016' and ds.CollegeCode='13' and ds.CourseID='24' and ds.EduLevel='UG'
            ds = d2.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSession.DataSource = ds;
                ddlSession.DataTextField = "SlotTime";
                ddlSession.DataValueField = "SlotTime";
                ddlSession.DataBind();
                ddlSession.Enabled = true;
            }
        }
        catch (Exception ex)
        {

        }
    }

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
                streamValue = d2.GetFunction("select TextVal  from TextValTable where TextCode =" + streamValue + "");

            }
        }
        catch { streamValue = string.Empty; }
        return streamValue;
    }
}
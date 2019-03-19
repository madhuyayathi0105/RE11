/*
 * 
 * Author : Mohamed Idhris Sheik Dawood
 * Date created : 23-05-2017
 * 
 * */

using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Linq;
using InsproDataAccess;
using System.Collections.Generic;

public partial class SlotwiseRankListSettings : System.Web.UI.Page
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
            bindStream();
            bindCategory();
            bindDate();
            bindSession();
        }
    }
    //Base screen controls loaders
    private void bindCollege()
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
    private void bindBatch()
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
    private void bindEdulevel()
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
    private void bindCourse()
    {
        try
        {
            ds.Clear();
            ds = d2.select_method_wo_parameter("select distinct course_id,Course_Name from Course where college_code=" + ddlCollege.SelectedValue + " and edu_level='" + ddlEduLev.SelectedValue + "' order by course_id", "Text");
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
    private void bindDate()
    {
        try
        {
            ddlDate.Items.Clear();
            ds.Clear();
            ds = d2.select_method_wo_parameter("select distinct CONVERT(varchar(10),SlotDate,103) as SlotDate,CONVERT(varchar(10),SlotDate,101) as SlotDateVal from st_dayslot where collegecode='" + ddlCollege.SelectedValue + "' and batchyear='" + ddlbatch.SelectedValue + "' and CourseId='" + ddlcourse.SelectedValue + "' and EduLevel='" + ddlEduLev.SelectedValue + "' order by SlotDate ", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlDate.DataSource = ds;
                ddlDate.DataTextField = "SlotDate";
                ddlDate.DataValueField = "SlotDateVal";
                ddlDate.DataBind();
            }
            ddlDate.Items.Insert(0, "All");
        }
        catch
        {

        }
    }
    private void bindStream()
    {
        try
        {
            ddlStream.Items.Clear();
            ds.Clear();
            ds = d2.select_method_wo_parameter("SELECT TextVal,TextCode FROM TextValTable WHERE TextCriteria='ADMst' AND college_code='" + ddlCollege.SelectedValue + "' order by TextVal,TextCode ", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlStream.DataSource = ds;
                ddlStream.DataTextField = "TextVal";
                ddlStream.DataValueField = "TextCode";
                ddlStream.DataBind();
            }
        }
        catch
        {

        }
    }
    private void bindCategory()
    {
        try
        {
            ddlCategory.Items.Clear();
            DataSet dsStudRankCrit = d2.select_method_wo_parameter("select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='StudRankCriteria' and collegeCode ='" + ddlCollege.SelectedValue + "' ", "Text");
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
    private void bindSession()
    {
        try
        {
            cbl_Session.Items.Clear();
            cb_Session.Checked = true;
            txtSession.Text = "Session";

            string selectQ = "select LinkValue from New_InsSettings where LinkName='AdmissionSlotSettings'  and college_code ='" + ddlCollege.SelectedItem.Value + "'";
            string result = d2.GetFunction(selectQ).Trim();
            if (!string.IsNullOrEmpty(result) || result != "0")
            {
                string[] fromtoCombo = result.Split(',');
                foreach (string fromto in fromtoCombo)
                {
                    cbl_Session.Items.Add(fromto);
                }
            }
            CallCheckBoxChangedEvent(cbl_Session, cb_Session, txtSession, "Session");
        }
        catch { }
    }
    //Common Functions
    private void CallCheckBoxChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            tb.Text = dispString;
            if (cb.Checked)
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = true;
                }
                tb.Text = dispString + "(" + cbl.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }
    private void CallCheckBoxListChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            cb.Checked = false;
            tb.Text = dispString;
            int count = 0;
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Selected == true)
                {
                    count++;
                }
            }
            tb.Text = dispString + "(" + count + ")";
            if (count == cbl.Items.Count)
            {
                cb.Checked = true;
            }
        }
        catch { }
    }
    //Base screen controls events
    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindBatch();
        bindEdulevel();
        bindCourse();
        bindStream();
        bindCategory();
        bindDate();
        bindSession();
        btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEdulevel();
        bindCourse();
        bindStream();
        bindCategory();
        bindDate();
        btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void ddlEdulevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindCourse();
        bindStream();
        bindCategory();
        bindDate();
        btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void ddlcourse_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindStream();
        bindCategory();
        bindDate();
        btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void ddlStream_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void ddlCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void ddlDate_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void cb_Session_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_Session, cb_Session, txtSession, "Session");
        btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void cbl_Session_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_Session, cb_Session, txtSession, "Session");
        btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    //Base screen search
    protected void btnBaseGo_OnClick(object sender, EventArgs e)
    {
        try
        {
            gridDaySlots.Visible = false;
            gridDaySlots.DataSource = null;
            gridDaySlots.DataBind();

            btnDaySlotSave.Visible = false;

            DataTable dtDaySlot = new DataTable();
            dtDaySlot.Columns.Add("Date");
            dtDaySlot.Columns.Add("DateVal");
            dtDaySlot.Columns.Add("Slot");
            dtDaySlot.Columns.Add("SlotPk");
            dtDaySlot.Columns.Add("From");
            dtDaySlot.Columns.Add("To");

            string collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            string batchYear = Convert.ToString(ddlbatch.SelectedValue);
            string eduLevel = Convert.ToString(ddlEduLev.SelectedValue);
            string courseCode = Convert.ToString(ddlcourse.SelectedValue);

            StringBuilder sbSlot = new StringBuilder();
            for (int slI = 0; slI < cbl_Session.Items.Count; slI++)
            {
                if (cbl_Session.Items[slI].Selected)
                {
                    sbSlot.Append(cbl_Session.Items[slI].Text + "','");
                }
            }
            if (sbSlot.Length > 2)
            {
                sbSlot.Remove(sbSlot.Length - 3, 3);
            }

            string selDate = ddlDate.SelectedItem.Text;
            string selDateVal = ddlDate.SelectedValue;

            string dateValue = ddlDate.SelectedValue != "All" ? " and slotdate='" + selDateVal + "' " : string.Empty;

            string selQ = "select ST_DaySlotPK,slottime,convert(varchar(10),slotdate,103) as slotdate,convert(varchar(10),slotdate,101) as slotdateVal from st_dayslot where collegecode='" + collegeCode + "' and batchyear='" + batchYear + "' and CourseId='" + courseCode + "' and EduLevel='" + eduLevel + "' " + dateValue + " and Slottime in ('" + sbSlot.ToString() + "') order by slotdate,Slottime";
            DataSet dsSlt = d2.select_method_wo_parameter(selQ, "Text");

            if (dsSlt.Tables.Count > 0 && dsSlt.Tables[0].Rows.Count > 0)
            {
                for (int slI = 0; slI < dsSlt.Tables[0].Rows.Count; slI++)
                {
                    DataRow dr = dtDaySlot.NewRow();
                    dr["Date"] = Convert.ToString(dsSlt.Tables[0].Rows[slI]["slotdate"]);
                    dr["DateVal"] = Convert.ToString(dsSlt.Tables[0].Rows[slI]["slotdateVal"]);
                    dr["Slot"] = Convert.ToString(dsSlt.Tables[0].Rows[slI]["slottime"]);
                    dr["SlotPk"] = Convert.ToString(dsSlt.Tables[0].Rows[slI]["ST_DaySlotPK"]);
                    dtDaySlot.Rows.Add(dr);
                }

                gridDaySlots.Visible = true;
                gridDaySlots.DataSource = dtDaySlot;
                gridDaySlots.DataBind();

                btnDaySlotSave.Visible = true;
            }
            else
            {
                lbl_alert.Text = "No slots available";
                imgdiv2.Visible = true;
            }
        }
        catch
        {
            lbl_alert.Text = "Please check inputs";
            imgdiv2.Visible = true;
        }
    }
    protected void gridDaySlots_DataBound(object sender, EventArgs e)
    {
        try
        {
            for (int i = gridDaySlots.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = gridDaySlots.Rows[i];
                GridViewRow previousRow = gridDaySlots.Rows[i - 1];
                for (int j = 2; j <= 2; j++)
                {
                    bool validation = false;
                    switch (j)
                    {
                        case 2:
                            {
                                Label lblDate = (Label)row.FindControl("lblDate");
                                Label lblDatePrev = (Label)previousRow.FindControl("lblDate");
                                if (lblDate.Text == lblDatePrev.Text)
                                {
                                    validation = true;
                                }
                            }
                            break;
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
            DataSet dsSlotDet = d2.select_method_wo_parameter("select ST_DaySlotFk,RankFrom,RankTo from ST_RankListSlot where   streamCode='" + Convert.ToString(ddlStream.SelectedValue) + "' and criteriaCode='" + ddlCategory.SelectedValue + "' ", "Text");
            if (dsSlotDet.Tables.Count > 0 && dsSlotDet.Tables[0].Rows.Count > 0)
            {
                foreach (GridViewRow gRow in gridDaySlots.Rows)
                {
                    HiddenField hdnSlotPk = (HiddenField)gRow.FindControl("hdnSlotPk");
                    TextBox txtFrom = (TextBox)gRow.FindControl("txtFrom");
                    TextBox txtTo = (TextBox)gRow.FindControl("txtTo");
                    dsSlotDet.Tables[0].DefaultView.RowFilter = "ST_DaySlotFk='" + hdnSlotPk.Value + "'";
                    DataTable dtCur = dsSlotDet.Tables[0].DefaultView.ToTable();
                    if (dtCur.Rows.Count > 0)
                    {
                        txtFrom.Text = Convert.ToString(dtCur.Rows[0]["RankFrom"]);
                        txtTo.Text = Convert.ToString(dtCur.Rows[0]["RankTo"]);
                    }
                }
            }
        }
        catch
        {
        }
    }
    //Base screen save
    protected void btnDaySlotSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            string batchYear = Convert.ToString(ddlbatch.SelectedValue);
            string eduLevel = Convert.ToString(ddlEduLev.SelectedValue);
            string courseCode = Convert.ToString(ddlcourse.SelectedValue);
            string criteriaCode = Convert.ToString(ddlCategory.SelectedValue);
            string Streamvalue = Convert.ToString(ddlStream.SelectedValue);

            DataTable dtFullRank = dirAcc.selectDataTable("select ST_RankPK,ST_AppNo,ST_RankCriteria,ST_Rank from ST_RankTable r, applyn a where a.app_no =r.ST_AppNo and a.courseID ='" + courseCode + "' and ST_RankCriteria='" + criteriaCode + "' and st_stream='" + Streamvalue + "'");

            if (dtFullRank.Rows.Count > 0)
            {
                int updated = 0;
                foreach (GridViewRow gRow in gridDaySlots.Rows)
                {
                    HiddenField hdnDate = (HiddenField)gRow.FindControl("hdnDate");
                    Label lblDate = (Label)gRow.FindControl("lblDate");
                    HiddenField hdnSlotPk = (HiddenField)gRow.FindControl("hdnSlotPk");
                    Label lblSlotVal = (Label)gRow.FindControl("lblSlotVal");

                    TextBox txtFrom = (TextBox)gRow.FindControl("txtFrom");
                    TextBox txtTo = (TextBox)gRow.FindControl("txtTo");

                    if (string.IsNullOrEmpty(txtFrom.Text) || string.IsNullOrEmpty(txtTo.Text))
                    {
                        continue;
                    }
                    else
                    {
                        dtFullRank.DefaultView.RowFilter = "ST_Rank>='" + txtFrom.Text + "' and ST_Rank <='" + txtTo.Text + "'";
                        DataTable dtCurRank = dtFullRank.DefaultView.ToTable("ST_AppNo");
                        if (dtCurRank.Rows.Count > 0)
                        {
                            List<Int64> lstAppNo = dtCurRank.AsEnumerable()
                                .Select(r => r.Field<Int64>("ST_AppNo"))
                                .ToList<Int64>();

                            string app_nos = string.Join(",", lstAppNo);

                            //Update Student slot and send sms
                            //string updQ = "UPDATE applyn SET enrollment_card_date='" + hdnDate.Value + "', enrollment_session='" + lblSlotVal.Text + "', enrollmentcard='1' where app_no in (" + app_nos + ")";
                            string updQ = "UPDATE applyn SET enrollmentcard='1' where app_no in (" + app_nos + ")";
                            int updStud = dirAcc.updateData(updQ);
                            if (updStud > 0)
                            {
                                // SendSms(UserCode, collegeCode, lstAppNo, string.Empty);
                                //Insert and Update
                                string insQ = " IF EXISTS (SELECT ST_RankListPk FROM ST_RankListSlot WHERE ST_DaySlotFk = '" + hdnSlotPk.Value + "' and streamCode='" + Convert.ToString(ddlStream.SelectedValue) + "' and criteriaCode='" + criteriaCode + "')  UPDATE ST_RankListSlot SET RankFrom='" + txtFrom.Text + "',RankTo='" + txtTo.Text + "' WHERE ST_DaySlotFk = '" + hdnSlotPk.Value + "' and streamCode='" + Convert.ToString(ddlStream.SelectedValue) + "' and criteriaCode='" + criteriaCode + "'  ELSE INSERT INTO ST_RankListSlot(ST_DaySlotFk,RankFrom,RankTo,streamCode,criteriaCode) VALUES ('" + hdnSlotPk.Value + "','" + txtFrom.Text + "','" + txtTo.Text + "','" + Convert.ToString(ddlStream.SelectedValue) + "','" + criteriaCode + "') ";
                                updated += d2.update_method_wo_parameter(insQ, "Text");

                                string rankListFk = dirAcc.selectScalarString("SELECT ST_RankListPk FROM ST_RankListSlot WHERE ST_DaySlotFk = '" + hdnSlotPk.Value + "' and streamCode='" + Convert.ToString(ddlStream.SelectedValue) + "' and criteriaCode='" + criteriaCode + "'");

                                foreach (Int64 appNo in lstAppNo)
                                {
                                    string insUpd = " if not exists (select ST_sessionpk from ST_StudentSession where ST_RankListFk ='" + rankListFk + "' and ST_App_No='" + appNo + "') insert into ST_StudentSession (ST_RankListFk,ST_App_No) values ('" + rankListFk + "','" + appNo + "') ";

                                    dirAcc.updateData(insUpd);
                                }
                            }
                        }
                    }
                }
                if (updated > 0)
                    lbl_alert.Text = "Saved successfully";
                else
                    lbl_alert.Text = updated + " slot(s) saved";
            }
            else
            {
                lbl_alert.Text = "Please generate rank list";
            }
            imgdiv2.Visible = true;
        }
        catch
        {
            lbl_alert.Text = "Please try later";
            imgdiv2.Visible = true;
        }
    }
    //Alert Close
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    //Send Sms for slot allocation
    private void SendSms(string userCode, string collegeCode, List<Int64> lstAppNo, string message)
    {
        try
        {
            string user_id = string.Empty;
            string SenderID = string.Empty;
            string Password = string.Empty;

            string ssr = "select * from Track_Value where college_code='" + collegeCode + "'";
            DataSet ds = new DataSet();
            ds = d2.select_method_wo_parameter(ssr, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]).Trim();
            }

            if (user_id != string.Empty)
            {
                string getval = d2.GetUserapi(user_id);
                string[] spret = getval.Split('-');
                if (spret.GetUpperBound(0) == 1)
                {

                    SenderID = spret[0].ToString();
                    Password = spret[0].ToString();

                }
                foreach (Int64 app in lstAppNo)
                {
                    string appformNo = dirAcc.selectScalarString("SELECT app_formno FROM applyn where app_no='" + app + "'");
                    string Mobile_no = dirAcc.selectScalarString("SELECT Student_Mobile FROM applyn where app_no='" + app + "'");
                    string Msg = "Your slot for SASTRA University admission counselling has been alloted. ";

                    d2.send_sms(user_id.Trim(), collegeCode, userCode, Mobile_no, Msg, "1");
                }
            }
        }
        catch
        {
        }
    }
}
/*
 * 
 * Author : Mohamed Idhris Sheik Dawood
 * Modified:sudhagar -29-05-2017
 * Date created : 22-05-2017
 * 
 * */

using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;

public partial class DateAndTimeSlotSettings : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string UserCode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Dictionary<string, int> dtSlotDet = new Dictionary<string, int>();
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
            bindSlots();

            txt_fromSlotSet.Attributes.Add("read-only", "read-only");
            txt_fromSlotSet.Text = DateTime.Now.ToString("dd/MM/yyyy"); ;

            txt_toSlotSet.Attributes.Add("read-only", "read-only");
            txt_toSlotSet.Text = DateTime.Now.ToString("dd/MM/yyyy"); ;

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
    private void bindSlots()
    {
        try
        {
            cb_slot.Checked = true;
            cbl_slot.Items.Clear();

            string selectQ = "select LinkValue from New_InsSettings where LinkName='AdmissionSlotSettings'  and college_code ='" + ddlCollege.SelectedItem.Value + "'";
            string result = d2.GetFunction(selectQ).Trim();
            if (!string.IsNullOrEmpty(result) || result != "0")
            {
                string[] fromtoCombo = result.Split(',');
                foreach (string fromto in fromtoCombo)
                {
                    cbl_slot.Items.Add(fromto);
                }
            }
            CallCheckBoxChangedEvent(cbl_slot, cb_slot, txt_Slot, "Slot");
        }
        catch { }
    }
    //Base screen controls events
    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindBatch();
        bindEdulevel();
        bindCourse();
        bindSlots();
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEdulevel();
        bindCourse();
        bindSlots();
    }
    protected void ddlEdulevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindCourse();
        bindSlots();
    }
    protected void ddlcourse_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindSlots();
    }
    protected void cb_slot_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_slot, cb_slot, txt_Slot, "Slot");
    }
    protected void cbl_slot_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_slot, cb_slot, txt_Slot, "Slot");
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
            btnDaySlotDelete.Visible = false;
            DataTable dtDaySlot = new DataTable();
            dtDaySlot.Columns.Add("Date");
            dtDaySlot.Columns.Add("Slot");

            string[] fromdate = txt_fromSlotSet.Text.Split('/');
            string[] todate = txt_toSlotSet.Text.Split('/');

            DateTime dtFrom = Convert.ToDateTime(fromdate[1] + "/" + fromdate[0] + "/" + fromdate[2]);
            DateTime dtTo = Convert.ToDateTime(todate[1] + "/" + todate[0] + "/" + todate[2]);
            dtSlotDet = getSlotDetails(dtFrom, dtTo);//get exist slot details
            while (dtFrom <= dtTo)
            {
                for (int slI = 0; slI < cbl_slot.Items.Count; slI++)
                {
                    if (cbl_slot.Items[slI].Selected)
                    {
                        DataRow dr = dtDaySlot.NewRow();
                        dr["Date"] = dtFrom.ToString("dd/MM/yyyy");
                        dr["Slot"] = cbl_slot.Items[slI].Value;
                        dtDaySlot.Rows.Add(dr);
                    }
                }
                dtFrom = dtFrom.AddDays(1);
            }
            if (dtDaySlot.Rows.Count > 0)
            {
                gridDaySlots.Visible = true;
                gridDaySlots.DataSource = dtDaySlot;
                gridDaySlots.DataBind();

                btnDaySlotSave.Visible = true;
            }
            else
            {
                lbl_alert.Text = "No slots created";
                imgdiv2.Visible = true;
            }
        }
        catch { }
    }
    protected Dictionary<string, int> getSlotDetails(DateTime dtFrom, DateTime dtTo)
    {
        Dictionary<string, int> dtSlotDet = new Dictionary<string, int>();
        try
        {
            string collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            string batchYear = Convert.ToString(ddlbatch.SelectedValue);
            string eduLevel = Convert.ToString(ddlEduLev.SelectedValue);
            string courseCode = Convert.ToString(ddlcourse.SelectedValue);
            string selSlotQ = " SELECT distinct (convert(varchar(10),slotdate,103)+'-'+slottime) as slot, ST_DaySlotPK FROM ST_DaySlot WHERE CollegeCode='" + collegeCode + "' AND BatchYear='" + batchYear + "' AND CourseID='" + courseCode + "' AND EduLevel='" + eduLevel + "' AND SlotDate between'" + dtFrom + "' AND '" + dtTo + "'";
            DataSet dsval = d2.select_method_wo_parameter(selSlotQ, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int slrow = 0; slrow < dsval.Tables[0].Rows.Count; slrow++)
                {
                    string slotDate = Convert.ToString(dsval.Tables[0].Rows[slrow]["slot"]);
                    string slotPK = Convert.ToString(dsval.Tables[0].Rows[slrow]["ST_DaySlotPK"]);
                    if (!dtSlotDet.ContainsKey(slotDate))
                        dtSlotDet.Add(slotDate, Convert.ToInt32(slotPK));
                }
            }
        }
        catch { }
        return dtSlotDet;
    }
    protected void gridDaySlots_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbldt = (Label)e.Row.Cells[2].FindControl("lblDate");
            Label lbltm = (Label)e.Row.Cells[3].FindControl("lblSlotVal");
            string slotDet = Convert.ToString(lbldt.Text + "-" + lbltm.Text);
            if (dtSlotDet.ContainsKey(slotDet))
            {
                CheckBox cb = (CheckBox)e.Row.Cells[1].FindControl("chkSel");
                cb.Checked = true;
            }
        }
    }
    protected void gridDaySlots_DataBound(object sender, EventArgs e)
    {
        try
        {
            #region span
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
            #endregion

            #region select all
            int CheckCount = 0;
            foreach (GridViewRow gRow in gridDaySlots.Rows)
            {
                CheckBox chkSel = (CheckBox)gRow.FindControl("chkSel");
                if (chkSel.Checked)
                    CheckCount++;
            }
            CheckBox chkSelAll = (CheckBox)gridDaySlots.HeaderRow.FindControl("cb_selectHead");
            if (CheckCount == gridDaySlots.Rows.Count)
                chkSelAll.Checked = true;
            else
                chkSelAll.Checked = false;
            #endregion
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
            ArrayList arSlotDate = new ArrayList();
            foreach (GridViewRow gRow in gridDaySlots.Rows)
            {
                CheckBox chkSel = (CheckBox)gRow.FindControl("chkSel");
                if (chkSel.Checked)
                {
                    Label lblDate = (Label)gRow.FindControl("lblDate");
                    Label lblSlotVal = (Label)gRow.FindControl("lblSlotVal");

                    string[] dateSplit = lblDate.Text.Split('/');
                    string[] slotSplit = lblSlotVal.Text.Split('-');

                    string slotDate = dateSplit[1] + "/" + dateSplit[0] + "/" + dateSplit[2];
                    string[] fromTime = slotSplit[0].Split(':');
                    string[] toTime = slotSplit[1].Split(':');

                    if (!arSlotDate.Contains(slotDate))
                    {
                        d2.update_method_wo_parameter("if not exists(select * from ST_DaySlot ds,st_ranklistslot rls where ds.st_dayslotpk=rls.st_dayslotfk and ds.slotdate='" + slotDate + "')delete from st_dayslot where slotdate='" + slotDate + "'", "Text");
                        arSlotDate.Add(slotDate);
                    }
                    string insQ = "IF NOT EXISTS (SELECT ST_DaySlotPK FROM ST_DaySlot WHERE CollegeCode='" + collegeCode + "' AND BatchYear='" + batchYear + "' AND CourseID='" + courseCode + "' AND EduLevel='" + eduLevel + "' AND SlotDate='" + slotDate + "' AND SlotTime='" + lblSlotVal.Text + "') INSERT INTO ST_DaySlot (SlotDate ,SlotTime,SlotFromTime ,SlotToTime ,CollegeCode,BatchYear ,CourseID ,EduLevel ) VALUES ('" + slotDate + "' ,'" + lblSlotVal.Text + "','" + (fromTime[0] + fromTime[1]) + "','" + (toTime[0] + toTime[1]) + "','" + collegeCode + "','" + batchYear + "','" + courseCode + "' ,'" + eduLevel + "')";
                    d2.update_method_wo_parameter(insQ, "Text");
                }
            }

            lbl_alert.Text = "Saved successfully";
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
    //Slot Settings
    protected void btnAddSlot_Click(object sender, EventArgs e)
    {
        divSlotSet.Visible = true;
        gridSlots.DataSource = null;
        gridSlots.DataBind();
        txtNoOfSlot.Text = string.Empty;
        btnSaveSlot.Visible = false;
    }
    protected void closeSlotSet(object sender, EventArgs e)
    {
        divSlotSet.Visible = false;
        bindSlots();
    }
    protected void checkDate(object sender, EventArgs e)
    {
        try
        {
            DateTime fromdate = Convert.ToDateTime(txt_fromSlotSet.Text.Split('/')[1] + "/" + txt_fromSlotSet.Text.Split('/')[0] + "/" + txt_fromSlotSet.Text.Split('/')[2]);
            DateTime todate = Convert.ToDateTime(txt_toSlotSet.Text.Split('/')[1] + "/" + txt_toSlotSet.Text.Split('/')[0] + "/" + txt_toSlotSet.Text.Split('/')[2]);

            if (fromdate <= todate)
            {
            }
            else
            {
                txt_fromSlotSet.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_toSlotSet.Text = DateTime.Now.ToString("dd/MM/yyyy");
                imgdiv2.Visible = true;
                lbl_alert.Text = "From Date Should Not Exceed To Date";
            }
        }
        catch { }
    }
    protected void btnNoOfSlot_Click(object sender, EventArgs e)
    {
        try
        {
            byte slotNo = Convert.ToByte(txtNoOfSlot.Text);
            DataTable dtSlot = new DataTable();
            for (int slI = 0; slI < slotNo; slI++)
            {
                DataRow drN = dtSlot.NewRow();
                dtSlot.Rows.Add(drN);
            }

            gridSlots.Visible = true;
            gridSlots.DataSource = dtSlot;
            gridSlots.DataBind();
            btnSaveSlot.Visible = true;

        }
        catch { }
    }
    //Slot Settings save
    protected void btnSaveSlot_Click(object sender, EventArgs e)
    {
        try
        {
            StringBuilder sbSaveVal = new StringBuilder();
            foreach (GridViewRow gRow in gridSlots.Rows)
            {
                DropDownList ddlFromHour = (DropDownList)gRow.FindControl("ddlSlotFromHrs");
                DropDownList ddlFromMins = (DropDownList)gRow.FindControl("ddlSlotFromMin");
                DropDownList ddlToHour = (DropDownList)gRow.FindControl("ddlSlotToHrs");
                DropDownList ddlToMins = (DropDownList)gRow.FindControl("ddlSlotToMin");

                string fromHr = ddlFromHour.SelectedValue + ":" + ddlFromMins.SelectedValue;
                string toHr = ddlToHour.SelectedValue + ":" + ddlToMins.SelectedValue;

                sbSaveVal.Append(fromHr + "-" + toHr + ",");

            }
            if (sbSaveVal.Length > 0)
            {
                sbSaveVal.Remove(sbSaveVal.Length - 1, 1);
            }

            //Save and Update queries
            string saveValcopy = "if exists (select LinkValue from New_InsSettings where LinkName='AdmissionSlotSettings'  and college_code ='" + ddlCollege.SelectedItem.Value + "' ) update New_InsSettings set LinkValue ='" + sbSaveVal.ToString() + "' where LinkName='AdmissionSlotSettings' and college_code ='" + ddlCollege.SelectedItem.Value + "' else insert into New_InsSettings(LinkName,LinkValue,college_code) values ('AdmissionSlotSettings','" + sbSaveVal.ToString() + "','" + ddlCollege.SelectedItem.Value + "')";
            d2.update_method_wo_parameter(saveValcopy, "text");

            lbl_alert.Text = "Saved Successfully";
            imgdiv2.Visible = true;

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

    //base slot show details 
    protected void btnDaySlotShow_OnClick(object sender, EventArgs e)
    {
        getSlotShowDet();
    }
    protected void getSlotShowDet()
    {
        try
        {
            gridDaySlots.Visible = false;
            gridDaySlots.DataSource = null;
            gridDaySlots.DataBind();

            btnDaySlotSave.Visible = false;
            btnDaySlotDelete.Visible = false;
            string collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            string batchYear = Convert.ToString(ddlbatch.SelectedValue);
            string eduLevel = Convert.ToString(ddlEduLev.SelectedValue);
            string courseCode = Convert.ToString(ddlcourse.SelectedValue);
            string[] fromdate = txt_fromSlotSet.Text.Split('/');
            string[] todate = txt_toSlotSet.Text.Split('/');

            DateTime dtFrom = Convert.ToDateTime(fromdate[1] + "/" + fromdate[0] + "/" + fromdate[2]);
            DateTime dtTo = Convert.ToDateTime(todate[1] + "/" + todate[0] + "/" + todate[2]);
            DataTable dtDaySlot = new DataTable();
            dtDaySlot.Columns.Add("Date");
            dtDaySlot.Columns.Add("Slot");

            string selSlotQ = " SELECT distinct convert(varchar(10),slotdate,103) as slotdate,slottime, ST_DaySlotPK FROM ST_DaySlot WHERE CollegeCode='" + collegeCode + "' AND BatchYear='" + batchYear + "' AND CourseID='" + courseCode + "' AND EduLevel='" + eduLevel + "' AND SlotDate between'" + dtFrom + "' AND '" + dtTo + "'";
            DataSet dsval = d2.select_method_wo_parameter(selSlotQ, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int slrow = 0; slrow < dsval.Tables[0].Rows.Count; slrow++)
                {
                    string slotDate = Convert.ToString(dsval.Tables[0].Rows[slrow]["slotdate"]);
                    string slotTime = Convert.ToString(dsval.Tables[0].Rows[slrow]["slottime"]);
                    DataRow dr = dtDaySlot.NewRow();
                    dr["Date"] = slotDate;
                    dr["Slot"] = slotTime;
                    dtDaySlot.Rows.Add(dr);
                }
            }
            if (dtDaySlot.Rows.Count > 0)
            {
                gridDaySlots.Visible = true;
                gridDaySlots.DataSource = dtDaySlot;
                gridDaySlots.DataBind();
                btnDaySlotDelete.Visible = true;
            }
            else
            {
                lbl_alert.Text = "No slots created";
                imgdiv2.Visible = true;
            }
        }
        catch { }
    }
    protected void btnDaySlotDelete_OnClick(object sender, EventArgs e)
    {
        int delVal = 0;
        bool boolsave = false;
        foreach (GridViewRow gRow in gridDaySlots.Rows)
        {
            CheckBox chkSel = (CheckBox)gRow.FindControl("chkSel");
            if (chkSel.Checked)
            {
                Label lblDate = (Label)gRow.FindControl("lblDate");
                Label lblSlotVal = (Label)gRow.FindControl("lblSlotVal");

                string[] dateSplit = lblDate.Text.Split('/');
                string[] slotSplit = lblSlotVal.Text.Split('-');

                string slotDate = dateSplit[1] + "/" + dateSplit[0] + "/" + dateSplit[2];
                string[] fromTime = slotSplit[0].Split(':');
                string[] toTime = slotSplit[1].Split(':');
                delVal = d2.update_method_wo_parameter("if not exists(select * from ST_DaySlot ds,st_ranklistslot rls where ds.st_dayslotpk=rls.st_dayslotfk and ds.slotdate='" + slotDate + "' and ds.slottime='" + lblSlotVal.Text + "') delete from st_dayslot where slotdate='" + slotDate + "' and slottime='" + lblSlotVal.Text + "'", "Text");
                if (delVal > 0)
                    boolsave = true;
            }
        }
        if (boolsave)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
            getSlotShowDet();
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Already Alloted!')", true);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class AdmissionMod_StudentExtentedTime : System.Web.UI.Page
{

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
            bindDate();
            bindSlots();

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
            ds.Clear();
            ddlbindSlot.Items.Clear();
            string selectQ = "  select distinct slotTime from ST_DaySlot where SlotDate='" + ddlShowDate.SelectedItem.Value + "' and collegeCode='" + ddlCollege.SelectedValue + "' and batchyear='" + ddlbatch.SelectedItem.Text + "' and CourseId='" + ddlcourse.SelectedValue + "' and EduLevel='" + ddlEduLev.SelectedItem.Text + "'";
            ds = d2.select_method_wo_parameter(selectQ, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbindSlot.DataSource = ds;
                ddlbindSlot.DataTextField = "slotTime";
                ddlbindSlot.DataValueField = "slotTime";
                ddlbindSlot.DataBind();
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


    //Base screen search
    protected void btnBaseGo_OnClick(object sender, EventArgs e)
    {
        try
        {
            byte slotNo = Convert.ToByte(1);
            DataTable dtSlot = new DataTable();
            for (int slI = 0; slI < slotNo; slI++)
            {
                DataRow drN = dtSlot.NewRow();
                dtSlot.Rows.Add(drN);
            }

            gridSlots.Visible = true;
            gridSlots.DataSource = dtSlot;
            gridSlots.DataBind();
            btnDaySlotSave.Visible = true;
        }
        catch { }
    }

    protected void gridSlots_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string[] getDdlValue = Convert.ToString(ddlbindSlot.SelectedItem.Text).Split('-');
            if (getDdlValue.Length > 1)
            {
                (e.Row.Cells[0].FindControl("ddlSlotFromHrs") as DropDownList).Text = Convert.ToString(getDdlValue[0]).Split(':')[0];
                (e.Row.Cells[0].FindControl("ddlSlotFromMin") as DropDownList).Text = Convert.ToString(getDdlValue[0]).Split(':')[1];
                (e.Row.Cells[0].FindControl("ddlSlotToHrs") as DropDownList).Text = Convert.ToString(getDdlValue[1]).Split(':')[0];
                (e.Row.Cells[0].FindControl("ddlSlotToMin") as DropDownList).Text = Convert.ToString(getDdlValue[1]).Split(':')[1];
            }
        }
    }

    //Alert Close
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    //Slot Settings

    private void bindDate()
    {
        try
        {
            ddlShowDate.Items.Clear();
            ds.Clear();
            ds = d2.select_method_wo_parameter("select distinct CONVERT(varchar(10),SlotDate,103) as SlotDate,CONVERT(varchar(10),SlotDate,101) as SlotDateVal from st_dayslot where collegecode='" + ddlCollege.SelectedValue + "' and batchyear='" + ddlbatch.SelectedValue + "' and CourseId='" + ddlcourse.SelectedValue + "' and EduLevel='" + ddlEduLev.SelectedValue + "' order by SlotDate ", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlShowDate.DataSource = ds;
                ddlShowDate.DataTextField = "SlotDate";
                ddlShowDate.DataValueField = "SlotDateVal";
                ddlShowDate.DataBind();
            }
            //ddlShowDate.Items.Insert(0, "All");
        }
        catch
        {

        }
    }
    //Slot Settings save
    protected void btnSaveSlot_Click(object sender, EventArgs e)
    {
        try
        {
            string collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            string batchYear = Convert.ToString(ddlbatch.SelectedValue);
            string eduLevel = Convert.ToString(ddlEduLev.SelectedValue);
            string courseCode = Convert.ToString(ddlcourse.SelectedValue);
            string slotDate = Convert.ToString(ddlShowDate.SelectedItem.Value);
            string SlotSession = Convert.ToString(ddlbindSlot.SelectedItem.Text);
            StringBuilder sbSaveVal = new StringBuilder();
            string fromHr = string.Empty;
            string toHr = string.Empty;
            foreach (GridViewRow gRow in gridSlots.Rows)
            {
                DropDownList ddlFromHour = (DropDownList)gRow.FindControl("ddlSlotFromHrs");
                DropDownList ddlFromMins = (DropDownList)gRow.FindControl("ddlSlotFromMin");
                DropDownList ddlToHour = (DropDownList)gRow.FindControl("ddlSlotToHrs");
                DropDownList ddlToMins = (DropDownList)gRow.FindControl("ddlSlotToMin");

                fromHr = ddlFromHour.SelectedValue + ":" + ddlFromMins.SelectedValue;
                toHr = ddlToHour.SelectedValue + ":" + ddlToMins.SelectedValue;

                sbSaveVal.Append(fromHr + "-" + toHr + ",");

            }
            if (sbSaveVal.Length > 0)
            {
                sbSaveVal.Remove(sbSaveVal.Length - 1, 1);
            }
            string[] fromTime = fromHr.Split(':');
            string[] toTime = toHr.Split(':');

            string insQ = "IF EXISTS (SELECT ST_DaySlotPK FROM ST_DaySlot WHERE CollegeCode='" + collegeCode + "' AND BatchYear='" + batchYear + "' AND CourseID='" + courseCode + "' AND EduLevel='" + eduLevel + "' AND SlotDate='" + slotDate + "' AND SlotTime='" + SlotSession + "') update ST_DaySlot set SlotTime='" + sbSaveVal + "',SlotFromTime='" + (fromTime[0] + fromTime[1]) + "',SlotToTime='" + (toTime[0] + toTime[1]) + "' WHERE CollegeCode='" + collegeCode + "' AND BatchYear='" + batchYear + "' AND CourseID='" + courseCode + "' AND EduLevel='" + eduLevel + "' AND SlotDate='" + slotDate + "' AND SlotTime='" + SlotSession + "' ";
            //insQ += "    update applyn set enrollment_session='" + sbSaveVal + "' where enrollmentcard='1' and enrollment_Card_date='" + slotDate + "' and enrollment_session='" + SlotSession + "'";
            d2.update_method_wo_parameter(insQ, "Text");

            lbl_alert.Text = "Saved successfully";
            imgdiv2.Visible = true;

        }
        catch
        {
            lbl_alert.Text = "Please try later";
            imgdiv2.Visible = true;
        }
    }

    protected void ddlShowDate_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {

        }
    }

    protected void ddlShowDate_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            bindSlots();
        }
        catch
        {
        }
    }
}
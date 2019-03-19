/*
 * 
 * Author : Mohamed Idhris Sheik Dawood
 * Date created : 05-06-2017
 * 
 * */

using System;
using System.Data;
using InsproDataAccess;

public partial class TTSelectionSettings : System.Web.UI.Page
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
            txtLogDate.Attributes.Add("readonly", "readonly");
            txtLogDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            txtLogDateTo.Attributes.Add("readonly", "readonly");
            txtLogDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

            txtTTSelectFrom.Attributes.Add("readonly", "readonly");
            txtTTSelectFrom.Text = DateTime.Now.ToString("dd/MM/yyyy");

            txtTTSelectTo.Attributes.Add("readonly", "readonly");
            txtTTSelectTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

            txtETSelectFrom.Attributes.Add("readonly", "readonly");
            txtETSelectFrom.Text = DateTime.Now.ToString("dd/MM/yyyy");

            txtETSelectTo.Attributes.Add("readonly", "readonly");
            txtETSelectTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

            bindCollege();
            bindBatch();
            bindEdulevel();
            bindCourse();
            bindBranch();
            //bindStream();

            loadValues();
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
    public void bindBranch()
    {
        try
        {
            ddlBranch.Items.Clear();
            DataSet dsBran = d2.select_method_wo_parameter("select d.Degree_Code,dt.dept_name from Degree d, Department dt,course c where dt.Dept_Code=d.Dept_Code and c.Course_Id=d.Course_Id and d.college_code='" + ddlCollege.SelectedValue + "' and c.Edu_Level='" + ddlEduLev.SelectedValue + "' and d.Course_Id='" + ddlcourse.SelectedValue + "'  order by Dept_Name asc ", "Text");
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
    //Base screen controls events
    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindBatch();
        bindEdulevel();
        bindCourse();
        //bindStream();
        bindBranch();
        loadValues();
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEdulevel();
        bindCourse();
        //bindStream();
        bindBranch();
        loadValues();
    }
    protected void ddlEdulevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindCourse();
        //bindStream();
        bindBranch();
        loadValues();
    }
    protected void ddlcourse_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        loadValues();
        //bindStream();
        bindBranch();
    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadValues();
    }
    private void loadValues()
    {
        try
        {
            ClearValues();

            string collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            string courseCode = Convert.ToString(ddlcourse.SelectedValue);
            string degreeCode = Convert.ToString(ddlBranch.SelectedValue);
            string batchYear = Convert.ToString(ddlbatch.SelectedValue);
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(courseCode) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(batchYear))
            {
                string selQ = "select convert(varchar(10),loginTimeFrom,103) as logFromDate , convert(varchar(10),loginTimeTo,103) as logToDate , convert(varchar(10),TTSelectTimeFrom,103) as ttFromDate , convert(varchar(10),TTSelectTimeTo,103) as ttToDate ,convert(varchar(10),ElectiveSelectFrom,103) as etFromDate ,convert(varchar(10),ElectiveSelectTo,103) as etToDate,convert(varchar(10),loginTimeFrom,108) as logFromTime , convert(varchar(10),loginTimeTo,108) as logToTime , convert(varchar(10),TTSelectTimeFrom,108) as ttFromTime , convert(varchar(10),TTSelectTimeTo,108) as ttToTime ,convert(varchar(10),ElectiveSelectFrom,108) as etFromTime ,convert(varchar(10),ElectiveSelectTo,108) as etToTime from TT_SelectionSettings where collegeCode='" + collegeCode + "' and courseCode='" + courseCode + "' and degreeCode='" + degreeCode + "' and batchYear='" + batchYear + "'";
                DataTable dtSaveDateTime = dirAcc.selectDataTable(selQ);
                if (dtSaveDateTime.Rows.Count > 0)
                {
                    txtLogDate.Text = Convert.ToString(dtSaveDateTime.Rows[0]["logFromDate"]);
                    txtLogDateTo.Text = Convert.ToString(dtSaveDateTime.Rows[0]["logToDate"]);
                    string[] logFromTime = Convert.ToString(dtSaveDateTime.Rows[0]["logFromTime"]).Split(':');
                    string[] logToTime = Convert.ToString(dtSaveDateTime.Rows[0]["logToTime"]).Split(':');
                    ddlLogHrs.SelectedIndex = ddlLogHrs.Items.IndexOf(ddlLogHrs.Items.FindByText(logFromTime[0]));
                    ddlLogMin.SelectedIndex = ddlLogMin.Items.IndexOf(ddlLogMin.Items.FindByText(logFromTime[1]));
                    ddlLogHrsTo.SelectedIndex = ddlLogHrsTo.Items.IndexOf(ddlLogHrsTo.Items.FindByText(logToTime[0]));
                    ddlLogMinTo.SelectedIndex = ddlLogMinTo.Items.IndexOf(ddlLogMinTo.Items.FindByText(logToTime[1]));

                    txtTTSelectFrom.Text = Convert.ToString(dtSaveDateTime.Rows[0]["ttFromDate"]);
                    txtTTSelectTo.Text = Convert.ToString(dtSaveDateTime.Rows[0]["ttToDate"]);
                    string[] ttFromTime = Convert.ToString(dtSaveDateTime.Rows[0]["ttFromTime"]).Split(':');
                    string[] ttToTime = Convert.ToString(dtSaveDateTime.Rows[0]["ttToTime"]).Split(':');
                    ddlTTSelectHr.SelectedIndex = ddlTTSelectHr.Items.IndexOf(ddlTTSelectHr.Items.FindByText(ttFromTime[0]));
                    ddlTTSelectMin.SelectedIndex = ddlTTSelectMin.Items.IndexOf(ddlTTSelectMin.Items.FindByText(ttFromTime[1]));
                    ddlTTSelectHrTo.SelectedIndex = ddlTTSelectHrTo.Items.IndexOf(ddlTTSelectHrTo.Items.FindByText(ttToTime[0]));
                    ddlTTSelectMinTo.SelectedIndex = ddlTTSelectMinTo.Items.IndexOf(ddlTTSelectMinTo.Items.FindByText(ttToTime[1]));

                    txtETSelectFrom.Text = Convert.ToString(dtSaveDateTime.Rows[0]["etFromDate"]);
                    txtETSelectTo.Text = Convert.ToString(dtSaveDateTime.Rows[0]["etToDate"]);
                    string[] etFromTime = Convert.ToString(dtSaveDateTime.Rows[0]["etFromTime"]).Split(':');
                    string[] etToTime = Convert.ToString(dtSaveDateTime.Rows[0]["etToTime"]).Split(':');
                    ddlETSelectHr.SelectedIndex = ddlETSelectHr.Items.IndexOf(ddlETSelectHr.Items.FindByText(etFromTime[0]));
                    ddlETSelectMin.SelectedIndex = ddlETSelectMin.Items.IndexOf(ddlETSelectMin.Items.FindByText(etFromTime[1]));
                    ddlETSelectHrTo.SelectedIndex = ddlETSelectHrTo.Items.IndexOf(ddlETSelectHrTo.Items.FindByText(etToTime[0]));
                    ddlETSelectMinTo.SelectedIndex = ddlETSelectMinTo.Items.IndexOf(ddlETSelectMinTo.Items.FindByText(etToTime[1]));
                }

            }
        }
        catch { }
    }
    private void ClearValues()
    {
        txtLogDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtLogDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

        txtTTSelectFrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtTTSelectTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

        txtETSelectFrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtETSelectTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

        ddlLogHrs.SelectedIndex = 0;
        ddlLogMin.SelectedIndex = 0;
        ddlLogHrsTo.SelectedIndex = 0;
        ddlLogMinTo.SelectedIndex = 0;

        ddlTTSelectHr.SelectedIndex = 0;
        ddlTTSelectMin.SelectedIndex = 0;
        ddlTTSelectHrTo.SelectedIndex = 0;
        ddlTTSelectMinTo.SelectedIndex = 0;

        ddlETSelectHr.SelectedIndex = 0;
        ddlETSelectMin.SelectedIndex = 0;
        ddlETSelectHrTo.SelectedIndex = 0;
        ddlETSelectMinTo.SelectedIndex = 0;
    }
    //Settings save
    protected void btnSaveSettings_OnClick(object sender, EventArgs e)
    {
        try
        {
            string collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            string courseCode = Convert.ToString(ddlcourse.SelectedValue);
            string degreeCode = Convert.ToString(ddlBranch.SelectedValue);
            string batchYear = Convert.ToString(ddlbatch.SelectedValue);
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(courseCode) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(batchYear))
            {

                #region Login Date and Time Check
                //From
                string[] loginDateFrom = txtLogDate.Text.Split('/');
                byte logHrs = Convert.ToByte(ddlLogHrs.SelectedValue);// 6/5/2017 11:36:28 AM
                byte logMins = Convert.ToByte(ddlLogMin.SelectedItem.Text);
                string ampmLog = (logHrs > 11) ? "PM" : "AM";
                if (logHrs > 12)
                {
                    logHrs -= 12;
                }
                else if (logHrs == 0)
                {
                    logHrs = 12;
                }

                string logFromDateTime = loginDateFrom[1] + "/" + loginDateFrom[0] + "/" + loginDateFrom[2] + " " + logHrs + ":" + logMins + ":00 " + ampmLog;
                DateTime logDateTimeFrom = Convert.ToDateTime(logFromDateTime);

                //To 
                string[] loginDateTo = txtLogDateTo.Text.Split('/');
                byte logHrsTo = Convert.ToByte(ddlLogHrsTo.SelectedValue);// 6/5/2017 11:36:28 AM
                byte logMinsTo = Convert.ToByte(ddlLogMinTo.SelectedItem.Text);
                string ampmLogTo = (logHrsTo > 11) ? "PM" : "AM";
                if (logHrsTo > 12)
                {
                    logHrsTo -= 12;
                }
                else if (logHrsTo == 0)
                {
                    logHrsTo = 12;
                }

                string logToDateTime = loginDateTo[1] + "/" + loginDateTo[0] + "/" + loginDateTo[2] + " " + logHrsTo + ":" + logMinsTo + ":00 " + ampmLogTo;
                DateTime logDateTimeTo = Convert.ToDateTime(logToDateTime);

                if (logDateTimeFrom > logDateTimeTo)
                {
                    lbl_alert.Text = "Please check login date and time";
                    imgdiv2.Visible = true;
                    return;
                }

                #endregion

                #region Time Table Selection Date and Time Check
                //From
                string[] TTSelectDateFrom = txtTTSelectFrom.Text.Split('/');

                byte ttHrs = Convert.ToByte(ddlTTSelectHr.SelectedValue);
                byte ttMins = Convert.ToByte(ddlTTSelectMin.SelectedItem.Text);
                string ampmTT = (ttHrs > 11) ? "PM" : "AM";
                if (ttHrs > 12)
                {
                    ttHrs -= 12;
                }
                else if (ttHrs == 0)
                {
                    ttHrs = 12;
                }

                string ttFromDateTime = TTSelectDateFrom[1] + "/" + TTSelectDateFrom[0] + "/" + TTSelectDateFrom[2] + " " + ttHrs + ":" + ttMins + ":00 " + ampmTT;
                DateTime ttDateTimeFrom = Convert.ToDateTime(ttFromDateTime);

                //To 
                string[] TTSelectDateTo = txtTTSelectTo.Text.Split('/');
                byte ttHrsTo = Convert.ToByte(ddlTTSelectHrTo.SelectedValue);
                byte ttMinsTo = Convert.ToByte(ddlTTSelectMinTo.SelectedItem.Text);
                string ampmTTTo = (ttHrsTo > 11) ? "PM" : "AM";
                if (ttHrsTo > 12)
                {
                    ttHrsTo -= 12;
                }
                else if (ttHrsTo == 0)
                {
                    ttHrsTo = 12;
                }

                string ttToDateTime = TTSelectDateTo[1] + "/" + TTSelectDateTo[0] + "/" + TTSelectDateTo[2] + " " + ttHrsTo + ":" + ttMinsTo + ":00 " + ampmTTTo;
                DateTime ttDateTimeTo = Convert.ToDateTime(ttToDateTime);

                if (ttDateTimeFrom > ttDateTimeTo)
                {
                    lbl_alert.Text = "Please check Time table selection date and time";
                    imgdiv2.Visible = true;
                    return;
                }

                #endregion

                #region Elective Selection Date and Time Check
                //From
                string[] ETSelectDateFrom = txtETSelectFrom.Text.Split('/');

                byte etHrs = Convert.ToByte(ddlETSelectHr.SelectedValue);
                byte etMins = Convert.ToByte(ddlETSelectMin.SelectedItem.Text);
                string ampmET = (etHrs > 11) ? "PM" : "AM";
                if (etHrs > 12)
                {
                    etHrs -= 12;
                }
                else if (etHrs == 0)
                {
                    etHrs = 12;
                }

                string etFromDateTime = ETSelectDateFrom[1] + "/" + ETSelectDateFrom[0] + "/" + ETSelectDateFrom[2] + " " + etHrs + ":" + etMins + ":00 " + ampmET;
                DateTime etDateTimeFrom = Convert.ToDateTime(etFromDateTime);

                //To 
                string[] ETSelectDateTo = txtETSelectTo.Text.Split('/');
                byte etHrsTo = Convert.ToByte(ddlETSelectHrTo.SelectedValue);
                byte etMinsTo = Convert.ToByte(ddlETSelectMinTo.SelectedItem.Text);
                string ampmETTo = (etHrsTo > 11) ? "PM" : "AM";
                if (etHrsTo > 12)
                {
                    etHrsTo -= 12;
                }
                else if (etHrsTo == 0)
                {
                    etHrsTo = 12;
                }

                string etToDateTime = ETSelectDateTo[1] + "/" + ETSelectDateTo[0] + "/" + ETSelectDateTo[2] + " " + etHrsTo + ":" + etMinsTo + ":00 " + ampmETTo;
                DateTime etDateTimeTo = Convert.ToDateTime(etToDateTime);

                if (etDateTimeFrom > etDateTimeTo)
                {
                    lbl_alert.Text = "Please check Elective selection date and time";
                    imgdiv2.Visible = true;
                    return;
                }

                #endregion


                string insUpdateQ = "if exists(select TT_setpk from TT_SelectionSettings where collegeCode='" + collegeCode + "' and courseCode='" + courseCode + "' and degreeCode='" + degreeCode + "' and batchYear='" + batchYear + "') update TT_SelectionSettings set loginTimeFrom ='" + logDateTimeFrom + "', loginTimeTo ='" + logDateTimeTo + "', TTSelectTimeFrom ='" + ttDateTimeFrom + "', TTSelectTimeTo ='" + ttDateTimeTo + "',ElectiveSelectFrom ='" + etDateTimeFrom + "',ElectiveSelectTo ='" + etDateTimeTo + "' where  collegeCode='" + collegeCode + "' and courseCode='" + courseCode + "' and degreeCode='" + degreeCode + "' and batchYear='" + batchYear + "' else insert into TT_SelectionSettings (collegeCode  ,courseCode ,degreeCode , batchYear, loginTimeFrom , loginTimeTo , TTSelectTimeFrom , TTSelectTimeTo ,ElectiveSelectFrom ,ElectiveSelectTo ) values ('" + collegeCode + "'  ,'" + courseCode + "' ,'" + degreeCode + "' , '" + batchYear + "', '" + logDateTimeFrom + "' , '" + logDateTimeTo + "' , '" + ttDateTimeFrom + "' , '" + ttDateTimeTo + "' ,'" + etDateTimeFrom + "' ,'" + etDateTimeTo + "')";

                dirAcc.updateData(insUpdateQ);

                lbl_alert.Text = "Saved successfully";
            }
            else
            {
                lbl_alert.Text = "Please check inputs";
            }
        }
        catch { lbl_alert.Text = "Please try later"; }
        imgdiv2.Visible = true;
    }

    //Alert Close
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
}
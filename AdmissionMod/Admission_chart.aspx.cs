using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
public partial class AdmissionMod_Admission_chart : System.Web.UI.Page
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
            bindstream();
            bindsession();
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            admission_chart.Visible = false;
        }
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
    public void bindstream()
    {
        try
        {
            if (ddlCollege.Items.Count > 0)
            {
                ds.Clear();
                ds = d2.select_method_wo_parameter(" select textval,TextCode from textvaltable where TextCriteria='admst' and college_code='" + ddlCollege.SelectedValue + "' order by textval", "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_stream.DataSource = ds;
                    ddl_stream.DataTextField = "textval";
                    ddl_stream.DataValueField = "TextCode";
                    ddl_stream.DataBind();
                    ddl_stream.Items.Insert(0, "All");
                }
            }
        }
        catch
        {
        }
    }
    public void bindsession()
    {
        try
        {
            if (ddlCollege.Items.Count > 0 && ddlbatch.Items.Count > 0 && ddlEduLev.Items.Count > 0 && ddlcourse.Items.Count > 0)
            {
                ds.Clear();
                ds = d2.select_method_wo_parameter(" select distinct SlotTime from ST_DaySlot where CollegeCode='" + Convert.ToString(ddlCollege.SelectedValue) + "' and BatchYear='" + Convert.ToString(ddlbatch.SelectedValue) + "' and CourseID='" + Convert.ToString(ddlcourse.SelectedValue) + "' and EduLevel='" + Convert.ToString(ddlEduLev.SelectedValue) + "'", "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_session.DataSource = ds;
                    ddl_session.DataTextField = "SlotTime";
                    ddl_session.DataValueField = "SlotTime";
                    ddl_session.DataBind();
                    ddl_session.Items.Insert(0, "All");
                }
            }
        }
        catch
        {
        }
    }
    protected void ddlEduLev_selectedindexchanged(object sender, EventArgs e)
    {
        bindCourse(); bindsession();
    }
    protected void ddlcourse_selectedindexchanged(object sender, EventArgs e)
    {
        bindsession();
    }
    protected void ddlbatch_selectedindexchanged(object sender, EventArgs e)
    {
        bindCourse(); bindsession();
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            admission_chart.Visible = false;
            if (ddl_session.Items.Count > 0 && ddlCollege.Items.Count > 0 && ddlcourse.Items.Count > 0 && ddlEduLev.Items.Count > 0 && ddlbatch.Items.Count > 0)
            {
                DataSet chart_ds = new DataSet();
                string fromdate = getdatetimethrow(txt_fromdate.Text);
                string todate = getdatetimethrow(txt_todate.Text);
                string Session = "";
                if (ddl_session.SelectedItem.Text.ToUpper() != "ALL")
                    Session = " and s.slottime='" + ddl_session.SelectedValue + "'";
                string chartquery = " select count(a.quota)quotacount,(select MasterValue from CO_MasterValues where convert(varchar,MasterCode)=a.quota)quota,s.slottime+' - '+CONVERT(varchar(10),slotdate,103) slotdatetime  from Registration r,applyn a,ST_DaySlot s where a.app_no=r.App_No and  ISNULL(enrollment_card_date,'')<>'' and ISNULL(enrollmentcard,0)=1  and s.slotdate=r.Adm_Date and a.enrollment_session=s.slottime  and Adm_Date between '" + fromdate + "' and '" + todate + "' and r.college_code='" + ddlCollege.SelectedValue + "' and r.batch_year='" + ddlbatch.SelectedValue + "' and a.courseID='" + ddlcourse.SelectedValue + "' and s.edulevel='" + ddlEduLev.SelectedValue + "' " + Session + " group by a.quota,s.slottime,slotdate order by SlotDate ";
                chart_ds = d2.select_method_wo_parameter(chartquery, "text");
                int chartwidth = 20;
                if (chart_ds.Tables[0].Rows.Count > 0)
                {
                    admission_chart.Series.Clear();
                    DataTable dtcol = new DataTable();
                    DataRow dtrow;
                    dtrow = dtcol.NewRow();
                    foreach (DataRow dr in chart_ds.Tables[0].Rows)
                    {
                        dtcol.Columns.Add(Convert.ToString(dr["slotdatetime"]) + "-" + Convert.ToString(dr["quota"]));
                        admission_chart.Series.Add(Convert.ToString(dr["slotdatetime"]) + "-" + Convert.ToString(dr["quota"]));
                        dtrow = dtcol.NewRow();
                        dtrow[Convert.ToString(dr["slotdatetime"]) + "-" + Convert.ToString(dr["quota"])] = Convert.ToString(dr["quotacount"]);
                        dtcol.Rows.Add(dtrow);
                    }
                    admission_chart.RenderType = RenderType.ImageTag;
                    admission_chart.ImageType = ChartImageType.Png;
                    admission_chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
                    admission_chart.ImageLocation = Path.Combine("~/Report/", "admissionchart");
                    if (dtcol.Columns.Count > 0)
                    {
                        for (int r = 0; r < dtcol.Rows.Count; r++)
                        {
                            for (int c = 0; c < dtcol.Columns.Count; c++)
                            {
                                string col = dtcol.Columns[c].ToString();
                                string row = dtcol.Rows[r][c].ToString();
                                admission_chart.Series[r].Points.AddXY(dtcol.Columns[c].ToString(), dtcol.Rows[r][c].ToString());
                                admission_chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                admission_chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                                admission_chart.Series[r].IsValueShownAsLabel = true;
                                admission_chart.Series[r].IsXValueIndexed = true;
                                admission_chart.Series[r].ChartType = SeriesChartType.Column;
                                admission_chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                admission_chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                chartwidth += 40;
                            }
                        }
                    }
                    admission_chart.Legends[0].Enabled = true;
                    if (chartwidth < 500)
                        chartwidth = 500;
                    admission_chart.Width = chartwidth;
                    admission_chart.Visible = true;
                }
                else
                {
                    lbl_alert.Text = "No Records Founds";
                    alert_pop.Visible = true;
                    admission_chart.Visible = false;
                }
            }
            else
            {
                lbl_alert.Text = "Please Select All Fields ";
                alert_pop.Visible = true;
                admission_chart.Visible = false;
            }
        }
        catch (Exception ex)
        {
            admission_chart.Visible = false;
            lbl_alert.Text = ex.ToString();
            d2.sendErrorMail(ex, ddlCollege.SelectedValue, "Admissionchart");
            alert_pop.Visible = false;
        }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alert_pop.Visible = false;
    }
    public string getdatetimethrow(string textboxvalue)
    {
        string[] split = textboxvalue.Split('/');
        DateTime dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        textboxvalue = dt.ToString("MM/dd/yyyy");
        return textboxvalue;
    }
}
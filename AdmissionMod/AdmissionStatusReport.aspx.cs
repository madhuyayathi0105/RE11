using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public partial class AdmissionMod_AdmissionStatusReport : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ReuasableMethods Rs = new ReuasableMethods();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        try
        {
            if (!IsPostBack)
            {
                BindCollege();
                bindbatch();
                edu_level();
                degree();
                bindsem();
                BindStream();
                BindSession();
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        catch
        {

        }
    }
    void BindCollege()
    {
        try
        {
            byte userType = 0;
            string userOrGroupCode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                userOrGroupCode = Convert.ToString(Session["group_code"]).Trim();
                userType = 0;
            }
            else if (Session["usercode"] != null)
            {
                userOrGroupCode = Convert.ToString(Session["usercode"]).Trim();
                userType = 1;
            }
            ds.Clear();
            ds = d2.BindCollegebaseonrights(userOrGroupCode, userType);
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
        }
        catch
        {
        }
    }
    public void bindbatch()
    {
        try
        {
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = ds;
                ddl_batch.DataTextField = "batch_year";
                ddl_batch.DataValueField = "batch_year";
                ddl_batch.DataBind();
                degree();
            }
        }
        catch
        {
        }
    }
    public void edu_level()
    {

        string st = "select distinct edu_level from course where college_code='" + ddlcollege.SelectedItem.Value + "'";
        ds = d2.select_method_wo_parameter(st, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_graduation.DataSource = ds;
            ddl_graduation.DataTextField = "edu_level";
            ddl_graduation.DataValueField = "edu_level";
            ddl_graduation.DataBind();
        }
    }
    public void degree()
    {
        try
        {
            string query, edulvl = "";
            string typeg = "";
            if (ddl_graduation.Items.Count > 0)
            {
                edulvl = Convert.ToString(ddl_graduation.SelectedItem.Value);
            }
            string rights = "";
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
            {
                rights = "and group_code='" + group_user + "'";
            }
            else
            {
                rights = " and user_code='" + usercode + "'";
            }

            query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c ,DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddlcollege.SelectedItem.Value + "' and Edu_Level in('" + edulvl + "')  " + rights + "";

            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddl_degree.DataSource = ds;
                ddl_degree.DataTextField = "course_name";
                ddl_degree.DataValueField = "course_id";
                ddl_degree.DataBind();
                bindbranch(Convert.ToString(ddl_degree.SelectedItem.Value));
            }
            else
            {
                ddl_degree.Items.Clear();
                txt_branch.Text = "Select";
                cb_branch.Checked = false;
                cbl_branch.Items.Clear();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindbranch(string branch)
    {
        try
        {
            branch = "";
            branch = Convert.ToString(ddl_degree.SelectedItem.Value);
            string rights = "";
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
            {
                rights = "and group_code='" + group_user + "'";
            }
            else
            {
                rights = " and user_code='" + usercode + "'";
            }
            cb_branch.Checked = false;
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + " ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + "";
            }
            ds.Clear();
            cbl_branch.Items.Clear();
            ds = d2.select_method_wo_parameter(commname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_branch.DataSource = ds;
                cbl_branch.DataTextField = "dept_name";
                cbl_branch.DataValueField = "degree_code";
                cbl_branch.DataBind();
                if (cbl_branch.Items.Count > 0)
                {
                    cbl_branch.Items[0].Selected = true;
                }
                txt_branch.Text = "Branch(" + 1 + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindsem()
    {
        ddlsem.Items.Clear();
        int duration = 0;
        int i = 0;
        ds.Clear();
        string branch = "";
        string build = "";
        string batch = "";
        if (cbl_branch.Items.Count > 0)
        {
            for (i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    build = cbl_branch.Items[i].Value.ToString();
                    if (branch == "")
                    {
                        branch = build;
                    }
                    else
                    {
                        branch = branch + "," + build;
                    }
                }
            }
        }
        if (ddl_batch.Items.Count > 0)
        {
            batch = ddl_batch.SelectedItem.Value;
        }
        if (branch.Trim() != "" && batch.Trim() != "")
        {
            // ds = d2.BindSem(branch, batch, ddlcollege.SelectedItem.Value);
            string strsql1 = "select  Max(duration)  from degree where degree_code in (" + branch + ") and college_code=" + ddlcollege.SelectedItem.Value + "";
            ds = d2.select_method_wo_parameter(strsql1, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string dur = Convert.ToString(ds.Tables[0].Rows[i][0]);
                    if (dur.Trim() != "")
                    {
                        if (duration < Convert.ToInt32(dur))
                        {
                            duration = Convert.ToInt32(dur);
                        }
                    }
                }
            }
            if (duration != 0)
            {
                for (i = 1; i <= duration; i++)
                {
                    ddlsem.Items.Add(Convert.ToString(i));
                }
            }
        }
    }
    private void BindStream()
    {
        try
        {
            string qry = "select TextCode,TextVal from TextValTable tv where TextCriteria='ADMst' and college_code ='" + ddlcollege.SelectedValue + "' order by TextVal";
            ds = d2.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlStream.DataSource = ds;
                ddlStream.DataTextField = "TextVal";
                ddlStream.DataValueField = "TextCode";
                ddlStream.DataBind();
                ddlStream.Enabled = true;
                ddlStream.Items.Insert(0, "All");
                ddlStream.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {

        }
    }
    private void BindSession()
    {
        try
        {

            string qry = "select MasterCode,MasterValue  from CO_MasterValues where MasterCriteria like'%StudRankCriteria%' and collegecode ='" + ddlcollege.SelectedValue + "'";
            //select distinct SlotTime from ST_DaySlot ds where SlotTime is not null and SlotTime<>'' 
            ds = d2.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSession.DataSource = ds;
                ddlSession.DataTextField = "MasterValue";
                ddlSession.DataValueField = "MasterCode";
                ddlSession.DataBind();
                ddlSession.Enabled = true;
                ddlSession.Items.Insert(0, "All");
                ddlSession.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {

        bindbatch();
        edu_level();
        degree();
        bindsem();
        BindSession();
    }
    protected void ddl_graduation_SelectedIndexChanged(object sender, EventArgs e)
    {
        degree();
        bindsem();
    }
    protected void ddl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        edu_level();
        degree();
        bindsem();
    }
    protected void ddl_degree_Selectedindexchange(object sender, EventArgs e)
    {
        bindbranch(Convert.ToString(ddl_degree.SelectedItem.Value));
    }
    protected void cb_branch_checkedchange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_branch, cb_branch, txt_branch, "Branch");
    }
    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_branch, cb_branch, txt_branch, "Branch");
    }
    protected void ddlStream_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        div_report.Visible = false;
    }
    protected void txtDate_TextChanged(object sender, EventArgs e)
    {

    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        Gofuncation();
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

    public void Gofuncation()
    {
        try
        {
            string Branch = Rs.GetSelectedItemsValueAsString(cbl_branch);
            string batch = Convert.ToString(ddl_batch.SelectedItem.Text);
            string college = Convert.ToString(ddlcollege.SelectedValue);
            string[] fromDate = txtFromDate.Text.Split('/');
            string[] toDate = txtToDate.Text.Split('/');

            string fromDt = fromDate[1] + "/" + fromDate[0] + "/" + fromDate[2];
            string toDt = toDate[1] + "/" + toDate[0] + "/" + toDate[2];
            string Stream = string.Empty;
            string Category = string.Empty;
            if (ddlStream.SelectedItem.Text != "All")
            {
                Stream = ddlStream.SelectedValue;
            }
            if (ddlSession.SelectedItem.Text != "All")
            {
                Category = ddlSession.SelectedValue;
            }
            DataTable data = new DataTable();
            DataView Dsview = new DataView();
            DataView DvRank = new DataView();
            string Query = "select r.app_no,r.Roll_Admit,r.Stud_Name,CONVERT(varchar(10),r.Adm_Date,103) as AdmitDate,(select Mastervalue from CO_MasterValues where MasterCode=a.quota) as Categroy,a.quota,a.StreamAdmission,(select TextVal from TextValTable where TextCode= a.StreamAdmission) as Stream,r.degree_code,st.HSCMarkSec,st.CombinedScore,st.CombinedScoreSII,enrollment_card_date,(c.Course_Name +''+dt.Dept_Name) as Departmet from applyn a,Registration r,ST_Student_Mark_Detail st ,Degree d,Department dt,Course c where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.app_no =r.App_No and a.app_no =st.ST_AppNo and r.App_No =st.ST_AppNo and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.college_code ='" + college + "' and r.Batch_Year ='" + batch + "' and r.degree_code in ('" + Branch + "') and Adm_Date between '" + fromDt + "' and '" + toDt + "' ";
            if (Stream.Trim() != "")
            {
                Query += " and a.StreamAdmission ='" + Stream + "'";
            }
            if (Category.Trim() != "")
            {
                Query += " and a.quota ='" + Category + "'";
            }
            Query += "  select ST_Rank,ST_RankCriteria,ST_Stream,ST_AppNo from ST_RankTable St,Registration r where st.ST_AppNo =r.App_No  and r.degree_code in ('" + Branch + "') and r.college_code ='" + college + "' and r.Batch_Year ='" + batch + "'";
            if (Stream.Trim() != "")
            {
                Query += " and ST_Stream ='" + Stream + "'";
            }
            if (Category.Trim() != "")
            {
                Query += " and ST_RankCriteria ='" + Category + "'";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int SNo = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;


                FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 25;
                FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Admission / Application No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;

                FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Student Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;

                FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Date of Admit";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;

                FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Combined Score";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;

                FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Percentile";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;

                FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Rank";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FarPoint.Web.Spread.TextCellType db = new FarPoint.Web.Spread.TextCellType();

                data = ds.Tables[0].DefaultView.ToTable(true, "degree_code", "StreamAdmission", "quota");
                DataView Dsort = data.DefaultView;
                Dsort.Sort = "degree_code,StreamAdmission,quota";
                for (int intds = 0; intds < Dsort.Count; intds++)
                {
                    ds.Tables[0].DefaultView.RowFilter = "degree_code='" + Convert.ToString(Dsort[intds]["degree_code"]) + "' and StreamAdmission='" + Convert.ToString(Dsort[intds]["StreamAdmission"]) + "' and quota='" + Convert.ToString(Dsort[intds]["quota"]) + "'";
                    Dsview = ds.Tables[0].DefaultView;
                    string Strm = Convert.ToString(Dsview[0]["Stream"]);
                    if (Strm.Trim() != "Stream II")
                    {
                        Dsview.Sort = " CombinedScore desc";
                    }
                    else
                    {
                        Dsview.Sort = " CombinedScoreSII desc,stud_name asc";
                    }
                    if (Dsview.Count > 0)
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = (Convert.ToString(Dsview[0]["Departmet"]) + "            " + Convert.ToString(Dsview[0]["Stream"]) + "            " + Convert.ToString(Dsview[0]["Categroy"]));
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        for (int intdv = 0; intdv < Dsview.Count; intdv++)
                        {
                            SNo++;
                            string AppNo = Convert.ToString(Dsview[intdv]["app_no"]);
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(SNo);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(Dsview[intdv]["Roll_Admit"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(Dsview[intdv]["stud_name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(Dsview[intdv]["AdmitDate"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                            double comI = 0;
                            string ComIIval = Convert.ToString(Dsview[intdv]["CombinedScore"]);
                            double.TryParse(ComIIval, out comI);

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = db;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(Math.Round(comI, 4));
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                            double comII = 0;
                            string ComIIvalue = Convert.ToString(Dsview[intdv]["CombinedScoreSII"]);
                            double.TryParse(ComIIvalue, out comII);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].CellType = db;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(Math.Round(comII, 4));
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;


                            ds.Tables[1].DefaultView.RowFilter = "ST_RankCriteria='" + Convert.ToString(Dsort[intds]["quota"]) + "' and ST_Stream ='" + Convert.ToString(Dsort[intds]["StreamAdmission"]) + "' and ST_AppNo ='" + AppNo + "'";
                            DvRank = ds.Tables[1].DefaultView;
                            if (DvRank.Count > 0)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(DvRank[0]["ST_Rank"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                }
                if (FpSpread1.Sheets[0].RowCount > 0)
                {
                    FpSpread1.Visible = true;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    div_report.Visible = true;
                }
            }
            else
            {
                lblAlertMsg.Text = "No Record(s) Found";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
        }
        catch
        {

        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text.ToString().Trim();
            if (reportname != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                //lbl_err.Visible = false;
            }
            else
            {

            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        int year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
        string degreedetails = "Admission Status Report " + DateTime.Now.ToString("yyyy") + " - " + (year + 1) + "";
        string pagename = "AdmissionStatusReport.aspx";
        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
        }
        catch (Exception ex)
        {

        }
    }
}

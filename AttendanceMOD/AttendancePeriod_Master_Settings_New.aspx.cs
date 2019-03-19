using System;
using System.Linq;
using System.Web.UI.WebControls;
//using FarPoint.Web.Spread;
using System.Collections;
using System.Data;
using System.Drawing;

public partial class AttendancePeriod_Master_Settings_New : System.Web.UI.Page
{
    Hashtable hat = new Hashtable();
    string usercode = "", collegecode = "", singleuser = "", group_user = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string qry = "";
    string sptype = "Text";
    string batchyear = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        if (!IsPostBack)
        {
            bindEduLevel();
            BindBatch();
            bindDays();
            FpDeptment.Visible = false;
            btnSave.Visible = false;
            lblerrmsg.Visible = false;
            //FpDeptment.Sheets[0].RowCount = 0;
            FpDeptment.Sheets[0].AutoPostBack = false;

            FpDeptment.CommandBar.Visible = false;
            //FpDeptment.Sheets[0].SheetCorner
            FpDeptment.Sheets[0].SheetCorner.ColumnCount = 0;
            //FpDeptment.Sheets[0].SheetCorner.RowCount = 0;
            FpDeptment.Sheets[0].ColumnCount = 0;
            FpDeptment.Sheets[0].RowCount = 0;
            FpDeptment.Sheets[0].ColumnCount = 12;
            FpDeptment.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpDeptment.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpDeptment.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Batch";
            FpDeptment.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpDeptment.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Degree";
            FpDeptment.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
            FpDeptment.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Sem";
            FpDeptment.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Sec";
            FpDeptment.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total Hours Per Day";
            FpDeptment.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Total Hours Per First Half";
            FpDeptment.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Total Hours Per Second Half";
            FpDeptment.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Min. Hour Present First Half";
            FpDeptment.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Min. Hour Present Second Half";

            FpDeptment.Sheets[0].Columns[0].Width = 37;
            FpDeptment.Sheets[0].Columns[1].Width = 48;
            FpDeptment.Sheets[0].Columns[2].Width = 50;
            FpDeptment.Sheets[0].Columns[3].Width = 60;
            FpDeptment.Sheets[0].Columns[4].Width = 310;
            FpDeptment.Sheets[0].Columns[5].Width = 35;
            FpDeptment.Sheets[0].Columns[6].Width = 38;
            //FpDeptment.Sheets[0].Columns[4].Width = 72;
            FpDeptment.Sheets[0].Columns[7].Width = 80;
            FpDeptment.Sheets[0].Columns[8].Width = 80;
            FpDeptment.Sheets[0].Columns[9].Width = 80;
            FpDeptment.Sheets[0].Columns[10].Width = 80;
            FpDeptment.Sheets[0].Columns[11].Width = 80;
            //FpDeptment.Sheets[0].Columns[9].Width = 200;

            FpDeptment.Sheets[0].Columns[0].Locked = true;
            FpDeptment.Sheets[0].Columns[2].Locked = true;
            FpDeptment.Sheets[0].Columns[3].Locked = true;
            FpDeptment.Sheets[0].Columns[4].Locked = true;
            FpDeptment.Sheets[0].Columns[5].Locked = true;
            FpDeptment.Sheets[0].Columns[6].Locked = true;

            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.Black;
            // style2.BackColor = System.Drawing.Color.Teal;
            style2.BackColor = ColorTranslator.FromHtml("#0CA6CA");


            FpDeptment.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

            FpDeptment.Sheets[0].SheetName = "Settings";
            FpDeptment.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            FpDeptment.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
            FpDeptment.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            FpDeptment.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpDeptment.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpDeptment.Sheets[0].DefaultStyle.Font.Bold = false;
        }

    }

    public void bindEduLevel()
    {
        try
        {
            rblCourse.Items.Clear();
            qry = "select distinct Edu_Level from course where college_code='" + Convert.ToString(Session["collegecode"]) + "' order by Edu_Level desc";
            ds = d2.select_method_wo_parameter(qry, sptype);
            if (ds.Tables[0].Rows.Count > 0)
            {
                rblCourse.DataSource = ds;
                rblCourse.DataTextField = "Edu_Level";
                rblCourse.DataValueField = "Edu_Level";
                rblCourse.DataBind();
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
        }
    }

    public void BindBatch()
    {
        try
        {
            ds = d2.select_method_wo_parameter("bind_batch", "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = ds;
                ddlBatch.DataTextField = "batch_year";
                ddlBatch.DataValueField = "batch_year";
                ddlBatch.DataBind();
                ddlBatch.SelectedIndex = ddlBatch.Items.Count - 1;
            }
        }
        catch
        {

        }
    }

    public void bindDays()
    {
        ddlDays.Items.Clear();
        ddlDays.Items.Add(new ListItem("", "-1"));
        ddlDays.Items.Add(new ListItem("Sunday", "0"));
        ddlDays.Items.Add(new ListItem("Monday", "1"));
        ddlDays.Items.Add(new ListItem("Tuesday", "2"));
        ddlDays.Items.Add(new ListItem("Wednesday", "3"));
        ddlDays.Items.Add(new ListItem("Thursday", "4"));
        ddlDays.Items.Add(new ListItem("Friday", "5"));
        ddlDays.Items.Add(new ListItem("Saturday", "6"));

        if (ddlDays.Items.Count > 0)
        {
            ddlDays.SelectedIndex = 2;
        }
    }

    protected void rbCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDepartmentSpread();
    }

    protected void ddlBatch__SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDepartmentSpread();
    }

    protected void ddlSemester_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlDays_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDepartmentSpread();
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }


    protected void FpDeptment_UpdateCommand(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(FpDeptment.Sheets[0].Cells[0, 1].Value) == 1)
            {
                for (int i = 0; i < FpDeptment.Sheets[0].RowCount; i++)
                {
                    FpDeptment.Sheets[0].Cells[i, 1].Value = 1;
                }
            }
            else if (Convert.ToInt32(FpDeptment.Sheets[0].Cells[0, 1].Value) == 0)
            {
                for (int i = 0; i < FpDeptment.Sheets[0].RowCount; i++)
                {
                    FpDeptment.Sheets[0].Cells[i, 1].Value = 0;
                }

            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int val = 0;
            int selcount = 0;
            string degreecode = "";
            string dayorder = "", batchyear = "", sem = "", sec = "";
            qry = "";
            FpDeptment.SaveChanges();
            string nofHours = "", totfHalf = "", totSHalf = "", minFHalf = "", minSHalf = "";
            int noofhours1 = 0, totfHalf1 = 0, totSHalf1 = 0, minFHalf1 = 0, minSHalf1 = 0;
            if (ddlDays.Items.Count == 0)
            {
                lbl_popuperr.Text = "No Days are Found";
                imgdiv2.Visible = true;
                return;
            }
            else if (ddlDays.Items.Count > 0 && rblCourse.Items.Count > 0)
            {
                //rblCourse.Items.Count == 0
                if (ddlDays.SelectedValue.ToString() == "0" || ddlDays.SelectedValue.ToString() == "-1")
                {
                    lbl_popuperr.Text = "Please Select Other Than Sunday and Empty";
                    imgdiv2.Visible = true;
                    return;
                }
                if (FpDeptment.Sheets[0].RowCount > 0)
                {
                    FpDeptment.SaveChanges();
                    dayorder = Convert.ToString(ddlDays.SelectedValue);
                    for (int row = 1; row < FpDeptment.Sheets[0].RowCount; row++)
                    {
                        val = Convert.ToInt32(FpDeptment.Sheets[0].Cells[row, 1].Value);
                        if (val == 1)
                        {
                            selcount++;
                            batchyear = Convert.ToString(FpDeptment.Sheets[0].Cells[row, 2].Text);
                            degreecode = Convert.ToString(FpDeptment.Sheets[0].Cells[row, 3].Tag);
                            sem = Convert.ToString(FpDeptment.Sheets[0].Cells[row, 5].Text);
                            sec = Convert.ToString(FpDeptment.Sheets[0].Cells[row, 6].Text);
                            nofHours = Convert.ToString(FpDeptment.Sheets[0].Cells[row, 7].Text);
                            totfHalf = Convert.ToString(FpDeptment.Sheets[0].Cells[row, 8].Text);
                            totSHalf = Convert.ToString(FpDeptment.Sheets[0].Cells[row, 9].Text);
                            minFHalf = Convert.ToString(FpDeptment.Sheets[0].Cells[row, 10].Text);
                            minSHalf = Convert.ToString(FpDeptment.Sheets[0].Cells[row, 11].Text);
                            qry += "if exists(select * from PeriodAttndScheduleNew where degree_code='" + degreecode + "' and DayOrder='" + dayorder + "' and batch_year='" + batchyear + "' and semester='" + sem + "' and section='" + sec + "') update PeriodAttndScheduleNew set DayOrder='" + dayorder + "',No_of_hrs_per_day='" + nofHours + "',no_of_hrs_I_half_day='" + totfHalf + "',no_of_hrs_II_half_day='" + totSHalf + "',min_pres_I_half_day='" + minFHalf + "',min_pres_II_half_day='" + minSHalf + "', batch_year='" + batchyear + "' , semester='" + sem + "' , section='" + sec + "' where degree_code='" + degreecode + "' and DayOrder='" + dayorder + "' and batch_year='" + batchyear + "' and semester='" + sem + "' and section='" + sec + "' else insert into PeriodAttndScheduleNew (degree_code,DayOrder,No_of_hrs_per_day,no_of_hrs_I_half_day,no_of_hrs_II_half_day,min_pres_I_half_day,min_pres_II_half_day,batch_year,semester,section) values ('" + degreecode + "','" + dayorder + "','" + nofHours + "','" + totfHalf + "','" + totSHalf + "','" + minFHalf + "','" + minSHalf + "','" + batchyear + "','" + sem + "','" + sec + "')";
                        }
                    }
                    int inserted = 0;
                    if (selcount > 0)
                    {
                        inserted = d2.update_method_wo_parameter(qry, sptype);
                    }
                    if (selcount == 0)
                    {
                        lbl_popuperr.Text = "Please Select Atleast one Record";
                        imgdiv2.Visible = true;
                        return;
                    }
                    else if (inserted > 0)
                    {
                        lbl_popuperr.Text = "Saved Successfully";
                        imgdiv2.Visible = true;
                    }
                    else
                    {
                        lbl_popuperr.Text = "Not Saved Successfully";
                        imgdiv2.Visible = true;
                        return;
                    }
                }
            }
            LoadDepartmentSpread();
        }
        catch (Exception ex)
        {

        }

    }

    public void LoadDepartmentSpread()
    {
        try
        {
            FpDeptment.Visible = false;
            btnSave.Visible = false;
            string dayorder = "";
            string sel_edu_level = "";
            if (ddlBatch.Items.Count > 0)
            {
                batchyear = Convert.ToString(ddlBatch.SelectedItem.Text);
            }
            if (rblCourse.Items.Count == 0)
            {
                lbl_popuperr.Text = "No Records Found";
                imgdiv2.Visible = true;
                return;
            }
            else if (ddlDays.Items.Count == 0)
            {
                lbl_popuperr.Text = "No Days are Found";
                imgdiv2.Visible = true;
                return;
            }
            else
            {
                if (ddlDays.SelectedValue.ToString() == "0" || ddlDays.SelectedValue.ToString() == "-1")
                {
                    lbl_popuperr.Text = "Please Select Other Than Sunday and Empty";
                    imgdiv2.Visible = true;
                    return;
                }
                dayorder = Convert.ToString(ddlDays.SelectedValue);
                sel_edu_level = Convert.ToString(rblCourse.SelectedItem.Text);
                //and r.degree_code=45
                //select distinct (c.Course_Name+'-'+ dpt.dept_acronym) as dept,dg.Degree_Code,c.Course_Name,dpt.Dept_Name,dg.NoofSections,Duration,r.Current_Semester,r.Sections from Degree dg,course c,Department dpt,Registration r where r.college_code=dg.college_code and r.degree_code=dg.Degree_Code and c.college_code=dpt.college_code and dpt.college_code=dg.college_code and dg.Dept_Code=dpt.Dept_Code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar' and dg.Course_Id=c.Course_Id and c.Edu_Level='" + sel_edu_level + "' and dg.college_code='" + Convert.ToString(Session["collegecode"]) + "' and Batch_Year="'+batchyear+'"  order by r.batch_year desc,current_semester asc, dg.degree_code,r.Sections asc;
                //qry = "select distinct Degree_Code,c.Course_Name,dpt.Dept_Name,dg.NoofSections,Duration from Degree dg,course c,Department dpt where c.college_code=dpt.college_code and dpt.college_code=dg.college_code and dg.Dept_Code=dpt.Dept_Code and dg.Course_Id=c.Course_Id and c.Edu_Level='" + sel_edu_level + "' and dg.college_code='" + Convert.ToString(Session["collegecode"]) + "' order by Course_Name; select * from PeriodAttndScheduleNew where DayOrder='" + dayorder + "'";
                qry = "select distinct (c.Course_Name+'-'+ dpt.dept_acronym) as dept,r.batch_year,dg.Degree_Code,c.Course_Name,dpt.Dept_Name,dg.NoofSections,Duration,r.Current_Semester,r.Sections from Degree dg,course c,Department dpt,Registration r where r.college_code=dg.college_code and r.degree_code=dg.Degree_Code and c.college_code=dpt.college_code and dpt.college_code=dg.college_code and dg.Dept_Code=dpt.Dept_Code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar' and dg.Course_Id=c.Course_Id and c.Edu_Level='" + sel_edu_level + "' and dg.college_code='" + Convert.ToString(Session["collegecode"]) + "' and Batch_Year='" + batchyear + "' order by r.batch_year desc,r.current_semester asc, dg.degree_code,r.Sections asc; select * from PeriodAttndScheduleNew where DayOrder='" + dayorder + "' and batch_year='" + batchyear + "'";
                ds = d2.select_method_wo_parameter(qry, sptype);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                    style2.Font.Size = FontUnit.Medium;
                    style2.Font.Name = "Book Antiqua";
                    style2.Font.Bold = false;
                    style2.HorizontalAlign = HorizontalAlign.Center;
                    style2.VerticalAlign = VerticalAlign.Middle;
                    style2.ForeColor = System.Drawing.Color.Black;

                    FarPoint.Web.Spread.DoubleCellType dblcell = new FarPoint.Web.Spread.DoubleCellType();
                    FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                    FarPoint.Web.Spread.CheckBoxCellType chkeach = new FarPoint.Web.Spread.CheckBoxCellType();
                    FarPoint.Web.Spread.IntegerCellType intgrcell = new FarPoint.Web.Spread.IntegerCellType();
                    intgrcell.ErrorMessage = "Enter Positive Integer Values Only";
                    intgrcell.MinimumValue = 0;
                    chkall.AutoPostBack = true;


                    FpDeptment.Sheets[0].RowCount = 0;
                    FpDeptment.Sheets[0].AutoPostBack = false;
                    FpDeptment.Sheets[0].RowCount++;
                    FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 1].CellType = chkall;
                    FpDeptment.Sheets[0].SpanModel.Add(FpDeptment.Sheets[0].RowCount - 1, 2, 1, 12);

                    FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 7].Locked = true;
                    FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 8].Locked = true;
                    FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 9].Locked = true;
                    FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 10].Locked = true;
                    FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 11].Locked = true;

                    FpDeptment.Sheets[0].FrozenRowCount = 1;
                    FpDeptment.Sheets[0].FrozenColumnCount = 7;

                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        FpDeptment.Sheets[0].RowCount++;
                        //int noofsec = 0;
                        //int.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["NoofSections"]), out noofsec);

                        string degree_code = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
                        string sem = Convert.ToString(ds.Tables[0].Rows[row]["Current_Semester"]);
                        string sec = Convert.ToString(ds.Tables[0].Rows[row]["Sections"]);
                        FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);

                        FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 1].CellType = chkeach;
                        FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 2].Text = batchyear;
                        FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Course_Name"]);
                        FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(degree_code);
                        FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;

                        FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Dept_Name"]);
                        FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;

                        FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["Current_Semester"]);
                        FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;


                        FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Sections"]);
                        FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;


                        FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 7].CellType = intgrcell;
                        FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 8].CellType = intgrcell;

                        FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 9].CellType = intgrcell;
                        FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 10].CellType = intgrcell;
                        FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 11].CellType = intgrcell;
                        if (ds.Tables.Count == 2)
                        {
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                ds.Tables[1].DefaultView.RowFilter = "Degree_Code='" + degree_code + "' and semester='" + sem + "' and section='" + sec + "'";
                                DataView dv = new DataView();
                                dv = ds.Tables[1].DefaultView;
                                if (dv.Count > 0)
                                {
                                    FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dv[0]["No_of_hrs_per_day"]);
                                    FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(dv[0]["no_of_hrs_I_half_day"]);

                                    FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dv[0]["no_of_hrs_II_half_day"]);
                                    FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(dv[0]["min_pres_I_half_day"]);
                                    FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(dv[0]["min_pres_II_half_day"]);
                                }
                            }
                        }

                        //if (noofsec == 0)
                        //{                           
                        //    FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 4].Text = "";
                        //}
                        //else if (noofsec == 1)
                        //{
                        //    FpDeptment.Sheets[0].Cells[FpDeptment.Sheets[0].RowCount - 1, 4].Text = "A";
                        //}
                        //else if (noofsec > 1)
                        //{

                        //}
                    }
                    FpDeptment.Visible = true;
                    btnSave.Visible = true;
                }
                else
                {
                    FpDeptment.Visible = false;
                    btnSave.Visible = false;
                    lbl_popuperr.Text = "No Departments were Found for " + sel_edu_level;
                    imgdiv2.Visible = true;
                    return;
                }
            }
            FpDeptment.Sheets[0].PageSize = FpDeptment.Sheets[0].RowCount;
            FpDeptment.Height = (FpDeptment.Sheets[0].RowCount * 28) + 120;
            FpDeptment.SaveChanges();
        }
        catch (Exception ex)
        {

        }
    }

}
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;

public partial class SplhourBatchAllocation : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    string user_code = "";
    string college_code = "";
    string college = "";
    string batch = "";
    string degreevalue = "";
    string semester = "";
    string section = "";
    string selectdate = "";
    string subject_no = "";
    string[] addnewlist;
    bool testflage = false;
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        user_code = Session["usercode"].ToString();
        college_code = Session["collegecode"].ToString();
        if (!IsPostBack)
        {
            bindcollege();
            BindBatch();
            degree();
            bindbranch();
            bindsem();
            bindsection();
            Fpspread.Sheets[0].RowCount = 0;
            Fpspread.Sheets[0].ColumnCount = 0;
            Fpspread.Visible = false;
            btngo.Visible = true;
            subtable.Visible = false;
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;

            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            ds = d2.select_method_wo_parameter(Master1, "text");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    if (ds.Tables[0].Rows[k]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[k]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[k]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                }
            }
        }
    }

    public void bindcollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch
        {
        }
    }

    public void BindBatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
                ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void degree()
    {
        try
        {
            ds.Clear();
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            ds = d2.select_method_wo_parameter("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + ddlcollege.SelectedItem.Value + "' and deptprivilages.Degree_code=degree.Degree_code " + columnfield + " order by degree.Course_Id ", "Text");
            ddldegree.Items.Clear();
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();

            }

        }
        catch (Exception ex)
        {
        }
    }

    public void bindbranch()
    {
        try
        {
            ddldepartment.Items.Clear();
            string commname = "";
            string branch = ddldegree.SelectedItem.Value;

            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }

            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code " + columnfield + " order by degree.Degree_code";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code " + columnfield + " order by degree.Degree_code";
            }
            ds = d2.select_method_wo_parameter(commname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldepartment.DataSource = ds;
                ddldepartment.DataTextField = "dept_name";
                ddldepartment.DataValueField = "degree_code";
                ddldepartment.DataBind();
            }


        }
        catch (Exception ex)
        {

        }
    }

    public void bindsem()
    {
        try
        {
            ds.Clear();
            ddlsem.Items.Clear();
            ds = d2.BindSem(ddldepartment.SelectedItem.Value, ddlbatch.SelectedItem.Text, ddlcollege.SelectedItem.Value);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string duration = Convert.ToString(ds.Tables[0].Rows[0][0]);
                if (duration.Trim() != "")
                {
                    for (int i = 1; i <= Convert.ToInt32(duration); i++)
                    {
                        ddlsem.Items.Add(Convert.ToString(i));
                    }
                }

            }
        }
        catch
        {

        }
    }

    public void bindsection()
    {
        try
        {
            ds.Clear();
            ds = d2.BindSectionDetail(ddlbatch.SelectedItem.Text, ddldepartment.SelectedItem.Value);
            ddlsection.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsection.DataSource = ds;
                ddlsection.DataTextField = "sections";
                ddlsection.DataValueField = "sections";
                ddlsection.DataBind();
            }
            if (ddlsection.Items.Count > 0)
            {
                ddlsection.Enabled = true;
            }
            else
            {
                ddlsection.Enabled = false;
            }

        }
        catch
        {

        }
    }
    protected void ddlbatch_Change(object sender, EventArgs e)
    {
        try
        {
            bindsem();
            bindsection();
            btngo.Visible = true;
            mainvlaue.Visible = false;
            subtable.Visible = false;
            // fpspread.Visible = false;
            //rptprint.Visible = false;
        }
        catch
        {

        }
    }


    protected void ddldepartment_Change(object sender, EventArgs e)
    {
        bindsem();
        bindsection();
        btngo.Visible = true;
        mainvlaue.Visible = false;
        subtable.Visible = false;
        // fpspread.Visible = false;
        // rptprint.Visible = false;
    }

    protected void ddldegree_Change(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
        bindsection();
        btngo.Visible = true;
        mainvlaue.Visible = false;
        subtable.Visible = false;
        //fpspread.Visible = false;
        //  rptprint.Visible = false;
    }

    protected void ddlsem_Change(object sender, EventArgs e)
    {
        try
        {
            mainvlaue.Visible = false;
            btngo.Visible = true;
            subtable.Visible = false;
        }
        catch
        {
        }
    }

    protected void ddlsection_Change(object sender, EventArgs e)
    {
        try
        {
            mainvlaue.Visible = false;
            btngo.Visible = true;
            subtable.Visible = false;
        }
        catch
        {
        }
    }

    protected void btngo_click(object sender, EventArgs e)
    {

        try
        {
            college = Convert.ToString(ddlcollege.SelectedItem.Value);
            batch = Convert.ToString(ddlbatch.SelectedItem.Text);
            degreevalue = Convert.ToString(ddldepartment.SelectedItem.Value);
            semester = Convert.ToString(ddlsem.SelectedItem.Text);
            section = "";
            string sectionquery = "";
            if (ddlsection.Enabled == true)
            {
                section = Convert.ToString(ddlsection.SelectedItem.Text);
                sectionquery = "and sections='" + section + "'";
            }
            string selectquery = "select CONVERT(varchar(10), date,103) as date,hrentry_no  from specialhr_master where degree_code =" + degreevalue + " and semester =" + semester + " and batch_year =" + batch + " " + sectionquery + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlspecialdate.DataSource = ds;
                ddlspecialdate.DataTextField = "date";
                ddlspecialdate.DataValueField = "hrentry_no";
                ddlspecialdate.DataBind();
                if (ddlspecialdate.Items.Count > 0)
                {
                    string subjecquery = "select distinct sh.subject_no,subject_name  from specialhr_details sh,subject s where sh.subject_no =s.subject_no  and hrentry_no  in ('" + ddlspecialdate.SelectedItem.Value + "')";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(subjecquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlsubject.DataSource = ds;
                        ddlsubject.DataTextField = "subject_name";
                        ddlsubject.DataValueField = "subject_no";
                        ddlsubject.DataBind();
                    }

                }
                subtable.Visible = true;
                errorlable.Visible = false;
                btngo.Visible = false;
            }
            else
            {
                subtable.Visible = false;
                errorlable.Text = "No Records Found";
                errorlable.Visible = true;
                mainvlaue.Visible = false;
                btngo.Visible = true;
            }

        }
        catch
        {

        }
    }

    protected void ddlspecialdate_Change(object sender, EventArgs e)
    {
        try
        {
            string subjecquery = "select distinct sh.subject_no,subject_name  from specialhr_details sh,subject s where sh.subject_no =s.subject_no  and hrentry_no  in ('" + ddlspecialdate.SelectedItem.Value + "') ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(subjecquery, "Text");
            ddlsubject.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsubject.DataSource = ds;
                ddlsubject.DataTextField = "subject_name";
                ddlsubject.DataValueField = "subject_no";
                ddlsubject.DataBind();
            }
        }
        catch
        {

        }
    }

    protected void btnallocate_click(object sender, EventArgs e)
    {
        try
        {

            selectdate = Convert.ToString(ddlspecialdate.SelectedItem.Text);
            subject_no = Convert.ToString(ddlsubject.SelectedItem.Value);
            college = Convert.ToString(ddlcollege.SelectedItem.Value);
            batch = Convert.ToString(ddlbatch.SelectedItem.Text);
            degreevalue = Convert.ToString(ddldepartment.SelectedItem.Value);
            semester = Convert.ToString(ddlsem.SelectedItem.Text);
            section = "";
            string sectionquery = "";
            if (ddlsection.Enabled == true)
            {
                section = Convert.ToString(ddlsection.SelectedItem.Text);
                sectionquery = "and sections='" + section + "'";
            }
            if (txt_noofbatchs.Text.Trim() != "")
            {
                ddllabbatch.Items.Clear();
                ddllabbatch.Items.Add("Select");
                int ch = 0;
                addnewlist = new string[Convert.ToInt32(txt_noofbatchs.Text)];
                for (int i = 1; i <= Convert.ToInt32(txt_noofbatchs.Text); i++)
                {
                    ddllabbatch.Items.Add("B" + i + "");
                    addnewlist[ch] = ("B" + i + "");
                    ch++;
                }
            }

            DataSet ds_batch = new DataSet();
            string batchcomboxquery = "select distinct subjectChooser_New_Spl.batch as batch from subjectChooser_New_Spl,Registration where subjectChooser_New_Spl.roll_no= registration.roll_no and semester ='" + ddlsem.SelectedItem.Text + "' and  registration.degree_Code = '" + ddldepartment.SelectedItem.Value + "' and registration.batch_year = '" + ddlbatch.SelectedItem.Text + "' " + sectionquery + " and batch<>''";
            ds_batch = d2.select_method_wo_parameter(batchcomboxquery, "text");

            if (ds_batch.Tables[0].Rows.Count > 0)
            {
                cbbatchlist.DataSource = ds_batch;
                cbbatchlist.DataValueField = "batch";
                cbbatchlist.DataTextField = "batch";
                cbbatchlist.DataBind();
            }

            string islab = d2.GetFunction("select sm.Lab  from subject s,sub_sem sm where s.subType_no =sm.subType_no and s.subject_no ='" + subject_no + "'");
            if (islab.Trim() == "1" || islab.Trim() == "True")
            {
                string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                string strorder = "ORDER BY Roll_No";
                if (orderby_Setting == "0")
                {
                    strorder = "ORDER BY Roll_No";
                }
                else if (orderby_Setting == "1")
                {
                    strorder = "ORDER BY Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = "ORDER BY Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "ORDER BY Roll_No,Reg_No,Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "ORDER BY Roll_No,Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "ORDER BY Reg_No,Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "ORDER BY Roll_No,Stud_Name";
                }
                string stu_namequery = "select  roll_no as rollno,Reg_No , stud_name as studentname  from registration where degree_code='" + degreevalue + "' and batch_year='" + batch + "' " + sectionquery + " and current_semester='" + semester + "' and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' " + strorder + "";
                ds.Clear();
                ds = d2.select_method_wo_parameter(stu_namequery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread.Sheets[0].RowCount = 0;
                    Fpspread.Sheets[0].ColumnCount = 0;

                    Fpspread.CommandBar.Visible = false;
                    Fpspread.Sheets[0].RowHeader.Visible = false;
                    Fpspread.Sheets[0].AutoPostBack = false;
                    Fpspread.Sheets[0].ColumnCount = 6;
                    Fpspread.Sheets[0].ColumnHeader.RowCount = 1;
                    MyStyle.Font.Bold = true;
                    MyStyle.Font.Size = FontUnit.Medium;
                    MyStyle.HorizontalAlign = HorizontalAlign.Center;
                    MyStyle.ForeColor = Color.Black;
                    MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Fpspread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Batch";
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;

                    Fpspread.Sheets[0].Columns[0].Locked = true;

                    Fpspread.Sheets[0].Columns[2].Locked = true;
                    Fpspread.Sheets[0].Columns[3].Locked = true;
                    Fpspread.Sheets[0].Columns[4].Locked = true;
                    Fpspread.Sheets[0].Columns[5].Locked = true;
                    FarPoint.Web.Spread.TextCellType txt1 = new FarPoint.Web.Spread.TextCellType();
                    FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                    cb.AutoPostBack = false;
                    ArrayList batchadd = new ArrayList();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fpspread.Sheets[0].RowCount++;
                        Fpspread.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                        Fpspread.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread.Sheets[0].Cells[i, 1].CellType = cb;
                        Fpspread.Sheets[0].Cells[i, 1].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread.Sheets[0].Cells[i, 2].CellType = txt1;
                        Fpspread.Sheets[0].Cells[i, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["rollno"]);

                        //  Fpspread.Sheets[0].Cells[i, 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread.Sheets[0].Cells[i, 3].CellType = txt1;
                        Fpspread.Sheets[0].Cells[i, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                        // Fpspread.Sheets[0].Cells[i, 3].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread.Sheets[0].Cells[i, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["studentname"]);
                        //Fpspread.Sheets[0].Cells[i, 4].HorizontalAlign = HorizontalAlign.Center;

                        string selectedbatch = "select distinct batch from subjectChooser_New_Spl where roll_no='" + Convert.ToString(ds.Tables[0].Rows[i]["rollno"]) + "' and semester='" + ddlsem.SelectedItem.Text + "' and batch is not null and batch<>''";
                        DataSet ds_selebatch = new DataSet();
                        ds_selebatch = d2.select_method_wo_parameter(selectedbatch, "text");
                        string bat = "";
                        if (ds_selebatch.Tables[0].Rows.Count > 0)
                        {
                            bat = ds_selebatch.Tables[0].Rows[0]["batch"].ToString();
                            if (!batchadd.Contains(bat))
                            {
                                batchadd.Add(Convert.ToString(bat));
                            }
                        }
                        if (bat == "")
                        {
                            Fpspread.Sheets[0].Cells[i, 5].Text = "";
                        }
                        else
                        {
                            Fpspread.Sheets[0].Cells[i, 5].Text = bat;
                            Fpspread.Sheets[0].Cells[i, 5].HorizontalAlign = HorizontalAlign.Center;
                        }

                    }

                    if (Session["Rollflag"].ToString() == "1")
                    {
                        Fpspread.Sheets[0].Columns[2].Visible = true;
                    }
                    else
                    {
                        Fpspread.Sheets[0].Columns[2].Visible = false;
                    }
                    if (Session["Regflag"].ToString() == "1")
                    {
                        Fpspread.Sheets[0].Columns[3].Visible = true;
                    }
                    else
                    {
                        Fpspread.Sheets[0].Columns[3].Visible = false;
                    }
                    Fpspread.SaveChanges();
                    Fpspread.Sheets[0].PageSize = Fpspread.Sheets[0].RowCount;
                    errorlable.Visible = false;
                    Fpspread.Visible = true;
                    Fieldset2.Visible = true;
                    mainvlaue.Visible = true;

                    if (ddlspecialdate.Items.Count > 0)
                    {
                        Fpspread1.Sheets[0].RowCount = 0;
                        Fpspread1.Sheets[0].ColumnCount = 0;

                        Fpspread1.CommandBar.Visible = false;
                        Fpspread1.Sheets[0].RowHeader.Visible = false;
                        Fpspread1.Sheets[0].AutoPostBack = false;

                        Fpspread1.Sheets[0].ColumnCount = 3;
                        Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Day";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Hour";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;

                        Fpspread1.Sheets[0].Columns[0].Locked = true;
                        Fpspread1.Sheets[0].Columns[1].Locked = true;
                        Fpspread1.Sheets[0].Columns[2].Locked = true;

                        FarPoint.Web.Spread.ComboBoxCellType cb1;

                        if (txt_noofbatchs.Text.Trim() != "")
                        {
                            cb1 = new FarPoint.Web.Spread.ComboBoxCellType(addnewlist);
                        }
                        else
                        {
                            cb1 = new FarPoint.Web.Spread.ComboBoxCellType();
                        }
                        //cb1.AutoPostBack = true;
                        //cb1.UseValue = true;
                        //cb1.ShowButton = true;
                        string subjecquery = "  select distinct sh.subject_no,subject_name,subject_code ,start_time,end_time,date,sh.staff_code  from specialhr_details sh,subject s ,specialhr_master m ,sub_sem su  where sh.subject_no =s.subject_no and su.subType_no =s.subType_no and Lab='1' and  sh.subject_no =s.subject_no and m.hrentry_no =sh.hrentry_no   and sh.hrentry_no  in ('" + ddlspecialdate.SelectedItem.Value + "')";
                        subjecquery = subjecquery + "   select distinct subject_code,sh.subject_no  from specialhr_details sh,subject s,sub_sem su where sh.subject_no =s.subject_no  and su.subType_no =s.subType_no and Lab='1'  and hrentry_no  in ('" + ddlspecialdate.SelectedItem.Value + "')";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(subjecquery, "Text");
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
                            {
                                Fpspread1.Sheets[0].ColumnCount++;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[1].Rows[k]["subject_code"]);
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                Fpspread1.Sheets[0].ColumnHeader.Cells[0, Fpspread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[1].Rows[k]["subject_no"]);
                            }
                        }

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataView dnew = new DataView();
                            ArrayList addarray = new ArrayList();
                            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                            {
                                string date = Convert.ToString(ds.Tables[0].Rows[k]["date"]);
                                DateTime dt = Convert.ToDateTime(date);
                                string dayvalue = dt.ToString("ddd");
                                DateTime dt1 = Convert.ToDateTime(ds.Tables[0].Rows[k]["start_time"]);
                                string start = dt1.ToString("hh:mm");
                                DateTime dt2 = Convert.ToDateTime(ds.Tables[0].Rows[k]["end_time"]);
                                string end = dt2.ToString("hh:mm");
                                string hour = start + "-" + end;
                                if (!addarray.Contains(hour))
                                {
                                    addarray.Add(hour);
                                    Fpspread1.Sheets[0].RowCount++;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(hour);
                                    int col = 2;
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = dt.ToString("ddd");
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(Fpspread1.Sheets[0].RowCount);
                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    for (int k1 = 0; k1 < ds.Tables[1].Rows.Count; k1++)
                                    {
                                        col++;
                                        ds.Tables[0].DefaultView.RowFilter = "subject_no='" + Convert.ToString(ds.Tables[1].Rows[k1]["subject_no"]) + "' and start_time ='" + dt1 + "' and end_time='" + dt2 + "' ";
                                        dnew = ds.Tables[0].DefaultView;
                                        if (dnew.Count > 0)
                                        {
                                            dt = Convert.ToDateTime(dnew[0]["start_time"]);
                                            string start1 = dt.ToString("hh:mm");
                                            dt = Convert.ToDateTime(dnew[0]["end_time"]);
                                            string end1 = dt.ToString("hh:mm");
                                            string hour2 = start1 + "-" + end1;
                                            string staff_code = Convert.ToString(dnew[0]["staff_code"]);
                                            if (hour == hour2)
                                            {
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].CellType = cb1;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Tag = Convert.ToString(staff_code);
                                                string getvalue = d2.GetFunction("select Stu_Batch  from LabAlloc_New_Spl where Degree_Code ='" + degreevalue + "' and Batch_Year=" + batch + " and Semester=" + semester + " and Subject_No='" + Convert.ToString(ds.Tables[1].Rows[k1]["subject_no"]) + "' and Day_Value ='" + dayvalue + "' and Hour_Value ='" + hour + "' and fdate ='" + date + "' and Staff_Code ='" + staff_code + "' " + sectionquery + "");
                                                if (getvalue.Trim() != "" && getvalue.Trim() != "0")
                                                {
                                                    if (getvalue.Contains(',') == true)
                                                    {
                                                        //if (batchadd.Contains(Convert.ToString(getvalue)))
                                                        //{
                                                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].CellType = txt;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Text = getvalue;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightBlue;
                                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Locked = true;
                                                        //}
                                                    }
                                                    else
                                                    {
                                                        if (batchadd.Contains(Convert.ToString(getvalue)))
                                                        {
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Text = getvalue;
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightBlue;
                                                        }
                                                        else
                                                        {
                                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightBlue;
                                                        }

                                                    }
                                                }
                                                else
                                                {
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightBlue;
                                                }
                                            }
                                            else
                                            {
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Locked = true;
                                            }
                                        }
                                        else
                                        {
                                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Locked = true;
                                        }
                                    }
                                }
                            }
                        }
                        Fpspread1.SaveChanges();
                        Fpspread1.Visible = true;
                        btnbatchsave.Visible = true;
                        lnkmultiple.Visible = true;
                        mainvlaue.Visible = true;
                    }


                }
                else
                {
                    Fpspread.Visible = false;
                    errorlable.Text = "No Records Found";
                    errorlable.Visible = true;
                    Fieldset2.Visible = false;
                    mainvlaue.Visible = false;
                }
            }
            else
            {
                Fpspread.Visible = false;
                errorlable.Text = "This is Not a Lab Subject";
                errorlable.Visible = true;
                Fieldset2.Visible = false;
                mainvlaue.Visible = false;
            }
        }
        catch
        {

        }
    }

    protected void selectgo_Click(object sender, EventArgs e)
    {
        Fpspread.SaveChanges();
        string from = fromno.Text;
        string to = tono.Text;
        if (from != null && from != "" && to != null && to != "")
        {
            int m = Convert.ToInt32(fromno.Text);
            int n = Convert.ToInt32(tono.Text);
            if (m != 0 && n != 0)
            {
                if (Fpspread.Sheets[0].RowCount >= n)
                {
                    for (int rowcount = m; rowcount <= n; rowcount++)
                    {

                        Fpspread.Sheets[0].Cells[rowcount - 1, 1].Value = true;
                        //added by srinath 31/8/2013
                        Btnsave.Enabled = true;
                        Btndelete.Enabled = true;

                    }
                }
                else
                {
                    errorlable.Visible = true;
                    errorlable.Text = "Please Enter Available Student Count";
                }
            }
            else
            {
                errorlable.Visible = true;
                errorlable.Text = "Please Enter Greater than Zero";
            }
        }
        else
        {
            errorlable.Visible = true;
            errorlable.Text = "Please Enter Values";
        }


        fromno.Text = "";
        tono.Text = "";
    }
    protected void Btnsave_Click(object sender, EventArgs e)
    {
        if (Fpspread.Sheets[0].Rows.Count > 0)
        {

            if (ddllabbatch.SelectedItem.Text != "Select")
            {
                testflage = false;
                Fpspread.SaveChanges();
                string subjectvalue = "";
                if (Fpspread1.Sheets[0].ColumnCount > 0)
                {
                    for (int j = 3; j < Fpspread1.Sheets[0].ColumnCount; j++)
                    {
                        string gettag = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, j].Tag);
                        if (subjectvalue.Trim() == "")
                        {
                            subjectvalue = gettag;
                        }
                        else
                        {
                            subjectvalue = subjectvalue + "," + gettag;
                        }
                    }
                }

                for (int jk = 0; jk < Fpspread.Sheets[0].Rows.Count; jk++)
                {
                    int isval = Convert.ToInt16(Fpspread.Sheets[0].Cells[jk, 1].Value);
                    string rollno = Convert.ToString(Fpspread.Sheets[0].Cells[jk, 2].Text);
                    if (isval == 1)
                    {
                        isval = 0;
                        ds.Clear();
                        string batchsql = "select * from subjectchooser,sub_sem,subject where subjectchooser.roll_no='" + rollno + "' and semester = '" + ddlsem.SelectedItem.Text + "' and subjectchooser.subject_no in(" + subjectvalue + ")  and subjectchooser.subtype_no=sub_sem.subtype_no and subjectchooser.subject_no=subject.subject_no";
                        ds = d2.select_method_wo_parameter(batchsql, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int subno = 0; subno < ds.Tables[0].Rows.Count; subno++)
                            {
                                testflage = true;
                                string ssub_no = ds.Tables[0].Rows[subno]["subject_no"].ToString();
                                string paper_order = ds.Tables[0].Rows[subno]["paper_order"].ToString();
                                string subtype = ds.Tables[0].Rows[subno]["subtype_no"].ToString();

                                Fpspread.Sheets[0].Cells[jk, 5].Text = Convert.ToString(ddllabbatch.SelectedItem.Text);
                                Fpspread.Sheets[0].Cells[jk, 5].HorizontalAlign = HorizontalAlign.Center;

                                string updatquery = " if exists (select * from subjectChooser_New_Spl where roll_no='" + rollno + "' and subject_no='" + ssub_no.ToString() + "')";
                                updatquery = updatquery + " update subjectChooser_New_Spl set batch ='" + ddllabbatch.SelectedItem.Text + "' where roll_no='" + rollno + "' and subject_no='" + ssub_no.ToString() + "' else ";
                                updatquery = updatquery + " insert into subjectChooser_New_Spl(semester,roll_no,subject_no,paper_order,subtype_no,Batch) values('" + ddlsem.SelectedItem.Text + "','" + rollno + "','" + ssub_no.ToString() + "','" + paper_order + "','" + subtype + "','" + ddllabbatch.SelectedItem.Text + "')";
                                //con.Close();
                                //con.Open();
                                //SqlCommand cmd = new SqlCommand(updatquery, con);
                                //cmd.ExecuteReader();
                                int u = d2.update_method_wo_parameter(updatquery, "Text");
                            }
                            Fpspread.Sheets[0].Cells[jk, 1].Value = 0;
                        }

                    }
                }
                if (testflage == true)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('saved successfully')", true);
                }
            }
            else
            {
                errorlable.Visible = true;
                errorlable.Text = "Please Select Batch";
            }
        }
    }
    protected void Btndelete_Click(object sender, EventArgs e)
    {
        if (Fpspread.Sheets[0].Rows.Count > 0)
        {
            testflage = false;
            Fpspread.SaveChanges();
            for (int jk = 0; jk < Fpspread.Sheets[0].Rows.Count; jk++)
            {
                int isval = Convert.ToInt16(Fpspread.Sheets[0].Cells[jk, 1].Value);
                if (isval == 1)
                {
                    testflage = true;
                    isval = 0;
                    string rollno = Fpspread.Sheets[0].Cells[jk, 2].Text.ToString();

                    string deletbatch = "update subjectChooser_New_Spl set batch ='' where roll_no='" + rollno + "' and semester='" + ddlsem.SelectedItem.Text + "' ";

                    int d = d2.update_method_wo_parameter(deletbatch, "Text");
                    Fpspread.Sheets[0].Cells[jk, 5].Text = "";
                    Fpspread.Sheets[0].Cells[jk, 5].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            btnallocate_click(sender, e);
            if (testflage == true)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted successfully')", true);
            }
        }
    }
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox1.Checked == true)
            {
                fromno.Enabled = true;
                tono.Enabled = true;
                Btnsave.Enabled = true;
                Btndelete.Enabled = true;
            }
            if (CheckBox1.Checked == false)
            {
                fromno.Enabled = false;
                tono.Enabled = false;
                Btnsave.Enabled = false;
                Btndelete.Enabled = false;
            }
        }
        catch
        {

        }
    }

    protected void Batchallotsave_Click(object sender, EventArgs e)
    {
        try
        {
            Fpspread1.SaveChanges();
            if (Fpspread1.Sheets[0].RowCount > 0)
            {
                section = "";
                testflage = false;
                string sectionquery = "";
                if (ddlsection.Enabled == true)
                {
                    section = Convert.ToString(ddlsection.SelectedItem.Text);
                    sectionquery = "and sections='" + section + "'";
                }

                for (int jr = 0; jr < Fpspread1.Sheets[0].RowCount; jr++)
                {
                    string dayvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[jr, 1].Text);
                    string hourvalue = Convert.ToString(Fpspread1.Sheets[0].Cells[jr, 2].Text);
                    string date = Convert.ToString(ddlspecialdate.SelectedItem.Text);
                    string[] splitdate = date.Split('/');
                    date = Convert.ToString(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                    if (Fpspread1.Sheets[0].ColumnCount > 3)
                    {
                        int col = 2;
                        for (int jk = 3; jk < Fpspread1.Sheets[0].ColumnCount; jk++)
                        {
                            testflage = true;
                            col++;
                            string subjectno = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                            string batch_value = Convert.ToString(Fpspread1.Sheets[0].Cells[jr, col].Text);
                            string staff_code = Convert.ToString(Fpspread1.Sheets[0].Cells[jr, col].Tag);
                            if (batch_value.Trim() != "")
                            {
                                string insertquery = "if not exists ( select * from LabAlloc_New_Spl where Degree_Code ='" + ddldepartment.SelectedItem.Value + "' and Batch_Year=" + ddlbatch.SelectedItem.Text + " and Semester=" + ddlsem.SelectedItem.Text + " and Subject_No='" + subjectno + "' and Day_Value ='" + dayvalue + "' and Hour_Value ='" + hourvalue + "' and fdate ='" + date + "' and Staff_Code ='" + staff_code + "' " + sectionquery + ") insert into  LabAlloc_New_Spl (Degree_Code,Semester,Batch_Year,Subject_No,Day_Value,Hour_Value,Stu_Batch,Staff_Code ,Sections ,fdate) values ('" + ddldepartment.SelectedItem.Value + "','" + ddlsem.SelectedItem.Text + "','" + ddlbatch.SelectedItem.Text + "','" + subjectno + "','" + dayvalue + "','" + hourvalue + "','" + batch_value + "','" + staff_code + "','" + section + "','" + date + "') else update LabAlloc_New_Spl set Stu_Batch ='" + batch_value + "' where Degree_Code ='" + ddldepartment.SelectedItem.Value + "' and Batch_Year=" + ddlbatch.SelectedItem.Text + " and Semester=" + ddlsem.SelectedItem.Text + " and Subject_No='" + subjectno + "' and Day_Value ='" + dayvalue + "' and Hour_Value ='" + hourvalue + "' and fdate ='" + date + "' and Staff_Code ='" + staff_code + "' " + sectionquery + " ";
                                int up = d2.update_method_wo_parameter(insertquery, "Text");
                            }
                        }
                    }
                }
                if (testflage == true)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                }
            }
        }
        catch
        {

        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        try
        {
            int ar = 0;
            int ac = 0;
            string value = "";

            section = "";
            string sectionquery = "";
            if (ddlsection.Enabled == true)
            {
                section = Convert.ToString(ddlsection.SelectedItem.Text);
                sectionquery = "and sections='" + section + "'";
            }

            DataSet ds_batch = new DataSet();
            string batchcomboxquery = "select distinct subjectChooser_New_Spl.batch as batch from subjectChooser_New_Spl,Registration where subjectChooser_New_Spl.roll_no= registration.roll_no and semester ='" + ddlsem.SelectedItem.Text + "' and  registration.degree_Code = '" + ddldepartment.SelectedItem.Value + "' and registration.batch_year = '" + ddlbatch.SelectedItem.Text + "' " + sectionquery + " and batch<>''";
            ds_batch = d2.select_method_wo_parameter(batchcomboxquery, "text");

            if (ds_batch.Tables[0].Rows.Count > 0)
            {
                cbbatchlist.DataSource = ds_batch;
                cbbatchlist.DataValueField = "batch";
                cbbatchlist.DataTextField = "batch";
                cbbatchlist.DataBind();
            }

            Fpspread1.SaveChanges();
            ar = Fpspread1.ActiveSheetView.ActiveRow;
            ac = Fpspread1.ActiveSheetView.ActiveColumn;
            if (ac > 1)
            {
                string batchbb = Fpspread1.Sheets[0].Cells[ar, ac].Text;
                string[] batc = batchbb.Split(',');
                if (batc.Length > 0)
                {
                    for (int uu = 0; uu <= batc.GetUpperBound(0); uu++)
                    {
                        string bvv = batc[uu].ToString();
                        for (int i = 0; i < cbbatchlist.Items.Count; i++)
                        {
                            value = cbbatchlist.Items[i].Text;

                            if (bvv == value)
                            {
                                cbbatchlist.Items[i].Selected = true;
                            }

                        }
                    }
                }
                else
                {
                    for (int i = 0; i < cbbatchlist.Items.Count; i++)
                    {
                        value = cbbatchlist.Items[i].Text;

                        if (batchbb == value)
                        {
                            cbbatchlist.Items[i].Selected = true;
                        }
                        else
                        {
                            cbbatchlist.Items[i].Selected = false;
                        }
                    }
                }
            }
            subdiv.Visible = true;
        }
        catch
        {

        }
    }

    protected void btnsub_Clcik(object sender, EventArgs e)
    {
        try
        {
            string value = "";
            string code = "";
            Fpspread.SaveChanges();
            string batchva = "";
            for (int i = 0; i < cbbatchlist.Items.Count; i++)
            {
                if (cbbatchlist.Items[i].Selected == true)
                {
                    value = cbbatchlist.Items[i].Text;
                    code = cbbatchlist.Items[i].Value.ToString();
                    if (batchva == "")
                    {
                        batchva = value;
                    }
                    else
                    {
                        batchva = batchva + ',' + value;
                    }
                }
            }
            int ar = 0;
            int ac = 0;
            ar = Fpspread1.ActiveSheetView.ActiveRow;
            ac = Fpspread1.ActiveSheetView.ActiveColumn;
            if (batchva.Trim() != "")
            {
                if (ac > 1)
                {

                    FarPoint.Web.Spread.TextCellType btva = new FarPoint.Web.Spread.TextCellType();
                    Fpspread1.Sheets[0].Cells[ar, ac].CellType = btva;
                    Fpspread1.Sheets[0].Cells[ar, ac].Text = batchva;
                    Fpspread1.Sheets[0].Cells[ar, ac].Locked = true;
                    subdiv.Visible = false;
                }
            }
            else
            {
                errorlable.Visible = true;
                errorlable.Text = "Please Select Batch";
            }
            Fpspread1.SaveChanges();
        }
        catch
        {

        }
    }

    protected void ddlcollege_change(object sender, EventArgs e)
    {
        try
        {
            BindBatch();
            degree();
            bindbranch();
            bindsem();
            bindsection();
            btngo.Visible = true;
            mainvlaue.Visible = false;
            subtable.Visible = false;
        }
        catch
        {

        }

    }
}


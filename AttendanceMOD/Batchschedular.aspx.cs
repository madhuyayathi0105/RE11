using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Batchschedular : System.Web.UI.Page
{
    DAccess2 obi_access = new DAccess2();
    DAccess2 d2 = new DAccess2();
    bool flagbatch = false;
    bool flag_true = false;
    Hashtable htsubjectno = new Hashtable();
    string ddlcollegevalue = string.Empty;
    string ddlbatchvalue = string.Empty;
    string ddlsemvalue = string.Empty;
    string ddlsecvalue = string.Empty;
    string ddldegreevalue = string.Empty;
    string ddlbranchvalue = string.Empty;
    string selectedbatch = string.Empty;
    string studentname = string.Empty;
    string rollno = string.Empty;
    string regno = string.Empty;
    
    string batchva = string.Empty;
    bool chk = false;
    string droptime = string.Empty;
    FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
    
    DropDownList ddlsection;
    bool serialflag = false;
    static string dayorder = string.Empty;

    protected void ddlsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerror.Visible = false;//added by Srinath 17/8/2013
        DropDownList ddlbatchparent = (DropDownList)this.user_control.FindControl("ddlBatch");
        DropDownList ddlbranchparent = (DropDownList)this.user_control.FindControl("ddlBranch");
        DropDownList ddlsemparent = (DropDownList)this.user_control.FindControl("ddlSemYr");
        DropDownList ddlsection = (DropDownList)this.user_control.FindControl("ddlSec");
        ddlsection.Items.Remove("All");
        ddltimetable.Items.Clear();
        DataSet ds_cssem = new DataSet();
        string cseme = "select distinct current_semester from registration where degree_code ='" + ddlbranchparent.SelectedValue.ToString() + "' and batch_year='" + ddlbatchparent.SelectedValue.ToString() + "' and cc=0 and delflag=0 and exam_flag!='debar' ";
        ds_cssem = obi_access.select_method_wo_parameter(cseme, "text");
        string currentsem = string.Empty;
        if (ds_cssem.Tables[0].Rows.Count > 0)
        {
            currentsem = ds_cssem.Tables[0].Rows[0]["current_semester"].ToString();
            if (Convert.ToString(currentsem) == Convert.ToString(ddlsemparent.SelectedValue.ToString()))
            {
                if (ddltimetable != null)
                {
                    string sections = string.Empty;
                    string strsec = string.Empty;
                    sections = ddlsection.SelectedValue.ToString();
                    if (sections.ToString().Trim().ToLower() == "all" || sections.ToString().Trim().ToLower() == string.Empty || sections.ToString().Trim().ToLower() == "-1")
                    {
                        strsec = string.Empty;
                    }
                    else
                    {
                        strsec = " and sections='" + sections.ToString().Trim() + "'";
                    }
                    DataSet ds_batchs = new DataSet();
                    string batchquery = " select TTName, convert(varchar(15),FromDate,103) as FromDate from Semester_Schedule where degree_code='" + ddlbranchparent.SelectedValue.ToString() + "' and batch_year='" + ddlbatchparent.SelectedValue.ToString() + "' " + strsec + " and semester='" + ddlsemparent.SelectedValue.ToString() + "'";
                    ds_batchs = obi_access.select_method_wo_parameter(batchquery, "text");
                    if (ds_batchs.Tables[0].Rows.Count > 0)
                    {
                        ddltimetable.Items.Clear();
                        DataTable tbl = ds_batchs.Tables[0];
                        foreach (DataRow row in tbl.Rows)
                        {
                            object value = row["TTname"];
                            object value1 = row["FromDate"];
                            string total = value.ToString() + "@" + value1.ToString();
                            ddltimetable.Items.Add(total);
                        }
                    }
                    else
                    {
                        ddltimetable.Items.Clear();
                        ddltimetable.Visible = true;
                        lblerror.Text = "Please Add Timetable Name Before Allot the Batch";
                        lblerror.Visible = true;
                    }
                }
            }
        }
    }

    protected void Removesecall(object sender, EventArgs e)
    {
        DropDownList ddlsection = (DropDownList)this.user_control.FindControl("ddlSec");
        ddltimetable.Items.Clear();
        ddlsection.Items.Remove("All");
        ddlsection_SelectedIndexChanged(sender, e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            //start=======Added by Manikandan 28/07/2013
            ddlsection = ((DropDownList)this.user_control.FindControl("ddlSec"));
            ddlsection.Items.Remove("All");
            ddlsection.SelectedIndexChanged += new EventHandler(this.ddlsection_SelectedIndexChanged);
            DropDownList ddlbatchparent = (DropDownList)this.user_control.FindControl("ddlBatch");
            DropDownList DDldegree = (DropDownList)this.user_control.FindControl("ddlDegree");
            DropDownList ddlbranchparent = (DropDownList)this.user_control.FindControl("ddlBranch");
            DropDownList ddlsemparent = (DropDownList)this.user_control.FindControl("ddlSemYr");
            ddlbatchparent.SelectedIndexChanged += new EventHandler(this.Removesecall);
            DDldegree.SelectedIndexChanged += new EventHandler(this.Removesecall);
            ddlbranchparent.SelectedIndexChanged += new EventHandler(this.Removesecall);
            ddlsemparent.SelectedIndexChanged += new EventHandler(this.Removesecall);
            lblerror.Visible = false;
            //End============
            if (!IsPostBack)
            {
                lblerror.Visible = false;
                batch_spread.Sheets[0].RowCount = 0;
                batch_spread.Sheets[0].ColumnCount = 0;
                batch_spread.Visible = false;
                Batchallot_spread.Sheets[0].RowCount = 0;
                Batchallot_spread.Sheets[0].ColumnCount = 0;
                Batchallot_spread.Visible = false;
                lblselect.Visible = false;
                Btnsave.Visible = false;
                Button1.Visible = false;
                Btndelete.Visible = false;
                fromno.Visible = false;
                tono.Visible = false;
                Button2.Visible = false;
                CheckBox1.Visible = false;
                lblfrom.Visible = false;
                lblto.Visible = false;
                Fieldset4.Visible = false;
                Fieldset2.Visible = false;
                // Panel3.Visible = false;
                Checkboxlistbatch.Visible = false;
                LinkButton1.Visible = false;
                Button3.Visible = false;
                Fieldset5.Visible = false;
                Btnsave.Enabled = false;//added by srinath 31/8/2013
                Btndelete.Enabled = false;
                Fieldset6.Visible = false;
                Fieldset7.Visible = false;
                string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                DataSet ds = d2.select_method_wo_parameter(Master1, "text");
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
            batch_spread.SaveChanges();
            lblvalidation1.Visible = false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(batch_spread, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Batch Allocation Report";
            string pagename = "BatchSchedular.aspx";
            Printcontrol.loadspreaddetails(batch_spread, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }

    protected void ddltimetable_SelectedIndexChanged(object sender, EventArgs e)
    {
        droptime = ddltimetable.SelectedItem.ToString();
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            rptprint.Visible = true;
            //Printcontrol.Visible = true;
            Fieldset6.Visible = false;
            Fieldset7.Visible = false;
            if (ddltimetable.Items.Count >= 1)
            {
                if (ddltimetable.SelectedItem.Text.Trim().ToString() == "")
                {
                    lblerror.Text = "Select Time Table Name";
                    lblerror.Visible = true;
                    return;
                }
            }
            else
            {
                lblerror.Text = "Select Time Table Name";
                lblerror.Visible = true;
                return;
            }
            int icount = 0;
            string subjectnu = string.Empty;
            batch_spread.Sheets[0].AutoPostBack = false;
            Batchallot_spread.Sheets[0].AutoPostBack = false;
            ddlcollegevalue = ((DropDownList)this.user_control.FindControl("ddlcollege")).SelectedValue.ToString();
            ddlbatchvalue = ((DropDownList)this.user_control.FindControl("ddlBatch")).SelectedValue.ToString();
            ddldegreevalue = ((DropDownList)this.user_control.FindControl("ddlDegree")).SelectedValue.ToString();
            ddlsecvalue = ((DropDownList)this.user_control.FindControl("ddlSec")).SelectedValue.ToString();
            ddlsemvalue = ((DropDownList)this.user_control.FindControl("ddlSemYr")).SelectedValue.ToString();
            ddlbranchvalue = ((DropDownList)this.user_control.FindControl("ddlBranch")).SelectedValue.ToString();
            FarPoint.Web.Spread.ComboBoxCellType cmbbox = new FarPoint.Web.Spread.ComboBoxCellType();
            string sections = string.Empty;
            string strsec = string.Empty;
            if (ddlsecvalue.ToString().Trim().ToLower() == "all")
            {
                lblerror.Visible = true;
                batch_spread.Visible = false;
                Batchallot_spread.Visible = false;
                Fieldset2.Visible = false;
                Fieldset4.Visible = false;
                LinkButton1.Visible = false;
                Fieldset5.Visible = false;
                Button3.Visible = false;
                Checkboxlistbatch.Visible = false;
                lblerror.Text = "Please Select Any One Section";
            }
            else
            {
                sections = ddlsecvalue.ToString().Trim();
                if (sections.ToString().Trim().ToLower() == "all" || sections.ToString().Trim().ToLower() == string.Empty || sections.ToString().Trim().ToLower() == "-1")
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " and sections='" + sections.ToString().Trim() + "'";
                }
                //Hidden by srinath
                //DataSet ds_cssem = new DataSet();
                //string cseme = "select distinct current_semester from registration where degree_code ='" + ddlbranchvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "' and cc=0 and delflag=0 and exam_flag!='debar' ";
                //ds_cssem = obi_access.select_method(cseme, hat, "text");
                //string currentsem = ds_cssem.Tables[0].Rows[0]["current_semester"].ToString();
                //if (currentsem != ddlsemvalue.ToString())
                //{
                //    Btnsave.Enabled = false;
                //    Btndelete.Enabled = false;
                //}
                //else
                //{
                //    Btnsave.Enabled = true;
                //    Btndelete.Enabled = true;
                //}
                //batch_spread.Sheets[0].RowCount++;
                // SqlDataReader serial_dr;
                // con.Close();
                // con.Open();
                string cmd = "select LinkValue from inssettings where college_code='" + Session["collegecode"].ToString() + "' and linkname='Student Attendance'";
                //serial_dr = cmd.ExecuteReader();
                DataSet dnew = new DataSet();
                dnew = obi_access.select_method_wo_parameter(cmd, "Text");
                if (dnew.Tables.Count > 0 && dnew.Tables[0].Rows.Count > 0)
                {
                    if (dnew.Tables[0].Rows[0]["LinkValue"].ToString() == "1")
                    {
                        serialflag = true;
                    }
                    else
                    {
                        serialflag = false;
                    }
                }
                DataSet ds_stu_names = new DataSet();
                string stu_namequery = "select roll_no as rollno,Reg_No as regno,stud_name as studentname  from registration where degree_code='" + ddlbranchvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "' " + strsec + " and current_semester='" + ddlsemvalue.ToString() + "' and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by roll_no";
                if (serialflag == true)
                {
                    stu_namequery = "select roll_no as rollno, stud_name as studentname,reg_no as regno  from registration where degree_code='" + ddlbranchvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "' " + strsec + " and current_semester='" + ddlsemvalue.ToString() + "' and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' order by serialno";  //modified by prabha  on jan 
                }
                else
                {
                    string orderby_Setting = obi_access.GetFunction("select value from master_Settings where settings='order_by'");
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
                    stu_namequery = "select roll_no as rollno,Reg_No as regno, stud_name as studentname  from registration where degree_code='" + ddlbranchvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "' " + strsec + " and current_semester='" + ddlsemvalue.ToString() + "' and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR' " + strorder + "";
                }
                ds_stu_names = obi_access.select_method_wo_parameter(stu_namequery, "text");
                //saved batch details 
                DataSet ds_totalsubjects = new DataSet();
                string total_subjects = "select batch from subjectchooser,subject,registration where subject.subject_no=Subjectchooser.subject_no and Subjectchooser.semester = '" + ddlsemvalue.ToString() + "'and Subjectchooser.roll_no=registration.roll_no and  registration.degree_code='" + ddlbranchvalue.ToString() + "' and registration.batch_year='" + ddlbatchvalue.ToString() + "' and registration.current_Semester='" + ddlsemvalue.ToString() + "'";
                ds_totalsubjects = obi_access.select_method_wo_parameter(total_subjects, "text");
                DataSet ds_syllcode = new DataSet();
                string syllcode = "select syll_code from syllabus_master where degree_code = '" + ddlbranchvalue.ToString() + "' and semester = '" + ddlsemvalue.ToString() + "' and Batch_Year = '" + ddlbatchvalue.ToString() + "'";
                ds_syllcode = obi_access.select_method_wo_parameter(syllcode, "Text");

                string syllCode = string.Empty;
                if (ds_syllcode.Tables.Count > 0 && ds_syllcode.Tables[0].Rows.Count > 0)
                    syllCode = Convert.ToString(ds_syllcode.Tables[0].Rows[0]["syll_code"]);

                DataSet ds_subjectnum = new DataSet();
                string subjectnumber = "Select subjecT_no,subjecT_code from subject,sub_sem where sub_sem.subtype_no = subject.subtype_no and (sub_sem.Lab = 1 or sub_sem.projThe=1) and sub_sem.syll_code = subject.syll_code and subject.syll_code='" + syllCode + "'";
                ds_subjectnum = obi_access.select_method_wo_parameter(subjectnumber, "Text");
                string SubNo = string.Empty;
                if (ds_subjectnum.Tables.Count > 0 && ds_subjectnum.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds_subjectnum.Tables[0].Rows)
                    {
                        if (string.IsNullOrEmpty(SubNo))
                            SubNo = Convert.ToString(dr["subjecT_no"]);
                        else
                            SubNo = SubNo + "," + Convert.ToString(dr["subjecT_no"]);
                    }
                }

                if (ds_stu_names.Tables.Count > 0 && ds_stu_names.Tables[0].Rows.Count > 0)
                {
                    batch_spread.Visible = true;
                    batch_spread.SaveChanges();
                    batch_spread.Sheets[0].ColumnCount = 5;
                    batch_spread.Sheets[0].RowCount = 0;
                    lblerror.Visible = false;
                    lblselect.Visible = false;
                    Fieldset4.Visible = true;
                    Btnsave.Visible = true;
                    Button1.Visible = true;
                    Btndelete.Visible = true;
                    CheckBox1.Visible = true;
                    Fieldset2.Visible = true;
                    //sasi
                    LinkButton1.Visible = true;
                    Fieldset5.Visible = false;
                    Button3.Visible = false;
                    Checkboxlistbatch.Visible = false;
                    //
                    batch_spread.Sheets[0].ColumnHeader.RowCount = 1;
                    batch_spread.Sheets[0].RowHeader.ColumnCount = 1;
                    batch_spread.Sheets[0].ColumnHeader.Visible = true;
                    batch_spread.Sheets[0].Rows.Default.Font.Size = FontUnit.Medium;
                    Color c = batch_spread.ColumnHeader.DefaultStyle.BackColor;
                    batch_spread.ActiveSheetView.SheetCorner.DefaultStyle.BackColor = Color.LightCyan;
                    batch_spread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                    batch_spread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                    batch_spread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                    batch_spread.Sheets[0].ColumnHeader.DefaultStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    batch_spread.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = Color.Black;
                    batch_spread.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                    batch_spread.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    batch_spread.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    //start===Added by Manikandan 28/07/2013
                    FarPoint.Web.Spread.TextCellType tb = new FarPoint.Web.Spread.TextCellType();
                    batch_spread.Sheets[0].Columns[1].CellType = tb;
                    batch_spread.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                    batch_spread.Sheets[0].SheetCorner.Cells[0, 0].Font.Bold = true;
                    //=====end
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    batch_spread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
                    batch_spread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                    batch_spread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                    batch_spread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                    batch_spread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Batch";
                    batch_spread.Sheets[0].Columns[2].Width = 200;
                    batch_spread.Sheets[0].Columns[0].Width = 45;
                    batch_spread.Sheets[0].Columns[1].Locked = true;//added by Srinath 17/8/2013
                    batch_spread.Sheets[0].Columns[2].Locked = true;//added by Srinath 17/8/2013
                    batch_spread.Sheets[0].Columns[3].Locked = true;//added by Srinath 17/8/2013
                    if (Session["Rollflag"].ToString() == "1")
                    {
                        batch_spread.Sheets[0].Columns[1].Visible = true;
                    }
                    else
                    {
                        batch_spread.Sheets[0].Columns[1].Visible = false;
                    }
                    if (Session["Regflag"].ToString() == "1")
                    {
                        batch_spread.Sheets[0].Columns[2].Visible = true;
                    }
                    else
                    {
                        batch_spread.Sheets[0].Columns[2].Visible = false;
                    }
                    for (int i = 0; i < ds_stu_names.Tables[0].Rows.Count; i++)
                    {
                        studentname = ds_stu_names.Tables[0].Rows[i]["studentname"].ToString();
                        regno = ds_stu_names.Tables[0].Rows[i]["regno"].ToString();
                        rollno = ds_stu_names.Tables[0].Rows[i]["rollno"].ToString();
                        batch_spread.Sheets[0].RowCount++;
                        string selectedbatch = "select distinct batch from subjectchooser where roll_no='" + rollno + "' and semester='" + ddlsemvalue.ToString() + "' and subject_no in(" + SubNo + ") and batch is not null and batch<>''";
                        DataSet ds_selebatch = new DataSet();
                        ds_selebatch = obi_access.select_method_wo_parameter(selectedbatch, "text");
                        string bat = string.Empty;
                        if (ds_selebatch.Tables[0].Rows.Count > 0)
                        {
                            bat = ds_selebatch.Tables[0].Rows[0]["batch"].ToString();
                        }
                        batch_spread.Sheets[0].Cells[batch_spread.Sheets[0].RowCount - 1, 0].CellType = chkcell;
                        chkcell.AutoPostBack = true;
                        batch_spread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                        batch_spread.Sheets[0].Cells[batch_spread.Sheets[0].RowCount - 1, 1].Text = rollno;
                        batch_spread.Sheets[0].Cells[batch_spread.Sheets[0].RowCount - 1, 2].CellType = txt;
                        batch_spread.Sheets[0].Cells[batch_spread.Sheets[0].RowCount - 1, 2].Text = regno;
                        batch_spread.Sheets[0].Cells[batch_spread.Sheets[0].RowCount - 1, 3].Text = studentname;
                        if (bat == "")
                        {
                            batch_spread.Sheets[0].Cells[batch_spread.Sheets[0].RowCount - 1, 4].Text = string.Empty;
                        }
                        else
                        {
                            batch_spread.Sheets[0].Cells[batch_spread.Sheets[0].RowCount - 1, 4].Text = bat;
                        }
                    }
                    int rowcount = batch_spread.Sheets[0].RowCount;
                    batch_spread.Height = 300;
                    batch_spread.Sheets[0].PageSize = 25 + (rowcount * 20);
                    batch_spread.SaveChanges();
                    // con.Close();
                    ///////======================LabAlloc Start
                    Batchallot_spread.Visible = true;
                    DataSet ds_periodallot = new DataSet();
                    int numbhrs = 0;
                    int numbdays = 0;
                    ArrayList daylist = new ArrayList();
                    daylist.Add("Mon");
                    daylist.Add("Tue");
                    daylist.Add("Wed");
                    daylist.Add("Thu");
                    daylist.Add("Fri");
                    daylist.Add("Sat");
                    daylist.Add("Sun");
                    
                    string getsyllcode = string.Empty;
                    if (ds_syllcode.Tables.Count > 0 && ds_syllcode.Tables[0].Rows.Count > 0)
                    {
                        getsyllcode = ds_syllcode.Tables[0].Rows[0]["syll_code"].ToString();
                      
                        string date1 = ddltimetable.SelectedItem.ToString();
                        string[] date_fm = date1.Split(new Char[] { '@' });
                        string[] date_fm1 = date_fm[date_fm.GetUpperBound(0)].Split(new Char[] { '/' });
                        string fmdate = date_fm1[2].ToString() + "/" + date_fm1[1].ToString() + "/" + date_fm1[0].ToString();
                        string period = "select * from semester_schedule where degree_Code = '" + ddlbranchvalue.ToString() + "' and semester = '" + ddlsemvalue.ToString() + "' and batch_year = '" + ddlbatchvalue.ToString() + "' " + strsec + " and fromdate = '" + fmdate + "'";
                        ds_periodallot = obi_access.select_method_wo_parameter(period, "text");
                        string numberofhrs = "select no_of_hrs_per_day,nodays from periodattndschedule where degree_Code = '" + ddlbranchvalue.ToString() + "'and semester ='" + ddlsemvalue.ToString() + "'";
                        DataSet ds_noofdays = obi_access.select_method_wo_parameter(numberofhrs, "text");
                        numbhrs = Convert.ToInt32(ds_noofdays.Tables[0].Rows[0]["no_of_hrs_per_day"]);
                        numbdays = Convert.ToInt32(ds_noofdays.Tables[0].Rows[0]["nodays"]);
                        DataTable dtv = ds_subjectnum.Tables[0];
                        Hashtable hatsubject = new Hashtable();
                        string validsunno = string.Empty;
                        if (ds_periodallot.Tables.Count > 0 && ds_periodallot.Tables[0].Rows.Count > 0)
                        {
                            for (int days = 0; days < numbdays; days++)
                            {
                                string dayvalue = Convert.ToString(daylist[days]);
                                string temphr = string.Empty;
                                for (int hrs = 1; hrs <= numbhrs; hrs++)
                                {
                                    int hrvalue = Convert.ToInt32(hrs);
                                    string dayhrvalue = dayvalue.ToString() + hrvalue.ToString();
                                    string schdeva = ds_periodallot.Tables[0].Rows[0][dayhrvalue].ToString();
                                    string[] sp = schdeva.Split(';');
                                    bool getflag = false;
                                    string othsub = string.Empty;
                                    for (int hr = 0; hr <= sp.GetUpperBound(0); hr++)
                                    {
                                        string val = sp[hr].ToString();
                                        if (val.Trim() != "" && val != null)
                                        {
                                            string[] spsub = val.Split('-');
                                            if (spsub.GetUpperBound(0) > 1)
                                            {
                                                dtv.DefaultView.RowFilter = " subject_no='" + spsub[0] + "'";
                                                DataView dt = dtv.DefaultView;
                                                if (dt.Count > 0)
                                                {
                                                    getflag = true;
                                                }
                                                if (dt.Count > 0)//magesh 4.9.18
                                                {
                                                    if (othsub == "")
                                                    {
                                                        othsub = spsub[0];
                                                    }
                                                    else
                                                    {
                                                        othsub = othsub + ',' + spsub[0];
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (getflag == true)
                                    {
                                        string[] val = othsub.Split(',');
                                        for (int k = 0; k <= val.GetUpperBound(0); k++)
                                        {
                                            string gva = val[k];
                                            if (!hatsubject.Contains(gva))
                                            {
                                                hatsubject.Add(gva, dayhrvalue);
                                                if (validsunno == "")
                                                {
                                                    validsunno = gva;
                                                }
                                                else
                                                {
                                                    validsunno = validsunno + ',' + gva;
                                                }
                                            }
                                            else
                                            {
                                                string gphr = hatsubject[gva].ToString();
                                                gphr = gphr + ',' + dayhrvalue;
                                                hatsubject[gva] = gphr;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (validsunno.Trim() != "" && validsunno != null)
                        {
                            subjectnumber = "select subjecT_no,subjecT_code from subject where subject_no in(" + validsunno + ")";
                            ds_subjectnum = obi_access.select_method_wo_parameter(subjectnumber, "Text");
                            if (ds_subjectnum.Tables.Count > 0 && ds_subjectnum.Tables[0].Rows.Count > 0)
                            {
                                Batchallot_spread.SaveChanges();
                                Batchallot_spread.Visible = true;
                                lblerror.Visible = false;
                                Batchallot_spread.Sheets[0].ColumnCount = 2;
                                Batchallot_spread.Sheets[0].RowCount = 0;
                                Batchallot_spread.Sheets[0].Rows.Default.Font.Size = FontUnit.Medium;
                                Color c1 = Batchallot_spread.ColumnHeader.DefaultStyle.BackColor;
                                Batchallot_spread.ActiveSheetView.SheetCorner.DefaultStyle.BackColor = Color.Cyan;
                                Batchallot_spread.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                                Batchallot_spread.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                                Batchallot_spread.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                                Batchallot_spread.Sheets[0].ColumnHeader.DefaultStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                Batchallot_spread.Sheets[0].ColumnHeader.DefaultStyle.ForeColor = Color.Black;
                                Batchallot_spread.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                                Batchallot_spread.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                                Batchallot_spread.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                                Batchallot_spread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Day";
                                Batchallot_spread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Hour";
                                //start===Added by Manikandan 28/07/2013                           
                                Batchallot_spread.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                                Batchallot_spread.Sheets[0].SheetCorner.Cells[0, 0].Font.Bold = true;
                                //=====end
                                for (int ssno = 0; ssno < ds_subjectnum.Tables[0].Rows.Count; ssno++)
                                {
                                    string selectedsubjectno = ds_subjectnum.Tables[0].Rows[ssno]["subjecT_no"].ToString();
                                    string selectedsubjectcode = ds_subjectnum.Tables[0].Rows[ssno]["subjecT_code"].ToString();
                                    Batchallot_spread.Sheets[0].ColumnCount++;
                                    Batchallot_spread.Sheets[0].SheetCorner.DefaultStyle.BackColor = Batchallot_spread.Sheets[0].ColumnHeader.DefaultStyle.BackColor;
                                    Batchallot_spread.Sheets[0].ColumnHeader.Cells[0, Batchallot_spread.Sheets[0].ColumnCount - 1].Text = selectedsubjectcode.ToString();
                                    Batchallot_spread.Sheets[0].ColumnHeader.Cells[0, Batchallot_spread.Sheets[0].ColumnCount - 1].Tag = selectedsubjectno.ToString();
                                    // split the date and time table name
                                    //if (ddltimetable.SelectedItem.ToString() != "")
                                    //{
                                    //    string date1 = ddltimetable.SelectedItem.ToString();
                                    //    string[] date_fm = date1.Split(new Char[] { '@' });
                                    //    string[] date_fm1 = date_fm[1].Split(new Char[] { '/' });
                                    //    string fmdate = date_fm1[2].ToString() + "/" + date_fm1[1].ToString() + "/" + date_fm1[0].ToString();
                                    //    period = "select * from semester_schedule where degree_Code = '" + ddlbranchvalue.ToString() + "' and semester = '" + ddlsemvalue.ToString() + "' and batch_year = '" + ddlbatchvalue.ToString() + "' " + strsec + " and fromdate = '" + fmdate + "'";
                                    //    ds_periodallot = obi_access.select_method(period, hat, "text");
                                    //}
                                }
                                DataSet ds_batch = new DataSet();
                                string batchcomboxquery = "select distinct subjectchooser.batch as batch from subjectchooser,Registration where subjectchooser.roll_no= registration.roll_no and semester ='" + ddlsemvalue.ToString() + "' and  registration.degree_Code = '" + ddlbranchvalue.ToString() + "' and registration.batch_year = '" + ddlbatchvalue.ToString() + "' " + strsec + " and batch<>''";
                                ds_batch = obi_access.select_method_wo_parameter(batchcomboxquery, "text");
                                DataTable dt_batch = ds_batch.Tables[0];
                                string[] sublist1 = new string[dt_batch.Rows.Count + 1];
                                if (ds_batch.Tables.Count > 0 && ds_batch.Tables[0].Rows.Count > 0)
                                {
                                    for (icount = 0; icount < dt_batch.Rows.Count; icount++)
                                    {
                                        sublist1[icount] = dt_batch.Rows[icount]["batch"].ToString();
                                    }
                                    if (sublist1.GetUpperBound(0) > 0)
                                    {
                                        sublist1[icount] = " ";
                                    }
                                }
                                //added by sasi  on 
                                if (ds_batch.Tables.Count > 0 && ds_batch.Tables[0].Rows.Count > 0)
                                {
                                    Checkboxlistbatch.DataSource = ds_batch;
                                    Checkboxlistbatch.DataValueField = "batch";
                                    Checkboxlistbatch.DataTextField = "batch";
                                    Checkboxlistbatch.DataBind();
                                }
                                //-------end--------
                                if (ds_periodallot.Tables.Count > 0 && ds_periodallot.Tables[0].Rows.Count > 0)
                                {
                                    for (int days = 0; days < numbdays; days++)
                                    {
                                        string dayvalue = Convert.ToString(daylist[days]);
                                        string temphr = string.Empty;
                                        for (int hrs = 1; hrs <= numbhrs; hrs++)
                                        {
                                            int hrvalue = Convert.ToInt32(hrs);
                                            string dayhrvalue = dayvalue.ToString() + hrvalue.ToString();
                                            string sub = ds_periodallot.Tables[0].Rows[0][dayhrvalue].ToString();
                                            string[] sp_rd_split = sub.Split(';');
                                            for (int index = 0; index <= sp_rd_split.GetUpperBound(0); index++)
                                            {
                                                string[] sp2 = sp_rd_split[index].Split(new Char[] { '-' });
                                                if (sp2.GetUpperBound(0) >= 1)
                                                {
                                                    int upperbound = sp2.GetUpperBound(0);
                                                    subjectnu = sp2[0].ToString();
                                                    bool valiflag = false;
                                                    if (hatsubject.Contains(subjectnu))
                                                    {
                                                        string gethr = hatsubject[subjectnu].ToString();
                                                        string[] spi = gethr.Split(',');
                                                        for (int lo = 0; lo <= spi.GetUpperBound(0); lo++)
                                                        {
                                                            string valhr = spi[lo].ToString();
                                                            if (valhr.Trim().ToLower() == dayhrvalue.Trim().ToLower())
                                                            {
                                                                valiflag = true;
                                                            }
                                                        }
                                                    }
                                                    for (int subcol = 2; subcol < (Convert.ToInt32(Batchallot_spread.Sheets[0].ColumnCount)); subcol++)
                                                    {
                                                        if (subjectnu == Convert.ToString((Batchallot_spread.Sheets[0].ColumnHeader.Cells[0, subcol].Tag)))
                                                        {
                                                            if (valiflag == true)
                                                            {
                                                                if (temphr.ToString() != hrvalue.ToString())
                                                                {
                                                                    Batchallot_spread.Sheets[0].RowCount++;
                                                                    temphr = hrvalue.ToString();
                                                                    Batchallot_spread.Sheets[0].Cells[Batchallot_spread.Sheets[0].RowCount - 1, 0].Text = dayvalue.ToString();
                                                                    Batchallot_spread.Sheets[0].Cells[Batchallot_spread.Sheets[0].RowCount - 1, 1].Text = hrvalue.ToString();
                                                                }
                                                                Batchallot_spread.Sheets[0].Cells[Batchallot_spread.Sheets[0].RowCount - 1, subcol].Locked = false;
                                                                FarPoint.Web.Spread.ComboBoxCellType sub_combo = new FarPoint.Web.Spread.ComboBoxCellType(sublist1);
                                                                Batchallot_spread.Sheets[0].Cells[Batchallot_spread.Sheets[0].RowCount - 1, subcol].CellType = sub_combo;
                                                                sub_combo.AutoPostBack = true;
                                                                string timetablename = string.Empty;
                                                                if (ddltimetable.Text.ToString().Trim() != "")
                                                                {
                                                                    string[] ttname = ddltimetable.SelectedItem.ToString().Split(new Char[] { '@' });
                                                                    timetablename = " and timetablename='" + ttname[0] + "'";
                                                                }
                                                                string selecttedbatch = "select distinct stu_batch from laballoc where batch_year='" + ddlbatchvalue.ToString() + "' and Degree_code='" + ddlbranchvalue.ToString() + "' and semester='" + ddlsemvalue.ToString() + "' and  day_value='" + dayvalue.ToString() + "' and Hour_value='" + hrvalue.ToString() + "' and subject_no='" + subjectnu.ToString() + "' " + strsec + " " + timetablename + " ";
                                                                DataSet ds_setbatch = new DataSet();
                                                                ds_setbatch = obi_access.select_method_wo_parameter(selecttedbatch, "text");
                                                                if (ds_setbatch.Tables.Count > 0 && ds_setbatch.Tables[0].Rows.Count > 0)
                                                                {
                                                                    string gkj = string.Empty;
                                                                    for (int bb = 0; bb < ds_setbatch.Tables[0].Rows.Count; bb++)
                                                                    {
                                                                        string shg = ds_setbatch.Tables[0].Rows[bb]["stu_batch"].ToString();
                                                                        if (gkj == "")
                                                                        {
                                                                            gkj = shg;
                                                                        }
                                                                        else
                                                                        {
                                                                            gkj = gkj + ',' + shg;
                                                                            FarPoint.Web.Spread.TextCellType textcell = new FarPoint.Web.Spread.TextCellType();
                                                                            Batchallot_spread.Sheets[0].Cells[Batchallot_spread.Sheets[0].RowCount - 1, subcol].CellType = textcell;
                                                                        }
                                                                    }
                                                                    Batchallot_spread.Sheets[0].Cells[Batchallot_spread.Sheets[0].RowCount - 1, subcol].Locked = false;
                                                                    Batchallot_spread.Sheets[0].Cells[Batchallot_spread.Sheets[0].RowCount - 1, subcol].Text = gkj;
                                                                }
                                                                Batchallot_spread.Sheets[0].Cells[Batchallot_spread.Sheets[0].RowCount - 1, subcol].BackColor = Color.CornflowerBlue;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Batchallot_spread.Sheets[0].Rows[Batchallot_spread.Sheets[0].RowCount - 1].Locked = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    Batchallot_spread.Sheets[0].PageSize = Batchallot_spread.Sheets[0].RowCount;
                                    Batchallot_spread.SaveChanges();
                                    Fieldset6.Visible = true;
                                    chkautoswitch.Checked = false;
                                    string timetablename1 = string.Empty;
                                    if (ddltimetable.Text.ToString().Trim() != "")
                                    {
                                        string[] ttname = ddltimetable.SelectedItem.ToString().Split(new Char[] { '@' });
                                        timetablename1 = " and timetablename='" + ttname[0] + "'";
                                    }
                                    sections = ddlsecvalue.ToString().Trim();
                                    if (sections.ToString().Trim().ToLower() == "all" || sections.ToString().Trim().ToLower() == string.Empty || sections.ToString().Trim().ToLower() == "-1")
                                    {
                                        sections = string.Empty;
                                    }
                                    else
                                    {
                                        sections = " and sections='" + sections.ToString().Trim() + "'";
                                    }
                                    txtautoswitch.Text = "---Select---";
                                    chkswitch.Checked = false;
                                    string getautoswitchsub = "select distinct Day_Value,Hour_Value from LabAlloc where Batch_Year='" + ddlbatchvalue + "' and Degree_Code='" + ddlbranchvalue + "' and Semester='" + ddlsemvalue + "' " + sections + " and ISNULL(auto_switch,'0')<>'0'  " + timetablename1 + "";
                                    DataSet dsautoswitch = obi_access.select_method_wo_parameter(getautoswitchsub, "Text");
                                    if (dsautoswitch.Tables.Count > 0 && dsautoswitch.Tables[0].Rows.Count > 0)
                                    {
                                        chkautoswitch.Checked = true;
                                        loadautoswich();
                                    }
                                }
                            }
                            else
                            {
                                lblerror.Visible = true;
                                batch_spread.Visible = false;
                                Batchallot_spread.Visible = false;
                                Fieldset2.Visible = false;
                                Fieldset4.Visible = false;
                                Button3.Visible = false;
                                LinkButton1.Visible = false;
                                Button3.Visible = false;
                                Fieldset5.Visible = false;
                                lblerror.Text = "Please Chooser Lab Subject";
                            }
                        }
                        else
                        {
                            lblerror.Visible = true;
                            batch_spread.Visible = false;
                            Batchallot_spread.Visible = false;
                            Fieldset2.Visible = false;
                            Fieldset4.Visible = false;
                            Button3.Visible = false;
                            LinkButton1.Visible = false;
                            Button3.Visible = false;
                            Fieldset5.Visible = false;
                            lblerror.Text = "Please Chooser Lab Subject";
                        }
                    }
                }
                else
                {
                    lblerror.Visible = true;
                    batch_spread.Visible = false;
                    Batchallot_spread.Visible = false;
                    Fieldset2.Visible = false;
                    Fieldset4.Visible = false;
                    Button3.Visible = false;
                    LinkButton1.Visible = false;
                    Button3.Visible = false;
                    Fieldset5.Visible = false;
                    lblerror.Text = "Students Not Available In This Semester";
                }
            }
            batch_spread.SaveChanges();
        }
        catch (Exception es)
        {
            throw es;
        }
    }

    protected void batch_spread_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
    }

    protected void txtbatch_TextChanged(object sender, EventArgs e)
    {
        lblerror.Visible = false;//Added By Srinath 17/8/2013
        ddlnobatches.Items.Clear();
        string numbatch = string.Empty;
        int b_val = 0;
        numbatch = txtbatch.Text.ToString();
        if (numbatch != "" && numbatch != "0")
        {
            ddlnobatches.Items.Insert(0, new ListItem("--Select--", "-1"));
            for (b_val = 1; b_val <= Convert.ToInt16(numbatch.ToString()); b_val++)
            {
                ddlnobatches.Items.Add("B" + b_val.ToString());
            }
        }
        else
        {
            lblerror.Visible = true;
            lblerror.Text = "Select Number of Batch";
        }
    }

    protected void ddlnobatches_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerror.Visible = false;
        selectedbatch = ddlnobatches.SelectedItem.ToString();
        //Btnsave.Enabled = false;
        //Btndelete.Enabled = false;
    }

    protected void batch_spread_selectindexchanged(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
    }

    protected void batch_spread_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
    }

    protected void batch_spread_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void Btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            flagbatch = false;
            lblerror.Visible = false;
            batch_spread.SaveChanges();
            ddlsemvalue = ((DropDownList)this.user_control.FindControl("ddlSemYr")).SelectedValue.ToString();
            string allsubject = string.Empty;
            for (int l = 2; l < Batchallot_spread.Sheets[0].ColumnCount; l++)
            {
                if (allsubject == "")
                {
                    allsubject = Batchallot_spread.Sheets[0].ColumnHeader.Cells[0, l].Tag.ToString();
                }
                else
                {
                    allsubject = allsubject + ',' + Batchallot_spread.Sheets[0].ColumnHeader.Cells[0, l].Tag.ToString();
                }
            }
            if (ddlnobatches.Items.Count > 0 && ddlnobatches.Items.Count != null)//Added By Srinath
            {
             
                if (selectedbatch != "--Select--")
                {
                    for (int stdcount = 0; stdcount <= Convert.ToInt32(batch_spread.Sheets[0].RowCount) - 1; stdcount++)
                    {
                        int isval = Convert.ToInt16(batch_spread.Sheets[0].Cells[stdcount, 0].Value);
                        if (isval == 1)//isval == 1
                        {
                            selectedbatch = ddlnobatches.SelectedItem.ToString();
                            isval = 0;
                            rollno = batch_spread.Sheets[0].Cells[stdcount, 1].Text.ToString();
                            DataSet ds_subjectno = new DataSet();
                            //string batchsql = "select * from subjectchooser,sub_sem,subject where subjectchooser.roll_no='" + rollno + "' and sub_sem.lab=1 and semester = '" + ddlsemvalue.ToString() + "' and subjectchooser.subtype_no=sub_sem.subtype_no and subjectchooser.subject_no=subject.subject_no";
                            string batchsql = "select * from subjectchooser,sub_sem,subject where subjectchooser.roll_no='" + rollno + "' and semester = '" + ddlsemvalue.ToString() + "' and subjectchooser.subject_no in(" + allsubject + ")  and subjectchooser.subtype_no=sub_sem.subtype_no and subjectchooser.subject_no=subject.subject_no";
                            ds_subjectno = obi_access.select_method_wo_parameter(batchsql, "Text");
                            if (ds_subjectno.Tables[0].Rows.Count > 0)
                            {
                                for (int subno = 0; subno < ds_subjectno.Tables[0].Rows.Count; subno++)
                                {
                                    string ssub_no = ds_subjectno.Tables[0].Rows[subno]["subject_no"].ToString();
                                    string paper_order = ds_subjectno.Tables[0].Rows[subno]["paper_order"].ToString();
                                    string subtype = ds_subjectno.Tables[0].Rows[subno]["subtype_no"].ToString();
                                    batch_spread.Sheets[0].Cells[stdcount, 3].Text = selectedbatch.ToString();
                                    string updatquery = " if exists (select * from subjectchooser where roll_no='" + rollno + "' and subject_no='" + ssub_no.ToString() + "')";
                                    updatquery = updatquery + " update subjectchooser set batch ='" + selectedbatch + "' where roll_no='" + rollno + "' and subject_no='" + ssub_no.ToString() + "' else ";
                                    updatquery = updatquery + " insert into subjectchooser(semester,roll_no,subject_no,paper_order,subtype_no,Batch) values('" + ddlsemvalue.ToString() + "','" + rollno + "','" + ssub_no.ToString() + "','" + paper_order + "','" + subtype + "','" + selectedbatch + "')";
                                    //con.Close();
                                    //con.Open();
                                    //SqlCommand cmd = new SqlCommand(updatquery, con);
                                    //cmd.ExecuteReader();
                                    int u = obi_access.update_method_wo_parameter(updatquery, "Text");
                                    flagbatch = true;
                                }
                            }
                            batch_spread.Sheets[0].Cells[stdcount, 0].Value = false;
                        }
                        else//----------------------Modified by Rajkumar on 20-9-2018
                        {
                            isval = 0;
                            rollno = batch_spread.Sheets[0].Cells[stdcount, 1].Text.ToString();
                            selectedbatch = batch_spread.Sheets[0].Cells[stdcount, 4].Text.ToString();
                            DataSet ds_subjectno = new DataSet();
                            string batchsql = "select * from subjectchooser,sub_sem,subject where subjectchooser.roll_no='" + rollno + "' and semester = '" + ddlsemvalue.ToString() + "' and subjectchooser.subject_no in(" + allsubject + ")  and subjectchooser.subtype_no=sub_sem.subtype_no and subjectchooser.subject_no=subject.subject_no";
                            ds_subjectno = obi_access.select_method_wo_parameter(batchsql, "Text");
                            if (ds_subjectno.Tables[0].Rows.Count > 0)
                            {
                                for (int subno = 0; subno < ds_subjectno.Tables[0].Rows.Count; subno++)
                                {
                                    string ssub_no = ds_subjectno.Tables[0].Rows[subno]["subject_no"].ToString();
                                    string paper_order = ds_subjectno.Tables[0].Rows[subno]["paper_order"].ToString();
                                    string subtype = ds_subjectno.Tables[0].Rows[subno]["subtype_no"].ToString();
                                    batch_spread.Sheets[0].Cells[stdcount, 3].Text = selectedbatch.ToString();
                                    string updatquery = " if exists (select * from subjectchooser where roll_no='" + rollno + "' and subject_no='" + ssub_no.ToString() + "')";
                                    updatquery = updatquery + " update subjectchooser set batch ='" + selectedbatch + "' where roll_no='" + rollno + "' and subject_no='" + ssub_no.ToString() + "' else ";
                                    updatquery = updatquery + " insert into subjectchooser(semester,roll_no,subject_no,paper_order,subtype_no,Batch) values('" + ddlsemvalue.ToString() + "','" + rollno + "','" + ssub_no.ToString() + "','" + paper_order + "','" + subtype + "','" + selectedbatch + "')";
                                    int u = obi_access.update_method_wo_parameter(updatquery, "Text");
                                    flagbatch = true;
                                }
                            }
                            batch_spread.Sheets[0].Cells[stdcount, 0].Value = false;
                        }
                    }
                    btnGo_Click(sender, e);//Added by Manikandan 28/07/2013
                    if (flagbatch == true)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('successfully saved')", true);
                        Btnsave.Enabled = false;//added by srinath 31/8/2013
                        Btndelete.Enabled = false;
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Please Select Student and Proceed";
                    }
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Select Batch and Proceed";
                }
            }//Added By Srinath 17/8/2013
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Add No of Batch";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void Btndelete_Click(object sender, EventArgs e)
    {
        batch_spread.SaveChanges();
        ddlsemvalue = ((DropDownList)this.user_control.FindControl("ddlSemYr")).SelectedValue.ToString();
        for (int stdcount = 0; stdcount <= Convert.ToInt32(batch_spread.Sheets[0].RowCount) - 1; stdcount++)
        {
            int isval = Convert.ToInt16(batch_spread.Sheets[0].Cells[stdcount, 0].Value);
            if (isval == 1)
            {
                isval = 0;
                rollno = batch_spread.Sheets[0].Cells[stdcount, 1].Text.ToString();
                string deletbatch = "update subjectchooser set batch ='' where roll_no='" + rollno + "' and semester='" + ddlsemvalue.ToString() + "' ";
                //con.Close();
                //con.Open();
                //SqlCommand cmd = new SqlCommand(deletbatch, con);
                //cmd.ExecuteReader();
                int d = obi_access.update_method_wo_parameter(deletbatch, "Text");
                batch_spread.Sheets[0].Cells[stdcount, 3].Text = string.Empty;
            }
        }
        btnGo_Click(sender, e);
    }

    protected void Batchallotsave_Click(object sender, EventArgs e)
    {
        lblerror.Visible = false;
        flagbatch = false;//added By Srinath 17/8/2013
        Batchallot_spread.SaveChanges();
        int insert = 0;
        try
        {
            ddlcollegevalue = ((DropDownList)this.user_control.FindControl("ddlcollege")).SelectedValue.ToString();
            ddlbatchvalue = ((DropDownList)this.user_control.FindControl("ddlBatch")).SelectedValue.ToString();
            ddldegreevalue = ((DropDownList)this.user_control.FindControl("ddlDegree")).SelectedValue.ToString();
            ddlsecvalue = ((DropDownList)this.user_control.FindControl("ddlSec")).SelectedValue.ToString();
            ddlsemvalue = ((DropDownList)this.user_control.FindControl("ddlSemYr")).SelectedValue.ToString();
            ddlbranchvalue = ((DropDownList)this.user_control.FindControl("ddlBranch")).SelectedValue.ToString();
            string sections = string.Empty;
            string strsec = string.Empty;
            sections = ddlsecvalue.ToString();
            if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
            {
                strsec = string.Empty;
            }
            else
            {
                strsec = " and sections='" + sections.ToString() + "'";
            }
            for (int batchrowcount = 0; batchrowcount <= Convert.ToInt32(Batchallot_spread.Sheets[0].RowCount) - 1; batchrowcount++)
            {
                string fpday = Batchallot_spread.Sheets[0].Cells[batchrowcount, 0].Text;
                string fphour = Batchallot_spread.Sheets[0].Cells[batchrowcount, 1].Text;
                string date1 = ddltimetable.SelectedItem.ToString();
                string[] date_fm = date1.Split(new Char[] { '@' });
                string selecteddate = date_fm[0];
                string[] date_fm1 = date_fm[1].Split(new Char[] { '/' });
                string fmdate = date_fm1[2].ToString() + "/" + date_fm1[1].ToString() + "/" + date_fm1[0].ToString();
                string deletequery = "delete from laballoc where degree_code='" + ddlbranchvalue.ToString() + "' and batch_year='" + ddlbatchvalue.ToString() + "' and semester='" + ddlsemvalue.ToString() + "' and day_value='" + fpday.ToString() + "' and Hour_value='" + fphour.ToString() + "' and sections='" + ddlsecvalue.ToString() + "' and Timetablename='" + selecteddate.ToString() + "'";
                insert = obi_access.update_method_wo_parameter(deletequery, "Text");
                for (int batchcolcount = 2; batchcolcount < Convert.ToInt32(Batchallot_spread.Sheets[0].ColumnCount); batchcolcount++)
                {
                    if (Batchallot_spread.Sheets[0].Cells[batchrowcount, batchcolcount].Text != "")
                    {
                        string fpsubno = string.Empty;
                        string batchname = string.Empty;
                        batchname = Batchallot_spread.Sheets[0].Cells[batchrowcount, batchcolcount].Text;
                        //added by sasi 
                        if (batchname != "")
                        {
                            string[] setbatch = batchname.Split(',');
                            for (int index = 0; index <= setbatch.GetUpperBound(0); index++)
                            {
                                string setbatchname = setbatch[index].ToString();
                                //------end---
                                fpsubno = Batchallot_spread.Sheets[0].ColumnHeader.Cells[0, batchcolcount].Tag.ToString();
                                for (int batchsubcolcount = batchcolcount; batchsubcolcount <= Convert.ToInt32(Batchallot_spread.Sheets[0].ColumnCount - 1); batchsubcolcount++)
                                {
                                    if (batchsubcolcount != batchcolcount)
                                    {
                                        string selectedbatcname = Batchallot_spread.Sheets[0].Cells[batchrowcount, batchsubcolcount].Text;
                                        if (batchname == selectedbatcname)
                                        {
                                            //if (fpsubno.ToString() == Convert.ToString(Batchallot_spread.Sheets[0].ColumnHeader.Cells[0, batchsubcolcount].Tag))
                                            //{
                                            //    /
                                            if (Batchallot_spread.Sheets[0].Cells[batchrowcount, batchcolcount].Text == Batchallot_spread.Sheets[0].Cells[batchrowcount, batchsubcolcount].Text)
                                            {
                                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Student cannot select the same subjects more than once')", true);
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            goto l1;
                                        }
                                    }
                                }
                            l1: string insertcmd = "insert into laballoc(Degree_code,batch_year,semester,day_value,Hour_value,sections,stu_batch,subject_no,Timetablename,Fromdate)values('" + ddlbranchvalue.ToString() + "','" + ddlbatchvalue.ToString() + "','" + ddlsemvalue + "','" + fpday + "','" + fphour + "','" + ddlsecvalue.ToString() + "','" + setbatchname.ToString() + "','" + fpsubno.ToString() + "','" + selecteddate + "','" + fmdate.ToString() + "')";
                                insert = obi_access.update_method_wo_parameter(insertcmd, "Text");
                                flagbatch = true;
                            }
                        }
                        //----end-------
                    }
                }
            }
            if (flagbatch == true)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('successfully saved')", true);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void Batchallot_spread_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs ex)
    {
        string actrow = ex.SheetView.ActiveRow.ToString();
        string actcol = ex.SheetView.ActiveColumn.ToString();
        if (flag_true == false && actrow == "0")
        {
            for (int j = 1; j < Convert.ToInt16(batch_spread.Sheets[0].RowCount); j++)
            {
                actcol = ex.SheetView.ActiveColumn.ToString();
                string seltext = ex.EditValues[Convert.ToInt16(actcol)].ToString();
                batch_spread.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
            }
            flag_true = true;
        }
    }

    private object GetCorrespondingKey(string p, Hashtable htsubjectno)
    {
        throw new NotImplementedException();
    }

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        fromno.Text = string.Empty;
        tono.Text = string.Empty;
        if (CheckBox1.Checked)
        {
            this.fromno.Visible = true;
            this.tono.Visible = true;
            this.lblfrom.Visible = true;
            this.lblto.Visible = true;
            this.Button2.Visible = true;
        }
        else
        {
            this.fromno.Visible = false;
            this.tono.Visible = false;
            this.Button2.Visible = false;
            this.lblfrom.Visible = false;
            this.lblto.Visible = false;
        }
    }

    protected void selectgo_Click(object sender, EventArgs e)
    {
        batch_spread.SaveChanges();
        string from = fromno.Text;
        string to = tono.Text;
        lblerror.Visible = false;
        if (ddlnobatches.Text != "Select" && ddlnobatches.Text != "-1")
        {
            if (from != null && from != "" && to != null && to != "")
            {
                int m = Convert.ToInt32(fromno.Text);
                int n = Convert.ToInt32(tono.Text);
                if (m != 0 && n != 0)
                {
                    if (batch_spread.Sheets[0].RowCount >= n)
                    {
                        for (int rowcount = m; rowcount <= n; rowcount++)
                        {
                            if (txtbatch.Text != "" && txtbatch.Text != "0" && txtbatch.Text != null && ddlnobatches.SelectedItem.ToString() != null && ddlnobatches.SelectedItem.ToString() != "" && ddlnobatches.SelectedItem.ToString() != "--Select--")
                            {
                                batch_spread.Sheets[0].Cells[rowcount - 1, 0].Value = true;
                                //added by srinath 31/8/2013
                                Btnsave.Enabled = true;
                                Btndelete.Enabled = true;
                            }
                            else
                            {
                                lblerror.Visible = true;
                                lblerror.Text = "Please Add No of Batch";
                            }
                        }
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Please Enter Available Student Count";
                    }
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please Enter Greater than Zero";
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Enter Values";
            }
        }
        else
        {
            lblerror.Visible = true;
            lblerror.Text = "Please Select Batch";
        }
        fromno.Text = string.Empty;
        tono.Text = string.Empty;
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        // Panel3.Visible = true;
        int ar = 0;
        int ac = 0;
        string value = string.Empty;
        ar = Batchallot_spread.ActiveSheetView.ActiveRow;
        ac = Batchallot_spread.ActiveSheetView.ActiveColumn;
        if (ac > 1)
        {
            Checkboxlistbatch.Visible = true;
            Button3.Visible = true;
            Fieldset5.Visible = true;
            string batchbb = Batchallot_spread.Sheets[0].Cells[ar, ac].Text;
            string[] batc = batchbb.Split(',');
            if (batc.GetUpperBound(0) > 0)
            {
                for (int uu = 0; uu <= batc.GetUpperBound(0); uu++)
                {
                    string bvv = batc[uu].ToString();
                    for (int i = 0; i < Checkboxlistbatch.Items.Count; i++)
                    {
                        value = Checkboxlistbatch.Items[i].Text;
                        if (bvv == value)
                        {
                            Checkboxlistbatch.Items[i].Selected = true;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < Checkboxlistbatch.Items.Count; i++)
                {
                    value = Checkboxlistbatch.Items[i].Text;
                    if (batchbb == value)
                    {
                        Checkboxlistbatch.Items[i].Selected = true;
                    }
                    else
                    {
                        Checkboxlistbatch.Items[i].Selected = false;
                    }
                }
            }
        }
    }

    protected void Checkboxlistbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        string value = string.Empty;
        string code = string.Empty;
        for (int i = 0; i < Checkboxlistbatch.Items.Count; i++)
        {
            if (Checkboxlistbatch.Items[i].Selected == true)
            {
                value = Checkboxlistbatch.Items[i].Text;
                code = Checkboxlistbatch.Items[i].Value.ToString();
            }
        }
    }

    protected void Batchallot_spread_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        chk = true;
    }

    protected void Batchallot_spread_SelectedIndexChanged(Object sender, EventArgs e)
    {
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        string value = string.Empty;
        string code = string.Empty;
        //string[] strcomo = new string[20];
        //int j = 0;
        Batchallot_spread.SaveChanges();
        for (int i = 0; i < Checkboxlistbatch.Items.Count; i++)
        {
            if (Checkboxlistbatch.Items[i].Selected == true)
            {
                value = Checkboxlistbatch.Items[i].Text;
                code = Checkboxlistbatch.Items[i].Value.ToString();
                if (batchva == "")
                {
                    batchva = value;
                }
                else
                {
                    batchva = batchva + ',' + value;
                }
            }
            //strcomo[j++] = Checkboxlistbatch.Items[i].Text;
        }
        //strcomo[j++] = string.Empty;
        int ar = 0;
        int ac = 0;
        ar = Batchallot_spread.ActiveSheetView.ActiveRow;
        ac = Batchallot_spread.ActiveSheetView.ActiveColumn;
        if (ac > 1)
        {
            if (Batchallot_spread.Sheets[0].Cells[ar, ac].BackColor == Color.CornflowerBlue)
            {
                FarPoint.Web.Spread.TextCellType btva = new FarPoint.Web.Spread.TextCellType();
                Batchallot_spread.Sheets[0].Cells[ar, ac].CellType = btva;
                Batchallot_spread.Sheets[0].Cells[ar, ac].Text = batchva;
                Batchallot_spread.Sheets[0].Cells[ar, ac].Locked = true;
                Checkboxlistbatch.Visible = false;
            }
        }
        Button3.Visible = false;
        Fieldset5.Visible = false;
        //  Batchallot_spread.SaveChanges();
        //   Batchallot_spread.Sheets[0].AutoPostBack = true;
    }

    protected void batch_spread_UpdateCommand1(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            if (txtbatch.Text != "" && txtbatch.Text != "0")
            {
                if (ddlnobatches.Text != "Select" && ddlnobatches.Text != "-1")
                {
                    string actrow = e.CommandArgument.ToString();
                    bool actflag = false;
                    if (actrow != "-1")
                    {
                        for (int i = 0; i < batch_spread.Sheets[0].RowCount; i++)
                        {
                            int isval = 0;
                            isval = Convert.ToInt32(batch_spread.ActiveSheetView.Cells[i, 0].Value);
                            if (isval == 1)
                            {
                                actflag = true;
                                i = batch_spread.Sheets[0].RowCount;
                            }
                        }
                        string val = e.EditValues[0].ToString();
                        if (val.Trim().ToLower() == "true")
                        {
                            actflag = true;
                        }
                        if (actflag == true)
                        {
                            Btnsave.Enabled = true;
                            Btndelete.Enabled = true;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void Batchallot_spread_UpdateCommand1(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
    }

    public string Getdayorder(string strday_value)
    {
        dayorder = string.Empty;
        if (strday_value == "Mon")
        {
            dayorder = "Day 1";
        }
        else if (strday_value == "Tue")
        {
            dayorder = "Day 2";
        }
        else if (strday_value == "Wed")
        {
            dayorder = "Day 3";
        }
        else if (strday_value == "Thu")
        {
            dayorder = "Day 4";
        }
        else if (strday_value == "Fri")
        {
            dayorder = "Day 5";
        }
        else if (strday_value == "Sat")
        {
            dayorder = "Day 6";
        }
        else if (strday_value == "Sun")
        {
            dayorder = "Day 7";
        }
        return dayorder;
    }

    public void loadautoswich()
    {
        try
        {
            ddlcollegevalue = ((DropDownList)this.user_control.FindControl("ddlcollege")).SelectedValue.ToString();
            ddlbatchvalue = ((DropDownList)this.user_control.FindControl("ddlBatch")).SelectedValue.ToString();
            ddldegreevalue = ((DropDownList)this.user_control.FindControl("ddlDegree")).SelectedValue.ToString();
            ddlsecvalue = ((DropDownList)this.user_control.FindControl("ddlSec")).SelectedValue.ToString();
            ddlsemvalue = ((DropDownList)this.user_control.FindControl("ddlSemYr")).SelectedValue.ToString();
            ddlbranchvalue = ((DropDownList)this.user_control.FindControl("ddlBranch")).SelectedValue.ToString();
            string timetablename = string.Empty;
            if (ddltimetable.Text.ToString().Trim() != "")
            {
                string[] ttname = ddltimetable.SelectedItem.ToString().Split(new Char[] { '@' });
                timetablename = " and timetablename='" + ttname[0] + "'";
            }
            string sections = ddlsecvalue.ToString().Trim();
            if (sections.ToString().Trim().ToLower() == "all" || sections.ToString().Trim().ToLower() == string.Empty || sections.ToString().Trim().ToLower() == "-1")
            {
                sections = string.Empty;
            }
            else
            {
                sections = " and sections='" + sections.ToString().Trim() + "'";
            }
            Hashtable hatautoswitch = new Hashtable();
            string getautoswitchsub = "select distinct Day_Value,Hour_Value from LabAlloc where Batch_Year='" + ddlbatchvalue + "' and Degree_Code='" + ddlbranchvalue + "' and Semester='" + ddlsemvalue + "' " + sections + " and ISNULL(auto_switch,'')<>''  " + timetablename + "";
            DataSet dsautoswitch = obi_access.select_method_wo_parameter(getautoswitchsub, "Text");
            if (dsautoswitch.Tables.Count > 0 && dsautoswitch.Tables[0].Rows.Count > 0)
            {
                for (int af = 0; af < dsautoswitch.Tables[0].Rows.Count; af++)
                {
                    string setval = dsautoswitch.Tables[0].Rows[af]["Day_Value"].ToString() + '/' + dsautoswitch.Tables[0].Rows[af]["Hour_Value"].ToString();
                    if (!hatautoswitch.Contains(setval))
                    {
                        hatautoswitch.Add(setval, setval);
                    }
                }
            }
            Fieldset7.Visible = true;
            chklsautoswitch.Items.Clear();
            txtautoswitch.Text = "---Select---";
            chkswitch.Checked = false;
            bool getswitchlab = false;
            int icou = 0;
            bool automatcilab = false;
            for (int i = 0; i < Batchallot_spread.Sheets[0].RowCount; i++)
            {
                string value = Batchallot_spread.Sheets[0].Cells[i, 0].Text + '/' + Batchallot_spread.Sheets[0].Cells[i, 1].Text;
                string subno = string.Empty;
                getswitchlab = false;
                for (int su = 2; su < Batchallot_spread.Sheets[0].ColumnCount; su++)
                {
                    if (Batchallot_spread.Sheets[0].Cells[i, su].BackColor == Color.CornflowerBlue)
                    {
                        if (subno == "")
                        {
                            subno = Batchallot_spread.Sheets[0].ColumnHeader.Cells[0, su].Tag.ToString();
                        }
                        else
                        {
                            getswitchlab = true;
                            subno = subno + ',' + Batchallot_spread.Sheets[0].ColumnHeader.Cells[0, su].Tag.ToString();
                        }
                    }
                }
                if (getswitchlab == true)
                {
                    automatcilab = true;
                    chklsautoswitch.Items.Insert(icou, new System.Web.UI.WebControls.ListItem(value, subno));
                    if (hatautoswitch.Contains(value))
                    {
                        chklsautoswitch.Items[chklsautoswitch.Items.Count - 1].Selected = true;
                    }
                    else
                    {
                        chklsautoswitch.Items[chklsautoswitch.Items.Count - 1].Selected = false;
                    }
                    icou++;
                }
            }
            if (automatcilab == true)
            {
                Fieldset7.Visible = true;
                int t = 0;
                for (int ch = 0; ch < chklsautoswitch.Items.Count; ch++)
                {
                    if (chklsautoswitch.Items[ch].Selected == true)
                    {
                        t++;
                    }
                }
                if (t > 0)
                {
                    txtautoswitch.Text = "Items (" + t + ")";
                    if (t == chklsautoswitch.Items.Count)
                    {
                        chkswitch.Checked = true;
                    }
                }
            }
            else
            {
                Fieldset7.Visible = false;
                lblerror.Text = "No Items to Automatic Switch Lab";
                lblerror.Visible = true;
                chkautoswitch.Checked = false;
            }
        }
        catch
        {
        }
    }

    protected void chkautoswitch_CheckedChanged(object sender, EventArgs e)
    {
        if (chkautoswitch.Checked == true)
        {
            Fieldset7.Visible = true;
            loadautoswich();
        }
        else
        {
            Fieldset7.Visible = false;
        }
    }

    protected void chkswitch_CheckedChanged(object sender, EventArgs e)
    {
        if (chkswitch.Checked == true)
        {
            for (int i = 0; i < chklsautoswitch.Items.Count; i++)
            {
                chklsautoswitch.Items[i].Selected = true;
            }
            txtautoswitch.Text = "Items (" + chklsautoswitch.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsautoswitch.Items.Count; i++)
            {
                chklsautoswitch.Items[i].Selected = false;
            }
            txtautoswitch.Text = "---Select---";
        }
    }

    protected void chklsautoswitch_SelectedIndexChanged(object sender, EventArgs e)
    {
        int coun = 0;
        for (int i = 0; i < chklsautoswitch.Items.Count; i++)
        {
            if (chklsautoswitch.Items[i].Selected == true)
            {
                coun++;
            }
        }
        chkswitch.Checked = false;
        if (coun > 0)
        {
            if (coun == chklsautoswitch.Items.Count)
            {
                chkswitch.Checked = true;
            }
            txtautoswitch.Text = "Items (" + coun + ")";
        }
        else
        {
            txtautoswitch.Text = "---Select---";
        }
    }

    protected void btnautoswitch_Click(object sender, EventArgs e)
    {
        try
        {
            ddlcollegevalue = ((DropDownList)this.user_control.FindControl("ddlcollege")).SelectedValue.ToString();
            ddlbatchvalue = ((DropDownList)this.user_control.FindControl("ddlBatch")).SelectedValue.ToString();
            ddldegreevalue = ((DropDownList)this.user_control.FindControl("ddlDegree")).SelectedValue.ToString();
            ddlsecvalue = ((DropDownList)this.user_control.FindControl("ddlSec")).SelectedValue.ToString();
            ddlsemvalue = ((DropDownList)this.user_control.FindControl("ddlSemYr")).SelectedValue.ToString();
            ddlbranchvalue = ((DropDownList)this.user_control.FindControl("ddlBranch")).SelectedValue.ToString();
            string date1 = ddltimetable.SelectedItem.ToString();
            string[] date_fm = date1.Split(new Char[] { '@' });
            string selecteddate = date_fm[0];
            string sections = ddlsecvalue.ToString().Trim();
            if (sections.ToString().Trim().ToLower() == "all" || sections.ToString().Trim().ToLower() == string.Empty || sections.ToString().Trim().ToLower() == "-1")
            {
                sections = string.Empty;
            }
            else
            {
                sections = " and sections='" + sections.ToString().Trim() + "'";
            }
            bool saveflag = false;
            int set = obi_access.update_method_wo_parameter("update LabAlloc set Auto_Switch='' where Degree_Code='" + ddlbranchvalue.ToString() + "' and Batch_Year='" + ddlbatchvalue.ToString() + "' and Semester='" + ddlsemvalue.ToString() + "' " + sections + " and Timetablename='" + selecteddate + "' ", "Text");
            if (set > 0)
            {
                for (int i = 0; i < chklsautoswitch.Items.Count; i++)
                {
                    string strdayhour = chklsautoswitch.Items[i].Text.ToString();
                    string subno = chklsautoswitch.Items[i].Value.ToString();
                    string[] stg = strdayhour.Split('/');
                    if (stg.GetUpperBound(0) == 1)
                    {
                        if (chklsautoswitch.Items[i].Selected == true)
                        {
                            string[] spsu = subno.Split(',');
                            if (spsu.GetUpperBound(0) > 0)
                            {
                                set = obi_access.update_method_wo_parameter("update LabAlloc set Auto_Switch='" + subno + "' where Degree_Code='" + ddlbranchvalue.ToString() + "' and Batch_Year='" + ddlbatchvalue.ToString() + "' and Semester='" + ddlsemvalue.ToString() + "' " + sections + " and Timetablename='" + selecteddate + "' and Day_Value='" + stg[0].ToString() + "' and Hour_Value='" + stg[1].ToString() + "'", "Text");
                                saveflag = true;
                            }
                        }
                    }
                }
            }
            else
            {
                lblerror.Text = "Please Save Batch Allocation Before Automatic Batch Switch";
                lblerror.Visible = true;
            }
            if (saveflag == false)
            {
                lblerror.Text = "Please Select The Items And Then Proceed.";
                lblerror.Visible = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Automatic Batch Switch is Saved successfully')", true);
            }
        }
        catch
        {
        }
    }

}